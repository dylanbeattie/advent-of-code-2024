var reports = File.ReadAllLines("input.txt")
 	.Select(line => line.Split(' ')
 		.Select(n => Int32.Parse(n)).ToList());

var part1 = reports.Count(r => r.IsSafe());
Console.WriteLine($"Part 1: {part1}");

var part2 = reports.Count(r => r.IsSafeAfterDampening());
Console.WriteLine($"Part 2: {part2}");
