using System;
using System.IO;
using Nini.Config;

namespace SharpHaven
{
	public class Config
	{
		private const string DefaultHost = "legacy.havenandhearth.com";
		private const int DefaultGamePort = 1870;
		private const int DefaultAuthPort = 1871;
		private const string DefaultMapUrl = "http://legacy.havenandhearth.com/mm/";
		private const string DefaultResUrl = "http://legacy.havenandhearth.com/res/";

		private readonly IConfig havenConfig;

		public Config()
		{
			var configSource = new IniConfigSource();
			configSource.AutoSave = true;

			CreateOrLoadIniConfig(configSource);
			havenConfig = configSource.Configs["haven"] ?? configSource.AddConfig("haven");

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

		public byte[] AuthToken
		{
			get
			{
				var encoded = havenConfig.Get("authtoken", "");
				return string.IsNullOrEmpty(encoded)
					? null
					: Convert.FromBase64String(encoded);
			}
			set
			{
				var decoded = value != null ? Convert.ToBase64String(value) : "";
				havenConfig.Set("authtoken", decoded);
			}
		}

		public string UserName
		{
			get { return havenConfig.Get("username", ""); }
			set { havenConfig.Set("username", value); }
		}

		public string GameHost
		{
			get { return havenConfig.Get("gamesrv", DefaultHost); }
		}

		public int GamePort
		{
			get { return DefaultGamePort; }
		}

		public string MapUrl
		{
			get { return havenConfig.Get("mapurl", DefaultMapUrl); }
		}

		public string ResUrl
		{
			get { return havenConfig.Get("resurl", DefaultResUrl); }
		}

		private static void CreateOrLoadIniConfig(IniConfigSource config)
		{
			var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			var configPath = Path.Combine(appDataPath, "sharphaven", "settings.ini");
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

