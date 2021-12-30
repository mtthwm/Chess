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
								} else
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
			List<RayHit> hits = new List<RayHit>();

			// Up
			hits.Add(IterativeRay(x, y, grid, x => x, y => y - 1));
			// Down
			hits.Add(IterativeRay(x, y, grid, x => x, y => y + 1));
			// Left
			hits.Add(IterativeRay(x, y, grid, x => x - 1, y => y));
			// Right
			hits.Add(IterativeRay(x, y, grid, x => x + 1, y => y));
			// Up Right
			hits.Add(IterativeRay(x, y, grid, x => x + 1, y => y - 1));
			// Down Left
			hits.Add(IterativeRay(x, y, grid, x => x - 1, y => y + 1));
			// Up Left
			hits.Add(IterativeRay(x, y, grid, x => x - 1, y => y - 1));
			// Down Right
			hits.Add(IterativeRay(x, y, grid, x => x + 1, y => y + 1));

			foreach (RayHit hit in hits)
            {
				if (hit.piece != null && hit.piece.team != team)
                {
					Move danger_move = hit.piece.GetMoveSet(hit.x, hit.y, grid).Find(move => move.target_x == x && move.target_y == y);
					if (danger_move != null)
                    {
						return false;
                    }
                }
            }

			return true;
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
