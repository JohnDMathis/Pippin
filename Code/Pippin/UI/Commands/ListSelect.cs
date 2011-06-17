using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Pippin.UI.Commands
{
    public static class ListSelect
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(
            "Command", typeof(ICommand), typeof(ListSelect), new PropertyMetadata(OnSetCommandCallback));

        private static readonly DependencyProperty SelectCommandBehaviorProperty = DependencyProperty.RegisterAttached(
            "SelectCommandBehavior",
            typeof(ListSelectCommandBehavior),
            typeof(ListSelect),
            null);

        private static void OnSetCommandCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ListBox selector = d as ListBox;
            if (selector != null)
            {
                var behavior = GetOrCreateBehavior(selector);
                behavior.Command = e.NewValue as ICommand;
            }

        }

        private static ListSelectCommandBehavior GetOrCreateBehavior(ListBox selector)
        {
            var behavior = selector.GetValue(SelectCommandBehaviorProperty) as ListSelectCommandBehavior;
            if (behavior == null)
            {
                behavior = new ListSelectCommandBehavior(selector);
                selector.SetValue(SelectCommandBehaviorProperty, behavior);
            }

            return behavior;
        }
        public static void SetCommand(ListBox selector, ICommand command)
        {
            selector.SetValue(CommandProperty, command);
        }

        /// <summary>
        /// Retrieves the <see cref="ICommand"/> attached to the <see cref="TextBox"/>.
        /// </summary>
        /// <param name="selector">TextBox containing the Command dependency property</param>
        /// <returns>The value of the command attached</returns>
        public static ICommand GetCommand(ListBox selector)
        {
            return selector.GetValue(CommandProperty) as ICommand;
        }

    }
}