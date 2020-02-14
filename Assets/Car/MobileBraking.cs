using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacFagg.Mobile
{
    public class MobileBraking : MonoBehaviour
    {
        public bool braking = false;

        bool leftBrake = false;
        bool rightBrake = false;

        private void UpdateBraking()
        {
            if (leftBrake || rightBrake)
            {
                braking = true;
                Debug.Log("braking");
            }
            else
            {
                braking = false;
            }
        }

        public void StartBrakeLeft()
        {
            leftBrake = true;
            UpdateBraking();
        }

        public void StopBrakeLeft()
        {
            leftBrake = false;
            UpdateBraking();
        }

        public void StartBrakeRight()
        {
            rightBrake = true;
            UpdateBraking();
        }

        public void StopBrakeRight()
        {
            rightBrake = false;
            UpdateBraking();
        }

    }
}
