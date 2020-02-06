using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IsaacFagg.Utility;

namespace IsaacFagg.Track3
{
    public class Track3Check : MonoBehaviour
    {
		public List<Vector2> intersections;

		public bool TrackCheck(Track3 track)
		{
			//Length
			//Straights
			//Curves
			//Track Mesh

			//Intersections

			//Obstacles



			return true;
		}





		private Rotation RotationCheck(List<Vector2> points, Vector2 centre)
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



		public void OverlapCheck(List<Vector2> points)
		{
			intersections = new List<Vector2>();

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

			GameObject interParent = GameObject.Find("Intersections Parent");
			if (interParent == null)
			{
				interParent = new GameObject("Intersections Parent");
			}


			foreach (Vector2 point in intersections)
			{
				GameObject intersect = new GameObject("intersect");
				intersect.transform.parent = interParent.transform;

				Vector2 newPos = new Vector2(point.x, point.y);

				intersect.transform.position = newPos;

			}
		}
	}
}
