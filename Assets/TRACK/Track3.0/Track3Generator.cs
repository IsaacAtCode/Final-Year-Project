// Made by Isaac Fagg
// Final Year Project
// 30/01/2020

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IsaacFagg.Paths;

namespace IsaacFagg.Track3
{
	[ExecuteInEditMode]
	public class Track3Generator : MonoBehaviour
	{
		[Header("Components")]
		public Track3 track;
		public RandomNameGenerator randomNameGenerator;

		[Header("Checkpoints")]
		public List<Checkpoint> checkpoints;
		public List<Vector2> checkpointLocations;
		public GameObject checkpointTemplate;
		public int checkpointCount = 40;

		[Header("Road")]
		public Material roadMat;
		public Material gravelMat;
		public Material minimapMat;
		public Sprite background;

		public bool generateNewTrack = false;

		private void Start()
		{
			if (!track)
			{
				track = GetComponent<Track3>();
			}
			if (!randomNameGenerator)
			{
				randomNameGenerator = GetComponent<RandomNameGenerator>();
			}
		}

		private void Update()
		{
			if (generateNewTrack)
			{
				track.GenerateTrack();

				if (track.type == TrackType.Random)
				{
					track.name = randomNameGenerator.GenerateName();
				}

				generateNewTrack = false;



				GenerateFromTrack();



			}
		}

		private void GenerateFromTrack()
		{
			DeleteOldTrack();

			GameObject trackGO = new GameObject(track.name);
			GameObject gravelGO = new GameObject("Gravel");
			GameObject minimapGO = new GameObject("Minimap Track");
			
			//Parents
			trackGO.transform.parent = this.gameObject.transform;
			gravelGO.transform.parent = trackGO.transform;

			//Tags
			trackGO.tag = "CurrentTrack";
			minimapGO.tag = "MinimapTrack";

			//Layers
			trackGO.layer = 10;
			gravelGO.layer = 10;
			minimapGO.layer = 11;


			PathCreator tPC = trackGO.AddComponent<PathCreator>();
			PathCreator gPC = gravelGO.AddComponent<PathCreator>();
			PathCreator mPC = minimapGO.AddComponent<PathCreator>();

			track.path = GenerateBevierPath();

			tPC.path = track.path;
			gPC.path = track.path;
			mPC.path = track.path;

			AddMesh(trackGO, roadMat, 20f);
			AddMesh(gravelGO, gravelMat, 30f);
			AddMesh(minimapGO, minimapMat, 50f);

			//AddBackground(background);

			GenerateCheckpoints(tPC.path, trackGO);
		}

		public Path GenerateBevierPath()
		{
			Path newPath = new Path(track.centre);


			if (track.points.Count > 1)
			{
				newPath.MovePoint(0, track.points[0]);

				newPath.MovePoint(3, track.points[1]);

				for (int i = 0; i < track.points.Count - 2; i++)
				{
					//Vector2 anchorPos = new Vector2(allPoints[i + 2].x, allPoints[i + 2].y);

					newPath.AddSegment(track.points[i + 2]);
				}
			}

			newPath.IsClosed = true;
			newPath.AutoSetControlPoints = true;

			return newPath;
		}

		public void AddMesh(GameObject go, Material mat, float width)
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

		private void AddBackground(Sprite backgroundSprite)
		{
			GameObject oldBackground = GameObject.Find("Background");
			DestroyImmediate(oldBackground);


			GameObject background = new GameObject("Background");
			background.layer = (12);
			background.transform.position = new Vector3(0, 0, 30f);
			SpriteRenderer spriteRender = background.AddComponent<SpriteRenderer>();
			spriteRender.sprite = backgroundSprite;
			spriteRender.drawMode = SpriteDrawMode.Tiled;
			spriteRender.size = new Vector2(track.width, track.height) * 1.5f;

		}

		private void GenerateCheckpoints(Path path, GameObject go)
		{
			checkpoints.Clear();

			//Invisible Checkpoints
			checkpointLocations = path.CalculateEvenlySpacedPoints(track.length/checkpointCount, 1);

			GameObject checkpointParent = new GameObject("Checkpoints");
			checkpointParent.transform.position = Vector2.zero;
			checkpointParent.transform.parent = go.transform;

			if (track.rotation == Rotation.Anticlockwise)
			{
				for (int i = checkpointLocations.Count - 1; i >= 0 ; i--)
				{
					CreateCheckpoint(i, checkpointParent.transform);
				}
			}
			else
			{
				for (int i = 0; i < checkpointLocations.Count; i++)
				{
					CreateCheckpoint(i, checkpointParent.transform);
				}
			}

			track.OverlapCheck(checkpointLocations);
		}

		private void CreateCheckpoint(int i, Transform parent)
		{
			Vector3 newPos = checkpointLocations[i];

			GameObject checkpoint = Instantiate(checkpointTemplate, parent);

			checkpoint.name = ("Checkpoint: " + i);
			checkpoint.transform.position = newPos;
			Checkpoint cp = checkpoint.GetComponent<Checkpoint>();
			checkpoints.Add(cp);
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
			Vector3 targetPos;

			if (i == 0)
			{
				targetPos = new Vector3(checkpointLocations[checkpointLocations.Count - 1].x, checkpointLocations[checkpointLocations.Count - 1].y);
			}
			else
			{
				targetPos = new Vector3(checkpointLocations[i - 1].x, checkpointLocations[i - 1].y);
			}
			Vector3 difference = targetPos - newPos;
			float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
			checkpoint.transform.rotation = Quaternion.Euler(0f, 0f, rotZ + 90f);
		}


		private void OnDrawGizmos()
		{
			Gizmos.color = Color.blue;

			for (int i = 0; i < checkpointLocations.Count - 1; i++)
			{
				Gizmos.DrawLine(checkpointLocations[i], checkpointLocations[i + 1]);
			}
		}





		private void DeleteOldTrack()
		{
			GameObject[] oldTrack = GameObject.FindGameObjectsWithTag("CurrentTrack");

			foreach (GameObject item in oldTrack)
			{
				DestroyImmediate(item);
			}

			DestroyImmediate(GameObject.FindGameObjectWithTag("MinimapTrack"));
		}
	}
}
