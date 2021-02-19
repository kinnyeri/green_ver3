using System;
using System.Collections.Generic;

public class GeneticAlgorithm
{
    public List<DNA> Population { get; private set; }
    public int Generation { get; private set; }
    public double BestFitness;
    public float[,] BestGenes; //float

    public int Elitism;
    public float MutationRate;

    private List<DNA> newPopulation;
    private Random random;
    private double fitnessSum;
    private Func<float> getRandomGene;
    private Func<int, double> fitnessFunction;

    testTerraiin tt;

    public GeneticAlgorithm(int populationSize, Random random, Func<float> getRandomGene, Func<int, double> fitnessFunction,
        int elitism, testTerraiin tt, float mutationRate = 0.01f)
    {
        Generation = 1;
        Elitism = elitism;
        MutationRate = mutationRate;
        Population = new List<DNA>(populationSize);
        newPopulation = new List<DNA>(populationSize);
        this.random = random;
        this.getRandomGene = getRandomGene;
        this.fitnessFunction = fitnessFunction;
        this.tt = tt;
        BestGenes = new float[tt.RowSize,tt.RowSize];

        for (int i = 0; i < populationSize;i++)
        {
            if (!tt.finish)
            {
                
                Population.Add(new DNA(tt.RowSize, random, getRandomGene, fitnessFunction, tt, shouldInitGenes: true));
            }
            tt.gaDebug(i + " pop size");
        }
        tt.gaDebug("ga finish");
    }

    public void NewGeneration(int numNewDNA = 0, bool crossoverNewDNA = false)
    {
        int finalCount = Population.Count + numNewDNA;

        if (finalCount <= 0)
        {
            return;
        }

        if (Population.Count > 0)
        {
            CalculateFitness();
            Population.Sort(CompareDNA);
        }
        newPopulation.Clear();

        for (int i = 0; i < Population.Count; i++)
        {
            if (i < Elitism && i < Population.Count)
            {
                newPopulation.Add(Population[i]);
            }
            else if (i < Population.Count || crossoverNewDNA)
            {
                DNA parent1 = ChooseParent();
                DNA parent2 = ChooseParent();

                DNA child = parent1.Crossover(parent2);

                child.Mutate(MutationRate);

                newPopulation.Add(child);
            }
            else
            {
                newPopulation.Add(new DNA(tt.RowSize, random, getRandomGene, fitnessFunction, tt, shouldInitGenes: true));
            }
        }

        List<DNA> tmpList = Population;
        Population = newPopulation;
        newPopulation = tmpList;

        Generation++;
    }

    private int CompareDNA(DNA a, DNA b)
    {
        if (a.Fitness > b.Fitness)
        {
            return -1;
        }
        else if (a.Fitness < b.Fitness)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    private void CalculateFitness()
    {
        fitnessSum = 0;
        DNA best = Population[0];

        for (int i = 0; i < Population.Count; i++)
        {
            fitnessSum += Population[i].CalculateFitness(i);

            if (Population[i].Fitness > best.Fitness)
            {
                best = Population[i];
            }
        }

        BestFitness = best.Fitness;
        Array.Copy(best.Genes, 0, BestGenes, 0, BestGenes.Length);
        //best.Genes.CopyTo(BestGenes, 2);
    }

    private DNA ChooseParent()
    {
        double randomNumber = random.NextDouble() * fitnessSum;

        for (int i = 0; i < Population.Count; i++)
        {
            if (randomNumber < Population[i].Fitness)
            {
                return Population[i];
            }

            randomNumber -= Population[i].Fitness;
        }

        return null;
    }
}
