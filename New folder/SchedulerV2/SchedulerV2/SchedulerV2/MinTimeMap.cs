using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerV2
{
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
            int[] times = WaitMap.Keys.ToArray();
            foreach (int t in times)
            {
                if (t >= time) WaitMap.Remove(t);
            }
        }
    }
}
