
using System.Text;

var grid = File.ReadAllLines("input.txt")
	.Select(line => line.ToCharArray()).ToArray();

var costs = grid.Clone(() => Int32.MaxValue);
var reindeer = grid.Find('S');
var target = grid.Find('E');

costs[reindeer.Row][reindeer.Col] = 0;

var prev = grid.Clone(() => new HashSet<(int, int)>());
Console.Clear();
Console.OutputEncoding = Encoding.UTF8;
grid.Search(costs, reindeer.Row, reindeer.Col, 'e', prev);

var paths = prev.FindAllPaths(reindeer, target).SelectMany(path => path).ToList();
for (var row = 0; row < grid.Length; row++) {
	for (var col = 0; col < grid[row].Length; col++) {
		if (paths.Contains((row, col))) {
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write("O");
			Console.ForegroundColor = ConsoleColor.White;
		} else {
			Console.Write(grid[row][col]);
		}
	}
	Console.WriteLine();
}

Console.WriteLine(costs[target.Row][target.Col]);
Console.WriteLine(paths.Distinct().Count());


public static class Extensions {
	public static void Print(this char[][] grid, int[][] costs, HashSet<(int, int)>[][] prev, int highlightRow = 0, int highlightCol = 0) {
		for (var row = 0; row < costs.Length; row++) {
			for (var col = 0; col < costs[row].Length; col++) {

				var cost = costs[row][col];
				if (row == highlightRow && col == highlightCol) {
					Console.BackgroundColor = ConsoleColor.Yellow;
					Console.ForegroundColor = ConsoleColor.Black;
				} else {
					Console.BackgroundColor = ConsoleColor.Black;
					Console.ForegroundColor = grid[row][col] switch {
						'E' => ConsoleColor.Green,
						'S' => ConsoleColor.Red,
						_ => ConsoleColor.White
					};
				}
				Console.Write(prev[row][col].Contains((row, col - 1)) ? "→" : " ");
				if (grid[row][col] == '#') {
					Console.Write("█████");
				} else if (cost == Int32.MaxValue) {
					Console.Write("     ");
				} else {
					Console.Write($"{cost:00000}");
				}
				Console.Write(prev[row][col].Contains((row, col + 1)) ? "←" : " ");
			}

			Console.WriteLine();
			for (var col = 0; col < costs[row].Length; col++) {
				if (grid[row][col] == '#') {
					Console.Write(" █████ ");
				} else if (costs[row][col] == Int32.MaxValue) {
					Console.Write("       ");
				} else if (row + 1 < costs.Length) {
					Console.Write((prev[row + 1][col].Contains((row, col)), prev[row][col].Contains((row + 1, col))) switch {
						(true, true) => "   ↕   ",
						(true, false) => "   ↓   ",
						(false, true) => "   ↑   ",
						_ => "       "
					});
				}
			}
			Console.WriteLine();
		}
	}

	public static IEnumerable<IEnumerable<(int, int)>> FindAllPaths(
		this HashSet<(int Row, int Col)>[][] prev,
		(int Row, int Col) source,
		(int Row, int Col) target
	) {
		if (source == target) yield return [target];
		var steps = prev[target.Row][target.Col];
		foreach (var step in steps) {
			var paths = prev.FindAllPaths(source, step);
			foreach (var path in paths) {
				yield return path.Concat([target]);
			}
		}
	}

	private static Dictionary<char, (char Next, int Row, int Col, int Cost)[]> moves = new() {
		{ 'e', [ ('e', 0,1,1) ,  ('s', 1,0,1001), ('n', -1,0, 1001) ] },
		{ 'w', [ ('w', 0,-1,1) , ('s', 1,0,1001), ('n', -1,0, 1001) ] },
		{ 'n', [ ('n', -1,0,1) , ('w', 0,-1,1001), ('e', 0,1, 1001) ] },
		{ 's', [ ('s', 1,0,1) ,  ('w', 0,-1,1001), ('e', 0,1,1001) ] }
	};

	public static void Search(this char[][] grid, int[][] costs, int row, int col, char direction, HashSet<(int, int)>[][] prev) {
		//Console.Clear();
		//grid.Print(costs, prev, row, col);
		//Console.ReadKey();
		//Console.WriteLine($"{row} {col} {direction}");
		if (grid[row][col] == 'E') return;
		foreach (var c in moves[direction]) {
			var rr = row + c.Row;
			var cc = col + c.Col;
			var oldCost = costs[rr][cc];
			var newCost = costs[row][col] + c.Cost;
			if (!"E.".Contains(grid[rr][cc])) continue;
			var isTurn = ((newCost - oldCost) % 1000 == 0);
			if (newCost > oldCost && ! isTurn) {
				continue;
			} else {
				if (prev[rr][cc].Any() && newCost < oldCost && !isTurn) prev[rr][cc].Clear();
				prev[rr][cc].Add((row, col));
				costs[rr][cc] = newCost;
				if (! isTurn) grid.Search(costs, rr, cc, c.Next, prev);
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

