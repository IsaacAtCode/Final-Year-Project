using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using IsaacFagg.UI;
using IsaacFagg.Paths;
using IsaacFagg.Utility;

namespace IsaacFagg.Track
{
	public class TrackGenerator : MonoBehaviour
	{
		public TrackData trackData;

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

		private void Start()
		{
			if (!trackData)
			{
				trackData = RandomTrackGenerator.GenerateRandomTrack();

				TrackData tdGO = gameObject.AddComponent<TrackData>();

				tdGO.PopulateTrackData(trackData);
			}

			GenerateTrackMesh(trackData);
		}


		public void GenerateTrackMesh(TrackData data)
		{
			GameObject trackGO = new GameObject(data.name);
			GameObject gravelGO = new GameObject("Gravel");
			GameObject minimapGO = new GameObject("Minimap Track");
			//Parents
			trackGO.transform.parent = this.gameObject.transform;
			gravelGO.transform.parent = trackGO.transform;
			minimapGO.transform.parent = trackGO.transform;
			//Tags
			trackGO.tag = "CurrentTrack";
			gravelGO.tag = "Gravel";
			minimapGO.tag = "MinimapTrack";
			//Layers
			trackGO.layer = 10;
			gravelGO.layer = 10;
			minimapGO.layer = 11;
			//Path Creators
			PathCreator tPC = trackGO.AddComponent<PathCreator>();
			PathCreator gPC = gravelGO.AddComponent<PathCreator>();
			PathCreator mPC = minimapGO.AddComponent<PathCreator>();
			//Paths
			Path path = GenerateBevierPath(data);

			tPC.path = path;
			gPC.path = path;
			mPC.path = path;

			AddMesh(trackGO, roadMat, 2f, false);
			AddMesh(gravelGO, gravelMat, 5f,false);
			AddMesh(minimapGO, minimapMat, 5f, true);

			//AddBackground(background);

			GenerateCheckpoints(tPC.path, trackGO);

				
		}

		private Path GenerateBevierPath(TrackData trackData)
		{
			Path newPath = new Path(trackData.Centre);


			if (trackData.points.Count > 1)
			{
				newPath.MovePoint(0, trackData.points[0]);

				newPath.MovePoint(3, trackData.points[1]);

				for (int i = 2; i < trackData.points.Count - 2; i++)
				{
					//Vector2 anchorPos = new Vector2(allPoints[i + 2].x, allPoints[i + 2].y);

					newPath.AddSegment(trackData.points[i]);
				}
			}

			newPath.IsClosed = true;
			newPath.AutoSetControlPoints = true;

			return newPath;
		}

		private void AddMesh(GameObject go, Material mat, float width, bool isMinimap)
		{
			RoadCreator rc = go.AddComponent<RoadCreator>();
			MeshRenderer mr = go.GetComponent<MeshRenderer>();
			mr.material = mat;
			mr.sortingLayerName = "Track";
			mr.sortingOrder = 0;

			if (!isMinimap)
			{
				AddCollider(go);
			}

			rc.roadWidth = width;
			rc.UpdateRoad();
		}

		private void AddCollider(GameObject go)
		{
			go.AddComponent<Mesh2DColliderMaker>();

			go.GetComponent<PolygonCollider2D>().isTrigger = true;
		}

		private void GenerateCheckpoints(Path path, GameObject go)
		{
			checkpoints.Clear();

			//Invisible Checkpoints
			checkpointLocations = path.CalculateEvenlySpacedPoints(path.EstimatedLength() / checkpointCount);

			GameObject checkpointParent = new GameObject("Checkpoints");
			checkpointParent.transform.position = Vector2.zero;
			checkpointParent.transform.parent = go.transform;

			if (trackData.rotation == Rotation.Anticlockwise)
			{
				checkpointLocations.Reverse();
			}

			for (int i = 0; i < checkpointLocations.Count; i++)
			{
				CreateCheckpoint(i, checkpointParent.transform);
			}
		}

		private void CreateCheckpoint(int i, Transform parent)
		{
			Vector3 newPos = checkpointLocations[i];

			GameObject checkpoint = Instantiate(checkpointTemplate, parent);

			checkpoint.name = ("Checkpoint: " + i);
			checkpoint.transform.position = newPos;
			checkpoint.tag = "Checkpoint";
			Checkpoint cp = checkpoint.GetComponent<Checkpoint>();
			checkpoints.Add(cp);
			cp.position = i;

			SpriteRenderer sprite = checkpoint.GetComponentInChildren<SpriteRenderer>();

			if (i == 0)
			{
				cp.finishLine = true;
				sprite.enabled = true;
			}
			//else if (i > 0)
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

			checkpoint.transform.rotation = MathsUtility.LookAt(newPos, targetPos);
		}


	}
}
