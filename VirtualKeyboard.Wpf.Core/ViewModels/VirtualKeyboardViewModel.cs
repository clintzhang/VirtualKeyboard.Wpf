using System.ComponentModel;
using System.Text;
using VirtualKeyboard.Wpf.Core.Types;

namespace VirtualKeyboard.Wpf.Core.ViewModels
{
    class VirtualKeyboardViewModel : INotifyPropertyChanged {
        private readonly char _passwordChar;
        private readonly bool _isPassword;
        private string _keyboardPwd;

        public string KeyboardPwd {
            get => _keyboardPwd;
            set {
                _keyboardPwd = value;
                NotifyPropertyChanged(nameof(KeyboardPwd));
            }
        }

        private string _keyboardText;
        public string KeyboardText
        {
            get => _keyboardText;
            set
            {
                _keyboardText = value;
                NotifyPropertyChanged(nameof(KeyboardText));
                KeyboardPwd = _isPassword
                    ? new StringBuilder().Append(_passwordChar, _keyboardText.Length).ToString()
                    : _keyboardText;
            }
        }
        private KeyboardType _keyboardType;
        public KeyboardType KeyboardType
        {
            get => _keyboardType;
            private set
            {
                _keyboardType = value;
                NotifyPropertyChanged(nameof(KeyboardType));
            }
        }
        private bool _uppercase;
        public bool Uppercase
        {
            get => _uppercase;
            private set
            {
                _uppercase = value;
                NotifyPropertyChanged(nameof(Uppercase));
            }
        }
        private int _caretPosition;
        public int CaretPosition
        {
            get => _caretPosition;
            set
            {
                if (value < 0) _caretPosition = 0;
                else if (value > KeyboardText.Length) _caretPosition = KeyboardText.Length;
                else _caretPosition = value;
                NotifyPropertyChanged(nameof(CaretPosition));
            }
        }
        private string _selectedValue;

        public string SelectedValue
        {
            get => _selectedValue;
            set
            {
                _selectedValue = value;
                NotifyPropertyChanged(nameof(SelectedValue));
            }
        }
        public Command AddCharacter { get; }
        public Command ChangeCasing { get; }
        public Command RemoveCharacter { get; }
        public Command ChangeKeyboardType { get; }
        public Command Accept { get; }

        public VirtualKeyboardViewModel(string initialValue, bool isPassword, char passwordChar)
        {
            _isPassword = isPassword;
            _passwordChar = passwordChar;
            KeyboardText = initialValue;

            _keyboardType = KeyboardType.Alphabet;
            _uppercase = false;
            CaretPosition = KeyboardText.Length;

            AddCharacter = new Command(a =>
            {
                if (a is string character)
                    if (character.Length == 1)
                    {
                        if (Uppercase) character = character.ToUpper();
                        if (!string.IsNullOrEmpty(SelectedValue))
                        {
                            RemoveSubstring(SelectedValue);
                            KeyboardText = KeyboardText.Insert(CaretPosition, character);
                            CaretPosition++;
                            SelectedValue = "";
                        }
                        else
                        {
                            KeyboardText = KeyboardText.Insert(CaretPosition, character);
                            CaretPosition++;
                        }
                    }
            });
            ChangeCasing = new Command(a => Uppercase = !Uppercase);
            RemoveCharacter = new Command(a =>
            {
                if (!string.IsNullOrEmpty(SelectedValue))
                {
                    RemoveSubstring(SelectedValue);
                }
                else
                {
                    var position = CaretPosition - 1;
                    if (position >= 0)
                    {
                        KeyboardText = KeyboardText.Remove(position, 1);
                        if (position < KeyboardText.Length) CaretPosition--;
                        else CaretPosition = KeyboardText.Length;
                    }
                }
            });
            ChangeKeyboardType = new Command(a =>
            {
                if (KeyboardType == KeyboardType.Alphabet) KeyboardType = KeyboardType.Special;
                else KeyboardType = KeyboardType.Alphabet;
            });
            Accept = new Command(a => VKeyboard.Close());
        }

        private void RemoveSubstring(string substring)
        {
            var position = KeyboardText.IndexOf(substring);
            KeyboardText = KeyboardText.Remove(position, substring.Length);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
