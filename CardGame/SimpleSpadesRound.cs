using System;
using System.Collections.Generic;
using System.Linq;

namespace CardGame {
	public class SimpleSpadesRound : ICardGameRound {

		public static bool debug = false;

		public List<Player> Players {get; set;}
		public Deck Deck {get; set;}

		private readonly int minCardRank = 2;
		private int numStartingCardsInHand;

		private Random random = new Random();

		private Suit TrumpSuit = Suit.SPADE;

		public SimpleSpadesRound(List<Player> players) {
			this.Players = players;
			this.Deck = new Deck(minCardRank);

			if (this.Deck.Cards.Count % Players.Count != 0) {
				throw new Exception("Deck % Players != 0");
			} else if (this.Deck.Cards.Count/Players.Count % 2 !=1) { //Odd number of tricks
				throw new Exception("Game requires an odd number of tricks");
			} else {
				numStartingCardsInHand = this.Deck.Cards.Count / Players.Count;
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
				.FindAll(card => card.Suit.Equals(TrumpSuit))
				.ForEach(card => { card.Value += 15 - minCardRank; card.IsTrump = true; });
			});
		}

		public void SortHands(HandSortOptions sortOptions) {
			Players.ForEach(player => player.SortHand(sortOptions));
		}

		public void PlayRound() {

			//TODO select lead player
			Player leadPlayer = Players[0];

			while (leadPlayer.Hand.Any()) {

				Debug();
				Debug(leadPlayer.Name + " to lead." );
				Debug();

				Card leadCard = null;

				if (leadPlayer.IsHuman()) {
					leadCard = leadPlayer.HumanPickCard(leadCard);
				} else {

					if (leadPlayer.IsAiEasy() || leadPlayer.IsAiRandom()) {
						leadCard = leadPlayer.PopRandomCard();
					} else if (leadPlayer.IsAiMedium()) {
						PrintGameState();
						if (leadPlayer.Hand.Count == numStartingCardsInHand) {
							leadCard = leadPlayer.AiPickLeadCard(); //This appears to have no effect when players can't use the information gained.
						} else {
							leadCard = leadPlayer.PopRandomCard();
						}
						Debug(leadCard);
					}
				}

				Trick trick = new Trick(leadCard, leadPlayer);

				Card currentWinningCard = leadCard;

				foreach (Player player in Players) {

					if (!player.Equals(leadPlayer)) {

						Card cardToPlay = null;
						if (player.IsHuman()) {
							trick.PrintTrick();
							cardToPlay = player.HumanPickCard(leadCard);
						} else { //Player is AI

							if (player.IsAiRandom()) {
								cardToPlay = player.AiPickRandomCard(leadCard);
							} else {
								cardToPlay = player.AiPickCard(leadCard, currentWinningCard);
							}

						}

						trick.AddCard(cardToPlay, player);

						if (cardToPlay.Value > currentWinningCard.Value &&
							cardToPlay.Suit.Equals(leadCard.Suit) || cardToPlay.IsTrump) {

							currentWinningCard = cardToPlay;
						}

					}
				}

				Debug();
				if (debug) {
					trick.PrintTrick();
				}
				Debug();

				Player winner = trick.DetermineWinner();
				winner.Tricks++;

				Debug("Trick won by: " + winner.Name);
				Debug();
				Debug();

				leadPlayer = winner;

				//DEBUG
				//trick.PrintTrick();
				//Debug(winner.Name);
			}

		}



		public void PrintGameState() {

			Players.ForEach(player => {
				Debug("");
				Debug(player.Name);
				player.Hand.ForEach(card => Debug(card));
				Debug(player.Tricks);
			});
		}

		public void PrintScores() {
			Players.ForEach(player => {
				Console.WriteLine();
				Console.WriteLine(player.Name);
				Console.WriteLine(player.Tricks);
			});

		}

		public Player GetWinner() {
			List<Player> teamOne = new List<Player>() { Players[0], Players[0].Partner };
			List<Player> teamTwo = new List<Player>();

			Players.ForEach(player => { if (!teamOne.Contains(player)) { teamTwo.Add(player); } });

			int teamOneScore = teamOne.Sum(player => player.Tricks);
			int teamTwoScore = teamTwo.Sum(player => player.Tricks);

			return teamOneScore > teamTwoScore ? teamOne[0] : teamTwo[0];
		}

		private void Debug(Object text = null) {
			if (debug) {
				if (text == null) {
					Console.WriteLine();
				} else {
					Console.WriteLine(text);
				}
			}
		}
	}
}
