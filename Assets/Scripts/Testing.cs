using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;

public class Testing : MonoBehaviour
{
    [SerializeField]
    private UI_StatsRadarChart uiStatsRadarChart;
    DataStorage ds;

    private void Awake()
    {
        ds = FindObjectOfType<DataStorage>();
    }
    private void Start()
    {
        Stats stats = new Stats(ds.ballDistace, ds.BF, ds.LR, ds.hitCount/5.0f);

        uiStatsRadarChart.SetStats(stats);
        Debug.Log(uiStatsRadarChart.stats.GetStatAmountNormalized(Stats.Type.Distance));
      
    }
}
