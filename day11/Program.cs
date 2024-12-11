using System.Diagnostics;

var stones = File.ReadAllText("input.txt")
	.Split(" ").Select(Int64.Parse);

var cache = new Dictionary<(long, int), long>();
var sw = new Stopwatch();
sw.Start();
var part1 = stones.Select(s => Blink(s, 25, cache)).Sum();
Console.WriteLine($"Part 1: {part1} ({sw.ElapsedMilliseconds} ms)");
sw.Restart();
var part2 = stones.Select(s => Blink(s, 75, cache)).Sum();
Console.WriteLine($"Part 2: {part2} ({sw.ElapsedMilliseconds} ms)");

long Blink(long stone, int times, Dictionary<(long,int), long> cache) {
	long result = 1;
	if (cache.ContainsKey((stone,times))) return cache[(stone,times)];
	if (times == 0) {
		cache.Add((stone,times), 1);
		return 1;
	}
	if (stone == 0) {
		result = Blink(1, times - 1, cache);
		cache.Add((stone, times), result);
		return result;
	}
	if (stone.ToString().Length % 2 == 0) {
		var bits = stone.ToString().Bisect().Select(Int64.Parse).ToArray();
		result = Blink(bits[0], times - 1, cache) + Blink(bits[1], times - 1, cache);
		cache.Add((stone,times), result);
		return result;
	}
	result = Blink(2024L * stone, times - 1, cache);
	cache.Add((stone, times), result);
	return result;
}


 public static class Extensions {
// 	public static IEnumerable<long> Blink(this IEnumerable<long> stones) {
// 		foreach (var stone in stones) {
// 			if (stone == 0) {
// 				yield return 1;
// 			} else if (stone.ToString().Length % 2 == 0) {
// 				foreach (var digit in stone.ToString().Bisect()) {
// 					yield return Int64.Parse(digit);
// 				}
// 			} else {
// 				yield return stone * 2024;
// 			}
// 		}
// 	}
	public static IEnumerable<string> Bisect(this string s) {
		var len = s.Length / 2;
		yield return s[0..len];
		yield return s[len..];
	}

// 	public static void Write(this IEnumerable<long> stones) {
// 		foreach (var stone in stones) {
// 			Console.Write(stone);
// 			Console.Write(" ");
// 		}
// 		Console.WriteLine();
// 	}
}
