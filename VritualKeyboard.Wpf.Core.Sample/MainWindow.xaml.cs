using System.Windows;
using VirtualKeyboard.Wpf.Core;

namespace VritualKeyboard.Wpf.Core.Sample {
    /// <summary>
    ///     Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            VKeyboard.Config(typeof(Window1));
        }

        private async void Button_Click(object sender, RoutedEventArgs e) {
            Label.Content = await VKeyboard.OpenAsync();
        }
    }
}