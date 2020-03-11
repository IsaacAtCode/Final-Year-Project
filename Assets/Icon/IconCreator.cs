using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IsaacFagg.Track;

namespace IsaacFagg.Icons
{
    [RequireComponent(typeof(IconGenerator))]
    public class IconCreator : MonoBehaviour
    {
        public Texture2D CreateIcon(List<Vector2> points)
        {
            //Camera iconCam = GetComponentInChildren<Camera>();
            Camera iconCam = new GameObject("Icon Camera").AddComponent<Camera>();
            iconCam.transform.parent = this.transform;

            IconGenerator mg = GetComponent<IconGenerator>();
            mg.GenerateTrackMesh(points);

            Bounds bounds = GetComponent<Renderer>().bounds;

            FocusCameraOnBounds(iconCam, bounds);

            Texture2D texture = RenderToTexture(iconCam);

            Destroy(iconCam.gameObject  );


            return texture;
        }

        private void FocusCameraOnBounds(Camera cam, Bounds bounds)
        {
            cam.orthographic = true;
            cam.orthographicSize = (Mathf.Max(bounds.size.x, bounds.size.y) * 1.15f) / 2;
            cam.transform.position = new Vector3(bounds.center.x, bounds.center.y, -1f);
        }


        private Texture2D RenderToTexture(Camera cam)
        {
            RenderTexture rt = new RenderTexture(256, 256, 16);
            cam.targetTexture = rt;

            Texture2D image = new Texture2D(246, 256);
            cam.Render();

            RenderTexture.active = rt;
            image.ReadPixels(new Rect(0, 0, 256, 256), 0, 0);
            image.Apply();

            cam.targetTexture = null;
            RenderTexture.active = null;
            Destroy(rt);

            return image;

        }


    }
}
