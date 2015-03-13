using System;
using System.Drawing;
using MonoHaven.Game;
using OpenTK.Input;

namespace MonoHaven.UI.Widgets
{
	public class MapClickEventArgs : EventArgs
	{
		private readonly MouseButton button;
		private readonly Point mapPoint;
		private readonly Point screenPoint;
		private readonly Gob gob;

		public MapClickEventArgs(
			MouseButton button,
			Point mapPoint,
			Point screenPoint,
			Gob gob)
		{
			this.button = button;
			this.mapPoint = mapPoint;
			this.screenPoint = screenPoint;
			this.gob = gob;
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
	}
}
