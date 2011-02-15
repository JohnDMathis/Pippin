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

namespace Pippin.UI.Behaviors
{
    
    public class UpdateOnChange:DependencyObject
    {


        public static bool GetEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnabledProperty);
        }

        public static void SetEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(EnabledProperty, value);
        }

        // Using a DependencyProperty as the backing store for Enabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnabledProperty =
            DependencyProperty.RegisterAttached("Enabled", typeof(bool), typeof(UpdateOnChange), new PropertyMetadata(IsEnabledPropertyChanged));


        private static void IsEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d.GetType() == typeof(PasswordBox))
                SetEnabledForPasswordBox((PasswordBox)d, (bool)e.NewValue);
            else
                SetEnabledForTextBox((TextBox)d, (bool)e.NewValue);
        }

        private static void SetEnabledForTextBox(TextBox textBox, bool enabled)
        {
            if (enabled)
                textBox.TextChanged += OnTextChanged;
            else
                textBox.TextChanged -= OnTextChanged;
        }

        private static void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb == null) return;
                var be = tb.GetBindingExpression(TextBox.TextProperty);
                if (be != null)
                    be.UpdateSource();
        }

        private static void SetEnabledForPasswordBox(PasswordBox passwordBox, bool enabled)
        {
            if (enabled)
                passwordBox.PasswordChanged += OnPasswordChanged;
            else
                passwordBox.PasswordChanged -= OnPasswordChanged;
        }

        private static void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            var pb = sender as PasswordBox;
            if (pb == null) return;
            var be = pb.GetBindingExpression(PasswordBox.PasswordProperty);
            if (be != null)
                be.UpdateSource();
        }


    }
}
