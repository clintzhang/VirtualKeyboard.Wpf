using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;

namespace VirtualKeyboard.Wpf.Core.Controls {
    internal class AdvancedTextBox : TextBox {
        // Using a DependencyProperty as the backing store for CaretPosition.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CaretPositionProperty =
            DependencyProperty.Register("CaretPosition", typeof(int), typeof(AdvancedTextBox),
                new PropertyMetadata(0, OnCaretPositionChange));

        // Using a DependencyProperty as the backing store for SelectedValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedValueProperty =
            DependencyProperty.Register("SelectedValue", typeof(string), typeof(AdvancedTextBox),
                new PropertyMetadata("", OnSelectedTextChange));

        // Using a DependencyProperty as the backing store for TextValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextValueProperty =
            DependencyProperty.Register("TextValue", typeof(string), typeof(AdvancedTextBox),
                new PropertyMetadata("", OnTextValueChange));

        public AdvancedTextBox() {
            SelectionChanged += AdvancedTextBox_SelectionChanged;
            TextChanged += (s, e) => SetValue(TextValueProperty, Text);
        }

        public int CaretPosition {
            get => (int)GetValue(CaretPositionProperty);
            set => SetValue(CaretPositionProperty, value);
        }

        public string SelectedValue {
            get => (string)GetValue(SelectedValueProperty);
            set => SetValue(SelectedValueProperty, value);
        }

        public string TextValue {
            get => (string)GetValue(TextValueProperty);
            set => SetValue(TextValueProperty, value);
        }

        protected override AutomationPeer OnCreateAutomationPeer() {
            return new FrameworkElementAutomationPeer(this);
        }

        private void AdvancedTextBox_SelectionChanged(object sender, RoutedEventArgs e) {
            SetValue(CaretPositionProperty, CaretIndex);
            SetValue(SelectedValueProperty, SelectedText);
        }

        private static void OnCaretPositionChange(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            var value = e.NewValue as int?;
            ((TextBox)sender).CaretIndex = value ?? 0;
        }

        private static void OnSelectedTextChange(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            var value = e.NewValue as string;
            ((TextBox)sender).SelectedText = value ?? "";
        }

        private static void OnTextValueChange(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            var s = (AdvancedTextBox)sender;
            var caretPosition = s.CaretPosition;
            var value = e.NewValue as string;
            s.Text = value;
            s.CaretIndex = caretPosition <= value.Length ? caretPosition : value.Length;
        }
    }
}