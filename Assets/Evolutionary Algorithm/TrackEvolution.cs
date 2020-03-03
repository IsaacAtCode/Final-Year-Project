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
            track1.name = "Track 1";
            track1.points = RandomTrackGenerator.GenerateRandomTrack().points;


            TrackData track2 = gameObject.AddComponent<TrackData>();
            track2.name = "Track 2";
            track2.points = RandomTrackGenerator.GenerateRandomTrack().points;


            TrackData babyTrack = gameObject.AddComponent<TrackData>();
            babyTrack.name = "Baby Outside Track";
            babyTrack.points = CombineTrackData(track1, track2);



        }

        //public TrackData CombineTrackData(TrackData track1, TrackData track2)

        public List<Vector2> CombineTrackData(TrackData track1, TrackData track2)
        {
            int scale = Mathf.Clamp(Random.Range(track1.points.Count, track2.points.Count), minPoints, maxPoints);

            //Changes for new track
            List<Vector2> track1ScaledPoints = new List<Vector2>();
            List<Vector2> track2ScaledPoints = new List<Vector2>();

            if (track1.points.Count == scale)
            {
                track1ScaledPoints = track1.points;
            }
            else
            {
                track1ScaledPoints = track1.ScaledPoints(scale);
            }
            if (track2.points.Count == scale)
            {
                track2ScaledPoints = track2.points;
            }
            else
            {
                track2ScaledPoints = track2.ScaledPoints(scale);
            }

            TrackData track1Scaled = gameObject.AddComponent<TrackData>();
            track1Scaled.name = "Track 1 Scaled";
            track1Scaled.points = track1ScaledPoints;

            TrackData track2Scaled = gameObject.AddComponent<TrackData>();
            track2Scaled.name = "Track 2 Scaled";
            track2Scaled.points = track2ScaledPoints;

            //Determine what stats the track should have
            float startDistance = Random.Range(track1.DistanceFromCentre, track2.DistanceFromCentre);
            float startRotation = Random.Range(-179.9f, 180);


            int straights = Random.Range(track1.StraightCount, track2.StraightCount);
            int curves = scale - straights;
            float minDistance = Mathf.Min(track1.MinDistance, track2.MinDistance);
            float maxDistance = Mathf.Max(track1.MaxDistance, track2.MaxDistance);
            float minAngle = Mathf.Min(track1.MinAngle, track2.MinAngle);
            float maxAngle = Mathf.Max(track1.MaxAngle, track2.MaxAngle);

            float angleTotal = (scale - 2) * 180;
            //SUm of exterior angles = 360

            //ADD CONSTARINTS


            List<Vector2> newTrackPoints = new List<Vector2>();

            newTrackPoints.Add(NextPoint(Vector2.zero, startDistance, startRotation));

            for (int i = 0; i < scale; i++)
            {
                float testDist = Random.Range(minDistance, maxDistance);
                float testRot = Random.Range(minAngle, maxAngle);

                newTrackPoints.Add(NextPoint(newTrackPoints[i], testDist, testRot));
            }

            return newTrackPoints;

            //TrackData newTrack = new TrackData(newTrackPoints);
            //return newTrack;
        }

        private Vector2 NextPoint(Vector2 prev, float distance, float rotation)
        {
            float x = prev.x + (distance * Mathf.Cos(rotation));
            float y = prev.y + (distance * Mathf.Sin(rotation));

            return new Vector2(x, y);
        }






    }
}
