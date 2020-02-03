using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacFagg.Player
{

    //Database for all the data kept from the player

    [RequireComponent(typeof(Player))]
    public class PlayerData : MonoBehaviour
    {

       
        private Player player;


        [Header("Ratings")]
        public float meanRate;

        private void Awake()
        {
            player = GetComponent<Player>();
        }
    }
}
