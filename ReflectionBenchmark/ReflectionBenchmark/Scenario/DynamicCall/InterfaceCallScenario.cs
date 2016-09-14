using System;
using System.Threading.Tasks;

namespace ReflectionBenchmark.DynamicCall
{
    /// <summary>
    /// Interface cast call scenario.
    /// </summary>
    public sealed class InterfaceCallScenario : IScenario
    {
        public string ScenarioName => "Interface cast call";

        public async Task<BenchmarkResult> DoBenchmark()
        {
            var task = Task.Factory.StartNew(RunBenchmark);
            return await task;
        }

        private BenchmarkResult RunBenchmark()
        {
            ICallableInterface callable = new CallableClass();
            var ticks1 = Environment.TickCount;
            for (var i = 0; i < Consts.RunCount*100; i++)
            {
                CallInterface(callable, i);
            }
            var ticks2 = Environment.TickCount;
            return new BenchmarkResult() { RunCount = Consts.RunCount*100, Milliseconds = ticks2 - ticks1 };
        }

        private void CallInterface(object callableObj, int i)
        {
            var callable = callableObj as ICallableInterface;
            callable?.Run();
            callable?.RunWithArgs(i, "");
        }
    }
}