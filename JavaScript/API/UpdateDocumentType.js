// Set document id and target document type id
// You need to be logged in KTA Workspace to run in script
var documentId = "";
var documentTypeId = "";

// payload structure
var UpdateDocumentTypeStruct = function () {
var struct = {
    "sessionId": "",
    "reportingData": null,
    "documentId": "",
    "documentTypeIdentity": { "Id": "" },
};

return struct;
};

// populate data
var updateDocTypePayload = new UpdateDocumentTypeStruct();

updateDocTypePayload.sessionId = JSON.parse(sessionStorage.getItem("SESSION_ID")).value;
updateDocTypePayload.documentId = documentId;
updateDocTypePayload.documentTypeIdentity.Id = documentTypeId;

//$("#Request").html("<BR/><B>Request:</B><BR/>" + JSON.stringify(createJobJSON));

// call method
$.ajax({
type: "POST",
async: "false",
url: window.location.origin+"/TotalAgility/Services/Sdk/CaptureDocumentService.svc/json/UpdateDocumentType2",
data: JSON.stringify(updateDocTypePayload),
contentType: "application/json; charset=utf-8",
crossDomain: true,
dataType: "json",
success: function (result) {
    //$("#Result").html("<BR/><B>Response: </B><BR/>" + JSON.stringify(result.d));
    console.log(JSON.stringify(result.d));
    //console.log(result.d.Value);
},
error: function (jqXHR, textStatus, errorThrown) {
    //alert("***Error***\n");
    console.log(textStatus);
    console.log(errorThrown);
}
});
