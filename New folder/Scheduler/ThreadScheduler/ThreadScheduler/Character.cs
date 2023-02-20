using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadScheduler
{
    public class Character
    {

        public int Team;
        public int Num;

        public void Step()
        {

        }

        public Character(int team, int num)
        {
            Team = team;
            Num = num;
        }

        public override string ToString()
        {
            if (Team == 1)
            {
                return "A" + Num;
            }
            return "B" + Num;
        }
    }
}
