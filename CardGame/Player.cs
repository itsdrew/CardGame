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
