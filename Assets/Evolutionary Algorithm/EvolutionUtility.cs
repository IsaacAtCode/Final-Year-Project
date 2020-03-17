using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IsaacFagg.Track;

namespace IsaacFagg.Utility
{
    public static class EvolutionUtility
    {
        public static float GetFloatWithMutation(float minimum, float maximum, float maxRange, float mutationRate)
        {
            float min = Mathf.Min(minimum, maximum);
            float max = Mathf.Max(minimum, maximum);

            float randomNumber = Random.Range(min, max);
            float randomMutationStart = Random.Range(min, max);
            float randomMutationEnd = randomMutationStart + mutationRate;

            if (randomNumber > randomMutationStart && randomNumber < randomMutationEnd )
            {
                float newRandom = Random.Range(min * (1 - mutationRate), max * (1 + mutationRate));


                if (newRandom > maxRange)
                {
                    newRandom %= maxRange;

                    return newRandom;
                }
                else
                {
                    return newRandom;
                }
            }
            else
            {
                return randomNumber;
            }
        }
        public static float GetAngleToNextPoint(Vector2 initialPoint, Vector2 nextPoint)
        {
            float x = nextPoint.x - initialPoint.x;
            float y = nextPoint.y - initialPoint.y;
            Vector2 slopePoint = new Vector2(x, y);

            //return Vector2.Angle(Vector2.up, slopePoint);
            return Vector2.SignedAngle(Vector2.up, slopePoint);

        }

        public static float GetSlopeForPoint(Vector2 point, Vector2 centre)
        {
            Vector2 perpSlope = Vector2.Perpendicular(point);

            float px = perpSlope.x - centre.x;
            float py = perpSlope.y - centre.y;
            float perpAngle = py / px;

            //Debug.Log(point + " " + perpSlope);
           // Debug.Log(" " + perpAngle);

            float angle = Vector2.SignedAngle(Vector2.up, perpSlope);


            return angle;
        }

        public static Rotation OppositeRotation(Rotation rot)
        {
            if (rot == Rotation.Clockwise)
            {
                return Rotation.Anticlockwise;
            }
            else
            {
                return Rotation.Clockwise;
            }
        }


        public static Rotation CompareRotation(Rotation p1, Rotation p2)
        {
            if (p1 == p2)
            {
               return p1;
            }
            else
            {
                return TrackUtility.RandomRotation();
            }
        }


        public static SegmentType RandomSegment()
        {
            return (SegmentType)Random.Range(0, 3);
        }

        public static SegmentType RandomSegment(bool onlyTurns)
        {
            if (onlyTurns)
            {
                return (SegmentType)Random.Range(1, 3);
            }
            else
            {
                return (SegmentType)Random.Range(0, 3);
            }
        }

        public static SegmentType WeightedRandomSegment(float sCount, float lCount, float rCount)
        {
            List<SegmentType> sTypes = new List<SegmentType>();

            float maxCount = sCount + lCount + rCount;
            float scale = 100 / maxCount;

            int scaledStraightCount = Mathf.CeilToInt(sCount* scale);
            int scaledLeftCount = Mathf.CeilToInt(lCount * scale);
            int scaledRightCount = Mathf.CeilToInt(rCount * scale);

            sTypes.AddRange(Enumerable.Repeat(SegmentType.Straight, scaledStraightCount));
            sTypes.AddRange(Enumerable.Repeat(SegmentType.Left, scaledLeftCount));
            sTypes.AddRange(Enumerable.Repeat(SegmentType.Right, scaledRightCount));

            return sTypes[Random.Range(0, sTypes.Count - 1)];
        }

        public static SegmentType CompareSegments(SegmentType s1, SegmentType s2)
        {
            if (s1 == s2)
            {
                //Debug.Log("Same " + s1.ToString());

                return s1;
            }
            else if (s1 == SegmentType.Straight || s2 == SegmentType.Straight)
            {
                if (s1 == SegmentType.Left || s2 == SegmentType.Left)
                {
                    //Debug.Log("S1: " + s1 + " S2: " + s2 + " Result: Left");
                    return SegmentType.Left;
                }
                else if (s1 == SegmentType.Right || s2 == SegmentType.Right)
                {
                    //Debug.Log("S1: " + s1 + " S2: " + s2 + " Result: Right");
                    return SegmentType.Right;
                }
                else
                {
                    //Debug.Log("S1: " + s1 + " S2: " + s2 + " Result: Straight");
                    return SegmentType.Straight;
                }
            }
            else
            {
                //Debug.Log("S1: " + s1 + " S2: " + s2 + " Result: Random");
                return RandomSegment();
            }
        }

        public static SegmentType CompareWeightedSegments(SegmentType s1, float w1, SegmentType s2, float w2)
        {
            if (s1 == s2)
            {
                return s1;
            }
            else if (s1 == SegmentType.Straight || s2 == SegmentType.Straight)
            {
                if (s1 == SegmentType.Left)
                {
                    return WeightedRandomSegment(w2, w1, 0);
                }
                else if (s2 == SegmentType.Left)
                {
                    return WeightedRandomSegment(w1, w2, 0);
                }
                else if (s1 == SegmentType.Right)
                {
                    return WeightedRandomSegment(w2, 0, w1);
                }
                else if (s2 == SegmentType.Right)
                {
                    return WeightedRandomSegment(w1, 0, w2);
                }
                else
                {
                    return SegmentType.Straight;
                }
            }
            else
            {
                return SegmentType.Straight;
            }
        }

        public static int SegmentTypeCount(List<SegmentType> list, SegmentType type)
        {
            return list.Count(st => st == type);
        }

        public static List<Segment> FindSegments(List<Segment> segments, SegmentType type)
        {
            List<Segment> segmentsOfType = new List<Segment>();

            foreach (Segment segment in segments)
            {
                if (segment.Type == type)
                {
                    segmentsOfType.Add(segment);
                }
            }

            return segmentsOfType;
        }

    }




}
