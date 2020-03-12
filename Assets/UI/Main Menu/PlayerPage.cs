using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IsaacFagg.Player;

using Object = UnityEngine.Object;

namespace IsaacFagg.UI.Main
{
    public class PlayerPage : MonoBehaviour
    {
        public PlayerInfo playerInfo;


        private void Awake()
        {
            playerInfo = Object.FindObjectOfType<PlayerInfo>();
        }

        private void OnEnable()
        {
            if (!playerInfo)
            {
                playerInfo = Object.FindObjectOfType<PlayerInfo>();
            }

            PopulateDropdown();

            SetDropdown((int)playerInfo.gender);

        }



        #region Avatar
        [Header("Avatar")]
        public Image avatarImage;
        public List<Sprite> avatarOptions;
       
        public void ChangeProfileColour(int newColour)
        {
            int col = Mathf.Abs(newColour % (avatarOptions.Count - 1));

            avatarImage.sprite = avatarOptions[col];
            playerInfo.avatarColour = col;
        }

        public void NextColour()
        {
            ChangeProfileColour(playerInfo.avatarColour + 1);
        }
        public void PrevColour()
        {
            ChangeProfileColour(playerInfo.avatarColour - 1);
        }

        #endregion

        #region Age
        [Header("Age")]
        public InputField ageInput;

        public void SetInput()
        {
            ageInput.text = playerInfo.age.ToString();
        }

        public void SetAge(string newAge)
        {
            int age = Mathf.Clamp(int.Parse(newAge), 0, 130);

            playerInfo.SetAge(age);
        }



        #endregion

        #region Gender
        [Header("Gender")]
        public Dropdown genderDrop;

        public void PopulateDropdown()
        {
            genderDrop.ClearOptions();

            string[] genders = Enum.GetNames(typeof(Gender));

            genderDrop.AddOptions(new List<string>(genders));
        }


        public void SetDropdown(int value)
        {
            genderDrop.value = value;
        }



        public void SetGender(int newGender)
        {
            playerInfo.SetGender((Gender)newGender);
        }

        #endregion

    }




}
