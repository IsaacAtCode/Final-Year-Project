using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IsaacFagg.Mobile
{

    public class MobileMovement : MonoBehaviour
    {
        public int direction = 0;

        bool leftTurn = true;
        bool rightTurn = false;

        public void UpdateDirection()
        {
            if (leftTurn && !rightTurn)
            {
                direction = 1;
            }
            else if (!leftTurn && rightTurn)
            {
                direction = -1;
            }
            else
            {
                direction = 0;
            }
        }

        public void StartTurnLeft()
        {
            leftTurn = true;
            UpdateDirection();
        }

        public void StopTurnLeft()
        {
            leftTurn = false;
            UpdateDirection();

        }

        public void StartTurnRight()
        {
            rightTurn = true;
            UpdateDirection();

        }

        public void StopTurnRight()
        {
            rightTurn = false;
            UpdateDirection();

        }


    }
}
