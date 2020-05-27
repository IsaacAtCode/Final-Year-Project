using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacFagg.Track
{
    public class IdealTrack : MonoBehaviour
    {
        private float length;
        private int straightCount;
        private int cornerCount;
        private float speed;
        private float difficulty;

        public float Length
        {
            get
            {
                return length;
            }
            set
            {
                length = ChangeValue(length, value);
            }
        }
        public int StraightCount
        {
            get
            {
                return straightCount;
            }
            set
            {
                straightCount = ChangeValue(straightCount, value);
            }
        }
        public int CornerCount
        {
            get
            {
                return cornerCount;
            }
            set
            {
                cornerCount = ChangeValue(cornerCount, value);
            }
        }
        public float Speed
        {
            get
            {
                return speed;
            }
            set
            {
                speed = ChangeValue(speed, value);
            }
        }
        public float Difficulty
        {
            get
            {
                return difficulty;
            }
            set
            {
                difficulty = ChangeValue(difficulty, value);
            }
        }

        public List<Segment> AvoidSegments;
        public List<Segment> IncludeSegments;

        private int ChangeValue(int value, int amount)
        {
            return value + amount;
        }
        private float ChangeValue(float value, float amount)
        {
            return value + amount;
        }

        public void AddAvoidSegment(Segment avoid)
        {
            AvoidSegments.Add(avoid);
        }
        public void AddIncludeSegment(Segment inc)
        {
            IncludeSegments.Add(inc);
        }
    }
}
