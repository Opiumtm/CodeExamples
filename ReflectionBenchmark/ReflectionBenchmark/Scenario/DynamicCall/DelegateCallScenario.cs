using System;
using System.Threading.Tasks;
using ReflectionBenchmark.Callable;

namespace ReflectionBenchmark.DynamicCall
{
    public class DelegateCallScenario : IScenario
    {
        public string ScenarioName => ".NET delegate call";

        public async Task<BenchmarkResult> DoBenchmark()
        {
            var task = Task.Factory.StartNew(RunBenchmark);
            return await task;
        }

        private BenchmarkResult RunBenchmark()
        {
            var callable = new CallableClass();
            var ticks1 = Environment.TickCount;
            var d1 = new Func<int>(callable.Run);
            var d2 = new Func<int, string, int>(callable.RunWithArgs);
            for (var i = 0; i < Consts.RunCount * 100; i++)
            {
                d1();
                d2(i, "");
            }
            var ticks2 = Environment.TickCount;
            return new BenchmarkResult() { RunCount = Consts.RunCount * 100, Milliseconds = ticks2 - ticks1 };
        }

    }

    public class StaticDelegateCallScenario : IScenario
    {
        public string ScenarioName => ".NET static delegate call";

        public async Task<BenchmarkResult> DoBenchmark()
        {
            var task = Task.Factory.StartNew(RunBenchmark);
            return await task;
        }

        private BenchmarkResult RunBenchmark()
        {
            var ticks1 = Environment.TickCount;
            var d1 = new Func<int>(CallableClassStatic.Run);
            var d2 = new Func<int, string, int>(CallableClassStatic.RunWithArgs);
            for (var i = 0; i < Consts.RunCount * 100; i++)
            {
                d1();
                d2(i, "");
            }
            var ticks2 = Environment.TickCount;
            return new BenchmarkResult() { RunCount = Consts.RunCount * 100, Milliseconds = ticks2 - ticks1 };
        }

    }

}