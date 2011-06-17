using System.Windows.Controls;
using Microsoft.Practices.Prism.Commands;

namespace Pippin.UI.Commands
{
    public class ListSelectCommandBehavior : CommandBehaviorBase<ListBox>
    {
        public ListSelectCommandBehavior(ListBox selector)
            : base(selector)
        {
            selector.SelectionChanged += (s, e) => this.ItemSelected(e);
        }

        private void ItemSelected(object item)
        {
            this.CommandParameter = item;
            ExecuteCommand();
        }
    }
}