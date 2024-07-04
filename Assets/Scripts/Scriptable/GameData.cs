using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardProject
{
	[CreateAssetMenu(fileName = "Levels", menuName = "CardsGame/New levels file")]
	public class GameData : ScriptableObject
	{
		public List<LevelInfo> levels;
		public List<Sprite> icons;
		public List<DifficultyDictItem> difficultyList;

		[Serializable]
		public struct LevelInfo
		{
			public int rows;
			public int columns;
		}

		[Serializable]
		public struct DifficultyMode
		{
			public int startLevelIndex;
			public int lastLevelIndex;
		}

		[Serializable]
		public enum Difficulty
		{
			Easy,
			Medium,
			Hard
		}

		[Serializable]
		public struct DifficultyDictItem // Новая структура для представления пары ключ-значение
		{
			public Difficulty difficulty;
			public DifficultyMode mode;
		}
	}
}