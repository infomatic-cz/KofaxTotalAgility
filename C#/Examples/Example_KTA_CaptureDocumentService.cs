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
    public class Class_KTA_CaptureDocumentService
    {
        public Class_KTA_CaptureDocumentService()
        {
        }

        [StartMethodAttribute()]
        public void Method1(ScriptParameters sp)
        {

            CaptureDocumentService captureDocumentService = new CaptureDocumentService();

            string sessionId = sp.InputVariables["InputProcessVariableName"].ToString();
            string documentId= sp.InputVariables["InputProcessVariableName"].ToString();
            string fieldId = sp.InputVariables["InputProcessVariableName"].ToString();

            // Get document and check if field is valid
            Agility.Sdk.Model.Capture.Document document = captureDocumentService.GetDocument(sessionId, null, documentId);
            if (document.Fields.Single(ff => ff.Id == fieldId).Valid)
            {
                // do when field is valid
            }
            else
            {
                // do when field is invalid
            }
            



        }


        public void CreateDocument(string sessionId, byte[] image, string mimeType)
        {

            CaptureDocumentService captureDocumentService = new CaptureDocumentService();

            Agility.Sdk.Model.Capture.DocumentDataInput2 documentDataInput2 = new Agility.Sdk.Model.Capture.DocumentDataInput2();

            documentDataInput2.Data = image;
            documentDataInput2.MimeType = mimeType;

            captureDocumentService.CreateDocument3(sessionId, null, null, null, null, documentDataInput2, 0);

        }
    }
}
