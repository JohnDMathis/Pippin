using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;


namespace Pippin.UI.ViewModel
{
    public abstract class ViewModelBase : EventAggregatorClient, INotifyPropertyChanged, IViewModel
    {
        protected ViewModelBase() {} // needed for design time and testing

        public FrameworkElement ContentContainer { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;
//        public virtual ScreenBase Screen { get; set; }
//       protected DomainContext _DomainContext { get; set; }

        #region [ INotifyPropertyChanged ]

        protected internal void FirePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        public virtual void CleanUp()
        {
            
        }
    }
}