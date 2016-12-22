using System;
using System.Threading;
using System.Threading.Tasks;
using Ipatov.Async;
using ReflectionBenchmark.Callable;
using ReflectionBenchmark.DynamicCall;

namespace ReflectionBenchmark.Locks
{
    /// <summary>
    /// Task call
    /// </summary>
    public class SyncSerializedContextScenarion : IScenario
    {
        public string ScenarioName => "Task sync serialized task context";

        public Task<BenchmarkResult> DoBenchmark()
        {
            return RunBenchmark();
        }

        private async Task<BenchmarkResult> RunBenchmark()
        {
            var callable = new CallableClass();
            var ticks1 = Environment.TickCount;
            var context = new ExecutionSerializeContext();
            for (var i = 0; i < Consts.RunCount * 100; i++)
            {
                var i1 = i;
                await context.Execute(() =>
                {
                    callable.Run();
                    callable.RunWithArgs(i1, "");
                }, CancellationToken.None);
            }
            var ticks2 = Environment.TickCount;
            return new BenchmarkResult() { RunCount = Consts.RunCount * 1, Milliseconds = ticks2 - ticks1 };
        }
    }
}