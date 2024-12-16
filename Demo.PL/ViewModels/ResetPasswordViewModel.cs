namespace Demo.PL.ViewModels
{
	public class ResetPasswordViewModel
	{
		[DataType(DataType.Password)]

		public string Password { get; set; } = null!;

		[DataType(DataType.Password)]
		[Compare(nameof(Password) , ErrorMessage = "Password and ConfirmPassword does not match!")]
		public string ConfirmPassword { get; set; } = null!;
		public string Email { get; set; } = null!;
		public string Token { get; set; } = null!;
	}
}
