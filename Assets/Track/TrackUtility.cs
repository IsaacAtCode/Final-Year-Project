using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IsaacFagg.Track;
using IsaacFagg.Paths;

namespace IsaacFagg.Utility
{
    public static class TrackUtility
    {
        public static float GetWidth(List<Vector2> points)
        {
            float minX = 0;
            float maxX = 0;

            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].x < minX)
                {
                    minX = points[i].x;
                }
                if (points[i].x > maxX)
                {
                    maxX = points[i].x;
                }
            }
            return Mathf.Abs(maxX - minX);
        }

        public static float GetHeight(List<Vector2> points)
        {
            float minY = 0;
            float maxY = 0;

            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].y < minY)
                {
                    minY = points[i].y;
                }
                if (points[i].y > maxY)
                {
                    maxY = points[i].y;
                }
            }

            return Mathf.Abs(maxY - minY);
        }

        public static float GetLength(List<Vector2> points)
        {
            Path path = new Path(points);

            return path.EstimatedLength();
        }

        public static int GetStraights(List<Vector2> points)
        {
            int straightCount = 0;

            for (int i = 0; i < points.Count - 1; i++)
            {
                if (Vector2.Distance(points[i], points[i + 1]) > 100) //MinDistace
                {
                    straightCount++;
                }
            }

            return straightCount;
        }

        public static int GetCurves(List<Vector2> points)
        {
            return points.Count - GetStraights(points);
        }

        public static Vector2 GetCentre(List<Vector2> points)
        {
            Vector2 centre = Vector2.zero;
            if (points.Count == 0 || points == null)
            {
                return Vector2.zero;
            }
            else
            {
                foreach (Vector2 item in points)
                {
                    centre += item;
                }
            }


            centre /= points.Count;

            return centre;
        }

        public static List<Vector2> CentreOnZero(List<Vector2> points)
        {
            List<Vector2> newPoints = new List<Vector2>();
            Vector2 centre = GetCentre(points);

            foreach (Vector2 point in points)
            {
                Vector2 newPoint = new Vector2(point.x - centre.x, point.y - centre.y);
                newPoints.Add(newPoint);
            }
            return newPoints;
        }

        public static float GetDistanceFromCentre(List<Vector2> points, int position)
        {
            float distance = Vector2.Distance(points[position], GetCentre(points));
            return distance;
        }

        public static List<Vector2> GetEqualPoints(List<Vector2> points, int count)
        {
            Path path = new Path(TrackUtility.GetCentre(points));

            return path.CalculateNumberOfPoints(count);

        }

        public static Rotation RandomRotation()
        {
            return (Rotation)Random.Range(0, 2);
        }

        public static Rotation GetRotation(List<Vector2> points)
        {
            Vector2 centre = GetCentre(points);

            if (((points[0].x - centre.x) * (points[points.Count - 1].y - centre.y) - (points[0].y - centre.y) * (points[points.Count - 1].x - centre.x)) > 0)
            {
                return Rotation.Clockwise;
            }
            else
            {
                return Rotation.Anticlockwise;
            }
        }



        public static List<Vector2> EqualPoints(List<Vector2> points, int count)
        {
            Path path = new Path(points);

            List<Vector2> scaledPoints = path.CalculateNumberOfPoints(count);

            return scaledPoints;
        }

        public static List<Vector2> ReversePoints(List<Vector2> points)
        {
            List<Vector2> mPoints = new List<Vector2>(points);

            mPoints.Reverse();

            return mPoints;
        }

        public static List<Vector2> CentredPoints(List<Vector2> points)
        {
            List<Vector2> cPoints = new List<Vector2>();

            Vector2 oldCentre = GetCentre(points);

            foreach (Vector2 point in points)
            {
                Vector2 newPoint = new Vector2(point.x - oldCentre.x, point.y - oldCentre.y);
                cPoints.Add(newPoint);
            }

            return cPoints;
        }

        public static List<Vector2> ScaledPoints(List<Vector2> points, float dWidth, float dHeight)
        {
            List<Vector2> scaled = new List<Vector2>(points);

            float width = GetWidth(scaled);
            float height = GetHeight(scaled);

            float widthScale = dWidth / width;
            float heightScale = dHeight / height;

            float scale = Mathf.Min(widthScale, heightScale);
            Vector2 vScale = new Vector2(scale, scale);

            //Debug.Log(scale + " " + vScale);

            for (int i = 0; i < scaled.Count; i++)
            {
                scaled[i] = Vector2.Scale(scaled[i], vScale);
            }

            return scaled;

        }


        public static bool ArePointsTooClose(Vector2 a, Vector2 b)
        {
            if (Vector2.Distance(a,b) < 100)
            {
                return true;
            }
            return false;
        }

        public static Vector2 CombinePoints(Vector2 a, Vector2 b)
        {
            float x = (a.x + b.x) / 2;
            float y = (a.y + b.y) / 2;
            Vector2 midPoint = new Vector2(x, y);

            return midPoint;
        }



    }

    public enum Rotation
    {
        Clockwise,
        Anticlockwise,
    }

    public enum SegmentType
    {
        Straight,
        Left,
        Right
    }

}
