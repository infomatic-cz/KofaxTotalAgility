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
    public class Class_Common
    {
        public Class_Common()
        {
        }

        [StartMethodAttribute()]
        public void ReadWriteProcessVariable(ScriptParameters sp)
        {
            // Read input variable
            string input = sp.InputVariables["InputProcessVariableName"].ToString();
            // Set output variable
            sp.OutputVariables["OutputProcessVariableName"] = null;
        }

        public void VisualStudioInteliSense()
        {
            // Ctrl + j -> list of methods for object -> String.
            // Ctrl + Shift + Space -> parameters of method String.Substring(
        }

        public void Operators()
        {
            // Declare variable
            int i, k = 2;
            int j = 1;
            string xx = "ahoj 123";

            // Assign value
            i = j;
            k = 3;
            xx = "ff";

            // Math
            i += 1;
            i = i + 1;
            i -= 1;
            i = i - 1;
            i = i * k;
            i = i / 1;

            // String
            xx = xx.Replace("123", "");
            xx = xx.Trim();
            xx = xx.Trim('0','1');
            xx = xx.Substring(1, 3);
            k = xx.Length;
            xx = xx.ToUpper();
            xx = xx.ToLower();
            xx = xx + xx;
        }

        public void TryCatch()
        {

            try
            {
                // Do you magic here
            }
            catch (Exception)
            {
                // In case of error following code is executed

                // throw will pass error to caller of method/code -> will suspend job
                // comment throw to handle it in catch -> will not suspend job
                throw;
            }
        }

        public void If()
        {
            if (true)
            {

            }
            else
            {

            }
        }

        public void For()
        {
            for (int i = 0; i < 10; i++)
            {

            }
        }

        public void ForEach()
        {
            string[] array = new string[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
            foreach (var item in array)
            {

            }
        }

        /// <summary>
        /// Suspend job with custom message
        /// </summary>
        public void ThrowException()
        {
            throw new Exception("error message");
        }
    }
}
