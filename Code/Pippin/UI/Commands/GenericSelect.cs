using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Odin.UI.Infrastructure.Commands
{
    public static class GenericSelect
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(
            "Command", typeof(ICommand), typeof(Selector), new PropertyMetadata(OnSetCommandCallback));

        private static readonly DependencyProperty SelectCommandBehaviorProperty = DependencyProperty.RegisterAttached(
            "SelectCommandBehavior",
            typeof(GenericSelectCommandBehavior),
            typeof(GenericSelect),
            null);

        private static void OnSetCommandCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Selector selector = d as Selector;
            if (selector != null)
            {
                var behavior = GetOrCreateBehavior(selector);
                behavior.Command = e.NewValue as ICommand;
            }

        }

        private static GenericSelectCommandBehavior GetOrCreateBehavior(Selector selector)
        {
            var behavior = selector.GetValue(SelectCommandBehaviorProperty) as GenericSelectCommandBehavior;
            if (behavior == null)
            {
                behavior = new GenericSelectCommandBehavior(selector);
                selector.SetValue(SelectCommandBehaviorProperty, behavior);
            }

            return behavior;
        }
        public static void SetCommand(Selector selector, ICommand command)
        {
            selector.SetValue(CommandProperty, command);
        }

        /// <summary>
        /// Retrieves the <see cref="ICommand"/> attached to the <see cref="TextBox"/>.
        /// </summary>
        /// <param name="selector">TextBox containing the Command dependency property</param>
        /// <returns>The value of the command attached</returns>
        public static ICommand GetCommand(Selector selector)
        {
            return selector.GetValue(CommandProperty) as ICommand;
        }

    }
}