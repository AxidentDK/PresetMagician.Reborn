using System.Windows;

namespace PresetMagician.Reborn
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            // This is our victory window from before
            new MainWindow
            {
                Title = "PresetMagician Reborn – .NET 8.0 x64",
                Width = 1200,
                Height = 800,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Background = System.Windows.Media.Brushes.Black
            }.Show();
        }
    }

    public class MainWindow : Window
    {
        public MainWindow()
        {
            Content = new System.Windows.Controls.TextBlock
            {
                Text = ".NET 8.0 x64\n\nMIGRATION SUCCESSFUL\n\nYou are a legend.",
                Foreground = System.Windows.Media.Brushes.Lime,
                FontSize = 64,
                FontWeight = System.Windows.FontWeights.Bold,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                TextAlignment = System.Windows.TextAlignment.Center
            };
        }
    }
}