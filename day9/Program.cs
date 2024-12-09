using System.Linq;

var input = "2333133121414131402";

var id = 0;
var map = String.Join("", input.ToCharArray()
	.Select(c => c - '0')
	.Chunk(2)
	.Select(pair =>
		String.Empty.PadRight(pair[0], (id++).ToString()[0])
		+
		(pair.Length > 1 ? String.Empty.PadRight(pair[1], '.') : "")
	));
Console.WriteLine(map);
