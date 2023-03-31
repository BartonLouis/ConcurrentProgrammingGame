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
        int maxTime = 0;
        foreach (int time in Queue.Keys)
        {
            if (time > maxTime) maxTime = time;
        }
        for (int i = startTime; i < startTime + timeToQueue; i++)
        {
            Queue[i] = character;
        }
        if (startTime > maxTime + 1) {
            BattleModel.AddVisualBlock(Index, null, startTime - (maxTime+1));
        }
        BattleModel.AddVisualBlock(Index, character, timeToQueue);
    }

    public void ClearFrom(int time)
    {
        List<int> temp = new List<int>();
        foreach (int t in Queue.Keys)
        {
            temp.Add(t);
        }
        foreach (int t in temp)
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
            if (Queue.ContainsKey(time)) toReturn += ", " + time +  ":"+ Queue[time].ToString(); 
            else toReturn += ", " + time + ":##";
        }
        return toReturn;
    }

    public List<KeyValuePair<Character, int>> GetRepresentation()
    {
        List<KeyValuePair<Character, int>> toReturn = new List<KeyValuePair<Character, int>>();
        // Step 1: Get the max time
        int maxTime = 0;
        foreach (int time in Queue.Keys)
        {
            if (time > maxTime) maxTime = time;
        }

        KeyValuePair<Character, int> current = new KeyValuePair<Character, int>();
        for (int time = 0; time < maxTime; time++)
        {
            // If current is null then start a new current
            if (current.Equals(new KeyValuePair<Character, int>()))
            {
                current = new KeyValuePair<Character, int>(GetAt(time), 1);
            } 
            else
            {
                // Else, check if character matches current time step, if so add 1
                if (GetAt(time) == current.Key)
                {
                    current = new KeyValuePair<Character, int>(current.Key, current.Value + 1);
                } else
                {
                    // Else, add current onto toReturn, reset current to new character, 0
                    toReturn.Add(current);
                    current = new KeyValuePair<Character, int>(GetAt(time), 1);
                }
            }

        } 
        return toReturn;
    }

}