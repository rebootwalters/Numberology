using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace Numberology
{
    public class Logger
    {
        static bool simulateharderror = false;
        static string connectionstring;
         static Logger()
            {
            connectionstring = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString;
            }
        public static void Log(Exception ex)
        {
            try
            {
                if (simulateharderror)
                {
                    throw new Exception("simulated hard error");
                }
                using (SqlConnection con = new SqlConnection(connectionstring))
                {
                    con.Open();
                    using (var com = con.CreateCommand())
                    {
                 com.CommandText = "InsertLogItem";
                 com.CommandType = System.Data.CommandType.StoredProcedure;
                 com.Parameters.AddWithValue("@message", ex.Message);
                 com.Parameters.AddWithValue("@stacktrace", ex.StackTrace.ToString());
                 com.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception exc)
            {
               var p = HttpContext.Current.Server.MapPath("~");
                p += @"ErrorLog.Log";
               System.IO.File.AppendAllText(p , exc.ToString());
               
            }

        }
    }
}