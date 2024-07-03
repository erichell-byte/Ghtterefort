using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CardProject
{
	public class CardView : MonoBehaviour, IPointerClickHandler
	{
		[SerializeField]
		private Image icon;
		[SerializeField]
		private GameObject back;

		private float y = 1;
		private bool backIsActive;
		public int timer;

		public int Index;

		public event Action<CardView> OnCardClicked;
		public bool IsFlipped { get; set; }
		

		public void Init(int index)
		{
			Index = index;
			backIsActive = true;
		}
		
		#region Flip
		
		public void StartFlip()
		{
			StartCoroutine(CalculateFlip());
		}

		public void StartWaitAndFlip()
		{
			StartCoroutine(WaitAndFlip());
		}

		public void StartWaitAndDestroy()
		{
			StartCoroutine(WaitAndDestroy());
		}

		public IEnumerator WaitAndFlip()
		{
			yield return new WaitForSeconds(1f);

			StartCoroutine(CalculateFlip());
		}
		
		public IEnumerator WaitAndDestroy()
		{
			yield return new WaitForSeconds(1f);

			Destroy(gameObject);
		}
		

		public void Flip()
		{
			if (backIsActive)
			{
				back.SetActive(false);
				backIsActive = false;
			}
			else
			{
				back.SetActive(true);
				backIsActive = true;
				IsFlipped = true;
			}
		}

		private IEnumerator CalculateFlip()
		{
			
			for (int i = 0; i < 180; i++)
			{
				yield return new WaitForSeconds(0.001f);
				transform.Rotate(new Vector3(0,y,0));
				timer++;

				if (timer == 90 || timer == -90)
				{
					Flip();
				}
			}

			timer = 0;
		}
		
		#endregion
		
		public void SetIcon(Sprite sprite)
		{
			icon.sprite = sprite;
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			StartFlip();
			OnCardClicked?.Invoke(this);
		}
	}
}