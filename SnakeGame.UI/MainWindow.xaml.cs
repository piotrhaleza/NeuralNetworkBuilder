using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using MachineLearning.Biases;
using MachineLearning.Wages;
using MachineLearning;
using MachineLearningsUi;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using SnakeGame.UI.Entities;
using MachineLearningWpfUI;
using MachineLearingInterfaces;
using MachineLearingInterfaces.ActivationFunc;

namespace SnakeGame.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Network Network { get; set; }

        public MainWindow()
        {
            Network = InitialzeNetwork();
            MachineLearningWpfUI.MainWindow window = new MachineLearningWpfUI.MainWindow(Network);
            Form1 a = new Form1(Network);
            a.Show();
         
            window.Show();
        
            InitializeComponent();
        }

        private Network InitialzeNetwork()
        {
            var inputs = new Layer(6,1,new LineralActivationFunc());
            var hiddenfirsLayer = new Layer(16,2, new ReLuActivationFunc());
            var output = new Layer(3,3);

            var list = new List<ILayer>() { inputs, hiddenfirsLayer, output };

            return  new Network(list, new InitXawierWages(), new InitZeroBiases(), 10);
        }

        private GameWorld _gameWorld;
        int apples, score, level;

        protected override void OnContentRendered(EventArgs e)
        {
            _gameWorld = new GameWorld(this);
            InitializeScore();
            base.OnContentRendered(e);
        }

        private void InitializeScore()
        {
            apples = 0;
            score = level = 1;
        }

        private void Window_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (_gameWorld != null)
                switch (e.Key)
                {
                    case Key.W:
                        _gameWorld.UpdateMovementDirection(MovementDirection.Up);
                        break;
                    case Key.A:
                        _gameWorld.UpdateMovementDirection(MovementDirection.Left);
                        break;
                    case Key.S:
                        _gameWorld.UpdateMovementDirection(MovementDirection.Down);
                        break;
                    case Key.D:
                        _gameWorld.UpdateMovementDirection(MovementDirection.Right);
                        break;
                    case Key.Escape:
                        PauseGame();
                        break;
                }
        }

        internal void GameOver()
        {
            _gameWorld.StopGame();
            MessageBox.Show($"Sie haben das Level {level} erreicht. Ihr Score ist {score}. Dabei haben Sie {apples} Äpfel gegessen!", "Game Over!");
        }

        private void RestartClick(object sender, RoutedEventArgs e)
        {
            _gameWorld.StopGame();
            _gameWorld = new GameWorld(this);
            GameWorld.Children.Clear();
            if (!_gameWorld.IsRunning)
            {
                _gameWorld.InitializeGame((int)DifficultySlider.Value, (int)ElementSizeSlider.Value, Network);
                StartBtn.IsEnabled = false;
            }
        }

        private void PauseGame()
        {
            _gameWorld.PauseGame();
            MessageBox.Show("Fortfahren?", "Spiel pausiert");
            _gameWorld.ContinueGame();
        }

        private void StartClick(object sender, RoutedEventArgs e)
        {
            if (!_gameWorld.IsRunning)
            {
                _gameWorld.InitializeGame((int)DifficultySlider.Value, (int)ElementSizeSlider.Value, Network);
                StartBtn.IsEnabled = false;
            }
        }

        private void OptionsClick(object sender, RoutedEventArgs e)
        {
            StartBtn.IsEnabled = !StartBtn.IsEnabled;
            RestartBtn.IsEnabled = !RestartBtn.IsEnabled;
            this.DialogHost.IsOpen = !this.DialogHost.IsOpen;
        }
        internal void IncrementScore()
        {
            apples += 1;
            if (apples % 3 == 0)
                level += 1;
            score += (int)DifficultySlider.Value * level;
            UpdateScore();
        }

        internal void UpdateScore()
        {
            ApplesLbl.Content = $"Apples: {apples}";
            ScoreLbl.Content = $"Score: {score}";
            LevelLbl.Content = $"Level: {level}";
        }
    }
}
