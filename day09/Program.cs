using System.Diagnostics;

var input = File.ReadAllText("input.txt").Trim();
var sw = new Stopwatch();
var blocks = input.ToCharArray()
	.Select(c => c - '0')
	.Chunk(2)
	.SelectMany((pair, index) =>
		Enumerable.Range(0, pair[0]).Select(_ => index)
		.Concat(Enumerable.Range(0, pair.Length > 1 ? pair[1] : 0).Select(_ => -1))
	).ToArray();

var blocks2 = blocks.ToArray();

sw.Start();
while (frag(blocks));
var part1 = blocks.Checksum();
Console.WriteLine($"Part 1: {part1} ({sw.ElapsedMilliseconds}ms)");

sw.Restart();
var part2 = defrag(blocks2).Checksum();
Console.WriteLine($"Part 2: {part2} ({sw.ElapsedMilliseconds}ms)");

bool frag(int[] blocks) {
	var target = blocks.IndexOfFirst(block => block < 0);
	var source = blocks.IndexOfLast(block => block >= 0);
	if (source <= target) return false;
	blocks[target] = blocks[source];
	blocks[source] = -1;
	return true;
}

int[] defrag(int[] blocks) {
	var result = blocks.ToArray();
	var EOF = blocks.Length;
	while(EOF > 0) {
		while (EOF >= 1 && blocks[EOF-1] < 0)	EOF--;
		var BOF = EOF;
		while (BOF >= 1 && blocks[BOF-1] == blocks[EOF-1]) BOF--;
		MoveFileIfYouCan(result, BOF, EOF);
		EOF = BOF;
	}
	return result;
}

void MoveFileIfYouCan(int[] blocks, int BOF, int EOF) {
	var fileSize = EOF - BOF;
	var BOG = 0;
	while (true) {
		if (BOG >= BOF || BOG >= blocks.Length) return;
		while (BOG < BOF && blocks[BOG] >= 0) BOG++;
		var EOG = BOG;
		while (EOG < BOF && blocks[EOG] < 0) EOG++;
		var gapSize = EOG - BOG;
		if (fileSize <= gapSize) {
			while (BOF < EOF) {
				blocks[BOG++] = blocks[BOF];
				blocks[BOF++] = -1;
			}
			return;
		} else {
			BOG = EOG + 1;
		}
	}
}

void Print(int[] blocks, int BOF, int EOF, int BOG, int EOG) {
	for (var i = 0; i < blocks.Length; i++) Console.Write(blocks[i] < 0 ? "." : blocks[i]);
	Console.WriteLine();
	Console.Write(String.Empty.PadRight(Math.Max(BOF, 0)));
	Console.WriteLine(String.Empty.PadRight(Math.Max(EOF - BOF, 0), '^'));
}

public static class Extensions {

	public static long Checksum(this int[] blocks)
		=> blocks.Select((value,index) => value < 0 ? 0L : (long)value * index).Sum();

	public static int IndexOfFirst<T>(this IList<T> list, Func<T, bool> match) {
		var i = -1;
		while (i++ < list.Count) if (match(list[i])) return i;
		return Int32.MaxValue;
	}

	public static int IndexOfLast<T>(this IList<T> list, Func<T, bool> match) {
		var i = list.Count;
		while (i-- > 0) if (match(list[i])) return i;
		return Int32.MinValue;
	}
}

