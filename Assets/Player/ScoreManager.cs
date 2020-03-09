using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using IsaacFagg.Track;
using IsaacFagg.Race;

namespace IsaacFagg.Player
{
    public class ScoreManager : MonoBehaviour
    {
        public TrackData track;
        public StarRating sr;
        public RequestManager rq;
        public RaceManager rm;

        public TrackScore Score
        {
            get
            {
                return GetScore();
            }
        }

        private TrackScore GetScore()
        {
            TrackScore tScore = new TrackScore(track, sr.Rating, rq.Requests, rm.Laps);
            return tScore;
        }


    }

    public class TrackScore
    {
        public TrackData track;
        public int rating;
        public List<TrackRequest> requests;
        public List<Lap> laps;



        public TrackScore(TrackData trackData, int starRating, List<TrackRequest> trackRequests, List<Lap> lapTimes)
        {
            track = trackData;
            rating = starRating;
            requests = trackRequests;
            laps = lapTimes;
        }
    }




    public class TrackRequest
    {
        public RequestType type;
        public RequestChange change;

        public TrackRequest(RequestType requestType)
        {
            type = requestType;
            change = RequestChange.NoChange;
        }

        public TrackRequest(RequestType requestType, RequestChange requestChange)
        {
            type = requestType;
            change = requestChange;
        }
    }

    public enum RequestType
    {
        Length,
        Difficulty,
        Obstacles,
        Powerups
    }

    public enum RequestChange
    {
        NoChange,
        Increase,
        Decrease
    }

}
