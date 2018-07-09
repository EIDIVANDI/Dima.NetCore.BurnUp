using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Dima.NetSTandard.BurnUp.CircuiteBreaker
{
    public class TaskStateToggle : ITaskStateToggle
    {
        public void Close(IRunningTastState state)
        {
            throw new NotImplementedException();
        }

        public void Open(IRunningTastState state)
        {
            throw new NotImplementedException();
        }

        public void TryClose(IRunningTastState state)
        {
            throw new NotImplementedException();
        }

        //void Trip()
        //{
        //    if (Interlocked.CompareExchange(ref _currentState, to, from) == from)
        //    {
        //        to.Enter();
        //        return true;
        //    }
        //    return false;
        //}
    }
}
