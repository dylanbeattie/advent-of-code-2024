var pairs = File.ReadAllLines("input.txt")
	.Select(line => line.Split("   "));

var list1 = pairs.Select(pair => Int32.Parse(pair[0])).OrderBy(v => v);
var list2 = pairs.Select(pair => Int32.Parse(pair[1])).OrderBy(v => v);
var part1 = list1.Zip(list2, (lhs,  rhs) => Math.Abs(lhs - rhs)).Sum();
Console.WriteLine($"Solution to part 1: {part1}");

var counts = list2.GroupBy(v => v).ToDictionary(group => group.Key, group => group.Count());

var products = list1.Select(n => n * counts.GetValueOrDefault(n));

var part2 = products.Sum();
Console.WriteLine($"Solution to part 2: {part2}");

// foreach(var l in list1) Console.WriteLine(l);
// Console.ReadKey();
// foreach(var l in list2) Console.WriteLine(l);





