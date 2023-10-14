using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnerGPT.Core.Extensions
{
    public static class TaskExtensions
    {
        public static Task<TNewResult> Then<TResult, TNewResult>(this Task<TResult> source, Func<TResult, TNewResult> continuationFunction, CancellationToken cancellationToken = default, TaskContinuationOptions continuationOptions = TaskContinuationOptions.None, TaskScheduler scheduler = null)
        {
            scheduler ??= TaskScheduler.Current;
            return source.ContinueWith(task =>
            {
                if (task.IsCanceled)
                    task.GetAwaiter().GetResult();

                if (task.IsFaulted)
                {
                    var taskCompletionSource = new TaskCompletionSource<TNewResult>();
                    taskCompletionSource.SetException(task.Exception.InnerExceptions);
                    return taskCompletionSource.Task;
                }

                var newResult = continuationFunction(task.Result);

                return Task.FromResult(newResult);
            }, cancellationToken, continuationOptions, scheduler).Unwrap();
        }
    }
}
