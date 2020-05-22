using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IsaacFagg.Track;
using IsaacFagg.Utility;

namespace IsaacFagg.Genetics
{
    public class TrackEvolution : MonoBehaviour
    {
        public enum EvolutionType { Combine, Mutate, Segments}
        public EvolutionType evoType;



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

        private void Start()
        {
            TrackData track1 = gameObject.AddComponent<TrackData>();
            track1.name = "Track 1";
            track1.points = RandomTrackGenerator.GenerateRandomTrack().points;
            track1.type = TrackType.Random;


            TrackData track2 = gameObject.AddComponent<TrackData>();
            track2.name = "Track 2";
            track2.points = RandomTrackGenerator.GenerateRandomTrack().points;
            track2.type = TrackType.Random;

            TrackData track3 = gameObject.AddComponent<TrackData>();
            track3.name = "Track 3";
            track3.points = RandomTrackGenerator.GenerateRandomTrack().points;
            track3.type = TrackType.Random;

            TrackData track4 = gameObject.AddComponent<TrackData>();
            track4.name = "Track 4";
            track4.points = RandomTrackGenerator.GenerateRandomTrack().points;
            track4.type = TrackType.Random;

            TrackData track5 = gameObject.AddComponent<TrackData>();
            track5.name = "Track 5";
            track5.points = RandomTrackGenerator.GenerateRandomTrack().points;
            track5.type = TrackType.Random;




            //TrackData[] tracks = GetComponents<TrackData>();

            //if (evoType == EvolutionType.Combine)
            //{
            //    TrackData babytrack = gameObject.AddComponent<TrackData>();
            //    babytrack.name = "Baby Track";
            //    babytrack.points = CombineTrackData(track1, track2);
            //}
            //else if (evoType == EvolutionType.Mutate)
            //{
            //    TrackData mutatedTrack1 = gameObject.AddComponent<TrackData>();
            //    mutatedTrack1.name = "Mutated Track 1";
            //    mutatedTrack1.points = MutateTrackData(track1);
            //    mutatedTrack1.type = TrackType.Algorithm;

            //    TrackData mutatedTrack2 = gameObject.AddComponent<TrackData>();
            //    mutatedTrack2.name = "Mutated Track 2";
            //    mutatedTrack2.points = MutateTrackData(track2);
            //    mutatedTrack2.type = TrackType.Algorithm;
            //}
            //else if (evoType == EvolutionType.Segments)
            //{
            //    TrackData segmentedTrack = gameObject.AddComponent<TrackData>();
            //    segmentedTrack.name = "Segment Track";
            //    segmentedTrack.points = CombineSegments(track1, track2);
            //    segmentedTrack.type = TrackType.Algorithm;
            //}



        }

        public List<Vector2> CombineTrackData(TrackData parent1, TrackData parent2)
        {
            int scale = Mathf.Clamp(MutationScale(Random.Range(parent1.points.Count, parent2.points.Count)), minPoints, maxPoints);
            Rotation rot = MutationRotation(EvolutionUtility.CompareRotation(parent1.rotation, parent2.rotation));



            //Parents Created
            TrackData track1 = gameObject.AddComponent<TrackData>();
            track1.name = "Track 1 Scaled";
            track1.points = NormaliseParent(parent1, scale, rot);
            track1.type = TrackType.Algorithm;

            TrackData track2 = gameObject.AddComponent<TrackData>();
            track2.name = "Track 2 Scaled";
            track2.points = NormaliseParent(parent2, scale, rot);
            track2.type = TrackType.Algorithm;

            //Get segments from Each Parent
            List<SegmentType> segmentTypes = SegmentChildFromParents(track1, track2);

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

            return TrackUtility.CentredPoints(childTrack);

        }

        public List<Vector2> MutateTrackData(TrackData parent)
        {
            //Chance to Randomly Change Scale
            int scale = MutationScale(parent.points.Count);

            //Chance to mutate Rotation
            Rotation rot = MutationRotation(parent.rotation);


            TrackData parentScaled = new TrackData(NormaliseParent(parent, scale, rot));
            parentScaled.type = TrackType.Algorithm;

            List<Vector2> points = new List<Vector2>(parentScaled.points);

            List<SegmentType> segmentTypes = new List<SegmentType>();

            for (int i = 0; i < parentScaled.SegmentTypes.Count; i++)
            {
                SegmentType type = parentScaled.SegmentTypes[i];

                int rand = Random.Range(0, 100);

                if (rand < 10)
                {
                    type = MutateSegmentType(type);

                    //Debug.Log("Mutated at: " + i);
                }

                segmentTypes.Add(type);
            }

            Vector2 testPoint = MathsUtility.RotateVectorAroundVector(points[0], points[1], 50, true);

            Debug.Log("Before: " + points[1] + " Centre: " + points[0] + " Results: " + testPoint);

            float distOC = Vector2.Distance(points[0], points[1]);
            float distON = Vector2.Distance(points[1], testPoint);
            float distCN = Vector2.Distance(points[0], testPoint);



            return TrackUtility.CentredPoints(points);
        }

        public List<Vector2> CombineSegments(TrackData parent1, TrackData parent2)
        {
            int scale = Mathf.Clamp(MutationScale(Random.Range(parent1.points.Count, parent2.points.Count)), minPoints, maxPoints);
            Rotation rot = MutationRotation(EvolutionUtility.CompareRotation(parent1.rotation, parent2.rotation));

            TrackData track1 = gameObject.AddComponent<TrackData>();
            track1.name = "Track 1 Scaled";
            track1.points = NormaliseParent(parent1, scale, rot);
            track1.type = TrackType.Algorithm;

            TrackData track2 = gameObject.AddComponent<TrackData>();
            track2.name = "Track 2 Scaled";
            track2.points = NormaliseParent(parent2, scale, rot);
            track2.type = TrackType.Algorithm;

            List<Segment> segments = new List<Segment>(track1.Segments);
            segments.AddRange(track2.Segments);


            List<SegmentType> types = SegmentChildFromParents(track1, track2);

            Debug.Log("Straights: " + EvolutionUtility.SegmentTypeCount(types, SegmentType.Straight));
            Debug.Log("Lefts: " + EvolutionUtility.SegmentTypeCount(types, SegmentType.Left));
            Debug.Log("Rights: " + EvolutionUtility.SegmentTypeCount(types, SegmentType.Right));

            int straights = EvolutionUtility.SegmentTypeCount(types, SegmentType.Straight);
            int lefts = EvolutionUtility.SegmentTypeCount(types, SegmentType.Left);
            int rights = EvolutionUtility.SegmentTypeCount(types, SegmentType.Right);

            List<Segment> straightSegments = EvolutionUtility.FindSegments(segments, SegmentType.Straight);
            List<Segment> leftSegments = EvolutionUtility.FindSegments(segments, SegmentType.Left);
            List<Segment> rightSegments = EvolutionUtility.FindSegments(segments, SegmentType.Right);

            List<Vector2> points = new List<Vector2> { Vector2.zero, Vector2.one, Vector2.up};




            return TrackUtility.CentredPoints(points);
        }



        #region Mutations

        private int MutationScale(int currentScale)
        {
            if (Random.Range(0, 100) > 90)
            {
                return Mathf.Clamp(Random.Range(currentScale - 2, currentScale + 2), 10, 40);
            }
            else
            {
                return currentScale;
            }
        }



        private Rotation MutationRotation(Rotation rot)
        {
            if (Random.Range(0, 100) > 90)
            {
                return EvolutionUtility.OppositeRotation(rot);
            }
            else
            {
                return rot;
            }
        }

        private SegmentType MutateSegmentType(SegmentType type)
        {
            SegmentType mutatedType = EvolutionUtility.RandomSegment();

            while (mutatedType == type)
            {
                mutatedType = EvolutionUtility.RandomSegment();
            }

            return mutatedType;

        }

        #endregion

        #region Point Generation


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

        #endregion

        #region Angles


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

        private float GetTotalAngles(List<float> angles)
        {
            float total = 0f;

            foreach (float angle in angles)
            {
                total += angle;
            }

            return total;
        }

        #endregion


        #region Segments
        private List<SegmentType> SegmentChildFromParents(TrackData parent1, TrackData parent2)
        {
            List<SegmentType> types = new List<SegmentType>();
            bool atLeastOneStraight = false;
            float currentPercent = 0;

            for (int i = 0; i < parent1.SegmentTypes.Count; i++)
            {
                if (EvolutionUtility.SegmentTypeCount(types, SegmentType.Straight) > 0)
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
                        types.Add(SegmentType.Straight);
                        atLeastOneStraight = true;
                        currentPercent = 0f;

                        continue;
                    }
                }

                if (i != 0 && types[i - 1] == SegmentType.Straight && EvolutionUtility.SegmentTypeCount(types, SegmentType.Straight) <= 2)
                {
                    if (Random.Range(0f, 1f) <=  0.65)
                    {
                        types.Add(SegmentType.Straight);

                        //Debug.Log("Second straight");

                        continue;
                    }
                }

                int rand = Random.Range(0, 100);

                if (rand > 25 && rand < 40)
                {
                    types.Add(EvolutionUtility.RandomSegment(true));
                    //Debug.Log("Random segment");
                }
                else
                {
                    types.Add(EvolutionUtility.CompareSegments(parent1.Segments[i].Type, parent2.Segments[i].Type));
                }
            }
            return types;
        }
        #endregion


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
            if (TrackUtility.GetTrackRotation(parent) != rot)
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
