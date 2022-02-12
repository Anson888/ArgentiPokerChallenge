using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ArgentiPokerChallenge
{
    public class PokerGame
    {
        public string FilePath { get; set; }
        public void PlayAllGames()
        {
            try
            {
                using (var reader = new StreamReader(FilePath))
                {
                    int playerOneWins = 0;
                    int playerTwoWins = 0;
                    int counter = 0;
                    while (reader.Peek() >= 0)
                    {
                        counter++;
                        var lineToParse = reader.ReadLine();
                        var lineSplit = lineToParse.Split(' ');
                        var player1Hand = new Hand();
                        var player2Hand = new Hand();

                        if (lineSplit.Length != 10)
                        {
                            throw new IOException("Invalid input: exactly 10 pairs are required per line");
                        }
                        else
                        {
                            //decided on hard coding to 10 as the first 5 are for player 1 so I'd like to have a incrementing variable i
                            //the 10 could definitely be a variable from config, from an argument or from some DB table but this is just for simplicity's sake
                            for (int i = 0; i < 10; i++)
                            {
                                var card = new Card(lineSplit[i][0], lineSplit[i][1]);
                                if (i < 5)
                                {
                                    player1Hand.Cards.Add(card);
                                }
                                else
                                {
                                    player2Hand.Cards.Add(card);
                                }
                            }
                            int winner = DetermineWinningHand(player1Hand, player2Hand);
                            if (winner == 1)
                            {
                                playerOneWins++;
                            }
                            else if (winner == 2) //would normally just write else, but the given rules don't account for the very unlikely circumstance where both players get a royal flush
                            {
                                playerTwoWins++;
                            }
                        }
                    }
                    Console.WriteLine("Player 1: {0} hands", playerOneWins);
                    Console.WriteLine("Player 2: {0} hands", playerTwoWins);
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Please ensure the argument is a valid path to a valid .txt file (must be a full path e.g. C:\\onehand.txt)");
            }
        }

        public int DetermineWinningHand(Hand player1, Hand player2)
        {
            int player1Rank = player1.GetHandRank();
            int player2Rank = player2.GetHandRank();
            if (player1Rank > player2Rank)
                return 1;
            else if (player2Rank > player1Rank)
                return 2;
            else if (player1Rank == player2Rank)
            {
                return ResolveTieBreakers(player1, player2, player1Rank, player2Rank);
            }
            //if it somehow gets to this point, and as mentioned it will in the rare double royal flush instance, or if both players have the exact same hands, nothing should increment
            return 0;
        }

        public int ResolveTieBreakers(Hand player1, Hand player2, int player1Rank, int player2Rank)
        {
            //the below conditions don't require iterating for the highest number as they only need to compare the four/three of a kind
            if (player1Rank == 8) //four of a kind
            {
                if (player1.Quads > player2.Quads)
                    return 1;
                else
                    return 2;
            }
            if (player1Rank == 7 || player1Rank == 4) //flush or three of a kind
            {
                if (player1.Triple > player2.Triple)
                    return 1;
                else
                    return 2;
            }
            if (player1Rank == 2 || player1Rank == 3)
            {
                for (int i = player1.Pairs.Count - 1; i >= 0; i--)
                {
                    if (player1.Pairs[i].CardOne.Value > player2.Pairs[i].CardOne.Value)
                        return 1;
                    else if (player2.Pairs[i].CardOne.Value > player1.Pairs[i].CardOne.Value)
                        return 2;
                }
            }
            for (int i = 4; i >= 0; i--)
            {
                if (player1.Cards[i].Value > player2.Cards[i].Value)
                    return 1;
                else if (player2.Cards[i].Value > player1.Cards[i].Value)
                    return 2;
            }
            return 0;
        }
    }
}
