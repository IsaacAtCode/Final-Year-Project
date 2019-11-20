using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacFagg
{
	[RequireComponent(typeof(Car))]
	public class CarLap : MonoBehaviour
	{
		private Car car;
		private Rigidbody2D rb;

		public float lapTotal;
		public float currentLap;

		//Laps
		public Lap[] laps;
		public Lap lastLap;
		public Lap bestLap;

		private void Start()
		{
			car = GetComponent<Car>();
			rb = GetComponent<Rigidbody2D>();
		}

		private void Update()
		{
			lapTotal += Time.deltaTime;
			currentLap += Time.deltaTime;

		}



		//StartLap

		//TrackLap

		//EndLap & Store it


		//Set Last Lap

		//FindBestLap

	}



}
