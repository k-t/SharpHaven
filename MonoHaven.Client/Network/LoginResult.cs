namespace MonoHaven.Network
{
	public class LoginResult
	{
		private readonly byte[] cookie;
		private readonly string error;

		public LoginResult(string error)
		{
			this.error = error;
		}

		public LoginResult(byte[] cookie)
		{
			this.cookie = cookie;
		}

		public bool IsSuccessful
		{
			get { return cookie != null; }
		}

		public string Error
		{
			get { return error; }
		}
	}
}
