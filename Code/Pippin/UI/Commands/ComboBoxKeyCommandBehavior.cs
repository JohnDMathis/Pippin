using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Composite.Presentation.Commands;

namespace Odin.UI.Infrastructure.Commands
{
    public class ComboBoxKeyCommandBehavior : CommandBehaviorBase<ComboBox>    // not yet working
    {
        public ComboBoxKeyCommandBehavior(ComboBox comboBox)
            : base(comboBox)
        {
            comboBox.KeyDown += (s, e) => this.KeyPressed(e.Key);
            comboBox.GotFocus += (s, e) => this.GotFocus();
            comboBox.LostFocus += (s, e) => this.LostFocus();
        }

        protected void KeyPressed(Key key)
        {
            if (key == Key.Enter && TargetObject != null)
            {
                // if developer entered a name for the textbox, include it in the parameter response, delimited with a '|'
                string cmdParam = "";
                if (TargetObject.Name != null)
                    cmdParam = TargetObject.Name + "|";
                cmdParam +=TargetObject.SelectedIndex;
                
                CommandParameter = cmdParam;
                ExecuteCommand();

                this.ResetText();
            }
        }

        private void GotFocus()
        {
            if (TargetObject != null && TargetObject.SelectedIndex == -1)
            {
                this.ResetText();
            }
        }

        private void ResetText()
        {
            TargetObject.SelectedIndex = -1;
        }

        private void LostFocus()
        {
            if (TargetObject != null && TargetObject.SelectedIndex == -1)
            {
                TargetObject.SelectedIndex = -1;
            }
        }
    }
}
