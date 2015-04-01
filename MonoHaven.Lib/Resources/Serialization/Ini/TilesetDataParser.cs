using System;
using IniParser.Model;

namespace MonoHaven.Resources.Serialization.Ini
{
	public class TilesetDataParser : IIniDataLayerParser
	{
		public string SectionName
		{
			get { return "tileset"; }
		}

		public Type LayerType
		{
			get { return typeof(TilesetData); }
		}

		public object ReadData(KeyDataCollection keyData)
		{
			var data = new TilesetData();
			data.HasTransitions = keyData.GetBool("has_transitions", false);
			data.FlavorDensity = keyData.GetUInt16("flavor_density", 0);
			// TODO: read flavor objects
			return data;
		}

		public KeyDataCollection GetData(object obj)
		{
			throw new NotImplementedException();
		}
	}
}
