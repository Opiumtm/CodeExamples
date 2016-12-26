using System;
using System.Threading;
using System.Threading.Tasks;
using Ipatov.Async.Primitives;
using ReflectionBenchmark.Callable;

namespace ReflectionBenchmark.AsyncPrimitives
{
    /// <summary>
    /// Async region scenario set.
    /// </summary>
    /// <typeparam name="T">Region type.</typeparam>
    public abstract class AsyncRegionScenarioBase<T> : IScenario where T : IAsyncWaitRegion
    {
        /// <summary>
        /// Scenario name.
        /// </summary>
        public abstract string ScenarioName { get; }

        /// <summary>
        /// Do benchmarking (if test is synchronous, run it on background thread).
        /// </summary>
        /// <returns>Benchmark result.</returns>
        public async Task<BenchmarkResult> DoBenchmark()
        {
            var callable = new CallableClass();
            var region = CreateRegion();
            try
            {
                var ticks1 = Environment.TickCount;
                for (var i = 0; i < RunCount; i++)
                {
                    using (await region.WaitRegion(CancellationToken.None))
                    {
                        callable.Run();
                        callable.RunWithArgs(i, "");
                    }
                }
                var ticks2 = Environment.TickCount;
                return new BenchmarkResult() { RunCount = RunCount, Milliseconds = ticks2 - ticks1 };
            }
            finally
            {
                DisposeRegion(region);
            }
        }

        /// <summary>
        /// Create async region.
        /// </summary>
        /// <returns>Region.</returns>
        protected abstract T CreateRegion();

        private void DisposeRegion(T region)
        {
            (region as IDisposable)?.Dispose();
        }

        /// <summary>
        /// Run count.
        /// </summary>
        protected abstract int RunCount { get; }
    }
}