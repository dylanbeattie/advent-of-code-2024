using System.Text.RegularExpressions;

var input = File.ReadAllText("input.txt");
var matches1 = Regex.Matches(input, @"mul\([0-9]+,[0-9]+\)");
var part1 = matches1
    .Select(mul => mul.ToString()[4..].Trim(')').Split(',')
    .Select(n => Int32.Parse(n)).ToArray())
    .Select(a => a[0] * a[1])
    .Sum();
Console.WriteLine(part1);

var doos = String.Join("", input.Split("do()")
    .Select(doo => doo.Split("don't()")[0]));

var matches2 = Regex.Matches(doos, @"mul\([0-9]+,[0-9]+\)");
var part2 = matches2
    .Select(mul => mul.ToString()[4..].Trim(')').Split(',')
    .Select(n => Int32.Parse(n)).ToArray())
    .Select(a => a[0] * a[1])
    .Sum();
Console.WriteLine(part2);
  