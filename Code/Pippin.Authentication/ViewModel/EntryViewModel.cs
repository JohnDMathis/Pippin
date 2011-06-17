using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Pippin.UI.ViewModel;
using MI.Model;
using MI.Services;
using System.ServiceModel.DomainServices.Client;
using System.Linq;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Commands;
using System.ComponentModel;
using Pippin.UI.Events;

namespace MI.Authentication.ViewModel
{
    public class EntryViewModel:ViewModelBase
    {
        public AuthenticationContext Context { get; set; }
        public IEventAggregator EventMgr { get; set; }
        public AppUser AuthenticatedUser { get; set; }

        public EntryViewModel(AuthenticationContext context, IEventAggregator ea)
        {
            Context = context;
            EventMgr = ea;
           InitCommands();
        }

        public override void Start()
        {
            // check login to determine if a user is already logged in
            Context.GetAuthenticatedUser("{54B72F19-1A6B-4379-BAD4-72544B3761D9}").Completed += new EventHandler(GetAuthenticatedUser_Done);
            State = LoginState.CheckingAuthentication;
        }

        void GetAuthenticatedUser_Done(object sender, EventArgs e)
        {
            AppUser user = ((InvokeOperation<AppUser>)sender).Value;
            if (user == null)
                State = LoginState.Ready;
            else
            {
                State = LoginState.Authenticated;
                AuthenticatedUser = user;
                EventMgr.GetEvent<LogonEvent>().Publish(AuthenticatedUser);
            }
        }

        public void RequestLogin()
        {
            if (UserName == string.Empty || Password == string.Empty) return;

            State = LoginState.Authenticating;
            Context.InvokeLogin(UserName, Password, RememberMe, "{54B72F19-1A6B-4379-BAD4-72544B3761D9}")
                .Completed += new EventHandler(InvokeLogin_Done);
        }

        void InvokeLogin_Done(object sender, EventArgs e)
        {
            AppUser user = ((InvokeOperation<AppUser>)sender).Value;
            if (user == null)
            {
                State = LoginState.InvalidCredential;
                UserName = string.Empty;
                Password = string.Empty;
            }
            else
            {
                State = LoginState.Authenticated;
                AuthenticatedUser = user;
                EventMgr.GetEvent<LogonEvent>().Publish(AuthenticatedUser);
            }
        }

        // currently unused; this is the version of the above that is used when
        // using the LoginQuery instead of the InvokeLogin method
        void Login_Done(object sender, EventArgs e)
        {
            var op = (LoadOperation<AppUser>)sender;
            if (op.Entities.Count<AppUser>() == 0)
            {
                State = LoginState.InvalidCredential;
                UserName = string.Empty;
                Password = string.Empty;
            }
            else
            {
                State = LoginState.Authenticated;
                AuthenticatedUser = ((LoadOperation<AppUser>)sender).Entities.FirstOrDefault();
                EventMgr.Send_Logon(AuthenticatedUser);
            }
        }

        public void LoginFailureAcknowledged()
        {
            State = LoginState.Ready;
        }

        public void RequestLogout()
        {
            Context.InvokeLogout();
            State = LoginState.Ready;
            EventMgr.Send_Logoff();
        }


        #region [ Bindable Properties ]

        
        public bool CanLogin
        {
            get { return userName != string.Empty && password != string.Empty; }
        }

        private LoginState state;
        public LoginState State
        {
            get { return state; }
            set
            {
                state = value;
                RaisePropertyChanged("State");
            }
        }

        private string userName = String.Empty;
        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                RaisePropertyChanged("UserName");
                LoginCommand.RaiseCanExecuteChanged();
            }
        }

        private string password = String.Empty;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                RaisePropertyChanged("Password");
                LoginCommand.RaiseCanExecuteChanged();
            }
        }

        private bool rememberMe;
        public bool RememberMe
        {
            get { return rememberMe; }
            set
            {
                rememberMe = value;
                RaisePropertyChanged("RememberMe");
            }
        }


        #endregion [ Bindable Properties ]

        #region [ Commanding ]

        public DelegateCommand LoginCommand { get; set; }
        public DelegateCommand LogoutCommand { get; set; }
        public DelegateCommand AcknowledgeErrorCommand { get; set; }
        public DelegateCommand DefaultCommand { get; set; }
        public void InitCommands()
        {
            LoginCommand = new DelegateCommand(RequestLogin, () => CanLogin);
            LogoutCommand = new DelegateCommand(RequestLogout);
            AcknowledgeErrorCommand = new DelegateCommand(LoginFailureAcknowledged);
            DefaultCommand = new DelegateCommand(RequestLogin);
        }

        #endregion



    }
}
