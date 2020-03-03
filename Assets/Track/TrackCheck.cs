using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IsaacFagg.Track;


namespace IsaacFagg.Utility
{
    public static class TrackCheck
    {
		public static void OverlapCheck(List<Vector2> points, GameObject parent)
		{
			List<Vector2> intersections = new List<Vector2>();

			for (int i = 0; i < points.Count - 1; i++)
			{
				for (int j = points.Count - 1; j >= 1; j--)
				{
					if (j == i || j == i + 1 || j == i - 1 || i == j - 1 || i == j + 1)
					{
						continue;
					}
					else
					{
						if (MathsUtility.LineSegmentsIntersect(points[i], points[i + 1], points[j], points[j - 1]))
						{
							Vector2 intersect = MathsUtility.PointOfLineLineIntersection(points[i], points[i + 1], points[j], points[j - 1]);

							if (!points.Contains(intersect))
							{
								intersections.Add(intersect);
							}
						}
					}
				}
			}

			foreach (Vector2 point in intersections)
			{
				GameObject intersect = GameObject.CreatePrimitive(PrimitiveType.Cube);
				intersect.transform.parent = parent.transform;
				intersect.transform.localScale = new Vector2(10, 10);


				Vector2 newPos = new Vector2(point.x, point.y);

				intersect.transform.position = newPos;

			}
		}

		public static bool IntersectCheck(TrackData td)
        {
			List<Vector2> points = td.points;
			int segmentCount = points.Count / 3;

			List<Segment> segments =  new List<Segment>();

			for (int i = 0; i < segmentCount; i++)
			{
				Segment segment = new Segment();
				segment.points = GetPointsInSegment(points, i);
				segments.Add(segment);
			}

			bool isIntersecting = false;


			for (int i = 0; i < segments.Count-1; i++)
			{
				for (int j = 0; j < segments.Count-1; j++)
				{
					if (i != j)
					{
						if(BezierIntersect(segments[i].points, segments[j].points))
						{
							isIntersecting = true;
						}
					}
				}
			}

			return isIntersecting;
        }

		public static bool BezierIntersect(List<Vector2> bezier1, List<Vector2> bezier2 )
		{
			//Bounds boundingBox1 = Bezier.CalculateSegmentBounds(bezier1);
			//Bounds boundingBox2 = Bezier.CalculateSegmentBounds(bezier2);

			//if (!boundingBox1.Intersects(boundingBox2))
			//{
			//	return false;
			//}

			//Vector2 middle1 = Vector2.zero;
			//Vector2 middle2 = Vector2.zero;

			////How to turn a 3 point bevier into a 4 point one


			//middle1 = Bezier.EvaluateCubic(bezier1, 0.5f);

			//if (bezier2.Count == 3)
			//{
			//	middle2 = Bezier.EvaluateQuadratic(bezier2, 0.5f);

			//}
			//else if (bezier2.Count == 4)
			//{
			//	middle2 = Bezier.EvaluateCubic(bezier2, 0.5f);
			//}


			//List<Vector2> b1a = new List<Vector2> { bezier1[0], bezier1[1], middle1 };
			//List<Vector2> b1b = new List<Vector2> { middle1, bezier1[2], bezier1[3] };


			//List<Vector2> b2a = new List<Vector2> { bezier2[0], bezier2[1], middle2 };
			//List<Vector2> b2b = new List<Vector2> { middle2, bezier2[2], bezier2[3] };

			//return (BezierIntersect(b1a, b2a) || BezierIntersect(b1a, b2b) || BezierIntersect(b1b, b2a) || BezierIntersect(b1b, b2b));

			return true;
		}

		public static float BoundsArea2D(Bounds bound)
		{
			return bound.size.x * bound.size.y;
		}

		public static List<Vector2> GetPointsInSegment(List<Vector2> points, int i)
		{
			int loopIndex = ((i * 3 + 3) + points.Count) % points.Count;

			return new List<Vector2> { points[i * 3], points[i * 3 + 1], points[i * 3 + 2], points[loopIndex] };
		}

		private static Rotation RotationCheck(List<Vector2> points, Vector2 centre)
		{

			Rotation rot;

			if (((points[0].x - centre.x) * (points[points.Count - 1].y - centre.y) - (points[0].y - centre.y) * (points[points.Count - 1].x - centre.x)) > 0)
			{
				rot = Rotation.Clockwise;
			}
			else
			{
				rot = Rotation.Anticlockwise;
			}

			return rot;
		}
	}

	public class Segment
	{
		public List<Vector2> points;
	}


}

