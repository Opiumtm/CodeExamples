using System;
using System.Collections.Generic;

namespace ReflectionBenchmark.DynamicCall
{
    /// <summary>
    /// Callable dispatcher using dictionary.
    /// </summary>
    public class CallableDictionaryDispatcher : IDispatcherProxy
    {
        private readonly ICallableInterface _callable;

        private readonly Dictionary<string, Func<object[], object>> _methods = new Dictionary<string, Func<object[], object>>();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="callable">Callable interface.</param>
        public CallableDictionaryDispatcher(ICallableInterface callable)
        {
            if (callable == null) throw new ArgumentNullException(nameof(callable));
            _callable = callable;
            _methods[nameof(ICallableInterface.Run)] = RunDispatch;
            _methods[nameof(ICallableInterface.RunWithArgs)] = RunWithArgsDispatch;
        }

        private object RunDispatch(object[] arguments)
        {
            if (arguments != null && arguments.Length > 0)
            {
                throw new ArgumentException();
            }
            return _callable.Run();
        }

        private object RunWithArgsDispatch(object[] arguments)
        {
            if (arguments == null || arguments.Length != 2)
            {
                throw new ArgumentException();
            }
            return _callable.RunWithArgs((int)arguments[0], (string)arguments[1]);
        }

        /// <summary>
        /// Dynamic dispatch call.
        /// </summary>
        /// <param name="methodName">Method name.</param>
        /// <param name="arguments">Call arguments.</param>
        /// <returns>Call result.</returns>
        public object DispatchCall(string methodName, params object[] arguments)
        {
            if (_methods.ContainsKey(methodName))
            {
                return _methods[methodName](arguments);
            }
            throw new NotImplementedException();
        }
    }
}