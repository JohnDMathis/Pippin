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

namespace Pippin.UI.Commands
{
    public static class SliderValue
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(
            "Command", typeof(ICommand), typeof(SliderValue), new PropertyMetadata(OnSetCommandCallback));

        private static readonly DependencyProperty SliderCommandBehaviorProperty = DependencyProperty.RegisterAttached(
            "SliderCommandBehavior",
            typeof(SliderCommandBehavior),
        typeof(SliderValue),
        null);

        private static void OnSetCommandCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Slider slider = (Slider)d;
            if (slider == null) return;

            var behavior = GetOrCreateBehavior(slider);
        }

        private static SliderCommandBehavior GetOrCreateBehavior(Slider slider)
        {
            var behavior = slider.GetValue(SliderCommandBehaviorProperty) as SliderCommandBehavior;
            if (behavior == null)
            {
                behavior = new SliderCommandBehavior(slider);
                slider.SetValue(SliderCommandBehaviorProperty, behavior);
            }
            return behavior;
        }

        public static void SetCommand(Slider slider, ICommand command)
        {
            slider.SetValue(CommandProperty, command);
        }

        public static ICommand GetCommand(Slider slider)
        {
            return (ICommand)slider.GetValue(CommandProperty);
        }
    }
}

//    public static void SetCommand(ListBox selector, ICommand command)
//    {
//        selector.SetValue(CommandProperty, command);
//    }

//    /// <summary>
//    /// Retrieves the <see cref="ICommand"/> attached to the <see cref="TextBox"/>.
//    /// </summary>
//    /// <param name="selector">TextBox containing the Command dependency property</param>
//    /// <returns>The value of the command attached</returns>
//    public static ICommand GetCommand(ListBox selector)
//    {
//        return selector.GetValue(CommandProperty) as ICommand;
//    }

//}
