using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardProject
{
	public class CardPool : MonoBehaviour
	{
		[SerializeField] private CardView cardPrefab;
		[SerializeField] private Transform parent;
		[SerializeField] private int initialPoolSize = 20;

		private Queue<CardView> pooledCards = new();

		private void Awake()
		{
			InitializePool();
		}

		private void InitializePool()
		{
			for (int i = 0; i < initialPoolSize; i++)
			{
				CardView card = CreateNewCard();
				ReturnToPool(card);
			}
		}

		private CardView CreateNewCard()
		{
			CardView card = Instantiate(cardPrefab, parent);
			card.gameObject.SetActive(false);
			return card;
		}

		public CardView GetCard()
		{
			if (pooledCards.Count == 0)
			{
				var newCard = CreateNewCard();
				newCard.gameObject.SetActive(true);
				return newCard;
			}

			CardView card = pooledCards.Dequeue();
			card.gameObject.SetActive(true);
			return card;
		}

		public void ReturnToPool(CardView card)
		{
			card.gameObject.SetActive(false);
			pooledCards.Enqueue(card);
		}

		public void ReturnAllToPoolAndUnsubscribe(Action<CardView> func)
		{
			CardView[] activeCards = FindObjectsOfType<CardView>();
			foreach (CardView card in activeCards)
			{
				card.OnCardClicked -= func;
				ReturnToPool(card);
			}
		}
	}
}