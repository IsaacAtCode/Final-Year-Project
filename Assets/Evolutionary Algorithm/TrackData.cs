using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IsaacFagg.Paths;
using IsaacFagg.Utility;
using IsaacFagg.Icons;

namespace IsaacFagg.Track
{
    public class TrackData : MonoBehaviour
    {
        public new string name;

        public Texture2D icon;

        public TrackType type = TrackType.Random;


        [HideInInspector]
        public bool validTrack = false;
        //Points
        public List<Vector2> points;

        [HideInInspector]
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
        [HideInInspector]
        public int obstacleCount = 0;
        [HideInInspector]
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


        public void PopulateTrackData(TrackData newTrack)
        {
            name = newTrack.name;
            points = newTrack.points;
        }


        public Vector2 Centre
        {
            get
            {
                return TrackUtility.GetCentre(points);
            }
        }



        #region Angles

        public List<float> Angles
        {
            get
            {
                return AnglesFromPoints();
            }
        }
        public float MaxAngle
        {
            get
            {
                return Angles.Max();
            }
        }
        public float MinAngle
        {
            get
            {
                return Angles.Min();
            }
        }
        public float AverageAngle
        {
            get
            {
                return Angles.Average();
            }
        }
        public List<float> Slopes
        {
            get
            {
                return SlopesFromPoints();
            }
        }
        private List<float> AnglesFromPoints()
        {
            List<float> angles = new List<float>();

            List<Vector2> cPoints = TrackUtility.CentreOnZero(points);

            for (int i = 0; i < points.Count; i++)
            {
                float angle;
                Vector2 point;
                Vector2 next;

                if (i == points.Count - 1)
                {
                    point = cPoints[i];
                    next = cPoints[0];

                }
                else
                {
                    point = cPoints[i];
                    next = cPoints[i + 1];
                }

                angle = EvolutionUtility.GetAngleToNextPoint(point, next);

                angles.Add(angle);
            }

            return angles;
        }
        private List<float> SlopesFromPoints()
        {
            List<float> slopes = new List<float>();

            for (int i = 0; i < points.Count; i++)
            {
                slopes.Add(EvolutionUtility.GetSlopeForPoint(points[i], Centre));
            }

            return slopes;
        }

        #endregion

        #region Distances

        public List<float> Distances
        {
            get
            {
                return DistancesFromPoints();
            }
        }
        public float MaxDistance
        {
            get
            {
                return Distances.Max();
            }
        }
        public float MinDistance
        {
            get
            {
                return Distances.Min();
            }
        }
        public float AverageDistance
        {
            get
            {
                return Distances.Average();
            }
        }
        public float DistanceFromCentre
        {
            get
            {
                return Vector2.Distance(Centre, points[0]);
            }
        }

        public List<float> DistancesFromPoints()
        {
            List<float> distances = new List<float>();

            for (int i = 0; i < points.Count; i++)
            {
                float dist;

                if (i == points.Count - 1)
                {
                    dist = Vector2.Distance(points[i], points[0]);

                }
                else
                {
                    dist = Vector2.Distance(points[i], points[i + 1]);
                }

                distances.Add(dist);
            }

            return distances;
        }

        #endregion

        #region Segments

        public List<SegmentType> SegmentTypes
        {
            get
            {
                return GetSegmentTypes();
            }
        }

        public List<SegmentType> GetSegmentTypes()
        {
            List<SegmentType> types = new List<SegmentType>();

            for (int i = 0; i < points.Count; i++)
            {
                int prevPoint;

                if (i == 0)
                {
                    prevPoint = points.Count - 1;
                }
                else
                {
                    prevPoint = i - 1;
                }

                int nextPoint = (i + 1) % points.Count;

                types.Add(DetermineSegmentType(points[prevPoint], points[i], points[nextPoint]));
            }
            return types;
        }

        private SegmentType DetermineSegmentType(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            if (MathsUtility.Left(p1, p2, p3))
            {
                return SegmentType.Left;
            }
            else if (MathsUtility.Right(p1, p2, p3))
            {
                return SegmentType.Right;
            }
            else
            {
                return SegmentType.Straight;
            }
        }

        #endregion

        //Delete once testing finished
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
                //Debug.Log(name + " is intersecting");
            }
        }
    }

    public enum TrackType
    { 
        Human, 
        Algorithm, 
        Random 
    }

}
