//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;


//	public class CarController : MonoBehaviour
//	{

//		private Car car;
//		public Rigidbody2D rb;

//		public TrackManager trackM;

//		public float turnRate = 0.5f;

//		public CarState carState;

//		public CarGear carGear;

//		public CarDirection carDirection = CarDirection.Forward;

//		public void Start()
//		{
//			car = GetComponent<Car>();
//			rb = GetComponent<Rigidbody2D>();
//			//cl = GetComponent<CarLap>();

//			//trackM = GameObject.Find("Track").GetComponent<TrackManager>();
//		}

//		private void Update()
//		{
//			car.WheelTurn(carDirection);
//		}

//		private void FixedUpdate()
//		{
//			if (carState == CarState.Moving)
//			{
//				Moving();
//				Turning();
//			}


//			CalculateVelocity();
//		}

//		private void TouchControls()
//		{
//			if (Input.touchCount > 0)
//			{
//				Touch touch = Input.GetTouch(0);
//			}
//		}




//		private void Moving()
//		{
//			if (carGear == CarGear.Accelerating)
//			{
//				rb.drag = 1f;
//				rb.AddForce(transform.up * car.acceleration);
//			}
//			else if (carGear == CarGear.Braking)
//			{
//				rb.AddForce(transform.up * -car.acceleration);
//			}

//		}

//		public void StartBrake()
//		{
//			carGear = CarGear.Braking;
//		}

//		public void EndBrake()
//		{
//			carGear = CarGear.Accelerating;
//		}

//		private void Turning()
//		{
//			//Get the longest or newest touch?
//			//Sensitivy Settings

//			if (carDirection == CarTurn.Left)
//			{
//				rb.angularVelocity = (-turnRate * -car.torque);
//			}
//			if (carDirection == CarTurn.Right)
//			{
//				rb.angularVelocity = (turnRate * -car.torque);
//			}



//		}

//		public void TouchTurnLeft()
//		{
//			if (carDirection == CarDirection.Forward)
//			{
//				carDirection = CarDirection.Left;
//			}
//			else if (carDirection == CarDirection.Right)
//			{
//				carDirection = CarDirection.Both;
//			}

//		}

//		public void TouchTurnRight()
//		{
//			if (carDirection == CarDirection.Forward)
//			{
//				carDirection = CarDirection.Right;
//			}
//			else if (carDirection == CarDirection.Left)
//			{
//				carDirection = CarDirection.Both;
//			}

//		}

//		public void StopTurning()
//		{
//			if (carDirection == CarDirection.Both)
//			{

//			}
//			else
//			{
//				carDirection = CarDirection.Forward;
//			}


//		}


//		private void CalculateVelocity()
//		{
//			rb.velocity = ForwardVelocity() + (RightVelocity() * car.drift);

//		}

//		Vector2 ForwardVelocity()
//		{
//			return transform.up * Vector2.Dot(rb.velocity, transform.up);
//		}
//		Vector2 RightVelocity()
//		{
//			return transform.right * Vector2.Dot(rb.velocity, transform.right);
//		}

//	}

