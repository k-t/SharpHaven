#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

using System.Collections.Generic;

namespace MonoHaven.Graphics.Sprites
{
	public class AnimFrame
	{
		private readonly int id;
		private readonly int duration;
		private readonly List<SpritePart> parts;

		public AnimFrame(int id, int duration)
		{
			this.id = id;
			this.duration = duration;
			this.parts = new List<SpritePart>();
		}

		public int Id
		{
			get { return id; }
		}

		public int Duration
		{
			get { return duration; }
		}

		public List<SpritePart> Parts
		{
			get { return parts; }
		}
	}
}
