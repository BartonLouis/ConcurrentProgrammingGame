

using SchedulerV2;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Thread scheduler analysis: ");
        Console.WriteLine("Number of cores: 3");
        Console.WriteLine("Forecast size: 50");
        Console.WriteLine("Number of characters: 8");
        Console.WriteLine("Ideal amount on processor each: 50 * 3 / 8 = 18.75");
        Console.WriteLine("Ideal average wait time: Minimal");
        int x = 0;
        float waitTimes = 0;
        float processTime = 0;
        for (int iteration = 0; iteration < 1000; iteration++)
        {
            Character[] characters =
            {
                new Character(1, 1),
                new Character(2, 1),
                new Character(1, 2),
                new Character(2, 2),
                new Character(1, 3),
                new Character(2, 3),
                new Character(1, 4),
                new Character(2, 4),
                new Character(1, 5),
                new Character(2, 5),
                new Character(1, 6),
                new Character(2, 6),
            };
            Core[] cores =
            {
                new Core(),
                new Core(),
                new Core()
            };
            ThreadScheduler threadScheduler = new(characters, cores, 5, 15, 500, 1, 1);
            Dictionary<Character, int> analysis = new();
            foreach (Character c in characters)
            {
                analysis.Add(c, 0);
            }
            foreach (Core core in threadScheduler.GetCores())
            {
                Dictionary<Character, int> coreAnalysis = core.GetAnalysis(characters);
                foreach (Character c in coreAnalysis.Keys)
                {
                    analysis[c] += coreAnalysis[c];
                }
            }
            float average = 0;
            foreach (Character c in analysis.Keys)
            {
                average += analysis[c];
            }
            average /= analysis.Keys.Count;
            for (int i = 0; i < 50; i++)
            {
                foreach (Core c in threadScheduler.GetCores())
                {
                    c.Step(i);
                }
                foreach (Character c in characters)
                {
                    c.AnalysisStep();
                }
            }
            processTime += average;
            float waitTime = 0;
            foreach (Character c in characters)
            {
                waitTime += c.getAverageWait();
            }
            waitTime /= characters.Count();
            waitTimes += waitTime;
            x++;
        }
        processTime /= x;
        waitTimes /= x;
        Console.WriteLine("After 1000 iterations, average processor time: " + processTime);
        Console.WriteLine("Average Time Waiting: " + waitTimes);
        
    }
}