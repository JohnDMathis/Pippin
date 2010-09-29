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
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.Hosting;

namespace Pippin.UI
{
    public interface ICatalogService
    {
        void Add(ComposablePartCatalog catalog);
        AggregateCatalog GetCatalog();
    }
}
