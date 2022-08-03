using System;
// Common .Net features
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
// KTA 
using TotalAgility.Sdk;
using Agility.Sdk.Model;
using Agility.Server.Scripting.ScriptAssembly;
// Database
using System.Data;
using System.Data.SqlClient;

using Agility.Sdk.Model.Jobs;
using Agility.Sdk.Model.Processes;

namespace KTA_ScriptNodeTester
{
    class TerminateJobs
    {
        [StartMethodAttribute()]
        public void TerminateJobsByProcess(ScriptParameters sp)
        {
            JobService jobService = new JobService();
            
            string sessionId = sp.InputVariables["SPP_SYSTEM_SESSION_ID"].ToString();

            short jobStatusFilter = (short) sp.InputVariables["JOBSTATUSFILTER"];
            string processName = sp.InputVariables["PROCESSNAME"].ToString();
            int maxNumberToRetrieve = (int)sp.InputVariables["MAXNUMBERTORETRIEVE"];
            DateTime? startTimeFrom = (DateTime?)sp.InputVariables["STARTTIMEFROM"];
            DateTime? startTimeTo = (DateTime?)sp.InputVariables["STARTTIMETO"];

            if (jobStatusFilter < 0 || jobStatusFilter > 13)
            {
                throw new Exception("jobStatusFilter musí být mezi 0 a 12, zadaná hodnota je "+ jobStatusFilter);
            }
            if (string.IsNullOrWhiteSpace(processName))
            {
                throw new Exception("processName musí obsahovat text");
            }
            if (maxNumberToRetrieve < 1 || maxNumberToRetrieve > 10000)
            {
                throw new Exception("maxNumberToRetrieve musí být větší než 1 a menší než 10 000, zadaná hodnota je "+ maxNumberToRetrieve);
            }

            JobList jobList = new JobList();
            do
            {
                jobList = jobService.GetJobs4(sessionId, new JobFilter4()
                {
                    JobStatusFilter = jobStatusFilter,
                    Process = new ProcessIdentity() { Name = processName },
                    MaxNumberToRetrieve = maxNumberToRetrieve,
                    StartTimeFrom = startTimeFrom,
                    StartTimeTo = startTimeTo,
                });

                if (jobList != null && jobList.JobCount > 0)
                {
                    JobIdentityCollection jobIdentities = new JobIdentityCollection();

                    // příprava job id z jobList do JobIdentityCollection pro zavolání terminate jobs
                    foreach (var job in jobList.Jobs)
                    {
                        jobIdentities.Add(job.Identity);
                    }

                    // hromadné ukončení jobů
                    jobService.TerminateJobs(sessionId, jobIdentities);
                }
                
            } while (jobList != null && jobList.JobCount > 0);
            
            
        }
    }
}
