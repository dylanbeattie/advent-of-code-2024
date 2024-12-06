var grid = File.ReadAllLines("input.txt")
	.Select(line => line.ToArray())
	.ToArray();

var visited = new HashSet<(int, int)>();
var (row, col) = FindStartingPoint(grid);
visited.Add((row,col));
var delta = (row: -1, col: 0);
while(true) {
	row += delta.row;
	col += delta.col;
	if (row < 0 || row >= grid.Length || col < 0 || col >= grid[0].Length) break;
	if (grid[row][col] == '#') {
		row -= delta.row;
		col -= delta.col;
		delta = delta switch {
			(-1,+0) => (0, +1),
			(+0,+1) => (+1,+0),
			(+1,+0) => (+0,-1),
			(+0,-1) => (-1, 0),
			_ => throw new("Don't know how you ended up going that way!")
		};
	} else {
		visited.Add((row,col));
	}
	// for(var r = 0; r < grid.Length; r++) {
	// 	for(var c =0; c < grid[0].Length; c++) {
	// 		if (visited.Contains((r,c))) {
	// 			Console.ForegroundColor = ConsoleColor.Magenta;
	// 			Console.Write("x");
	// 			Console.ForegroundColor = ConsoleColor.White;
	// 		} else {
	// 			Console.Write(grid[r][c]);
	// 		}
	// 	}
	// 	Console.WriteLine();
	// }
	// Console.WriteLine(visited.Count);
	// Console.ReadKey();
	// Console.Clear();
	// Thread.Sleep(TimeSpan.FromMilliseconds(10));
}
Console.WriteLine($"Part 1: {visited.Count}");


(int, int) FindStartingPoint(char[][] grid) {
	for (var row = 0; row < grid.Length; row++) {
		for (var col = 0; col < grid[0].Length; col++) {
			if (grid[row][col] == '^') return (row, col);
		}
	}
	return(-1,-1);
}





// var list = new List<(int,int)>();
// var hash = new HashSet<(int,int)>();

// foreach(var pair in new[] { (1,1), (1,1), (1,2), (1,2) }) {
// 	list.Add(pair);
// 	hash.Add(pair);
// }

// Console.WriteLine(list.Count);
// Console.WriteLine(hash.Count);
