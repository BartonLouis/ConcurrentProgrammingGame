using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinTimeMap
{

    private Dictionary<int, int> WaitMap;

    public MinTimeMap()
    {
        WaitMap = new Dictionary<int, int>
        {
            [0] = 0
        };
    }

    public void SetAt(int time, int priority)
    {
        WaitMap[time] = priority;
    }

    public int GetAt(int time)
    {
        if (WaitMap.ContainsKey(time)) return WaitMap[time];
        return 1;
    }

    public void ClearFrom(int time)
    {
        List<int> temp = new List<int>();
        foreach (int t in WaitMap.Keys)
        {
            temp.Add(t);
        }
        foreach (int t in temp)
        {
            if (t >= time) WaitMap.Remove(t);
        }
    }
}