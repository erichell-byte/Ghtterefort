using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardProject
{
	public class SaveProvider : MonoBehaviour
	{
		[SerializeField] private GameData gameData;

		private SaveData saveData;
		private ISaver<SaveData> saver;

		private void Awake()
		{
			saver = new JsonSaver();
			saveData = saver.Load();
		}

		public void SaveGame(SaveData saveData)
		{
			saver.Save(saveData);
		}
		
		public void SaveGameProgress(int currentLevelIndex, GameData.Difficulty currentDifficulty, int scoreFromLastLevel, int score)
		{
			SaveData saveData = new SaveData
			{
				lastLevelIndex = currentLevelIndex,
				lastDifficulty = currentDifficulty,
				lastlevelScore = scoreFromLastLevel
			};

			int currentScore = score;
			
			var scoreMethods = new Dictionary<GameData.Difficulty, (Func<int> GetScore, Action<int> SetScore)>
			{
				{ GameData.Difficulty.Easy, (() => GetBestEasyScore(), score => saveData.easyScore = score) },
				{ GameData.Difficulty.Medium, (() => GetBestMediumScore(), score => saveData.mediumScore = score) },
				{ GameData.Difficulty.Hard, (() => GetBestHardScore(), score => saveData.hardScore = score) }
			};

			foreach (var (difficulty, (getScore, setScore)) in scoreMethods)
			{
				int bestScore = getScore();
				setScore(difficulty == currentDifficulty ? Math.Max(currentScore, bestScore) : bestScore);
			}

			SaveGame(saveData);
		}

		public GameData GetGameData()
		{
			return gameData;
		}
		
		public int GetBestEasyScore()
		{
			saveData = saver.Load();
			return saveData.easyScore;
		}
		
		public int GetBestMediumScore()
		{
			saveData = saver.Load();
			return saveData.mediumScore;
		}
		
		public int GetBestHardScore()
		{
			saveData = saver.Load();
			return saveData.hardScore;
		}

		public int GetLastLevelIndex()
		{
			saveData = saver.Load();
			return saveData.lastLevelIndex;
		}
		
		public int GetLastLevelScore()
		{
			saveData = saver.Load();
			return saveData.lastlevelScore;
		}
		
		public GameData.Difficulty GetLastDifficulty()
		{
			saveData = saver.Load();
			return saveData.lastDifficulty;
		}
	}
}