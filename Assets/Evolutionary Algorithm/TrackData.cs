using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IsaacFagg.Paths;

namespace IsaacFagg.Track
{
    public class TrackData : MonoBehaviour
    {
        public bool validTrack = false;
        //Points
        public List<Vector2> initialPoints;

        public List<Vector2> scaledPoints;

        public Rotation rotation;
       
        //Properies
        public float height
        {
            get
            {
                return TrackUtility.GetHeight(initialPoints);
            }
        }
        public float width
        {
            get
            {
                return TrackUtility.GetWidth(initialPoints);
            }
        }
        public float length
        {
            get
            {
                return TrackUtility.GetLength(initialPoints);
            }
        }
        public int straightCount
        {
            get
            {
                    return TrackUtility.GetStraights(initialPoints);
            }
        }
        public int curveCount
        {
            get
            {
                return TrackUtility.GetCurves(initialPoints);
            }
        }
    }

}
