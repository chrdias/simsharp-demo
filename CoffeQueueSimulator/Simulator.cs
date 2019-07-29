using System;

namespace CoffeeQueueSimulator
{
    public class Simulator
    {
        private readonly CoffeeDrinker[] _coffeeDrinkers;
        private readonly CoffeeMachine[] _coffeeMachines;
        private readonly EnvironmentWithLogging _env;

        private readonly TimeSpan _simulationTime = TimeSpan.FromHours(24);
        private readonly TimeSpan _workingHours = TimeSpan.FromHours(8);
        private readonly TimeSpan _brewTime = TimeSpan.FromMinutes(2);
        private readonly TimeSpan _serviceTime = TimeSpan.FromMinutes(30);
        private readonly int _serviceInterval = 20;


        public Simulator(int coffeeMachines, int coffeeDrinkers)
        {
            _env = new EnvironmentWithLogging(1);

            _coffeeMachines = new CoffeeMachine[coffeeMachines];
            for (var i = 0; i < coffeeMachines; i++)
                _coffeeMachines[i] = new CoffeeMachine(_env, _brewTime, _serviceInterval, _serviceTime, i);

            _coffeeDrinkers = new CoffeeDrinker[coffeeDrinkers];
            for (var i = 0; i < coffeeDrinkers; i++)
                _coffeeDrinkers[i] = new CoffeeDrinker(_env, _workingHours, i, _coffeeMachines);
        }

        public void RunSimulation()
        {
            _env.Run(_simulationTime);

            foreach (var coffeeMachine in _coffeeMachines)
            {
                coffeeMachine.LogDailyStatistics();
            }

            foreach (var coffeeDrinker in _coffeeDrinkers)
            {
                coffeeDrinker.LogDailyStatistics();
            }
        }
    }
}