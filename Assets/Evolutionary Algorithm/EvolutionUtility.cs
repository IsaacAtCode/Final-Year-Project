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

    }
}
