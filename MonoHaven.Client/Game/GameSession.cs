using System;
using MonoHaven.Network;
using MonoHaven.Resources;
using MonoHaven.UI;
using NLog;

namespace MonoHaven.Game
{
	public class GameSession
	{
		#region Constants

		private const int RMSG_NEWWDG = 0;
		private const int RMSG_WDGMSG = 1;
		private const int RMSG_DSTWDG = 2;
		private const int RMSG_MAPIV = 3;
		private const int RMSG_GLOBLOB = 4;
		private const int RMSG_PAGINAE = 5;
		private const int RMSG_RESID = 6;
		private const int RMSG_PARTY = 7;
		private const int RMSG_SFX = 8;
		private const int RMSG_CATTR = 9;
		private const int RMSG_MUSIC = 10;
		private const int RMSG_TILES = 11;
		private const int RMSG_BUFF = 12;

		#endregion

		private static readonly Logger log = LogManager.GetCurrentClassLogger();

		private readonly Connection connection;
		private readonly GameState state;
		private readonly GameScreen screen;

		public GameSession(ConnectionSettings connSettings)
		{
			connection = new Connection(connSettings);
			connection.MessageReceived += OnMessageReceived;
			connection.Closed += OnConnectionClosed;
			state = new GameState();
			screen = new GameScreen(state);
		}

		public IScreen Screen
		{
			get { return screen; }
		}

		public void Start()
		{
			connection.Open();
		}

		public void Finish()
		{
			connection.Closed -= OnConnectionClosed;
			connection.Close();
			screen.Close();
		}

		private void OnConnectionClosed()
		{
			Finish();
		}

		private void OnMessageReceived(MessageReader msg)
		{
			switch (msg.MessageType)
			{
				case RMSG_NEWWDG:
					log.Info("RMSG_NEWWDG");
					break;
				case RMSG_WDGMSG:
					log.Info("RMSG_WDGMSG");
					break;
				case RMSG_DSTWDG:
					log.Info("RMSG_DSTWDG");
					break;
				case RMSG_MAPIV:
					log.Info("RMSG_MAPIV");
					break;
				case RMSG_GLOBLOB:
					log.Info("RMSG_GLOBLOB");
					break;
				case RMSG_PAGINAE:
					log.Info("RMSG_PAGINAE");
					break;
				case RMSG_RESID:
					log.Info("RMSG_RESID");
					break;
				case RMSG_PARTY:
					log.Info("RMSG_PARTY");
					break;
				case RMSG_SFX:
					log.Info("RMSG_SFX");
					break;
				case RMSG_CATTR:
					log.Info("RMSG_CATTR");
					break;
				case RMSG_MUSIC:
					log.Info("RMSG_MUSIC");
					break;
				case RMSG_TILES:
					while(!msg.IsEom)
					{
						var id = msg.ReadByte();
						var resName = msg.ReadString();
						var resVer = msg.ReadUint16();
						state.Map.SetTileset(id, App.Instance.Resources.GetTileset(resName));
					}
					break;
				case RMSG_BUFF:
					log.Info("RMSG_BUFF");
					break;
				default:
					throw new Exception("Unknown rmsg type: " + msg.MessageType);
			}
		}
	}
}
