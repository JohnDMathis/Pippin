using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace Pippin.UI.Commands
{

    public class ReturnCommandBehavior : CommandBehaviorBase<TextBox>, IReturnCommandBehavior
    {
        public ReturnCommandBehavior(TextBox textBox)
            : base(textBox)
        {
            textBox.AcceptsReturn = false;
            textBox.KeyDown += (s, e) => this.KeyPressed(e.Key);
            textBox.GotFocus += (s, e) => this.GotFocus();
            textBox.LostFocus += (s, e) => this.LostFocus();
        }

        public string DefaultTextAfterExecution { get; set; }

        protected void KeyPressed(Key key)
        {
            if (key == Key.Enter && TargetObject != null)
            {
                // if developer entered a name for the textbox, include it in the parameter response, delimited with a '|'
                string cmdParam = "";
                if (!string.IsNullOrEmpty(TargetObject.Name))
                    cmdParam = TargetObject.Name + "|";
                cmdParam += TargetObject.Text;

                CommandParameter = cmdParam;
                ExecuteCommand();

                if (!string.IsNullOrEmpty(DefaultTextAfterExecution))
                    TargetObject.Text = DefaultTextAfterExecution;
            }
        }

        private void GotFocus()
        {
            if (TargetObject != null && TargetObject.Text == this.DefaultTextAfterExecution)
            {
                TargetObject.Text = string.Empty;
            }
        }

        private void LostFocus()
        {
            if (TargetObject != null && string.IsNullOrEmpty(TargetObject.Text) && this.DefaultTextAfterExecution != null)
            {
                TargetObject.Text = this.DefaultTextAfterExecution;
            }
        }
    }
}
