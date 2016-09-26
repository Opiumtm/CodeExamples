using System;

namespace ReflectionBenchmark.WeakEvents
{
    public sealed class LockDicWeakEventChannelScenario : WeakEventChannelScenarioBase
    {
        public override string ScenarioName => "LockDic weak event channel";

        protected override IWeakEventChannel CreateChannel()
        {
            return new LockDic.WeakEventChannel(Guid.NewGuid());
        }

        protected override void DoBenchmarkCycle(int count, IWeakEventChannel channel)
        {
            base.DoBenchmarkCycle(count, channel);
            LockDic.WeakEventChannel.DoCleanup();
        }

        protected override void DoBenchmarkSync()
        {
            LockDic.WeakEventChannel.CleanChannels();
            base.DoBenchmarkSync();
        }
    }
}