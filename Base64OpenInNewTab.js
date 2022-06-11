// https://stackoverflow.com/questions/2805330/opening-pdf-string-in-new-window-with-javascript

var FileName = thisForm.formVariables.getFormVariableByName("FileName").getVariableValue();
var FileMimeType = thisForm.formVariables.getFormVariableByName("FileMimeType").getVariableValue();
var FileBase64 = thisForm.formVariables.getFormVariableByName("FileBase64").getVariableValue();

let documentWindow = window.open("")

documentWindow.document.write(
   "<iframe width='100%' height='100%' border='none' overflow='hidden' frameborder='0' margin='0' padding='0' "
   +"position='fixed' left'0' right'0' bottom'0' top'0px' src='data:"+FileMimeType+";base64, " + encodeURI(FileBase64) + "'></iframe>")

setTimeout(function(){ documentWindow.stop() }, 1000);
