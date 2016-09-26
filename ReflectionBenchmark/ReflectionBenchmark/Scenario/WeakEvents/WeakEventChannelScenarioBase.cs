using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReflectionBenchmark.WeakEvents
{
    /// <summary>
    /// Weak events channel scenario base.
    /// </summary>
    public abstract class WeakEventChannelScenarioBase : IScenario
    {
        /// <summary>
        /// Scenario name.
        /// </summary>
        public abstract string ScenarioName { get; }

        /// <summary>
        /// Do benchmarking (if test is synchronous, run it on background thread).
        /// </summary>
        /// <returns>Benchmark result.</returns>
        public async Task<BenchmarkResult> DoBenchmark()
        {
            var ticks1 = Environment.TickCount;
            var task = Task.Factory.StartNew(DoBenchmarkSync);
            await task;
            var ticks2 = Environment.TickCount;
            return new BenchmarkResult() { RunCount = 10000*100, Milliseconds = ticks2 - ticks1 };
        }

        protected virtual void DoBenchmarkSync()
        {
            var ch = CreateChannel();
            for (var i = 0; i < 10000; i++)
            {
                DoBenchmarkCycle(100, ch);
            }
        }

        private List<IWeakEventCallback> callbacks;

        protected virtual void DoBenchmarkCycle(int count, IWeakEventChannel channel)
        {
            callbacks = new List<IWeakEventCallback>();
            for (int i = 0; i < count; i++)
            {
                callbacks.Add(AddCallback(channel));
            }
            for (int i = 0; i < count; i++)
            {
                channel.RaiseEvent(this, null);
            }
            callbacks = null;
            GC.Collect(2, GCCollectionMode.Forced);
            GC.WaitForPendingFinalizers();
            GC.Collect(2, GCCollectionMode.Forced);
        }

        private IWeakEventCallback AddCallback(IWeakEventChannel channel)
        {
            var callback = new Callback();
            channel.AddCallback(callback);
            return callback;
        }

        /// <summary>
        /// Create weak event channel.
        /// </summary>
        /// <returns>Weak event channel.</returns>
        protected abstract IWeakEventChannel CreateChannel();

        private class Callback : IWeakEventCallback
        {
            private int _count;

            public void ReceiveWeakEvent(object sender, IWeakEventChannel channel, object e)
            {
                _count++;
            }
        }
    }
}