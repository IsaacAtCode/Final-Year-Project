﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IsaacFagg.Utility;

namespace IsaacFagg.Paths
{
	[System.Serializable]
	public class Path
	{
		[SerializeField, HideInInspector]
		public List<Vector2> points;
		[SerializeField, HideInInspector]
		bool isClosed = false;
		[SerializeField, HideInInspector]
		bool autoSetControlPoints = true;

		public Path(Vector2 centre)
		{
			points = new List<Vector2>
			{
				centre + Vector2.left, //Initial point
				centre + (Vector2.left+Vector2.up)*.5f,
				centre + (Vector2.right+Vector2.down)*.5f,
				centre + Vector2.right // Second Point
			};
		}

		public Path(List<Vector2> newPoints)
		{
			Vector2 centre = TrackUtility.GetCentre(newPoints);

			points = new List<Vector2>
			{
				centre + Vector2.left, //Initial point
				centre + (Vector2.left+Vector2.up)*.5f,
				centre + (Vector2.right+Vector2.down)*.5f,
				centre + Vector2.right // Second Point
			};

			if (newPoints.Count > 1)
			{
				MovePoint(0, newPoints[0]);

				MovePoint(3, newPoints[1]);

				for (int i = 2; i < newPoints.Count; i++)
				{
					//Vector2 anchorPos = new Vector2(allPoints[i + 2].x, allPoints[i + 2].y);

					AddSegment(newPoints[i]);
				}
			}

			IsClosed = true;
			AutoSetControlPoints = true;
		}

		public Vector2 this[int i]
		{
			get
			{
				return points[i];
			}
		}

		public bool IsClosed
		{
			get
			{
				return isClosed;
			}
			set
			{
				if (isClosed != value)
				{
					isClosed = value;

					if (isClosed)
					{
						points.Add(points[points.Count - 1] * 2 - points[points.Count - 2]);
						points.Add(points[0] * 2 - points[1]);
						if (autoSetControlPoints)
						{
							AutoSetAnchorControlPoints(0);
							AutoSetAnchorControlPoints(points.Count - 3);
						}
					}
					else
					{
						points.RemoveRange(points.Count - 2, 2);
						if (autoSetControlPoints)
						{
							AutoSetStartAndEndControls();
						}
					}
				}
			}
		}

		public bool AutoSetControlPoints
		{
			get
			{
				return autoSetControlPoints;
			}
			set
			{
				if (autoSetControlPoints != value)
				{
					autoSetControlPoints = value;
					if (autoSetControlPoints)
					{
						AutoSetAllControlPoints();
					}
				}
			}

		}

		public int NumPoints
		{
			get
			{
				return points.Count;
			}
		}

		public int NumSegments
		{
			get
			{
				return points.Count / 3;
			}
		}

		public void AddSegment(Vector2 anchorPos)
		{
			points.Add(points[points.Count - 1] * 2 - points[points.Count - 2]);
			points.Add((points[points.Count - 1] + anchorPos) * .5f);
			points.Add(anchorPos);

			if (autoSetControlPoints)
			{
				AutoSetAllAffectedControlPoints(points.Count - 1);
			}
		}

		public void SplitSegment(Vector2 anchorPos, int segmentIndex)
		{
			points.InsertRange(segmentIndex * 3 + 2, new Vector2[] { Vector2.zero, anchorPos, Vector2.zero });

			if (autoSetControlPoints)
			{
				AutoSetAllAffectedControlPoints(segmentIndex * 3 + 3);
			}
			else
			{
				AutoSetAnchorControlPoints(segmentIndex * 3 + 3);
			}
		}

		public void DeleteSegment(int anchorIndex)
		{
			if (NumSegments > 2 || !isClosed && NumSegments > 1)
			{
				if (anchorIndex == 0)
				{
					if (isClosed)
					{
						points[points.Count - 1] = points[2];
					}
					points.RemoveRange(0, 3);
				}
				else if (anchorIndex == points.Count - 1 && !isClosed)
				{
					points.RemoveRange(anchorIndex - 2, 3);
				}
				else
				{
					points.RemoveRange(anchorIndex - 1, 3);
				}
			}
		}

		public List<Vector2> GetPointsInSegment(int i)
		{
			return new List<Vector2> { points[i * 3], points[i * 3 + 1], points[i * 3 + 2], points[LoopIndex(i * 3 + 3)] };
		}

		public void MovePoint(int i, Vector2 pos)
		{
			Vector2 deltaMove = pos - points[i];

			if (i % 3 == 0 || !autoSetControlPoints)
			{
				points[i] = pos;


				if (autoSetControlPoints)
				{
					AutoSetAllAffectedControlPoints(i);
				}
				else
				{
					if (i % 3 == 0)
					{
						if (i + 1 < points.Count || isClosed)
						{
							points[LoopIndex(i + 1)] += deltaMove;
						}
						if (i - 1 >= 0 || isClosed)
						{
							points[LoopIndex(i - 1)] += deltaMove;
						}
					}
					else
					{
						bool nextPointIsAnchor = (i + 1) % 3 == 0;
						int correspondingControlIndex = (nextPointIsAnchor) ? i + 2 : i - 2;
						int anchorIndex = (nextPointIsAnchor) ? i + 1 : i - 1;

						if (correspondingControlIndex >= 0 && correspondingControlIndex < points.Count || isClosed)
						{
							float dst = (points[LoopIndex(anchorIndex)] - points[LoopIndex(correspondingControlIndex)]).magnitude;
							Vector2 dir = (points[LoopIndex(anchorIndex)] - pos).normalized;
							points[LoopIndex(correspondingControlIndex)] = points[LoopIndex(anchorIndex)] + dir * dst;
						}
					}
				}
			}
		}

		public List<Vector2> CalculateEvenlySpacedPoints(float spacing, float accuracy = 1)
		{
			List<Vector2> evenlySpacedPoints = new List<Vector2>();
			evenlySpacedPoints.Add(points[0]);

			Vector2 previousPoint = points[0];
			Vector2 lastAddedPoint = points[0];

			float currentPathLength = 0;
			float distSinceLastEvenPoint = 0;

			for (int segmentIndex = 0; segmentIndex < NumSegments; segmentIndex++)
			{
				List<Vector2> p = GetPointsInSegment(segmentIndex);
				float controlNetLength = (p[0] - p[1]).magnitude + (p[1] - p[2]).magnitude + (p[2] - p[3]).magnitude;
				float estimatedSegmentLength = (p[0] - p[3]).magnitude + controlNetLength / 2f;

				int divisions = Mathf.CeilToInt(estimatedSegmentLength * accuracy);
				float increment = 1f / divisions;

				for (float t = increment; t <= 1; t += increment)
				{
					bool isLastPointOnPath = (t + increment > 1 && segmentIndex == NumSegments - 1);
					if (isLastPointOnPath)
					{
						t = 1;
					}

					Vector2 pointOnCurve = Bezier.EvaluateCubic(p, t);
					distSinceLastEvenPoint += (pointOnCurve - previousPoint).magnitude;

					if (distSinceLastEvenPoint > spacing)
					{
						float overshootDist = distSinceLastEvenPoint - spacing;
						pointOnCurve += (previousPoint - pointOnCurve).normalized * overshootDist;
						t -= increment;
					}

					if (distSinceLastEvenPoint >= spacing || isLastPointOnPath)
					{
						currentPathLength += (lastAddedPoint - pointOnCurve).magnitude;
						evenlySpacedPoints.Add(pointOnCurve);
						distSinceLastEvenPoint = 0;
						lastAddedPoint = pointOnCurve;
					}

					previousPoint = pointOnCurve;
				}
			}
			return evenlySpacedPoints;
		}

		public List<Vector2> CalculateNumberOfPoints(float desiredCount)
		{
			Path path = new Path(points);
			float spacing = path.EstimatedLength() / 5;

			for (float t = spacing; t >= desiredCount; t -= 1f)
			{
				List<Vector2>  testingScaledPoints = path.CalculateEvenlySpacedPoints(t);

				if (testingScaledPoints.Count == desiredCount)
				{
					spacing = t;
					break;
				}
			}

			return path.CalculateEvenlySpacedPoints(spacing);

		}

		public float EstimatedLength()
		{
			float length = 0;

			for (int segmentIndex = 0; segmentIndex < NumSegments; segmentIndex++)
			{
				List<Vector2> p = GetPointsInSegment(segmentIndex);
				float controlNetLength = (p[0] - p[1]).magnitude + (p[1] - p[2]).magnitude + (p[2] - p[3]).magnitude;
				float estimatedCurveLength = (p[0] - p[3]).magnitude + controlNetLength / 2f;

				length += estimatedCurveLength;
			}

			return length;
		}

		void AutoSetAllAffectedControlPoints(int updatedAnchorIndex)
		{
			for (int i = updatedAnchorIndex - 3; i <= updatedAnchorIndex + 3;  i+=3)
			{
				if (i>=0 && i < points.Count || isClosed)
				{
					AutoSetAnchorControlPoints(LoopIndex(i));
				}
			}

			AutoSetStartAndEndControls();
		}


		void AutoSetAllControlPoints()
		{
			for (int i = 0; i < points.Count; i+=3)
			{
				AutoSetAnchorControlPoints(i);
			}

			AutoSetStartAndEndControls();
		}


		void AutoSetAnchorControlPoints(int anchorIndex)
		{
			Vector2 anchorPos = points[anchorIndex];
			Vector2 dir = Vector2.zero;
			float[] neighbourDistances = new float[2];

			if (anchorIndex - 3 >= 0 || isClosed)
			{
				Vector2 offset = points[LoopIndex(anchorIndex - 3)] - anchorPos;
				dir += offset.normalized;
				neighbourDistances[0] = offset.magnitude;
			}
			if (anchorIndex + 3 >= 0 || isClosed)
			{
				Vector2 offset = points[LoopIndex(anchorIndex + 3)] - anchorPos;
				dir -= offset.normalized;
				neighbourDistances[1] = -offset.magnitude;
			}

			dir.Normalize();

			for (int i = 0; i < 2; i++)
			{
				int controlIndex = anchorIndex + i * 2 - 1;
				if (controlIndex >=0 && controlIndex < points.Count || isClosed )
				{
					points[LoopIndex(controlIndex)] = anchorPos + dir * neighbourDistances[i] * .5f;
				}
			}
		}

		void AutoSetStartAndEndControls()
		{
			if (!isClosed)
			{
				points[1] = (points[0] + points[2]) * .5f;
				points[points.Count - 2] = (points[points.Count - 1] + points[points.Count - 3]) * .5f;
			}
		}
		int LoopIndex(int i)
		{
			return (i + points.Count) % points.Count;
		}

		public void PrintPoints()
		{
			foreach (Vector2 point in points)
			{
				Debug.Log(point.ToString());
			}
		}

	}
}
