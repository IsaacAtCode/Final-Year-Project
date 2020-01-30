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
		public Sprite background;

		[Header("Checkpoint")]
		public List<Checkpoint> checkpoints = new List<Checkpoint>();
		public GameObject checkpointGO;
		public float checkpointSpacing;

		[Header("Track Info")]
		public GameObject trackGO;
		public Track track;

		public float tDistance;
		public string tName;

		public float maxDegs = 100;


		private void Update()
		{
			if (generateNewTrack == true)
			{
				generateNewTrack = false;
				GameObject[] oldTrack = GameObject.FindGameObjectsWithTag("CurrentTrack");

				foreach (GameObject item in oldTrack)
				{
					DestroyImmediate(item);
				}
				
				GenerateNewTrack();

			}
		}


		public void GenerateNewTrack()
		{

			GenerateGameObjects(tName);

			tDistance = Mathf.RoundToInt(CalculateDistance(allPoints));

			CheckpointCollisionCheck();

			

		}

		#region Mesh Generation

		public void GenerateGameObjects(string trackName)
		{
			//Random Track Name Generator
			trackGO = new GameObject(trackName);
			GameObject gravelGO = new GameObject("Gravel");
			gravelGO.transform.parent = trackGO.transform;
			trackGO.tag = ("CurrentTrack");

			PathCreator tPC = trackGO.AddComponent<PathCreator>();
			PathCreator gPC = gravelGO.AddComponent<PathCreator>();

			GenerateBevierPath(tPC);
			gPC.path = tPC.path;
			AddRoad(trackGO, roadMat,20f);
			AddRoad(gravelGO,gravelMat, 30f);
			AddBackground(background);

			GenerateCheckpoints(tPC.path, trackGO);
		}

		public void AddRoad(GameObject go, Material mat, float width)
		{
			RoadCreator rc = go.AddComponent<RoadCreator>();
			MeshRenderer mr = go.GetComponent<MeshRenderer>();
			mr.material = mat;
			mr.sortingLayerName = "Track";
			mr.sortingOrder = 0;

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

		private void AddBackground(Sprite backgroundSprite)
		{
			GameObject oldBackground = GameObject.Find("Background");
			DestroyImmediate(oldBackground);
	

			GameObject background = new GameObject("Background");
			background.layer = (12);
			background.transform.position = new Vector3(0,0,30f);
			SpriteRenderer spriteRender =  background.AddComponent<SpriteRenderer>();
			spriteRender.sprite = backgroundSprite;
			spriteRender.drawMode = SpriteDrawMode.Tiled;
			spriteRender.size = new Vector2(maxWidth, maxHeight) * 1.5f;

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
			List<Vector2> outputPoints = new List<Vector2>();


			for (int i = 0; i < inputPoints.Count - 1; i++)
			{
				if (Vector2.Distance(inputPoints[i],inputPoints[i+1]) > minDistance)
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


		#region General Generation

		//Checkpoints
		private void GenerateCheckpoints(Path path, GameObject go)
		{
			checkpoints.Clear();

			//Invisible Checkpoints
			List<Vector2> checkpointLocations = new List<Vector2>(allPoints);

			GameObject checkpointParent = new GameObject("Checkpoints");
			checkpointParent.transform.parent = go.transform;

			for (int i = 0; i < checkpointLocations.Count; i++)
			{

				Vector3 newPos = checkpointLocations[i];

				GameObject checkpoint = Instantiate(checkpointGO, checkpointParent.transform);

				checkpoint.name = ("Checkpoint: " + i);
				checkpoint.transform.position = newPos;
				Checkpoint cp = checkpoint.GetComponent<Checkpoint>();
				checkpoints.Add(cp);
;				cp.position = i;

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

				Vector3 targetPos = new Vector3(path.points[i * 3 + 1].x, path.points[i * 3 + 1].y, 0);
				Vector3 difference = targetPos - checkpoint.transform.position;
				float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
				checkpoint.transform.rotation = Quaternion.Euler(0f, 0f, rotZ + 90f);

			}

		}
		//Obstacles
		private void GenerateObstacles(int spawnRate)
		{

		}

		private void GeneratePlayerSpawnpoint()
		{
			GameObject playerSpawn = new GameObject("Player Spawn");
			playerSpawn.tag = "Player Spawn";
			playerSpawn.transform.position = checkpoints[0].gameObject.transform.position;
		}

		private void SaveTrack()
		{

		}




		#endregion

		#region Checks

		//private Rotation RotationCheck(List<Vector2> points)
		//{

		//	Rotation rot = Rotation.Clockwise;
		//	Vector2 centre = Vector2.zero;

		//	foreach (Vector2 item in points)
		//	{
		//		centre += item;
		//	}

		//	centre /= points.Count;

		//	if (((points[0].x - centre.x) * (points[points.Count - 1].y - centre.y) - (points[0].y - centre.y) * (points[points.Count - 1].x - centre.x)) > 0)
		//	{
		//		rot = Rotation.Clockwise;
		//	}
		//	else
		//	{
		//		rot = Rotation.Anticlockwise;
		//	}

		//	return rot;
		//}

		private float CalculateDistance(List<Vector2> points)
		{
			float distance = 0;

			for (int i = 0; i < points.Count - 1; i++)
			{
				distance += Vector2.Distance(points[i], points[i + 1]);
			}

			return distance;
		}


		List<KeyValuePair<BoxCollider2D, BoxCollider2D>> usedCollider = new List<KeyValuePair<BoxCollider2D, BoxCollider2D>>();
		public List<BoxCollider2D> boxColliders = new List<BoxCollider2D>();

		private void CheckpointCollisionCheck()
		{
			boxColliders.Clear();


			foreach (Checkpoint item in checkpoints)
			{
				boxColliders.Add(item.GetComponent<BoxCollider2D>());
			}

			for (int i = 0; i < boxColliders.Count; i++)
			{
				CollisionCheck(i, ref usedCollider);
			}
		}

		private void CollisionCheck(int currentIndex, ref List<KeyValuePair<BoxCollider2D, BoxCollider2D>> usedCollider)
		{
			for (int i = 0; i < boxColliders.Count; i++)
			{
				//Make sure that this two Colliders are not the-same
				if (boxColliders[currentIndex] != boxColliders[i])
				{
					//Now, make sure we have not checked between this 2 Objects
					if (!checkedBefore(usedCollider, boxColliders[currentIndex], boxColliders[i]))
					{
						if (boxColliders[currentIndex].IsTouching(boxColliders[i]))
						{
							//FINALLY, COLLISION IS DETECTED HERE, call ArrayCollisionDetection
							ArrayCollisionDetection(boxColliders[currentIndex], boxColliders[i]);
						}
						//Mark it checked now
						usedCollider.Add(new KeyValuePair<BoxCollider2D, BoxCollider2D>(boxColliders[currentIndex], boxColliders[i]));
					}
				}
			}
		}

		bool checkedBefore(List<KeyValuePair<BoxCollider2D, BoxCollider2D>> usedCollider, BoxCollider2D col1, BoxCollider2D col2)
		{
			bool checkedBefore = false;
			for (int i = 0; i < usedCollider.Count; i++)
			{
				//Check if key and value exist and vice versa
				if ((usedCollider[i].Key == col1 && usedCollider[i].Value == col2) ||
						(usedCollider[i].Key == col2 && usedCollider[i].Value == col1))
				{
					checkedBefore = true;
					break;
				}
			}
			return checkedBefore;
		}

		void ArrayCollisionDetection(Collider2D col1, Collider2D col2)
		{
			Debug.Log(col1.name + " is Touching " + col2.name);
		}



		#endregion

	}


}
