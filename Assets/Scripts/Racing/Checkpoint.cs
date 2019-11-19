using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int position;
    public bool finishLine;
    public TrackManager tm;

    private void Start()
    {
        tm = GameObject.Find("TrackManager").GetComponent<TrackManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (finishLine ==true)
            {
                tm.StartNewLap();
                tm.UpdateCheckpoint(0);
            }
            tm.UpdateCheckpoint(position);
        }

    }



}
