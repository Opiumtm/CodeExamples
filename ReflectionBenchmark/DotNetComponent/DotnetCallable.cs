namespace DotNetComponent
{
    /// <summary>
    /// Dotnet callable class.
    /// </summary>
    public sealed class DotnetCallable : IDotnetCallableInterface
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