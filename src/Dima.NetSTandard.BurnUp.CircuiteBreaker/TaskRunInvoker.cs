using System;
using Dima.NetStandard.BurnUp.Extensions;
using System.Threading;
using System.Threading.Tasks;

namespace Dima.NetSTandard.BurnUp.CircuiteBreaker
{
    public class TaskRunInvoker : ITaskRunInvoker
    {
        // this is the task schaduler which is responsable of MT synchronisation 
        private readonly TaskScheduler _Schaduler;
        private Timer _timer;

        public TaskRunInvoker(TaskScheduler taskScheduler) => _Schaduler = taskScheduler;

        public void InvokePlanified(Action methodCall, TimeSpan interval)
        {
            if (methodCall == null) throw new ArgumentNullException();

            _timer = new Timer(_ => methodCall(),
                                    null,
                                    interval,
                                    new TimeSpan(0,0,0,-1));
        }

        public void Invoke(Action methodCall, TimeSpan timeout, IRunningTastState state)
        {
            try
            {
                Invoke(methodCall, timeout);
                state.RegisterSuccess();
            }
            catch (Exception)
            {
                state.RegisterFailure();
                throw;
            }
        }

        public T Invoke<T>(Func<T> methodCall, TimeSpan timeout, IRunningTastState state)
        {
            try
            {
                T result = Invoke(methodCall, timeout);
                state.RegisterSuccess();
                return result;
            }
            catch (Exception)
            {
                state.RegisterFailure();
                throw;
            }
        }

        public async Task InvokeAsync(Func<Task> methoCall, TimeSpan timeout, IRunningTastState state)
        {
            try
            {
                await InvokeAsync(methoCall, timeout);
                state.RegisterSuccess();
            }
            catch (Exception)
            {
                state.RegisterFailure();
                throw;
            }
        }

        public async Task<T> InvokeAsync<T>(Func<Task<T>> MethodCall, TimeSpan timeout, IRunningTastState state)
        {
            try
            {
                Task<T> task = InvokeAsync(MethodCall, timeout);
                var result = await task;

                state.RegisterSuccess();
                return result;
            }
            catch (Exception)
            {
                state.RegisterFailure();
                throw;
            }
        }

        private void Invoke(Action methodCall, TimeSpan timeout)
        {
            CancellationTokenSource cntkn;

            if (methodCall == null)
                throw new ArgumentNullException();

            cntkn = new CancellationTokenSource();
            var tkn = cntkn.Token;

            var task = Task.Factory.StartNew(methodCall, tkn, TaskCreationOptions.None, _Schaduler);
            if (task.IsCompleted || task.Wait((int)timeout.TotalMilliseconds, tkn))
                return;

            cntkn.Cancel();
            throw new TimeoutException();
        }

        private async Task InvokeAsync(Func<Task> methodCall, TimeSpan timeout)
        {
            if (methodCall == null)
                throw new ArgumentNullException();

            await Task.Factory.StartNew(
                methodCall,
                CancellationToken.None,
                TaskCreationOptions.None,
                _Schaduler).Unwrap().TimeItOut(timeout);
        }

        private async Task<T> InvokeAsync<T>(Func<Task<T>> methodCall, TimeSpan timeout)
        {
            if (methodCall == null) throw new ArgumentNullException();

            return await Task<Task<T>>.Factory.StartNew(methodCall, CancellationToken.None, TaskCreationOptions.None, _Schaduler).Unwrap().TimeItOut(timeout);
        }

        private T Invoke<T>(Func<T> methodCall, TimeSpan timeout)
        {
            CancellationTokenSource cntkn;

            if (methodCall == null)
                throw new ArgumentNullException();

            cntkn = new CancellationTokenSource();
            var tkn = cntkn.Token;

            var task = Task<T>.Factory.StartNew(methodCall, tkn, TaskCreationOptions.None, _Schaduler);
            
            if (task.IsCompleted || task.Wait((int)timeout.TotalMilliseconds, tkn))
                return task.Result;

            cntkn.Cancel();
            throw new TimeoutException();
        }
    }
}
