using Dapper;
using Microsoft.Extensions.Configuration;
using SurveyAppCore2.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyAppCore2.DataProvider
{
    public class EmailDataProvider
    {
        public IConfigurationRoot Configuration { get; }
        private SqlConnection sqlConnection;
        private string connectionString;

        public EmailDataProvider()
        {
            string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new String[] { @"bin\" }, StringSplitOptions.None)[0];
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(projectPath)
                .AddJsonFile("appsettings.json")
                .Build();
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }


        public async Task<Email> GetThankEmail(int SettingsId)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();

                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@SettingsId", SettingsId);
     
                var emails = await sqlConnection.QueryAsync<Email>(
                         "SP_GetThankyouEmail",
                         dynamicParameters,
                         commandType: CommandType.StoredProcedure);

                return emails.FirstOrDefault();

            }
        }



        public async Task<Email> GetEmailById(int EmailId, int SettingsId)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@EmailId", EmailId);
                dynamicParameters.Add("@SettingsId", SettingsId);
                var emails = await sqlConnection.QueryAsync<Email>(
                    "SP_GetEmailById",
                    dynamicParameters,
                    commandType: CommandType.StoredProcedure);

                return emails.FirstOrDefault();
            
            }
        }


        public async Task<IEnumerable<Email>> GetEmails(int SettingsId)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@SettingsId", SettingsId);
                return await sqlConnection.QueryAsync<Email>(
                    "SP_GetAllEmail",
                    dynamicParameters,
                    commandType: CommandType.StoredProcedure);
            }
        }


        public async Task<int> AddEmail(Email email)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();

                var dynamicParameters = new DynamicParameters();


                dynamicParameters.Add("@From", email.From);
                dynamicParameters.Add("@To", email.To);
                dynamicParameters.Add("@CC", email.CC);
                dynamicParameters.Add("@Folder", email.Folder);
                dynamicParameters.Add("@Subject", email.Subject);
                dynamicParameters.Add("@IsRead", email.IsRead);
                dynamicParameters.Add("@AutoEmail", email.AutoEmail);
                dynamicParameters.Add("@Content", email.Content);
                dynamicParameters.Add("@DateTime", email.DateTime);
                dynamicParameters.Add("@SettingsId", email.SettingsId);


                int returns = await sqlConnection.QuerySingleOrDefaultAsync<int>(
                   "SP_AddEmail",
                   dynamicParameters,
                   commandType: CommandType.StoredProcedure);


                return returns;
            }


        }


        public async Task<int> UpdateEmail(Email email)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();

                var dynamicParameters = new DynamicParameters();

                dynamicParameters.Add("@EmailId", email.EmailId);
                dynamicParameters.Add("@From", email.From);
                dynamicParameters.Add("@To", email.To);
                dynamicParameters.Add("@CC", email.CC);
                dynamicParameters.Add("@Folder", email.Folder);
                dynamicParameters.Add("@Subject", email.Subject);
                dynamicParameters.Add("@IsRead", email.IsRead);
                dynamicParameters.Add("@AutoEmail", email.AutoEmail);
                dynamicParameters.Add("@Content", email.Content);
                dynamicParameters.Add("@DateTime", email.DateTime);
                dynamicParameters.Add("@SettingsId", email.SettingsId);


                await sqlConnection.QuerySingleOrDefaultAsync<int>(
                   "SP_UpdateEmail",
                   dynamicParameters,
                   commandType: CommandType.StoredProcedure);


                return 1;
            }


        }

    }
}
