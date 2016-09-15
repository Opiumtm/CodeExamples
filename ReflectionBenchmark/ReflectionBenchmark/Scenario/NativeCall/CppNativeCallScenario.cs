using System;
using System.Threading.Tasks;
using NativeComponent;
using ReflectionBenchmark.DynamicCall;

namespace ReflectionBenchmark.NativeCall
{
    /// <summary>
    /// Pure native C++ call scenario.
    /// </summary>
    public sealed class CppNativeCallScenario : IScenario
    {
        public string ScenarioName => "C++ pure native call";

        public async Task<BenchmarkResult> DoBenchmark()
        {
            var task = Task.Factory.StartNew(RunBenchmark);
            return await task;
        }

        private BenchmarkResult RunBenchmark()
        {
            INativeCaller caller = new NativeCaller();
            INativeCallable callable = new NativeCallable();
            var ticks1 = Environment.TickCount;
            caller.Invoke(callable, Consts.RunCount * 100);
            var ticks2 = Environment.TickCount;
            return new BenchmarkResult() { RunCount = Consts.RunCount * 100, Milliseconds = ticks2 - ticks1 };
        }
    }
}