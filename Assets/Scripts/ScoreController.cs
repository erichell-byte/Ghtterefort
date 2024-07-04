using System;
using UnityEngine;

namespace CardProject
{
	public class ScoreController : MonoBehaviour
	{
		[SerializeField]
		private int valueToAdd;
		[SerializeField]
		private int comboMultiplier;
		[SerializeField]
		private float comboDuration;
		
		private float elapsed;
		private int score;
		public int Score
		{
			get => score;
			set
			{
				score = Math.Max(0, value);
				OnScoreChanged?.Invoke(score);
			}
		}
		
		public bool isComboTime;
		public event Action<float> ComboTimeRemaining;
		public event Action<int> OnScoreChanged;
		public event Action<bool> OnComboStateChanged; 

		private void Update()
		{
			if (isComboTime)
			{
				ComboTimeRemaining?.Invoke(comboDuration - elapsed);
				
				elapsed += Time.deltaTime;
				
				if (elapsed >= comboDuration)
				{
					elapsed = 0f;
					OnComboStateChanged?.Invoke(isComboTime = false);
				}
			}
		}

		public void AddScoreByMatching()
		{
			Score = isComboTime 
				? Score + (valueToAdd * comboMultiplier)
				: Score + valueToAdd;
			
			if (isComboTime)
				elapsed = 0;
			else
				OnComboStateChanged?.Invoke(isComboTime = true);
			
		}
	}
}