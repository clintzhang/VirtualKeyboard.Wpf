using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;

namespace VirtualKeyboard.Wpf.Core.Controls;

internal class AdvancedTextBox : TextBox {
    // Using a DependencyProperty as the backing store for CaretPosition.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CaretPositionProperty =
        DependencyProperty.Register(nameof(CaretPosition), typeof(int), typeof(AdvancedTextBox),
            new PropertyMetadata(0, OnCaretPositionChange));

    // Using a DependencyProperty as the backing store for SelectedValue.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SelectedValueProperty =
        DependencyProperty.Register(nameof(SelectedValue), typeof(string), typeof(AdvancedTextBox),
            new PropertyMetadata("", OnSelectedTextChange));

    // Using a DependencyProperty as the backing store for TextValue.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TextValueProperty =
        DependencyProperty.Register(nameof(TextValue), typeof(string), typeof(AdvancedTextBox),
            new PropertyMetadata("", OnTextValueChange));

    private static bool _isPasted;

    public AdvancedTextBox() {
        SelectionChanged += AdvancedTextBox_SelectionChanged;
        TextChanged += (_, _) => SetValue(TextValueProperty, Text);
        DataObject.AddPastingHandler(this, OnPaste);
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

    private void OnPaste(object sender, DataObjectPastingEventArgs e) {
        var isText = e.SourceDataObject.GetDataPresent(DataFormats.UnicodeText, true);
        _isPasted = isText;
        if (!isText) return;

        _ = e.SourceDataObject.GetData(DataFormats.UnicodeText) as string;
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
        s.Text = value!;
        if (_isPasted) {
            caretPosition = value.Length;
            _isPasted = false;
        }

        s.CaretIndex = caretPosition <= value.Length ? caretPosition : value.Length;
    }
}