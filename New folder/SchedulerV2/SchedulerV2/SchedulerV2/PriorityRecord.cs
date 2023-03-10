using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerV2
{
    public class PriorityRecord
    {

        private Dictionary<int, int> PriorityMap;

        public PriorityRecord() {
            PriorityMap = new Dictionary<int, int>
            {
                [0] = 1
            };
        }

        public void SetAt(int time, int priority)
        {
            PriorityMap[time] = priority;
        }

        public int GetAt(int time)
        {
            if (PriorityMap.ContainsKey(time)) return PriorityMap[time];
            return 0;
        }

        public void ClearFrom(int time)
        {
            int[] times = PriorityMap.Keys.ToArray();
            foreach(int t in times)
            {
                if (t >= time) PriorityMap.Remove(t);
            }
        }

        public void Boost(int amount)
        {
            foreach(int timeStep in PriorityMap.Keys)
            {
                PriorityMap[timeStep] += amount;
            }
        }
    }
}
