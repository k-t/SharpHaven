using System;
using Haven;

namespace SharpHaven.Client
{
	public class GobMovement
	{
		private readonly Point2D origin;
		private readonly Point2D destination;
		private readonly int totalSteps;
		private double a;

		public GobMovement(Point2D origin, Point2D destination, int totalSteps)
		{
			this.origin = origin;
			this.destination = destination;
			this.totalSteps = totalSteps;
			this.a = 0;
		}

		public Point2D Position
		{
			get
			{
				var dx = destination.X - origin.X;
				var dy = destination.Y - origin.Y;
				var m = new Point2D((int)(dx * a), (int)(dy * a));
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
