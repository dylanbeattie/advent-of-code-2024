using System.Diagnostics;
var sw = new Stopwatch();
sw.Start();
var grid = File.ReadAllLines("input.txt").Select(line => line.ToArray()).ToArray();
var visited = new HashSet<(int, int)>();
var blocked = new HashSet<(int, int)>();
var origin = grid.FindStartingPoint();
var direction = Directions.N;
visited.Add(origin);
var point = origin;
while (true) {
	var next = point.Move(direction);
	if (!grid.Contains(next)) break;
	if (grid[next.row][next.col] == '#') {
		direction = direction.Rotate();
	} else visited.Add(point = next);
}
Console.WriteLine($"Part 1: {visited.Count} ({sw.ElapsedMilliseconds} ms)");
sw.Restart();
foreach (var block in visited) {
	if (block == origin) continue;
	if (grid.WithBlockAt(block).HasLoop(origin)) blocked.Add(block);
}
Console.WriteLine($"Part 2: {blocked.Count} ({sw.ElapsedMilliseconds} ms)");

public static class Directions {
	public const char E = '→';
	public const char W = '←';
	public const char N = '↑';
	public const char S = '↓';

	public static char Rotate(this char direction) => direction switch {
		N => E,
		E => S,
		S => W,
		W => N,
		_ => '?'
	};

	public static char[][] WithBlockAt(this char[][] grid, (int row, int col) pair) {
		var clone = grid.Select(line => line.ToArray()).ToArray();
		clone[pair.row][pair.col] = '#';
		return clone;
	}

	public static bool HasLoop(this char[][] grid, (int row, int col) point) {
		var direction = N;
		while (true) {
			var next = point.Move(direction);
			if (!grid.Contains(next)) return false;
			if (grid[next.row][next.col] == direction) return true;
			if (grid[next.row][next.col] == '#') {
				direction = direction.Rotate();
			} else {
				point = next;
				grid[point.row][point.col] = direction;
			}
		}
	}

	public static (int row, int col) FindStartingPoint(this char[][] grid) {
		for (var row = 0; row < grid.Length; row++) {
			for (var col = 0; col < grid[0].Length; col++) {
				if (grid[row][col] == '^') return (row, col);
			}
		}
		return (-1, -1);
	}

	public static (int row, int col) Move(this (int row, int col) pair, char direction)
		=> direction switch {
			S => (pair.row + 1, pair.col),
			E => (pair.row, pair.col + 1),
			W => (pair.row, pair.col - 1),
			_ => (pair.row - 1, pair.col)
		};

	public static bool Contains(this char[][] grid, (int row, int col) pair)
		=> pair.row >= 0 && pair.row < grid.Length
		&& pair.col >= 0 && pair.col < grid[0].Length;
}
