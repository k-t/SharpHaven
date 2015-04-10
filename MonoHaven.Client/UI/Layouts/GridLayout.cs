using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using SharpHaven.UI.Widgets;

namespace SharpHaven.UI.Layouts
{
	public class GridLayout
	{
		private int rowCount;
		private List<int> columns;
		private readonly Dictionary<Tuple<int, int>, Widget> cells;

		public GridLayout()
		{
			cells = new Dictionary<Tuple<int, int>, Widget>();
			columns = new List<int>();
		}

		public Padding Padding
		{
			get;
			set;
		}

		public int Spacing
		{
			get;
			set;
		}

		public int RowCount
		{
			get { return rowCount; }
		}

		public int ColumnCount
		{
			get { return columns.Count; }
		}

		public void AddWidget(Widget widget, int row, int column)
		{
			EnsureGridSize(row + 1, column + 1);
			cells[Tuple.Create(row, column)] = widget;
		}

		public void UpdateGeometry(int x, int y, int width, int height)
		{
			int cy = y + Padding.Top;
			for (int row = 0; row < RowCount; row++)
			{
				int rowHeight = 0;
				int cx = x + Padding.Left;
				for (int col = 0; col < ColumnCount; col++)
				{
					Widget widget;
					if (cells.TryGetValue(Tuple.Create(row, col), out widget))
					{
						widget.Move(cx, cy);
						cx += columns[col] + Spacing;
						if (widget.Height > rowHeight)
							rowHeight = widget.Height;
					}
				}
				cy += rowHeight + Spacing;
			}
		}

		public void UpdateGeometry(Rectangle bounds)
		{
			UpdateGeometry(bounds.X, bounds.Y, bounds.Width, bounds.Height);
		}

		public void SetColumnWidth(int column, int width)
		{
			EnsureGridSize(RowCount, column + 1);
			columns[column] = width;
		}

		private void EnsureGridSize(int rowCount, int columnCount)
		{
			if (rowCount > this.rowCount)
				this.rowCount = rowCount;
			if (columnCount > ColumnCount)
				columns.AddRange(Enumerable.Repeat(0, columnCount - ColumnCount + 1));
		}
	}
}
