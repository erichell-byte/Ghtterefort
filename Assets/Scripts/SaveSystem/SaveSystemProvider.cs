using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardProject
{
	public class SaveSystemProvider : MonoBehaviour
	{
		[SerializeField] private GameData gameData;

		private SaveData saveData;
		private ISaver<SaveData> saver;

		private void Awake()
		{
			saver = new JsonSaver();
			saveData = saver.Load();
		}

		public GameData GetGameData()
		{
			return gameData;
		}
		
	}
}