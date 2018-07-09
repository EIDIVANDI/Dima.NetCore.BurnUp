using System;
using System.Collections.Generic;
using System.Text;

namespace Dima.NetStandard.BurnUp.Tasks
{
    public class NonInvokedTask : TaskInvokationStatus
    {
        public NonInvokedTask() 
            : base(TaskInvokationStatusEnum.NotInvoked)
        {}
    }
}
