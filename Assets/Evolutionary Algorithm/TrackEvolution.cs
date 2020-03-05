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
        private int maxPoints = 25;
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
            babyTrack.name = "Baby Track";
            babyTrack.points = CombineTrackData(track1, track2);



        }

        //public TrackData CombineTrackData(TrackData track1, TrackData track2)

        public List<Vector2> CombineTrackData(TrackData parent1, TrackData parent2)
        {
            //Get New Point Count
            int scale = Mathf.Clamp(Random.Range(parent1.points.Count, parent2.points.Count), minPoints, maxPoints);
            
            //Get Rotation From Parents
            Rotation rot;
            if (parent1.rotation == parent2.rotation)
            {
                rot = parent1.rotation;
            }
            else
            {
                rot = TrackUtility.RandomRotation();
            }


            TrackData track1 = gameObject.AddComponent<TrackData>();
            track1.name = "Track 1 Scaled";
            track1.points = NormaliseParent(parent1, scale, rot);

            TrackData track2 = gameObject.AddComponent<TrackData>();
            track2.name = "Track 2 Scaled";
            track2.points = NormaliseParent(parent2, scale, rot);

            //Determine what stats the track should have
            float startDistance = Random.Range(track1.DistanceFromCentre, track2.DistanceFromCentre);
            float startRotation = Random.Range(-179.9f, 180);


            int straights = Random.Range(track1.StraightCount, track2.StraightCount);
            int curves = scale - straights;
            float minDistance = Mathf.Min(track1.MinDistance, track2.MinDistance);
            float maxDistance = Mathf.Max(track1.MaxDistance, track2.MaxDistance);
            float minAngle = Mathf.Clamp(Mathf.Min(track1.MinAngle, track2.MinAngle), 0, 360);
            float maxAngle = Mathf.Clamp(Mathf.Max(track1.MaxAngle, track2.MaxAngle), 0, 360);

            float angleTotal = (scale - 2) * 180;
            //SUm of exterior angles = 360

            desiredAngles = GetAngles(track1.Angles, track2.Angles);



            //ADD CONSTARINTS


            List<Vector2> radTrack = new List<Vector2>();

            radTrack.Add(NextPoint(Vector2.zero, startDistance, startRotation* Mathf.Deg2Rad));


            for (int i = 0; i < scale - 1; i++)
            {
                float testDist = Random.Range(minDistance, maxDistance);
                //float testRot = Random.Range(minAngle, maxAngle);
                float testRot = desiredAngles[i];

                radTrack.Add(NextPoint(radTrack[i], testDist, testRot * Mathf.Deg2Rad));
            }

            return radTrack;

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

        private List<Vector2> NormaliseParent(TrackData parent, int scale, Rotation rot)
        {
            List<Vector2> tempParent = new List<Vector2>(parent.points);

            tempParent = GetScaledParent(tempParent, scale);
            tempParent = GetCentredParent(tempParent);
            tempParent = GetReversedParent(tempParent, rot);

            return tempParent;
        }

        private List<Vector2> GetScaledParent(List<Vector2> parent, int newCount)
        {
            if (parent.Count != newCount)
            {
                return TrackUtility.ScaledPoints(parent, newCount);
            }
            else
            {
                return parent;
            }
        }

        private List<Vector2> GetReversedParent(List<Vector2> parent, Rotation rot)
        {
            if (TrackUtility.GetRotation(parent) != rot)
            {
                return TrackUtility.ReversePoints(parent);
            }
            else
            {
                return parent;
            }
        }

        private List<Vector2> GetCentredParent(List<Vector2> parent)
        {
            if (TrackUtility.GetCentre(parent) != Vector2.zero)
            {
                return TrackUtility.CentredPoints(parent);
            }
            else
            {
                return parent;
            }
        }

        //private List<Vector2> GetScaledParent(TrackData parent, int newCount)
        //{
        //    if (parent.points.Count != newCount)
        //    {
        //        return TrackUtility.ScaledPoints(parent.points, newCount);
        //    }
        //    else
        //    {
        //        return parent.points;

        //    }
        //}

        //private List<Vector2> GetReversedParents(TrackData parent, Rotation rot)
        //{
        //    if (parent.rotation != rot)
        //    {
        //        return TrackUtility.ReversePoints(parent.points);
        //    }
        //    else
        //    {
        //        return parent.points;
        //    }
        //}

        //private List<Vector2> GetCentredParent(TrackData parent)
        //{
        //    if (parent.Centre != Vector2.zero)
        //    {
        //        return TrackUtility.CentredPoints(parent.points);
        //    }
        //    else
        //    {
        //        return parent.points;
        //    }
        //}




    }
}
