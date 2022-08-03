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


namespace MyNamespace
{
    public class Class_RegEx
    {
        public Class_RegEx()
        {
        }

        [StartMethodAttribute()]
        public void Method1(ScriptParameters sp)
        {
            // https://docs.microsoft.com/cs-cz/dotnet/api/system.text.regularexpressions.regex?view=net-5.0
            // regex tester/builder https://regexr.com/

            // Read input variables
            string input = sp.InputVariables["InputProcessVariableName"].ToString();
            string pattern= sp.InputVariables["InputProcessVariableName"].ToString();

            string emailRegEx = @"\w +([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";

            // Simple check, true if pattern is found, false otherwise
            sp.OutputVariables["OutputProcessVariableName"] = Regex.IsMatch(input, pattern);

            // Simple extraction, returns first match
            sp.OutputVariables["OutputProcessVariableName"] = Regex.Match(input, pattern).Value;

        }
    }
}
