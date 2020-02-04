using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IsaacFagg.Track3;

namespace IsaacFagg.Race
{
    public class RaceManager : MonoBehaviour
    {
        public List<Checkpoint> checkpoints;
        public Vector2 playerSpawn;

        private void Start()
        {
            GetVariables();
        }

        private void GetVariables()
        {
            Track3Generator trackGen = GetComponent<Track3Generator>();

            checkpoints = trackGen.checkpoints;
            playerSpawn = trackGen.checkpointLocations[trackGen.checkpointLocations.Count - 1];

        }



    }
}
