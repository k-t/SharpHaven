using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IniParser;
using IniParser.Model;

namespace SharpHaven.Resources.Serialization.Ini
{
	public class IniResourceSerializer : IResourceSerializer
	{
		private const string MainSectionName = "resource";

		private readonly StreamIniDataParser iniParser;
		private readonly Dictionary<string, IIniDataLayerParser> parsers;
		private readonly Dictionary<Type, IIniDataLayerParser> writers;

		public IniResourceSerializer()
		{
			iniParser = new StreamIniDataParser();
			iniParser.Parser.Configuration.AllowDuplicateKeys = true;
			iniParser.Parser.Configuration.OverrideDuplicateKeys = false;

			parsers = new Dictionary<string, IIniDataLayerParser>();
			writers = new Dictionary<Type, IIniDataLayerParser>();

			Register(new FontDataParser());
			Register(new ImageDataParser());
			Register(new NinepatchDataParser());
			Register(new TileDataParser());
			Register(new TilesetDataParser());
		}

		public void Register(IIniDataLayerParser parser)
		{
			parsers[parser.SectionName] = parser;
			writers[parser.LayerType] = parser;
		}

		public Resource Deserialize(Stream inputStream)
		{
			var fs = inputStream as FileStream;
			if (fs == null)
				throw new NotSupportedException();
			// TODO: implement some kind of path resolver instead
			Environment.CurrentDirectory = Path.GetDirectoryName(fs.Name);

			var iniData = iniParser.ReadData(new StreamReader(inputStream));

			var mainSection = iniData.Sections[MainSectionName];
			if (mainSection == null)
				throw new ResourceException("Main section is missing");

			var version = mainSection.GetInt("version", 1);
			var layers = iniData.Sections
				.Where(x => x.SectionName != MainSectionName)
				.Select(ReadLayer);

			return new Resource(version, layers);
		}

		private object ReadLayer(SectionData section)
		{
			var layerName = section.SectionName.Split('_')[0];
			IIniDataLayerParser parser;
			return parsers.TryGetValue(layerName, out parser)
				? parser.ReadData(section.Keys)
				: new UnknownDataLayer(layerName);
		}

		public void Serialize(Resource res, Stream outputStream)
		{
			throw new NotImplementedException();
		}
	}
}
