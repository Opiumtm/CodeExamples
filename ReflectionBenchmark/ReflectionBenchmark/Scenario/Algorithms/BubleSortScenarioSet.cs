using System.Collections.Generic;
using ReflectionBenchmark.StaticCall;

namespace ReflectionBenchmark.Algorithms
{
    public class BubleSortScenarioSet : IScenarioSet
    {
        public string Name => "C++ native vs .NET bubble sort";

        public IScenario BaselineScenario { get; } = new BubbleSortNativeScenario();


        private readonly IScenario[] _scenarios = new IScenario[]
        {
            new BubbleSortDotnetScenario(), 
        };

        /// <summary>
        /// Get benchmark scenarios.
        /// </summary>
        /// <returns>Scenarios.</returns>
        public IEnumerable<IScenario> GetScenarios() => _scenarios;
    }
}