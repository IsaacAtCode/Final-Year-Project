using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace IsaacFagg
{
	[RequireComponent(typeof(CarStats))]
	public class CarController : MonoBehaviour
	{

		private CarStats cStats;
		private Rigidbody2D rb;

		//Wheels
		public GameObject wheel_fl;
		public GameObject wheel_fr;
		public GameObject wheel_bl;
		public GameObject wheel_br;
		public float wheelMinMax = 10f;
		public float backWheelTurn = 0.5f;

		public enum CarState { Accelerating, Braking, Reversing}
		public CarState carState; 


		public void Start()
		{
			cStats = GetComponent<CarStats>();
			rb = GetComponent<Rigidbody2D>();
		}

		private void Update()
		{
			WheelTurn();

			//Debug.Log(rb.velocity.x * rb.velocity.y);

		}



		private void FixedUpdate()
		{
			Accelerate();
			Brake();
			Turn();

			CalculateVelocity();

		}


		private void Accelerate()
		{
			if (Input.GetButton("Accelerate"))
			{
				rb.AddForce(transform.up * cStats.acceleration);

			}
		}

		private void Brake()
		{
			if (Input.GetButton("Brake"))
			{
				rb.AddForce(transform.up * -cStats.acceleration);

			}
		}

		private void Turn()
		{
			rb.angularVelocity = (Input.GetAxis("Horizontal") * -cStats.torque);
		}

		/*CarState GearShift (CarState state)
		{
			if (Input.GetButtonDown("Accelerate"))
			{
				state = CarState.Accelerating;
			}
			else if (Input.GetButton("Brake"))
			{
				state = CarState.Braking;
			}
			else if (Input.GetButton("Brake"))
			{
				state = CarState.Reversing;
			}

				return state;
		}*/



		private void CalculateVelocity()
		{
			rb.velocity = ForwardVelocity() + (RightVelocity()*cStats.drift);

		}

		private void WheelTurn()
		{

			float wheelRot = Mathf.Clamp(Input.GetAxisRaw("Horizontal") * wheelMinMax, -wheelMinMax, wheelMinMax);
			if (wheelRot > wheelMinMax)
			{
				wheelRot = wheelMinMax;
			}
			else if (wheelRot < -wheelMinMax)
			{
				wheelRot = -wheelMinMax;
			}

			Vector3 frontWheelRotation =  new Vector3(0f, 0f, -wheelRot);
			Vector3 backWheelRotation = new Vector3(0f, 0f, wheelRot * backWheelTurn);



			wheel_fl.transform.localRotation = Quaternion.Lerp(wheel_fl.transform.rotation, Quaternion.Euler(frontWheelRotation), 1f);
			wheel_fr.transform.localRotation = Quaternion.Lerp(wheel_fr.transform.rotation, Quaternion.Euler(frontWheelRotation), 1f);

			wheel_bl.transform.localRotation = Quaternion.Lerp(wheel_bl.transform.rotation, Quaternion.Euler(backWheelRotation), 1f);
			wheel_br.transform.localRotation = Quaternion.Lerp(wheel_br.transform.rotation, Quaternion.Euler(backWheelRotation), 1f);




		}




		Vector2 ForwardVelocity()
		{
			return transform.up * Vector2.Dot(rb.velocity, transform.up);
		}
		Vector2 RightVelocity()
		{
			return transform.right * Vector2.Dot(rb.velocity, transform.right);
		}



	}
}
