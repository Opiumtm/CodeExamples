using System;
using System.Diagnostics;
using System.Threading.Tasks;
using ReflectionBenchmark.Callable;
using ReflectionBenchmark.DynamicCall;

namespace ReflectionBenchmark.BranchHypo
{
    public class IfBranchCallScenario : IScenario
    {
        public static ulong gcnt = 0;

        public string ScenarioName => "If branch call";

        public async Task<BenchmarkResult> DoBenchmark()
        {
            var task = Task.Factory.StartNew(RunBenchmark);
            return await task;
        }

        private BenchmarkResult RunBenchmark()
        {
            var ticks1 = Environment.TickCount;
            gcnt = 0;
            int c = 0;
            bool state = false;
            for (var i = 0; i < Consts.RunCount*1000; i++)
            {
                c++;
                if (c >= 100)
                {
                    c = 0;
                    state = !state;
                }
                if (state)
                {
                    gcnt += 1;
                }
                else
                {
                    gcnt += 2;
                }
            }
            var ticks2 = Environment.TickCount;
            return new BenchmarkResult() { RunCount = Consts.RunCount*1000, Milliseconds = ticks2 - ticks1 };
        }
    }
}