using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    internal class Deck
    {
        private List<Card> cards;
        
        //create deck

        public Deck()
        {
            Initialize();
        }

        public List<Card> GetColdDeck()
        {
            List<Card> coldDeck = new List<Card>();

            for(int i = 0; i < 13; i++)
            {
                for(int j = 0; j < 4; j++) // here N(O) != 1 
                {
                    coldDeck.Add(new Card((Suit)j, (Face)i));
                }
            }
            return coldDeck;
        }

        //remove top 2 card of deck and turn into list
        public List<Card> DealHand()
        {
            List<Card> hand = new List<Card>();
            hand.Add(cards[0]);
            hand.Add(cards[1]);
            //remove added to hand cards
            cards.RemoveRange(0, 2);

            return hand;
        }

        //pick top card and remove it from deck

    public Card DrawCard()
        {
            Card card = cards[0];
            cards.Remove(card);

            return card;
        }

        //randomize the order of the cards in the deck
        public void Shuffle()
        {
            Random rng = new Random();

            int n = cards.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n+1);
                Card card = cards[k];
                cards[k] = cards[n];
                cards[n] = card;
            }
        }

        //replace the deck with a cold deck and then shuffle it
        public void Initialize()
        {
            cards = GetColdDeck();
            Shuffle();
        }
    }
}
