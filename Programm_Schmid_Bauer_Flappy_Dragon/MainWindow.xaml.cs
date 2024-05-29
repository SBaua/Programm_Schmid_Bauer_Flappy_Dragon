using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Programm_Schmid_Bauer_Flappy_Dragon
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        DispatcherTimer gameTimer = new DispatcherTimer();

        double score;
        int gravity = 5;
        bool gameOver;
        Rect flappyBirdHitBox;

        public MainWindow()
        {
            InitializeComponent();

            gameTimer.Tick += MainEventTimer;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            StartGame();

        }
        private void MainEventTimer(object sender, EventArgs e)
        {
            txtScore.Content = "Score: " + score;

            if (score >= 3)
            {
                string s = "Achtung";
               /* this.gravity = 7; */
                txtWarning.Text = s;
                this.txtWarning.Visibility = Visibility.Visible;
            }
            if (score >= 25)
            {
                this.gravity = 10;
            }

            flappyBirdHitBox = new Rect(Canvas.GetLeft(bird_hitbox), Canvas.GetTop(bird_hitbox), bird_hitbox.Width - 12, bird_hitbox.Height);

            Canvas.SetTop(flappyBird, Canvas.GetTop(flappyBird) + gravity);
            Canvas.SetTop(bird_hitbox, Canvas.GetTop(bird_hitbox) + gravity);

            if (Canvas.GetTop(bird_hitbox) < -30 || Canvas.GetTop(bird_hitbox) + bird_hitbox.Height > 460)
            {
                EndGame();
            }
            foreach (var x in MyCanvas.Children.OfType<Image>())
            {
                if ((string)x.Tag == "obs1" || (string)x.Tag == "obs2" || (string)x.Tag == "obs3")
                {
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - 5);

                    if (Canvas.GetLeft(x) < -100)
                    {
                        Canvas.SetLeft(x, 800);

                        score += .5;
                    }
                    Rect PillarHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    if (flappyBirdHitBox.IntersectsWith(PillarHitBox))
                    {
                        EndGame();
                    }
                }
                if ((string)x.Tag == "clouds")
                {
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - 1);

                    if (Canvas.GetLeft(x) < -250)
                    {
                        Canvas.SetLeft(x, 550);

                        score += .5;
                    }
                }
            }
        }
        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                flappyBird.RenderTransform = new RotateTransform(-20, flappyBird.Width / 2, flappyBird.Height / 2);
                
                bird_hitbox.RenderTransform = new RotateTransform(-20, bird_hitbox.Width / 2, bird_hitbox.Height / 2);
                gravity = -5;
            }

            if (e.Key == Key.R && gameOver == true)
            {
                StartGame();
            }
            
        }
        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            flappyBird.RenderTransform = new RotateTransform(5, bird_hitbox.Width / 2, bird_hitbox.Height / 2);

            
            bird_hitbox.RenderTransform = new RotateTransform(5, bird_hitbox.Width / 2, bird_hitbox.Height / 2);

            gravity = 5;
        }
        private void StartGame()
        {
            this.btnBird.Visibility = Visibility.Hidden;
            this.btnGameMode.Visibility = Visibility.Hidden;

            this.pillertop1.Visibility = Visibility.Visible;
            this.pillertop2.Visibility = Visibility.Visible;
            this.pillertop3.Visibility = Visibility.Visible;

            this.pillerbottom1.Visibility = Visibility.Visible;
            this.pillerbottom2.Visibility = Visibility.Visible;
            this.pillerbottom3.Visibility = Visibility.Visible;

            this.flappyBird.Visibility = Visibility.Visible;

            MyCanvas.Focus();

            int temp = 300;

            score = 0;

            gameOver = false;

            Canvas.SetTop(flappyBird, 190);
            Canvas.SetTop(bird_hitbox, 205);

            foreach (var x in MyCanvas.Children.OfType<Image>())
            {        
                if ((string)x.Tag == "obs1")
                {
                    Canvas.SetLeft(x, 500);
                }
                if ((string)x.Tag == "obs2")
                {
                    Canvas.SetLeft(x, 800);
                }
                if ((string)x.Tag == "obs3")
                {
                    Canvas.SetLeft(x, 1100);
                }
                if ((string)x.Tag == "clouds")
                {
                    Canvas.SetLeft(x, 300 + temp);
                    temp = 800;
                }
                
            }
            gameTimer.Start();
            
        }
        private void EndGame()
        {
            gameTimer.Stop();
            gameOver = true;
            txtScore.Content += " Game Over!!!";
            this.btnBird.Visibility = Visibility.Visible;
            this.btnGameMode.Visibility = Visibility.Visible;
            this.pillertop1.Visibility = Visibility.Hidden;
            this.pillertop2.Visibility = Visibility.Hidden;
            this.pillertop3.Visibility = Visibility.Hidden;

            this.pillerbottom1.Visibility = Visibility.Hidden;
            this.pillerbottom2.Visibility = Visibility.Hidden;
            this.pillerbottom3.Visibility = Visibility.Hidden;

            this.txtWarning.Visibility = Visibility.Hidden;

            this.flappyBird.Visibility = Visibility.Hidden;
            var btnBird = new Button { Content = "btnBird" };
            btnBird.Click += btnBird_Click;

        }

        private void btnBird_Click(object sender, RoutedEventArgs e)
        {
           /* flappyBird.Opacity */
        }

        private void btnGameMode_Click(object sender, RoutedEventArgs e)
        {
            StartGame();
        }
    }
}