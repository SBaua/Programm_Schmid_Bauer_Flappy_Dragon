using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Programm_Schmid_Bauer_Flappy_Dragon
{
    public partial class MainWindow : Window
    {
        private const int Gravity = 2;
        private const int JumpSpeed = -20;
        private const int PipeSpeed = 5;
        private const int PipeSpacing = 200;
        private const int PipeWidth = 100;

        private int _score = 0;
        private bool _gameOver = false;
        private readonly DispatcherTimer _gameTimer = new DispatcherTimer();
        private readonly List<Pipe> _pipes = new List<Pipe>();
        private int _frameCounter = 0;

        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            _gameTimer.Tick += GameTimer_Tick;
            _gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            _gameTimer.Start();

            GameCanvas.Background = new ImageBrush(new BitmapImage(new Uri("Images/background.png", UriKind.Relative)));

            FlappyBirdImage.Source = new BitmapImage(new Uri("Images/flappy_bird.png", UriKind.Relative));
            Canvas.SetLeft(FlappyBirdImage, 110);
            Canvas.SetTop(FlappyBirdImage, 150);

            ScoreText.Text = "Score: 0";
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (!_gameOver)
            {
                _frameCounter++;

                if (_frameCounter % 80 == 0)
                {
                    AddPipe();
                }

                UpdatePipes();

                FlappyBirdFall();
                CheckCollision();
                UpdateScore();
            }
        }

        private void FlappyBirdFall()
        {
            Canvas.SetTop(FlappyBirdImage, Canvas.GetTop(FlappyBirdImage) + Gravity);
        }

        private void FlappyBirdJump()
        {
            Canvas.SetTop(FlappyBirdImage, Canvas.GetTop(FlappyBirdImage) + JumpSpeed);
        }

        private void AddPipe()
        {
            var topPipe = new Pipe(true);
            var bottomPipe = new Pipe(false);

            GameCanvas.Children.Add(topPipe.Rect);
            GameCanvas.Children.Add(bottomPipe.Rect);

            _pipes.Add(topPipe);
            _pipes.Add(bottomPipe);
        }

        private void UpdatePipes()
        {
            for (int i = 0; i < _pipes.Count; i++)
            {
                _pipes[i].Move(PipeSpeed);

                if (Canvas.GetLeft(_pipes[i].Rect) < -PipeWidth)
                {
                    GameCanvas.Children.Remove(_pipes[i].Rect);
                    _pipes.RemoveAt(i);
                    i--;
                }
            }
        }

       private Rect GetFlappyBirdBounds()
{
    // Berechnen Sie die Grenzen des Flappy Bird-Bildes basierend auf der aktuellen Position und Größe
    return new Rect(Canvas.GetLeft(FlappyBirdImage), Canvas.GetTop(FlappyBirdImage), FlappyBirdImage.ActualWidth, FlappyBirdImage.ActualHeight);
}

private void CheckCollision()
{
    Rect flappyBirdBounds = GetFlappyBirdBounds();

    if (flappyBirdBounds.Top < 0 || flappyBirdBounds.Bottom > GameCanvas.ActualHeight)
    {
        GameOver();
    }

    foreach (var pipe in _pipes)
    {
        if (flappyBirdBounds.IntersectsWith(pipe.Rect.Bounds))
        {
            GameOver();
        }
    }
}


        private void UpdateScore()
        {
            foreach (var pipe in _pipes)
            {
                if (Canvas.GetLeft(FlappyBirdImage) > Canvas.GetLeft(pipe.Rect) + PipeWidth &&
                    (bool)pipe.Rect.GetValue(TagProperty) == false &&
                    pipe.Rect.Visibility == Visibility.Visible)
                {
                    _score++;
                    ScoreText.Content = "Score: " + _score;
                    pipe.Rect.SetValue(TagProperty, true);
                }
            }
        }

        private void GameOver()
        {
            _gameOver = true;
            _gameTimer.Stop();
            MessageBox.Show("Game Over! Your score: " + _score);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space && !_gameOver)
            {
                FlappyBirdJump();
            }

            if (e.Key == Key.Enter && _gameOver)
            {
                RestartGame();
            }
        }

        private void RestartGame()
        {
            _gameOver = false;
            _score = 0;
            ScoreText.Content = "Score: 0";
            _pipes.Clear();
            GameCanvas.Children.Clear();
            InitializeGame();
        }
    }

    public class Pipe
    {
        public Rectangle Rect { get; }

        public Pipe(bool isTop)
        {
            Rect = new Rectangle
            {
                Width = 100,
                Height = 500,
                Fill = Brushes.Green,
                Visibility = Visibility.Visible
            };

            if (isTop)
            {
                Rect.RenderTransform = new RotateTransform(180);
            }

            Canvas.SetTop(Rect, isTop ? -PipeSpacing - Rect.Height : 400 + PipeSpacing);
            Canvas.SetLeft(Rect, 800);
            Rect.SetValue(TagProperty, false);
        }

        public void Move(int speed)
        {
            Canvas.SetLeft(Rect, Canvas.GetLeft(Rect) - speed);
        }
    }
}
