﻿using System;
using System.Drawing;
using System.Linq;
using MonoHaven.Game;
using MonoHaven.Graphics;
using MonoHaven.Input;
using OpenTK.Input;

namespace MonoHaven.UI.Widgets
{
	public class MenuGrid : Widget
	{
		private const int RowCount = 4;
		private const int ColumnCount = 4;

		private static readonly Drawable backImage;
		private static readonly Drawable nextImage;
		private static readonly Drawable cellImage;
		private static readonly int cellWidth;
		private static readonly int cellHeight;

		static MenuGrid()
		{
			backImage = App.Resources.Get<Drawable>("gfx/hud/sc-back");
			nextImage = App.Resources.Get<Drawable>("gfx/hud/sc-next");
			cellImage = App.Resources.Get<Drawable>("gfx/hud/invsq");
			cellWidth = cellImage.Width - 1;
			cellHeight = cellImage.Height - 1;
		}

		private readonly GameActionTree actionTree;
		private readonly GridButton[,] buttons;
		private readonly GridButton back;
		private readonly GridButton next;
		private GameAction current;
		private GridButton pressed;

		public MenuGrid(Widget parent, GameActionTree actionTree) : base(parent)
		{
			this.actionTree = actionTree;
			this.actionTree.Changed += UpdateCells;
			this.current = actionTree.Root;
			this.buttons = new GridButton[RowCount, ColumnCount];
			this.back = new GridButton(backImage);
			this.next = new GridButton(nextImage);
			Resize(cellWidth * ColumnCount, cellHeight * RowCount);
		}

		public event Action<GameAction> ActionSelected;

		public void SetCurrentAction(string resName)
		{
			current = string.IsNullOrEmpty(resName)
				? actionTree.Root
				: actionTree.Get(resName) ?? actionTree.Root;
			UpdateCells();
		}

		protected override void OnDraw(DrawingContext dc)
		{
			for (int i = 0; i < RowCount; i++)
				for (int j = 0; j < ColumnCount; j++)
				{
					int x = cellWidth * j;
					int y = cellHeight * i;
					dc.Draw(cellImage, x, y);
					var button = buttons[i, j];
					if (button != null)
						button.Draw(dc, x, y, button == pressed);
				}
		}

		protected override void OnMouseButtonDown(MouseButtonEvent e)
		{
			if (e.Button == MouseButton.Left)
			{
				var button = GetButtonAt(MapFromScreen(e.Position));
				if (button != null)
				{
					pressed = button;
					Host.GrabMouse(this);
				}
			}
			e.Handled = true;
		}

		protected override void OnMouseButtonUp(MouseButtonEvent e)
		{
			if (e.Button == MouseButton.Left)
			{
				if (pressed != null)
				{
					if (pressed == back)
						current = current.Parent;
					else if (pressed.Action.HasChildren)
						current = pressed.Action;
					else
						ActionSelected.Raise(pressed.Action);
					UpdateCells();
				}
				pressed = null;
				Host.ReleaseMouse();
			}
			e.Handled = true;
		}

		private void UpdateCells()
		{
			var children = current.Children.ToArray();
			Array.Sort(children);
			for (int i = 0; i < RowCount; i++)
				for (int j = 0; j < ColumnCount; j++)
					if (i * ColumnCount + j < children.Length)
						buttons[i, j] = new GridButton(children[i * ColumnCount + j]);
					else
						buttons[i, j] = null;

			if (current.Parent != null)
				buttons[RowCount - 1, ColumnCount - 1] = back;
		}

		private GridButton GetButtonAt(Point p)
		{
			int row = p.Y / cellHeight;
			int col = p.X / cellWidth;
			if (row >= 0 && row < RowCount && col >= 0 && col < ColumnCount)
				return buttons[row, col];
			return null;
		}

		private class GridButton
		{
			private readonly GameAction action;
			private readonly Drawable image;

			public GridButton(Drawable image)
			{
				this.image = image;
			}

			public GridButton(GameAction action)
			{
				this.action = action;
				this.image = action.Image;
			}

			public GameAction Action
			{
				get { return action; }
			}

			public void Draw(DrawingContext dc, int x, int y, bool isPressed)
			{
				dc.Draw(image, x + 1, y + 1);
				if (isPressed)
				{
					dc.SetColor(0, 0, 0, 128);
					dc.DrawRectangle(x, y, cellWidth, cellHeight);
					dc.ResetColor();
				}
			}
		}
	}
}