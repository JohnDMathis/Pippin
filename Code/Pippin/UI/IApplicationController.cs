using System.Windows;

namespace Pippin.UI
{
    public interface IApplicationController
    {
        void Startup();
        void ConfigureScreenFramework();
        void RegisterServices();
       // void UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e);
    }
}