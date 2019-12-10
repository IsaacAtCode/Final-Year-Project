using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IsaacFagg.Tracks;
using IsaacFagg.Player;

public class TrackGenerator : MonoBehaviour
{
	[SerializeField]
	public Track track;
	public GameObject trackGO;

	public PlayerData pd;

    public ConvexHull ch;

    public bool generateNewHull = false;

	//Generate track from track
	//Generate track from nothing?

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

        //Create convex hull then delete?
        ch = GetComponent<ConvexHull>();

        ResetTransforms();
	}

    private void Update()
    {
        if (generateNewHull)
        {
            ResetTransforms();
            generateNewHull = false;
        }
    }


    private void GeneratePoints()
    { 
        Vector2[] points = new Vector2[track.pointCount * 2];
        for (int i = 0; i < track.pointCount; ++i)
        {
            float x = Random.Range(0.0f, track.width) - track.width/2;  // we subtract 125 to keep the square centralized  
            float y = Random.Range(0.0f, track.height) - track.height/2;
            points[i] = new Vector2(x, y);
        }
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

    public void ResetTransforms()
    {
        ch.points = GenerateRandomTranforms();
    }

    public void GenerateMidpoints(Transform[] points)
    {
        Transform[] midTrans = new Transform[points.Length * 2];
        Vector2 disp = new Vector2();

        float maxDisp = 20f;
        Vector3 newPos;
        GameObject newGO;


        for (int i = 0; i < points.Length; i++)
        {
            float dispLen = (float)Mathf.Pow(Random.Range(0, 1), track.difficulty) * maxDisp;
            disp.Set(0, 1);
            disp = Vector2FromAngle(Random.Range(0,1) * 360);
            disp *= dispLen;

            newGO = new GameObject("Midpoint " + i);
            newGO.transform.position = newPos;
            newGO.transform.parent = this.gameObject.transform;

            midTrans[i * 2] = points[i];



        }
    }

    public Vector2 Vector2FromAngle(float a)
    {
        a *= Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(a), Mathf.Sin(a));
    }






}
