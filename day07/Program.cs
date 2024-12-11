using System.Diagnostics;
using System.Numerics;

var equations = File.ReadAllLines("input.txt")
	.Select(line => new Equation(line)).ToList();

var sw = new Stopwatch();
sw.Start();
List<Func<long, long, long>> operators = [(a, b) => a * b, (a, b) => a + b];
var part1 = equations.Where(e => e.Solvable(operators)).Sum(e => e.Answer);
sw.Stop();
Console.WriteLine($"Part 1: {part1} ({sw.ElapsedMilliseconds} ms)");

var concat = (long a, long b) => Int64.Parse(a.ToString() + b);
var ops2 = operators.Append(concat).ToList();
sw.Restart();
var part2 = equations.Where(e => e.Solvable(ops2)).Sum(e => e.Answer);
sw.Stop();
Console.WriteLine($"Part 2: {part2} ({sw.ElapsedMilliseconds} ms)");

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


