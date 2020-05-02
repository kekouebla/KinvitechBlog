using Kinvitech.Services.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kinvitech.Services.Tasks
{
    public class FileMonitorCheckInboundFolderTask : ScheduledMonitoringTask
    {
        public FileMonitorCheckInboundFolderTask() : base(new FileMonitorCheckInboundFolderService())
        {

        }

        protected override int ScheduleTimeoutInSecs
        {
            get
            {
                return int.Parse(Environment.GetEnvironmentVariable(EnvVar.INCOMING_FOLDER_CHECK_SECONDS));
            }
        }
    }
}
