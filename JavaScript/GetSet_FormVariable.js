
// For KTA 7.9 + new JavaScript API is available
// https://docshield.kofax.com/KTA/en_US/7.9.0-ud9cfx6hos/help/Designer/All_Shared/UserInterface/t_actionjavascript.html
Forms.SDK.GetFormVariableValue('CustomerName');
Forms.SDK.UpdateFormVariableValue('CustomerName', 'TestName');


// Get form variable value
var varValue = thisForm.formVariables.getFormVariableByName("formVariableName").getVariableValue();

// Set form variable value
thisForm.formVariables.updateFormVariable("formVariableName", "new value goes here");