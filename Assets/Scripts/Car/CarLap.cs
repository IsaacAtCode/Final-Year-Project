using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using IsaacFagg.UI;

namespace IsaacFagg
{
	[RequireComponent(typeof(Car))]
	public class CarLap : MonoBehaviour
	{
		private Rigidbody2D rb;
		private TrackManager trackM;
		private CarController cc;

		public PlayerHUD hud;

		public enum RaceState
		{
			Starting,
			Ongoing,
			Paused,
			Finished,
		}
		public RaceState raceState;


		[Header("Lap")]
		public List<Lap> laps = new List<Lap>();
		public Lap currentLap;
		public Lap lastLap;
		public Lap bestLap;
		public int lapCount = 0;
		public float lapTotal;


		[Header("Checkpoints")]
		public int lastCheck;
		public List<Checkpoint> passedCheck;


		//Race Settings
		public int countdown = 3;
		public float countdownTimer;

		private void Start()
		{
			rb = GetComponent<Rigidbody2D>();
			trackM = GameObject.FindGameObjectWithTag("Track").GetComponent<TrackManager>();
			cc = GetComponent<CarController>();

			StartCoroutine(StartRace());
			raceState = RaceState.Starting;


			//hud.UpdateCounter(lastCheck.ToString() + "/" + trackM.maxCheckpoints, hud.checkCount);
			hud.UpdateCounter((lapCount + 1).ToString() + "/" + trackM.maxLaps + " Laps", hud.lapCount);

		}

		private void Update()
		{
			//Check if its player or ai
			if (raceState == RaceState.Starting)
			{
				cc.carState = CarController.CarState.NoMove;
				StartCountdown();
			}
			else if (raceState == RaceState.Ongoing)
			{
				cc.carState = CarController.CarState.Moving;
				lapTotal += Time.deltaTime;
				currentLap.time += Time.deltaTime;
			}
			else if (raceState == RaceState.Finished)
			{
				cc.carState = CarController.CarState.Auto;
				//Finish
			}

		}


		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.tag == "Checkpoint")
			{
				Checkpoint cp = other.gameObject.GetComponent<Checkpoint>();

				if (cp.finishLine == true && CheckAllCheckpointsPassed())
				{
					if (lapCount == trackM.maxLaps - 1)
					{
						raceState = RaceState.Finished;
					}
						FinishLap();
				}
				else
				{
					CheckpointPass(cp);
				}

				//hud.UpdateCounter(lastCheck.ToString() + "/" + trackM.maxCheckpoints, hud.checkCount);

			}
		}

		IEnumerator StartRace()
		{
			
			countdownTimer = countdown;

			yield return new WaitForSecondsRealtime(countdown);

			if (raceState == RaceState.Starting)
			{
				raceState = RaceState.Ongoing;
				CreateCurrentLap();
			}
		}

		private void StartCountdown()
		{
			countdownTimer -= Time.deltaTime;
		}

		private void CheckpointPass(Checkpoint check)
		{
			
			if (check.position == lastCheck + 1)
			{
				passedCheck.Add(check);
				lastCheck++;

				//split
			}
		}

		private void FinishLap()
		{
			lastCheck = 0;
			lapCount++;
			passedCheck.Clear();

			SetLastLap();

			hud.UpdateCounter((lapCount + 1).ToString() + "/" + trackM.maxLaps + " Laps", hud.lapCount);
		}

		private bool CheckAllCheckpointsPassed()
		{
			if (passedCheck.Count == trackM.maxCheckpoints - 1 && lastCheck != 0)
			{
				return true;
			}
			else
			{
				Debug.Log("You need to go through all the checkpoints");
				return false;
			}
		}

		private void CreateCurrentLap()
		{
			if (currentLap == null)
			{
				currentLap = new Lap();
			}
			else
			{
				currentLap.time = 0;
				currentLap.position = lapCount;
				//currentLap.playerPosition = 
				//currentLap.splits.Clear();
			}
		}

		private void SetLastLap()
		{
			if (lastLap == null)
			{

				lastLap = new Lap();
			}

			lastLap = currentLap.DeepCopy(); ;

			hud.UpdateCounter(hud.FormatLapTime(lastLap.time), hud.lastLap);
			hud.UpdateCounter(hud.FormatLapTime(lapTotal), hud.lapTime);

			AddLap(lastLap);
			//AddToLapsList

			if (raceState == RaceState.Ongoing)
			{
				CreateCurrentLap();
			}
		}

		private void AddLap(Lap newLap)
		{
			laps.Add(lastLap);
			SortLaps();
			hud.UpdateCounter(hud.FormatLapTime(bestLap.time), hud.bestLap);

		}

		private void SortLaps()
		{
			List<Lap> fastLaps = laps.OrderBy(Lap => Lap.time).ToList();

			bestLap = fastLaps[0].DeepCopy();
		}
	}
}

