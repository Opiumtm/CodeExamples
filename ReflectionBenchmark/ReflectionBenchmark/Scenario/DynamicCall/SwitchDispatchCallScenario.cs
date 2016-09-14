namespace ReflectionBenchmark.DynamicCall
{
    /// <summary>
    /// Switch dispatch call.
    /// </summary>
    public sealed class SwitchDispatchCallScenario : DispatchCallScenarioBase
    {
        public override string ScenarioName => "Switch dispatch call";

        protected override IDispatcherProxy CreateProxy(ICallableInterface callable)
        {
            return new CallableCaseDispatcher(callable);
        }
    }
}