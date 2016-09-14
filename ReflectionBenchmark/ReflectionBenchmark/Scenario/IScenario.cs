using System.Threading.Tasks;

namespace ReflectionBenchmark
{
    /// <summary>
    /// Benchmark scenario.
    /// </summary>
    public interface IScenario
    {
        /// <summary>
        /// Scenario name.
        /// </summary>
        string ScenarioName { get; }

        /// <summary>
        /// Do benchmarking (if test is synchronous, run it on background thread).
        /// </summary>
        /// <returns>Benchmark result.</returns>
        Task<BenchmarkResult> DoBenchmark();
    }
}