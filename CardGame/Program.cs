using System;
using System.Collections.Generic;

namespace CardGame {
	class MainClass {
		public static void Main(string[] args) {


			Player player1 = new Player("Drew");
			Player player2 = new Player("Jordan");
			Player player3 = new Player("Rain");
			Player player4 = new Player("Riley");

			List<Player> players = new List<Player> { player1 };

			Round round = new Round(players);
			round.Deal();
			round.SortHands(HandSortOptions.STANDARD);
			round.Bid(); //TODO
			round.SetCardValues();
			round.SortHands(HandSortOptions.TRUMP_VALUE);
			round.Play(); //TODO

			//Test
			round.Players[0].Hand.ForEach(card => Console.WriteLine(card));

		}
	}
}
