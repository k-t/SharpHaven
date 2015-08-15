using System;
using OpenTK.Input;

namespace SharpHaven.UI.Widgets
{
	public class CombatRelationClickEvent : EventArgs
	{
		public CombatRelationClickEvent(MouseButton button, CombatRelation relation)
		{
			Button = button;
			Relation = relation;
		}

		public MouseButton Button { get; }

		public CombatRelation Relation { get; }
	}
}
