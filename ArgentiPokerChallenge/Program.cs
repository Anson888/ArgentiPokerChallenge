using System;
using System.Collections.Generic;
using System.IO;

namespace ArgentiPokerChallenge
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                PokerGame game = new PokerGame { FilePath = args[0] };
                game.PlayAllGames();
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Please specify a valid .txt file to read");
            }
        }
    }
}
