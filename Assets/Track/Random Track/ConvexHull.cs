﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvexHull : MonoBehaviour
{
    //public Transform[] points;
    //[Range(0.05f, 1.5f)]
    //public float size;
    //public bool drawIt;
    //public List<Transform> result;

    //void Start()
    //{

    //	result = new List<Transform>();
    //       GenerateConvexHull();
    //   }

    //   private void Update()
    //   {
    //       GenerateConvexHull();
    //   }


    //   private void GenerateConvexHull()
    //{
    //	result = new List<Transform>();
    //	int leftMostIndex = 0;
    //	for (int i = 1; i < points.Length; i++)
    //	{
    //		if (points[leftMostIndex].position.x > points[i].position.x)
    //			leftMostIndex = i;
    //	}
    //	result.Add(points[leftMostIndex]);
    //	List<Transform> collinearPoints = new List<Transform>();
    //	Transform current = points[leftMostIndex];
    //	while (true)
    //	{
    //		Transform nextTarget = points[0];
    //		for (int i = 1; i < points.Length; i++)
    //		{
    //			if (points[i] == current)
    //				continue;
    //			float x1, x2, y1, y2;
    //			x1 = current.position.x - nextTarget.position.x;
    //			x2 = current.position.x - points[i].position.x;

    //			y1 = current.position.y - nextTarget.position.y;
    //			y2 = current.position.y - points[i].position.y;

    //			float val = (y2 * x1) - (y1 * x2);
    //			if (val > 0)
    //			{
    //				nextTarget = points[i];
    //				collinearPoints = new List<Transform>();
    //			}
    //			else if (val == 0)
    //			{
    //				if (Vector2.Distance(current.position, nextTarget.position) < Vector2.Distance(current.position, points[i].position))
    //				{
    //					collinearPoints.Add(nextTarget);
    //					nextTarget = points[i];
    //				}
    //				else
    //					collinearPoints.Add(points[i]);
    //			}
    //		}

    //		foreach (Transform t in collinearPoints)
    //			result.Add(t);
    //		if (nextTarget == points[leftMostIndex])
    //			break;
    //		result.Add(nextTarget);
    //		current = nextTarget;
    //	}
    //}


    //void OnDrawGizmos()
    //{
    //    if (drawIt)
    //    {
    //        if (result != null)
    //        {
    //            List<Transform> outter = new List<Transform>();
    //            foreach (var item in result)
    //                outter.Add(item);
    //            for (int i = 0; i < outter.Count - 1; i++)
    //                Gizmos.DrawLine(outter[i].position, outter[i + 1].position);
    //            Gizmos.DrawLine(outter[0].position, outter[outter.Count - 1].position);
    //        }

    //        for (int i = 0; i < points.Length; i++)
    //        {
    //            Gizmos.color = Color.cyan;
    //            if (result != null)
    //            {
    //                if (result.Contains(points[i]))
    //                    Gizmos.color = Color.yellow;
    //            }
    //            Gizmos.DrawSphere(points[i].position, size);
    //        }
    //    }
    //}




}


