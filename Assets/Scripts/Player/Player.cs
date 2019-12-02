using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacFagg.Player
{
    public class Player : MonoBehaviour
    {
        public string nick;
        public int age; //Age not DOB so less invasive
        public Gender gender;
        public string UUID;
        public bool hasConsent;
        public bool firstLoad = true;

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
            gender = Gender.Empty;
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
        Empty,
    }

}
