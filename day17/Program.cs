var lines = File.ReadAllLines("input.txt");
var A = Int64.Parse(lines[0].Split(": ")[1]);
var B = Int64.Parse(lines[1].Split(": ")[1]);
var C = Int64.Parse(lines[2].Split(": ")[1]);
var program = lines[4].Split(": ")[1].Split(",").Select(Int64.Parse).ToArray();

var computer = new Computer(A, B, C, program);
var part1 = computer.Run();
Console.WriteLine("Part 1: " + String.Join(",", part1));
A = 0;
var s1 = String.Join(",", program);
void Search(long a, List<long> quines) {
	for (var i = 0; i < 8; i++) {
		computer = new Computer(a + i, B, C, program);
		var output = computer.Run();
		if (program.EndsWith(output)) {
			var s2 = String.Join(",", output);
			Console.WriteLine("===============================");
			Console.WriteLine("A: " + (a + i));
			Console.WriteLine("Program: " + s1);
			Console.WriteLine("Output:  " + s2);
			if (program.Count() == output.Count()) {
				quines.Add(a + i);
				return;
			}
			if (a + i > 0) Search((a + i) << 3, quines);
		}
	}
}

var quines = new List<long>();
Search(0, quines);
Console.WriteLine(String.Join(Environment.NewLine, quines.OrderBy(q => q)));


public class Computer(long a, long b, long c, long[] program) {

	public void Dump() {
		Console.WriteLine("====================");
		Console.WriteLine("Program: " + String.Join(",", Program));
		Console.WriteLine($"A: {A}");
		Console.WriteLine($"B: {B}");
		Console.WriteLine($"C: {C}");
	}

	public long A { get; private set; } = a;
	public long B { get; private set; } = b;
	public long C { get; private set; } = c;
	public long[] Program { get; private set; } = program;
	private long ip = 0;

	private string Code(long op) => op switch {
		ADV => "0=adv",
		BXL => "1=bxl",
		BST => "2=bst",
		JNZ => "3=jnz",
		BXC => "4=bxc",
		OUT => "5=out",
		BDV => "6=bdv",
		CDV => "7=cdv",
		_ => $"[{op}]"
	};

	private const long ADV = 0;
	private const long BXL = 1;
	private const long BST = 2;
	private const long JNZ = 3;
	private const long BXC = 4;
	private const long OUT = 5;
	private const long BDV = 6;
	private const long CDV = 7;

	private long Decombo(long value) => value switch {
		4 => A,
		5 => B,
		6 => C,
		7 => throw new Exception("Combo operand 7 is reserved and will not appear in valid programs."),
		_ => value
	};
	public List<long> Run() {
		List<long> output = [];
		while (ip < Program.Length) {
			var instruction = Program[ip++];
			var operand = Program[ip++];
			// Console.WriteLine($"{Code(instruction)} {operand} (decombo: {Decombo(operand)})     A: {A:00000000} B: {B:00000000} C: {C:00000000}");
			switch (instruction) {
				case ADV:
					A = (long) (A / Math.Pow(2, Decombo(operand)));
					break;
				case BXL:
					B ^= operand;
					break;
				case BST:
					B = Decombo(operand) % 8;
					break;
				case JNZ:
					if (A != 0) ip = operand;
					break;
				case BXC:
					B ^= C;
					break;
				case OUT:
					output.Add(Decombo(operand) % 8);
					break;
				case BDV:
					B = (long) (A / Math.Pow(2, Decombo(operand)));
					break;
				case CDV:
					C = (long) (A / Math.Pow(2, Decombo(operand)));
					break;
			}
		}
		return output;
	}
}


public static class Extensions {
	public static bool EndsWith<T>(this IList<T> haystack, IList<T> needle) {
		var n = needle.Count;
		var h = haystack.Count;
		while (n-- > 0 && h-- > 0) {
			if (!needle[n].Equals(haystack[h])) return false;
		}
		return true;
	}
}
