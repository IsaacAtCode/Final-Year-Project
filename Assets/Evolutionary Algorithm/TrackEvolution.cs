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

            testPopulation = GetComponents<TrackData>();

            List<Vector2> testPoints = CombineTrackData(testPopulation[0], testPopulation[1]).points;

        }


        public TrackData CombineTrackData(TrackData track1, TrackData track2)
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

            //for (int i = 0; i < scale - 1; i++)
            //{
            //    float firstAngle = 0;
            //    float secondAngle = 0;

            //    if (i == scale - 1)
            //    {
            //        firstAngle = EvolutionUtility.GetAngleToNextPoint(TrackUtility.CentreOnZero(track1ScaledPoints)[i], TrackUtility.CentreOnZero(track1ScaledPoints)[0]);
            //        secondAngle = EvolutionUtility.GetAngleToNextPoint(TrackUtility.CentreOnZero(track2ScaledPoints)[i], TrackUtility.CentreOnZero(track2ScaledPoints)[0]);
            //        Debug.Log("Got to zero");
            //    }
            //    else
            //    {
            //        firstAngle = EvolutionUtility.GetAngleToNextPoint(TrackUtility.CentreOnZero(track1ScaledPoints)[i], TrackUtility.CentreOnZero(track1ScaledPoints)[i + 1]);
            //        secondAngle = EvolutionUtility.GetAngleToNextPoint(TrackUtility.CentreOnZero(track2ScaledPoints)[i], TrackUtility.CentreOnZero(track2ScaledPoints)[i + 1]);
                    
            //    }

            //    //Debug.Log(firstAngle + "  " + secondAngle);
            //}


            
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
