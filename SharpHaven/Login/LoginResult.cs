namespace SharpHaven.Login
{
	public class LoginResult
	{
		private readonly string error;
		private readonly string userName;
		private readonly byte[] cookie;

		public LoginResult(string userName, byte[] cookie)
		{
			this.userName = userName;
			this.cookie = cookie;
		}

		public LoginResult(string error)
		{
			this.error = error;
		}

		public bool IsSuccessful
		{
			get { return cookie != null; }
		}

		public string Error
		{
			get { return error; }
		}

		public string UserName
		{
			get { return userName; }
		}

		public byte[] Cookie
		{
			get { return cookie; }
		}
	}
}
