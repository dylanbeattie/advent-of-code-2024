using System.Text.RegularExpressions;

var machines = File.ReadAllText("input.txt")
	.Split(Environment.NewLine + Environment.NewLine)
		.Select(ClawMachine.Parse).ToList();

var part1 = machines.Select(m => m.Solve()).Where(cost => cost > 0).Sum();
Console.WriteLine($"Part 1: {part1}");

const long TEN_TRILLION = 10_000_000_000_000;

var part2 = machines.Select(m => m.Solve(TEN_TRILLION)).Where(cost => cost > 0).Sum();
Console.WriteLine($"Part 2: {part2}");

public record Button(long X, long Y);

public record ClawMachine(Button A, Button B, long X, long Y) {

	public long Solve(long offset = 0) {
		decimal ax = A.X;
		decimal ay = A.Y;
		decimal bx = B.X;
		decimal by = B.Y;
		var n = ((offset + X) * ay - (offset +Y) * ax) / (bx * ay - ax * by);
		var m = (offset + Y - n * by) / ay;
		if ((n == (long)n) && (m == (long)m)) return (3 * (long)m) + (long)n;
		return 0;
	}

	internal static ClawMachine Parse(string input) {
		var numbers = input.ReplaceLineEndings().Replace(Environment.NewLine, "").ParseDigits();
		if (numbers.Length != 6) {
			Console.Write(input);
			throw new Exception("WRONG NUMBERS");
		}
		return new ClawMachine(
			new Button(numbers[0], numbers[1]),
			new Button(numbers[2], numbers[3]),
			numbers[4], numbers[5]);
	}
}

public static class StringExtensions {
	public static long[] ParseDigits(this string input)
		=> Regex.Matches(input, "[0-9]+", RegexOptions.Singleline)
			.Select(match => Int64.Parse(match.Value)).ToArray();
}


