using System;
using IniParser.Model;

namespace SharpHaven.Resources.Serialization.Ini
{
	internal class NinepatchDataParser : IIniDataLayerParser
	{
		public string SectionName
		{
			get { return "ninepatch"; }
		}

		public Type LayerType
		{
			get { return typeof(NinepatchData); }
		}

		public object ReadData(KeyDataCollection keyData)
		{
			return new NinepatchData {
				Top = keyData.GetByte("top", 0),
				Bottom = keyData.GetByte("bottom", 0),
				Left = keyData.GetByte("left", 0),
				Right = keyData.GetByte("right", 0)
			};
		}

		public KeyDataCollection GetData(object obj)
		{
			var data = (NinepatchData)obj;
			var keyData = new KeyDataCollection();
			keyData.AddKey("top", data.Top);
			keyData.AddKey("bottom", data.Bottom);
			keyData.AddKey("left", data.Left);
			keyData.AddKey("rigth", data.Right);
			return keyData;
		}
	}
}
