﻿using System;
using System.Drawing;
using MonoHaven.Game;
using OpenTK.Input;
using KeyModifiers = MonoHaven.Input.KeyModifiers;

namespace MonoHaven.UI.Widgets
{
	public class MapClickEventArgs : EventArgs
	{
		private readonly MouseButton button;
		private readonly KeyModifiers mods;
		private readonly Point mapPoint;
		private readonly Point screenPoint;
		private readonly Gob gob;

		public MapClickEventArgs(
			MouseButton button,
			KeyModifiers mods,
			Point mapPoint,
			Point screenPoint,
			Gob gob)
		{
			this.button = button;
			this.mapPoint = mapPoint;
			this.screenPoint = screenPoint;
			this.gob = gob;
			this.mods = mods;
		}

		public MouseButton Button
		{
			get { return button; }
		}

		public Gob Gob
		{
			get { return gob; }
		}

		public Point MapPoint
		{
			get { return mapPoint; }
		}

		public Point ScreenPoint
		{
			get { return screenPoint; }
		}

		public KeyModifiers Modifiers
		{
			get { return mods; }
		}
	}
}