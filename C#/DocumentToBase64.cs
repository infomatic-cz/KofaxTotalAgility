using System;
// Common .Net features
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Collections;
// KTA 
using TotalAgility.Sdk;
using Agility.Sdk.Model;
using Agility.Server.Scripting.ScriptAssembly;
// Database
using System.Data;
using System.Data.SqlClient;
 
namespace MyNamespace
{
    
    public class Class1
    {
    public Class1() 
    {
    }

        /// <summary>
        /// Document to Base64 string
        /// </summary>
        /// <param name="sp"></param>
        [StartMethodAttribute()]   
        public void DocumentToBase64(ScriptParameters sp)
        {
            string sessionId = sp.InputVariables["SPP_SYSTEM_SESSION_ID"].ToString();
            string documentId = sp.InputVariables["DocumentId"].ToString();
            string fileType = sp.InputVariables["fileType"].ToString();

            TotalAgility.Sdk.CaptureDocumentService captureDocumentService = new CaptureDocumentService();

            MemoryStream memoryStream = new MemoryStream();
            captureDocumentService.GetDocumentFile(sessionId, null, documentId, fileType).CopyTo(memoryStream);
            byte[] doc = memoryStream.ToArray();

            sp.OutputVariables["DocInBase64"] = Convert.ToBase64String(doc);
        }

    }

}
