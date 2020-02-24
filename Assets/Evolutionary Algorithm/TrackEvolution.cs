using System;
using System.Collections;
using System.Collections.Generic;
using Random = System.Random;
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
        public float maxLength = 4000f;
        public int minStraights = 1;
        public float minHW = 250f;
        public float maxHW = 1500f;

        private void Start()
        {
            testPopulation = GetComponents<TrackData>();
            List<Vector2> testPoints = CombineTracks(testPopulation[0], testPopulation[1]).points;

            TrackData babyTrack = gameObject.AddComponent<TrackData>();

            babyTrack.points = testPoints;

        }


        public TrackData CombineTracks(TrackData track1, TrackData track2)
        {
            int scale = Mathf.Clamp(Mathf.Max(track1.points.Count, track2.points.Count), minPoints, maxPoints);
            //Changes for new track
            List<Vector2> track1ScaledPoints = track1.ScaledPoints(scale);
            List<Vector2> track2ScaledPoints = track2.ScaledPoints(scale);
            Debug.Log(track1ScaledPoints.Count + " , " + track2ScaledPoints.Count);

            List<Vector2> newTrackPoints = new List<Vector2>();

            for (int i = 0; i < track1ScaledPoints.Count - 1; i++)
            {
                newTrackPoints.Add(AveragePoint(track1ScaledPoints, track2ScaledPoints, i));
            }

            TrackData newTrack = new TrackData(newTrackPoints);
            return newTrack;
        }

        private Vector2 AveragePoint(List<Vector2> points1, List<Vector2> points2, int position)
        {
            float averageDistance = (TrackUtility.GetDistanceFromCentre(points1, position) + TrackUtility.GetDistanceFromCentre(points2, position)) / 2;
            float averageAngle = (TrackUtility.GetAngleFromCentre(points1, position) + TrackUtility.GetAngleFromCentre(points2, position)) / 2;

            float x = averageDistance * Mathf.Cos(averageAngle * Mathf.Deg2Rad);
            float y = averageDistance * Mathf.Sin(averageAngle * Mathf.Deg2Rad);

            Vector2 averagePoint = new Vector2(x, y);

            return averagePoint;
        }
    }
}
