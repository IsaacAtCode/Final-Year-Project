using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IsaacFagg.Paths;


namespace IsaacFagg.Tracks
{
	[ExecuteInEditMode]
	public class TrackGenerator : MonoBehaviour
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

        public float minDistance = 50;

		[Header("Road")]
		public Material roadMat;
		public Material gravelMat;

		[Header("Checkpoint")]
		public GameObject checkpointGO;
		public float checkpointSpacing;

		[Header("Track Info")]
		public float tDistance;
		public string tName;

		public float maxDegs = 100;


		private void Update()
		{
			if (generateNewTrack == true)
			{
                GameObject[] oldTrack = GameObject.FindGameObjectsWithTag("CurrentTrack");

                foreach (GameObject item in oldTrack)
                {
                    DestroyImmediate(item);
                }
                
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
				allPoints = GenerateMidpoints(hullPoints);

				//for (int i = 0; i < 10; i++)
				//{
				//	FixAngles(allPoints);
				//}

			}
			else if (type == TrackType.PlayerData)
			{
				//GetPlayerData
				//GenerateNewTrackBasedOnThat
			}

			GenerateGameObjects(tName);

			tDistance = CalculateDistance(allPoints);

		}

		#region Generation

		public void GenerateGameObjects(string trackName)
		{
			//Random Track Name Generator
			GameObject trackGO = new GameObject(trackName);
			GameObject gravelGO = new GameObject("Gravel");
			gravelGO.transform.parent = trackGO.transform;
            trackGO.tag = ("CurrentTrack");
            gravelGO.tag = ("CurrentTrack");

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

		private List<Vector2> GenerateMidpoints(List<Vector2> inputPoints)
		{
			List<Vector2> outputPoints = new List<Vector2>(inputPoints.Count * 2);


			for (int i = 0; i < inputPoints.Count - 1; i++)
			{
                if (Vector2.Distance(inputPoints[i],inputPoints[i+1]) > minDistance)
                {

                    outputPoints.Add(inputPoints[i]);

                    //Chnage Range and range
                    float newX = Midpoint(inputPoints[i].x, inputPoints[i + 1].x);
                    float newY = Midpoint(inputPoints[i].y, inputPoints[i + 1].y);

                    float diffX = (inputPoints[i].x - inputPoints[i + 1].x) * difficulty;
                    float diffY = (inputPoints[i].y - inputPoints[i + 1].y) * difficulty;

                    Vector2 newPoint = new Vector2(newX, newY);

                    newPoint.x += Random.Range(-diffX, diffX);
                    newPoint.y += Random.Range(-diffY, diffY);

                    GameObject MidpointGO = new GameObject("Midpoint");
                    MidpointGO.transform.position = newPoint;
                    MidpointGO.tag = ("CurrentTrack");

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

		public Vector2 RotateVector(Vector2 v, float angle)
		{
			float radian = angle * Mathf.Deg2Rad;
			float _x = v.x * Mathf.Cos(radian) - v.y * Mathf.Sin(radian);
			float _y = v.x * Mathf.Sin(radian) + v.y * Mathf.Cos(radian);
			return new Vector2(_x, _y);
		}

		private void FixAngles(List<Vector2> points)
		{
			for (int i = 0; i < points.Count; ++i)
			{
				int previous = (i - 1 < 0) ? points.Count - 1 : i - 1;
				int next = (i + 1) % points.Count;
				float px = points[i].x - points[previous].x;
				float py = points[i].y - points[previous].y;
				float pl = (float)Mathf.Sqrt(px * px + py * py);
				px /= pl;
				py /= pl;

				float nx = points[i].x - points[next].x;
				float ny = points[i].y - points[next].y;
				nx = -nx;
				ny = -ny;
				float nl = (float)Mathf.Sqrt(nx * nx + ny * ny);
				nx /= nl;
				ny /= nl;
				//I got a vector going to the next and to the previous points, normalised.  

				float a = (float)Mathf.Atan2(px * ny - py * nx, px * nx + py * ny); // perp dot product between the previous and next point. Google it you should learn about it!  

				if (Mathf.Abs(a * Mathf.Rad2Deg) <= 100) continue;

				float nA = maxDegs * Mathf.Sign(a) * Mathf.Deg2Rad;
				float diff = nA - a;
				float cos = (float)Mathf.Cos(diff);
				float sin = (float)Mathf.Sin(diff);
				float newX = nx * cos - ny * sin;
				float newY = nx * sin + ny * cos;
				newX *= nl;
				newY *= nl;

				Vector2 nextV = new Vector2(points[i].x + newX, points[i].y + newY);


				points[next] = nextV;


				//I got the difference between the current angle and 100degrees, and built a new vector that puts the next point at 100 degrees.  
			}
		}



		#endregion

		#region Player Data Generation

		#endregion

		//Checkpoints
		private void GenerateCheckpoints(Path path, GameObject go)
		{
			//Invisible Checkpoints

			List<Checkpoint> checkpoints = new List<Checkpoint>();
			List<Vector2> checkpointLocations = new List<Vector2>(allPoints);

			GameObject checkpointParent = new GameObject("Checkpoints");
			checkpointParent.transform.parent = go.transform;

			for (int i = 0; i < checkpointLocations.Count; i++)
			{

				Vector3 newPos = checkpointLocations[i];

				GameObject checkpoint = Instantiate(checkpointGO, checkpointParent.transform);

				checkpoint.name = ("Checkpoint: " + i);
				checkpoint.transform.position = newPos;
                checkpoint.tag = ("CurrentTrack");
                Checkpoint cp = checkpoint.GetComponent<Checkpoint>();
				cp.position = i;

				SpriteRenderer sprite = checkpoint.GetComponent<SpriteRenderer>();

				if (i == 0)
				{
					cp.finishLine = true;
					sprite.enabled = true;
				}
				//else if (i>0)
				//{
				//	sprite.enabled = false;
				//}

				Vector2 anchor = new Vector2(path.points[i * 3 + 1].x, path.points[i * 3 + 1].y);
				float angle = Mathf.Atan2(anchor.y, anchor.x) * Mathf.Rad2Deg;
				checkpoint.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);




			}

		}
		//Obstacles
		private void GenerateObstacles(int spawnRate)
		{

		}

		private float CalculateDistance(List<Vector2> points)
		{
			float distance = 0;

			for (int i = 0; i < points.Count - 1; i++)
			{
				distance += Vector2.Distance(points[i], points[i + 1]);
			}

			return distance;
		}



	}

	public enum TrackType
	{
		Random,
		PlayerData,
	}
}
