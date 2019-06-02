using System;
using System.Collections.Generic;
using System.Linq;

namespace CardGame {
	public class Player {

		public string Name { get; set; }

		public PlayerType PlayerType { get; set; }
		public AiType AiType { get; set; }

		public List<Card> Hand { get; set; } = new List<Card>();
		public int Tricks;

		public Player Partner;

		private Random random = new Random();

		public Player(string name, PlayerType playerType) {
			this.Name = name;
			this.PlayerType = playerType;
		}

		public Player(string name, PlayerType playerType, AiType aiType) {
			this.Name = name;
			this.PlayerType = playerType;
			this.AiType = aiType;
		}

		public bool IsHuman() {
			return PlayerType.Equals(PlayerType.HUMAN);
		}

		public bool IsAi() {
			return PlayerType.Equals(PlayerType.AI);
		}

		public bool IsAiRandom() {
			return AiType.Equals(AiType.RANDOM);
		}

		public bool IsAiEasy() {
			return AiType.Equals(AiType.EASY);
		}

		public bool IsAiMedium() {
			return AiType.Equals(AiType.MEDIUM);
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

		private Card PlayCard(Card card) {
			card.PlayedBy = this;
			Hand.Remove(card);
			return card;
		}

		private Card PlayCard(int handIndex) {
			Card card = Hand[handIndex];
			card.PlayedBy = this;
			Hand.RemoveAt(handIndex);
			return card;
		}

		//Lead the longest suited ace first
		public Card AiPickLeadCard() {

			List<Card> offTrumpAces = Hand.FindAll(card => card.Rank.Equals(Rank.ACE) && !card.IsTrump);

			if (offTrumpAces.Count>0) {

				int currentMaxSuitCount = 0;
				Card currentAce = null;

				offTrumpAces.ForEach(ace => {

					int suitCount = Hand.Count(card => card.Suit.Equals(ace.Suit));

					if (suitCount > currentMaxSuitCount) {
						currentMaxSuitCount = suitCount;
						currentAce = ace;
					}

				});

				return PlayCard(currentAce);

			} else {

				return PopRandomCard();

			}
		}

		public Card AiPickRandomCard(Card leadCard) {

			List<Card> sameSuit = Hand.FindAll(card => card.Suit.Equals(leadCard.Suit));

			if (sameSuit.Count > 0) {

				return PlayCard(sameSuit[random.Next(0, sameSuit.Count)]);

			} else {

				return PlayCard(random.Next(0, Hand.Count));

			}
		}

		//Follow suit if have card higher, throw highest, else throw lowest, else throw lowest spade, else throw lowest card
		public Card AiPickCard(Card leadCard, Card currentWinningCard) {

			if (currentWinningCard.PlayedBy.Equals(Partner)) {
				return PlayCard(Hand.OrderByDescending(card => card.Value).ToList().Last());
			}

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

				cardsInSuit = cardsInSuit.OrderByDescending(card => card.Value).ToList();

				if (cardsInSuit.First().Value>currentWinningCard.Value) {
					cardToPlay = cardsInSuit.First();
				} else {
					cardToPlay = cardsInSuit.Last();
				}

			} else if (trumps.Any()) {

				trumps = trumps.OrderBy(card => card.Value).ToList();

				if (trumps.First().Value>currentWinningCard.Value) {
					cardToPlay = trumps.First();
				} else {
					offSuit = offSuit.OrderByDescending(card => card.Value).ToList();
					if (offSuit.Any()) {
						cardToPlay = offSuit.Last();
					} else {
						cardToPlay = trumps.Last();
					}
				}


			} else {

				offSuit = offSuit.OrderByDescending(card => card.Value).ToList();

				cardToPlay = offSuit.Last();

			}

			return PlayCard(cardToPlay);
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

					if (CardChoiceIsValid(leadCard, Hand[idx])) {

						Console.WriteLine(Hand[idx]);
						return PlayCard(idx);

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
		
			return PlayCard(random.Next(0, Hand.Count()));

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
		AI,
	
	}

	public enum AiType {
	
		RANDOM,
		EASY,
		MEDIUM
	}
}
