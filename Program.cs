using System;
using System.Text;

namespace BlackJack
{
    internal class Program
    {
        private static Deck deck = new Deck();
        private static Player player = new Player();

        private enum GameResult
        {
            PUSH,
            PLAYER_WIN,
            PLAYER_LOST,
            DEAKER_WIN,
            SURRENDER,
            PLAYER_BLACKJACK,
            INVALID_BET
        }

        

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8; // won't see ♠♥♣♦ without it

            Console.WriteLine("Welcome to BlackJack♠♥♣♦");
            Console.WriteLine("Press <Enter> to start");
            Console.ReadKey();
            GameStart();
        }
    }
}
