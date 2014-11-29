using System.Collections.Generic;

namespace MonoHaven.UI
{
	public class CharWindow : Window
	{
		private readonly Container cntAttr;
		private readonly Container cntStudy;
		private readonly List<Container> tabs;

		public CharWindow(Widget parent) : base(parent, "Character Sheet")
		{
			Resize(400, 340);

			tabs = new List<Container>();

			cntAttr = new Container(this);
			cntAttr.Resize(400, 300);
			tabs.Add(cntAttr);

			var lblBase = new Label(cntAttr, Fonts.LabelText);
			lblBase.Move(10, 10);
			lblBase.Text = "Base Attributes:";

			var y = 25;
			AddBase(y += 15, "str", "Strength");
			AddBase(y += 15, "agil", "Agility");
			AddBase(y += 15, "intel", "Intelligence");
			AddBase(y += 15, "cons", "Constitution");
			AddBase(y += 15, "perc", "Perception");
			AddBase(y += 15, "csm", "Charisma");
			AddBase(y += 15, "dxt", "Dexterity");
			AddBase(y += 15, "psy", "Psyche");

			var lblSkill = new Label(cntAttr, Fonts.LabelText);
			lblSkill.Move(210, 10);
			lblSkill.Text = "Skill Values:";

			y = 25;
			AddSkill(y += 15, "unarmed", "Unarmed Combat");
			AddSkill(y += 15, "melee", "Melee Combat");
			AddSkill(y += 15, "ranged", "Marksmanship");
			AddSkill(y += 15, "explore", "Exploration");
			AddSkill(y += 15, "stealth", "Stealth");
			AddSkill(y += 15, "sewing", "Sewing");
			AddSkill(y += 15, "smithing", "Smithing");
			AddSkill(y += 15, "carpentry", "Carpentry");
			AddSkill(y += 15, "cooking", "Cooking");
			AddSkill(y += 15, "farming", "Farming");
			AddSkill(y += 15, "survive", "Survival");

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

		private void AddBase(int y, string name, string title)
		{
			var image = new Image(cntAttr);
			image.Move(10, y);
			image.Drawable = App.Resources.GetImage("gfx/hud/charsh/" + name);
			var label = new Label(cntAttr, Fonts.LabelText);
			label.Move(30, y);
			label.Text = title + ":";
		}

		private void AddSkill(int y, string name, string title)
		{
			var image = new Image(cntAttr);
			image.Move(210, y);
			image.Drawable = App.Resources.GetImage("gfx/hud/charsh/" + name);
			var label = new Label(cntAttr, Fonts.LabelText);
			label.Move(230, y);
			label.Text = title + ":";
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
	}
}
