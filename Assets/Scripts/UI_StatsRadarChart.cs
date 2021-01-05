using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_StatsRadarChart : MonoBehaviour
{
    [SerializeField]
    private Material radarMaterial;

    public Stats stats;
    private CanvasRenderer raderMeshCanvasRenderer;

    private void Awake()
    {
        raderMeshCanvasRenderer = transform.Find("radarMesh").GetComponent<CanvasRenderer>();
    }

    public void SetStats(Stats stats)
    {
        this.stats = stats;
        stats.OnStatsChanged += Stats_OnStatsChanged;
        UpdateStatsVisual();
    }

    private void Stats_OnStatsChanged(object sender, System.EventArgs e)
    {
        UpdateStatsVisual();
    }

    private void UpdateStatsVisual()
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[5];
        Vector2[] uv = new Vector2[5];
        int[] triangles = new int[3 * 4];

        float angleIncrement = 360f / 4;
        float radarChartSize = 99f-6.5f;

        Vector3 DistanceVertex = Quaternion.Euler(0, 0, -angleIncrement * 0) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(Stats.Type.Distance);
        int DistanceVertexIndex = 1;
        Vector3 BFVertex = Quaternion.Euler(0, 0, -angleIncrement * 1) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(Stats.Type.BF);
        int BFVertexIndex = 2;
        Vector3 LRVertex = Quaternion.Euler(0, 0, -angleIncrement * 2) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(Stats.Type.LR);
        int LRVertexIndex = 3;
        Vector3 HitVertex = Quaternion.Euler(0, 0, -angleIncrement * 3) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(Stats.Type.Hit);
        int HitVertexIndex = 4;


        vertices[0] = Vector3.zero;
        vertices[DistanceVertexIndex] = DistanceVertex;
        vertices[BFVertexIndex] = BFVertex;
        vertices[LRVertexIndex] = LRVertex;
        vertices[HitVertexIndex] = HitVertex;


        triangles[0] = 0;
        triangles[1] = DistanceVertexIndex;
        triangles[2] = BFVertexIndex;

        triangles[3] = 0;
        triangles[4] = BFVertexIndex;
        triangles[5] = LRVertexIndex;

        triangles[6] = 0;
        triangles[7] = LRVertexIndex;
        triangles[8] = HitVertexIndex;

        triangles[9] = 0;
        triangles[10] = HitVertexIndex;
        triangles[11] = DistanceVertexIndex;



        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        raderMeshCanvasRenderer.SetMesh(mesh);
        raderMeshCanvasRenderer.SetMaterial(radarMaterial, null);
    }
}