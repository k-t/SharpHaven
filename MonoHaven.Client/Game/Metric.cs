using System.Drawing;

namespace SharpHaven.Game
{
	public struct Metric
	{
		private readonly Color color;
		private readonly int value;

		public Metric(Color color, int value)
		{
			this.color = color;
			this.value = value;
		}

		public Color Color
		{
			get { return color; }
		}

		public int Value
		{
			get { return value; }
		}
	}
}
