using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacFagg
{
	[RequireComponent(typeof(Car))]
	public class CarLap : MonoBehaviour
	{
		[SerializeField] private Car car;
		[SerializeField] private Rigidbody2D rb;
		[SerializeField] private Track track;


		//Laps
		public Lap[] laps;
		public Lap lastLap;
		public Lap bestLap;
		public int lapCount;
		public float lapTotal;
		public float currentLap;

		//Checkpoints
		public int lastCheck;
		public List<Checkpoint> passedCheck;


		public bool finishedRace = false;

		private void Start()
		{
			car = GetComponent<Car>();
			rb = GetComponent<Rigidbody2D>();
			track = GameObject.FindGameObjectWithTag("Track").GetComponent<Track>();
		}

		private void Update()
		{
			lapTotal += Time.deltaTime;
			currentLap += Time.deltaTime;

		}


		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.tag == "Checkpoint")
			{
				Checkpoint cp = other.gameObject.GetComponent<Checkpoint>();

				if (cp.finishLine == true)
				{
					if (lapCount == track.maxLaps - 1)
					{
						Debug.Log("finished the whole race");
						finishedRace = true;
					}
					else
					{
						//Finish Lap
						FinishPass(cp);
					}
					
				}
				else
				{
					CheckpointPass(cp);
				}

			}
		}


		private void CheckpointPass(Checkpoint check)
		{
			
			if (check.position == lastCheck + 1)
			{
				passedCheck.Add(check);
				lastCheck++;
			}

		}

		private void FinishPass(Checkpoint check)
		{
			if (check.finishLine == true && CheckAllCheckpointsPassed() == true)
			{
				lastCheck = 0;
				lapCount++;
				passedCheck.Clear();
				Debug.Log("Fin lap");

				//Store Lap
			}
		}

		private bool CheckAllCheckpointsPassed()
		{
			if (passedCheck.Count == track.maxCheckpoints - 1 && lastCheck != 0)
			{
				return true;
			}
			else
			{
				Debug.Log("You need to go through all the checkpoints");
				return false;
			}


		}





		//NewLap

		//TrackLap

		//EndLap & Store it


		//Set Last Lap

		//FindBestLap

	}



}
