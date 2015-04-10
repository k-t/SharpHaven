using System;
using OpenTK.Input;

namespace SharpHaven.UI.Widgets
{
	public class CombatRelationClickEvent : EventArgs
	{
		private readonly MouseButton button;
		private readonly CombatRelation relation;

		public CombatRelationClickEvent(MouseButton button, CombatRelation relation)
		{
			this.button = button;
			this.relation = relation;
		}

		public MouseButton Button
		{
			get { return button; }
		}

		public CombatRelation Relation
		{
			get { return relation; }
		}
	}
}
