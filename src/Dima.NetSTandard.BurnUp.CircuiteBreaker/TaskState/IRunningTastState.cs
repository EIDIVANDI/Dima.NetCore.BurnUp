using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dima.NetSTandard.BurnUp.CircuiteBreaker
{
    public interface IRunningTastState
    {
        void Try();

        void Run(Action action);

        TResult Run<TResult>(Func<TResult> action);

        Task Run(Func<Task> action);

        Task<TResult> Run<TResult>(Func<Task<TResult>> action);
        
        void RegisterFailure();

        void RegisterSuccess();

    }
}
