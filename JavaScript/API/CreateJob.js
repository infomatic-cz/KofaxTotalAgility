var InputVariableStruct = function () {
 var struct = {
	 "DisplayName": "",
	 "Id": "",
	 "Value": "",
	 "VariableType": {
		 "FormattedAsText": null,
		 "Value": ""
	 }
 };

 return struct;
};

var CreateJobStruct = function () {
 var struct = {
	 "jobInitialization": {
		 "InputVariables": [],
		 "StartDate": null,
	 },
	 "processIdentity": {
		 "Id": "",
		 "Name": "",
		 "Version": 0
	 },
	 "sessionId": ""
 };

 return struct;
};


var createJobJSON = new CreateJobStruct();

var varFirstName = new InputVariableStruct();
//varFirstName.DisplayName = "FirstName";
varFirstName.Id = "JOBID";
varFirstName.Value = "Ferda"; // string
createJobJSON.jobInitialization.InputVariables.push(varFirstName);


//varLoanApplied.Value = "2016-01-05T10:19:00.000Z"; //Date
//varAmountRequired.Value = 25000.00; // Decimal
//createJobJSON.jobInitialization.InputVariables.push(varAmountRequired);

createJobJSON.processIdentity.Id = "AF3DAF2F039C40FC9766B8A15C360F5B";
createJobJSON.processIdentity.Name = "RaiseEvent";
//createJobJSON.processIdentity.Version = 0;
createJobJSON.sessionId = "68BB738BCA4E364593A2222D27750691";

$("#Request").html("<BR/><B>Request:</B><BR/>" + JSON.stringify(createJobJSON));

// Calling method CreateJob
$.ajax({
 type: "POST",
 async: "false",
 //url: "http://localhost/TotalAgility/Services/Sdk/JobService.svc/json/CreateJob",
 url: window.location.origin+"/TotalAgility/Services/Sdk/JobService.svc/json/CreateJob",
 data: JSON.stringify(createJobJSON),
 contentType: "application/json; charset=utf-8",
 crossDomain: true,
 dataType: "json",
 success: function (result) {
	 $("#Result").html("<BR/><B>Response: </B><BR/>" + JSON.stringify(result.d));
 },
 error: function (jqXHR, textStatus, errorThrown) {
	 alert("***Error***\n");
 },
 async: false
});
