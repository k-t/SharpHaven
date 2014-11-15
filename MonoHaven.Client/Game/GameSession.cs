﻿using System;
using System.Collections.Generic;
using System.Net;
using MonoHaven.Game.Messages;
using MonoHaven.Network;
using MonoHaven.Resources;
using MonoHaven.UI;
using MonoHaven.Utils;
using NLog;

namespace MonoHaven.Game
{
	public class GameSession : IMapDataProvider
	{
		#region Constants

		private const int MSG_MAPREQ = 4;

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
		private readonly Dictionary<int, Resource> resources;

		public GameSession(ConnectionSettings connSettings)
		{
			connection = new Connection(connSettings);
			connection.MessageReceived += OnMessageReceived;
			connection.MapDataReceived += OnMapDataReceived;
			connection.Closed += OnConnectionClosed;

			state = new GameState(this);
			screen = new GameScreen(this);
			resources = new Dictionary<int, Resource>();
		}

		public event Action Finished;

		public GameScreen Screen
		{
			get { return screen; }
		}

		public GameState State
		{
			get { return state; }
		}

		public void Start()
		{
			connection.Open();
		}

		public void Finish()
		{
			lock (this)
			{
				connection.Closed -= OnConnectionClosed;
				connection.Close();
			}
			Finished.Raise();
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
					App.Instance.QueueOnMainThread(() =>
					{
						var id = msg.ReadUint16();
						var type = msg.ReadString();
						var location = msg.ReadCoord();
						var parent = msg.ReadUint16();
						var args = msg.ReadList();
						WidgetCreated.Raise(new WidgetCreateMessage(id, parent, type, location, args));
					});
					break;
				case RMSG_WDGMSG:
					App.Instance.QueueOnMainThread(() =>
					{
						var id = msg.ReadUint16();
						var name = msg.ReadString();
						var args = msg.ReadList();
						WidgetMessage.Raise(new WidgetMessage(id, name, args));
					});
					break;
				case RMSG_DSTWDG:
					App.Instance.QueueOnMainThread(() =>
					{
						var id = msg.ReadUint16();
						WidgetDestroyed.Raise(new WidgetDestroyMessage(id));
					});
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
					App.Instance.QueueOnMainThread(() =>
					{
						var id = msg.ReadUint16();
						var name = msg.ReadString();
						var ver = msg.ReadUint16();
						LoadResource(id, name, ver);
					});
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
					App.Instance.QueueOnMainThread(() =>
					{
						while (!msg.IsEom)
						{
							var id = msg.ReadByte();
							var name = msg.ReadString();
							var version = msg.ReadUint16();
							var tileset = App.Instance.Resources.GetTileset(name);
							TilesetAvailable.Raise(id, tileset);
						}
					});
					break;
				case RMSG_BUFF:
					log.Info("RMSG_BUFF");
					break;
				default:
					throw new Exception("Unknown rmsg type: " + msg.MessageType);
			}
		}

		private void OnMapDataReceived(MessageReader msg)
		{
			int packetId = msg.ReadInt32();
			int offset = msg.ReadUint16();
			int length = msg.ReadUint16();

			// TODO: defragmentation
			if (offset != 0 || msg.Length - 8 < length)
				throw new NotImplementedException("Defragmentation is not supported yet");

			var mapData = MapData.FromMessage(msg);
			App.Instance.QueueOnMainThread(() => DataAvailable.Raise(mapData));
		}

		#region Resource Management

		public Resource GetResource(int id)
		{
			return resources[id];
		}

		private void LoadResource(int id, string name, int version)
		{
			resources[id] = App.Instance.Resources.Get(name);
		}

		#endregion

		#region Widget Management

		public event Action<WidgetCreateMessage> WidgetCreated;
		public event Action<WidgetDestroyMessage> WidgetDestroyed;
		public event Action<WidgetMessage> WidgetMessage;

		public void SendMessage(ushort widgetId, string name, object[] args)
		{
			var message = new Message(RMSG_WDGMSG)
				.Uint16(widgetId)
				.String(name);
			if (args != null)
				message.List(args);
			connection.SendMessage(message);
		}

		#endregion

		#region Map Data Management

		public event Action<MapData> DataAvailable;
		public event Action<byte, Tileset> TilesetAvailable;

		public void RequestData(int x, int y)
		{
			connection.RequestMapData(x, y);
		}

		#endregion
	}
}
