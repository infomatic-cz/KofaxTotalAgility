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
    public void Method1(ScriptParameters sp) 
    {

        // Log script parameters
        log.AppendScriptParameters(sp);

        try
        {


            // Input variables
            log.AppendLog("Loading input variables");
            string sessionId = sp.InputVariables["SPP_SYSTEM_SESSION_ID"].ToString();
            string rootFolderId = sp.InputVariables["RootFolderId"].ToString();
            string documentId = sp.InputVariables["EmailDocumentId"].ToString();
            
            string ConnectionString = sp.InputVariables["DB_Conn_EmailStorage"].ToString();

            string SourceJobId = sp.InputVariables["SourceJobId"].ToString();
            string EmailFrom = sp.InputVariables["EmailFrom"].ToString();
            string EmailTo = sp.InputVariables["EmailTo"].ToString();
            string EmailBody = sp.InputVariables["EmailBody"].ToString();
            string EmailCC = sp.InputVariables["EmailCC"].ToString();
            string EmailBCC = sp.InputVariables["EmailBCC"].ToString();
            string EmailSubject = sp.InputVariables["EmailSubject"].ToString();
            string MessageImportFolder = sp.InputVariables["MessageImportFolder"].ToString();
            string ImportSourceType = sp.InputVariables["ImportSourceType"].ToString();
            string InputSourceType = sp.InputVariables["InputSourceType"].ToString();
            string MessageAttachmentList = sp.InputVariables["InputSourceType"].ToString();
            string MessageId = sp.InputVariables["MessageId"].ToString();
            DateTime? MessageReceptionTimeReceived = sp.InputVariables["MessageReceptionTimeReceived"];
            string FileName = sp.InputVariables["EmailFileName"].ToString();
            
            // Get file extension from filename
            string fileType = Path.GetExtension(FileName);

            // Convert document to Base64
            log.AppendLog("Converting document file to Base64 string");
            TotalAgility.Sdk.CaptureDocumentService captureDocumentService = new CaptureDocumentService();

            MemoryStream memoryStream = new MemoryStream();
            captureDocumentService.GetDocumentFile(sessionId, null, documentId, fileType).CopyTo(memoryStream);
            byte[] doc = memoryStream.ToArray();
            if (doc.Length <= 0)
            {
                throw new Exception("File length is 0. Document id "+documentId+"");
            }

            string FileBase64 = Convert.ToBase64String(doc);

            // Prepare SQL command
            log.AppendLog("Preparing sql command");
            string commandText = @"
            
            set xact_abort on
            begin tran
                EXEC sp_getapplock @Resource = 'EmailStorage', @LockMode = 'Exclusive';

                -- TBD kontrola duplicitního zápisu přes source job id -> kdyby došlo k resetu, nechci znovu zapisovat

                INSERT INTO [EmailStorage] 
                (
                    [SourceJobId],
                    [RootFolderId],
                    [DocumentId],
                    [EmailFrom],
                    [EmailTo],
                    [EmailBody],
                    [EmailCC],
                    [EmailBCC],
                    [EmailSubject],
                    [MessageImportFolder],
                    [ImportSourceType],
                    [InputSourceType],
                    [MessageAttachmentList],
                    [MessageId],
                    [MessageReceptionTimeReceived],
                    [FileName],
                    [FileBase64]
                )

                output INSERTED.Id 

                VALUES
                (
                    @SourceJobId,
                    @RootFolderId,
                    @DocumentId,
                    @EmailFrom,
                    @EmailTo,
                    @EmailBody,
                    @EmailCC,
                    @EmailBCC,
                    @EmailSubject,
                    @MessageImportFolder,
                    @ImportSourceType,
                    @InputSourceType,
                    @MessageAttachmentList,
                    @MessageId,
                    @MessageReceptionTimeReceived,
                    @FileName,
                    @FileBase64
                )

                EXEC sp_releaseapplock @Resource = 'EmailStorage'; 
            commit tran
            ";

            // string xx = "INSERT INTO Mem_Basic(Mem_Na,Mem_Occ) output INSERTED.ID VALUES(@na,@occ)";
            // string xx1 = "SELECT COUNT(*) FROM[MIGRATION_THREAD] WHERE(( [JOB_ID] = " + JobId + ") AND( [FINISHED] = 0))";

            // Execute command
            log.AppendLog("Executing command");
            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand())
                {
                    Command.Connection = Connection;
                    Command.CommandTimeout = 600;
                    Command.CommandText = commandText;
                    Command.CommandType = CommandType.Text;

                    Command.Parameters.AddWithValue("@SourceJobId", SourceJobId);
                    Command.Parameters.AddWithValue("@RootFolderId", rootFolderId);
                    Command.Parameters.AddWithValue("@DocumentId", documentId);
                    Command.Parameters.AddWithValue("@EmailFrom", EmailFrom);
                    Command.Parameters.AddWithValue("@EmailTo", EmailTo);
                    Command.Parameters.AddWithValue("@EmailBody", EmailBody);
                    Command.Parameters.AddWithValue("@EmailCC", EmailCC);
                    Command.Parameters.AddWithValue("@EmailBCC", EmailBCC);
                    Command.Parameters.AddWithValue("@EmailSubject", EmailSubject);
                    Command.Parameters.AddWithValue("@MessageImportFolder", MessageImportFolder);
                    Command.Parameters.AddWithValue("@ImportSourceType", ImportSourceType);
                    Command.Parameters.AddWithValue("@InputSourceType", InputSourceType);
                    Command.Parameters.AddWithValue("@MessageAttachmentList", MessageAttachmentList);
                    Command.Parameters.AddWithValue("@MessageId", MessageId);
                    Command.Parameters.AddWithValue("@MessageReceptionTimeReceived", ((object)MessageReceptionTimeReceived) ?? DBNull.Value);
                    Command.Parameters.AddWithValue("@FileName", FileName);
                    Command.Parameters.AddWithValue("@FileBase64", FileBase64);

                    log.AppendLog("Opening connection");
                    Connection.Open();
                    log.AppendLog("Executing command");
                    Int64 insertedId = (Int64)Command.ExecuteScalar();
                    log.AppendLog("Command executed");

                    // uložení output proměnné z procky do proměnné v KTA, pozor na datové typy
                    sp.OutputVariables["ArchiveId"] = insertedId;
                }

                log.AppendLog("Processing concluded");
                sp.OutputVariables["LogInsert"] = log.SerializeLog();
            }
        }
        catch (Exception ex)
        {
            log.WriteToEventLog(ex);
            throw new SystemException(log.SerializeLog(), ex);
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
                string scriptParams = "Script input variables:"+Environment.NewLine;
                foreach (DictionaryEntry variable in sp.InputVariables)
                {
                    if (variable.Value != null)
                    {
                        scriptParams=scriptParams+"Name: "+variable.Key.ToString()+" type: "+variable.Value.GetType().ToString()+" value: "+variable.Value.ToString()+Environment.NewLine;
                    } 
                    else
                    {
                        scriptParams=scriptParams+"Name: "+variable.Key.ToString();
                    }
                    
                }
                scriptParams = scriptParams+"Script output variables:"+Environment.NewLine;
                foreach (DictionaryEntry variable in sp.OutputVariables)
                {
                    if (variable.Value != null)
                    {
                        scriptParams=scriptParams+"Id: "+variable.Key.ToString()+" type: "+variable.Value.GetType().ToString()+" value: "+variable.Value.ToString()+Environment.NewLine;
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
