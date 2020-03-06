using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IsaacFagg.Utility;

namespace IsaacFagg.Track
{ 
    public static class RandomTrackGenerator
    {
		public static int minPoints = 5;
		public static int maxPoints = 40;
		public static float minDistance = 50f;
		public static float minHW = 250f;
		public static float maxHW = 1250f;

		public static TrackData GenerateRandomTrack()
        {
            float randomHeight = Random.Range(minHW, maxHW);
            float randomWidth = Random.Range(minHW, maxHW);

            float randheight = randomHeight;
            float randwidth = randomWidth;

			List<Vector2> randomPoints = GenerateMidpoints(GenerateConvexHull(GenerateRandomPoints(Random.Range(minPoints, maxPoints), randheight, randwidth)), 1);

			List<Vector2> points =  TrackUtility.ScaledPoints(randomPoints, randomPoints.Count);

			TrackData track = new TrackData(points, TrackUtility.RandomRotation());

			return track;
        }

        private static List<Vector2> GenerateRandomPoints(int count, float height, float width)
        {
            List<Vector2> points = new List<Vector2>(count);

            for (int i = 0; i < count; i++)
            {
                float x = Random.Range(-width * 0.35f, width * 0.35f);
                float y = Random.Range(-height * 0.35f, height * 0.35f);

                Vector2 vector = new Vector2(x, y);

                points.Add(vector);
            }

            return points;
        }

		private static List<Vector2> GenerateConvexHull(List<Vector2> inputPoints)
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

		private static List<Vector2> GenerateMidpoints(List<Vector2> inputPoints, float difficulty)
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

		private static float Midpoint(float x, float y)
		{
			return (x + y) / 2;
		}
	}
}
