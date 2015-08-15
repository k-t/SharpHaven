using System.Drawing;

namespace SharpHaven.Client
{
	public struct Metric
	{
		public Metric(Color color, int value)
		{
			Color = color;
			Value = value;
		}

		public Color Color { get; }

		public int Value { get; }
	}
}
