using System;
using System.Collections.Generic;
using System.Linq;

namespace CardGame {
	public class Player {

		public string Name { get; set; }


		public PlayerType PlayerType { get; set; }

		public List<Card> Hand { get; set; } = new List<Card>();
		public int Tricks;
		public Player partner;

		public Player(string name, PlayerType playerType) {
			this.Name = name;
			this.PlayerType = playerType;
		}

		public bool IsHuman() {
			return PlayerType.Equals(PlayerType.HUMAN);
		}

		public bool IsAI() {
			return PlayerType.Equals(PlayerType.AI);
		}

		public void SortHand(HandSortOptions options) {
			if (options.Equals(HandSortOptions.STANDARD)) {
				this.Hand = Hand.OrderBy(card => card.Suit).ThenBy(card => card.Rank).ToList();
			} else if (options.Equals(HandSortOptions.TRUMP_VALUE)) {
				this.Hand = Hand.OrderBy(card => card.Value).ThenBy(card => card.Suit).ThenBy(card => card.Rank).ToList();
			}
		}

		private bool CardChoiceIsValid(Card leadCard, Card cardChoice) {

			Suit cardChoiceSuit = cardChoice.Suit;

			//TODO Change this so it works for a game that includes High/Low Jack or 9
			if (leadCard == null || cardChoice.Suit.Equals(leadCard.Suit)) {
				return true;
			} else {
				if (Hand.FindAll(card => card.Suit.Equals(leadCard.Suit)).Count>0) {
					return false;
				} else {
					return true;
				}
			}

		}

		//Follow suit if have card higher, throw highest, else throw lowest, else throw lowest spade, else throw lowest card
		public Card AIPickCard(Card leadCard, Card currentWinningCard) {

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

		public Card HumanPickCard(Card leadCard) {

			Console.WriteLine();
			PrintHandWithIndex();
			Console.WriteLine();
			Console.WriteLine("Select a card to play");
			Console.WriteLine();

			string userInput = Console.ReadLine();

			int idx;

			if (int.TryParse(userInput, out idx)) {

				if (idx>=0 && idx<Hand.Count) {
					
					Card choice = Hand[idx];

					if (CardChoiceIsValid(leadCard, choice)) {

						Console.WriteLine(choice);
						Hand.RemoveAt(idx);
						return choice;

					} else {

						Console.WriteLine("You must follow suit");
						return HumanPickCard(leadCard);

					}

				} else {

					Console.WriteLine("Enter a valid choice");
					return HumanPickCard(leadCard);

				}
			} else {

				Console.WriteLine("Enter a valid number");

				return HumanPickCard(leadCard);

			}


		}

			public Card PopRandomCard() {

			Random random = new Random();

			int randCardIndex = random.Next(0, Hand.Count());

			Card card = Hand[randCardIndex];
			Hand.RemoveAt(randCardIndex);

			return card;
		}

		public void PrintHandWithIndex() {

			for (int i = 0; i<Hand.Count; i++) {
				Console.WriteLine("(" + i + ")  " + Hand[i]);
			}

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

	public enum PlayerType {
	
		HUMAN,
		AI
	
	}
}
