using System;
using System.Threading.Tasks;
using NativeComponent;
using ReflectionBenchmark.Callable;
using ReflectionBenchmark.DynamicCall;

namespace ReflectionBenchmark.Algorithms
{
    /// <summary>
    /// Benchmark runner scenario base.
    /// </summary>
    public abstract class BenchmarkRunnerScenarioBase : IScenario
    {
        public abstract string ScenarioName { get; }

        public async Task<BenchmarkResult> DoBenchmark()
        {
            var task = Task.Factory.StartNew(RunBenchmark);
            return await task;
        }

        /// <summary>
        /// Run count.
        /// </summary>
        protected virtual int RunCount => Consts.AlgoRunCount;

        /// <summary>
        /// Create benchmark runner.
        /// </summary>
        /// <returns>Runner.</returns>
        protected abstract IBenchmarkRunner CreateRunner();

        private BenchmarkResult RunBenchmark()
        {
            var ticks1 = Environment.TickCount;
            var runner = CreateRunner();
            runner.Run(RunCount);
            var ticks2 = Environment.TickCount;
            return new BenchmarkResult() { RunCount = RunCount, Milliseconds = ticks2 - ticks1 };
        }
    }
}