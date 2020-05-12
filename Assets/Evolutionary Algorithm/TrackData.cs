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
        [HideInInspector]
        public bool createdByScript = false;

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
                return GetAngles();
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
        private List<float> GetAngles()
        {
            List<float> angles = new List<float>();

            foreach (Segment segment in Segments)
            {
                angles.Add(segment.Angle);
            }
            return angles;
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

        public List<Segment> Segments
        {
            get
            {
                return GetSegments();
            }
        }

        public List<Segment> GetSegments()
        {
            List<Segment> segments = new List<Segment>();

            for (int i = 1; i < points.Count; i++)
            {
                Segment newSegment;


                if (i == points.Count - 2)
                {
                    newSegment = new Segment(points[i], points[i + 1], points[0]);

                }
                else if (i == points.Count - 1)
                {
                    newSegment = new Segment(points[i], points[0], points[1]);

                }
                else
                {
                    newSegment = new Segment(points[i], points[i + 1], points[i + 2]);

                }

                segments.Add(newSegment);
            }


            return segments;
        }




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

            foreach (Segment segment in Segments)
            {
                types.Add(segment.Type);
            }

            return types;
        }

        #endregion

        //Delete once testing finished
        private void Start()
        {
            if (!createdByScript)
            {
                CreateGameObject();
            }

        }

        public void CreateGameObject()
        {
            Color background = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

            GameObject go = new GameObject(name);

            TrackData newTD = go.AddComponent<TrackData>();
            newTD.name = name;
            newTD.points = points;
            newTD.createdByScript = true;
            newTD.type = type;

            PathCreator pc = go.AddComponent<PathCreator>();

            pc.path = new Path(points);
            pc.segmentColor = background;
            pc.displayControlPoints = false;

            if (TrackCheck.IntersectCheck(this))
            {
                //Debug.Log(name + " is intersecting");
            }

            Destroy(this);
        }

    }

    public class Segment
    {
        public Vector2 point1;
        public Vector2 point2;
        public Vector2 point3;

        public List<Vector2> Points
        {
            get
            {
                return new List<Vector2> { point1, point2, point3 };
            }
        }

        public Segment (Vector2 p1, Vector2 p2, Vector2 p3)
        {
            point1 = p1;
            point2 = p2;
            point3 = p3;
        }

        public Segment(List<Vector2> points)
        {
            point1 = points[0];
            point2 = points[1];
            point3 = points[2];
        }

        public SegmentType Type
        {
            get
            {
                if (point1 != null && Angle < 20f)
                {
                    return SegmentType.Straight;
                }
                else if (Angle > 20f)
                {
                    return TrackUtility.DetermineSegmentType(Points);
                }
                else
                {
                    return SegmentType.Straight;
                }

            }
        }    

        public float Angle
        {
            get
            {
                return GetAngle();
            }
        }

        private float GetAngle()
        {
            float distAB = Vector2.Distance(point1, point2);
            float distBC = Vector2.Distance(point2, point3);
            float distCA = Vector2.Distance(point1, point3);

            float sqrAB = Mathf.Pow(distAB, 2);
            float sqrBC = Mathf.Pow(distBC, 2);
            float sqrCA = Mathf.Pow(distCA, 2);

            return Mathf.Acos((sqrAB + sqrBC - sqrCA) / (2 * distAB * distBC)) * Mathf.Rad2Deg;
        }

        public Vector2 Size
        {
            get
            {
                return GetSize();
            }
        }

        private float GetSize()
        {
            MinMax2d size = new MinMax2D();

            size.AddValue(point1);
            size.AddValue(point2);
            size.AddValue(point3);

            float x = size.Max.x - size.Min.x;
            float y = size.Max.y - size.Min.y;
            Vector2 totalSize = new Vector2(x, y);

            return totalSize;
        }

        public float Area
        {
            get
            {
                return GetArea();
            }
        }

        private float GetArea()
        {
            return Size.x * Size.y;
        }

    }


    public enum TrackType
    {
        Random,
        Human, 
        Algorithm, 
        
    }

    public enum SegmentType
    {
        Straight,
        Left,
        Right
    }

}
