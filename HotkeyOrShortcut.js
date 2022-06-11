// https://stackoverflow.com/questions/23075203/javascript-how-to-listen-keyboard-shortcut>
window.onkeyup= function(e){
    varpressed = "";
    if(e.shiftKey){
        pressed += " + Shift";
    }elseif(e.ctrlKey){
        pressed += " + Ctrl";
    } //and so onpressed += e.keyCode;
    console.log(pressed);
  // do some stuff, call function etc.
}
