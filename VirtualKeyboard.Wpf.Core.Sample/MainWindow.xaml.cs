using System.Windows;

namespace VirtualKeyboard.Wpf.Core.Sample {
    /// <summary>
    ///     Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e) {
            Label.Content = await VKeyboard.OpenAsync();
        }
    }
}