using System;
using System.Collections.Generic;
using System.Linq;

namespace CardGame {
	public class Player {

		public string Name { get; set; }
		public List<Card> Hand { get; set; } = new List<Card>();
		public int Tricks;
		public Player partner;

		public Player(string name) {
			this.Name = name;
		}

		public void SortHand(HandSortOptions options) {
			if (options.Equals(HandSortOptions.STANDARD)) {
				this.Hand = Hand.OrderBy(card => card.Suit).ThenBy(card => card.Rank).ToList();
			} else if (options.Equals(HandSortOptions.TRUMP_VALUE)) {
				this.Hand = Hand.OrderBy(card => card.Value).ThenBy(card => card.Suit).ThenBy(card => card.Rank).ToList();
			}
		}

		//Follow suit if have card higher, throw highest, else throw lowest, else throw lowest spade, else throw lowest card
		public Card PopRandomCard(Card leadCard, Card currentWinningCard) {

			List<Card> cardsInSuit = new List<Card>();
			List<Card> trumps = new List<Card>();
			List<Card> offSuit = new List<Card>();

			foreach (Card card in Hand) {
				if (card.Suit.Equals(leadCard.Suit)) { //And card is not trump for games where trump is not only by suit
					cardsInSuit.Add(card);
				} else if (card.IsTrump) {
					trumps.Add(card);
				} else {
					offSuit.Add(card);
				}
			}

			Card cardToPlay = null;

			if (cardsInSuit.Any()) {

				cardsInSuit.OrderByDescending(card => card.Value);

				if (cardsInSuit.First().Value>currentWinningCard.Value) {
					cardToPlay = cardsInSuit.First();
				} else {
					cardToPlay = cardsInSuit.Last();
				}

			} else if (trumps.Any()) {

				trumps.OrderBy(card => card.Value);

				if (trumps.First().Value>currentWinningCard.Value) {
					cardToPlay = trumps.First();
				} else {
					offSuit.OrderByDescending(card => card.Value);
					cardToPlay = offSuit.Last();
				}


			} else {

				offSuit.OrderByDescending(card => card.Value);

				cardToPlay = offSuit.Last();

			}

			Hand.Remove(cardToPlay);

			return cardToPlay;
		}

		public Card PopRandomCard() {

			Random random = new Random();

			int randCardIndex = random.Next(0, Hand.Count());

			Card card = Hand[randCardIndex];
			Hand.RemoveAt(randCardIndex);

			return card;
		}

		//Debug
		public void PrintHand() {
			Hand.ForEach(card => Console.WriteLine(card));
		}

	}

	public enum HandSortOptions {

		STANDARD,
		TRUMP_VALUE
	}
}
