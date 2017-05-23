using System;
using System.Threading.Tasks;
using ReflectionBenchmark.Callable;

namespace ReflectionBenchmark.DynamicCall
{
    /// <summary>
    /// Struct interface cast call scenario.
    /// </summary>
    public sealed class StructInterfaceCallScenario : IScenario
    {
        public string ScenarioName => "Struct interface cast call";

        public async Task<BenchmarkResult> DoBenchmark()
        {
            var task = Task.Factory.StartNew(RunBenchmark);
            return await task;
        }

        private BenchmarkResult RunBenchmark()
        {
            ICallableInterface callable = new CallableStruct();
            var ticks1 = Environment.TickCount;
            for (var i = 0; i < Consts.RunCount * 100; i++)
            {
                CallInterface(callable, i);
            }
            var ticks2 = Environment.TickCount;
            return new BenchmarkResult() { RunCount = Consts.RunCount * 100, Milliseconds = ticks2 - ticks1 };
        }

        private void CallInterface(object callableObj, int i)
        {
            var callable = callableObj as ICallableInterface;
            callable?.Run();
            callable?.RunWithArgs(i, "");
        }
    }
}