using System;
using IniParser.Model;

namespace MonoHaven.Resources.Serialization.Ini
{
	public interface IIniDataLayerParser
	{
		string LayerName { get; }
		Type LayerType { get; }
		object ReadData(KeyDataCollection keyData);
		KeyDataCollection GetData(object obj);
	}
}
