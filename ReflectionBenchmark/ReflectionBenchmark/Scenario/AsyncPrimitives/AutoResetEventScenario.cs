using System.Threading;
using Ipatov.Async.Primitives;
using ReflectionBenchmark.DynamicCall;

namespace ReflectionBenchmark.AsyncPrimitives
{
    /// <summary>
    /// Async signal test.
    /// </summary>
    public class AutoResetEventScenario : AsyncSignalScenarioBase<AsyncAutoResetEventWrapper>
    {
        public override string ScenarioName => "Auto reset event wrapper";

        /// <summary>
        /// Run count.
        /// </summary>
        protected override int RunCount => Consts.RunCount;

        /// <summary>
        /// Create async primitive.
        /// </summary>
        /// <returns>Async primitive.</returns>
        protected override AsyncAutoResetEventWrapper CreateAsyncPrimitive()
        {
            return new AsyncAutoResetEventWrapper(new AutoResetEvent(false));
        }
    }
}