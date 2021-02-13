using System;

public class DNA
{
    public float[,] Genes { get; private set; }
    public double Fitness { get; private set; }

    private Random random;
    private Func<float> getRandomGene;
    private Func<int, double> fitnessFunction;
    testTerraiin tt;

    //public testTerraiin tt;
    public DNA(int size, Random random, Func<float> getRandomGene, Func<int, double> fitnessFunction, testTerraiin tt, bool shouldInitGenes = true )
    {
        Genes = new float[size, size];
        this.random = random;
        this.getRandomGene = getRandomGene;
        this.fitnessFunction = fitnessFunction;
        this.tt = tt;

        if (shouldInitGenes)
        {
            tt.startMakingGene();
            Genes = tt.Genes;
        }
    }

    public double CalculateFitness(int index)
    {
        Fitness = fitnessFunction(index);
        return Fitness;
    }

    public DNA Crossover(DNA otherParent)
    {
        DNA child = new DNA(Genes.Length, random, getRandomGene, fitnessFunction, tt,shouldInitGenes: false);

        for (int i = 0; i < Genes.Length; i++)
        {
            for(int j = 0; j < Genes.Length; j++)
            {
                child.Genes[i,j] = random.NextDouble() < 0.5 ? Genes[i,j] : otherParent.Genes[i,j];
            }
        }

        return child;
    }

    public void Mutate(float mutationRate)
    {
        for (int i = 0; i < Genes.Length; i++)
        {
            for(int j = 0; j < Genes.Length; j++)
            {
                if (random.NextDouble() < mutationRate)
                    {
                            Genes[i, j] = getRandomGene();
                    }
            }
        }
    }
}