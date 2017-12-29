using Microsoft.Extensions.Configuration;
using SurveyAppCore2.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Data;

namespace SurveyAppCore2.DataProvider
{
    public class SubscriptionDataProvider
    {
        public IConfigurationRoot Configuration { get; }
        private SqlConnection sqlConnection;
        private string connectionString;

        public SubscriptionDataProvider()
        {
            string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new String[] { @"bin\" }, StringSplitOptions.None)[0];
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(projectPath)
                .AddJsonFile("appsettings.json")
                .Build();
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }


        public async Task<IEnumerable<Subscription>> GetSubscriptions()
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                return await sqlConnection.QueryAsync<Subscription>(
                    "SP_GetAllSubscription",
                    null,
                    commandType: CommandType.StoredProcedure);
            }
        }
    }
}
