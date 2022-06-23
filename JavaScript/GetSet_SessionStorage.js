// For KTA 7.9 + new JavaScript SDK is available
// https://docshield.kofax.com/KTA/en_US/7.9.0-ud9cfx6hos/help/Designer/All_Shared/UserInterface/t_actionjavascript.html
Forms.SDK.GetGlobalVariableValue('CompanyCode');
Forms.SDK.UpdateGlobalVariableValue('CompanyCode', 'comp123');


// Documentation https://www.w3schools.com/jsref/prop_win_sessionstorage.asp

// Set
sessionStorage.setItem("sessionVariableName", "newValueGoesHere");

// Get
var sessionStorageItem = sessionStorage.getItem("sessionVariableName");

// Delete
sessionStorage.removeItem("sessionVariableName");


// KTA Global variables are saved in session storage as JSONs
// It is possible to get value by parsing JSON (below gets session id)
var sessionStorageItem = JSON.parse(sessionStorage.getItem("SESSION_ID")).value