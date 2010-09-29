using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Odin.UI.Infrastructure.Commands
{
    public class ComboBoxKey    // not yet working
    {
        /// <summary>
        /// Command to execute on ComboBox key event.
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(
            "Command",
            typeof(ICommand),
            typeof(ComboBoxKey),
            new PropertyMetadata(OnSetCommandCallback));

        private static readonly DependencyProperty ComboBoxKeyCommandBehaviorProperty = DependencyProperty.RegisterAttached(
            "ComboBoxKeyCommandBehavior",
            typeof(ComboBoxKeyCommandBehavior),
            typeof(ComboBoxKey),
            null);

        /// <summary>
        /// Sets the <see cref="ICommand"/> to execute on the ComboBox key event.
        /// </summary>
        /// <param name="textBox">TextBox dependency object to attach command</param>
        /// <param name="command">Command to attach</param>
        public static void SetCommand(ComboBox key, ICommand command)
        {
            key.SetValue(CommandProperty, command);
        }

        /// <summary>
        /// Retrieves the <see cref="ICommand"/> attached to the <see cref="TextBox"/>.
        /// </summary>
        /// <param name="textBox">TextBox containing the Command dependency property</param>
        /// <returns>The value of the command attached</returns>
        public static ICommand GetCommand(ComboBox comboBox)
        {
            return comboBox.GetValue(CommandProperty) as ICommand;
        }

        private static void OnSetCommandCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            ComboBox comboBox = dependencyObject as ComboBox;
            if (comboBox != null)
            {
                var behavior = GetOrCreateBehavior(comboBox);
                behavior.Command = e.NewValue as ICommand;
            }
        }

        private static ComboBoxKeyCommandBehavior GetOrCreateBehavior(ComboBox comboBox)
        {
            var behavior = comboBox.GetValue(ComboBoxKeyCommandBehaviorProperty) as ComboBoxKeyCommandBehavior;
            if (behavior == null)
            {
                behavior = new ComboBoxKeyCommandBehavior(comboBox);
                comboBox.SetValue(ComboBoxKeyCommandBehaviorProperty, behavior);
            }

            return behavior;
        }
    }
}
