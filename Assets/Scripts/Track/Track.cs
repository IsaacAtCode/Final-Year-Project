using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacFagg.Tracks
{
	public class Track : MonoBehaviour
	{
		//Track Details
		//All details for future track generation

		public float height = 250.0f;
		public float width = 250.0f;

		public int pointCount = 10;
		//public List<Vector2> points = new List<Vector2>();
		public Transform[] points;

		public float difficulty = 1;

		public Direction dir;

		public string trackName = "Speedway";
		//public int straightCount;
		//public int turnCount;

		public List<Segment> segments;

	}

	public class Straight
	{
		private float length;
	}
	public class Turn
	{
		[Range(0,1)]
		private int direction; //Left is 0, Right is 1
		private float arc; //In radians
		//Start and end radius
		private float startRad;
		private float endRad; 
	}

	public class Segment
	{
		public Transform location;
	}

	public enum Direction
	{
		Clockwise,
		Anticlockwise,
	}

}
