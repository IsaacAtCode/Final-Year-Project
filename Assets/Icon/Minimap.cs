using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IsaacFagg.Track;

namespace IsaacFagg.UI
{
	public class Minimap : MonoBehaviour
	{
		public GameObject minimapTrack;
		public float offset;

		private Camera trackCamera;

		private void Update()
		{
			if (!minimapTrack)
			{
				minimapTrack = GameObject.FindGameObjectWithTag("MinimapTrack");
			}
			if (!trackCamera)
			{
				trackCamera = GetComponent<Camera>();
			}

			//CalculateOffset();
			//CentreCamera();


			Bounds bounds = minimapTrack.GetComponent<Renderer>().bounds;
			FocusCameraOnBounds(trackCamera, bounds);

		}

		//private void CalculateOffset()
		//{
		//	offset = ((track.Width * 1.5f) / 2) / Mathf.Tan(trackCamera.fieldOfView);

		//	trackCamera.farClipPlane = Mathf.Abs(offset * 1.1f);
		//}

		private void FocusCameraOnBounds(Camera cam, Bounds bounds)
		{
			cam.orthographic = true;
			cam.orthographicSize = (Mathf.Max(bounds.size.x, bounds.size.y) * 1.15f) / 2;
			cam.transform.position = new Vector3(bounds.center.x, bounds.center.y, -1f);
		}


		//private void CentreCamera()
		//{
		//	Vector3 trackPos = new Vector3(minimapTrack.transform.position.x + track.Width / 2, minimapTrack.transform.position.y + track.Height / 2, minimapTrack.transform.position.z + offset);



		//	//Vector3 camPos = new Vector3(trackPos.x, trackPos.y, offset);

		//	transform.position = trackPos;
		//}


	}
}
