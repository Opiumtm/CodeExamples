using System.Runtime.CompilerServices;
using NativeComponent;
using ReflectionBenchmark.DynamicCall;

namespace ReflectionBenchmark.Callable
{
    /// <summary>
    /// Callable class sample.
    /// </summary>
    public sealed class CallableClass : ICallableInterface, INativeCallable
    {
        private int _callNumber;

        /// <summary>
        /// Do some work.
        /// </summary>
        /// <returns>Result.</returns>
        public int Run()
        {
            _callNumber++;
            return _callNumber;
        }

        /// <summary>
        /// Do some work with arguments.
        /// </summary>
        /// <param name="arg1">Argument 1.</param>
        /// <param name="arg2">Argument 2.</param>
        /// <returns>Return value.</returns>
        public int RunWithArgs(int arg1, string arg2)
        {
            _callNumber++;
            return _callNumber;
        }
    }

    /// <summary>
    /// Callable class sample.
    /// </summary>
    public sealed class CallableClassNoInline : ICallableInterface, INativeCallable
    {
        private int _callNumber;

        /// <summary>
        /// Do some work.
        /// </summary>
        /// <returns>Result.</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public int Run()
        {
            unchecked
            {
                _callNumber++;
            }
            return _callNumber;
        }

        /// <summary>
        /// Do some work with arguments.
        /// </summary>
        /// <param name="arg1">Argument 1.</param>
        /// <param name="arg2">Argument 2.</param>
        /// <returns>Return value.</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public int RunWithArgs(int arg1, string arg2)
        {
            unchecked
            {
                _callNumber++;
            }
            return _callNumber;
        }
    }

    /// <summary>
    /// Callable class sample.
    /// </summary>
    public static class CallableClassStatic
    {
        private static int _callNumber;

        /// <summary>
        /// Do some work.
        /// </summary>
        /// <returns>Result.</returns>
        public static int Run()
        {
            unchecked
            {
                _callNumber++;
            }
            return _callNumber;
        }

        /// <summary>
        /// Do some work with arguments.
        /// </summary>
        /// <param name="arg1">Argument 1.</param>
        /// <param name="arg2">Argument 2.</param>
        /// <returns>Return value.</returns>
        public static int RunWithArgs(int arg1, string arg2)
        {
            unchecked
            {
                _callNumber++;
            }
            return _callNumber;
        }
    }

}