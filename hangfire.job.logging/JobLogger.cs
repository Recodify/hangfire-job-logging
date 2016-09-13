using System;
using System.Data.SqlClient;

namespace hangfire.job.logging
{
    public class JobLogger : IJobLogger
    {
        private readonly string connectionString;

        public JobLogger(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Write(string jobId, string message)
        {
            var command = BuildCommand(jobId, message);

            ExecuteCommand(command);
        }

        public void Write(string message)
        {
            var jobId = JobContext.JobId;

            if (string.IsNullOrEmpty(jobId))
            {
                return;
            }

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