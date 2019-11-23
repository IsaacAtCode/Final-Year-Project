using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacFagg
{
	public class Car : MonoBehaviour
	{
		public float acceleration;
		public float torque;
		[Range(0, 1)]
		public float drift;

	}
}
