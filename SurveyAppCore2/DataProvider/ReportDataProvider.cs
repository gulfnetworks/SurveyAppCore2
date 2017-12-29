using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using SurveyAppCore2.Models;
using System.Data;
using System.Diagnostics;

namespace SurveyAppCore2.DataProvider
{
    public class ReportDataProvider
    {
        public IConfigurationRoot Configuration { get; }
        private SqlConnection sqlConnection;
        private string connectionString;

        public ReportDataProvider()
        {
            string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new String[] { @"bin\" }, StringSplitOptions.None)[0];
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(projectPath)
                .AddJsonFile("appsettings.json")
                .Build();
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<SurveyComplete>> GetFeedbackReport(GeneralSurvey generalSurvey, int SettingsId)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();

                var dynamicParameters = new DynamicParameters();
                string[] segmentList = generalSurvey.Segment.Split(',');


                generalSurvey.DateStart += " 00:00:00";
                generalSurvey.DateEnd += " 23:59:59";

                dynamicParameters.Add("@OutletId", generalSurvey.OutletId);
                dynamicParameters.Add("@DateStart", generalSurvey.DateStart);
                dynamicParameters.Add("@DateEnd", generalSurvey.DateEnd);
                dynamicParameters.Add("@Status", generalSurvey.Statuses);
                dynamicParameters.Add("@SettingsId", SettingsId);

                dynamic returns = await sqlConnection.QueryAsync<dynamic>(
                     "SP_GetGeneralSurvey",
                     dynamicParameters,
                     commandType: CommandType.StoredProcedure);


                List<SurveyComplete> details = new List<SurveyComplete>();
                foreach (var ret in returns)
                {
                    SurveyComplete surveyComplete = new SurveyComplete();

                    surveyComplete.SurveyId = ret.SurveyId;
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




                    bool isFeedback = false;

                    if (surveyComplete.QualityComment != null) {  if(surveyComplete.QualityComment.Trim() != "") isFeedback = true;  }

                    if (surveyComplete.ValueComment != null) { if(surveyComplete.ValueComment.Trim() != "") isFeedback = true; }

                    if (surveyComplete.ServiceComment != null) { if(surveyComplete.ServiceComment.Trim() != "") isFeedback = true; }

                    if (surveyComplete.AmbienceComment != null) { if(surveyComplete.AmbienceComment.Trim() != "") isFeedback = true; }

                    if (surveyComplete.RecommendPoorArea != null) { if(surveyComplete.RecommendPoorArea.Trim() != "") isFeedback = true; }


                    if (surveyComplete.RecommendImprovements != null) { if(surveyComplete.RecommendImprovements.Trim() != "") isFeedback = true; }

                    if (surveyComplete.RecommendSuggestions != null) { if(surveyComplete.RecommendSuggestions.Trim() != "") isFeedback = true; }

                    if (surveyComplete.LastVisitComment != null) { if(surveyComplete.LastVisitComment.Trim() != "") isFeedback = true; }



                    if (isFeedback)
                    {
                        if (segmentList.ToList().Contains("24"))
                        {
                            details.Add(surveyComplete);
                        }
                        else if (segmentList.ToList().Contains(ret.DateTime.ToString("HH")))
                        {
                            details.Add(surveyComplete);
                        }

                    }


                }



                if (generalSurvey.Type.ToUpper() == "COMPLAINT")
                {
                    return details.Where(a => a.Complaint == 1).ToList().AsEnumerable();
                }
                else if (generalSurvey.Type.ToUpper() == "COMPLIMENT")
                {
                    return details.Where(a => a.Compliment == 1).ToList().AsEnumerable();
                }
                else if (generalSurvey.Type.ToUpper() == "FEEDBACK")
                {

                    return details.Where(a => a.Feedback == 1).ToList().AsEnumerable();
                }


                //var questions =
                //    from a in details
                //    select
                //        new
                //        {
                //            a.ManagerId,
                //            AverageMark = details
                //                .Where(arg => arg.ManagerId == 13)
                //                .Average(arg => arg.AmbienceRate)
                //        };

                return details.AsEnumerable();

            }



        }

        public async Task<IEnumerable<SurveyComplete>> GetGeneralSurveyReport(GeneralSurvey generalSurvey, int SettingsId)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();

                var dynamicParameters = new DynamicParameters();
                string[] segmentList = generalSurvey.Segment.Split(',');
        

                generalSurvey.DateStart += " 00:00:00";
                generalSurvey.DateEnd += " 23:59:59";

                dynamicParameters.Add("@OutletId", generalSurvey.OutletId);
                dynamicParameters.Add("@DateStart", generalSurvey.DateStart);
                dynamicParameters.Add("@DateEnd", generalSurvey.DateEnd);
                dynamicParameters.Add("@Status", generalSurvey.Statuses);
                dynamicParameters.Add("@SettingsId", SettingsId);

                dynamic returns = await sqlConnection.QueryAsync<dynamic>(
                     "SP_GetGeneralSurvey",
                     dynamicParameters,
                     commandType: CommandType.StoredProcedure);


                List<SurveyComplete> details = new List<SurveyComplete>();
                foreach (var ret in returns)
                {
                    SurveyComplete surveyComplete = new SurveyComplete();

                    surveyComplete.SurveyId = ret.SurveyId;
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



                    if (segmentList.ToList().Contains("24"))
                    {
                        details.Add(surveyComplete);
                    }
                    else if (segmentList.ToList().Contains(ret.DateTime.ToString("HH")))
                    {
                        details.Add(surveyComplete);
                    }

                   
                }


              
                if(generalSurvey.Type.ToUpper() == "COMPLAINT"){
                    return details.Where(a => a.Complaint == 1).ToList().AsEnumerable();
                }
                else if (generalSurvey.Type.ToUpper() == "COMPLIMENT") {
                    return details.Where(a => a.Compliment == 1).ToList().AsEnumerable();
                }
                else if (generalSurvey.Type.ToUpper() == "FEEDBACK") {

                    return details.Where(a => a.Feedback== 1).ToList().AsEnumerable();
                }


                //var questions =
                //    from a in details
                //    select
                //        new
                //        {
                //            a.ManagerId,
                //            AverageMark = details
                //                .Where(arg => arg.ManagerId == 13)
                //                .Average(arg => arg.AmbienceRate)
                //        };

                return details.AsEnumerable();

            }



        }


        //private class DaysOutletMetQuota
        //{
        //    public string Date { get; set; }
        //    public float Average { get; set; }
        //}

        //private class TimeGroup
        //{
        //    public string Date { get; set; }
        //    public DateTime DateTime { get; set; }
        //    public TimeSpan StartTime { get; set; }
        //    public TimeSpan EndTime { get; set; }
        //    public string TimeZone { get; set; }
        //}


        //private int countNoOfDays(List<TimeGroup> timeGroups)
        //{
        //    int tmpCount = 0;
        //    var dates = timeGroups.Select(a => a.Date).ToList().Distinct();
        //    foreach (var date in dates)
        //    {
        //        var query = timeGroups.Where(a => a.Date == date).ToList();
        //        tmpCount += (isTwoDayDuty(query) ? 2 : 1);
        //    }

        //    return tmpCount;
        //}


        //private int countNoOfExcludedDays(List<TimeGroup> timeGroups, List<string> excludedDates)
        //{
        //    int tmpCount = 0;
        //    var dates = timeGroups.Select(a => a.Date).ToList().Distinct();
        //    foreach (var date in dates)
        //    {
        //        if (excludedDates.Contains(date))
        //        {
        //            var query = timeGroups.Where(a => a.Date == date).ToList();
        //            tmpCount += (isTwoDayDuty(query) ? 2 : 1);
        //        }
         
        //    }

        //    return tmpCount;
        //}

        //private bool isTwoDayDuty(List<TimeGroup> timeGroups)
        //{
        //    var startTime = timeGroups[0].StartTime;
        //    var endTime = timeGroups[0].EndTime;
        //    if(timeGroups.Count > 1)
        //    if(endTime < startTime)
        //    {
        //            var maxTime = timeGroups.Max(a => a.DateTime);
        //            var minTime = timeGroups.Min(a => a.DateTime);
        //            return (maxTime - minTime).TotalHours > 8;
        //    }
        //    return false;
        //}


        //public async Task<IEnumerable<ManagerReport>> GetManagerReport(GeneralSurvey generalSurvey,int SettingsId)
        //{

   
        //    using (var sqlConnection = new SqlConnection(connectionString))
        //    {
        //        await sqlConnection.OpenAsync();

        //        var dynamicParameters = new DynamicParameters();
        //        string[] segmentList = generalSurvey.Segment.Split(',');


        //        generalSurvey.DateStart += " 00:00:00";
        //        generalSurvey.DateEnd += " 23:59:59";

        //        dynamicParameters.Add("@OutletId", generalSurvey.OutletId);
        //        dynamicParameters.Add("@DateStart", generalSurvey.DateStart);
        //        dynamicParameters.Add("@DateEnd", generalSurvey.DateEnd);
        //        dynamicParameters.Add("@SettingsId", SettingsId);
                
        //        dynamic returns = await sqlConnection.QueryAsync<dynamic>(
        //           "SP_GetManagerReport",
        //           dynamicParameters,
        //           commandType: CommandType.StoredProcedure);


        //        List<ManagerReport> managerReportList = new List<ManagerReport>();
     

        //        float Quality = 0;
        //        float Value = 0;
        //        float Service = 0;
        //        float Ambience = 0;


        //        float Recommend = 0;
        //        float Overall = 0;
        //        string curCountry = "";
        //        string curManager = "";
        //        string curOutlet = "";
        //        int totalSurvey = 0;
        //        int overallTotalSurver = 0;

        //        int Trigger = 0;

        //        List<DaysOutletMetQuota> dqList = new List<DaysOutletMetQuota>();
        //        List<float> npsList = new List<float>();
        //        List<TimeGroup> tgList = new List<TimeGroup>();

        //        foreach (var ret in returns)
        //        {
        //            // SET CURRENT MANAGER
        //            if(curManager == "") { curManager = ret.Manager; curOutlet = ret.OutletName; curCountry = ret.OutletCountry; }
        //            overallTotalSurver += 1;


                

        //            if (ret.Manager != curManager)
        //            {

                        

        //                ManagerReport mr = new ManagerReport();
        //                mr.Ambience = (float)Math.Round(Ambience / totalSurvey, 1, System.MidpointRounding.AwayFromZero);
        //                mr.Value = (float)Math.Round(Value / totalSurvey, 1, System.MidpointRounding.AwayFromZero);
        //                mr.Quality = (float)Math.Round(Quality / totalSurvey, 1, System.MidpointRounding.AwayFromZero);
        //                mr.Service = (float)Math.Round(Service / totalSurvey, 1, System.MidpointRounding.AwayFromZero);
        //                mr.Recommend = (float)Math.Round(Recommend / totalSurvey, 1, System.MidpointRounding.AwayFromZero);

        //                mr.Overall = (float)Math.Round(Overall / totalSurvey, 1, System.MidpointRounding.AwayFromZero);
        //                mr.Manager = curManager;
        //                mr.SurveyCount = totalSurvey;
        //                mr.Outlet = curOutlet;
        //                var query = dqList.Where(a=>a.Average >= 8).GroupBy(a => a.Date).Select(g => new { Date = g.Key, Count = g.ToList().Count }).Where(a => a.Count >= 6).ToList();
        //                //int dateCount = dqList.GroupBy(a => a.Date).Select(g => new { Date = g.Key }).ToList().Count;

        //                int dateCount = countNoOfDays(tgList);
        //                int excludedCnt = countNoOfExcludedDays(tgList,query.Select(a=> a.Date.ToString()).Distinct().ToList());


                      
        //                int promoterCnt =0;
        //                int detractorCnt = 0;
        //                foreach (var npsRate in npsList)
        //                {
        //                    if(npsRate > 8)
        //                    {
        //                        promoterCnt += 1;
        //                    }else if (npsRate < 7)
        //                    {
        //                        detractorCnt += 1;
        //                    }
        //                }


        //                float tmpPromoterPercent = ((float)promoterCnt / (float)totalSurvey) * 100;
        //                float tmpDetractorPercent = ((float)detractorCnt / (float)totalSurvey) * 100;
        //                float nps = (float)Math.Round(tmpPromoterPercent - tmpDetractorPercent, System.MidpointRounding.AwayFromZero);
        //                //float nps = (float)Math.Round((((float)promoterCnt / (float)totalSurvey) * 100) - (((float)detractorCnt / (float)totalSurvey) * 100), System.MidpointRounding.AwayFromZero);
        //                mr.NetPromoterScore = nps;


        //                mr.DaysOnDuty = dateCount;
        //                mr.DaysOutletMetQuota = (float)Math.Round(((float)excludedCnt / (float)dateCount) * 100, System.MidpointRounding.AwayFromZero);
        //                mr.Country = ret.OutletCountry;

        //                mr.OutletDailyQuota = ret.OutletDailyQuota;
        //                managerReportList.Add(mr);


        //                // CLEAR TEMPORARY VARIABLE
        //                curManager = ret.Manager;

        //                totalSurvey = 0;
        //                Quality = 0;
        //                Value = 0;
        //                Service = 0;
        //                Ambience = 0;
        //                Recommend = 0;
        //                Overall = 0;
        //                dqList.Clear();
        //                npsList.Clear();
        //                tgList.Clear();
        //            }

                   
        //            curOutlet = ret.OutletName;
        //            curCountry = ret.OutletCountry;

        //            Quality += ret.QualityRate;

        //            Value += ret.ValueRate;

        //            Service += ret.ServiceRate;

        //            Ambience += ret.AmbienceRate;

        //            Trigger += ret.Trigger;

        //            string tmpStatus = ret.Status;

        //            float tmpQuality = ret.QualityRate;
        //            float tmpValue = ret.ValueRate;
        //            float tmpService = ret.ServiceRate;
        //            float tmpAmbience = ret.AmbienceRate;
        //            float tmpOverall = (float)Math.Round((tmpQuality + tmpValue + tmpService + tmpAmbience) / 4, 1, System.MidpointRounding.AwayFromZero);

        //            Overall += tmpOverall;

        //            var tmpDate = ret.DateTime.ToString("yyyy-MM-dd");
        //            dqList.Add(new DaysOutletMetQuota() { Average = tmpOverall, Date = tmpDate });
        //            tgList.Add(new TimeGroup() { Date = tmpDate, DateTime = ret.DateTime, StartTime = ret.StartTime, EndTime = ret.EndTime, TimeZone = ret.TimeZone });
        //            float tmpRecommend = ret.RecommendRate;
        //            Recommend += tmpRecommend;
        //            totalSurvey += 1;
        //            npsList.Add( tmpRecommend);

        //            //surveyComplete.OutletCountry = ret.OutletCountry;
        //            //surveyComplete.OutletName = ret.OutletName;
        //            //surveyComplete.Staff = ret.Staff;
        //            //surveyComplete.Manager = ret.Manager;


        //            if (overallTotalSurver == returns.Count)
        //            {
        //                ManagerReport mr = new ManagerReport();
        //                mr.Ambience = (float)Math.Round(Ambience / totalSurvey, 1, System.MidpointRounding.AwayFromZero);
        //                mr.Value = (float)Math.Round(Value / totalSurvey, 1, System.MidpointRounding.AwayFromZero);
        //                mr.Quality = (float)Math.Round(Quality / totalSurvey, 1, System.MidpointRounding.AwayFromZero);
        //                mr.Service = (float)Math.Round(Service / totalSurvey, 1, System.MidpointRounding.AwayFromZero);
        //                mr.Recommend = (float)Math.Round(Recommend / totalSurvey, 1, System.MidpointRounding.AwayFromZero);
        //                mr.Overall = (float)Math.Round(Overall / totalSurvey, 1, System.MidpointRounding.AwayFromZero);

        //                mr.Outlet = ret.Outlet;
        //                mr.Manager = ret.Manager;
        //                mr.SurveyCount = totalSurvey;

        //                mr.Trigger = Trigger;
        
        //                var query = dqList.Where(a => a.Average >= 8).GroupBy(a => a.Date).Select(g => new { Date = g.Key, Count = g.ToList().Count }).Where(a => a.Count >= 6).ToList();
        //                //int dateCount = dqList.GroupBy(a => a.Date).Select(g => new { Date = g.Key }).ToList().Count;

        //                int dateCount = countNoOfDays(tgList);
        //                //int excludedCnt = query.Count;
        //                int excludedCnt = countNoOfExcludedDays(tgList, query.Select(a => a.Date.ToString()).Distinct().ToList());


        //                int promoterCnt = 0;
        //                int detractorCnt = 0;
        //                foreach (var npsRate in npsList)
        //                {
        //                    if (npsRate > 8)
        //                    {
        //                        promoterCnt += 1;
        //                    }
        //                    else if (npsRate < 7)
        //                    {
        //                        detractorCnt += 1;
        //                    }
        //                }


        //                float tmpPromoterPercent = ((float)promoterCnt / (float)totalSurvey) * 100;
        //                float tmpDetractorPercent = ((float)detractorCnt / (float)totalSurvey) * 100;
        //                float nps = (float)Math.Round(tmpPromoterPercent - tmpDetractorPercent, System.MidpointRounding.AwayFromZero);
        //                //float nps = (float)Math.Round((((float)promoterCnt / (float)totalSurvey) * 100) - (((float)detractorCnt / (float)totalSurvey) * 100), System.MidpointRounding.AwayFromZero);
        //                mr.NetPromoterScore = nps;

        //                mr.Country = ret.OutletCountry;

        //                mr.DaysOnDuty = dateCount;
        //                mr.DaysOutletMetQuota = (float)Math.Round( ((float)excludedCnt / (float)dateCount) * 100, System.MidpointRounding.AwayFromZero);

        //                mr.Country = ret.OutletCountry;
        //                mr.OutletDailyQuota = ret.OutletDailyQuota;

        //                managerReportList.Add(mr);

        //            }


        //        }






     

        //        return managerReportList.OrderByDescending(a=>a.SurveyCount).ToList();

        //    }



        //}

        public class TempSurveyComplete : SurveyComplete
        {
            public int Trigger { get; set; }
        }

        public async Task<IEnumerable<ManagerReport>> GetManagerReport(GeneralSurvey generalSurvey, int SettingsId)
        {
            List<TempSurveyComplete> details = new List<TempSurveyComplete>();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();

                var dynamicParameters = new DynamicParameters();
       
                string outletList = generalSurvey.OutletId;

                generalSurvey.DateStart += " 00:00:00";
                generalSurvey.DateEnd += " 23:59:59";


                dynamicParameters.Add("@OutletId", outletList);
                dynamicParameters.Add("@DateStart", generalSurvey.DateStart);
                dynamicParameters.Add("@DateEnd", generalSurvey.DateEnd);
                dynamicParameters.Add("@SettingsId", SettingsId);

                dynamic returns = await sqlConnection.QueryAsync<dynamic>(
                     "SP_GetManagerReport",
                     dynamicParameters,
                     commandType: CommandType.StoredProcedure);


               

                int outletDailyQuota = 0;


                foreach (var ret in returns)
                {
                    TempSurveyComplete surveyComplete = new TempSurveyComplete();

                    surveyComplete.SurveyId = ret.SurveyId;
                    surveyComplete.QualityRate = ret.QualityRate;
           
                    surveyComplete.ValueRate = ret.ValueRate;
             
                    surveyComplete.ServiceRate = ret.ServiceRate;
    
                    surveyComplete.AmbienceRate = ret.AmbienceRate;
          
                    surveyComplete.RecommendRate = ret.RecommendRate;
   

         
                    surveyComplete.DateTime = ret.DateTime;
    
                    surveyComplete.ManagerId = ret.ManagerId3;
                    surveyComplete.StaffId = ret.StaffId;
                    surveyComplete.OutletId = ret.OutletId;

                    surveyComplete.OutletCountry = ret.OutletCountry;
                    surveyComplete.OutletName = ret.OutletName;
                    surveyComplete.Manager = ret.Manager;


                    surveyComplete.UTCDate = ret.DateTime.ToString("yyyy-MM-dd");
                    surveyComplete.UTCTime = ret.DateTime.ToString("HH:mm:ss");

                    surveyComplete.Trigger = ret.Trigger;

                    if (outletDailyQuota == 0)
                    outletDailyQuota = ret.OutletDailyQuota;


                    details.Add(surveyComplete);
                }



                return await CalculateManagerData(details, SettingsId, outletList, outletDailyQuota);

            }



        }

        private async Task<IEnumerable<ManagerReport>> CalculateManagerData(List<TempSurveyComplete> SurveyList, int SettingsId,string OutletList, int DailyQuota )
        {


            List<ManagerReport> result = new List<ManagerReport>();
            UserDetailDataProvider userDetailDP = new UserDetailDataProvider();
            SettingsDataProvider settingsDP = new SettingsDataProvider();

            var settings = await settingsDP.GetSettings(SettingsId);

            var outlets = OutletList.Split(',').ToList();
            var managersQuery = await userDetailDP.GetManagers(SettingsId);
            var distinctManagers = managersQuery.Where(a=> outlets.Contains(a.OutletId.ToString())).Select(a => new { ManagerId = a.UserDetailId, a.UserFirstName , a.OutletName , a.OutletCountry }).ToList().Distinct();


            foreach (var manager in distinctManagers)
            {
                ManagerReport managerReport = new ManagerReport();
        
                var surveyListTemp = SurveyList.Where(a => a.ManagerId == manager.ManagerId).ToList();


                int surveyCount = surveyListTemp.Count;


                managerReport.SurveyCount = surveyCount;
                managerReport.Manager = manager.UserFirstName;
                managerReport.Outlet = manager.OutletName;
                managerReport.Country = manager.OutletCountry;

                if (surveyCount > 0)
                {

                    // COMPUTE AVERAGE
                    int ambienceSubTotal = surveyListTemp.Sum(a => a.AmbienceRate);
                    int serviceSubTotal = surveyListTemp.Sum(a => a.ServiceRate);
                    int valueSubTotal = surveyListTemp.Sum(a => a.ValueRate);
                    int qualitySubTotal = surveyListTemp.Sum(a => a.QualityRate);
                    int recommendSubTotal = surveyListTemp.Sum(a => a.RecommendRate);

                    float averageSubTotal = surveyListTemp.Sum(a => (float)Math.Round((float)
                        (a.AmbienceRate + a.ServiceRate + a.ValueRate + a.QualityRate) / 4, 1, System.MidpointRounding.AwayFromZero));



                    managerReport.Ambience = (float)Math.Round((float)ambienceSubTotal / surveyCount, 1, System.MidpointRounding.AwayFromZero);
                    managerReport.Value = (float)Math.Round((float)valueSubTotal / surveyCount, 1, System.MidpointRounding.AwayFromZero);
                    managerReport.Quality = (float)Math.Round((float)qualitySubTotal / surveyCount, 1, System.MidpointRounding.AwayFromZero);
                    managerReport.Service = (float)Math.Round((float)serviceSubTotal / surveyCount, 1, System.MidpointRounding.AwayFromZero);
                    managerReport.Recommend = (float)Math.Round((float)recommendSubTotal / surveyCount, 1, System.MidpointRounding.AwayFromZero);

                    managerReport.Overall = (float)Math.Round(averageSubTotal / surveyCount, 1, System.MidpointRounding.AwayFromZero);



                    // DAYS MET QUOTA PERCENTAGE
                    var daysMetQuotaQuery = surveyListTemp
                                .GroupBy(a => a.UTCDate)
                                .Select(g => new 
                                {
                                    Date = g.Key,
                                    Days =  (TimeSpan.Parse(settings.EndTime) < TimeSpan.Parse(settings.StartTime) ? ( (TimeSpan.Parse(g.Max(a=> a.UTCTime))  - TimeSpan.Parse(g.Min(a => a.UTCTime))).TotalHours > 8 ? 2 : 1) : 1),
                                    Count = g.Count()
                                }).ToList();

                    int daysOnDuty = daysMetQuotaQuery.Sum(a => a.Days);
                    int daysMetQuota = daysMetQuotaQuery.Where(a => a.Count >= 6).Sum(a => a.Days);

                    managerReport.DaysOnDuty = daysOnDuty;
                    managerReport.DaysOutletMetQuota = (float)Math.Round(((float)daysMetQuota / (float)daysOnDuty) * 100, System.MidpointRounding.AwayFromZero);
                    managerReport.OutletDailyQuota = DailyQuota;


                    // TRIGGER COUNT
                    managerReport.Trigger = surveyListTemp.Sum(a=> a.Trigger);

                    // NET PROMOTER
                    int promoterCnt = 0;
                    int detractorCnt = 0;
                    var npsList = surveyListTemp.Select(a => a.RecommendRate).ToList();
                    foreach (var npsRate in npsList)
                    {
                        if (npsRate > 8)
                        {
                            promoterCnt += 1;
                        }
                        else if (npsRate < 7)
                        {
                            detractorCnt += 1;
                        }
                    }


                    float tmpPromoterPercent = ((float)promoterCnt / (float)surveyCount) * 100;
                    float tmpDetractorPercent = ((float)detractorCnt / (float)surveyCount) * 100;
                    float nps = (float)Math.Round(tmpPromoterPercent - tmpDetractorPercent, System.MidpointRounding.AwayFromZero);
                    //float nps = (float)Math.Round((((float)promoterCnt / (float)totalSurvey) * 100) - (((float)detractorCnt / (float)totalSurvey) * 100), System.MidpointRounding.AwayFromZero);
                    managerReport.NetPromoterScore = nps;
                    result.Add(managerReport);
                }



            }

            // ADD AVERAGE
            ManagerReport averageQuery = (from x in result
                                group x by 1 into g
                                select new ManagerReport
                                { 
                                    Country = "AVERAGE",
                                    DaysOnDuty = (int)g.Average(x => x.DaysOnDuty),
                                    DaysOutletMetQuota = (float)g.Average(x => x.DaysOutletMetQuota),
                                    Manager = "AVERAGE",
                                    NetPromoterScore = (float)Math.Round(g.Average(x => x.NetPromoterScore), System.MidpointRounding.AwayFromZero),
                                    Outlet = "AVERAGE",
                                    OutletDailyQuota = (int)g.Average(x => x.OutletDailyQuota),
                                    Recommend = (float)Math.Round(g.Average(x => x.Recommend), 1, System.MidpointRounding.AwayFromZero),
                                    Staff = "AVERAGE",
                                    SurveyCount = (int)g.Average(x => x.SurveyCount),
                                    Trigger = (int)g.Average(x => x.Trigger),
                                    Ambience = (float)Math.Round(g.Average(x => x.Ambience),1, System.MidpointRounding.AwayFromZero),
                                    Value = (float)Math.Round(g.Average(x => x.Value), 1, System.MidpointRounding.AwayFromZero),
                                    Quality = (float)Math.Round(g.Average(x => x.Quality), 1, System.MidpointRounding.AwayFromZero),
                                    Service = (float)Math.Round(g.Average(x => x.Service), 1, System.MidpointRounding.AwayFromZero),
                                    Overall = (float)Math.Round(g.Average(x => x.Overall), 1, System.MidpointRounding.AwayFromZero),
                                }).FirstOrDefault<ManagerReport>();

            result.Add(averageQuery);

            return result.OrderByDescending(a=>a.SurveyCount).AsEnumerable();
        }





        // GET STAFF REPORT

        public async Task<IEnumerable<StaffReport>> GetStaffReport(GeneralSurvey generalSurvey, int SettingsId)
        {
            List<TempSurveyComplete> details = new List<TempSurveyComplete>();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();

                var dynamicParameters = new DynamicParameters();

                string outletList = generalSurvey.OutletId;

                generalSurvey.DateStart += " 00:00:00";
                generalSurvey.DateEnd += " 23:59:59";


                dynamicParameters.Add("@OutletId", outletList);
                dynamicParameters.Add("@DateStart", generalSurvey.DateStart);
                dynamicParameters.Add("@DateEnd", generalSurvey.DateEnd);
                dynamicParameters.Add("@SettingsId", SettingsId);

                dynamic returns = await sqlConnection.QueryAsync<dynamic>(
                     "SP_GetManagerReport",
                     dynamicParameters,
                     commandType: CommandType.StoredProcedure);




                int outletDailyQuota = 0;


                foreach (var ret in returns)
                {
                    TempSurveyComplete surveyComplete = new TempSurveyComplete();

                    surveyComplete.SurveyId = ret.SurveyId;
                    surveyComplete.QualityRate = ret.QualityRate;

                    surveyComplete.ValueRate = ret.ValueRate;

                    surveyComplete.ServiceRate = ret.ServiceRate;

                    surveyComplete.AmbienceRate = ret.AmbienceRate;

                    surveyComplete.RecommendRate = ret.RecommendRate;



                    surveyComplete.DateTime = ret.DateTime;

      
                    surveyComplete.StaffId = ret.StaffId;
                    surveyComplete.OutletId = ret.OutletId;

                    surveyComplete.OutletCountry = ret.OutletCountry;
                    surveyComplete.OutletName = ret.OutletName;
                    surveyComplete.Staff = ret.Staff;


                    surveyComplete.UTCDate = ret.DateTime.ToString("yyyy-MM-dd");
                    surveyComplete.UTCTime = ret.DateTime.ToString("HH:mm:ss");

                    surveyComplete.Trigger = ret.Trigger;

                    if (outletDailyQuota == 0)
                        outletDailyQuota = ret.OutletDailyQuota;


                    details.Add(surveyComplete);
                }



                return await CalculateStaffData(details, SettingsId, outletList, outletDailyQuota);

            }



        }

        private async Task<IEnumerable<StaffReport>> CalculateStaffData(List<TempSurveyComplete> SurveyList, int SettingsId, string OutletList, int DailyQuota)
        {


            List<StaffReport> result = new List<StaffReport>();
            UserDetailDataProvider userDetailDP = new UserDetailDataProvider();
            SettingsDataProvider settingsDP = new SettingsDataProvider();

            var settings = await settingsDP.GetSettings(SettingsId);

            var outlets = OutletList.Split(',').ToList();
            var staffsQuery = await userDetailDP.GetStaffs(SettingsId);
            var distinctStaffs = staffsQuery.Where(a => outlets.Contains(a.OutletId.ToString())).Select(a => new { StaffId = a.UserDetailId, a.UserFirstName, a.OutletName, a.OutletCountry }).ToList().Distinct();


            foreach (var staff in distinctStaffs)
            {
                StaffReport staffReport = new StaffReport();

                var surveyListTemp = SurveyList.Where(a => a.StaffId == staff.StaffId).ToList();


                int surveyCount = surveyListTemp.Count;


                staffReport.SurveyCount = surveyCount;
                staffReport.Staff = staff.UserFirstName;
                staffReport.Outlet = staff.OutletName;
                staffReport.Country = staff.OutletCountry;

                if (surveyCount > 0)
                {

                    // COMPUTE AVERAGE
                    int ambienceSubTotal = surveyListTemp.Sum(a => a.AmbienceRate);
                    int serviceSubTotal = surveyListTemp.Sum(a => a.ServiceRate);
                    int valueSubTotal = surveyListTemp.Sum(a => a.ValueRate);
                    int qualitySubTotal = surveyListTemp.Sum(a => a.QualityRate);
                    int recommendSubTotal = surveyListTemp.Sum(a => a.RecommendRate);

                    float averageSubTotal = surveyListTemp.Sum(a => (float)Math.Round((float)
                        (a.AmbienceRate + a.ServiceRate + a.ValueRate + a.QualityRate) / 4, 1, System.MidpointRounding.AwayFromZero));



                    staffReport.Ambience = (float)Math.Round((float)ambienceSubTotal / surveyCount, 1, System.MidpointRounding.AwayFromZero);
                    staffReport.Value = (float)Math.Round((float)valueSubTotal / surveyCount, 1, System.MidpointRounding.AwayFromZero);
                    staffReport.Quality = (float)Math.Round((float)qualitySubTotal / surveyCount, 1, System.MidpointRounding.AwayFromZero);
                    staffReport.Service = (float)Math.Round((float)serviceSubTotal / surveyCount, 1, System.MidpointRounding.AwayFromZero);
                    staffReport.Recommend = (float)Math.Round((float)recommendSubTotal / surveyCount, 1, System.MidpointRounding.AwayFromZero);

                    staffReport.Overall = (float)Math.Round(averageSubTotal / surveyCount, 1, System.MidpointRounding.AwayFromZero);



                    // DAYS MET QUOTA PERCENTAGE
                    var daysMetQuotaQuery = surveyListTemp
                                .GroupBy(a => a.UTCDate)
                                .Select(g => new
                                {
                                    Date = g.Key,
                                    Days = (TimeSpan.Parse(settings.EndTime) < TimeSpan.Parse(settings.StartTime) ? ((TimeSpan.Parse(g.Max(a => a.UTCTime)) - TimeSpan.Parse(g.Min(a => a.UTCTime))).TotalHours > 8 ? 2 : 1) : 1),
                                    Count = g.Count()
                                }).ToList();

                    int daysOnDuty = daysMetQuotaQuery.Sum(a => a.Days);
                    int daysMetQuota = daysMetQuotaQuery.Where(a => a.Count >= 6).Sum(a => a.Days);

                    staffReport.DaysOnDuty = daysOnDuty;
                    staffReport.DaysOutletMetQuota = (float)Math.Round(((float)daysMetQuota / (float)daysOnDuty) * 100, System.MidpointRounding.AwayFromZero);
                    staffReport.OutletDailyQuota = DailyQuota;


                    // TRIGGER COUNT
                    staffReport.Trigger = surveyListTemp.Sum(a => a.Trigger);

                    // NET PROMOTER
                    int promoterCnt = 0;
                    int detractorCnt = 0;
                    var npsList = surveyListTemp.Select(a => a.RecommendRate).ToList();
                    foreach (var npsRate in npsList)
                    {
                        if (npsRate > 8)
                        {
                            promoterCnt += 1;
                        }
                        else if (npsRate < 7)
                        {
                            detractorCnt += 1;
                        }
                    }


                    float tmpPromoterPercent = ((float)promoterCnt / (float)surveyCount) * 100;
                    float tmpDetractorPercent = ((float)detractorCnt / (float)surveyCount) * 100;
                    float nps = (float)Math.Round(tmpPromoterPercent - tmpDetractorPercent, System.MidpointRounding.AwayFromZero);
                    //float nps = (float)Math.Round((((float)promoterCnt / (float)totalSurvey) * 100) - (((float)detractorCnt / (float)totalSurvey) * 100), System.MidpointRounding.AwayFromZero);
                    staffReport.NetPromoterScore = nps;
                    result.Add(staffReport);
                }



            }

            // ADD AVERAGE
            StaffReport averageQuery = (from x in result
                                          group x by 1 into g
                                          select new StaffReport
                                          {
                                              Country = "AVERAGE",
                                              DaysOnDuty = (int)g.Average(x => x.DaysOnDuty),
                                              DaysOutletMetQuota = (float)g.Average(x => x.DaysOutletMetQuota),
                                              NetPromoterScore = (float)Math.Round(g.Average(x => x.NetPromoterScore), System.MidpointRounding.AwayFromZero),
                                              Outlet = "AVERAGE",
                                              OutletDailyQuota = (int)g.Average(x => x.OutletDailyQuota),
                                              Recommend = (float)Math.Round(g.Average(x => x.Recommend), 1, System.MidpointRounding.AwayFromZero),
                                              Staff = "AVERAGE",
                                              SurveyCount = (int)g.Average(x => x.SurveyCount),
                                              Trigger = (int)g.Average(x => x.Trigger),
                                              Ambience = (float)Math.Round(g.Average(x => x.Ambience), 1, System.MidpointRounding.AwayFromZero),
                                              Value = (float)Math.Round(g.Average(x => x.Value), 1, System.MidpointRounding.AwayFromZero),
                                              Quality = (float)Math.Round(g.Average(x => x.Quality), 1, System.MidpointRounding.AwayFromZero),
                                              Service = (float)Math.Round(g.Average(x => x.Service), 1, System.MidpointRounding.AwayFromZero),
                                              Overall = (float)Math.Round(g.Average(x => x.Overall), 1, System.MidpointRounding.AwayFromZero),
                                          }).FirstOrDefault<StaffReport>();

            result.Add(averageQuery);

            return result.OrderByDescending(a => a.SurveyCount).AsEnumerable();
        }



        // GET OUTLET REPORT
        public class OutletSurveyComplete : SurveyComplete
        {
            public int Trigger { get; set; }
            public int TriggerPercent { get; set; }
            public double LVMonths { get; set; }
            public double LVNever { get; set; }
            public double LV1 { get; set; }
            public double LV2 { get; set; }
            public double LV3 { get; set; }
            public string Status { get; set; }
        }


        public async Task<IEnumerable<OutletReport>> GetOutletReport(GeneralSurvey generalSurvey, int SettingsId)
        {
            List<OutletSurveyComplete> details = new List<OutletSurveyComplete>();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();

                var dynamicParameters = new DynamicParameters();

                string outletList = generalSurvey.OutletId;

                generalSurvey.DateStart += " 00:00:00";
                generalSurvey.DateEnd += " 23:59:59";


                dynamicParameters.Add("@OutletId", outletList);
                dynamicParameters.Add("@DateStart", generalSurvey.DateStart);
                dynamicParameters.Add("@DateEnd", generalSurvey.DateEnd);
                dynamicParameters.Add("@SettingsId", SettingsId);

                dynamic returns = await sqlConnection.QueryAsync<dynamic>(
                     "SP_GetOutletReport",
                     dynamicParameters,
                     commandType: CommandType.StoredProcedure);




                int outletDailyQuota = 0;


                foreach (var ret in returns)
                {
                    OutletSurveyComplete surveyComplete = new OutletSurveyComplete();

                    surveyComplete.SurveyId = ret.SurveyId;
                    surveyComplete.QualityRate = ret.QualityRate;

                    surveyComplete.ValueRate = ret.ValueRate;

                    surveyComplete.ServiceRate = ret.ServiceRate;

                    surveyComplete.AmbienceRate = ret.AmbienceRate;

                    surveyComplete.RecommendRate = ret.RecommendRate;



                    surveyComplete.DateTime = ret.DateTime;

      
                    surveyComplete.StaffId = ret.StaffId;
                    surveyComplete.OutletId = ret.OutletId;

                    surveyComplete.OutletCountry = ret.OutletCountry;
                    surveyComplete.OutletName = ret.OutletName;


                    surveyComplete.UTCDate = ret.DateTime.ToString("yyyy-MM-dd");
                    surveyComplete.UTCTime = ret.DateTime.ToString("HH:mm:ss");

                    surveyComplete.Trigger = ret.Trigger;
                    surveyComplete.Status = ret.Status;

                    if (outletDailyQuota == 0)
                        outletDailyQuota = ret.OutletDailyQuota;


                    details.Add(surveyComplete);
                }



                return await CalculateOutletData(details, SettingsId, outletList, outletDailyQuota);

            }



        }

        private async Task<IEnumerable<OutletReport>> CalculateOutletData(List<OutletSurveyComplete> SurveyList, int SettingsId, string OutletList, int DailyQuota)
        {


            List<OutletReport> result = new List<OutletReport>();
            OutletDataProvider outletDP = new OutletDataProvider();
            SettingsDataProvider settingsDP = new SettingsDataProvider();

            var settings = await settingsDP.GetSettings(SettingsId);

            var outlets = OutletList.Split(',').ToList();
            var outletsQuery = await outletDP.GetOutletsBySettingsId(SettingsId);
            var distinctOutlets = outletsQuery.Where(a => outlets.Contains(a.OutletId.ToString())).Select(a => new { a.OutletId, a.OutletName, a.OutletCountry }).ToList().Distinct();
            var totalTrigger = SurveyList.Sum(a => a.Trigger);

            foreach (var outlet in distinctOutlets)
            {
                OutletReport outletReport = new OutletReport();

                var surveyListTemp = SurveyList.Where(a => a.OutletId == outlet.OutletId).ToList();


                int surveyCount = surveyListTemp.Count;


                outletReport.SurveyCount = surveyCount;

                outletReport.Outlet = outlet.OutletName;
                outletReport.Country = outlet.OutletCountry;

                if (surveyCount > 0)
                {

                    // COMPUTE AVERAGE
                    int ambienceSubTotal = surveyListTemp.Sum(a => a.AmbienceRate);
                    int serviceSubTotal = surveyListTemp.Sum(a => a.ServiceRate);
                    int valueSubTotal = surveyListTemp.Sum(a => a.ValueRate);
                    int qualitySubTotal = surveyListTemp.Sum(a => a.QualityRate);
                    int recommendSubTotal = surveyListTemp.Sum(a => a.RecommendRate);

                    float averageSubTotal = surveyListTemp.Sum(a => (float)Math.Round((float)
                        (a.AmbienceRate + a.ServiceRate + a.ValueRate + a.QualityRate) / 4, 1, System.MidpointRounding.AwayFromZero));



                    outletReport.Ambience = (float)Math.Round((float)ambienceSubTotal / surveyCount, 1, System.MidpointRounding.AwayFromZero);
                    outletReport.Value = (float)Math.Round((float)valueSubTotal / surveyCount, 1, System.MidpointRounding.AwayFromZero);
                    outletReport.Quality = (float)Math.Round((float)qualitySubTotal / surveyCount, 1, System.MidpointRounding.AwayFromZero);
                    outletReport.Service = (float)Math.Round((float)serviceSubTotal / surveyCount, 1, System.MidpointRounding.AwayFromZero);
                    outletReport.Recommend = (float)Math.Round((float)recommendSubTotal / surveyCount, 1, System.MidpointRounding.AwayFromZero);

                    outletReport.Overall = (float)Math.Round(averageSubTotal / surveyCount, 1, System.MidpointRounding.AwayFromZero);



                    // DAYS MET QUOTA PERCENTAGE
                    var daysMetQuotaQuery = surveyListTemp
                                .GroupBy(a => a.UTCDate)
                                .Select(g => new
                                {
                                    Date = g.Key,
                                    Days = (TimeSpan.Parse(settings.EndTime) < TimeSpan.Parse(settings.StartTime) ? ((TimeSpan.Parse(g.Max(a => a.UTCTime)) - TimeSpan.Parse(g.Min(a => a.UTCTime))).TotalHours > 8 ? 2 : 1) : 1),
                                    Count = g.Count()
                                }).ToList();

                    int daysOnDuty = daysMetQuotaQuery.Sum(a => a.Days);
                    int daysMetQuota = daysMetQuotaQuery.Where(a => a.Count >= 6).Sum(a => a.Days);

                    outletReport.DaysOnDuty = daysOnDuty;
                    outletReport.DaysOutletMetQuota = (float)Math.Round(((float)daysMetQuota / (float)daysOnDuty) * 100, System.MidpointRounding.AwayFromZero);
                    outletReport.OutletDailyQuota = DailyQuota;


                    // TRIGGER COUNT
                    outletReport.Trigger = surveyListTemp.Sum(a => a.Trigger);

                    // TRIGGER PERCENTAGE
                    outletReport.TriggerPercent = (outletReport.Trigger / totalTrigger) * 100;


                    // NET PROMOTER
                    int promoterCnt = 0;
                    int detractorCnt = 0;
                    var npsList = surveyListTemp.Select(a => a.RecommendRate).ToList();
                    foreach (var npsRate in npsList)
                    {
                        if (npsRate > 8)
                        {
                            promoterCnt += 1;
                        }
                        else if (npsRate < 7)
                        {
                            detractorCnt += 1;
                        }
                    }


                    float tmpPromoterPercent = ((float)promoterCnt / (float)surveyCount) * 100;
                    float tmpDetractorPercent = ((float)detractorCnt / (float)surveyCount) * 100;
                    float nps = (float)Math.Round(tmpPromoterPercent - tmpDetractorPercent, System.MidpointRounding.AwayFromZero);
                    //float nps = (float)Math.Round((((float)promoterCnt / (float)totalSurvey) * 100) - (((float)detractorCnt / (float)totalSurvey) * 100), System.MidpointRounding.AwayFromZero);
                    outletReport.NetPromoterScore = nps;
                    result.Add(outletReport);
                }



            }

            // ADD AVERAGE
            OutletReport averageQuery = (from x in result
                                          group x by 1 into g
                                          select new OutletReport
                                          {
                                              Country = "AVERAGE",
                                              DaysOnDuty = (int)g.Average(x => x.DaysOnDuty),
                                              DaysOutletMetQuota = (float)g.Average(x => x.DaysOutletMetQuota),
                                              NetPromoterScore = (float)Math.Round(g.Average(x => x.NetPromoterScore), System.MidpointRounding.AwayFromZero),
                                              Outlet = "AVERAGE",
                                              OutletDailyQuota = (int)g.Average(x => x.OutletDailyQuota),
                                              Recommend = (float)Math.Round(g.Average(x => x.Recommend), 1, System.MidpointRounding.AwayFromZero),
                                              SurveyCount = (int)g.Average(x => x.SurveyCount),
                                              Trigger = (int)g.Average(x => x.Trigger),
                                              Ambience = (float)Math.Round(g.Average(x => x.Ambience), 1, System.MidpointRounding.AwayFromZero),
                                              Value = (float)Math.Round(g.Average(x => x.Value), 1, System.MidpointRounding.AwayFromZero),
                                              Quality = (float)Math.Round(g.Average(x => x.Quality), 1, System.MidpointRounding.AwayFromZero),
                                              Service = (float)Math.Round(g.Average(x => x.Service), 1, System.MidpointRounding.AwayFromZero),
                                              Overall = (float)Math.Round(g.Average(x => x.Overall), 1, System.MidpointRounding.AwayFromZero),
                                          }).FirstOrDefault<OutletReport>();

            result.Add(averageQuery);

            return result.OrderByDescending(a => a.SurveyCount).AsEnumerable();
        }



        public class SurveyCompleteCustomer : SurveyComplete
        {
            public int NumberOfVisit { get; set; }
        }

        // NEW CODE
        public async Task<IEnumerable<SurveyCompleteCustomer>> GetCustomerReport(GeneralSurvey generalSurvey, int SettingsId)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();

                var dynamicParameters = new DynamicParameters();
                string[] segmentList = generalSurvey.Segment.Split(',');


                generalSurvey.DateStart += " 00:00:00";
                generalSurvey.DateEnd += " 23:59:59";

                dynamicParameters.Add("@OutletId", generalSurvey.OutletId);
                dynamicParameters.Add("@DateStart", generalSurvey.DateStart);
                dynamicParameters.Add("@DateEnd", generalSurvey.DateEnd);
    
                dynamicParameters.Add("@SettingsId", SettingsId);

                dynamic returns = await sqlConnection.QueryAsync<dynamic>(
                     "SP_GetCustomerList",
                     dynamicParameters,
                     commandType: CommandType.StoredProcedure);


                List<SurveyCompleteCustomer> details = new List<SurveyCompleteCustomer>();
                foreach (var ret in returns)
                {
                    SurveyCompleteCustomer surveyComplete = new SurveyCompleteCustomer();

                    surveyComplete.SurveyId = ret.SurveyId;
                
                    surveyComplete.MobileNo = ret.MobileNo;
 

                    surveyComplete.DateTime = ret.DateTime;
                    surveyComplete.Customer = ret.Customer;
                    surveyComplete.MobileNo = ret.MobileNo;
                    surveyComplete.Email = ret.Email;
                    surveyComplete.CheckNo = ret.CheckNo;
                    surveyComplete.TableNo = ret.TableNo;
                    surveyComplete.ManagerId = ret.ManagerId;
                    surveyComplete.OutletId = ret.OutletId;

                    surveyComplete.OutletCountry = ret.OutletCountry;
                    surveyComplete.OutletName = ret.OutletName;
                    surveyComplete.Manager = ret.Manager;


                    surveyComplete.UTCDate = ret.DateTime.ToString("yyyy-MM-dd");
                    surveyComplete.UTCTime = ret.DateTime.ToString("HH:mm:ss");

                    surveyComplete.NumberOfVisit = ret.NumberOfVisit;



                        details.Add(surveyComplete);
                    


                }


     
                return details.AsEnumerable();

            }



        }


        /////////////

    }
}
