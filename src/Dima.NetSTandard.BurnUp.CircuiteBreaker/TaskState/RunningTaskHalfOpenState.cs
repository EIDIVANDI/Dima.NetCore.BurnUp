using Dima.NetStandard.BurnUp.Tasks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Dima.NetSTandard.BurnUp.CircuiteBreaker
{
    public class RunningTaskHalfOpenState : RunningTaskState
    {
        TaskInvokationStatus _Status;

        private readonly TimeSpan _TimeOut;

        public RunningTaskHalfOpenState(
            ITaskRunInvoker invoker,
            ITaskStateToggle stateToggle,
            TimeSpan timeout) : base(invoker, stateToggle)
        {
            _TimeOut = timeout;
        }

        public override void Run(Action action)
        {
            if (TaskInvokationVerification())
                TaskInvoker.Invoke( action, _TimeOut, this);
            else
                throw new OpenStateException();

        }

        public override TResult Run<TResult>(Func<TResult> action)
        {
            if (TaskInvokationVerification())
                return TaskInvoker.Invoke(action, _TimeOut, this);
            else
                throw new OpenStateException();
        }

        public override async Task Run(Func<Task> action)
        {
            if (TaskInvokationVerification())
                await TaskInvoker.Invoke(action, _TimeOut, this);
            else
                throw new OpenStateException();
        }

        public override async Task<TResult> Run<TResult>(Func<Task<TResult>> action)
        {
            if (TaskInvokationVerification())
                return await TaskInvoker.Invoke(action, _TimeOut, this);
            else
                throw new OpenStateException();
        }

        public override void RegisterFailure()
        {
                StateToggle.Open(this);
        }

        public override void RegisterSuccess()
        {
            StateToggle.Close(this);
        }

        public override void Try()
        {
            _Status = new NonInvokedTask();
        }

        bool TaskInvokationVerification()
        {
            return Interlocked.CompareExchange<TaskInvokationStatus>(ref _Status, new InvokedTask() , new NonInvokedTask()) == new NonInvokedTask();
        }
    }
}
