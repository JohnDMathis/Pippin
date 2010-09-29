using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Odin.UI.Infrastructure.Commands
{
    public static class ListDoubleClick
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(
            "Command", typeof(ICommand), typeof(ListDoubleClick), new PropertyMetadata(OnSetCommandCallback));

        private static readonly DependencyProperty SelectCommandBehaviorProperty = DependencyProperty.RegisterAttached(
            "SelectCommandBehavior",
            typeof(ListDoubleClickCommandBehavior),
            typeof(ListDoubleClick),
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

        private static ListDoubleClickCommandBehavior GetOrCreateBehavior(ListBox selector)
        {
            var behavior = selector.GetValue(SelectCommandBehaviorProperty) as ListDoubleClickCommandBehavior;
            if (behavior == null)
            {
                behavior = new ListDoubleClickCommandBehavior(selector);
                selector.SetValue(SelectCommandBehaviorProperty, behavior);
            }

            return behavior;
        }
        public static void SetCommand(ListBox selector, ICommand command)
        {
            selector.SetValue(CommandProperty, command);
        }

        public static ICommand GetCommand(ListBox selector)
        {
            return selector.GetValue(CommandProperty) as ICommand;
        }

    }
}