using System;
using System.Threading;
using System.Threading.Tasks;
using ReflectionBenchmark.Callable;
using ReflectionBenchmark.DynamicCall;

namespace ReflectionBenchmark.Locks
{
    /// <summary>
    /// Call inside ReaderWriterLock on write
    /// </summary>
    public class WriterLockedCallScenario : IScenario
    {
        public string ScenarioName => "Writer lock call";

        public async Task<BenchmarkResult> DoBenchmark()
        {
            var task = Task.Factory.StartNew(RunBenchmark);
            return await task;
        }

        private BenchmarkResult RunBenchmark()
        {
            using (var rd = new ReaderWriterLockSlim())
            {
                var callable = new CallableClass();
                var ticks1 = Environment.TickCount;
                for (var i = 0; i < Consts.RunCount * 100; i++)
                {
                    rd.EnterWriteLock();
                    try
                    {
                        callable.Run();
                        callable.RunWithArgs(i, "");
                    }
                    finally
                    {
                        rd.ExitWriteLock();
                    }
                }
                var ticks2 = Environment.TickCount;
                return new BenchmarkResult() { RunCount = Consts.RunCount * 100, Milliseconds = ticks2 - ticks1 };
            }
        }
    }
}