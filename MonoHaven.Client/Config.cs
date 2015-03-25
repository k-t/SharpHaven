using System;
using System.IO;
using Nini.Config;

namespace MonoHaven
{
	public class Config
	{
		private const string DefaultHost = "moltke.seatribe.se";
		private const int DefaultGamePort = 1870;
		private const int DefaultAuthPort = 1871;

		private readonly IConfig havenConfig;

		public Config()
		{
			var configSource = new IniConfigSource();
			configSource.AutoSave = true;

			havenConfig = configSource.AddConfig("haven");

			CreateOrLoadIniConfig(configSource);
			var commandLineConfigSource = LoadCommandLineConfig();
			configSource.Merge(commandLineConfigSource);
		}

		public string AuthHost
		{
			get { return havenConfig.Get("authsrv", DefaultHost); }
		}

		public int AuthPort
		{
			get { return DefaultAuthPort; }
		}


		public string GameHost
		{
			get { return havenConfig.Get("gamesrv", DefaultHost); }
		}

		public int GamePort
		{
			get { return DefaultGamePort; }
		}

		private static void CreateOrLoadIniConfig(IniConfigSource config)
		{
			var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			var configPath = Path.Combine(appDataPath, "monohaven", "settings.ini");
			try
			{
				config.Load(configPath);
			}
			catch (Exception)
			{
				Directory.CreateDirectory(Path.GetDirectoryName(configPath));
				config.Save(configPath);
			}
		}

		private static IConfigSource LoadCommandLineConfig()
		{
			var argvSource = new ArgvConfigSource(Environment.GetCommandLineArgs());
			argvSource.AddSwitch("haven", "gamesrv", "g");
			argvSource.AddSwitch("haven", "authsrv", "a");
			return argvSource;
		}
		
	}
}

