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

		[Header("Wheels")]
		public GameObject wheel_fl;
		public GameObject wheel_fr;
		public GameObject wheel_bl;
		public GameObject wheel_br;
		public float wheelMinMax = 10f;
		public float backWheelTurn = 0.5f;

		public void WheelTurn(CarDirection dir)
		{

			float wheelRot = 0f;

			if (dir == CarDirection.Left)
			{
				wheelRot = Mathf.Clamp(-1f * wheelMinMax, -wheelMinMax, wheelMinMax);
			}
			if (dir == CarDirection.Right)
			{
				wheelRot = Mathf.Clamp(1f * wheelMinMax, -wheelMinMax, wheelMinMax);
			}

			Vector3 frontWheelRotation = new Vector3(0f, 0f, -wheelRot);
			Vector3 backWheelRotation = new Vector3(0f, 0f, wheelRot * backWheelTurn);

			wheel_fl.transform.localRotation = Quaternion.Lerp(wheel_fl.transform.rotation, Quaternion.Euler(frontWheelRotation), 1f);
			wheel_fr.transform.localRotation = Quaternion.Lerp(wheel_fr.transform.rotation, Quaternion.Euler(frontWheelRotation), 1f);

			wheel_bl.transform.localRotation = Quaternion.Lerp(wheel_bl.transform.rotation, Quaternion.Euler(backWheelRotation), 1f);
			wheel_br.transform.localRotation = Quaternion.Lerp(wheel_br.transform.rotation, Quaternion.Euler(backWheelRotation), 1f);
		}


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

	public enum CarDirection
	{
		Left,
		Forward,
		Right,
		Both,
		//Backwards,
	}
}
