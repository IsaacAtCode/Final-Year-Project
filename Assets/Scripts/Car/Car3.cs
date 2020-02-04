using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacFagg.Cars
{
    public class Car3 : MonoBehaviour
    {
        CarModel carModel;

		public CarDirection direction;
		public CarTurn turn;


        private void Start()
        {
            carModel = GetComponent<CarModel>();
        }

		private void Update()
		{
			if (Input.GetKey(KeyCode.A))
			{
				turn = CarTurn.Left;
			}
			else if (Input.GetKey(KeyCode.D))
			{
				turn = CarTurn.Right;
			}
			else
			{
				turn = CarTurn.Forward;
			}

			carModel.WheelTurn(turn);
		}
	}

	public enum CarDirection
	{
		Forward,
		Stationary,
		Backwards,
	}


	public enum CarTurn
	{
		Left,
		Forward,
		Right,
	}
}
