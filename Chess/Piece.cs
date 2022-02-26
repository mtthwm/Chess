using System;
using System.Collections.Generic;

namespace Chess
{
	public enum Teams
	{
		WHITE,
		BLACK
	}

	public struct RayHit
    {
		public Piece piece;
		public int x;
		public int y;
    }

	public abstract class Piece
	{
		public enum MPT
		{
			Y, // Yes, can move
			N, // No, cannot move
			X, // Can Capture
			O, // Origin (Where the piece is now)
			B, // Both can move and can capture
		}

		public Teams team { get; private set; }
		public bool original_position { get; private set; }
		public Grid<MPT>? move_pattern;

		protected List<Move> registered_moves = new List<Move>();

		public Piece(Teams team, bool original_position = true)
		{
			this.team = team;
			this.original_position = original_position;
		}

		private List<Move> GetMoveSetFromPattern (int x, int y, Grid<Piece> grid)
        {
			List<Move> moves = new List<Move>();
			if (move_pattern != null)
            {
				int hs = move_pattern.Size / 2;
				for (int ay = y - hs; ay < y + hs + 1; ay++)
                {
					for (int ax = x - hs; ax < x + hs + 1; ax++)
					{
						int px = ax - (x - hs);
						int py;
						if (team == Teams.WHITE)
						{
							py = ay - (y - hs);
						}
						else
						{
							py = (move_pattern.Size - 1) - (ay - (y - hs));
						}
						if (!grid.CheckBounds(ax, ay) || !move_pattern.CheckBounds(px, py))
                        {
							continue;
                        }
						MPT cell = move_pattern.Get(px, py);
						Piece cell_piece = grid.Get(ax, ay);				
						Move new_move = null;
						Move new_capture = null;
						
						switch (cell)
                        {
							case MPT.Y:
								if (cell_piece == null)
                                {
									new_move = new Move(x, y, ax, ay, this, null, null);
								}
								break;
							case MPT.N:
								break;
							case MPT.O:
								break;
							case MPT.X:
								if (cell_piece != null)
                                {
									if (cell_piece.team != team)
									{
										new_capture = new Move(x, y, ax, ay, this, cell_piece, null);
									}
								}
								break;
							case MPT.B:
								if (cell_piece != null)
								{
									if (cell_piece.team != team)
									{
										new_capture = new Move(x, y, ax, ay, this, cell_piece, null);
									}
								}
								else
								{
									new_move = new Move(x, y, ax, ay, this, null, null);
								}
								break;
						}
						if (new_capture != null)
                        {
							moves.Add(new_capture);
                        }
						if (new_move != null)
                        {
							moves.Add(new_move);
                        }
                    }
                }

				return moves;
            } 
			else
            {
				return new List<Move>();
            }
        }

		/* <summary> From position x and y, generate a moveset iteratively 
		 * using fx and fy to generate new values for x and y values</summary> */
		protected virtual List<Move> GetIteratedMoveSet (int x, int y, Grid<Piece> grid, Func<int, int> fx, Func<int, int> fy)
        {
			List<Move> moves = new List<Move>();
			int ax = fx(x);
			int ay = fy(y);
			
			while (grid.CheckBounds(ax, ay))
            {
				Piece a_piece = grid.Get(ax, ay);
				if (a_piece == null)
                {
					moves.Add(new Move(x, y, ax, ay, this, null, null));
                } else
                {
					if (a_piece.team != team)
                    {
						moves.Add(new Move(x, y, ax, ay, this, a_piece, null));
					}
					break;
                }
				ax = fx(ax);
				ay = fy(ay);
            }

			return moves;
        }

		protected virtual RayHit IterativeRay (int x, int y, Grid<Piece> grid, Func<int, int> fx, Func<int, int> fy)
        {
			RayHit hit = new RayHit();
			int ax = fx(x);
			int ay = fy(y);

			while (grid.CheckBounds(ax, ay))
			{
				Piece a_piece = grid.Get(ax, ay);
				if (a_piece != null)
				{
					hit.piece = a_piece;
					break;
				}
				ax = fx(ax);
				ay = fy(ay);
			}
			hit.x = ax;
			hit.y = ay;
			return hit;
		}

		public bool CheckDanger(int x, int y, Grid<Piece> grid)
        {
			for (int ay = 0; ay < grid.Size; ay++)
            {
				for (int ax = 0; ax < grid.Size; ax++)
                {
					Piece enemy = grid.Get(ax, ay);
					if (enemy != null && enemy.team != team)
                    {
						Move danger = enemy.GetMoveSet(ax, ay, grid).Find(move => move.target_x == x && move.target_y == y);
						if (danger != null)
                        {
							return true;
                        }
                    }
                }
            }
			return false;
		}

		public void RegisterMove (Move move)
        {
			registered_moves.Add(move);
        }

		public virtual List<Move> GetMoveSet(int x, int y, Grid<Piece> grid)
        {
			if (move_pattern != null && grid.CheckBounds(x, y))
            {
				return GetMoveSetFromPattern(x, y, grid);
            }
            else
            {
				return new List<Move>();
            }
        }
	}
}
