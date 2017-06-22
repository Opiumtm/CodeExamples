using System;
using System.Threading.Tasks;
using ReflectionBenchmark.DynamicCall;

namespace ReflectionBenchmark.BranchHypo
{
    public class DelegateBranchCallScenario : IScenario
    {
        public static ulong gcnt = 0;

        public string ScenarioName => "Delegate branch call";

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
            BranchDelegate branch = FalseBranch;
            for (var i = 0; i < Consts.RunCount*1000; i++)
            {
                c++;
                if (c >= 100)
                {
                    c = 0;
                    state = !state;
                    branch = state ? TrueDelegate : FalseDelegate;
                }
                branch();
            }
            var ticks2 = Environment.TickCount;
            return new BenchmarkResult() { RunCount = Consts.RunCount*1000, Milliseconds = ticks2 - ticks1 };
        }

        private static readonly BranchDelegate TrueDelegate = TrueBranch;
        private static readonly BranchDelegate FalseDelegate = FalseBranch;

        private static void TrueBranch()
        {
            gcnt += 1;
        }

        private static void FalseBranch()
        {
            gcnt += 2;
        }

        private delegate void BranchDelegate();
    }
}