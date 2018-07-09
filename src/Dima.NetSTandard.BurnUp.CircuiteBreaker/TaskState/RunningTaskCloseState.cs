using Dima.NetStandard.BurnUp;
using Dima.NetStandard.BurnUp.PatternTools;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Dima.NetSTandard.BurnUp.CircuiteBreaker
{
    public class RunningTaskCloseState : RunningTaskState
    {
        readonly TimeSpan _TimeOut;
        readonly FailureRetrySeverity _FailureRetrySeverity;
        int _FailureAccured;

        public RunningTaskCloseState(
            ITaskRunInvoker invoker,
            ITaskStateToggle stateToggle,
            FailureRetrySeverity failureSeverity,
            TimeSpan timeout) 
            : base(invoker, stateToggle)
        {
            _TimeOut = timeout;
            _FailureRetrySeverity = failureSeverity;
        }

        public override void Run(Action action)
        {
            TaskInvoker.Invoke(action, _TimeOut, this);
        }

        public override TResult Run<TResult>(Func<TResult> action)
        {
            return TaskInvoker.Invoke<TResult>(action, _TimeOut, this);
        }

        public override async Task Run(Func<Task> action)
        {
            await TaskInvoker.InvokeAsync(action, _TimeOut, this);
        }

        public override async Task<TResult> Run<TResult>(Func<Task<TResult>> action)
        {
            return await TaskInvoker.InvokeAsync<TResult>(action, _TimeOut, this);
        }

        public override void Try()
        {
            _FailureAccured = 0; 
        }

        public override void RegisterFailure()
        {
            if (Interlocked.Increment(ref _FailureAccured) >= (int)_FailureRetrySeverity)
            {
                StateToggle.Open(this);
            }
        }

        public override void RegisterSuccess()
        {
            _FailureAccured = 0;
        }

    }
}
