using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dima.NetStandard.BurnUp.Tasks
{
    public class TastSyncContext<Result>
    {
        public Timer Timer;
        public TaskScheduler Scheduler;


        public TaskCompletionSource<Result> CompletionSource;
        public CancellationTokenSource CancellationSource;


    }
}
