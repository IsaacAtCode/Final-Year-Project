using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IsaacFagg.Tracks;
using IsaacFagg.Player;

public class TrackGenerator : MonoBehaviour
{
	[SerializeField]
	public Track track;
	public GameObject trackGO;

	public PlayerData pd;

	//Generate track from track
	//Generate track from nothing?

	private void Awake()
	{
		pd = Object.FindObjectOfType<PlayerData>();
	}

	private void Start()
	{
		
	}







}
