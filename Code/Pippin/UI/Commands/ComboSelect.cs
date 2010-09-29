using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Odin.UI.Infrastructure.Commands
{
    public static class ComboSelect
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(
            "Command", typeof(ICommand), typeof(ComboSelect), new PropertyMetadata(OnSetCommandCallback));

        private static readonly DependencyProperty SelectCommandBehaviorProperty = DependencyProperty.RegisterAttached(
            "SelectCommandBehavior",
            typeof(ComboSelectCommandBehavior),
            typeof(ComboSelect),
            null);

        private static void OnSetCommandCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ComboBox selector = d as ComboBox;
            if (selector != null)
            {
                var behavior = GetOrCreateBehavior(selector);
                behavior.Command = e.NewValue as ICommand;
            }

        }

        private static ComboSelectCommandBehavior GetOrCreateBehavior(ComboBox selector)
        {
            var behavior = selector.GetValue(SelectCommandBehaviorProperty) as ComboSelectCommandBehavior;
            if (behavior == null)
            {
                behavior = new ComboSelectCommandBehavior(selector);
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