using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core
{
    private Dictionary<int, Character> Queue;
    private BattleModel BattleModel;
    private int Index;

    public Core(int index)
    {
        Queue = new Dictionary<int, Character>();
        BattleModel = BattleModel.instance;
        Index = index;
    }

    public void Step(int time)
    {
        if (Queue.ContainsKey(time))
        {
            Queue[time].Step();
        }
    }

    public void QueueFor(int startTime, Character character, int timeToQueue)
    {
        for (int i = startTime; i < startTime + timeToQueue; i++)
        {
            Queue[i] = character;
        }
        int maxTime = 0;
        foreach (int time in Queue.Keys)
        {
            if (time > maxTime) maxTime = time;
        }
        if (startTime > maxTime) BattleModel.AddVisualBlock(Index, null, startTime - maxTime);
        BattleModel.AddVisualBlock(Index, character, timeToQueue);
    }

    public void ClearFrom(int time)
    {
        Dictionary<int, Character>.KeyCollection times = Queue.Keys;
        foreach (int t in times)
        {
            if (t >= time) Queue.Remove(t);
        }
    }

    public Character GetAt(int time)
    {
        if (Queue.ContainsKey(time))
            return Queue[time];
        return null;
    }

    public Dictionary<Character, int> GetAnalysis(Character[] characters)
    {
        Dictionary<Character, int> toReturn = new Dictionary<Character, int>();
        foreach (Character character in characters)
        {
            toReturn.Add(character, 0);
        }
        foreach (int timeStep in Queue.Keys)
        {
            toReturn[Queue[timeStep]]++;
        }
        return toReturn;
    }

    public override string ToString()
    {
        string toReturn = "";
        int maxTime = 0;
        foreach (int time in Queue.Keys)
        {
            if (time > maxTime) maxTime = time;
        }
        for (int time = 0; time <= maxTime; time++)
        {
            if (Queue.ContainsKey(time)) toReturn += ", " + Queue[time].ToString(); 
            else toReturn += ", ##";
        }
        return toReturn;
    }

}