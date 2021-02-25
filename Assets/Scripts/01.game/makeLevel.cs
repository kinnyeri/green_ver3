using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class makeLevel : MonoBehaviour
{
    float target = 0.85f;
    public int populationSize = 5;
    float mutationRate = 0.01f;
    int elitism = 5;

    public GeneticGreenMode ggm;
    public testTerraiin tt;
    public GeneticAlgorithm ga;

    void Start()
    {
        ga = new GeneticAlgorithm(populationSize, new System.Random(), tt.getRandomInt, Phi, elitism, tt,ggm, mutationRate); // i should change this into func<float>
        Debug.Log("ga start");
    }

    // Update is called once per frame
    void Update()
    {
        if (ggm.finish)
        {
            if (ga.currPopulation() >= populationSize)
            {
                Debug.Log("new generation");
                ga.NewGeneration();
                if (ga.BestFitness >= 0.95f)
                {
                    this.enabled = false;
                    Debug.Log("fitness ok!");
                }
                
            }
            else
            {
                ggm.finish = false;
                Debug.Log("green restart" + ggm.finish);
            }

        }
        
    }
    double Phi(int index)
    {
        if (ggm.probList[index] != null)
        {


            float x = ggm.probList[index];
            int n = 10;

            float sum = 0;
            float x2 = x * x;
            float nom = x;
            float denom = 1;
            float c = 1;
            for (int i = 0; i < n; i++)
            {
                sum += nom / denom;
                c += 2;
                nom *= x2;
                denom *= c;
            }
            return 0.5 + sum * Mathf.Exp(-x2 * 0.5f) / Mathf.Sqrt(2 * Mathf.PI);
        }
        return 0;
    }
}
