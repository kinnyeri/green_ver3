using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class makeLevel : MonoBehaviour
{
    float target = 0.85f;
    public int populationSize = 5;
    float mutationRate = 0.01f;
    int elitism = 5;

    public GeneticAlgorithm geneticAlgorithm;

    void Start()
    {
        geneticAlgorithm = new GeneticAlgorithm(populationSize, new System.Random(), Probability, elitism, mutationRate);
    }

    void Update()
    {
        
    }

    double Probability(int index)
    {
        return 0;
    }
}
