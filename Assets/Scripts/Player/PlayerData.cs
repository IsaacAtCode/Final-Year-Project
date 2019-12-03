using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacFagg.Player
{
    [RequireComponent(typeof(Player))]
    public class PlayerData : MonoBehaviour
    {

        //Database for all the data kept from the player
        private Player player;


        [Header("Ratings")]
        public float meanRate;






        private void Start()
        {
            player = GetComponent<Player>();
        }
    }
}
