using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Input;
using SharpHaven.Client;
using SharpHaven.Input;
using SharpHaven.UI.Layouts;
using SharpHaven.Utils;

namespace SharpHaven.UI.Widgets
{
	public class CraftWindow : Window
	{
		private readonly Label lblName;
		private readonly List<Widget> inputs;
		private readonly List<Widget> outputs;
		private GridLayout inputLayout;
		private GridLayout outputLayout;

		public CraftWindow(Widget parent) : base(parent, "Craft")
		{
			inputs = new List<Widget>();
			outputs = new List<Widget>();

			inputLayout = new GridLayout();
			outputLayout = new GridLayout();

			var lblInput = new Label(this, Fonts.LabelText);
			lblInput.Move(10, 18);
			lblInput.Text = "Input:";

			var lblResult = new Label(this, Fonts.LabelText);
			lblResult.Move(10, 73);
			lblResult.Text = "Result:";

			var btnCraft = new Button(this, 60);
			btnCraft.Move(290, 71);
			btnCraft.Text = "Craft";
			btnCraft.Click += () => Craft.Raise();

			var btnCraftAll = new Button(this, 60);
			btnCraftAll.Move(360, 71);
			btnCraftAll.Text = "Craft All";
			btnCraftAll.Click += () => CraftAll.Raise();

			lblName = new Label(this, Fonts.Heading);
			lblName.AutoSize = true;
			lblName.Move(10, 10);

			IsFocusable = true;
			Margin = 15;
			Pack();
		}

		public event Action Craft;
		public event Action CraftAll;

		public string RecipeName
		{
			get { return lblName.Text; }
			set
			{
				lblName.Text = value;
				UpdateLayout();
			}
		}

		public void AddInput(Item item)
		{
			var inv = new InventoryWidget(this);
			inv.SetInventorySize(1, 1);
			var itemWidget = new ItemWidget(inv, null);
			itemWidget.Move(1, 1);
			itemWidget.Item = item;

			int column = inputLayout.ColumnCount;
			inputLayout.AddWidget(inv, 0, column);
			inputLayout.SetColumnWidth(column, 31);
			inputLayout.UpdateGeometry(50, 10, 0, 0);

			inputs.Add(inv);
		}

		public void AddOutput(Item item)
		{
			var inv = new InventoryWidget(this);
			inv.SetInventorySize(1, 1);
			var itemWidget = new ItemWidget(inv, null);
			itemWidget.Item = item;

			int column = outputLayout.ColumnCount;
			outputLayout.AddWidget(inv, 0, column);
			outputLayout.SetColumnWidth(column, 31);
			outputLayout.UpdateGeometry(50, 65, 0, 0);

			outputs.Add(inv);
		}

		public void Clear()
		{
			foreach (var item in inputs.Union(outputs))
			{
				item.Remove();
				item.Dispose();
			}
			inputLayout = new GridLayout();
			outputLayout = new GridLayout();
		}

		protected override void OnSizeChanged()
		{
			base.OnSizeChanged();
			UpdateLayout();
		}

		protected override void OnKeyDown(KeyEvent e)
		{
			if (e.Key == Key.Enter)
			{
				if (e.Modifiers.HasControl())
					CraftAll.Raise();
				else
					Craft.Raise();
				e.Handled = true;
			}
			else
				base.OnKeyDown(e);
		}

		private void UpdateLayout()
		{
			lblName.Move(Width - Margin - 10 - lblName.Width, 10);
		}
	}
}
