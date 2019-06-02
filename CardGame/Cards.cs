using System;
using System.Collections.Generic;
using System.Linq;

namespace CardGame {


	public class Deck {

		public List<Card> Cards { get; set; } = new List<Card>();
		private Random random = new Random();

		public Deck(int minRank, bool shuffled = true) {

			foreach (Rank rank in Enum.GetValues(typeof(Rank))) {

				if ((int)rank >= minRank) {

					foreach (Suit suit in Enum.GetValues(typeof(Suit))) {

						Card card = new Card(rank, suit);
						card.Value = (int)card.Rank - minRank + 1;
						this.Cards.Add(card);

					}
				}
			}

			if (shuffled) { this.Shuffle(); }

		}


		public Card Take() {
			Card card = null;
			if (Cards.Any()) {
				card = Cards[0];
				Cards.RemoveAt(0);
			}
			return card;
		}

		public void Shuffle() {
			List<Card> shuffled = new List<Card>();
			while (Cards.Any()) {
				int idx = random.Next(Cards.Count);
				shuffled.Add(Cards[idx]);
				Cards.RemoveAt(idx);
			}
			this.Cards = shuffled;
		}

	
	}

	public class Card {

		public Rank Rank { get; set; }
		public Suit Suit { get; set; }
		public Color Color { get; set; }
		public int Value { get; set; }
		public bool IsTrump { get; set; }
		public Player PlayedBy { get; set; }


		public Card(Rank rank, Suit suit) {
			this.Rank = rank;
			this.Suit = suit;
			this.Color = (suit.Equals(Suit.CLUB) || suit.Equals(Suit.SPADE)) ? Color.BLACK : Color.RED;
		}

		public override string ToString() {
			return this.Rank + ":" + this.Suit;
		}

	}

	public enum Rank {
		TWO = 2, THREE = 3, FOUR = 4, FIVE = 5, SIX = 6, SEVEN = 7, EIGHT = 8, NINE = 9, TEN = 10, JACK = 11, QUEEN = 12, KING = 13, ACE = 14
	}


	public enum Suit {
		CLUB = 1, DIAMOND = 2, HEART = 3, SPADE = 4
	}

	public enum Color {
		RED, BLACK
	}

}
