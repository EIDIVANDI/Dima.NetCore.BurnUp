using System;
using System.Collections.Generic;
using System.Text;

namespace Dima.NetStandard.BurnUp.Tasks
{
    public class InvokedTask : TaskInvokationStatus
    {
        public InvokedTask() : base(TaskInvokationStatusEnum.Invoked)
        {}
    }
}
