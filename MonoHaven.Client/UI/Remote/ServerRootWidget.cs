#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

using MonoHaven.Game;

namespace MonoHaven.UI.Remote
{
	public class ServerRootWidget : ServerWidget
	{
		public ServerRootWidget(ushort id, GameSession session, Widget widget)
			: base(id, session, widget)
		{
		}
	}
}
