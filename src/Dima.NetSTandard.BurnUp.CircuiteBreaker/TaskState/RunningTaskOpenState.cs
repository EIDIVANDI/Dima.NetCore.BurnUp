using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dima.NetSTandard.BurnUp.CircuiteBreaker
{
    public class RunningTaskOpenState : RunningTaskState
    {
        TimeSpan _ReTimeOut;

        public RunningTaskOpenState(
            ITaskRunInvoker invoker,
            ITaskStateToggle stateToggle,
            TimeSpan resetTimeout) : base(invoker, stateToggle)
        {
            _ReTimeOut = resetTimeout;
        }

        public override void Run(Action action)
        {
            throw new OpenStateException();
        }

        public override TResult Run<TResult>(Func<TResult> action)
        {
            throw new OpenStateException();
        }

        public override Task Run(Func<Task> action)
        {
            throw new OpenStateException();
        }

        public override Task<TResult> Run<TResult>(Func<Task<TResult>> action)
        {
            throw new OpenStateException();
        }

        public override void Try()
        {
            TaskInvoker.InvokePlanified(
                () => _TryCloseTheGate(),
                _ReTimeOut);
        }

        private void _TryCloseTheGate()
        {
            StateToggle.TryClose(this);
        }
    }
}
