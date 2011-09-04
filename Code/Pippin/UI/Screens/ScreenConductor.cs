using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Regions;
using Pippin.UI.ViewModel;
using Pippin.UI.VisibilityServices;
using Pippin.UI.Regions;
using Pippin.UI.Events;
using Microsoft.Practices.Prism.Logging;

namespace Pippin.UI.Screens
{
    public class ScreenConductor : IScreenConductor
    {

        protected IUnityContainer Container { get; set; }
        protected IScreenFactoryRegistry ScreenFactoryRegistry { get; set; }
        protected IRegionManager RegionManager { get; set; }
        protected IVisibilityService VisibilityService { get; set; }
        protected IDictionary<string, IScreen> ScreenCollection { get; set; }
        protected IRegion Region { get; set; }
        protected RegionName MyRegionName { get; set; }
        protected IEventAggregator _eventManager { get; private set; }
        protected string _activeScreenName { get; set; }
        protected ILoggerFacade _logger{get;set;}

        public ScreenConductor(IUnityContainer container, 
            IScreenFactoryRegistry screenFactoryRegistry, 
            IEventAggregator eventAggregator, 
            IRegionManager regionManager, 
            IVisibilityService visibilityService
            )
        {
            this._activeScreenName = "";

            this.Container = container;
            this.ScreenFactoryRegistry = screenFactoryRegistry;
            _eventManager = eventAggregator;
            this.RegionManager = regionManager;
            this.VisibilityService = visibilityService;
            _logger = Container.Resolve<ILoggerFacade>();
            this.ScreenCollection = new Dictionary<string, IScreen>();
            SubscribeToEvents();
        }


        #region Private Methods

        protected void Log(string message)
        {
            _logger.Log("ScreenConductor<"+MyRegionName+">; "+message, Category.Debug, Priority.High);
        }

        protected void SubscribeToEvents()
        {
            _eventManager.GetEvent<ScreenEvent>().Subscribe(
                    HandleScreenEvent,
                    ThreadOption.UIThread,
                    true,
                    IsNotificationRelevant);
        }

        protected bool IsNotificationRelevant(ScreenEventArgs args)
        {
            return true;
        }

        protected virtual void HandleScreenEvent(ScreenEventArgs args)
        {
            // check the event type for deactivate, because
            // check the screen's region.  If it is not "my" region, then ignore the request
            if (args.RegionName != MyRegionName) return;
            Log("HandleScreenEvent; handling screen=" + args.ScreenName + ",  event="+args.Event);
            if (args.Event == ScreenEventType.Activate)
                ActivateScreen(args);
            else if (args.Event == ScreenEventType.Deactivate)
                DeactivateScreen(args.ScreenName);
            else if (args.Event == ScreenEventType.CloseInactiveScreens)
                CloseInactiveScreens();
        }
        private void ActivateScreen(ScreenEventArgs args){
            Log("ActivateScreen(); " + args.ScreenName);
            // check if there is no such registered screen type. 
            if (!this.ScreenFactoryRegistry.HasFactory(args.ScreenName))
            {
                // the most likely reason is that the screen's module is not loaded.
                // check again in 2 seconds
                Log(string.Format("Requested screen '{0}' is not available. Verify it is registered.", args.ScreenName));
                HandleScreenEventRetry(args);
                return;
            }

            // Check if an active screen exists. 
            if (this.ScreenCollection.ContainsKey(this._activeScreenName))
            {
                // Get the currently active screen
                IScreen activeScreen = this.ScreenCollection[this._activeScreenName];
                // Check if we can leave
                if (activeScreen.CanLeave())
                {
                    IScreen screen = this.ScreenCollection[this._activeScreenName];
                    if (args.UseAnimation)
                        VisibilityService.LeaveViewAnimation(screen.View, () => SwitchScreens(screen, args));
                    else
                        SwitchScreens(screen, args);
                }
                else
                    activeScreen.LeaveCanceled(); 
            }
            else
            {
                Log("call PrepareScreen()");
                // no active screen exists
                PrepareScreen(args.ScreenName, args.ScreenSubject);
                ShowScreen(args.ScreenName, args.UseAnimation);
            }
        }

        private DispatcherTimer _timer;
        private List<ScreenEventArgs> _jobQueue;

        private void HandleScreenEventRetry(ScreenEventArgs args)
        {
            Log("HandleScreenEventRetry(); screenName="+args.ScreenName);
            // create a dispatcherTimer to call activate screen
            if (_jobQueue == null) _jobQueue = new List<ScreenEventArgs>();
            _jobQueue.Add(args);
            if (_timer == null)
            {
                _timer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(2)};
                _timer.Tick += TimerDing;
                _timer.Start();
            }
        }

        private void TimerDing(object sender, EventArgs e)
        {
            Log("TimerDing(); Jobs=" + _jobQueue.Count);
            _timer.Stop();
            if (_jobQueue.Count > 0)
            {
                ScreenEventArgs job = _jobQueue[0];
                Log("TimerDing(); removing 1;" + job.ScreenName);
                _jobQueue.RemoveAt(0);
                HandleScreenEvent(job);
            }
            // more jobs in queue?
            if (_jobQueue.Count == 0)
            {
                _timer = null;
            }
        }

        private void SwitchScreens(IScreen oldScreen, ScreenEventArgs newScreenArgs)
        {
            if (oldScreen != null && Region.Views.Contains(oldScreen.View))
            {
                oldScreen.Leaving();
                Region.Deactivate(oldScreen.View);
                Region.Remove(oldScreen.View);
            }
            PrepareScreen(newScreenArgs.ScreenName,newScreenArgs.ScreenSubject);
            ShowScreen(newScreenArgs.ScreenName, newScreenArgs.UseAnimation);
        }

        public void DeactivateScreen(string screenName)
        {
            IScreen screen;
            try
            {
                screen = this.ScreenCollection[screenName];
            }
            catch (Exception)
            {
                screen = null;
            }

            // Check if we can leave
            if (screen != null)
                DeactivateScreen(screen);
        }

        public void DeactivateScreen()
        {
            DeactivateScreen(_activeScreenName);
        }

        public void DeactivateScreen(IScreen screen)
        {
            if (screen != null && screen.CanLeave())
            {
                this.VisibilityService.LeaveViewAnimation(screen.View, () =>
                {
                    Region.Deactivate(screen.View);
                    Region.Remove(screen.View);
                    ScreenDeactivated(screen.Name);
                    screen.Conductor = null;
                });
                if (screen.Name == _activeScreenName)
                    _activeScreenName = "";
            }
        }
        private IScreen PrepareScreen(string screenName, object screenSubject)
        {
            IScreen screen = null;
            // use the screen type to see if the screen exists in the collection
            if (ScreenCollection.ContainsKey(screenName))
            {
                screen = ScreenCollection[screenName];
                screen.Subject = screenSubject;
            }
            else // if it does not, then use the screen type to get the factory that is made for creating that type of screen and make it, add to collection
            {
                if (this.ScreenFactoryRegistry.HasFactory(screenName))
                {
                    screen = this.ScreenFactoryRegistry.Get(screenName).CreateScreen(screenSubject);
                    ScreenCollection.Add(screenName, screen);
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Screen Key not found");
                }
            }
            screen.Setup();
            return screen;
        }

        private void ShowScreen(string screenName)
        {
            ShowScreen(screenName,true);
        }
        private void ShowScreen(string screenName, bool useAnimation)
        {
            if (!this.ScreenCollection.ContainsKey(screenName)) return;

            IScreen screen = this.ScreenCollection[screenName];
            this._activeScreenName = screenName;
            if (!Region.Views.Contains(screen.View))
                Region.Add(screen.View);
            Region.Activate(screen.View);
            if (useAnimation)
                this.VisibilityService.EnterViewAnimation(screen.View);
            else
            {
                screen.View.Visibility = Visibility.Visible;
                screen.View.Opacity = 1.0;
                screen.View.RenderTransform = new ScaleTransform {ScaleX = 1, ScaleY = 1};
            }
            screen.Activated(this, screenName);
            ScreenActivated(screenName);
        }

        private void CloseInactiveScreens()
        {
            // remove the screen from the screen collection
            var keys = ScreenCollection.Keys.ToList();
            foreach (string key in keys)
            {
                IScreen screen = ScreenCollection[key];
                screen.Leaving();
                ScreenCollection.Remove(key);
            }
        }

        #endregion

        protected virtual void ScreenActivated(string screenName)
        {
        }
        protected virtual void ScreenDeactivated(string screenName)
        {
        }
    }
}