using System;
using System.Collections.Generic;
using System.Text;

namespace ArgentiPokerChallenge
{
    public class Hand
    {
        public List<Card> Cards { get; set; } = new List<Card>();
        public List<Pair> Pairs { get; set; } = new List<Pair>();
        public int Triple { get; set; } = -1;
        public int Quads { get; set; } = -1;
        private void SortAscending()
        {
            Cards.Sort(CompareCardsByValue);
        }
        private int CompareCardsByValue(Card x, Card y)
        {
            if (x.Value == y.Value)
                return 0;
            else
            {
                if (x.Value < y.Value)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
        }
        public int GetHandRank()
        {
            //note for the below: I did think about creating an Enum for the hand ranks but ultimately decided for simplicity's sake to just use ints
            SortAscending();
            bool isStraight = IsStraight();
            bool isFlush = IsFlush();
            bool isStraightFlush = isStraight && isFlush;
            bool isRoyalFlush = isStraightFlush && Cards[4].Value == 14;
            //my idea is to waterfall down each condition
            if (isRoyalFlush)
                return 10;
            if (isStraightFlush)
                return 9;
            if (IsFourOfAKind())
                return 8;
            //the above invalidate other tests so I run them first 
            bool isThreeOfAKind = IsThreeOfAKind();
            //start at second card
            GetPairs();
            if (Pairs.Count == 1 && isThreeOfAKind)//full house
                return 7;
            if (isFlush)
                return 6;
            if (isStraight)
                return 5;
            if (isThreeOfAKind)
                return 4;
            if (Pairs.Count == 2)
                return 3;
            if (Pairs.Count == 1)
                return 2;
            return 1;
        }
        private bool IsStraight()
        {
            if (Cards[0].Value == Cards[1].Value - 1 &&
                Cards[1].Value == Cards[2].Value - 1 &&
                Cards[2].Value == Cards[3].Value - 1 &&
                Cards[3].Value == Cards[4].Value - 1)
                return true;
            else
                return false;
        }
        private bool IsFlush()
        {
            if (Cards[0].Suit == Cards[1].Suit &&
                Cards[0].Suit == Cards[2].Suit &&
                Cards[0].Suit == Cards[3].Suit &&
                Cards[0].Suit == Cards[4].Suit)
                return true;
            else
                return false;
        }
        private bool IsFourOfAKind()
        {
            if (Cards[0].Value == Cards[1].Value &&
                Cards[0].Value == Cards[2].Value &&
                Cards[0].Value == Cards[3].Value)
            {
                Quads = Cards[0].Value;
                return true;
            }
            else if (Cards[4].Value == Cards[1].Value &&
               Cards[4].Value == Cards[2].Value &&
               Cards[4].Value == Cards[3].Value)
            {
                Quads = Cards[4].Value;
                return true;
            }
            else
                return false;
        }
        private bool IsThreeOfAKind()
        {
            if (Cards[0].Value == Cards[1].Value && Cards[0].Value == Cards[2].Value)
            {
                Triple = Cards[0].Value;
                return true;
            }
            else if (Cards[1].Value == Cards[2].Value && Cards[1].Value == Cards[3].Value)
            {
                Triple = Cards[1].Value;
                return true;
            }
            else if (Cards[2].Value == Cards[3].Value && Cards[2].Value == Cards[4].Value)
            {
                Triple = Cards[2].Value;
                return true;
            }
            else
                return false;
        }
        private void GetPairs()
        {
            int prevValue = Cards[0].Value;
            for (int i = 1; i < Cards.Count; i++)
            {
                //edge case for when the last two are a pair
                if (i == Cards.Count - 1 && Cards[i].Value == prevValue && Cards[i].Value != Triple)
                {
                    Pairs.Add(new Pair
                    {
                        CardOne = Cards[i - 1],
                        CardTwo = Cards[i]
                    });
                    break; // reach last pair, no longer need to complete loop
                }
                else if (i == Cards.Count - 1 && Cards[i].Value == Triple) //edge case for when the last three may be a triple
                {
                    break;
                }
                else if (Cards[i].Value == prevValue && Cards[i + 1].Value != prevValue && Cards[i].Value != Triple)
                {
                    Pairs.Add(new Pair
                    {
                        CardOne = Cards[i - 1],
                        CardTwo = Cards[i]
                    });
                    prevValue = -1;
                }
                else
                    prevValue = Cards[i].Value;
            }
        }        
        
    }

    public class Pair
    {
        public Card CardOne { get; set; }
        public Card CardTwo { get; set; }
    }
}
