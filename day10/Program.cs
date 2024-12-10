var grid = File.ReadAllLines("example.txt")
	.Select(line => line.Select(c => (int)c - '0').ToArray())
	.ToArray();

foreach(var line in grid) {
	foreach(var cell in line) Console.Write($"{cell} ");
	Console.WriteLine();
}
