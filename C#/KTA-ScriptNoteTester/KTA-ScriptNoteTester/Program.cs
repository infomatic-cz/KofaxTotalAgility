using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agility.Server.Scripting;
using Agility.Server.Scripting.ScriptAssembly;

namespace KTA_ScriptNoteTester
{
    class Program
    {
        static void Main(string[] args)
        {

            ParameterCollection input = new ParameterCollection();
            input.Add("SPP_SYSTEM_SESSION_ID", "68BB738BCA4E364593A2222D27750691");
            //input.Add("JOBID", "C3C69AE7B324409D8F3DB967F4A41FF9");
            //input.Add("FOLDERID", "9843a23c-fa5f-44f7-a817-ab1f00b8b3d7");

            ParameterCollection output = new ParameterCollection();
            output.Add("JobPropertiesXML", null);
            output.Add("JobVariablesXML", null);

            ScriptParameters sp = new ScriptParameters(input, output);

            Class1 tester = new Class1();
            tester.Method1(sp);

        }
    }
}
