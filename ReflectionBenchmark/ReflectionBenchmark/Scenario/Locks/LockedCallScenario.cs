using System;
using System.Threading.Tasks;
using ReflectionBenchmark.Callable;
using ReflectionBenchmark.DynamicCall;

namespace ReflectionBenchmark.Locks
{
    /// <summary>
    /// Call inside lock (...)
    /// </summary>
    public class LockedCallScenario : IScenario
    {
        public string ScenarioName => "Monitor locked call";

        public async Task<BenchmarkResult> DoBenchmark()
        {
            var task = Task.Factory.StartNew(RunBenchmark);
            return await task;
        }

        private object _lock = new object();

        private BenchmarkResult RunBenchmark()
        {
            var callable = new CallableClass();
            var ticks1 = Environment.TickCount;
            for (var i = 0; i < Consts.RunCount * 100; i++)
            {
                lock (_lock)
                {
                    callable.Run();
                    callable.RunWithArgs(i, "");
                }
            }
            var ticks2 = Environment.TickCount;
            return new BenchmarkResult() { RunCount = Consts.RunCount * 100, Milliseconds = ticks2 - ticks1 };
        }
    }
}