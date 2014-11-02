namespace MonoHaven.Network
{
	public class AuthResult
	{
		private readonly byte[] cookie;
		private readonly string error;

		public AuthResult(string error)
		{
			this.error = error;
		}

		public AuthResult(byte[] cookie)
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
