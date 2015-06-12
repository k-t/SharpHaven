using System.Collections.Generic;
using SharpHaven.Game;

namespace SharpHaven.Net
{
	public static class GobDeltaMessageReaders
	{
		public static GobDelta.Position ReadGobPosition(this MessageReader reader)
		{
			return new GobDelta.Position { Value = reader.ReadCoord() };
		}

		public static GobDelta.Resource ReadGobResource(this MessageReader reader)
		{
			int resId = reader.ReadUint16();

			byte[] spriteData;
			if ((resId & 0x8000) != 0)
			{
				resId &= ~0x8000;
				var len = reader.ReadByte();
				spriteData = reader.ReadBytes(len);
			}
			else
			{
				spriteData = new byte[0];
			}

			return new GobDelta.Resource { Id = resId, SpriteData = spriteData };
		}

		public static GobDelta.StartMovement ReadGobStartMovement(this MessageReader reader)
		{
			return new GobDelta.StartMovement {
				Origin = reader.ReadCoord(),
				Destination = reader.ReadCoord(),
				TotalSteps = reader.ReadInt32()
			};
		}

		public static GobDelta.AdjustMovement ReadGobAdjustMovement(this MessageReader reader)
		{
			return new GobDelta.AdjustMovement { Step = reader.ReadInt32() };
		}

		public static GobDelta.Speech ReadGobSpeech(this MessageReader reader)
		{
			return new GobDelta.Speech {
				Offset = reader.ReadCoord(),
				Text = reader.ReadString()
			};
		}

		public static GobDelta.DrawOffset ReadGobDrawOffset(this MessageReader reader)
		{
			return new GobDelta.DrawOffset { Value = reader.ReadCoord() };
		}

		public static GobDelta.Light ReadGobLight(this MessageReader reader)
		{
			return new GobDelta.Light {
				Offset = reader.ReadCoord(),
				Size = reader.ReadUint16(),
				Intensity = reader.ReadByte()
			};
		}

		public static GobDelta.Follow ReadGobFollow(this MessageReader reader)
		{
			int oid = reader.ReadInt32();
			if (oid != -1)
			{
				return new GobDelta.Follow {
					GobId = oid,
					Szo = reader.ReadByte(),
					Offset = reader.ReadCoord()
				};
			}
			return new GobDelta.Follow { GobId = oid };
		}

		public static GobDelta.Homing ReadGobHoming(this MessageReader reader)
		{
			int oid = reader.ReadInt32();
			if (oid != -1)
			{
				return new GobDelta.Homing {
					GobId = oid,
					Target = reader.ReadCoord(),
					Velocity = reader.ReadUint16()
				};
			}
			return new GobDelta.Homing { GobId = oid };
		}

		public static GobDelta.Overlay ReadGobOverlay(this MessageReader reader)
		{
			int overlayId = reader.ReadInt32();
			var prs = (overlayId & 1) != 0;
			overlayId >>= 1;

			int resId = reader.ReadUint16();
			
			byte[] spriteData = null;
			if (resId != 65535)
			{
				if ((resId & 0x8000) != 0)
				{
					resId &= ~0x8000;
					var len = reader.ReadByte();
					spriteData = reader.ReadBytes(len);
				}
				else
					spriteData = new byte[0];
			}
			else
				resId = -1;

			return new GobDelta.Overlay {
				Id = overlayId,
				IsPersistent = prs,
				ResourceId = resId,
				SpriteData = spriteData
			};
		}

		public static GobDelta.Health ReadGobHealth(this MessageReader reader)
		{
			return new GobDelta.Health { Value = reader.ReadByte() };
		}

		public static GobDelta.Buddy ReadGobBuddy(this MessageReader reader)
		{
			return new GobDelta.Buddy {
				Name = reader.ReadString(),
				Group = reader.ReadByte(),
				Type = reader.ReadByte()
			};
		}

		public static GobDelta.Avatar ReadGobAvatar(this MessageReader reader)
		{
			var layers = new List<int>();
			while (true)
			{
				int layer = reader.ReadUint16();
				if (layer == 65535)
					break;
				layers.Add(layer);
			}
			return new GobDelta.Avatar { ResourceIds = layers.ToArray() };
		}

		public static GobDelta.Layers ReadGobLayers(this MessageReader reader)
		{
			int baseResId = reader.ReadUint16();
			var layers = new List<int>();
			while (true)
			{
				int layer = reader.ReadUint16();
				if (layer == 65535)
					break;
				layers.Add(layer);
			}
			return new GobDelta.Layers {
				BaseResourceId = baseResId,
				ResourceIds = layers.ToArray()
			};
		}
	}
}
