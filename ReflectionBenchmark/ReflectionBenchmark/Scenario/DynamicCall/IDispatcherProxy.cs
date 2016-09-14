namespace ReflectionBenchmark.DynamicCall
{
    /// <summary>
    /// Call dispatcher proxy.
    /// </summary>
    public interface IDispatcherProxy
    {
        /// <summary>
        /// Dynamic dispatch call.
        /// </summary>
        /// <param name="methodName">Method name.</param>
        /// <param name="arguments">Call arguments.</param>
        /// <returns>Call result.</returns>
        object DispatchCall(string methodName, params object[] arguments);
    }
}