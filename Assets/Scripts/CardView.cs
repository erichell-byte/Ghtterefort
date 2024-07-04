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
			transform.localEulerAngles = new Vector3(0, 0, 0);
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
			bool flipped = false;
			float duration = 0.5f;
			float elapsedTime = 0f;
			Quaternion startRotation = transform.rotation;
			Quaternion endRotation = startRotation * Quaternion.Euler(0, 180, 0);
    
			while (elapsedTime < duration)
			{
				elapsedTime += Time.deltaTime;
				float t = elapsedTime / duration;
				transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);
				
				if (t >= 0.5f && !flipped)
				{
					
					Flip();
					flipped = true;
				}
        
				yield return null;
			}
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

				transform.Rotate(new Vector3(0, 0, 3));
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