using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IsaacFagg.Track;
using IsaacFagg.Utility;

namespace IsaacFagg.Genetics
{
    public class TrackEvolution : MonoBehaviour
    {
        public TrackData[] testPopulation;
        public List<Vector2> testPoints = new List<Vector2>();

        //public List<TrackDNA> Population { get; private set; }
        //public int Generation { get; private set; }
        //public float BestFitness { get; private set; }
        //public TrackData BestGenes { get; private set; }

        //public int Elitism;
        //public float MutationRate;

        //private List<TrackDNA> newPopulation;
        //private Random random;
        //private float fitnessSum;
        //private int dnaSize;
        //private Func<Vector2> getRandomGene;
        //private Func<int, float> fitnessFunction;

        public int minPoints = 5;
        public int maxPoints = 40;
        public float maxLength = 6000f;
        public int minStraights = 1;
        public float minHW = 250f;
        public float maxHW = 1500f;

        private void Start()
        {
            TrackData track1 = gameObject.AddComponent<TrackData>();
            track1.points = RandomTrackGenerator.GenerateRandomTrack().points;


            TrackData track2 = gameObject.AddComponent<TrackData>();
            track2.points = RandomTrackGenerator.GenerateRandomTrack().points;

            testPopulation = GetComponents<TrackData>();

            List<Vector2> testPoints = CombineTracks(testPopulation[0], testPopulation[1]).points;

            TrackData babyTrack = gameObject.AddComponent<TrackData>();
            babyTrack.points = testPoints;
        }


        public TrackData CombineTracks(TrackData track1, TrackData track2)
        {
            int scale = Mathf.Clamp(Mathf.Max(track1.points.Count, track2.points.Count), minPoints, maxPoints);

            Debug.Log(scale);
            //Changes for new track
            List<Vector2> track1ScaledPoints = track1.ScaledPoints(scale);
            List<Vector2> track2ScaledPoints = track2.ScaledPoints(scale);
            Debug.Log(track1ScaledPoints.Count + " , " + track2ScaledPoints.Count);

            //New Tracks Statistics
            //int straight1 = TrackUtility.GetStraights(track1ScaledPoints);
            //int straight2 = TrackUtility.GetStraights(track2ScaledPoints);
            //int wantedStraights = Random.Range(Mathf.Min(straight1, straight2), Mathf.Max(straight1, straight2));

            //int curve1 = TrackUtility.GetCurves(track1ScaledPoints);
            //int curve2 = TrackUtility.GetCurves(track2ScaledPoints);
            //int wantedCurves = scale - wantedStraights;






            List<Vector2> newTrackPoints = new List<Vector2>();



            TrackData newTrack = new TrackData(newTrackPoints);
            return newTrack;
        }

    }
}
