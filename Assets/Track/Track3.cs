// Made by Isaac Fagg
// Final Year Project
// 30/01/2020

using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IsaacFagg.Paths;
using IsaacFagg.Utility;

namespace IsaacFagg.Track3
{
	public class Track3 : MonoBehaviour
	{
		public new string name;

		public TrackType type;
		private bool pointsGenerated = false;


		[Header("Properties - Essential")]
		[Range(0.1f, 5f)]
		public float difficulty = 1;
		public float calculatedDifficulty;

		public List<Vector2> points;
		public Path path;
		private List<float> distanceBetweenPoints;
		public Rotation rotation;

		public Vector2 centre = Vector2.zero;
		

		[Header("Properties - Non Essential")]
		public float length;
		public int straights;
		public int corners;
		public float totalAngles;
		public float height;
		public float width;

		[Header("Constraints")]
		public int minPoints;
		public int maxPoints;
		public float minDistance = 50f;
		public float minHeight;
		public float maxHeight;
		public float minWidth;
		public float maxWidth;

		[Header("Checks")]
		public float maxLength;
		public int minStraights = 1;
		public float minStraightLength = 100f;


		public int retries = 0;

		private void Start()
		{
			retries = 0;
		}

		public void GenerateTrack()
		{
			//if (!pointsGenerated)
			//{
			if (type == TrackType.Random)
			{
				RandomTrack();
			}
			else if (type == TrackType.Human)
			{
				//Need input of points
			}
			else if (type == TrackType.PlayerData)
			{
				//Get Player file
			}
			//}


			//Measurements
			GetCentre();
			GetLength();
			GetTrackSize();
			GetStraights();
			GetCorners();


			//Checks
		}


		#region Random Track Generation

		private void RandomTrack()
		{
			float randomHeight = Random.Range(minHeight, maxHeight);
			float randomWidth = Random.Range(minWidth, maxWidth);

			height = randomHeight;
			width = randomWidth;

			points = GenerateMidpoints(GenerateConvexHull(GenerateRandomPoints(Random.Range(minPoints, maxPoints), height, width)));

			rotation = (Rotation)Random.Range(0,2);

			//pointsGenerated = true;
		}

		//Straightforward Point Generation
		private List<Vector2> GenerateRandomPoints(int count, float height, float width)
		{
			List<Vector2> points = new List<Vector2>(count);

			for (int i = 0; i < count; i++)
			{
				float x = Random.Range(0.0f, width * 0.75f);
				float y = Random.Range(0.0f, height * 0.75f);

				Vector2 vector = new Vector2(x, y);

				points.Add(vector);
			}

			return points;
		}

		private List<Vector2> GenerateConvexHull(List<Vector2> inputPoints)
		{
			List<Vector2> outputPoints = new List<Vector2>();

			int leftMostIndex = 0;
			for (int i = 1; i < inputPoints.Count; i++)
			{
				if (inputPoints[leftMostIndex].x > inputPoints[i].x)
					leftMostIndex = i;
			}
			outputPoints.Add(inputPoints[leftMostIndex]);
			List<Vector2> collinearPoints = new List<Vector2>();
			Vector2 current = inputPoints[leftMostIndex];
			while (true)
			{
				Vector2 nextTarget = inputPoints[0];
				for (int i = 1; i < inputPoints.Count; i++)
				{
					if (inputPoints[i] == current)
						continue;
					float x1, x2, y1, y2;
					x1 = current.x - nextTarget.x;
					x2 = current.x - inputPoints[i].x;

					y1 = current.y - nextTarget.y;
					y2 = current.y - inputPoints[i].y;

					float val = (y2 * x1) - (y1 * x2);
					if (val > 0)
					{
						nextTarget = inputPoints[i];
						collinearPoints = new List<Vector2>();
					}
					else if (val == 0)
					{
						if (Vector2.Distance(current, nextTarget) < Vector2.Distance(current, inputPoints[i]))
						{
							collinearPoints.Add(nextTarget);
							nextTarget = inputPoints[i];
						}
						else
							collinearPoints.Add(inputPoints[i]);
					}
				}

				foreach (Vector2 t in collinearPoints)
					outputPoints.Add(t);
				if (nextTarget == inputPoints[leftMostIndex])
					break;
				outputPoints.Add(nextTarget);
				current = nextTarget;
			}

			return outputPoints;
		}

		private List<Vector2> GenerateMidpoints(List<Vector2> inputPoints)
		{
			List<Vector2> outputPoints = new List<Vector2>();


			for (int i = 0; i < inputPoints.Count - 1; i++)
			{
				if (Vector2.Distance(inputPoints[i], inputPoints[i + 1]) > minDistance)
				{
					outputPoints.Add(inputPoints[i]);

					float newX = Midpoint(inputPoints[i].x, inputPoints[i + 1].x);
					float newY = Midpoint(inputPoints[i].y, inputPoints[i + 1].y);

					float diffX = (inputPoints[i].x - inputPoints[i + 1].x) * difficulty;
					float diffY = (inputPoints[i].y - inputPoints[i + 1].y) * difficulty;

					Vector2 newPoint = new Vector2(newX, newY);

					newPoint.x += Random.Range(-diffX, diffX);
					newPoint.y += Random.Range(-diffY, diffY);

					outputPoints.Add(newPoint);
				}
				else
				{
					outputPoints.Add(inputPoints[i]);
				}
			}

			return outputPoints;
		}

		private float Midpoint(float x, float y)
		{
			return (x + y) / 2;
		}

		#endregion

		#region Measurements

		private void GetCentre()
		{
			centre = Vector2.zero;

			foreach (Vector2 item in points)
			{
				centre += item;
			}

			centre /= points.Count;
		}


		private void GetLength()
		{
			length = 0;

			for (int i = 0; i < points.Count - 1; i++)
			{
				length += Vector2.Distance(points[i], points[i + 1]);
			}
			length += Vector2.Distance(points[points.Count-1], points[0]);
		}

		private void GetTrackSize()
		{
			float minX = 0;
			float minY = 0;

			float maxX = 0;
			float maxY = 0;

			for (int i = 0; i < points.Count; i++)
			{
				if (points[i].x < minX)
				{
					minX = points[i].x;
				}
				if (points[i].y < minY)
				{
					minY = points[i].y;
				}
				if (points[i].x > maxX)
				{
					maxX = points[i].x;
				}
				if (points[i].y > maxY)
				{
					maxY = points[i].y;
				}
			}

			height = CalculateDifference(minY, maxY);
			width = CalculateDifference(minX, maxX);
		}

		private void GetStraights()
		{
			straights = 0;

			for (int i = 0; i < points.Count - 1; i++)
			{
				if (Vector2.Distance(points[i], points[i+1]) > minStraightLength)
				{
					straights++;
				}
			}

		}

		private void GetCorners()
		{
			corners = 0;

			corners = points.Count - straights;
		}


		private float CalculateDifference(float x, float y)
		{
			return Mathf.Abs(x - y);
		}

		#endregion






	}

	public enum Rotation
	{
		Clockwise,
		Anticlockwise,
	}

	public enum TrackType
	{
		Random,
		Human,
		PlayerData,
	}
}
