using System.Runtime.InteropServices;

var grid = File.ReadAllLines("scratch.txt")
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

var part1 = regionsToPerimeters.Sum(pair => pair.Value * areas[pair.Key]);
Console.WriteLine(part1);

Dictionary<int, List<(int,int)>> corners = new();
for (var row = -1; row < regions.Length; row++) {
	for (var col = -1; col < regions[0].Length; col++) {
		var region = regions.At(row,col);
		corners.TryAdd(region, new());
		var found = regions.FindCorners(row,col).Distinct();
		Console.Write(region + ": ");
		foreach(var finding in found) {
			Console.WriteLine("   " + finding.row + ", " + finding.col);
		}
		corners[region].AddRange();
	}
}
Console.WriteLine(String.Empty.PadRight(72, '-'));
for (var row = 0; row < regions.Length; row++) {
	for (var col = 0; col < regions[row].Length; col++) {
		Console.Write(regions[row][col]);
	}
	Console.WriteLine();
}

corners.Remove(0);
foreach(var region in corners.Keys) {
	Console.Write($"{region}: ");
	Console.Write(String.Join(", ", corners[region].Distinct().Select(pair => $"({pair.Item1},{pair.Item2})")));
	Console.WriteLine();
}


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

	public static IEnumerable<(int row, int col)> FindCorners(this int[][] grid, int row, int col) {
		if (grid.HasCornerAt(row-1,col-1)) yield return(row-1,col-1);
		if (grid.HasCornerAt(row-1,col)) yield return(row-1,col-1);
		if (grid.HasCornerAt(row-1,col+1)) yield return(row-1,col-1);
		if (grid.HasCornerAt(row,col-1)) yield return(row-1,col-1);
		if (grid.HasCornerAt(row, col)) yield return (row, col);
		if (grid.HasCornerAt(row, col + 1)) yield return (row, col + 1);
		if (grid.HasCornerAt(row + 1, col-1)) yield return (row + 1, col);
		if (grid.HasCornerAt(row + 1, col)) yield return (row + 1, col);
		if (grid.HasCornerAt(row + 1, col + 1)) yield return (row + 1, col + 1);
	}

	private static bool HasCornerAt(this int[][] grid, int row, int col) {
		//  Console.WriteLine($"{row}, {col} ===========================");
		var nw = grid.At(row, col);
		var ne = grid.At(row, col + 1);
		var sw = grid.At(row + 1, col);
		var se = grid.At(row + 1, col + 1);
		//  Console.WriteLine($"""
		//      {nw} {ne}
		//      {sw} {se}

		//  """);

		var bits = (nw == ne ? 1 : 0, nw == sw ? 1 : 0, nw == se ? 1 : 0);
		// Console.WriteLine($"{bits.Item1}{bits.Item2}{bits.Item3}");
		var result =  bits switch {
			(0, 1, 0) => false,
			(1, 0, 0) => false,
			(1, 1, 1) => false,
			_ => true
		};
		// if (result) Console.WriteLine($"corner at {row}, {col}!");
		return result;
	}

	public static int At(this int[][] grid, int row, int col) {
		if (row < 0) return 0;
		if (col < 0) return 0;
		if (row >= grid.Length) return 0;
		if (col >= grid[row].Length) return 0;
		return grid[row][col];
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





