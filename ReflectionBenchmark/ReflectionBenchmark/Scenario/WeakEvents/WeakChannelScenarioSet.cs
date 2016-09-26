using System.Collections.Generic;

namespace ReflectionBenchmark.WeakEvents
{
    public sealed class WeakChannelScenarioSet : IScenarioSet
    {
        public string Name => "Weak event channels";

        public IScenario BaselineScenario { get; } = new DvachBrowserWeakEventChannelScenario();

        private readonly IScenario[] _scenarios = new[]
        {
            new LockDicWeakEventChannelScenario(), 
        };

        public IEnumerable<IScenario> GetScenarios()
        {
            return _scenarios;
        }
    }
}