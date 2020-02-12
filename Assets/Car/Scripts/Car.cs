using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacFagg.Cars
{
	public class Car : MonoBehaviour
	{
		CarModel carModel;

		public CarDirection direction;
		public CarTurn turn;

		public float speed;



		private void Start()
		{
			carModel = GetComponent<CarModel>();
		}

		private void Update()
		{
			speed = GetComponent<Rigidbody2D>().velocity.magnitude;	
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
