using System;
using System.Linq;
using System.Windows;
using Microsoft.Silverlight.Testing;
using System.Windows.Input;
using System.Collections.Generic;

namespace TestRunner
{
    public partial class App : Application
    {

        public App()
        {
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;

            InitializeComponent();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            UnitTestSettings settings = UnitTestSystem.CreateDefaultSettings();
            settings.SortTestClasses = false;
            settings.SortTestMethods = false;
#if (!NOV_09_VERSION)
            ModifierKeys keys = Keyboard.Modifiers;
            bool altIsDown = (keys & ModifierKeys.Alt) != 0;
            if (altIsDown)
            {
                settings.SampleTags = new List<string>();
                settings.SampleTags.Add("Test Tag");
                settings.StartRunImmediately = false;
            }
            else
            {
                settings.StartRunImmediately = true;
            }
#endif
            var testAssemblies = Deployment.Current.Parts.Where(p => p.Source.Contains(".Specs")).ToList();
            foreach (var assembly in testAssemblies)
            {
                settings.TestAssemblies.Add(new AssemblyPart().Load(GetResourceStream(new Uri(assembly.Source, UriKind.Relative)).Stream));
            }

            RootVisual = UnitTestSystem.CreateTestPage(settings);
        }

        private void Application_Exit(object sender, EventArgs e)
        {

        }
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // If the app is running outside of the debugger then report the exception using
            // the browser's exception mechanism. On IE this will display it a yellow alert 
            // icon in the status bar and Firefox will display a script error.
            if (!System.Diagnostics.Debugger.IsAttached)
            {

                // NOTE: This will allow the application to continue running after an exception has been thrown
                // but not handled. 
                // For production applications this error handling should be replaced with something that will 
                // report the error to the website and stop the application.
                e.Handled = true;
                Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); });
            }
        }
        private void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
        {
            try
            {
                string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");");
            }
            catch (Exception)
            {
            }
        }
    }
}