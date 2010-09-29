using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Presentation.Events;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Composite.Regions;
using Odin.UI.Infrastructure.Constants;
using Odin.UI.Infrastructure.ViewModel;
using Odin.UI.Infrastructure.VisibilityService;

namespace Odin.UI.Infrastructure.ScreenFramework
{
    public class ScreenConductor:EventAggregatorClient
    {

        protected IUnityContainer Container { get; set; }
        protected IScreenFactoryRegistry ScreenFactoryRegistry { get; set; }
        protected IRegionManager RegionManager { get; set; }
        protected IVisibilityService VisibilityService { get; set; }
        protected IDictionary<ScreenKeyType, IScreen> ScreenCollection { get; set; }
        protected IRegion Region { get; set; }
        protected RegionName MyRegionName { get; set; }
        protected ScreenKeyType activeScreenKey { get; set; }

        public ScreenConductor(IUnityContainer container, 
            IScreenFactoryRegistry screenFactoryRegistry, 
            IEventAggregator eventAggregator, 
            IRegionManager regionManager, 
            IVisibilityService visibilityService)
        {
            this.activeScreenKey = ScreenKeyType.None;

            this.Container = container;
            this.ScreenFactoryRegistry = screenFactoryRegistry;
            this.EventManager = eventAggregator;
            this.RegionManager = regionManager;
            this.VisibilityService = visibilityService;
            
            this.ScreenCollection = new Dictionary<ScreenKeyType, IScreen>();
            SubscribeToEvents();
        }


        #region Private Methods

        protected void SubscribeToEvents()
        {
            this.EventManager.GetEvent<ScreenEvent>().Subscribe(
                    HandleScreenEvent,
                    ThreadOption.UIThread,
                    true,
                    IsNotificationRelevant);
        }

        protected bool IsNotificationRelevant(ScreenEventArgs args)
        {
            //TODO: PAPA :-)
            return true;
        }

        protected virtual void HandleScreenEvent(ScreenEventArgs args)
        {
            // check the screen's region.  If it is not "my" region, then ignore the request
            if (args.RegionName != MyRegionName) return;

            if (args.Event == ScreenEventType.Activate)
                ActivateScreen(args);
            else if (args.Event == ScreenEventType.Deactivate)
                DeactivateScreen();
            else if (args.Event == ScreenEventType.CloseInactiveScreens)
                CloseInactiveScreens();
        }
        private void ActivateScreen(ScreenEventArgs args){

            // check if there is no such registered screen type. 
            if (!this.ScreenFactoryRegistry.HasFactory(args.ScreenKey))
            {
                // the most likely reason is that the screen's module is not loaded.
                // check again in 2 seconds
                HandleScreenEventRetry(args);
                return;
            }

            // Check if an active screen exists. 
            if (this.ScreenCollection.ContainsKey(this.activeScreenKey))
            {
                // Get the currently active screen
                IScreen activeScreen = this.ScreenCollection[this.activeScreenKey];
                // Check if we can leave
                if (activeScreen.CanLeave())
                {
                    IScreen screen = this.ScreenCollection[this.activeScreenKey];
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
                // no active screen exists
                PrepareScreen(args.ScreenKey, args.ScreenSubject);
                ShowScreen(args.ScreenKey, args.UseAnimation);
            }
        }

        private DispatcherTimer _timer;
        private List<ScreenEventArgs> _jobQueue;

        private void HandleScreenEventRetry(ScreenEventArgs args)
        {
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
            _timer.Stop();
            if (_jobQueue.Count > 0)
            {
                ScreenEventArgs job = _jobQueue[0];
                _jobQueue.RemoveAt(0);
                HandleScreenEvent(job);
            }
            // more jobs in queue?
            if (_jobQueue.Count == 0)
                _timer = null;
            else
                _timer.Start();
        }

        private void SwitchScreens(IScreen oldScreen, ScreenEventArgs newScreenArgs)
        {
            if (oldScreen != null && Region.Views.Contains(oldScreen.View))
            {
                oldScreen.Leaving();
                Region.Deactivate(oldScreen.View);
                Region.Remove(oldScreen.View);
            }
            PrepareScreen(newScreenArgs.ScreenKey,newScreenArgs.ScreenSubject);
            ShowScreen(newScreenArgs.ScreenKey, newScreenArgs.UseAnimation);
        }

        private void DeactivateScreen()
        {
            IScreen activeScreen;
            try
            {
                activeScreen = this.ScreenCollection[this.activeScreenKey];
            }
            catch (Exception)
            {
                activeScreen = null;
            }

            // Check if we can leave
            if (activeScreen !=null && activeScreen.CanLeave())
            {
                IScreen screen = this.ScreenCollection[this.activeScreenKey];
                
                this.VisibilityService.LeaveViewAnimation(screen.View, () =>
                                                                           {
                                                                               Region.Deactivate(screen.View);
                                                                               Region.Remove(screen.View);
                                                                           });
                activeScreenKey = ScreenKeyType.None;
            }
        }

        private IScreen PrepareScreen(ScreenKeyType screenKey, object screenSubject)
        {
            IScreen screen = null;
            // use the screen type to see if the screen exists in the collection
            if (ScreenCollection.ContainsKey(screenKey))
            {
                screen = ScreenCollection[screenKey];
                screen.Subject = screenSubject;
            }
            else // if it does not, then use the screen type to get the factory that is made for creating that type of screen and make it, add to collection
            {
                if (this.ScreenFactoryRegistry.HasFactory(screenKey))
                {
                    screen = this.ScreenFactoryRegistry.Get(screenKey).CreateScreen(screenSubject);
                    ScreenCollection.Add(screenKey, screen);
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Screen Key not found");
                }
            }
            screen.Setup();
            return screen;
        }

        private void ShowScreen(ScreenKeyType screenKey)
        {
            ShowScreen(screenKey,true);
        }
        private void ShowScreen(ScreenKeyType screenKey, bool useAnimation)
        {
            if (!this.ScreenCollection.ContainsKey(screenKey)) return;

            IScreen screen = this.ScreenCollection[screenKey];
            this.activeScreenKey = screenKey;
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
        }

        private void CloseInactiveScreens()
        {
            // remove the screen from the screen collection
            var keys = ScreenCollection.Keys.ToList();
            foreach (ScreenKeyType key in keys)
            {
                IScreen screen = ScreenCollection[key];
                screen.Leaving();
                ScreenCollection.Remove(key);
            }
        }

        #endregion
    }
}