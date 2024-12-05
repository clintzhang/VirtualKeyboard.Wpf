using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using VirtualKeyboard.Wpf.Core.Controls;
using VirtualKeyboard.Wpf.Core.ViewModels;
using VirtualKeyboard.Wpf.Core.Views;

namespace VirtualKeyboard.Wpf.Core {
    public static class VKeyboard {
        private const string KeyboardValueName = "KeyboardValueContent";
        private const string KeyboardName = "KeyboardContent";

        private static Type _hostType = typeof(DefaultKeyboardHost);

        private static TaskCompletionSource<string> _tcs;
        private static Window _windowHost;

        public static void Config(Type hostType) {
            if (hostType.IsSubclassOf(typeof(Window))) _hostType = hostType;
            else throw new ArgumentException();
        }

        public static void Listen<T>(Expression<Func<T, string>> property, bool isPassword = false,
            char passwordChar = '*') where T : UIElement {
            EventManager.RegisterClassHandler(typeof(T), UIElement.PreviewMouseLeftButtonDownEvent,
                (RoutedEventHandler)(async void (s, e) => {
                    try {
                        if (s is AdvancedTextBox) return;
                        if (property.Body is not MemberExpression memberSelectorExpression) return;
                        var prop = memberSelectorExpression.Member as PropertyInfo;
                        if (prop == null) return;
                        var initValue = (string)prop.GetValue(s);
                        var value = await OpenAsync(initValue, isPassword, passwordChar);
                        prop.SetValue(s, value, null);
                    }
                    catch (Exception ex) {
                        Console.WriteLine(ex.Message);
                    }
                }));
        }

        public static Task<string> OpenAsync(string initialValue = "", bool isPassword = false,
            char passwordChar = '*') {
            if (_windowHost != null) throw new InvalidOperationException();

            _tcs = new TaskCompletionSource<string>();
            _windowHost = (Window)Activator.CreateInstance(_hostType);

            if (_windowHost == null) return _tcs.Task;
            _windowHost.DataContext = new VirtualKeyboardViewModel(initialValue, isPassword, passwordChar);
            ((ContentControl)_windowHost.FindName(KeyboardValueName))!.Content = new KeyboardValueView();
            ((ContentControl)_windowHost.FindName(KeyboardName))!.Content = new VirtualKeyboardView();

            _windowHost.Closing += Handler;

            _windowHost.Owner = Application.Current.MainWindow;
            _windowHost.Show();

            return _tcs.Task;

            void Handler(object s, CancelEventArgs a) {
                var result = GetResult();
                ((Window)s).Closing -= Handler;
                _windowHost = null;
                _tcs?.SetResult(result);
                _tcs = null;
            }
        }

        public static void Close() {
            //if (_windowHost == null) throw new InvalidOperationException();

            _windowHost?.Close();
        }

        private static string GetResult() {
            var viewModel = (VirtualKeyboardViewModel)_windowHost.DataContext;
            return viewModel.KeyboardText;
        }
    }
}