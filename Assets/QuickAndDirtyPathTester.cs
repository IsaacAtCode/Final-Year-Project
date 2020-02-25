using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IsaacFagg.Paths;

public class QuickAndDirtyPathTester : MonoBehaviour
{
    [Header("Original Points")]
    public List<Vector2> points;
    public PathCreator originPC;

    [Header("Count")]
    public int count; //Desired Number
    private int oldCount;
    [Range(0, 400)]
    float spacing = 300f;
    private bool hasSpacingBeenFound = false;

    [Header("Scaled Points")]
    public PathCreator scaledPC;
    public List<Vector2> scaledPoints;

    private void Start()
    {
        originPC.path = new Path(points);
    }


    private void Update()
    {
        if (oldCount != count)
        {
            hasSpacingBeenFound = false;
        }

        if (!hasSpacingBeenFound)
        {
            FindDesiredCount(count, points);
        }


        if (hasSpacingBeenFound)
        {
            scaledPoints = originPC.path.CalculateEvenlySpacedPoints(spacing);

            scaledPC.path = new Path(scaledPoints);

            oldCount = count;
        }

    }

    public void FindDesiredCount(int desiredCount, List<Vector2> points)
    {
        Path path = new Path(points);
        spacing = path.EstimatedLength() / 5;

        List<Vector2> testingScaledPoints = path.CalculateEvenlySpacedPoints(spacing);


        for (float t = spacing; t >= desiredCount; t -= 1f)
        {
            testingScaledPoints = path.CalculateEvenlySpacedPoints(t);

            if (testingScaledPoints.Count == desiredCount)
            {
                Debug.Log("Got here");
                spacing = t;
                hasSpacingBeenFound = true;
                break;
            }
        }
    }
}
