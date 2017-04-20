using System.Collections.Generic;

namespace ReflectionBenchmark.EmbeddedDatabase
{
    /// <summary>
    /// Embedded database scenario set.
    /// </summary>
    public sealed class EmbeddedDatabaseScenarioSet : IScenarioSet
    {
        /// <summary>
        /// Scenario set name.
        /// </summary>
        public string Name => "Embedded DB scenario set";

        /// <summary>
        /// Baseline scenario to compare results.
        /// </summary>
        public IScenario BaselineScenario => null;

        /// <summary>
        /// Get benchmark scenarios.
        /// </summary>
        /// <returns>Scenarios.</returns>
        public IEnumerable<IScenario> GetScenarios()
        {
            return new IScenario[]
            {
                new EsentInsertScenario(),
                new EsentInsertScenarioWithTransaction(), 
                new SqliteInsertScenario(), 
                new EsentQueryScenario(), 
                new SqliteQueryScenario(), 
            };
        }
    }
}