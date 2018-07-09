using System;
using System.Collections.Generic;
using System.Text;

namespace Dima.NetStandard.BurnUp.Tasks
{
    public class InvokingTask : TaskInvokationStatus
    {
        public InvokingTask() : base(TaskInvokationStatusEnum.Invoking)
        {
        }
    }
}
