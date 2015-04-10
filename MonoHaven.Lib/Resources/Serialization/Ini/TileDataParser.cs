using System;
using System.IO;
using IniParser.Model;

namespace SharpHaven.Resources.Serialization.Ini
{
	public class TileDataParser : IIniDataLayerParser
	{
		public string SectionName
		{
			get { return "tile"; }
		}

		public Type LayerType
		{
			get { return typeof(TileData); }
		}

		public object ReadData(KeyDataCollection keyData)
		{
			var data = new TileData();
			data.Type = keyData.GetChar("type");
			data.Id = keyData.GetByte("id");
			data.Weight = keyData.GetUInt16("weight", 0);
			data.ImageData = File.ReadAllBytes(keyData["image"]);
			return data;
		}

		public KeyDataCollection GetData(object obj)
		{
			throw new NotImplementedException();
		}
	}
}
