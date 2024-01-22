using System;
using SnakeGame.UI.Entities;
using System.Windows.Threading;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;
using System.Linq;
using MachineLearning;
using System.Collections.Generic;
using MachineLearning.Wages;
using MachineLearning.Biases;
using System.Windows.Forms;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows;
using System.Windows.Documents;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using System.Xml.Linq;
using System.IO;

namespace SnakeGame.UI
{
    class GameWorld
    {
        private MainWindow mainWindow;
        public int ElementSize { get; private set; }
        public int ColumnCount { get; private set; }
        public int RowCount { get; private set; }
        public double GameAreaWidth { get; private set; }
        public double GameAreaHeight { get; private set; }
        int count = 0;
        Random _randoTron;
        Network Network;
        public Apple Apple { get; set; }
        public Snake Snake { get; set; }
        DispatcherTimer _gameLoopTimer;
        public bool IsRunning { get; set; }
        public List<(int cols, int rows, MovementDirection initialDirection)> starts { get; set; }

        public GameWorld(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            _randoTron = new Random(DateTime.Now.Millisecond / DateTime.Now.Second + 1);

        }

        public void InitializeGame(int difficulty, int elementSize, Network network)
        {
            ElementSize = 15;
            GameAreaWidth = mainWindow.GameWorld.ActualWidth;
            GameAreaHeight = mainWindow.GameWorld.ActualHeight;
            ColumnCount = (int)GameAreaWidth / ElementSize;
            RowCount = (int)GameAreaHeight / ElementSize;

            starts = new List<(int cols, int rows, MovementDirection initialDirection)>
        {
            (0,0,MovementDirection.Right),
            (0,0,MovementDirection.Down),
            (0,RowCount-1,MovementDirection.Up),
            (0,RowCount-1,MovementDirection.Right),
            (ColumnCount-1,RowCount-1,MovementDirection.Left),
            (ColumnCount-1,RowCount-1,MovementDirection.Up),
            (ColumnCount-1,0,MovementDirection.Left),
            (ColumnCount-1,0,MovementDirection.Down),
        };

            DrawGameWorld();
            InitializeSnake();
            InitializeTimer(difficulty);
            Network = network;
            IsRunning = true;
        }



        private void InitializeTimer(int difficulty)
        {
            var interval = TimeSpan.FromSeconds(0.001).Ticks;
            _gameLoopTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromTicks(interval)
            };
            _gameLoopTimer.Tick += MainGameLoop;
            _gameLoopTimer.Start();
        }
        private void InitializeSnake()
        {
            Snake = new Snake(ElementSize);
            Snake.PositionFirstElement(ColumnCount, RowCount, MovementDirection.Right);
        }

        static int counting = 0;
        private void MainGameLoop(object sender, EventArgs e)
        {

            if (Apple != null)
            {
                var previousdirect = Snake.MovementDirection;

                var inputs = GetInputs();


                var expected = GetNewExpected(inputs.Values.ToList());
                var result = Network.TrainModel(inputs.Values.ToList(), expected);

                var direct = GetDirect(result);

                using (StreamWriter sw = new StreamWriter(@"C:\\Users\\piotr\\Desktop\\lala.txt", true))
                {
                    sw.WriteLine($"");
                    sw.WriteLine($"-----------------------------");


                    foreach (var input in inputs)
                        sw.WriteLine($"{input.Key}:  {input.Value}");
                    sw.WriteLine("aktualna pozycaja " + direct.ToString());
                    sw.WriteLine("poprzednia pozycaja " + previousDirect.ToString());
                }

                UpdateMovementDirection(direct);

                Snake.MoveSnake();
                CheckCollision();
            }


            CreateApple();
            Draw();
        }
        MovementDirection GetDirect(IList<double> output)
        {
            var result = new List<double>();

            switch (Snake.MovementDirection)
            {
                case MovementDirection.Left:
                    if (output[0] == output.Max())
                        return MovementDirection.Down;
                    if (output[1] == output.Max())
                        return MovementDirection.Left;
                    if (output[2] == output.Max())
                        return MovementDirection.Up;
                    break;
                case MovementDirection.Right:
                    if (output[0] == output.Max())
                        return MovementDirection.Up;
                    if (output[1] == output.Max())
                        return MovementDirection.Right;
                    if (output[2] == output.Max())
                        return MovementDirection.Down;
                    break;
                case MovementDirection.Up:
                    if (output[0] == output.Max())
                        return MovementDirection.Left;
                    if (output[1] == output.Max())
                        return MovementDirection.Up;
                    if (output[2] == output.Max())
                        return MovementDirection.Right;
                    break;
                case MovementDirection.Down:
                    if (output[0] == output.Max())
                        return MovementDirection.Right;
                    if (output[1] == output.Max())
                        return MovementDirection.Down;
                    if (output[2] == output.Max())
                        return MovementDirection.Left;
                    break;

            }
            throw new Exception();
        }
        static int FindNearestValue(double value)
        {
            double closestToMinusOne = Math.Abs(value - (-1));
            double closestToZero = Math.Abs(value - 0);
            double closestToOne = Math.Abs(value - 1);

            if (closestToMinusOne <= closestToZero && closestToMinusOne <= closestToOne)
            {
                return -1;
            }
            else if (closestToZero <= closestToMinusOne && closestToZero <= closestToOne)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
        private int OppositeDierectionToInt(MovementDirection direct)
        {
            switch (direct)
            {
                case MovementDirection.Left:
                    return 1;
                case MovementDirection.Right:
                    return 0;
                case MovementDirection.Up:
                    return 3;
                case MovementDirection.Down:
                    return 2;
                default:
                    return 0;
            }
        }
        private double[] GetNormalExpected()
        {
            double[] result = new double[4] { 0, 0, 0, 0 };
            result[(int)Snake.MovementDirection] = 1;

            return result;
        }

        private List<double> GetNewExpected(List<double> inputs)
        {
            List<double> result = new List<double> { 0.5, 0.5, 0.5 };

            if (inputs[3] == 1)
            {
                result[0] = result[0] != 0 ? 1 : 0;
            }
            else if (inputs[4] == 1)
            {
                result[1] = result[1] != 0 ? 1 : 0; ;
            }
            else if (inputs[5] == 1)
            {
                result[2] = result[2] != 0 ? 1 : 0; ;
            }
            if (inputs[1] == 1 && !result.Any(x => x == 1))
            {
                if (inputs[0] != 1 && inputs[2] != 1)
                {
                    Random random = new Random();
                    var exp = random.Next(0, 1);

                    if (exp == 1)
                        result[0] = 1;
                    else
                        result[2] = 1;
                }
                else
                {
                    if (inputs[0] == 1)
                        result[2] = 1;
                    else
                        result[0] = 1;
                }
            }
            else if (!result.Any(x => x == 1))
            {
                if (result[1] != 0)
                    result[1] = 1;
                else if (result[1] != 0 && result[0] != 0)
                    result[0] = 1;
                else
                    result[2] = 1;
            }


            for (int i = 0; i < result.Count(); i++)
                result[i] = result[i] == 0.5 ? 0 : result[i];

            return result;
        }

        private void makeChange(double[] result)
        {
            if (Snake.MovementDirection == MovementDirection.Up || Snake.MovementDirection == MovementDirection.Down)
            {
                var left = DistansToWall(Direction.Left);
                var right = DistansToWall(Direction.Right);

                if (left > right)
                    result[1] = 10;
                else
                    result[0] = 10;
            }
            else
            {
                var up = DistansToWall(Direction.Up);
                var down = DistansToWall(Direction.Down);

                if (up > down)
                    result[3] = 10;
                else
                    result[2] = 10;
            }

        }

        private double[] GetExpected()
        {
            double[] result = new double[4] { 0.5, 0.5, 0.5, 0.5 };

            result[OppositeDierectionToInt(Snake.MovementDirection)] = 0;



            if (result[0] != 0 && DistansToApple(Direction.Left) > 0.02)
                result[0] = 1;
            else if (result[1] != 0 && DistansToApple(Direction.Right) > 0.02)
                result[1] = 1;
            else if (result[2] != 0 && DistansToApple(Direction.Up) > 0.02)
                result[2] = 1;
            else if (result[3] != 0 && DistansToApple(Direction.Down) > 0.02)
                result[3] = 1;


            bool isToWall = false;

            bool a = CollisionWithWorldBounds();

            if (!result.Any(x => x == 1) && DistansToWall(Direction.Left) > 0.90)
            {
                result[0] = 0;
                result[1] = result[1] != 0 ? 1 : 0;
                isToWall = true;
            }
            if (!result.Any(x => x == 1) && DistansToWall(Direction.Right) > 0.90)
            {
                result[1] = 0;
                result[0] = result[0] != 0 ? 1 : 0;
                isToWall = true;
            }
            if (!result.Any(x => x == 1) && DistansToWall(Direction.Up) > 0.90)
            {
                result[2] = 0;
                result[3] = result[3] != 0 ? 1 : 0;
                isToWall = true;
            }
            if (!result.Any(x => x == 1) && DistansToWall(Direction.Down) > 0.90)
            {
                result[3] = 0;
                result[2] = result[2] != 0 ? 1 : 0;
                isToWall = true;
            }

            //if (isToWall && count <= 30000)
            //{
            //    for (int i = 0; i < result.Length; i++)
            //        if (result[i] == 0.5)
            //            result[i] = 1;

            //    return result;
            //}

            if (DistansToTail(Direction.Left) > 0.95)
                result[0] = 0;
            if (DistansToTail(Direction.Right) > 0.95)
                result[1] = 0;
            if (DistansToTail(Direction.Up) > 0.95)
                result[2] = 0;
            if (DistansToTail(Direction.Down) > 0.95)
                result[3] = 0;

            if (!result.Any(x => x == 1))
            {
                List<int> adres = new List<int>();

                for (int i = 0; i < 4; i++)
                {
                    if (result[i] == 0.5)
                        adres.Add(i);
                }
                Random rand = new Random();

                result[adres[rand.Next(0, adres.Count)]] = 1;
            }
            for (int i = 0; i < result.Length; i++)
                if (result[i] != 1)
                    result[i] = 0;

            return result;
        }
        double previousDirect = 0;


        private void addLeftInput(bool left, bool forward, bool right, Dictionary<string, double> result)
        {
            if (left && right)
            {
                result.Add("jablko z lewej", 0);
                result.Add("jablko z naprzod", 1);
                result.Add("jablko z prawej", 0);
            }
            else if(left)
            {
                result.Add("jablko z lewej", 0);
                result.Add("jablko z naprzod", 0);
                result.Add("jablko z prawej", 1);
            }
            else
            {
                result.Add("jablko z lewej", 1);
                result.Add("jablko z naprzod", 0);
                result.Add("jablko z prawej", 0);

            }
        }
        private void addStraightInput(bool left, bool forward, bool right, Dictionary<string, double> result)
        {
            if (left && forward)
            {
                result.Add("jablko z lewej", 0);
                result.Add("jablko z naprzod", 0);
                result.Add("jablko z prawej", 1);
            }
            else if(right && forward)
            {
                result.Add("jablko z lewej", 1);
                result.Add("jablko z naprzod", 0);
                result.Add("jablko z prawej", 0);
            }
            else if (forward)
            {
                Random rand = new Random();
                var x = rand.Next();

                result.Add("jablko z lewej", x > 0.5 ? 1 : 0);
                result.Add("jablko z naprzod", 0);
                result.Add("jablko z prawej", x > 0.5 ? 0 : 1);

            }
            else
            {
                result.Add("jablko z lewej", 0);
                result.Add("jablko z naprzod", 1);
                result.Add("jablko z prawej", 0);

            }
        }

        private void addLRightInput(bool left, bool forward, bool right, Dictionary<string, double> result)
        {
            if (left && right)
            {
                result.Add("jablko z lewej", 0);
                result.Add("jablko z naprzod", 1);
                result.Add("jablko z prawej", 0);
            }
            else if (right)
            {
                result.Add("jablko z lewej", 1);
                result.Add("jablko z naprzod", 0);
                result.Add("jablko z prawej", 0);
            }
            else
            {
                result.Add("jablko z lewej", 0);
                result.Add("jablko z naprzod", 0);
                result.Add("jablko z prawej", 1);

            }
        }

        private Dictionary<string, double> GetInputs()
        {
            var result = new Dictionary<string, double>();
            double radians = Math.Atan2(Snake.Head.Y - Apple.Y, Snake.Head.X - Apple.X);
            double degrees = radians * (180 / Math.PI) + 180;
            var snakeHead = Snake.Head;
            var up = Snake.Elements.Where(x => !x.IsHead).Any(x => x.Y + ElementSize == snakeHead.Y && x.X == snakeHead.X);
            var left = Snake.Elements.Where(x => !x.IsHead).Any(x => x.X + ElementSize == snakeHead.X && x.Y == snakeHead.Y);
            var right = Snake.Elements.Where(x => !x.IsHead).Any(x => x.X - ElementSize == snakeHead.X && x.Y == snakeHead.Y);
            var down = Snake.Elements.Where(x => !x.IsHead).Any(x => x.Y - ElementSize == snakeHead.Y && x.X == snakeHead.X);

            switch (Snake.MovementDirection)
            {
                case MovementDirection.Left:
                    result.Add("Dystans z lewej do sciany", DistansToWall(Direction.Down));
                    result.Add("Dystans z naprzod do sciany", DistansToWall(Direction.Left));
                    result.Add("Dystans z prawej do sciany", DistansToWall(Direction.Up));
                    if (degrees >= 0 && degrees < 180)
                    {
                        addLeftInput(down, left, up, result);
                    }
                    else if (degrees == 180)
                    {
                        addStraightInput(down, left, up,result);
                    }
                    else if (degrees > 180 && degrees <= 360)
                    {
                        addLRightInput(down, left, up, result);
                    }

                    break;
                case MovementDirection.Right:
                    result.Add("Dystans z lewej do sciany", DistansToWall(Direction.Up));
                    result.Add("Dystans z naprzod do sciany", DistansToWall(Direction.Right));
                    result.Add("Dystans z prawej do sciany", DistansToWall(Direction.Down));
                    if (degrees >= 180 && degrees < 360)
                    {
                        addLeftInput(up, right,down, result);
                    }
                    else if (degrees == 360 || degrees == 0)
                    {
                        addStraightInput(up, right, down, result);
                    }
                    else if (degrees > 0 && degrees <= 180)
                    {
                        addLRightInput(up, right, down, result);

                    }

                    break;
                case MovementDirection.Up:
                    result.Add("Dystans z lewej do sciany", DistansToWall(Direction.Left));
                    result.Add("Dystans z naprzod do sciany", DistansToWall(Direction.Up));
                    result.Add("Dystans z prawej do sciany", DistansToWall(Direction.Right));
                    if (degrees >= 90 && degrees < 270)
                    {
                        addLeftInput(left, up,right, result);
                      
                    }
                    else if (degrees == 270)
                    {
                        addStraightInput(left, up, right, result);

                    }
                    else if ((degrees > 270 && degrees <= 360) || (degrees <= 90 && degrees > 0))
                    {
                        addLRightInput(left, up, right, result);

                    }
                    break;
                case MovementDirection.Down:
                    result.Add("Dystans z lewej do sciany", DistansToWall(Direction.Right));
                    result.Add("Dystans z naprzod do sciany", DistansToWall(Direction.Down));
                    result.Add("Dystans z prawej do sciany", DistansToWall(Direction.Left));
                    if ((degrees >= 270 && degrees <= 360) || (degrees < 90 && degrees > 0))
                    {
                        addLeftInput(right, down,left, result);
                    }
                    else if (degrees == 90)
                    {
                        addStraightInput(right, down, left, result);

                    }
                    else if (degrees > 90 && degrees <= 270)
                    {
                        addLRightInput(right, down,left, result);

                    }

                    break;
                default:
                    break;
            }


            return result;
        }
        private void DistansToSnake()
        {



        }

        private double DistansToWall(Direction direct)
        {

            var snakeHead = Snake.Head;

            switch (direct)
            {
                case Direction.Left:
                    return snakeHead.X - ElementSize < 0 ? 1 : 0;
                case Direction.Right:
                    return snakeHead.X > GameAreaWidth - 2 * ElementSize ? 1 : 0;
                case Direction.Up:
                    return snakeHead.Y - ElementSize < 0 ? 1 : 0;
                case Direction.Down:
                    return snakeHead.Y > GameAreaHeight - 2 * ElementSize ? 1 : 0;
                case Direction.LeftUp:
                    return Math.Sqrt(Math.Pow((GameAreaWidth - ElementSize - Snake.Head.X) / (GameAreaWidth - ElementSize), 2) + Math.Pow((GameAreaHeight - ElementSize - Snake.Head.Y) / (GameAreaHeight - ElementSize), 2));
                case Direction.RightUp:
                    return Math.Sqrt(Math.Pow(Snake.Head.X / (GameAreaWidth - ElementSize), 2) + Math.Pow((GameAreaHeight - ElementSize - Snake.Head.Y) / (GameAreaHeight - ElementSize), 2));
                case Direction.RightDown:
                    return Math.Sqrt(Math.Pow(Snake.Head.X / (GameAreaWidth - ElementSize), 2) + Math.Pow(Snake.Head.Y / (GameAreaHeight - ElementSize), 2));
                case Direction.LeftDown:
                    return Math.Sqrt(Math.Pow((GameAreaWidth - ElementSize - Snake.Head.X) / (GameAreaWidth - ElementSize), 2) + Math.Pow(Snake.Head.Y / (GameAreaHeight - ElementSize), 2));

            }
            return 0.01;
        }

        private double DistansToTail(Direction direct)
        {
            switch (direct)
            {
                case Direction.Left:
                    var leftTail = Snake.Body
                        .Where(x => Snake.Head.Y == x.Y && Snake.Head.X >= x.X)?
                        .Select(x => Math.Abs(Snake.Head.X - x.X));

                    if (leftTail != null && leftTail.Any())
                        return leftTail.Min() / (GameAreaWidth - ElementSize);
                    break;
                case Direction.Right:
                    var rightTail = Snake.Body
                        .Where(x => Snake.Head.Y == x.Y && Snake.Head.X <= x.X)?
                        .Select(x => Math.Abs(Snake.Head.X - x.X));

                    if (rightTail != null && rightTail.Any())
                        return rightTail.Min() / (GameAreaWidth - ElementSize);
                    break;

                case Direction.Up:
                    var upTail = Snake.Body
                        .Where(x => Snake.Head.X == x.X && Snake.Head.Y >= x.Y)?
                        .Select(x => Math.Abs(Snake.Head.Y - x.Y));

                    if (upTail != null && upTail.Any())
                        return upTail.Min() / (GameAreaHeight - ElementSize);
                    break;

                case Direction.Down:
                    var downTail = Snake.Body
                        .Where(x => Snake.Head.X == x.X && Snake.Head.Y <= x.Y)?
                        .Select(x => Math.Abs(Snake.Head.Y - x.Y));

                    if (downTail != null && downTail.Any())
                        return downTail.Min() / (GameAreaHeight - ElementSize);
                    break;
                case Direction.LeftUp:
                    var leftUpTail = Snake.Body
                        .Where(x => Snake.Head.X >= x.X && Snake.Head.Y >= x.Y)?
                        .Select(x => Math.Min(Math.Abs(Snake.Head.X - x.X), Math.Abs(Snake.Head.Y - x.Y)));

                    if (leftUpTail != null && leftUpTail.Any())
                        return leftUpTail.Min() / (Math.Min(GameAreaWidth - ElementSize, GameAreaHeight - ElementSize));
                    break;
                case Direction.RightUp:
                    var rightUpTail = Snake.Body
                        .Where(x => Snake.Head.X <= x.X && Snake.Head.Y >= x.Y)?
                        .Select(x => Math.Min(Math.Abs(Snake.Head.X - x.X), Math.Abs(Snake.Head.Y - x.Y)));

                    if (rightUpTail != null && rightUpTail.Any())
                        return rightUpTail.Min() / (Math.Min(GameAreaWidth - ElementSize, GameAreaHeight - ElementSize));
                    break;
                case Direction.RightDown:
                    var rightDownTail = Snake.Body
                        .Where(x => Snake.Head.X <= x.X && Snake.Head.Y <= x.Y)?
                        .Select(x => Math.Min(Math.Abs(Snake.Head.X - x.X), Math.Abs(Snake.Head.Y - x.Y)));

                    if (rightDownTail != null && rightDownTail.Any())
                        return rightDownTail.Min() / (Math.Min(GameAreaWidth - ElementSize, GameAreaHeight - ElementSize));
                    break;
                case Direction.LeftDown:
                    var leftDownTail = Snake.Body
                        .Where(x => Snake.Head.X >= x.X && Snake.Head.Y <= x.Y)?
                        .Select(x => Math.Min(Math.Abs(Snake.Head.X - x.X), Math.Abs(Snake.Head.Y - x.Y)));

                    if (leftDownTail != null && leftDownTail.Any())
                        return leftDownTail.Min() / (Math.Min(GameAreaWidth - ElementSize, GameAreaHeight - ElementSize));
                    break;
                default:
                    return 0;
            }

            return 0;
        }

        private double DistansToApple(Direction direct)
        {
            switch (direct)
            {
                case Direction.Left:
                    if (Snake.Head.Y == Apple.Y && Snake.Head.X >= Apple.X)
                        return 1 - Math.Abs(Snake.Head.X - Apple.X) / (GameAreaWidth - ElementSize);
                    break;
                case Direction.Right:
                    if (Snake.Head.Y == Apple.Y && Snake.Head.X <= Apple.X)
                        return 1 - Math.Abs(Snake.Head.X - Apple.X) / (GameAreaWidth - ElementSize);
                    break;
                case Direction.Up:
                    if (Snake.Head.X == Apple.X && Snake.Head.Y >= Apple.Y)
                        return 1 - Math.Abs(Snake.Head.Y - Apple.Y) / (GameAreaHeight - ElementSize);
                    break;
                case Direction.Down:
                    if (Snake.Head.X == Apple.X && Snake.Head.Y <= Apple.Y)
                        return 1 - Math.Abs(Snake.Head.Y - Apple.Y) / (GameAreaHeight - ElementSize);
                    break;
                case Direction.LeftUp:
                    if (Snake.Head.X >= Apple.X && Snake.Head.Y >= Apple.Y)
                        return 1 - Math.Min(Math.Abs(Snake.Head.X - Apple.X), Math.Abs(Snake.Head.Y - Apple.Y)) / (Math.Min(GameAreaWidth - ElementSize, GameAreaHeight - ElementSize));
                    break;
                case Direction.RightUp:
                    if (Snake.Head.X <= Apple.X && Snake.Head.Y >= Apple.Y)
                        return 1 - Math.Min(Math.Abs(Snake.Head.X - Apple.X), Math.Abs(Snake.Head.Y - Apple.Y)) / (Math.Min(GameAreaWidth - ElementSize, GameAreaHeight - ElementSize));
                    break;
                case Direction.RightDown:
                    if (Snake.Head.X <= Apple.X && Snake.Head.Y <= Apple.Y)
                        return 1 - Math.Min(Math.Abs(Snake.Head.X - Apple.X), Math.Abs(Snake.Head.Y - Apple.Y)) / (Math.Min(GameAreaWidth - ElementSize, GameAreaHeight - ElementSize));
                    break;
                case Direction.LeftDown:
                    if (Snake.Head.X >= Apple.X && Snake.Head.Y <= Apple.Y)
                        return 1 - Math.Min(Math.Abs(Snake.Head.X - Apple.X), Math.Abs(Snake.Head.Y - Apple.Y)) / (Math.Min(GameAreaWidth - ElementSize, GameAreaHeight - ElementSize));
                    break;
                default:
                    return 0;
            }

            return 0;
        }

        private void Draw()
        {
            DrawSnake();
            DrawApple();
        }

        public void DrawGameWorld()
        {
            int i = 0;
            for (; i < RowCount; i++)
                mainWindow.GameWorld.Children.Add(GenerateHorizontalWorldLine(i));
            int j = 0;
            for (; j < ColumnCount; j++)
                mainWindow.GameWorld.Children.Add(GenerateVerticalWorldLine(j));
            mainWindow.GameWorld.Children.Add(GenerateVerticalWorldLine(j));
            mainWindow.GameWorld.Children.Add(GenerateHorizontalWorldLine(i));
        }
        private void DrawSnake()
        {
            foreach (var snakeElement in Snake.Elements)
            {
                if (!mainWindow.GameWorld.Children.Contains(snakeElement.UIElement))
                    mainWindow.GameWorld.Children.Add(snakeElement.UIElement);
                Canvas.SetLeft(snakeElement.UIElement, snakeElement.X + 2);
                Canvas.SetTop(snakeElement.UIElement, snakeElement.Y + 2);
            }
        }

        private void DrawApple()
        {
            if (!mainWindow.GameWorld.Children.Contains(Apple.UIElement))
                mainWindow.GameWorld.Children.Add(Apple.UIElement);
            Canvas.SetLeft(Apple.UIElement, Apple.X + 2);
            Canvas.SetTop(Apple.UIElement, Apple.Y + 2);
        }
        private Line GenerateVerticalWorldLine(int j)
        {
            return new Line
            {
                Stroke = Brushes.Black,
                X1 = j * ElementSize,
                Y1 = 0,
                X2 = j * ElementSize,
                Y2 = ElementSize * RowCount
            };
        }

        private Line GenerateHorizontalWorldLine(int i)
        {
            return new Line
            {
                Stroke = Brushes.Black,
                X1 = 0,
                Y1 = i * ElementSize,
                X2 = ElementSize * ColumnCount,
                Y2 = i * ElementSize
            };
        }

        int az = 0;

        private void CheckCollision()
        {
            if (CollisionWithApple())
                ProcessCollisionWithApple();
            if (/*count > 100 ||*/ Snake.CollisionWithSelf() || CollisionWithWorldBounds())
            {
                using (StreamWriter sw = new StreamWriter(@"C:\\Users\\piotr\\Desktop\\lala.txt", true))
                {
                    sw.WriteLine($"porażka");
                    sw.WriteLine($"-----------------------------");
                    sw.WriteLine($"");
                }

                count = 0;
                previousDirect = 0;
                Snake = new Snake(ElementSize);
                var start = starts[az++];
                if (az == 8)
                    az = 0;
                Snake.PositionFirstElement(ColumnCount / 2, RowCount / 2, start.initialDirection);
                mainWindow.GameWorld.Children.Clear();
                if (!(Apple == null || Snake == null || Snake.Head == null))
                    mainWindow.GameWorld.Children.Remove(Apple.UIElement);
                Apple = null;

                DrawGameWorld();
            }
        }

        private bool CollisionWithApple()
        {
            if (Apple == null || Snake == null || Snake.Head == null)
                return false;
            SnakeElement head = Snake.Head;
            return (head.X == Apple.X && head.Y == Apple.Y);
        }

        private void ProcessCollisionWithApple()
        {
            mainWindow.IncrementScore();
            mainWindow.GameWorld.Children.Remove(Apple.UIElement);
            count = 0;
            Apple = null;
            Snake.Grow();

        }

        private void CreateApple()
        {
            if (Apple != null)
                return;

            Apple = new Apple(ElementSize)
            {
                X = _randoTron.Next(0, ColumnCount) * ElementSize,
                Y = _randoTron.Next(0, RowCount) * ElementSize
            };
        }
        private bool CollisionWithWorldBounds()
        {
            if (Snake == null || Snake.Head == null)
                return false;
            var snakeHead = Snake.Head;
            return (snakeHead.X > GameAreaWidth - ElementSize ||
                snakeHead.Y > GameAreaHeight - ElementSize ||
                snakeHead.X < 0 || snakeHead.Y < 0);
        }

        public void StopGame()
        {
            _gameLoopTimer.Stop();
            _gameLoopTimer.Tick -= MainGameLoop;
            IsRunning = false;
        }

        private void IncreaseGameSpeed()
        {
            var part = _gameLoopTimer.Interval.Ticks / 10;
            _gameLoopTimer.Interval = TimeSpan.FromTicks(_gameLoopTimer.Interval.Ticks - part);
        }

        public void PauseGame()
        {
            _gameLoopTimer.Stop();
            IsRunning = false;
        }
        public void ContinueGame()
        {
            _gameLoopTimer.Start();
            IsRunning = true;
        }

        internal void UpdateMovementDirection(MovementDirection movementDirection)
        {
            if (Snake != null)
                Snake.UpdateMovementDirection(movementDirection);
        }
    }

}
