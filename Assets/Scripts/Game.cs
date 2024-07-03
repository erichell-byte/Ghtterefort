using System.Collections.Generic;
using UnityEngine;

namespace CardProject
{
	public class Game : MonoBehaviour
	{
		[SerializeField] private SaveSystemProvider saveProvider;
		[SerializeField] private MenuView menu;
		[SerializeField] private CardPool cardPool;
		[SerializeField] private GameBoardView boardView;
		[SerializeField] private ScoreController scoreController;
		
		private int currentLevelIndex;
		private int currentLevelRows;
		private int currentLevelColumns;
		private int difficultyIndex; 
		private bool isFirstTouch;
		private CardView firstCardSelected;
		private GameData gameData;
		private GameBoard board;
		private List<CardView> activeCards = new ();
		
		private void Awake()
		{
			gameData = saveProvider.GetGameData();
			menu.OnStartGameButtonClicked += StartGame;
		}

		private void StartGame()
		{
			FindOutDifficulty();
			
			currentLevelIndex = gameData.dificultyList[difficultyIndex].startLevelIndex;
			
			currentLevelColumns = gameData.levels[currentLevelIndex].columns;
			currentLevelRows = gameData.levels[currentLevelIndex].rows;

			board = new GameBoard(currentLevelRows, currentLevelColumns);
			boardView.UpdateBoard(board);
			
			menu.gameObject.SetActive(false);
			boardView.gameObject.SetActive(true);
			
			CreateCards();
		}

		public void GoToNextLevel()
		{
			if (currentLevelIndex >= gameData.dificultyList[difficultyIndex].lastLevelIndex)
			{
				GoToMenu();
			}
			else
			{
				currentLevelIndex++;
			}

			currentLevelColumns = gameData.levels[currentLevelIndex].columns;
			currentLevelRows = gameData.levels[currentLevelIndex].rows;

			board = null;
			board = new GameBoard(currentLevelRows, currentLevelColumns);
			
			boardView.UpdateBoard(board);

			CreateCards();

		}

		private void GoToMenu()
		{
			boardView.gameObject.SetActive(false);
			menu.gameObject.SetActive(true);
		}

		private void FindOutDifficulty()
		{
			float rawDifficulty = menu.GetCurrentRawDifficulty();

			if (rawDifficulty < 0.25f)
				difficultyIndex = 0;
			else if (rawDifficulty < 0.75f)
				difficultyIndex = 1;
			else
				difficultyIndex = 2;
		}
		
		public void CreateCards()
		{
			for (int row = 0; row < board.Rows; row++)
			{
				for (int col = 0; col < board.Columns; col++)
				{
					CreateCard();
				}
			}
		}

		private void CreateCard()
		{
			int currentCardIndex = board.GetCurrentCardIndex();
			Vector2 position = boardView.GetCardPosition(currentCardIndex);
			Card card = board.GetNextCard();

			CardView cardView = cardPool.GetCard();
			cardView.Init(card.Id);
			cardView.OnCardClicked += OnCardClicked;
			cardView.transform.localPosition = position;
					
			float cardWidth, cardHeight;
			boardView.CalculateCardSize(out cardWidth, out cardHeight);

			cardView.GetComponent<RectTransform>().sizeDelta = new Vector2(cardWidth, cardHeight);
			cardView.SetIcon(gameData.icons[card.Id]);
			
			activeCards.Add(cardView);
		}

		private void OnCardClicked(CardView cardView)
		{
			if (firstCardSelected == null)
			{
				firstCardSelected = cardView;
			}
			else
			{
				var secondCardSelected = cardView;
				
				if (firstCardSelected.Index == secondCardSelected.Index 
				    && firstCardSelected != secondCardSelected)
				{
					firstCardSelected.StartRotateToZeroWithCallback(ReturnCard);
					secondCardSelected.StartRotateToZeroWithCallback(ReturnCard);
					
				}
				else
				{
					firstCardSelected.StartWaitAndFlip();
					secondCardSelected.StartWaitAndFlip();
				}
				
				firstCardSelected = null;
			}
		}

		private void ReturnCard(CardView cardView)
		{
			cardView.OnCardClicked -= OnCardClicked;
			cardPool.ReturnToPool(cardView);
			activeCards.Remove(cardView);
			CheckLevelEnd();
		}

		private void CheckLevelEnd()
		{
			if (activeCards.Count == 0)
				GoToNextLevel();
		}

		private void OnDestroy()
		{
			menu.OnStartGameButtonClicked -= StartGame;
		}
	}
}
