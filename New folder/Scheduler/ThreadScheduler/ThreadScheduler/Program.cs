using System;

namespace ThreadScheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            // Character( int team, int num)
            Character c1 = new Character(1, 1);
            Character c2 = new Character(2, 1);
            Character c3 = new Character(1, 2);
            Character c4 = new Character(2, 2);
            Character[] chars = new Character[4] { c1, c2, c3, c4 };
            ThreadScheduler scheduler = new ThreadScheduler(
                3,  // Number of cores
                2,  // Minimum Time to be Queued for
                5,  // Maximum Time to be Queued for
                10, // Forecast size
                1,  // Yield Boost (the amount that friendly threads are boosted for when a thread yields)
                1,  // Passive Priority Boost (the rate at which priority passively builds)
                chars   // Characters to queue
            );
            Console.WriteLine(scheduler);
            Console.ReadLine();
            scheduler.Schedule(5, 20, true);
            Console.WriteLine(scheduler);
            Console.ReadLine();
        }
    }
}
