using Kinvitech.Services.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kinvitech.Services.Tasks
{
    /// <summary>
    /// File monitor SFG Task
    /// </summary>
    public class FileMonitorSfgTask : ScheduledMonitoringTask
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FileMonitorSfgTask() : base(new FileMonitorSfgService())
        {

        }

        /// <summary>
        /// Schedule timeout in seconds
        /// </summary>
        protected override int ScheduleTimeoutInSecs
        {
            get
            {
                return int.Parse(Environment.GetEnvironmentVariable(EnvVar.SFG_CHECK_IN_SECONDS));
            }
        }
    }
}
