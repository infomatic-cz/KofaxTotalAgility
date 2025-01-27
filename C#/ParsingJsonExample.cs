using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Agility.Server.Scripting.ScriptAssembly;

namespace MyNamespace
{
    public class ProjectProcessor
    {
        [StartMethodAttribute()]
        public void ProcessProjects(ScriptParameters sp)
        {
            string jsonString = sp.InputVariables["RESPONSE"] as string;    // zadání vstupní proměnné
            var jsonArray = JArray.Parse(jsonString);
            var projects = new List<Project>();

            foreach (var item in jsonArray)
            {
                var project = new Project
                {    // mapování klíčů dle json struktury
                    Id = GetTokenValue(item, "id", "No ID"),
                    Key = GetTokenValue(item, "key", "No Key"),
                    Name = GetTokenValue(item, "name", "No Name"),
                    CategoryName = item.SelectToken("projectCategory") != null
                        ? GetTokenValue(item.SelectToken("projectCategory"), "name", "No Category")
                        : "No Category",
                    ProjectTypeKey = GetTokenValue(item, "projectTypeKey", "No Project Type Key"),
                    Self = GetTokenValue(item, "self", "No Self")
                };

                projects.Add(project);
            }

            sp.OutputVariables["RESULT"] = JsonConvert.SerializeObject(projects);    // uložení do výstupní proměnné
        }

        private string GetTokenValue(JToken token, string path, string defaultValue)
        {
            var valueToken = token.SelectToken(path);
            return valueToken != null ? valueToken.ToString() : defaultValue;
        }

        public class Project
        {
            public string Id { get; set; }
            public string Key { get; set; }
            public string Name { get; set; }
            public string CategoryName { get; set; }
            public string ProjectTypeKey { get; set; }
            public string Self { get; set; }
        }
    }
}
