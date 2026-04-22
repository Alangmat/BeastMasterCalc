using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace View
{
    public partial class SplashWindow : Window
    {
        public SplashWindow()
        {
            InitializeComponent();
            Loaded  += SplashWindow_Loaded;
            MouseDown += (s, e) => { if (e.ChangedButton == MouseButton.Left) FinishSplash(); };
            KeyDown   += (s, e) =>
            {
                if (e.Key == Key.Escape || e.Key == Key.Space || e.Key == Key.Enter)
                    FinishSplash();
            };
        }

        private void SplashWindow_Loaded(object sender, RoutedEventArgs e)
        {
            string videoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "videos", "Calc Logo.mp4");
            if (!File.Exists(videoPath))
            {
                FinishSplash();
                return;
            }
            splashVideo.Source = new Uri(videoPath);
            splashVideo.Play();
        }

        private void SplashVideo_MediaEnded(object sender, RoutedEventArgs e)
        {
            FinishSplash();
        }

        private bool _finished = false;
        private void FinishSplash()
        {
            if (_finished) return;
            _finished = true;
            splashVideo.Stop();
            var main = new MainWindow();
            main.Show();
            Close();
        }
    }
}
