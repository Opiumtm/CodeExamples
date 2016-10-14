using System;
using System.Threading.Tasks;
using ReflectionBenchmark.Callable;

namespace ReflectionBenchmark.DynamicCall
{
    /// <summary>
    /// Dynamic call scenario.
    /// </summary>
    public sealed class DynamicCallScenario : IScenario
    {
        public string ScenarioName => "Dynamic invoke call";

        public async Task<BenchmarkResult> DoBenchmark()
        {
            var task = Task.Factory.StartNew(RunBenchmark);
            return await task;
        }

        private BenchmarkResult RunBenchmark()
        {
            dynamic callable = new CallableClass();
            var ticks1 = Environment.TickCount;
            for (var i = 0; i < Consts.RunCount; i++)
            {
                CallDynamic(callable, i);
            }
            var ticks2 = Environment.TickCount;
            return new BenchmarkResult() { RunCount = Consts.RunCount, Milliseconds = ticks2 - ticks1 };
        }

        private void CallDynamic(dynamic callableObj, int i)
        {
            callableObj.Run();
            callableObj.RunWithArgs(i, "");
        }
    }
}