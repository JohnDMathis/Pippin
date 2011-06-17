using System.Windows.Controls;
using Microsoft.Practices.Unity;
using Pippin.UI.Screens;
using MI.Authentication.ViewModel;
using MI.Authentication.Views;
using Microsoft.Practices.Prism.Events;
using Pippin.UI.Events;

namespace MI.Authentication
{
    public class EntryScreen : ScreenBase
    {
        public EntryScreen(EntryView view, EntryViewModel viewModel, IUnityContainer container)
            : base(null)
        {
            View = view;
            ViewModel = viewModel;
            view.DataContext = ViewModel;
        }

        public EntryViewModel ViewModel { get; set; }

        public EntryView EntryView { get { return (EntryView)this.View; } }

        #region Overrides of ScreenBase

        public override object Subject { get; set; }

        public override UserControl View { get; set; }

        public override bool CanEnter()
        {
            return true;
        }

        public override bool CanLeave()
        {
            return true;
        }

        public override void Leaving()
        {

        }

        public override void Setup()
        {
            ViewModel.Start();
        }

        public override void LeaveCanceled()
        {

        }

        #endregion


    }
}
