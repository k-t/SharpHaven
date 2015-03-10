using System.Drawing;

namespace MonoHaven.Game.Messages
{
	public abstract class GobDelta
	{
		public abstract void Visit(IGobDeltaVisitor visitor);

		public class Clear : GobDelta
		{
			public override void Visit(IGobDeltaVisitor visitor)
			{
				visitor.Visit(this);
			}
		}

		public class Position : GobDelta
		{
			public Point Value { get; set; }

			public override void Visit(IGobDeltaVisitor visitor)
			{
				visitor.Visit(this);
			}
		}

		public class Resource : GobDelta
		{
			public int Id { get; set; }
			public byte[] SpriteData { get; set; }

			public override void Visit(IGobDeltaVisitor visitor)
			{
				visitor.Visit(this);
			}
		}

		public class StartMovement : GobDelta
		{
			public Point Origin { get; set; }
			public Point Destination { get; set; }
			public int Time { get; set; }

			public override void Visit(IGobDeltaVisitor visitor)
			{
				visitor.Visit(this);
			}
		}

		public class AdjustMovement : GobDelta
		{
			public int Time { get; set; }

			public override void Visit(IGobDeltaVisitor visitor)
			{
				visitor.Visit(this);
			}
		}

		public class Speech : GobDelta
		{
			public Point Offset { get; set; }
			public string Text { get; set; }

			public override void Visit(IGobDeltaVisitor visitor)
			{
				visitor.Visit(this);
			}
		}

		public class Layers : GobDelta
		{
			public int BaseResourceId { get; set; }
			public int[] ResourceIds { get; set; }

			public override void Visit(IGobDeltaVisitor visitor)
			{
				visitor.Visit(this);
			}
		}

		public class Avatar : GobDelta
		{
			public int[] ResourceIds { get; set; }

			public override void Visit(IGobDeltaVisitor visitor)
			{
				visitor.Visit(this);
			}
		}

		public class DrawOffset : GobDelta
		{
			public Point Value { get; set; }

			public override void Visit(IGobDeltaVisitor visitor)
			{
				visitor.Visit(this);
			}
		}

		public class Light : GobDelta
		{
			public Point Offset { get; set; }
			public int Size { get; set; }
			public byte Intensity { get; set; }

			public override void Visit(IGobDeltaVisitor visitor)
			{
				visitor.Visit(this);
			}
		}

		public class Follow : GobDelta
		{
			public int GobId { get; set; }
			public byte Szo { get; set; } // TODO: Rename
			public Point Offset { get; set; }

			public override void Visit(IGobDeltaVisitor visitor)
			{
				visitor.Visit(this);
			}
		}

		public class Homing : GobDelta
		{
			public int GobId { get; set; }
			public Point Target { get; set; }
			public int Velocity { get; set; }

			public override void Visit(IGobDeltaVisitor visitor)
			{
				visitor.Visit(this);
			}
		}

		public class Overlay : GobDelta
		{
			public int Id { get; set; }
			public bool Prs { get; set; } // TODO: Rename
			public int ResourceId { get; set; }
			public byte[] SpriteData { get; set; }

			public override void Visit(IGobDeltaVisitor visitor)
			{
				visitor.Visit(this);
			}
		}

		public class Health : GobDelta
		{
			public byte Value { get; set; }

			public override void Visit(IGobDeltaVisitor visitor)
			{
				visitor.Visit(this);
			}
		}

		public class Buddy : GobDelta
		{
			public string Name { get; set; }
			public byte Group { get; set; }
			public byte Type { get; set; }

			public override void Visit(IGobDeltaVisitor visitor)
			{
				visitor.Visit(this);
			}
		}
	}
}
