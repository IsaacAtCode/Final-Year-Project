using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IsaacFagg.Paths;
using IsaacFagg.Track;
using IsaacFagg.Utility;

namespace IsaacFagg.Icons
{
	[RequireComponent(typeof(PathCreator))]
	public class IconGenerator : MonoBehaviour
	{
		public Material mat;

		public float minimapScale = 0.5f;

		public void GenerateTrackMesh(List<Vector2> points)
		{
			List<Vector2> scaledPoints = TrackUtility.ScaledPoints(points, 32, 32);

			PathCreator pc = GetComponent<PathCreator>();

			Path path = GenerateBevierPath(scaledPoints);

			pc.path = path;

			AddMesh(mat, minimapScale);

		}

		private Path GenerateBevierPath(List<Vector2> points)
		{
			Path newPath = new Path(points);


			newPath.IsClosed = true;
			newPath.AutoSetControlPoints = true;

			return newPath;
		}

		private void AddMesh(Material mat, float width)
		{
			RoadCreator rc = gameObject.AddComponent<RoadCreator>();
			MeshRenderer mr = GetComponent<MeshRenderer>();
			mr.material = mat;

			rc.roadWidth = width;
			rc.UpdateRoad();
		}

	}
}
