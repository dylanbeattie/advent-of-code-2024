var textarea = document.querySelector("textarea");
textarea.value = examples.example1;

const TINY = 0; // .00000001;

function Grid(input) {
	var GRID_SPACING = 32;
	function Node(row, col, dir) {
		this.toString = function () { return `(${row}:${col}:${dir})`; }
		this.row = row;
		this.col = col;
		this.dir = dir;
		this.cost = Infinity;
		this.prev = [];
		let NODE_SIZE = GRID_SPACING / 2 - 2;
		this.get_x = function () {
			return (this.col * GRID_SPACING) + (this.dir == 'e' ? -NODE_SIZE : this.dir == 'w' ? NODE_SIZE : 0);
		}

		this.get_y = function () {
			return (this.row * GRID_SPACING) + (this.dir == 'n' ? -NODE_SIZE : this.dir == 's' ? NODE_SIZE : 0);
		}

		this.neighbours = new Map();

		this.linkNeighbours = function (nodes) {
			var cluster = nodes[this.row][this.col];
			if (this.dir == 'e') {
				this.neighbours.set(cluster['w'], TINY);
				this.neighbours.set(cluster['n'], 1000);
				this.neighbours.set(cluster['s'], 1000);
				var next = nodes[this.row][this.col + 1]['w'];
				if (next) this.neighbours.set(next, 1);

			}
			if (this.dir == 'w') {
				this.neighbours.set(cluster['e'], TINY);
				this.neighbours.set(cluster['n'], 1000);
				this.neighbours.set(cluster['s'], 1000);
				var next = nodes[this.row][this.col - 1]['e'];
				if (next) this.neighbours.set(next, 1);
			}
			if (this.dir == 'n') {
				this.neighbours.set(cluster['s'], TINY);
				this.neighbours.set(cluster['e'], 1000);
				this.neighbours.set(cluster['w'], 1000);
				var next = nodes[this.row - 1][this.col]['s'];
				if (next) this.neighbours.set(next, 1);
			}
			if (this.dir == 's') {
				this.neighbours.set(cluster['n'], TINY);
				this.neighbours.set(cluster['e'], 1000);
				this.neighbours.set(cluster['w'], 1000);
				var next = nodes[this.row + 1][this.col]['n'];
				if (next) this.neighbours.set(next, 1);
			}
		}
	}

	var grid = input.split('\n').map(line => Array.from(line));
	this.nodes = new Array();
	this.allNodes = new Array();
	this.start = null;
	this.end = null;
	for (var row = 0; row < grid.length; row++) {
		this.nodes[row] ??= new Array();
		for (var col = 0; col < grid[row].length; col++) {
			this.nodes[row][col] ??= new Array();
			if (grid[row][col] == '#') continue;
			for (var dir of ['n', 's', 'w', 'e']) {
				let node = new Node(row, col, dir);
				this.nodes[row][col][dir] = node;
				this.allNodes.push(node);
				if (grid[row][col] == 'S' && dir == 'e') {
					this.start = node;
					node.cost = 0;
				}
				if (grid[row][col] == 'E') this.end = node;
			}
		}
	}

	for (var node of this.allNodes) node.linkNeighbours(this.nodes);

	this.Draw = function (ctx) {
		ctx.fillStyle = "black";
		ctx.fillRect(0, 0, 10000, 10000);
		ctx.strokeStyle = "#333";
		ctx.strokeWidth = 0.5;
		for (var node of this.allNodes) {
			var fillStyle = `hsl(${Math.floor(node.cost / 50)} 100% 30%)`;
			ctx.fillStyle = fillStyle;
			var neighbours = Array.from(node.neighbours.keys());
			for (var neighbour of neighbours) {
				ctx.beginPath();
				ctx.moveTo(node.get_x(), node.get_y());
				ctx.lineTo(neighbour.get_x(), neighbour.get_y());
				ctx.stroke();
			}
			ctx.beginPath();
			ctx.arc(node.get_x(), node.get_y(), 3, Math.PI * 2, false);
			ctx.fill();
		}
	}

	this.Backtrack = function(ctx, seen, node) {
		if (node == null) node = this.end;
		seen ??= [];
		if (seen.includes(node)) return;
		seen.push(node);
		ctx.strokeStyle = "#ffffff";
		ctx.lineWidth = 3;
		for(var p of node.prev) {
			ctx.beginPath();
			ctx.moveTo(p.get_x(), p.get_y());
			ctx.lineTo(node.get_x(), node.get_y());
			ctx.stroke();
			this.Backtrack(ctx, seen, p);
			if (p == this.start) return;
		}
	}
}

function Dijkstra(grid) {
	let unvisited = Array.from(grid.allNodes);
	let result = Infinity;
	while (unvisited.length > 0) {
		unvisited.sort((a, b) => a.cost - b.cost);
		let node = unvisited.shift();
		if (node.row == grid.end.row && node.col == grid.end.col && node.cost < result) {
			grid.end = node;
			result = node.cost;
		}
		var neighbours = Array.from(node.neighbours.keys());
		for (var neighbour of neighbours) {
			var oldCost = neighbour.cost;
			var newCost = node.cost + node.neighbours.get(neighbour);
			if (newCost <= oldCost) {
				if (newCost < oldCost) {
					neighbour.prev = [];
					neighbour.cost = newCost;
				}
				neighbour.prev.push(node);
			}
		}
	}
	return result;
}

function resizeCanvas() {
	var canvas = document.querySelector("canvas");
	canvas.width = canvas.getBoundingClientRect().width;
	canvas.height = canvas.getBoundingClientRect().height;
}

resizeCanvas();
// window.addEventListener("resize", resizeCanvas);

function Solve() {
	window.grid = new Grid(textarea.value);
	var canvas = document.querySelector("canvas");
	var ctx = canvas.getContext("2d")
	var cost = Dijkstra(window.grid);
	grid.Draw(ctx);
	console.log(cost);
	var seen = [];
	grid.Backtrack(ctx, seen);
	var things = seen.map(node => node.row +"," + node.col);
	// console.log(things);
	console.log([...new Set(things)].length);

}

Solve();
textarea.addEventListener("keyup", Solve);

