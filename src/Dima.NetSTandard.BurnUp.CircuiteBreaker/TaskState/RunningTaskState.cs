using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dima.NetSTandard.BurnUp.CircuiteBreaker
{
    public abstract class RunningTaskState : IRunningTastState
    {
        readonly ITaskRunInvoker _TaskInvoker;
        readonly ITaskStateToggle _StateToggle;

        protected ITaskRunInvoker TaskInvoker { get { return _TaskInvoker; } }
        protected ITaskStateToggle StateToggle { get { return _StateToggle; } }

        public RunningTaskState(
            ITaskRunInvoker invoker,
            ITaskStateToggle stateToggle)
        {
            _TaskInvoker = invoker;
            _StateToggle = stateToggle;
        }

        public virtual void RegisterFailure() { }

        public virtual void RegisterSuccess() { }

        public abstract void Run(Action action);

        public abstract TResult Run<TResult>(Func<TResult> action);

        public abstract Task Run(Func<Task> action);

        public abstract Task<TResult> Run<TResult>(Func<Task<TResult>> action);

        public abstract void Try();
    }
}
