// JSON Intro documentation https://www.w3schools.com/js/js_json.asp


// Serialize object to JSON
// https://www.w3schools.com/js/js_json_stringify.asp
// https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/JSON/stringify
JSON.stringify(obj);


// Deserialize JSON to object
// https://www.w3schools.com/js/js_json_parse.asp
JSON.parse('{"name": "", "skills": "", "jobtitel": "Entwickler", "res_linkedin": "GwebSearch"}');
JSON.parse(stringVariableWithJSON);


// Create object and serialize to JSON
// https://stackoverflow.com/questions/8963693/how-to-create-json-string-in-javascript
var obj = newObject();
  obj.name= "Raj";
  obj.age= 32;
  obj.married= false;
varjsonString= JSON.stringify(obj);


// Remove property from JSON
// https://www.w3schools.com/howto/howto_js_remove_property_object.asp
var person = {
  firstName:"John",
  lastName:"Doe",
  age:50,
  eyeColor:"blue"
};
delete person.age;  // or delete person["age"];


// Exclude properties from serialization (stringify) by passing list of allowed properties
// Function can also be used instead of 
var obj = newObject();
  obj.name= "Raj";
  obj.age= 32;
  obj.married= false;
varjsonString= JSON.stringify(obj,['name']); // outputs '{"name":"Raj"}'
