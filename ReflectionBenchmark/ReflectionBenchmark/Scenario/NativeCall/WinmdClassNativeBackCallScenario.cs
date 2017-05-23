using System;
using System.Threading.Tasks;
using DotNetComponent;
using NativeComponent;
using ReflectionBenchmark.DynamicCall;

namespace ReflectionBenchmark.NativeCall
{
    /// <summary>
    /// WinRT/COM back call scenario.
    /// </summary>
    public sealed class WinmdClassNativeBackCallScenario : IScenario
    {
        public string ScenarioName => "WinRT/COM back CCW winmd/c call";

        public async Task<BenchmarkResult> DoBenchmark()
        {
            var task = Task.Factory.StartNew(RunBenchmark);
            return await task;
        }

        private BenchmarkResult RunBenchmark()
        {
            IWinmdNativeCaller2 caller = new WinmdNativeCaller();
            DotnetCallable callable = new DotnetCallable();
            var ticks1 = Environment.TickCount;
            caller.Invoke(callable, Consts.RunCount * 100);
            var ticks2 = Environment.TickCount;
            return new BenchmarkResult() { RunCount = Consts.RunCount * 100, Milliseconds = ticks2 - ticks1 };
        }
    }
}