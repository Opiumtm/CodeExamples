using System;
using System.Threading.Tasks;

namespace ReflectionBenchmark.DynamicCall
{
    /// <summary>
    /// Dispatch call scenario.
    /// </summary>
    public abstract class DispatchCallScenarioBase : IScenario
    {
        public abstract string ScenarioName { get; }

        public async Task<BenchmarkResult> DoBenchmark()
        {
            var task = Task.Factory.StartNew(RunBenchmark);
            return await task;
        }

        /// <summary>
        /// Create dispatch proxy.
        /// </summary>
        /// <param name="callable">Callable.</param>
        /// <returns>Proxy/</returns>
        protected abstract IDispatcherProxy CreateProxy(ICallableInterface callable);

        private BenchmarkResult RunBenchmark()
        {
            var proxy = CreateProxy(new CallableClass());
            var ticks1 = Environment.TickCount;
            for (var i = 0; i < Consts.RunCount; i++)
            {
                CallProxy(proxy, i);
            }
            var ticks2 = Environment.TickCount;
            return new BenchmarkResult() { RunCount = Consts.RunCount, Milliseconds = ticks2 - ticks1 };
        }

        private void CallProxy(IDispatcherProxy proxy, int i)
        {
            proxy.DispatchCall(nameof(ICallableInterface.Run));
            proxy.DispatchCall(nameof(ICallableInterface.RunWithArgs), i, "");
        }
    }
}