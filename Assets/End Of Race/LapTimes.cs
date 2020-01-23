using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LapTimes : MonoBehaviour
{
	public GameObject lapTemplate;

	public ScrollRect scrollView;
	public GameObject contentParent;

	public LapTemplate fastestLap;
	public LapTemplate totalLap;

	public List<float> lapTimes;

	private void Start()
	{
		PopulateScrollView();

		SetFastestLap();

		TotalLapTime();
	}


	public void PopulateScrollView()
	{
		float tempHeight = lapTemplate.GetComponent<RectTransform>().rect.height;
		float totalHeight = (lapTimes.Count * tempHeight);

		//Debug.Log(totalHeight);

		RectTransform contentRect = contentParent.GetComponent<RectTransform>();
		Vector2 newSize = new Vector2(contentRect.rect.x, totalHeight);
		contentRect.sizeDelta = newSize;




		for (int i = 0; i < lapTimes.Count; i++)
		{
			GameObject newLap = Instantiate(lapTemplate, contentParent.transform);

			LapTemplate newLapInfo = newLap.GetComponent<LapTemplate>();
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
