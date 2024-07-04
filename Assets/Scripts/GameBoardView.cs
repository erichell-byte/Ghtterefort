using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CardProject
{
	public class GameBoardView : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI scoreText;
		[SerializeField] private TextMeshProUGUI comboTimeText;
		[SerializeField] private GameObject comboGO;
		[SerializeField] private Button goToMenuButton;
		
		private RectTransform gameBoardRect;
		private GameBoard board;

		public float Padding;
		public event UnityAction OnMenuButtonClicked;

		private void Awake()
		{
			gameBoardRect = GetComponent<RectTransform>();
			goToMenuButton.onClick.AddListener(OnMenuButtonClicked);
		}

		public void UpdateBoard(GameBoard board)
		{
			this.board = board;
		}

		public void UpdateComboTimeText(float value)
		{
			var roundedValue = Math.Round(value, 3);
			comboTimeText.text = roundedValue.ToString();
		}

		public void UpdateComboState(bool isActive)
		{
			comboGO.SetActive(isActive);
		}

		public void UpdateScore(int score)
		{
			scoreText.text = score.ToString();
		}

		public void CalculateCardSize(out float cardWidth, out float cardHeight)
		{
			float boardWidth = gameBoardRect.rect.width;
			float boardHeight = gameBoardRect.rect.height;

			float maxCardWidth = (boardWidth - (board.Columns + 1) * Padding) / board.Columns;
			float maxCardHeight = (boardHeight - (board.Rows + 1) * Padding) / board.Rows;
			
			if (maxCardWidth / 2 * 3 <= maxCardHeight)
			{
				cardWidth = maxCardWidth;
				cardHeight = cardWidth / 2 * 3;
			}
			else
			{
				cardHeight = maxCardHeight;
				cardWidth = cardHeight * 2 / 3;
			}
		}

		public Vector3 GetCardPosition(int index)
		{
			float cardWidth, cardHeight;
			CalculateCardSize(out cardWidth, out cardHeight);

			int row = index / board.Columns;
			int col = index % board.Columns;

			float x = (col + 0.5f) * (cardWidth + Padding) - (board.Columns * (cardWidth + Padding)) / 2 + Padding / 2;
			float y = -(row + 0.5f) * (cardHeight + Padding) + (board.Rows * (cardHeight + Padding)) / 2 - Padding / 2;

			return new Vector3(x, y, 0);
		}

		private void OnDestroy()
		{
			goToMenuButton.onClick.RemoveListener(OnMenuButtonClicked);
		}
	}
}