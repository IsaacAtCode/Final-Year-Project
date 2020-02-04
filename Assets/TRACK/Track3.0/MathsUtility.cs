using System.Collections.Generic;
using UnityEngine;

namespace IsaacFagg.Utility
{
	public static class MathsUtility
	{
		public static bool LineSegmentsIntersect(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
		{
			float d = (b2.x - b1.x) * (a1.y - a2.y) - (a1.x - a2.x) * (b2.y - b1.y);
			if (d == 0)
				return false;
			float t = ((b1.y - b2.y) * (a1.x - b1.x) + (b2.x - b1.x) * (a1.y - b1.y)) / d;
			float u = ((a1.y - a2.y) * (a1.x - b1.x) + (a2.x - a1.x) * (a1.y - b1.y)) / d;

			return t >= 0 && t <= 1 && u >= 0 && u <= 1;
		}

		public static bool LinesIntersect(Vector2 a1, Vector2 a2, Vector2 a3, Vector2 a4)
		{
			return (a1.x - a2.x) * (a3.y - a4.y) - (a1.y - a2.y) * (a3.x - a4.x) != 0;
		}

		public static Vector2 PointOfLineLineIntersection(Vector2 a1, Vector2 a2, Vector2 a3, Vector2 a4)
		{
			float d = (a1.x - a2.x) * (a3.y - a4.y) - (a1.y - a2.y) * (a3.x - a4.x);
			if (d == 0)
			{
				Debug.LogError("Lines are parallel, please check that this is not the case before calling line intersection method");
				return Vector2.zero;
			}
			else
			{
				float n = (a1.x - a3.x) * (a3.y - a4.y) - (a1.y - a3.y) * (a3.x - a4.x);
				float t = n / d;
				return a1 + (a2 - a1) * t;
			}
		}

		public static Vector2 ClosestPointOnLineSegment(Vector2 p, Vector2 a, Vector2 b)
		{
			Vector2 aB = b - a;
			Vector2 aP = p - a;
			float sqrLenAB = aB.sqrMagnitude;

			if (sqrLenAB == 0)
				return a;

			float t = Mathf.Clamp01(Vector2.Dot(aP, aB) / sqrLenAB);
			return a + aB * t;
		}

		public static bool PointsAreClockwise(List<Vector2> points)
		{
			float signedArea = 0;
			for (int i = 0; i < points.Count; i++)
			{
				int nextIndex = (i + 1) % points.Count;
				signedArea += (points[nextIndex].x - points[i].x) * (points[nextIndex].y + points[i].y);
			}

			return signedArea >= 0;
		}

		public static Quaternion LookAt(Vector3 pos, Vector3 targetPos)
		{
			Vector3 difference = targetPos - pos;
			float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
			return Quaternion.Euler(0f, 0f, rotZ + 90f);
		}

		

	}
}

