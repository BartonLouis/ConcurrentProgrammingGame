using System;
using System.Collections.Generic;
using System.Linq;

namespace ThreadScheduler
{
    class ThreadScheduler
    {
        Dictionary<Character, PriorityRecord> PriorityRecord = new Dictionary<Character, PriorityRecord>();

        private readonly int NumCores;
        private readonly Core[] Cores;

        private System.Random Rnd = new Random();
        private readonly int MinThreadTime = 10;
        private readonly int MaxThreadTime = 20;
        private readonly int ForecastSize = 20;
        private readonly int YieldBoost = 1;             // This is the amount that friendly characters priority is boosted when a teammate yields
        private readonly int PassivePriorityBuild;       // The rate at which a characters priority builds at

        private bool ShouldReschedule = false;
        private int CurrentTime = 0;

        public ThreadScheduler(int numCores, int minThreadTime, int maxThreadTime, int forecastSize, int yieldBoost, int passivePriorityBuild, Character[] characters)
        {
            // Setup cores
            NumCores = numCores;
            Cores = new Core[numCores];
            for (int i = 0; i < NumCores; i++)
            {
                Cores[i] = new Core(this);
            }
            // Setup thread time limits
            CurrentTime = 0;
            MinThreadTime = minThreadTime;
            MaxThreadTime = maxThreadTime;
            ForecastSize = forecastSize;
            YieldBoost = yieldBoost;
            PassivePriorityBuild = passivePriorityBuild;
            // Setup priority records, all threads start at 1 priority
            foreach (Character character in characters)
            {
                PriorityRecord.Add(character, new PriorityRecord(1));
            }
            Schedule(CurrentTime, ForecastSize, true);
        }

        public override string ToString()
        {
            string toReturn = "";
            toReturn += "Thread Scheduler: ";
            toReturn += $"\n\t Current Time     : {CurrentTime}";
            toReturn += $"\n\t Forecast Size    : {ForecastSize}";
            toReturn += $"\n\t NumCores         : {NumCores}";
            toReturn += $"\n\t Cores: ";
            foreach (Core core in Cores)
            {
                toReturn += $"\n\t\t{core}";
            }

            return toReturn;
        }

        public void SetShouldReschedule(bool shouldReschedule)
        {
            // Called when a thread yields, signifies to the scheduler that it should reschedule
            ShouldReschedule = shouldReschedule;
        }

        public void SetShouldReschedule(bool shoulReschedule, int team)
        {
            ShouldReschedule = shoulReschedule;
            foreach (Character character in PriorityRecord.Keys)
            {
                if (character.Team == team)
                {
                    PriorityRecord[character].Boost(YieldBoost);
                }
            }
        }

        public void RemoveCharacter(Character character)
        {
            // Used when a character has died during battle
            if (PriorityRecord.ContainsKey(character))
            {
                PriorityRecord.Remove(character);
                SetShouldReschedule(true);
            }
        }

        public void Step()
        {
            ShouldReschedule = false;
            // Each core will take one step forward
            foreach(Core core in Cores)
            {
                core.Step();
            }
            // If one of the threads has caused a reschedule, the reschedule entirely, otherwise just schedule another step ahead
            if (ShouldReschedule) Schedule(CurrentTime, ForecastSize+1, true);
            else Schedule(CurrentTime + ForecastSize, 1, false);
            CurrentTime++;
        }

        
        public void Schedule(int from, int numSteps, bool clear)
        {
            // Step 1: Load up the starting priorities, setup MinTimeQueue as well
            Dictionary<Character, int> priorityMap = new Dictionary<Character, int>();
            foreach (Character character in PriorityRecord.Keys)
            {
                priorityMap.Add(character, Math.Max(PriorityRecord[character].GetAt(from), 1));
            }
            Dictionary<Character, int> timeStepMap = new Dictionary<Character, int>();
            // Step 2: If we're rescheduling, then clear each core from this point onward to make space for rescheduling
            // Otherwise, we can leave pre-loaded characters in as we are just adding onto the existing forecast
            if (clear)
            {
                foreach (Core core in Cores)
                {
                    core.ClearFrom(from);
                }
            }
            // Step 3: For reach time step, queue some characters onto each core
            for (int timeStep = from; timeStep <= from+numSteps; timeStep++)
            {
                // Save a record of the characters priority at this time step so that rescheduling from that point can occur accuratlely if needed.
                foreach (Character character in priorityMap.Keys)
                {
                    PriorityRecord[character].SetAt(timeStep, priorityMap[character]);
                }

                // Step 4: For each core, check if a character has already been queued at this time step
                int num = 0;
                foreach (Core core in Cores){
                    num++;
                    if (core.GetAt(timeStep) != null) continue;                             // Skip this core if it already has something queued

                    int totalPriority = priorityMap.Sum(x => (Math.Max(0, x.Value)));       // Count up total priority of all threads
                    if (totalPriority == 0) break;                                          // If no threads are available to queue at this time, give up on this time step
                    
                    // Step 5: Pick a character to queue
                    double random = Rnd.NextDouble();   // Random number (0-1)
                    double total = 0;                   // counting total
                    foreach (Character character in priorityMap.Keys) {
                        total += ((float)priorityMap[character]) / totalPriority;
                        if (total < random) continue;

                        // Step 6: Queue the character
                        int time = Rnd.Next(MinThreadTime, MaxThreadTime);  // choose how long to queue for
                        core.SetAt(timeStep, character, time);              // queue character on core
                        timeStepMap.Add(character, time);                   // prevent character for being requeued until <timeSteps> steps have passed
                        priorityMap[character] = 0;                         // set priority to 0
                        break;
                    }
                }

                // For all characters which have some priority greater than 0, increase it by 1 
                // Characters with priority 0 will stay at 0 as they are already queued and we need to prevent overlap
                List<Character> keys = new List<Character>(priorityMap.Keys);
                foreach(Character character in keys)
                {
                    if (priorityMap[character] > 0)
                    {
                        priorityMap[character] = priorityMap[character]+PassivePriorityBuild;
                    }
                }

                // For each character in the minTimeStep table, reduce its min time wait by 1
                // If it reaches 0, remove it from the table, and add some priority so it can be requeued
                keys = new List<Character>(timeStepMap.Keys);
                foreach (Character character in keys)
                {
                    timeStepMap[character]--;
                    if (timeStepMap[character] > 0) continue;
                    timeStepMap.Remove(character);
                    priorityMap[character] = 1;
                    
                }
            }
        }
    }
}
