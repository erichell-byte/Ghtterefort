using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardProject
{
    public class MenuView : MonoBehaviour
    {
        [SerializeField] private Scrollbar dificultyScrollbar;
        [SerializeField] private Button startGameButton;
        [SerializeField] private Button continueGameButton;
        [SerializeField] private TextMeshProUGUI easyScoreText;
        [SerializeField] private TextMeshProUGUI mediumScoreText;
        [SerializeField] private TextMeshProUGUI hardScoreText;

        public event Action OnStartGameButtonClicked;
        public event Action OnContinueGameButtonClicked;

        private void Awake()
        {
            startGameButton.onClick.AddListener(StartGameClicked);
            continueGameButton.onClick.AddListener(ContinueGameClicked);
        }

        private void StartGameClicked()
        {
            OnStartGameButtonClicked?.Invoke();
        }

        private void ContinueGameClicked()
        {
            OnContinueGameButtonClicked?.Invoke();
        }

        public void ChangeContinueButtonState(bool isActive)
        {
            continueGameButton.interactable = isActive;
        }

        public float GetCurrentRawDifficulty()
        {
            return dificultyScrollbar.value;
        }

        public void UpdateScoresAtMode(int easy, int medium, int hard)
        {
            easyScoreText.text = easy.ToString();
            mediumScoreText.text = medium.ToString();
            hardScoreText.text = hard.ToString();
        }

        private void OnDestroy()
        {
            startGameButton.onClick.RemoveListener(StartGameClicked);
            continueGameButton.onClick.RemoveListener(ContinueGameClicked);
        }
    }
}