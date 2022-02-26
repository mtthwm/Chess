using System;

namespace Chess
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            Console.WriteLine(game);
            game.TakeTurn(0, 6, 0, 4);
            Console.WriteLine(game);
            game.TakeTurn(1, 1, 1, 3);
            Console.WriteLine(game);
            game.TakeTurn(0, 4, 1, 3);
            Console.WriteLine(game);
            game.TakeTurn(2, 1, 2, 3);
            Console.WriteLine(game);
            game.TakeTurn(1, 3, 2, 2);
            Console.WriteLine(game);
            Console.WriteLine(game.grid.Get(7, 1).CheckDanger(7, 1, game.grid));
        }
    }
}
