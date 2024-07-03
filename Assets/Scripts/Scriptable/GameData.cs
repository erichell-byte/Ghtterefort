using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardProject
{
	[CreateAssetMenu(fileName = "Levels", menuName = "CardsGame/New levels file")]
	public class GameData : ScriptableObject
	{
		public List<levelSize> levels;
		public List<Sprite> icons;
		public List<dificultyMode> dificultyList;
	
	
		[Serializable]
		public struct levelSize
		{
			public int rows;
			public int columns;
		}

		[Serializable]
		public struct dificultyMode
		{
			public int startLevelIndex;
			public int lastLevelIndex;
		}
	}
}