using System;
using System.Collections.Generic;
using System.Linq;

namespace CardGame {
	class MainClass {

		private static Random random = new Random();

		public static List<Player> RandomOrderedPlayerList() {

			Player player1 = new Player("Drew", PlayerType.AI, AiType.MEDIUM);
			Player player2 = new Player("Jordan", PlayerType.AI, AiType.EASY);
			Player player3 = new Player("Rain", PlayerType.AI, AiType.EASY);
			Player player4 = new Player("Riley", PlayerType.AI, AiType.EASY);

			List<Player> players = new List<Player> { player1, player2, player3, player4 };

			List<Player> randomized = new List<Player>();

			while (players.Any()) {
				int idx = random.Next(0, players.Count);
				randomized.Add(players[idx]);
				players.RemoveAt(idx);
			}

			randomized[0].Partner = randomized[2];
			randomized[2].Partner = randomized[0];
			randomized[1].Partner = randomized[3];
			randomized[3].Partner = randomized[1];

			return randomized;

		}

		public static void Mainn(String[] args) {

			Tests.TestLeadCard();
		}

		public static void Main(string[] args) {

			SimpleSpadesRound.debug = false;

			List<Player> players = RandomOrderedPlayerList();

			Dictionary<string, int> wins = new Dictionary<string, int>();
			players.ForEach(player => wins[player.Name] = 0);

			int numRounds = 10000;
			int roundCounter = 0;

			while (roundCounter < numRounds) {

				SimpleSpadesRound round = new SimpleSpadesRound(players);
				round.Deal();
				//round.SortHands(HandSortOptions.STANDARD);
				//round.Bid(); //TODO
				round.SetCardValues();
				round.SortHands(HandSortOptions.TRUMP_VALUE);
				round.PlayRound();

				wins[round.GetWinner().Name]++;
				wins[round.GetWinner().Partner.Name]++;

				players = RandomOrderedPlayerList();

				roundCounter++;
			}

			wins.OrderByDescending(kv => kv.Value)
				.ToList()
				.ForEach(kv => Console.WriteLine(kv.Key + " : " + kv.Value));

		}


	}
}

