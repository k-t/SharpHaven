using System.Collections.Generic;
using MonoHaven.UI.Layouts;

namespace MonoHaven.UI.Widgets
{
	public class CharWindow : Window
	{
		private Container cntAttr;
		private Container cntStudy;
		private readonly List<Container> tabs;

		private GridLayout baseLayout;
		private GridLayout skillLayout;

		public CharWindow(Widget parent) : base(parent, "Character Sheet")
		{
			Resize(400, 340);

			tabs = new List<Container>();

			InitAttributesTab();

			var cntSkill = new Container(this);
			cntSkill.Resize(400, 275);
			tabs.Add(cntSkill);

			var cntBelief = new Container(this);
			cntBelief.Resize(400, 275);
			tabs.Add(cntBelief);

			cntStudy = new Container(this);
			cntStudy.Resize(400, 275);
			tabs.Add(cntStudy);

			int bx = 10;
			AddTabButton(bx, "gfx/hud/charsh/attribup", "gfx/hud/charsh/attribdown", cntAttr);
			AddTabButton(bx += 70, "gfx/hud/charsh/ideasup", "gfx/hud/charsh/ideasdown", cntStudy);
			AddTabButton(bx += 70, "gfx/hud/charsh/skillsup", "gfx/hud/charsh/skillsdown", cntSkill);
			AddTabButton(bx += 70, "gfx/hud/charsh/worshipup", "gfx/hud/charsh/worshipdown", cntBelief);

			SetTab(cntAttr);
			Pack();
		}

		public Widget Study
		{
			get { return cntStudy; }
		}

		private void AddTabButton(int x, string image, string pressedImage, Container tab)
		{
			var button = new ImageButton(this);
			button.Move(x, 310);
			button.Image = App.Resources.GetImage(image);
			button.PressedImage = App.Resources.GetImage(pressedImage);
			button.Resize(button.Image.Size);
			button.Clicked += () => SetTab(tab);
		}

		private void SetTab(Container tab)
		{
			foreach (var t in tabs)
				t.Visible = (t == tab);
		}

		#region Attributes Tab

		private void InitAttributesTab()
		{
			cntAttr = new Container(this);
			cntAttr.Resize(400, 300);
			tabs.Add(cntAttr);

			var lblBase = new Label(cntAttr, Fonts.LabelText);
			lblBase.Move(10, 10);
			lblBase.Text = "Base Attributes:";

			baseLayout = new GridLayout();
			baseLayout.SetColumnWidth(0, 20);
			AddBase("str", "Strength");
			AddBase("agil", "Agility");
			AddBase("intel", "Intelligence");
			AddBase("cons", "Constitution");
			AddBase("perc", "Perception");
			AddBase("csm", "Charisma");
			AddBase("dxt", "Dexterity");
			AddBase("psy", "Psyche");
			baseLayout.UpdateGeometry(10, 40, 0, 0);

			var lblSkill = new Label(cntAttr, Fonts.LabelText);
			lblSkill.Move(210, 10);
			lblSkill.Text = "Skill Values:";

			skillLayout = new GridLayout();
			skillLayout.SetColumnWidth(0, 20);
			AddSkill("unarmed", "Unarmed Combat");
			AddSkill("melee", "Melee Combat");
			AddSkill("ranged", "Marksmanship");
			AddSkill("explore", "Exploration");
			AddSkill("stealth", "Stealth");
			AddSkill("sewing", "Sewing");
			AddSkill("smithing", "Smithing");
			AddSkill("carpentry", "Carpentry");
			AddSkill("cooking", "Cooking");
			AddSkill("farming", "Farming");
			AddSkill("survive", "Survival");
			skillLayout.UpdateGeometry(210, 40, 0, 0);
		}

		private void AddBase(string name, string title)
		{
			int row = baseLayout.RowCount;

			var image = new Image(cntAttr);
			image.Drawable = App.Resources.GetImage("gfx/hud/charsh/" + name);
			baseLayout.AddWidget(image, row, 0);

			var label = new Label(cntAttr, Fonts.LabelText);
			label.Text = title + ":";
			baseLayout.AddWidget(label, row, 1);
		}

		private void AddSkill(string name, string title)
		{
			int row = skillLayout.RowCount;

			var image = new Image(cntAttr);
			image.Drawable = App.Resources.GetImage("gfx/hud/charsh/" + name);
			skillLayout.AddWidget(image, row, 0);

			var label = new Label(cntAttr, Fonts.LabelText);
			label.Text = title + ":";
			skillLayout.AddWidget(label, row, 1);
		}

		#endregion
	}
}
