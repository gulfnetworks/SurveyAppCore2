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
    public class ResolutionLogDataProvider
    {
        public IConfigurationRoot Configuration { get; }
        private SqlConnection sqlConnection;
        private string connectionString;

        public ResolutionLogDataProvider()
        {
            string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new String[] { @"bin\" }, StringSplitOptions.None)[0];
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(projectPath)
                .AddJsonFile("appsettings.json")
                .Build();
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> AddResolutionLog(ResolutionLog resolutionLog)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                var dynamicParameters = new DynamicParameters();


                dynamicParameters.Add("@SurveyId", resolutionLog.SurveyId);
                dynamicParameters.Add("@DateTime", DateTime.UtcNow);
                dynamicParameters.Add("@UpdaterId", resolutionLog.UpdaterId);
                dynamicParameters.Add("@ResolutionDetails", resolutionLog.ResolutionDetails);
                dynamicParameters.Add("@Status", resolutionLog.Status);

                return await sqlConnection.QuerySingleOrDefaultAsync<int>(
                      "SP_AddResolutionLog",
                      dynamicParameters,
                      commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> GetResolutionLogLastInsertedId()
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
    

                return await sqlConnection.QuerySingleOrDefaultAsync<int>(
                      "SELECT IIF(max([ResolutionLogId]) IS NULL,0,max([ResolutionLogId])) AS ID FROM [dbo].[ResolutionLog]",
                      null,
                      commandType: CommandType.Text);
            }
        }


        public async Task<int> UpdateSurveyStatusById(int Id , string Status)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                var dynamicParameters = new DynamicParameters();


                dynamicParameters.Add("@SurveyId", Id);
                dynamicParameters.Add("@Status", Status);

                return await sqlConnection.QuerySingleOrDefaultAsync<int>(
                      "UPDATE [dbo].[Survey] SET Status = @Status WHERE SurveyId = @SurveyId;",
                      dynamicParameters,
                      commandType: CommandType.Text);
            }
        }

        public async Task<IEnumerable<ResolutionLog>> GetResolutionLogBySurveyId(int SurveyId)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();

                var dynamicParameters = new DynamicParameters();
        
       
                dynamicParameters.Add("@SurveyId", SurveyId);



                dynamic returns = await sqlConnection.QueryAsync<dynamic>(
                   "SP_GetResolutionLogBySurveyId",
                   dynamicParameters,
                   commandType: CommandType.StoredProcedure);

                if (returns.Count == 0) return null;

                    List<ResolutionLog> details = new List<ResolutionLog>();
                foreach (var ret in returns)
                {
                    ResolutionLog resolutionLog = new ResolutionLog();


                    resolutionLog.UTCDate = ret.DateTime.ToString("yyyy-MM-dd");
                    resolutionLog.UTCTime = ret.DateTime.ToString("HH:mm:ss");

                    resolutionLog.Status = ret.Status;
                    resolutionLog.OutletCountry = ret.OutletCountry;
                    resolutionLog.OutletName = ret.OutletName;

                    resolutionLog.UpdaterName = ret.Updater;
                    resolutionLog.UpdaterId = ret.UpdaterId;

                    resolutionLog.SurveyId = ret.SurveyId;
                    resolutionLog.ResolutionDetails = ret.ResolutionDetails;



                    details.Add(resolutionLog);
          

                }



                return details.AsEnumerable();

            }



        }




    }




}
