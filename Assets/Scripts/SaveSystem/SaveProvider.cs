
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

		public GameData.Difficulty GetLastDifficulty()
		{
			saveData = saver.Load();
			return saveData.lastDifficulty;
		}

		public int GetLastLevelScore()
		{
			saveData = saver.Load();
			return saveData.lastlevelScore;
		}

		public void SaveProgress()
		{
			
		}
		
	}
}