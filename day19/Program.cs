var lines = File.ReadAllLines("input.txt");
var towels = lines[0].Split(", ").ToArray();
var designs = lines.Skip(2).ToArray();

Dictionary<string,long> memo = new() { { "", 1 } };
long Combinations(string design, string[] towels) {
	if (memo.TryGetValue(design, out var result)) return result;
	result = towels.Where(design.StartsWith).Sum(t => Combinations(design[t.Length..], towels));
	memo.Add(design, result);
	return result;
}

var part1 = designs.Count(d => Combinations(d, towels) > 0);
Console.WriteLine($"Part 1: {part1}");

var part2 = designs.Sum(d => Combinations(d, towels));
Console.WriteLine($"Part 2: {part2}");
