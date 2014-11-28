using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Threading;
using NLog;
using OpenTK;
using OpenTK.Graphics;
using MonoHaven.Resources;

namespace MonoHaven
{
	public static class App
	{
		private static readonly Logger log = LogManager.GetCurrentClassLogger();

		private static Config config;
		private static ResourceManager resourceManager;
		private static MainWindow window;

		public static void Main(string[] args)
		{
			log.Info("Client started");

			AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
			SetSynchronizationContext();
			GraphicsContext.ShareContexts = false;

			config = new Config();
			resourceManager = new ResourceManager();

			using (var iconStream = new MemoryStream(Resources.GetImage("custom/ui/icon").Data))
			using (var iconImage = new Bitmap(iconStream))
			using (var icon = Icon.FromHandle(iconImage.GetHicon()))
			using (var gameWindow = new MainWindow(800, 600))
			{
				window = gameWindow;
				gameWindow.Icon = icon;
				gameWindow.Run(30, 60);
			}
		}

		public static Config Config
		{
			get { return config; }
		}

		public static ResourceManager Resources
		{
			get { return resourceManager; }
		}

		public static INativeWindow Window
		{
			get { return window; }
		}

		public static void QueueOnMainThread(Action action)
		{
			window.QueueUpdate(action);
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
