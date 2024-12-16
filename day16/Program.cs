var grid = File.ReadAllLines("input.txt")
	.Select(line => line.ToCharArray()).ToArray();

var costs = grid.Clone(Int32.MaxValue);
var reindeer = grid.Find('S');
var target = grid.Find('E');

costs[reindeer.Row][reindeer.Col] = 0;

grid.Search(costs, reindeer.Row, reindeer.Col, 'e');

// for (var row = 0; row < costs.Length; row++) {
// 	for (var col = 0; col < costs[row].Length; col++) {
// 		var cost = costs[row][col];
// 		Console.ForegroundColor = grid[row][col] switch {
// 			'E' => ConsoleColor.Green,
// 			'S' => ConsoleColor.Red,
// 			_ => ConsoleColor.White
// 		};
// 		if (cost == Int32.MaxValue) {
// 			Console.Write("###### ");
// 		} else {
// 			Console.Write($"{cost:000000} ");
// 		}
// 	}
// 	Console.WriteLine();
// }

Console.WriteLine(costs[target.Row][target.Col]);

// Console.SetCursorPosition(0, 0);
// foreach (var line in grid) {
// 	foreach (var cell in line) {
// 		Console.ForegroundColor = cell switch {
// 			'S' => ConsoleColor.Green,
// 			_ => ConsoleColor.White
// 		};
// 		Console.Write(cell);
// 	}
// 	Console.WriteLine();
// }
public static class Extensions {

	public static void Search(this char[][] grid, int[][] costs, int row, int col, char direction) {
		if (direction == 'e') {
			if ("E.".Contains(grid[row][col + 1]) && costs[row][col + 1] > costs[row][col] + 1) {
				costs[row][col + 1] = costs[row][col] + 1;
				grid.Search(costs, row, col + 1, 'e');
			}
			if ("E.".Contains(grid[row + 1][col]) && costs[row + 1][col] > costs[row][col] + 1001) {
				costs[row + 1][col] = costs[row][col] + 1001;
				grid.Search(costs, row + 1, col, 's');
			}
			if ("E.".Contains(grid[row - 1][col]) && costs[row - 1][col] > costs[row][col] + 1001) {
				costs[row - 1][col] = costs[row][col] + 1001;
				grid.Search(costs, row - 1, col, 'n');
			}
		}
		if (direction == 'w') {
			if ("E.".Contains(grid[row][col - 1]) && costs[row][col - 1] > costs[row][col] + 1) {
				costs[row][col - 1] = costs[row][col] + 1;
				grid.Search(costs, row, col - 1, 'w');
			}
			if ("E.".Contains(grid[row + 1][col]) && costs[row + 1][col] > costs[row][col] + 1001) {
				costs[row + 1][col] = costs[row][col] + 1001;
				grid.Search(costs, row + 1, col, 's');
			}
			if ("E.".Contains(grid[row-1][col]) && costs[row - 1][col] > costs[row][col] + 1001) {
				costs[row - 1][col] = costs[row][col] + 1001;
				grid.Search(costs, row - 1, col, 'n');
			}
		}
		if (direction == 'n') {
			if ("E.".Contains(grid[row - 1][col]) && costs[row - 1][col] > costs[row][col] + 1) {
				costs[row - 1][col] = costs[row][col] + 1;
				grid.Search(costs, row - 1, col, 'n');
			}
			if ("E.".Contains(grid[row][col - 1]) && costs[row][col - 1] > costs[row][col] + 1001) {
				costs[row][col - 1] = costs[row][col] + 1001;
				grid.Search(costs, row, col - 1, 'w');
			}
			if ("E.".Contains(grid[row][col + 1]) && costs[row][col + 1] > costs[row][col] + 1001) {
				costs[row][col + 1] = costs[row][col] + 1001;
				grid.Search(costs, row, col + 1, 'e');
			}
		}
		if (direction == 's') {
			if ("E.".Contains(grid[row + 1][col]) && costs[row + 1][col] > costs[row][col] + 1) {
				costs[row + 1][col] = costs[row][col] + 1;
				grid.Search(costs, row + 1, col, 's');
			}
			if ("E.".Contains(grid[row][col - 1]) && costs[row][col - 1] > costs[row][col] + 1001) {
				costs[row][col - 1] = costs[row][col] + 1001;
				grid.Search(costs, row, col - 1, 'w');
			}
			if ("E.".Contains(grid[row][col + 1]) && costs[row][col + 1] > costs[row][col] + 1001) {
				costs[row][col + 1] = costs[row][col] + 1001;
				grid.Search(costs, row, col + 1, 'e');
			}
		}
	}

	public static T[][] Clone<T>(this char[][] grid, T value) {
		var clone = new T[grid.Length][];
		for (var row = 0; row < clone.Length; row++) {
			clone[row] = Enumerable.Range(0, grid[row].Length).Select(_ => value).ToArray();
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

