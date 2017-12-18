namespace VocaDbToIcal.AspNet {

	public static class StringHelpers {

		public static string Truncate(this string str, int maxLength) {
			return string.IsNullOrEmpty(str) || str.Length <= maxLength ? str : str.Substring(0, maxLength);
		}

	}

}
