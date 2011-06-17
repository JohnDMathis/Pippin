using System;
using System.Collections.Generic;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using MI.Model;
using MI.Services;
using System.Windows.Threading;
using System.ServiceModel.DomainServices.Client;
using Pippin.UI.Screens;
using Pippin.UI.ViewModel;
using Pippin.UI.Events;
using Pippin.UI.Regions;

namespace MI.Authentication
{
    public enum ActionItem { } //TBD

    public class CurrentUser
    {

        #region [ Properties / Constructor ]

        public static AppUser Entity { get; set; }

        private readonly AuthenticationContext _authContext;
        private DispatcherTimer _LogoutTimer;
        private DispatcherTimer _AuthenticationCheckTimer;
        private static CurrentUser Instance;
        private IEventAggregator EventManager;

        private static IList<ActionItem> AllowedActions { get; set; }

        public CurrentUser(AuthenticationContext authContext, IEventAggregator eventMgr)
        {
            _authContext = authContext;
            EventManager = eventMgr;
            Instance = this;
            EventManager.GetEvent<LogonEvent>().Subscribe(HandleLogonEvent);
            EventManager.GetEvent<LogoffEvent>().Subscribe(HandleLogoffEvent);
        }

        #endregion [ Fields / Constructor ]

        #region [ Login / Logout ]

        public void HandleLogonEvent(object authenticatedUser)
        {
            DoLogin((AppUser)authenticatedUser);
            var deactivate = new ScreenEventArgs { Event = ScreenEventType.Deactivate, RegionName = RegionName.Body };
            EventManager.Screen_Event(deactivate);

        }

        public void HandleLogoffEvent(object authenticatedUser)
        {
            DoLogout();
        }

        public static void Login(AppUser authenticatedUser)
        {
            if (Instance == null) return;
            Instance.DoLogin(authenticatedUser);
        }

        public void DoLogin(AppUser authenticatedUser)
        {
            if (Entity == null)
            {
                Entity = authenticatedUser;
                LoadPermissions(authenticatedUser.Id);
                Instance = this;
            }
        }

        public static void Logout()
        {
            if (Instance == null) return;
            Instance.DoLogout();
        }

        private void DoLogout()
        {
            // make sure the contexts are cleared
//            var lookupContext = Container.Instance.Resolve<LookupContext>();

            // if this is NOT an automatic logout, make sure there are no context changes;
            // this is a safeguard; the user should not be able to logout when there are changes anyway.
            //if (_LogoutTimer != null)
               // if (_coreContext.HasChanges || lookupContext.HasChanges || employmentContext.HasChanges) return;

            StopActivityMonitor();

            // all good; clear everything
            //_coreContext.Addresses.Clear();


            AllowedActions = null;
            Entity = null;
            Instance = null;

            //_authContext.LogoutQuery();

            // take down screens
            var closeMainRegion = new ScreenEventArgs {Event = ScreenEventType.CloseInactiveScreens, RegionName = RegionName.Body};
            EventManager.Screen_Event(closeMainRegion);

            var closeOverlayActiveScreen = new ScreenEventArgs { Event = ScreenEventType.Deactivate, RegionName = RegionName.Overlay };
            EventManager.Screen_Event(closeOverlayActiveScreen);

            var closeOverlayRegionInactive = new ScreenEventArgs { Event = ScreenEventType.CloseInactiveScreens, RegionName = RegionName.Overlay };
            EventManager.Screen_Event(closeOverlayRegionInactive);

           EventManager.Screen_Activate("EntryScreen");

        }


        #endregion [ Login / Logout ]

        #region [ Permissions ]

        public static bool IsLoggedOn
        {
            get { return Entity != null; }
        }

        public static bool Can(ActionItem action)
        {
            if (Entity == null || AllowedActions == null) return false;
            return AllowedActions.Contains(action);
        }


        private void LoadPermissions(int id)
        {
            //Send<StartupProgressEvent>("Load Permitted Actions");
           // _coreContext.Load(_coreContext.GetPermissionsForUserQuery(id)).Completed += LoadPermissions_Completed;
        }

        //private void LoadPermissions_Completed(object sender, EventArgs e)
        //{
        //    // set allowed actions
        //    AllowedActions = new List<ActionItem>();
        //    foreach (UserPermission permission in _coreContext.UserPermissions)
        //    {
        //        AllowedActions.Add(permission.PermissionNumber);
        //    }
        //    Send<StartupProgressEvent>(AllowedActions.Count + " Permitted Actions Allowed");

        //    // the LogonEvent is handled by the LoginViewModel; it performs any post-login actions
        //    Send<LogonEvent>(null);

        //    InitializeActivityMonitor();
        //    StartAuthenticationCheckTimer();
        //}

        #endregion [ Permissions ]

        #region [ Navigation ]

        public static void GoHome()
        {
            if (Instance == null) return;
            Instance.DisplayUserHome();
        }

        private void DisplayUserHome()
        {
            //if (Entity != null)
            //{
            //    if (CurrentUser.Can(ActionItem.ViewVeteran))
            //    {
            //        Send<ScreenEvent, ScreenEventArgs>(new ScreenEventArgs { ScreenKey = ScreenKeyType.Home });
            //    }
            //    else if (CurrentUser.Can(ActionItem.ViewReports))
            //        Send<ScreenEvent, ScreenEventArgs>(new ScreenEventArgs { ScreenKey = ScreenKeyType.ReportSelection });
            //    else if (CurrentUser.Can(ActionItem.EditLookups))
            //        Send<ScreenEvent, ScreenEventArgs>(new ScreenEventArgs { ScreenKey = ScreenKeyType.AdEditLookup });
            //    else if (CurrentUser.Can(ActionItem.ManageUsers) || CurrentUser.Can(ActionItem.EditUserPermissions) || CurrentUser.Can(ActionItem.ResetPassword))
            //        Send<ScreenEvent, ScreenEventArgs>(new ScreenEventArgs { ScreenKey = ScreenKeyType.AddUser });
            //    else
            //        Send<ScreenEvent, ScreenEventArgs>(new ScreenEventArgs { ScreenKey = ScreenKeyType.ChangePswd });
            //}
        }

        #endregion [ Navigation ]

        #region [ Timers ]

        private void InitializeActivityMonitor()
        {
            // start logout timer
            int timeoutPeriod = 30; // Convert.ToInt16(AppSettings.Default("UserTimeoutMinutes"));
            _LogoutTimer = new DispatcherTimer { Interval = TimeSpan.FromMinutes(timeoutPeriod) };
            _LogoutTimer.Tick += LogoutTimerDing;
            _LogoutTimer.Start();

            // set up hooks for watching activity
            // this is used to determine user activity for the purpose of timing out after an idle period
            //_coreContext.PropertyChanged += Context_PropertyChanged;
            //_employmentContext.PropertyChanged += Context_PropertyChanged;
        }


        private void LogoutTimerDing(object sender, EventArgs e)
        {
            // timeout period has elapsed.  AppUser will be logged out
            _LogoutTimer.Stop();
            _LogoutTimer = null;
            //_coreContext.PropertyChanged -= Context_PropertyChanged;
            //_employmentContext.PropertyChanged -= Context_PropertyChanged;

            // log the user out.
            DoLogout();
        }

        private void Context_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // a change was made on the context, indicating the user is interacting with data
            // reset the timeout timer.
            _LogoutTimer.Start();
        }

        private void StartAuthenticationCheckTimer()
        {
            // start authentication check timer
            int timeoutPeriod = 30; // Convert.ToInt16(AppSettings.Default("AuthenticationCheckPeriodInMinutes"));
            _AuthenticationCheckTimer = new DispatcherTimer { Interval = TimeSpan.FromMinutes(timeoutPeriod) };
            _AuthenticationCheckTimer.Tick += AuthenticationCheckTimerDing;
            _AuthenticationCheckTimer.Start();
        }

        private void AuthenticationCheckTimerDing(object sender, EventArgs e)
        {
            // ping the server and make sure the user is still authenticated
            _authContext.CheckLogin().Completed += CheckLogin_Completed;
        }

        private void CheckLogin_Completed(object sender, EventArgs e)
        {
            var isAuthenticated = ((InvokeOperation<bool>)sender).Value;
            if (!isAuthenticated)
            {
                // Turn off the logout timer first, otherwise the logout method thinks the user initiated the logout.
                _LogoutTimer.Stop();
                _LogoutTimer = null;

                // the user is no longer authenticated; logout immediately.
                DoLogout();
                
                // use deferred execution to display the message box in 1 second
                // this gives the app a moment to refresh the screen after logging off.
                DeferredExecution(DisplayLostConnectionDialog, TimeSpan.FromSeconds(1));
            }
        }

        private void DisplayLostConnectionDialog(object sender, EventArgs e)
        {
            //MessageBox.Show(
            //    "You have been logged out due to a lost connection with the server.\n\nPlease login again. If this problem persists, please contact your administrator.",
            //    "Lost Connection",
            //    MessageBoxButton.OK);
        }

        private void StopActivityMonitor()
        {
            // stop listening to changes
            //_coreContext.PropertyChanged -= Context_PropertyChanged;
            //_employmentContext.PropertyChanged -= Context_PropertyChanged;

            // stop logout timer
            if (_LogoutTimer != null)
            {
                _LogoutTimer.Stop();
                _LogoutTimer = null;
            }

            // stop authenticationCheck timer
            if (_AuthenticationCheckTimer != null)
            {
                _AuthenticationCheckTimer.Stop();
                _AuthenticationCheckTimer = null;
            }
        }

        #endregion [ Timers ]

        #region [ Helpers ]


        private DispatcherTimer _workTimer;
        private EventHandler _callback;

        /// <summary>
        /// DeferredExecution executes the specified handler once, after the specifed interval elapses.
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="interval"></param>
        private void DeferredExecution(EventHandler handler, TimeSpan interval)
        {
            _callback = handler;
            _workTimer = new DispatcherTimer { Interval = interval };
            _workTimer.Tick += WorkTimer_Ding;
            _workTimer.Start();
        }

        void WorkTimer_Ding(object sender, EventArgs e)
        {
            _workTimer.Stop();
            _workTimer = null;
            _callback.Invoke(null, new EventArgs());
            _callback = null;
        }

        #endregion [ Helpers ]

    }
}
