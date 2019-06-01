using System;
namespace CardGame {
	public interface ICardGameRound {

		void Deal();

		void SetCardValues();

		void SortHands(HandSortOptions sortOptions);

		void PrintGameState();
	}
}
