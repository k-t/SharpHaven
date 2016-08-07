using System;
using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.Zip.Compression;
using NLog;
using SharpHaven.Game.Messages;
using SharpHaven.Graphics;
using SharpHaven.Resources;
using SharpHaven.Utils;

namespace SharpHaven.Net
{
	public static class MessageToObject
	{
		private static readonly Logger Log = LogManager.GetCurrentClassLogger();

		public static UpdateAmbientLight ReadAmbientLightUpdateEvent(this ByteBuffer reader)
		{
			return new UpdateAmbientLight {
				Color = reader.ReadColor()
			};
		}

		public static UpdateAstronomy ReadAstronomyUpdateEvent(this ByteBuffer reader)
		{
			int dt = reader.ReadInt32();
			int mp = reader.ReadInt32();
			/*int yt =*/ reader.ReadInt32();
			return new UpdateAstronomy
			{
				DayTime = Defix(dt),
				MoonPhase = Defix(mp)
			};
		}

		public static UpdateCharAttributes ReadCharAttributesUpdateEvent(this ByteBuffer reader)
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
			return new UpdateCharAttributes { Attributes = attributes.ToArray() };
		}

		public static BuffRemove ReadBuffRemoveEvent(this ByteBuffer reader)
		{
			return new BuffRemove { BuffId = reader.ReadInt32() };
		}

		public static BuffUpdate ReadBuffUpdateEvent(this ByteBuffer reader)
		{
			return new BuffUpdate {
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

		public static UpdateActions ReadGameActionsUpdateEvent(this ByteBuffer reader)
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

			return new UpdateActions {
				Added = added.ToArray(),
				Removed = removed.ToArray()
			};
		}

		public static UpdateGameTime ReadGameTimeUpdateEvent(this ByteBuffer reader)
		{
			return new UpdateGameTime {
				Time = reader.ReadInt32()
			};
		}

		public static MapInvalidateGrid ReadMapInvalidateGridEvent(this ByteBuffer reader)
		{
			return new MapInvalidateGrid {
				Coord = reader.ReadInt32Coord()
			};
		}

		public static MapInvalidateRegion ReadMapInvalidateRegionEvent(this ByteBuffer reader)
		{
			var ul = reader.ReadInt32Coord();
			var br = reader.ReadInt32Coord();
			return new MapInvalidateRegion {
				Region = Rect.FromLTRB(ul, br)
			};
		}

		public static MapUpdate ReadMapUpdateEvent(this ByteBuffer reader)
		{
			var msg = new MapUpdate
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

		public static PartyUpdate ReadPartyUpdateEvent(this ByteBuffer reader)
		{
			var ids = new List<int>();
			while (true)
			{
				int id = reader.ReadInt32();
				if (id == -1)
					break;
				ids.Add(id);
			}
			return new PartyUpdate { MemberIds = ids.ToArray() };
		}

		public static PartyChangeLeader ReadPartyLeaderChangeEvent(this ByteBuffer reader)
		{
			return new PartyChangeLeader {
				LeaderId = reader.ReadInt32()
			};
		}

		public static PartyUpdateMember ReadPartyMemberUpdateEvent(this ByteBuffer reader)
		{
			var memberId = reader.ReadInt32();
			var hasLocation = reader.ReadByte() == 1;
			var location = hasLocation ? reader.ReadInt32Coord() : (Coord2D?)null;
			var color = reader.ReadColor();
			return new PartyUpdateMember {
				Color = color,
				Location = location,
				MemberId = memberId
			};
		}

		public static PlaySound ReadPlaySoundEvent(this ByteBuffer reader)
		{
			return new PlaySound {
				ResourceId = reader.ReadUInt16(),
				Volume = reader.ReadUInt16() / 256.0,
				Speed = reader.ReadUInt16() / 256.0
			};
		}

		public static LoadResource ReadResourceLoadEvent(this ByteBuffer reader)
		{
			return new LoadResource {
				ResourceId = reader.ReadUInt16(),
				Name = reader.ReadCString(),
				Version = reader.ReadUInt16()
			};
		}

		public static LoadTilesets ReadTilesetsLoadEvent(this ByteBuffer reader)
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
			return new LoadTilesets { Tilesets = tilesets.ToArray() };
		}

		public static WidgetCreate ReadWidgetCreateEvent(this ByteBuffer reader)
		{
			var id = reader.ReadUInt16();
			var type = reader.ReadCString();
			var position = reader.ReadInt32Coord();
			var parentId = reader.ReadUInt16();
			var args = reader.ReadList();

			return new WidgetCreate {
				Id = id,
				Type = type,
				Position = position,
				ParentId = parentId,
				Args = args
			};
		}

		public static WidgetDestroy ReadWidgetDestroyEvent(this ByteBuffer reader)
		{
			return new WidgetDestroy {
				WidgetId = reader.ReadUInt16()
			};
		}

		public static WidgetMessage ReadWidgetMessageEvent(this ByteBuffer reader)
		{
			return new WidgetMessage {
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
