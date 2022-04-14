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
            PLAYER_BLACKJACK,
            DEALER_WIN,
            SURRENDER,
            INVALID_BET
        }

        static void InitializeHands()
        {
            deck.Initialize();

            player.Hand = deck.DealHand();
            Dealer.HiddenCards = deck.DealHand();
            Dealer.RevealedCards = new List<Card>();

            if(player.Hand[0].Face == Face.Ace && player.Hand[1].Face == Face.Ace)
            {
                player.Hand[1].Value = 1;
            }

            if (Dealer.HiddenCards[0].Face == Face.Ace && Dealer.HiddenCards[1].Face == Face.Ace)
            {
                Dealer.HiddenCards[1].Value = 1;
            }

            if(Dealer.HiddenCards[0].Face == Face.Ace && Dealer.HiddenCards[1].Face == Face.Ace)
            {
                Dealer.HiddenCards[1].Value = 1;
            }

            Dealer.RevealCard();

            player.WriteHand();
            Dealer.WriteHand();
        }

        static void StartRound()
        {
            Console.Clear();

            if (!TakeBet())
            {
                EndRound(RoundResult.INVALID_BET);
                return;
            }
            Console.Clear();

            InitializeHands();
            TakeActions();

            Dealer.ReavealCard();

            Console.Clear();
            player.WriteHand();
            Dealer.WriteHand();

            player.HandsCompleted++;

            if (player.Hand.Count == 0)
            {
                EndRound(RoundResult.SURRENDER);
                return;
            }
            else if (player.GetHandValue() > 21)
            {
                EndRound(RoundResult.PLAYER_LOST);
                return;
            }

            while(Dealer.GetHandValue() <= 16)
            {
                Thread.Sleep(1000);
                Dealer.RevealedCards.Add(deck.DrawCard());

                Console.Clear();
                player.WriteHand();
                Dealer.WriteHand();
            }

            if(player.GetHandValue() > Dealer.GetHandValue())
            {
                player.Wins++;
                if (Casino.IsHandIsBlackJack(player.Hand))
                {
                    EndRound(RoundResult.PLAYER_BLACKJACK);

                }
                else
                {
                    EndRound(RoundResult.PLAYER_WIN);

                }
                else if (Dealer.GetHandValue() > player.GetHandValue())
                {
                    EndRound(RoundResult.DEALER_WIN);

                }
                else
                {
                    EndRound(RoundResult.PUSH)
                }
            }

            //Ask user for action and perform his action until they stand , double or bust
            static void TakeActions()
            {
                string action;
                do
                {
                    Console.Clear();
                    player.WriteHand();
                    Dealer.WriteHand();

                    Console.Write("Enter Action: ");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    action = Console.ReadLine();
                    Casino.ResetColor();

                    switch (action.ToLower())
                    {
                        case "HIT":
                            player.Hand.Add(deck.DrawCard());
                            break;
                        case "STAND":
                            break;
                        case "SURRENDER":
                            player.Hand.Clear();
                        case "DOUBLE":
                            if(player.Chips <= player.Bet)
                            {
                                player.AddBet(player.Chips);
                            }
                            else
                            {
                                player.AddBet(player.Bet);
                            }
                    }
                }
            }
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
