﻿using System;
using System.IO;

namespace MonoHaven.Resources
{
	public class ActionData
	{
		public string Name { get; set; }
		public string Prerequisite { get; set; }
		public ResourceRef Parent { get; set; }
		public char Hotkey { get; set; }
		public string[] Verbs { get; set; }
	}

	public class ActionDataSerializer : IDataLayerSerializer
	{
		public string LayerName
		{
			get { return "action"; }
		}

		public Type LayerType
		{
			get { return typeof(ActionData); }
		}

		public object Deserialize(int size, BinaryReader reader)
		{
			var action = new ActionData();
			action.Parent = new ResourceRef(reader.ReadCString(), reader.ReadUInt16());
			action.Name = reader.ReadCString();
			action.Prerequisite = reader.ReadCString();
			action.Hotkey = (char)reader.ReadUInt16();
			action.Verbs = new string[reader.ReadUInt16()];
			for (int i = 0; i < action.Verbs.Length; i++)
				action.Verbs[i] = reader.ReadCString();
			return action;
		}

		public void Serialize(BinaryWriter writer, object data)
		{
			var action = (ActionData)data;
			writer.WriteCString(action.Parent.Name);
			writer.Write(action.Parent.Version);
			writer.WriteCString(action.Name);
			writer.WriteCString(action.Prerequisite);
			writer.Write((ushort)action.Hotkey);
			writer.Write((ushort)action.Verbs.Length);
			foreach (var verb in action.Verbs)
				writer.WriteCString(verb);
		}
	}
}
