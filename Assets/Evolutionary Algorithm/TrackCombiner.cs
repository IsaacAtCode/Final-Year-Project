using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IsaacFagg.Track;
using IsaacFagg.Utility;

namespace IsaacFagg.Genetics
{
    public static class TrackCombiner
    {
        public static TrackData CombineTracks(TrackData track1, TrackData track2)
        {
            int scale = Mathf.Max(track1.points.Count, track2.points.Count);

            List<Vector2> track1ScaledPoints = track1.ScaledPoints(scale);
            List<Vector2> track2ScaledPoints = track2.ScaledPoints(scale);

            List<Vector2> newTrackPoints = new List<Vector2>();


            TrackData newTrack = new TrackData(newTrackPoints);
            return newTrack;
        }




    }
}
