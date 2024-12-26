using System.Data;

var grid = File.ReadAllLines("scratch1.txt")
	.Select(line => line.ToCharArray()).ToArray();

var costs = grid.Clone(() => Int32.MaxValue);
var reindeer = grid.Find('S');
var target = grid.Find('E');

costs[reindeer.Row][reindeer.Col] = 0;

var prev = grid.Clone(() => new List<(int, int)>());

grid.Search(costs, reindeer.Row, reindeer.Col, 'e', prev);

for (var row = 0; row < prev.Length; row++) {
	for (var col = 0; col < prev[row].Length; col++) {
		Console.WriteLine($"{row},{col}:      " + String.Join(" ", prev[row][col].ToArray()));
	}
}


var paths = prev.FindAllPaths(reindeer, target, []).ToList();

foreach(var path in paths) {
	Console.WriteLine(String.Join(">", path));
}

var shortests = paths.SelectMany(path =>path).Distinct();

for (var row = 0; row < grid.Length; row++) {
	for (var col = 0; col < grid[row].Length; col++) {
		Console.Write(shortests.Contains((row,col)) ? "x" : grid[row][col]);
	}
	Console.WriteLine();
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
			Console.Write("#### ");
		} else {
			Console.Write($"{cost:0000} ");
		}
	}
	Console.WriteLine();
}

Console.WriteLine(costs[target.Row][target.Col]);

public static class Extensions {
	public static IEnumerable<IEnumerable<(int,int)>> FindAllPaths(
		this List<(int Row,int Col)>[][] prev, 
		(int Row,int Col)source, 
		(int Row,int Col) target,
		List<((int Row,int Col) Source, (int Row, int Col) Target)> visited
	) {
		if (source == target) yield return [ target ];
		if (visited.Contains((source,target))) yield break;
		visited.Add((source,target));
		foreach(var step in prev[target.Row][target.Col]) {
			foreach(var path in prev.FindAllPaths(source, step, visited)) {
				yield return path.Concat( [ target ]);
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
			var oldCost = costs[rr][cc];
			var newCost = costs[row][col] + c.Cost;
			if ("E.".Contains(grid[rr][cc]) && newCost <= oldCost) {
				if (newCost < oldCost) prev[rr][cc].Clear();
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

