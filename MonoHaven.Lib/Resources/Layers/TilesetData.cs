﻿using System;
using System.IO;

namespace MonoHaven.Resources
{
	public class TilesetData
	{
		public bool HasTransitions { get; set; }
		public FlavorObjectData[] FlavorObjects { get; set; }
		public ushort FlavorDensity { get; set; }
	}

	public struct FlavorObjectData
	{
		public string ResName { get; set; }
		public ushort ResVersion { get; set; }
		public byte Weight { get; set; }
	}

	public class TilesetDataSerializer : IDataLayerSerializer
	{
		public string LayerName
		{
			get { return "tileset"; }
		}

		public Type LayerType
		{
			get { return typeof(TilesetData); }
		}

		public object Deserialize(int size, BinaryReader reader)
		{
			var tileset = new TilesetData();
			tileset.HasTransitions = reader.ReadBoolean();
			var flavorCount = reader.ReadUInt16();
			tileset.FlavorDensity = reader.ReadUInt16();
			tileset.FlavorObjects = new FlavorObjectData[flavorCount];
			for (int i = 0; i < flavorCount; i++)
			{
				var fob = new FlavorObjectData();
				fob.ResName = reader.ReadCString();
				fob.ResVersion = reader.ReadUInt16();
				fob.Weight = reader.ReadByte();
				tileset.FlavorObjects[i] = fob;
			}
			return tileset;
		}

		public void Serialize(BinaryWriter writer, object data)
		{
			var tileset = (TilesetData)data;
			writer.Write(tileset.HasTransitions);
			writer.Write((ushort)tileset.FlavorObjects.Length);
			writer.Write(tileset.FlavorDensity);
			foreach (var fob in tileset.FlavorObjects)
			{
				writer.WriteCString(fob.ResName);
				writer.Write(fob.ResVersion);
				writer.Write(fob.Weight);
			}
		}
	}
}
