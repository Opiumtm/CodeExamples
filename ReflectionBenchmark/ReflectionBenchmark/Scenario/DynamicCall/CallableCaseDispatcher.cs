using System;

namespace ReflectionBenchmark.DynamicCall
{
    /// <summary>
    /// Callable dispatcher using switch statement.
    /// </summary>
    public sealed class CallableCaseDispatcher : IDispatcherProxy
    {
        private readonly ICallableInterface _callable;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="callable">Callable interface.</param>
        public CallableCaseDispatcher(ICallableInterface callable)
        {
            if (callable == null) throw new ArgumentNullException(nameof(callable));
            _callable = callable;
        }

        /// <summary>
        /// Dynamic dispatch call.
        /// </summary>
        /// <param name="methodName">Method name.</param>
        /// <param name="arguments">Call arguments.</param>
        /// <returns>Call result.</returns>
        public object DispatchCall(string methodName, params object[] arguments)
        {
            switch (methodName)
            {
                case nameof(ICallableInterface.Run):
                    if (arguments != null && arguments.Length > 0)
                    {
                        throw new ArgumentException();
                    }
                    return _callable.Run();
                case nameof(ICallableInterface.RunWithArgs):
                    if (arguments == null || arguments.Length != 2)
                    {
                        throw new ArgumentException();
                    }
                    return _callable.RunWithArgs((int) arguments[0], (string) arguments[1]);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}