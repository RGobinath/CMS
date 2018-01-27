using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TIPS.Entities.Assess;
using PersistenceFactory;
using TIPS.Entities.Assess.ReportCardClasses;
using System.Collections;

namespace TIPS.Component
{
    public class ReportCardBC
    {
        PersistenceServiceFactory PSF = null;

        public ReportCardBC()
        {
            List<String> Assembly = new List<String>();
            Assembly.Add("TIPS.Entities");
            Assembly.Add("TIPS.Entities.Assess");
            Assembly.Add("TIPS.Entities.TicketingSystem");
            Assembly.Add("TIPS.Entities.TaskManagement");
            PSF = new PersistenceServiceFactory(Assembly);
        }

        #region "MYP"
        public long SaveOrUpdateMYPReportCard(RptCardMYP RptCardMYP)
        {
            try
            {
                //logic to check before saving
                if (RptCardMYP != null && RptCardMYP.Id > 0)
                {
                    PSF.Update<RptCardMYP>(RptCardMYP);
                }
                else
                {
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("Grade", RptCardMYP.Grade);
                    criteria.Add("Campus", RptCardMYP.Campus);
                    criteria.Add("Semester", RptCardMYP.Semester);
                    criteria.Add("AcademicYear", RptCardMYP.AcademicYear);
                    criteria.Add("IdNo", RptCardMYP.IdNo);// this line added by anbu
                    Dictionary<long, IList<RptCardMYP>> dcnRptCardMYPExists = PSF.GetListWithExactSearchCriteriaCount<RptCardMYP>(0, 10, "StudentId", "Asc", criteria);
                    IList<RptCardMYP> RepCardMYPExists = null;
                    if (dcnRptCardMYPExists != null && dcnRptCardMYPExists.FirstOrDefault().Key > 0)
                    {
                        RepCardMYPExists = dcnRptCardMYPExists.FirstOrDefault().Value;
                    }

                    if (RepCardMYPExists != null)
                    {
                        throw new Exception("This student " + RepCardMYPExists[0].Name + " already added in Report Card for Grade " + RepCardMYPExists[0].Grade + "\n and <i><b>Report Card No is :" + RepCardMYPExists[0].RequestNo + "</b></i>");
                    }
                    else
                    {
                        PSF.SaveOrUpdate<RptCardMYP>(RptCardMYP);
                        RptCardMYP.RequestNo = "RC-" + RptCardMYP.IdNo + "-" + RptCardMYP.Id.ToString();
                        PSF.SaveOrUpdate<RptCardMYP>(RptCardMYP);
                    }
                }
                return RptCardMYP.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public RptCardMYP GetMYPReportCard(long Id)
        {
            try
            {
                RptCardMYP RptCardMYP = null;
                if (Id > 0)
                {
                    RptCardMYP = PSF.Get<RptCardMYP>(Id);
                }
                return RptCardMYP;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion "MYP"

        #region "PYP"
        public long SaveOrUpdatePYPReportCard(RptCardPYP RptCardPYP)
        {
            try
            {
                //logic to check before saving
                if (RptCardPYP != null && RptCardPYP.Id > 0)
                {
                    PSF.Update<RptCardPYP>(RptCardPYP);
                }
                else
                {
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("Grade", RptCardPYP.Grade);
                    criteria.Add("Campus", RptCardPYP.Campus);
                    criteria.Add("Semester", RptCardPYP.Semester);
                    criteria.Add("AcademicYear", RptCardPYP.AcademicYear);

                    Dictionary<long, IList<RptCardPYP>> dcnRptCardPYPExists = PSF.GetListWithExactSearchCriteriaCount<RptCardPYP>(0, 10, "StudentId", "Asc", criteria);
                    IList<RptCardPYP> RepCardPYPExists = null;
                    if (dcnRptCardPYPExists != null && dcnRptCardPYPExists.FirstOrDefault().Key > 0)
                    {
                        RepCardPYPExists = dcnRptCardPYPExists.FirstOrDefault().Value;
                    }

                    if (RepCardPYPExists != null)
                    {
                        throw new Exception("This student " + RepCardPYPExists[0].Name + " already added in Report Card for Grade " + RepCardPYPExists[0].Grade + "\n and <i><b>Report Card No is :" + RepCardPYPExists[0].RequestNo + "</b></i>");
                    }
                    else
                    {
                        PSF.SaveOrUpdate<RptCardPYP>(RptCardPYP);
                        RptCardPYP.RequestNo = "RC-" + RptCardPYP.IdNo + "-" + RptCardPYP.Id.ToString();
                        PSF.SaveOrUpdate<RptCardPYP>(RptCardPYP);
                    }
                }
                return RptCardPYP.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public RptCardPYP GetPYPReportCard(long Id)
        {
            try
            {
                RptCardPYP RptCardPYP = null;
                if (Id > 0)
                {
                    RptCardPYP = PSF.Get<RptCardPYP>(Id);
                }
                return RptCardPYP;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion "1 Semester"

        public long SaveOrUpdateRptCardFocus(RptCardFocus RptCardFocus)
        {
            try
            {
                if (RptCardFocus != null && RptCardFocus.RptCardFocusId > 0)
                {
                    PSF.SaveOrUpdate<RptCardFocus>(RptCardFocus);
                }
                else
                {
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("Grade", RptCardFocus.Grade);
                    criteria.Add("Campus", RptCardFocus.Campus);
                    criteria.Add("Semester", RptCardFocus.Semester);
                    criteria.Add("AcademicYear", RptCardFocus.AcademicYear);

                   // Dictionary<long, IList<RptCardFocus>> dcnRptCardFocusExists = PSF.GetListWithExactSearchCriteriaCount<RptCardFocus>(0, 10, "RptCardFocusId", "Asc", criteria);
                    Dictionary<long, IList<RptCardFocus>> dcnRptCardFocusExists = PSF.GetListWithEQSearchCriteriaCount<RptCardFocus>(0, 10, "Asc", "RptCardFocusId", criteria);
                    IList<RptCardFocus> RptCardFocusExists = null;
                    if (dcnRptCardFocusExists != null && dcnRptCardFocusExists.FirstOrDefault().Key > 0)
                    {
                        RptCardFocusExists = dcnRptCardFocusExists.FirstOrDefault().Value;
                    }

                    if (RptCardFocusExists != null)
                    {
                        throw new Exception(string.Format("Focus already added for Grade {0}, Campus {1}, Semester {2} and Academic Year {3}", RptCardFocusExists[0].Grade, RptCardFocusExists[0].Campus, RptCardFocusExists[0].Semester, RptCardFocusExists[0].AcademicYear));
                    }
                    else
                    {
                        PSF.SaveOrUpdate<RptCardFocus>(RptCardFocus);
                    }
                }
                return RptCardFocus.RptCardFocusId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public RptCardFocus GetRptCardFocusById(long RptCardFocusId)
        {
            try
            {
                return PSF.Get<RptCardFocus>(RptCardFocusId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public RptCardFocus GetRptCardFocusByGradeCampusSem(string Grade, string Campus, long Semester, string AcademicYear)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Grade", Grade);
                criteria.Add("Campus", Campus);
                criteria.Add("Semester", Semester);
                criteria.Add("AcademicYear", AcademicYear);

                RptCardFocus RptCardFocus = new RptCardFocus();

               // Dictionary<long, IList<RptCardFocus>> dcnRptCardPYPExists = PSF.GetListWithExactSearchCriteriaCount<RptCardFocus>(0, 10, "RptCardFocusId", "Asc", criteria);
                Dictionary<long, IList<RptCardFocus>> dcnRptCardPYPExists = PSF.GetListWithEQSearchCriteriaCount<RptCardFocus>(0, 10, "Asc", "RptCardFocusId", criteria);
                if (dcnRptCardPYPExists != null && dcnRptCardPYPExists.FirstOrDefault().Key > 0)
                {
                    RptCardFocus = dcnRptCardPYPExists.FirstOrDefault().Value[0];
                }

                return RptCardFocus;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<StudentDtlsView>> GetStudentDtlsViewListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, string name, string[] values, Dictionary<string, object> criteria)
        {
            try
            {
                criteria.Add("AdmissionStatus", "Registered");
                sortType = sortType == "desc" ? "Desc" : "Asc";
                return PSF.GetListWithSearchCriteriaCountArray<StudentDtlsView>(page, pageSize, sortby, sortType, name, values, criteria, null);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<SubjectMarksForRptCard_Vw>> GetSubjectMarksForRptCard_VwWidthSubjectWiseList(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<SubjectMarksForRptCard_Vw>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region "Report Card Inbox"
        public Dictionary<long, IList<RptCardInBoxView>> GetRepCardInBoxListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                sortType = sortType == "desc" ? "Desc" : "Asc";
                return PSF.GetListWithExactSearchCriteriaCount<RptCardInBoxView>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<RptCardInBoxView>> GetRepCardInBoxListWithPagingAndCriteriaEQSearch(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                sortType = sortType == "desc" ? "Desc" : "Asc";
                return PSF.GetListWithEQSearchCriteriaCount<RptCardInBoxView>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion "Report Card Inbox"

        #region Start PYP 1 to 5 Grades
        public long SaveOrUpdatePYPFirstGrade(RptCardPYPFirstGrade RptPYP)
        {
            try
            {
                if (RptPYP != null && RptPYP.Id > 0)
                {
                    PSF.Update<RptCardPYPFirstGrade>(RptPYP);
                }
                else
                {
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("Grade", RptPYP.Grade);
                    criteria.Add("Campus", RptPYP.Campus);
                    criteria.Add("Semester", RptPYP.Semester);
                    criteria.Add("AcademicYear", RptPYP.AcademicYear);
                    criteria.Add("IdNo", RptPYP.IdNo);// this line added by felix
                    Dictionary<long, IList<RptCardPYPFirstGrade>> dcnRptCardPYPExists = PSF.GetListWithExactSearchCriteriaCount<RptCardPYPFirstGrade>(0, 10, "PreRegNum", "Asc", criteria);
                    IList<RptCardPYPFirstGrade> RepCardPYPExists = null;
                    if (dcnRptCardPYPExists != null && dcnRptCardPYPExists.FirstOrDefault().Key > 0)
                    {
                        RepCardPYPExists = dcnRptCardPYPExists.FirstOrDefault().Value;
                    }

                    if (RepCardPYPExists != null)
                    {
                        throw new Exception("This student " + RepCardPYPExists[0].Name + " already added in Report Card for Grade " + RepCardPYPExists[0].Grade + "\n and <i><b>Report Card No is :" + RepCardPYPExists[0].RequestNo + "</b></i>");
                    }
                    else
                    {


                        PSF.SaveOrUpdate<RptCardPYPFirstGrade>(RptPYP);
                        RptPYP.RequestNo = "FRC-" + RptPYP.IdNo + "-" + RptPYP.Id.ToString();


                        PSF.SaveOrUpdate<RptCardPYPFirstGrade>(RptPYP);
                    }
                }
                return RptPYP.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public RptCardPYPFirstGrade GetPYPFirstGrade(long Id)
        {
            try
            {
                RptCardPYPFirstGrade RptCardPYP = null;
                if (Id > 0)
                {
                    RptCardPYP = PSF.Get<RptCardPYPFirstGrade>(Id);
                }
                return RptCardPYP;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<RptCardPYPFirstGrade>> GetRepCardFirstGradeListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                sortType = sortType == "desc" ? "Desc" : "Asc";
                return

PSF.GetListWithExactSearchCriteriaCount<RptCardPYPFirstGrade>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion End PYP 1 to 5 Grades

        public Dictionary<long, IList<RptCardMYP>> GetRptCardMYPListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<RptCardMYP>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region "Bulk Report Card Creation for MYP"
        public Dictionary<long, IList<StudentDtlsView>> GetStudentDtlsViewListforBulkRptCardWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                sortType = sortType == "desc" ? "Desc" : "Asc";
                return PSF.GetListWithEQSearchCriteriaCount<StudentDtlsView>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long SaveOrUpdateBulkMYPReportCard(List<StudentDtlsView> RptMYP, string semester, string teachName, string rptMypDate)
        {
            try
            {
                IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                long count = 0;
                foreach (var item in RptMYP)
                {
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("Grade", item.Grade);
                    criteria.Add("Campus", item.Campus);
                    criteria.Add("Semester", Convert.ToInt64(semester == "Sem I" ? 1 : 2));
                    criteria.Add("AcademicYear", item.AcademicYear);
                    criteria.Add("IdNo", item.IdNo);// this line added by anbu
                    Dictionary<long, IList<RptCardMYP>> dcnRptCardMYPExists = PSF.GetListWithExactSearchCriteriaCount<RptCardMYP>(0, 9999, "StudentId", "Asc", criteria);
                    IList<RptCardMYP> RepCardMYPExists = null;
                    if (dcnRptCardMYPExists != null && dcnRptCardMYPExists.FirstOrDefault().Key > 0)
                    {
                        RepCardMYPExists = dcnRptCardMYPExists.FirstOrDefault().Value;
                    }
                    if (RepCardMYPExists != null)
                    {
                        count += 1;
                        // throw new Exception("This student " + RepCardMYPExists[0].Name + " already added in Report Card for Grade " + RepCardMYPExists[0].Grade + "\n and <i><b>Report Card No is :" + RepCardMYPExists[0].RequestNo + "</b></i>");
                    }
                    else
                    {
                        RptCardMYP rptCardMYP = new RptCardMYP();
                        rptCardMYP.Campus = item.Campus;
                        rptCardMYP.Grade = item.Grade;
                        rptCardMYP.Section = item.Section;
                        rptCardMYP.IdNo = item.IdNo;
                        rptCardMYP.PreRegNum = Convert.ToInt64(item.PreRegNum);
                        rptCardMYP.StudentId = item.Id;
                        rptCardMYP.Name = item.Name;
                        rptCardMYP.AcademicYear = item.AcademicYear;
                        rptCardMYP.Semester = semester == "Sem I" ? 1 : 2;
                        rptCardMYP.Second_Language = item.SecondLanguage;
                        rptCardMYP.RptCardStatus = "WIP";
                        rptCardMYP.TeacherName = teachName;
                        if (!string.IsNullOrEmpty(rptMypDate))
                        {
                            var rptDate = DateTime.Parse(rptMypDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                            rptCardMYP.RptDate = rptDate;
                        }
                        PSF.SaveOrUpdate<RptCardMYP>(rptCardMYP);
                        rptCardMYP.RequestNo = "RC-" + rptCardMYP.IdNo + "-" + rptCardMYP.Id.ToString();
                        PSF.SaveOrUpdate<RptCardMYP>(rptCardMYP);
                    }
                }
                return count;// already existed student counts
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion End

        #region ReportCardForTirupuandKarur CBSE
        public Dictionary<long, IList<ReportCardRequest>> GetReportCardRequestListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<ReportCardRequest>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateReportCardRequest(ReportCardRequest RptReq)
        {
            try
            {
                if (RptReq != null)
                {
                    PSF.SaveOrUpdate<ReportCardRequest>(RptReq);
                    return RptReq.Id;
                }
                else
                {
                    return 0;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public ReportCardRequest GetRepoetCardRequestById(long Id)
        {
            try
            {
                ReportCardRequest RptCardReq = null;
                if (Id > 0)
                {
                    RptCardReq = PSF.Get<ReportCardRequest>(Id);
                }
                return RptCardReq;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<RptStudentDtlsView>> GetStudentDtlsViewListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                criteria.Add("AdmissionStatus", "Registered");
                sortType = sortType == "desc" ? "Desc" : "Asc";
                return PSF.GetListWithEQSearchCriteriaCount<RptStudentDtlsView>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<ReportCardCBSE>> GetRptCardForCBSEListWithCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                sortType = sortType == "desc" ? "Desc" : "Asc";
                return PSF.GetListWithEQSearchCriteriaCount<ReportCardCBSE>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<ReportCardCBSECommon>> GetRptCardForCBSECommonListWithCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                sortType = sortType == "desc" ? "Desc" : "Asc";
                return PSF.GetListWithEQSearchCriteriaCount<ReportCardCBSECommon>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ReportCardCBSE GetReportCardCBSEById(long Id)
        {
            try
            {
                ReportCardCBSE RptCardCBSE = null;
                if (Id > 0)
                {
                    RptCardCBSE = PSF.Get<ReportCardCBSE>(Id);
                }
                return RptCardCBSE;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ReportCardCBSECommon GetReportCardCBSECommonById(long Id)
        {
            try
            {
                ReportCardCBSECommon RptCardCBSECommon = null;
                if (Id > 0)
                {
                    RptCardCBSECommon = PSF.Get<ReportCardCBSECommon>(Id);
                }
                return RptCardCBSECommon;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public RptCoScholasticCBSE GetReportCardCoScholasticCBSEById(long Id)
        {
            try
            {
                RptCoScholasticCBSE RptCardCoCBSE = null;
                if (Id > 0)
                {
                    RptCardCoCBSE = PSF.Get<RptCoScholasticCBSE>(Id);
                }
                return RptCardCoCBSE;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long SaveOrUpdateRptCardForCBSE(ReportCardCBSE RptVItoVIII)
        {
            try
            {
                if (RptVItoVIII != null && RptVItoVIII.Id > 0)
                {
                    PSF.Update<ReportCardCBSE>(RptVItoVIII);
                }
                else
                {
                    PSF.SaveOrUpdate<ReportCardCBSE>(RptVItoVIII);
                }
                return RptVItoVIII.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long SaveOrUpdateRptCardForCBSECommon(ReportCardCBSECommon RptVItoVIII)
        {
            try
            {
                if (RptVItoVIII != null && RptVItoVIII.Id > 0)
                {
                    PSF.Update<ReportCardCBSECommon>(RptVItoVIII);
                }
                else
                {
                    PSF.SaveOrUpdate<ReportCardCBSECommon>(RptVItoVIII);
                }
                return RptVItoVIII.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long SaveOrUpdateRptCardCoScholasticForCBSE(RptCoScholasticCBSE RptCoSchoVItoVIII)
        {
            try
            {
                if (RptCoSchoVItoVIII != null && RptCoSchoVItoVIII.Id > 0)
                {
                    PSF.Update<RptCoScholasticCBSE>(RptCoSchoVItoVIII);
                }
                else
                {
                    PSF.SaveOrUpdate<RptCoScholasticCBSE>(RptCoSchoVItoVIII);
                }
                return RptCoSchoVItoVIII.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ReportCardCBSECommon GetReportCardCBSECommon(long RegNum, string AcYear)
        {
            try
            {
                ReportCardCBSECommon RptCardCBSECommon = null;
                if (RegNum > 0 && !string.IsNullOrEmpty(AcYear))
                {
                    RptCardCBSECommon = PSF.Get<ReportCardCBSECommon>("PreRegNum", RegNum, "AcademicYear", AcYear);
                }
                return RptCardCBSECommon;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Newly Added
        /// </summary>
        public Dictionary<long, IList<CoScholasticMaster>> GetCoScholictMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<CoScholasticMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<CoScholasticItemMaster>> GetCoScholasticItemMasterListWithCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<CoScholasticItemMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<CoScholasticStudentDetails_Vw>> GetCoScholasticStudentDtlsViewListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                criteria.Add("AdmissionStatus", "Registered");
                sortType = sortType == "desc" ? "Desc" : "Asc";
                return PSF.GetListWithEQSearchCriteriaCount<CoScholasticStudentDetails_Vw>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<RptCoScholasticCBSE>> GetRptScholasticCBSEListWithCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                sortType = sortType == "desc" ? "Desc" : "Asc";
                return PSF.GetListWithEQSearchCriteriaCount<RptCoScholasticCBSE>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<CoScholasticItemMaster>> GetCoScholasticItemMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                sortType = sortType == "desc" ? "Desc" : "Asc";
                return PSF.GetListWithEQSearchCriteriaCount<CoScholasticItemMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<CoSchCriteriaMaster>> GetCoScholasticCriteriaMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                sortType = sortType == "desc" ? "Desc" : "Asc";
                return PSF.GetListWithEQSearchCriteriaCount<CoSchCriteriaMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<ReportCardCBSECo_Scholastic>> GetRptCardCoSchWorkandEducationListWithCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                sortType = sortType == "desc" ? "Desc" : "Asc";
                return PSF.GetListWithEQSearchCriteriaCount<ReportCardCBSECo_Scholastic>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ReportCardCBSECo_Scholastic GetReportCardCBSECo_ScholasticById(long Id)
        {
            try
            {
                ReportCardCBSECo_Scholastic ReportCardCBSECo_Scholastic = null;
                if (Id > 0)
                {
                    ReportCardCBSECo_Scholastic = PSF.Get<ReportCardCBSECo_Scholastic>(Id);
                }
                return ReportCardCBSECo_Scholastic;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long SaveOrUpdateReportCardCBSECo_Scholastic(ReportCardCBSECo_Scholastic ReportCardCBSECo_Scholastic)
        {
            try
            {
                if (ReportCardCBSECo_Scholastic != null && ReportCardCBSECo_Scholastic.Id > 0)
                {
                    PSF.Update<ReportCardCBSECo_Scholastic>(ReportCardCBSECo_Scholastic);
                }
                else
                {
                    PSF.SaveOrUpdate<ReportCardCBSECo_Scholastic>(ReportCardCBSECo_Scholastic);
                }
                return ReportCardCBSECo_Scholastic.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<CoSch_Item_Vw>> GetRptCardCosch_ItemListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<CoSch_Item_Vw>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetCoSchGradeByPreRegNum(long PreRegNum, string CoSchGradeColumn,string Term)
        {
            try
            {
                if (PreRegNum != 0 && CoSchGradeColumn != null &&!string.IsNullOrEmpty(Term))
                {
                    string query = "Select " + CoSchGradeColumn + " from ReportCardCBSECo_Scholastic where PreRegNum=" + PreRegNum + "and Term=" + Term + " and " + CoSchGradeColumn + " Is Not NULL";
                    IList list = PSF.ExecuteSql(query);
                    if (list != null && list.Count > 0 && list[0] != null)
                    {
                        return Convert.ToString(list[0]);
                    }
                    else
                        return null;
                }
                else
                    return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region "New changes on CBSE ReportCard"
        public Dictionary<long, IList<RptStudentDtlsViewNew>> GetStudentDtlsViewNewListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                criteria.Add("AdmissionStatus", "Registered");
                sortType = sortType == "desc" ? "Desc" : "Asc";
                return PSF.GetListWithEQSearchCriteriaCount<RptStudentDtlsViewNew>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<ReportCardCBSENew>> GetRptCardForCBSENewListWithCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                sortType = sortType == "desc" ? "Desc" : "Asc";
                return PSF.GetListWithEQSearchCriteriaCount<ReportCardCBSENew>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ReportCardCBSENew GetReportCardCBSENewById(long Id)
        {
            try
            {
                ReportCardCBSENew RptCardCBSE = null;
                if (Id > 0)
                {
                    RptCardCBSE = PSF.Get<ReportCardCBSENew>(Id);
                }
                return RptCardCBSE;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long SaveOrUpdateRptCardForCBSENew(ReportCardCBSENew RptVItoVIII)
        {
            try
            {
                if (RptVItoVIII != null && RptVItoVIII.Id > 0)
                {
                    PSF.Update<ReportCardCBSENew>(RptVItoVIII);
                }
                else
                {
                    PSF.SaveOrUpdate<ReportCardCBSENew>(RptVItoVIII);
                }
                return RptVItoVIII.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}