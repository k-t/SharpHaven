using System;
using OpenTK.Input;

namespace MonoHaven.UI.Widgets
{
	public class CombatRelationClickEventArgs : EventArgs
	{
		private readonly MouseButton button;
		private readonly CombatRelation relation;

		public CombatRelationClickEventArgs(MouseButton button, CombatRelation relation)
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
