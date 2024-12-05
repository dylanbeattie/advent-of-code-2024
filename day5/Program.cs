var inputs = File.ReadAllText("input.txt").ReplaceLineEndings()
    .Split(Environment.NewLine + Environment.NewLine);
var pairs = inputs[0].Split(Environment.NewLine)
    .Select(line => line.Split("|").Select(Int32.Parse).ToArray());
var lists = inputs[1].Split(Environment.NewLine)
    .Select(line => line.Split(",").Select(Int32.Parse).ToList());
var prefixes = pairs
    .GroupBy(pair => pair[1])
    .ToDictionary(group => group.Key, group => group.Select(pair => pair[0]).ToList());

int Compare(int x, int y) 
    => (prefixes.TryGetValue(x, out var list) && list.Contains(y)) ? -1 : 1;

T Middle<T>(IList<T> list) => list[list.Count / 2];

var part1 = 0;
var part2 = 0;

foreach (var list in lists) {
    for (var i = 0; i < list.Count; i++) {
        if (!prefixes.TryGetValue(list[i], out var things)) continue;
        if (list.Skip(i + 1).Intersect(things).Any()) {
            list.Sort(Compare);
            part2 += Middle(list);
            goto NEXT_LIST;
        }
    }
    part1 += Middle(list);
NEXT_LIST: { }
}

Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");