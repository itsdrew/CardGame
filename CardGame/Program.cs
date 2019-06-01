using System;
using System.Collections.Generic;

namespace CardGame {
	class MainClass {
		public static void Main(string[] args) {


			Player player1 = new Player("Drew");
			Player player2 = new Player("Jordan");
			Player player3 = new Player("Rain");
			Player player4 = new Player("Riley");

			List<Player> players = new List<Player> { player1, player2, player3, player4 };

			int numRounds = 100000;

			while (numRounds > 0) {


				SimpleSpadesRound round = new SimpleSpadesRound(players);
				round.Deal();
				round.SortHands(HandSortOptions.STANDARD);
				//round.Bid(); //TODO
				round.SetCardValues();
				round.SortHands(HandSortOptions.TRUMP_VALUE);
				//round.Play(); //TODO
				//round.PrintGameState();
				
				round.PlayRound();


				round.PrintScores();

				numRounds--;
			}


		}
	}
}
