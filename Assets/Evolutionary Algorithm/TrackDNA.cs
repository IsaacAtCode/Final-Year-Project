using System;
using System.Collections;
using System.Collections.Generic;
using Random = System.Random;
using UnityEngine;
using IsaacFagg.Track;


namespace IsaacFagg.Genetics
{
    public class TrackDNA : MonoBehaviour
    {
        public TrackData Genes;
        //{ get; private set; }
        public float Fitness { get; private set; }

        private Random random;
        private Func<Vector2> getRandomGene;
        private Func<int, float> fitnessFunction;

        public TrackDNA()
        {

        }

        public float CalculateFitness(int index)
        {
            Fitness = fitnessFunction(index);
            return Fitness;
        }

        public void Mutate(float mutationRate)
        {
            for (int i = 0; i < Genes.points.Count; i++)
            {
                if (random.NextDouble() < mutationRate)
                {
                    Genes.points[i] = getRandomGene();
                }
            }
        }

    }
}
