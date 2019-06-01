using System;
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
	}
}
