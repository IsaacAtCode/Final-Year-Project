using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacFagg.Player
{
    public class RequestManager : MonoBehaviour
    {
        public List<TrackRequest> possibleRequests = new List<TrackRequest> { new TrackRequest(RequestType.Length), new TrackRequest(RequestType.Straights), new TrackRequest(RequestType.Corners), new TrackRequest(RequestType.Speed), new TrackRequest(RequestType.Difficulty) };

        public ToggleRequest request1;
        public ToggleRequest request2;

        public List<TrackRequest> Requests
        {
            get
            {
                List<TrackRequest> requests = new List<TrackRequest> { GetRequestFromToggle(request1), GetRequestFromToggle(request2) };
                return requests;

            }
        }




        private void Start()
        {
            TrackRequest RandomR1 = possibleRequests[Random.Range(0, possibleRequests.Count - 1)];

            List<TrackRequest> requests = new List<TrackRequest>(possibleRequests);
            requests.Remove(RandomR1);
            TrackRequest RandomR2 = requests[Random.Range(0, requests.Count - 1)];

            request1.PopulateRequest(RandomR1);
            request2.PopulateRequest(RandomR2);
        }



        //private void Update()
        //{
        //    Debug.Log(GetRequestFromToggle(request1).type + " " + GetRequestFromToggle(request1).change);
        //    Debug.Log(GetRequestFromToggle(request2).type + " " + GetRequestFromToggle(request2).change);

        //}



        public TrackRequest GetRequestFromToggle(ToggleRequest tr)
        {
            return tr.request;
        }


    }
}

