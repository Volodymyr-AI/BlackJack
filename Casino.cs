using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    public class Casino
    {
        public static int MinimumBet { get; set; } = 20;

        //check hand; check hand for blackjack
        public static bool IsHandIsBlackJack(List<Card> hand)
        {
            if (hand.Count == 2)
            {
                if (hand[0].Face == Face.Ace && hand[1].Value == 10) return true;
                else if (hand[1].Face == Face.Ace && hand[0].Value == 10) return true;
            }

            return false;
        }

        // console colors to darkgrey on black
        public static void ResetColor()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.BackgroundColor = ConsoleColor.Black;
        }

    }
    public class Player
    {
        public int Chips { get; set; } = 500;
        public int Bet { get; set; }
        public int  Wins { get; set; }
        public int HandsCompleted { get; set; } = 1;
        public string Name { get; set; }

        public List<Card> Hand { get; set; }

        
        public void AddBet(int bet)
        {
            Bet += bet;
            Chips -= bet;
        }
        //set bet to 0
        public void DenyBet()
        {
            Bet = 0;
        }
        public void CancelBet()
        {
            Chips += Bet;
            DenyBet();
        }

        public int WinBet(bool blackjack)
        {
            int chipsWon;
            if (blackjack)
            {
                chipsWon = (int)Math.Floor(Bet * 2.5);
            }
            else
            {
                chipsWon = Bet * 2;
            }
            Chips += chipsWon;
            DenyBet();
            return chipsWon;
        }

        public int GetHandValue()
        {
            int value = 0;
            foreach(Card card in Hand){
                value += card.Value;
            }
            return value;
        }
        //output player's hand to console
        public void WriteHand()
        {
            Console.WriteLine("Bet: ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(Bet + " ");
            Casino.ResetColor();
            Console.Write("Chips: ");
            Console.ForegroundColor= ConsoleColor.Green;
            Console.ResetColor();
            Console.Write("Wins: ");
            Console.ForegroundColor=ConsoleColor.Yellow;
            Console.WriteLine(Wins);
            Console.ResetColor();
            Console.WriteLine("Round #" + HandsCompleted);

            Console.WriteLine();
            Console.WriteLine("Your Hand (" + GetHandValue() + "):");
            foreach(Card card in Hand)
            {
                card.WriteDescription();
            }
            Console.WriteLine();

        }
    }

    public class Dealer
    {
        public static List<Card> HiddenCards { get; set; } = new List<Card>();
        public static List<Card> RevealedCards { get; set; } = new List<Card>();

        //dealer takes first card from hidden and reveal it
        public static void RevealCard()
        {
            RevealedCards.Add(HiddenCards[0]);
            HiddenCards.RemoveAt(0);
        }
        // value of all revealedcards
        public static int GetHandValue()
        {
            int value = 0;
            foreach (Card card in RevealedCards)
            {
                value += card.Value;
            }
            return value;
        }

        //get dealers revealedcards to console
        public static void WriteHand()
        {
            Console.WriteLine("Dealer's Hand (" + GetHandValue() + "):");
            foreach(Card card in RevealedCards)
            {
                card.WriteDescription();
            }
            for(int i = 0; i < HiddenCards.Count; i++)
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("<hidden>");
                Casino.ResetColor();
            }
            Console.WriteLine();
        }
    }
    
}
