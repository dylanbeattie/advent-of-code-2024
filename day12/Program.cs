var grid = new string[] { "A" } // File.ReadAllLines("example.txt")
	.Select(line => line.ToCharArray())
	.ToArray();

var map = grid.BuildMap();
for (var row = 0; row < map.Length; row++)
{
	for (var col = 0; col < map[row].Length; col++)
	{
		Console.Write($"{map[row][col]} ");
	}
	Console.WriteLine();
}



public static class Extensions
{
	public static int[][] BuildMap(this char[][] grid)
	{
		var result = grid.Select(row => row.Select(_ => 0).ToArray()).ToArray();
		for (var row = 0; row < grid.Length; row++)
		{
			for (var col = 0; col < grid[row].Length; col++)
			{
				result[row][col] = grid.Perimeter(row, col);
			}
		}
		return result;
	}

	public static int Perimeter(this char[][] grid, int row, int col)
	{
		var p = 0;
		var me = grid[row][col];
		if (row == 0 || grid[row - 1][col] != me) p++;
		if (col == 0 || grid[row][col - 1] != me) p++;
		if (row == grid.Length - 1 || grid[row + 1][col] != me) p++;
		if (col == grid[row].Length - 1 || grid[row][col + 1] != me) p++;
		return p;
	}
}





