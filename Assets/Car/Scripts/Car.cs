using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacFagg.Cars
{
	public class Car : MonoBehaviour
	{
		CarModel cm;

		public float speed;



		private void Start()
		{
			cm = GetComponent<CarModel>();
		}

		private void Update()
		{
			speed = GetComponent<Rigidbody2D>().velocity.magnitude;	

		}


	}
}
