using System.Collections.Generic;

namespace ReflectionBenchmark
{
    /// <summary>
    /// Scenario set.
    /// </summary>
    public interface IScenarioSet
    {
        /// <summary>
        /// Scenario set name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Baseline scenario to compare results.
        /// </summary>
        IScenario BaselineScenario { get; }

        /// <summary>
        /// Get benchmark scenarios.
        /// </summary>
        /// <returns>Scenarios.</returns>
        IEnumerable<IScenario> GetScenarios();
    }
}