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

        //Properies
        public float height
        {
            get
            {
                return TrackUtility.GetHeight(points);
            }
        }
        public float width
        {
            get
            {
                return TrackUtility.GetWidth(points);
            }
        }
        public float length
        {
            get
            {
                return TrackUtility.GetLength(points);
            }
        }
        public int straightCount
        {
            get
            {
                    return TrackUtility.GetStraights(points);
            }
        }
        public int curveCount
        {
            get
            {
                return TrackUtility.GetCurves(points);
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
