using System.Collections.Generic;
using ReflectionBenchmark.StaticCall;

namespace ReflectionBenchmark.Locks
{
    public class LockedCallScenarioSet : IScenarioSet
    {
        public string Name => "Locked calls";

        public IScenario BaselineScenario { get; } = new SimpleCallScenario();


        private readonly IScenario[] _scenarios = new IScenario[]
        {
            new LockedCallScenario(), 
            new ReaderLockedCallScenario(), 
            new WriterLockedCallScenario(), 
            new MutexLockedCallScenario(),
            new EventLockedCallScenario(),
            new SpinLockedCallScenario(),
            new SpinLockedNoBarrierCallScenario(), 
            new TaskCallScenario(),
            new AwaitTaskCallScenario(), 
        };

        /// <summary>
        /// Get benchmark scenarios.
        /// </summary>
        /// <returns>Scenarios.</returns>
        public IEnumerable<IScenario> GetScenarios() => _scenarios;
    }
}