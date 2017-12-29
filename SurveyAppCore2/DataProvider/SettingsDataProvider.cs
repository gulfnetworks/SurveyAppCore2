using Dapper;
using Microsoft.Extensions.Configuration;
using SurveyAppCore2.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyAppCore2.DataProvider
{
    public class SettingsDataProvider
    {
        public IConfigurationRoot Configuration { get; }
        private SqlConnection sqlConnection;
        private string connectionString;

        public SettingsDataProvider()
        {
            string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new String[] { @"bin\" }, StringSplitOptions.None)[0];
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(projectPath)
                .AddJsonFile("appsettings.json")
                .Build();
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<Settings> GetSettingsByCompanyCode(string code)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();

                var dynamicParameters = new DynamicParameters();

                dynamicParameters.Add("@CompanyCode", code);


                dynamic returns = await sqlConnection.QueryAsync<dynamic>(
                   "SELECT * FROM [dbo].[Settings] WHERE [CompanyCode] = @CompanyCode",
                   dynamicParameters,
                   commandType: CommandType.Text);


                Settings SettingsDetails = new Settings();
                foreach (var ret in returns)
                {
                    SettingsDetails.SettingsId = ret.SettingsId;
                    SettingsDetails.TimeZone = ret.TimeZone;
                    SettingsDetails.CompanyName = ret.CompanyName;
                    SettingsDetails.CompanyAddress = ret.CompanyAddress;
                    SettingsDetails.CompanyContact = ret.CompanyContact;
                    SettingsDetails.CompanyLogoUrl = ret.CompanyLogoUrl;
                    SettingsDetails.Country = ret.Country;
                    SettingsDetails.CompanyCode = ret.CompanyCode;

            


                    SettingsDetails.EndTime = (ret.EndTime != null ? ret.EndTime.ToString() : "");
                    SettingsDetails.StartTime = (ret.StartTime != null ? ret.StartTime.ToString() : "");
                    SettingsDetails.ExpiryDate = (ret.ExpiryDate != null ? ret.ExpiryDate.ToString() : "");
                }

                return SettingsDetails;
            }


        }

        public async Task<Settings> GetSettings(int id)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();

                var dynamicParameters = new DynamicParameters();

                dynamicParameters.Add("@SettingsId", id);


                dynamic returns = await sqlConnection.QueryAsync<dynamic>(
                   "SELECT * FROM [dbo].[Settings] WHERE [SettingsId] = @SettingsId",
                   dynamicParameters,
                   commandType: CommandType.Text);


                Settings SettingsDetails = new Settings();
                foreach (var ret in returns)
                {
                    SettingsDetails.SettingsId = ret.SettingsId;
                    SettingsDetails.TimeZone = ret.TimeZone;
                    SettingsDetails.CompanyName = ret.CompanyName;
                    SettingsDetails.CompanyAddress = ret.CompanyAddress;
                    SettingsDetails.CompanyContact = ret.CompanyContact;
                    SettingsDetails.CompanyLogoUrl = ret.CompanyLogoUrl;
                    SettingsDetails.Country = ret.Country;
                    SettingsDetails.CompanyCode = ret.CompanyCode;



                    SettingsDetails.EndTime = (ret.EndTime != null ? ret.EndTime.ToString() : "");
                    SettingsDetails.StartTime =  (ret.StartTime != null ? ret.StartTime.ToString() : "");

                    SettingsDetails.ExpiryDate = (ret.ExpiryDate != null ? ret.ExpiryDate.ToString() : "");

                }

                return SettingsDetails;
            }


        }

        public async Task<int> UpdateSettings(Settings settings)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();

                var dynamicParameters = new DynamicParameters();

                dynamicParameters.Add("@SettingsId", settings.SettingsId);
                dynamicParameters.Add("@CompanyAddress", settings.CompanyAddress);
                dynamicParameters.Add("@CompanyContact", settings.CompanyContact);
                dynamicParameters.Add("@CompanyLogoUrl", settings.CompanyLogoUrl);
                dynamicParameters.Add("@CompanyName", settings.CompanyName);
                dynamicParameters.Add("@Country", settings.Country);
                dynamicParameters.Add("@TimeZone", settings.TimeZone);
                dynamicParameters.Add("@CompanyCode", settings.CompanyCode);
                dynamicParameters.Add("@StartTime", settings.StartTime);
                dynamicParameters.Add("@EndTime", settings.EndTime);
                dynamicParameters.Add("@ExpiryDate", settings.ExpiryDate);
                int returns = await sqlConnection.QuerySingleOrDefaultAsync<int>(
                   "SP_UpdateSettings",
                   dynamicParameters,
                   commandType: CommandType.StoredProcedure);


                return returns;
            }


        }



        public async Task<int> AddSettings(Settings settings)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();

                var dynamicParameters = new DynamicParameters();

                dynamicParameters.Add("@SettingsId", settings.SettingsId);
                dynamicParameters.Add("@CompanyAddress", settings.CompanyAddress);
                dynamicParameters.Add("@CompanyContact", settings.CompanyContact);
                dynamicParameters.Add("@CompanyLogoUrl", settings.CompanyLogoUrl);
                dynamicParameters.Add("@CompanyName", settings.CompanyName);
                dynamicParameters.Add("@Country", settings.Country);
                dynamicParameters.Add("@TimeZone", settings.TimeZone);
                dynamicParameters.Add("@CompanyCode", settings.CompanyCode);
                dynamicParameters.Add("@StartTime", settings.StartTime);
                dynamicParameters.Add("@EndTime", settings.EndTime);
                dynamicParameters.Add("@ExpiryDate", settings.ExpiryDate);
                int returns = await sqlConnection.QuerySingleOrDefaultAsync<int>(
                   "SP_AddSettings",
                   dynamicParameters,
                   commandType: CommandType.StoredProcedure);


                return returns;
            }


        }


        public async Task<IEnumerable<Settings>> GetAllCompany()
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();

                dynamic returns = await sqlConnection.QueryAsync<dynamic>(
                   "SP_GetAllCompany",
                   null,
                   commandType: CommandType.Text);


                List<Settings> tmpList = new List<Settings>();

          
                foreach (var ret in returns)
                {
                    Settings SettingsDetails = new Settings();
                    SettingsDetails.SettingsId = ret.SettingsId;
                    SettingsDetails.TimeZone = ret.TimeZone;
                    SettingsDetails.CompanyName = ret.CompanyName;
                    SettingsDetails.CompanyAddress = ret.CompanyAddress;
                    SettingsDetails.CompanyContact = ret.CompanyContact;
                    SettingsDetails.CompanyLogoUrl = ret.CompanyLogoUrl;
                    SettingsDetails.Country = ret.Country;
                    SettingsDetails.CompanyCode = ret.CompanyCode;



                    SettingsDetails.EndTime = (ret.EndTime != null ? ret.EndTime.ToString() : "");
                    SettingsDetails.StartTime = (ret.StartTime != null ? ret.StartTime.ToString() : "");
                    SettingsDetails.ExpiryDate = (ret.ExpiryDate != null ? ret.ExpiryDate.ToString() : "");

                    tmpList.Add(SettingsDetails);

                }

                return tmpList.AsEnumerable();
            }


        }
    }
}
