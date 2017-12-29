using Microsoft.Extensions.Configuration;
using SurveyAppCore2.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Globalization;

namespace SurveyAppCore2.DataProvider
{


    public class UserDetailDataProvider
    {

        public IConfigurationRoot Configuration { get; }
        private SqlConnection sqlConnection;
        private string connectionString;

        public UserDetailDataProvider()
        {
            string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new String[] { @"bin\" }, StringSplitOptions.None)[0];
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(projectPath)
                .AddJsonFile("appsettings.json")
                .Build();
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }


        public async Task AddUserDetail(UserDetail userDetail)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                var dynamicParameters = new DynamicParameters();


                dynamicParameters.Add("@UserFirstName", userDetail.UserFirstName);
                dynamicParameters.Add("@UserLastName", userDetail.UserLastName);
                dynamicParameters.Add("@MobileNo", userDetail.MobileNo);
                dynamicParameters.Add("@UserEmail", userDetail.UserEmail);
                dynamicParameters.Add("@JobTitleId", userDetail.JobTitleId);
                dynamicParameters.Add("@ManagerId", userDetail.ManagerId);
                dynamicParameters.Add("@OutletId", userDetail.OutletId);
                dynamicParameters.Add("@ExtraOutletId", userDetail.ExtraOutletId);
                dynamicParameters.Add("@SubscriptionId", userDetail.SubscriptionId);
                dynamicParameters.Add("@Active", userDetail.Active);
                dynamicParameters.Add("@SettingsId", userDetail.SettingsId);

                await sqlConnection.ExecuteAsync(
                    "SP_AddUserDetail",
                    dynamicParameters,
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task DeleteUserDetail(int id)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@UserDetailId", id);
                await sqlConnection.ExecuteAsync(
                    "SP_DeleteUserDetail",
                    dynamicParameters,
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<UserDetail> GetUserDetailById(int id)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@UserDetailId", id);
                return await sqlConnection.QuerySingleOrDefaultAsync<UserDetail>(
                    "SP_GetUserDetailById",
                    dynamicParameters,
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<UserDetail> GetUserDetailByEmail(string email)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@UserEmail", email);
                return await sqlConnection.QuerySingleOrDefaultAsync<UserDetail>(
                    "SP_GetUserDetailByEmail",
                    dynamicParameters,
                    commandType: CommandType.StoredProcedure);
            }
        }



        public async Task<IEnumerable<UserDetail>> GetUserDetails(int SettingsId)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@SettingsId", SettingsId);
                return await sqlConnection.QueryAsync<UserDetail>(
                    "SP_GetAllUserDetail",
                    dynamicParameters,
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task UpdateUserDetail(UserDetail userDetail)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                var dynamicParameters = new DynamicParameters();




                dynamicParameters.Add("@UserDetailId", userDetail.UserDetailId);

                dynamicParameters.Add("@UserFirstName", userDetail.UserFirstName);
                dynamicParameters.Add("@UserLastName", userDetail.UserLastName);
                dynamicParameters.Add("@MobileNo", userDetail.MobileNo);
                dynamicParameters.Add("@UserEmail", userDetail.UserEmail);
                dynamicParameters.Add("@JobTitleId", userDetail.JobTitleId);
                dynamicParameters.Add("@ManagerId", userDetail.ManagerId);
                dynamicParameters.Add("@OutletId", userDetail.OutletId);
                dynamicParameters.Add("@ExtraOutletId", userDetail.ExtraOutletId);
                dynamicParameters.Add("@SubscriptionId", userDetail.SubscriptionId);
                dynamicParameters.Add("@Active", userDetail.Active);
                dynamicParameters.Add("@SettingsId", userDetail.SettingsId);
                

                await sqlConnection.ExecuteAsync(
                    "SP_UpdateUserDetail",
                    dynamicParameters,
                    commandType: CommandType.StoredProcedure);
            }
        }



        public async Task<UserDetailComplete> GetUserDetailCompleteById(int id)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();

                var dynamicParameters = new DynamicParameters();

                dynamicParameters.Add("@UserDetailId", id);


                dynamic returns = await sqlConnection.QueryAsync<dynamic>(
                     "SP_GetUserDetailCompleteById",
                     dynamicParameters,
                     commandType: CommandType.StoredProcedure);

                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                UserDetailComplete userDetailComplete = new UserDetailComplete();

                foreach (var ret in returns)
                {
                

                    userDetailComplete.Active = ret.Active;
                    userDetailComplete.UserDetailId = ret.UserDetailId;
                    userDetailComplete.UserFirstName = ret.UserFirstName != null ? ret.UserFirstName : "";
                    userDetailComplete.UserLastName = ret.UserLastName != null ? ret.UserLastName : "";

                    userDetailComplete.UserFirstName = textInfo.ToTitleCase(Convert.ToString(userDetailComplete.UserFirstName));
                    userDetailComplete.UserLastName = textInfo.ToTitleCase(Convert.ToString(userDetailComplete.UserLastName));



                    userDetailComplete.MobileNo = ret.MobileNo;
                    userDetailComplete.UserEmail = ret.UserEmail;
                    userDetailComplete.JobTitleId = ret.JobTitleId;
                    userDetailComplete.ManagerId = ret.ManagerId;
                    userDetailComplete.OutletId = ret.OutletId;
                    userDetailComplete.ExtraOutletId = ret.ExtraOutletId;
                    userDetailComplete.SubscriptionId = ret.SubscriptionId;
                    userDetailComplete.Active = ret.Active;

                    userDetailComplete.JobTitleDesc = ret.JobTitleDesc;
                    userDetailComplete.UserTypeId = ret.UserTypeId;

                    userDetailComplete.OutletName = ret.OutletName;
                    userDetailComplete.OutletAddress = ret.OutletAddress;
                    userDetailComplete.OutletCountry = ret.OutletCountry;


                    userDetailComplete.SubscriptionName = ret.SubscriptionName;
                    userDetailComplete.SubscriptionType = ret.SubscriptionType;
                    var price = Convert.ToString(ret.SubscriptionPrice);
                    userDetailComplete.SubscriptionPrice = double.Parse(price);

                    return (userDetailComplete);
                }


                return null;

            }



        }





        public async Task<IEnumerable<UserDetailComplete>> GetUserDetailCompleteByManagerId(int ManagerId)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();

                var dynamicParameters = new DynamicParameters();

                dynamicParameters.Add("@ManagerId", ManagerId);

                dynamic returns = await sqlConnection.QueryAsync<dynamic>(
                     "SP_GetUserDetailCompleteByManagerId",
                     dynamicParameters,
                     commandType: CommandType.StoredProcedure);

                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;


                List<UserDetailComplete> details = new List<UserDetailComplete>();
                foreach (var ret in returns)
                {
                    UserDetailComplete userDetailComplete = new UserDetailComplete();

                    userDetailComplete.Active = ret.Active;
                    userDetailComplete.UserDetailId = ret.UserDetailId;
                    userDetailComplete.UserFirstName = ret.UserFirstName != null ? ret.UserFirstName : "";
                    userDetailComplete.UserLastName = ret.UserLastName != null ? ret.UserLastName : "";

                    userDetailComplete.UserFirstName = textInfo.ToTitleCase(Convert.ToString(userDetailComplete.UserFirstName));
                    userDetailComplete.UserLastName = textInfo.ToTitleCase(Convert.ToString(userDetailComplete.UserLastName));



                    userDetailComplete.MobileNo = ret.MobileNo;
                    userDetailComplete.UserEmail = ret.UserEmail;
                    userDetailComplete.JobTitleId = ret.JobTitleId;
                    userDetailComplete.ManagerId = ret.ManagerId;
                    userDetailComplete.OutletId = ret.OutletId;
                    userDetailComplete.ExtraOutletId = ret.ExtraOutletId;
                    userDetailComplete.SubscriptionId = ret.SubscriptionId;
                    userDetailComplete.Active = ret.Active;

                    userDetailComplete.JobTitleDesc = ret.JobTitleDesc;
                    userDetailComplete.UserTypeId = ret.UserTypeId;

                    userDetailComplete.OutletName = ret.OutletName;
                    userDetailComplete.OutletAddress = ret.OutletAddress;
                    userDetailComplete.OutletCountry = ret.OutletCountry;


                    userDetailComplete.SubscriptionName = ret.SubscriptionName;
                    userDetailComplete.SubscriptionType = ret.SubscriptionType;
                    var price = Convert.ToString(ret.SubscriptionPrice);
                    userDetailComplete.SubscriptionPrice = double.Parse(price);

                    details.Add(userDetailComplete);
                }


                return details.AsEnumerable();

            }



        }



        public async Task<IEnumerable<UserDetailComplete>> GetUserDetailComplete(int SettingsId)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();

                var dynamicParameters = new DynamicParameters();

                dynamicParameters.Add("@SettingsId", SettingsId);
                dynamic returns = await sqlConnection.QueryAsync<dynamic>(
                     "SP_GetAllUserDetailComplete",
                     dynamicParameters,
                     commandType: CommandType.StoredProcedure);

                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;


                List<UserDetailComplete> details = new List<UserDetailComplete>();
                foreach (var ret in returns)
                {
                    UserDetailComplete userDetailComplete = new UserDetailComplete();

                    userDetailComplete.Active = ret.Active;
                    userDetailComplete.UserDetailId = ret.UserDetailId;
                    userDetailComplete.UserFirstName = ret.UserFirstName != null ? ret.UserFirstName : "";
                    userDetailComplete.UserLastName = ret.UserLastName != null ? ret.UserLastName : "";

                    userDetailComplete.UserFirstName = textInfo.ToTitleCase(Convert.ToString(userDetailComplete.UserFirstName));
                    userDetailComplete.UserLastName = textInfo.ToTitleCase(Convert.ToString(userDetailComplete.UserLastName));



                    userDetailComplete.MobileNo = ret.MobileNo;
                    userDetailComplete.UserEmail = ret.UserEmail;
                    userDetailComplete.JobTitleId = ret.JobTitleId;
                    userDetailComplete.ManagerId = ret.ManagerId;
                    userDetailComplete.OutletId = ret.OutletId;
                    userDetailComplete.ExtraOutletId = ret.ExtraOutletId;
                    userDetailComplete.SubscriptionId = ret.SubscriptionId;
                    userDetailComplete.Active = ret.Active;

                    userDetailComplete.JobTitleDesc = ret.JobTitleDesc;
                    userDetailComplete.UserTypeId = ret.UserTypeId;

                    userDetailComplete.OutletName = ret.OutletName;
                    userDetailComplete.OutletAddress = ret.OutletAddress;
                    userDetailComplete.OutletCountry = ret.OutletCountry;


                    userDetailComplete.SubscriptionName = ret.SubscriptionName;
                    userDetailComplete.SubscriptionType = ret.SubscriptionType;
                    var price = Convert.ToString(ret.SubscriptionPrice);
                    userDetailComplete.SubscriptionPrice = double.Parse(price);

                    details.Add(userDetailComplete);
                }


                return details.AsEnumerable();

            }



        }


        public async Task<IEnumerable<UserDetailComplete>> GetManagers(int SettingsId)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();

                var dynamicParameters = new DynamicParameters();

                dynamicParameters.Add("@SettingsId", SettingsId);

                dynamic returns = await sqlConnection.QueryAsync<dynamic>(
                    "SP_GetAllManager",
                    dynamicParameters,
                    commandType: CommandType.StoredProcedure);
                
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;


                List<UserDetailComplete> details = new List<UserDetailComplete>();
                foreach (var ret in returns)
                {
                    UserDetailComplete userDetailComplete = new UserDetailComplete();

                    userDetailComplete.Active = ret.Active;
                    userDetailComplete.UserDetailId = ret.UserDetailId;
                    userDetailComplete.UserFirstName = ret.UserFirstName != null ? ret.UserFirstName : "" ;
                    userDetailComplete.UserLastName = ret.UserLastName != null ? ret.UserLastName : "";

                    userDetailComplete.UserFirstName = textInfo.ToTitleCase(Convert.ToString(userDetailComplete.UserFirstName));
                    userDetailComplete.UserLastName = textInfo.ToTitleCase(Convert.ToString(userDetailComplete.UserLastName));



                    userDetailComplete.MobileNo = ret.MobileNo;
                    userDetailComplete.UserEmail = ret.UserEmail;
                    userDetailComplete.JobTitleId = ret.JobTitleId;
                    userDetailComplete.ManagerId = ret.ManagerId;
                    userDetailComplete.OutletId = ret.OutletId;
                    userDetailComplete.ExtraOutletId = ret.ExtraOutletId;
                    userDetailComplete.SubscriptionId = ret.SubscriptionId;
                    userDetailComplete.Active = ret.Active;

                    userDetailComplete.JobTitleDesc = ret.JobTitleDesc;
                    userDetailComplete.UserTypeId = ret.UserTypeId;

                    userDetailComplete.OutletName = ret.OutletName;
                    userDetailComplete.OutletAddress = ret.OutletAddress;
                    userDetailComplete.OutletCountry = ret.OutletCountry;


                    userDetailComplete.SubscriptionName = ret.SubscriptionName;
                    userDetailComplete.SubscriptionType = ret.SubscriptionType;
                    var price = Convert.ToString(ret.SubscriptionPrice);
                    userDetailComplete.SubscriptionPrice = double.Parse(price);

                    details.Add(userDetailComplete);
                }


                return details.AsEnumerable();

            }

     

        }



        public async Task<IEnumerable<UserDetailComplete>> GetAllStaffCompleteByOutletId(int SettingsId, int OutletId)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();

                var dynamicParameters = new DynamicParameters();
                
                dynamicParameters.Add("@SettingsId", SettingsId);
                dynamicParameters.Add("@OutletId", OutletId);
                dynamic returns = await sqlConnection.QueryAsync<dynamic>(
                    "SP_GetAllStaffByOutletId",
                    dynamicParameters,
                    commandType: CommandType.StoredProcedure);

                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;


                List<UserDetailComplete> details = new List<UserDetailComplete>();
                foreach (var ret in returns)
                {
                    UserDetailComplete userDetailComplete = new UserDetailComplete();

                    userDetailComplete.Active = ret.Active;
                    userDetailComplete.UserDetailId = ret.UserDetailId;
                    userDetailComplete.UserFirstName = ret.UserFirstName != null ? ret.UserFirstName : "";
                    userDetailComplete.UserLastName = ret.UserLastName != null ? ret.UserLastName : "";

                    userDetailComplete.UserFirstName = textInfo.ToTitleCase(Convert.ToString(userDetailComplete.UserFirstName));
                    userDetailComplete.UserLastName = textInfo.ToTitleCase(Convert.ToString(userDetailComplete.UserLastName));



                    userDetailComplete.MobileNo = ret.MobileNo;
                    userDetailComplete.UserEmail = ret.UserEmail;
                    userDetailComplete.JobTitleId = ret.JobTitleId;
                    userDetailComplete.ManagerId = ret.ManagerId;
                    userDetailComplete.OutletId = ret.OutletId;
                    userDetailComplete.ExtraOutletId = ret.ExtraOutletId;
                    userDetailComplete.SubscriptionId = ret.SubscriptionId;
                    userDetailComplete.Active = ret.Active;

                    userDetailComplete.JobTitleDesc = ret.JobTitleDesc;
                    userDetailComplete.UserTypeId = ret.UserTypeId;

                    userDetailComplete.OutletName = ret.OutletName;
                    userDetailComplete.OutletAddress = ret.OutletAddress;
                    userDetailComplete.OutletCountry = ret.OutletCountry;


                    userDetailComplete.SubscriptionName = ret.SubscriptionName;
                    userDetailComplete.SubscriptionType = ret.SubscriptionType;
                    var price = Convert.ToString(ret.SubscriptionPrice);
                    userDetailComplete.SubscriptionPrice = double.Parse(price);

                    details.Add(userDetailComplete);
                }


                return details.AsEnumerable();

            }



        }



        public async Task<IEnumerable<UserDetailComplete>> GetStaffs(int SettingsId)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();

                var dynamicParameters = new DynamicParameters();

                dynamicParameters.Add("@SettingsId", SettingsId);

                dynamic returns = await sqlConnection.QueryAsync<dynamic>(
                    "SP_GetAllStaffBySettingsId",
                    dynamicParameters,
                    commandType: CommandType.StoredProcedure);

                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;


                List<UserDetailComplete> details = new List<UserDetailComplete>();
                foreach (var ret in returns)
                {
                    UserDetailComplete userDetailComplete = new UserDetailComplete();

                    userDetailComplete.Active = ret.Active;
                    userDetailComplete.UserDetailId = ret.UserDetailId;
                    userDetailComplete.UserFirstName = ret.UserFirstName != null ? ret.UserFirstName : "";
                    userDetailComplete.UserLastName = ret.UserLastName != null ? ret.UserLastName : "";

                    userDetailComplete.UserFirstName = textInfo.ToTitleCase(Convert.ToString(userDetailComplete.UserFirstName));
                    userDetailComplete.UserLastName = textInfo.ToTitleCase(Convert.ToString(userDetailComplete.UserLastName));

                    userDetailComplete.MobileNo = ret.MobileNo;
                    userDetailComplete.UserEmail = ret.UserEmail;
                    userDetailComplete.JobTitleId = ret.JobTitleId;
                    userDetailComplete.OutletId = ret.OutletId;
                    userDetailComplete.Active = ret.Active;

                    userDetailComplete.JobTitleDesc = ret.JobTitleDesc;
                    userDetailComplete.UserTypeId = ret.UserTypeId;

                    userDetailComplete.OutletName = ret.OutletName;
                    userDetailComplete.OutletAddress = ret.OutletAddress;
                    userDetailComplete.OutletCountry = ret.OutletCountry;

                    details.Add(userDetailComplete);
                }


                return details.AsEnumerable();

            }



        }



    }
}


