using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using MonoHaven.Game;
using MonoHaven.Graphics;
using MonoHaven.UI.Layouts;

namespace MonoHaven.UI.Widgets
{
	public class CharWindow : Window
	{
		private static readonly Color DebuffColor = Color.FromArgb(255, 128, 128);
		private static readonly Color BuffColor = Color.FromArgb(128, 255, 128);

		private static readonly Drawable MinusUp;
		private static readonly Drawable MinusDown;
		private static readonly Drawable PlusUp;
		private static readonly Drawable PlusDown;

		static CharWindow()
		{
			MinusUp = App.Resources.GetImage("gfx/hud/charsh/minusup");
			MinusDown = App.Resources.GetImage("gfx/hud/charsh/minusdown");
			PlusUp = App.Resources.GetImage("gfx/hud/charsh/plusup");
			PlusDown = App.Resources.GetImage("gfx/hud/charsh/plusdown");
		}

		private readonly GameState gstate;
		private int exp;
		private List<SkillAttributeView> skills;

		private Container cntAttr;
		private Container cntStudy;
		private Label lblExpValue;
		private Label lblCostValue;
		private readonly List<Container> tabs;
		private GridLayout baseLayout;
		private GridLayout skillLayout;

		public CharWindow(Widget parent, GameState gstate) : base(parent, "Character Sheet")
		{
			Resize(400, 340);

			this.gstate = gstate;
			this.tabs = new List<Container>();
			this.skills = new List<SkillAttributeView>();

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

		public event Action<object[]> AttributesBought;

		public Widget Study
		{
			get { return cntStudy; }
		}

		public void SetExp(int value)
		{
			exp = value;
			lblExpValue.Text = value.ToString();
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

			// LP
			var lblCost = new Label(cntAttr, Fonts.LabelText);
			lblCost.Text = "Cost:";

			lblCostValue = new Label(cntAttr, Fonts.LabelText);
			lblCostValue.Text = "0";

			var lblExp = new Label(cntAttr, Fonts.LabelText);
			lblExp.Text = "Learning Points:";

			lblExpValue = new Label(cntAttr, Fonts.LabelText);
			lblExpValue.Text = "0";

			var lblExpMod = new Label(cntAttr, Fonts.LabelText);
			lblExpMod.Text = "Learning Ability:";

			var lblExpModValue = new Label(cntAttr, Fonts.LabelText);
			var expModView = new ExpView(gstate.GetAttr("expmod"), lblExpModValue);

			var btnBuy = new Button(cntAttr, 75);
			btnBuy.Text = "Buy";
			btnBuy.Clicked += BuySkills;

			var lpLayout = new GridLayout();
			lpLayout.AddWidget(lblCost, 0, 0);
			lpLayout.AddWidget(lblCostValue, 0, 1);
			lpLayout.AddWidget(lblExp, 1, 0);
			lpLayout.AddWidget(lblExpValue, 1, 1);
			lpLayout.AddWidget(lblExpMod, 2, 0);
			lpLayout.AddWidget(lblExpModValue, 2, 1);
			lpLayout.AddWidget(btnBuy, 3, 0);
			lpLayout.SetColumnWidth(0, 90);
			lpLayout.UpdateGeometry(210, 220, 0, 0);

			// Base Attributes
			var lblBase = new Label(cntAttr, Fonts.LabelText);
			lblBase.Move(10, 10);
			lblBase.Text = "Base Attributes:";

			baseLayout = new GridLayout();
			baseLayout.SetColumnWidth(0, 20);
			baseLayout.SetColumnWidth(1, 70);
			AddBase("str", "Strength");
			AddBase("agil", "Agility");
			AddBase("intel", "Intelligence");
			AddBase("cons", "Constitution");
			AddBase("perc", "Perception");
			AddBase("csm", "Charisma");
			AddBase("dxt", "Dexterity");
			AddBase("psy", "Psyche");
			baseLayout.UpdateGeometry(10, 40, 0, 0);

			// Skills
			var lblSkill = new Label(cntAttr, Fonts.LabelText);
			lblSkill.Move(210, 10);
			lblSkill.Text = "Skill Values:";

			skillLayout = new GridLayout();
			skillLayout.SetColumnWidth(0, 20);
			skillLayout.SetColumnWidth(1, 90);
			skillLayout.SetColumnWidth(2, 30);
			skillLayout.SetColumnWidth(3, 15);
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
			var attr = gstate.GetAttr(name);

			var image = new Image(cntAttr);
			image.Drawable = App.Resources.GetImage("gfx/hud/charsh/" + name);

			var lblName = new Label(cntAttr, Fonts.LabelText);
			lblName.Text = title + ":";

			var lblValue = new Label(cntAttr, Fonts.LabelText);
			lblValue.Text = attr.ComputedValue.ToString();
			lblValue.TextColor = GetAttributeColor(attr);

			int row = baseLayout.RowCount;
			baseLayout.AddWidget(image, row, 0);
			baseLayout.AddWidget(lblName, row, 1);
			baseLayout.AddWidget(lblValue, row, 2);
		}

		private void AddSkill(string name, string title)
		{
			var attr = gstate.GetAttr(name);

			var image = new Image(cntAttr);
			image.Drawable = App.Resources.GetImage("gfx/hud/charsh/" + name);
			
			var lblName = new Label(cntAttr, Fonts.LabelText);
			lblName.Text = title + ":";
			
			var lblValue = new Label(cntAttr, Fonts.LabelText);
			var skill = new SkillAttributeView(attr, lblValue);
			skill.CostChanged += UpdateCost;
			skills.Add(skill);
			
			var btnMinus = new ImageButton(cntAttr);
			btnMinus.Image = MinusUp;
			btnMinus.PressedImage = MinusDown;
			btnMinus.Clicked += () => skill.Add(-1);
			btnMinus.MouseWheel += arg => skill.Add(Math.Sign(arg.Delta));

			var btnPlus = new ImageButton(cntAttr);
			btnPlus.Image = PlusUp;
			btnPlus.PressedImage = PlusDown;
			btnPlus.Clicked += () => skill.Add(1);
			btnPlus.MouseWheel += arg => skill.Add(Math.Sign(arg.Delta));

			int row = skillLayout.RowCount;
			skillLayout.AddWidget(image, row, 0);
			skillLayout.AddWidget(lblName, row, 1);
			skillLayout.AddWidget(lblValue, row, 2);
			skillLayout.AddWidget(btnMinus, row, 3);
			skillLayout.AddWidget(btnPlus, row, 4);
		}

		private void UpdateCost()
		{
			int cost = 0;
			foreach (var skill in skills)
				cost += skill.Cost;

			lblCostValue.Text = cost.ToString();
			lblCostValue.TextColor = cost > exp ? Color.FromArgb(255, 128, 128) : Color.White;
		}

		private void BuySkills()
		{
			var args = new List<object>();
			foreach (var skill in skills)
			{
				args.Add(skill.Name);
				args.Add(skill.CurrentBaseValue);
			}
			AttributesBought.Raise(args.ToArray());
		}

		#endregion

		private Color GetAttributeColor(CharAttribute attr)
		{
			if (attr.ComputedValue > attr.BaseValue)
				return BuffColor;
			if (attr.ComputedValue < attr.BaseValue)
				return DebuffColor;
			return Color.White;
		}

		private abstract class CharAttributeView
		{
			protected readonly CharAttribute attribute;
			private readonly Label label;

			protected CharAttributeView(CharAttribute attribute, Label label)
			{
				this.label = label;
				this.attribute = attribute;
				this.attribute.Changed += OnAttributeChange;
				UpdateLabel();
			}

			protected abstract string DisplayValue { get; }
			protected abstract Color DisplayColor { get; }

			protected virtual void OnAttributeChange()
			{
				UpdateLabel();
			}

			protected void UpdateLabel()
			{
				label.Text = DisplayValue;
				label.TextColor = DisplayColor;
			}
		}

		private class ExpView : CharAttributeView
		{
			public ExpView(CharAttribute attribute, Label label)
				: base(attribute, label)
			{
			}

			protected override string DisplayValue
			{
				get { return string.Format("{0}%", attribute.ComputedValue); }
			}

			protected override Color DisplayColor
			{
				get
				{
					if (attribute.ComputedValue > 100)
						return BuffColor;
					if (attribute.ComputedValue < 100)
						return DebuffColor;
					return Color.White;
				}
			}
		}

		private class SkillAttributeView : CharAttributeView
		{
			private int baseValue;
			private int compValue;
			private int cost;

			public SkillAttributeView(CharAttribute attribute, Label label)
				: base(attribute, label)
			{
				baseValue = attribute.BaseValue;
				compValue = attribute.ComputedValue;
				UpdateLabel();
			}

			public event Action CostChanged;

			protected override string DisplayValue
			{
				get { return compValue.ToString(); }
			}

			protected override Color DisplayColor
			{
				get
				{
					if (baseValue > attribute.BaseValue)
						return Color.FromArgb(128, 128, 255);
					if (attribute.ComputedValue > attribute.BaseValue)
						return BuffColor;
					if (attribute.ComputedValue < attribute.BaseValue)
						return DebuffColor;
					return Color.White;
				}
			}

			public string Name
			{
				get { return attribute.Name; }
			}

			public int CurrentBaseValue
			{
				get { return baseValue; }
			}

			public int Cost
			{
				get { return cost; }
			}

			public void Add(int value)
			{
				if (baseValue + value < attribute.BaseValue)
					return;
				
				baseValue += value;
				compValue += value;
				UpdateLabel();

				if (value > 0)
					cost += baseValue * 100;
				else
					cost -= (baseValue - value) * 100;
				CostChanged.Raise();
			}

			protected override void OnAttributeChange()
			{
				baseValue = attribute.BaseValue;
				compValue = attribute.ComputedValue;
				cost = 0;
				base.OnAttributeChange();
				CostChanged.Raise();
			}
		}
	}
}
