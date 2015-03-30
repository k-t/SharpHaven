using System;
using System.Drawing;
using System.Threading;
using System.Windows.Threading;
using MonoHaven.Resources;
using NLog;
using OpenTK;
using OpenTK.Graphics;

namespace MonoHaven
{
	public static class App
	{
		private static readonly Logger Log = LogManager.GetCurrentClassLogger();

		private static Audio audio;
		private static Config config;
		private static ResourceManager resourceManager;
		private static MainWindow window;

		public static void Main(string[] args)
		{
			Log.Info("Client started");

			AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
			SetSynchronizationContext();
			GraphicsContext.ShareContexts = false;

			config = new Config();
			resourceManager = new ResourceManager();

			using (audio = new Audio())
			using (var iconImage = Resources.Get<Bitmap>("custom/ui/icon"))
			using (var icon = Icon.FromHandle(iconImage.GetHicon()))
			using (var gameWindow = new MainWindow(800, 600))
			{
				window = gameWindow;
				gameWindow.Icon = icon;
				gameWindow.Run(60, 60);
			}
		}

		public static Audio Audio
		{
			get { return audio; }
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
			Log.Fatal((Exception)e.ExceptionObject);
			// flush log before the termination (otherwise it can be empty)
			LogManager.Flush();
		}
	}
}
