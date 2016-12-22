using System;
using System.Threading.Tasks;
using ReflectionBenchmark.Callable;
using ReflectionBenchmark.DynamicCall;

namespace ReflectionBenchmark.Locks
{
    /// <summary>
    /// Task await call
    /// </summary>
    public class AwaitTaskCallScenario : IScenario
    {
        public string ScenarioName => "Task tcs call with await";

        public Task<BenchmarkResult> DoBenchmark()
        {
            return RunBenchmark();
        }

        private async Task<BenchmarkResult> RunBenchmark()
        {
            var callable = new CallableClass();
            var ticks1 = Environment.TickCount;
            for (var i = 0; i < Consts.RunCount * 100; i++)
            {
                var tcs = new TaskCompletionSource<bool>();
                callable.Run();
                callable.RunWithArgs(i, "");
                tcs.SetResult(true);
                await tcs.Task;
            }
            var ticks2 = Environment.TickCount;
            return new BenchmarkResult() { RunCount = Consts.RunCount * 10, Milliseconds = ticks2 - ticks1 };
        }
    }
}