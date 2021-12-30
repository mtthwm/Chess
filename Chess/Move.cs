using System;

namespace Chess
{
	public class Move
	{
		public int start_x { get; private set; }
		public int start_y { get; private set; }
		public int target_x { get; private set; }
		public int target_y { get; private set; }
		public Piece moving_piece;
		public Piece? capturing = null;
		public Move? extra_move = null;
		public Type? promotion = null;

		public Move(int start_x, int start_y, int target_x, int target_y, Piece moving_piece, Piece capturing = null, Move extra_move = null, Type promotion = null)
		{
			this.start_x = start_x;
			this.start_y = start_y;
			this.target_x = target_x;
			this.target_y = target_y;

			this.moving_piece = moving_piece;
			this.capturing = capturing;
			this.extra_move = extra_move;
			this.promotion = promotion;
		}

        public override string ToString()
        {
			return moving_piece + "(" + start_x + ", " + start_y + ") " + (capturing == null ? "to" : "captures") + " (" + target_x + ", " + target_y + ")";
        }
    }
}
