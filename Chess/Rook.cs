using System;
using System.Collections.Generic;

namespace Chess
{
	public class Rook : Piece
	{
		public Rook(Teams team, bool original_position = true) : base(team, original_position)
		{
		}

		public override List<Move> GetMoveSet(int x, int y, Grid<Piece> grid)
		{
			List<Move> moves = base.GetMoveSet(x, y, grid);

			// Up (universal)
			moves.AddRange(GetIteratedMoveSet(x, y, grid, ax => ax, ay => ay - 1));

			// Down (universal)
			moves.AddRange(GetIteratedMoveSet(x, y, grid, ax => ax, ay => ay + 1));

			// Left (universal)
			moves.AddRange(GetIteratedMoveSet(x, y, grid, ax => ax - 1, ay => ay));

			// Right (universal)
			moves.AddRange(GetIteratedMoveSet(x, y, grid, ax => ax  + 1, ay => ay));

			return moves;
		}

		public override string ToString()
		{
			return "Rook";
		}
	}
}
