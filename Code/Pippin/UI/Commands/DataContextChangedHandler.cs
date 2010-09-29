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
using System.Windows.Interactivity;
using System.Windows.Data;

namespace Pippin.UI.Commands
{
    // Borrowed from Jeremy Likeness
    // See http://csharperimage.jeremylikness.com/2010/02/mef-instead-of-prism-for-silverlight-3.html

    public static class DataContextChangedHandler
    {
        private const string INTERNAL_CONTEXT = "InternalDataContext";
        private const string CONTEXT_CHANGED = "DataContextChanged";

        public static readonly DependencyProperty InternalDataContextProperty =
            DependencyProperty.Register(INTERNAL_CONTEXT,
                                        typeof(Object),
                                        typeof(FrameworkElement),
                                        new PropertyMetadata(_DataContextChanged));

        public static readonly DependencyProperty DataContextChangedProperty =
            DependencyProperty.Register(CONTEXT_CHANGED,
                                        typeof(Action<Control>),
                                        typeof(FrameworkElement),
                                        null);


        private static void _DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var control = (Control)sender;
            var handler = (Action<Control>)control.GetValue(DataContextChangedProperty);
            if (handler != null)
            {
                handler(control);
            }
        }

        public static void Bind(Control control, Action<Control> dataContextChanged)
        {
            control.SetBinding(InternalDataContextProperty, new Binding());
            control.SetValue(DataContextChangedProperty, dataContextChanged);
        }
    }
}
