using System.Diagnostics;

var stones = File.ReadAllText("input.txt").Split(" ").Select(Int64.Parse);

var cache = new Dictionary<(long, int), long>();
var sw = new Stopwatch();
sw.Start();
var part1 = stones.Select(s => Blink(s, 25, cache)).Sum();
Console.WriteLine($"Part 1: {part1} ({sw.ElapsedMilliseconds} ms)");
sw.Restart();
var part2 = stones.Select(s => Blink(s, 75, cache)).Sum();
Console.WriteLine($"Part 2: {part2} ({sw.ElapsedMilliseconds} ms)");

long Blink(long stone, int times, Dictionary<(long, int), long> cache) {
	long result = 1;
	if (cache.ContainsKey((stone, times))) return cache[(stone, times)];
	if (times == 0) return cache.Cache(stone, times, 1);
	if (stone == 0) return cache.Cache(stone, times, Blink(1, times - 1, cache));
	if (stone.TryBisect(out var bits)) {
		result = Blink(bits.Item1, times - 1, cache) + Blink(bits.Item2, times - 1, cache);
		return cache.Cache(stone, times, result);
	}
	return cache.Cache(stone, times, Blink(2024L * stone, times - 1, cache));
}

public static class Extensions {

	public static bool TryBisect(this long l, out (long, long) bits) {
		bits = (0, 0);
		var s = l.ToString();
		var len = s.Length;
		if (len % 2 != 0) return false;
		bits = (Int64.Parse(s[0..(len/2)]), Int64.Parse(s[(len/2)..]));
		return true;
	}

	public static long Cache(this Dictionary<(long, int), long> cache, long stone, int times, long value) {
		cache.TryAdd((stone, times), value);
		return value;
	}

	public static IEnumerable<string> Bisect(this string s) {
		var len = s.Length / 2;
		yield return s[0..len];
		yield return s[len..];
	}
}
