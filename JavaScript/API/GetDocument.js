// Set jobid and variable id, you need to be logged in KTA Workspace
var documentId = "80033a24-f9fe-4f12-a90b-af3300cd8ee2";

// payload structure
var GetFolderStruct = function () {
var struct = {
    "sessionId": "",
    "reportingData": null,
    "documentId": "",
};

return struct;
};

// populate data
var getJobVariable = new GetFolderStruct();

getJobVariable.sessionId = JSON.parse(sessionStorage.getItem("SESSION_ID")).value;
getJobVariable.documentId = documentId;

//$("#Request").html("<BR/><B>Request:</B><BR/>" + JSON.stringify(createJobJSON));

// call method
$.ajax({
type: "POST",
async: "false",
url: window.location.origin+"/TotalAgility/Services/Sdk/CaptureDocumentService.svc/json/GetDocument",
data: JSON.stringify(getJobVariable),
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
