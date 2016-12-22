using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ipatov.Async
{
    /// <summary>
    /// Context with serialized execution of actions.
    /// </summary>
    public sealed class ExecutionSerializeContext
    {
        private Task _currentTask;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ExecutionSerializeContext()
        {
            _currentTask = Task.FromResult(Nothing.Value);
        }

        /// <summary>
        /// Execute action with guarantee that no actions would execute in parallel.
        /// </summary>
        /// <param name="action">Action to execute.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Task to wait.</returns>
        public async Task Execute(Action action, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<Nothing>();
            var cancel = new TaskCompletionSource<Nothing>();
            cancellationToken.Register(() => cancel.TrySetCanceled());
            var oldTask = Interlocked.Exchange(ref _currentTask, tcs.Task);
            try
            {
                await Task.WhenAny(oldTask, cancel.Task);
                action?.Invoke();
            }
            finally
            {
                tcs.SetResult(Nothing.Value);
            }
        }

        /// <summary>
        /// Execute action with guarantee that no actions would execute in parallel.
        /// </summary>
        /// <param name="action">Action to execute.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Task to wait.</returns>
        public async Task Execute(Task action, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<Nothing>();
            var cancel = new TaskCompletionSource<Nothing>();
            cancellationToken.Register(() => cancel.TrySetCanceled());
            var oldTask = Interlocked.Exchange(ref _currentTask, tcs.Task);
            try
            {
                await Task.WhenAny(oldTask, cancel.Task);
                await action;
            }
            finally
            {
                tcs.SetResult(Nothing.Value);
            }
        }
    }
}