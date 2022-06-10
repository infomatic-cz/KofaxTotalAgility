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

