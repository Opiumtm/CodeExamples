using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.System.Threading;
using Ipatov.Async.Primitives;
using ReflectionBenchmark.Callable;

namespace ReflectionBenchmark.AsyncPrimitives
{
    /// <summary>
    /// Async signal test base.
    /// </summary>
    /// <typeparam name="T">Async signal type.</typeparam>
    public abstract class AsyncSignalScenarioBase<T> : IScenario where T : IAsyncSignal
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
            var primitive = CreateAsyncPrimitive();
            try
            {
                var ticks1 = Environment.TickCount;
                for (var i = 0; i < RunCount; i++)
                {
                    await primitive.Set();
                    await primitive.Wait(CancellationToken.None);
                    callable.Run();
                    callable.RunWithArgs(i, "");
                }
                var ticks2 = Environment.TickCount;
                return new BenchmarkResult() { RunCount = RunCount, Milliseconds = ticks2 - ticks1 };
            }
            finally
            {
                DisposeAsyncPrimitive(primitive);
            }
        }

        /// <summary>
        /// Run count.
        /// </summary>
        protected abstract int RunCount { get; }

        /// <summary>
        /// Create async primitive.
        /// </summary>
        /// <returns>Async primitive.</returns>
        protected abstract T CreateAsyncPrimitive();

        /// <summary>
        /// Dispose async primitive.
        /// </summary>
        /// <param name="primitive">Primitive.</param>
        protected virtual void DisposeAsyncPrimitive(T primitive)
        {
            (primitive as IDisposable)?.Dispose();
        }
    }
}