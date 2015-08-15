namespace SharpHaven.Login
{
	public class LoginResult
	{
		public LoginResult(string userName, byte[] cookie)
		{
			UserName = userName;
			Cookie = cookie;
		}

		public LoginResult(string error)
		{
			Error = error;
		}

		public string Error { get; }

		public string UserName { get; }

		public byte[] Cookie { get; }

		public bool IsSuccessful
		{
			get { return Cookie != null; }
		}
	}
}
