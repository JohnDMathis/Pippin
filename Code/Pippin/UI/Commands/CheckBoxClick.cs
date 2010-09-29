
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Odin.UI.Infrastructure.Commands
{
    public class CheckBoxClick
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(
            "Command", typeof(ICommand), typeof(CheckBoxClick), new PropertyMetadata(OnSetCommandCallback));

        private static readonly DependencyProperty ClickCommandBehaviorProperty = DependencyProperty.RegisterAttached(
            "ClickCommandBehavior",
            typeof(CheckBoxClickCommandBehavior),
            typeof(CheckBoxClick),
            null);

        private static void OnSetCommandCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CheckBox clicker = d as CheckBox;
            if (clicker != null)
            {
                var behavior = GetOrCreateBehavior(clicker);
                behavior.Command = e.NewValue as ICommand;
            }
            CheckBox c = new CheckBox();
        }

        private static CheckBoxClickCommandBehavior GetOrCreateBehavior(CheckBox clicker)
        {
            var behavior = clicker.GetValue(ClickCommandBehaviorProperty) as CheckBoxClickCommandBehavior;
            if (behavior == null)
            {
                behavior = new CheckBoxClickCommandBehavior(clicker);
                clicker.SetValue(ClickCommandBehaviorProperty, behavior);
            }

            return behavior;
        }
        public static void SetCommand(CheckBox clicker, ICommand command)
        {
            clicker.SetValue(CommandProperty, command);
        }

        /// <summary>
        /// Retrieves the <see cref="ICommand"/> attached to the <see cref="TextBox"/>.
        /// </summary>
        /// <param name="selector">TextBox containing the Command dependency property</param>
        /// <returns>The value of the command attached</returns>
        public static ICommand GetCommand(CheckBox clicker)
        {
            return clicker.GetValue(CommandProperty) as ICommand;
        }

    }
}
