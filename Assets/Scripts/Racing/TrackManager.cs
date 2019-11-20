using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    [Header("Laps")]
    public int maxLaps = 3;
    public int currentLap = 0;
    //public Lap[] allLaps;
    [Header("Checkpoint")]
    public Checkpoint[] checkpoints;
    public int maxCheck;
    public int currentCheck;

    [Header("Car")]
    public GameObject CarGo;


    public void Start()
    {
        CarGo = GameObject.Find("CarGO");

        FindAllCheckpoints();
    }


    public void FindAllCheckpoints()
    {
        GameObject[] checkpointsGO = GameObject.FindGameObjectsWithTag("Checkpoint");

        checkpoints = new Checkpoint[(checkpointsGO.Length)];
        maxCheck = checkpoints.Length-1;
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

            Debug.Log("Checkpoint: " + currentCheck);
            return;
        }

    }


    public void StartNewLap()
    {
        //Successful Lap
        //Checks you go through ALL checkpoints in order
        if (currentCheck != 0)
        {
            currentLap++;
            currentCheck = 0;
            Debug.Log("Lap: " + (currentLap - 1) + "/" + maxLaps);
        }
        
    }





}

