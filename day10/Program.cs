using System.Diagnostics;

var grid = File.ReadAllLines("input.txt")
	.Select(line => line.Select(c => c - '0').ToArray())
	.ToArray();

List<(int Row, int Col)> trailheads = [];
for (var row = 0; row < grid.Length; row++) {
	for (var col = 0; col < grid[0].Length; col++) {
		if (grid[row][col] == 0) trailheads.Add((row, col));
	}
}
var sw = new Stopwatch();
sw.Start();
var part1 = 0;
foreach(var th in trailheads) {
	var hash = new HashSet<(int Row, int Col)>();
	grid.FindPeaks(th.Row, th.Col, hash);
	part1 += hash.Count;
}
Console.WriteLine($"Part 1: {part1} ({sw.ElapsedMilliseconds}ms)");
sw.Restart();
var part2 = 0;
foreach(var th in trailheads) part2 += grid.FindTrails(th.Row, th.Col);
Console.WriteLine($"Part 2: {part2} ({sw.ElapsedMilliseconds}ms)");


