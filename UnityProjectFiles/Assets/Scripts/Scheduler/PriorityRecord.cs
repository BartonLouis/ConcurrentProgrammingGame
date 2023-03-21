using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityRecord
{

    private Dictionary<int, int> PriorityMap;

    public PriorityRecord()
    {
        PriorityMap = new Dictionary<int, int>
        {
            [0] = 1,
            [1] = 1
        };
    }

    public void SetAt(int time, int priority)
    {
        PriorityMap[time] = priority;
    }

    public int GetAt(int time)
    {
        if (PriorityMap.ContainsKey(time)) return PriorityMap[time];
        return 1;
    }

    public void ClearFrom(int time)
    {
        List<int> temp = new List<int>();
        foreach(int t in PriorityMap.Keys)
        {
            temp.Add(t);
        }
        foreach (int t in temp)
        {
            if (t >= time) PriorityMap.Remove(t);
        }
    }

    public void Boost(int amount)
    {
        Debug.Log(amount);
        List<int> keys = new List<int>();
        foreach(int timeStep in PriorityMap.Keys)
        {
            // if (PriorityMap[timeStep] > 0)
            keys.Add(timeStep);
        }
        foreach (int timeStep in keys)
        {
            PriorityMap[timeStep] += amount;
        }
    }
}