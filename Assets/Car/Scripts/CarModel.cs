using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IsaacFagg.Cars
{
    [RequireComponent(typeof(Car))]
    public class CarModel : MonoBehaviour
    {
        private SpriteRenderer render;

        [Header("Wheels")]
        public GameObject wheel_FL;
        public GameObject wheel_FR;
        public GameObject wheel_BL;
        public GameObject wheel_BR;

        public Sprite carSprite;
        public Color carColour;
        public Sprite wheelSprite;

        public float wheelMinMax = 0.5f;
        public float backWheelMultiplier = 0.25f;

        private void Start()
        {
            render = GetComponent<SpriteRenderer>();
        }


        public void CreateCar()
        {
            render.sprite = carSprite;
            render.color = carColour;
        }

        public void WheelTurn(CarTurn dir)
        {
            float wheelRot = 0f;

            if (dir == CarTurn.Left)
            {
                wheelRot = Mathf.Clamp(-1f * wheelMinMax, -wheelMinMax, wheelMinMax);
            }
            if (dir == CarTurn.Right)
            {
                wheelRot = Mathf.Clamp(1f * wheelMinMax, -wheelMinMax, wheelMinMax);
            }

            Vector3 frontWheelRotation = new Vector3(0f, 0f, -wheelRot);
            Vector3 backWheelRotation = new Vector3(0f, 0f, wheelRot * backWheelMultiplier);

            wheel_FL.transform.localRotation = Quaternion.Lerp(wheel_FL.transform.rotation, Quaternion.Euler(frontWheelRotation), 1f);
            wheel_FR.transform.localRotation = Quaternion.Lerp(wheel_FR.transform.rotation, Quaternion.Euler(frontWheelRotation), 1f);

            wheel_BL.transform.localRotation = Quaternion.Lerp(wheel_BL.transform.rotation, Quaternion.Euler(backWheelRotation), 1f);
            wheel_BR.transform.localRotation = Quaternion.Lerp(wheel_BR.transform.rotation, Quaternion.Euler(backWheelRotation), 1f);
        }
    }
}
