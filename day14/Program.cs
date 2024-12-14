using System.Text.RegularExpressions;
var W = 101;
var H = 103;

var robots = File.ReadAllLines("input.txt")
	.Where(l => l.Contains("="))
	.Select(line => Regex.Split(line, "[^0-9-]+").Skip(1).Select(Int32.Parse).ToArray())
	.Select(numbers => new Robot(numbers[0], numbers[1], numbers[2], numbers[3])).ToList();


for (var seconds = 0; seconds < 100; seconds++) {
	foreach (var robot in robots) robot.Move(W, H);
}

for (var y = 0; y < H; y++) {
	for (var x = 0; x < W; x++) {
		var count = robots.Where(r => r.X == x && r.Y == y).Count();
		Console.Write(count > 0 ? count : ".");
	}
	Console.WriteLine();
}

var nw = robots.Where(r => r.X < W/2 && r.Y < H/2).Count();
var ne = robots.Where(r => r.X < W/2 && r.Y > H/2).Count();
var sw = robots.Where(r => r.X > W/2 && r.Y < H/2).Count();
var se = robots.Where(r => r.X > W/2 && r.Y > H/2).Count();

var part1 = nw * ne * sw *se;
Console.WriteLine($"Part 1: {part1}");




public class Robot(int x, int y, int dx, int dy) {
	public int X { get; private set; } = x;
	public int Y { get; private set; } = y;

	public void Move(int xMax, int yMax) {
		X = (X + dx) % xMax;
		Y = (Y + dy) % yMax;
		if (X < 0) X += xMax;
		if (Y < 0) Y += yMax;
	}
	public override string ToString() => $"Pos: ({X},{Y}) Vel: ({dx},{dy})";
}







