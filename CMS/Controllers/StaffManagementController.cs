using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using TIPS.Entities;
using TIPS.Entities.AdmissionEntities;
using TIPS.Entities.StaffManagementEntities;
using TIPS.Service;
using TIPS.ServiceContract;
using System.Globalization;
using System.Text;
using System.Data.OleDb;
using System.Data;
using CMS.Helpers;
using CustomAuthentication;
using Rotativa;
using Rotativa.Options;
using TIPS.Entities.InboxEntities;
using System.Net;
using TIPS.Entities.TransportEntities;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using TIPS.Entities.BioMetricsEntities;

namespace CMS.Controllers
{
    public class StaffManagementController : CMS.Controllers.PDFGeneration.PdfViewController
    {
        string info = string.Empty;
        #region "Object Declaration"
        StaffManagementService smsObj = new StaffManagementService();
        IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);

        #endregion "End"

        public ActionResult NewEmployment(string id)
        {
            string userId = base.ValidateUser();
            if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
            StaffManagementService sms = new StaffManagementService();

            FillViewBag();

            if (id == null)
            {
                Dictionary<long, IList<StaffRequestNumDetails>> prd = sms.GetStaffRequestNumDetailsListWithPaging(0, 10000, string.Empty, string.Empty, null);

                var id1 = prd.First().Value[0].PreRegNum + 1;
                ViewBag.RequestNum = id1;
                Session["Reqnum"] = id1;
                ViewBag.Date = DateTime.Now.ToShortDateString();
                ViewBag.Time = DateTime.Now.ToShortTimeString();
                StaffRequestNumDetails srn = new StaffRequestNumDetails();
                srn.PreRegNum = id1;

                srn.Id = prd.First().Value[0].Id;
                srn.Date = DateTime.Now.ToShortDateString();
                srn.Time = DateTime.Now.ToShortTimeString();
                sms.CreateOrUpdateStaffRequestNumDetails(srn);

                return View();
            }
            else
            {
                StaffDetails StaffDetails = new StaffDetails();

                StaffDetails = sms.GetStaffDetailsId(Convert.ToInt32(id));
                ViewBag.RequestNum = StaffDetails.PreRegNum;
                ViewBag.Date = DateTime.Now.ToShortDateString();
                ViewBag.Time = DateTime.Now.ToShortTimeString();
                Session["Reqnum"] = StaffDetails.PreRegNum;
                return View(StaffDetails);
            }
        }

        [HttpPost]
        public ActionResult NewEmployment(StaffDetails sd, HttpPostedFileBase file1)
        {
            StaffManagementService sms = new StaffManagementService();

            if (Request.Form["DocUpload"] == "Upload")
            {
                AdmissionManagementService ads = new AdmissionManagementService();

                string path = file1.InputStream.ToString();
                byte[] imageSize = new byte[file1.ContentLength];

                file1.InputStream.Read(imageSize, 0, (int)file1.ContentLength);
                UploadedFiles fu = new UploadedFiles();
                fu.DocumentFor = "Staff";
                fu.PreRegNum = Convert.ToInt32(Session["Reqnum"]);
                fu.DocumentData = imageSize;
                fu.DocumentName = file1.FileName;
                //   fu.DocumentType = sd.DocumentType;
                fu.DocumentSize = file1.ContentLength.ToString();
                fu.UploadedDate = DateTime.Now.Date.ToString("dd/MM/yyyy");
                ads.CreateOrUpdateUploadedFiles(fu);
            }

            if (Request.Form["Save"] == "submit")
            {
                Dictionary<long, IList<StaffDetails>> staffdetails;
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("PreRegNum", Convert.ToInt32(Session["Reqnum"]));
                staffdetails = sms.GetStaffDetailsListWithPaging(0, 10, string.Empty, string.Empty, criteria);

                if (staffdetails.First().Value.Count != 0)
                {
                    sd.Id = staffdetails.First().Value[0].Id;
                }

                sd.PreRegNum = Convert.ToInt32(Session["Reqnum"]);
                sms.CreateOrUpdateStaffDetails(sd);
            }

            ViewBag.RequestNum = Session["Reqnum"];
            ViewBag.Date = DateTime.Now.ToShortDateString();
            ViewBag.Time = DateTime.Now.ToShortTimeString();

            FillViewBag();

            return View();
        }

        public string FillViewBag()
        {
            MastersService ms = new MastersService();
            StaffManagementService sms = new StaffManagementService();
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
            Dictionary<long, IList<BloodGroupMaster>> BloodGroupMaster = ms.GetBloodGroupMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            Dictionary<long, IList<IssueGroupMaster>> IssueGroupMaster = ms.GetIssueGroupMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            Dictionary<long, IList<StaffDepartmentMaster>> DepartmentMaster = ms.GetStaffDepartmentMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);

            criteria.Clear();
            criteria.Add("DocumentFor", "Staff");
            Dictionary<long, IList<DocumentTypeMaster>> DocumentTypeMaster = ms.GetDocumentTypeMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            criteria.Clear();
            Dictionary<long, IList<DesignationMaster>> DesignationMaster = ms.GetDesignationMasterListWithPagingAndCriteria(0, 200, string.Empty, string.Empty, criteria);
            Dictionary<long, IList<StaffTypeMaster>> StaffTypeMaster = ms.GetStaffTypeMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);

            ViewBag.campusddl = CampusMaster.First().Value;
            ViewBag.bloodgrpddl = BloodGroupMaster.First().Value;
            ViewBag.departmentddl = DepartmentMaster.First().Value;
            ViewBag.documentddl = DocumentTypeMaster.First().Value;
            ViewBag.designationddl = DesignationMaster.First().Value;
            ViewBag.stafftypeddl = StaffTypeMaster.First().Value;
            return "";
        }

        public ActionResult StaffDisplay()
        {
            string userId = base.ValidateUser();
            if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
            FillViewBag();
            #region BreadCrumb
            string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
            #endregion
            return View();
        }

        //public ActionResult StaffListJqGrid(string campus, string designation, string department, string stat, string appname, string idno, string flag, string type, int rows, string sidx, string sord, int? page = 1)
        //{
        //    try
        //    {
        //        TIPS.Entities.User sessionUser = (TIPS.Entities.User)Session["objUser"];
        //        StaffManagementService sms = new StaffManagementService();
        //        Dictionary<string, object> criteria = new Dictionary<string, object>();
        //        string colName = string.Empty; string[] values = new string[1];
        //        {
        //            var usrcmp = Session["UserCampus"] as IEnumerable<string>;
        //            if (!string.IsNullOrWhiteSpace(campus))
        //            {
        //                criteria.Add("Campus", campus);
        //            }
        //            else
        //            {
        //                if (usrcmp != null && usrcmp.Count() != 0)
        //                {
        //                    if (usrcmp.First() != null)   // to check if the usrcmp obj is null or with data
        //                    {
        //                        criteria.Add("Campus", usrcmp);
        //                    }
        //                }

        //            }
        //            if (!string.IsNullOrWhiteSpace(designation))criteria.Add("Designation", designation);

        //            if (!string.IsNullOrWhiteSpace(department))criteria.Add("Department", department);

        //            if (!string.IsNullOrWhiteSpace(appname))criteria.Add("Name", appname);

        //            if (!string.IsNullOrWhiteSpace(idno))criteria.Add("IdNumber", idno);

        //            if (type == "new")
        //            {
        //                if (Session["staffapproverrole"].ToString() == "STM-APP")
        //                {
        //                    string[] status = { "Sent For Approval", "On Hold", "Call For Interview" };
        //                    criteria.Add("Status", status);
        //                }
        //                else
        //                {
        //                    string[] status = { "New Registration", "Sent For Approval", "On Hold", "Call For Interview" };
        //                    criteria.Add("Status", status);
        //                }
        //            }

        //            if (type == "old")
        //            {
        //                if (string.IsNullOrWhiteSpace(stat))
        //                {
        //                    string[] status = { "Registered" };
        //                    criteria.Add("Status", status);
        //                }
        //                else
        //                {
        //                    string[] status = { stat.ToString() };
        //                    criteria.Add("Status", status);
        //                }
        //            }

        //            if (string.Equals(sessionUser.UserType != null ? sessionUser.UserType : string.Empty, "Staff", StringComparison.CurrentCultureIgnoreCase))
        //            {
        //                colName = "IdNumber";
        //                values[0] = sessionUser.EmployeeId != null ? sessionUser.EmployeeId : string.Empty;
        //            }
        //            Dictionary<long, IList<TIPS.Entities.StaffManagementEntities.StaffDetailsView>> StaffDetails;

        //            StaffDetails = sms.GetStaffDetailsListWithPagingAndCriteriaWithAlias(page - 1, rows, sidx, sord, colName, values, criteria, null);

        //            if (StaffDetails.Count > 0)
        //            {
        //                long totalrecords = StaffDetails.First().Key;
        //                int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);

        //                var jsondat = new
        //                {
        //                    total = totalPages,
        //                    page = page,
        //                    records = totalrecords,

        //                    rows = (from items in StaffDetails.First().Value
        //                            select new
        //                            {
        //                                i = 2,
        //                                cell = new string[] {
        //                            items.PreRegNum.ToString(),

        //                    String.Format(@"<a style='color:#034af3;text-decoration:underline' href= '/StaffManagement/ApplicationForm?id="+items.Id+"'  >{0}</a>",items.Name),
        //                    items.IdNumber,      
        //                    items.Campus,
        //                    items.Designation,
        //                    items.Department,
        //                    items.Gender,
        //                    items.Status,
        //                    items.Id.ToString()
        //                    }
        //                            })
        //                };
        //                return Json(jsondat, JsonRequestBehavior.AllowGet);
        //            }
        //            else
        //            {
        //                return null;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
        //        throw ex;
        //    }
        //}

        public ActionResult StaffListJqGrid(string PreRegNum, string Name, string IdNumber, string campus, string designation, string department, string Gender, string Status, string stat, string appname, string idno, string flag, string type, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                TIPS.Entities.User sessionUser = (TIPS.Entities.User)Session["objUser"];
                StaffManagementService sms = new StaffManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                string colName = string.Empty; string[] values = new string[1];
                {
                    var usrcmp = Session["UserCampus"] as IEnumerable<string>;
                    if (!string.IsNullOrWhiteSpace(campus))
                    {
                        criteria.Add("Campus", campus);
                    }
                    else
                    {
                        if (usrcmp != null && usrcmp.Count() != 0)
                        {
                            if (usrcmp.First() != null)   // to check if the usrcmp obj is null or with data
                            {
                                usrcmp = usrcmp.Concat(new string[] { "" });
                                criteria.Add("Campus", usrcmp);
                            }
                        }

                    }
                    //criteria.Add("WorkingType", "Staff");
                    if (!string.IsNullOrWhiteSpace(designation)) criteria.Add("Designation", designation);
                    if (!string.IsNullOrWhiteSpace(department)) criteria.Add("Department", department);
                    if (!string.IsNullOrWhiteSpace(appname)) criteria.Add("Name", appname);
                    if (!string.IsNullOrWhiteSpace(idno)) criteria.Add("IdNumber", idno);
                    if (!string.IsNullOrWhiteSpace(PreRegNum)) criteria.Add("PreRegNum", Convert.ToInt32(PreRegNum));//Tool bar search
                    if (!string.IsNullOrWhiteSpace(Name)) criteria.Add("Name", Name);//Tool bar search
                    if (!string.IsNullOrWhiteSpace(IdNumber)) criteria.Add("IdNumber", IdNumber);//Tool bar search
                    if (!string.IsNullOrWhiteSpace(Gender)) criteria.Add("Gender", Gender);//Tool bar search
                    if (!string.IsNullOrWhiteSpace(Status)) criteria.Add("Status", Status);//Tool bar search

                    if (type == "new")
                    {
                        if (Session["staffapproverrole"].ToString() == "STM-APP")
                        {
                            string[] status = { "Sent For Approval", "On Hold", "Call For Interview" };
                            if (string.IsNullOrEmpty(Status)) { criteria.Add("Status", status); }
                        }
                        else
                        {
                            string[] status = { "New Registration", "Sent For Approval", "On Hold", "Call For Interview", "NewStaffEnquiry" };
                            if (string.IsNullOrEmpty(Status)) { criteria.Add("Status", status); }
                        }
                    }

                    if (type == "old")
                    {
                        if (string.IsNullOrWhiteSpace(stat))
                        {
                            string[] status = { "Registered" };
                            if (string.IsNullOrEmpty(Status)) { criteria.Add("Status", status); }
                        }
                        else
                        {
                            string[] status = { stat.ToString() };
                            if (string.IsNullOrEmpty(Status)) { criteria.Add("Status", status); }
                        }
                    }

                    if (string.Equals(sessionUser.UserType != null ? sessionUser.UserType : string.Empty, "Staff", StringComparison.CurrentCultureIgnoreCase))
                    {
                        colName = "IdNumber";
                        values[0] = sessionUser.EmployeeId != null ? sessionUser.EmployeeId : string.Empty;
                    }
                    Dictionary<long, IList<TIPS.Entities.StaffManagementEntities.StaffDetailsView>> StaffDetails;

                    StaffDetails = sms.GetStaffDetailsListWithPagingAndCriteriaWithAlias(page - 1, rows, sidx, sord, colName, values, criteria, null);

                    if (StaffDetails.Count > 0)
                    {
                        long totalrecords = StaffDetails.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);

                        var jsondat = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,

                            rows = (from items in StaffDetails.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                                    items.PreRegNum.ToString(),
                             
                            String.Format(@"<a style='color:#034af3;text-decoration:underline' href= '/StaffManagement/ApplicationForm?id="+items.Id+"'  >{0}</a>",items.Name),
                            type == "new"?items.TempIdNumber:items.IdNumber,      
                            items.Campus,
                            items.Designation,
                            items.Department,
                            items.Gender,
                            items.Status,
                            items.Id.ToString()
                            }
                                    })
                        };
                        return Json(jsondat, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var AssLst = new { rows = (new { cell = new string[] { } }) };
                        return Json(AssLst, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult StaffListPopupJqGrid(string campus, string designation, string appname, string idno, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();

                Dictionary<string, object> criteria = new Dictionary<string, object>();

                var usrcmp = Session["UserCampus"] as IEnumerable<string>;
                if (usrcmp.Count() != 0)
                {
                    if (usrcmp.First() != null)            // to check if the usrcmp obj is null or with data
                    {
                        if (!string.IsNullOrWhiteSpace(campus))
                        {
                            criteria.Add("Campus", campus);
                        }
                        else
                        {
                            criteria.Add("Campus", usrcmp);
                        }
                    }
                }
                else { criteria.Add("Campus", "nocampus"); }

                if (!string.IsNullOrWhiteSpace(designation))
                {
                    criteria.Add("Designation", designation);
                }

                if (!string.IsNullOrWhiteSpace(appname))
                {
                    criteria.Add("Name", appname);
                }

                if (!string.IsNullOrWhiteSpace(idno))
                {
                    criteria.Add("IdNumber", idno);
                }

                string[] status = { "Registered" };
                criteria.Add("Status", status);


                Dictionary<long, IList<TIPS.Entities.StaffManagementEntities.StaffDetailsView>> StaffDetails;

                StaffDetails = sms.GetStaffDetailsViewListWithPaging(page - 1, rows, sidx, sord, criteria);

                long totalrecords = StaffDetails.First().Key;
                int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);

                var jsondat = new
                {
                    total = totalPages,
                    page = page,
                    records = totalrecords,

                    rows = (from items in StaffDetails.First().Value
                            select new
                            {
                                i = 2,
                                cell = new string[] {
                                    items.PreRegNum.ToString(),
                                items.Name,
                            items.IdNumber,      
                            items.Campus,
                            items.Designation
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

        public ActionResult StaffDetailsSearch()
        {
            FillViewBag();
            return View();
        }


        [HttpPost]
        public ActionResult FileUpload(StaffDetails sd)
        {
            return PartialView();
        }

        public JsonResult Documentsjqgrid(string id, string txtSearch, string idno, string name, string sect, string cname, string grad, string btype, int rows, string sidx, string sord, int? page)
        {
            try
            {
                AdmissionManagementService ads = new AdmissionManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("DocumentFor", "Staff");
                criteria.Add("PreRegNum", Convert.ToInt64(Session["Reqnum"]));
                Dictionary<long, IList<UploadedFilesView>> UploadedFilesview = ads.GetUploadedFilesViewListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                IList<UploadedFilesView> uploadList = new List<UploadedFilesView>();
                foreach (UploadedFilesView item in UploadedFilesview.FirstOrDefault().Value)
                {
                    if (item.DocumentName != "Salary Slip")
                    {
                        uploadList.Add(item);
                    }
                    else
                    {
                        if (item.DocumentName == "Salary Slip" && DateTime.Now.Month == item.MonthOfSalary)
                        {
                            uploadList.Add(item);
                        }
                    }
                }

                //if (UploadedFiles != null && UploadedFiles.Count > 0)
                {
                    long totalrecords = uploadList.Count;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,
                        rows = (from items in uploadList
                                select new
                                {
                                    i = 2,
                                    cell = new string[] {
                               items.DocumentType,
                              // String.Format(@"<a style='color:#034af3;text-decoration:underline' href= '/StaffManagement/uploaddisplay?Id="+items.Id+"' >{0}</a>",items.DocumentName),
                              String.Format(@"<a style='color:#034af3;text-decoration:underline' href= '/StaffManagement/uploaddisplay?Id="+items.Id+"' target='_Blank'>{0}</a>",items.DocumentName),
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

        //[HttpPost]
        //public ActionResult UploadDocuments(HttpPostedFileBase uploadedFile, string docType)
        //{
        //    HttpPostedFileBase theFile = HttpContext.Request.Files["uploadedFile"];
        //    if (theFile.ContentLength != 0)
        //    {
        //        string path = uploadedFile.InputStream.ToString();
        //        byte[] imageSize = new byte[uploadedFile.ContentLength];

        //        uploadedFile.InputStream.Read(imageSize, 0, (int)uploadedFile.ContentLength);
        //        UploadedFiles fu = new UploadedFiles();
        //        fu.DocumentFor = "Staff";
        //        fu.DocumentType = docType;
        //        fu.PreRegNum = Convert.ToInt32(Session["Reqnum"]);
        //        fu.DocumentData = imageSize;
        //        fu.DocumentName = theFile.FileName;
        //        //   fu.DocumentType = sd.DocumentType;
        //        fu.DocumentSize = theFile.ContentLength.ToString();
        //        fu.UploadedDate = DateTime.Now.Date.ToString("dd/MM/yyyy");
        //        AdmissionManagementService ads = new AdmissionManagementService();
        //        ads.CreateOrUpdateUploadedFiles(fu);

        //        return Json(new { success = true, result = "Successfully uploaded the file!" }, "text/html", JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json(new { success = false, result = "You have uploded the empty file. Please upload the correct file." }, "text/x-json", JsonRequestBehavior.AllowGet);
        //    }
        //}

        public ActionResult uploaddisplay(long Id)
        {
            try
            {
                AdmissionManagementService ads = new AdmissionManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Id", Id);

                Dictionary<long, IList<UploadedFiles>> UploadedFiles = ads.GetUploadedFilesListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteria);
                long tempId = Id;
                if (UploadedFiles.FirstOrDefault().Value[0].DocumentType == "Salary Slip")
                {
                    SalarySlip SalSlip = SalarySlipPdf(tempId);
                    TipsLogo(SalSlip, "TipsLogo.jpg");
                    TipsName(SalSlip, "TipsName.jpg");
                    TipsAddress(SalSlip, "TipsAddress.jpg");
                    return this.ViewPdf("", "SalarySlip", SalSlip, "Landscape", "SalarySlip");
                }
                else
                {
                    if (UploadedFiles != null && UploadedFiles.FirstOrDefault().Value != null)
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
                                // always prompt the user for downloading, set to true if you want 
                                // the browser to try to show the file inline
                                FileName = UploadedFiles.First().Value[0].DocumentName,
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
                    else
                    {
                        return null;
                    }
                }
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
                criteria.Add("DocumentFor", "Staff");
                criteria.Add("DocumentType", "Staff Photo");

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
                        //    IList<UploadedFiles> list = UploadedFiles;
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
                            //System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
                            //
                            //return File(doc.DocumentData, doc.DocumentType);
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

        public ActionResult StaffIdCard(string ReqNo)
        {
            try
            {
                Session["StaffIdCardData"] = ReqNo;

                ViewBag.StaffIdHtmlTag = IdHtmlTags();
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
                StaffManagementService sds = new StaffManagementService();
                var prereg = Session["StaffIdCardData"].ToString().Split(',');
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
                    criteria.Add("PreRegNum", Convert.ToInt32(id[i]));
                    //    criteria.Add("DocumentFor", "Staff");
                    Dictionary<long, IList<StaffDetails>> StaffDetails = sds.GetStaffDetailsListWithPaging(0, 10000, string.Empty, string.Empty, criteria);

                    if (StaffDetails.First().Value[0].BGRP == "Not Given")
                    {
                        StaffDetails.First().Value[0].BGRP = "";
                    }

                    Dictionary<long, IList<StaffDetails>> StaffDetails2 = null;
                    CampusEmailId campusemailiddtls2 = null;
                    UserService us = new UserService();
                    CampusEmailId campusemailiddtls = us.GetCampusEmailIdByCampusWithServer(StaffDetails.First().Value[0].Campus, "Test");
                    if (i + 1 < Convert.ToInt32(prereg.Count()))
                    {
                        Dictionary<string, object> criteria2 = new Dictionary<string, object>();
                        criteria2.Add("PreRegNum", Convert.ToInt32(id[i + 1]));
                        StaffDetails2 = sds.GetStaffDetailsListWithPaging(0, 10000, string.Empty, string.Empty, criteria2);
                        campusemailiddtls2 = us.GetCampusEmailIdByCampusWithServer(StaffDetails2.First().Value[0].Campus, "Test");
                        if (StaffDetails2.First().Value[0].BGRP == "Not Given")
                        {
                            StaffDetails2.First().Value[0].BGRP = "";
                        }
                    }
                    rowcnt = rowcnt + 1;
                    html.Append("<tr style='page-break-inside:avoid'>");
                    html.Append("<td style='padding-bottom:20px; padding-right:25px'>");
                    html.Append("<div style='width: 350px;'>");

                    html.Append("<span style='font-size: 10pt; float:left'><img src='../../Images/left-top-org.png' height='70' width='100'/><br/>    </span>");
                    html.Append("<span style='font-size: 10pt; float:left;padding-top:56px'><label for='identitycard' style='padding-left:24px; padding-top:100px;'>IDENTITY CARD</label><br/>    </span>");

                    html.Append("<span style='font-size: 10pt; float:right; padding-right:10px; padding-top:10px'><img src='../../Images/Tips Logo.JPG' height='45' width='100'/><br/>    </span></div>");
                    html.Append("<br />");
                    html.Append("<div style='width: 350px;display:table;'>");
                    html.Append("<table cellpadding='0' cellspacing='0' width='100%'><tr><td width='31%'>&nbsp;</td><td width='41%'></td>");
                    html.Append("<td rowspan=2 style='padding-left:0px;'><img src='" + Url.Action("uploaddisplay1", "StaffManagement", new { id = id[i] }) + "' id='image1' name='logoFile' width= '80' height=90' style='padding-top:0px;padding-right:0px;' /></td></tr>");
                    html.Append("<tr><td width='31%'><span style='float:left;padding-left:20px;'><strong>Emp. Name</strong><br/>");
                    html.Append("<strong>Emp. Id.</strong><br/>");
                    html.Append("<strong>Blood Group</strong><br/>");
                    html.Append("<strong>Emergency No.</strong></span></td>");
                    html.Append("<td width='41%'><span><strong>:&nbsp;" + StaffDetails.First().Value[0].Name + "</strong><br/>");
                    html.Append("<strong>:&nbsp;" + StaffDetails.First().Value[0].IdNumber + "</strong><br/>");
                    html.Append("<strong>:&nbsp;" + StaffDetails.First().Value[0].BGRP + "</strong><br/>");
                    html.Append("<strong>:&nbsp;" + StaffDetails.First().Value[0].EmergencyContactNo + "</strong></span></td><td></td>");
                    html.Append("</table><br/><br/>");
                    html.Append("</div>");
                    html.Append("<div id='onbottom' style='width: 350px;'>" + campusemailiddtls.SchoolName + ", " + campusemailiddtls.Address + " -" + campusemailiddtls.PinCode + " " + campusemailiddtls.PhoneNumber + " | " + campusemailiddtls.WebSiteName + "");
                    html.Append("</div>");
                    html.Append("<td/>");

                    if (i + 1 < Convert.ToInt32(prereg.Count()))
                    {
                        html.Append("<td style='padding-bottom:20px'>");
                        html.Append("<div style='width: 350px'>");

                        html.Append("<span style='font-size: 10pt; float:left'><img src='../../Images/left-top-org.png' width='100' height='70'/><br/>    </span>");
                        html.Append("<span style='font-size: 10pt; float:left;padding-top:56px'><label for='identitycard' style='padding-left:24px; padding-top:100px;'>IDENTITY CARD</label><br/>    </span>");
                        html.Append("<span style='font-size: 10pt; float:right; padding-right:10px; padding-top:10px'><img src='../../Images/Tips Logo.JPG' width='100' height='45'/><br/>    </span></div>");
                        html.Append("<br />");
                        html.Append("<div style='width: 350px;display:table;'>");
                        html.Append("<table cellpadding='0' cellspacing='0' width='100%'><tr><td width='31%'>&nbsp;</td><td width='41%'></td>");
                        html.Append("<td rowspan=2 style='padding-left:0px;'><img src='" + Url.Action("uploaddisplay1", "StaffManagement", new { id = id[i] }) + "' id='image1' name='logoFile' width= '80' height='90' style='padding-top:0px;padding-right:0px;' /></td></tr>");
                        html.Append("<tr><td width='31%'><span style='float:left;padding-left:20px;'><strong>Emp. Name</strong><br/>");
                        html.Append("<strong>Emp. Id.</strong><br/>");
                        html.Append("<strong>Blood Group</strong><br/>");
                        html.Append("<strong>Emergency No.</strong></span></td>");
                        html.Append("<td width='41%'><span><strong>:&nbsp;" + StaffDetails2.First().Value[0].Name + "</strong><br/>");
                        html.Append("<strong>:&nbsp;" + StaffDetails2.First().Value[0].IdNumber + "</strong><br/>");
                        html.Append("<strong>:&nbsp;" + StaffDetails2.First().Value[0].BGRP + "</strong><br/>");
                        html.Append("<strong>:&nbsp;" + StaffDetails2.First().Value[0].EmergencyContactNo + "</strong></span></td><td></td>");
                        html.Append("</table><br/><br/>");
                        html.Append("</div>");
                        html.Append("<div id='onbottom' style='width: 350px;'>" + campusemailiddtls2.SchoolName + ", " + campusemailiddtls2.Address + " -" + campusemailiddtls2.PinCode + " " + campusemailiddtls2.PhoneNumber + " | " + campusemailiddtls2.WebSiteName + "");
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

        public string XCDIdHtmlTags()
        {
            try
            {
                StaffManagementService sds = new StaffManagementService();
                var prereg = Session["StaffIdCardData"].ToString().Split(',');
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
                    criteria.Add("PreRegNum", Convert.ToInt32(id[i]));
                    //    criteria.Add("DocumentFor", "Staff");
                    Dictionary<long, IList<StaffDetails>> StaffDetails = sds.GetStaffDetailsListWithPaging(0, 10000, string.Empty, string.Empty, criteria);

                    if (StaffDetails.First().Value[0].BGRP == "Not Given")
                    {
                        StaffDetails.First().Value[0].BGRP = "";
                    }

                    Dictionary<long, IList<StaffDetails>> StaffDetails2 = null;

                    if (i + 1 < Convert.ToInt32(prereg.Count()))
                    {
                        Dictionary<string, object> criteria2 = new Dictionary<string, object>();
                        criteria2.Add("PreRegNum", Convert.ToInt32(id[i + 1]));
                        StaffDetails2 = sds.GetStaffDetailsListWithPaging(0, 10000, string.Empty, string.Empty, criteria2);

                        if (StaffDetails2.First().Value[0].BGRP == "Not Given")
                        {
                            StaffDetails2.First().Value[0].BGRP = "";
                        }
                    }
                    rowcnt = rowcnt + 1;
                    html.Append("<tr style='page-break-inside:avoid'>");
                    html.Append("<td style='padding-bottom:20px; padding-right:25px'>");
                    html.Append("<div style='width: 350px;'>");
                    html.Append("<span style='font-size: 10pt; float:left'><img src='../../Images/itgrowth.jpg' height='70';width='80'/><br/>    </span>");
                    html.Append("<span style='font-size: 10pt; float:right; padding-right:10px; padding-top:10px'><img src='../../Images/xcdlogo.png' height='45';width='30'/><br/>    </span>");
                    html.Append("<br />");
                    html.Append("<span style='font-size: 8pt; padding-left:30px'>  <img src='" + Url.Action("uploaddisplay1", "StaffManagement", new { id = id[i] }) + "' id='image1' name='logoFile' width= '80' height='80' style='padding-top:16px' /> </span><br />");
                    html.Append("<span style='font-family: Arial; font-size:12px'><div style=' padding-top:3px; padding-left:130px; height: 7px'><strong> " + StaffDetails.First().Value[0].Name + "  </strong></div><br />");
                    html.Append("<div style=' padding-left:130px; height: 7px'><strong>" + StaffDetails.First().Value[0].IdNumber + "</strong></div><br />");
                    html.Append("<div style=' padding-left:130px; height: 7px'><strong>" + StaffDetails.First().Value[0].BGRP + "</strong></div><br />");
                    html.Append("<div style=' padding-left:130px; height: 7px'><strong>" + StaffDetails.First().Value[0].PhoneNo + "</strong></div><br />");
                    html.Append("<span style='font-size: 10pt; float:right'><img src='../../Images/xcdaddr.png'   style='width:350px; height:30px;'/> </span>");
                    html.Append("<div/>");
                    html.Append("<td/>");

                    if (i + 1 < Convert.ToInt32(prereg.Count()))
                    {
                        html.Append("<td style='padding-bottom:20px'>");
                        html.Append("<div style='width: 350px'>");

                        html.Append("<span style='font-size: 10pt; float:left'><img src='../../Images/itgrowth.jpg' height='70';width='80'/><br/>    </span>");

                        html.Append("<span style='font-size: 10pt; float:right; padding-right:10px; padding-top:10px'><img src='../../Images/xcdlogo.png' height='45';width='30'/><br/>    </span>");
                        html.Append("<br />");
                        html.Append("<span style='font-size: 8pt; padding-left:30px'>  <img src='" + Url.Action("uploaddisplay1", "StaffManagement", new { id = id[i + 1] }) + "' id='image1' name='logoFile' width= '80' height='80' style='padding-top:16px'/> </span><br />");
                        html.Append("<span style='font-family: Arial; font-size:12px'><div style=' padding-top:3px; padding-left:130px; height: 7px'><strong> " + StaffDetails2.First().Value[0].Name + "<br />  </strong></div><br/>");
                        html.Append("<div style='padding-left:130px; height: 7px'><strong>" + StaffDetails2.First().Value[0].IdNumber + "</strong></div><br />");
                        html.Append("<div style='padding-left:130px; height: 7px'><strong>" + StaffDetails2.First().Value[0].BGRP + "</strong></div><br />");
                        html.Append("<div style='padding-left:130px; height: 7px'><strong>" + StaffDetails2.First().Value[0].PhoneNo + "</strong></div><br />");
                        html.Append("<span style='font-size: 10pt; float:right'><img src='../../Images/xcdaddr.png'   style='width:350px; height:30px;'/> </span>");

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

        public ActionResult ApplicationForm(string id)
        {
            try
            {
                FillViewBag();
                Session["status"] = "";
                StaffManagementService sms = new StaffManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("IsActive", true);
                Dictionary<long, IList<StaffGroupMaster>> StaffGroupMaster = sms.GetStaffGroupMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                Dictionary<long, IList<StaffSubGroupMaster>> StaffSubGroupMaster = sms.GetStaffSubGroupMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                ViewBag.staffgroupddl = StaffGroupMaster.First().Value;
                ViewBag.staffsubgroupddl = StaffSubGroupMaster.First().Value;
                criteria.Clear();
                if (id == null)
                {
                    Dictionary<long, IList<StaffRequestNumDetails>> prd = sms.GetStaffRequestNumDetailsListWithPaging(0, 10000, string.Empty, string.Empty, null);
                    var id1 = prd.First().Value[0].PreRegNum + 1;
                    ViewBag.RequestNum = id1;
                    Session["Reqnum"] = id1;
                    ViewBag.Date = DateTime.Now.ToShortDateString();
                    ViewBag.Time = DateTime.Now.ToShortTimeString();
                    StaffRequestNumDetails srn = new StaffRequestNumDetails();
                    srn.PreRegNum = id1;
                    Session["status"] = "New Registration";
                    ViewBag.admissionstatus = "New Registration";
                    srn.Id = prd.First().Value[0].Id;
                    srn.Date = DateTime.Now.ToShortDateString();
                    srn.Time = DateTime.Now.ToShortTimeString();
                    ViewBag.initialpage = "yes";
                    sms.CreateOrUpdateStaffRequestNumDetails(srn);
                    ViewBag.HideValidation = "True";
                    return View();
                }
                else
                {
                    StaffDetails StaffDetails = new StaffDetails();
                    StaffDetails = sms.GetStaffDetailsId(Convert.ToInt32(id));

                    if (StaffDetails.Status == "SentForApproval")
                    {
                        Session["sentforapproval"] = "yes";
                    }
                    else
                    {
                        Session["sentforapproval"] = "";
                    }

                    StaffQualification staffqual = new StaffQualification();
                    IList<StaffQualification> staffqualificationList = new List<StaffQualification>();
                    staffqualificationList.Add(staffqual);
                    StaffDetails.StaffQualificationList = staffqualificationList;

                    StaffExperience staffexp = new StaffExperience();
                    IList<StaffExperience> staffexpList = new List<StaffExperience>();
                    staffexpList.Add(staffexp);
                    StaffDetails.StaffExperienceList = staffexpList;

                    StaffTraining stafftraining = new StaffTraining();
                    IList<StaffTraining> stafftrainingList = new List<StaffTraining>();
                    stafftrainingList.Add(stafftraining);
                    StaffDetails.StaffTrainingList = stafftrainingList;

                    //  UploadedFiles uploadedfile = new UploadedFiles();
                    UploadedFilesView uploadedfile = new UploadedFilesView();

                    //      if (StaffDetails.UploadedFilesList.Count == 0)
                    IList<UploadedFilesView> uploadedfileviewList = new List<UploadedFilesView>();
                    uploadedfileviewList.Add(uploadedfile);
                    StaffDetails.UploadedFilesList = uploadedfileviewList;

                    ViewBag.RequestNum = StaffDetails.PreRegNum;
                    ViewBag.Date = StaffDetails.CreatedDate;// DateTime.Now.ToShortDateString();
                    ViewBag.Time = StaffDetails.CreatedTime;// DateTime.Now.ToShortTimeString();
                    Session["Reqnum"] = StaffDetails.PreRegNum;
                    Session["status"] = StaffDetails.Status;
                    ViewBag.admissionstatus = StaffDetails.Status;
                    ViewBag.nam = StaffDetails.Name;
                    ViewBag.idnum = StaffDetails.IdNumber;
                    ViewBag.desig = StaffDetails.Designation;
                    ViewBag.campus = StaffDetails.Campus;
                    ViewBag.dept = StaffDetails.Department;
                    ViewBag.doj = StaffDetails.DateOfJoin;
                    ViewBag.subgroup = StaffDetails.StaffSubGroup;
                    ViewBag.PKId = StaffDetails.Id;
                    long counVar = sms.ExecutePercentageQueryFromStaffDetailsUsingQuery(StaffDetails.PreRegNum);
                    //if (counVar >= 0 && counVar <= 20) { ViewData["perVar"] = 90; }
                    //if (counVar >= 21 && counVar <= 40) { ViewData["perVar"] = 80; }
                    //if (counVar >= 41 && counVar <= 60) { ViewData["perVar"] = 60; }
                    //if (counVar >= 61 && counVar <= 80) { ViewData["perVar"] = 40; }
                    ViewData["perVar"] = Convert.ToInt64(((90 - counVar) / 90.00) * 100.00);
                    if (StaffDetails.DOB != null)
                    {
                        var today = DateTime.Today;
                        var dateOfBirth = StaffDetails.DOB.Split('/');
                        var TodayDate = (today.Year * 100 + today.Month) * 100 + today.Day;
                        var BirthDate = (Convert.ToInt32(dateOfBirth[2]) * 100 + Convert.ToInt32(dateOfBirth[1])) * 100 + Convert.ToInt32(dateOfBirth[0]);
                        var CalculateAge = (TodayDate - BirthDate) / 10000;
                        if (StaffDetails.Age != CalculateAge)
                        {
                            StaffDetails.Age = CalculateAge;
                            StaffDetailsView staffdtls = sms.GetStaffDetailsViewById(Convert.ToInt64(id));
                            staffdtls.Age = CalculateAge;
                            sms.CreateOrUpdateStaffDetailsView(staffdtls);
                        }
                    }
                    return View(StaffDetails);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
            //   return View();
        }

        [HttpPost]
        public ActionResult ApplicationForm(StaffDetailsView sd, string test)
        {
            try
            {
                //string desMas = string.Empty;
                string dateOfBirth = string.Empty;
                //string RecipientInfo = string.Empty, Subject = string.Empty, Body = string.Empty, MailBody = string.Empty;
                bool retValue;
                string staffName = string.Empty; string campus = string.Empty;
                sd.IdNumber = sd.IdNumber != null ? sd.IdNumber.Replace(" ", "").Trim() : null;
                StaffManagementService sms = new StaffManagementService();
                Dictionary<long, IList<StaffDetails>> staffdetails;
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("PreRegNum", Convert.ToInt32(Session["Reqnum"]));
                staffdetails = sms.GetStaffDetailsListWithPaging(0, 10000, string.Empty, string.Empty, criteria);

                if (staffdetails != null && staffdetails.First().Value != null && staffdetails.First().Value.Count > 0)
                {
                    dateOfBirth = staffdetails.First().Value[0].DOB;
                    staffName = staffdetails.First().Value[0].StaffUserName;
                    campus = staffdetails.First().Value[0].Campus;

                    if (staffdetails.First().Value[0].Status == "Sent For Approval")
                    {
                        Session["sentforapproval"] = "yes";
                    }
                    else
                    {
                        Session["sentforapproval"] = "";
                    }
                }
                if (staffdetails.First().Value.Count != 0)
                {
                    sd.Id = staffdetails.First().Value[0].Id;
                }

                sd.PreRegNum = Convert.ToInt32(Session["Reqnum"]);
                if (Request.Form["btnSave"] == "Save")
                {
                    if (staffdetails != null && staffdetails.First().Value != null && staffdetails.First().Value.Count > 0)
                    {
                        sd.Status = staffdetails.First().Value[0].Status;
                        Session["status"] = staffdetails.First().Value[0].Status;
                        sd.IdKeyValue = sd.Id;
                        ViewBag.admissionstatus = staffdetails.First().Value[0].Status;
                        info = "You have Entered New staff Details with PRE-Registration Number " + sd.PreRegNum;
                        UpdateInbox(sd.Campus, info, sd.PreRegNum);
                    }
                    else
                    {
                        sd.Status = "New Registration";
                        sd.WorkingType = "Staff";
                        sd.TempIdNumber = "TIPS-" + sd.PreRegNum;
                        Session["status"] = sd.Status;// staffdetails.First().Value[0].Status;
                        sd.CreatedDate = DateTime.Now.ToShortDateString();
                        sd.CreatedTime = DateTime.Now.ToShortTimeString();
                        ViewBag.admissionstatus = sd.Status;
                    }
                    ViewBag.save = "yes";
                    if (sd.DocCheck == "yes") { ViewBag.doccheck = "yes"; }
                    else if (sd.QualCheck == "yes") { ViewBag.qualcheck = "yes"; }
                }
                if (Request.Form["btnSave"] == "Submitted Resignation")
                {
                    sd.Status = "Submitted Resignation";

                    Session["status"] = sd.Status;
                    ViewBag.admissionstatus = sd.Status;
                }
                if (Request.Form["btnSave"] == "Serving Notes Period")
                {
                    sd.Status = "Serving Notes Period";
                    Session["status"] = sd.Status;
                    ViewBag.admissionstatus = sd.Status;
                }
                if (Request.Form["btnsentforapproval"] == "Send For Approval")
                {
                    sd.Status = "Sent For Approval";
                    Session["status"] = sd.Status;
                    ViewBag.admissionstatus = sd.Status;
                    info = "The application has been Sent for approval with PRE-Registration Number " + sd.PreRegNum;
                    UpdateInbox(sd.Campus, info, sd.PreRegNum);
                    ViewBag.sentforappr = "yes";
                }

                if (Request.Form["btnapprove"] == "Approve")
                {
                    MastersService ms = new MastersService();
                    //Dictionary<string, object> criteriam = new Dictionary<string, object>();
                    //criteriam.Add("Designation", sd.Designation);
                    //Dictionary<long, IList<DesignationMaster>> DesignationMaster = ms.GetDesignationMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteriam);
                    //if (DesignationMaster != null && DesignationMaster.FirstOrDefault().Value.Count > 0 && DesignationMaster.FirstOrDefault().Key > 0)
                    //{
                    //    desMas = DesignationMaster.FirstOrDefault().Value[0].Code;
                    //}
                    TIPS.Service.UserService us = new UserService();
                    //MailBody = GetBodyofMail();
                    if (sd.Status == "Registered")
                    {
                        Dictionary<string, object> criteria2 = new Dictionary<string, object>();
                        criteria2.Add("PreRegNum", Convert.ToInt64(Session["Reqnum"]));   // to check if staff is already registered or not while changing from inactive to registered
                        Dictionary<long, IList<StaffDetailsView>> IdCheck = sms.GetStaffDetailsViewListWithPaging(0, 0, string.Empty, string.Empty, criteria2);
                        if (sd.Campus == "TIPS ERODE" || sd.Campus == "TIPS SALEM" && IdCheck.First().Value[0].IdNumber == null)
                        {
                            Sequence scq = sms.GetStaffIdnumberfromSequenceTable(2);
                            sd.IdNumber = "TIPS" + JoiningYear(sd.DateOfJoin) + "-ACA-" + scq.Value + "";
                            scq.Value = scq.Value + 1;
                            scq.Id = 2;
                            sms.CreateOrUpdateSequence(scq);
                        }
                        else
                        {
                            User user = us.GetUserByUserId(IdCheck.First().Value[0].IdNumber != null ? IdCheck.First().Value[0].IdNumber : sd.TempIdNumber);
                            if (user != null && user.UserId != sd.IdNumber)
                            {
                                user.UserId = sd.IdNumber;
                                user.EmployeeId = sd.IdNumber;
                                us.CreateOrUpdateUser(user);
                                SendMailtoExistingStaff(sd.IdNumber, user.Password, sd.Name, campus, user.EmailId);
                            }
                            else
                            {
                                string passwrd = string.Empty;
                                if (user == null)
                                {
                                    //if (!string.IsNullOrEmpty(dateOfBirth))
                                    //{
                                    //    dateOfBirth = dateOfBirth.Replace(@"/", string.Empty).Trim();
                                    //}                                        
                                    User usrObj = new User();
                                    usrObj.CreatedDate = DateTime.Now;
                                    usrObj.ModifiedDate = DateTime.Now;
                                    PassworAuth PA = new PassworAuth();
                                    passwrd = GenerateRandomString(8);
                                    //encode and save the password
                                    usrObj.UserName = sd.Name;
                                    usrObj.UserId = sd.IdNumber;
                                    usrObj.EmployeeId = sd.IdNumber;
                                    usrObj.EmailId = sd.EmailId;
                                    usrObj.Campus = campus;
                                    usrObj.Password = PA.base64Encode(passwrd);
                                    usrObj.IsActive = true;
                                    usrObj.UserType = "Staff";
                                    usrObj.CreatedBy = Session["UserId"] != null ? Session["UserId"].ToString() : "";
                                    us.CreateOrUpdateUser(usrObj);
                                    if (ConfigurationManager.AppSettings["SendEmail1"].ToString() == "true")
                                    {

                                        //RecipientInfo = "Dear Sir/Madam,";
                                        //Subject = "Login Details";
                                        //Body = "Welcome to the TIPS Family!<br /><br/> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;we would like to congratulate you to join with us.<br/><br/>Your UserId is “" + IdNumber + "” and password will be “" + passwrd + "” i.e. the DOB of your's as given in the records. You can access the same through 172.16.17.252:8081 .<br/><br/> For any queries, mail us at “" + campusemaildet.First().EmailId.ToString() + "”. ";
                                        //retValue = emailObj.SendStudentRegistrationMail(null, staffdetails.EmailId, campus, Subject, "", Body, RecipientInfo, "Parent", null);                    
                                        System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                                        string CCMail = string.Empty;
                                        CCMail = ConfigurationManager.AppSettings["NewStaffCCMail"].ToString();
                                        string body = "<b>Dear " + sd.Name + "<b><br/><br/>";
                                        body = body + "<b>Welcome to ICMS...! </b><br/><br/>";
                                        body = body + "<b>URL :</b> <a href='http://myaccess.tipsglobal.net/'>myaccess.tipsglobal.net</a><br/><br/>";
                                        body = body + "<table style='border-collapse:collapse; border:1px solid black; padding: 5px;'> ";
                                        body = body + "<tr> <td style='font-weight:bold; border:1px solid black; padding: 5px;'><b> User Name </b></td> <td style='border:1px solid black; padding: 5px;'>" + sd.IdNumber + "  </td> <tr> ";
                                        body = body + "<tr> <td style='font-weight:bold; border:1px solid black; padding: 5px;'><b>Password</b></td> <td style='border:1px solid black; padding: 5px;'> " + passwrd + " </td><tr> ";
                                        body = body + "</table><br/><br/>Use the above Username and Password in Staff Portal.<br/> <br/>";
                                        body = body + "<b>Note:</b><br/>You are Requested to Update your Profile in Staff Portal i.e., EMail,D.O.B,Experience Details,Photo..Etc<br/> <br/>";
                                        mail.To.Add(sd.EmailId);
                                        mail.CC.Add(CCMail);
                                        mail.Body = body;
                                        mail.IsBodyHtml = true;
                                        mail.Subject = "ICMS-Login credentials ";
                                        EmailHelper emailObj = new EmailHelper();
                                        retValue = emailObj.SendEmailWithEmailTemplate(mail, campus, GetGeneralBodyofMail());
                                    }
                                }

                            }
                        }
                        //Sequence scq = sms.GetStaffIdnumberfromSequenceTable(1);                                
                        //sd.IdNumber = "TIPS-" + desMas + "" + JoiningYear(sd.DateOfJoin) + "-" + scq.Value + "";
                        //scq.Value = scq.Value + 1;
                        //scq.Id = 1;
                        //sms.CreateOrUpdateSequence(scq);                                                                            
                        /// Written by Kino
                        //EmailHelper emailObj = new EmailHelper();
                        //if (!string.IsNullOrEmpty(dateOfBirth))
                        //{
                        //    dateOfBirth = dateOfBirth.Replace(@"/", string.Empty).Trim();
                        //}
                        //User usrObj = new User();
                        //usrObj.CreatedDate = DateTime.Now;
                        //usrObj.ModifiedDate = DateTime.Now;
                        //PassworAuth PA = new PassworAuth();
                        ////encode and save the password
                        //usrObj.UserName = staffName;
                        //usrObj.UserId = sd.IdNumber;
                        //usrObj.Campus = campus;
                        //usrObj.Password = PA.base64Encode(dateOfBirth);
                        //usrObj.IsActive = true;
                        info = "The Staff has been moved to status 'Registered' with PRE-Registration Number " + sd.PreRegNum;
                        UpdateInbox(sd.Campus, info, sd.PreRegNum);
                        //us.CreateOrUpdateUser(usrObj);
                        //IList<CampusEmailId> campusemaildet = GetEmailIdByCampus(sd.Campus, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
                        //if (ConfigurationManager.AppSettings["SendEmail1"].ToString() == "true")
                        //{                            
                        //    RecipientInfo = "Dear Sir/Madam,";
                        //    Subject = "Login Details"; 
                        //    Body = "Welcome to the TIPS Family!<br /><br/> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;we would like to congratulate you to join with us.<br/><br/>Your UserId is “" + sd.IdNumber + "” and password will be “" + dateOfBirth + "” i.e. the DOB of your's as given in the records. You can access the same through 172.16.17.252:8081 .<br/><br/> For any queries, mail us at “" + campusemaildet.First().EmailId.ToString() + "”. ";
                        //    retValue = emailObj.SendStudentRegistrationMail(null, staffdetails.First().Value[0].EmailId, campus, Subject, Body, MailBody, RecipientInfo, "Parent", null);
                        //}
                        /// end

                        User usr = new User();
                        if (staffdetails != null && staffdetails.First().Value != null && staffdetails.First().Value.Count > 0)
                        {
                            if (staffdetails.First().Value[0].StaffUserName != null)
                            {
                                usr = us.GetUserByUserId(staffdetails.First().Value[0].StaffUserName);
                                if (usr != null)
                                {
                                    if (staffdetails.First().Value[0].CreatedDate == null)
                                    {
                                        usr.CreatedDate = DateTime.Now;
                                    }
                                    usr.ModifiedDate = DateTime.Now;
                                    usr.IsActive = true;
                                    us.CreateOrUpdateUser(usr);
                                }
                            }
                        }
                        ViewBag.regtrd = "yes";
                        ViewBag.regtrdid = sd.IdNumber;

                    }
                    if (sd.Status == "Inactive")
                    {
                        User usr = new User();
                        if (staffdetails != null && staffdetails.First().Value != null && staffdetails.First().Value.Count > 0)
                        {
                            if (staffdetails.First().Value[0].StaffUserName != null)
                            {
                                usr = us.GetUserByUserId(staffdetails.First().Value[0].StaffUserName);

                                if (usr != null)
                                {
                                    usr.ModifiedDate = DateTime.Now;
                                    usr.IsActive = false;

                                    if (staffdetails.First().Value[0].CreatedDate == null)
                                    {
                                        usr.CreatedDate = DateTime.Now;
                                    }
                                    info = "The Staff has been moved to status 'Inactive' with PRE-Registration Number " + sd.PreRegNum;
                                    UpdateInbox(sd.Campus, info, sd.PreRegNum);
                                    us.CreateOrUpdateUser(usr);
                                }
                            }
                        }
                    }
                    Session["status"] = sd.Status;
                    ViewBag.admissionstatus = sd.Status;

                }
                ViewBag.Date = sd.CreatedDate;// DateTime.Now.ToShortDateString();
                ViewBag.Time = sd.CreatedTime;// DateTime.Now.ToShortTimeString();                
                ViewBag.nam = sd.Name;// StaffDetails.Name;
                ViewBag.idnum = sd.IdNumber;// StaffDetails.IdNumber;
                ViewBag.desig = sd.Designation;// StaffDetails.Designation;
                ViewBag.campus = sd.Campus;// StaffDetails.Campus;               
                //  sms.CreateOrUpdateStaffDetails(sd);
                sd.BeenShortListedBefore = Request.Form["BeenShortListedBefore"] == "Yes" ? true : false;
                sd.RelativeWorkingWithUs = Request.Form["RelativeWorkingWithUs"] == "Yes" ? true : false;
                sms.CreateOrUpdateStaffDetailsView(sd);
                ViewBag.RequestNum = Session["Reqnum"];
                ViewBag.PKId = sd.Id;

                FillViewBag();
                long counVar = sms.ExecutePercentageQueryFromStaffDetailsUsingQuery(Convert.ToInt32(Session["Reqnum"]));
                //if (counVar >= 0 && counVar <= 20) { ViewData["perVar"] = 90; }
                //if (counVar >= 21 && counVar <= 40) { ViewData["perVar"] = 80; }
                //if (counVar >= 41 && counVar <= 60) { ViewData["perVar"] = 60; }
                //if (counVar >= 61 && counVar <= 80) { ViewData["perVar"] = 40; }
                ViewData["perVar"] = Convert.ToInt64(((90 - counVar) / 90.00) * 100.00);
                return RedirectToAction("ApplicationForm", "StaffManagement", new { id = sd.Id });
                //return View();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditQualification(StaffQualification sq)
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();

                sms.CreateOrUpdateStaffQualificatoinDetails(sq);
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditExperience(StaffExperience se)
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();

                sms.CreateOrUpdateStaffExperienceDetails(se);
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditTraining(StaffTraining st)
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();
                sms.CreateOrUpdateStaffTrainingDetails(st);
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult loadPartialView(string PartialViewName)
        {
            FillViewBag();
            return PartialView(PartialViewName);
        }

        public ActionResult WorkDetails()
        {
            return PartialView();
        }

        public ActionResult PersonalDetails()
        {
            return PartialView();
        }

        public ActionResult qualificationjqgrid(int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                StaffManagementService sm = new StaffManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("PreRegNum", Convert.ToInt32(Session["Reqnum"]));

                Dictionary<long, IList<StaffQualification>> StaffQualification = sm.GetStaffQualificationDetailsListWithPaging(page - 1, rows, sidx, sord, criteria);

                long totalrecords = StaffQualification.First().Key;
                int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                var jsondat = new
                {
                    total = totalPages,
                    page = page,
                    records = totalrecords,

                    rows = (from items in StaffQualification.First().Value
                            select new
                            {
                                i = 2,
                                cell = new string[] {
                            items.Course,
                            items.Board,
                            items.Institute,
                            items.YearOfComplete,
                            items.MajorSubjects,
                            items.Percentage,
                            items.Id.ToString(),
                            items.PreRegNum.ToString()
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

        public ActionResult AddQualification(string course, string board, string school, string yearofcomplete, string subjects, string percent)
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();
                StaffQualification staffqual = new StaffQualification();
                staffqual.PreRegNum = Convert.ToInt32(Session["reqnum"]);

                staffqual.Course = course;
                staffqual.Board = board;
                staffqual.Institute = school;
                staffqual.YearOfComplete = yearofcomplete;
                staffqual.MajorSubjects = subjects;
                staffqual.Percentage = percent;

                sms.CreateOrUpdateStaffQualificatoinDetails(staffqual);
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult Experiencejqgrid(int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                StaffManagementService sm = new StaffManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("PreRegNum", Convert.ToInt32(Session["Reqnum"]));

                Dictionary<long, IList<StaffExperience>> StaffExperience = sm.GetStaffExperienceDetailsListWithPaging(page - 1, rows, sidx, sord, criteria);

                long totalrecords = StaffExperience.First().Key;
                int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                var jsondat = new
                {
                    total = totalPages,
                    page = page,
                    records = totalrecords,

                    rows = (from items in StaffExperience.First().Value
                            select new
                            {
                                i = 2,
                                cell = new string[] {
                            items.EmployerName,
                            items.Location,
                            items.StartDate,
                            items.TillDate,
                            items.LastDesignation,
                            items.SpecificReasonForLeaving,
                            items.Id.ToString(),
                            items.PreRegNum.ToString()
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

        public ActionResult AddExperience(string empname, string location, string strtdate, string enddate, string lastdesig, string specificreasonforleaving)
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();
                StaffExperience staffexp = new StaffExperience();
                staffexp.PreRegNum = Convert.ToInt32(Session["reqnum"]);
                staffexp.SpecificReasonForLeaving = specificreasonforleaving;
                staffexp.EmployerName = empname;
                staffexp.Location = location;
                staffexp.StartDate = strtdate;
                staffexp.TillDate = enddate;
                staffexp.LastDesignation = lastdesig;

                sms.CreateOrUpdateStaffExperienceDetails(staffexp);
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult Trainingjqgrid(int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                StaffManagementService sm = new StaffManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("PreRegNum", Convert.ToInt32(Session["Reqnum"]));

                Dictionary<long, IList<StaffTraining>> StaffTraining = sm.GetStaffTrainingDetailsListWithPaging(page - 1, rows, sidx, sord, criteria);

                long totalrecords = StaffTraining.First().Key;
                int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                var jsondat = new
                {
                    total = totalPages,
                    page = page,
                    records = totalrecords,

                    rows = (from items in StaffTraining.First().Value
                            select new
                            {
                                i = 2,
                                cell = new string[] {
                            items.Particulars,
                            items.Place,
                            items.Date,
                            items.SponsoredBy,
                            items.Id.ToString(),
                            items.PreRegNum.ToString()
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

        public ActionResult AddTraining(string particulars, string place, string date, string sponsoredby)
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();
                StaffTraining stafftrn = new StaffTraining();
                stafftrn.PreRegNum = Convert.ToInt32(Session["reqnum"]);

                stafftrn.Particulars = particulars;
                stafftrn.Place = place;
                stafftrn.Date = date;
                stafftrn.SponsoredBy = sponsoredby;

                sms.CreateOrUpdateStaffTrainingDetails(stafftrn);
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult NewStaffDisplay()
        {
            FillViewBag();
            #region BreadCrumb
            string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
            #endregion
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Deletequalification(string id)
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();
                var test = id.Split(',');

                long[] idtodelete = new long[test.Length];
                int i = 0;
                foreach (string val in test)
                {
                    idtodelete[i] = Convert.ToInt64(val);
                    i++;
                }
                sms.DeleteQualificationDetails(idtodelete);
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteExperience(string id)
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();
                var test = id.Split(',');

                long[] idtodelete = new long[test.Length];
                int i = 0;
                foreach (string val in test)
                {
                    idtodelete[i] = Convert.ToInt64(val);
                    i++;
                }
                sms.DeleteExperienceDetails(idtodelete);
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteTraining(string id)
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();
                var test = id.Split(',');

                long[] idtodelete = new long[test.Length];
                int i = 0;
                foreach (string val in test)
                {
                    idtodelete[i] = Convert.ToInt64(val);
                    i++;
                }
                sms.DeleteTrainingDetails(idtodelete);
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult checkdocqual(string Preregno)
        {
            if (Preregno != "")
            {
                AdmissionManagementService ads = new AdmissionManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("PreRegNum", Convert.ToInt64(Preregno));
                criteria.Add("DocumentFor", "Staff");
                Dictionary<long, IList<UploadedFilesView>> UploadedFiles = ads.GetUploadedFilesViewListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteria);

                if (UploadedFiles.First().Key > 0)
                { return Json(new { success = true }, "text/html", JsonRequestBehavior.AllowGet); }
                else { return Json(new { success = false }, "text/html", JsonRequestBehavior.AllowGet); }
            }
            else { return Json(new { success = false }, "text/html", JsonRequestBehavior.AllowGet); }
        }

        public ActionResult EmployeeSalaryDetails()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EmployeeSalaryDetails(HttpPostedFileBase[] uploadedFile, Int32? monthOfSalary)
        {
            StringBuilder retValue = new StringBuilder();
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    StaffManagementService sms = new StaffManagementService();
                    HttpPostedFileBase theFile = HttpContext.Request.Files["uploadedFile"];
                    StringBuilder alreadyExists = new StringBuilder();
                    StringBuilder ErrorFilename = new StringBuilder();
                    StringBuilder UploadedFilename = new StringBuilder();
                    if (theFile != null && theFile.ContentLength > 0)
                    {
                        string fileName = string.Empty;
                        int length = uploadedFile.Length;
                        for (int l = 0; l < length; l++)
                        {
                            try
                            {
                                int AlreadyExistFile = 0;
                                string path = uploadedFile[l].InputStream.ToString();
                                byte[] imageSize = new byte[uploadedFile[l].ContentLength];
                                uploadedFile[l].InputStream.Read(imageSize, 0, (int)uploadedFile[l].ContentLength);
                                string UploadConnStr = "";
                                fileName = uploadedFile[l].FileName;
                                string fileExtn = Path.GetExtension(uploadedFile[l].FileName);
                                string fileLocation = ConfigurationManager.AppSettings["SalaryFilePath"].ToString() + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss").Replace(":", ".") + fileName;
                                uploadedFile[l].SaveAs(fileLocation);
                                if (fileExtn == ".xls")
                                {
                                    UploadConnStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
                                }
                                if (fileExtn == ".xlsx")
                                {
                                    UploadConnStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";
                                }

                                OleDbConnection itemconn = new OleDbConnection();
                                DataTable DtblXcelItemData = new DataTable();
                                string QeryToGetXcelItemData = "select * from " + string.Format("{0}${1}", "[Sheet1", "B1:AZ]");
                                itemconn.ConnectionString = UploadConnStr;
                                itemconn.Open();
                                OleDbCommand cmd = new OleDbCommand(QeryToGetXcelItemData, itemconn);
                                cmd.CommandType = CommandType.Text;
                                OleDbDataAdapter DtAdptrr = new OleDbDataAdapter();
                                DtAdptrr.SelectCommand = cmd;
                                DtAdptrr.Fill(DtblXcelItemData);
                                string[] strArray = { "Employee Id", "Name", "BASIC+DA", "HRA", "Monthly Gross", "Employer PF", "Total Compensation", "LOP Days", "LOP BASIC", "LOP HRA", "Earned Basic", "Earned LOP", "Total Earnings", "PF", "TDS", "MESS", "EB", "Advance", "Fine", "Others", 
                                                    "Total", "Reimbursement", "Tutor Allowance", "Net Salary","Payment Mode", "Bank Account Number", "PF Number"};
                                char chrFlag = 'Y';
                                if (DtblXcelItemData.Columns.Count == strArray.Length)
                                {
                                    int j = 0;
                                    string[] strColumnsAray = new string[DtblXcelItemData.Columns.Count];
                                    foreach (DataColumn dtColumn in DtblXcelItemData.Columns)
                                    {
                                        strColumnsAray[j] = dtColumn.ColumnName;
                                        j++;
                                    }
                                    for (int i = 0; i < strArray.Length - 1; i++)
                                    {
                                        if (strArray[i].Trim() != strColumnsAray[i].Trim())
                                        {
                                            chrFlag = 'N';
                                            break;
                                        }
                                    }
                                    if (chrFlag == 'Y')
                                    {
                                        IList<EmployeeSalaryDetails> SalaryList = new List<EmployeeSalaryDetails>();
                                        foreach (DataRow item in DtblXcelItemData.Rows)
                                        {
                                            if (item["Name"].ToString().Trim() != "")
                                            {
                                                EmployeeSalaryDetails esd = new EmployeeSalaryDetails();
                                                esd.EmployeeId = item["Employee Id"] != "" ? item["Employee Id"].ToString().Trim() : "";
                                                esd.StaffUserName = item["Name"] != "" ? item["Name"].ToString().Trim() : "";
                                                esd.Basic_DA = item["BASIC+DA"] != "" ? Convert.ToDecimal(item["BASIC+DA"].ToString().Trim()) : 0;
                                                esd.HRA = item["HRA"] != "" ? Convert.ToDecimal(item["HRA"].ToString().Trim()) : 0;
                                                esd.MonthlyGross = item["Monthly Gross"] != "" ? Convert.ToDecimal(item["Monthly Gross"].ToString().Trim()) : 0;
                                                esd.EmployerPF = Convert.ToDecimal(item["Employer PF"].ToString().Trim());
                                                esd.TotalCompensation = Convert.ToDecimal(item["Total Compensation"].ToString().Trim());
                                                esd.LOP = item["LOP Days"] != "" ? Convert.ToDecimal(item["LOP Days"].ToString().Trim()) : 0;
                                                esd.LOP_Basic = item["LOP BASIC"] != "" ? Convert.ToDecimal(item["LOP BASIC"].ToString().Trim()) : 0;
                                                esd.LOP_HRA = item["LOP HRA"] != "" ? Convert.ToDecimal(item["LOP HRA"].ToString().Trim()) : 0;
                                                esd.EarnedBasic = item["Earned Basic"] != "" ? Convert.ToDecimal(item["Earned Basic"].ToString().Trim()) : 0;
                                                esd.Earned_LOP = item["Earned LOP"] != "" ? Convert.ToDecimal(item["Earned LOP"].ToString().Trim()) : 0;
                                                esd.TotalEarnings = item["Total Earnings"] != "" ? Convert.ToDecimal(item["Total Earnings"].ToString().Trim()) : 0;
                                                esd.PF = item["PF"] != "" ? Convert.ToDecimal(item["PF"].ToString().Trim()) : 0;
                                                esd.TDS = item["TDS"] != "" ? Convert.ToDecimal(item["TDS"].ToString().Trim()) : 0;
                                                esd.Mess = item["MESS"] != "" ? Convert.ToDecimal(item["MESS"].ToString().Trim()) : 0;
                                                esd.EB = item["EB"] != "" ? Convert.ToDecimal(item["EB"].ToString().Trim()) : 0;
                                                esd.Advance = item["Advance"] != "" ? Convert.ToDecimal(item["Advance"].ToString().Trim()) : 0;
                                                esd.Fine = item["Fine"] != "" ? Convert.ToDecimal(item["Fine"].ToString().Trim()) : 0;
                                                esd.Others = item["Others"] != "" ? Convert.ToDecimal(item["Others"].ToString().Trim()) : 0;
                                                esd.Total = item["Total"] != "" ? Convert.ToDecimal(item["Total"].ToString().Trim()) : 0;
                                                esd.Reimbursement = item["Reimbursement"] != "" ? Convert.ToDecimal(item["Reimbursement"].ToString().Trim()) : 0;
                                                esd.TutorAllowance = item["Tutor Allowance"] != "" ? Convert.ToDecimal(item["Tutor Allowance"].ToString().Trim()) : 0;
                                                esd.NetSalary = item["Net Salary"] != "" ? Convert.ToDecimal(item["Net Salary"].ToString().Trim()) : 0;
                                                esd.PaymentMode = item["Payment Mode"] != "" ? item["Payment Mode"].ToString().Trim() : "";
                                                esd.BankAccNum = item["Bank Account Number"] != "" ? item["Bank Account Number"].ToString().Trim() : "";
                                                esd.PFNo = item["PF Number"] != "" ? item["PF Number"].ToString().Trim() : "";
                                                esd.CreatedDate = DateTime.Now;
                                                esd.CreatedBy = userId;

                                                SalaryList.Add(esd);

                                                // sms.CreateOrUpdateEmployeeSalaryDetails(esd);
                                            }
                                        }
                                        if (SalaryList.Count > 0)
                                        {
                                            sms.CreateOrUpdateEmployeeSalaryList(SalaryList);
                                            AdmissionManagementService ams = new AdmissionManagementService();
                                            foreach (var item in SalaryList)
                                            {
                                                StaffDetails sd = sms.GetStaffDetailsByIdNumber(item.EmployeeId);
                                                if (sd != null)
                                                {
                                                    UploadedFiles uf = new UploadedFiles();
                                                    uf.PreRegNum = sd.PreRegNum;
                                                    uf.DocumentType = "Salary Slip";
                                                    uf.DocumentName = "Salary Slip";
                                                    uf.UploadedDate = DateTime.Now.ToString();
                                                    uf.DocumentFor = "Staff";
                                                    uf.Type = "SalarySlip";
                                                    uf.DocumentData = null;
                                                    uf.MonthOfSalary = monthOfSalary ?? 0;
                                                    ams.CreateOrUpdateUploadedFiles(uf);
                                                }
                                            }
                                            return Json(new { success = true, result = "You have successfully uploaded the file." }, "text/html", JsonRequestBehavior.AllowGet);
                                        }
                                    }
                                    else
                                    {
                                        ErrorFilename.Append(fileName + ",");
                                    }
                                }
                                else
                                {
                                    ErrorFilename.Append(fileName + ",");
                                }
                            }
                            catch (Exception ex)
                            {
                                ErrorFilename.Append(fileName + ",");
                                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                            }
                        }
                    }
                    else
                    {
                        return Json(new { success = false, result = "You have uploaded the empty file. Please upload the correct file." }, "text/html", JsonRequestBehavior.AllowGet);
                    }

                    if (UploadedFilename != null && !string.IsNullOrEmpty(UploadedFilename.ToString()))
                    {
                        retValue.Append("-------files uploaded successfully-----------");
                        retValue.Append("<br />");
                        string[] upfiles = UploadedFilename.ToString().Split(',');
                        if (upfiles != null && upfiles.Count() > 0)
                        {
                            foreach (string s in upfiles)
                            {
                                if (!string.IsNullOrEmpty(s))
                                {
                                    retValue.Append(s + ";");
                                    retValue.Append("<br />");
                                }
                            }

                            retValue.Append("<br />");
                            retValue.Append("Successfully uploaded files" + Convert.ToInt32(UploadedFilename.ToString().Split(',').Count() - 1));
                            retValue.Append("<br />");
                            //retValue.Append("-----------------------------------------------------");
                        }
                    }
                    if (alreadyExists != null && !string.IsNullOrEmpty(alreadyExists.ToString()))
                    {
                        retValue.Append("-----------files already exists--------------");
                        retValue.Append("<br />");
                        string[] existsfiles = alreadyExists.ToString().Split(',');
                        if (existsfiles != null && existsfiles.Count() > 0)
                        {
                            foreach (string s in existsfiles)
                            { if (!string.IsNullOrEmpty(s)) retValue.Append(s + ";"); retValue.Append("<br />"); }
                            //retValue.Append("-------------------------------------------------");
                        }
                    }
                    if (ErrorFilename != null && !string.IsNullOrEmpty(ErrorFilename.ToString()))
                    {
                        retValue.Append("-----------error occured Files--------------");
                        string[] errfiles = ErrorFilename.ToString().Split(',');
                        if (errfiles != null && errfiles.Count() > 0)
                        {
                            foreach (string s in errfiles)
                            { if (!string.IsNullOrEmpty(s))retValue.Append(s + ";"); retValue.Append("<br />"); }
                            //retValue.Append("-------------------------------------------------");
                        }
                    }
                    return Json(new { success = true, result = retValue.ToString().Replace(Environment.NewLine, "<br />") }, "text/html", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
            }
            //MyLabel.Text = sb.ToString().Replace(Environment.NewLine, "<br />");
            return Json(new { success = true, result = retValue.ToString().Replace(Environment.NewLine, "<br />") }, "text/html", JsonRequestBehavior.AllowGet);
        }

        #region Staff Salary Slip
        public SalarySlip SalarySlipPdf(long Id)
        {
            try
            {
                AdmissionManagementService ads = new AdmissionManagementService();
                long tempId = Id;
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Id", Id);

                Dictionary<long, IList<UploadedFiles>> UploadedFiles = ads.GetUploadedFilesListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteria);
                int UploadPreRegNum = Convert.ToInt32(UploadedFiles.FirstOrDefault().Value[0].PreRegNum);

                StaffManagementService sms = new StaffManagementService();
                criteria.Clear();
                criteria.Add("PreRegNum", UploadPreRegNum);
                Dictionary<long, IList<StaffDetails>> StaffDetails = sms.GetStaffDetailsListWithPaging(0, 1000, string.Empty, string.Empty, criteria);
                string IdNumber = StaffDetails.FirstOrDefault().Value[0].IdNumber;

                criteria.Clear();
                criteria.Add("EmployeeId", IdNumber);
                Dictionary<long, IList<EmployeeSalaryDetails>> SalaryDetails = sms.GetEmployeeSalaryDetailsListWithPaging(0, 1000, string.Empty, string.Empty, criteria);

                int amt = Convert.ToInt32(SalaryDetails.FirstOrDefault().Value[0].NetSalary);

                double value = Convert.ToDouble(SalaryDetails.FirstOrDefault().Value[0].NetSalary);

                var values = value.ToString(CultureInfo.InvariantCulture).Split('.');
                int firstValue = int.Parse(values[0]);
                int secondValue = int.Parse(values[1]);
                int secFirVal = secondValue / 10;
                int secSecVal = secondValue % 10;
                string secFirValWords = NumberToText(secFirVal);
                string secSecValWords = NumberToText(secSecVal);
                string SeconNetValueWords = secFirValWords + secSecValWords;

                string SlryMonth = string.Empty;
                DateTime DateNow = DateTime.Now;
                int month = DateNow.Month;
                if (month == 01) { SlryMonth = "January"; }
                if (month == 02) { SlryMonth = "February"; }
                if (month == 03) { SlryMonth = "March"; }
                if (month == 04) { SlryMonth = "April"; }
                if (month == 05) { SlryMonth = "May"; }
                if (month == 06) { SlryMonth = "June"; }
                if (month == 07) { SlryMonth = "July"; }
                if (month == 08) { SlryMonth = "August"; }
                if (month == 09) { SlryMonth = "September"; }
                if (month == 10) { SlryMonth = "October"; }
                if (month == 11) { SlryMonth = "November"; }
                if (month == 12) { SlryMonth = "December"; }
                //string firstvaluewords = NumberToText(firstValue);
                //string secondValuewords = NumberToText(secondValue);
                //string amntwords = firstvaluewords + secondValuewords;

                return new SalarySlip
                {

                    Name = StaffDetails.FirstOrDefault().Value[0].Name,
                    EmployeeCode = StaffDetails.FirstOrDefault().Value[0].IdNumber,
                    Branch = StaffDetails.FirstOrDefault().Value[0].Campus,
                    DateOfJoining = StaffDetails.FirstOrDefault().Value[0].DateOfJoin,
                    Designation = StaffDetails.FirstOrDefault().Value[0].Designation,
                    PFNumber = StaffDetails.FirstOrDefault().Value[0].PFNo,
                    PaymentBy = "Gobinath",
                    AcNumber = StaffDetails.FirstOrDefault().Value[0].BankAccountNumber,
                    BasicPay_DA = Convert.ToDecimal(SalaryDetails.FirstOrDefault().Value[0].Basic_DA),
                    HRA = Convert.ToDecimal(SalaryDetails.FirstOrDefault().Value[0].HRA),
                    MonthlyGross = Convert.ToDecimal(SalaryDetails.FirstOrDefault().Value[0].MonthlyGross),
                    EmployerPf = Convert.ToDecimal(SalaryDetails.FirstOrDefault().Value[0].EmployerPF),
                    PF = Convert.ToDecimal(SalaryDetails.FirstOrDefault().Value[0].PF),
                    TDS = Convert.ToDecimal(SalaryDetails.FirstOrDefault().Value[0].TDS),
                    MessBill = Convert.ToDecimal(SalaryDetails.FirstOrDefault().Value[0].Mess),
                    EbBill = Convert.ToDecimal(SalaryDetails.FirstOrDefault().Value[0].EB),
                    AdvancePayment = Convert.ToDecimal(SalaryDetails.FirstOrDefault().Value[0].Advance),
                    OtherDeduction = Convert.ToDecimal(SalaryDetails.FirstOrDefault().Value[0].Others),
                    TotalCompensation = Convert.ToDecimal(SalaryDetails.FirstOrDefault().Value[0].TotalCompensation),
                    TotalEarnings = Convert.ToDecimal(SalaryDetails.FirstOrDefault().Value[0].TotalEarnings),
                    TotalDeduction = Convert.ToDecimal(SalaryDetails.FirstOrDefault().Value[0].Total),
                    Reimbursement = Convert.ToDecimal(SalaryDetails.FirstOrDefault().Value[0].Reimbursement),
                    TutorAllowance = Convert.ToDecimal(SalaryDetails.FirstOrDefault().Value[0].TutorAllowance),
                    NetAmount = Convert.ToDecimal(SalaryDetails.FirstOrDefault().Value[0].NetSalary),
                    FirstNetAmountInWords = NumberToText(firstValue),
                    SecondNetAmountInWords = SeconNetValueWords,
                    LOP = Convert.ToDecimal(SalaryDetails.FirstOrDefault().Value[0].LOP),
                    LOP_Basic = Convert.ToDecimal(SalaryDetails.FirstOrDefault().Value[0].LOP_Basic),
                    LOP_HRA = Convert.ToDecimal(SalaryDetails.FirstOrDefault().Value[0].LOP_HRA),
                    EarnedBasic = Convert.ToDecimal(SalaryDetails.FirstOrDefault().Value[0].EarnedBasic),
                    Earned_LOP = Convert.ToDecimal(SalaryDetails.FirstOrDefault().Value[0].Earned_LOP),
                    SalaryMonth = SlryMonth
                };
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "InsightOrderPolicy");
                throw ex;
            }
        }
        private void TipsLogo(SalarySlip SalSlip, string imageName)
        {
            string url = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            SalSlip.TipsLogo = url + "Images/" + imageName;
        }
        private void TipsName(SalarySlip SalSlip, string imageName)
        {
            string url = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            SalSlip.TipsName = url + "Images/" + imageName;
        }
        private void TipsAddress(SalarySlip SalSlip, string imageName)
        {
            string url = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            SalSlip.TipsAddress = url + "Images/" + imageName;
        }
        public static string NumberToText(int number)
        {
            StringBuilder wordNumber = new StringBuilder();

            string[] powers = new string[] { "Thousand ", "Million ", "Billion " };
            string[] tens = new string[] { "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
            string[] ones = new string[] { "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten",
"Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };

            if (number == 0) { return "Zero"; }
            if (number < 0)
            {
                wordNumber.Append("Negative ");
                number = -number;
            }

            long[] groupedNumber = new long[] { 0, 0, 0, 0 };
            int groupIndex = 0;

            while (number > 0)
            {
                groupedNumber[groupIndex++] = number % 1000;
                number /= 1000;
            }

            for (int i = 3; i >= 0; i--)
            {
                long group = groupedNumber[i];

                if (group >= 100)
                {
                    wordNumber.Append(ones[group / 100 - 1] + " Hundred ");
                    group %= 100;

                    if (group == 0 && i > 0)
                        wordNumber.Append(powers[i - 1]);
                }

                if (group >= 20)
                {
                    if ((group % 10) != 0)
                        wordNumber.Append(tens[group / 10 - 2] + " " + ones[group % 10 - 1] + " ");
                    else
                        wordNumber.Append(tens[group / 10 - 2] + " ");
                }
                else if (group > 0)
                    wordNumber.Append(ones[group - 1] + " ");

                if (group != 0 && i > 0)
                    wordNumber.Append(powers[i - 1]);
            }

            return wordNumber.ToString().Trim();
        }
        #endregion

        #region Staff Birthday Wishes
        public ActionResult SendBDayWishesMail()
        {
            //StaffManagementService SMS=new StaffManagementService();
            //bool ReturnValue;
            //ReturnValue = SMS.SendBDayWishes();
            return View();
        }
        public ActionResult StaffBDayWisheStatusJqgrid(int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    StaffManagementService SMS = new StaffManagementService();
                    criteria.Clear();
                    criteria.Add("IsSent", false);
                    sord = sord == "desc" ? "Desc" : "Asc";
                    Dictionary<long, IList<StaffBDayWishesStatus>> StaffBDayWishesStatus = null;
                    StaffBDayWishesStatus = SMS.GetStaffBDayWishesStatusListWithPaging(0, 9999, sidx, sord, criteria);
                    if (StaffBDayWishesStatus != null && StaffBDayWishesStatus.FirstOrDefault().Value.Count > 0 && StaffBDayWishesStatus.FirstOrDefault().Key > 0)
                    {
                        long totalrecords = StaffBDayWishesStatus.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)5);
                        var AssLst = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (
                                 from items in StaffBDayWishesStatus.First().Value

                                 select new
                                 {
                                     cell = new string[] 
                                         {
                                             items.Id.ToString(),
                                             items.IdNumber,
                                             items.Name,
                                             items.DOB,
                                             items.EmailId,
                                             items.IsSent==false?"Not Send(Due to Email Id Format)":"Sent"
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
        #endregion

        public string GetBodyofMail()
        {
            string MessageBody = System.IO.File.ReadAllText(HttpContext.Server.MapPath("~/Views/Shared/AdmissionEmailBody.html"));
            return MessageBody;
        }


        #region EmploymentApplication
        public ActionResult EmploymentApplicationForm()
        {
            FillViewBag();
            #region BreadCrumb
            string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
            #endregion
            return View();
        }
        public ActionResult EmployeeListJqGrid(string campus, string designation, string department, string stat, string appname, string idno, string flag, string type, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                TIPS.Entities.User sessionUser = (TIPS.Entities.User)Session["objUser"];
                StaffManagementService sms = new StaffManagementService();

                Dictionary<string, object> criteria = new Dictionary<string, object>();
                string colName = string.Empty; string[] values = new string[1];
                {
                    var usrcmp = Session["UserCampus"] as IEnumerable<string>;
                    if (usrcmp.Count() != 0)
                    {
                        if (usrcmp.First() != null)            // to check if the usrcmp obj is null or with data
                        {
                            if (!string.IsNullOrWhiteSpace(campus))
                            {
                                criteria.Add("Campus", campus);
                            }
                            else
                            {
                                criteria.Add("Campus", usrcmp);
                            }
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(designation))
                    {
                        criteria.Add("Designation", designation);
                    }

                    if (!string.IsNullOrWhiteSpace(department))
                    {
                        criteria.Add("Department", department);
                    }

                    if (!string.IsNullOrWhiteSpace(appname))
                    {
                        criteria.Add("Name", appname);
                    }

                    if (!string.IsNullOrWhiteSpace(idno))
                    {
                        criteria.Add("IdNumber", idno);
                    }

                    if (type == "new")
                    {
                        if (Session["staffapproverrole"].ToString() == "STM-APP")
                        {
                            string[] status = { "Sent For Approval", "On Hold", "Call For Interview" };
                            criteria.Add("Status", status);
                        }
                        else
                        {
                            string[] status = { "New Registration", "Sent For Approval", "On Hold", "Call For Interview" };
                            criteria.Add("Status", status);
                        }
                    }

                    if (type == "old")
                    {
                        if (string.IsNullOrWhiteSpace(stat))
                        {
                            string[] status = { "Registered" };
                            criteria.Add("Status", status);
                        }
                        else
                        {
                            string[] status = { stat.ToString() };
                            criteria.Add("Status", status);
                        }
                    }

                    if (string.Equals(sessionUser.UserType != null ? sessionUser.UserType : string.Empty, "Staff", StringComparison.CurrentCultureIgnoreCase))
                    {
                        colName = "IdNumber";
                        values[0] = sessionUser.EmployeeId != null ? sessionUser.EmployeeId : string.Empty;
                    }

                    Dictionary<long, IList<TIPS.Entities.StaffManagementEntities.StaffDetailsView>> StaffDetails;

                    StaffDetails = sms.GetStaffDetailsListWithPagingAndCriteriaWithAlias(page - 1, rows, sidx, sord, colName, values, criteria, null);

                    if (StaffDetails.Count > 0)
                    {
                        long totalrecords = StaffDetails.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);

                        var jsondat = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,

                            rows = (from items in StaffDetails.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                                    items.PreRegNum.ToString(),
                             
                            items.Name,
                            items.IdNumber,      
                            items.Campus,
                            items.Designation,
                            items.Department,
                            items.Gender,
                            items.Status,
                            String.Format(@"<a style='color:#034af3;text-decoration:underline' href= '/StaffManagement/EmploymentApplicationPDF?StaffIdNumber="+items.IdNumber+"' target='_Blank'>{0}</a>","<i class='ace-icon fa fa-file-pdf-o red'></i>"),
                            items.Id.ToString()
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
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult EmploymentApplicationPDF(string StaffIdNumber)
        {
            StaffDetails Staff = new StaffDetails();
            StaffManagementService SMS = new StaffManagementService();
            //string StaffIdNumber = "TIPS -AD07-007";
            Staff = SMS.GetStaffDetailsByIdNumber(StaffIdNumber);
            EmploymentApplicationPDF Emp = new EmploymentApplicationPDF();
            Emp.PreRegNum = Staff.PreRegNum;
            Emp.Designation = (!string.IsNullOrEmpty(Staff.Designation)) ? Staff.Designation : "";
            Emp.Name = (!string.IsNullOrEmpty(Staff.Name)) ? Staff.Name : "NILL";
            Emp.Gender = (!string.IsNullOrEmpty(Staff.Gender)) ? Staff.Gender : "NILL";
            Emp.DOB = (!string.IsNullOrEmpty(Staff.DOB)) ? Staff.DOB : "NILL";
            Emp.PhoneNo = (!string.IsNullOrEmpty(Staff.PhoneNo)) ? Staff.PhoneNo : "NILL";
            Emp.Alt_PhoneNo = (!string.IsNullOrEmpty(Staff.AltPhoneNo)) ? Staff.AltPhoneNo : "NILL";
            Emp.NativeState = (!string.IsNullOrEmpty(Staff.NativeState)) ? Staff.NativeState : "NILL";
            Emp.EmailId = (!string.IsNullOrEmpty(Staff.EmailId)) ? Staff.EmailId : "NILL";
            Emp.BGRP = (!string.IsNullOrEmpty(Staff.BGRP)) ? Staff.BGRP : "NILL";
            Emp.MaritalStatus = (!string.IsNullOrEmpty(Staff.MaritalStatus)) ? Staff.MaritalStatus : "NILL";
            Emp.Spoken_LanguagesKnown = (!string.IsNullOrEmpty(Staff.LanguagesKnown)) ? Staff.LanguagesKnown : "NILL";
            Emp.Written_LanguagesKnown = (!string.IsNullOrEmpty(Staff.Written_LanguagesKnown)) ? Staff.Written_LanguagesKnown : "NILL";
            Emp.FatherName = (!string.IsNullOrEmpty(Staff.FatherName)) ? Staff.FatherName : "NILL";
            Emp.FatherOccupation = (!string.IsNullOrEmpty(Staff.FatherOccupation)) ? Staff.FatherOccupation : "NILL";
            Emp.SpouseName = (!string.IsNullOrEmpty(Staff.SpouseName)) ? Staff.SpouseName : "NILL";
            Emp.SpouseOccupation = (!string.IsNullOrEmpty(Staff.SpouseOccupation)) ? Staff.SpouseOccupation : "NILL";
            Emp.CorrespondenceAddress = (!string.IsNullOrEmpty(Staff.AlternateAddress)) ? Staff.AlternateAddress : "NILL";
            Emp.PermanantAddress = (!string.IsNullOrEmpty(Staff.PermanantAddress)) ? Staff.PermanantAddress : "NILL";
            Emp.EmrgcyContPrsn = (!string.IsNullOrEmpty(Staff.EmergencyContactPerson)) ? Staff.EmergencyContactPerson : "NILL";
            Emp.EmrgcyContNumber = (!string.IsNullOrEmpty(Staff.EmergencyContactNo)) ? Staff.EmergencyContactNo : "NILL";
            TipsLogo(Emp, "TipsLogo.jpg");
            NaceLogo(Emp, "logonace.jpg");
            IList<StaffFamilyDetails> StaffFamilyList = new List<StaffFamilyDetails>();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Clear();
            criteria.Add("PreRegNum", Emp.PreRegNum);
            Dictionary<long, IList<StaffFamilyDetails>> StaffFamilyDetails = SMS.GetStaffFamilyDetailsListWithPaging(0, 9999, string.Empty, string.Empty, criteria);
            if (StaffFamilyDetails != null && StaffFamilyDetails.Count > 0)
            {
                foreach (var item in StaffFamilyDetails.FirstOrDefault().Value)
                {
                    StaffFamilyDetails Det = new StaffFamilyDetails();
                    Det.Name = item.Name;
                    Det.Occupation = item.Occupation;
                    Det.Age = item.Age;
                    Det.Relationship = item.Relationship;
                    StaffFamilyList.Add(Det);
                }
            }
            Emp.StaffFamilyDetailsList = StaffFamilyList;
            IList<StaffQualification> StaffQualificationList = new List<StaffQualification>();
            criteria.Clear();
            criteria.Add("PreRegNum", Emp.PreRegNum);
            Dictionary<long, IList<StaffQualification>> StaffQualificationDetails = SMS.GetStaffQualificationDetailsListWithPaging(0, 9999, string.Empty, string.Empty, criteria);
            if (StaffQualificationDetails != null && StaffQualificationDetails.Count > 0)
            {
                foreach (var QaulItem in StaffQualificationDetails.FirstOrDefault().Value)
                {
                    StaffQualification Qaul = new StaffQualification();
                    Qaul.Course = QaulItem.Course;
                    Qaul.Board = QaulItem.Board;
                    Qaul.Institute = QaulItem.Institute;
                    Qaul.YearOfComplete = QaulItem.YearOfComplete;
                    Qaul.MajorSubjects = QaulItem.MajorSubjects;
                    Qaul.Percentage = QaulItem.Percentage;
                    StaffQualificationList.Add(Qaul);
                }
            }
            Emp.StaffQualificationDetailsList = StaffQualificationList;

            IList<StaffExperience> StaffExperienceList = new List<StaffExperience>();
            criteria.Clear();
            criteria.Add("PreRegNum", Emp.PreRegNum);
            Dictionary<long, IList<StaffExperience>> StaffExperienceDetails = SMS.GetStaffExperienceDetailsListWithPaging(0, 9999, string.Empty, string.Empty, criteria);
            if (StaffExperienceDetails != null && StaffExperienceDetails.Count > 0)
            {
                //Emp.TotYrOfExp = 0;
                //Emp.TotYrOfTeachExp = 0;
                Emp.SyllabusHandled = string.Empty;
                Emp.SubjectsTaught = string.Empty;
                Emp.Achievments = string.Empty;
                int i = 0;
                foreach (var ExpItem in StaffExperienceDetails.FirstOrDefault().Value)
                {
                    StaffExperience Exp = new StaffExperience();
                    Exp.EmployerName = ExpItem.EmployerName;
                    Exp.Location = ExpItem.Location;
                    Exp.StartDate = ExpItem.StartDate;
                    Exp.TillDate = ExpItem.TillDate;
                    Exp.LastDesignation = ExpItem.LastDesignation;
                    Exp.SpecificReasonForLeaving = ExpItem.SpecificReasonForLeaving;
                    StaffExperienceList.Add(Exp);
                    //Emp.TotYrOfExp = (!string.IsNullOrEmpty(ExpItem.TotalYearsOfExp)) ? Emp.TotYrOfExp + Convert.ToInt32(ExpItem.TotalYearsOfExp) : Emp.TotYrOfExp + 0;
                    //Emp.TotYrOfTeachExp = (!string.IsNullOrEmpty(ExpItem.TotalYearsOfTeachingExp)) ? Emp.TotYrOfTeachExp + Convert.ToInt32(ExpItem.TotalYearsOfTeachingExp) : Emp.TotYrOfTeachExp + 0;
                    //if (i == 0) Emp.SyllabusHandled = Emp.SyllabusHandled + ExpItem.SyllabusHandled;
                    //else Emp.SyllabusHandled = Emp.SyllabusHandled + "," + ExpItem.SyllabusHandled;
                    //if (i == 0) Emp.SubjectsTaught = Emp.SubjectsTaught + ExpItem.SubjectsTaught;
                    //else Emp.SubjectsTaught = Emp.SubjectsTaught + "," + ExpItem.SubjectsTaught;
                    //if (i == 0) Emp.GradesTaught = Emp.GradesTaught + ExpItem.GradesTaught;
                    //else Emp.GradesTaught = Emp.GradesTaught + "," + ExpItem.GradesTaught;
                    //i++;
                }
            }
            Emp.TotYrOfExp = (!string.IsNullOrEmpty(Staff.TotalYearsOfExp)) ? Staff.TotalYearsOfExp : "NILL";
            Emp.TotYrOfTeachExp = (!string.IsNullOrEmpty(Staff.TotalYearsOfTeachingExp)) ? Staff.TotalYearsOfTeachingExp : "NILL";
            Emp.SyllabusHandled = (!string.IsNullOrEmpty(Staff.SyllabusHandled)) ? Staff.SyllabusHandled : "NILL";
            Emp.SubjectsTaught = (!string.IsNullOrEmpty(Staff.SubjectsTaught)) ? Staff.SubjectsTaught : "NILL";
            Emp.GradesTaught = (!string.IsNullOrEmpty(Staff.GradesTaught)) ? Staff.GradesTaught : "NILL";
            Emp.Achievments = (!string.IsNullOrEmpty(Staff.Achievments)) ? Staff.Achievments : "NILL";
            Emp.StaffExperienceDetailsList = StaffExperienceList;
            Emp.ExpectedSalary = (!string.IsNullOrEmpty(Staff.ExpectedSalary)) ? Staff.ExpectedSalary : "NILL";
            Emp.LastDrawnGrossSalary = (!string.IsNullOrEmpty(Staff.LastDrawnGrossSalary)) ? Staff.LastDrawnGrossSalary : "NILL";
            Emp.LastDrawnNettSalary = (!string.IsNullOrEmpty(Staff.LastDrawnNettSalary)) ? Staff.LastDrawnNettSalary : "NILL";
            Emp.JoiningDateOrDays = (!string.IsNullOrEmpty(Staff.JoiningDateOrDays)) ? Staff.JoiningDateOrDays : "NILL";

            //For Other Details
            Emp.AnyOtherSignificant = (!string.IsNullOrEmpty(Staff.AnyOtherSignificant)) ? Staff.AnyOtherSignificant : "NILL";
            Emp.SpecialInterests = (!string.IsNullOrEmpty(Staff.SpecialInterests)) ? Staff.SpecialInterests : "NILL";
            Emp.HowYouKnowVacancy = (!string.IsNullOrEmpty(Staff.HowYouKnowVacancy)) ? Staff.HowYouKnowVacancy : "NILL";
            Emp.BeenShortListedBefore = Staff.BeenShortListedBefore == true ? "Yes" : "No";
            Emp.ShortlistedWhy = (!string.IsNullOrEmpty(Staff.ShortlistedWhy)) ? Staff.ShortlistedWhy : "NILL";
            Emp.RelativeWorkingWithUs = Staff.RelativeWorkingWithUs == true ? "Yes" : "No";
            Emp.RelativeDetails = (!string.IsNullOrEmpty(Staff.RelativeDetails)) ? Staff.RelativeDetails : "NILL";
            Emp.CommitTimeWithTIPS = (!string.IsNullOrEmpty(Staff.CommitTimeWithTIPS)) ? Staff.CommitTimeWithTIPS : "NILL";
            Emp.CareerGrowthExpectation = (!string.IsNullOrEmpty(Staff.CareerGrowthExpectation)) ? Staff.CareerGrowthExpectation : "NILL";
            Emp.WillingToTravel = (!string.IsNullOrEmpty(Staff.WillingToTravel)) ? Staff.WillingToTravel : "NILL";
            Emp.WillingForRelocation = (!string.IsNullOrEmpty(Staff.WillingForRelocation)) ? Staff.WillingForRelocation : "NILL";


            Emp.StaffFamilyDetailsList = StaffFamilyList;
            IList<StaffReferenceDetails> StaffReferenceList = new List<StaffReferenceDetails>();
            criteria.Clear();
            criteria.Add("PreRegNum", Emp.PreRegNum);
            Dictionary<long, IList<StaffReferenceDetails>> StaffReferenceDetails = SMS.GetStaffReferenceDetailsListWithPaging(0, 9999, string.Empty, string.Empty, criteria);
            if (StaffReferenceDetails != null && StaffReferenceDetails.Count > 0)
            {
                foreach (var RefeItem in StaffReferenceDetails.FirstOrDefault().Value)
                {
                    StaffReferenceDetails Ref = new StaffReferenceDetails();
                    Ref.RefName = RefeItem.RefName;
                    Ref.RefContactNo = RefeItem.RefContactNo;
                    Ref.RefHowKnow = RefeItem.RefHowKnow;
                    Ref.RefHowLongKnow = RefeItem.RefHowLongKnow;
                    StaffReferenceList.Add(Ref);
                }
            }
            Emp.StaffReferenceDetailsList = StaffReferenceList;
            Emp.CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");

            return new Rotativa.ViewAsPdf("EmploymentApplicationPDF", Emp)
            {
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageMargins = new Rotativa.Options.Margins(0, 0, 0, 0)
            };
        }
        private void TipsLogo(EmploymentApplicationPDF Emp, string imageName)
        {
            string url = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            Emp.TipsLogo = url + "Images/" + imageName;
        }
        private void NaceLogo(EmploymentApplicationPDF Emp, string imageName)
        {
            string url = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            Emp.NaceLogo = url + "Images/" + imageName;
        }

        public ActionResult AddFamilyDetails(string Name, string Occupation, string Age, string Relationship)
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();
                StaffFamilyDetails Family = new StaffFamilyDetails();
                Family.PreRegNum = Convert.ToInt32(Session["reqnum"]);
                Family.Name = Name;
                Family.Occupation = Occupation;
                Family.Age = Age;
                Family.Relationship = Relationship;
                sms.CreateOrUpdateStaffFamilyDetails(Family);
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult FamilyDetailsJqgrid(int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                StaffManagementService sm = new StaffManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("PreRegNum", Convert.ToInt32(Session["Reqnum"]));

                Dictionary<long, IList<StaffFamilyDetails>> StaffQualification = sm.GetStaffFamilyDetailsListWithPaging(page - 1, rows, sidx, sord, criteria);

                long totalrecords = StaffQualification.First().Key;
                int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                var jsondat = new
                {
                    total = totalPages,
                    page = page,
                    records = totalrecords,

                    rows = (from items in StaffQualification.First().Value
                            select new
                            {
                                i = 2,
                                cell = new string[] {
                            items.Name,
                            items.Occupation,
                            items.Age,
                            items.Relationship,
                            items.Id.ToString(),
                            items.PreRegNum.ToString()
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
        public ActionResult EditFamilyDetails(StaffFamilyDetails Family)
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();
                sms.CreateOrUpdateStaffFamilyDetails(Family);
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteStaffFamilyDetails(string id)
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();
                var test = id.Split(',');
                long[] idtodelete = new long[test.Length];
                int i = 0;
                foreach (string val in test)
                {
                    idtodelete[i] = Convert.ToInt64(val);
                    i++;
                }
                sms.DeleteStaffFamilyDetails(idtodelete);
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult AddReferenceDetails(string RefName, string RefContactNo, string RefHowKnow, string RefHowLongKnow)
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();
                StaffReferenceDetails Ref = new StaffReferenceDetails();
                Ref.PreRegNum = Convert.ToInt32(Session["reqnum"]);
                Ref.RefName = RefName;
                Ref.RefContactNo = RefContactNo;
                Ref.RefHowKnow = RefHowKnow;
                Ref.RefHowLongKnow = RefHowLongKnow;
                sms.CreateOrUpdateStaffReferenceDetails(Ref);
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult ReferenceDetailsJqgrid(int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                StaffManagementService sm = new StaffManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("PreRegNum", Convert.ToInt32(Session["Reqnum"]));

                Dictionary<long, IList<StaffReferenceDetails>> ReferenceDetails = sm.GetStaffReferenceDetailsListWithPaging(page - 1, rows, sidx, sord, criteria);

                long totalrecords = ReferenceDetails.First().Key;
                int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                var jsondat = new
                {
                    total = totalPages,
                    page = page,
                    records = totalrecords,

                    rows = (from items in ReferenceDetails.First().Value
                            select new
                            {
                                i = 2,
                                cell = new string[] {
                            items.RefName,
                            items.RefContactNo,
                            items.RefHowKnow,
                            items.RefHowLongKnow,
                            items.Id.ToString(),
                            items.PreRegNum.ToString()
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
        public ActionResult EditStaffReferenceDetails(StaffReferenceDetails Ref)
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();
                sms.CreateOrUpdateStaffReferenceDetails(Ref);
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteStaffReferenceDetails(string id)
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();
                var test = id.Split(',');
                long[] idtodelete = new long[test.Length];
                int i = 0;
                foreach (string val in test)
                {
                    idtodelete[i] = Convert.ToInt64(val);
                    i++;
                }
                sms.DeleteStaffReferenceDetails(idtodelete);
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        #endregion

        #region "Staff Training and Developments"

        public ActionResult GenerateCertificate(long staffId, string certFy, string Course, string DateofRedsn, string CurDesg, string Letterdate, string DateofRelv, string RSCertfy, string AmountSalryApr, string DateofSalryApr, string SalaryCerAmount, string AmtInWords, string PFAmount, string DateofTrsfr, string Campus)
        {
            Course = (Course == "undefined") ? "" : Course;
            CurDesg = (CurDesg == "undefined") ? "" : CurDesg;
            DateofRedsn = (DateofRedsn == "undefined") ? "" : DateofRedsn;
            Letterdate = (Letterdate == "undefined") ? "" : Letterdate;
            DateofRelv = (DateofRelv == "undefined") ? "" : DateofRelv;
            RSCertfy = (RSCertfy == "undefined") ? "" : RSCertfy;
            AmountSalryApr = (AmountSalryApr == "undefined") ? "" : AmountSalryApr;
            DateofSalryApr = (DateofSalryApr == "undefined") ? "" : DateofSalryApr;
            SalaryCerAmount = (SalaryCerAmount == "undefined") ? "" : SalaryCerAmount;
            AmtInWords = (AmtInWords == "undefined") ? "" : AmtInWords;
            PFAmount = (PFAmount == "undefined") ? "" : PFAmount;
            DateofTrsfr = (DateofTrsfr == "undefined") ? "" : DateofTrsfr;
            Campus = (Campus == "undefined") ? "" : Campus;

            return new ActionAsPdf("CertificateComponents", new { staffId, certFy, Course, CurDesg, DateofRedsn, Letterdate, DateofRelv, RSCertfy, AmountSalryApr, DateofSalryApr, SalaryCerAmount, AmtInWords, PFAmount, DateofTrsfr, Campus })
            {
                FileName = staffId + ".pdf",
                PageSize = Size.A4,
                PageOrientation = Orientation.Portrait,
                PageMargins = { Left = 25, Right = 25 }
            };
        }
        public ActionResult CertificateComponents(long staffId, string certFy, string Course, string DateofRedsn, string CurDesg, string Letterdate, string DateofRelv, string RSCertfy, string AmountSalryApr, string DateofSalryApr, string SalaryCerAmount, string AmtInWords, string PFAmount, string DateofTrsfr, string Campus)
        {
            try
            {
                StaffDetails stfObj = smsObj.GetStaffDeatailsByPreRegNum(Convert.ToInt32(staffId));
                StaffDetails staffObj = smsObj.GetStaffDetailsId(stfObj.Id);
                staffObj.Course = Course;
                if (!string.IsNullOrEmpty(DateofRedsn))
                {
                    staffObj.DateofRedsn = DisplayDateMnthToMonthDate(DateofRedsn);
                }
                staffObj.CurDesg = CurDesg;
                if (!string.IsNullOrEmpty(Letterdate))
                {
                    staffObj.Letterdate = DisplayDateMnthToMonthDate(Letterdate);
                }
                if (!string.IsNullOrEmpty(staffObj.Gender))
                {
                    staffObj.Honor = staffObj.Gender.ToUpper() == "MALE" ? "His" : staffObj.Gender.ToUpper() == "FEMALE" ? "Her" : "";
                }
                if (!string.IsNullOrEmpty(DateofRelv))
                {
                    staffObj.DateofRelv = DisplayDateMnthToMonthDate(DateofRelv);
                }
                staffObj.RSCertfy = RSCertfy;
                staffObj.AmountSalryApr = AmountSalryApr == "" ? 0 : Convert.ToDouble(AmountSalryApr);
                if (!string.IsNullOrEmpty(DateofSalryApr))
                {
                    staffObj.DateofSalryApr = DisplayDateMnthToMonthDate(DateofSalryApr);
                }
                staffObj.SalaryCerAmount = SalaryCerAmount == "" ? 0 : Convert.ToDouble(SalaryCerAmount);
                staffObj.AmtInWords = AmtInWords;
                staffObj.PFAmount = PFAmount == "" ? 0 : Convert.ToDouble(PFAmount);
                if (!string.IsNullOrEmpty(DateofTrsfr))
                {
                    staffObj.DateofTrsfr = DisplayDateMnthToMonthDate(DateofTrsfr);
                }
                staffObj.TransferedCampus = Campus;
                smsObj.CreateOrUpdateStaffDetails(staffObj);
                ViewBag.Flag = certFy;
                return View(staffObj);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult StaffCredentialsPartialView(string PartialViewName, string certify, long PreRegNum)
        {
            ViewBag.PreRegNum = PreRegNum;
            ViewBag.CertiFy = certify;
            return PartialView(PartialViewName);
        }
        private DateTime DisplayDateMnthToMonthDate(string DateParam)
        {
            try
            {
                return DateTime.Parse(DateParam, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion "End"

        public void UpdateInbox(string Campus, string issue, long Id)
        {
            InboxService IBS = new InboxService();
            Inbox In = new Inbox();
            In.Campus = Campus;
            In.UserId = base.ValidateUser();
            In.InformationFor = issue;
            In.CreatedDate = DateTime.Now;
            In.Module = "Staff Management";
            In.Status = "Inbox";
            In.Campus = Campus;
            In.PreRegNum = Id;
            In.RefNumber = Id;
            IBS.CreateOrUpdateInbox(In);
        }

        #region CaptureImage Processing

        public ActionResult StaffCaptureImage(string PreRegNo)
        {
            ViewBag.PreRegNum = PreRegNo;
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

                Session["imgpath"] = path;

                Session["imgval"] = ConfigurationManager.AppSettings["ImageCropShow"] + date + "test.jpg";
            }
            return Json("", JsonRequestBehavior.AllowGet);
            //return View("CaptureImage");
        }

        public JsonResult Rebind()
        {
            string path = Session["imgval"].ToString();
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
            if (string.IsNullOrEmpty(Session["imgpath"].ToString()) || !cropPointX.HasValue || !cropPointY.HasValue || !imageCropWidth.HasValue || !imageCropHeight.HasValue)
            {
                return new HttpStatusCodeResult((int)HttpStatusCode.BadRequest);
            }


            byte[] imageBytes = System.IO.File.ReadAllBytes(Session["imgpath"].ToString());
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
            Session["imgCropedPath"] = Path.Combine(tempFolderName, fileName);
            string photoPath = ConfigurationManager.AppSettings["ImageCropShow"] + fileName;
            return Json(photoPath, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UploadCropedPhotos(string docType, string documentFor, long RegNo)
        {
            AdmissionManagementService ads = new AdmissionManagementService();
            //HttpPostedFileBase theFile = HttpContext.Request.Files["uploadedFile"];
            //string path = uploadedFile.InputStream.ToString();
            byte[] imageSize = System.IO.File.ReadAllBytes(Session["imgCropedPath"].ToString());
            //uploadedFile.InputStream.Read(imageSize, 0, (int)uploadedFile.ContentLength);
            UploadedFiles fu = new UploadedFiles();
            fu.DocumentFor = documentFor;
            fu.DocumentType = docType;
            fu.PreRegNum = RegNo;
            fu.DocumentData = imageSize;
            fu.DocumentName = Path.GetFileName(Session["imgCropedPath"].ToString());
            fu.DocumentSize = imageSize.Length.ToString();
            fu.UploadedDate = DateTime.Now.Date.ToString("dd/MM/yyyy");
            ads.CreateOrUpdateUploadedFiles(fu);
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

        #region Staff Events
        public ActionResult StaffEvent()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    ViewBag.UserName = userId;
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StaffIssuesCategory");
                throw ex;
            }
        }

        public ActionResult CreateEvent()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StaffIssuesCategory");
                throw ex;
            }
        }

        //public ActionResult AddEvent(string eventTitle, string eventDescription, string staffId, string eventFor)
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        StaffManagementService sms = new StaffManagementService();
        //        Event stfevent = new Event();
        //        stfevent.EventTitle = eventTitle;
        //        stfevent.EventDescription = eventDescription;
        //        stfevent.EventFor = eventFor;
        //        stfevent.CreatedBy = userId;
        //        stfevent.CreatedDate = DateTime.Now;
        //        stfevent.ModifiedBy = userId;
        //        stfevent.ModifiedDate = DateTime.Now;

        //        sms.CreateOrUpdateEvents(stfevent);
        //        return Json("Sucess", JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "StaffIssuesCategory");
        //        throw ex;
        //    }
        //}

        public ActionResult AddEvent(string title, string userId, string eventFor)
        {
            try
            {

                StaffManagementService sms = new StaffManagementService();
                Event stfevent = new Event();
                stfevent.EventTitle = title;
                stfevent.EventFor = eventFor;
                stfevent.CreatedBy = userId;
                stfevent.CreatedDate = DateTime.Now;
                stfevent.ModifiedBy = userId;
                stfevent.ModifiedDate = DateTime.Now;
                sms.CreateOrUpdateEvents(stfevent);
                return Json(stfevent.EventId, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StaffIssuesCategory");
                throw ex;
            }
        }

        public ActionResult AddEventList(string events, long? Id, string eventFor)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {

                    StaffManagementService sms = new StaffManagementService();
                    Event evnt = sms.GetEventsById(Id ?? 0);
                    EventList eventlist = new EventList();
                    eventlist.EventListDescription = events;
                    eventlist.EventId = Id ?? 0;
                    eventlist.CreatedBy = userId;
                    eventlist.CreatedDate = DateTime.Now;
                    eventlist.ModifiedBy = userId;
                    eventlist.ModifiedDate = DateTime.Now;
                    sms.CreateOrUpdateEventList(eventlist);
                    evnt.EventListCount = evnt.EventListCount + 1;
                    sms.CreateOrUpdateEvents(evnt);
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StaffIssuesCategory");
                throw ex;
            }
        }

        //public ActionResult GetEventList(long EventId)
        //{
        //}
        #endregion

        #region Staff promotion Details

        public ActionResult PromotionOfStaff(long? StaffID)
        {
            FillViewBag();
            StaffPromotionAndTransferDetails staffPromObj = new StaffPromotionAndTransferDetails();
            StaffDetails staffObj = smsObj.GetStaffDetailsId(Convert.ToInt32(StaffID));
            if (staffObj != null)
            {
                staffPromObj.StaffID = staffObj.Id;
                staffPromObj.StaffName = staffObj.Name;
                staffPromObj.BeforeCampus = staffObj.Campus;
                staffPromObj.BeforeDesignation = staffObj.Designation;
                staffPromObj.BeforeDepartment = staffObj.Department;
                staffPromObj.CreatedBy = base.ValidateUser();
            }
            return PartialView("PromotionOfStaff", staffPromObj);
        }

        [HttpPost]
        public ActionResult PromotionOfStaff(StaffPromotionAndTransferDetails stffObj)
        {

            try
            {
                stffObj.CreatedDate = DateTime.Now;
                smsObj.CreateOrUpdateStaffPromotionAndTransferDetails(stffObj);
                StaffDetails Objlst = smsObj.GetStaffDetailsId(stffObj.StaffID);
                if (Objlst != null)
                {
                    Objlst.Campus = stffObj.AfterCampus;
                    Objlst.Designation = stffObj.AfterDesignation;
                    Objlst.Department = stffObj.AfterDepartment;
                    smsObj.CreateOrUpdateStaffDetails(Objlst);
                }
            }
            catch (Exception)
            {

                throw;
            }


            return RedirectToAction("StaffDisplay", "StaffManagement");
        }
        #endregion "End"

        #region StaffEvaluationCategoryMaster by Prabakaran
        public ActionResult StaffEvaluationCategoryMaster()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
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
                    #region BreadCrumb
                    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                    #endregion
                    ViewBag.currentAcyear = DateTime.Now.Month > 5 ? DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Year + 1).ToString() : (DateTime.Now.Year - 1).ToString() + "-" + DateTime.Now.Year.ToString();
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult StaffEvaluationCategoryMasterJqgrid(string Campus, string Grade, string AcademicYear, string Section, string CategoryName, string IsActive, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<string, object> likeCriteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(Campus))
                    likeCriteria.Add("Campus", Campus);
                if (!string.IsNullOrEmpty(Grade))
                    criteria.Add("Grade", Grade);
                if (!string.IsNullOrEmpty(Section))
                    criteria.Add("Section", Section);
                if (!string.IsNullOrEmpty(AcademicYear))
                    criteria.Add("AcademicYear", AcademicYear);
                if (!string.IsNullOrEmpty(CategoryName))
                    likeCriteria.Add("CategoryName", CategoryName);
                if (!string.IsNullOrEmpty(IsActive))
                {
                    if (IsActive == "true" || IsActive == "True")
                    {
                        criteria.Add("IsActive", true);
                    }
                    if (IsActive == "false" || IsActive == "False")
                    {
                        criteria.Add("IsActive", false);
                    }
                }
                Dictionary<long, IList<StaffEvaluationCategoryMaster>> StaffEvaluationCategoryList = sms.GetStaffEvaluationCategoryListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria, likeCriteria);
                if (StaffEvaluationCategoryList != null && StaffEvaluationCategoryList.Count > 0)
                {
                    long totalrecords = StaffEvaluationCategoryList.First().Key;
                    int totalpages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalpages,
                        page = page,
                        records = totalrecords,
                        rows = (from items in StaffEvaluationCategoryList.FirstOrDefault().Value
                                select new
                                {
                                    cell = new string[]{
                                          items.StaffEvaluationCategoryId.ToString(),
                                          items.AcademicYear,
                                          items.Campus,
                                          items.Grade,
                                          items.Section,
                                          items.CategoryName, 
                                          items.EvaluationDate.ToString("dd/MM/yyyy"),
                                          items.OTP.ToString(),
                                          items.IsActive==true?GetValidByEvaluationDate(items.EvaluationDate,items.StaffEvaluationCategoryId):"In Valid",
                                          items.CreatedBy,
                                          items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy"):"",
                                          items.ModifiedBy,
                                          items.ModifiedDate!=null?items.ModifiedDate.Value.ToString("dd/MM/yyyy"):""
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
        public ActionResult AddStaffEvaluationCategory(StaffEvaluationCategoryMaster staffevaluationcategory)
        {
            try
            {
                IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                string userId = ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    StaffManagementService sms = new StaffManagementService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<string, object> likeCriteria = new Dictionary<string, object>();
                    criteria.Add("AcademicYear", staffevaluationcategory.AcademicYear);
                    criteria.Add("IsActive", true);
                    criteria.Add("Campus", staffevaluationcategory.Campus);
                    criteria.Add("Grade", staffevaluationcategory.Grade);
                    criteria.Add("Section", staffevaluationcategory.Section);
                    Dictionary<long, IList<StaffEvaluationCategoryMaster>> StaffEvaluationCategoryList = sms.GetStaffEvaluationCategoryListWithPagingAndCriteria(null, null, null, null, criteria, likeCriteria);
                    criteria.Add("SurveyNumber", staffevaluationcategory.CategoryName);
                    Dictionary<long, IList<SurveyConfiguration>> surveyconfig = sms.GetSurveyConfigurationListWithPagingAndCriteria(0, 99999, null, null, criteria, likeCriteria);
                    if (surveyconfig == null || surveyconfig.FirstOrDefault().Key == 0)
                    {
                        return Json("notexist", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        if (StaffEvaluationCategoryList != null && StaffEvaluationCategoryList.Count > 0 && StaffEvaluationCategoryList.FirstOrDefault().Key != 0 && StaffEvaluationCategoryList.FirstOrDefault().Value[0].IsActive == true)
                        {
                            StaffEvaluationCategoryList.FirstOrDefault().Value[0].IsActive = false;
                            sms.SaveOrUpdateStaffEvaluationCateogry(StaffEvaluationCategoryList.FirstOrDefault().Value[0]);
                        }
                        Random rnd = new Random();
                        staffevaluationcategory.OTP = rnd.Next(0, 1000000);
                        if (!string.IsNullOrWhiteSpace(Request["EvaluationDate"]))
                        {
                            //staffevaluationcategory.EvaluationDate = DateTime.Parse(Request["EvaluationDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                            staffevaluationcategory.EvaluationDate = DateTime.ParseExact(Request["EvaluationDate"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        staffevaluationcategory.IsActive = true;
                        staffevaluationcategory.CreatedBy = userId;
                        staffevaluationcategory.CreatedDate = DateTime.Now;
                        staffevaluationcategory.ModifiedBy = userId;
                        staffevaluationcategory.ModifiedDate = DateTime.Now;
                        sms.SaveOrUpdateStaffEvaluationCateogry(staffevaluationcategory);
                        return Json("success", JsonRequestBehavior.AllowGet);
                    }
                }
                //return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult EditStaffEvaluationCategory(StaffEvaluationCategoryMaster staffevaluationcategory)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (staffevaluationcategory.StaffEvaluationCategoryId > 0)
                    {
                        StaffEvaluationCategoryMaster Obj = new StaffEvaluationCategoryMaster();
                        StaffManagementService sms = new StaffManagementService();
                        Obj = sms.GetStaffEvaluationCategoryById(staffevaluationcategory.StaffEvaluationCategoryId);
                        if (Obj != null)
                        {
                            Obj.ModifiedBy = userId;
                            Obj.ModifiedDate = DateTime.Now;
                            Obj.IsActive = staffevaluationcategory.IsActive;
                            sms.SaveOrUpdateStaffEvaluationCateogry(Obj);
                        }
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
        public string GetValidByEvaluationDate(DateTime EvaluationDate, long StaffEvaluationCategoryId)
        {
            if (EvaluationDate.Date < DateTime.Now.Date)
            {
                StaffEvaluationCategoryMaster staffEvaluationCategoryMaster = smsObj.GetStaffEvaluationCategoryById(StaffEvaluationCategoryId);
                staffEvaluationCategoryMaster.IsActive = false;
                smsObj.SaveOrUpdateStaffEvaluationCateogry(staffEvaluationCategoryMaster);
                return "In Valid";
            }
            else
            {
                return "Valid";
            }
        }
        #endregion

        #region Staff Evaluation Questionnaires by Prabakaran
        public ActionResult StaffEvaluationQuestionnaires()
        {
            try
            {
                #region BreadCrumb
                string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                #endregion
                return View();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult StaffEvaluationQuestionnairesJqgrid(string Campus, string Grade, string AcademicYear, string CategoryName, string IsActive, string IsPositive, string StaffEvaluationParameters, string Month, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<string, object> likeCriteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(Campus))
                {
                    likeCriteria.Add("Campus", Campus);
                }
                if (!string.IsNullOrEmpty(Grade))
                {
                    criteria.Add("Grade", Grade);
                }
                if (!string.IsNullOrEmpty(AcademicYear))
                {
                    criteria.Add("AcademicYear", AcademicYear);
                }
                if (!string.IsNullOrEmpty(Month))
                {
                    criteria.Add("Month", Month);
                }
                if (!string.IsNullOrEmpty(StaffEvaluationParameters))
                {
                    likeCriteria.Add("StaffEvaluationParameters", StaffEvaluationParameters);
                }
                if (!string.IsNullOrEmpty(CategoryName))
                {
                    likeCriteria.Add("CategoryName", CategoryName);
                }
                if (!string.IsNullOrEmpty(IsActive))
                {
                    if (IsActive == "true" || IsActive == "True")
                    {
                        criteria.Add("IsActive", true);
                    }
                    if (IsActive == "false" || IsActive == "False")
                    {
                        criteria.Add("IsActive", false);
                    }
                }
                if (!string.IsNullOrEmpty(IsPositive))
                {
                    if (IsPositive == "true" || IsPositive == "True")
                    {
                        criteria.Add("IsPositive", true);
                    }
                    if (IsPositive == "false" || IsPositive == "False")
                    {
                        criteria.Add("IsPositive", false);
                    }
                }

                //Dictionary<long, IList<StaffEvaluationParameter>> StaffEvaluationParameterList = sms.GetStaffEvaluationParameterListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria, likeCriteria);
                Dictionary<long, IList<StaffEvaluationParameter_vw>> StaffEvaluationParameterList = sms.GetStaffEvaluationParameter_vwListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria, likeCriteria);
                if (StaffEvaluationParameterList != null && StaffEvaluationParameterList.Count > 0)
                {
                    long totalrecords = StaffEvaluationParameterList.First().Key;
                    int totalpages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalpages,
                        page = page,
                        records = totalrecords,
                        rows = (from items in StaffEvaluationParameterList.FirstOrDefault().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[]{
                                          items.Id.ToString(),
                                          items.StaffEvaluationParameterId.ToString(),
                                          items.StaffEvaluationCategoryId.ToString(),
                                          items.Campus,
                                          items.Grade,
                                          items.AcademicYear,
                                          items.Month,
                                          items.CategoryName,
                                          items.StaffEvaluationParameters,
                                          items.IsPositive == true?"Yes":"No",
                                          items.IsActive == true?"Yes":"No",                                          
                                          //items.CreatedBy,
                                          //items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy"):"",
                                          //items.ModifiedBy,
                                          //items.ModifiedDate!=null?items.ModifiedDate.Value.ToString("dd/MM/yyyy"):""
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
        public ActionResult AddStaffEvaluationParameter(StaffEvaluationParameter staffevaluationparameter)
        {
            try
            {
                IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                string userId = ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    StaffManagementService sms = new StaffManagementService();
                    staffevaluationparameter.IsActive = true;
                    staffevaluationparameter.CreatedBy = userId;
                    staffevaluationparameter.CreatedDate = DateTime.Now;
                    sms.SaveOrUpdateStaffEvaluationParameter(staffevaluationparameter);
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult EditStaffEvaluationParameter(StaffEvaluationParameter staffevaluationparameter)
        {
            try
            {
                IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                string userId = ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    StaffManagementService sms = new StaffManagementService();
                    if (staffevaluationparameter.StaffEvaluationParameterId > 0)
                    {
                        StaffEvaluationParameter sep = sms.GetStaffEvaluationParameterById(staffevaluationparameter.StaffEvaluationParameterId);
                        if (sep != null)
                        {
                            sep.IsActive = staffevaluationparameter.IsActive;
                            sep.IsPositive = staffevaluationparameter.IsPositive;
                            sep.StaffEvaluationParameters = staffevaluationparameter.StaffEvaluationParameters;
                            sep.ModifiedBy = userId;
                            sep.ModifiedDate = DateTime.Now;
                            sms.SaveOrUpdateStaffEvaluationParameter(sep);
                            return Json("success", JsonRequestBehavior.AllowGet);
                        }
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
        public ActionResult FillCategoryName(string Campus, string Grade, string AcademicYear, string Month)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    StaffManagementService sms = new StaffManagementService();
                    Dictionary<string, Object> criteria = new Dictionary<string, object>();
                    Dictionary<string, Object> likeCriteria = new Dictionary<string, object>();
                    if (!string.IsNullOrEmpty(Campus) && !string.IsNullOrEmpty(Grade) && !string.IsNullOrEmpty(AcademicYear))
                    {
                        criteria.Add("Campus", Campus);
                        criteria.Add("Grade", Grade);
                        criteria.Add("AcademicYear", AcademicYear);
                        criteria.Add("Month", Month);
                        criteria.Add("IsActive", true);
                        Dictionary<long, IList<StaffEvaluationCategoryMaster>> CategoryNameList = sms.GetStaffEvaluationCategoryListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria, likeCriteria);
                        var CategoryName = (
                              from items in CategoryNameList.FirstOrDefault().Value
                              select new
                              {
                                  Text = items.CategoryName,
                                  Value = items.StaffEvaluationCategoryId
                              }).ToList();
                        return Json(CategoryName, JsonRequestBehavior.AllowGet);
                    }
                    else
                        return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        //public string GetCategoryName(long Id)
        //{
        //    StaffManagementService sms = new StaffManagementService();
        //    if (Id > 0)
        //    {
        //        StaffEvaluationCategoryMaster scmaster = sms.GetStaffEvaluationCategoryById(Id);
        //        if (scmaster != null)
        //        {
        //            return scmaster.CategoryName;
        //        }
        //    }
        //    return "";

        //}
        public ActionResult CategoryNameddl()
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();
                Dictionary<string, string> aca = new Dictionary<string, string>();

                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<string, object> likeCriteria = new Dictionary<string, object>();

                Dictionary<long, IList<StaffEvaluationCategoryMaster>> CategoryNameList = sms.GetStaffEvaluationCategoryListWithPagingAndCriteria(null, null, null, null, criteria, likeCriteria);
                foreach (var cn in CategoryNameList.First().Value)
                {
                    if (aca.Keys.Contains(cn.CategoryName))
                    {
                        aca.Remove(cn.CategoryName);
                    }
                    aca.Add(cn.CategoryName, cn.CategoryName);
                }
                return PartialView("Dropdown", aca);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        #endregion

        #region Added By Prabakaran CampusBasedStaffDetails
        public ActionResult CampusBasedStaffDetails()
        {
            try
            {
                string userId = ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
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
                    #region BreadCrumb
                    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                    #endregion
                    ViewBag.campusddl = CampusMaster.First().Value;
                    ViewBag.gradeddl = GradeMaster.First().Value;
                    ViewBag.acadddl = AcademicyrMaster.First().Value;
                    FillViewBag();
                    return View();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult CampusBasedStaffDetails_VwJqGrid(CampusBasedStaffDetails campusbasedstaffdetails, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<string, object> likecriteria = new Dictionary<string, object>();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (campusbasedstaffdetails != null)
                {
                    if (!string.IsNullOrEmpty(campusbasedstaffdetails.Campus))
                    { criteria.Add("Campus", campusbasedstaffdetails.Campus); }
                    if (!string.IsNullOrEmpty(campusbasedstaffdetails.Department))
                    { criteria.Add("Department", campusbasedstaffdetails.Department); }
                    if (!string.IsNullOrEmpty(campusbasedstaffdetails.SubDepartment))
                    { criteria.Add("SubDepartment", campusbasedstaffdetails.SubDepartment); }
                    if (!string.IsNullOrEmpty(campusbasedstaffdetails.Programme))
                    { criteria.Add("Programme", campusbasedstaffdetails.Programme); }
                    if (!string.IsNullOrEmpty(campusbasedstaffdetails.Designation))
                    { criteria.Add("Designation", campusbasedstaffdetails.Designation); }
                    if (!string.IsNullOrEmpty(campusbasedstaffdetails.StaffName))
                    { likecriteria.Add("StaffName", campusbasedstaffdetails.StaffName); }
                    if (!string.IsNullOrEmpty(campusbasedstaffdetails.StaffType))
                    { criteria.Add("StaffType", campusbasedstaffdetails.StaffType); }
                    if (!string.IsNullOrEmpty(campusbasedstaffdetails.AcademicYear))
                    { criteria.Add("AcademicYear", campusbasedstaffdetails.AcademicYear); }
                    //if (!string.IsNullOrEmpty(campusbasedstaffdetails.ConfigurationStatus))
                    //{ criteria.Add("ConfigurationStatus", campusbasedstaffdetails.ConfigurationStatus); }
                    if (!string.IsNullOrEmpty(campusbasedstaffdetails.ConfigurationStatus))
                    {
                        if (campusbasedstaffdetails.ConfigurationStatus == "OverAllNotConfigured")
                        {
                            string[] StatusArray = new string[3];
                            StatusArray[0] = "OverAllNotConfigured";
                            StatusArray[1] = "ReportingHeadNonAssigned";
                            StatusArray[2] = "GradeSectionSubjectNonAssigned";
                            criteria.Add("ConfigurationStatus", StatusArray);
                        }
                        else
                            criteria.Add("ConfigurationStatus", campusbasedstaffdetails.ConfigurationStatus);
                    }
                    if (!string.IsNullOrEmpty(campusbasedstaffdetails.ReportingHeads) && !string.IsNullOrEmpty(campusbasedstaffdetails.Campus))
                        BuildCriteriaForReportingHeadsBasedOnDesignation(campusbasedstaffdetails.Campus, campusbasedstaffdetails.ReportingHeads, criteria);
                    //if (!string.IsNullOrEmpty(campusbasedstaffdetails.ReportingHeads))
                    //{ likecriteria.Add("ReportingHeads", campusbasedstaffdetails.ReportingHeads); }
                    if (campusbasedstaffdetails != null)
                        BuidCriteriaForGrdeSearch(campusbasedstaffdetails.Campus, campusbasedstaffdetails.Grade, campusbasedstaffdetails.Section, criteria);
                }
                Dictionary<long, IList<CampusBasedStaffDetails_Vw>> CampusBasedStaffDetails = null;
                CampusBasedStaffDetails = smsObj.GetCampusBasedStaffDetails_VwListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria, likecriteria);

                if (CampusBasedStaffDetails != null && CampusBasedStaffDetails.FirstOrDefault().Key > 0)
                {
                    IList<CampusBasedStaffDetails_Vw> cbsd = CampusBasedStaffDetails.FirstOrDefault().Value;
                    long totalRecords = CampusBasedStaffDetails.FirstOrDefault().Key;
                    int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                    var jsonData = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalRecords,
                        rows =
                        (
                        from items in cbsd
                        select new
                        {
                            i = items.Id,
                            cell = new string[]
                           {
                                items.Id.ToString(),     
                                items.StaffPreRegNumber.ToString(),
                                items.Campus,
                                //items.StaffName,
                                !string.IsNullOrEmpty(campusbasedstaffdetails.Grade)?BuidSubjectArrayWithStaffName(items.StaffPreRegNumber,items.StaffName):items.StaffName,
                                items.Department,
                                items.SubDepartment,
                                items.Programme,
                                items.Designation,
                                items.AcademicYear,
                                "<img src='/Images/History.png ' id='ImgHistory' onclick=\"ShowCampusBasedStaffDetails('" + items.StaffPreRegNumber +"','"+items.Campus+"');\" />",
                           }
                        }
                        )
                    };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var Empty = new { rows = (new { cell = new string[] { } }) };
                    return Json(Empty, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        private void BuidCriteriaForGrdeSearch(string Campus, string Grade, string Section, Dictionary<string, object> criteria)
        {
            try
            {
                QAService QAs = new QAService();
                Dictionary<string, object> LocalExactCriteria = new Dictionary<string, object>();
                Dictionary<string, object> LocalLikeCriteria = new Dictionary<string, object>();
                StaffManagementService smsObj = new StaffManagementService();
                if (!string.IsNullOrEmpty(Grade) || !string.IsNullOrEmpty(Section))
                {
                    if (!string.IsNullOrEmpty(Campus))
                        LocalExactCriteria.Add("Campus", Campus);
                    if (!string.IsNullOrEmpty(Grade))
                        LocalExactCriteria.Add("Grade", Grade);
                    if (!string.IsNullOrEmpty(Section))
                        LocalExactCriteria.Add("Section", Section);
                    Dictionary<long, IList<CampusBasedStaffDetails>> CampusBasedStaffDetails = null;
                    CampusBasedStaffDetails = QAs.GetCampusBasedStaffDetailsListWithPagingAndCriteria(null, 99999, string.Empty, string.Empty, LocalExactCriteria, LocalLikeCriteria);
                    if (CampusBasedStaffDetails != null && CampusBasedStaffDetails.FirstOrDefault().Key > 0)
                    {
                        var GradeSectionPreRegNumArray = (from u in CampusBasedStaffDetails.FirstOrDefault().Value
                                                          select u.StaffPreRegNumber).Distinct().ToArray();
                        if (GradeSectionPreRegNumArray != null && GradeSectionPreRegNumArray.Length > 0)
                            criteria.Add("StaffPreRegNumber", GradeSectionPreRegNumArray);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        //private void BuildCriteriaForReportingHeadsBasedOnDesignation(string Campus, string ReportingHeads, Dictionary<string, object> criteria)
        //{

        //    Dictionary<string, object> LocalCriteria = new Dictionary<string, object>();
        //    StaffManagementService smsObj = new StaffManagementService();
        //    if (!string.IsNullOrEmpty(Campus))
        //        LocalCriteria.Add("Campus", Campus);
        //    if (!string.IsNullOrEmpty(ReportingHeads))
        //        LocalCriteria.Add("Designation", ReportingHeads);
        //    LocalCriteria.Add("IsReportingManager", true);
        //    LocalCriteria.Add("Status", "Registered");
        //    Dictionary<long, IList<StaffDetailsView>> StaffDetailsViewDetails = smsObj.GetStaffDetailsViewListWithPaging(null, 99999, string.Empty, string.Empty, LocalCriteria);
        //    if (StaffDetailsViewDetails != null && StaffDetailsViewDetails.FirstOrDefault().Key > 0)
        //    {
        //        var ReportingsPreRegNumArray = (from u in StaffDetailsViewDetails.FirstOrDefault().Value
        //                                        where u.PreRegNum>0
        //                                        select Convert.ToInt64(u.PreRegNum)).Distinct().ToArray();
        //        IList<Staff_AttendanceReportConfigurationByStaffs> ConfigurationList = new List<Staff_AttendanceReportConfigurationByStaffs>();
        //        ConfigurationList = smsObj.GetStaff_AttendanceReportConfigurationsListBasedOnReportingHeadPreRegNums(ReportingsPreRegNumArray);
        //        if (ConfigurationList != null && ConfigurationList.Count > 0)
        //        {
        //            var StaffPreRegNums = (from u in ConfigurationList
        //                                   select u.StaffPreRegNum).Distinct().ToArray();
        //            criteria.Add("StaffPreRegNumber", StaffPreRegNums);
        //        }
        //    }
        //}
        private void BuildCriteriaForReportingHeadsBasedOnDesignation(string Campus, string ReportingHeads, Dictionary<string, object> criteria)
        {

            Dictionary<string, object> LocalCriteria = new Dictionary<string, object>();
            StaffManagementService smsObj = new StaffManagementService();
            if (!string.IsNullOrEmpty(Campus))
                LocalCriteria.Add("Campus", Campus);
            if (!string.IsNullOrEmpty(ReportingHeads))
                LocalCriteria.Add("ReportingHeadPreRegNum", Convert.ToInt64(ReportingHeads));
            Dictionary<long, IList<Staff_AttendanceReportConfigurationByStaffs>> Staff_AttendanceReportConfigurationByStaffsDetails = smsObj.GetStaff_AttendanceReportConfigurationByStaffsListWithPagingAndCriteria(null, 99999, string.Empty, string.Empty, LocalCriteria);
            if (Staff_AttendanceReportConfigurationByStaffsDetails != null && Staff_AttendanceReportConfigurationByStaffsDetails.FirstOrDefault().Key > 0)
            {
                var StaffPreRegNums = (from u in Staff_AttendanceReportConfigurationByStaffsDetails.FirstOrDefault().Value
                                       where u.StaffPreRegNum > 0
                                       select u.StaffPreRegNum).Distinct().ToArray();
                criteria.Add("StaffPreRegNumber", StaffPreRegNums);
            }
        }
        private string BuidSubjectArrayWithStaffName(long StaffPreRegNumber, string StaffName)
        {
            try
            {
                string GradeArrayWithStaffName = string.Empty;
                IList<CampusBasedStaffDetails> CampusBasedStaffDetailsListByStaffPreRegNum = new List<CampusBasedStaffDetails>();
                CampusBasedStaffDetailsListByStaffPreRegNum = smsObj.GetCampusBasedStaffDetailsByStaffsByStaffPreRegNumber(StaffPreRegNumber);
                if (CampusBasedStaffDetailsListByStaffPreRegNum != null && CampusBasedStaffDetailsListByStaffPreRegNum.Count > 0)
                {
                    var SubjectsArray = (from u in CampusBasedStaffDetailsListByStaffPreRegNum
                                         where !string.IsNullOrEmpty(u.Subject)
                                         select u.Subject).Distinct().ToArray();
                    if (SubjectsArray != null && SubjectsArray.Length > 0)
                    {
                        //GradeArrayWithStaffName = GradeArrayWithStaffName + " [";
                        for (int i = 0; i < SubjectsArray.Length; i++)
                        {
                            if (i == 0)
                                GradeArrayWithStaffName = GradeArrayWithStaffName + SubjectsArray[i];
                            else
                                GradeArrayWithStaffName = GradeArrayWithStaffName + "," + SubjectsArray[i];
                        }
                        //GradeArrayWithStaffName = GradeArrayWithStaffName + " ]";
                    }
                }
                GradeArrayWithStaffName = StaffName + " [" + GradeArrayWithStaffName + "]";
                return GradeArrayWithStaffName;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult CampusBasedStaffDetailsJqGrid(CampusBasedStaffDetails campusbasedstaffdetails, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                QAService QASrvc = new QAService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<string, object> likecriteria = new Dictionary<string, object>();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (campusbasedstaffdetails != null)
                {
                    if (!string.IsNullOrEmpty(campusbasedstaffdetails.Campus))
                    { likecriteria.Add("Campus", campusbasedstaffdetails.Campus); }
                    if (!string.IsNullOrEmpty(campusbasedstaffdetails.Grade))
                    { criteria.Add("Grade", campusbasedstaffdetails.Grade); }
                    if (!string.IsNullOrEmpty(campusbasedstaffdetails.Section))
                    { criteria.Add("Section", campusbasedstaffdetails.Section); }
                    if (!string.IsNullOrEmpty(campusbasedstaffdetails.AcademicYear))
                    { criteria.Add("AcademicYear", campusbasedstaffdetails.AcademicYear); }
                    if (!string.IsNullOrEmpty(campusbasedstaffdetails.Subject))
                    { criteria.Add("Subject", campusbasedstaffdetails.Subject); }
                    if (campusbasedstaffdetails.StaffPreRegNumber > 0)
                    { criteria.Add("StaffPreRegNumber", campusbasedstaffdetails.StaffPreRegNumber); }
                    if (!string.IsNullOrEmpty(campusbasedstaffdetails.StaffName))
                    { likecriteria.Add("StaffName", campusbasedstaffdetails.StaffName); }
                    if (!string.IsNullOrEmpty(campusbasedstaffdetails.UserId))
                    { likecriteria.Add("UserId", campusbasedstaffdetails.UserId); }
                }
                Dictionary<long, IList<CampusBasedStaffDetails>> CampusBasedStaffDetails = null;
                CampusBasedStaffDetails = QASrvc.GetCampusBasedStaffDetailsListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria, likecriteria);

                if (CampusBasedStaffDetails != null && CampusBasedStaffDetails.FirstOrDefault().Key > 0)
                {
                    IList<CampusBasedStaffDetails> cbsd = CampusBasedStaffDetails.FirstOrDefault().Value;
                    long totalRecords = CampusBasedStaffDetails.FirstOrDefault().Key;
                    int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                    var jsonData = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalRecords,
                        rows =
                        (
                        from items in cbsd
                        select new
                        {
                            i = items.Id,
                            cell = new string[]
                           {
                                items.Id.ToString(),     
                                items.StaffPreRegNumber.ToString(),
                                items.Campus,
                                items.Grade,
                                items.Section,
                                items.Subject,
                                items.AcademicYear
                           }
                        }
                        )
                    };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var Empty = new { rows = (new { cell = new string[] { } }) };
                    return Json(Empty, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult StaffNameAutoComplete(string term, string Campus)
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(Campus))
                { criteria.Add("Campus", Campus); }
                if (!string.IsNullOrEmpty(term))
                { criteria.Add("Name", term); }
                Dictionary<long, IList<StaffDetailsView>> StaffDetails = sms.GetStaffDetailsViewListWithPaging(0, 9999, "Name", string.Empty, criteria);
                if (StaffDetails.Count > 0 && StaffDetails.FirstOrDefault().Key > 0)
                {
                    var StaffNames = (from u in StaffDetails.FirstOrDefault().Value where u.Name != null select new { Name = u.Name, PreRegNum = u.PreRegNum }).Distinct().ToList();
                    return Json(StaffNames, JsonRequestBehavior.AllowGet);
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult AddCampusBasedStaffDetails(CampusBasedStaffDetails campusbasedstaffdetails, long StaffPreRegNum, string[] ReportingHeadPreRegNums)
        {
            try
            {
                IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                string userId = ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    campusbasedstaffdetails.StaffPreRegNumber = StaffPreRegNum;
                    StaffDetailsView StaffDetailsObj = new StaffDetailsView();
                    StaffDetailsObj = smsObj.GetStaffDetailsViewByPreRegNum(Convert.ToInt32(StaffPreRegNum));
                    if (StaffDetailsObj != null)
                    {
                        campusbasedstaffdetails.Campus = StaffDetailsObj.Campus;
                        campusbasedstaffdetails.StaffName = StaffDetailsObj.Name;
                        campusbasedstaffdetails.UserId = StaffDetailsObj.StaffUserName;
                    }
                    //StaffManagementService sms = new StaffManagementService();
                    //if (ReportingHeadPreRegNums != null && ReportingHeadPreRegNums.Length > 0)
                    //{
                    //    var ReportingHeadPreRegNumsArray = ReportingHeadPreRegNums[0].Split(',');
                    //    sms.AddOrUpdateStaff_AttendanceReportConfigurationByStaffs(campusbasedstaffdetails.StaffPreRegNumber, campusbasedstaffdetails.Campus, ReportingHeadPreRegNumsArray, userId);
                    //}
                    QAService QAs = new QAService();
                    IList<CampusBasedStaffDetails> CampusBasedStaffDtlsList = new List<CampusBasedStaffDetails>();
                    IList<CampusBasedStaffDetails> StaffEvaluationCategoryMasterList1 = new List<CampusBasedStaffDetails>();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<string, object> likeCriteria = new Dictionary<string, object>();
                    string[] ArrGrade = campusbasedstaffdetails.Grade.Split(',');
                    string[] ArrSection = campusbasedstaffdetails.Section.Split(',');
                    if (campusbasedstaffdetails != null)
                    {
                        if (!string.IsNullOrEmpty(campusbasedstaffdetails.Campus))
                        {
                            criteria.Add("Campus", campusbasedstaffdetails.Campus);
                        }
                        if (!string.IsNullOrEmpty(campusbasedstaffdetails.AcademicYear))
                        {
                            criteria.Add("AcademicYear", campusbasedstaffdetails.AcademicYear);
                        }
                        if (campusbasedstaffdetails.StaffPreRegNumber > 0)
                        {
                            criteria.Add("StaffPreRegNumber", campusbasedstaffdetails.StaffPreRegNumber);
                        }
                        if (!string.IsNullOrEmpty(campusbasedstaffdetails.Subject))
                        {
                            criteria.Add("Subject", campusbasedstaffdetails.Subject);
                        }
                    }
                    if (!string.IsNullOrEmpty(campusbasedstaffdetails.Campus))
                    {
                        foreach (var Gradeitems in ArrGrade)
                        {
                            if (!string.IsNullOrEmpty(Gradeitems))
                            {
                                criteria.Add("Grade", Gradeitems);
                                foreach (var Sectionitems in ArrSection)
                                {
                                    if (!string.IsNullOrEmpty(Sectionitems))
                                    {
                                        criteria.Add("Section", Sectionitems);
                                        Dictionary<long, IList<CampusBasedStaffDetails>> CampusBasedStaffDetailsList = QAs.GetCampusBasedStaffDetailsListWithPagingAndCriteria(null, null, null, null, criteria, likeCriteria);
                                        if (CampusBasedStaffDetailsList.FirstOrDefault().Key == 0)
                                        {
                                            CampusBasedStaffDetails cbsd = new CampusBasedStaffDetails();
                                            cbsd.Campus = campusbasedstaffdetails.Campus;
                                            cbsd.Grade = Gradeitems;
                                            cbsd.AcademicYear = campusbasedstaffdetails.AcademicYear;
                                            cbsd.StaffName = campusbasedstaffdetails.StaffName;
                                            cbsd.StaffPreRegNumber = campusbasedstaffdetails.StaffPreRegNumber;
                                            cbsd.Subject = campusbasedstaffdetails.Subject;
                                            cbsd.Section = Sectionitems;
                                            cbsd.UserId = userId;
                                            cbsd.CreatedBy = userId;
                                            cbsd.CreatedDate = DateTime.Now;
                                            CampusBasedStaffDtlsList.Add(cbsd);
                                        }
                                        criteria.Remove("Section");
                                    }
                                }
                                criteria.Remove("Grade");
                            }
                        }
                    }
                    if (CampusBasedStaffDtlsList.Count > 0)
                    {
                        smsObj.SaveOrUpdateCampusBasedStaffDetailsByList(CampusBasedStaffDtlsList);
                        return Json("success", JsonRequestBehavior.AllowGet);
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
        public ActionResult EditCampusBasedStaffDetails(CampusBasedStaffDetails campusbasedstaffdetails)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("logOut", "Account");
                else
                {
                    var script = "";
                    if (campusbasedstaffdetails == null) return null;
                    if (campusbasedstaffdetails.Id > 0)
                    {
                        QAService QAs = new QAService();
                        Dictionary<string, object> criteria = new Dictionary<string, object>();
                        Dictionary<string, object> likeCriteria = new Dictionary<string, object>();
                        criteria.Add("Grade", campusbasedstaffdetails.Grade);
                        Dictionary<long, IList<CampusBasedStaffDetails>> CampusBasedStaffDetailsList = QAs.GetCampusBasedStaffDetailsListWithPagingAndCriteria(null, null, null, null, criteria, likeCriteria);
                        if (CampusBasedStaffDetailsList.FirstOrDefault().Key == 0)
                        {
                            CampusBasedStaffDetails existObj = new CampusBasedStaffDetails();
                            existObj = smsObj.GetCampusBasedStaffDetailsById(campusbasedstaffdetails.Id);
                            if (existObj != null)
                            {
                                existObj.Grade = campusbasedstaffdetails.Grade;
                                existObj.Section = campusbasedstaffdetails.Section;
                                existObj.Subject = campusbasedstaffdetails.Subject;
                                existObj.AcademicYear = campusbasedstaffdetails.AcademicYear;
                                existObj.ModifiedBy = userId;
                                existObj.ModifiedDate = DateTime.Now;
                                smsObj.SaveOrUpdateCampusBasedStaffDetails(existObj);
                                script = @"SucessMsg(""Updated Successfully"");";
                                return JavaScript(script);
                            }
                            else
                            {
                                script = @"ErrMsg(""This configuration does not exist!"");";
                                return JavaScript(script);
                            }
                        }
                        else
                        {
                            script = @"ErrMsg(""This configuration already exist!"");";
                            return JavaScript(script);
                        }
                    }
                    script = @"InfoMsg(""Updated not Successfully"");";
                    return JavaScript(script);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "MasterPolicy");
                throw ex;
            }
        }
        public ActionResult SaveOrUpdateOnStaffAttendanceReportConfiguration(Staff_AttendanceReportConfiguration_Vw Config, long StaffPreRegNum, long ReportingHeadDesignation)
        {
            try
            {
                string userId = base.ValidateUser();
                var script = "";
                StaffDetailsView StaffDetailsObj = new StaffDetailsView();
                StaffDetailsObj = smsObj.GetStaffDetailsViewByPreRegNum(Convert.ToInt32(StaffPreRegNum));
                if (StaffDetailsObj != null && !string.IsNullOrEmpty(StaffDetailsObj.Campus) && StaffPreRegNum > 0 && Config != null)
                {
                    if (Config.Id > 0)
                    {
                        Staff_AttendanceReportConfigurationByStaffs ExistingConfig = new Staff_AttendanceReportConfigurationByStaffs();
                        ExistingConfig = smsObj.GetStaff_AttendanceReportConfigurationByStaffsByStaffPreRegNumAndReportPreRegNum(Config.StaffPreRegNum, ReportingHeadDesignation);
                        if (ExistingConfig == null)
                        {
                            Staff_AttendanceReportConfigurationByStaffs NewObj = new Staff_AttendanceReportConfigurationByStaffs();
                            NewObj = smsObj.GetStaff_AttendanceReportConfigurationByStaffsById(Config.Staff_AttendanceReportConfig_Id);
                            if (NewObj != null)
                            {
                                NewObj.ReportingHeadPreRegNum = ReportingHeadDesignation;
                                NewObj.ModifiedBy = userId;
                                NewObj.ModifiedDate = DateTime.Now;
                                smsObj.SaveOrUpdateStaff_AttendanceReportConfigurationByStaffs(NewObj);
                                if (NewObj.Staff_AttendanceReportConfig_Id > 0)
                                    script = @"SucessMsg(""Updated Successfully"");";
                            }
                            else
                                script = @"InfoMsg(""This configuration already exist!"");";
                        }
                        else
                            script = @"ErrMsg(""This configuration already exist!"");";
                    }
                    else
                    {
                        Staff_AttendanceReportConfigurationByStaffs AddExistingConfig = new Staff_AttendanceReportConfigurationByStaffs();
                        AddExistingConfig = smsObj.GetStaff_AttendanceReportConfigurationByStaffsByStaffPreRegNumAndReportPreRegNum(Config.StaffPreRegNum, ReportingHeadDesignation);
                        if (AddExistingConfig == null)
                        {
                            Staff_AttendanceReportConfigurationByStaffs NewConfigObj = new Staff_AttendanceReportConfigurationByStaffs();
                            NewConfigObj.Campus = StaffDetailsObj.Campus;
                            NewConfigObj.StaffPreRegNum = Config.StaffPreRegNum;
                            NewConfigObj.ReportingHeadPreRegNum = ReportingHeadDesignation;
                            NewConfigObj.CreatedBy = userId;
                            NewConfigObj.CreatedDate = DateTime.Now;
                            NewConfigObj.ModifiedBy = userId;
                            NewConfigObj.ModifiedDate = DateTime.Now;
                            smsObj.SaveOrUpdateStaff_AttendanceReportConfigurationByStaffs(NewConfigObj);
                            if (NewConfigObj.Staff_AttendanceReportConfig_Id > 0)
                                script = @"SucessMsg(""Added Successfully"");";
                        }
                        else
                            script = @"ErrMsg(""This configuration already exist!"");";
                    }
                }
                return JavaScript(script);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult StaffAttendanceReportConfigurationJqGrid(long StaffPreRegNum, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<string, object> likecriteria = new Dictionary<string, object>();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (StaffPreRegNum > 0)
                    criteria.Add("StaffPreRegNum", StaffPreRegNum);
                Dictionary<long, IList<Staff_AttendanceReportConfiguration_Vw>> Staff_AttendanceReportConfigurationDetails = null;
                Staff_AttendanceReportConfigurationDetails = smsObj.GetStaff_AttendanceReportConfiguration_VwListWithCriteria(page - 1, rows, sidx, sord, criteria);
                if (Staff_AttendanceReportConfigurationDetails != null && Staff_AttendanceReportConfigurationDetails.FirstOrDefault().Key > 0)
                {
                    IList<Staff_AttendanceReportConfiguration_Vw> cbsd = Staff_AttendanceReportConfigurationDetails.FirstOrDefault().Value;
                    long totalRecords = Staff_AttendanceReportConfigurationDetails.FirstOrDefault().Key;
                    int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                    var jsonData = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalRecords,
                        rows =
                        (
                        from items in cbsd
                        select new
                        {
                            i = items.Id,
                            cell = new string[]
                           {
                                items.Id.ToString(),     
                                items.StaffPreRegNum.ToString(),
                                items.Campus,
                                items.Department,
                                items.Designation,
                                items.Staff_AttendanceReportConfig_Id.ToString(),
                                items.ReportingHeadDesignation,
                                items.ReportingHeadName,
                                items.ReportingHeadPreRegNum.ToString(),
                                items.ModifiedBy,
                                items.ModifiedDate!=null?items.ModifiedDate.Value.ToString("dd/MM/yyyy"):"",
                                
                           }
                        }
                        )
                    };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var Empty = new { rows = (new { cell = new string[] { } }) };
                    return Json(Empty, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult DeleteCampusBasedStaffDetails(string[] Id)
        {
            try
            {
                StaffManagementService QAs = new StaffManagementService();
                var bulkId = Id[0].Split(',');
                List<long> bulkIds = new List<long>();
                foreach (var item in bulkId) { bulkIds.Add(Convert.ToInt64(item)); }
                QAs.DeleteCampusBasedStaffDetailsList(bulkIds);
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public JsonResult GetStaffDetailsViewByPreRegNum(Int32 PreRegNum)
        {
            StaffDetailsView StaffDetailsView = new StaffDetailsView();
            if (PreRegNum > 0)
                StaffDetailsView = smsObj.GetStaffDetailsViewByPreRegNum(PreRegNum);
            return Json(StaffDetailsView, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ShowCampusBasedStaffDetails(long StaffPreRegNumber, string Campus)
        {
            try
            {
                ViewBag.StaffPreRegNumber = StaffPreRegNumber;
                ViewBag.Campus = Campus;
                return View();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public JsonResult FillReportingManagersNameAndDesignationByCampus(string campus)
        {
            try
            {
                string userId = ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return Json(null, JsonRequestBehavior.AllowGet);
                else
                {
                    Dictionary<string, Object> criteria = new Dictionary<string, object>();
                    if (!string.IsNullOrEmpty(campus))
                    {
                        criteria.Add("Campus", campus);
                        criteria.Add("Status", "Registered");
                        criteria.Add("IsReportingManager", true);
                        Dictionary<long, IList<TIPS.Entities.StaffManagementEntities.StaffDetailsView>> StaffDetails;
                        StaffDetails = smsObj.GetStaffDetailsViewListWithPaging(null, 99999, string.Empty, string.Empty, criteria);
                        if (StaffDetails != null && StaffDetails.FirstOrDefault().Key > 0)
                        {
                            var StaffDetailsList = (
                            from items in StaffDetails.FirstOrDefault().Value
                            select new
                            {
                                Text = items.Name + "[" + items.Designation + "]",
                                Value = items.PreRegNum
                            }).Distinct().ToList();
                            return Json(StaffDetailsList, JsonRequestBehavior.AllowGet);
                        }
                        else
                            return Json(null, JsonRequestBehavior.AllowGet);
                    }
                    else
                        return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }
        //private void BuildCriteriaForReportingHeadsBasedOnDesignation(string Campus, string ReportingHeads, Dictionary<string, object> criteria)
        //{

        //    Dictionary<string, object> LocalCriteria = new Dictionary<string, object>();
        //    StaffManagementService smsObj = new StaffManagementService();
        //    if (!string.IsNullOrEmpty(Campus))
        //        LocalCriteria.Add("Campus", Campus);
        //    if (!string.IsNullOrEmpty(ReportingHeads))
        //        LocalCriteria.Add("Designation", ReportingHeads);
        //    LocalCriteria.Add("IsReportingManager", true);
        //    LocalCriteria.Add("Status", "Registered");
        //    Dictionary<long, IList<StaffDetailsView>> StaffDetailsViewDetails = smsObj.GetStaffDetailsViewListWithPaging(null, 99999, string.Empty, string.Empty, LocalCriteria);
        //    if (StaffDetailsViewDetails != null && StaffDetailsViewDetails.FirstOrDefault().Key > 0)
        //    {
        //        var ReportingsPreRegNumArray = (from u in StaffDetailsViewDetails.FirstOrDefault().Value
        //                                        select u.PreRegNum).Distinct().ToArray();
        //        IList<Staff_AttendanceReportConfigurationByStaffs> ConfigurationList = new List<Staff_AttendanceReportConfigurationByStaffs>();
        //        ConfigurationList = smsObj.GetStaff_AttendanceReportConfigurationsListBasedOnReportingHeadPreRegNums(ReportingsPreRegNumArray);
        //        if (ConfigurationList != null && ConfigurationList.Count > 0)
        //        {
        //            var StaffPreRegNums = (from u in ConfigurationList
        //                                   select u.StaffPreRegNum).Distinct().ToArray();
        //            criteria.Add("StaffPreRegNumber", StaffPreRegNums);
        //        }
        //    }
        //}
        public JsonResult FillReportingManagersDesignation(string Campus)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                StaffManagementService smsObj = new StaffManagementService();
                if (!string.IsNullOrEmpty(Campus))
                    criteria.Add("Campus", Campus);
                criteria.Add("IsReportingManager", true);
                criteria.Add("Status", "Registered");
                Dictionary<long, IList<StaffDetailsView>> StaffDetailsViewDetails = smsObj.GetStaffDetailsViewListWithPaging(null, 99999, string.Empty, string.Empty, criteria);
                if (StaffDetailsViewDetails != null && StaffDetailsViewDetails.FirstOrDefault().Key > 0)
                {
                    var ReportingManagersDesignationList = (
                             from items in StaffDetailsViewDetails.FirstOrDefault().Value
                             where !string.IsNullOrEmpty(items.Designation)
                             select new
                             {
                                 Text = items.Designation,
                                 Value = items.Designation
                             }).Distinct().ToList();
                    return Json(ReportingManagersDesignationList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }
        #endregion

        #region Added By Prabakaran StaffWiseScoreReport
        public ActionResult StaffWiseScoreReport()
        {
            try
            {
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
                Dictionary<long, IList<AcademicyrMaster>> AcademicyrMaster = ms.GetAcademicyrMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                ViewBag.campusddl = CampusMaster.First().Value;
                ViewBag.acadddl = AcademicyrMaster.First().Value;
                #region BreadCrumb
                string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                #endregion
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult StaffWiseScoreReportDetailsJqGrid(StaffWiseScoreReport_Vw staffwisescorereport, int rows, string sidx, string sord, int? page = 1, long? Expt = 0)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<string, object> likecriteria = new Dictionary<string, object>();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (staffwisescorereport != null)
                {
                    if (!string.IsNullOrEmpty(staffwisescorereport.Campus))
                    { likecriteria.Add("Campus", staffwisescorereport.Campus); }
                    if (!string.IsNullOrEmpty(staffwisescorereport.Grade))
                    { criteria.Add("Grade", staffwisescorereport.Grade); }
                    if (!string.IsNullOrEmpty(staffwisescorereport.Subject))
                    { criteria.Add("Subject", staffwisescorereport.Subject); }
                    if (!string.IsNullOrEmpty(staffwisescorereport.AcademicYear))
                    { criteria.Add("AcademicYear", staffwisescorereport.AcademicYear); }
                    if (!string.IsNullOrEmpty(staffwisescorereport.Month))
                    { criteria.Add("Month", staffwisescorereport.Month); }
                    //if (staffwisescorereport.PreRegNum > 0)
                    //{ criteria.Add("PreRegNum", staffwisescorereport.PreRegNum); }
                    if (!string.IsNullOrEmpty(staffwisescorereport.IdNumber))
                    { criteria.Add("IdNumber", staffwisescorereport.IdNumber); }
                    if (!string.IsNullOrEmpty(staffwisescorereport.Name))
                    { likecriteria.Add("Name", staffwisescorereport.Name); }
                }
                Dictionary<long, IList<StaffWiseScoreReport_Vw>> staffwisescorereportdetails = null;
                staffwisescorereportdetails = smsObj.GetStaffWiseScoreReportListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria, likecriteria);
                if (Expt == 1)
                {
                    int i = 1;
                    foreach (var item1 in staffwisescorereportdetails.FirstOrDefault().Value)
                    {
                        item1.Id = i;
                        i++;
                    }
                    base.ExptToXL(staffwisescorereportdetails.FirstOrDefault().Value, "StaffWiseScoreReport" + "-On-" + DateTime.Today.ToString("dd/MM/yyyy"), (items => new
                    {
                        SNo = items.Id,
                        Staff_Name = items.Name,
                        IdNumber = items.IdNumber,
                        Campus = items.Campus,
                        Grade = items.Grade,
                        AcademicYear = items.AcademicYear,
                        Month = items.Month,
                        Subject = items.Subject,
                        Sec_A = items.A,
                        Sec_B = items.B,
                        Sec_C = items.C,
                        Sec_D = items.D,
                        Sec_E = items.E,
                        Sec_F = items.F,
                    }));
                    return new EmptyResult();
                }
                else
                {
                    if (staffwisescorereportdetails != null && staffwisescorereportdetails.FirstOrDefault().Key > 0)
                    {
                        IList<StaffWiseScoreReport_Vw> cbsd = staffwisescorereportdetails.FirstOrDefault().Value;
                        long totalRecords = staffwisescorereportdetails.FirstOrDefault().Key;
                        int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                        var jsonData = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalRecords,
                            rows =
                            (
                            from items in cbsd
                            select new
                            {
                                i = items.Id,
                                cell = new string[]
                           {
                                items.Id.ToString(),     
                                items.PreRegNum.ToString(),
                                items.Name,
                                items.IdNumber,
                                items.Campus,
                                items.Grade,
                                items.AcademicYear,
                                items.Month,
                                items.Subject,                               
                                "<a style='text-decoration: none; border-bottom: 1px solid black;cursor:pointer;' onclick=\"ShowCategoryWiseMarks('" + items.Campus + "','" + items.Grade + "','" + "A" + "','" + items.AcademicYear + "','" + items.Subject + "','" + items.Month +"','"+items.PreRegNum+"');\" '>"+items.A+"</a>",                                          
                                "<a style='text-decoration: none; border-bottom: 1px solid black;cursor:pointer;' onclick=\"ShowCategoryWiseMarks('" + items.Campus + "','" + items.Grade + "','" + "B" + "','" + items.AcademicYear + "','" + items.Subject + "','" + items.Month +"','"+items.PreRegNum+"');\" '>"+items.B+"</a>",
                                "<a style='text-decoration: none; border-bottom: 1px solid black;cursor:pointer;' onclick=\"ShowCategoryWiseMarks('" + items.Campus + "','" + items.Grade + "','" + "C" + "','" + items.AcademicYear + "','" + items.Subject + "','" + items.Month +"','"+items.PreRegNum+"');\" '>"+items.C+"</a>",
                                "<a style='text-decoration: none; border-bottom: 1px solid black;cursor:pointer;' onclick=\"ShowCategoryWiseMarks('" + items.Campus + "','" + items.Grade + "','" + "D" + "','" + items.AcademicYear + "','" + items.Subject + "','" + items.Month +"','"+items.PreRegNum+"');\" '>"+items.D+"</a>",
                                "<a style='text-decoration: none; border-bottom: 1px solid black;cursor:pointer;' onclick=\"ShowCategoryWiseMarks('" + items.Campus + "','" + items.Grade + "','" + "E" + "','" + items.AcademicYear + "','" + items.Subject + "','" + items.Month +"','"+items.PreRegNum+"');\" '>"+items.E+"</a>",
                                "<a style='text-decoration: none; border-bottom: 1px solid black;cursor:pointer;' onclick=\"ShowCategoryWiseMarks('" + items.Campus + "','" + items.Grade + "','" + "F" + "','" + items.AcademicYear + "','" + items.Subject + "','" + items.Month +"','"+items.PreRegNum+"');\" '>"+items.F+"</a>",
                           }
                            }
                            )
                        };
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
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
        public ActionResult StaffEvaluationCategoryWise(string cam, string gra, string sect, string sub, string acayear, string mon, long PreRegNum)
        {
            try
            {
                StaffEvaluationCategoryWise_Vw secw = new StaffEvaluationCategoryWise_Vw();
                secw.Campus = cam;
                secw.Grade = gra;
                secw.Section = sect;
                secw.Subject = sub;
                secw.AcademicYear = acayear;
                secw.Month = mon;
                secw.StaffPreRegNum = PreRegNum;
                return PartialView(secw);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult StaffEvaluationCategoryWiseJQGrid(string cam, string gra, string sect, string sub, string acayear, string mon, long PreRegNum, int rows, string sidx, string sord, int? page = 1, int? ExptXl = 0)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<string, object> likecriteria = new Dictionary<string, object>();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (!string.IsNullOrEmpty(cam))
                { likecriteria.Add("Campus", cam); }
                if (!string.IsNullOrEmpty(gra))
                { criteria.Add("Grade", gra); }
                if (!string.IsNullOrEmpty(sect))
                { criteria.Add("Section", sect); }
                if (!string.IsNullOrEmpty(sub))
                { criteria.Add("Subject", sub); }
                if (!string.IsNullOrEmpty(acayear))
                { criteria.Add("AcademicYear", acayear); }
                if (!string.IsNullOrEmpty(mon))
                { criteria.Add("Month", mon); }
                if (PreRegNum > 0)
                { criteria.Add("StaffPreRegNum", PreRegNum); }
                Dictionary<long, IList<StaffEvaluationCategoryWise_Vw>> StaffEvaluationCategoryWise = null;
                StaffEvaluationCategoryWise = smsObj.GetSStaffEvaluationCategoryWiseListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria, likecriteria);
                if (ExptXl == 1)
                {
                    int i = 1;
                    foreach (var item1 in StaffEvaluationCategoryWise.FirstOrDefault().Value)
                    {
                        item1.Id = i;
                        i++;
                    }
                    base.ExptToXL(StaffEvaluationCategoryWise.FirstOrDefault().Value, "StaffEvaluationCategoryWiseScoreReport" + "-On-" + DateTime.Today.ToString("dd/MM/yyyy"), (items => new
                    {
                        SNo = items.Id,
                        Campus = items.Campus,
                        Grade = items.Grade,
                        Section = items.Section,
                        AcademicYear = items.AcademicYear,
                        Month = items.Month,
                        Subject = items.Subject,
                        Staff_Name = items.StaffName,
                        Category_Name = items.CategoryName,
                        Score = items.AvgScore
                    }));
                    return new EmptyResult();
                }
                else
                {
                    if (StaffEvaluationCategoryWise != null && StaffEvaluationCategoryWise.FirstOrDefault().Key > 0)
                    {
                        IList<StaffEvaluationCategoryWise_Vw> secwsr = StaffEvaluationCategoryWise.FirstOrDefault().Value;
                        long totalRecords = StaffEvaluationCategoryWise.FirstOrDefault().Key;
                        int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                        var jsonData = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalRecords,
                            rows =
                            (
                            from items in secwsr
                            select new
                            {
                                i = items.Id,
                                cell = new string[]
                           {
                                items.Id.ToString(),                                                                     
                                items.Campus,
                                items.Grade,
                                items.Section,
                                items.AcademicYear,
                                items.Month,
                                items.Subject,
                                items.StaffName,     
                                items.CategoryName,
                                items.AvgScore.ToString()
                           }
                            }
                            )
                        };
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var Empty = new { rows = (new { cell = new string[] { } }) };
                        return Json(Empty, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        public ActionResult StaffDetailsPopupJqgrid(int rows, string sidx, string sord, int? page = 1)
        {

            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<long, IList<StaffDetailsView>> staffdetailsList = smsObj.GetStaffDetailsViewListWithPaging(0, 9999, "Asc", "Name", criteria);
                if (staffdetailsList != null && staffdetailsList.Count > 0 && staffdetailsList.FirstOrDefault().Key > 0)
                {
                    long totalRecords = staffdetailsList.FirstOrDefault().Key;
                    int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                    var jsonData = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalRecords,
                        rows = (
                        from items in staffdetailsList.FirstOrDefault().Value
                        select new
                        {
                            i = items.Id,
                            cell = new string[] 
                       { 
                           items.Id.ToString(),items.PreRegNum.ToString(),items.Name,items.Campus,items.Department
                    }
                        })
                    };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult StaffDetailsPopup()
        {
            return PartialView();
        }
        #region PrintIdCard for Staff
        public ActionResult PrintStaffIdCard()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                #region BreadCrumb
                string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                #endregion
                return View();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult StaffandDriverListJqgrid(string Type, string Campus, string Name, string IdNumber, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                sord = sord == "desc" ? "Desc" : "Asc";
                if (!string.IsNullOrEmpty(Type))
                {
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    if (Type == "Staff")
                    {
                        criteria.Add("Status", "Registered");
                        criteria.Add("WorkingType", "Staff");
                        if (!string.IsNullOrEmpty(Campus))
                        {
                            criteria.Add("Campus", Campus);
                        }
                        if (!string.IsNullOrEmpty(Name))
                        {
                            criteria.Add("Name", Name);
                        }
                        if (!string.IsNullOrEmpty(IdNumber))
                        {
                            criteria.Add("IdNumber", IdNumber);
                        }
                        Dictionary<long, IList<StaffDetailsView>> staffdetailsList = smsObj.GetStaffDetailsViewListWithPaging(page - 1, rows, sord, sidx, criteria);
                        if (staffdetailsList != null && staffdetailsList.Count > 0 && staffdetailsList.FirstOrDefault().Key > 0)
                        {
                            long totalRecords = staffdetailsList.FirstOrDefault().Key;
                            int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                            var jsonData = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalRecords,
                                rows = (
                                from items in staffdetailsList.FirstOrDefault().Value
                                select new
                                {
                                    i = items.Id,
                                    cell = new string[] 
                       { 
                           items.Id.ToString(),items.PreRegNum.ToString(),items.Name,items.IdNumber,items.Campus,items.DOB,items.Gender,items.PhoneNo,items.Status
                    }
                                })
                            };
                            return Json(jsonData, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            var Empty = new { rows = (new { cell = new string[] { } }) };
                            return Json(Empty, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else if (Type == "Driver")
                    {

                        criteria.Clear();
                        criteria.Add("Status", "Registered");
                        if (!string.IsNullOrEmpty(Campus))
                        {
                            criteria.Add("Campus", Campus);
                        }
                        if (!string.IsNullOrEmpty(Name))
                        {
                            criteria.Add("Name", Name);
                        }
                        if (!string.IsNullOrEmpty(IdNumber))
                        {
                            criteria.Add("DriverIdNo", IdNumber);
                        }
                        TransportService ts = new TransportService();
                        Dictionary<long, IList<DriverMaster>> DriverdetailsList = ts.GetDriverListWithEQsearchCriteria(page - 1, rows, sidx, sord, criteria);
                        if (DriverdetailsList != null && DriverdetailsList.Count > 0 && DriverdetailsList.FirstOrDefault().Key > 0)
                        {
                            long totalRecords = DriverdetailsList.FirstOrDefault().Key;
                            int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                            var jsonData = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalRecords,
                                rows = (
                                from items in DriverdetailsList.FirstOrDefault().Value
                                select new
                                {
                                    i = items.Id,
                                    cell = new string[] 
                       { 
                           items.Id.ToString(),items.DriverRegNo.ToString(),items.Name,items.DriverIdNo,items.Campus,items.Dob,items.Gender,items.PhoneNo,items.Status
                    }
                                })
                            };
                            return Json(jsonData, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            var Empty = new { rows = (new { cell = new string[] { } }) };
                            return Json(Empty, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        var Empty = new { rows = (new { cell = new string[] { } }) };
                        return Json(Empty, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var Empty = new { rows = (new { cell = new string[] { } }) };
                    return Json(Empty, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        #region staff profile
        public ActionResult StaffProfile()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    FillViewBag();
                    Session["status"] = "";
                    StaffDetails StaffDetails = new StaffDetails();
                    StaffManagementService sms = new StaffManagementService();
                    TIPS.Entities.User sessionUser = (TIPS.Entities.User)Session["objUser"];
                    if (sessionUser.UserType.ToUpper() == "STAFF")
                    {
                        // ViewBag.disableInput = true;
                        StaffDetails = sms.GetStaffDetailsByIdNumber(sessionUser.EmployeeId);
                    }
                    else
                    {
                        return RedirectToAction("StaffDisplay", "StaffManagement");
                    }

                    //   StaffDetails = sms.GetStaffDeatailsByPreRegNum(Convert.ToInt32(id));///by preregnum
                    if (StaffDetails.DateOfJoin != null && StaffDetails.DOB != null)
                    {
                        ViewBag.dateofjoin = DateTime.Parse(StaffDetails.DateOfJoin).ToString("dd/MM/yyyy"); // for show date only instead of datetime
                        ViewBag.DOB = DateTime.Parse(StaffDetails.DOB).ToString("dd/MM/yyyy");
                    }

                    StaffQualification staffqual = new StaffQualification();

                    {
                        IList<StaffQualification> staffqualificationList = new List<StaffQualification>();
                        staffqualificationList.Add(staffqual);
                        StaffDetails.StaffQualificationList = staffqualificationList;
                    }

                    StaffExperience staffexp = new StaffExperience();
                    {
                        IList<StaffExperience> staffexpList = new List<StaffExperience>();
                        staffexpList.Add(staffexp);
                        StaffDetails.StaffExperienceList = staffexpList;
                    }

                    StaffTraining stafftraining = new StaffTraining();
                    {
                        IList<StaffTraining> stafftrainingList = new List<StaffTraining>();
                        stafftrainingList.Add(stafftraining);
                        StaffDetails.StaffTrainingList = stafftrainingList;
                    }

                    UploadedFilesView uploadedfile = new UploadedFilesView();

                    //      if (StaffDetails.UploadedFilesList.Count == 0)
                    {
                        IList<UploadedFilesView> uploadedfileviewList = new List<UploadedFilesView>();
                        uploadedfileviewList.Add(uploadedfile);
                        StaffDetails.UploadedFilesList = uploadedfileviewList;
                    }
                    ViewBag.RequestNum = StaffDetails.PreRegNum;
                    if (StaffDetails.CreatedDate != null)
                    {
                        ViewBag.Date = DateTime.Parse(StaffDetails.CreatedDate).ToString("dd/MM/yyyy");// DateTime.Now.ToShortDateString();
                    }
                    else
                    {
                        ViewBag.Date = StaffDetails.CreatedDate;
                    }
                    ViewBag.Time = StaffDetails.CreatedTime;// DateTime.Now.ToShortTimeString();
                    Session["Reqnum"] = StaffDetails.PreRegNum;
                    Session["status"] = StaffDetails.Status;
                    ViewBag.admissionstatus = StaffDetails.Status;
                    ViewBag.nam = StaffDetails.Name;
                    //+ " " + StaffDetails.Initial;
                    ViewBag.idnum = StaffDetails.IdNumber;
                    ViewBag.desig = StaffDetails.Designation;
                    ViewBag.campus = StaffDetails.Campus;
                    ViewBag.ForPage = "StaffProfile";
                    if (StaffDetails.DocCheck != null)
                        ViewBag.ToDocCheck = StaffDetails.DocCheck;
                    //if (StaffDetails.FamilyCheck != null)
                    //  ViewBag.ToFamilyCheck = StaffDetails.FamilyCheck;
                    //if (StaffDetails.ExpCheck != null)
                    //  ViewBag.ToExpCheck = StaffDetails.ExpCheck;
                    if (StaffDetails.QualCheck != null)
                        ViewBag.ToQualCheck = StaffDetails.QualCheck;
                    //if (StaffDetails.TraCheck != null)
                    //  ViewBag.ToTraCheck = StaffDetails.TraCheck;

                    //
                    if (StaffDetails.DateOfJoin != null)
                    {
                        ViewBag.doj = DateTime.Parse(StaffDetails.DateOfJoin).ToString("dd/MM/yyyy");
                    }
                    ViewBag.dept = StaffDetails.Department;
                    ViewBag.processby = base.ValidateUser();

                    long counVar = sms.ExecutePercentageQueryFromStaffDetailsUsingQuery(StaffDetails.PreRegNum);
                    //if (counVar >= 0 && counVar <= 20) { ViewData["perVar"] = 90; }
                    //if (counVar >= 21 && counVar <= 40) { ViewData["perVar"] = 80; }
                    //if (counVar >= 41 && counVar <= 60) { ViewData["perVar"] = 60; }
                    //if (counVar >= 61 && counVar <= 80) { ViewData["perVar"] = 40; }
                    ViewData["perVar"] = Convert.ToInt64(((47 - counVar) / 47.00) * 100.00);
                    if (StaffDetails.DateOfJoin != null)
                    {
                        StaffDetails.DateOfJoin = DateTime.Parse(StaffDetails.DateOfJoin).ToString("dd/MM/yyyy");
                    }
                    if (StaffDetails.DOB != null)
                    {
                        StaffDetails.DOB = DateTime.Parse(StaffDetails.DOB).ToString("dd/MM/yyyy");
                    }
                    //if (StaffDetails.ResignationDate != null)
                    //{
                    //    StaffDetails.ResignationDate = DateTime.Parse(StaffDetails.ResignationDate).ToString("dd/MM/yyyy");
                    //}
                    //if (StaffDetails.RelievingDate != null)
                    //{
                    //    StaffDetails.RelievingDate = DateTime.Parse(StaffDetails.RelievingDate).ToString("dd/MM/yyyy");
                    //}
                    //if (StaffDetails.LastWorkingDate != null)
                    //{
                    //    StaffDetails.LastWorkingDate = DateTime.Parse(StaffDetails.LastWorkingDate).ToString("dd/MM/yyyy");
                    //}

                    return View(StaffDetails);

                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        #endregion
        #region StaffGroupMaster
        public ActionResult StaffGroupMaster()
        {
            try
            {
                #region BreadCrumb
                string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                #endregion
                return View();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult StaffGroupMasterJqGrid(string Campus, string GroupName, string IsActive, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (!string.IsNullOrEmpty(Campus))
                { criteria.Add("Campus", Campus); }
                if (!string.IsNullOrEmpty(GroupName))
                { criteria.Add("GroupName", GroupName); }
                if (!string.IsNullOrEmpty(IsActive))
                {
                    if (IsActive == "true" || IsActive == "True")
                        criteria.Add("IsActive", true);
                    if (IsActive == "false" || IsActive == "False")
                        criteria.Add("IsActive", false);
                }
                Dictionary<long, IList<StaffGroupMaster>> StaffGroupMasterList = smsObj.GetStaffGroupMasterListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (StaffGroupMasterList != null && StaffGroupMasterList.Count > 0)
                {
                    long totalrecords = StaffGroupMasterList.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,
                        rows = (from items in StaffGroupMasterList.First().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[]{
                                       items.StaffGroupId.ToString(),                                                                                                                     
                                       items.Campus,                                       
                                       items.GroupName,
                                       items.IsActive==true?"Yes":"No",
                                       items.CreatedBy,
                                       items.CreatedDate.ToString(),
                                       items.ModifiedBy,
                                       items.ModifiedDate.ToString()
                                       }
                                })

                    };
                    return Json(jsondat, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var Empty = new { rows = (new { cell = new string[] { } }) };
                    return Json(Empty, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult AddStaffGroupMaster(StaffGroupMaster sbg)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    StaffGroupMaster sgmdetails = smsObj.GetStaffGroupMasterByCampusandGroup(sbg.Campus, sbg.GroupName);
                    if (sgmdetails != null)
                    {
                        var script = @"ErrMsg(""Already Exist"");";
                        return JavaScript(script);
                    }
                    sbg.GroupName = sbg.GroupName;
                    sbg.Campus = sbg.Campus;
                    sbg.CreatedBy = userId;
                    sbg.CreatedDate = DateTime.Now;
                    sbg.IsActive = sbg.IsActive;
                    smsObj.CreateOrUpdateStaffGroupMaster(sbg);
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult EditStaffGroupMaster(StaffGroupMaster sgm)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (sgm.StaffGroupId > 0)
                    {
                        StaffGroupMaster sgmdtls = smsObj.GetStaffGroupMasterByCampusandGroup(sgm.Campus, sgm.GroupName);
                        if (sgmdtls != null)
                        {
                            var script = @"ErrMsg(""Already Exist"");";
                            return JavaScript(script);
                        }
                        StaffGroupMaster sgmdetails = smsObj.GetStaffGroupMasterByStaffGroupId(sgm.StaffGroupId);
                        if (sgmdetails != null)
                        {
                            sgmdetails.ModifiedBy = userId;
                            sgmdetails.ModifiedDate = DateTime.Now;
                            sgmdetails.Campus = sgm.Campus;
                            sgmdetails.GroupName = sgm.GroupName;
                            sgmdetails.IsActive = sgm.IsActive;
                            smsObj.CreateOrUpdateStaffGroupMaster(sgmdetails);
                        }
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult DeleteStaffGroupMaster(string Id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    string[] arrayId = Id.Split(',');
                    long[] StaffGroupId = Array.ConvertAll(arrayId, Int64.Parse);
                    smsObj.DeleteStaffGroupMaster(StaffGroupId);
                    var script = @"SucessMsg(""Deleted  Successfully"");";
                    return JavaScript(script);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult SendStaffLoginDetailsByEmail(string id)
        {
            try
            {
                string Status = string.Empty;
                StaffManagementService sms = new StaffManagementService();
                string passwrd = string.Empty;
                string RecipientInfo = string.Empty, Subject = string.Empty, Body = string.Empty, MailBody = string.Empty; bool retValue;
                string staffName = string.Empty; string campus = string.Empty;
                string IdNumber = string.Empty;
                //Dictionary<long, IList<StaffDetails>> staffdetails;
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                //criteria.Add("PreRegNum", Convert.ToInt32(Session["Reqnum"]));
                //if (PreRegNum > 0)
                //{
                //    criteria.Add("PreRegNum", PreRegNum);
                //}
                //else
                //{
                //    return Json("failed", JsonRequestBehavior.AllowGet);
                //}
                StaffDetails staffdetails = new StaffDetails();
                staffdetails = sms.GetStaffDetailsId(Convert.ToInt32(id));
                //staffdetails = sms.GetStaffDetailsListWithPaging(0, 10000, string.Empty, string.Empty, criteria);
                if (staffdetails != null)
                {
                    //dateOfBirth = staffdetails.DOB;                    
                    staffName = staffdetails.Name;
                    campus = staffdetails.Campus;
                    IdNumber = staffdetails.IdNumber != null ? staffdetails.IdNumber : staffdetails.TempIdNumber;
                    if (string.IsNullOrEmpty(staffdetails.EmailId))
                    {
                        Status = "Email";
                        return Json(Status, JsonRequestBehavior.AllowGet);
                    }
                }
                UserService us = new UserService();
                User userdetails = us.GetUserByUserId(IdNumber);
                if (userdetails == null)
                {
                    //if (!string.IsNullOrEmpty(dateOfBirth))
                    //{
                    //    dateOfBirth = dateOfBirth.Replace(@"/", string.Empty).Trim();
                    //}
                    User usrObj = new User();
                    usrObj.CreatedDate = DateTime.Now;
                    usrObj.ModifiedDate = DateTime.Now;
                    PassworAuth PA = new PassworAuth();
                    passwrd = GenerateRandomString(8);
                    //encode and save the password
                    usrObj.UserName = staffName;
                    usrObj.UserId = IdNumber;
                    usrObj.EmailId = staffdetails.EmailId;
                    usrObj.Campus = campus;
                    usrObj.Password = PA.base64Encode(passwrd);
                    usrObj.IsActive = true;
                    usrObj.UserType = "Staff";
                    usrObj.CreatedBy = Session["UserId"] != null ? Session["UserId"].ToString() : "";
                    us.CreateOrUpdateUser(usrObj);
                    Status = "user";
                }
                else
                {
                    PassworAuth PA = new PassworAuth();
                    passwrd = PA.base64Decode2(userdetails.Password);
                }
                //IList<CampusEmailId> campusemaildet = GetEmailIdByCampus(campus, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
                if (ConfigurationManager.AppSettings["SendEmail1"].ToString() == "true")
                {

                    //RecipientInfo = "Dear Sir/Madam,";
                    //Subject = "Login Details";
                    //Body = "Welcome to the TIPS Family!<br /><br/> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;we would like to congratulate you to join with us.<br/><br/>Your UserId is “" + IdNumber + "” and password will be “" + passwrd + "” i.e. the DOB of your's as given in the records. You can access the same through 172.16.17.252:8081 .<br/><br/> For any queries, mail us at “" + campusemaildet.First().EmailId.ToString() + "”. ";
                    //retValue = emailObj.SendStudentRegistrationMail(null, staffdetails.EmailId, campus, Subject, "", Body, RecipientInfo, "Parent", null);                    
                    System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                    string CCMail = string.Empty;
                    CCMail = ConfigurationManager.AppSettings["NewStaffCCMail"].ToString();
                    string body = "<b>Dear " + staffName + "<b><br/><br/>";
                    body = body + "<b>Welcome to ICMS...! </b><br/><br/>";
                    body = body + "<b>URL :</b> <a href='http://myaccess.tipsglobal.net/'>myaccess.tipsglobal.net</a><br/><br/>";
                    body = body + "<table style='border-collapse:collapse; border:1px solid black; padding: 5px;'> ";
                    body = body + "<tr> <td style='font-weight:bold; border:1px solid black; padding: 5px;'><b> User Name </b></td> <td style='border:1px solid black; padding: 5px;'>" + IdNumber + "  </td> <tr> ";
                    body = body + "<tr> <td style='font-weight:bold; border:1px solid black; padding: 5px;'><b>Password</b></td> <td style='border:1px solid black; padding: 5px;'> " + passwrd + " </td><tr> ";
                    body = body + "</table><br/><br/>Use the above Username and Password in Staff Portal.<br/> <br/>";
                    body = body + "<b>Note:</b><br/>You are Requested to Update your Profile in Staff Portal i.e., EMail,D.O.B,Experience Details,Photo..Etc<br/> <br/>";
                    mail.To.Add(staffdetails.EmailId);
                    mail.CC.Add(CCMail);
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    mail.Subject = "ICMS-Login credentials ";
                    EmailHelper emailObj = new EmailHelper();
                    retValue = emailObj.SendEmailWithEmailTemplate(mail, campus, GetGeneralBodyofMail());
                    if (retValue == true)
                    {
                        Status += ",success";
                    }
                    else
                    {
                        Status += ",failed";
                    }

                }
                return Json(Status, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool SendMailtoExistingStaff(string IdNumber, string Password, string Name, string campus, string emailid)
        {
            bool retValue = false;
            if (ConfigurationManager.AppSettings["SendEmail1"].ToString() == "true")
            {
                string pswd = string.Empty;
                //RecipientInfo = "Dear Sir/Madam,";
                //Subject = "Login Details";
                //Body = "Welcome to the TIPS Family!<br /><br/> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;we would like to congratulate you to join with us.<br/><br/>Your UserId is “" + IdNumber + "” and password will be “" + passwrd + "” i.e. the DOB of your's as given in the records. You can access the same through 172.16.17.252:8081 .<br/><br/> For any queries, mail us at “" + campusemaildet.First().EmailId.ToString() + "”. ";
                //retValue = emailObj.SendStudentRegistrationMail(null, staffdetails.EmailId, campus, Subject, "", Body, RecipientInfo, "Parent", null);                    
                PassworAuth PA = new PassworAuth();
                pswd = PA.base64Decode2(Password);
                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                string CCMail = string.Empty;
                CCMail = ConfigurationManager.AppSettings["NewStaffCCMail"].ToString();
                string body = "<b>Dear " + Name + "<b><br/><br/>";
                body = body + "<table style='border-collapse:collapse; border:1px solid black; padding: 5px;'> ";
                body = body + "<tr> <td style='font-weight:bold; border:1px solid black; padding: 5px;'><b> User Name </b></td> <td style='border:1px solid black; padding: 5px;'>" + IdNumber + "  </td> <tr> ";
                body = body + "<tr> <td style='font-weight:bold; border:1px solid black; padding: 5px;'><b>Password</b></td> <td style='border:1px solid black; padding: 5px;'> " + pswd + " </td><tr> ";
                body = body + "</table><br/><br/>Here After,Please use the above Username and Password in Staff Portal.<br/> <br/>";
                body = body + "<b>Note:</b><br/>You are Requested to Update your Profile in Staff Portal i.e., EMail,D.O.B,Experience Details,Photo..Etc<br/> <br/>";
                mail.To.Add(emailid);
                mail.CC.Add(CCMail);
                mail.Body = body;
                mail.IsBodyHtml = true;
                mail.Subject = "ICMS-Login credentials ";
                EmailHelper emailObj = new EmailHelper();
                retValue = emailObj.SendEmailWithEmailTemplate(mail, campus, GetGeneralBodyofMail());
            }
            return retValue;
        }
        #endregion
        public ActionResult checkIdNumber(string IdNumber)
        {
            StaffDetails sdetails = smsObj.GetStaffDetailsByIdNumber(IdNumber.Replace(" ", "").Trim());
            if (sdetails != null)
            {
                return Json("failed", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("success", JsonRequestBehavior.AllowGet);
            }
        }
        #region Staff Attendance
        public ActionResult StaffAttendance()
        {
            try
            {

                #region BreadCrumb
                string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                #endregion
                return View();
            }
            catch (Exception)
            {

                throw;
            }

        }
        public JsonResult GetStaffDetailsByPreRegNum(Int32 PreRegNum)
        {
            StaffDetails sdetails = smsObj.GetStaffDeatailsByPreRegNum(PreRegNum);
            StaffAttendance sattendance = smsObj.GetStaffAttendanceByAttendanceDatewithPreRegNum(PreRegNum);
            if (sdetails != null)
            {
                var checkin = string.Empty;
                if (sattendance == null)
                {
                    checkin = "true";
                }
                else if (sattendance != null && sattendance.LogOut == null)
                {
                    checkin = "false";
                }
                else if (sattendance != null && sattendance.LogOut != null)
                {
                    checkin = "completed";
                }
                var jsondata = new
                {
                    Name = sdetails.Name,
                    Designation = sdetails.Designation,
                    Campus = sdetails.Campus,
                    IdNumber = sdetails.IdNumber,
                    Department = sdetails.Department,
                    checkinval = checkin
                };
                return Json(jsondata, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SaveStaffAttendanceDetails(StaffAttendance sa)
        {
            try
            {
                //string userId = ValidateUser();
                //if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                //else
                //{
                if (sa != null)
                {
                    sa.AttendanceDate = DateTime.Now.Date;
                    sa.LogIn = DateTime.Now;
                    //string MACAddress = GetUser_IP();
                    //if (!string.IsNullOrEmpty(MACAddress))
                    //{
                    //    sa.LogInIPAddress = MACAddress;
                    //}
                    //else
                    //{
                    //    return RedirectToAction("StaffAttendance", "StaffManagement");
                    //}
                    sa.CreatedDate = DateTime.Now;
                    sa.CreatedBy = "";
                    smsObj.CreateOrUpdateStaffAttendance(sa);
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
                return Json("failed", JsonRequestBehavior.AllowGet);
                //}
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult EditStaffAttendanceDetails(StaffAttendance sa)
        {
            try
            {
                //string userId = ValidateUser();
                //if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                //else
                //{
                if (sa != null)
                {
                    StaffAttendance sattendance = smsObj.GetStaffAttendanceByAttendanceDatewithPreRegNum(sa.PreRegNum);
                    if (sattendance != null)
                    {
                        sattendance.LogOut = DateTime.Now;
                        string MACAddress = GetUser_IP();
                        if (!string.IsNullOrEmpty(MACAddress))
                        {
                            sattendance.LogOutIPAddress = MACAddress;
                        }
                        else
                        {
                            return RedirectToAction("StaffAttendance", "StaffManagement");
                        }
                        sattendance.ModifiedBy = "";
                        sattendance.ModifiedDate = DateTime.Now;
                        smsObj.CreateOrUpdateStaffAttendance(sattendance);
                        return Json("success", JsonRequestBehavior.AllowGet);
                    }
                    return Json("failed", JsonRequestBehavior.AllowGet);
                }
                return Json("failed", JsonRequestBehavior.AllowGet);
                //}
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        #region Staff Attendance Reports
        public ActionResult StaffAttendanceReport()
        {
            try
            {
                string userId = ValidateUser();
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
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult StaffAttendance_vwJqGrid(string Campus, string IdNumber, string Name, string Department, string Designation, string AttendanceDate, int rows, string sidx, string sord, int? page = 1, long? ExptXl = 0)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (!string.IsNullOrEmpty(Campus))
                    criteria.Add("Campus", Campus);
                if (!string.IsNullOrEmpty(IdNumber))
                    criteria.Add("IdNumber", IdNumber);
                if (!string.IsNullOrEmpty(Name))
                    criteria.Add("Name", Name);
                if (!string.IsNullOrEmpty(Department))
                    criteria.Add("Department", Department);
                if (!string.IsNullOrEmpty(Designation))
                    criteria.Add("Designation", Designation);
                if (!string.IsNullOrEmpty(AttendanceDate))
                    criteria.Add("AttendanceDate", DateTime.Parse(AttendanceDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault));
                Dictionary<long, IList<StaffAttendance_vw>> StaffAttendance_vwList = smsObj.GetStaffAttendanceListWithPaging(page - 1, rows, sidx, sord, criteria);
                if (ExptXl == 1)
                {
                    base.ExptToXL(StaffAttendance_vwList.FirstOrDefault().Value, "StaffAttendanceReport" + "-On-" + DateTime.Today.ToString("dd/MM/yyyy"), (items => new
                        {
                            items.Campus,
                            items.Name,
                            items.IdNumber,
                            items.Department,
                            items.Designation,
                            AttendanceDate = items.AttendanceDate != null ? items.AttendanceDate.Value.ToString("dd'/'MM'/'yyyy") : "",
                            Check_In = items.LogIn != null ? items.LogIn.Value.ToString("dd'/'MM'/'yyyy HH:mm:ss") : "",
                            Check_Out = items.LogOut != null ? items.LogOut.Value.ToString("dd'/'MM'/'yyyy HH:mm:ss") : "",
                        }));
                    return new EmptyResult();
                }
                else if (StaffAttendance_vwList != null && StaffAttendance_vwList.Count > 0)
                {
                    long totalrecords = StaffAttendance_vwList.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,
                        rows = (from items in StaffAttendance_vwList.First().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[]{
                                       items.Id.ToString(),                                                                                                                     
                                       items.Campus,                                       
                                       items.Name,
                                       items.IdNumber,
                                       items.Department,
                                       items.Designation,
                                       items.AttendanceDate!=null?items.AttendanceDate.Value.ToString("dd'/'MM'/'yyyy"):"",
                                       items.LogIn!=null?items.LogIn.Value.ToString("dd'/'MM'/'yyyy HH:mm:ss"):"",
                                       items.LogOut!=null?items.LogOut.Value.ToString("dd'/'MM'/'yyyy HH:mm:ss"):"",
                                       items.CreatedBy,
                                       items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd'/'MM'/'yyyy"):"",
                                       items.ModifiedBy,
                                       items.ModifiedDate!=null?items.ModifiedDate.Value.ToString("dd'/'MM'/'yyyy"):""
                                       }
                                })

                    };
                    return Json(jsondat, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var Empty = new { rows = (new { cell = new string[] { } }) };
                    return Json(Empty, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        public ActionResult StaffIdNumberAutoComplete(string term, string Campus)
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(Campus))
                { criteria.Add("Campus", Campus); }
                if (!string.IsNullOrEmpty(term))
                { criteria.Add("IdNumber", term); }
                criteria.Add("Status", "Registered");
                Dictionary<long, IList<StaffDetailsView>> StaffDetails = sms.GetStaffDetailsViewListWithPaging(0, 9999, "IdNumber", string.Empty, criteria);
                if (StaffDetails.Count > 0 && StaffDetails.FirstOrDefault().Key > 0)
                {
                    var StaffIdNumber = (from u in StaffDetails.FirstOrDefault().Value where u.IdNumber != null && u.PreRegNum > 0 select new { IdNumber = u.IdNumber, PreRegNum = u.PreRegNum }).ToList();
                    return Json(StaffIdNumber, JsonRequestBehavior.AllowGet);
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        //public static string GetLocalIPAddress()
        //{
        //    var host = Dns.GetHostEntry(Dns.GetHostName());
        //    var IPAddress = string.Empty;
        //    foreach (var ip in host.AddressList)
        //    {
        //        if (ip.AddressFamily == AddressFamily.InterNetwork)
        //        {
        //            IPAddress = ip.ToString();
        //            break;
        //        }
        //    }
        //    return IPAddress;
        //}
        //protected string GetLocalIPAddress()
        //{
        //    System.Web.HttpContext context = System.Web.HttpContext.Current;
        //    string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

        //    if (!string.IsNullOrEmpty(ipAddress))
        //    {
        //        string[] addresses = ipAddress.Split(',');
        //        if (addresses.Length != 0)
        //        {
        //            return addresses[0];
        //        }
        //    }
        //    return context.Request.ServerVariables["REMOTE_ADDR"];
        //}


        //public string GetLocalIPAddress()
        //{
        //    string IPAddress = string.Empty;
        //    IPHostEntry Host = default(IPHostEntry);
        //    string Hostname = null;
        //    Hostname = System.Environment.MachineName;
        //    Host = Dns.GetHostEntry(Hostname);
        //    foreach (IPAddress IP in Host.AddressList)
        //    {
        //        if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
        //        {
        //            IPAddress = Convert.ToString(IP);
        //        }
        //    }
        //    return IPAddress;

        //}

        protected string GetUser_IP()
        {
            string VisitorsIPAddr = string.Empty;
            if (HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                VisitorsIPAddr = HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else if (HttpContext.Request.UserHostAddress.Length != 0)
            {
                VisitorsIPAddr = HttpContext.Request.UserHostAddress;
            }
            return VisitorsIPAddr;
        }
        //public ActionResult StaffRegistration()
        //{
        //    MastersService ms = new MastersService();
        //    Dictionary<string, object> criteria = new Dictionary<string, object>();
        //    Dictionary<long, IList<BloodGroupMaster>> BloodGroupMaster = ms.GetBloodGroupMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
        //    Dictionary<long, IList<NationalityMaster>> NationMaster = ms.GetNationalityDetails(0, 9999, string.Empty, string.Empty, criteria);
        //    Dictionary<long, IList<RelationshipMaster>> RelationshipMaster = ms.GetRelationshipMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);

        //    var bloodgrup = (
        //             from items in BloodGroupMaster.First().Value
        //             select new
        //             {
        //                 Text = items.BloodGroup.ToUpper(),
        //                 Value = items.BloodGroup.ToUpper()
        //             }).ToList();
        //    ViewBag.bloodgrpddl = bloodgrup;
        //    var nation = (
        //            from items in NationMaster.First().Value
        //            select new
        //            {
        //                Text = items.Nationality.ToUpper(),
        //                Value = items.Nationality.ToUpper()
        //            }).ToList();
        //    ViewBag.Nationality = nation;
        //    var Relation = (
        //           from items in RelationshipMaster.First().Value
        //           select new
        //           {
        //               Text = items.Relationships.ToUpper(),
        //               Value = items.Relationships.ToUpper()
        //           }).ToList();
        //    ViewBag.RelationType = Relation;
        //    return View();
        //}
        public ActionResult StaffRegistration()
        {
            MastersService ms = new MastersService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            Dictionary<long, IList<BloodGroupMaster>> BloodGroupMaster = ms.GetBloodGroupMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            Dictionary<long, IList<NationalityMaster>> NationMaster = ms.GetNationalityDetails(0, 9999, string.Empty, string.Empty, criteria);
            Dictionary<long, IList<RelationshipMaster>> RelationshipMaster = ms.GetRelationshipMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            Dictionary<long, IList<CampusMaster>> CampusMaster = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);

            var bloodgrup = (
                     from items in BloodGroupMaster.First().Value
                     select new
                     {
                         Text = items.BloodGroup.ToUpper(),
                         Value = items.BloodGroup.ToUpper()
                     }).ToList();
            ViewBag.bloodgrpddl = bloodgrup;
            var nation = (
                    from items in NationMaster.First().Value
                    select new
                    {
                        Text = items.Nationality.ToUpper(),
                        Value = items.Nationality.ToUpper()
                    }).ToList();
            ViewBag.Nationality = nation;
            var Relation = (
                   from items in RelationshipMaster.First().Value
                   select new
                   {
                       Text = items.Relationships.ToUpper(),
                       Value = items.Relationships.ToUpper()
                   }).ToList();
            ViewBag.RelationType = Relation;

            var CampusDtls = (
                   from items in CampusMaster.First().Value
                   select new
                   {
                       Text = items.Name.ToUpper(),
                       Value = items.Name.ToUpper()
                   }).ToList();
            ViewBag.CampusDtls = CampusDtls;
            return View();
        }
        public ActionResult GetDepartment(string Stafftype)
        {
            try
            {
                MastersService ms = new MastersService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("StaffType", Stafftype);
                Dictionary<long, IList<NewDepartmentMaster>> DepartmentMaster = ms.GetNewDepartmentMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);

                var Dept = new
                {
                    rows = (
                         from items in DepartmentMaster.First().Value
                         select new
                         {
                             Text = items.DesignationName,
                             Value = items.DesignationName
                         }).ToArray(),
                };

                return Json(Dept, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {

                throw;
            }
        }
        # region Added by dhanabalan
        [HttpPost]
        public ActionResult UploadDocuments(HttpPostedFileBase uploadedFile, string docType, string documentFor, long staffId)
        {

            HttpPostedFileBase theFile = HttpContext.Request.Files["uploadedFile"];
            if (theFile.ContentLength != 0)
            {
                StaffManagementService sms = new StaffManagementService();
                StaffDetailsView sdv = new StaffDetailsView();
                sdv = sms.GetStaffDetailsViewById(staffId);
                AdmissionManagementService ads = new AdmissionManagementService();
                string path = uploadedFile.InputStream.ToString();
                byte[] imageSize = new byte[uploadedFile.ContentLength];

                uploadedFile.InputStream.Read(imageSize, 0, (int)uploadedFile.ContentLength);
                UploadedFiles fu = new UploadedFiles();
                fu.DocumentFor = documentFor;
                fu.DocumentType = docType;
                fu.PreRegNum = sdv.PreRegNum;
                fu.DocumentData = imageSize;
                fu.DocumentName = theFile.FileName;
                //   fu.DocumentType = sd.DocumentType;
                fu.DocumentSize = theFile.ContentLength.ToString();
                fu.UploadedDate = DateTime.Now.Date.ToString("dd/MM/yyyy");

                ads.CreateOrUpdateUploadedFiles(fu);

                return Json(new { success = true, result = "Successfully uploaded the file!" }, "text/html", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, result = "You have uploded the empty file. Please upload the correct file." }, "text/x-json", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        //Comments by Dhana and Given by Krishna
        //[HttpPost]
        //public ActionResult StaffRegistrationDetails(HttpPostedFileBase imgupload, StaffDetails staffdetails, long step, string btn, StaffFamilyDetails family, long? FamilyId, StaffExperience experience, long? expId, StaffReferenceDetails reference, long? refId, StaffQualification qualification, long? StaffQualId)
        //{
        //    if (imgupload != null || staffdetails != null)
        //    {
        //        if (staffdetails.Id > 0)
        //        {
        //            StaffDetails ExistStaff = smsObj.GetStaffDetailsId(staffdetails.Id);
        //            if (ExistStaff != null)
        //            {
        //                if (step == 1)
        //                {
        //                    ExistStaff.Name = staffdetails.Name;
        //                    ExistStaff.Gender = staffdetails.Gender;
        //                    ExistStaff.DOB = staffdetails.DOB;
        //                    ExistStaff.Age = staffdetails.Age;
        //                    ExistStaff.PhoneNo = staffdetails.PhoneNo;
        //                    ExistStaff.AltPhoneNo = staffdetails.AltPhoneNo;
        //                    ExistStaff.EmailId = staffdetails.EmailId;
        //                    ExistStaff.BGRP = staffdetails.BGRP;
        //                    ExistStaff.MaritalStatus = staffdetails.MaritalStatus;
        //                    ExistStaff.Country = staffdetails.Country;
        //                    //ExistStaff.State = staffdetails.State;
        //                    ExistStaff.NativeState = staffdetails.NativeState;
        //                    ExistStaff.City = staffdetails.City;
        //                    ExistStaff.LanguagesKnown = staffdetails.LanguagesKnown;
        //                    ExistStaff.Written_LanguagesKnown = staffdetails.Written_LanguagesKnown;
        //                    ExistStaff.FatherName = staffdetails.FatherName;
        //                    ExistStaff.FatherOccupation = staffdetails.FatherOccupation;
        //                    ExistStaff.SpouseName = staffdetails.SpouseName;
        //                    ExistStaff.SpouseOccupation = staffdetails.SpouseOccupation;
        //                    ExistStaff.EmergencyContactPerson = staffdetails.EmergencyContactPerson;
        //                    ExistStaff.EmergencyContactNo = staffdetails.EmergencyContactNo;
        //                    ExistStaff.AlternateAddress = staffdetails.AlternateAddress;
        //                    ExistStaff.PermanantAddress = staffdetails.PermanantAddress;
        //                    AdmissionManagementService adms = new AdmissionManagementService();
        //                    UploadedFiles uploaded = adms.GetUploadedFilesByPreRegNumandDocumentType(ExistStaff.PreRegNum);
        //                    if (uploaded != null && uploaded.DocumentName == imgupload.FileName)
        //                    {

        //                    }
        //                    else if (uploaded != null && uploaded.DocumentName != imgupload.FileName)
        //                    {
        //                        string path1 = imgupload.InputStream.ToString();
        //                        byte[] imageSize1 = new byte[imgupload.ContentLength];
        //                        uploaded.DocumentName = imgupload.FileName;
        //                        uploaded.DocumentData = imageSize1;
        //                        uploaded.DocumentSize = imgupload.ContentLength.ToString();
        //                        uploaded.UploadedDate = DateTime.Now.Date.ToString("dd/MM/yyyy");
        //                        adms.CreateOrUpdateUploadedFiles(uploaded);
        //                    }
        //                }
        //                else if (step == 2)
        //                {
        //                    if (FamilyId > 0)
        //                    {
        //                        StaffFamilyDetails stafffamily = smsObj.GetStaffFamilyDetailsById(FamilyId ?? 0);
        //                        stafffamily.Name = family.Name;
        //                        stafffamily.Occupation = family.Occupation;
        //                        stafffamily.Age = family.Age;
        //                        stafffamily.Relationship = family.Relationship;
        //                        smsObj.CreateOrUpdateStaffFamilyDetails(stafffamily);
        //                        return Json(new { success = true, Message = "success", FamilyId = stafffamily.Id }, "text/html", JsonRequestBehavior.AllowGet);
        //                    }
        //                    else
        //                    {
        //                        family.PreRegNum = ExistStaff.PreRegNum;
        //                        family.Id = 0;
        //                        smsObj.CreateOrUpdateStaffFamilyDetails(family);
        //                        return Json(new { success = true, Message = "success", FamilyId = family.Id }, "text/html", JsonRequestBehavior.AllowGet);
        //                    }
        //                }
        //                else if (step == 3)
        //                {
        //                    if (StaffQualId > 0)
        //                    {
        //                        smsObj.CreateOrUpdateStaffDetails(ExistStaff);
        //                        StaffQualification staffqualification = smsObj.GetStaffQualificationDetailsById(StaffQualId ?? 0);
        //                        staffqualification.Course = qualification.Course;
        //                        staffqualification.Board = qualification.Board;
        //                        staffqualification.Institute = qualification.Institute;
        //                        staffqualification.YearOfComplete = qualification.YearOfComplete;
        //                        staffqualification.MajorSubjects = qualification.MajorSubjects;
        //                        staffqualification.Percentage = qualification.Percentage;
        //                        smsObj.CreateOrUpdateStaffQualificatoinDetails(staffqualification);
        //                        return Json(new { success = true, Message = "success", QualId = staffqualification.Id }, "text/html", JsonRequestBehavior.AllowGet);
        //                    }
        //                    else
        //                    {
        //                        qualification.PreRegNum = ExistStaff.PreRegNum;
        //                        qualification.Id = 0;
        //                        smsObj.CreateOrUpdateStaffQualificatoinDetails(qualification);
        //                        return Json(new { success = true, Message = "success", QualId = qualification.Id }, "text/html", JsonRequestBehavior.AllowGet);
        //                    }
        //                }
        //                else if (step == 4)
        //                {
        //                    if (btn == "next")
        //                    {
        //                        ExistStaff.TotalYearsOfExp = staffdetails.TotalYearsOfExp;
        //                        ExistStaff.TotalYearsOfTeachingExp = staffdetails.TotalYearsOfTeachingExp;
        //                        ExistStaff.GradesTaught = staffdetails.GradesTaught;
        //                        ExistStaff.SyllabusHandled = staffdetails.SyllabusHandled;
        //                        ExistStaff.Achievments = staffdetails.Achievments;
        //                        ExistStaff.SubjectsTaught = staffdetails.SubjectsTaught;
        //                        smsObj.CreateOrUpdateStaffDetails(ExistStaff);
        //                    }
        //                    else if (expId > 0)
        //                    {
        //                        StaffExperience staffexperience = smsObj.GetStaffExperienceDetailsById(expId ?? 0);
        //                        staffexperience.EmployerName = experience.EmployerName;
        //                        staffexperience.Location = experience.Location;
        //                        staffexperience.StartDate = experience.StartDate;
        //                        staffexperience.TillDate = experience.TillDate;
        //                        staffexperience.LastDesignation = experience.LastDesignation;
        //                        staffexperience.SpecificReasonForLeaving = experience.SpecificReasonForLeaving;
        //                        smsObj.CreateOrUpdateStaffExperienceDetails(staffexperience);
        //                        return Json(new { success = true, Message = "success", ExpId = staffexperience.Id }, "text/html", JsonRequestBehavior.AllowGet);
        //                    }
        //                    else
        //                    {
        //                        experience.PreRegNum = ExistStaff.PreRegNum;
        //                        experience.Id = 0;
        //                        smsObj.CreateOrUpdateStaffExperienceDetails(experience);
        //                        return Json(new { success = true, Message = "success", ExpId = experience.Id, fresher = experience.Fresher }, "text/html", JsonRequestBehavior.AllowGet);
        //                    }
        //                }
        //                else if (step == 5)
        //                {
        //                    ExistStaff.LastDrawnGrossSalary = staffdetails.LastDrawnGrossSalary;
        //                    ExistStaff.LastDrawnNettSalary = staffdetails.LastDrawnNettSalary;
        //                    ExistStaff.ExpectedSalary = staffdetails.ExpectedSalary;
        //                    ExistStaff.JoiningDateOrDays = staffdetails.JoiningDateOrDays;
        //                }
        //                else if (step == 6)
        //                {
        //                    ExistStaff.AnyOtherSignificant = staffdetails.AnyOtherSignificant;
        //                    ExistStaff.SpecialInterests = staffdetails.SpecialInterests;
        //                    ExistStaff.HowYouKnowVacancy = staffdetails.HowYouKnowVacancy;
        //                    ExistStaff.BeenShortListedBefore = staffdetails.BeenShortListedBefore;
        //                    ExistStaff.ShortlistedWhy = staffdetails.ShortlistedWhy;
        //                    ExistStaff.RelativeWorkingWithUs = staffdetails.RelativeWorkingWithUs;
        //                    ExistStaff.RelativeDetails = staffdetails.RelativeDetails;
        //                    ExistStaff.CommitTimeWithTIPS = staffdetails.CommitTimeWithTIPS;
        //                    ExistStaff.CareerGrowthExpectation = staffdetails.CareerGrowthExpectation;
        //                    ExistStaff.WillingToTravel = staffdetails.WillingToTravel;
        //                    ExistStaff.WillingForRelocation = staffdetails.WillingForRelocation;
        //                }
        //                else if (step == 7)
        //                {
        //                    if (refId > 0)
        //                    {
        //                        StaffReferenceDetails staffreference = smsObj.GetStaffReferenceDetailsById(refId ?? 0);
        //                        staffreference.RefName = reference.RefName;
        //                        staffreference.RefContactNo = reference.RefContactNo;
        //                        staffreference.RefHowKnow = reference.RefHowKnow;
        //                        staffreference.RefHowLongKnow = reference.RefHowLongKnow;
        //                        smsObj.CreateOrUpdateStaffReferenceDetails(staffreference);
        //                        return Json(new { success = true, Message = "success", RefrenceId = staffreference.Id }, "text/html", JsonRequestBehavior.AllowGet);
        //                    }
        //                    else
        //                    {
        //                        reference.PreRegNum = ExistStaff.PreRegNum;
        //                        reference.Id = 0;
        //                        smsObj.CreateOrUpdateStaffReferenceDetails(reference);
        //                        return Json(new { success = true, Message = "success", RefrenceId = reference.Id }, "text/html", JsonRequestBehavior.AllowGet);
        //                    }
        //                }
        //                else { }
        //                smsObj.CreateOrUpdateStaffDetails(ExistStaff);
        //            }
        //        }
        //        else
        //        {
        //            Dictionary<long, IList<StaffRequestNumDetails>> prd = smsObj.GetStaffRequestNumDetailsListWithPaging(0, 10000, string.Empty, string.Empty, null);
        //            var id1 = prd.First().Value[0].PreRegNum + 1;
        //            ViewBag.RequestNum = id1;
        //            Session["Reqnum"] = id1;
        //            StaffRequestNumDetails srn = new StaffRequestNumDetails();
        //            srn.PreRegNum = id1;
        //            srn.Id = prd.First().Value[0].Id;
        //            srn.Date = DateTime.Now.ToShortDateString();
        //            srn.Time = DateTime.Now.ToShortTimeString();
        //            smsObj.CreateOrUpdateStaffRequestNumDetails(srn);
        //            staffdetails.Status = "NewStaffEnquiry";
        //            staffdetails.PreRegNum = id1;
        //            smsObj.CreateOrUpdateStaffDetails(staffdetails);
        //            if (imgupload != null)
        //            {
        //                AdmissionManagementService ads = new AdmissionManagementService();

        //                UploadedFiles upload = new UploadedFiles();
        //                string path = imgupload.InputStream.ToString();
        //                byte[] imageSize = new byte[imgupload.ContentLength];
        //                imgupload.InputStream.Read(imageSize, 0, (int)imgupload.ContentLength);
        //                upload.DocumentType = "Staff Photo";
        //                upload.DocumentSize = imgupload.ContentLength.ToString();
        //                upload.DocumentFor = "Staff";
        //                upload.PreRegNum = staffdetails.PreRegNum;
        //                upload.DocumentData = imageSize;
        //                upload.DocumentName = imgupload.FileName;
        //                //   fu.DocumentType = sd.DocumentType;                        
        //                upload.UploadedDate = DateTime.Now.Date.ToString("dd/MM/yyyy");
        //                ads.CreateOrUpdateUploadedFiles(upload);
        //            }
        //            return Json(new { success = true, Message = "success", StaffId = staffdetails.Id, interest = staffdetails.InterestedArea }, "text/html", JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    return Json(new { success = false, Message = "failed" }, "text/html", JsonRequestBehavior.AllowGet);

        //}


        //given by Krishna and Deployed by Dhanabalan
        [HttpPost]
        public ActionResult StaffRegistrationDetails(HttpPostedFileBase imgupload, CandidateDtls staffdetails)
        {
            if (staffdetails != null)
            {
                staffdetails.Status = "NewStaffEnquiry";
//                staffdetails.PreRegNum = long.Parse("1");
                staffdetails.CreatedDate = DateTime.Now;
                smsObj.CreateOrUpdateCandidateDtls(staffdetails);
                staffdetails.TempId = "TIPSCAN-" + staffdetails.Id;
                smsObj.CreateOrUpdateCandidateDtls(staffdetails);
                if (imgupload != null)
                {
                    var imagepath = ConfigurationManager.AppSettings["ImageFilePath2"];
                    var path = Path.Combine(imagepath, staffdetails.Id.ToString() + ".jpg");
                    imgupload.SaveAs(path);
                }
                return Json(new { success = true, Message = "success", StaffId = staffdetails.Id, interest = staffdetails.InterestedArea }, "text/html", JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { success = false, Message = "failed" }, "text/html", JsonRequestBehavior.AllowGet);
            //if (staffdetails != null)
            //{
            //    if (staffdetails.Id > 0)
            //    {
            //        CandidateDtls ExistStaff = smsObj.GetCandidateDtlsId(staffdetails.Id);
            //        if (ExistStaff != null)
            //        {
            //                ExistStaff.Name = staffdetails.Name;
            //                ExistStaff.Gender = staffdetails.Gender;
            //                ExistStaff.DOB = staffdetails.DOB;
            //                ExistStaff.Age = staffdetails.Age;
            //                ExistStaff.PhoneNo = staffdetails.PhoneNo;
            //                ExistStaff.AltPhoneNo = staffdetails.AltPhoneNo;
            //                ExistStaff.EmailId = staffdetails.EmailId;
            //                ExistStaff.BGRP = staffdetails.BGRP;
            //                ExistStaff.MaritalStatus = staffdetails.MaritalStatus;
            //                ExistStaff.NativeCountry = staffdetails.NativeCountry;
            //                ExistStaff.NativeState = staffdetails.NativeState;
            //                ExistStaff.NativeCity = staffdetails.NativeCity;
            //                ExistStaff.LanguagesKnown = staffdetails.LanguagesKnown;
            //                ExistStaff.Written_LanguagesKnown = staffdetails.Written_LanguagesKnown;
            //                ExistStaff.FatherName = staffdetails.FatherName;
            //                ExistStaff.FatherOccupation = staffdetails.FatherOccupation;
            //                ExistStaff.SpouseName = staffdetails.SpouseName;
            //                ExistStaff.SpouseOccupation = staffdetails.SpouseOccupation;
            //                ExistStaff.EmergencyContactPerson = staffdetails.EmergencyContactPerson;
            //                ExistStaff.EmergencyContactNo = staffdetails.EmergencyContactNo;
            //                ExistStaff.AlternateAddress = staffdetails.AlternateAddress;
            //                ExistStaff.PermanantAddress = staffdetails.PermanantAddress;
            //                ExistStaff.AadhaarNo = staffdetails.AadhaarNo;
            //                ExistStaff.LocationPreferred = staffdetails.LocationPreferred;
            //                ExistStaff.InterestedArea = staffdetails.InterestedArea;
            //                ExistStaff.Department = staffdetails.Department;
            //                ExistStaff.Designation = staffdetails.Designation;
            //                //AdmissionManagementService adms = new AdmissionManagementService();
            //                //UploadedFiles uploaded = adms.GetUploadedFilesByPreRegNumandDocumentType(ExistStaff.PreRegNum);
            //                //if (uploaded != null && uploaded.DocumentName == imgupload.FileName)
            //                //{

            //                //}
            //                //else if (uploaded != null && uploaded.DocumentName != imgupload.FileName)
            //                //{
            //                //    string path1 = imgupload.InputStream.ToString();
            //                //    byte[] imageSize1 = new byte[imgupload.ContentLength];
            //                //    uploaded.DocumentName = imgupload.FileName;
            //                //    uploaded.DocumentData = imageSize1;
            //                //    uploaded.DocumentSize = imgupload.ContentLength.ToString();
            //                //    uploaded.UploadedDate = DateTime.Now.Date.ToString("dd/MM/yyyy");
            //                //    adms.CreateOrUpdateUploadedFiles(uploaded);
            //                //}
            //            }
            //            else if (step == 2)
            //            {
            //                if (FamilyId > 0)
            //                {
            //                    CandidateFamilyDetails stafffamily = smsObj.GetCandidateFamilyDetailsById(FamilyId ?? 0);
            //                    stafffamily.Name = family.Name;
            //                    stafffamily.Occupation = family.Occupation;
            //                    stafffamily.Age = family.Age;
            //                    stafffamily.Relationship = family.Relationship;
            //                    smsObj.CreateOrUpdateCandidateFamilyDetails(stafffamily);
            //                    return Json(new { success = true, Message = "success", FamilyId = stafffamily.Id }, "text/html", JsonRequestBehavior.AllowGet);
            //                }
            //                else
            //                {
            //                    family.PreRegNum = ExistStaff.PreRegNum;
            //                    family.Id = 0;
            //                    smsObj.CreateOrUpdateCandidateFamilyDetails(family);
            //                    return Json(new { success = true, Message = "success", FamilyId = family.Id }, "text/html", JsonRequestBehavior.AllowGet);
            //                }
            //            }
            //            else if (step == 3)
            //            {
            //                if (StaffQualId > 0)
            //                {
            //                    smsObj.CreateOrUpdateCandidateDtls(ExistStaff);
            //                    CandidateQualification staffqualification = smsObj.GetCandidateQualificationById(StaffQualId ?? 0);
            //                    staffqualification.Course = qualification.Course;
            //                    staffqualification.Board = qualification.Board;
            //                    staffqualification.Institute = qualification.Institute;
            //                    staffqualification.YearOfComplete = qualification.YearOfComplete;
            //                    staffqualification.MajorSubjects = qualification.MajorSubjects;
            //                    staffqualification.Percentage = qualification.Percentage;
            //                    smsObj.CreateOrUpdateCandidateQualificatoin(staffqualification);
            //                    return Json(new { success = true, Message = "success", QualId = staffqualification.Id }, "text/html", JsonRequestBehavior.AllowGet);
            //                }
            //                else
            //                {
            //                    qualification.PreRegNum = ExistStaff.PreRegNum;
            //                    qualification.Id = 0;
            //                    smsObj.CreateOrUpdateCandidateQualificatoin(qualification);
            //                    return Json(new { success = true, Message = "success", QualId = qualification.Id }, "text/html", JsonRequestBehavior.AllowGet);
            //                }
            //            }
            //            else if (step == 4)
            //            {
            //                if (btn == "next")
            //                {
            //                    ExistStaff.Qualification = staffdetails.Qualification;
            //                    ExistStaff.TotalYearsOfExp = staffdetails.TotalYearsOfExp;
            //                    ExistStaff.TotalYearsOfTeachingExp = staffdetails.TotalYearsOfTeachingExp;
            //                    ExistStaff.GradesTaught = staffdetails.GradesTaught;
            //                    ExistStaff.SyllabusHandled = staffdetails.SyllabusHandled;
            //                    ExistStaff.Achievments = staffdetails.Achievments;
            //                    ExistStaff.SubjectsTaught = staffdetails.SubjectsTaught;
            //                    smsObj.CreateOrUpdateCandidateDtls(ExistStaff);
            //                }
            //                else if (expId > 0)
            //                {
            //                    CandidateExperience staffexperience = smsObj.GetCandidateExperienceById(expId ?? 0);
            //                    staffexperience.EmployerName = experience.EmployerName;
            //                    staffexperience.Location = experience.Location;
            //                    staffexperience.StartDate = experience.StartDate;
            //                    staffexperience.TillDate = experience.TillDate;
            //                    staffexperience.LastDesignation = experience.LastDesignation;
            //                    staffexperience.SpecificReasonForLeaving = experience.SpecificReasonForLeaving;
            //                    smsObj.CreateOrUpdateCandidateExperience(staffexperience);
            //                    return Json(new { success = true, Message = "success", ExpId = staffexperience.Id }, "text/html", JsonRequestBehavior.AllowGet);
            //                }
            //                else
            //                {
            //                    experience.PreRegNum = ExistStaff.PreRegNum;
            //                    experience.Id = 0;
            //                    smsObj.CreateOrUpdateCandidateExperience(experience);
            //                    return Json(new { success = true, Message = "success", ExpId = experience.Id, fresher = experience.Fresher }, "text/html", JsonRequestBehavior.AllowGet);
            //                }
            //            }
            //            else if (step == 5)
            //            {
            //                ExistStaff.LastDrawnGrossSalary = staffdetails.LastDrawnGrossSalary;
            //                ExistStaff.LastDrawnNettSalary = staffdetails.LastDrawnNettSalary;
            //                ExistStaff.ExpectedSalary = staffdetails.ExpectedSalary;
            //                ExistStaff.JoiningDateOrDays = staffdetails.JoiningDateOrDays;
            //            }
            //            else if (step == 6)
            //            {
            //                ExistStaff.AnyOtherSignificant = staffdetails.AnyOtherSignificant;
            //                ExistStaff.SpecialInterests = staffdetails.SpecialInterests;
            //                ExistStaff.HowYouKnowVacancy = staffdetails.HowYouKnowVacancy;
            //                ExistStaff.BeenShortListedBefore = staffdetails.BeenShortListedBefore;
            //                ExistStaff.ShortlistedWhy = staffdetails.ShortlistedWhy;
            //                ExistStaff.RelativeWorkingWithUs = staffdetails.RelativeWorkingWithUs;
            //                ExistStaff.RelativeDetails = staffdetails.RelativeDetails;
            //                ExistStaff.CommitTimeWithTIPS = staffdetails.CommitTimeWithTIPS;
            //                ExistStaff.CareerGrowthExpectation = staffdetails.CareerGrowthExpectation;
            //                ExistStaff.WillingToTravel = staffdetails.WillingToTravel;
            //                ExistStaff.WillingForRelocation = staffdetails.WillingForRelocation;
            //                //ExistStaff.InterviewCampus = staffdetails.InterviewCampus;
            //                //ExistStaff.InterviewDate = DateTime.Now;
            //            }
            //            else if (step == 7)
            //            {
            //                if (refId > 0)
            //                {
            //                    CandidateReferenceDtls staffreference = smsObj.GetCandidateReferenceDtlsById(refId ?? 0);
            //                    staffreference.RefName = reference.RefName;
            //                    staffreference.RefContactNo = reference.RefContactNo;
            //                    staffreference.RefHowKnow = reference.RefHowKnow;
            //                    staffreference.RefHowLongKnow = reference.RefHowLongKnow;
            //                    smsObj.CreateOrUpdateCandidateReferenceDtls(staffreference);
            //                    return Json(new { success = true, Message = "success", RefrenceId = staffreference.Id }, "text/html", JsonRequestBehavior.AllowGet);
            //                }
            //                else
            //                {
            //                    reference.PreRegNum = ExistStaff.PreRegNum;
            //                    reference.Id = 0;
            //                    smsObj.CreateOrUpdateCandidateReferenceDtls(reference);
            //                    return Json(new { success = true, Message = "success", RefrenceId = reference.Id }, "text/html", JsonRequestBehavior.AllowGet);
            //                }
            //            }
            //            else { }
            //            smsObj.CreateOrUpdateCandidateDtls(ExistStaff);
            //        }
            //    }
            //else
            //{
            //    //Dictionary<long, IList<CandidateRequestNumDetails>> prd = smsObj.GetCandidateRequestNumDetailsListWithPaging(0, 10000, string.Empty, string.Empty, null);
            //    //var id1 = prd.First().Value[0].PreRegNum + 1;
            //    //ViewBag.RequestNum = id1;
            //    //Session["Reqnum"] = id1;
            //    //CandidateRequestNumDetails srn = new CandidateRequestNumDetails();
            //    //srn.PreRegNum = id1;
            //    //srn.Id = prd.First().Value[0].Id;
            //    //srn.Date = DateTime.Now.ToShortDateString();
            //    //srn.Time = DateTime.Now.ToShortTimeString();
            //    //smsObj.CreateOrUpdateCandidateRequestNumDetails(srn);
            //    staffdetails.Status = "NewStaffEnquiry";
            //    staffdetails.PreRegNum = long.Parse("1");
            //    smsObj.CreateOrUpdateCandidateDtls(staffdetails);
            //    //if (imgupload != null)
            //    //{
            //    //    AdmissionManagementService ads = new AdmissionManagementService();

            //    //    UploadedFiles upload = new UploadedFiles();
            //    //    string path = imgupload.InputStream.ToString();
            //    //    byte[] imageSize = new byte[imgupload.ContentLength];
            //    //    imgupload.InputStream.Read(imageSize, 0, (int)imgupload.ContentLength);
            //    //    upload.DocumentType = "Staff Photo";
            //    //    upload.DocumentSize = imgupload.ContentLength.ToString();
            //    //    upload.DocumentFor = "Staff";
            //    //    upload.PreRegNum = staffdetails.PreRegNum;
            //    //    upload.DocumentData = imageSize;
            //    //    upload.DocumentName = imgupload.FileName;
            //    //    //   fu.DocumentType = sd.DocumentType;                        
            //    //    upload.UploadedDate = DateTime.Now.Date.ToString("dd/MM/yyyy");
            //    //    ads.CreateOrUpdateUploadedFiles(upload);
            //    //}
            //    return Json(new { success = true, Message = "success", StaffId = staffdetails.Id, interest = staffdetails.InterestedArea }, "text/html", JsonRequestBehavior.AllowGet);
            //}
            //}
            //return Json(new { success = false, Message = "failed" }, "text/html", JsonRequestBehavior.AllowGet);
        }

        #region autocomplete by john Naveen

        public ActionResult GetStaffNameAutoComplete(String term)
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(term)) { criteria.Add("Name", term); }
                criteria.Add("WorkingType", "Employee");
                Dictionary<long, IList<StaffDetailsView>> HelperList = sms.GetStaffDetailsViewListWithPaging(0, 9999, "Name", string.Empty, criteria);
                if (HelperList != null && HelperList.FirstOrDefault().Key > 0)
                {
                    var HelperNameList = (from u in HelperList.FirstOrDefault().Value
                                          select new { Name = u.Name, Id = u.Id }).Distinct().ToList();
                    return Json(HelperNameList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        #endregion
        #region StudentSurveyGroup by john naveen
        public ActionResult StudentSurveyGroup()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
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
                    #region BreadCrumb
                    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                    #endregion
                    ViewBag.currentAcyear = DateTime.Now.Month > 5 ? DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Year + 1).ToString() : (DateTime.Now.Year - 1).ToString() + "-" + DateTime.Now.Year.ToString();
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult StudentSurveyGroupJqgrid(string Campus, string Grade, string AcademicYear, string StudentSurveyGroup, string IsActive, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<string, object> likeCriteria = new Dictionary<string, object>();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (!string.IsNullOrEmpty(Campus))
                {
                    likeCriteria.Add("Campus", Campus);
                }
                if (!string.IsNullOrEmpty(Grade))
                {
                    criteria.Add("Grade", Grade);
                }
                if (!string.IsNullOrEmpty(AcademicYear))
                {
                    criteria.Add("AcademicYear", AcademicYear);
                }
                if (!string.IsNullOrEmpty(StudentSurveyGroup))
                {
                    likeCriteria.Add("StudentSurveyGroup", StudentSurveyGroup);
                }
                if (!string.IsNullOrEmpty(IsActive))
                {
                    if (IsActive == "true" || IsActive == "True")
                    {
                        criteria.Add("IsActive", true);
                    }
                    if (IsActive == "false" || IsActive == "False")
                    {
                        criteria.Add("IsActive", false);
                    }
                }
                Dictionary<long, IList<StudentSurveyGroupMaster>> StudentSurveyGroupList = sms.GetStudentSurveyGroupListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria, likeCriteria);
                if (StudentSurveyGroupList != null && StudentSurveyGroupList.Count > 0)
                {
                    long totalrecords = StudentSurveyGroupList.First().Key;
                    int totalpages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalpages,
                        page = page,
                        records = totalrecords,
                        rows = (from items in StudentSurveyGroupList.FirstOrDefault().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[]{
                                          items.StudentSurveyGroupId.ToString(),
                                          items.AcademicYear,
                                          items.Campus,
                                          items.Grade,
                                          items.StudentSurveyGroup, 
                                          items.IsActive==true?"Yes":"No",
                                          items.CreatedBy,
                                          items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy"):"",
                                          items.ModifiedBy,
                                          items.ModifiedDate!=null?items.ModifiedDate.Value.ToString("dd/MM/yyyy"):""
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
        public ActionResult AddStudentSurveyGroup(StudentSurveyGroupMaster studentSurveyGroupms)
        {
            try
            {
                IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                string userId = ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    StaffManagementService sms = new StaffManagementService();
                    IList<StudentSurveyGroupMaster> StudentSurveyGroupList = new List<StudentSurveyGroupMaster>();
                    IList<StudentSurveyGroupMaster> StudentSurveyGroupList1 = new List<StudentSurveyGroupMaster>();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<string, object> likeCriteria = new Dictionary<string, object>();
                    criteria.Add("AcademicYear", studentSurveyGroupms.AcademicYear);
                    string[] ArrCampus = studentSurveyGroupms.Campus.Split(',');
                    string[] ArrGrade = studentSurveyGroupms.Grade.Split(',');
                    criteria.Add("StudentSurveyGroup", studentSurveyGroupms.StudentSurveyGroup);
                    criteria.Add("IsActive", true);
                    foreach (var items in ArrCampus)
                    {
                        if (!string.IsNullOrEmpty(items))
                        {
                            criteria.Add("Campus", items);
                            foreach (var Gradeitems in ArrGrade)
                            {
                                if (!string.IsNullOrEmpty(Gradeitems))
                                {
                                    criteria.Add("Grade", Gradeitems);
                                    Dictionary<long, IList<StudentSurveyGroupMaster>> studentSurveyGroupList = sms.GetStudentSurveyGroupListWithPagingAndCriteria(null, null, null, null, criteria, likeCriteria);
                                    criteria.Remove("Grade");
                                    if (studentSurveyGroupList.FirstOrDefault().Key == 0)
                                    {
                                        StudentSurveyGroupMaster studsurgp = new StudentSurveyGroupMaster();
                                        studsurgp.AcademicYear = studentSurveyGroupms.AcademicYear;
                                        studsurgp.Campus = items;
                                        studsurgp.Grade = Gradeitems;
                                        studsurgp.StudentSurveyGroup = studentSurveyGroupms.StudentSurveyGroup;
                                        studsurgp.IsActive = true;
                                        studsurgp.CreatedBy = userId;
                                        studsurgp.CreatedDate = DateTime.Now;
                                        studsurgp.ModifiedBy = userId;
                                        studsurgp.ModifiedDate = DateTime.Now;
                                        StudentSurveyGroupList.Add(studsurgp);
                                    }
                                }
                            }
                            criteria.Remove("Campus");
                        }
                    }
                    if (StudentSurveyGroupList.Count > 0)
                    {
                        sms.SaveOrUpdateStudentSurveyGroupByList(StudentSurveyGroupList);
                        return Json("success", JsonRequestBehavior.AllowGet);
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
        public ActionResult EditStudentSurveyGroup(StudentSurveyGroupMaster studentSurveyGroupms)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (studentSurveyGroupms.StudentSurveyGroupId > 0)
                    {
                        StudentSurveyGroupMaster Obj = new StudentSurveyGroupMaster();
                        StaffManagementService sms = new StaffManagementService();
                        Obj = sms.GetStudentSurveyGroupById(studentSurveyGroupms.StudentSurveyGroupId);
                        if (Obj != null)
                        {
                            Obj.IsActive = studentSurveyGroupms.IsActive;
                            Obj.ModifiedBy = userId;
                            Obj.ModifiedDate = DateTime.Now;
                            Obj.IsActive = studentSurveyGroupms.IsActive;
                            sms.SaveOrUpdateStudentSurveyGroup(Obj);
                        }
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
        #endregion
        #region Student Survey Question and Answer Master by vinoth
        public ActionResult StudentSurveyQuestionMaster()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    ViewBag.ddlcampus = CampusMasterFunc();
                    ViewBag.acadddl = AcademicyrMasterFunc();
                    #region BreadCrumb
                    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                    #endregion
                    ViewBag.currentAcyear = DateTime.Now.Month > 5 ? DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Year + 1).ToString() : (DateTime.Now.Year - 1).ToString() + "-" + DateTime.Now.Year.ToString();
                    return View();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult GetJqGridstudsurveyList(StudentSurveyQuestionMasterView studsurvey, string IsActive, long? StudentSurveyGroupId, string AcademicYear, string Campus, string Grade, int rows, string sord, string sidx, int? page = 1)
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<string, object> likeCriteria = new Dictionary<string, object>();
                criteria.Clear();
                sord = sord == "desc" ? "Desc" : "Asc";
                var usrcmp = Session["UserCampus"] as IEnumerable<string>;
                if (!string.IsNullOrEmpty(studsurvey.AcademicYear))
                    criteria.Add("AcademicYear", studsurvey.AcademicYear);
                if (!string.IsNullOrEmpty(studsurvey.Campus))
                    criteria.Add("Campus", studsurvey.Campus);
                if (!string.IsNullOrEmpty(studsurvey.Grade))
                    criteria.Add("Grade", studsurvey.Grade);
                if (studsurvey.StudentSurveyGroupId > 0)
                    criteria.Add("StudentSurveyGroupId", studsurvey.StudentSurveyGroupId);
                if (!string.IsNullOrEmpty(studsurvey.StudentSurveyQuestion))
                    likeCriteria.Add("StudentSurveyQuestion", studsurvey.StudentSurveyQuestion);

                if (!string.IsNullOrEmpty(IsActive))
                {
                    if (IsActive == "true" || IsActive == "True")
                    {
                        criteria.Add("IsActive", true);
                    }
                    if (IsActive == "false" || IsActive == "False")
                    {
                        criteria.Add("IsActive", false);
                    }
                }
                Dictionary<long, IList<StudentSurveyQuestionMasterView>> StudentSurveyQuestionObj = null;

                StudentSurveyQuestionObj = sms.GetStudentSurveyQuestionMasterViewListWithExcactAndLikeSearchCriteria(page - 1, rows, sord, sidx, criteria, likeCriteria);

                if (StudentSurveyQuestionObj != null && StudentSurveyQuestionObj.FirstOrDefault().Key > 0)
                {
                    IList<StudentSurveyQuestionMasterView> StudentSurveyQuestionDetails = StudentSurveyQuestionObj.FirstOrDefault().Value;
                    long totalRecords = StudentSurveyQuestionObj.FirstOrDefault().Key;
                    int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                    var jsonData = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalRecords,
                        rows =
                        (
                        from items in StudentSurveyQuestionDetails
                        select new
                        {
                            i = items.SurveyQuestionId,
                            cell = new string[]
                           {
                                items.SurveyQuestionId.ToString(),                              
                                items.StudentSurveyQuestionId.ToString(),
                                //items.StudentSurveyGroupMaster.StudentSurveyGroupId.ToString(),
                                items.AcademicYear,
                                items.Campus,
                                items.Grade,
                                items.StudentSurveyGroup,
                                
                                items.StudentSurveyQuestion,
                                items.IsActive==true?"Yes":"No"
                               
                                                             
                           }
                        }
                        )
                    };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var Empty = new { rows = (new { cell = new string[] { } }) };
                    return Json(Empty, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                //ExceptionPolicy.HandleException(ex, "MasterPolicy");
                throw ex;
            }
        }
        public ActionResult AddOrEditStudentSurveyQuestion(StudentSurveyQuestionMaster studsurvey, StudentSurveyQuestionMasterView studsurveyview, string AcademicYear, string Campus, string Grade)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("logOut", "Account");
                else
                {
                    StaffManagementService sms = new StaffManagementService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<string, object> likeCriteria = new Dictionary<string, object>();
                    if (studsurvey == null) return null;
                    if (!string.IsNullOrEmpty(studsurveyview.AcademicYear))
                        criteria.Add("AcademicYear", studsurveyview.AcademicYear);
                    if (!string.IsNullOrEmpty(studsurveyview.Campus))
                        likeCriteria.Add("Campus", studsurveyview.Campus);
                    if (!string.IsNullOrEmpty(studsurveyview.Grade))
                        likeCriteria.Add("Grade", studsurveyview.Grade);
                    if (studsurveyview.StudentSurveyGroupId > 0)
                        likeCriteria.Add("StudentSurveyGroupId", studsurveyview.StudentSurveyGroupId);
                    if (!string.IsNullOrEmpty(studsurveyview.StudentSurveyQuestion))
                        likeCriteria.Add("StudentSurveyQuestion", studsurveyview.StudentSurveyQuestion);
                    Dictionary<long, IList<StudentSurveyQuestionMasterView>> StudentSurveyQuestionObj = null;
                    StudentSurveyQuestionObj = sms.GetStudentSurveyQuestionMasterViewListWithExcactAndLikeSearchCriteria(null, 99999, "", "", criteria, likeCriteria);
                    if (StudentSurveyQuestionObj != null && StudentSurveyQuestionObj.FirstOrDefault().Key > 0)
                    {
                        return Json("failed", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        studsurvey.CreatedBy = userId;
                        studsurvey.CreatedDate = DateTime.Now;
                        studsurvey.ModifiedBy = userId;
                        studsurvey.ModifiedDate = DateTime.Now;
                        studsurvey.IsActive = true;
                        StudentSurveyGroupMaster ssgm = new StudentSurveyGroupMaster();
                        ssgm.StudentSurveyGroupId = studsurveyview.StudentSurveyGroupId;
                        studsurvey.StudentSurveyGroupMaster = ssgm;
                        sms.CreateOrUpdateStudentSurveyQuestion(studsurvey);
                        return Json("insert", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "MasterPolicy");
                throw ex;
            }
        }
        public ActionResult GetStudentAcdemicYearCampusGrade(string AcademicYear, string Campus, string Grade)
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();
                Dictionary<string, object> likeCriteria = new Dictionary<string, object>();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(AcademicYear))
                    criteria.Add("AcademicYear", AcademicYear);
                if (!string.IsNullOrEmpty(Campus))
                    criteria.Add("Campus", Campus);
                if (!string.IsNullOrEmpty(Grade))
                    criteria.Add("Grade", Grade);
                Dictionary<long, IList<StudentSurveyGroupMaster>> StudentSurveyGroupMasterList = sms.GetStudentSurveyGroupListWithPagingAndCriteria(0, 999999, "", "", criteria, likeCriteria);
                var camp = (from items in StudentSurveyGroupMasterList.FirstOrDefault().Value
                            select new
                            {
                                Text = items.StudentSurveyGroup,
                                Value = items.StudentSurveyGroupId
                            }).Distinct().ToList();
                return Json(camp, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "MasterPolicy");
                throw ex;
            }
        }
        public ActionResult StudentSurveyAnswerMaster()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    ViewBag.ddlcampus = CampusMasterFunc();
                    ViewBag.acadddl = AcademicyrMasterFunc();
                    #region BreadCrumb
                    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                    #endregion
                    ViewBag.currentAcyear = DateTime.Now.Month > 5 ? DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Year + 1).ToString() : (DateTime.Now.Year - 1).ToString() + "-" + DateTime.Now.Year.ToString();
                    return View();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult GetJqGridstudsurveyAnswerList(StudentSurveyAnswerView studsurveyans, long? StudentSurveyGroupId, string IsActive, string IsPositive, int rows, string sord, string sidx, int? page = 1)
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<string, object> likeCriteria = new Dictionary<string, object>();
                criteria.Clear();
                sord = sord == "desc" ? "Desc" : "Asc";
                var usrcmp = Session["UserCampus"] as IEnumerable<string>;
                if (!string.IsNullOrEmpty(studsurveyans.AcademicYear))
                    criteria.Add("AcademicYear", studsurveyans.AcademicYear);
                if (!string.IsNullOrEmpty(studsurveyans.Campus))
                    criteria.Add("Campus", studsurveyans.Campus);
                if (!string.IsNullOrEmpty(studsurveyans.Grade))
                    criteria.Add("Grade", studsurveyans.Grade);
                if (studsurveyans.StudentSurveyGroupId > 0)
                    criteria.Add("StudentSurveyGroupId", studsurveyans.StudentSurveyGroupId);
                if (studsurveyans.StudentSurveyQuestionId > 0)
                    criteria.Add("StudentSurveyQuestionId", studsurveyans.StudentSurveyQuestionId);
                if (!string.IsNullOrEmpty(studsurveyans.StudentSurveyAnswer))
                    likeCriteria.Add("StudentSurveyAnswer", studsurveyans.StudentSurveyAnswer);
                if (!string.IsNullOrEmpty(IsActive))
                {
                    if (IsActive == "true" || IsActive == "True")
                    {
                        criteria.Add("IsActive", true);
                    }
                    if (IsActive == "false" || IsActive == "False")
                    {
                        criteria.Add("IsActive", false);
                    }
                }
                if (!string.IsNullOrEmpty(IsPositive))
                {
                    if (IsPositive == "true" || IsPositive == "True")
                    {
                        criteria.Add("IsPositive", true);
                    }
                    if (IsPositive == "false" || IsPositive == "False")
                    {
                        criteria.Add("IsPositive", false);
                    }
                }
                Dictionary<long, IList<StudentSurveyAnswerView>> StudentSurveyAnswerMasterObj = null;

                StudentSurveyAnswerMasterObj = sms.GetStudentSurveyAnswerMasterListWithExcactAndLikeSearchCriteria(page - 1, rows, sord, sidx, criteria, likeCriteria);

                if (StudentSurveyAnswerMasterObj != null && StudentSurveyAnswerMasterObj.FirstOrDefault().Key > 0)
                {
                    IList<StudentSurveyAnswerView> StudentSurveyAnswerMasterDetails = StudentSurveyAnswerMasterObj.FirstOrDefault().Value;
                    long totalRecords = StudentSurveyAnswerMasterObj.FirstOrDefault().Key;
                    int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                    var jsonData = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalRecords,
                        rows =
                        (
                        from items in StudentSurveyAnswerMasterDetails
                        select new
                        {
                            i = items.StudentSurveyAnswerViewId,
                            cell = new string[]
                           {
                                items.StudentSurveyAnswerViewId.ToString(),  
                                items.AcademicYear,
                                items.Campus,
                                items.Grade,
                                items.StudentSurveyGroup,
                                //items.StudentSurveyQuestionMaster.StudentSurveyQuestionId.ToString(),
                                items.StudentSurveyQuestion,
                                items.StudentSurveyAnswerId.ToString(),
                                items.StudentSurveyAnswer,
                                items.StudentSurveyMark.ToString(),
                                items.IsPositive==true?"Yes":"No",
                                items.IsActive==true?"Yes":"No",
                               
                                items.CreatedBy,
                                items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy"):"",
                                items.ModifiedBy,
                                items.ModifiedDate!=null?items.ModifiedDate.Value.ToString("dd/MM/yyyy"):"",                                
                           }
                        }
                        )
                    };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var Empty = new { rows = (new { cell = new string[] { } }) };
                    return Json(Empty, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                //ExceptionPolicy.HandleException(ex, "MasterPolicy");
                throw ex;
            }
        }
        public ActionResult AddOrEditStudentSurveyAnswer(StudentSurveyAnswerMaster studsurveyans, StudentSurveyAnswerView studanswerview, string AcademicYear, string Campus, string Grade)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("logOut", "Account");
                else
                {
                    StaffManagementService sms = new StaffManagementService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<string, object> likeCriteria = new Dictionary<string, object>();
                    if (studsurveyans == null) return null;
                    if (!string.IsNullOrEmpty(studanswerview.AcademicYear))
                        criteria.Add("AcademicYear", studanswerview.AcademicYear);
                    if (!string.IsNullOrEmpty(studanswerview.Campus))
                        likeCriteria.Add("Campus", studanswerview.Campus);
                    if (!string.IsNullOrEmpty(studanswerview.Grade))
                        likeCriteria.Add("Grade", studanswerview.Grade);
                    if (studanswerview.StudentSurveyGroupId > 0)
                        likeCriteria.Add("StudentSurveyGroupId", studanswerview.StudentSurveyGroupId);
                    if (studanswerview.StudentSurveyQuestionId > 0)
                        likeCriteria.Add("StudentSurveyQuestionId", studanswerview.StudentSurveyQuestionId);
                    if (!string.IsNullOrEmpty(studanswerview.StudentSurveyAnswer))
                        criteria.Add("StudentSurveyAnswer", studanswerview.StudentSurveyAnswer);
                    Dictionary<long, IList<StudentSurveyAnswerView>> StudentSurveyAnswerViewObj = null;
                    StudentSurveyAnswerViewObj = sms.GetStudentSurveyAnswerMasterListWithExcactAndLikeSearchCriteria(null, 99999, "", "", criteria, likeCriteria);
                    if (StudentSurveyAnswerViewObj != null && StudentSurveyAnswerViewObj.FirstOrDefault().Key > 0)
                    {
                        return Json("failed", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        studsurveyans.CreatedBy = userId;
                        studsurveyans.CreatedDate = DateTime.Now;
                        studsurveyans.ModifiedBy = userId;
                        studsurveyans.ModifiedDate = DateTime.Now;
                        studsurveyans.IsActive = true;
                        StudentSurveyQuestionMaster ssqm = new StudentSurveyQuestionMaster();
                        ssqm.StudentSurveyQuestionId = studanswerview.StudentSurveyQuestionId;
                        studsurveyans.StudentSurveyQuestionMaster = ssqm;
                        sms.CreateOrUpdateStudentSurveyAnswerMaster(studsurveyans);
                        return Json("insert", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "MasterPolicy");
                throw ex;
            }
        }
        public ActionResult GetStudentAcdemicYearCampusGradeandGroup(string AcademicYear, string Campus, string Grade, long? StudentSurveyGroupId)
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();
                Dictionary<string, object> likeCriteria = new Dictionary<string, object>();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(AcademicYear))
                    criteria.Add("AcademicYear", AcademicYear);
                if (!string.IsNullOrEmpty(Campus))
                    criteria.Add("Campus", Campus);
                if (!string.IsNullOrEmpty(Grade))
                    criteria.Add("Grade", Grade);
                if (StudentSurveyGroupId > 0)
                    criteria.Add("StudentSurveyGroupId", StudentSurveyGroupId);
                Dictionary<long, IList<StudentSurveyQuestionMasterView>> StudentSurveyQuestionMasterList = sms.GetStudentSurveyQuestionMasterViewListWithExcactAndLikeSearchCriteria(0, 999999, "", "", criteria, likeCriteria);
                var camp = (from items in StudentSurveyQuestionMasterList.FirstOrDefault().Value
                            select new
                            {
                                Text = items.StudentSurveyQuestion,
                                Value = items.StudentSurveyQuestionId
                            }).Distinct().ToList();
                return Json(camp, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "MasterPolicy");
                throw ex;
            }
        }
        public ActionResult GetStudentQuestionddl()
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();

                Dictionary<string, object> likeCriteria = new Dictionary<string, object>();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<long, IList<StudentSurveyGroupMaster>> StudentSurveyGroupMasterList = sms.GetStudentSurveyGroupListWithPagingAndCriteria(0, 999999, "", "", criteria, likeCriteria);
                var camp = (from items in StudentSurveyGroupMasterList.FirstOrDefault().Value
                            select new
                            {
                                Text = items.StudentSurveyGroup,
                                Value = items.StudentSurveyGroupId
                            }).Distinct().ToList();
                return Json(camp, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "MasterPolicy");
                throw ex;
            }
        }
        public ActionResult GetStudentAnswerddl(long? StudentSurveyGroupId)
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();

                Dictionary<string, object> likeCriteria = new Dictionary<string, object>();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (StudentSurveyGroupId > 0)
                    criteria.Add("StudentSurveyGroupMaster.StudentSurveyGroupId", StudentSurveyGroupId);
                Dictionary<long, IList<StudentSurveyQuestionMaster>> StudentSurveyQuestionMasterList = sms.GetStudentSurveyQuestionListWithExcactAndLikeSearchCriteria(0, 999999, "", "", criteria, likeCriteria);
                var camp = (from items in StudentSurveyQuestionMasterList.FirstOrDefault().Value
                            select new
                            {
                                Text = items.StudentSurveyQuestion,
                                Value = items.StudentSurveyQuestionId
                            }).Distinct().ToList();
                return Json(camp, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "MasterPolicy");
                throw ex;
            }
        }
        #endregion
        #region Student Survey Grade,Section,Survey no Dropdown list

        [AcceptVerbs(HttpVerbs.Get)] //Newly added for campus wise grade by micheal
        public JsonResult GetStaffEvaluationCategoryGradeByCampus(string Active, string Campus)
        {
            try
            {
                StaffManagementService SS = new StaffManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(Active))
                    criteria.Add("IsActive", Active == "true" ? true : false);
                criteria.Add("Campus", Campus);
                Dictionary<long, IList<StaffEvaluationCategoryMaster>> StaffEvaluationCategoryList = SS.GetStaffEvaluationCategoryListWithPagingAndCriteria(0, 100, string.Empty, string.Empty, criteria, criteria);
                if (StaffEvaluationCategoryList != null && StaffEvaluationCategoryList.Count > 0)
                {
                    var gradeMstrLst =
                         (
                             from items in StaffEvaluationCategoryList.FirstOrDefault().Value
                             select new
                             {
                                 Text = items.Grade,
                                 Value = items.Grade
                             }).Distinct().ToList();
                    return Json(gradeMstrLst, JsonRequestBehavior.AllowGet);
                }
                else
                { return Json(null, JsonRequestBehavior.AllowGet); }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Get)] //Newly added for campus wise grade by micheal
        public JsonResult GetStaffEvaluationCategoryByCampusGrade(string AcaYear, string Active, string Campus, string Grade, string Section)
        {
            try
            {
                StaffManagementService SS = new StaffManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(Active))
                    criteria.Add("IsActive", Active == "true" ? true : false);
                criteria.Add("Campus", Campus);
                if (!string.IsNullOrEmpty(AcaYear))
                    criteria.Add("AcademicYear", AcaYear);
                if (!string.IsNullOrEmpty(Grade))
                    criteria.Add("Grade", Grade);
                if (!string.IsNullOrEmpty(Section) && Section != "ALL")
                    criteria.Add("Section", Section);
                Dictionary<long, IList<StaffEvaluationCategoryMaster>> StaffEvaluationCategoryList = SS.GetStaffEvaluationCategoryListWithPagingAndCriteria(0, 100, string.Empty, string.Empty, criteria, criteria);
                if (StaffEvaluationCategoryList != null && StaffEvaluationCategoryList.Count > 0)
                {
                    var SurveyNoMstrLst =
                         (
                             from items in StaffEvaluationCategoryList.FirstOrDefault().Value
                             select new
                             {
                                 Text = items.CategoryName,
                                 Value = items.CategoryName
                             }).Distinct().ToList();
                    return Json(SurveyNoMstrLst, JsonRequestBehavior.AllowGet);
                }
                else
                { return Json(null, JsonRequestBehavior.AllowGet); }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        #endregion
        //#region StudentSurveyReport

        //public ActionResult StudentSurveyReport()
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //        else
        //        {
        //            MastersService ms = new MastersService();
        //            StaffManagementService SS = new StaffManagementService();
        //            Dictionary<string, object> criteria = new Dictionary<string, object>();
        //            //criteria.Add("IsActive", true);
        //            Dictionary<long, IList<StaffEvaluationCategoryMaster>> StaffEvaluationCategoryList = SS.GetStaffEvaluationCategoryListWithPagingAndCriteria(0, 100, string.Empty, string.Empty, criteria, criteria);
        //            ViewBag.campusddl = (from items in StaffEvaluationCategoryList.FirstOrDefault().Value
        //                                 where items.IsActive = true
        //                                 select new { Campus = items.Campus }).Distinct().ToList();
        //            ViewBag.campusallddl = (from items in StaffEvaluationCategoryList.FirstOrDefault().Value
        //                                    select new { Campus = items.Campus }).Distinct().ToList();
        //            criteria.Clear();
        //            Dictionary<long, IList<AcademicyrMaster>> AcademicyrMaster = ms.GetAcademicyrMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
        //            ViewBag.acadddl = AcademicyrMaster.First().Value;
        //            ViewBag.currentAcyear = DateTime.Now.Month > 5 ? DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Year + 1).ToString() : (DateTime.Now.Year - 1).ToString() + "-" + DateTime.Now.Year.ToString();
        //            #region BreadCrumb
        //            string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //            ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
        //            #endregion
        //            return View();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
        //        throw ex;
        //    }
        //}

        //public ActionResult StudentSurveyStaffwiseReportJqgrid(long Id, string Survey, string ExportType, int rows, string sidx, string sord, int? page = 1)
        //{
        //    try
        //    {
        //        StaffManagementService sms = new StaffManagementService();
        //        string campus = string.Empty;
        //        string AcaYear = string.Empty;
        //        string surveyname = string.Empty;
        //        string StaffName = string.Empty;

        //        long preRegNum = 0;
        //        if (!string.IsNullOrEmpty(Survey))
        //        {
        //            StaffWiseStudentSurveyNewResult_Vw staffwise = sms.GetStaffWiseStudentSurveyNewResultById(Id);
        //            if (staffwise != null)
        //            {
        //                campus = staffwise.Campus;
        //                preRegNum = staffwise.StaffPreRegNumber;
        //            }
        //        }
        //        else
        //        {
        //            StaffWiseStudentSurveyNewResultWOS_Vw staffwisewos = sms.StaffWiseStudentSurveyNewResultWOSById(Id);
        //            if (staffwisewos != null)
        //            {
        //                campus = staffwisewos.Campus;
        //                preRegNum = staffwisewos.StaffPreRegNumber;
        //            }
        //        }

        //        Dictionary<string, object> criteria = new Dictionary<string, object>();
        //        Dictionary<string, object> likeCriteria = new Dictionary<string, object>();
        //        //criteria.Add("CategoryName", staffwise.CategoryName);
        //        criteria.Add("Campus", campus);
        //        criteria.Add("StaffPreRegNumber", preRegNum);
        //        Dictionary<long, IList<StudentSurveyReportNew_Vw>> StaffEvaluationstdCount = sms.GetStaffEvaluationStudentCountList(page - 1, rows, sidx, sord, criteria, likeCriteria);
        //        if (StaffEvaluationstdCount != null && StaffEvaluationstdCount.Count > 0)
        //        {
        //            if (ExportType == "Excel")
        //            {
        //                string title = "StaffStudentSurveyDetailsReprot";
        //                string logopath = ConfigurationManager.AppSettings["AppLogos"].ToString() + "tips.gif";
        //                string headerTable = @"<Table><tr><td colspan='7' align='center' style='font-size: large;'><img src=" + logopath + "><b> The Indian Public School,Coimbatore</b></td></tr><tr><td colspan='7' align='center' style='font-size: large;'><b>Student Survey Result - " + AcaYear + "</b></td></tr><tr>";
        //                headerTable = headerTable + "<td colspan='7' align='center' style='font-size: medium;'><b>Campus</b> : " + campus + " &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<b>Grade : </b> All &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<b>Survey Number : </b>" + surveyname + "<b>Staff Name : </b>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp" + StaffName + " &nbsp&nbsp&nbsp&nbsp&nbsp </td></tr></Table>";
        //                var stuList = StaffEvaluationstdCount.First().Value.ToList();
        //                base.ExptToXL_AssessPointChart(stuList, title, (item => new
        //                {
        //                    EvaluationDate = item.EvaluationDate.ToString("dd/MM/yyyy"),
        //                    item.Grade,
        //                    item.Section,
        //                    item.Subject,
        //                    NoofStudentAttend = item.StudentCount.ToString(),
        //                    NoofQuestioninSurvey = item.QuestionCount.ToString(),
        //                    //Weightage = item.Score
        //                    AverageScore = item.Average
        //                }), headerTable);
        //            }
        //            else
        //            {
        //                long totalrecords = StaffEvaluationstdCount.First().Key;
        //                int totalpages = (int)Math.Ceiling(totalrecords / (float)rows);
        //                var jsondat = new
        //                {
        //                    total = totalpages,
        //                    page = page,
        //                    records = totalrecords,
        //                    rows = (from items in StaffEvaluationstdCount.FirstOrDefault().Value
        //                            select new
        //                            {
        //                                cell = new string[]{
        //                                  items.Id.ToString(),
        //                                  items.AcademicYear, 
        //                                  items.Campus,
        //                                  items.Grade,
        //                                  items.Section,
        //                                  items.CategoryName,
        //                                  items.EvaluationDate.ToString("dd/MM/yyyy"),
        //                                  items.StudentCount.ToString(),
        //                                  items.QuestionCount.ToString(),
        //                                  items.Score.ToString(),
        //                                  //items.Average.ToString(),
        //                                  "<a style='text-decoration: none;cursor: pointer;border-bottom: 1px solid blue;' title='' onclick=\"ShowQuestionWeightageMarkinStaff('"+items.Id+"','" + items.AcademicYear+"','"+items.Campus+ "','" +items.Grade+"','"+items.Section+"','"+ items.StaffPreRegNumber + "','" + items.StaffEvaluationCategoryId + "');\" '>"+items.Average+"</a>",
        //                              }
        //                            })
        //                };
        //                return Json(jsondat, JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //        return Json(new { rows = (new { cell = new string[] { } }) }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
        //        throw ex;
        //    }
        //}

        //public ActionResult StudentSurveyReportAllJqgrid(string Campus, string Grade, string Section, string Surveyno, string AcademicYear, string ExportType, int rows, string sidx, string sord, int? page = 1)
        //{
        //    try
        //    {
        //        StaffManagementService sms = new StaffManagementService();
        //        Dictionary<string, object> criteria = new Dictionary<string, object>();
        //        Dictionary<string, object> likeCriteria = new Dictionary<string, object>();
        //        if (!string.IsNullOrEmpty(Campus))
        //            criteria.Add("Campus", Campus);
        //        if (!string.IsNullOrEmpty(Grade))
        //            criteria.Add("Grade", Grade);
        //        if (!string.IsNullOrEmpty(AcademicYear))
        //            criteria.Add("AcademicYear", AcademicYear);
        //        if (!string.IsNullOrEmpty(Surveyno))
        //            criteria.Add("CategoryName", Surveyno);
        //        if (!string.IsNullOrEmpty(Section) && Section == "ALL")
        //        {
        //            Dictionary<long, IList<StudentSurveyReportWOSecNew_Vw>> studentSureyReportList = sms.GetStaffEvaluationStudentCountListWithoutSection(page - 1, rows, sidx, sord, criteria, likeCriteria);
        //            if (studentSureyReportList != null && studentSureyReportList.Count > 0)
        //            {
        //                if (ExportType == "Excel")
        //                {
        //                    string campus = string.IsNullOrEmpty(Campus) ? "All" : Campus;
        //                    string grade = string.IsNullOrEmpty(Grade) ? "All" : Grade;
        //                    string surveyno = string.IsNullOrEmpty(Surveyno) ? "All" : Surveyno;
        //                    string title = "SutdentSurveyReport";
        //                    string logopath = ConfigurationManager.AppSettings["AppLogos"].ToString() + "tips.gif";
        //                    string headerTable = @"<Table><tr><td colspan='6' align='center' style='font-size: large;'><img src=" + logopath + "><b> The Indian Public School,Coimbatore</b></td></tr><tr><td colspan='6' align='center' style='font-size: large;'><b>Student Survey Result-" + AcademicYear + "</b></td></tr><tr>";
        //                    headerTable = headerTable + "<td colspan='6' align='center' style='font-size: medium;'><b>Campus</b> : " + campus + " &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp <b>Grade : </b> " + grade + " &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<b>Survey Number : </b>" + surveyno + " </td></tr></Table>";
        //                    var stuList = studentSureyReportList.First().Value.ToList();
        //                    base.ExptToXL_AssessPointChart(stuList, title, (item => new
        //                    {
        //                        EvaluationDate = item.EvaluationDate.ToString("dd/MM/yyyy"),
        //                        item.StaffName,
        //                        item.Subject,
        //                        NoofStudentAttend = item.StudentCount.ToString(),
        //                        NoofQuestionAsked = item.QuestionCount.ToString(),
        //                        //Weightage = item.Score,
        //                        AverageScore = item.Average,
        //                    }), headerTable);
        //                }
        //                else
        //                {
        //                    long totalrecords = studentSureyReportList.First().Key;
        //                    int totalpages = (int)Math.Ceiling(totalrecords / (float)rows);
        //                    var jsondat = new
        //                    {
        //                        total = totalpages,
        //                        page = page,
        //                        records = totalrecords,
        //                        rows = (from items in studentSureyReportList.FirstOrDefault().Value
        //                                select new
        //                                {
        //                                    cell = new string[]{
        //                                  items.Id.ToString(),
        //                                  items.AcademicYear, 
        //                                  items.Campus,
        //                                  items.Grade,
        //                                  items.CategoryName,
        //                                  items.EvaluationDate.ToString("dd/MM/yyyy"),
        //                                  items.StaffName+"("+items.Subject+")",
        //                                  items.StudentCount.ToString(),
        //                                  items.QuestionCount.ToString(),
        //                                  items.Score.ToString(),
        //                                  //"<a style='text-decoration: none;border-bottom: 1px solid blue;' onclick=\"ShowWeightageMarkswithQuestion('"+items.Id+"','" + items.AcademicYear+"','"+items.Campus+ "','" +items.Grade+"','"+ items.StaffPreRegNumber + "','" + items.StaffEvaluationCategoryId + "');\" '>"+items.Score+"</a>",
        //                                  //items.Average.ToString()
        //                                  "<a style='text-decoration: none;cursor: pointer;border-bottom: 1px solid blue;' title='' onclick=\"ShowWeightageMarkswithQuestion('"+items.Id+"','" + items.AcademicYear+"','"+items.Campus+ "','" +items.Grade+"','"+ items.StaffPreRegNumber + "','" + items.StaffEvaluationCategoryId + "');\" '>"+items.Average+"</a>",
        //                              }
        //                                })
        //                    };
        //                    return Json(jsondat, JsonRequestBehavior.AllowGet);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (!string.IsNullOrEmpty(Section))
        //                criteria.Add("Section", Section);
        //            Dictionary<long, IList<StudentSurveyReportNew_Vw>> StaffEvaluationstdCount = sms.GetStaffEvaluationStudentCountList(page - 1, rows, sidx, sord, criteria, likeCriteria);
        //            if (StaffEvaluationstdCount != null && StaffEvaluationstdCount.Count > 0)
        //            {
        //                if (ExportType == "Excel")
        //                {
        //                    string campus = String.IsNullOrEmpty(Campus) ? "All" : Campus;
        //                    string gradesec = Grade == null ? "All" : Grade;
        //                    gradesec = gradesec + "-" + Section;
        //                    string surveyno = String.IsNullOrEmpty(Surveyno) ? "All" : Surveyno;
        //                    string title = "SutdentSurveyReport";
        //                    string logopath = ConfigurationManager.AppSettings["AppLogos"].ToString() + "tips.gif";
        //                    string headerTable = @"<Table><tr><td colspan='6' align='center' style='font-size: large;'><img src=" + logopath + "><b> The Indian Public School,Coimbatore</b></td></tr><tr><td colspan='6' align='center' style='font-size: large;'><b>Student Survey Result - " + AcademicYear + "</b></td></tr><tr>";
        //                    headerTable = headerTable + "<td colspan='6' align='center' style='font-size: medium;'><b>Campus</b> : " + campus + " &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<b>Grade : </b> " + gradesec + " &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<b>Survey Number : </b>" + surveyno + " </td></tr></Table>";
        //                    var stuList = StaffEvaluationstdCount.First().Value.ToList();
        //                    base.ExptToXL_AssessPointChart(stuList, title, (item => new
        //                    {
        //                        EvaluationDate = item.EvaluationDate.ToString("dd/MM/yyyy"),
        //                        item.StaffName,
        //                        item.Subject,
        //                        NoofStudentAttend = item.StudentCount.ToString(),
        //                        NoofQuestionAsked = item.QuestionCount.ToString(),
        //                        //Weightage = item.Score
        //                        AverageScore = item.Average,
        //                    }), headerTable);
        //                }
        //                else
        //                {
        //                    long totalrecords = StaffEvaluationstdCount.First().Key;
        //                    int totalpages = (int)Math.Ceiling(totalrecords / (float)rows);
        //                    var jsondat = new
        //                    {
        //                        total = totalpages,
        //                        page = page,
        //                        records = totalrecords,
        //                        rows = (from items in StaffEvaluationstdCount.FirstOrDefault().Value
        //                                select new
        //                                {
        //                                    cell = new string[]{
        //                                  items.Id.ToString(),
        //                                  items.AcademicYear, 
        //                                  items.Campus,
        //                                  items.Grade,
        //                                  items.Section,
        //                                  items.CategoryName,
        //                                  items.EvaluationDate.ToString("dd/MM/yyyy"),
        //                                  items.StaffName+"("+items.Subject+")",
        //                                  items.StudentCount.ToString(),
        //                                  items.QuestionCount.ToString(),
        //                                  items.Score.ToString(),
        //                                  //"<a style='text-decoration: none;border-bottom: 1px solid blue;' onclick=\"ShowWeightageMarkwithQuestionSec('"+items.Id+"','" + items.AcademicYear+"','"+items.Campus+ "','" +items.Grade+"','"+items.Section+"','"+ items.StaffPreRegNumber + "','" + items.StaffEvaluationCategoryId + "');\" '>"+items.Score+"</a>",
        //                                  //items.Average.ToString()
        //                                  "<a style='text-decoration: none;cursor: pointer;border-bottom: 1px solid blue;'  title='' onclick=\"ShowWeightageMarkwithQuestionSec('"+items.Id+"','" + items.AcademicYear+"','"+items.Campus+ "','" +items.Grade+"','"+items.Section+"','"+ items.StaffPreRegNumber + "','" + items.StaffEvaluationCategoryId + "');\" '>"+items.Average+"</a>"
        //                              }
        //                                })
        //                    };
        //                    return Json(jsondat, JsonRequestBehavior.AllowGet);
        //                }
        //            }
        //        }
        //        return Json(new { rows = (new { cell = new string[] { } }) }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
        //        throw ex;
        //    }
        //}

        //public ActionResult StaffWiseStudentSurveyReportJqgrid(string Campus, string AcaYear, long? StaffPreRegNum, string Subject, string surveyNo, string ExportType, int rows, string sidx, string sord, int? page = 1)
        //{
        //    try
        //    {
        //        StaffManagementService sms = new StaffManagementService();
        //        Dictionary<string, object> criteria = new Dictionary<string, object>();
        //        Dictionary<string, object> likeCriteria = new Dictionary<string, object>();
        //        if (!string.IsNullOrEmpty(Campus))
        //            criteria.Add("Campus", Campus);
        //        if (!string.IsNullOrEmpty(AcaYear))
        //            criteria.Add("AcademicYear", AcaYear);
        //        if (StaffPreRegNum > 0)
        //            criteria.Add("StaffPreRegNumber", StaffPreRegNum);
        //        if (!string.IsNullOrEmpty(Subject))
        //            criteria.Add("Subject", Subject);
        //        if (!string.IsNullOrEmpty(surveyNo))
        //        {
        //            criteria.Add("CategoryName", surveyNo);
        //            Dictionary<long, IList<StaffWiseStudentSurveyNewResult_Vw>> StaffstudentsurveyCount = sms.GetStaffWiseStudentSurverList(page - 1, rows, sidx, sord, criteria, likeCriteria);
        //            if (StaffstudentsurveyCount != null && StaffstudentsurveyCount.Count > 0)
        //            {
        //                if (ExportType == "Excel")
        //                {
        //                    string campus = string.IsNullOrEmpty(Campus) ? "All" : Campus;
        //                    string surveyno = string.IsNullOrEmpty(surveyNo) ? "All" : surveyNo;
        //                    string title = "StaffWiseSutdentSurveyReport";
        //                    string logopath = ConfigurationManager.AppSettings["AppLogos"].ToString() + "tips.gif";
        //                    string headerTable = @"<Table><tr><td colspan='5' align='center' style='font-size: large;'><img src=" + logopath + "><b> The Indian Public School,Coimbatore</b></td></tr><tr><td colspan='5' align='center' style='font-size: large;'><b>Staff Wise Student Survey Result-" + AcaYear + "</b></td></tr><tr>";
        //                    headerTable = headerTable + "<td colspan='5' align='center' style='font-size: medium;'><b>Campus</b> : " + campus + "<b>Survey Number : </b>" + surveyno + " </td></tr></Table>";
        //                    var stuList = StaffstudentsurveyCount.First().Value.ToList();
        //                    base.ExptToXL_AssessPointChart(stuList, title, (item => new
        //                    {
        //                        item.StaffName,
        //                        item.Subject,
        //                        NoofStudentAttend = item.StudentCount.ToString(),
        //                        NoofQuestioninSurvey = item.QuestionCount.ToString(),
        //                        //Weightage = item.Score
        //                        AverageScore = item.Average,
        //                    }), headerTable);
        //                }
        //                else
        //                {
        //                    long totalrecords = StaffstudentsurveyCount.First().Key;
        //                    int totalpages = (int)Math.Ceiling(totalrecords / (float)rows);
        //                    var jsondat = new
        //                    {
        //                        total = totalpages,
        //                        page = page,
        //                        records = totalrecords,
        //                        rows = (from items in StaffstudentsurveyCount.FirstOrDefault().Value
        //                                select new
        //                                {
        //                                    cell = new string[]{
        //                                  items.Id.ToString(),
        //                                  items.AcademicYear, 
        //                                  items.Campus,
        //                                  items.StaffName,
        //                                  items.StaffPreRegNumber.ToString(),
        //                                  items.Subject,
        //                                  items.CategoryName,
        //                                  items.StudentCount.ToString(),
        //                                  items.QuestionCount.ToString(),
        //                                  items.Score.ToString(),
        //                                  items.Average.ToString(),
        //                              }
        //                                })
        //                    };
        //                    return Json(jsondat, JsonRequestBehavior.AllowGet);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            Dictionary<long, IList<StaffWiseStudentSurveyNewResultWOS_Vw>> StaffstdsurveyCount = sms.GetStaffWiseStudentSurverWOSList(page - 1, rows, sidx, sord, criteria, likeCriteria);
        //            if (StaffstdsurveyCount != null && StaffstdsurveyCount.Count > 0)
        //            {
        //                if (ExportType == "Excel")
        //                {
        //                    string campus = string.IsNullOrEmpty(Campus) ? "All" : Campus;
        //                    string surveyno = string.IsNullOrEmpty(surveyNo) ? "All" : surveyNo;
        //                    string title = "StaffWiseSutdentSurveyReport";
        //                    string logopath = ConfigurationManager.AppSettings["AppLogos"].ToString() + "tips.gif";
        //                    string headerTable = @"<Table><tr><td colspan='5' align='center' style='font-size: large;'><img src=" + logopath + "><b> The Indian Public School,Coimbatore</b></td></tr><tr><td colspan='5' align='center' style='font-size: large;'><b>Staff Wise Student Survey Result-" + AcaYear + "</b></td></tr><tr>";
        //                    headerTable = headerTable + "<td colspan='5' align='center' style='font-size: medium;'><b>Campus</b> : " + campus + "<b>Survey Number : </b>" + surveyno + " </td></tr></Table>";
        //                    var stuList = StaffstdsurveyCount.First().Value.ToList();
        //                    base.ExptToXL_AssessPointChart(stuList, title, (item => new
        //                    {
        //                        item.StaffName,
        //                        item.Subject,
        //                        NoofStudentAttend = item.StudentCount.ToString(),
        //                        NoofQuestioninSurvey = item.QuestionCount.ToString(),
        //                        //Weightage = item.Score
        //                        AverageScore = item.Average,
        //                    }), headerTable);
        //                }
        //                else
        //                {
        //                    long totalrecords = StaffstdsurveyCount.First().Key;
        //                    int totalpages = (int)Math.Ceiling(totalrecords / (float)rows);
        //                    var jsondat = new
        //                    {
        //                        total = totalpages,
        //                        page = page,
        //                        records = totalrecords,
        //                        rows = (from items in StaffstdsurveyCount.FirstOrDefault().Value
        //                                select new
        //                                {
        //                                    cell = new string[]{
        //                                  items.Id.ToString(),
        //                                  items.AcademicYear, 
        //                                  items.Campus,
        //                                  items.StaffName,
        //                                  items.StaffPreRegNumber.ToString(),
        //                                  items.Subject,
        //                                  items.CategoryName,
        //                                  items.StudentCount.ToString(),
        //                                  items.QuestionCount.ToString(),
        //                                  items.Score.ToString(),
        //                                  items.Average.ToString(),
        //                              }
        //                                })
        //                    };
        //                    return Json(jsondat, JsonRequestBehavior.AllowGet);
        //                }
        //            }
        //        }
        //        return Json(new { rows = (new { cell = new string[] { } }) }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
        //        throw ex;
        //    }
        //}

        //public PartialViewResult ShowQuestionMarksWithSection(long? RowId)
        //{
        //    StudentSurveyReportNew_Vw ssqr = smsObj.GetStudentSurveyReportById(RowId ?? 0);
        //    var viewModel = new StaffwiseSurveyQuestionViewModel()
        //    {
        //        AcademicYear = ssqr.AcademicYear,
        //        Campus = ssqr.Campus,
        //        Grade = ssqr.Grade,
        //        Section = ssqr.Section,
        //        Subject = ssqr.Subject,
        //        StaffName = ssqr.StaffName,
        //        StaffPreRegNumber = ssqr.StaffPreRegNumber,
        //        StaffEvaluationCategoryId = ssqr.StaffEvaluationCategoryId,
        //        StudentCount = ssqr.StudentCount
        //    };
        //    return PartialView(viewModel);
        //}

        ////public PartialViewResult ShowQuestionMarksWithoutSection(long? RowId)
        ////{
        ////    StudentSurveyReportWOSecNew_Vw ssqr = smsObj.GetStudentSurveyReportWOSecById(RowId ?? 0);
        ////    var viewModel = new StaffwiseSurveyQuestionViewModel()
        ////    {
        ////        AcademicYear = ssqr.AcademicYear,
        ////        Campus = ssqr.Campus,
        ////        Grade = ssqr.Grade,
        ////        Subject = ssqr.Subject,
        ////        StaffName = ssqr.StaffName,
        ////        StaffPreRegNumber = ssqr.StaffPreRegNumber,
        ////        StudentSurveyQuestionId = ssqr.StaffEvaluationCategoryId,
        ////        Section = "All",
        ////        StudentCount = ssqr.StudentCount
        ////    };
        ////    return PartialView(viewModel);
        ////}
        //public PartialViewResult ShowQuestionMarksInStaff(long? RowId)
        //{
        //    StudentSurveyReportNew_Vw ssqr = smsObj.GetStudentSurveyReportById(RowId ?? 0);
        //    var viewModel = new StaffwiseSurveyQuestionViewModel()
        //    {
        //        AcademicYear = ssqr.AcademicYear,
        //        Campus = ssqr.Campus,
        //        Grade = ssqr.Grade,
        //        Subject = ssqr.Subject,
        //        StaffName = ssqr.StaffName,
        //        StaffPreRegNumber = ssqr.StaffPreRegNumber,
        //        StaffEvaluationCategoryId = ssqr.StaffEvaluationCategoryId,
        //        Section = ssqr.Section,
        //        StudentCount = ssqr.StudentCount
        //    };
        //    return PartialView(viewModel);
        //}

        //public ActionResult ShowQuestionMarksListJqGrid(string acayear, string cam, string gra, string sect, long? preRegNum, long? SurveyId, int rows, string sidx, string sord, int? page = 1, int? ExptXl = 0)
        //{
        //    try
        //    {
        //        StaffManagementService sms = new StaffManagementService();
        //        Dictionary<string, object> criteria = new Dictionary<string, object>();
        //        Dictionary<string, object> likeCriteria = new Dictionary<string, object>();
        //        if (!string.IsNullOrEmpty(cam))
        //            criteria.Add("Campus", cam);
        //        if (!string.IsNullOrEmpty(gra))
        //            criteria.Add("Grade", gra);
        //        if (!string.IsNullOrEmpty(acayear))
        //            criteria.Add("AcademicYear", acayear);
        //        if (SurveyId > 0)
        //            criteria.Add("StaffEvaluationCategoryId", SurveyId);
        //        if (preRegNum > 0)
        //            criteria.Add("StaffPreRegNumber", preRegNum);
        //        sord = sord == "desc" ? "Desc" : "Asc";
        //        if (!string.IsNullOrEmpty(sect) && sect == "All")
        //        {
        //            Dictionary<long, IList<StaffwiseSurveyQuestionReportWOSec_Vw>> studentSureyMarksecList = sms.GetStudentSurveyQuestionMarkListWithoutSection(page - 1, rows, sidx, sord, criteria, likeCriteria);
        //            if (studentSureyMarksecList != null && studentSureyMarksecList.Count > 0)
        //            {
        //                if (ExptXl == 1)
        //                {
        //                    string title = "SutdentSurveyQuestionwiseReport";
        //                    string logopath = ConfigurationManager.AppSettings["AppLogos"].ToString() + "tips.gif";
        //                    string headerTable = @"<Table><tr><td colspan='7' align='center' style='font-size: large;'><img src=" + logopath + "><b> The Indian Public School,Coimbatore</b></td></tr><tr><td colspan='7' align='center' style='font-size: large;'><b>Student Survey Result-" + acayear + "</b></td></tr><tr>";
        //                    headerTable = headerTable + "<td colspan='7' align='center' style='font-size: medium;'><b>Campus</b> : " + cam + " &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp <b>Grade : </b> " + gra + " &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<b>Grade : </b> " + gra + "</td></tr></Table>";
        //                    var stuList = studentSureyMarksecList.First().Value.ToList();
        //                    base.ExptToXL_AssessPointChart(stuList, title, (item => new
        //                    {
        //                        EvaluationDate = item.EvaluationDate.ToString("dd/MM/yyyy"),
        //                        item.StaffName,
        //                        item.Subject,
        //                        NoofStudentAttend = item.StudentCount.ToString(),
        //                        QuestionAsked = item.StudentSurveyQuestion.ToString(),
        //                        ScoreinSurvey = item.Score,
        //                        AverageScore = item.Average,
        //                    }), headerTable);
        //                }
        //                else
        //                {
        //                    long totalrecords = studentSureyMarksecList.First().Key;
        //                    int totalpages = (int)Math.Ceiling(totalrecords / (float)rows);
        //                    var jsondat = new
        //                    {
        //                        total = totalpages,
        //                        page = page,
        //                        records = totalrecords,
        //                        userdata = new
        //                        {
        //                            StudentSurveyQuestion = "Total:",
        //                            Score = studentSureyMarksecList.FirstOrDefault().Value.Sum(x => x.Score).ToString(),
        //                            Average = studentSureyMarksecList.FirstOrDefault().Value.Sum(x => x.Average).ToString()
        //                        },
        //                        rows = (from items in studentSureyMarksecList.FirstOrDefault().Value
        //                                select new
        //                                {
        //                                    cell = new string[]{
        //                                  items.Id.ToString(),
        //                                  items.AcademicYear, 
        //                                  items.Campus,
        //                                  items.Grade,
        //                                  items.CategoryName,
        //                                  items.EvaluationDate.ToString("dd/MM/yyyy"),
        //                                  items.StaffName+"("+items.Subject+")",
        //                                  items.StudentCount.ToString(),
        //                                  items.StudentSurveyQuestion,
        //                                  items.Score.ToString(),
        //                                  items.Average.ToString()
        //                              }
        //                                })
        //                    };
        //                    return Json(jsondat, JsonRequestBehavior.AllowGet);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            criteria.Add("Section", sect);
        //            Dictionary<long, IList<StaffwiseSurveyQuestionReport_Vw>> studentSureyMarkList = sms.GetStudentSurveyQuestionMarkList(page - 1, rows, sidx, sord, criteria, likeCriteria);
        //            if (studentSureyMarkList != null && studentSureyMarkList.Count > 0)
        //            {
        //                if (ExptXl > 0)
        //                {
        //                    string title = "SutdentSurveyQuestionwiseReport";
        //                    string logopath = ConfigurationManager.AppSettings["AppLogos"].ToString() + "tips.gif";
        //                    string headerTable = @"<Table><tr><td colspan='7' align='center' style='font-size: large;'><img src=" + logopath + "><b> The Indian Public School,Coimbatore</b></td></tr><tr><td colspan='7' align='center' style='font-size: large;'><b>Student Survey Result - " + acayear + "</b></td></tr><tr>";
        //                    headerTable = headerTable + "<td colspan='7' align='center' style='font-size: medium;'><b>Campus</b> : " + cam + " &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<b>Grade : </b> " + gra + " &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<b>Survey Number : </b></td></tr></Table>";
        //                    var stuList = studentSureyMarkList.First().Value.ToList();
        //                    base.ExptToXL_AssessPointChart(stuList, title, (item => new
        //                    {
        //                        EvaluationDate = item.EvaluationDate.ToString("dd/MM/yyyy"),
        //                        item.StaffName,
        //                        item.Subject,
        //                        NoofStudentAttend = item.StudentCount.ToString(),
        //                        QuestionAsked = item.StudentSurveyQuestion.ToString(),
        //                        ScoreinSurvey = item.Score,
        //                        AverageScore = item.Average
        //                    }), headerTable);
        //                }
        //                else
        //                {
        //                    long totalrecords = studentSureyMarkList.First().Key;
        //                    int totalpages = (int)Math.Ceiling(totalrecords / (float)rows);
        //                    var jsondat = new
        //                    {
        //                        total = totalpages,
        //                        page = page,
        //                        records = totalrecords,
        //                        userdata = new
        //                        {
        //                            StudentSurveyQuestion = "Total:",
        //                            Score = studentSureyMarkList.FirstOrDefault().Value.Sum(x => x.Score).ToString(),
        //                            Average = studentSureyMarkList.FirstOrDefault().Value.Sum(x => x.Average).ToString()
        //                        },
        //                        rows = (from items in studentSureyMarkList.FirstOrDefault().Value
        //                                select new
        //                                {
        //                                    cell = new string[]{
        //                                  items.Id.ToString(),
        //                                  items.AcademicYear, 
        //                                  items.Campus,
        //                                  items.Grade,
        //                                  items.CategoryName,
        //                                  items.EvaluationDate.ToString("dd/MM/yyyy"),
        //                                  items.StaffName+"("+items.Subject+")",
        //                                  items.StudentCount.ToString(),
        //                                  items.StudentSurveyQuestion,
        //                                  items.Score.ToString(),
        //                                  //"<a style='border-bottom: 1px solid blue;' onclick=\"ShowWeightageMarkwithQuestionSec('" + items.AcademicYear+"','"+items.Campus+ "','" +items.Grade+"','"+items.Section+"','"+ items.StaffPreRegNumber + "','" + items.StaffEvaluationCategoryId + "');\" '>"+items.Score+"</a>",
        //                                  items.Average.ToString()
        //                              }
        //                                })
        //                    };
        //                    return Json(jsondat, JsonRequestBehavior.AllowGet);
        //                }
        //            }
        //        }
        //        return Json(new { rows = (new { cell = new string[] { } }) }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
        //        throw ex;
        //    }
        //}

        //#endregion
        //#region StudentSurveyReport
        //public ActionResult StudentSurveyReport()
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //        else
        //        {
        //            MastersService ms = new MastersService();
        //            StaffManagementService SS = new StaffManagementService();
        //            Dictionary<string, object> criteria = new Dictionary<string, object>();
        //            //criteria.Add("IsActive", true);
        //            Dictionary<long, IList<StaffEvaluationCategoryMaster>> StaffEvaluationCategoryList = SS.GetStaffEvaluationCategoryListWithPagingAndCriteria(0, 100, string.Empty, string.Empty, criteria, criteria);
        //            ViewBag.campusddl = (from items in StaffEvaluationCategoryList.FirstOrDefault().Value
        //                                 where items.IsActive = true
        //                                 select new { Campus = items.Campus }).Distinct().ToList();
        //            ViewBag.campusallddl = (from items in StaffEvaluationCategoryList.FirstOrDefault().Value
        //                                    select new { Campus = items.Campus }).Distinct().ToList();
        //            criteria.Clear();
        //            Dictionary<long, IList<AcademicyrMaster>> AcademicyrMaster = ms.GetAcademicyrMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
        //            ViewBag.acadddl = AcademicyrMaster.First().Value;
        //            ViewBag.currentAcyear = DateTime.Now.Month > 5 ? DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Year + 1).ToString() : (DateTime.Now.Year - 1).ToString() + "-" + DateTime.Now.Year.ToString();
        //            #region BreadCrumb
        //            string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //            ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
        //            #endregion
        //            return View();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
        //        throw ex;
        //    }
        //}        
        //public ActionResult StudentSurveyStaffwiseReportJqgrid(long Id, string Survey, string ExportType, int rows, string sidx, string sord, int? page = 1)
        //{
        //    try
        //    {
        //        StaffManagementService sms = new StaffManagementService();
        //        string campus = string.Empty;
        //        string AcaYear = string.Empty;
        //        string surveyname = string.Empty;
        //        string StaffName = string.Empty;

        //        long preRegNum = 0;
        //        if (!string.IsNullOrEmpty(Survey))
        //        {
        //            StaffWiseStudentSurveyNewResult_Vw staffwise = sms.GetStaffWiseStudentSurveyNewResultById(Id);
        //            if (staffwise != null)
        //            {
        //                campus = staffwise.Campus;
        //                preRegNum = staffwise.StaffPreRegNumber;
        //            }
        //        }
        //        else
        //        {
        //            StaffWiseStudentSurveyNewResultWOS_Vw staffwisewos = sms.StaffWiseStudentSurveyNewResultWOSById(Id);
        //            if (staffwisewos != null)
        //            {
        //                campus = staffwisewos.Campus;
        //                preRegNum = staffwisewos.StaffPreRegNumber;
        //            }
        //        }

        //        Dictionary<string, object> criteria = new Dictionary<string, object>();
        //        Dictionary<string, object> likeCriteria = new Dictionary<string, object>();
        //        //criteria.Add("CategoryName", staffwise.CategoryName);
        //        criteria.Add("Campus", campus);
        //        criteria.Add("StaffPreRegNumber", preRegNum);
        //        Dictionary<long, IList<StudentSurveyReportNew_Vw>> StaffEvaluationstdCount = sms.GetStaffEvaluationStudentCountList(page - 1, rows, sidx, sord, criteria, likeCriteria);
        //        if (StaffEvaluationstdCount != null && StaffEvaluationstdCount.Count > 0)
        //        {
        //            if (ExportType == "Excel")
        //            {
        //                string title = "StaffStudentSurveyDetailsReprot";
        //                string logopath = ConfigurationManager.AppSettings["AppLogos"].ToString() + "tips.gif";
        //                string headerTable = @"<Table><tr><td colspan='7' align='center' style='font-size: large;'><img src=" + logopath + "><b> The Indian Public School,Coimbatore</b></td></tr><tr><td colspan='7' align='center' style='font-size: large;'><b>Student Survey Result - " + AcaYear + "</b></td></tr><tr>";
        //                headerTable = headerTable + "<td colspan='7' align='center' style='font-size: medium;'><b>Campus</b> : " + campus + " &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<b>Grade : </b> All &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<b>Survey Number : </b>" + surveyname + "<b>Staff Name : </b>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp" + StaffName + " &nbsp&nbsp&nbsp&nbsp&nbsp </td></tr></Table>";
        //                var stuList = StaffEvaluationstdCount.First().Value.ToList();
        //                base.ExptToXL_AssessPointChart(stuList, title, (item => new
        //                {
        //                    EvaluationDate = item.EvaluationDate.ToString("dd/MM/yyyy"),
        //                    item.Grade,
        //                    item.Section,
        //                    item.Subject,
        //                    NoofStudentAttend = item.StudentCount.ToString(),
        //                    NoofQuestioninSurvey = item.QuestionCount.ToString(),
        //                    //Weightage = item.Score
        //                    AverageScore = item.Average
        //                }), headerTable);
        //            }
        //            else
        //            {
        //                long totalrecords = StaffEvaluationstdCount.First().Key;
        //                int totalpages = (int)Math.Ceiling(totalrecords / (float)rows);
        //                var jsondat = new
        //                {
        //                    total = totalpages,
        //                    page = page,
        //                    records = totalrecords,
        //                    rows = (from items in StaffEvaluationstdCount.FirstOrDefault().Value
        //                            select new
        //                            {
        //                                cell = new string[]{
        //                                  items.Id.ToString(),
        //                                  items.AcademicYear, 
        //                                  items.Campus,
        //                                  items.Grade,
        //                                  items.Section,
        //                                  items.CategoryName,
        //                                  items.EvaluationDate.ToString("dd/MM/yyyy"),
        //                                  items.StudentCount.ToString(),
        //                                  items.QuestionCount.ToString(),
        //                                  items.Score.ToString(),
        //                                  //items.Average.ToString(),
        //                                  "<a style='text-decoration: none;cursor: pointer;border-bottom: 1px solid blue;' title='' onclick=\"ShowQuestionWeightageMarkinStaff('"+items.Id+"','" + items.AcademicYear+"','"+items.Campus+ "','" +items.Grade+"','"+items.Section+"','"+ items.StaffPreRegNumber + "','" + items.StaffEvaluationCategoryId + "');\" '>"+items.Average+"</a>",
        //                              }
        //                            })
        //                };
        //                return Json(jsondat, JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //        return Json(new { rows = (new { cell = new string[] { } }) }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
        //        throw ex;
        //    }
        //}        
        //public ActionResult StudentSurveyReportAllJqgrid(string Campus, string Grade, string Section, string Surveyno, string AcademicYear, string ExportType, int rows, string sidx, string sord, int? page = 1)
        //{
        //    try
        //    {
        //        StaffManagementService sms = new StaffManagementService();
        //        Dictionary<string, object> criteria = new Dictionary<string, object>();
        //        Dictionary<string, object> likeCriteria = new Dictionary<string, object>();
        //        if (!string.IsNullOrEmpty(Campus))
        //            criteria.Add("Campus", Campus);
        //        if (!string.IsNullOrEmpty(Grade))
        //            criteria.Add("Grade", Grade);
        //        if (!string.IsNullOrEmpty(AcademicYear))
        //            criteria.Add("AcademicYear", AcademicYear);
        //        if (!string.IsNullOrEmpty(Surveyno))
        //            criteria.Add("CategoryName", Surveyno);
        //        if (!string.IsNullOrEmpty(Section) && Section == "ALL")
        //        {
        //            Dictionary<long, IList<StudentSurveyReportWOSecNew_Vw>> studentSureyReportList = sms.GetStaffEvaluationStudentCountListWithoutSection(page - 1, rows, sidx, sord, criteria, likeCriteria);
        //            if (studentSureyReportList != null && studentSureyReportList.Count > 0)
        //            {
        //                if (ExportType == "Excel")
        //                {
        //                    string campus = string.IsNullOrEmpty(Campus) ? "All" : Campus;
        //                    string grade = string.IsNullOrEmpty(Grade) ? "All" : Grade;
        //                    string surveyno = string.IsNullOrEmpty(Surveyno) ? "All" : Surveyno;
        //                    string title = "SutdentSurveyReport";
        //                    string logopath = ConfigurationManager.AppSettings["AppLogos"].ToString() + "tips.gif";
        //                    string headerTable = @"<Table><tr><td colspan='6' align='center' style='font-size: large;'><img src=" + logopath + "><b> The Indian Public School,Coimbatore</b></td></tr><tr><td colspan='6' align='center' style='font-size: large;'><b>Student Survey Result-" + AcademicYear + "</b></td></tr><tr>";
        //                    headerTable = headerTable + "<td colspan='6' align='center' style='font-size: medium;'><b>Campus</b> : " + campus + " &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp <b>Grade : </b> " + grade + " &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<b>Survey Number : </b>" + surveyno + " </td></tr></Table>";
        //                    var stuList = studentSureyReportList.First().Value.ToList();
        //                    base.ExptToXL_AssessPointChart(stuList, title, (item => new
        //                    {
        //                        EvaluationDate = item.EvaluationDate.ToString("dd/MM/yyyy"),
        //                        item.StaffName,
        //                        item.Subject,
        //                        NoofStudentAttend = item.StudentCount.ToString(),
        //                        NoofQuestionAsked = item.QuestionCount.ToString(),
        //                        //Weightage = item.Score,
        //                        AverageScore = item.Average,
        //                    }), headerTable);
        //                }
        //                else
        //                {
        //                    long totalrecords = studentSureyReportList.First().Key;
        //                    int totalpages = (int)Math.Ceiling(totalrecords / (float)rows);
        //                    var jsondat = new
        //                    {
        //                        total = totalpages,
        //                        page = page,
        //                        records = totalrecords,
        //                        rows = (from items in studentSureyReportList.FirstOrDefault().Value
        //                                select new
        //                                {
        //                                    cell = new string[]{
        //                                  items.Id.ToString(),
        //                                  items.AcademicYear, 
        //                                  items.Campus,
        //                                  items.Grade,
        //                                  items.CategoryName,
        //                                  items.EvaluationDate.ToString("dd/MM/yyyy"),
        //                                  items.StaffName+"("+items.Subject+")",
        //                                  items.StudentCount.ToString(),
        //                                  items.QuestionCount.ToString(),
        //                                  items.Score.ToString(),
        //                                  //"<a style='text-decoration: none;border-bottom: 1px solid blue;' onclick=\"ShowWeightageMarkswithQuestion('"+items.Id+"','" + items.AcademicYear+"','"+items.Campus+ "','" +items.Grade+"','"+ items.StaffPreRegNumber + "','" + items.StaffEvaluationCategoryId + "');\" '>"+items.Score+"</a>",
        //                                  //items.Average.ToString()
        //                                  "<a style='text-decoration: none;cursor: pointer;border-bottom: 1px solid blue;' title='' onclick=\"ShowWeightageMarkswithQuestion('"+items.Id+"','" + items.AcademicYear+"','"+items.Campus+ "','" +items.Grade+"','"+ items.StaffPreRegNumber + "','" + items.StaffEvaluationCategoryId + "');\" '>"+items.Average+"</a>",
        //                              }
        //                                })
        //                    };
        //                    return Json(jsondat, JsonRequestBehavior.AllowGet);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (!string.IsNullOrEmpty(Section))
        //                criteria.Add("Section", Section);
        //            Dictionary<long, IList<StudentSurveyReportNew_Vw>> StaffEvaluationstdCount = sms.GetStaffEvaluationStudentCountList(page - 1, rows, sidx, sord, criteria, likeCriteria);
        //            if (StaffEvaluationstdCount != null && StaffEvaluationstdCount.Count > 0)
        //            {
        //                if (ExportType == "Excel")
        //                {
        //                    string campus = String.IsNullOrEmpty(Campus) ? "All" : Campus;
        //                    string gradesec = Grade == null ? "All" : Grade;
        //                    gradesec = gradesec + "-" + Section;
        //                    string surveyno = String.IsNullOrEmpty(Surveyno) ? "All" : Surveyno;
        //                    string title = "SutdentSurveyReport";
        //                    string logopath = ConfigurationManager.AppSettings["AppLogos"].ToString() + "tips.gif";
        //                    string headerTable = @"<Table><tr><td colspan='6' align='center' style='font-size: large;'><img src=" + logopath + "><b> The Indian Public School,Coimbatore</b></td></tr><tr><td colspan='6' align='center' style='font-size: large;'><b>Student Survey Result - " + AcademicYear + "</b></td></tr><tr>";
        //                    headerTable = headerTable + "<td colspan='6' align='center' style='font-size: medium;'><b>Campus</b> : " + campus + " &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<b>Grade : </b> " + gradesec + " &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<b>Survey Number : </b>" + surveyno + " </td></tr></Table>";
        //                    var stuList = StaffEvaluationstdCount.First().Value.ToList();
        //                    base.ExptToXL_AssessPointChart(stuList, title, (item => new
        //                    {
        //                        EvaluationDate = item.EvaluationDate.ToString("dd/MM/yyyy"),
        //                        item.StaffName,
        //                        item.Subject,
        //                        NoofStudentAttend = item.StudentCount.ToString(),
        //                        NoofQuestionAsked = item.QuestionCount.ToString(),
        //                        //Weightage = item.Score
        //                        AverageScore = item.Average,
        //                    }), headerTable);
        //                }
        //                else
        //                {
        //                    long totalrecords = StaffEvaluationstdCount.First().Key;
        //                    int totalpages = (int)Math.Ceiling(totalrecords / (float)rows);
        //                    var jsondat = new
        //                    {
        //                        total = totalpages,
        //                        page = page,
        //                        records = totalrecords,
        //                        rows = (from items in StaffEvaluationstdCount.FirstOrDefault().Value
        //                                select new
        //                                {
        //                                    cell = new string[]{
        //                                  items.Id.ToString(),
        //                                  items.AcademicYear, 
        //                                  items.Campus,
        //                                  items.Grade,
        //                                  items.Section,
        //                                  items.CategoryName,
        //                                  items.EvaluationDate.ToString("dd/MM/yyyy"),
        //                                  items.StaffName+"("+items.Subject+")",
        //                                  items.StudentCount.ToString(),
        //                                  items.QuestionCount.ToString(),
        //                                  items.Score.ToString(),
        //                                  //"<a style='text-decoration: none;border-bottom: 1px solid blue;' onclick=\"ShowWeightageMarkwithQuestionSec('"+items.Id+"','" + items.AcademicYear+"','"+items.Campus+ "','" +items.Grade+"','"+items.Section+"','"+ items.StaffPreRegNumber + "','" + items.StaffEvaluationCategoryId + "');\" '>"+items.Score+"</a>",
        //                                  //items.Average.ToString()
        //                                  "<a style='text-decoration: none;cursor: pointer;border-bottom: 1px solid blue;'  title='' onclick=\"ShowWeightageMarkwithQuestionSec('"+items.Id+"','" + items.AcademicYear+"','"+items.Campus+ "','" +items.Grade+"','"+items.Section+"','"+ items.StaffPreRegNumber + "','" + items.StaffEvaluationCategoryId + "');\" '>"+items.Average+"</a>"
        //                              }
        //                                })
        //                    };
        //                    return Json(jsondat, JsonRequestBehavior.AllowGet);
        //                }
        //            }
        //        }
        //        return Json(new { rows = (new { cell = new string[] { } }) }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
        //        throw ex;
        //    }
        //}        
        //public ActionResult StaffWiseStudentSurveyReportJqgrid(string Campus, string AcaYear, long? StaffPreRegNum, string Subject, string surveyNo, string ExportType, int rows, string sidx, string sord, int? page = 1)
        //{
        //    try
        //    {
        //        StaffManagementService sms = new StaffManagementService();
        //        Dictionary<string, object> criteria = new Dictionary<string, object>();
        //        Dictionary<string, object> likeCriteria = new Dictionary<string, object>();
        //        if (!string.IsNullOrEmpty(Campus))
        //            criteria.Add("Campus", Campus);
        //        if (!string.IsNullOrEmpty(AcaYear))
        //            criteria.Add("AcademicYear", AcaYear);
        //        if (StaffPreRegNum > 0)
        //            criteria.Add("StaffPreRegNumber", StaffPreRegNum);
        //        if (!string.IsNullOrEmpty(Subject))
        //            criteria.Add("Subject", Subject);
        //        if (!string.IsNullOrEmpty(surveyNo))
        //        {
        //            criteria.Add("CategoryName", surveyNo);
        //            Dictionary<long, IList<StaffWiseStudentSurveyNewResult_Vw>> StaffstudentsurveyCount = sms.GetStaffWiseStudentSurverList(page - 1, rows, sidx, sord, criteria, likeCriteria);
        //            if (StaffstudentsurveyCount != null && StaffstudentsurveyCount.Count > 0)
        //            {
        //                if (ExportType == "Excel")
        //                {
        //                    string campus = string.IsNullOrEmpty(Campus) ? "All" : Campus;
        //                    string surveyno = string.IsNullOrEmpty(surveyNo) ? "All" : surveyNo;
        //                    string title = "StaffWiseSutdentSurveyReport";
        //                    string logopath = ConfigurationManager.AppSettings["AppLogos"].ToString() + "tips.gif";
        //                    string headerTable = @"<Table><tr><td colspan='5' align='center' style='font-size: large;'><img src=" + logopath + "><b> The Indian Public School,Coimbatore</b></td></tr><tr><td colspan='5' align='center' style='font-size: large;'><b>Staff Wise Student Survey Result-" + AcaYear + "</b></td></tr><tr>";
        //                    headerTable = headerTable + "<td colspan='5' align='center' style='font-size: medium;'><b>Campus</b> : " + campus + "<b>Survey Number : </b>" + surveyno + " </td></tr></Table>";
        //                    var stuList = StaffstudentsurveyCount.First().Value.ToList();
        //                    base.ExptToXL_AssessPointChart(stuList, title, (item => new
        //                    {
        //                        item.StaffName,
        //                        item.Subject,
        //                        NoofStudentAttend = item.StudentCount.ToString(),
        //                        NoofQuestioninSurvey = item.QuestionCount.ToString(),
        //                        //Weightage = item.Score
        //                        AverageScore = item.Average,
        //                    }), headerTable);
        //                }
        //                else
        //                {
        //                    long totalrecords = StaffstudentsurveyCount.First().Key;
        //                    int totalpages = (int)Math.Ceiling(totalrecords / (float)rows);
        //                    var jsondat = new
        //                    {
        //                        total = totalpages,
        //                        page = page,
        //                        records = totalrecords,
        //                        rows = (from items in StaffstudentsurveyCount.FirstOrDefault().Value
        //                                select new
        //                                {
        //                                    cell = new string[]{
        //                                  items.Id.ToString(),
        //                                  items.AcademicYear, 
        //                                  items.Campus,
        //                                  items.StaffName,
        //                                  items.StaffPreRegNumber.ToString(),
        //                                  items.Subject,
        //                                  items.CategoryName,
        //                                  items.StudentCount.ToString(),
        //                                  items.QuestionCount.ToString(),
        //                                  items.Score.ToString(),
        //                                  items.Average.ToString(),
        //                              }
        //                                })
        //                    };
        //                    return Json(jsondat, JsonRequestBehavior.AllowGet);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            Dictionary<long, IList<StaffWiseStudentSurveyNewResultWOS_Vw>> StaffstdsurveyCount = sms.GetStaffWiseStudentSurverWOSList(page - 1, rows, sidx, sord, criteria, likeCriteria);
        //            if (StaffstdsurveyCount != null && StaffstdsurveyCount.Count > 0)
        //            {
        //                if (ExportType == "Excel")
        //                {
        //                    string campus = string.IsNullOrEmpty(Campus) ? "All" : Campus;
        //                    string surveyno = string.IsNullOrEmpty(surveyNo) ? "All" : surveyNo;
        //                    string title = "StaffWiseSutdentSurveyReport";
        //                    string logopath = ConfigurationManager.AppSettings["AppLogos"].ToString() + "tips.gif";
        //                    string headerTable = @"<Table><tr><td colspan='5' align='center' style='font-size: large;'><img src=" + logopath + "><b> The Indian Public School,Coimbatore</b></td></tr><tr><td colspan='5' align='center' style='font-size: large;'><b>Staff Wise Student Survey Result-" + AcaYear + "</b></td></tr><tr>";
        //                    headerTable = headerTable + "<td colspan='5' align='center' style='font-size: medium;'><b>Campus</b> : " + campus + "<b>Survey Number : </b>" + surveyno + " </td></tr></Table>";
        //                    var stuList = StaffstdsurveyCount.First().Value.ToList();
        //                    base.ExptToXL_AssessPointChart(stuList, title, (item => new
        //                    {
        //                        item.StaffName,
        //                        item.Subject,
        //                        NoofStudentAttend = item.StudentCount.ToString(),
        //                        NoofQuestioninSurvey = item.QuestionCount.ToString(),
        //                        //Weightage = item.Score
        //                        AverageScore = item.Average,
        //                    }), headerTable);
        //                }
        //                else
        //                {
        //                    long totalrecords = StaffstdsurveyCount.First().Key;
        //                    int totalpages = (int)Math.Ceiling(totalrecords / (float)rows);
        //                    var jsondat = new
        //                    {
        //                        total = totalpages,
        //                        page = page,
        //                        records = totalrecords,
        //                        rows = (from items in StaffstdsurveyCount.FirstOrDefault().Value
        //                                select new
        //                                {
        //                                    cell = new string[]{
        //                                  items.Id.ToString(),
        //                                  items.AcademicYear, 
        //                                  items.Campus,
        //                                  items.StaffName,
        //                                  items.StaffPreRegNumber.ToString(),
        //                                  items.Subject,
        //                                  items.CategoryName,
        //                                  items.StudentCount.ToString(),
        //                                  items.QuestionCount.ToString(),
        //                                  items.Score.ToString(),
        //                                  items.Average.ToString(),
        //                              }
        //                                })
        //                    };
        //                    return Json(jsondat, JsonRequestBehavior.AllowGet);
        //                }
        //            }
        //        }
        //        return Json(new { rows = (new { cell = new string[] { } }) }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
        //        throw ex;
        //    }
        //}        
        //public PartialViewResult ShowQuestionMarksWithSection(long? RowId)
        //{
        //    SurveyReportNew_Vw ssqr = smsObj.GetSurveyReportById(RowId ?? 0);
        //    var viewModel = new StaffwiseSurveyQuestionViewModel()
        //    {
        //        AcademicYear = ssqr.AcademicYear,
        //        Campus = ssqr.Campus,
        //        Grade = ssqr.Grade,
        //        Section = ssqr.Section,
        //        Subject = ssqr.Subject,
        //        StaffName = ssqr.StaffName,
        //        StaffPreRegNumber = ssqr.StaffPreRegNumber,
        //        StaffEvaluationCategoryId = ssqr.StaffEvaluationCategoryId,
        //        StudentCount = ssqr.StudentCount
        //    };
        //    return PartialView(viewModel);
        //}
        ////public PartialViewResult ShowQuestionMarksWithoutSection(long? RowId)
        ////{
        ////    StudentSurveyReportWOSecNew_Vw ssqr = smsObj.GetStudentSurveyReportWOSecById(RowId ?? 0);
        ////    var viewModel = new StaffwiseSurveyQuestionViewModel()
        ////    {
        ////        AcademicYear = ssqr.AcademicYear,
        ////        Campus = ssqr.Campus,
        ////        Grade = ssqr.Grade,
        ////        Subject = ssqr.Subject,
        ////        StaffName = ssqr.StaffName,
        ////        StaffPreRegNumber = ssqr.StaffPreRegNumber,
        ////        StudentSurveyQuestionId = ssqr.StaffEvaluationCategoryId,
        ////        Section = "All",
        ////        StudentCount = ssqr.StudentCount
        ////    };
        ////    return PartialView(viewModel);
        ////}
        //public PartialViewResult ShowQuestionMarksInStaff(long? RowId)
        //{
        //    SurveyReportNew_Vw ssqr = smsObj.GetSurveyReportById(RowId ?? 0);
        //    var viewModel = new StaffwiseSurveyQuestionViewModel()
        //    {
        //        AcademicYear = ssqr.AcademicYear,
        //        Campus = ssqr.Campus,
        //        Grade = ssqr.Grade,
        //        Subject = ssqr.Subject,
        //        StaffName = ssqr.StaffName,
        //        StaffPreRegNumber = ssqr.StaffPreRegNumber,
        //        StaffEvaluationCategoryId = ssqr.StaffEvaluationCategoryId,
        //        Section = ssqr.Section,
        //        StudentCount = ssqr.StudentCount
        //    };
        //    return PartialView(viewModel);
        //}
        //public ActionResult ShowQuestionMarksListJqGrid(string acayear, string cam, string gra, string sect, long? preRegNum, long? SurveyId, int rows, string sidx, string sord, int? page = 1, int? ExptXl = 0)
        //{
        //    try
        //    {
        //        StaffManagementService sms = new StaffManagementService();
        //        Dictionary<string, object> criteria = new Dictionary<string, object>();
        //        Dictionary<string, object> likeCriteria = new Dictionary<string, object>();
        //        if (!string.IsNullOrEmpty(cam))
        //            criteria.Add("Campus", cam);
        //        if (!string.IsNullOrEmpty(gra))
        //            criteria.Add("Grade", gra);
        //        if (!string.IsNullOrEmpty(acayear))
        //            criteria.Add("AcademicYear", acayear);
        //        if (SurveyId > 0)
        //            criteria.Add("StaffEvaluationCategoryId", SurveyId);
        //        if (preRegNum > 0)
        //            criteria.Add("StaffPreRegNumber", preRegNum);
        //        sord = sord == "desc" ? "Desc" : "Asc";
        //        if (!string.IsNullOrEmpty(sect) && sect == "All")
        //        {
        //            Dictionary<long, IList<StaffwiseSurveyQuestionReportWOSec_Vw>> studentSureyMarksecList = sms.GetStudentSurveyQuestionMarkListWithoutSection(page - 1, rows, sidx, sord, criteria, likeCriteria);
        //            if (studentSureyMarksecList != null && studentSureyMarksecList.Count > 0)
        //            {
        //                if (ExptXl == 1)
        //                {
        //                    string title = "SutdentSurveyQuestionwiseReport";
        //                    string logopath = ConfigurationManager.AppSettings["AppLogos"].ToString() + "tips.gif";
        //                    string headerTable = @"<Table><tr><td colspan='7' align='center' style='font-size: large;'><img src=" + logopath + "><b> The Indian Public School,Coimbatore</b></td></tr><tr><td colspan='7' align='center' style='font-size: large;'><b>Student Survey Result-" + acayear + "</b></td></tr><tr>";
        //                    headerTable = headerTable + "<td colspan='7' align='center' style='font-size: medium;'><b>Campus</b> : " + cam + " &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp <b>Grade : </b> " + gra + " &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<b>Grade : </b> " + gra + "</td></tr></Table>";
        //                    var stuList = studentSureyMarksecList.First().Value.ToList();
        //                    base.ExptToXL_AssessPointChart(stuList, title, (item => new
        //                    {
        //                        EvaluationDate = item.EvaluationDate.ToString("dd/MM/yyyy"),
        //                        item.StaffName,
        //                        item.Subject,
        //                        NoofStudentAttend = item.StudentCount.ToString(),
        //                        QuestionAsked = item.StudentSurveyQuestion.ToString(),
        //                        ScoreinSurvey = item.Score,
        //                        AverageScore = item.Average,
        //                    }), headerTable);
        //                }
        //                else
        //                {
        //                    long totalrecords = studentSureyMarksecList.First().Key;
        //                    int totalpages = (int)Math.Ceiling(totalrecords / (float)rows);
        //                    var jsondat = new
        //                    {
        //                        total = totalpages,
        //                        page = page,
        //                        records = totalrecords,
        //                        userdata = new
        //                        {
        //                            StudentSurveyQuestion = "Total:",
        //                            Score = studentSureyMarksecList.FirstOrDefault().Value.Sum(x => x.Score).ToString(),
        //                            Average = studentSureyMarksecList.FirstOrDefault().Value.Sum(x => x.Average).ToString()
        //                        },
        //                        rows = (from items in studentSureyMarksecList.FirstOrDefault().Value
        //                                select new
        //                                {
        //                                    cell = new string[]{
        //                                  items.Id.ToString(),
        //                                  items.AcademicYear, 
        //                                  items.Campus,
        //                                  items.Grade,
        //                                  items.CategoryName,
        //                                  items.EvaluationDate.ToString("dd/MM/yyyy"),
        //                                  items.StaffName+"("+items.Subject+")",
        //                                  items.StudentCount.ToString(),
        //                                  items.StudentSurveyQuestion,
        //                                  items.Score.ToString(),
        //                                  items.Average.ToString()
        //                              }
        //                                })
        //                    };
        //                    return Json(jsondat, JsonRequestBehavior.AllowGet);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            criteria.Add("Section", sect);
        //            Dictionary<long, IList<StaffwiseSurveyQuestionReport_Vw>> studentSureyMarkList = sms.GetStudentSurveyQuestionMarkList(page - 1, rows, sidx, sord, criteria, likeCriteria);
        //            if (studentSureyMarkList != null && studentSureyMarkList.Count > 0)
        //            {
        //                if (ExptXl > 0)
        //                {
        //                    string title = "SutdentSurveyQuestionwiseReport";
        //                    string logopath = ConfigurationManager.AppSettings["AppLogos"].ToString() + "tips.gif";
        //                    string headerTable = @"<Table><tr><td colspan='7' align='center' style='font-size: large;'><img src=" + logopath + "><b> The Indian Public School,Coimbatore</b></td></tr><tr><td colspan='7' align='center' style='font-size: large;'><b>Student Survey Result - " + acayear + "</b></td></tr><tr>";
        //                    headerTable = headerTable + "<td colspan='7' align='center' style='font-size: medium;'><b>Campus</b> : " + cam + " &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<b>Grade : </b> " + gra + " &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<b>Survey Number : </b></td></tr></Table>";
        //                    var stuList = studentSureyMarkList.First().Value.ToList();
        //                    base.ExptToXL_AssessPointChart(stuList, title, (item => new
        //                    {
        //                        EvaluationDate = item.EvaluationDate.ToString("dd/MM/yyyy"),
        //                        item.StaffName,
        //                        item.Subject,
        //                        NoofStudentAttend = item.StudentCount.ToString(),
        //                        QuestionAsked = item.StudentSurveyQuestion.ToString(),
        //                        ScoreinSurvey = item.Score,
        //                        AverageScore = item.Average
        //                    }), headerTable);
        //                }
        //                else
        //                {
        //                    long totalrecords = studentSureyMarkList.First().Key;
        //                    int totalpages = (int)Math.Ceiling(totalrecords / (float)rows);
        //                    var jsondat = new
        //                    {
        //                        total = totalpages,
        //                        page = page,
        //                        records = totalrecords,
        //                        userdata = new
        //                        {
        //                            StudentSurveyQuestion = "Total:",
        //                            Score = studentSureyMarkList.FirstOrDefault().Value.Sum(x => x.Score).ToString(),
        //                            Average = studentSureyMarkList.FirstOrDefault().Value.Sum(x => x.Average).ToString()
        //                        },
        //                        rows = (from items in studentSureyMarkList.FirstOrDefault().Value
        //                                select new
        //                                {
        //                                    cell = new string[]{
        //                                  items.Id.ToString(),
        //                                  items.AcademicYear, 
        //                                  items.Campus,
        //                                  items.Grade,
        //                                  items.CategoryName,
        //                                  items.EvaluationDate.ToString("dd/MM/yyyy"),
        //                                  items.StaffName+"("+items.Subject+")",
        //                                  items.StudentCount.ToString(),
        //                                  items.StudentSurveyQuestion,
        //                                  items.Score.ToString(),
        //                                  //"<a style='border-bottom: 1px solid blue;' onclick=\"ShowWeightageMarkwithQuestionSec('" + items.AcademicYear+"','"+items.Campus+ "','" +items.Grade+"','"+items.Section+"','"+ items.StaffPreRegNumber + "','" + items.StaffEvaluationCategoryId + "');\" '>"+items.Score+"</a>",
        //                                  items.Average.ToString()
        //                              }
        //                                })
        //                    };
        //                    return Json(jsondat, JsonRequestBehavior.AllowGet);
        //                }
        //            }
        //        }
        //        return Json(new { rows = (new { cell = new string[] { } }) }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
        //        throw ex;
        //    }
        //}        
        //#endregion
        #region StudentSurveyReport

        public ActionResult StudentSurveyReport()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MastersService ms = new MastersService();
                    StaffManagementService SS = new StaffManagementService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    //criteria.Add("IsActive", true);
                    Dictionary<long, IList<StaffEvaluationCategoryMaster>> StaffEvaluationCategoryList = SS.GetStaffEvaluationCategoryListWithPagingAndCriteria(0, 100, string.Empty, string.Empty, criteria, criteria);
                    ViewBag.campusddl = (from items in StaffEvaluationCategoryList.FirstOrDefault().Value
                                         where items.IsActive = true
                                         select new { Campus = items.Campus }).Distinct().ToList();
                    ViewBag.campusallddl = (from items in StaffEvaluationCategoryList.FirstOrDefault().Value
                                            select new { Campus = items.Campus }).Distinct().ToList();
                    criteria.Clear();
                    Dictionary<long, IList<AcademicyrMaster>> AcademicyrMaster = ms.GetAcademicyrMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                    ViewBag.acadddl = AcademicyrMaster.First().Value;
                    ViewBag.currentAcyear = DateTime.Now.Month > 5 ? DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Year + 1).ToString() : (DateTime.Now.Year - 1).ToString() + "-" + DateTime.Now.Year.ToString();
                    #region BreadCrumb
                    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                    #endregion
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult StudentSurveyStaffwiseReportJqgrid(long Id, string Survey, string ExportType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();
                string campus = string.Empty;
                string AcaYear = string.Empty;
                string surveyname = string.Empty;
                string StaffName = string.Empty;

                long preRegNum = 0;
                if (!string.IsNullOrEmpty(Survey))
                {
                    StaffWiseStudentSurveyNewResult_Vw staffwise = sms.GetStaffWiseStudentSurveyNewResultById(Id);
                    if (staffwise != null)
                    {
                        campus = staffwise.Campus;
                        preRegNum = staffwise.StaffPreRegNumber;
                    }
                }
                else
                {
                    StaffWiseStudentSurveyNewResultWOS_Vw staffwisewos = sms.StaffWiseStudentSurveyNewResultWOSById(Id);
                    if (staffwisewos != null)
                    {
                        campus = staffwisewos.Campus;
                        preRegNum = staffwisewos.StaffPreRegNumber;
                    }
                }

                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<string, object> likeCriteria = new Dictionary<string, object>();
                //criteria.Add("CategoryName", staffwise.CategoryName);
                criteria.Add("Campus", campus);
                criteria.Add("StaffPreRegNumber", preRegNum);
                sord = sord == "desc" ? "Desc" : "Asc";
                Dictionary<long, IList<StudentSurveyReportNew_Vw>> StaffEvaluationstdCount = sms.GetStaffEvaluationStudentCountList(page - 1, rows, sidx, sord, criteria, likeCriteria);
                if (StaffEvaluationstdCount != null && StaffEvaluationstdCount.Count > 0)
                {
                    if (ExportType == "Excel")
                    {
                        string title = "StaffStudentSurveyDetailsReprot";
                        string logopath = ConfigurationManager.AppSettings["AppLogos"].ToString() + "tips.gif";
                        string headerTable = @"<Table><tr><td colspan='7' align='center' style='font-size: large;'><img src=" + logopath + "><b> The Indian Public School,Coimbatore</b></td></tr><tr><td colspan='7' align='center' style='font-size: large;'><b>Student Survey Result - " + AcaYear + "</b></td></tr><tr>";
                        headerTable = headerTable + "<td colspan='7' align='center' style='font-size: medium;'><b>Campus</b> : " + campus + " &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<b>Grade : </b> All &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<b>Survey Number : </b>" + surveyname + "<b>Staff Name : </b>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp" + StaffName + " &nbsp&nbsp&nbsp&nbsp&nbsp </td></tr></Table>";
                        var stuList = StaffEvaluationstdCount.First().Value.ToList();
                        base.ExptToXL_AssessPointChart(stuList, title, (item => new
                        {
                            EvaluationDate = item.EvaluationDate.ToString("dd/MM/yyyy"),
                            item.Grade,
                            item.Section,
                            item.Subject,
                            NoofStudentAttend = item.StudentCount.ToString(),
                            NoofQuestioninSurvey = item.QuestionCount.ToString(),
                            //Weightage = item.Score
                            AverageScore = item.Average
                        }), headerTable);
                    }
                    else
                    {
                        long totalrecords = StaffEvaluationstdCount.First().Key;
                        int totalpages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat = new
                        {
                            total = totalpages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in StaffEvaluationstdCount.FirstOrDefault().Value
                                    select new
                                    {
                                        cell = new string[]{
                                          items.Id.ToString(),
                                          items.AcademicYear, 
                                          items.Campus,
                                          items.Grade,
                                          items.Section,
                                          items.CategoryName,
                                          items.EvaluationDate.ToString("dd/MM/yyyy"),
                                          items.StudentCount.ToString(),
                                          items.QuestionCount.ToString(),
                                          items.Score.ToString(),
                                          //items.Average.ToString(),
                                          "<a style='text-decoration: none;cursor: pointer;border-bottom: 1px solid blue;' title='' onclick=\"ShowQuestionWeightageMarkinStaff('"+items.Id+"','" + items.AcademicYear+"','"+items.Campus+ "','" +items.Grade+"','"+items.Section+"','"+ items.StaffPreRegNumber + "','" + items.StaffEvaluationCategoryId + "');\" '>"+items.Average+"</a>",
                                      }
                                    })
                        };
                        return Json(jsondat, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { rows = (new { cell = new string[] { } }) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult StudentSurveyReportAllJqgrid(string Campus, string Grade, string Section, string Surveyno, string AcademicYear, string ExportType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<string, object> likeCriteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(Campus))
                    criteria.Add("Campus", Campus);
                if (!string.IsNullOrEmpty(Grade))
                    criteria.Add("Grade", Grade);
                if (!string.IsNullOrEmpty(AcademicYear))
                    criteria.Add("AcademicYear", AcademicYear);
                if (!string.IsNullOrEmpty(Surveyno))
                    criteria.Add("CategoryName", Surveyno);
                sord = sord == "desc" ? "Desc" : "Asc";
                if (!string.IsNullOrEmpty(Section) && Section == "ALL")
                {
                    Dictionary<long, IList<StudentSurveyReportWOSecNew_Vw>> studentSureyReportList = sms.GetStaffEvaluationStudentCountListWithoutSection(page - 1, rows, sidx, sord, criteria, likeCriteria);
                    if (studentSureyReportList != null && studentSureyReportList.Count > 0)
                    {
                        if (ExportType == "Excel")
                        {
                            string campus = string.IsNullOrEmpty(Campus) ? "All" : Campus;
                            string grade = string.IsNullOrEmpty(Grade) ? "All" : Grade;
                            string surveyno = string.IsNullOrEmpty(Surveyno) ? "All" : Surveyno;
                            string title = "SutdentSurveyReport";
                            string logopath = ConfigurationManager.AppSettings["AppLogos"].ToString() + "tips.gif";
                            string headerTable = @"<Table><tr><td colspan='6' align='center' style='font-size: large;'><img src=" + logopath + "><b> The Indian Public School,Coimbatore</b></td></tr><tr><td colspan='6' align='center' style='font-size: large;'><b>Student Survey Result-" + AcademicYear + "</b></td></tr><tr>";
                            headerTable = headerTable + "<td colspan='6' align='center' style='font-size: medium;'><b>Campus</b> : " + campus + " &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp <b>Grade : </b> " + grade + " &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<b>Survey Number : </b>" + surveyno + " </td></tr></Table>";
                            var stuList = studentSureyReportList.First().Value.ToList();
                            base.ExptToXL_AssessPointChart(stuList, title, (item => new
                            {
                                EvaluationDate = item.EvaluationDate.ToString("dd/MM/yyyy"),
                                item.StaffName,
                                item.Subject,
                                NoofStudentAttend = item.StudentCount.ToString(),
                                NoofQuestionAsked = item.QuestionCount.ToString(),
                                //Weightage = item.Score,
                                AverageScore = item.Average,
                            }), headerTable);
                        }
                        else
                        {
                            long totalrecords = studentSureyReportList.First().Key;
                            int totalpages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var jsondat = new
                            {
                                total = totalpages,
                                page = page,
                                records = totalrecords,
                                rows = (from items in studentSureyReportList.FirstOrDefault().Value
                                        select new
                                        {
                                            cell = new string[]{
                                          items.Id.ToString(),
                                          items.AcademicYear, 
                                          items.Campus,
                                          items.Grade,
                                          items.CategoryName,
                                          items.EvaluationDate.ToString("dd/MM/yyyy"),
                                          items.StaffName+"("+items.Subject+")",
                                          items.StudentCount.ToString(),
                                          items.QuestionCount.ToString(),
                                          items.Score.ToString(),
                                          //"<a style='text-decoration: none;border-bottom: 1px solid blue;' onclick=\"ShowWeightageMarkswithQuestion('"+items.Id+"','" + items.AcademicYear+"','"+items.Campus+ "','" +items.Grade+"','"+ items.StaffPreRegNumber + "','" + items.StaffEvaluationCategoryId + "');\" '>"+items.Score+"</a>",
                                          //items.Average.ToString()
                                          "<a style='text-decoration: none;cursor: pointer;border-bottom: 1px solid blue;' title='' onclick=\"ShowWeightageMarkswithQuestion('"+items.Id+"','" + items.AcademicYear+"','"+items.Campus+ "','" +items.Grade+"','"+ items.StaffPreRegNumber + "','" + items.StaffEvaluationCategoryId + "');\" '>"+items.Average+"</a>",
                                      }
                                        })
                            };
                            return Json(jsondat, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(Section))
                        criteria.Add("Section", Section);
                    Dictionary<long, IList<StudentSurveyReportNew_Vw>> StaffEvaluationstdCount = sms.GetStaffEvaluationStudentCountList(page - 1, rows, sidx, sord, criteria, likeCriteria);
                    if (StaffEvaluationstdCount != null && StaffEvaluationstdCount.Count > 0)
                    {
                        if (ExportType == "Excel")
                        {
                            string campus = String.IsNullOrEmpty(Campus) ? "All" : Campus;
                            string gradesec = Grade == null ? "All" : Grade;
                            gradesec = gradesec + "-" + Section;
                            string surveyno = String.IsNullOrEmpty(Surveyno) ? "All" : Surveyno;
                            string title = "SutdentSurveyReport";
                            string logopath = ConfigurationManager.AppSettings["AppLogos"].ToString() + "tips.gif";
                            string headerTable = @"<Table><tr><td colspan='6' align='center' style='font-size: large;'><img src=" + logopath + "><b> The Indian Public School,Coimbatore</b></td></tr><tr><td colspan='6' align='center' style='font-size: large;'><b>Student Survey Result - " + AcademicYear + "</b></td></tr><tr>";
                            headerTable = headerTable + "<td colspan='6' align='center' style='font-size: medium;'><b>Campus</b> : " + campus + " &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<b>Grade : </b> " + gradesec + " &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<b>Survey Number : </b>" + surveyno + " </td></tr></Table>";
                            var stuList = StaffEvaluationstdCount.First().Value.ToList();
                            base.ExptToXL_AssessPointChart(stuList, title, (item => new
                            {
                                EvaluationDate = item.EvaluationDate.ToString("dd/MM/yyyy"),
                                item.StaffName,
                                item.Subject,
                                NoofStudentAttend = item.StudentCount.ToString(),
                                NoofQuestionAsked = item.QuestionCount.ToString(),
                                //Weightage = item.Score
                                AverageScore = item.Average,
                            }), headerTable);
                        }
                        else
                        {
                            long totalrecords = StaffEvaluationstdCount.First().Key;
                            int totalpages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var jsondat = new
                            {
                                total = totalpages,
                                page = page,
                                records = totalrecords,
                                rows = (from items in StaffEvaluationstdCount.FirstOrDefault().Value
                                        select new
                                        {
                                            cell = new string[]{
                                          items.Id.ToString(),
                                          items.AcademicYear, 
                                          items.Campus,
                                          items.Grade,
                                          items.Section,
                                          items.CategoryName,
                                          items.EvaluationDate.ToString("dd/MM/yyyy"),
                                          items.StaffName+"("+items.Subject+")",
                                          items.StudentCount.ToString(),
                                          items.QuestionCount.ToString(),
                                          items.Score.ToString(),
                                          //"<a style='text-decoration: none;border-bottom: 1px solid blue;' onclick=\"ShowWeightageMarkwithQuestionSec('"+items.Id+"','" + items.AcademicYear+"','"+items.Campus+ "','" +items.Grade+"','"+items.Section+"','"+ items.StaffPreRegNumber + "','" + items.StaffEvaluationCategoryId + "');\" '>"+items.Score+"</a>",
                                          //items.Average.ToString()
                                          "<a style='text-decoration: none;cursor: pointer;border-bottom: 1px solid blue;'  title='' onclick=\"ShowWeightageMarkwithQuestionSec('"+items.Id+"','" + items.AcademicYear+"','"+items.Campus+ "','" +items.Grade+"','"+items.Section+"','"+ items.StaffPreRegNumber + "','" + items.StaffEvaluationCategoryId + "');\" '>"+items.Average+"</a>"
                                      }
                                        })
                            };
                            return Json(jsondat, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                return Json(new { rows = (new { cell = new string[] { } }) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult StaffWiseStudentSurveyReportJqgrid(string Campus, string AcaYear, long? StaffPreRegNum, string Subject, string surveyNo, string ExportType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<string, object> likeCriteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(Campus))
                    criteria.Add("Campus", Campus);
                if (!string.IsNullOrEmpty(AcaYear))
                    criteria.Add("AcademicYear", AcaYear);
                if (StaffPreRegNum > 0)
                    criteria.Add("StaffPreRegNumber", StaffPreRegNum);
                if (!string.IsNullOrEmpty(Subject))
                    criteria.Add("Subject", Subject);
                sord = sord == "desc" ? "Desc" : "Asc";
                if (!string.IsNullOrEmpty(surveyNo))
                {
                    criteria.Add("CategoryName", surveyNo);
                    Dictionary<long, IList<StaffWiseStudentSurveyNewResult_Vw>> StaffstudentsurveyCount = sms.GetStaffWiseStudentSurverList(page - 1, rows, sidx, sord, criteria, likeCriteria);
                    if (StaffstudentsurveyCount != null && StaffstudentsurveyCount.Count > 0)
                    {
                        if (ExportType == "Excel")
                        {
                            string campus = string.IsNullOrEmpty(Campus) ? "All" : Campus;
                            string surveyno = string.IsNullOrEmpty(surveyNo) ? "All" : surveyNo;
                            string title = "StaffWiseSutdentSurveyReport";
                            string logopath = ConfigurationManager.AppSettings["AppLogos"].ToString() + "tips.gif";
                            string headerTable = @"<Table><tr><td colspan='5' align='center' style='font-size: large;'><img src=" + logopath + "><b> The Indian Public School,Coimbatore</b></td></tr><tr><td colspan='5' align='center' style='font-size: large;'><b>Staff Wise Student Survey Result-" + AcaYear + "</b></td></tr><tr>";
                            headerTable = headerTable + "<td colspan='5' align='center' style='font-size: medium;'><b>Campus</b> : " + campus + "<b>Survey Number : </b>" + surveyno + " </td></tr></Table>";
                            var stuList = StaffstudentsurveyCount.First().Value.ToList();
                            base.ExptToXL_AssessPointChart(stuList, title, (item => new
                            {
                                item.StaffName,
                                item.Subject,
                                NoofStudentAttend = item.StudentCount.ToString(),
                                NoofQuestioninSurvey = item.QuestionCount.ToString(),
                                //Weightage = item.Score
                                AverageScore = item.Average,
                            }), headerTable);
                        }
                        else
                        {
                            long totalrecords = StaffstudentsurveyCount.First().Key;
                            int totalpages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var jsondat = new
                            {
                                total = totalpages,
                                page = page,
                                records = totalrecords,
                                rows = (from items in StaffstudentsurveyCount.FirstOrDefault().Value
                                        select new
                                        {
                                            cell = new string[]{
                                          items.Id.ToString(),
                                          items.AcademicYear, 
                                          items.Campus,
                                          items.StaffName,
                                          items.StaffPreRegNumber.ToString(),
                                          items.Subject,
                                          items.CategoryName,
                                          items.StudentCount.ToString(),
                                          items.QuestionCount.ToString(),
                                          items.Score.ToString(),
                                          items.Average.ToString(),
                                      }
                                        })
                            };
                            return Json(jsondat, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else
                {
                    Dictionary<long, IList<StaffWiseStudentSurveyNewResultWOS_Vw>> StaffstdsurveyCount = sms.GetStaffWiseStudentSurverWOSList(page - 1, rows, sidx, sord, criteria, likeCriteria);
                    if (StaffstdsurveyCount != null && StaffstdsurveyCount.Count > 0)
                    {
                        if (ExportType == "Excel")
                        {
                            string campus = string.IsNullOrEmpty(Campus) ? "All" : Campus;
                            string surveyno = string.IsNullOrEmpty(surveyNo) ? "All" : surveyNo;
                            string title = "StaffWiseSutdentSurveyReport";
                            string logopath = ConfigurationManager.AppSettings["AppLogos"].ToString() + "tips.gif";
                            string headerTable = @"<Table><tr><td colspan='5' align='center' style='font-size: large;'><img src=" + logopath + "><b> The Indian Public School,Coimbatore</b></td></tr><tr><td colspan='5' align='center' style='font-size: large;'><b>Staff Wise Student Survey Result-" + AcaYear + "</b></td></tr><tr>";
                            headerTable = headerTable + "<td colspan='5' align='center' style='font-size: medium;'><b>Campus</b> : " + campus + "<b>Survey Number : </b>" + surveyno + " </td></tr></Table>";
                            var stuList = StaffstdsurveyCount.First().Value.ToList();
                            base.ExptToXL_AssessPointChart(stuList, title, (item => new
                            {
                                item.StaffName,
                                item.Subject,
                                NoofStudentAttend = item.StudentCount.ToString(),
                                NoofQuestioninSurvey = item.QuestionCount.ToString(),
                                //Weightage = item.Score
                                AverageScore = item.Average,
                            }), headerTable);
                        }
                        else
                        {
                            long totalrecords = StaffstdsurveyCount.First().Key;
                            int totalpages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var jsondat = new
                            {
                                total = totalpages,
                                page = page,
                                records = totalrecords,
                                rows = (from items in StaffstdsurveyCount.FirstOrDefault().Value
                                        select new
                                        {
                                            cell = new string[]{
                                          items.Id.ToString(),
                                          items.AcademicYear, 
                                          items.Campus,
                                          items.StaffName,
                                          items.StaffPreRegNumber.ToString(),
                                          items.Subject,
                                          items.CategoryName,
                                          items.StudentCount.ToString(),
                                          items.QuestionCount.ToString(),
                                          items.Score.ToString(),
                                          items.Average.ToString(),
                                      }
                                        })
                            };
                            return Json(jsondat, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                return Json(new { rows = (new { cell = new string[] { } }) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public PartialViewResult ShowQuestionMarksWithSection(long? RowId)
        {
            StudentSurveyReportNew_Vw ssqr = smsObj.GetStudentSurveyReportById(RowId ?? 0);
            var viewModel = new StaffwiseSurveyQuestionViewModel()
            {
                AcademicYear = ssqr.AcademicYear,
                Campus = ssqr.Campus,
                Grade = ssqr.Grade,
                Section = ssqr.Section,
                Subject = ssqr.Subject,
                StaffName = ssqr.StaffName,
                StaffPreRegNumber = ssqr.StaffPreRegNumber,
                StaffEvaluationCategoryId = ssqr.StaffEvaluationCategoryId,
                StudentCount = ssqr.StudentCount
            };
            return PartialView(viewModel);
        }

        //public PartialViewResult ShowQuestionMarksWithoutSection(long? RowId)
        //{
        //    StudentSurveyReportWOSecNew_Vw ssqr = smsObj.GetStudentSurveyReportWOSecById(RowId ?? 0);
        //    var viewModel = new StaffwiseSurveyQuestionViewModel()
        //    {
        //        AcademicYear = ssqr.AcademicYear,
        //        Campus = ssqr.Campus,
        //        Grade = ssqr.Grade,
        //        Subject = ssqr.Subject,
        //        StaffName = ssqr.StaffName,
        //        StaffPreRegNumber = ssqr.StaffPreRegNumber,
        //        StudentSurveyQuestionId = ssqr.StaffEvaluationCategoryId,
        //        Section = "All",
        //        StudentCount = ssqr.StudentCount
        //    };
        //    return PartialView(viewModel);
        //}
        public PartialViewResult ShowQuestionMarksInStaff(long? RowId)
        {
            StudentSurveyReportNew_Vw ssqr = smsObj.GetStudentSurveyReportById(RowId ?? 0);
            var viewModel = new StaffwiseSurveyQuestionViewModel()
            {
                AcademicYear = ssqr.AcademicYear,
                Campus = ssqr.Campus,
                Grade = ssqr.Grade,
                Subject = ssqr.Subject,
                StaffName = ssqr.StaffName,
                StaffPreRegNumber = ssqr.StaffPreRegNumber,
                StaffEvaluationCategoryId = ssqr.StaffEvaluationCategoryId,
                Section = ssqr.Section,
                StudentCount = ssqr.StudentCount
            };
            return PartialView(viewModel);
        }

        public ActionResult ShowQuestionMarksListJqGrid(string acayear, string cam, string gra, string sect, long? preRegNum, long? SurveyId, int rows, string sidx, string sord, int? page = 1, int? ExptXl = 0)
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<string, object> likeCriteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(cam))
                    criteria.Add("Campus", cam);
                if (!string.IsNullOrEmpty(gra))
                    criteria.Add("Grade", gra);
                if (!string.IsNullOrEmpty(acayear))
                    criteria.Add("AcademicYear", acayear);
                if (SurveyId > 0)
                    criteria.Add("StaffEvaluationCategoryId", SurveyId);
                if (preRegNum > 0)
                    criteria.Add("StaffPreRegNumber", preRegNum);
                sord = sord == "desc" ? "Desc" : "Asc";
                if (!string.IsNullOrEmpty(sect) && sect == "All")
                {
                    Dictionary<long, IList<StaffwiseSurveyQuestionReportWOSec_Vw>> studentSureyMarksecList = sms.GetStudentSurveyQuestionMarkListWithoutSection(page - 1, rows, sidx, sord, criteria, likeCriteria);
                    if (studentSureyMarksecList != null && studentSureyMarksecList.Count > 0)
                    {
                        if (ExptXl == 1)
                        {
                            string title = "SutdentSurveyQuestionwiseReport";
                            string logopath = ConfigurationManager.AppSettings["AppLogos"].ToString() + "tips.gif";
                            string headerTable = @"<Table><tr><td colspan='7' align='center' style='font-size: large;'><img src=" + logopath + "><b> The Indian Public School,Coimbatore</b></td></tr><tr><td colspan='7' align='center' style='font-size: large;'><b>Student Survey Result-" + acayear + "</b></td></tr><tr>";
                            headerTable = headerTable + "<td colspan='7' align='center' style='font-size: medium;'><b>Campus</b> : " + cam + " &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp <b>Grade : </b> " + gra + " &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<b>Grade : </b> " + gra + "</td></tr></Table>";
                            var stuList = studentSureyMarksecList.First().Value.ToList();
                            base.ExptToXL_AssessPointChart(stuList, title, (item => new
                            {
                                EvaluationDate = item.EvaluationDate.ToString("dd/MM/yyyy"),
                                item.StaffName,
                                item.Subject,
                                NoofStudentAttend = item.StudentCount.ToString(),
                                QuestionAsked = item.StudentSurveyQuestion.ToString(),
                                ScoreinSurvey = item.Score,
                                AverageScore = decimal.Round(item.Average, 2, MidpointRounding.AwayFromZero).ToString()
                            }), headerTable);
                        }
                        else
                        {
                            long totalrecords = studentSureyMarksecList.First().Key;
                            int totalpages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var jsondat = new
                            {
                                total = totalpages,
                                page = page,
                                records = totalrecords,
                                userdata = new
                                {
                                    StudentSurveyQuestion = "Total:",
                                    Score = studentSureyMarksecList.FirstOrDefault().Value.Sum(x => x.Score).ToString(),
                                    Average = String.Format("{0:0.00}", studentSureyMarksecList.FirstOrDefault().Value.Sum(x => x.Average).ToString())
                                },
                                rows = (from items in studentSureyMarksecList.FirstOrDefault().Value
                                        select new
                                        {
                                            cell = new string[]{
                                          items.Id.ToString(),
                                          items.AcademicYear, 
                                          items.Campus,
                                          items.Grade,
                                          items.CategoryName,
                                          items.EvaluationDate.ToString("dd/MM/yyyy"),
                                          items.StaffName+"("+items.Subject+")",
                                          items.StudentCount.ToString(),
                                          items.StudentSurveyQuestion,
                                          items.Score.ToString(),
                                          decimal.Round(items.Average, 2, MidpointRounding.AwayFromZero).ToString()
                                      }
                                        })
                            };
                            return Json(jsondat, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else
                {
                    criteria.Add("Section", sect);
                    Dictionary<long, IList<StaffwiseSurveyQuestionReport_Vw>> studentSureyMarkList = sms.GetStudentSurveyQuestionMarkList(page - 1, rows, sidx, sord, criteria, likeCriteria);
                    if (studentSureyMarkList != null && studentSureyMarkList.Count > 0)
                    {
                        if (ExptXl > 0)
                        {
                            string title = "SutdentSurveyQuestionwiseReport";
                            string logopath = ConfigurationManager.AppSettings["AppLogos"].ToString() + "tips.gif";
                            string headerTable = @"<Table><tr><td colspan='7' align='center' style='font-size: large;'><img src=" + logopath + "><b> The Indian Public School,Coimbatore</b></td></tr><tr><td colspan='7' align='center' style='font-size: large;'><b>Student Survey Result - " + acayear + "</b></td></tr><tr>";
                            headerTable = headerTable + "<td colspan='7' align='center' style='font-size: medium;'><b>Campus</b> : " + cam + " &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<b>Grade : </b> " + gra + " &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<b>Survey Number : </b></td></tr></Table>";
                            var stuList = studentSureyMarkList.First().Value.ToList();
                            base.ExptToXL_AssessPointChart(stuList, title, (item => new
                            {
                                EvaluationDate = item.EvaluationDate.ToString("dd/MM/yyyy"),
                                item.StaffName,
                                item.Subject,
                                NoofStudentAttend = item.StudentCount.ToString(),
                                QuestionAsked = item.StudentSurveyQuestion.ToString(),
                                ScoreinSurvey = item.Score,
                                AverageScore = decimal.Round(item.Average, 2, MidpointRounding.AwayFromZero).ToString()
                            }), headerTable);
                        }
                        else
                        {
                            long totalrecords = studentSureyMarkList.First().Key;
                            int totalpages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var jsondat = new
                            {
                                total = totalpages,
                                page = page,
                                records = totalrecords,
                                userdata = new
                                {
                                    StudentSurveyQuestion = "Total:",
                                    Score = studentSureyMarkList.FirstOrDefault().Value.Sum(x => x.Score).ToString(),
                                    Average = decimal.Round(studentSureyMarkList.FirstOrDefault().Value.Sum(x => x.Average), 2, MidpointRounding.AwayFromZero).ToString()
                                },
                                rows = (from items in studentSureyMarkList.FirstOrDefault().Value
                                        select new
                                        {
                                            cell = new string[]{
                                          items.Id.ToString(),
                                          items.AcademicYear, 
                                          items.Campus,
                                          items.Grade,
                                          items.CategoryName,
                                          items.EvaluationDate.ToString("dd/MM/yyyy"),
                                          items.StaffName+"("+items.Subject+")",
                                          items.StudentCount.ToString(),
                                          items.StudentSurveyQuestion,
                                          items.Score.ToString(),
                                          //"<a style='border-bottom: 1px solid blue;' onclick=\"ShowWeightageMarkwithQuestionSec('" + items.AcademicYear+"','"+items.Campus+ "','" +items.Grade+"','"+items.Section+"','"+ items.StaffPreRegNumber + "','" + items.StaffEvaluationCategoryId + "');\" '>"+items.Score+"</a>",
                                          //String.Format("{0:0.00}", items.Average.ToString())
                                          decimal.Round(items.Average, 2, MidpointRounding.AwayFromZero).ToString()
                                      }
                                        })
                            };
                            return Json(jsondat, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                return Json(new { rows = (new { cell = new string[] { } }) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        #endregion
        #region NewStudentSurveyReport
        public ActionResult NewStudentSurveyReport()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MastersService ms = new MastersService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<long, IList<StaffEvaluationCategoryMaster>> StaffEvaluationCategoryList = smsObj.GetStaffEvaluationCategoryListWithPagingAndCriteria(0, 100, string.Empty, string.Empty, criteria, criteria);
                    ViewBag.campusddl = (from items in StaffEvaluationCategoryList.FirstOrDefault().Value
                                         select new { Campus = items.Campus }).Distinct().ToList();
                    ViewBag.campusallddl = (from items in StaffEvaluationCategoryList.FirstOrDefault().Value
                                            select new { Campus = items.Campus }).Distinct().ToList();
                    criteria.Clear();
                    Dictionary<long, IList<AcademicyrMaster>> AcademicyrMaster = ms.GetAcademicyrMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                    ViewBag.acadddl = AcademicyrMaster.First().Value;
                    ViewBag.currentAcyear = DateTime.Now.Month > 5 ? DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Year + 1).ToString() : (DateTime.Now.Year - 1).ToString() + "-" + DateTime.Now.Year.ToString();
                    #region BreadCrumb
                    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                    #endregion
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult SurveyStaffwiseReportJqgrid(long Id, string Survey, string ExportType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string campus = string.Empty;
                string AcaYear = string.Empty;
                string surveyname = string.Empty;
                string StaffName = string.Empty;
                long CampusBasedStaffDetails_Id = 0;
                long preRegNum = 0;
                if (!string.IsNullOrEmpty(Survey))
                {
                    StaffWiseSurveyNewResult_Vw staffwise = smsObj.GetStaffWiseSurveyNewResultById(Id);
                    if (staffwise != null)
                    {
                        campus = staffwise.Campus;
                        preRegNum = staffwise.StaffPreRegNumber;
                        CampusBasedStaffDetails_Id = staffwise.CampusBasedStaffDetails_Id;
                        surveyname = Survey;
                    }
                }
                else
                {
                    StaffWiseSurveyNewResultWOS_Vw staffwisewos = smsObj.StaffWiseSurveyNewResultWOSById(Id);
                    if (staffwisewos != null)
                    {
                        campus = staffwisewos.Campus;
                        preRegNum = staffwisewos.StaffPreRegNumber;
                        CampusBasedStaffDetails_Id = staffwisewos.CampusBasedStaffDetails_Id;
                    }
                }
                Dictionary<long, IList<SurveyReportNew_SP>> StaffEvaluationstdCount = smsObj.GetSurveyReportNew_SPListbySP(campus, null, null, null,surveyname, 0, preRegNum, CampusBasedStaffDetails_Id);
                if (StaffEvaluationstdCount != null && StaffEvaluationstdCount.Count > 0)
                {
                    if (ExportType == "Excel")
                    {
                        string title = "StaffStudentSurveyDetailsReprot";
                        string logopath = ConfigurationManager.AppSettings["AppLogos"].ToString() + "tips.gif";
                        string headerTable = @"<Table><tr><td colspan='7' align='center' style='font-size: large;'><img src=" + logopath + "><b> The Indian Public School,Coimbatore</b></td></tr><tr><td colspan='7' align='center' style='font-size: large;'><b>Student Survey Result - " + AcaYear + "</b></td></tr><tr>";
                        headerTable = headerTable + "<td colspan='7' align='center' style='font-size: medium;'><b>Campus</b> : " + campus + " &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<b>Grade : </b> All &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<b>Survey Number : </b>" + surveyname + "<b>Staff Name : </b>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp" + StaffName + " &nbsp&nbsp&nbsp&nbsp&nbsp </td></tr></Table>";
                        var stuList = StaffEvaluationstdCount.First().Value.ToList();
                        base.ExptToXL_AssessPointChart(stuList, title, (item => new
                        {
                            EvaluationDate = item.EvaluationDate.ToString("dd/MM/yyyy"),
                            item.Grade,
                            item.Section,
                            item.Subject,
                            NoofStudentAttend = item.StudentCount.ToString(),
                            NoofQuestioninSurvey = item.QuestionCount.ToString(),
                            AverageScore = item.Average
                        }), headerTable);
                    }
                    else
                    {
                        long totalrecords = StaffEvaluationstdCount.First().Key;
                        int totalpages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat = new
                        {
                            total = totalpages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in StaffEvaluationstdCount.FirstOrDefault().Value
                                    select new
                                    {
                                        cell = new string[]{
                                          items.Id.ToString(),
                                          items.AcademicYear, 
                                          items.Campus,
                                          items.Grade,
                                          items.Section,
                                          items.Subject,
                                          items.CategoryName,
                                          items.EvaluationDate.ToString("dd/MM/yyyy"),
                                          items.StudentCount.ToString(),
                                          items.QuestionCount.ToString(),
                                          items.Score.ToString(),                                          
                                          "<a style='text-decoration: none;cursor: pointer;border-bottom: 1px solid blue;' title='' onclick=\"ShowQuestionWeightageMarkinStaff('"+items.Id+"','" + items.AcademicYear+"','"+items.Campus+ "','" +items.Grade+"','"+items.Section+"','"+ items.StaffPreRegNumber + "','" + items.StaffEvaluationCategoryId + "','" + items.CampusBasedStaffDetails_Id + "');\" '>"+items.Average+"</a>",
                                      }
                                    })
                        };
                        return Json(jsondat, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { rows = (new { cell = new string[] { } }) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult SurveyReportAllJqgrid(string Campus, string Grade, string Section, string Surveyno, string AcademicYear, string ExportType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<string, object> likeCriteria = new Dictionary<string, object>();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (!string.IsNullOrEmpty(Campus))
                    criteria.Add("Campus", Campus);
                if (!string.IsNullOrEmpty(Grade))
                    criteria.Add("Grade", Grade);
                if (!string.IsNullOrEmpty(AcademicYear))
                    criteria.Add("AcademicYear", AcademicYear);
                if (!string.IsNullOrEmpty(Surveyno))
                    criteria.Add("CategoryName", Surveyno);
                if (!string.IsNullOrEmpty(Section) && Section == "ALL")
                {
                    Dictionary<long, IList<StudentSurveyReportWOSecNew_Vw>> studentSureyReportList = smsObj.GetStaffEvaluationStudentCountListWithoutSection(page - 1, rows, sidx, sord, criteria, likeCriteria);
                    if (studentSureyReportList != null && studentSureyReportList.Count > 0)
                    {
                        if (ExportType == "Excel")
                        {
                            string campus = string.IsNullOrEmpty(Campus) ? "All" : Campus;
                            string grade = string.IsNullOrEmpty(Grade) ? "All" : Grade;
                            string surveyno = string.IsNullOrEmpty(Surveyno) ? "All" : Surveyno;
                            string title = "SutdentSurveyReport";
                            string logopath = ConfigurationManager.AppSettings["AppLogos"].ToString() + "tips.gif";
                            string headerTable = @"<Table><tr><td colspan='6' align='center' style='font-size: large;'><img src=" + logopath + "><b> The Indian Public School,Coimbatore</b></td></tr><tr><td colspan='6' align='center' style='font-size: large;'><b>Student Survey Result-" + AcademicYear + "</b></td></tr><tr>";
                            headerTable = headerTable + "<td colspan='6' align='center' style='font-size: medium;'><b>Campus</b> : " + campus + " &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp <b>Grade : </b> " + grade + " &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<b>Survey Number : </b>" + surveyno + " </td></tr></Table>";
                            var stuList = studentSureyReportList.First().Value.ToList();
                            base.ExptToXL_AssessPointChart(stuList, title, (item => new
                            {
                                EvaluationDate = item.EvaluationDate.ToString("dd/MM/yyyy"),
                                item.StaffName,
                                item.Subject,
                                NoofStudentAttend = item.StudentCount.ToString(),
                                NoofQuestionAsked = item.QuestionCount.ToString(),
                                AverageScore = item.Average,
                            }), headerTable);
                        }
                        else
                        {
                            long totalrecords = studentSureyReportList.First().Key;
                            int totalpages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var jsondat = new
                            {
                                total = totalpages,
                                page = page,
                                records = totalrecords,
                                rows = (from items in studentSureyReportList.FirstOrDefault().Value
                                        select new
                                        {
                                            cell = new string[]{
                                          items.Id.ToString(),
                                          items.AcademicYear, 
                                          items.Campus,
                                          items.Grade,
                                          items.CategoryName,
                                          items.EvaluationDate.ToString("dd/MM/yyyy"),
                                          items.StaffName+"("+items.Subject+")",
                                          items.StudentCount.ToString(),
                                          items.QuestionCount.ToString(),
                                          items.Score.ToString(),                                          
                                          "<a style='text-decoration: none;cursor: pointer;border-bottom: 1px solid blue;' title='' onclick=\"ShowWeightageMarkswithQuestion('"+items.Id+"','" + items.AcademicYear+"','"+items.Campus+ "','" +items.Grade+"','"+ items.StaffPreRegNumber + "','" + items.StaffEvaluationCategoryId + "');\" '>"+items.Average+"</a>",
                                      }
                                        })
                            };
                            return Json(jsondat, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(Section))
                        criteria.Add("Section", Section);
                    Dictionary<long, IList<SurveyReportNew_Vw>> StaffEvaluationstdCount = smsObj.GetSurveyReportNew_VwStudentCountList(page - 1, rows, sidx, sord, criteria, likeCriteria);
                    if (StaffEvaluationstdCount != null && StaffEvaluationstdCount.Count > 0)
                    {
                        if (ExportType == "Excel")
                        {
                            string campus = String.IsNullOrEmpty(Campus) ? "All" : Campus;
                            string gradesec = Grade == null ? "All" : Grade;
                            gradesec = gradesec + "-" + Section;
                            string surveyno = String.IsNullOrEmpty(Surveyno) ? "All" : Surveyno;
                            string title = "SutdentSurveyReport";
                            string logopath = ConfigurationManager.AppSettings["AppLogos"].ToString() + "tips.gif";
                            string headerTable = @"<Table><tr><td colspan='6' align='center' style='font-size: large;'><img src=" + logopath + "><b> The Indian Public School,Coimbatore</b></td></tr><tr><td colspan='6' align='center' style='font-size: large;'><b>Student Survey Result - " + AcademicYear + "</b></td></tr><tr>";
                            headerTable = headerTable + "<td colspan='6' align='center' style='font-size: medium;'><b>Campus</b> : " + campus + " &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<b>Grade : </b> " + gradesec + " &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<b>Survey Number : </b>" + surveyno + " </td></tr></Table>";
                            var stuList = StaffEvaluationstdCount.First().Value.ToList();
                            base.ExptToXL_AssessPointChart(stuList, title, (item => new
                            {
                                EvaluationDate = item.EvaluationDate.ToString("dd/MM/yyyy"),
                                item.StaffName,
                                item.Subject,
                                NoofStudentAttend = item.StudentCount.ToString(),
                                NoofQuestionAsked = item.QuestionCount.ToString(),
                                AverageScore = item.Average,
                            }), headerTable);
                        }
                        else
                        {
                            long totalrecords = StaffEvaluationstdCount.First().Key;
                            int totalpages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var jsondat = new
                            {
                                total = totalpages,
                                page = page,
                                records = totalrecords,
                                rows = (from items in StaffEvaluationstdCount.FirstOrDefault().Value
                                        select new
                                        {
                                            cell = new string[]{
                                          items.Id.ToString(),
                                          items.AcademicYear, 
                                          items.Campus,
                                          items.Grade,
                                          items.Section,
                                          items.CategoryName,
                                          items.EvaluationDate.ToString("dd/MM/yyyy"),
                                          items.StaffName+"("+items.Subject+")",
                                          items.StudentCount.ToString(),
                                          items.QuestionCount.ToString(),
                                          items.Score.ToString(),                                          
                                          "<a style='text-decoration: none;cursor: pointer;border-bottom: 1px solid blue;'  title='' onclick=\"ShowWeightageMarkwithQuestionSec('"+items.Id+"','" + items.AcademicYear+"','"+items.Campus+ "','" +items.Grade+"','"+items.Section+"','"+ items.StaffPreRegNumber + "','" + items.StaffEvaluationCategoryId + "','" + items.CampusBasedStaffDetails_Id + "');\" '>"+items.Average+"</a>"
                                      }
                                        })
                            };
                            return Json(jsondat, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                return Json(new { rows = (new { cell = new string[] { } }) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult StaffWiseSurveyReportJqgrid(string Campus, string AcaYear, long? StaffPreRegNum, string Subject, string surveyNo, string ExportType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<string, object> likeCriteria = new Dictionary<string, object>();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (!string.IsNullOrEmpty(Campus))
                    criteria.Add("Campus", Campus);
                if (!string.IsNullOrEmpty(AcaYear))
                    criteria.Add("AcademicYear", AcaYear);
                if (StaffPreRegNum > 0)
                    criteria.Add("StaffPreRegNumber", StaffPreRegNum);
                if (!string.IsNullOrEmpty(Subject))
                    criteria.Add("Subject", Subject);
                if (!string.IsNullOrEmpty(surveyNo))
                {
                    criteria.Add("CategoryName", surveyNo);
                    Dictionary<long, IList<StaffWiseSurveyNewResult_Vw>> StaffstudentsurveyCount = smsObj.GetStaffWiseSurveyList(page - 1, rows, sidx, sord, criteria, likeCriteria);
                    if (StaffstudentsurveyCount != null && StaffstudentsurveyCount.Count > 0)
                    {
                        if (ExportType == "Excel")
                        {
                            string campus = string.IsNullOrEmpty(Campus) ? "All" : Campus;
                            string surveyno = string.IsNullOrEmpty(surveyNo) ? "All" : surveyNo;
                            string title = "StaffWiseSutdentSurveyReport";
                            string logopath = ConfigurationManager.AppSettings["AppLogos"].ToString() + "tips.gif";
                            string headerTable = @"<Table><tr><td colspan='5' align='center' style='font-size: large;'><img src=" + logopath + "><b> The Indian Public School,Coimbatore</b></td></tr><tr><td colspan='5' align='center' style='font-size: large;'><b>Staff Wise Student Survey Result-" + AcaYear + "</b></td></tr><tr>";
                            headerTable = headerTable + "<td colspan='5' align='center' style='font-size: medium;'><b>Campus</b> : " + campus + "<b>Survey Number : </b>" + surveyno + " </td></tr></Table>";
                            var stuList = StaffstudentsurveyCount.First().Value.ToList();
                            base.ExptToXL_AssessPointChart(stuList, title, (item => new
                            {
                                item.StaffName,
                                item.Subject,
                                NoofStudentAttend = item.StudentCount.ToString(),
                                NoofQuestioninSurvey = item.QuestionCount.ToString(),
                                AverageScore = item.Average,
                            }), headerTable);
                        }
                        else
                        {
                            long totalrecords = StaffstudentsurveyCount.First().Key;
                            int totalpages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var jsondat = new
                            {
                                total = totalpages,
                                page = page,
                                records = totalrecords,
                                rows = (from items in StaffstudentsurveyCount.FirstOrDefault().Value
                                        select new
                                        {
                                            cell = new string[]{
                                          items.Id.ToString(),
                                          items.AcademicYear, 
                                          items.Campus,
                                          items.StaffName,                                          
                                          items.StaffPreRegNumber.ToString(),
                                          items.Subject,
                                          items.CategoryName,
                                          items.StudentCount.ToString(),
                                          items.QuestionCount.ToString(),
                                          items.Score.ToString(),
                                          items.Average.ToString(),
                                      }
                                        })
                            };
                            return Json(jsondat, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else
                {
                    Dictionary<long, IList<StaffWiseSurveyNewResultWOS_Vw>> StaffstdsurveyCount = smsObj.GetStaffWiseSurveyWOSList(page - 1, rows, sidx, sord, criteria, likeCriteria);
                    if (StaffstdsurveyCount != null && StaffstdsurveyCount.Count > 0)
                    {
                        if (ExportType == "Excel")
                        {
                            string campus = string.IsNullOrEmpty(Campus) ? "All" : Campus;
                            string surveyno = string.IsNullOrEmpty(surveyNo) ? "All" : surveyNo;
                            string title = "StaffWiseSutdentSurveyReport";
                            string logopath = ConfigurationManager.AppSettings["AppLogos"].ToString() + "tips.gif";
                            string headerTable = @"<Table><tr><td colspan='5' align='center' style='font-size: large;'><img src=" + logopath + "><b> The Indian Public School,Coimbatore</b></td></tr><tr><td colspan='5' align='center' style='font-size: large;'><b>Staff Wise Student Survey Result-" + AcaYear + "</b></td></tr><tr>";
                            headerTable = headerTable + "<td colspan='5' align='center' style='font-size: medium;'><b>Campus</b> : " + campus + "<b>Survey Number : </b>" + surveyno + " </td></tr></Table>";
                            var stuList = StaffstdsurveyCount.First().Value.ToList();
                            base.ExptToXL_AssessPointChart(stuList, title, (item => new
                            {
                                item.StaffName,
                                item.Subject,
                                NoofStudentAttend = item.StudentCount.ToString(),
                                NoofQuestioninSurvey = item.QuestionCount.ToString(),
                                AverageScore = item.Average,
                            }), headerTable);
                        }
                        else
                        {
                            long totalrecords = StaffstdsurveyCount.First().Key;
                            int totalpages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var jsondat = new
                            {
                                total = totalpages,
                                page = page,
                                records = totalrecords,
                                rows = (from items in StaffstdsurveyCount.FirstOrDefault().Value
                                        select new
                                        {
                                            cell = new string[]{
                                          items.Id.ToString(),
                                          items.AcademicYear, 
                                          items.Campus,
                                          items.StaffName,
                                          items.StaffPreRegNumber.ToString(),
                                          items.Subject,
                                          items.CategoryName,
                                          items.StudentCount.ToString(),
                                          items.QuestionCount.ToString(),
                                          items.Score.ToString(),
                                          items.Average.ToString(),
                                      }
                                        })
                            };
                            return Json(jsondat, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                return Json(new { rows = (new { cell = new string[] { } }) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult ShowSurveyQuestionMarksListJqGrid(string acayear, string cam, string gra, string sect, long? preRegNum, long? SurveyId, long? CampusBasedStaffDetails_Id, int rows, string sidx, string sord, int? page = 1, int? ExptXl = 0)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<string, object> likeCriteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(cam))
                    criteria.Add("Campus", cam);
                if (!string.IsNullOrEmpty(gra))
                    criteria.Add("Grade", gra);
                if (!string.IsNullOrEmpty(acayear))
                    criteria.Add("AcademicYear", acayear);
                if (SurveyId > 0)
                    criteria.Add("StaffEvaluationCategoryId", SurveyId);
                if (preRegNum > 0)
                    criteria.Add("StaffPreRegNumber", preRegNum);
                if (CampusBasedStaffDetails_Id > 0)
                    criteria.Add("CampusBasedStaffDetails_Id", CampusBasedStaffDetails_Id);
                sord = sord == "desc" ? "Desc" : "Asc";
                if (!string.IsNullOrEmpty(sect) && sect == "All")
                {
                    Dictionary<long, IList<StaffwiseSurveyQuestionReportWOSec_Vw>> studentSureyMarksecList = smsObj.GetStudentSurveyQuestionMarkListWithoutSection(page - 1, rows, sidx, sord, criteria, likeCriteria);
                    if (studentSureyMarksecList != null && studentSureyMarksecList.Count > 0)
                    {
                        if (ExptXl == 1)
                        {
                            string title = "SutdentSurveyQuestionwiseReport";
                            string logopath = ConfigurationManager.AppSettings["AppLogos"].ToString() + "tips.gif";
                            string headerTable = @"<Table><tr><td colspan='7' align='center' style='font-size: large;'><img src=" + logopath + "><b> The Indian Public School,Coimbatore</b></td></tr><tr><td colspan='7' align='center' style='font-size: large;'><b>Student Survey Result-" + acayear + "</b></td></tr><tr>";
                            headerTable = headerTable + "<td colspan='7' align='center' style='font-size: medium;'><b>Campus</b> : " + cam + " &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp <b>Grade : </b> " + gra + " &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<b>Grade : </b> " + gra + "</td></tr></Table>";
                            var stuList = studentSureyMarksecList.First().Value.ToList();
                            base.ExptToXL_AssessPointChart(stuList, title, (item => new
                            {
                                EvaluationDate = item.EvaluationDate.ToString("dd/MM/yyyy"),
                                item.StaffName,
                                item.Subject,
                                NoofStudentAttend = item.StudentCount.ToString(),
                                QuestionAsked = item.StudentSurveyQuestion.ToString(),
                                ScoreinSurvey = item.Score.ToString(),
                                AverageScore = item.Average.ToString()
                            }), headerTable);
                        }
                        else
                        {
                            long totalrecords = studentSureyMarksecList.First().Key;
                            int totalpages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var jsondat = new
                            {
                                total = totalpages,
                                page = page,
                                records = totalrecords,
                                userdata = new
                                {
                                    StudentSurveyQuestion = "Total:",
                                    Score = studentSureyMarksecList.FirstOrDefault().Value.Sum(x => x.Score).ToString(),
                                    Average = studentSureyMarksecList.FirstOrDefault().Value.Sum(x => x.Average).ToString()
                                },
                                rows = (from items in studentSureyMarksecList.FirstOrDefault().Value
                                        select new
                                        {
                                            cell = new string[]{
                                          items.Id.ToString(),
                                          items.AcademicYear, 
                                          items.Campus,
                                          items.Grade,
                                          items.CategoryName,
                                          items.EvaluationDate.ToString("dd/MM/yyyy"),
                                          items.StaffName+"("+items.Subject+")",
                                          items.StudentCount.ToString(),
                                          items.StudentSurveyQuestion,
                                          items.Score.ToString(),                                          
                                          items.Average.ToString()
                                      }
                                        })
                            };
                            return Json(jsondat, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else
                {
                    criteria.Add("Section", sect);
                    Dictionary<long, IList<StaffwiseSurveyQuestionReportNew_Vw>> studentSureyMarkList = smsObj.GetStudentSurveyQuestionMarkNewList(page - 1, rows, sidx, sord, criteria, likeCriteria);
                    if (studentSureyMarkList != null && studentSureyMarkList.Count > 0)
                    {
                        if (ExptXl > 0)
                        {
                            string title = "SutdentSurveyQuestionwiseReport";
                            string logopath = ConfigurationManager.AppSettings["AppLogos"].ToString() + "tips.gif";
                            string headerTable = @"<Table><tr><td colspan='7' align='center' style='font-size: large;'><img src=" + logopath + "><b> The Indian Public School,Coimbatore</b></td></tr><tr><td colspan='7' align='center' style='font-size: large;'><b>Student Survey Result - " + acayear + "</b></td></tr><tr>";
                            headerTable = headerTable + "<td colspan='7' align='center' style='font-size: medium;'><b>Campus</b> : " + cam + " &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<b>Grade : </b> " + gra + " &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<b>Survey Number : </b></td></tr></Table>";
                            var stuList = studentSureyMarkList.First().Value.ToList();
                            base.ExptToXL_AssessPointChart(stuList, title, (item => new
                            {
                                EvaluationDate = item.EvaluationDate.ToString("dd/MM/yyyy"),
                                item.StaffName,
                                item.Subject,
                                NoofStudentAttend = item.StudentCount.ToString(),
                                QuestionAsked = item.SurveyQuestion.ToString(),
                                ScoreinSurvey = item.Score,
                                AverageScore = item.Average.ToString()
                            }), headerTable);
                        }
                        else
                        {
                            long totalrecords = studentSureyMarkList.First().Key;
                            int totalpages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var jsondat = new
                            {
                                total = totalpages,
                                page = page,
                                records = totalrecords,
                                userdata = new
                                {
                                    StudentSurveyQuestion = "Total:",
                                    Score = studentSureyMarkList.FirstOrDefault().Value.Sum(x => x.Score).ToString(),
                                    Average = studentSureyMarkList.FirstOrDefault().Value.Sum(x => x.Average).ToString()
                                },
                                rows = (from items in studentSureyMarkList.FirstOrDefault().Value
                                        select new
                                        {
                                            cell = new string[]{
                                          items.Id.ToString(),
                                          items.AcademicYear, 
                                          items.Campus,
                                          items.Grade,
                                          items.CategoryName,
                                          items.EvaluationDate.ToString("dd/MM/yyyy"),
                                          items.StaffName+"("+items.Subject+")",
                                          items.StudentCount.ToString(),
                                          items.SurveyQuestion,
                                          items.Score.ToString(),                                          
                                          items.Average.ToString()
                                      }
                                        })
                            };
                            return Json(jsondat, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                return Json(new { rows = (new { cell = new string[] { } }) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public PartialViewResult ShowSurveyQuestionMarksWithSection(long? RowId)
        {
            SurveyReportNew_Vw ssqr = smsObj.GetSurveyReportById(RowId ?? 0);
            var viewModel = new StaffwiseSurveyQuestionViewModel()
            {
                AcademicYear = ssqr.AcademicYear,
                Campus = ssqr.Campus,
                Grade = ssqr.Grade,
                Section = ssqr.Section,
                Subject = ssqr.Subject,
                StaffName = ssqr.StaffName,
                StaffPreRegNumber = ssqr.StaffPreRegNumber,
                StaffEvaluationCategoryId = ssqr.StaffEvaluationCategoryId,
                StudentCount = ssqr.StudentCount
            };
            return PartialView(viewModel);
        }
        public PartialViewResult ShowSurveyQuestionMarksInStaff(long? RowId, string Cam, string grad, string sec, string acayr, long surveyid, long preregnum, long CampusBasedStaffDetails_Id)
        {
            Dictionary<long, IList<SurveyReportNew_SP>> ssqrlist = smsObj.GetSurveyReportNew_SPListbySP(Cam, grad, sec, acayr,null, surveyid, preregnum, CampusBasedStaffDetails_Id);
            var viewModel = new StaffwiseSurveyQuestionViewModel()
            {
                AcademicYear = ssqrlist.FirstOrDefault().Value[0].AcademicYear,
                Campus = ssqrlist.FirstOrDefault().Value[0].Campus,
                Grade = ssqrlist.FirstOrDefault().Value[0].Grade,
                Subject = ssqrlist.FirstOrDefault().Value[0].Subject,
                StaffName = ssqrlist.FirstOrDefault().Value[0].StaffName,
                StaffPreRegNumber = ssqrlist.FirstOrDefault().Value[0].StaffPreRegNumber,
                StaffEvaluationCategoryId = ssqrlist.FirstOrDefault().Value[0].StaffEvaluationCategoryId,
                Section = ssqrlist.FirstOrDefault().Value[0].Section,
                StudentCount = ssqrlist.FirstOrDefault().Value[0].StudentCount
            };
            return PartialView(viewModel);
        }
        #endregion
        #region survey group master added by john naveen
        public ActionResult SurveyGroupMaster()
        {

            try
            {
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
                #region BreadCrumb
                string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                #endregion
                return View();

            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult AddOrEditSurveyGroupMaster(SurveyGroupMaster survey)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("logOut", "Account");
                else
                {

                    StaffManagementService sms = new StaffManagementService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    if (!string.IsNullOrEmpty(survey.SurveyGroup))
                        criteria.Add("SurveyGroup", survey.SurveyGroup);
                    if (survey == null) return null;
                    SurveyGroupMaster surveyObj = new SurveyGroupMaster();
                    surveyObj = sms.GetSurveyGroupMasterByGroupName(survey.SurveyGroup);
                    var script = "";
                    if (survey.SurveyGroupId == 0)
                    {

                        if (surveyObj == null)
                        {
                            survey.IsActive = true;
                            survey.CreatedBy = userId;
                            survey.CreatedDate = DateTime.Now;
                            survey.ModifiedBy = userId;
                            survey.ModifiedDate = DateTime.Now;
                            sms.CreateOrUpdateSurveyGroupMaster(survey);
                            return JavaScript(script);
                        }
                        else
                        {
                            script = @"ErrMsg(""The Name is already exist!"");";
                            return JavaScript(script);

                        }
                    }
                    else
                    {

                        survey.ModifiedBy = userId;
                        survey.ModifiedDate = DateTime.Now;
                        sms.CreateOrUpdateSurveyGroupMaster(survey);
                        script = @"SucessMsg(""Updated Sucessfully"");";
                        return JavaScript(script);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "MasterPolicy");
                throw ex;
            }
        }
        public ActionResult DeleteSurveyGroupMaster(string[] Id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("Logout", "Account");
                else
                {
                    if (Id == null)
                        return null;
                    StaffManagementService sms = new StaffManagementService();
                    string[] CityIdArray = Id[0].Split(',');
                    long[] longCityIdArray = new long[CityIdArray.Length];
                    for (int j = 0; j < CityIdArray.Length; j++)
                        longCityIdArray[j] = Convert.ToInt64(CityIdArray[j]);
                    sms.DeleteSurveyGroupMaster(longCityIdArray);
                    var script = @"SucessMsg(""Deleted Successfully"");";
                    return JavaScript(script);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public ActionResult SurveyGroupJMasterqgrid(string SurveyGroup, string IsActive, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                sord = sord == "desc" ? "Desc" : "Asc";

                if (!string.IsNullOrEmpty(SurveyGroup))
                {
                    criteria.Add("SurveyGroup", SurveyGroup);
                }
                if (!string.IsNullOrEmpty(IsActive))
                {
                    if (IsActive == "true" || IsActive == "True")
                    {
                        criteria.Add("IsActive", true);
                    }
                    if (IsActive == "false" || IsActive == "False")
                    {
                        criteria.Add("IsActive", false);
                    }
                }
                Dictionary<long, IList<SurveyGroupMaster>> SurveyGroupList = sms.GetSurveyGroupMasterListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (SurveyGroupList != null && SurveyGroupList.Count > 0)
                {
                    long totalrecords = SurveyGroupList.First().Key;
                    int totalpages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalpages,
                        page = page,
                        records = totalrecords,
                        rows = (from items in SurveyGroupList.FirstOrDefault().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[]{
                                          items.SurveyGroupId.ToString(),
                                          items.SurveyGroup, 
                                          items.IsActive==true?"Yes":"No",
                                          items.CreatedBy,
                                          items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy"):"",
                                          items.ModifiedBy,
                                          items.ModifiedDate!=null?items.ModifiedDate.Value.ToString("dd/MM/yyyy"):""
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
        public ActionResult GetSurveyGroupddl()
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<long, IList<SurveyGroupMaster>> SurveyGroupMasterList = smsObj.GetSurveyGroupMasterListWithPagingAndCriteria(0, 9999, "SurveyGroup", "Asc", criteria);
                var camp = (from items in SurveyGroupMasterList.FirstOrDefault().Value
                            select new
                            {
                                Text = items.SurveyGroup,
                                Value = items.SurveyGroupId
                            }).Distinct().ToList();
                return Json(camp, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "MasterPolicy");
                throw ex;
            }
        }
        #endregion
        #region SurveyQuestionMaster
        public ActionResult SurveyQuestionMaster()
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
                    return View();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult GetJqGridsurveyList(SurveyQuestionMaster survey, string IsActive, long? SurveyGroupId, int rows, string sord, string sidx, int? page = 1)
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<string, object> likecriteria = new Dictionary<string, object>();
                criteria.Clear();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (!string.IsNullOrEmpty(IsActive))
                {
                    if (IsActive == "true" || IsActive == "True")
                    {
                        criteria.Add("IsActive", true);
                    }
                    if (IsActive == "false" || IsActive == "False")
                    {
                        criteria.Add("IsActive", false);
                    }
                }
                if (SurveyGroupId > 0)
                {
                    criteria.Add("SurveyGroupMaster.SurveyGroupId", SurveyGroupId);
                }
                if (!string.IsNullOrEmpty(survey.SurveyQuestion))
                {
                    likecriteria.Add("SurveyQuestion", survey.SurveyQuestion);
                }
                Dictionary<long, IList<SurveyQuestionMaster>> SurveyQuestionObj = sms.GetSurveyQuestionMasterWithExactAndLikeSearchCriteriaWithCount(page - 1, rows, sidx, sord, criteria, likecriteria);
                if (SurveyQuestionObj != null && SurveyQuestionObj.FirstOrDefault().Key > 0)
                {
                    IList<SurveyQuestionMaster> SurveyQuestionDetails = SurveyQuestionObj.FirstOrDefault().Value;
                    long totalRecords = SurveyQuestionObj.FirstOrDefault().Key;
                    int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                    var jsonData = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalRecords,
                        rows =
                        (
                        from items in SurveyQuestionDetails
                        select new
                        {
                            i = items.SurveyQuestionId,
                            cell = new string[]
                           {
                                items.SurveyQuestionId.ToString(),                                                                                              
                                items.SurveyGroupMaster.SurveyGroup,                                
                                items.SurveyQuestion,
                                items.IsActive==true?"Yes":"No"                                                                                            
                           }
                        }
                        )
                    };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var Empty = new { rows = (new { cell = new string[] { } }) };
                    return Json(Empty, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                //ExceptionPolicy.HandleException(ex, "MasterPolicy");
                throw ex;
            }
        }
        public ActionResult AddSurveyQuestionMaster(SurveyQuestionMaster sqm)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    SurveyQuestionMaster sqmdetails = smsObj.GetSurveyQuestionMasterBySurveyGroupIdandQuestion(sqm.SurveyGroupMaster.SurveyGroupId, sqm.SurveyQuestion);
                    if (sqmdetails != null)
                    {
                        var script = @"ErrMsg(""Already Exist"");";
                        return JavaScript(script);
                    }
                    sqm.CreatedBy = userId;
                    sqm.CreatedDate = DateTime.Now;
                    sqm.IsActive = true;
                    smsObj.CreateOrUpdateSurveyQuestionMaster(sqm);
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult EditSurveyQuestionMaster(SurveyQuestionMaster sqm)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (sqm.SurveyQuestionId > 0)
                    {
                        SurveyQuestionMaster sqmdtls = smsObj.GetSurveyQuestionMasterBySurveyGroupIdandQuestion(sqm.SurveyGroupMaster.SurveyGroupId, sqm.SurveyQuestion);
                        if (sqmdtls != null)
                        {
                            var script = @"ErrMsg(""Already Exist"");";
                            return JavaScript(script);
                        }
                        SurveyQuestionMaster sqmdetails = smsObj.GetSurveyQuestionMasterById(sqm.SurveyQuestionId);
                        if (sqmdetails != null)
                        {
                            sqmdetails.ModifiedBy = userId;
                            sqmdetails.ModifiedDate = DateTime.Now;
                            sqmdetails.SurveyGroupMaster.SurveyGroupId = sqm.SurveyGroupMaster.SurveyGroupId;
                            sqmdetails.SurveyQuestion = sqm.SurveyQuestion;
                            smsObj.CreateOrUpdateSurveyQuestionMaster(sqmdetails);
                        }
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult DeleteSurveyQuestionMaster(string Id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    string[] arrayId = Id.Split(',');
                    long[] SurveyQuestionMasterId = Array.ConvertAll(arrayId, Int64.Parse);
                    smsObj.DeleteSurveyQuestionMaster(SurveyQuestionMasterId);
                    var script = @"SucessMsg(""Deleted  Successfully"");";
                    return JavaScript(script);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        #endregion
        #region Survey Answer Master john naveen
        public ActionResult SurveyAnswerMaster()
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
                    return View();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult GetJqGridsurveyanswerList(SurveyAnswerMaster survey, string IsActive, string IsPositive, long? SurveyQuestionId, int rows, string sord, string sidx, int? page = 1)
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Clear();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (!string.IsNullOrEmpty(survey.SurveyAnswer)) { criteria.Add("SurveyAnswer", survey.SurveyAnswer); }
                if (survey.SurveyMark > 0) { criteria.Add("SurveyMark", survey.SurveyMark); }
                if (!string.IsNullOrEmpty(IsActive))
                {
                    if (IsActive == "true" || IsActive == "True")
                    {
                        criteria.Add("IsActive", true);
                    }
                    if (IsActive == "false" || IsActive == "False")
                    {
                        criteria.Add("IsActive", false);
                    }
                }
                if (!string.IsNullOrEmpty(IsPositive))
                {
                    if (IsPositive == "true" || IsPositive == "True")
                    {
                        criteria.Add("IsPositive", true);
                    }
                    if (IsPositive == "false" || IsPositive == "False")
                    {
                        criteria.Add("IsPositive", false);
                    }
                }
                criteria.Add("SurveyQuestionMaster.SurveyQuestionId", SurveyQuestionId);
                Dictionary<long, IList<SurveyAnswerMaster>> SurveyAnswerObj = sms.GetSurveyAnswerMasterWithPagingAndCriteria(page - 1, rows, sord, sidx, criteria);
                if (SurveyAnswerObj != null && SurveyAnswerObj.FirstOrDefault().Key > 0)
                {
                    IList<SurveyAnswerMaster> SurveyAnswerDetails = SurveyAnswerObj.FirstOrDefault().Value;
                    long totalRecords = SurveyAnswerObj.FirstOrDefault().Key;
                    int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                    var jsonData = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalRecords,
                        rows =
                        (
                        from items in SurveyAnswerDetails
                        select new
                        {
                            i = items.SurveyAnswerId,
                            cell = new string[]
                           {
                                items.SurveyAnswerId.ToString(),
                                items.SurveyQuestionMaster.SurveyQuestion,  
                                items.SurveyAnswer,
                                items.SurveyMark.ToString(),                             
                                items.IsPositive==true?"Yes":"No",
                                items.IsActive==true?"Yes":"No"                                                            
                           }
                        }
                        )
                    };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var Empty = new { rows = (new { cell = new string[] { } }) };
                    return Json(Empty, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult SaveOrUpdateSurveyAnswerMaster(SurveyAnswerMaster sam)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("logOut", "Account");
                else
                {

                    StaffManagementService sms = new StaffManagementService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    SurveyAnswerMaster samObj = new SurveyAnswerMaster();
                    samObj = sms.GetSurveyAnswerMasterBySurveyQuestionandMark(sam.SurveyAnswer, sam.SurveyQuestionMaster.SurveyQuestionId, sam.SurveyMark, sam.IsPositive);
                    var script = "";
                    if (sam.SurveyAnswerId == 0)
                    {

                        if (samObj == null)
                        {
                            sam.IsActive = true;
                            sam.CreatedBy = userId;
                            sam.CreatedDate = DateTime.Now;
                            sam.ModifiedBy = userId;
                            sam.ModifiedDate = DateTime.Now;
                            sms.CreateOrUpdateSurveyAnswerMaster(sam);
                            return JavaScript(script);
                        }
                        else
                        {
                            script = @"ErrMsg(""The Name is already exist!"");";
                            return JavaScript(script);

                        }
                    }
                    else
                    {
                        if (samObj == null)
                        {
                            sam.IsActive = true;
                            sam.CreatedBy = userId;
                            sam.CreatedDate = DateTime.Now;
                            sam.ModifiedBy = userId;
                            sam.ModifiedDate = DateTime.Now;
                            sms.CreateOrUpdateSurveyAnswerMaster(sam);
                            script = @"SucessMsg(""Updated Sucessfully"");";
                            return JavaScript(script);
                        }
                        else
                        {
                            script = @"ErrMsg(""The Name is already exist!"");";
                            return JavaScript(script);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "MasterPolicy");
                throw ex;
            }
        }
        public ActionResult GetSurveyQuestionddl()
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<string, object> likecriteria = new Dictionary<string, object>();
                Dictionary<long, IList<SurveyQuestionMaster>> SurveyQuestionList = sms.GetSurveyQuestionMasterWithExactAndLikeSearchCriteriaWithCount(0, 9999, null, null, criteria, likecriteria);
                var camp = (from items in SurveyQuestionList.FirstOrDefault().Value
                            select new
                            {
                                Text = items.SurveyQuestion,
                                Value = items.SurveyQuestionId
                            }).Distinct().ToList();
                return Json(camp, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "MasterPolicy");
                throw ex;
            }
        }
        #endregion
        #region SurveyConfiguration
        public ActionResult SurveyConfiguration()
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
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    MastersService ms = new MastersService();
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
                    Dictionary<long, IList<AcademicyrMaster>> AcademicyrMaster = ms.GetAcademicyrMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);

                    ViewBag.campusddl = CampusMaster.First().Value;
                    ViewBag.acadddl = AcademicyrMaster.First().Value;
                    return View();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult GetJqGridSurveyConfigurationList(SurveyConfiguration_vw surveyconfig, string IsActive, int rows, string sord, string sidx, int? page = 1)
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Clear();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (!string.IsNullOrEmpty(surveyconfig.Campus))
                    criteria.Add("Campus", surveyconfig.Campus);
                if (!string.IsNullOrEmpty(surveyconfig.Grade))
                    criteria.Add("Grade", surveyconfig.Grade);
                if (!string.IsNullOrEmpty(surveyconfig.AcademicYear))
                    criteria.Add("AcademicYear", surveyconfig.AcademicYear);
                if (!string.IsNullOrEmpty(surveyconfig.SurveyNumber))
                    criteria.Add("SurveyNumber", surveyconfig.SurveyNumber);
                if (!string.IsNullOrEmpty(IsActive))
                {
                    if (IsActive == "true" || IsActive == "True")
                    {
                        criteria.Add("IsActive", true);
                    }
                    if (IsActive == "false" || IsActive == "False")
                    {
                        criteria.Add("IsActive", false);
                    }
                }
                else
                {                    
                        criteria.Add("IsActive", true);                    
                }
                //if (SurveyGroupId > 0)
                //{
                //    criteria.Add("SurveyGroupMaster.SurveyGroupId", SurveyGroupId);
                //}
                Dictionary<long, IList<SurveyConfiguration_vw>> SurveyConfigurationList = sms.GetSurveyConfiguration_vwWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (SurveyConfigurationList != null && SurveyConfigurationList.FirstOrDefault().Key > 0)
                {
                    IList<SurveyConfiguration_vw> SurveyConfigurationListDetails = SurveyConfigurationList.FirstOrDefault().Value;
                    long totalRecords = SurveyConfigurationList.FirstOrDefault().Key;
                    int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                    var jsonData = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalRecords,
                        rows =
                        (
                        from items in SurveyConfigurationListDetails
                        select new
                        {
                            i = items.Id,
                            cell = new string[]
                           {
                                items.Id.ToString(), 
                                items.AcademicYear,
                                items.Campus,
                                items.Grade,    
                                items.Section,
                                items.SurveyNumber,
                                //items.SurveyGroupMaster.SurveyGroup,                                                                
                                items.IsActive==true?"Yes":"No",
                                //items.CreatedBy,
                                //items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd'/'MM'/'yyyy"):"",
                                //items.ModifiedBy,
                                //items.ModifiedDate!=null?items.ModifiedDate.Value.ToString("dd'/'MM'/'yyyy"):""
                           }
                        }
                        )
                    };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var Empty = new { rows = (new { cell = new string[] { } }) };
                    return Json(Empty, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                //ExceptionPolicy.HandleException(ex, "MasterPolicy");
                throw ex;
            }
        }
        public ActionResult AddSurveyConfiguration(SurveyConfiguration surveyconfig, string SurveyGroupId)
        {
            try
            {
                IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                string userId = ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    StaffManagementService sms = new StaffManagementService();
                    IList<SurveyConfiguration> SurveyConfigList = new List<SurveyConfiguration>();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<string, object> likeCriteria = new Dictionary<string, object>();
                    string[] ArrCampus = surveyconfig.Campus.Split(',');
                    string[] ArrGrade = surveyconfig.Grade.Split(',');
                    string[] ArrSurveygrp = SurveyGroupId.Split(',');
                    criteria.Add("AcademicYear", surveyconfig.AcademicYear);
                    criteria.Add("IsActive", true);
                    foreach (var items in ArrCampus)
                    {
                        if (!string.IsNullOrEmpty(items))
                        {
                            criteria.Add("Campus", items);
                            foreach (var Gradeitems in ArrGrade)
                            {
                                if (!string.IsNullOrEmpty(Gradeitems))
                                {
                                    criteria.Add("Grade", Gradeitems);
                                    MastersService ms = new MastersService();
                                    Dictionary<long, IList<CampusWiseSectionMaster_vw>> CampusWiseSectionMasterList = ms.GetCampusWiseSectionMaster_vwListWithExactAndLikeSearchCriteriaWithCount(0, 999999, null, null, criteria, likeCriteria);
                                    if (CampusWiseSectionMasterList != null && CampusWiseSectionMasterList.FirstOrDefault().Key > 0)
                                    {
                                        foreach (var sectionitem in CampusWiseSectionMasterList.FirstOrDefault().Value)
                                        {
                                            foreach (var groupid in ArrSurveygrp)
                                            {
                                                if (!string.IsNullOrEmpty(groupid))
                                                {
                                                    criteria.Add("SurveyGroupMaster.SurveyGroupId", Convert.ToInt64(groupid));
                                                    criteria.Add("SurveyNumber", surveyconfig.SurveyNumber);
                                                    Dictionary<long, IList<SurveyConfiguration>> SurveyConfigurationList = sms.GetSurveyConfigurationListWithPagingAndCriteria(null, null, null, null, criteria, likeCriteria);
                                                    criteria.Remove("SurveyGroupMaster.SurveyGroupId");
                                                    criteria.Remove("SurveyNumber");
                                                    if (SurveyConfigurationList.FirstOrDefault().Key == 0)
                                                    {
                                                        SurveyConfiguration surveyconfiguration = new SurveyConfiguration();
                                                        surveyconfiguration.Campus = items;
                                                        surveyconfiguration.Grade = Gradeitems;
                                                        surveyconfiguration.AcademicYear = surveyconfig.AcademicYear;
                                                        SurveyGroupMaster sgm = new SurveyGroupMaster();
                                                        sgm.SurveyGroupId = Convert.ToInt64(groupid);
                                                        surveyconfiguration.SurveyGroupMaster = sgm;
                                                        surveyconfiguration.Section = sectionitem.Section;
                                                        surveyconfiguration.SurveyNumber = surveyconfig.SurveyNumber;
                                                        surveyconfiguration.IsActive = true;
                                                        surveyconfiguration.CreatedBy = userId;
                                                        surveyconfiguration.CreatedDate = DateTime.Now;
                                                        SurveyConfigList.Add(surveyconfiguration);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        return Json("failed", JsonRequestBehavior.AllowGet);
                                    }
                                    criteria.Remove("Grade");
                                }
                            }
                            criteria.Remove("Campus");
                        }
                    }
                    if (SurveyConfigList.Count > 0)
                    {
                        sms.SaveOrUpdateSurveyConfigurationByList(SurveyConfigList);
                        return Json("success", JsonRequestBehavior.AllowGet);
                    }
                    return Json(null, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        #region Staff Programme Master
        public ActionResult StaffProgrammeMaster()
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
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "MasterPolicy");
                throw;
            }
        }
        public ActionResult StaffProgrammeMasterJqgrid(StaffProgrammeMaster spm, string Campus, string StaffType, string ProgrammName, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                sord = sord == "desc" ? "Desc" : "Asc";
                criteria.Add("IsActive", true);
                if (!string.IsNullOrEmpty(spm.Campus)) { criteria.Add("Campus", spm.Campus); }
                if (!string.IsNullOrEmpty(spm.StaffType)) { criteria.Add("StaffType", spm.StaffType); }
                if (!string.IsNullOrEmpty(spm.ProgrammeName)) { criteria.Add("ProgrammeName", spm.ProgrammeName); }
                Dictionary<long, IList<StaffProgrammeMaster>> StaffProgrammeMasterList = sms.GetStaffProgrammeMasterListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (StaffProgrammeMasterList != null && StaffProgrammeMasterList.Count > 0)
                {
                    long totalrecords = StaffProgrammeMasterList.First().Key;
                    int totalpages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalpages,
                        page = page,
                        records = totalrecords,
                        rows = (from items in StaffProgrammeMasterList.FirstOrDefault().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[]{
                                          items.StaffProgrammeMatserId.ToString(),
                                          items.Campus,
                                          items.StaffType,
                                          items.ProgrammeName, 
                                          items.CreatedBy,
                                          items.CreatedDate!=null?items.CreatedDate.ToString("dd/MM/yyyy"):"",
                                          items.ModifiedBy,
                                          items.ModifiedDate!=null?items.ModifiedDate.ToString("dd/MM/yyyy"):""
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
        public ActionResult SaveOrUpdateStaffProgrammeMaster(StaffProgrammeMaster spm)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("logOut", "Account");
                else
                {

                    StaffManagementService sms = new StaffManagementService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    StaffProgrammeMaster spmObj = new StaffProgrammeMaster();
                    //criteria.Add("Campus", spm.Campus);
                    //criteria.Add("StaffType", spm.StaffType);
                    //criteria.Add("ProgrammeName", spm.ProgrammeName);
                    //criteria.Add("IsActive", true);

                    spmObj = sms.GetStaffProgrammeMasterByCampusAndStaffType(spm.Campus, spm.StaffType, spm.ProgrammeName, true);
                    var script = "";
                    if (spm.StaffProgrammeMatserId == 0)
                    {

                        if (spmObj == null)
                        {
                            spm.IsActive = true;
                            spm.CreatedBy = userId;
                            spm.CreatedDate = DateTime.Now;
                            spm.ModifiedBy = userId;
                            spm.ModifiedDate = DateTime.Now;
                            sms.CreateOrUpdateStaffProgrammeMaster(spm);
                            return JavaScript(script);
                        }
                        else
                        {
                            script = @"ErrMsg(""The Name is already exist!"");";
                            return JavaScript(script);

                        }
                    }
                    else
                    {
                        if (spmObj == null)
                        {
                            spm.IsActive = true;
                            spm.CreatedBy = userId;
                            spm.CreatedDate = DateTime.Now;
                            spm.ModifiedBy = userId;
                            spm.ModifiedDate = DateTime.Now;
                            sms.CreateOrUpdateStaffProgrammeMaster(spm);
                            script = @"SucessMsg(""Updated Sucessfully"");";
                            return JavaScript(script);
                        }
                        else
                        {
                            script = @"ErrMsg(""The Name is already exist!"");";
                            return JavaScript(script);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "MasterPolicy");
                throw ex;
            }
        }
        public ActionResult DeleteStaffProgrammeMaster(string[] Id)
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();
                IList<StaffProgrammeMaster> StaffProgrammeMasterList = new List<StaffProgrammeMaster>();
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("Logout", "Account");
                else
                {
                    if (Id == null)
                        return null;
                    string[] CityIdArray = Id[0].Split(',');

                    long[] longCityIdArray = new long[CityIdArray.Length];
                    StaffProgrammeMaster spm = new StaffProgrammeMaster();
                    for (int j = 0; j < CityIdArray.Length; j++)
                    {

                        spm = sms.GetStaffProgrammeMasterByStaffProgrammeMasterId(Convert.ToInt64(CityIdArray[j]));
                        spm.IsActive = false;
                        StaffProgrammeMasterList.Add(spm);

                    }

                    sms.SaveOrUpdateStaffProgrammeMasterByList(StaffProgrammeMasterList);
                    var script = "";
                    return JavaScript(script);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "MasterPolicy");
                throw;
            }
        }
        #endregion
        #region SurveyMaster_Krishna_13062017
        public ActionResult SurveyMaster()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    DateTime dttime = DateTime.Now;
                    ViewBag.acadddl = GetAcademicYear();
                    ViewBag.dattime = dttime.ToString("dd/MM/yyyy");

                    #region BreadCrumb
                    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                    #endregion

                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AttendancePolicy");
                throw ex;
            }
        }
        public ActionResult SurveyMasterGridLoad(SurveyMaster objSurveyMaster, string IsActive, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                sord = sord == "desc" ? "Desc" : "Asc";

                if (!string.IsNullOrEmpty(objSurveyMaster.SurveyName))
                    criteria.Add("SurveyName", objSurveyMaster.SurveyName);

                if (!string.IsNullOrEmpty(IsActive) && IsActive != "Select One")
                    criteria.Add("IsActive", objSurveyMaster.IsActive);

                Dictionary<long, IList<SurveyMaster>> objSurveyMasterList = smsObj.GetSurveyListWithPaging(page - 1, rows, sidx, sord, criteria);

                if (objSurveyMasterList != null && objSurveyMasterList.FirstOrDefault().Key > 0)
                {
                    long totalrecords = objSurveyMasterList.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,

                        rows = (from items in objSurveyMasterList.First().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[]{
                                       items.SurveyId.ToString(),  
                                       items.SurveyName,
                                       items.IsActive==true?"Yes":"No", 
                                       items.CreatedBy, 
                                       items.CreatedDate!=null ? items.CreatedDate.Value.ToString("dd'/'MM'/'yyyy"): "",
                                       }
                                })

                    };
                    return Json(jsondat, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var Empty = new { rows = (new { cell = new string[] { } }) };
                    return Json(Empty, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult AddSurveyMaster(SurveyMaster objSurveyMaster)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    SurveyMaster objlSurveyMaster = smsObj.GetSurveyName(objSurveyMaster.SurveyName);
                    if (objlSurveyMaster != null)
                    {
                        var script = @"ErrMsg(""Already Exist"");";
                        return JavaScript(script);
                    }
                    objSurveyMaster.CreatedBy = userId;
                    objSurveyMaster.CreatedDate = DateTime.Now;
                    objSurveyMaster.IsActive = true;
                    smsObj.CreateOrUpdateSurveyMaster(objSurveyMaster);
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult EditSurveyMaster(SurveyMaster objSurveyMaster)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (objSurveyMaster.SurveyId > 0)
                    {
                        SurveyMaster objSurveyMasterName = smsObj.GetSurveyName(objSurveyMaster.SurveyName);
                        if (objSurveyMasterName != null)
                        {
                            var script = @"ErrMsg(""Already Exist"");";
                            return JavaScript(script);
                        }
                        SurveyMaster objSurveyMasterId = smsObj.GetSurveyId(objSurveyMaster.SurveyId);
                        if (objSurveyMasterId != null)
                        {
                            objSurveyMasterId.SurveyName = objSurveyMaster.SurveyName;
                            objSurveyMasterId.ModifiedBy = userId;
                            objSurveyMasterId.ModifiedDate = DateTime.Now;
                            smsObj.CreateOrUpdateSurveyMaster(objSurveyMasterId);
                        }
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult DeleteSurveyMaster(string Id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    string[] arrayId = Id.Split(',');
                    long[] lSurveyId = Array.ConvertAll(arrayId, Int64.Parse);
                    smsObj.DeleteSurveyMaster(lSurveyId);
                    var script = @"SucessMsg(""Deleted  Successfully"");";
                    return JavaScript(script);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        #endregion
        #region  New Survey Answer Master

        public ActionResult AddNewSurveyAnswerMaster(long? SurveyQuestionId)
        {
            try
            {
                ViewBag.SurveyQuestionId = SurveyQuestionId;
                return View();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw;
            }
        }
        public ActionResult GetJqGridAddNewsurveyanswerList(SurveyAnswerMaster survey, long? SurveyQuestionId, int rows, string sord, string sidx, int? page = 1)
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Clear();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (!string.IsNullOrEmpty(survey.SurveyAnswer)) { criteria.Add("SurveyAnswer", survey.SurveyAnswer); }
                if (survey.SurveyMark > 0) { criteria.Add("SurveyMark", survey.SurveyMark); }

                criteria.Add("SurveyQuestionMaster.SurveyQuestionId", SurveyQuestionId);
                Dictionary<long, IList<SurveyAnswerMaster>> SurveyAnswerObj = sms.GetSurveyAnswerMasterWithPagingAndCriteria(page - 1, rows, sord, sidx, criteria);
                if (SurveyAnswerObj != null && SurveyAnswerObj.FirstOrDefault().Key > 0)
                {
                    IList<SurveyAnswerMaster> SurveyAnswerDetails = SurveyAnswerObj.FirstOrDefault().Value;
                    long totalRecords = SurveyAnswerObj.FirstOrDefault().Key;
                    int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                    var jsonData = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalRecords,
                        rows =
                        (
                        from items in SurveyAnswerDetails
                        select new
                        {
                            i = items.SurveyAnswerId,
                            cell = new string[]
                           {
                                items.SurveyAnswerId.ToString(),
                                items.SurveyQuestionMaster.SurveyQuestion,  
                                items.SurveyAnswer,
                                items.SurveyMark.ToString(),                             
                                items.IsPositive==true?"Yes":"No",
                                items.IsActive==true?"Yes":"No" ,
                                items.CreatedBy,
                                items.CreatedDate!=null?items.CreatedDate.ToString("dd/MM/yyyy"):"",
                                items.ModifiedBy,
                                items.ModifiedDate!=null?items.ModifiedDate.ToString("dd/MM/yyyy"):"",
                           }
                        }
                        )
                    };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var Empty = new { rows = (new { cell = new string[] { } }) };
                    return Json(Empty, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult NewSurveyAnswerMaster()
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
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw;
            }
        }
        public ActionResult GetJqGridNewsurveyanswerList(SurveyAnswerMaster survey, string IsActive, string IsPositive, long? SurveyQuestionId, int rows, string sord, string sidx, int? page = 1)
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Clear();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (!string.IsNullOrEmpty(survey.SurveyAnswer)) { criteria.Add("SurveyAnswer", survey.SurveyAnswer); }
                if (survey.SurveyMark > 0) { criteria.Add("SurveyMark", survey.SurveyMark); }
                if (!string.IsNullOrEmpty(IsActive))
                {
                    if (IsActive == "true" || IsActive == "True")
                    {
                        criteria.Add("IsActive", true);
                    }
                    if (IsActive == "false" || IsActive == "False")
                    {
                        criteria.Add("IsActive", false);
                    }
                }
                if (!string.IsNullOrEmpty(IsPositive))
                {
                    if (IsPositive == "true" || IsPositive == "True")
                    {
                        criteria.Add("IsPositive", true);
                    }
                    if (IsPositive == "false" || IsPositive == "False")
                    {
                        criteria.Add("IsPositive", false);
                    }
                }
                criteria.Add("SurveyQuestionMaster.SurveyQuestionId", SurveyQuestionId);
                Dictionary<long, IList<SurveyAnswerMaster>> SurveyAnswerObj = sms.GetSurveyAnswerMasterWithPagingAndCriteria(page - 1, rows, sord, sidx, criteria);
                if (SurveyAnswerObj != null && SurveyAnswerObj.FirstOrDefault().Key > 0)
                {
                    IList<SurveyAnswerMaster> SurveyAnswerDetails = SurveyAnswerObj.FirstOrDefault().Value;
                    long totalRecords = SurveyAnswerObj.FirstOrDefault().Key;
                    int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                    var jsonData = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalRecords,
                        rows =
                        (
                        from items in SurveyAnswerDetails
                        select new
                        {
                            i = items.SurveyAnswerId,
                            cell = new string[]
                           {
                                items.SurveyAnswerId.ToString(),
                                items.SurveyQuestionMaster.SurveyQuestion,  
                                items.SurveyAnswer,
                                items.SurveyMark.ToString(),                             
                                items.IsPositive==true?"Yes":"No",
                                items.IsActive==true?"Yes":"No" ,
                                items.CreatedBy,
                                items.CreatedDate!=null?items.CreatedDate.ToString("dd/MM/yyyy"):"",
                                items.ModifiedBy,
                                items.ModifiedDate!=null?items.ModifiedDate.ToString("dd/MM/yyyy"):"",
                               "<img src='/Images/History.png ' id='ImgHistory' onclick=\"AddAnswerDetails( '" + items.SurveyQuestionMaster.SurveyQuestionId +"');\" />",
                           }
                        }
                        )
                    };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var Empty = new { rows = (new { cell = new string[] { } }) };
                    return Json(Empty, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        #endregion
        public ActionResult GetSurveyNameddl()
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("IsActive", true);
                Dictionary<long, IList<SurveyMaster>> SurveyMasterList = smsObj.GetSurveyListWithPaging(0, 9999, "SurveyName", "Asc", criteria);
                var surveyname = (from items in SurveyMasterList.FirstOrDefault().Value
                            select new
                            {
                                Text = items.SurveyName,
                                Value = items.SurveyId
                            }).Distinct().ToList();
                return Json(surveyname, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "MasterPolicy");
                throw ex;
            }
        }
        //#region Staff_WorkingDaysMaster
        //public ActionResult StaffWorkingDaysMaster()
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //        else
        //        {
        //            #region BreadCrumb
        //            string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //            ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
        //            #endregion
        //            return View();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
        //        throw;
        //    }
        //}
        //public ActionResult StaffWorkingDaysMasterGridListJqGrid(Staff_WorkingDaysMaster swdm, string ExportType, int rows, string sidx, string sord, int? page = 1)
        //{
        //    try
        //    {
        //        Dictionary<string, object> criteria = new Dictionary<string, object>();
        //        sord = sord == "desc" ? "Desc" : "Asc";
        //        if (!string.IsNullOrEmpty(swdm.Campus))
        //            criteria.Add("Campus", swdm.Campus);
        //        if (!string.IsNullOrEmpty(swdm.StaffType))
        //            criteria.Add("StaffType", swdm.StaffType);
        //        if (swdm.Month > 0)
        //            criteria.Add("Month", swdm.Month);
        //        if (swdm.Year > 0)
        //            criteria.Add("Year", swdm.Year);
        //        Dictionary<long, IList<Staff_WorkingDaysMaster>> Staff_WorkingDaysMasterDetails = smsObj.GetStaff_WorkingDaysMasterListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
        //        if (ExportType == "Excel")
        //        {
        //            base.ExptToXL(Staff_WorkingDaysMasterDetails.FirstOrDefault().Value, "StaffWorkingDyasMaserReport" + "-On-" + DateTime.Today.ToString("dd/MM/yyyy"), (items => new
        //            {
        //                items.Campus,
        //                items.StaffType,
        //                Month = items.Month.ToString(),
        //                Year = items.Year.ToString(),
        //                NoOfworkingDays = items.NoOfworkingDays.ToString()
        //            }));
        //            return new EmptyResult();
        //        }
        //        else if (Staff_WorkingDaysMasterDetails != null && Staff_WorkingDaysMasterDetails.FirstOrDefault().Key > 0)
        //        {
        //            long totalrecords = Staff_WorkingDaysMasterDetails.First().Key;
        //            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
        //            var jsondat = new
        //            {
        //                total = totalPages,
        //                page = page,
        //                records = totalrecords,
        //                rows = (from items in Staff_WorkingDaysMasterDetails.First().Value
        //                        select new
        //                        {
        //                            i = 2,
        //                            cell = new string[]{
        //                               items.Staff_WorkingDaysMaster_Id.ToString(),                                                                                                                     
        //                               items.Campus,                                       
        //                               items.StaffType,
        //                               items.Month.ToString(),
        //                               items.Year.ToString(),
        //                               items.NoOfworkingDays.ToString(),
        //                               items.CreatedBy,
        //                               items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy"):string.Empty,
        //                               items.ModifiedBy,
        //                               items.ModifiedDate!=null?items.ModifiedDate.Value.ToString("dd/MM/yyyy"):string.Empty,
        //                               }
        //                        })

        //            };
        //            return Json(jsondat, JsonRequestBehavior.AllowGet);
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
        //public ActionResult FillMonthDdl()
        //{
        //    IList<long> MonthList = new List<long>();
        //    for (int i = 1; i <= 12; i++)
        //        MonthList.Add(i);
        //    if (MonthList != null && MonthList.Count > 0)
        //    {
        //        var MonthLst = (
        //                 from items in MonthList
        //                 select new
        //                 {
        //                     Text = items,
        //                     Value = items
        //                 }).Distinct().ToList().OrderBy(x => x.Text);
        //        return Json(MonthLst, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json(null, JsonRequestBehavior.AllowGet);
        //    }
        //}
        //public ActionResult FillYearDdl()
        //{
        //    long CurrentYear = DateTime.Now.Year;
        //    IList<long> YearList = new List<long>();
        //    for (long i = CurrentYear - 1; i < CurrentYear + 10; i++)
        //        YearList.Add(i);
        //    if (YearList != null && YearList.Count > 0)
        //    {
        //        var YearLst = (
        //                 from items in YearList
        //                 select new
        //                 {
        //                     Text = items,
        //                     Value = items
        //                 }).Distinct().ToList().OrderBy(x => x.Text);
        //        return Json(YearLst, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json(null, JsonRequestBehavior.AllowGet);
        //    }
        //}
        ////public ActionResult SaveOrUpdateStaffWorkingDaysMaster(Staff_WorkingDaysMaster workingMasterObj)
        ////{
        ////    try
        ////    {
        ////        var script = "";
        ////        string userId = base.ValidateUser();
        ////        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("logOut", "Account");
        ////        else
        ////        {
        ////            Staff_WorkingDaysMaster ExistingObj = new Staff_WorkingDaysMaster();
        ////            if (workingMasterObj.Staff_WorkingDaysMaster_Id == 0)
        ////            {
        ////                ExistingObj = smsObj.GetStaff_WorkingDaysMasterByCampusAndStaffType(workingMasterObj.Campus, workingMasterObj.StaffType, workingMasterObj.Month, workingMasterObj.Year);
        ////                if (ExistingObj == null)
        ////                {
        ////                    workingMasterObj.CreatedBy = userId;
        ////                    workingMasterObj.CreatedDate = DateTime.Now;
        ////                    smsObj.SaveOrUpdateStaff_WorkingDaysMaster(workingMasterObj);
        ////                    script = @"SucessMsg(""Added Sucessfully"");";
        ////                }
        ////                else
        ////                    script = @"ErrMsg(""The configuration is already exist!"");";
        ////            }
        ////            else
        ////            {
        ////                ExistingObj = smsObj.GetStaff_WorkingDaysMasterByStaff_WorkingDaysMaster_Id(workingMasterObj.Staff_WorkingDaysMaster_Id);
        ////                if (ExistingObj != null)
        ////                {
        ////                    workingMasterObj.ModifiedBy = userId;
        ////                    workingMasterObj.ModifiedDate = DateTime.Now;
        ////                    smsObj.SaveOrUpdateStaff_WorkingDaysMaster(workingMasterObj);
        ////                    script = @"SucessMsg(""Updated Sucessfully"");";
        ////                }
        ////                else
        ////                    script = @"ErrMsg(""The configuration is already exist!"");";
        ////            }
        ////        }
        ////        return JavaScript(script);
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
        ////        throw ex;
        ////    }
        ////}
        //public ActionResult SaveOrUpdateStaffWorkingDaysMaster(Staff_WorkingDaysMaster workingMasterObj, string Month)
        //{
        //    try
        //    {
        //        IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
        //        string userId = ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //        else
        //        {
        //            StaffManagementService sms = new StaffManagementService();
        //            IList<Staff_WorkingDaysMaster> StaffWorkingDaysMaster = new List<Staff_WorkingDaysMaster>();
        //            IList<Staff_WorkingDaysMaster> StaffWorkingDaysMaster1 = new List<Staff_WorkingDaysMaster>();
        //            Dictionary<string, object> criteria = new Dictionary<string, object>();
        //            var script = "";
        //            criteria.Add("Year", workingMasterObj.Year);
        //            string[] ArrCampus = workingMasterObj.Campus.Split(',');
        //            string[] ArrStaffType = workingMasterObj.StaffType.Split(',');
        //            string[] ArrMonth = Month.Split(',');
        //            // criteria.Add("StudentSurveyGroup", workingMasterObj.StudentSurveyGroup);
        //            // criteria.Add("IsActive", true);
        //            foreach (var items in ArrCampus)
        //            {
        //                if (!string.IsNullOrEmpty(items))
        //                {
        //                    criteria.Add("Campus", items);
        //                    foreach (var StaffTypeitems in ArrStaffType)
        //                    {
        //                        if (!string.IsNullOrEmpty(StaffTypeitems))
        //                        {
        //                            criteria.Add("StaffType", StaffTypeitems);
        //                            foreach (var Monthitem in ArrMonth)
        //                            {
        //                                if (!string.IsNullOrEmpty(Monthitem))
        //                                {
        //                                    criteria.Add("Month", Convert.ToInt64(Monthitem));
        //                                    Dictionary<long, IList<Staff_WorkingDaysMaster>> staff_WorkingDaysMaster = smsObj.GetStaff_WorkingDaysMasterListWithPagingAndCriteria(null, null, null, null, criteria);
        //                                    criteria.Remove("Month");
        //                                    if (staff_WorkingDaysMaster.FirstOrDefault().Key == 0)
        //                                    {
        //                                        Staff_WorkingDaysMaster swdm = new Staff_WorkingDaysMaster();
        //                                        swdm.Campus = items;
        //                                        swdm.StaffType = StaffTypeitems;
        //                                        swdm.Month = Convert.ToInt64(Monthitem);
        //                                        swdm.Year = workingMasterObj.Year;
        //                                        swdm.NoOfworkingDays = workingMasterObj.NoOfworkingDays;
        //                                        swdm.CreatedBy = userId;
        //                                        swdm.CreatedDate = DateTime.Now;
        //                                        swdm.ModifiedBy = userId;
        //                                        swdm.ModifiedDate = DateTime.Now;
        //                                        StaffWorkingDaysMaster.Add(swdm);
        //                                    }

        //                                }
        //                            }
        //                            criteria.Remove("StaffType");
        //                        }
        //                    }
        //                    criteria.Remove("Campus");
        //                }
        //            }
        //            if (StaffWorkingDaysMaster.Count > 0)
        //            {
        //                sms.SaveOrUpdateStaff_WorkingDaysMasterByList(StaffWorkingDaysMaster);
        //                script = @"SucessMsg(""Added Sucessfully"");";
        //                return JavaScript(script);
        //            }

        //            return Json(null, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
        //        throw ex;
        //    }
        //}
        //public ActionResult UpdateStaffWorkingDaysMaster(Staff_WorkingDaysMaster StaffWorkingDaysMaster)
        //{
        //    try
        //    {
        //        var script = "";
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //        else
        //        {

        //            Staff_WorkingDaysMaster Obj = new Staff_WorkingDaysMaster();
        //            StaffManagementService smsObj = new StaffManagementService();
        //            Obj = smsObj.GetStaff_WorkingDaysMasterByStaff_WorkingDaysMaster_Id(StaffWorkingDaysMaster.Staff_WorkingDaysMaster_Id);
        //            if (Obj != null)
        //            {
        //                StaffWorkingDaysMaster.ModifiedBy = userId;
        //                StaffWorkingDaysMaster.ModifiedDate = DateTime.Now;
        //                smsObj.SaveOrUpdateStaff_WorkingDaysMaster(StaffWorkingDaysMaster);
        //                script = @"SucessMsg(""Updated Sucessfully"");";
        //                return JavaScript(script);
        //            }
        //            else
        //            {
        //                script = @"ErrMsg(""The configuration is already exist!"");";
        //                return JavaScript(script);
        //            }
        //        }

        //        return Json(null, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
        //        throw ex;
        //    }
        //}
        //public ActionResult DeleteStaff_WorkingDaysMaster(string[] Id)
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("logOut", "Account");
        //        else
        //        {
        //            int i;
        //            var script = "";
        //            string[] arrayId = Id[0].Split(',');
        //            long[] WaivedDetails_Ids = new long[arrayId.Length];
        //            for (i = 0; i < arrayId.Length; i++)
        //            {
        //                if (!string.IsNullOrEmpty(arrayId[i]))
        //                    WaivedDetails_Ids[i] = Convert.ToInt64(arrayId[i]);
        //            }
        //            if (WaivedDetails_Ids != null && WaivedDetails_Ids.Length > 0)
        //            {
        //                smsObj.DeleteStaff_WorkingDaysMaster(WaivedDetails_Ids);
        //                script = @"SucessMsg(""Deleted Successfully"");";
        //            }
        //            else
        //                script = @"ErrMsg(""Deleted UnSuccessfully"");";
        //            return JavaScript(script);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
        //        throw ex;
        //    }
        //}
        //#endregion
        #region Staff_WorkingDaysMaster
        public ActionResult StaffWorkingDaysMaster()
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
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw;
            }
        }
        public ActionResult StaffWorkingDaysMasterGridListJqGrid(Staff_WorkingDaysMaster swdm, string ExportType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (!string.IsNullOrEmpty(swdm.Campus))
                    criteria.Add("Campus", swdm.Campus);
                if (!string.IsNullOrEmpty(swdm.StaffType))
                    criteria.Add("StaffType", swdm.StaffType);
                if (swdm.Month > 0)
                    criteria.Add("Month", swdm.Month);
                if (swdm.Year > 0)
                    criteria.Add("Year", swdm.Year);
                Dictionary<long, IList<Staff_WorkingDaysMaster>> Staff_WorkingDaysMasterDetails = smsObj.GetStaff_WorkingDaysMasterListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (ExportType == "Excel")
                {
                    base.ExptToXL(Staff_WorkingDaysMasterDetails.FirstOrDefault().Value, "StaffWorkingDyasMaserReport" + "-On-" + DateTime.Today.ToString("dd/MM/yyyy"), (items => new
                    {
                        items.Campus,
                        items.StaffType,
                        Month = items.Month.ToString(),
                        Year = items.Year.ToString(),
                        NoOfworkingDays = items.NoOfworkingDays.ToString()
                    }));
                    return new EmptyResult();
                }
                else if (Staff_WorkingDaysMasterDetails != null && Staff_WorkingDaysMasterDetails.FirstOrDefault().Key > 0)
                {
                    long totalrecords = Staff_WorkingDaysMasterDetails.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,
                        rows = (from items in Staff_WorkingDaysMasterDetails.First().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[]{
                                       items.Staff_WorkingDaysMaster_Id.ToString(),                                                                                                                     
                                       items.Campus,                                       
                                       items.StaffType,
                                       "All",
                                       "All",
                                       items.NoOfworkingDays.ToString(),
                                       items.CreatedBy,
                                       items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy"):string.Empty,
                                       items.ModifiedBy,
                                       items.ModifiedDate!=null?items.ModifiedDate.Value.ToString("dd/MM/yyyy"):string.Empty,
                                       }
                                })

                    };
                    return Json(jsondat, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var Empty = new { rows = (new { cell = new string[] { } }) };
                    return Json(Empty, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult FillMonthDdl()
        {
            IList<long> MonthList = new List<long>();
            for (int i = 1; i <= 12; i++)
                MonthList.Add(i);
            if (MonthList != null && MonthList.Count > 0)
            {
                var MonthLst = (
                         from items in MonthList
                         select new
                         {
                             Text = items,
                             Value = items
                         }).Distinct().ToList().OrderBy(x => x.Text);
                return Json(MonthLst, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult FillYearDdl()
        {
            long CurrentYear = DateTime.Now.Year;
            IList<long> YearList = new List<long>();
            for (long i = CurrentYear - 1; i < CurrentYear + 10; i++)
                YearList.Add(i);
            if (YearList != null && YearList.Count > 0)
            {
                var YearLst = (
                         from items in YearList
                         select new
                         {
                             Text = items,
                             Value = items
                         }).Distinct().ToList().OrderBy(x => x.Text);
                return Json(YearLst, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SaveOrUpdateStaffWorkingDaysMaster(Staff_WorkingDaysMaster workingMasterObj)
        {
            try
            {
                var script = "";
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("logOut", "Account");
                else
                {
                    Staff_WorkingDaysMaster ExistingObj = new Staff_WorkingDaysMaster();
                    if (workingMasterObj.Staff_WorkingDaysMaster_Id == 0)
                    {
                        ExistingObj = smsObj.GetStaff_WorkingDaysMasterByCampusAndStaffType(workingMasterObj.Campus, workingMasterObj.StaffType, workingMasterObj.Month, workingMasterObj.Year);
                        if (ExistingObj == null)
                        {
                            workingMasterObj.CreatedBy = userId;
                            workingMasterObj.CreatedDate = DateTime.Now;
                            smsObj.SaveOrUpdateStaff_WorkingDaysMaster(workingMasterObj);
                            script = @"SucessMsg(""Added Sucessfully"");";
                        }
                        else
                            script = @"ErrMsg(""The configuration is already exist!"");";
                    }
                    else
                    {
                        ExistingObj = smsObj.GetStaff_WorkingDaysMasterByStaff_WorkingDaysMaster_Id(workingMasterObj.Staff_WorkingDaysMaster_Id);
                        if (ExistingObj != null)
                        {
                            workingMasterObj.ModifiedBy = userId;
                            workingMasterObj.ModifiedDate = DateTime.Now;
                            smsObj.SaveOrUpdateStaff_WorkingDaysMaster(workingMasterObj);
                            script = @"SucessMsg(""Updated Sucessfully"");";
                        }
                        else
                            script = @"ErrMsg(""The configuration is already exist!"");";
                    }
                }
                return JavaScript(script);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }        
        public ActionResult DeleteStaff_WorkingDaysMaster(string[] Id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("logOut", "Account");
                else
                {
                    int i;
                    var script = "";
                    string[] arrayId = Id[0].Split(',');
                    long[] WaivedDetails_Ids = new long[arrayId.Length];
                    for (i = 0; i < arrayId.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(arrayId[i]))
                            WaivedDetails_Ids[i] = Convert.ToInt64(arrayId[i]);
                    }
                    if (WaivedDetails_Ids != null && WaivedDetails_Ids.Length > 0)
                    {
                        smsObj.DeleteStaff_WorkingDaysMaster(WaivedDetails_Ids);
                        script = @"SucessMsg(""Deleted Successfully"");";
                    }
                    else
                        script = @"ErrMsg(""Deleted UnSuccessfully"");";
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
        #region Staff Attendance Change Details john naveen
        public ActionResult StaffAttendanceChangeDetails()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AttendancePolicy");
                throw ex;
            }
        }
        public ActionResult StaffAttendanceChangeDetailsGridListJqGrid(Staff_AttendanceChangeDetails sacd, string ExportType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                sord = sord == "desc" ? "Desc" : "Asc";
                StaffManagementService StaffManagementService = new StaffManagementService();
                Dictionary<long, IList<Staff_AttendanceChangeDetails>> Staff_AttendanceChangeDetails = StaffManagementService.GetStaff_AttendanceChangeDetailsListWithCriteria(page - 1, rows, sidx, sord, criteria);
                if (Staff_AttendanceChangeDetails != null && Staff_AttendanceChangeDetails.FirstOrDefault().Key > 0)
                {
                    long totalrecords = Staff_AttendanceChangeDetails.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,
                        rows = (from items in Staff_AttendanceChangeDetails.First().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[]{
                                       items.Staff_AttendanceChangeDetails_Id.ToString(),                                                                                                                     
                                       items.PreRegNum.ToString(),        
                                       items.Month.ToString(),
                                       items.Year.ToString(),
                                       items.AllowedCasualLeaves.ToString(),
                                       items.TotalNoOfDaysWorkedByLogs.ToString(),
                                       items.TotalNoOfDaysWorkedByChange.ToString(),
                                       items.TotalNoOfLeavesTakenByLogs.ToString(),
                                       items.TotalNoOfLeavesTakenByChange.ToString(),
                                       items.CreatedBy,
                                       items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy"):string.Empty,
                                       items.ModifiedBy,
                                       items.ModifiedDate!=null?items.ModifiedDate.Value.ToString("dd/MM/yyyy"):string.Empty,
                                       items.IsActive==true?"Yes":"No",
                                       items.OnDuty.ToString(),
                                       items.NoOfPermissionsTaken.ToString(),
                                       items.NoOfLeavesCalculatedByPermissions.ToString(),
                                       items.NoOfHolidays.ToString(),
                                       items.CasualLeavesTaken.ToString(),
                                       items.NoOfLeaveToBeCalculated.ToString()
                                       }
                                })

                    };
                    return Json(jsondat, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var Empty = new { rows = (new { cell = new string[] { } }) };
                    return Json(Empty, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AttendancePolicy");
                throw ex;
            }
        }
        public ActionResult SaveStaffAttendanceChangeDetails(Staff_AttendanceChangeDetails StaffAttendanceChangeDetails, Staff_AttendanceCLDetails StaffAttendanceClDetails, CLDetailsMaster clMaster,
          decimal TotalNoOfDaysWorkedByChange, decimal TotalNoOfLeavesTakenByChange, long PreRegNum,
          long TotalNoOfLeavesTakenByLogs, long TotalNoOfDaysWorkedByLogs, long AttendanceMonth, long AttendanceYear, decimal? OnDuty,
          decimal? NoOfPermissionsTaken, decimal? NoOfLeavesCalculatedByPermissions, long NoOfHolidays,
          decimal? OpeningBalance, decimal AllotedCl, decimal? ClosingBalance, decimal? LeaveToBeCalculated, decimal TotalAvailableBalance, string Remarks)
        {
            try
            {
                StaffManagementService StaffManagementService = new StaffManagementService();
                var script = "";
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("logOut", "Account");
                else
                {
                    Staff_AttendanceChangeDetails objStaffAttendanceChangeDetails = new Staff_AttendanceChangeDetails();
                    objStaffAttendanceChangeDetails = StaffManagementService.GetStaff_AttendanceChangeDetailsByPreRegNumAndMonthYear(StaffAttendanceChangeDetails.PreRegNum, AttendanceMonth, AttendanceYear, true);
                    Staff_AttendanceCLDetails StaffAttendanceClDetailsObj = new Staff_AttendanceCLDetails();
                    StaffAttendanceClDetailsObj = StaffManagementService.GetStaff_AttendanceCLDetailsByPreRegNum(AttendanceMonth, AttendanceYear, PreRegNum, true);
                    CLDetailsMaster clMasterObj = new CLDetailsMaster();
                    clMasterObj = StaffManagementService.GetCLDetailsMasterByPreRegNum(AttendanceMonth, AttendanceYear, PreRegNum);
                    if (StaffAttendanceClDetailsObj != null && clMasterObj != null)
                    {
                        StaffAttendanceClDetails.PreRegNum = PreRegNum;
                        StaffAttendanceClDetails.Month = AttendanceMonth;
                        StaffAttendanceClDetails.Year = AttendanceYear;
                        StaffAttendanceClDetails.OpeningCLBalance = OpeningBalance ?? 0;
                        StaffAttendanceClDetails.AllotedCL = AllotedCl;
                        StaffAttendanceClDetails.TotalAvailableBalane = TotalAvailableBalance;
                        StaffAttendanceClDetails.NoOfLeavesTaken = TotalNoOfLeavesTakenByLogs;
                        StaffAttendanceClDetails.LeaveToBeCalculated = LeaveToBeCalculated ?? 0;
                        StaffAttendanceClDetails.ClosingBalance = ClosingBalance ?? 0;
                        StaffAttendanceClDetails.IsActive = true;
                        StaffAttendanceClDetails.CreatedBy = userId;
                        StaffAttendanceClDetails.CreatedDate = DateTime.Now;
                        StaffAttendanceClDetails.Remarks = Remarks;
                        StaffManagementService.SaveOrUpdateStaff_AttendanceCLDetails(StaffAttendanceClDetails);
                        StaffAttendanceClDetailsObj.IsActive = false;
                        StaffManagementService.SaveOrUpdateStaff_AttendanceCLDetails(StaffAttendanceClDetailsObj);
                        clMasterObj.OpeningBalance = OpeningBalance ?? 0;
                        clMasterObj.PreRegNum = PreRegNum;
                        clMasterObj.Month = AttendanceMonth;
                        clMasterObj.Year = AttendanceYear;
                        clMasterObj.AllotedCL = Convert.ToInt64(AllotedCl);
                        clMasterObj.CLInHands = TotalAvailableBalance;
                        clMasterObj.ClosingBalance = ClosingBalance ?? 0;
                        clMasterObj.CreatedBy = userId;
                        clMasterObj.CreatedDate = DateTime.Now;
                        clMasterObj.ModifiedBy = userId;
                        clMasterObj.ModifiedDate = DateTime.Now;
                        StaffManagementService.SaveOrUpdateCLDetailsMaster(clMasterObj);
                        if (objStaffAttendanceChangeDetails == null)
                        {

                            Staff_AttendanceChangeDetails obj = new Staff_AttendanceChangeDetails();
                            StaffAttendanceChangeDetails.PreRegNum = PreRegNum;
                            StaffAttendanceChangeDetails.TotalNoOfDaysWorkedByLogs = TotalNoOfDaysWorkedByLogs;
                            StaffAttendanceChangeDetails.TotalNoOfLeavesTakenByLogs = TotalNoOfLeavesTakenByLogs;
                            StaffAttendanceChangeDetails.TotalNoOfDaysWorkedByChange = TotalNoOfDaysWorkedByChange;
                            StaffAttendanceChangeDetails.TotalNoOfLeavesTakenByChange = TotalNoOfLeavesTakenByChange;
                            StaffAttendanceChangeDetails.Month = AttendanceMonth;
                            StaffAttendanceChangeDetails.Year = AttendanceYear;
                            StaffAttendanceChangeDetails.CreatedBy = userId;
                            StaffAttendanceChangeDetails.CreatedDate = DateTime.Now;
                            StaffAttendanceChangeDetails.IsActive = true;
                            StaffAttendanceChangeDetails.OnDuty = OnDuty ?? 0;
                            StaffAttendanceChangeDetails.NoOfPermissionsTaken = NoOfPermissionsTaken ?? 0;
                            StaffAttendanceChangeDetails.NoOfLeavesCalculatedByPermissions = NoOfLeavesCalculatedByPermissions ?? 0;
                            StaffAttendanceChangeDetails.NoOfHolidays = NoOfHolidays;
                            StaffAttendanceChangeDetails.NoOfLeaveToBeCalculated = LeaveToBeCalculated ?? 0;
                            StaffManagementService.SaveOrUpdateStaff_AttendanceChangeDetails(StaffAttendanceChangeDetails);
                            return Json(StaffAttendanceChangeDetails.Staff_AttendanceChangeDetails_Id, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            StaffAttendanceChangeDetails.PreRegNum = PreRegNum;
                            StaffAttendanceChangeDetails.TotalNoOfDaysWorkedByLogs = TotalNoOfDaysWorkedByLogs;
                            StaffAttendanceChangeDetails.TotalNoOfLeavesTakenByLogs = objStaffAttendanceChangeDetails.TotalNoOfLeavesTakenByLogs;
                            StaffAttendanceChangeDetails.TotalNoOfDaysWorkedByChange = TotalNoOfDaysWorkedByChange;
                            StaffAttendanceChangeDetails.TotalNoOfLeavesTakenByChange = TotalNoOfLeavesTakenByChange;
                            //StaffAttendanceChangeDetails.AllowedCasualLeaves = AllowedCasualLeaves ?? 0;
                            StaffAttendanceChangeDetails.Month = AttendanceMonth;
                            StaffAttendanceChangeDetails.Year = AttendanceYear;
                            StaffAttendanceChangeDetails.CreatedBy = userId;
                            StaffAttendanceChangeDetails.CreatedDate = DateTime.Now;
                            StaffAttendanceChangeDetails.IsActive = true;
                            StaffAttendanceChangeDetails.OnDuty = OnDuty ?? 0;
                            StaffAttendanceChangeDetails.NoOfPermissionsTaken = NoOfPermissionsTaken ?? 0;
                            StaffAttendanceChangeDetails.NoOfLeavesCalculatedByPermissions = NoOfLeavesCalculatedByPermissions ?? 0;
                            StaffAttendanceChangeDetails.NoOfHolidays = NoOfHolidays;
                            StaffAttendanceChangeDetails.NoOfLeaveToBeCalculated = LeaveToBeCalculated ?? 0;
                            StaffManagementService.SaveOrUpdateStaff_AttendanceChangeDetails(StaffAttendanceChangeDetails);
                            objStaffAttendanceChangeDetails.ModifiedBy = userId;
                            objStaffAttendanceChangeDetails.ModifiedDate = DateTime.Now;
                            objStaffAttendanceChangeDetails.IsActive = false;
                            StaffManagementService.SaveOrUpdateStaff_AttendanceChangeDetails(objStaffAttendanceChangeDetails);
                            return Json(StaffAttendanceChangeDetails.Staff_AttendanceChangeDetails_Id, JsonRequestBehavior.AllowGet);
                        }

                    }
                    else
                    {
                        //Cl Balance Entry of Another table
                        StaffAttendanceClDetails.PreRegNum = PreRegNum;
                        StaffAttendanceClDetails.Month = AttendanceMonth;
                        StaffAttendanceClDetails.Year = AttendanceYear;
                        StaffAttendanceClDetails.OpeningCLBalance = OpeningBalance ?? 0;
                        StaffAttendanceClDetails.AllotedCL = AllotedCl;
                        StaffAttendanceClDetails.TotalAvailableBalane = TotalAvailableBalance;
                        StaffAttendanceClDetails.NoOfLeavesTaken = TotalNoOfLeavesTakenByLogs;
                        StaffAttendanceClDetails.LeaveToBeCalculated = LeaveToBeCalculated ?? 0;
                        StaffAttendanceClDetails.ClosingBalance = ClosingBalance ?? 0;
                        StaffAttendanceClDetails.IsActive = true;
                        StaffAttendanceClDetails.CreatedBy = userId;
                        StaffAttendanceClDetails.CreatedDate = DateTime.Now;
                        StaffAttendanceClDetails.Remarks = Remarks;
                        StaffManagementService.SaveOrUpdateStaff_AttendanceCLDetails(StaffAttendanceClDetails);
                        StaffAttendanceClDetailsObj.IsActive = false;
                        StaffAttendanceClDetailsObj.ClosingBalance = ClosingBalance ?? 0;
                        StaffManagementService.SaveOrUpdateStaff_AttendanceCLDetails(StaffAttendanceClDetailsObj);
                        clMaster.OpeningBalance = OpeningBalance ?? 0;
                        clMaster.PreRegNum = PreRegNum;
                        clMaster.Month = AttendanceMonth;
                        clMaster.Year = AttendanceYear;
                        clMaster.AllotedCL = Convert.ToInt64(AllotedCl);
                        clMaster.CLInHands = TotalAvailableBalance;
                        clMaster.ClosingBalance = ClosingBalance ?? 0;
                        clMaster.CreatedBy = userId;
                        clMaster.CreatedDate = DateTime.Now;
                        clMaster.ModifiedBy = userId;
                        clMaster.ModifiedDate = DateTime.Now;
                        StaffManagementService.SaveOrUpdateCLDetailsMaster(clMaster);

                    }

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
        #region Staff Holidays Master
        public ActionResult StaffHolidaysMaster()
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
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }

        }
        public ActionResult StaffHolidaysMasterGridListJqGrid(StaffHolidaysMaster ObjStaffHolidaysMaster, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (!string.IsNullOrEmpty(ObjStaffHolidaysMaster.Campus))
                    criteria.Add("Campus", ObjStaffHolidaysMaster.Campus);
                if (ObjStaffHolidaysMaster.Year > 0)
                    criteria.Add("Year", ObjStaffHolidaysMaster.Year);
                if (!string.IsNullOrEmpty(ObjStaffHolidaysMaster.Month))
                    criteria.Add("Month", ObjStaffHolidaysMaster.Month);
                Dictionary<long, IList<StaffHolidaysMaster>> StaffHolidaysMasterList = smsObj.GetStaffHolidaysMasterListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (StaffHolidaysMasterList != null && StaffHolidaysMasterList.FirstOrDefault().Key > 0)
                {
                    long totalrecords = StaffHolidaysMasterList.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,
                        rows = (from items in StaffHolidaysMasterList.First().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[]{
                                       items.StaffHolidaysMaster_Id.ToString(),  
                                       items.HolidayType,                                                                            
                                       items.Campus,         
                                       items.HolidayDate,
                                       items.Year.ToString(),
                                       items.Month,
                                       items.NoOfHolidays.ToString(),
                                       items.CreatedBy,
                                       items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy"):string.Empty,
                                       items.ModifiedBy,
                                       items.ModifiedDate!=null?items.ModifiedDate.Value.ToString("dd/MM/yyyy"):string.Empty,
                                       items.MonthNumber.ToString(),
                                       items.Descriptions,
                                      
                                       }
                                })

                    };
                    return Json(jsondat, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var Empty = new { rows = (new { cell = new string[] { } }) };
                    return Json(Empty, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult SaveOrUpdateStaffHolidaysMaster(StaffHolidaysMaster StaffHolidaysMaster, string HolidayDate)
        {
            try
            {
                var script = "";
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("logOut", "Account");
                else
                {
                    StaffHolidaysMaster ObjStaffHolidaysMaster = new StaffHolidaysMaster();
                    string[] HolidayDateSplit = HolidayDate.Split(',');
                    long HolidayDateCount = HolidayDateSplit.Length;
                    DateTime Date = DateTime.ParseExact(Convert.ToString(HolidayDateSplit[0]), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    long MonthNumber = Date.Month;
                    long Year = Date.Year;
                    DateTime dtDate = new DateTime(Convert.ToInt32(Year), Convert.ToInt32(MonthNumber), 1);
                    string MonthName = dtDate.ToString("MMMM");
                    //string monthName = StaffHolidaysMaster.Month;
                    //long MonthNumber = DateTime.ParseExact(monthName, "MMMM", CultureInfo.CurrentCulture).Month;
                    if (StaffHolidaysMaster.StaffHolidaysMaster_Id == 0)
                    {
                        ObjStaffHolidaysMaster = smsObj.GetStaffHolidaysMasterByAcademicYearAndMonth(Year, MonthNumber, StaffHolidaysMaster.HolidayType, StaffHolidaysMaster.Campus);
                        if (ObjStaffHolidaysMaster == null)
                        {
                            StaffHolidaysMaster.CreatedBy = userId;
                            StaffHolidaysMaster.CreatedDate = DateTime.Now;
                            StaffHolidaysMaster.Month = MonthName;
                            StaffHolidaysMaster.Year = Year;
                            StaffHolidaysMaster.MonthNumber = MonthNumber;
                            StaffHolidaysMaster.HolidayDate = HolidayDate;
                            smsObj.SaveOrUpdateStaffHolidaysMaster(StaffHolidaysMaster);
                            script = @"SucessMsg(""Added Sucessfully"");";
                        }
                        else
                            script = @"ErrMsg(""The configuration is already exist!"");";
                    }
                    else
                    {
                        ObjStaffHolidaysMaster = smsObj.GetStaffHolidaysMasterByAcademicYearAndMonth(Year, MonthNumber, StaffHolidaysMaster.HolidayType, StaffHolidaysMaster.Campus);
                        if (ObjStaffHolidaysMaster == null)
                        {
                            StaffHolidaysMaster.ModifiedBy = userId;
                            StaffHolidaysMaster.ModifiedDate = DateTime.Now;
                            StaffHolidaysMaster.Month = MonthName;
                            StaffHolidaysMaster.Year = Year;
                            StaffHolidaysMaster.MonthNumber = MonthNumber;
                            StaffHolidaysMaster.HolidayDate = HolidayDate;
                            smsObj.SaveOrUpdateStaffHolidaysMaster(StaffHolidaysMaster);
                            script = @"SucessMsg(""Updated Sucessfully"");";
                        }
                        else
                            script = @"ErrMsg(""The configuration is already exist!"");";
                    }
                }
                return JavaScript(script);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult DeleteStaffHolidaysMaster(string[] Id)
        {
            try
            {

                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("Logout", "Account");
                else
                {
                    if (Id == null)
                        return null;
                    string[] CityIdArray = Id[0].Split(',');
                    long[] longCityIdArray = new long[CityIdArray.Length];
                    for (int j = 0; j < CityIdArray.Length; j++)
                        longCityIdArray[j] = Convert.ToInt64(CityIdArray[j]);
                    smsObj.DeleteStaffHolidaysMaster(longCityIdArray);
                    var script = @"SucessMsg(""Deleted Successfully"");";
                    return JavaScript(script);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region Staff Attendance New Status
        public ActionResult StaffAttendanceNewStatusGridListJqGrid(long PreRegNum, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (PreRegNum > 0)
                    criteria.Add("PreRegNum", PreRegNum);
                //if (!string.IsNullOrEmpty(IdNumber))
                //    criteria.Add("IdNumber", IdNumber);
                Dictionary<long, IList<StaffAttendanceNewStatus>> StaffAttendanceNewStatusList = smsObj.GetStaffAttendanceNewStatusListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (StaffAttendanceNewStatusList != null && StaffAttendanceNewStatusList.FirstOrDefault().Key > 0)
                {
                    long totalrecords = StaffAttendanceNewStatusList.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,
                        rows = (from items in StaffAttendanceNewStatusList.First().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[]{
                                       items.StaffStatus_Id.ToString(),                                                                                                                     
                                       items.StaffName,                                       
                                       items.IdNumber,
                                       items.PreRegNum.ToString(),
                                       items.StaffStatus,
                                       items.DateOfLongLeaveAndResigned!=null? items.DateOfLongLeaveAndResigned.Value.ToString("dd/MM/yyyy"):string.Empty,
                                       items.ToDateOfLongLeaveAndResigned,
                                       items.Remarks,
                                       items.CreatedBy,
                                       items.DateOfCreated!=null?items.DateOfCreated.Value.ToString("dd/MM/yyyy"):string.Empty,
                                       items.ModifiedBy,
                                       items.ModifiedDate!=null?items.ModifiedDate.Value.ToString("dd/MM/yyyy"):string.Empty,
                                       }
                                })

                    };
                    return Json(jsondat, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var Empty = new { rows = (new { cell = new string[] { } }) };
                    return Json(Empty, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        #endregion
        #region Staff_AttendanceConfigurationsReport
        public ActionResult AttendanceConfigurationsReport()
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
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "HRManagementPolicy");
                throw;
            }
        }
        public ActionResult GetAttendanceConfigurationsReportListJqGrid(Staff_AttendanceConfigurationsReport_Vw ObjAttendanceConfigurationRpt, string ExportType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                Dictionary<string, object> ExactCriteria = new Dictionary<string, object>();
                Dictionary<string, object> LikeCriteria = new Dictionary<string, object>();
                UserService us = new UserService();

                sord = sord == "desc" ? "Desc" : "Asc";
                if (ObjAttendanceConfigurationRpt != null && !string.IsNullOrEmpty(ObjAttendanceConfigurationRpt.Name))
                    LikeCriteria.Add("Name", ObjAttendanceConfigurationRpt.Name);
                if (ObjAttendanceConfigurationRpt != null && !string.IsNullOrEmpty(ObjAttendanceConfigurationRpt.IdNumber))
                    LikeCriteria.Add("IdNumber", ObjAttendanceConfigurationRpt.IdNumber);
                //if (ObjAttendanceConfigurationRpt != null && !string.IsNullOrEmpty(ObjAttendanceConfigurationRpt.Campus))
                //    ExactCriteria.Add("Campus", ObjAttendanceConfigurationRpt.Campus);
                if (!string.IsNullOrEmpty(ObjAttendanceConfigurationRpt.Campus))
                {
                    ExactCriteria.Add("Campus", ObjAttendanceConfigurationRpt.Campus);
                }
                else
                {
                    string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                    ExactCriteria.Add("UserId", userid);
                    ExactCriteria.Add("AppCode", "ATTCNFRPT");
                    Dictionary<long, IList<UserAppRole>> listObj = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, ExactCriteria);
                    if (listObj != null && listObj.FirstOrDefault().Key > 0)
                    {
                        var BranchList = (from item in listObj.FirstOrDefault().Value
                                          where item.DeptCode == "FEES / FINANCE" || item.DeptCode == "HR"
                                          select item.BranchCode).ToArray();
                        var Rolearr = (from item in listObj.FirstOrDefault().Value                                          
                                          select item.RoleCode).ToArray();
                        if(Rolearr!=null && Rolearr.Contains("Bio-All"))
                        {

                        }
                        else if (BranchList != null && BranchList.Count() > 0)
                        {
                            ExactCriteria.Add("Campus", BranchList);
                        }
                        //ExactCriteria.Clear();                        
                    }
                    ExactCriteria.Remove("UserId");
                    ExactCriteria.Remove("AppCode");
                }
                if (ObjAttendanceConfigurationRpt != null && !string.IsNullOrEmpty(ObjAttendanceConfigurationRpt.Designation))
                    ExactCriteria.Add("Designation", ObjAttendanceConfigurationRpt.Designation);
                if (ObjAttendanceConfigurationRpt != null && !string.IsNullOrEmpty(ObjAttendanceConfigurationRpt.Department))
                    ExactCriteria.Add("Department", ObjAttendanceConfigurationRpt.Department);
                if (ObjAttendanceConfigurationRpt != null && !string.IsNullOrEmpty(ObjAttendanceConfigurationRpt.Status))
                    ExactCriteria.Add("Status", ObjAttendanceConfigurationRpt.Status);
                if (ObjAttendanceConfigurationRpt != null && !string.IsNullOrEmpty(ObjAttendanceConfigurationRpt.StaffCategoryForAttendane))
                    ExactCriteria.Add("StaffCategoryForAttendane", ObjAttendanceConfigurationRpt.StaffCategoryForAttendane);
                if (ObjAttendanceConfigurationRpt != null && !string.IsNullOrEmpty(ObjAttendanceConfigurationRpt.IsHavingICMSAccount))
                    ExactCriteria.Add("IsHavingICMSAccount", ObjAttendanceConfigurationRpt.IsHavingICMSAccount);
                if (ObjAttendanceConfigurationRpt != null && !string.IsNullOrEmpty(ObjAttendanceConfigurationRpt.IsAttendanceConfigured))
                    ExactCriteria.Add("IsAttendanceConfigured", ObjAttendanceConfigurationRpt.IsAttendanceConfigured);
                if (ObjAttendanceConfigurationRpt != null && !string.IsNullOrEmpty(ObjAttendanceConfigurationRpt.IsAttendanceMappedInICMS))
                    ExactCriteria.Add("IsAttendanceMappedInICMS", ObjAttendanceConfigurationRpt.IsAttendanceMappedInICMS);
                Dictionary<long, IList<Staff_AttendanceConfigurationsReport_Vw>> AttendanceConfigurationsReportDetails = smsObj.GetStaff_AttendanceConfigurationsReport_VwListWithPagingAndCriteria(page - 1, rows, sidx, sord, ExactCriteria, LikeCriteria);
                if (AttendanceConfigurationsReportDetails != null && AttendanceConfigurationsReportDetails.FirstOrDefault().Key > 0)
                {
                    if (ExportType == "Excel")
                    {
                        string ExcelFileName = "Report" + DateTime.Now.ToString("dd/MM/yyyy");
                        var List = AttendanceConfigurationsReportDetails.First().Value.ToList();
                        ExptToXL(List, ExcelFileName, (items => new
                        {
                            items.Campus,
                            items.IdNumber,
                            items.Name,
                            items.Department,
                            items.Designation,
                            items.Status,
                            StaffCategory = items.StaffCategoryForAttendane,
                            EmployeeCode = items.IdNumberToEmployeeCode,
                            StaffBioMetricId = items.StaffBioMetricId != null ? items.StaffBioMetricId.ToString() : string.Empty,
                            items.IsAttendanceConfigured,
                            items.IsAttendanceMappedInICMS
                        }));
                        return new EmptyResult();
                    }
                    else
                    {
                        long totalrecords = AttendanceConfigurationsReportDetails.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var ReturnJsonData = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in AttendanceConfigurationsReportDetails.First().Value
                                    select new
                                    {
                                        i = items.Id,
                                        cell = new string[]{
                                       items.Id.ToString(),                                                                                                                     
                                       items.PreRegNum.ToString(),                                       
                                       items.Campus,
                                       items.IdNumber,
                                       items.Name,
                                       items.Department,
                                       items.Designation,
                                       items.Status,
                                       items.StaffType,
                                       items.StaffCategoryForAttendane,
                                       items.StaffUserName,
                                       items.IdNumberToEmployeeCode,
                                       items.StaffBioMetricId.ToString(),                                       
                                       items.IsHavingICMSAccount,
                                       items.IsAttendanceConfigured,
                                       items.IsAttendanceMappedInICMS
                                       }
                                    })

                        };
                        return Json(ReturnJsonData, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var Empty = new { rows = (new { cell = new string[] { } }) };
                    return Json(Empty, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "HRManagementPolicy");
                throw ex;
            }
        }
        public JsonResult UpdateStaffBiometricIdByIdNumberToEmployeeCode(string IdNumberToEmployeeCodeArray)
        {
            try
            {
                bool ReturnValue = false;
                string[] EmployeeCodeArray = IdNumberToEmployeeCodeArray.Split(',').Distinct().ToArray();
                if (EmployeeCodeArray != null && EmployeeCodeArray.Length > 0)
                {
                    smsObj.UpdateStaffBiometricIdByIdNumberToEmployeeCode(EmployeeCodeArray);
                    ReturnValue = true;
                }
                return Json(ReturnValue, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "HRManagementPolicy");
                throw ex;
            }
        }
        #endregion

        #region CandidateDetails
        public ActionResult CandidateStatusChange()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MastersService ms = new MastersService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<long, IList<CampusMaster>> CampusMaster = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                    ViewBag.Campus = CampusMaster.First().Value;
                    #region BreadCrumb
                    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                    #endregion
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult CandidateStatusJqGrid(string InterviewCampus, string InterviewDate, string Status, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                sord = sord == "desc" ? "Desc" : "Asc";

                if (!string.IsNullOrEmpty(InterviewCampus))
                    criteria.Add("Campus", InterviewCampus);

                if (!string.IsNullOrEmpty(InterviewDate))
                {
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    var Datevalue = DateTime.Parse(InterviewDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    criteria.Add("CreatedDate", Datevalue);
                }
                if (!string.IsNullOrEmpty(Status))
                    criteria.Add("Status", Status);

                Dictionary<long, IList<CandidateDtls>> CandidateList_vwList = smsObj.GetCandidateList_vwStatusListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (CandidateList_vwList != null && CandidateList_vwList.FirstOrDefault().Key > 0)
                {
                    long totalrecords = CandidateList_vwList.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,
                        rows = (from items in CandidateList_vwList.First().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[]{
                                       items.Id.ToString(),                                                                                                                     
                                       items.PreRegNum.ToString(),         
                                       items.Salutation + items.Name,
                                       items.Gender,
                                       items.Age.ToString(),
                                       //items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy"):"",
                                       items.Campus,
                                       items.AppliedFor,
                                       items.Qualification,
                                       items.TotalYearsOfExp,
                                       items.TotalYearsOfTeachingExp,
                                       items.SubjectsTaught,
                                       items.GradesTaught,
                                       items.LastDrawnGrossSalary,
                                       items.ExpectedSalary,
                                       items.Status
                                       }
                                })
                    };
                    return Json(jsondat, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var Empty = new { rows = (new { cell = new string[] { } }) };
                    return Json(Empty, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult CandidateDetailsProfileView(string Id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    StaffManagementService smsObj = new StaffManagementService();
                    CandidateDtls CandidateDtls = smsObj.GetCandidateDtlsId(Convert.ToInt64(Id));
                    #region BreadCrumb
                    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                    #endregion
                    return View(CandidateDtls);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult CandidateRegistration(string Id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MastersService ms = new MastersService();
                    StaffManagementService smsObj = new StaffManagementService();

                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<long, IList<CampusMaster>> CampusMaster = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                    ViewBag.Campus = CampusMaster.First().Value;
                    CandidateDtls CandidateDtls = new CandidateDtls();
                    if (!string.IsNullOrEmpty(Id))
                    {
                        CandidateDtls = smsObj.GetCandidateDtlsId(Convert.ToInt64(Id));
                    }
                    #region BreadCrumb
                    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                    #endregion
                    return View(CandidateDtls);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        #endregion
    }
}

