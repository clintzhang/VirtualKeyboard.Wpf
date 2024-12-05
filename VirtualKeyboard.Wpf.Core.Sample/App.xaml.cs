using System.Windows;
using System.Windows.Controls;
using VirtualKeyboard.Wpf.Core;

namespace VirtualKeyboard.Wpf.Core.Sample {
    /// <summary>
    ///     Logika interakcji dla klasy App.xaml
    /// </summary>
    public partial class App : Application {
        public App() {
            VKeyboard.Config(typeof(DefaultKeyboardHost));
            VKeyboard.Listen<TextBox>(e => e.Text);
        }
    }
}