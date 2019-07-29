using SimSharp;
using System;
using System.Collections.Generic;

namespace CoffeeQueueSimulator
{
    public class CoffeeDrinker
    {
        private readonly CoffeeMachine[] _coffeeMachines;
        private readonly EnvironmentWithLogging _env;
        private readonly int _number;
        private readonly TimeSpan _workingHours;
        private TimeSpan _arrivalTime;
        private readonly Stats _stats;

        public CoffeeDrinker(EnvironmentWithLogging env, TimeSpan workingHours, int number, CoffeeMachine[] coffeeMachines)
        {
            _env = env;
            _workingHours = workingHours;
            _number = number;
            _coffeeMachines = coffeeMachines;
            _stats = new Stats();

            _env.Process(GoToWork());
        }

        public IEnumerable<Event> GoToWork()
        {
            //Arrive at office around 09:00
            yield return _env.TimeoutNormalPositive(TimeSpan.FromHours(9), TimeSpan.FromHours(0.5));
            _arrivalTime = _env.Now.TimeOfDay;
            Log($"Arrived at the office, grabbing coffee :)");

            yield return _env.Process(GetCoffee());

            while (_env.Now.TimeOfDay < _arrivalTime.Add(_workingHours))
            {
                //Wait for a few hours
                yield return _env.TimeoutNormalPositive(TimeSpan.FromHours(2), TimeSpan.FromMinutes(10));
                //More coffee
                yield return _env.Process(GetCoffee());
            }

            Log($"Time to head home.");
        }

        private IEnumerable<Event> GetCoffee()
        {
            var coffeeDispensed = false;

            foreach (var coffeeMachine in _coffeeMachines)
            {
                //Walk to coffee machine
                yield return _env.TimeoutNormalPositiveD(1, 0.2);

                //Is there a queue? Then find another machine
                if (coffeeMachine.InUse >= coffeeMachine.Capacity)
                {
                    _stats.TriedAnotherMachine++;
                    continue;
                }

                _env.Process(coffeeMachine.DispenseCoffee());
                Log("Ah, coffee!");
                coffeeDispensed = true;
                break;
            }

            if (!coffeeDispensed)
            {
                _stats.NotCaffeinated++;
                Log("All machines taken :(");
            }
        }

        public void LogDailyStatistics()
        {
            Log($"I had to find another coffee machine {_stats.TriedAnotherMachine} times.");
            Log($"I couldn't get coffee {_stats.NotCaffeinated} times.");
        }

        private void Log(string message)
        {
            _env.Log($"(#{_number}) {message}");
        }
    }

    internal class Stats
    {
        public int TriedAnotherMachine { get; set; }
        public int NotCaffeinated { get; set; }
    }
}