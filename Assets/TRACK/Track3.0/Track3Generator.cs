// Made by Isaac Fagg
// Final Year Project
// 30/01/2020

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacFagg.Track3
{
    [ExecuteInEditMode]
    public class Track3Generator : MonoBehaviour
    {
        public Track3 track;
        public RandomNameGenerator randomNameGenerator;

        public bool generateNewTrack = false;

        private void Start()
        {
            if (!track)
            {
                track = GetComponent<Track3>();
            }
            if (!randomNameGenerator)
            {
                randomNameGenerator = GetComponent<RandomNameGenerator>();
            }
        }

        private void Update()
        {
            if (generateNewTrack)
            {
                track.GenerateTrack();
                if (track.name == null || track.name == "")
                {
                    track.name = randomNameGenerator.GenerateName();
                }
                generateNewTrack = false;
            }
        }

        private void GenerateFromTrack()
        {

        }



    }
}
