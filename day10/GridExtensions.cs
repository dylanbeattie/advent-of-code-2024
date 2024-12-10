public static class GridExtensions {
	public static bool FindPeaks(this int[][] grid, int row, int col,
		HashSet<(int Row, int Col)> peaks) {
		var cell = grid.At(row, col);
		if (cell == 9) return peaks.Add((row, col));
		if (grid.At(row - 1, col) == cell + 1) grid.FindPeaks(row - 1, col, peaks);
		if (grid.At(row + 1, col) == cell + 1) grid.FindPeaks(row + 1, col, peaks);
		if (grid.At(row, col - 1) == cell + 1) grid.FindPeaks(row, col - 1, peaks);
		if (grid.At(row, col + 1) == cell + 1) grid.FindPeaks(row, col + 1, peaks);
		return false;
	}

	public static int FindTrails(this int[][] grid, int row, int col) {
		var cell = grid.At(row, col);
		if (cell == 9) return 1;
		var tally = 0;
		if (grid.At(row - 1, col) == cell + 1) tally += grid.FindTrails(row - 1, col);
		if (grid.At(row + 1, col) == cell + 1) tally += grid.FindTrails(row + 1, col);
		if (grid.At(row, col - 1) == cell + 1) tally += grid.FindTrails(row, col - 1);
		if (grid.At(row, col + 1) == cell + 1) tally += grid.FindTrails(row, col + 1);
		return tally;
	}

	public static int At(this int[][] grid, int row, int col) {
		if (row >= 0 && col >= 0 && row < grid.Length && col < grid[row].Length)
			return grid[row][col];
		return -1;
	}
}

