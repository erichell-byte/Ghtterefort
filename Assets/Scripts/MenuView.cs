using System;
using UnityEngine;
using UnityEngine.UI;

namespace CardProject
{
    public class MenuView : MonoBehaviour
    {
        [SerializeField]
        private Scrollbar dificultyScrollbar;

        [SerializeField]
        private Button startGameButton;
    
        public event Action OnStartGameButtonClicked;

        private void Awake()
        {
            startGameButton.onClick.AddListener(StartGameClicked);
        }

        private void StartGameClicked()
        {
            OnStartGameButtonClicked?.Invoke();
        }

        public float GetCurrentRawDifficulty()
        {
            return dificultyScrollbar.value;
        }

        private void OnDestroy()
        {
            startGameButton.onClick.RemoveListener(StartGameClicked);
        }
    }
}