using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IsaacFagg.Paths;
using IsaacFagg.Utility;

namespace IsaacFagg.Track
{
    public class TrackData : MonoBehaviour
    {
        public new string name;

        [HideInInspector]
        public bool validTrack = false;
        //Points
        public List<Vector2> points;

        public Rotation rotation;
        [HideInInspector]
        public bool isRotationSet = false;

        #region Inputs

        public TrackData(List<Vector2> newPoints)
        {
            points = newPoints;

            if (!isRotationSet)
            {
                rotation = TrackUtility.RandomRotation();
                isRotationSet = true;
            }
        }
        public TrackData(List<Vector2> newPoints, int obstacle, int powerups)
        {
            points = newPoints;
            obstacleCount = obstacle;
            powerCount = powerups;

            if (!isRotationSet)
            {
                rotation = TrackUtility.RandomRotation();
                isRotationSet = true;
            }
        }
        public TrackData(List<Vector2> newPoints, string newName)
        {
            name = newName;
            points = newPoints;

            if (!isRotationSet)
            {
                rotation = TrackUtility.RandomRotation();
                isRotationSet = true;
            }
        }
        public TrackData(List<Vector2> newPoints, string newName, int obstacle, int powerups)
        {
            name = newName;
            points = newPoints;
            obstacleCount = obstacle;
            powerCount = powerups;

            if (!isRotationSet)
            {
                rotation = TrackUtility.RandomRotation();
                isRotationSet = true;
            }
        }
        public TrackData(List<Vector2> newPoints, Rotation rot)
        {
            points = newPoints;

            rotation = rot;
            isRotationSet = true;
        }
        public TrackData(List<Vector2> newPoints, Rotation rot, string newName)
        {
            name = newName;
            points = newPoints;

            rotation = rot;
            isRotationSet = true;
        }
        public TrackData(List<Vector2> newPoints, Rotation rot, string newName, int obstacle, int powerups)
        {
            name = newName;
            points = newPoints;

            rotation = rot;
            isRotationSet = true;

            obstacleCount = obstacle;
            powerCount = powerups;
        }

        #endregion

        //Properties
        private float height = 0;
        private bool isHeightSet = false;
        public float Height
        {
            get
            {
                if (isHeightSet)
                {
                    return height;
                }
                else
                {
                    return TrackUtility.GetHeight(points);
                }
            }
            set
            {
                height = value;
                isHeightSet = true;
            }
        }


        private float width = 0;
        private bool isWidthSet = false;
        public float Width
        {
            get
            {
                if (isWidthSet)
                {
                    return width;
                }
                else
                {
                    return TrackUtility.GetWidth(points);
                }
            }
            set
            {
                width = value;
                isWidthSet = true;
            }
        }
        private float length = 0;
        private bool isLengthSet = false;
        public float Length
        {
            get
            {
                if (isLengthSet)
                {
                    return length;
                }
                else
                {
                    return TrackUtility.GetLength(points);
                }
            }
            set
            {
                length = value;
                isLengthSet = true;
            }
        }

        private int straightCount = 0;
        private bool isStraightsSet = false;
        public int StraightCount
        {
            get
            {
                if (isStraightsSet)
                {
                    return straightCount;
                }
                else
                {
                    return TrackUtility.GetStraights(points);
                }
            }
            set
            {
                straightCount = value;
                isStraightsSet = true;
            }
        }
        private int curveCount = 0;
        private bool isCurvesSet = false;
        public int CurveCount
        {
            get
            {
                if (isCurvesSet)
                {
                    return curveCount;
                }
                else
                {
                    return TrackUtility.GetCurves(points);
                }
            }
            set
            {
                curveCount = value;
                isCurvesSet = true;
            }
        }

        public int obstacleCount = 0;
        public int powerCount = 0;

        public List<Vector2> ScaledPoints(int count)
        {
            Path path = new Path(points);

            List<Vector2> scaledPoints = path.CalculateNumberOfPoints(count);

            return scaledPoints;
        }
    }

}
