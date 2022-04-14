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
        }
    }
}
