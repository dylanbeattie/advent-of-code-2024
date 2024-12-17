var grid = File.ReadAllLines("scratch1.txt")
	.Select(line => line.ToCharArray()).ToArray();

var costs = grid.Clone(() => Int32.MaxValue);
var reindeer = grid.Find('S');
var target = grid.Find('E');

costs[reindeer.Row][reindeer.Col] = 0;

var prev = grid.Clone(() => new List<(int, int)>());

grid.Search(costs, reindeer.Row, reindeer.Col, 'e', prev);

foreach (var s1 in prev[target.Row][target.Col]) {
	foreach (var s2 in prev[s1.Item1][s1.Item2]) {
		foreach (var s3 in prev[s2.Item1][s2.Item2]) {
			foreach (var s4 in prev[s3.Item1][s3.Item2]) {
				foreach (var s5 in prev[s4.Item1][s4.Item2]) {
					Console.WriteLine(s5 + " > " + s4 + " > " + s3 + " > " + s2 + " > " + s1);
				}
				Console.WriteLine(s4 + " > " + s3 + " > " + s2 + " > " + s1);
			}
			Console.WriteLine(s3 + " > " + s2 + " > " + s1);
		}
		Console.WriteLine(s2 + " > " + s1);
	}
	Console.WriteLine(s1);
}

// var bucket = new List<string>();
// prev.Backtrack(target.Row, target.Col, bucket);
// foreach(var thing in bucket) Console.WriteLine(thing);

for (var row = 0; row < prev.Length; row++) {
	for (var col = 0; col < prev[row].Length; col++) {
		Console.WriteLine($"{row},{col}:      " + String.Join(" ", prev[row][col].ToArray()));
	}
}

for (var row = 0; row < costs.Length; row++) {
	for (var col = 0; col < costs[row].Length; col++) {
		var cost = costs[row][col];
		Console.ForegroundColor = grid[row][col] switch {
			'E' => ConsoleColor.Green,
			'S' => ConsoleColor.Red,
			_ => ConsoleColor.White
		};
		if (cost == Int32.MaxValue) {
			Console.Write("###### ");
		} else {
			Console.Write($"{cost:000000} ");
		}
	}
	Console.WriteLine();
}

Console.WriteLine(costs[target.Row][target.Col]);

public static class Extensions {

	public static void Backtrack(this List<(int, int)>[][] prev, int row, int col) {
		if (prev[row][col].Count == 0) return;;
		List<string> lists = [];
		foreach(var pair in prev[row][col]) {
			foreach(var list in prev.Backtrack(pair.Item1, pair.Item2)) {
				Console.Write(list);
			}
		}
	}


	private static Dictionary<char, (char Next, int Row, int Col, int Cost)[]> moves = new() {
		{ 'e', [ ('e', 0,1,1) ,  ('s', 1,0,1001), ('n', -1,0, 1001) ] },
		{ 'w', [ ('w', 0,-1,1) , ('s', 1,0,1001), ('n', -1,0, 1001) ] },
		{ 'n', [ ('n', -1,0,1) , ('w', 0,-1,1001), ('e', 0,1, 1001) ] },
		{ 's', [ ('s', 1,0,1) ,  ('w', 0,-1,1001), ('e', 0,1,1001) ] }
	};


	public static void Search(this char[][] grid, int[][] costs, int row, int col, char direction, List<(int, int)>[][] prev) {
		foreach (var c in moves[direction]) {
			var rr = row + c.Row;
			var cc = col + c.Col;
			if ("E.".Contains(grid[rr][cc]) && costs[rr][cc] >= costs[row][col] + c.Cost) {
				prev[rr][cc].Add((row, col));
				costs[rr][cc] = costs[row][col] + c.Cost;
				grid.Search(costs, rr, cc, c.Next, prev);
			}
		}
	}

	public static T[][] Clone<T>(this char[][] grid, Func<T> makeValue) {
		var clone = new T[grid.Length][];
		for (var row = 0; row < clone.Length; row++) {
			clone[row] = Enumerable.Range(0, grid[row].Length).Select(_ => makeValue()).ToArray();
		}
		return clone;
	}

	public static (int Row, int Col) Find(this char[][] grid, char target) {
		for (var row = 0; row < grid.Length; row++) {
			for (var col = 0; col < grid[0].Length; col++) {
				if (grid[row][col] == target) return (row, col);
			}
		}
		return (-1, -1);
	}
}

