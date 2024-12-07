using System.Diagnostics;
using System.Numerics;

var equations = File.ReadAllLines("input.txt")
	.Select(line => new Equation(line));

var sw = new Stopwatch();
sw.Start();
List<Func<long, long, long>> part1Operators = [(a, b) => a * b, (a, b) => a + b];
var part1 = equations.Where(e => e.Solvable(part1Operators)).Sum(e => e.Answer);
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

	public bool Solvable(List<Func<long, long, long>> reductions)
		=> Inputs.Solvable<long>(Answer, reductions);
}

public static class ListExtensions {
	public static IEnumerable<List<T>> Reduce<T>(this List<T> list, List<Func<T, T, T>> reductions)
		where T : INumber<T> {
		foreach (var reduce in reductions) {
			yield return ((T[]) [reduce(list[0], list[1])]).Concat(list.Skip(2)).ToList();
		}
	}

	public static bool Solvable<T>(this List<T> list, T result,
		List<Func<T,T,T>> reductions) where T : INumber<T> {
		if (list.Count > 1) return list.Reduce(reductions).Any(l => l.Solvable(result, reductions));
		return (list[0] == result);
	}
}


