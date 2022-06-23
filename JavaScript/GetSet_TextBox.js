// For KTA 7.9 + new JavaScript SDK is available
// https://docshield.kofax.com/KTA/en_US/7.9.0-ud9cfx6hos/help/Designer/All_Shared/UserInterface/t_actionjavascript.html
Forms.SDK.UpdateControlValue('textbox1.Label', 'test');
​Forms.SDK.GetControlValue('textbox1.Label');


// Using KTA API
// don't set the value directly - use the appropriate method!

// Create textbox object
var field = thisForm.controlManager.getControlByName("textboxNmae");

// Set textbox value
field.setValue("newValuesGoesHere");

// Get textbox value
var value = field.value;



// Quick and dirty, pure JavaScript

// Set textbox value
document.getElementsByName('textboxNmae')[0].value = 'newValuesGoesHere'

// Get textbox value
document.getElementsByName('textboxNmae')[0].value


