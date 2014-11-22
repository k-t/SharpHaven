#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

namespace MonoHaven.Login
{
	public class LoginSettings
	{
		public string AuthHost { get; set; }
		public int AuthPort { get; set; }
		public string GameHost { get; set; }
		public int GamePort { get; set; }
	}
}
