using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Composite.Presentation.Commands;

namespace Odin.UI.Infrastructure.Commands
{
    public class ReturnCommandBehavior : CommandBehaviorBase<TextBox>
    {
        public ReturnCommandBehavior(TextBox textBox)
            : base(textBox)
        {
            textBox.AcceptsReturn = false;
            textBox.KeyDown += (s, e) => this.KeyPressed(e.Key);
            textBox.GotFocus += (s, e) => this.GotFocus();
            textBox.LostFocus += (s, e) => this.LostFocus();
        }

        public string DefaultTextAfterCommandExecution { get; set; }

        protected void KeyPressed(Key key)
        {
            if (key == Key.Enter && TargetObject != null)
            {
                // if developer entered a name for the textbox, include it in the parameter response, delimited with a '|'
                string cmdParam = "";
                if (TargetObject.Name != null)
                    cmdParam = TargetObject.Name + "|";
                cmdParam +=TargetObject.Text;
                
                CommandParameter = cmdParam;
                ExecuteCommand();

                this.ResetText();
            }
        }

        private void GotFocus()
        {
            if (TargetObject != null && TargetObject.Text == this.DefaultTextAfterCommandExecution)
            {
                this.ResetText();
            }
        }

        private void ResetText()
        {
            TargetObject.Text = string.Empty;
        }

        private void LostFocus()
        {
            if (TargetObject != null && string.IsNullOrEmpty(TargetObject.Text) && this.DefaultTextAfterCommandExecution != null)
            {
                TargetObject.Text = this.DefaultTextAfterCommandExecution;
            }
        }
    }
}