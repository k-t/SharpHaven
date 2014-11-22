#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

namespace MonoHaven.Resources.Layers
{
	public class AnimData : IDataLayer
	{
		public short Id { get; set; }
		public ushort Duration { get; set; }
		public short[] Frames { get; set; }
	}
}
