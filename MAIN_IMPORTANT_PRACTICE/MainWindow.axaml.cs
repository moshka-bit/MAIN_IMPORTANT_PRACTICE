using Avalonia.Controls;

namespace MAIN_IMPORTANT_PRACTICE
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var parent = this.VisualRoot as Window;
            var log_in = new LogIn();
            await log_in.ShowDialog(parent);
        }
    }
}