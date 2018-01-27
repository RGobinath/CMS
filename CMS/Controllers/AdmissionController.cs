using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CustomAuthentication;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using ThoughtWorks.QRCode.Codec;
using TIPS.Entities;
using TIPS.Entities.EnquiryEntities;
using TIPS.Entities.AdmissionEntities;
using TIPS.Entities.Assess;
using TIPS.Service;
using TIPS.ServiceContract;
using TIPS.Entities.CommunictionEntities;
using TIPS.Entities.Assess.ReportCardClasses;
using CMS.Helpers;
using TIPS.Component;
using TIPS.Entities.StudentsReportEntities;
using TIPS.Entities.InboxEntities;
using System.Text;

//using OnBarcode.Barcode;
//using CMS.CountryService;

namespace CMS.Controllers
{
    public class AdmissionController : BaseController
    {
        AdmissionManagementService AMC = new AdmissionManagementService();
        MastersService ms = new MastersService();
        UserService us = new UserService();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AdmissionManagement(string resetsession, string pagename)
        {
            try
            {
                if (resetsession != "no")
                {
                    ResetSession("");
                }
                if (pagename != null)
                {
                    Session["pagename"] = pagename.ToString();
                }
                MastersService ms = new MastersService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();

                var usrcmp = Session["UserCampus"] as IEnumerable<string>;
                if (usrcmp != null && usrcmp.Count() != 0)
                {
                    if (usrcmp.First() != null)            // to check if the usrcmp obj is null or with data
                    {
                        criteria.Add("Name", usrcmp);
                    }
                }
                Dictionary<long, IList<CampusMaster>> CampusMaster = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                criteria.Clear();
                Dictionary<long, IList<GradeMaster>> GradeMaster = ms.GetGradeMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                Dictionary<long, IList<AcademicyrMaster>> AcademicyrMaster = ms.GetAcademicyrMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);

                ViewBag.campusddl = CampusMaster.First().Value;
                ViewBag.gradeddl = GradeMaster.First().Value;
                ViewBag.acadddl = AcademicyrMaster.First().Value;
                if (Session["registered"] == "yes")
                {
                    ViewBag.Registered = "yes";
                    ViewBag.RegId = Session["regid"];  // regid;
                    Session["registered"] = "";
                    Session["regid"] = "";
                }
                Session["editid"] = 0;
                Session["Time"] = DateTime.Now.ToString("T");
                string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                return View();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public JsonResult AdmissionManagementListJqGrid(string Id, string Gender, string grade, string section, string acadyr, string feestructyr, string appname, string admstat, string appno, string idnum, string preregnumber, string ishosteller, string flag, string flag1, string reset, string stdntmgmt, string AppDateFrm, string AppDateTo, int rows, string sidx, string sord, int? page = 1)
        {
            string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
            try
            {
                string searchedItems = Id + "," + grade + "," + Gender + "," + acadyr + "," + appname + "," + appno + "," + admstat + "," + preregnumber + "," + AppDateFrm + "," + AppDateTo;
                Session["AdmissionSearched"] = searchedItems;
                Session["emailsent"] = "";
                sord = sord == "desc" ? "Desc" : "Asc";
                AdmissionManagementService ads = new AdmissionManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                string colName = string.Empty; string[] values = new string[1];
                var usrcmp = Session["UserCampus"] as IEnumerable<string>;
                if (usrcmp.Count() != 0)
                {
                    if (usrcmp.First() != null)            // to check if the usrcmp obj is null or with data
                    {
                        if (reset != "yes")
                        {
                            if (!string.IsNullOrWhiteSpace(Id) && usrcmp.Contains(Id)) { criteria.Add("Campus", Id); }
                            else if (!string.IsNullOrWhiteSpace(Id) && !usrcmp.Contains(Id)) { criteria.Add("Campus", "no campus"); }
                            else
                            { criteria.Add("Campus", usrcmp); }
                        }
                        else
                        { criteria.Add("Campus", usrcmp); }
                    }
                }
                else { criteria.Add("Campus", "no campus"); }

                #region reset
                if (reset != "yes")
                {
                    if (!string.IsNullOrWhiteSpace(grade) || (Session["grd"].ToString() != ""))
                    {
                        var test = grade.Split(',');
                        string[] grds = new string[test.Length];
                        int gr = 0;
                        foreach (string val in test)
                        {
                            grds[gr] = val;
                            gr++;
                        }
                        if (grade != "")
                        {
                            Session["grd"] = grds;
                        }
                        colName = "Grade";
                        values = grds;     //  values[0] = Session["grd"].ToString();
                    }
                    if (!string.IsNullOrWhiteSpace(Gender))
                    {
                        //var Gndr = Gender.Split(',');
                        //string[] Gndrs = new string[Gndr.Length];
                        //int gnr = 0;
                        //foreach (var item in Gndr)
                        //{
                        //    Gndrs[gnr] = item;
                        //    gnr++;
                        //}

                        //if (Gender != "")
                        //{
                        //    Session["gndr"] = Gndrs;
                        //}
                        //colName = "Gender";
                        //values = Gndrs;
                        criteria.Add("Gender", Gender);
                    }
                    if (!string.IsNullOrWhiteSpace(section) || (Session["sect"].ToString() != ""))
                    {
                        if (section != "")
                        {
                            Session["sect"] = section;
                        }
                        criteria.Add("Section", Session["sect"]);
                    }

                    if (!string.IsNullOrWhiteSpace(acadyr) || (Session["acdyr"].ToString() != ""))
                    {
                        if (acadyr != "")
                        {
                            Session["acdyr"] = acadyr;
                        }
                        criteria.Add("AcademicYear", Session["acdyr"]);
                    }

                    if (!string.IsNullOrWhiteSpace(appname) || (Session["apnam"].ToString() != ""))
                    {
                        if (appname != "")
                        {
                            Session["apnam"] = appname;
                        }
                        criteria.Add("Name", Session["apnam"]);
                    }
                    if (!string.IsNullOrWhiteSpace(feestructyr) || (Session["feestructyr"].ToString() != ""))
                    {
                        if (feestructyr != "")
                        {
                            Session["feestructyr"] = feestructyr;
                        }
                        criteria.Add("FeeStructYear", Session["feestructyr"]);
                    }

                    if (((!string.IsNullOrWhiteSpace(admstat)) || (Session["stats"].ToString() != "")) && (flag1 != "Register"))
                    {
                        if ((admstat != "") && (admstat != null))
                        {
                            Session["stats"] = admstat;
                        }
                        criteria.Add("AdmissionStatus", Session["stats"]);
                    }

                    if (!string.IsNullOrWhiteSpace(appno) || (Session["appno"].ToString() != ""))
                    {
                        if (appno != "")
                        {
                            Session["appno"] = appno;
                        }
                        criteria.Add("ApplicationNo", Session["appno"]);
                    }

                    if (!string.IsNullOrWhiteSpace(preregnumber) || (Session["regno"].ToString() != ""))
                    {
                        if (preregnumber != "")
                        {
                            Session["regno"] = preregnumber;
                        }
                        criteria.Add("PreRegNum", Convert.ToInt64(Session["regno"]));
                    }

                    if (!string.IsNullOrWhiteSpace(idnum) || (Session["idnum"].ToString() != ""))
                    {
                        if (idnum != "")
                        {
                            Session["idnum"] = idnum;
                        }
                        criteria.Add("NewId", Session["idnum"]);
                    }
                    if (!string.IsNullOrEmpty(AppDateFrm) && !string.IsNullOrEmpty(AppDateTo))
                    {
                        AppDateFrm = AppDateFrm.Trim();
                        AppDateTo = AppDateTo.Trim();
                        IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                        //string To = string.Format("{0:MM/dd/yyyy}", DateTime.Now);
                        DateTime[] fromto = new DateTime[2];
                        fromto[0] = DateTime.Parse(AppDateFrm, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        fromto[1] = DateTime.Parse(AppDateTo, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);// Convert.ToDateTime(To + " " + "23:59:59");
                        criteria.Add("CreatedDateNew", fromto);
                    }
                    if (stdntmgmt == "yes")
                    {
                        if (!string.IsNullOrWhiteSpace(ishosteller) || (Session["ishosteller"].ToString() != ""))
                        {
                            if (ishosteller != "")
                            {
                                if (ishosteller == "Yes")
                                {
                                    Session["ishosteller"] = true;
                                    Session["hostlr"] = "Yes";
                                }
                                else
                                {
                                    Session["ishosteller"] = false;
                                    Session["hostlr"] = "No";
                                }
                            }
                            criteria.Add("IsHosteller", Session["ishosteller"]);
                        }
                    }
                }
                else
                {
                    Session["cmpnm"] = "";
                    Session["grd"] = "";
                    Session["sect"] = "";
                    Session["acdyr"] = "";
                    Session["apnam"] = "";
                    Session["stats"] = "";
                    Session["appno"] = "";
                    Session["regno"] = "";
                    Session["ishosteller"] = "";
                    Session["feestructyr"] = "";
                    Session["idnum"] = "";
                    Session["Gen"] = "";
                }
                #endregion

                if (flag1 == "Register")   // To check whether this call is from StudentManagement page
                {
                    if (!string.IsNullOrWhiteSpace(admstat) || (Session["stats"].ToString() != ""))
                    {
                        if (admstat != "")
                        {
                            Session["stats"] = admstat;
                        }
                        criteria.Add("AdmissionStatus", Session["stats"]);
                    }
                    else
                    {
                        criteria.Add("AdmissionStatus", "Registered");
                    }
                    ViewBag.Studentmgmt = "yes";
                    Session["studentmgmt"] = "yes";
                }
                else
                {
                    var rle = Session["userrolesarray"] as IEnumerable<string>;
                    if (rle.Contains("ADM-FNC"))
                    {
                        criteria.Add("AdmissionStatus", "Sent For Clearance");
                    }
                    else
                    {
                        Session["studentmgmt"] = "";
                        if (Session["userrole"].ToString() == "ADM-APP")
                        {
                            criteria.Add("Status", "1");
                        }
                        else
                        {
                            criteria.Add("Status1", "2");
                        }
                    }
                }
                Dictionary<long, IList<TIPS.Entities.AdmissionEntities.StudentTemplateView>> StudentTemplate;
                if (!string.IsNullOrEmpty(idnum) || !string.IsNullOrEmpty(appname))
                {//like Search
                    StudentTemplate = ads.GetStudentDetailsListWithPagingAndCriteriaWithAlias(page - 1, rows, sidx, sord, colName, values, criteria, null);
                }
                else
                {//Exact Search
                    StudentTemplate = ads.GetStudentDetailsListExactWithPagingAndCriteriaWithAlias(page - 1, rows, sidx, sord, colName, values, criteria, null);
                }

                if (StudentTemplate.Count > 0)
                {
                    long totalrecords = StudentTemplate.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,

                        rows = (from items in StudentTemplate.First().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[] {
                                        items.Id.ToString(),
                                    items.ApplicationNo,
                              items.PreRegNum.ToString(),
                              String.Format(@"<a style='color:#034af3;text-decoration:underline' href= '#' onclick = 'getdata("+"\"" + items.Id + "\"" + ")' >{0}</a>",items.Name),
                              items.Gender,
                              items.Grade,
                              items.Section,
                              items.Campus,
                              items.FeeStructYear,
                              items.AdmissionStatus,
                              items.NewId,
                              items.AcademicYear,
                              //items.CreatedDateNew!=null? items.CreatedDateNew.ToString("dd/MM/yyyy"):"",
                              items.CreatedDate,
                              items.EntryFrom
                              }
                                })
                    };
                    return Json(jsondat, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult ExportToExcel(string Id, string grade, string section, string acadyr, string appname, string feestructyr, string admstat, string appno, string idnum, string preregnumber, string ishosteller, string flag, string flag1, string reset, string stdntmgmt, string AppDateFrm, string AppDateTo, int rows)
        {
            string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
            try
            {
                Session["emailsent"] = "";
                AdmissionManagementService ads = new AdmissionManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                string colName = string.Empty; string[] values = new string[1];
                var usrcmp = Session["UserCampus"] as IEnumerable<string>;
                if (usrcmp.Count() != 0)
                {
                    if (usrcmp.First() != null)            // to check if the usrcmp obj is null or with data
                    {
                        if (reset != "yes")
                        {
                            if (!string.IsNullOrWhiteSpace(Id) && usrcmp.Contains(Id)) { criteria.Add("Campus", Id); }
                            else if (!string.IsNullOrWhiteSpace(Id) && !usrcmp.Contains(Id)) { criteria.Add("Campus", "no campus"); }
                            else
                            { criteria.Add("Campus", usrcmp); }
                        }
                        else
                        { criteria.Add("Campus", usrcmp); }
                    }
                }
                else { criteria.Add("Campus", "no campus"); }

                if (reset != "yes")
                {
                    if (!string.IsNullOrWhiteSpace(grade) || (Session["grd"].ToString() != ""))
                    {
                        if (grade != "")
                        {
                            Session["grd"] = grade;
                        }
                        criteria.Add("Grade", Session["grd"].ToString());
                    }
                    if (!string.IsNullOrWhiteSpace(section) || (Session["sect"].ToString() != ""))
                    {
                        if (section != "")
                        {
                            Session["sect"] = section;
                        }
                        criteria.Add("Section", Session["sect"]);
                    }
                    if (!string.IsNullOrWhiteSpace(acadyr) || (Session["acdyr"].ToString() != ""))
                    {
                        if (acadyr != "")
                        {
                            Session["acdyr"] = acadyr;
                        }
                        criteria.Add("AcademicYear", Session["acdyr"]);
                    }
                    if (!string.IsNullOrWhiteSpace(appname) || (Session["apnam"].ToString() != ""))
                    {
                        if (appname != "")
                        {
                            Session["apnam"] = appname;
                        }
                        criteria.Add("Name", Session["apnam"]);
                    }
                    if (!string.IsNullOrWhiteSpace(feestructyr) || (Session["feestructyr"].ToString() != ""))
                    {
                        if (feestructyr != "")
                        {
                            Session["feestructyr"] = feestructyr;
                        }
                        criteria.Add("FeeStructYear", Session["feestructyr"]);
                    }

                    if (((!string.IsNullOrWhiteSpace(admstat)) || (Session["stats"].ToString() != "")) && (flag1 != "Register"))
                    {
                        if ((admstat != "") && (admstat != null))
                        {
                            Session["stats"] = admstat;
                        }
                        criteria.Add("AdmissionStatus", Session["stats"]);
                    }

                    if (!string.IsNullOrWhiteSpace(appno) || (Session["appno"].ToString() != ""))
                    {
                        if (appno != "")
                        {
                            Session["appno"] = appno;
                        }
                        criteria.Add("ApplicationNo", Session["appno"]);
                    }

                    if (!string.IsNullOrWhiteSpace(preregnumber) || (Session["regno"].ToString() != ""))
                    {
                        if (preregnumber != "")
                        {
                            Session["regno"] = preregnumber;
                        }
                        criteria.Add("PreRegNum", Convert.ToInt64(Session["regno"]));
                    }
                    if (!string.IsNullOrWhiteSpace(idnum) || (Session["idnum"].ToString() != ""))
                    {
                        if (idnum != "")
                        {
                            Session["idnum"] = idnum;
                        }
                        criteria.Add("NewId", Session["idnum"]);
                    }
                    if (!string.IsNullOrEmpty(AppDateFrm) && !string.IsNullOrEmpty(AppDateTo))
                    {
                        AppDateFrm = AppDateFrm.Trim();
                        AppDateTo = AppDateTo.Trim();
                        IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                        DateTime[] fromto = new DateTime[2];
                        fromto[0] = DateTime.Parse(AppDateFrm, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        fromto[1] = DateTime.Parse(AppDateTo, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);// Convert.ToDateTime(To + " " + "23:59:59");
                        criteria.Add("CreatedDateNew", fromto);
                    }
                    if (stdntmgmt == "yes")
                    {
                        if (!string.IsNullOrWhiteSpace(ishosteller) || (Session["ishosteller"].ToString() != ""))
                        {
                            if (ishosteller != "")
                            {
                                if (ishosteller == "Yes")
                                {
                                    Session["ishosteller"] = true;
                                    Session["hostlr"] = "Yes";
                                }
                                else
                                {
                                    Session["ishosteller"] = false;
                                    Session["hostlr"] = "No";
                                }
                            }
                            criteria.Add("IsHosteller", Session["ishosteller"]);
                        }
                    }
                }
                else
                {
                    Session["cmpnm"] = "";
                    Session["grd"] = "";
                    Session["sect"] = "";
                    Session["acdyr"] = "";
                    Session["apnam"] = "";
                    Session["stats"] = "";
                    Session["appno"] = "";
                    Session["regno"] = "";
                    Session["ishosteller"] = "";
                }

                if (flag1 == "Register")   // To check whether this call is from StudentManagement page
                {
                    if (!string.IsNullOrWhiteSpace(admstat) || (Session["stats"].ToString() != ""))
                    {
                        if (admstat != "")
                        {
                            Session["stats"] = admstat;
                        }
                        criteria.Add("AdmissionStatus", Session["stats"]);
                    }
                    else
                    {
                        criteria.Add("AdmissionStatus", "Registered");
                    }
                    ViewBag.Studentmgmt = "yes";
                    Session["studentmgmt"] = "yes";
                }
                else
                {
                    Session["studentmgmt"] = "";
                    if (Session["userrole"].ToString() == "ADM-APP")
                    {
                        if (flag != "ADM-APP")
                        {
                            criteria.Add("Status", "1");
                        }
                    }
                    if ((userid == "GRL-ADMS") || (userid == "CHE-GRL") || (userid == "TIR-GRL") || (userid == "ERN-GRL") || (userid == "KAR-GRL") || (userid == "APP-ADMN"))
                    {
                        if ((flag != "GRL-ADMS") || (flag != "CHE-GRL") || (userid == "TIR-GRL") || (userid == "ERN-GRL") || (userid == "KAR-GRL") || (userid == "APP-ADMN"))
                        {
                            criteria.Add("Status1", "2");
                        }
                    }
                }
                Dictionary<long, IList<TIPS.Entities.AdmissionEntities.StudentTemplateView>> StudentTemplate;

                if ((!string.IsNullOrWhiteSpace(grade)) && (string.IsNullOrWhiteSpace(appname)) && (string.IsNullOrWhiteSpace(appno)))
                {
                    StudentTemplate = ads.GetStudentTemplateViewListWithEQsearchCriteria(null, rows, string.Empty, string.Empty, criteria);
                }
                else
                {
                    StudentTemplate = ads.GetStudentTemplateViewListWithPagingAndCriteria(null, rows, string.Empty, string.Empty, criteria);
                }

                if (StudentTemplate != null && StudentTemplate.First().Value != null && StudentTemplate.First().Value.Count > 0)
                {
                    var stuList = StudentTemplate.First().Value.ToList();
                    base.ExptToXL(stuList, "StudentList", (item => new
                    {
                        item.ApplicationNo,
                        item.PreRegNum,
                        item.Name,
                        item.Grade,
                        item.Section,
                        item.Campus,
                        item.FeeStructYear,
                        item.AdmissionStatus,
                        item.NewId,
                        item.AcademicYear,
                        item.CreatedDate
                    }));
                    return new EmptyResult();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult LoadPartialView(string tabindex)
        {
            string partialviewname = null;
            StudentTemplate StudentTemplate = new StudentTemplate();

            MastersService ms = new MastersService();

            Dictionary<string, object> criteria = new Dictionary<string, object>();

            Dictionary<long, IList<AcademicyrMaster>> AcademicyrMaster = ms.GetAcademicyrMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            Dictionary<long, IList<GradeMaster>> GradeMaster = ms.GetGradeMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            Dictionary<long, IList<BloodGroupMaster>> BloodGroupMaster = ms.GetBloodGroupMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            Dictionary<long, IList<FeeTypeMaster>> FeeTypeMaster = ms.GetFeeTypeMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            Dictionary<long, IList<ModeOfPaymentMaster>> ModeOfPaymentMaster = ms.GetModeOfPaymentMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            Dictionary<long, IList<FeeStructureYearMaster>> FeeStructureYearMaster = ms.GetFeeStructureYearMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            Dictionary<long, IList<SectionMaster>> SectionMaster = ms.GetSectionMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);

            Dictionary<long, IList<CampusMaster>> CampusMaster = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            Dictionary<long, IList<RelationshipMaster>> RelationshipMaster = ms.GetRelationshipMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            criteria.Clear();
            criteria.Add("DocumentFor", "Student");
            Dictionary<long, IList<DocumentTypeMaster>> DocumentTypeMaster = ms.GetDocumentTypeMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            criteria.Clear();

            if (tabindex == "0")
            {
                partialviewname = "StudentDetails";

                string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                Session["preregno"] = 0;
                Session["preregid"] = 0;
                AdmissionManagementService ams = new AdmissionManagementService();

                ViewBag.campusddl = CampusMaster.First().Value;
                ViewBag.familyddl = RelationshipMaster.First().Value;
                ViewBag.documentddl = DocumentTypeMaster.First().Value;

                ViewBag.acadyrddl = AcademicyrMaster.First().Value;
                ViewBag.gradeddl = GradeMaster.First().Value;
                ViewBag.gradeddl1 = GradeMaster.First().Value;

                ViewBag.bloodgrpddl = BloodGroupMaster.First().Value;
                ViewBag.feetypeddl = FeeTypeMaster.First().Value;
                ViewBag.modeofpmtddl = ModeOfPaymentMaster.First().Value;
                ViewBag.feestructyrddl = FeeStructureYearMaster.First().Value;
                ViewBag.sectionddl = SectionMaster.First().Value;

                ViewBag.admissionstatus = "New Registration";               //Registration Status
                var aa = Session["editid"];
                Session["time"] = "";
                Session["date"] = "";
                Session["admstat"] = "";
                Session["tabfreeze"] = "no";

                AdmissionManagementService ads = new AdmissionManagementService();

                StudentTemplate = ads.GetStudentDetailsById(Convert.ToInt32(Session["editid"]));

                CampusGradeddl(StudentTemplate.Campus);
                if (StudentTemplate != null)
                {
                    IList<AddressDetails> adList = new List<AddressDetails>();
                    IEnumerable<AddressDetails> ad1 = from cust in StudentTemplate.AddressDetailsList
                                                      where cust.AddressType == "Primary Address"
                                                      select cust;
                    adList.Add(ad1.FirstOrDefault());
                    IEnumerable<AddressDetails> ad2 = from cust in StudentTemplate.AddressDetailsList
                                                      where cust.AddressType == "Secondary Address"
                                                      select cust;
                    adList.Add(ad2.FirstOrDefault());
                    StudentTemplate.AddressDetailsList = adList;

                }
            }
            else if (tabindex == "1")
            {
                partialviewname = "StudentDetails";
                FamilyDetails familyDetails = new FamilyDetails();
                IList<FamilyDetails> familydetailsList = new List<FamilyDetails>();
                familydetailsList.Add(familyDetails);
                StudentTemplate.FamilyDetailsList = familydetailsList;
            }

            return PartialView(partialviewname, StudentTemplate);
        }

        public ActionResult StudentDetails()
        {
            return PartialView();
        }

        public ActionResult CheckApplicationNo(string appno)
        {
            AdmissionManagementService ams1 = new AdmissionManagementService();
            Dictionary<string, object> accnoCriteria = new Dictionary<string, object>();
            accnoCriteria.Add("ApplicationNo", appno);
            StudentTemplate sd = new StudentTemplate();
            sd = ams1.GetStudentDetailsByAppNo(appno);
            if (sd != null && sd.PreRegNum != 0)
            {
                return Json("Err", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult NewRegistration()
        {
            try
            {
                string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                Session["preregno"] = 0;
                Session["preregid"] = 0;
                AdmissionManagementService ams = new AdmissionManagementService();

                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<long, IList<AcademicyrMaster>> AcademicyrMaster = ms.GetAcademicyrMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                Dictionary<long, IList<GradeMaster>> GradeMaster = ms.GetGradeMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                Dictionary<long, IList<BloodGroupMaster>> BloodGroupMaster = ms.GetBloodGroupMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                Dictionary<long, IList<FeeTypeMaster>> FeeTypeMaster = ms.GetFeeTypeMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                Dictionary<long, IList<ModeOfPaymentMaster>> ModeOfPaymentMaster = ms.GetModeOfPaymentMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                Dictionary<long, IList<FeeStructureYearMaster>> FeeStructureYearMaster = ms.GetFeeStructureYearMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                Dictionary<long, IList<SectionMaster>> SectionMaster = ms.GetSectionMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                Dictionary<long, IList<NationalityMaster>> NationMaster = ms.GetNationalityDetails(0, 9999, string.Empty, string.Empty, criteria);
                //int[] values;
                //values = new int[16];
                //criteria.Clear();
                //values[0] = 1; values[1] = 2; values[2] = 3;
                //values[3] = 4; values[4] = 5; values[5] = 6;
                //values[6] = 7; values[7] = 8; values[8] = 9; values[9] = 10;
                //values[10] = 11; values[11] = 12; values[12] = 100; values[13] = 101; values[14] = 102; values[15] = 103;
                //Dictionary<long, IList<GradeMaster>> GradeMaster2 = ms.GetGradeMasterListWithPagingAndCriteriaWithIn(0, 50, "", "", "grad", values, criteria, null);
                //ViewBag.gradeddl2 = GradeMaster2.FirstOrDefault().Value;

                var usrcmp = Session["UserCampus"] as IEnumerable<string>;
                if (usrcmp.First() != null)            // to check if the usrcmp obj is null or with data
                {
                    criteria.Add("Name", usrcmp);
                }
                Dictionary<long, IList<CampusMaster>> CampusMaster = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                criteria.Clear();
                Dictionary<long, IList<RelationshipMaster>> RelationshipMaster = ms.GetRelationshipMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                criteria.Clear();
                criteria.Add("DocumentFor", "Student");
                Dictionary<long, IList<DocumentTypeMaster>> DocumentTypeMaster = ms.GetDocumentTypeMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                criteria.Clear();
                ViewBag.campusddl = CampusMaster.First().Value;
                ViewBag.familyddl = RelationshipMaster.First().Value;
                ViewBag.documentddl = DocumentTypeMaster.First().Value;

                ViewBag.acadyrddl = AcademicyrMaster.First().Value;
                ViewBag.gradeddl = GradeMaster.First().Value;
                ViewBag.gradeddl1 = GradeMaster.First().Value;

                ViewBag.bloodgrpddl = BloodGroupMaster.First().Value;
                ViewBag.feetypeddl = FeeTypeMaster.First().Value;
                ViewBag.modeofpmtddl = ModeOfPaymentMaster.First().Value;
                ViewBag.feestructyrddl = FeeStructureYearMaster.First().Value;
                ViewBag.sectionddl = SectionMaster.First().Value;
                ViewBag.Nationality = NationMaster.First().Value;
                IList<BankMaster> bankMsObj = BankMasterFunc();
                ViewBag.BankName = bankMsObj;
                ViewBag.admissionstatus = "New Registration";               //Registration Status
                var aa = Session["editid"];
                Session["time"] = "";
                Session["date"] = "";
                Session["admstat"] = "";
                if (Convert.ToInt32(Session["editid"]) == 0)
                {
                    Session["tabfreeze"] = "yes";
                    Dictionary<long, IList<PreRegDetails>> prd = ams.GetPreRegDetailsListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, null);
                    var id1 = prd.First().Value[0].PreRegNum + 1;
                    PreRegDetails srn = new PreRegDetails();
                    srn.PreRegNum = id1;
                    srn.Id = prd.First().Value[0].Id;
                    srn.Date = DateTime.Now;
                    ams.CreateOrUpdatePreRegDetails(srn);
                    Session["preregno"] = id1;
                    ViewBag.preregno = id1;
                    ViewBag.Date = DateTime.Now.ToString("dd/MM/yyyy"); // prd.First().Value[id1 - 1].Date.ToString();
                    ViewBag.time = DateTime.Now.ToString("hh:mm:ss");

                    Session["time"] = DateTime.Now.ToString("hh:mm:ss");

                    Session["date"] = DateTime.Now.ToString("dd/MM/yyyy");

                    //ViewBag.admissionstatus = "New Registration";               //Registration Status
                    //Session["admstat"] = "New Registration";
                    ViewBag.admissionstatus = "New Enquiry";
                    Session["admstat"] = "New Enquiry";      //First level of Registration Status
                    ViewBag.processby = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                    return View();
                }
                else
                {
                    Session["tabfreeze"] = "no";
                    AdmissionManagementService ads = new AdmissionManagementService();
                    StudentTemplate StudentTemplate = ads.GetStudentDetailsById(Convert.ToInt32(Session["editid"]));
                    CampusGradeddl(StudentTemplate.Campus);
                    if (StudentTemplate != null)
                    {
                        IList<AddressDetails> adList = new List<AddressDetails>();
                        IEnumerable<AddressDetails> ad1 = from cust in StudentTemplate.AddressDetailsList
                                                          where cust.AddressType == "Primary Address"
                                                          select cust;
                        adList.Add(ad1.FirstOrDefault());
                        IEnumerable<AddressDetails> ad2 = from cust in StudentTemplate.AddressDetailsList
                                                          where cust.AddressType == "Secondary Address"
                                                          select cust;
                        adList.Add(ad2.FirstOrDefault());
                        StudentTemplate.AddressDetailsList = adList;
                    }
                    FamilyDetails familyDetails = new FamilyDetails();
                    if (StudentTemplate.FamilyDetailsList.Count == 0)
                    {
                        IList<FamilyDetails> familydetailsList = new List<FamilyDetails>();
                        familydetailsList.Add(familyDetails);
                        StudentTemplate.FamilyDetailsList = familydetailsList;
                    }
                    PastSchoolDetails PastSchoolDetails = new PastSchoolDetails();
                    if (StudentTemplate.PastSchoolDetailsList.Count == 0)
                    {
                        IList<PastSchoolDetails> PastSchoolDetailsList = new List<PastSchoolDetails>();
                        PastSchoolDetailsList.Add(PastSchoolDetails);
                        StudentTemplate.PastSchoolDetailsList = PastSchoolDetailsList;
                    }
                    PaymentDetails PaymentDetails = new PaymentDetails();
                    if (StudentTemplate.PaymentDetailsList.Count == 0)
                    {
                        IList<PaymentDetails> PaymentDetailsList = new List<PaymentDetails>();
                        PaymentDetailsList.Add(PaymentDetails);
                        StudentTemplate.PaymentDetailsList = PaymentDetailsList;
                    }
                    ApproveAssign ApproveAssign = new ApproveAssign();
                    if (StudentTemplate.ApproveAssignList.Count == 0)
                    {
                        IList<ApproveAssign> ApproveAssignList = new List<ApproveAssign>();
                        ApproveAssignList.Add(ApproveAssign);
                        StudentTemplate.ApproveAssignList = ApproveAssignList;
                    }
                    DocumentDetails DocumentDetails = new DocumentDetails();
                    if (StudentTemplate.DocumentDetailsList.Count == 0)
                    {
                        IList<DocumentDetails> DocumentDetailsList = new List<DocumentDetails>();
                        DocumentDetailsList.Add(DocumentDetails);
                        StudentTemplate.DocumentDetailsList = DocumentDetailsList;
                    }
                    Session["preregno"] = StudentTemplate.PreRegNum;
                    ViewBag.preregno = Session["preregno"];
                    ViewBag.grade = StudentTemplate.Grade;
                    ViewBag.nam = StudentTemplate.Name;
                    ViewBag.gender = StudentTemplate.Gender;
                    ViewBag.campus = StudentTemplate.Campus;
                    ViewBag.acadyr = StudentTemplate.AcademicYear;
                    ViewBag.RouteNo = StudentTemplate.VanNo;
                    ViewBag.approvalstatus = StudentTemplate.Status;
                    ViewBag.admissionstatus = StudentTemplate.AdmissionStatus;    //Registration Status
                    Session["admstat"] = StudentTemplate.AdmissionStatus;
                    ViewBag.Date = StudentTemplate.CreatedDate;
                    ViewBag.time = StudentTemplate.CreatedTime;
                    Session["time"] = StudentTemplate.CreatedTime;
                    Session["date"] = StudentTemplate.CreatedDate;
                    ViewBag.processby = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                    if (Session["studentmgmt"] == "yes" && StudentTemplate.AdmissionStatus == "Registered")
                    {
                        ViewBag.Studentmgmt = "yes";
                    }
                    if (Session["userrole"].ToString() == "ADM-APP")
                    {
                        ViewBag.userrole = "Approver";
                    }
                    Session["grad"] = StudentTemplate.Grade;
                    Session["nam"] = StudentTemplate.Name;
                    Session["gender"] = StudentTemplate.Gender;
                    Session["campus"] = StudentTemplate.Campus;
                    return View(StudentTemplate);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult NewRegistration(StudentTemplate st, FamilyDetails fd, HttpPostedFileBase file1)
        {
            try
            {
                string RecipientInfo = "", Subject = "", Body = "", MailBody = ""; bool retValue;
                string info = string.Empty;
                string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                AdmissionManagementService ams1 = new AdmissionManagementService();
                IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                EmailHelper em = new EmailHelper();
                MastersService ms = new MastersService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<long, IList<PreRegDetails>> prd = ams1.GetPreRegDetailsListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, null);
                Dictionary<long, IList<AcademicyrMaster>> AcademicyrMaster = ms.GetAcademicyrMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                Dictionary<long, IList<GradeMaster>> GradeMaster = ms.GetGradeMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                Dictionary<long, IList<BloodGroupMaster>> BloodGroupMaster = ms.GetBloodGroupMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                Dictionary<long, IList<FeeTypeMaster>> FeeTypeMaster = ms.GetFeeTypeMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                Dictionary<long, IList<ModeOfPaymentMaster>> ModeOfPaymentMaster = ms.GetModeOfPaymentMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                Dictionary<long, IList<FeeStructureYearMaster>> FeeStructureYearMaster = ms.GetFeeStructureYearMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                Dictionary<long, IList<SectionMaster>> SectionMaster = ms.GetSectionMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                Dictionary<long, IList<NationalityMaster>> NationMaster = ms.GetNationalityDetails(0, 9999, string.Empty, string.Empty, criteria);
                var usrcmp = Session["UserCampus"] as IEnumerable<string>;
                if (usrcmp != null && usrcmp.First() != null)            // to check if the usrcmp obj is null or with data
                {
                    criteria.Add("Name", usrcmp);
                }
                Dictionary<long, IList<CampusMaster>> CampusMaster = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                criteria.Clear();
                criteria.Add("Module", "Student");
                Dictionary<long, IList<RelationshipMaster>> RelationshipMaster = ms.GetRelationshipMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                criteria.Clear();
                criteria.Add("DocumentFor", "Student");
                Dictionary<long, IList<DocumentTypeMaster>> DocumentTypeMaster = ms.GetDocumentTypeMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                criteria.Clear();
                IList<BankMaster> bnkMstr = BankMasterFunc();
                ViewBag.BankName = bnkMstr;
                ViewBag.campusddl = CampusMaster.First().Value;
                ViewBag.familyddl = RelationshipMaster.First().Value;
                ViewBag.documentddl = DocumentTypeMaster.First().Value;
                ViewBag.acadyrddl = AcademicyrMaster.First().Value;
                ViewBag.gradeddl = GradeMaster.First().Value;
                ViewBag.bloodgrpddl = BloodGroupMaster.First().Value;
                ViewBag.feetypeddl = FeeTypeMaster.First().Value;
                ViewBag.modeofpmtddl = ModeOfPaymentMaster.First().Value;
                ViewBag.feestructyrddl = FeeStructureYearMaster.First().Value;
                ViewBag.sectionddl = SectionMaster.First().Value;
                ViewBag.Nationality = NationMaster.First().Value;
                ViewBag.RouteNo = st.VanNo;
                ViewBag.grade = Session["grad"]; // grad;
                ViewBag.nam = Session["nam"]; // nam;
                ViewBag.gender = Session["gender"]; // gender;
                ViewBag.campus = Session["campus"]; // campus;
                ViewBag.processby = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                if (Session["userrole"] != null && Session["userrole"].ToString() == "ADM-APP") { ViewBag.userrole = "Approver"; }
                MailBody = GetBodyofMail();
                Dictionary<string, object> criteria1 = new Dictionary<string, object>();
                criteria1.Add("PreRegNum", st.PreRegNum);
                Dictionary<long, IList<TIPS.Entities.AdmissionEntities.StudentTemplateView>> sdt = ams1.GetStudentTemplateViewListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria1);
                CampusGradeddl(st.Campus);
                if (sdt.First().Value.Count == 0) { Session["preregid"] = 0; }
                else
                {
                    Session["preregid"] = sdt.First().Value[0].Id;
                    st.Id = sdt.First().Value[0].Id;
                }
                ViewBag.preregno = st.PreRegNum; //Session["preregno"];
                if (!string.IsNullOrEmpty(st.CreatedDate))
                    st.CreatedDateNew = DateTime.Parse(st.CreatedDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                if (Request.Form["AddPayment"] == "AddPayment")
                {
                    criteria.Clear();
                    criteria.Add("FeeType", st.PaymentDetailsList[0].FeeType);
                    criteria.Add("PreRegNum", st.PreRegNum);
                    //criteria.Add("ReferenceNo", refno);
                    Dictionary<long, IList<PaymentDetails>> PaymentDetails = ams1.GetPaymentDetailsListWithPagingAndCriteria(null, null, null, null, criteria);
                    if (PaymentDetails.First().Value.Count == 0)
                    {
                        PaymentDetails pmtadd = new PaymentDetails();
                        pmtadd.PreRegNum = st.PreRegNum;
                        pmtadd.StudentId = 0;
                        pmtadd.FeeType = st.PaymentDetailsList[0].FeeType;
                        pmtadd.ModeOfPayment = st.PaymentDetailsList[0].ModeOfPayment;
                        pmtadd.Amount = st.PaymentDetailsList[0].Amount;
                        pmtadd.ReferenceNo = st.PaymentDetailsList[0].ReferenceNo;
                        if (!string.IsNullOrEmpty(Request["PaymentDetailsList[0].ChequeDate"]))
                        {
                            pmtadd.ChequeDate = DateTime.Parse(Request["PaymentDetailsList[0].ChequeDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        }
                        pmtadd.BankName = st.PaymentDetailsList[0].BankName;
                        pmtadd.Remarks = st.PaymentDetailsList[0].Remarks;
                        if (!string.IsNullOrEmpty(Request["PaymentDetailsList[0].PaidDate"]))
                        {
                            pmtadd.PaidDate = DateTime.Parse(Request["PaymentDetailsList[0].PaidDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        }
                        if (!string.IsNullOrEmpty(Request["PaymentDetailsList[0].ClearedDate"]))
                        {
                            pmtadd.ClearedDate = DateTime.Parse(Request["PaymentDetailsList[0].ClearedDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        }
                        pmtadd.FeePaidStatus = st.PaymentDetailsList[0].FeePaidStatus;
                        pmtadd.CreatedDate = DateTime.Now;
                        pmtadd.CreatedBy = userid;
                        pmtadd.ModifiedDate = DateTime.Now;
                        pmtadd.ModifiedBy = userid;
                        info = " You have Created an New Registration with Student Application Number " + st.ApplicationNo;
                        UpdateInbox(st.Campus, info, userid, st.PreRegNum);
                        ams1.CreateOrUpdatePaymentDetails(pmtadd);
                        if (st != null && st.PaymentDetailsList[0].FeeType == "Pre-Registration Amount")
                        {
                            st.FamilyDetailsList = null;
                            st.PastSchoolDetailsList = null;
                            st.ApproveAssignList = null;
                            st.DocumentDetailsList = null;
                            criteria.Clear();
                            criteria.Add("StudentId", st.Id);
                            Dictionary<long, IList<AddressDetails>> add = ams1.GetAddressDetailsListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                            if (add.First().Value.Count != 0)
                            {
                                if (add.Count == 1) { st.AddressDetailsList[0].AddressType = "Primary Address"; st.AddressDetailsList[0].Id = add.First().Value[0].Id; }
                                else if (add.Count == 2)
                                {
                                    st.AddressDetailsList[0].AddressType = "Primary Address"; st.AddressDetailsList[0].Id = add.First().Value[0].Id;
                                    st.AddressDetailsList[1].AddressType = "Secondary Address"; st.AddressDetailsList[1].Id = add.First().Value[1].Id;
                                }
                                else { }
                            }
                            else
                            {
                                if (add.Count == 1) { st.AddressDetailsList[0].AddressType = "Primary Address"; }
                                else if (add.Count == 2) { st.AddressDetailsList[0].AddressType = "Primary Address"; st.AddressDetailsList[1].AddressType = "Secondary Address"; }
                                else { }
                            }
                            //st.PreRegNum = Convert.ToInt64(Session["preregno"]);                    //Pre-Registration number
                            //st.Id = Convert.ToInt64(Session["preregid"]);  // preregid;
                            //st.CreatedDate = DateTime.Now.Date.ToString("dd/MM/yyyy");
                            st.AdmissionStatus = "New Registration";
                            st.Status = "1";          // key for sentfor approval, on hold,callforinterview
                            st.Status1 = "2";         // key for newregistration,sentforapproval,onhold,callforinterview
                            st.PaymentDetailsList = null;
                            info = " You have Created an Enquiry with Student Application Number " + st.ApplicationNo;
                            UpdateInbox(st.Campus, info, userid, st.PreRegNum);
                            ams1.CreateOrUpdateStudentDetails(st);
                            ViewBag.Tabselected = "Payment";
                        }
                        ViewBag.admissionstatus = st.AdmissionStatus;
                        return View(st);
                    }
                    else
                    {
                        ViewBag.errmsg = "Paid";
                        ViewBag.admissionstatus = st.AdmissionStatus;
                        return View(st);
                    }
                }
                else if (Request.Form["DocUpload"] == "Upload")
                {
                    ViewBag.tabindex = 1;
                    if (Session["studentmgmt"] == "yes")
                    {
                        ViewBag.Studentmgmt = "yes";
                    }
                    string path = file1.InputStream.ToString();
                    byte[] imageSize = new byte[file1.ContentLength];
                    file1.InputStream.Read(imageSize, 0, (int)file1.ContentLength);
                    UploadedFiles fu1 = new UploadedFiles();
                    fu1.DocumentData = imageSize;
                    fu1.DocumentName = file1.FileName;
                    //    fu1.DocumentType = st.UploadedFilesList[0].DocumentType;
                    fu1.DocumentType = st.DocumentDetailsList[0].DocumentType;
                    fu1.DocumentSize = file1.ContentLength.ToString();
                    fu1.UploadedDate = DateTime.Now.Date.ToString("dd/MM/yyyy");
                    //fu1.Name = st.UploadedFilesList[0].Name;
                    //fu1.Phone = st.UploadedFilesList[0].Phone;
                    fu1.DocumentFor = "Student";
                    fu1.Name = st.DocumentDetailsList[0].Name;
                    fu1.Phone = st.DocumentDetailsList[0].Phone;
                    fu1.PreRegNum = st.PreRegNum;// Convert.ToInt64(Session["preregno"]);              // Pre Registration number
                    ViewBag.Date = Session["date"];  // date;
                    ViewBag.time = Session["time"]; // time;
                    ViewBag.admissionstatus = Session["admstat"]; /// admstat;
                    AdmissionManagementService ams = new AdmissionManagementService();
                    ams.CreateOrUpdateUploadedFiles(fu1);
                    ViewData["imageid"] = fu1.Id;
                    return View();
                }
                else if (Request.Form["SmgtSave"] == "Save")
                {
                    Session["tabfreeze"] = "no";
                    st.FamilyDetailsList = null;
                    st.PastSchoolDetailsList = null;
                    st.ApproveAssignList = null;
                    st.PaymentDetailsList = null;
                    st.DocumentDetailsList = null;
                    if (Session["studentmgmt"].ToString() == "yes")
                    {
                        ViewBag.Studentmgmt = "yes";
                        ViewBag.admissionstatus = "Registered";               //Registration Status
                        AdmissionManagementService ads1 = new AdmissionManagementService();
                        st.AddressDetailsList[0].AddressType = "Primary Address";
                        st.AddressDetailsList[1].AddressType = "Secondary Address";
                        //st.PreRegNum = Convert.ToInt64(Session["preregno"]);                    //Pre-Registration number
                        //st.Id = Convert.ToInt64(Session["preregid"]); // preregid;
                        if (Session["date"] == null)
                        {
                            Session["date"] = DateTime.Now.ToString("dd/MM/yyyy");
                        }
                        if (Session["time"] == null)
                        {
                            Session["time"] = DateTime.Now.ToString("hh:mm:ss");
                        }
                        //  st.CreatedDate = Session["date"].ToString(); // date;
                        //  st.CreatedTime = Session["time"].ToString(); // time;// DateTime.Now.ToString("hh:mm:ss");
                        st.AdmissionStatus = "Registered";
                        switch (st.BoardingType)
                        {
                            case "Day Scholar":
                                st.IsHosteller = false;
                                break;
                            case "Week Boarder":
                                st.IsHosteller = true;
                                break;
                            case "Residential":
                                st.IsHosteller = true;
                                break;
                            case "Full Boarder":
                                st.IsHosteller = true;
                                break;
                            default:
                                st.IsHosteller = false;
                                break;
                        }
                        ViewBag.Date = Session["date"]; // date;
                        ViewBag.time = Session["time"]; // time;
                        info = " The Student is Registered with Apllication Number " + st.ApplicationNo;
                        UpdateInbox(st.Campus, info, userid, st.PreRegNum);
                        ads1.CreateOrUpdateStudentDetails(st);
                    }
                    return View(st);
                }
                else if (Request.Form["btnSave"] == "Save")
                {
                    Session["tabfreeze"] = "no";
                    st.FamilyDetailsList = null;
                    st.PastSchoolDetailsList = null;
                    st.ApproveAssignList = null;
                    st.PaymentDetailsList = null;
                    st.DocumentDetailsList = null;
                    Dictionary<string, object> PaymentCriteria = new Dictionary<string, object>();
                    PaymentCriteria.Add("PreRegNum", st.PreRegNum);
                    Dictionary<long, IList<PaymentDetails>> paymentdetails = ams1.GetPaymentDetailsListWithPagingAndCriteria(0, 0, string.Empty, string.Empty, PaymentCriteria);
                    if (paymentdetails != null && paymentdetails.FirstOrDefault().Value.Count > 0 && paymentdetails.Count > 0)
                    {
                        int count = 0;
                        foreach (PaymentDetails pay in paymentdetails.FirstOrDefault().Value)
                        {
                            if (pay.FeeType == "Pre-Registration Amount") { count++; }
                        }
                        ViewBag.admissionstatus = count < 1 ? "New Enquiry" : "New Registration";
                    }
                    else { ViewBag.admissionstatus = "New Enquiry"; }

                    AdmissionManagementService ads = new AdmissionManagementService();
                    Dictionary<string, object> criteria4 = new Dictionary<string, object>();
                    criteria4.Add("StudentId", st.Id);
                    Dictionary<long, IList<AddressDetails>> add = ams1.GetAddressDetailsListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria4);
                    if (add.First().Value.Count != 0)
                    {
                        if (add.First().Value.Count == 1) { st.AddressDetailsList[0].AddressType = "Primary Address"; st.AddressDetailsList[0].Id = add.First().Value[0].Id; }
                        else if (add.First().Value.Count == 2)  // (add.Count == 2)
                        {
                            st.AddressDetailsList[0].AddressType = "Primary Address"; st.AddressDetailsList[0].Id = add.First().Value[0].Id;
                            st.AddressDetailsList[1].AddressType = "Secondary Address"; st.AddressDetailsList[1].Id = add.First().Value[1].Id;
                        }
                        else { }
                    }
                    else
                    {
                        st.AddressDetailsList[0].AddressType = "Primary Address"; st.AddressDetailsList[1].AddressType = "Secondary Address";
                    }
                    //st.PreRegNum = Convert.ToInt64(Session["preregno"]);                    //Pre-Registration number
                    //st.Id = Convert.ToInt64(Session["preregid"]);  // preregid;
                    st.AdmissionStatus = ViewBag.admissionstatus;
                    switch (st.BoardingType)
                    {
                        case "Day Scholar":
                            st.IsHosteller = false;
                            break;
                        case "Week Boarder":
                            st.IsHosteller = true;
                            break;
                        case "Residential":
                            st.IsHosteller = true;
                            break;
                        case "Full Boarder":
                            st.IsHosteller = true;
                            break;
                        default:
                            st.IsHosteller = false;
                            break;
                    }
                    st.Status1 = "2";         // key for newregistration,sentforapproval,onhold,callforinterview
                    ViewBag.Date = Session["date"];  // date;
                    ViewBag.time = Session["time"]; // time;                     
                    //   if (st.Id == 0)    // to chk 
                    if (st.JoiningAcademicYear == null)
                    {
                        st.JoiningAcademicYear = st.AcademicYear; // to add only the first time
                    }
                    if (st.Id == 0) // to send email only once
                    {
                        //send Mail To Parents with template added by micheal
                        IList<CampusEmailId> campusemaildet = GetEmailIdByCampus(st.Campus, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
                        if (ConfigurationManager.AppSettings["SendEmail1"].ToString() == "true")
                        {
                            RecipientInfo = "Dear Parent,";
                            Subject = "Welcome to TIPS - Student Registration In Progress"; // st.Subject;
                            Body = "Thanks for registering with TIPS school and your application is sent for processing. Your online application number is " + st.PreRegNum + " and please refer this number for all further communication. For any queries, mail us at " + campusemaildet.First().EmailId.ToString() + ".";
                            if (!string.IsNullOrEmpty(st.EmailId))
                                retValue = em.SendStudentRegistrationMail(st, st.EmailId, st.Campus, Subject, Body, MailBody, RecipientInfo, "Parent", null);
                            //Send Mail to Managment
                            RecipientInfo = "Dear Team,";
                            Subject = "A new application is available for processing"; // st.Subject;
                            string Body1 = "A new application is available for processing and you are requested to process the same.<br><br>Campus: " + st.Campus + "<br>";
                            Body1 += "Application number: " + st.ApplicationNo + "<br>Pre-Reg number: " + st.PreRegNum + "<br>Applicant name: " + st.Name + "<br>Application grade: " + st.Grade + "<br>Applied Academic year: " + st.AcademicYear + "<br>";
                            retValue = em.SendStudentRegistrationMail(st, null, st.Campus, Subject, Body1, MailBody, RecipientInfo, "Management", null);
                        }
                    }
                    if (!string.IsNullOrEmpty(st.CreatedDate))
                        st.CreatedDateNew = DateTime.Parse(st.CreatedDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    info = " You have saved an New Student Registration with Application Number " + st.ApplicationNo;
                    UpdateInbox(st.Campus, info, userid, st.PreRegNum);
                    ads.CreateOrUpdateStudentDetails(st);
                    return View(st);
                }
                #region Update
                else if (Request.Form["btnApprove"] == "Update")
                {
                    st.FamilyDetailsList = null;
                    st.PastSchoolDetailsList = null;
                    st.ApproveAssignList = null;
                    st.PaymentDetailsList = null;
                    st.DocumentDetailsList = null;
                    AdmissionManagementService ads = new AdmissionManagementService();
                    Dictionary<string, object> criteria4 = new Dictionary<string, object>();
                    criteria4.Add("StudentId", st.Id);
                    Dictionary<long, IList<AddressDetails>> add = ams1.GetAddressDetailsListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria4);
                    if (add.First().Value.Count != 0)
                    {
                        if (add.Count == 1) { st.AddressDetailsList[0].AddressType = "Primary Address"; st.AddressDetailsList[0].Id = add.First().Value[0].Id; }
                        else if (add.Count == 2)
                        {
                            st.AddressDetailsList[0].AddressType = "Primary Address"; st.AddressDetailsList[0].Id = add.First().Value[0].Id;
                            st.AddressDetailsList[1].AddressType = "Secondary Address"; st.AddressDetailsList[1].Id = add.First().Value[1].Id;
                        }
                        else { }
                    }
                    else
                    {
                        if (add.Count == 1) { st.AddressDetailsList[0].AddressType = "Primary Address"; }
                        else if (add.Count == 2) { st.AddressDetailsList[0].AddressType = "Primary Address"; st.AddressDetailsList[1].AddressType = "Secondary Address"; }
                        else { }
                    }
                    //st.PreRegNum = Convert.ToInt64(Session["preregno"]);                    //Pre-Registration number
                    //st.Id = Convert.ToInt64(Session["preregid"]); // preregid;

                    ViewBag.Date = Session["date"]; // date;
                    ViewBag.time = Session["time"];  // time; // st.CreatedTime;
                    switch (st.BoardingType)
                    {
                        case "Day Scholar":
                            st.IsHosteller = false;
                            break;
                        case "Week Boarder":
                            st.IsHosteller = true;
                            break;
                        case "Residential":
                            st.IsHosteller = true;
                            break;
                        case "Full Boarder":
                            st.IsHosteller = true;
                            break;
                        default:
                            st.IsHosteller = false;
                            break;
                    }
                    if (st.AdmissionStatus == "Registered")
                    {
                        System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                        IList<CampusEmailId> campusemaildet = GetEmailIdByCampus(st.Campus, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
                        string[] dob = st.DOB.ToString().Split('/');   // dateofbirth split
                        string sex = "";
                        if (st.Gender == "Male") { sex = "his"; }
                        else if (st.Gender == "Female") { sex = "her"; }
                        st.NewId = StudentIdNumberLogic(st.Campus, st.FeeStructYear, st.Grade, st.AcademicYear);
                        st.AdmissionStatus = "Registered";    //Admission Registered By Admin  
                        //   st.Status = "1";          // key for sentfor approval, on hold,callforinterview  
                        st.OperationalYear = st.AcademicYear; // for MIS Report Year
                        ads.CreateOrUpdateStudentDetails(st);
                        Session["registered"] = "yes";
                        //registered = "yes";
                        Session["regid"] = st.NewId;
                        // ---------------------------- creating Parent userid -----------------------------------------
                        //    string[] dob = st.DOB.ToString().Split('/');
                        TIPS.Entities.User user = new TIPS.Entities.User();
                        user.UserId = "P" + st.NewId;
                        user.Password = dob[0] + dob[1] + dob[2];
                        user.Campus = st.Campus;
                        user.UserType = "Parent";
                        user.EmailId = st.EmailId;
                        user.CreatedDate = DateTime.Now;
                        user.ModifiedDate = DateTime.Now;
                        TIPS.Service.UserService us = new UserService();
                        PassworAuth PA = new PassworAuth();
                        //encode and save the password
                        user.Password = PA.base64Encode(user.Password);
                        user.IsActive = true;
                        TIPS.Entities.User userexists = us.GetUserByUserId(user.UserId);
                        if (userexists == null)    // to check if user already exists
                        {
                            us.CreateOrUpdateUser(user);
                        }
                        // ----------------Create Assess360 for student while registering. Its for grade (VI to XII)---------------------//
                        if (st.Grade == "VI" || st.Grade == "VII" || st.Grade == "VIII" || st.Grade == "IX" || st.Grade == "X" || st.Grade == "XI" || st.Grade == "XII")
                        {
                            CreateAssess360(st);
                        }
                        //Send Mail to parent for sending ParentPortal Id and Password
                        if (ConfigurationManager.AppSettings["SendEmail1"].ToString() == "true" && (st.Campus == "CHENNAI SWC" || st.Campus == "CBE SWC"))
                        {
                            Subject = "Welcome to TIPS - Student Registration Successfull"; // st.Subject; 
                            RecipientInfo = "Dear Parent,";
                            Body = "We are happy to add you in our TIPS family and your admission is approved. “" + st.Name + "” is admitted in Grade “" + st.Grade + "” section “" + st.Section + "” of our “" + st.Campus + "” campus and " + sex + " Id number will be “" + st.NewId + "”, Once the school re-open the confirmed Section will be informed.<br/><br/>";
                            Body = Body + "Your parent portal id is “" + "P" + st.NewId + "” and password will be “" + dob[0] + dob[1] + dob[2] + "” i.e. the DOB of the student as given in the records. You can access the same through http://portal.tipsglobal.org and <a href='https://play.google.com/store/apps/details?id=com.tips.parent'>Click here</a> to download our TIPS android mobile? ?application.";
                            Body = Body + "<br/><br/>For any queries, mail us at “" + campusemaildet.First().EmailId.ToString() + "”. ";
                            criteria.Clear();
                            criteria.Add("Campus", st.Campus);
                            criteria.Add("DocumentType", "Parent Portal Circular");
                            Dictionary<long, IList<CampusDocumentMaster>> PBCircularDoc = ads.GetCampusDocumentListwithCriteria(0, 10, string.Empty, string.Empty, criteria);
                            Attachment CircularAttach = null;
                            if (PBCircularDoc != null && PBCircularDoc.Count > 0 && PBCircularDoc.FirstOrDefault().Key > 0 && PBCircularDoc.FirstOrDefault().Value.Count > 0)
                            {
                                MemoryStream memStream = new MemoryStream(PBCircularDoc.FirstOrDefault().Value[0].ActualDocument);
                                CircularAttach = new Attachment(memStream, PBCircularDoc.FirstOrDefault().Value[0].DocumentName);  //Data posted from form
                                Body = Body + "<br/><br/>Please find the attached Parent Portal Circular.";
                            }
                            retValue = em.SendStudentRegistrationMail(st, st.EmailId, st.Campus, Subject, Body, MailBody, RecipientInfo, "Parent", CircularAttach);
                        }
                    }
                    else if (st.AdmissionStatus == "On Hold")
                    {
                        st.Status = "1";          // key for sentfor approval, on hold,callforinterview
                        st.Status1 = "2";         // key for newregistration,sentforapproval,onhold,callforinterview
                        ads.CreateOrUpdateStudentDetails(st);
                    }
                    else if (st.AdmissionStatus == "Declined")
                    {
                        ads.CreateOrUpdateStudentDetails(st);
                    }
                    else if (st.AdmissionStatus == "Inactive")
                    {
                        st.AdmissionStatus = "Inactive";
                        ads.CreateOrUpdateStudentDetails(st);
                        // To Deactivate a student from Assess360 using student primary key
                        DeactivateStudent(st.Id);
                    }
                    else
                    {
                        st.Status = "1";          // key for sentfor approval, on hold,callforinterview
                        st.Status1 = "2";         // key for newregistration,onhold,sentforapproval,callforinterview
                        ads.CreateOrUpdateStudentDetails(st);
                    }
                    return RedirectToAction("AdmissionManagement");
                }
                #endregion
                #region Sent for Clearance
                else if (Request.Form["btnsendclearance"] == "Submit For Clearance")
                {
                    st.FamilyDetailsList = null;
                    st.PastSchoolDetailsList = null;
                    st.ApproveAssignList = null;
                    st.PaymentDetailsList = null;
                    st.DocumentDetailsList = null;
                    criteria.Clear();
                    criteria.Add("StudentId", st.Id);
                    Dictionary<long, IList<AddressDetails>> add = ams1.GetAddressDetailsListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                    if (add.First().Value.Count != 0)
                    {
                        if (add.Count == 1) { st.AddressDetailsList[0].AddressType = "Primary Address"; st.AddressDetailsList[0].Id = add.First().Value[0].Id; }
                        else if (add.Count == 2)
                        {
                            st.AddressDetailsList[0].AddressType = "Primary Address"; st.AddressDetailsList[0].Id = add.First().Value[0].Id;
                            st.AddressDetailsList[1].AddressType = "Secondary Address"; st.AddressDetailsList[1].Id = add.First().Value[1].Id;
                        }
                        else { }
                    }
                    else
                    {
                        if (add.Count == 1) { st.AddressDetailsList[0].AddressType = "Primary Address"; }
                        else if (add.Count == 2) { st.AddressDetailsList[0].AddressType = "Primary Address"; st.AddressDetailsList[1].AddressType = "Secondary Address"; }
                        else { }
                    }
                    //st.PreRegNum = Convert.ToInt64(Session["preregno"]);                    //Pre-Registration number
                    //st.Id = Convert.ToInt64(Session["preregid"]);  // preregid;
                    //st.CreatedDate = DateTime.Now.Date.ToString("dd/MM/yyyy");
                    ViewBag.Date = Session["date"];  // date;
                    ViewBag.time = Session["time"];  // time; // st.CreatedTime;
                    st.Status1 = "2";         // key for newregistration,sentforapproval,onhold,callforinterview
                    // st.AdmissionStatus = "Sent For Approval";
                    // long preNum = Convert.ToInt64(Session["preregno"]);
                    // StudentTemplate stud= ams1.GetStudentDetailsByPreRegNo(preNum);
                    st.AdmissionStatus = "Sent For Clearance";
                    ViewBag.admissionstatus = st.AdmissionStatus;
                    info = " Application Number " + st.ApplicationNo + " has moved to Sent For Clearence ";
                    UpdateInbox(st.Campus, info, userid, st.PreRegNum);
                    ams1.CreateOrUpdateStudentDetails(st);
                    return RedirectToAction("AdmissionManagement");
                }
                #endregion
                #region Sent for Approval
                else if (Request.Form["btnsendapproval"] == "Submit For Approval")
                {
                    st.FamilyDetailsList = null;
                    st.PastSchoolDetailsList = null;
                    st.ApproveAssignList = null;
                    st.PaymentDetailsList = null;
                    st.DocumentDetailsList = null;
                    AdmissionManagementService ads = new AdmissionManagementService();
                    Dictionary<string, object> criteria3 = new Dictionary<string, object>();
                    criteria3.Add("PreRegNum", st.PreRegNum);
                    Dictionary<long, IList<PaymentDetails>> paymentdetails = ads.GetPaymentDetailsListWithPagingAndCriteria(0, 0, string.Empty, string.Empty, criteria3);
                    if (paymentdetails != null && paymentdetails.FirstOrDefault().Value.Count > 0 && paymentdetails.Count > 0)
                    {
                        int count = 0;
                        foreach (PaymentDetails pay in paymentdetails.FirstOrDefault().Value)
                        {
                            if (pay.FeeType == "Registration Fee" && pay.FeePaidStatus == "FeePaid") { count++; }
                        }
                        if (count < 1)
                        {
                            ViewBag.admissionstatus = Session["admstat"].ToString();
                            ViewBag.errmsg = "Not Paid";
                            return View(st);
                        }
                    }
                    else
                    {
                        ViewBag.admissionstatus = Session["admstat"].ToString();
                        ViewBag.errmsg = "Not Paid";
                        return View(st);
                    }
                    Dictionary<string, object> criteria4 = new Dictionary<string, object>();
                    criteria4.Add("StudentId", st.Id);
                    Dictionary<long, IList<AddressDetails>> add = ams1.GetAddressDetailsListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria4);
                    if (add.First().Value.Count != 0)
                    {
                        if (add.Count == 1) { st.AddressDetailsList[0].AddressType = "Primary Address"; st.AddressDetailsList[0].Id = add.First().Value[0].Id; }
                        else if (add.Count == 2)
                        {
                            st.AddressDetailsList[0].AddressType = "Primary Address"; st.AddressDetailsList[0].Id = add.First().Value[0].Id;
                            st.AddressDetailsList[1].AddressType = "Secondary Address"; st.AddressDetailsList[1].Id = add.First().Value[1].Id;
                        }
                        else { }
                    }
                    else
                    {
                        if (add.Count == 1) { st.AddressDetailsList[0].AddressType = "Primary Address"; }
                        else if (add.Count == 2) { st.AddressDetailsList[0].AddressType = "Primary Address"; st.AddressDetailsList[1].AddressType = "Secondary Address"; }
                        else { }
                    }
                    //st.PreRegNum = Convert.ToInt64(Session["preregno"]);                    //Pre-Registration number
                    //st.Id = Convert.ToInt64(Session["preregid"]);  // preregid;
                    //st.CreatedDate = DateTime.Now.Date.ToString("dd/MM/yyyy");
                    ViewBag.Date = Session["date"];  // date;
                    ViewBag.time = Session["time"];  // time; // st.CreatedTime;
                    st.AdmissionStatus = "Sent For Approval";
                    switch (st.BoardingType)
                    {
                        case "Day Scholar":
                            st.IsHosteller = false;
                            break;
                        case "Week Boarder":
                            st.IsHosteller = true;
                            break;
                        case "Residential":
                            st.IsHosteller = true;
                            break;
                        case "Full Boarder":
                            st.IsHosteller = true;
                            break;
                        default:
                            st.IsHosteller = false;
                            break;
                    }
                    st.Status = "1";          // key for sentfor approval, on hold,callforinterview
                    st.Status1 = "2";         // key for newregistration,sentforapproval,onhold,callforinterview
                    ViewBag.admissionstatus = "New Registration";               //Registration Status
                    StudentsReportBC srbc = new StudentsReportBC();
                    criteria.Clear();
                    criteria.Add("EmailType", "Head-Admission");
                    criteria.Add("IsMailNeeded", true);
                    Dictionary<long, IList<MISMailMaster>> MISMailMasterDetails = srbc.GetMISMailMasterListWithPaging(null, 9999, string.Empty, string.Empty, criteria);
                    if (MISMailMasterDetails != null && MISMailMasterDetails.Count > 0 && MISMailMasterDetails.FirstOrDefault().Value.Count > 0)
                    {
                        foreach (MISMailMaster item in MISMailMasterDetails.First().Value)
                        {
                            RecipientInfo = "Dear Team,";
                            Subject = "A new student application is available for Approval"; // st.Subject;
                            string Body1 = "A new application is available in Sent For Approval status and you are requested to process the same.<br><br>Campus: " + st.Campus + "<br>";
                            Body1 += "Application number: " + st.ApplicationNo + "<br>Pre-Reg number: " + st.PreRegNum + "<br>Applicant name: " + st.Name + "<br>Application grade: " + st.Grade + "<br>Applied Academic year: " + st.AcademicYear + "<br>";
                            retValue = em.SendStudentRegistrationMail(st, item.EmailId, st.Campus, Subject, Body1, MailBody, RecipientInfo, "Head-Admission", null);
                        }
                    }
                    info = " Application Number " + st.ApplicationNo + " has moved to Sent For Approval ";
                    UpdateInbox(st.Campus, info, userid, st.PreRegNum);
                    ads.CreateOrUpdateStudentDetails(st);
                    return RedirectToAction("AdmissionManagement");
                }
                #endregion
                else if (Request.Form["relationtype"] == "Add")
                {
                    AdmissionManagementService ads = new AdmissionManagementService();
                    ads.CreateOrUpdateStudentDetails(st);
                    return View();
                }
                else if (Request.Form["famsub"] == "Add")
                {
                    AdmissionManagementService ads = new AdmissionManagementService();
                    FamilyDetails fdet = new FamilyDetails();
                    fdet.PreRegNum = Convert.ToInt64(Session["preregno"]);
                    fdet.StudentId = st.Id;
                    fdet.FamilyDetailType = st.FamilyDetailsList[0].FamilyDetailType;
                    fdet.Name = st.FamilyDetailsList[0].Name;
                    fdet.Education = st.FamilyDetailsList[0].Education;
                    fdet.Age = st.FamilyDetailsList[0].Age;
                    fdet.Mobile = st.FamilyDetailsList[0].Mobile;
                    fdet.EmpType = st.FamilyDetailsList[0].EmpType;
                    fdet.Occupation = st.FamilyDetailsList[0].Occupation;
                    fdet.CompName = st.FamilyDetailsList[0].CompName;
                    fdet.CompAddress = st.FamilyDetailsList[0].CompAddress;
                    fdet.StayingWithChild = st.FamilyDetailsList[0].StayingWithChild;
                    fdet.Email = st.FamilyDetailsList[0].Email;
                    fdet.TransportReq = st.FamilyDetailsList[0].TransportReq;
                    info = " Student Application  Number  " + st.ApplicationNo + " has taken For Approvel";
                    UpdateInbox(st.Campus, info, userid, st.PreRegNum);
                    ads.CreateOrUpdateFamilyDetails(fdet);
                    return View();
                }
                else if (Request.Form["PastAdd"] == "Add")
                {
                    AdmissionManagementService ads = new AdmissionManagementService();
                    PastSchoolDetails pastadd = new PastSchoolDetails();
                    pastadd.PreRegNum = st.PreRegNum;// Convert.ToInt64(Session["preregno"]);
                    pastadd.StudentId = st.Id;
                    pastadd.FromDate = st.PastSchoolDetailsList[0].FromDate.ToString();
                    pastadd.ToDate = st.PastSchoolDetailsList[0].ToDate;
                    pastadd.SchoolName = st.PastSchoolDetailsList[0].SchoolName;
                    pastadd.City = st.PastSchoolDetailsList[0].City;
                    pastadd.FromGrade = st.PastSchoolDetailsList[0].FromGrade;
                    pastadd.ToGrade = st.PastSchoolDetailsList[0].ToGrade;

                    ads.CreateOrUpdatePastSchoolDetails(pastadd);
                    return View();
                }
                else
                {
                    return View(st);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        //public ActionResult NewRegistration(StudentTemplate st, FamilyDetails fd, HttpPostedFileBase file1)
        //{
        //    try
        //    {
        //        string RecipientInfo = "", Subject = "", Body = "", MailBody = ""; bool retValue;
        //        string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
        //        AdmissionManagementService ams1 = new AdmissionManagementService();
        //        EmailHelper em = new EmailHelper();
        //        MastersService ms = new MastersService();
        //        IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
        //        Dictionary<string, object> criteria = new Dictionary<string, object>();
        //        Dictionary<long, IList<PreRegDetails>> prd = ams1.GetPreRegDetailsListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, null);
        //        Dictionary<long, IList<AcademicyrMaster>> AcademicyrMaster = ms.GetAcademicyrMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
        //        Dictionary<long, IList<GradeMaster>> GradeMaster = ms.GetGradeMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
        //        Dictionary<long, IList<BloodGroupMaster>> BloodGroupMaster = ms.GetBloodGroupMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
        //        Dictionary<long, IList<FeeTypeMaster>> FeeTypeMaster = ms.GetFeeTypeMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
        //        Dictionary<long, IList<ModeOfPaymentMaster>> ModeOfPaymentMaster = ms.GetModeOfPaymentMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
        //        Dictionary<long, IList<FeeStructureYearMaster>> FeeStructureYearMaster = ms.GetFeeStructureYearMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
        //        Dictionary<long, IList<SectionMaster>> SectionMaster = ms.GetSectionMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
        //        var usrcmp = Session["UserCampus"] as IEnumerable<string>;
        //        if (usrcmp != null && usrcmp.First() != null)            // to check if the usrcmp obj is null or with data
        //        {
        //            criteria.Add("Name", usrcmp);
        //        }
        //        Dictionary<long, IList<CampusMaster>> CampusMaster = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
        //        criteria.Clear();
        //        Dictionary<long, IList<RelationshipMaster>> RelationshipMaster = ms.GetRelationshipMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
        //        criteria.Clear();
        //        criteria.Add("DocumentFor", "Student");
        //        Dictionary<long, IList<DocumentTypeMaster>> DocumentTypeMaster = ms.GetDocumentTypeMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
        //        criteria.Clear();
        //        ViewBag.campusddl = CampusMaster.First().Value;
        //        ViewBag.familyddl = RelationshipMaster.First().Value;
        //        ViewBag.documentddl = DocumentTypeMaster.First().Value;
        //        ViewBag.acadyrddl = AcademicyrMaster.First().Value;
        //        ViewBag.gradeddl = GradeMaster.First().Value;
        //        ViewBag.bloodgrpddl = BloodGroupMaster.First().Value;
        //        ViewBag.feetypeddl = FeeTypeMaster.First().Value;
        //        ViewBag.modeofpmtddl = ModeOfPaymentMaster.First().Value;
        //        ViewBag.feestructyrddl = FeeStructureYearMaster.First().Value;
        //        ViewBag.sectionddl = SectionMaster.First().Value;
        //        ViewBag.RouteNo = st.VanNo;
        //        ViewBag.grade = Session["grad"]; // grad;
        //        ViewBag.nam = Session["nam"]; // nam;
        //        ViewBag.gender = Session["gender"]; // gender;
        //        ViewBag.campus = Session["campus"]; // campus;
        //        ViewBag.processby = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
        //        if (Session["userrole"].ToString() == "ADM-APP") { ViewBag.userrole = "Approver"; }
        //        MailBody = GetBodyofMail();
        //        Dictionary<string, object> criteria1 = new Dictionary<string, object>();
        //        criteria1.Add("PreRegNum", Convert.ToInt64(Session["preregno"]));
        //        Dictionary<long, IList<TIPS.Entities.AdmissionEntities.StudentTemplateView>> sdt = ams1.GetStudentTemplateViewListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria1);
        //        CampusGradeddl(st.Campus);
        //        if (sdt.First().Value.Count == 0)
        //        {
        //            Session["preregid"] = 0;
        //        }
        //        else
        //        {
        //            Session["preregid"] = sdt.First().Value[0].Id;
        //        }
        //        ViewBag.preregno = Session["preregno"];
        //        if (!string.IsNullOrEmpty(st.CreatedDate))
        //            st.CreatedDateNew = DateTime.Parse(st.CreatedDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
        //        if (Request.Form["AddPayment"] == "AddPayment")
        //        {
        //            criteria.Clear();
        //            criteria.Add("FeeType", st.PaymentDetailsList[0].FeeType);
        //            criteria.Add("PreRegNum", Convert.ToInt64(Session["preregno"]));
        //            //criteria.Add("ReferenceNo", refno);
        //            Dictionary<long, IList<PaymentDetails>> PaymentDetails = ams1.GetPaymentDetailsListWithPagingAndCriteria(null, null, null, null, criteria);
        //            if (PaymentDetails.First().Value.Count == 0)
        //            {
        //                PaymentDetails pmtadd = new PaymentDetails();
        //                pmtadd.PreRegNum = Convert.ToInt64(Session["preregno"]);
        //                pmtadd.StudentId = 0;
        //                pmtadd.FeeType = st.PaymentDetailsList[0].FeeType;
        //                pmtadd.ModeOfPayment = st.PaymentDetailsList[0].ModeOfPayment;
        //                pmtadd.Amount = st.PaymentDetailsList[0].Amount;
        //                pmtadd.ReferenceNo = st.PaymentDetailsList[0].ReferenceNo;
        //                ams1.CreateOrUpdatePaymentDetails(pmtadd);
        //                if (st != null && st.PaymentDetailsList[0].FeeType == "Pre-Registration Amount")
        //                {
        //                    st.FamilyDetailsList = null;
        //                    st.PastSchoolDetailsList = null;
        //                    st.ApproveAssignList = null;
        //                    st.DocumentDetailsList = null;
        //                    criteria.Clear();
        //                    criteria.Add("StudentId", st.Id);
        //                    Dictionary<long, IList<AddressDetails>> add = ams1.GetAddressDetailsListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
        //                    if (add.First().Value.Count != 0)
        //                    {
        //                        if (add.Count == 1) { st.AddressDetailsList[0].AddressType = "Primary Address"; st.AddressDetailsList[0].Id = add.First().Value[0].Id; }
        //                        else if (add.Count == 2)
        //                        {
        //                            st.AddressDetailsList[0].AddressType = "Primary Address"; st.AddressDetailsList[0].Id = add.First().Value[0].Id;
        //                            st.AddressDetailsList[1].AddressType = "Secondary Address"; st.AddressDetailsList[1].Id = add.First().Value[1].Id;
        //                        }
        //                        else { }
        //                    }
        //                    else
        //                    {
        //                        if (add.Count == 1) { st.AddressDetailsList[0].AddressType = "Primary Address"; }
        //                        else if (add.Count == 2) { st.AddressDetailsList[0].AddressType = "Primary Address"; st.AddressDetailsList[1].AddressType = "Secondary Address"; }
        //                        else { }
        //                    }
        //                    st.PreRegNum = Convert.ToInt64(Session["preregno"]);                    //Pre-Registration number
        //                    //st.Id = Convert.ToInt64(Session["preregid"]);  // preregid commented by micheal Nov-15;
        //                    //st.CreatedDate = DateTime.Now.Date.ToString("dd/MM/yyyy");
        //                    st.AdmissionStatus = "New Registration";
        //                    st.Status = "1";          // key for sentfor approval, on hold,callforinterview
        //                    st.Status1 = "2";         // key for newregistration,sentforapproval,onhold,callforinterview
        //                    st.PaymentDetailsList = null;
        //                    ams1.CreateOrUpdateStudentDetails(st);
        //                    ViewBag.Tabselected = "Payment";
        //                }
        //                ViewBag.admissionstatus = st.AdmissionStatus;
        //                return View(st);
        //            }
        //            else
        //            {
        //                ViewBag.errmsg = "Paid";
        //                ViewBag.admissionstatus = st.AdmissionStatus;
        //                return View(st);
        //            }
        //        }
        //        #region old
        //        else if (Request.Form["DocUpload"] == "Upload")
        //        {
        //            ViewBag.tabindex = 1;
        //            if (Session["studentmgmt"] == "yes")
        //            {
        //                ViewBag.Studentmgmt = "yes";
        //            }
        //            string path = file1.InputStream.ToString();
        //            byte[] imageSize = new byte[file1.ContentLength];
        //            file1.InputStream.Read(imageSize, 0, (int)file1.ContentLength);
        //            UploadedFiles fu1 = new UploadedFiles();
        //            fu1.DocumentData = imageSize;
        //            fu1.DocumentName = file1.FileName;
        //            //    fu1.DocumentType = st.UploadedFilesList[0].DocumentType;
        //            fu1.DocumentType = st.DocumentDetailsList[0].DocumentType;
        //            fu1.DocumentSize = file1.ContentLength.ToString();
        //            fu1.UploadedDate = DateTime.Now.Date.ToString("dd/MM/yyyy");
        //            //fu1.Name = st.UploadedFilesList[0].Name;
        //            //fu1.Phone = st.UploadedFilesList[0].Phone;
        //            fu1.DocumentFor = "Student";
        //            fu1.Name = st.DocumentDetailsList[0].Name;
        //            fu1.Phone = st.DocumentDetailsList[0].Phone;
        //            fu1.PreRegNum = Convert.ToInt64(Session["preregno"]);              // Pre Registration number
        //            ViewBag.Date = Session["date"];  // date;
        //            ViewBag.time = Session["time"]; // time;
        //            ViewBag.admissionstatus = Session["admstat"]; /// admstat;
        //            AdmissionManagementService ams = new AdmissionManagementService();
        //            ams.CreateOrUpdateUploadedFiles(fu1);
        //            ViewData["imageid"] = fu1.Id;
        //            return View();
        //        }
        //        else if (Request.Form["btnSave"] == "Save")
        //        {
        //            Session["tabfreeze"] = "no";
        //            st.FamilyDetailsList = null;
        //            st.PastSchoolDetailsList = null;
        //            st.ApproveAssignList = null;
        //            st.PaymentDetailsList = null;
        //            st.DocumentDetailsList = null;
        //            if (Session["studentmgmt"].ToString() == "yes")
        //            {
        //                ViewBag.Studentmgmt = "yes";
        //                ViewBag.admissionstatus = "Registered";               //Registration Status
        //                AdmissionManagementService ads1 = new AdmissionManagementService();
        //                st.AddressDetailsList[0].AddressType = "Primary Address";
        //                st.AddressDetailsList[1].AddressType = "Secondary Address";
        //                st.PreRegNum = Convert.ToInt64(Session["preregno"]);                    //Pre-Registration number
        //                //st.Id = Convert.ToInt64(Session["preregid"]); // preregid; commented by micheal Nov-15
        //                if (Session["date"] == null)
        //                {
        //                    Session["date"] = DateTime.Now.ToString("dd/MM/yyyy");
        //                }
        //                if (Session["time"] == null)
        //                {
        //                    Session["time"] = DateTime.Now.ToString("hh:mm:ss");
        //                }
        //                //  st.CreatedDate = Session["date"].ToString(); // date;
        //                //  st.CreatedTime = Session["time"].ToString(); // time;// DateTime.Now.ToString("hh:mm:ss");
        //                st.AdmissionStatus = "Registered";
        //                switch (st.BoardingType)
        //                {
        //                    case "Day Scholar":
        //                        st.IsHosteller = false;
        //                        break;
        //                    case "Week Boarder":
        //                        st.IsHosteller = true;
        //                        break;
        //                    case "Residential":
        //                        st.IsHosteller = true;
        //                        break;
        //                    case "Full Boarder":
        //                        st.IsHosteller = true;
        //                        break;
        //                    default:
        //                        st.IsHosteller = false;
        //                        break;
        //                }
        //                ViewBag.Date = Session["date"]; // date;
        //                ViewBag.time = Session["time"]; // time;
        //                ads1.CreateOrUpdateStudentDetails(st);
        //            }
        //            else
        //            {
        //                Dictionary<string, object> PaymentCriteria = new Dictionary<string, object>();
        //                PaymentCriteria.Add("PreRegNum", Convert.ToInt64(Session["preregno"]));
        //                Dictionary<long, IList<PaymentDetails>> paymentdetails = ams1.GetPaymentDetailsListWithPagingAndCriteria(0, 0, string.Empty, string.Empty, PaymentCriteria);
        //                if (paymentdetails != null && paymentdetails.FirstOrDefault().Value.Count > 0 && paymentdetails.Count > 0)
        //                {
        //                    int count = 0;
        //                    foreach (PaymentDetails pay in paymentdetails.FirstOrDefault().Value)
        //                    {
        //                        if (pay.FeeType == "Pre-Registration Amount") { count++; }
        //                    }
        //                    ViewBag.admissionstatus = count < 1 ? "New Enquiry" : "New Registration";
        //                }
        //                else { ViewBag.admissionstatus = "New Enquiry"; }

        //                AdmissionManagementService ads = new AdmissionManagementService();
        //                Dictionary<string, object> criteria4 = new Dictionary<string, object>();
        //                criteria4.Add("StudentId", st.Id);
        //                Dictionary<long, IList<AddressDetails>> add = ams1.GetAddressDetailsListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria4);
        //                if (add.First().Value.Count != 0)
        //                {
        //                    if (add.First().Value.Count == 1) { st.AddressDetailsList[0].AddressType = "Primary Address"; st.AddressDetailsList[0].Id = add.First().Value[0].Id; }
        //                    else if (add.First().Value.Count == 2)  // (add.Count == 2)
        //                    {
        //                        st.AddressDetailsList[0].AddressType = "Primary Address"; st.AddressDetailsList[0].Id = add.First().Value[0].Id;
        //                        st.AddressDetailsList[1].AddressType = "Secondary Address"; st.AddressDetailsList[1].Id = add.First().Value[1].Id;
        //                    }
        //                    else { }
        //                }
        //                else
        //                {
        //                    if (add.First().Value.Count == 1) { st.AddressDetailsList[0].AddressType = "Primary Address"; }
        //                    else if (add.First().Value.Count == 2) { st.AddressDetailsList[0].AddressType = "Primary Address"; st.AddressDetailsList[1].AddressType = "Secondary Address"; }
        //                    else { }
        //                }
        //                st.PreRegNum = Convert.ToInt64(Session["preregno"]);                    //Pre-Registration number
        //                //st.Id = Convert.ToInt64(Session["preregid"]);  // preregid; commented by micheal nov-15
        //                st.AdmissionStatus = ViewBag.admissionstatus;
        //                switch (st.BoardingType)
        //                {
        //                    case "Day Scholar":
        //                        st.IsHosteller = false;
        //                        break;
        //                    case "Week Boarder":
        //                        st.IsHosteller = true;
        //                        break;
        //                    case "Residential":
        //                        st.IsHosteller = true;
        //                        break;
        //                    case "Full Boarder":
        //                        st.IsHosteller = true;
        //                        break;
        //                    default:
        //                        st.IsHosteller = false;
        //                        break;
        //                }
        //                st.Status1 = "2";         // key for newregistration,sentforapproval,onhold,callforinterview
        //                ViewBag.Date = Session["date"];  // date;
        //                ViewBag.time = Session["time"]; // time;                     
        //                //   if (st.Id == 0)    // to chk 
        //                if (st.JoiningAcademicYear == null)
        //                {
        //                    st.JoiningAcademicYear = st.AcademicYear; // to add only the first time
        //                }
        //                if (st.Id == 0)    // to send email only once
        //                {
        //                    //send Mail To Parents with template added by micheal
        //                    IList<CampusEmailId> campusemaildet = GetEmailIdByCampus(st.Campus, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
        //                    if (ConfigurationManager.AppSettings["SendEmail1"].ToString() == "true")
        //                    {
        //                        RecipientInfo = "Dear Parent,";
        //                        Subject = "Welcome to TIPS - Student Registration In Progress"; // st.Subject;
        //                        Body = "Thanks for registering with TIPS school and your application is sent for processing. Your online application number is " + st.PreRegNum + " and please refer this number for all further communication. For any queries, mail us at " + campusemaildet.First().EmailId.ToString() + ".";
        //                        if (!string.IsNullOrEmpty(st.EmailId))
        //                            retValue = em.SendStudentRegistrationMail(st, st.EmailId, st.Campus, Subject, Body, MailBody, RecipientInfo, "Parent", null);
        //                        //Send Mail to Managment
        //                        RecipientInfo = "Dear Team,";
        //                        Subject = "A new application is available for processing"; // st.Subject;
        //                        string Body1 = "A new application is available for processing and you are requested to process the same.<br><br>Campus: " + st.Campus + "<br>";
        //                        Body1 += "Application number: " + st.ApplicationNo + "<br>Pre-Reg number: " + st.PreRegNum + "<br>Applicant name: " + st.Name + "<br>Application grade: " + st.Grade + "<br>Applied Academic year: " + st.AcademicYear + "<br>";
        //                        retValue = em.SendStudentRegistrationMail(st, null, st.Campus, Subject, Body1, MailBody, RecipientInfo, "Management", null);
        //                    }
        //                }
        //                if (!string.IsNullOrEmpty(st.CreatedDate))
        //                    st.CreatedDateNew = DateTime.Parse(st.CreatedDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
        //                ads.CreateOrUpdateStudentDetails(st);
        //            }
        //            return View();
        //        }
        //        else if (Request.Form["btnApprove"] == "Update")
        //        {
        //            st.FamilyDetailsList = null;
        //            st.PastSchoolDetailsList = null;
        //            st.ApproveAssignList = null;
        //            st.PaymentDetailsList = null;
        //            st.DocumentDetailsList = null;
        //            AdmissionManagementService ads = new AdmissionManagementService();
        //            Dictionary<string, object> criteria4 = new Dictionary<string, object>();
        //            criteria4.Add("StudentId", Convert.ToInt64(Session["preregid"]));
        //            Dictionary<long, IList<AddressDetails>> add = ams1.GetAddressDetailsListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria4);
        //            if (add.First().Value.Count != 0)
        //            {
        //                if (add.Count == 1) { st.AddressDetailsList[0].AddressType = "Primary Address"; st.AddressDetailsList[0].Id = add.First().Value[0].Id; }
        //                else if (add.Count == 2)
        //                {
        //                    st.AddressDetailsList[0].AddressType = "Primary Address"; st.AddressDetailsList[0].Id = add.First().Value[0].Id;
        //                    st.AddressDetailsList[1].AddressType = "Secondary Address"; st.AddressDetailsList[1].Id = add.First().Value[1].Id;
        //                }
        //                else { }
        //            }
        //            else
        //            {
        //                if (add.Count == 1) { st.AddressDetailsList[0].AddressType = "Primary Address"; }
        //                else if (add.Count == 2) { st.AddressDetailsList[0].AddressType = "Primary Address"; st.AddressDetailsList[1].AddressType = "Secondary Address"; }
        //                else { }
        //            }
        //            st.PreRegNum = Convert.ToInt64(Session["preregno"]);                    //Pre-Registration number
        //            // st.Id = Convert.ToInt64(Session["preregid"]); // preregid; commented by micheal nov-15

        //            ViewBag.Date = Session["date"]; // date;
        //            ViewBag.time = Session["time"];  // time; // st.CreatedTime;
        //            switch (st.BoardingType)
        //            {
        //                case "Day Scholar":
        //                    st.IsHosteller = false;
        //                    break;
        //                case "Week Boarder":
        //                    st.IsHosteller = true;
        //                    break;
        //                case "Residential":
        //                    st.IsHosteller = true;
        //                    break;
        //                case "Full Boarder":
        //                    st.IsHosteller = true;
        //                    break;
        //                default:
        //                    st.IsHosteller = false;
        //                    break;
        //            }
        //            if (st.AdmissionStatus == "Registered")
        //            {
        //                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
        //                IList<CampusEmailId> campusemaildet = GetEmailIdByCampus(st.Campus, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
        //                string[] dob = st.DOB.ToString().Split('/');   // dateofbirth split
        //                Dictionary<string, object> criteria2 = new Dictionary<string, object>();
        //                criteria2.Add("PreRegNum", Convert.ToInt64(Session["preregno"]));   // to check if student is already registered or not while changing from inactive to registered
        //                Dictionary<long, IList<StudentTemplateView>> IdCheck = ads.GetStudentTemplateViewListWithEQsearchCriteria(0, 0, string.Empty, string.Empty, criteria2);
        //                if (IdCheck.First().Value[0].NewId == null)
        //                {
        //                    var w = 0;            // grade
        //                    if (st.Grade == "I")
        //                    {
        //                        w = 1;
        //                    }
        //                    else if (st.Grade == "II")
        //                    {
        //                        w = 2;
        //                    }
        //                    else if (st.Grade == "III")
        //                    {
        //                        w = 3;
        //                    }
        //                    else if (st.Grade == "IV")
        //                    {
        //                        w = 4;
        //                    }
        //                    else if (st.Grade == "V")
        //                    {
        //                        w = 5;
        //                    }
        //                    else if (st.Grade == "VI")
        //                    {
        //                        w = 6;
        //                    }
        //                    else if (st.Grade == "VII")
        //                    {
        //                        w = 7;
        //                    }
        //                    else if (st.Grade == "VIII")
        //                    {
        //                        w = 8;
        //                    }
        //                    else if (st.Grade == "IX")
        //                    {
        //                        w = 9;
        //                    }
        //                    else if (st.Grade == "X")
        //                    {
        //                        w = 10;
        //                    }
        //                    else if (st.Grade == "XI")
        //                    {
        //                        w = 11;
        //                    }
        //                    else if (st.Grade == "XII")
        //                    {
        //                        w = 12;
        //                    }
        //                    else if (st.Grade == "UKG")
        //                    {
        //                        w = 0;
        //                    }
        //                    else if (st.Grade == "LKG")
        //                    {
        //                        w = -1;
        //                    }
        //                    else if (st.Grade == "PreKG")
        //                    {
        //                        w = -2;
        //                    }
        //                    else { w = -3; }
        //                    string sex = "";
        //                    if (st.Gender == "Male") { sex = "his"; }
        //                    else if (st.Gender == "Female") { sex = "her"; }
        //                    if (st.Campus == "TIPS ERODE")
        //                    {
        //                        long cnt = ads.ErodeStudentIdCount(st.Grade, st.Campus, st.FeeStructYear);            //for getting the maximum id no
        //                        cnt = cnt + 1;
        //                        string stdntcnt = "";
        //                        if (cnt.ToString().Length == 1) { stdntcnt = "000" + cnt.ToString(); }
        //                        else if (cnt.ToString().Length == 2) { stdntcnt = "00" + cnt.ToString(); }
        //                        else if (cnt.ToString().Length == 3) { stdntcnt = "0" + cnt.ToString(); }
        //                        string e = "";
        //                        if (st.Grade == "UKG")
        //                        {
        //                            e = "K2";
        //                        }
        //                        else if (st.Grade == "LKG")
        //                        {
        //                            e = "K1";
        //                        }
        //                        else if (st.Grade == "PreKG")
        //                        {
        //                            e = "P1";
        //                        }
        //                        else
        //                        {
        //                            e = w.ToString();
        //                            if (e.Length == 1)
        //                            { e = "0" + e; }
        //                        }
        //                        st.NewId = DateTime.Now.Year.ToString().Substring(2, 2) + e + stdntcnt;
        //                        RecipientInfo = "Dear Parent,";
        //                        Body = "We are happy to add you in our TIPS family and your admission is approved. “" + st.Name + "” is admitted in Grade “" + st.Grade + "” section “" + st.Section + "” of our “" + st.Campus + "” campus and " + sex + " Id number will be “" + st.NewId + "”, Once the school re-open the confirmed Section will be informed.<br/><br/>";
        //                        Body = Body + "Your parent portal id is “" + "P" + st.NewId + "” and password will be “" + dob[0] + dob[1] + dob[2] + "” i.e. the DOB of the student as given in the records. You can access the same through http://portal.tipsglobal.org and <a href='https://play.google.com/store/apps/details?id=com.tips.parent'>Click here</a> to download our TIPS android mobile​ ​application.";
        //                        Body = Body + "<br/><br/>For any queries, mail us at “" + campusemaildet.First().EmailId.ToString() + "”. ";
        //                    }
        //                    else if (st.Campus == "TIPS SALEM")
        //                    {
        //                        long cnt = ads.ErodeStudentIdCount(st.Grade, st.Campus, st.FeeStructYear);            //for getting the maximum id no
        //                        cnt = cnt + 1;
        //                        string stdntcnt = "";
        //                        if (cnt.ToString().Length == 1) { stdntcnt = "000" + cnt.ToString(); }
        //                        else if (cnt.ToString().Length == 2) { stdntcnt = "00" + cnt.ToString(); }
        //                        else if (cnt.ToString().Length == 3) { stdntcnt = "0" + cnt.ToString(); }
        //                        string e = "";
        //                        if (st.Grade == "UKG")
        //                        {
        //                            e = "K2";
        //                        }
        //                        else if (st.Grade == "LKG")
        //                        {
        //                            e = "K1";
        //                        }
        //                        else if (st.Grade == "PreKG")
        //                        {
        //                            e = "P1";
        //                        }
        //                        else
        //                        {
        //                            e = w.ToString();
        //                            if (e.Length == 1)
        //                            { e = "0" + e; }
        //                        }
        //                        st.NewId = "S" + DateTime.Now.Year.ToString().Substring(2, 2) + e + stdntcnt;
        //                        RecipientInfo = "Dear Parent,";
        //                        Body = "We are happy to add you in our TIPS family and your admission is approved. “" + st.Name + "” is admitted in Grade “" + st.Grade + "” section “" + st.Section + "” of our “" + st.Campus + "” campus and " + sex + " Id number will be “" + st.NewId + "”, Once the school re-open the confirmed Section will be informed.<br/><br/>";
        //                        Body = Body + "Your parent portal id is “" + "P" + st.NewId + "” and password will be “" + dob[0] + dob[1] + dob[2] + "” i.e. the DOB of the student as given in the records. You can access the same through http://portal.tipsglobal.org and <a href='https://play.google.com/store/apps/details?id=com.tips.parent'>Click here</a> to download our TIPS android mobile​ ​application.";
        //                        Body = Body + "<br/><br/>For any queries, mail us at “" + campusemaildet.First().EmailId.ToString() + "”. ";
        //                    }
        //                    else
        //                    {
        //                        string cam = "";
        //                        if ((st.Campus == "CHENNAI CITY") || (st.Campus == "CHENNAI MAIN"))
        //                        { cam = "CHENNAI"; }
        //                        else { cam = st.Campus; }
        //                        long count = ads.StudentIdCount(st.Grade, st.FeeStructYear, cam);            //for getting the maximum id no
        //                        count = count + 1;

        //                        string z = count.ToString();
        //                        if (z.Length == 1)
        //                        {
        //                            z = "00" + z;
        //                        }
        //                        else if (z.Length == 2)
        //                        {
        //                            z = "0" + z;
        //                        }
        //                        var acadyr = st.AcademicYear.Substring(2, 2);

        //                        string x = (Convert.ToInt32(acadyr) - Convert.ToInt32(w)).ToString();
        //                        if (x.Length == 1)
        //                        {
        //                            x = "0" + x;
        //                        }
        //                        //switch case
        //                        switch (st.Campus)
        //                        {
        //                            case "IB MAIN":
        //                                st.NewId = "I" + st.FeeStructYear + "-" + x + "-" + z;
        //                                break;
        //                            case "IB KG":
        //                                st.NewId = "I" + st.FeeStructYear + "-" + x + "-" + z;
        //                                break;
        //                            case "ERNAKULAM":
        //                                st.NewId = "E" + st.FeeStructYear + "-" + x + "-" + z;
        //                                break;
        //                            case "ERNAKULAM KG":
        //                                st.NewId = "E" + st.FeeStructYear + "-" + x + "-" + z;
        //                                break;
        //                            case "TIRUPUR":
        //                                st.NewId = "T" + st.FeeStructYear + "-" + x + "-" + z;
        //                                break;
        //                            case "TIRUPUR KG":
        //                                st.NewId = "T" + st.FeeStructYear + "-" + x + "-" + z;
        //                                break;
        //                            case "CHENNAI CITY":
        //                                st.NewId = "C" + st.FeeStructYear + "-" + x + "-" + z;
        //                                break;
        //                            case "CHENNAI MAIN":
        //                                st.NewId = "C" + st.FeeStructYear + "-" + x + "-" + z;
        //                                break;
        //                            case "KARUR":
        //                                st.NewId = "K" + st.FeeStructYear + "-" + x + "-" + z;
        //                                break;
        //                            case "KARUR KG":
        //                                st.NewId = "K" + st.FeeStructYear + "-" + x + "-" + z;
        //                                break;
        //                            case "TIPS SARAN":
        //                                st.NewId = "H" + st.FeeStructYear + "-" + x + "-" + z;
        //                                break;
        //                            case "TIPS ERODE":
        //                                //for erode id generation done above 
        //                                break;
        //                            default:
        //                                st.NewId = ""; break;
        //                        }
        //                        RecipientInfo = "Dear Parent,";
        //                        Body = "We are happy to add you in our TIPS family and your admission is approved. “" + st.Name + "” is admitted in Grade “" + st.Grade + "” section “" + st.Section + "” of our “" + st.Campus + "” campus and " + sex + " Id number will be “" + st.NewId + "”, Once the school re-open the confirmed Section will be informed.<br/><br/>";
        //                        Body = Body + "Your parent portal id is “" + "P" + st.NewId + "” and password will be “" + dob[0] + dob[1] + dob[2] + "” i.e. the DOB of the student as given in the records. You can access the same through http://portal.tipsglobal.org and <a href='https://play.google.com/store/apps/details?id=com.tips.parent'>Click here</a> to download our TIPS android mobile​ ​application.";
        //                        Body = Body + "<br/><br/>For any queries, mail us at “" + campusemaildet.First().EmailId.ToString() + "”. ";
        //                    }
        //                }
        //                st.AdmissionStatus = "Registered";    //Admission Registered By Admin                   
        //                //   st.Status = "1";          // key for sentfor approval, on hold,callforinterview                
        //                ads.CreateOrUpdateStudentDetails(st);
        //                Session["registered"] = "yes";
        //                //registered = "yes";
        //                Session["regid"] = st.NewId;
        //                // ---------------------------- creating Parent userid -----------------------------------------
        //                //    string[] dob = st.DOB.ToString().Split('/');
        //                TIPS.Entities.User user = new TIPS.Entities.User();
        //                user.UserId = "P" + st.NewId;
        //                user.Password = dob[0] + dob[1] + dob[2];
        //                user.Campus = st.Campus;
        //                user.UserType = "Parent";
        //                user.CreatedDate = DateTime.Now;
        //                user.ModifiedDate = DateTime.Now;
        //                TIPS.Service.UserService us = new UserService();
        //                PassworAuth PA = new PassworAuth();
        //                //encode and save the password
        //                user.Password = PA.base64Encode(user.Password);
        //                user.IsActive = true;
        //                TIPS.Entities.User userexists = us.GetUserByUserId(user.UserId);
        //                if (userexists == null)    // to check if user already exists
        //                {
        //                    us.CreateOrUpdateUser(user);
        //                }
        //                // ----------------Create Assess360 for student while registering. Its for grade (VI to XII)---------------------//
        //                if (st.Grade == "VI" || st.Grade == "VII" || st.Grade == "VIII" || st.Grade == "IX" || st.Grade == "X" || st.Grade == "XI" || st.Grade == "XII")
        //                {
        //                    CreateAssess360(st);
        //                }
        //                //Send Mail to parent for sending ParentPortal Id and Password
        //                Subject = "Welcome to TIPS - Student Registration Successfull"; // st.Subject;   
        //                if (ConfigurationManager.AppSettings["SendEmail1"].ToString() == "true")
        //                {
        //                    criteria.Clear();
        //                    criteria.Add("Campus", st.Campus);
        //                    criteria.Add("DocumentType", "Parent Portal Circular");
        //                    Dictionary<long, IList<CampusDocumentMaster>> PBCircularDoc = ads.GetCampusDocumentListwithCriteria(0, 10, string.Empty, string.Empty, criteria);
        //                    Attachment CircularAttach = null;
        //                    if (PBCircularDoc != null && PBCircularDoc.Count > 0 && PBCircularDoc.FirstOrDefault().Key > 0 && PBCircularDoc.FirstOrDefault().Value.Count > 0)
        //                    {
        //                        MemoryStream memStream = new MemoryStream(PBCircularDoc.FirstOrDefault().Value[0].ActualDocument);
        //                        CircularAttach = new Attachment(memStream, PBCircularDoc.FirstOrDefault().Value[0].DocumentName);  //Data posted from form
        //                        Body = Body + "<br/><br/>Please find the attached Parent Portal Circular.";
        //                    }
        //                    retValue = em.SendStudentRegistrationMail(st, st.EmailId, st.Campus, Subject, Body, MailBody, RecipientInfo, "Parent", CircularAttach);
        //                }
        //            }
        //            else if (st.AdmissionStatus == "On Hold")
        //            {
        //                st.Status = "1";          // key for sentfor approval, on hold,callforinterview
        //                st.Status1 = "2";         // key for newregistration,sentforapproval,onhold,callforinterview
        //                ads.CreateOrUpdateStudentDetails(st);
        //            }
        //            else if (st.AdmissionStatus == "Declined")
        //            {
        //                ads.CreateOrUpdateStudentDetails(st);
        //            }
        //            else if (st.AdmissionStatus == "Inactive")
        //            {
        //                st.AdmissionStatus = "Inactive";
        //                ads.CreateOrUpdateStudentDetails(st);
        //                // To Deactivate a student from Assess360 using student primary key
        //                DeactivateStudent(st.Id);
        //            }
        //            else
        //            {
        //                st.Status = "1";          // key for sentfor approval, on hold,callforinterview
        //                st.Status1 = "2";         // key for newregistration,onhold,sentforapproval,callforinterview
        //                ads.CreateOrUpdateStudentDetails(st);
        //            }
        //            return RedirectToAction("AdmissionManagement");
        //        }
        //        #endregion
        //        else if (Request.Form["btnsendclearance"] == "Submit For Clearance")
        //        {
        //            st.FamilyDetailsList = null;
        //            st.PastSchoolDetailsList = null;
        //            st.ApproveAssignList = null;
        //            st.PaymentDetailsList = null;
        //            st.DocumentDetailsList = null;
        //            criteria.Clear();
        //            criteria.Add("StudentId", st.Id);
        //            Dictionary<long, IList<AddressDetails>> add = ams1.GetAddressDetailsListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
        //            if (add.First().Value.Count != 0)
        //            {
        //                if (add.Count == 1) { st.AddressDetailsList[0].AddressType = "Primary Address"; st.AddressDetailsList[0].Id = add.First().Value[0].Id; }
        //                else if (add.Count == 2)
        //                {
        //                    st.AddressDetailsList[0].AddressType = "Primary Address"; st.AddressDetailsList[0].Id = add.First().Value[0].Id;
        //                    st.AddressDetailsList[1].AddressType = "Secondary Address"; st.AddressDetailsList[1].Id = add.First().Value[1].Id;
        //                }
        //                else { }
        //            }
        //            else
        //            {
        //                if (add.Count == 1) { st.AddressDetailsList[0].AddressType = "Primary Address"; }
        //                else if (add.Count == 2) { st.AddressDetailsList[0].AddressType = "Primary Address"; st.AddressDetailsList[1].AddressType = "Secondary Address"; }
        //                else { }
        //            }
        //            st.PreRegNum = Convert.ToInt64(Session["preregno"]);                    //Pre-Registration number
        //            //st.Id = Convert.ToInt64(Session["preregid"]);  // preregid; commented by micheal nov-15
        //            //st.CreatedDate = DateTime.Now.Date.ToString("dd/MM/yyyy");
        //            ViewBag.Date = Session["date"];  // date;
        //            ViewBag.time = Session["time"];  // time; // st.CreatedTime;
        //            st.Status1 = "2";         // key for newregistration,sentforapproval,onhold,callforinterview
        //            // st.AdmissionStatus = "Sent For Approval";
        //            // long preNum = Convert.ToInt64(Session["preregno"]);
        //            // StudentTemplate stud= ams1.GetStudentDetailsByPreRegNo(preNum);
        //            st.AdmissionStatus = "Sent For Clearance";
        //            ViewBag.admissionstatus = st.AdmissionStatus;
        //            ams1.CreateOrUpdateStudentDetails(st);
        //            return RedirectToAction("AdmissionManagement");
        //        }
        //        else if (Request.Form["btnsendapproval"] == "Submit For Approval")
        //        {
        //            st.FamilyDetailsList = null;
        //            st.PastSchoolDetailsList = null;
        //            st.ApproveAssignList = null;
        //            st.PaymentDetailsList = null;
        //            st.DocumentDetailsList = null;
        //            AdmissionManagementService ads = new AdmissionManagementService();
        //            Dictionary<string, object> criteria3 = new Dictionary<string, object>();
        //            criteria3.Add("PreRegNum", Convert.ToInt64(Session["preregno"]));
        //            Dictionary<long, IList<PaymentDetails>> paymentdetails = ads.GetPaymentDetailsListWithPagingAndCriteria(0, 0, string.Empty, string.Empty, criteria3);
        //            if (paymentdetails != null && paymentdetails.FirstOrDefault().Value.Count > 0 && paymentdetails.Count > 0)
        //            {
        //                int count = 0;
        //                foreach (PaymentDetails pay in paymentdetails.FirstOrDefault().Value)
        //                {
        //                    if (pay.FeeType == "Registration Fee") { count++; }
        //                }
        //                if (count < 1)
        //                {
        //                    ViewBag.admissionstatus = Session["admstat"].ToString();
        //                    ViewBag.errmsg = "Not Paid";
        //                    return View(st);
        //                }
        //            }
        //            else
        //            {
        //                ViewBag.admissionstatus = Session["admstat"].ToString();
        //                ViewBag.errmsg = "Not Paid";
        //                return View(st);
        //            }
        //            Dictionary<string, object> criteria4 = new Dictionary<string, object>();
        //            criteria4.Add("StudentId", st.Id);
        //            Dictionary<long, IList<AddressDetails>> add = ams1.GetAddressDetailsListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria4);
        //            if (add.First().Value.Count != 0)
        //            {
        //                if (add.Count == 1) { st.AddressDetailsList[0].AddressType = "Primary Address"; st.AddressDetailsList[0].Id = add.First().Value[0].Id; }
        //                else if (add.Count == 2)
        //                {
        //                    st.AddressDetailsList[0].AddressType = "Primary Address"; st.AddressDetailsList[0].Id = add.First().Value[0].Id;
        //                    st.AddressDetailsList[1].AddressType = "Secondary Address"; st.AddressDetailsList[1].Id = add.First().Value[1].Id;
        //                }
        //                else { }
        //            }
        //            else
        //            {
        //                if (add.Count == 1) { st.AddressDetailsList[0].AddressType = "Primary Address"; }
        //                else if (add.Count == 2) { st.AddressDetailsList[0].AddressType = "Primary Address"; st.AddressDetailsList[1].AddressType = "Secondary Address"; }
        //                else { }
        //            }
        //            st.PreRegNum = Convert.ToInt64(Session["preregno"]);                    //Pre-Registration number
        //            //st.Id = Convert.ToInt64(Session["preregid"]);  // preregid; commented by micheal nov-15
        //            //st.CreatedDate = DateTime.Now.Date.ToString("dd/MM/yyyy");
        //            ViewBag.Date = Session["date"];  // date;
        //            ViewBag.time = Session["time"];  // time; // st.CreatedTime;
        //            st.AdmissionStatus = "Sent For Approval";
        //            switch (st.BoardingType)
        //            {
        //                case "Day Scholar":
        //                    st.IsHosteller = false;
        //                    break;
        //                case "Week Boarder":
        //                    st.IsHosteller = true;
        //                    break;
        //                case "Residential":
        //                    st.IsHosteller = true;
        //                    break;
        //                case "Full Boarder":
        //                    st.IsHosteller = true;
        //                    break;
        //                default:
        //                    st.IsHosteller = false;
        //                    break;
        //            }
        //            st.Status = "1";          // key for sentfor approval, on hold,callforinterview
        //            st.Status1 = "2";         // key for newregistration,sentforapproval,onhold,callforinterview
        //            ViewBag.admissionstatus = "New Registration";               //Registration Status
        //            if (st.Campus != "TIPS SALEM" && st.Campus != "TIPS ERODE")
        //            {
        //                StudentsReportBC srbc = new StudentsReportBC();
        //                criteria.Clear();
        //                criteria.Add("EmailType", "Head-Admission");
        //                criteria.Add("IsMailNeeded", true);
        //                Dictionary<long, IList<MISMailMaster>> MISMailMasterDetails = srbc.GetMISMailMasterListWithPaging(null, 9999, string.Empty, string.Empty, criteria);
        //                if (MISMailMasterDetails != null && MISMailMasterDetails.Count > 0 && MISMailMasterDetails.FirstOrDefault().Value.Count > 0)
        //                {
        //                    foreach (MISMailMaster item in MISMailMasterDetails.First().Value)
        //                    {
        //                        RecipientInfo = "Dear Team,";
        //                        Subject = "A new student application is available for Approval"; // st.Subject;
        //                        string Body1 = "A new application is available in Sent For Approval status and you are requested to process the same.<br><br>Campus: " + st.Campus + "<br>";
        //                        Body1 += "Application number: " + st.ApplicationNo + "<br>Pre-Reg number: " + st.PreRegNum + "<br>Applicant name: " + st.Name + "<br>Application grade: " + st.Grade + "<br>Applied Academic year: " + st.AcademicYear + "<br>";
        //                        retValue = em.SendStudentRegistrationMail(st, item.EmailId, st.Campus, Subject, Body1, MailBody, RecipientInfo, "Head-Admission", null);
        //                    }
        //                }
        //            }
        //            ads.CreateOrUpdateStudentDetails(st);
        //            return RedirectToAction("AdmissionManagement");
        //        }
        //        else if (Request.Form["relationtype"] == "Add")
        //        {
        //            AdmissionManagementService ads = new AdmissionManagementService();
        //            ads.CreateOrUpdateStudentDetails(st);
        //            return View();
        //        }
        //        else if (Request.Form["famsub"] == "Add")
        //        {
        //            AdmissionManagementService ads = new AdmissionManagementService();
        //            FamilyDetails fdet = new FamilyDetails();
        //            fdet.PreRegNum = Convert.ToInt64(Session["preregno"]);
        //            fdet.StudentId = st.Id;
        //            fdet.FamilyDetailType = st.FamilyDetailsList[0].FamilyDetailType;
        //            fdet.Name = st.FamilyDetailsList[0].Name;
        //            fdet.Education = st.FamilyDetailsList[0].Education;
        //            fdet.Age = st.FamilyDetailsList[0].Age;
        //            fdet.Mobile = st.FamilyDetailsList[0].Mobile;
        //            fdet.EmpType = st.FamilyDetailsList[0].EmpType;
        //            fdet.Occupation = st.FamilyDetailsList[0].Occupation;
        //            fdet.CompName = st.FamilyDetailsList[0].CompName;
        //            fdet.CompAddress = st.FamilyDetailsList[0].CompAddress;
        //            fdet.StayingWithChild = st.FamilyDetailsList[0].StayingWithChild;
        //            fdet.Email = st.FamilyDetailsList[0].Email;
        //            fdet.TransportReq = st.FamilyDetailsList[0].TransportReq;

        //            ads.CreateOrUpdateFamilyDetails(fdet);
        //            return View();
        //        }
        //        else if (Request.Form["PastAdd"] == "Add")
        //        {
        //            AdmissionManagementService ads = new AdmissionManagementService();
        //            PastSchoolDetails pastadd = new PastSchoolDetails();
        //            pastadd.PreRegNum = Convert.ToInt64(Session["preregno"]);
        //            pastadd.StudentId = st.Id;
        //            pastadd.FromDate = st.PastSchoolDetailsList[0].FromDate.ToString();
        //            pastadd.ToDate = st.PastSchoolDetailsList[0].ToDate;
        //            pastadd.SchoolName = st.PastSchoolDetailsList[0].SchoolName;
        //            pastadd.City = st.PastSchoolDetailsList[0].City;
        //            pastadd.FromGrade = st.PastSchoolDetailsList[0].FromGrade;
        //            pastadd.ToGrade = st.PastSchoolDetailsList[0].ToGrade;
        //            ads.CreateOrUpdatePastSchoolDetails(pastadd);
        //            return View();
        //        }
        //        else
        //        {
        //            return View(st);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
        //        throw ex;
        //    }
        //}

        public ActionResult UpdateStatus(string status, string feestruct, string sect)
        {
            AdmissionManagementService ads = new AdmissionManagementService();

            StudentTemplate st = ads.GetStudentDetailsById(Convert.ToInt32(Session["editid"]));

            st.AdmissionStatus = status;
            st.FeeStructYear = feestruct;
            st.Section = sect;

            st.FamilyDetailsList = null;
            st.PastSchoolDetailsList = null;
            st.ApproveAssignList = null;
            st.PaymentDetailsList = null;
            st.DocumentDetailsList = null;

            Dictionary<string, object> criteria4 = new Dictionary<string, object>();
            criteria4.Add("StudentId", Convert.ToInt64(Session["preregid"]));
            Dictionary<long, IList<AddressDetails>> add = ads.GetAddressDetailsListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria4);

            if (add.First().Value.Count != 0)
            {
                st.AddressDetailsList[0].AddressType = "Primary Address";
                st.AddressDetailsList[1].AddressType = "Secondary Address";
                st.AddressDetailsList[0].Id = add.First().Value[0].Id;
                st.AddressDetailsList[1].Id = add.First().Value[1].Id;
            }
            else
            {
                st.AddressDetailsList[0].AddressType = "Primary Address";
                st.AddressDetailsList[1].AddressType = "Secondary Address";
            }
            ViewBag.Date = Session["date"]; // date;
            ViewBag.time = Session["time"];  // time; // st.CreatedTime;
            st.CreatedTime = Session["time"] == null ? "" : Session["time"].ToString(); // time;
            st.CreatedDate = Session["date"] == null ? "" : Session["date"].ToString();  // date;

            switch (st.BoardingType)
            {
                case "Day Scholar":
                    st.IsHosteller = false;
                    break;

                case "Week Boarder":
                    st.IsHosteller = true;
                    break;
                case "Residential":
                    st.IsHosteller = true;
                    break;
                case "Full Boarder":
                    st.IsHosteller = true;
                    break;
                default:
                    st.IsHosteller = false;
                    break;
            }

            if (st.AdmissionStatus == "Registered")
            {
                Dictionary<string, object> criteria2 = new Dictionary<string, object>();
                criteria2.Add("PreRegNum", Convert.ToInt64(Session["preregno"]));   // to check if student is already registered or not while changing from inactive to registered
                Dictionary<long, IList<StudentTemplateView>> IdCheck = ads.GetStudentTemplateViewListWithEQsearchCriteria(0, 0, string.Empty, string.Empty, criteria2);
                if (IdCheck.First().Value[0].NewId == null)
                {
                    st.NewId = StudentIdNumberLogic(st.Campus, st.FeeStructYear, st.Grade, st.AcademicYear);
                }

                st.AdmissionStatus = "Registered";    //Admission Registered By Admin                   
                st.Status = null;
                st.Status1 = null;
                ads.CreateOrUpdateStudentDetails(st);
                Session["registered"] = "yes";
                Session["regid"] = st.NewId;
            }
            else if (st.AdmissionStatus == "On Hold")
            {
                st.Status = "1";          // key for sentfor approval, on hold,callforinterview
                st.Status1 = "2";         // key for newregistration,sentforapproval,onhold,callforinterview

                ads.CreateOrUpdateStudentDetails(st);
            }
            else if (st.AdmissionStatus == "Declined")
            {
                ads.CreateOrUpdateStudentDetails(st);
            }
            else if (st.AdmissionStatus == "Inactive")
            {
                st.AdmissionStatus = "Inactive";
                ads.CreateOrUpdateStudentDetails(st);

                // To deactivate a student from Assess360 by passing Student Primary Key ---- By Anbu

                DeactivateStudent(st.Id);
            }
            else
            {
                st.Status = "1";          // key for sentfor approval, on hold,callforinterview
                st.Status1 = "2";         // key for newregistration,onhold,sentforapproval,callforinterview
                ads.CreateOrUpdateStudentDetails(st);
            }
            return RedirectToAction("AdmissionManagement");
        }

        public ActionResult GetFormData(long id, string RegisteredForm)
        {
            try
            {
                if (RegisteredForm == "yes")
                {
                    Session["RegisteredForm"] = "yes";
                }
                else
                {
                    Session["RegisteredForm"] = "";
                }

                AdmissionManagementService ads = new AdmissionManagementService();

                StudentTemplate StudentTemplate = ads.GetStudentDetailsById(id);

                ViewBag.approvalstatus = StudentTemplate.Status;

                FamilyDetails familyDetails = new FamilyDetails();
                if (StudentTemplate.FamilyDetailsList == null && StudentTemplate.FamilyDetailsList.Count == 0)
                {
                    IList<FamilyDetails> familydetailsList = new List<FamilyDetails>();
                    familydetailsList.Add(familyDetails);
                    StudentTemplate.FamilyDetailsList = familydetailsList;
                }

                PastSchoolDetails PastSchoolDetails = new PastSchoolDetails();

                if (StudentTemplate.PastSchoolDetailsList == null && StudentTemplate.PastSchoolDetailsList.Count == 0)
                {
                    IList<PastSchoolDetails> PastSchoolDetailsList = new List<PastSchoolDetails>();
                    PastSchoolDetailsList.Add(PastSchoolDetails);
                    StudentTemplate.PastSchoolDetailsList = PastSchoolDetailsList;
                }

                AddressDetails AddressDetails = new AddressDetails();
                if (StudentTemplate.AddressDetailsList == null && StudentTemplate.AddressDetailsList.Count == 0)
                {
                    IList<AddressDetails> AddressDetailsList = new List<AddressDetails>();
                    AddressDetailsList.Add(AddressDetails);
                    StudentTemplate.AddressDetailsList = AddressDetailsList;
                }

                Session["editid"] = StudentTemplate.Id;

                Session["preregno"] = StudentTemplate.PreRegNum;

                return RedirectToAction("NewRegistration");
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult GetFamilyFormData(long id)
        {
            try
            {
                AdmissionManagementService ads = new AdmissionManagementService();
                Dictionary<string, object> criteria1 = new Dictionary<string, object>();
                criteria1.Add("PreRegNum", id);
                Dictionary<long, IList<StudentTemplate>> StudentTemplate = ads.GetStudentDetailsListWithEQsearchCriteria(0, 0, null, null, criteria1);
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("PreRegNum", id);
                Dictionary<long, IList<FamilyDetails>> familyDetails = ads.GetFamilyDetailsListWithPagingAndCriteria(0, 0, null, null, criteria);
                StudentTemplate.First().Value[0].FamilyDetailsList = familyDetails.First().Value;

                Session["preregno"] = id;

                return RedirectToAction("NewRegistration", StudentTemplate);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public JsonResult Documentsjqgrid(string id, string txtSearch, string idno, string name, string sect, string cname, string grad, string btype, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                AdmissionManagementService ads = new AdmissionManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("DocumentFor", "Student");
                criteria.Add("PreRegNum", Convert.ToInt64(Session["preregno"]));

                Dictionary<long, IList<UploadedFilesView>> UploadedFilesview = ads.GetUploadedFilesViewListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);

                {
                    long totalrecords = UploadedFilesview.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,

                        rows = (from items in UploadedFilesview.First().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[] {
                               items.DocumentType,
                              // String.Format(@"<a style='color:#034af3;text-decoration:underline' href= '#' onclick = 'uploaddat("+"\"" + items.Id + "\"" + ")' >{0}</a>",items.DocumentName),
                               "<a style='color:#034af3;text-decoration:underline' onclick=\"uploaddat('" + items.Id + "','" + items.Type + "','" + items.PreRegNum+  "');\" '>"+items.DocumentName+"</a>",
                               items.DocumentSize+" Bytes",
                               items.UploadedDate,
                               items.Id.ToString()
                            }
                                })
                    };
                    return Json(jsondat, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult uploaddisplay(long? Id, string type, AddressDetails add)
        {
            try
            {
                AdmissionManagementService ads = new AdmissionManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (Id > 0)
                {
                    criteria.Add("Id", Id);
                    Dictionary<long, IList<UploadedFiles>> UploadedFiles = ads.GetUploadedFilesListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteria);

                    if (UploadedFiles != null && UploadedFiles.FirstOrDefault().Value != null)
                    {
                        if (UploadedFiles.First().Value[0].Type == "TransferPDF")
                        {
                            PDFGeneratorTransfer(UploadedFiles.First().Value[0].PreRegNum);
                            //  return File(ConfigurationManager.AppSettings["TC"].ToString(), "Application/pdf", "" + UploadedFiles.First().Value[0].PreRegNum + "TC.pdf");
                            return File(ConfigurationManager.AppSettings["TC"].ToString() + UploadedFiles.First().Value[0].PreRegNum + ".pdf", "Application/pdf", "" + UploadedFiles.First().Value[0].PreRegNum + "TC.pdf");
                        }
                        else if (UploadedFiles.First().Value[0].Type == "BonafidePDF")
                        {

                            PDFGeneratorBonafide(UploadedFiles.First().Value[0].PreRegNum, type, add);
                            return File(ConfigurationManager.AppSettings["BC"].ToString() + UploadedFiles.First().Value[0].PreRegNum + ".pdf", "Application/pdf", "" + UploadedFiles.First().Value[0].PreRegNum + "BC.pdf");
                        }

                        else
                        {
                            if (UploadedFiles.First().Value[0].OldFiles == 1)
                            {
                                var dir = Server.MapPath("/Images");
                                string ImagePath = UploadedFiles.First().Value[0].FileDirectory + "\\" + UploadedFiles.First().Value[0].DocumentName;// "green.jpg"; ConfigurationManager.AppSettings["ImageFilePath1"] 
                                if (!System.IO.File.Exists(ImagePath))
                                {
                                    ImagePath = ConfigurationManager.AppSettings["ImageFilePath1"] + UploadedFiles.First().Value[0].DocumentName;
                                }
                                var cd = new System.Net.Mime.ContentDisposition
                                {
                                    Inline = false,
                                };
                                Response.AppendHeader("Content-Disposition", cd.ToString());
                                return File(ImagePath, "JPG");
                            }
                            else
                            {
                                IList<UploadedFiles> list = UploadedFiles.FirstOrDefault().Value;
                                UploadedFiles doc = list.FirstOrDefault();
                                if (doc.DocumentData != null)
                                {
                                    var cd = new System.Net.Mime.ContentDisposition
                                    {
                                        // always prompt the user for downloading, set to true if you want 
                                        // the browser to try to show the file inline
                                        FileName = UploadedFiles.First().Value[0].DocumentName,
                                        Inline = false,
                                    };
                                    Response.AppendHeader("Content-Disposition", cd.ToString());
                                    return File(doc.DocumentData, "image");
                                }
                                else
                                {
                                    var dir = Server.MapPath("/Images");
                                    string ImagePath = ConfigurationManager.AppSettings["ImageFilePath"];
                                    return File(ImagePath, "image/jpg");
                                }
                            }
                        }
                    }
                    return null;
                }
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult uploaddisplay1(long Id)
        {
            try
            {
                AdmissionManagementService ads = new AdmissionManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("PreRegNum", Id);
                criteria.Add("DocumentType", "Student Photo");

                Dictionary<long, IList<UploadedFiles>> UploadedFiles = ads.GetUploadedFilesListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteria);

                //                IList<UploadedFiles> list1 = ads.GetUploadedFilesByPreRegNum(Id,"no");
                if (UploadedFiles != null && UploadedFiles.FirstOrDefault().Value != null && UploadedFiles.FirstOrDefault().Value.Count != 0)
                {
                    if (UploadedFiles.First().Value[0].OldFiles == 1)
                    {
                        string ImagePath = UploadedFiles.First().Value[0].FileDirectory + "\\" + UploadedFiles.First().Value[0].DocumentName;// "green.jpg"; ConfigurationManager.AppSettings["ImageFilePath1"] 

                        if (!System.IO.File.Exists(ImagePath))
                        {
                            ImagePath = ConfigurationManager.AppSettings["ImageFilePath1"] + UploadedFiles.First().Value[0].DocumentName;
                        }
                        return File(ImagePath, "image/jpg");
                    }
                    else
                    {
                        IList<UploadedFiles> list = UploadedFiles.FirstOrDefault().Value;
                        UploadedFiles doc = list.FirstOrDefault();
                        if (doc.DocumentData != null)
                        {
                            MemoryStream ms = new MemoryStream(doc.DocumentData);

                            var cd = new System.Net.Mime.ContentDisposition
                            {
                                // for example foo.bak
                                FileName = doc.DocumentName,

                                // always prompt the user for downloading, set to true if you want 
                                // the browser to try to show the file inline
                                Inline = false,
                            };
                            Response.AppendHeader("Content-Disposition", cd.ToString());
                            return File(doc.DocumentData, "image/jpg");
                        }
                        else
                        {
                            string ImagePath = ConfigurationManager.AppSettings["ImageFilePath"];
                            return File(ImagePath, "image/jpg");
                        }
                    }
                }
                else
                {
                    var dir = Server.MapPath("/Images");
                    string ImagePath = ConfigurationManager.AppSettings["ImageFilePath"];
                    return File(ImagePath, "image/jpg");
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult uploaddisplay2(long Id)
        {
            try
            {
                AdmissionManagementService ads = new AdmissionManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Id", Id);
                Dictionary<long, IList<UploadedFiles>> UploadedFiles = ads.GetUploadedFilesListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteria);
                if (UploadedFiles != null && UploadedFiles.FirstOrDefault().Value != null && UploadedFiles.FirstOrDefault().Value.Count != 0)
                {
                    if (UploadedFiles.First().Value[0].OldFiles == 1)
                    {
                        var dir = Server.MapPath("/Images");
                        string ImagePath = UploadedFiles.First().Value[0].FileDirectory + "\\" + UploadedFiles.First().Value[0].DocumentName;// "green.jpg"; ConfigurationManager.AppSettings["ImageFilePath1"] 
                        if (!System.IO.File.Exists(ImagePath))
                        {
                            ImagePath = ConfigurationManager.AppSettings["ImageFilePath1"] + UploadedFiles.First().Value[0].DocumentName;
                        }
                        return File(ImagePath, "image/jpg");
                    }
                    else
                    {
                        IList<UploadedFiles> list = UploadedFiles.FirstOrDefault().Value;
                        UploadedFiles doc = list.FirstOrDefault();
                        MemoryStream ms = new MemoryStream(doc.DocumentData);
                        var cd = new System.Net.Mime.ContentDisposition
                        {
                            // for example foo.bak
                            FileName = doc.DocumentName,

                            // always prompt the user for downloading, set to true if you want 
                            // the browser to try to show the file inline
                            Inline = false,
                        };
                        Response.AppendHeader("Content-Disposition", cd.ToString());
                        return File(doc.DocumentData, doc.DocumentType);
                    }
                }
                else
                {
                    var dir = Server.MapPath("/Images");
                    var path = Path.Combine("green.jpg");
                    string ImagePath = ConfigurationManager.AppSettings["ImageFilePath"];
                    return File(ImagePath, "image/jpg");
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult Printgriddisplay(string id)
        {
            try
            {
                AdmissionManagementService ads = new AdmissionManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("PreRegNum", Session["prergno"]);
                criteria.Add("DocumentType", "Student Photo");

                Dictionary<long, IList<UploadedFiles>> UploadedFiles = ads.GetUploadedFilesListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteria);

                if (UploadedFiles != null && UploadedFiles.FirstOrDefault().Value != null && UploadedFiles.FirstOrDefault().Value.Count != 0)
                {
                    if (UploadedFiles.First().Value[0].OldFiles == 1)
                    {
                        string ImagePath = UploadedFiles.First().Value[0].FileDirectory + "\\" + UploadedFiles.First().Value[0].DocumentName;// "green.jpg"; ConfigurationManager.AppSettings["ImageFilePath1"] 

                        if (!System.IO.File.Exists(ImagePath))
                        {
                            ImagePath = ConfigurationManager.AppSettings["ImageFilePath1"] + UploadedFiles.First().Value[0].DocumentName;
                        }
                        return File(ImagePath, "image/jpg");
                    }
                    else
                    {
                        IList<UploadedFiles> list = UploadedFiles.FirstOrDefault().Value;
                        UploadedFiles doc = list.FirstOrDefault();
                        if (doc.DocumentData != null)
                        {
                            MemoryStream ms = new MemoryStream(doc.DocumentData);
                            var cd = new System.Net.Mime.ContentDisposition
                            {
                                FileName = doc.DocumentName,
                                Inline = false,
                            };
                            Response.AppendHeader("Content-Disposition", cd.ToString());

                            return File(doc.DocumentData, "image/jpg");
                        }
                        else
                        {
                            string ImagePath = ConfigurationManager.AppSettings["ImageFilePath"];
                            return File(ImagePath, "image/jpg");
                        }
                    }
                }
                else
                {
                    var dir = Server.MapPath("/Images");
                    string ImagePath = ConfigurationManager.AppSettings["ImageFilePath"];
                    return File(ImagePath, "image/jpg");
                }
            }
            catch (Exception ex)
            {

                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            return null;
        }

        public ActionResult familyjqgrid(int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                AdmissionManagementService ads = new AdmissionManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("PreRegNum", Convert.ToInt64(Session["preregno"]));

                Dictionary<long, IList<FamilyDetails>> FamilyDetails = ads.GetFamilyDetailsListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);

                long totalrecords = FamilyDetails.First().Key;
                int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                var jsondat = new
                {
                    total = totalPages,
                    page = page,
                    records = totalrecords,

                    rows = (from items in FamilyDetails.First().Value
                            select new
                            {
                                i = 2,
                                cell = new string[] {
                            items.FamilyDetailType,
                            items.Name,
                            items.Education,
                            items.Mobile,
                            items.Age.ToString(),
                            items.Email,
                            
                            items.EmpType,
                            items.Occupation,
                            items.CompName,
                            items.CompAddress,
                            items.StayingWithChild.ToString(),
                            items.TransportReq.ToString(),
                            items.Id.ToString()
                            }
                            })
                };
                return Json(jsondat, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult EditFamily(FamilyDetails fd)
        {
            try
            {
                if (fd.TransportReq.ToString() == "Yes")
                {
                    fd.TransportReq = true;
                }
                else if (fd.TransportReq.ToString() == "No")
                {
                    fd.TransportReq = false;
                }

                if (fd.StayingWithChild.ToString() == "Yes")
                {
                    fd.StayingWithChild = true;
                }
                else if (fd.StayingWithChild.ToString() == "No")
                {
                    fd.StayingWithChild = false;
                }

                AdmissionManagementService ads = new AdmissionManagementService();
                fd.PreRegNum = Convert.ToInt64(Session["preregno"]);

                ads.CreateOrUpdateFamilyDetails(fd);
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditPastSchool(PastSchoolDetails pd)
        {
            try
            {
                AdmissionManagementService ads = new AdmissionManagementService();
                pd.PreRegNum = Convert.ToInt64(Session["preregno"]);

                ads.CreateOrUpdatePastSchoolDetails(pd);
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult pastschooljqgrid(int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                AdmissionManagementService ads = new AdmissionManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("PreRegNum", Convert.ToInt64(Session["preregno"]));
                Dictionary<long, IList<PastSchoolDetails>> PastSchoolDetails = ads.GetPastSchoolDetailsListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                long totalrecords = PastSchoolDetails.First().Key;
                int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                var jsondat = new
                {
                    total = totalPages,
                    page = page,
                    records = totalrecords,
                    rows = (from items in PastSchoolDetails.First().Value
                            select new
                            {
                                i = 2,
                                cell = new string[] {
                            items.FromDate,
                            items.ToDate,
                            items.SchoolName,
                            items.City,
                            items.FromGrade,
                            items.ToGrade,
                            items.Id.ToString()
                            }
                            })
                };
                return Json(jsondat, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult paymentjqgrid(long preregno, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                AdmissionManagementService ads = new AdmissionManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("PreRegNum", preregno);
                Dictionary<long, IList<PaymentDetails>> PaymentDetails = ads.GetPaymentDetailsListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                long totalrecords = PaymentDetails.First().Key;
                int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                var jsondat = new
                {
                    total = totalPages,
                    page = page,
                    records = totalrecords,

                    rows = (from items in PaymentDetails.First().Value
                            select new
                            {
                                i = 2,
                                cell = new string[] {
                            items.FeeType,
                            items.ModeOfPayment,
                            items.ChequeDate!=null?items.ChequeDate.Value.ToString("dd/MM/yyyy"):"",
                            items.ReferenceNo,
                            items.BankName,
                            items.Amount,
                            items.Remarks,
                            items.PaidDate!=null?items.PaidDate.Value.ToString("dd/MM/yyyy"):"",
                            items.ClearedDate!=null?items.ClearedDate.Value.ToString("dd/MM/yyyy"):"",
                            items.FeePaidStatus,
                            items.CreatedDate!=null?items.CreatedDate.ToString("dd/MM/yyyy"):"",
                            items.CreatedBy,
                            items.ModifiedDate!=null?items.ModifiedDate.ToString("dd/MM/yyyy"):"",
                            items.ModifiedBy,
                            items.Id.ToString()
                            }
                            })
                };
                return Json(jsondat, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult approveassignjqgrid(int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                AdmissionManagementService ads = new AdmissionManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("PreRegNum", Convert.ToInt64(Session["preregno"]));

                Dictionary<long, IList<ApproveAssign>> ApproveAssign = ads.GetApproveAssignListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);

                long totalrecords = ApproveAssign.First().Key;
                int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                var jsondat = new
                {
                    total = totalPages,
                    page = page,
                    records = totalrecords,

                    rows = (from items in ApproveAssign.First().Value
                            select new
                            {
                                i = 2,
                                cell = new string[] {
                             items.Status,
                             items.FeeStructYear,
                            items.AssignSection,
                            items.Id.ToString()
                            }
                            })
                };
                return Json(jsondat, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditPaymentDetails(PaymentDetails pd)
        {
            try
            {
                string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                AdmissionManagementService ads = new AdmissionManagementService();
                IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                pd.PreRegNum = Convert.ToInt64(Session["preregno"]);
                if (!string.IsNullOrEmpty(Request["ChequeDate"]))
                    pd.ChequeDate = DateTime.Parse(Request["ChequeDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                else pd.ChequeDate = null;
                if (!string.IsNullOrEmpty(Request["PaidDate"]))
                    pd.PaidDate = DateTime.Parse(Request["PaidDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                else pd.PaidDate = null;
                if (!string.IsNullOrEmpty(Request["ClearedDate"]))
                    pd.ClearedDate = DateTime.Parse(Request["ClearedDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                else pd.ClearedDate = null;
                pd.CreatedDate = DateTime.Now;
                pd.ModifiedDate = DateTime.Now;
                pd.CreatedBy = userid;
                pd.ModifiedBy = userid;
                ads.CreateOrUpdatePaymentDetails(pd);
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteFamilyDetails(string id)
        {
            try
            {
                AdmissionManagementService ads = new AdmissionManagementService();

                var test = id.Split(',');

                long[] idtodelete = new long[test.Length];
                int i = 0;
                foreach (string val in test)
                {
                    idtodelete[i] = Convert.ToInt64(val);
                    i++;
                }

                ads.DeleteFamilyDetails(idtodelete);

                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeletePastSchoolDetails(string id)
        {
            try
            {
                AdmissionManagementService ads = new AdmissionManagementService();
                var test = id.Split(',');

                long[] idtodelete = new long[test.Length];
                int i = 0;
                foreach (string val in test)
                {
                    idtodelete[i] = Convert.ToInt64(val);
                    i++;
                }

                ads.DeletePastSchoolDetails(idtodelete);
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeletePaymentDetails(string id)
        {
            try
            {
                AdmissionManagementService ads = new AdmissionManagementService();
                var test = id.Split(',');

                long[] idtodelete = new long[test.Length];
                int i = 0;
                foreach (string val in test)
                {
                    idtodelete[i] = Convert.ToInt64(val);
                    i++;
                }
                ads.DeletePaymentDetails(idtodelete);
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteApproveAssign(string id)
        {
            try
            {
                AdmissionManagementService ads = new AdmissionManagementService();
                var test = id.Split(',');

                long[] idtodelete = new long[test.Length];
                int i = 0;
                foreach (string val in test)
                {
                    idtodelete[i] = Convert.ToInt64(val);
                    i++;
                }

                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Id", Convert.ToInt64(id));
                Dictionary<long, IList<ApproveAssign>> ApproveAsgn = ads.GetApproveAssignListWithPagingAndCriteria(null, null, null, null, criteria);

                Dictionary<string, object> criteria1 = new Dictionary<string, object>();
                criteria1.Add("PreRegNum", ApproveAsgn.First().Value[0].PreRegNum);
                Dictionary<long, IList<IdGeneration>> idgen = ads.GetIdGenerationListWithPagingAndCriteria(null, null, null, null, criteria1);
                if (idgen.First().Value.Count != 0)
                {
                    ads.DeleteIdGeneration(idgen.First().Value[0].Id);
                }
                ads.DeleteApproveAssign(idtodelete);


                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteUploadedFiles(string id)
        {
            try
            {
                AdmissionManagementService ads = new AdmissionManagementService();
                var test = id.Split(',');

                long[] idtodelete = new long[test.Length];
                int[] values = new int[test.Length];
                int i = 0;
                foreach (string val in test)
                {
                    idtodelete[i] = Convert.ToInt64(val);
                    values[i] = Convert.ToInt32(val);
                    i++;
                }

                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<long, IList<UploadedFilesView>> uploadedfilesview = ads.GetUploadedFilesViewListWithPagingAndCriteriaWithIn(0, 50, "", "", "Id", values, criteria, null);
                List<long> tid = new List<long>();
                List<long> sid = new List<long>();
                int j = 0;
                foreach (var val in uploadedfilesview.First().Value)
                {
                    if (val.Type == "TransferPDF")
                    {
                        criteria.Clear();
                        criteria.Add("Type", "Discontinue");
                        criteria.Add("PreRegNum", val.PreRegNum);
                        Dictionary<long, IList<TransferDetails>> td = ads.GetTransferDetailsListWithPagingAndCriteria(null, null, string.Empty, string.Empty, criteria);
                        tid.Add(td.First().Value[0].Id);
                        j++;
                    }
                    else if (val.Type == "BonafidePDF")
                    {
                        criteria.Clear();
                        criteria.Add("Type", "Bonafide");
                        criteria.Add("PreRegNum", val.PreRegNum);
                        Dictionary<long, IList<TransferDetails>> td = ads.GetTransferDetailsListWithPagingAndCriteria(null, null, string.Empty, string.Empty, criteria);
                        tid.Add(td.First().Value[0].Id);
                        j++;
                    }
                    else if (val.DocumentType == "Sports Certificate")
                    {
                        criteria.Clear();
                        criteria.Add("SportsId", Convert.ToInt32(val.Id));
                        Dictionary<long, IList<Sports>> sd = ads.GetSportsDetailsListWithPagingAndCriteria(null, null, string.Empty, string.Empty, criteria);
                        if (sd.First().Value.Count > 0) { sid.Add(sd.First().Value[0].Id); }
                        j++;
                    }
                    else { }
                }
                if (tid.Count > 0)
                {
                    ads.DeleteTransferDetails(tid.ToArray());
                }
                if (sid.Count > 0)
                {
                    ads.DeleteSportsDetails(sid.ToArray());
                }
                ads.DeleteUploadedFiles(idtodelete);
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult ViewPage1()
        {
            return View();
        }

        public ActionResult Stayingwithchildddl()
        {
            try
            {
                Dictionary<bool, string> stayingwithchild = new Dictionary<bool, string>();
                stayingwithchild.Add(true, "Yes");
                stayingwithchild.Add(false, "No");

                return PartialView("Select", stayingwithchild);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult pickupcardddl()
        {
            try
            {
                Dictionary<bool, string> pickupcard = new Dictionary<bool, string>();

                pickupcard.Add(false, "No");
                pickupcard.Add(true, "Yes");

                return PartialView("Select", pickupcard);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult Employeetypeddl()
        {
            try
            {
                Dictionary<long, string> employeetype = new Dictionary<long, string>();

                employeetype.Add(1, "Self Employed");
                employeetype.Add(2, "Salaried");
                employeetype.Add(3, "Others");

                return PartialView("Select", employeetype);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult Gradeddl()
        {
            try
            {
                MastersService ms = new MastersService();
                Dictionary<long, string> gradecd = new Dictionary<long, string>();

                Dictionary<string, object> criteria = new Dictionary<string, object>();

                Dictionary<long, IList<GradeMaster>> grade = ms.GetGradeMasterListWithPagingAndCriteria(null, null, null, null, criteria);

                foreach (GradeMaster grad in grade.First().Value)
                {
                    gradecd.Add(grad.FormId, grad.gradcod);
                }
                return PartialView("Select1", gradecd);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult Gradeddl1(string campus)
        {
            try
            {
                MastersService ms = new MastersService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();

                int[] values;
                var g1 = "";
                if (campus == "IB KG")
                {
                    g1 = "2";
                    criteria.Add("Code", g1);
                    values = new int[0];
                }
                else if (campus == "IB MAIN")
                {
                    g1 = "1";
                    criteria.Add("Code", g1);
                    values = new int[0];
                }
                else if (campus == "ERNAKULAM")
                {
                    g1 = "";
                    criteria.Add("Code", g1);
                    values = new int[0];
                }
                else if (campus == "TIPS SARAN")   // Get using IN condition
                {
                    values = new int[2];
                    criteria.Clear();
                    values[0] = 11;                     // get grade with column GRAD
                    values[1] = 12;
                }
                else if (campus == "CHENNAI CITY")   // Get using IN condition
                {
                    values = new int[5];
                    criteria.Clear();
                    values[0] = 102;                     // get grade with column GRAD
                    values[1] = 101;
                    values[2] = 100; values[3] = 1; values[4] = 2;
                }
                else if (campus == "CHENNAI MAIN")   // Get using IN condition
                {
                    values = new int[6];
                    criteria.Clear();
                    values[0] = 3;                     // get grade with column GRAD
                    values[1] = 4;
                    values[2] = 5; values[3] = 6; values[4] = 7; values[5] = 8;
                }
                else if (campus == "TIPS CBSE")
                {
                    values = new int[10];
                    criteria.Clear();
                    values[0] = 1; values[1] = 2; values[2] = 3;
                    values[3] = 4; values[4] = 5; values[5] = 6;
                    values[6] = 7; values[7] = 8; values[8] = 9; values[9] = 10;
                }
                else
                {
                    g1 = "3";
                    criteria.Add("Code1", g1);
                    values = new int[0];
                }
                Dictionary<long, IList<GradeMaster>> GradeMaster = ms.GetGradeMasterListWithPagingAndCriteriaWithIn(0, 50, "", "", "grad", values, criteria, null);
                ViewBag.gradeddl1 = GradeMaster.First().Value;

                return Json(ViewBag.gradeddl1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Get)] //Newly added for campus wise grade by micheal
        public JsonResult CampusGradeddl(string Campus)
        {
            try
            {
                MastersService ms = new MastersService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();

                criteria.Add("Campus", Campus);
                Dictionary<long, IList<CampusGradeMaster>> GradeMaster = ms.GetCampusGradeMasterListWithPagingAndCriteria(null, null, null, null, criteria);
                ViewBag.gradeddl1 = GradeMaster.First().Value;

                return Json(ViewBag.gradeddl1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult Modeofpmtddl()
        {
            try
            {
                Dictionary<string, string> modofpmt = new Dictionary<string, string>();
                modofpmt.Add("Cash", "Cash");
                modofpmt.Add("Cheque", "Cheque");
                modofpmt.Add("DD", "DD");
                modofpmt.Add("RTGS/NEFT", "RTGS/NEFT");

                return PartialView("Dropdown", modofpmt);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult AddFamilydetails(string relationtype, string name1, string name2, string mobile, string age, string email, string occupation, string compn, string compa, string emptype, string stayingwithchild, string pickupcard)
        {
            try
            {
                if (age == "")
                {
                    age = "0";
                }

                AdmissionManagementService ads = new AdmissionManagementService();

                FamilyDetails fdet = new FamilyDetails();
                fdet.PreRegNum = Convert.ToInt64(Session["preregno"]);
                fdet.StudentId = 0;
                fdet.FamilyDetailType = relationtype;
                fdet.Name = name1;
                fdet.Education = name2;
                fdet.Age = Convert.ToInt32(age);
                fdet.Mobile = mobile;
                fdet.EmpType = emptype;
                fdet.Occupation = occupation;
                fdet.CompName = compn;
                fdet.CompAddress = compa;
                fdet.StayingWithChild = Convert.ToBoolean(stayingwithchild);
                fdet.Email = email;
                fdet.TransportReq = Convert.ToBoolean(pickupcard);

                ads.CreateOrUpdateFamilyDetails(fdet);
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult AddPastschooldetails(string frmdate, string todate, string schlname, string city, string fgrade, string tgrade)
        {
            try
            {
                AdmissionManagementService ads = new AdmissionManagementService();
                PastSchoolDetails pastadd = new PastSchoolDetails();
                pastadd.PreRegNum = Convert.ToInt64(Session["preregno"]);
                pastadd.StudentId = 0;
                pastadd.FromDate = frmdate;
                pastadd.ToDate = todate;
                pastadd.SchoolName = schlname;
                pastadd.City = city;
                pastadd.FromGrade = fgrade;
                pastadd.ToGrade = tgrade;

                ads.CreateOrUpdatePastSchoolDetails(pastadd);
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult AddPaymentdetails(string feetype, string modofpmt, string amount, string refno)
        {
            try
            {
                AdmissionManagementService ads = new AdmissionManagementService();

                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("FeeType", feetype);
                criteria.Add("PreRegNum", Convert.ToInt64(Session["preregno"]));
                //criteria.Add("ReferenceNo", refno);
                Dictionary<long, IList<PaymentDetails>> PaymentDetails = ads.GetPaymentDetailsListWithPagingAndCriteria(null, null, null, null, criteria);

                if (PaymentDetails.First().Value.Count == 0)
                {
                    PaymentDetails pmtadd = new PaymentDetails();
                    pmtadd.PreRegNum = Convert.ToInt64(Session["preregno"]);
                    pmtadd.StudentId = 0;
                    pmtadd.FeeType = feetype;
                    pmtadd.ModeOfPayment = modofpmt;
                    pmtadd.Amount = amount;
                    pmtadd.ReferenceNo = refno;

                    ads.CreateOrUpdatePaymentDetails(pmtadd);
                    return null;
                }
                else
                {
                    var script1 = @"ErrMsg(""Fee Type Already Added "");";
                    return JavaScript(script1);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult AddApproveAssign(string feestruct, string status, string section, string grade, string name)
        {
            try
            {
                AdmissionManagementService ads = new AdmissionManagementService();

                Dictionary<string, object> criteria2 = new Dictionary<string, object>();
                criteria2.Add("PreRegNum", Convert.ToInt64(Session["preregno"]));
                Dictionary<long, IList<StudentTemplateView>> sdt = ads.GetStudentTemplateViewListWithEQsearchCriteria(0, 50, string.Empty, string.Empty, criteria2);
                Session["preregid"] = sdt.First().Value[0].Id;

                StudentTemplate st = new StudentTemplate();
                st.FamilyDetailsList = null;
                st.PastSchoolDetailsList = null;
                st.ApproveAssignList = null;
                st.PaymentDetailsList = null;

                st.Id = Convert.ToInt64(Session["preregid"]); // preregid;
                st.FeeStructYear = feestruct;
                st.Status = status;
                st.Section = section;
                st.Grade = Session["grad"].ToString(); // grad;

                st.ApplicationNo = sdt.First().Value[0].ApplicationNo;
                st.PreRegNum = sdt.First().Value[0].PreRegNum;
                st.Name = sdt.First().Value[0].Name;
                st.Gender = sdt.First().Value[0].Gender;
                st.DOB = sdt.First().Value[0].DOB;
                st.Transport = sdt.First().Value[0].Transport;
                st.Food = sdt.First().Value[0].Food;
                st.BoardingType = sdt.First().Value[0].BoardingType;
                st.EducationGoalYesorNo = sdt.First().Value[0].EducationGoalYesorNo;
                st.EducationGoals = sdt.First().Value[0].EducationGoals;
                st.AcademicYear = sdt.First().Value[0].AcademicYear;
                st.Campus = sdt.First().Value[0].Campus;
                st.BGRP = sdt.First().Value[0].BGRP;
                st.AnnualIncome = sdt.First().Value[0].AnnualIncome;
                st.LanguagesKnown = sdt.First().Value[0].LanguagesKnown;
                st.CreatedDate = sdt.First().Value[0].CreatedDate;

                Dictionary<string, object> criteria1 = new Dictionary<string, object>();
                criteria1.Add("Grade", grade);
                criteria1.Add("Status", "Registered");
                Dictionary<long, IList<StudentTemplateView>> StudentIdcnt = ads.GetStudentTemplateViewListWithEQsearchCriteria(null, null, null, null, criteria1);

                if (status == "Registered")
                {
                    string x = (DateTime.Now.Year - Convert.ToInt32(grade)).ToString();
                    var y = x.Substring(2);
                    st.NewId = "I" + feestruct + "-" + y + "-" + (Convert.ToInt32(StudentIdcnt.First().Value.Count + 1));
                    ads.CreateOrUpdateStudentDetails(st);
                }
                else
                {
                    ads.CreateOrUpdateStudentDetails(st);
                    var script1 = @"InfoMsg(""This Activity Has Already Been Performed "");";
                    return JavaScript(script1);
                }
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult AddDocumentdetails(string doctype, string docdata)
        {
            return null;
        }

        public ActionResult StudentsRegisteredCnt(string grade, string section, string acadyr, string feestructyr)
        {
            try
            {
                string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                //  if (userid == "Ashok")
                if (Session["userrole"] == "ADM-APP")
                {
                    AdmissionManagementService ads = new AdmissionManagementService();
                    Dictionary<string, object> criteria3 = new Dictionary<string, object>();
                    criteria3.Add("Grade", grade);
                    criteria3.Add("Section", section);
                    criteria3.Add("FeeStructYear", feestructyr);
                    criteria3.Add("AdmissionStatus", "Registered");
                    Dictionary<long, IList<StudentTemplateView>> StudentIdcnt1 = ads.GetStudentTemplateViewListWithEQsearchCriteria(0, 10000, string.Empty, string.Empty, criteria3);
                    ViewBag.studentcnt = StudentIdcnt1.First().Value.Count;
                }
                return Json(ViewBag.studentcnt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult StudentManagement(string pagename, string type, string resetsession)
        {
            try
            {
                if (resetsession != "no")
                {
                    ResetSession("");
                }
                if (pagename != null)
                {
                    Session["pagename"] = pagename.ToString();
                }
                else { Session["pagename"] = ""; }
                if (pagename != "transfer")
                {
                    Session["transferpdf"] = "";
                }
                if (type == "yes")
                {
                    ViewBag.transferpdf = "discontinuepdf";
                }
                if (type == "SMSsent")
                {
                    ViewBag.SMSsent = "yes";
                }
                if (type == "NoSMS")
                {
                    ViewBag.SMSsent = "no";
                }

                var rle = Session["userrolesarray"] as IEnumerable<string>;
                if (rle != null)
                {
                    if (rle.Contains("ADM-PMN")) { ViewBag.pmnbtn = "ADM-PMN"; }
                    if (rle.Contains("ADM-TFR")) { ViewBag.tfrbtn = "ADM-TFR"; }
                }
                MastersService ms = new MastersService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                var usrcmp = Session["UserCampus"] as IEnumerable<string>;
                if (usrcmp != null && usrcmp.Count() != 0)            // to check if the usrcmp obj is null or with dat)
                {
                    criteria.Add("Name", usrcmp);
                }
                Dictionary<long, IList<CampusMaster>> CampusMaster = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                criteria.Clear();
                Dictionary<long, IList<GradeMaster>> GradeMaster = ms.GetGradeMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                Dictionary<long, IList<AcademicyrMaster>> AcademicyrMaster = ms.GetAcademicyrMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                Dictionary<long, IList<SectionMaster>> SectionMaster = ms.GetSectionMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                Dictionary<long, IList<FeeStructureYearMaster>> FeeStructyrMaster = ms.GetFeeStructureYearMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                ViewBag.campusddl = CampusMaster.First().Value;
                ViewBag.gradeddl1 = GradeMaster.First().Value;
                ViewBag.acadddl = AcademicyrMaster.First().Value;
                ViewBag.sectionddl = SectionMaster.First().Value;
                ViewBag.feestructddl = FeeStructyrMaster.First().Value;
                if (Session["registered"] == "yes")
                {
                    ViewBag.Registered = "yes";
                    ViewBag.RegId = Session["regid"];  // regid;
                    Session["registered"] = "";
                    Session["regid"] = "";
                }
                Session["editid"] = 0;
                StudentTemplate st = new StudentTemplate();
                User Userobj = (User)Session["objUser"];
                if (Userobj != null)
                {
                    st.Campus = Userobj.Campus;
                }
                string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                ViewBag.StudentMgmtSearched = Session["StudentMgmtSearched"];
                return View(st);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult EmailId(long[] PreRegNo)
        {
            return null;
        }

        public ActionResult StudentEmail(string PreRegNo, string campus)
        {
            Session["emailpreregno"] = PreRegNo;
            Session["emailcmps"] = campus;
            Session["attachment"] = "";
            Session["attachmentnames"] = "";
            return View();
        }

        [HttpPost]
        public ActionResult StudentEmail(StudentTemplate st, HttpPostedFileBase file1)
        {
            try
            {
                AdmissionManagementService ads = new AdmissionManagementService();

                var prereg = Session["emailpreregno"].ToString().Split(',');
                var cmps = Session["emailcmps"].ToString().Split(',');
                var distcmps = cmps.Distinct().ToList();
                string attachmentid = Session["attachment"].ToString();
                new Task(() => { StudentEmailLoop(prereg, distcmps, cmps, st, attachmentid); }).Start();

                Session["emailsent"] = "yes";
                return RedirectToAction("StudentManagement", new { pagename = "email" });
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public void StudentEmailLoop(IList prereg, IList distcmps, IList cmps, StudentTemplate st, string attachmentid)
        {
            AdmissionManagementService ads = new AdmissionManagementService();
            #region emailloop
            for (int l = 0; l < distcmps.Count; l++)
            {
                IList<CampusEmailId> campusemaildet = GetEmailIdByCampus(distcmps[l].ToString(), ConfigurationManager.AppSettings["CampusEmailType"].ToString());

                long[] id = new long[prereg.Count];
                int j = 0;
                string From = ConfigurationManager.AppSettings["From"];
                string To = ConfigurationManager.AppSettings["To"];

                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                if (st.Subject != null)
                {
                    mail.Subject = st.Subject;
                }
                if (attachmentid != "")
                {
                    var ar = attachmentid.ToString().Split(',');// Session["attachment"].ToString().Split(',');
                    foreach (var var in ar)
                    {
                        Dictionary<string, object> criteria3 = new Dictionary<string, object>();
                        criteria3.Add("Id", Convert.ToInt64(var));
                        Dictionary<long, IList<EmailAttachment>> emailattachment = ads.GetEmailAttachmentListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria3);
                        MemoryStream ms = new MemoryStream(emailattachment.First().Value[0].Attachment);
                        Attachment mailAttach = new Attachment(ms, emailattachment.First().Value[0].AttachmentName.ToString());  //Data posted from form
                        mail.Attachments.Add(mailAttach);
                    }
                }

                string Body = st.Email;

                Body = Body.Replace("\r\n", "<br/>");

                mail.Body = Body;
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient("localhost", 25);
                smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address  
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.EnableSsl = true;

                int mailcnt = 0;

                foreach (string val in prereg)
                {
                    if (distcmps[l].ToString() == cmps[j].ToString())   // to send email campus wise according to the campus email id
                    {
                        EmailLog el = new EmailLog();

                        mailcnt = mailcnt + 1;
                        id[j] = Convert.ToInt64(val);

                        if (st.Father == true)
                        {
                            Dictionary<string, object> criteria1 = new Dictionary<string, object>();
                            criteria1.Add("PreRegNum", id[j]);
                            criteria1.Add("FamilyDetailType", "Father");
                            Dictionary<long, IList<FamilyDetails>> FamilyDetails = ads.GetFamilyDetailsListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria1);

                            if (FamilyDetails.First().Value.Count() != 0)
                            {
                                mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
                                if ((!string.IsNullOrEmpty(FamilyDetails.First().Value[0].Email)) && (FamilyDetails.First().Value[0].Email.Contains("@")))
                                {
                                    if (Regex.IsMatch(FamilyDetails.First().Value[0].Email,
            @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$",
            RegexOptions.IgnoreCase))
                                    {
                                        mail.To.Add(FamilyDetails.First().Value[0].Email);
                                        emailsendloop(campusemaildet, From, To, attachmentid, mail, smtp);
                                    }
                                    else
                                    {
                                        el.Id = 0;
                                        el.IsSent = false;
                                        StudentTemplate stud = ads.GetStudentDetailsByPreRegNo(FamilyDetails.First().Value[0].PreRegNum);
                                        if (stud != null) { el.StudName = stud.Name; el.NewId = stud.NewId; }
                                        el.EmailTo = FamilyDetails.First().Value[0].Email;
                                        el.EmailFrom = campusemaildet.First().EmailId.ToString();
                                        el.Attachment = attachmentid;
                                        emaillogloop(From, To, mail, el);
                                    }
                                }
                                else
                                {
                                    el.Id = 0;
                                    el.IsSent = false;
                                    StudentTemplate stud = ads.GetStudentDetailsByPreRegNo(FamilyDetails.First().Value[0].PreRegNum);
                                    if (stud != null) { el.StudName = stud.Name; el.NewId = stud.NewId; }
                                    el.EmailTo = FamilyDetails.First().Value[0].Email;
                                    el.EmailFrom = campusemaildet.First().EmailId.ToString();
                                    el.Attachment = attachmentid;
                                    emaillogloop(From, To, mail, el);
                                }
                            }
                        }

                        if (st.Mother == true)
                        {
                            Dictionary<string, object> criteria2 = new Dictionary<string, object>();
                            criteria2.Add("PreRegNum", id[j]);
                            criteria2.Add("FamilyDetailType", "Mother");
                            Dictionary<long, IList<FamilyDetails>> FamilyDetails = ads.GetFamilyDetailsListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria2);

                            if (FamilyDetails.First().Value.Count() != 0)
                            {
                                mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
                                if ((!string.IsNullOrEmpty(FamilyDetails.First().Value[0].Email)) && (FamilyDetails.First().Value[0].Email.Contains("@")))
                                {
                                    if (Regex.IsMatch(FamilyDetails.First().Value[0].Email,
            @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$",
            RegexOptions.IgnoreCase))
                                    {
                                        mail.To.Add(FamilyDetails.First().Value[0].Email);
                                        emailsendloop(campusemaildet, From, To, attachmentid, mail, smtp);
                                    }
                                    else
                                    {
                                        el.Id = 0;
                                        el.IsSent = false;
                                        StudentTemplate stud = ads.GetStudentDetailsByPreRegNo(FamilyDetails.First().Value[0].PreRegNum);
                                        if (stud != null) { el.StudName = stud.Name; el.NewId = stud.NewId; }
                                        el.EmailTo = FamilyDetails.First().Value[0].Email;
                                        el.EmailFrom = campusemaildet.First().EmailId.ToString();
                                        el.Attachment = attachmentid;
                                        emaillogloop(From, To, mail, el);
                                    }
                                }
                                else
                                {
                                    el.Id = 0;
                                    el.IsSent = false;
                                    StudentTemplate stud = ads.GetStudentDetailsByPreRegNo(FamilyDetails.First().Value[0].PreRegNum);
                                    if (stud != null) { el.StudName = stud.Name; el.NewId = stud.NewId; }
                                    el.EmailTo = FamilyDetails.First().Value[0].Email;
                                    el.EmailFrom = campusemaildet.First().EmailId.ToString();
                                    el.Attachment = attachmentid;
                                    emaillogloop(From, To, mail, el);
                                }
                            }
                        }

                        if (st.General == true)
                        {
                            Dictionary<string, object> criteria3 = new Dictionary<string, object>();
                            criteria3.Add("PreRegNum", id[j]);
                            Dictionary<long, IList<StudentTemplateView>> StudentTemplate = ads.GetStudentTemplateViewListWithEQsearchCriteria(0, 10000, string.Empty, string.Empty, criteria3);
                            if (StudentTemplate.First().Value.Count() != 0)
                            {
                                mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
                                if ((!string.IsNullOrEmpty(StudentTemplate.First().Value[0].EmailId)) && (StudentTemplate.First().Value[0].EmailId.Contains("@")))
                                {
                                    if (Regex.IsMatch(StudentTemplate.First().Value[0].EmailId,
                                            @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                                            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$",
                                            RegexOptions.IgnoreCase))
                                    {
                                        mail.To.Add(StudentTemplate.First().Value[0].EmailId);
                                        emailsendloop(campusemaildet, From, To, attachmentid, mail, smtp);
                                    }
                                    else
                                    {
                                        el.Id = 0;
                                        el.IsSent = false;
                                        el.StudName = StudentTemplate.First().Value[0].Name;
                                        el.NewId = StudentTemplate.First().Value[0].NewId;
                                        el.EmailTo = StudentTemplate.First().Value[0].EmailId;
                                        el.EmailFrom = campusemaildet.First().EmailId.ToString();
                                        el.Attachment = attachmentid;
                                        emaillogloop(From, To, mail, el);
                                    }
                                }
                                else
                                {
                                    el.Id = 0;
                                    el.IsSent = false;
                                    el.StudName = StudentTemplate.First().Value[0].Name;
                                    el.NewId = StudentTemplate.First().Value[0].NewId;
                                    el.EmailTo = StudentTemplate.First().Value[0].EmailId;
                                    el.EmailFrom = campusemaildet.First().EmailId.ToString();
                                    el.Attachment = attachmentid;
                                    emaillogloop(From, To, mail, el);
                                }
                            }
                        }
                    }
                    j++;
                }
            }
            #endregion emailloop
        }

        public ActionResult CampusEmail(string campusname)
        {
            Session["attachment"] = "";
            Session["attachmentnames"] = "";
            return View();
        }

        [HttpPost]
        public ActionResult CampusEmail(StudentTemplate st, HttpPostedFileBase file2)
        {
            try
            {
                //get all emailids, with message to send and attachments
                //log one by one in the table and update as success first time, if mail sent success then leave it otherwise mark it as false as second db hit
                //if success leave the attachment unstored//but if it is failure then sotre the attachment too
                AdmissionManagementService ads = new AdmissionManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();

                var usrcmp = Session["UserCampus"] as IEnumerable<string>;
                string[] str = new string[usrcmp.ToList().Count];
                int i = 0;
                foreach (var var in usrcmp)
                {
                    str[i] = var;
                    i++;
                }

                if (usrcmp.Count() != 0)
                {
                    if (usrcmp.First() != null)            // to check if the usrcmp obj is null or with data
                    {
                        if (Session["cmpnm"].ToString() != "")
                        {
                            if (usrcmp.Contains(Session["cmpnm"].ToString()))
                            {
                                criteria.Add("Campus", Session["cmpnm"].ToString());
                            }
                            else { criteria.Add("Campus", "no campus"); }
                        }
                        else
                        { criteria.Add("Campus", str); }
                    }
                    else
                    {
                        criteria.Add("Campus", str);
                    }
                }
                else { criteria.Add("Campus", "no campus"); }

                string colName = string.Empty; string[] values = new string[1];
                if (Session["grd"].ToString() != "")
                {
                    var usrgrd = Session["grd"] as IEnumerable<string>;
                    values = usrgrd.ToArray();
                    colName = "Grade";
                }
                if (Session["sect"].ToString() != "")
                {
                    criteria.Add("Section", Session["sect"].ToString());
                }
                if (Session["acdyr"].ToString() != "")
                {
                    criteria.Add("AcademicYear", Session["acdyr"].ToString());
                }

                if (Session["apnam"].ToString() != "")
                {
                    criteria.Add("Name", Session["apnam"].ToString());
                }
                if (Session["appno"].ToString() != "")
                {
                    criteria.Add("ApplicationNo", Session["appno"].ToString());
                }
                if (Session["ishosteller"].ToString() != "")
                {
                    criteria.Add("IsHosteller", Session["ishosteller"]);
                }
                if (Session["feestructyr"].ToString() != "")
                {
                    criteria.Add("FeeStructYear", Session["feestructyr"].ToString());
                }
                if (Session["idnum"].ToString() != "")
                {
                    criteria.Add("NewId", Session["idnum"]);
                }

                if (Session["stats"].ToString() != "")
                {
                    criteria.Add("AdmissionStatus", Session["stats"].ToString());
                }
                else { criteria.Add("AdmissionStatus", "Registered"); }

                Dictionary<long, IList<StudentTemplateView>> preregnum;
                preregnum = ads.GetStudentDetailsListWithPagingAndCriteriaWithAlias(0, 10000, null, null, colName, values, criteria, null);
                string attachmentid = Session["attachment"].ToString();
                new Task(() => { CampusEmailLoop(preregnum, st, attachmentid); }).Start();

                Session["emailsent"] = "yes";
                Session["attachment"] = "";
                Session["attachmentnames"] = "";
                return RedirectToAction("StudentManagement", new { pagename = "email" });
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public void CampusEmailLoop(Dictionary<long, IList<StudentTemplateView>> prereglist, StudentTemplate st, string attachmentid)
        {
            AdmissionManagementService ads = new AdmissionManagementService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            IEnumerable<StudentTemplateView> abc = prereglist.First().Value.ToList();
            var distcmp = abc.Select(x => x.Campus).Distinct().ToList();      // get distinct campus of the students from the grid list

            BaseController bc = new BaseController();

            #region emailloop
            for (int l = 0; l < distcmp.Count(); l++)
            {
                IList<CampusEmailId> campusemaildet = GetEmailIdByCampus(distcmp[l].ToString(), ConfigurationManager.AppSettings["CampusEmailType"].ToString());
                string From = ConfigurationManager.AppSettings["From"];
                string To = ConfigurationManager.AppSettings["To"];
                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();

                if (st.Subject != null)
                {
                    mail.Subject = st.Subject;
                }
                if (attachmentid.ToString() != "")
                {
                    var ar = attachmentid.ToString().Split(','); // Session["attachment"].ToString().Split(',');
                    foreach (var var in ar)
                    {
                        Dictionary<string, object> criteria3 = new Dictionary<string, object>();
                        criteria3.Add("Id", Convert.ToInt64(var));
                        Dictionary<long, IList<EmailAttachment>> emailattachment = ads.GetEmailAttachmentListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria3);
                        MemoryStream ms = new MemoryStream(emailattachment.First().Value[0].Attachment);
                        Attachment mailAttach = new Attachment(ms, emailattachment.First().Value[0].AttachmentName.ToString());  //Data posted from form
                        mail.Attachments.Add(mailAttach);
                    }
                }
                string Body = st.Email;
                Body = Body.Replace("\r\n", "<br/>");
                mail.Body = Body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient("localhost", 25);
                smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address  
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.EnableSsl = true;

                int mailcnt = 0;
                int quotient = Convert.ToInt32((prereglist.First().Key) / Convert.ToInt32(ConfigurationManager.AppSettings["EmailCnt"]));
                int remainder = Convert.ToInt32((prereglist.First().Key) % Convert.ToInt32(ConfigurationManager.AppSettings["EmailCnt"]));
                int cnt = 1;

                for (int j = 0; j < prereglist.First().Key; j++)
                {
                    EmailLog el = new EmailLog();
                    if (distcmp[l].ToString() == prereglist.First().Value[j].Campus.ToString())   // to send email campus wise according to the campus email id
                    {
                        mailcnt = mailcnt + 1;
                        el.Subject = st.Subject;
                        el.Message = st.Email;
                        if (st.Father == true)
                        {
                            Dictionary<string, object> criteria1 = new Dictionary<string, object>();
                            criteria1.Add("PreRegNum", prereglist.First().Value[j].PreRegNum);
                            criteria1.Add("FamilyDetailType", "Father");
                            Dictionary<long, IList<FamilyDetails>> FamilyDetails = ads.GetFamilyDetailsListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria1);
                            if (FamilyDetails.First().Value.Count() != 0)
                            {
                                mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
                                if ((!string.IsNullOrEmpty(FamilyDetails.First().Value[0].Email)) && (FamilyDetails.First().Value[0].Email.Contains("@")))
                                {
                                    if (Regex.IsMatch(FamilyDetails.First().Value[0].Email,
            @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$",
            RegexOptions.IgnoreCase))
                                    {
                                        mail.To.Add(FamilyDetails.First().Value[0].Email);
                                        emailsendloop(campusemaildet, From, To, attachmentid, mail, smtp);
                                    }
                                    else
                                    {
                                        el.Id = 0;
                                        el.IsSent = false;
                                        StudentTemplate stud = ads.GetStudentDetailsByPreRegNo(FamilyDetails.First().Value[0].PreRegNum);
                                        if (stud != null) { el.StudName = stud.Name; el.NewId = stud.NewId; }
                                        el.EmailTo = FamilyDetails.First().Value[0].Email;
                                        el.EmailFrom = campusemaildet.First().EmailId.ToString();
                                        el.Attachment = attachmentid;
                                        emaillogloop(From, To, mail, el);
                                    }
                                }
                                else
                                {
                                    el.Id = 0;
                                    el.IsSent = false;
                                    StudentTemplate stud = ads.GetStudentDetailsByPreRegNo(FamilyDetails.First().Value[0].PreRegNum);
                                    if (stud != null) { el.StudName = stud.Name; el.NewId = stud.NewId; }
                                    el.EmailTo = FamilyDetails.First().Value[0].Email;
                                    el.EmailFrom = campusemaildet.First().EmailId.ToString();
                                    el.Attachment = attachmentid;
                                    emaillogloop(From, To, mail, el);
                                }
                            }
                        }

                        if (st.Mother == true)
                        {
                            Dictionary<string, object> criteria2 = new Dictionary<string, object>();
                            criteria2.Add("PreRegNum", prereglist.First().Value[j].PreRegNum);
                            criteria2.Add("FamilyDetailType", "Mother");
                            Dictionary<long, IList<FamilyDetails>> FamilyDetails = ads.GetFamilyDetailsListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria2);

                            if (FamilyDetails.First().Value.Count() != 0)
                            {
                                mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
                                if ((!string.IsNullOrEmpty(FamilyDetails.First().Value[0].Email)) && (FamilyDetails.First().Value[0].Email.Contains("@")))
                                {
                                    if (Regex.IsMatch(FamilyDetails.First().Value[0].Email,
            @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$",
            RegexOptions.IgnoreCase))
                                    {
                                        mail.To.Add(FamilyDetails.First().Value[0].Email);
                                        emailsendloop(campusemaildet, From, To, attachmentid, mail, smtp);
                                    }
                                    else
                                    {
                                        el.Id = 0;
                                        el.IsSent = false;
                                        StudentTemplate stud = ads.GetStudentDetailsByPreRegNo(FamilyDetails.First().Value[0].PreRegNum);
                                        if (stud != null) { el.StudName = stud.Name; el.NewId = stud.NewId; }
                                        el.EmailTo = FamilyDetails.First().Value[0].Email;
                                        el.EmailFrom = campusemaildet.First().EmailId.ToString();
                                        el.Attachment = attachmentid;
                                        emaillogloop(From, To, mail, el);
                                    }
                                }
                                else
                                {
                                    el.Id = 0;
                                    el.IsSent = false;
                                    StudentTemplate stud = ads.GetStudentDetailsByPreRegNo(FamilyDetails.First().Value[0].PreRegNum);
                                    if (stud != null) { el.StudName = stud.Name; el.NewId = stud.NewId; }
                                    el.EmailTo = FamilyDetails.First().Value[0].Email;
                                    el.EmailFrom = campusemaildet.First().EmailId.ToString();
                                    el.Attachment = attachmentid;
                                    emaillogloop(From, To, mail, el);
                                }
                            }
                        }

                        if (st.General == true)
                        {
                            Dictionary<string, object> criteria3 = new Dictionary<string, object>();
                            criteria3.Add("PreRegNum", prereglist.First().Value[j].PreRegNum);
                            Dictionary<long, IList<StudentTemplateView>> StudentTemplate = ads.GetStudentTemplateViewListWithEQsearchCriteria(0, 10000, string.Empty, string.Empty, criteria3);

                            if (StudentTemplate.First().Value.Count() != 0)
                            {
                                mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
                                if ((!string.IsNullOrEmpty(StudentTemplate.First().Value[0].EmailId)) && (StudentTemplate.First().Value[0].EmailId.Contains("@")))
                                {
                                    if (Regex.IsMatch(StudentTemplate.First().Value[0].EmailId,
            @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$",
            RegexOptions.IgnoreCase))
                                    {
                                        mail.To.Add(StudentTemplate.First().Value[0].EmailId);
                                        emailsendloop(campusemaildet, From, To, attachmentid, mail, smtp);
                                    }
                                    else
                                    {
                                        el.Id = 0;
                                        el.IsSent = false;
                                        el.NewId = StudentTemplate.First().Value[0].NewId;
                                        el.StudName = StudentTemplate.First().Value[0].Name;
                                        el.EmailTo = StudentTemplate.First().Value[0].EmailId;
                                        el.EmailFrom = campusemaildet.First().EmailId.ToString();
                                        el.Attachment = attachmentid;
                                        emaillogloop(From, To, mail, el);
                                    }
                                }
                                else
                                {
                                    el.Id = 0;
                                    el.IsSent = false;
                                    el.NewId = StudentTemplate.First().Value[0].NewId;
                                    el.StudName = StudentTemplate.First().Value[0].Name;
                                    el.EmailTo = StudentTemplate.First().Value[0].EmailId;
                                    el.EmailFrom = campusemaildet.First().EmailId.ToString();
                                    el.Attachment = attachmentid;
                                    emaillogloop(From, To, mail, el);
                                }
                            }
                        }
                    }
                }
            }
            #endregion emailloop
        }

        public void emailsendloop(IList<CampusEmailId> campusemaildet, string From, string To, string attachmentid, MailMessage mail, SmtpClient smtp)
        {
            EmailLog el = new EmailLog();

            if (From == "live")
            {
                smtp.Credentials = new System.Net.NetworkCredential
               (campusemaildet.First().EmailId.ToString(), campusemaildet.First().Password.ToString());
                el.EmailTo = mail.To.ToString();
                el.Subject = mail.Subject;
                el.Message = mail.Body;
                el.EmailFrom = mail.From.ToString();
                el.Attachment = attachmentid;
                el.Id = 0;
                try
                {
                    if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                    {
                        smtp.Send(mail);
                        el.IsSent = true;
                    }
                    else
                    {
                        el.IsSent = false;
                    }
                    emaillogloop(From, To, mail, el);
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("quota"))
                    {
                        mail.From = new MailAddress(campusemaildet.FirstOrDefault().AlternateEmailId);
                        smtp.Credentials = new System.Net.NetworkCredential
                        (campusemaildet.FirstOrDefault().AlternateEmailId, campusemaildet.FirstOrDefault().AlternateEmailIdPassword);

                        if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                        {
                            smtp.Send(mail);
                            el.IsSent = true;
                            emaillogloop(From, To, mail, el);
                        }
                    }
                    else if (!ex.Message.Contains("quota"))
                    {
                        mail.From = new MailAddress(campusemaildet.FirstOrDefault().AlternateEmailId);
                        smtp.Credentials = new System.Net.NetworkCredential
                        (campusemaildet.FirstOrDefault().AlternateEmailId, campusemaildet.FirstOrDefault().AlternateEmailIdPassword);

                        if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                        {
                            smtp.Send(mail);
                            el.IsSent = true;
                            emaillogloop(From, To, mail, el);
                        }
                    }
                    else
                    {
                        el.IsSent = false;
                        el.ActualException = ex.Message;
                        emaillogloop(From, To, mail, el);
                    }
                }
            }
            else if (From == "test")
            {
                smtp.Credentials = new System.Net.NetworkCredential
               (campusemaildet.First().EmailId.ToString(), campusemaildet.First().Password.ToString());
                el.EmailTo = mail.To.ToString();
                el.Subject = mail.Subject;
                el.Message = mail.Body;
                el.EmailFrom = mail.From.ToString();
                el.Attachment = attachmentid;
                el.Id = 0;
                try
                {
                    if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                    {
                        smtp.Send(mail);
                        el.IsSent = true;
                    }
                    else
                    {
                        el.IsSent = false;
                    }
                    emaillogloop(From, To, mail, el);
                }
                catch (Exception ex)
                {
                    el.IsSent = false;
                    el.ActualException = ex.Message;
                    emaillogloop(From, To, mail, el);
                }
            }
        }

        public void emaillogloop(string From, string To, MailMessage mail, EmailLog el)
        {
            AdmissionManagementService ads = new AdmissionManagementService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            el.EmailCC = mail.CC.ToString();
            if (mail.Bcc.ToString().Length < 3990)
            {
                el.EmailBCC = mail.Bcc.ToString();
            }
            el.Subject = mail.Subject.ToString();
            if (mail.Body.ToString().Length < 3990)
            {
                el.Message = mail.Body;
            }
            el.EmailDateTime = DateTime.Now.ToString();
            el.BCC_Count = mail.Bcc.Count;
            ads.CreateOrUpdateEmailLog(el);
            mail.To.Clear();
        }

        public ActionResult MailAttachments1(HttpPostedFileBase file1)
        {
            string[] strAttachname = file1.FileName.Split('\\');
            Attachment mailAttach = new Attachment(file1.InputStream, strAttachname[strAttachname.Length - 1]);  //Data posted from form

            byte[] imageSize = new byte[file1.ContentLength];
            file1.InputStream.Read(imageSize, 0, (int)file1.ContentLength);
            EmailAttachment ea = new EmailAttachment();
            ea.Attachment = imageSize;
            ea.AttachmentName = strAttachname.First().ToString();

            AdmissionManagementService ams = new AdmissionManagementService();
            ams.CreateOrUpdateEmailAttachment(ea);

            Dictionary<string, object> criteria3 = new Dictionary<string, object>();
            var maxattachemntid = ams.GetMaxAttachmentId();

            if ((Session["attachment"].ToString() != ""))
            {
                Session["attachment"] = Session["attachment"] + "," + maxattachemntid;
            }
            else
            {
                Session["attachment"] = maxattachemntid;
            }

            Session["attachmentnames"] = strAttachname.First().ToString();

            return Json(new { success = true, result = strAttachname.First().ToString() }, "text/html", JsonRequestBehavior.AllowGet);
        }

        public ActionResult MailAttachments2(HttpPostedFileBase file2)
        {
            string[] strAttachname = file2.FileName.Split('\\');
            Attachment mailAttach = new Attachment(file2.InputStream, strAttachname[strAttachname.Length - 1]);  //Data posted from form

            byte[] imageSize = new byte[file2.ContentLength];
            file2.InputStream.Read(imageSize, 0, (int)file2.ContentLength);
            EmailAttachment ea = new EmailAttachment();
            ea.Attachment = imageSize;
            ea.AttachmentName = strAttachname.First().ToString();

            AdmissionManagementService ams = new AdmissionManagementService();
            ams.CreateOrUpdateEmailAttachment(ea);

            Dictionary<string, object> criteria3 = new Dictionary<string, object>();
            var maxattachemntid = ams.GetMaxAttachmentId();

            if ((Session["attachment"].ToString() != ""))
            {
                Session["attachment"] = Session["attachment"] + "," + maxattachemntid;
            }
            else
            {
                Session["attachment"] = maxattachemntid;
            }

            Session["attachmentnames"] = strAttachname.First().ToString();
            return Json(new { success = true, result = strAttachname.First().ToString() }, "text/html", JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteAttachment()
        {
            try
            {
                AdmissionManagementService ads = new AdmissionManagementService();
                if (Session["attachment"].ToString() != "")
                {
                    var test = Session["attachment"].ToString().Split(',');

                    long[] idtodelete = new long[test.Length];
                    int i = 0;
                    foreach (string val in test)
                    {
                        idtodelete[i] = Convert.ToInt64(val);
                        i++;
                    }
                    ads.DeleteAttachment(idtodelete);
                    Session["attachment"] = "";
                    Session["attachmentnames"] = "";
                }
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult PrintIdCard(string PreRegNo)
        {
            try
            {
                Session["IdCardData"] = PreRegNo;

                ViewBag.IdHtmlTag = IdHtmlTags();
                return View();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public string IdHtmlTags()
        {
            try
            {
                AdmissionManagementService ads = new AdmissionManagementService();

                var prereg = Session["IdCardData"].ToString().Split(',');
                long[] id = new long[prereg.Length];
                int j = 0;
                foreach (string val in prereg)
                {
                    id[j] = Convert.ToInt64(val);
                    j++;
                }

                System.Text.StringBuilder html = new System.Text.StringBuilder();
                int rowcnt = 0;

                for (int i = 0; i < Convert.ToInt32(prereg.Count()); i = i + 2)
                {

                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("PreRegNum", id[i]);
                    Dictionary<long, IList<StudentTemplateView>> StudentIdcnt1 = ads.GetStudentTemplateViewListWithEQsearchCriteria(0, 10000, string.Empty, string.Empty, criteria);

                    if (StudentIdcnt1.First().Value[0].BGRP == "Not Given")
                    {
                        StudentIdcnt1.First().Value[0].BGRP = "";
                    }
                    var mobile = "";
                    if (!string.IsNullOrEmpty(StudentIdcnt1.First().Value[0].MobileNo))
                    {
                        mobile = StudentIdcnt1.First().Value[0].MobileNo.Split(',').ToString();
                    }
                    //Dictionary<string, object> criteria3 = new Dictionary<string, object>();
                    //criteria3.Add("PreRegNum", id[i]);
                    //criteria3.Add("FamilyDetailType", "Father");
                    //Dictionary<long, IList<FamilyDetails>> StudentIdcnt3 = ads.GetFamilyDetailsListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria3);

                    //var mobi1 = "";
                    //var mobi2 = "";
                    //if (StudentIdcnt3.First().Key == 0)
                    //{
                    //    mobi1 = "";
                    //}
                    //else
                    //{
                    //    mobi1 = StudentIdcnt3.First().Value[0].Mobile;
                    //    if ((mobi1 != "") && (mobi1 != null))
                    //    {
                    //        mobi1 = mobi1.Split(',')[0].ToString();
                    //    }
                    //}

                    Dictionary<long, IList<StudentTemplateView>> StudentIdcnt2 = null;
                    //Dictionary<long, IList<FamilyDetails>> StudentIdcnt4 = null;

                    if (i + 1 < Convert.ToInt32(prereg.Count()))
                    {
                        Dictionary<string, object> criteria2 = new Dictionary<string, object>();
                        criteria2.Add("PreRegNum", id[i + 1]);
                        StudentIdcnt2 = ads.GetStudentTemplateViewListWithEQsearchCriteria(0, 10000, string.Empty, string.Empty, criteria2);

                        if (StudentIdcnt2.First().Value[0].BGRP == "Not Given")
                        {
                            StudentIdcnt2.First().Value[0].BGRP = "";
                        }

                        //Dictionary<string, object> criteria4 = new Dictionary<string, object>();
                        //criteria4.Add("PreRegNum", id[i + 1]);
                        //criteria4.Add("FamilyDetailType", "Father");
                        //StudentIdcnt4 = ads.GetFamilyDetailsListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria4);

                        //if (StudentIdcnt4.First().Key == 0)
                        //{
                        //    mobi2 = "";
                        //}
                        //else
                        //{
                        //    mobi2 = StudentIdcnt4.First().Value[0].Mobile;

                        //    if ((mobi2 != "") && (mobi2 != null))
                        //    {
                        //        mobi2 = mobi2.Split(',')[0].ToString();
                        //    }
                        //}
                    }
                    rowcnt = rowcnt + 1;
                    html.Append("<tr style='page-break-inside:avoid'>");
                    html.Append("<td style='padding-bottom:20px; padding-right:25px'>");
                    html.Append("<div style='width: 350px;'>");

                    html.Append("<span style='font-size: 10pt; float:left'><img src='../../Images/left-top-org.png' height='70';width='80'/><br/>    </span>");

                    html.Append("<span style='font-size: 10pt; float:right; padding-right:10px; padding-top:10px'><img src='../../Images/Tips Logo.JPG' height='45';width='30'/><br/>    </span>");
                    html.Append("<br />");
                    html.Append("<span style='font-size: 8pt; padding-left:25px'>  <img src='" + Url.Action("uploaddisplay1", "Admission", new { id = id[i] }) + "' id='image1' name='logoFile' width= '80' height='80' style='padding-top:16px' /> </span><br />");

                    html.Append("<span style='font-family: Arial; font-size:12px'><div style=' padding-top:3px; padding-left:130px; height: 7px'><strong> " + StudentIdcnt1.First().Value[0].Name + "  </strong></div><br />");
                    html.Append("<div style=' padding-left:130px; height: 7px'><strong>Grade &nbsp" + StudentIdcnt1.First().Value[0].Grade + "</strong></div><br />");
                    html.Append("<div style=' padding-left:130px; height: 7px'><strong>" + StudentIdcnt1.First().Value[0].NewId + "</strong></div><br />");
                    html.Append("<div style='padding-left:8px; height: 19px'><strong>Blood Group : &nbsp" + StudentIdcnt1.First().Value[0].BGRP + "&nbsp&nbsp  Emergency No: &nbsp " + mobile + "</strong></div></span>");
                    html.Append("<span style='font-size: 10pt; float:right'><img src='../../Images/new_bottom.jpg'   style='width:350px; height:26px;'/> </span>");
                    html.Append("<div/>");
                    html.Append("<td/>");

                    if (i + 1 < Convert.ToInt32(prereg.Count()))
                    {
                        html.Append("<td style='padding-bottom:20px'>");
                        html.Append("<div style='width: 350px'>");

                        html.Append("<span style='font-size: 10pt; float:left'><img src='../../Images/left-top-org.png' height='70';width='80'/><br/>    </span>");

                        html.Append("<span style='font-size: 10pt; float:right; padding-right:10px; padding-top:10px'><img src='../../Images/Tips Logo.JPG' height='45';width='30'/><br/>    </span>");
                        html.Append("<br />");
                        html.Append("<span style='font-size: 8pt; padding-left:25px'>  <img src='" + Url.Action("uploaddisplay1", "Admission", new { id = id[i + 1] }) + "' id='image1' name='logoFile' width= '80' height='80' style='padding-top:16px'/> </span><br />");
                        html.Append("<span style='font-family: Arial; font-size:12px'><div style=' padding-top:3px; padding-left:130px; height: 7px'><strong> " + StudentIdcnt2.First().Value[0].Name + "<br />  </strong></div><br/>");
                        html.Append("<div style='padding-left:130px; height: 7px'><strong>Grade &nbsp" + StudentIdcnt2.First().Value[0].Grade + "</strong></div><br />");
                        html.Append("<div style='padding-left:130px; height: 7px'><strong>" + StudentIdcnt2.First().Value[0].NewId + "</strong></div><br />");
                        html.Append("<div style='padding-left:8px; height: 19px'><strong>Blood Group : &nbsp" + StudentIdcnt2.First().Value[0].BGRP + "&nbsp&nbsp  Emergency No: &nbsp " + mobile + "</strong></div></span>");
                        html.Append("<span style='font-size: 10pt; float:right'><img src='../../Images/new_bottom.jpg'   style='width:350px; height:26px;'/> </span>");

                        html.Append("<div/>");
                        html.Append("<td/>");
                    }
                    else
                    {
                        html.Append("<td style='padding-bottom:20px'>");
                        html.Append("<div style='width: 350px'>");
                        html.Append("<div/>");
                        html.Append("<td/>");
                    }

                    html.Append("</tr>");

                    if (rowcnt == 4)
                    {
                        html.Append("<tr style='page-break-after: always'>");
                        html.Append("</tr>");

                        rowcnt = 0;
                    }
                }
                return html.ToString();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult PrintPickupCard(string PreRegNo)
        {
            Session["PickupCardData"] = PreRegNo;

            ViewBag.PickupHtmlTag = PickupHtmlTags();
            return View();
        }

        public string PickupHtmlTags()
        {
            AdmissionManagementService ads = new AdmissionManagementService();

            var prereg = Session["PickupCardData"].ToString().Split(',');
            long[] id = new long[prereg.Length];

            int j = 0;
            foreach (string val in prereg)
            {
                id[j] = Convert.ToInt64(val);
                j++;
            }

            long[] id2 = new long[1000];
            long[] id3 = new long[1000];
            string[] idnam = new string[1000];
            string[] idno = new string[1000];

            int l = 0;
            for (int k = 0; k < Convert.ToInt32(prereg.Count()); k++)
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("PreRegNum", id[k]);
                criteria.Add("DocumentType", "Pickup Card");
                Dictionary<long, IList<UploadedFiles>> uploadedfile = ads.GetUploadedFilesListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria);
                //
                for (int y = 0; y < uploadedfile.First().Value.Count; y++)
                {
                    id2[l] = uploadedfile.First().Value[y].PreRegNum;
                    id3[l] = uploadedfile.First().Value[y].Id;
                    idnam[l] = uploadedfile.First().Value[y].Name;
                    idno[l] = uploadedfile.First().Value[y].Phone;
                    l++;
                }
            }

            System.Text.StringBuilder html = new System.Text.StringBuilder();
            int rowcnt = 0;
            for (int i = 0; i < l; i = i + 2)
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("PreRegNum", id2[i]);
                Dictionary<long, IList<StudentTemplateView>> StudentIdcnt1 = ads.GetStudentTemplateViewListWithEQsearchCriteria(0, 10000, string.Empty, string.Empty, criteria);

                Dictionary<long, IList<StudentTemplateView>> StudentIdcnt2 = null;

                if (i + 1 < l)
                {
                    Dictionary<string, object> criteria2 = new Dictionary<string, object>();
                    criteria2.Add("PreRegNum", id2[i + 1]);
                    StudentIdcnt2 = ads.GetStudentTemplateViewListWithEQsearchCriteria(0, 10000, string.Empty, string.Empty, criteria2);
                }
                rowcnt = rowcnt + 1;

                html.Append("<tr>");

                html.Append("<td style='padding-bottom:20px; padding-right:25px'>");
                html.Append("<div style='width: 350px'>");

                html.Append("<span style='font-size: 10pt; float:left'><img src='../../Images/left-top-org.png' height='70';width='180'/><br/>    </span>");

                html.Append("<span style='font-size: 10pt; float:right; padding-right:10px; padding-top:10px'><img src='../../Images/Tips Logo.JPG' height='45';width='30'/>    </span>");
                html.Append("<span style='font-size: 8pt; padding-left:25px'>  <img src='" + Url.Action("uploaddisplay1", "Admission", new { id = id2[i] }) + "' id='image1' name='logoFile' width= '80' height='80'  style='padding-top:35px'/></span>");
                html.Append("<br />");
                html.Append("<span style='font-size: 9pt; float:left; padding-left:30px; padding-bottom:9px'>  <img src='" + Url.Action("uploaddisplay2", "Admission", new { id = id3[i] }) + "' id='image1' name='logoFile' width= '80' height='80' style='padding-top:0px'/> </span>");

                html.Append("<div style='font-family: Arial; font-size:12px; padding-left:80px; padding-top:3px; margin-left:50px; height:20px'><strong> " + StudentIdcnt1.First().Value[0].Name + "</strong></div>");
                html.Append("<div style='font-family: Arial; font-size:12px; padding-left:80px; margin-left:50px; height:15px'><strong> " + StudentIdcnt1.First().Value[0].NewId + "</strong></div>");

                html.Append("<div style='font-family: Arial; font-size:12px; padding-left:80px; margin-left:50px; padding-top:30px; height:16px'><strong> " + idnam[i] + "</strong></div>");

                html.Append("<span style='float:right'><img src='../../Images/new_bottom.jpg'   style='width:350px; height:26px;'/> </span>");

                html.Append("<div/>");
                html.Append("<td/>");

                if (i + 1 < l)
                {
                    html.Append("<td style='padding-bottom:20px'>");
                    html.Append("<div style='width: 350px;'>");

                    html.Append("<span style='font-size: 10pt; float:left'><img src='../../Images/left-top-org.png' height='70';width='180'/><br/>    </span>");

                    html.Append("<span style='font-size: 10pt; float:right; padding-right:10px; padding-top:10px'><img src='../../Images/Tips Logo.JPG' height='45';width='30'/>    </span>");
                    html.Append("<span style='font-size: 8pt; padding-left:25px'>  <img src='" + Url.Action("uploaddisplay1", "Admission", new { id = id2[i + 1] }) + "' id='image1' name='logoFile' width= '80' height='80'  style='padding-top:35px'/></span>");
                    html.Append("<br />");
                    html.Append("<span style='font-size: 9pt; float:left; padding-left:30px; padding-bottom:9px'>  <img src='" + Url.Action("uploaddisplay2", "Admission", new { id = id3[i + 1] }) + "' id='image1' name='logoFile' width= '80' height='80' style='padding-top:0px'/> </span>");

                    html.Append("<div style='font-family: Arial; font-size:12px; padding-left:80px; padding-top:3px; margin-left:50px; height:20px'><strong> " + StudentIdcnt2.First().Value[0].Name + "</strong></div>");
                    html.Append("<div style='font-family: Arial; font-size:12px; padding-left:80px; margin-left:50px; height:15px'><strong> " + StudentIdcnt2.First().Value[0].NewId + "</strong></div>");

                    html.Append("<div style='font-family: Arial; font-size:12px; padding-left:80px; margin-left:50px; padding-top:30px; height:16px'><strong> " + idnam[i + 1] + "</strong></div>");

                    html.Append("<span style='float:right'><img src='../../Images/new_bottom.jpg'   style='width:350px; height:26px;'/> </span>");

                    html.Append("<div/>");
                    html.Append("<td/>");

                }
                else
                {
                    html.Append("<td style='padding-bottom:20px'>");
                    html.Append("<div style='width: 350px'>");
                    html.Append("<div/>");
                    html.Append("<td/>");
                }

                html.Append("</tr>");

                if (rowcnt == 4)
                {
                    html.Append("<tr style='page-break-after: always'>");
                    html.Append("</tr>");
                    rowcnt = 0;
                }
            }
            return html.ToString();
        }

        public ActionResult PrintSC(string PreRegNo, string sportsid, string sportevent)
        {
            try
            {
                Session["SCData"] = PreRegNo;
                ViewBag.SCHtmlTag = SCHtmlTags(PreRegNo, sportsid, sportevent);
                return View();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public string SCHtmlTags(string preregnos, string sportsid, string sportevent)
        {
            try
            {
                AdmissionManagementService ads = new AdmissionManagementService();
                System.Text.StringBuilder html = new System.Text.StringBuilder();
                if (preregnos == null) { preregnos = ""; }
                var prereg = preregnos.ToString().Split(',');
                if (sportevent == null) { sportevent = ""; }
                var sptevent = sportevent.ToString().Split(',');
                string[] str = new string[sptevent.ToList().Count];
                int z = 0;
                foreach (var var in sptevent)
                {
                    str[z] = var;
                    z++;
                }

                long[] id = new long[prereg.Length];
                int j = 0;
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                string colName = string.Empty; string[] values = new string[0];
                for (int i = 0; i < prereg.Count(); i++)
                {
                    criteria.Clear();
                    criteria.Add("Preregno", Convert.ToInt32(prereg[i]));
                    if (!string.IsNullOrEmpty(sportevent))
                    { criteria.Add("Event", str); }                                                                 //page - 1, rows, sidx, sord, colName, values, criteria, null

                    Dictionary<long, IList<Sports>> sportsdetails = ads.GetSportsDetailsListWithSearchCriteriaCountArray(0, 10000, string.Empty, string.Empty, colName, values, criteria, null);

                    if (sportsdetails != null && sportsdetails.Count > 0) //&& sportsdetails.First().Value != null && sportsdetails.First().Value.Count > 0)
                    {
                        if (sportsdetails.First().Value.Count > 1)
                        {
                            for (int k = 0; k < sportsdetails.First().Value.Count; k++)
                            {
                                {
                                    html.Append("<div style='font-family: Times New Roman; font-style:italic; font-size:26px; padding-left:400px; padding-top:140px; margin-left:50px; height:20px'>Award for excellence in Sports - " + sportsdetails.First().Value[k].acady + "</div>");
                                }
                                html.Append("<div style='font-family: Times New Roman; font-size:24px; padding-left:280px;padding-top:100px; margin-left:40px; height:15px'> Certificate of honour to <label style='font-weight:bold'>" + sportsdetails.First().Value[k].Name + "</label></div>");
                                html.Append("<div style='font-family: Times New Roman; font-size:24px; padding-left:340px;padding-top:30px; margin-left:60px;  height:16px'>for the excellent performance in " + sportsdetails.First().Value[k].Sport + "</div>"); // + sportsdetails.First().Value[k].Performance + "</div>");

                                html.Append("<div style='font-family: Times New Roman; font-size:24px; padding-left:240px;padding-top:155px; margin-left:60px;  height:16px;'>");
                                html.Append("<div style='font-family: Times New Roman; font-size:24px;float:left; width:63%;'> Event: " + sportsdetails.First().Value[k].Event + "</div>");
                                html.Append("<div style='font-family: Times New Roman; font-size:24px;float:right; width:37%;'>Award : " + sportsdetails.First().Value[k].Award + "</div>   <div style='clear:both'></div></div>");

                                html.Append("<div style='font-family: Times New Roman; font-size:24px; padding-left:240px; margin-left:60px;padding-top:40px; height:22px; '>"); //Date: " + sportsdetails.First().Value[k].Date + " &nbsp");                             
                                html.Append("<div style='font-family: Times New Roman; font-size:24px;float:left; width:63%;'> Date: " + sportsdetails.First().Value[k].Date + "</div>");
                                html.Append("<div style='font-family: Times New Roman; font-size:24px;float:right; width:37%;'>Place : " + sportsdetails.First().Value[k].Place + "</div>   <div style='clear:both'></div></div>");
                                html.Append("<div style='font-family: Times New Roman; font-size:24px; padding-left:260px;padding-top:180px; margin-left:70px;  height:16px'> Coach &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp");
                                html.Append("Sports Co Ordinator &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp Chairman</div>");
                            }
                        }
                        else
                        {
                            html.Append("<div style='font-family: Times New Roman; font-style:italic; font-size:26px; padding-left:400px; padding-top:140px; margin-left:50px; height:20px'>Award for excellence in Sports - 2013/2014</div>");

                            html.Append("<div style='font-family: Times New Roman; font-size:24px; padding-left:280px;padding-top:100px; margin-left:40px; height:15px'> Certificate of honour to <label style='font-weight:bold'>" + sportsdetails.First().Value[0].Name + "</label></div>");
                            html.Append("<div style='font-family: Times New Roman; font-size:24px; padding-left:340px;padding-top:30px; margin-left:60px;  height:16px'>for the excellent performance in " + sportsdetails.First().Value[0].Sport + "</div>");// + sportsdetails.First().Value[0].Performance + "</div>");

                            html.Append("<div style='font-family: Times New Roman; font-size:24px; padding-left:240px;padding-top:155px; margin-left:60px;  height:16px'>");//Event: " + sportsdetails.First().Value[0].Event + " &nbsp");
                            html.Append("<div style='font-family: Times New Roman; font-size:24px;float:left; width:63%'> Event: " + sportsdetails.First().Value[0].Event + "</div>");
                            html.Append("<div style='font-family: Times New Roman; font-size:24px;float:right; width:37%'>Award : " + sportsdetails.First().Value[0].Award + "</div>   <div style='clear:both'></div></div>");
                            html.Append("<div style='font-family: Times New Roman; font-size:24px; padding-left:240px; margin-left:60px;padding-top:40px; height:22px'>");  //Date: " + sportsdetails.First().Value[0].Date + " &nbsp");
                            html.Append("<div style='font-family: Times New Roman; font-size:24px;float:left; width:63%'> Date: " + sportsdetails.First().Value[0].Date + "</div>");
                            html.Append("<div style='font-family: Times New Roman; font-size:24px;float:right; width:37%'>Place : " + sportsdetails.First().Value[0].Place + "</div>   <div style='clear:both'></div></div>");
                            html.Append("<div style='font-family: Times New Roman; font-size:24px; padding-left:260px;padding-top:180px; margin-left:70px;  height:16px'> Coach &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp");
                            html.Append("Sports Co Ordinator &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp Chairman</div>");
                        }
                    }
                    j++;
                }
                return html.ToString();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult PrintIdPickupCard(string Id, string PreRegNo, string idtype)
        {
            try
            {
                Session["idtype"] = idtype;
                Session["IdPickupCardData"] = PreRegNo;
                Session["Id"] = Id;

                ViewBag.IdPickupHtmlTag = IdPickupHtmlTags();
                return View();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public string IdPickupHtmlTags()
        {
            try
            {
                AdmissionManagementService ads = new AdmissionManagementService();

                var prereg = Session["IdPickupCardData"].ToString().Split(',');
                var idtpe = Session["idtype"].ToString().Split(',');
                var idtst = Session["Id"].ToString().Split(',');

                long[] id = new long[prereg.Length];
                int j = 0;
                foreach (string val in prereg)
                {
                    id[j] = Convert.ToInt64(val);
                    j++;
                }

                long[] idnum = new long[idtst.Length];
                int a = 0;
                foreach (string val in idtst)
                {
                    idnum[a] = Convert.ToInt64(val);
                    a++;
                }


                long[] id2 = new long[1000];
                long[] id3 = new long[1000];
                string[] idnam = new string[1000];
                string[] idno = new string[1000];

                int l = 0;
                for (int k = 0; k < Convert.ToInt32(prereg.Count()); k++)
                {
                    Dictionary<string, object> criteriap = new Dictionary<string, object>();
                    criteriap.Add("PreRegNum", id[k]);
                    criteriap.Add("DocumentType", "Pickup Card");
                    criteriap.Add("Id", idnum[k]);

                    Dictionary<long, IList<UploadedFiles>> uploadedfile = ads.GetUploadedFilesListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteriap);

                    if (uploadedfile.First().Value.Count == 0)
                    {
                        id2[l] = 0;
                        id3[l] = 0;
                        idnam[l] = "";
                        idno[l] = "";
                        l++;
                    }
                    else
                    {
                        for (int y = 0; y < uploadedfile.First().Value.Count; y++)
                        {
                            id2[l] = uploadedfile.First().Value[y].PreRegNum;
                            id3[l] = uploadedfile.First().Value[y].Id;
                            idnam[l] = uploadedfile.First().Value[y].Name;
                            idno[l] = uploadedfile.First().Value[y].Phone;
                            l++;
                        }
                    }
                }

                System.Text.StringBuilder html = new System.Text.StringBuilder();
                for (int i = 0; i < Convert.ToInt32(prereg.Count()); i = i + 2)
                {

                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("PreRegNum", id[i]);
                    Dictionary<long, IList<StudentTemplateView>> StudentIdcnt1 = ads.GetStudentTemplateViewListWithEQsearchCriteria(0, 10000, string.Empty, string.Empty, criteria);

                    if (StudentIdcnt1.First().Value[0].BGRP == "Not Given")
                    {
                        StudentIdcnt1.First().Value[0].BGRP = "";
                    }

                    Dictionary<string, object> criteria3 = new Dictionary<string, object>();
                    criteria3.Add("PreRegNum", id[i]);
                    criteria3.Add("FamilyDetailType", "Father");
                    Dictionary<long, IList<FamilyDetails>> StudentIdcnt3 = ads.GetFamilyDetailsListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria3);

                    var mobi1 = "";
                    var mobi2 = "";
                    if (StudentIdcnt3.First().Key == 0)
                    {
                        mobi1 = "";
                    }
                    else
                    {
                        mobi1 = StudentIdcnt3.First().Value[0].Mobile;
                        if (mobi1 != "")
                        {
                            mobi1 = mobi1.Split(',')[0].ToString();
                        }
                    }

                    Dictionary<long, IList<StudentTemplateView>> StudentIdcnt2 = null;
                    Dictionary<long, IList<FamilyDetails>> StudentIdcnt4 = null;

                    if (i + 1 < Convert.ToInt32(prereg.Count()))
                    {
                        Dictionary<string, object> criteria2 = new Dictionary<string, object>();
                        criteria2.Add("PreRegNum", id[i + 1]);
                        StudentIdcnt2 = ads.GetStudentTemplateViewListWithEQsearchCriteria(0, 10000, string.Empty, string.Empty, criteria2);

                        if (StudentIdcnt2.First().Value[0].BGRP == "Not Given")
                        {
                            StudentIdcnt2.First().Value[0].BGRP = "";
                        }

                        Dictionary<string, object> criteria4 = new Dictionary<string, object>();
                        criteria4.Add("PreRegNum", id[i + 1]);
                        criteria4.Add("FamilyDetailType", "Father");
                        StudentIdcnt4 = ads.GetFamilyDetailsListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria4);

                        if (StudentIdcnt4.First().Key == 0)
                        {
                            mobi2 = "";
                        }
                        else
                        {
                            mobi2 = StudentIdcnt4.First().Value[0].Mobile;
                            if (mobi2 != "")
                            {
                                mobi2 = mobi2.Split(',')[0].ToString();
                            }
                        }
                    }

                    if (idtpe[i] == "IdCard")
                    {
                        html.Append("<tr>");
                        html.Append("<td style='padding-bottom:20px; padding-right:25px'>");
                        html.Append("<div style='width: 350px'>");

                        html.Append("<span style='font-size: 10pt; float:left'><img src='../../Images/left-top-org.png' height='70';width='80'/><br/>    </span>");

                        html.Append("<span style='font-size: 10pt; float:right; padding-right:10px; padding-top:10px'><img src='../../Images/Tips Logo.JPG' height='45';width='30'/><br/>    </span>");
                        html.Append("<br />");
                        html.Append("<span style='font-size: 8pt; padding-left:25px'>  <img src='" + Url.Action("uploaddisplay1", "Admission", new { id = id[i] }) + "' id='image1' name='logoFile' width= '80' height='80' style='padding-top:15px' /> </span><br />");

                        html.Append("<span style='font-family: Arial; font-size:12px'><div style=' padding-top:2px; padding-left:130px; height: 4px'><strong> " + StudentIdcnt1.First().Value[0].Name + "  </strong></div><br />");
                        html.Append("<div style=' padding-left:130px; height: 2px'><strong>Grade &nbsp" + StudentIdcnt1.First().Value[0].Grade + "</strong></div><br />");
                        html.Append("<div style=' padding-left:130px; height: 2px'><strong>" + StudentIdcnt1.First().Value[0].NewId + "</strong></div><br />");
                        html.Append("<div style='padding-left:8px; height: 18px'><strong>Blood Group : &nbsp" + StudentIdcnt1.First().Value[0].BGRP + "&nbsp&nbsp  Emergency No: &nbsp " + mobi1 + "</strong></div></span>");
                        html.Append("<span style='font-size: 10pt; float:right'><img src='../../Images/new_bottom.jpg'   style='width:350px; height:26px;'/> </span>");
                        html.Append("<div/>");
                        html.Append("<td/>");
                    }
                    else
                    {

                        html.Append("<td style='padding-bottom:20px; padding-right:25px'>");
                        html.Append("<div style='width: 350px'>");

                        html.Append("<span style='font-size: 10pt; float:left'><img src='../../Images/left-top-org.png' height='70';width='180'/><br/>    </span>");

                        html.Append("<span style='font-size: 10pt; float:right; padding-right:10px; padding-top:10px'><img src='../../Images/Tips Logo.JPG' height='45';width='30'/>    </span>");
                        html.Append("<span style='font-size: 8pt; padding-left:25px'>  <img src='" + Url.Action("uploaddisplay1", "Admission", new { id = id2[i] }) + "' id='image1' name='logoFile' width= '80' height='80'  style='padding-top:35px'/></span>");
                        html.Append("<br />");
                        html.Append("<span style='font-size: 9pt; float:left; padding-left:30px; padding-bottom:6px'>  <img src='" + Url.Action("uploaddisplay2", "Admission", new { id = id3[i] }) + "' id='image1' name='logoFile' width= '80' height='80' style='padding-top:-1px'/> </span>");

                        html.Append("<div style='font-family: Arial; font-size:12px; padding-left:80px; padding-top:3px; margin-left:50px; height:20px'><strong> " + StudentIdcnt1.First().Value[0].Name + "</strong></div>");
                        html.Append("<div style='font-family: Arial; font-size:12px; padding-left:80px; margin-left:50px; height:15px'><strong> " + StudentIdcnt1.First().Value[0].NewId + "</strong></div>");

                        html.Append("<div style='font-family: Arial; font-size:12px; padding-left:80px; margin-left:50px; padding-top:25px; height:14px'><strong> " + idnam[i] + "</strong></div>");

                        html.Append("<span style='float:right'><img src='../../Images/new_bottom.jpg'   style='width:350px; height:26px;'/> </span>");

                        html.Append("<div/>");
                        html.Append("<td/>");
                    }

                    if (i + 1 < Convert.ToInt32(prereg.Count()))
                    {
                        if (idtpe[i + 1] == "IdCard")
                        {

                            html.Append("<td style='padding-bottom:20px'>");
                            html.Append("<div style='width: 350px'>");

                            html.Append("<span style='font-size: 10pt; float:left'><img src='../../Images/left-top-org.png' height='70';width='80'/><br/>    </span>");

                            html.Append("<span style='font-size: 10pt; float:right; padding-right:10px; padding-top:10px'><img src='../../Images/Tips Logo.JPG' height='45';width='30'/><br/>    </span>");
                            html.Append("<br />");
                            html.Append("<span style='font-size: 8pt; padding-left:25px'>  <img src='" + Url.Action("uploaddisplay1", "Admission", new { id = id[i + 1] }) + "' id='image1' name='logoFile' width= '80' height='80' style='padding-top:15px'/> </span><br />");
                            html.Append("<span style='font-family: Arial; font-size:12px'><div style=' padding-top:3px; padding-left:130px; height: 4px'><strong> " + StudentIdcnt2.First().Value[0].Name + "<br />  </strong></div><br/>");
                            html.Append("<div style='padding-left:130px; height: 2px'><strong>Grade &nbsp" + StudentIdcnt2.First().Value[0].Grade + "</strong></div><br />");
                            html.Append("<div style='padding-left:130px; height: 2px'><strong>" + StudentIdcnt2.First().Value[0].NewId + "</strong></div><br />");
                            html.Append("<div style='padding-left:8px; height: 18px'><strong>Blood Group : &nbsp" + StudentIdcnt2.First().Value[0].BGRP + "&nbsp&nbsp  Emergency No: &nbsp " + mobi2 + "</strong></div></span>");
                            //html.Append("<br/>");
                            html.Append("<span style='font-size: 10pt; float:right'><img src='../../Images/new_bottom.jpg'   style='width:350px; height:26px;'/> </span>");

                            html.Append("<div/>");
                            html.Append("<td/>");
                        }
                        else
                        {
                            html.Append("<td style='padding-bottom:20px; padding-right:25px'>");
                            html.Append("<div style='width: 350px'>");

                            html.Append("<span style='font-size: 10pt; float:left'><img src='../../Images/left-top-org.png' height='70';width='180'/><br/>    </span>");

                            html.Append("<span style='font-size: 10pt; float:right; padding-right:10px; padding-top:10px'><img src='../../Images/Tips Logo.JPG' height='45';width='30'/>    </span>");
                            html.Append("<span style='font-size: 8pt; padding-left:25px'>  <img src='" + Url.Action("uploaddisplay1", "Admission", new { id = id2[i + 1] }) + "' id='image1' name='logoFile' width= '80' height='80'  style='padding-top:35px'/></span>");
                            html.Append("<br />");
                            html.Append("<span style='font-size: 9pt; float:left; padding-left:30px; padding-bottom:6px'>  <img src='" + Url.Action("uploaddisplay2", "Admission", new { id = id3[i + 1] }) + "' id='image1' name='logoFile' width= '80' height='80' style='padding-top:-1px'/> </span>");

                            html.Append("<div style='font-family: Arial; font-size:12px; padding-left:80px; padding-top:3px; margin-left:50px; height:20px'><strong> " + StudentIdcnt2.First().Value[0].Name + "</strong></div>");
                            html.Append("<div style='font-family: Arial; font-size:12px; padding-left:80px; margin-left:50px; height:15px'><strong> " + StudentIdcnt2.First().Value[0].NewId + "</strong></div>");

                            html.Append("<div style='font-family: Arial; font-size:12px; padding-left:80px; margin-left:50px; padding-top:25px; height:14px'><strong> " + idnam[i + 1] + "</strong></div>");

                            html.Append("<span style='float:right'><img src='../../Images/new_bottom.jpg'   style='width:350px; height:26px;'/> </span>");

                            html.Append("<div/>");
                            html.Append("<td/>");
                        }
                    }
                    else
                    {
                        html.Append("<td style='padding-bottom:20px'>");
                        html.Append("<div style='width: 350px'>");
                        html.Append("<div/>");
                        html.Append("<td/>");
                    }

                    html.Append("</tr>");
                }
                return html.ToString();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult FillGrade(string Campus)
        {
            try
            {
                MasterDataService mds = new MasterDataService();
                IList<Grade> GradeList = mds.GetGradeByCampus(Campus);
                var IssueType = new
                {
                    rows = (
                         from items in GradeList
                         select new
                         {
                             Text = items.Grade1,
                             Value = items.Grade1
                         }).ToArray(),
                };

                return Json(IssueType, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult BulkRegister(string PreRegNo)
        {
            Session["emailpreregno"] = PreRegNo;
            MastersService ms = new MastersService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            Dictionary<long, IList<FeeStructureYearMaster>> FeeStructureYearMaster = ms.GetFeeStructureYearMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            Dictionary<long, IList<SectionMaster>> SectionMaster = ms.GetSectionMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            ViewBag.feestructyrddl = FeeStructureYearMaster.First().Value;
            ViewBag.sectionddl = SectionMaster.First().Value;
            return View();
        }

        [HttpPost]
        public ActionResult BulkRegister(StudentTemplate st)
        {
            try
            {
                string RecipientInfo = "", Subject = "", Body = "", MailBody = ""; bool retValue;
                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                string[] dob = new string[3];
                EmailHelper em = new EmailHelper();
                IList<CampusEmailId> campusemaildet = null;
                var prereg = Session["emailpreregno"].ToString().Split(',');
                long[] id = new long[prereg.Length];
                string[] grad = new string[prereg.Length];
                string[] acdyr = new string[prereg.Length];
                string[] cmps = new string[prereg.Length];
                long[] RowId = new long[prereg.Length];
                MailBody = GetBodyofMail();
                AdmissionManagementService ads = new AdmissionManagementService();
                for (int g = 0; g < prereg.Length; g++)
                {
                    Dictionary<string, object> criteria1 = new Dictionary<string, object>();
                    criteria1.Add("PreRegNum", Convert.ToInt64(prereg[g]));
                    Dictionary<long, IList<StudentTemplateView>> StudentIdcnt = ads.GetStudentTemplateViewListWithEQsearchCriteria(0, 0, string.Empty, string.Empty, criteria1);
                    grad[g] = StudentIdcnt.First().Value[0].Grade;
                    acdyr[g] = StudentIdcnt.First().Value[0].AcademicYear;
                    cmps[g] = StudentIdcnt.First().Value[0].Campus;
                    RowId[g] = StudentIdcnt.First().Value[0].Id;
                }
                st.FamilyDetailsList = null;
                st.PastSchoolDetailsList = null;
                st.ApproveAssignList = null;
                st.PaymentDetailsList = null;
                st.PreRegNum = Convert.ToInt64(Session["preregno"]);                    //Pre-Registration number
                //st.Id = Convert.ToInt64(Session["preregid"]); // preregid;
                ViewBag.Date = Session["date"]; // date;
                ViewBag.time = Session["time"];  // time; // st.CreatedTime;
                st.CreatedTime = Session["time"] == null ? "" : Session["time"].ToString(); // time;
                st.CreatedDate = Session["date"] == null ? "" : Session["date"].ToString();  // date;

                if (st.AdmissionStatus == "Registered")
                {
                    for (int j = 0; j < prereg.Length; j++)
                    {

                        //   var y = x.Substring(2);
                        //  st.NewId = "I" + st.FeeStructYear + "-" + x + "-" + z;
                        st.AdmissionStatus = "Registered";    //Admission Registered By Admin
                        //   st.Status = "1";          // key for sentfor approval, on hold,callforinterview
                        Dictionary<string, object> criteria2 = new Dictionary<string, object>();
                        criteria2.Add("Id", Convert.ToInt64(RowId[j]));
                        Dictionary<long, IList<StudentTemplate>> StudentTemp = ads.GetStudentDetailsListWithEQsearchCriteria(0, 0, string.Empty, string.Empty, criteria2);
                        //   Dictionary<long, IList<StudentTemplateView>> StudentTemp = ads.GetStudentTemplateViewListWithEQsearchCriteria(0, 0, string.Empty, string.Empty, criteria2);
                        st.AcademicYear = StudentTemp.First().Value[0].AcademicYear;
                        st.CreatedDate = StudentTemp.First().Value[0].CreatedDate;
                        st.AnnualIncome = StudentTemp.First().Value[0].AnnualIncome;
                        st.ApplicationNo = StudentTemp.First().Value[0].ApplicationNo;
                        st.BGRP = StudentTemp.First().Value[0].BGRP;
                        st.BoardingType = StudentTemp.First().Value[0].BoardingType;
                        st.Campus = StudentTemp.First().Value[0].Campus;
                        st.DOB = StudentTemp.First().Value[0].DOB;
                        st.EducationGoals = StudentTemp.First().Value[0].EducationGoals;
                        st.EducationGoalYesorNo = StudentTemp.First().Value[0].EducationGoalYesorNo;
                        st.EmailId = StudentTemp.First().Value[0].EmailId;
                        //    st.FeeStructYear = StudentTemp.First().Value[0].FeeStructYear;
                        st.Food = StudentTemp.First().Value[0].Food;
                        st.Gender = StudentTemp.First().Value[0].Gender;
                        st.Grade = StudentTemp.First().Value[0].Grade;
                        st.Id = StudentTemp.First().Value[0].Id;
                        st.IsHosteller = StudentTemp.First().Value[0].IsHosteller;
                        st.LanguagesKnown = StudentTemp.First().Value[0].LanguagesKnown;
                        st.Name = StudentTemp.First().Value[0].Name;
                        //  st.NewId = StudentTemp.First().Value[0].NewId;
                        st.PreRegNum = StudentTemp.First().Value[0].PreRegNum;
                        st.NewId = StudentIdNumberLogic(st.Campus, st.FeeStructYear, st.Grade, st.AcademicYear);
                        IList<AddressDetails> adList = new List<AddressDetails>();
                        IEnumerable<AddressDetails> ad1 = from cust in StudentTemp.First().Value[0].AddressDetailsList
                                                          where cust.AddressType == "Primary Address"
                                                          select cust;
                        adList.Add(ad1.FirstOrDefault());
                        IEnumerable<AddressDetails> ad2 = from cust in StudentTemp.First().Value[0].AddressDetailsList
                                                          where cust.AddressType == "Secondary Address"
                                                          select cust;
                        adList.Add(ad2.FirstOrDefault());
                        st.AddressDetailsList = adList;

                        ads.CreateOrUpdateStudentDetails(st);
                        ///////
                        // To Generate Parent Id and send mail to parent
                        dob = st.DOB.ToString().Split('/');
                        string sex = "";
                        if (st.Gender == "Male") { sex = "his"; }
                        else if (st.Gender == "Female") { sex = "her"; }
                        campusemaildet = GetEmailIdByCampus(cmps[j], ConfigurationManager.AppSettings["CampusEmailType"].ToString());
                        TIPS.Entities.User user = new TIPS.Entities.User();
                        user.UserId = "P" + st.NewId;
                        user.Password = dob[0] + dob[1] + dob[2];
                        user.Campus = st.Campus;
                        user.UserType = "Parent";
                        user.CreatedDate = DateTime.Now;
                        user.ModifiedDate = DateTime.Now;
                        TIPS.Service.UserService us = new UserService();
                        PassworAuth PA = new PassworAuth();
                        //encode and save the password
                        user.Password = PA.base64Encode(user.Password);
                        user.IsActive = true;
                        TIPS.Entities.User userexists = us.GetUserByUserId(user.UserId);
                        if (userexists == null)    // to check if user already exists
                        {
                            //Send Mail to parent for sending ParentPortal Id and Password
                            RecipientInfo = "Dear Parent,";
                            Body = "We are happy to add you in our TIPS family and your admission is approved. “" + st.Name + "” is admitted in Grade “" + st.Grade + "” section “" + st.Section + "” of our “" + st.Campus + "” campus and " + sex + " Id number will be “" + st.NewId + "”, Once the school re-open the confirmed Section will be informed.<br/><br/>";
                            Body = Body + "Your parent portal id is “" + "P" + st.NewId + "” and password will be “" + dob[0] + dob[1] + dob[2] + "” i.e. the DOB of the student as given in the records. You can access the same through http://portal.tipsglobal.org and <a href='https://play.google.com/store/apps/details?id=com.tips.parent'>Click here</a> to download our TIPS android mobile​ ​application.";
                            Body = Body + "<br/><br/>For any queries, mail us at “" + campusemaildet.First().EmailId.ToString() + "”. ";
                            Subject = "Welcome to TIPS - Student Registration Successfull"; // st.Subject;   
                            if (ConfigurationManager.AppSettings["SendEmail1"].ToString() == "true")
                            {
                                criteria2.Clear();
                                criteria2.Add("Campus", st.Campus);
                                criteria2.Add("DocumentType", "Parent Portal Circular");
                                Dictionary<long, IList<CampusDocumentMaster>> PBCircularDoc = ads.GetCampusDocumentListwithCriteria(0, 10, string.Empty, string.Empty, criteria2);
                                Attachment CircularAttach = null;
                                if (PBCircularDoc != null && PBCircularDoc.Count > 0 && PBCircularDoc.FirstOrDefault().Key > 0 && PBCircularDoc.FirstOrDefault().Value.Count > 0)
                                {
                                    MemoryStream memStream = new MemoryStream(PBCircularDoc.FirstOrDefault().Value[0].ActualDocument);
                                    CircularAttach = new Attachment(memStream, PBCircularDoc.FirstOrDefault().Value[0].DocumentName);  //Data posted from form
                                    Body = Body + "<br/><br/>Please find the attached Parent Portal Circular.";
                                }
                                retValue = em.SendStudentRegistrationMail(st, st.EmailId, st.Campus, Subject, Body, MailBody, RecipientInfo, "Parent", CircularAttach);
                            }
                            us.CreateOrUpdateUser(user);
                        }
                        // Create Assess360 for student while registering. Its for grade (VI to XII)
                        if (st.Grade == "VI" || st.Grade == "VII" || st.Grade == "VIII" || st.Grade == "IX" || st.Grade == "X" || st.Grade == "XI" || st.Grade == "XII")
                        {
                            CreateAssess360(st);
                        }
                        Session["registered"] = "yes";
                        Session["regid"] = st.NewId;
                    }
                }
                return RedirectToAction("AdmissionManagement");
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult Promotion()
        {
            Session["promotion"] = "";
            Session["promotionId"] = "";

            MastersService ms = new MastersService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            Dictionary<long, IList<CampusMaster>> CampusMaster = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            ViewBag.campusddl = CampusMaster.First().Value;

            Dictionary<long, IList<SectionMaster>> SectionMaster = ms.GetSectionMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            ViewBag.sectionddl = SectionMaster.First().Value;
            Dictionary<string, object> criteria1 = new Dictionary<string, object>();
            var g1 = "";
            int[] values = new int[2];
            if (Session["Promotioncamp"].ToString() == "IB KG")
            {
                criteria1.Add("Code", g1);
                values = new int[0];
            }
            else if (Session["Promotioncamp"].ToString() == "IB MAIN")
            {
                g1 = "3";
                criteria1.Add("Code1", g1);
                values = new int[0];
            }
            else if (Session["Promotioncamp"].ToString() == "ERNAKULAM")
            {
                g1 = "";
                criteria1.Add("Code", g1);
                values = new int[0];
            }
            else if (Session["Promotioncamp"].ToString() == "TIPS SARAN")   // Get using IN condition
            {
                criteria1.Clear();
                values[0] = 11;                     // get grade with column GRAD
                values[1] = 12;
            }
            else
            {
                g1 = "3";
                criteria1.Add("Code1", g1);
                values = new int[0];
            }
            Dictionary<long, IList<GradeMaster>> GradeMaster = ms.GetGradeMasterListWithPagingAndCriteriaWithIn(0, 50, "", "", "grad", values, criteria1, null);

            ViewBag.gradeddl = GradeMaster.First().Value;

            StudentTemplate st = new StudentTemplate();

            switch (Session["promgrd"].ToString())
            {
                case "PreKG":
                    st.Grade = "LKG";
                    if (Session["Promotioncamp"].ToString() == "IB MAIN") { st.Campus = "IB KG"; }
                    else { st.Campus = "CHENNAI CITY"; }
                    break;
                case "PREKG":
                    st.Grade = "LKG";
                    if (Session["Promotioncamp"].ToString() == "IB MAIN") { st.Campus = "IB KG"; }
                    else { st.Campus = "CHENNAI CITY"; }
                    break;
                case "LKG":
                    st.Grade = "UKG";
                    if (Session["Promotioncamp"].ToString() == "IB MAIN") { st.Campus = "IB KG"; }
                    else { st.Campus = "CHENNAI CITY"; }
                    break;
                case "UKG":
                    st.Grade = "I";
                    if (Session["Promotioncamp"].ToString() == "IB MAIN") { st.Campus = "IB MAIN"; }
                    else { st.Campus = "CHENNAI CITY"; }
                    break;
                case "I":
                    st.Grade = "II";
                    if (Session["Promotioncamp"].ToString() == "IB MAIN") { st.Campus = "IB MAIN"; }
                    else { st.Campus = "CHENNAI CITY"; }
                    break;
                case "II":
                    st.Grade = "III";
                    if (Session["Promotioncamp"].ToString() == "IB MAIN") { st.Campus = "IB MAIN"; }
                    else { st.Campus = "CHENNAI MAIN"; }
                    break;
                case "III":
                    st.Grade = "IV";
                    if (Session["Promotioncamp"].ToString() == "IB MAIN") { st.Campus = "IB MAIN"; }
                    else { st.Campus = "CHENNAI MAIN"; }
                    break;
                case "IV":
                    st.Grade = "V";
                    if (Session["Promotioncamp"].ToString() == "IB MAIN") { st.Campus = "IB MAIN"; }
                    else { st.Campus = "CHENNAI MAIN"; }
                    break;
                case "V":
                    st.Grade = "VI";
                    if (Session["Promotioncamp"].ToString() == "IB MAIN") { st.Campus = "IB MAIN"; }
                    else { st.Campus = "CHENNAI MAIN"; }
                    break;
                case "VI":
                    st.Grade = "VII";
                    if (Session["Promotioncamp"].ToString() == "IB MAIN") { st.Campus = "IB MAIN"; }
                    else { st.Campus = "CHENNAI MAIN"; }
                    break;
                case "VII":
                    st.Grade = "VIII";
                    if (Session["Promotioncamp"].ToString() == "IB MAIN") { st.Campus = "IB MAIN"; }
                    else { st.Campus = "CHENNAI MAIN"; }
                    break;
                case "VIII":
                    st.Grade = "IX";
                    if (Session["Promotioncamp"].ToString() == "IB MAIN") { st.Campus = "IB MAIN"; }
                    else { st.Campus = "CHENNAI MAIN"; }
                    break;
                case "IX":
                    st.Grade = "X";
                    if (Session["Promotioncamp"].ToString() == "IB MAIN") { st.Campus = "IB MAIN"; }
                    else { st.Campus = "CHENNAI MAIN"; }
                    st.Campus = "IB MAIN";
                    break;
                case "X":
                    st.Grade = "XI";
                    if (Session["Promotioncamp"].ToString() == "IB MAIN") { st.Campus = "IB MAIN"; }
                    else { st.Campus = "CHENNAI MAIN"; }
                    st.Campus = "IB MAIN";
                    break;
                case "XI":
                    st.Grade = "XII";
                    if (Session["Promotioncamp"].ToString() == "IB MAIN") { st.Campus = "IB MAIN"; }
                    else { st.Campus = "CHENNAI MAIN"; }
                    st.Campus = "IB MAIN";
                    break;
            }
            return View(st);
        }

        public bool promotioncampus(string PreRegNo, string campus, string grade, string check)
        {
            var cmps = campus.Split(',');
            if (check == "yes")
            {
                var grd = grade.Split(',');                        // To check whether same grade students are selected or not. 
                if ((grd.Distinct().Count() > 1) || (cmps.Distinct().Count() > 1))
                {
                    return false;
                }
                else
                {
                    if (string.Equals(Session["UserId"].ToString(), "GRL-ADMS", StringComparison.CurrentCultureIgnoreCase) && !string.Equals(cmps[0].ToString(), "IB MAIN", StringComparison.CurrentCultureIgnoreCase) && !string.Equals(cmps[0].ToString(), "IB KG", StringComparison.CurrentCultureIgnoreCase))
                    {
                        return false;
                    }
                    else if (string.Equals(Session["UserId"].ToString(), "CHE-GRL", StringComparison.CurrentCultureIgnoreCase) && !string.Equals(cmps[0].ToString(), "CHENNAI MAIN", StringComparison.CurrentCultureIgnoreCase) && !string.Equals(cmps[0].ToString(), "CHENNAI CITY", StringComparison.CurrentCultureIgnoreCase))
                    {
                        return false;
                    }
                    else
                    {
                        Session["promgrd"] = grd[0];     // Current grade of student to be promoted
                        return true;
                    }
                }
            }
            else
            {
                Session["promotionno"] = PreRegNo;

                //if (Session["UserId"].ToString() == "CHE-GRL")
                //{
                //    Session["Promotioncamp"] = "CHENNAI";
                //}
                //if (Session["UserId"].ToString() == "TIR-GRL")
                //{
                //    Session["Promotioncamp"] = "TIRUPUR";
                //}
                //if (Session["UserId"].ToString() == "ERN-GRL")
                //{
                //    Session["Promotioncamp"] = "ERNAKULAM";
                //}
                //if (Session["UserId"].ToString() == "KAR-GRL")
                //{
                //    Session["Promotioncamp"] = "KARUR";
                //}
                //if (Session["UserId"].ToString() == "GRL-ADMS")
                //{
                //    Session["Promotioncamp"] = "IB MAIN";
                //}
                //if (Session["UserId"].ToString() == "Uma")
                //{
                //    Session["Promotioncamp"] = "TIPS ERODE";
                //}

                // added by anbu ..replaced the above commented code
                //User user = (User)Session["objUser"];
                //if (user != null)
                //    Session["Promotioncamp"] = user.Campus;
                //else
                //    Session["Promotioncamp"] = string.Empty;
                Session["Promotioncamp"] = cmps[0];
                return false;
            }
        }

        [HttpPost]
        public ActionResult Promotion(StudentTemplate st)
        {
            var prereg = Session["promotionno"].ToString().Split(',');
            long[] prereno = new long[prereg.Length];

            TransferDetails td = new TransferDetails();

            for (int g = 0; g < prereg.Length; g++)
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("PreRegNum", Convert.ToInt64(prereg[g]));

                AdmissionManagementService ams = new AdmissionManagementService();
                Dictionary<long, IList<StudentTemplate>> studenttemplate = ams.GetStudentDetailsListWithEQsearchCriteria(0, 50, string.Empty, string.Empty, criteria);

                if (st.Grade == studenttemplate.First().Value[0].Grade)
                {
                    Session["promotion"] = "no";
                    return RedirectToAction("StudentManagement", new { pagename = "promotion" });
                }

                td.Id = 0;
                td.BeforeGrade = studenttemplate.First().Value[0].Grade;
                td.BeforeSection = studenttemplate.First().Value[0].Section;
                td.BeforeId = studenttemplate.First().Value[0].NewId;

                td.ApplicationNo = studenttemplate.First().Value[0].ApplicationNo;
                td.FeeStructYear = studenttemplate.First().Value[0].FeeStructYear;
                td.Gender = studenttemplate.First().Value[0].Gender;
                td.TransferedDate = DateTime.Now.ToString();
                td.Name = studenttemplate.First().Value[0].Name;
                td.Type = "Promotion";

                td.PreRegNum = Convert.ToInt64(prereg[g]);
                if (Session["UserId"].ToString() == "GRL-ADMS")
                {
                    td.AfterCampus = st.Campus;
                }
                else
                {
                    td.AfterCampus = Session["Promotioncamp"].ToString();
                }
                td.AfterGrade = st.Grade;
                td.AfterSection = st.Section;
                td.AfterId = studenttemplate.First().Value[0].NewId;

                if (Session["UserId"].ToString() == "GRL-ADMS")
                {
                    studenttemplate.First().Value[0].Campus = st.Campus;
                }
                else if (Session["UserId"].ToString() == "CHE-GRL")
                {
                    studenttemplate.First().Value[0].Campus = st.Campus;
                }
                else
                {
                    studenttemplate.First().Value[0].Campus = Session["Promotioncamp"].ToString();
                }

                var acdyr = studenttemplate.First().Value[0].AcademicYear.ToString().Split('-');   // To increment academic year while promoting

                int acd1 = Convert.ToInt32(acdyr[0]);
                acd1 = acd1 + 1;

                int acd2 = Convert.ToInt32(acdyr[1]);
                if (acd2 == DateTime.Now.Year)                  // if current academicyear of student is equal to current year
                {
                    acd2 = acd2 + 1;
                    td.AcademicYear = acd1.ToString() + "-" + acd2.ToString();
                    studenttemplate.First().Value[0].AcademicYear = td.AcademicYear;
                    studenttemplate.First().Value[0].Grade = st.Grade;
                    studenttemplate.First().Value[0].Section = st.Section;
                    st = studenttemplate.First().Value[0];

                    ams.CreateOrUpdateTransferDetails(td);
                    ams.CreateOrUpdateStudentDetails(st);
                    Session["promotionId"] = st.Grade + " " + st.Section;
                }
                else
                {
                    if (Session["notpromotedpreregno"].ToString() == "")
                    {
                        Session["notpromotedpreregno"] = prereg[g];
                    }
                    else
                    {
                        Session["notpromotedpreregno"] = Session["notpromotedpreregno"].ToString() + "," + prereg[g];
                    }
                }
            }
            Session["promotion"] = "yes";
            return RedirectToAction("StudentManagement", new { pagename = "promotion", resetsession = "no" });
        }

        public bool Discontinuestats(string PreRegNo, string campus, string type)
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("PreRegNum", Convert.ToInt64(PreRegNo));
            if (type == "bonafide")
            {
                criteria.Add("DocumentType", "" + campus + " Bonafide Certificate");
            }
            else
            {
                criteria.Add("DocumentType", "" + campus + " Transfer Certificate");
            }
            criteria.Add("DocumentFor", "Student");  // to check if transfer certificate has been issued earlier for this student

            AdmissionManagementService ams = new AdmissionManagementService();
            Dictionary<long, IList<UploadedFilesView>> UploadedFilesView = ams.GetUploadedFilesViewListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);

            if (UploadedFilesView != null && UploadedFilesView.First().Value != null && UploadedFilesView.First().Value.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public ActionResult Discontinue(string PreRegNo)
        {
            MastersService ms = new MastersService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            Dictionary<long, IList<GradeMaster>> GradeMaster = ms.GetGradeMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            ViewBag.gradeddl1 = GradeMaster.First().Value;
            return View();
        }

        [HttpPost]
        public ActionResult Discontinue(TransferDetails td)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("PreRegNum", Convert.ToInt64(td.PreRegNum));

                AdmissionManagementService ams = new AdmissionManagementService();
                Dictionary<long, IList<StudentTemplate>> studenttemplate = ams.GetStudentDetailsListWithEQsearchCriteria(0, 50, string.Empty, string.Empty, criteria);
                if (studenttemplate.First().Value[0].Grade == "XI" || studenttemplate.First().Value[0].Grade == "XII")
                {
                    td.AfterGrade = "IB DIPlOMA";
                }
                else
                {
                    td.AfterGrade = studenttemplate.First().Value[0].Grade;
                }
                td.AcademicYear = studenttemplate.First().Value[0].AcademicYear;
                td.AdmissionDate = studenttemplate.First().Value[0].CreatedDate;
                td.ApplicationNo = studenttemplate.First().Value[0].ApplicationNo;
                td.FeeStructYear = studenttemplate.First().Value[0].FeeStructYear;
                td.Gender = studenttemplate.First().Value[0].Gender;
                td.Name = studenttemplate.First().Value[0].Name;
                td.Type = "Discontinue";
                td.AfterId = studenttemplate.First().Value[0].NewId;
                td.AfterCampus = studenttemplate.First().Value[0].Campus;
                td.AfterSection = studenttemplate.First().Value[0].Section;
                //td.AfterGrade = studenttemplate.First().Value[0].Grade;
                ams.CreateOrUpdateTransferDetails(td);

                UploadedFilesView fu1 = new UploadedFilesView();
                fu1.DocumentName = "Transfer Certificate";
                fu1.DocumentType = "" + studenttemplate.First().Value[0].Campus + " Transfer Certificate";
                fu1.UploadedDate = DateTime.Now.Date.ToString("dd/MM/yyyy");
                fu1.DocumentFor = "Student";
                fu1.PreRegNum = td.PreRegNum;              // Pre Registration number
                fu1.Type = "TransferPDF";
                Session["transferpdf"] = "yes";
                Session["transferpreregno"] = td.PreRegNum;
                ams.CreateOrUpdateUploadedFilesView(fu1);
                DeactivateStudent(studenttemplate.First().Value[0].Id);
                //
                return RedirectToAction("StudentManagement", new { pagename = "transfer" });

            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult Sports(string preregno)
        {
            MastersService ms = new MastersService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            Dictionary<long, IList<AcademicyrMaster>> AcademicyrMaster = ms.GetAcademicyrMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            ViewBag.Acadyddl = AcademicyrMaster.First().Value;
            return View();
        }

        [HttpPost]
        public ActionResult Sports(Sports sp)
        {
            try
            {
                string stdntname = "";
                AdmissionManagementService ams = new AdmissionManagementService();
                Dictionary<long, IList<StudentTemplate>> studenttemplate;

                var prereg = sp.Preregno1.ToString().Split(',');
                long[] preregno = new long[prereg.Length];

                for (int x = 0; x < prereg.Length; x++)
                {

                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("PreRegNum", Convert.ToInt64(prereg[x]));
                    studenttemplate = ams.GetStudentDetailsListWithEQsearchCriteria(0, 50, string.Empty, string.Empty, criteria);
                    stdntname = studenttemplate.First().Value[0].Name;
                    var doc1 = new iTextSharp.text.Document();
                    doc1.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                    string SCpath = ConfigurationManager.AppSettings["SC"].ToString() + prereg[x] + ".pdf";
                    PdfWriter.GetInstance(doc1, new FileStream(SCpath, FileMode.Create));
                    doc1.Open();

                    iTextSharp.text.Image TipsSideImage;
                    TipsSideImage = iTextSharp.text.Image.GetInstance(ConfigurationManager.AppSettings["SCimages"].ToString() + "tips_sidelogo.gif"); //"D:/SC Images/tips_sidelogo.gif");
                    TipsSideImage.Border = 0;
                    TipsSideImage.ScaleAbsolute(90, 600);

                    iTextSharp.text.Image logoImage;
                    string ImagePath = ConfigurationManager.AppSettings["SCimages"].ToString() + "Tips Logo.PNG"; //  ConfigurationManager.AppSettings["ImageFilePath"];
                    logoImage = iTextSharp.text.Image.GetInstance(ImagePath);
                    logoImage.Border = 0;
                    logoImage.ScaleAbsolute(120, 120);

                    iTextSharp.text.Image footballImage;
                    string footballImagePath = ConfigurationManager.AppSettings["SCimages"].ToString() + "1.jpg"; //  ConfigurationManager.AppSettings["ImageFilePath"];
                    footballImage = iTextSharp.text.Image.GetInstance(footballImagePath);
                    footballImage.Border = 0;
                    footballImage.ScaleAbsolute(200, 200);

                    iTextSharp.text.Image starImage;
                    string starImagePath = ConfigurationManager.AppSettings["SCimages"].ToString() + "star.jpg"; //  ConfigurationManager.AppSettings["ImageFilePath"];
                    starImage = iTextSharp.text.Image.GetInstance(starImagePath);
                    starImage.Border = 0;
                    footballImage.ScaleAbsolute(115, 80);

                    iTextSharp.text.Image AthlImage;
                    string AthlImagePath = ConfigurationManager.AppSettings["SCimages"].ToString() + "2.jpg"; //  ConfigurationManager.AppSettings["ImageFilePath"];
                    AthlImage = iTextSharp.text.Image.GetInstance(AthlImagePath);
                    AthlImage.Border = 0;
                    AthlImage.ScaleAbsolute(100, 90);

                    iTextSharp.text.Image basketImage;
                    string basketImagePath = ConfigurationManager.AppSettings["SCimages"].ToString() + "3.jpg"; //  ConfigurationManager.AppSettings["ImageFilePath"];
                    basketImage = iTextSharp.text.Image.GetInstance(basketImagePath);
                    basketImage.Border = 0;
                    basketImage.ScaleAbsolute(150, 280);

                    PdfPTable table = new PdfPTable(6);
                    table.DefaultCell.Border = 0;
                    table.HorizontalAlignment = 1;
                    table.TotalWidth = 760f;
                    table.LockedWidth = true;

                    float[] widths = new float[] { 16f, 42f, 42f, 40f, 49f, 43f };
                    table.SetWidths(widths);

                    PdfPCell cell = new PdfPCell();
                    cell.AddElement(TipsSideImage);
                    cell.Rowspan = 6;
                    cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                    cell.PaddingLeft = -35;
                    cell.PaddingTop = -8;
                    cell.Border = 0;
                    table.AddCell(cell);

                    PdfPCell cell1 = new PdfPCell();
                    cell1.AddElement(logoImage);
                    cell1.Rowspan = 1;
                    cell1.Colspan = 5;
                    cell1.PaddingLeft = 260;
                    cell1.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    cell1.Border = 0;
                    table.AddCell(cell1);

                    PdfPCell cell2 = new PdfPCell();
                    cell2.AddElement(footballImage);
                    cell2.PaddingTop = -10;
                    cell2.Rowspan = 1;
                    cell2.Colspan = 1;
                    cell2.Border = 0;
                    table.AddCell(cell2);

                    BaseFont bfTimesI = BaseFont.CreateFont(BaseFont.TIMES_ITALIC, BaseFont.CP1252, false);
                    iTextSharp.text.Font timesI = new iTextSharp.text.Font(bfTimesI, 16, iTextSharp.text.Font.ITALIC, iTextSharp.text.BaseColor.BLACK);
                    PdfPCell cell3 = new PdfPCell(new Phrase("Award for excellence in sports - " + sp.acady + "", timesI));
                    cell3.Rowspan = 1;
                    cell3.Colspan = 4;
                    cell3.PaddingTop = 35;
                    cell3.PaddingLeft = 50;
                    cell3.Border = 0;
                    cell3.HorizontalAlignment = PdfPCell.ALIGN_MIDDLE;
                    cell3.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    table.AddCell(cell3);

                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                    iTextSharp.text.Font times = new iTextSharp.text.Font(bfTimes, 15, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);

                    BaseFont bfTimesbold = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                    iTextSharp.text.Font timesbold = new iTextSharp.text.Font(bfTimes, 15, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);

                    Chunk ch1 = new Chunk("Certificate of honour to ", times);
                    Chunk ch2 = new Chunk(stdntname, timesbold);
                    Phrase ph1 = new Phrase(2);
                    ph1.Add(ch1);
                    ph1.Add(ch2);

                    PdfPCell cell4 = new PdfPCell(ph1);
                    cell4.Rowspan = 1;
                    cell4.Colspan = 5;
                    cell4.Border = 0;
                    cell4.PaddingLeft = 105;
                    cell4.PaddingTop = 40;
                    table.AddCell(cell4);

                    PdfPCell cell5 = new PdfPCell(new Phrase("for the excellent performance in " + sp.Sport + "", times));
                    cell5.Rowspan = 1;
                    cell5.Colspan = 5;
                    cell5.Border = 0;
                    cell5.PaddingLeft = 140;
                    cell5.PaddingTop = 15;
                    table.AddCell(cell5);

                    PdfPCell cell7 = new PdfPCell(new Phrase(" Event:  " + sp.Event + " \n\n\n Date:  " + sp.Date + "", times));
                    cell7.Rowspan = 1;
                    cell7.Colspan = 2;
                    cell7.Border = 0;
                    cell7.PaddingTop = 90;
                    cell7.PaddingLeft = 95;
                    table.AddCell(cell7);

                    PdfPCell cell8 = new PdfPCell();
                    cell8.Rowspan = 1;
                    cell8.AddElement(starImage);
                    cell8.Border = 0;
                    cell8.PaddingLeft = 0;
                    cell8.PaddingTop = 5;
                    table.AddCell(cell8);

                    PdfPCell cell9 = new PdfPCell(new Phrase(" Award:  " + sp.Award + " \n\n\n Place:  " + sp.Place + "", times));
                    cell9.Colspan = 2;
                    cell9.Border = 0;
                    cell9.PaddingLeft = 60;
                    cell9.PaddingTop = 90;
                    table.AddCell(cell9);

                    PdfPCell cell10 = new PdfPCell();
                    cell10.AddElement(AthlImage);
                    cell10.Rowspan = 1;
                    cell10.Border = 0;
                    cell10.PaddingTop = 25;
                    cell10.PaddingLeft = -5;
                    table.AddCell(cell10);

                    PdfPCell cell11 = new PdfPCell(new Phrase("Coach", times));
                    cell11.PaddingTop = 60;
                    cell11.PaddingLeft = -40;
                    cell11.Border = 0;
                    table.AddCell(cell11);

                    PdfPCell cell12 = new PdfPCell(new Phrase("Sports Co ordinator", times));
                    cell12.PaddingLeft = 0;
                    cell12.PaddingTop = 60;
                    cell12.Border = 0;
                    table.AddCell(cell12);

                    PdfPCell cell13 = new PdfPCell(new Phrase("Chairman", times));
                    cell13.PaddingLeft = 110;
                    cell13.PaddingRight = -15;
                    cell13.PaddingTop = 60;
                    cell13.Border = 0;
                    table.AddCell(cell13);

                    PdfPCell cell14 = new PdfPCell();
                    cell14.AddElement(basketImage);
                    cell14.PaddingLeft = 75;
                    cell14.PaddingTop = -45;
                    cell14.Border = 0;
                    table.AddCell(cell14);

                    doc1.Add(table);
                    doc1.Close();

                    FileStream fs;
                    fs = new FileStream(SCpath, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    byte[] photo = br.ReadBytes((int)fs.Length);

                    UploadedFiles fu1 = new UploadedFiles();
                    fu1.DocumentData = photo;
                    fu1.DocumentName = sp.Event + " SC";  // sp.Name; // sp.Preregno.ToString() + "SC";
                    fu1.DocumentType = "Sports Certificate";
                    fu1.DocumentSize = fs.Length.ToString();
                    fu1.UploadedDate = DateTime.Now.Date.ToString("dd/MM/yyyy");
                    fu1.DocumentFor = "Student";
                    fu1.PreRegNum = Convert.ToInt64(prereg[x]);              // Pre Registration number
                    ams.CreateOrUpdateUploadedFiles(fu1);
                    br.Close();
                    fs.Close();
                    string dat = fu1.UploadedDate;
                    Dictionary<string, object> criteria1 = new Dictionary<string, object>();
                    criteria1.Clear();
                    criteria1.Add("PreRegNum", Convert.ToInt64(prereg[x]));
                    criteria1.Add("DocumentType", "Sports Certificate");
                    criteria1.Add("DocumentName", sp.Event + " SC");
                    criteria1.Add("UploadedDate", dat);
                    criteria1.Add("DocumentFor", "Student");
                    Dictionary<long, IList<UploadedFilesView>> uploadedfilesview = ams.GetUploadedFilesViewListWithPagingAndCriteria(0, 50, "", "", criteria1);

                    sp.CreatedDate = DateTime.Now.ToShortDateString();
                    sp.Id = 0;
                    sp.Name = stdntname;
                    sp.Preregno = Convert.ToInt32(prereg[x]);
                    if (uploadedfilesview.First().Value.Count > 0) { sp.SportsId = Convert.ToInt32(uploadedfilesview.First().Value[0].Id); }
                    ams.CreateOrUpdateSportsDetails(sp);
                }
                Session["sportspdf"] = "yes";
                return RedirectToAction("StudentManagement", new { pagename = "SC", resetsession = "no" });
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        private static void addCell(PdfPTable table, string text, int rowspan)
        {
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            iTextSharp.text.Font times = new iTextSharp.text.Font(bfTimes, 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);

            PdfPCell cell = new PdfPCell(new Phrase(text, times));
            cell.Rowspan = rowspan;
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            table.AddCell(cell);
        }

        public JsonResult Bonafide(string PreRegNo)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("PreRegNum", Convert.ToInt64(PreRegNo));

                AdmissionManagementService ams = new AdmissionManagementService();
                Dictionary<long, IList<StudentTemplate>> studenttemplate = ams.GetStudentDetailsListWithEQsearchCriteria(0, 50, string.Empty, string.Empty, criteria);
                TransferDetails td = new TransferDetails();
                td.PreRegNum = Convert.ToInt64(PreRegNo);
                td.AcademicYear = studenttemplate.First().Value[0].AcademicYear;
                td.AdmissionDate = studenttemplate.First().Value[0].CreatedDate;
                td.ApplicationNo = studenttemplate.First().Value[0].ApplicationNo;
                td.FeeStructYear = studenttemplate.First().Value[0].FeeStructYear;
                td.Gender = studenttemplate.First().Value[0].Gender;
                td.Name = studenttemplate.First().Value[0].Name;
                td.Type = "Bonafide";
                td.AfterId = studenttemplate.First().Value[0].NewId;
                td.AfterCampus = studenttemplate.First().Value[0].Campus;
                td.AfterSection = studenttemplate.First().Value[0].Section;
                td.AfterGrade = studenttemplate.First().Value[0].Grade;
                td.TransferedDate = DateTime.Now.ToString();
                ams.CreateOrUpdateTransferDetails(td);

                UploadedFilesView fu1 = new UploadedFilesView();
                fu1.DocumentName = "Bonafide Certificate";
                //    fu1.DocumentType = st.UploadedFilesList[0].DocumentType;
                fu1.DocumentType = "" + studenttemplate.First().Value[0].Campus + " Bonafide Certificate";
                fu1.UploadedDate = DateTime.Now.Date.ToString("dd/MM/yyyy");
                fu1.DocumentFor = "Student";
                fu1.Type = "BonafidePDF";
                fu1.PreRegNum = Convert.ToInt64(PreRegNo);              // Pre Registration number
                ams.CreateOrUpdateUploadedFilesView(fu1);
                return Json(td, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public void PDFGeneratorBonafide(long id, string type, AddressDetails add)
        {
            try
            {
                AdmissionManagementService ams = new AdmissionManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();

                criteria.Add("PreRegNum", Convert.ToInt64(id));
                Dictionary<long, IList<StudentTemplate>> studenttemplate = ams.GetStudentDetailsListWithEQsearchCriteria(0, 50, string.Empty, string.Empty, criteria);

                criteria.Clear();
                criteria.Add("PreRegNum", Convert.ToInt64(id));
                Dictionary<long, IList<FamilyDetails>> FamilyDetails = ams.GetFamilyDetailsListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                criteria.Clear();
                IEnumerable<FamilyDetails> fathern;
                IEnumerable<FamilyDetails> mothern;
                fathern = from cust in FamilyDetails.FirstOrDefault().Value
                          where cust.FamilyDetailType == "Father"
                          select cust;
                mothern = from cust in FamilyDetails.FirstOrDefault().Value
                          where cust.FamilyDetailType == "Mother"
                          select cust;

                var doc1 = new iTextSharp.text.Document();
                string BCpath = ConfigurationManager.AppSettings["BC"].ToString() + id + ".pdf";
                PdfWriter.GetInstance(doc1, new FileStream(BCpath, FileMode.Create));
                doc1.Open();
                PdfPTable table = new PdfPTable(1);
                table.DefaultCell.Border = 0;
                table.DefaultCell.PaddingBottom = 8;

                iTextSharp.text.Image LogoImage = iTextSharp.text.Image.GetInstance(ConfigurationManager.AppSettings["LogoImagePath"].ToString());
                PdfPCell imgcel1 = new PdfPCell();
                imgcel1.Border = 0;
                imgcel1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                LogoImage.ScalePercent(12f);
                table.AddCell(imgcel1);

                PdfPCell cell1 = new PdfPCell(new Phrase("Date: " + DateTime.Now.ToString("d-MMMM-yyyy") + "", new iTextSharp.text.Font(FontFactory.GetFont("Helvetica", 12.0f, iTextSharp.text.Font.BOLD, new BaseColor(0, 0, 0)))));
                cell1.HorizontalAlignment = 2;
                cell1.Border = 0;
                cell1.PaddingTop = 100;
                cell1.PaddingBottom = 40;
                table.AddCell(cell1);
                PdfPCell cell = new PdfPCell(new Phrase("Bonafide Certificate", new iTextSharp.text.Font(FontFactory.GetFont("Helvetica", 16.0f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE, new BaseColor(0, 0, 0)))));
                cell.Colspan = 2;
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right            
                cell.Border = 0;
                cell.PaddingBottom = 20;
                table.AddCell(cell);

                string para = "";
                var fname = "";
                var mname = "";
                if (fathern.FirstOrDefault() != null)
                {
                    fname = fathern.FirstOrDefault().Name;
                    if (!fname.Contains("Mr"))
                        fname = "Mr." + fname;

                }
                if (mothern.FirstOrDefault() != null)
                {
                    mname = mothern.FirstOrDefault().Name;
                    if (!mname.Contains("Mrs"))
                        mname = "Mrs." + mname;
                }
                string gender = "";
                string gender1 = "";
                string child = "";
                if (studenttemplate.First().Value[0].Gender == "Male")
                {
                    gender = "He";
                    gender1 = "His";
                    child = "son";
                }
                if (studenttemplate.First().Value[0].Gender == "Female")
                {
                    gender = "She";
                    gender1 = "Her";
                    child = "daughter";
                }
                string dob = "";
                dob = studenttemplate.First().Value[0].DOB;

                string coma1 = ""; string coma2 = ""; string and = "";
                if (studenttemplate.First().Value[0].AddressDetailsList.Count > 0)
                {
                    if ((studenttemplate.First().Value[0].AddressDetailsList[0].Add1 != "") && (studenttemplate.First().Value[0].AddressDetailsList[0].Add1 != null))
                    {
                        coma1 = ", ";
                    }
                    if ((studenttemplate.First().Value[0].AddressDetailsList[0].Add2 != "") && (studenttemplate.First().Value[0].AddressDetailsList[0].Add2 != null))
                    {
                        coma2 = ", ";
                    }
                    if (((fname != "") && (fname != null)) && ((mname != null) && (mname != "")))
                    {
                        and = " and ";
                    }
                }
                string add1 = ""; string add2 = ""; string add3 = ""; string city = ""; string state = ""; string country = ""; string pin = "";
                if (studenttemplate.First().Value[0].AddressDetailsList.Count > 0 && type == "BonafidePDF")
                {
                    add1 = add.Add1;
                    add2 = add.Add2;
                    add3 = add.Add3;
                    city = add.City;
                    state = add.State;
                    country = add.Country;
                    pin = add.Pin;
                }
                else
                {
                    if (studenttemplate.First().Value[0].AddressDetailsList.Count > 0)
                    {
                        add1 = studenttemplate.First().Value[0].AddressDetailsList[0].Add1;
                        add2 = studenttemplate.First().Value[0].AddressDetailsList[0].Add2;
                        add3 = studenttemplate.First().Value[0].AddressDetailsList[0].Add3;
                        city = studenttemplate.First().Value[0].AddressDetailsList[0].City;
                        state = studenttemplate.First().Value[0].AddressDetailsList[0].State;
                        country = studenttemplate.First().Value[0].AddressDetailsList[0].Country;
                        pin = studenttemplate.First().Value[0].AddressDetailsList[0].Pin;
                    }
                }
                para = "       This is to certify that " + studenttemplate.First().Value[0].Name + " " + studenttemplate.First().Value[0].Initial + " IDNo:" + studenttemplate.First().Value[0].NewId + ", " + child + " of " + fname + and + mname + "";
                para = para + " is a bonafide pupil of this school in Grade " + studenttemplate.First().Value[0].Grade + "-" + studenttemplate.First().Value[0].Section + " for";
                para = para + " the academic year " + studenttemplate.First().Value[0].AcademicYear + ". " + gender1 + " date of birth as per the school record is " + dob + ".";

                para = para + " " + gender + " is residing at " + add1 + coma1 + add2 + coma2 + add3 + ". ";
                para = para + "" + city + ". " + state + "-" + pin + ". " + country + ".";

                Paragraph par = new Paragraph();
                Chunk chnk = new Chunk(para, new iTextSharp.text.Font(FontFactory.GetFont("Helvetica", 14.0f)));
                chnk.setLineHeight(25);

                par.Alignment = Element.ALIGN_JUSTIFIED;
                par.IndentationRight = 50;
                par.IndentationLeft = 50;
                par.Add(chnk);

                iTextSharp.text.Image saveImage;
                AdmissionManagementService ads = new AdmissionManagementService();
                Dictionary<string, object> criteria4 = new Dictionary<string, object>();
                criteria4.Add("PreRegNum", Convert.ToInt64(id));
                criteria4.Add("DocumentType", "Student Photo");

                Dictionary<long, IList<UploadedFiles>> UploadedFiles = ads.GetUploadedFilesListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteria4);

                if (UploadedFiles != null && UploadedFiles.FirstOrDefault().Value != null && UploadedFiles.FirstOrDefault().Value.Count != 0)
                {
                    if (UploadedFiles.First().Value[0].OldFiles == 1)
                    {
                        string ImagePath = UploadedFiles.First().Value[0].FileDirectory + "\\" + UploadedFiles.First().Value[0].DocumentName;// "green.jpg"; ConfigurationManager.AppSettings["ImageFilePath1"] 

                        if (!System.IO.File.Exists(ImagePath))
                        {
                            ImagePath = ConfigurationManager.AppSettings["ImageFilePath1"] + UploadedFiles.First().Value[0].DocumentName;
                        }
                        saveImage = iTextSharp.text.Image.GetInstance(ImagePath);
                    }
                    else
                    {
                        IList<UploadedFiles> list = UploadedFiles.FirstOrDefault().Value;
                        UploadedFiles doc = list.FirstOrDefault();
                        if (doc.DocumentData != null)
                        {
                            saveImage = iTextSharp.text.Image.GetInstance(doc.DocumentData);
                        }
                        else
                        {
                            string ImagePath = ConfigurationManager.AppSettings["ImageFilePath"];
                            saveImage = iTextSharp.text.Image.GetInstance(ImagePath);
                        }
                    }
                }
                else
                {
                    var dir = Server.MapPath("/Images");
                    //    var path = Path.Combine("ff906c97-1dd5-41bc-aff0-b7e1ee2bec13_1.jpg");
                    string ImagePath = ConfigurationManager.AppSettings["ImageFilePath"];
                    saveImage = iTextSharp.text.Image.GetInstance(ImagePath);
                }

                PdfPTable table1 = new PdfPTable(2);
                table1.DefaultCell.Border = 0;
                PdfPCell cell5 = new PdfPCell(new Phrase("With Regards,", new iTextSharp.text.Font(FontFactory.GetFont("Helvetica", 14.0f))));
                cell5.Border = 0;
                cell5.PaddingTop = 40;

                table1.AddCell(cell5);
                PdfPCell cell3 = new PdfPCell();
                cell3.Border = 0;
                cell3.Rowspan = 6;
                cell3.PaddingTop = 40;
                cell3.PaddingLeft = 25;
                saveImage.ScaleAbsolute(90f, 115f);
                //  saveImage.ScalePercent(30,30);
                cell3.AddElement(saveImage);
                table1.AddCell(cell3);
                PdfPCell cell2 = new PdfPCell(new Phrase("For The Indian Public School", new iTextSharp.text.Font(FontFactory.GetFont("Helvetica", 14.0f))));
                cell2.Border = 0;
                cell2.PaddingBottom = 70;
                table1.AddCell(cell2);
                table1.AddCell("");
                table1.AddCell("_____________________");
                table1.AddCell("");
                PdfPCell cell6 = new PdfPCell(new Phrase("Authorized Signatory", new iTextSharp.text.Font(FontFactory.GetFont("Helvetica", 14.0f))));
                cell6.Border = 0;
                table1.AddCell(cell6);
                table1.AddCell("");

                doc1.Add(table);
                doc1.Add(par);
                doc1.Add(table1);
                doc1.Close();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public void PDFGeneratorTransfer(long id)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("PreRegNum", Convert.ToInt64(id));

                AdmissionManagementService ams = new AdmissionManagementService();
                Dictionary<long, IList<StudentTemplate>> studenttemplate = ams.GetStudentDetailsListWithEQsearchCriteria(0, 50, string.Empty, string.Empty, criteria);

                criteria.Clear();
                criteria.Add("PreRegNum", Convert.ToInt64(id));
                Dictionary<long, IList<FamilyDetails>> FamilyDetails = ams.GetFamilyDetailsListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);

                IEnumerable<FamilyDetails> fathern;
                IEnumerable<FamilyDetails> mothern;
                IEnumerable<FamilyDetails> guardian;
                fathern = from cust in FamilyDetails.FirstOrDefault().Value
                          where cust.FamilyDetailType == "Father"
                          select cust;
                mothern = from cust in FamilyDetails.FirstOrDefault().Value
                          where cust.FamilyDetailType == "Mother"
                          select cust;
                guardian = from cust in FamilyDetails.FirstOrDefault().Value
                           where cust.FamilyDetailType == "Guardian"
                           select cust;

                criteria.Clear();
                //criteria.Add("Type", "Discontinue");
                criteria.Add("PreRegNum", Convert.ToInt64(id));
                Dictionary<long, IList<TCRequestDetails>> td = ams.GetTCRequestDetailsListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                //Dictionary<long, IList<TransferDetails>> td = ams.GetTransferDetailsListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);

                var doc1 = new iTextSharp.text.Document();
                // string TCpath = ConfigurationManager.AppSettings["TC"];
                string TCpath = ConfigurationManager.AppSettings["TC"].ToString() + id + ".pdf";
                PdfWriter.GetInstance(doc1, new FileStream(TCpath, FileMode.Create));
                doc1.Open();

                PdfPTable table = new PdfPTable(3);
                float[] widths = new float[] { 30f, 5f, 42f };
                table.SetWidths(widths);
                table.DefaultCell.Border = 0;
                table.DefaultCell.PaddingBottom = 9;

                iTextSharp.text.Image LogoImage = iTextSharp.text.Image.GetInstance(ConfigurationManager.AppSettings["LogoImagePath"].ToString());
                PdfPCell imgcel1 = new PdfPCell();
                imgcel1.Border = 0;
                LogoImage.ScalePercent(12f);
                table.AddCell(imgcel1);

                PdfPCell cell0 = new PdfPCell(new Phrase("TC No : " + studenttemplate.First().Value[0].PreRegNum, new iTextSharp.text.Font(FontFactory.GetFont("Helvetica", 15.0f))));
                cell0.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                cell0.Colspan = 3;
                cell0.Border = 0;
                cell0.PaddingTop = 80;
                cell0.PaddingBottom = 5;
                table.AddCell(cell0);

                //table.DefaultCell.Phrase = new  Phrase() { iTextSharp.text.Font=times };
                PdfPCell cell = new PdfPCell(new Phrase("Transfer Certificate", new iTextSharp.text.Font(FontFactory.GetFont("Helvetica", 15.0f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE, new BaseColor(0, 0, 0)))));
                cell.Colspan = 3;
                cell.Border = 0;
                //    cell.PaddingTop = 80;
                cell.PaddingBottom = 25;
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);

                PdfPCell cell11 = new PdfPCell(new Phrase("Name Of The Student"));
                cell11.Border = 0;
                table.AddCell(cell11);
                table.AddCell("");
                //table.AddCell("Name Of The Student");
                table.AddCell(":   " + studenttemplate.First().Value[0].Name + " " + studenttemplate.First().Value[0].Initial);
                table.AddCell("Name Of Father/Guardian"); table.AddCell("");
                if (fathern.FirstOrDefault() != null)
                {
                    table.AddCell(":   " + fathern.First().Name + "");
                }
                else if (guardian.FirstOrDefault() != null)
                {
                    table.AddCell(":   " + guardian.First().Name + "");
                }
                else
                {
                    table.AddCell(":   ");
                }
                table.AddCell("Name Of Mother/Guardian"); table.AddCell("");
                mothern.DefaultIfEmpty();
                if (mothern.FirstOrDefault() != null)
                {
                    table.AddCell(":   " + mothern.First().Name + "");
                }
                else if (guardian.FirstOrDefault() != null)
                {
                    table.AddCell(":   " + guardian.First().Name + "");
                }
                else
                {
                    table.AddCell(":   ");
                }

                DateTime dateValue;

                table.AddCell("Birthdate As Entered In Admission Register"); table.AddCell("");
                table.AddCell(":   " + (studenttemplate.First().Value[0].DOB) + "");
                table.AddCell("Gender"); table.AddCell("");
                table.AddCell(":   " + studenttemplate.First().Value[0].Gender + "");

                table.AddCell("Class To Which Admitted"); table.AddCell("");
                table.AddCell(":   " + td.First().Value[0].AdmittedClass + "");
                table.AddCell("Date Of Admission"); table.AddCell("");
                {
                    table.AddCell(":   " + (studenttemplate.First().Value[0].CreatedDate) + "");
                }
                table.AddCell("Campus"); table.AddCell("");
                table.AddCell(":   " + studenttemplate.First().Value[0].Campus + "");
                //table.AddCell(":   " +Convert.ToDateTime( studenttemplate.First().Value[0].CreatedDate,dtfi).ToString("dd/MM/yyyy") + "");
                table.AddCell("Present Class At Time Of Leaving School"); table.AddCell("");
                table.AddCell(":   " + studenttemplate.First().Value[0].Grade + "");
                table.AddCell("Whether Qualified For Promotion To Higher Class"); table.AddCell("");
                table.AddCell(":   " + td.First().Value[0].QualifiedForPromotion + "");
                table.AddCell("Last Date Of Attendance In The School"); table.AddCell("");
                //     table.AddCell(":   " + Convert.ToDateTime(td.First().Value[0].LastDateOfAttendance).ToString("dd/MM/yyyy") + "");
                table.AddCell(":   " + (td.First().Value[0].LastDateOfAttendance) + "");
                table.AddCell("Reasons For Leaving The School"); table.AddCell("");
                table.AddCell(":   " + td.First().Value[0].ReasonForLeaving + "");
                table.AddCell("General Conduct"); table.AddCell("");
                table.AddCell(":   " + td.First().Value[0].Conduct + "");
                table.AddCell("Date Of Issue Of Transfer Certificate"); table.AddCell("");
                if (DateTime.TryParseExact(td.First().Value[0].TransferedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue))
                {
                    table.AddCell(":   " + (td.First().Value[0].TransferedDate).ToString() + "");
                }
                else if (DateTime.TryParseExact(td.First().Value[0].TransferedDate, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue))
                {
                    table.AddCell(":   " + (td.First().Value[0].TransferedDate).ToString() + "");
                }
                else if (DateTime.TryParseExact(td.First().Value[0].TransferedDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue))
                {
                    table.AddCell(":   " + (td.First().Value[0].TransferedDate).ToString() + "");
                }
                else if (DateTime.TryParseExact(td.First().Value[0].TransferedDate, "M/d/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue))
                {
                    table.AddCell(":   " + (td.First().Value[0].TransferedDate).ToString() + "");
                }
                else
                {
                    table.AddCell(":   " + td.First().Value[0].TransferedDate + "");
                }

                iTextSharp.text.Image saveImage;
                AdmissionManagementService ads = new AdmissionManagementService();
                Dictionary<string, object> criteria4 = new Dictionary<string, object>();
                criteria4.Add("PreRegNum", Convert.ToInt64(id));
                criteria4.Add("DocumentType", "Student Photo");

                Dictionary<long, IList<UploadedFiles>> UploadedFiles = ads.GetUploadedFilesListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteria4);

                if (UploadedFiles != null && UploadedFiles.FirstOrDefault().Value != null && UploadedFiles.FirstOrDefault().Value.Count != 0)
                {
                    if (UploadedFiles.First().Value[0].OldFiles == 1)
                    {
                        string ImagePath = UploadedFiles.First().Value[0].FileDirectory + "\\" + UploadedFiles.First().Value[0].DocumentName;// "green.jpg"; ConfigurationManager.AppSettings["ImageFilePath1"] 

                        if (!System.IO.File.Exists(ImagePath))
                        {
                            ImagePath = ConfigurationManager.AppSettings["ImageFilePath1"] + UploadedFiles.First().Value[0].DocumentName;
                        }
                        saveImage = iTextSharp.text.Image.GetInstance(ImagePath);
                    }
                    else
                    {
                        IList<UploadedFiles> list = UploadedFiles.FirstOrDefault().Value;
                        UploadedFiles doc = list.FirstOrDefault();
                        if (doc.DocumentData != null)
                        {
                            saveImage = iTextSharp.text.Image.GetInstance(doc.DocumentData);
                        }
                        else
                        {
                            string ImagePath = ConfigurationManager.AppSettings["ImageFilePath"];
                            saveImage = iTextSharp.text.Image.GetInstance(ImagePath);
                        }
                    }
                }
                else
                {
                    var dir = Server.MapPath("/Images");
                    string ImagePath = ConfigurationManager.AppSettings["ImageFilePath"];
                    saveImage = iTextSharp.text.Image.GetInstance(ImagePath);
                }
                //   saveImage.Width = 160;
                PdfPCell imgcel = new PdfPCell();
                imgcel.PaddingTop = 10;
                imgcel.Border = 0;
                saveImage.ScaleAbsolute(115, 115);
                //saveImage.ScalePercent(30, 30);
                imgcel.AddElement(saveImage);
                imgcel.PaddingLeft = 5;
                table.AddCell(imgcel);

                OnBarcode.Barcode.QRCode qrcode = new OnBarcode.Barcode.QRCode();
                string qrcodedata = "TC No: " + studenttemplate.First().Value[0].PreRegNum + "\nName: " + studenttemplate.First().Value[0].Name; // "\nDOB: " + studenttemplate.First().Value[0].DOB + "\n";

                qrcodedata = qrcodedata + "\nTC Dt: " + td.First().Value[0].TransferedDate + "";
                qrcode.Data = qrcodedata;
                qrcode.X = 3;
                // Create QR-Code and encode barcode to Jpeg format
                qrcode.ImageFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
                string var = DateTime.Now.Second.ToString();

                QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
                qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                qrCodeEncoder.QRCodeScale = 2;
                qrCodeEncoder.QRCodeVersion = 9; //0-40
                qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
                System.Drawing.Image image;
                String data = qrcodedata;// txtEncodeData.Text;
                image = qrCodeEncoder.Encode(data);

                iTextSharp.text.Image QRImage = iTextSharp.text.Image.GetInstance(image, System.Drawing.Imaging.ImageFormat.Jpeg);

                PdfPCell QRimgcel = new PdfPCell();
                QRimgcel.PaddingTop = 10;
                QRimgcel.PaddingLeft = -20;
                QRimgcel.Border = 0;
                QRImage.ScalePercent(70f);
                QRimgcel.AddElement(QRImage);
                table.AddCell(QRimgcel);

                PdfPCell signcell = new PdfPCell(new Phrase("Authorized Signatory"));
                signcell.PaddingTop = 120;
                signcell.PaddingLeft = 95;
                signcell.Border = 0;
                table.AddCell(signcell);
                doc1.Add(table);
                doc1.Close();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public void HTMLToPdf(string HTML, string FilePath)
        {

        }
        private void ShowPdf(string s)
        {
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("Content-Disposition", "inline;filename=" + s);
            Response.ContentType = "application/pdf";
            Response.WriteFile(s);
            Response.Flush();
            Response.Clear();
        }

        private Stream GeneratePDF()
        {
            MemoryStream ms = new MemoryStream();

            byte[] byteInfo = null;
            ms.Write(byteInfo, 0, byteInfo.Length);
            ms.Position = 0;

            return ms;
        }

        public ActionResult Transfer(string PreRegNo)
        {
            Session["transfered"] = "";
            Session["transferedId"] = "";
            User Userobj = (User)Session["objUser"];
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            var usrcmp = Session["UserCampus"] as IEnumerable<string>;
            if (usrcmp != null && usrcmp.Count() != 0)
            {
                if (usrcmp.First() != null)            // to check if the usrcmp obj is null or with data
                {
                    criteria.Add("Name", usrcmp);
                }
            }
            MastersService ms = new MastersService();
            Dictionary<long, IList<CampusMaster>> CampusMaster = ms.GetCampusMasterNEqListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);

            ViewBag.campusddl = CampusMaster.First().Value;

            criteria.Clear();
            Dictionary<long, IList<GradeMaster>> GradeMaster = ms.GetGradeMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            ViewBag.gradeddl = GradeMaster.First().Value;
            Dictionary<long, IList<SectionMaster>> SectionMaster = ms.GetSectionMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            ViewBag.sectionddl = SectionMaster.First().Value;
            Dictionary<long, IList<AcademicyrMaster>> AcademicMaster = ms.GetAcademicyrMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            ViewBag.acadddl = AcademicMaster.First().Value;
            StudentTemplate st = new StudentTemplate();
            st.Grade = Session["promotngrd"].ToString();
            return View(st);
        }

        public bool transfergrade(string grd)
        {
            Session["promotngrd"] = "";
            Session["promotngrd"] = grd;
            return true;
        }

        [HttpPost]
        public ActionResult Transfer(StudentTemplate st)
        {
            TransferDetails td = new TransferDetails();
            string RecipientInfo = string.Empty, Subject = string.Empty, Body = "", MailBody = ""; bool retValue;
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            AdmissionManagementService ams = new AdmissionManagementService();
            EmailHelper em = new EmailHelper();
            UserService us = new UserService();
            PassworAuth PA = new PassworAuth();
            MailBody = GetBodyofMail();
            criteria.Add("PreRegNum", Convert.ToInt64(st.PreRegNum));
            Dictionary<long, IList<StudentTemplate>> studenttemplate = ams.GetStudentDetailsListWithEQsearchCriteria(0, 50, string.Empty, string.Empty, criteria);
            if (string.Equals(st.Campus, studenttemplate.First().Value[0].Campus, StringComparison.CurrentCultureIgnoreCase))
            {
                Session["transfered"] = "no";
                return RedirectToAction("StudentManagement", new { pagename = "transfer" });
            }
            else
            {
                td.BeforeCampus = studenttemplate.First().Value[0].Campus;
                td.BeforeGrade = studenttemplate.First().Value[0].Grade;
                td.BeforeSection = studenttemplate.First().Value[0].Section;
                td.BeforeId = studenttemplate.First().Value[0].NewId;
                td.AcademicYear = studenttemplate.First().Value[0].AcademicYear;
                td.ApplicationNo = studenttemplate.First().Value[0].ApplicationNo;
                td.FeeStructYear = studenttemplate.First().Value[0].FeeStructYear;
                td.Gender = studenttemplate.First().Value[0].Gender;
                td.TransferedDate = DateTime.Now.ToString();
                td.Name = studenttemplate.First().Value[0].Name;
                td.Type = "Transfer";
                studenttemplate.First().Value[0].Campus = st.Campus;
                studenttemplate.First().Value[0].Grade = st.Grade;
                studenttemplate.First().Value[0].Section = st.Section;

                Session["transfered"] = "yes";
                Session["transferedId"] = studenttemplate.First().Value[0].NewId;
                Session["transferedName"] = studenttemplate.First().Value[0].Name;

                td.PreRegNum = st.PreRegNum;
                td.AfterCampus = st.Campus;
                td.AfterGrade = st.Grade;
                td.AfterSection = st.Section;
                td.AfterId = studenttemplate.First().Value[0].NewId;
                ams.CreateOrUpdateTransferDetails(td);
                ///old Parent userid deactivation
                User olduser = us.GetUserByUserId(td.BeforeId);
                if (olduser != null)    // to check if user already exists
                {
                    olduser.IsActive = false;
                    us.CreateOrUpdateUser(olduser);
                }
                st = studenttemplate.First().Value[0];
                st.NewId = StudentIdNumberLogic(st.Campus, studenttemplate.First().Value[0].FeeStructYear, st.Grade, studenttemplate.First().Value[0].AcademicYear);
                st.AdmissionStatus = "Registered";   // if student is transfered and then discontinued and then admitted in other campus
                ams.CreateOrUpdateStudentDetails(st);
                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                IList<CampusEmailId> campusemaildet = GetEmailIdByCampus(st.Campus, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
                string[] dob = st.DOB.ToString().Split('/');
                string sex = "";
                if (st.Gender == "Male") { sex = "his"; }
                else if (st.Gender == "Female") { sex = "her"; }
                RecipientInfo = "Dear Parent,";
                Body = "We are happy to add you in our TIPS family and your admission is approved. “" + st.Name + "” is admitted in Grade “" + st.Grade + "” section “" + st.Section + "” of our “" + st.Campus + "” campus and " + sex + " Id number will be “" + st.NewId + "”.<br/><br/>";
                Body = Body + "Your parent portal id is “" + "P" + st.NewId + "” and password will be “" + dob[0] + dob[1] + dob[2] + "” i.e. the DOB of the student as given in the records. You can access the same through http://portal.tipsglobal.org and <a href='https://play.google.com/store/apps/details?id=com.tips.parent'>Click here</a> to download our TIPS android mobile​ ​application.";
                Body = Body + "<br/><br/>For any queries, mail us at “" + campusemaildet.First().EmailId.ToString() + "”. ";
                // ---------------------------- creating Parent New userid -----------------------------------------
                User userexists = us.GetUserByUserId("P" + st.NewId);
                if (userexists == null)    // to check if user already exists
                {
                    User user = new User();
                    user.UserId = "P" + st.NewId;
                    user.Password = dob[0] + dob[1] + dob[2];
                    user.Campus = st.Campus;
                    user.UserType = "Parent";
                    user.CreatedDate = DateTime.Now;
                    user.ModifiedDate = DateTime.Now;
                    //encode and save the password
                    user.Password = PA.base64Encode(user.Password);
                    user.IsActive = true;
                    us.CreateOrUpdateUser(user);
                }
                //Send Mail to parent for sending ParentPortal Id and Password
                Subject = "Welcome to TIPS - Student Registration Successfull"; // st.Subject;   
                if (ConfigurationManager.AppSettings["SendEmail1"].ToString() == "true")
                {
                    criteria.Clear();
                    criteria.Add("Campus", st.Campus);
                    criteria.Add("DocumentType", "Parent Portal Circular");
                    Dictionary<long, IList<CampusDocumentMaster>> PBCircularDoc = ams.GetCampusDocumentListwithCriteria(0, 10, string.Empty, string.Empty, criteria);
                    Attachment CircularAttach = null;
                    if (PBCircularDoc != null && PBCircularDoc.Count > 0 && PBCircularDoc.FirstOrDefault().Key > 0 && PBCircularDoc.FirstOrDefault().Value.Count > 0)
                    {
                        MemoryStream memStream = new MemoryStream(PBCircularDoc.FirstOrDefault().Value[0].ActualDocument);
                        CircularAttach = new Attachment(memStream, PBCircularDoc.FirstOrDefault().Value[0].DocumentName);  //Data posted from form
                        Body = Body + "<br/><br/>Please find the attached Parent Portal Circular.";
                    }
                    retValue = em.SendStudentRegistrationMail(st, st.EmailId, st.Campus, Subject, Body, MailBody, RecipientInfo, "Parent", CircularAttach);
                }
                return RedirectToAction("StudentManagement", new { pagename = "transfer", resetsession = "no" });
            }
        }

        public ActionResult DetainDiscontinue(string PreRegNo, string type)
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            if (PreRegNo == null)
            {
                criteria.Add("PreRegNum", Convert.ToInt64(Session["transferpreregno"]));
            }
            else
            {
                criteria.Add("PreRegNum", Convert.ToInt64(PreRegNo));
            }

            AdmissionManagementService ams = new AdmissionManagementService();
            Dictionary<long, IList<StudentTemplate>> studenttemplate = ams.GetStudentDetailsListWithEQsearchCriteria(0, 50, string.Empty, string.Empty, criteria);

            string pagename = "";
            string detained = "";
            if (type == "Discontinue")
            {
                studenttemplate.First().Value[0].AdmissionStatus = "Discontinued";
                Session["discontinue"] = "yes";
                Session["discontinueName"] = studenttemplate.First().Value[0].Name;
                pagename = "transfer";
                detained = "no";
                // To deactivate a student from Assess360 by passing Student Primary Key ---- By Anbu
                DeactivateStudent(studenttemplate.First().Value[0].Id);
            }

            if (type == "Detain")
            {
                var acdyr = studenttemplate.First().Value[0].AcademicYear.ToString().Split('-');   // To increment academic year while promoting

                int acd1 = Convert.ToInt32(acdyr[0]);
                acd1 = acd1 + 1;

                int acd2 = Convert.ToInt32(acdyr[1]);
                if (acd2 == DateTime.Now.Year)                  // if current academicyear of student is equal to current year
                {
                    acd2 = acd2 + 1;
                    studenttemplate.First().Value[0].AcademicYear = acd1.ToString() + "-" + acd2.ToString();
                    detained = "yes";
                }
                else { detained = "no"; }
                pagename = "promotion";
            }

            StudentTemplate st = studenttemplate.First().Value[0];
            ams.CreateOrUpdateStudentDetails(st);
            Session["transferpdf"] = "";
            Session["transferpreregno"] = "";
            return RedirectToAction("StudentManagement", new { pagename = pagename });
        }

        public ActionResult Detain(string PreRegNo)
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            if (PreRegNo == null)
            {
                criteria.Add("PreRegNum", Convert.ToInt64(Session["transferpreregno"]));
            }
            else
            {
                criteria.Add("PreRegNum", Convert.ToInt64(PreRegNo));
            }

            AdmissionManagementService ams = new AdmissionManagementService();
            Dictionary<long, IList<StudentTemplate>> studenttemplate = ams.GetStudentDetailsListWithEQsearchCriteria(0, 50, string.Empty, string.Empty, criteria);

            string pagename = "";
            string detained = "";

            var acdyr = studenttemplate.First().Value[0].AcademicYear.ToString().Split('-');   // To increment academic year while promoting
            int acd1 = Convert.ToInt32(acdyr[0]);
            acd1 = acd1 + 1;

            int acd2 = Convert.ToInt32(acdyr[1]);
            if (acd2 == DateTime.Now.Year)                  // if current academicyear of student is equal to current year
            {
                acd2 = acd2 + 1;
                studenttemplate.First().Value[0].AcademicYear = acd1.ToString() + "-" + acd2.ToString();
                detained = "yes";
            }
            else { detained = "no"; }
            pagename = "promotion";
            StudentTemplate st = studenttemplate.First().Value[0];
            ams.CreateOrUpdateStudentDetails(st);
            return Json(new { pagename = pagename, detained = detained, name = studenttemplate.First().Value[0].Name }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ResetSession(string type)
        {
            if (type == "email")
            {
                Session["emailsent"] = "";
            }
            else if (type == "transfer")
            {
                Session["transfered"] = "";
                Session["transferedId"] = "";
                Session["transferedName"] = "";
            }
            else if (type == "discontinue")
            {
                Session["discontinue"] = "";
                Session["discontinueName"] = "";
            }
            else if (type == "promotion")
            {
                Session["promotion"] = "";
                Session["promotionId"] = "";
                Session["notpromotedpreregno"] = "";
            }
            else if (type == "bonafide")
            {
                Session["bonafidepdf"] = "";
            }
            else if (type == "sports")
            {
                Session["sportspdf"] = "";
            }
            else
            {

                //Session["campusname"] = "";

                Session["cmpnm"] = "";
                Session["grd"] = "";
                Session["sect"] = "";
                Session["acdyr"] = "";
                Session["apnam"] = "";
                Session["stats"] = "";
                Session["appno"] = "";
                Session["regno"] = "";
                Session["ishosteller"] = "";
                Session["hostlr"] = "";
                Session["Promotioncamp"] = "";
                Session["pagename"] = "";
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult IdPickupPrint()
        {
            MastersService ms = new MastersService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            var usrcmp = Session["UserCampus"] as IEnumerable<string>;
            if (usrcmp.First() != null)            // to check if the usrcmp obj is null or with data
            {
                criteria.Add("Name", usrcmp);
            }
            Dictionary<long, IList<CampusMaster>> CampusMaster = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            criteria.Clear();
            ViewBag.campusddl = CampusMaster.First().Value;

            Dictionary<long, IList<GradeMaster>> GradeMaster = ms.GetGradeMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            ViewBag.gradeddl = GradeMaster.First().Value;
            Dictionary<long, IList<SectionMaster>> SectionMaster = ms.GetSectionMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            ViewBag.sectionddl = SectionMaster.First().Value;

            AdmissionManagementService ads = new AdmissionManagementService();
            Dictionary<long, IList<StudentTemplate>> st = ads.GetStudentDetailsListWithPagingAndCriteria(null, 10000, null, null, criteria);
            ViewBag.nameddl = st.First().Value;

            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult Studentddl(string campus, string grade, string section)
        {
            try
            {
                AdmissionManagementService ads = new AdmissionManagementService();
                Dictionary<long, string> studnam = new Dictionary<long, string>();

                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Campus", campus);
                criteria.Add("Grade", grade);
                criteria.Add("Section", section);

                Dictionary<long, IList<StudentTemplate>> st = ads.GetStudentDetailsListWithPagingAndCriteria(null, 10000, null, null, criteria);

                foreach (StudentTemplate name in st.First().Value)
                    //{
                    //    studnam.Add(name.Id, name.Name);
                    //}
                    ViewBag.nameddl = st.First().Value;

                return Json(ViewBag.nameddl, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public JsonResult PrintCardJqGrid(string campus, string grade, string section, string Name, string Cardtype, int rows, string sidx, string sord, int? page = 1)
        {
            AdmissionManagementService ads = new AdmissionManagementService();

            if (Cardtype == "PickupCard")
            {
                Dictionary<long, IList<TIPS.Entities.AdmissionEntities.StudentTemplate>> StudentTemplate;
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Campus", campus);
                criteria.Add("Grade", grade);
                criteria.Add("Section", section);
                criteria.Add("Name", Name);
                StudentTemplate = ads.GetStudentDetailsListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);

                Session["prergno"] = StudentTemplate.First().Value[0].PreRegNum;

                Dictionary<long, IList<TIPS.Entities.AdmissionEntities.UploadedFiles>> uploadfiles;
                Dictionary<string, object> criteria1 = new Dictionary<string, object>();
                criteria1.Add("DocumentType", "Pickup Card");
                criteria1.Add("PreRegNum", StudentTemplate.First().Value[0].PreRegNum);
                uploadfiles = ads.GetUploadedFilesListWithPagingAndCriteria(page - 1, rows, sord, sidx, criteria1);
                long totalrecords = uploadfiles.First().Key;
                int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                var jsondat = new
                {
                    total = totalPages,
                    page = page,
                    records = totalrecords,

                    rows = (from items in uploadfiles.First().Value
                            select new
                            {
                                i = 2,
                                cell = new string[] {
                                
                            items.Id.ToString(),
                            items.PreRegNum.ToString(),
                            items.Name                    
                            }
                            })
                };
                return Json(jsondat, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Dictionary<long, IList<TIPS.Entities.AdmissionEntities.StudentTemplate>> StudentTemplate;
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Campus", campus);
                criteria.Add("Grade", grade);
                criteria.Add("Section", section);
                criteria.Add("Name", Name);
                StudentTemplate = ads.GetStudentDetailsListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);

                Session["prergno"] = StudentTemplate.First().Value[0].PreRegNum;

                Dictionary<string, object> criteria1 = new Dictionary<string, object>();
                criteria1.Add("DocumentType", "Student Photo");

                long totalrecords = StudentTemplate.First().Key;
                int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                var jsondat = new
                {
                    total = totalPages,
                    page = page,
                    records = totalrecords,

                    rows = (from items in StudentTemplate.First().Value
                            select new
                            {
                                i = 2,
                                cell = new string[] {
                                items.Id.ToString(),
                            items.PreRegNum.ToString(),
                            items.Name
                            }
                            })
                };
                return Json(jsondat, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult MultipleUploadTest()
        {
            return View();
        }

        public ActionResult UploadDocuments(HttpPostedFileBase[] uploadedFile)
        {
            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase file = Request.Files[i];

                if (file.ContentLength == 0)
                    continue;
            }

            return null;
        }

        public ActionResult SMS()
        {
            AdmissionManagementService ads = new AdmissionManagementService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            Dictionary<long, IList<SMSTemplate>> smstemplate = ads.GetSMSTemplateListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria);
            ViewBag.smstemplate = smstemplate.First().Value;
            return View();
        }

        [HttpPost]
        public ActionResult SMS(SMS sms)
        {
            try
            {
                if (sms != null && !string.IsNullOrEmpty(sms.PreRegNo))
                {
                    var prereg = sms.PreRegNo.ToString().Split(',');

                    AdmissionManagementService ads = new AdmissionManagementService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    string recepientnos = "";
                    string failedrecepientnos = "";
                    foreach (string val in prereg)
                    {
                        if (sms.Father == true)
                        {
                            criteria.Clear();
                            criteria.Add("PreRegNum", Convert.ToInt64(val));
                            criteria.Add("FamilyDetailType", "Father");
                            Dictionary<long, IList<FamilyDetails>> FamilyDetails = ads.GetFamilyDetailsListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria);
                            if (FamilyDetails.First().Value.Count() != 0)
                            {
                                if (FamilyDetails.First().Value[0].Mobile != null)
                                {
                                    // Save stud name and newid in sms table
                                    StudentTemplate stud = ads.GetStudentDetailsByPreRegNo(FamilyDetails.First().Value[0].PreRegNum);
                                    if (stud != null) { sms.StudName = stud.Name; sms.NewId = stud.NewId; }
                                    if (Regex.IsMatch(FamilyDetails.First().Value[0].Mobile, @"(?<!\d)\d{10}(?!\d)", RegexOptions.IgnoreCase))
                                    {
                                        if (recepientnos == "") { recepientnos = FamilyDetails.First().Value[0].Mobile.ToString(); }
                                        else
                                        { recepientnos = recepientnos + "," + FamilyDetails.First().Value[0].Mobile.ToString(); }
                                    }
                                    else
                                    {
                                        if (failedrecepientnos == "") { failedrecepientnos = FamilyDetails.First().Value[0].Mobile.ToString(); }
                                        else
                                        { failedrecepientnos = failedrecepientnos + "," + FamilyDetails.First().Value[0].Mobile.ToString(); }
                                    }
                                }
                            }
                        }
                        if (sms.Mother == true)
                        {
                            criteria.Clear();
                            criteria.Add("PreRegNum", Convert.ToInt64(val));
                            criteria.Add("FamilyDetailType", "Mother");
                            Dictionary<long, IList<FamilyDetails>> FamilyDetails = ads.GetFamilyDetailsListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria);
                            if (FamilyDetails.First().Value.Count() != 0)
                            {
                                if (FamilyDetails.First().Value[0].Mobile != null)
                                {
                                    // Save stud name and newid in sms table
                                    StudentTemplate stud = ads.GetStudentDetailsByPreRegNo(FamilyDetails.First().Value[0].PreRegNum);
                                    if (stud != null) { sms.StudName = stud.Name; sms.NewId = stud.NewId; }
                                    if (Regex.IsMatch(FamilyDetails.First().Value[0].Mobile, @"(?<!\d)\d{10}(?!\d)", RegexOptions.IgnoreCase))
                                    {
                                        if (recepientnos == "") { recepientnos = FamilyDetails.First().Value[0].Mobile.ToString(); }
                                        else
                                        { recepientnos = recepientnos + "," + FamilyDetails.First().Value[0].Mobile.ToString(); }
                                    }
                                    else
                                    {
                                        if (failedrecepientnos == "") { failedrecepientnos = FamilyDetails.First().Value[0].Mobile.ToString(); }
                                        else
                                        { failedrecepientnos = failedrecepientnos + "," + FamilyDetails.First().Value[0].Mobile.ToString(); }
                                    }
                                }
                            }
                        }
                    }

                    string strUrl;
                    string dataString;
                    WebRequest request;
                    WebResponse response;
                    Stream s;
                    StreamReader readStream;
                    //string receipientNo = string.Empty;
                    // string Message = "Henry Welcome to Indian Public School and all future emergency correspondences will be through this account Good Evening Have a nice Day";
                    strUrl = ConfigurationManager.AppSettings["SMSService"].ToString() + "&senderID=TIPSGB&receipientno=" + recepientnos + "&dcs=0&msgtxt=" + sms.Message + "&state=1";
                    try
                    {
                        request = WebRequest.Create(strUrl);
                        response = request.GetResponse();
                        s = response.GetResponseStream();
                        readStream = new StreamReader(s);
                        dataString = readStream.ReadToEnd();
                        response.Close();
                        s.Close();
                        readStream.Close();
                        sms.SuccessSMSNos = recepientnos;
                        sms.FailedSMSNos = failedrecepientnos;
                        sms.CreatedDate = DateTime.Now.ToString();
                        sms.Flag = "success";
                        ads.CreateOrUpdateSMSLog(sms);
                        return RedirectToAction("StudentManagement", new { pagename = "SMS", resetsession = "no", type = "SMSsent" });
                    }
                    catch (Exception)
                    {
                        sms.SuccessSMSNos = recepientnos;
                        sms.FailedSMSNos = failedrecepientnos;
                        sms.CreatedDate = DateTime.Now.ToString();
                        sms.Flag = "failed";
                        sms.url = strUrl;
                        ads.CreateOrUpdateSMSLog(sms);
                        return RedirectToAction("StudentManagement", new { pagename = "SMS", resetsession = "no", type = "NoSMS" });
                    }
                }
                else { throw new Exception("sms object is required"); }
            }
            catch (System.Net.WebException ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult PrintPRF(string PreRegNo)
        {
            try
            {
                ViewBag.PRGhtmltag = PRFhtmltag(PreRegNo);
                return View();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public string PRFhtmltag(string PreRegNo)
        {
            AdmissionManagementService ads = new AdmissionManagementService();

            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("PreRegNum", Convert.ToInt64(PreRegNo));
            Dictionary<long, IList<StudentTemplate>> StudentIdcnt1 = ads.GetStudentDetailsListWithEQsearchCriteria(0, 10000, string.Empty, string.Empty, criteria);

            Dictionary<string, object> criteria3 = new Dictionary<string, object>();

            criteria3.Add("FamilyDetailType", "Father");
            criteria3.Add("PreRegNum", Convert.ToInt64(PreRegNo));
            Dictionary<long, IList<FamilyDetails>> fatherdata = ads.GetFamilyDetailsListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria3);

            criteria3.Clear();
            criteria3.Add("FamilyDetailType", "Mother");
            criteria3.Add("PreRegNum", Convert.ToInt64(PreRegNo));
            Dictionary<long, IList<FamilyDetails>> motherdata = ads.GetFamilyDetailsListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria3);

            criteria3.Clear();
            criteria3.Add("PreRegNum", Convert.ToInt64(PreRegNo));
            Dictionary<long, IList<FamilyDetails>> familydata = ads.GetFamilyDetailsListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria3);

            string[] relations = new string[2];
            relations[0] = "Father"; relations[1] = "Mother";
            IEnumerable<FamilyDetails> fdata = from cust in familydata.First().Value
                                               //     from w in relations
                                               where cust.FamilyDetailType != "Father" && cust.FamilyDetailType != "Mother"    // cust.FamilyDetailType.Contains(w)
                                               select cust;
            List<FamilyDetails> fdat1 = new List<FamilyDetails>();
            fdat1.AddRange(fdata);                   // addrange is used to add list of ienumerable items to list object

            FamilyDetails fd = new FamilyDetails();
            if (fatherdata.First().Key > 0)
            { fd = fatherdata.First().Value[0]; }
            FamilyDetails md = new FamilyDetails();
            if (motherdata.First().Key > 0)
            { md = motherdata.First().Value[0]; }

            criteria3.Clear();
            criteria3.Add("PreRegNum", Convert.ToInt64(PreRegNo));
            Dictionary<long, IList<PastSchoolDetails>> pastdata = ads.GetPastSchoolDetailsListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria3);

            System.Text.StringBuilder html = new System.Text.StringBuilder();
            if (StudentIdcnt1.First().Key > 0)
            {
                AddressDetails ad = new AddressDetails();
                string add = "";
                if (StudentIdcnt1.First().Value[0].AddressDetailsList.Count > 0)
                {
                    ad = StudentIdcnt1.First().Value[0].AddressDetailsList.First();
                    add = ad.Add1 + " " + ad.Add2 + " " + ad.Add3 + " " + ad.City + " " + ad.State + " " + ad.Country + " " + ad.Pin;
                }

                html.Append("<table style='font-size:x-large'>");
                html.Append("<tr>");
                html.Append("<td rowspan='2' style='padding-top:0px' ><img src='../../Images/logonace.JPG' height='90' widht='50'> </td>"); // <label style='padding-left:30px'>THE INDIAN PUBLIC SCHOOL </label> </br><label style='padding-left:30px'>PRE-REGISTRATION for the year " + StudentIdcnt1.First().Value[0].AcademicYear + "</label></td> ");
                html.Append("<td colspan='1' style=' font-weight:bold; padding-top:0px;font-size:xx-large'> THE INDIAN PUBLIC SCHOOL </td> ");
                html.Append("<td rowspan='2' style='padding-top:0px' ><img src='../../Images/Tips Logo.JPG' height='90' widht='50'> </td>");
                html.Append("<td style='padding-top:0px;'> No.: " + StudentIdcnt1.First().Value[0].ApplicationNo + "</td></tr>");
                html.Append("<tr><td colspan='2' style='padding-left:20px'>  PRE-REGISTRATION for the year " + StudentIdcnt1.First().Value[0].AcademicYear + " </td>");
                html.Append("<td rowspan='5' >   <img src= '" + Url.Action("uploaddisplay1", "Admission", new { id = PreRegNo }) + "' height='210';width='50' /></td></tr>");

                html.Append("<tr><td colspan='3' style=' padding-left:70px'>Standard Applied for: " + StudentIdcnt1.First().Value[0].Grade + "  <label style=' margin-left:120px'>Date of Birth:  " + StudentIdcnt1.First().Value[0].DOB + "</label></td></tr>");
                html.Append("  <tr><td colspan='3' >Name of child :   " + StudentIdcnt1.First().Value[0].Name + " </td></tr>");
                string trans = ""; if (StudentIdcnt1.First().Value[0].Transport == true) { trans = "Yes"; } else { trans = "No"; }
                html.Append("<tr><td colspan='3' >Gender :  " + StudentIdcnt1.First().Value[0].Gender + "  &nbsp;&nbsp;&nbsp;&nbsp; School transport required :  " + trans + " (Only for Pre-KG to UKG)</td></tr>");
                html.Append("<tr><td colspan='3' >Boarding : " + StudentIdcnt1.First().Value[0].BoardingType + "</td></tr>");
                html.Append("<tr><td colspan='4' >Residential Address : " + add + "</td></tr>");
                html.Append("<tr><td colspan='2' style=' padding-top:14px'>Father's Name :  " + fd.Name + " </td> <td colspan='2'>Education : " + fd.Education + " </td> </tr>");
                html.Append("<tr><td colspan='2'>Occupation : " + fd.Occupation + "</td><td colspan='2'>Mobile : " + fd.Mobile + "</td> </tr>");
                html.Append("<tr><td colspan='4'>Email : " + fd.Email + "</td></tr>");
                html.Append("<tr><td colspan='4' > Company Name & Address : " + fd.CompName + "." + fd.CompAddress + "</td></tr>");
                // html.Append("<tr><td colspan='4'>&nbsp</td></tr>");

                html.Append("<tr><td colspan='2' style=' padding-top:14px'>Mother's Name :  " + md.Name + "</td> <td colspan='2'>Education : " + md.Education + "</td></tr>");
                html.Append("<tr><td colspan='2' >Occupation : " + md.Occupation + " </td> <td colspan='2'>Mobile : " + md.Mobile + "</td></tr>");
                html.Append("<tr><td colspan='4'>Email : " + md.Email + "</td></tr>");
                html.Append("<tr><td colspan='4' style=' padding-bottom:14px'> Company Name & Address : " + md.CompName + "." + md.CompAddress + "</td></tr>");
                //html.Append("<tr><td colspan='4'>&nbsp</td></tr>");
                html.Append("<tr><td colspan='4'>Annual Family Income : " + StudentIdcnt1.First().Value[0].AnnualIncome + "</td></tr>");
                html.Append("<tr><td colspan='4'> Languages spoken at home : " + StudentIdcnt1.First().Value[0].LanguagesKnown + "</td></tr> <tr><td colspan='4'> Previous Schools attended by applicant child :</td></tr>");
                html.Append("<tr><td colspan='4'><table border='1'> ");

                html.Append("<tr><td style='width:16% ; font-weight:bold'>Years: From-To</td><td style='width:16%; font-weight:bold'> Grade level attended </td> <td style='width:16%; font-weight:bold'>School name & city</td></tr>");

                if (pastdata.First().Key == 0) { html.Append("<tr><td>&nbsp</td><td>&nbsp</td> <td>&nbsp</td></tr>"); }
                else
                {
                    for (int i = 0; i < pastdata.First().Key; i++)  // past school details
                    {
                        html.Append("<tr><td>" + pastdata.First().Value[i].FromDate + "-" + pastdata.First().Value[i].ToDate + "</td><td>" + pastdata.First().Value[i].FromGrade + "-" + pastdata.First().Value[i].ToGrade + "</td> <td>" + pastdata.First().Value[i].SchoolName + "," + pastdata.First().Value[i].City + "</td></tr>");
                    }
                }
                html.Append("</table></td></tr>");

                html.Append("<tr><td colspan='4'>Other members of the child living with the child :</td></tr>");
                html.Append("<tr><td colspan='4'><table border='1'> ");
                html.Append("<tr><td style='width:19%; font-weight:bold'>Relationship with Child</td><td style='width:19%; font-weight:bold'> Name </td> <td style='width:19%; font-weight:bold'> Age </td><td style='width:19%; font-weight:bold'> Occupation </td></tr>");
                if (fdat1.Count == 0) { html.Append("<tr><td>&nbsp</td><td>&nbsp</td> <td>&nbsp</td><td>&nbsp</td></tr>"); }
                else
                {
                    for (int i = 0; i < fdat1.Count; i++)
                    {
                        html.Append("<tr><td>" + fdat1[i].FamilyDetailType + "</td><td>" + fdat1[i].Name + "</td> <td>" + fdat1[i].Age + "</td><td>" + fdat1[i].Occupation + "</td></tr>");
                    }
                }

                //html.Append(" <tr><td>&nbsp</td><td>&nbsp</td> <td>&nbsp</td><td>&nbsp</td></tr> <tr><td>&nbsp</td><td>&nbsp</td> <td>&nbsp</td><td>&nbsp</td></tr>");
                html.Append("</table></td></tr>");
                html.Append("<tr><td colspan='4'>Education Goals for my child :" + StudentIdcnt1.First().Value[0].EducationGoals + "</td></tr>"); //<tr><td colspan='4'>&nbsp</td></tr>");
                html.Append("<tr><td colspan='4'> I hereby apply for the admission of my child(name) " + StudentIdcnt1.First().Value[0].Name + " to</td></tr>");
                html.Append("<tr><td colspan='4'> The Indian Public School, " + Place(StudentIdcnt1.First().Value[0].Campus) + " and agree to abide by the rules,regulations and all management decisions thereof.</td></tr>");
                html.Append("<tr><td colspan='4'>All fees paid are non-refundable and non-transferable and pre-registration does not guarantee admission.</td></tr>");
                html.Append("<tr><td>Place: " + Place(StudentIdcnt1.First().Value[0].Campus) + "</td></tr><tr><td>&nbsp</td></tr><tr><td colspan='3'>Date : " + DateTime.Now.ToString("dd/MM/yyyy") + "</td><td>Signature of  Parent</td></tr>");
                html.Append("<tr><td colspan='4'>-------------------------------------------------------------------------------------------------------------------------------------------------------</td></tr>");
                html.Append("<tr><td><img src='../../Images/logonace.JPG' height='90' width='180'></td> <td colspan='1' style='font-weight:bold;font-size:xx-large;'> THE INDIAN PUBLIC SCHOOL</td><td><img src='../../Images/Tips Logo.JPG' height='90' width='180'></td> ");
                html.Append("<td> Date : " + DateTime.Now.ToString("dd/MM/yyyy") + "</td></tr>");
                html.Append(" <tr><td colspan='4'>  PRE-REGISTRATION No : " + StudentIdcnt1.First().Value[0].ApplicationNo + "<label style='margin-left:120px'>Standard Applied for: " + StudentIdcnt1.First().Value[0].Grade + "</label></td></tr>");
                html.Append("<tr><td colspan='4'>Name of Child : " + StudentIdcnt1.First().Value[0].Name + "</td></tr><tr><td colspan='4'>Pre-registration fee received : Rs. ________</td></tr>");
                html.Append("<tr><td colspan='4'>&nbsp</td></tr><tr><td colspan='3'>&nbsp</td> <td>Authorised Signatory</td></tr>");
                html.Append("<tr><td colspan='4' style='font-size:medium'>All fees paid are non-refundable and non transferable and pre-registration does not guarantee admission.</td></tr></table>");
            }
            return html.ToString();
        }

        public string Place(string campus)
        {
            switch (campus)
            {
                case "IB MAIN":
                    return "Coimbatore";
                case "IB KG":
                    return "Coimbatore";
                case "TIPS SARAN":
                    return "Coimbatore";
                case "ERNAKULAM":
                    return "Ernakulam";
                case "TIRUPUR":
                    return "Tirupur";
                case "CHENNAI MAIN":
                    return "Chennai";
                case "CHENNAI CITY":
                    return "Chennai";
                case "KARUR":
                    return "Karur";
                case "TIPS ERODE":
                    return "Erode";
                case "TIPSE CBSE":
                    return "Erode";
                case "TIPS SALEM":
                    return "Salem";
                default:
                    return "";
            }
        }

        public ActionResult EmailLog()
        {
            return View();
        }

        public ActionResult EmailLogGrid(EmailLog emaillog, FormCollection fc, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                AdmissionManagementService ads = new AdmissionManagementService();
                var usrcmp = Session["UserCampus"] as IEnumerable<string>;
                int cnt;
                cnt = Convert.ToInt32(usrcmp.ToList().Count);
                if (usrcmp.ToList().Count == 0) { cnt = 1; }
                string[] str = new string[cnt];
                int i = 0;
                str[i] = "no campus";
                foreach (var var in usrcmp)
                {
                    str[i] = var;
                    i++;
                }
                UserService us = new UserService();
                string colName = string.Empty; string[] values = new string[1];
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Server", ConfigurationManager.AppSettings["CampusEmailType"].ToString());
                criteria.Add("Campus", str);
                Dictionary<long, IList<CampusEmailId>> campus = ads.GetCampusEmailIdListWithPagingAndCriteriaWithAlias(null, null, string.Empty, string.Empty, string.Empty, null, criteria, null);
                criteria.Clear();
                if (!string.IsNullOrEmpty(emaillog.EmailFrom)) { criteria.Add("EmailFrom", emaillog.EmailFrom); }
                if (!string.IsNullOrEmpty(emaillog.EmailTo)) { criteria.Add("EmailTo", emaillog.EmailTo); }
                if (!string.IsNullOrEmpty(emaillog.Subject)) { criteria.Add("Subject", emaillog.Subject); }
                if (!string.IsNullOrEmpty(emaillog.Message)) { criteria.Add("Message", emaillog.Message); }
                if (!string.IsNullOrEmpty(emaillog.StudName)) { criteria.Add("StudName", emaillog.StudName); }
                if (!string.IsNullOrEmpty(emaillog.NewId)) { criteria.Add("NewId", emaillog.NewId); }
                if (Request["IsSent"] == "Yes") { criteria.Add("IsSent", true); }
                if (Request["IsSent"] == "No") { criteria.Add("IsSent", false); }
                Dictionary<long, IList<EmailLog>> EmailLog = ads.GetEmailLogListWithPagingAndCriteria(page - 1, rows, "Desc", "Id", criteria);
                if (EmailLog.Count > 0)
                {
                    long totalrecords = EmailLog.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,
                        rows = (from items in EmailLog.First().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[] {
                                        items.Id.ToString(),
                                        items.StudName,
                                        items.NewId,
                                        items.EmailFrom,
                                        items.EmailTo,
                                        items.EmailBCC,
                                        items.BCC_Count.ToString(),
                                        items.Subject,
                                        items.Message,
                            Convert.ToDateTime( items.EmailDateTime).ToString("dd/MM/yyyy h:mm tt"),
                            items.IsSent.ToString(),
                            items.ActualException
                            }
                                })
                    };
                    return Json(jsondat, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult SportsPopup()
        {
            AdmissionManagementService ads = new AdmissionManagementService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            Dictionary<long, IList<Sports>> sportsdetails = ads.GetSportsDetailsListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria);

            //IEnumerable<char> eventList
            var eventList = from cust in sportsdetails.First().Value
                            select cust.Event;

            ViewBag.distinctsportseventddl = eventList.Distinct();
            return View();
        }
        [HttpPost]
        public ActionResult SportsPopup(Sports sp)
        {
            try
            {
                return RedirectToAction("PrintSC", new { PreRegNo = sp.Preregno1, sportevent = sp.Event });
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult SMSLog()
        {
            return View();
        }
        public JsonResult SMSLogGrid(SMS sms, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                AdmissionManagementService ads = new AdmissionManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(sms.StudName)) { criteria.Add("StudName", sms.StudName); }
                if (!string.IsNullOrEmpty(sms.NewId)) { criteria.Add("NewId", sms.NewId); }
                if (!string.IsNullOrEmpty(sms.SuccessSMSNos)) { criteria.Add("SuccessSMSNos", sms.SuccessSMSNos); }
                if (!string.IsNullOrEmpty(sms.FailedSMSNos)) { criteria.Add("FailedSMSNos", sms.FailedSMSNos); }
                if (!string.IsNullOrEmpty(sms.Message)) { criteria.Add("Message", sms.Message); }
                if (!string.IsNullOrEmpty(sms.CreatedDate)) { criteria.Add("CreatedDate", sms.CreatedDate); }
                if (!string.IsNullOrEmpty(sms.Flag)) { criteria.Add("Flag", sms.Flag); }
                Dictionary<long, IList<SMS>> SMSLog = ads.GetSMSLogListWithPagingAndCriteria(page - 1, rows, "Desc", "Id", criteria);
                if (SMSLog.Count > 0)
                {
                    long totalrecords = SMSLog.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,
                        rows = (from items in SMSLog.First().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[] {
                                        items.Id.ToString(),
                                        items.StudName,
                                        items.NewId,
                              items.SuccessSMSNos.ToString(),
                            items.FailedSMSNos,                        
                            items.Message.ToString(),
                            Convert.ToDateTime( items.CreatedDate).ToString("dd/MM/yyyy h:mm tt")
                            }
                                })
                    };
                    return Json(jsondat, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult AddFollowupdetails(FollowupDetails fw)
        {
            try
            {
                AdmissionManagementService ads = new AdmissionManagementService();
                fw.PreRegNum = Convert.ToInt64(Session["preregno"]);
                ads.CreateOrUpdateFollowupDetails(fw);
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult followupjqgrid(int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                UserService us = new UserService();
                AdmissionManagementService ads = new AdmissionManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("PreRegNum", Convert.ToInt64(Session["preregno"]));

                Dictionary<long, IList<FollowupDetails>> FollowupDetails = ads.GetFollowupDetailsListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);

                long totalrecords = FollowupDetails.First().Key;
                int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                var jsondat = new
                {
                    total = totalPages,
                    page = page,
                    records = totalrecords,

                    rows = (from items in FollowupDetails.First().Value
                            select new
                            {
                                i = 2,
                                cell = new string[] {
                            items.Id.ToString(),
                            items.Remarks,
                            items.FollowupDate,
                            items.CreatedUserName = items.CreatedBy!=null?us.GetUserNameByUserId(items.CreatedBy):"",
                            items.CreatedBy,
                            items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd'/'MM'/'yyyy"):"",
                            items.ModifiedBy,
                            items.ModifiedDate!=null?items.ModifiedDate.Value.ToString("dd'/'MM'/'yyyy"):"",
                            }
                            })
                };
                return Json(jsondat, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult EditNewRegistrationStatus(StudentTemplateStatus st, string AdmissionStatus)
        {
            try
            {
                AdmissionManagementService ads = new AdmissionManagementService();
                ViewBag.admissionstatus = AdmissionStatus;
                st.AdmissionStatus = AdmissionStatus;
                st.Id = Convert.ToInt64(Session["editid"]);
                st.PreRegNum = Convert.ToInt64(Session["preregno"]);
                ads.CreateOrUpdateStudentStatus(st);
                return RedirectToAction("NewRegistration");
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult EditFollowup(FollowupDetails fd)
        {
            try
            {
                AdmissionManagementService ads = new AdmissionManagementService();
                string userId = base.ValidateUser();
                //fd.PreRegNum = Convert.ToInt64(Session["preregno"]);
                FollowupDetails fdedit = ads.GetFollowupDetailsById(Convert.ToInt64(fd.Id));
                fdedit.FollowupDate = fd.FollowupDate;
                fdedit.Remarks = fd.Remarks;
                fdedit.ModifiedBy = userId;
                fdedit.ModifiedDate = DateTime.Now;
                ads.CreateOrUpdateFollowupDetails(fdedit);
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteFollowupDetails(string id)
        {
            try
            {
                AdmissionManagementService ads = new AdmissionManagementService();

                var test = id.Split(',');

                long[] idtodelete = new long[test.Length];
                int i = 0;
                foreach (string val in test)
                {
                    idtodelete[i] = Convert.ToInt64(val);
                    i++;
                }

                ads.DeleteFollowupDetails(idtodelete);

                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public void DeactivateStudent(long Id)
        {
            Assess360Service a360ser = new Assess360Service();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            DateTime DateNow = DateTime.Now;
            //AdmissionManagementService ams = new AdmissionManagementService();
            //StudentTemplate st = ams.GetStudentDetailsById(Id);
            //criteria.Add("AcademicYear", st.AcademicYear);

            //To deactivate a student from Assess360

            criteria.Add("StudentId", Id);
            Dictionary<long, IList<Assess360>> a360 = a360ser.GetAssess360ListWithPagingAndCriteria(0, 9999, "", "", criteria);
            if (a360 != null && a360.FirstOrDefault().Value != null && a360.FirstOrDefault().Value.Count > 0)
            {
                foreach (Assess360 a in a360.FirstOrDefault().Value)
                {
                    a.IsActive = false;
                    a360ser.SaveOrUpdateAssess360(a);
                }
            }

            AdmissionManagementService ams = new AdmissionManagementService();
            StudentTemplate st = ams.GetStudentDetailsById(Id);

            //To deactivate a Parent UserId from User Table
            if (st != null)
            {
                UserService us = new UserService();
                User userObj = us.GetUserByUserId("P" + st.NewId);
                if (userObj != null)
                {
                    userObj.IsActive = false;
                    us.CreateOrUpdateUser(userObj);
                }
            }
        }

        public JsonResult SendFailedEmailFromApplication(string Ids)
        {
            try
            {
                var arrIds = Ids.Split(',');
                long[] bulkIds = new long[arrIds.Length];
                if (Ids != "") { for (int i = 0; i < arrIds.Length; i++) { bulkIds[i] = Convert.ToInt64(arrIds[i]); } }
                WindowsService1 WindowsService1 = new WindowsService1();
                WindowsService1.GetFailedEmailList(bulkIds);
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult AddressDetailsPopUp(long Id, string type, long PreRegNum)
        {
            AdmissionManagementService ads = new AdmissionManagementService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("PreRegNum", PreRegNum);
            StudentTemplate st = new StudentTemplate();
            st = ads.GetStudentDetailsByPreRegNo(PreRegNum);
            ViewBag.Id = Id;
            ViewBag.type = type;
            ViewBag.PreRegNum = PreRegNum;
            return PartialView(st);
        }

        #region Edit or update studtemplate and familydetails

        public ActionResult CommunicationDetailsUpdate()
        {
            string userId = base.ValidateUser();
            if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
            else
            {
                #region BreadCrumb
                string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                #endregion
                return View();
            }
        }


        public JsonResult CommunicationDetailsUpdateJQGrid(string PreRegNum, string NewId, string Name, string Initial, string Campus, string Grade, string Section, string FoodPreference, string Vanno, string BoardingType, string SecondLanguage,

string AdmissionStatus, string AcademicYear, string General_EmailId, string Father_Mobile, string Father_EmailId, string Mother_Mobile, string Mother_EmailId, string LocationName, int

rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                AdmissionManagementService ams = new AdmissionManagementService();
                Dictionary<string, object> Extcriteria = new Dictionary<string, object>();
                Dictionary<string, object> likcriteria = new Dictionary<string, object>();
                var usrcmp = Session["UserCampus"] as IEnumerable<string>;
                string[] str = new string[usrcmp.ToList().Count];
                int i = 0;
                if (usrcmp != null && usrcmp.FirstOrDefault() != null && usrcmp.Count() > 0)            // to check if the usrcmp obj is null or with data
                {
                    foreach (var var in usrcmp)
                    {
                        str[i] = var;
                        i++;
                    }
                }

                if (!string.IsNullOrWhiteSpace(Campus) && str.Length > 0 && Campus != "Select" && Array.Exists(str, element => element == Campus)) { Extcriteria.Add("Campus", Campus); }
                else { Extcriteria.Add("Campus", str); }

                if (Convert.ToInt64(PreRegNum) > 0) { likcriteria.Add("PreRegNum", Convert.ToInt64(PreRegNum)); }
                if (!string.IsNullOrEmpty(NewId)) { likcriteria.Add("NewId", NewId); }
                if (!string.IsNullOrEmpty(Name)) { likcriteria.Add("Name", Name); }
                if (!string.IsNullOrEmpty(Initial)) { Extcriteria.Add("Initial", Initial); }
                //if (!string.IsNullOrEmpty(Campus)) { likcriteria.Add("Campus", Campus); }
                if (!string.IsNullOrEmpty(Grade)) { Extcriteria.Add("Grade", Grade); }
                if (!string.IsNullOrEmpty(Section)) { Extcriteria.Add("Section", Section); }
                if (!string.IsNullOrEmpty(FoodPreference)) { likcriteria.Add("FoodPreference", FoodPreference); }
                if (!string.IsNullOrEmpty(BoardingType)) { likcriteria.Add("BoardingType", BoardingType); }
                if (!string.IsNullOrEmpty(Vanno)) { likcriteria.Add("VanNo", Vanno); }
                if (!string.IsNullOrEmpty(SecondLanguage)) { likcriteria.Add("SecondLanguage", SecondLanguage); }
                if (!string.IsNullOrEmpty(AdmissionStatus)) { likcriteria.Add("AdmissionStatus", AdmissionStatus); }
                if (!string.IsNullOrEmpty(AcademicYear)) { likcriteria.Add("AcademicYear", AcademicYear); }
                if (!string.IsNullOrEmpty(General_EmailId)) { likcriteria.Add("General_EmailId", General_EmailId); }
                if (!string.IsNullOrEmpty(Father_Mobile)) { likcriteria.Add("Father_Mobile", Father_Mobile); }
                if (!string.IsNullOrEmpty(Father_EmailId)) { likcriteria.Add("Father_EmailId", Father_EmailId); }
                if (!string.IsNullOrEmpty(Mother_Mobile)) { likcriteria.Add("Mother_Mobile", Mother_Mobile); }
                if (!string.IsNullOrEmpty(Mother_EmailId)) { likcriteria.Add("Mother_EmailId", Mother_EmailId); }
                if (!string.IsNullOrEmpty(LocationName)) { likcriteria.Add("LocationName", LocationName); }
                sord = sord == "desc" ? "Desc" : "Asc";
                Dictionary<long, IList<StudentTemplateAndFamilyDetails_vw>> StudentTemplateAndFamilyDetailsList = null;
                StudentTemplateAndFamilyDetailsList = ams.StudentTemplateAndFamilyDetails_vwListWithLikeAndExcactSerachCriteria(page - 1, rows, sidx, sord, Extcriteria, likcriteria);
                if (StudentTemplateAndFamilyDetailsList != null && StudentTemplateAndFamilyDetailsList.First().Key > 0)
                {
                    long totalrecords = StudentTemplateAndFamilyDetailsList.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var AssLst = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,
                        rows = (
                             from items in StudentTemplateAndFamilyDetailsList.First().Value
                             select new
                             {
                                 cell = new string[] 
                                         {
                                            items.Id.ToString(),
                                            items.StudentTemplateId.ToString(),
                                            items.PreRegNum.ToString(),
                                            items.NewId,
                                            items.Name,
                                            items.Initial,
                                            items.Campus,
                                            items.Grade,
                                            items.Section,
                                            items.FoodPreference,
                                            items.BoardingType,
                                            items.VanNo,
                                            items.SecondLanguage,
                                            items.AdmissionStatus,
                                            items.AcademicYear,
                                            items.General_EmailId,
                                            items.Father_Id.ToString(),
                                            items.Father_Mobile,
                                            items.Father_EmailId,
                                            items.Mother_Id.ToString(),
                                            items.Mother_Mobile,
                                            items.Mother_EmailId,
                                            items.LocationName,
                                            items.LocationTamilDescription
                                         }
                             }).ToList()
                    };
                    return Json(AssLst, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    var AssLst = new { rows = (new { cell = new string[] { } }) };
                    return Json(AssLst, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public JsonResult EditEmailDetails(StudentTemplateAndFamilyDetails_vw obj)
        {
            try
            {
                string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                FamilyDetails FamilyDetails = new FamilyDetails();
                AdmissionManagementService amc = new AdmissionManagementService();
                if (obj != null && obj.StudentTemplateId > 0)
                {
                    StudentTemplate StudTemp = amc.GetStudentTemplateDetailsById(obj.StudentTemplateId);
                    //  Component.AdmissionManagementBC amc = new Component.AdmissionManagementBC();
                    if (obj.Section != null)
                    {
                        if (StudTemp != null && StudTemp.AdmissionStatus == "Registered")
                        {
                            StudTemp.Section = obj.Section;
                            amc.CreateOrUpdateStudentTemplateDetails(StudTemp);
                            Assess360Service a360srvc = new Assess360Service();
                            Dictionary<string, object> criteria = new Dictionary<string, object>();
                            criteria.Add("StudentId", StudTemp.Id);
                            criteria.Add("Campus", StudTemp.Campus);
                            criteria.Add("Grade", StudTemp.Grade);
                            criteria.Add("IdNo", StudTemp.NewId);
                            criteria.Add("AcademicYear", StudTemp.AcademicYear);
                            Dictionary<long, IList<Assess360>> AssessList = a360srvc.GetAssess360ListWithPagingAndCriteria(0, 50, "", "", criteria);
                            if (AssessList != null && AssessList.FirstOrDefault().Value != null && AssessList.FirstOrDefault().Value.Count > 0)
                            {
                                AssessList.FirstOrDefault().Value[0].Section = StudTemp.Section;
                                a360srvc.SaveOrUpdateAssess360(AssessList.FirstOrDefault().Value[0]);
                            }
                            ReportCardService rcs = new ReportCardService();
                            Dictionary<string, object> criteria1 = new Dictionary<string, object>();
                            criteria1.Add("StudentId", StudTemp.Id);
                            criteria1.Add("Campus", StudTemp.Campus);
                            criteria1.Add("Grade", StudTemp.Grade);
                            criteria1.Add("IdNo", StudTemp.NewId);
                            criteria1.Add("AcademicYear", StudTemp.AcademicYear);
                            Dictionary<long, IList<RptCardMYP>> RptCardMYP = rcs.GetRptCardMYPListWithPagingAndCriteria(0, 50, "", "", criteria1);
                            if (RptCardMYP != null && RptCardMYP.FirstOrDefault().Value != null && RptCardMYP.FirstOrDefault().Value.Count > 0)
                            {
                                RptCardMYP.FirstOrDefault().Value[0].Section = StudTemp.Section;
                                rcs.SaveOrUpdateMYPReportCard(RptCardMYP.FirstOrDefault().Value[0]);
                            }
                        }
                    }
                    if (obj.Name != null)
                    {
                        StudTemp = amc.GetStudentTemplateDetailsById(obj.StudentTemplateId);
                        if (StudTemp != null)
                        {
                            StudTemp.Name = obj.Name;
                            amc.CreateOrUpdateStudentTemplateDetails(StudTemp);
                            if (StudTemp.AdmissionStatus == "Registered")
                            {
                                Assess360Service a360srvc = new Assess360Service();
                                Dictionary<string, object> criteria = new Dictionary<string, object>();
                                criteria.Add("StudentId", StudTemp.Id);
                                criteria.Add("Campus", StudTemp.Campus);
                                criteria.Add("Grade", StudTemp.Grade);
                                criteria.Add("IdNo", StudTemp.NewId);
                                criteria.Add("AcademicYear", StudTemp.AcademicYear);
                                Dictionary<long, IList<Assess360>> AssessList = a360srvc.GetAssess360ListWithPagingAndCriteria(0, 50, "", "", criteria);
                                if (AssessList != null && AssessList.FirstOrDefault().Value != null && AssessList.FirstOrDefault().Value.Count > 0)
                                {
                                    AssessList.FirstOrDefault().Value[0].Name = StudTemp.Name;
                                    a360srvc.SaveOrUpdateAssess360(AssessList.FirstOrDefault().Value[0]);
                                }
                                ReportCardService rcs = new ReportCardService();
                                Dictionary<string, object> criteria1 = new Dictionary<string, object>();
                                criteria1.Add("StudentId", StudTemp.Id);
                                criteria1.Add("Campus", StudTemp.Campus);
                                criteria1.Add("Grade", StudTemp.Grade);
                                criteria1.Add("IdNo", StudTemp.NewId);
                                criteria1.Add("AcademicYear", StudTemp.AcademicYear);
                                Dictionary<long, IList<RptCardMYP>> RptCardMYP = rcs.GetRptCardMYPListWithPagingAndCriteria(0, 50, "", "", criteria1);
                                if (RptCardMYP != null && RptCardMYP.FirstOrDefault().Value != null && RptCardMYP.FirstOrDefault().Value.Count > 0)
                                {
                                    RptCardMYP.FirstOrDefault().Value[0].Name = StudTemp.Name;
                                    rcs.SaveOrUpdateMYPReportCard(RptCardMYP.FirstOrDefault().Value[0]);
                                }
                            }
                        }
                    }
                    if (obj.Initial != null)
                    {
                        StudTemp = amc.GetStudentTemplateDetailsById(obj.StudentTemplateId);
                        if (StudTemp != null)
                        {
                            StudTemp.Initial = obj.Initial;
                            amc.CreateOrUpdateStudentTemplateDetails(StudTemp);
                            if (StudTemp.AdmissionStatus == "Registered")
                            {
                                Assess360Service a360srvc = new Assess360Service();
                                Dictionary<string, object> criteria = new Dictionary<string, object>();
                                criteria.Add("StudentId", StudTemp.Id);
                                criteria.Add("Campus", StudTemp.Campus);
                                criteria.Add("Grade", StudTemp.Grade);
                                criteria.Add("IdNo", StudTemp.NewId);
                                criteria.Add("AcademicYear", StudTemp.AcademicYear);
                                Dictionary<long, IList<Assess360>> AssessList = a360srvc.GetAssess360ListWithPagingAndCriteria(0, 50, "", "", criteria);
                                if (AssessList != null && AssessList.FirstOrDefault().Value != null && AssessList.FirstOrDefault().Value.Count > 0)
                                {
                                    AssessList.FirstOrDefault().Value[0].Initial = StudTemp.Initial;
                                    a360srvc.SaveOrUpdateAssess360(AssessList.FirstOrDefault().Value[0]);
                                }
                            }
                            //ReportCardService rcs = new ReportCardService();
                            //Dictionary<string, object> criteria1 = new Dictionary<string, object>();
                            //criteria1.Add("StudentId", StudTemp.Id);
                            //criteria1.Add("Campus", StudTemp.Campus);
                            //criteria1.Add("Grade", StudTemp.Grade);
                            //criteria1.Add("IdNo", StudTemp.NewId);
                            //criteria1.Add("AcademicYear", StudTemp.AcademicYear);
                            //Dictionary<long, IList<RptCardMYP>> RptCardMYP = rcs.GetRptCardMYPListWithPagingAndCriteria(0, 50, "", "", criteria1);
                            //if (RptCardMYP != null && RptCardMYP.FirstOrDefault().Value != null && RptCardMYP.FirstOrDefault().Value.Count > 0)
                            //{
                            //    RptCardMYP.FirstOrDefault().Value[0].Initial = StudTemp.Initial;
                            //    rcs.SaveOrUpdateMYPReportCard(RptCardMYP.FirstOrDefault().Value[0]);
                            //}
                        }
                    }
                    if (obj.SecondLanguage != null && StudTemp.AdmissionStatus == "Registered")
                    {
                        StudTemp = amc.GetStudentTemplateDetailsById(obj.StudentTemplateId);
                        if (StudTemp != null)
                        {
                            StudTemp.SecondLanguage = obj.SecondLanguage;
                            amc.CreateOrUpdateStudentTemplateDetails(StudTemp);
                        }
                    }
                    if (!string.IsNullOrEmpty(obj.BoardingType) || !string.IsNullOrEmpty(obj.VanNo))
                    {
                        if (StudTemp != null)
                        {
                            if (obj.BoardingType != null)
                                StudTemp.BoardingType = obj.BoardingType;
                            if (obj.VanNo != null)
                                StudTemp.VanNo = obj.VanNo;

                            amc.CreateOrUpdateStudentTemplateDetails(StudTemp);
                        }
                    }
                    if (!string.IsNullOrEmpty(obj.FoodPreference))
                    {
                        StudTemp.FoodPreference = obj.FoodPreference;
                        amc.CreateOrUpdateStudentTemplateDetails(StudTemp);
                    }

                    if (obj.General_EmailId != null)
                    {
                        StudTemp = amc.GetStudentTemplateDetailsById(obj.StudentTemplateId);
                        if (StudTemp != null)
                        {
                            StudTemp.EmailId = obj.General_EmailId;
                            amc.CreateOrUpdateStudentTemplateDetails(StudTemp);
                        }
                    }

                    if (obj.Father_Mobile != null)
                    {
                        FamilyDetails = amc.GetFamilyDetailsById(obj.Father_Id);
                        if (FamilyDetails != null)
                        {
                            FamilyDetails.Mobile = obj.Father_Mobile;
                            amc.CreateOrUpdateFamilyDetails(FamilyDetails);
                        }
                    }
                    if (obj.Father_EmailId != null)
                    {
                        FamilyDetails = amc.GetFamilyDetailsById(obj.Father_Id);
                        if (FamilyDetails != null)
                        {
                            FamilyDetails.Email = obj.Father_EmailId;
                            amc.CreateOrUpdateFamilyDetails(FamilyDetails);
                        }
                    }

                    if (obj.Mother_Mobile != null)
                    {
                        FamilyDetails = amc.GetFamilyDetailsById(obj.Mother_Id);
                        FamilyDetails.Mobile = obj.Mother_Mobile;
                        amc.CreateOrUpdateFamilyDetails(FamilyDetails);
                    }
                    if (obj.Mother_EmailId != null)
                    {
                        FamilyDetails = amc.GetFamilyDetailsById(obj.Mother_Id);
                        if (FamilyDetails != null)
                        {
                            FamilyDetails.Email = obj.Mother_EmailId;
                            amc.CreateOrUpdateFamilyDetails(FamilyDetails);
                        }
                    }
                    if (obj.LocationName != null)
                    {
                        StudentLocationMaster studLocations = new StudentLocationMaster();
                        StudTemp = amc.GetStudentTemplateDetailsById(obj.StudentTemplateId);

                        string[] splitLocation = obj.LocationName.Split(',');
                        string LocationOtherDetails = string.Empty;
                        obj.LocationName = splitLocation[0];
                        for (int i = 1; i < splitLocation.Length; i++)
                        {
                            if (!string.IsNullOrEmpty(splitLocation[i]))
                            {
                                string temp = splitLocation[i].Trim();
                                LocationOtherDetails = LocationOtherDetails + temp;
                                if ((i + 1) <= (splitLocation.Length - 1)) { LocationOtherDetails = LocationOtherDetails + ","; }
                            }
                        }
                        if (StudTemp != null)
                        {
                            studLocations = amc.GetStudentLocationMasterByLocationName(splitLocation[0]);
                            if (studLocations == null) { StudTemp.LocationTamilDescription = obj.LocationTamilDescription; }
                            else
                                StudTemp.LocationTamilDescription = studLocations.TamilDescription;
                            StudTemp.LocationName = splitLocation[0];
                            StudTemp.LocationOtherDetails = LocationOtherDetails;
                            amc.CreateOrUpdateStudentTemplateDetails(StudTemp);
                        }
                        StudentLocationMaster studLocation = new StudentLocationMaster();
                        studLocation = amc.GetStudentLocationMasterByLocationName(StudTemp.LocationName);
                        if (studLocation == null)
                        {
                            StudentLocationMaster Location = new StudentLocationMaster();
                            Location.Campus = StudTemp.Campus;
                            if (!string.IsNullOrEmpty(splitLocation[0]))
                                Location.LocationName = splitLocation[0];
                            else
                                Location.LocationName = "";
                            Location.TamilDescription = obj.LocationTamilDescription;
                            Location.CreatedBy = userid;
                            Location.CreatedDate = DateTime.Now;
                            Location.ModifiedBy = userid;
                            Location.ModifiedDate = DateTime.Now;
                            //PLACE TO INCLUDE LANG DEPENDENT CODE
                            //string tamilValue = GetLanguageValue(LocationArray[0]);
                            amc.CreateOrUpdateStudentLocationMaster(Location);
                        }
                    }

                    return Json(null, JsonRequestBehavior.AllowGet);
                }
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }


        public JsonResult CheckLocationMaster(string LocationName)
        {
            try
            {
                TransportBC transSrvc = new TransportBC();
                StudentLocationMaster locationMaster = new StudentLocationMaster();
                AdmissionManagementService ams = new AdmissionManagementService();
                bool rtnMsg = false;
                locationMaster = ams.GetStudentLocationMasterByLocationName(LocationName);
                if (locationMaster != null)
                {
                    rtnMsg = true;
                }
                return Json(rtnMsg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        #endregion

        #region bulk promotion and transfer Details

        public ActionResult BulkPromTransferRequestDetails(long? BulkPromTransferRequestId)
        {
            try
            {
                TIPS.Entities.User Userobj = (TIPS.Entities.User)Session["objUser"];
                if (Userobj == null) { return RedirectToAction("LogOff", "Account"); }
                else
                {
                    if (BulkPromTransferRequestId == 0) { return View(); } else { return View(); }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }

        }

        public ActionResult BulkPromTransferRequestDetailsJqGrid(int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                AdmissionManagementService aMS = new AdmissionManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<long, IList<BulkPromTransferRequestDetails>> BulkPromTransReq = aMS.GetBulkPromTransferRequestDetailsListWithEQsearchCriteria(page - 1, rows, sidx, sord, criteria);
                if (BulkPromTransReq.Count > 0 && BulkPromTransReq.FirstOrDefault().Key > 0 && BulkPromTransReq.FirstOrDefault().Value.Count > 0)
                {
                    long totalrecords = BulkPromTransReq.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,

                        rows = (from items in BulkPromTransReq.First().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[] {
                                        items.BulkPromTransferRequestId.ToString(),
                                         String.Format(@"<a style='color:#034af3;text-decoration:underline' href= '#' onclick = 'getBulkPromTransReq("+"\"" + items.BulkPromTransferRequestId + "\"" + ","+"\"" + items.Status + "\"" + ")' >{0}</a>",items.RequestName),
                                    items.Campus,
                            items.Grade,
                            items.Section
                            
                            }
                                })
                    };
                    return Json(jsondat, JsonRequestBehavior.AllowGet);
                }
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult BulkPromTransfer(long? Id, string results)
        {
            try
            {
                TIPS.Entities.User Userobj = (TIPS.Entities.User)Session["objUser"];
                if (Userobj == null) { return RedirectToAction("LogOff", "Account"); }
                else
                {
                    if (results == "No")
                    {
                        ViewBag.results = "No";
                    }

                    AdmissionManagementService aMSer = new AdmissionManagementService();
                    BulkPromTransfer bPTrans = new BulkPromTransfer();
                    ViewBagFunctions();
                    ViewBag.UserId = Userobj.UserId;
                    ViewBag.Role = Userobj.Role;
                    ViewBag.DateTime = DateTime.Now;
                    if (Id != null && Id > 0)
                    {
                        Session["promotion"] = "";
                        Session["promotionId"] = "";
                        BulkPromTransferRequestDetails bulkPromList = aMSer.GetBulkPromTransferRequestDetailsById(Id ?? 0);
                        bPTrans.BulkPromTransferRequestId = bulkPromList.BulkPromTransferRequestId;
                        bPTrans.RequestName = bulkPromList.RequestName;
                        bPTrans.Campus = bulkPromList.Campus;
                        bPTrans.Grade = bulkPromList.Grade;
                        bPTrans.Section = bulkPromList.Section;
                        ViewBag.IsSaveList = bulkPromList.IsSaveList == true ? "True" : "False";
                        return View(bPTrans);
                    }
                    else
                    {
                        return View(bPTrans);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult BulkPromTransfer(BulkPromTransfer bPTRD, string pagename, string resetsession)
        {
            try
            {
                TIPS.Entities.User Userobj = (TIPS.Entities.User)Session["objUser"];
                if (Userobj == null) { return RedirectToAction("LogOff", "Account"); }
                else
                {
                    AdmissionManagementService aMS = new AdmissionManagementService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    BulkPromTransferRequestDetails bulkPTRD = new BulkPromTransferRequestDetails();
                    bulkPTRD.Campus = bPTRD.Campus;
                    if (Request.Form["Section"] != null)
                    {
                        bulkPTRD.Section = Request.Form["Section"];
                        string[] gradelist = Request.Form["Section"].ToString().Split(',');
                        if (!string.IsNullOrEmpty(bPTRD.Section)) { criteria.Add("Section", gradelist); }
                    }
                    bulkPTRD.Grade = bPTRD.Grade;
                    bulkPTRD.RequestName = bPTRD.RequestName;
                    bulkPTRD.IsSaveList = true;
                    bulkPTRD.Status = "Promotion";
                    long reqId = aMS.SaveOrUpdateBulkPromTransferRequestDetails(bulkPTRD);
                    if (!string.IsNullOrEmpty(bPTRD.Campus)) { criteria.Add("Campus", bPTRD.Campus); }
                    if (!string.IsNullOrEmpty(bPTRD.Grade)) { criteria.Add("Grade", bPTRD.Grade); }
                    Dictionary<long, IList<StudentTemplateView>> StudentTemplate = aMS.GetStudentTemplateViewListWithEQsearchCriteria(0, 99999, string.Empty, string.Empty, criteria);
                    IList<StudentTemplateView> StudentTemplateList = StudentTemplate.FirstOrDefault().Value.ToList();
                    IList<BulkPromTransfer> BulkPromTransferList = new List<BulkPromTransfer>();
                    foreach (StudentTemplateView items in StudentTemplate.FirstOrDefault().Value)
                    {
                        BulkPromTransfer bulkProm = new BulkPromTransfer();
                        bulkProm.PreRegNum = items.PreRegNum;
                        bulkProm.Name = items.Name;
                        bulkProm.RequestName = bPTRD.RequestName;// get from the textbox.
                        bulkProm.ApplicationNo = items.ApplicationNo;
                        bulkProm.FeeStructYear = items.FeeStructYear;
                        bulkProm.AdmissionStatus = items.AdmissionStatus;
                        bulkProm.AcademicYear = items.AcademicYear;
                        bulkProm.Campus = items.Campus;
                        bulkProm.Grade = items.Grade;
                        bulkProm.Section = items.Section;
                        bulkProm.NewId = items.NewId;
                        bulkProm.BulkPromTransferRequestId = reqId;
                        bulkProm.Type = "Promotion";
                        bulkProm.CreatedBy = Userobj.UserId;
                        bulkProm.CreatedDate = DateTime.Now;
                        //bulkProm.ModifiedBy = "modifiedby";
                        //bulkProm.ModifiedDate = DateTime.Now;
                        BulkPromTransferList.Add(bulkProm);
                    }
                    aMS.CreateOrUpdateBulkPromTransferList(BulkPromTransferList);
                    return RedirectToAction("BulkPromTransfer", new { Id = reqId });
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult BulkPromTransferJqGrid(long BulkPromTransferRequestId, string campus, string grade, string section, string admStatus, string feeStructure, string appName, string idNum, string isHosteller, string academicYear, string searchType, int rows, string sidx, string sord, int? page = 1)
        {

            try
            {
                AdmissionManagementService aMS = new AdmissionManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(grade)) { criteria.Add("Grade", grade); }
                if (!string.IsNullOrEmpty(section))
                {
                    string[] sectionVal = section.Split(',');
                    criteria.Add("Section", sectionVal);
                }
                if (!string.IsNullOrEmpty(admStatus)) { criteria.Add("AdmissionStatus", admStatus); }
                if (!string.IsNullOrEmpty(feeStructure)) { criteria.Add("FeeStructYear", feeStructure); }
                if (!string.IsNullOrEmpty(appName)) { criteria.Add("Name", appName); }
                if (!string.IsNullOrEmpty(idNum)) { criteria.Add("NewId", idNum); }
                if (!string.IsNullOrEmpty(isHosteller)) { criteria.Add("IsHosteller", isHosteller); }
                if (!string.IsNullOrEmpty(academicYear)) { criteria.Add("AcademicYear", academicYear); }
                if (BulkPromTransferRequestId != null && BulkPromTransferRequestId > 0)
                {
                    criteria.Add("BulkPromTransferRequestId", BulkPromTransferRequestId);
                    if (!string.IsNullOrEmpty(campus)) { criteria.Add("Campus", campus); }
                    Dictionary<long, IList<BulkPromTransfer>> BulkPromTransferList = aMS.GetBulkPromTransferListWithEQsearchCriteria(0, 99999, string.Empty, string.Empty, criteria);
                    if (BulkPromTransferList.Count > 0 && BulkPromTransferList.FirstOrDefault().Key > 0 && BulkPromTransferList.FirstOrDefault().Value.Count > 0)
                    {
                        long totalrecords = BulkPromTransferList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,

                            rows = (from items in BulkPromTransferList.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                                        items.BulkPromTransferRequestId.ToString(),
                                    items.ApplicationNo,
                              items.PreRegNum.ToString(),
                            items.Name,
                            items.Grade,
                            items.Section,
                            items.Campus,
                            items.FeeStructYear,
                            items.AdmissionStatus,
                            items.NewId,
                            items.AcademicYear,
                            items.CreatedDate.Value.ToString("dd/MM/yyyy"),
                            items.IsPromotionOrTransfer.ToString()
                            }
                                    })
                        };
                        return Json(jsondat, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    var usrcmp = Session["UserCampus"] as IEnumerable<string>;
                    string[] str = new string[usrcmp.ToList().Count];
                    int i = 0;
                    if (!string.IsNullOrEmpty(campus))
                    { criteria.Add("Campus", campus); }
                    else
                    {
                        foreach (var var in usrcmp)
                        {
                            str[i] = var;
                            i++;
                        }
                        criteria.Add("Campus", str);
                    }

                    Dictionary<long, IList<StudentTemplateView>> StudentTemplate = aMS.GetStudentTemplateViewListWithEQsearchCriteria(null, rows, string.Empty, string.Empty, criteria);
                    if (StudentTemplate.Count > 0 && StudentTemplate.FirstOrDefault().Key > 0 && StudentTemplate.FirstOrDefault().Value.Count > 0)
                    {
                        long totalrecords = StudentTemplate.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,

                            rows = (from items in StudentTemplate.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                                        items.Id.ToString(),
                                    items.ApplicationNo,
                              items.PreRegNum.ToString(),
                            items.Name,
                            items.Grade,
                            items.Section,
                            items.Campus,
                            items.FeeStructYear,
                            items.AdmissionStatus,
                            items.NewId,
                            items.AcademicYear,
                            items.CreatedDate,
                            items.IsPromotionOrTransfer.ToString()
                            }
                                    })
                        };
                        return Json(jsondat, JsonRequestBehavior.AllowGet);
                    }
                }
                var emptyJsonVal = new { rows = (new { cell = new string[] { } }) };
                return Json(emptyJsonVal, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public void ViewBagFunctions()
        {
            MastersService ms = new MastersService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            Dictionary<long, IList<CampusMaster>> CampusMaster = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            Dictionary<long, IList<GradeMaster>> GradeMaster = ms.GetGradeMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            Dictionary<long, IList<SectionMaster>> SectionMaster = ms.GetSectionMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            Dictionary<long, IList<FeeStructureYearMaster>> FeeStructyrMaster = ms.GetFeeStructureYearMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            Dictionary<long, IList<AcademicyrMaster>> AcademicyrMaster = ms.GetAcademicyrMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            ViewBag.campusddl = CampusMaster.First().Value;
            ViewBag.gradeddl = GradeMaster.First().Value;
            ViewBag.sectionddl = SectionMaster.First().Value;
            ViewBag.feestructddl = FeeStructyrMaster.First().Value;
            ViewBag.acadddl = AcademicyrMaster.First().Value;
        }

        public bool CheckPromotionDetails(string PreRegNo, string campus, string grade, string check)
        {
            try
            {
                var cmps = campus.Split(',');
                if (check == "yes")
                {
                    var grd = grade.Split(',');// To check whether same grade students are selected or not. 
                    if ((grd.Distinct().Count() > 1) || (cmps.Distinct().Count() > 1)) { return false; }
                    else
                    {
                        Session["promgrd"] = grd[0];// Current grade of student to be promoted
                        return true;
                    }
                }
                else
                {
                    Session["promotionno"] = PreRegNo;
                    Session["Promotioncamp"] = cmps[0];
                    return false;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult NewPromotion(long? BulkPromTransferRequestId)
        {
            try
            {
                TIPS.Entities.User Userobj = (TIPS.Entities.User)Session["objUser"];
                if (Userobj == null) { return RedirectToAction("LogOff", "Account"); }
                else
                {
                    ViewBag.BulkPromTransferRequestId = BulkPromTransferRequestId;
                    Session["promotion"] = "";
                    Session["promotionId"] = "";
                    MastersService ms = new MastersService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<long, IList<CampusMaster>> CampusMaster = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                    ViewBag.campusddl = CampusMaster.First().Value;

                    Dictionary<long, IList<SectionMaster>> SectionMaster = ms.GetSectionMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                    ViewBag.sectionddl = SectionMaster.First().Value;
                    Dictionary<string, object> criteria1 = new Dictionary<string, object>();
                    var g1 = "";
                    int[] values = new int[2];
                    if (Session["Promotioncamp"].ToString() == "IB KG")
                    {
                        criteria1.Add("Code", g1);
                        values = new int[0];
                    }
                    else if (Session["Promotioncamp"].ToString() == "IB MAIN")
                    {
                        g1 = "3";
                        criteria1.Add("Code1", g1);
                        values = new int[0];
                    }
                    else if (Session["Promotioncamp"].ToString() == "ERNAKULAM")
                    {
                        g1 = "";
                        criteria1.Add("Code", g1);
                        values = new int[0];
                    }
                    else if (Session["Promotioncamp"].ToString() == "TIPS SARAN")   // Get using IN condition
                    {
                        criteria1.Clear();
                        values[0] = 11;                     // get grade with column GRAD
                        values[1] = 12;
                    }
                    else
                    {
                        g1 = "3";
                        criteria1.Add("Code1", g1);
                        values = new int[0];
                    }
                    Dictionary<long, IList<GradeMaster>> GradeMaster = ms.GetGradeMasterListWithPagingAndCriteriaWithIn(0, 50, "", "", "grad", values, criteria1, null);
                    ViewBag.gradeddl = GradeMaster.First().Value;
                    StudentTemplate st = new StudentTemplate();
                    switch (Session["promgrd"].ToString())
                    {
                        case "PreKG":
                            st.Grade = "LKG";
                            if (Session["Promotioncamp"].ToString() == "IB MAIN") { st.Campus = "IB KG"; }
                            else { st.Campus = "CHENNAI CITY"; }
                            break;
                        case "PREKG":
                            st.Grade = "LKG";
                            if (Session["Promotioncamp"].ToString() == "IB MAIN") { st.Campus = "IB KG"; }
                            else { st.Campus = "CHENNAI CITY"; }
                            break;
                        case "LKG":
                            st.Grade = "UKG";
                            if (Session["Promotioncamp"].ToString() == "IB MAIN") { st.Campus = "IB KG"; }
                            else { st.Campus = "CHENNAI CITY"; }
                            break;
                        case "UKG":
                            st.Grade = "I";
                            if (Session["Promotioncamp"].ToString() == "IB MAIN") { st.Campus = "IB MAIN"; }
                            else { st.Campus = "CHENNAI CITY"; }
                            break;
                        case "I":
                            st.Grade = "II";
                            if (Session["Promotioncamp"].ToString() == "IB MAIN") { st.Campus = "IB MAIN"; }
                            else { st.Campus = "CHENNAI CITY"; }
                            break;
                        case "II":
                            st.Grade = "III";
                            if (Session["Promotioncamp"].ToString() == "IB MAIN") { st.Campus = "IB MAIN"; }
                            else { st.Campus = "CHENNAI MAIN"; }
                            break;
                        case "III":
                            st.Grade = "IV";
                            if (Session["Promotioncamp"].ToString() == "IB MAIN") { st.Campus = "IB MAIN"; }
                            else { st.Campus = "CHENNAI MAIN"; }
                            break;
                        case "IV":
                            st.Grade = "V";
                            if (Session["Promotioncamp"].ToString() == "IB MAIN") { st.Campus = "IB MAIN"; }
                            else { st.Campus = "CHENNAI MAIN"; }
                            break;
                        case "V":
                            st.Grade = "VI";
                            if (Session["Promotioncamp"].ToString() == "IB MAIN") { st.Campus = "IB MAIN"; }
                            else { st.Campus = "CHENNAI MAIN"; }
                            break;
                        case "VI":
                            st.Grade = "VII";
                            if (Session["Promotioncamp"].ToString() == "IB MAIN") { st.Campus = "IB MAIN"; }
                            else { st.Campus = "CHENNAI MAIN"; }
                            break;
                        case "VII":
                            st.Grade = "VIII";
                            if (Session["Promotioncamp"].ToString() == "IB MAIN") { st.Campus = "IB MAIN"; }
                            else { st.Campus = "CHENNAI MAIN"; }
                            break;
                        case "VIII":
                            st.Grade = "IX";
                            if (Session["Promotioncamp"].ToString() == "IB MAIN") { st.Campus = "IB MAIN"; }
                            else { st.Campus = "CHENNAI MAIN"; }
                            break;
                        case "IX":
                            st.Grade = "X";
                            if (Session["Promotioncamp"].ToString() == "IB MAIN") { st.Campus = "IB MAIN"; }
                            else { st.Campus = "CHENNAI MAIN"; }
                            st.Campus = "IB MAIN";
                            break;
                        case "X":
                            st.Grade = "XI";
                            if (Session["Promotioncamp"].ToString() == "IB MAIN") { st.Campus = "IB MAIN"; }
                            else { st.Campus = "CHENNAI MAIN"; }
                            st.Campus = "IB MAIN";
                            break;
                        case "XI":
                            st.Grade = "XII";
                            if (Session["Promotioncamp"].ToString() == "IB MAIN") { st.Campus = "IB MAIN"; }
                            else if (Session["Promotioncamp"].ToString() == "CHENNAI MAIN") { st.Campus = "CHENNAI MAIN"; }
                            else { st.Campus = "TIPS SARAN"; }
                            break;
                    }
                    return View(st);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }


        [HttpPost]
        public ActionResult NewPromotion(StudentTemplate st)
        {
            try
            {
                TIPS.Entities.User Userobj = (TIPS.Entities.User)Session["objUser"];
                if (Userobj == null) { return RedirectToAction("LogOff", "Account"); }
                else
                {
                    var prereg = Session["promotionno"].ToString().Split(',');
                    long Bulk_RequestId = st.BulkPromTransferRequestId;
                    foreach (var PreRegNum in prereg)
                    {
                        Dictionary<string, object> criteria = new Dictionary<string, object>();
                        criteria.Add("PreRegNum", Convert.ToInt64(PreRegNum));
                        AdmissionManagementService ams = new AdmissionManagementService();
                        Dictionary<long, IList<StudentTemplate>> studenttemplate = ams.GetStudentDetailsListWithEQsearchCriteria(0, 50, string.Empty, string.Empty, criteria);
                        criteria.Add("BulkPromTransferRequestId", Bulk_RequestId);
                        BulkPromTransfer PromotionVal = ams.GetBulkPromTransferByIds(Convert.ToInt64(PreRegNum), Bulk_RequestId);
                        if (st.Grade == studenttemplate.First().Value[0].Grade)
                        {
                            Session["promotion"] = "no";
                            return RedirectToAction("BulkPromTransfer", new { results = "No" });
                        }
                        var acdyr = studenttemplate.First().Value[0].AcademicYear.ToString().Split('-');   // To increment academic year while promoting
                        int acd1 = Convert.ToInt32(acdyr[0]);
                        acd1 = acd1 + 1;
                        int acd2 = Convert.ToInt32(acdyr[1]);
                        if (acd2 == DateTime.Now.Year)                  // if current academicyear of student is equal to current year
                        {
                            acd2 = acd2 + 1;
                            studenttemplate.First().Value[0].AcademicYear = acd1.ToString() + "-" + acd2.ToString();
                            studenttemplate.First().Value[0].Grade = st.Grade;
                            studenttemplate.First().Value[0].Section = st.Section;
                            st = studenttemplate.First().Value[0];
                            ams.CreateOrUpdateStudentDetails(st);
                            Session["promotionId"] = st.Grade + " " + st.Section;
                            if (PromotionVal != null)
                            {
                                PromotionVal.AfterCampus = st.Campus;
                                PromotionVal.AfterGrade = st.Grade;
                                PromotionVal.AfterSection = st.Section;
                                PromotionVal.IsPromotionOrTransfer = true;
                                PromotionVal.AfterAcademicYear = acd1.ToString() + "-" + acd2.ToString();
                                PromotionVal.ModifiedBy = Userobj.UserId;
                                PromotionVal.ModifiedDate = DateTime.Now;
                                ams.SaveOrUpdateBulkPromTransfer(PromotionVal);
                            }
                        }
                        else
                        {
                            return RedirectToAction("BulkPromTransfer", new { Id = Bulk_RequestId, results = "No" });
                        }
                    }
                    //Session["promotion"] = "yes";
                    return RedirectToAction("BulkPromTransfer", new { Id = Bulk_RequestId });
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }

        }

        public ActionResult BulkTransfer(long? Id, string IsTrurOfFalse)
        {
            try
            {
                TIPS.Entities.User Userobj = (TIPS.Entities.User)Session["objUser"];
                if (Userobj == null) { return RedirectToAction("LogOff", "Account"); }
                else
                {
                    AdmissionManagementService aMSer = new AdmissionManagementService();
                    BulkPromTransfer bPTrans = new BulkPromTransfer();
                    ViewBagFunctions();
                    ViewBag.UserId = Userobj.UserId;
                    ViewBag.Role = Userobj.Role;
                    ViewBag.DateTime = DateTime.Now;
                    ViewBag.TransferOrNot = IsTrurOfFalse;
                    if (Id != null && Id > 0)
                    {
                        BulkPromTransferRequestDetails bulkPromList = aMSer.GetBulkPromTransferRequestDetailsById(Id ?? 0);
                        bPTrans.BulkPromTransferRequestId = bulkPromList.BulkPromTransferRequestId;
                        bPTrans.RequestName = bulkPromList.RequestName;
                        bPTrans.Campus = bulkPromList.Campus;
                        bPTrans.Grade = bulkPromList.Grade;
                        bPTrans.Section = bulkPromList.Section;
                        ViewBag.IsSaveList = bulkPromList.IsSaveList == true ? "True" : "False";
                        return View(bPTrans);
                    }
                    else
                    {
                        return View(bPTrans);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult BulkTransfer(BulkPromTransfer bPTRD)
        {
            try
            {
                TIPS.Entities.User Userobj = (TIPS.Entities.User)Session["objUser"];
                if (Userobj == null) { return RedirectToAction("LogOff", "Account"); }
                else
                {
                    long reqId = 0;
                    AdmissionManagementService aMS = new AdmissionManagementService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    BulkPromTransferRequestDetails bulkPTRD = new BulkPromTransferRequestDetails();
                    bulkPTRD.Campus = bPTRD.Campus;
                    if (Request.Form["Section"] != null)
                    {
                        bulkPTRD.Section = Request.Form["Section"];
                        string[] gradelist = Request.Form["Section"].ToString().Split(',');
                        if (!string.IsNullOrEmpty(bPTRD.Section)) { criteria.Add("Section", gradelist); }
                    }
                    bulkPTRD.Grade = bPTRD.Grade;
                    bulkPTRD.Section = bPTRD.Section;
                    bulkPTRD.RequestName = bPTRD.RequestName;
                    bulkPTRD.IsSaveList = true;
                    bulkPTRD.Status = "Transfer";
                    if (bPTRD.BulkPromTransferRequestId == 0)
                    { reqId = aMS.SaveOrUpdateBulkPromTransferRequestDetails(bulkPTRD); }
                    else { reqId = bPTRD.BulkPromTransferRequestId; }

                    if (!string.IsNullOrEmpty(bPTRD.Campus)) { criteria.Add("Campus", bPTRD.Campus); }
                    if (!string.IsNullOrEmpty(bPTRD.Grade)) { criteria.Add("Grade", bPTRD.Grade); }
                    //if (!string.IsNullOrEmpty(bPTRD.Section)) { criteria.Add("Section", bPTRD.Section); }
                    if (!string.IsNullOrEmpty(bPTRD.Name)) { criteria.Add("Name", bPTRD.Name); }
                    if (!string.IsNullOrEmpty(bPTRD.AdmissionStatus)) { criteria.Add("AdmissionStatus", bPTRD.AdmissionStatus); }
                    if (!string.IsNullOrEmpty(bPTRD.FeeStructYear)) { criteria.Add("FeeStructYear", bPTRD.FeeStructYear); }
                    if (!string.IsNullOrEmpty(bPTRD.NewId)) { criteria.Add("NewId", bPTRD.NewId); }
                    // if (bPTRD.IsHosteller != null) { criteria.Add("IsHosteller", bPTRD.IsHosteller); }
                    if (!string.IsNullOrEmpty(bPTRD.AcademicYear)) { criteria.Add("AcademicYear", bPTRD.AcademicYear); }
                    Dictionary<long, IList<StudentTemplateView>> StudentTemplate = aMS.GetStudentTemplateViewListWithEQsearchCriteria(0, 99999, string.Empty, string.Empty, criteria);
                    IList<StudentTemplateView> StudentTemplateList = StudentTemplate.FirstOrDefault().Value.ToList();
                    IList<BulkPromTransfer> BulkPromTransferList = new List<BulkPromTransfer>();
                    foreach (StudentTemplateView items in StudentTemplate.FirstOrDefault().Value)
                    {
                        BulkPromTransfer bulkProm = new BulkPromTransfer();
                        bulkProm.PreRegNum = items.PreRegNum;
                        bulkProm.Name = items.Name;
                        bulkProm.RequestName = bPTRD.RequestName;// get from the textbox.
                        bulkProm.ApplicationNo = items.ApplicationNo;
                        bulkProm.FeeStructYear = items.FeeStructYear;
                        bulkProm.AdmissionStatus = items.AdmissionStatus;
                        bulkProm.AcademicYear = items.AcademicYear;
                        bulkProm.Campus = items.Campus;
                        bulkProm.Grade = items.Grade;
                        bulkProm.Section = items.Section;
                        bulkProm.NewId = items.NewId;
                        bulkProm.BulkPromTransferRequestId = reqId;
                        bulkProm.Type = "Transfer";
                        bulkProm.CreatedBy = Userobj.UserId;
                        bulkProm.CreatedDate = DateTime.Now;
                        BulkPromTransferList.Add(bulkProm);
                    }
                    aMS.CreateOrUpdateBulkPromTransferList(BulkPromTransferList);
                    return RedirectToAction("BulkTransfer", new { Id = reqId });
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult NewTransfer(long BulkPromTransferRequestId, string PreRegNum)
        {
            try
            {
                TIPS.Entities.User Userobj = (TIPS.Entities.User)Session["objUser"];
                if (Userobj == null) { return RedirectToAction("LogOff", "Account"); }
                else
                {
                    ViewBag.BulkPromTransferRequestId = BulkPromTransferRequestId;
                    Session["transfered"] = "";
                    Session["transferedId"] = "";

                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    // string campus = "";
                    //if (Session["UserId"].ToString() == "CHE-GRL")
                    //{
                    //    campus = "CHENNAI";
                    //    criteria.Add("Name", campus);
                    //}
                    //if (Session["UserId"].ToString() == "TIR-GRL")
                    //{
                    //    campus = "TIRUPUR";
                    //    criteria.Add("Name", campus);
                    //}
                    //if (Session["UserId"].ToString() == "ERN-GRL")
                    //{
                    //    campus = "ERNAKULAM";
                    //    criteria.Add("Name", campus);
                    //}
                    //if (Session["UserId"].ToString() == "KAR-GRL")
                    //{
                    //    campus = "KARUR";
                    //    criteria.Add("Name", campus);
                    //}
                    //if (Session["UserId"].ToString() == "GRL-ADMS")
                    //{
                    //    criteria.Clear();
                    //}
                    //if (Session["UserId"].ToString() == "Uma")
                    //{
                    //    campus = "TIPS ERODE";
                    //    criteria.Add("Name", campus);
                    //}
                    if (Session["UserId"].ToString() == "GRL-ADMS")
                    {
                        criteria.Clear();
                    }
                    else
                    {
                        if (Userobj != null)
                        {
                            criteria.Add("Name", Userobj.Campus);
                        }
                    }
                    MastersService ms = new MastersService();
                    Dictionary<long, IList<CampusMaster>> CampusMaster = ms.GetCampusMasterNEqListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);

                    ViewBag.campusddl = CampusMaster.First().Value;

                    criteria.Clear();
                    Dictionary<long, IList<GradeMaster>> GradeMaster = ms.GetGradeMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                    ViewBag.gradeddl = GradeMaster.First().Value;
                    Dictionary<long, IList<SectionMaster>> SectionMaster = ms.GetSectionMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                    ViewBag.sectionddl = SectionMaster.First().Value;
                    Dictionary<long, IList<AcademicyrMaster>> AcademicMaster = ms.GetAcademicyrMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                    ViewBag.acadddl = AcademicMaster.First().Value;

                    StudentTemplate st = new StudentTemplate();
                    st.Grade = Session["promotngrd"].ToString();
                    st.BulkPreRegNum = PreRegNum;
                    return View(st);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult NewTransferJqGrid(long BulkPromTransferRequestId, string campus, string grade, string section, string admStatus, string feeStructure, string appName, string idNum, string isHosteller, string academicYear, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                TIPS.Entities.User Userobj = (TIPS.Entities.User)Session["objUser"];
                if (Userobj == null) { return RedirectToAction("LogOff", "Account"); }
                else
                {
                    AdmissionManagementService aMS = new AdmissionManagementService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    // if (BulkPromTransferRequestId > 0 && BulkPromTransferRequestId != null) { criteria.Add("BulkPromTransferRequestId", BulkPromTransferRequestId); }
                    // if (!string.IsNullOrEmpty(campus)) { criteria.Add("Campus", campus); }
                    if (!string.IsNullOrEmpty(grade)) { criteria.Add("Grade", grade); }
                    if (!string.IsNullOrEmpty(section))
                    {
                        string[] sectionVal = section.Split(',');
                        criteria.Add("Section", sectionVal);
                    }
                    if (!string.IsNullOrEmpty(appName)) { criteria.Add("Name", appName); }
                    if (!string.IsNullOrEmpty(admStatus)) { criteria.Add("AdmissionStatus", admStatus); }
                    if (!string.IsNullOrEmpty(feeStructure)) { criteria.Add("FeeStructYear", feeStructure); }
                    if (!string.IsNullOrEmpty(idNum)) { criteria.Add("NewId", idNum); }
                    if (!string.IsNullOrEmpty(isHosteller)) { criteria.Add("IsHosteller", isHosteller); }
                    if (!string.IsNullOrEmpty(academicYear)) { criteria.Add("AcademicYear", academicYear); }
                    if (BulkPromTransferRequestId != null && BulkPromTransferRequestId > 0)
                    {
                        criteria.Add("BulkPromTransferRequestId", BulkPromTransferRequestId);
                        if (!string.IsNullOrEmpty(campus)) { criteria.Add("Campus", campus); }
                        Dictionary<long, IList<BulkPromTransfer>> BulkPromTransferList = aMS.GetBulkPromTransferListWithEQsearchCriteria(0, 99999, string.Empty, string.Empty, criteria);
                        if (BulkPromTransferList.Count > 0 && BulkPromTransferList.FirstOrDefault().Key > 0 && BulkPromTransferList.FirstOrDefault().Value.Count > 0)
                        {
                            long totalrecords = BulkPromTransferList.First().Key;
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var jsondat = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalrecords,

                                rows = (from items in BulkPromTransferList.First().Value
                                        select new
                                        {
                                            i = 2,
                                            cell = new string[] {
                                        items.BulkPromTransferRequestId.ToString(),
                                    items.ApplicationNo,
                              items.PreRegNum.ToString(),
                            items.Name,
                            items.Grade,
                            items.Section,
                            items.Campus,
                            items.FeeStructYear,
                            items.AdmissionStatus,
                            items.NewId,
                            items.AcademicYear,
                            items.CreatedDate.Value.ToString("dd/MM/yyyy"),
                            items.IsPromotionOrTransfer.ToString()
                            }
                                        })
                            };
                            return Json(jsondat, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        var usrcmp = Session["UserCampus"] as IEnumerable<string>;
                        string[] str = new string[usrcmp.ToList().Count];
                        int i = 0;
                        if (!string.IsNullOrEmpty(campus))
                        { criteria.Add("Campus", campus); }
                        else
                        {
                            foreach (var var in usrcmp)
                            {
                                str[i] = var;
                                i++;
                            }
                            criteria.Add("Campus", str);
                        }
                        Dictionary<long, IList<StudentTemplateView>> StudentTemplate = aMS.GetStudentTemplateViewListWithEQsearchCriteria(null, rows, string.Empty, string.Empty, criteria);
                        if (StudentTemplate.Count > 0 && StudentTemplate.FirstOrDefault().Key > 0 && StudentTemplate.FirstOrDefault().Value.Count > 0)
                        {
                            long totalrecords = StudentTemplate.First().Key;
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var jsondat = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalrecords,

                                rows = (from items in StudentTemplate.First().Value
                                        select new
                                        {
                                            i = 2,
                                            cell = new string[] {
                                        items.Id.ToString(),
                                    items.ApplicationNo,
                              items.PreRegNum.ToString(),
                            items.Name,
                            items.Grade,
                            items.Section,
                            items.Campus,
                            items.FeeStructYear,
                            items.AdmissionStatus,
                            items.NewId,
                            items.AcademicYear,
                            items.CreatedDate,
                            items.IsPromotionOrTransfer.ToString()
                            }
                                        })
                            };
                            return Json(jsondat, JsonRequestBehavior.AllowGet);
                        }
                    }
                    var emptyJsonVal = new { rows = (new { cell = new string[] { } }) };
                    return Json(emptyJsonVal, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult NewTransfer(StudentTemplate st)
        {
            try
            {
                var bulkPreRegNum = st.BulkPreRegNum.Split(',');
                long requestId = st.BulkPromTransferRequestId;
                TIPS.Entities.User Userobj = (TIPS.Entities.User)Session["objUser"];
                if (Userobj == null) { return RedirectToAction("LogOff", "Account"); }
                else
                {
                    foreach (var item in bulkPreRegNum)
                    {
                        TransferDetails td = new TransferDetails();
                        Dictionary<string, object> criteria = new Dictionary<string, object>();
                        // criteria.Add("PreRegNum", Convert.ToInt64(st.PreRegNum));
                        criteria.Add("PreRegNum", Convert.ToInt64(item));
                        AdmissionManagementService ams = new AdmissionManagementService();
                        Dictionary<long, IList<StudentTemplate>> studenttemplate = ams.GetStudentDetailsListWithEQsearchCriteria(0, 99999, string.Empty, string.Empty, criteria);
                        criteria.Add("BulkPromTransferRequestId", requestId);
                        BulkPromTransfer PromotionVal = ams.GetBulkPromTransferByIds(Convert.ToInt64(item), requestId);
                        if (string.Equals(st.Campus, studenttemplate.First().Value[0].Campus, StringComparison.CurrentCultureIgnoreCase))
                        {
                            Session["transfered"] = "no";
                            return RedirectToAction("BulkTransfer", new { Id = requestId, IsTrurOfFalse = "True" });
                            // return RedirectToAction("StudentManagement", new { pagename = "transfer" });
                        }
                        else
                        {
                            td.BeforeCampus = studenttemplate.First().Value[0].Campus;
                            td.BeforeGrade = studenttemplate.First().Value[0].Grade;
                            td.BeforeSection = studenttemplate.First().Value[0].Section;
                            td.BeforeId = studenttemplate.First().Value[0].NewId;
                            td.AcademicYear = studenttemplate.First().Value[0].AcademicYear;
                            td.ApplicationNo = studenttemplate.First().Value[0].ApplicationNo;
                            td.FeeStructYear = studenttemplate.First().Value[0].FeeStructYear;
                            td.Gender = studenttemplate.First().Value[0].Gender;
                            td.TransferedDate = DateTime.Now.ToString();
                            td.Name = studenttemplate.First().Value[0].Name;
                            td.Type = "Transfer";
                            studenttemplate.First().Value[0].Campus = st.Campus;
                            studenttemplate.First().Value[0].Grade = st.Grade;
                            studenttemplate.First().Value[0].Section = st.Section;


                            Dictionary<string, object> criteria2 = new Dictionary<string, object>();
                            criteria2.Add("Grade", st.Grade);
                            criteria2.Add("FeeStructYear", studenttemplate.First().Value[0].FeeStructYear);
                            criteria2.Add("AdmissionStatus", "Registered");
                            Dictionary<long, IList<StudentTemplate>> StudentIdcnt = ams.GetStudentDetailsListWithEQsearchCriteria(0, 0, string.Empty, string.Empty, criteria2);


                            Session["transfered"] = "yes";
                            Session["transferedId"] = studenttemplate.First().Value[0].NewId;
                            Session["transferedName"] = studenttemplate.First().Value[0].Name;

                            td.PreRegNum = st.PreRegNum;
                            td.AfterCampus = st.Campus;
                            td.AfterGrade = st.Grade;
                            td.AfterSection = st.Section;
                            td.AfterId = studenttemplate.First().Value[0].NewId;

                            ams.CreateOrUpdateTransferDetails(td);
                            st = studenttemplate.First().Value[0];
                            st.NewId = StudentIdNumberLogic(st.Campus, st.FeeStructYear, st.Grade, st.AcademicYear);
                            st.AdmissionStatus = "Registered";   // if student is transfered and then discontinued and then admitted in other campus
                            ams.CreateOrUpdateStudentDetails(st);

                            if (PromotionVal != null)
                            {
                                PromotionVal.AfterCampus = st.Campus;
                                PromotionVal.AfterGrade = st.Grade;
                                PromotionVal.AfterSection = st.Section;
                                PromotionVal.IsPromotionOrTransfer = true;
                                PromotionVal.ModifiedBy = Userobj.UserId;
                                PromotionVal.AfterNewId = studenttemplate.First().Value[0].NewId;
                                PromotionVal.ModifiedDate = DateTime.Now;
                                ams.SaveOrUpdateBulkPromTransfer(PromotionVal);
                            }

                            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                            IList<CampusEmailId> campusemaildet = GetEmailIdByCampus(st.Campus, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
                            string[] dob = st.DOB.ToString().Split('/');
                            string sex = "";
                            if (st.Gender == "Male") { sex = "his"; }
                            else if (st.Gender == "Female") { sex = "her"; }

                            // ---------------------------- creating Parent userid -----------------------------------------
                            //    string[] dob = st.DOB.ToString().Split('/');
                            TIPS.Entities.User user = new TIPS.Entities.User();
                            user.UserId = "P" + st.NewId;
                            user.Password = dob[0] + dob[1] + dob[2];
                            user.Campus = st.Campus;
                            user.UserType = "Parent";
                            user.CreatedDate = DateTime.Now;
                            user.ModifiedDate = DateTime.Now;
                            TIPS.Service.UserService us = new UserService();
                            PassworAuth PA = new PassworAuth();
                            //encode and save the password
                            user.Password = PA.base64Encode(user.Password);
                            user.IsActive = true;
                            TIPS.Entities.User userexists = us.GetUserByUserId(user.UserId);
                            if (userexists == null)    // to check if user already exists
                            {
                                us.CreateOrUpdateUser(user);
                            }
                            mail.To.Add(campusemaildet.First().EmailId);
                            mail.Subject = "Welcome to TIPS - Student Registration Successfull"; // st.Subject; 
                            mail.Body = "Dear Parent, <br/><br/>We are happy to add you in our TIPS family and your admission is approved. “" + st.Name + "” is admitted in Grade “" + st.Grade + "” section “" + st.Section + "” of our “" + st.Campus + "” campus and " + sex + " Id number will be “" + st.NewId + "”.<br/><br/>Your parent portal id is “" + "P" + st.NewId + "” and password will be “" + dob[0] + dob[1] + dob[2] + "” i.e. the DOB of the student as given in the records. You can access the same through http://portal.tipsglobal.org.<br/><br/> For any queries, mail us at “" + campusemaildet.First().EmailId.ToString() + "” <br/><br/><br/>Regards <br/><br/><br/><br/> TIPS Management team.";
                            mail.IsBodyHtml = true;
                            SmtpClient smtp = new SmtpClient("localhost", 25);
                            smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address  
                            //Or your Smtp Email ID and Password  
                            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                            smtp.EnableSsl = true;
                            if ((st.EmailId != "") && (!string.IsNullOrEmpty(st.EmailId)) && (st.EmailId != null))
                            {
                                if (Regex.IsMatch(st.EmailId,
            @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$",
            RegexOptions.IgnoreCase))
                                {
                                    mail.Bcc.Add(st.EmailId);
                                }
                            }
                            mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
                            smtp.Credentials = new System.Net.NetworkCredential
                           (campusemaildet.First().EmailId.ToString(), campusemaildet.First().Password.ToString());
                            // mail.To.Add("tipscmsupp0rthyderabad247@gmail.com");
                            if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                            {
                                // smtp.Send(mail);
                                try
                                {
                                    new Task(() => { smtp.Send(mail); }).Start();
                                }
                                catch (Exception ex)
                                {
                                    if (ex.Message.Contains("quota"))
                                    {
                                        mail.From = new MailAddress(campusemaildet.First().AlternateEmailId.ToString());
                                        smtp.Credentials = new System.Net.NetworkCredential
                           (campusemaildet.First().AlternateEmailId.ToString(), campusemaildet.First().AlternateEmailIdPassword.ToString());
                                        if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                                        {
                                            new Task(() => { smtp.Send(mail); }).Start();
                                        }
                                    }
                                    else
                                    {
                                        mail.From = new MailAddress(campusemaildet.First().AlternateEmailId.ToString());
                                        smtp.Credentials = new System.Net.NetworkCredential
                           (campusemaildet.First().AlternateEmailId.ToString(), campusemaildet.First().AlternateEmailIdPassword.ToString());
                                        if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                                        {
                                            new Task(() => { smtp.Send(mail); }).Start();
                                        }
                                    }
                                }
                                mail.Bcc.Clear();
                            }
                            //  return RedirectToAction("BulkTransfer", new { Id = requestId, IsTrurOfFalse = "True" });
                        }
                    }
                }
                return RedirectToAction("BulkTransfer", new { Id = requestId });
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult GetApplicantNames(string term, string campus, string grade, string section)
        {
            try
            {
                AdmissionManagementService aMS = new AdmissionManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(campus)) { criteria.Add("Campus", campus); }
                if (!string.IsNullOrEmpty(grade)) { criteria.Add("Grade", grade); }
                if (!string.IsNullOrEmpty(section)) { criteria.Add("Section", section); }
                if (!string.IsNullOrEmpty(term)) { criteria.Add("Name", term); }
                //change the method to not to use the count since it is not being used here "REVISIT"
                Dictionary<long, IList<StudentTemplate>> studenttemplate = aMS.GetStudentDetailsListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);

                //var NameList = (
                //             from items in studenttemplate.First().Value
                //             select new
                //             {
                //                 Text = items.Name,
                //                 Value = items.PreRegNum,
                //             }).Distinct().ToList();

                //return Json(NameList, JsonRequestBehavior.AllowGet);
                var NameList = (from u in studenttemplate.First().Value
                                where u.Name != null
                                select u.Name).Distinct().ToList();
                return Json(NameList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CustomAccountPolicy");
                throw ex;
            }
        }

        #endregion

        public string GetBodyofMail()
        {
            string MessageBody = System.IO.File.ReadAllText(HttpContext.Server.MapPath("~/Views/Shared/AdmissionEmailBody.html"));
            return MessageBody;
        }
        #region "Second Language Master added by Gobi"
        public ActionResult getSecondLanguageMaster()
        {
            try
            {
                AdmissionManagementService AMS = new AdmissionManagementService();
                string sidx = "Flag";
                string sord = "Asc";
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<long, IList<SecondLanguageMaster>> SecondLanguageMasterList = AMS.GetSecondLanguageMasterListWithCriteria(0, 1000, sidx, sord, criteria);
                if (SecondLanguageMasterList != null && SecondLanguageMasterList.First().Value != null && SecondLanguageMasterList.First().Value.Count > 0)
                {
                    Dictionary<string, string> SecondLanguageList = new Dictionary<string, string>();
                    foreach (var item in SecondLanguageMasterList.FirstOrDefault().Value)
                    {
                        SecondLanguageList.Add(item.SecondLanguageCode, item.SecondLanguageText);
                    }
                    return PartialView("Dropdown", SecondLanguageList.Distinct());
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

        public JsonResult callingforFollowupdetailsService()
        {
            AdmissionManagementBC ABC = new AdmissionManagementBC();
            bool RetVal = ABC.SendEnquiryRemainderMailtoAdmin();
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        #region AgeCutOff

        public ActionResult AdmissionAgeCutOff()
        {
            try
            {
                TIPS.Entities.User Userobj = (TIPS.Entities.User)Session["objUser"];
                if (Userobj == null) { return RedirectToAction("LogOff", "Account"); }
                else
                {//Modified by Thamizhmani
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
        public ActionResult AdmissionAgeCutOffJqgrid(string AcademicYear, string Grade, string FromDate, string ToDate, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    AdmissionManagementService AdmnSvc = new AdmissionManagementService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    //Modified By Thamizhmani
                    if (!string.IsNullOrWhiteSpace(AcademicYear)) criteria.Add("AcademicYear", AcademicYear);
                    if (!string.IsNullOrWhiteSpace(Grade)) criteria.Add("Grade", Grade);
                    if (!string.IsNullOrWhiteSpace(FromDate)) criteria.Add("FromDate", FromDate);
                    if (!string.IsNullOrWhiteSpace(ToDate)) criteria.Add("ToDate", ToDate);
                    sord = sord == "desc" ? "Desc" : "Asc";
                    Dictionary<long, IList<AgeCutOffMaster>> AgeCutOffList = AdmnSvc.GetAgeCutOffDetailsListWithCriteria(0, 9999, sord, sidx, criteria);
                    if (AgeCutOffList != null && AgeCutOffList.FirstOrDefault().Value.Count > 0 && AgeCutOffList.FirstOrDefault().Key > 0)
                    {
                        long totalrecords = AgeCutOffList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)5);
                        var AssLst = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in AgeCutOffList.First().Value
                                    select new
                                    {
                                        cell = new string[] 
                                         {
                                             items.Id.ToString(),
                                             items.AcademicYear,
                                             items.Grade,
                                             items.FromDate.Value.ToString("dd/MM/yyyy"),
                                             items.ToDate.Value.ToString("dd/MM/yyyy"),
                                             items.CreatedBy,
                                             items.CreatedDate.ToString(),
                                             items.ModifiedBy,
                                             items.ModifiedDate.ToString()
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

        public ActionResult AddAdmissionAgeCutOff(AgeCutOffMaster age, string edit)
        {
            try
            {
                string userId = ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MastersService ms = new MastersService();
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    if (age.Id > 0 && edit == "edit")
                    {
                        AgeCutOffMaster AgeCutOff = ms.GetAgeCutoffMasterDetailsById(age.Id);
                        AgeCutOff.AcademicYear = age.AcademicYear;
                        AgeCutOff.Grade = age.Grade;
                        if (!string.IsNullOrWhiteSpace(Request["FromDate"]))
                        {
                            age.FromDate = DateTime.Parse(Request["FromDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        }
                        if (!string.IsNullOrWhiteSpace(Request["ToDate"]))
                        {
                            age.ToDate = DateTime.Parse(Request["ToDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        }
                        AgeCutOff.FromDate = age.FromDate;
                        AgeCutOff.ToDate = age.ToDate;
                        AgeCutOff.ModifiedBy = userId;
                        AgeCutOff.ModifiedDate = DateTime.Now;
                        ms.SaveOrUpdateAgeCutOffDetails(AgeCutOff);
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(Request["FromDate"]))
                        {
                            age.FromDate = DateTime.Parse(Request["FromDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        }
                        if (!string.IsNullOrWhiteSpace(Request["ToDate"]))
                        {
                            age.ToDate = DateTime.Parse(Request["ToDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        }
                        age.CreatedDate = DateTime.Now;
                        age.CreatedBy = userId;
                        age.ModifiedBy = userId;
                        age.ModifiedDate = DateTime.Now;
                        ms.SaveOrUpdateAgeCutOffDetails(age);
                    }

                }
                return RedirectToAction("AdmissionAgeCutOff", "Admission");
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "SystemMgntPolicy");
                throw ex;
            }
        }

        public ActionResult DeleteAdmissionAgeCutOffMaster(string[] Id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MastersService ms = new MastersService();
                    string[] arrayrowId = Id[0].Split(',');
                    for (int i = 0; i < arrayrowId.Length; i++)
                    {
                        AgeCutOffMaster ACM = ms.GetAgeCutoffMasterDetailsById(Convert.ToInt64(arrayrowId[i]));
                        ms.GetDeleteAgeCutOffMasterById(ACM);
                    }
                    var script = @"SucessMsg(""Deleted Sucessfully...!"");";
                    return JavaScript(script);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult ValidateAgeCutOff(string AcYear, string Grade, string Dob)
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            AdmissionManagementService AdmnSvc = new AdmissionManagementService();
            criteria.Add("AcademicYear", AcYear);
            criteria.Add("Grade", Grade);
            Dictionary<long, IList<AgeCutOffMaster>> AgeCutOffList = AdmnSvc.GetAgeCutOffDetailsListWithCriteria(0, 9999, string.Empty, string.Empty, criteria);
            if (AgeCutOffList != null && AgeCutOffList.FirstOrDefault().Value.Count > 0 && AgeCutOffList.FirstOrDefault().Key > 0)
            {
                AgeCutOffMaster acm = AgeCutOffList.FirstOrDefault().Value[0];
                string[] DateOfBirth = Dob.Split('/');
                if ((acm.FromDate.Value.Year >= Convert.ToInt32(DateOfBirth[2])) && (acm.FromDate.Value.Month >= Convert.ToInt32(DateOfBirth[1])))
                {
                    if ((acm.ToDate.Value.Year >= Convert.ToInt32(DateOfBirth[2])) && (acm.ToDate.Value.Month >= Convert.ToInt32(DateOfBirth[1])))
                    {
                        return Json("Success", JsonRequestBehavior.AllowGet);
                    }
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                else { return Json("The given DOB doesn't meet in Age CutOff for Grade " + Grade + "...!", JsonRequestBehavior.AllowGet); }
            }
            else return Json("Success", JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult CampusDocmentUpload(HttpPostedFileBase[] file2, string PreRegNum)
        {
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            string attName = "";
            string att = "";
            for (int i = 0; i < file2.Length; i++)
            {
                CampusDocumentMaster dt = new CampusDocumentMaster();
                string[] strAttachname = file2[i].FileName.Split('\\');
                Attachment mailAttach = new Attachment(file2[i].InputStream, strAttachname[strAttachname.Length - 1]);  //Data posted from form
                dt.Campus = "IB Main";
                dt.DocumentType = "Parent Portal Circular";
                dt.DocumentName = strAttachname.First().ToString();
                dt.DocumentSize = file2[i].ContentLength.ToString();
                dt.CreatedBy = "Ashok";
                dt.CreatedDate = DateTime.Now;
                dt.ModifiedDate = DateTime.Now;
                dt.ModifiedBy = "Ashok";
                byte[] imageSize = new byte[file2[i].ContentLength];
                file2[i].InputStream.Read(imageSize, 0, (int)file2[i].ContentLength);
                dt.ActualDocument = imageSize;
                AdmissionManagementService ams = new AdmissionManagementService();
                ams.CreateOrUpdateCampusDocument(dt);
            }
            attName = att + " successfully Attached";
            return Json(new { success = true, result = attName }, "text/html", JsonRequestBehavior.AllowGet);
        }

        #region "Payment Report List Page"
        public ActionResult PaymentReport()
        {
            try
            {
                TIPS.Entities.User Userobj = (TIPS.Entities.User)Session["objUser"];
                if (Userobj == null || (Userobj != null && Userobj.UserId == null))
                { return RedirectToAction("LogOff", "Account"); }
                else
                {
                    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                    IList<CampusMaster> cmpsmst = CampusMasterFunc();
                    ViewBag.campusddl = cmpsmst;
                    Dictionary<long, IList<FeeTypeMaster>> FeeTypeMaster = ms.GetFeeTypeMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, null);
                    Dictionary<long, IList<ModeOfPaymentMaster>> ModeOfPaymentMaster = ms.GetModeOfPaymentMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, null);
                    ViewBag.feetypeddl = FeeTypeMaster.First().Value;
                    ViewBag.modeofpmtddl = ModeOfPaymentMaster.First().Value;
                    //<IList>CampusMstList=
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult PaymentReportJqgrid(string feetype, string modeofpmtddl, string sidx, string sord, string Name, string Campus, string Grade, string ExpExcel, int rows, int? page = 1)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                string colName = string.Empty; string[] values = new string[1];
                if (!string.IsNullOrWhiteSpace(Name)) { criteria.Add("Name", Name); }
                if (!string.IsNullOrWhiteSpace(Campus) && Campus != "Select") { criteria.Add("Campus", Campus); }
                if (!string.IsNullOrWhiteSpace(Grade) && Grade != "Select")
                { colName = "Grade"; values[0] = Grade; }
                if (!string.IsNullOrWhiteSpace(feetype)) { criteria.Add("FeeType", feetype); }
                if (!string.IsNullOrWhiteSpace(modeofpmtddl)) { criteria.Add("ModeOfPayment", modeofpmtddl); }
                sord = sord == "desc" ? "Desc" : "Asc";
                //criteria.Add("IsActive", true);
                Dictionary<long, IList<PaymentDetailsReport_vw>> pmtrptLst = AMC.GetPaymentReportListWithPagingAndCriteriaWithAlias(page - 1, rows, sidx, sord, colName, values, criteria, null);
                if (pmtrptLst != null && pmtrptLst.First().Value != null && pmtrptLst.First().Value.Count > 0)
                {
                    IList<PaymentDetailsReport_vw> paymentrptList = pmtrptLst.FirstOrDefault().Value;
                    long totalRecords = pmtrptLst.FirstOrDefault().Key;
                    int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                    if (ExpExcel == "Excel")
                    {
                        base.ExptToXL(paymentrptList, "PaymentReport", (item => new
                        {
                            item.Name,
                            item.Campus,
                            item.Grade,
                            item.BoardingType,
                            item.FeeType,
                            item.ModeOfPayment,
                            ChequeDate = item.ChequeDate != null ? item.ChequeDate.Value.ToString("dd/MM/yyyy") : "",
                            item.ReferenceNo,
                            item.BankName,
                            item.Amount,
                            item.Remarks,
                            PaidDate = item.PaidDate != null ? item.PaidDate.Value.ToString("dd/MM/yyyy") : "",
                            ClearedDate = item.ClearedDate != null ? item.ClearedDate.Value.ToString("dd/MM/yyyy") : ""
                        }));
                        return new EmptyResult();

                    }
                    else
                    {
                        var jsonData = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalRecords,
                            rows = (
                            from items in paymentrptList
                            select new
                            {
                                i = items.Id,
                                cell = new string[] { 
                            items.Id.ToString(),
                            items.PreRegNum.ToString(),
                            items.Name,    //inbox secrch Field     
                            items.Campus,   //inbox secrch Field
                            items.Grade,
                            items.BoardingType,
                            items.AcademicYear,
                            items.FeeType,
                            items.ModeOfPayment,
                            items.ChequeDate!=null?items.ChequeDate.Value.ToString("dd/MM/yyyy"):"",
                            items.ReferenceNo,
                            items.BankName,
                            items.Amount,
                            items.Remarks,
                            items.PaidDate!=null?items.PaidDate.Value.ToString("dd/MM/yyyy"):"",
                            items.ClearedDate!=null?items.ClearedDate.Value.ToString("dd/MM/yyyy"):""
                                }
                            })
                        };
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                    }
                }
                else { return Json(null, JsonRequestBehavior.AllowGet); }
            }
            catch (Exception ex)
            { return ThrowJSONErrorNew(ex, "AdmissionPolicy"); }
            finally
            { }
        }
        #endregion

        public ActionResult StudentLocalityUpdate()
        {
            string userId = base.ValidateUser();
            if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
            else
            {
                #region BreadCrumb
                string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                #endregion
                return View();
            }
        }

        public JsonResult StudentLocalityDetailsUpdateJQGrid(string NewId, string Name, string Campus, string Grade, string Section, string FoodPreference, string Vanno, string BoardingType, string AdmissionStatus, string AcademicYear, string LocationName, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                AdmissionManagementService ams = new AdmissionManagementService();
                Dictionary<string, object> Extcriteria = new Dictionary<string, object>();
                Dictionary<string, object> likcriteria = new Dictionary<string, object>();
                var usrcmp = Session["UserCampus"] as IEnumerable<string>;
                if (!string.IsNullOrWhiteSpace(Campus)) { Extcriteria.Add("Campus", Campus); }
                else
                {
                    if (usrcmp != null && usrcmp.Count() != 0)
                    {
                        if (usrcmp.First() != null)            // to check if the usrcmp obj is null or with data
                        {
                            Extcriteria.Add("Campus", usrcmp);
                        }
                    }
                }
                if (!string.IsNullOrEmpty(NewId)) { likcriteria.Add("NewId", NewId); }
                if (!string.IsNullOrEmpty(Name)) { likcriteria.Add("Name", Name); }
                if (!string.IsNullOrEmpty(Grade)) { Extcriteria.Add("Grade", Grade); }
                if (!string.IsNullOrEmpty(Section)) { Extcriteria.Add("Section", Section); }
                if (!string.IsNullOrEmpty(FoodPreference)) { likcriteria.Add("FoodPreference", FoodPreference); }
                if (!string.IsNullOrEmpty(BoardingType)) { likcriteria.Add("BoardingType", BoardingType); }
                if (!string.IsNullOrEmpty(Vanno)) { Extcriteria.Add("VanNo", Vanno); }
                if (!string.IsNullOrEmpty(AdmissionStatus)) { Extcriteria.Add("AdmissionStatus", AdmissionStatus); }
                else { Extcriteria.Add("AdmissionStatus", "Registered"); }
                if (!string.IsNullOrEmpty(AcademicYear)) { likcriteria.Add("AcademicYear", AcademicYear); }
                if (!string.IsNullOrEmpty(LocationName)) { likcriteria.Add("LocationName", LocationName); }
                sord = sord == "desc" ? "Desc" : "Asc";
                Dictionary<long, IList<StudentTemplateAndFamilyDetails_vw>> StudentTemplateAndFamilyDetailsList = null;
                StudentTemplateAndFamilyDetailsList = ams.StudentTemplateAndFamilyDetails_vwListWithLikeAndExcactSerachCriteria(page - 1, rows, sidx, sord, Extcriteria, likcriteria);
                if (StudentTemplateAndFamilyDetailsList != null && StudentTemplateAndFamilyDetailsList.First().Key > 0)
                {
                    long totalrecords = StudentTemplateAndFamilyDetailsList.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var AssLst = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,
                        rows = (
                             from items in StudentTemplateAndFamilyDetailsList.First().Value
                             select new
                             {
                                 cell = new string[] 
                                         {
                                            items.Id.ToString(),
                                            items.StudentTemplateId.ToString(),
                                            items.PreRegNum.ToString(),
                                            items.NewId,
                                            items.Name,
                                            items.Initial,
                                            items.Campus,
                                            items.Grade,
                                            items.Section,
                                            items.BoardingType,
                                            items.VanNo,
                                            items.AdmissionStatus,
                                            items.AcademicYear,
                                            items.Locality,
                                            items.Place,
                                            items.Kilometer,
                                            items.PickUpTime,
                                            items.DropTime,
                                            items.IsAdded.ToString()
                                         }
                             }).ToList()
                    };
                    return Json(AssLst, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    var AssLst = new { rows = (new { cell = new string[] { } }) };
                    return Json(AssLst, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult EditLocalityDetails(StudentTemplateAndFamilyDetails_vw obj)
        {
            try
            {
                string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                FamilyDetails FamilyDetails = new FamilyDetails();
                if (obj != null && obj.StudentTemplateId > 0)
                {
                    StudentTemplate StudTemp = AMC.GetStudentTemplateDetailsById(obj.StudentTemplateId);
                    if (StudTemp != null)
                    {
                        StudTemp.Locality = obj.Locality;
                        StudTemp.Kilometer = obj.Kilometer;
                        StudTemp.PickUpTime = obj.PickUpTime;
                        StudTemp.DropTime = obj.DropTime;
                        if (!String.IsNullOrEmpty(obj.Place) && obj.IsAdded == false)
                        {
                            StudentLocalityMaster StdLoc = AMC.GetStudentLocaLityMasterByLocality(obj.Locality);
                            if (StdLoc != null)
                            {
                                StudentSubLocalityMaster StdSLoc = new StudentSubLocalityMaster();
                                StdSLoc.Locality_Id = StdSLoc.Id;
                                StdSLoc.SubLocalityName = obj.Place;
                                StdSLoc.CreatedBy = userid;
                                StdSLoc.CreatedDate = DateTime.Now;
                                StdSLoc.ModifiedBy = userid;
                                StdSLoc.ModifiedDate = DateTime.Now;
                                AMC.CreateOrUpdateStudentSubLocality(StdSLoc);
                                StudTemp.Place = obj.Place;
                            }
                        }
                        else
                            StudTemp.Place = obj.Place;
                        AMC.CreateOrUpdateStudentTemplateDetails(StudTemp);
                    }
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public JsonResult GetSubLocality(string term)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(term)) { criteria.Add("SubLocalityName", term); }
                Dictionary<long, IList<StudentSubLocalityMaster>> SubLocMaster = AMC.GetSublocalityMasterListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                var PlaceNames = (from u in SubLocMaster.First().Value
                                  where u.SubLocalityName != null
                                  select u.SubLocalityName).Distinct().ToList();
                return Json(PlaceNames, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult GetLocality(string Campus)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(Campus)) { criteria.Add("Campus", Campus); }
                Dictionary<long, IList<StudentLocalityMaster>> LocMaster = AMC.GetlocalityMasterListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                if (LocMaster != null && LocMaster.First().Value != null && LocMaster.First().Value.Count > 0)
                {
                    Dictionary<string, string> LocMasterList = new Dictionary<string, string>();
                    foreach (var item in LocMaster.FirstOrDefault().Value)
                    {
                        if (!LocMasterList.ContainsKey(item.LocalityName))
                        {
                            LocMasterList.Add(item.LocalityName, item.LocalityName);
                        }
                    }
                    return PartialView("Dropdown", LocMasterList.Distinct());
                }
                else
                {
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult GetLocalitydropdown(string Campus)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(Campus)) { criteria.Add("Campus", Campus); }
                Dictionary<long, IList<StudentLocalityMaster>> LocMaster = AMC.GetlocalityMasterListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                if (LocMaster != null && LocMaster.First().Value != null && LocMaster.First().Value.Count > 0)
                {
                    var subjectMstrLst = new
                    {
                        rows = (from items in LocMaster.FirstOrDefault().Value
                                select new
                                {
                                    Text = items.LocalityName,
                                    Value = items.LocalityName
                                }).Distinct().ToArray()
                    };
                    return Json(subjectMstrLst, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        #region StudentLocalityReport

        public ActionResult StudentLocalityReport()
        {
            string userId = base.ValidateUser();
            if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
            else
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<long, IList<CampusMaster>> CampusMaster = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                ViewBag.campusddl = CampusMaster.First().Value;
                Dictionary<long, IList<GradeMaster>> GradeMaster = ms.GetGradeMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                ViewBag.gradeddl1 = GradeMaster.First().Value;

                #region BreadCrumb
                string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                #endregion
                return View();
            }
        }
        public ActionResult StudentLocalityReportJqgrid(string ExportType, string Campus, string Location, int? Id, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    if (!string.IsNullOrEmpty(Campus)) { criteria.Add("Campus", Campus); }
                    if (!string.IsNullOrEmpty(Location)) { criteria.Add("Locality", Location); }
                    AdmissionManagementService ams = new AdmissionManagementService();
                    Dictionary<long, IList<LocationCount_Vw>> LocationCount1 = ams.GetLocationListWithEQsearchCriteria(0, 9999, string.Empty, string.Empty, criteria);
                    if (LocationCount1 != null && LocationCount1.Count > 0)
                    {
                        var LocationCountList = LocationCount1.First().Value.ToList();
                        if (ExportType == "Excel")
                        {
                            ExptToXL(LocationCountList, "StudentLocationCountList", (items => new
                            {
                                items.Id,
                                items.Campus,
                                items.Locality,
                                LocationCount = items.LocationCount.ToString(),
                            }));
                            return new EmptyResult();

                        }
                        else
                        {
                            long totalrecords = LocationCount1.First().Key;
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var LocalityList = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalrecords,
                                rows = (from items in LocationCount1.First().Value
                                        select new
                                        {
                                            cell = new string[] {
                                                    items.Id.ToString(),
                                            items.Campus,
                                            items.Locality,
                                            items.LocationCount.ToString(),
                                                             }
                                        })
                            };
                        }
                        return Json(LocationCountList, JsonRequestBehavior.AllowGet);
                    }

                    else return Json(null);
                }
            }

            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;

            }
        }
        public JsonResult GetAutoCompleteLocationByCampus(string Campus, string term)
        {
            try
            {
                AdmissionManagementService ABC = new AdmissionManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                // criteria.Add("Campus", Campus);
                if (!string.IsNullOrEmpty(Campus)) { criteria.Add("Campus", Campus); }
                if (!string.IsNullOrEmpty(term)) { criteria.Add("LocalityName", term); }

                Dictionary<long, IList<StudentLocalityMaster>> LocationList = ABC.GetlocalityMasterListWithPagingAndCriteria(0, 9999, "", "", criteria);
                var Location = (from u in LocationList.First().Value
                                where u.LocalityName != null && u.LocalityName != ""
                                select u.LocalityName).Distinct().ToList();
                return Json(Location, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult StudentSubLocalityReport()
        {
            string userId = base.ValidateUser();
            if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
            else
            {
                MastersService ms = new MastersService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<long, IList<CampusMaster>> CampusMaster = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                ViewBag.campusddl = CampusMaster.First().Value;
                Dictionary<long, IList<GradeMaster>> GradeMaster = ms.GetGradeMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                ViewBag.gradeddl1 = GradeMaster.First().Value;

                #region BreadCrumb
                string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                #endregion
                return View();
            }
        }
        public ActionResult StudentSubLocalityReportJqgrid(string ExportType, string Campus, string Grade, string trans, string Locality, string SubLocality, int? Id, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    if (!string.IsNullOrEmpty(Campus)) { criteria.Add("Campus", Campus); }
                    if (!string.IsNullOrEmpty(Locality)) { criteria.Add("Locality", Locality); }
                    if (!string.IsNullOrEmpty(SubLocality)) { criteria.Add("Place", SubLocality); }
                    if (!string.IsNullOrEmpty(Grade)) { criteria.Add("Grade", Grade); }
                    if (!string.IsNullOrEmpty(trans)) { criteria.Add("Transport", trans == "True" ? true : false); }
                    criteria.Add("AdmissionStatus", "Registered");
                    AdmissionManagementService ams = new AdmissionManagementService();
                    Dictionary<long, IList<StudentTemplate>> studenttemplate = ams.GetStudentDetailsListWithEQsearchCriteria(page - 1, rows, sord, sidx, criteria);
                    if (studenttemplate != null && studenttemplate.Count > 0)
                    {
                        var StudentList = studenttemplate.First().Value.ToList();
                        if (ExportType == "Excel")
                        {
                            ExptToXL(StudentList, "StudentLocationCountList", (items => new
                            {
                                items.Campus,
                                items.Name,
                                items.NewId,
                                items.Grade,
                                items.Section,
                                items.AcademicYear,
                                items.AdmissionStatus,
                                Transport = items.Transport.ToString(),
                                items.Locality,
                                SubLocality = items.Place,
                                items.Kilometer,
                                items.PickUpTime,
                                items.DropTime
                            }));
                            return new EmptyResult();
                        }
                        else
                        {
                            long totalrecords = studenttemplate.First().Key;
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var StudentLocalityList = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalrecords,
                                rows = (from items in studenttemplate.First().Value
                                        select new
                                        {
                                            i = 2,
                                            cell = new string[] {
                                            items.Id.ToString(),
                                                items.Campus,
                                                items.Name,
                                                items.NewId,
                                                items.Grade,
                                                items.Section,
                                                items.AcademicYear,
                                                items.AdmissionStatus,
                                                items.Transport.ToString(),
                                                items.Locality,
                                                items.Place,
                                                items.Kilometer,
                                                items.PickUpTime,
                                                items.DropTime
                                            }
                                        })
                            };
                            return Json(StudentLocalityList, JsonRequestBehavior.AllowGet);
                        }
                    }

                    else return Json(null);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        #endregion

        public ActionResult AdmissionFollowUpDashBoard(string campus)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    long[] FlwCnt = new long[3];
                    FlwCnt = getFollowUpcount(campus);
                    #region BreadCrumb
                    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                    #endregion
                    @ViewBag.YesterdayFlwUpCount = FlwCnt[0];
                    @ViewBag.TodayFlwUpCount = FlwCnt[1];
                    @ViewBag.TomorrowFlwUpCount = FlwCnt[2];
                    ViewBag.Campus = campus;
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult FollowUpCampusCount(string campus)
        {
            long[] FollowUpCnt = new long[3];
            FollowUpCnt = getFollowUpcount(campus);
            return Json(FollowUpCnt, JsonRequestBehavior.AllowGet);
        }

        public long[] getFollowUpcount(string cam)
        {
            long[] followupcnt = new long[3];
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            Dictionary<long, IList<FollowUpReport_vw>> FollowUpTodayList = null;
            Dictionary<long, IList<FollowUpReport_vw>> FollowUpYesterdayList = null;
            Dictionary<long, IList<FollowUpReport_vw>> FollowUpTomorrowList = null;
            if (!string.IsNullOrWhiteSpace(cam)) criteria.Add("Campus", cam);
            criteria.Add("FollowupDate", DateTime.Now.AddDays(-1).ToString("dd'/'MM'/'yyyy"));
            criteria.Add("Sequence", 1);
            FollowUpYesterdayList = AMC.GetFollowupReportList(0, 99999, string.Empty, string.Empty, criteria, null);
            if (FollowUpYesterdayList != null && FollowUpYesterdayList.First().Key > 0)
                followupcnt[0] = FollowUpYesterdayList.First().Key;
            criteria.Clear();
            if (!string.IsNullOrWhiteSpace(cam)) criteria.Add("Campus", cam);
            criteria.Add("FollowupDate", DateTime.Now.ToString("dd'/'MM'/'yyyy"));
            criteria.Add("Sequence", 1);
            FollowUpTodayList = AMC.GetFollowupReportList(0, 99999, string.Empty, string.Empty, criteria, null);
            if (FollowUpTodayList != null && FollowUpTodayList.First().Key > 0)
                followupcnt[1] = FollowUpTodayList.First().Key;
            criteria.Clear();
            if (!string.IsNullOrWhiteSpace(cam)) criteria.Add("Campus", cam);
            criteria.Add("FollowupDate", DateTime.Now.AddDays(1).ToString("dd'/'MM'/'yyyy"));
            criteria.Add("Sequence", 1);
            FollowUpTomorrowList = AMC.GetFollowupReportList(0, 99999, string.Empty, string.Empty, criteria, null);
            if (FollowUpTomorrowList != null && FollowUpTomorrowList.First().Key > 0)
                followupcnt[2] = FollowUpTomorrowList.First().Key;
            return followupcnt;
        }

        public ActionResult FollowUpCampusWiseCountChart(string campus)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                var FollowupChart = "<graph caption='Campus Wise Students Enquiry FollowUp Details with Counts Reports' xAxisName='' decimalPrecision='0' forceDecimals='0' formatNumberScale='0' yAxisName='' animation='1' showNames='1' showValues='0' divlinecolor='d3d3d3' distance='5' angle='45' rotateNames='1'>";
                FollowupChart = FollowupChart + "<categories>";
                var usrcmp = Session["UserCampus"] as IEnumerable<string>;
                if (usrcmp != null && usrcmp.Count() != 0)
                {
                    foreach (var CampusNameItem in usrcmp)
                    {
                        FollowupChart = FollowupChart + "<category name='" + CampusNameItem + "' />";
                    }
                }
                FollowupChart = FollowupChart + "</categories>";
                //Yesterday Followup
                Dictionary<long, IList<FollowUpReport_vw>> FollowUpYesterdayList = null;
                FollowupChart = FollowupChart + " <dataset seriesname='Yesterday FollowUp' color='F6BD0F'>";
                criteria.Clear();
                if (!string.IsNullOrWhiteSpace(campus)) criteria.Add("Campus", campus);
                criteria.Add("FollowupDate", DateTime.Now.AddDays(-1).ToString("dd'/'MM'/'yyyy"));
                criteria.Add("Sequence", 1);
                FollowUpYesterdayList = AMC.GetFollowupReportList(0, 99999, string.Empty, string.Empty, criteria, null);
                if (FollowUpYesterdayList != null && FollowUpYesterdayList.First().Key > 0)
                {
                    foreach (var CampusItem in usrcmp)
                    {
                        var TempList = (from u in FollowUpYesterdayList.FirstOrDefault().Value
                                        where u.Campus == CampusItem
                                        select u).ToList();
                        if (TempList != null)
                        {
                            FollowupChart = FollowupChart + "<set value='" + TempList.Count + "' />";
                        }
                    }
                }
                FollowupChart = FollowupChart + "</dataset>";
                //Today Followup
                Dictionary<long, IList<FollowUpReport_vw>> FollowUpTodayList = null;
                FollowupChart = FollowupChart + " <dataset seriesname='Today FollowUp' color='08E8E'>";
                criteria.Clear();
                if (!string.IsNullOrWhiteSpace(campus)) criteria.Add("Campus", campus);
                criteria.Add("FollowupDate", DateTime.Now.ToString("dd'/'MM'/'yyyy"));
                criteria.Add("Sequence", 1);
                FollowUpTodayList = AMC.GetFollowupReportList(0, 99999, string.Empty, string.Empty, criteria, null);
                if (FollowUpTodayList != null && FollowUpTodayList.First().Key > 0)
                {
                    foreach (var CampusItem in usrcmp)
                    {
                        string Campus = string.Empty;
                        var TempList = (from u in FollowUpTodayList.FirstOrDefault().Value
                                        where u.Campus == CampusItem
                                        select u).ToList();
                        if (TempList != null)
                        {
                            FollowupChart = FollowupChart + "<set value='" + TempList.Count + "' />";
                        }
                    }
                }
                FollowupChart = FollowupChart + "</dataset>";
                //Tomorrow Followup
                Dictionary<long, IList<FollowUpReport_vw>> FollowUpTomorrowList = null;
                FollowupChart = FollowupChart + " <dataset seriesname='Tomorrow FollowUp' color='8BBA00'>";
                criteria.Clear();
                if (!string.IsNullOrWhiteSpace(campus)) criteria.Add("Campus", campus);
                criteria.Add("FollowupDate", DateTime.Now.AddDays(1).ToString("dd'/'MM'/'yyyy"));
                criteria.Add("Sequence", 1);
                FollowUpTomorrowList = AMC.GetFollowupReportList(0, 99999, string.Empty, string.Empty, criteria, null);
                if (FollowUpTomorrowList != null && FollowUpTomorrowList.First().Key > 0)
                {
                    foreach (var CampusItem in usrcmp)
                    {
                        var TempList = (from u in FollowUpTomorrowList.FirstOrDefault().Value
                                        where u.Campus == CampusItem
                                        select u).ToList();
                        if (TempList != null)
                        {
                            FollowupChart = FollowupChart + "<set value='" + TempList.Count + "' />";
                        }
                    }
                }
                FollowupChart = FollowupChart + "</dataset></graph>";
                Response.Write(FollowupChart);
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult FollowUpCountChart(string campus)
        {
            try
            {
                long YesterdayFlwUpCount = 0;
                long TodayFlwUpCount = 0;
                long TomorrowFlwUpCount = 0;
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<long, IList<FollowUpReport_vw>> FollowUpTodayList = null;
                Dictionary<long, IList<FollowUpReport_vw>> FollowUpYesterdayList = null;
                Dictionary<long, IList<FollowUpReport_vw>> FollowUpTomorrowList = null;
                if (!string.IsNullOrWhiteSpace(campus)) criteria.Add("Campus", campus);
                criteria.Add("FollowupDate", DateTime.Now.ToString("dd'/'MM'/'yyyy"));
                criteria.Add("Sequence", 1);
                FollowUpTodayList = AMC.GetFollowupReportList(0, 99999, string.Empty, string.Empty, criteria, null);
                if (FollowUpTodayList != null && FollowUpTodayList.First().Key > 0)
                    TodayFlwUpCount = FollowUpTodayList.First().Key;
                criteria.Clear();
                if (!string.IsNullOrWhiteSpace(campus)) criteria.Add("Campus", campus);
                criteria.Add("FollowupDate", DateTime.Now.AddDays(1).ToString("dd'/'MM'/'yyyy"));
                criteria.Add("Sequence", 1);
                FollowUpTomorrowList = AMC.GetFollowupReportList(0, 99999, string.Empty, string.Empty, criteria, null);
                if (FollowUpTomorrowList != null && FollowUpTomorrowList.First().Key > 0)
                    TomorrowFlwUpCount = FollowUpTomorrowList.First().Key;
                criteria.Clear();
                if (!string.IsNullOrWhiteSpace(campus)) criteria.Add("Campus", campus);
                criteria.Add("FollowupDate", DateTime.Now.AddDays(-1).ToString("dd'/'MM'/'yyyy"));
                criteria.Add("Sequence", 1);
                FollowUpYesterdayList = AMC.GetFollowupReportList(0, 99999, string.Empty, string.Empty, criteria, null);
                if (FollowUpYesterdayList != null && FollowUpYesterdayList.First().Key > 0)
                    YesterdayFlwUpCount = FollowUpYesterdayList.First().Key;
                if (YesterdayFlwUpCount != 0 || TodayFlwUpCount != 0 || TomorrowFlwUpCount != 0)
                {
                    var FollowupcountChart = "<graph caption='Enquiry Followup Count' xAxisName='' decimalPrecision='0' forceDecimals='0' formatNumberScale='0' yAxisName='' animation='1' showNames='1' showValues='1' divlinecolor='d3d3d3' distance='5' angle='45' rotateNames='1'>";
                    FollowupcountChart = FollowupcountChart + " <set name='Yesterday Followup' value='" + YesterdayFlwUpCount + "' color='008ee4' issliced='1'/>";
                    FollowupcountChart = FollowupcountChart + " <set name='Today Followup' value='" + TodayFlwUpCount + "' color='f8bd19' issliced='1'/>";
                    FollowupcountChart = FollowupcountChart + " <set name='Tomorrow Followup' value='" + TomorrowFlwUpCount + "' color='6baa01' issliced='1'/>";
                    FollowupcountChart = FollowupcountChart + "</graph>";
                    Response.Write(FollowupcountChart);
                }
                else
                {
                    var EmptyChart = "<graph caption='Enquiry Followup Count' xAxisName='' decimalPrecision='0' forceDecimals='0' formatNumberScale='0' yAxisName='' animation='1' showNames='1' showValues='1' divlinecolor='d3d3d3' distance='5' angle='45' rotateNames='1'>";
                    EmptyChart = EmptyChart + " <set name='No Enquires' value='1' color='008ee4' issliced='1'/>";
                    EmptyChart = EmptyChart + "</graph>";
                    Response.Write(EmptyChart);
                }
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult AdmissionFollowUp(string Campus, string flag)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    #region BreadCrumb
                    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                    #endregion
                    ViewBag.Campus = Campus;
                    ViewBag.flag = flag;
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }

        }

        public JsonResult AdmissionEnquiryFollowUpJqgrid(string Name, string Campus, string Grade, string AdmissionStatus, string flag, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                Dictionary<string, object> Extcriteria = new Dictionary<string, object>();
                Dictionary<string, object> likcriteria = new Dictionary<string, object>();
                if (!string.IsNullOrWhiteSpace(Campus)) { Extcriteria.Add("Campus", Campus); }
                if (!string.IsNullOrEmpty(Name)) { likcriteria.Add("Name", Name); }
                if (!string.IsNullOrEmpty(Grade)) { Extcriteria.Add("Grade", Grade); }
                if (!string.IsNullOrEmpty(AdmissionStatus)) { Extcriteria.Add("AdmissionStatus", AdmissionStatus); }
                if (flag == "tomorrow") { Extcriteria.Add("FollowupDate", DateTime.Now.AddDays(1).ToString("dd'/'MM'/'yyyy")); }
                else if (flag == "Yday") { Extcriteria.Add("FollowupDate", DateTime.Now.AddDays(-1).ToString("dd'/'MM'/'yyyy")); }
                else { Extcriteria.Add("FollowupDate", DateTime.Now.ToString("dd'/'MM'/'yyyy")); }
                Extcriteria.Add("Sequence", 1);
                sord = sord == "desc" ? "Desc" : "Asc";
                Dictionary<long, IList<FollowUpReport_vw>> FollowupList = AMC.GetFollowupReportList(page - 1, rows, sidx, sord, Extcriteria, likcriteria);
                if (FollowupList != null && FollowupList.First().Key > 0)
                {
                    long totalrecords = FollowupList.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var flwLst = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,
                        rows = (
                             from items in FollowupList.First().Value
                             select new
                             {
                                 cell = new string[] 
                                         {
                                            items.Id.ToString(),
                                            items.PreRegNum.ToString(),
                                            //items.Name,
                                            String.Format(@"<a style='color:#034af3;text-decoration:underline' href= '#' onclick = 'getdata("+"\"" + items.Student_ID + "\"" + ")' >{0}</a>",items.Name),
                                            items.Campus,
                                            items.Grade,
                                            items.AdmissionStatus,
                                            items.EmailId,
                                            items.MobileNo,
                                            items.FollowupDate,
                                            items.Remarks,
                                         }
                             }).ToList()
                    };
                    return Json(flwLst, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    var flwLst = new { rows = (new { cell = new string[] { } }) };
                    return Json(flwLst, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateInbox(string Campus, string issue, string userId, long AppNumber)
        {
            InboxService IBS = new InboxService();
            Inbox In = new Inbox();
            In.Campus = Campus;
            In.UserId = userId;
            In.InformationFor = issue;
            In.CreatedDate = DateTime.Now;
            In.Module = "Admission";
            In.Status = "Inbox";
            In.Campus = Campus;
            In.PreRegNum = AppNumber;
            IBS.CreateOrUpdateInbox(In);
        }

        public ActionResult AcademicYearReport()
        {
            MastersService ms = new MastersService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            Dictionary<long, IList<CampusMaster>> CampusMaster = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            ViewBag.campusddl = CampusMaster.First().Value;
            #region BreadCrumb
            string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
            #endregion
            return View();
        }

        public ActionResult AcademicYearReportJqgrid(string ExportType, int? Id, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    // if (!string.IsNullOrEmpty(Campus)) { criteria.Add("Campus", Campus); }
                    sord = sord == "desc" ? "Desc" : "Asc";
                    AdmissionManagementService ams = new AdmissionManagementService();
                    Assess360Controller As = new Assess360Controller();
                    Dictionary<long, IList<AcademicYearReport>> YearWiseReport = ams.GetAcademicYearWiseDetailsListWithEQsearchCriteria(page - 1, rows, sord, sidx, criteria);
                    if (YearWiseReport != null && YearWiseReport.Count > 0)
                    {
                        string headerTable = @"<Table border='1px' cellpadding='9' cellspacing='0'><tr><td colspan='9' align='center' style='font-size: large;'>Analysis Report For Admission</td></tr></b></Table>";
                        var StudentList = YearWiseReport.First().Value.ToList();
                        var List = (from s in StudentList
                                    orderby s.Id
                                    select s).ToList();
                        if (ExportType == "Excel")
                        {
                            ExptToXLWithHeader(StudentList, "AcademicYearWiseReport", (items => new
                            {
                                items.Id,
                                Academicyear = items.ShowAcademicYear,
                                IBMAIN = items.IBMain == null ? "0" : items.IBMain,
                                IBKG = items.IBKG == null ? "0" : items.IBKG,
                                TIPSSARAN = items.TipsSaran == null ? "0" : items.TipsSaran,
                                CHENNAICITY = items.ChennaiCity == null ? "0" : items.ChennaiCity,
                                CHENNAIMAIN = items.ChennaiMain == null ? "0" : items.ChennaiMain,
                                ERNAKULAM = items.Ernakulam == null ? "0" : items.Ernakulam,
                                ERNAKULAMKG = items.ErnakulamKG == null ? "0" : items.ErnakulamKG,
                                TIRPUR = items.Tirupur == null ? "0" : items.Tirupur,
                                TIRPURKG = items.TirupurKG == null ? "0" : items.TirupurKG,
                                KARUR = items.Karur == null ? "0" : items.Karur,
                                KARURKG = items.KarurKG == null ? "0" : items.KarurKG,

                            }), headerTable);
                            return new EmptyResult();
                        }
                        else
                        {
                            long totalrecords = YearWiseReport.First().Key;
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var YearWiseCountList = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalrecords,
                                rows = (from items in YearWiseReport.First().Value
                                        select new
                                        {
                                            cell = new string[] {
                                              items.Id.ToString(),
                                items.ShowAcademicYear,
                                items.IBMain==null?"0":items.IBMain,
                                items.IBKG==null?"0":items.IBKG,
                                items.TipsSaran==null?"0":items.TipsSaran,
                                items.ChennaiCity==null?"0":items.ChennaiCity,
                                items.ChennaiMain==null?"0":items.ChennaiMain,
                                items.Ernakulam==null?"0":items.Ernakulam,
                                items.ErnakulamKG==null?"0":items.ErnakulamKG,
                                items.Tirupur==null?"0":items.Tirupur,
                                items.TirupurKG==null?"0":items.TirupurKG,
                                items.Karur==null?"0":items.Karur,
                                items.KarurKG==null?"0":items.KarurKG,
                                            }
                                        })
                            };
                            return Json(YearWiseCountList, JsonRequestBehavior.AllowGet);
                        }

                    }

                    else return Json(null);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;

            }
        }

        #region TC Request
        public ActionResult TCRequest(string PreRegNo, long Id)
        {
            string userId = base.ValidateUser();
            if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
            else
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<long, IList<RelievingReasonMaster>> relievingMaster = AMC.GetRelievingReasonMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                Dictionary<long, IList<GradeMaster>> GradeMaster = ms.GetGradeMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                ViewBag.ReasonForLeaving = relievingMaster.First().Value;
                ViewBag.gradeddl = GradeMaster.First().Value;
                TCRequestDetails treq = new TCRequestDetails();
                if (Id == 0)
                {
                    StudentTemplate stud = AMC.GetStudentDetailsByPreRegNo(Convert.ToInt64(PreRegNo));
                    treq.Status = "Initiated";
                    treq.Name = stud.Name;
                    treq.Initial = stud.Initial;
                    treq.NewId = stud.NewId;
                    treq.PreRegNum = stud.PreRegNum;
                    treq.Campus = stud.Campus;
                    treq.Grade = stud.Grade;
                    treq.AcademicYear = stud.AcademicYear;
                }
                else
                {
                    treq = AMC.GetTCRequestDetailsById(Id);
                }
                return View(treq);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult TCRequest(TCRequestDetails tcReq, HttpPostedFileBase file1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<long, IList<RelievingReasonMaster>> relievingMaster = AMC.GetRelievingReasonMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                    Dictionary<long, IList<GradeMaster>> GradeMaster = ms.GetGradeMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                    ViewBag.ReasonForLeaving = relievingMaster.First().Value;
                    ViewBag.gradeddl = GradeMaster.First().Value;
                    tcReq.IsOtherReason = tcReq.ReasonForTCRequest == "Other" ? true : false;
                    tcReq.ReasonForTCRequest = tcReq.ReasonForTCRequest == "Other" ? Request.Form["otherReason"] : tcReq.ReasonForTCRequest;
                    if (Request.Form["btnSaveRequest"] == "SaveRequest")
                    {
                        if (tcReq.Id == 0)
                        {
                            tcReq.CreatedDate = DateTime.Now;
                            tcReq.ModifiedDate = DateTime.Now;
                            tcReq.CreatedBy = userId;
                            tcReq.ModifiedBy = userId;
                            tcReq.Status = "InProgress";
                            AMC.CreateOrUpdateTCRequestDetails(tcReq);
                            return RedirectToAction("TCRequest", new { Id = tcReq.Id });
                        }
                        else
                        {
                            tcReq.ModifiedDate = DateTime.Now;
                            tcReq.ModifiedBy = userId;
                            AMC.CreateOrUpdateTCRequestDetails(tcReq);
                        }
                    }
                    else if (Request.Form["DocUpload"] == "Upload")
                    {
                        string path = file1.InputStream.ToString();
                        byte[] imageSize = new byte[file1.ContentLength];
                        file1.InputStream.Read(imageSize, 0, (int)file1.ContentLength);
                        Documents doc = new Documents();
                        doc.EntityRefId = tcReq.Id;
                        doc.FileName = file1.FileName;
                        doc.UploadedOn = DateTime.Now;
                        doc.UploadedBy = userId;
                        doc.DocumentData = imageSize;
                        doc.DocumentSize = file1.ContentLength.ToString();
                        doc.AppName = "Admission";
                        doc.DocumentType = tcReq.DocumentType;
                        DocumentsService ds = new DocumentsService();
                        ds.CreateOrUpdateDocuments(doc);
                        return RedirectToAction("TCRequest", new { Id = tcReq.Id });
                    }
                    if (Request.Form["btnIdComplete"] == "Complete")
                    {
                        tcReq.ModifiedDate = DateTime.Now;
                        tcReq.ModifiedBy = userId;
                        tcReq.Status = "Completed";
                        AMC.CreateOrUpdateTCRequestDetails(tcReq);
                        UploadedFilesView fu1 = new UploadedFilesView();
                        fu1.DocumentName = "Transfer Certificate";
                        fu1.DocumentType = "" + tcReq.Campus + " Transfer Certificate";
                        fu1.UploadedDate = DateTime.Now.Date.ToString("dd/MM/yyyy");
                        fu1.DocumentFor = "Student";
                        fu1.PreRegNum = tcReq.PreRegNum;              // Pre Registration number
                        fu1.Type = "TransferPDF";
                        AMC.CreateOrUpdateUploadedFilesView(fu1);
                        StudentTemplate stud = AMC.GetStudentDetailsByPreRegNo(Convert.ToInt64(tcReq.PreRegNum));
                        stud.AdmissionStatus = "Discontinued";
                        stud.Status = null; stud.Status1 = null;
                        AMC.CreateOrUpdateStudentDetails(stud);
                        DeactivateStudent(stud.Id);
                        return RedirectToAction("TCRequestList");
                    }
                    return View(tcReq);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult TCRequestList()
        {
            string userId = base.ValidateUser();
            if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
            else
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<long, IList<CampusMaster>> CampusMaster = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                Dictionary<long, IList<AcademicyrMaster>> AcademicyrMaster = ms.GetAcademicyrMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                ViewBag.campusddl = CampusMaster.First().Value;
                ViewBag.acadddl = AcademicyrMaster.First().Value;

                string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);

                return View();
            }
        }

        //public ActionResult TCRequestListJqGrid(string name, string campus, string grade, string acadyr, string status, string ExportExcel, int rows, string sidx, string sord, int? page = 1)
        //{
        //    string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
        //    try
        //    {
        //        sord = sord == "desc" ? "Desc" : "Asc";
        //        AdmissionManagementService ads = new AdmissionManagementService();
        //        Dictionary<string, object> criteria = new Dictionary<string, object>();
        //        var usrcmp = Session["UserCampus"] as IEnumerable<string>;
        //        if (!string.IsNullOrEmpty(campus))
        //        { criteria.Add("Campus", campus); }
        //        else if (usrcmp != null && usrcmp.Count() != 0)
        //        {
        //            if (usrcmp.First() != null)            // to check if the usrcmp obj is null or with data
        //            {
        //                criteria.Add("Campus", usrcmp);
        //            }
        //        }
        //        else { }
        //        criteria.Add("Name", name);
        //        criteria.Add("Grade", grade);
        //        criteria.Add("Status", status);
        //        Dictionary<long, IList<TCRequestDetails>> tcRequestList = ads.GetTCRequestDetailsListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
        //        if (tcRequestList != null && tcRequestList.Count > 0)
        //        {
        //            if (ExportExcel == "Excel")
        //            {
        //                var List = tcRequestList.First().Value.ToList();
        //                base.ExptToXL(List, "StudentList", (items => new
        //                {
        //                    items.ApplicationNo,
        //                    items.PreRegNum,
        //                    Name = items.Name + items.Initial,
        //                    items.NewId,
        //                    items.Campus,
        //                    items.Grade,
        //                    items.AcademicYear,
        //                    items.TCRequestedDate,
        //                    items.TCRequiredOnDate,
        //                    CounselingForm = items.IsCounselingNeeded == true ? "Yes" : "No",
        //                    NoDueForm = items.IsNoDueForm == true ? "Yes" : "No",
        //                    ShortReason = items.ReasonForTCRequest,
        //                    CoordinatorComments = items.CoordComments,
        //                    AdmissionCoordinatorComments = items.ADMCordComments,
        //                    RegionalCoordinatorComments = items.RCordComments
        //                }));
        //                return new EmptyResult();

        //            }
        //            else
        //            {
        //                long totalrecords = tcRequestList.First().Key;
        //                int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
        //                var jsondat = new
        //                {
        //                    total = totalPages,
        //                    page = page,
        //                    records = totalrecords,

        //                    rows = (from items in tcRequestList.First().Value
        //                            select new
        //                            {
        //                                i = 2,
        //                                cell = new string[] {
        //                                items.Id.ToString(),
        //                      items.PreRegNum.ToString(),
        //                      String.Format(@"<a style='color:#034af3;text-decoration:underline' href= '#' onclick = 'getTcRequestdata("+"\"" + items.Id + "\"" + ")' >{0}</a>",items.Name+" "+items.Initial),
        //                      items.Campus,
        //                      items.Grade,
        //                      items.NewId,
        //                      items.AcademicYear,
        //                      items.Status,
        //                      items.CreatedBy!=null?us.GetUserNameByUserId(items.CreatedBy):"",
        //                      items.CreatedBy,
        //                      items.CreatedDate.ToString("dd'/'MM'/'yyyy"),
        //                      }
        //                            })
        //                };
        //                return Json(jsondat, JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //        else
        //        {
        //            var Empty = new { rows = (new { cell = new string[] { } }) };
        //            return Json(Empty, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
        //        throw ex;
        //    }
        //}



        public ActionResult TCRequestListJqGrid(string name, string AcademicYear, string TcType, string campus, string fromdate, string todate, string grade, string Tc, string status, string ExportExcel, int rows, string sidx, string sord, int? page = 1)
        {
            string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
            try
            {
                fromdate = null;
                todate = null;
                var TCTYPE = "TC";
                string[] s;
                sord = sord == "desc" ? "Desc" : "Asc";
                AdmissionManagementService ads = new AdmissionManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<string, object> likeSearchCriteria = new Dictionary<string, object>();
                var usrcmp = Session["UserCampus"] as IEnumerable<string>;
                if (!string.IsNullOrEmpty(campus))
                { criteria.Add("Campus", campus); }


                if (!string.IsNullOrEmpty(Tc))
                {
                    criteria.Add("IsNoDueForm", Convert.ToBoolean(Tc));
                }

                if (!string.IsNullOrEmpty(AcademicYear) && AcademicYear != "")
                {
                    s = AcademicYear.Split(',').ToArray();
                    fromdate = s[0];
                    todate = s[1];

                }


                if (TcType == "normaltc")
                {
                    TCTYPE = TcType;
                    TcType = null;
                }

                if (!string.IsNullOrEmpty(TcType))
                {
                    criteria.Add("ReasonForLeaving", TcType);
                }
                //else if (usrcmp != null && usrcmp.Count() != 0)
                //{
                //    if (usrcmp.First() != null)            // to check if the usrcmp obj is null or with data
                //    {
                //        criteria.Add("Campus", usrcmp);
                //    }
                //}
                else { }
                //criteria.Add("Name", name);
                //criteria.Add("Grade", grade);
                if (!string.IsNullOrEmpty(grade))
                {
                    criteria.Add("Grade", grade);
                }

                if (!string.IsNullOrEmpty(status))
                {
                    criteria.Add("Status", status);
                }

                if (!string.IsNullOrEmpty(fromdate) && !string.IsNullOrEmpty(todate))
                {
                    fromdate = fromdate.Trim();
                    todate = todate.Trim();
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    DateTime FromDate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime ToDate = DateTime.ParseExact(todate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    //DateTime FromDate = DateTime.Parse(fromdate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    //DateTime ToDate = DateTime.Parse(todate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    //string To = string.Format("{0:dd/MM/yyyy}", ToDate);
                    //DateTime TDate = Convert.ToDateTime(To + " " + "23:59:59.000");
                    DateTime[] fromto = new DateTime[2];
                    fromto[0] = FromDate;
                    fromto[1] = ToDate;
                    criteria.Add("LastAttendanceDateCopy", fromto);
                }

                Dictionary<long, IList<TCRequestDetails>> tcRequestList = ads.GetTCRequestDetailsListWithExactPagingAndExactCriteria(page - 1, rows, sord, sidx, criteria, likeSearchCriteria);
                if (!string.IsNullOrEmpty(TCTYPE) && TCTYPE == "normaltc")
                {

                    IList<TCRequestDetails> TcReqList = new List<TCRequestDetails>();
                    TcReqList = (from u in tcRequestList.FirstOrDefault().Value where u.ReasonForLeaving != "Course Completed" select u).ToList();
                    tcRequestList = null;
                    long totalrecords = TcReqList.Count();
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,

                        rows = (from items in TcReqList
                                select new
                                {
                                    i = 2,
                                    cell = new string[] {
                                        items.Id.ToString(),
                              items.PreRegNum.ToString(),
                              String.Format(@"<a style='color:#034af3;text-decoration:underline' href= '#' onclick = 'getTcRequestdata("+"\"" + items.Id + "\"" + ")' >{0}</a>",items.Name+" "+items.Initial),
                              items.Campus,
                              items.Grade,
                              items.NewId,
                              items.AcademicYear,
                              items.Status,
                              //items.CreatedBy!=null?us.GetUserNameByUserId(items.CreatedBy):"",
                              //items.CreatedBy,
                              //items.CreatedDate.ToString("dd'/'MM'/'yyyy"),
                              items.TCRequestedDate,
                              items.TransferedDate,
                              items.LastDateOfAttendance,
                              }
                                })
                    };
                    return Json(jsondat, JsonRequestBehavior.AllowGet);
                }
                //(from u in StudentDetails.FirstOrDefault().Value
                // where !string.IsNullOrEmpty(u.LocationTamilDescription)
                // select u).ToList();
                else
                {
                    if (tcRequestList != null && tcRequestList.Count > 0)
                    {
                        if (ExportExcel == "Excel")
                        {
                            var List = tcRequestList.First().Value.ToList();
                            base.ExptToXL(List, "StudentList", (items => new
                            {
                                items.ApplicationNo,
                                items.PreRegNum,
                                Name = items.Name + items.Initial,
                                items.NewId,
                                items.Campus,
                                items.Grade,
                                items.AcademicYear,
                                items.TCRequestedDate,
                                items.TCRequiredOnDate,
                                CounselingForm = items.IsCounselingNeeded == true ? "Yes" : "No",
                                NoDueForm = items.IsNoDueForm == true ? "Yes" : "No",
                                ShortReason = items.ReasonForTCRequest,
                                CoordinatorComments = items.CoordComments,
                                AdmissionCoordinatorComments = items.ADMCordComments,
                                RegionalCoordinatorComments = items.RCordComments
                            }));
                            return new EmptyResult();

                        }
                        else
                        {
                            long totalrecords = tcRequestList.First().Key;
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var jsondat = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalrecords,

                                rows = (from items in tcRequestList.First().Value
                                        select new
                                        {
                                            i = 2,
                                            cell = new string[] {
                                        items.Id.ToString(),
                              items.PreRegNum.ToString(),
                              String.Format(@"<a style='color:#034af3;text-decoration:underline' href= '#' onclick = 'getTcRequestdata("+"\"" + items.Id + "\"" + ")' >{0}</a>",items.Name+" "+items.Initial),
                              items.Campus,
                              items.Grade,
                              items.NewId,
                              items.AcademicYear,
                              items.Status,
                              //items.CreatedBy!=null?us.GetUserNameByUserId(items.CreatedBy):"",
                              //items.CreatedBy,
                              //items.CreatedDate.ToString("dd'/'MM'/'yyyy"),
                              //items.ModifiedDate.ToString("dd'/'MM'/'yyyy"),
                              items.TCRequestedDate,
                              items.TransferedDate,
                              items.LastDateOfAttendance,
                              }
                                        })
                            };
                            return Json(jsondat, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        var Empty = new { rows = (new { cell = new string[] { } }) };
                        return Json(Empty, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult TCRequestReport()
        {
            string userId = base.ValidateUser();
            if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
            else
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<long, IList<AcademicyrMaster>> AcademicyrMaster = ms.GetAcademicyrMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                ViewBag.acadyrddl = AcademicyrMaster.First().Value;
                string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                return View();
            }
        }

        public ActionResult TCRequestReportByReasonListJqGrid(string acadyr, string Status, string FromDate, string ToDate, string ExprtToExcel, int rows, string sidx, string sord, int? page = 1)
        {
            string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
            try
            {
                sord = sord == "desc" ? "Desc" : "Asc";
                AdmissionManagementService ads = new AdmissionManagementService();
                IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                //if (!string.IsNullOrEmpty(acadyr))
                //{ criteria.Add("AcademicYear", acadyr); }
                //if (!string.IsNullOrEmpty(Status))
                //{ criteria.Add("Status", Status); }
                DateTime? FromDateNew = new DateTime();
                DateTime? ToDatenew = new DateTime();
                if (!string.IsNullOrEmpty(FromDate))
                {
                    FromDate = FromDate + " " + "00:00:00";
                    FromDateNew = DateTime.Parse(FromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    if (string.IsNullOrEmpty(ToDate))
                    {
                        ToDatenew = DateTime.Now;
                    }
                    if (!string.IsNullOrEmpty(ToDate))
                    {
                        ToDate = ToDate + " " + "23:59:59";
                        ToDatenew = DateTime.Parse(ToDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    }
                }
                else
                {
                    FromDateNew = null;
                    ToDatenew = null;
                }
                Dictionary<long, IList<TcRequestReportByCampus_SP>> tcRequestReportByReasonList = ads.GetTcRequestReportByCampusListSP(acadyr, Status, FromDateNew, ToDatenew);
                if (tcRequestReportByReasonList == null || tcRequestReportByReasonList.FirstOrDefault().Key == 0)
                {
                    var Empty = new { rows = (new { cell = new string[] { } }) };
                    return Json(Empty, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    IList<TcRequestReportByCampus_SP> TcRepList = new List<TcRequestReportByCampus_SP>();
                    if (sord == "Desc")
                    {
                        TcRepList = (from u in tcRequestReportByReasonList.FirstOrDefault().Value select u).OrderByDescending(x => x.GetType().GetProperty(sidx).GetValue(x, null)).ToList();
                    }
                    else
                    {
                        TcRepList = (from u in tcRequestReportByReasonList.FirstOrDefault().Value select u).OrderBy(x => x.GetType().GetProperty(sidx).GetValue(x, null)).ToList();
                    }
                    if (ExprtToExcel == "Excel")
                    {
                        base.ExptToXL(TcRepList, "TCRequestReportByReason", (item => new
                        {
                            item.AcademicYear,
                            item.ReasonForTCRequest,
                            item.IBMain,
                            item.IBKG,
                            item.ChennaiMain,
                            item.ChennaiCity,
                            item.Ernakulam,
                            item.ErnakulamKG,
                            item.Karur,
                            item.KarurKG,
                            item.Tirupur,
                            item.TirupurKG,
                            item.TipsSaran,
                            item.Total
                        }));
                        return new EmptyResult();
                    }
                    else
                    {
                        //IList<TcRequestReasonByCampus_Vw> TcReportByReasonList = tcRequestReportByReasonList.FirstOrDefault().Value;
                        long totalRecords = TcRepList.Count;
                        int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                        var jsonData = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalRecords,
                            rows =
                            (from items in TcRepList
                             select new
                             {
                                 i = items.Id,
                                 cell = new string[]
                           {
                                  items.Id.ToString(),
                                  items.AcademicYear,
                                  items.ReasonForTCRequest,
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
                                  items.TipsSaran.ToString(),
                                  items.Total.ToString()
                           }
                             })
                        };
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult TCRequestReportByGradeListJqGrid(string acadyr, string Status, string FromDate, string ToDate, string ExprtToExcel, int rows, string sidx, string sord, int? page = 1)
        {
            string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
            try
            {
                sord = sord == "desc" ? "Desc" : "Asc";
                AdmissionManagementService ads = new AdmissionManagementService();
                IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                DateTime? FromDateNew = new DateTime();
                DateTime? ToDatenew = new DateTime();
                if (!string.IsNullOrEmpty(FromDate))
                {
                    FromDate = FromDate + " " + "00:00:00";
                    FromDateNew = DateTime.Parse(FromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    if (string.IsNullOrEmpty(ToDate))
                    {
                        ToDatenew = DateTime.Now;
                    }
                    if (!string.IsNullOrEmpty(ToDate))
                    {
                        ToDate = ToDate + " " + "23:59:59";
                        ToDatenew = DateTime.Parse(ToDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    }
                }
                else
                {
                    FromDateNew = null;
                    ToDatenew = null;
                }
                //Dictionary<string, object> criteria = new Dictionary<string, object>();
                //if (!string.IsNullOrEmpty(acadyr))
                //{ criteria.Add("AcademicYear", acadyr); }
                Dictionary<long, IList<TcRequestReportByCampusGrade_SP>> tcRequestReportByGradeList = ads.GetTcRequestReportByCampusGradeListSP(acadyr, Status, FromDateNew, ToDatenew);
                if (tcRequestReportByGradeList == null || tcRequestReportByGradeList.FirstOrDefault().Key == 0)
                {
                    var Empty = new { rows = (new { cell = new string[] { } }) };
                    return Json(Empty, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    IList<TcRequestReportByCampusGrade_SP> TcRepList = new List<TcRequestReportByCampusGrade_SP>();
                    if (sord == "Desc")
                    {
                        TcRepList = (from u in tcRequestReportByGradeList.FirstOrDefault().Value select u).OrderByDescending(x => x.GetType().GetProperty(sidx).GetValue(x, null)).ToList();
                    }
                    else
                    {
                        TcRepList = (from u in tcRequestReportByGradeList.FirstOrDefault().Value select u).OrderBy(x => x.GetType().GetProperty(sidx).GetValue(x, null)).ToList();
                    }
                    if (ExprtToExcel == "Excel")
                    {
                        base.ExptToXL(TcRepList, "TCRequestReportByGrade", (item => new
                        {
                            item.AcademicYear,
                            item.Grade,
                            item.IBMain,
                            item.IBKG,
                            item.ChennaiMain,
                            item.ChennaiCity,
                            item.Ernakulam,
                            item.ErnakulamKG,
                            item.Karur,
                            item.KarurKG,
                            item.Tirupur,
                            item.TirupurKG,
                            item.TipsSaran,
                            item.Total
                        }));
                        return new EmptyResult();
                    }
                    else
                    {
                        //IList<TcRequestReasonByCampus_Vw> TcReportByReasonList = tcRequestReportByReasonList.FirstOrDefault().Value;
                        long totalRecords = tcRequestReportByGradeList.FirstOrDefault().Key;
                        int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                        var jsonData = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalRecords,
                            rows =
                            (from items in TcRepList
                             select new
                             {
                                 i = items.Id,
                                 cell = new string[]
                           {
                                  items.Id.ToString(),
                                  items.AcademicYear,
                                  items.Grade,
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
                                  items.TipsSaran.ToString(),
                                  items.Total.ToString()
                           }
                             })
                        };
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        #endregion

        #region CaptureImage Processing

        public ActionResult CaptureImage(string PreRegNo, string IsPhotoUploaded)
        {
            ViewBag.PreRegNum = PreRegNo;
            ViewBag.IsPhotoUploaded = IsPhotoUploaded;
            return View();
        }

        public ActionResult Capture()
        {
            var stream = Request.InputStream;
            string dump;

            using (var reader = new StreamReader(stream))
            {
                dump = reader.ReadToEnd();

                DateTime nm = DateTime.Now;

                string date = nm.ToString("yyyymmddMMss");

                var path = Server.MapPath(ConfigurationManager.AppSettings["ImageCrop"] + date + "test.jpg");

                System.IO.File.WriteAllBytes(path, String_To_Bytes2(dump));

                Session["path"] = path;

                Session["val"] = ConfigurationManager.AppSettings["ImageCropShow"] + date + "test.jpg";
            }
            return Json("", JsonRequestBehavior.AllowGet);
            //return View("CaptureImage");
        }

        public JsonResult Rebind()
        {
            string path = Session["val"].ToString();
            return Json(path, JsonRequestBehavior.AllowGet);
        }

        private byte[] String_To_Bytes2(string strInput)
        {
            int numBytes = (strInput.Length) / 2;

            byte[] bytes = new byte[numBytes];

            for (int x = 0; x < numBytes; ++x)
            {
                bytes[x] = Convert.ToByte(strInput.Substring(x * 2, 2), 16);
            }
            return bytes;
        }

        [HttpPost]
        public virtual ActionResult CropImage(string imagePath, int? cropPointX, int? cropPointY, int? imageCropWidth, int? imageCropHeight)
        {
            if (string.IsNullOrEmpty(Session["path"].ToString()) || !cropPointX.HasValue || !cropPointY.HasValue || !imageCropWidth.HasValue || !imageCropHeight.HasValue)
            {
                return new HttpStatusCodeResult((int)HttpStatusCode.BadRequest);
            }


            byte[] imageBytes = System.IO.File.ReadAllBytes(Session["path"].ToString());
            byte[] croppedImage = ImageHelper.CropImage(imageBytes, cropPointX.Value, cropPointY.Value, imageCropWidth.Value, imageCropHeight.Value);

            string tempFolderName = Server.MapPath(ConfigurationManager.AppSettings["ImageCrop"]);
            string fileName = "Crop" + Path.GetFileName(imagePath);

            try
            {
                FileHelper.SaveFile(croppedImage, Path.Combine(tempFolderName, fileName));
            }
            catch (Exception ex)
            {
                //Log an error     
                return new HttpStatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
            Session["CropedPath"] = Path.Combine(tempFolderName, fileName);
            string photoPath = ConfigurationManager.AppSettings["ImageCropShow"] + fileName;
            return Json(photoPath, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UploadCropedPhotos(string docType, string documentFor, long RegNo)
        {

            //HttpPostedFileBase theFile = HttpContext.Request.Files["uploadedFile"];
            //string path = uploadedFile.InputStream.ToString();
            byte[] imageSize = System.IO.File.ReadAllBytes(Session["CropedPath"].ToString());
            //uploadedFile.InputStream.Read(imageSize, 0, (int)uploadedFile.ContentLength);
            UploadedFiles fu = new UploadedFiles();
            fu.DocumentFor = documentFor;
            fu.DocumentType = docType;
            fu.PreRegNum = RegNo;
            fu.DocumentData = imageSize;
            fu.DocumentName = Path.GetFileName(Session["CropedPath"].ToString());
            fu.DocumentSize = imageSize.Length.ToString();
            fu.UploadedDate = DateTime.Now.Date.ToString("dd/MM/yyyy");

            AMC.CreateOrUpdateUploadedFiles(fu);

            return Json(new { success = true, result = "Successfully uploaded the file!" }, "text/html", JsonRequestBehavior.AllowGet);
        }

        public UploadedFiles GetImageByPreRegNum(long Id)
        {
            try
            {
                UploadedFiles file = null;
                AdmissionManagementService ads = new AdmissionManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("PreRegNum", Id);
                criteria.Add("DocumentType", "Student Photo");
                Dictionary<long, IList<UploadedFiles>> UploadedFiles = ads.GetUploadedFilesListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteria);
                if (UploadedFiles != null && UploadedFiles.FirstOrDefault().Value != null && UploadedFiles.FirstOrDefault().Value.Count != 0)
                {
                    file = UploadedFiles.FirstOrDefault().Value[0];
                }
                return file;

            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        #endregion

        public ActionResult GenerateStudentProfilePDF(string Campus, string Grade, string Section)
        {
            if (!string.IsNullOrEmpty(Campus) && !string.IsNullOrEmpty(Grade))
            {
                return new Rotativa.ActionAsPdf("PrintStudentProfile", new { Campus = Campus, Grade = Grade, Section = Section })
                {
                    PageMargins = new Rotativa.Options.Margins(0, 0, 0, 0),
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    FileName = Campus + Grade + "profile.pdf"
                };
            }
            else return null;
        }

        public ActionResult PrintStudentProfile(string Campus, string Grade, string Section)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                StudentProfilePrint obj = new StudentProfilePrint();
                obj.Campus = Campus;
                obj.Grade = Grade;
                obj.Section = !string.IsNullOrEmpty(Section) ? Section : "";
                obj.AcademicYear = GetCurrentAcademicYear(Grade);
                criteria.Add("Campus", Campus);
                criteria.Add("Grade", Grade);
                if (!string.IsNullOrEmpty(Section)) criteria.Add("Section", Section);
                criteria.Add("AcademicYear", obj.AcademicYear);
                criteria.Add("AdmissionStatus", "Registered");
                Dictionary<long, IList<StudentTemplateView>> sdt = AMC.GetStudentTemplateViewListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
                obj.stdDtls = sdt.FirstOrDefault().Value;
                return PartialView("PrintStudentProfile", obj);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw;
            }

        }

        public ActionResult AcademicStudentReport()
        {
            MastersService ms = new MastersService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            Dictionary<long, IList<CampusMaster>> CampusMaster = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            ViewBag.campusddl = CampusMaster.First().Value;
            #region BreadCrumb
            string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
            #endregion
            return View();
        }


        #region for Kiosk Enquiry Grid
        public ActionResult GetKioskEnquiryList(string EnquiryFromDate, string EnquirytoDate, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                AdmissionManagementService ams = new AdmissionManagementService();
                //if (!string.IsNullOrEmpty(EnquiryFromDate)) {criteria.Add }
                Dictionary<long, IList<KioskEnquiryDetails>> Ked = ams.GetKioskEnquiryDetailsListWithPagingAndCriteria(0, 9999, string.Empty, "Asc", criteria);
                if (Ked != null && Ked.Count > 0 && Ked.FirstOrDefault().Key > 0)
                {
                    long totalrecords = Ked.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,

                        rows = (from items in Ked.First().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[] {
                                       items.Enq_Id.ToString(),
                                       items.Enq_Number,
                                       items.StudentName,
                                       items.AcademicYear,
                                       items.Grade,
                                       items.Campus,
                                       items.DOB,
                                       items.EmailId,
                                       items.FatherName,
                                       items.MotherName,
                                       items.Gender,
                                       items.Phone,
                                       items.Locality,
                                       items.Address,
                                       
                    
                              }
                                })
                    };
                    return Json(jsondat, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    
                     return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw;
            }
        }
        #endregion



        public ActionResult AcademicStudentReportJqgrid(string Campus, string Grade, string Section, string ExportType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    if (!string.IsNullOrEmpty(Campus)) { criteria.Add("Campus", Campus); }
                    if (!string.IsNullOrEmpty(Grade)) criteria.Add("Grade", Grade);
                    if (!string.IsNullOrEmpty(Section)) criteria.Add("Section", Section);
                    criteria.Add("AcademicYear", GetCurrentAcademicYear(Grade));
                    criteria.Add("AdmissionStatus", "Registered");
                    sord = sord == "desc" ? "Desc" : "Asc";
                    Dictionary<long, IList<StudentTemplateView>> sdt = AMC.GetStudentTemplateViewListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    if (sdt != null && sdt.Count > 0 && sdt.FirstOrDefault().Key > 0)
                    {
                        long totalrecords = sdt.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,

                            rows = (from items in sdt.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                                        items.Id.ToString(),
                              items.PreRegNum.ToString(),
                              items.Campus,
                              items.Grade,
                              items.Section,
                              items.Name+" "+items.Initial,
                              items.NewId,
                              "<img width='50px;' height='50px;'  src='uploaddisplay1?Id=" + items.PreRegNum + "' id='Student Photo' />"
                              }
                                    })
                        };
                        return Json(jsondat, JsonRequestBehavior.AllowGet);
                    }
                    else return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;

            }
        }

        public Int32 GetGradeNumberForIDNumber(string grade)
        {
            Int32 w = 0;            // grade
            switch (grade)
            {
                case "I":
                    w = 1;
                    break;
                case "II":
                    w = 2;
                    break;
                case "III":
                    w = 3;
                    break;
                case "IV":
                    w = 4;
                    break;
                case "V":
                    w = 5;
                    break;
                case "VI":
                    w = 6;
                    break;
                case "VII":
                    w = 7;
                    break;
                case "VIII":
                    w = 8;
                    break;
                case "IX":
                    w = 9;
                    break;
                case "X":
                    w = 10;
                    break;
                case "XI":
                    w = 11;
                    break;
                case "XII":
                    w = 12;
                    break;
                case "UKG":
                    w = 0;
                    break;
                case "LKG":
                    w = -1;
                    break;
                case "PreKG":
                    w = -2;
                    break;
                default:
                    w = -3;
                    break;
            }
            return w;
        }

        public string StudentIdNumberLogic(string campus, string feeStructure, string grade, string acYear)
        { //switch case

            string cam = "", NewId = "";
            if (campus == "CHENNAI CITY" || campus == "CHENNAI MAIN")
            { cam = "'CHENNAI CITY','CHENNAI MAIN'"; }
            else if (campus == "IB MAIN" || campus == "IB KG" || campus == "TIPS SARAN" || campus == "CBSE MAIN")
            { cam = "'IB MAIN','IB KG','TIPS SARAN','CBSE MAIN'"; }
            else if (campus == "KARUR" || campus == "KARUR KG" || campus == "KARUR CBSE")
            { cam = "'KARUR','KARUR KG','KARUR CBSE'"; }
            else if (campus == "TIRUPUR" || campus == "TIRUPUR KG" || campus == "TIRUPUR CBSE")
            { cam = "'TIRUPUR','TIRUPUR KG','TIRUPUR CBSE'"; }
            else if (campus == "ERNAKULAM" || campus == "ERNAKULAM KG")
            { cam = "'ERNAKULAM','ERNAKULAM KG'"; }
            else { cam = campus; }

            long count = AMC.StudentIdCount(grade, feeStructure, cam);            //for getting the maximum id no
            count = count + 1;
            string z = count.ToString();
            if (z.Length == 1)
            {
                z = "00" + z;
            }
            else if (z.Length == 2)
            {
                z = "0" + z;
            }

            string x = string.Empty;
            string grd = string.Empty;
            if (campus == "CHENNAI SWC" || campus == "CBE SWC")
            {
                grd = grade == "Group 1" ? "G1" : "G2";
            }
            else
            {
                var acadyr = acYear.Substring(2, 2);
                Int32 w = GetGradeNumberForIDNumber(grade);
                x = (Convert.ToInt32(acadyr) - Convert.ToInt32(w)).ToString();
                if (x.Length == 1)
                {
                    x = "0" + x;
                }
            }

            switch (campus)
            {
                case "IB MAIN":
                    NewId = "I" + feeStructure + "-" + x + "-" + z;
                    break;
                case "IB KG":
                    NewId = "I" + feeStructure + "-" + x + "-" + z;
                    break;
                case "CBSE MAIN":
                    NewId = "I" + feeStructure + "-" + x + "-" + z;
                    break;
                case "ERNAKULAM":
                    NewId = "E" + feeStructure + "-" + x + "-" + z;
                    break;
                case "ERNAKULAM KG":
                    NewId = "E" + feeStructure + "-" + x + "-" + z;
                    break;
                case "TIRUPUR":
                    NewId = "T" + feeStructure + "-" + x + "-" + z;
                    break;
                case "TIRUPUR KG":
                    NewId = "T" + feeStructure + "-" + x + "-" + z;
                    break;
                case "TIRUPUR CBSE":
                    NewId = "T" + feeStructure + "-" + x + "-" + z;
                    break;
                case "CHENNAI CITY":
                    NewId = "C" + feeStructure + "-" + x + "-" + z;
                    break;
                case "CHENNAI MAIN":
                    NewId = "C" + feeStructure + "-" + x + "-" + z;
                    break;
                case "KARUR":
                    NewId = "K" + feeStructure + "-" + x + "-" + z;
                    break;
                case "KARUR KG":
                    NewId = "K" + feeStructure + "-" + x + "-" + z;
                    break;
                case "KARUR CBSE":
                    NewId = "K" + feeStructure + "-" + x + "-" + z;
                    break;
                case "TIPS SARAN":
                    NewId = "H" + feeStructure + "-" + x + "-" + z;
                    break;
                case "CHENNAI SWC":
                    NewId = "CHESW-" + grd + "-" + z;
                    break;
                case "CBE SWC":
                    NewId = "CBESW-" + grd + "-" + z;
                    break;
                default:
                    NewId = ""; break;
            }
            return NewId;
        }
        public ActionResult CheckTCRequest(string NewId, long PreRegNo)
        {
            AdmissionManagementService ads = new AdmissionManagementService();
            TCRequestDetails tcreq = ads.GetTCRequestDetailsByPreRegNo(PreRegNo);
            if (tcreq != null)
            {
                if (tcreq.NewId == NewId)
                {
                    return Json("failed", JsonRequestBehavior.AllowGet);
                }
            }
            return Json("success", JsonRequestBehavior.AllowGet);
        }

    }

    public partial class WindowsService1
    {
        public void GetFailedEmailList(long[] bulkIds)
        {
            AdmissionManagementService ads = new AdmissionManagementService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();

            Dictionary<long, IList<TIPS.Entities.AdmissionEntities.EmailLog>> Emaillog;
            if (bulkIds[0] != 0) { criteria.Add("Id", bulkIds); }
            criteria.Add("IsSent", false);
            Emaillog = ads.GetEmailLogListWithPagingAndCriteria(0, 9000, "", "", criteria);

            Task taskA = Task.Factory.StartNew(() => NewTaskEmailSendInLoop(Emaillog.FirstOrDefault().Value, ads));
        }

        public void NewTaskEmailSendInLoop(IList<EmailLog> Emaillog, AdmissionManagementService ads)
        {
            if (Emaillog != null && Emaillog.Count > 0 && ads != null)
            {
                foreach (EmailLog var in Emaillog)
                {
                    try
                    {
                        System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();

                        Dictionary<string, object> criteria3 = new Dictionary<string, object>();
                        if (!string.IsNullOrEmpty(var.Attachment))
                        {
                            criteria3.Add("Id", Convert.ToInt64(var.Attachment));
                            Dictionary<long, IList<EmailAttachment>> emailattachment = ads.GetEmailAttachmentListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria3);
                            MemoryStream ms = new MemoryStream(emailattachment.First().Value[0].Attachment);
                            Attachment mailAttach = new Attachment(ms, emailattachment.First().Value[0].AttachmentName.ToString());  //Data posted from form
                            mail.Attachments.Add(mailAttach);
                        }
                        mail.Subject = var.Subject;
                        mail.Body = var.Message;
                        mail.IsBodyHtml = true;
                        SmtpClient smtp = new SmtpClient("localhost", 25);
                        smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address  
                        //Or your Smtp Email ID and Password  
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.EnableSsl = true;
                        if (!string.IsNullOrEmpty(var.EmailTo))
                        {
                            if (var.EmailTo.Contains(',')) { var.EmailTo = var.EmailTo.Replace(",", ""); }
                            if (Regex.IsMatch(var.EmailTo,
        @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
        @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$",
        RegexOptions.IgnoreCase))
                            { mail.To.Add(var.EmailTo); }
                            // else { mail.To.Add(var.EmailTo); }
                        }
                        if (!string.IsNullOrEmpty(var.EmailFrom))
                        {
                            if (Regex.IsMatch(var.EmailFrom,
        @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
        @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$",
        RegexOptions.IgnoreCase))
                                mail.From = new MailAddress(var.EmailFrom);
                        }

                        IEnumerable<CampusEmailId> cem = GetPassworddByEmailId(var.EmailFrom, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
                        if (cem != null)
                        {
                            smtp.Credentials = new System.Net.NetworkCredential
                            (cem.First().EmailId.ToString(), cem.First().Password.ToString());
                            try
                            {
                                if ((!string.IsNullOrEmpty(mail.From.ToString())) && (!string.IsNullOrEmpty(mail.To.ToString())))
                                {
                                    smtp.Send(mail);
                                    UpdateEmailStatus(var);
                                }
                            }
                            catch (Exception ex)
                            {
                                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                            }
                        }
                    }
                    catch (Exception ex)
                    { ExceptionPolicy.HandleException(ex, "AdmissionPolicy"); }
                }
            }
        }

        public void UpdateEmailStatus(TIPS.Entities.AdmissionEntities.EmailLog ce)
        {
            AdmissionManagementService ads = new AdmissionManagementService();
            ce.IsSent = true;
            ads.CreateOrUpdateEmailLog(ce);
        }

        public IList<CampusEmailId> GetPassworddByEmailId(string EmailId, string server)
        {
            UserService us = new UserService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("EmailId", EmailId);
            criteria.Add("Server", server);
            IList<CampusEmailId> Password = us.GetCampusEmailListWithPaging(0, 1000, string.Empty, string.Empty, criteria);

            if (Password.Count() != 0)
            {
                return Password.ToList();
            }
            else
            {
                return null;
            }
        }
       

        
    }
   

}






