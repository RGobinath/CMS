using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PersistenceFactory;
using TIPS.Entities.Assess;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using TIPS.Entities.AdmissionEntities;
using TIPS.Entities;

namespace TIPS.Component
{
    public class Assess360BC
    {
        PersistenceServiceFactory PSF = null;
        public Assess360BC()
        {
            List<String> Assembly = new List<String>();
            Assembly.Add("TIPS.Entities");
            Assembly.Add("TIPS.Entities.Assess");
            Assembly.Add("TIPS.Entities.TicketingSystem");
            Assembly.Add("TIPS.Entities.TaskManagement");
            PSF = new PersistenceServiceFactory(Assembly);
        }
        public long SaveOrUpdateAssess360(Assess360 Assess360)
        {
            try
            {
                //logic to check before saving
                if (Assess360.Id > 0)
                {
                    PSF.SaveOrUpdate<Assess360>(Assess360);
                }
                else
                {
                    Assess360 Assess360Exists = PSF.Get<Assess360>("StudentId", Assess360.StudentId, "Grade", Assess360.Grade, "AcademicYear", Assess360.AcademicYear);
                    if (Assess360Exists != null)
                    {
                        throw new Exception("This student " + Assess360Exists.Name + " already added in Assess360 for Grade " + Assess360Exists.Grade + " and Academic Year " + Assess360Exists.AcademicYear + "");
                    }
                    else
                    {
                        PSF.SaveOrUpdate<Assess360>(Assess360);
                        Assess360.RequestNo = "A360-" + Assess360.IdNo + "-" + Assess360.Id.ToString();
                        PSF.SaveOrUpdate<Assess360>(Assess360);
                    }
                }
                return Assess360.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string SaveOrUpdateAssess360RtnString(Assess360 Assess360)
        {
            try
            {
                //logic to check before saving
                if (Assess360.Id > 0)
                {
                    PSF.SaveOrUpdate<Assess360>(Assess360);
                }
                else
                {
                    Assess360 Assess360Exists = PSF.Get<Assess360>("StudentId", Assess360.StudentId, "Grade", Assess360.Grade);
                    if (Assess360Exists != null)
                    {
                        throw new Exception("This student " + Assess360Exists.Name + " already added in Assess360 for Grade " + Assess360Exists.Grade + "");
                    }
                    else
                    {
                        PSF.SaveOrUpdate<Assess360>(Assess360);
                        Assess360.RequestNo = "A360-" + Assess360.IdNo + "-" + Assess360.Id.ToString();
                        PSF.SaveOrUpdate<Assess360>(Assess360);
                    }
                }
                return Assess360.RequestNo;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long SaveOrUpdateAssess360Component(Assess360Component Assess360Component)
        {
            try
            {
                PSF.SaveOrUpdate<Assess360Component>(Assess360Component);
                return Assess360Component.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool SaveOrUpdateAssess360ComponentList(IList<Assess360Component> Assess360Component)
        {
            try
            {
                bool retValue = false;
                PSF.SaveOrUpdate<Assess360Component>(Assess360Component);
                retValue = true;
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Assess360 GetAssess360ById(long Id)
        {
            try
            {
                return PSF.Get<Assess360>(Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<Assess360>> GetAssess360ListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<Assess360>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<Assess360>> GetAssess360ListWithPagingAndCriteriaWithAlias(int? page, int? pageSize, string sortby, string sortType, string name, string[] values, Dictionary<string, object> criteria, string[] alias)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCountArray<Assess360>(page, pageSize, sortby, sortType, name, values, criteria, alias);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<StudentDetailsView>> GetStudentDetailsViewListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                string name = "Grade"; string[] values = new string[7] { "VI", "VII", "VIII", "IX", "X", "XI", "XII" };
                criteria.Add("AdmissionStatus", "Registered");
                return PSF.GetListWithSearchCriteriaCountArray<StudentDetailsView>(page, pageSize, sortby, sortType, name, values, criteria, null);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Assess360Component GetAssess360ComponentById(long Id)
        {
            try
            {
                return PSF.Get<Assess360Component>(Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<Assess360Component>> GetAssess360ComponentListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<Assess360Component>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<Assess360Component>> GetAssess360ComponentListWithPagingAndInCriteria(int? page, int? pageSize, string sortby, string sortType, string name, int[] values, Dictionary<string, object> criteria, string[] alias)
        {
            try
            {
                return PSF.GetListWithInSearchCriteriaCountArray<Assess360Component>(page, pageSize, sortby, sortType, name, values, criteria, alias);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<AssessCompMaster>> GetAssess360CompMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<AssessCompMaster>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<AssessCompMaster>> GetAssess360CompMasterListWithPagingAndCriteriaWithIn(int? page, int? pageSize, string sortby, string sortType, string name, int[] values, Dictionary<string, object> criteria, string[] alias)
        {
            try
            {
                return PSF.GetListWithInSearchCriteriaCountArray<AssessCompMaster>(page, pageSize, sortby, sortType, name, values, criteria, alias);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<AssessCompMaster> GetAssess360CompMasterListByName(string AssessCompGroup)
        {
            try
            {
                return PSF.GetList<AssessCompMaster>("AssessCompGroup", AssessCompGroup);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<SubjectMaster>> GetSubjectMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                string name = "Grade"; string[] values = new string[7] { "VI", "VII", "VIII", "IX", "X", "XI", "XII" };
                return PSF.GetListWithEQSearchCriteriaCount<SubjectMaster>(page, pageSize, sortby, sortType, criteria, name, values, null);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<StaffMaster>> GetStaffMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<StaffMaster>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<AssessGroupMaster>> GetAssessGroupMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<AssessGroupMaster>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<WeeklyTestForStud> GetStudWeeklyTestForAssess360Id(long Assess360Id)
        {
            try
            {
                IList<WeeklyTestForStud> WeeklyTestForStud = PSF.GetList<WeeklyTestForStud>("Assess360Id", Assess360Id);
                return WeeklyTestForStud;
                //return PSF.GetListWithExactSearchCriteriaCount<AssessGroupMaster>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<StudTermAssessment> GetStudTermAssessmentForAssess360Id(long Assess360Id)
        {
            try
            {
                IList<StudTermAssessment> WeeklyTestForStud = PSF.GetList<StudTermAssessment>("Assess360Id", Assess360Id);
                return WeeklyTestForStud;
                //return PSF.GetListWithExactSearchCriteriaCount<AssessGroupMaster>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<StudSLCAssessment> GetStudSLCAssessmentForAssess360Id(long Assess360Id)
        {
            try
            {
                IList<StudSLCAssessment> WeeklyTestForStud = PSF.GetList<StudSLCAssessment>("Assess360Id", Assess360Id);
                return WeeklyTestForStud;
                //return PSF.GetListWithExactSearchCriteriaCount<AssessGroupMaster>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<StudHWAccuracy> GetStudHWAccuracyForAssess360Id(long Assess360Id)
        {
            try
            {
                IList<StudHWAccuracy> StudHWAccuracy = PSF.GetList<StudHWAccuracy>("Assess360Id", Assess360Id);
                return StudHWAccuracy;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<StudCharacterAssessment> GetStudCharacterAssessmentForAssess360Id(long Assess360Id)
        {
            try
            {
                IList<StudCharacterAssessment> StudCharacterAssess = PSF.GetList<StudCharacterAssessment>("Assess360Id", Assess360Id);
                return StudCharacterAssess;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<StudAttenPuncAssessment> GetStudAttenPuncAssessmentForAssess360Id(long Assess360Id)
        {
            try
            {
                IList<StudAttenPuncAssessment> StudAttenPuncAssessment = PSF.GetList<StudAttenPuncAssessment>("Assess360Id", Assess360Id);
                return StudAttenPuncAssessment;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<StudHwCompletion> GetStudHwCompletionForAssess360Id(long Assess360Id)
        {
            try
            {
                IList<StudHwCompletion> StudHwCompletion = PSF.GetList<StudHwCompletion>("Assess360Id", Assess360Id);
                return StudHwCompletion;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<StudCopiedhomework> GetStudCopiedhomeworkForAssess360Id(long Assess360Id)
        {
            try
            {
                IList<StudCopiedhomework> StudCopiedhomework = PSF.GetList<StudCopiedhomework>("Assess360Id", Assess360Id);
                return StudCopiedhomework;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string GetConsolidatedMarksForAStudent(int Assess360Id)
        {
            //logic goes for the consolidation of Marks
            return "";
        }
        public string GetComponentWiseConsolidatedMarksForAStudent(long Assess360Id)
        { return ""; }
        public long GetPreRegNumForStudentId(long Id)
        {
            string query = "select PreRegNum from StudentTemplate where Id=" + Id + "";
            IList list = PSF.ExecuteSql(query);
            if (list != null && list[0] != null)
            {
                return Convert.ToInt64(list[0].ToString()); //list[0] = "0";
            }
            else return 0;
            //return Convert.ToInt64(list[0].ToString());
        }

        public string GetCampusForStudentId(long Id)
        {
            string query = "select Campus from StudentTemplate where Id=" + Id + "";
            IList list = PSF.ExecuteSql(query);
            if (list != null && list[0] != null)
            {
                return list[0].ToString();
            }
            else { return "0"; }
            //return Convert.ToInt64(list[0].ToString());
        }
        public bool DeleteAssess360Component(long[] Assess360ComponentIds)
        {
            try
            {
                bool retValue = false;
                IList<Assess360Component> List = PSF.GetListByIds<Assess360Component>(Assess360ComponentIds);
                PSF.DeleteAll<Assess360Component>(List);
                retValue = true;
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //public IList<Assess360Point> GetAssess360PointForAcademicYear(string grade, string section, string campus, string academicYear)
        //{
        //    IList<Assess360Point> retValue = new List<Assess360Point>();
        //    Dictionary<string, object> a360Criteria = new Dictionary<string, object>();
        //    a360Criteria.Add("Grade", grade);
        //    a360Criteria.Add("Section", section);
        //    a360Criteria.Add("Campus", campus);
        //    if (!string.IsNullOrEmpty(academicYear))
        //        a360Criteria.Add("AcademicYear", academicYear);
        //    a360Criteria.Add("IsActive", true);
        //    Dictionary<long, IList<Assess360>> GradeSectionList = PSF.GetListWithEQSearchCriteriaCount<Assess360>(0, 1000, string.Empty, string.Empty, a360Criteria);
        //    if (GradeSectionList != null && GradeSectionList.Count > 0)
        //    {
        //        IList<Assess360> studList = GradeSectionList.FirstOrDefault().Value;
        //        if (studList != null && studList.Count > 0)
        //        {
        //            Dictionary<string, object> criteria = null; int i = 1;
        //            //actual logic to calculate the consolidation
        //            foreach (Assess360 a360 in studList)
        //            {
        //                decimal? Consolidation = 0;
        //                criteria = new Dictionary<string, object>();
        //                //from date to date month logic
        //                DateTime now = DateTime.Now;
        //                DateTime fromDate;
        //                DateTime toDate = now;
        //                DateTime[] dateArray = new DateTime[2];
        //                if (grade == "IX" || grade == "X")
        //                {
        //                    fromDate = new DateTime(now.Year, 1, 1);
        //                    toDate = new DateTime(now.Year, 12, 31);
        //                }
        //                else
        //                {
        //                    if (now.Month > 5)
        //                    {
        //                        fromDate = new DateTime(now.Year, 6, 1);
        //                    }
        //                    else { fromDate = new DateTime(now.Year - 1, 6, 1); }
        //                }

        //                dateArray[0] = fromDate; dateArray[1] = toDate;
        //                criteria.Add("IncidentDate", dateArray);
        //                criteria.Add("Assess360Id", a360.Id);
        //                Assess360Point Assess360Point = new Assess360Point();
        //                Assess360Point.StudName = a360.Name;
        //                Assess360Point.Id = i++;
        //                Assess360Point.ReportGenDate = DateTime.Now;
        //                Assess360Point.AcademicYear = a360.AcademicYear;
        //                Assess360Point.Campus = a360.Campus;
        //                Assess360Point.Grade = a360.Grade;
        //                Assess360Point.Section = a360.Section;
        //                Assess360Point.DateCreated = a360.DateCreated;
        //                Assess360Point.IdNo = a360.IdNo;
        //                Assess360Point.RequestNo = a360.RequestNo;
        //                Assess360Point.StudentId = a360.StudentId;
        //                //get the complete list for the month
        //                //Dictionary<long, IList<Assess360Component>> StudCharacterAssess = PSF.GetListWithEQSearchCriteriaCount<Assess360Component>(0, 10000, string.Empty, string.Empty, criteria);
        //                //Character calculation part
        //                IList<StudCharacterAssessment> CharAssessment = this.GetStudCharacterAssessmentForAssess360Id(a360.Id);
        //                if (CharAssessment != null && CharAssessment.Count > 0)
        //                {
        //                    Consolidation = CharAssessment.FirstOrDefault().FinalMark;
        //                    Assess360Point.Character = CharAssessment.FirstOrDefault().FinalMark.Value.ToString("0.00");
        //                }
        //                else
        //                {
        //                    Consolidation += 20;
        //                    Assess360Point.Character = "20";
        //                }
        //                //Attendence and punctuality part
        //                IList<StudAttenPuncAssessment> AttnPuncAssessment = this.GetStudAttenPuncAssessmentForAssess360Id(a360.Id);
        //                if (AttnPuncAssessment != null && AttnPuncAssessment.Count > 0)
        //                {
        //                    Consolidation += AttnPuncAssessment.FirstOrDefault().FinalMark;
        //                    Assess360Point.AttPunctuality = AttnPuncAssessment.FirstOrDefault().FinalMark.Value.ToString("0.00");
        //                }
        //                else
        //                {
        //                    Consolidation += 10;
        //                    Assess360Point.AttPunctuality = "10";
        //                }
        //                //Homework completion part
        //                IList<StudHwCompletion> HWComplAssessment = this.GetStudHwCompletionForAssess360Id(a360.Id);
        //                if (HWComplAssessment != null && HWComplAssessment.Count > 0)
        //                {
        //                    Consolidation += HWComplAssessment.FirstOrDefault().FinalMark;
        //                    Assess360Point.HwCompletion = HWComplAssessment.FirstOrDefault().FinalMark.Value.ToString("0.00");
        //                }
        //                else
        //                {
        //                    Consolidation += 5;
        //                    Assess360Point.HwCompletion = "5";
        //                }
        //                //Homework accuracy part
        //                IList<StudHWAccuracy> HWAccuAssessment = this.GetStudHWAccuracyForAssess360Id(a360.Id);
        //                decimal HWAccuracyAvg;
        //                if (HWAccuAssessment != null && HWAccuAssessment.Count > 0)
        //                {
        //                    //find average
        //                    var a = from e in HWAccuAssessment select e.Mark;
        //                    HWAccuracyAvg = Convert.ToDecimal(a.ToArray().Average());
        //                    if (Convert.ToInt16(HWAccuracyAvg) == 0)
        //                    {
        //                        Consolidation += 15;
        //                        Assess360Point.HwAccuracy = "15";
        //                    }
        //                    else
        //                    {
        //                        Consolidation += HWAccuracyAvg;
        //                        Assess360Point.HwAccuracy = HWAccuracyAvg.ToString("0.00");
        //                    }
        //                }
        //                else
        //                {
        //                    Consolidation += 15;
        //                    Assess360Point.HwAccuracy = "15";
        //                }
        //                //Copied homework
        //                IList<StudCopiedhomework> StudCopiedhomework = this.GetStudCopiedhomeworkForAssess360Id(a360.Id);
        //                decimal copy;
        //                if (StudCopiedhomework != null && StudCopiedhomework.Count > 0)
        //                {
        //                    //find average
        //                    var a = from e in StudCopiedhomework select e.Mark;
        //                    copy = Convert.ToDecimal(a.ToArray().Sum());
        //                    if (Convert.ToInt16(copy) > 0)
        //                    {
        //                        Consolidation -= copy;
        //                    }
        //                }
        //                //Weekly Chapter test part
        //                IList<WeeklyTestForStud> WeeklyTestAssessment = this.GetStudWeeklyTestForAssess360Id(a360.Id);
        //                if (WeeklyTestAssessment != null && WeeklyTestAssessment.Count > 0)
        //                {
        //                    decimal weeklyTest;
        //                    if (WeeklyTestAssessment != null && WeeklyTestAssessment.Count > 0)
        //                    {
        //                        //find average
        //                        var a = from e in WeeklyTestAssessment select e.Mark;
        //                        weeklyTest = Convert.ToDecimal(a.ToArray().Average());
        //                        if (Convert.ToInt16(weeklyTest) == 0)
        //                        {
        //                            Consolidation += 20; Assess360Point.WkChapterTests = "20";
        //                        }
        //                        else { Consolidation += weeklyTest; Assess360Point.WkChapterTests = weeklyTest.ToString("0.00"); }
        //                    }
        //                }
        //                else { Consolidation += 20; Assess360Point.WkChapterTests = "20"; }
        //                //SLC parent Assessment
        //                IList<StudSLCAssessment> SLCTestAssessment = this.GetStudSLCAssessmentForAssess360Id(a360.Id);
        //                if (SLCTestAssessment != null && SLCTestAssessment.Count > 0)
        //                {
        //                    Consolidation += SLCTestAssessment.FirstOrDefault().Mark;
        //                    Assess360Point.SLCParentAssessment = SLCTestAssessment.FirstOrDefault().Mark.ToString("0.00");
        //                }
        //                else { Consolidation += 5; Assess360Point.SLCParentAssessment = "5"; }
        //                //Term assessment
        //                IList<StudTermAssessment> StudTermAssessment = this.GetStudTermAssessmentForAssess360Id(a360.Id);
        //                if (StudTermAssessment != null && StudTermAssessment.Count > 0)
        //                {
        //                    decimal TermAssess;
        //                    if (StudTermAssessment != null && StudTermAssessment.Count > 0)
        //                    {
        //                        //find average
        //                        var a = from e in StudTermAssessment select e.Mark;
        //                        TermAssess = Convert.ToDecimal(a.ToArray().Average());
        //                        if (Convert.ToInt16(TermAssess) == 0)
        //                        {
        //                            Consolidation += 25;
        //                            Assess360Point.TermAssessment = "25";
        //                        }
        //                        else { Consolidation += TermAssess; Assess360Point.TermAssessment = TermAssess.ToString("0.00"); }
        //                    }
        //                }
        //                else { Consolidation += 25; Assess360Point.TermAssessment = "25"; }
        //                Assess360Point.Total = Consolidation.Value.ToString("0.00");
        //                retValue.Add(Assess360Point);
        //            }
        //        }
        //    }
        //    return retValue;
        //}

        public IList<Assess360Point> GetAssess360PointForAcademicYear(string grade, string section, string campus, string academicYear)
        {
            IList<Assess360Point> retValue = new List<Assess360Point>();
            Dictionary<string, object> a360Criteria = new Dictionary<string, object>();
            a360Criteria.Add("Grade", grade);
            a360Criteria.Add("Section", section);
            a360Criteria.Add("Campus", campus);
            if (!string.IsNullOrEmpty(academicYear))
                a360Criteria.Add("AcademicYear", academicYear);
            a360Criteria.Add("IsActive", true);
            Dictionary<long, IList<Assess360>> GradeSectionList = PSF.GetListWithEQSearchCriteriaCount<Assess360>(0, 1000, string.Empty, string.Empty, a360Criteria);
            if (GradeSectionList != null && GradeSectionList.Count > 0)
            {
                IList<Assess360> studList = GradeSectionList.FirstOrDefault().Value;
                if (studList != null && studList.Count > 0)
                {
                    Dictionary<string, object> criteria = null; int i = 1;
                    //actual logic to calculate the consolidation
                    foreach (Assess360 a360 in studList)
                    {
                        decimal? Consolidation = 0;
                        criteria = new Dictionary<string, object>();
                        //from date to date month logic
                        DateTime now = DateTime.Now;
                        DateTime fromDate;
                        DateTime toDate = now;
                        DateTime[] dateArray = new DateTime[2];
                        if (grade == "IX" || grade == "X")
                        {
                            fromDate = new DateTime(now.Year, 1, 1);
                            toDate = new DateTime(now.Year, 12, 31);
                        }
                        else
                        {
                            if (now.Month > 5)
                            {
                                fromDate = new DateTime(now.Year, 6, 1);
                            }
                            else { fromDate = new DateTime(now.Year - 1, 6, 1); }
                        }

                        dateArray[0] = fromDate; dateArray[1] = toDate;
                        criteria.Add("IncidentDate", dateArray);
                        criteria.Add("Assess360Id", a360.Id);
                        Assess360Point Assess360Point = new Assess360Point();
                        Assess360Point.StudName = a360.Name;
                        Assess360Point.Id = i++;
                        Assess360Point.ReportGenDate = DateTime.Now;
                        Assess360Point.AcademicYear = a360.AcademicYear;
                        Assess360Point.Campus = a360.Campus;
                        Assess360Point.Grade = a360.Grade;
                        Assess360Point.Section = a360.Section;
                        Assess360Point.DateCreated = a360.DateCreated;
                        Assess360Point.IdNo = a360.IdNo;
                        Assess360Point.RequestNo = a360.RequestNo;
                        Assess360Point.StudentId = a360.StudentId;
                        //get the complete list for the month
                        //Dictionary<long, IList<Assess360Component>> StudCharacterAssess = PSF.GetListWithEQSearchCriteriaCount<Assess360Component>(0, 10000, string.Empty, string.Empty, criteria);
                        //Character calculation part
                        IList<StudCharacterAssessment> CharAssessment = this.GetStudCharacterAssessmentForAssess360Id(a360.Id);
                        if (CharAssessment != null && CharAssessment.Count > 0)
                        {
                            Consolidation = CharAssessment.FirstOrDefault().FinalMark;
                            Assess360Point.Character = CharAssessment.FirstOrDefault().FinalMark.Value.ToString("0.00");
                        }
                        else
                        {
                            Consolidation += 20;
                            Assess360Point.Character = "20";
                        }
                        //Attendence and punctuality part
                        IList<StudAttenPuncAssessment> AttnPuncAssessment = this.GetStudAttenPuncAssessmentForAssess360Id(a360.Id);
                        if (AttnPuncAssessment != null && AttnPuncAssessment.Count > 0)
                        {
                            Consolidation += AttnPuncAssessment.FirstOrDefault().FinalMark;
                            Assess360Point.AttPunctuality = AttnPuncAssessment.FirstOrDefault().FinalMark.Value.ToString("0.00");
                        }
                        else
                        {
                            Consolidation += 10;
                            Assess360Point.AttPunctuality = "10";
                        }
                        //Homework completion part
                        IList<StudHwCompletion> HWComplAssessment = this.GetStudHwCompletionForAssess360Id(a360.Id);
                        if (HWComplAssessment != null && HWComplAssessment.Count > 0)
                        {
                            Consolidation += HWComplAssessment.FirstOrDefault().FinalMark;
                            Assess360Point.HwCompletion = HWComplAssessment.FirstOrDefault().FinalMark.Value.ToString("0.00");
                        }
                        else
                        {
                            Consolidation += 5;
                            Assess360Point.HwCompletion = "5";
                        }
                        //Homework accuracy part
                        IList<StudHWAccuracy> HWAccuAssessment = this.GetStudHWAccuracyForAssess360Id(a360.Id);
                        decimal HWAccuracyAvg;
                        if (HWAccuAssessment != null && HWAccuAssessment.Count > 0)
                        {
                            //find average
                            var a = from e in HWAccuAssessment select e.Mark;
                            HWAccuracyAvg = Convert.ToDecimal(a.ToArray().Average());
                            //if (Convert.ToInt16(HWAccuracyAvg) == 0)
                            //{
                            //    Consolidation += 15;
                            //    Assess360Point.HwAccuracy = "15";
                            //}
                            //else
                            //{
                            Consolidation += HWAccuracyAvg;
                            Assess360Point.HwAccuracy = HWAccuracyAvg.ToString("0.00");
                            // }
                        }
                        else
                        {
                            Consolidation += 15;
                            Assess360Point.HwAccuracy = "15";
                        }
                        //Copied homework
                        IList<StudCopiedhomework> StudCopiedhomework = this.GetStudCopiedhomeworkForAssess360Id(a360.Id);
                        decimal copy;
                        if (StudCopiedhomework != null && StudCopiedhomework.Count > 0)
                        {
                            //find average
                            var a = from e in StudCopiedhomework select e.Mark;
                            copy = Convert.ToDecimal(a.ToArray().Sum());
                            if (Convert.ToInt16(copy) > 0)
                            {
                                Consolidation -= copy;
                            }
                        }
                        //Weekly Chapter test part
                        IList<WeeklyTestForStud> WeeklyTestAssessment = this.GetStudWeeklyTestForAssess360Id(a360.Id);
                        if (WeeklyTestAssessment != null && WeeklyTestAssessment.Count > 0)
                        {
                            decimal weeklyTest;
                            if (WeeklyTestAssessment != null && WeeklyTestAssessment.Count > 0)
                            {
                                //find average
                                var a = from e in WeeklyTestAssessment select e.Mark;
                                weeklyTest = Convert.ToDecimal(a.ToArray().Average());
                                //if (Convert.ToInt16(weeklyTest) == 0)
                                //{
                                //    Consolidation += 20; Assess360Point.WkChapterTests = "20";
                                //}
                                //else 
                                //{
                                Consolidation += weeklyTest; Assess360Point.WkChapterTests = weeklyTest.ToString("0.00");
                                //}
                            }
                        }
                        else { Consolidation += 20; Assess360Point.WkChapterTests = "20"; }
                        //SLC parent Assessment
                        IList<StudSLCAssessment> SLCTestAssessment = this.GetStudSLCAssessmentForAssess360Id(a360.Id);
                        if (SLCTestAssessment != null && SLCTestAssessment.Count > 0)
                        {
                            Consolidation += SLCTestAssessment.FirstOrDefault().Mark;
                            Assess360Point.SLCParentAssessment = SLCTestAssessment.FirstOrDefault().Mark.ToString("0.00");
                        }
                        else { Consolidation += 5; Assess360Point.SLCParentAssessment = "5"; }
                        //Term assessment
                        IList<StudTermAssessment> StudTermAssessment = this.GetStudTermAssessmentForAssess360Id(a360.Id);
                        if (StudTermAssessment != null && StudTermAssessment.Count > 0)
                        {
                            decimal TermAssess;
                            if (StudTermAssessment != null && StudTermAssessment.Count > 0)
                            {
                                //find average
                                var a = from e in StudTermAssessment select e.Mark;
                                TermAssess = Convert.ToDecimal(a.ToArray().Average());
                                //if (Convert.ToInt16(TermAssess) == 0)
                                //{
                                // Consolidation += 25;
                                // Assess360Point.TermAssessment = "25";
                                // }
                                //    else 
                                //{ 
                                Consolidation += TermAssess; Assess360Point.TermAssessment = TermAssess.ToString("0.00");
                                // }
                            }
                        }
                        else { Consolidation += 25; Assess360Point.TermAssessment = "25"; }
                        Assess360Point.Total = Consolidation.Value.ToString("0.00");
                        retValue.Add(Assess360Point);
                    }
                }
            }
            return retValue;
        }

        public Dictionary<long, IList<Assess360AssessmentSubgrids>> GetAssessListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<Assess360AssessmentSubgrids>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<Assess360AssessmentStaffNames>> GetStaffNamesBasedOnAssessGroupIdListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<Assess360AssessmentStaffNames>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<Assess360Assignment>> GetAssess360AssignmentListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<Assess360Assignment>(page, pageSize, sortType, sortby, criteria);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<NotEnteredCount>> GetNotEnteredAssignmentListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<NotEnteredCount>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable NotEnteredStudentList(string TotalCountPerDay1) //Administrative
        {

            try
            {
                return PSF.ExecuteSqlUsingSQLCommand(TotalCountPerDay1);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }
        public DataTable EnteredStudentList(string TotalCountPerDay1) //Administrative
        {

            try
            {
                return PSF.ExecuteSqlUsingSQLCommand(TotalCountPerDay1);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }
        //  public Dictionary<long, IList<Assess360BulkInsert>> GetAssess360BulkInsertListWithPagingAndCriteriaWithAlias(int? page, int? pageSize, string sortby, string sortType, string name, string[] values, Dictionary<string, object> criteria, string[] alias)
        public Dictionary<long, IList<Assess360BulkInsert>> GetAssess360BulkInsertListWithPagingAndCriteriaWithAlias(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, string name, string[] values, string[] alias)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCountArray<Assess360BulkInsert>(page, pageSize, sortby, sortType, name, values, criteria, alias);
                //  return PSF.GetListWithEQSearchCriteriaCount<Assess360BulkInsert>(page, pageSize, sortby, sortType, criteria, name, values, alias);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region start new subject based semester marks
        public Dictionary<long, IList<SubjectStudentTemplate>> GetSubjectMarksViewListWithCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<SubjectStudentTemplate>(page, pageSize, sortType, sortby, criteria);

            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<SubjectMarks>> GetSubjectMarksViewListWithCriteria1(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                //IList<SubjectMarks> hqlQueryTest = PSF.GetListByName<SubjectMarks>("from " + typeof(SubjectMarks) + " left join fetch cat.kittens child left join fetch child.kittens");
                return PSF.GetListWithEQSearchCriteriaCount<SubjectMarks>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<Totalsemlist>> GetTotalSemListWithCriteria1(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                //IList<SubjectMarks> hqlQueryTest = PSF.GetListByName<SubjectMarks>("from " + typeof(SubjectMarks) + " left join fetch cat.kittens child left join fetch child.kittens");
                return PSF.GetListWithEQSearchCriteriaCount<Totalsemlist>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public SubjectMarks CheckExistdatainSubjectMarks(long Id)
        {

            try
            {
                return PSF.Get<SubjectMarks>(Id);
            }
            catch (Exception) { throw; }
        }

        public long CreateOrUpdateSubjectMarks(SubjectMarks sub)
        {
            try
            {
                if (sub != null) { PSF.SaveOrUpdate<SubjectMarks>(sub); }
                else { throw new Exception("Value is required and it cannot be null.."); }
                return sub.Id;
            }
            catch (Exception) { throw; }
        }

        public Dictionary<long, IList<StudentFinalResult_vw>> GetStudentFinalResultWidthSubjectWiseList(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<StudentFinalResult_vw>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<Vw_FinalResult>> GetFinalResultsListWidthCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<Vw_FinalResult>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<StaffsNamesForSubjectMarks>> GetStaffsNamesForSubjectMarksListWithCriteria1(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<StaffsNamesForSubjectMarks>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion end new subject based semester marks

        #region start test
        public Dictionary<long, IList<ClassforIXAB>> GetClassIXABSubjectListWithCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                //IList<SubjectMarks> hqlQueryTest = PSF.GetListByName<SubjectMarks>("from " + typeof(SubjectMarks) + " left join fetch cat.kittens child left join fetch child.kittens");
                return PSF.GetListWithEQSearchCriteriaCount<ClassforIXAB>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<ClassforVItoVIII>> GetClassVItoVIIISubjectListWithCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<ClassforVItoVIII>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<ClassforIXCD>> GetClassIXCDSubjectListWithCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<ClassforIXCD>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion end test

        public Assess360 GetAssess360ByStudentId(long StudentId)
        {
            try
            {
                return PSF.Get<Assess360>("StudentId", StudentId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Ass360 Request Creation
        public Dictionary<long, IList<StudTempForAss360ReqCreation>> GetStudTemplateListWithCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<StudTempForAss360ReqCreation>(page, pageSize, sortType, sortby, criteria);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CreateOrUpdateAssess360List(Assess360 ass360)
        {
            try
            {
                bool retValue = false;
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(ass360.Campus)) { criteria.Add("Campus", ass360.Campus); }
                if (!string.IsNullOrEmpty(ass360.Grade)) { criteria.Add("Grade", ass360.Grade); }
                if (!string.IsNullOrEmpty(ass360.Section)) { criteria.Add("Section", ass360.Section); }
                criteria.Add("AdmissionStatus", "Registered");
                Dictionary<long, IList<StudTempForAss360ReqCreation>> GetStudentList = this.GetStudTemplateListWithCriteria(0, 99999, string.Empty, string.Empty, criteria);
                IList<Assess360> studlist = new List<Assess360>();
                if (GetStudentList != null && GetStudentList.FirstOrDefault().Value.Count > 0 && GetStudentList.FirstOrDefault().Key > 0)
                {

                    foreach (var item in GetStudentList.FirstOrDefault().Value)
                    {
                        Assess360 starc = new Assess360();
                        starc.StudentId = item.Id;
                        starc.Campus = item.Campus;
                        starc.Grade = item.Grade;
                        starc.Section = item.Section;
                        starc.Name = item.Name;
                        starc.AcademicYear = item.AcademicYear;
                        starc.IdNo = item.NewId;
                        starc.CreatedBy = ass360.CreatedBy;
                        starc.UserRole = ass360.UserRole;
                        starc.DateCreated = DateTime.Now;
                        studlist.Add(starc);
                    }

                    foreach (Assess360 item in studlist)
                    {
                        Assess360 Assess360Exists = PSF.Get<Assess360>("StudentId", item.StudentId, "Grade", item.Grade, "AcademicYear", item.AcademicYear);
                        if (Assess360Exists == null)
                        {
                            item.IsActive = true;
                            item.ConsolidatedMarks = "100.00 / 100";
                            PSF.SaveOrUpdate<Assess360>(item);
                            item.RequestNo = "A360-" + item.IdNo + "-" + item.Id.ToString();
                            PSF.SaveOrUpdate<Assess360>(item);
                        }
                    }
                    retValue = true;
                }

                return retValue;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally { }
        }


        #endregion

        #region Show Past Year Subject Marks
        public Dictionary<long, IList<ShowPastYearSubjectMarks>> GetMarklistfromSubjectMarksWithCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<ShowPastYearSubjectMarks>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<ShowPastYearFinalResults_Vw>> GetShowPastYearFinalResults_VwListWithCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<ShowPastYearFinalResults_Vw>(page, pageSize, sortType, sortby, criteria);

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Newly added by Micheal
        public Dictionary<long, IList<Assess360>> GetAssessDetailsListWithPaging(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<Assess360>> retValue = new Dictionary<long, IList<Assess360>>();
                return PSF.GetListWithSearchCriteriaCount<Assess360>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        public Dictionary<long, IList<NotUsedAssignmentList>> GetNotUsedAssignmentListWithPagingAndCriteriaEqSearch(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<NotUsedAssignmentList>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public Dictionary<long, IList<NotUsedAssignmentList>> GetNotUsedAssignmentListWithPagingAndCriteriaLikeSearch(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithLikeSearchCriteriaCount<NotUsedAssignmentList>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #region For Admin Template
        public Dictionary<long, IList<Assess360AdminTemplate_vw>> GetAssess360AdminTemplate_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<Assess360AdminTemplate_vw>(page, pageSize, sortType, sortby, criteria);

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        public Dictionary<long, IList<Assess360_ARC>> GetAssess360_ARCListWithPagingAndCriteriaWithAlias(int? page, int? pageSize, string sortby, string sortType, string name, string[] values, Dictionary<string, object> criteria, string[] alias)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCountArray<Assess360_ARC>(page, pageSize, sortby, sortType, name, values, criteria, alias);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Assess360_ARC GetAssess360_ARCById(long Id)
        {
            try
            {
                return PSF.Get<Assess360_ARC>(Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<StudCharacterAssessment_Old> GetStudCharacterAssessment_OldForAssess360Id(long Assess360Id)
        {
            try
            {
                IList<StudCharacterAssessment_Old> StudCharacterAssess = PSF.GetList<StudCharacterAssessment_Old>("Assess360Id", Assess360Id);
                return StudCharacterAssess;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<StudAttenPuncAssessment_Old> GetStudAttenPuncAssessment_OldForAssess360Id(long Assess360Id)
        {
            try
            {
                IList<StudAttenPuncAssessment_Old> StudAttenPuncAssessment = PSF.GetList<StudAttenPuncAssessment_Old>("Assess360Id", Assess360Id);
                return StudAttenPuncAssessment;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<StudHwCompletion_Old> GetStudHwCompletion_OldForAssess360Id(long Assess360Id)
        {
            try
            {
                IList<StudHwCompletion_Old> StudHwCompletion = PSF.GetList<StudHwCompletion_Old>("Assess360Id", Assess360Id);
                return StudHwCompletion;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<StudHWAccuracy_Old> GetStudHWAccuracy_OldForAssess360Id(long Assess360Id)
        {
            try
            {
                IList<StudHWAccuracy_Old> StudHWAccuracy = PSF.GetList<StudHWAccuracy_Old>("Assess360Id", Assess360Id);
                return StudHWAccuracy;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<StudCopiedhomework_Old> GetStudCopiedhomework_OldForAssess360Id(long Assess360Id)
        {
            try
            {
                IList<StudCopiedhomework_Old> StudCopiedhomework = PSF.GetList<StudCopiedhomework_Old>("Assess360Id", Assess360Id);
                return StudCopiedhomework;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<WeeklyTestForStud_Old> GetStudWeeklyTestFor_OldAssess360Id(long Assess360Id)
        {
            try
            {
                IList<WeeklyTestForStud_Old> WeeklyTestForStud = PSF.GetList<WeeklyTestForStud_Old>("Assess360Id", Assess360Id);
                return WeeklyTestForStud;
                //return PSF.GetListWithExactSearchCriteriaCount<AssessGroupMaster>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Semester
        public Dictionary<long, IList<SemesterMaster>> GetSemesterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<SemesterMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long SaveOrUpdateSemester(SemesterMaster semester)
        {
            try
            {
                if (semester != null)
                {
                    PSF.SaveOrUpdate<SemesterMaster>(semester);
                }
                else
                { throw new Exception("All Fields are required and it cannot be null.."); }
                return semester.SemesterMasterId;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public SemesterMaster GetSemesterById(long Id)
        {
            try
            {
                SemesterMaster semester = null;
                if (Id > 0)
                {
                    semester = PSF.Get<SemesterMaster>(Id);
                }
                return semester;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public long DeleteSemester(SemesterMaster semester)
        {
            try
            {
                if (semester != null)
                {
                    PSF.Delete<SemesterMaster>(semester);
                }
                else
                { throw new Exception("All Fields are required and it cannot be null.."); }
                return 0;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        public long SaveOrUpdateReportCardAchievement(RptCardAchievement RptAchieve)
        {
            try
            {
                //logic to check before saving
                PSF.SaveOrUpdate<RptCardAchievement>(RptAchieve);
                return RptAchieve.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<RptCardAchievement>> GetStudentAchievementListWithPagingAndInCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<RptCardAchievement>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool DeleteStudentAchievement(long[] studAchieveIds)
        {
            try
            {
                bool retValue = false;
                IList<RptCardAchievement> List = PSF.GetListByIds<RptCardAchievement>(studAchieveIds);
                PSF.DeleteAll<RptCardAchievement>(List);
                retValue = true;
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<RptCardMarkWeightingMaster>> GetReportCardMarkWeightingsWihtCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<RptCardMarkWeightingMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<WeightingMasterCrossCheck_Vw>> GetWeightingsMasterCrossCheckListWihtCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<WeightingMasterCrossCheck_Vw>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long SaveOrUpdateRptCardMarkWeightingMaster(RptCardMarkWeightingMaster rptCardWeighting)
        {
            try
            {
                if (rptCardWeighting != null)
                {
                    PSF.SaveOrUpdate<RptCardMarkWeightingMaster>(rptCardWeighting);
                }
                else
                { throw new Exception("All Fields are required and it cannot be null.."); }
                return rptCardWeighting.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public RptCardMarkWeightingMaster GetRptCardMarkWeightingMasterById(long Id)
        {
            try
            {
                RptCardMarkWeightingMaster rtpweighting = null;
                if (Id > 0)
                {
                    rtpweighting = PSF.Get<RptCardMarkWeightingMaster>(Id);
                }
                return rtpweighting;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public long DeleteRptCardMarkWeightingMaster(RptCardMarkWeightingMaster rptCardWeighting)
        {
            try
            {
                if (rptCardWeighting != null)
                {
                    PSF.Delete<RptCardMarkWeightingMaster>(rptCardWeighting);
                }
                else
                { throw new Exception("All Fields are required and it cannot be null.."); }
                return 0;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #region "Rotativa Pdf"
        public Dictionary<long, IList<Assess360MarksCaluctionForAStudent_Vw>> GetAssess360MarksCaluctionForAStudent_VwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<Assess360MarksCaluctionForAStudent_Vw>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<Assess360MarkcalculationForChart_Vw>> GetAssess360MarkcalculationForChart_VwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<Assess360MarkcalculationForChart_Vw>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<IXIGCSEWeightingsCalculation_Vw>> GetAssess360WeightingsMarksCaluctionForAStudent_VwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<IXIGCSEWeightingsCalculation_Vw>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion "End"

        public IList Assess360MarkCalculationForAStudent(long Assess360Id)
        {
            try
            {
                return PSF.ExecuteSql("EXECUTE Assess360MarkCalculationForAStudent '" + Assess360Id + "'");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public StudentTemplate GetStudentTemplateById(long StudentId)
        {
            try
            {
                StudentTemplate stud = null;
                if (StudentId > 0)
                    stud = PSF.Get<StudentTemplate>(StudentId);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return stud;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region "Xth IB ReportCard"
        public Dictionary<long, IList<SummativeAssessment_vw>> GetSummativeAssessmenttestResultList(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<SummativeAssessment_vw>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<SummativeAssessmentVIVII_vw>> GetSummativeAssessmenttestVIVIIResultList(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<SummativeAssessmentVIVII_vw>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<ComparitiveSAReport_vw>> GetComparativeSAReportResultList(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<ComparitiveSAReport_vw>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<SummativeMarkAnalysisIX_Vw>> GetSummativeMarkAnalysisWithCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<SummativeMarkAnalysisIX_Vw>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception) { throw; }

        }
        public Dictionary<long, IList<SummativeMarkAnalysisVIVIII_Vw>> GetSummativeMarkAnalysisVIVIII(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<SummativeMarkAnalysisVIVIII_Vw>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception) { throw; }

        }

        public Dictionary<long, IList<SumativeStudentDetails_Vw>> GetSumativeStudentListWithPagingAndInCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likecriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<SumativeStudentDetails_Vw>(page, pageSize, sortby, sortType, criteria, likecriteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<SumativeStudentDetailsIX_Vw>> GetSumativeStudentListIXWithPagingAndInCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likecriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<SumativeStudentDetailsIX_Vw>(page, pageSize, sortby, sortType, criteria, likecriteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        public AssessCompMaster GetAssessGroupUsingId(int StudentId)
        {
            try
            {
                return PSF.Get<AssessCompMaster>("GroupId", StudentId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<FormattiveAssessment_Vw>> GetFAMarkListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<FormattiveAssessment_Vw>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<ComparitiveFAReport_vw>> GetComparativeFAReportResultList(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<ComparitiveFAReport_vw>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<FA_HW_Vw>> GetFAHWMarkListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<FA_HW_Vw>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<ComparativeFAHWReport_Vw>> GetComparativeFAHWReportResultList(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<ComparativeFAHWReport_Vw>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        
    }
}
