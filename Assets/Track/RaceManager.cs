using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IsaacFagg.Player;
using IsaacFagg.Track3;
using IsaacFagg.Utility;
using IsaacFagg.Cars;

namespace IsaacFagg.Race
{
	public class RaceManager : MonoBehaviour
	{
		[Header("Race Properties")]
		Track3Generator trackGen;
		private Car car;
		public GameObject carPrefab;

		public List<Checkpoint> checkpoints;
		public List<Vector2> checkpointLocations;
		[HideInInspector]
		public Vector2 playerSpawn;
		[HideInInspector]
		public Quaternion playerRot;

		[Header("Race Settings")]
		public Race race;
		public int maxLaps = 5;
		public float countdownTime = 2;

		[Header("Race Statistics")]

		//Start of Race
		bool isRaceStarted = false;
		bool isRaceFinished = false;
		public float countdown = 0;


		//Laps
		bool isLapStarted = false;
		float currentLapStartTime;
		public float distanceAtLastCheckpoint = 0f;
		public float lapCount = 0;
		public Lap lastLap;
		public Lap bestLap;
		public Lap currentLap;

		public float totalTime;

		public float lastCheckpoint;
		public List<Checkpoint> passedCheck;



		private void Start()
		{
			car = GameObject.FindGameObjectWithTag("Player").GetComponent<Car>();
			trackGen = GameObject.FindGameObjectWithTag("Track").GetComponent<Track3Generator>();

			GetVariables();

			SpawnCar();


			StartCoroutine(StartRaceCountdown());

			car.carState = CarState.Off;
		}

		private void Update()
		{
			Countdown();

			if (isRaceStarted && !isRaceFinished)
			{
				totalTime += Time.deltaTime;
			}
		}

		#region Race Preperations


		private void GetVariables()
		{


			checkpoints = trackGen.checkpoints;
			playerSpawn = trackGen.checkpointLocations[0];

			//Need to fix
			Vector3 targetPos = trackGen.checkpointLocations[1];
			playerRot = MathsUtility.LookAt(playerSpawn, targetPos);


			checkpoints = trackGen.checkpoints;
		}


		private void SpawnCar()
		{
			if (GameObject.Find("Car"))
			{
				car = GameObject.Find("Car").GetComponent<Car>();
			}
			else
			{
				car = Instantiate(carPrefab, Vector2.zero, Quaternion.identity).GetComponent<Car>(); ;
			}

			car.transform.position = playerSpawn;
			car.transform.rotation = playerRot;

		}


		IEnumerator StartRaceCountdown()
		{
			countdown = countdownTime;

			yield return new WaitForSecondsRealtime(countdownTime);

			if (isRaceStarted == false)
			{
				StartRace();
				isRaceStarted = true;
			}
		}

		private void Countdown()
		{
			if (countdown > 0)
			{
				countdown -= Time.deltaTime;

			}
			else
			{
				countdown = 0;
			}


		}

		#endregion

		#region Race

		private void StartRace()
		{
			car.carState = CarState.On;

			race = new Race();

			lapCount = 0;
			lastCheckpoint = 0;
			distanceAtLastCheckpoint = car.distanceTravelled;

			currentLap = new Lap();
		}

		public void HitCheckpoint(Checkpoint checkpoint)
		{
			if (checkpoint.position == lastCheckpoint + 1)
			{
				float newLength = 0;

				if (checkpoint.position != checkpoints.Count - 1)
				{
					newLength = Vector2.Distance(trackGen.checkpointLocations[checkpoint.position], trackGen.checkpointLocations[checkpoint.position + 1]);
				}
				else
				{
					newLength = Vector2.Distance(trackGen.checkpointLocations[checkpoint.position], trackGen.checkpointLocations[0]);
				}

				passedCheck.Add(checkpoint);
				PassCheckpoint(newLength);
			}
			else if (checkpoint.position == lastCheckpoint)
			{
				Debug.Log("Same checkpoint");
			}
			else if (checkpoints.Count == passedCheck.Count)
			{
				PassFinish();
			}
			else if (checkpoint.position != lastCheckpoint + 1 && checkpoint.position < lastCheckpoint)
			{
				Debug.Log("right way but you skipped one");
			}
			else
			{
				Debug.Log("Going the wrong way");
			}
		}


		public void PassCheckpoint(float lengthBetweenCheckpoints)
		{

			lastCheckpoint++;

			currentLap.EndSplit(0, distanceAtLastCheckpoint, car.distanceTravelled, lengthBetweenCheckpoints);

			distanceAtLastCheckpoint = car.distanceTravelled;

		}

		public void PassFinish()
		{
			lastCheckpoint = 0;
			lapCount++;

			passedCheck.Clear();


			Debug.Log("newLap");
		}




		#endregion

		#region After Race

		#endregion

		#region Utility

		





		#endregion


	}

	public class Race
	{
		public List<Lap> laps = new List<Lap>();

		public float time
		{
			get
			{
				if (laps.Count == 0)
				{
					return 0f;
				}

				float total = 0;

				foreach (Lap item in laps)
				{
					total += item.time;
				}

				return total;
			}
		}
		public float distance
		{
			get
			{
				if (laps.Count == 0)
				{
					return 0f;
				}

				float total = 0;

				foreach (Lap item in laps)
				{
					total += item.distance;
				}

				return total;
			}
		}
		public float averageSpeed
		{
			get
			{
				return distance / time;
			}
		}

		public void EndLap()
		{
			Lap lap = new Lap
			{
				position = laps.Count
			};

			laps.Add(lap);

			Debug.Log(averageSpeed);
		}



	}

	public class Lap
	{
		public List<Split> splits = new List<Split>();

		public int position;
		public float time
		{
			get
			{
				if (splits.Count == 0)
				{
					return 0f;
				}

				float total = 0;

				foreach (Split item in splits)
				{
					total += item.time;
				}

				return total;
			}
		}

		public float distance
		{
			get
			{
				if (splits.Count == 0)
				{
					return 0f;
				}

				float total = 0;

				foreach (Split item in splits)
				{
					total += item.distance;
				}

				return total;
			}
		}
		public float averageSpeed
		{
			get
			{
				return distance / time;
			}
		}

		public void EndSplit(float time, float startDis, float endDis, float length)
		{
			Split split = new Split();

			split.position = splits.Count;
			split.time = time;

			split.distance = endDis - startDis;
			split.length = length;

			splits.Add(split);

		}


	}

	public class Split
	{
		public int position;
		public float time;

		public float distance; //distance travelled
		public float length; //Distance between checkpoints

		public float averageSpeed
		{
			get
			{
				return distance / time;
			}
		}

	}

}
