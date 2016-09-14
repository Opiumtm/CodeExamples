using System;
using System.Reflection;
using System.Threading.Tasks;

namespace ReflectionBenchmark.DynamicCall
{
    /// <summary>
    /// Reflection call scenario.
    /// </summary>
    public sealed class ReflectionCallScenario : IScenario
    {
        public string ScenarioName => "Reflection invoke call";

        public async Task<BenchmarkResult> DoBenchmark()
        {
            var task = Task.Factory.StartNew(RunBenchmark);
            return await task;
        }

        private BenchmarkResult RunBenchmark()
        {
            ICallableInterface callable = new CallableClass();
            var ticks1 = Environment.TickCount;
            for (var i = 0; i < Consts.RunCount; i++)
            {
                CallReflection(callable, i);
            }
            var ticks2 = Environment.TickCount;
            return new BenchmarkResult() { RunCount = Consts.RunCount, Milliseconds = ticks2 - ticks1 };
        }

        private static readonly TypeInfo InterfaceTypeInfo = typeof (ICallableInterface).GetTypeInfo();

        private static readonly MethodInfo RunTypeInfo = InterfaceTypeInfo.GetDeclaredMethod(nameof(ICallableInterface.Run));

        private static readonly MethodInfo RunWithArgsTypeInfo = InterfaceTypeInfo.GetDeclaredMethod(nameof(ICallableInterface.RunWithArgs));

        private void CallReflection(object callableObj, int i)
        {
            RunTypeInfo.Invoke(callableObj, new object[0]);
            RunWithArgsTypeInfo.Invoke(callableObj, new object[] {i, ""});
        }
    }
}