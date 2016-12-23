using System.Collections.Generic;
using ReflectionBenchmark.StaticCall;

namespace ReflectionBenchmark.AsyncPrimitives
{
    public class AsyncPrimitivesScenarioSet : IScenarioSet
    {
        public string Name => "Async primitives";

        public IScenario BaselineScenario { get; } = new SimpleCallScenario();


        private readonly IScenario[] _scenarios = new IScenario[]
        {
            new AsyncSignalScenario(), 
            new AutoResetEventScenario(),
        };

        /// <summary>
        /// Get benchmark scenarios.
        /// </summary>
        /// <returns>Scenarios.</returns>
        public IEnumerable<IScenario> GetScenarios() => _scenarios;
    }
}