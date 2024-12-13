using System.Text.RegularExpressions;

var machines = File.ReadAllText("scratch.txt")
	.Split(Environment.NewLine + Environment.NewLine)
		.Select(ClawMachine.Parse).ToList();

foreach (var machine in machines) {
	Console.WriteLine(machine);
}


public record Button(int X, int Y) {
}
public record ClawMachine(Button A, Button B, int X, int Y) {
	internal static ClawMachine Parse(string input) {
		var numbers = input.ParseDigits();
		if (numbers.Length != 6) throw new Exception("WRONG NUMBERS");
		return new ClawMachine(
			new Button(numbers[0], numbers[1]),
			new Button(numbers[2], numbers[3]),
			numbers[4], numbers[5]);
	}
}

public static class StringExtensions {
	public static int[] ParseDigits(this string input)
		=> Regex.Matches(input, "[0-9]+", RegexOptions.Singleline)
			.Select(match => Int32.Parse(match.Value)).ToArray();
}


