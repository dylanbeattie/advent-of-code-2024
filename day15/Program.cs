using System.Text;

var chunks = File.ReadAllText("input.txt").ReplaceLineEndings()
.Split(Environment.NewLine + Environment.NewLine);

var grid = chunks[0].Split(Environment.NewLine)
	.Select(row => row.ToCharArray()).ToArray();

var grid2 = chunks[0].Replace("O", "[]")
	.Replace("#", "##")
	.Replace(".", "..")
	.Replace("@", "@.")
	.Split(Environment.NewLine)
	.Select(row => row.ToCharArray()).ToArray();

var moves = chunks[1].Replace(Environment.NewLine, "").ToCharArray();

Console.WriteLine(String.Join(" ", moves.ToList().TakeLast(8).ToArray()));
//var robot = grid.Find('@');

//foreach (var move in moves) {
//	var (row, col, moved) = grid.Move(robot.Row, robot.Col, move);
//	robot.Row = row;
//	robot.Col = col;
//}

//var part1 = grid.Select((line, row) => line.Select((cell, col) => cell == 'O' ? 100 * row + col : 0))
//	.SelectMany(boxes => boxes)
//	.Sum();
//Console.WriteLine(part1);

var robot2 = grid2.Find('@');
Console.Clear();
grid2.Draw();

//while (true) {
//	var key = Console.ReadKey();
//	var move = key.Key switch {
//		ConsoleKey.RightArrow => '>',
//		ConsoleKey.LeftArrow => '<',
//		ConsoleKey.UpArrow => '^',
//		_ => 'v'
//	};
//	var (row, col, _) = grid2.Move2(robot2.Row, robot2.Col, move);
//	robot2.Row = row;
//	robot2.Col = col;
//	Console.Clear();
//	grid2.Draw();

//}

var i = 0;
foreach (var move in moves) {
	//Console.Clear();
	//grid2.Draw();
	//Console.WriteLine(i + " " + move);
	//Console.ReadKey();
	var (row, col, _) = grid2.Move2(robot2.Row, robot2.Col, move);
	robot2.Row = row;
	robot2.Col = col;
	i++;
	//if (!grid2.Sane()) {
	//}
}

var part2 = grid2.Select((line, row) => line.Select((cell, col) => cell == '[' ? 100 * row + col : 0))
	.SelectMany(boxes => boxes)
	.Sum();
Console.WriteLine(part2);

static class Extensions {

	public static bool CanMove(this char[][] grid, int row, int col, char direction) {
		switch (grid[row][col]) {
			case '#':
				return false;
			case '.':
				return true;
		}

		var (rr, cc) = direction switch {
			'>' => (row, col + 1),
			'<' => (row, col - 1),
			'^' => (row - 1, col),
			_ => (row + 1, col)
		};

		if (grid[rr][cc] == '#') return false;

		if ("<>".Contains(direction))  return grid.CanMove(rr, cc, direction);

		var cell = grid[row][col];

		return cell switch {
			'[' => grid.CanMove(rr, cc, direction) && grid.CanMove(rr, cc + 1, direction),
			']' => grid.CanMove(rr, cc, direction) && grid.CanMove(rr, cc - 1, direction),
			_ => grid.CanMove(rr, cc, direction)
		};
	}


	public static (int row, int col, bool moved) Move2(this char[][] grid,
		int row, int col, char direction) {
		var cell = grid[row][col];
		if (cell == '.') return (row, col, false);
		if (!grid.CanMove(row, col, direction)) return (row, col, false);
		var (rr, cc) = direction switch {
			'>' => (row, col + 1),
			'<' => (row, col - 1),
			'^' => (row - 1, col),
			_ => (row + 1, col)
		};
		if ("<>".Contains(direction)) return grid.Move(row, col, direction);

		if (grid[rr][cc] == '.' || grid.Move2(rr, cc, direction).moved) {
			switch (cell) {
				case '[':
					grid.Move2(rr, cc + 1, direction);
					grid[rr][cc + 1] = grid[row][col + 1];
					grid[row][col + 1] = '.';
					break;
				case ']':
					grid.Move2(rr, cc - 1, direction);
					grid[rr][cc - 1] = grid[row][col - 1];
					grid[row][col - 1] = '.';
					break;
			}
			grid[rr][cc] = grid[row][col];
			grid[row][col] = '.';
			return (rr, cc, true);
		}

		return (row, col, false);
	}

	public static (int row, int col, bool moved) Move(this char[][] grid,
		int row, int col, char direction) {
		var (rr, cc) = direction switch {
			'>' => (row, col + 1),
			'<' => (row, col - 1),
			'^' => (row - 1, col),
			_ => (row + 1, col)
		};
		if (grid[rr][cc] == '#') return (row, col, false);
		if (grid[rr][cc] == '.' || grid.Move(rr, cc, direction).moved) {
			grid[rr][cc] = grid[row][col];
			grid[row][col] = '.';
			return (rr, cc, true);
		}

		return (row, col, false);
	}

	public static void Draw(this char[][] grid) {
		var sb = new StringBuilder();
		for (var r = 0; r < grid.Length; r++) {
			for (var c = 0; c < grid[r].Length; c++) {
				Console.BackgroundColor = grid[r][c] switch {
					'[' => grid[r][c + 1] switch {
						']' => ConsoleColor.Black,
						_ => ConsoleColor.Red
					},
					']' => grid[r][c - 1] switch {
						'[' => ConsoleColor.Black,
						_ => ConsoleColor.Red
					},
					_ => ConsoleColor.Black
				};
				Console.ForegroundColor = grid[r][c] switch {
					'@' => ConsoleColor.Magenta,
					'#' => ConsoleColor.Yellow,
					_ => ConsoleColor.White
				};
				Console.Write(grid[r][c]);
			}

			Console.WriteLine();
			}

			Console.WriteLine(sb.ToString());
		}

	public static (int Row, int Col) Find(this char[][] grid, char robot) {
		for (var r = 0; r < grid.Length; r++) {
			for (var c = 0; c < grid[r].Length; c++) {
				if (grid[r][c] == robot) return (r, c);
			}
		}

		return (-1, -1);
	}

	public static bool Sane(this char[][] grid) {
		for (var r = 0; r < grid.Length; r++) {
			for (var c = 0; c < grid[r].Length; c++) {
				if (grid[r][c] == '[' && grid[r][c + 1] != ']') {
					Console.WriteLine($"MAD AT {r},{c}");
					return false;
				}
				if (grid[r][c] == ']' && grid[r][c - 1] != '[') {
					Console.WriteLine($"MAD AT {r},{c}");
					return false;
				}
			}
		}

		return true;
	}
}






