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
        // Log is global so it can be easily used in whole script without sending it to every function
        public LogCollection log = new LogCollection();

        [StartMethodAttribute()]
        public void RunSetPageExtension(ScriptParameters sp)
        {
            // Log script parameters
            log.AppendScriptParameters(sp);

            try
            {
                // ---- Start logic here ----
                // Usual input variables
                string sessionId = sp.InputVariables["SPP_SYSTEM_SESSION_ID"].ToString();   // id of system session if server variable
                string folderId = sp.InputVariables["FOLDER_F938266C4CC640FC8C289D1FE732CD3E"].ToString();
                string extensionName = sp.InputVariables["SaveEmail_ArchiveIdExtensionName"].ToString();
                            
                // Define 
                //CaptureDocumentService captureDocumentService = new CaptureDocumentService();
                //Agility.Sdk.Model.Capture.Folder folder = captureDocumentService.GetFolder(sessionId, null, folderId);            

                CaptureDocumentService captureDocumentService = new CaptureDocumentService();
                Agility.Sdk.Model.Capture.Folder folder = captureDocumentService.GetFolder(sessionId, null, folderId);  

                List<string> extensions = new List<string>();
                extensions = GetExtensions(sessionId, folder, extensionName, extensions);

                if (extensions.Count == 0)
                {
                    throw new SystemException("0 unique extension values found, do you have correct extension name?");
                }
                else if (extensions.Count == 1)
                {
                    sp.OutputVariables["EmailArchiveId"] = Convert.ToInt32(extensions[0]);
                }
                else if (extensions.Count > 1)
                {
                    throw new SystemException("More then one unique values found");
                }

                // ---- End logic here ----
                log.AppendLog("Processing concluded");
                sp.OutputVariables["ProcessingLog"] = sp.InputVariables["ProcessingLog"].ToString() + Environment.NewLine + log.SerializeLog();   // update log variable name if needed
            }
            catch (Exception ex)
            {
                log.WriteToEventLog(ex);
                throw new SystemException(log.SerializeLog(), ex);

                // StackTrace st = new StackTrace(ex, true);
                // var frame = st.GetFrame(st.FrameCount - 1);
                // var lineNumber = frame.GetFileLineNumber();
                // var fileName = frame.GetFileName();
                // var methodName = frame.GetMethod().Name;

                // log.WriteToEventLog();
                // throw new Exception("Error message: "+ex.Message+Environment.NewLine+"Custom log: "+Environment.NewLine+log.SerializeLog() + Environment.NewLine + Environment.NewLine +"Stack trace: "+Environment.NewLine+ ex.StackTrace);

                //throw new Exception("Při zpravoání nastala chyba. Line: " + lineNumber.ToString() + ", Method: " + methodName +
                //    ", FileName: " + fileName + ", error message: " + ex.Message + ", stacktrace:" + ex.StackTrace);
            }


        }


        public static void AddToListIfNotDuplicate(List<string> list, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (!list.Contains(value))
                {
                    list.Add(value);
                }
            }
        }

        public List<string> GetExtensions(string sessionId, Agility.Sdk.Model.Capture.Folder folder, string extensionName, List<string> extensions)
        {

            // if (extensions == null)
            // {
            //     extensions = new List<string>();
            // }

            CaptureDocumentService captureDocumentService = new CaptureDocumentService();

            string extension = "";

            try
            {
                // folder 
                // string 	GetFolderTextExtension (string sessionId, ReportingData reportingData, string folderId, string name)
                extension = captureDocumentService.GetFolderTextExtension(sessionId, null, folder.Id, extensionName);
                log.AppendLog("Extension of folder "+folder.Id + " is " + extension);

                AddToListIfNotDuplicate(extensions, extension);
            }
            catch (System.Exception)
            {
                log.AppendLog("Folder "+folder.Id + " does not contains extension " + extension);
            }
            

            // Projdu dokumenty a strany ve vstupním folderu
            if (folder.NumberOfDocuments > 0)
            {
                foreach (Agility.Sdk.Model.Capture.Document document in folder.Documents)
                {
                    try
                    {
                        // document
                        //string 	GetTextExtension (string sessionId, ReportingData reportingData, string documentId, string name)
                        extension = captureDocumentService.GetTextExtension(sessionId, null, document.Id, extensionName);
                        log.AppendLog("Extension of document "+document.Id + " is " + extension);
                        AddToListIfNotDuplicate(extensions, extension);
                    }
                    catch (System.Exception)
                    {
                        log.AppendLog("Document "+document.Id + " does not contains extension " + extension);
                    }
                    

                    if (document.NumberOfPages > 0)
                    {
                        foreach (Agility.Sdk.Model.Capture.Page page in document.Pages)
                        {
                            try
                            {
                                // page
                                // string 	GetPageTextExtension (string sessionId, ReportingData reportingData, string documentId, string pageId, string name)                           
                                extension = captureDocumentService.GetPageTextExtension(sessionId, null, document.Id, page.Id, extensionName);
                                log.AppendLog("Extension of page "+page.Id + " is " + extension);
                                AddToListIfNotDuplicate(extensions, extension);
                            }
                            catch (System.Exception)
                            {
                                log.AppendLog("Page "+page.Id + " does not contains extension " + extension);
                            }
                            
                        }
                    }
                }
            }

            // Rekurzivně volám pro subfoldery
            if (folder.Folders != null && folder.Folders.Count > 0)
            {
                foreach (Agility.Sdk.Model.Capture.Folder subfolder in folder.Folders)
                {
                    GetExtensions(sessionId, subfolder, extensionName, extensions);
                }
            }

            return extensions;
        }





    
        // Classes for logging
        public class LogCollection
        {
            public List<LogRecord> logRecords = new List<LogRecord>();

            // Default constructor used when LogCollection is global
            public LogCollection() {}

            // Constructor which takes ScriptParameters and automaticaly adds them as first record. Used when calling from initialization method (one with [StartMethodAttribute])
            public LogCollection(ScriptParameters sp)
            {
                this.AppendLog("KTA C# script. Script parameters: "+Environment.NewLine + SerializeScriptParameters(sp));
            }

            // Simply append message to log as a new row
            public void AppendLog(string message,
                [System.Runtime.CompilerServices.CallerMemberName] string methodName = "")
            {
                this.logRecords.Add(new LogRecord(message, methodName));
            }

            // Append serialized ScriptParameters in case LogCollection is global and ScriptParameters are not awailable when constructor is called
            public void AppendScriptParameters(ScriptParameters sp,
                [System.Runtime.CompilerServices.CallerMemberName] string methodName = "")
            {
                this.logRecords.Add(new LogRecord(SerializeScriptParameters(sp), methodName));
            }

            // Dump log to event log. If Exception is provided, include it's data
            public void WriteToEventLog(Exception ex = null)
            {
                using (System.Diagnostics.EventLog eventLog = new System.Diagnostics.EventLog("Application"))
                {
                    eventLog.Source = "TotalAgility_Script";

                    string formatedMessage = "Log generated by C# script in KTA. Log entries:" + Environment.NewLine + this.SerializeLog();
                    System.Diagnostics.EventLogEntryType eventLogEntryType;

                    if (ex != null)
                    {
                        formatedMessage = formatedMessage + Environment.NewLine + Environment.NewLine + ex.ToString();
                        eventLogEntryType = System.Diagnostics.EventLogEntryType.Error;
                    }
                    else
                    {
                        eventLogEntryType = System.Diagnostics.EventLogEntryType.Information;
                    }
                    eventLog.WriteEntry(formatedMessage, eventLogEntryType);
                }
            }

            // Return string containing names and values of Script parameters passed to C# activity from process
            private static string SerializeScriptParameters(ScriptParameters sp)
            {
                string scriptParams = "Script input variables (first 100 chars):"+Environment.NewLine;
                foreach (DictionaryEntry variable in sp.InputVariables)
                {
                    if (variable.Value != null)
                    {
                        string variableValue = variable.Value.ToString();
                        // Event log is limited to 32k chars so very long variables can exceed this size. Taking first 100 chars should be sufficient for usual variables
                        if (variable.Value.ToString().Length > 100)
                        {
                            variableValue = variable.Value.ToString().Substring(0,100);
                        }
                        scriptParams=scriptParams+"Name: "+variable.Key.ToString()+" type: "+variable.Value.GetType().ToString()+" value: "+variableValue+Environment.NewLine;
                    } 
                    else
                    {
                        scriptParams=scriptParams+"Name: "+variable.Key.ToString();
                    }
                    
                }
                scriptParams = scriptParams+"Script output variables (first 100 chars):"+Environment.NewLine;
                foreach (DictionaryEntry variable in sp.OutputVariables)
                {
                    if (variable.Value != null)
                    {
                        string variableValue = variable.Value.ToString();
                        // Event log is limited to 32k chars so very long variables can exceed this size. Taking first 100 chars should be sufficient for usual variables
                        if (variable.Value.ToString().Length > 100)
                        {
                            variableValue = variable.Value.ToString().Substring(0,100);
                        }
                        scriptParams=scriptParams+"Name: "+variable.Key.ToString()+" type: "+variable.Value.GetType().ToString()+" value: "+variableValue+Environment.NewLine;
                    } 
                    else
                    {
                        scriptParams=scriptParams+"Id: "+variable.Key.ToString()+" value is null"+Environment.NewLine;
                    }
                }
                return scriptParams;                
            }

            public string SerializeLog()
            {
                string result = "";
                foreach (var record in this.logRecords.OrderBy(x => x.DateTime))
                {
                    result = result + record.ToString() + Environment.NewLine;
                }
                return result;
            }

            public class LogRecord
            {
                public DateTime DateTime;
                public string Message;
                public string Method;

                public LogRecord(string message, string method)
                {
                    this.DateTime = DateTime.Now;
                    this.Message = message;
                    this.Method = method;
                }

                public override string ToString()
                {
                    return string.Format("{0} Method: {1} Message: {2}",
                        this.DateTime.ToString("yyyy-MM-dd hh:mm:ss.fff"),
                        this.Method,
                        this.Message);
                }
            }
        }
        // Classes for logging end




    }
}