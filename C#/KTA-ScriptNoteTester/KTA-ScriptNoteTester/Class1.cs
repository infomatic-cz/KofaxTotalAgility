using System;
// Common .Net features
using System.Collections.Generic;
using System.Linq;
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

namespace KTA_ScriptNoteTester
{
    class Class1
    {
        [StartMethodAttribute()]
        public void Method1(ScriptParameters sp)
        {
            JobService jobService = new JobService();

            string sessionId = sp.InputVariables["SPP_SYSTEM_SESSION_ID"].ToString();
            //string documentId = sp.InputVariables["InputProcessVariableName"].ToString();

            Agility.Sdk.Model.Jobs.JobFilter4 jobFilter = new Agility.Sdk.Model.Jobs.JobFilter4();

            jobFilter.MaxNumberToRetrieve = 50;
            jobFilter.JobStatusFilter = 1;

            Agility.Sdk.Model.Jobs.JobList jobList = jobService.GetJobs4(sessionId, jobFilter);

        }
    }
}
