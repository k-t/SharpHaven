using System;
using System.Linq;
using SharpHaven.Graphics;

namespace SharpHaven.UI.Widgets
{
	public class BuddyList : Widget
	{
		private static readonly Drawable online;
		private static readonly Drawable offline;

		static BuddyList()
		{
			online = App.Resources.Get<Drawable>("gfx/hud/online");
			offline = App.Resources.Get<Drawable>("gfx/hud/offline");
		}

		private readonly ListBox listBox;
		private readonly Label lblAlone;
		private Comparison<Buddy> sortFunc;

		public BuddyList(Widget parent) : base(parent)
		{
			listBox = new ListBox(this);
			listBox.SelectedIndexChanged += () => SelectedItemChanged.Raise();
			lblAlone = new Label(this, Fonts.LabelText);
			lblAlone.AutoSize = true;
			lblAlone.Text = "You are alone in the world";
		}

		public event Action SelectedItemChanged;

		public Buddy SelectedItem
		{
			get { return (Buddy)listBox.SelectedItem?.Tag; }
		}

		public void Add(Buddy buddy)
		{
			listBox.AddItem(ToListBoxItem(buddy));
			Update();
		}

		public void Remove(int buddyId)
		{
			listBox.RemoveItem(FindListItem(buddyId));
			Update();
		}

		public void SelectItem(int buddyId)
		{
			listBox.SelectedItem =  FindListItem(buddyId);
		}

		public void SetSort(Comparison<Buddy> sortFunc)
		{
			this.sortFunc = sortFunc;
			Sort();
		}

		public void SetStatus(int buddyId, int status)
		{
			var item = FindListItem(buddyId);
			if (item != null)
				item.Image = GetStatusImage(status);
		}

		public void SetName(int buddyId, string name)
		{
			var item = FindListItem(buddyId);
			if (item != null)
				item.Text = name;
		}

		public void SetGroup(int buddyId, int group)
		{
			var item = FindListItem(buddyId);
			if (item != null)
				item.TextColor = BuddyGroup.Colors[group];
		}

		protected override void OnSizeChanged()
		{
			lblAlone.Move((Width - lblAlone.Width) / 2, (Height - lblAlone.Height) / 2);
			listBox.Resize(Width, Height);
		}

		private ListBoxItem FindListItem(int buddyId)
		{
			return listBox.Items.FirstOrDefault(x => ((Buddy)x.Tag).Id == buddyId);
		}

		private void Update()
		{
			lblAlone.Visible = (listBox.Items.Count == 0);
			Sort();
		}

		private ListBoxItem ToListBoxItem(Buddy buddy)
		{
			var item = new ListBoxItem(
				GetStatusImage(buddy.OnlineStatus),
				buddy.Name,
				BuddyGroup.Colors[buddy.Group]);
			item.Tag = buddy;
			return item;
		}

		private Drawable GetStatusImage(int buddyStatus)
		{
			if (buddyStatus == 0)
				return offline;
			if (buddyStatus == 1)
				return online;
			return null;
		}

		private void Sort()
		{
			listBox.Sort((a, b) => sortFunc((Buddy)a.Tag, (Buddy)b.Tag));
		}
	}
}
