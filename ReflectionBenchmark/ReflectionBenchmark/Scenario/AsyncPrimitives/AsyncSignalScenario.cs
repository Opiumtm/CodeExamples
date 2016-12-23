using Ipatov.Async.Primitives;
using ReflectionBenchmark.DynamicCall;

namespace ReflectionBenchmark.AsyncPrimitives
{
    /// <summary>
    /// Async signal test.
    /// </summary>
    public class AsyncSignalScenario : AsyncSignalScenarioBase<AsyncSignal>
    {
        public override string ScenarioName => "Async signal";

        /// <summary>
        /// Run count.
        /// </summary>
        protected override int RunCount => Consts.RunCount;

        /// <summary>
        /// Create async primitive.
        /// </summary>
        /// <returns>Async primitive.</returns>
        protected override AsyncSignal CreateAsyncPrimitive()
        {
            return new AsyncSignal();
        }
    }
}