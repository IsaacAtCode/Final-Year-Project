using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacFagg.Tracks
{
	public class Track : MonoBehaviour
	{

		//Player Data
		public float difficulty = 1;

		//Main
		public float height = 250.0f;
		public float width = 250.0f;

		public List<Vector2> points;
        public List<Checkpoint> checkpoints;

		public string trackName = "";

        //Name Generator
    }

	public class Segment
	{
		public Transform location;
	}



}
