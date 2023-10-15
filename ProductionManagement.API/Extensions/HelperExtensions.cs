using ProductionManagement.DataContract.Constant;

namespace ProductionManagement.API.Extensions
{
	public static class HelperExtensions
	{
		public static string[] SplitAndRemoveBlank(this string sourceString, char separator = Separator.CommaSeparator) {
			return sourceString.Split(separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
		}
	}
}
