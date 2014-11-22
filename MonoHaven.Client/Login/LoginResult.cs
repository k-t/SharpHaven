#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

using MonoHaven.Game;

namespace MonoHaven.Login
{
	public class LoginResult
	{
		private readonly string error;
		private readonly GameSession session;

		public LoginResult(GameSession session)
		{
			this.session = session;
		}

		public LoginResult(string error)
		{
			this.error = error;
		}

		public bool IsSuccessful
		{
			get { return session != null; }
		}

		public string Error
		{
			get { return error; }
		}

		public GameSession Session
		{
			get { return session; }
		}
	}
}
