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
using Pippin.UI.Commands;

namespace Pippin.UI.Behaviors
{
    public class ReturnKey
    {
        public static ICommand GetCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandProperty);
        }

        public static void SetCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CommandProperty, value);
        }

        public static string GetDefaultTextAfterExecution(DependencyObject obj)
        {
            return (string)obj.GetValue(DefaultTextAfterExecutionProperty);
        }

        public static void GetDefaultTextAfterExecution(DependencyObject obj, string value)
        {
            obj.SetValue(DefaultTextAfterExecutionProperty, value);
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(ReturnKey), new PropertyMetadata(CommandPropertySet));

        public static readonly DependencyProperty CommandBehaviorProperty =
            DependencyProperty.RegisterAttached("CommandBehavior", typeof(IReturnCommandBehavior), typeof(ReturnKey), new PropertyMetadata(null));

        public static readonly DependencyProperty DefaultTextAfterExecutionProperty =
            DependencyProperty.RegisterAttached("DefaultTextAfterExecution", typeof(string), typeof(ReturnKey), new PropertyMetadata(DefaultTextAfterExecutionSet));

        private static void CommandPropertySet(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
            IReturnCommandBehavior behavior = depObj.GetValue(CommandBehaviorProperty) as IReturnCommandBehavior;
            if (behavior == null)
            {
                behavior = ResolveCommandBehavior(depObj);
                depObj.SetValue(CommandBehaviorProperty, behavior);
            }
            if (behavior != null)
                behavior.Command = (ICommand)e.NewValue;
            else
                throw new Exception("ReturnKey only operates on TextBox and PasswordBox.");
        }

        private static void DefaultTextAfterExecutionSet(DependencyObject ctrl, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (IReturnCommandBehavior)ctrl;
            if (behavior != null)
                behavior.DefaultTextAfterExecution = e.NewValue.ToString();
        }
        private static IReturnCommandBehavior ResolveCommandBehavior(DependencyObject ctrl)
        {
            IReturnCommandBehavior behavior;
            if (ctrl.GetType() == typeof(TextBox))
                behavior = new ReturnCommandBehavior((TextBox)ctrl);
            else if (ctrl.GetType() == typeof(PasswordBox))
                behavior = new pwReturnCommandBehavior((PasswordBox)ctrl);
            else
                behavior = null;
            return behavior;
        }

    }
}

