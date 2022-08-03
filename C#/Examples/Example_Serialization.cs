using System;
// Common .Net features
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Collections;
using System.Xml.XPath;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Runtime.Serialization.Json;
// KTA 
using TotalAgility.Sdk;
using Agility.Sdk.Model;
using Agility.Server.Scripting.ScriptAssembly;
// Database
using System.Data;
using System.Data.SqlClient;
 
namespace MyNamespace
{
    
    public class Class1
    {
    public Class1() 
    {
    }

        [StartMethodAttribute()]   
        public void Method1(ScriptParameters sp)
        {
            
        }

        public string ObjectToJson(object o)
        {
            DataContractJsonSerializer js = new DataContractJsonSerializer(o.GetType(),
                new DataContractJsonSerializerSettings
                {
                    DateTimeFormat = new DateTimeFormat("yyyy-MM-dd'T'HH:mm:ssZ")
                }); 
            MemoryStream msObj = new MemoryStream();
            js.WriteObject(msObj, folder);
            msObj.Position = 0;
            StreamReader sr = new StreamReader(msObj);

            string json = sr.ReadToEnd();

            sr.Close();
            msObj.Close();

            return json;
        }


        public string ObjectToXml(object o)
        {
            XmlSerializer xmlSerializer = null;

            xmlSerializer = new XmlSerializer(o.GetType());
            using (var sw = new StringWriter())
            {
                xmlSerializer.Serialize(sw, folder);
                string xml = sw.ToString();
            }

            return xml;
        }

    }

}
