using System;
using SharpHaven.Graphics;

namespace SharpHaven.Client
{
	public class PartyMember
	{
		private Color color;
		private Coord2D? location;

		public PartyMember(int id)
		{
			Id = id;
		}

		public event Action<PartyMember> Changed;

		public int Id { get; }

		public Color Color
		{
			get { return color; }
			set
			{
				color = value;
				Changed.Raise(this);
			}
		}

		public Coord2D? Location
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
