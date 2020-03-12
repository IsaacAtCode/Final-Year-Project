using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacFagg.Player
{
    public class PlayerInfo : MonoBehaviour
    {
        public string nick;
        public Gender gender = Gender.Other;
        public string UUID;
        public bool firstLoad = true;

        [Header("Age")]
        public bool underAge;
        public int age; //Age not DOB so less invasive


        [Header("Consent")]
        public bool infoConsent;
        public bool agreeConsent;
        public bool hasConsent;

        [Header("Customise")]
        public int avatarColour = 0;

        private void Awake()
        {
            if (firstLoad)
            {
                NewPlayer();
                //Maybe make firstload turn off after consent has been activated
                //Save firstload so it doesnt keep resetting
                firstLoad = false;
            }
            
        }

        private void NewPlayer()
        {
            nick = null;
            UUID = GetUUID();
            age = 0;
            gender = Gender.Other;
            hasConsent = false;

            //Clears player info, can also be used later
        }

        public void SetNick(string newNick)
        {
            nick = newNick;
        }

        public void SetAge(int newAge)
        {
            age = newAge;

            if (age <= 18)
            {
                Debug.Log("Too young, do smth");
            }
            else
            {
                Debug.Log("New age = " + age);
            }


        }

        public void SetGender(Gender newGender)
        {
            gender = newGender;
        }

        private string GetUUID()
        {
            return SystemInfo.deviceUniqueIdentifier;
        }





    }

    public enum Gender
    {
        Male,
        Female,
        Other,
    }

}
