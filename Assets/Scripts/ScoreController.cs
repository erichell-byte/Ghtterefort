using System;
using UnityEngine;

namespace CardProject
{
	public class ScoreController : MonoBehaviour
	{
		private int score;
		private float comboDuration;
		private float elapsed;

		public bool isComboTime;
		public event Action<float> ComboTimeRemaining;

		private void Update()
		{
			if (isComboTime)
			{
				ComboTimeRemaining?.Invoke(comboDuration - elapsed);
				
				elapsed += Time.deltaTime;
				
				if (elapsed >= comboDuration)
				{
					isComboTime = false;
					elapsed = 0f;
				}
					
			}
		}

		public void AddScore(int valueToAdd)
		{
			score += valueToAdd;
		}

		public void ReplaceScore(int newValue)
		{
			score = newValue;
		}
	}
}