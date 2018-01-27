using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TIPS.Component;
using TIPS.Entities.AdmissionEntities;
using TIPS.Entities.QAEntities;
using TIPS.Entities.StaffManagementEntities;

namespace TIPS.ServiceContract
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "QAService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select QAService.svc or QAService.svc.cs at the Solution Explorer and start debugging.
    public class QAService 
    {

        public Dictionary<long, IList<QADetails>> GetQADetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeSearchCriteria)
        {
            try
            {
                try
                {
                    QABC QABC = new QABC();
                    return QABC.GetQADetailsListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria, likeSearchCriteria);
                }
                catch (Exception)
                {
                    throw;
                }
                finally { }
            }
            catch (Exception)
            {
                throw;
            }
        }
        //getting CampusBasedStaffDetailsList

        public Dictionary<long, IList<CampusBasedStaffDetails>> GetCampusBasedStaffDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeSearchCriteria)
        {
            try
            {
                try
                {
                    QABC QABC = new QABC();
                    return QABC.GetCampusBasedStaffDetailsListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria, likeSearchCriteria);
                }
                catch (Exception)
                {
                    throw;
                }
                finally { }
            }
            catch (Exception)
            {
                throw;
            }
        }
        //getting the QAProcessLog list

        public Dictionary<long, IList<QAProcessLog>> GetQAProcessLogListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeSearchCriteria)
        {
            try
            {
                try
                {
                    QABC QABC = new QABC();
                    return QABC.GetQAProcessLogListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria, likeSearchCriteria);
                }
                catch (Exception)
                {
                    throw;
                }
                finally { }
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
                try
                {
                    QABC QABC = new QABC();
                    return QABC.GetQAQuestionListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria, likeSearchCriteria);
                }
                catch (Exception)
                {
                    throw;
                }
                finally { }
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
                QABC QABC = new QABC();
                return QABC.GetQAQuetionById(QuestionId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Dictionary<long, IList<QAAnswers>> GetQAAnswersListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeSearchCriteria)
        {
            try
            {
                try
                {
                    QABC QABC = new QABC();
                    return QABC.GetQAAnswersListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria, likeSearchCriteria);
                }
                catch (Exception)
                {
                    throw;
                }
                finally { }
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
                QABC QABC = new QABC();
                QABC.SaveorUpdateQAAnswers(Answers);
                return Answers.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public long SaveorUpdateQAQuestions(QAQuestions question)
        {
            try
            {
                QABC QABC = new QABC();
                QABC.SaveorUpdateQAQuestions(question);
                return question.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        //save or update QAProcessLog

        public long SaveOrUpdateQAProcessLog(QAProcessLog qaprocesslog)
        {
            try
            {
                QABC QABC = new QABC();
                QABC.SaveOrUpdateQAProcessLog(qaprocesslog);
                return qaprocesslog.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public Dictionary<long, IList<UploadedFiles>> GetUploadedFilesListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                QABC QABC = new QABC();
                return QABC.GetUploadedFilesListWithPagingAndCriteria(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public StaffDetails GetStaffDetailsByUserId(string UserId)
        {
            try
            {
                QABC QABC = new QABC();
                return QABC.GetStaffDetailsByUserId(UserId);
            }
            catch (Exception ex)
            {
                throw;

            }
        }


        public CampusBasedStaffDetails GetCampusBasedStaffDetailsbyUserId(string UserId)
        {
            try
            {
                QABC QABC = new QABC();
                return QABC.GetCampusBasedStaffDetailsbyUserId(UserId);
            }
            catch (Exception ex)
            {
                throw;

            }
        }
        //Getting QADashboardReport

        public IList<QADashboard> GetGetQAReportbyFlag(string Flag, string UserId)
        {
            QABC QABC = new QABC();

            return QABC.GetGetQAReportbyFlag(Flag, UserId);


        }


        public Dictionary<long, IList<QALikes>> GetQALikesListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeSearchCriteria)
        {
            try
            {
                try
                {
                    QABC QABC = new QABC();
                    return QABC.GetQALikesListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria, likeSearchCriteria);
                }
                catch (Exception)
                {
                    throw;
                }
                finally { }
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
                try
                {
                    QABC QABC = new QABC();
                    return QABC.GetQAViewsListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria, likeSearchCriteria);
                }
                catch (Exception)
                {
                    throw;
                }
                finally { }
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
                QABC QABC = new QABC();
                QABC.CreateorUpdateViewers(viewers);
                return;
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("Cannot insert duplicate key row in object 'dbo.QAQuestions' with"))
                {
                    throw new FaultException("Cannot insert duplicate key in QAQuestions.");
                }
                throw;
            }
            finally { }
        }

        public void CreateOrUpdateLikes(QALikes Likes)
        {
            try
            {
                QABC QABC = new QABC();
                QABC.CreateOrUpdateLikes(Likes);

            }
            catch (Exception ex)
            {

                throw;
            }
            finally { }
        }

        public long CreateOrUpdateQuestion(QAQuestions Question)
        {
            try
            {
                QABC QABC = new QABC();
                return QABC.CreateOrUpdateQuestion(Question);

            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("Cannot insert duplicate key row in object 'dbo.QAQuestions' with"))
                {
                    throw new FaultException("Cannot insert duplicate key in QAQuestions.");
                }
                throw;
            }
            finally { }
        }
        public QAAnswers GetQAAnswerById(long Id)
        {
            try
            {
                QABC QABC = new QABC();
                return QABC.GetQAAnswerById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

    }
}
