using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacFagg.Cars
{
	public class Car : MonoBehaviour
	{
		CarModel cm;

		public float speed;

		public float distanceTravelled = 0;
		Vector2 lastPos;

		public CarState carState = CarState.Off;


		private void Start()
		{
			cm = GetComponent<CarModel>();
		}

		private void Update()
		{
			speed = GetComponent<Rigidbody2D>().velocity.magnitude;

			distanceTravelled += Vector2.Distance(transform.position, lastPos);
			lastPos = transform.position;
		}


	}

	public enum CarState
	{
		Off,
		On,
		Auto
	}
}
