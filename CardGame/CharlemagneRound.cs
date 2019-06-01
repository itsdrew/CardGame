using System;
using System.Collections.Generic;
using System.Linq;

namespace CardGame {

	//Charlemagne
	public class CharlemagneRound : ICardGameRound {

		public List<Player> Players { get; set; }
		public Deck Deck { get; set; }
		public Suit Trump { get; set; }

		public CharlemagneRound(List<Player> players) {

			this.Players = players;
			this.Deck = new Deck(7);

			if (this.Deck.Cards.Count % Players.Count != 0) {
				throw new Exception("Deck % Players != 0");
			}

		}

		public void Deal() {

			while (Deck.Cards.Any()) {
				Players.ForEach(player => player.Hand.Add(Deck.Take()));
			}

		}

		public void Bid() {
			//TODO
			Suit trumpSuit = Suit.SPADE;
			this.Trump = trumpSuit;
		}

		public void SetCardValues() {

			Color trumpColor = (this.Trump.Equals(Suit.CLUB) || this.Trump.Equals(Suit.SPADE)) ? Color.BLACK : Color.RED;

			Players.ForEach(player => {

				player.Hand.ForEach(card => {
					if (card.Suit.Equals(this.Trump)) {
						card.IsTrump = true;
						if (card.Rank.Equals(Rank.NINE)) {
							card.Value = 17;
						} else if (card.Rank.Equals(Rank.JACK)) {
							card.Value = 16;
						} else if (card.Rank.Equals(Rank.SEVEN) || card.Rank.Equals(Rank.EIGHT)) {
							card.Value += 8;
						} else if (card.Rank.Equals(Rank.TEN)) {
							card.Value += 7;
						} else {
							card.Value += 6;
						}
					} else {
						if (card.Rank.Equals(Rank.JACK) && card.Color.Equals(trumpColor)) {
							card.Value = 15;
						}
					}
				});
			});
		}

		public void SortHands(HandSortOptions sortOptions) {
			Players.ForEach(player => player.SortHand(sortOptions));
		}

		public void PrintGameState() {

			Players.ForEach(player => {
				Console.WriteLine(player.Name);
				Console.WriteLine(player.Hand);
			});
		}

		public void Play() {
			//TODO
		}
	}
}
