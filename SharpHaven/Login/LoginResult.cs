namespace SharpHaven.Login
{
	public class LoginResult
	{
		private bool isSuccessful;
		private string errorMessage;

		private LoginResult()
		{
			this.isSuccessful = true;
		}

		private LoginResult(string errorMessage)
		{
			this.isSuccessful = false;
			this.errorMessage = errorMessage;
		}

		public string ErrorMessage
		{
			get { return errorMessage; }
		}

		public bool IsSuccessful
		{
			get { return isSuccessful; }
		}

		public static LoginResult Success()
		{
			return new LoginResult();
		}

		public static LoginResult Fail(string errorMessage)
		{
			return new LoginResult(errorMessage);
		}
	}
}
