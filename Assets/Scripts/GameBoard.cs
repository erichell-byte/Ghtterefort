using System.Collections.Generic;
using System.Linq;
using System;
using Random = System.Random;

namespace CardProject
{
    public class GameBoard
    {
        private List<Card> cards;
        private int rows;
        private int columns;

        private int cardIterator;

        public int Rows => rows;
        public int Columns => columns;

        public GameBoard(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;
            InitializeCards();
        }

        private void InitializeCards()
        {
            int pairsCount = rows * columns / 2;
            cards = new List<Card>();

            for (int i = 0; i < pairsCount; i++)
            {
                cards.Add(new Card { Id = i });
                cards.Add(new Card { Id = i });
            }
        
            Random rng = new Random();
            cards = cards.OrderBy(c => rng.Next()).ToList();
        }

        public Card GetNextCard()
        {
            if (cards == null)
            {
                throw new Exception("cards list is empty");
            }

            return cards[cardIterator++];
        }

        public int GetCurrentCardIndex()
        {
            return cardIterator;
        }
    }
}