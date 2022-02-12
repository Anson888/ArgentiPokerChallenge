using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArgentiPokerChallenge
{
    public class Card
    {
        private static char[] SuitCards = { 'T','J', 'Q', 'K', 'A' };
        public int Value { get; set; }
        public char Suit { get; set; }

        public Card(char valueChar, char suit)
        {
            int value = 0;
            if (SuitCards.Contains(valueChar))
            {
                switch (valueChar)
                {
                    case 'T':
                        Value = 10;
                        break;
                    case 'J':
                        Value = 11;
                        break;
                    case 'Q':
                        Value = 12;
                        break;
                    case 'K':
                        Value = 13;
                        break;
                    case 'A':
                        Value = 14;
                        break;
                }
            }
            else
            {
                int.TryParse(valueChar.ToString(), out value);
                Value = value;
            }
            Suit = suit;
        }
    }
}
