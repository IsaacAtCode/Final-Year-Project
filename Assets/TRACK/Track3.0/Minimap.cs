using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacFagg.Track3
{
	[ExecuteInEditMode]
	public class Minimap : MonoBehaviour
	{
		public Track3 track;
		public GameObject minimapTrack;
		public float offset;

		private Camera trackCamera;

		private void Update()
		{
			if (!track)
			{
				track = GameObject.FindGameObjectWithTag("Track").GetComponent<Track3>();
			}
			if (!minimapTrack)
			{
				minimapTrack = GameObject.FindGameObjectWithTag("MinimapTrack");
			}
			if (!trackCamera)
			{
				trackCamera = GetComponent<Camera>();
			}

			CalculateOffset();
			CentreCamera();
		}

		private void CalculateOffset()
		{
			offset = ((track.width * 1.5f) / 2) / Mathf.Tan(trackCamera.fieldOfView);

			trackCamera.farClipPlane = Mathf.Abs(offset * 1.1f);
		}



		private void CentreCamera()
		{
			Vector3 trackPos = new Vector3(minimapTrack.transform.position.x + track.width / 2, minimapTrack.transform.position.y + track.height / 2, minimapTrack.transform.position.z + offset);



			//Vector3 camPos = new Vector3(trackPos.x, trackPos.y, offset);

			transform.position = trackPos;
		}




	}
}
