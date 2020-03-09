using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IsaacFagg.Player
{
    public class ToggleRequest : MonoBehaviour
    {
        private TrackRequest currentRequest;
        public Text title;

        public Toggle noChange;
        public Toggle increase;
        public Toggle decrease;

        public TrackRequest request
        {
            get
            {
               return GetRequestFromToggle();
            }
        }



        public void PopulateRequest(TrackRequest tq)
        {
            currentRequest = tq;


            title.text = currentRequest.type.ToString();

            noChange.isOn = true;
        }

        private TrackRequest GetRequestFromToggle()
        {

            if (increase.isOn)
            {
                currentRequest.change = RequestChange.Increase;
            }
            else if (decrease.isOn)
            {
                currentRequest.change = RequestChange.Decrease;
            }
            else
            {
                currentRequest.change = RequestChange.NoChange;
            }

            return currentRequest;

        }

    }
}
