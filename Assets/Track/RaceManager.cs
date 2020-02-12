using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IsaacFagg.Player;
using IsaacFagg.Track3;
using IsaacFagg.Utility;

namespace IsaacFagg.Race
{
    public class RaceManager : MonoBehaviour
    {

        public GameObject carPrefab;

        public List<Checkpoint> checkpoints;
        public Vector2 playerSpawn;
        public Quaternion playerRot;


        private void Start()
        {

            GetVariables();

            //SpawnCar();
        }

        private void GetVariables()
        {
            Track3Generator trackGen = GetComponent<Track3Generator>();

            checkpoints = trackGen.checkpoints;
            playerSpawn = trackGen.checkpointLocations[trackGen.checkpointLocations.Count - 1];


            Vector3 targetPos = trackGen.checkpointLocations[0];


            playerRot = MathsUtility.LookAt(playerSpawn, targetPos);
        }


        private void SpawnCar()
        {
            //Get line gradient, spawn parallel


            GameObject car = Instantiate(carPrefab, playerSpawn, Quaternion.identity);

            car.transform.rotation = playerRot;

        }

    }

    public class Lap
    {
        float time;

    }

    public class Split
    {
        int position;
        float time;
    }

}
