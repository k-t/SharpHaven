using System;
using System.Drawing;
using System.IO;
using Haven.Resources;
using NLog;
using OpenTK;
using OpenTK.Graphics;
using SharpHaven.Resources;

namespace SharpHaven
{
	public static class App
	{
		private static readonly Logger Log = LogManager.GetCurrentClassLogger();

		private static MainWindow window;

		public static void Main(string[] args)
		{
			Log.Info("Client started");

			AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
			GraphicsContext.ShareContexts = false;

			Config = new Config();
			Resources = new ResourceManager();

			using (var icon = LoadApplicationIcon())
			using (Audio = new Audio())
			using (window = new MainWindow(1024, 768))
			{
				window.Icon = icon;
				window.Run(60, 60);
			}
		}

		public static Audio Audio
		{
			get;
			private set;
		}

		public static Config Config
		{
			get;
			private set;
		}

		public static ResourceManager Resources
		{
			get;
			private set;
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

		private static Icon LoadApplicationIcon()
		{
			var res = Resources.Load("gfx/icon");
			var data = res.GetLayer<ImageLayer>();
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
