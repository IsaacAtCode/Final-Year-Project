using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public TrackManager tm;

    public Text speedo;
    public Text lapCount;
    public Text lapTime;
    public Text checkpointBox;

    public void Start()
    {
        tm = GameObject.Find("TrackManager").GetComponent<TrackManager>();
    }

    public void UpdateLapCount()
    {
        lapCount.text = (tm.currentLap + "/" + tm.maxLaps + " Laps");
    }

    public void UpdateCheckpoint()
    {
        checkpointBox.text = (tm.currentCheck + "/" + tm.maxCheck + " Checkpoint");
    }

    public void LapTime()
    {
        //lapTime.text =;
    }


}
