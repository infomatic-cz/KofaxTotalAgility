// Simpliest way to execute other actions from JavaScript I have found is to create hidden button with 
//  actions we want to run and click the button from Javascript using following code.
// Code expects button to always exist and might not work when used in popup window.
document.getElementsByName('buttonName')[0].click()