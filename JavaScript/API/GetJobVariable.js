// Set jobid and variable id, you need to be logged in KTA Workspace
var jobid = "D14D6866230511ED8136005056940698";
var variableid = "FOLDER";

// payload structure
var GetJobVariableValueStruct = function () {
var struct = {
    "sessionId": "",
    "jobIdentity":{
        "Id":""
    },
    "variableIdentity":{
        "Id":""
    }  
};

return struct;
};

// populate data
var getJobVariable = new GetJobVariableValueStruct();

getJobVariable.sessionId = JSON.parse(sessionStorage.getItem("SESSION_ID")).value;
getJobVariable.jobIdentity.Id = jobid;
getJobVariable.variableIdentity.Id = variableid;

//$("#Request").html("<BR/><B>Request:</B><BR/>" + JSON.stringify(createJobJSON));

// call method
$.ajax({
type: "POST",
async: "false",
url: window.location.origin+"/TotalAgility/Services/Sdk/JobService.svc/json/GetJobVariableValue",
data: JSON.stringify(getJobVariable),
contentType: "application/json; charset=utf-8",
crossDomain: true,
dataType: "json",
success: function (result) {
    //$("#Result").html("<BR/><B>Response: </B><BR/>" + JSON.stringify(result.d));
    //console.log(JSON.stringify(result.d));
    console.log(result.d.Value);
},
error: function (jqXHR, textStatus, errorThrown) {
    //alert("***Error***\n");
    console.log(textStatus);
    console.log(errorThrown);
}
});
