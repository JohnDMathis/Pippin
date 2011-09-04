using System;
using System.Windows.Controls;
using Microsoft.Practices.Prism.Commands;

namespace Pippin.UI.Commands
{
    public class SliderCommandBehavior:CommandBehaviorBase<Slider>
    {
        public SliderCommandBehavior(Slider slider)
            : base(slider)
        {
            slider.ValueChanged += (s, e) =>
            {
                int val = Convert.ToInt32(Math.Round(e.NewValue));
                CommandParameter = val;
                ExecuteCommand();
            };
        }
    }
}

