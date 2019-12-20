using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacFagg.Tracks
{
	public class Track
	{
		public TrackType trackType;

		//Random
		public List<Vector2> fillPoints;

		//Player Data
		public float difficulty = 1;
		public Rotation rot;

		//Main
		public float height = 250.0f;
		public float width = 250.0f;

		public List<Vector2> vPoints;
		public List<Transform> tPoints;
		public List<Segment> segments;

		public string trackName = "Speedway";

		//Name Generator
	}

	public class Segment
	{
		public Transform location;
	}

	public enum Rotation
	{
		Clockwise,
		Anticlockwise,
	}

}
