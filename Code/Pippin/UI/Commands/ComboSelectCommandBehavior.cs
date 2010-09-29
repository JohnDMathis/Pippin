using System.Windows.Controls;
using Microsoft.Practices.Composite.Presentation.Commands;

namespace Odin.UI.Infrastructure.Commands
{
    public class ComboSelectCommandBehavior : CommandBehaviorBase<ComboBox>
    {
        public ComboSelectCommandBehavior(ComboBox selector)
            : base(selector)
        {
            selector.SelectionChanged += (s, e) => this.ItemSelected(e.AddedItems[0]);
        }

        private void ItemSelected(object item)
        {
            this.CommandParameter = item;
            ExecuteCommand();
        }
    }
}