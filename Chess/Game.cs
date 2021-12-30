using System;
using System.Collections.Generic;

namespace Chess
{

	public class Game
	{
		public Grid<Piece> grid;
		List<Piece> captured = new List<Piece>();
		Player[] players = new Player[2];
		int active_player_index = 0;
		Player Active_Player
        {
			get
			{
				return players[active_player_index];
			}

			set
            {
				active_player_index = Array.IndexOf(players, value);
            }
        }

		public Game()
		{
			StandardBoard();

			Pawn pawn = new Pawn(Teams.WHITE);

			players[0] = new Player("Player 1", Teams.WHITE);
			players[1] = new Player("Player 2", Teams.BLACK);

			Active_Player = players[0];

			Console.WriteLine(String.Join(" | ", pawn.GetMoveSet(0, 0, grid)));
		}

		public void TakeTurn (int start_x, int start_y, int target_x, int target_y)
        {
			Move legal_move = ValidateMove(start_x, start_y, target_x, target_y);
			if (legal_move != null)
            {
				bool player_can_move = ValidateMoveWithGameState(legal_move);
				if (player_can_move)
				{
					FulfillMove(legal_move);
					SwitchPlayer();
				}
			}
        }

        public override string ToString()
        {
            return grid.ToString() + "\n\nActive Player: " + Active_Player.name;
        }

        private void StandardBoard ()
        {
			// Pawns
			grid = new Grid<Piece>(8);
			for (int i = 0; i < 8; i++)
            {
				grid.Set(i, 1, new Pawn(Teams.BLACK));
				grid.Set(i, 6, new Pawn(Teams.WHITE));
            }

			// Rooks
			grid.Set(0, 0, new Rook(Teams.BLACK));
			grid.Set(7, 0, new Rook(Teams.BLACK));

			grid.Set(0, 7, new Rook(Teams.WHITE));
			grid.Set(7, 7, new Rook(Teams.WHITE));

			// Knights
			grid.Set(1, 0, new Knight(Teams.BLACK));
			grid.Set(6, 0, new Knight(Teams.BLACK));

			grid.Set(1, 7, new Knight(Teams.WHITE));
			grid.Set(6, 7, new Knight(Teams.WHITE));

			// Bishops
			grid.Set(2, 0, new Bishop(Teams.BLACK));
			grid.Set(5, 0, new Bishop(Teams.BLACK));

			grid.Set(2, 7, new Bishop(Teams.WHITE));
			grid.Set(5, 7, new Bishop(Teams.WHITE));

			// Kings
			grid.Set(3, 0, new King(Teams.BLACK));

			grid.Set(3, 7, new King(Teams.WHITE));

			// Queens
			grid.Set(4, 0, new Queen(Teams.BLACK));

			grid.Set(4, 7, new Queen(Teams.WHITE));
		}

		private void SwitchPlayer ()
        {
			active_player_index = (active_player_index + players.Length + 1) % players.Length;
        }

		//<summary>Move pieces based on a move. This method performs no validation.</summary>
		private void FulfillMove (Move move)
        {
			if (move.capturing != null)
			{
				captured.Add(move.capturing);
				Tuple<int, int> coords = grid.CoordinatesOf(move.capturing);
				if (coords != null)
                {
					grid.Set(coords.Item1, coords.Item2, null);
				}
			}
			grid.Set(move.start_x, move.start_y, null);
			grid.Set(move.target_x, move.target_y, move.moving_piece);
			move.moving_piece.RegisterMove(move);
			if (move.extra_move != null)
            {
				FulfillMove(move.extra_move);
            }
		}

		private bool ValidateMoveWithGameState(Move move)
		{
			Piece start_piece = grid.Get(move.start_x, move.start_y);
			return start_piece.team == Active_Player.team;
		}

		private Move ValidateMove(int start_x, int start_y, int target_x, int target_y)
        {
            Piece start_piece = grid.Get(start_x, start_y);
			Piece target_piece = grid.Get(target_x, target_y);

			List<Move> possible_moves = start_piece.GetMoveSet(start_x, start_y, grid);

			return possible_moves.Find(move => move.target_x == target_x && move.target_y == target_y);
        }
    }

}