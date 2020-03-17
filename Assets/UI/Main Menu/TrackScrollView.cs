using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IsaacFagg.Track;

namespace IsaacFagg.UI.Main
{
    public class TrackScrollView : MonoBehaviour
    {
        private ScrollRect scrollView;
        public GameObject trackIconPrefab;

        private void Start()
        {
            scrollView = GetComponentInChildren<ScrollRect>();


            List<TrackData> randomTracks = new List<TrackData>
            {
                RandomTrackGenerator.GenerateRandomTrack(),
                RandomTrackGenerator.GenerateRandomTrack(),
                RandomTrackGenerator.GenerateRandomTrack(),
                RandomTrackGenerator.GenerateRandomTrack()
            };

            PopulateScrollView(randomTracks);
        }

        public void PopulateScrollView(List<TrackData> tracks)
        {
            GameObject content = scrollView.content.gameObject;
            ClearOldTracks(content);

            for (int i = 0; i < tracks.Count; i++)
            {
                GameObject track = Instantiate(trackIconPrefab, content.transform);
                track.GetComponent<TrackIcon>().PopulateIcon(tracks[i]);
            }
        }

        private void ClearOldTracks(GameObject parent)
        {
            if (parent.transform.childCount > 0)
            {
                while (parent.transform.childCount > 0)
                {
                    Transform child = parent.transform.GetChild(0);
                    child.parent = null;
                    Destroy(child.gameObject);
                }
            }
        }
    }
}
