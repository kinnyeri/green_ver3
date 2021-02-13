using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class testTerraiin : MonoBehaviour
{
    Terrain testTerrain;
    static float sizeWeight = 1.5f;
    private System.Random random = new System.Random();

    public float[,] Genes;
    public int RowSize = (int) (300 * sizeWeight);
    public int ColSize = (int) (300 * sizeWeight);
    int depth = 5;

    public int verticesNum;

    float maxNumHeight = 100 * sizeWeight;
    float height;
    float rangeX = 30f * sizeWeight, rangeY = 30f* sizeWeight; // 랜덤

    float center_difference;

    public bool finish = false;
    // Start is called before the first frame update
    void Start()
    {
        //random = new System.Random();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void startMakingGene()
    {
        Genes = new float[RowSize, ColSize];
        GetRandomInt();

        maxNumHeight = 30 * (float)random.NextDouble() * sizeWeight;

        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
        Debug.Log("200,200 ::: " + terrain.terrainData.GetHeight(150, 150));
        Debug.Log("100,100 ::: " + terrain.terrainData.GetHeight(100, 100));
        //WriteTxt(Path.Combine(Application.streamingAssetsPath, "test.txt"));
        if (finish == false) finish = true;
    }

    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = RowSize + 1;

        terrainData.size = new Vector3(RowSize, depth, ColSize); //width, depth,height
        terrainData.SetHeights(0, 0, Genes);
        return terrainData;
    }

    private void GetRandomInt()
    {
        int x = 0, y = 0;

        // 0.5로 초기화 전체
        for (int i = 0; i < RowSize; i++)
        {
            for (int j = 0; j < ColSize; j++)
            {
                Genes[i, j] = 0.5f;
            }
        }


       //maxNumHeight = 1;
        double newHeight;
        for (int v = 0; v < maxNumHeight; v++)
        {
            int limit = (int)(50 * sizeWeight);
            x = random.Next(limit, RowSize - limit);  //x
            y = random.Next(limit, ColSize - limit); //y

            rangeX = random.Next(30, (int)(100 * sizeWeight));
            rangeY = random.Next(30, (int)(100 * sizeWeight));

            center_difference = (float)random.NextDouble() - 0.5f; //랜덤``

            for (float i = -rangeX / 2; i <= rangeX / 2; i++)
            {
                for (float j = -rangeY / 2; j <= rangeY / 2; j++)
                {
                    int tmpX = Mathf.FloorToInt(x + i);
                    int tmpY = Mathf.FloorToInt(y + j);
                    if ((tmpX >= RowSize || tmpX < 0) || (tmpY >= ColSize || tmpY < 0)) break;

                    newHeight = center_difference * Mathf.Clamp01(1 - Mathf.Sqrt(Mathf.Pow((x - tmpX) / rangeX * 2, 2) + Mathf.Pow((y - tmpY) / rangeY * 2, 2)));

                    Genes[tmpX, tmpY] += ((float)newHeight);
                    Genes[tmpX, tmpY] = Mathf.Clamp01(Genes[tmpX, tmpY]);
                }
            }
            }
        }
    public float getRandomInt()
    {
        return Random.Range(1f, 10f);
    }

    //public float getRandomInt(int x,int y)
    //{
    //    //보간 하면 좋겠다 옆에꺼랑 비교해서

    //    int tmpX = Mathf.FloorToInt(x + i);
    //    int tmpY = Mathf.FloorToInt(y + j);
    //    //if ((tmpX >= RowSize || tmpX < 0) || (tmpY >= ColSize || tmpY < 0)) break;

    //    double newHeight = center_difference * Mathf.Clamp01(1 - Mathf.Sqrt(Mathf.Pow((x - tmpX) / rangeX * 2, 2) + Mathf.Pow((y - tmpY) / rangeY * 2, 2)));
    //    return (float)newHeight;
    //}
}
