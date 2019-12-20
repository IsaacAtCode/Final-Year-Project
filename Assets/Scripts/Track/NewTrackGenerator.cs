using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IsaacFagg.Paths;


namespace IsaacFagg.Tracks
{
	[ExecuteInEditMode]
	public class NewTrackGenerator : MonoBehaviour
	{
		public bool generateNewTrack = false;

		[Header("Random")]
		private float minWidth = 50;
		private float minHeight = 50;

		private float maxWidth = 500;
		private float maxHeight = 500;

		public float difficulty = 1;

		[Header("Convex Hull")]
		private List<Vector2> randomPoints;
		private List<Vector2> hullPoints;
		public List<Vector2> allPoints;

		[Header("Road")]
		public Material roadMat;
		public Material gravelMat;

		[Header("Checkpoint")]
		public GameObject checkpointGO;
		public float checkpointSpacing;

		private void Update()
		{
			if (generateNewTrack == true)
			{
				GenerateNewTrack(TrackType.Random);
				generateNewTrack = false;
			}
		}


		public void GenerateNewTrack(TrackType type)
		{
			Track track = new Track();
			track.trackType = type;


			if (type == TrackType.Random)
			{
				//Generate Random Width/Height
				float randomWidth = Random.Range(minWidth, maxWidth);
				float randomHeight = Random.Range(minHeight,maxHeight);
				track.width = randomWidth;
				track.height = randomHeight;
				randomPoints = GenerateRandomPoints(20, randomWidth,randomHeight);

				GenerateConvexHull();
				GenerateMidpoints();

			}
			else if (type == TrackType.PlayerData)
			{
				//GetPlayerData
				//GenerateNewTrackBasedOnThat
			}

			GenerateGameObjects();


		}

		#region Generation

		public void GenerateGameObjects()
		{
			//Random Track Name Generator
			GameObject trackGO = new GameObject("New Track");
			GameObject gravelGO = new GameObject("Gravel");
			gravelGO.transform.parent = trackGO.transform;

			PathCreator tPC = trackGO.AddComponent<PathCreator>();
			PathCreator gPC = gravelGO.AddComponent<PathCreator>();

			GenerateBevierPath(tPC);
			gPC.path = tPC.path;
			AddRoad(trackGO, roadMat,20f);
			AddRoad(gravelGO,gravelMat, 30f);

			GenerateCheckpoints(tPC.path, trackGO);
		}

		public void AddRoad(GameObject go, Material mat, float width)
		{
			RoadCreator rc = go.AddComponent<RoadCreator>();
			go.GetComponent<MeshRenderer>().material = mat;

			AddCollider(go);

			rc.roadWidth = width;
			rc.UpdateRoad();

		}

		private void AddCollider(GameObject go)
		{
			go.AddComponent<Mesh2DColliderMaker>();

			go.GetComponent<PolygonCollider2D>().isTrigger = true;
		}

		public void GenerateBevierPath(PathCreator pc)
		{

			if (allPoints.Count > 1)
			{
				pc.path.MovePoint(0, allPoints[0]);

				pc.path.MovePoint(3, allPoints[1]);



				for (int i = 0; i < allPoints.Count - 2; i++)
				{
					//Vector2 anchorPos = new Vector2(allPoints[i + 2].x, allPoints[i + 2].y);

					pc.path.AddSegment(allPoints[i + 2]);
				}
			}

			pc.path.IsClosed = true;
			pc.path.AutoSetControlPoints = true;
		}

#endregion 


		#region Random Generation

		public List<Vector2> GenerateRandomPoints(int count, float width, float height)
		{
			List<Vector2> points = new List<Vector2>(count);

			for (int i = 0; i < count; i++)
			{
				float x = Random.Range(0.0f, width) - (width / 2);
				float y = Random.Range(0.0f, height) - (height / 2);

				Vector2 vector = new Vector2(x, y);

				points.Add(vector);
			}

			return points;
		}

		private void GenerateConvexHull()
		{
			hullPoints = new List<Vector2>();

			int leftMostIndex = 0;
			for (int i = 1; i < randomPoints.Count; i++)
			{
				if (randomPoints[leftMostIndex].x > randomPoints[i].x)
					leftMostIndex = i;
			}
			hullPoints.Add(randomPoints[leftMostIndex]);
			List<Vector2> collinearPoints = new List<Vector2>();
			Vector2 current = randomPoints[leftMostIndex];
			while (true)
			{
				Vector2 nextTarget = randomPoints[0];
				for (int i = 1; i < randomPoints.Count; i++)
				{
					if (randomPoints[i] == current)
						continue;
					float x1, x2, y1, y2;
					x1 = current.x - nextTarget.x;
					x2 = current.x - randomPoints[i].x;

					y1 = current.y - nextTarget.y;
					y2 = current.y - randomPoints[i].y;

					float val = (y2 * x1) - (y1 * x2);
					if (val > 0)
					{
						nextTarget = randomPoints[i];
						collinearPoints = new List<Vector2>();
					}
					else if (val == 0)
					{
						if (Vector2.Distance(current, nextTarget) < Vector2.Distance(current, randomPoints[i]))
						{
							collinearPoints.Add(nextTarget);
							nextTarget = randomPoints[i];
						}
						else
							collinearPoints.Add(randomPoints[i]);
					}
				}

				foreach (Vector2 t in collinearPoints)
					hullPoints.Add(t);
				if (nextTarget == randomPoints[leftMostIndex])
					break;
				hullPoints.Add(nextTarget);
				current = nextTarget;
			}
		}

		private void GenerateMidpoints()
		{
			allPoints = new List<Vector2>(hullPoints.Count * 2);


			for (int i = 0; i < hullPoints.Count - 1; i++)
			{
				float newX = Midpoint(hullPoints[i].x, hullPoints[i + 1].x);
				float newY = Midpoint(hullPoints[i].y, hullPoints[i + 1].y);

				float diffX = (hullPoints[i].x - hullPoints[i + 1].x) * difficulty;
				float diffY = (hullPoints[i].y - hullPoints[i + 1].y) * difficulty;

				Vector2 newPoint = new Vector2(newX, newY);

				newPoint.x += Random.Range(-diffX, diffX);
				newPoint.y += Random.Range(-diffY, diffY);


				allPoints.Insert((i * 2), hullPoints[i]);
				allPoints.Insert((i * 2+1), newPoint);
			}
		}

		private float Midpoint(float x, float y)
		{
			return (x + y) / 2;
		}

		public Vector2 RotateVector(Vector2 v, float angle)
		{
			float radian = angle * Mathf.Deg2Rad;
			float _x = v.x * Mathf.Cos(radian) - v.y * Mathf.Sin(radian);
			float _y = v.x * Mathf.Sin(radian) + v.y * Mathf.Cos(radian);
			return new Vector2(_x, _y);
		}

		



		#endregion

		#region Player Data Generation

		#endregion

		//Checkpoints
		private void GenerateCheckpoints(Path path, GameObject go)
		{
			List<Checkpoint> checkpoints = new List<Checkpoint>();
			List<Vector2> checkpointLocations = new List<Vector2>(allPoints);

			GameObject checkpointParent = new GameObject("Checkpoints");
			checkpointParent.transform.parent = go.transform;

			for (int i = 0; i < checkpointLocations.Count; i++)
			{
				Vector3 newPos = new Vector3(checkpointLocations[i].x, checkpointLocations[i].y, 0);

				GameObject checkpoint = Instantiate(checkpointGO, checkpointParent.transform);

				checkpoint.name = ("Checkpoint: " + i);
				checkpoint.transform.position = newPos;
				Checkpoint cp = checkpoint.GetComponent<Checkpoint>();
				cp.position = i;

				if (i == 0)
				{
					cp.finishLine = true;
				}

				Vector2 anchor = new Vector2(path.points[i * 3 + 1].x, path.points[i * 3 + 1].y);
				float angle = Mathf.Atan2(anchor.y, anchor.x) * Mathf.Rad2Deg;
				checkpoint.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);







			}

		}
		//Obstacles
		private void GenerateObstacles()
		{

		}
	}

	public enum TrackType
	{
		Random,
		PlayerData,
	}
}
