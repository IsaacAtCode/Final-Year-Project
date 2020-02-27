using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IsaacFagg;

public static class Bezier
{
	public static Vector2 EvaluateQuadratic(List<Vector2> points, float t)
	{
		Vector2 p0 = Vector2.Lerp(points[0], points[1], t);
		Vector2 p1 = Vector2.Lerp(points[1], points[2], t);
		return Vector2.Lerp(p0, p1, t);
	}
	public static Vector2 EvaluateQuadratic(Vector2 a, Vector2 b, Vector2 c, float t)
	{
		Vector2 p0 = Vector2.Lerp(a, b, t);
		Vector2 p1 = Vector2.Lerp(b, c, t);
		return Vector2.Lerp(p0, p1, t);
	}

	public static Vector2 EvaluateCubic(List<Vector2> points, float t)
	{
		return EvaluateCubic(points[0], points[1], points[2], points[3], t);
	}

	public static Vector2 EvaluateCubic(Vector2 a, Vector2 b, Vector2 c, Vector2 d, float t)
	{
		t = Mathf.Clamp01(t);
		Vector2 p0 = EvaluateQuadratic(a, b, c, t);
		Vector2 p1 = EvaluateQuadratic(b, c, d, t);
		return Vector2.Lerp(p0, p1, t);
	}

	public static Bounds CalculateSegmentBounds(List<Vector2> points)
	{
		if (points.Count == 3)
		{
			return CalculateSegmentBounds(points[0], points[1], points[2]);
		}
		else
		{
			return CalculateSegmentBounds(points[0], points[1], points[2], points[3]);
		}
	}


	public static Bounds CalculateSegmentBounds(Vector2 p0, Vector2 p1, Vector2 p2)
	{
		MinMax2D minMax = new MinMax2D();
		minMax.AddValue(p0);
		minMax.AddValue(p2);

		List<float> extremePointTimes = ExtremePointTimes(p0, p1, p2);
		foreach (float t in extremePointTimes)
		{
			minMax.AddValue(EvaluateQuadratic(p0, p1, p2, t));
		}

		return new Bounds((minMax.Min + minMax.Max) / 2, minMax.Max - minMax.Min);
	}


	public static Bounds CalculateSegmentBounds(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
	{
		MinMax2D minMax = new MinMax2D();
		minMax.AddValue(p0);
		minMax.AddValue(p3);

		List<float> extremePointTimes = ExtremePointTimes(p0, p1, p2, p3);
		foreach (float t in extremePointTimes)
		{
			minMax.AddValue(EvaluateCubic(p0, p1, p2, p3, t));
		}

		return new Bounds((minMax.Min + minMax.Max) / 2, minMax.Max - minMax.Min);
	}

	public static List<float> ExtremePointTimes(Vector2 p0, Vector2 p1, Vector2 p2)
	{
		// coefficients of derivative function
		Vector2 a = 3 * (-p0 + 3 * p1 - 3 * p2);
		Vector2 b = 6 * (p0 - 2 * p1 + p2);
		Vector2 c = 3 * (p1 - p0);

		List<float> times = new List<float>();
		times.AddRange(StationaryPointTimes(a.x, b.x, c.x));
		times.AddRange(StationaryPointTimes(a.y, b.y, c.y));
		return times;
	}

	public static List<float> ExtremePointTimes(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
	{
		// coefficients of derivative function
		Vector2 a = 3 * (-p0 + 3 * p1 - 3 * p2 + p3);
		Vector2 b = 6 * (p0 - 2 * p1 + p2);
		Vector2 c = 3 * (p1 - p0);

		List<float> times = new List<float>();
		times.AddRange(StationaryPointTimes(a.x, b.x, c.x));
		times.AddRange(StationaryPointTimes(a.y, b.y, c.y));
		return times;
	}

	static IEnumerable<float> StationaryPointTimes(float a, float b, float c)
	{
		List<float> times = new List<float>();

		// from quadratic equation: y = [-b +- sqrt(b^2 - 4ac)]/2a
		if (a != 0)
		{
			float discriminant = b * b - 4 * a * c;
			if (discriminant >= 0)
			{
				float s = Mathf.Sqrt(discriminant);
				float t1 = (-b + s) / (2 * a);
				if (t1 >= 0 && t1 <= 1)
				{
					times.Add(t1);
				}

				if (discriminant != 0)
				{
					float t2 = (-b - s) / (2 * a);

					if (t2 >= 0 && t2 <= 1)
					{
						times.Add(t2);
					}
				}
			}
		}
		return times;
	}

}


