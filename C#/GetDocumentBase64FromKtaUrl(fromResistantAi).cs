using System; 
using Agility.Server.Scripting.ScriptAssembly;
using System.Net;
using System.IO;
using System.Text;

namespace MyNamespace
{
    
 public class Class1
 {
  public Class1() 
  {
  }
 
  [StartMethodAttribute()] 
  public void Method1(ScriptParameters sp) 
  {

     string sessionId = sp.InputVariables["SESSIONID"];
        
 string documentId = sp.InputVariables["DOCUMENTINSTANCEID"];

   string KTAServiceUri =  sp.InputVariables["KTASDKURL"];
   string fileExtension = sp.InputVariables["FILEEXTENSION"];


    KTA.CaptureDocumentService.CaptureDocumentServiceClient captureDocumentService = new KTA.CaptureDocumentService.CaptureDocumentServiceClient();

    Stream documentStream = captureDocumentService.GetDocumentFile(sessionId, null, documentId, fileExtension);


//Setting the URi and calling the get document API
var KTAGetDocumentFile = KTAServiceUri  + "/CaptureDocumentService.svc/json/GetDocumentFile";
                HttpWebRequest ktaHttpWebRequest = (HttpWebRequest)WebRequest.Create(KTAGetDocumentFile);
                ktaHttpWebRequest.ContentType = "application/json";
                ktaHttpWebRequest.Method = "POST";

   // CONSTRUCT JSON Payload

                using (var streamWriter = new StreamWriter(ktaHttpWebRequest.GetRequestStream()))
                {
           string json = "{\"sessionId\":\"" + sessionId  + "\",\"documentId\":\"" + documentId  + "\",\"fileType\":\""+ sp.InputVariables["FILEEXTENSION"] + "\"}";
                    streamWriter.Write(json);
                    streamWriter.Flush();
                }

                HttpWebResponse ktaHttpWebResponse = (HttpWebResponse)ktaHttpWebRequest.GetResponse();
                Stream receiveStream = ktaHttpWebResponse.GetResponseStream();
                Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                StreamReader readStream = new StreamReader(receiveStream, encode);
                int streamContentLength = unchecked((int)ktaHttpWebResponse.ContentLength);

                byte[] result;
                byte[] buffer = new byte[4096];

                using (Stream responseStream = ktaHttpWebResponse.GetResponseStream())
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        int count = 0;
                        do
                        {
                            count = responseStream.Read(buffer, 0, buffer.Length);
                            memoryStream.Write(buffer, 0, count);

                        } while (count != 0);

                        result = memoryStream.ToArray();
                    }
                }

   sp.OutputVariables["BASE64STRING"]  = System.Convert.ToBase64String(result);
 
                // END KTA GET DOCUMENT FILE BINARY

	 
  }
 }
}