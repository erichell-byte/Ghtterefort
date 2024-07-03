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
        [SerializeField] private TextMeshProUGUI easyScore;
        [SerializeField] private TextMeshProUGUI mediumScore;
        [SerializeField] private TextMeshProUGUI hardScore;
        
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

        private void OnDestroy()
        {
            startGameButton.onClick.RemoveListener(StartGameClicked);
            continueGameButton.onClick.RemoveListener(ContinueGameClicked);
        }
    }
}