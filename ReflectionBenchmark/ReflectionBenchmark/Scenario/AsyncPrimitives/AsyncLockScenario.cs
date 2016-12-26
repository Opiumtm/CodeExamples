using Ipatov.Async.Primitives;
using ReflectionBenchmark.DynamicCall;

namespace ReflectionBenchmark.AsyncPrimitives
{
    /// <summary>
    /// Async lock scenario.
    /// </summary>
    public class AsyncLockScenario : AsyncRegionScenarioBase<AsyncLock>
    {
        /// <summary>
        /// Scenario name.
        /// </summary>
        public override string ScenarioName => "Async lock call";

        protected override AsyncLock CreateRegion() => new AsyncLock();

        protected override int RunCount => Consts.RunCount * 10;
    }
}