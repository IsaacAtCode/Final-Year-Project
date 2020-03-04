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

        private int minPoints = 5;
        private int maxPoints = 40;
        private float maxLength = 6000f;
        private int minStraights = 1;
        private float minHW = 250f;
        private float maxHW = 1500f;

        public List<float> desiredAngles;



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

        public List<Vector2> CombineTrackData(TrackData parent1, TrackData parent2)
        {
            int scale = Mathf.Clamp(Random.Range(parent1.points.Count, parent2.points.Count), minPoints, maxPoints);

            TrackData track1 = gameObject.AddComponent<TrackData>();
            track1.name = "Track 1 Scaled";
            track1.points = GetScaledParent(parent1, scale);

            TrackData track2 = gameObject.AddComponent<TrackData>();
            track2.name = "Track 2 Scaled";
            track2.points = GetScaledParent(parent2, scale);

            //Determine what stats the track should have
            float startDistance = Random.Range(track1.DistanceFromCentre, track2.DistanceFromCentre);
            float startRotation = Random.Range(-179.9f, 180);


            int straights = Random.Range(track1.StraightCount, track2.StraightCount);
            int curves = scale - straights;
            float minDistance = Mathf.Min(track1.MinDistance, track2.MinDistance);
            float maxDistance = Mathf.Max(track1.MaxDistance, track2.MaxDistance);
            float minAngle = Mathf.Clamp(Mathf.Min(track1.MinAngle, track2.MinAngle), 0, 360);
            float maxAngle = Mathf.Clamp(Mathf.Max(track1.MaxAngle, track2.MaxAngle), 0, 360);

            Debug.Log("Distance: " + minDistance + "   " + maxDistance);

            Debug.Log("Angles: " + track1.MinAngle + "   " + track2.MinAngle);


            float angleTotal = (scale - 2) * 180;
            //SUm of exterior angles = 360

            desiredAngles = GetAngles(track1.Angles, track2.Angles);



            //ADD CONSTARINTS


            List<Vector2> newTrackPoints = new List<Vector2>();

            newTrackPoints.Add(NextPoint(Vector2.zero, startDistance, startRotation));

            for (int i = 0; i < scale - 1; i++)
            {
                float testDist = Random.Range(minDistance, maxDistance);
                //float testRot = Random.Range(minAngle, maxAngle);
                float testRot = desiredAngles[i];

                newTrackPoints.Add(NextPoint(newTrackPoints[i], testDist, testRot));
            }

            return newTrackPoints;

            //TrackData newTrack = new TrackData(newTrackPoints);
            //return newTrack;
        }

        private List<float> GetAngles(List<float> parent1, List<float> parent2)
        {
            float maxAngles = (Mathf.Min(parent1.Count, parent2.Count) - 2) * 180;


            List<float> angles = new List<float>();

            for (int i = 0; i < parent1.Count - 1; i++)
            {
                float min = Mathf.Clamp(Mathf.Min(parent1[i], parent2[i]), 0, 360);
                float max = Mathf.Clamp(Mathf.Max(parent1[i], parent2[i]), 0, 360);

                //float newAngle = Random.Range(min, max);
                float newAngle = max;


                angles.Add(newAngle);
            }

            float lastAngle = (maxAngles - GetTotalAngles(angles)) % 360;


            angles.Add(lastAngle);




            return angles;
        }

        float GetTotalAngles(List<float> angles)
        {
            float total = 0f;

            foreach (float angle in angles)
            {
                total += angle;
            }

            return total;
        }




        private Vector2 NextPoint(Vector2 prev, float distance, float rotation)
        {
            float x = prev.x + (distance * Mathf.Cos(rotation));
            float y = prev.y + (distance * Mathf.Sin(rotation));

            return new Vector2(x, y);
        }

        private List<Vector2> GetScaledParent(TrackData parent, int desiredCount)
        {
            if (parent.points.Count == desiredCount)
            {
                return parent.points;
            }
            else
            {
                return parent.ScaledPoints(desiredCount);
            }
        }




    }
}
