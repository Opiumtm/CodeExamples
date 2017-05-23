using System.Collections.Generic;
using ReflectionBenchmark.StaticCall;

namespace ReflectionBenchmark.DynamicCall
{
    /// <summary>
    /// Dynamic call scenario set.
    /// </summary>
    public sealed class DynamicCallScenarioSet : IScenarioSet
    {
        /// <summary>
        /// Scenario set name.
        /// </summary>
        public string Name => "Dynamic calls";

        /// <summary>
        /// Baseline scenario to compare results.
        /// </summary>
        public IScenario BaselineScenario { get; } = new SimpleCallScenario();

        private readonly IScenario[] _scenarios = new IScenario[]
        {
            new InterfaceCallScenario(),
            new StructInterfaceCallScenario(), 
            new ReflectionCallScenario(),
            new ReflectionDelegateCallScenario(), 
            new DynamicCallScenario(), 
            new SwitchDispatchCallScenario(), 
            new DictionaryDispatchCallScenario(), 
        };

        /// <summary>
        /// Get benchmark scenarios.
        /// </summary>
        /// <returns>Scenarios.</returns>
        public IEnumerable<IScenario> GetScenarios() => _scenarios;
    }
}