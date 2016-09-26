using System;

namespace ReflectionBenchmark.WeakEvents
{
    public sealed class DvachBrowserWeakEventChannelScenario : WeakEventChannelScenarioBase
    {
        public override string ScenarioName => "DvachBrowser weak event channel";

        protected override IWeakEventChannel CreateChannel()
        {
            return new DvachBrowser.WeakEventChannel(Guid.NewGuid());
        }
    }
}