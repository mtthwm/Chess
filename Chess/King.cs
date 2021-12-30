using System;
using System.Collections.Generic;

namespace Chess
{
	public class King : Piece
	{
		public King(Teams team, bool original_position = true) : base(team, original_position)
		{
			move_pattern = new Grid<MPT>(new MPT[,] {
				{ MPT.B, MPT.B, MPT.B },
				{ MPT.B, MPT.O, MPT.B },
				{ MPT.B, MPT.B, MPT.B } }
			);
		}

		public override List<Move> GetMoveSet(int x, int y, Grid<Piece> grid)
		{
			// Get moves from pattern defined in the constructor.
			List<Move> moves = base.GetMoveSet(x, y, grid);

			// TODO: Castling

			return moves;
		}

		public override string ToString()
		{
			return "King";
		}
	}
}
