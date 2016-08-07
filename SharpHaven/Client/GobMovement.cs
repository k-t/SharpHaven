using System;
using SharpHaven.Graphics;

namespace SharpHaven.Client
{
	public class GobMovement
	{
		private readonly Coord2D origin;
		private readonly Coord2D destination;
		private readonly int totalSteps;
		private double a;

		public GobMovement(Coord2D origin, Coord2D destination, int totalSteps)
		{
			this.origin = origin;
			this.destination = destination;
			this.totalSteps = totalSteps;
			this.a = 0;
		}

		public Coord2D Position
		{
			get
			{
				var dx = destination.X - origin.X;
				var dy = destination.Y - origin.Y;
				var m = new Coord2D((int)(dx * a), (int)(dy * a));
				return origin.Add(m);
			}
		}

		public int TotalSteps
		{
			get { return totalSteps; }
		}

		public void Adjust(int step)
		{
			a = Math.Max((double)step / totalSteps, a);
		}

		public void Tick(int dt)
		{
			double da = ((double)dt / 1000) / (totalSteps * 0.06);
			a += da * 0.9;
			if (a > 1)
				a = 1;
		}
	}
}
