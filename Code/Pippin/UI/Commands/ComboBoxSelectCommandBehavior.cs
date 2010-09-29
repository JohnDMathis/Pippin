using System.Windows.Controls;
using Microsoft.Practices.Composite.Presentation.Commands;

namespace Odin.UI.Infrastructure.Commands
{
    public class ComboBoxSelectCommandBehavior : CommandBehaviorBase<ComboBox>
    {
        public ComboBoxSelectCommandBehavior(ComboBox selector)
            : base(selector)
        {
            selector.SelectionChanged += ItemSelected;
        }

        private void ItemSelected(object sender, object e)
        {
            var combobox = (ComboBox) sender;
            if (combobox.IsDropDownOpen)
            {
                combobox.IsDropDownOpen = false;
                CommandParameter = e;
                ExecuteCommand();
            }
        }
    }
}
