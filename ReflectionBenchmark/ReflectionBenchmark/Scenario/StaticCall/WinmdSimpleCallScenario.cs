using System;
using System.Threading.Tasks;
using ReflectionBenchmark.Callable;
using ReflectionBenchmark.DynamicCall;

namespace ReflectionBenchmark.StaticCall
{
    /// <summary>
    /// Simple call scenario.
    /// </summary>
    public sealed class WinmdSimpleCallScenario : IScenario
    {
        public string ScenarioName => ".NET direct winmd call";

        public async Task<BenchmarkResult> DoBenchmark()
        {
            var task = Task.Factory.StartNew(RunBenchmark);
            return await task;
        }

        private BenchmarkResult RunBenchmark()
        {
            var callable = new DotNetComponent.DotnetCallable();
            var ticks1 = Environment.TickCount;
            for (var i = 0; i < Consts.RunCount * 100; i++)
            {
                callable.Run();
                callable.RunWithArgs(i, "");
            }
            var ticks2 = Environment.TickCount;
            return new BenchmarkResult() { RunCount = Consts.RunCount * 100, Milliseconds = ticks2 - ticks1 };
        }
    }
}