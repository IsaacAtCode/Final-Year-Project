using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IsaacFagg.UI.Main
{
    public class SpeedDisplay : MonoBehaviour
    {
        public Toggle kmh;
        public Toggle mph;

        public bool isMPH
        {
            get
            {
                if (mph.isOn)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


    }

}
