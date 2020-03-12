using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

namespace IsaacFagg.Player
{
	[RequireComponent(typeof(PlayerInfo))]
	public class PlayerSettings : MonoBehaviour
	{
		[Header("Display")]
		public int fpsLimiter;
		public bool cameraShake;
		public SpeedDisplay speedDisplay;

		[Header("Minimap")]
		public bool mapState = true;
		public MapType mapType;
		public Color mapColour;
		[Range(0,100)]
		public float mapOpacity;
		public float mapZoom;


		[Header("Sound")]
		public float masterVolume;
		public float musicVolume;
		public float sfxVolume;
		public float engineVolume;

		[Header("Controls")]
		public ControlType controlType;
		public float sensitivity;

		[Header("Notifications")]
		public bool notifOn = true;

		//Stuff to save the settings

	}


	public enum SpeedDisplay
	{
		KMH,
		MPH,
	}

	public enum MapType
	{
		Follow,
		Static,
	}

	public enum ControlType
	{
		Touch,
		Tilt,
	}




}
