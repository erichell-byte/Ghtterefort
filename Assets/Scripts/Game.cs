
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CardProject
{
	public class Game : MonoBehaviour
	{
		[SerializeField] private SaveProvider saveProvider;
		[SerializeField] private MenuView menu;
		[SerializeField] private CardPool cardPool;
		[SerializeField] private GameBoardView boardView;
		[SerializeField] private ScoreController scoreController;
		[SerializeField] private SoundController soundController;
		
		private int currentLevelIndex;
		private int currentLevelRows;
		private int currentLevelColumns;
		private GameData.Difficulty currentDifficulty;
		private int  scoreFromLastLevel; 
		private bool isFirstTouch;
		private CardView firstCardSelected;
		private GameData gameData;
		private GameBoard board;

		private void Start()
		{
			menu.OnStartGameButtonClicked += StartNewGame;
			menu.OnContinueGameButtonClicked += ContinueGame;
			scoreController.OnScoreChanged += boardView.UpdateScore;
			scoreController.ComboTimeRemaining += boardView.UpdateComboTimeText;
			scoreController.OnComboStateChanged += boardView.UpdateComboState;
			boardView.OnMenuButtonClicked += GoToMenu;

			InitSavedData();
		}

		private void InitSavedData()
		{
			gameData = saveProvider.GetGameData();
			
			menu.UpdateScoresAtMode(
				saveProvider.GetBestEasyScore(),
				saveProvider.GetBestMediumScore(),
				saveProvider.GetBestHardScore());
			
			if (saveProvider.GetLastLevelIndex() == -1)
				menu.ChangeContinueButtonState(false);
		}
		
		private void SaveGameProgress()
		{
			SaveData saveData = new SaveData
			{
				lastLevelIndex = currentLevelIndex,
				lastDifficulty = currentDifficulty,
				lastlevelScore = scoreFromLastLevel
			};

			int currentScore = scoreController.Score;
			
			var scoreMethods = new Dictionary<GameData.Difficulty, (Func<int> GetScore, Action<int> SetScore)>
			{
				{ GameData.Difficulty.Easy, (() => saveProvider.GetBestEasyScore(), score => saveData.easyScore = score) },
				{ GameData.Difficulty.Medium, (() => saveProvider.GetBestMediumScore(), score => saveData.mediumScore = score) },
				{ GameData.Difficulty.Hard, (() => saveProvider.GetBestHardScore(), score => saveData.hardScore = score) }
			};

			foreach (var difficulty in scoreMethods.Keys)
			{
				var (getScore, setScore) = scoreMethods[difficulty];
				int bestScore = getScore();

				if (difficulty == currentDifficulty)
					setScore(Math.Max(currentScore, bestScore));
				else
					setScore(bestScore);
			}

			saveProvider.SaveGame(saveData);
		}

		private void ContinueGame()
		{
			scoreController.Score = saveProvider.GetLastLevelScore();
			currentLevelIndex = saveProvider.GetLastLevelIndex();
			
			LaunchGame();
		}

		private void StartNewGame()
		{
			FindOutDifficulty();
			scoreController.Score = 0;
			
			currentLevelIndex = gameData.difficultyList
				.FirstOrDefault(item => item.difficulty == currentDifficulty)
				.mode.startLevelIndex;

			LaunchGame();
		}

		private void LaunchGame()
		{
			currentLevelColumns = gameData.levels[currentLevelIndex].columns;
			currentLevelRows = gameData.levels[currentLevelIndex].rows;
			
			board = null;
			board = new GameBoard(currentLevelRows, currentLevelColumns);
			boardView.UpdateBoard(board);
			
			menu.gameObject.SetActive(false);
			boardView.gameObject.SetActive(true);
			
			CreateCards();
		}

		private void GoToNextLevel()
		{
			scoreFromLastLevel = scoreController.Score;
				
			if (currentLevelIndex >= gameData.difficultyList
				    .FirstOrDefault(item => item.difficulty == currentDifficulty)
				    .mode.lastLevelIndex)
				GoToMenu();
			else
				currentLevelIndex++;
			
			LaunchGame();
			
			SaveGameProgress();
			soundController.PlaySound(SoundController.SoundsType.GameOver, 0.7f);
		}
		
		private void GoToMenu()
		{
			SaveGameProgress();
			
			menu.UpdateScoresAtMode(
				saveProvider.GetBestEasyScore(),
				saveProvider.GetBestMediumScore(),
				saveProvider.GetBestHardScore());
			
			ResetGame();
			
			boardView.gameObject.SetActive(false);
			menu.gameObject.SetActive(true);
		}

		private void FindOutDifficulty()
		{
			float rawDifficulty = menu.GetCurrentRawDifficulty();

			if (rawDifficulty < 0.25f)
				currentDifficulty = GameData.Difficulty.Easy;
			else if (rawDifficulty < 0.75f)
				currentDifficulty = GameData.Difficulty.Medium;
			else
				currentDifficulty = GameData.Difficulty.Hard;
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
		}

		private void OnCardClicked(CardView cardView)
		{
			soundController.PlaySound(SoundController.SoundsType.Flipping, 0.5f);
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
					scoreController.AddScoreByMatching();
					soundController.PlaySound(SoundController.SoundsType.Matching, 0.7f);
				}
				else
				{
					firstCardSelected.StartWaitAndFlip();
					secondCardSelected.StartWaitAndFlip();
					soundController.PlaySound(SoundController.SoundsType.Mismatching, 0.5f);
				}
				
				firstCardSelected = null;
			}
		}

		private void ResetGame()
		{
			cardPool.ReturnAllToPoolAndUnsubscribe(OnCardClicked);
		}

		private void ReturnCard(CardView cardView)
		{
			cardView.OnCardClicked -= OnCardClicked;
			cardPool.ReturnToPool(cardView);
			board.RemoveCard(cardView.Index);
			CheckLevelEnd();
		}

		private void CheckLevelEnd()
		{
			if (board.GetCardsCount() == 0)
				GoToNextLevel();
		}

		private void OnDestroy()
		{
			menu.OnStartGameButtonClicked -= StartNewGame;
			menu.OnContinueGameButtonClicked -= ContinueGame;
			scoreController.OnScoreChanged -= boardView.UpdateScore;
			scoreController.ComboTimeRemaining -= boardView.UpdateComboTimeText;
			scoreController.OnComboStateChanged -= boardView.UpdateComboState;
			boardView.OnMenuButtonClicked -= GoToMenu;
		}
	}
}
