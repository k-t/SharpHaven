using System;
using MonoHaven.Network;
using System.Collections.Generic;

namespace MonoHaven.Game.Messages
{
	public class GobChangeset
	{
		private const int OD_REM = 0;
		private const int OD_MOVE = 1;
		private const int OD_RES = 2;
		private const int OD_LINBEG = 3;
		private const int OD_LINSTEP = 4;
		private const int OD_SPEECH = 5;
		private const int OD_LAYERS = 6;
		private const int OD_DRAWOFF = 7;
		private const int OD_LUMIN = 8;
		private const int OD_AVATAR = 9;
		private const int OD_FOLLOW = 10;
		private const int OD_HOMING = 11;
		private const int OD_OVERLAY = 12;
		/* private const int OD_AUTH = 13; -- Removed */
		private const int OD_HEALTH = 14;
		private const int OD_BUDDY = 15;
		private const int OD_END = 255;

		private readonly List<GobDelta> deltas = new List<GobDelta>();

		public bool ReplaceFlag
		{
			get;
			set;
		}

		public int GobId
		{
			get;
			set;
		}

		public int Frame
		{
			get;
			set;
		}

		public IList<GobDelta> Deltas
		{
			get { return deltas; }
		}

		public static GobChangeset ReadFrom(MessageReader msg)
		{
			var data = new GobChangeset();

			data.ReplaceFlag = (msg.ReadByte() & 1) != 0;
			data.GobId = msg.ReadInt32();
			data.Frame = msg.ReadInt32();

			while (true)
			{
				GobDelta delta;
				int type = msg.ReadByte();
				switch (type)
				{
					case OD_REM:
						delta = new GobDelta.Clear();
						break;
					case OD_MOVE:
					{
						var pos = msg.ReadCoord();
						delta = new GobDelta.Position {Value = pos};
						break;
					}
					case OD_RES:
					{
						int resId = msg.ReadUint16();
						byte[] spriteData;
						if ((resId & 0x8000) != 0)
						{
							resId &= ~0x8000;
							var len = msg.ReadByte();
							spriteData = msg.ReadBytes(len);
						}
						else
						{
							spriteData = new byte[0];
						}
						delta = new GobDelta.Resource {Id = resId, SpriteData = spriteData};
						break;
					}
					case OD_LINBEG:
						delta = new GobDelta.StartMovement
						{
							Origin = msg.ReadCoord(),
							Destination = msg.ReadCoord(),
							Time = msg.ReadInt32()
						};
						break;
					case OD_LINSTEP:
						delta = new GobDelta.AdjustMovement {Time = msg.ReadInt32()};
						break;
					case OD_SPEECH:
						delta = new GobDelta.Speech
						{
							Offset = msg.ReadCoord(),
							Text = msg.ReadString()
						};
						break;
					case OD_LAYERS:
					case OD_AVATAR:
					{
						int baseResId = -1;
						if (type == OD_LAYERS)
							baseResId = msg.ReadUint16();
						var layers = new List<int>();
						while (true)
						{
							int layer = msg.ReadUint16();
							if (layer == 65535)
								break;
							layers.Add(layer);
						}
						if (type == OD_LAYERS)
							delta = new GobDelta.Layers
							{
								BaseResourceId = baseResId,
								ResourceIds = layers.ToArray()
							};
						else
							delta = new GobDelta.Avatar {ResourceIds = layers.ToArray()};
						break;
					}
					case OD_DRAWOFF:
						delta = new GobDelta.DrawOffset {Value = msg.ReadCoord()};
						break;
					case OD_LUMIN:
						delta = new GobDelta.Light
						{
							Offset = msg.ReadCoord(),
							Size = msg.ReadUint16(),
							Intensity = msg.ReadByte()
						};
						break;
					case OD_FOLLOW:
					{
						int oid = msg.ReadInt32();
						if (oid != -1)
						{
							delta = new GobDelta.Follow
							{
								GobId = oid,
								Szo = msg.ReadByte(),
								Offset = msg.ReadCoord()
							};
						}
						else
							delta = new GobDelta.Follow {GobId = oid};
						break;
					}
					case OD_HOMING:
					{
						int oid = msg.ReadInt32();
						if (oid == -1)
							delta = new GobDelta.Homing {GobId = oid};
						else
							delta = new GobDelta.Homing
							{
								GobId = oid,
								Target = msg.ReadCoord(),
								Velocity = msg.ReadUint16()
							};
						break;
					}
					case OD_OVERLAY:
					{
						int overlayId = msg.ReadInt32();
						var prs = (overlayId & 1) != 0;
						overlayId >>= 1;
						int resId = msg.ReadUint16();
						byte[] spriteData = null;
						if (resId != 65535)
						{
							if ((resId & 0x8000) != 0)
							{
								resId &= ~0x8000;
								var len = msg.ReadByte();
								spriteData = msg.ReadBytes(len);
							}
							else
								spriteData = new byte[0];
						}
						delta = new GobDelta.Overlay
						{
							Id = overlayId,
							Prs = prs,
							ResourceId = resId,
							SpriteData = spriteData
						};
						break;
					}
					case OD_HEALTH:
						delta = new GobDelta.Health { Value = msg.ReadByte() };
						break;
					case OD_BUDDY:
						delta = new GobDelta.Buddy
						{
							Name = msg.ReadString(),
							Group = msg.ReadByte(),
							Type = msg.ReadByte()
						};
						break;
					case OD_END:
						delta = null;
						break;
					default:
						// TODO: MessageException
						throw new Exception("Unknown objdelta type: " + type);
				}
				if (delta == null)
					break;
				data.Deltas.Add(delta);
			}

			return data;
		}
	}
}
