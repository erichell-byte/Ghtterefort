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
		
		private bool backIsActive;
		private bool isFlipped;
		public int timer;
		
		public int Index; 
		public event Action<CardView> OnCardClicked;
		
		
		public void Init(int index)
		{
			Index = index;
			transform.localEulerAngles = new Vector3(0, 0 ,0);
			transform.localScale = new Vector3(1, 1, 1);
			back.SetActive(true);
			backIsActive = true;
			isFlipped = false;
		}
		
		#region Flip
		
		private void StartFlip()
		{
			StartCoroutine(CalculateFlip());
		}

		public void StartWaitAndFlip()
		{
			StartCoroutine(WaitAndFlip());
		}

		private IEnumerator WaitAndFlip()
		{
			yield return new WaitForSeconds(0.5f);

			StartCoroutine(CalculateFlip());
		}
		
		private void Flip()
		{
			back.SetActive(!backIsActive);
			backIsActive = !backIsActive;
		}

		private IEnumerator CalculateFlip()
		{
			isFlipped = !isFlipped;
			for (int i = 0; i < 180; i++)
			{
				yield return new WaitForSeconds(0.001f);
				transform.Rotate(new Vector3(0,1,0));
				timer++;

				if (timer == 90 || timer == -90)
				{
					Flip();
				}
			}

			timer = 0;
		}
		
		#endregion

		public void StartRotateToZeroWithCallback(Action<CardView> callback)
		{
			StartCoroutine(RotateToZero(callback));
		}
		
		private IEnumerator RotateToZero(Action<CardView> callback)
		{
			yield return new WaitForSeconds(0.5f);
			
			float duration = 0.5f;
			float elapsed = 0f;

			while (elapsed < duration)
			{
				elapsed += Time.deltaTime;
				float progress = elapsed / duration;

				transform.Rotate(new Vector3(0,0,3));
				transform.localScale = new Vector3(1 - progress, 1 - progress, 1 - progress);
				
				yield return null;
			}
			callback?.Invoke(this);
		}
		
		public void SetIcon(Sprite sprite)
		{
			icon.sprite = sprite;
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			if (isFlipped) return;
			
			StartFlip();
			OnCardClicked?.Invoke(this);
		}
		
	}
}