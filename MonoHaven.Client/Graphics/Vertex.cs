namespace MonoHaven.Graphics
{
	public struct Vertex
	{
		public Vertex(int x, int y, float u, float v)
			: this()
		{
			X = x;
			Y = y;
			U = u;
			V = v;
		}

		public int X { get; set; }
		public int Y { get; set; }
		public float U { get; set; }
		public float V { get; set; }
	}
}
