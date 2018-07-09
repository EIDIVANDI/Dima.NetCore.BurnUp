using System;
using System.Threading.Tasks;

namespace Dima.NetSTandard.BurnUp.CircuiteBreaker
{
    public interface ITaskRunInvoker
    {
        void InvokePlanified(Action action, TimeSpan interval);
        void Invoke(Action action, TimeSpan timeOut, IRunningTastState runningTaskCloseState);
        T Invoke<T>(Func<T> func, TimeSpan timeout, IRunningTastState runningTaskCloseState);
        Task InvokeAsync(Func<Task> func, TimeSpan timeout, IRunningTastState runningTaskCloseState);
        Task<T> InvokeAsync<T>(Func<Task<T>> func, TimeSpan timeout, IRunningTastState runningTaskCloseState);
    }
}