using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class ThreadScheduler
{
    private Dictionary<Character, PriorityRecord> PriorityRecords = new Dictionary<Character, PriorityRecord>();
    private Dictionary<Character, MinTimeMap> MinWaitTimeRecords = new Dictionary<Character, MinTimeMap>();

    private System.Random Rnd = new System.Random();
    private int CurrentTime;

    private Core[] Cores;
    private readonly List<Character> Characters;
    private int NumCores;

    private readonly int MinQueueTime = 10;
    private readonly int MaxQueueTime = 20;
    private readonly int ForecastSize = 20;
    private readonly int YieldBoost = 1;
    private readonly int PassivePriorityBuild = 1;
    private readonly int MinTimeBetweenTurns = 0;

    private bool ShouldReschedule = false;

    public ThreadScheduler(Character[] characters, Core[] cores, int minQueueTime, int maxQueueTime, int forecastSize, int yieldBoost, int passivePriorityBuild, int minTimeBetweenTurns)
    {
        Characters = characters.ToList();
        Cores = cores;
        NumCores = Cores.Length;
        MinQueueTime = minQueueTime;
        MaxQueueTime = maxQueueTime;
        ForecastSize = forecastSize;
        YieldBoost = yieldBoost;
        PassivePriorityBuild = passivePriorityBuild;
        MinTimeBetweenTurns = minTimeBetweenTurns;

        foreach (Character character in Characters)
        {
            PriorityRecords.Add(character, new PriorityRecord());
            MinWaitTimeRecords.Add(character, new MinTimeMap());
        }
        
        Schedule(0, ForecastSize, true);
    }

    public Core[] GetCores()
    {
        return Cores;
    }

    public void SetShouldReschedule()
    {
        ShouldReschedule = true;
    }

    public void SetShouldReschedule(TeamCenter team)
    {
        ShouldReschedule = true;
        foreach (Character character in Characters)
        {
            if (character != null && character.Team.TeamNum == team.TeamNum)
                PriorityRecords[character].Boost(YieldBoost);
        }
    }

    public void RemoveCharacter(Character character)
    {
        if (character == null) return;
        if (!Characters.Contains(character)) return;
        PriorityRecords.Remove(character);
        MinWaitTimeRecords.Remove(character);
        Characters.Remove(character);
    }

    public void Step(int timeStep)
    {
        if (ShouldReschedule)
        {
            Schedule(timeStep-1, ForecastSize+1, true);
            Debug.Log("Rescheduling");
        }
        else Schedule(timeStep + ForecastSize - 1, 1, false);
        ShouldReschedule = false;
    }

    private void Schedule(int startTime, int numSteps, bool clear)
    {
        CurrentTime = startTime;
        // If we are doing a re-forecast then we need to clear up some variables
        if (clear)
            foreach (Core core in Cores)
            {
                // Clear all cores
                core.ClearFrom(startTime);
                // Clear priority and min wait map from next step onward
                // so we can use the current step in re-forecasting
                // and re-forecast the next steps
                foreach (Character c in Characters)
                {
                    PriorityRecords[c].ClearFrom(startTime );
                    MinWaitTimeRecords[c].ClearFrom(startTime);
                }
            }
        

        for (int timeStep = startTime; timeStep < startTime + numSteps; timeStep++)
        {
            // Step 1: For each core, check if a character has already been queued up
            foreach (Core core in Cores)
            {
                // Skip this core if already occupied
                if (core.GetAt(timeStep) != null) continue;

                // Count up total priority and skip all cores if no available threads to queue
                int totalPriority = PriorityRecords.Sum(x => x.Value.GetAt(timeStep) * x.Value.GetAt(timeStep));
                if (totalPriority == 0) break;

                // Step 2: Pick a character to queue
                double random = Rnd.NextDouble();
                double total = 0;
                foreach (Character character in PriorityRecords.Keys)
                {
                    total += Math.Pow(PriorityRecords[character].GetAt(timeStep), 2) / totalPriority;
                    if (total < random || PriorityRecords[character].GetAt(timeStep) == 0) continue;

                    // Step 3: Queue that character
                    int time = Rnd.Next(MinQueueTime, MaxQueueTime);
                    core.QueueFor(timeStep, character, time);
                    MinWaitTimeRecords[character].SetAt(timeStep, time+MinTimeBetweenTurns);
                    PriorityRecords[character].SetAt(timeStep, 0);
                    break;
                }
            }

            // Step 4: Update Priority Map and Min Wait Time Map
            foreach (Character character in Characters)
            {
                // Increment Priority for next step if priority > 0, else keep at 0
                int p = PriorityRecords[character].GetAt(timeStep);
                if (p > 0) { PriorityRecords[character].SetAt(timeStep + 1, p + PassivePriorityBuild);}
                else PriorityRecords[character].SetAt(timeStep + 1, 0);

                // Decrement min wait time if it's greater than 0
                int w = MinWaitTimeRecords[character].GetAt(timeStep);
                if (w > 0) MinWaitTimeRecords[character].SetAt(timeStep + 1, w - 1);
                // If min wait time == 0, and the character doesn't have any priority, then give it some priority
                else if (PriorityRecords[character].GetAt(timeStep) == 0){
                    PriorityRecords[character].SetAt(timeStep + 1, 1);
                    MinWaitTimeRecords[character].SetAt(timeStep + 1, 0);
                } else
                {
                    MinWaitTimeRecords[character].SetAt(timeStep + 1, 0);
                }
            }
        }
    }

    public override string ToString()
    {
        string toReturn = "";
        toReturn += "Thread Scheduler: ";
        toReturn += $"\n\t Forecast Size    : {ForecastSize}";
        toReturn += $"\n\t NumCores         : {NumCores}";
        toReturn += $"\n\t Cores: ";
        foreach (Core core in Cores)
        {
            toReturn += $"\n\t\t{core}";
        }
        toReturn += "\n\nPriorities:";
        foreach (Character c in PriorityRecords.Keys)
        {
            toReturn += "\n" + c.ToString() + ": \t\t";
            for (int i = 0; i < CurrentTime; i++)
            {
                toReturn += ", " + i + ":" + PriorityRecords[c].GetAt(i);
            }
        }

        toReturn += "\nMinTimeRecord:";
        foreach (Character c in MinWaitTimeRecords.Keys)
        {
            toReturn += "\n" + c.ToString() + ": \t\t";
            for (int i = 0; i < CurrentTime; i++)
            {
                toReturn += ", " + i + ":" + MinWaitTimeRecords[c].GetAt(i);
            }
        }
        return toReturn;
    }


    public List<List<KeyValuePair<Character, int>>> GetVisualRepresentation()
    {
        List<List<KeyValuePair<Character, int>>> toReturn = new List<List<KeyValuePair<Character, int>>>();
        foreach(Core core in Cores)
        {
            toReturn.Add(core.GetRepresentation());
        }

        return toReturn;
    }
}