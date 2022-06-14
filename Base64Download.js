// https://stackoverflow.com/questions/14011021/how-to-download-a-base64-encoded-image
// Expecting PDF. Might be enhanced to alse take MIME type and file name from variables
var a = document.createElement("a"); //Create <a>
a.href = "data:application/pdf;base64," + thisForm.formVariables.getFormVariableByName("DocumentInBase64").getVariableValue(); //Image Base64 Goes here
a.download = "File.pdf"; //File name Here
a.click(); //Downloaded file



// v2 WIP
var FileName = thisForm.formVariables.getFormVariableByName("FileName").getVariableValue();
var FileMimeType = thisForm.formVariables.getFormVariableByName("FileMimeType").getVariableValue();
var FileBase64 = thisForm.formVariables.getFormVariableByName("FileBase64").getVariableValue();
var a = document.createElement("a"); //Create <a>
a.href = "data:"+FileMimeType+";base64," + FileBase64; //Image Base64 Goes here
a.download = FileName; //File name Here
a.click(); //Downloaded file

test změna
