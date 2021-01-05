using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats
{
    public event EventHandler OnStatsChanged; //값이 바뀔 때만 UI 변경
    public static float STAT_MIN = 0;
    public static float STAT_MAX = 1;

    public enum Type
    {
        Distance,
        BF,
        LR,
        Hit,
    }
    private SingleStat DistanceStat;
    private SingleStat BFStat;
    private SingleStat LRStat;
    private SingleStat HitStat;

    public Stats(float DistanceStatAmount, float BFStatAmount, float LRStatAmount, float HitStatAmount)
    {
        DistanceStat = new SingleStat(DistanceStatAmount);
        BFStat = new SingleStat(BFStatAmount);
        LRStat = new SingleStat(LRStatAmount);
        HitStat = new SingleStat(HitStatAmount);
    }
    private SingleStat GetSingleStat(Type statType)
    {
        switch (statType)
        {
            default:
            case Type.Distance: return DistanceStat;
            case Type.BF: return BFStat;
            case Type.LR: return LRStat;
            case Type.Hit: return HitStat;
        }
    }
    public void SetStatAmount(Type statType, int statAmount)
    {
        GetSingleStat(statType).SetStatAmount(statAmount);

        if (OnStatsChanged != null) OnStatsChanged(this, EventArgs.Empty);
    }
    public float GetStatAmount(Type statType)
    {
        return GetSingleStat(statType).GetStatAmount();
    }
    public float GetStatAmountNormalized(Type statType)
    {
        return GetSingleStat(statType).GetStatAmountNormalized();
    }
    private class SingleStat
    {
        private float stat;

        public SingleStat(float statAmount)
        {
            SetStatAmount(statAmount);
        }
        public void SetStatAmount(float statAmount)
        {
            stat = Mathf.Clamp(statAmount, STAT_MIN, STAT_MAX); //최대/최소 값 범위 넘어가지 않게

        }
        public float GetStatAmount()
        {
            return stat;
        }
        public float GetStatAmountNormalized()
        {
            return (float)stat / STAT_MAX;
        }
    }
}