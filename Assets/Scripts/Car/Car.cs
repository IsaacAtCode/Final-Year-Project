using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacFagg.Cars
{
	public class Car : MonoBehaviour
	{
		public float acceleration;
		public float torque;
		[Range(0, 1)]
		public float drift;

	}


	public enum CarState
	{
		NoMove,
		Moving,
		Auto,
	}

	public enum CarGear
	{
		Accelerating,
		Braking,
		Reversing,
		Stopped,
	}


}
