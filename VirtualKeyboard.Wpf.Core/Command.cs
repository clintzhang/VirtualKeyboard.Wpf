﻿using System;
using System.Windows.Input;

namespace VirtualKeyboard.Wpf.Core;

internal class Command : ICommand {
    private readonly Action<object> _execute;

    private bool _canExecuteCommand = true;

    public Command(Action<object> execute) {
        _execute = execute;
    }

    public bool CanExecuteCommand {
        get => _canExecuteCommand;
        set {
            _canExecuteCommand = value;
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter) {
        return CanExecuteCommand;
    }

    public void Execute(object parameter) {
        _execute?.Invoke(parameter);
    }
}