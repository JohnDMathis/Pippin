using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Microsoft.Practices.Composite.Presentation.Commands;

namespace Odin.UI.Infrastructure.Commands
{
    public class GenericSelectCommandBehavior : CommandBehaviorBase<Selector>
    {
        public GenericSelectCommandBehavior(Selector selector)
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