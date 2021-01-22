using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class testTerraiin : MonoBehaviour
{
    Terrain testTerrain;
    static float sizeWeight = 1.5f;
    private System.Random random;

    public float[,] Genes;
    int RowSize = (int) (300 * sizeWeight);
    int ColSize = (int) (300 * sizeWeight);
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
        random = new System.Random();
        Genes = new float[RowSize, ColSize];
        GetRandomInt();

        maxNumHeight = 30 * (float)random.NextDouble() * sizeWeight;

        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
        Debug.Log("200,200 ::: "+terrain.terrainData.GetHeight(150,150));
        Debug.Log("100,100 ::: "+terrain.terrainData.GetHeight(100,100));
        //WriteTxt(Path.Combine(Application.streamingAssetsPath, "test.txt"));
        if (finish == false) finish = true;
    }

    // Update is called once per frame
    void Update()
    {
        
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
                                                                   //  center_difference *= 0.3f;
                                                                   // Genes[x, y] = Genes[x, y] * 2f - 1f;

            //Debug.Log(Genes[x, y]);
            //Debug.Log("center: " + x + ", " + y);
            //Debug.Log("range:" + rangeX + ", " + rangeY);

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
                    //Genes[tmpX, tmpY] = 0;

                }
            }
            }
            //int cnt = 0;
            //for (int i = 0; i < 450; i++)
            //{
            //    for (int j = 0; j < 450; j++)
            //    {
            //        if ((i < 50&&i>30) && ( j < 50 && j>30))
            //        {
            //            Genes[i, j] = 1f;
            //        }
            //        else if( i==100 && j == 100)
            //        {
            //            Genes[i, j] = 2f;
            //        }
            //        else
            //        {
            //            Genes[i, j] = 0.5f;
            //        }
            //    }
            //}
        }

    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = RowSize + 1;

        terrainData.size = new Vector3(RowSize, depth, ColSize); //width, depth,height
        terrainData.SetHeights(0, 0, Genes);
        return terrainData;
    }

    void WriteTxt(string filePath)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(Path.GetDirectoryName(filePath));

        if (!directoryInfo.Exists)
        {
            directoryInfo.Create();
        }

        FileStream fileStream
            = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);

        StreamWriter writer = new StreamWriter(fileStream, System.Text.Encoding.Unicode);

        string msg = "Genes\n";
        for(int i = 0; i< 450; i++) {
            for (int j = 0; j < 450; j++) {
                msg += Genes[i, j] + " ";
            }
            msg += "\n";
        }
        writer.WriteLine(msg);
        writer.Close();
    }
}
