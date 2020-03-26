using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Quan.Word.Core
{
    /// <summary>
    /// Handles anything to do with Tasks
    /// </summary>
    public interface ITaskManager
    {
        /// <summary>
        /// Queues the specified work to run on the ThreadPool and returns a Task handle for that work.
        /// </summary>
        /// <param name="action">The work to execute asynchronously</param>
        /// <param name="origin">The method/function this message was logged in</param>
        /// <param name="filePath">The code filename that this message was logged from</param>
        /// <param name="lineNumber">The line of code in the filename this message was logged from</param>
        /// <returns>A Task that represents the work queued to execute in the ThreadPool.</returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="action"/> parameter was null.</exception>
        Task Run(Action action, [CallerMemberName] string origin = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0);


        /// <summary>
        /// Queues the specified work to run on the ThreadPool and returns a Task handle for that work.
        /// </summary>
        /// <param name="action">The work to execute asynchronously</param>
        /// <param name="cancellationToken">A cancellation token that should be used to cancel the work</param>
        /// <param name="origin">The method/function this message was logged in</param>
        /// <param name="filePath">The code filename that this message was logged from</param>
        /// <param name="lineNumber">The line of code in the filename this message was logged from</param>
        /// <returns>A Task that represents the work queued to execute in the ThreadPool.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="action"/> parameter was null.</exception>
        /// <exception cref="ObjectDisposedException">
        /// The <see cref="CancellationTokenSource"/> associated with <paramref name="cancellationToken"/> was disposed.</exception>
        Task Run(Action action, CancellationToken cancellationToken, [CallerMemberName] string origin = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0);

        /// <summary>
        /// Queues the specified work to run on the ThreadPool and returns a Task(TResult) handle for that work.
        /// </summary>
        /// <param name="function">The work to execute asynchronously</param>
        /// <param name="origin">The method/function this message was logged in</param>
        /// <param name="filePath">The code filename that this message was logged from</param>
        /// <param name="lineNumber">The line of code in the filename this message was logged from</param>
        /// <returns>A Task(TResult) that represents the work queued to execute in the ThreadPool.</returns>
        /// <exception cref="System.ArgumentNullException">The <paramref name="function"/> parameter was null.</exception>
        Task<TResult> Run<TResult>(Func<TResult> function, [CallerMemberName] string origin = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0);

        /// <summary>
        /// Queues the specified work to run on the ThreadPool and returns a Task(TResult) handle for that work.
        /// </summary>
        /// <param name="function">The work to execute asynchronously</param>
        /// <param name="cancellationToken">A cancellation token that should be used to cancel the work</param>
        /// <param name="origin">The method/function this message was logged in</param>
        /// <param name="filePath">The code filename that this message was logged from</param>
        /// <param name="lineNumber">The line of code in the filename this message was logged from</param>
        /// <returns>A Task(TResult) that represents the work queued to execute in the ThreadPool.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="function"/> parameter was null.</exception>
        /// <exception cref="ObjectDisposedException">
        /// The <see cref="CancellationTokenSource"/> associated with <paramref name="cancellationToken"/> was disposed.</exception>
        Task<TResult> Run<TResult>(Func<TResult> function, CancellationToken cancellationToken, [CallerMemberName] string origin = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0);

        /// <summary>
        /// Queues the specified work to run on the ThreadPool and returns a proxy for the
        /// Task returned by <paramref name="function"/>.
        /// </summary>
        /// <param name="function">The work to execute asynchronously</param>
        /// <param name="origin">The method/function this message was logged in</param>
        /// <param name="filePath">The code filename that this message was logged from</param>
        /// <param name="lineNumber">The line of code in the filename this message was logged from</param>
        /// <returns>A Task that represents a proxy for the Task returned by <paramref name="function"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="function"/> parameter was null.</exception>
        Task Run(Func<Task> function, [CallerMemberName] string origin = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0);


        /// <summary>
        /// Queues the specified work to run on the ThreadPool and returns a proxy for the
        /// Task returned by <paramref name="function"/>.
        /// </summary>
        /// <param name="function">The work to execute asynchronously</param>
        /// <param name="cancellationToken">A cancellation token that should be used to cancel the work</param>
        /// <param name="origin">The method/function this message was logged in</param>
        /// <param name="filePath">The code filename that this message was logged from</param>
        /// <param name="lineNumber">The line of code in the filename this message was logged from</param>
        /// <returns>A Task that represents a proxy for the Task returned by <paramref name="function"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="function"/> parameter was null.</exception>
        /// <exception cref="ObjectDisposedException">
        /// The <see cref="CancellationTokenSource"/> associated with <paramref name="cancellationToken"/> was disposed.
        /// </exception>
        Task Run(Func<Task> function, CancellationToken cancellationToken, [CallerMemberName] string origin = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0);

        /// <summary>
        /// Queues the specified work to run on the ThreadPool and returns a proxy for the
        /// Task(TResult) returned by <paramref name="function"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result returned by the proxy Task.</typeparam>
        /// <param name="function">The work to execute asynchronously</param>
        /// <param name="origin">The method/function this message was logged in</param>
        /// <param name="filePath">The code filename that this message was logged from</param>
        /// <param name="lineNumber">The line of code in the filename this message was logged from</param>
        /// <returns>A Task(TResult) that represents a proxy for the Task(TResult) returned by <paramref name="function"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="function"/> parameter was null.
        /// </exception>
        Task<TResult> Run<TResult>(Func<Task<TResult>> function, [CallerMemberName] string origin = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0);

        /// <summary>
        /// Queues the specified work to run on the ThreadPool and returns a proxy for the
        /// Task(TResult) returned by <paramref name="function"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result returned by the proxy Task.</typeparam>
        /// <param name="function">The work to execute asynchronously</param>
        /// <param name="cancellationToken">A cancellation token that should be used to cancel the work</param>
        /// <param name="origin">The method/function this message was logged in</param>
        /// <param name="filePath">The code filename that this message was logged from</param>
        /// <param name="lineNumber">The line of code in the filename this message was logged from</param>
        /// <returns>A Task(TResult) that represents a proxy for the Task(TResult) returned by <paramref name="function"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="function"/> parameter was null.
        /// </exception>
        Task<TResult> Run<TResult>(Func<Task<TResult>> function, CancellationToken cancellationToken, [CallerMemberName] string origin = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0);

    }
}
