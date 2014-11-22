#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

using System.Drawing;

namespace MonoHaven.Game
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
