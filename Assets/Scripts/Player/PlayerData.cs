using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacFagg.Player
{
    //Database for all the data kept from the player

    [RequireComponent(typeof(PlayerInfo))]
    public class PlayerData : MonoBehaviour
    {

        [Header("Ratings")]
        public float meanRate;

    }
}
