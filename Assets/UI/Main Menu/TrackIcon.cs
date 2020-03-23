using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IsaacFagg.Track;
using IsaacFagg.Icons;

namespace IsaacFagg.UI
{
    public class TrackIcon : MonoBehaviour
    {
        public RawImage icon;
        public Text title;

        public TrackData data;

        //private void Start()
        //{
        //    PopulateIcon(RandomTrackGenerator.GenerateRandomTrack());
        //}

        public void PopulateIcon(TrackData trackInfo)
        {
            data = trackInfo;

            title.text = trackInfo.name;

            Texture2D texture = GameObject.Find("Icon Renderer").GetComponent<IconCreator>().CreateIcon(trackInfo.points);

            trackInfo.icon = texture;
            icon.texture = texture;            
        }


    }
}
