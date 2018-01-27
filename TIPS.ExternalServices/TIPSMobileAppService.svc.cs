using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using CustomAuthentication;
using TIPS.Component;
using TIPS.Entities;
using TIPS.Entities.AdmissionEntities;
using TIPS.Entities.ParentPortalEntities;
using TIPS.ExternalServiceContract;
//using System.Web.Mvc;
using TIPS.Entities.Assess;
using TIPS.Service;
using TIPS.ServiceContract;
namespace TIPS.ExternalServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class TIPSMobileAppService : ITIPSMobileAppService
    {
        public bool ValidateWebServiceUser(string uid, string pwd, string roleCode)
        {
            try
            {
                if (uid == "WSU" && pwd == "p@ssw0rd" && roleCode == "MobileApp")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public LoginHistory GetLoginDetails(User u)
        {
            LoginHistory s = new LoginHistory();
            //System.Web.HttpBrowserCapabilitiesBase browser = Request.Browser;
            s.UserId = u.UserId;
            s.UserType = u.UserType;
            s.TimeIn = DateTime.Now;
            //s.IPAddress = Request.UserHostAddress;
            s.BrowserName = "MobileApp";
            //s.BrowserVersion = browser.Version;
            //s.Platform = browser.Platform;
            //s.BrowserType = browser.Type;
            return s;
        }
        public bool Login(string UserId, string Password)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(UserId) && !string.IsNullOrWhiteSpace(Password))
                {
                    TIPS.Component.UserBC UserBC = new Component.UserBC();
                    TIPS.Entities.User User = UserBC.GetUserByUserId(UserId);

                    PassworAuth PA = new PassworAuth();
                    if (User.IsActive == false)
                    {
                        return false;
                    }
                    else
                    {
                        if (Password == PA.base64Decode2(User.Password))
                        {
                            LoginHistory s = GetLoginDetails(User);
                            UserBC.CreateOrUpdateSession(s);
                            return true;
                        }
                    }
                }
                return false;
            }

            catch (Exception ex)
            {
                //ExceptionPolicy.HandleException(ex, "CustomAccountPolicy");
                throw ex;
            }
        }
        public User LoginAndGetUser(string UserId, string Password)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(UserId) && !string.IsNullOrWhiteSpace(Password))
                {
                    TIPS.Component.UserBC UserBC = new Component.UserBC();
                    TIPS.Entities.User User = UserBC.GetUserByUserId(UserId);

                    PassworAuth PA = new PassworAuth();
                    if (User.IsActive == false)
                    {
                        return null;
                    }
                    else
                    {
                        if (Password == PA.base64Decode2(User.Password))
                        {
                            LoginHistory s = GetLoginDetails(User);
                            UserBC.CreateOrUpdateSession(s);
                            return User;
                        }
                    }
                }
                return null;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string[] GetIssueGroupMasterListWithPagingAndCriteria()
        {
            try
            {
                TIPS.Component.MastersBC MastersBC = new Component.MastersBC();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("ParentCode","1");
                Dictionary<long, IList<IssueGroupMaster>> dicValue = MastersBC.GetIssueGroupMasterListWithPagingAndCriteria(0, 1000, "", "", criteria);
                if (dicValue != null && dicValue.Count > 0 && dicValue.FirstOrDefault().Value != null)
                {
                    return (from v in dicValue.FirstOrDefault().Value select v.IssueGroup).ToArray<string>();
                    //return issgrp.ToArray<string>();
                }
                else return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string[] GetIssueTypeMasterListWithPagingAndCriteria(string IssueGroup)
        {
            try
            {
                if (!string.IsNullOrEmpty(IssueGroup))
                {
                    TIPS.Component.MastersBC MastersBC = new Component.MastersBC();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("IssueGroup", IssueGroup);
                    Dictionary<long, IList<IssueTypeMaster>> dicValue = MastersBC.GetIssueTypeMasterListWithPagingAndCriteria(0, 1000, "", "", criteria);
                    if (dicValue != null && dicValue.Count > 0 && dicValue.FirstOrDefault().Value != null)
                    {
                        return (from v in dicValue.FirstOrDefault().Value select v.IssueType).ToArray<string>();
                    }
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public CallManagementWrapper IssueManagementList(string userId, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                ParentPortalBC ParentPortalBC = new ParentPortalBC();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                string performerName = userId;// (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                if (!string.IsNullOrWhiteSpace(sord) && sord == "desc")
                    sord = "Desc";
                else
                    sord = "Asc";
                string stNewId;
                stNewId = userId.Substring(0, userId.Length - 1);
                if (!string.IsNullOrEmpty(stNewId))
                {
                    StudentTemplateView st = ParentPortalBC.GetStudentTemplateViewByStNewId(stNewId);
                    if (st != null)
                    {
                        string StudentNumber = st.NewId;
                        long StudPreRegNum = st.PreRegNum;
                        criteria.Add("StudentNumber", StudentNumber);
                    }
                }

                criteria.Add("UserInbox", performerName);
                Dictionary<long, IList<CallManagementView>> piDet = ParentPortalBC.GetCallManagementViewParentIssueMgt(page - 1, rows, sidx, sord, criteria);
                CallManagementWrapper CallManagementWrapper = new CallManagementWrapper();
                CallManagementWrapper.count = piDet.FirstOrDefault().Key;
                CallManagementWrapper.list = piDet.FirstOrDefault().Value;
                return CallManagementWrapper;
            }
            catch (Exception ex)
            {
                //ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw ex;
            }
        }

        public long SaveOrUpdateCallManagement(CallManagement CallManagement, string TemplateName, string userId)
        {
            try
            {
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                ProcessFlowBC.StartCallManagement(CallManagement, TemplateName, userId);
                CallManagement = ProcessFlowBC.GetCallManagementById(CallManagement.Id);
                ProcessFlowBC.CompleteActivityCallManagement(CallManagement, TemplateName, userId, "LogIssue", false);
                return CallManagement.Id;
            }
            catch (Exception ex)
            {
                //ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw ex;
            }
        }

        public StudentTemplateView GetStudentDetails(string userId)
        {
            try
            {
                var stNewID = userId.Substring(1);
                ParentPortalBC ParentPortalBC = new ParentPortalBC();
                StudentTemplateView st = ParentPortalBC.GetStudentTemplateViewByStNewId(stNewID);
                return st;
            }
            catch (Exception ex)
            {
                //ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw ex;
            }
        }

        public Dictionary<long, IList<Notification>> NoteGeneral(string Campus)
        {
            try
            {
                int? page = 1;
                string[] pubToarray = { "General", "Parent" };
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("NoteType", "General");
                criteria.Add("Valid", "1");
                criteria.Add("Campus", Campus);
                ParentPortalBC piBC = new ParentPortalBC();
                Dictionary<long, IList<Notification>> noteList = piBC.GetNoteTypesearchCriteriaAlias(page - 1, 9999, "Id", "desc", "PublishTo", pubToarray, criteria, null);
                if (noteList != null && noteList.Count > 0 && noteList.First().Value != null)
                    return noteList;
                else
                    return null;
            }
            catch (Exception ex)
            {
                //ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw ex;
            }
        }

        public Notification GetGeneralNotificationByNotePreId(long NotePreId)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                Notification noti = ppBC.GetGeneralNotificationByNotePreId(NotePreId);
                return noti;
            }
            catch (Exception)
            {
                //ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw;
            }
            finally { }
        }

        public IList<Notification> IndividualNotificationList(string UserId, string Campus)
        {
            try
            {
                string cUserId = UserId.Substring(1);
                string[] notetypearray = { "GradeLevel", "Individual" };
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("PublishTo", "Parent");
                criteria.Add("Valid", "1");
                criteria.Add("Campus", Campus);
                ParentPortalBC piBC = new ParentPortalBC();
                Dictionary<long, IList<Notification>> noteList = piBC.GetNoteTypesearchCriteriaAlias(0, 9999, "Id", "desc", "NoteType", notetypearray, criteria, null);
                if (noteList != null && noteList.Count > 0 && noteList.First().Value != null)
                {
                    IList<Notification> NoteList = (from item in noteList.First().Value
                                                    where item.NewIds != null && item.NewIds.Contains(cUserId)
                                                    select item).ToList();
                    if (NoteList.Count > 0)
                    {                      
                        return NoteList;
                    }
                    else
                        return null;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                //ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw ex;
            }
        }

        public Notification GetParentNotificationByViewId(long viewId)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                Notification noti = ppBC.GetParentNotificationByViewId(viewId);
                return noti;
            }
            catch (Exception)
            {
                //ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw;
            }
            finally { }
        }

        public IList<NoteAttachment> GetDocListById(long dId)
        {
            try
            {
                ParentPortalBC piBC = new ParentPortalBC();
                IList<NoteAttachment> dList = piBC.GetDocListById(dId);
                return dList;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        #region Assess360
        public Assess360StudentMarks Assess360(string UserId)
        {
            string StudentId = UserId.Substring(1);
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("IdNo", StudentId);
            DateTime DateNow = DateTime.Now;
            string acayear = "";
            if (DateNow.Month >= 5)
            {
                acayear = DateNow.Year.ToString() + "-" + (DateNow.Year + 1).ToString();
                criteria.Add("AcademicYear", acayear);
            }
            Assess360BC aBC = new Assess360BC();
            Dictionary<long, IList<Assess360>> Assess360List = aBC.GetAssess360ListWithPagingAndCriteria(0, 9999, "", "", criteria);
            if (Assess360List != null && Assess360List.FirstOrDefault().Value != null && Assess360List.FirstOrDefault().Key > 0)
            {
                long AssessId = Assess360List.First().Value[0].Id;
                Assess360Service a360 = new Assess360Service();
                string AssessMarks = a360.GetComponentWiseConsolidatedMarksForAStudent(AssessId);
                string[] AssMarksArr = AssessMarks.Split(',');
                string finalconsol = AssMarksArr[0].Split('/').First().ToString();
                Assess360StudentMarks asm = new Assess360StudentMarks();
                asm.ConsolidatedMarks = Convert.ToDecimal(finalconsol);
                asm.Character = Convert.ToDecimal(AssMarksArr[1]);
                asm.AttPunc = Convert.ToDecimal(AssMarksArr[2]);
                asm.HomeComp = Convert.ToDecimal(AssMarksArr[3]);
                asm.HomeAccu = Convert.ToDecimal(AssMarksArr[4]);
                asm.WeekTest = Convert.ToDecimal(AssMarksArr[5]);
                asm.SLC = Convert.ToDecimal(AssMarksArr[6]);
                asm.TermAssess = Convert.ToDecimal(AssMarksArr[7]);
                return asm;
            }
            else
            {
                return null;
            }
        }
        #endregion Assess360

        public Dictionary<long, IList<NoteAttachment>> GetDocumentsListWithPaging(string id)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Id", Convert.ToInt64(id));
                Dictionary<long, IList<NoteAttachment>> attachList = ppBC.GetDocumentsListWithPaging(null, null, null, null, criteria);
                return attachList;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<NoteAttachmentView>> GetDocumentsListViewWithPaging(string notepreid)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("NotePreId", Convert.ToInt64(notepreid));
                Dictionary<long, IList<NoteAttachmentView>> attachList = ppBC.GetDocumentsListViewWithPaging(null, null, null, null, criteria);
                return attachList;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

    }
}
