using System;
using System.Threading;
using System.Threading.Tasks;
using ReflectionBenchmark.Callable;
using ReflectionBenchmark.DynamicCall;

namespace ReflectionBenchmark.Locks
{
    /// <summary>
    /// Call inside spin lock (...)
    /// </summary>
    public class SpinLockedCallScenario : IScenario
    {
        public string ScenarioName => "Spin locked call";

        public async Task<BenchmarkResult> DoBenchmark()
        {
            var task = Task.Factory.StartNew(RunBenchmark);
            return await task;
        }

        private BenchmarkResult RunBenchmark()
        {
            var sl = new SpinLock();            
            var callable = new CallableClass();
            var ticks1 = Environment.TickCount;
            for (var i = 0; i < Consts.RunCount * 100; i++)
            {
                bool lt = false;
                sl.Enter(ref lt);
                callable.Run();
                callable.RunWithArgs(i, "");
                if (lt)
                {
                    sl.Exit(true);
                }
            }
            var ticks2 = Environment.TickCount;
            return new BenchmarkResult() { RunCount = Consts.RunCount * 100, Milliseconds = ticks2 - ticks1 };
        }
    }
}