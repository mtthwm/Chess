using System;
using System.Collections.Generic;

namespace Chess
{
	public class Pawn : Piece
	{
		public bool Can_Capture_In_Passing
        {
			get
            {
				if (registered_moves.Count == 1)
                {
					Move only_move = registered_moves[0];
					return Math.Abs(only_move.target_y - only_move.start_y) > 1;
                }
                else
                {
					return false;
                }
            }
        }

		public Pawn(Teams team, bool original_position = true) : base(team, original_position)
		{
			move_pattern = new Grid<MPT>(new MPT[,] {
				{ MPT.X, MPT.Y, MPT.X },
				{ MPT.N, MPT.O, MPT.N },
				{ MPT.N, MPT.N, MPT.N } }
			);
		}

		public override List<Move> GetMoveSet(int x, int y, Grid<Piece> grid)
		{
			// Get moves from pattern defined in the constructor.
			List<Move> moves = base.GetMoveSet(x, y, grid);

			int modifier = team == Teams.WHITE ? 1 : -1;

			// Double Move
			if (original_position)
			{
				if (grid.CheckBounds(x, y - modifier * 2) && grid.Get(x, y - modifier) == null)
				{
					Piece target_piece = grid.Get(x, y - modifier * 2);
					if (target_piece == null)
					{
						moves.Add(new Move(x, y, x, y - modifier * 2, this, null, null));
					}
				}
			}

			// En Passant
			if (grid.CheckBounds(x - 1, y))
            {
				Pawn left_adjacent = grid.Get(x - 1, y) as Pawn;
				if (left_adjacent != null)
				{
					if (left_adjacent.Can_Capture_In_Passing && left_adjacent.team != team)
					{
						moves.Add(new Move(x, y, x - 1, y - modifier, this, left_adjacent, null));
					}
				}
			}
			
			if (grid.CheckBounds(x + 1, y))
            {
				Pawn right_adjacent = grid.Get(x + 1, y) as Pawn;
				if (right_adjacent != null)
				{
					if (right_adjacent.Can_Capture_In_Passing && right_adjacent.team != team)
					{
						moves.Add(new Move(x, y, x + 1, y - modifier, this, right_adjacent, null));
					}
				}
			}

            // Promotion
			if (y >= 7 || y <= 0)
            {
				Type[] possible_promotions = new Type[] { typeof(Knight), typeof(Bishop), typeof(Rook), typeof(Queen) };
				foreach (Type promotion in possible_promotions)
				{
					moves.Add(new Move(x, y, x, y, this, null, null, promotion));
				}
			}

            return moves;
		}

		public override string ToString()
		{
			return "Pawn";
		}
	}
}
