var grid = File.ReadAllLines("input.txt")
	.Select(line => line.ToArray())
	.ToArray();

var costs = grid.Dijkstra('S', 'E', '#');
for (var row = 0; row < costs.Length; row++) {
	for (var col = 0; col < costs[row].Length; col++) {
		Console.Write(costs[row][col] == Int32.MaxValue ? "##" : costs[row][col].ToString("00"));
		Console.Write(" ");
	}
	Console.WriteLine();
}

List<int> cheats = new();
for(var row = 0; row < costs.Length; row++) {
	for(var col = 0; col < costs.Length; col++) {
		if (costs[row][col] == Int32.MaxValue) continue;
		if (row+2 < costs.Length && costs[row+1][col] == Int32.MaxValue && costs[row+2][col] != Int32.MaxValue && costs[row+2][col] > costs[row][col]) cheats.Add(costs[row+2][col] - costs[row][col] - 2);
		if (row-2 >= 0 && costs[row-1][col] == Int32.MaxValue && costs[row-2][col] != Int32.MaxValue && costs[row-2][col] > costs[row][col]) cheats.Add(costs[row-2][col] - costs[row][col] - 2);
		if (col + 2 < costs[row].Length && costs[row][col+1] == Int32.MaxValue && costs[row][col+2] != Int32.MaxValue && costs[row][col+2] > costs[row][col]) cheats.Add(costs[row][col+2] - costs[row][col] - 2);
		if (col - 2 >= 0 && costs[row][col-1] == Int32.MaxValue && costs[row][col-2] != Int32.MaxValue && costs[row][col-2] > costs[row][col]) cheats.Add(costs[row][col-2] - costs[row][col] - 2);
	}
}

foreach(var group in cheats.GroupBy(c => c).OrderBy(g => g.Key)) {
	Console.WriteLine(group.Count() + ": " + group.Key);
}

var part1 = cheats.Count(c => c >= 100);
Console.WriteLine("Part 1: " + part1);





public static class Extensions {
	public static U[][] Clone<T,U>(this T[][] grid, U value) {
		var result = new U[grid.Length][];
		for(var r = 0; r < grid.Length; r++) {
			var line = new U[grid[r].Length];
			for(var c = 0; c < line.Length; c++) line[c] = value;
			result[r] = line;
		}
		return result;
	}

	public static int[][] Dijkstra<T>(this T[][] grid, T origin, T target, T block) {
		var costs = grid.Clone(Int32.MaxValue);
		var start = grid.Locate(origin);
		costs[start.Row][start.Col] = 0;
		var unvisited = grid.SelectMany((line, row) => line.Select((_, col) => (Row: row, Col: col))).ToList();
		while (unvisited.Any(node => costs[node.Row][node.Col] < Int32.MaxValue)) {
			unvisited = unvisited.OrderBy(c => costs[c.Row][c.Col]).ToList();
			var node = unvisited[0];
			unvisited.RemoveAt(0);
			foreach (var nbor in new List<(int Row, int Col)> {
				(node.Row - 1, node.Col), (node.Row+1, node.Col),
				(node.Row, node.Col - 1), (node.Row, node.Col + 1) }) {
				if (nbor.Row < 0 || nbor.Col < 0) continue;
				if (nbor.Row >= costs.Length || nbor.Col >= costs[0].Length) continue;
				if (grid[nbor.Row][nbor.Col].Equals(block)) continue;
				var oldCost = costs[nbor.Row][nbor.Col];
				var newCost = costs[node.Row][node.Col] + 1;
				if (newCost < oldCost) costs[nbor.Row][nbor.Col] = newCost;
			}
		}
		return costs;
	}

	public static (int Row, int Col) Locate<T>(this T[][] grid, T needle) {
		for (var row = 0; row < grid.Length; row++)
			for (var col = 0; col < grid[row].Length; col++)
				if (needle.Equals(grid[row][col])) return (row, col);
		return (-1, -1);
	}
}



