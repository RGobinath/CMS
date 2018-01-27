using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mail;
using TIPS.Entities;
using TIPS.Entities.StudentsReportEntities;
using PersistenceFactory;
using System.Threading.Tasks;
using System.Configuration;

//using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Net.NetworkInformation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Collections;
using TIPS.Entities.ReportEntities;

namespace TIPS.Component
{
    public class StudentsReportBC
    {
        PersistenceServiceFactory PSF = null;
        public StudentsReportBC()
        {
            List<String> Assembly = new List<String>();
            Assembly.Add("TIPS.Entities");
            Assembly.Add("TIPS.Entities.Assess");
            Assembly.Add("TIPS.Entities.TicketingSystem");
            Assembly.Add("TIPS.Entities.TaskManagement");
            PSF = new PersistenceServiceFactory(Assembly);
        }
        #region MIS Report
        public Dictionary<long, IList<MISReport_vw>> GetMISReport_vwListWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<MISReport_vw>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<long, IList<MISMailMaster>> GetMISMailMasterListWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<MISMailMaster>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<long, IList<MISMailStatus>> GetMISMailStatusListWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<MISMailStatus>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public long SaveOrUpdateMISMailStatusDetails(MISMailStatus Status)
        {
            try
            {

                if (Status != null)
                    PSF.SaveOrUpdate<MISMailStatus>(Status);
                return Status.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<CampusEmailId>> GetCampusEmailId(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<CampusEmailId>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public MISMailStatus GetMISMailStatusDetailsById(long Id)
        {
            try
            {
                MISMailStatus MISMailStatusDetails = null;

                if (Id > 0)
                    MISMailStatusDetails = PSF.Get<MISMailStatus>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return MISMailStatusDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<MISOverAllReport_vw>> GetMISOverAllReport_vwListWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<MISOverAllReport_vw>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<long, IList<CampusMaster>> GetCampusMasterListWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<CampusMaster>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region MIS MAIL MASTER
        public long SaveOrUpdateMISMailMasterDetails(MISMailMaster MailMaster)
        {
            try
            {
                if (MailMaster != null)
                    PSF.SaveOrUpdate<MISMailMaster>(MailMaster);
                else { throw new Exception("All Fields are required and it cannot be null.."); }
                return MailMaster.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public MISMailMaster GetMISMailMasterDetailsById(long Id)
        {
            try
            {
                MISMailMaster MISMailMasterDetails = null;

                if (Id > 0)
                    MISMailMasterDetails = PSF.Get<MISMailMaster>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return MISMailMasterDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long GetDeleteMISMailMasterrowById(MISMailMaster MailMaster)
        {
            try
            {
                if (MailMaster != null)
                    PSF.Delete<MISMailMaster>(MailMaster);
                else { throw new Exception("Value is required and it cannot be null.."); }
                return 0;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region MISReport Service
        public bool SendEmailFromWindowsService(string campus)
        {
            try
            {
                bool retValue = false;
                DateTime Currnt_Date = DateTime.Now;
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Clear();
                criteria.Add("EmailType", "MISReport");
                criteria.Add("IsMailNeeded", true);
                Dictionary<long, IList<MISMailMaster>> MISMailMasterDetails = GetMISMailMasterListWithPaging(null, 9999, string.Empty, string.Empty, criteria);
                if (MISMailMasterDetails != null && MISMailMasterDetails.Count > 0 && MISMailMasterDetails.FirstOrDefault().Value.Count > 0)
                {
                    foreach (MISMailMaster item in MISMailMasterDetails.First().Value)
                    {
                        string DayName = DateTime.Now.ToString("dddd");
                        if (item.SentCategory == "Daily" || (item.SentCategory == "Weekly" && DayName == "Monday") || (item.SentCategory == "Weekly" && DayName == "Wednesday") || (item.SentCategory == "Weekly" && DayName == "Friday"))
                        {
                            MISMailStatus MISMailStatus = MailSendStatus(item.Id, item.EmailId, item.SentCategory, item.EmailType);
                            if (MISMailStatus == null || MISMailStatus.IsSent == false)
                            {
                                if (MISMailStatus == null)
                                {
                                    MISMailStatus = MISMailStatusFirstInsert(item.Id, item.EmailId, item.Campus, item.SentCategory, item.EmailType);
                                }
                                string CampusVal = item.Campus;
                                byte[] MISExcelFile;
                                bool ret;
                                int Currnt_Year = Currnt_Date.Year;
                                string CurrentAcademicYear = string.Empty;
                                string NextAcademicYear = string.Empty;
                                if (Currnt_Date.Month >= 6 && Currnt_Date.Month <= 12)
                                {
                                    CurrentAcademicYear = Currnt_Year + "-" + (Currnt_Year + 1);
                                    NextAcademicYear = (Currnt_Year + 1) + "-" + (Currnt_Year + 2);
                                }
                                if (Currnt_Date.Month >= 1 && Currnt_Date.Month <= 4)
                                {
                                    CurrentAcademicYear = (Currnt_Year - 1) + "-" + Currnt_Year;
                                    NextAcademicYear = (Currnt_Year) + "-" + (Currnt_Year + 1);
                                }
                                string FileName = "";
                                string MailBody;
                                MailBody = GetBodyofMail();
                                DataTable table = new DataTable();
                                DataSet Workbookset = new DataSet("Work Book");
                                table.TableName = "MIS Report";
                                Workbookset.Tables.Add(table);
                                if (CampusVal == "All")
                                {
                                    MISExcelFile = AllCampusMISReportExcel(Workbookset, CurrentAcademicYear, NextAcademicYear);
                                    FileName = "MISReportForAllCampus";
                                    ret = email_send(MISExcelFile, FileName, CampusVal, item.EmailId, MailBody, item.EmailType);
                                }
                                else
                                {
                                    MISExcelFile = SingleMISReportExcel(Workbookset, CampusVal, CurrentAcademicYear);
                                    FileName = "MISReportFor-" + CampusVal;
                                    ret = email_send(MISExcelFile, FileName, CampusVal, item.EmailId, MailBody, item.EmailType);
                                }
                                if (ret == true)
                                {
                                    string TodayDate = DateTime.Now.ToShortDateString();
                                    MISMailStatus.IsSent = true;
                                    MISMailStatus.CheckDate = TodayDate;
                                    MISMailStatus.Campus = item.Campus;
                                    MISMailStatus.EmailId = item.EmailId;
                                    MISMailStatus.EmailRefId = item.Id;
                                    MISMailStatus.SentCategory = item.SentCategory;
                                    MISMailStatus.EmailType = item.EmailType;
                                    MISMailStatus.SentDate = Currnt_Date;
                                    SaveOrUpdateMISMailStatusDetails(MISMailStatus);
                                }

                            }
                        }
                    }
                }
                return retValue;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
            }
            return false;
        }
        public byte[] SingleMISReportExcel(DataSet Workbookset, string Campus, string AcademicYear)
        {
            try
            {
                using (ExcelPackage Epkg = new ExcelPackage())
                {
                    int TableCount = Workbookset.Tables.Count;
                    ExcelWorksheet ews = Epkg.Workbook.Worksheets.Add(Workbookset.Tables[0].TableName);
                    ews.View.ZoomScale = 100;
                    ews.View.ShowGridLines = false;
                    ews.Cells["H1:L42"].Style.Font.Name = "Calibri";
                    ews.Cells["H1:L42"].Style.Font.Size = 13;
                    for (int k = 9; k < 42; k++)
                    {
                        ews.Cells["H" + k + ":I" + (k + 1)].Merge = true;
                        ews.Cells["J" + k + ":J" + (k + 1)].Merge = true;
                        ews.Cells["K" + k + ":K" + (k + 1)].Merge = true;
                        ews.Cells["L" + k + ":L" + (k + 1)].Merge = true;
                        k++;
                    }

                    ews.Cells["H9:I10"].Value = "Play School";
                    ews.Cells["H11:I12"].Value = "PRE-KG";
                    ews.Cells["H13:I14"].Value = "LKG";
                    ews.Cells["H15:I16"].Value = "UKG";
                    ews.Cells["H17:I18"].Value = "Grade-I";
                    ews.Cells["H19:I20"].Value = "Grade-II";
                    ews.Cells["H21:I22"].Value = "Grade-III";
                    ews.Cells["H23:I24"].Value = "Grade-IV";
                    ews.Cells["H25:I26"].Value = "Grade-V";
                    ews.Cells["H27:I28"].Value = "Grade-VI";
                    ews.Cells["H29:I30"].Value = "Grade-VII";
                    ews.Cells["H31:I32"].Value = "Grade-VIII";
                    ews.Cells["H33:I34"].Value = "Grade-IX";
                    ews.Cells["H35:I36"].Value = "Grade-X";
                    ews.Cells["H37:I38"].Value = "DP-1/Grade 11";
                    ews.Cells["H39:I40"].Value = "DP-2/Grade 12";
                    ews.Cells["H41:I42"].Value = "Total";

                    //ews.Cells["H9:I42"].Style.Font.Bold = true;
                    ews.Cells["J9:L42"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ews.Cells["H3:L8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ews.Cells["H1:L8"].Style.Font.Bold = true;

                    //For Title
                    ews.Cells["H1:L2"].Merge = true;
                    ews.Cells["H1:L2"].Value = "MIS REPORT";
                    ews.Cells["H1:L2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                    //For Campus 1
                    ews.Cells["H3:L4"].Merge = true;
                    ews.Cells["H3:L4"].Value = Campus;
                    ews.Cells["H5:L6"].Merge = true;
                    ews.Cells["H5:L6"].Value = AcademicYear;
                    ews.Cells["H7:I8"].Merge = true;
                    ews.Cells["H7:I8"].Value = "Grades";
                    ews.Cells["J7:J8"].Merge = true;
                    ews.Cells["J7:J8"].Value = "Boys";
                    ews.Cells["K7:K8"].Merge = true;
                    ews.Cells["K7:K8"].Value = "Girls";
                    ews.Cells["L7:L8"].Merge = true;
                    ews.Cells["L7:L8"].Value = "Total";

                    //int TotalGradeCount;
                    //CellStartVal = 9;
                    //CellEndVal = 42;
                    int TotBoys = 0;
                    int TotGirls = 0;
                    int CampusTotal = 0;
                    int i = 9;
                    int j = 0;
                    string Grade = "";
                    IList<MISReport_vw> MISReportDetails = new List<MISReport_vw>();
                    for (j = 0; j < 16; j++)
                    {
                        Grade = GetGradeName(j);
                        MISReportDetails = MISReportWithCriteria(Campus, Grade, AcademicYear);
                        if (MISReportDetails.Count > 0)
                        {
                            ews.Cells["J" + i + ":J" + (i + 1)].Value = MISReportDetails[0].Boys;
                            TotBoys = TotBoys + Convert.ToInt32(MISReportDetails[0].Boys);
                            ews.Cells["K" + i + ":K" + (i + 1)].Value = MISReportDetails[0].Girls;
                            TotGirls = TotGirls + Convert.ToInt32(MISReportDetails[0].Girls);
                            ews.Cells["L" + i + ":L" + (i + 1)].Value = MISReportDetails[0].Total;
                            CampusTotal = CampusTotal + Convert.ToInt32(MISReportDetails[0].Total);
                            i = i + 2;
                        }
                        else
                        {
                            ews.Cells["J" + i + ":J" + (i + 1)].Value = "";
                            ews.Cells["K" + i + ":K" + (i + 1)].Value = "";
                            ews.Cells["L" + i + ":L" + (i + 1)].Value = "";
                            i = i + 2;
                        }

                    }
                    ews.Cells["J41:J42"].Value = TotBoys;
                    ews.Cells["K41:K42"].Value = TotGirls;
                    ews.Cells["L41:L42"].Value = CampusTotal;

                    ews.Cells["H1:L42"].Style.Border.Bottom.Style = ExcelBorderStyle.Hair;
                    ews.Cells["H1:L42"].Style.Border.Top.Style = ExcelBorderStyle.Hair;
                    ews.Cells["H1:L42"].Style.Border.Left.Style = ExcelBorderStyle.Hair;
                    ews.Cells["H1:L42"].Style.Border.Right.Style = ExcelBorderStyle.Hair;

                    if (!string.IsNullOrEmpty(Campus)) { Campus = Campus.Replace(" ", ""); }
                    //Date for Filename attachment
                    string Todaydate = DateTime.Now.ToString("dd/MM/yyyy");
                    string FileName = "MISReportFor-" + Campus + "-On-" + Todaydate; ;
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;  filename=" + FileName + ".xlsx");
                    byte[] File = Epkg.GetAsByteArray();
                    HttpContext.Current.Response.BinaryWrite(File);
                    // bool returnval = email_send(File, FileName, CampusVal);
                    //HttpContext.Current.Response.End();
                    return File;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
            }
            return null;
        }
        public byte[] AllCampusMISReportExcel(DataSet Workbookset, string CurrentAcademicYear, string NextAcademicYear)
        {
            try
            {
                //StudentsReportService SRS = new StudentsReportService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                List<CampusMaster> CampusList = new List<CampusMaster>();

                string sord = "";
                sord = sord == "desc" ? "Desc" : "Asc";
                string sidx = "Flag";

                Dictionary<long, IList<CampusMaster>> CampusMasterList = GetCampusMasterListWithPaging(0, 9999, sidx, sord, criteria);
                CampusList = CampusMasterList.FirstOrDefault().Value.ToList();

                IList<MISReport_vw> MISReportDetails = new List<MISReport_vw>();
                //IList<MISReport_vw> MISReportDetails2 = new List<MISReport_vw>();
                using (ExcelPackage Epkg = new ExcelPackage())
                {
                    int TableCount = Workbookset.Tables.Count;
                    ExcelWorksheet ews = Epkg.Workbook.Worksheets.Add(Workbookset.Tables[0].TableName);
                    ews.View.ZoomScale = 100;
                    ews.View.ShowGridLines = false;
                    //int img = 0;
                    //ews.Row(img * 5).Height = 39.00D;
                    ews.Cells["A1:CN42"].Style.Font.Name = "Calibri";
                    ews.Cells["A1:CN50"].Style.Font.Size = 13;
                    ews.Column(1).Width = 20;
                    //For Title
                    ews.Cells["A1:CN1"].Merge = true;
                    ews.Cells["A1:CN1"].Value = "MIS REPORT";
                    ews.Cells["A1:CN1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ews.Cells["B2:CN21"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    for (int RowNum = 1; RowNum <= 21; RowNum++)
                    {
                        ews.Row(RowNum).Height = 30.00D;
                    }
                    #region For Grade Title
                    ews.Cells["A5"].Value = "Play School";
                    ews.Cells["A6"].Value = "PRE-KG";
                    ews.Cells["A7"].Value = "LKG";
                    ews.Cells["A8"].Value = "UKG";
                    ews.Cells["A9"].Value = "Grade-I";
                    ews.Cells["A10"].Value = "Grade-II";
                    ews.Cells["A11"].Value = "Grade-III";
                    ews.Cells["A12"].Value = "Grade-IV";
                    ews.Cells["A13"].Value = "Grade-V";
                    ews.Cells["A14"].Value = "Grade-VI";
                    ews.Cells["A15"].Value = "Grade-VII";
                    ews.Cells["A16"].Value = "Grade-VIII";
                    ews.Cells["A17"].Value = "Grade-IX";
                    ews.Cells["A18"].Value = "Grade-X";
                    ews.Cells["A19"].Value = "DP-1/Grade 11";
                    ews.Cells["A20"].Value = "DP-2/Grade 12";
                    ews.Cells["A21"].Value = "Total";
                    #endregion

                    #region For Campus 1
                    ews.Cells["B2:G2"].Merge = true;
                    ews.Cells["B2:G2"].Value = CampusList[0].ShowName;

                    //Current Academic Year
                    ews.Cells["B3:D3"].Merge = true;
                    ews.Cells["B3:D3"].Value = CurrentAcademicYear;
                    ews.Cells["B4"].Value = "Boys";
                    ews.Cells["C4"].Value = "Girls";
                    ews.Cells["D4"].Value = "Total";

                    int TotBoys = 0;
                    int TotGirls = 0;
                    int CampusTotal = 0;
                    int i = 5;
                    int j = 0;
                    string Grade = string.Empty;
                    for (j = 0; j < 15; j++)
                    {
                        Grade = GetGradeName(j);
                        MISReportDetails = MISReportWithCriteria(CampusList[0].Name, Grade, CurrentAcademicYear);
                        if (MISReportDetails.Count > 0)
                        {
                            ews.Cells["B" + i].Value = MISReportDetails[0].Boys;
                            TotBoys = TotBoys + Convert.ToInt32(MISReportDetails[0].Boys);
                            ews.Cells["C" + i].Value = MISReportDetails[0].Girls;
                            TotGirls = TotGirls + Convert.ToInt32(MISReportDetails[0].Girls);
                            ews.Cells["D" + i].Value = MISReportDetails[0].Total;
                            CampusTotal = CampusTotal + Convert.ToInt32(MISReportDetails[0].Total);
                            i = i + 1;
                        }
                        else
                        {
                            ews.Cells["B" + i].Value = "";
                            ews.Cells["C" + i].Value = "";
                            ews.Cells["D" + i].Value = "";
                        }

                    }
                    ews.Cells["B21"].Value = TotBoys;
                    ews.Cells["C21"].Value = TotGirls;
                    ews.Cells["D21"].Value = CampusTotal;

                    //Next Academic Year
                    ews.Cells["E3:G3"].Merge = true;
                    ews.Cells["E3:G3"].Value = NextAcademicYear;
                    ews.Cells["E4"].Value = "Boys";
                    ews.Cells["F4"].Value = "Girls";
                    ews.Cells["G4"].Value = "Total";

                    TotBoys = 0;
                    TotGirls = 0;
                    CampusTotal = 0;
                    i = 5;
                    j = 0;
                    Grade = string.Empty;
                    for (j = 0; j < 15; j++)
                    {
                        Grade = GetGradeName(j);
                        MISReportDetails = MISReportWithCriteria(CampusList[0].Name, Grade, NextAcademicYear);
                        if (MISReportDetails.Count > 0)
                        {
                            ews.Cells["E" + i].Value = MISReportDetails[0].Boys;
                            TotBoys = TotBoys + Convert.ToInt32(MISReportDetails[0].Boys);
                            ews.Cells["F" + i].Value = MISReportDetails[0].Girls;
                            TotGirls = TotGirls + Convert.ToInt32(MISReportDetails[0].Girls);
                            ews.Cells["G" + i].Value = MISReportDetails[0].Total;
                            CampusTotal = CampusTotal + Convert.ToInt32(MISReportDetails[0].Total);
                            i = i + 1;
                        }
                        else
                        {
                            ews.Cells["E" + i].Value = "";
                            ews.Cells["F" + i].Value = "";
                            ews.Cells["G" + i].Value = "";
                        }

                    }
                    ews.Cells["E21"].Value = TotBoys;
                    ews.Cells["F21"].Value = TotGirls;
                    ews.Cells["G21"].Value = CampusTotal;
                    #endregion
                    ews.Cells["H2:H21"].Merge = true;
                    #region For Campus 2
                    ews.Cells["I2:N2"].Merge = true;
                    ews.Cells["I2:N2"].Value = CampusList[1].ShowName;
                    //Current Academic Year
                    ews.Cells["I3:K3"].Merge = true;
                    ews.Cells["I3:K3"].Value = CurrentAcademicYear;
                    ews.Cells["I4"].Value = "Boys";
                    ews.Cells["J4"].Value = "Girls";
                    ews.Cells["K4"].Value = "Total";
                    TotBoys = 0;
                    TotGirls = 0;
                    CampusTotal = 0;
                    i = 5;
                    j = 0;
                    Grade = string.Empty;
                    for (j = 0; j < 15; j++)
                    {
                        Grade = GetGradeName(j);
                        MISReportDetails = MISReportWithCriteria(CampusList[1].Name, Grade, CurrentAcademicYear);
                        if (MISReportDetails.Count > 0)
                        {
                            ews.Cells["I" + i].Value = MISReportDetails[0].Boys;
                            TotBoys = TotBoys + Convert.ToInt32(MISReportDetails[0].Boys);
                            ews.Cells["J" + i].Value = MISReportDetails[0].Girls;
                            TotGirls = TotGirls + Convert.ToInt32(MISReportDetails[0].Girls);
                            ews.Cells["K" + i].Value = MISReportDetails[0].Total;
                            CampusTotal = CampusTotal + Convert.ToInt32(MISReportDetails[0].Total);
                            i = i + 1;
                        }
                        else
                        {
                            ews.Cells["I" + i].Value = "";
                            ews.Cells["J" + i].Value = "";
                            ews.Cells["K" + i].Value = "";
                        }

                    }
                    ews.Cells["I21"].Value = TotBoys;
                    ews.Cells["J21"].Value = TotGirls;
                    ews.Cells["K21"].Value = CampusTotal;
                    //Next Academic Year
                    ews.Cells["L3:N3"].Merge = true;
                    ews.Cells["L3:N3"].Value = NextAcademicYear;
                    ews.Cells["L4"].Value = "Boys";
                    ews.Cells["M4"].Value = "Girls";
                    ews.Cells["N4"].Value = "Total";

                    TotBoys = 0;
                    TotGirls = 0;
                    CampusTotal = 0;
                    i = 5;
                    j = 0;
                    Grade = string.Empty;
                    for (j = 0; j < 15; j++)
                    {
                        Grade = GetGradeName(j);
                        MISReportDetails = MISReportWithCriteria(CampusList[1].Name, Grade, NextAcademicYear);
                        if (MISReportDetails.Count > 0)
                        {
                            ews.Cells["L" + i].Value = MISReportDetails[0].Boys;
                            TotBoys = TotBoys + Convert.ToInt32(MISReportDetails[0].Boys);
                            ews.Cells["M" + i].Value = MISReportDetails[0].Girls;
                            TotGirls = TotGirls + Convert.ToInt32(MISReportDetails[0].Girls);
                            ews.Cells["N" + i].Value = MISReportDetails[0].Total;
                            CampusTotal = CampusTotal + Convert.ToInt32(MISReportDetails[0].Total);
                            i = i + 1;
                        }
                        else
                        {
                            ews.Cells["L" + i].Value = "";
                            ews.Cells["M" + i].Value = "";
                            ews.Cells["N" + i].Value = "";
                        }

                    }
                    ews.Cells["L21"].Value = TotBoys;
                    ews.Cells["M21"].Value = TotGirls;
                    ews.Cells["N21"].Value = CampusTotal;
                    #endregion
                    ews.Cells["O2:O21"].Merge = true;
                    #region For Campus 3
                    ews.Cells["P2:U2"].Merge = true;
                    ews.Cells["P2:U2"].Value = CampusList[2].ShowName;
                    //Current Academic Year
                    ews.Cells["P3:R3"].Merge = true;
                    ews.Cells["P3:R3"].Value = CurrentAcademicYear;
                    ews.Cells["P4"].Value = "Boys";
                    ews.Cells["Q4"].Value = "Girls";
                    ews.Cells["R4"].Value = "Total";
                    TotBoys = 0;
                    TotGirls = 0;
                    CampusTotal = 0;
                    i = 5;
                    j = 0;
                    Grade = string.Empty;
                    for (j = 0; j < 15; j++)
                    {
                        Grade = GetGradeName(j);
                        MISReportDetails = MISReportWithCriteria(CampusList[2].Name, Grade, CurrentAcademicYear);
                        if (MISReportDetails.Count > 0)
                        {
                            ews.Cells["P" + i].Value = MISReportDetails[0].Boys;
                            TotBoys = TotBoys + Convert.ToInt32(MISReportDetails[0].Boys);
                            ews.Cells["Q" + i].Value = MISReportDetails[0].Girls;
                            TotGirls = TotGirls + Convert.ToInt32(MISReportDetails[0].Girls);
                            ews.Cells["R" + i].Value = MISReportDetails[0].Total;
                            CampusTotal = CampusTotal + Convert.ToInt32(MISReportDetails[0].Total);
                            i = i + 1;
                        }
                        else
                        {
                            ews.Cells["P" + i].Value = "";
                            ews.Cells["Q" + i].Value = "";
                            ews.Cells["R" + i].Value = "";
                        }

                    }
                    ews.Cells["P21"].Value = TotBoys;
                    ews.Cells["Q21"].Value = TotGirls;
                    ews.Cells["R21"].Value = CampusTotal;
                    //Next Academic Year
                    ews.Cells["S3:U3"].Merge = true;
                    ews.Cells["S3:U3"].Value = NextAcademicYear;
                    ews.Cells["S4"].Value = "Boys";
                    ews.Cells["T4"].Value = "Girls";
                    ews.Cells["U4"].Value = "Total";

                    TotBoys = 0;
                    TotGirls = 0;
                    CampusTotal = 0;
                    i = 5;
                    j = 0;
                    Grade = string.Empty;
                    for (j = 0; j < 15; j++)
                    {
                        Grade = GetGradeName(j);
                        MISReportDetails = MISReportWithCriteria(CampusList[2].Name, Grade, NextAcademicYear);
                        if (MISReportDetails.Count > 0)
                        {
                            ews.Cells["S" + i].Value = MISReportDetails[0].Boys;
                            TotBoys = TotBoys + Convert.ToInt32(MISReportDetails[0].Boys);
                            ews.Cells["T" + i].Value = MISReportDetails[0].Girls;
                            TotGirls = TotGirls + Convert.ToInt32(MISReportDetails[0].Girls);
                            ews.Cells["U" + i].Value = MISReportDetails[0].Total;
                            CampusTotal = CampusTotal + Convert.ToInt32(MISReportDetails[0].Total);
                            i = i + 1;
                        }
                        else
                        {
                            ews.Cells["S" + i].Value = "";
                            ews.Cells["T" + i].Value = "";
                            ews.Cells["U" + i].Value = "";
                        }

                    }
                    ews.Cells["S21"].Value = TotBoys;
                    ews.Cells["T21"].Value = TotGirls;
                    ews.Cells["U21"].Value = CampusTotal;
                    #endregion
                    ews.Cells["V2:V21"].Merge = true;
                    #region For Campus 4
                    ews.Cells["W2:AB2"].Merge = true;
                    ews.Cells["W2:AB2"].Value = CampusList[3].ShowName;
                    //Current Academic Year
                    ews.Cells["W3:Y3"].Merge = true;
                    ews.Cells["W3:Y3"].Value = CurrentAcademicYear;
                    ews.Cells["W4"].Value = "Boys";
                    ews.Cells["X4"].Value = "Girls";
                    ews.Cells["Y4"].Value = "Total";
                    TotBoys = 0;
                    TotGirls = 0;
                    CampusTotal = 0;
                    i = 5;
                    j = 0;
                    Grade = string.Empty;
                    for (j = 0; j < 15; j++)
                    {
                        Grade = GetGradeName(j);
                        MISReportDetails = MISReportWithCriteria(CampusList[3].Name, Grade, CurrentAcademicYear);
                        if (MISReportDetails.Count > 0)
                        {
                            ews.Cells["W" + i].Value = MISReportDetails[0].Boys;
                            TotBoys = TotBoys + Convert.ToInt32(MISReportDetails[0].Boys);
                            ews.Cells["X" + i].Value = MISReportDetails[0].Girls;
                            TotGirls = TotGirls + Convert.ToInt32(MISReportDetails[0].Girls);
                            ews.Cells["Y" + i].Value = MISReportDetails[0].Total;
                            CampusTotal = CampusTotal + Convert.ToInt32(MISReportDetails[0].Total);
                            i = i + 1;
                        }
                        else
                        {
                            ews.Cells["W" + i].Value = "";
                            ews.Cells["X" + i].Value = "";
                            ews.Cells["Y" + i].Value = "";
                        }

                    }
                    ews.Cells["W21"].Value = TotBoys;
                    ews.Cells["X21"].Value = TotGirls;
                    ews.Cells["Y21"].Value = CampusTotal;
                    //Next Academic Year
                    ews.Cells["Z3:AB3"].Merge = true;
                    ews.Cells["Z3:AB3"].Value = NextAcademicYear;
                    ews.Cells["Z4"].Value = "Boys";
                    ews.Cells["AA4"].Value = "Girls";
                    ews.Cells["AB4"].Value = "Total";

                    TotBoys = 0;
                    TotGirls = 0;
                    CampusTotal = 0;
                    i = 5;
                    j = 0;
                    Grade = string.Empty;
                    for (j = 0; j < 15; j++)
                    {
                        Grade = GetGradeName(j);
                        MISReportDetails = MISReportWithCriteria(CampusList[3].Name, Grade, NextAcademicYear);
                        if (MISReportDetails.Count > 0)
                        {
                            ews.Cells["Z" + i].Value = MISReportDetails[0].Boys;
                            TotBoys = TotBoys + Convert.ToInt32(MISReportDetails[0].Boys);
                            ews.Cells["AA" + i].Value = MISReportDetails[0].Girls;
                            TotGirls = TotGirls + Convert.ToInt32(MISReportDetails[0].Girls);
                            ews.Cells["AB" + i].Value = MISReportDetails[0].Total;
                            CampusTotal = CampusTotal + Convert.ToInt32(MISReportDetails[0].Total);
                            i = i + 1;
                        }
                        else
                        {
                            ews.Cells["Z" + i].Value = "";
                            ews.Cells["AA" + i].Value = "";
                            ews.Cells["AB" + i].Value = "";
                        }
                    }
                    ews.Cells["Z21"].Value = TotBoys;
                    ews.Cells["AA21"].Value = TotGirls;
                    ews.Cells["AB21"].Value = CampusTotal;
                    #endregion
                    ews.Cells["AC2:AC21"].Merge = true;

                    #region For Campus 5
                    ews.Cells["AD2:AI2"].Merge = true;
                    ews.Cells["AD2:AI2"].Value = CampusList[4].ShowName;
                    //Current Academic Year
                    ews.Cells["AD3:AF3"].Merge = true;
                    ews.Cells["AD3:AF3"].Value = CurrentAcademicYear;
                    ews.Cells["AD4"].Value = "Boys";
                    ews.Cells["AE4"].Value = "Girls";
                    ews.Cells["AF4"].Value = "Total";
                    TotBoys = 0;
                    TotGirls = 0;
                    CampusTotal = 0;
                    i = 5;
                    j = 0;
                    Grade = string.Empty;
                    for (j = 0; j < 15; j++)
                    {
                        Grade = GetGradeName(j);
                        MISReportDetails = MISReportWithCriteria(CampusList[4].Name, Grade, CurrentAcademicYear);
                        if (MISReportDetails.Count > 0)
                        {
                            ews.Cells["AD" + i].Value = MISReportDetails[0].Boys;
                            TotBoys = TotBoys + Convert.ToInt32(MISReportDetails[0].Boys);
                            ews.Cells["AE" + i].Value = MISReportDetails[0].Girls;
                            TotGirls = TotGirls + Convert.ToInt32(MISReportDetails[0].Girls);
                            ews.Cells["AF" + i].Value = MISReportDetails[0].Total;
                            CampusTotal = CampusTotal + Convert.ToInt32(MISReportDetails[0].Total);
                            i = i + 1;
                        }
                        else
                        {
                            ews.Cells["AD" + i].Value = "";
                            ews.Cells["AE" + i].Value = "";
                            ews.Cells["AF" + i].Value = "";
                        }

                    }
                    ews.Cells["AD21"].Value = TotBoys;
                    ews.Cells["AE21"].Value = TotGirls;
                    ews.Cells["AF21"].Value = CampusTotal;
                    //Next Academic Year
                    ews.Cells["AG3:AI3"].Merge = true;
                    ews.Cells["AG3:AI3"].Value = NextAcademicYear;
                    ews.Cells["AG4"].Value = "Boys";
                    ews.Cells["AH4"].Value = "Girls";
                    ews.Cells["AI4"].Value = "Total";

                    TotBoys = 0;
                    TotGirls = 0;
                    CampusTotal = 0;
                    i = 5;
                    j = 0;
                    Grade = string.Empty;
                    for (j = 0; j < 15; j++)
                    {
                        Grade = GetGradeName(j);
                        MISReportDetails = MISReportWithCriteria(CampusList[4].Name, Grade, NextAcademicYear);
                        if (MISReportDetails.Count > 0)
                        {
                            ews.Cells["AG" + i].Value = MISReportDetails[0].Boys;
                            TotBoys = TotBoys + Convert.ToInt32(MISReportDetails[0].Boys);
                            ews.Cells["AH" + i].Value = MISReportDetails[0].Girls;
                            TotGirls = TotGirls + Convert.ToInt32(MISReportDetails[0].Girls);
                            ews.Cells["AI" + i].Value = MISReportDetails[0].Total;
                            CampusTotal = CampusTotal + Convert.ToInt32(MISReportDetails[0].Total);
                            i = i + 1;
                        }
                        else
                        {
                            ews.Cells["AG" + i].Value = "";
                            ews.Cells["AH" + i].Value = "";
                            ews.Cells["AI" + i].Value = "";
                        }
                    }
                    ews.Cells["AG21"].Value = TotBoys;
                    ews.Cells["AH21"].Value = TotGirls;
                    ews.Cells["AI21"].Value = CampusTotal;
                    #endregion
                    ews.Cells["AJ2:AJ21"].Merge = true;
                    #region For Campus 6
                    ews.Cells["AK2:AP2"].Merge = true;
                    ews.Cells["AK2:AP2"].Value = CampusList[5].ShowName;
                    //Current Academic Year
                    ews.Cells["AK3:AM3"].Merge = true;
                    ews.Cells["AK3:AM3"].Value = CurrentAcademicYear;
                    ews.Cells["AK4"].Value = "Boys";
                    ews.Cells["AL4"].Value = "Girls";
                    ews.Cells["AM4"].Value = "Total";
                    TotBoys = 0;
                    TotGirls = 0;
                    CampusTotal = 0;
                    i = 5;
                    j = 0;
                    Grade = string.Empty;
                    for (j = 0; j < 15; j++)
                    {
                        Grade = GetGradeName(j);
                        MISReportDetails = MISReportWithCriteria(CampusList[5].Name, Grade, CurrentAcademicYear);
                        if (MISReportDetails.Count > 0)
                        {
                            ews.Cells["AK" + i].Value = MISReportDetails[0].Boys;
                            TotBoys = TotBoys + Convert.ToInt32(MISReportDetails[0].Boys);
                            ews.Cells["AL" + i].Value = MISReportDetails[0].Girls;
                            TotGirls = TotGirls + Convert.ToInt32(MISReportDetails[0].Girls);
                            ews.Cells["AM" + i].Value = MISReportDetails[0].Total;
                            CampusTotal = CampusTotal + Convert.ToInt32(MISReportDetails[0].Total);
                            i = i + 1;
                        }
                        else
                        {
                            ews.Cells["AK" + i].Value = "";
                            ews.Cells["AL" + i].Value = "";
                            ews.Cells["AM" + i].Value = "";
                        }

                    }
                    ews.Cells["AK21"].Value = TotBoys;
                    ews.Cells["AL21"].Value = TotGirls;
                    ews.Cells["AM21"].Value = CampusTotal;
                    //Next Academic Year
                    ews.Cells["AN3:AP3"].Merge = true;
                    ews.Cells["AN3:AP3"].Value = NextAcademicYear;
                    ews.Cells["AN4"].Value = "Boys";
                    ews.Cells["AO4"].Value = "Girls";
                    ews.Cells["AP4"].Value = "Total";

                    TotBoys = 0;
                    TotGirls = 0;
                    CampusTotal = 0;
                    i = 5;
                    j = 0;
                    Grade = string.Empty;
                    for (j = 0; j < 15; j++)
                    {
                        Grade = GetGradeName(j);
                        MISReportDetails = MISReportWithCriteria(CampusList[5].Name, Grade, NextAcademicYear);
                        if (MISReportDetails.Count > 0)
                        {
                            ews.Cells["AN" + i].Value = MISReportDetails[0].Boys;
                            TotBoys = TotBoys + Convert.ToInt32(MISReportDetails[0].Boys);
                            ews.Cells["AO" + i].Value = MISReportDetails[0].Girls;
                            TotGirls = TotGirls + Convert.ToInt32(MISReportDetails[0].Girls);
                            ews.Cells["AP" + i].Value = MISReportDetails[0].Total;
                            CampusTotal = CampusTotal + Convert.ToInt32(MISReportDetails[0].Total);
                            i = i + 1;
                        }
                        else
                        {
                            ews.Cells["AN" + i].Value = "";
                            ews.Cells["AO" + i].Value = "";
                            ews.Cells["AP" + i].Value = "";
                        }
                    }
                    ews.Cells["AN21"].Value = TotBoys;
                    ews.Cells["AO21"].Value = TotGirls;
                    ews.Cells["AP21"].Value = CampusTotal;
                    #endregion
                    ews.Cells["AQ2:AQ21"].Merge = true;
                    #region For Campus 7
                    ews.Cells["AR2:AW2"].Merge = true;
                    ews.Cells["AR2:AW2"].Value = CampusList[6].ShowName;
                    //Current Academic Year
                    ews.Cells["AR3:AT3"].Merge = true;
                    ews.Cells["AR3:AT3"].Value = CurrentAcademicYear;
                    ews.Cells["AR4"].Value = "Boys";
                    ews.Cells["AS4"].Value = "Girls";
                    ews.Cells["AT4"].Value = "Total";
                    TotBoys = 0;
                    TotGirls = 0;
                    CampusTotal = 0;
                    i = 5;
                    j = 0;
                    Grade = string.Empty;
                    for (j = 0; j < 15; j++)
                    {
                        Grade = GetGradeName(j);
                        MISReportDetails = MISReportWithCriteria(CampusList[6].Name, Grade, CurrentAcademicYear);
                        if (MISReportDetails.Count > 0)
                        {
                            ews.Cells["AR" + i].Value = MISReportDetails[0].Boys;
                            TotBoys = TotBoys + Convert.ToInt32(MISReportDetails[0].Boys);
                            ews.Cells["AS" + i].Value = MISReportDetails[0].Girls;
                            TotGirls = TotGirls + Convert.ToInt32(MISReportDetails[0].Girls);
                            ews.Cells["AT" + i].Value = MISReportDetails[0].Total;
                            CampusTotal = CampusTotal + Convert.ToInt32(MISReportDetails[0].Total);
                            i = i + 1;
                        }
                        else
                        {
                            ews.Cells["AR" + i].Value = "";
                            ews.Cells["AS" + i].Value = "";
                            ews.Cells["AT" + i].Value = "";
                        }

                    }
                    ews.Cells["AR21"].Value = TotBoys;
                    ews.Cells["AS21"].Value = TotGirls;
                    ews.Cells["AT21"].Value = CampusTotal;
                    //Next Academic Year
                    ews.Cells["AU3:AW3"].Merge = true;
                    ews.Cells["AU3:AW3"].Value = NextAcademicYear;
                    ews.Cells["AU4"].Value = "Boys";
                    ews.Cells["AV4"].Value = "Girls";
                    ews.Cells["AW4"].Value = "Total";

                    TotBoys = 0;
                    TotGirls = 0;
                    CampusTotal = 0;
                    i = 5;
                    j = 0;
                    Grade = string.Empty;
                    for (j = 0; j < 15; j++)
                    {
                        Grade = GetGradeName(j);
                        MISReportDetails = MISReportWithCriteria(CampusList[6].Name, Grade, NextAcademicYear);
                        if (MISReportDetails.Count > 0)
                        {
                            ews.Cells["AU" + i].Value = MISReportDetails[0].Boys;
                            TotBoys = TotBoys + Convert.ToInt32(MISReportDetails[0].Boys);
                            ews.Cells["AV" + i].Value = MISReportDetails[0].Girls;
                            TotGirls = TotGirls + Convert.ToInt32(MISReportDetails[0].Girls);
                            ews.Cells["AW" + i].Value = MISReportDetails[0].Total;
                            CampusTotal = CampusTotal + Convert.ToInt32(MISReportDetails[0].Total);
                            i = i + 1;
                        }
                        else
                        {
                            ews.Cells["AU" + i].Value = "";
                            ews.Cells["AV" + i].Value = "";
                            ews.Cells["AW" + i].Value = "";
                        }
                    }
                    ews.Cells["AU21"].Value = TotBoys;
                    ews.Cells["AV21"].Value = TotGirls;
                    ews.Cells["AW21"].Value = CampusTotal;
                    #endregion
                    ews.Cells["AX2:AX21"].Merge = true;
                    #region For Campus 8
                    ews.Cells["AY2:BD2"].Merge = true;
                    ews.Cells["AY2:BD2"].Value = CampusList[7].ShowName;
                    //Current Academic Year
                    ews.Cells["AY3:BA3"].Merge = true;
                    ews.Cells["AY3:BA3"].Value = CurrentAcademicYear;
                    ews.Cells["AY4"].Value = "Boys";
                    ews.Cells["AZ4"].Value = "Girls";
                    ews.Cells["BA4"].Value = "Total";
                    TotBoys = 0;
                    TotGirls = 0;
                    CampusTotal = 0;
                    i = 5;
                    j = 0;
                    Grade = string.Empty;
                    for (j = 0; j < 15; j++)
                    {
                        Grade = GetGradeName(j);
                        MISReportDetails = MISReportWithCriteria(CampusList[7].Name, Grade, CurrentAcademicYear);
                        if (MISReportDetails.Count > 0)
                        {
                            ews.Cells["AY" + i].Value = MISReportDetails[0].Boys;
                            TotBoys = TotBoys + Convert.ToInt32(MISReportDetails[0].Boys);
                            ews.Cells["AZ" + i].Value = MISReportDetails[0].Girls;
                            TotGirls = TotGirls + Convert.ToInt32(MISReportDetails[0].Girls);
                            ews.Cells["BA" + i].Value = MISReportDetails[0].Total;
                            CampusTotal = CampusTotal + Convert.ToInt32(MISReportDetails[0].Total);
                            i = i + 1;
                        }
                        else
                        {
                            ews.Cells["AY" + i].Value = "";
                            ews.Cells["AZ" + i].Value = "";
                            ews.Cells["BA" + i].Value = "";
                        }

                    }
                    ews.Cells["AY21"].Value = TotBoys;
                    ews.Cells["AZ21"].Value = TotGirls;
                    ews.Cells["BA21"].Value = CampusTotal;
                    //Next Academic Year
                    ews.Cells["BB3:BD3"].Merge = true;
                    ews.Cells["BB3:BD3"].Value = NextAcademicYear;
                    ews.Cells["BB4"].Value = "Boys";
                    ews.Cells["BC4"].Value = "Girls";
                    ews.Cells["BD4"].Value = "Total";

                    TotBoys = 0;
                    TotGirls = 0;
                    CampusTotal = 0;
                    i = 5;
                    j = 0;
                    Grade = string.Empty;
                    for (j = 0; j < 15; j++)
                    {
                        Grade = GetGradeName(j);
                        MISReportDetails = MISReportWithCriteria(CampusList[7].Name, Grade, NextAcademicYear);
                        if (MISReportDetails.Count > 0)
                        {
                            ews.Cells["BB" + i].Value = MISReportDetails[0].Boys;
                            TotBoys = TotBoys + Convert.ToInt32(MISReportDetails[0].Boys);
                            ews.Cells["BC" + i].Value = MISReportDetails[0].Girls;
                            TotGirls = TotGirls + Convert.ToInt32(MISReportDetails[0].Girls);
                            ews.Cells["BD" + i].Value = MISReportDetails[0].Total;
                            CampusTotal = CampusTotal + Convert.ToInt32(MISReportDetails[0].Total);
                            i = i + 1;
                        }
                        else
                        {
                            ews.Cells["BB" + i].Value = "";
                            ews.Cells["BC" + i].Value = "";
                            ews.Cells["BD" + i].Value = "";
                        }
                    }
                    ews.Cells["BB21"].Value = TotBoys;
                    ews.Cells["BC21"].Value = TotGirls;
                    ews.Cells["BD21"].Value = CampusTotal;
                    #endregion
                    ews.Cells["BE2:BE21"].Merge = true;
                    #region For Campus 9
                    ews.Cells["BF2:BK2"].Merge = true;
                    ews.Cells["BF2:BK2"].Value = CampusList[8].ShowName;
                    //Current Academic Year
                    ews.Cells["BF3:BH3"].Merge = true;
                    ews.Cells["BF3:BH3"].Value = CurrentAcademicYear;
                    ews.Cells["BF4"].Value = "Boys";
                    ews.Cells["BG4"].Value = "Girls";
                    ews.Cells["BH4"].Value = "Total";
                    TotBoys = 0;
                    TotGirls = 0;
                    CampusTotal = 0;
                    i = 5;
                    j = 0;
                    Grade = string.Empty;
                    for (j = 0; j < 15; j++)
                    {
                        Grade = GetGradeName(j);
                        MISReportDetails = MISReportWithCriteria(CampusList[8].Name, Grade, CurrentAcademicYear);
                        if (MISReportDetails.Count > 0)
                        {
                            ews.Cells["BF" + i].Value = MISReportDetails[0].Boys;
                            TotBoys = TotBoys + Convert.ToInt32(MISReportDetails[0].Boys);
                            ews.Cells["BG" + i].Value = MISReportDetails[0].Girls;
                            TotGirls = TotGirls + Convert.ToInt32(MISReportDetails[0].Girls);
                            ews.Cells["BH" + i].Value = MISReportDetails[0].Total;
                            CampusTotal = CampusTotal + Convert.ToInt32(MISReportDetails[0].Total);
                            i = i + 1;
                        }
                        else
                        {
                            ews.Cells["BF" + i].Value = "";
                            ews.Cells["BG" + i].Value = "";
                            ews.Cells["BH" + i].Value = "";
                        }

                    }
                    ews.Cells["BF21"].Value = TotBoys;
                    ews.Cells["BG21"].Value = TotGirls;
                    ews.Cells["BH21"].Value = CampusTotal;
                    //Next Academic Year
                    ews.Cells["BI3:BK3"].Merge = true;
                    ews.Cells["BI3:BK3"].Value = NextAcademicYear;
                    ews.Cells["BI4"].Value = "Boys";
                    ews.Cells["BJ4"].Value = "Girls";
                    ews.Cells["BK4"].Value = "Total";

                    TotBoys = 0;
                    TotGirls = 0;
                    CampusTotal = 0;
                    i = 5;
                    j = 0;
                    Grade = string.Empty;
                    for (j = 0; j < 15; j++)
                    {
                        Grade = GetGradeName(j);
                        MISReportDetails = MISReportWithCriteria(CampusList[8].Name, Grade, NextAcademicYear);
                        if (MISReportDetails.Count > 0)
                        {
                            ews.Cells["BI" + i].Value = MISReportDetails[0].Boys;
                            TotBoys = TotBoys + Convert.ToInt32(MISReportDetails[0].Boys);
                            ews.Cells["BJ" + i].Value = MISReportDetails[0].Girls;
                            TotGirls = TotGirls + Convert.ToInt32(MISReportDetails[0].Girls);
                            ews.Cells["BK" + i].Value = MISReportDetails[0].Total;
                            CampusTotal = CampusTotal + Convert.ToInt32(MISReportDetails[0].Total);
                            i = i + 1;
                        }
                        else
                        {
                            ews.Cells["BI" + i].Value = "";
                            ews.Cells["BJ" + i].Value = "";
                            ews.Cells["BK" + i].Value = "";
                        }
                    }
                    ews.Cells["BI21"].Value = TotBoys;
                    ews.Cells["BJ21"].Value = TotGirls;
                    ews.Cells["BK21"].Value = CampusTotal;
                    #endregion
                    ews.Cells["BL2:BL21"].Merge = true;
                    #region For Campus 10
                    ews.Cells["BM2:BR2"].Merge = true;
                    ews.Cells["BM2:BR2"].Value = CampusList[9].ShowName;
                    //Current Academic Year
                    ews.Cells["BM3:BO3"].Merge = true;
                    ews.Cells["BM3:BO3"].Value = CurrentAcademicYear;
                    ews.Cells["BM4"].Value = "Boys";
                    ews.Cells["BN4"].Value = "Girls";
                    ews.Cells["BO4"].Value = "Total";
                    TotBoys = 0;
                    TotGirls = 0;
                    CampusTotal = 0;
                    i = 5;
                    j = 0;
                    Grade = string.Empty;
                    for (j = 0; j < 15; j++)
                    {
                        Grade = GetGradeName(j);
                        MISReportDetails = MISReportWithCriteria(CampusList[9].Name, Grade, CurrentAcademicYear);
                        if (MISReportDetails.Count > 0)
                        {
                            ews.Cells["BM" + i].Value = MISReportDetails[0].Boys;
                            TotBoys = TotBoys + Convert.ToInt32(MISReportDetails[0].Boys);
                            ews.Cells["BN" + i].Value = MISReportDetails[0].Girls;
                            TotGirls = TotGirls + Convert.ToInt32(MISReportDetails[0].Girls);
                            ews.Cells["BO" + i].Value = MISReportDetails[0].Total;
                            CampusTotal = CampusTotal + Convert.ToInt32(MISReportDetails[0].Total);
                            i = i + 1;
                        }
                        else
                        {
                            ews.Cells["BM" + i].Value = "";
                            ews.Cells["BN" + i].Value = "";
                            ews.Cells["BO" + i].Value = "";
                        }

                    }
                    ews.Cells["BM21"].Value = TotBoys;
                    ews.Cells["BN21"].Value = TotGirls;
                    ews.Cells["BO21"].Value = CampusTotal;
                    //Next Academic Year
                    ews.Cells["BP3:BR3"].Merge = true;
                    ews.Cells["BP3:BR3"].Value = NextAcademicYear;
                    ews.Cells["BP4"].Value = "Boys";
                    ews.Cells["BQ4"].Value = "Girls";
                    ews.Cells["BR4"].Value = "Total";

                    TotBoys = 0;
                    TotGirls = 0;
                    CampusTotal = 0;
                    i = 5;
                    j = 0;
                    Grade = string.Empty;
                    for (j = 0; j < 15; j++)
                    {
                        Grade = GetGradeName(j);
                        MISReportDetails = MISReportWithCriteria(CampusList[9].Name, Grade, NextAcademicYear);
                        if (MISReportDetails.Count > 0)
                        {
                            ews.Cells["BP" + i].Value = MISReportDetails[0].Boys;
                            TotBoys = TotBoys + Convert.ToInt32(MISReportDetails[0].Boys);
                            ews.Cells["BQ" + i].Value = MISReportDetails[0].Girls;
                            TotGirls = TotGirls + Convert.ToInt32(MISReportDetails[0].Girls);
                            ews.Cells["BR" + i].Value = MISReportDetails[0].Total;
                            CampusTotal = CampusTotal + Convert.ToInt32(MISReportDetails[0].Total);
                            i = i + 1;
                        }
                        else
                        {
                            ews.Cells["BP" + i].Value = "";
                            ews.Cells["BQ" + i].Value = "";
                            ews.Cells["BR" + i].Value = "";
                        }
                    }
                    ews.Cells["BP21"].Value = TotBoys;
                    ews.Cells["BQ21"].Value = TotGirls;
                    ews.Cells["BR21"].Value = CampusTotal;
                    #endregion
                    ews.Cells["BS2:BS21"].Merge = true;
                    #region For Campus 11
                    ews.Cells["BT2:BY2"].Merge = true;
                    ews.Cells["BT2:BY2"].Value = CampusList[10].ShowName;
                    //Current Academic Year
                    ews.Cells["BT3:BV3"].Merge = true;
                    ews.Cells["BT3:BV3"].Value = CurrentAcademicYear;
                    ews.Cells["BT4"].Value = "Boys";
                    ews.Cells["BU4"].Value = "Girls";
                    ews.Cells["BV4"].Value = "Total";
                    TotBoys = 0;
                    TotGirls = 0;
                    CampusTotal = 0;
                    i = 5;
                    j = 0;
                    Grade = string.Empty;
                    for (j = 0; j < 15; j++)
                    {
                        Grade = GetGradeName(j);
                        MISReportDetails = MISReportWithCriteria(CampusList[10].Name, Grade, CurrentAcademicYear);
                        if (MISReportDetails.Count > 0)
                        {
                            ews.Cells["BT" + i].Value = MISReportDetails[0].Boys;
                            TotBoys = TotBoys + Convert.ToInt32(MISReportDetails[0].Boys);
                            ews.Cells["BU" + i].Value = MISReportDetails[0].Girls;
                            TotGirls = TotGirls + Convert.ToInt32(MISReportDetails[0].Girls);
                            ews.Cells["BV" + i].Value = MISReportDetails[0].Total;
                            CampusTotal = CampusTotal + Convert.ToInt32(MISReportDetails[0].Total);
                            i = i + 1;
                        }
                        else
                        {
                            ews.Cells["BT" + i].Value = "";
                            ews.Cells["BU" + i].Value = "";
                            ews.Cells["BV" + i].Value = "";
                        }

                    }
                    ews.Cells["BT21"].Value = TotBoys;
                    ews.Cells["BU21"].Value = TotGirls;
                    ews.Cells["BV21"].Value = CampusTotal;
                    //Next Academic Year
                    ews.Cells["BW3:BY3"].Merge = true;
                    ews.Cells["BW3:BY3"].Value = NextAcademicYear;
                    ews.Cells["BW4"].Value = "Boys";
                    ews.Cells["BX4"].Value = "Girls";
                    ews.Cells["BY4"].Value = "Total";

                    TotBoys = 0;
                    TotGirls = 0;
                    CampusTotal = 0;
                    i = 5;
                    j = 0;
                    Grade = string.Empty;
                    for (j = 0; j < 15; j++)
                    {
                        Grade = GetGradeName(j);
                        MISReportDetails = MISReportWithCriteria(CampusList[10].Name, Grade, NextAcademicYear);
                        if (MISReportDetails.Count > 0)
                        {
                            ews.Cells["BW" + i].Value = MISReportDetails[0].Boys;
                            TotBoys = TotBoys + Convert.ToInt32(MISReportDetails[0].Boys);
                            ews.Cells["BX" + i].Value = MISReportDetails[0].Girls;
                            TotGirls = TotGirls + Convert.ToInt32(MISReportDetails[0].Girls);
                            ews.Cells["BY" + i].Value = MISReportDetails[0].Total;
                            CampusTotal = CampusTotal + Convert.ToInt32(MISReportDetails[0].Total);
                            i = i + 1;
                        }
                        else
                        {
                            ews.Cells["BW" + i].Value = "";
                            ews.Cells["BX" + i].Value = "";
                            ews.Cells["BY" + i].Value = "";
                        }
                    }
                    ews.Cells["BW21"].Value = TotBoys;
                    ews.Cells["BX21"].Value = TotGirls;
                    ews.Cells["BY21"].Value = CampusTotal;
                    #endregion
                    ews.Cells["BZ2:BZ21"].Merge = true;
                    #region For Campus 12
                    ews.Cells["CA2:CF2"].Merge = true;
                    ews.Cells["CA2:CF2"].Value = CampusList[11].ShowName;
                    //Current Academic Year
                    ews.Cells["CA3:CC3"].Merge = true;
                    ews.Cells["CA3:CC3"].Value = CurrentAcademicYear;
                    ews.Cells["CA4"].Value = "Boys";
                    ews.Cells["CB4"].Value = "Girls";
                    ews.Cells["CC4"].Value = "Total";
                    TotBoys = 0;
                    TotGirls = 0;
                    CampusTotal = 0;
                    i = 5;
                    j = 0;
                    Grade = string.Empty;
                    for (j = 0; j < 15; j++)
                    {
                        Grade = GetGradeName(j);
                        MISReportDetails = MISReportWithCriteria(CampusList[11].Name, Grade, CurrentAcademicYear);
                        if (MISReportDetails.Count > 0)
                        {
                            ews.Cells["CA" + i].Value = MISReportDetails[0].Boys;
                            TotBoys = TotBoys + Convert.ToInt32(MISReportDetails[0].Boys);
                            ews.Cells["CB" + i].Value = MISReportDetails[0].Girls;
                            TotGirls = TotGirls + Convert.ToInt32(MISReportDetails[0].Girls);
                            ews.Cells["CC" + i].Value = MISReportDetails[0].Total;
                            CampusTotal = CampusTotal + Convert.ToInt32(MISReportDetails[0].Total);
                            i = i + 1;
                        }
                        else
                        {
                            ews.Cells["CA" + i].Value = "";
                            ews.Cells["CB" + i].Value = "";
                            ews.Cells["CC" + i].Value = "";
                        }

                    }
                    ews.Cells["CA21"].Value = TotBoys;
                    ews.Cells["CB21"].Value = TotGirls;
                    ews.Cells["CC21"].Value = CampusTotal;
                    //Next Academic Year
                    ews.Cells["CD3:CF3"].Merge = true;
                    ews.Cells["CD3:CF3"].Value = NextAcademicYear;
                    ews.Cells["CD4"].Value = "Boys";
                    ews.Cells["CE4"].Value = "Girls";
                    ews.Cells["CF4"].Value = "Total";

                    TotBoys = 0;
                    TotGirls = 0;
                    CampusTotal = 0;
                    i = 5;
                    j = 0;
                    Grade = string.Empty;
                    for (j = 0; j < 15; j++)
                    {
                        Grade = GetGradeName(j);
                        MISReportDetails = MISReportWithCriteria(CampusList[11].Name, Grade, NextAcademicYear);
                        if (MISReportDetails.Count > 0)
                        {
                            ews.Cells["CD" + i].Value = MISReportDetails[0].Boys;
                            TotBoys = TotBoys + Convert.ToInt32(MISReportDetails[0].Boys);
                            ews.Cells["CE" + i].Value = MISReportDetails[0].Girls;
                            TotGirls = TotGirls + Convert.ToInt32(MISReportDetails[0].Girls);
                            ews.Cells["CF" + i].Value = MISReportDetails[0].Total;
                            CampusTotal = CampusTotal + Convert.ToInt32(MISReportDetails[0].Total);
                            i = i + 1;
                        }
                        else
                        {
                            ews.Cells["CD" + i].Value = "";
                            ews.Cells["CE" + i].Value = "";
                            ews.Cells["CF" + i].Value = "";
                        }
                    }
                    ews.Cells["CD21"].Value = TotBoys;
                    ews.Cells["CE21"].Value = TotGirls;
                    ews.Cells["CF21"].Value = CampusTotal;
                    #endregion
                    ews.Cells["CG2:CG21"].Merge = true;
                    #region For Campus 13
                    ews.Cells["CH2:CM2"].Merge = true;
                    ews.Cells["CH2:CM2"].Value = CampusList[12].ShowName;
                    //Current Academic Year
                    ews.Cells["CH3:CJ3"].Merge = true;
                    ews.Cells["CH3:CJ3"].Value = CurrentAcademicYear;
                    ews.Cells["CH4"].Value = "Boys";
                    ews.Cells["CI4"].Value = "Girls";
                    ews.Cells["CJ4"].Value = "Total";
                    TotBoys = 0;
                    TotGirls = 0;
                    CampusTotal = 0;
                    i = 5;
                    j = 0;
                    Grade = string.Empty;
                    for (j = 0; j < 15; j++)
                    {
                        Grade = GetGradeName(j);
                        MISReportDetails = MISReportWithCriteria(CampusList[12].Name, Grade, CurrentAcademicYear);
                        if (MISReportDetails.Count > 0)
                        {
                            ews.Cells["CH" + i].Value = MISReportDetails[0].Boys;
                            TotBoys = TotBoys + Convert.ToInt32(MISReportDetails[0].Boys);
                            ews.Cells["CI" + i].Value = MISReportDetails[0].Girls;
                            TotGirls = TotGirls + Convert.ToInt32(MISReportDetails[0].Girls);
                            ews.Cells["CJ" + i].Value = MISReportDetails[0].Total;
                            CampusTotal = CampusTotal + Convert.ToInt32(MISReportDetails[0].Total);
                            i = i + 1;
                        }
                        else
                        {
                            ews.Cells["CH" + i].Value = "";
                            ews.Cells["CI" + i].Value = "";
                            ews.Cells["CJ" + i].Value = "";
                        }

                    }
                    ews.Cells["CH21"].Value = TotBoys;
                    ews.Cells["CI21"].Value = TotGirls;
                    ews.Cells["CJ21"].Value = CampusTotal;
                    //Next Academic Year
                    ews.Cells["CK3:CM3"].Merge = true;
                    ews.Cells["CK3:CM3"].Value = NextAcademicYear;
                    ews.Cells["CK4"].Value = "Boys";
                    ews.Cells["CL4"].Value = "Girls";
                    ews.Cells["CM4"].Value = "Total";

                    TotBoys = 0;
                    TotGirls = 0;
                    CampusTotal = 0;
                    i = 5;
                    j = 0;
                    Grade = string.Empty;
                    for (j = 0; j < 15; j++)
                    {
                        Grade = GetGradeName(j);
                        MISReportDetails = MISReportWithCriteria(CampusList[12].Name, Grade, NextAcademicYear);
                        if (MISReportDetails.Count > 0)
                        {
                            ews.Cells["CK" + i].Value = MISReportDetails[0].Boys;
                            TotBoys = TotBoys + Convert.ToInt32(MISReportDetails[0].Boys);
                            ews.Cells["CL" + i].Value = MISReportDetails[0].Girls;
                            TotGirls = TotGirls + Convert.ToInt32(MISReportDetails[0].Girls);
                            ews.Cells["CM" + i].Value = MISReportDetails[0].Total;
                            CampusTotal = CampusTotal + Convert.ToInt32(MISReportDetails[0].Total);
                            i = i + 1;
                        }
                        else
                        {
                            ews.Cells["CK" + i].Value = "";
                            ews.Cells["CL" + i].Value = "";
                            ews.Cells["CM" + i].Value = "";
                        }
                    }
                    ews.Cells["CK21"].Value = TotBoys;
                    ews.Cells["CL21"].Value = TotGirls;
                    ews.Cells["CM21"].Value = CampusTotal;
                    #endregion
                    ews.Cells["CN2:CN21"].Merge = true;

                    ews.View.ShowGridLines = true;

                    string Todaydate = DateTime.Now.ToString("dd/MM/yyyy");
                    string FileName = "MISReportFor-AllCampus-On-" + Todaydate; ;
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;  filename=" + FileName + ".xlsx");
                    byte[] File = Epkg.GetAsByteArray();
                    HttpContext.Current.Response.BinaryWrite(File);
                    return File;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
            }
            return null;
        }
        public IList<MISReport_vw> MISReportWithCriteria(string Campus, string Grade, string AcademicYear)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Clear();
                //HRManagementService HRMS = new HRManagementService();
                //StudentsReportService SRS = new StudentsReportService();
                List<MISReport_vw> MISReportList = new List<MISReport_vw>();
                if (!string.IsNullOrEmpty(Campus)) criteria.Add("Campus", Campus);
                if (!string.IsNullOrEmpty(Grade)) criteria.Add("Grade", Grade);
                if (!string.IsNullOrEmpty(AcademicYear)) criteria.Add("AcademicYear", AcademicYear);
                Dictionary<long, IList<MISReport_vw>> MISReport_vwDetails = GetMISReport_vwListWithPaging(null, 9999, string.Empty, string.Empty, criteria);
                MISReportList = MISReport_vwDetails.FirstOrDefault().Value.ToList();
                return MISReportList;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public bool email_send(byte[] File, string FileName, string campus, string Email, string MailBody, string EmailType)
        {
            try
            {
                string SendEmail = ConfigurationManager.AppSettings["SendEmail1"];
                string From = ConfigurationManager.AppSettings["From"];

                if (SendEmail == "false")
                { return false; }
                else
                {
                    System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Clear();
                    criteria.Add("Campus", campus);
                    criteria.Add("Server", From);
                    Dictionary<long, IList<CampusEmailId>> CampusMailIdList = GetCampusEmailId(0, 9999, string.Empty, string.Empty, criteria);
                    if (CampusMailIdList.First().Value.Count > 0 && CampusMailIdList.Count > 0 && CampusMailIdList != null)
                    {
                        mail.From = new MailAddress(CampusMailIdList.FirstOrDefault().Value[0].EmailId);
                        if (!String.IsNullOrEmpty(Email))
                        {
                            mail.To.Add(Email);
                            string mailid = CampusMailIdList.FirstOrDefault().Value[0].EmailId;
                            string password = CampusMailIdList.FirstOrDefault().Value[0].Password;
                            string CampusPhoneNumber = CampusMailIdList.FirstOrDefault().Value[0].PhoneNumber;
                            string CampusFBLink = CampusMailIdList.FirstOrDefault().Value[0].FBLink;
                            if (!string.IsNullOrEmpty(campus)) { campus = campus.Replace(" ", ""); }
                            string Todaydate = DateTime.Now.ToString("dd/MM/yyyy");
                            string Name = "";
                            if (EmailType == "MISReport")
                            {
                                Name = "MISReportFor-" + campus + "-On-" + Todaydate + ".xlsx";
                            }
                            if (EmailType == "MISConsolidateReport")
                            {
                                Name = "Consolidate Report-On-" + Todaydate + ".xlsx";
                            }
                            if (EmailType == "AdmissionReport")
                            {
                                Name = "Admission Report-On-" + Todaydate + ".xlsx";
                            }
                            if (EmailType == "IssueMgmtWeeklyReport")
                            {
                                Name = "Issue Management Report-On-" + Todaydate + ".xlsx";
                                mail.To.Add("m.anbarasan@xcdsys.com");
                                mail.To.Add("tipsics@tipsglobal.org");
                            }
                            SmtpClient SmtpServer = new SmtpClient("localhost", 587);
                            SmtpServer.Host = "smtp.gmail.com";
                            Attachment att = new Attachment(new MemoryStream(File), Name);
                            mail.Subject = FileName;
                            string BodyMsg = "";
                            if (EmailType == "MISReport")
                            {
                                BodyMsg = "      Please find the attachment of MIS Report for <b><i>\"" + campus + "\" </i></b>Campus.";
                            }
                            if (EmailType == "MISConsolidateReport")
                            {
                                BodyMsg = "      Please find the attachment of Consolidate Report for <b><i>\"" + campus + "\" </i></b>Campus.";
                            }
                            if (EmailType == "AdmissionReport")
                            {
                                BodyMsg = "      Please find the attachment of Admission Report for <b><i>\"" + campus + "\" </i></b>Campus.";
                            }
                            if (EmailType == "IssueMgmtWeeklyReport")
                            {
                                BodyMsg = "      Please find the attachment of Issue Management Report for <b><i>\"" + campus + "\" </i></b>Campus.";
                            }
                            MailBody = MailBody.Replace("{{DateTime}}", DateTime.Now.ToString("dd/MM/yyyy"));
                            MailBody = MailBody.Replace("{{Content}}", BodyMsg);
                            MailBody = MailBody.Replace("{{PhoneNumber}}", CampusPhoneNumber);
                            MailBody = MailBody.Replace("{{EmailId}}", mailid);
                            MailBody = MailBody.Replace("{{FBLink}}", CampusFBLink != null ? CampusFBLink : "#");

                            mail.Body = MailBody;
                            mail.IsBodyHtml = true;
                            mail.Attachments.Add(att);
                            SmtpServer.Credentials = new System.Net.NetworkCredential(mailid, password);
                            SmtpServer.EnableSsl = true;
                            bool result = SendReportMail(mail, SmtpServer, CampusMailIdList);
                            return result;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
            }
            return false;
        }

        private bool SendReportMail(System.Net.Mail.MailMessage mail, SmtpClient SmtpServer, Dictionary<long, IList<CampusEmailId>> CampusMailIdList)
        {
            bool retValue = false;
            try
            {
                SmtpServer.Send(mail);
                retValue = true;
            }
            catch (Exception)
            {
                try
                {
                    mail.From = new MailAddress(CampusMailIdList.First().Value[0].AlternateEmailId.ToString());
                    SmtpServer.Credentials = new System.Net.NetworkCredential(CampusMailIdList.First().Value[0].AlternateEmailId.ToString(), CampusMailIdList.First().Value[0].AlternateEmailIdPassword.ToString());
                    SmtpServer.Send(mail);
                    mail.Bcc.Clear();
                    retValue = true;
                }
                catch (Exception)
                {
                    retValue = false;
                }
            }
            return retValue;
        }

        public IList<MISMailMaster> MISMailMasterList()
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Clear();
                //StudentsReportService SRS = new StudentsReportService();
                List<MISMailMaster> MailMaster = new List<MISMailMaster>();
                Dictionary<long, IList<MISMailMaster>> MISReport_vwDetails = GetMISMailMasterListWithPaging(null, 9999, string.Empty, string.Empty, criteria);
                MailMaster = MISReport_vwDetails.FirstOrDefault().Value.ToList();
                return MailMaster;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public MISMailStatus MailSendStatus(long EmailRefId, string MailId, string SentCategory, string EmailType)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Clear();
                //StudentsReportService SRS = new StudentsReportService();
                string DayName = DateTime.Now.ToString("dddd");
                string TodayDate = DateTime.Now.ToShortDateString();
                criteria.Add("CheckDate", TodayDate);
                criteria.Add("EmailId", MailId);
                criteria.Add("EmailRefId", EmailRefId);
                criteria.Add("SentCategory", SentCategory);
                criteria.Add("EmailType", EmailType);
                Dictionary<long, IList<MISMailStatus>> MailStatus = GetMISMailStatusListWithPaging(null, 9999, string.Empty, string.Empty, criteria);
                if (MailStatus != null && MailStatus.FirstOrDefault().Value != null && MailStatus.FirstOrDefault().Value.Count > 0)
                {
                    MISMailStatus MISMailStatus = MailStatus.FirstOrDefault().Value[0];
                    return MISMailStatus;
                }
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
            }
            return null;
        }
        public MISMailStatus MISMailStatusFirstInsert(long Id, string EmailId, string Campus, string SentCategory, string EmailType)
        {
            try
            {
                MISMailStatus MailStatus = new MISMailStatus();
                string TodayDate = DateTime.Now.ToShortDateString();
                MailStatus.CheckDate = TodayDate;
                MailStatus.IsSent = false;
                MailStatus.Campus = Campus;
                MailStatus.EmailId = EmailId;
                MailStatus.EmailRefId = Id;
                MailStatus.SentCategory = SentCategory;
                MailStatus.EmailType = EmailType;
                MailStatus.SentDate = DateTime.Now;
                SaveOrUpdateMISMailStatusDetails(MailStatus);
                return MailStatus;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
            }
            return null;
        }
        public string GetBodyofMail()
        {
            try
            {
                string MessageBody = System.IO.File.ReadAllText(HttpContext.Current.Request.MapPath("~/Views/Shared/EmailBody.html"));
                return MessageBody;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
            }
            return null;
        }
        public string GetGradeName(int Flag)
        {
            try
            {
                string Grade = "";
                if (Flag == 0)
                {
                    Grade = "Playschool";
                }
                if (Flag == 1)
                {
                    Grade = "PreKg";
                }
                if (Flag == 2)
                {
                    Grade = "LKG";
                }
                if (Flag == 3)
                {
                    Grade = "UKG";
                }
                if (Flag == 4)
                {
                    Grade = "I";
                }
                if (Flag == 5)
                {
                    Grade = "II";
                }
                if (Flag == 6)
                {
                    Grade = "III";
                }
                if (Flag == 7)
                {
                    Grade = "IV";
                }
                if (Flag == 8)
                {
                    Grade = "V";
                }
                if (Flag == 9)
                {
                    Grade = "VI";
                }
                if (Flag == 10)
                {
                    Grade = "VII";
                }
                if (Flag == 11)
                {
                    Grade = "VIII";
                }
                if (Flag == 12)
                {
                    Grade = "IX";
                }
                if (Flag == 13)
                {
                    Grade = "X";
                }
                if (Flag == 14)
                {
                    Grade = "XI";
                }
                if (Flag == 15)
                {
                    Grade = "XII";
                }
                return Grade;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
            }
            return null;
        }
        #endregion

        #region MIS Consolidate Report Service
        public bool SendMISConsolidateReport()
        {
            bool retValue = false;
            DateTime Currnt_Date = DateTime.Now;
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Clear();
            criteria.Add("EmailType", "MISConsolidateReport");
            criteria.Add("IsMailNeeded", true);
            Dictionary<long, IList<MISMailMaster>> MISMailMasterDetails = GetMISMailMasterListWithPaging(null, 9999, string.Empty, string.Empty, criteria);
            if (MISMailMasterDetails != null && MISMailMasterDetails.Count > 0 && MISMailMasterDetails.FirstOrDefault().Value.Count > 0)
            {
                foreach (MISMailMaster item in MISMailMasterDetails.First().Value)
                {
                    DateTime LastReportSentDate = new DateTime();
                    criteria.Clear();
                    criteria.Add("EmailRefId", item.Id);
                    Dictionary<long, IList<MISMailStatus>> MailStatusById = GetMISMailStatusListWithPaging(null, 9999, string.Empty, string.Empty, criteria);
                    if (MailStatusById != null && MailStatusById.FirstOrDefault().Value != null && MailStatusById.FirstOrDefault().Value.Count > 0)
                    {
                        int count = MailStatusById.FirstOrDefault().Value.Count;
                        LastReportSentDate = MailStatusById.FirstOrDefault().Value[count - 1].SentDate;
                    }
                    string LastSentDate = (LastReportSentDate.AddDays(3)).ToShortDateString();
                    string currentDateStr = Currnt_Date.ToShortDateString();
                    MISMailStatus MISMailStatus = MailSendStatus(item.Id, item.EmailId, item.SentCategory, item.EmailType);
                    if ((currentDateStr == LastSentDate && MISMailStatus == null) || MailStatusById.FirstOrDefault().Value.Count == 0 || (MISMailStatus != null && MISMailStatus.IsSent == false))
                    {
                        //MISMailStatus MISMailStatus = MailSendStatus(item.Id, item.EmailId, item.SentCategory, item.EmailType);
                        if (MISMailStatus == null || MISMailStatus.IsSent == false)
                        {
                            if (MISMailStatus == null)
                            {
                                MISMailStatus = MISMailStatusFirstInsert(item.Id, item.EmailId, item.Campus, item.SentCategory, item.EmailType);
                            }
                            string CampusVal = item.Campus;
                            byte[] MISExcelFile;
                            bool ret = false;
                            int Currnt_Year = Currnt_Date.Year;
                            string AcademicYear = "";
                            if (Currnt_Date.Month >= 6 && Currnt_Date.Month <= 12)
                            {
                                AcademicYear = Currnt_Year + "-" + (Currnt_Year + 1);
                            }
                            if (Currnt_Date.Month >= 1 && Currnt_Date.Month <= 4)
                            {
                                AcademicYear = (Currnt_Year - 1) + "-" + Currnt_Year;
                            }
                            string FileName = "";
                            string MailBody;
                            MailBody = GetBodyofMail();
                            DataTable table = new DataTable();
                            DataSet Workbookset = new DataSet("Work Book");
                            table.TableName = "MIS Conslidate Report";
                            Workbookset.Tables.Add(table);
                            if (CampusVal == "All")
                            {
                                MISExcelFile = DateWiseMISREportExcel(Workbookset, AcademicYear);
                                FileName = "MISConsolidateReport";
                                ret = email_send(MISExcelFile, FileName, CampusVal, item.EmailId, MailBody, item.EmailType);
                            }
                            if (ret == true)
                            {
                                string TodayDate = DateTime.Now.ToShortDateString();
                                MISMailStatus.IsSent = true;
                                MISMailStatus.CheckDate = TodayDate;
                                MISMailStatus.Campus = item.Campus;
                                MISMailStatus.EmailId = item.EmailId;
                                MISMailStatus.EmailRefId = item.Id;
                                MISMailStatus.SentCategory = item.SentCategory;
                                MISMailStatus.EmailType = item.EmailType;
                                MISMailStatus.SentDate = Currnt_Date;
                                SaveOrUpdateMISMailStatusDetails(MISMailStatus);
                            }
                        }
                    }
                }
            }
            return retValue;
        }
        private byte[] DateWiseMISREportExcel(DataSet Workbookset, string AcademicYear)
        {
            try
            {
                using (ExcelPackage Epkg = new ExcelPackage())
                {
                    string sord = "";
                    sord = sord == "desc" ? "Desc" : "Asc";
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    List<CampusMaster> CampusList = new List<CampusMaster>();
                    List<MISDateWiseReport_vw> DateWiseReportList = new List<MISDateWiseReport_vw>();
                    Dictionary<long, IList<MISDateWiseReport_vw>> MISDateWiseReportList = null;
                    criteria.Clear();
                    criteria.Add("AcademicYear", AcademicYear);
                    MISDateWiseReportList = GetMISDateWiseReport_vwListWithPaging(0, 9999, string.Empty, sord, criteria);
                    DateWiseReportList = MISDateWiseReportList.FirstOrDefault().Value.ToList();
                    criteria.Clear();
                    string sidx = "Flag";
                    Dictionary<long, IList<CampusMaster>> CampusMasterList = GetCampusMasterListWithPaging(0, 9999, sidx, sord, criteria);
                    CampusList = CampusMasterList.FirstOrDefault().Value.ToList();

                    var DirectList = (from u in MISDateWiseReportList.FirstOrDefault().Value
                                      select u).ToList();
                    List<MISDateWiseReport_vw> ExcelShowList = new List<MISDateWiseReport_vw>();

                    MISDateWiseReport_vw CampusTotal = new MISDateWiseReport_vw();


                    long Campus1Total = 0;
                    long Campus1BoysTotal = 0;
                    long Campus1GirlsTotal = 0;
                    long Campus1BoysToddlerTotal = 0;
                    long Campus1GirlsToddlerTotal = 0;
                    long Campus1Toddler = 0;

                    long Campus2Total = 0;
                    long Campus2BoysTotal = 0;
                    long Campus2GirlsTotal = 0;
                    long Campus2BoysToddlerTotal = 0;
                    long Campus2GirlsToddlerTotal = 0;
                    long Campus2Toddler = 0;

                    long Campus3Total = 0;
                    long Campus3BoysTotal = 0;
                    long Campus3GirlsTotal = 0;
                    long Campus3BoysToddlerTotal = 0;
                    long Campus3GirlsToddlerTotal = 0;
                    long Campus3Toddler = 0;

                    long Campus4Total = 0;
                    long Campus4BoysTotal = 0;
                    long Campus4GirlsTotal = 0;
                    long Campus4BoysToddlerTotal = 0;
                    long Campus4GirlsToddlerTotal = 0;
                    long Campus4Toddler = 0;

                    long Campus5Total = 0;
                    long Campus5BoysTotal = 0;
                    long Campus5GirlsTotal = 0;
                    long Campus5BoysToddlerTotal = 0;
                    long Campus5GirlsToddlerTotal = 0;
                    long Campus5Toddler = 0;

                    long Campus6Total = 0;
                    long Campus6BoysTotal = 0;
                    long Campus6GirlsTotal = 0;
                    long Campus6BoysToddlerTotal = 0;
                    long Campus6GirlsToddlerTotal = 0;
                    long Campus6Toddler = 0;

                    long Campus7Total = 0;
                    long Campus7BoysTotal = 0;
                    long Campus7GirlsTotal = 0;
                    long Campus7BoysToddlerTotal = 0;
                    long Campus7GirlsToddlerTotal = 0;
                    long Campus7Toddler = 0;

                    long Campus8Total = 0;
                    long Campus8BoysTotal = 0;
                    long Campus8GirlsTotal = 0;
                    long Campus8BoysToddlerTotal = 0;
                    long Campus8GirlsToddlerTotal = 0;
                    long Campus8Toddler = 0;

                    long Campus9Total = 0;
                    long Campus9BoysTotal = 0;
                    long Campus9GirlsTotal = 0;
                    long Campus9BoysToddlerTotal = 0;
                    long Campus9GirlsToddlerTotal = 0;
                    long Campus9Toddler = 0;

                    long Campus10Total = 0;
                    long Campus10BoysTotal = 0;
                    long Campus10GirlsTotal = 0;
                    long Campus10BoysToddlerTotal = 0;
                    long Campus10GirlsToddlerTotal = 0;
                    long Campus10Toddler = 0;

                    long Campus11Total = 0;
                    long Campus11BoysTotal = 0;
                    long Campus11GirlsTotal = 0;
                    long Campus11BoysToddlerTotal = 0;
                    long Campus11GirlsToddlerTotal = 0;
                    long Campus11Toddler = 0;

                    long Campus12Total = 0;
                    long Campus12BoysTotal = 0;
                    long Campus12GirlsTotal = 0;
                    long Campus12BoysToddlerTotal = 0;
                    long Campus12GirlsToddlerTotal = 0;
                    long Campus12Toddler = 0;

                    long Campus13Total = 0;
                    long Campus13BoysTotal = 0;
                    long Campus13GirlsTotal = 0;
                    long Campus13BoysToddlerTotal = 0;
                    long Campus13GirlsToddlerTotal = 0;
                    long Campus13Toddler = 0;

                    long ForEachLoopCount = 0;

                    string[] ThreeDaysBetweenDateStringArry = null;
                    string[] BetWeenDateStringArray = null;
                    DateTime MonthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    DateTime TodayDate = DateTime.Now;

                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);

                    ThreeDaysBetweenDateStringArry = GetThreeDaysOnceList(MonthStart, TodayDate);
                    BetWeenDateStringArray = GetDateStringArraList(TodayDate.AddDays(-3), TodayDate);

                    string[] BetweenTwoDates = null;
                    List<MISDateWiseReport_vw> TotalList = new List<MISDateWiseReport_vw>();
                    foreach (var ThreeDaysBetweenDateItem in ThreeDaysBetweenDateStringArry)
                    {
                        ForEachLoopCount = ForEachLoopCount + 1;

                        List<MISDateWiseReport_vw> ShowList = new List<MISDateWiseReport_vw>();
                        MISDateWiseReport_vw obj = new MISDateWiseReport_vw();
                        obj.DateString = ThreeDaysBetweenDateItem;
                        foreach (var DirectListItem in DirectList)
                        {
                            int FlagCount = 0;
                            DateTime MyDateTime;
                            string Tempdate = "";
                            string[] strArray = ThreeDaysBetweenDateItem.Split('/');
                            string Month = strArray[1];
                            string Day = strArray[0];
                            string Year = strArray[2];
                            if (strArray.Length == 3) { Tempdate = Day + "/" + Month + "/" + Year; }

                            MyDateTime = DateTime.Parse(Tempdate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);

                            BetweenTwoDates = GetDateStringArraList(MyDateTime, TodayDate);
                            foreach (var BetweenTwoDatesItem in BetweenTwoDates)
                            {
                                if (DirectListItem.CreatedDate == BetweenTwoDatesItem)
                                {
                                    FlagCount = FlagCount + 1;
                                }
                            }
                            if (FlagCount == 0) { ShowList.Add(DirectListItem); }
                        }



                        for (int i = 0; i < CampusList.Count; i++)
                        {
                            string CampusName = CampusList[i].Name;
                            var CampusListByName = (from u in ShowList
                                                    where u.Campus == CampusList[i].Name
                                                    select u).ToList();
                            long Boys = 0;
                            long Girls = 0;

                            string BoysString = "";
                            string GirlsString = "";
                            long BoysToddler = 0;
                            long GirlsToddler = 0;

                            long Total = 0;
                            long Toddler = 0;
                            string ToddlerString = "";
                            string TotalString = "";
                            foreach (var item in CampusListByName)
                            {
                                if (item.Grade == "PlaySchool")
                                {
                                    BoysToddler = BoysToddler + item.Boys;
                                    GirlsToddler = GirlsToddler + item.Girls;
                                }
                                else
                                {
                                    Boys = Boys + item.Boys;
                                    Girls = Girls + item.Girls;
                                }
                                Total = Total + item.Total;
                            }

                            if (BoysToddler == 0) { BoysString = Boys.ToString() + ""; }
                            if (BoysToddler == 1) { BoysString = Boys > 1 ? Boys.ToString() + "+" + BoysToddler : BoysToddler + " Toddler"; }
                            if (BoysToddler > 1) { BoysString = Boys > 1 ? Boys.ToString() + "+" + BoysToddler : BoysToddler + " Toddlers"; }

                            if (GirlsToddler == 0) { GirlsString = Girls.ToString() + ""; }
                            if (GirlsToddler == 1) { GirlsString = Girls > 1 ? Girls.ToString() + "+" + GirlsToddler : GirlsToddler + " Toddler"; }
                            if (GirlsToddler > 1) { GirlsString = Girls > 1 ? Girls.ToString() + "+" + GirlsToddler : GirlsToddler + " Toddlers"; }

                            long BoysAndGirlsToddler = BoysToddler + GirlsToddler;
                            Total = Total - BoysAndGirlsToddler;

                            if (BoysAndGirlsToddler == 0) { TotalString = Total.ToString(); }
                            if (BoysAndGirlsToddler == 1) { TotalString = Total > 1 ? Total.ToString() + "+" + BoysAndGirlsToddler : BoysAndGirlsToddler + " Toddler"; }
                            if (BoysAndGirlsToddler > 1) { TotalString = Total > 1 ? Total.ToString() + "+" + BoysAndGirlsToddler : BoysAndGirlsToddler + " Toddlers"; }



                            if (i == 0)
                            {
                                obj.Campus1 = TotalString;
                                obj.Campus1Boys = BoysString;
                                obj.Campus1Girls = GirlsString;

                                if (ForEachLoopCount == (ThreeDaysBetweenDateStringArry.Length - 1))
                                {
                                    Campus1BoysTotal = Campus1BoysTotal + Boys;
                                    Campus1GirlsTotal = Campus1GirlsTotal + Girls;
                                    Campus1BoysToddlerTotal = Campus1BoysToddlerTotal + BoysToddler;
                                    Campus1GirlsToddlerTotal = Campus1GirlsToddlerTotal + GirlsToddler;
                                    Campus1Toddler = Campus1Toddler + BoysAndGirlsToddler;
                                    Campus1Total = Campus1Total + Total;
                                }

                            }
                            if (i == 1)
                            {
                                obj.Campus2 = TotalString;
                                obj.Campus2Boys = BoysString;
                                obj.Campus2Girls = GirlsString;

                                if (ForEachLoopCount == (ThreeDaysBetweenDateStringArry.Length - 1))
                                {
                                    Campus2BoysTotal = Campus2BoysTotal + Boys;
                                    Campus2GirlsTotal = Campus2GirlsTotal + Girls;
                                    Campus2BoysToddlerTotal = Campus2BoysToddlerTotal + BoysToddler;
                                    Campus2GirlsToddlerTotal = Campus2GirlsToddlerTotal + GirlsToddler;
                                    Campus2Toddler = Campus2Toddler + BoysAndGirlsToddler;
                                    Campus2Total = Campus2Total + Total;
                                }
                            }
                            if (i == 2)
                            {
                                obj.Campus3 = TotalString;
                                obj.Campus3Boys = BoysString;
                                obj.Campus3Girls = GirlsString;
                                if (ForEachLoopCount == (ThreeDaysBetweenDateStringArry.Length - 1))
                                {
                                    Campus3BoysTotal = Campus3BoysTotal + Boys;
                                    Campus3GirlsTotal = Campus3GirlsTotal + Girls;
                                    Campus3BoysToddlerTotal = Campus3BoysToddlerTotal + BoysToddler;
                                    Campus3GirlsToddlerTotal = Campus3GirlsToddlerTotal + GirlsToddler;
                                    Campus3Toddler = Campus3Toddler + BoysAndGirlsToddler;
                                    Campus3Total = Campus3Total + Total;
                                }
                            }
                            if (i == 3)
                            {
                                obj.Campus4 = TotalString;
                                obj.Campus4Boys = BoysString;
                                obj.Campus4Girls = GirlsString;
                                if (ForEachLoopCount == (ThreeDaysBetweenDateStringArry.Length - 1))
                                {
                                    Campus4BoysTotal = Campus4BoysTotal + Boys;
                                    Campus4GirlsTotal = Campus4GirlsTotal + Girls;
                                    Campus4BoysToddlerTotal = Campus4BoysToddlerTotal + BoysToddler;
                                    Campus4GirlsToddlerTotal = Campus4GirlsToddlerTotal + GirlsToddler;
                                    Campus4Toddler = Campus4Toddler + BoysAndGirlsToddler;
                                    Campus4Total = Campus4Total + Total;
                                }
                            }
                            if (i == 4)
                            {
                                obj.Campus5 = TotalString;
                                obj.Campus5Boys = BoysString;
                                obj.Campus5Girls = GirlsString;
                                if (ForEachLoopCount == (ThreeDaysBetweenDateStringArry.Length - 1))
                                {
                                    Campus5BoysTotal = Campus5BoysTotal + Boys;
                                    Campus5GirlsTotal = Campus5GirlsTotal + Girls;
                                    Campus5BoysToddlerTotal = Campus5BoysToddlerTotal + BoysToddler;
                                    Campus5GirlsToddlerTotal = Campus5GirlsToddlerTotal + GirlsToddler;
                                    Campus5Toddler = Campus5Toddler + BoysAndGirlsToddler;
                                    Campus5Total = Campus5Total + Total;
                                }
                            }
                            if (i == 5)
                            {
                                obj.Campus6 = TotalString;
                                obj.Campus6Boys = BoysString;
                                obj.Campus6Girls = GirlsString;
                                if (ForEachLoopCount == (ThreeDaysBetweenDateStringArry.Length - 1))
                                {
                                    Campus6BoysTotal = Campus6BoysTotal + Boys;
                                    Campus6GirlsTotal = Campus6GirlsTotal + Girls;
                                    Campus6BoysToddlerTotal = Campus6BoysToddlerTotal + BoysToddler;
                                    Campus6GirlsToddlerTotal = Campus6GirlsToddlerTotal + GirlsToddler;
                                    Campus6Toddler = Campus6Toddler + BoysAndGirlsToddler;
                                    Campus6Total = Campus6Total + Total;
                                }
                            }
                            if (i == 6)
                            {
                                obj.Campus7 = TotalString;
                                obj.Campus7Boys = BoysString;
                                obj.Campus7Girls = GirlsString;
                                if (ForEachLoopCount == (ThreeDaysBetweenDateStringArry.Length - 1))
                                {
                                    Campus7BoysTotal = Campus7BoysTotal + Boys;
                                    Campus7GirlsTotal = Campus7GirlsTotal + Girls;
                                    Campus7BoysToddlerTotal = Campus7BoysToddlerTotal + BoysToddler;
                                    Campus7GirlsToddlerTotal = Campus7GirlsToddlerTotal + GirlsToddler;
                                    Campus7Toddler = Campus7Toddler + BoysAndGirlsToddler;
                                    Campus7Total = Campus7Total + Total;
                                }
                            }
                            if (i == 7)
                            {
                                obj.Campus8 = TotalString;
                                obj.Campus8Boys = BoysString;
                                obj.Campus8Girls = GirlsString;
                                if (ForEachLoopCount == (ThreeDaysBetweenDateStringArry.Length - 1))
                                {
                                    Campus8BoysTotal = Campus8BoysTotal + Boys;
                                    Campus8GirlsTotal = Campus8GirlsTotal + Girls;
                                    Campus8BoysToddlerTotal = Campus8BoysToddlerTotal + BoysToddler;
                                    Campus8GirlsToddlerTotal = Campus8GirlsToddlerTotal + GirlsToddler;
                                    Campus8Toddler = Campus8Toddler + BoysAndGirlsToddler;
                                    Campus8Total = Campus8Total + Total;
                                }
                            }
                            if (i == 8)
                            {
                                obj.Campus9 = TotalString;
                                obj.Campus9Boys = BoysString;
                                obj.Campus9Girls = GirlsString;
                                if (ForEachLoopCount == (ThreeDaysBetweenDateStringArry.Length - 1))
                                {
                                    Campus9BoysTotal = Campus9BoysTotal + Boys;
                                    Campus9GirlsTotal = Campus9GirlsTotal + Girls;
                                    Campus9BoysToddlerTotal = Campus9BoysToddlerTotal + BoysToddler;
                                    Campus9GirlsToddlerTotal = Campus9GirlsToddlerTotal + GirlsToddler;
                                    Campus9Toddler = Campus9Toddler + BoysAndGirlsToddler;
                                    Campus9Total = Campus9Total + Total;
                                }
                            }
                            if (i == 9)
                            {
                                obj.Campus10 = TotalString;
                                obj.Campus10Boys = BoysString;
                                obj.Campus10Girls = GirlsString;
                                if (ForEachLoopCount == (ThreeDaysBetweenDateStringArry.Length - 1))
                                {
                                    Campus10BoysTotal = Campus10BoysTotal + Boys;
                                    Campus10GirlsTotal = Campus10GirlsTotal + Girls;
                                    Campus10BoysToddlerTotal = Campus10BoysToddlerTotal + BoysToddler;
                                    Campus10GirlsToddlerTotal = Campus10GirlsToddlerTotal + GirlsToddler;
                                    Campus10Toddler = Campus10Toddler + BoysAndGirlsToddler;
                                    Campus10Total = Campus10Total + Total;
                                }
                            }
                            if (i == 10)
                            {
                                obj.Campus11 = TotalString;
                                obj.Campus11Boys = BoysString;
                                obj.Campus11Girls = GirlsString;
                                if (ForEachLoopCount == (ThreeDaysBetweenDateStringArry.Length - 1))
                                {
                                    Campus11BoysTotal = Campus11BoysTotal + Boys;
                                    Campus11GirlsTotal = Campus11GirlsTotal + Girls;
                                    Campus11BoysToddlerTotal = Campus11BoysToddlerTotal + BoysToddler;
                                    Campus11GirlsToddlerTotal = Campus11GirlsToddlerTotal + GirlsToddler;
                                    Campus11Toddler = Campus11Toddler + BoysAndGirlsToddler;
                                    Campus11Total = Campus11Total + Total;
                                }
                            }
                            if (i == 11)
                            {
                                obj.Campus12 = TotalString;
                                obj.Campus12Boys = BoysString;
                                obj.Campus12Girls = GirlsString;
                                if (ForEachLoopCount == (ThreeDaysBetweenDateStringArry.Length - 1))
                                {
                                    Campus12BoysTotal = Campus12BoysTotal + Boys;
                                    Campus12GirlsTotal = Campus12GirlsTotal + Girls;
                                    Campus12BoysToddlerTotal = Campus12BoysToddlerTotal + BoysToddler;
                                    Campus12GirlsToddlerTotal = Campus12GirlsToddlerTotal + GirlsToddler;
                                    Campus12Toddler = Campus12Toddler + BoysAndGirlsToddler;
                                    Campus12Total = Campus12Total + Total;
                                }
                            }
                            if (i == 12)
                            {
                                obj.Campus13 = TotalString;
                                obj.Campus13Boys = BoysString;
                                obj.Campus13Girls = GirlsString;
                                if (ForEachLoopCount == (ThreeDaysBetweenDateStringArry.Length - 1))
                                {
                                    Campus13BoysTotal = Campus13BoysTotal + Boys;
                                    Campus13GirlsTotal = Campus13GirlsTotal + Girls;
                                    Campus13BoysToddlerTotal = Campus13BoysToddlerTotal + BoysToddler;
                                    Campus13GirlsToddlerTotal = Campus13GirlsToddlerTotal + GirlsToddler;
                                    Campus13Toddler = Campus13Toddler + BoysAndGirlsToddler;
                                    Campus13Total = Campus13Total + Total;
                                }
                            }
                            Total = 0;
                        }
                        ExcelShowList.Add(obj);
                    }

                    //Campus1Total = Campus1Total - (Campus1BoysToddlerTotal + Campus1GirlsToddlerTotal);

                    //Campus1 Total
                    if (Campus1BoysToddlerTotal == 0) { CampusTotal.Campus1Boys = Campus1BoysTotal + ""; }
                    if (Campus1BoysToddlerTotal == 1) { CampusTotal.Campus1Boys = Campus1BoysTotal + "+" + Campus1BoysToddlerTotal + " Toddler"; }
                    if (Campus1BoysToddlerTotal > 1) { CampusTotal.Campus1Boys = Campus1BoysTotal + "+" + Campus1BoysToddlerTotal + " Toddlers"; }
                    if (Campus1GirlsToddlerTotal == 0) { CampusTotal.Campus1Girls = Campus1GirlsTotal + ""; }
                    if (Campus1GirlsToddlerTotal == 1) { CampusTotal.Campus1Girls = Campus1GirlsTotal + "+" + Campus1GirlsToddlerTotal + " Toddler"; }
                    if (Campus1GirlsToddlerTotal > 1) { CampusTotal.Campus1Girls = Campus1GirlsTotal + "+" + Campus1GirlsToddlerTotal + " Toddlers"; }
                    if (Campus1Toddler == 0) { CampusTotal.Campus1Total = Campus1Total + ""; }
                    if (Campus1Toddler == 1) { CampusTotal.Campus1Total = Campus1Total + "+" + Campus1Toddler + " Toddler"; }
                    if (Campus1Toddler > 1) { CampusTotal.Campus1Total = Campus1Total + "+" + Campus1Toddler + " Toddlers"; }

                    //Campus2 Total
                    if (Campus2BoysToddlerTotal == 0) { CampusTotal.Campus2Boys = Campus2BoysTotal + ""; }
                    if (Campus2BoysToddlerTotal == 1) { CampusTotal.Campus2Boys = Campus2BoysTotal + "+" + Campus2BoysToddlerTotal + " Toddler"; }
                    if (Campus2BoysToddlerTotal > 1) { CampusTotal.Campus2Boys = Campus2BoysTotal + "+" + Campus2BoysToddlerTotal + " Toddlers"; }
                    if (Campus2GirlsToddlerTotal == 0) { CampusTotal.Campus2Girls = Campus2GirlsTotal + ""; }
                    if (Campus2GirlsToddlerTotal == 1) { CampusTotal.Campus2Girls = Campus2GirlsTotal + "+" + Campus2GirlsToddlerTotal + " Toddler"; }
                    if (Campus2GirlsToddlerTotal > 1) { CampusTotal.Campus2Girls = Campus2GirlsTotal + "+" + Campus2GirlsToddlerTotal + " Toddlers"; }
                    if (Campus2Toddler == 0) { CampusTotal.Campus2Total = Campus2Total + ""; }
                    if (Campus2Toddler == 1) { CampusTotal.Campus2Total = Campus2Total + "+" + Campus2Toddler + " Toddler"; }
                    if (Campus2Toddler > 1) { CampusTotal.Campus2Total = Campus2Total + "+" + Campus2Toddler + " Toddlers"; }

                    //Campus3 Total
                    if (Campus3BoysToddlerTotal == 0) { CampusTotal.Campus3Boys = Campus3BoysTotal + ""; }
                    if (Campus3BoysToddlerTotal == 1) { CampusTotal.Campus3Boys = Campus3BoysTotal + "+" + Campus3BoysToddlerTotal + " Toddler"; }
                    if (Campus3BoysToddlerTotal > 1) { CampusTotal.Campus3Boys = Campus3BoysTotal + "+" + Campus3BoysToddlerTotal + " Toddlers"; }
                    if (Campus3GirlsToddlerTotal == 0) { CampusTotal.Campus3Girls = Campus3GirlsTotal + ""; }
                    if (Campus3GirlsToddlerTotal == 1) { CampusTotal.Campus3Girls = Campus3GirlsTotal + "+" + Campus3GirlsToddlerTotal + " Toddler"; }
                    if (Campus3GirlsToddlerTotal > 1) { CampusTotal.Campus3Girls = Campus3GirlsTotal + "+" + Campus3GirlsToddlerTotal + " Toddlers"; }
                    if (Campus3Toddler == 0) { CampusTotal.Campus3Total = Campus3Total + ""; }
                    if (Campus3Toddler == 1) { CampusTotal.Campus3Total = Campus3Total + "+" + Campus3Toddler + " Toddler"; }
                    if (Campus3Toddler > 1) { CampusTotal.Campus3Total = Campus3Total + "+" + Campus3Toddler + " Toddlers"; }

                    //Campus4 Total
                    if (Campus4BoysToddlerTotal == 0) { CampusTotal.Campus4Boys = Campus4BoysTotal + ""; }
                    if (Campus4BoysToddlerTotal == 1) { CampusTotal.Campus4Boys = Campus4BoysTotal + "+" + Campus4BoysToddlerTotal + " Toddler"; }
                    if (Campus4BoysToddlerTotal > 1) { CampusTotal.Campus4Boys = Campus4BoysTotal + "+" + Campus4BoysToddlerTotal + " Toddlers"; }
                    if (Campus4GirlsToddlerTotal == 0) { CampusTotal.Campus4Girls = Campus4GirlsTotal + ""; }
                    if (Campus4GirlsToddlerTotal == 1) { CampusTotal.Campus4Girls = Campus4GirlsTotal + "+" + Campus4GirlsToddlerTotal + " Toddler"; }
                    if (Campus4GirlsToddlerTotal > 1) { CampusTotal.Campus4Girls = Campus4GirlsTotal + "+" + Campus4GirlsToddlerTotal + " Toddlers"; }
                    if (Campus4Toddler == 0) { CampusTotal.Campus4Total = Campus4Total + ""; }
                    if (Campus4Toddler == 1) { CampusTotal.Campus4Total = Campus4Total + "+" + Campus4Toddler + " Toddler"; }
                    if (Campus4Toddler > 1) { CampusTotal.Campus4Total = Campus4Total + "+" + Campus4Toddler + " Toddlers"; }

                    //Campus5 Total
                    if (Campus5BoysToddlerTotal == 0) { CampusTotal.Campus5Boys = Campus5BoysTotal + ""; }
                    if (Campus5BoysToddlerTotal == 1) { CampusTotal.Campus5Boys = Campus5BoysTotal + "+" + Campus5BoysToddlerTotal + " Toddler"; }
                    if (Campus5BoysToddlerTotal > 1) { CampusTotal.Campus5Boys = Campus5BoysTotal + "+" + Campus5BoysToddlerTotal + " Toddlers"; }
                    if (Campus5GirlsToddlerTotal == 0) { CampusTotal.Campus5Girls = Campus5GirlsTotal + ""; }
                    if (Campus5GirlsToddlerTotal == 1) { CampusTotal.Campus5Girls = Campus5GirlsTotal + "+" + Campus5GirlsToddlerTotal + " Toddler"; }
                    if (Campus5GirlsToddlerTotal > 1) { CampusTotal.Campus5Girls = Campus5GirlsTotal + "+" + Campus5GirlsToddlerTotal + " Toddlers"; }
                    if (Campus5Toddler == 0) { CampusTotal.Campus5Total = Campus5Total + ""; }
                    if (Campus5Toddler == 1) { CampusTotal.Campus5Total = Campus5Total + "+" + Campus5Toddler + " Toddler"; }
                    if (Campus5Toddler > 1) { CampusTotal.Campus5Total = Campus5Total + "+" + Campus5Toddler + " Toddlers"; }

                    //Campus6 Total
                    if (Campus6BoysToddlerTotal == 0) { CampusTotal.Campus6Boys = Campus6BoysTotal + ""; }
                    if (Campus6BoysToddlerTotal == 1) { CampusTotal.Campus6Boys = Campus6BoysTotal + "+" + Campus6BoysToddlerTotal + " Toddler"; }
                    if (Campus6BoysToddlerTotal > 1) { CampusTotal.Campus6Boys = Campus6BoysTotal + "+" + Campus6BoysToddlerTotal + " Toddlers"; }
                    if (Campus6GirlsToddlerTotal == 0) { CampusTotal.Campus6Girls = Campus6GirlsTotal + ""; }
                    if (Campus6GirlsToddlerTotal == 1) { CampusTotal.Campus6Girls = Campus6GirlsTotal + "+" + Campus6GirlsToddlerTotal + " Toddler"; }
                    if (Campus6GirlsToddlerTotal > 1) { CampusTotal.Campus6Girls = Campus6GirlsTotal + "+" + Campus6GirlsToddlerTotal + " Toddlers"; }
                    if (Campus6Toddler == 0) { CampusTotal.Campus6Total = Campus6Total + ""; }
                    if (Campus6Toddler == 1) { CampusTotal.Campus6Total = Campus6Total + "+" + Campus6Toddler + " Toddler"; }
                    if (Campus6Toddler > 1) { CampusTotal.Campus6Total = Campus6Total + "+" + Campus6Toddler + " Toddlers"; }

                    //Campus7 Total
                    if (Campus7BoysToddlerTotal == 0) { CampusTotal.Campus7Boys = Campus7BoysTotal + ""; }
                    if (Campus7BoysToddlerTotal == 1) { CampusTotal.Campus7Boys = Campus7BoysTotal + "+" + Campus7BoysToddlerTotal + " Toddler"; }
                    if (Campus7BoysToddlerTotal > 1) { CampusTotal.Campus7Boys = Campus7BoysTotal + "+" + Campus7BoysToddlerTotal + " Toddlers"; }
                    if (Campus7GirlsToddlerTotal == 0) { CampusTotal.Campus7Girls = Campus7GirlsTotal + ""; }
                    if (Campus7GirlsToddlerTotal == 1) { CampusTotal.Campus7Girls = Campus7GirlsTotal + "+" + Campus7GirlsToddlerTotal + " Toddler"; }
                    if (Campus7GirlsToddlerTotal > 1) { CampusTotal.Campus7Girls = Campus7GirlsTotal + "+" + Campus7GirlsToddlerTotal + " Toddlers"; }
                    if (Campus7Toddler == 0) { CampusTotal.Campus7Total = Campus7Total + ""; }
                    if (Campus7Toddler == 1) { CampusTotal.Campus7Total = Campus7Total + "+" + Campus7Toddler + " Toddler"; }
                    if (Campus7Toddler > 1) { CampusTotal.Campus7Total = Campus7Total + "+" + Campus7Toddler + " Toddlers"; }

                    //Campus8 Total
                    if (Campus8BoysToddlerTotal == 0) { CampusTotal.Campus8Boys = Campus8BoysTotal + ""; }
                    if (Campus8BoysToddlerTotal == 1) { CampusTotal.Campus8Boys = Campus8BoysTotal + "+" + Campus8BoysToddlerTotal + " Toddler"; }
                    if (Campus8BoysToddlerTotal > 1) { CampusTotal.Campus8Boys = Campus8BoysTotal + "+" + Campus8BoysToddlerTotal + " Toddlers"; }
                    if (Campus8GirlsToddlerTotal == 0) { CampusTotal.Campus8Girls = Campus8GirlsTotal + ""; }
                    if (Campus8GirlsToddlerTotal == 1) { CampusTotal.Campus8Girls = Campus8GirlsTotal + "+" + Campus8GirlsToddlerTotal + " Toddler"; }
                    if (Campus8GirlsToddlerTotal > 1) { CampusTotal.Campus8Girls = Campus8GirlsTotal + "+" + Campus8GirlsToddlerTotal + " Toddlers"; }
                    if (Campus8Toddler == 0) { CampusTotal.Campus8Total = Campus8Total + ""; }
                    if (Campus8Toddler == 1) { CampusTotal.Campus8Total = Campus8Total + "+" + Campus8Toddler + " Toddler"; }
                    if (Campus8Toddler > 1) { CampusTotal.Campus8Total = Campus8Total + "+" + Campus8Toddler + " Toddlers"; }

                    //Campus9 Total
                    if (Campus9BoysToddlerTotal == 0) { CampusTotal.Campus9Boys = Campus9BoysTotal + ""; }
                    if (Campus9BoysToddlerTotal == 1) { CampusTotal.Campus9Boys = Campus9BoysTotal + "+" + Campus9BoysToddlerTotal + " Toddler"; }
                    if (Campus9BoysToddlerTotal > 1) { CampusTotal.Campus9Boys = Campus9BoysTotal + "+" + Campus9BoysToddlerTotal + " Toddlers"; }
                    if (Campus9GirlsToddlerTotal == 0) { CampusTotal.Campus9Girls = Campus9GirlsTotal + ""; }
                    if (Campus9GirlsToddlerTotal == 1) { CampusTotal.Campus9Girls = Campus9GirlsTotal + "+" + Campus9GirlsToddlerTotal + " Toddler"; }
                    if (Campus9GirlsToddlerTotal > 1) { CampusTotal.Campus9Girls = Campus9GirlsTotal + "+" + Campus9GirlsToddlerTotal + " Toddlers"; }
                    if (Campus9Toddler == 0) { CampusTotal.Campus9Total = Campus9Total + ""; }
                    if (Campus9Toddler == 1) { CampusTotal.Campus9Total = Campus9Total + "+" + Campus9Toddler + " Toddler"; }
                    if (Campus9Toddler > 1) { CampusTotal.Campus9Total = Campus9Total + "+" + Campus9Toddler + " Toddlers"; }

                    //Campus10 Total
                    if (Campus10BoysToddlerTotal == 0) { CampusTotal.Campus10Boys = Campus10BoysTotal + ""; }
                    if (Campus10BoysToddlerTotal == 1) { CampusTotal.Campus10Boys = Campus10BoysTotal + "+" + Campus10BoysToddlerTotal + " Toddler"; }
                    if (Campus10BoysToddlerTotal > 1) { CampusTotal.Campus10Boys = Campus10BoysTotal + "+" + Campus10BoysToddlerTotal + " Toddlers"; }
                    if (Campus10GirlsToddlerTotal == 0) { CampusTotal.Campus10Girls = Campus10GirlsTotal + ""; }
                    if (Campus10GirlsToddlerTotal == 1) { CampusTotal.Campus10Girls = Campus10GirlsTotal + "+" + Campus10GirlsToddlerTotal + " Toddler"; }
                    if (Campus10GirlsToddlerTotal > 1) { CampusTotal.Campus10Girls = Campus10GirlsTotal + "+" + Campus10GirlsToddlerTotal + " Toddlers"; }
                    if (Campus10Toddler == 0) { CampusTotal.Campus10Total = Campus10Total + ""; }
                    if (Campus10Toddler == 1) { CampusTotal.Campus10Total = Campus10Total + "+" + Campus10Toddler + " Toddler"; }
                    if (Campus10Toddler > 1) { CampusTotal.Campus10Total = Campus10Total + "+" + Campus10Toddler + " Toddlers"; }

                    //Campus11 Total
                    if (Campus11BoysToddlerTotal == 0) { CampusTotal.Campus11Boys = Campus11BoysTotal + ""; }
                    if (Campus11BoysToddlerTotal == 1) { CampusTotal.Campus11Boys = Campus11BoysTotal + "+" + Campus11BoysToddlerTotal + " Toddler"; }
                    if (Campus11BoysToddlerTotal > 1) { CampusTotal.Campus11Boys = Campus11BoysTotal + "+" + Campus11BoysToddlerTotal + " Toddlers"; }
                    if (Campus11GirlsToddlerTotal == 0) { CampusTotal.Campus11Girls = Campus11GirlsTotal + ""; }
                    if (Campus11GirlsToddlerTotal == 1) { CampusTotal.Campus11Girls = Campus11GirlsTotal + "+" + Campus11GirlsToddlerTotal + " Toddler"; }
                    if (Campus11GirlsToddlerTotal > 1) { CampusTotal.Campus11Girls = Campus11GirlsTotal + "+" + Campus11GirlsToddlerTotal + " Toddlers"; }
                    if (Campus11Toddler == 0) { CampusTotal.Campus11Total = Campus11Total + ""; }
                    if (Campus11Toddler == 1) { CampusTotal.Campus11Total = Campus11Total + "+" + Campus11Toddler + " Toddler"; }
                    if (Campus11Toddler > 1) { CampusTotal.Campus11Total = Campus11Total + "+" + Campus11Toddler + " Toddlers"; }

                    //Campus12 Total
                    if (Campus12BoysToddlerTotal == 0) { CampusTotal.Campus12Boys = Campus12BoysTotal + ""; }
                    if (Campus12BoysToddlerTotal == 1) { CampusTotal.Campus12Boys = Campus12BoysTotal + "+" + Campus12BoysToddlerTotal + " Toddler"; }
                    if (Campus12BoysToddlerTotal > 1) { CampusTotal.Campus12Boys = Campus12BoysTotal + "+" + Campus12BoysToddlerTotal + " Toddlers"; }
                    if (Campus12GirlsToddlerTotal == 0) { CampusTotal.Campus12Girls = Campus12GirlsTotal + ""; }
                    if (Campus12GirlsToddlerTotal == 1) { CampusTotal.Campus12Girls = Campus12GirlsTotal + "+" + Campus12GirlsToddlerTotal + " Toddler"; }
                    if (Campus12GirlsToddlerTotal > 1) { CampusTotal.Campus12Girls = Campus12GirlsTotal + "+" + Campus12GirlsToddlerTotal + " Toddlers"; }
                    if (Campus12Toddler == 0) { CampusTotal.Campus12Total = Campus12Total + ""; }
                    if (Campus12Toddler == 1) { CampusTotal.Campus12Total = Campus12Total + "+" + Campus12Toddler + " Toddler"; }
                    if (Campus12Toddler > 1) { CampusTotal.Campus12Total = Campus12Total + "+" + Campus12Toddler + " Toddlers"; }

                    //Campus13 Total
                    if (Campus13BoysToddlerTotal == 0) { CampusTotal.Campus13Boys = Campus13BoysTotal + ""; }
                    if (Campus13BoysToddlerTotal == 1) { CampusTotal.Campus13Boys = Campus13BoysTotal + "+" + Campus13BoysToddlerTotal + " Toddler"; }
                    if (Campus13BoysToddlerTotal > 1) { CampusTotal.Campus13Boys = Campus13BoysTotal + "+" + Campus13BoysToddlerTotal + " Toddlers"; }
                    if (Campus13GirlsToddlerTotal == 0) { CampusTotal.Campus13Girls = Campus13GirlsTotal + ""; }
                    if (Campus13GirlsToddlerTotal == 1) { CampusTotal.Campus13Girls = Campus13GirlsTotal + "+" + Campus13GirlsToddlerTotal + " Toddler"; }
                    if (Campus13GirlsToddlerTotal > 1) { CampusTotal.Campus13Girls = Campus13GirlsTotal + "+" + Campus13GirlsToddlerTotal + " Toddlers"; }
                    if (Campus13Toddler == 0) { CampusTotal.Campus13Total = Campus13Total + ""; }
                    if (Campus13Toddler == 1) { CampusTotal.Campus13Total = Campus13Total + "+" + Campus13Toddler + " Toddler"; }
                    if (Campus13Toddler > 1) { CampusTotal.Campus13Total = Campus13Total + "+" + Campus13Toddler + " Toddlers"; }

                    TotalList.Add(CampusTotal);

                    int TotalRow = ((ExcelShowList.Count) + 2) + ExcelShowList.Count;
                    int TableCount = Workbookset.Tables.Count;
                    ExcelWorksheet ews = Epkg.Workbook.Worksheets.Add(Workbookset.Tables[0].TableName);
                    ews.View.ZoomScale = 100;

                    ews.View.ShowGridLines = false;
                    ews.Cells["A1:AZ1"].Merge = true;
                    ews.Cells["A1:AZ1"].Value = "MIS Date Wise Report";
                    ews.Cells["A1:AZ1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //ews.Row(1).Height = 30.00D;

                    ews.Cells["A2"].Value = "Campus";
                    ews.Cells["A3"].Value = "Academic Year";
                    ews.Cells["A4"].Value = "Date";

                    //ews.Cells["A3:B8"].Style.Font.Size = 13;
                    //ews.Cells["A3:B8"].Style.Font.Name = "Calibri";
                    //ews.Cells["A3:B8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    //ews.Cells["C3:CN8"].Style.Font.Size = 13;
                    //ews.Cells["C3:CN8"].Style.Font.Name = "Calibri";
                    //ews.Cells["C3:CN8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    //Campus1
                    ews.Cells["B2:D2"].Merge = true;
                    ews.Cells["B2:D2"].Value = CampusMasterList.FirstOrDefault().Value[0].ShowName;
                    ews.Cells["B3:D3"].Merge = true;
                    ews.Cells["B3:D3"].Value = AcademicYear;
                    ews.Cells["B4"].Value = "Boys";
                    ews.Cells["C4"].Value = "Girls";
                    ews.Cells["D4"].Value = "Total";
                    //Campus2
                    ews.Cells["F2:H2"].Merge = true;
                    ews.Cells["F2:H2"].Value = CampusMasterList.FirstOrDefault().Value[1].ShowName;
                    ews.Cells["F3:H3"].Merge = true;
                    ews.Cells["F3:H3"].Value = AcademicYear;
                    ews.Cells["F4"].Value = "Boys";
                    ews.Cells["G4"].Value = "Girls";
                    ews.Cells["H4"].Value = "Total";
                    //Campus3
                    ews.Cells["J2:L2"].Merge = true;
                    ews.Cells["J2:L2"].Value = CampusMasterList.FirstOrDefault().Value[2].ShowName;
                    ews.Cells["J3:L3"].Merge = true;
                    ews.Cells["J3:L3"].Value = AcademicYear;
                    ews.Cells["J4"].Value = "Boys";
                    ews.Cells["K4"].Value = "Girls";
                    ews.Cells["L4"].Value = "Total";
                    //Campus4
                    ews.Cells["N2:P2"].Merge = true;
                    ews.Cells["N2:P2"].Value = CampusMasterList.FirstOrDefault().Value[3].ShowName;
                    ews.Cells["N3:P3"].Merge = true;
                    ews.Cells["N3:P3"].Value = AcademicYear;
                    ews.Cells["N4"].Value = "Boys";
                    ews.Cells["O4"].Value = "Girls";
                    ews.Cells["P4"].Value = "Total";
                    //Campus5
                    ews.Cells["R2:T2"].Merge = true;
                    ews.Cells["R2:T2"].Value = CampusMasterList.FirstOrDefault().Value[4].ShowName;
                    ews.Cells["R3:T3"].Merge = true;
                    ews.Cells["R3:T3"].Value = AcademicYear;
                    ews.Cells["R4"].Value = "Boys";
                    ews.Cells["S4"].Value = "Girls";
                    ews.Cells["T4"].Value = "Total";
                    //Campus6
                    ews.Cells["V2:X2"].Merge = true;
                    ews.Cells["V2:X2"].Value = CampusMasterList.FirstOrDefault().Value[5].ShowName;
                    ews.Cells["V3:X3"].Merge = true;
                    ews.Cells["V3:X3"].Value = AcademicYear;
                    ews.Cells["V4"].Value = "Boys";
                    ews.Cells["W4"].Value = "Girls";
                    ews.Cells["X4"].Value = "Total";
                    //Campus7
                    ews.Cells["Z2:AB2"].Merge = true;
                    ews.Cells["Z2:AB2"].Value = CampusMasterList.FirstOrDefault().Value[6].ShowName;
                    ews.Cells["Z3:AB3"].Merge = true;
                    ews.Cells["Z3:AB3"].Value = AcademicYear;
                    ews.Cells["Z4"].Value = "Boys";
                    ews.Cells["A4"].Value = "Girls";
                    ews.Cells["AB4"].Value = "Total";
                    //Campus8
                    ews.Cells["AD2:AF2"].Merge = true;
                    ews.Cells["AD2:AF2"].Value = CampusMasterList.FirstOrDefault().Value[7].ShowName;
                    ews.Cells["AD3:AF3"].Merge = true;
                    ews.Cells["AD3:AF3"].Value = AcademicYear;
                    ews.Cells["AD4"].Value = "Boys";
                    ews.Cells["AE4"].Value = "Girls";
                    ews.Cells["AF4"].Value = "Total";
                    //Campus9
                    ews.Cells["AH2:AJ2"].Merge = true;
                    ews.Cells["AH2:AJ2"].Value = CampusMasterList.FirstOrDefault().Value[8].ShowName;
                    ews.Cells["AH3:AJ3"].Merge = true;
                    ews.Cells["AH3:AJ3"].Value = AcademicYear;
                    ews.Cells["AH4"].Value = "Boys";
                    ews.Cells["AI4"].Value = "Girls";
                    ews.Cells["AJ4"].Value = "Total";
                    //Campus10
                    ews.Cells["AL2:AN2"].Merge = true;
                    ews.Cells["AL2:AN2"].Value = CampusMasterList.FirstOrDefault().Value[9].ShowName;
                    ews.Cells["AL3:AN3"].Merge = true;
                    ews.Cells["AL3:AN3"].Value = AcademicYear;
                    ews.Cells["AL4"].Value = "Boys";
                    ews.Cells["AM4"].Value = "Girls";
                    ews.Cells["AN4"].Value = "Total";
                    //Campus11
                    ews.Cells["AP2:AR2"].Merge = true;
                    ews.Cells["AP2:AR2"].Value = CampusMasterList.FirstOrDefault().Value[10].ShowName;
                    ews.Cells["AP3:AR3"].Merge = true;
                    ews.Cells["AP3:AR3"].Value = AcademicYear;
                    ews.Cells["AP4"].Value = "Boys";
                    ews.Cells["AQ4"].Value = "Girls";
                    ews.Cells["AR4"].Value = "Total";
                    //Campus12
                    ews.Cells["AT2:AV2"].Merge = true;
                    ews.Cells["AT2:AV2"].Value = CampusMasterList.FirstOrDefault().Value[11].ShowName;
                    ews.Cells["AT3:AV3"].Merge = true;
                    ews.Cells["AT3:AV3"].Value = AcademicYear;
                    ews.Cells["AT4"].Value = "Boys";
                    ews.Cells["AU4"].Value = "Girls";
                    ews.Cells["AV4"].Value = "Total";
                    //Campus13
                    ews.Cells["AX2:AZ2"].Merge = true;
                    ews.Cells["AX2:AZ2"].Value = CampusMasterList.FirstOrDefault().Value[12].ShowName;
                    ews.Cells["AX3:AZ3"].Merge = true;
                    ews.Cells["AX3:AZ3"].Value = AcademicYear;
                    ews.Cells["AX4"].Value = "Boys";
                    ews.Cells["AY4"].Value = "Girls";
                    ews.Cells["AZ4"].Value = "Total";


                    int j = 0;
                    int count = 0;
                    for (int i = 5; i <= TotalRow; i++)
                    {
                        if (count < ExcelShowList.Count)
                        {
                            if (ExcelShowList[j] != null)
                            {
                                count++;
                                ews.Cells["A" + i].Value = ExcelShowList[j].DateString;
                                //Campus1
                                ews.Cells["B" + i].Value = ExcelShowList[j].Campus1Boys;
                                ews.Cells["C" + i].Value = ExcelShowList[j].Campus1Girls;
                                ews.Cells["D" + i].Value = ExcelShowList[j].Campus1;
                                //Campus2
                                ews.Cells["F" + i].Value = ExcelShowList[j].Campus2Boys;
                                ews.Cells["G" + i].Value = ExcelShowList[j].Campus2Girls;
                                ews.Cells["H" + i].Value = ExcelShowList[j].Campus2;
                                //Campus3
                                ews.Cells["J" + i].Value = ExcelShowList[j].Campus3Boys;
                                ews.Cells["K" + i].Value = ExcelShowList[j].Campus3Girls;
                                ews.Cells["L" + i].Value = ExcelShowList[j].Campus3;
                                //Campus4
                                ews.Cells["N" + i].Value = ExcelShowList[j].Campus4Boys;
                                ews.Cells["O" + i].Value = ExcelShowList[j].Campus4Girls;
                                ews.Cells["P" + i].Value = ExcelShowList[j].Campus4;
                                //Campus5
                                ews.Cells["R" + i].Value = ExcelShowList[j].Campus5Boys;
                                ews.Cells["S" + i].Value = ExcelShowList[j].Campus5Girls;
                                ews.Cells["T" + i].Value = ExcelShowList[j].Campus5;
                                //Campus6
                                ews.Cells["V" + i].Value = ExcelShowList[j].Campus6Boys;
                                ews.Cells["W" + i].Value = ExcelShowList[j].Campus6Girls;
                                ews.Cells["X" + i].Value = ExcelShowList[j].Campus6;
                                //Campus7
                                ews.Cells["Z" + i].Value = ExcelShowList[j].Campus7Boys;
                                ews.Cells["AA" + i].Value = ExcelShowList[j].Campus7Girls;
                                ews.Cells["AB" + i].Value = ExcelShowList[j].Campus7;
                                //Campus8
                                ews.Cells["AD" + i].Value = ExcelShowList[j].Campus8Boys;
                                ews.Cells["AE" + i].Value = ExcelShowList[j].Campus8Girls;
                                ews.Cells["AF" + i].Value = ExcelShowList[j].Campus8;
                                //Campus9
                                ews.Cells["AH" + i].Value = ExcelShowList[j].Campus9Boys;
                                ews.Cells["AI" + i].Value = ExcelShowList[j].Campus9Girls;
                                ews.Cells["AJ" + i].Value = ExcelShowList[j].Campus9;
                                //Campus10
                                ews.Cells["AL" + i].Value = ExcelShowList[j].Campus10Boys;
                                ews.Cells["AM" + i].Value = ExcelShowList[j].Campus10Girls;
                                ews.Cells["AN" + i].Value = ExcelShowList[j].Campus10;
                                //Campus11
                                ews.Cells["AP" + i].Value = ExcelShowList[j].Campus11Boys;
                                ews.Cells["AQ" + i].Value = ExcelShowList[j].Campus11Girls;
                                ews.Cells["AR" + i].Value = ExcelShowList[j].Campus11;
                                //Campus12
                                ews.Cells["AT" + i].Value = ExcelShowList[j].Campus12Boys;
                                ews.Cells["AU" + i].Value = ExcelShowList[j].Campus12Girls;
                                ews.Cells["AV" + i].Value = ExcelShowList[j].Campus12;
                                //Campus13
                                ews.Cells["AX" + i].Value = ExcelShowList[j].Campus13Boys;
                                ews.Cells["AY" + i].Value = ExcelShowList[j].Campus13Girls;
                                ews.Cells["AZ" + i].Value = ExcelShowList[j].Campus13;
                                j = j + 1;
                            }
                        }
                        //i++;
                    }
                    j = 0;
                    count = 0;
                    for (int i = TotalRow + 1; i <= TotalRow + 1; i++)
                    {
                        if (TotalList[j] != null)
                        {
                            //Campus 1
                            ews.Cells["A" + i].Value = "Total";
                            ews.Cells["B" + i].Value = TotalList[j].Campus1Boys;
                            ews.Cells["C" + i].Value = TotalList[j].Campus1Girls;
                            ews.Cells["D" + i].Value = TotalList[j].Campus1Total;
                            //Campus 2
                            ews.Cells["F" + i].Value = TotalList[j].Campus2Boys;
                            ews.Cells["G" + i].Value = TotalList[j].Campus2Girls;
                            ews.Cells["H" + i].Value = TotalList[j].Campus2Total;
                            //Campus 3
                            ews.Cells["J" + i].Value = TotalList[j].Campus3Boys;
                            ews.Cells["K" + i].Value = TotalList[j].Campus3Girls;
                            ews.Cells["L" + i].Value = TotalList[j].Campus3Total;
                            //Campus 4
                            ews.Cells["N" + i].Value = TotalList[j].Campus4Boys;
                            ews.Cells["O" + i].Value = TotalList[j].Campus4Girls;
                            ews.Cells["P" + i].Value = TotalList[j].Campus4Total;
                            //Campus 5
                            ews.Cells["R" + i].Value = TotalList[j].Campus5Boys;
                            ews.Cells["S" + i].Value = TotalList[j].Campus5Girls;
                            ews.Cells["T" + i].Value = TotalList[j].Campus5Total;
                            //Campus 6
                            ews.Cells["V" + i].Value = TotalList[j].Campus6Boys;
                            ews.Cells["W" + i].Value = TotalList[j].Campus6Girls;
                            ews.Cells["X" + i].Value = TotalList[j].Campus6Total;
                            //Campus 7
                            ews.Cells["Z" + i].Value = TotalList[j].Campus7Boys;
                            ews.Cells["AA" + i].Value = TotalList[j].Campus7Girls;
                            ews.Cells["AB" + i].Value = TotalList[j].Campus7Total;
                            //Campus 8
                            ews.Cells["AD" + i].Value = TotalList[j].Campus8Boys;
                            ews.Cells["AE" + i].Value = TotalList[j].Campus8Girls;
                            ews.Cells["AF" + i].Value = TotalList[j].Campus8Total;
                            //Campus 9
                            ews.Cells["AH" + i].Value = TotalList[j].Campus9Boys;
                            ews.Cells["AI" + i].Value = TotalList[j].Campus9Girls;
                            ews.Cells["AJ" + i].Value = TotalList[j].Campus9Total;
                            //Campus 10
                            ews.Cells["AL" + i].Value = TotalList[j].Campus10Boys;
                            ews.Cells["AM" + i].Value = TotalList[j].Campus10Girls;
                            ews.Cells["AN" + i].Value = TotalList[j].Campus10Total;
                            //Campus 11
                            ews.Cells["AP" + i].Value = TotalList[j].Campus11Boys;
                            ews.Cells["AQ" + i].Value = TotalList[j].Campus11Girls;
                            ews.Cells["AR" + i].Value = TotalList[j].Campus11Total;
                            //Campus 12
                            ews.Cells["AT" + i].Value = TotalList[j].Campus12Boys;
                            ews.Cells["AU" + i].Value = TotalList[j].Campus12Girls;
                            ews.Cells["AV" + i].Value = TotalList[j].Campus12Total;
                            //Campus 13
                            ews.Cells["AX" + i].Value = TotalList[j].Campus13Boys;
                            ews.Cells["AY" + i].Value = TotalList[j].Campus13Girls;
                            ews.Cells["AZ" + i].Value = TotalList[j].Campus13Total;
                            j = j + 1;
                        }
                        i++;
                    }
                    int LastRow = TotalRow + 1;
                    ews.Cells["A1" + ":AZ" + LastRow].Style.Font.Name = "Calibri";
                    ews.Cells["A1" + ":AZ" + LastRow].Style.Font.Size = 13;
                    ews.Cells["A2" + ":A" + LastRow].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    ews.Cells["B2" + ":AZ" + LastRow].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    ews.Cells["E2" + ":E" + LastRow].Merge = true;
                    ews.Cells["I2" + ":I" + LastRow].Merge = true;
                    ews.Cells["M2" + ":M" + LastRow].Merge = true;
                    ews.Cells["Q2" + ":Q" + LastRow].Merge = true;
                    ews.Cells["U2" + ":U" + LastRow].Merge = true;
                    ews.Cells["Y2" + ":Y" + LastRow].Merge = true;
                    ews.Cells["AC2" + ":AC" + LastRow].Merge = true;
                    ews.Cells["AG2" + ":AG" + LastRow].Merge = true;
                    ews.Cells["AK2" + ":AK" + LastRow].Merge = true;
                    ews.Cells["AO2" + ":AO" + LastRow].Merge = true;
                    ews.Cells["AS2" + ":AS" + LastRow].Merge = true;
                    ews.Cells["AW2" + ":AW" + LastRow].Merge = true;

                    ews.Cells["A1" + ":AZ" + LastRow].Style.Border.Bottom.Style = ExcelBorderStyle.Hair;
                    ews.Cells["A1" + ":AZ" + LastRow].Style.Border.Top.Style = ExcelBorderStyle.Hair;
                    ews.Cells["A1" + ":AZ" + LastRow].Style.Border.Left.Style = ExcelBorderStyle.Hair;
                    ews.Cells["A1" + ":AZ" + LastRow].Style.Border.Right.Style = ExcelBorderStyle.Hair;

                    for (int RowNum = 1; RowNum <= LastRow; RowNum++)
                    {
                        ews.Row(RowNum).Height = 30.00D;
                    }
                    ews.Column(1).Width = 18;
                    for (int col = 1; col <= 54; col++)
                    {
                        ews.Column(col).Width = 18;
                    }
                    for (int col = 5; col <= 54; col = col + 4)
                    {
                        ews.Column(col).Width = 10;
                    }

                    //Date for Filename attachment
                    string Todaydate = DateTime.Now.ToString("dd/MM/yyyy");
                    string FileName = "Consolidate-MIS-Report-On-" + Todaydate; ;
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;  filename=" + FileName + ".xlsx");
                    byte[] File = Epkg.GetAsByteArray();
                    HttpContext.Current.Response.BinaryWrite(File);
                    return File;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
            }
            return null;
        }
        public string[] GetDateStringArraList(DateTime StartDate, DateTime EndDate)
        {
            try
            {

                DateTime fromDate = DateTime.Now;
                DateTime toDate = DateTime.Now;
                DateTime[] fromto = new DateTime[2];

                string from = string.Format("{0:MM/dd/yyyy}", StartDate);
                string to = string.Format("{0:MM/dd/yyyy}", EndDate);
                fromDate = Convert.ToDateTime(from + " " + "12:00:00 AM");
                toDate = Convert.ToDateTime(to + " " + "23:59:59");
                fromto[0] = fromDate;
                fromto[1] = toDate;

                List<DateTime> allDates = new List<DateTime>();
                List<string> StringDates = new List<string>();
                for (DateTime date = fromto[0]; date <= fromto[1]; date = date.AddDays(1))
                {
                    string StrDate = date.ToShortDateString();
                    string[] strArray = StrDate.Split('/');
                    string strmonth = strArray[0]; if (strmonth.Length == 1) { strmonth = "0" + strArray[0]; }
                    string strdate = strArray[1]; if (strdate.Length == 1) { strdate = "0" + strArray[1]; }
                    string stryear = strArray[2];
                    string strdateFormat = strdate + "/" + strmonth + "/" + stryear;
                    StringDates.Add(strdateFormat);
                }
                string[] dateArry;
                dateArry = StringDates.ToArray();
                return dateArry;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
            }
            return null;
        }
        public Dictionary<long, IList<MISDateWiseReport_vw>> GetMISDateWiseReport_vwListWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<MISDateWiseReport_vw>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public string[] GetThreeDaysOnceList(DateTime StartDate, DateTime EndDate)
        {
            try
            {

                DateTime fromDate = DateTime.Now;
                DateTime toDate = DateTime.Now;
                DateTime[] fromto = new DateTime[2];

                string from = string.Format("{0:MM/dd/yyyy}", StartDate);
                string to = string.Format("{0:MM/dd/yyyy}", EndDate);
                fromDate = Convert.ToDateTime(from + " " + "12:00:00 AM");
                toDate = Convert.ToDateTime(to + " " + "23:59:59");
                fromto[0] = fromDate;
                fromto[1] = toDate;

                List<DateTime> allDates = new List<DateTime>();
                List<string> StringDates = new List<string>();
                for (DateTime date = fromto[0]; date <= fromto[1]; date = date.AddDays(3))
                {
                    string StrDate = date.ToShortDateString();
                    string[] strArray = StrDate.Split('/');
                    string strmonth = strArray[0]; if (strmonth.Length == 1) { strmonth = "0" + strArray[0]; }
                    string strdate = strArray[1]; if (strdate.Length == 1) { strdate = "0" + strArray[1]; }
                    string stryear = strArray[2];
                    string strdateFormat = strdate + "/" + strmonth + "/" + stryear;
                    StringDates.Add(strdateFormat);
                }
                string[] dateArry;
                dateArry = StringDates.ToArray();
                return dateArry;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
            }
            return null;
        }
        #endregion

        #region DetailedAdmissionReport
        public bool SendAdmissionReport()
        {
            try
            {
                bool retValue = false;
                DateTime Currnt_Date = DateTime.Now;
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Clear();
                criteria.Add("EmailType", "AdmissionReport");
                Dictionary<long, IList<MISMailMaster>> MISMailMasterDetails = GetMISMailMasterListWithPaging(null, 9999, string.Empty, string.Empty, criteria);
                if (MISMailMasterDetails != null && MISMailMasterDetails.Count > 0 && MISMailMasterDetails.FirstOrDefault().Value.Count > 0)
                {
                    foreach (MISMailMaster item in MISMailMasterDetails.First().Value)
                    {
                        string DayName = DateTime.Now.ToString("dddd");
                        if (item.SentCategory == "Daily" || (item.SentCategory == "Weekly" && DayName == "Monday") || (item.SentCategory == "Weekly" && DayName == "Wednesday") || (item.SentCategory == "Weekly" && DayName == "Friday"))
                        {
                            MISMailStatus MISMailStatus = MailSendStatus(item.Id, item.EmailId, item.SentCategory, item.EmailType);
                            if (MISMailStatus == null || MISMailStatus.IsSent == false)
                            {
                                if (MISMailStatus == null)
                                {
                                    MISMailStatus = MISMailStatusFirstInsert(item.Id, item.EmailId, item.Campus, item.SentCategory, item.EmailType);
                                }
                                string CampusVal = item.Campus;
                                byte[] AdmissionReportExcelFile;
                                bool ret;
                                int Currnt_Year = Currnt_Date.Year;
                                string AcademicYear = "";
                                if (Currnt_Date.Month >= 6 && Currnt_Date.Month <= 12)
                                {
                                    AcademicYear = Currnt_Year + "-" + (Currnt_Year + 1);
                                }
                                if (Currnt_Date.Month >= 1 && Currnt_Date.Month <= 4)
                                {
                                    AcademicYear = (Currnt_Year - 1) + "-" + Currnt_Year;
                                }
                                string FileName = "";
                                string MailBody;
                                MailBody = GetBodyofMail();
                                DataTable table = new DataTable();
                                DataSet Workbookset = new DataSet("Work Book");
                                table.TableName = "AdmissionReport";
                                Workbookset.Tables.Add(table);
                                IList<DetailedAdmissionReport_vw> ShowList = new List<DetailedAdmissionReport_vw>();
                                ShowList = AdmissionCountReportList(CampusVal, AcademicYear);
                                AdmissionReportExcelFile = GenerateAdmissionCountReportExcel(Workbookset, CampusVal, AcademicYear, ShowList);
                                FileName = "AdmissionReport-" + CampusVal;
                                ret = email_send(AdmissionReportExcelFile, FileName, CampusVal, item.EmailId, MailBody, item.EmailType);
                                if (ret == true)
                                {
                                    string TodayDate = DateTime.Now.ToShortDateString();
                                    MISMailStatus.IsSent = true;
                                    MISMailStatus.CheckDate = TodayDate;
                                    MISMailStatus.Campus = item.Campus;
                                    MISMailStatus.EmailId = item.EmailId;
                                    MISMailStatus.EmailRefId = item.Id;
                                    MISMailStatus.SentCategory = item.SentCategory;
                                    MISMailStatus.EmailType = item.EmailType;
                                    MISMailStatus.SentDate = Currnt_Date;
                                    SaveOrUpdateMISMailStatusDetails(MISMailStatus);
                                }
                            }
                        }
                    }
                }
                return retValue;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
            }
            return true;
        }
        private byte[] GenerateAdmissionCountReportExcel(DataSet Workbookset, string CampusVal, string AcademicYear, IList<DetailedAdmissionReport_vw> ShowList)
        {
            try
            {
                using (ExcelPackage Epkg = new ExcelPackage())
                {
                    int TableCount = Workbookset.Tables.Count;
                    ExcelWorksheet ews = Epkg.Workbook.Worksheets.Add(Workbookset.Tables[0].TableName);
                    ews.View.ZoomScale = 100;
                    ews.View.ShowGridLines = false;
                    ews.Cells["H1:N28"].Style.Font.Name = "Calibri";
                    ews.Cells["H1:N28"].Style.Font.Size = 13;

                    //For Title
                    ews.Cells["H1:N2"].Merge = true;
                    ews.Cells["H1:N2"].Value = "Admission Count Report for " + CampusVal + " Campus";
                    ews.Cells["H1:N2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    ews.Cells["H3:N4"].Merge = true;
                    ews.Cells["H3:N4"].Value = "Academic Year : " + AcademicYear;
                    ews.Cells["H3:N4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    ews.Cells["H5:L6"].Merge = true;
                    ews.Cells["H5:L6"].Value = "Count Criteria";
                    ews.Cells["H5:L6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    ews.Cells["M5:N6"].Merge = true;
                    ews.Cells["M5:N6"].Value = "Count";
                    ews.Cells["M5:N6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    int TotalData = ShowList.Count;
                    //int TotalExcelRowNumber = (TotalData * 2) + 6;

                    for (int k = 7; k < 28; k++)
                    {
                        ews.Cells["H" + k + ":L" + (k + 1)].Merge = true;
                        ews.Cells["M" + k + ":N" + (k + 1)].Merge = true;
                        k++;
                    }
                    ews.Cells["H7:L8"].Value = "Decliend";
                    ews.Cells["H9:L10"].Value = "Deleted";
                    ews.Cells["H11:L12"].Value = "Dis Continued";
                    ews.Cells["H13:L14"].Value = "In Active";
                    ews.Cells["H15:L16"].Value = "New Enquiry";
                    ews.Cells["H17:L18"].Value = "New Registration";
                    ews.Cells["H19:L20"].Value = "Not Interested";
                    ews.Cells["H21:L22"].Value = "Not Joined";
                    ews.Cells["H23:L24"].Value = "Registered";
                    ews.Cells["H25:L26"].Value = "Sent For Approval";
                    ews.Cells["H27:L28"].Value = "Sent For Clearance";

                    ews.Cells["M7:N28"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ews.Cells["M7:N8"].Value = ShowList.FirstOrDefault().DeclinedCnt;
                    ews.Cells["M9:N10"].Value = ShowList.FirstOrDefault().DeletedCnt;
                    ews.Cells["M11:N12"].Value = ShowList.FirstOrDefault().DiscontinuedCnt;
                    ews.Cells["M13:N14"].Value = ShowList.FirstOrDefault().InactiveCnt;
                    ews.Cells["M15:N16"].Value = ShowList.FirstOrDefault().NewEnquiryCnt;
                    ews.Cells["M17:N18"].Value = ShowList.FirstOrDefault().NewRegistrationCnt;
                    ews.Cells["M19:N20"].Value = ShowList.FirstOrDefault().NotInterestedCnt;
                    ews.Cells["M21:N22"].Value = ShowList.FirstOrDefault().NotJoinedCnt;
                    ews.Cells["M23:N24"].Value = ShowList.FirstOrDefault().RegisteredCnt;
                    ews.Cells["M25:N26"].Value = ShowList.FirstOrDefault().SentForApprovalCnt;
                    ews.Cells["M27:N28"].Value = ShowList.FirstOrDefault().SentForClearanceCnt;

                    ews.Cells["H1:N28"].Style.Border.Bottom.Style = ExcelBorderStyle.Hair;
                    ews.Cells["H1:N28"].Style.Border.Top.Style = ExcelBorderStyle.Hair;
                    ews.Cells["H1:N28"].Style.Border.Left.Style = ExcelBorderStyle.Hair;
                    ews.Cells["H1:N28"].Style.Border.Right.Style = ExcelBorderStyle.Hair;

                    if (!string.IsNullOrEmpty(CampusVal)) { CampusVal = CampusVal.Replace(" ", ""); }
                    //Date for Filename attachment
                    string Todaydate = DateTime.Now.ToString("dd/MM/yyyy");
                    string FileName = "AdmissionCountReport-For-" + CampusVal + "-On-" + Todaydate; ;
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;  filename=" + FileName + ".xlsx");
                    byte[] File = Epkg.GetAsByteArray();
                    HttpContext.Current.Response.BinaryWrite(File);
                    // bool returnval = email_send(File, FileName, CampusVal);
                    //HttpContext.Current.Response.End();
                    return File;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
            }
            return null;
        }
        public IList<DetailedAdmissionReport_vw> AdmissionCountReportList(string Campus, string AcademicYear)
        {
            try
            {
                Dictionary<string, object> ExactCriteria = new Dictionary<string, object>();
                Dictionary<string, object> LikeCriteria = new Dictionary<string, object>();
                string[] strArray = AcademicYear.Split('-');
                string Year1 = strArray[0];
                string Year2 = strArray[1];

                DateTime FromDate = new DateTime(Convert.ToInt32(Year1), 6, 1);
                DateTime ToDate = DateTime.Now;
                string[] DateStringArr = GetDateStringArraList(FromDate, ToDate);
                ExactCriteria.Add("CreatedDate", DateStringArr);
                if (!string.IsNullOrEmpty(Campus)) { ExactCriteria.Add("Campus", Campus); }
                //if (!string.IsNullOrEmpty(AcademicYear)) { ExactCriteria.Add("AcademicYear", AcademicYear); }
                string sidx = "Id";
                string sord = "Asc";
                Dictionary<long, IList<DetailedAdmissionReport_vw>> DetailedAdmissionReport_vwList = DetailedAdmissionReport_vwListWithLikeAndExcactSerachCriteria(0, 9999, string.Empty, string.Empty, ExactCriteria, LikeCriteria);
                List<DetailedAdmissionReport_vw> ShowList = new List<DetailedAdmissionReport_vw>();
                if (string.IsNullOrEmpty(Campus)) { }
                List<CampusMaster> CampusList = new List<CampusMaster>();
                ExactCriteria.Clear();
                sidx = "Flag";
                Dictionary<long, IList<CampusMaster>> CampusMasterList = GetCampusMasterListWithPaging(0, 9999, sidx, sord, ExactCriteria);
                CampusList = CampusMasterList.FirstOrDefault().Value.ToList();
                if (string.IsNullOrEmpty(Campus) || Campus == "")
                {
                    foreach (var items in CampusList)
                    {
                        DetailedAdmissionReport_vw Obj = new DetailedAdmissionReport_vw();
                        Obj.DeclinedCnt = 0;
                        Obj.DeletedCnt = 0;
                        Obj.DiscontinuedCnt = 0;
                        Obj.InactiveCnt = 0;
                        Obj.NewEnquiryCnt = 0;
                        Obj.NewRegistrationCnt = 0;
                        Obj.NotInterestedCnt = 0;
                        Obj.NotJoinedCnt = 0;
                        Obj.RegisteredCnt = 0;
                        Obj.SentForApprovalCnt = 0;
                        Obj.SentForClearanceCnt = 0;
                        var CampusStatusList = (
                         from u in DetailedAdmissionReport_vwList.FirstOrDefault().Value
                         where u.Campus == items.Name
                         select u
                         ).ToList();
                        Obj.Campus = items.Name;
                        foreach (var item in CampusStatusList)
                        {
                            if (item.AdmissionStatus == "Declined")
                            {
                                Obj.DeclinedCnt = Obj.DeclinedCnt + item.Count;
                            }
                            if (item.AdmissionStatus == "Deleted")
                            {
                                Obj.DeletedCnt = Obj.DeletedCnt + item.Count;
                            }
                            if (item.AdmissionStatus == "Discontinued")
                            {
                                Obj.DiscontinuedCnt = Obj.DiscontinuedCnt + item.Count;
                            }
                            if (item.AdmissionStatus == "Inactive")
                            {
                                Obj.InactiveCnt = Obj.InactiveCnt + item.Count;
                            }
                            if (item.AdmissionStatus == "New Enquiry")
                            {
                                Obj.NewEnquiryCnt = Obj.NewEnquiryCnt + item.Count;
                            }
                            if (item.AdmissionStatus == "New Registration")
                            {
                                Obj.NewRegistrationCnt = Obj.NewRegistrationCnt + item.Count;
                            }
                            if (item.AdmissionStatus == "Not Interested")
                            {
                                Obj.NotInterestedCnt = Obj.NotInterestedCnt + item.Count;
                            }
                            if (item.AdmissionStatus == "Not Joined")
                            {
                                Obj.NotJoinedCnt = Obj.NotJoinedCnt + item.Count;
                            }
                            if (item.AdmissionStatus == "Registered")
                            {
                                Obj.RegisteredCnt = Obj.RegisteredCnt + item.Count;
                            }
                            if (item.AdmissionStatus == "Sent For Approval")
                            {
                                Obj.SentForApprovalCnt = Obj.SentForApprovalCnt + item.Count;
                            }
                            if (item.AdmissionStatus == "Sent For Clearance")
                            {
                                Obj.SentForClearanceCnt = Obj.SentForClearanceCnt + item.Count;
                            }
                        }
                        ShowList.Add(Obj);
                    }
                }
                else
                {
                    DetailedAdmissionReport_vw Obj = new DetailedAdmissionReport_vw();
                    Obj.Campus = Campus;
                    Obj.DeclinedCnt = 0;
                    Obj.DeletedCnt = 0;
                    Obj.DiscontinuedCnt = 0;
                    Obj.InactiveCnt = 0;
                    Obj.NewEnquiryCnt = 0;
                    Obj.NewRegistrationCnt = 0;
                    Obj.NotInterestedCnt = 0;
                    Obj.NotJoinedCnt = 0;
                    Obj.RegisteredCnt = 0;
                    Obj.SentForApprovalCnt = 0;
                    Obj.SentForClearanceCnt = 0;
                    foreach (var item in DetailedAdmissionReport_vwList.FirstOrDefault().Value)
                    {
                        if (item.AdmissionStatus == "Declined")
                        {
                            Obj.DeclinedCnt = Obj.DeclinedCnt + item.Count;
                        }
                        if (item.AdmissionStatus == "Deleted")
                        {
                            Obj.DeletedCnt = Obj.DeletedCnt + item.Count;
                        }
                        if (item.AdmissionStatus == "Discontinued")
                        {
                            Obj.DiscontinuedCnt = Obj.DiscontinuedCnt + item.Count;
                        }
                        if (item.AdmissionStatus == "Inactive")
                        {
                            Obj.InactiveCnt = Obj.InactiveCnt + item.Count;
                        }
                        if (item.AdmissionStatus == "New Enquiry")
                        {
                            Obj.NewEnquiryCnt = Obj.NewEnquiryCnt + item.Count;
                        }
                        if (item.AdmissionStatus == "New Registration")
                        {
                            Obj.NewRegistrationCnt = Obj.NewRegistrationCnt + item.Count;
                        }
                        if (item.AdmissionStatus == "Not Interested")
                        {
                            Obj.NotInterestedCnt = Obj.NotInterestedCnt + item.Count;
                        }
                        if (item.AdmissionStatus == "Not Joined")
                        {
                            Obj.NotJoinedCnt = Obj.NotJoinedCnt + item.Count;
                        }
                        if (item.AdmissionStatus == "Registered")
                        {
                            Obj.RegisteredCnt = Obj.RegisteredCnt + item.Count;
                        }
                        if (item.AdmissionStatus == "Sent For Approval")
                        {
                            Obj.SentForApprovalCnt = Obj.SentForApprovalCnt + item.Count;
                        }
                        if (item.AdmissionStatus == "Sent For Clearance")
                        {
                            Obj.SentForClearanceCnt = Obj.SentForClearanceCnt + item.Count;
                        }
                    }
                    ShowList.Add(Obj);
                }
                return ShowList;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public Dictionary<long, IList<DetailedAdmissionReport_vw>> DetailedAdmissionReport_vwListWithLikeAndExcactSerachCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> exctcriteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCountWithDecimal<DetailedAdmissionReport_vw>(page, pageSize, sortType, sortby, exctcriteria, likeCriteria);

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region AdmissionStatusWiseReport
        public Dictionary<long, IList<AdmissionStatusReport_vw>> GetAdmissionStatusListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<AdmissionStatusReport_vw>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        public Dictionary<long, IList<PastStudentList_Vw>> GetPastStudentRecords_VwCountWithPagingAndCriteria(int? page, int rows, string sortBy, string sortType, Dictionary<string, object> searchCriteria)
        {
            try
            {
                return PSF.GetListWithLikeSearchCriteriaCount<PastStudentList_Vw>(page, rows, sortBy, sortType, searchCriteria);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<MISMonthlyReport_Vw>> GetMISMonthlyReport_vwListWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<MISMonthlyReport_Vw>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #region PageHistoryReport by Karthy
        public long SaveOrUpdatePageHistory(PageHistory pageHistory)
        {
            try
            {
                if (pageHistory != null)
                {
                    string query = string.Empty;
                    string querystr = string.Empty;
                    PSF.SaveOrUpdate<PageHistory>(pageHistory);
                    query = query + "if exists (select * from PageHistoryReport where ControllerName='" + pageHistory.Controller + "' and ActionName='" + pageHistory.Action + "' and Campus='" + pageHistory.Campus + "')";
                    query = query + "begin";
                    query = query + " update PageHistoryReport set ControllerHit=ControllerHit + 1 , ActionHit=ActionHit + 1,ActionName='" + pageHistory.Action + "' where ControllerName='" + pageHistory.Controller + "' and Campus='" + pageHistory.Campus + "'";
                    query = query + "end else ";
                    query = query + "begin";
                    query = query + " insert into PageHistoryReport values('" + pageHistory.Campus + "','" + pageHistory.Controller + "',1,'" + pageHistory.Action + "',1,null,null,null,null)";
                    query = query + "end";
                    querystr = query;
                    string queryobj = querystr;
                    IList list = PSF.ExecuteSql(queryobj);
                    // return (list);
                }
                else { throw new Exception("doc is required and it cannot be null.."); }
                return pageHistory.PageHistory_Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<long, IList<PageHistoryReport>> GetPageHistoryReportListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<PageHistoryReport>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region CampusWiseUsageModule

        public Dictionary<long, IList<CampusWiseUsageModule>> GetCampusWiseUsageModuleListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<CampusWiseUsageModule>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long SaveOrUpdateCampusWiseUsageModule(CampusWiseUsageModule CampusWiseUsageModule)
        {
            try
            {
                if (CampusWiseUsageModule != null)
                    PSF.SaveOrUpdate<CampusWiseUsageModule>(CampusWiseUsageModule);
                else { throw new Exception("doc is required and it cannot be null.."); }
                return CampusWiseUsageModule.CampusWiseUsageModule_Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public long SaveOrUpdatecampusWiseUsageModule(CampusWiseUsageModule campuswiseusagemodule)
        {
            try
            {
                if (campuswiseusagemodule != null)
                    PSF.SaveOrUpdate<CampusWiseUsageModule>(campuswiseusagemodule);
                else { throw new Exception("doc is required and it cannot be null.."); }
                return campuswiseusagemodule.CampusWiseUsageModule_Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public CampusWiseUsageModule GetCampusWiseUsageModuleById(long Id)
        {
            try
            {

                CampusWiseUsageModule CampusWiseUsageModule = null;
                if (Id > 0)
                    CampusWiseUsageModule = PSF.Get<CampusWiseUsageModule>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return CampusWiseUsageModule;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool DeleteCampusWiseUsageModule(long Id)
        {
            try
            {
                CampusWiseUsageModule CampusWiseUsageModule = PSF.Get<CampusWiseUsageModule>(Id);
                PSF.Delete<CampusWiseUsageModule>(CampusWiseUsageModule);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region JQGrid CampusWiseUsageModule_vw
        public Dictionary<long, IList<CampusWiseUsageModule_vw>> GetCampusWiseUsageModule_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<CampusWiseUsageModule_vw>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region CampusWiseModuleUsageReport
        public Dictionary<long, IList<CampusWiseModuleUsageReport_vw>> GetCampusWiseModuleUsageReport_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<CampusWiseModuleUsageReport_vw>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

    }
}
