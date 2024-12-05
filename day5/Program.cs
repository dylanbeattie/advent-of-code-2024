var lines = File.ReadAllText("input.txt").ReplaceLineEndings()
    .Split(Environment.NewLine + Environment.NewLine);
var pairs = lines[0].Split(Environment.NewLine);
var lists = lines[1].Split(Environment.NewLine)
    .Select(line => line.Split(",").Select(Int32.Parse).ToList());

var prefixes = new Dictionary<int, HashSet<int>>();
//var suffixes = new Dictionary<int, HashSet<int>>();

foreach (var pair in pairs) {
    var tokens = pair.Split("|").Select(Int32.Parse).ToArray();
//    if (!suffixes.ContainsKey(tokens[0])) suffixes.Add(tokens[0], []);
    if (!prefixes.ContainsKey(tokens[1])) prefixes.Add(tokens[1], []);
    prefixes[tokens[1]].Add(tokens[0]);
//    suffixes[tokens[0]].Add(tokens[1]);
}

List<List<int>> part2Lists = [];
var part1 = 0;
foreach (var list in lists) {
    for (var i = 0; i < list.Count; i++) {
        if (prefixes.TryGetValue(list[i], out var things)) {
            if (things.Intersect(list.Skip(i + 1)).Any()) {
                part2Lists.Add(list);
                goto INVALID;
            }
        }
    }
    part1 += list.Middle();
    continue;
INVALID: {}
}

var part2 = 0;

foreach(var list in part2Lists) {
    list.Sort(Compare);
    part2 += list.Middle();
}
   
Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");

int Compare(int x, int y)
    => (prefixes.TryGetValue(x, out var list) && list.Contains(y)) ? -1 : 1;
 
public static class IListExtensions {
    public static T Middle<T>(this IList<T> list) => list[list.Count / 2];
}