using System;
using Environment = SimSharp.Environment;

namespace CoffeeQueueSimulator
{
    public class EnvironmentWithLogging : Environment
    {
        public EnvironmentWithLogging(int randomSeed) : base(randomSeed)
        {
        }

        public override void Log(string message, params object[] args)
        {
            Console.Out.WriteLine($"{this.Now.TimeOfDay:g}: {message}");
        }
    }
}
