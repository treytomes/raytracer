using System;
using System.Windows;

namespace raytracer
{
	public partial class App : Application
	{
		public App()
		{
			Startup += App_Startup;
			DispatcherUnhandledException += App_DispatcherUnhandledException;
		}

		private void App_Startup(object sender, StartupEventArgs e)
		{
			MainWindow = new MainWindow();
			MainWindow.Show();
		}

		private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
		{
			MessageBox.Show(e.Exception.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			Shutdown(1);
		}

		[STAThread]
		public static int Main(string[] args)
		{
			var app = new App();
			return app.Run();
		}
	}
}
