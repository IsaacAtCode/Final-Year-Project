using UnityEngine;

namespace IsaacFagg.Cars
{
    [RequireComponent(typeof(CarMovement))]
    public class CarInputBase : MonoBehaviour
    {
        CarMovement movement;

        private void Awake()
        {
            movement = GetComponent<CarMovement>();
        }

        protected void SetEnginePower(float power)
        {
            movement.SetEnginePower(power);
        }


        protected void SetSteeringDirection(float steering)
        {
            movement.SetSteeringDirectrion(steering);
        }


    }
}

