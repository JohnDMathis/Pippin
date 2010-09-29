using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Interactivity;
using System.Windows.Data;

namespace Pippin.UI.Commands
{
    // Borrowed from Jeremy Likeness
    // See http://csharperimage.jeremylikness.com/2010/02/mef-instead-of-prism-for-silverlight-3.html

    public class CommandAction : ICommand
    {
        private readonly Func<bool> _canExecute;

        private readonly Action _execute;

        public CommandAction(Action action, Func<bool> canExecute)
        {
            _execute = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute();
        }

        public void Execute(object parameter)
        {
            _execute();
        }

        public void RaiseCanExecuteChanged()
        {
            EventHandler handler = CanExecuteChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}
