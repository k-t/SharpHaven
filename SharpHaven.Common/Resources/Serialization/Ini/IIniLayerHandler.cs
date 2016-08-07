using System;
using MadMilkman.Ini;

namespace SharpHaven.Resources.Serialization.Ini
{
	public interface IIniLayerHandler
	{
		string SectionName { get; }
		Type Type { get; }
		IniLayer Create(object data);
		IniLayer Load(IniKeyCollection keys, IFileSource fileSource);
		void Save(IniLayer layer, IniKeyCollection keys, IFileSource fileSource);
	}
}
