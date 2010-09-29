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
using System.ComponentModel.Composition;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Pippin.UI.Regions
{
    public interface IRegionManager
    {
        ObservableCollection<Lazy<UserControl, ITargetRegionCapabilities>> Controls { get; set; }
    }

    [Export]
    public class RegionManager
    {
        /// <summary>
        ///     Controls
        /// </summary>
        private readonly List<UserControl> _controls = new List<UserControl>();

        /// <summary>
        ///     The controls
        /// </summary>
        [ImportMany(AllowRecomposition = true)]
        public ObservableCollection<Lazy<UserControl, ITargetRegionCapabilities>> Controls { get; set; }

        /// <summary>
        ///     Constructor, set it all up
        /// </summary>
        public RegionManager()
        {
            Controls = new ObservableCollection<Lazy<UserControl, ITargetRegionCapabilities>>();
            Controls.CollectionChanged += Controls_CollectionChanged;
        }

        /// <summary>
        ///     When the collection changes, inspect it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Controls_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    var controlInfo = item as Lazy<UserControl, ITargetRegionCapabilities>;

                    if (controlInfo != null)
                    {
                        if (!_controls.Contains(controlInfo.Value))
                        {
                            ViewRegion region = controlInfo.Metadata.Region;
                            Panel panel = RegionDef.GetPanelForRegion(region);
                            panel.Children.Add(controlInfo.Value);
                            _controls.Add(controlInfo.Value);
                        }
                    }
                }
            }
        }
    }
}
