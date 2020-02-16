using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IsaacFagg.Player;
using IsaacFagg.Utility;

namespace IsaacFagg.Race
{
    [RequireComponent(typeof(RaceManager))]
    public class RaceUI : MonoBehaviour
    {
        PlayerSettings ps;
        public Canvas canvas;
        RaceManager rm;

        [Header("Lap Times")]
        public GameObject totalLap;
        public Text totalLapTime;
        public GameObject currentLap;
        public Text currentLapTime;
        public GameObject bestLap;
        public Text bestLapTime;
        public GameObject lastLap;
        public Text lastLapTime;

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

        private void Start()
        {
            totalLap.SetActive(false);
            bestLap.SetActive(false);
            lastLap.SetActive(false);
        }

        private void FixedUpdate()
        {
            UpdateCountdown();
            UpdateTotalLap();
            UpdateCurrentLap();

        }

        public void UpdateCounter(string input, Text textbox)
        {
            textbox.text = input;
        }

        private void UpdateTotalLap()
        {
            if (rm.lapCount >= 1)
            {
                if (!totalLap.activeInHierarchy)
                {
                    totalLap.SetActive(true);
                }

                UpdateCounter(MathsUtility.FormatLapTime(rm.totalTime), totalLapTime);
            }
            else
            {
                totalLap.SetActive(false);
            }            
        }

        private void UpdateCurrentLap()
        {
            if (!rm.isRaceStarted && !rm.isRaceFinished)
            {
                currentLap.SetActive(false);
            }
            else if (rm.isRaceStarted && !rm.isRaceFinished)
            {
                if (!currentLap.activeInHierarchy)
                {
                    currentLap.SetActive(true);
                }

                UpdateCounter(MathsUtility.FormatLapTime(rm.currentLapTime), currentLapTime);
            }
            else if (rm.isRaceFinished)
            {
                currentLap.SetActive(false);
            }
        }

        private void UpdateCountdown()
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

        public void UpdateLap(int count, int max)
        {
            UpdateCounter(count + "/" + max, lapCount);

            UpdateBestLap();
            UpdateLastLap();
        }

        private void UpdateBestLap()
        {
            if (rm.lapCount >= 2)
            {
                if (!bestLap.activeInHierarchy)
                {
                    bestLap.SetActive(true);
                }

                UpdateCounter(MathsUtility.FormatLapTime(rm.bestLap.time), bestLapTime);
            }
            else
            {
                bestLap.SetActive(false);
            }
        }

        private void UpdateLastLap()
        {
            if (rm.lapCount >= 1)
            {
                if (!lastLap.activeInHierarchy)
                {
                    lastLap.SetActive(true);
                }

                UpdateCounter(MathsUtility.FormatLapTime(rm.lastLap.time), lastLapTime);
            }
            else
            {
                bestLap.SetActive(false);
            }
        }



    }
}