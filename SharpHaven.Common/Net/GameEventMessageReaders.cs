using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using ICSharpCode.SharpZipLib.Zip.Compression;
using NLog;
using SharpHaven.Game;
using SharpHaven.Game.Events;
using SharpHaven.Resources;

namespace SharpHaven.Net
{
	public static class GameEventMessageReaders
	{
		private static readonly Logger Log = LogManager.GetCurrentClassLogger();

		public static AmbientLightUpdateEvent ReadAmbientLightUpdateEvent(this MessageReader reader)
		{
			return new AmbientLightUpdateEvent {
				Color = reader.ReadColor()
			};
		}

		public static AstronomyUpdateEvent ReadAstronomyUpdateEvent(this MessageReader reader)
		{
			int dt = reader.ReadInt32();
			int mp = reader.ReadInt32();
			/*int yt =*/ reader.ReadInt32();
			return new AstronomyUpdateEvent
			{
				DayTime = Defix(dt),
				MoonPhase = Defix(mp)
			};
		}

		public static CharAttributesUpdateEvent ReadCharAttributesUpdateEvent(this MessageReader reader)
		{
			var attributes = new List<CharAttribute>();
			while (!reader.IsEom)
			{
				attributes.Add(new CharAttribute {
					Name = reader.ReadString(),
					BaseValue = reader.ReadInt32(),
					ModifiedValue = reader.ReadInt32()
				});
			}
			return new CharAttributesUpdateEvent { Attributes = attributes.ToArray() };
		}

		public static BuffRemoveEvent ReadBuffRemoveEvent(this MessageReader reader)
		{
			return new BuffRemoveEvent { BuffId = reader.ReadInt32() };
		}

		public static BuffUpdateEvent ReadBuffUpdateEvent(this MessageReader reader)
		{
			return new BuffUpdateEvent {
				Id = reader.ReadInt32(),
				ResourceId = reader.ReadUint16(),
				Tooltip = reader.ReadString(),
				AMeter = reader.ReadInt32(),
				NMeter = reader.ReadInt32(),
				CMeter = reader.ReadInt32(),
				CTicks = reader.ReadInt32(),
				IsMajor = reader.ReadByte() != 0
			};
		}

		public static GameActionsUpdateEvent ReadGameActionsUpdateEvent(this MessageReader reader)
		{
			var added = new List<ResourceRef>();
			var removed = new List<ResourceRef>();

			while (!reader.IsEom)
			{
				var removeFlag = reader.ReadByte() == '-';
				var name = reader.ReadString();
				var version = reader.ReadUint16();

				var list = removeFlag ? removed : added;
				list.Add(new ResourceRef(name, version));
			}

			return new GameActionsUpdateEvent {
				Added = added.ToArray(),
				Removed = removed.ToArray()
			};
		}

		public static GameTimeUpdateEvent ReadGameTimeUpdateEvent(this MessageReader reader)
		{
			return new GameTimeUpdateEvent {
				Time = reader.ReadInt32()
			};
		}

		public static MapInvalidateGridEvent ReadMapInvalidateGridEvent(this MessageReader reader)
		{
			return new MapInvalidateGridEvent {
				Coord = reader.ReadCoord()
			};
		}

		public static MapInvalidateRegionEvent ReadMapInvalidateRegionEvent(this MessageReader reader)
		{
			var ul = reader.ReadCoord();
			var br = reader.ReadCoord();
			return new MapInvalidateRegionEvent {
				Region = Rectangle.FromLTRB(ul.X, ul.Y, br.X, br.Y)
			};
		}

		public static MapUpdateEvent ReadMapUpdateEvent(this MessageReader reader)
		{
			var msg = new MapUpdateEvent
			{
				Grid = reader.ReadCoord(),
				MinimapName = reader.ReadString(),
				Tiles = new byte[100 * 100],
				Overlays = new int[100 * 100]
			};

			var pfl = new byte[256];
			while (true)
			{
				int pidx = reader.ReadByte();
				if (pidx == 255)
					break;
				pfl[pidx] = reader.ReadByte();
			}

			var blob = Unpack(reader.Buffer, reader.Position, reader.Length - reader.Position);
			Array.Copy(blob, msg.Tiles, msg.Tiles.Length);

			reader = new MessageReader(0, blob);
			reader.Position += msg.Tiles.Length;
			while (true)
			{
				int pidx = reader.ReadByte();
				if (pidx == 255)
					break;
				int fl = pfl[pidx];
				int type = reader.ReadByte();
				var c1 = new Point(reader.ReadByte(), reader.ReadByte());
				var c2 = new Point(reader.ReadByte(), reader.ReadByte());

				int ol;
				if (type == 0)
					ol = ((fl & 1) == 1) ? 2 : 1;
				else if (type == 1)
					ol = ((fl & 1) == 1) ? 8 : 4;
				else
				{
					Log.Warn("Unknown plot type " + type);
					continue;
				}

				for (int y = c1.Y; y <= c2.Y; y++)
					for (int x = c1.X; x <= c2.X; x++)
						msg.Overlays[y * 100 + x] |= ol;
			}

			return msg;
		}

		public static PartyUpdateEvent ReadPartyUpdateEvent(this MessageReader reader)
		{
			var ids = new List<int>();
			while (true)
			{
				int id = reader.ReadInt32();
				if (id == -1)
					break;
				ids.Add(id);
			}
			return new PartyUpdateEvent { MemberIds = ids.ToArray() };
		}

		public static PartyLeaderChangeEvent ReadPartyLeaderChangeEvent(this MessageReader reader)
		{
			return new PartyLeaderChangeEvent {
				LeaderId = reader.ReadInt32()
			};
		}

		public static PartyMemberUpdateEvent ReadPartyMemberUpdateEvent(this MessageReader reader)
		{
			var memberId = reader.ReadInt32();
			var hasLocation = reader.ReadByte() == 1;
			var location = hasLocation ? reader.ReadCoord() : (Point?)null;
			var color = reader.ReadColor();
			return new PartyMemberUpdateEvent {
				Color = color,
				Location = location,
				MemberId = memberId
			};
		}

		public static PlaySoundEvent ReadPlaySoundEvent(this MessageReader reader)
		{
			return new PlaySoundEvent {
				ResourceId = reader.ReadUint16(),
				Volume = reader.ReadUint16() / 256.0,
				Speed = reader.ReadUint16() / 256.0
			};
		}

		public static ResourceLoadEvent ReadResourceLoadEvent(this MessageReader reader)
		{
			return new ResourceLoadEvent {
				ResourceId = reader.ReadUint16(),
				Name = reader.ReadString(),
				Version = reader.ReadUint16()
			};
		}

		public static TilesetsLoadEvent ReadTilesetsLoadEvent(this MessageReader reader)
		{
			var tilesets = new List<TilesetBinding>();
			while (!reader.IsEom)
			{
				tilesets.Add(new TilesetBinding {
					Id = reader.ReadByte(),
					Name = reader.ReadString(),
					Version = reader.ReadUint16()
				});
			}
			return new TilesetsLoadEvent { Tilesets = tilesets.ToArray() };
		}

		public static WidgetCreateEvent ReadWidgetCreateEvent(this MessageReader reader)
		{
			var id = reader.ReadUint16();
			var type = reader.ReadString();
			var position = reader.ReadCoord();
			var parentId = reader.ReadUint16();
			var args = reader.ReadList();

			return new WidgetCreateEvent {
				Id = id,
				Type = type,
				Position = position,
				ParentId = parentId,
				Args = args
			};
		}

		public static WidgetDestroyEvent ReadWidgetDestroyEvent(this MessageReader reader)
		{
			return new WidgetDestroyEvent {
				WidgetId = reader.ReadUint16()
			};
		}

		public static WidgetMessageEvent ReadWidgetMessageEvent(this MessageReader reader)
		{
			return new WidgetMessageEvent {
				WidgetId = reader.ReadUint16(),
				Name = reader.ReadString(),
				Args = reader.ReadList()
			};
		}

		private static double Defix(int i)
		{
			return i / 1e9;
		}

		private static byte[] Unpack(byte[] input, int offset, int length)
		{
			var buf = new byte[4096];
			var inflater = new Inflater();
			using (var output = new MemoryStream())
			{
				inflater.SetInput(input, offset, length);
				int n;
				while ((n = inflater.Inflate(buf)) != 0)
					output.Write(buf, 0, n);

				if (!inflater.IsFinished)
					throw new Exception("Got unterminated map blob");

				return output.ToArray();
			}
		}
	}
}
