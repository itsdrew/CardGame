using System;
using System.Collections.Generic;
using System.Linq;

namespace CardGame {
	public class Trick {

		public Dictionary<Card, Player> Cards {
			get;
			private set;
		}
		
		public Suit LeadSuit {
			get;
			private set;
		}


		public Trick(Card leadCard, Player leadPlayer) {

			this.LeadSuit = leadCard.Suit;

			this.Cards = new Dictionary<Card, Player> {
				{ leadCard, leadPlayer }
			};
		}

		public void AddCard(Card card, Player player) {
			Cards.Add(card, player);

		}

		public Player DetermineWinner() {

			Card winner = Cards.Keys
				.OrderByDescending(card => card.Value)
				.ThenBy(card => card.Suit.Equals(LeadSuit))
				.First();

			return Cards[winner];

		}

		public void PrintTrick() {

			Console.WriteLine("");

			List<Card> cards = Cards.Keys
				.OrderByDescending(card => card.Value)
				.ThenBy(card => card.Suit.Equals(LeadSuit))
				.ToList();

			cards.ForEach(card => Console.WriteLine(card + " : " + Cards[card].Name));

		}
	}
}
