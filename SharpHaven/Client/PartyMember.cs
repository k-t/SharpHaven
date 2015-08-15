using System;
using System.Drawing;

namespace SharpHaven.Client
{
	public class PartyMember
	{
		private Color color;
		private Point? location;

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
