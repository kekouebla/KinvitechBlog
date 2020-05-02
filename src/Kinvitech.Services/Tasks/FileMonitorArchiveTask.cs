using Kinvitech.Services.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kinvitech.Services.Tasks
{
    public class FileMonitorArchiveTask : ScheduledMonitoringTask
    {
        public FileMonitorArchiveTask() : base(new FileMonitorArchiveService())
        {

        }

        protected override int ScheduleTimeoutInSecs
        {
            get
            {
                return int.Parse(Environment.GetEnvironmentVariable(EnvVar.ARCHIVE_FOLDER_CHECK_HOURS)) * 60 * 60;
            }
        }
    }
}
