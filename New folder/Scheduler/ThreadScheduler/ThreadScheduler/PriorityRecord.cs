using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadScheduler
{
    class PriorityRecord
    {
        private Dictionary<int, int> PriorityMap;
        private int MaxTime;

        public PriorityRecord(int startPriority)
        {
            PriorityMap = new Dictionary<int, int>();
            PriorityMap.Add(0, startPriority);
            MaxTime = 0;
        }

        public void SetAt(int time, int priority)
        {
            if (MaxTime < time)
            {
                MaxTime = time;
            }
            if (PriorityMap.ContainsKey(time)) PriorityMap[time] = priority;
            else PriorityMap.Add(time, priority);
        }

        public void ClearFrom(int time)
        {
            for(int timeStep = time; timeStep < MaxTime; timeStep++)
            {
                if (PriorityMap.ContainsKey(timeStep))
                {
                    PriorityMap.Remove(timeStep);
                }
            }
        }

        public int GetAt(int time)
        {
            if (PriorityMap.ContainsKey(time)) return PriorityMap[time];
            else return 1;
        }

        public void Boost(int amount)
        {
            foreach (int timeStep in PriorityMap.Keys)
            {
                PriorityMap[timeStep]++;
            }
        }

    }
}
