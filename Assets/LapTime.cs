using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LapTime : MonoBehaviour
{
    public int lapNumber;
    public float lapTime;



    public Text lapNumberText;
    public Text lapTimeText;

    private void Start()
    {
        lapNumberText = gameObject.transform.GetChild(0).GetComponent<Text>();
        lapTimeText = gameObject.transform.GetChild(1).GetComponent<Text>();

        PopulateText();
    }

    public void PopulateText()
    {
        lapNumberText.text = ("Lap " + lapNumber + ": ");
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
