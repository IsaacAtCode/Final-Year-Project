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
		private Track track;

		public PlayerHUD hud;

		[Header("Lap")]
		public List<Lap> laps = new List<Lap>();
		public Lap currentLap;
		public Lap lastLap;
		public Lap bestLap;
		public int lapCount = 0;
		public float lapTotal;
		public float raceTotal;


		[Header("Checkpoints")]
		public int lastCheck;
		public List<Checkpoint> passedCheck;


		//Race Settings
		public int countdown = 3;
		public float countdownTimer;
		public bool startedRace = false;
		public bool finishedRace = false;

		private void Start()
		{
			rb = GetComponent<Rigidbody2D>();
			track = GameObject.FindGameObjectWithTag("Track").GetComponent<Track>();

			StartCoroutine(StartRace());

			hud.UpdateCounter(lastCheck.ToString() + "/" + track.maxCheckpoints, hud.checkCount);
			hud.UpdateCounter((lapCount + 1).ToString() + "/" + track.maxLaps + " Laps", hud.lapCount);
		}

		private void Update()
		{
			if (startedRace == false)
			{
				StartCountdown();
			}
			else if (startedRace == true && finishedRace == false)
			{
				lapTotal += Time.deltaTime;
				currentLap.time += Time.deltaTime;
			}

		}


		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.tag == "Checkpoint")
			{
				Checkpoint cp = other.gameObject.GetComponent<Checkpoint>();

				if (cp.finishLine == true && CheckAllCheckpointsPassed())
				{
					if (lapCount == track.maxLaps - 1)
					{
						finishedRace = true;
					}
						//Finish Lap
						FinishLap();
				}
				else
				{
					CheckpointPass(cp);
				}

				hud.UpdateCounter(lastCheck.ToString() + "/" + track.maxCheckpoints, hud.checkCount);

			}
		}

		IEnumerator StartRace()
		{
			countdownTimer = countdown;

			yield return new WaitForSecondsRealtime(countdown);

			if (startedRace == false)
			{
				CreateCurrentLap();
				startedRace = true;
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

			hud.UpdateCounter((lapCount + 1).ToString() + "/" + track.maxLaps + " Laps", hud.lapCount);
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

			if (finishedRace == false)
			{
				CreateCurrentLap();
			}
		}

		private void AddLap(Lap newLap)
		{
			laps.Add(lastLap);
			SortLaps();
		}

		private void SortLaps()
		{
			List<Lap> fastLaps = laps.OrderBy(Lap => Lap.time).ToList();

			foreach (Lap item in fastLaps)
			{
				Debug.Log(item.time);
			}

			bestLap = fastLaps[0].DeepCopy();
		}

	}






		//Store Lap then create a new one

		//Set Last Lap

		//FindBestLap

}

