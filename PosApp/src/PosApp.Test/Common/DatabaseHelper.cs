using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace PosApp.Test.Common
{
    static class DatabaseHelper
    {
        public static void ResetDatabase()
        {
            const string cleanupSql =
                "EXEC sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL';" +
                "EXEC sp_MSForEachTable 'IF OBJECT_ID(''?'') NOT IN ( " +
                "ISNULL(OBJECT_ID(''[dbo].[VersionInfo]''), 0)) DELETE FROM ?';" +
                "EXEC sp_MSForEachTable 'ALTER TABLE ? CHECK CONSTRAINT ALL'";

            string connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                ExecuteSql(connection, cleanupSql);
            }
        }

        static void ExecuteSql(SqlConnection connection, string sql)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql;
                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
            }
        }
    }
}