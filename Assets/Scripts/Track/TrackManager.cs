using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacFagg
{
    public class TrackManager : MonoBehaviour
    {
        public string trackName;
        public int maxLaps;
        public int maxCheckpoints;

        public Track currentTrack;


        public Checkpoint[] checkpoints;

        //track statistics & influences

        public float trackFriction;
        public float kerbFricition;
        public float gravelFriction;

        public void Start()
        {
            FindAllCheckpoints();
        }

        public void FindAllCheckpoints()
        {
            GameObject[] checkpointsGO = GameObject.FindGameObjectsWithTag("Checkpoint");

            checkpoints = new Checkpoint[(checkpointsGO.Length)];
            maxCheckpoints = checkpoints.Length;
            for (int i = 0; i < checkpointsGO.Length; i++)
            {
                checkpoints[i] = checkpointsGO[i].GetComponent<Checkpoint>();
            }
        }
    }
}

