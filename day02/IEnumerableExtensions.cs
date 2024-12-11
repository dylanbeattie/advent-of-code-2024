public static class IEnumerableExtensions {

	public static IEnumerable<T> Intertwingle<T>(this IList<T> list, Func<T, T, T> twingle) {
		for (var i = 1; i < list.Count; i++) yield return twingle(list[i - 1], list[i]);
	}

	public static bool IsSafeAfterDampening(this IList<int> report)
		=> report.IsSafe()
		|| report.Dampen().Any(damped => damped.IsSafe());

	public static IEnumerable<IList<int>> Dampen(this IList<int> report) {
		for(var i = 0; i < report.Count(); i++) {
			yield return report.Where((_,index) => index != i).ToList();
		}
	}

	public static bool IsSafe(this IList<int> report)
		=> report.Intertwingle((a, b) => a - b).MatchesCriteria();

	public static bool MatchesCriteria(this IEnumerable<int> report)
		=> report.HasSameSign() && report.AllLevelsInRange(1, 3);

	public static bool HasSameSign(this IEnumerable<int> report)
		=> report.Skip(1).All(e => e > 0 == report.First() > 0);

	public static bool AllLevelsInRange(this IEnumerable<int> list, int lower, int upper)
		=> list.All(n => Math.Abs(n) >= lower && Math.Abs(n) <= upper);
}
