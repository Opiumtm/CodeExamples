using System.Collections.Generic;
using ReflectionBenchmark.StaticCall;

namespace ReflectionBenchmark.NativeCall
{
    /// <summary>
    /// WinRT/COM call scenario set.
    /// </summary>
    public class NativeCallScenarioSet : IScenarioSet
    {
        public string Name => "WinRT/COM calls";

        public IScenario BaselineScenario { get; } = new SimpleCallScenario();


        private readonly IScenario[] _scenarios = new IScenario[]
        {
            new WinmdSimpleCallScenario(), 
            new NativeCallScenario(),
            new NativeBackCallScenario(), 
            new StructNativeBackCallScenario(),
            new WinmdInterfaceNativeBackCallScenario(), 
            new WinmdClassNativeBackCallScenario(), 
        };

        /// <summary>
        /// Get benchmark scenarios.
        /// </summary>
        /// <returns>Scenarios.</returns>
        public IEnumerable<IScenario> GetScenarios() => _scenarios;
    }
}