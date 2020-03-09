using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IsaacFagg.Player;
using IsaacFagg.Utility;
using IsaacFagg.Cars;
using IsaacFagg.Track;

namespace IsaacFagg.Race
{
	public class RaceManager : MonoBehaviour
	{
		[Header("Race Properties")]
		TrackGenerator trackGen;
		private Car car;
		public GameObject carPrefab;
		RaceUI raceUI;

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
		public bool isRaceStarted = false;
		public bool isRaceFinished = false;
		public float countdown = 0;


		[Header("Laps")]
		public int lapCount = 0;
		float distanceAtLastCheckpoint = 0f;

		public Lap lastLap
		{
			get
			{
				return race.laps[lapCount - 1];
			}
		}
		public Lap bestLap
		{
			get
			{
				List<Lap> allLaps = new List<Lap>();

				allLaps = race.laps.OrderBy(e => e.time).ToList();

				return allLaps[0];
			}
		}
		public Lap currentLap;

		private List<Lap> laps;

		public List<Lap> Laps
		{
			get
			{
				return laps;
			}
		}






		//Time
		public float totalTime;
		public float currentLapTime;
		float timeAtLastCheckpoint;

		public float lastCheckpoint;
		public List<Checkpoint> passedCheck;

		private void Awake()
		{
			car = GameObject.FindGameObjectWithTag("Player").GetComponent<Car>();
			trackGen = GameObject.FindGameObjectWithTag("Track").GetComponent<TrackGenerator>();
			raceUI = GetComponent<RaceUI>();
		}

		private void Start()
		{
			GetVariables();

			SpawnCar();

			StartCoroutine(StartRaceCountdown());

			car.carState = CarState.Off;

			raceUI.UpdateLap(lapCount, maxLaps);

			laps = new List<Lap>();


		}

		private void Update()
		{
			Countdown();

			if (isRaceStarted && !isRaceFinished)
			{
				totalTime += Time.deltaTime;
				currentLapTime += Time.deltaTime;
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
				car.transform.position = playerSpawn;
				car.transform.rotation = playerRot;
			}
			else
			{
				car = Instantiate(carPrefab, playerSpawn, playerRot).GetComponent<Car>(); ;
			}

			

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
			float newLength;

			if (checkpoint.position == lastCheckpoint + 1)
			{
				

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
				//Debug.Log("Same checkpoint");
			}
			else if (checkpoints.Count - 1 == passedCheck.Count && checkpoint.finishLine)
			{
				newLength = Vector2.Distance(trackGen.checkpointLocations[checkpoint.position], trackGen.checkpointLocations[0]);

				PassCheckpoint(newLength);
				PassFinish();

				if (lapCount >= maxLaps)
				{
					FinishRace();
				}
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

			float time = totalTime - timeAtLastCheckpoint;
			float distance = car.distanceTravelled - distanceAtLastCheckpoint;

			currentLap.EndSplit(time, distance, lengthBetweenCheckpoints);

			distanceAtLastCheckpoint = car.distanceTravelled;
			timeAtLastCheckpoint = totalTime;

			//Debug.Log(currentLap.distance);

		}

		public void PassFinish()
		{
			race.EndLap(currentLap);

			laps.Add(currentLap);


			lastCheckpoint = 0;
			lapCount++;


			currentLap = new Lap();
			passedCheck.Clear();

			currentLapTime = 0;

			raceUI.UpdateLap(lapCount, maxLaps);
		}

		private void FinishRace()
		{
			isRaceFinished = true;
			car.carState = CarState.Auto;
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

		public void EndLap(Lap lap)
		{
			laps.Add(lap);
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

		public void EndSplit(float time, float dis, float length)
		{
			Split split = new Split();

			split.position = splits.Count;
			split.time = time;

			split.distance = dis;
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
