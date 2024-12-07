var equations = File.ReadAllLines("example.txt")
	.Select(line => new Equation(line));

foreach (var eq in equations) Console.WriteLine(eq);

public class Equation {
	public Equation(string line) {
		var pair = line.Split(": ");
		this.Answer = Int64.Parse(pair[0]);
		this.Inputs = pair[1].Split(' ').Select(Int64.Parse).ToList();
	}
	public long Answer { get; set; }
	public List<long> Inputs { get; set; }
	public override string ToString() => $"{Answer}: {String.Join(',', Inputs)}";
}
