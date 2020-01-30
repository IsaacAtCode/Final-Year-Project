// Made by Isaac Fagg
// Final Year Project
// 30/01/2020

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IsaacFagg.Paths;

namespace IsaacFagg.Track3
{
    [ExecuteInEditMode]
    public class Track3Generator : MonoBehaviour
    {
        [Header("Components")]
        public Track3 track;
        public RandomNameGenerator randomNameGenerator;

        [Header("Road")]
        public Material roadMat;
        public Material gravelMat;
        public Sprite background;

        public bool generateNewTrack = false;

        private void Start()
        {
            if (!track)
            {
                track = GetComponent<Track3>();
            }
            if (!randomNameGenerator)
            {
                randomNameGenerator = GetComponent<RandomNameGenerator>();
            }
        }

        private void Update()
        {
            if (generateNewTrack)
            {
                track.GenerateTrack();
                if (track.type == TrackType.Random)
                {
                    track.name = randomNameGenerator.GenerateName();
                }

                generateNewTrack = false;



                GenerateFromTrack();



            }
        }

        private void GenerateFromTrack()
        {
            DeleteOldTrack();

            GameObject trackGO = new GameObject(track.name);
            GameObject gravelGO = new GameObject("Gravel");
            trackGO.transform.parent = this.gameObject.transform;
            gravelGO.transform.parent = trackGO.transform;
            trackGO.tag = ("CurrentTrack");

            PathCreator tPC = trackGO.AddComponent<PathCreator>();
            PathCreator gPC = gravelGO.AddComponent<PathCreator>();

            GenerateBevierPath(tPC);
            gPC.path = tPC.path;
            AddRoad(trackGO, roadMat, 20f);
            AddRoad(gravelGO, gravelMat, 30f);
            //AddBackground(background);

            //GenerateCheckpoints(tPC.path, trackGO);
        }

        public void GenerateBevierPath(PathCreator pc)
        {

            if (track.points.Count > 1)
            {
                pc.path.MovePoint(0, track.points[0]);

                pc.path.MovePoint(3, track.points[1]);



                for (int i = 0; i < track.points.Count - 2; i++)
                {
                    //Vector2 anchorPos = new Vector2(allPoints[i + 2].x, allPoints[i + 2].y);

                    pc.path.AddSegment(track.points[i + 2]);
                }
            }

            pc.path.IsClosed = true;
            pc.path.AutoSetControlPoints = true;
        }

        public void AddRoad(GameObject go, Material mat, float width)
        {
            RoadCreator rc = go.AddComponent<RoadCreator>();
            MeshRenderer mr = go.GetComponent<MeshRenderer>();
            mr.material = mat;
            mr.sortingLayerName = "Track";
            mr.sortingOrder = 0;

            AddCollider(go);

            rc.roadWidth = width;
            rc.UpdateRoad();
        }

        private void AddCollider(GameObject go)
        {
            go.AddComponent<Mesh2DColliderMaker>();

            go.GetComponent<PolygonCollider2D>().isTrigger = true;
        }

        private void AddBackground(Sprite backgroundSprite)
        {
            GameObject oldBackground = GameObject.Find("Background");
            DestroyImmediate(oldBackground);


            GameObject background = new GameObject("Background");
            background.layer = (12);
            background.transform.position = new Vector3(0, 0, 30f);
            SpriteRenderer spriteRender = background.AddComponent<SpriteRenderer>();
            spriteRender.sprite = backgroundSprite;
            spriteRender.drawMode = SpriteDrawMode.Tiled;
            spriteRender.size = new Vector2(track.width, track.height) * 1.5f;

        }

        private void DeleteOldTrack()
        {
            GameObject[] oldTrack = GameObject.FindGameObjectsWithTag("CurrentTrack");

            foreach (GameObject item in oldTrack)
            {
                DestroyImmediate(item);
            }
        }




    }
}
