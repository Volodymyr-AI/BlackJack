using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static BlackJack.Suit;
using static BlackJack.Face;

namespace BlackJack
{

    public enum Suit
    {
        Hearts,
        Spades,
        Clubs,
        Diamonds
    }

    public enum Face
    {
        Ace,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King
        
    }
    public class Card
    {
        public Suit Suit { get; set; }
        public Face Face { get; set; }
        public int Value { get; set; }
        public char Symbol { get; }

        //initialize value and suit symbol

        public Card(Suit suit, Face face)
        {
            Suit = suit;
            Face = face;

            switch (Suit)
            {
                case Clubs:
                    Symbol = '♣';
                    break;
                case Spades:
                    Symbol = '♠';
                    break;
                case Diamonds:
                    Symbol = '♦';
                    break;
                case Hearts:
                    Symbol = '♥';
                    break;
            }
            switch (Face)
            {
                case Ten:
                case Jack:
                case Queen:
                case King:
                    Value = 10;
                    break;
                case Ace:
                    Value = 11;
                    break;
                default:
                    Value = (int)Face + 1;
                    break;
            }
        }

        //print out description of card, Ace(soft == 1 or Hard == 11)
        public void WriteDescription()
        {
            if (Suit == Suit.Diamonds || Suit == Suit.Hearts)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                
            }
             else
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
            if(Face == Ace)
            {
                if(Value == 11)
                {
                    Console.WriteLine(Symbol + " Soft" + Face + " of " + Suit);
                }
                else
                {
                    Console.WriteLine(Symbol + " Hard" + Face + " of " + Suit);
                }
            }
            else
            {
                Console.WriteLine(Symbol + " " + Face + " of " + Suit);
            }
            Casino.ResetColor();
        }
    }
}
