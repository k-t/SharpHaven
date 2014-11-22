#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

namespace MonoHaven.Resources.Layers
{
	public class TileData : IDataLayer
	{
		public int Id { get; set; }
		public int Weight { get; set; }
		public char Type { get; set; }
		public byte[] ImageData { get; set; }
	}
}
