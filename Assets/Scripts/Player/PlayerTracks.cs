using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacFagg.Player
{
    [RequireComponent(typeof(Player))]
    public class PlayerTracks : MonoBehaviour
    {
        private Player player;

        private void Start()
        {
            player = GetComponent<Player>();
        }
    }

}
