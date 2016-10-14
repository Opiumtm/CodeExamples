using System;
using System.Reflection;
using System.Threading.Tasks;
using ReflectionBenchmark.Callable;

namespace ReflectionBenchmark.DynamicCall
{
    /// <summary>
    /// Reflection delegate call scenario.
    /// </summary>
    public sealed class ReflectionDelegateCallScenario : IScenario
    {
        public string ScenarioName => "Reflection cached delegate invoke call";

        public async Task<BenchmarkResult> DoBenchmark()
        {
            var task = Task.Factory.StartNew(RunBenchmark);
            return await task;
        }

        private BenchmarkResult RunBenchmark()
        {
            ICallableInterface callable = new CallableClass();
            var ticks1 = Environment.TickCount;
            Func<int> cache1 = null;
            Func<int, string, int> cache2 = null;

            for (var i = 0; i < Consts.RunCount * 100; i++)
            {
                CallReflection(callable, i, ref cache1, ref cache2);
            }
            var ticks2 = Environment.TickCount;
            return new BenchmarkResult() { RunCount = Consts.RunCount*100, Milliseconds = ticks2 - ticks1 };
        }

        private static readonly TypeInfo InterfaceTypeInfo = typeof(ICallableInterface).GetTypeInfo();

        private static readonly MethodInfo RunTypeInfo = InterfaceTypeInfo.GetDeclaredMethod(nameof(ICallableInterface.Run));

        private static readonly MethodInfo RunWithArgsTypeInfo = InterfaceTypeInfo.GetDeclaredMethod(nameof(ICallableInterface.RunWithArgs));

        private void CallReflection(object callableObj, int i, ref Func<int> cache1, ref Func<int, string, int> cache2)
        {
            if (cache1 == null)
            {
                cache1 = RunTypeInfo.CreateDelegate(typeof (Func<int>), callableObj) as Func<int>;
            }
            if (cache2 == null)
            {
                cache2 = RunWithArgsTypeInfo.CreateDelegate(typeof(Func<int, string, int>), callableObj) as Func<int, string, int>;
            }
            cache1();
            cache2(i, "");
        }
    }
}