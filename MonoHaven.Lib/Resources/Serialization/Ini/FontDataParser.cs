using System;
using System.IO;
using IniParser.Model;

namespace MonoHaven.Resources.Serialization.Ini
{
	internal class FontDataParser : IIniDataLayerParser
	{
		public string LayerName
		{
			get { return "font"; }
		}

		public Type LayerType
		{
			get { return typeof(FontData); }
		}

		public object ReadData(KeyDataCollection keyData)
		{
			return new FontData { Data = File.ReadAllBytes(keyData["file"]) };
		}

		public KeyDataCollection GetData(object obj)
		{
			throw new NotImplementedException();
		}
	}
}
