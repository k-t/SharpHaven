namespace SharpHaven.UI
{
	public struct Padding
	{
		public Padding(int all) : this(all, all, all, all)
		{
		}

		public Padding(int left, int top, int right, int bottom) : this()
		{
			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
		}

		public int All
		{
			get { return (Left == Top && Top == Right && Right == Bottom) ? Left : -1; }
			set { Left = Top = Right = Bottom = value; }
		}

		public int Left
		{
			get;
			set;
		}

		public int Top
		{
			get;
			set;
		}

		public int Right
		{
			get;
			set;
		}

		public int Bottom
		{
			get;
			set;
		}
	}
}
