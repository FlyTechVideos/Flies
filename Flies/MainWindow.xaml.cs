using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Flies
{
    public partial class MainWindow : Window
    {

        private Random random = new Random();
        private int flyCount = 1;
        private int delay = 0;

        public MainWindow(int flyCount, int delay)
        {
            this.flyCount = flyCount;
            this.delay = delay;

            Closed += Window_Close;
            InitializeComponent();
        }

        private void Window_Close(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Thread t = new Thread(delegate ()
            {
                Thread.Sleep(1000 * delay);

                Dispatcher.BeginInvoke((Action)(() =>
                {
                    for (int i = 0; i < flyCount; i++)
                    {
                        spawnFly();
                    }
                }));
            });

            t.Start();
        }

        private void spawnFly()
        {
            var canvasWidth = SystemParameters.PrimaryScreenWidth;
            var canvasHeight = SystemParameters.PrimaryScreenHeight;

            var imageWidth = 60;
            var imageHeight = 40;

            Fly fly = new Fly(random, FlowDirection.LeftToRight, canvasWidth, canvasHeight, imageWidth, imageHeight);

            Image MovingImage = new Image();
            MovingImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Fly.png", UriKind.Absolute));

            MovingImage.Width = 60;
            MovingImage.Height = 40;

            FullscreenCanvas.Children.Add(MovingImage);

            Thread t = new Thread(delegate ()
            {
                while (true)
                {
                    fly.move();

                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        Canvas.SetLeft(MovingImage, fly.CurrentX);
                        Canvas.SetTop(MovingImage, fly.CurrentY);

                        MovingImage.FlowDirection = fly.FlowDirection;
                        MovingImage.RenderTransform = new RotateTransform(fly.CurrentRotationAngle);
                    }));

                    Thread.Sleep(1000 / 30);
                }
            });

            t.Start();
        }
    }
}
