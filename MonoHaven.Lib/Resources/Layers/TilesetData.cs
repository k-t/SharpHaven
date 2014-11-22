#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

namespace MonoHaven.Resources.Layers
{
	public class TilesetData : IDataLayer
	{
		public bool HasTransitions { get; set; }
		public FlavorObjectData[] FlavorObjects { get; set; }
		public ushort FlavorDensity { get; set; }
	}

	public class FlavorObjectData
	{
		public string ResName { get; set; }
		public ushort ResVersion { get; set; }
		public ushort Weight { get; set; }
	}
}
