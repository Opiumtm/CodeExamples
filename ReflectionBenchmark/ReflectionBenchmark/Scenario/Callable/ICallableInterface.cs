namespace ReflectionBenchmark.Callable
{
    /// <summary>
    /// Callable interface.
    /// </summary>
    public interface ICallableInterface
    {
        /// <summary>
        /// Do some work.
        /// </summary>
        /// <returns>Result.</returns>
        int Run();

        /// <summary>
        /// Do some work with arguments.
        /// </summary>
        /// <param name="arg1">Argument 1.</param>
        /// <param name="arg2">Argument 2.</param>
        /// <returns>Return value.</returns>
        int RunWithArgs(int arg1, string arg2);
    }
}