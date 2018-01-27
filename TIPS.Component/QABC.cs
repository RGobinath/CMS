using PersistenceFactory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TIPS.Entities.AdmissionEntities;
using TIPS.Entities.QAEntities;
using TIPS.Entities.StaffManagementEntities;

namespace TIPS.Component
{
   public  class QABC
    {
     PersistenceServiceFactory PSF = null;
     public QABC()
        {
            List<String> Assembly = new List<String>();
            Assembly.Add("TIPS.Entities");
            Assembly.Add("TIPS.Entities.Assess");
            Assembly.Add("TIPS.Entities.TicketingSystem");
            Assembly.Add("TIPS.Entities.TaskManagement");
            PSF = new PersistenceServiceFactory(Assembly);
        }


     public Dictionary<long, IList<QADetails>> GetQADetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeSearchCriteria)
     {
         try
         {
             return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<QADetails>(page, pageSize, sortType, sortby, criteria, likeSearchCriteria);

         }
         catch (Exception)
         {
             throw;
         }
     }
       //getting CampusBasedStaffDetails

     public Dictionary<long, IList<CampusBasedStaffDetails>> GetCampusBasedStaffDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeSearchCriteria)
     {
         try
         {
             return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<CampusBasedStaffDetails>(page, pageSize, sortType, sortby, criteria, likeSearchCriteria);

         }
         catch (Exception)
         {
             throw;
         }
     }

       //getting QAProcessLog details list
     public Dictionary<long, IList<QAProcessLog>> GetQAProcessLogListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeSearchCriteria)
     {
         try
         {
             return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<QAProcessLog>(page, pageSize, sortType, sortby, criteria, likeSearchCriteria);

         }
         catch (Exception)
         {
             throw;
         }
     }

     public Dictionary<long, IList<QAQuestions>> GetQAQuestionListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeSearchCriteria)
     {
         try
         {
             return PSF.GetListWithExactAndLikeSearchCriteriaWithCountWithLongArray<QAQuestions>(page, pageSize, sortType, sortby, criteria, likeSearchCriteria);
         }
         catch (Exception)
         {
             throw;
         }
     }


     public QAQuestions GetQAQuetionById(long QuestionId)
        {
            try
            {
                return PSF.Get<QAQuestions>("Id", QuestionId);
            }
            catch (Exception)
            {
                throw;
            }
        }


     public Dictionary<long, IList<QAAnswers>> GetQAAnswersListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeSearchCriteria)
     {
         try
         {
             return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<QAAnswers>(page, pageSize, sortType, sortby, criteria, likeSearchCriteria);
         }
         catch (Exception)
         {
             throw;
         }
     }

     public long SaveorUpdateQAAnswers(QAAnswers Answers)
     {
         try
         {
             if (Answers != null)
                 PSF.SaveOrUpdate<QAAnswers>(Answers);
             else { throw new Exception("QAAnswers is required and it cannot be null.."); }
             return Answers.Id;
         }
         catch (Exception)
         {

             throw;
         }
     }
       public long SaveorUpdateQAQuestions(QAQuestions questions)
     {
         try
         {
             if (questions != null)
                 PSF.SaveOrUpdate<QAQuestions>(questions);
             else { throw new Exception("QAAnswers is required and it cannot be null.."); }
             return questions.Id;
         }
         catch (Exception)
         {

             throw;
         }
     }
       //save or update QAProcesslog

       public long SaveOrUpdateQAProcessLog(QAProcessLog qaprocesslog)
     {
         try
         {
             if (qaprocesslog != null)
                 PSF.SaveOrUpdate<QAProcessLog>(qaprocesslog);
             else { throw new Exception("qaprocesslog is required and it cannot be null.."); }
             return qaprocesslog.Id;
         }
         catch (Exception)
         {

             throw;
         }
     }

       public Dictionary<long, IList<UploadedFiles>> GetUploadedFilesListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
       {
           try
           {
               Dictionary<long, IList<UploadedFiles>> retValue = new Dictionary<long, IList<UploadedFiles>>();
               return PSF.GetListWithExactSearchCriteriaCount<UploadedFiles>(page, pageSize, sortType, sortBy, criteria);
           }
           catch (Exception)
           {
               throw;
           }
       }
       //Getting StaffDetails by Userid
       public StaffDetails GetStaffDetailsByUserId(string userId)
       {
           try
           {
               return PSF.Get<StaffDetails>("StaffUserName", userId);

           }
           catch (Exception)
           {
               throw;

           }
           }
       //getting QAProcessLog by ProcessedDate

       public QAProcessLog GetProcessLogByProcessedDate()
       {
           try
           {
            //   return PSF.Get<QAProcessLog>("ProcessedDate", DateTime.Now);
               return null;

           }
           catch (Exception)
           {
               throw;

           }
           }

       //Getting CampusBasedStaffDetails by userid
       public CampusBasedStaffDetails GetCampusBasedStaffDetailsbyUserId(string userId)
       {
           try
           {
               return PSF.Get<CampusBasedStaffDetails>("UserId", userId);

           }
           catch (Exception)
           {
               throw;

           }
           }
       //Getting QADashboard details

       public IList<QADashboard> GetGetQAReportbyFlag(string Flag, string UserId)
        {
            try
            {

                string query = "Exec QADashboard @spFlag='" + Flag;
                query = query + "', @spUserId='" + UserId+"'";
              
               

                IList list = PSF.ExecuteSql(query);

                IList<QADashboard> QAReport = new List<QADashboard>();
                switch (Flag)
                {
                    case "INBOX":
                        foreach (var obj in list)
                        {
                            QADashboard dashboard = new QADashboard();
                            dashboard.Id = Convert.ToInt64(((object[])(obj))[0]);
                            dashboard.Grade = Convert.ToString(((object[])(obj))[1]);
                            dashboard.NoOfQuestions = Convert.ToInt64(((object[])(obj))[2]);
                            QAReport.Add(dashboard);

                        }
                        break;
                    case "ALLCAMPUS":
                        foreach (var obj in list)
                        {
                            QADashboard dashboard = new QADashboard();
                            dashboard.Id = Convert.ToInt64(((object[])(obj))[0]);
                            dashboard.Campus = Convert.ToString(((object[])(obj))[1]);
                            dashboard.NoOfQuestions = Convert.ToInt64(((object[])(obj))[2]);
                            QAReport.Add(dashboard);

                        }
                        break;

                    case "QUESTIONCOUNT":
                        foreach (var obj in list)
                        {
                            QADashboard dashboard = new QADashboard();
                            dashboard.UserId = Convert.ToString(((object[])(obj))[0]);
                            dashboard.InboxCount = Convert.ToInt64(((object[])(obj))[1]);
                            dashboard.CampusCount = Convert.ToInt64(((object[])(obj))[2]);
                            dashboard.AllCampusCount = Convert.ToInt64(((object[])(obj))[3]);
                            QAReport.Add(dashboard);

                        }
                        break;
                    case "SUCCESSRATIO":
                        foreach (var obj in list)
                        {
                            QADashboard dashboard = new QADashboard();
                            dashboard.UserId = Convert.ToString(((object[])(obj))[0]);
                            dashboard.Success = Convert.ToInt64(((object[])(obj))[1]);
                            dashboard.Failiure = Convert.ToInt64(((object[])(obj))[2]);
                     
                            QAReport.Add(dashboard);

                        }
                        break;
                   
                    default:
                        break;


                }

                return QAReport;
                // return PSF.ExecuteSql<samplestoredproceduure>(query);

            }
            catch (Exception)
            {
                throw;
            }
        }


       public Dictionary<long, IList<QALikes>> GetQALikesListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeSearchCriteria)
       {
           try
           {
               //return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<QAQuestions>(page, pageSize, sortType, sortby, criteria, likeSearchCriteria);
               return PSF.GetListWithExactAndLikeSearchCriteriaWithCountWithLongArray<QALikes>(page, pageSize, sortType, sortby, criteria, likeSearchCriteria);
           }
           catch (Exception)
           {
               throw;
           }
       }


       public Dictionary<long, IList<QAViewers>> GetQAViewsListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeSearchCriteria)
       {
           try
           {
               //return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<QAQuestions>(page, pageSize, sortType, sortby, criteria, likeSearchCriteria);
               return PSF.GetListWithExactAndLikeSearchCriteriaWithCountWithLongArray<QAViewers>(page, pageSize, sortType, sortby, criteria, likeSearchCriteria);
           }
           catch (Exception)
           {
               throw;
           }
       }
       public void CreateorUpdateViewers(QAViewers viewers)
       {
           try
           {
               if (viewers != null)
               {
                   PSF.SaveOrUpdate<QAViewers>(viewers);
               }
               return;
           }
           catch (Exception)
           {
               throw;
           }
       }
       public void CreateOrUpdateLikes(QALikes Likes)
       {
           try
           {
               if (Likes != null)
               {
                   PSF.SaveOrUpdate<QALikes>(Likes);
               }
               return;
           }
           catch (Exception)
           {
               throw;
           }
       }
       public long CreateOrUpdateQuestion(QAQuestions Question)
       {
           try
           {
               if (Question != null)
               {
                   PSF.SaveOrUpdate<QAQuestions>(Question);

               }
               return Question.Id;
           }
           catch (Exception)
           {
               throw;
           }
       }

       public QAAnswers GetQAAnswerById(long Id)
       {
           try
           {
               if (Id > 0)
                   return PSF.Get<QAAnswers>("Id", Id);
               else { throw new Exception("Id Should not be Empty"); }
           }
           catch (Exception)
           {

               throw;
           }
       }
    }
}
