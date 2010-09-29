using System;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Threading;
using Microsoft.Practices.Composite.Presentation.Commands;

namespace Odin.UI.Infrastructure.Commands
{
    public class ListDoubleClickCommandBehavior:CommandBehaviorBase<ListBox>
    {
        private DispatcherTimer _timer;
        public ListDoubleClickCommandBehavior(ListBox targetObject)
            : base(targetObject)
        {
            // init timer
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(300);

            // event handlers
            _timer.Tick += (s, e) => TimerDing();
            targetObject.MouseLeftButtonUp += (s, e) => MouseUp();
           

        }

        private void MouseUp()
        {
            // is the timer running?
            if (_timer.IsEnabled)
            {
                _timer.Stop();
                CommandParameter = TargetObject.SelectedItem;
                ExecuteCommand();
            }
            else
            {
                _timer.Start();
            }
        }

        private void TimerDing()
        {
            _timer.Stop();
        }
    }
}