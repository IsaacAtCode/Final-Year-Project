using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    //Laps
    public int lapCount;
    public int maxLaps = 3;
    public int currentLap = 0;
    public Lap[] allLaps;
    public int checkpointNum;

    //Car
    public GameObject CarGo;

    public void Start()
    {
        CarGo = GameObject.Find("CarGO");
    }


    public void StartNewLap()
    {

    }


    public void FinishLap()
    {
        //When Lap is finished, store lap statistics
        //Then start a new Lap
        
        //newLap = new Lap(0.00f, 1);
    }





}

public class Lap
{
    public float lapTime;
    public int lapNumber;
}

public class Checkpoint
{
    public int position;
}
