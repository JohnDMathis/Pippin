using System;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Practices.Composite.Presentation.Commands;

namespace Odin.UI.Infrastructure.Commands
{
    public class DataGridSelectCommandBehavior : CommandBehaviorBase<DataGrid>
    {
        private DispatcherTimer _timer;
        private bool _okToExecute = true;
        public DataGridSelectCommandBehavior(DataGrid selector)
            : base(selector)
        {
            selector.MouseLeftButtonDown += selector_MouseLeftButtonDown;
            selector.SelectionChanged += (s, e) => ItemSelected(e.AddedItems.Count >= 1 ? e.AddedItems[0] : null);
        }

        // this is a work-around for a presumed bug in DataGrid.
        // The SelectionChanged event fires in response to the user sorting via the column header
        // Fortunately, a data row click consumes the mouse down message, but clicks in the header do not
        //  execution is surpressed when the user clicks in the header, and then reset back to true
        // when the timer runs out (after 500 milliseconds)
        // Note; this problem does not manifest when the ItemsSource is ObservableCollection;
        // so far, only List has displayed this behavior.
        void selector_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _okToExecute = false;
            _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
            _timer.Tick += TimerDing;
            _timer.Start();
        }

        void TimerDing(object sender, EventArgs e)
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer = null;
            }
            _okToExecute = true;
        }

        private void ItemSelected(object item)
        {
            if (_okToExecute)
            {
                CommandParameter = item;
                ExecuteCommand();
            }
        }
    }
}
