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

            foreach (Vector2 item in points)
            {
                centre += item;
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

        public static float GetAngleFromCentre(List<Vector2> points, int positions)
        {
            float angle = Vector2.Angle(GetCentre(points), points[0]);
            return angle;
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

    }
    public enum Rotation
    {
        Clockwise,
        Anticlockwise,
    }
}
