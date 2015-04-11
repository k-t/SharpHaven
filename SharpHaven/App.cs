using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using NLog;
using OpenTK;
using OpenTK.Graphics;
using SharpHaven.Resources;

namespace SharpHaven
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
			using (var icon = GetApplicationIcon())
			using (var gameWindow = new MainWindow(1024, 768))
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
		
		public static void Exit()
		{
			window.Close();
		}

		private static void SetSynchronizationContext()
		{
			SynchronizationContext.SetSynchronizationContext(
				new WindowsFormsSynchronizationContext());
		}

		private static Icon GetApplicationIcon()
		{
			var res = Resources.Load("custom/ui/icon");
			var data = res.GetLayer<ImageData>();
			return new Icon(new MemoryStream(data.Data));
		}

		private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Log.Fatal((Exception)e.ExceptionObject);
			// flush log before the termination (otherwise it can be empty)
			LogManager.Flush();
		}
	}
}
