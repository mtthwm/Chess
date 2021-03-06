using System;
using System.Collections.Generic;

namespace Chess
{
	public class Bishop : Piece
	{
		public Bishop(Teams team, bool original_position = true) : base(team, original_position)
		{
		}

		public override List<Move> GetMoveSet(int x, int y, Grid<Piece> grid)
		{
			List<Move> moves = base.GetMoveSet(x, y, grid);

			// Up Right (universal)
			moves.AddRange(GetIteratedMoveSet(x, y, grid, ax => ax + 1, ay => ay - 1));

			// Down Right (universal)
			moves.AddRange(GetIteratedMoveSet(x, y, grid, ax => ax + 1, ay => ay + 1));

			// Up Left (universal)
			moves.AddRange(GetIteratedMoveSet(x, y, grid, ax => ax - 1, ay => ay - 1));

			// Down Right (universal)
			moves.AddRange(GetIteratedMoveSet(x, y, grid, ax => ax + 1, ay => ay + 1));

			return moves;
		}

		public override string ToString()
		{
			return "Bishop";
		}
	}
}
