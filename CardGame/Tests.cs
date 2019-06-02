using System;
using System.Collections.Generic;
namespace CardGame {
	public class Tests {
		public Tests() {
		}

		public static void TrickyTrick() {

			Trick trick = new Trick(new Card(Rank.TEN, Suit.DIAMOND), new Player("a", PlayerType.HUMAN));
			trick.AddCard(new Card(Rank.ACE, Suit.CLUB), new Player("b", PlayerType.HUMAN));
			trick.AddCard(new Card(Rank.KING, Suit.CLUB), new Player("c", PlayerType.HUMAN));

			Player winner = trick.DetermineWinner();

			Console.WriteLine(winner.Name);
		}

		public static void TestLeadCard() {

			List<Player> players = MainClass.RandomOrderedPlayerList();
			SimpleSpadesRound round = new SimpleSpadesRound(players);
			round.Deal();
			round.SetCardValues();
			round.SortHands(HandSortOptions.TRUMP_VALUE);

			players[0].Hand.ForEach(card => Console.WriteLine(card));
			Card leadCard = players[0].AiPickLeadCard();
			Console.WriteLine("Lead: " + leadCard);
		}

	}
}
