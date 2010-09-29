using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Odin.UI.Infrastructure.Commands
{
    public static class pwReturnKey
    {
        /// <summary>
        /// Command to execute on return key event.
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(
            "Command",
            typeof(ICommand),
            typeof(pwReturnKey),
            new PropertyMetadata(OnSetCommandCallback));

        private static readonly DependencyProperty pwReturnCommandBehaviorProperty = DependencyProperty.RegisterAttached(
            "pwReturnCommandBehavior",
            typeof(pwReturnCommandBehavior),
            typeof(pwReturnKey),
            null);

        public static void SetCommand(PasswordBox pwBox, ICommand command)
        {
            pwBox.SetValue(CommandProperty, command);
        }

        public static ICommand GetCommand(PasswordBox pwBox)
        {
            return pwBox.GetValue(CommandProperty) as ICommand;
        }


        private static void OnSetCommandCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox pwBox = dependencyObject as PasswordBox;
            if (pwBox != null)
            {
                pwReturnCommandBehavior behavior = GetOrCreateBehavior(pwBox);
                behavior.Command = e.NewValue as ICommand;
            }
        }

        private static pwReturnCommandBehavior GetOrCreateBehavior(PasswordBox pwBox)
        {
            pwReturnCommandBehavior behavior = pwBox.GetValue(pwReturnCommandBehaviorProperty) as pwReturnCommandBehavior;
            if (behavior == null)
            {
                behavior = new pwReturnCommandBehavior(pwBox);
                pwBox.SetValue(pwReturnCommandBehaviorProperty, behavior);
            }

            return behavior;
        }
    }
}