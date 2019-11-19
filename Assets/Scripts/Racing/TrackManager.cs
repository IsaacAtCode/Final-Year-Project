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
    //Checkpoints
    public Checkpoint[] checkpoints;
    public int maxCheck;
    public int currentCheck;

    //Car
    public GameObject CarGo;

    public void Start()
    {
        CarGo = GameObject.Find("CarGO");

        FindAllCheckpoints();
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

    public void FindAllCheckpoints()
    {
        GameObject[] checkpointsGO = GameObject.FindGameObjectsWithTag("Checkpoint");

        //checkpoints.Length == checkpointsGO.Length;
        for (int i = 0; i < checkpointsGO.Length; i++)
        {
            checkpoints[i] = checkpointsGO[i].GetComponent<Checkpoint>();
        }
    }



    public void UpdateCheckpoint(int checkPos)
    {
        if (checkPos == currentCheck +1)
        {
            currentCheck = checkPos;
            Debug.Log(currentCheck);
            return;
        }

    }





}

public class Lap
{
    public float lapTime;
    public int lapNumber;
}

