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

        public List<SegmentType> segmentTypes;

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
            Rotation rot = EvolutionUtility.CompareRotation(parent1.rotation, parent2.rotation);



            //Parents Created
            TrackData track1 = gameObject.AddComponent<TrackData>();
            track1.name = "Track 1 Scaled";
            track1.points = NormaliseParent(parent1, scale, rot);

            TrackData track2 = gameObject.AddComponent<TrackData>();
            track2.name = "Track 2 Scaled";
            track2.points = NormaliseParent(parent2, scale, rot);

            //Get segments from Each Parent
            segmentTypes = SegmentChildFromParents(track1, track2);

            Debug.Log("Straights: " + EvolutionUtility.SegmentTypeCount(segmentTypes, SegmentType.Straight));
            Debug.Log("Lefts: " + EvolutionUtility.SegmentTypeCount(segmentTypes, SegmentType.Left));
            Debug.Log("Rights: " + EvolutionUtility.SegmentTypeCount(segmentTypes, SegmentType.Right));


            //Adding possible angles 
            //List<float> p1Left = GetAnglesFromSegmentType(parent1, SegmentType.Left);
            //List<float> p1Right = GetAnglesFromSegmentType(parent1, SegmentType.Right);

            //List<float> p2Left = GetAnglesFromSegmentType(parent2, SegmentType.Left);
            //List<float> p2Right = GetAnglesFromSegmentType(parent2, SegmentType.Right);

            //List<float> possLeft = new List<float>();
            //List<float> possRight = new List<float>();

            //possLeft.AddRange(p1Left);
            //possLeft.AddRange(p2Left);

            //possRight.AddRange(p1Right);
            //possRight.AddRange(p2Right);

            //Determine what stats the track should have
            float startDistance = Random.Range(track1.DistanceFromCentre, track2.DistanceFromCentre);
            float startRotation = 0f;

            if (rot == Rotation.Clockwise)
            {
                startRotation = Random.Range(-180, -15);
            }
            else
            {
                startRotation = Random.Range(15, 180);
            }

            float dist = Random.Range(track1.AverageDistance, track2.AverageDistance);
            float length = dist * scale * 0.5f;

            float minAngle = Mathf.Clamp(Mathf.Min(track1.MinAngle, track2.MinAngle), 0, 360);
            float maxAngle = Mathf.Clamp(Mathf.Max(track1.MaxAngle, track2.MaxAngle), 0, 360);

            float angleTotal = (scale - 2) * 180;

            List<Vector2> childTrack = new List<Vector2>();

            //First 3 Points




            childTrack.AddRange(GenerateStartingPoints(startDistance, startRotation, segmentTypes[0], segmentTypes[1]));

            for (int i = 2; i < segmentTypes.Count; i++)
            {
                float prevAngle = EvolutionUtility.GetAngleToNextPoint(childTrack[i - 2], childTrack[i - 1]);
                float testAngle = EvolutionUtility.GetAngleToNextPoint(childTrack[i - 1], childTrack[i - 2]);

                Debug.Log(prevAngle + "  " + testAngle);


                Vector2 point;


                if (segmentTypes[i] == SegmentType.Straight)
                {
                    point = GenerateStraight(childTrack[i], dist, prevAngle); 
                }
                else if (segmentTypes[i] == SegmentType.Left)
                {
                    point = GenerateLeft(childTrack[i], dist, prevAngle);
                }
                else if (segmentTypes[i] == SegmentType.Right)
                {
                    point = GenerateRight(childTrack[i], dist, prevAngle);
                }
                else
                {
                    Debug.Log("Shouldnt be getting here");
                    point = GenerateStraight(childTrack[i], dist, 0);
                }

                childTrack.Add(point);
            }

            return childTrack;

        }






        private Vector2 NextPoint(Vector2 prev, float distance, float rotation)
        {
            float x = prev.x + (distance * Mathf.Cos(rotation));
            float y = prev.y + (distance * Mathf.Sin(rotation));

            return new Vector2(x, y);
        }

        private Vector2 GenerateStraight(Vector2 prev, float distance, float prevAngle)
        {
            float angle = prevAngle + Random.Range(-10, 10);
            return NextPoint(prev, distance, angle);
        }

        private Vector2 GenerateLeft(Vector2 prev, float distance, float prevAngle)
        {
            float angle = prevAngle + Random.Range(-160, -10);
            return NextPoint(prev, distance, angle);
        }

        private Vector2 GenerateRight(Vector2 prev, float distance, float prevAngle)
        {
            float angle = prevAngle + Random.Range(10, 160);
            return NextPoint(prev, distance, angle);
        }

        private List<Vector2> GenerateStartingPoints(float dist, float angle, SegmentType firstST, SegmentType secondST)
        {
            Vector2 startPoint = NextPoint(Vector2.zero, dist, angle * Mathf.Deg2Rad);

            Vector2 secondPoint;

            if (firstST == SegmentType.Straight)
            {
                secondPoint = GenerateStraight(startPoint, dist, angle);
            }
            else if (firstST == SegmentType.Left)
            {
                secondPoint = GenerateLeft(startPoint, dist, angle);
            }
            else if (firstST == SegmentType.Right)
            {
                secondPoint = GenerateRight(startPoint, dist, angle);
            }
            else
            {
                secondPoint = GenerateStraight(startPoint, dist, angle);
            }

            Vector2 thirdPoint;
            float otherAngle = EvolutionUtility.GetAngleToNextPoint(startPoint, secondPoint);

            if (secondST == SegmentType.Straight)
            {
                thirdPoint = GenerateStraight(secondPoint, dist, otherAngle);
            }
            else if (secondST == SegmentType.Left)
            {
                thirdPoint = GenerateLeft(secondPoint, dist, otherAngle);
            }
            else if (secondST == SegmentType.Right)
            {
                thirdPoint = GenerateRight(secondPoint, dist, otherAngle);
            }
            else
            {
                thirdPoint = GenerateStraight(secondPoint, dist, otherAngle);
            }



            List<Vector2> points = new List<Vector2> { startPoint, secondPoint, thirdPoint };

            return points;

        }



        private List<float> GetParentsAngles(List<float> parent1, List<float> parent2)
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

        private List<float> GetAnglesFromSegmentType(TrackData parent, SegmentType type)
        {
            List<float> angles = new List<float>();

            for (int i = 0; i < parent.SegmentTypes.Count; i++)
            {
                if (parent.SegmentTypes[i] == type)
                {
                    angles.Add(parent.Angles[i]);
                }
            }

            return angles;
        }



        private List<SegmentType> SegmentChildFromParents(TrackData parent1, TrackData parent2)
        {
            List<SegmentType> segments = new List<SegmentType>();
            bool atLeastOneStraight = false;
            float currentPercent = 0;

            for (int i = 0; i < parent1.SegmentTypes.Count; i++)
            {
                if (EvolutionUtility.SegmentTypeCount(segments, SegmentType.Straight) > 0)
                {
                    atLeastOneStraight = true;
                }

                if (!atLeastOneStraight)
                {
                    float stepsLeft = parent1.SegmentTypes.Count - i;
                    float step = (100 - currentPercent) / stepsLeft;
                    currentPercent += step;
                    currentPercent = Mathf.Clamp(currentPercent, 0, 100);

                    if (Random.Range(0f, 1f) <= currentPercent / 100)
                    {
                        segments.Add(SegmentType.Straight);
                        atLeastOneStraight = true;
                        currentPercent = 0f;

                        Debug.Log(i);

                        continue;
                    }
                }

                if (i != 0 && segments[i - 1] == SegmentType.Straight && EvolutionUtility.SegmentTypeCount(segments, SegmentType.Straight) <= 2)
                {
                    if (Random.Range(0f, 1f) <=  0.65)
                    {
                        segments.Add(SegmentType.Straight);

                        //Debug.Log("Second straight");

                        continue;
                    }
                }

                int rand = Random.Range(0, 100);

                if (rand > 25 && rand < 40)
                {
                    segments.Add(EvolutionUtility.RandomSegment(true));
                    //Debug.Log("Random segment");
                }
                else
                {
                    segments.Add(EvolutionUtility.CompareSegments(parent1.SegmentTypes[i], parent2.SegmentTypes[i]));
                }
            }
            return segments;
        }

        private float GetTotalAngles(List<float> angles)
        {
            float total = 0f;

            foreach (float angle in angles)
            {
                total += angle;
            }

            return total;
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
                return TrackUtility.EqualPoints(parent, newCount);
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
    }
}
