
// Gets list of jobs matching provided criteria and retrieves specific variable value from each job. Returns JSON 

// Get jobs using GetJobs4 with desired filters
// https://docshield.kofax.com/KTA/en_US/750-4kcae04o43/help/API/latest/class_agility_1_1_sdk_1_1_services_1_1_job_service.html#a54e111e77aff14d3f1902d8a18567f78
// Loop result and get desired variable value using GetJobVariableValue
// https://docshield.kofax.com/KTA/en_US/750-4kcae04o43/help/API/latest/class_agility_1_1_sdk_1_1_services_1_1_job_service.html#a6112b92dde69a2d6ed2845b0adac0766


// Set and variable id you want to extract
var variableid = "FOLDER";


// payload structure for GetJobs4
var GetJobs4Struct = function () {
    var struct = {
        "sessionId": "",
        "jobFilter":{
            "MaxNumberToRetrieve": 1000,
            "StartTimeFrom": "/Date("+new Date("2022-11-21").getTime()+")/",
            "StartTimeTo": "/Date("+new Date("2022-12-03").getTime()+")/",
            "Process":{ "Name": "KMC_DocumentsProcessing"},
            "JobStatusFilter": 4,   // completed jobs
        },
    };
    return struct;
};

// payload structure for GetJobVariableValue
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

// populate data for job query
var getJobsVarible = new GetJobs4Struct();
getJobsVarible.sessionId = JSON.parse(sessionStorage.getItem("SESSION_ID")).value;

// debug
//console.log(JSON.stringify(getJobsVarible));

// variable to hold job query result
var getJobsResult;

// call API
$.ajax({
type: "POST",
async: "false",
url: window.location.origin+"/TotalAgility/Services/Sdk/JobService.svc/json/GetJobs4",
data: JSON.stringify(getJobsVarible),
contentType: "application/json; charset=utf-8",
crossDomain: true,
dataType: "json",
success: function (result) {
    console.log("Jobs found "+result.d.JobCount);

    // save result to variable so it can be used 
    getJobsResult = result.d;
},
error: function (jqXHR, textStatus, errorThrown) {
    console.log(jqXHR);
    console.log(textStatus);
    console.log(errorThrown);
},
async: false
});

// debug, print whole result
//console.log(getJobsResult);

// populate data for get job variable
var getJobVariable = new GetJobVariableValueStruct();
getJobVariable.sessionId = JSON.parse(sessionStorage.getItem("SESSION_ID")).value;
getJobVariable.variableIdentity.Id = variableid;

// result struct that will be pushed to array
var resultStruct = function () {
    var struct = {
        "jobId": "",
        "variableValue": "",
    };
    return struct;
};

var variableValueList = [];

// loop returned jobs
for(var k in getJobsResult.Jobs) {
    //console.log(getJobsResult.Jobs[k].Identity.Id);
    var jobId = getJobsResult.Jobs[k].Identity.Id;
    getJobVariable.jobIdentity.Id = jobId;

    // debug
    //console.log("iteration "+k+" job id "+jobId);

    // call API
    $.ajax({
        type: "POST",
        async: "false",
        url: window.location.origin+"/TotalAgility/Services/Sdk/JobService.svc/json/GetJobVariableValue",
        data: JSON.stringify(getJobVariable),
        contentType: "application/json; charset=utf-8",
        crossDomain: true,
        dataType: "json",
        success: function (result) {          
            // debug
            //console.log("pushing value to list: "+result.d.Value);
            
            var output = new resultStruct();
            output.jobId = jobId;
            output.variableValue = result.d.Value
            
            // push jobid + variable value pair to result array
            variableValueList.push(output);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(jqXHR);
            console.log(textStatus);
            console.log(errorThrown);
        },
        async: false
        });

 }

// compose result into object and print as json
var returnObject = new Object;
returnObject.VariableId = variableid;
returnObject.JobFilter = getJobsVarible.jobFilter;
returnObject.JobCount = getJobsResult.JobCount;
returnObject.ValueCollection = variableValueList;

console.log("Final result, variable retrieved "+variableid+" :"); 
console.log(JSON.stringify(returnObject));

// just to be sure processing is completed
alert("Done");