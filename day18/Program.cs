var filename = "input.txt";

var limit = filename == "example.txt" ? 7 : 71;
var howMany = filename == "example.txt" ? 12 : 1024;

var positions = File.ReadAllLines(filename)
	.Select(line => line.Split(",").Select(Int32.Parse).ToArray())
	.Select(a => (X: a[0], Y: a[1])).ToList();

for (var i = 2800; i < positions.Count; i++) Crank(i);

void Crank(int count) {
	var grid = new char[limit, limit];
	var costs = new int[limit, limit];

	for (var y = 0; y < grid.GetLength(1); y++) {
		for (var x = 0; x < grid.GetLength(0); x++) {
			grid[x, y] = '.';
			costs[x, y] = Int32.MaxValue;
		}
	}
	var ps = positions.Take(count).ToList();
	var coord = ps.Last();
	foreach (var position in ps) grid[position.X, position.Y] = '#';
	costs[0, 0] = 0;
	var unvisited = new List<(int X, int Y)>();
	for (var y = 0; y < grid.GetLength(1); y++)
		for (var x = 0; x < grid.GetLength(0); x++)
			unvisited.Add((x, y));

	while (unvisited.Any(node => costs[node.X, node.Y] < Int32.MaxValue)) {
		unvisited = unvisited.OrderBy(pos => costs[pos.X, pos.Y]).ToList();
		var node = unvisited[0];
		unvisited.RemoveAt(0);
		foreach (var nbor in new List<(int X, int Y)> {
		(node.X - 1, node.Y), (node.X+1, node.Y),
		(node.X, node.Y - 1), (node.X, node.Y + 1) }) {
			if (nbor.X < 0) continue;
			if (nbor.Y < 0) continue;
			if (nbor.X >= costs.GetLength(0)) continue;
			if (nbor.Y >= costs.GetLength(1)) continue;
			if (grid[nbor.X, nbor.Y] == '#') continue;
			var oldCost = costs[nbor.X, nbor.Y];
			var newCost = costs[node.X, node.Y] + 1;
			if (newCost < oldCost) costs[nbor.X, nbor.Y] = newCost;
		}
	}
	Console.WriteLine(coord.X + "," + coord.Y + " (" + costs[limit - 1, limit - 1] + ")");
}




