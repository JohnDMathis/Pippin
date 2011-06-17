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
using Microsoft.Practices.Unity;
using Pippin.UI.Screens;

namespace MI.Authentication.Screens
{
    public class EntryScreenFactory:ScreenFactoryBase
    {
        public EntryScreenFactory(IUnityContainer container) : base(container) { }

        public override IScreen CreateScreen(object screenSubject)
        {
            Screen = Container.Resolve<EntryScreen>();
            return Screen;
        }
    }
}
