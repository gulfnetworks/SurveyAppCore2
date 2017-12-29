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
    public class SurveyDataProvider
    {
        public IConfigurationRoot Configuration { get; }
        private SqlConnection sqlConnection;
        private string connectionString;

        public SurveyDataProvider()
        {
            string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new String[] { @"bin\" }, StringSplitOptions.None)[0];
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(projectPath)
                .AddJsonFile("appsettings.json")
                .Build();
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<Survey> GetSurveyById(int id)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@SurveyId", id);
                return await sqlConnection.QuerySingleOrDefaultAsync<Survey>(
                    "SP_GetSurveyById",
                    dynamicParameters,
                    commandType: CommandType.StoredProcedure);
            }
        }


        private DateTime ConvertToTimeZone(DateTime dateTime,string TimeZone)
        {
            var info = TimeZoneInfo.FindSystemTimeZoneById(TimeZone);
            return TimeZoneInfo.ConvertTime(dateTime, info);
        }


        public async Task<int> AddSurvey(Survey survey, string TimeZone)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                var dynamicParameters = new DynamicParameters();


                //dynamicParameters.Add("@SurveyId", survey.SurveyId);
                //dynamicParameters.Add("@QualityRate", survey.QualityRate);
                //dynamicParameters.Add("@QualityComment", survey.QualityComment);
                //dynamicParameters.Add("@ValueRate", survey.ValueRate);
                //dynamicParameters.Add("@ValueComment", survey.ValueComment);
                //dynamicParameters.Add("@ServiceRate", survey.ServiceRate);
                //dynamicParameters.Add("@ServiceComment", survey.ServiceComment);
                //dynamicParameters.Add("@AmbienceRate", survey.AmbienceRate);
                //dynamicParameters.Add("@AmbienceComment", survey.AmbienceComment);
                //dynamicParameters.Add("@RecommendRate", survey.RecommendRate);

                //dynamicParameters.Add("@RecommendPoorArea", survey.RecommendPoorArea);
                //dynamicParameters.Add("@RecommendImprovements", survey.RecommendImprovements);

                //dynamicParameters.Add("@RecommendSuggestions", survey.RecommendSuggestions);

                //dynamicParameters.Add("@LastVisit", survey.LastVisit);
                //dynamicParameters.Add("@LastVisitComment", survey.LastVisitComment);
                //dynamicParameters.Add("@Action", survey.Action);
                //dynamicParameters.Add("@Status", survey.Status);

                DateTime dt = ConvertToTimeZone(DateTime.Now, TimeZone);

                dynamicParameters.Add("@DateTime", dt);
                //dynamicParameters.Add("@DateTime", DateTime.UtcNow);

                dynamicParameters.Add("@Customer", survey.Customer);
                dynamicParameters.Add("@MobileNo", survey.MobileNo);
                dynamicParameters.Add("@Email", survey.Email);
                dynamicParameters.Add("@CheckNo", survey.CheckNo);
                dynamicParameters.Add("@TableNo", survey.TableNo);
                dynamicParameters.Add("@ManagerId", survey.ManagerId);
                dynamicParameters.Add("@StaffId", survey.StaffId);
                dynamicParameters.Add("@OutletId", survey.OutletId);


                return await sqlConnection.QuerySingleOrDefaultAsync<int>(
                      "SP_AddSurvey2",
                      dynamicParameters,
                      commandType: CommandType.StoredProcedure);
            }
        }

        public async Task UpdateSurvey(Survey survey, string TimeZone)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                var dynamicParameters = new DynamicParameters();


                dynamicParameters.Add("@SurveyId", survey.SurveyId);
                dynamicParameters.Add("@QualityRate", survey.QualityRate);
                dynamicParameters.Add("@QualityComment", survey.QualityComment);
                dynamicParameters.Add("@ValueRate", survey.ValueRate);
                dynamicParameters.Add("@ValueComment", survey.ValueComment);
                dynamicParameters.Add("@ServiceRate", survey.ServiceRate);
                dynamicParameters.Add("@ServiceComment", survey.ServiceComment);
                dynamicParameters.Add("@AmbienceRate", survey.AmbienceRate);
                dynamicParameters.Add("@AmbienceComment", survey.AmbienceComment);
                dynamicParameters.Add("@RecommendRate", survey.RecommendRate);
                dynamicParameters.Add("@RecommendPoorArea", survey.RecommendPoorArea);
                dynamicParameters.Add("@RecommendImprovements", survey.RecommendImprovements);
                dynamicParameters.Add("@RecommendSuggestions", survey.RecommendSuggestions);
                dynamicParameters.Add("@LastVisit", survey.LastVisit);
                dynamicParameters.Add("@LastVisitComment", survey.LastVisitComment);
                dynamicParameters.Add("@Action", survey.Action);
                dynamicParameters.Add("@Status", survey.Status);

                DateTime dt = ConvertToTimeZone(DateTime.Now, TimeZone);
                
                dynamicParameters.Add("@DateTime", dt);
                dynamicParameters.Add("@Customer", survey.Customer);
                dynamicParameters.Add("@MobileNo", survey.MobileNo);
                dynamicParameters.Add("@Email", survey.Email);
                dynamicParameters.Add("@CheckNo", survey.CheckNo);
                dynamicParameters.Add("@TableNo", survey.TableNo);
                dynamicParameters.Add("@ManagerId", survey.ManagerId);
                dynamicParameters.Add("@StaffId", survey.StaffId);
                dynamicParameters.Add("@OutletId", survey.OutletId);


                await sqlConnection.ExecuteAsync( "SP_UpdateSurvey",
                       dynamicParameters, commandType: CommandType.StoredProcedure);

            }
        }


        public async Task<IEnumerable<SurveyComplete>> GetAllSurveyComplete(int SettingsId)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@SettingsId", SettingsId);
                dynamic returns = await sqlConnection.QueryAsync<dynamic>(
                     "SP_GetAllSurveyComplete",
                     dynamicParameters,
                     commandType: CommandType.StoredProcedure);

       
                List<SurveyComplete> details = new List<SurveyComplete>();
                foreach (var ret in returns)
                {
                    SurveyComplete surveyComplete = new SurveyComplete();
                    surveyComplete.Action = ret.Action;
                    surveyComplete.MobileNo = ret.MobileNo;
                    surveyComplete.QualityRate = ret.QualityRate;
                    surveyComplete.QualityComment = ret.QualityComment;
                    surveyComplete.ValueRate = ret.ValueRate;
                    surveyComplete.ValueComment = ret.ValueComment;
                    surveyComplete.ServiceRate = ret.ServiceRate;
                    surveyComplete.ServiceComment = ret.ServiceComment;
                    surveyComplete.AmbienceRate = ret.AmbienceRate;
                    surveyComplete.AmbienceComment = ret.AmbienceComment;
                    surveyComplete.RecommendRate = ret.RecommendRate;
                    surveyComplete.RecommendPoorArea = ret.RecommendPoorArea;
                    surveyComplete.RecommendImprovements = ret.RecommendImprovements;
                    surveyComplete.RecommendSuggestions = ret.RecommendSuggestions;

                    surveyComplete.LastVisit = ret.LastVisit;
                    surveyComplete.LastVisitComment = ret.LastVisitComment;
                    surveyComplete.Action = ret.Action;
                    surveyComplete.Status = ret.Status;
                    surveyComplete.DateTime = ret.DateTime;
                    surveyComplete.Customer = ret.Customer;
                    surveyComplete.MobileNo = ret.MobileNo;
                    surveyComplete.Email = ret.Email;
                    surveyComplete.CheckNo = ret.CheckNo;
                    surveyComplete.TableNo = ret.TableNo;
                    surveyComplete.ManagerId = ret.ManagerId;
                    surveyComplete.StaffId = ret.StaffId;
                    surveyComplete.OutletId = ret.OutletId;

                    surveyComplete.OutletCountry = ret.OutletCountry;
                    surveyComplete.OutletName = ret.OutletName;
                    surveyComplete.Staff = ret.Staff;
                    surveyComplete.Manager = ret.Manager;

                    details.Add(surveyComplete);
                }


                return details.AsEnumerable();

            }



        }


    


        public async Task<SurveyComplete> GetSurveyCompleteById(int SurveyId)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();

                var dynamicParameters = new DynamicParameters();


                dynamicParameters.Add("@SurveyId", SurveyId);


                dynamic  list  = await sqlConnection.QueryAsync<dynamic>(
                   "SP_GetSurveyCompleteById",
                   dynamicParameters,
                   commandType: CommandType.StoredProcedure);
                foreach (var ret in list)
                {

                    SurveyComplete surveyComplete = new SurveyComplete();
                    surveyComplete.Action = ret.Action;
                    surveyComplete.MobileNo = ret.MobileNo;
                    surveyComplete.QualityRate = ret.QualityRate;
                    surveyComplete.QualityComment = ret.QualityComment;
                    surveyComplete.ValueRate = ret.ValueRate;
                    surveyComplete.ValueComment = ret.ValueComment;
                    surveyComplete.ServiceRate = ret.ServiceRate;
                    surveyComplete.ServiceComment = ret.ServiceComment;
                    surveyComplete.AmbienceRate = ret.AmbienceRate;
                    surveyComplete.AmbienceComment = ret.AmbienceComment;
                    surveyComplete.RecommendRate = ret.RecommendRate;
                    surveyComplete.RecommendPoorArea = ret.RecommendPoorArea;
                    surveyComplete.RecommendImprovements = ret.RecommendImprovements;
                    surveyComplete.RecommendSuggestions = ret.RecommendSuggestions;

                    surveyComplete.LastVisit = ret.LastVisit;
                    surveyComplete.LastVisitComment = ret.LastVisitComment;
                    surveyComplete.Action = ret.Action;
                    surveyComplete.Status = ret.Status;
                    surveyComplete.DateTime = ret.DateTime;
                    surveyComplete.Customer = ret.Customer;
                    surveyComplete.MobileNo = ret.MobileNo;
                    surveyComplete.Email = ret.Email;
                    surveyComplete.CheckNo = ret.CheckNo;
                    surveyComplete.TableNo = ret.TableNo;
                    surveyComplete.ManagerId = ret.ManagerId;
                    surveyComplete.StaffId = ret.StaffId;
                    surveyComplete.OutletId = ret.OutletId;

                    surveyComplete.OutletCountry = ret.OutletCountry;
                    surveyComplete.OutletName = ret.OutletName;
                    surveyComplete.Staff = ret.Staff;
                    surveyComplete.Manager = ret.Manager;

                    surveyComplete.Complaint = ret.Complaint;
                    surveyComplete.Feedback = ret.Feedback;
                    surveyComplete.Compliment = ret.Compliment;

                    surveyComplete.UTCDate = ret.DateTime.ToString("yyyy-MM-dd");
                    surveyComplete.UTCTime = ret.DateTime.ToString("HH:mm:ss");

                    return surveyComplete;
                }


                return null;

           
            }



        }
    }
}
