using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IsaacFagg.Player
{
    public class Consenting : MonoBehaviour
    {
        [Header("Player")]
        public Player player;
        PlayerData pd;

        [Header("Age Check")]
        public GameObject ageGo;

        [Header("Information Panel")]
        public GameObject infoGO;
        public Scrollbar infoScroll;
        public Toggle infoToggle;
        private bool infoScrollComplete;

        private void Awake()
        {
            player = Object.FindObjectOfType<Player>();
        }


        private void Start()
        {
            ageGo = GameObject.Find("Age Check");
            infoGO = GameObject.Find("Information Sheet");

            infoToggle.interactable = false;

        }

        public void ReadCheck()
        {
            if (!infoScrollComplete)
            {
                if (infoScroll.value < 0.02f)
                {
                    infoScrollComplete = true;
                }
                else
                {
                    infoToggle.interactable = false;
                }
            }
            else if (infoScrollComplete)
            {
                infoToggle.interactable = true;
            }
            
        }

    }
}
