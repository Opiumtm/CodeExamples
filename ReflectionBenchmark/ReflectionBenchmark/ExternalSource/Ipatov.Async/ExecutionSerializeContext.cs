using System;
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
        /// <param name="taskScheduler">Task scheduler.</param>
        /// <param name="scheduleSync">Schedule synchronously.</param>
        /// <returns>Task to wait.</returns>
        public Task Execute(Action action, CancellationToken cancellationToken, TaskScheduler taskScheduler, bool scheduleSync = false)
        {
            var options = scheduleSync ? TaskContinuationOptions.ExecuteSynchronously : TaskContinuationOptions.None;
            var tcs = new TaskCompletionSource<Nothing>();
            var oldTask = Interlocked.Exchange(ref _currentTask, tcs.Task);
            oldTask.ContinueWith(t =>
            {
                tcs.SetResult(Nothing.Value);
            }, CancellationToken.None, options, taskScheduler);
            return oldTask.ContinueWith(t =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                action?.Invoke();
            }, cancellationToken, options, taskScheduler);
        }

        /// <summary>
        /// Execute action with guarantee that no actions would execute in parallel.
        /// </summary>
        /// <param name="action">Action to execute.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <param name="scheduleSync">Schedule synchronously.</param>
        /// <returns>Task to wait.</returns>
        public Task Execute(Action action, CancellationToken cancellationToken, bool scheduleSync = false)
        {
            return Execute(action, cancellationToken, TaskScheduler.Current, scheduleSync);
        }

        /// <summary>
        /// Execute action with guarantee that no actions would execute in parallel.
        /// </summary>
        /// <param name="action">Action to execute.</param>
        /// <param name="scheduleSync">Schedule synchronously.</param>
        /// <returns>Task to wait.</returns>
        public Task Execute(Action action, bool scheduleSync = false)
        {
            return Execute(action, CancellationToken.None, TaskScheduler.Current, scheduleSync);
        }
    }
}