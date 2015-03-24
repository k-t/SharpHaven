using System;
using System.Drawing;

namespace MonoHaven.Game
{
	public class PartyMember
	{
		private readonly int id;
		private Color color;
		private Point? location;

		public PartyMember(int id)
		{
			this.id = id;
		}

		public event Action<PartyMember> Changed;

		public int Id
		{
			get { return id; }
		}

		public Color Color
		{
			get { return color; }
			set
			{
				color = value;
				Changed.Raise(this);
			}
		}

		public Point? Location
		{
			get { return location; }
			set
			{
				location = value;
				Changed.Raise(this);
			}
		}
	}
}
