﻿using System;
using SharpHaven.Client;
using SharpHaven.Graphics;
using SharpHaven.UI.Widgets;

namespace SharpHaven.UI.Remote
{
	public class ServerClaimWindow : ServerWindow
	{
		private ClaimWindow widget;
		private MapOverlay overlay;

		public ServerClaimWindow(ushort id, ServerWidget parent)
			: base(id, parent)
		{
			SetHandler("upd", UpdateArea);
			SetHandler("shared", UpdateRights);
		}

		public override Widget Widget
		{
			get { return widget; }
		}

		public static new ServerWidget Create(ushort id, ServerWidget parent)
		{
			return new ServerClaimWindow(id, parent);
		}

		protected override void OnInit(Coord2d position, object[] args)
		{
			var p1 = (Coord2d)args[0];
			var p2 = (Coord2d)args[1];
			var area = Rect.FromLTRB(p1.X, p1.Y, p2.X + 1, p2.Y + 1);

			overlay = new MapOverlay(16);
			overlay.Bounds = area;
			Session.Map.Overlays.Add(overlay);

			widget = new ClaimWindow(Parent.Widget, area);
			widget.Move(position);
			widget.Closed += OnClosed;
			widget.SelectedAreaChanged += OnSelectedAreaChanged;
			widget.Buy += OnBuy;
			widget.Declaim += OnDeclaim;
			widget.RightsChanged += OnRightsChanged;
		}

		protected override void OnDestroy()
		{
			Session.Map.Overlays.Remove(overlay);
		}

		private void UpdateArea(object[] args)
		{
			var p1 = (Coord2d)args[0];
			var p2 = (Coord2d)args[1];
			var area = Rect.FromLTRB(p1.X, p1.Y, p2.X + 1, p2.Y + 1);
			widget.Area = area;
		}

		private void UpdateRights(object[] args)
		{
			int group = (int)args[0];
			int rights = (int)args[1];
			widget.SetRight(group, (ClaimRight)rights);
		}

		private void OnClosed()
		{
			SendMessage("close");
		}

		private void OnSelectedAreaChanged(object sender, EventArgs e)
		{
			overlay.Bounds = widget.SelectedArea;
		}

		private void OnBuy(object sender, EventArgs e)
		{
			var ul = new Coord2d(widget.SelectedArea.Left, widget.SelectedArea.Top);
			var br = new Coord2d(widget.SelectedArea.Right - 1, widget.SelectedArea.Bottom - 1);
			SendMessage("take", ul, br);
		}

		private void OnDeclaim(object sender, EventArgs e)
		{
			SendMessage("declaim");
		}

		private void OnRightsChanged(object sender, ClaimRightsChangeEvent e)
		{
			SendMessage("shared", e.Group, (int)e.Rights);
		}
	}
}