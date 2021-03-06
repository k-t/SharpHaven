﻿using System;
using System.Collections.Generic;
using System.Linq;
using Haven;
using Haven.Utils;
using OpenTK.Input;
using SharpHaven.Client;
using SharpHaven.Graphics;
using SharpHaven.Graphics.Sprites;
using SharpHaven.Graphics.Text;
using SharpHaven.Input;
using SharpHaven.Utils;

namespace SharpHaven.UI.Widgets
{
	public class MapView : Widget, IItemDropTarget
	{
		private static readonly Drawable circle;
		private static readonly Drawable overlay;
		private static readonly List<Tuple<Point2D, Drawable>> overlayBorders;
		private static readonly Color[] overlayColors;

		private readonly Camera2D camera;
		private Gob placeGob;
		private int placeRadius;
		private bool placeOnTile;
		private string ownerName;
		private TextLine ownerNameText;
		private DateTime ownerShowTime;

		static MapView()
		{
			circle = App.Resources.Get<Drawable>("gfx/circle");

			var tileset = App.Resources.Get<Tileset>("gfx/tiles/ol/ol");
			var tiles = tileset.GroundTiles.ToList();
			overlay = tiles[0];
			overlayBorders = new List<Tuple<Point2D, Drawable>> {
				Tuple.Create(new Point2D(0, -1), tiles[1]),
				Tuple.Create(new Point2D(0, 1), tiles[2]),
				Tuple.Create(new Point2D(-1, 0), tiles[3]),
				Tuple.Create(new Point2D(1, 0), tiles[4])
			};

			overlayColors = new Color[31];
			overlayColors[0] = Color.FromArgb(255, 0, 128);
			overlayColors[1] = Color.FromArgb(0, 0, 255);
			overlayColors[2] = Color.FromArgb(255, 0, 0);
			overlayColors[3] = Color.FromArgb(128, 0, 255);
			overlayColors[16] = Color.FromArgb(0, 255, 0);
			overlayColors[17] = Color.FromArgb(255, 255, 0);
		}

		public MapView(Widget parent) : base(parent)
		{
			IsFocusable = true;

			camera = new FixedCamera(this);

			ownerNameText = new TextLine(Fonts.Create(FontFaces.Serif, 20));
			ownerNameText.TextColor = Color.White;
			ownerNameText.OutlineColor = Color.Black;
		}

		public event Action<MapClickEvent> MapClick;
		public event Action<KeyModifiers> ItemDrop;
		public event Action<MapClickEvent> ItemInteract;
		public event Action<MapPlaceEvent> Placed;
		public event Action<Point2D> GridRequest;

		public int PlayerId { get; set; }

		public ClientSession Session { get; set; }

		public void Place(ISprite sprite, bool snapToTile, int? radius)
		{
			if (Session == null)
				throw new InvalidOperationException();

			placeOnTile = snapToTile;
			placeRadius = radius ?? -1;
			placeGob = new Gob(-1);
			placeGob.SetSprite(new Delayed<ISprite>(sprite));

			var mc = Geometry.ScreenToMap(ToAbsolute(Host.MousePosition));
			placeGob.Position = placeOnTile ? Geometry.Tilify(mc) : mc;

			Session.Objects.AddLocal(placeGob);
		}

		public void Unplace()
		{
			if (Session == null)
				throw new InvalidOperationException();

			Session.Objects.RemoveLocal(placeGob);
			placeGob = null;
		}

		public void ShowOverlayOwner(string name)
		{
			ownerNameText.Clear();
			if (string.IsNullOrEmpty(name))
			{
				if (string.IsNullOrEmpty(ownerName))
					return;

				ownerNameText.Append("Leaving " + ownerName);
				ownerShowTime = DateTime.Now;
				ownerName = null;
			}
			else
			{
				ownerName = name;
				ownerNameText.Append("Entering " + name);
				ownerShowTime = DateTime.Now;
			}
		}

		protected override void OnDraw(DrawingContext dc)
		{
			if (Session == null)
				return;

			camera.Update();

			RequestMaps();
			DrawTiles(dc);
			DrawScene(dc);

			// draw placed object
			if (placeGob != null && placeRadius > 0)
			{
				var p = ToRelative(Geometry.MapToScreen(placeGob.Position));
				int w = (int)(placeRadius * 4 * Math.Sqrt(0.5)) * 2;
				int h = (int)(placeRadius * 2 * Math.Sqrt(0.5)) * 2;
				dc.SetColor(0, 255, 0, 32);
				dc.Draw(circle, p.X - (w / 2), p.Y - (h / 2), w, h);
				dc.ResetColor();
			}

			// draw owner name
			var span = (DateTime.Now - ownerShowTime).TotalSeconds;
			if (span < 6)
			{
				byte a;
				if (span < 1)
					a = (byte)(255 * span);
				else if (span < 4)
					a = 255;
				else
					a = (byte)((255 * (2 - (span - 4))) / 2);

				ownerNameText.TextColor = Color.FromArgb(a, Color.White);
				ownerNameText.OutlineColor = Color.FromArgb(a, Color.Black);
				dc.Draw(ownerNameText,
					(Width - ownerNameText.TextWidth) / 2,
					(Height - ownerNameText.Font.Height) / 2);
			}
		}

		private void RequestMaps()
		{
			if (PlayerId != -1)
			{
				var player = Session.Objects.Get(PlayerId);
				var ul = Geometry.MapToGrid(player.Position.Add(-500));
				var br = Geometry.MapToGrid(player.Position.Add(500));
				for (int y = ul.Y; y <= br.Y; y++)
					for (int x = ul.X; x <= br.X; x++)
						GridRequest.Raise(new Point2D(x, y));
			}
		}

		private void DrawTiles(DrawingContext g)
		{
			// get tile in the center
			var center = Geometry.ScreenToTile(camera.Position);
			// how much tiles fit onto screen vertically and horizontally
			var h = Width / (GameScene.ScreenTileWidth * 2);
			var v = Height / (GameScene.ScreenTileHeight * 2);

			// draw all tiles around the center one
			for (int x = -(h + 4); x <= h + 2; x++)
				for (int y = -(v + 3); y <= v + 2; y++)
					for (int i = 0; i < 2; i++)
					{
						int tx = center.X + x + y;
						int ty = center.Y + y - x - i;
						DrawTile(g, tx, ty);
					}
		}

		private void DrawTile(DrawingContext g, int tx, int ty)
		{
			var tile = Session.Map.GetTile(tx, ty);
			if (tile == null)
				return;
			// determine screen position
			var sc = ToRelative(Geometry.TileToScreen(tx, ty));
			// draw tile itself
			g.Draw(tile.Texture, sc.X, sc.Y);
			// draw transitions
			foreach (var trans in tile.Transitions)
				g.Draw(trans, sc.X, sc.Y);

			// draw overlays
			foreach (var overlayIndex in tile.Overlays)
			{
				var color = overlayColors[overlayIndex];
				g.SetColor(color);
				g.Draw(overlay, sc.X, sc.Y);
				foreach (var border in overlayBorders)
				{
					int dx = border.Item1.X;
					int dy = border.Item1.Y;
					var btile = Session.Map.GetTile(tx + dx, ty + dy);
					if (btile != null && !btile.Overlays.Contains(overlayIndex))
						g.Draw(border.Item2, sc.X, sc.Y);
				}
				g.ResetColor();
			}
			foreach (var mapOverlay in Session.Map.Overlays.Where(x => x.Bounds.Contains(tx, ty)))
			{
				var color = overlayColors[mapOverlay.Index];
				g.SetColor(color);
				g.Draw(overlay, sc.X, sc.Y);
				foreach (var border in overlayBorders)
				{
					int dx = border.Item1.X;
					int dy = border.Item1.Y;
					if (!mapOverlay.Bounds.Contains(tx + dx, ty + dy))
						g.Draw(border.Item2, sc.X, sc.Y);
				}
				g.ResetColor();
			}
		}

		private void DrawScene(DrawingContext g)
		{
			Session.Scene.Draw(g, Width / 2 - camera.Position.X, Height / 2 - camera.Position.Y);
		}

		protected override void OnMouseButtonDown(MouseButtonEvent e)
		{
			if (Session == null)
				return;

			camera.OnMouseButtonDown(e);
			if (e.Handled)
				return;

			var sc = ToAbsolute(e.Position);
			var mc = Geometry.ScreenToMap(sc);
			var gob = Session.Scene.GetObjectAt(sc);

			if (placeGob != null)
			{
				Placed.Raise(new MapPlaceEvent(e, placeGob.Position));
				Session.Objects.RemoveLocal(placeGob);
				placeGob = null;
			}
			else
			{
				MapClick.Raise(new MapClickEvent(e, mc, e.Position, gob));
			}
			e.Handled = true;
		}

		protected override void OnMouseButtonUp(MouseButtonEvent e)
		{
			if (Session == null)
				return;

			camera.OnMouseButtonUp(e);
			if (e.Handled)
				return;

			e.Handled = true;
		}

		protected override void OnMouseMove(MouseMoveEvent e)
		{
			if (Session == null)
				return;

			camera.OnMouseMove(e);
			if (e.Handled)
				return;

			if (placeGob != null)
			{
				var mc = Geometry.ScreenToMap(ToAbsolute(e.Position));
				var snap = placeOnTile ^ e.Modifiers.HasShift();
				placeGob.Position = snap ? Geometry.Tilify(mc) : mc;
			}
		}

		protected override void OnDispose()
		{
			ownerNameText.Dispose();
		}

		/// <summary>
		/// Converts absolute screen coordinate to a relative to the current viewport.
		/// </summary>
		private Point2D ToRelative(Point2D abs)
		{
			return new Point2D(
				abs.X + Width / 2 - camera.Position.X,
				abs.Y + Height / 2 - camera.Position.Y);
		}

		/// <summary>
		/// Converts relative screen coordinate to absolute.
		/// </summary>
		private Point2D ToAbsolute(Point2D rel)
		{
			return new Point2D(
				rel.X - Width / 2 + camera.Position.X,
				rel.Y - Height / 2 + camera.Position.Y);
		}

		#region IItemDropTarget

		bool IItemDropTarget.Drop(Point2D p, Point2D ul, KeyModifiers mods)
		{
			ItemDrop.Raise(mods);
			return true;
		}

		bool IItemDropTarget.Interact(Point2D p, Point2D ul, KeyModifiers mods)
		{
			if (Session == null)
				return false;

			var sc = ToAbsolute(p);
			var mc = Geometry.ScreenToMap(sc);
			var gob = Session.Scene.GetObjectAt(sc);
			ItemInteract.Raise(new MapClickEvent(0, mods, mc, p, gob));
			return true;
		}

		#endregion

		#region Cameras

		public abstract class Camera2D
		{
			protected readonly MapView mv;

			protected Camera2D(MapView mv)
			{
				this.mv = mv;
			}

			public Point2D Position
			{
				get
				{
					return Geometry.MapToScreen(mv.Session.WorldPosition);
				}
				protected set
				{
					mv.Session.WorldPosition = Geometry.ScreenToMap(value);
				}
			}

			public virtual void Update()
			{
			}

			public virtual void OnMouseButtonDown(MouseButtonEvent e)
			{
			}

			public virtual void OnMouseButtonUp(MouseButtonEvent e)
			{
			}

			public virtual void OnMouseMove(MouseMoveEvent e)
			{
			}
		}

		public class FreeCamera : Camera2D
		{
			private bool dragging;
			private Point2D dragPosition;
			private Point2D dragCameraOffset;

			public FreeCamera(MapView mv) : base(mv)
			{
				// TODO: this won't work well if we'll need multiple camera instances!
				mv.Host.Hotkeys.Register(Key.Up, () => Move(0, -50));
				mv.Host.Hotkeys.Register(Key.Down, () => Move(0, 50));
				mv.Host.Hotkeys.Register(Key.Left, () => Move(-50, 0));
				mv.Host.Hotkeys.Register(Key.Right, () => Move(50, 0));
				mv.Host.Hotkeys.Register(Key.Home, MoveToCenter);
				mv.Host.Hotkeys.Register(Key.Keypad7, MoveToCenter);
			}

			private void Move(int deltaX, int deltaY)
			{
				Position = Position.Add(deltaX, deltaY);
			}

			private void MoveToCenter()
			{
				if (mv.PlayerId != -1)
				{
					var player = mv.Session.Objects.Get(mv.PlayerId);
					mv.Session.WorldPosition = player.Position;
				}
			}

			public override void OnMouseButtonDown(MouseButtonEvent e)
			{
				if (e.Button == MouseButton.Middle)
				{
					mv.Host.GrabMouse(mv);
					dragging = true;
					dragPosition = e.Position;
					dragCameraOffset = Position;
					e.Handled = true;
				}
			}

			public override void OnMouseButtonUp(MouseButtonEvent e)
			{
				if (e.Button == MouseButton.Middle)
				{
					mv.Host.ReleaseMouse();
					dragging = false;
					e.Handled = true;
				}
			}

			public override void OnMouseMove(MouseMoveEvent e)
			{
				if (dragging)
				{
					Position = dragCameraOffset.Add(dragPosition.Sub(e.Position));
					e.Handled = true;
				}
			}
		}

		public class FixedCamera : Camera2D
		{
			public FixedCamera(MapView mv) : base(mv)
			{
			}

			public override void Update()
			{
				if (mv.PlayerId != -1)
				{
					var player = mv.Session.Objects.Get(mv.PlayerId);
					mv.Session.WorldPosition = player.Position;
				}
			}
		}

		#endregion
	}
}