using System;
using System.Threading.Tasks;
using NativeComponent;
using ReflectionBenchmark.DynamicCall;

namespace ReflectionBenchmark.NativeCall
{
    /// <summary>
    /// WinRT/COM call scenario.
    /// </summary>
    public sealed class NativeCallScenario : IScenario
    {
        public string ScenarioName => "WinRT/COM call";

        public async Task<BenchmarkResult> DoBenchmark()
        {
            var task = Task.Factory.StartNew(RunBenchmark);
            return await task;
        }

        private BenchmarkResult RunBenchmark()
        {
            INativeCallable callable = new NativeCallable();
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