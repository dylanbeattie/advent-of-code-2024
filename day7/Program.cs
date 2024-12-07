using System.Diagnostics;
using System.Numerics;

var equations = File.ReadAllLines("input.txt")
	.Select(line => new Equation(line));

var sw = new Stopwatch();
sw.Start();
var part1 = equations.Where(e => e.Solvable).Sum(e => e.Answer);
sw.Stop();
Console.WriteLine($"Part 1: {part1} ({sw.ElapsedMilliseconds} ms)");

public class Equation {
	public Equation(string line) {
		var pair = line.Split(": ");
		this.Answer = Int64.Parse(pair[0]);
		this.Inputs = pair[1].Split(' ').Select(Int64.Parse).ToList();
	}
	public long Answer { get; set; }
	public List<long> Inputs { get; set; }
	public override string ToString() => $"{Answer}: {String.Join(',', Inputs)}";

	public bool Solvable => Inputs.Solvable(Answer);
}

public static class ListExtensions {
	public static IEnumerable<List<T>> Reduce<T>(this List<T> list) where T : INumber<T> {
		foreach (var result in new[] { list[0] + list[1], list[0] * list[1] }) {
			yield return ((List<T>) [result]).Concat(list.Skip(2)).ToList();
		}
	}

	public static bool Solvable<T>(this List<T> list, T result) where T : INumber<T> {
		if (list.Count > 1) return list.Reduce().Any(l => l.Solvable(result));
		return (list[0] == result);
	}
}


