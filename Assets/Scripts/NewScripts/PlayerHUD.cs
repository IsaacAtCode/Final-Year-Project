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


		[Header("Laps")]
		public Text lapCount;
		public Text lapTime;
		public Text bestLap;

		[Header("Checkpoints")]
		public Text checkCount;

		[Header("Speedometer")]
		public Text speedo;

		private void Start()
		{
			pCar = player.GetComponent<Car>();
			pController = player.GetComponent<CarController>();
			pLap = player.GetComponent<CarLap>();
		}

		private void Update()
		{
			UpdateCounter(FormatLapTime(pLap.currentLap), lapTime);
		}

		public string FormatLapTime(float time)
		{
			int minutes = (int)time / 60;
			int seconds = (int)time % 60;
			float milliseconds = time * 1000;
			return string.Format("{0:00}:{1:00}:{2:0}", minutes, seconds, milliseconds);
		}


		public void UpdateCounter(string input, Text textbox)
		{
			textbox.text = input;
		}

		//Only show the best lap after a lap has been completed


	}
}
