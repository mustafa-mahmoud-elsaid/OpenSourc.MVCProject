namespace Demo.PL.Utilities
{
	public static class StringExtensions
	{
		public static string ControllerName(this string controller) =>
		 controller.Replace("Controller", string.Empty);
	}
}
