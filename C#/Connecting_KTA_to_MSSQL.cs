using System;
using System.Data;
using System.Data.SqlClient;
using Agility.Server.Scripting.ScriptAssembly;

namespace MyNamespace
{
    public class Class1
    {
        [StartMethodAttribute()]
        public void InsertOrUpdateEventType(ScriptParameters sp)
        {
            // Načtení hodnot z proměnných procesu
            object filteredResponse = sp.InputVariables["FILTERED_RESPONSE"];
            if (filteredResponse == null || string.IsNullOrEmpty(filteredResponse.ToString()))
            {
                throw new ArgumentException("filtered_response je prázdný nebo null.");
            }

            string jsonData = filteredResponse.ToString();

            // Načtení připojovacího řetězce z InputVariables
            object jiraConn = sp.InputVariables["JIRA_CONN"];
            if (jiraConn == null || string.IsNullOrEmpty(jiraConn.ToString()))
            {
                throw new InvalidOperationException("Vstupní proměnná 'jira_conn' není nastavena nebo je prázdná.");
            }

            string connectionString = jiraConn.ToString();
            bool result = false;

            try
            {
                // Připojení k databázi a volání uložené procedury
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("InsertOrUpdateAT_EventType", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Předání parametrů uložené proceduře
                        command.Parameters.Add(new SqlParameter("@jsonData", SqlDbType.NVarChar) { Value = jsonData });
                        SqlParameter outputParam = new SqlParameter("@result", SqlDbType.Bit)
                        {
                            Direction = ParameterDirection.InputOutput,
                            Value = false
                        };
                        command.Parameters.Add(outputParam);

                        // Spuštění uložené procedury
                        command.ExecuteNonQuery();

                        // Načtení výsledné hodnoty z výstupního parametru
                        result = (bool)outputParam.Value;
                    }
                }

                // Uložení výsledku do výstupní proměnné procesu
                sp.OutputVariables["ODPOVED"] = result;
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Chyba při komunikaci s databází: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Neočekávaná chyba: " + ex.Message, ex);
            }
        }
    }
}