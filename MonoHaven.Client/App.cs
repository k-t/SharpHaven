using System;
using System.Drawing;
using System.Threading;
using System.Windows.Threading;
using NLog;
using MonoHaven.UI;
using MonoHaven.Resources;

namespace MonoHaven
{
	public class App
	{
		private static readonly Logger log = LogManager.GetCurrentClassLogger();
		private static readonly App instance = new App();

		public static void Main(string[] args)
		{
			log.Info("Client started");

			AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
			SetSynchronizationContext();

			using (var iconImage = EmbeddedResource.GetImage("icon.png"))
			using (var icon = Icon.FromHandle(iconImage.GetHicon()))
			using (var gameWindow = new MainWindow(800, 600))
			{
				instance.Config = new Config();
				instance.Resources = new ResourceManager();
				instance.Window = gameWindow;

				gameWindow.Icon = icon;
				gameWindow.Run(30, 60);
			}
		}

		public static App Instance
		{
			get { return instance; }
		}

		public Config Config
		{
			get;
			private set;
		}

		public ResourceManager Resources
		{
			get;
			private set;
		}

		public IScreenHost Window
		{
			get;
			private set;
		}

		private static void SetSynchronizationContext()
		{
			SynchronizationContext.SetSynchronizationContext(
				new DispatcherSynchronizationContext());
		}

		private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			log.Fatal((Exception)e.ExceptionObject);
			// flush log before the termination (otherwise it can be empty)
			LogManager.Flush();
		}
	}
}
