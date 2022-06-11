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
