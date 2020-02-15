using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IsaacFagg.Player;

namespace IsaacFagg.Race
{
    [RequireComponent(typeof(RaceManager))]
    public class RaceUI : MonoBehaviour
    {
        PlayerSettings ps;
        public Canvas canvas;
        RaceManager rm;

        [Header("Lap Times")]
        public Text totalLap;
        public Text bestLap;
        public Text currentLap;
        public Text lapCount;

        [Header("Countdown")]
        public Text countdown;

        [Header("Minimap")]
        public GameObject minimap;



        private void Awake()
        {
            ps = Object.FindObjectOfType<PlayerSettings>();
            rm = GetComponent<RaceManager>();
        }

        private void Update()
        {
            if (rm.countdown > 0)
            {
                UpdateCounter((Mathf.Ceil(rm.countdown)).ToString(), countdown);
            }
            else
            {
                UpdateCounter("", countdown);
            }
        }

        public void UpdateCounter(string input, Text textbox)
        {
            textbox.text = input;
        }

    }
}