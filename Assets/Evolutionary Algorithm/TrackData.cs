using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
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
        private float width = 0;
        private float length = 0;
        private int straightCount = 0;
        private int curveCount = 0;
        public int obstacleCount = 0;
        public int powerCount = 0;

        #region Properties Get/Set
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
        private bool isHeightSet = false;
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
        private bool isWidthSet = false;
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
        private bool isLengthSet = false;
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
        private bool isStraightsSet = false;
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
        private bool isCurvesSet = false;
        #endregion


        public List<Vector2> ScaledPoints(int count)
        {
            Path path = new Path(points);

            List<Vector2> scaledPoints = path.CalculateNumberOfPoints(count);

            return scaledPoints;
        }

        private void Start()
        {
            Color background = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f),Random.Range(0f, 1f));

            GameObject go = new GameObject(name);

            PathCreator pc = go.AddComponent<PathCreator>();

            pc.path = new Path(points);
            pc.segmentColor = background;
            pc.displayControlPoints = false;

            if (TrackCheck.IntersectCheck(this))
            {
                Debug.Log(name + " is intersecting");
            }




        }



    }

}
