using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Odin.UI.Infrastructure.Commands
{
    public class DataGridSelect
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(
            "Command", typeof(ICommand), typeof(DataGridSelect), new PropertyMetadata(OnSetCommandCallback));

        private static readonly DependencyProperty SelectCommandBehaviorProperty = DependencyProperty.RegisterAttached(
            "SelectCommandBehavior",
            typeof(DataGridSelectCommandBehavior),
            typeof(DataGridSelect),
            null);

        private static void OnSetCommandCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataGrid selector = d as DataGrid;
            if (selector != null)
            {
                var behavior = GetOrCreateBehavior(selector);
                behavior.Command = e.NewValue as ICommand;
            }
        }

        private static DataGridSelectCommandBehavior GetOrCreateBehavior(DataGrid selector)
        {
            var behavior = selector.GetValue(SelectCommandBehaviorProperty) as DataGridSelectCommandBehavior;
            if (behavior == null)
            {
                behavior = new DataGridSelectCommandBehavior(selector);
                selector.SetValue(SelectCommandBehaviorProperty, behavior);
            }

            return behavior;
        }
        public static void SetCommand(DataGrid selector, ICommand command)
        {
            selector.SetValue(CommandProperty, command);
        }

        /// <summary>
        /// Retrieves the <see cref="ICommand"/> attached to the <see cref="TextBox"/>.
        /// </summary>
        /// <param name="selector">TextBox containing the Command dependency property</param>
        /// <returns>The value of the command attached</returns>
        public static ICommand GetCommand(DataGrid selector)
        {
            return selector.GetValue(CommandProperty) as ICommand;
        }

    }
}
