using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Odin.UI.Infrastructure.Commands
{
    public class ComboBoxSelect
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(
            "Command", typeof(ICommand), typeof(ComboBoxSelect), new PropertyMetadata(OnSetCommandCallback));

        private static readonly DependencyProperty SelectCommandBehaviorProperty = DependencyProperty.RegisterAttached(
            "SelectCommandBehavior",
            typeof(ComboBoxSelectCommandBehavior),
            typeof(ComboBoxSelect),
            null);

        private static void OnSetCommandCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ComboBox selector = d as ComboBox;
            if (selector != null)
            {
                var behavior = GetOrCreateBehavior(selector);
                behavior.Command = e.NewValue as ICommand;
            }
            ComboBox c = new ComboBox();
        }

        private static ComboBoxSelectCommandBehavior GetOrCreateBehavior(ComboBox selector)
        {
            var behavior = selector.GetValue(SelectCommandBehaviorProperty) as ComboBoxSelectCommandBehavior;
            if (behavior == null)
            {
                behavior = new ComboBoxSelectCommandBehavior(selector);
                selector.SetValue(SelectCommandBehaviorProperty, behavior);
            }

            return behavior;
        }
        public static void SetCommand(ComboBox selector, ICommand command)
        {
            selector.SetValue(CommandProperty, command);
        }

        /// <summary>
        /// Retrieves the <see cref="ICommand"/> attached to the <see cref="TextBox"/>.
        /// </summary>
        /// <param name="selector">TextBox containing the Command dependency property</param>
        /// <returns>The value of the command attached</returns>
        public static ICommand GetCommand(ComboBox selector)
        {
            return selector.GetValue(CommandProperty) as ICommand;
        }

    }
}
