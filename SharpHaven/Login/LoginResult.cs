using SharpHaven.Client;

namespace SharpHaven.Login
{
	public class LoginResult
	{
		private readonly string error;
		private readonly ClientSession session;

		public LoginResult(ClientSession session)
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

		public ClientSession Session
		{
			get { return session; }
		}
	}
}
