using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Projekt_FlappyBird_HenkovaIT2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            gameTimer.Tick += MainEventTimer;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            StartGame();
        }

       

        DispatcherTimer gameTimer = new DispatcherTimer();
        Rect flappyBirdHitBox;

        private void MainEventTimer(object? sender, EventArgs e)
        {
            Score_Label.Content = "Skóre: " + score;
            flappyBirdHitBox = new Rect(Canvas.GetLeft(Flappy_Bird_Image), Canvas.GetTop(Flappy_Bird_Image), Flappy_Bird_Image.Width, Flappy_Bird_Image.Height);
            Canvas.SetTop(Flappy_Bird_Image, Canvas.GetTop(Flappy_Bird_Image) + gravity);

            if(Canvas.GetTop(Flappy_Bird_Image) < -10 || Canvas.GetTop(Flappy_Bird_Image) > 460)
                EndGame();

            foreach(var x in My_Canvas.Children.OfType<Image>())
            {
                if((string)x.Tag == "Obstracle1" || (string)x.Tag == "Obstracle2" || (string)x.Tag == "Obstracle3")
                {
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - 5);

                    if (Canvas.GetLeft(x) < -100)
                    {
                        Canvas.SetLeft(x, 800);
                        score += 0.5;
                    }
                    Rect pipeHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    if (flappyBirdHitBox.IntersectsWith(pipeHitBox))
                        EndGame();
                }

                if ((string)x.Tag == "Cloud")
                {
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - 2);

                    if (Canvas.GetLeft(x) < -250)
                        Canvas.SetLeft(x, 550);
                }
            }
        
        }


        double score;
        int gravity = 8;
        bool GameOver;


        private void My_Canvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                Flappy_Bird_Image.RenderTransform = new RotateTransform(-20, Flappy_Bird_Image.Width / 2, Flappy_Bird_Image.Height / 2);
                gravity = -8;
            }

            if (e.Key == Key.R && GameOver)
                StartGame();
        }

        private void My_Canvas_KeyUp(object sender, KeyEventArgs e)
        {
           
            Flappy_Bird_Image.RenderTransform = new RotateTransform(5, Flappy_Bird_Image.Width / 2, Flappy_Bird_Image.Height / 2);
            gravity = 8;
           
        }

        private void StartGame()
        {
            My_Canvas.Focus();
            score = 0;
            int temp = 300;
            GameOver = false;
            Canvas.SetTop(Flappy_Bird_Image, 190);

            foreach (var x in My_Canvas.Children.OfType<Image>())
            {
                if ((string)x.Tag == "Obstracle1")
                    Canvas.SetLeft(x, 500);

                if ((string)x.Tag == "Obstracle2")
                    Canvas.SetLeft(x, 800);

                if ((string)x.Tag == "Obstracle3")
                    Canvas.SetLeft(x, 1100);

                if ((string)x.Tag == "Cloud")
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
            GameOver = true;
            Score_Label.Content += " Hra skončila! Stiskni R a zkus to znovu :)";
        }
    }
}
