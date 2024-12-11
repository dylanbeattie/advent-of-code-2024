var grid = File.ReadAllLines("input.txt")
	.Select(line => line.ToArray())
	.ToArray();

Dictionary<char, List<(int, int)>> antennae = [];
for (var row = 0; row < grid.Length; row++) {
	for (var col = 0; col < grid[0].Length; col++) {
		var cell = grid[row][col];
		if (cell == '.') continue;
		antennae.TryAdd(cell, new());
		antennae[cell].Add((row, col));
	}
}

HashSet<(int, int)> part1 = new();
HashSet<(int, int)> part2 = new();

foreach (var (key, list) in antennae) {
	foreach (var antinode in FindAntinodesPart1(list, grid)) part1.Add(antinode);
	foreach (var antinode in FindAntinodesPart2(list, grid)) part2.Add(antinode);
}

Console.WriteLine($"Part 1: {part1.Count}");
Console.WriteLine($"Part 2: {part2.Count}");

IEnumerable<(int Row, int Col)> FindAntinodesPart2(List<(int, int)> antennae, char[][] grid) {
	foreach (var (l, r) in ExtractPair(antennae.Count)) {
		var lhs = antennae[l];
		var rhs = antennae[r];
		var delta = rhs.Minus(lhs);
		while (grid.Contains(lhs)) {
			yield return lhs;
			lhs = lhs.Minus(delta);
		}
		while (grid.Contains(rhs)) {
			yield return rhs;
			rhs = rhs.Plus(delta);
		}
	}
}

IEnumerable<(int Row, int Col)> FindAntinodesPart1(List<(int, int)> antennae, char[][] grid) {
	foreach (var (l, r) in ExtractPair(antennae.Count)) {
		var lhs = antennae[l];
		var rhs = antennae[r];
		var delta = rhs.Minus(lhs);
		lhs = lhs.Minus(delta);
		rhs = rhs.Plus(delta);
		if (grid.Contains(lhs)) yield return lhs;
		if (grid.Contains(rhs)) yield return rhs;
	}
}

IEnumerable<(int A, int B)> ExtractPair(int count) {
	for (var a = 0; a < count; a++) {
		for (var b = a + 1; b < count; b++) yield return (a, b);
	}
}

public static class Extensions {
	public static (int Row, int Col) Plus(this (int Row, int Col) lhs, (int Row, int Col) rhs)
		=> (lhs.Row + rhs.Row, lhs.Col + rhs.Col);
	public static (int Row, int Col) Minus(this (int Row, int Col) lhs, (int Row, int Col) rhs)
		=> (lhs.Row - rhs.Row, lhs.Col - rhs.Col);

	public static (int Row, int Col) Delta(this (int Row, int Col) lhs, (int Row, int Col) rhs)
		=> (Math.Abs(lhs.Row - rhs.Row), Math.Abs(lhs.Col - rhs.Col));

	public static bool Contains<T>(this T[][] grid, (int row, int col) pair)
		=> pair.row >= 0 && pair.row < grid.Length
		&& pair.col >= 0 && pair.col < grid[0].Length;
}
