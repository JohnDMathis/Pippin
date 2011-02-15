using System;
using System.Windows.Input;

namespace Pippin.UI.Commands
{
    public interface IReturnCommandBehavior
    {
        string DefaultTextAfterCommandExecution { get; set; }
        ICommand Command { get; set; }
        object CommandParameter { get; set; }
    }
}
