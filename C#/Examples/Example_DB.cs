using System;
// Common .Net features
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
// KTA 
using TotalAgility.Sdk;
using Agility.Sdk.Model;
using Agility.Server.Scripting.ScriptAssembly;
// Database
using System.Data;
using System.Data.SqlClient;


namespace MyNamespace
{
    public class Class_DB
    {
        public Class_DB()
        {
        }

        [StartMethodAttribute()]
        public void Method1(ScriptParameters sp)
        {
            // Read input variables
            string ConnectionString = sp.InputVariables["InputProcessVariableName"].ToString();
            int CommandTimeout = (int)sp.InputVariables["InputProcessVariableName"].ToString();
            string ProcedureName = sp.InputVariables["InputProcessVariableName"].ToString();
            string JobId = sp.InputVariables["InputProcessVariableName"].ToString();
            

            // DB spuštění procedury
            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand())
                {
                    Command.Connection = Connection;
                    Command.CommandTimeout = 600;
                    Command.CommandText = ProcedureName;
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@JobId", JobId);   // naplnění vstupního parametru, pozor na datové typy
                    //Command.Parameters.AddWithValue("@Value", "8888888");

                    Command.Parameters["@ThreadId"].Direction = ParameterDirection.Output;    // nastavení proměnné jako výstupní

                    Connection.Open();
                    Command.ExecuteNonQuery();

                    // uložení output proměnné z procky do proměnné v KTA, pozor na datové typy
                    // sp.OutputVariables["ThreadId"] = Convert.ToInt32(Command.Parameters["@ThreadId"].Value);
                }
            }



            
            // DB počet záznamů
            //string ConnectionString = sp.InputVariables["DB_Conn_Migration"].ToString();
            //string JobId = sp.InputVariables["SPP_ID"].ToString();
            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand())
                {
                    Command.Connection = Connection;
                    Command.CommandTimeout = 600;
                    Command.CommandText = "SELECT COUNT(*) FROM[MIGRATION_THREAD] WHERE(( [JOB_ID] = " + JobId + ") AND( [FINISHED] = 0))";
                    Command.CommandType = CommandType.Text;

                    Connection.Open();
                    Int32 count = (Int32)Command.ExecuteScalar();

                    // uložení output proměnné z procky do proměnné v KTA, pozor na datové typy
                    sp.OutputVariables["UnfinishedThreadCount"] = count;
                }
            }
            
            
            // Insert do tabulky
            // Kód je dost dlouhý, nechám tu jen odkaz
            //https://github.com/infomatic-cz/IM-LetistePraha/blob/main/CHR-EmailImport/InserEmailToDB.cs
            
            
            
            // Vrácení záznamů z DB, vykopírováno z https://github.com/infomatic-cz/IM-LetistePraha/blob/main/CHR-EmailImport/EmailFiletoFS.cs
            // Prepare query
            log.AppendLog("Preparing query");
            string commandText = @"
            SELECT *
            FROM [dbo].[EmailStorage]
            WHERE [Id] = @Id";

            // Get row from DB
            log.AppendLog("Preparing connection");
            DataSet dataSet = new DataSet();

            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                log.AppendLog("Preparing command");
                using (SqlCommand Command = new SqlCommand())
                {
                    Command.Connection = Connection;
                    Command.CommandTimeout = 600;
                    Command.CommandText = commandText;
                    Command.CommandType = CommandType.Text;

                    Command.Parameters.AddWithValue("@Id", ArchiveId);

                    SqlDataAdapter adapter = new SqlDataAdapter(Command);

                    log.AppendLog("Executing query, result to dataset");
                    adapter.Fill(dataSet);

                }
            }

            // Validate dataset (result from DB query)
            log.AppendLog("Validating dataset");
            if (dataSet.Tables.Count != 1)
            {
                throw new Exception("Query returned "+dataSet.Tables.Count+" tables. Expected count is 1");
            }
            if (dataSet.Tables[0].Rows.Count != 1)
            {
                throw new Exception("Query returned "+dataSet.Tables[0].Rows.Count+" rows. Expected count is 1");
            }
            if (dataSet.Tables[0].Rows[0].Table.Columns.Contains("FileName") == false)
            {
                throw new Exception("Dataset does not contain column FileName");
            }
            if (dataSet.Tables[0].Rows[0].Table.Columns.Contains("FileBase64") == false)
            {
                throw new Exception("Dataset does not contain column FileBase64");
            }

            // If code gets here, all validations passed
            log.AppendLog("Saving data from dataset to variables");
            string fileName = dataSet.Tables[0].Rows[0]["FileName"].ToString();
            string fileBase64 = dataSet.Tables[0].Rows[0]["FileBase64"].ToString();
            
            
            
        }
    }
}
