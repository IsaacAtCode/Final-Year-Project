using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacFagg
{
	public class Track
	{
        //Track Details
        //All details for future track generation

        public string trackName;
        public int straightCount;
        public int turnCount;


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

}
