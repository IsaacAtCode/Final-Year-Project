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

		[Header("Checkpoints")]
		public Text checkCount;

		[Header("Speedometer")]
		public Text speedo;

		[Header("Finish")]
		public Text finishText;

		private void Start()
		{
			pCar = player.GetComponent<Car>();
			pController = player.GetComponent<CarController>();
			pLap = player.GetComponent<CarLap>();
		}

		private void Update()
		{
			//CurrentLapTime
			UpdateCounter(FormatLapTime(pLap.currentLap), lapTime);

			//Speedo
			UpdateCounter((pController.rb.velocity.magnitude * 10).ToString("0") + " mph", speedo);

			//Checkpoint Counter
			UpdateCounter(pLap.lastCheck.ToString() + "/" + track.maxCheckpoints, checkCount);


			//Lap Counter
			UpdateCounter((pLap.lapCount + 1).ToString() + "/" + track.maxLaps + " Laps", lapCount);

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
			if (pLap.finishedRace == true)
			{
				UpdateCounter("Finished Race", finishText);
			}
			else
			{
				finishText.text = "";
			}
		}



		//Only show the best lap after a lap has been completed


	}
}
