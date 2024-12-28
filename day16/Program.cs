var grid = File.ReadAllLines("example2.txt")
	.Select(line => line.ToCharArray()).ToArray();

for(var row = 0; row < grid.Length; row++) {



public record Node(int Row, int Col, char Direction) {
	public Dictionary<Node, int> Neighbours { get; set; } = new();



}

public class GridExtensions {
	public static IEnumerable<(int,int)> Scan<T>(this T[][] grid) {
		for(var row = 0; row < grid.Length; row++) {
			for(var col = 0; col < grid.Length; col++) {
				yield return (row,col);
			}
		}
	}
}
