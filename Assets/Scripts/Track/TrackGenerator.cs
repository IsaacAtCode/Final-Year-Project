using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IsaacFagg.Tracks;
using IsaacFagg.Player;
using IsaacFagg.Paths;

public class TrackGenerator : MonoBehaviour
{
	[SerializeField]
	public Track track;
	public GameObject trackGO;
	public GameObject gravelGO;


	public PlayerData pd;

	//public ConvexHull ch;

	[Header("Convex Hull")]
	public Transform[] points;
	[Range(0.05f, 1.5f)]
	public float size;
	public bool drawIt;
	private List<Transform> result;

	[Header("Road")]
	public Material roadMat;
	public Material gravelMat;

	[Header("Reset")]
	public bool generateNewTrack = false;

	private void Awake()
	{
		pd = Object.FindObjectOfType<PlayerData>();
	}

	private void Start()
	{
		if (track == null)
		{
			track = gameObject.AddComponent<Track>();
		}

		CreateNewTrack();
	}

	private void Update()
	{
		if (generateNewTrack == true)
		{
			CreateNewTrack();
			generateNewTrack = false;
		}


		GenerateConvexHull();
	}

	public void CreateNewTrack()
	{
		trackGO = new GameObject("New Track");
		gravelGO = new GameObject("Gravel");
		gravelGO.transform.parent = trackGO.transform;

		PathCreator tPC = trackGO.AddComponent<PathCreator>();
		PathCreator gPC = gravelGO.AddComponent<PathCreator>();


		tPC.CreatePath();
		points = GenerateRandomTranforms();
		GenerateConvexHull();
		GenerateBevierPath(tPC);

		//Add Midpoints

		gPC.path = tPC.path;

		AddRoad();
	}

	public Transform[] GenerateRandomTranforms()
	{
		Transform[] transforms = new Transform[track.pointCount];
		Vector3 newPos;
		GameObject newGO;

		for (int i = 0; i < track.pointCount; ++i)
		{
			float x = Random.Range(0.0f, track.width) - track.width / 2;  // we subtract 125 to keep the square centralized  
			float y = Random.Range(0.0f, track.height) - track.height / 2;
			newPos = new Vector3(x, y, 0);

			newGO = new GameObject("Point " + i);
			newGO.transform.position = newPos;
			newGO.transform.parent = this.gameObject.transform;


			transforms[i] = newGO.transform;
		}

		return transforms;
	}

	private void GenerateConvexHull()
	{
		result = new List<Transform>();
		int leftMostIndex = 0;
		for (int i = 1; i < points.Length; i++)
		{
			if (points[leftMostIndex].position.x > points[i].position.x)
				leftMostIndex = i;
		}
		result.Add(points[leftMostIndex]);
		List<Transform> collinearPoints = new List<Transform>();
		Transform current = points[leftMostIndex];
		while (true)
		{
			Transform nextTarget = points[0];
			for (int i = 1; i < points.Length; i++)
			{
				if (points[i] == current)
					continue;
				float x1, x2, y1, y2;
				x1 = current.position.x - nextTarget.position.x;
				x2 = current.position.x - points[i].position.x;

				y1 = current.position.y - nextTarget.position.y;
				y2 = current.position.y - points[i].position.y;

				float val = (y2 * x1) - (y1 * x2);
				if (val > 0)
				{
					nextTarget = points[i];
					collinearPoints = new List<Transform>();
				}
				else if (val == 0)
				{
					if (Vector2.Distance(current.position, nextTarget.position) < Vector2.Distance(current.position, points[i].position))
					{
						collinearPoints.Add(nextTarget);
						nextTarget = points[i];
					}
					else
						collinearPoints.Add(points[i]);
				}
			}

			foreach (Transform t in collinearPoints)
				result.Add(t);
			if (nextTarget == points[leftMostIndex])
				break;
			result.Add(nextTarget);
			current = nextTarget;
		}
	}

	public void GenerateBevierPath(PathCreator pc)
	{

		List<Transform> corners = new List<Transform>(result);

		if (corners.Count > 1)
		{
			Vector2 startPos = new Vector2(corners[0].position.x, corners[0].position.y);
			pc.path.MovePoint(0, startPos);


			Vector2 secondPos = new Vector2(corners[1].position.x, corners[1].position.y);
			pc.path.MovePoint(3, secondPos);



			for (int i = 0; i < corners.Count - 2; i++)
			{
				Vector2 anchorPos = new Vector2(corners[i+2].position.x, corners[i+2].position.y);

				pc.path.AddSegment(anchorPos);
			}
		}

		pc.path.IsClosed = true;
		pc.path.AutoSetControlPoints = true;
	}

	public void AddRoad()
	{
		RoadCreator rc = trackGO.AddComponent<RoadCreator>();
		trackGO.GetComponent<MeshRenderer>().material = roadMat;
		rc.roadWidth = 10f;
		rc.UpdateRoad();

		AddGravel();
	}

	private void AddGravel()
	{
		RoadCreator rc = gravelGO.AddComponent<RoadCreator>();
		gravelGO.GetComponent<MeshRenderer>().material = gravelMat;
		rc.roadWidth = 20f;
		rc.UpdateRoad();
	}

	private void AddSides()
	{

	}




	void OnDrawGizmos()
	{
		if (drawIt)
		{
			if (result != null)
			{
				List<Transform> outter = new List<Transform>();
				foreach (var item in result)
					outter.Add(item);
				for (int i = 0; i < outter.Count - 1; i++)
					Gizmos.DrawLine(outter[i].position, outter[i + 1].position);
				Gizmos.DrawLine(outter[0].position, outter[outter.Count - 1].position);
			}

			for (int i = 0; i < points.Length; i++)
			{
				Gizmos.color = Color.cyan;
				if (result != null)
				{
					if (result.Contains(points[i]))
						Gizmos.color = Color.yellow;
				}
				Gizmos.DrawSphere(points[i].position, size);
			}
		}
	}



}
