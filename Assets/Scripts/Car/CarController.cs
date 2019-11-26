using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace IsaacFagg
{
	[RequireComponent(typeof(Car))]
	public class CarController : MonoBehaviour
	{

		private Car car;
		public Rigidbody2D rb;
		private CarLap cl;

		[Header("Wheels")]
		public GameObject wheel_fl;
		public GameObject wheel_fr;
		public GameObject wheel_bl;
		public GameObject wheel_br;
		public float wheelMinMax = 10f;
		public float backWheelTurn = 0.5f;

		public Track currentTrack;

        public float turnRate = 0.5f;

		public enum CarState
		{
			NoMove,
			Moving,
			Auto,
		}
		public CarState carState;

		public enum CarGear
		{
			Accelerating,
			Braking,
			Reversing,
			Stopped,
		}
		public CarGear carGear;

		public enum CarDirection
		{
			Left,
			Forward,
			Right,
            Both,
			//Backwards,
		}
		public CarDirection carDirection = CarDirection.Forward;


		public void Start()
		{
			car = GetComponent<Car>();
			rb = GetComponent<Rigidbody2D>();
			cl = GetComponent<CarLap>();

			currentTrack = GameObject.Find("Track").GetComponent<Track>();





		}

		private void Update()
		{
			WheelTurn();
		}

		private void FixedUpdate()
		{
			if (carState == CarState.Moving)
			{
				Moving();
				Turning();
			}
			

			CalculateVelocity();
		}

		private void TouchControls()
		{
			if (Input.touchCount > 0)
			{
				Touch touch = Input.GetTouch(0);
			}
		}




		private void Moving()
		{
			if (carGear == CarGear.Accelerating)
			{
                rb.drag = 1f;
                rb.AddForce(transform.up * car.acceleration);
			}
			else if (carGear == CarGear.Braking)
			{
                rb.AddForce(transform.up * -car.acceleration);
			}
			
		}

		public void StartBrake()
		{
			carGear = CarGear.Braking;
		}

		public void EndBrake()
		{
			carGear = CarGear.Accelerating;
		}

		private void Turning()
		{
			//Get the longest or newest touch?
			//Sensitivy Settings

			if (carDirection == CarDirection.Left)
			{
				rb.angularVelocity = (-turnRate * -car.torque);
			}
			if (carDirection == CarDirection.Right)
			{
				rb.angularVelocity = (turnRate * -car.torque);
			}



		}

		public void TouchTurnLeft()
		{
            if (carDirection == CarDirection.Forward)
            {
                carDirection = CarDirection.Left;
            }
            else if (carDirection == CarDirection.Right)
            {
                carDirection = CarDirection.Both;
            }
			
		}

		public void TouchTurnRight()
		{
            if (carDirection == CarDirection.Forward)
            {
                carDirection = CarDirection.Right;
            }
            else if (carDirection == CarDirection.Left)
            {
                carDirection = CarDirection.Both;
            }

        }

		public void StopTurning()
		{
            if (carDirection == CarDirection.Both)
            {

            }
            else
            {
                carDirection = CarDirection.Forward;
            }

			
		}


		private void CalculateVelocity()
		{
			rb.velocity = ForwardVelocity() + (RightVelocity()*car.drift);

		}

		private void WheelTurn()
		{

			float wheelRot = 0f;
			
			if (carDirection == CarDirection.Left)
			{
				wheelRot = Mathf.Clamp(-1f * wheelMinMax, -wheelMinMax, wheelMinMax);
			}
			if (carDirection == CarDirection.Right)
			{
				wheelRot = Mathf.Clamp(1f * wheelMinMax, -wheelMinMax, wheelMinMax);
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
