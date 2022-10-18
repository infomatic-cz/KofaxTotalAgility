using System; 
using Agility.Server.Scripting.ScriptAssembly;
using Agility.Sdk.Model.Capture;
using TotalAgility.Sdk;
using System.Collections;
 
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
	// Credit to Petr Boxan

string SessionId=sp.InputVariables["[SPP_SYSTEM_SESSION_ID]"].ToString();
string FolderID=sp.InputVariables["[Folder.InstanceID]"].ToString();

CaptureDocumentService cds = new CaptureDocumentService();
DocumentDataInput2 DocumentData = new DocumentDataInput2();
PageDataCollection Pages = new PageDataCollection();          

// Expects dynamic complex with pages in Base64
object strArray =sp.InputVariables["[Pages]"];
IEnumerable myList = strArray as IEnumerable;
if (myList != null)
{
    foreach (string Element in myList)
    {
PageData Page = new PageData();
      
Page.Base64Data =Element ;
       Page.MimeType = "image/png"; 
Pages.Add(Page);
   }
}

// Create subfolder and save documents there instead of root folder
string subfolderId = cds.CreateFolder(SessionId,null,FolderID,null,0,null,new FolderTypeIdentity()
{
   Id = "BE315438E611486EA0BE1F460EEDC2F0",
   Name = "KMC subfolder"
});

sp.OutputVariables["[SelectedDocumentId]"] = cds.CreateDocumentWithPages(SessionId, null,  subfolderId, null, null, null, Pages ).DocumentId;

	//
  }
 }
}