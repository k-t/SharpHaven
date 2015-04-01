using System;
using IniParser.Model;
using System.IO;

namespace MonoHaven.Resources.Serialization.Ini
{
	internal class ImageDataParser : IIniDataLayerParser
	{
		public string SectionName
		{
			get { return "image"; }
		}

		public Type LayerType
		{
			get { return typeof(ImageData); }
		}

		public object ReadData(KeyDataCollection keyData)
		{
			return new ImageData { Data = File.ReadAllBytes(keyData["file"]) };
		}

		public KeyDataCollection GetData(object obj)
		{
			throw new NotImplementedException();
		}
	}
}
