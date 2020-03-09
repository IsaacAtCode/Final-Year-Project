using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IsaacFagg.Player
{
	public class StarRating : MonoBehaviour
	{
		public int Rating
		{
			get
			{
				return GenerateScore();
			}
		}

		[Header("Toggles")]
		public Toggle star1;
		public Toggle star2;
		public Toggle star3;
		public Toggle star4;
		public Toggle star5;

		public ToggleGroup starGroup;
		private Toggle[] stars;

		[Header("Images")]
		public Image image1;
		public Image image2;
		public Image image3;
		public Image image4;

		private Image[] images;

		private void Start()
		{
			starGroup = GetComponent<ToggleGroup>();

			stars = new[] { star1, star2, star3, star4, star5 };
			images = new[] { image1, image2, image3, image4 };

			HideAllImages();
		}


		public int GenerateScore()
		{
			int number = 0;

			for (int i = 0; i < stars.Length; i++)
			{
				if (stars[i].isOn)
				{
					number = i + 1;
				}
			}

			return number;
		}

		private void HideAllImages()
		{
			foreach (Image image in images)
			{
				image.enabled = false;
			}
		}

		public void ShowImages(int starPos)
		{
			HideAllImages();

			if (stars[starPos - 1].isOn)
			{
				for (int i = 0; i < images.Length; i++)
				{
					if (i < starPos)
					{
						images[i].enabled = true;
					}
				}
			}

		}

	}
}
