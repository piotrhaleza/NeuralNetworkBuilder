using GalaSoft.MvvmLight.Messaging;
using SnakeGame.Messages;
using SnakeGame.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SnakeGame.ViewModels
{
    internal class MainViewModel
    {

        private Canvas _GameField;

        public Canvas GameField
        {
            get { return _GameField; }
            set { _GameField = value; }
        }


        public List<ObjectInGame> objectsInGame;

        public Random randomNumber;

        public bool IncorrectFoodPosition;

        public bool GameOver;

        Thread thread1;

        void UpdateGameField()
        {
            GameField.Children.Clear();

            foreach(ObjectInGame obj in objectsInGame)
            {
                if(obj.Type == "SnakeBody")
                {
                    Canvas.SetLeft(obj.rectangle, obj.x);
                    Canvas.SetTop(obj.rectangle, obj.y);

                    GameField.Children.Add(obj.rectangle);
                }
                
                if(obj.Type == "Food")
                {
                    Canvas.SetLeft(obj.ellipse, obj.x);
                    Canvas.SetTop(obj.ellipse, obj.y);

                    GameField.Children.Add(obj.ellipse);
                }
            }
        }

        void MoveSnake()
        {
            for(int i = objectsInGame.Count - 1; i > 1; i--)
            {
                objectsInGame[i].x = objectsInGame[i - 1].x;
                objectsInGame[i].y = objectsInGame[i - 1].y;
                objectsInGame[i].Direction = objectsInGame[i - 1].Direction;
            }

            switch(objectsInGame[1].Direction)
            {
                case "Right":
                    objectsInGame[1].x += 10;
                    break;

                case "Left":
                    objectsInGame[1].x -= 10;
                    break;

                case "Up":
                    objectsInGame[1].y -= 10;
                    break;

                case "Down":
                    objectsInGame[1].y += 10;
                    break;
            }
        }

        void ChangeSnakeDirection(KeyPressedMessage obj)
        {
            if (obj.keyEventArgs.Key == Key.Up && objectsInGame[1].Direction != "Down")
                objectsInGame[1].Direction = "Up";

            if (obj.keyEventArgs.Key == Key.Down && objectsInGame[1].Direction != "Up")
                objectsInGame[1].Direction = "Down";

            if (obj.keyEventArgs.Key == Key.Right && objectsInGame[1].Direction != "Left")
                objectsInGame[1].Direction = "Right";

            if (obj.keyEventArgs.Key == Key.Left && objectsInGame[1].Direction != "Right")
                objectsInGame[1].Direction = "Left";
        }

        void IncreaseSnakeLength()
        {
            string direction = objectsInGame[1].Direction;

            if (objectsInGame[1].x == objectsInGame[0].x && objectsInGame[1].y == objectsInGame[0].y)
            {
                objectsInGame.Insert(1, new ObjectInGame("SnakeBody", direction, objectsInGame[0].x, objectsInGame[0].y));

                GenerateNewFoodPosition();
            }
        }

        void GenerateNewFoodPosition()
        {
            int x = 1;
            int y = 1;

            while(IncorrectFoodPosition)
            {
                x = randomNumber.Next(1, 27) * 10;
                y = randomNumber.Next(1, 27) * 10;

                IncorrectFoodPosition = false;

                for(int i = 1; i < objectsInGame.Count; i++)
                {
                    if (objectsInGame[i].x == x && objectsInGame[i].y == y)
                        IncorrectFoodPosition = true;
                }
            }

            objectsInGame[0].x = x;
            objectsInGame[0].y = y;

            IncorrectFoodPosition = true;
        }

        void CheckSnakeCollition()
        {
            for(int i = 2; i < objectsInGame.Count; i++)
            {
                if (objectsInGame[1].x == objectsInGame[i].x && objectsInGame[1].y == objectsInGame[i].y)
                    GameOver = true;
            }

            if (objectsInGame[1].x > 270 || objectsInGame[1].x < 1 || objectsInGame[1].y > 280 || objectsInGame[1].y < 1)
                GameOver = true;
        }

        void RunTheGame()
        {
            while(!GameOver)
            {
                GameField.Dispatcher.Invoke(() =>
                {
                    CheckSnakeCollition();
                    IncreaseSnakeLength();
                    MoveSnake();
                    UpdateGameField();
                });

                Thread.Sleep(100);
            }

            GameField.Dispatcher.Invoke(() => 
            {
                MessageBox.Show("Game Over");
                Messenger.Default.Send(new CloseMainWindowMessage());
            });
        }

        public MainViewModel()
        {
            Messenger.Default.Register<KeyPressedMessage>(this, ChangeSnakeDirection);

            GameField = new Canvas();

            IncorrectFoodPosition = true;
            GameOver = false;

            randomNumber = new Random();

            objectsInGame = new List<ObjectInGame>();

            objectsInGame.Add(new ObjectInGame("Food", "Null", 50, 200));
            objectsInGame.Add(new ObjectInGame("SnakeBody", "Right", 100, 10));
            objectsInGame.Add(new ObjectInGame("SnakeBody", "Right", 90, 10));
            objectsInGame.Add(new ObjectInGame("SnakeBody", "Right", 80, 10));

            UpdateGameField();

            thread1 = new Thread(() => RunTheGame());

            thread1.Start();
        }
    }
}
