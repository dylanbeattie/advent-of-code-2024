using System.Text.RegularExpressions;

var robots = File.ReadAllLines("example.txt")
	.Where(l => l.Contains("="))
	.Select(line => Regex.Split(line, "[^0-9-]+").Skip(1).Select(Int32.Parse).ToArray())
	.Select(numbers => new Robot(numbers[0],numbers[1],numbers[2],numbers[3]));

foreach(var robot in robots) {
	Console.WriteLine(String.Join(",", robot));
}



record Robot(int x, int y, int dx, int dy);






