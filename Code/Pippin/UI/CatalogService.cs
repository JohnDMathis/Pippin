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
using System.Windows.Resources;
using System.Reflection;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.Hosting;

namespace Pippin.UI
{
    public class CatalogService:ICatalogService
    {
        private readonly AggregateCatalog _catalog = new AggregateCatalog();

        public CatalogService()
        {
            Add(new DeploymentCatalog());
        }

        public void Add(ComposablePartCatalog catalog)
        {
            _catalog.Catalogs.Add(catalog);
        }

        public AggregateCatalog GetCatalog()
        {
            return _catalog;
        }

    }
}
