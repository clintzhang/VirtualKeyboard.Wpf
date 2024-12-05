using System;
using System.ComponentModel;
using System.Text;
using VirtualKeyboard.Wpf.Core.Types;

namespace VirtualKeyboard.Wpf.Core.ViewModels;

internal class VirtualKeyboardViewModel : INotifyPropertyChanged {
    private readonly bool _isPassword;
    private readonly char _passwordChar;
    private int _caretPosition;
    private string _keyboardPwd;

    private string _keyboardText;
    private KeyboardType _keyboardType;
    private string _selectedValue;
    private bool _uppercase;

    public VirtualKeyboardViewModel(string initialValue, bool isPassword, char passwordChar) {
        _isPassword = isPassword;
        _passwordChar = passwordChar;
        KeyboardText = initialValue;

        _keyboardType = KeyboardType.Alphabet;
        _uppercase = false;
        CaretPosition = KeyboardText.Length;

        AddCharacter = new Command(a => {
            if (a is not string character) return;
            if (character.Length != 1) return;
            if (Uppercase) character = character.ToUpper();
            if (!string.IsNullOrEmpty(SelectedValue)) {
                RemoveSubstring(SelectedValue);
                KeyboardText = KeyboardText.Insert(CaretPosition, character);
                CaretPosition++;
                SelectedValue = "";
            }
            else {
                KeyboardText = KeyboardText.Insert(CaretPosition, character);
                CaretPosition++;
            }
        });
        ChangeCasing = new Command(_ => Uppercase = !Uppercase);
        RemoveCharacter = new Command(_ => {
            if (!string.IsNullOrEmpty(SelectedValue)) {
                RemoveSubstring(SelectedValue);
            }
            else {
                var position = CaretPosition - 1;
                if (position < 0) return;
                KeyboardText = KeyboardText.Remove(position, 1);
                if (position < KeyboardText.Length) CaretPosition--;
                else CaretPosition = KeyboardText.Length;
            }
        });
        ChangeKeyboardType = new Command(_ => {
            KeyboardType = KeyboardType == KeyboardType.Alphabet ? KeyboardType.Special : KeyboardType.Alphabet;
        });
        Accept = new Command(_ => VKeyboard.Close());
    }

    public string KeyboardPwd {
        get => _keyboardPwd;
        set {
            _keyboardPwd = value;
            NotifyPropertyChanged(nameof(KeyboardPwd));
        }
    }

    public string KeyboardText {
        get => _keyboardText;
        set {
            _keyboardText = value;
            NotifyPropertyChanged(nameof(KeyboardText));
            KeyboardPwd = _isPassword
                ? new StringBuilder().Append(_passwordChar, _keyboardText.Length).ToString()
                : _keyboardText;
        }
    }

    public KeyboardType KeyboardType {
        get => _keyboardType;
        private set {
            _keyboardType = value;
            NotifyPropertyChanged(nameof(KeyboardType));
        }
    }

    public bool Uppercase {
        get => _uppercase;
        private set {
            _uppercase = value;
            NotifyPropertyChanged(nameof(Uppercase));
        }
    }

    public int CaretPosition {
        get => _caretPosition;
        set {
            if (value < 0) _caretPosition = 0;
            else if (value > KeyboardText.Length) _caretPosition = KeyboardText.Length;
            else _caretPosition = value;
            NotifyPropertyChanged(nameof(CaretPosition));
        }
    }

    public string SelectedValue {
        get => _selectedValue;
        set {
            _selectedValue = value;
            NotifyPropertyChanged(nameof(SelectedValue));
        }
    }

    public Command AddCharacter { get; }
    public Command ChangeCasing { get; }
    public Command RemoveCharacter { get; }
    public Command ChangeKeyboardType { get; }
    public Command Accept { get; }

    public event PropertyChangedEventHandler PropertyChanged;

    private void RemoveSubstring(string substring) {
        var position = KeyboardText.IndexOf(substring, StringComparison.Ordinal);
        KeyboardText = KeyboardText.Remove(position, substring.Length);
    }

    private void NotifyPropertyChanged(string prop) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}