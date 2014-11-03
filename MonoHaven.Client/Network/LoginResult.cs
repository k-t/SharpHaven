namespace MonoHaven.Network
{
	public class LoginResult
	{
		private readonly string error;

		public LoginResult()
		{
		}

		public LoginResult(string error)
		{
			this.error = error;
		}

		public bool IsSuccessful
		{
			get { return string.IsNullOrEmpty(error); }
		}

		public string Error
		{
			get { return error; }
		}
	}
}
