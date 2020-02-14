using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IsaacFagg.Mobile;

namespace IsaacFagg.Cars
{
    public class CarInputMobile : CarInputBase
    {
        //Left and Right buttons
        //Reverse Button
        [Header("Movement")]
        public MobileMovement mm;

        [Header("Brakes")]
        public MobileBraking mb;

        private void Start()
        {
            mm = GameObject.Find("Interaction Canvas").GetComponentInChildren<MobileMovement>();
            mb = GameObject.Find("Interaction Canvas").GetComponentInChildren<MobileBraking>();
        }

        private void Update()
        {
            UpdateEnginePower();
            UpdateSteering();
        }

        void UpdateEnginePower()
        {
            if (mb.braking)
            {
                SetEnginePower(-1);
            }
            else
            {
                SetEnginePower(1);
            }

        }

        void UpdateSteering()
        {
            SetSteeringDirection(mm.direction);
        }
    }
}
