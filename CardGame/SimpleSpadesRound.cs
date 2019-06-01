using System;
using System.Collections.Generic;
using System.Linq;

namespace CardGame {
	public class SimpleSpadesRound : ICardGameRound {
	

		public List<Player> Players {get; set;}
		public Deck Deck {get; set;}

		private readonly int minCardRank = 10;

		public SimpleSpadesRound(List<Player> players) {
			this.Players = players;
			this.Deck = new Deck(minCardRank);

			if (this.Deck.Cards.Count % Players.Count != 0) {
				throw new Exception("Deck % Players != 0");
			}
		}

		public void Deal() {

			while (Deck.Cards.Any()) {
				Players.ForEach(player => player.Hand.Add(Deck.Take()));
			}

		}

		public void SetCardValues() {
			Players.ForEach(player => {
				player.Hand
				.FindAll(card => card.Suit.Equals(Suit.SPADE))
				.ForEach(card => card.Value += 15 - minCardRank);
			});
		}

		public void SortHands(HandSortOptions sortOptions) {
			Players.ForEach(player => player.SortHand(sortOptions));
		}

		public void PlayRound() {

			//TODO select lead player
			Player leadPlayer = Players[0];

			while (leadPlayer.Hand.Any()) {

				//TODO select card
				Card leadCard = leadPlayer.PopRandomCard();
				Trick trick = new Trick(leadCard, leadPlayer);
				Card currentWinningCard = leadCard;

				foreach (Player player in Players) {
					if (!player.Equals(leadPlayer)) {

						Card cardToPlay = player.PopRandomCard(leadCard, currentWinningCard);

						trick.AddCard(cardToPlay, player);

						if (cardToPlay.Value > currentWinningCard.Value &&
							cardToPlay.Suit.Equals(leadCard.Suit) || cardToPlay.IsTrump)

							currentWinningCard = cardToPlay;
					}
				}

				Player winner = trick.DetermineWinner();
				winner.Tricks++;

				leadPlayer = winner;

				//DEBUG
				//trick.PrintTrick();
				//Console.WriteLine(winner.Name);
			}

		}



		public void PrintGameState() {

			Players.ForEach(player => {
				Console.WriteLine("");
				Console.WriteLine(player.Name);
				player.Hand.ForEach(card => Console.WriteLine(card));
				Console.WriteLine(player.Tricks);
			});
		}

		public void PrintScores() {
			Players.ForEach(player => {
				Console.WriteLine("");
				Console.WriteLine(player.Name);
				Console.WriteLine(player.Tricks);
			});

		}

		public Player GetWinner() {
			return Players.OrderBy(player => player.Tricks).First();
		}
	}
}
