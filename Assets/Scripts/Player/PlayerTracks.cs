using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IsaacFagg.Track;

namespace IsaacFagg.Player
{
    [RequireComponent(typeof(PlayerInfo))]
    public class PlayerTracks : MonoBehaviour
    {
        public List<TrackData> tracks;
        public List<TrackData> DistinctTracks
        {
            get
            {
                return RemoveDuplicates(tracks);
            }
        }


        private List<TrackData> RemoveDuplicates(List<TrackData> tracks)
        {
            List<TrackData> noDupes = new List<TrackData>(tracks);

            noDupes = noDupes.Distinct().ToList();

            return noDupes;
        }




    }

}
