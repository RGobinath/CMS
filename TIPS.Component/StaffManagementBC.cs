using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using PersistenceFactory;
using TIPS.Entities;
using TIPS.Entities.StaffManagementEntities;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Text.RegularExpressions;
using TIPS.Entities.StudentsReportEntities;
using System.Collections;
using TIPS.Entities.BioMetricsEntities;
using System.Data.SqlClient;
namespace TIPS.Component
{
    public class StaffManagementBC
    {
        PersistenceServiceFactory PSF = null;
        public StaffManagementBC()
        {
            List<String> Assembly = new List<String>();
            Assembly.Add("TIPS.Entities");
            Assembly.Add("TIPS.Entities.Assess");
            Assembly.Add("TIPS.Entities.TicketingSystem");
            Assembly.Add("TIPS.Entities.TaskManagement");
            PSF = new PersistenceServiceFactory(Assembly);
        }
        public long CreateOrUpdateStaffDetails(StaffDetails sd)
        {
            try
            {
                if (sd != null)
                    PSF.SaveOrUpdate<StaffDetails>(sd);
                else { throw new Exception("StaffManagement is required and it cannot be null.."); }
                return sd.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public long CreateOrUpdateStaffDetailsView(StaffDetailsView sd)
        {
            try
            {
                if (sd != null)
                    PSF.SaveOrUpdate<StaffDetailsView>(sd);
                else { throw new Exception("StaffManagement is required and it cannot be null.."); }
                return sd.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public long CreateOrUpdateStaffDetailsEdit(StaffDetailsEdit sd)
        {
            try
            {
                if (sd != null)
                    PSF.SaveOrUpdate<StaffDetailsEdit>(sd);
                else { throw new Exception("StaffManagement is required and it cannot be null.."); }
                return sd.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public long CreateOrUpdateStaffIdnumber(StaffIdNumber sid)
        {
            try
            {
                if (sid != null)
                    PSF.SaveOrUpdate<StaffIdNumber>(sid);
                else { throw new Exception("StaffManagement is required and it cannot be null.."); }
                return sid.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public StaffIdNumber GetStaffIdnumberById(long Id)
        {
            try
            {
                StaffIdNumber StaffIdno = null;
                if (Id > 0)
                    StaffIdno = PSF.Get<StaffIdNumber>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return StaffIdno;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<StaffDetails>> GetStaffDetailsListWithPaging(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<StaffDetails>> retValue = new Dictionary<long, IList<StaffDetails>>();
                return PSF.GetListWithSearchCriteriaCount<StaffDetails>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<StaffDetailsView>> GetStaffDetailsListWithPagingAndCriteriaWithAlias(int? page, int? pageSize, string sortby, string sortType, string name, string[] values, Dictionary<string, object> criteria, string[] alias)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCountArray<StaffDetailsView>(page, pageSize, sortby, sortType, name, values, criteria, alias);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<StaffDetailsView>> GetStaffDetailsViewListWithPaging(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<StaffDetailsView>> retValue = new Dictionary<long, IList<StaffDetailsView>>();
                return PSF.GetListWithExactSearchCriteriaCount<StaffDetailsView>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<StaffDetailsEdit>> GetStaffDetailsEditListWithPaging(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<StaffDetailsEdit>> retValue = new Dictionary<long, IList<StaffDetailsEdit>>();
                return PSF.GetListWithExactSearchCriteriaCount<StaffDetailsEdit>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<StaffDetailsView>> GetStaffDetailsViewListINWithPaging(int? page, int? pageSize, string sortType, string sortBy, string name, int[] values, Dictionary<string, object> searchCriteria, string[] criteriaAlias)
        {
            try
            {
                Dictionary<long, IList<StaffDetailsView>> retValue = new Dictionary<long, IList<StaffDetailsView>>();
                return PSF.GetListWithInSearchCriteriaCountArray<StaffDetailsView>(page, pageSize, sortType, sortBy, name, values, searchCriteria, criteriaAlias);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public StaffDetails GetStaffDetailsById(long Id)
        {
            try
            {
                StaffDetails StaffDetails = null;
                if (Id > 0)
                    StaffDetails = PSF.Get<StaffDetails>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return StaffDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateStaffRequestNumDetails(StaffRequestNumDetails srn)
        {
            try
            {
                if (srn != null)
                    PSF.SaveOrUpdate<StaffRequestNumDetails>(srn);
                else { throw new Exception("StaffManagement is required and it cannot be null.."); }
                return srn.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<StaffRequestNumDetails>> GetStaffRequestNumDetailsListWithPaging(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<StaffRequestNumDetails>> retValue = new Dictionary<long, IList<StaffRequestNumDetails>>();
                return PSF.GetListWithSearchCriteriaCount<StaffRequestNumDetails>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<StaffQualification>> GetStaffQualificationDetailsListWithPaging(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<StaffQualification>> retValue = new Dictionary<long, IList<StaffQualification>>();
                return PSF.GetListWithSearchCriteriaCount<StaffQualification>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateStaffQualificationDetails(StaffQualification sq)
        {
            try
            {
                if (sq != null)
                    PSF.SaveOrUpdate<StaffQualification>(sq);
                else { throw new Exception("StaffManagement is required and it cannot be null.."); }
                return sq.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<StaffExperience>> GetStaffExperienceDetailsListWithPaging(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<StaffExperience>> retValue = new Dictionary<long, IList<StaffExperience>>();
                return PSF.GetListWithSearchCriteriaCount<StaffExperience>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateStaffExperienceDetails(StaffExperience se)
        {
            try
            {
                if (se != null)
                    PSF.SaveOrUpdate<StaffExperience>(se);
                else { throw new Exception("StaffManagement is required and it cannot be null.."); }
                return se.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<StaffTraining>> GetStaffTrainingDetailsListWithPaging(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<StaffTraining>> retValue = new Dictionary<long, IList<StaffTraining>>();
                return PSF.GetListWithSearchCriteriaCount<StaffTraining>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateStaffTrainingDetails(StaffTraining st)
        {
            try
            {
                if (st != null)
                    PSF.SaveOrUpdate<StaffTraining>(st);
                else { throw new Exception("StaffManagement is required and it cannot be null.."); }
                return st.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool DeleteQualificationDetails(long id)
        {
            try
            {
                StaffQualification StaffQualification = PSF.Get<StaffQualification>(id);
                PSF.Delete<StaffQualification>(StaffQualification);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool DeleteQualificationDetails(long[] id)
        {
            try
            {
                IList<StaffQualification> StaffQualification = PSF.GetListByIds<StaffQualification>(id);
                PSF.DeleteAll<StaffQualification>(StaffQualification);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool DeleteExperienceDetails(long id)
        {
            try
            {
                StaffExperience StaffExperience = PSF.Get<StaffExperience>(id);
                PSF.Delete<StaffExperience>(StaffExperience);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool DeleteExperienceDetails(long[] id)
        {
            try
            {
                IList<StaffExperience> StaffExperience = PSF.GetListByIds<StaffExperience>(id);
                PSF.DeleteAll<StaffExperience>(StaffExperience);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteTrainingDetails(long id)
        {
            try
            {
                StaffTraining StaffTraining = PSF.Get<StaffTraining>(id);
                PSF.Delete<StaffTraining>(StaffTraining);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool DeleteTrainingDetails(long[] id)
        {
            try
            {
                IList<StaffTraining> StaffTraining = PSF.GetListByIds<StaffTraining>(id);
                PSF.DeleteAll<StaffTraining>(StaffTraining);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int CreateOrUpdateEmployeeSalaryDetails(EmployeeSalaryDetails esd)
        {
            try
            {
                if (esd != null)
                    PSF.SaveOrUpdate<EmployeeSalaryDetails>(esd);
                else { throw new Exception("Employee Salary Details is required and it cannot be null.."); }
                return esd.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IList<EmployeeSalaryDetails> CreateOrUpdateEmployeeSalaryList(IList<EmployeeSalaryDetails> empsalLst)
        {
            try
            {
                if (empsalLst != null)
                    PSF.SaveOrUpdate<EmployeeSalaryDetails>(empsalLst);
                else { throw new Exception("MaterialRequestList is required and it cannot be null.."); }
                return empsalLst;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public StaffDetails GetStaffDetailsByIdNumber(string EmployeeId)
        {
            try
            {
                return PSF.Get<StaffDetails>("IdNumber", EmployeeId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<EmployeeSalaryDetails>> GetEmployeeSalaryDetailsDetailsListWithPaging(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCount<EmployeeSalaryDetails>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Staff Birthday Wishes Service
        public bool SendBDayWishes()
        {
            try
            {
                bool ReturnValue = false;
                DateTime Currnt_Date = DateTime.Now;
                int TodayDate = Currnt_Date.Day;
                int TodayMonth = Currnt_Date.Month;
                int TodayYear = Currnt_Date.Year;
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Clear();
                criteria.Add("Status", "Registered");
                criteria.Add("IsBDayWishNeed", true);
                Dictionary<long, IList<StaffDetails>> GetStaffDetails = GetStaffDetailsWithPaging(0, 9999, string.Empty, string.Empty, criteria);
                if (GetStaffDetails != null && GetStaffDetails.Count > 0 && GetStaffDetails.FirstOrDefault().Value.Count > 0)
                {
                    foreach (StaffDetails item in GetStaffDetails.First().Value)
                    {
                        if (item.DOB != null)
                        {
                            item.DOB = item.DOB.Trim();
                            if (!string.IsNullOrEmpty(item.Name) && !string.IsNullOrEmpty(item.DOB) && !string.IsNullOrEmpty(item.EmailId) && !string.IsNullOrEmpty(item.IdNumber))
                            {
                                string DateOfBith = item.DOB;
                                bool Val = DateOfBith.Contains("/");
                                if (Val == true)
                                {
                                    string[] strArray = DateOfBith.Split('/');
                                    if (strArray.Length == 3)
                                    {
                                        int BDayDate = Convert.ToInt32(strArray[0]);
                                        int BDayMonth = Convert.ToInt32(strArray[1]);
                                        int BDayYear = Convert.ToInt32(strArray[2]);
                                        if (TodayDate == BDayDate && TodayMonth == BDayMonth)
                                        {
                                            string StaffBirthdayWishesTask = ConfigurationManager.AppSettings["StaffBirthdayWishesTask"];
                                            if (!string.IsNullOrEmpty(StaffBirthdayWishesTask) && StaffBirthdayWishesTask == "MailAlertOnly")
                                            {
                                                ReturnValue = SendBirthDayWishesMail(item.IdNumber, item.Name, item.DOB, item.EmailId, item.Campus);
                                            }
                                            //if (StaffBirthdayWishesTask == "SMSAlertOnly")
                                            //{
                                            //    if (!string.IsNullOrEmpty(item.PhoneNo))
                                            //    {
                                            //        ReturnValue = SendBirthDaySMS(item.IdNumber, item.Name, item.DOB, item.EmailId, item.Campus, item.PhoneNo);
                                            //    }
                                            //}
                                            //if (StaffBirthdayWishesTask == "MailAndSMSAlert")
                                            //{
                                            //    if (!string.IsNullOrEmpty(item.PhoneNo))
                                            //    {
                                            //        bool Mail;
                                            //        bool SMS;
                                            //        Mail = SendBirthDayWishesMail(item.IdNumber, item.Name, item.DOB, item.EmailId, item.Campus);
                                            //        SMS = SendBirthDaySMS(item.IdNumber, item.Name, item.DOB, item.EmailId, item.Campus, item.PhoneNo);
                                            //        if (Mail == true && SMS == true) { ReturnValue = true; }
                                            //        if (Mail == true && SMS == false) { ReturnValue = true; }
                                            //        if (Mail == false && SMS == true) { ReturnValue = true; }
                                            //    }
                                            //}
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return ReturnValue;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
            }
            return false;
        }
        #region Birthday Mail Work
        public bool SendBirthDayWishesMail(string IdNumber, string Name, string DOB, string EmailId, string Campus)
        {
            try
            {
                DateTime Currnt_Date = DateTime.Now;
                int TodayDate = Currnt_Date.Day;
                int TodayMonth = Currnt_Date.Month;
                int TodayYear = Currnt_Date.Year;
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                string MailBody;
                MailBody = GetBodyofMail();
                StaffBDayWishesStatus WishesStatus = StaffBDayMailSendStatus(IdNumber, EmailId);
                if (WishesStatus == null || WishesStatus.IsSent == false)
                {
                    if (WishesStatus == null)
                    {
                        WishesStatus = StaffMailStatusFirstInsert(Name, IdNumber, DOB, EmailId);
                    }
                    bool ReturnVal;
                    List<StaffBDayWishesMaster> StaffBDayWishesMasterList = new List<StaffBDayWishesMaster>();
                    StaffBDayWishesMaster BDayWishMaster = new StaffBDayWishesMaster();
                    criteria.Clear();
                    criteria.Add("IsUsed", false);
                    Dictionary<long, IList<StaffBDayWishesMaster>> GetStaffBDayWishesMasterDetails = null;
                    GetStaffBDayWishesMasterDetails = GetStaffBDayWishesMaster(0, 9999, string.Empty, string.Empty, criteria);
                    if (GetStaffBDayWishesMasterDetails == null || GetStaffBDayWishesMasterDetails.FirstOrDefault().Key == 0 || GetStaffBDayWishesMasterDetails.FirstOrDefault().Value == null || GetStaffBDayWishesMasterDetails.FirstOrDefault().Value.Count <= 0)
                    {
                        criteria.Clear();
                        criteria.Add("IsUsed", true);
                        Dictionary<long, IList<StaffBDayWishesMaster>> GetStaffBDayWishesMasterDetailsForUpdation = null;
                        GetStaffBDayWishesMasterDetailsForUpdation = GetStaffBDayWishesMaster(0, 9999, string.Empty, string.Empty, criteria);
                        foreach (StaffBDayWishesMaster obj in GetStaffBDayWishesMasterDetailsForUpdation.FirstOrDefault().Value)
                        {
                            obj.IsUsed = false;
                            SaveOrUpdateStaffBDayWishesMasterDetails(obj);
                        }
                        criteria.Clear();
                        criteria.Add("IsUsed", false);
                        GetStaffBDayWishesMasterDetails = GetStaffBDayWishesMaster(0, 9999, string.Empty, string.Empty, criteria);
                    }
                    if (GetStaffBDayWishesMasterDetails != null && GetStaffBDayWishesMasterDetails.Count > 0 && GetStaffBDayWishesMasterDetails.FirstOrDefault().Key > 0)
                    {
                        string Subject = GetStaffBDayWishesMasterDetails.FirstOrDefault().Value[0].Subject;
                        string MailMessage = GetStaffBDayWishesMasterDetails.FirstOrDefault().Value[0].GreetingMessage;
                        string GreetingURl = GetStaffBDayWishesMasterDetails.FirstOrDefault().Value[0].GreetingCardImageURL;
                        long StaffBDayWishesMasterId = GetStaffBDayWishesMasterDetails.FirstOrDefault().Value[0].Id;
                        string GreetingGlobalImage = "<img src='" + GreetingURl + "'" + " width='593' height='325' />";
                        bool IsValidEmailId = IsValidEmailAddressByRegex(EmailId);
                        if (IsValidEmailId == true)
                        {
                            ReturnVal = BDayEMailSend(Name, EmailId, Campus, Subject, MailMessage, MailBody, GreetingGlobalImage);
                            if (ReturnVal == true)
                            {
                                WishesStatus.Name = Name;
                                WishesStatus.IdNumber = IdNumber;
                                WishesStatus.EmailId = EmailId;
                                WishesStatus.DOB = DOB;
                                WishesStatus.IsSent = true;
                                WishesStatus.Message = MailMessage;
                                WishesStatus.CheckDate = Currnt_Date.ToShortDateString();
                                SaveOrUpdateBDayWishMailStatusDetails(WishesStatus);
                                BDayWishMaster = GetStaffBDayWishesMasterById(StaffBDayWishesMasterId);
                                BDayWishMaster.IsUsed = true;
                                SaveOrUpdateStaffBDayWishesMasterDetails(BDayWishMaster);
                            }
                        }
                        else
                        {
                            WishesStatus.Name = Name;
                            WishesStatus.IdNumber = IdNumber;
                            WishesStatus.EmailId = EmailId;
                            WishesStatus.DOB = DOB;
                            WishesStatus.IsSent = false;
                            WishesStatus.Message = MailMessage;
                            WishesStatus.CheckDate = Currnt_Date.ToShortDateString();
                            SaveOrUpdateBDayWishMailStatusDetails(WishesStatus);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public StaffBDayWishesStatus StaffBDayMailSendStatus(string IdNumber, string MailId)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Clear();
                string TodayDate = DateTime.Now.ToShortDateString();
                criteria.Add("CheckDate", TodayDate);
                criteria.Add("IdNumber", IdNumber);
                Dictionary<long, IList<StaffBDayWishesStatus>> MailStatusDetails = GetStaffBDayWishesStatusListWithPaging(null, 9999, string.Empty, string.Empty, criteria);
                if (MailStatusDetails != null && MailStatusDetails.FirstOrDefault().Key > 0 && MailStatusDetails.FirstOrDefault().Value != null && MailStatusDetails.FirstOrDefault().Value.Count > 0)
                {
                    StaffBDayWishesStatus WishStatus = MailStatusDetails.FirstOrDefault().Value[0];
                    return WishStatus;
                }
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public Dictionary<long, IList<StaffBDayWishesStatus>> GetStaffBDayWishesStatusListWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<StaffBDayWishesStatus>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public StaffBDayWishesStatus StaffMailStatusFirstInsert(string Name, string IdNumber, string DOB, string EmailId)
        {
            try
            {
                StaffBDayWishesStatus MailStatus = new StaffBDayWishesStatus();
                string TodayDate = DateTime.Now.ToShortDateString();
                MailStatus.Name = Name;
                MailStatus.IdNumber = IdNumber;
                MailStatus.EmailId = EmailId;
                MailStatus.DOB = DOB;
                MailStatus.IsSent = false;
                MailStatus.Message = "";
                SaveOrUpdateBDayWishMailStatusDetails(MailStatus);
                return MailStatus;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public long SaveOrUpdateBDayWishMailStatusDetails(StaffBDayWishesStatus MailStatus)
        {
            try
            {

                if (MailStatus != null)
                    PSF.SaveOrUpdate<StaffBDayWishesStatus>(MailStatus);
                return MailStatus.Id;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public bool BDayEMailSend(string Name, string EmailId, string Campus, string Subject, string MailMessage, string MailBody, string GreetingGlobalImage)
        {
            try
            {
                string SendEmail = ConfigurationManager.AppSettings["SendEmail1"];
                string From = ConfigurationManager.AppSettings["From"];
                string BCCMailId = ConfigurationManager.AppSettings["StaffBirthdayWishesBCCMailId"];
                if (SendEmail == "false")
                { return false; }
                else
                {
                    System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Clear();
                    criteria.Add("Campus", Campus);
                    criteria.Add("Server", From);
                    Dictionary<long, IList<CampusEmailId>> CampusMailIdList = GetCampusEmailId(0, 9999, string.Empty, string.Empty, criteria);
                    mail.From = new MailAddress(CampusMailIdList.FirstOrDefault().Value[0].EmailId);
                    if (!String.IsNullOrEmpty(EmailId))
                    {
                        mail.To.Add(EmailId);
                        mail.Bcc.Add(BCCMailId);
                        string Mailid = CampusMailIdList.FirstOrDefault().Value[0].EmailId;
                        string Password = CampusMailIdList.FirstOrDefault().Value[0].Password;
                        string Todaydate = DateTime.Now.ToString("dd/MM/yyyy");
                        SmtpClient SmtpServer = new SmtpClient("localhost", 587);
                        SmtpServer.Host = "smtp.gmail.com";
                        mail.Subject = Subject;
                        string BodyMsg = MailMessage;
                        MailBody = MailBody.Replace("{{DateTime}}", DateTime.Now.ToString("dd/MM/yyyy"));
                        MailBody = MailBody.Replace("{{Name}}", Name);
                        MailBody = MailBody.Replace("{{GreetingImageURL}}", GreetingGlobalImage);
                        MailBody = MailBody.Replace("{{Content}}", BodyMsg);
                        mail.Body = MailBody;
                        mail.IsBodyHtml = true;
                        mail.Priority = MailPriority.High;
                        SmtpServer.Credentials = new System.Net.NetworkCredential(Mailid, Password);
                        SmtpServer.EnableSsl = true;
                        SmtpServer.Send(mail);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public string GetBodyofMail()
        {
            try
            {
                string MessageBody = System.IO.File.ReadAllText(HttpContext.Current.Request.MapPath("~/Views/Shared/BDayWishEmailBody.html"));
                return MessageBody;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public Dictionary<long, IList<StaffDetails>> GetStaffDetailsWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<StaffDetails>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        //public string GetRandomBDayWishMessage(int Flag)
        //{
        //    string Message = "";
        //    if (Flag == 0)
        //    {
        //        Message = "Many things have changed over the years, but you're still the same great person you always have been. Here's wishing the best of blessings and good luck on your Happy Birthday!";
        //    }
        //    if (Flag == 1)
        //    {
        //        Message = "On your Birthday see how many more horizons you have to conquer, how many more dreams to live, how many more happy times to witness and how many more milestone to achieve in life. We are wish you a Happy Birthday!";
        //    }
        //    if (Flag == 2)
        //    {
        //        Message = "Your birthday is the first day of another 365-day journey. Be the shining thread in the beautiful tapestry of the cosmos to make this year be your best ever. Enjoy the trip!";
        //    }
        //    if (Flag == 3)
        //    {
        //        Message = "It seems such a great day to say - we feel so lucky that you came our way! Happy Birthday to you. Make it grand.";
        //    }
        //    if (Flag == 4)
        //    {
        //        Message = "Your good deeds have been many, the thanks you get too few… But we really thank you for beeing with us! Celebrate! You deserve the best!";
        //    }
        //    if (Flag == 5)
        //    {
        //        Message = "We hope that you have a great year and accomplish all the fabulous goals you have set. Wish you a Happy Birthday! May you get the best of everything in life.";
        //    }
        //    if (Flag == 6)
        //    {
        //        Message = "Be happy! Today is the day you were sent to this world to be a blessing and inspiration to the people around you! You are a wonderful person. May you be given more birthdays to fulfill all of your dreams.";
        //    }
        //    if (Flag == 7)
        //    {
        //        Message = "May you achieve much more than what you already have in each of the coming birthdays for I know you have the potential to touch the sky. Warm greetings and best wishes on your birthday. Have a successful year ahead.";
        //    }
        //    if (Flag == 8)
        //    {
        //        Message = "Forget about the past and hope for the best for we all believe this birthday will bring all the success you deserve in your life and you will again grow in your professional career. Have a fantastic birthday filled with love and surprises.";
        //    }
        //    if (Flag == 9)
        //    {
        //        Message = "Hope you have all your ambitions come true and always look forward for a better future. Happy birthday!";
        //    }
        //    if (Flag == 10)
        //    {
        //        Message = "May God shower you with success and determination so that you achieve all your dreams on this very special day. Have a wonderful birthday.";
        //    }
        //    return Message;
        //}
        //public string GetRandomBDayWishImages(int Flag)
        //{
        //    string URL = "";
        //    if (Flag == 0)
        //    {
        //        URL = "http://jennibailey.com/wp-content/uploads/2013/12/Birthday-Message.jpg";
        //    }
        //    if (Flag == 1)
        //    {
        //        URL = "http://blogabove.com/wp-content/uploads/birthday-greeting-cards-5.jpg";
        //    }
        //    if (Flag == 2)
        //    {
        //        URL = "https://lh3.ggpht.com/zqK9u1pXsqkCYFy5WzajQGySJklUQOpgRI3p45p-PeJcg0SvjOAcfjaHCLJiziTTMZmB=h310";
        //    }
        //    if (Flag == 3)
        //    {
        //        URL = "http://3.bp.blogspot.com/-RvNhy1fkYuc/TbMWnnSjWGI/AAAAAAAAROc/eEZlH-X5N44/s1600/wishes-on-friends-birthday.gif";
        //    }
        //    if (Flag == 4)
        //    {
        //        URL = "http://wallpapersfly.com/wp-content/uploads/2013/05/hd-birthday-greeting-cards1.jpg";
        //    }
        //    if (Flag == 5)
        //    {
        //        URL = "http://3.bp.blogspot.com/-p_w9FbWZD4g/Ux6j1u2vIgI/AAAAAAAAAtw/6aM9vM1sdt8/s1600/happy_birthday_wishes.jpg";
        //    }
        //    if (Flag == 6)
        //    {
        //        URL = "http://3.bp.blogspot.com/-7KJTgvhkHuc/UqHXjp9QW-I/AAAAAAAAC1c/ECyh6zULlTc/s1600/birthday+wish.gif";
        //    }
        //    if (Flag == 7)
        //    {
        //        URL = "http://wallpaperspoints.com/wp-content/uploads/2013/08/Birthday-Wishes.jpg";
        //    }
        //    if (Flag == 8)
        //    {
        //        URL = "http://www.shayariinhindi.net/wp-content/uploads/2013/11/Happy-Birthday-Greeting-Card-In-English.jpg";
        //    }
        //    if (Flag == 9)
        //    {
        //        URL = "http://ministrygreetings.com/images/PC7027_Happy_birthday_postcard.jpg";
        //    }
        //    if (Flag == 10)
        //    {
        //        URL = "http://blogabove.com/wp-content/uploads/free-birthday-ecards-5.jpg";
        //    }
        //    return URL;
        //}
        private bool IsValidEmailAddressByRegex(string EmailAddress)
        {
            try
            {
                bool valid = false;

                if (!string.IsNullOrEmpty(EmailAddress) && Regex.IsMatch(EmailAddress,
                          @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                          @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$",
                          RegexOptions.IgnoreCase))
                {
                    valid = true;
                }
                return valid;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
            }
            return false;
        }
        public Dictionary<long, IList<CampusEmailId>> GetCampusEmailId(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<CampusEmailId>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public Dictionary<long, IList<StaffBDayWishesMaster>> GetStaffBDayWishesMaster(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<StaffBDayWishesMaster>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public StaffBDayWishesMaster GetStaffBDayWishesMasterById(long Id)
        {
            try
            {

                StaffBDayWishesMaster BDayWishMaster = null;
                if (Id > 0)
                    BDayWishMaster = PSF.Get<StaffBDayWishesMaster>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return BDayWishMaster;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public long SaveOrUpdateStaffBDayWishesMasterDetails(StaffBDayWishesMaster StaffWishMaster)
        {
            try
            {
                if (StaffWishMaster != null)
                    PSF.SaveOrUpdate<StaffBDayWishesMaster>(StaffWishMaster);
                return StaffWishMaster.Id;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        #endregion
        #endregion

        public Sequence GetStaffIdnumberfromSequenceTable(long Id)
        {
            try
            {
                Sequence seqId = null;
                if (Id > 0)
                    seqId = PSF.Get<Sequence>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return seqId;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateSequence(Sequence sid)
        {
            try
            {
                if (sid != null)
                    PSF.SaveOrUpdate<Sequence>(sid);
                else { throw new Exception("StaffManagement is required and it cannot be null.."); }
                return sid.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }


        #region Employee Form
        public long CreateOrUpdateStaffFamilyDetails(StaffFamilyDetails SFD)
        {
            try
            {
                if (SFD != null)
                    PSF.SaveOrUpdate<StaffFamilyDetails>(SFD);
                else { throw new Exception("StaffManagement is required and it cannot be null.."); }
                return SFD.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<long, IList<StaffFamilyDetails>> GetStaffFamilyDetailsListWithPaging(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<StaffFamilyDetails>> retValue = new Dictionary<long, IList<StaffFamilyDetails>>();
                return PSF.GetListWithSearchCriteriaCount<StaffFamilyDetails>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool DeleteStaffFamilyDetails(long[] id)
        {
            try
            {
                IList<StaffFamilyDetails> StaffFamilyDetails = PSF.GetListByIds<StaffFamilyDetails>(id);
                PSF.DeleteAll<StaffFamilyDetails>(StaffFamilyDetails);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateStaffReferenceDetails(StaffReferenceDetails Ref)
        {
            try
            {
                if (Ref != null)
                    PSF.SaveOrUpdate<StaffReferenceDetails>(Ref);
                else { throw new Exception("StaffManagement is required and it cannot be null.."); }
                return Ref.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<long, IList<StaffReferenceDetails>> GetStaffReferenceDetailsListWithPaging(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<StaffReferenceDetails>> retValue = new Dictionary<long, IList<StaffReferenceDetails>>();
                return PSF.GetListWithSearchCriteriaCount<StaffReferenceDetails>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool DeleteStaffReferenceDetails(long[] id)
        {
            try
            {
                IList<StaffReferenceDetails> StaffReferenceDetails = PSF.GetListByIds<StaffReferenceDetails>(id);
                PSF.DeleteAll<StaffReferenceDetails>(StaffReferenceDetails);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        public StaffDetails GetStaffDeatailsByPreRegNum(Int32 PreRegNum)
        {
            try
            {
                StaffDetails StaffDetails = PSF.Get<StaffDetails>("PreRegNum", PreRegNum);
                return StaffDetails;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #region StaffBirthDayReminderToAdmin
        public bool StaffBirthDayReminderToAdmin()
        {
            bool RetValue = false;
            string RecipientInfo = "", Subject = "", MailBody = "";
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            Dictionary<long, IList<CampusMaster>> CampusList = PSF.GetListWithExactSearchCriteriaCount<CampusMaster>(0, 99999, string.Empty, string.Empty, criteria);
            if (CampusList != null && CampusList.Count > 0 && CampusList.FirstOrDefault().Key != 0 && CampusList.FirstOrDefault().Value != null)
            {
                foreach (var CampusItem in CampusList.FirstOrDefault().Value)
                {
                    StaffBdayReminderTracker StafBdayTracker = PSF.Get<StaffBdayReminderTracker>("Campus", CampusItem.Name, "CheckDate", DateTime.Now.ToString("dd'/'MM'/'yyyy"));
                    if (StafBdayTracker == null || StafBdayTracker != null && StafBdayTracker.IsSent == false)
                    {
                        criteria.Clear();
                        DateTime nxttendays;
                        nxttendays = DateTime.Now.AddDays(10);
                        criteria.Add("BirthDay", Convert.ToInt64(nxttendays.Day));
                        criteria.Add("BirthMonth", Convert.ToInt64(nxttendays.Month));
                        criteria.Add("Campus", CampusItem.Name);
                        Dictionary<long, IList<StaffBdayReminder_Vw>> StaffBdayWish = PSF.GetListWithEQSearchCriteriaCount<StaffBdayReminder_Vw>(0, 99999, string.Empty, string.Empty, criteria);
                        if (StaffBdayWish != null && StaffBdayWish.Count > 0 && StaffBdayWish.FirstOrDefault().Key != 0 && StaffBdayWish.FirstOrDefault().Value != null)
                        {
                            string RegNum = "";
                            MailBody = GetMailTemplateBody();
                            RecipientInfo = "Dear Team,";
                            Subject = "Reminded you for the following Birthday Dates";
                            int Tcount = 0;
                            string TdayBday = "Following staff are celebrating their Birthdays in Next 10 Days.<br /><br /><table class='tftable' border='1'><tr><th>S.No</th><th>ID Number</th><th>Campus</th><th>Name</th><th>Designation</th><th>Date Of Birth</th><th>EmailId</th><th>Phone Number</th></tr>";
                            foreach (var BdayWish in StaffBdayWish.FirstOrDefault().Value)
                            {
                                RegNum = RegNum + BdayWish.IdNumber + ",";
                                Tcount++; TdayBday = TdayBday + "<tr><td>" + Tcount + "</td><td>" + BdayWish.IdNumber + "</td><td>" + BdayWish.Campus + "</td><td>" + BdayWish.Name + "</td><td>" + BdayWish.Designation + "</td><td>" + BdayWish.DofB + "</td><td>" + BdayWish.EmailId + "</td><td>" + BdayWish.PhoneNo + "</td><td>";
                            }
                            TdayBday = TdayBday + "</table><br/><br/>";
                            string BodyofMail = "";
                            if (Tcount > 0)
                            {
                                BodyofMail = BodyofMail + TdayBday;
                                RetValue = SendMailFollowupBdayDetailstoGRL(BodyofMail, CampusItem.Name, MailBody, Subject, RecipientInfo);
                            }
                            else { }

                            if (RetValue == true)
                            {
                                BdayRemainderTrackerUpdate("Daily", RegNum, CampusItem.Name, true);
                            }
                        }
                        else
                        {
                            BdayRemainderTrackerUpdate("Daily", "There are no BDay wishes to Sent", CampusItem.Name, true);
                        }
                    }
                }
            }
            return RetValue;
        }
        private string GetMailTemplateBody()
        {
            string MessageBody = System.IO.File.ReadAllText(HttpContext.Current.Request.MapPath("~/Views/Shared/AdmissionEmailBody.html"));
            return MessageBody;
        }
        private bool SendMailFollowupBdayDetailstoGRL(string Body, string Campus, string MailBody, string Subject, string RecipientInfo)
        {
            try
            {

                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                string server = ConfigurationManager.AppSettings["CampusEmailType"].ToString();
                string BdayRemainderInfo = ConfigurationManager.AppSettings["BdayRemainderInfo"].ToString();
                criteria.Add("Campus", Campus);
                criteria.Add("Server", server);
                IList<CampusEmailId> CampusreminderList = PSF.GetListWithSearchCriteria<CampusEmailId>(0, 99999, string.Empty, string.Empty, criteria);
                if (CampusreminderList != null & CampusreminderList.Count > 0)
                {
                    criteria.Clear();
                    string[] CampusAll = new string[2];
                    CampusAll[0] = "All";
                    CampusAll[1] = Campus;
                    criteria.Add("CampusAll", CampusAll);
                    criteria.Add("EmailType", "BDayReminder");
                    IList<MISMailMaster> RemainderEmailList = PSF.GetListWithSearchCriteria<MISMailMaster>(0, 99999, string.Empty, string.Empty, criteria);
                    if (RemainderEmailList != null & RemainderEmailList.Count > 0)
                    {
                        foreach (var item in RemainderEmailList)
                        {
                            mail.To.Add(item.EmailId);

                        }
                    }
                }
                mail.Subject = Subject; // st.Subject;
                MailBody = MailBody.Replace("{{CampusmailId}}", CampusreminderList.First().EmailId);
                MailBody = MailBody.Replace("{{Ph.No}}", CampusreminderList.First().PhoneNumber);
                MailBody = MailBody.Replace("{{DateTime}}", DateTime.Now.ToString());
                MailBody = MailBody.Replace("{{Recipients}}", RecipientInfo);
                MailBody = MailBody.Replace("{{Content}}", Body);
                mail.Body = MailBody;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient("localhost", 25);
                smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address  
                //Or your Smtp Email ID and Password  
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.EnableSsl = true;
                mail.From = new MailAddress(CampusreminderList.First().EmailId.ToString());
                smtp.Credentials = new System.Net.NetworkCredential
               (CampusreminderList.First().EmailId.ToString(), CampusreminderList.First().Password.ToString());
                if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                {
                    try
                    {
                        smtp.Send(mail);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("quota"))
                        {
                            mail.From = new MailAddress(CampusreminderList.First().AlternateEmailId.ToString());
                            smtp.Credentials = new System.Net.NetworkCredential(CampusreminderList.First().AlternateEmailId.ToString(), CampusreminderList.First().AlternateEmailIdPassword.ToString());
                            smtp.Send(mail);
                            return true;
                        }
                        else
                        {
                            mail.From = new MailAddress(CampusreminderList.First().AlternateEmailId.ToString());
                            smtp.Credentials = new System.Net.NetworkCredential(CampusreminderList.First().AlternateEmailId, CampusreminderList.First().AlternateEmailIdPassword);
                            smtp.Send(mail);
                            return true;
                        }
                    }
                }

                else { return false; }
            }
            catch (Exception ey)
            {
                ExceptionPolicy.HandleException(ey, "AdmissionPolicy");
                throw ey;
            }

        }
        public Dictionary<long, IList<StaffBdayReminder_Vw>> GetStaffBdayReminderWithPaging(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<StaffBdayReminder_Vw>> retValue = new Dictionary<long, IList<StaffBdayReminder_Vw>>();
                return PSF.GetListWithSearchCriteriaCount<StaffBdayReminder_Vw>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {
                throw;
            }
        }
        private void BdayRemainderTrackerUpdate(string Type, string sentList, string Campus, bool IsSent)
        {
            StaffBdayReminderTracker BdayTrack = new StaffBdayReminderTracker();
            BdayTrack.Campus = Campus;
            BdayTrack.RemainderType = Type;
            BdayTrack.SentList = sentList;
            BdayTrack.CheckDate = DateTime.Now.ToString("dd'/'MM'/'yyyy");
            BdayTrack.IsSent = IsSent;
            PSF.SaveOrUpdate<StaffBdayReminderTracker>(BdayTrack);
        }
        #endregion

        public long CreateOrUpdateEvents(Event et)
        {
            try
            {
                if (et != null)
                    PSF.SaveOrUpdate<Event>(et);
                else { throw new Exception("StaffManagement is required and it cannot be null.."); }
                return et.EventId;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateEventList(EventList el)
        {
            try
            {
                if (el != null)
                    PSF.SaveOrUpdate<EventList>(el);
                else { throw new Exception("StaffManagement is required and it cannot be null.."); }
                return el.EventListId;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Event GetEventsById(long Id)
        {
            try
            {
                Event evId = null;
                if (Id > 0)
                    evId = PSF.Get<Event>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return evId;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #region AddedByPrabakaran for ProgressBar
        public long ExecutePercentageQueryFromStaffDetailsUsingQuery(long PreRegNum)
        {
            try
            {

                var strsql = "";
                var strsql1 = "";
                var strsql2 = "";
                var strsql3 = "";
                var strsql4 = "";
                long progresscount = 0;
                strsql = strsql + "SELECT (";
                strsql = strsql + "( CASE WHEN Name IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN IdNumber IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN Campus IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN Designation IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN Department IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN DateOfJoin IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN ReportingManager IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN Status IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN StaffType IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN PFNo IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN ESINo IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN TotalYearsOfExp IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN TotalYearsOfTeachingExp IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN SyllabusHandled IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN DOB IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN Age IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN BGRP IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN Gender IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN EmailId IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN PhoneNo IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN FatherName IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN MotherName IS NULL THEN 1 ELSE 0 END)+";
                //strsql = strsql + "( CASE WHEN GuardianName IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN ChildrenIfAny IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN NativeState IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN EmergencyContactPerson IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN EmergencyContactNo IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN InsuranceDetails IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN MaritalStatus IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN LanguagesKnown IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN BankName IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN BankAccountNumber IS NULL THEN 1 ELSE 0 END)+";
                //strsql = strsql + "( CASE WHEN DocumentsGiven IS NULL THEN 1 ELSE 0 END)+";
                //strsql = strsql + "( CASE WHEN PreviousEmployer IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN PermanantAddress IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN AlternateAddress IS NULL THEN 1 ELSE 0 END)+";
                //strsql = strsql + "( CASE WHEN Add1 IS NULL THEN 1 ELSE 0 END)+";
                //strsql = strsql + "( CASE WHEN Add2 IS NULL THEN 1 ELSE 0 END)+";
                //strsql = strsql + "( CASE WHEN City IS NULL THEN 1 ELSE 0 END)+";
                //strsql = strsql + "( CASE WHEN State IS NULL THEN 1 ELSE 0 END)+";
                //strsql = strsql + "( CASE WHEN Country IS NULL THEN 1 ELSE 0 END)+";
                //strsql = strsql + "( CASE WHEN Pin IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN Achievments IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN AnyOtherSignificant IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN SpecialInterests IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN HowYouKnowVacancy IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN BeenShortListedBefore IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN ShortlistedWhy IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN RelativeWorkingWithUs IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN RelativeDetails IS NULL THEN 1 ELSE 0 END)+";
                //strsql = strsql + "( CASE WHEN CommitTimeWithTIPS IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN CareerGrowthExpectation IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN WillingToTravel IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN WillingForRelocation IS NULL THEN 1 ELSE 0 END)+";
                //strsql = strsql + "( CASE WHEN CreatedDate IS NULL THEN 1 ELSE 0 END)+";
                //strsql = strsql + "( CASE WHEN CreatedTime IS NULL THEN 1 ELSE 0 END)+";
                //strsql = strsql + "( CASE WHEN Qualification IS NULL THEN 1 ELSE 0 END)+";
                //strsql = strsql + "( CASE WHEN IsBDayWishNeed IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN AltPhoneNo IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN Written_LanguagesKnown IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN FatherOccupation IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN SpouseOccupation IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN ExpectedSalary IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN LastDrawnGrossSalary IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN LastDrawnNettSalary IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN StaffGroup IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN StaffSubGroup IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN OfficialEmailId IS NULL THEN 1 ELSE 0 END)+";
                strsql = strsql + "( CASE WHEN JoiningDateOrDays IS NULL THEN 1 ELSE 0 END)) AS SumOFValue";

                //strsql = strsql + "( CASE WHEN TypeofAccommodation IS NULL THEN 1 ELSE 0 END)+";
                //strsql = strsql + "( CASE WHEN StudentConcessionAvailed IS NULL THEN 1 ELSE 0 END)+";
                //strsql = strsql + "( CASE WHEN percentageofconcession IS NULL THEN 1 ELSE 0 END)+";
                //strsql = strsql + "( CASE WHEN GradeDivision IS NULL THEN 1 ELSE 0 END)+";
                //strsql = strsql + "( CASE WHEN AccommType IS NULL THEN 1 ELSE 0 END)) AS SumOFValue";
                strsql = strsql + " FROM StaffDetails WHERE PreRegNum='" + PreRegNum + "'";

                IList list = PSF.ExecuteSql(strsql);
                strsql1 = strsql1 + "select PreRegNum from UploadedFiles where PreRegNum='" + PreRegNum + "' and DocumentFor='Staff' and DocumentType='Staff Photo'";
                IList list1 = PSF.ExecuteSql(strsql1);
                strsql2 = strsql2 + "select PreRegNum from UploadedFiles where PreRegNum='" + PreRegNum + "' and DocumentFor='Staff' and DocumentType='School Certificate'";
                IList list2 = PSF.ExecuteSql(strsql2);
                strsql3 = strsql3 + "select PreRegNum from UploadedFiles where PreRegNum='" + PreRegNum + "' and DocumentFor='Staff' and DocumentType='College Certificate'";
                IList list3 = PSF.ExecuteSql(strsql3);
                strsql4 = strsql4 + "select PreRegNum from StaffQualification where PreRegNum='" + PreRegNum + "'";
                IList list4 = PSF.ExecuteSql(strsql4);
                progresscount = Convert.ToInt64(list[0]);
                if (list1.Count == 0)
                {
                    progresscount = progresscount + 1;
                }
                if (list2.Count == 0)
                {
                    progresscount = progresscount + 1;
                }
                if (list3.Count == 0)
                {
                    progresscount = progresscount + 1;
                }
                if (list4.Count == 0)
                {
                    progresscount = progresscount + 1;
                }
                return progresscount;

            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }
        #endregion
        #region Added By Prabakaran for Staff Promotion
        public long CreateOrUpdateStaffPromotionAndTransferDetails(StaffPromotionAndTransferDetails srn)
        {
            try
            {
                if (srn != null)
                    PSF.SaveOrUpdate<StaffPromotionAndTransferDetails>(srn);
                else { throw new Exception("StaffManagement is required and it cannot be null.."); }
                return srn.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        #region Staff Evaluation Category Master By Prabakaran
        public Dictionary<long, IList<StaffEvaluationCategoryMaster>> GetStaffEvaluationCategoryListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<StaffEvaluationCategoryMaster>(page, pageSize, sortType, sortby, criteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long SaveOrUpdateStaffEvaluationCategory(StaffEvaluationCategoryMaster stfevaluationcategory)
        {
            try
            {
                if (stfevaluationcategory != null)
                {
                    PSF.SaveOrUpdate<StaffEvaluationCategoryMaster>(stfevaluationcategory);
                }
                else
                { throw new Exception("All Fields are required and it cannot be null.."); }
                return stfevaluationcategory.StaffEvaluationCategoryId;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public StaffEvaluationCategoryMaster GetStaffEvaluationCategoryById(long Id)
        {
            try
            {
                StaffEvaluationCategoryMaster StaffEvaluationCategory = null;
                if (Id > 0)
                {
                    StaffEvaluationCategory = PSF.Get<StaffEvaluationCategoryMaster>(Id);
                }
                return StaffEvaluationCategory;
            }
            catch (Exception)
            {

                throw;
            }

        }
        //public StaffEvaluationCategoryMaster GetStaffEvaluationCategoryByCategory(string Campus,string Grade,string AcademicYear,string CategoryName)
        //{
        //    try
        //    {
        //        StaffEvaluationCategoryMaster StaffEvaluationCategory = null;
        //        if (!string.IsNullOrEmpty(Campus) && !string.IsNullOrEmpty(Grade) && !string.IsNullOrEmpty(AcademicYear) && !string.IsNullOrEmpty(CategoryName))
        //        {
        //            StaffEvaluationCategory = PSF.Get<StaffEvaluationCategoryMaster>("Campus",Campus,"Grade",Grade,"AcademicYear", AcademicYear,"CategoryName",CategoryName);
        //        }
        //        return StaffEvaluationCategory;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

        //}
        public IList<StaffEvaluationCategoryMaster> SaveOrUpdateStaffEvaluationCategoryByList(IList<StaffEvaluationCategoryMaster> stfevaluationcategory)
        {
            try
            {
                if (stfevaluationcategory.Count > 0)
                {
                    PSF.SaveOrUpdate<StaffEvaluationCategoryMaster>(stfevaluationcategory);
                }
                else
                { throw new Exception("All Fields are required and it cannot be null.."); }
                return stfevaluationcategory;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        #region Staff Evaluation Parameter By Prabakaran
        public Dictionary<long, IList<StaffEvaluationParameter>> GetStaffEvaluationParameterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<StaffEvaluationParameter>(page, pageSize, sortType, sortby, criteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<StaffEvaluationParameter_vw>> GetStaffEvaluationParameter_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<StaffEvaluationParameter_vw>(page, pageSize, sortType, sortby, criteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long SaveOrUpdateStaffEvaluationParameter(StaffEvaluationParameter staffevaluationparameter)
        {
            try
            {
                if (staffevaluationparameter != null)
                {
                    PSF.SaveOrUpdate<StaffEvaluationParameter>(staffevaluationparameter);
                }
                else
                { throw new Exception("All Fields are required and it cannot be null.."); }
                return staffevaluationparameter.StaffEvaluationParameterId;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public StaffEvaluationParameter GetStaffEvaluationParameterById(long Id)
        {
            try
            {
                StaffEvaluationParameter staffevaluationparameter = null;
                if (Id > 0)
                {
                    staffevaluationparameter = PSF.Get<StaffEvaluationParameter>(Id);
                }
                return staffevaluationparameter;
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion
        #region Added By Prabakaran CampusBasedStaffDetails
        public IList<CampusBasedStaffDetails> SaveOrUpdateCampusBasedStaffDetailsByList(IList<CampusBasedStaffDetails> campusbasedstaffdetails)
        {
            try
            {
                if (campusbasedstaffdetails.Count > 0)
                {
                    PSF.SaveOrUpdate<CampusBasedStaffDetails>(campusbasedstaffdetails);
                }
                else
                { throw new Exception("All Fields are required and it cannot be null.."); }
                return campusbasedstaffdetails;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public long DeleteCampusBasedStaffDetailsList(IList<long> Id)
        {
            try
            {
                if (Id != null)
                {
                    IList<CampusBasedStaffDetails> cbsdlist = new List<CampusBasedStaffDetails>();
                    foreach (long item in Id)
                    {
                        CampusBasedStaffDetails cbsd = PSF.Get<CampusBasedStaffDetails>("Id", item);
                        if (cbsd != null)
                        {
                            cbsdlist.Add(cbsd);
                        }
                    }
                    if (cbsdlist.Count > 0)
                    {
                        PSF.DeleteAll<CampusBasedStaffDetails>(cbsdlist);
                    }
                }
                return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public CampusBasedStaffDetails GetCampusBasedStaffDetailsById(long Id)
        {
            try
            {
                CampusBasedStaffDetails CampusBasedStaffDetails = null;
                if (Id > 0)
                    CampusBasedStaffDetails = PSF.Get<CampusBasedStaffDetails>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return CampusBasedStaffDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long SaveOrUpdateCampusBasedStaffDetails(CampusBasedStaffDetails CampusBasedStaffDetails)
        {
            try
            {
                if (CampusBasedStaffDetails != null)
                    PSF.SaveOrUpdate<CampusBasedStaffDetails>(CampusBasedStaffDetails);
                else { throw new Exception("CampusBasedStaffDetails is required and it cannot be null.."); }
                return CampusBasedStaffDetails.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IList<CampusBasedStaffDetails> GetCampusBasedStaffDetailsByStaffsByStaffPreRegNumber(long StaffPreRegNumber)
        {
            try
            {
                IList<CampusBasedStaffDetails> CampusBasedStaffDetailsListByStaffPreRegNum = PSF.GetList<CampusBasedStaffDetails>("StaffPreRegNumber", StaffPreRegNumber);
                return CampusBasedStaffDetailsListByStaffPreRegNum;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        #region Added By Prabakaran StaffWiseScoreReport
        public Dictionary<long, IList<StaffWiseScoreReport_Vw>> GetStaffWiseScoreReportListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeSearchCriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<StaffWiseScoreReport_Vw>(page, pageSize, sortType, sortby, criteria, likeSearchCriteria);

            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<StaffEvaluationCategoryWise_Vw>> GetSStaffEvaluationCategoryWiseListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeSearchCriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<StaffEvaluationCategoryWise_Vw>(page, pageSize, sortType, sortby, criteria, likeSearchCriteria);

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region StaffGroupMaster
        public Dictionary<long, IList<StaffGroupMaster>> GetStaffGroupMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<StaffGroupMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region
        public string GetStaffNameByPreRegNum(long PreRegNum)
        {
            try
            {
                StaffDetailsView StaffDetails = PSF.Get<StaffDetailsView>("PreRegNum", PreRegNum);
                return StaffDetails.Name;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        #region Added By Prabakaran
        public StaffDetailsView GetStaffDetailsViewById(long Id)
        {
            try
            {
                StaffDetailsView StaffDetails = null;
                if (Id > 0)
                    StaffDetails = PSF.Get<StaffDetailsView>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return StaffDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region StaffGroupMaster
        public long CreateOrUpdateStaffGroupMaster(StaffGroupMaster sgm)
        {
            try
            {
                if (sgm != null)
                    PSF.SaveOrUpdate<StaffGroupMaster>(sgm);
                else { throw new Exception("Error"); }
                return sgm.StaffGroupId;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public StaffGroupMaster GetStaffGroupMasterByCampusandGroup(string Campus, string GroupName)
        {
            try
            {
                StaffGroupMaster sgm = null;
                if (!string.IsNullOrEmpty(Campus) && !string.IsNullOrEmpty(GroupName))
                    sgm = PSF.Get<StaffGroupMaster>("Campus", Campus, "GroupName", GroupName);
                else { throw new Exception("Campus and GroupName is required"); }
                return sgm;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public StaffGroupMaster GetStaffGroupMasterByStaffGroupId(long StaffGroupId)
        {
            try
            {
                StaffGroupMaster sgm = null;
                if (StaffGroupId > 0)
                    sgm = PSF.Get<StaffGroupMaster>(StaffGroupId);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return sgm;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool DeleteStaffGroupMaster(long[] id)
        {
            try
            {
                IList<StaffGroupMaster> sgm = PSF.GetListByIds<StaffGroupMaster>(id);
                PSF.DeleteAll<StaffGroupMaster>(sgm);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region StaffSubGroupMaster
        public Dictionary<long, IList<StaffSubGroupMaster>> GetStaffSubGroupMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<StaffSubGroupMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region StaffAttendance
        public long CreateOrUpdateStaffAttendance(StaffAttendance sa)
        {
            try
            {
                if (sa != null)
                    PSF.SaveOrUpdate<StaffAttendance>(sa);
                else { throw new Exception("Staff Attendance Details is required and it cannot be null.."); }
                return sa.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public StaffAttendance GetStaffAttendanceByAttendanceDatewithPreRegNum(long PreRegNum)
        {
            StaffAttendance sa = null;
            if (PreRegNum > 0)
            {
                DateTime AttendanceDate = DateTime.Now.Date;
                sa = PSF.Get<StaffAttendance>("PreRegNum", PreRegNum, "AttendanceDate", AttendanceDate);
            }
            else
            {
                throw new Exception("PreRegNum is required and it cannot be null..");
            }
            return sa;
        }
        #endregion
        #region
        public Dictionary<long, IList<StaffAttendance_vw>> GetStaffAttendance_vwListWithPaging(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<StaffAttendance_vw>> retValue = new Dictionary<long, IList<StaffAttendance_vw>>();
                return PSF.GetListWithExactSearchCriteriaCount<StaffAttendance_vw>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        public StaffFamilyDetails GetStaffFamilyDetailsById(long Id)
        {
            try
            {
                StaffFamilyDetails StaffFamilyDetails = null;
                if (Id > 0)
                    StaffFamilyDetails = PSF.Get<StaffFamilyDetails>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return StaffFamilyDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public StaffQualification GetStaffQualificationDetailsById(long Id)
        {
            try
            {
                StaffQualification StaffQualificationDetails = null;
                if (Id > 0)
                    StaffQualificationDetails = PSF.Get<StaffQualification>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return StaffQualificationDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public StaffExperience GetStaffExperienceDetailsById(long Id)
        {
            try
            {
                StaffExperience StaffExperienceDetails = null;
                if (Id > 0)
                    StaffExperienceDetails = PSF.Get<StaffExperience>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return StaffExperienceDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public StaffReferenceDetails GetStaffReferenceDetailsById(long Id)
        {
            try
            {
                StaffReferenceDetails StaffReferenceDetails = null;
                if (Id > 0)
                    StaffReferenceDetails = PSF.Get<StaffReferenceDetails>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return StaffReferenceDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public StaffDetails GetStaffDeatailsByIdNumber(string IdNumber)
        {
            try
            {
                StaffDetails StaffDetails = PSF.Get<StaffDetails>("IdNumber", IdNumber);
                return StaffDetails;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #region Student survey group By john naveen
        public Dictionary<long, IList<StudentSurveyGroupMaster>> GetStudentSurveyGroupListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<StudentSurveyGroupMaster>(page, pageSize, sortType, sortby, criteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long SaveOrUpdateStudentSurveyGroup(StudentSurveyGroupMaster studentSurveyGroupms)
        {
            try
            {
                if (studentSurveyGroupms != null)
                {
                    PSF.SaveOrUpdate<StudentSurveyGroupMaster>(studentSurveyGroupms);
                }
                else
                { throw new Exception("All Fields are required and it cannot be null.."); }
                return studentSurveyGroupms.StudentSurveyGroupId;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public StudentSurveyGroupMaster GetStudentSurveyGroupById(long Id)
        {
            try
            {
                StudentSurveyGroupMaster StudentSurveyGroup = null;
                if (Id > 0)
                {
                    StudentSurveyGroup = PSF.Get<StudentSurveyGroupMaster>(Id);
                }
                return StudentSurveyGroup;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public IList<StudentSurveyGroupMaster> SaveOrUpdateStudentSurveyGroupByList(IList<StudentSurveyGroupMaster> StudentSurveyGroupms)
        {
            try
            {
                if (StudentSurveyGroupms.Count > 0)
                {
                    PSF.SaveOrUpdate<StudentSurveyGroupMaster>(StudentSurveyGroupms);
                }
                else
                { throw new Exception("All Fields are required and it cannot be null.."); }
                return StudentSurveyGroupms;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        # region Student Survey Question and answer master by vinoth
        public StudentSurveyQuestionMaster GetStudentSurveyQuestionByStudentSurveyQuestionId(long StudentSurveyQuestionId)
        {
            try
            {
                return PSF.Get<StudentSurveyQuestionMaster>("StudentSurveyQuestionId", StudentSurveyQuestionId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public long CreateOrUpdateStudentSurveyQuestion(StudentSurveyQuestionMaster studsurvey)
        {
            try
            {
                if (studsurvey != null)
                    PSF.SaveOrUpdate<StudentSurveyQuestionMaster>(studsurvey);
                else { throw new Exception("StudentSurveyQuestion is required and it cannot be null.."); }
                return studsurvey.StudentSurveyQuestionId;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<StudentSurveyQuestionMaster>> GetStudentSurveyQuestionListWithExcactAndLikeSearchCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria, Dictionary<string, object> likecriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<StudentSurveyQuestionMaster>(page, pageSize, sortType, sortby, criteria, likecriteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool DeleteStudentSurveyQuestion(long[] StudentSurveyQuestionId)
        {
            try
            {
                IList<StudentSurveyQuestionMaster> tasksList = PSF.GetListByIds<StudentSurveyQuestionMaster>(StudentSurveyQuestionId);
                PSF.DeleteAll<StudentSurveyQuestionMaster>(tasksList);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public StudentSurveyAnswerMaster GetStudentSurveyAnswerMasterByStudentSurveyAnswerMasterId(long StudentSurveyAnswerId)
        {
            try
            {
                return PSF.Get<StudentSurveyAnswerMaster>("StudentSurveyAnswerId", StudentSurveyAnswerId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public long CreateOrUpdateStudentSurveyAnswerMaster(StudentSurveyAnswerMaster studsurveyans)
        {
            try
            {
                if (studsurveyans != null)
                    PSF.SaveOrUpdate<StudentSurveyAnswerMaster>(studsurveyans);
                else { throw new Exception("StudentSurveyAnswerMaster is required and it cannot be null.."); }
                return studsurveyans.StudentSurveyAnswerId;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<StudentSurveyAnswerView>> GetStudentSurveyAnswerMasterListWithExcactAndLikeSearchCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria, Dictionary<string, object> likecriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<StudentSurveyAnswerView>(page, pageSize, sortType, sortby, criteria, likecriteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool DeleteStudentSurveyAnswerMaster(long[] StudentSurveyAnswerId)
        {
            try
            {
                IList<StudentSurveyAnswerMaster> tasksList = PSF.GetListByIds<StudentSurveyAnswerMaster>(StudentSurveyAnswerId);
                PSF.DeleteAll<StudentSurveyAnswerMaster>(tasksList);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public StudentSurveyQuestionMaster GetStudentSurveyQuestion(long StudentSurveyGroupId, string StudentSurveyQuestion)
        {
            try
            {
                return PSF.Get<StudentSurveyQuestionMaster>("StudentSurveyGroupMaster.StudentSurveyGroupId", StudentSurveyGroupId, "StudentSurveyQuestion", StudentSurveyQuestion);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public StudentSurveyAnswerMaster GetStudentSurveyAnswer(long StudentSurveyQuestionId, string StudentSurveyAnswer)
        {
            try
            {
                return PSF.Get<StudentSurveyAnswerMaster>("StudentSurveyQuestionMaster.StudentSurveyQuestionId", StudentSurveyQuestionId, "StudentSurveyAnswer", StudentSurveyAnswer);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public StudentSurveyAnswerView GetStudentSurveyAnswerView(long StudentSurveyGroupId, long StudentSurveyQuestionId, string StudentSurveyAnswer)
        {
            try
            {
                return PSF.Get<StudentSurveyAnswerView>("StudentSurveyGroupId", StudentSurveyGroupId, "StudentSurveyQuestionId", StudentSurveyQuestionId, "StudentSurveyAnswer", StudentSurveyAnswer);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<long, IList<StudentSurveyQuestionMasterView>> GetStudentSurveyQuestionMasterViewListWithExcactAndLikeSearchCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria, Dictionary<string, object> likecriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<StudentSurveyQuestionMasterView>(page, pageSize, sortType, sortby, criteria, likecriteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public StudentSurveyGroupMaster GetStudentSurveyGroupMasterByCampusAcdemicYearGrade(string AcademicYear, string Campus, string Grade)
        {
            try
            {
                return PSF.Get<StudentSurveyGroupMaster>("AcademicYear", AcademicYear, "Campus", Campus, "Grade", Grade);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        #region "Staff Survey Report"
        public StaffWiseStudentSurveyNewResult_Vw GetStaffWiseStudentSurveyNewResultById(long Id)
        {
            try
            {
                StaffWiseStudentSurveyNewResult_Vw staffsurvey = null;
                if (Id > 0)
                {
                    staffsurvey = PSF.Get<StaffWiseStudentSurveyNewResult_Vw>(Id);
                }
                return staffsurvey;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public Dictionary<long, IList<StaffWiseStudentSurveyNewResult_Vw>> GetStaffWiseStudentSurverList(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<StaffWiseStudentSurveyNewResult_Vw>(page, pageSize, sortType, sortby, criteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<StaffWiseStudentSurveyNewResultWOS_Vw>> GetStaffWiseStudentSurverWOSList(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<StaffWiseStudentSurveyNewResultWOS_Vw>(page, pageSize, sortType, sortby, criteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<StudentSurveyReportNew_Vw>> GetStaffEvaluationStudentCountList(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<StudentSurveyReportNew_Vw>(page, pageSize, sortType, sortby, criteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<StudentSurveyReportWOSecNew_Vw>> GetStaffEvaluationStudentCountListWithoutSection(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<StudentSurveyReportWOSecNew_Vw>(page, pageSize, sortType, sortby, criteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<StaffEvaluationScore_Vw>> GetStaffEvaluationScoreList(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<StaffEvaluationScore_Vw>(page, pageSize, sortType, sortby, criteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<StaffwiseSurveyQuestionReportWOSec_Vw>> GetStudentSurveyQuestionMarkListWithoutSection(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<StaffwiseSurveyQuestionReportWOSec_Vw>(page, pageSize, sortType, sortby, criteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<StaffwiseSurveyQuestionReport_Vw>> GetStudentSurveyQuestionMarkList(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<StaffwiseSurveyQuestionReport_Vw>(page, pageSize, sortType, sortby, criteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public StudentSurveyReportWOSecNew_Vw GetStudentSurveyReportWOSecById(long Id)
        {
            try
            {
                StudentSurveyReportWOSecNew_Vw staffsurvey = null;
                if (Id > 0)
                {
                    staffsurvey = PSF.Get<StudentSurveyReportWOSecNew_Vw>(Id);
                }
                return staffsurvey;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public StudentSurveyReportNew_Vw GetStudentSurveyReportById(long Id)
        {
            try
            {
                StudentSurveyReportNew_Vw staffsurvey = null;
                if (Id > 0)
                {
                    staffsurvey = PSF.Get<StudentSurveyReportNew_Vw>(Id);
                }
                return staffsurvey;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public StaffWiseStudentSurveyNewResultWOS_Vw StaffWiseStudentSurveyNewResultWOSById(long Id)
        {
            try
            {
                StaffWiseStudentSurveyNewResultWOS_Vw staffsurvey = null;
                if (Id > 0)
                {
                    staffsurvey = PSF.Get<StaffWiseStudentSurveyNewResultWOS_Vw>(Id);
                }
                return staffsurvey;
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion
        #region SurveyQuestionMaster
        public Dictionary<long, IList<SurveyQuestionMaster>> GetSurveyQuestionMasterWithExactAndLikeSearchCriteriaWithCount(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likecriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<SurveyQuestionMaster>(page, pageSize, sortType, sortby, criteria, likecriteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateSurveyQuestionMaster(SurveyQuestionMaster sqm)
        {
            try
            {
                if (sqm != null)
                    PSF.SaveOrUpdate<SurveyQuestionMaster>(sqm);
                else { throw new Exception("Error"); }
                return sqm.SurveyQuestionId;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public SurveyQuestionMaster GetSurveyQuestionMasterBySurveyGroupIdandQuestion(long SurveyGroupId, string SurveyQuestion)
        {
            try
            {
                SurveyQuestionMaster sqm = null;
                sqm = PSF.Get<SurveyQuestionMaster>("SurveyGroupMaster.SurveyGroupId", SurveyGroupId, "SurveyQuestion", SurveyQuestion);
                return sqm;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public SurveyQuestionMaster GetSurveyQuestionMasterById(long Id)
        {
            try
            {
                SurveyQuestionMaster sqm = null;
                if (Id > 0)
                    sqm = PSF.Get<SurveyQuestionMaster>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return sqm;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool DeleteSurveyQuestionMaster(long[] id)
        {
            try
            {
                IList<SurveyQuestionMaster> aptm = PSF.GetListByIds<SurveyQuestionMaster>(id);
                PSF.DeleteAll<SurveyQuestionMaster>(aptm);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region survey group master john naveen
        public long CreateOrUpdateSurveyGroupMaster(SurveyGroupMaster sgm)
        {
            try
            {
                if (sgm != null)
                    PSF.SaveOrUpdate<SurveyGroupMaster>(sgm);
                else { throw new Exception("CallManagement is required and it cannot be null.."); }
                return sgm.SurveyGroupId;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<SurveyGroupMaster>> GetSurveyGroupMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithLikeSearchCriteriaCount<SurveyGroupMaster>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteSurveyGroupMaster(long id)
        {
            try
            {
                SurveyGroupMaster surveymaster = PSF.Get<SurveyGroupMaster>(id);
                PSF.Delete<SurveyGroupMaster>(surveymaster);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteSurveyGroupMaster(long[] id)
        {
            try
            {
                IList<SurveyGroupMaster> surveymaster = PSF.GetListByIds<SurveyGroupMaster>(id);
                PSF.DeleteAll<SurveyGroupMaster>(surveymaster);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public SurveyGroupMaster GetSurveyGroupMasterrByGroupName(string SurveyGroup)
        {
            try
            {
                SurveyGroupMaster SurveyGroupMaster = null;
                if (!string.IsNullOrEmpty(SurveyGroup))
                    SurveyGroupMaster = PSF.Get<SurveyGroupMaster>("SurveyGroup", SurveyGroup);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return SurveyGroupMaster;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        #region SurveyAnswerMaster john naveen
        public Dictionary<long, IList<SurveyAnswerMaster>> GetSurveyAnswerMasterWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithLikeSearchCriteriaCount<SurveyAnswerMaster>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateSurveyAnswerMaster(SurveyAnswerMaster sam)
        {
            try
            {
                if (sam != null)
                    PSF.SaveOrUpdate<SurveyAnswerMaster>(sam);
                else { throw new Exception("Error"); }
                return sam.SurveyAnswerId;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public SurveyAnswerMaster GetSurveyAnswerMasterBySurveyQuestionandMark(string SurveyAnswer, long SurveyQuestionId, long SurveyMark, bool IsPositive)
        {
            try
            {
                SurveyAnswerMaster sam = null;
                sam = PSF.Get<SurveyAnswerMaster>("SurveyAnswer", SurveyAnswer, "SurveyQuestionMaster.SurveyQuestionId", SurveyQuestionId, "SurveyMark", SurveyMark, "IsPositive", IsPositive);
                return sam;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public SurveyAnswerMaster GetSurveyAnswerMasterById(long Id)
        {
            try
            {
                SurveyAnswerMaster sam = null;
                if (Id > 0)
                    sam = PSF.Get<SurveyAnswerMaster>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return sam;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool DeleteSurveyAnswerMaster(long[] id)
        {
            try
            {
                IList<SurveyAnswerMaster> aptm = PSF.GetListByIds<SurveyAnswerMaster>(id);
                PSF.DeleteAll<SurveyAnswerMaster>(aptm);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Biometric Auto EMail
        public IList<StaffDetailsView> GetStaffDetailsListByPreRegNum(long[] PreRegNum)
        {
            try
            {
                IList<StaffDetailsView> StaffDetailsList = PSF.GetList<StaffDetailsView>("PreRegNum", PreRegNum);
                return StaffDetailsList;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IList<StaffDetailsView> GetStaffDetailsListByCampusProgrammeDepartmentAndDesignation(string Campus, string Programme, string Department, string Designation)
        {
            try
            {
                //IList<StaffDetailsView> StaffDetailsList = PSF.GetList<StaffDetailsView>("Campus", Campus, "Programme", Programme, "Department", Department, "Designation", Designation);
                IList<StaffDetailsView> StaffDetailsList = PSF.GetList<StaffDetailsView>("Campus", Campus, "Designation", Designation);
                return StaffDetailsList;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool SendMailFunctionForBioMetric(string BodyofMail, string Campus, string MailBody, string Subject, string RecipientInfo, string[] ReportingManagersMailIds)
        {
            try
            {
                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                string server = ConfigurationManager.AppSettings["CampusEmailType"].ToString();
                criteria.Add("Campus", Campus);
                criteria.Add("Server", server);
                IList<CampusEmailId> campusemaildet = PSF.GetListWithSearchCriteria<CampusEmailId>(0, 99999, string.Empty, string.Empty, criteria);
                if (ReportingManagersMailIds != null && ReportingManagersMailIds.Length > 0)
                {
                    foreach (var EMailItem in ReportingManagersMailIds)
                    {
                        mail.To.Add(EMailItem);
                    }
                }
                mail.Subject = Subject; // st.Subject;
                MailBody = MailBody.Replace("{{CampusmailId}}", campusemaildet.First().EmailId);
                MailBody = MailBody.Replace("{{Ph.No}}", campusemaildet.First().PhoneNumber);
                MailBody = MailBody.Replace("{{DateTime}}", DateTime.Now.ToString());
                MailBody = MailBody.Replace("{{Recipients}}", RecipientInfo);
                MailBody = MailBody.Replace("{{Content}}", BodyofMail);
                mail.Body = MailBody;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient("localhost", 25);
                smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address  
                //Or your Smtp Email ID and Password  
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.EnableSsl = true;
                mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
                smtp.Credentials = new System.Net.NetworkCredential
               (campusemaildet.First().EmailId.ToString(), campusemaildet.First().Password.ToString());
                if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                {
                    try
                    {
                        //ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                        //{
                        //    return true;
                        //};
                        //new Task(() => { smtp.Send(mail); }).Start();
                        //return true;
                        smtp.Send(mail);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("quota"))
                        {
                            mail.From = new MailAddress(campusemaildet.First().AlternateEmailId.ToString());
                            smtp.Credentials = new System.Net.NetworkCredential(campusemaildet.First().AlternateEmailId.ToString(), campusemaildet.First().AlternateEmailIdPassword.ToString());
                            smtp.Send(mail);
                            return true;
                        }
                        else
                        {
                            mail.From = new MailAddress(campusemaildet.First().AlternateEmailId.ToString());
                            smtp.Credentials = new System.Net.NetworkCredential(campusemaildet.First().AlternateEmailId, campusemaildet.First().AlternateEmailIdPassword);
                            smtp.Send(mail);
                            return true;
                        }
                    }
                }
                else { return false; }
            }
            catch (Exception ey)
            {
                ExceptionPolicy.HandleException(ey, "AdmissionPolicy");
                throw ey;
            }
        }
        public long SaveOrUpdateStaff_AutoEmailNotificationLogForAttendance(Staff_AutoEmailNotificationLogForAttendance MailLog)
        {
            try
            {
                if (MailLog != null)
                    PSF.SaveOrUpdate<Staff_AutoEmailNotificationLogForAttendance>(MailLog);
                else { throw new Exception("Mail Log Details is required and it cannot be null.."); }
                return MailLog.Staff_AutoEmailNotificationLogForAttendance_Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void SaveIntoMailLog(string Campus, string MailFor, string stringAttendanceDate)
        {
            Staff_AutoEmailNotificationLogForAttendance MailLogObj = new Staff_AutoEmailNotificationLogForAttendance();
            MailLogObj.Campus = Campus;
            MailLogObj.MailFor = MailFor;
            MailLogObj.AttendanceDate = stringAttendanceDate;
            MailLogObj.LoggedDateAndTime = DateTime.Now;
            SaveOrUpdateStaff_AutoEmailNotificationLogForAttendance(MailLogObj);
        }
        public bool IsMailLogged(string Campus, string MailFor, string AttendanceDate)
        {
            bool IsMailLogged = false;
            Staff_AutoEmailNotificationLogForAttendance MailLoggedObj = new Staff_AutoEmailNotificationLogForAttendance();
            MailLoggedObj = PSF.Get<Staff_AutoEmailNotificationLogForAttendance>("Campus", Campus, "MailFor", MailFor, "AttendanceDate", AttendanceDate);
            if (MailLoggedObj != null)
                IsMailLogged = true;
            return IsMailLogged;
        }
        #endregion

        #region Staff Attendance Report Configuration
        public Staff_AttendanceReportConfiguration GetStaff_AttendanceReportConfigurationByCampusProgrammeDepartmentAndDesignation(string Campus, string Programme, string Department, string Designation)
        {
            try
            {
                Staff_AttendanceReportConfiguration ConfigObj = new Staff_AttendanceReportConfiguration();
                ConfigObj = PSF.Get<Staff_AttendanceReportConfiguration>("Campus", Campus, "Programme", Programme, "Department", Department, "Designation", Designation);
                return ConfigObj;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IList<Staff_AttendanceReportConfiguration> GetStaff_AttendanceReportConfigurationList()
        {
            try
            {
                IList<Staff_AttendanceReportConfiguration> Staff_AttendanceReportConfigurationList = PSF.GetList<Staff_AttendanceReportConfiguration>("IsActive", true);
                return Staff_AttendanceReportConfigurationList;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<long, IList<Staff_AttendanceReportConfiguration>> GetStaff_AttendanceReportConfigurationDetailsListWithPaging(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCount<Staff_AttendanceReportConfiguration>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long SaveOrUpdateStaffAttendanceReportConfigurationByCampusBasedStaffDetails(CampusBasedStaffDetails CampusBasedStaffDetailsObj, string userId)
        {
            if (CampusBasedStaffDetailsObj != null && !string.IsNullOrEmpty(CampusBasedStaffDetailsObj.ReportingDesignation))
            {
                IList<Staff_AttendanceReportConfiguration> ConfigList = new List<Staff_AttendanceReportConfiguration>();
                string[] ReportingDesignationArray = CampusBasedStaffDetailsObj.ReportingDesignation.Split(',');
                foreach (var ReportingDesignationItem in ReportingDesignationArray)
                {
                    Staff_AttendanceReportConfiguration ConfigObj = new Staff_AttendanceReportConfiguration();
                    ConfigObj.Campus = CampusBasedStaffDetailsObj.Campus;
                    ConfigObj.Programme = CampusBasedStaffDetailsObj.Programme;
                    ConfigObj.Department = CampusBasedStaffDetailsObj.Department;
                    ConfigObj.Designation = CampusBasedStaffDetailsObj.Designation;
                    ConfigObj.ReportingLevel = CampusBasedStaffDetailsObj.ReportingLevel;
                    ConfigObj.ReportingDesignation = ReportingDesignationItem;
                    ConfigObj.SubDepartment = CampusBasedStaffDetailsObj.SubDepartment;
                    ConfigObj.CreatedBy = userId;
                    ConfigObj.CreatedDate = DateTime.Now;
                    ConfigObj.ModifiedBy = userId;
                    ConfigObj.ModifiedDate = DateTime.Now;
                    if (IsStaff_AttendanceReportConfigurationAlreadyExist(ConfigObj) == false)
                        //PSF.SaveOrUpdate<Staff_AttendanceReportConfiguration>(ConfigObj);
                        ConfigList.Add(ConfigObj);
                }
                if (ConfigList != null && ConfigList.Count > 0)
                {
                    SaveOrUpdateStaffAttendanceReportConfigurationList(ConfigList);
                    return ConfigList.Count;
                }
                else
                    return 0;
            }
            else
                return 0;
        }
        public void SaveOrUpdateStaffAttendanceReportConfigurationList(IList<Staff_AttendanceReportConfiguration> ConfigList)
        {
            try
            {
                PSF.SaveOrUpdate<Staff_AttendanceReportConfiguration>(ConfigList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool IsStaff_AttendanceReportConfigurationAlreadyExist(Staff_AttendanceReportConfiguration ConfigObj)
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(ConfigObj.Campus)) criteria.Add("Campus", ConfigObj.Campus);
            if (!string.IsNullOrEmpty(ConfigObj.Programme)) criteria.Add("Programme", ConfigObj.Programme);
            if (!string.IsNullOrEmpty(ConfigObj.Department)) criteria.Add("Department", ConfigObj.Department);
            if (!string.IsNullOrEmpty(ConfigObj.Designation)) criteria.Add("Designation", ConfigObj.Designation);
            if (!string.IsNullOrEmpty(ConfigObj.ReportingDesignation)) criteria.Add("ReportingDesignation", ConfigObj.ReportingDesignation);
            if (!string.IsNullOrEmpty(ConfigObj.SubDepartment)) criteria.Add("SubDepartment", ConfigObj.SubDepartment);
            Dictionary<long, IList<Staff_AttendanceReportConfiguration>> ExistingConfigDetails = null;
            ExistingConfigDetails = GetStaff_AttendanceReportConfigurationDetailsListWithPaging(null, 99999, string.Empty, string.Empty, criteria);
            if (ExistingConfigDetails != null && ExistingConfigDetails.FirstOrDefault().Key > 0)
                return true;
            else
                return false;
        }
        public StaffDetailsView GetStaffDetailsViewByPreRegNum(Int32 PreRegNum)
        {
            try
            {
                StaffDetailsView StaffDetailsView = new StaffDetailsView();
                if (PreRegNum > 0)
                    StaffDetailsView = PSF.Get<StaffDetailsView>("PreRegNum", Convert.ToInt64(PreRegNum));
                return StaffDetailsView;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<long, IList<Staff_AttendanceReportConfiguration_Vw>> GetStaff_AttendanceReportConfiguration_VwListWithCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCount<Staff_AttendanceReportConfiguration_Vw>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Dictionary<long, IList<CampusBasedStaffDetails_Vw>> GetCampusBasedStaffDetails_VwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<CampusBasedStaffDetails_Vw>(page, pageSize, sortType, sortby, criteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Staff Attendance Configuration
        public bool AddOrUpdateStaff_AttendanceReportConfigurationByStaffs(long StaffPreRegNum, string Campus, string[] ReportingHeadPreRegNums, string userId)
        {
            bool IsAddOrUpdate = false;
            IList<Staff_AttendanceReportConfigurationByStaffs> ExistingConfigList = new List<Staff_AttendanceReportConfigurationByStaffs>();
            IList<Staff_AttendanceReportConfigurationByStaffs> NewConfigList = new List<Staff_AttendanceReportConfigurationByStaffs>();
            ExistingConfigList = GetStaff_AttendanceReportConfigurationByStaffsByStaffPreRegNum(StaffPreRegNum);
            if (ExistingConfigList != null && ExistingConfigList.Count > 0)
            {
                DeleteStaff_AttendanceReportConfigurationList(ExistingConfigList);
            }
            if (ReportingHeadPreRegNums.Length > 0)
            {
                long[] CurrentReportingLongArray = Array.ConvertAll(ReportingHeadPreRegNums, s => long.Parse(s));
                foreach (var ReportingPreRegNumItem in CurrentReportingLongArray)
                {
                    Staff_AttendanceReportConfigurationByStaffs SaveObj = new Staff_AttendanceReportConfigurationByStaffs();
                    SaveObj.StaffPreRegNum = StaffPreRegNum;
                    SaveObj.ReportingHeadPreRegNum = ReportingPreRegNumItem;
                    SaveObj.Campus = Campus;
                    SaveObj.CreatedBy = userId;
                    SaveObj.CreatedDate = DateTime.Now;
                    SaveObj.ModifiedBy = userId;
                    SaveObj.ModifiedDate = DateTime.Now;
                    NewConfigList.Add(SaveObj);
                }
                if (NewConfigList.Count > 0)
                {
                    SaveOrUpdateStaff_AttendanceReportConfigurationList(NewConfigList);
                    IsAddOrUpdate = true;
                }
            }
            return IsAddOrUpdate;
        }
        public IList<Staff_AttendanceReportConfigurationByStaffs> GetStaff_AttendanceReportConfigurationByStaffsByStaffPreRegNum(long StaffPreRegNum)
        {
            try
            {
                IList<Staff_AttendanceReportConfigurationByStaffs> Staff_AttendanceReportConfigurationByStaffsList = PSF.GetList<Staff_AttendanceReportConfigurationByStaffs>("StaffPreRegNum", StaffPreRegNum);
                return Staff_AttendanceReportConfigurationByStaffsList;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void DeleteStaff_AttendanceReportConfigurationList(IList<Staff_AttendanceReportConfigurationByStaffs> ConfigList)
        {
            if (ConfigList != null && ConfigList.Count > 0)
            {
                PSF.DeleteAll<Staff_AttendanceReportConfigurationByStaffs>(ConfigList);
            }
            else { }
        }
        public void SaveOrUpdateStaff_AttendanceReportConfigurationList(IList<Staff_AttendanceReportConfigurationByStaffs> ConfigList)
        {
            try
            {
                if (ConfigList != null && ConfigList.Count > 0)
                    PSF.SaveOrUpdate<Staff_AttendanceReportConfigurationByStaffs>(ConfigList);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public IList<Staff_AttendanceReportConfigurationByStaffs> GetStaff_AttendanceReportConfigurationsListBasedOnReportingHeadPreRegNums(long[] ReportingHeadPreRegNums)
        {
            try
            {
                IList<Staff_AttendanceReportConfigurationByStaffs> Staff_AttendanceReportConfigurationList = PSF.GetList<Staff_AttendanceReportConfigurationByStaffs>("ReportingHeadPreRegNum", ReportingHeadPreRegNums);
                return Staff_AttendanceReportConfigurationList;
            }
            catch (Exception)
            {

                throw;
            }
        }
        //public void SaveOrUpdateStaff_AttendanceReportConfigurationByStaffs(Staff_AttendanceReportConfiguration_Vw ConfigObj,string UserId)
        //{
        //    try
        //    {
        //        if (SaveObj != null)
        //        { 
        //            if(SaveObj.
        //        }
        //       // Staff_AttendanceReportConfigurationByStaffs SaveObj = new Staff_AttendanceReportConfigurationByStaffs();
        //        SaveObj.StaffPreRegNum = StaffPreRegNum;
        //        SaveObj.ReportingHeadPreRegNum = ReportingHeadPreRegNum;
        //        SaveObj.Campus = Campus;
        //        SaveObj.CreatedBy = userId;
        //        SaveObj.CreatedDate = DateTime.Now;
        //        SaveObj.ModifiedBy = userId;
        //        SaveObj.ModifiedDate = DateTime.Now;
        //        PSF.SaveOrUpdate<Staff_AttendanceReportConfigurationByStaffs>(SaveObj);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        public Staff_AttendanceReportConfigurationByStaffs GetStaff_AttendanceReportConfigurationByStaffsById(long Staff_AttendanceReportConfig_Id)
        {
            try
            {
                Staff_AttendanceReportConfigurationByStaffs ConfigObj = new Staff_AttendanceReportConfigurationByStaffs();
                if (ConfigObj != null)
                    ConfigObj = PSF.Get<Staff_AttendanceReportConfigurationByStaffs>("Staff_AttendanceReportConfig_Id", Staff_AttendanceReportConfig_Id);
                return ConfigObj;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Staff_AttendanceReportConfiguration_Vw GetStaff_AttendanceReportConfiguration_VwById(long Id)
        {
            try
            {
                Staff_AttendanceReportConfiguration_Vw ConfigObj = new Staff_AttendanceReportConfiguration_Vw();
                if (ConfigObj != null)
                    ConfigObj = PSF.Get<Staff_AttendanceReportConfiguration_Vw>("Id", Id);
                return ConfigObj;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public long SaveOrUpdateStaff_AttendanceReportConfigurationByStaffs(Staff_AttendanceReportConfigurationByStaffs ConfigObj)
        {
            try
            {
                if (ConfigObj != null)
                    PSF.SaveOrUpdate<Staff_AttendanceReportConfigurationByStaffs>(ConfigObj);
                else { throw new Exception("StaffManagement is required and it cannot be null.."); }
                return ConfigObj.Staff_AttendanceReportConfig_Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Staff_AttendanceReportConfigurationByStaffs GetStaff_AttendanceReportConfigurationByStaffsByStaffPreRegNumAndReportPreRegNum(long StaffPreRegNum, long ReportingHeadPreRegNum)
        {
            try
            {
                Staff_AttendanceReportConfigurationByStaffs ConfigObj = new Staff_AttendanceReportConfigurationByStaffs();
                if (ConfigObj != null)
                    ConfigObj = PSF.Get<Staff_AttendanceReportConfigurationByStaffs>("StaffPreRegNum", StaffPreRegNum, "ReportingHeadPreRegNum", ReportingHeadPreRegNum);
                return ConfigObj;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<long, IList<Staff_AttendanceReportConfigurationByStaffs>> GetStaff_AttendanceReportConfigurationByStaffsListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithLikeSearchCriteriaCount<Staff_AttendanceReportConfigurationByStaffs>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region SurveyConfiguration
        public Dictionary<long, IList<SurveyConfiguration>> GetSurveyConfigurationWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<SurveyConfiguration>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateSurveyConfiguration(SurveyConfiguration surveyconfig)
        {
            try
            {
                if (surveyconfig != null)
                    PSF.SaveOrUpdate<SurveyConfiguration>(surveyconfig);
                else { throw new Exception("Error"); }
                return surveyconfig.SurveyConfigurationId;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<long, IList<SurveyConfiguration>> GetSurveyConfigurationListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<SurveyConfiguration>(page, pageSize, sortType, sortby, criteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<SurveyConfiguration> SaveOrUpdateSurveyConfigurationByList(IList<SurveyConfiguration> surveyconfiguration)
        {
            try
            {
                if (surveyconfiguration.Count > 0)
                {
                    PSF.SaveOrUpdate<SurveyConfiguration>(surveyconfiguration);
                }
                else
                { throw new Exception("All Fields are required and it cannot be null.."); }
                return surveyconfiguration;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<long, IList<SurveyConfiguration_vw>> GetSurveyConfiguration_vwWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<SurveyConfiguration_vw>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region Staff Programme Master by john naveen
        public Dictionary<long, IList<StaffProgrammeMaster>> GetStaffProgrammeMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithLikeSearchCriteriaCount<StaffProgrammeMaster>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateStaffProgrammeMaster(StaffProgrammeMaster spm)
        {
            try
            {
                if (spm != null)
                    PSF.SaveOrUpdate<StaffProgrammeMaster>(spm);
                else { throw new Exception("Error"); }
                return spm.StaffProgrammeMatserId;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool DeleteStaffProgrammeMaster(long[] id)
        {
            try
            {
                IList<StaffProgrammeMaster> spm = PSF.GetListByIds<StaffProgrammeMaster>(id);
                PSF.DeleteAll<StaffProgrammeMaster>(spm);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public StaffProgrammeMaster GetStaffProgrammeMasterByCampusAndStaffType(string Campus, string StaffType, string ProgrammeName, bool IsActive)
        {
            try
            {
                StaffProgrammeMaster spm = null;
                spm = PSF.Get<StaffProgrammeMaster>("Campus", Campus, "StaffType", StaffType, "ProgrammeName", ProgrammeName, "IsActive", IsActive);
                return spm;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public StaffProgrammeMaster GetStaffProgrammeMasterByStaffProgrammeMasterId(long Id)
        {
            try
            {
                StaffProgrammeMaster spm = null;
                if (Id > 0)
                    spm = PSF.Get<StaffProgrammeMaster>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return spm;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IList<StaffProgrammeMaster> SaveOrUpdateStaffProgrammeMasterByList(IList<StaffProgrammeMaster> StaffProgrammeMaster)
        {
            try
            {
                if (StaffProgrammeMaster.Count > 0)
                {
                    PSF.SaveOrUpdate<StaffProgrammeMaster>(StaffProgrammeMaster);
                }
                else
                { throw new Exception("All Fields are required and it cannot be null.."); }
                return StaffProgrammeMaster;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        #region SurveyMaster Krishna_14062017

        public Dictionary<long, IList<SurveyMaster>> GetSurveyDetailsListWithPaging(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<SurveyMaster>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public SurveyMaster GetSurveyName(string lSurveyName)
        {
            try
            {
                SurveyMaster objSurveyMaster = null;
                if (!string.IsNullOrEmpty(lSurveyName))
                    objSurveyMaster = PSF.Get<SurveyMaster>("SurveyName", lSurveyName);

                else { throw new Exception("Id is required and it cannot be 0"); }
                return objSurveyMaster;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public long CreateOrUpdateSurveyMaster(SurveyMaster objSurveyMaster)
        {
            try
            {
                if (objSurveyMaster != null)
                    PSF.SaveOrUpdate<SurveyMaster>(objSurveyMaster);
                else { throw new Exception("Error"); }
                return objSurveyMaster.SurveyId;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public SurveyMaster GetAssetBrandMasterByBrandMasterId(long SurveyId)
        {
            try
            {
                SurveyMaster abm = null;
                if (SurveyId > 0)
                    abm = PSF.Get<SurveyMaster>(SurveyId);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return abm;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool DeleteSurveyMaster(long[] id)
        {
            try
            {
                IList<SurveyMaster> abm = PSF.GetListByIds<SurveyMaster>(id);
                PSF.DeleteAll<SurveyMaster>(abm);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
        #region SurveyResult by Prabakaran
        public Dictionary<long, IList<StaffWiseSurveyNewResult_Vw>> GetStaffWiseSurveyList(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<StaffWiseSurveyNewResult_Vw>(page, pageSize, sortType, sortby, criteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<SurveyReportNew_Vw>> GetSurveyReportNew_VwStudentCountList(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<SurveyReportNew_Vw>(page, pageSize, sortType, sortby, criteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public SurveyReportNew_Vw GetSurveyReportById(long Id)
        {
            try
            {
                SurveyReportNew_Vw staffsurvey = null;
                if (Id > 0)
                {
                    staffsurvey = PSF.Get<SurveyReportNew_Vw>(Id);
                }
                return staffsurvey;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public Dictionary<long, IList<StaffwiseSurveyQuestionReportNew_Vw>> GetStudentSurveyQuestionMarkNewList(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<StaffwiseSurveyQuestionReportNew_Vw>(page, pageSize, sortType, sortby, criteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public StaffWiseSurveyNewResult_Vw GetStaffWiseSurveyNewResultById(long Id)
        {
            try
            {
                StaffWiseSurveyNewResult_Vw staffsurvey = null;
                if (Id > 0)
                {
                    staffsurvey = PSF.Get<StaffWiseSurveyNewResult_Vw>(Id);
                }
                return staffsurvey;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public Dictionary<long, IList<StaffWiseSurveyNewResultWOS_Vw>> GetStaffWiseSurveyWOSList(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<StaffWiseSurveyNewResultWOS_Vw>(page, pageSize, sortType, sortby, criteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public StaffWiseSurveyNewResultWOS_Vw StaffWiseSurveyNewResultWOSById(long Id)
        {
            try
            {
                StaffWiseSurveyNewResultWOS_Vw staffsurvey = null;
                if (Id > 0)
                {
                    staffsurvey = PSF.Get<StaffWiseSurveyNewResultWOS_Vw>(Id);
                }
                return staffsurvey;
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion
        public Dictionary<long, IList<SurveyReportNew_SP>> GetSurveyReportNew_SPListbySP(string Campus, string Grade, string Section, string AcademicYear, string CategoryName, long StaffEvaluationCategoryId, long StaffPreRegNumber, long CampusBasedStaffDetails_Id)
        {
            try
            {
                return PSF.ExecuteStoredProcedurewithOptionalParametersByDictonary<SurveyReportNew_SP>("GetSurveyReportNew_SPList",
                         new[] { new SqlParameter("Campus", Campus),
                             new SqlParameter("Grade", Grade),
                             new SqlParameter("Section", Section),
                             new SqlParameter("AcademicYear", AcademicYear),
                             new SqlParameter("CategoryName", CategoryName),
                             new SqlParameter("StaffEvaluationCategoryId", StaffEvaluationCategoryId),
                             new SqlParameter("StaffPreRegNumber",StaffPreRegNumber),                                                       
                             new SqlParameter("CampusBasedStaffDetails_Id",CampusBasedStaffDetails_Id),   
                    });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region Staff_WorkingDaysMaster
        public Dictionary<long, IList<Staff_WorkingDaysMaster>> GetStaff_WorkingDaysMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<Staff_WorkingDaysMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long SaveOrUpdateStaff_WorkingDaysMaster(Staff_WorkingDaysMaster workingMasterObj)
        {
            try
            {
                if (workingMasterObj != null)
                    PSF.SaveOrUpdate<Staff_WorkingDaysMaster>(workingMasterObj);
                else { throw new Exception("Error"); }
                return workingMasterObj.Staff_WorkingDaysMaster_Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool DeleteStaff_WorkingDaysMaster(long[] id)
        {
            try
            {
                IList<Staff_WorkingDaysMaster> workingMasterObj = PSF.GetListByIds<Staff_WorkingDaysMaster>(id);
                PSF.DeleteAll<Staff_WorkingDaysMaster>(workingMasterObj);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Staff_WorkingDaysMaster GetStaff_WorkingDaysMasterByCampusAndStaffType(string Campus, string StaffType, long Month, long Year)
        {
            try
            {
                Staff_WorkingDaysMaster workingMasterObj = null;
                workingMasterObj = PSF.Get<Staff_WorkingDaysMaster>("Month", Month, "Year", Year, "Campus", Campus, "StaffType", StaffType);
                return workingMasterObj;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Staff_WorkingDaysMaster GetStaff_WorkingDaysMasterByStaff_WorkingDaysMaster_Id(long Staff_WorkingDaysMaster_Id)
        {
            try
            {
                Staff_WorkingDaysMaster workingMasterObj = null;
                workingMasterObj = PSF.Get<Staff_WorkingDaysMaster>("Staff_WorkingDaysMaster_Id", Staff_WorkingDaysMaster_Id);
                return workingMasterObj;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IList<Staff_WorkingDaysMaster> SaveOrUpdateStaff_WorkingDaysMasterByList(IList<Staff_WorkingDaysMaster> StaffWorkingDaysMaster)
        {
            try
            {
                if (StaffWorkingDaysMaster.Count > 0)
                {
                    PSF.SaveOrUpdate<Staff_WorkingDaysMaster>(StaffWorkingDaysMaster);
                }
                else
                { throw new Exception("All Fields are required and it cannot be null.."); }
                return StaffWorkingDaysMaster;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion        
        #region Staff Attendance Change details
        public Dictionary<long, IList<Staff_AttendanceChangeDetails>> GetStaff_AttendanceChangeDetailsListWithCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithLikeSearchCriteriaCount<Staff_AttendanceChangeDetails>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public long SaveOrUpdateStaff_AttendanceChangeDetails(Staff_AttendanceChangeDetails Staff_AttendanceChangeDetails)
        {
            try
            {
                if (Staff_AttendanceChangeDetails != null)
                    PSF.SaveOrUpdate<Staff_AttendanceChangeDetails>(Staff_AttendanceChangeDetails);
                else { throw new Exception("Error"); }
                return Staff_AttendanceChangeDetails.Staff_AttendanceChangeDetails_Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Staff_AttendanceChangeDetails GetStaff_AttendanceChangeDetailsByPreRegNumAndMonthYear(long PreRegNum, long Month, long Year, bool IsActive)
        {
            try
            {
                Staff_AttendanceChangeDetails sacd = null;
                sacd = PSF.Get<Staff_AttendanceChangeDetails>("PreRegNum", PreRegNum, "Month", Month, "Year", Year, "IsActive", IsActive);
                return sacd;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        #region Staff Holidays Master
        public Dictionary<long, IList<StaffHolidaysMaster>> GetStaffHolidaysMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<StaffHolidaysMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long SaveOrUpdateStaffHolidaysMaster(StaffHolidaysMaster StaffHolidaysMaster)
        {
            try
            {
                if (StaffHolidaysMaster != null)
                    PSF.SaveOrUpdate<StaffHolidaysMaster>(StaffHolidaysMaster);
                else { throw new Exception("Error"); }
                return StaffHolidaysMaster.StaffHolidaysMaster_Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool DeleteStaffHolidaysMaster(long[] id)
        {
            try
            {
                IList<StaffHolidaysMaster> staffHolidaysMaster = PSF.GetListByIds<StaffHolidaysMaster>(id);
                PSF.DeleteAll<StaffHolidaysMaster>(staffHolidaysMaster);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public StaffHolidaysMaster GetStaffHolidaysMasterByAcademicYearAndMonth(long Year, long MonthNumber, string HolidayType, string Campus)
        {
            try
            {
                StaffHolidaysMaster StaffHolidaysMaster = null;
                StaffHolidaysMaster = PSF.Get<StaffHolidaysMaster>("Year", Year, "MonthNumber", MonthNumber, "HolidayType", HolidayType, "Campus", Campus);
                return StaffHolidaysMaster;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public StaffHolidaysMaster GetStaffHolidaysMasterByAcademicYearAndMonthCampus(long Year, long Month, string Campus)
        {
            try
            {
                StaffHolidaysMaster StaffHolidaysMaster = null;
                StaffHolidaysMaster = PSF.Get<StaffHolidaysMaster>("Year", Year, "MonthNumber", Month, "Campus", Campus);
                return StaffHolidaysMaster;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        #region Casual leaves Balance Details
        public Dictionary<long, IList<Staff_AttendanceCLDetails>> GetStaff_AttendanceCLDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<Staff_AttendanceCLDetails>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Staff_AttendanceCLDetails GetStaff_AttendanceCLDetailsByPreRegNum(long Month, long Year, long PreRegNum, bool IsActive)
        {
            try
            {
                Staff_AttendanceCLDetails StaffAttendanceCLDetails = null;
                StaffAttendanceCLDetails = PSF.Get<Staff_AttendanceCLDetails>("Month", Month, "Year", Year, "PreRegNum", PreRegNum, "IsActive", IsActive);
                return StaffAttendanceCLDetails;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public long SaveOrUpdateStaff_AttendanceCLDetails(Staff_AttendanceCLDetails StaffAttendanceCLDetails)
        {
            try
            {
                if (StaffAttendanceCLDetails != null)
                    PSF.SaveOrUpdate<Staff_AttendanceCLDetails>(StaffAttendanceCLDetails);
                else { throw new Exception("Error"); }
                return StaffAttendanceCLDetails.Staff_AttendanceCLBalance_Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IList<Staff_AttendanceCLDetails> SaveOrUpdateStaffAttendanceCLDetailsByList(IList<Staff_AttendanceCLDetails> StaffAttendanceCLDetails)
        {
            try
            {
                if (StaffAttendanceCLDetails.Count > 0)
                {
                    PSF.SaveOrUpdate<Staff_AttendanceCLDetails>(StaffAttendanceCLDetails);
                }
                else
                { throw new Exception("All Fields are required and it cannot be null.."); }
                return StaffAttendanceCLDetails;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        #region Casual Leaves Master
        public Dictionary<long, IList<CLDetailsMaster>> GetCLDetailsMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<CLDetailsMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long SaveOrUpdateStaff_AttendanceCLDetails(CLDetailsMaster clDetailsMaster)
        {
            try
            {
                if (clDetailsMaster != null)
                    PSF.SaveOrUpdate<CLDetailsMaster>(clDetailsMaster);
                else { throw new Exception("Error"); }
                return clDetailsMaster.CLDetails_Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public CLDetailsMaster GetCLDetailsMasterByPreRegNum(long Month, long Year, long PreRegNum)
        {
            try
            {
                CLDetailsMaster CLDetailsMaster = null;
                CLDetailsMaster = PSF.Get<CLDetailsMaster>("Month", Month, "Year", Year, "PreRegNum", PreRegNum);
                return CLDetailsMaster;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IList<CLDetailsMaster> SaveOrUpdateCLDetailsMasterByList(IList<CLDetailsMaster> clDetailsmaster)
        {
            try
            {
                if (clDetailsmaster.Count > 0)
                {
                    PSF.SaveOrUpdate<CLDetailsMaster>(clDetailsmaster);
                }
                else
                { throw new Exception("All Fields are required and it cannot be null.."); }
                return clDetailsmaster;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public CLDetailsMaster GetCLDetailsMasterByMonthYear(long Month, long Year)
        {
            try
            {
                CLDetailsMaster CLDetailsMaster = null;
                CLDetailsMaster = PSF.Get<CLDetailsMaster>("Month", Month, "Year", Year);
                return CLDetailsMaster;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        #region Staff Opening Balance Generate count
        public long SaveOrUpdateStaffAttendanceOpeningBalanaceGenerateCount(Staff_AttendanceOpeningBalanceGenerateCount obj)
        {
            try
            {
                if (obj != null)
                    PSF.SaveOrUpdate<Staff_AttendanceOpeningBalanceGenerateCount>(obj);
                else { throw new Exception("Error"); }
                return obj.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Staff_AttendanceOpeningBalanceGenerateCount GetStaffAttendanceOpeningBalanceGenerateCountByMonth(long Month, long Year)
        {
            try
            {
                Staff_AttendanceOpeningBalanceGenerateCount obj = null;
                obj = PSF.Get<Staff_AttendanceOpeningBalanceGenerateCount>("LastMonth", Month, "LastYear", Year);
                return obj;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        public StaffDetails GetStaffDetailsViewByPreRegNumAndStatus(Int32 PreRegNum, string Status)
        {
            try
            {
                StaffDetails StaffDetails = new StaffDetails();
                if (PreRegNum > 0)
                    StaffDetails = PSF.Get<StaffDetails>("PreRegNum", PreRegNum, "Status", Status);
                return StaffDetails;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public StaffDetailsView GetStaffDetailsViewByStatus(Int64 PreRegNum)
        {
            try
            {
                StaffDetailsView StaffDetailsView = new StaffDetailsView();
                if (PreRegNum > 0)
                    StaffDetailsView = PSF.Get<StaffDetailsView>("PreRegNum", PreRegNum);
                return StaffDetailsView;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #region Staff Attendnace New Status
        public Dictionary<long, IList<StaffAttendanceNewStatus>> GetStaffAttendanceNewStatusListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<StaffAttendanceNewStatus>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long SaveOrUpdateStaffAttendanceNewStatus(StaffAttendanceNewStatus obj)
        {
            try
            {
                if (obj != null)
                    PSF.SaveOrUpdate<StaffAttendanceNewStatus>(obj);
                else { throw new Exception("Error"); }
                return obj.StaffStatus_Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public StaffAttendanceNewStatus GetStaffAttendanceNewStatusByPreRegNum(long PreRegNum)
        {
            try
            {
                StaffAttendanceNewStatus obj = new StaffAttendanceNewStatus();
                if (PreRegNum > 0)
                    obj = PSF.Get<StaffAttendanceNewStatus>("PreRegNum", PreRegNum);
                return obj;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        #region Staff_AttendanceConfigurationsReport
        public Dictionary<long, IList<Staff_AttendanceConfigurationsReport_Vw>> GetStaff_AttendanceConfigurationsReport_VwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> ExactCriteria, Dictionary<string, object> LikeCriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<Staff_AttendanceConfigurationsReport_Vw>(page, pageSize, sortType, sortby, ExactCriteria, LikeCriteria);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IList<Staff_AttendanceConfigurationsReport_Vw> GetAttendanceConfigurationsListByIdNumberToEmployeeCode(string[] IdNumberToEmployeeCodeArray)
        {
            try
            {
                IList<Staff_AttendanceConfigurationsReport_Vw> AttendanceConfigurationsList = new List<Staff_AttendanceConfigurationsReport_Vw>();
                Dictionary<string, object> ExactCriteria = new Dictionary<string, object>();
                Dictionary<string, object> LikeCriteria = new Dictionary<string, object>();
                if (IdNumberToEmployeeCodeArray != null && IdNumberToEmployeeCodeArray.Length > 0)
                    ExactCriteria.Add("IdNumberToEmployeeCode", IdNumberToEmployeeCodeArray);
                Dictionary<long, IList<Staff_AttendanceConfigurationsReport_Vw>> AttendanceConfigurationsDetails = GetStaff_AttendanceConfigurationsReport_VwListWithPagingAndCriteria(0, 10, string.Empty, string.Empty, ExactCriteria, LikeCriteria);
                if (AttendanceConfigurationsDetails != null && AttendanceConfigurationsDetails.FirstOrDefault().Key > 0)
                    AttendanceConfigurationsList = (from u in AttendanceConfigurationsDetails.FirstOrDefault().Value
                                                    select u).Distinct().ToList();
                return AttendanceConfigurationsList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public bool UpdateStaffBiometricIdByIdNumberToEmployeeCode(string[] IdNumberToEmployeeCodeArray)
        {
            try
            {
                bool ReturnValue = false;
                IList<Staff_AttendanceConfigurationsReport_Vw> AttendanceConfigurationsList = new List<Staff_AttendanceConfigurationsReport_Vw>();
                AttendanceConfigurationsList = GetAttendanceConfigurationsListByIdNumberToEmployeeCode(IdNumberToEmployeeCodeArray);
                if (AttendanceConfigurationsList != null && AttendanceConfigurationsList.Count > 0)
                {
                    foreach (Staff_AttendanceConfigurationsReport_Vw AttendanceConfigurationsObj in AttendanceConfigurationsList)
                    {
                        StaffDetailsView StaffDetailsViewObj = new StaffDetailsView();
                        StaffDetailsViewObj = GetStaffDetailsViewByPreRegNum(Convert.ToInt32(AttendanceConfigurationsObj.PreRegNum));
                        if (StaffDetailsViewObj != null)
                        {
                            StaffDetailsViewObj.StaffBioMetricId = AttendanceConfigurationsObj.EmployeeId;
                            CreateOrUpdateStaffDetailsView(StaffDetailsViewObj);
                        }
                    }
                }
                return ReturnValue;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region CandidateStatusChange
        public Dictionary<long, IList<CandidateDtls>> GetCandidateList_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<CandidateDtls>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long SaveOrUpdateCandidateList_vw(CandidateDtls obj)
        {
            try
            {
                if (obj != null)
                    PSF.SaveOrUpdate<CandidateDtls>(obj);
                else { throw new Exception("Error"); }
                return obj.PreRegNum;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public CandidateDtls GetCandidateDtlsId(long Id)
        {
            try
            {
                CandidateDtls objCandidateDtls = null;
                if (Id > 0)
                    objCandidateDtls = PSF.Get<CandidateDtls>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return objCandidateDtls;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public long CreateOrUpdateCandidateDtls(CandidateDtls sd)
        {
            try
            {
                if (sd != null)
                    PSF.SaveOrUpdate<CandidateDtls>(sd);
                else { throw new Exception("StaffManagement is required and it cannot be null.."); }
                return sd.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

    }
}
