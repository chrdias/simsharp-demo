using System;

namespace CoffeeQueueSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.Out.WriteLine("Usage: CoffeeQueueSimulator <#coffeeMachines> <#coffeeDrinkers>");
                return;
            }

            if (!int.TryParse(args[0], out int coffeeMachines))
            {
                Console.Out.WriteLine($"{args[0]} should be an integer");
                return;
            }

            if (!int.TryParse(args[1], out int coffeeDrinkers))
            {
                Console.Out.WriteLine($"{args[0]} should be an integer");
                return;
            }

            Console.WriteLine("Another day at the office");
            var coffeeQueueSimulator = new Simulator(coffeeMachines, coffeeDrinkers);
            coffeeQueueSimulator.RunSimulation();
        }
    }
}
