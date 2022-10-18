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

                CaptureDocumentService captureDocumentService = new CaptureDocumentService();

                string sessionId = sp.InputVariables["SPP_SYSTEM_SESSION_ID"].ToString();
                string folderId = sp.InputVariables["RootFolderId"].ToString();
                string extensionName = sp.InputVariables["SaveEmail_ArchiveIdExtensionName"].ToString();
                string extensionValue = sp.InputVariables["ArchiveId"].ToString();

                Agility.Sdk.Model.Capture.Folder folder = captureDocumentService.GetFolder(sessionId, null, folderId);

                log.AppendLog("Starting processing");
                SetExtension(sessionId, folder, extensionName, extensionValue);
                log.AppendLog("Ending processing");

                sp.OutputVariables["LogExtension"] = log.SerializeLog();
            }
            catch (Exception ex)
            {
                log.WriteToEventLog(ex);
                throw new SystemException(log.SerializeLog(), ex);
            }
        }


        /// <summary>
        /// Všem folderům, dokumentům a stranám nastaví text extension
        /// </summary>
        /// <param name="sessionId">SessionId pro volání KTA API</param>
        /// <param name="folder">Objekt vstupního folderu</param>
        /// <param name="extensionName">Jméno extension</param>
        /// <param name="extensionValue">Hodnota extension</param>
        public void SetExtension(string sessionId, Agility.Sdk.Model.Capture.Folder folder, string extensionName, string extensionValue)
        {
            CaptureDocumentService captureDocumentService = new CaptureDocumentService();

            // folder 
            // SaveFolderTextExtension (string sessionId, ReportingData reportingData, string folderId, string name, string text)
            log.AppendLog("Set extension to folder "+folder.Id);
            captureDocumentService.SaveFolderTextExtension(sessionId, null, folder.Id, extensionName, extensionValue);


            // Projdu dokumenty a strany ve vstupním folderu
            if (folder.NumberOfDocuments > 0)
            {
                foreach (Agility.Sdk.Model.Capture.Document document in folder.Documents)
                {
                    // document
                    //SaveTextExtension (string sessionId, ReportingData reportingData, string documentId, string name, string text)
                    log.AppendLog("Set extension to document "+document.Id);
                    captureDocumentService.SaveTextExtension(sessionId, null, document.Id, extensionName, extensionValue);
                    if (document.NumberOfPages > 0)
                    {
                        foreach (Agility.Sdk.Model.Capture.Page page in document.Pages)
                        {
                            // page
                            log.AppendLog("Set extension to page "+page.Id);
                            captureDocumentService.SavePageTextExtension(sessionId, null, document.Id, page.Id, extensionName, extensionValue);
                        }
                    }
                }
            }

            // Rekurzivně volám pro subfoldery
            if (folder.Folders != null && folder.Folders.Count > 0)
            {
                foreach (Agility.Sdk.Model.Capture.Folder subfolder in folder.Folders)
                {
                    SetExtension(sessionId, subfolder, extensionName, extensionValue);
                }
            }
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










