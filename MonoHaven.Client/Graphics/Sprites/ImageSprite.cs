using System.Collections.Generic;
using MonoHaven.Resources;

namespace MonoHaven.Graphics.Sprites
{
	public class ImageSprite : Sprite
	{
		public ImageSprite(Resource res) : base(res)
		{
		}

		public override IEnumerable<SpritePart> Parts
		{
			get { return parts; }
		}
	}
}
