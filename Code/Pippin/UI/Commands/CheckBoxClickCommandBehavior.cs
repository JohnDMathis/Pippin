
using System.Windows.Controls;
using Microsoft.Practices.Composite.Presentation.Commands;

namespace Odin.UI.Infrastructure.Commands
{
    public class CheckBoxClickCommandBehavior : CommandBehaviorBase<CheckBox>
    {
        public CheckBoxClickCommandBehavior(CheckBox clicker)
            : base(clicker)
        {
            clicker.Click += (s, e) => ItemSelected(e.OriginalSource);
        }

        private void ItemSelected(object item)
        {
            CommandParameter = item;
            ExecuteCommand();
        }
    }
}
