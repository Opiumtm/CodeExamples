namespace ReflectionBenchmark.DynamicCall
{
    /// <summary>
    /// Dictionary dispatch call.
    /// </summary>
    public sealed class DictionaryDispatchCallScenario : DispatchCallScenarioBase
    {
        public override string ScenarioName => "Dictionary dispatch call";

        protected override IDispatcherProxy CreateProxy(ICallableInterface callable)
        {
            return new CallableDictionaryDispatcher(callable);
        }
    }
}