using Dima.NetStandard.BurnUp.Tasks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dima.NetStandard.BurnUp.Extensions
{
    public static class TaskBasedExtension
    {
        public static Task<TResult> TimeItOut<TResult>(this Task<TResult> task, TimeSpan timeout)
        {
            return task.IsCompleted || (timeout == TimeSpan.MaxValue) ? task : ProcessTaskState(task, timeout);
        }

        public static Task TimeItOut(this Task task, TimeSpan timeout)
        {
            return TimeItOut<object>(task as Task<object>, timeout);
        }
        private static Task<TResult> ProcessTaskState<TResult>(Task<TResult> task, TimeSpan timeout)
        {
            TastSyncContext<TResult> taskSyncState = new TastSyncContext<TResult>();
            taskSyncState.CompletionSource = new TaskCompletionSource<TResult>();


            if (timeout == new TimeSpan(0))
            {
                taskSyncState.CompletionSource.SetException(new TimeoutException());
                return taskSyncState.CompletionSource.Task;
            }

            taskSyncState.Timer = new Timer(
                state => ((TaskCompletionSource<TResult>)state).TrySetException(new TimeoutException()),
                taskSyncState.CompletionSource,
                timeout,
                new TimeSpan(0,0,0,-1));

            
            task.ContinueWith(
                (antecedent, state) =>
                {
                    var obj = state as TastSyncContext<TResult>;
                    if (obj != null)
                    {
                        obj.Timer.Dispose();
                        VerifyTaskState(antecedent, obj.CompletionSource);
                    }
                },

                taskSyncState,
                CancellationToken.None,
                // we force async to be run as sync
                TaskContinuationOptions.ExecuteSynchronously,
                TaskScheduler.Default);

            return taskSyncState.CompletionSource.Task;
        }


        private static void VerifyTaskState<TResult>(
            Task source,
            TaskCompletionSource<TResult> completionSource)
        {
            switch (source.Status)
            {
                case TaskStatus.Faulted:
                    completionSource.TrySetException(source.Exception);
                    break;
                case TaskStatus.Canceled:
                    completionSource.TrySetCanceled();
                    break;
                case TaskStatus.RanToCompletion:
                    Task<TResult> castedSource = source as Task<TResult>;
                    completionSource.TrySetResult(
                        castedSource == null
                            ? default(TResult)
                            : castedSource.Result); 
                    break;
                    
            }
        }
    }
}
