using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerV2
{

    public class Core
    {
        private Dictionary<int, Character> Queue;

        public Core() { 
            Queue = new Dictionary<int, Character>();
        }

        public void Step(int time)
        {
            if (Queue.ContainsKey(time))
            {
                Queue[time].Step();
            }
        }

        public void QueueFor(int startTime, Character character, int timeToQueue) {
            for (int i = startTime; i < startTime + timeToQueue; i++)
            {
                Queue[i] = character;
            }
        }

        public void ClearFrom(int time)
        {
            int[] times = Queue.Keys.ToArray();
            foreach(int t in times)
            {
                if (t >= time) Queue.Remove(t);
            }
        }

        public Character? GetAt(int time)
        {
            if (Queue.ContainsKey(time))
                return Queue[time];
            return null;
        }

        public Dictionary<Character, int> GetAnalysis(Character[] characters)
        {
            Dictionary<Character, int> toReturn = new();
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

    }
}
