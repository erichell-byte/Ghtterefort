using UnityEngine;

namespace CardProject
{
	public class Game : MonoBehaviour
	{
		[SerializeField]
		private GameData data;
		
		[SerializeField]
		private MenuView menu;
		
		[SerializeField]
		private CardView cardViewPrefab;
		
		[SerializeField]
		private GameBoardView boardView;
		
		private GameBoard board;

		private int currentLevelIndex;
		private int currentLevelRows;
		private int currentLevelColumns;
		private bool isFirstTouch;
		
		private int selectedCardIndex;
		private CardView firstCardSelected;
		
		[Range(0, 2)]
		private int difficultyIndex; 

		private void Awake()
		{
			menu.OnStartGameButtonClicked += StartGame;
		}

		private void StartGame()
		{
			FindOutDifficulty();
			menu.gameObject.SetActive(false);
			
			currentLevelIndex = data.dificulty[difficultyIndex].startLevelIndex;
			
			currentLevelColumns = data.levels[currentLevelIndex].columns;
			currentLevelRows = data.levels[currentLevelIndex].rows;

			board = new GameBoard(currentLevelRows, currentLevelColumns);
			boardView.gameObject.SetActive(true);
			boardView.Init(board);
			
			CreateCards();
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
					DrawCard();
				}
			}
		}

		private void DrawCard()
		{
			int currentCardIndex = board.GetCurrentCardIndex();
			Vector2 position = boardView.GetCardPosition(currentCardIndex);
			Card card = board.GetNextCard();

			CardView cardView = Instantiate(cardViewPrefab, boardView.gameObject.transform);
			cardView.Init(card.Id);
			cardView.OnCardClicked += OnCardClicked;
			cardView.transform.localPosition = position;
					
			float cardWidth, cardHeight;
			boardView.CalculateCardSize(out cardWidth, out cardHeight);

			cardView.GetComponent<RectTransform>().sizeDelta = new Vector2(cardWidth, cardHeight);
			cardView.SetIcon(data.icons[card.Id]);
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
					firstCardSelected.StartWaitAndDestroy();
					secondCardSelected.StartWaitAndDestroy();
				}
				else
				{
					firstCardSelected.StartWaitAndFlip();
					secondCardSelected.StartWaitAndFlip();
				}
				firstCardSelected = null;
				
			}
		}

		private void OnDestroy()
		{
			menu.OnStartGameButtonClicked -= StartGame;
		}
	}
}
