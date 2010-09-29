using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Odin.UI.Infrastructure.VisibilityService
{
    public class VisibilityService : IVisibilityService
    {
        #region Properties

        private Storyboard EnterViewVisibilityStoryboard { get; set; }
        private Storyboard LeaveViewVisibilityStoryboard { get; set; }

        #endregion

        #region Constructors

        public VisibilityService()
        {
        }

        #endregion
        
        #region Begin Animation Methods
        
        public void EnterViewAnimation(UserControl view)
        {
            //return;
            this.EnterViewVisibilityStoryboard = new Storyboard();
            PerformAnimation(view, this.EnterViewVisibilityStoryboard);
        }


        public void LeaveViewAnimation(UserControl view, Action onLeaveComplete)
        {
            this.LeaveViewVisibilityStoryboard = new Storyboard();
            this.LeaveViewVisibilityStoryboard.Completed += new EventHandler((sender, e) => onLeaveComplete());
            PerformAnimation(view, this.LeaveViewVisibilityStoryboard);
          //  this.LeaveViewVisibilityStoryboard.Completed -= new EventHandler((sender, e) => onLeaveComplete());
        }

        public void LeaveViewAnimation(UserControl view)
        {
            this.LeaveViewVisibilityStoryboard = new Storyboard();
            PerformAnimation(view, this.LeaveViewVisibilityStoryboard);
        }

        #endregion

        #region Private Helper Methods

        private void PerformAnimation(UserControl view, Storyboard storyboard)
        {
            bool enterAnimation = (storyboard == this.EnterViewVisibilityStoryboard);

            const string enterKey = "EnterViewVisibilityStoryboard";
            const string leaveKey = "LeaveViewVisibilityStoryboard";
            var key = enterAnimation ? enterKey : leaveKey;
            view.Opacity = enterAnimation ? 0 : 1;
            //Storyboard storyboard = new Storyboard();
            var transitionTo = TimeSpan.FromMilliseconds(250);

            if (view.Resources.Contains(key))
                view.Resources.Remove(key);

            if (!view.Resources.Contains(key))
            {
                SetupSequencedRenderTransform(view);
                view.RenderTransformOrigin = new Point(.5, .5);
                SetupScaleAnimation(storyboard, enterAnimation, view, transitionTo);
                SetupOpacityAnimation(storyboard, enterAnimation, view, transitionTo);

                #region Begin the Animation

                if (enterAnimation)
                {
                    this.EnterViewVisibilityStoryboard = storyboard;
                    view.Resources.Add(key, this.EnterViewVisibilityStoryboard);
                    //view.Loaded += ((sender, e) => this.EnterViewVisibilityStoryboard.Begin());
                    this.EnterViewVisibilityStoryboard.Begin();
                }
                else
                {
                    this.LeaveViewVisibilityStoryboard = storyboard;
                    view.Resources.Add(key, this.LeaveViewVisibilityStoryboard);
                    this.LeaveViewVisibilityStoryboard.Begin();
                }

                #endregion
            }
        }

        private void SetupScaleAnimation(Storyboard storyboard, bool enterAnimation, UserControl view, TimeSpan transitionTo)
        {
            var scaleFrom = enterAnimation ? .5 : 1;
            var scaleTo = enterAnimation ? 1 : .5;

            // Scale X
            DoubleAnimation daScaleTransformX = new DoubleAnimation();
            storyboard.Children.Add(daScaleTransformX);
            daScaleTransformX.From = scaleFrom;
            daScaleTransformX.To = scaleTo;
            daScaleTransformX.Duration = new Duration(transitionTo);
            Storyboard.SetTarget(daScaleTransformX, view);
            Storyboard.SetTargetProperty(daScaleTransformX,
                                         new PropertyPath(
                                             "(View.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)"));

            // Scale Y
            DoubleAnimation daScaleTransformY = new DoubleAnimation();
            storyboard.Children.Add(daScaleTransformY);
            daScaleTransformY.From = scaleFrom;
            daScaleTransformY.To = scaleTo;
            daScaleTransformY.Duration = new Duration(transitionTo);
            Storyboard.SetTarget(daScaleTransformY, view);
            Storyboard.SetTargetProperty(daScaleTransformY,
                                         new PropertyPath(
                                             "(View.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)"));
        }

        private void SetupOpacityAnimation(Storyboard storyboard, bool enterAnimation, UserControl view, TimeSpan transitionTo)
        {
            var showDuration = new Duration(transitionTo);
            var opacityFrom = enterAnimation ? 0 : 1;
            var opacityTo = enterAnimation ? 1 : 0;
            var showDoubleAnimation = new DoubleAnimation { Duration = showDuration, From = opacityFrom, To = opacityTo };
            storyboard.Children.Add(showDoubleAnimation);
            Storyboard.SetTarget(showDoubleAnimation, view);
            Storyboard.SetTargetProperty(showDoubleAnimation, new PropertyPath("(View.Opacity)"));
        }

        private void SetupSequencedRenderTransform(UserControl view)
        {
            if (view.RenderTransform == null)
            {
                view.RenderTransform = CreateTransformGroup(1);
            }
            else
            {
                if (view.RenderTransform.GetType() == typeof(TransformGroup))
                {
                    TransformGroup tg = CreateTransformGroup(1);
                    for (int i = 0; i < (view.RenderTransform as TransformGroup).Children.Count; i++)
                    {
                        if ((view.RenderTransform as TransformGroup).Children[i].GetType() ==
                            typeof(TranslateTransform))
                        {
                            (tg.Children[i] as TranslateTransform).X =
                                ((view.RenderTransform as TransformGroup).Children[i] as TranslateTransform).X;
                            (tg.Children[i] as TranslateTransform).Y =
                                ((view.RenderTransform as TransformGroup).Children[i] as TranslateTransform).Y;
                        }
                        else if ((view.RenderTransform as TransformGroup).Children[i].GetType() ==
                                 typeof(ScaleTransform))
                        {
                            (tg.Children[i] as ScaleTransform).ScaleX =
                                ((view.RenderTransform as TransformGroup).Children[i] as ScaleTransform).ScaleX;
                            (tg.Children[i] as ScaleTransform).ScaleY =
                                ((view.RenderTransform as TransformGroup).Children[i] as ScaleTransform).ScaleY;
                        }
                        //else if ((View.RenderTransform as TransformGroup).Children[i].GetType() == typeof(RotateTransform)){
                        //    (tg.Children[2] as RotateTransform).Angle = ((View.RenderTransform as TransformGroup).Children[i] as RotateTransform).Angle;
                        //}
                    }
                    view.RenderTransform = tg;
                }
                else
                {
                    view.RenderTransform = CreateTransformGroup(1);
                }
            }
        }

        private TransformGroup CreateTransformGroup(double scale) // double x, double y, double angle, 
        {
            var transformGroup = new TransformGroup();
            var scaleTransform = new ScaleTransform {ScaleX = scale, ScaleY = scale};
            transformGroup.Children.Add(scaleTransform);

            //var translateTransform = new TranslateTransform {X = x, Y = y};
            //transformGroup.Children.Add(translateTransform);

            //var rotateTransform = new RotateTransform {CenterX = translateTransform.X, CenterY = translateTransform.Y, Angle = angle};
            //transformGroup.Children.Add(rotateTransform);

            return transformGroup;
        }

        #endregion
    }
}