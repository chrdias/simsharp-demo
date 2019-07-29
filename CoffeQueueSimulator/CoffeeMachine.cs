using SimSharp;
using System;
using System.Collections.Generic;

namespace CoffeeQueueSimulator
{
    public class CoffeeMachine : Resource
    {
        private readonly EnvironmentWithLogging _env;
        private readonly TimeSpan _brewTime;
        private readonly int _serviceInterval;
        private readonly TimeSpan _serviceTime;
        private readonly int _number;
        private int _count;

        public CoffeeMachine(EnvironmentWithLogging env, TimeSpan brewTime, int serviceInterval, TimeSpan serviceTime, int number) : base(env)
        {
            _env = env;
            _brewTime = brewTime;
            _serviceInterval = serviceInterval;
            _serviceTime = serviceTime;
            _number = number;
        }

        public IEnumerable<Event> DispenseCoffee()
        {
            using (var coffee = Request())
            {
                if (++_count % _serviceInterval == 0)
                {
                    Log($"Servicing machine. Unavailable for {_serviceTime.Minutes} minutes.");
                    yield return _env.Timeout(_serviceTime);
                }
                yield return coffee;
                yield return _env.Timeout(_brewTime);
                _count++;
            }
        }

        public void LogDailyStatistics()
        {
            Log($"Brewed {_count} cups of coffee");
        }

        private void Log(string message)
        {
            _env.Log($"(#{_number}) {message}");
        }
    }
}