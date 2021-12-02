using System.Windows;
using System.Windows.Controls;
using VirtualKeyboard.Wpf.Core;

namespace VritualKeyboard.Wpf.Sample {
    /// <summary>
    ///     Logika interakcji dla klasy App.xaml
    /// </summary>
    public partial class App : Application {
        public App() {
            VKeyboard.Listen<TextBox>(e => e.Text);
        }
    }
}