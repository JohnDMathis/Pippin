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

    public class CommandBehavior : TriggerAction<Control>
    {
        public static readonly DependencyProperty CommandBindingProperty = DependencyProperty.Register(
            "CommandBinding",
            typeof(string),
            typeof(CommandBehavior),
            null);

        public string CommandBinding
        {
            get { return (string)GetValue(CommandBindingProperty); }
            set { SetValue(CommandBindingProperty, value); }
        }

        private CommandAction _action;

        protected override void OnAttached()
        {
            DataContextChangedHandler.Bind(AssociatedObject, obj => _ProcessCommand());
        }

        private void _ProcessCommand()
        {
            if (AssociatedObject != null)
            {

                var dataContext = AssociatedObject.DataContext;

                if (dataContext != null)
                {
                    var property = dataContext.GetType().GetProperty(CommandBinding);
                    if (property != null)
                    {
                        var value = property.GetValue(dataContext, null);
                        if (value != null && value is CommandAction)
                        {
                            _action = value as CommandAction;
                            AssociatedObject.IsEnabled = _action.CanExecute(null);
                            _action.CanExecuteChanged += (o, e) => AssociatedObject.IsEnabled = _action.CanExecute(null);

                        }
                    }
                }
            }
        }

        protected override void Invoke(object parameter)
        {
            if (_action != null && _action.CanExecute(null))
            {
                _action.Execute(null);
            }
        }
    }
}
