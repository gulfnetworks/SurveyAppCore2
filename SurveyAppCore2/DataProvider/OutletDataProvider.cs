using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using SurveyAppCore2.Models;

namespace SurveyAppCore2.DataProvider
{
    public class OutletDataProvider
    {
        public IConfigurationRoot Configuration { get; }
        private SqlConnection sqlConnection;
        private string connectionString;

        public OutletDataProvider()
        {
            string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new String[] { @"bin\" }, StringSplitOptions.None)[0];
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(projectPath)
                .AddJsonFile("appsettings.json")
                .Build();
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }



        public async Task<IEnumerable<Outlet>> GetOutlets()
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                return await sqlConnection.QueryAsync<Outlet>(
                    "SP_GetAllOutlet",
                    null,
                    commandType: CommandType.StoredProcedure);
            }
        }



        public async Task<IEnumerable<Outlet>> GetOutletsBySettingsId(int SettingsId)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                var dynamicParameters = new DynamicParameters();


                dynamicParameters.Add("@SettingsId", SettingsId);
                return await sqlConnection.QueryAsync<Outlet>(
                    "SP_GetOutletsBySettingsId",
                    dynamicParameters,
                    commandType: CommandType.StoredProcedure);
            }
        }
    }
}
