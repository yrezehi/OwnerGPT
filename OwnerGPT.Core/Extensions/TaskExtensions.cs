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
            return source.ContinueWith(t =>
            {
                if (t.IsCanceled) t.GetAwaiter().GetResult(); // Propagate the correct token
                if (t.IsFaulted)
                {
                    var tcs = new TaskCompletionSource<TNewResult>();
                    tcs.SetException(t.Exception.InnerExceptions);
                    return tcs.Task;
                }
                var newResult = continuationFunction(t.Result);
                return Task.FromResult(newResult);
            }, cancellationToken, continuationOptions, scheduler).Unwrap();
        }
    }
}
