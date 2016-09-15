using NativeComponent;

namespace ReflectionBenchmark.Algorithms
{
    /// <summary>
    /// Native bubble sort scenario.
    /// </summary>
    public sealed class BubbleSortNativeScenario : BenchmarkRunnerScenarioBase
    {
        /// <summary>
        /// Scenario name.
        /// </summary>
        public override string ScenarioName => "C++ bubble sort";

#if DEBUG
        protected override int RunCount => 1;
#endif

        /// <summary>
        /// Create benchmark runner.
        /// </summary>
        /// <returns>Runner.</returns>
        protected override IBenchmarkRunner CreateRunner()
        {
            return new BubbleSortRunner();
        }
    }
}