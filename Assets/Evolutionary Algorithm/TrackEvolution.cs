using System;
using System.Collections;
using System.Collections.Generic;
using Random = System.Random;
using UnityEngine;

using IsaacFagg.Track;

namespace IsaacFagg.Genetics
{
    public class TrackEvolution
    {
        public List<TrackDNA> Population { get; private set; }
        public int Generation { get; private set; }
        public float BestFitness { get; private set; }
        public TrackData BestGenes { get; private set; }

        public int Elitism;
        public float MutationRate;

        private List<TrackDNA> newPopulation;
        private Random random;
        private float fitnessSum;
        private int dnaSize;
        private Func<Vector2> getRandomGene;
        private Func<int, float> fitnessFunction;

        ///<summary> Evolves a track based on a previous dataset of tracks
        ///<param name = "populationSize"> How many tracks should it make?
        ///<param name="dnaSize"> 
        ///</param>
        public TrackEvolution(int populationSize, int dnaSize, Random random, Func<Vector2> getRandomGene, Func<int, float> fitnessFunction, int elitism, float mutationRate = 0.01f)
        {
            Generation = 1;
            Elitism = elitism;
            MutationRate = mutationRate;
            Population = new List<TrackDNA>(populationSize);
            newPopulation = new List<TrackDNA>(populationSize);

            this.random = random;
            this.dnaSize = dnaSize;
            this.getRandomGene = getRandomGene;
            this.fitnessFunction = fitnessFunction;

            //BestGenes = new TrackDNA();

            for (int i = 0; i < populationSize; i++)
            {
                Population.Add(new TrackDNA(dnaSize, random, getRandomGene, fitnessFunction, shouldInitGenes: true));
            }
        }
    }
}
