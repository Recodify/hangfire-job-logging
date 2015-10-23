using System;
using System.Data.SqlClient;

namespace hangfire.job.logging
{
    /// <summary>
    /// Simple sql server IJobLogger implementation that will log to the hangfire.state table.
    /// </summary>
    public class SqlServerJobLogger : IJobLogger
    {
        private readonly string connectionString;

        public SqlServerJobLogger(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Write a message to the job log.
        /// </summary>
        /// <param name="jobId">The id of the job that is logging a message.</param>
        /// <param name="message">The message to log.</param>
        public void Write(string jobId, string message)
        {
            var command = BuildCommand(jobId, message);

            ExecuteCommand(command);
        }

        private void ExecuteCommand(SqlCommand command)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    command.Connection = connection;
                    command.ExecuteNonQuery();
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private static SqlCommand BuildCommand(string jobId, string message)
        {
            var date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var data = "{\"Message\":\"" + message + "\"}";

            var command = new SqlCommand("insert into [HangFire].[State] (jobid, name, createdAt, Data) values (@jobId,@name,@createdDate,@data)");

            command.Parameters.AddWithValue("jobId", jobId);
            command.Parameters.AddWithValue("name", "Output");
            command.Parameters.AddWithValue("createdDate", date);
            command.Parameters.AddWithValue("data", data);

            return command;
        }
    }
}