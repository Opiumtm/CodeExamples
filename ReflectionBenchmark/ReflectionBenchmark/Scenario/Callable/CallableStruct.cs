﻿using NativeComponent;

namespace ReflectionBenchmark.Callable
{
    /// <summary>
    /// Callable struct sample.
    /// </summary>
    public struct CallableStruct : ICallableInterface, INativeCallable
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
}