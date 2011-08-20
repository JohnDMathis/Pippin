using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using Pippin.UI.Events;
using Pippin.UI.Screens;

using Microsoft.Practices.Prism.Events;

namespace Pippin.UI.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged, IViewModel
    {
        protected ViewModelBase() {} // needed for design time and testing
        public IEventAggregator EventManager { get; set; }
        public FrameworkElement ContentContainer { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public ScreenBase Screen { get; set; }

        #region [ INotifyPropertyChanged ]

        public ViewModelBase(IEventAggregator ea)
        {
            EventManager = ea;
        }

        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        public virtual void Start()
        {
        }
        
        public virtual void CleanUp()
        {
            
        }
    }
}