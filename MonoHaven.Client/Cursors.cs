#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

using OpenTK;

namespace MonoHaven
{
	public static class Cursors
	{
		static Cursors()
		{
			Default = App.Instance.Resources.GetCursor("gfx/hud/curs/arw");
		}

		public static MouseCursor Default
		{
			get;
			private set;
		}
	}
}
