#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

using System;
using System.Drawing;
using System.Threading;
using System.Windows.Threading;
using NLog;
using OpenTK;
using OpenTK.Graphics;
using MonoHaven.Resources;

namespace MonoHaven
{
	public class App
	{
		private static readonly Logger log = LogManager.GetCurrentClassLogger();
		private static readonly App instance = new App();

		private Config config;
		private ResourceManager resourceManager;
		private MainWindow window;

		public static void Main(string[] args)
		{
			log.Info("Client started");

			AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
			SetSynchronizationContext();
			GraphicsContext.ShareContexts = false;

			using (var iconImage = EmbeddedResource.GetImage("icon.png"))
			using (var icon = Icon.FromHandle(iconImage.GetHicon()))
			using (var gameWindow = new MainWindow(800, 600))
			{
				instance.config = new Config();
				instance.resourceManager = new ResourceManager();
				instance.window = gameWindow;

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
			get { return config; }
		}

		public ResourceManager Resources
		{
			get { return resourceManager; }
		}

		public INativeWindow Window
		{
			get { return window; }
		}

		public void QueueOnMainThread(Action action)
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
