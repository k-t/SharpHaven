using System;
using System.Drawing;
using System.Threading;
using System.Windows.Threading;
using NLog;
using MonoHaven.Resources;

namespace MonoHaven
{
	internal class Program
	{
		private static Logger Log = LogManager.GetCurrentClassLogger();

		public static void Main(string[] args)
		{
			Log.Info("Client started");

			AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
			SetSynchronizationContext();

			using (var iconImage = EmbeddedResource.GetImage("icosn.png"))
			using (var icon = Icon.FromHandle(iconImage.GetHicon()))
			using (var gameWindow = new MainWindow(800, 600))
			{
				gameWindow.Icon = icon;
				gameWindow.Run();
			}
		}

		private static void SetSynchronizationContext()
		{
			SynchronizationContext.SetSynchronizationContext(
				new DispatcherSynchronizationContext());
		}

		private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Log.Fatal((Exception)e.ExceptionObject);
			// flush log before the termination (otherwise it can be empty)
			LogManager.Flush();
		}
	}
}
