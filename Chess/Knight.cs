using System;
using System.Collections.Generic;

namespace Chess
{
	public class Knight : Piece
	{
		public Knight(Teams team, bool original_position = true) : base(team, original_position)
		{
			move_pattern = new Grid<MPT>(new MPT[,] {
				{ MPT.N, MPT.B, MPT.N, MPT.B, MPT.N, },
				{ MPT.B, MPT.N, MPT.N, MPT.N, MPT.B, },
				{ MPT.N, MPT.N, MPT.O, MPT.N, MPT.N, },
				{ MPT.B, MPT.N, MPT.N, MPT.N, MPT.B, },
				{ MPT.N, MPT.B, MPT.N, MPT.B, MPT.N, },
			});
		}

		public override string ToString()
		{
			return "Knight";
		}
	}
}
