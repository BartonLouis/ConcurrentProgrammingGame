using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadScheduler
{
    class Core
    {
        private ThreadScheduler ThreadScheduler;
        private Dictionary<int, Character> Queue;
        private int MaxTime;

        public Core(ThreadScheduler threadScheduler)
        {
            Queue = new Dictionary<int, Character>();
            MaxTime = 0;
            ThreadScheduler = threadScheduler;
        }

        public void Step()
        {

        }

        public void Clear()
        {
            Queue.Clear();
            MaxTime = 0;
        }

        public void ClearFrom(int time)
        {
            for (int i = time; i <= MaxTime; i++)
            {
                Queue.Remove(i);
            }
            MaxTime = time;
        }

        public void SetAt(int timeStep, Character character, int steps)
        {
            if (timeStep + steps > MaxTime)
            {
                MaxTime = timeStep + steps;
            }
            for (int i = timeStep; i < timeStep + steps; i++)
            {
                if (Queue.ContainsKey(i))
                {
                    Queue[i] = character;
                } else
                {
                    Queue.Add(i, character);
                }
            }
        }

        public Character GetAt(int timeStep)
        {
            if (Queue.ContainsKey(timeStep))
            {
                return Queue[timeStep];
            }
            return null;
        }

        public override string ToString()
        {
            string toReturn = "Core: ";
            for (int timeStep = 0; timeStep < MaxTime; timeStep++)
            {
                if (Queue.ContainsKey(timeStep)) toReturn += $", {Queue[timeStep].ToString()}";
                else toReturn += ", ##";
            }
            return toReturn;
        }
    }
}
