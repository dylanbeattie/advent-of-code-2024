var lines = File.ReadAllLines("input.txt")
    .Select(line => line.ToArray())
    .ToArray();

var count = 0;
for (var r = 0; r < lines.Length; r++) {
    for (var c = 0; c < lines[r].Length; c++) {
        count += lines.Search(r, c);
    }
}
Console.WriteLine($"Part 1: {count}");

count = 0;
for (var r = 1; r < lines.Length-1; r++) {
    for (var c = 1; c < lines[r].Length-1; c++) {
        if (lines.Mas(r, c)) count++;
    }
}
Console.WriteLine($"Part 2: {count}");
public static class GridExtensions {
    static int[] deltas = [-1, 0, 1];
    public static int Search(this char[][] grid, int r, int c) {
        int hits = 0;
        foreach (var dr in deltas) {
            foreach (var dc in deltas) {
                if (grid.Xmas(r, c, dr, dc)) hits++;
            }
        }
        return hits;
    }
    static bool Xmas(this char[][] grid, int r, int c, int dr, int dc) {
        if (dr == 0 && dc == 0) return false;
        if (grid[r][c] != 'X') return false;
        if (r + 3 * dr < 0) return false;
        if (r + 3 * dr >= grid.Length) return false;
        if (c + 3 * dc < 0) return false;
        if (c + 3 * dc >= grid[0].Length) return false;
        return grid[r + dr][c + dc] == 'M'
            && grid[r + 2 * dr][c + 2 * dc] == 'A'
            && grid[r + 3 * dr][c + 3 * dc] == 'S';
    }

    public static bool Mas(this char[][] grid, int r, int c) {
        if (grid[r][c] != 'A') return false;
        return (
            grid[r-1][c-1] == 'M' && grid[r+1][c+1] == 'S'
            || 
            grid[r-1][c-1] == 'S' && grid[r+1][c+1] == 'M'
        ) && (
            grid[r-1][c+1] == 'M' && grid[r+1][c-1] == 'S'
            || 
            grid[r-1][c+1] == 'S' && grid[r+1][c-1] == 'M'
        );
    }
}


