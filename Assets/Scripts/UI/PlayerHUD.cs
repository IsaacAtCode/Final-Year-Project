using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IsaacFagg.UI
{
	public class PlayerHUD : MonoBehaviour
	{
		public GameObject player;
		public Car pCar;
		public CarController pController;
		public CarLap pLap;
		public Track track;


		[Header("Laps")]
		public Text lapCount;
		public Text lapTime;
		public Text bestLap;
		public Text lastLap;

		[Header("Checkpoints")]
		public Text checkCount;

		[Header("Speedometer")]
		public Text speedo;

		[Header("Finish")]
		public Text finishText;

		[Header("Countdown")]
		public Text countdown;

		private void Start()
		{
			pCar = player.GetComponent<Car>();
			pController = player.GetComponent<CarController>();
			pLap = player.GetComponent<CarLap>();

			lastLap.text = "";
			bestLap.text = "";

		}

		private void Update()
		{
			//CurrentLapTime
			if (pLap.currentLap != null && pLap.raceState == CarLap.RaceState.Ongoing)
			{
				UpdateCounter(FormatLapTime(pLap.currentLap.time), lapTime);
			}

			//Speedo
			UpdateCounter((pController.rb.velocity.magnitude * 10).ToString("0") + " mph", speedo);

			//Countdown
			if (pLap.raceState == CarLap.RaceState.Starting)
			{
				UpdateCounter((Mathf.Ceil(pLap.countdownTimer)).ToString(), countdown);
			}
			else
			{
				UpdateCounter("", countdown);
			}

			//Finish
			ShowFinish();

		}

		public string FormatLapTime(float time)
		{
			int minutes = (int)time / 60;
			int seconds = (int)time % 60;
			float milliseconds = time * 100;
			milliseconds %= 100;


			return string.Format("{0:0}:{1:00}:{2:00}", minutes, seconds, milliseconds);
		}


		public void UpdateCounter(string input, Text textbox)
		{
			textbox.text = input;
		}

		public void ShowFinish()
		{
			if (pLap.raceState == CarLap.RaceState.Finished)
			{
				UpdateCounter("Finished Race", finishText);
				UpdateCounter(track.maxLaps + "/" + track.maxLaps, lapCount);
			}
			else
			{
				finishText.text = "";
			}
		}





		//Only show the best lap after a lap has been completed


	}
}
