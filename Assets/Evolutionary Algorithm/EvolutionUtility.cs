using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacFagg.Utility
{
    public static class EvolutionUtility
    {
        public static float GetFloatWithMutation(float minimum, float maximum, float maxRange, float mutationRate)
        {
            float min = Mathf.Min(minimum, maximum);
            float max = Mathf.Max(minimum, maximum);

            float randomNumber = Random.Range(min, max);
            float randomMutationStart = Random.Range(min, max);
            float randomMutationEnd = randomMutationStart + mutationRate;

            if (randomNumber > randomMutationStart && randomNumber < randomMutationEnd )
            {
                float newRandom = Random.Range(min * (1 - mutationRate), max * (1 + mutationRate));


                if (newRandom > maxRange)
                {
                    newRandom %= maxRange;

                    return newRandom;
                }
                else
                {
                    return newRandom;
                }
            }
            else
            {
                return randomNumber;
            }
        }
        public static float GetAngleToNextPoint(Vector2 initialPoint, Vector2 nextPoint)
        {
            float distToInitial = Vector2.Distance(Vector2.zero, initialPoint);
            float distBetween = Vector2.Distance(initialPoint, nextPoint);
            float distToNext = Vector2.Distance(Vector2.zero, nextPoint);

            float angle = Mathf.Acos((Mathf.Pow(distToInitial, 2) + Mathf.Pow(distBetween, 2) - Mathf.Pow(distToNext, 2)) / (2 * distToInitial * distBetween)) * Mathf.Rad2Deg;

            //float angleBetween = Mathf.Acos((Mathf.Pow(distToInitial,2) + Mathf.Pow(distToNext, 2) - Mathf.Pow(distBetween, 2)) / (2 * distToInitial * distToNext)) * Mathf.Rad2Deg;
            //float otherAngle = Mathf.Acos((Mathf.Pow(distToNext, 2) + Mathf.Pow(distBetween, 2) - Mathf.Pow(distToInitial, 2)) / (2 * distToNext * distBetween)) * Mathf.Rad2Deg;
            //float total = angleBetween + angle + otherAngle;


            //Debug.Log("DistInitial: " + distToInitial + " DistBetn: " + distBetween + " DistNext: " + distToNext);
            //Debug.Log("Angle: " + angle + " Other:  " + otherAngle + "  Opposite:  " + angleBetween);
            //Debug.Log("Total: " + total);

            return angle;
        }
    }




}
