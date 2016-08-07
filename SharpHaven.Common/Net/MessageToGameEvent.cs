using System;
using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.Zip.Compression;
using NLog;
using SharpHaven.Game;
using SharpHaven.Game.Events;
using SharpHaven.Graphics;
using SharpHaven.Resources;
using SharpHaven.Utils;

namespace SharpHaven.Net
{
	public static class MessageToGameEvent
	{
		private static readonly Logger Log = LogManager.GetCurrentClassLogger();

		public static AmbientLightUpdateEvent ReadAmbientLightUpdateEvent(this ByteBuffer reader)
		{
			return new AmbientLightUpdateEvent {
				Color = reader.ReadColor()
			};
		}

		public static AstronomyUpdateEvent ReadAstronomyUpdateEvent(this ByteBuffer reader)
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

		public static CharAttributesUpdateEvent ReadCharAttributesUpdateEvent(this ByteBuffer reader)
		{
			var attributes = new List<CharAttribute>();
			while (reader.HasRemaining)
			{
				attributes.Add(new CharAttribute {
					Name = reader.ReadCString(),
					BaseValue = reader.ReadInt32(),
					ModifiedValue = reader.ReadInt32()
				});
			}
			return new CharAttributesUpdateEvent { Attributes = attributes.ToArray() };
		}

		public static BuffRemoveEvent ReadBuffRemoveEvent(this ByteBuffer reader)
		{
			return new BuffRemoveEvent { BuffId = reader.ReadInt32() };
		}

		public static BuffUpdateEvent ReadBuffUpdateEvent(this ByteBuffer reader)
		{
			return new BuffUpdateEvent {
				Id = reader.ReadInt32(),
				ResourceId = reader.ReadUInt16(),
				Tooltip = reader.ReadCString(),
				AMeter = reader.ReadInt32(),
				NMeter = reader.ReadInt32(),
				CMeter = reader.ReadInt32(),
				CTicks = reader.ReadInt32(),
				IsMajor = reader.ReadByte() != 0
			};
		}

		public static GameActionsUpdateEvent ReadGameActionsUpdateEvent(this ByteBuffer reader)
		{
			var added = new List<ResourceRef>();
			var removed = new List<ResourceRef>();

			while (reader.HasRemaining)
			{
				var removeFlag = reader.ReadByte() == '-';
				var name = reader.ReadCString();
				var version = reader.ReadUInt16();

				var list = removeFlag ? removed : added;
				list.Add(new ResourceRef(name, version));
			}

			return new GameActionsUpdateEvent {
				Added = added.ToArray(),
				Removed = removed.ToArray()
			};
		}

		public static GameTimeUpdateEvent ReadGameTimeUpdateEvent(this ByteBuffer reader)
		{
			return new GameTimeUpdateEvent {
				Time = reader.ReadInt32()
			};
		}

		public static MapInvalidateGridEvent ReadMapInvalidateGridEvent(this ByteBuffer reader)
		{
			return new MapInvalidateGridEvent {
				Coord = reader.ReadInt32Coord()
			};
		}

		public static MapInvalidateRegionEvent ReadMapInvalidateRegionEvent(this ByteBuffer reader)
		{
			var ul = reader.ReadInt32Coord();
			var br = reader.ReadInt32Coord();
			return new MapInvalidateRegionEvent {
				Region = Rect.FromLTRB(ul, br)
			};
		}

		public static MapUpdateEvent ReadMapUpdateEvent(this ByteBuffer reader)
		{
			var msg = new MapUpdateEvent
			{
				Grid = reader.ReadInt32Coord(),
				MinimapName = reader.ReadCString(),
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

			reader = new ByteBuffer(Unpack(reader.ReadRemaining()));
			msg.Tiles = reader.ReadBytes(100 * 100);
			while (true)
			{
				int pidx = reader.ReadByte();
				if (pidx == 255)
					break;
				int fl = pfl[pidx];
				int type = reader.ReadByte();
				var c1 = new Coord2D(reader.ReadByte(), reader.ReadByte());
				var c2 = new Coord2D(reader.ReadByte(), reader.ReadByte());

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

		public static PartyUpdateEvent ReadPartyUpdateEvent(this ByteBuffer reader)
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

		public static PartyLeaderChangeEvent ReadPartyLeaderChangeEvent(this ByteBuffer reader)
		{
			return new PartyLeaderChangeEvent {
				LeaderId = reader.ReadInt32()
			};
		}

		public static PartyMemberUpdateEvent ReadPartyMemberUpdateEvent(this ByteBuffer reader)
		{
			var memberId = reader.ReadInt32();
			var hasLocation = reader.ReadByte() == 1;
			var location = hasLocation ? reader.ReadInt32Coord() : (Coord2D?)null;
			var color = reader.ReadColor();
			return new PartyMemberUpdateEvent {
				Color = color,
				Location = location,
				MemberId = memberId
			};
		}

		public static PlaySoundEvent ReadPlaySoundEvent(this ByteBuffer reader)
		{
			return new PlaySoundEvent {
				ResourceId = reader.ReadUInt16(),
				Volume = reader.ReadUInt16() / 256.0,
				Speed = reader.ReadUInt16() / 256.0
			};
		}

		public static ResourceLoadEvent ReadResourceLoadEvent(this ByteBuffer reader)
		{
			return new ResourceLoadEvent {
				ResourceId = reader.ReadUInt16(),
				Name = reader.ReadCString(),
				Version = reader.ReadUInt16()
			};
		}

		public static TilesetsLoadEvent ReadTilesetsLoadEvent(this ByteBuffer reader)
		{
			var tilesets = new List<TilesetBinding>();
			while (reader.HasRemaining)
			{
				tilesets.Add(new TilesetBinding {
					Id = reader.ReadByte(),
					Name = reader.ReadCString(),
					Version = reader.ReadUInt16()
				});
			}
			return new TilesetsLoadEvent { Tilesets = tilesets.ToArray() };
		}

		public static WidgetCreateEvent ReadWidgetCreateEvent(this ByteBuffer reader)
		{
			var id = reader.ReadUInt16();
			var type = reader.ReadCString();
			var position = reader.ReadInt32Coord();
			var parentId = reader.ReadUInt16();
			var args = reader.ReadList();

			return new WidgetCreateEvent {
				Id = id,
				Type = type,
				Position = position,
				ParentId = parentId,
				Args = args
			};
		}

		public static WidgetDestroyEvent ReadWidgetDestroyEvent(this ByteBuffer reader)
		{
			return new WidgetDestroyEvent {
				WidgetId = reader.ReadUInt16()
			};
		}

		public static WidgetMessageEvent ReadWidgetMessageEvent(this ByteBuffer reader)
		{
			return new WidgetMessageEvent {
				WidgetId = reader.ReadUInt16(),
				Name = reader.ReadCString(),
				Args = reader.ReadList()
			};
		}

		private static double Defix(int i)
		{
			return i / 1e9;
		}

		private static byte[] Unpack(byte[] input)
		{
			var buf = new byte[4096];
			var inflater = new Inflater();
			using (var output = new MemoryStream())
			{
				inflater.SetInput(input, 0, input.Length);
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
