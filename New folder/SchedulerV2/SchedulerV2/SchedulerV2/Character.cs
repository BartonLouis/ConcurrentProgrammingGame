using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerV2
{
    public class Character
    {
        public int teamNum;
        public int characterNum;

        public int totalWaitTime = 0;
        public int waitTime = 0;
        public int numTurns;
        public bool takenATurn = false;

        public Character(int team, int num)
        {
            teamNum = team;
            characterNum = num;
        }
        public override string ToString()
        {
            return teamNum.ToString() + characterNum.ToString();
        }

        public void Step()
        {
            totalWaitTime += waitTime;
            waitTime= 0;
            takenATurn = true;
        }

        public void AnalysisStep()
        {
            if (!takenATurn && waitTime == 0)
            {
                numTurns++;
            } 
            if (!takenATurn)
            {
                waitTime++;
            }
            takenATurn = false;
        }

        public float getAverageWait()
        {
            return totalWaitTime / numTurns;
        }

    }
}
