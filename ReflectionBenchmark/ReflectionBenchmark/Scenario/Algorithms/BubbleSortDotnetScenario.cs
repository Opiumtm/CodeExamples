using NativeComponent;

namespace ReflectionBenchmark.Algorithms
{
    /// <summary>
    /// .NET bubble sort scenario.
    /// </summary>
    public sealed class BubbleSortDotnetScenario : BenchmarkRunnerScenarioBase
    {
        /// <summary>
        /// Scenario name.
        /// </summary>
        public override string ScenarioName => ".NET bubble sort";

        /// <summary>
        /// Create benchmark runner.
        /// </summary>
        /// <returns>Runner.</returns>
        protected override IBenchmarkRunner CreateRunner()
        {
            return new DotNetBubbleSortRunner();
        }
    }
}