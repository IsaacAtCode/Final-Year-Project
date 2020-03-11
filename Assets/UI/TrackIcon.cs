using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IsaacFagg.Track;

namespace IsaacFagg.UI
{
    public class TrackIcon : MonoBehaviour
    {
        public Image image;
        public Text title;


        public void PopulateIcon(TrackData trackInfo)
        {
            title.text = trackInfo.name;

        }


    }
}
