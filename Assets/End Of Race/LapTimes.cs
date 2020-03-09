using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace IsaacFagg.Player
{
	public class LapTimes : MonoBehaviour
	{
		public GameObject lapGO;

		public ScrollRect scrollView;
		public GameObject contentParent;

		public LapText fastestLap;
		public LapText totalLap;

		public List<float> lapTimes;


		private void Start()
		{
			PopulateScrollView();

			SetFastestLap();

			TotalLapTime();
		}


		public void PopulateScrollView()
		{
			float tempHeight = lapGO.GetComponent<RectTransform>().rect.height;
			float totalHeight = (lapTimes.Count * tempHeight);

			//Debug.Log(totalHeight);

			RectTransform contentRect = contentParent.GetComponent<RectTransform>();
			Vector2 newSize = new Vector2(contentRect.rect.x, totalHeight);
			contentRect.sizeDelta = newSize;




			for (int i = 0; i < lapTimes.Count; i++)
			{
				GameObject newLap = Instantiate(lapGO, contentParent.transform);

				LapText newLapInfo = newLap.GetComponent<LapText>();
				newLapInfo.lapTime = lapTimes[i];
				newLapInfo.lapNumber = i + 1;
				newLapInfo.PopulateText();
			}

		}

		public void SetFastestLap()
		{
			fastestLap.lapTime = lapTimes.Min();
			fastestLap.PopulateText();
		}

		public void TotalLapTime()
		{
			float total = 0;

			foreach (float lap in lapTimes)
			{
				total += lap;
			}

			totalLap.lapTime = total;

			totalLap.PopulateText();
		}

	}







}

