using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using TIPS.Entities;
using TIPS.Entities.StudentsReportEntities;
using TIPS.Service;
using TIPS.ServiceContract;
using OfficeOpenXml;
using System.Drawing;
using OfficeOpenXml.Style;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.IO;
using TIPS.Entities.AdmissionEntities;
using TIPS.Entities.ReportEntities;
using System.Text;
using TIPS.Entities.MenuEntities;



namespace CMS.Controllers
{
    public class StudentsReportController : BaseController
    {
        StudentsReportService SRS = new StudentsReportService();
        MastersService MS = new MastersService();
        public ActionResult Index()
        {
            return View();
        }
        #region MIS Reports
        public ActionResult MISReport(string campus)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (campus == "All" || campus == null)
                {
                    AdmissionManagementService AMS = new AdmissionManagementService();
                    DateTime Currnt_Date = DateTime.Now;
                    int Currnt_Year = Currnt_Date.Year;
                    string AcademicYear = "" + Currnt_Year + "-" + (Currnt_Year + 1) + "";
                    //Dictionary<string, object> criteria = new Dictionary<string, object>();
                    List<MISCampusReport> OverAllList = new List<MISCampusReport>();
                    IList<MISOverAllReport_vw> OverAllListVw = new List<MISOverAllReport_vw>();
                    criteria.Clear();
                    Dictionary<long, IList<StudentTemplate>> StudentTemplate = AMS.GetStudentDetailsListWithEQsearchCriteria(null, 9999, string.Empty, string.Empty, criteria);
                    var CampusList = (from u in StudentTemplate.First().Value
                                      select u.Campus).Distinct().ToList();
                    for (int i = 0; i < CampusList.Count; i++)
                    {
                        MISCampusReport CR = new MISCampusReport();
                        CR.Campus = CampusList[i];
                        CR.Boys = 0;
                        CR.Girls = 0;
                        CR.Total = 0;
                        //CR.AcdYr = AcademicYear;
                        OverAllListVw = GetOverAllReport(CR.Campus, AcademicYear);
                        if (OverAllListVw != null)
                        {
                            CR.Boys = OverAllListVw.FirstOrDefault().Boys;
                            CR.Girls = OverAllListVw.FirstOrDefault().Girls;
                            CR.Total = OverAllListVw.FirstOrDefault().Total;
                        }
                        //CR.Total = CR.Total + CR.Total;
                        OverAllList.Add(CR);
                    }
                    ViewBag.OverAllList = OverAllList;
                    ViewBag.Pagename = "OverAll";
                    ViewBag.AcademicYear = AcademicYear;
                    Session["MISCampus"] = campus != null ? campus : "";
                    return View();
                }
                else
                {
                    List<MISCampusReport> SingleCampusList = new List<MISCampusReport>();
                    IList<MISReport_vw> ReportDetails = new List<MISReport_vw>();
                    DateTime Currnt_Date = DateTime.Now;
                    int Currnt_Year = Currnt_Date.Year;
                    string AcademicYear = "" + Currnt_Year + "-" + (Currnt_Year + 1) + "";
                    for (int i = 0; i < 16; i++)
                    {
                        MISCampusReport CR = new MISCampusReport();
                        CR.Boys = 0;
                        CR.Girls = 0;
                        CR.Total = 0;
                        CR.OverAllTotal = 0;
                        CR.Grade = GetGradeName(i);
                        CR.ShowGrade = GetGradeShowName(i);
                        //CR.AcdYr = AcademicYear;
                        criteria.Clear();
                        criteria.Add("Campus", campus);
                        criteria.Add("Grade", CR.Grade);
                        ReportDetails = MISReportWithCriteria(campus, CR.Grade, AcademicYear);
                        if (ReportDetails != null && ReportDetails.Count > 0)
                        {
                            CR.Boys = ReportDetails[0].Boys;
                            CR.Girls = ReportDetails[0].Girls;
                            CR.Total = ReportDetails[0].Total;
                        }
                        else
                        {
                            CR.Boys = 0;
                            CR.Girls = 0;
                            CR.Total = 0;
                        }
                        CR.OverAllTotal = CR.OverAllTotal + CR.Total;
                        SingleCampusList.Add(CR);
                    }
                    ViewBag.SingleCampusList = SingleCampusList;
                    ViewBag.Pagename = "Single";
                    ViewBag.AcademicYear = AcademicYear;
                    Session["MISCampus"] = campus != null ? campus : "";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult MISReportWithExcel(string Campus)
        {
            DateTime Currnt_Date = DateTime.Now;
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
            DataTable table = new DataTable();
            DataSet Workbookset = new DataSet("Work Book");
            table.TableName = "MIS Report";
            Workbookset.Tables.Add(table);
            if (Campus == "All" || Campus == "" || Campus == null)
            {
                AllCampusMISReportExcel(Workbookset, CurrentAcademicYear, NextAcademicYear);
            }
            else
            {
                SingleMISReportExcel(Workbookset, Campus, CurrentAcademicYear);
            }

            return View();
        }
        private void SingleMISReportExcel(DataSet Workbookset, string Campus, string AcademicYear)
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

                ews.Cells["H9:I42"].Style.Font.Bold = true;
                //ews.Cells["C45:E42"].Style.Font.Bold = true;
                //ews.Cells["C45:E42"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
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
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=" + FileName + ".xlsx");
                byte[] File = Epkg.GetAsByteArray();
                Response.BinaryWrite(File);
                Response.End();
            }
        }
        public void AllCampusMISReportExcel(DataSet Workbookset, string CurrentAcademicYear, string NextAcademicYear)
        {
            //StudentsReportService SRS = new StudentsReportService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            List<CampusMaster> CampusList = new List<CampusMaster>();

            string sord = "";
            sord = sord == "desc" ? "Desc" : "Asc";
            string sidx = "Flag";

            Dictionary<long, IList<CampusMaster>> CampusMasterList = SRS.GetCampusMasterListWithPaging(0, 9999, sidx, sord, criteria);
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

                //Date for Filename attachment
                string Todaydate = DateTime.Now.ToString("dd/MM/yyyy");
                string FileName = "MISReportFor-AllCampus-On-" + Todaydate; ;
                //Write it back to the client
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=" + FileName + ".xlsx");
                byte[] File = Epkg.GetAsByteArray();
                Response.BinaryWrite(File);
                //email_send(File, FileName);
                Response.End();
            }
        }
        public ActionResult getCampusList()
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<long, IList<CampusMaster>> CampusMasterList = SRS.GetCampusMasterListWithPaging(0, 9999, string.Empty, string.Empty, criteria);
                if (CampusMasterList.First().Value.Count > 0 && CampusMasterList.Count > 0 && CampusMasterList != null)
                {
                    var CampusNames = (
                             from items in CampusMasterList.First().Value

                             select new
                             {
                                 Text = items.Name,
                                 Value = items.Name
                             }).Distinct().ToList();

                    return Json(CampusNames, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(null, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                throw ex;
            }
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
                throw ex;
            }
        }
        public string GetGradeShowName(int Flag)
        {
            try
            {
                string ShowGrade = "";
                if (Flag == 0)
                {
                    ShowGrade = "Playschool";
                }
                if (Flag == 1)
                {
                    ShowGrade = "PreKg";
                }
                if (Flag == 2)
                {
                    ShowGrade = "LKG";
                }
                if (Flag == 3)
                {
                    ShowGrade = "UKG";
                }
                if (Flag == 4)
                {
                    ShowGrade = "Grade-I";
                }
                if (Flag == 5)
                {
                    ShowGrade = "Grade-II";
                }
                if (Flag == 6)
                {
                    ShowGrade = "Grade-III";
                }
                if (Flag == 7)
                {
                    ShowGrade = "Grade-IV";
                }
                if (Flag == 8)
                {
                    ShowGrade = "Grade-V";
                }
                if (Flag == 9)
                {
                    ShowGrade = "Grade-VI";
                }
                if (Flag == 10)
                {
                    ShowGrade = "Grade-VII";
                }
                if (Flag == 11)
                {
                    ShowGrade = "Grade-VIII";
                }
                if (Flag == 12)
                {
                    ShowGrade = "Grade-IX";
                }
                if (Flag == 13)
                {
                    ShowGrade = "Grade-X";
                }
                if (Flag == 14)
                {
                    ShowGrade = "DP-1/Grade 11";
                }
                if (Flag == 15)
                {
                    ShowGrade = "DP-2/Grade 12";
                }
                return ShowGrade;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public IList<MISReport_vw> MISReportWithCriteria(string Campus, string Grade, string AcademicYear)
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Clear();
            HRManagementService HRMS = new HRManagementService();
            StudentsReportService SRS = new StudentsReportService();
            List<MISReport_vw> MISReportList = new List<MISReport_vw>();
            if (!string.IsNullOrEmpty(Campus)) criteria.Add("Campus", Campus);
            if (!string.IsNullOrEmpty(Grade)) criteria.Add("Grade", Grade);
            if (!string.IsNullOrEmpty(AcademicYear)) criteria.Add("AcademicYear", AcademicYear);
            Dictionary<long, IList<MISReport_vw>> MISReport_vwDetails = SRS.GetMISReport_vwListWithPaging(null, 9999, string.Empty, string.Empty, criteria);
            MISReportList = MISReport_vwDetails.FirstOrDefault().Value.ToList();
            return MISReportList;
        }
        public IList<MISOverAllReport_vw> GetOverAllReport(string Campus, string AcademicYear)
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Clear();
            StudentsReportService SRS = new StudentsReportService();
            List<MISOverAllReport_vw> OverAllList = new List<MISOverAllReport_vw>();
            if (!string.IsNullOrEmpty(Campus)) criteria.Add("Campus", Campus);
            if (!string.IsNullOrEmpty(AcademicYear)) criteria.Add("AcademicYear", AcademicYear);
            Dictionary<long, IList<MISOverAllReport_vw>> MISOverAllReport_vwDetails = SRS.GetMISOverAllReport_vwListWithPaging(0, 9999, string.Empty, string.Empty, criteria);
            OverAllList = MISOverAllReport_vwDetails.FirstOrDefault().Value.ToList();
            if (OverAllList != null && OverAllList.Count > 0)
            {
                return OverAllList;
            }
            else
            {
                return null;
            }
        }
        #region MIS MAIL MASTER
        public ActionResult MISMailMasterConfiguration()
        {

            return View();
        }
        public ActionResult MISMailMasterJqgrid(string EmailType, string Campus, string EmailId, string SentCategory, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Clear();
                    sord = sord == "desc" ? "Desc" : "Asc";
                    if (!string.IsNullOrWhiteSpace(EmailType)) { criteria.Add("EmailType", EmailType); }
                    if (!string.IsNullOrWhiteSpace(Campus)) { criteria.Add("Campus", Campus); }
                    if (!string.IsNullOrWhiteSpace(EmailId)) { criteria.Add("EmailId", EmailId); }
                    if (!string.IsNullOrWhiteSpace(SentCategory)) { criteria.Add("SentCategory", SentCategory); }
                    Dictionary<long, IList<MISMailMaster>> MISMailMasterList = null;
                    MISMailMasterList = SRS.GetMISMailMasterListWithPaging(0, 9999, sidx, sord, criteria);
                    if (MISMailMasterList != null && MISMailMasterList.FirstOrDefault().Value.Count > 0 && MISMailMasterList.FirstOrDefault().Key > 0)
                    {
                        long totalrecords = MISMailMasterList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)5);
                        var AssLst = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (
                                 from items in MISMailMasterList.First().Value

                                 select new
                                 {
                                     cell = new string[] 
                                         {
                                             items.Id.ToString(),
                                             items.EmailType,
                                             items.Campus,
                                             items.EmailId,
                                             items.SentCategory
                                         }
                                 }).ToList()
                        };

                        return Json(AssLst, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var jsondat = new { rows = (new { cell = new string[] { } }) };
                        return Json(jsondat, JsonRequestBehavior.AllowGet);

                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult GetRoleCodes()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    Dictionary<string, string> CampsCode = new Dictionary<string, string>();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<long, IList<CampusEmailId>> CampusList = SRS.GetCampusEmailId(0, 9999, null, null, criteria);
                    foreach (CampusEmailId Campus in CampusList.First().Value)
                    {
                        if (!string.IsNullOrWhiteSpace(Campus.Campus) && !CampsCode.ContainsKey(Campus.Campus))
                            CampsCode.Add(Campus.Campus, Campus.Campus);
                    }
                    return PartialView("Dropdown", CampsCode.Distinct());
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult GetSentCategory()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    Dictionary<string, string> SentCategory = new Dictionary<string, string>();
                    SentCategory.Add("Daily", "Daily");
                    SentCategory.Add("Weekly", "Weekly");
                    //SentCategory.Add("Monthly", "Monthly");
                    return PartialView("Dropdown", SentCategory.Distinct());
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult GetEmailType()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    Dictionary<string, string> SentCategory = new Dictionary<string, string>();
                    SentCategory.Add("MISReport", "MISReport");
                    SentCategory.Add("MISConsolidateReport", "MISConsolidateReport");
                    SentCategory.Add("AdmissionReport", "AdmissionReport");
                    //SentCategory.Add("Monthly", "Monthly");
                    return PartialView("Dropdown", SentCategory.Distinct());
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult AddMISMailMasterDetails(MISMailMaster MailMaster)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Clear();
                    MISMailMaster MIS = new MISMailMaster();
                    MIS.EmailType = MailMaster.EmailType;
                    MIS.Campus = MailMaster.Campus;
                    MIS.EmailId = MailMaster.EmailId;
                    MIS.SentCategory = MailMaster.SentCategory;
                    SRS.SaveOrUpdateMISMailMasterDetails(MIS);
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult EditMISMailMasterDetails(MISMailMaster MailMaster)
        {
            try
            {

                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (MailMaster.Id > 0)
                    {
                        MISMailMaster MIS = SRS.GetMISMailMasterDetailsById(MailMaster.Id);
                        MIS.EmailType = MailMaster.EmailType;
                        MIS.Campus = MailMaster.Campus;
                        MIS.EmailId = MailMaster.EmailId;
                        MIS.SentCategory = MailMaster.SentCategory;
                        SRS.SaveOrUpdateMISMailMasterDetails(MIS);
                    }
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult DeleteMISMailMasterDetails(string[] Id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    int i;
                    string[] arrayId = Id[0].Split(',');
                    for (i = 0; i < arrayId.Length; i++)
                    {
                        var singleId = arrayId[i];
                        MISMailMaster MIS = SRS.GetMISMailMasterDetailsById(Convert.ToInt64(singleId));
                        SRS.GetDeleteMISMailMasterById(MIS);
                    }
                    var script = @"SucessMsg(""Deleted Sucessfully"");";
                    return JavaScript(script);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        #endregion
        #endregion

    
        #region AdmissionStatusWiseReport
        public ActionResult AdmissionReport()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MastersService ms = new MastersService();
                    Dictionary<long, IList<AcademicyrMaster>> AcademicyrMaster = ms.GetAcademicyrMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, null);
                    ViewBag.acadddl = AcademicyrMaster.First().Value;
                    ViewBag.CurYear = DateTime.Now.Year.ToString() + '-' + Convert.ToString(DateTime.Now.Year + 1);
                    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult AddmissionStatusReportJqGrid(string Campus, string CurrAcadamicYear, int rows, string sidx, string sord, int? page = 1, int? ExptXl = 0)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                StudentsReportService SRS = new StudentsReportService();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (!string.IsNullOrEmpty(Campus))
                    criteria.Add("Campus", Campus.Trim());
                if (!string.IsNullOrEmpty(CurrAcadamicYear))
                    criteria.Add("AcademicYear", CurrAcadamicYear);
                Dictionary<long, IList<AdmissionStatusReport_vw>> AdmissionStatusList = SRS.GetAdmissionStatusListWithPagingAndCriteria(page - 1, rows, sord, sidx, criteria);
                if (AdmissionStatusList != null && AdmissionStatusList.Count > 0 && AdmissionStatusList.FirstOrDefault().Key > 0 && AdmissionStatusList.FirstOrDefault().Value.Count > 0)
                {
                    if (ExptXl == 1)
                    {
                        var List = AdmissionStatusList.First().Value.ToList();
                        base.ExptToXL(List, "AdmissionStatusReport", (items => new
                        {
                            items.Id,
                            items.Campus,
                            items.AcademicYear,
                            items.NewEnquiry,
                            items.NewRegistration,
                            items.SentForApproval,
                            items.SentForClearance,
                            items.Registered,
                            items.Discontinued,
                            items.NotInterested,
                        }));
                        return new EmptyResult();
                    }
                    else
                    {
                        long totalRecords = AdmissionStatusList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                        var jsonData = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalRecords,
                            rows = (
                                 from items in AdmissionStatusList.First().Value
                                 select new
                                 {
                                     i = items.Id,
                                     cell = new string[] { 
                                     items.Id.ToString(), 
                                     items.Campus,
                                     items.AcademicYear,
                                     items.NewEnquiry.ToString(),
                                     items.NewRegistration.ToString(),
                                     items.Registered.ToString(),
                                     items.Discontinued.ToString(),
                                     items.NotInterested.ToString(),
                                     }
                                 })
                        };
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        //public ActionResult GetAddmissionStatusReportChart(string Campus, string CurrAcadamicYear)
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //        else
        //        {
        //            StudentsReportService SRS = new StudentsReportService();
        //            Dictionary<string, object> Criteria = new Dictionary<string, object>();
        //            if (!string.IsNullOrEmpty(Campus))
        //                Criteria.Add("Campus", Campus.Trim());
        //            if (!string.IsNullOrEmpty(CurrAcadamicYear))
        //                Criteria.Add("AcademicYear", CurrAcadamicYear);
        //            Dictionary<long, IList<AdmissionStatusReport_vw>> AdmissionStatusList = SRS.GetAdmissionStatusListWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, Criteria);
        //            if (AdmissionStatusList != null && AdmissionStatusList.Count > 0 && AdmissionStatusList.FirstOrDefault().Key > 0 && AdmissionStatusList.FirstOrDefault().Value.Count > 0)
        //            {
        //                var StatusCount = (from u in AdmissionStatusList.First().Value
        //                                   select u).ToList();
        //                long NewEnquiry = 0; long NewRegistration = 0; long SentForApproval = 0; long Registered = 0; long Discontinued = 0;

        //                foreach (var itemdata in StatusCount)
        //                {
        //                    NewEnquiry = NewEnquiry + itemdata.NewEnquiry;
        //                    NewRegistration = NewRegistration + itemdata.NewRegistration;
        //                    SentForApproval = SentForApproval + itemdata.SentForApproval;
        //                    Registered = Registered + itemdata.Registered;
        //                    Discontinued = Discontinued + itemdata.Discontinued;

        //                }
        //                var AdmissionStatusCountChart = "<graph caption='' xAxisName='Admission Status' yAxisName='Student Count' decimalPrecision='0' formatNumberScale='0' showNames='1' rotateNames='1'>";
        //                AdmissionStatusCountChart = AdmissionStatusCountChart + " <set name='NewEnquiry' value='" + NewEnquiry + "' color='AFD8F8' />";
        //                AdmissionStatusCountChart = AdmissionStatusCountChart + " <set name='NewRegistration' value='" + NewRegistration + "' color='F6BD0F' />";
        //                AdmissionStatusCountChart = AdmissionStatusCountChart + " <set name='SentForApproval' value='" + SentForApproval + "' color='8BBA00' />";
        //                AdmissionStatusCountChart = AdmissionStatusCountChart + " <set name='Registered' value='" + Registered + "' color='FF8E46' />";
        //                AdmissionStatusCountChart = AdmissionStatusCountChart + " <set name='Discontinued' value='" + Discontinued + "' color='08E8E' /></graph>";
        //                Response.Write(AdmissionStatusCountChart);
        //                return null;
        //            }
        //            return null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
        //        throw ex;
        //    }
        //}
        public ActionResult GetAcademicYearWiseReportChart(string Campus)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    DateTime CurrentDate = DateTime.Now;
                    string[] CurAcadamicYear = new string[3];
                    int startYear = 0;
                    if (CurrentDate.Month > 5)
                        startYear = CurrentDate.Year;
                    else
                        startYear = CurrentDate.Year - 1;
                    //int Currnt_Year = Currnt_Date.Year;
                    for (int i = 0; i < 3; i++)
                    {
                        CurAcadamicYear[i] = startYear + "-" + ++startYear;
                    }
                    Dictionary<string, object> Criteria = new Dictionary<string, object>();
                    AdminTemplateService ATS = new AdminTemplateService();
                    if (!string.IsNullOrEmpty(Campus))
                        Criteria.Add("Campus", Campus.Trim());
                    if (CurAcadamicYear.Length > 0)
                        Criteria.Add("AcademicYear", CurAcadamicYear);
                    Dictionary<long, IList<AdmissionStatusReport_vw>> AdmissionStatusList = SRS.GetAdmissionStatusListWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, Criteria);
                    var MutiSeriesGraph = "<graph caption='Admission status wise with academis years' xAxisName='' decimalPrecision='0' forceDecimals='0' formatNumberScale='0' yAxisName='' animation='1' showNames='1' showValues='1' divlinecolor='d3d3d3' distance='5' angle='45' rotateNames='0'>";
                    MutiSeriesGraph = MutiSeriesGraph + "<categories>";
                    MutiSeriesGraph = MutiSeriesGraph + "<category name='" + CurAcadamicYear[0] + "'/>";
                    MutiSeriesGraph = MutiSeriesGraph + "<category name='" + CurAcadamicYear[1] + "' />";
                    MutiSeriesGraph = MutiSeriesGraph + "<category name='" + CurAcadamicYear[2] + "' />";
                    MutiSeriesGraph = MutiSeriesGraph + "</categories>";
                    if (AdmissionStatusList != null && AdmissionStatusList.FirstOrDefault().Key > 0 && AdmissionStatusList.FirstOrDefault().Value != null)
                    {
                        long firstACNewEnquiry = 0; long firstACNewRegistration = 0; long firstACRegistered = 0; long firstACNotInstrested = 0;
                        long secondACNewEnquiry = 0; long secondACNewRegistration = 0; long secondACRegistered = 0; long secondACNotInstrested = 0;
                        long thirdACNewEnquiry = 0; long thirdACNewRegistration = 0; long thirdACRegistered = 0; long thirdACNotInstrested = 0;
                        List<AdmissionStatusReport_vw> firstAcYear = (from u in AdmissionStatusList.FirstOrDefault().Value
                                                                      where u.AcademicYear == CurAcadamicYear[0]
                                                                      select u).ToList();
                        if (firstAcYear != null && firstAcYear.Count > 0)
                        {
                            foreach (var firstdata in firstAcYear)
                            {
                                firstACNewEnquiry = firstACNewEnquiry + firstdata.NewEnquiry;
                                firstACNewRegistration = firstACNewRegistration + firstdata.NewRegistration;
                                firstACRegistered = firstACRegistered + firstdata.Registered;
                                firstACNotInstrested = firstACNotInstrested + firstdata.NotInterested;
                            }
                        }

                        List<AdmissionStatusReport_vw> secondAcYear = (from u in AdmissionStatusList.FirstOrDefault().Value
                                                                       where u.AcademicYear == CurAcadamicYear[1]
                                                                       select u).ToList();
                        if (secondAcYear != null && secondAcYear.Count > 0)
                        {
                            foreach (var seconddata in secondAcYear)
                            {
                                secondACNewEnquiry = secondACNewEnquiry + seconddata.NewEnquiry;
                                secondACNewRegistration = secondACNewRegistration + seconddata.NewRegistration;
                                secondACRegistered = secondACRegistered + seconddata.Registered;
                                secondACNotInstrested = secondACNotInstrested + seconddata.NotInterested;
                            }
                        }
                        List<AdmissionStatusReport_vw> thirdAcYear = (from u in AdmissionStatusList.FirstOrDefault().Value
                                                                      where u.AcademicYear == CurAcadamicYear[2]
                                                                      select u).ToList();
                        if (thirdAcYear != null && thirdAcYear.Count > 0)
                        {
                            foreach (var thirddata in thirdAcYear)
                            {
                                thirdACNewEnquiry = thirdACNewEnquiry + thirddata.NewEnquiry;
                                thirdACNewRegistration = thirdACNewRegistration + thirddata.NewRegistration;
                                thirdACRegistered = thirdACRegistered + thirddata.Registered;
                                thirdACNotInstrested = thirdACNotInstrested + thirddata.NotInterested;
                            }
                        }

                        MutiSeriesGraph = MutiSeriesGraph + " <dataset seriesname='New Enquiry' color='008ee4'>";
                        MutiSeriesGraph = MutiSeriesGraph + "<set value='" + firstACNewEnquiry + "' />";
                        MutiSeriesGraph = MutiSeriesGraph + "<set value='" + secondACNewEnquiry + "' />";
                        MutiSeriesGraph = MutiSeriesGraph + "<set value='" + thirdACNewEnquiry + "' />";
                        MutiSeriesGraph = MutiSeriesGraph + "</dataset>";

                        MutiSeriesGraph = MutiSeriesGraph + " <dataset seriesname='New Registration' color='808000'>";
                        MutiSeriesGraph = MutiSeriesGraph + "<set value='" + firstACNewRegistration + "' />";
                        MutiSeriesGraph = MutiSeriesGraph + "<set value='" + secondACNewRegistration + "' />";
                        MutiSeriesGraph = MutiSeriesGraph + "<set value='" + thirdACNewRegistration + "' />";
                        MutiSeriesGraph = MutiSeriesGraph + "</dataset>";

                        MutiSeriesGraph = MutiSeriesGraph + " <dataset seriesname='Registered' color='808080'>";
                        MutiSeriesGraph = MutiSeriesGraph + "<set value='" + firstACRegistered + "' />";
                        MutiSeriesGraph = MutiSeriesGraph + "<set value='" + secondACRegistered + "' />";
                        MutiSeriesGraph = MutiSeriesGraph + "<set value='" + thirdACRegistered + "' />";
                        MutiSeriesGraph = MutiSeriesGraph + "</dataset>";

                        MutiSeriesGraph = MutiSeriesGraph + " <dataset seriesname='Not Interested' color='8ace80'>";
                        MutiSeriesGraph = MutiSeriesGraph + "<set value='" + firstACNotInstrested + "' />";
                        MutiSeriesGraph = MutiSeriesGraph + "<set value='" + secondACNotInstrested + "' />";
                        MutiSeriesGraph = MutiSeriesGraph + "<set value='" + thirdACNotInstrested + "' />";
                        MutiSeriesGraph = MutiSeriesGraph + "</dataset>";
                    }
                    MutiSeriesGraph = MutiSeriesGraph + "</graph>";
                    Response.Write(MutiSeriesGraph);
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        #endregion
        #region MIS Consolidate Report
        public ActionResult DateWiseConsolidateMISReport(string Excel)
        {
            DateTime Currnt_Date = DateTime.Now;
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
            string sord = "";
            sord = sord == "desc" ? "Desc" : "Asc";
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            List<CampusMaster> CampusList = new List<CampusMaster>();
            List<MISDateWiseReport_vw> DateWiseReportList = new List<MISDateWiseReport_vw>();
            Dictionary<long, IList<MISDateWiseReport_vw>> MISDateWiseReportList = null;
            criteria.Clear();
            criteria.Add("AcademicYear", AcademicYear);
            MISDateWiseReportList = SRS.GetMISDateWiseReport_vwListWithPaging(0, 9999, string.Empty, sord, criteria);
            DateWiseReportList = MISDateWiseReportList.FirstOrDefault().Value.ToList();
            criteria.Clear();
            string sidx = "Flag";
            Dictionary<long, IList<CampusMaster>> CampusMasterList = SRS.GetCampusMasterListWithPaging(0, 9999, sidx, sord, criteria);
            CampusList = CampusMasterList.FirstOrDefault().Value.ToList();

            var DirectList = (from u in MISDateWiseReportList.FirstOrDefault().Value
                              select u).ToList();
            List<MISDateWiseReport_vw> ExcelShowList = new List<MISDateWiseReport_vw>();

            //MISDateWiseReport_vw RawDataObject = new MISDateWiseReport_vw();
            //RawDataObject.DateString = "Date/Campus";
            //RawDataObject.Campus1 = CampusList[0].Name;
            //RawDataObject.Campus2 = CampusList[1].Name;
            //RawDataObject.Campus3 = CampusList[2].Name;
            //RawDataObject.Campus4 = CampusList[3].Name;
            //RawDataObject.Campus5 = CampusList[4].Name;
            //RawDataObject.Campus6 = CampusList[5].Name;
            //RawDataObject.Campus7 = CampusList[6].Name;
            //RawDataObject.Campus8 = CampusList[7].Name;
            //RawDataObject.Campus9 = CampusList[8].Name;
            //RawDataObject.Campus10 = CampusList[9].Name;
            //RawDataObject.Campus11 = CampusList[10].Name;
            //RawDataObject.Campus12 = CampusList[11].Name;
            //RawDataObject.Campus13 = CampusList[12].Name;
            //ExcelShowList.Add(RawDataObject);

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
                    //MyDateTime = DateTime.ParseExact(ThreeDaysBetweenDateItem, "dd/MM/yyyy", null);

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

            //Campus1BoysTotal = Campus1BoysTotal - Campus1BoysToddlerTotal;
            //Campus1GirlsTotal = Campus1GirlsTotal - Campus1GirlsToddlerTotal;
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


            //Excel = "AllCampusConsolidateReport";
            if (Excel == "AllCampusConsolidateReport")
            {
                DataTable table = new DataTable();
                DataSet Workbookset = new DataSet("Work Book");
                table.TableName = "MIS Report";
                Workbookset.Tables.Add(table);
                DateWiseMISREportExcel(Workbookset, AcademicYear, ExcelShowList, CampusMasterList, TotalList);
            }
            //ViewBag.ShowList = ShowList;
            //ViewBag.PreviousDate = ThreeDayBefore;
            //ViewBag.CurrentDate = CurrentDateStr;
            return View();
        }
        private void DateWiseMISREportExcel(DataSet Workbookset, string AcademicYear, List<MISDateWiseReport_vw> ShowList, Dictionary<long, IList<CampusMaster>> CampusMasterList, List<MISDateWiseReport_vw> TotalList)
        {
            try
            {
                using (ExcelPackage Epkg = new ExcelPackage())
                {
                    int TotalRow = ((ShowList.Count) + 2) + ShowList.Count;
                    if (CampusMasterList != null && CampusMasterList.FirstOrDefault().Key > 0 && CampusMasterList.FirstOrDefault().Value != null)
                    {
                        int TableCount = Workbookset.Tables.Count;
                        ExcelWorksheet ews = Epkg.Workbook.Worksheets.Add(Workbookset.Tables[0].TableName);
                        ews.View.ZoomScale = 100;
                        ews.View.ShowGridLines = false;
                        //ews.Cells["A1:CN2"].Style.Font.Name = "Calibri";
                        //ews.Cells["A1:CN2"].Style.Font.Size = 13;
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
                            if (count < ShowList.Count)
                            {
                                if (ShowList[j] != null)
                                {
                                    count++;
                                    ews.Cells["A" + i].Value = ShowList[j].DateString;
                                    //Campus1
                                    ews.Cells["B" + i].Value = ShowList[j].Campus1Boys;
                                    ews.Cells["C" + i].Value = ShowList[j].Campus1Girls;
                                    ews.Cells["D" + i].Value = ShowList[j].Campus1;
                                    //Campus2
                                    ews.Cells["F" + i].Value = ShowList[j].Campus2Boys;
                                    ews.Cells["G" + i].Value = ShowList[j].Campus2Girls;
                                    ews.Cells["H" + i].Value = ShowList[j].Campus2;
                                    //Campus3
                                    ews.Cells["J" + i].Value = ShowList[j].Campus3Boys;
                                    ews.Cells["K" + i].Value = ShowList[j].Campus3Girls;
                                    ews.Cells["L" + i].Value = ShowList[j].Campus3;
                                    //Campus4
                                    ews.Cells["N" + i].Value = ShowList[j].Campus4Boys;
                                    ews.Cells["O" + i].Value = ShowList[j].Campus4Girls;
                                    ews.Cells["P" + i].Value = ShowList[j].Campus4;
                                    //Campus5
                                    ews.Cells["R" + i].Value = ShowList[j].Campus5Boys;
                                    ews.Cells["S" + i].Value = ShowList[j].Campus5Girls;
                                    ews.Cells["T" + i].Value = ShowList[j].Campus5;
                                    //Campus6
                                    ews.Cells["V" + i].Value = ShowList[j].Campus6Boys;
                                    ews.Cells["W" + i].Value = ShowList[j].Campus6Girls;
                                    ews.Cells["X" + i].Value = ShowList[j].Campus6;
                                    //Campus7
                                    ews.Cells["Z" + i].Value = ShowList[j].Campus7Boys;
                                    ews.Cells["AA" + i].Value = ShowList[j].Campus7Girls;
                                    ews.Cells["AB" + i].Value = ShowList[j].Campus7;
                                    //Campus8
                                    ews.Cells["AD" + i].Value = ShowList[j].Campus8Boys;
                                    ews.Cells["AE" + i].Value = ShowList[j].Campus8Girls;
                                    ews.Cells["AF" + i].Value = ShowList[j].Campus8;
                                    //Campus9
                                    ews.Cells["AH" + i].Value = ShowList[j].Campus9Boys;
                                    ews.Cells["AI" + i].Value = ShowList[j].Campus9Girls;
                                    ews.Cells["AJ" + i].Value = ShowList[j].Campus9;
                                    //Campus10
                                    ews.Cells["AL" + i].Value = ShowList[j].Campus10Boys;
                                    ews.Cells["AM" + i].Value = ShowList[j].Campus10Girls;
                                    ews.Cells["AN" + i].Value = ShowList[j].Campus10;
                                    //Campus11
                                    ews.Cells["AP" + i].Value = ShowList[j].Campus11Boys;
                                    ews.Cells["AQ" + i].Value = ShowList[j].Campus11Girls;
                                    ews.Cells["AR" + i].Value = ShowList[j].Campus11;
                                    //Campus12
                                    ews.Cells["AT" + i].Value = ShowList[j].Campus12Boys;
                                    ews.Cells["AU" + i].Value = ShowList[j].Campus12Girls;
                                    ews.Cells["AV" + i].Value = ShowList[j].Campus12;
                                    //Campus13
                                    ews.Cells["AX" + i].Value = ShowList[j].Campus13Boys;
                                    ews.Cells["AY" + i].Value = ShowList[j].Campus13Girls;
                                    ews.Cells["AZ" + i].Value = ShowList[j].Campus13;
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
                    }
                    //Date for Filename attachment
                    string Todaydate = DateTime.Now.ToString("dd/MM/yyyy");
                    string FileName = "Consolidate-MIS-Report-On-" + Todaydate; ;
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;  filename=" + FileName + ".xlsx");
                    byte[] File = Epkg.GetAsByteArray();
                    Response.BinaryWrite(File);
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
            }
        }
        public ActionResult DateWiseConsolidateMISReportChart()
        {
            DateTime Currnt_Date = DateTime.Now;
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
            string sord = "";
            sord = sord == "desc" ? "Desc" : "Asc";
            //DateTime Currnt_Date = DateTime.Now;
            //int Currnt_Year = Currnt_Date.Year;
            string CurrentDateStr = Currnt_Date.ToString("dd/MM/yyyy");
            string OneDayBefore = Currnt_Date.AddDays(-1).ToString("dd/MM/yyyy");
            string TwoDayBefore = Currnt_Date.AddDays(-2).ToString("dd/MM/yyyy");
            string ThreeDayBefore = Currnt_Date.AddDays(-3).ToString("dd/MM/yyyy");

            Dictionary<string, object> criteria = new Dictionary<string, object>();
            List<CampusMaster> CampusList = new List<CampusMaster>();
            List<MISDateWiseReport_vw> DateWiseReportList = new List<MISDateWiseReport_vw>();
            Dictionary<long, IList<MISDateWiseReport_vw>> MISDateWiseReportList = null;
            criteria.Clear();
            criteria.Add("AcademicYear", AcademicYear);
            MISDateWiseReportList = SRS.GetMISDateWiseReport_vwListWithPaging(0, 9999, string.Empty, sord, criteria);
            DateWiseReportList = MISDateWiseReportList.FirstOrDefault().Value.ToList();
            criteria.Clear();
            string sidx = "Flag";
            Dictionary<long, IList<CampusMaster>> CampusMasterList = SRS.GetCampusMasterListWithPaging(0, 9999, sidx, sord, criteria);
            CampusList = CampusMasterList.FirstOrDefault().Value.ToList();
            List<MISDateWiseReport_vw> ShowList = new List<MISDateWiseReport_vw>();
            for (int i = 0; i < CampusList.Count; i++)
            {
                MISDateWiseReport_vw Obj = new MISDateWiseReport_vw();
                List<MISDateWiseReport_vw> ToList = new List<MISDateWiseReport_vw>();
                List<MISDateWiseReport_vw> PrevList = new List<MISDateWiseReport_vw>();
                Obj.Campus = CampusList[i].Name;
                var TodayList = (from u in MISDateWiseReportList.FirstOrDefault().Value
                                 where u.Campus == CampusList[i].Name
                                 select u).ToList();
                foreach (var item in TodayList)
                {
                    ToList.Add(item);
                    if (item.CreatedDate != CurrentDateStr && item.CreatedDate != OneDayBefore && item.CreatedDate != TwoDayBefore)
                    {
                        PrevList.Add(item);
                    }
                }
                Obj.PreviousTotal = 0;
                Obj.PreviousToddlerTotal = 0;
                if (PrevList != null && PrevList.Count > 0)
                {
                    for (int j = 0; j < PrevList.Count; j++)
                    {
                        if (PrevList[j].Grade == "PlaySchool") { Obj.PreviousToddlerTotal = Obj.PreviousToddlerTotal + PrevList[j].Total; }
                        else { Obj.PreviousTotal = Obj.PreviousTotal + Convert.ToInt32(PrevList[j].Total); }
                    }
                }
                Obj.CurrentTotal = 0;
                Obj.CurrentToddlerTotal = 0;
                if (ToList != null && ToList.Count > 0)
                {
                    for (int j = 0; j < ToList.Count; j++)
                    {
                        if (ToList[j].Grade == "PlaySchool") { Obj.CurrentToddlerTotal = Obj.CurrentToddlerTotal + ToList[j].Total; }
                        else { Obj.CurrentTotal = Obj.CurrentTotal + Convert.ToInt32(ToList[j].Total); }
                    }
                }
                ShowList.Add(Obj);
            }
            List<MISDateWiseReport_vw> MultiSeriesChartlist = new List<MISDateWiseReport_vw>();
            foreach (var items in ShowList)
            {
                //if (items.PreviousTotal != items.CurrentTotal) { Chartlist.Add(items); }
                items.PreviousTotal = items.PreviousTotal + items.PreviousToddlerTotal;
                items.CurrentTotal = items.CurrentTotal + items.CurrentToddlerTotal;
                if (items.PreviousTotal != items.CurrentTotal) { MultiSeriesChartlist.Add(items); }
            }
            var LineChart = "<graph caption='' xAxisName='Campus Name' yAxisName='Count' decimalPrecision='0' formatNumberScale='0' showNames='1' rotateNames='0' showLabels='1'>";
            if (MultiSeriesChartlist.Count >= 1)
            {
                LineChart = LineChart + "<categories showLabels='1'>";
                foreach (var ChartItem in MultiSeriesChartlist)
                {
                    //LineChart = LineChart + " <set name='" + ChartItem.Campus + "' label='" + ChartItem.Campus + "' value='" + ChartItem.PreviousTotal + "' color='AFD8F8'/>";
                    //LineChart = LineChart + " <set name='' value='" + ChartItem.CurrentTotal + "' color='336699'/>";
                    LineChart = LineChart + "<category label='" + ChartItem.Campus + "' />";
                }
                LineChart = LineChart + "</categories>";

                LineChart = LineChart + "<dataset seriesName='" + ThreeDayBefore + "' color='CC0066'>";
                foreach (var ChartItem in MultiSeriesChartlist)
                {
                    LineChart = LineChart + "<set value='" + ChartItem.PreviousTotal + "'color='CC0066' />";
                }
                LineChart = LineChart + "</dataset>";

                LineChart = LineChart + "<dataset seriesName='" + CurrentDateStr + "' color='FFCC66'>";
                foreach (var ChartItem in MultiSeriesChartlist)
                {
                    LineChart = LineChart + "<set name='" + ChartItem.Campus + "' value='" + ChartItem.CurrentTotal + "' color='FFCC66' />";
                }
                LineChart = LineChart + "</dataset>";
            }
            else
            {
                foreach (var ChartItem in ShowList)
                {
                    long Total = ChartItem.CurrentTotal + ChartItem.CurrentToddlerTotal;
                    LineChart = LineChart + " <set name='' value='" + Total + "' color='336699'/>";
                }
            }
            LineChart = LineChart + "</graph>";
            //LineChart = LineChart + "var chart = new FusionCharts('../../Charts/FCF_MSColumn2D.swf', 'Campus List', '700', '430');";

            Response.Write(LineChart);
            return null;
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
        #endregion
        #region DetailedAdmissionReport
        public ActionResult DetailedAdmissionReport()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult DetailedAdmissionReportJqGrid(string Campus, string AcademicYear, string Criteria, string ExportType, string FromDate, string ToDate, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    sord = sord == "desc" ? "Desc" : "Asc";
                    Dictionary<string, object> ExactCriteria = new Dictionary<string, object>();
                    Dictionary<string, object> LikeCriteria = new Dictionary<string, object>();

                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    DateTime FromDt = new DateTime();
                    DateTime ToDt = new DateTime();
                    if (!string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate))
                    {
                        FromDt = DateTime.ParseExact(FromDate, "dd/MM/yyyy", null);
                        ToDt = DateTime.ParseExact(ToDate, "dd/MM/yyyy", null);
                        string[] DateStringArr = GetDateStringArraList(FromDt, ToDt);
                        ExactCriteria.Add("CreatedDate", DateStringArr);
                    }
                    if (!string.IsNullOrEmpty(Campus)) { ExactCriteria.Add("Campus", Campus); }
                    if (!string.IsNullOrEmpty(AcademicYear)) { ExactCriteria.Add("AcademicYear", AcademicYear); }
                    Dictionary<long, IList<DetailedAdmissionReport_vw>> DetailedAdmissionReport_vwList = SRS.DetailedAdmissionReport_vwListWithLikeAndExcactSerachCriteria(0, 9999, sidx, sord, ExactCriteria, LikeCriteria);
                    List<DetailedAdmissionReport_vw> ShowList = new List<DetailedAdmissionReport_vw>();
                    if (string.IsNullOrEmpty(Campus)) { }
                    List<CampusMaster> CampusList = new List<CampusMaster>();
                    ExactCriteria.Clear();
                    sidx = "Flag";
                    Dictionary<long, IList<CampusMaster>> CampusMasterList = SRS.GetCampusMasterListWithPaging(0, 9999, sidx, sord, ExactCriteria);
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
                    if (ShowList != null && ShowList.Count > 0)
                    {
                        if (ExportType == "Excel")
                        {
                            var List = ShowList;
                            ExptToXL(List, "DetailedAdmissionReport", (items => new
                            {
                                Campus = items.Campus,
                                Declined = items.DeclinedCnt,
                                Deleted = items.DeletedCnt,
                                Discontinued = items.DiscontinuedCnt,
                                Inactive = items.InactiveCnt,
                                New_Enquiry = items.NewEnquiryCnt,
                                New_Registration = items.NewRegistrationCnt,
                                Not_Interested = items.NotInterestedCnt,
                                Not_Joined = items.NotJoinedCnt,
                                Registered = items.RegisteredCnt,
                                Sent_For_Approval = items.SentForApprovalCnt,
                                Sent_For_Clearance = items.SentForClearanceCnt
                            }));
                            return new EmptyResult();
                        }
                        else
                        {
                            long totalrecords = ShowList.Count;
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var AssLst = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalrecords,

                                rows = (
                                     from items in ShowList
                                     select new
                                     {
                                         cell = new string[] 
                                         {
                                             items.Id.ToString(),
                                             items.Campus,
                                             items.DeclinedCnt.ToString(),
                                             items.DeletedCnt.ToString(),
                                             items.DiscontinuedCnt.ToString(),
                                             items.InactiveCnt.ToString(),
                                             items.NewEnquiryCnt.ToString(),
                                             items.NewRegistrationCnt.ToString(),
                                             items.NotInterestedCnt.ToString(),
                                             items.NotJoinedCnt.ToString(),
                                             items.RegisteredCnt.ToString(),
                                             items.SentForApprovalCnt.ToString(),
                                             items.SentForClearanceCnt.ToString()
                                         }
                                     }).ToList()
                            };
                            return Json(AssLst, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        var CampusStatus = new { rows = (new { cell = new string[] { } }) };
                        return Json(CampusStatus, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult getCampus()
        {
            try
            {
                MastersService ms = new MastersService();
                Dictionary<string, object> ExactCriteria = new Dictionary<string, object>();
                string sord;
                sord = "Asc";
                string sidx = "Flag";
                Dictionary<long, IList<CampusMaster>> CampusMasterList = SRS.GetCampusMasterListWithPaging(0, 9999, sidx, sord, ExactCriteria);
                if (CampusMasterList != null && CampusMasterList.First().Value != null && CampusMasterList.First().Value.Count > 0)
                {
                    var CampusList = (
                             from items in CampusMasterList.First().Value
                             select new
                             {
                                 Text = items.Name,
                                 Value = items.Name
                             }).Distinct().ToList();
                    return Json(CampusList, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(null, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Check StudentReport Services
        public ActionResult CheckStudentsReportServices()
        {
            bool RetValue;
            //RetValue = SRS.SendEmailFromWindowsService("All");
            RetValue = SRS.SendMISConsolidateReport();
            //RetValue = SRS.SendAdmissionReport();
            return View();
        }
        #endregion

        #region past student reccords
        public ActionResult PastStudentRecords()
        {
            #region BreadCrumb
            string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
            #endregion
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            Dictionary<long, IList<CampusMaster>> Campus = MS.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            Dictionary<long, IList<AcademicyrMaster>> AcademicyrMaster = MS.GetAcademicyrMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            ViewBag.campusddl = Campus.First().Value;
            ViewBag.acadddl = AcademicyrMaster.First().Value;
            return View();
        }
        #endregion
        #region Past Students Grid
        public JsonResult PastStudentRecordGrid(string campus, string year, int rows, string sortBy, string sortType, Dictionary<string, object> searchCriteria, int? page = 1)
        {
            try
            {
                if (string.IsNullOrEmpty(campus) && string.IsNullOrEmpty(year))
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }

                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Campus", campus);
                criteria.Add("AcademicYear", year);
                Dictionary<long, IList<PastStudentList_Vw>> PastStudentRecords_Vw = SRS.GetPastStudentRecords_VwCountWithPagingAndCriteria(page - 1, rows, sortBy, sortType, criteria);

                if (PastStudentRecords_Vw != null && PastStudentRecords_Vw.FirstOrDefault().Value.Count > 0 && PastStudentRecords_Vw.FirstOrDefault().Key > 0)
                {
                    long totalrecords = PastStudentRecords_Vw.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)5);
                    var AssLst = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,
                        rows = (
                             from items in PastStudentRecords_Vw.First().Value

                             select new
                             {
                                 cell = new string[] 
                                         {
                                             items.Id.ToString(),
                                             items.Campus,
                                             items.AcademicYear,
                                             items.Grade,
                                             items.Boys.ToString(),
                                             items.Girls.ToString(),
                                             items.Total.ToString()
                                         }
                             }).ToList()
                    };

                    return Json(AssLst, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var jsondat = new { rows = (new { cell = new string[] { } }) };
                    return Json(jsondat, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        #endregion

        #region MISMonthlyReport
        public ActionResult MISMonthlyReport(string campus)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (campus == "All" || campus == null)
                {
                    AdmissionManagementService AMS = new AdmissionManagementService();
                    DateTime Currnt_Date = DateTime.Now;
                    int Currnt_Year = Currnt_Date.Year;
                    string CurrentAcademicYear = string.Empty;
                    string NextAcademicYear = string.Empty;
                    if (Currnt_Date.Month >= 6 && Currnt_Date.Month <= 12)
                    {
                        CurrentAcademicYear = Currnt_Year + "-" + (Currnt_Year + 1);
                        NextAcademicYear = (Currnt_Year + 1) + "-" + (Currnt_Year + 2);
                    }
                    if (Currnt_Date.Month >= 1 && Currnt_Date.Month <= 5)
                    {
                        CurrentAcademicYear = (Currnt_Year - 1) + "-" + Currnt_Year;
                        NextAcademicYear = (Currnt_Year) + "-" + (Currnt_Year + 1);
                    }
                    //Dictionary<string, object> criteria = new Dictionary<string, object>();
                    List<MISCampusReport> OverAllList = new List<MISCampusReport>();
                    IList<MISOverAllReport_vw> OverAllListVw = new List<MISOverAllReport_vw>();
                    criteria.Clear();
                    string sord = "";
                    sord = sord == "desc" ? "Desc" : "Asc";
                    string sidx = "Flag";
                    List<CampusMaster> CampusList = new List<CampusMaster>();
                    Dictionary<long, IList<CampusMaster>> CampusMasterList = SRS.GetCampusMasterListWithPaging(0, 9999, sidx, sord, criteria);
                    CampusList = CampusMasterList.FirstOrDefault().Value.ToList();
                    for (int i = 0; i < CampusList.Count; i++)
                    {
                        MISCampusReport CR = new MISCampusReport();
                        CR.Campus = CampusList[i].Name;
                        CR.Boys = 0;
                        CR.Girls = 0;
                        CR.Total = 0;
                        //CR.AcdYr = AcademicYear;
                        OverAllListVw = GetOverAllReport(CR.Campus, CurrentAcademicYear);
                        if (OverAllListVw != null)
                        {
                            CR.Boys = OverAllListVw.FirstOrDefault().Boys;
                            CR.Girls = OverAllListVw.FirstOrDefault().Girls;
                            CR.Total = OverAllListVw.FirstOrDefault().Total;
                        }
                        //CR.Total = CR.Total + CR.Total;
                        OverAllList.Add(CR);
                    }
                    ViewBag.OverAllList = OverAllList;
                    ViewBag.Pagename = "OverAll";
                    ViewBag.AcademicYear = CurrentAcademicYear;
                    Session["MISCampus"] = campus != null ? campus : "";
                    return View();
                }
                else
                {
                    List<MISCampusReport> SingleCampusList = new List<MISCampusReport>();
                    IList<MISReport_vw> ReportDetails = new List<MISReport_vw>();
                    DateTime Currnt_Date = DateTime.Now;
                    int Currnt_Year = Currnt_Date.Year;
                    string AcademicYear = "" + Currnt_Year + "-" + (Currnt_Year + 1) + "";
                    for (int i = 0; i < 16; i++)
                    {
                        MISCampusReport CR = new MISCampusReport();
                        CR.Boys = 0;
                        CR.Girls = 0;
                        CR.Total = 0;
                        CR.OverAllTotal = 0;
                        CR.Grade = GetGradeName(i);
                        CR.ShowGrade = GetGradeShowName(i);
                        //CR.AcdYr = AcademicYear;
                        criteria.Clear();
                        criteria.Add("Campus", campus);
                        criteria.Add("Grade", CR.Grade);
                        ReportDetails = MISReportWithCriteria(campus, CR.Grade, AcademicYear);
                        if (ReportDetails != null && ReportDetails.Count > 0)
                        {
                            CR.Boys = ReportDetails[0].Boys;
                            CR.Girls = ReportDetails[0].Girls;
                            CR.Total = ReportDetails[0].Total;
                        }
                        else
                        {
                            CR.Boys = 0;
                            CR.Girls = 0;
                            CR.Total = 0;
                        }
                        CR.OverAllTotal = CR.OverAllTotal + CR.Total;
                        SingleCampusList.Add(CR);
                    }
                    ViewBag.SingleCampusList = SingleCampusList;
                    ViewBag.Pagename = "Single";
                    ViewBag.AcademicYear = AcademicYear;
                    Session["MISCampus"] = campus != null ? campus : "";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult MISMonthlyReportWithExcel(string Campus, int Month)
        {
            DateTime Currnt_Date = DateTime.Now;
            int Currnt_Year = Currnt_Date.Year;
            string CurrentAcademicYear = string.Empty;
            string NextAcademicYear = string.Empty;
            if (Currnt_Date.Month >= 6 && Currnt_Date.Month <= 12)
            {
                CurrentAcademicYear = Currnt_Year + "-" + (Currnt_Year + 1);
                NextAcademicYear = (Currnt_Year + 1) + "-" + (Currnt_Year + 2);
            }
            if (Currnt_Date.Month >= 1 && Currnt_Date.Month <= 5)
            {
                CurrentAcademicYear = (Currnt_Year - 1) + "-" + Currnt_Year;
                NextAcademicYear = (Currnt_Year) + "-" + (Currnt_Year + 1);
            }
            DataTable table = new DataTable();
            DataSet Workbookset = new DataSet("Work Book");
            table.TableName = "MIS Report";
            Workbookset.Tables.Add(table);
            if (Campus == "All" || Campus == "" || Campus == null)
            {
                if (Month > 0 || Month != 0)
                {
                    AllCampusMISMonthlyReportExcel(Workbookset, CurrentAcademicYear, NextAcademicYear, Month);
                }
                else
                {

                    AllCampusMISReportExcel(Workbookset, CurrentAcademicYear, NextAcademicYear);
                }
            }
            else
            {
                if (Month > 0 || Month != 0)
                {
                    SingleMISMonthlyReportExcel(Workbookset, Campus, CurrentAcademicYear, NextAcademicYear, Month);
                }
                else
                {
                    SingleMISReportExcel(Workbookset, Campus, CurrentAcademicYear);
                }

            }

            return View();
        }
        private void SingleMISMonthlyReportExcel(DataSet Workbookset, string Campus, string CurrentAcademicYear, string NextAcademicYear, int Month)
        {
            IList<MISMonthlyReport_Vw> MISMonthlyReportDetails = new List<MISMonthlyReport_Vw>();
            //IList<MISReport_vw> MISReportDetails2 = new List<MISReport_vw>();
            using (ExcelPackage Epkg = new ExcelPackage())
            {

                int TableCount = Workbookset.Tables.Count;
                ExcelWorksheet ews = Epkg.Workbook.Worksheets.Add(Workbookset.Tables[0].TableName);
                ews.View.ZoomScale = 100;
                ews.View.ShowGridLines = false;
                //int img = 0;
                //ews.Row(img * 5).Height = 39.00D;
                ews.Cells["A1:BZ42"].Style.Font.Name = "Calibri";
                ews.Cells["A1:BZ50"].Style.Font.Size = 13;
                ews.Column(1).Width = 20;
                //For Title
                ews.Cells["A1:BZ1"].Merge = true;
                ews.Cells["A1:BZ1"].Value = "MIS REPORT";
                ews.Cells["A1:BZ1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ews.Cells["B2:BZ21"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
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
                //ews.Cells["B2:G2"].Merge = true;
                //ews.Cells["B2:G2"].Value = CampusList[0].ShowName;

                ////Current Academic Year
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
                for (j = 0; j <= 15; j++)
                {
                    Grade = GetGradeName(j);
                    MISMonthlyReportDetails = MISMonthlyReportWithCriteria(Campus, Grade, CurrentAcademicYear, Month);
                    if (MISMonthlyReportDetails.Count > 0)
                    {
                        ews.Cells["B" + i].Value = MISMonthlyReportDetails[0].Boys;
                        TotBoys = TotBoys + Convert.ToInt32(MISMonthlyReportDetails[0].Boys);
                        ews.Cells["C" + i].Value = MISMonthlyReportDetails[0].Girls;
                        TotGirls = TotGirls + Convert.ToInt32(MISMonthlyReportDetails[0].Girls);
                        ews.Cells["D" + i].Value = MISMonthlyReportDetails[0].Total;
                        CampusTotal = CampusTotal + Convert.ToInt32(MISMonthlyReportDetails[0].Total);
                        i = i + 1;
                    }
                    else
                    {
                        ews.Cells["B" + i].Value = "";
                        ews.Cells["C" + i].Value = "";
                        ews.Cells["D" + i].Value = "";
                        i = i + 1;
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
                for (j = 0; j <= 15; j++)
                {
                    Grade = GetGradeName(j);
                    MISMonthlyReportDetails = MISMonthlyReportWithCriteria(Campus, Grade, NextAcademicYear, Month);
                    if (MISMonthlyReportDetails.Count > 0)
                    {
                        ews.Cells["E" + i].Value = MISMonthlyReportDetails[0].Boys;
                        TotBoys = TotBoys + Convert.ToInt32(MISMonthlyReportDetails[0].Boys);
                        ews.Cells["F" + i].Value = MISMonthlyReportDetails[0].Girls;
                        TotGirls = TotGirls + Convert.ToInt32(MISMonthlyReportDetails[0].Girls);
                        ews.Cells["G" + i].Value = MISMonthlyReportDetails[0].Total;
                        CampusTotal = CampusTotal + Convert.ToInt32(MISMonthlyReportDetails[0].Total);
                        i = i + 1;
                    }
                    else
                    {
                        ews.Cells["E" + i].Value = "";
                        ews.Cells["F" + i].Value = "";
                        ews.Cells["G" + i].Value = "";
                        i = i + 1;
                    }

                }
                ews.Cells["E21"].Value = TotBoys;
                ews.Cells["F21"].Value = TotGirls;
                ews.Cells["G21"].Value = CampusTotal;
                #endregion
                ews.Cells["H2:H21"].Merge = true;
                ews.View.ShowGridLines = true;
                string Todaydate = DateTime.Now.ToString("dd/MM/yyyy");
                string FileName = "MISReportFor-AllCampus-On-" + Todaydate; ;
                //Write it back to the client
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=" + FileName + ".xlsx");
                byte[] File = Epkg.GetAsByteArray();
                Response.BinaryWrite(File);
                //email_send(File, FileName);
                Response.End();
            }
        }
        public void AllCampusMISMonthlyReportExcel(DataSet Workbookset, string CurrentAcademicYear, string NextAcademicYear, int Month)
        {
            //StudentsReportService SRS = new StudentsReportService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            List<CampusMaster> CampusList = new List<CampusMaster>();

            string sord = "";
            sord = sord == "desc" ? "Desc" : "Asc";
            string sidx = "Flag";

            Dictionary<long, IList<CampusMaster>> CampusMasterList = SRS.GetCampusMasterListWithPaging(0, 9999, sidx, sord, criteria);
            CampusList = CampusMasterList.FirstOrDefault().Value.ToList();

            IList<MISMonthlyReport_Vw> MISMonthlyReportDetails = new List<MISMonthlyReport_Vw>();
            //IList<MISReport_vw> MISReportDetails2 = new List<MISReport_vw>();
            using (ExcelPackage Epkg = new ExcelPackage())
            {
                int TableCount = Workbookset.Tables.Count;
                ExcelWorksheet ews = Epkg.Workbook.Worksheets.Add(Workbookset.Tables[0].TableName);
                ews.View.ZoomScale = 100;
                ews.View.ShowGridLines = false;
                //int img = 0;
                //ews.Row(img * 5).Height = 39.00D;
                ews.Cells["A1:BZ42"].Style.Font.Name = "Calibri";
                ews.Cells["A1:BZ50"].Style.Font.Size = 13;
                ews.Column(1).Width = 20;
                //For Title
                ews.Cells["A1:BZ1"].Merge = true;
                ews.Cells["A1:BZ1"].Value = "MIS REPORT";
                ews.Cells["A1:BZ1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ews.Cells["B2:BZ21"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
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
                for (j = 0; j <= 15; j++)
                {
                    Grade = GetGradeName(j);
                    MISMonthlyReportDetails = MISMonthlyReportWithCriteria(CampusList[0].Name, Grade, CurrentAcademicYear, Month);
                    if (MISMonthlyReportDetails.Count > 0)
                    {
                        ews.Cells["B" + i].Value = MISMonthlyReportDetails[0].Boys;
                        TotBoys = TotBoys + Convert.ToInt32(MISMonthlyReportDetails[0].Boys);
                        ews.Cells["C" + i].Value = MISMonthlyReportDetails[0].Girls;
                        TotGirls = TotGirls + Convert.ToInt32(MISMonthlyReportDetails[0].Girls);
                        ews.Cells["D" + i].Value = MISMonthlyReportDetails[0].Total;
                        CampusTotal = CampusTotal + Convert.ToInt32(MISMonthlyReportDetails[0].Total);
                        i = i + 1;
                    }
                    else
                    {
                        ews.Cells["B" + i].Value = "";
                        ews.Cells["C" + i].Value = "";
                        ews.Cells["D" + i].Value = "";
                        i = i + 1;
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
                for (j = 0; j <= 15; j++)
                {
                    Grade = GetGradeName(j);
                    MISMonthlyReportDetails = MISMonthlyReportWithCriteria(CampusList[0].Name, Grade, NextAcademicYear, Month);
                    if (MISMonthlyReportDetails.Count > 0)
                    {
                        ews.Cells["E" + i].Value = MISMonthlyReportDetails[0].Boys;
                        TotBoys = TotBoys + Convert.ToInt32(MISMonthlyReportDetails[0].Boys);
                        ews.Cells["F" + i].Value = MISMonthlyReportDetails[0].Girls;
                        TotGirls = TotGirls + Convert.ToInt32(MISMonthlyReportDetails[0].Girls);
                        ews.Cells["G" + i].Value = MISMonthlyReportDetails[0].Total;
                        CampusTotal = CampusTotal + Convert.ToInt32(MISMonthlyReportDetails[0].Total);
                        i = i + 1;
                    }
                    else
                    {
                        ews.Cells["E" + i].Value = "";
                        ews.Cells["F" + i].Value = "";
                        ews.Cells["G" + i].Value = "";
                        i = i + 1;
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
                for (j = 0; j <= 15; j++)
                {
                    Grade = GetGradeName(j);
                    MISMonthlyReportDetails = MISMonthlyReportWithCriteria(CampusList[1].Name, Grade, CurrentAcademicYear, Month);
                    if (MISMonthlyReportDetails.Count > 0)
                    {
                        ews.Cells["I" + i].Value = MISMonthlyReportDetails[0].Boys;
                        TotBoys = TotBoys + Convert.ToInt32(MISMonthlyReportDetails[0].Boys);
                        ews.Cells["J" + i].Value = MISMonthlyReportDetails[0].Girls;
                        TotGirls = TotGirls + Convert.ToInt32(MISMonthlyReportDetails[0].Girls);
                        ews.Cells["K" + i].Value = MISMonthlyReportDetails[0].Total;
                        CampusTotal = CampusTotal + Convert.ToInt32(MISMonthlyReportDetails[0].Total);
                        i = i + 1;
                    }
                    else
                    {
                        ews.Cells["I" + i].Value = "";
                        ews.Cells["J" + i].Value = "";
                        ews.Cells["K" + i].Value = "";
                        i = i + 1;
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
                for (j = 0; j <= 15; j++)
                {
                    Grade = GetGradeName(j);
                    MISMonthlyReportDetails = MISMonthlyReportWithCriteria(CampusList[1].Name, Grade, NextAcademicYear, Month);
                    if (MISMonthlyReportDetails.Count > 0)
                    {
                        ews.Cells["L" + i].Value = MISMonthlyReportDetails[0].Boys;
                        TotBoys = TotBoys + Convert.ToInt32(MISMonthlyReportDetails[0].Boys);
                        ews.Cells["M" + i].Value = MISMonthlyReportDetails[0].Girls;
                        TotGirls = TotGirls + Convert.ToInt32(MISMonthlyReportDetails[0].Girls);
                        ews.Cells["N" + i].Value = MISMonthlyReportDetails[0].Total;
                        CampusTotal = CampusTotal + Convert.ToInt32(MISMonthlyReportDetails[0].Total);
                        i = i + 1;
                    }
                    else
                    {
                        ews.Cells["L" + i].Value = "";
                        ews.Cells["M" + i].Value = "";
                        ews.Cells["N" + i].Value = "";
                        i = i + 1;
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
                for (j = 0; j <= 15; j++)
                {
                    Grade = GetGradeName(j);
                    MISMonthlyReportDetails = MISMonthlyReportWithCriteria(CampusList[2].Name, Grade, CurrentAcademicYear, Month);
                    if (MISMonthlyReportDetails.Count > 0)
                    {
                        ews.Cells["P" + i].Value = MISMonthlyReportDetails[0].Boys;
                        TotBoys = TotBoys + Convert.ToInt32(MISMonthlyReportDetails[0].Boys);
                        ews.Cells["Q" + i].Value = MISMonthlyReportDetails[0].Girls;
                        TotGirls = TotGirls + Convert.ToInt32(MISMonthlyReportDetails[0].Girls);
                        ews.Cells["R" + i].Value = MISMonthlyReportDetails[0].Total;
                        CampusTotal = CampusTotal + Convert.ToInt32(MISMonthlyReportDetails[0].Total);
                        i = i + 1;
                    }
                    else
                    {
                        ews.Cells["P" + i].Value = "";
                        ews.Cells["Q" + i].Value = "";
                        ews.Cells["R" + i].Value = "";
                        i = i + 1;
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
                for (j = 0; j <= 15; j++)
                {
                    Grade = GetGradeName(j);
                    MISMonthlyReportDetails = MISMonthlyReportWithCriteria(CampusList[2].Name, Grade, NextAcademicYear, Month);
                    if (MISMonthlyReportDetails.Count > 0)
                    {
                        ews.Cells["S" + i].Value = MISMonthlyReportDetails[0].Boys;
                        TotBoys = TotBoys + Convert.ToInt32(MISMonthlyReportDetails[0].Boys);
                        ews.Cells["T" + i].Value = MISMonthlyReportDetails[0].Girls;
                        TotGirls = TotGirls + Convert.ToInt32(MISMonthlyReportDetails[0].Girls);
                        ews.Cells["U" + i].Value = MISMonthlyReportDetails[0].Total;
                        CampusTotal = CampusTotal + Convert.ToInt32(MISMonthlyReportDetails[0].Total);
                        i = i + 1;
                    }
                    else
                    {
                        ews.Cells["S" + i].Value = "";
                        ews.Cells["T" + i].Value = "";
                        ews.Cells["U" + i].Value = "";
                        i = i + 1;
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
                for (j = 0; j <= 15; j++)
                {
                    Grade = GetGradeName(j);
                    MISMonthlyReportDetails = MISMonthlyReportWithCriteria(CampusList[3].Name, Grade, CurrentAcademicYear, Month);
                    if (MISMonthlyReportDetails.Count > 0)
                    {
                        ews.Cells["W" + i].Value = MISMonthlyReportDetails[0].Boys;
                        TotBoys = TotBoys + Convert.ToInt32(MISMonthlyReportDetails[0].Boys);
                        ews.Cells["X" + i].Value = MISMonthlyReportDetails[0].Girls;
                        TotGirls = TotGirls + Convert.ToInt32(MISMonthlyReportDetails[0].Girls);
                        ews.Cells["Y" + i].Value = MISMonthlyReportDetails[0].Total;
                        CampusTotal = CampusTotal + Convert.ToInt32(MISMonthlyReportDetails[0].Total);
                        i = i + 1;
                    }
                    else
                    {
                        ews.Cells["W" + i].Value = "";
                        ews.Cells["X" + i].Value = "";
                        ews.Cells["Y" + i].Value = "";
                        i = i + 1;
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
                for (j = 0; j <= 15; j++)
                {
                    Grade = GetGradeName(j);
                    MISMonthlyReportDetails = MISMonthlyReportWithCriteria(CampusList[3].Name, Grade, NextAcademicYear, Month);
                    if (MISMonthlyReportDetails.Count > 0)
                    {
                        ews.Cells["Z" + i].Value = MISMonthlyReportDetails[0].Boys;
                        TotBoys = TotBoys + Convert.ToInt32(MISMonthlyReportDetails[0].Boys);
                        ews.Cells["AA" + i].Value = MISMonthlyReportDetails[0].Girls;
                        TotGirls = TotGirls + Convert.ToInt32(MISMonthlyReportDetails[0].Girls);
                        ews.Cells["AB" + i].Value = MISMonthlyReportDetails[0].Total;
                        CampusTotal = CampusTotal + Convert.ToInt32(MISMonthlyReportDetails[0].Total);
                        i = i + 1;
                    }
                    else
                    {
                        ews.Cells["Z" + i].Value = "";
                        ews.Cells["AA" + i].Value = "";
                        ews.Cells["AB" + i].Value = "";
                        i = i + 1;
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
                for (j = 0; j <= 15; j++)
                {
                    Grade = GetGradeName(j);
                    MISMonthlyReportDetails = MISMonthlyReportWithCriteria(CampusList[4].Name, Grade, CurrentAcademicYear, Month);
                    if (MISMonthlyReportDetails.Count > 0)
                    {
                        ews.Cells["AD" + i].Value = MISMonthlyReportDetails[0].Boys;
                        TotBoys = TotBoys + Convert.ToInt32(MISMonthlyReportDetails[0].Boys);
                        ews.Cells["AE" + i].Value = MISMonthlyReportDetails[0].Girls;
                        TotGirls = TotGirls + Convert.ToInt32(MISMonthlyReportDetails[0].Girls);
                        ews.Cells["AF" + i].Value = MISMonthlyReportDetails[0].Total;
                        CampusTotal = CampusTotal + Convert.ToInt32(MISMonthlyReportDetails[0].Total);
                        i = i + 1;
                    }
                    else
                    {
                        ews.Cells["AD" + i].Value = "";
                        ews.Cells["AE" + i].Value = "";
                        ews.Cells["AF" + i].Value = "";
                        i = i + 1;
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
                for (j = 0; j <= 15; j++)
                {
                    Grade = GetGradeName(j);
                    MISMonthlyReportDetails = MISMonthlyReportWithCriteria(CampusList[4].Name, Grade, NextAcademicYear, Month);
                    if (MISMonthlyReportDetails.Count > 0)
                    {
                        ews.Cells["AG" + i].Value = MISMonthlyReportDetails[0].Boys;
                        TotBoys = TotBoys + Convert.ToInt32(MISMonthlyReportDetails[0].Boys);
                        ews.Cells["AH" + i].Value = MISMonthlyReportDetails[0].Girls;
                        TotGirls = TotGirls + Convert.ToInt32(MISMonthlyReportDetails[0].Girls);
                        ews.Cells["AI" + i].Value = MISMonthlyReportDetails[0].Total;
                        CampusTotal = CampusTotal + Convert.ToInt32(MISMonthlyReportDetails[0].Total);
                        i = i + 1;
                    }
                    else
                    {
                        ews.Cells["AG" + i].Value = "";
                        ews.Cells["AH" + i].Value = "";
                        ews.Cells["AI" + i].Value = "";
                        i = i + 1;
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
                for (j = 0; j <= 15; j++)
                {
                    Grade = GetGradeName(j);
                    MISMonthlyReportDetails = MISMonthlyReportWithCriteria(CampusList[5].Name, Grade, CurrentAcademicYear, Month);
                    if (MISMonthlyReportDetails.Count > 0)
                    {
                        ews.Cells["AK" + i].Value = MISMonthlyReportDetails[0].Boys;
                        TotBoys = TotBoys + Convert.ToInt32(MISMonthlyReportDetails[0].Boys);
                        ews.Cells["AL" + i].Value = MISMonthlyReportDetails[0].Girls;
                        TotGirls = TotGirls + Convert.ToInt32(MISMonthlyReportDetails[0].Girls);
                        ews.Cells["AM" + i].Value = MISMonthlyReportDetails[0].Total;
                        CampusTotal = CampusTotal + Convert.ToInt32(MISMonthlyReportDetails[0].Total);
                        i = i + 1;
                    }
                    else
                    {
                        ews.Cells["AK" + i].Value = "";
                        ews.Cells["AL" + i].Value = "";
                        ews.Cells["AM" + i].Value = "";
                        i = i + 1;
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
                for (j = 0; j <= 15; j++)
                {
                    Grade = GetGradeName(j);
                    MISMonthlyReportDetails = MISMonthlyReportWithCriteria(CampusList[5].Name, Grade, NextAcademicYear, Month);
                    if (MISMonthlyReportDetails.Count > 0)
                    {
                        ews.Cells["AN" + i].Value = MISMonthlyReportDetails[0].Boys;
                        TotBoys = TotBoys + Convert.ToInt32(MISMonthlyReportDetails[0].Boys);
                        ews.Cells["AO" + i].Value = MISMonthlyReportDetails[0].Girls;
                        TotGirls = TotGirls + Convert.ToInt32(MISMonthlyReportDetails[0].Girls);
                        ews.Cells["AP" + i].Value = MISMonthlyReportDetails[0].Total;
                        CampusTotal = CampusTotal + Convert.ToInt32(MISMonthlyReportDetails[0].Total);
                        i = i + 1;
                    }
                    else
                    {
                        ews.Cells["AN" + i].Value = "";
                        ews.Cells["AO" + i].Value = "";
                        ews.Cells["AP" + i].Value = "";
                        i = i + 1;
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
                for (j = 0; j <= 15; j++)
                {
                    Grade = GetGradeName(j);
                    MISMonthlyReportDetails = MISMonthlyReportWithCriteria(CampusList[6].Name, Grade, CurrentAcademicYear, Month);
                    if (MISMonthlyReportDetails.Count > 0)
                    {
                        ews.Cells["AR" + i].Value = MISMonthlyReportDetails[0].Boys;
                        TotBoys = TotBoys + Convert.ToInt32(MISMonthlyReportDetails[0].Boys);
                        ews.Cells["AS" + i].Value = MISMonthlyReportDetails[0].Girls;
                        TotGirls = TotGirls + Convert.ToInt32(MISMonthlyReportDetails[0].Girls);
                        ews.Cells["AT" + i].Value = MISMonthlyReportDetails[0].Total;
                        CampusTotal = CampusTotal + Convert.ToInt32(MISMonthlyReportDetails[0].Total);
                        i = i + 1;
                    }
                    else
                    {
                        ews.Cells["AR" + i].Value = "";
                        ews.Cells["AS" + i].Value = "";
                        ews.Cells["AT" + i].Value = "";
                        i = i + 1;
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
                for (j = 0; j <= 15; j++)
                {
                    Grade = GetGradeName(j);
                    MISMonthlyReportDetails = MISMonthlyReportWithCriteria(CampusList[6].Name, Grade, NextAcademicYear, Month);
                    if (MISMonthlyReportDetails.Count > 0)
                    {
                        ews.Cells["AU" + i].Value = MISMonthlyReportDetails[0].Boys;
                        TotBoys = TotBoys + Convert.ToInt32(MISMonthlyReportDetails[0].Boys);
                        ews.Cells["AV" + i].Value = MISMonthlyReportDetails[0].Girls;
                        TotGirls = TotGirls + Convert.ToInt32(MISMonthlyReportDetails[0].Girls);
                        ews.Cells["AW" + i].Value = MISMonthlyReportDetails[0].Total;
                        CampusTotal = CampusTotal + Convert.ToInt32(MISMonthlyReportDetails[0].Total);
                        i = i + 1;
                    }
                    else
                    {
                        ews.Cells["AU" + i].Value = "";
                        ews.Cells["AV" + i].Value = "";
                        ews.Cells["AW" + i].Value = "";
                        i = i + 1;
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
                for (j = 0; j <= 15; j++)
                {
                    Grade = GetGradeName(j);
                    MISMonthlyReportDetails = MISMonthlyReportWithCriteria(CampusList[7].Name, Grade, CurrentAcademicYear, Month);
                    if (MISMonthlyReportDetails.Count > 0)
                    {
                        ews.Cells["AY" + i].Value = MISMonthlyReportDetails[0].Boys;
                        TotBoys = TotBoys + Convert.ToInt32(MISMonthlyReportDetails[0].Boys);
                        ews.Cells["AZ" + i].Value = MISMonthlyReportDetails[0].Girls;
                        TotGirls = TotGirls + Convert.ToInt32(MISMonthlyReportDetails[0].Girls);
                        ews.Cells["BA" + i].Value = MISMonthlyReportDetails[0].Total;
                        CampusTotal = CampusTotal + Convert.ToInt32(MISMonthlyReportDetails[0].Total);
                        i = i + 1;
                    }
                    else
                    {
                        ews.Cells["AY" + i].Value = "";
                        ews.Cells["AZ" + i].Value = "";
                        ews.Cells["BA" + i].Value = "";
                        i = i + 1;
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
                for (j = 0; j <= 15; j++)
                {
                    Grade = GetGradeName(j);
                    MISMonthlyReportDetails = MISMonthlyReportWithCriteria(CampusList[7].Name, Grade, NextAcademicYear, Month);
                    if (MISMonthlyReportDetails.Count > 0)
                    {
                        ews.Cells["BB" + i].Value = MISMonthlyReportDetails[0].Boys;
                        TotBoys = TotBoys + Convert.ToInt32(MISMonthlyReportDetails[0].Boys);
                        ews.Cells["BC" + i].Value = MISMonthlyReportDetails[0].Girls;
                        TotGirls = TotGirls + Convert.ToInt32(MISMonthlyReportDetails[0].Girls);
                        ews.Cells["BD" + i].Value = MISMonthlyReportDetails[0].Total;
                        CampusTotal = CampusTotal + Convert.ToInt32(MISMonthlyReportDetails[0].Total);
                        i = i + 1;
                    }
                    else
                    {
                        ews.Cells["BB" + i].Value = "";
                        ews.Cells["BC" + i].Value = "";
                        ews.Cells["BD" + i].Value = "";
                        i = i + 1;
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
                for (j = 0; j <= 15; j++)
                {
                    Grade = GetGradeName(j);
                    MISMonthlyReportDetails = MISMonthlyReportWithCriteria(CampusList[8].Name, Grade, CurrentAcademicYear, Month);
                    if (MISMonthlyReportDetails.Count > 0)
                    {
                        ews.Cells["BF" + i].Value = MISMonthlyReportDetails[0].Boys;
                        TotBoys = TotBoys + Convert.ToInt32(MISMonthlyReportDetails[0].Boys);
                        ews.Cells["BG" + i].Value = MISMonthlyReportDetails[0].Girls;
                        TotGirls = TotGirls + Convert.ToInt32(MISMonthlyReportDetails[0].Girls);
                        ews.Cells["BH" + i].Value = MISMonthlyReportDetails[0].Total;
                        CampusTotal = CampusTotal + Convert.ToInt32(MISMonthlyReportDetails[0].Total);
                        i = i + 1;
                    }
                    else
                    {
                        ews.Cells["BF" + i].Value = "";
                        ews.Cells["BG" + i].Value = "";
                        ews.Cells["BH" + i].Value = "";
                        i = i + 1;
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
                for (j = 0; j <= 15; j++)
                {
                    Grade = GetGradeName(j);
                    MISMonthlyReportDetails = MISMonthlyReportWithCriteria(CampusList[8].Name, Grade, NextAcademicYear, Month);
                    if (MISMonthlyReportDetails.Count > 0)
                    {
                        ews.Cells["BI" + i].Value = MISMonthlyReportDetails[0].Boys;
                        TotBoys = TotBoys + Convert.ToInt32(MISMonthlyReportDetails[0].Boys);
                        ews.Cells["BJ" + i].Value = MISMonthlyReportDetails[0].Girls;
                        TotGirls = TotGirls + Convert.ToInt32(MISMonthlyReportDetails[0].Girls);
                        ews.Cells["BK" + i].Value = MISMonthlyReportDetails[0].Total;
                        CampusTotal = CampusTotal + Convert.ToInt32(MISMonthlyReportDetails[0].Total);
                        i = i + 1;
                    }
                    else
                    {
                        ews.Cells["BI" + i].Value = "";
                        ews.Cells["BJ" + i].Value = "";
                        ews.Cells["BK" + i].Value = "";
                        i = i + 1;
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
                for (j = 0; j <= 15; j++)
                {
                    Grade = GetGradeName(j);
                    MISMonthlyReportDetails = MISMonthlyReportWithCriteria(CampusList[9].Name, Grade, CurrentAcademicYear, Month);
                    if (MISMonthlyReportDetails.Count > 0)
                    {
                        ews.Cells["BM" + i].Value = MISMonthlyReportDetails[0].Boys;
                        TotBoys = TotBoys + Convert.ToInt32(MISMonthlyReportDetails[0].Boys);
                        ews.Cells["BN" + i].Value = MISMonthlyReportDetails[0].Girls;
                        TotGirls = TotGirls + Convert.ToInt32(MISMonthlyReportDetails[0].Girls);
                        ews.Cells["BO" + i].Value = MISMonthlyReportDetails[0].Total;
                        CampusTotal = CampusTotal + Convert.ToInt32(MISMonthlyReportDetails[0].Total);
                        i = i + 1;
                    }
                    else
                    {
                        ews.Cells["BM" + i].Value = "";
                        ews.Cells["BN" + i].Value = "";
                        ews.Cells["BO" + i].Value = "";
                        i = i + 1;
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
                for (j = 0; j <= 15; j++)
                {
                    Grade = GetGradeName(j);
                    MISMonthlyReportDetails = MISMonthlyReportWithCriteria(CampusList[9].Name, Grade, NextAcademicYear, Month);
                    if (MISMonthlyReportDetails.Count > 0)
                    {
                        ews.Cells["BP" + i].Value = MISMonthlyReportDetails[0].Boys;
                        TotBoys = TotBoys + Convert.ToInt32(MISMonthlyReportDetails[0].Boys);
                        ews.Cells["BQ" + i].Value = MISMonthlyReportDetails[0].Girls;
                        TotGirls = TotGirls + Convert.ToInt32(MISMonthlyReportDetails[0].Girls);
                        ews.Cells["BR" + i].Value = MISMonthlyReportDetails[0].Total;
                        CampusTotal = CampusTotal + Convert.ToInt32(MISMonthlyReportDetails[0].Total);
                        i = i + 1;
                    }
                    else
                    {
                        ews.Cells["BP" + i].Value = "";
                        ews.Cells["BQ" + i].Value = "";
                        ews.Cells["BR" + i].Value = "";
                        i = i + 1;
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
                for (j = 0; j <= 15; j++)
                {
                    Grade = GetGradeName(j);
                    MISMonthlyReportDetails = MISMonthlyReportWithCriteria(CampusList[10].Name, Grade, CurrentAcademicYear, Month);
                    if (MISMonthlyReportDetails.Count > 0)
                    {
                        ews.Cells["BT" + i].Value = MISMonthlyReportDetails[0].Boys;
                        TotBoys = TotBoys + Convert.ToInt32(MISMonthlyReportDetails[0].Boys);
                        ews.Cells["BU" + i].Value = MISMonthlyReportDetails[0].Girls;
                        TotGirls = TotGirls + Convert.ToInt32(MISMonthlyReportDetails[0].Girls);
                        ews.Cells["BV" + i].Value = MISMonthlyReportDetails[0].Total;
                        CampusTotal = CampusTotal + Convert.ToInt32(MISMonthlyReportDetails[0].Total);
                        i = i + 1;
                    }
                    else
                    {
                        ews.Cells["BT" + i].Value = "";
                        ews.Cells["BU" + i].Value = "";
                        ews.Cells["BV" + i].Value = "";
                        i = i + 1;
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
                for (j = 0; j <= 15; j++)
                {
                    Grade = GetGradeName(j);
                    MISMonthlyReportDetails = MISMonthlyReportWithCriteria(CampusList[10].Name, Grade, NextAcademicYear, Month);
                    if (MISMonthlyReportDetails.Count > 0)
                    {
                        ews.Cells["BW" + i].Value = MISMonthlyReportDetails[0].Boys;
                        TotBoys = TotBoys + Convert.ToInt32(MISMonthlyReportDetails[0].Boys);
                        ews.Cells["BX" + i].Value = MISMonthlyReportDetails[0].Girls;
                        TotGirls = TotGirls + Convert.ToInt32(MISMonthlyReportDetails[0].Girls);
                        ews.Cells["BY" + i].Value = MISMonthlyReportDetails[0].Total;
                        CampusTotal = CampusTotal + Convert.ToInt32(MISMonthlyReportDetails[0].Total);
                        i = i + 1;
                    }
                    else
                    {
                        ews.Cells["BW" + i].Value = "";
                        ews.Cells["BX" + i].Value = "";
                        ews.Cells["BY" + i].Value = "";
                        i = i + 1;
                    }
                }
                ews.Cells["BW21"].Value = TotBoys;
                ews.Cells["BX21"].Value = TotGirls;
                ews.Cells["BY21"].Value = CampusTotal;
                #endregion
                ews.Cells["BZ2:BZ21"].Merge = true;
                ews.View.ShowGridLines = true;

                //Date for Filename attachment
                string Todaydate = DateTime.Now.ToString("dd/MM/yyyy");
                string FileName = "MISReportFor-AllCampus-On-" + Todaydate; ;
                //Write it back to the client
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=" + FileName + ".xlsx");
                byte[] File = Epkg.GetAsByteArray();
                Response.BinaryWrite(File);
                //email_send(File, FileName);
                Response.End();
            }
        }

        #endregion
        public IList<MISMonthlyReport_Vw> MISMonthlyReportWithCriteria(string Campus, string Grade, string AcademicYear, int Month)
        {
            DateTime today = DateTime.Today;
            DateTime first = new DateTime(today.Year, Month, 1);
            DateTime temp = first.AddMonths(1);
            DateTime last = temp.AddDays(-1);
            DateTime[] fromto = new DateTime[2];
            fromto[0] = first;
            fromto[1] = last;
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Clear();
            HRManagementService HRMS = new HRManagementService();
            StudentsReportService SRS = new StudentsReportService();
            List<MISMonthlyReport_Vw> MISReportList = new List<MISMonthlyReport_Vw>();
            if (!string.IsNullOrEmpty(Campus)) criteria.Add("Campus", Campus);
            if (!string.IsNullOrEmpty(Grade)) criteria.Add("Grade", Grade);
            if (!string.IsNullOrEmpty(AcademicYear)) criteria.Add("AcademicYear", AcademicYear);
            criteria.Add("CreatedDateNew", fromto);
            Dictionary<long, IList<MISMonthlyReport_Vw>> MISMonthlyReport_vwDetails = SRS.GetMISMonthlyReport_vwListWithPaging(null, 9999, string.Empty, string.Empty, criteria);
            MISReportList = MISMonthlyReport_vwDetails.FirstOrDefault().Value.ToList();
            return MISReportList;
        }

        //#region Module Wise Usage Report by Prabakaran
        //#region CampusWiseUsageModule
        ////Get Method
        //public ActionResult CampusWiseUsageModule_vw()
        //{
        //    try
        //    {
        //        return View();
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "ReportsPolicy");
        //        throw ex;
        //    }
        //}
        ////JQGrid method
        //public ActionResult GetCampusWiseUsageModule_vwListJqGrid(CampusWiseUsageModule_vw campususagemodule, int rows, string sidx, string sord, int? page = 1)
        //{
        //    try
        //    {
        //        Dictionary<string, object> criteria = new Dictionary<string, object>();

        //        sord = sord == "desc" ? "Desc" : "Asc";
        //        if (!string.IsNullOrEmpty(campususagemodule.Module))
        //            criteria.Add("Module", campususagemodule.Module);
                
        //        Dictionary<long, IList<CampusWiseUsageModule_vw>> CampusWiseUsageModule_vw = null;

        //        CampusWiseUsageModule_vw = SRS.GetCampusWiseUsageModule_vwListWithPagingAndCriteria(page - 1, 9999, sidx, sord, criteria);
        //        if (CampusWiseUsageModule_vw == null || CampusWiseUsageModule_vw.FirstOrDefault().Key == 0)
        //        {
        //            var Empty = new { rows = (new { cell = new string[] { } }) };
        //            return Json(Empty, JsonRequestBehavior.AllowGet);
        //        }

        //        else
        //        {
        //            IList<CampusWiseUsageModule_vw> campusWiseUsageModule_vwList = CampusWiseUsageModule_vw.FirstOrDefault().Value;
        //            long totalRecords = CampusWiseUsageModule_vw.FirstOrDefault().Key;
        //            int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
        //            var jsonData = new
        //            {
        //                total = totalPages,
        //                page = page,
        //                records = totalRecords,
        //                rows =
        //                (
        //                from items in campusWiseUsageModule_vwList
        //                select new
        //                {
        //                    i = items.Id,
        //                    cell = new string[]
        //                   {
        //                        items.Id.ToString(),
        //                        items.Module,
        //                        items.IBMain==true?"Yes":"No",
        //                        items.IBKG==true?"Yes":"No",
        //                        items.ChennaiMain==true?"Yes":"No",
        //                        items.ChennaiCity==true?"Yes":"No",
        //                        items.Ernakulam==true?"Yes":"No",
        //                        items.ErnakulamKG==true?"Yes":"No",
        //                        items.Karur==true?"Yes":"No",
        //                        items.KarurKG==true?"Yes":"No",
        //                        items.Tirupur==true?"Yes":"No",
        //                        items.TirupurKG==true?"Yes":"No",
        //                        items.TipsSaran==true?"Yes":"No",
        //                        items.CreatedBy,
        //                        items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy"):"",
        //                        items.ModifiedBy,
        //                        items.ModifiedDate!=null?items.ModifiedDate.Value.ToString("dd/MM/yyyy"):""

        //                   }
        //                }
        //                )
        //            };
        //            return Json(jsonData, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "ReportsPolicy");
        //        throw ex;
        //    }
        //}
        //#endregion
        //#region SaveCampusWiseUsage
        //public ActionResult AddCampusWiseModuleRecords(CampusWiseUsageModule_vw obj)
        //{
        //    try
        //    {
        //        if (obj.Id > 0)
        //        {
        //            DeleteCampusWiseModuleRecords(obj);
        //            return null;
        //        }
        //        else
        //        {
        //            obj.CreatedBy = "Prabhakaran";
        //            obj.CreatedDate = DateTime.Now;
        //            obj.ModifiedBy = "Prabhakaran";
        //            obj.ModifiedDate = DateTime.Now;
        //            SRS.SaveOrUpdateCampusWiseUsageModule_vw(obj);
        //            return null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "ReportsPolicy");
        //        throw ex;
        //    }

        //}
        //#endregion
        //// #region edcampus
        ////public ActionResult EdCampusWiseModuleRecords(CampusWiseUsageModule_vw obj)
        ////{

        ////    obj.ModifiedBy = "Prabhakaran";
        ////    obj.ModifiedDate = DateTime.Now;
        ////    reportsSrvc.SaveOrUpdateCampusWiseUsageModule_vw(obj);
        ////    return null;

        ////}
        ////#endregion
        //#region UpdateCampusWiseUsage
        //public ActionResult EditCampusWiseModuleRecords(CampusWiseUsageModule_vw campusWiseusagemodule)
        //{
        //    CampusWiseUsageModule_vw campusWiseUsageModule_vw = SRS.GetCampusWiseUsageModuleById(campusWiseusagemodule.Id);
        //    if (!string.IsNullOrEmpty(campusWiseusagemodule.Module))
        //        campusWiseUsageModule_vw.Module = campusWiseusagemodule.Module;
        //    campusWiseUsageModule_vw.IBMain = campusWiseusagemodule.IBMain;
        //    campusWiseUsageModule_vw.IBKG = campusWiseusagemodule.IBKG;
        //    campusWiseUsageModule_vw.ChennaiMain = campusWiseusagemodule.ChennaiMain;
        //    campusWiseUsageModule_vw.ChennaiCity = campusWiseusagemodule.ChennaiCity;
        //    campusWiseUsageModule_vw.Ernakulam = campusWiseusagemodule.Ernakulam;
        //    campusWiseUsageModule_vw.ErnakulamKG = campusWiseusagemodule.ErnakulamKG;
        //    campusWiseUsageModule_vw.Karur = campusWiseusagemodule.Karur;
        //    campusWiseUsageModule_vw.KarurKG = campusWiseusagemodule.KarurKG;
        //    campusWiseUsageModule_vw.Tirupur = campusWiseusagemodule.Tirupur;
        //    campusWiseUsageModule_vw.TirupurKG = campusWiseusagemodule.TirupurKG;
        //    campusWiseUsageModule_vw.TipsSaran = campusWiseusagemodule.TipsSaran;
        //    campusWiseUsageModule_vw.ModifiedBy = "Prabakaran";
        //    campusWiseUsageModule_vw.ModifiedDate = DateTime.Now;
        //    SRS.SaveOrUpdatecampusWiseUsageModule_vw(campusWiseUsageModule_vw);
        //    return null;
        //}
        //#endregion
        //#region DeleteCampusWiseUsage
        //public ActionResult DeleteCampusWiseModuleRecords(CampusWiseUsageModule_vw campuswiseusagemodule)
        //{
        //    SRS.Deletecampuswiseusagemodule(campuswiseusagemodule);
        //    return null;
        //}

        //public ActionResult DeleteCampusWiseUsageModule_vw(string[] Id)
        //{
        //    try
        //    {
        //        int i;
        //        string[] arrayId = Id[0].Split(',');
        //        for (i = 0; i < arrayId.Length; i++)
        //        {
        //            var singleId = arrayId[i];
        //            CampusWiseUsageModule_vw CampusWiseUsageModule_vw = SRS.GetCampusWiseUsageModuleById(Convert.ToInt64(singleId));
        //            SRS.DeleteCampusWiseUsageModule_vw(CampusWiseUsageModule_vw.Id);
        //        }
        //        var script = @"SucessMsg(""Deleted Sucessfully"");";
        //        return JavaScript(script);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        //#endregion
        //#endregion

        //#region PageHistoryReport By Karthy
        //public ActionResult PageHistoryReport()
        //{
        //    try
        //    {
        //        return View();
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "ReportsPolicy");
        //        throw ex;
        //    }
        //}
        //public ActionResult GetPageHistoryReportListJqGrid(PageHistoryReport pageHistoryReport, int rows, string sidx, string sord, int? page = 1)
        //{
        //    try
        //    {
        //        Dictionary<string, object> criteria = new Dictionary<string, object>();
        //        sord = sord == "desc" ? "Desc" : "Asc";
        //        if (!string.IsNullOrEmpty(pageHistoryReport.Campus))
        //            criteria.Add("Campus", pageHistoryReport.Campus);
        //        if (!string.IsNullOrEmpty(pageHistoryReport.ControllerName))
        //            criteria.Add("ControllerName", pageHistoryReport.ControllerName);
        //        if ((pageHistoryReport.ControllerHit > 0))
        //            criteria.Add("ControllerHit", pageHistoryReport.ControllerHit);
        //        if (!string.IsNullOrEmpty(pageHistoryReport.ActionName))
        //            criteria.Add("ActionName", pageHistoryReport.ActionName);
        //        if ((pageHistoryReport.ActionHit > 0))
        //            criteria.Add("ActionHit", pageHistoryReport.ActionHit);

        //        Dictionary<long, IList<PageHistoryReport>> PageHistoryReport = null;
        //        PageHistoryReport = SRS.GetPageHistoryReportListWithPagingAndCriteria(page - 1, 9999, sidx, sord, criteria);
        //        if (PageHistoryReport == null || PageHistoryReport.FirstOrDefault().Key == 0)
        //        {
        //            var Empty = new { rows = (new { cell = new string[] { } }) };
        //            return Json(Empty, JsonRequestBehavior.AllowGet);
        //        }

        //        else
        //        {
        //            IList<PageHistoryReport> pageHistoryReportList = PageHistoryReport.FirstOrDefault().Value;
        //            long totalRecords = PageHistoryReport.FirstOrDefault().Key;
        //            int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
        //            var jsonData = new
        //            {
        //                total = totalPages,
        //                page = page,
        //                records = totalRecords,
        //                rows =
        //                (
        //                from items in pageHistoryReportList
        //                select new
        //                {
        //                    i = items.PageHistoryReport_Id,
        //                    cell = new string[]
        //                   {
        //                       items.PageHistoryReport_Id.ToString(),
        //                        items.Campus,
        //                        items.ControllerName,
        //                        items.ControllerHit.ToString(),
        //                        items.ActionName,
        //                        items.ActionHit.ToString(),
        //                        items.CreatedBy,
        //                        items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy"):"",
        //                        items.ModifiedBy,
        //                        items.ModifiedDate!=null?items.ModifiedDate.Value.ToString("dd/MM/yyyy"):""
        //                   }
        //                }
        //                )
        //            };
        //            return Json(jsonData, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "MasterPolicy");
        //        throw ex;
        //    }
        //}
        //#endregion

        #region CampusWiseModuleUsageReport
        public ActionResult CampusWiseModuleUsageReport()
        {
            //string userId = "prabakaran";
            string userId = base.ValidateUser();
            if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogIn", "Account");
            else
            {
                return View();
            }
        }
        public ActionResult GetCampusWiseModuleUsageReport_vwListJqGrid(string Module, string ExprtToExcel, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (!string.IsNullOrEmpty(Module))
                    criteria.Add("Module", Module);

                Dictionary<long, IList<CampusWiseModuleUsageReport_vw>> CampusWiseModuleUsageReport_vw = null;
                CampusWiseModuleUsageReport_vw = SRS.GetCampusWiseModuleUsageReport_vwListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (CampusWiseModuleUsageReport_vw == null || CampusWiseModuleUsageReport_vw.FirstOrDefault().Key == 0)
                {
                    var Empty = new { rows = (new { cell = new string[] { } }) };
                    return Json(Empty, JsonRequestBehavior.AllowGet);
                }

                else
                {
                    IList<CampusWiseModuleUsageReport_vw> campusWiseModuleUsageReportList = CampusWiseModuleUsageReport_vw.FirstOrDefault().Value;
                    if (ExprtToExcel == "Excel")
                    {
                        //Write xcel code here
                        StringBuilder sb = new StringBuilder();
                        sb.Append("<table border='" + "2px" + "'b>");
                        //write column headings
                        sb.Append("<tr>");
                        string[] Columns = new string[12];

                        Columns[0] = "Module";
                        Columns[1] = "IBMain";
                        Columns[2] = "IBKG";
                        Columns[3] = "ChennaiMain";
                        Columns[4] = "ChennaiCity";
                        Columns[5] = "Ernakulam";
                        Columns[6] = "ErnakulamKG";
                        Columns[7] = "Karur";
                        Columns[8] = "KarurKG";
                        Columns[9] = "Tirupur";
                        Columns[10] = "TirupurKG";
                        Columns[11] = "TipsSaran";
                        //Columns[1] = "MCS-ANTHIYUR";
                        //Columns[2] = "MHSS-ANTHIYUR";
                        //Columns[3] = "MMS-ANTHIYUR";
                        //Columns[4] = "MCOE-ANTHIYUR";
                        //Columns[5] = "MTTI-ANTHIYUR";
                        //Columns[6] = "RPS-KOTAGIRI";
                        for (int k = 0; k < Columns.Length; k++)
                        {
                            sb.Append("<td><b><font face=Arial size=2>" + Columns[k] + "</font></b></td>");
                        }
                        sb.Append("</tr>");
                        //write table data
                        for (int i = 0; i < campusWiseModuleUsageReportList.Count; i++)
                        {
                            sb.Append("<tr>");

                            Columns[0] = campusWiseModuleUsageReportList[i].Module;
                            //Columns[1] = campusWiseModuleUsageReportList[i].MCS_ANTHIYUR.ToString();
                            //Columns[2] = campusWiseModuleUsageReportList[i].MHSS_ANTHIYUR.ToString();
                            //Columns[3] = campusWiseModuleUsageReportList[i].MMS_ANTHIYUR.ToString();
                            //Columns[4] = campusWiseModuleUsageReportList[i].MCOE_ANTHIYUR.ToString();
                            //Columns[5] = campusWiseModuleUsageReportList[i].MTTI_ANTHIYUR.ToString();
                            //Columns[6] = campusWiseModuleUsageReportList[i].RPS_KOTAGIRI.ToString();
                            Columns[1] = campusWiseModuleUsageReportList[i].IBMain.ToString();
                            Columns[2] = campusWiseModuleUsageReportList[i].IBKG.ToString();
                            Columns[3] = campusWiseModuleUsageReportList[i].ChennaiMain.ToString();
                            Columns[4] = campusWiseModuleUsageReportList[i].ChennaiCity.ToString();
                            Columns[5] = campusWiseModuleUsageReportList[i].Ernakulam.ToString();
                            Columns[6] = campusWiseModuleUsageReportList[i].ErnakulamKG.ToString();
                            Columns[7] = campusWiseModuleUsageReportList[i].Karur.ToString();
                            Columns[8] = campusWiseModuleUsageReportList[i].KarurKG.ToString();
                            Columns[9] = campusWiseModuleUsageReportList[i].Tirupur.ToString();
                            Columns[10] = campusWiseModuleUsageReportList[i].TirupurKG.ToString();
                            Columns[11] = campusWiseModuleUsageReportList[i].TipsSaran.ToString();
                            for (int k = 0; k < Columns.Length; k++)
                            {
                                sb.Append("<td><font face=Arial size=" + "14px" + ">" + Columns[k] + "</font></td>");
                            }
                        }
                        sb.Append("</tr>");
                        sb.Append("</table>");
                        this.Response.AddHeader("Content-Disposition", "attachment;filename=CampusWiseModuleUsageReport.xls");
                        this.Response.ContentType = "application/vnd.ms-excel";
                        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
                        return File(buffer, "application/vnd.ms-excel");

                    }
                    else
                    {
                        IList<CampusWiseModuleUsageReport_vw> campusWiseModuleUsageReport_vwList = CampusWiseModuleUsageReport_vw.FirstOrDefault().Value;
                        long totalRecords = CampusWiseModuleUsageReport_vw.FirstOrDefault().Key;
                        int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                        var jsonData = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalRecords,
                            rows =
                            (
                            from items in campusWiseModuleUsageReport_vwList
                            select new
                            {
                                i = items.Id,
                                cell = new string[]
                           {
                                  items.Id.ToString(),
                                  items.Module,
                                //items.MCS_ANTHIYUR.ToString(),
                                //items.MHSS_ANTHIYUR.ToString(),
                                //items.MMS_ANTHIYUR.ToString(),
                                //items.MCOE_ANTHIYUR.ToString(),
                                //items.MTTI_ANTHIYUR.ToString(),
                                //items.RPS_KOTAGIRI.ToString()
                                
                                items.IBMain.ToString(),
                                items.IBKG.ToString(),
                                items.ChennaiMain.ToString(),
                                items.ChennaiCity.ToString(),
                                items.Ernakulam.ToString(),
                                items.ErnakulamKG.ToString(),
                                items.Karur.ToString(),
                                items.KarurKG.ToString(),
                                items.Tirupur.ToString(),
                                items.TirupurKG.ToString(),
                                items.TipsSaran.ToString()
                                
                   
                           }
                            }
                            )
                        };
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "ReportsPolicy");
                throw ex;
            }

        }
        #endregion

        #region CampusWiseUsageModule
        //Get Method
        //public ActionResult CampusWiseUsageModule()
        //{
        //    return View();
        //}
        //JQGrid method
        //public ActionResult GetCampusWiseUsageModuleListJqGrid(CampusWiseUsageModule campususagemodule, string Campus, string IsUsage, string ExprtToExcel, int rows, string sidx, string sord, int? page = 1)
        //{
        //    try
        //    {
        //        Dictionary<string, object> criteria = new Dictionary<string, object>();
        //        sord = sord == "desc" ? "Desc" : "Asc";


        //        Dictionary<long, IList<CampusWiseUsageModule>> CampusWiseUsageModule = null;
        //        CampusWiseUsageModule = reportsSrvc.GetCampusWiseUsageModuleListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
        //        if (CampusWiseUsageModule == null || CampusWiseUsageModule.FirstOrDefault().Key == 0)
        //        {
        //            var Empty = new { rows = (new { cell = new string[] { } }) };
        //            return Json(Empty, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            IList<CampusWiseUsageModule> campusWiseUsageModuleList = CampusWiseUsageModule.FirstOrDefault().Value;
        //            if (ExprtToExcel == "Excel")
        //            {
        //                //Write xcel code here
        //                StringBuilder sb = new StringBuilder();
        //                sb.Append("<table border='" + "2px" + "'b>");
        //                //write column headings
        //                sb.Append("<tr>");
        //                string[] Columns = new string[7];

        //                Columns[0] = "Module";
        //                //Columns[1] = "IBMain";
        //                //Columns[2] = "IBKG";
        //                //Columns[3] = "ChennaiMain";
        //                //Columns[4] = "ChennaiCity";
        //                //Columns[5] = "Ernakulam";
        //                //Columns[6] = "ErnakulamKG";
        //                //Columns[7] = "Karur";
        //                //Columns[8] = "KarurKG";
        //                //Columns[9] = "Tirupur";
        //                //Columns[10] = "TirupurKG";
        //                //Columns[11] = "TipsSaran";
        //                Columns[1] = "MCSANTHIYUR";
        //                Columns[2] = "MHSSANTHIYUR";
        //                Columns[3] = "MMSANTHIYUR";
        //                Columns[4] = "MCOEANTHIYUR";
        //                Columns[5] = "MTTIANTHIYUR";
        //                Columns[6] = "RPSKOTAGIRI";
        //                for (int k = 0; k < Columns.Length; k++)
        //                {
        //                    sb.Append("<td><b><font face=Arial size=2>" + Columns[k] + "</font></b></td>");
        //                }
        //                sb.Append("</tr>");
        //                //write table data
        //                for (int i = 0; i < campusWiseUsageModuleList.Count; i++)
        //                {
        //                    sb.Append("<tr>");

        //                    Columns[0] = campusWiseUsageModuleList[i].Module;
        //                    //Columns[1] = campusWiseUsageModuleList[i].IBMain == true ? "Yes" : "No";
        //                    //Columns[2] = campusWiseUsageModuleList[i].IBKG == true ? "Yes" : "No";
        //                    //Columns[3] = campusWiseUsageModuleList[i].ChennaiMain == true ? "Yes" : "No";
        //                    //Columns[4] = campusWiseUsageModuleList[i].ChennaiCity== true ? "Yes" : "No";
        //                    //Columns[5] = campusWiseUsageModuleList[i].Ernakulam== true ? "Yes" : "No";
        //                    //Columns[6] = campusWiseUsageModuleList[i].ErnakulamKG == true ? "Yes" : "No";
        //                    //Columns[7] = campusWiseUsageModuleList[i].Karur == true ? "Yes" : "No";
        //                    //Columns[8] = campusWiseUsageModuleList[i].KarurKG == true ? "Yes" : "No";
        //                    //Columns[9] = campusWiseUsageModuleList[i].Tirupur == true ? "Yes" : "No";
        //                    //Columns[10] = campusWiseUsageModuleList[i].TirupurKG == true ? "Yes" : "No";
        //                    //Columns[11] = campusWiseUsageModuleList[i].TipsSaran == true ? "Yes" : "No";
        //                    Columns[1] = campusWiseUsageModuleList[i].MCS_ANTHIYUR == true ? "Yes" : "No";
        //                    Columns[2] = campusWiseUsageModuleList[i].MHSS_ANTHIYUR == true ? "Yes" : "No";
        //                    Columns[3] = campusWiseUsageModuleList[i].MMS_ANTHIYUR == true ? "Yes" : "No";
        //                    Columns[4] = campusWiseUsageModuleList[i].MCOE_ANTHIYUR == true ? "Yes" : "No";
        //                    Columns[5] = campusWiseUsageModuleList[i].MTTI_ANTHIYUR == true ? "Yes" : "No";
        //                    Columns[6] = campusWiseUsageModuleList[i].RPS_KOTAGIRI == true ? "Yes" : "No";
        //                    for (int k = 0; k < Columns.Length; k++)
        //                    {
        //                        sb.Append("<td><font face=Arial size=" + "14px" + ">" + Columns[k] + "</font></td>");
        //                    }
        //                }
        //                sb.Append("</tr>");
        //                sb.Append("</table>");
        //                this.Response.AddHeader("Content-Disposition", "attachment;filename=CampusWiseUsageModule.xls");
        //                this.Response.ContentType = "application/vnd.ms-excel";
        //                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
        //                return File(buffer, "application/vnd.ms-excel");

        //            }

        //            else
        //            {
        //                IList<CampusWiseUsageModule> CampusWiseUsageModuleList = CampusWiseUsageModule.FirstOrDefault().Value;
        //                long totalRecords = CampusWiseUsageModule.FirstOrDefault().Key;
        //                int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
        //                var jsonData = new
        //                {
        //                    total = totalPages,
        //                    page = page,
        //                    records = totalRecords,
        //                    rows =
        //                    (
        //                    from items in CampusWiseUsageModuleList
        //                    select new
        //                    {
        //                        i = items.CampusWiseUsageModule_Id,
        //                        cell = new string[]
        //                   {
        //                        items.CampusWiseUsageModule_Id.ToString(),
        //                        items.Module,
        //                        items.IBMain==true?"Yes":"No",
        //                        items.IBKG==true?"Yes":"No",
        //                        items.ChennaiMain==true?"Yes":"No",
        //                        items.ChennaiCity==true?"Yes":"No",
        //                        items.Ernakulam==true?"Yes":"No",
        //                        items.ErnakulamKG==true?"Yes":"No",
        //                        items.Karur==true?"Yes":"No",
        //                        items.KarurKG==true?"Yes":"No",
        //                        items.Tirupur==true?"Yes":"No",
        //                        items.TirupurKG==true?"Yes":"No",
        //                        items.TipsSaran==true?"Yes":"No",
        //                        items.MCS_ANTHIYUR==true?"Yes":"No",
        //                        items.MHSS_ANTHIYUR==true?"Yes":"No",
        //                        items.MMS_ANTHIYUR==true?"Yes":"No",
        //                        items.MCOE_ANTHIYUR==true?"Yes":"No",
        //                        items.MTTI_ANTHIYUR==true?"Yes":"No",
        //                        items.RPS_KOTAGIRI==true?"Yes":"No",
        //                        items.CreatedBy,
        //                        items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy"):"",
        //                        items.ModifiedBy,
        //                        items.ModifiedDate!=null?items.ModifiedDate.Value.ToString("dd/MM/yyyy"):""

        //                   }
        //                    }
        //                    )
        //                };
        //                return Json(jsonData, JsonRequestBehavior.AllowGet);
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "ReportsPolicy");
        //        throw ex;
        //    }
        //}

        public ActionResult AddCampusWiseModuleRecords(CampusWiseUsageModule obj)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrEmpty(userId)) return RedirectToAction("LogIn", "Account");
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                MenuService menuservice = new MenuService();
                criteria.Add("MenuLevel", "Level1");
                Dictionary<long, IList<Menu>> Menu = menuservice.GetMenuListWithPagingAndCriteria(0, 9999, null, null, criteria);
                var MenuName = (from u in Menu.FirstOrDefault().Value where u.MenuName == obj.Module select new { u.MenuName }).ToList();
                if (MenuName.Count > 0)
                {
                    var MenuId = (from u in Menu.FirstOrDefault().Value where u.MenuName == obj.Module select u.Id).ToList();
                    if (obj.CampusWiseUsageModule_Id > 0)
                    {
                        EditCampusWiseModuleRecords(obj);
                    }
                    else
                    {
                        if (MenuId.Count > 0)
                        {
                            obj.CreatedBy = userId;
                            obj.CreatedDate = DateTime.Now;
                            obj.Menu_Id = MenuId[0];
                            SRS.SaveOrUpdateCampusWiseUsageModule(obj);

                        }

                    }
                    return null;
                }
                else
                {
                    var script = @"ErrMsg(""Select One From List"");";
                    return JavaScript(script);
                }

            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "ReportsPolicy");
                throw ex;
            }

        }



        public ActionResult EditCampusWiseModuleRecords(CampusWiseUsageModule campusWiseusagemodule)
        {
            try
            {

                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogIn", "Account");
                else
                {
                    CampusWiseUsageModule campusWiseUsageModule = SRS.GetCampusWiseUsageModuleById(campusWiseusagemodule.CampusWiseUsageModule_Id);
                    if (!string.IsNullOrEmpty(campusWiseusagemodule.Module))
                        campusWiseUsageModule.Module = campusWiseusagemodule.Module;
                    campusWiseUsageModule.IBMain = campusWiseusagemodule.IBMain;
                    campusWiseUsageModule.IBKG = campusWiseusagemodule.IBKG;
                    campusWiseUsageModule.ChennaiMain = campusWiseusagemodule.ChennaiMain;
                    campusWiseUsageModule.ChennaiCity = campusWiseusagemodule.ChennaiCity;
                    campusWiseUsageModule.Ernakulam = campusWiseusagemodule.Ernakulam;
                    campusWiseUsageModule.ErnakulamKG = campusWiseusagemodule.ErnakulamKG;
                    campusWiseUsageModule.Karur = campusWiseusagemodule.Karur;
                    campusWiseUsageModule.KarurKG = campusWiseusagemodule.KarurKG;
                    campusWiseUsageModule.Tirupur = campusWiseusagemodule.Tirupur;
                    campusWiseUsageModule.TirupurKG = campusWiseusagemodule.TirupurKG;
                    campusWiseUsageModule.TipsSaran = campusWiseusagemodule.TipsSaran;
                    //campusWiseUsageModule.MCS_ANTHIYUR = campusWiseusagemodule.MCS_ANTHIYUR;
                    //campusWiseUsageModule.MHSS_ANTHIYUR = campusWiseusagemodule.MHSS_ANTHIYUR;
                    //campusWiseUsageModule.MMS_ANTHIYUR = campusWiseusagemodule.MMS_ANTHIYUR;
                    //campusWiseUsageModule.MCOE_ANTHIYUR = campusWiseusagemodule.MCOE_ANTHIYUR;
                    //campusWiseUsageModule.MTTI_ANTHIYUR = campusWiseusagemodule.MTTI_ANTHIYUR;
                    //campusWiseUsageModule.RPS_KOTAGIRI = campusWiseusagemodule.RPS_KOTAGIRI;
                    campusWiseUsageModule.ModifiedBy = userId;
                    campusWiseUsageModule.ModifiedDate = DateTime.Now;
                    SRS.SaveOrUpdatecampusWiseUsageModule(campusWiseUsageModule);
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "ReportsPolicy");
                throw ex;
            }
        }


        public ActionResult DeleteCampusWiseUsageModule(string[] Id)
        {
            try
            {
                if (Id != null && Id.Length > 0)
                {
                    int i;
                    string[] arrayId = Id[0].Split(',');
                    for (i = 0; i < arrayId.Length; i++)
                    {
                        var singleId = arrayId[i];

                        long GetSingleId = Convert.ToInt64(singleId);
                        if (GetSingleId > 0)
                        {
                            CampusWiseUsageModule CampusWiseUsageModule = SRS.GetCampusWiseUsageModuleById(GetSingleId);
                            if (CampusWiseUsageModule != null)
                            {
                                SRS.DeleteCampusWiseUsageModule(CampusWiseUsageModule.CampusWiseUsageModule_Id);
                            }
                        }
                    }
                    var script = @"SucessMsg(""Deleted Sucessfully"");";
                    return JavaScript(script);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "ReportsPolicy");
                throw;
            }
        }
        public JsonResult ModuleAutoComplete(string term)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                MenuService menuservice = new MenuService();
                if (!string.IsNullOrEmpty(term))
                {
                    criteria.Add("MenuLevel", "Level1");
                    criteria.Add("MenuName", term);
                }
                Dictionary<long, IList<Menu>> Menu = menuservice.GetMenuListWithPagingAndCriteria(0, 9999, null, null, criteria);
                Dictionary<long, IList<CampusWiseUsageModule>> CampusWiseUsageModule = SRS.GetCampusWiseUsageModuleListWithPagingAndCriteria(0, 9999, null, null, null);
                var CampuWiseUsageModuleList = (from items in CampusWiseUsageModule.FirstOrDefault().Value
                                                select items.Module 
                                                ).Distinct().ToList();

                if (Menu != null && CampusWiseUsageModule != null && Menu.FirstOrDefault().Key >= 1)
                {
                    var MenuLevel = (from items in Menu.FirstOrDefault().Value select items.MenuName).Distinct().ToArray();
                    string[] AvailableMenu = MenuLevel.Except(CampuWiseUsageModuleList).ToArray();
                    if (AvailableMenu.Length > 0)
                    {
                    criteria.Clear();
                    criteria.Add("MenuLevel", "Level1");
                    criteria.Add("MenuName", AvailableMenu);
                    Menu = menuservice.GetMenuListWithPagingAndCriteria(0, 9999, null, null, criteria);
                    var Menus = (from items in Menu.FirstOrDefault().Value select new {MenuName = items.MenuName}).Distinct().ToArray();
                    return Json(Menus, JsonRequestBehavior.AllowGet);
                    }

                }
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "ReportsPolicy");
                throw;
            }
        }
        #endregion

        #region CampusWiseUsageModule_vw
        public ActionResult CampusWiseUsageModule()
        {
            return View();
        }
        //JQGrid method
        public ActionResult GetCampusWiseUsageModule_vwListJqGrid(string ExprtToExcel, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                sord = sord == "desc" ? "Desc" : "Asc";
                Dictionary<long, IList<CampusWiseUsageModule_vw>> CampusWiseUsageModule_vw = null;
                CampusWiseUsageModule_vw = SRS.GetCampusWiseUsageModule_vwListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (CampusWiseUsageModule_vw == null || CampusWiseUsageModule_vw.FirstOrDefault().Key == 0)
                {
                    var Empty = new { rows = (new { cell = new string[] { } }) };
                    return Json(Empty, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    IList<CampusWiseUsageModule_vw> CampusWiseUsageModule_vwList = CampusWiseUsageModule_vw.FirstOrDefault().Value;
                    if (ExprtToExcel == "Excel")
                    {
                        //Write xcel code here
                        StringBuilder sb = new StringBuilder();
                        sb.Append("<table border='" + "2px" + "'b>");
                        //write column headings
                        sb.Append("<tr>");
                        string[] Columns = new string[23];

                        Columns[0] = "Module";
                        Columns[1] = "IBMain_COUNT";
                        Columns[2] = "IBMain";
                        Columns[3] = "IBKG_COUNT";
                        Columns[4] = "IBKG";
                        Columns[5] = "ChennaiMain_COUNT";
                        Columns[6] = "ChennaiMain";
                        Columns[7] = "ChennaiCity_COUNT";
                        Columns[8] = "ChennaiCity";
                        Columns[9] = "Ernakulam_COUNT";
                        Columns[10] = "Ernakulam";
                        Columns[11] = "ErnakulamKG_COUNT";
                        Columns[12] = "ErnakulamKG";
                        Columns[13] = "Karur_COUNT";
                        Columns[14] = "Karur";
                        Columns[15] = "KarurKG_COUNT";
                        Columns[16] = "KarurKG";
                        Columns[17] = "Tirupur_COUNT";
                        Columns[18] = "Tirupur";
                        Columns[19] = "TirupurKG_COUNT";
                        Columns[20] = "TirupurKG";
                        Columns[21] = "TipsSaran_COUNT";
                        Columns[22] = "TipsSaran";
                        //Columns[1] = "MCSANTHIYUR_COUNT";
                        //Columns[2] = "MCSANTHIYUR";
                        //Columns[3] = "MHSSANTHIYUR_COUNT";
                        //Columns[4] = "MHSSANTHIYUR";
                        //Columns[5] = "MMSANTHIYUR_COUNT";
                        //Columns[6] = "MMSANTHIYUR";
                        //Columns[7] = "MCOEANTHIYUR_COUNT";
                        //Columns[8] = "MCOEANTHIYUR";
                        //Columns[9] = "MTTIANTHIYUR_COUNT";
                        //Columns[10] = "MTTIANTHIYUR";
                        //Columns[11] = "RPSKOTAGIRI_COUNT";
                        //Columns[12] = "RPSKOTAGIRI";
                        for (int k = 0; k < Columns.Length; k++)
                        {
                            sb.Append("<td><b><font face=Arial size=2>" + Columns[k] + "</font></b></td>");
                        }
                        sb.Append("</tr>");
                        //write table data
                        for (int i = 0; i < CampusWiseUsageModule_vwList.Count; i++)
                        {
                            sb.Append("<tr>");

                            Columns[0] = CampusWiseUsageModule_vwList[i].Module;
                            Columns[1] = CampusWiseUsageModule_vwList[i].IBMain_Count.ToString();
                            Columns[2] = CampusWiseUsageModule_vwList[i].IBMain == true ? "Yes" : "No";
                            Columns[3] = CampusWiseUsageModule_vwList[i].IBKG_Count.ToString();
                            Columns[4] = CampusWiseUsageModule_vwList[i].IBKG == true ? "Yes" : "No";
                            Columns[5] = CampusWiseUsageModule_vwList[i].ChennaiMain_Count.ToString();
                            Columns[6] = CampusWiseUsageModule_vwList[i].ChennaiMain == true ? "Yes" : "No";
                            Columns[7] = CampusWiseUsageModule_vwList[i].ChennaiCity_Count.ToString();
                            Columns[8] = CampusWiseUsageModule_vwList[i].ChennaiCity == true ? "Yes" : "No";
                            Columns[9] = CampusWiseUsageModule_vwList[i].Ernakulam_Count.ToString();
                            Columns[10] = CampusWiseUsageModule_vwList[i].Ernakulam == true ? "Yes" : "No";
                            Columns[11] = CampusWiseUsageModule_vwList[i].ErnakulamKG_Count.ToString();
                            Columns[12] = CampusWiseUsageModule_vwList[i].ErnakulamKG == true ? "Yes" : "No";
                            Columns[13] = CampusWiseUsageModule_vwList[i].Karur_Count.ToString();
                            Columns[14] = CampusWiseUsageModule_vwList[i].Karur == true ? "Yes" : "No";
                            Columns[15] = CampusWiseUsageModule_vwList[i].KarurKG_Count.ToString();
                            Columns[16] = CampusWiseUsageModule_vwList[i].KarurKG == true ? "Yes" : "No";
                            Columns[17] = CampusWiseUsageModule_vwList[i].Tirupur_Count.ToString();
                            Columns[18] = CampusWiseUsageModule_vwList[i].Tirupur == true ? "Yes" : "No";
                            Columns[19] = CampusWiseUsageModule_vwList[i].TirupurKG_Count.ToString();
                            Columns[20] = CampusWiseUsageModule_vwList[i].TirupurKG == true ? "Yes" : "No";
                            Columns[21] = CampusWiseUsageModule_vwList[i].TipsSaran_Count.ToString();
                            Columns[22] = CampusWiseUsageModule_vwList[i].TipsSaran == true ? "Yes" : "No";
                            //Columns[1] = CampusWiseUsageModule_vwList[i].MCS_ANTHIYUR_Count.ToString();
                            //Columns[2] = CampusWiseUsageModule_vwList[i].MCS_ANTHIYUR == true ? "Yes" : "No";
                            //Columns[3] = CampusWiseUsageModule_vwList[i].MHSS_ANTHIYUR_Count.ToString();
                            //Columns[4] = CampusWiseUsageModule_vwList[i].MHSS_ANTHIYUR == true ? "Yes" : "No";
                            //Columns[5] = CampusWiseUsageModule_vwList[i].MMS_ANTHIYUR_Count.ToString();
                            //Columns[6] = CampusWiseUsageModule_vwList[i].MMS_ANTHIYUR == true ? "Yes" : "No";
                            //Columns[7] = CampusWiseUsageModule_vwList[i].MCOE_ANTHIYUR_Count.ToString();
                            //Columns[8] = CampusWiseUsageModule_vwList[i].MCOE_ANTHIYUR == true ? "Yes" : "No";
                            //Columns[9] = CampusWiseUsageModule_vwList[i].MTTI_ANTHIYUR_Count.ToString();
                            //Columns[10] = CampusWiseUsageModule_vwList[i].MTTI_ANTHIYUR == true ? "Yes" : "No";
                            //Columns[11] = CampusWiseUsageModule_vwList[i].RPS_KOTAGIRI_Count.ToString();
                            //Columns[12] = CampusWiseUsageModule_vwList[i].RPS_KOTAGIRI == true ? "Yes" : "No";
                            for (int k = 0; k < Columns.Length; k++)
                            {
                                sb.Append("<td><font face=Arial size=" + "14px" + ">" + Columns[k] + "</font></td>");
                            }
                        }
                        sb.Append("</tr>");
                        sb.Append("</table>");
                        this.Response.AddHeader("Content-Disposition", "attachment;filename=CampusWiseLiveModulesWithCountReportList.xls");
                        this.Response.ContentType = "application/vnd.ms-excel";
                        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
                        return File(buffer, "application/vnd.ms-excel");

                    }

                    else
                    {
                        IList<CampusWiseUsageModule_vw> campusWiseUsageModule_vwList = CampusWiseUsageModule_vw.FirstOrDefault().Value;
                        long totalRecords = CampusWiseUsageModule_vw.FirstOrDefault().Key;
                        int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                        var jsonData = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalRecords,
                            rows =
                            (
                            from items in campusWiseUsageModule_vwList
                            select new
                            {
                                i = items.Id,
                                cell = new string[]
                           {
                                items.Id.ToString(),
                                items.CampusWiseUsageModule_Id.ToString(),
                                items.Module,
                                items.IBMain_Count.ToString(),
                                items.IBMain==true?"Yes":"No",
                                items.IBKG_Count.ToString(),
                                items.IBKG==true?"Yes":"No",
                                items.ChennaiMain_Count.ToString(),
                                items.ChennaiMain==true?"Yes":"No",
                                items.ChennaiCity_Count.ToString(),
                                items.ChennaiCity==true?"Yes":"No",
                                items.Ernakulam_Count.ToString(),
                                items.Ernakulam==true?"Yes":"No",
                                items.ErnakulamKG_Count.ToString(),
                                items.ErnakulamKG==true?"Yes":"No",
                                items.Karur_Count.ToString(),
                                items.Karur==true?"Yes":"No",
                                items.KarurKG_Count.ToString(),
                                items.KarurKG==true?"Yes":"No",
                                items.Tirupur_Count.ToString(),
                                items.Tirupur==true?"Yes":"No",
                                items.TirupurKG_Count.ToString(),
                                items.TirupurKG==true?"Yes":"No",
                                items.TipsSaran_Count.ToString(),
                                items.TipsSaran==true?"Yes":"No",
                                //items.MCS_ANTHIYUR_Count.ToString(),
                                //items.MCS_ANTHIYUR==true?"Yes":"No",
                                //items.MHSS_ANTHIYUR_Count.ToString(),
                                //items.MHSS_ANTHIYUR==true?"Yes":"No",
                                //items.MMS_ANTHIYUR_Count.ToString(),
                                //items.MMS_ANTHIYUR==true?"Yes":"No",
                                //items.MCOE_ANTHIYUR_Count.ToString(),
                                //items.MCOE_ANTHIYUR==true?"Yes":"No",
                                //items.MTTI_ANTHIYUR_Count.ToString(),
                                //items.MTTI_ANTHIYUR==true?"Yes":"No",
                                //items.RPS_KOTAGIRI_Count.ToString(),
                                //items.RPS_KOTAGIRI==true?"Yes":"No",
                               

                           }
                            }
                            )
                        };
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                    }

                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "ReportsPolicy");
                throw ex;
            }
        }
        #endregion
        
    }
}