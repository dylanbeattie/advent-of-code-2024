using System.Runtime.InteropServices;

var grid = File.ReadAllLines("input.txt")
	.Select(line => line.ToCharArray())
	.ToArray();

var perimeterMap = grid.MapPerimeters();
var regions = grid.MapRegions();

var areas = regions.SelectMany(line => line).GroupBy(cell => cell)
	.ToDictionary(group => group.Key, group => group.Count());

Dictionary<int, int> regionsToPerimeters = new();

for (var row = 0; row < regions.Length; row++) {
	for (var col = 0; col < regions[row].Length; col++) {
		var region = regions[row][col];
		CollectionsMarshal.GetValueRefOrAddDefault(regionsToPerimeters, region, out _)
			+= perimeterMap[row][col];
	}
}

//foreach (var thing in areas) {
//	Console.WriteLine(thing.Key + " = " + thing.Value);
//}

var part1 = 0;
foreach (var thing in regionsToPerimeters) {
	//Console.WriteLine(thing.Key + " = " + thing.Value);
	part1 += thing.Value * areas[thing.Key];
}

Console.WriteLine(part1);






//for (var row = 0; row < regions.Length; row++) {
//	for (var col = 0; col < regions[row].Length; col++) {
//		Console.Write($"{regions[row][col]} ");
//	}
//	Console.WriteLine();
//}

public static class Extensions {
	public static int[][] MapRegions(this char[][] grid) {
		int[][] result = new int[grid.Length][];

		for (int i = 0; i < result.Length; i++) {
			result[i] = new int[grid[0].Length];
		}

		int regionId = 1;

		for (int r = 0; r < grid.Length; r++) {
			for (int c = 0; c < grid[r].Length; c++) {
				if (result[r][c] != 0) continue;
				result[r][c] = regionId;
				CheckNeighbours(grid[r][c], regionId, grid, result, r, c);
				regionId++;
			}
		}

		return result;
	}

	private static void CheckNeighbours(char c, int regionId, char[][] grid, int[][] result, int row, int col) {
		if (row > 0) {
			if (grid[row - 1][col] == c && result[row - 1][col] == 0) {
				result[row - 1][col] = regionId;
				CheckNeighbours(c, regionId, grid, result, row - 1, col);
			}
		}

		if (row < grid.Length - 1) {
			if (grid[row + 1][col] == c && result[row + 1][col] == 0) {
				result[row + 1][col] = regionId;
				CheckNeighbours(c, regionId, grid, result, row + 1, col);
			}
		}

		if (col > 0) {
			if (grid[row][col - 1] == c && result[row][col - 1] == 0) {
				result[row][col - 1] = regionId;
				CheckNeighbours(c, regionId, grid, result, row, col - 1);
			}
		}

		if (col < grid[row].Length - 1) {
			if (grid[row][col + 1] == c && result[row][col + 1] == 0) {
				result[row][col + 1] = regionId;
				CheckNeighbours(c, regionId, grid, result, row, col + 1);
			}
		}
	}

	public static int[][] MapPerimeters(this char[][] grid) {
		var result = grid.Select(row => row.Select(_ => 0).ToArray()).ToArray();
		for (var row = 0; row < grid.Length; row++) {
			for (var col = 0; col < grid[row].Length; col++) {
				result[row][col] = grid.Perimeter(row, col);
			}
		}
		return result;
	}

	public static int Perimeter(this char[][] grid, int row, int col) {
		var p = 0;
		var me = grid[row][col];
		if (row == 0 || grid[row - 1][col] != me) p++;
		if (col == 0 || grid[row][col - 1] != me) p++;
		if (row == grid.Length - 1 || grid[row + 1][col] != me) p++;
		if (col == grid[row].Length - 1 || grid[row][col + 1] != me) p++;
		return p;
	}
}





