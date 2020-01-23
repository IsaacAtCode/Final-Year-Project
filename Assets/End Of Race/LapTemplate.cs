using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LapTemplate : MonoBehaviour
{
	public int lapNumber;
	public float lapTime;
	public enum LapTimeType { Standard, Fastest, Total}
	public LapTimeType lapType;

	public Text lapNumberText;
	public Text lapTimeText;

	private void Start()
	{
		if (!lapNumberText)
		{
			lapNumberText = gameObject.transform.GetChild(0).GetComponent<Text>();
		}
		if (lapTimeText)
		{
			lapTimeText = gameObject.transform.GetChild(1).GetComponent<Text>();
		}

		PopulateText();
	}

	public void PopulateText()
	{
		if (lapType == LapTimeType.Total)
		{
			lapNumberText.text = ("Total: ");
		}
		else if (lapType == LapTimeType.Fastest)
		{
			lapNumberText.text = ("Fastest Lap: ");

		}
		else
		{
			lapNumberText.text = ("Lap " + lapNumber + ": ");

		}


		lapTimeText.text = FormatLapTime(lapTime);
	}

	public string FormatLapTime(float time)
	{
		int minutes = (int)time / 60;
		int seconds = (int)time % 60;
		float milliseconds = time * 100;
		milliseconds %= 100;

		return string.Format("{0:0}:{1:00}:{2:00}", minutes, seconds, milliseconds);
	}
}
