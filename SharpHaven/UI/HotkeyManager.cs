using System;
using System.Collections.Generic;
using NLog;
using OpenTK.Input;

namespace SharpHaven.UI
{
	public class HotkeyManager
	{
		private static readonly Logger Log = LogManager.GetCurrentClassLogger();

		private readonly Dictionary<Tuple<Key, KeyModifiers>, Action> hotkeys;

		public HotkeyManager()
		{
			this.hotkeys = new Dictionary<Tuple<Key, KeyModifiers>, Action>();
		}

		public Action GetAction(Key key, KeyModifiers modifiers)
		{
			var entry = Tuple.Create(key, modifiers);
			Action action;
			return hotkeys.TryGetValue(entry, out action) ? action : null;
		}

		public void Register(Key key, Action command)
		{
			Register(key, 0, command);
		}

		public void Register(Key key, KeyModifiers modifiers, Action command)
		{
			var entry = Tuple.Create(key, modifiers);
			if (hotkeys.ContainsKey(entry))
				Log.Warn("Hotkey '{1}+{0}' was already registered!", key, modifiers);
			hotkeys[entry] = command;
		}
	}
}
