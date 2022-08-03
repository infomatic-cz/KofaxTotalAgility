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

        /// <summary>
        /// Apply XSLT to xml and return result
        /// </summary>
        /// <param name="sp"></param>
        [StartMethodAttribute()]   
        public void ApplyXSLT(ScriptParameters sp)
        {
            XmlReader xslt = XmlReader.Create(new StringReader(sp.InputVariables["xslt"].ToString()));
            XmlReader xmlSource = XmlReader.Create(new StringReader(sp.InputVariables["sourcexml"].ToString()));

            XmlWriterSettings settings = new XmlWriterSettings();
            //settings.OmitXmlDeclaration = true;
            settings.ConformanceLevel = ConformanceLevel.Auto;

            // Load the style sheet.
            XslCompiledTransform XslTransformation = new XslCompiledTransform();
            XslTransformation.Load(xslt);

            // Execute the transform and output the results to a file.
            string xmlTransformed;
            using (var sw = new StringWriter())
            {
                using (var xw = XmlWriter.Create(sw,settings))
                {
                    // Build Xml with xw.
                    XslTransformation.Transform(xmlSource, xw);
                }
                xmlTransformed = sw.ToString();
            }
            sp.OutputVariables["XmlTransformed"] = xmlTransformed;
        }

    }

}
