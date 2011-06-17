using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace Pippin.UI.Commands
{
    public class pwReturnCommandBehavior : CommandBehaviorBase<PasswordBox>, IReturnCommandBehavior
    {
        public pwReturnCommandBehavior(PasswordBox pwBox)
            : base(pwBox)
        {
            pwBox.KeyDown += (s, e) => this.KeyPressed(e.Key);
            pwBox.GotFocus += (s, e) => this.GotFocus();
        }


        public string DefaultTextAfterExecution { get; set; }

        protected void KeyPressed(Key key)
        {
            if (key == Key.Enter && TargetObject != null)
            {
                CommandParameter = TargetObject.Password;
                ExecuteCommand();

                TargetObject.Password = "";
            }
        }

        private void GotFocus()
        {
            if (TargetObject != null)
            {
                TargetObject.Password = "";
            }
        }


    }
}