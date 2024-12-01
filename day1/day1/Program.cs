var pairs = File.ReadAllLines("input.txt")
	.Select(line => line.Split("   "));

var list1 = pairs.ExtractAndOrderColumn(0);
var list2 = pairs.ExtractAndOrderColumn(1);

var part1 = list1.Zip(list2, (lhs,  rhs) => Math.Abs(lhs - rhs)).Sum();
Console.WriteLine($"Solution to part 1: {part1}");

var counts = list2.GroupBy(v => v)
	.ToDictionary(group => group.Key, group => group.Count());

var part2 = list1.Select(n => n * counts.GetValueOrDefault(n)).Sum();

Console.WriteLine($"Solution to part 2: {part2}");

public static class IEnumerableExtensions {
	public static IEnumerable<int> ExtractAndOrderColumn(this IEnumerable<string[]> list, int colIndex)
		=> list.Select(cols => Int32.Parse(cols[colIndex])).OrderBy(v => v);
}


