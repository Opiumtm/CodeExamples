using System;
using System.Threading;
using System.Threading.Tasks;
using ReflectionBenchmark.Callable;
using ReflectionBenchmark.DynamicCall;

namespace ReflectionBenchmark.Locks
{
    /// <summary>
    /// Call inside mutex
    /// </summary>
    public class MutexLockedCallScenario : IScenario
    {
        public string ScenarioName => "Mutex lock call";

        public async Task<BenchmarkResult> DoBenchmark()
        {
            var task = Task.Factory.StartNew(RunBenchmark);
            return await task;
        }

        private BenchmarkResult RunBenchmark()
        {
            using (var m = new Mutex())
            {
                var callable = new CallableClass();
                var ticks1 = Environment.TickCount;
                for (var i = 0; i < Consts.RunCount * 100; i++)
                {
                    m.WaitOne();
                    try
                    {
                        callable.Run();
                        callable.RunWithArgs(i, "");
                    }
                    finally
                    {
                        m.ReleaseMutex();
                    }
                }
                var ticks2 = Environment.TickCount;
                return new BenchmarkResult() { RunCount = Consts.RunCount * 10, Milliseconds = ticks2 - ticks1 };
            }
        }
    }
}