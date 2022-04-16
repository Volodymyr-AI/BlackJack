using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;

namespace BlackJack
{
    public class Program
    {
        private static Deck deck = new Deck();
        private static Player player = new Player();

        // greeting player
        static string Acquaintance()
        {
            Console.Write("What is your name: ");
            string name = Console.ReadLine();
            player.Name = name;
            return name;
        }
        private enum RoundResult
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

            if (player.Hand[0].Face == Face.Ace && player.Hand[1].Face == Face.Ace)
            {
                player.Hand[1].Value = 1;
            }

            if (Dealer.HiddenCards[0].Face == Face.Ace && Dealer.HiddenCards[1].Face == Face.Ace)
            {
                Dealer.HiddenCards[1].Value = 1;
            }

            if (Dealer.HiddenCards[0].Face == Face.Ace && Dealer.HiddenCards[1].Face == Face.Ace)
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

            Dealer.RevealCard();

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

            while (Dealer.GetHandValue() <= 16)
            {
                Thread.Sleep(1000);
                Dealer.RevealedCards.Add(deck.DrawCard());

                Console.Clear();
                player.WriteHand();
                Dealer.WriteHand();
            }

            if (player.GetHandValue() > Dealer.GetHandValue())
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
            }
            else if (Dealer.GetHandValue() > player.GetHandValue())
            {
                EndRound(RoundResult.DEALER_WIN);
            }
            else
            {
                EndRound(RoundResult.PUSH);
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

                    Console.Write( player.Name + " enter Action (? for help): ");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    action = Console.ReadLine();
                    Casino.ResetColor();

                    switch (action.ToUpper())
                    {
                        case "HIT":
                            player.Hand.Add(deck.DrawCard());
                            break;
                        case "STAND":
                            break;
                        case "SURRENDER":
                            player.Hand.Clear();
                            break;
                        case "DOUBLE":
                            if (player.Chips <= player.Bet)
                            {
                                player.AddBet(player.Chips);
                            }
                            else
                            {
                                player.AddBet(player.Bet);
                            }
                            player.Hand.Add(deck.DrawCard());
                            break;
                        default:
                            Console.WriteLine("Valid Moves:");
                            Console.WriteLine("Hit, Stand, Surrunder, Double");
                            Console.WriteLine("Press <Enter> to continue");
                            Console.ReadKey();
                            break;
                    }
                    if (player.GetHandValue() > 21)
                    {
                        foreach (Card card in player.Hand)
                        {
                            if (card.Value == 21)
                            {
                                card.Value = 1;
                                break;
                            }
                        }
                    }
                } while (!action.ToUpper().Equals("STAND") && !action.ToUpper().Equals("DOUBLE")
                         && !action.ToUpper().Equals("SURRENDER") && player.GetHandValue() <= 21);
            }

            //take player's bet
            static bool TakeBet()
            {
                Console.WriteLine("Current Chip Count: ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(player.Chips);
                Casino.ResetColor();

                Console.WriteLine("Minimum Bet: ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(Casino.MinimumBet);
                Casino.ResetColor();

                Console.Write("Enter bet to begin " + player.HandsCompleted + ": ");
                Console.ForegroundColor = ConsoleColor.Magenta;
                string s = Console.ReadLine();
                Casino.ResetColor();

                if (Int32.TryParse(s, out int bet) && bet >= Casino.MinimumBet && player.Chips >= bet)
                {
                    player.AddBet(bet);
                    return true;
                }
                return false;
            }

            // perform action based on result of round and start next 
            static void EndRound(RoundResult result)
            {
                switch (result)
                {
                    case RoundResult.PUSH:
                        player.CancelBet();
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine("Player and Dealer Push.");
                        break;
                    case RoundResult.PLAYER_WIN:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Player wins " + player.WinBet(false) + " chips");
                        break;
                    case RoundResult.PLAYER_LOST:
                        player.DenyBet();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Player lost =(");
                        break;
                    case RoundResult.PLAYER_BLACKJACK:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Player wins " + player.WinBet(true) + " chips with BlackJack");
                        break;
                    case RoundResult.DEALER_WIN:
                        player.DenyBet();
                        Console.ForegroundColor= ConsoleColor.Red;
                        Console.WriteLine("Dealer wins");
                        break;
                    case RoundResult.SURRENDER:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Player Surrenders " + (player.Bet/2) + " chips");
                        player.Chips += player.Bet / 2;
                        player.DenyBet(); 
                        break;
                    case RoundResult.INVALID_BET:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid Bet");
                        break;
                }

                if(player.Chips <= 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;

                    Console.WriteLine();
                    Console.WriteLine("You ran out of chips after " + (player.HandsCompleted - 1) + " rounds.");
                    Console.WriteLine("500 more chips will be added to your pocket and your score will be reset");

                    player = new Player();
                }
                Casino.ResetColor();
                Console.WriteLine("Press <Enter> to continue");
                Console.ReadKey();
                StartRound();
            }
        }


        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8; // won't see ♠♥♣♦ without it

            Console.WriteLine("Welcome to BlackJack♠♥♣♦");
            Console.WriteLine("Press <Enter> to start");
            Console.ReadKey();
            Acquaintance();
            StartRound();
        }
    }
}
