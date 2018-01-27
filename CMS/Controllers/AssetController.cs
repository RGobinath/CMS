using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TIPS.Service;
using TIPS.ServiceContract;
using TIPS.Entities;
using TIPS.Entities.AssetEntities;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Globalization;
using TIPS.Component;
using TIPS.Entities.StudentsReportEntities;
using CMS.Helpers;
using System.Collections;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using System.Web;
using TIPS.Entities.AdmissionEntities;
using System.Text;
using System.Configuration;
using Microsoft.Win32.SafeHandles;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using BarCode.Models;
using System.Web.UI;
using OfficeOpenXml;
using System.Data.OleDb;
using System.Data;

//using OnBarcode.Barcode;
//using CMS.CountryService;

namespace CMS.Controllers
{
    public class AssetController : BaseController
    {
        MastersService ms = new MastersService();
        AssetService ass = new AssetService();
        EmailHelper em = new EmailHelper();
        UserService us = new UserService();
        StudentsReportBC srbc = new StudentsReportBC();
        IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
        #region Asset
        public ActionResult AssetOrganizer()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
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
                    ViewBag.campusddl = CampusMaster.First().Value;
                    ViewBag.loggedInUserId = userId;
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
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult GetAssetByCampus(string Campus)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Campus", Campus);
                Dictionary<long, IList<AssetMaster>> AssetList = ass.GetAssetMasterListWithPagingAndCriteria(0, 9999, "", "", criteria);
                var AssetDetails =
                                       (from items in AssetList.FirstOrDefault().Value
                                        where items.Asset != null && items.Asset != ""
                                        select new
                                        {
                                            Text = items.Asset,
                                            Value = items.Asset
                                        }).Distinct().ToList();
                return Json(AssetDetails, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult GetEventList(string Campus, string Hall)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrWhiteSpace(Campus)) { criteria.Add("Campus", Campus); }
                if (!string.IsNullOrWhiteSpace(Hall)) { criteria.Add("AssetName", Hall); }
                criteria.Add("Status", "Approved");
                Dictionary<long, IList<AssetOrganizer_vw>> AssetviewList = ass.GetAssetEventviewListWithPagingAndCriteria(0, 9999, "", "", criteria);
                var EventList = from e in AssetviewList.FirstOrDefault().Value
                                where e.Date != null && !string.IsNullOrEmpty(e.StartTimeString) && !string.IsNullOrEmpty(e.EndTimeString)
                                select new
                                {
                                    id = e.Id,
                                    title = e.AssetName + " for " + e.ReasonForBooking,
                                    textColor = "#ffffff",
                                    Room = e.AssetName,
                                    backgroundColor = e.AssetColor,
                                    borderColor = "#ffffff",
                                    //start = e.StartTime.ToString("s"),
                                    //end = e.EndTime.ToString("s"),
                                    start = e.Date.ToString("yyyy-MM-dd") + "T" + e.StartTimeString,
                                    end = e.Date.ToString("yyyy-MM-dd") + "T" + e.EndTimeString,
                                    allDay = false
                                };
                //AssetOrganizer tstObj = new AssetOrganizer();
                //IList<AssetOrganizer> TestList = new List<AssetOrganizer>();
                //tstObj.ReasonForBooking = "test1";
                //tstObj.StartTime = DateTime.Now;
                //tstObj.EndTime = DateTime.Now;
                //tstObj.StaffIncharge = "Gobi";
                //TestList.Add(tstObj);
                //var eventList = from e in TestList
                //                select new
                //                {
                //                    // id = e.timetable_id,
                //                    id = 1,
                //                    //id1 = e.allotment_id,
                //                    //id2 = e.teacher_id,
                //                    title = e.ReasonForBooking,
                //                    start = e.StartTime.ToString("s"),
                //                    end = e.EndTime.ToString("s"),                            //color = e.StatusColor,
                //                    textColor = "#ffffff",
                //                    //someKey = e.SomeImportantKeyID,
                //                    backgroundColor = "#ffffff",
                //                    //borderColor="#ffffff",
                //                    //color=e.SubjectColor,
                //                    allDay = false
                //                };
                //var rows = eventList.ToArray();
                var rows = EventList.ToArray();
                return Json(rows, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }

        }
        public ActionResult DeleteEvent(long EvntId)
        {
            try
            {
                if (EvntId > 0)
                {
                    ass.DeleteAssetEvent(EvntId);
                    return Json("Request deleted sucessfully.");
                }
                else
                { return Json("Please select Request."); }
            }
            catch (Exception ex)
            { return ThrowJSONError(ex); }
        }
        public ActionResult EventListDetailsListJqGrid(string Campus, string Status, string AssetName, string Date, string PageName, int? Id, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MastersService ts = new MastersService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    var rle = Session["userrolesarray"] as IEnumerable<string>;
                    if (rle.Contains("AST-GRL"))
                    {
                        criteria.Add("CreatedBy", userId);
                    }
                    if (!string.IsNullOrEmpty(Status)) { criteria.Add("Status", Status); }
                    else { criteria.Add("Status", "Initiated"); }
                    if (!string.IsNullOrEmpty(AssetName)) { criteria.Add("AssetName", AssetName); }
                    if (!string.IsNullOrEmpty(Date)) { criteria.Add("Date", DateTime.Parse(Date, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault)); }
                    sord = sord == "desc" ? "Desc" : "Asc";
                    Dictionary<long, IList<AssetOrganizer>> EventDetails = ass.GetEventListWithPagingAndCriteria(page - 1, rows, sord, sidx, criteria);
                    if (EventDetails != null && EventDetails.Count > 0 && EventDetails.First().Key > 0 && PageName == "Approve")
                    {
                        UserService us = new UserService();
                        long totalrecords = EventDetails.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var EventList = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in EventDetails.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                                        items.Id.ToString(),
                                        items.Status != "Approved"?String.Format(@"<a style='color:#034af3;text-decoration:underline' href= '#' onclick = 'getAssetdata("+"\"" + items.Id + "\"" + ")' >{0}</a>",items.RequestNo):items.RequestNo,
                                        items.Campus,
                                        items.AssetName,
                                        items.Date!=null? items.Date.ToString("dd'/'MM'/'yyyy"):"",
                                        items.StartTimeString,
                                        items.EndTimeString,
                                        items.ReasonForBooking,
                                        items.Status,
                                        items.CreatedDate!=null? items.CreatedDate.ToString("dd'/'MM'/'yyyy"):"",
                                        items.CreatedBy!=null?us.GetUserNameByUserId(items.CreatedBy):"",
                                        items.CreatedBy!=null?items.CreatedBy:""
                                    }
                                    })
                        };
                        return Json(EventList, JsonRequestBehavior.AllowGet);
                    }
                    else if (EventDetails != null && EventDetails.Count > 0 && EventDetails.First().Key > 0 && PageName == "Request")
                    {
                        UserService us = new UserService();
                        long totalrecords = EventDetails.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var EventList = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in EventDetails.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                                        items.Id.ToString(),
                                        items.RequestNo,
                                        items.Campus,
                                        items.AssetName,
                                        items.Date!=null? items.Date.ToString("dd'/'MM'/'yyyy"):"",
                                        items.StartTimeString,
                                        items.EndTimeString,
                                        items.ReasonForBooking,
                                        items.Status,
                                        items.CreatedDate!=null? items.CreatedDate.ToString("dd'/'MM'/'yyyy"):"",
                                        items.CreatedBy!=null?us.GetUserNameByUserId(items.CreatedBy):"",
                                        items.CreatedBy!=null?items.CreatedBy:""
                                        }
                                    })
                        };
                        return Json(EventList, JsonRequestBehavior.AllowGet);

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
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult AddEventList(long? Id, string Campus, string AssetName, string Date, string FromTime, string ToTime, string ReasonForBooking)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    bool ischeck = CheckBookedAvailability(Campus, AssetName, Date, FromTime, ToTime);
                    if ((Id == null || Id == 0) && ischeck == false)
                    {
                        AssetOrganizer Ao = new AssetOrganizer();
                        Ao.Campus = Campus;
                        Ao.AssetName = AssetName;
                        Ao.Date = DateTime.Parse(Date, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        Ao.StartTimeString = ChangeTimeto24hours(FromTime);
                        Ao.EndTimeString = ChangeTimeto24hours(ToTime);
                        Ao.ReasonForBooking = ReasonForBooking;
                        Ao.CreatedDate = DateTime.Now;
                        Ao.CreatedBy = userId;
                        Ao.ModifiedBy = userId;
                        Ao.ModifiedDate = DateTime.Now;
                        Ao.Status = "Initiated";
                        Ao.IsActive = true;
                        ass.CreateOrUpdateEvent(Ao);
                        Ao.RequestNo = "Ast-" + Ao.Id;
                        ass.CreateOrUpdateEvent(Ao);
                        Dictionary<string, object> criteria = new Dictionary<string, object>();
                        criteria.Add("RoleCode", "AST-APP");
                        criteria.Add("BranchCode", Campus);
                        Dictionary<long, IList<UserAppRole_Vw>> UserList = us.GetAppRoleOnlyActiveUsersPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                        string[] UserDetails = (from u in UserList.First().Value
                                                select u.Email).ToArray();
                        bool retvalue = sendMailtoAssetOrganizer("Asset-Approver", Ao, UserDetails);
                        return Json("Success", JsonRequestBehavior.AllowGet);
                    }
                    else if (Id > 0 && ischeck == false)
                    {
                        AssetOrganizer Ast = new AssetOrganizer();
                        Ast = ass.GetAssetEventById(Id ?? 0);
                        if (Ast != null)
                        {
                            Ast.Campus = Campus;
                            Ast.AssetName = AssetName;
                            Ast.Date = DateTime.Parse(Date, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                            Ast.StartTimeString = ChangeTimeto24hours(FromTime);
                            Ast.EndTimeString = ChangeTimeto24hours(ToTime);
                            Ast.ReasonForBooking = ReasonForBooking;
                            Ast.ModifiedBy = userId;
                            Ast.ModifiedDate = DateTime.Now;
                            ass.CreateOrUpdateEvent(Ast);
                            return Json("Success", JsonRequestBehavior.AllowGet);
                        }
                        else
                            return Json("Failed", JsonRequestBehavior.AllowGet);
                    }
                    else { return Json("Booked", JsonRequestBehavior.AllowGet); }

                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public bool CheckBookedAvailability(string cam, string ast, string date, string frmtime, string endtime)
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("Campus", cam);
            criteria.Add("AssetName", ast);
            criteria.Add("Date", DateTime.Parse(date, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault));
            criteria.Add("Status", "Approved");
            Dictionary<long, IList<AssetOrganizer>> AssetApprovedList = ass.GetEventListWithPagingAndCriteria(0, 9999, "", "", criteria);
            if (AssetApprovedList != null && AssetApprovedList.Count > 0 && AssetApprovedList.FirstOrDefault().Key > 0)
            {

                double strtme = TimeSpan.Parse(frmtime).TotalSeconds;
                double endtme = TimeSpan.Parse(endtime).TotalSeconds;
                int count = 0;
                foreach (AssetOrganizer astapp in AssetApprovedList.FirstOrDefault().Value)
                {
                    double srttemp = TimeSpan.Parse(astapp.StartTimeString).TotalSeconds;
                    double endtemp = TimeSpan.Parse(astapp.EndTimeString).TotalSeconds;
                    //string[] srttemp = astapp.StartTimeString.Split(':');
                    //string[] endtemp = astapp.EndTimeString.Split(':');
                    if ((srttemp >= endtme && srttemp >= strtme) || (endtemp <= strtme && endtemp <= endtme))
                    { count = count + 1; }
                    else return true;
                }
                if (count > 0)
                    return false;
                else
                    return true;
            }
            else return false;
        }

        public string ChangeTimeto24hours(string time)
        {
            string retvalue = "";
            string[] temp = time.Split(' ');
            if (temp.Length > 1 && temp[1] == "PM")
            {
                string[] temp1 = temp[0].Split(':');
                int temphour = Convert.ToInt32(temp1[0]) + 12;
                retvalue = temphour.ToString() + ":" + temp1[1].ToString() + ":00";
            }
            else if (temp.Length > 1 && temp[1] == "AM")
                retvalue = temp[0] + ":00";
            else
            {
                string[] temp2 = time.Split(':');
                if (temp2.Length < 3)
                    retvalue = time + ":00";
                else
                    retvalue = time;
            }
            return retvalue;
        }

        public ActionResult ApproveAssetEvent(long? Id)
        {
            try
            {
                AssetOrganizer ast = new AssetOrganizer();
                ast = ass.GetAssetEventById(Id ?? 0);
                return View(ast);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }


        public ActionResult UpdateAssetEvent(long? Id, string Status)
        {
            try
            {
                string userId = base.ValidateUser();
                AssetOrganizer ast = new AssetOrganizer();
                ast = ass.GetAssetEventById(Id ?? 0);
                if (ast != null)
                {
                    bool ischeck = CheckBookedAvailability(ast.Campus, ast.AssetName, ast.Date.ToString("dd'/'MM'/'yyyy"), ast.StartTimeString, ast.EndTimeString);
                    if (ischeck == false)
                    {
                        ast.Status = Status;
                        ast.ModifiedBy = userId;
                        ast.ModifiedDate = DateTime.Now;
                        ass.CreateOrUpdateEvent(ast);
                        Dictionary<string, object> criteria = new Dictionary<string, object>();
                        criteria.Add("RoleCode", "AST-GRL");
                        criteria.Add("BranchCode", ast.Campus);
                        Dictionary<long, IList<UserAppRole_Vw>> UserList = us.GetAppRoleOnlyActiveUsersPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                        string[] UserDetails = (from u in UserList.First().Value
                                                select u.Email).ToArray();
                        bool retval = sendMailtoAssetOrganizer("Asset-Requester", ast, UserDetails);
                        return Json("Success", JsonRequestBehavior.AllowGet);
                    }
                    else
                        return Json("Failed", JsonRequestBehavior.AllowGet);
                }
                else { return Json("Failed", JsonRequestBehavior.AllowGet); }

            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public bool sendMailtoAssetOrganizer(string rType, AssetOrganizer objAo, string[] receipient)
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            string RecipientInfo = string.Empty;
            string MailBody = GetBodyofMail();
            bool retValue = false;
            System.Net.Mail.MailMessage mailmsg = new System.Net.Mail.MailMessage();
            for (int i = 0; i < receipient.Length; i++)
            {
                mailmsg.To.Add(receipient[i]);
            }
            RecipientInfo = "Dear Sir/Madam,";
            if (rType == "Asset-Approver")
            {
                mailmsg.Subject = "A new Asset request #" + objAo.RequestNo + " is available for Approval"; // st.Subject;
                mailmsg.Body = "A new Asset request is available in Initiated state and you are requested to process the same.<br><br>Campus: " + objAo.Campus + "<br> Asset Name: " + objAo.AssetName + "<br>Date: " + objAo.Date.ToString("dd'/'MM'/'yyyy") + " on " + objAo.StartTimeString + " - " + objAo.EndTimeString + "<br>";
            }
            else
            {
                if (objAo.Status == "Rejected")
                {
                    mailmsg.Subject = "Your Request #" + objAo.RequestNo + " is Rejected and Reverted to you";
                    mailmsg.Body = "Your Asset request is rejected and reverted to you the same.<br><br>";
                }
                else
                {
                    mailmsg.Subject = "Your Asset Request #" + objAo.RequestNo + " is Approved by Approver";
                    mailmsg.Body = "Your Asset request has been approved by approver.<br><br> Campus: " + objAo.Campus + "<br> Asset Name: " + objAo.AssetName + "<br>Date: " + objAo.Date.ToString("dd'/'MM'/'yyyy") + " on " + objAo.StartTimeString + " - " + objAo.EndTimeString + "<br> Requested By:" + objAo.CreatedBy != null ? us.GetUserNameByUserId(objAo.CreatedBy) : "";
                }
            }
            criteria.Clear();
            criteria.Add("EmailType", "Asset");
            criteria.Add("IsMailNeeded", true);
            Dictionary<long, IList<MISMailMaster>> MISMailMasterDetails = srbc.GetMISMailMasterListWithPaging(null, 9999, string.Empty, string.Empty, criteria);
            if (MISMailMasterDetails != null && MISMailMasterDetails.Count > 0 && MISMailMasterDetails.FirstOrDefault().Value.Count > 0)
            {
                foreach (MISMailMaster item in MISMailMasterDetails.First().Value)
                {
                    mailmsg.CC.Add(item.EmailId);
                }
            }
            retValue = em.SendMailToRecipient(mailmsg, objAo.Campus, MailBody, RecipientInfo, null);
            return retValue;
        }

        public string GetBodyofMail()
        {
            string MessageBody = System.IO.File.ReadAllText(HttpContext.Server.MapPath("~/Views/Shared/AdmissionEmailBody.html"));
            return MessageBody;
        }

        public ActionResult AssetReport()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
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
                    ViewBag.campusddl = CampusMaster.First().Value;
                    ViewBag.loggedInUserId = userId;
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
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult AssetReportListJqGrid(string ExportType, string Campus, string AssetName, string frmdate, string todate, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MastersService ts = new MastersService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    if (!string.IsNullOrEmpty(Campus)) { criteria.Add("Campus", Campus); }
                    if (!string.IsNullOrEmpty(AssetName)) { criteria.Add("AssetName", AssetName); }
                    if (!string.IsNullOrEmpty(frmdate) && !string.IsNullOrEmpty(todate))
                    {
                        frmdate = frmdate.Trim();
                        todate = todate.Trim();
                        DateTime[] fromto = new DateTime[2];
                        fromto[0] = DateTime.Parse(frmdate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        fromto[1] = DateTime.Parse(todate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);// Convert.ToDateTime(To + " " + "23:59:59");
                        criteria.Add("Date", fromto);
                    }
                    criteria.Add("Status", "Approved");
                    Dictionary<long, IList<AssetOrganizer>> AssetReportList = ass.GetEventListWithPagingAndCriteria(page - 1, rows, sord, sidx, criteria);
                    if (AssetReportList != null && AssetReportList.Count > 0)
                    {
                        var AssetRptList = AssetReportList.First().Value.ToList();
                        if (ExportType == "Excel")
                        {
                            ExptToXL(AssetRptList, "AssetReportList", (items => new
                            {
                                items.Campus,
                                items.AssetName,
                                BookedOn = items.Date != null ? items.Date.ToString("dd'/'MM'/'yyyy") : "",
                                items.StartTimeString,
                                items.EndTimeString,
                                items.ReasonForBooking,
                            }));
                            return new EmptyResult();
                        }
                        else
                        {
                            UserService us = new UserService();
                            long totalrecords = AssetReportList.First().Key;
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var EventList = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalrecords,
                                rows = (from items in AssetReportList.First().Value
                                        select new
                                        {
                                            i = 2,
                                            cell = new string[] {
                                        items.Id.ToString(),
                                        items.Campus,
                                        items.AssetName,
                                        items.Date!=null? items.Date.ToString("dd'/'MM'/'yyyy"):"",
                                        items.StartTimeString,
                                        items.EndTimeString,
                                        items.ReasonForBooking,
                                    }
                                        })
                            };
                            return Json(EventList, JsonRequestBehavior.AllowGet);
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
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        #endregion
        #region IT Asset management by Thamizhmani
        public JsonResult FillITAssetName(long? FormId)
        {
            string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Clear();
            criteria.Add("IsSubAsset", false);
            //if (FormId > 0)
            //    criteria.Add("CampusMaster.FormId", FormId);
            Dictionary<long, IList<AssetDetailsTemplate>> AssetMaster = ass.GetAssetTemplateListWithPagingAndCriteria(0, 9999, "AssetType", "Asc", criteria);
            if (AssetMaster != null && AssetMaster.First().Value != null && AssetMaster.First().Value.Count > 0)
            {
                var assetList = (
                         from items in AssetMaster.First().Value
                         select new
                         {
                             Text = items.AssetType,
                             Value = items.Asset_Id
                         }).Distinct().ToList();
                return Json(assetList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult FillITAssetSpecification()
        {
            string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Clear();
            Dictionary<long, IList<ITAssetSpecification>> AssetMaster = ass.GetAssetSpecificationListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            if (AssetMaster != null && AssetMaster.First().Value != null && AssetMaster.First().Value.Count > 0)
            {
                var assetList = (
                         from items in AssetMaster.First().Value
                         select new
                         {
                             Text = items.Description,
                             Value = items.Specification
                         }).Distinct().ToList();
                return Json(assetList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ITAssetTypeMaster()
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
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult ITAssetTypeMasterjqGrid(AssetDetailsTemplate assetMaster, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    sord = sord == "desc" ? "Desc" : "Asc";
                    //if (sidx == "FormId") sidx = "CampusMaster.FormId";
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Clear();
                    Dictionary<string, object> likecriteria = new Dictionary<string, object>();
                    likecriteria.Clear();
                    if (assetMaster != null)
                    {
                        //if (assetMaster.CampusMaster != null && assetMaster.CampusMaster.FormId > 0)
                        //    criteria.Add("CampusMaster.FormId", assetMaster.CampusMaster.FormId);
                        if (assetMaster.Asset_Id > 0)
                            criteria.Add("Asset_Id", assetMaster.Asset_Id);
                        if (!string.IsNullOrEmpty(assetMaster.AssetCode))
                            likecriteria.Add("AssetCode", assetMaster.AssetCode);
                    }
                    //Dictionary<long, IList<AssetDetailsTemplate>> AssetReportList = ass.GetAssetDetailsTemplateWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    Dictionary<long, IList<AssetDetailsTemplate>> AssetReportList = ass.GetITAssetDetailsTemplateWithPagingAndExactAndLikeCriteria(page - 1, rows, sidx, sord, criteria, likecriteria);
                    if (AssetReportList != null && AssetReportList.Count > 0)
                    {
                        //criteria.Clear();
                        //Dictionary<long, IList<ITAssetSpecification>> specification = ass.GetITAssetSpecificationWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                        ////Hashtable hashvalues=new Hashtable();
                        //Dictionary<string, string> values = new Dictionary<string, string>();
                        //foreach (var items in specification.FirstOrDefault().Value)
                        //{
                        //    values.Add(items.Specification, items.Description);
                        //}
                        //foreach (var items in AssetReportList.FirstOrDefault().Value)
                        //{
                        //    string value1 = string.Empty;
                        //    Dictionary<Array, int> dic = new Dictionary<Array, int>();
                        //    string[] spec = items.SpecificationsDetails.Split(',');
                        //    for (int i = 0; i < spec.Length; i++)
                        //    {
                        //        if (values.ContainsKey(spec[i]) == true)
                        //        {
                        //            string newvalue = values[spec[i]].ToString() + ",";
                        //            value1 += newvalue;
                        //        }
                        //    }
                        //    items.SpecificationsDetails = value1.TrimEnd(',');
                        //}
                        long totalrecords = AssetReportList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in AssetReportList.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[]{
                                       items.Asset_Id.ToString(),
                                       //items.CampusMaster.Name,
                                       items.AssetType,
                                       items.AssetCode,
                                       items.IsSubAsset==true?"Yes":"No",
                                       items.SpecificationsDetails,                                       
                                       items.CreatedBy!=null?us.GetUserNameByUserId(items.CreatedBy):"",
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
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult AddITAssetTypeMaster(AssetDetailsTemplate assetMaster)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (assetMaster == null) return null;
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Clear();

                    //if (FormId > 0)
                    //    criteria.Add("CampusMaster.FormId", FormId);
                    criteria.Add("AssetCode", assetMaster.AssetCode);
                    Dictionary<long, IList<AssetDetailsTemplate>> AssetCodeList = ass.GetAssetDetailsTemplateWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                    criteria.Clear();
                    criteria.Add("AssetType", assetMaster.AssetType);
                    Dictionary<long, IList<AssetDetailsTemplate>> AssetDetailsList = ass.GetAssetDetailsTemplateWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                    criteria.Clear();
                    Dictionary<long, IList<ITAssetSpecification>> specification = ass.GetITAssetSpecificationWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                    if (AssetDetailsList.FirstOrDefault().Key == 0 && AssetCodeList.FirstOrDefault().Key == 0)
                    {
                        //CampusMaster campusmaster = new CampusMaster();
                        //campusmaster.FormId = FormId;
                        //assetMaster.CampusMaster = campusmaster;

                        assetMaster.AssetType = assetMaster.AssetType.ToUpper();
                        assetMaster.AssetCode = assetMaster.AssetCode.ToUpper();

                        var specs = assetMaster.SpecificationsDetails.Split(new string[] { ",", "null" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        //assetMaster.SpecificationsDetails = string.Join(",", specs.ToArray());
                        Dictionary<string, string> values = new Dictionary<string, string>();
                        foreach (var items in specification.FirstOrDefault().Value)
                        {
                            values.Add(items.Specification, items.Description);
                        }
                        List<string> spec = new List<string>();
                        string SpecificationsDetails = string.Empty;
                        spec.Add("Id");
                        foreach (var item in specs)
                        {
                            spec.Add(item);
                            if (values.ContainsKey(item) == true)
                            {
                                string newvalue = values[item].ToString() + ",";
                                SpecificationsDetails += newvalue;
                            }
                        }
                        string[] specArr = spec.ToArray();
                        string result = string.Join(",", specArr);
                        assetMaster.Specifications = result;
                        assetMaster.SpecificationsDetails = SpecificationsDetails.TrimEnd(',');
                        assetMaster.CreatedBy = userId;
                        assetMaster.CreatedDate = DateTime.Now;
                        ass.CreateOrUpdateITAsset(assetMaster);
                        var Addscript = @"SucessMsg(""Added Successfully"");";
                        return JavaScript(Addscript);
                    }
                    else
                    {
                        var Addscript = @"ErrMsg(""Already Exist!!"");";
                        return JavaScript(Addscript);
                    }

                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult EditITAssetTypeMaster(AssetDetailsTemplate assetMaster)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                {
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Clear();

                    if (assetMaster == null) return null;
                    //if (FormId > 0)
                    //    criteria.Add("CampusMaster.FormId", FormId);
                    if (!string.IsNullOrWhiteSpace(assetMaster.AssetType))
                        criteria.Add("AssetType", assetMaster.AssetType);
                    Dictionary<long, IList<AssetDetailsTemplate>> AssetReportList = ass.GetAssetDetailsTemplateWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                    AssetDetailsTemplate assetDetailsObj = ass.GetAssetDetailsTemplateByAssetId(assetMaster.Asset_Id);
                    if (AssetReportList != null && AssetReportList.First().Key == 0 && assetDetailsObj.Asset_Id == assetMaster.Asset_Id)
                    {
                        //CampusMaster campusmaster = new CampusMaster();
                        //campusmaster.FormId = FormId;
                        //assetDetailsObj.CampusMaster = campusmaster;

                        assetDetailsObj.AssetType = assetMaster.AssetType;
                        assetDetailsObj.Specifications = assetMaster.Specifications;
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        assetDetailsObj.SpecificationsDetails = serializer.Serialize(assetMaster.Specifications);

                        assetDetailsObj.ModifiedBy = userId;
                        assetDetailsObj.ModifiedDate = DateTime.Now;

                        ass.CreateOrUpdateITAsset(assetDetailsObj);
                        var Addscript = @"SucessMsg(""Added Successfully"");";
                        return JavaScript(Addscript);
                    }
                    else
                    {
                        var Addscript = @"ErrMsg(""Already Exist!!"");";
                        return JavaScript(Addscript);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult DeleteITAssetTypeMaster(string Id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    string[] arrayId = Id.Split(',');
                    long[] AssetId = Array.ConvertAll(arrayId, Int64.Parse);
                    ass.DeleteITAssetTypeMaster(AssetId);
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
        public ActionResult ITAssetManagement()
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
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        #region ITAsset ManagementGrid old
        //public JsonResult GetITAssetjqGrid(long AssetId)
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return null;
        //        else
        //        {
        //            Dictionary<string, object> criteria = new Dictionary<string, object>();
        //            criteria.Add("Asset_Id", AssetId);
        //            IList list = null;
        //            if (AssetId > 0)
        //                list = ass.GetITAssetDetailsListbyAssetType(AssetId);

        //            Dictionary<long, IList<AssetDetailsTemplate>> AssetReportList = ass.GetAssetDetailsTemplateWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
        //            if (AssetReportList != null && AssetReportList.FirstOrDefault().Key > 0)
        //            {
        //                var specifications = AssetReportList.FirstOrDefault().Value[0].Specifications.Split(',');


        //                string[] commonColM = { "AssetDet_Id", "AssetCode", "AssetType", "Model", "Make", "SerialNo", "Location", "Asset_Id", "Campus" };
        //                var colNamesList = specifications.Union(commonColM).ToList();

        //                string colNames = string.Empty;
        //                colNames = "[";
        //                if (colNamesList.Count > 0)
        //                {
        //                    foreach (var item in colNamesList)
        //                    {
        //                        colNames = colNames + "'" + item + "',";
        //                    }
        //                }
        //                colNames = colNames + "]";

        //                var colModelList = specifications.Union(commonColM).ToArray();
        //                string colModels = "[";
        //                for (int i = 0; i < colModelList.Length; i++)
        //                {
        //                    if (colModelList[i] == "AssetDet_Id")
        //                        colModels = colModels + "{name:'" + colModelList[i] + "', index:'" + colModelList[i] + "', hidden: true,key:true}";
        //                    else if (colModelList[i] == "Asset_Id" || colModelList[i] == "SpecificationsDetails")
        //                        colModels = colModels + "{name:'" + colModelList[i] + "', index:'" + colModelList[i] + "', hidden: true}";
        //                    else
        //                        colModels = colModels + "{name:'" + colModelList[i] + "', index:'" + colModelList[i] + "', width:'120'}";
        //                    if (i != colModelList.Length - 1)
        //                    {
        //                        colModels = colModels + ",";
        //                    }
        //                }
        //                colModels = colModels + "]";

        //                if (list != null && list.Count > 0)
        //                {
        //                    return Json(new { colNames = colNames, colModel = colModels, list }, JsonRequestBehavior.AllowGet);
        //                }
        //                else
        //                {
        //                    var Empty = new { rows = (new { cell = new string[] { } }) };
        //                    return Json(new { colNames = colNames, colModel = colModels, Empty }, JsonRequestBehavior.AllowGet);
        //                }

        //            }
        //            else
        //            {
        //                var Empty = new { rows = (new { cell = new string[] { } }) };
        //                return Json(Empty, JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
        //        throw ex;
        //    }
        //}
        //******************************************************************************//
        //public JsonResult GetITAssetjqGrid(long AssetId)
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return null;
        //        else
        //        {
        //            Dictionary<string, object> criteria = new Dictionary<string, object>();
        //            criteria.Add("Asset_Id", AssetId);
        //            IList list = null;
        //            if (AssetId > 0)
        //                list = ass.GetITAssetDetailsListbyAssetType(AssetId);

        //            Dictionary<long, IList<AssetDetailsTemplate>> AssetReportList = ass.GetAssetDetailsTemplateWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
        //            if (AssetReportList != null && AssetReportList.FirstOrDefault().Key > 0)
        //            {
        //                var specifications = AssetReportList.FirstOrDefault().Value[0].Specifications.Split(',');

        //                //List<string> colModelList = new List<string>();

        //                //for (int i = 0; i < specifications.Length; i++)
        //                //{
        //                //    if (specifications[i] == "Id")
        //                //        colModelList.Add("{name:'" + specifications[i] + "',index:'" + specifications[i] + "', hidden: true,key:true },");
        //                //    else
        //                //        colModelList.Add("{name:'" + specifications[i] + "',index:'" + specifications[i] + "'},");
        //                //}

        //                string[] commonColM = { "AssetCode", "Location", "SerialNumber", "AssetType", "Asset_Id", "Campus" };
        //                var colNamesList = specifications.Union(commonColM).ToList();

        //                string colNames = string.Empty;
        //                colNames = "[";
        //                if (colNamesList.Count > 0)
        //                {
        //                    foreach (var item in colNamesList)
        //                    {
        //                        //if (item == "Id")
        //                        //    colNames = colNames + "'" + item + "'";
        //                        //else
        //                        //    colNames = colNames + ",'" + item + "'";
        //                        colNames = colNames + "'" + item + "',";
        //                    }
        //                }
        //                colNames = colNames + "]";

        //                var colModelList = specifications.Union(commonColM).ToArray();
        //                string colModels = "[";
        //                for (int i = 0; i < colModelList.Length; i++)
        //                {
        //                    if (colModelList[i] == "Id")
        //                        colModels = colModels + "{name:'" + colModelList[i] + "', index:'" + colModelList[i] + "', hidden: true,key:true}";
        //                    else if (colModelList[i] == "Asset_Id" || colModelList[i] == "SpecificationsDetails")
        //                        colModels = colModels + "{name:'" + colModelList[i] + "', index:'" + colModelList[i] + "', hidden: true}";
        //                    else
        //                        colModels = colModels + "{name:'" + colModelList[i] + "', index:'" + colModelList[i] + "', width:'120'}";
        //                    if (i != colModelList.Length - 1)
        //                    {
        //                        colModels = colModels + ",";
        //                    }
        //                }
        //                colModels = colModels + "]";

        //                if (list != null && list.Count > 0)
        //                {
        //                    return Json(new { colNames = colNames, colModel = colModels, list }, JsonRequestBehavior.AllowGet);
        //                }
        //                else
        //                {
        //                    var Empty = new { rows = (new { cell = new string[] { } }) };
        //                    return Json(new { colNames = colNames, colModel = colModels, Empty }, JsonRequestBehavior.AllowGet);
        //                }

        //            }
        //            else
        //            {
        //                var Empty = new { rows = (new { cell = new string[] { } }) };
        //                return Json(Empty, JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
        //        throw ex;
        //    }
        //}
        //public ActionResult GetITAssetjqGrid(long AssetId)
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //        else
        //        {
        //            Dictionary<string, object> criteria = new Dictionary<string, object>();
        //            criteria.Add("Asset_Id", AssetId);
        //            IList list = null;
        //            if (AssetId > 0)
        //                list = ass.GetITAssetDetailsListbyAssetType(AssetId);

        //            Dictionary<long, IList<AssetDetailsTemplate>> AssetReportList = ass.GetAssetDetailsTemplateWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
        //            if (AssetReportList != null && AssetReportList.FirstOrDefault().Key > 0)
        //            {
        //                var specifications = AssetReportList.FirstOrDefault().Value[0].Specifications.Split(',');

        //                List<string> colModelList = new List<string>();

        //                for (int i = 0; i < specifications.Length; i++)
        //                {
        //                    if (specifications[i] == "Id")
        //                        colModelList.Add("{name:'" + specifications[i] + "',index:'" + specifications[i] + "', hidden: true,key:true },");
        //                    else
        //                        colModelList.Add("{name:'" + specifications[i] + "',index:'" + specifications[i] + "'},");
        //                }

        //                string[] commonColM = { "AssetDet_Id", "AssetCode", "Location", "SerialNumber", "SpecificationsDetails", "AssetType" };
        //                var colNames = specifications.Union(commonColM);
        //                var testcolModel = colModelList.ToArray();

        //                var colModel = colModelList.Union(commonColM);
        //                if (list != null && list.Count > 0)
        //                {
        //                    return Json(new { colNames, colModel, list }, JsonRequestBehavior.AllowGet);
        //                }
        //                else
        //                {
        //                    var Empty = new { rows = (new { cell = new string[] { } }) };
        //                    return Json(new { colNames, colModel, Empty }, JsonRequestBehavior.AllowGet);
        //                }

        //            }
        //            else
        //            {
        //                var Empty = new { rows = (new { cell = new string[] { } }) };
        //                return Json(Empty, JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
        //        throw ex;
        //    }
        //}
        #endregion

        public ActionResult AddNewITAsset(long AssetId)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    AssetDetailsTemplate assetDetailsObj = ass.GetAssetDetailsTemplateByAssetId(AssetId);
                    List<string> specList = assetDetailsObj.Specifications.Split(',').ToList();
                    List<string> DescList = assetDetailsObj.SpecificationsDetails.Split(',').ToList();
                    assetDetailsObj.specList = specList;
                    ViewBag.DescList = DescList;
                    ViewBag.specList = specList;
                    ViewBag.IsSubAsset = assetDetailsObj.IsSubAsset;
                    return View(assetDetailsObj);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ActionResult GetSpecificationlistByAsset(long AssetId)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    AssetDetailsTemplate assetDetailsObj = ass.GetAssetDetailsTemplateByAssetId(AssetId);
                    //string[] specList = assetDetailsObj.Specifications.Split(',').ToArray();                   
                    string[] specList = assetDetailsObj.Specifications.Split(',').ToArray();
                    return Json(specList, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public ActionResult AddAssetDetails(AssetDetails assetDetails, String SpecDetails, long FormId)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (assetDetails == null) return null;
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    if (assetDetails.UserType == "Student")
                    {
                        criteria.Add("Asset_Id", assetDetails.Asset_Id);
                        criteria.Add("UserType", "Student");
                        criteria.Add("IsActive", true);
                        criteria.Add("IdNum", assetDetails.IdNum);
                        Dictionary<long, IList<AssetDetails>> AssetDetails = ass.GetITAssetDetailsWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                        criteria.Clear();
                        if (AssetDetails != null && AssetDetails.FirstOrDefault().Key > 0)
                        {
                            bool Flag = false;
                            return Json(Flag, JsonRequestBehavior.AllowGet);
                        }
                    }
                    //if (!string.IsNullOrEmpty(assetDetails.AssetCode))
                    //    criteria.Add("AssetCode", assetDetails.AssetCode);

                    if (assetDetails.Asset_Id > 0)
                        criteria.Add("Asset_Id", assetDetails.Asset_Id);
                    if (!string.IsNullOrEmpty(assetDetails.Make))
                        criteria.Add("Make", assetDetails.Make);
                    if (!string.IsNullOrEmpty(assetDetails.Model))
                        criteria.Add("Model", assetDetails.Model);
                    if (!string.IsNullOrEmpty(assetDetails.Location))
                        criteria.Add("Location", assetDetails.Location);
                    if (!string.IsNullOrEmpty(assetDetails.SerialNo))
                        criteria.Add("SerialNo", assetDetails.SerialNo);
                    if (FormId > 0)
                        criteria.Add("CampusMaster.FormId", FormId);
                    Dictionary<long, IList<AssetDetails>> AssetList = ass.GetITAssetDetailsWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                    if (AssetList == null || AssetList.FirstOrDefault().Key == 0 || assetDetails.SerialNo == null || assetDetails.SerialNo == "")
                    {
                        //Get AssetCode
                        criteria.Clear();
                        if (assetDetails.Asset_Id > 0)
                            criteria.Add("Asset_Id", assetDetails.Asset_Id);
                        Dictionary<long, IList<AssetDetails>> AssetCont = ass.GetITAssetDetailsWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                        var Assetcount = (from u in AssetCont.FirstOrDefault().Value select u).Distinct().ToList();
                        long count = Assetcount.Count;
                        AssetDetailsTemplate assetDetailsObj = ass.GetAssetDetailsTemplateByAssetId(assetDetails.Asset_Id);
                        assetDetails.AssetCode = "TIPS-" + assetDetailsObj.AssetCode + "-" + (count + 1).ToString();

                        CampusMaster campusmaster = new CampusMaster();
                        campusmaster.FormId = FormId;
                        assetDetails.CampusMaster = campusmaster;
                        assetDetails.Make = assetDetails.Make.ToUpper();
                        assetDetails.Model = assetDetails.Model.ToUpper();
                        assetDetails.Location = assetDetails.Location;
                        assetDetails.FromBlock = assetDetails.FromBlock;
                        assetDetails.TransactionType = "";
                        if (assetDetails.UserType == "Not Applicable")
                        {
                            assetDetails.TransactionType = "Stock";
                        }
                        if (assetDetailsObj.IsSubAsset == true)
                        {
                            assetDetails.SubAssetType = "External";
                            assetDetails.IsSubAsset = true;
                            assetDetails.UserType = null;
                        }
                        //StaffDetailsView staffdetailsview = new StaffDetailsView();
                        //staffdetailsview.PreRegNum = PreRegNum;
                        //assetDetails.StaffDetails = staffdetailsview;
                        //StudentTemplateView studenttemplateview = new StudentTemplateView();
                        //studenttemplateview.PreRegNum = PreRegNum;
                        //assetDetails.StudentTemplate = studenttemplateview;
                        assetDetails.EngineerName = userId;
                        assetDetails.IsActive = true;
                        assetDetails.IsStandBy = false;
                        if (assetDetails.TransactionType == "Stock")
                            assetDetails.IsStandBy = true;
                        assetDetails.CreatedBy = userId;
                        assetDetails.CreatedDate = DateTime.Now;

                        ass.CreateOrUpdateITAssetDetails(assetDetails);

                        #region commented for IT Asset
                        if (assetDetails.InvoiceDetailsId > 0)
                        {
                            AssetInvoiceDetails assetinvoicedetails = ass.GetAssetInvoiceDetailsByInvoiceDetailsId(assetDetails.InvoiceDetailsId);
                            if (assetinvoicedetails != null)
                            {
                                assetinvoicedetails.AssetCount = assetinvoicedetails.AssetCount + 1;
                                if (assetinvoicedetails.TotalAsset == assetinvoicedetails.AssetCount)
                                {
                                    assetinvoicedetails.IsActive = false;
                                }
                                ass.CreateOrUpdateAssetInvoiceDetails(assetinvoicedetails);
                            }
                        }
                        #endregion
                        if (assetDetails.AssetDet_Id > 0)
                        {
                            string[] specList = assetDetailsObj.Specifications.Split(',').ToArray();
                            string[] specValues = SpecDetails.Split(',').ToArray();

                            List<string> specDetailsList = new List<string>();

                            Dictionary<string, object> spec = new Dictionary<string, object>();


                            for (int i = 0; i < specList.Length; i++)
                            {
                                if (specList[i] == "Id")
                                    spec.Add(specList[i], assetDetails.AssetDet_Id);
                                else
                                    spec.Add(specList[i], specValues[i]);
                            }
                            JavaScriptSerializer serializer = new JavaScriptSerializer();
                            assetDetails.SpecificationsDetails = serializer.Serialize(spec);

                            CampusMaster campus = ass.GetAssetDetailsTemplateByFormId(assetDetails.CampusMaster.FormId);
                            assetDetails.FromCampus = campus.Name;
                            if (assetDetails.IsSubAsset == false)
                            {
                                assetDetails.CurrentCampus = campus.Name;
                                assetDetails.CurrentLocation = assetDetails.Location;
                                assetDetails.CurrentBlock = assetDetails.FromBlock;
                            }
                            ass.CreateOrUpdateITAssetDetails(assetDetails);
                            ITAssetDetailsTransactionHistory transactionhistory = new ITAssetDetailsTransactionHistory();
                            transactionhistory.AssetDetails = assetDetails;
                            transactionhistory.FromCampus = assetDetails.FromCampus;
                            transactionhistory.FromBlock = assetDetails.FromBlock;
                            transactionhistory.FromLocation = assetDetails.Location;
                            if (assetDetails.IsSubAsset == false)
                            {
                                transactionhistory.ToCampus = assetDetails.CurrentCampus;
                                transactionhistory.ToLocation = assetDetails.CurrentLocation;
                                transactionhistory.ToBlock = assetDetails.CurrentBlock;
                            }
                            transactionhistory.UserType = assetDetails.UserType;
                            transactionhistory.IdNum = assetDetails.IdNum;
                            transactionhistory.CreatedBy = userId;
                            transactionhistory.CreatedDate = DateTime.Now;
                            transactionhistory.TransactionType = "Initial";
                            transactionhistory.Amount = assetDetails.Amount;
                            transactionhistory.InvoiceDetailsId = assetDetails.InvoiceDetailsId;
                            transactionhistory.Warranty = assetDetails.Warranty;
                            transactionhistory.IsSubAsset = assetDetails.IsSubAsset;
                            transactionhistory.AssetRefId = assetDetails.AssetRefId;
                            if (assetDetails.UserType == "Not Applicable")
                            {
                                transactionhistory.TransactionType = "Stock";
                            }
                            transactionhistory.TransactionType_Id = assetDetails.AssetDet_Id;
                            ass.CreateOrUpdateITAssetDetailsHistory(transactionhistory);
                        }
                        bool Flag = true;
                        return Json(Flag, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        bool Flag = false;
                        return Json(Flag, JsonRequestBehavior.AllowGet);
                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #region Build ColModels and ColNames
        public JsonResult GetITAssetManagementColModelAndColName(long AssetId)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return null;
                else
                {
                    IList list = null;
                    if (AssetId > 0)
                        list = ass.GetITAssetDetailsListbyAssetType(AssetId);

                    string[] ColModelData = BuildColModelDataByAssetType(AssetId);
                    string ColNames = BuildColNamesByAssetType(AssetId);
                    string ColModels = "[";
                    for (var i = 0; i < ColModelData.Length; i++)
                    {
                        if (ColModelData[i] == "AssetDet_Id")
                            ColModels = ColModels + "{name:'" + ColModelData[i] + "', index:'" + ColModelData[i] + "', hidden: true,key:true}";
                        else if (ColModelData[i] == "AssetCode")
                            ColModels = ColModels + "{name:'" + ColModelData[i] + "', index:'" + ColModelData[i] + "', formatter:showAssetTransactionFormat}";
                        else if (ColModelData[i] == "Id" || ColModelData[i] == "Asset_Id" || ColModelData[i] == "ModifiedBy" || ColModelData[i] == "ModifiedDate")
                            ColModels = ColModels + "{name:'" + ColModelData[i] + "', index:'" + ColModelData[i] + "', hidden: true}";
                        else
                            ColModels = ColModels + "{name:'" + ColModelData[i] + "', index:'" + ColModelData[i] + "', width:'120'}";
                        if (i != ColModelData.Length - 1)
                        {
                            ColModels = ColModels + ",";
                        }
                    }
                    ColModels = ColModels + "]";
                    ;
                    return Json(new { ColNames = ColNames, ColModels = ColModels, list }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public string[] BuildColModelDataByAssetType(long AssetId)
        {
            try
            {
                string userId = base.ValidateUser();
                AssetService assetServ = new AssetService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Clear();
                criteria.Add("Asset_Id", AssetId);
                Dictionary<long, IList<AssetDetailsTemplate>> AssetReportList = assetServ.GetAssetDetailsTemplateWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                var specifications = AssetReportList.FirstOrDefault().Value[0].Specifications.Split(',').ToArray();
                long TotalColModels = specifications.Count();
                string[] FirstHalfCommonColModels = GetFirstHalfCommonColModels();
                string[] SecondHalfCommonColModels = GetSecondHalfCommonColModels();
                string[] ColModelData = new string[TotalColModels];
                IList<string> ColModelDateList = new List<string>();
                if (string.IsNullOrWhiteSpace(userId)) return ColModelData;
                else
                {
                    foreach (var FirstHalfItem in FirstHalfCommonColModels)
                    {
                        ColModelDateList.Add(FirstHalfItem);
                    }
                    if (AssetReportList != null && AssetReportList.FirstOrDefault().Key > 0)
                    {
                        foreach (var item in specifications)
                        {
                            if (!string.IsNullOrEmpty(item))
                                ColModelDateList.Add(item);
                        }
                    }
                    foreach (var SecondHalfItem in SecondHalfCommonColModels)
                    {
                        ColModelDateList.Add(SecondHalfItem);
                    }
                    return ColModelDateList.ToArray();
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public string BuildColNamesByAssetType(long AssetId)
        {
            try
            {
                string ColNamesByCampus = string.Empty;
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return ColNamesByCampus;
                else
                {
                    AssetService assetServ = new AssetService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Clear();
                    criteria.Add("Asset_Id", AssetId);
                    Dictionary<long, IList<AssetDetailsTemplate>> AssetReportList = assetServ.GetAssetDetailsTemplateWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                    var specifications = AssetReportList.FirstOrDefault().Value[0].Specifications.Split(',').ToArray();
                    long TotalColModels = specifications.Count();
                    string[] FirstHalfCommonColModels = GetFirstHalfCommonColModels();
                    string[] SecondHalfCommonColModels = GetSecondHalfCommonColModels();
                    string[] ColModelData = new string[TotalColModels];
                    IList<string> ColModelDateList = new List<string>();
                    foreach (var FirstHalfItem in FirstHalfCommonColModels)
                    {
                        ColModelDateList.Add(FirstHalfItem);
                    }
                    if (AssetReportList != null && AssetReportList.FirstOrDefault().Key > 0)
                    {
                        foreach (var item in specifications)
                        {
                            if (!string.IsNullOrEmpty(item))
                                ColModelDateList.Add(item);
                        }
                    }
                    foreach (var SecondHalfItem in SecondHalfCommonColModels)
                    {
                        ColModelDateList.Add(SecondHalfItem);
                    }
                    ColNamesByCampus = "[";
                    if (ColModelDateList.Count > 0)
                    {
                        for (int i = 0; i < ColModelDateList.Count; i++)
                        {
                            if (i == 0)
                                ColNamesByCampus = ColNamesByCampus + "'" + ColModelDateList[i] + "'";
                            else
                                ColNamesByCampus = ColNamesByCampus + ",'" + ColModelDateList[i] + "'";
                        }
                    }
                    ColNamesByCampus = ColNamesByCampus + "]";
                    return ColNamesByCampus;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public string[] GetFirstHalfCommonColModels()
        {
            try
            {
                string[] FirstHalfCommonColModels = { "AssetDet_Id", "AssetCode", "AssetType", "Model", "Make", "SerialNo", "Location", "Asset_Id", "Campus" };
                return FirstHalfCommonColModels;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public string[] GetSecondHalfCommonColModels()
        {
            try
            {
                string[] SecondHalfCommonColModels = { "CreatedBy", "CreatedDate", "ModifiedBy", "ModifiedDate" };
                return SecondHalfCommonColModels;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region Added by prabakaran IT Asset Management Grid
        public ActionResult ITAssetManagementjqGrid(AssetDetails assetdetails, long? FormId, long? AssetRefId, bool IsSubAsset, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    sord = sord == "desc" ? "Desc" : "Asc";
                    if (sidx == "FormId") sidx = "CampusMaster.FormId";
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<string, object> likecriteria = new Dictionary<string, object>();
                    criteria.Clear();
                    if (assetdetails != null)
                    {

                        if (!string.IsNullOrEmpty(assetdetails.AssetCode))
                            likecriteria.Add("AssetCode", assetdetails.AssetCode);
                        //if (!string.IsNullOrEmpty(assetdetails.AssetType))
                        //    likecriteria.Add("AssetType", assetdetails.AssetType);
                        if (!string.IsNullOrEmpty(assetdetails.Make))
                            likecriteria.Add("Make", assetdetails.Make);
                        if (!string.IsNullOrEmpty(assetdetails.Model))
                            likecriteria.Add("Model", assetdetails.Model);
                        if (!string.IsNullOrEmpty(assetdetails.SerialNo))
                            likecriteria.Add("SerialNo", assetdetails.SerialNo);
                        if (!string.IsNullOrEmpty(assetdetails.CurrentBlock))
                            criteria.Add("CurrentBlock", assetdetails.CurrentBlock);
                        if (!string.IsNullOrEmpty(assetdetails.CurrentLocation))
                            criteria.Add("CurrentLocation", assetdetails.CurrentLocation);
                        if (!string.IsNullOrEmpty(assetdetails.TransactionType))
                            criteria.Add("TransactionType", assetdetails.TransactionType);
                        if (!string.IsNullOrEmpty(assetdetails.UserType))
                            criteria.Add("UserType", assetdetails.UserType);
                    }
                    if (FormId > 0)
                    {
                        likecriteria.Add("CampusMaster.FormId", FormId);
                    }
                    criteria.Add("IsSubAsset", IsSubAsset);
                    if (AssetRefId != null)
                    {
                        criteria.Add("AssetRefId", AssetRefId);
                    }
                    if (!string.IsNullOrEmpty(assetdetails.AssetType))
                        criteria.Add("Asset_Id", Convert.ToInt64(assetdetails.AssetType));
                    Dictionary<long, IList<AssetDetails>> AssetDetailsList = ass.GetITAssetDetailsWithPagingAndExactAndLikeCriteria(page - 1, rows, sidx, sord, criteria, likecriteria);
                    if (AssetDetailsList != null && AssetDetailsList.Count > 0)
                    {
                        StaffManagementService sms = new StaffManagementService();
                        AdmissionManagementService abc = new AdmissionManagementService();
                        long totalrecords = AssetDetailsList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in AssetDetailsList.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[]{
                                       items.AssetDet_Id.ToString(),
                                       //items.AssetCode!=null?"<a href='/Asset/ITAssetTransaction?AssetDet_Id=" + items.AssetDet_Id +"'>" + items.AssetCode + "</a>":"",
                                       items.AssetCode,
                                       items.Asset_Id.ToString(),
                                       items.AssetType,
                                       items.Make,
                                       items.Model,
                                       items.SerialNo,
                                       items.TransactionType,
                                       //items.CampusMaster!=null?items.CampusMaster.Name:"",
                                       items.CurrentCampus,
                                       items.CurrentBlock,
                                       items.CurrentLocation,
                                       items.UserType,                                       
                                       //items.UserType.Contains("Staff")?sms.GetStaffNameByPreRegNum(items.PreRegNum):items.UserType.Contains("Student")?abc.GetStudentNameByPreRegNum(items.PreRegNum):items.UserType.Contains("Common")?"":"",                                       
                                       items.UserType.Contains("Staff")?items.StaffDetailsView.Name:items.UserType.Contains("Student")?items.StudentTemplateView.Name:items.UserType.Contains("Common")?"":"",                                       
                                       items.CreatedBy,
                                       items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy"):"",
                                       items.ModifiedBy,
                                       items.ModifiedDate!=null?items.ModifiedDate.Value.ToString("dd/MM/yyyy"):"",
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
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        #endregion
        public ActionResult ITAssetTransaction(long AssetDet_Id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MastersService ms = new MastersService();
                    AssetService assetServ = new AssetService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Clear();
                    AssetDetails assetDetails = new AssetDetails();
                    AssetDetailsTemplate assetDetailsTemplate = new AssetDetailsTemplate();
                    var usrcmp = Session["UserCampus"] as IEnumerable<string>;
                    if (usrcmp.First() != null)            // to check if the usrcmp obj is null or with data
                    {
                        criteria.Add("Name", usrcmp);
                    }
                    Dictionary<long, IList<CampusMaster>> CampusMaster = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                    criteria.Clear();
                    criteria.Add("AssetDetails.AssetDet_Id", AssetDet_Id);
                    Dictionary<long, IList<ITAssetServiceDetails>> serviceDetails = ass.GetITAssetServiceDetailsWithPagingAndCriteria(0, 99999, "AssetService_Id", "desc", criteria);
                    if (AssetDet_Id > 0)
                    {
                        assetDetails = assetServ.GetAssetDetailsByAssetId(AssetDet_Id);
                        assetDetailsTemplate = assetServ.GetAssetDetailsTemplateByAssetId(assetDetails.Asset_Id);
                    }
                    if (serviceDetails != null && serviceDetails.FirstOrDefault().Key > 0)
                    {
                        var DCDate = (from u in serviceDetails.FirstOrDefault().Value where u.InwardDate == null select u.DCDate).ToArray();
                        if (DCDate.Length > 0)
                        {
                            ViewBag.DCDate = DCDate[0].Value.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            ViewBag.DCDate = "";
                        }
                    }
                    else
                    {
                        ViewBag.DCDate = "";
                    }
                    var jsonString = assetDetails.SpecificationsDetails;
                    var json = JValue.Parse(jsonString);
                    var specList = (from u in json select u).ToList();
                    ViewBag.specList = specList;
                    ViewBag.DescList = assetDetailsTemplate.SpecificationsDetails.Split(',');
                    ViewBag.campusddl = CampusMaster.First().Value;
                    AssetDetails ParentassetDetails = new AssetDetails();
                    ViewBag.AssetCode = ParentassetDetails != null ? ParentassetDetails.AssetCode : "";
                    return View(assetDetails);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpPost]
        public ActionResult ITAssetTransaction(AssetDetails assetDetails)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MastersService ms = new MastersService();
                    AssetService assetServ = new AssetService();
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Clear();
                    var usrcmp = Session["UserCampus"] as IEnumerable<string>;
                    if (usrcmp.First() != null)            // to check if the usrcmp obj is null or with data
                    {
                        criteria.Add("Name", usrcmp);
                    }
                    Dictionary<long, IList<CampusMaster>> CampusMaster = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                    criteria.Clear();
                    ViewBag.campusddl = CampusMaster.First().Value;
                    AssetDetails assetDetailsObj = new AssetDetails();
                    if (assetDetails.Asset_Id > 0)
                        assetDetailsObj = assetServ.GetAssetDetailsByAssetId(assetDetails.AssetDet_Id);
                    var jsonString = assetDetailsObj.SpecificationsDetails;
                    var json = JValue.Parse(jsonString);
                    var specList = (from u in json select u).ToList();
                    ViewBag.specList = specList;
                    ITAssetDetailsTransactionHistory history = new ITAssetDetailsTransactionHistory();
                    if (Request.Form["btnSave"] == "SaveAsset" && assetDetails.IsSubAsset == false)
                    {
                        if (assetDetailsObj != null && assetDetails.TransactionType == "IntraCampus" || assetDetails.TransactionType == "InterCampus" || assetDetails.TransactionType == "Stock")
                        {
                            if (assetDetails.UserType == "Student" && assetDetails.TransactionType == "InterCampus" && assetDetailsObj.StudentTemplateView.Campus != assetDetails.AssetDetailsTransaction.ToCampus)
                            {
                                return RedirectToAction("ITAssetTransaction", new { assetDetails.AssetDet_Id });
                            }
                            criteria.Clear();
                            criteria.Add("Asset_Id", assetDetails.Asset_Id);
                            criteria.Add("UserType", "Student");
                            criteria.Add("IsActive", true);
                            criteria.Add("IdNum", assetDetails.IdNum);
                            Dictionary<long, IList<AssetDetails>> AssetDetails = ass.GetITAssetDetailsWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                            if (AssetDetails != null && AssetDetails.FirstOrDefault().Key > 0 && assetDetails.UserType == "Student" && assetDetailsObj.UserType == "Not Applicable" && assetDetailsObj.Location == "Stock")
                            {
                                return RedirectToAction("ITAssetTransaction", new { assetDetails.AssetDet_Id });
                            }
                            //else if (assetDetails.UserType == "Staff" && assetDetailsObj.StaffDetailsView.Campus != assetDetails.AssetDetailsTransaction.ToCampus)
                            //{

                    //    return RedirectToAction("ITAssetTransaction", new { assetDetails.AssetDet_Id });
                            //}
                            else
                            {
                                //AssetDetailsTransaction AssetDetailsTransaction = new AssetDetailsTransaction();
                                assetDetails.AssetDetailsTransaction.AssetDet_Id = assetDetailsObj.AssetDet_Id;
                                assetDetails.AssetDetailsTransaction.AssetCode = assetDetailsObj.AssetCode;
                                assetDetails.AssetDetailsTransaction.FromCampus = assetDetailsObj.CurrentCampus;
                                assetDetails.AssetDetailsTransaction.FromBlock = assetDetailsObj.CurrentBlock;
                                assetDetails.AssetDetailsTransaction.FromLocation = assetDetailsObj.CurrentLocation;
                                history.FromCampus = assetDetailsObj.CurrentCampus;
                                history.FromBlock = assetDetailsObj.CurrentBlock;
                                history.FromLocation = assetDetailsObj.CurrentLocation;
                                //assetDetails.AssetDetailsTransaction.ToBlock = assetDetailsObj.CurrentBlock;
                                //assetDetails.AssetDetailsTransaction.ToLocation = assetDetailsObj.Location;
                                if (assetDetails.TransactionType == "IntraCampus")
                                {
                                    assetDetails.AssetDetailsTransaction.ToCampus = assetDetailsObj.CurrentCampus;
                                }
                                if (assetDetails.TransactionType == "Stock")
                                {
                                    assetDetails.AssetDetailsTransaction.ToCampus = assetDetailsObj.CurrentCampus;
                                    assetDetails.AssetDetailsTransaction.FromCampus = assetDetailsObj.CurrentCampus;
                                    //assetDetails.AssetDetailsTransaction.FromBlock = "Stock";
                                    assetDetails.AssetDetailsTransaction.ToBlock = "Stock";
                                    //assetDetails.AssetDetailsTransaction.FromLocation = "Stock";
                                    assetDetails.AssetDetailsTransaction.ToLocation = "Stock";

                                }
                                assetDetails.AssetDetailsTransaction.InstalledOn = DateTime.ParseExact(Request.Form["InstalledOn"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                assetDetails.AssetDetailsTransaction.EngineerName = userId;
                                assetDetails.AssetDetailsTransaction.CreatedBy = userId;
                                assetDetails.AssetDetailsTransaction.CreatedDate = DateTime.Now;
                                assetServ.CreateOrUpdateAssetDetailsTransaction(assetDetails.AssetDetailsTransaction);
                                history.TransactionType_Id = assetDetails.AssetDetailsTransaction.AssetTrans_Id;
                                if (assetDetails.AssetDetailsTransaction.AssetTrans_Id > 0)
                                {
                                    assetDetailsObj.CurrentCampus = assetDetails.AssetDetailsTransaction.ToCampus;
                                    assetDetailsObj.CurrentBlock = assetDetails.AssetDetailsTransaction.ToBlock;
                                    assetDetailsObj.CurrentLocation = assetDetails.AssetDetailsTransaction.ToLocation;
                                    assetDetailsObj.TransactionType = assetDetails.TransactionType;
                                    assetDetailsObj.IsStandBy = false;
                                    if (assetDetails.TransactionType == "Stock")
                                    {
                                        assetDetailsObj.IsStandBy = true;
                                    }
                                    assetDetailsObj.IsActive = true;
                                    assetDetailsObj.EngineerName = userId;
                                    assetDetailsObj.ModifiedBy = userId;
                                    assetDetailsObj.ModifiedDate = DateTime.Now;
                                    assetDetailsObj.IdNum = assetDetails.IdNum;
                                    assetDetailsObj.UserType = assetDetails.UserType;
                                    assetServ.CreateOrUpdateITAssetDetails(assetDetailsObj);
                                }
                            }
                        }
                        if (assetDetails.TransactionType == "Service")
                        {
                            //ITAssetServiceDetails ITAssetServiceDetails = new ITAssetServiceDetails();                            
                            assetDetails.ITAssetServiceDetails.AssetDetails = assetDetailsObj;
                            assetDetails.ITAssetServiceDetails.EngineerName = userId;
                            assetDetails.ITAssetServiceDetails.FromCampus = assetDetailsObj.CurrentCampus;
                            assetDetails.ITAssetServiceDetails.FromBlock = assetDetailsObj.CurrentBlock;
                            assetDetails.ITAssetServiceDetails.FromLocation = assetDetailsObj.CurrentLocation;
                            history.FromCampus = assetDetailsObj.CurrentCampus;
                            history.FromBlock = assetDetailsObj.CurrentBlock;
                            history.FromLocation = assetDetailsObj.CurrentLocation;
                            assetDetails.ITAssetServiceDetails.DCDate = assetDetails.AssetDetailsTransaction.InstalledOn = DateTime.ParseExact(Request.Form["ITAssetServiceDetails.DCDate"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            assetDetails.ITAssetServiceDetails.ExpectedDate = DateTime.ParseExact(Request.Form["ITAssetServiceDetails.ExpectedDate"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            assetDetails.ITAssetServiceDetails.CreatedBy = userId;
                            assetDetails.ITAssetServiceDetails.CreatedDate = DateTime.Now;

                            assetServ.CreateOrUpdateITAssetServiceDetails(assetDetails.ITAssetServiceDetails);
                            history.TransactionType_Id = assetDetails.ITAssetServiceDetails.AssetService_Id;
                            //history.UserType = assetDetailsObj.UserType;

                            if (assetDetails.ITAssetServiceDetails.AssetService_Id > 0)
                            {
                                assetDetailsObj.CurrentCampus = assetDetails.TransactionType;
                                assetDetailsObj.CurrentLocation = assetDetails.ITAssetServiceDetails.Vendor;
                                assetDetailsObj.CurrentBlock = assetDetails.ITAssetServiceDetails.Vendor;
                                assetDetailsObj.TransactionType = assetDetails.TransactionType;
                                assetDetailsObj.InstalledOn = assetDetails.ITAssetServiceDetails.DCDate;
                                assetDetailsObj.IsActive = true;
                                assetDetailsObj.IsStandBy = false;
                                assetDetailsObj.EngineerName = userId;
                                assetDetailsObj.ModifiedBy = userId;
                                assetDetailsObj.ModifiedDate = DateTime.Now;
                                assetServ.CreateOrUpdateITAssetDetails(assetDetailsObj);
                            }
                        }
                        if (assetDetails.TransactionType == "Scrap")
                        {
                            //ITAssetScrapDetails ITAssetScrapDetails = new ITAssetScrapDetails();

                            assetDetails.ITAssetScrapDetails.AssetDetails = assetDetailsObj;
                            assetDetails.ITAssetScrapDetails.InwardDate = DateTime.ParseExact(Request.Form["ITAssetScrapDetails.InwardDate"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            assetDetails.ITAssetScrapDetails.EngineerName = userId;
                            assetDetails.ITAssetScrapDetails.FromCampus = assetDetails.CurrentCampus;
                            assetDetails.ITAssetScrapDetails.FromBlock = assetDetails.CurrentBlock;
                            assetDetails.ITAssetScrapDetails.FromLocation = assetDetails.CurrentLocation;
                            history.FromCampus = assetDetails.CurrentCampus;
                            history.FromBlock = assetDetails.CurrentBlock;
                            history.FromLocation = assetDetails.CurrentLocation;
                            assetDetails.ITAssetScrapDetails.CreatedBy = userId;
                            assetDetails.ITAssetScrapDetails.CreatedDate = DateTime.Now;
                            assetServ.CreateOrUpdateITAssetScrapDetails(assetDetails.ITAssetScrapDetails);
                            history.TransactionType_Id = assetDetails.ITAssetScrapDetails.AssetScrap_Id;
                            if (assetDetails.ITAssetScrapDetails.AssetScrap_Id > 0)
                            {
                                //assetDetailsObj.CurrentCampus = assetDetails.FromCampus;
                                assetDetailsObj.CurrentCampus = assetDetails.CurrentCampus;//changed by prabakaran
                                assetDetailsObj.CurrentLocation = "Scrap";
                                assetDetailsObj.CurrentBlock = "Scrap";
                                assetDetailsObj.InstalledOn = assetDetails.ITAssetScrapDetails.InwardDate;
                                assetDetailsObj.TransactionType = assetDetails.TransactionType;
                                assetDetailsObj.IsActive = false;
                                assetDetailsObj.IsStandBy = false;
                                assetDetailsObj.EngineerName = userId;
                                assetDetailsObj.ModifiedBy = userId;
                                assetDetailsObj.ModifiedDate = DateTime.Now;
                                assetServ.CreateOrUpdateITAssetDetails(assetDetailsObj);
                                criteria.Clear();
                                criteria.Add("AssetRefId", assetDetailsObj.AssetDet_Id);
                                criteria.Add("IsActive", true);
                                criteria.Add("IsSubAsset", true);
                                Dictionary<long, IList<AssetDetails>> SubAssetDetails = assetServ.GetITAssetDetailsWithPagingAndCriteria(0, 9999, "AssetDet_Id", "Asc", criteria);
                                if (SubAssetDetails != null && SubAssetDetails.FirstOrDefault().Key > 0)
                                {
                                    var SubAssetDetailscraplist = (from u in SubAssetDetails.FirstOrDefault().Value where u.TransactionType != "Service" select u).ToList();
                                    if (SubAssetDetailscraplist != null && SubAssetDetailscraplist.Count > 0)
                                    {
                                        foreach (var items in SubAssetDetailscraplist)
                                        {

                                            items.TransactionType = "Scrap";
                                            items.IsActive = false;
                                            items.IsStandBy = false;
                                            items.EngineerName = userId;
                                            items.ModifiedBy = userId;
                                            items.ModifiedDate = DateTime.Now;
                                            assetServ.CreateOrUpdateITAssetDetails(items);
                                            ITAssetScrapDetails assetscrapdetails = new ITAssetScrapDetails();
                                            ITAssetDetailsTransactionHistory subassettransactionhistory = new ITAssetDetailsTransactionHistory();
                                            assetscrapdetails.AssetDetails = items;
                                            assetscrapdetails.InwardDate = DateTime.ParseExact(Request.Form["ITAssetScrapDetails.InwardDate"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                            assetscrapdetails.EngineerName = userId;
                                            assetscrapdetails.FromCampus = assetDetailsObj.CurrentCampus;
                                            assetscrapdetails.FromBlock = assetDetailsObj.CurrentBlock;
                                            assetscrapdetails.FromLocation = assetDetailsObj.CurrentLocation;
                                            assetscrapdetails.CreatedBy = userId;
                                            assetscrapdetails.CreatedDate = DateTime.Now;
                                            assetServ.CreateOrUpdateITAssetScrapDetails(assetDetails.ITAssetScrapDetails);

                                            subassettransactionhistory.FromCampus = assetDetailsObj.CurrentCampus;
                                            subassettransactionhistory.FromBlock = assetDetailsObj.CurrentBlock;
                                            subassettransactionhistory.FromLocation = assetDetailsObj.CurrentLocation;
                                            subassettransactionhistory.TransactionType_Id = assetscrapdetails.AssetScrap_Id;
                                            subassettransactionhistory.AssetDetails = assetDetailsObj;
                                            subassettransactionhistory.TransactionType = items.TransactionType;
                                            subassettransactionhistory.ToBlock = "Scrap";
                                            subassettransactionhistory.ToLocation = "Scrap";
                                            subassettransactionhistory.ToCampus = assetDetailsObj.CurrentCampus;
                                            subassettransactionhistory.CreatedBy = userId;
                                            subassettransactionhistory.CreatedDate = DateTime.Now;
                                            subassettransactionhistory.InvoiceDetailsId = items.InvoiceDetailsId;
                                            subassettransactionhistory.Warranty = items.Warranty;
                                            subassettransactionhistory.AssetRefId = items.AssetRefId;
                                            subassettransactionhistory.IsSubAsset = items.IsSubAsset;
                                            subassettransactionhistory.Amount = items.Amount;
                                            subassettransactionhistory.IdNum = items.IdNum;
                                            subassettransactionhistory.UserType = items.UserType;
                                            assetServ.CreateOrUpdateITAssetDetailsHistory(subassettransactionhistory);
                                        }
                                    }
                                }
                            }
                        }
                        history.AssetDetails = assetDetailsObj;
                        history.TransactionType = assetDetails.TransactionType;
                        //history.FromCampus = assetDetailsObj.FromCampus;
                        //history.FromLocation = assetDetailsObj.Location;
                        //history.FromBlock = assetDetailsObj.FromBlock;                        
                        history.ToBlock = assetDetailsObj.CurrentBlock;
                        history.ToLocation = assetDetailsObj.CurrentLocation;
                        history.ToCampus = assetDetailsObj.CurrentCampus;
                        history.CreatedBy = userId;
                        history.CreatedDate = DateTime.Now;
                        history.UserType = assetDetailsObj.UserType;
                        history.IdNum = assetDetailsObj.IdNum;
                        history.InvoiceDetailsId = assetDetailsObj.InvoiceDetailsId;
                        history.Warranty = assetDetailsObj.Warranty;
                        //if (assetDetails.UserType == "Student")
                        //{
                        //    history.UserType = assetDetailsObj.UserType;
                        //    history.IdNum = assetDetailsObj.IdNum;
                        //}
                        //else
                        //{
                        //    history.UserType = assetDetails.UserType;
                        //    history.IdNum = assetDetails.IdNum;
                        //}
                        assetServ.CreateOrUpdateITAssetDetailsHistory(history);
                        return RedirectToAction("ITAssetManagement");
                    }
                    if (Request.Form["btnSave"] == "SaveAsset" && assetDetails.IsSubAsset == true)
                    {
                        if (assetDetailsObj != null && assetDetails.TransactionType == "IntraCampus" || assetDetails.TransactionType == "InterCampus" || assetDetails.TransactionType == "Stock")
                        {
                            if (assetDetails.UserType == "Student" && assetDetails.TransactionType == "InterCampus" && assetDetailsObj.StudentTemplateView.Campus != assetDetails.AssetDetailsTransaction.ToCampus)
                            {
                                return RedirectToAction("ITAssetTransaction", new { assetDetails.AssetDet_Id });
                            }
                            else
                            {
                                assetDetails.AssetDetailsTransaction.AssetDet_Id = assetDetailsObj.AssetDet_Id;
                                assetDetails.AssetDetailsTransaction.AssetCode = assetDetailsObj.AssetCode;
                                assetDetails.AssetDetailsTransaction.FromCampus = assetDetailsObj.CurrentCampus;
                                assetDetails.AssetDetailsTransaction.FromBlock = assetDetailsObj.CurrentBlock;
                                assetDetails.AssetDetailsTransaction.FromLocation = assetDetailsObj.CurrentLocation;
                                assetDetails.AssetDetailsTransaction.AssetRefId = assetDetails.AssetRefId;
                                assetDetails.AssetDetailsTransaction.IsSubAsset = assetDetails.IsSubAsset;
                                history.FromCampus = assetDetailsObj.CurrentCampus;
                                history.FromBlock = assetDetailsObj.CurrentBlock;
                                history.FromLocation = assetDetailsObj.CurrentLocation;
                                if (assetDetails.TransactionType == "IntraCampus")
                                {
                                    assetDetails.AssetDetailsTransaction.ToCampus = assetDetailsObj.CurrentCampus;
                                }
                                if (assetDetails.TransactionType == "Stock")
                                {
                                    assetDetails.AssetDetailsTransaction.ToCampus = assetDetailsObj.CurrentCampus;
                                    assetDetails.AssetDetailsTransaction.FromCampus = assetDetailsObj.CurrentCampus;
                                    assetDetails.AssetDetailsTransaction.ToBlock = "Stock";
                                    assetDetails.AssetDetailsTransaction.ToLocation = "Stock";
                                }
                                assetDetails.AssetDetailsTransaction.InstalledOn = DateTime.ParseExact(Request.Form["InstalledOn"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                assetDetails.AssetDetailsTransaction.EngineerName = userId;
                                assetDetails.AssetDetailsTransaction.CreatedBy = userId;
                                assetDetails.AssetDetailsTransaction.CreatedDate = DateTime.Now;
                                assetServ.CreateOrUpdateAssetDetailsTransaction(assetDetails.AssetDetailsTransaction);
                                history.TransactionType_Id = assetDetails.AssetDetailsTransaction.AssetTrans_Id;
                                if (assetDetails.AssetDetailsTransaction.AssetTrans_Id > 0)
                                {
                                    assetDetailsObj.CurrentCampus = assetDetails.AssetDetailsTransaction.ToCampus;
                                    assetDetailsObj.CurrentBlock = assetDetails.AssetDetailsTransaction.ToBlock;
                                    assetDetailsObj.CurrentLocation = assetDetails.AssetDetailsTransaction.ToLocation;
                                    assetDetailsObj.TransactionType = assetDetails.TransactionType;
                                    assetDetailsObj.IsStandBy = false;
                                    if (assetDetails.TransactionType == "Stock")
                                    {
                                        assetDetailsObj.IsStandBy = true;
                                    }
                                    assetDetailsObj.IsActive = true;
                                    assetDetailsObj.EngineerName = userId;
                                    assetDetailsObj.ModifiedBy = userId;
                                    assetDetailsObj.ModifiedDate = DateTime.Now;
                                    assetDetailsObj.IdNum = assetDetails.IdNum;
                                    assetDetailsObj.UserType = assetDetails.UserType;
                                    assetDetailsObj.AssetRefId = assetDetails.AssetRefId;
                                    assetServ.CreateOrUpdateITAssetDetails(assetDetailsObj);
                                }
                            }
                        }
                        if (assetDetails.TransactionType == "Service")
                        {
                            assetDetails.ITAssetServiceDetails.AssetDetails = assetDetailsObj;
                            assetDetails.ITAssetServiceDetails.EngineerName = userId;
                            assetDetails.ITAssetServiceDetails.FromCampus = assetDetailsObj.CurrentCampus;
                            assetDetails.ITAssetServiceDetails.FromBlock = assetDetailsObj.CurrentBlock;
                            assetDetails.ITAssetServiceDetails.FromLocation = assetDetailsObj.CurrentLocation;
                            assetDetails.ITAssetServiceDetails.AssetRefId = assetDetails.AssetRefId;
                            assetDetails.ITAssetServiceDetails.IsSubAsset = assetDetails.IsSubAsset;
                            history.FromCampus = assetDetailsObj.CurrentCampus;
                            history.FromBlock = assetDetailsObj.CurrentBlock;
                            history.FromLocation = assetDetailsObj.CurrentLocation;
                            assetDetails.ITAssetServiceDetails.DCDate = assetDetails.AssetDetailsTransaction.InstalledOn = DateTime.ParseExact(Request.Form["ITAssetServiceDetails.DCDate"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            assetDetails.ITAssetServiceDetails.ExpectedDate = DateTime.ParseExact(Request.Form["ITAssetServiceDetails.ExpectedDate"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            assetDetails.ITAssetServiceDetails.CreatedBy = userId;
                            assetDetails.ITAssetServiceDetails.CreatedDate = DateTime.Now;
                            assetServ.CreateOrUpdateITAssetServiceDetails(assetDetails.ITAssetServiceDetails);
                            history.TransactionType_Id = assetDetails.ITAssetServiceDetails.AssetService_Id;
                            if (assetDetails.ITAssetServiceDetails.AssetService_Id > 0)
                            {
                                assetDetailsObj.CurrentCampus = assetDetails.TransactionType;
                                assetDetailsObj.CurrentLocation = assetDetails.ITAssetServiceDetails.Vendor;
                                assetDetailsObj.CurrentBlock = assetDetails.ITAssetServiceDetails.Vendor;
                                assetDetailsObj.TransactionType = assetDetails.TransactionType;
                                assetDetailsObj.InstalledOn = assetDetails.ITAssetServiceDetails.DCDate;
                                assetDetailsObj.IsActive = true;
                                assetDetailsObj.IsStandBy = false;
                                assetDetailsObj.EngineerName = userId;
                                assetDetailsObj.ModifiedBy = userId;
                                assetDetailsObj.ModifiedDate = DateTime.Now;
                                assetServ.CreateOrUpdateITAssetDetails(assetDetailsObj);
                            }
                        }
                        if (assetDetails.TransactionType == "Scrap")
                        {
                            assetDetails.ITAssetScrapDetails.AssetDetails = assetDetailsObj;
                            assetDetails.ITAssetScrapDetails.InwardDate = DateTime.ParseExact(Request.Form["ITAssetScrapDetails.InwardDate"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            assetDetails.ITAssetScrapDetails.EngineerName = userId;
                            assetDetails.ITAssetScrapDetails.FromCampus = assetDetails.CurrentCampus;
                            assetDetails.ITAssetScrapDetails.FromBlock = assetDetails.CurrentBlock;
                            assetDetails.ITAssetScrapDetails.FromLocation = assetDetails.CurrentLocation;
                            assetDetails.ITAssetScrapDetails.AssetRefId = assetDetails.AssetRefId;
                            assetDetails.ITAssetScrapDetails.IsSubAsset = assetDetails.IsSubAsset;
                            history.FromCampus = assetDetails.CurrentCampus;
                            history.FromBlock = assetDetails.CurrentBlock;
                            history.FromLocation = assetDetails.CurrentLocation;
                            assetDetails.ITAssetScrapDetails.CreatedBy = userId;
                            assetDetails.ITAssetScrapDetails.CreatedDate = DateTime.Now;
                            assetServ.CreateOrUpdateITAssetScrapDetails(assetDetails.ITAssetScrapDetails);
                            history.TransactionType_Id = assetDetails.ITAssetScrapDetails.AssetScrap_Id;
                            if (assetDetails.ITAssetScrapDetails.AssetScrap_Id > 0)
                            {
                                assetDetailsObj.CurrentCampus = assetDetails.CurrentCampus;//changed by prabakaran
                                assetDetailsObj.CurrentLocation = "Scrap";
                                assetDetailsObj.CurrentBlock = "Scrap";
                                assetDetailsObj.InstalledOn = assetDetails.ITAssetScrapDetails.InwardDate;
                                assetDetailsObj.TransactionType = assetDetails.TransactionType;
                                assetDetailsObj.IsActive = false;
                                assetDetailsObj.IsStandBy = false;
                                assetDetailsObj.EngineerName = userId;
                                assetDetailsObj.ModifiedBy = userId;
                                assetDetailsObj.ModifiedDate = DateTime.Now;
                                assetServ.CreateOrUpdateITAssetDetails(assetDetailsObj);
                            }
                        }
                        history.AssetDetails = assetDetailsObj;
                        history.TransactionType = assetDetails.TransactionType;
                        history.ToBlock = assetDetailsObj.CurrentBlock;
                        history.ToLocation = assetDetailsObj.CurrentLocation;
                        history.ToCampus = assetDetailsObj.CurrentCampus;
                        history.CreatedBy = userId;
                        history.CreatedDate = DateTime.Now;
                        history.UserType = assetDetailsObj.UserType;
                        history.IdNum = assetDetailsObj.IdNum;
                        history.IsSubAsset = assetDetailsObj.IsSubAsset;
                        history.AssetRefId = assetDetailsObj.AssetRefId;
                        history.InvoiceDetailsId = assetDetailsObj.InvoiceDetailsId;
                        history.Warranty = assetDetailsObj.Warranty;
                        assetServ.CreateOrUpdateITAssetDetailsHistory(history);
                        return RedirectToAction("ITAssetTransaction", new { AssetDet_Id = assetDetails.AssetRefId });
                    }
                    //if (Request.Form["DocUpload"] == "Upload")
                    //{
                    //    UploadedFiles doc = assetServ.GetAssetDocumentsByAssetId(assetDetailsObj.AssetDet_Id, Request.Form["ddlDocumentType"].ToString());
                    //    if (doc == null)
                    //    {
                    //        string path = file1.InputStream.ToString();
                    //        byte[] imageSize = new byte[file1.ContentLength];
                    //        file1.InputStream.Read(imageSize, 0, (int)file1.ContentLength);
                    //        UploadedFiles fu1 = new UploadedFiles();
                    //        fu1.DocumentData = imageSize;
                    //        fu1.DocumentName = file1.FileName;
                    //        fu1.DocumentSize = file1.ContentLength.ToString();
                    //        fu1.UploadedDate = DateTime.Now.Date.ToString("dd/MM/yyyy");
                    //        fu1.DocumentFor = "ITAsset";
                    //        fu1.DocumentType = Request.Form["ddlDocumentType"].ToString();
                    //        fu1.PreRegNum = assetDetailsObj.AssetDet_Id;
                    //        fu1.Name = assetDetailsObj.AssetCode;
                    //        AdmissionManagementService ams = new AdmissionManagementService();
                    //        ams.CreateOrUpdateUploadedFiles(fu1);
                    //        return null;
                    //    }

                    //}
                    return RedirectToAction("ITAssetManagement");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpPost]
        public ActionResult ITAssetDocumentsUpload(long AssetDet_Id, string DocType, HttpPostedFileBase file1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MastersService ms = new MastersService();
                    AssetService assetServ = new AssetService();
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    AssetDetails assetDetailsObj = new AssetDetails();
                    if (AssetDet_Id > 0)
                        assetDetailsObj = assetServ.GetAssetDetailsByAssetId(AssetDet_Id);
                    UploadedFiles doc = assetServ.GetAssetDocumentsByAssetId(assetDetailsObj.AssetDet_Id, DocType);
                    if (doc != null && (DocType == "Warranty" || DocType == "PurchaseInvoice"))
                    {
                        StringBuilder retValue = new StringBuilder();
                        retValue.Append("Document is Already Exist for " + DocType);
                        return Json(new { success = false, result = retValue.ToString().Replace(Environment.NewLine, "<br />") }, "text/html", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        string path = file1.InputStream.ToString();
                        byte[] imageSize = new byte[file1.ContentLength];
                        file1.InputStream.Read(imageSize, 0, (int)file1.ContentLength);
                        UploadedFiles fu1 = new UploadedFiles();
                        fu1.DocumentData = imageSize;
                        fu1.DocumentName = file1.FileName;
                        fu1.DocumentSize = file1.ContentLength.ToString();
                        fu1.UploadedDate = DateTime.Now.Date.ToString("dd/MM/yyyy");
                        fu1.DocumentFor = "ITAsset";
                        fu1.DocumentType = DocType;
                        fu1.PreRegNum = assetDetailsObj.AssetDet_Id;
                        fu1.Name = assetDetailsObj.AssetCode;
                        AdmissionManagementService ams = new AdmissionManagementService();
                        ams.CreateOrUpdateUploadedFiles(fu1);
                        StringBuilder retValue = new StringBuilder();
                        retValue.Append("-------files uploaded successfully-----------");
                        return Json(new { success = true, result = retValue.ToString().Replace(Environment.NewLine, "<br />") }, "text/html", JsonRequestBehavior.AllowGet);
                    }
                    //return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        //public JsonResult Documentsjqgrid(long id, int rows, string sidx, string sord, int? page = 1)
        //{
        //    try
        //    {
        //        AdmissionManagementService ads = new AdmissionManagementService();
        //        Dictionary<string, object> criteria = new Dictionary<string, object>();
        //        criteria.Add("DocumentFor", "ITAsset");
        //        criteria.Add("PreRegNum", id);
        //        Dictionary<long, IList<UploadedFilesView>> UploadedFilesview = ads.GetUploadedFilesViewListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
        //        {
        //            long totalrecords = UploadedFilesview.First().Key;
        //            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
        //            var jsondat = new
        //            {
        //                total = totalPages,
        //                page = page,
        //                records = totalrecords,

        //                rows = (from items in UploadedFilesview.First().Value
        //                        select new
        //                        {
        //                            i = 2,
        //                            cell = new string[] {
        //                                items.Id.ToString(),
        //                       items.DocumentType,
        //                       String.Format(@"<a style='color:#034af3;text-decoration:underline' href= '#' onclick = 'uploaddat("+"\"" + items.Id + "\"" + ")' >{0}</a>",items.DocumentName),
        //                       items.DocumentSize+" Bytes",
        //                       items.UploadedDate,
        //                    }
        //                        })
        //            };
        //            return Json(jsondat, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
        //        throw ex;
        //    }
        //}
        public JsonResult Documentsjqgrid(long id, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                AdmissionManagementService ads = new AdmissionManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                //criteria.Add("DocumentFor", "ITAsset");
                criteria.Add("AssetDet_Id", id);
                criteria.Add("IsExpired", false);
                Dictionary<long, IList<AssetInvoiceDetails_vw>> assetinvoicedetails_vw = ass.GetAssetInvoiceDetails_vwListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (assetinvoicedetails_vw != null && assetinvoicedetails_vw.FirstOrDefault().Key > 0)
                {
                    foreach (var items in assetinvoicedetails_vw.FirstOrDefault().Value)
                    {
                        if (items.DocumentType == "PurchaseInvoice")
                        {
                            //DateTime EndTime = items.InvoiceDate.AddDays(Convert.ToInt64(items.Warranty) * 30);
                            DateTime EndTime = items.InvoiceDate.AddMonths(Convert.ToInt32(items.Warranty));
                            if (DateTime.Now > EndTime)
                            {
                                AssetDetails assetdtls = ass.GetAssetDetailsByAssetId(id);
                                assetdtls.IsExpired = true;
                                ass.CreateOrUpdateITAssetDetails(assetdtls);
                            }

                        }
                        if (items.DocumentType == "ServiceInvoice")
                        {

                            ITAssetServiceDetails svcdetails = ass.GetITAssetServiceDetailsByAssetIdandInvoiceId(items.AssetDet_Id, items.InvoiceDetailsId);
                            if (svcdetails != null)
                            {
                                DateTime EndTime = items.InvoiceDate.AddMonths(Convert.ToInt32(items.Warranty));
                                //DateTime EndTime = items.InvoiceDate.AddDays(Convert.ToInt64(items.Warranty) * 30);
                                if (DateTime.Now > EndTime)
                                {
                                    svcdetails.IsExpired = true;
                                    ass.CreateOrUpdateITAssetServiceDetails(svcdetails);
                                }
                            }

                        }
                    }
                }
                criteria.Remove("IsExpired");
                Dictionary<long, IList<AssetInvoiceDetails_vw>> assetinvoicedetails_vwlist = ass.GetAssetInvoiceDetails_vwListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (assetinvoicedetails_vwlist != null && assetinvoicedetails_vwlist.FirstOrDefault().Key > 0)
                {
                    long totalrecords = assetinvoicedetails_vwlist.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,

                        rows = (from items in assetinvoicedetails_vwlist.First().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[] {
                               items.InvoiceDetailsId.ToString(),
                               items.VendorMaster!=null?items.VendorMaster.VendorName:"",
                               items.InvoiceNo,
                               items.DocumentType,
                               items.InvoiceDetailsId >0?String.Format(@"<a style='color:#034af3;text-decoration:underline' href= '#' onclick = 'uploaddat("+"\"" + items.InvoiceDetailsId + "\"" + ")' >{0}</a>",items.DocumentName):"",
                               items.DocumentSize+" Bytes",
                               items.UploadedDate.ToString("dd'/'MM'/'yyyy"),
                               items.InvoiceDate.ToString("dd'/'MM'/'yyyy"), 
                               items.Amount.ToString(),
                               items.Warranty!=null?CalculateWarrantyAsYear(Convert.ToInt64(items.Warranty)): "0 Year 0 Month",
                               items.Warranty!=null?items.InvoiceDate.AddMonths(Convert.ToInt32(items.Warranty)).ToString("dd'/'MM'/'yyyy"):"No Warranty",
                               items.IsExpired == true?"Yes":"No",
                               items.Warranty!=null?CalculateWarrantyAge( items.InvoiceDate,items.Warranty):"0"
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
        public ActionResult ITAssetSpecificationMaster()
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

                throw ex;
            }
        }
        public ActionResult ITAssetSpecificationjqGrid(ITAssetSpecification spec, long? FormId, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    sord = sord == "desc" ? "Desc" : "Asc";
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Clear();
                    Dictionary<string, object> likecriteria = new Dictionary<string, object>();
                    likecriteria.Clear();
                    if (spec != null)
                    {
                        if (!string.IsNullOrEmpty(spec.Specification))
                            likecriteria.Add("Specification", spec.Specification);
                    }
                    Dictionary<long, IList<ITAssetSpecification>> specificationList = ass.GetITAssetSpecificationWithPagingAndExactAndLikeCriteria(page - 1, rows, sidx, sord, criteria, likecriteria);
                    if (specificationList != null && specificationList.Count > 0)
                    {
                        long totalrecords = specificationList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in specificationList.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[]{
                                       items.Spec_Id.ToString(),
                                       items.Specification,
                                       items.Description,
                                       items.CreatedBy!=null?us.GetUserNameByUserId(items.CreatedBy):"",
                                       items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy"):"",
                                       items.ModifiedBy,
                                       items.ModifiedDate!=null?items.ModifiedDate.Value.ToString("dd/MM/yyyy"):"",
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
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult AddITAssetSpecificationMaster(ITAssetSpecification spec)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (spec == null) return null;
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Clear();
                    if (!string.IsNullOrEmpty(spec.Description))
                        criteria.Add("Description", spec.Description);
                    Dictionary<long, IList<ITAssetSpecification>> specList = ass.GetITAssetSpecificationWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                    if (specList.FirstOrDefault().Key == 0)
                    {
                        spec.Specification = spec.Description.Replace(" ", string.Empty).ToUpper();
                        spec.CreatedBy = userId;
                        spec.CreatedDate = DateTime.Now;
                        ass.CreateOrUpdateITAssetSpecification(spec);
                        var Addscript = @"SucessMsg(""Added Successfully"");";
                        return JavaScript(Addscript);
                    }
                    else
                    {
                        var Addscript = @"ErrMsg(""Already Exist!!"");";
                        return JavaScript(Addscript);
                    }

                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult EditITAssetSpecificationMaster(ITAssetSpecification spec)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                {
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Clear();
                    if (spec == null) return null;
                    if (!string.IsNullOrEmpty(spec.Description))
                        criteria.Add("Description", spec.Description);
                    Dictionary<long, IList<ITAssetSpecification>> specList = ass.GetITAssetSpecificationWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                    ITAssetSpecification specObj = ass.GetITAssetSpecificationBySpec_Id(spec.Spec_Id);
                    if (specList != null && specList.First().Key == 0 && specObj.Spec_Id == spec.Spec_Id)
                    {
                        specObj.Specification = spec.Description.Replace("", string.Empty).ToUpper();
                        specObj.Description = spec.Description;
                        specObj.ModifiedBy = userId;
                        specObj.ModifiedDate = DateTime.Now;

                        ass.CreateOrUpdateITAssetSpecification(specObj);
                        var Addscript = @"SucessMsg(""Added Successfully"");";
                        return JavaScript(Addscript);
                    }
                    else
                    {
                        var Addscript = @"ErrMsg(""Already Exist!!"");";
                        return JavaScript(Addscript);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult DeleteITAssetSpecificationMaster(string Id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    string[] arrayId = Id.Split(',');
                    long[] specId = Array.ConvertAll(arrayId, Int64.Parse);
                    ass.DeleteITAssetSpecification(specId);
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
        #region AssetLocationMaster by Prabakaran
        public ActionResult AssetLocationMaster()
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
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult AssetLocationMasterJqGrid(string Campus, string Block, string Location, string IsActive, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (!string.IsNullOrEmpty(Campus))
                {
                    criteria.Add("Campus", Campus);
                }
                if (!string.IsNullOrEmpty(Block))
                {
                    criteria.Add("BlockName", Block);
                }
                if (!string.IsNullOrEmpty(Location))
                {
                    criteria.Add("Location", Location);
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
                Dictionary<long, IList<AssetLocationMaster>> AssetLocationMasterList = ass.GetAssetLocationMasterWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (AssetLocationMasterList != null && AssetLocationMasterList.Count > 0)
                {
                    long totalrecords = AssetLocationMasterList.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,
                        rows = (from items in AssetLocationMasterList.First().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[]{
                                       items.LocationId.ToString(),
                                       //items.CampusMaster.Name,
                                       items.LocationCode,
                                       items.Campus,
                                       items.BlockName,
                                       items.Location,
                                       items.IsActive==true?"Yes":"No",
                                       items.CreatedBy!=null?us.GetUserNameByUserId(items.CreatedBy):"",
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
        public ActionResult AddAssetLocationMaster(AssetLocationMaster alm)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    AssetLocationMaster almdtls = ass.GetAssetLocationMasterByLocationName(alm.BlockName, alm.Campus, alm.Location);
                    if (almdtls != null)
                    {
                        //if (string.Equals(cbm.BlockName,cbmdetails.BlockName,StringComparison.CurrentCultureIgnoreCase))
                        //{
                        var script = @"ErrMsg(""Already Exist"");";
                        return JavaScript(script);
                        //}
                    }
                    alm.CreatedBy = userId;
                    alm.CreatedDate = DateTime.Now;
                    alm.IsActive = true;
                    ass.CreateOrUpdateAssetLocationMaster(alm);
                    alm.LocationCode = "LOC-" + alm.LocationId;
                    ass.CreateOrUpdateAssetLocationMaster(alm);
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult EditAssetLocationMaster(AssetLocationMaster alm)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (alm.LocationId > 0)
                    {

                        AssetLocationMaster almdtls = ass.GetAssetLocationMasterByLocationName(alm.BlockName, alm.Campus, alm.Location);
                        if (almdtls != null)
                        {
                            var script = @"ErrMsg(""Already Exist"");";
                            return JavaScript(script);
                        }
                        AssetLocationMaster almdetails = ass.GetAssetLocationMasterByLocationId(alm.LocationId);
                        if (almdetails != null)
                        {
                            almdetails.ModifiedBy = userId;
                            almdetails.ModifiedDate = DateTime.Now;
                            almdetails.IsActive = true;
                            almdetails.Campus = alm.Campus;
                            almdetails.BlockName = alm.BlockName;
                            almdetails.Location = alm.Location;
                            ass.CreateOrUpdateAssetLocationMaster(almdetails);
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
        public ActionResult DeleteAssetLocationMaster(string Id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    string[] arrayId = Id.Split(',');
                    long[] BlockId = Array.ConvertAll(arrayId, Int64.Parse);
                    ass.DeleteAssetLocationMaster(BlockId);
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
        #region CampusBlockMaster by Prabakaran
        public ActionResult CampusBlockMaster()
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

                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult CampusBlockMasterJqGrid(string Campus, string BlockName, string IsActive, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (!string.IsNullOrEmpty(Campus))
                { criteria.Add("Campus", Campus); }
                if (!string.IsNullOrEmpty(BlockName))
                { criteria.Add("BlockName", BlockName); }
                if (!string.IsNullOrEmpty(IsActive))
                {
                    if (IsActive == "true" || IsActive == "True")
                        criteria.Add("IsActive", true);
                    if (IsActive == "false" || IsActive == "False")
                        criteria.Add("IsActive", false);
                }
                Dictionary<long, IList<CampusBlockMaster>> CampusBlockMasterList = ass.GetCampusBlockMasterWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (CampusBlockMasterList != null && CampusBlockMasterList.Count > 0)
                {
                    long totalrecords = CampusBlockMasterList.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,
                        rows = (from items in CampusBlockMasterList.First().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[]{
                                       items.BlockId.ToString(),                                       
                                       items.BlockCode,
                                       items.Campus,
                                       items.BlockName,                                       
                                       items.IsActive==true?"Yes":"No",
                                       items.CreatedBy!=null?us.GetUserNameByUserId(items.CreatedBy):"",
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
        public ActionResult AddCampusBlockMaster(CampusBlockMaster cbm)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    CampusBlockMaster cbmdetails = ass.GetCampusBlockMasterByBlockName(cbm.BlockName, cbm.Campus);
                    if (cbmdetails != null)
                    {
                        //if (string.Equals(cbm.BlockName,cbmdetails.BlockName,StringComparison.CurrentCultureIgnoreCase))
                        //{
                        var script = @"ErrMsg(""Already Exist"");";
                        return JavaScript(script);
                        //}
                    }
                    cbm.CreatedBy = userId;
                    cbm.CreatedDate = DateTime.Now;
                    cbm.IsActive = true;
                    ass.CreateOrUpdateCampusBlockMaster(cbm);
                    cbm.BlockCode = "BLK-" + cbm.BlockId;
                    ass.CreateOrUpdateCampusBlockMaster(cbm);
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult EditCampusBlockMaster(CampusBlockMaster cbm)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (cbm.BlockId > 0)
                    {

                        CampusBlockMaster cbmdtls = ass.GetCampusBlockMasterByBlockName(cbm.BlockName, cbm.Campus);
                        if (cbmdtls != null)
                        {
                            var script = @"ErrMsg(""Already Exist"");";
                            return JavaScript(script);
                        }
                        CampusBlockMaster cbmdetails = ass.GetCampusBlockMasterByBlockId(cbm.BlockId);
                        if (cbmdetails != null)
                        {
                            cbmdetails.ModifiedBy = userId;
                            cbmdetails.ModifiedDate = DateTime.Now;
                            cbmdetails.IsActive = true;
                            cbmdetails.Campus = cbm.Campus;
                            cbmdetails.BlockName = cbm.BlockName;
                            ass.CreateOrUpdateCampusBlockMaster(cbmdetails);
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
        public ActionResult DeleteCampusBlockMaster(string Id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    string[] arrayId = Id.Split(',');
                    long[] BlockId = Array.ConvertAll(arrayId, Int64.Parse);
                    ass.DeleteCampusBlockMaster(BlockId);
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
        public JsonResult GetBlockByCampus(string Campus)
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(Campus))
            { criteria.Add("Campus", Campus); }
            Dictionary<long, IList<CampusBlockMaster>> cbm = ass.GetCampusBlockMasterWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            if (cbm != null && cbm.First().Value != null && cbm.First().Value.Count > 0)
            {
                var BlockList = (
                         from items in cbm.First().Value
                         select new
                         {
                             Text = items.BlockName,
                             Value = items.BlockName
                         }).Distinct().ToList();
                return Json(BlockList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetLocationByCampusWithBlock(string Campus, string Block)
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(Campus))
            { criteria.Add("Campus", Campus); }
            if (!string.IsNullOrEmpty(Block))
            { criteria.Add("BlockName", Block); }
            Dictionary<long, IList<AssetLocationMaster>> alm = ass.GetAssetLocationMasterWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            if (alm != null && alm.First().Value != null && alm.First().Value.Count > 0)
            {
                var BlockList = (
                         from items in alm.First().Value
                         select new
                         {
                             Text = items.Location,
                             Value = items.Location
                         }).Distinct().ToList();
                return Json(BlockList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region VendorMaster by Prabakaran
        public ActionResult VendorMaster()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrEmpty(userId)) return RedirectToAction("LogOff", "Account");
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

                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult VendorMasterJqGrid(string VendorType, string VendorName, string IsActive, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (!string.IsNullOrEmpty(VendorType))
                { criteria.Add("VendorType", VendorType); }
                if (!string.IsNullOrEmpty(VendorName))
                { criteria.Add("VendorName", VendorName); }
                if (!string.IsNullOrEmpty(IsActive))
                {
                    if (IsActive == "true" || IsActive == "True")
                        criteria.Add("IsActive", true);
                    if (IsActive == "false" || IsActive == "False")
                        criteria.Add("IsActive", false);
                }
                Dictionary<long, IList<VendorMaster>> VendorMasterList = ass.GetVendorMasterWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (VendorMasterList != null && VendorMasterList.Count > 0)
                {
                    long totalrecords = VendorMasterList.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,
                        rows = (from items in VendorMasterList.First().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[]{
                                       items.VendorId.ToString(),                                       
                                       items.VendorCode,
                                       items.VendorName,
                                       items.VendorType,                                       
                                       items.IsActive==true?"Yes":"No",
                                       items.CreatedBy!=null?us.GetUserNameByUserId(items.CreatedBy):"",
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
        public ActionResult AddVendorMaster(VendorMaster vm)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    VendorMaster vmdetails = ass.GetVendorMasterByVendorName(vm.VendorName);
                    if (vmdetails != null)
                    {
                        if (string.Equals(vm.VendorType, vmdetails.VendorType, StringComparison.CurrentCultureIgnoreCase))
                        {
                            var script = @"ErrMsg(""Already Exist"");";
                            return JavaScript(script);
                        }
                    }
                    vm.CreatedBy = userId;
                    vm.CreatedDate = DateTime.Now;
                    vm.IsActive = true;
                    ass.CreateOrUpdateVendorMaster(vm);
                    vm.VendorCode = "VDR-" + vm.VendorId;
                    ass.CreateOrUpdateVendorMaster(vm);
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult EditVendorMaster(VendorMaster vm)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (vm.VendorId > 0)
                    {

                        VendorMaster vmdtls = ass.GetVendorMasterByVendorName(vm.VendorName);
                        if (vmdtls != null)
                        {
                            if (string.Equals(vm.VendorType, vmdtls.VendorType, StringComparison.CurrentCultureIgnoreCase))
                            {
                                var script = @"ErrMsg(""Already Exist"");";
                                return JavaScript(script);
                            }
                        }
                        VendorMaster vmdetails = ass.GetVendorMasterByVendorId(vm.VendorId);
                        if (vmdetails != null)
                        {
                            vmdetails.ModifiedBy = userId;
                            vmdetails.ModifiedDate = DateTime.Now;
                            vmdetails.IsActive = true;
                            //cbmdetails.Campus = vm.Campus;
                            vmdetails.VendorName = vm.VendorName;
                            vmdetails.VendorType = vm.VendorType;
                            ass.CreateOrUpdateVendorMaster(vmdetails);
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
        public ActionResult DeleteVendorMaster(string Id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    string[] arrayId = Id.Split(',');
                    long[] VendorId = Array.ConvertAll(arrayId, Int64.Parse);
                    ass.DeleteVendorMaster(VendorId);
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
        #region GetVendorName
        public JsonResult GetVendorNameByVendorType(string VendorType)
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(VendorType))
            {
                string[] VendorTypes = VendorType.Split(',');
                criteria.Add("VendorType", VendorTypes);
            }
            Dictionary<long, IList<VendorMaster>> vm = ass.GetVendorMasterWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
            if (vm != null && vm.First().Value != null && vm.First().Value.Count > 0)
            {
                var VendorNameList = (
                         from items in vm.First().Value
                         select new
                         {
                             Text = items.VendorName,
                             Value = items.VendorName
                         }).Distinct().ToList();
                return Json(VendorNameList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetVendorNameWithIdByVendorType(string VendorType)
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(VendorType))
            {
                string[] VendorTypes = new string[2];
                VendorTypes[0] = "Both";
                VendorTypes[1] = VendorType;
                criteria.Add("VendorType", VendorTypes);
            }
            Dictionary<long, IList<VendorMaster>> vm = ass.GetVendorMasterWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
            if (vm != null && vm.First().Value != null && vm.First().Value.Count > 0)
            {
                var VendorNameList = (
                         from items in vm.First().Value
                         select new
                         {
                             Text = items.VendorName,
                             Value = items.VendorId
                         }).Distinct().ToList();
                return Json(VendorNameList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region Service Return Page
        public ActionResult ITAssetService()
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
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult ITAssetServicejqGrid(AssetDetails assetdetails, long? FormId, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    sord = sord == "desc" ? "Desc" : "Asc";
                    if (sidx == "FormId") sidx = "CampusMaster.FormId";
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<string, object> likecriteria = new Dictionary<string, object>();
                    criteria.Clear();
                    if (assetdetails != null)
                    {
                        if (!string.IsNullOrEmpty(assetdetails.AssetCode))
                            likecriteria.Add("AssetCode", assetdetails.AssetCode);
                        //if (!string.IsNullOrEmpty(assetdetails.AssetType))
                        //    likecriteria.Add("AssetType", assetdetails.AssetType);
                        if (!string.IsNullOrEmpty(assetdetails.Make))
                            likecriteria.Add("Make", assetdetails.Make);
                        if (!string.IsNullOrEmpty(assetdetails.Model))
                            likecriteria.Add("Model", assetdetails.Model);
                        if (!string.IsNullOrEmpty(assetdetails.SerialNo))
                            likecriteria.Add("SerialNo", assetdetails.SerialNo);
                        if (!string.IsNullOrEmpty(assetdetails.CurrentBlock))
                            criteria.Add("CurrentBlock", assetdetails.CurrentBlock);
                        if (!string.IsNullOrEmpty(assetdetails.CurrentLocation))
                            criteria.Add("CurrentLocation", assetdetails.CurrentLocation);
                        if (!string.IsNullOrEmpty(assetdetails.UserType))
                            criteria.Add("UserType", assetdetails.UserType);
                    }
                    if (FormId > 0)
                    {
                        likecriteria.Add("CampusMaster.FormId", FormId);
                    }
                    if (!string.IsNullOrEmpty(assetdetails.AssetType))
                        criteria.Add("Asset_Id", Convert.ToInt64(assetdetails.AssetType));
                    if (criteria.Count == 0)
                        criteria.Add("IsActive", true);
                    criteria.Add("TransactionType", "Service");
                    criteria.Add("IsSubAsset", false);
                    Dictionary<long, IList<AssetDetails>> AssetDetailsList = ass.GetITAssetDetailsWithPagingAndExactAndLikeCriteria(page - 1, rows, sidx, sord, criteria, likecriteria);
                    if (AssetDetailsList != null && AssetDetailsList.Count > 0)
                    {
                        StaffManagementService sms = new StaffManagementService();
                        AdmissionManagementService abc = new AdmissionManagementService();
                        long totalrecords = AssetDetailsList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in AssetDetailsList.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[]{
                                       items.AssetDet_Id.ToString(),
                                       items.AssetCode,
                                       //items.AssetCode!=null?"<a href='/Asset/ITAssetTransaction?AssetDet_Id=" + items.AssetDet_Id +"'>" + items.AssetCode + "</a>":"",
                                       items.Asset_Id.ToString(),
                                       items.AssetType,
                                       items.Make,
                                       items.Model,
                                       items.SerialNo,
                                       //items.Location,
                                       //items.CampusMaster!=null?items.CampusMaster.Name:"",
                                       items.CurrentCampus,
                                       items.CurrentBlock,
                                       items.CurrentLocation,
                                       items.UserType,                                       
                                       //items.UserType.Contains("Staff")?sms.GetStaffNameByPreRegNum(items.PreRegNum):items.UserType.Contains("Student")?abc.GetStudentNameByPreRegNum(items.PreRegNum):items.UserType.Contains("Common")?"":"",                                       
                                       items.UserType.Contains("Staff")?items.StaffDetailsView.Name:items.UserType.Contains("Student")?items.StudentTemplateView.Name:items.UserType.Contains("Common")?"":"",                                       
                                       items.CreatedBy,
                                       items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy"):"",
                                       items.ModifiedBy,
                                       items.ModifiedDate!=null?items.ModifiedDate.Value.ToString("dd/MM/yyyy"):"",
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
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult ITSubAssetServicejqGrid(AssetDetails assetdetails, long? FormId, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    sord = sord == "desc" ? "Desc" : "Asc";
                    if (sidx == "FormId") sidx = "CampusMaster.FormId";
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<string, object> likecriteria = new Dictionary<string, object>();
                    criteria.Clear();
                    if (assetdetails != null)
                    {
                        if (!string.IsNullOrEmpty(assetdetails.AssetCode))
                            likecriteria.Add("AssetCode", assetdetails.AssetCode);
                        //if (!string.IsNullOrEmpty(assetdetails.AssetType))
                        //    likecriteria.Add("AssetType", assetdetails.AssetType);
                        if (!string.IsNullOrEmpty(assetdetails.Make))
                            likecriteria.Add("Make", assetdetails.Make);
                        if (!string.IsNullOrEmpty(assetdetails.Model))
                            likecriteria.Add("Model", assetdetails.Model);
                        if (!string.IsNullOrEmpty(assetdetails.SerialNo))
                            likecriteria.Add("SerialNo", assetdetails.SerialNo);
                        if (!string.IsNullOrEmpty(assetdetails.CurrentBlock))
                            criteria.Add("CurrentBlock", assetdetails.CurrentBlock);
                        if (!string.IsNullOrEmpty(assetdetails.CurrentLocation))
                            criteria.Add("CurrentLocation", assetdetails.CurrentLocation);
                        if (!string.IsNullOrEmpty(assetdetails.UserType))
                            criteria.Add("UserType", assetdetails.UserType);
                    }
                    if (FormId > 0)
                    {
                        likecriteria.Add("CampusMaster.FormId", FormId);
                    }
                    if (!string.IsNullOrEmpty(assetdetails.AssetType))
                        criteria.Add("Asset_Id", Convert.ToInt64(assetdetails.AssetType));
                    if (criteria.Count == 0)
                        criteria.Add("IsActive", true);
                    criteria.Add("TransactionType", "Service");
                    criteria.Add("IsSubAsset", true);
                    Dictionary<long, IList<SubAssetDetails_vw>> AssetDetailsList = ass.GetITSubAssetDetails_vwWithPagingAndExactAndLikeCriteria(page - 1, rows, sidx, sord, criteria, likecriteria);
                    if (AssetDetailsList != null && AssetDetailsList.Count > 0)
                    {
                        StaffManagementService sms = new StaffManagementService();
                        AdmissionManagementService abc = new AdmissionManagementService();
                        long totalrecords = AssetDetailsList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in AssetDetailsList.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[]{
                                       items.AssetDet_Id.ToString(),
                                       items.AssetCode,
                                       //items.AssetCode!=null?"<a href='/Asset/ITAssetTransaction?AssetDet_Id=" + items.AssetDet_Id +"'>" + items.AssetCode + "</a>":"",
                                       items.Asset_Id.ToString(),
                                       items.AssetType,
                                       items.Make,
                                       items.Model,
                                       items.SerialNo,
                                       //items.Location,
                                       //items.CampusMaster!=null?items.CampusMaster.Name:"",
                                       items.CurrentCampus,
                                       items.CurrentBlock,
                                       items.CurrentLocation,
                                       items.UserType,                                       
                                       //items.UserType.Contains("Staff")?sms.GetStaffNameByPreRegNum(items.PreRegNum):items.UserType.Contains("Student")?abc.GetStudentNameByPreRegNum(items.PreRegNum):items.UserType.Contains("Common")?"":"",                                       
                                       items.UserType.Contains("Staff")?items.StaffDetailsView.Name:items.UserType.Contains("Student")?items.StudentTemplateView.Name:items.UserType.Contains("Common")?"":"",                                       
                                       items.CreatedBy,
                                       items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy"):"",
                                       items.ModifiedBy,
                                       items.ModifiedDate!=null?items.ModifiedDate.Value.ToString("dd/MM/yyyy"):"",
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
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult ITAssetServiceReturn(long AssetId)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    var usrcmp = Session["UserCampus"] as IEnumerable<string>;
                    if (usrcmp.First() != null)            // to check if the usrcmp obj is null or with data
                    {
                        criteria.Add("Name", usrcmp);
                    }
                    Dictionary<long, IList<CampusMaster>> CampusMaster = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                    criteria.Clear();
                    criteria.Add("AssetDetails.AssetDet_Id", AssetId);
                    Dictionary<long, IList<ITAssetServiceDetails>> serviceDetails = ass.GetITAssetServiceDetailsWithPagingAndCriteria(0, 99999, "AssetService_Id", "desc", criteria);
                    ITAssetDetailsTransactionHistory transaction = ass.GetITAssetDetailsTransactionHistoryByTransactionType_IdwithAssetId(AssetId, serviceDetails.First().Value[0].AssetService_Id);
                    AssetDetails assetDetailsObj = ass.GetAssetDetailsByAssetId(AssetId);
                    AssetDetailsTemplate assetdtls = ass.GetAssetDetailsTemplateByAssetId(assetDetailsObj.Asset_Id);
                    if (serviceDetails != null && serviceDetails.FirstOrDefault().Key > 0)
                    {
                        var DCDate = (from u in serviceDetails.FirstOrDefault().Value where u.InwardDate == null select u.DCDate).ToArray();
                        if (DCDate.Length > 0)
                        {
                            ViewBag.DCDate = DCDate[0].Value.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            ViewBag.DCDate = "";
                        }
                    }
                    else
                    {
                        ViewBag.DCDate = "";
                    }
                    List<string> specList = assetdtls.Specifications.Split(',').ToList();
                    List<string> DescList = assetdtls.SpecificationsDetails.Split(',').ToList();
                    //assetdtls.specList = specList;
                    ViewBag.DescList = DescList;
                    ViewBag.specList = specList;
                    ViewBag.campusddl = CampusMaster.First().Value;
                    assetDetailsObj.AssetHistory = transaction;
                    return View(assetDetailsObj);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public ActionResult EditAssetandServiceDetails(AssetDetails assetdetails, DateTime InwardDate, long PendingAge, long InvoiceDetailsId)
        {
            try
            {
                string userId = base.ValidateUser();
                bool Flag = false;
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (assetdetails != null)
                    {
                        AssetDetails AssetDetailsObj = ass.GetAssetDetailsByAssetId(assetdetails.AssetDet_Id);
                        if (AssetDetailsObj != null)
                        {
                            //ITAssetServiceDetails serviceDetails = ass.GetITAssetServiceDetailsByAssetId(AssetDetailsObj.AssetDet_Id);
                            Dictionary<string, object> criteria = new Dictionary<string, object>();
                            criteria.Add("AssetDetails.AssetDet_Id", AssetDetailsObj.AssetDet_Id);
                            Dictionary<long, IList<ITAssetServiceDetails>> serviceDetails = ass.GetITAssetServiceDetailsWithPagingAndCriteria(0, 99999, "AssetService_Id", "desc", criteria);
                            ITAssetDetailsTransactionHistory history = new ITAssetDetailsTransactionHistory();
                            if (serviceDetails.FirstOrDefault().Key > 0 && serviceDetails != null)
                            {
                                ITAssetServiceDetails svcdetails = ass.GetITAssetServiceDetailsByAssetService_Id(serviceDetails.First().Value[0].AssetService_Id);
                                svcdetails.InwardDate = InwardDate;
                                svcdetails.FromCampus = assetdetails.CurrentCampus;
                                svcdetails.FromBlock = assetdetails.CurrentBlock;
                                svcdetails.FromLocation = assetdetails.CurrentLocation;
                                svcdetails.PendingAge = PendingAge;
                                svcdetails.InvoiceDetailsId = InvoiceDetailsId;
                                svcdetails.ModifiedBy = userId;
                                svcdetails.ModifiedDate = DateTime.Now;
                                svcdetails.InvoiceDetailsId = InvoiceDetailsId;
                                svcdetails.Warranty = assetdetails.Warranty;
                                svcdetails.Amount = assetdetails.Amount;
                                if (assetdetails.UserType == "Staff" || assetdetails.UserType == "Common" || assetdetails.UserType == "Not Applicable")
                                {
                                    svcdetails.AssetRefId = assetdetails.AssetRefId;
                                }
                                ass.CreateOrUpdateITAssetServiceDetails(svcdetails);
                                AssetInvoiceDetails assetinvoicedetails = ass.GetAssetInvoiceDetailsByInvoiceDetailsId(InvoiceDetailsId);
                                assetinvoicedetails.AssetCount = assetinvoicedetails.AssetCount + 1;
                                if (assetinvoicedetails.TotalAsset == assetinvoicedetails.AssetCount)
                                {
                                    assetinvoicedetails.IsActive = false;
                                }
                                ass.CreateOrUpdateAssetInvoiceDetails(assetinvoicedetails);
                                history.TransactionType_Id = svcdetails.AssetService_Id;
                                history.FromCampus = AssetDetailsObj.CurrentCampus;
                                history.FromLocation = AssetDetailsObj.CurrentLocation;
                                history.FromBlock = AssetDetailsObj.CurrentBlock;
                                history.InvoiceDetailsId = svcdetails.InvoiceDetailsId;
                                history.Warranty = svcdetails.Warranty;
                                history.IsSubAsset = svcdetails.IsSubAsset;
                                history.AssetRefId = svcdetails.AssetRefId;
                            }
                            AssetDetailsObj.CurrentCampus = assetdetails.CurrentCampus;
                            AssetDetailsObj.CurrentBlock = assetdetails.CurrentBlock;
                            AssetDetailsObj.CurrentLocation = assetdetails.CurrentLocation;
                            if (assetdetails.UserType == "Not Applicable")
                            {
                                AssetDetailsObj.TransactionType = "Stock";
                            }
                            else
                            {
                                AssetDetailsObj.TransactionType = "Inward";
                            }
                            if (assetdetails.IsSubAsset == false)
                            {
                                AssetDetailsObj.IdNum = assetdetails.IdNum;
                                AssetDetailsObj.UserType = assetdetails.UserType;
                            }
                            AssetDetailsObj.ModifiedBy = userId;
                            AssetDetailsObj.ModifiedDate = DateTime.Now;
                            if (assetdetails.UserType == "Staff" || assetdetails.UserType == "Common" || assetdetails.UserType == "Not Applicable")
                            {
                                AssetDetailsObj.AssetRefId = assetdetails.AssetRefId;
                            }
                            ass.CreateOrUpdateITAssetDetails(AssetDetailsObj);
                            history.AssetDetails = AssetDetailsObj;
                            history.TransactionType = AssetDetailsObj.TransactionType;
                            //history.FromCampus = AssetDetailsObj.FromCampus;
                            //history.FromLocation = AssetDetailsObj.Location;
                            //history.FromBlock = AssetDetailsObj.FromBlock;                            
                            history.ToBlock = AssetDetailsObj.CurrentBlock;
                            history.ToLocation = AssetDetailsObj.CurrentLocation;
                            history.ToCampus = AssetDetailsObj.CurrentCampus;
                            history.CreatedBy = userId;
                            history.CreatedDate = DateTime.Now;
                            history.UserType = assetdetails.UserType;
                            history.IdNum = assetdetails.IdNum;
                            history.Amount = assetdetails.Amount;
                            history.AssetRefId = AssetDetailsObj.AssetRefId;
                            history.IsSubAsset = assetdetails.IsSubAsset;
                            ass.CreateOrUpdateITAssetDetailsHistory(history);
                            Flag = true;
                            return Json(Flag, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                return Json(Flag, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion
        //public ActionResult uploaddisplay(long Id, string DocumentFor)
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //        else
        //        {
        //            AdmissionManagementService ads = new AdmissionManagementService();
        //            Dictionary<string, object> criteria = new Dictionary<string, object>();
        //            criteria.Add("Id", Id);
        //            criteria.Add("DocumentFor", DocumentFor);
        //            Dictionary<long, IList<UploadedFiles>> UploadedFiles = ads.GetUploadedFilesListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteria);
        //            if (UploadedFiles != null && UploadedFiles.FirstOrDefault().Value != null)
        //            {
        //                IList<UploadedFiles> list = UploadedFiles.FirstOrDefault().Value;
        //                UploadedFiles doc = list.FirstOrDefault();
        //                if (doc.DocumentData != null)
        //                {
        //                    int startIndx = Convert.ToInt32(doc.DocumentName.LastIndexOf(".").ToString());
        //                    int FileLength = Convert.ToInt32(doc.DocumentName.Length);
        //                    string fileExtn = doc.DocumentName.Substring(startIndx, (FileLength - startIndx));
        //                    return File(doc.DocumentData, GetContentTypeByFileExtension(fileExtn), doc.DocumentName);
        //                }
        //                else
        //                {
        //                    var dir = Server.MapPath("/Images");
        //                    string ImagePath = ConfigurationManager.AppSettings["ImageFilePath"];
        //                    return File(ImagePath, "image/jpg");
        //                }
        //            }
        //            else
        //            {
        //                return null;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
        //        throw ex;
        //    }
        //}
        //public ActionResult AddNewBulkITAsset(long AssetId)
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //        else
        //        {
        //            AssetDetailsTemplate assetDetailsObj = ass.GetAssetDetailsTemplateByAssetId(AssetId);
        //            List<string> specList = assetDetailsObj.Specifications.Split(',').ToList();
        //            List<string> DescList = assetDetailsObj.SpecificationsDetails.Split(',').ToList();
        //            assetDetailsObj.specList = specList;
        //            ViewBag.DescList = DescList;
        //            ViewBag.specList = specList;
        //            return View(assetDetailsObj);
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}
        public ActionResult uploaddisplay(long Id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    //AdmissionManagementService ads = new AdmissionManagementService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<string, object> likecriteria = new Dictionary<string, object>();
                    criteria.Add("InvoiceDetailsId", Id);
                    //criteria.Add("DocumentFor", DocumentFor);
                    Dictionary<long, IList<AssetInvoiceDetails>> AssetInvoiceDetails = ass.GetAssetInvoiceDetailsWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteria, likecriteria);
                    if (AssetInvoiceDetails != null && AssetInvoiceDetails.FirstOrDefault().Value != null)
                    {
                        IList<AssetInvoiceDetails> list = AssetInvoiceDetails.FirstOrDefault().Value;
                        AssetInvoiceDetails doc = list.FirstOrDefault();
                        if (doc.DocumentData != null)
                        {
                            int startIndx = Convert.ToInt32(doc.DocumentName.LastIndexOf(".").ToString());
                            int FileLength = Convert.ToInt32(doc.DocumentName.Length);
                            string fileExtn = doc.DocumentName.Substring(startIndx, (FileLength - startIndx));
                            return File(doc.DocumentData, GetContentTypeByFileExtension(fileExtn), doc.DocumentName);
                        }
                        else
                        {
                            var dir = Server.MapPath("/Images");
                            string ImagePath = ConfigurationManager.AppSettings["ImageFilePath"];
                            return File(ImagePath, "image/jpg");
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
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }
        #region AssetBrandMaster
        public ActionResult AssetBrandMaster()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrEmpty(userId)) return RedirectToAction("LogOff", "Account");
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
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }
        public ActionResult AssetBrandMasterJqGrid(string Brand, string IsActive, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (!string.IsNullOrEmpty(Brand))
                { criteria.Add("Brand", Brand); }
                if (!string.IsNullOrEmpty(IsActive))
                {
                    if (IsActive == "true" || IsActive == "True")
                        criteria.Add("IsActive", true);
                    if (IsActive == "false" || IsActive == "False")
                        criteria.Add("IsActive", false);
                }
                Dictionary<long, IList<AssetBrandMaster>> AssetBrandMasterList = ass.GetAssetBrandMasterWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (AssetBrandMasterList != null && AssetBrandMasterList.Count > 0)
                {
                    long totalrecords = AssetBrandMasterList.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,
                        rows = (from items in AssetBrandMasterList.First().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[]{
                                       items.BrandMasterId.ToString(),                                                                                                                     
                                       items.Brand,                                       
                                       items.IsActive==true?"Yes":"No",
                                       items.CreatedBy!=null?us.GetUserNameByUserId(items.CreatedBy):"",
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
        public ActionResult AddAssetBrandMaster(AssetBrandMaster abm)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    AssetBrandMaster abmdetails = ass.GetAssetBrandMasterByBrandName(abm.Brand.ToUpper());
                    if (abmdetails != null)
                    {
                        var script = @"ErrMsg(""Already Exist"");";
                        return JavaScript(script);
                    }
                    abm.Brand = abm.Brand.ToUpper();
                    abm.CreatedBy = userId;
                    abm.CreatedDate = DateTime.Now;
                    abm.IsActive = true;
                    ass.CreateOrUpdateAssetBrandMaster(abm);
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult EditAssetBrandMaster(AssetBrandMaster abm)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (abm.BrandMasterId > 0)
                    {
                        AssetBrandMaster abmdtls = ass.GetAssetBrandMasterByBrandName(abm.Brand.ToUpper());
                        if (abmdtls != null)
                        {
                            var script = @"ErrMsg(""Already Exist"");";
                            return JavaScript(script);
                        }
                        AssetBrandMaster abmdetails = ass.GetAssetBrandMasterByBrandMasterId(abm.BrandMasterId);
                        if (abmdetails != null)
                        {
                            abmdetails.ModifiedBy = userId;
                            abmdetails.ModifiedDate = DateTime.Now;
                            abmdetails.Brand = abm.Brand.ToUpper();
                            ass.CreateOrUpdateAssetBrandMaster(abmdetails);
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
        public ActionResult DeleteAssetBrandMaster(string Id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    string[] arrayId = Id.Split(',');
                    long[] BrandMasterId = Array.ConvertAll(arrayId, Int64.Parse);
                    ass.DeleteAssetBrandMaster(BrandMasterId);
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
        public ActionResult GetBrandName()
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("IsActive", true);
            Dictionary<long, IList<AssetBrandMaster>> assetBrandMaster = ass.GetAssetBrandMasterWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
            if (assetBrandMaster != null && assetBrandMaster.First().Value != null && assetBrandMaster.FirstOrDefault().Key > 0)
            {
                var AssetBrandList = (
                         from items in assetBrandMaster.First().Value
                         where items.Brand != null
                         select new
                         {
                             Text = items.Brand,
                             Value = items.Brand
                         }).Distinct().ToList().OrderBy(x => x.Text);
                return Json(AssetBrandList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetBrandNameddl()
        {
            try
            {

                Dictionary<string, string> Brand = new Dictionary<string, string>();

                Dictionary<string, object> criteria = new Dictionary<string, object>();

                Dictionary<long, IList<AssetBrandMaster>> AssetBrandMstr = ass.GetAssetBrandMasterWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);

                foreach (AssetBrandMaster brand in AssetBrandMstr.First().Value)
                {
                    Brand.Add(brand.Brand, brand.Brand);
                }
                return PartialView("Dropdown", Brand);
            }
            catch (Exception ex)
            {
                // ExceptionPolicy.HandleException(ex, "Policy");
                throw ex;
            }
        }
        #endregion
        #region AssetModelMaster
        public ActionResult AssetModelMaster()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrEmpty(userId)) return RedirectToAction("LogOff", "Account");
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
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }
        public ActionResult AssetModelMasterJqGrid(string Brand, string Model, string IsActive, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (!string.IsNullOrEmpty(Brand))
                { criteria.Add("Brand", Brand); }
                if (!string.IsNullOrEmpty(Model))
                { criteria.Add("Model", Model); }
                if (!string.IsNullOrEmpty(IsActive))
                {
                    if (IsActive == "true" || IsActive == "True")
                        criteria.Add("IsActive", true);
                    if (IsActive == "false" || IsActive == "False")
                        criteria.Add("IsActive", false);
                }
                Dictionary<long, IList<AssetModelMaster>> AssetModelMasterList = ass.GetAssetModelMasterWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (AssetModelMasterList != null && AssetModelMasterList.Count > 0)
                {
                    long totalrecords = AssetModelMasterList.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,
                        rows = (from items in AssetModelMasterList.First().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[]{
                                       items.ModelMasterId.ToString(),                                                                                                                     
                                       items.Brand,                         
                                       items.Model,
                                       items.IsActive==true?"Yes":"No",
                                       items.CreatedBy!=null?us.GetUserNameByUserId(items.CreatedBy):"",
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
        public ActionResult AddAssetModelMaster(AssetModelMaster amm)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    AssetModelMaster ammdetails = ass.GetAssetModelMasterByBrandandModel(amm.Brand.ToUpper(), amm.Model.ToUpper());
                    if (ammdetails != null)
                    {
                        var script = @"ErrMsg(""Already Exist"");";
                        return JavaScript(script);
                    }
                    amm.Brand = amm.Brand.ToUpper();
                    amm.Model = amm.Model.ToUpper();
                    amm.CreatedBy = userId;
                    amm.CreatedDate = DateTime.Now;
                    amm.IsActive = true;
                    ass.CreateOrUpdateAssetModelMaster(amm);
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult EditAssetModelMaster(AssetModelMaster amm)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (amm.ModelMasterId > 0)
                    {
                        AssetModelMaster ammdtls = ass.GetAssetModelMasterByBrandandModel(amm.Brand.ToUpper(), amm.Model.ToUpper());
                        if (ammdtls != null)
                        {
                            var script = @"ErrMsg(""Already Exist"");";
                            return JavaScript(script);
                        }
                        AssetModelMaster ammdetails = ass.GetAssetModelMasterByModelMasterId(amm.ModelMasterId);
                        if (ammdetails != null)
                        {
                            ammdetails.ModifiedBy = userId;
                            ammdetails.ModifiedDate = DateTime.Now;
                            ammdetails.Brand = amm.Brand.ToUpper();
                            ammdetails.Model = amm.Model.ToUpper();
                            ass.CreateOrUpdateAssetModelMaster(ammdetails);
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
        public ActionResult DeleteAssetModelMaster(string Id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    string[] arrayId = Id.Split(',');
                    long[] ModelMasterId = Array.ConvertAll(arrayId, Int64.Parse);
                    ass.DeleteAssetModelMaster(ModelMasterId);
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
        public ActionResult GetModelByBrand(string Brand)
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(Brand))
            {
                criteria.Add("Brand", Brand);
            }
            criteria.Add("IsActive", true);
            Dictionary<long, IList<AssetModelMaster>> assetmodelmaster = ass.GetAssetModelMasterWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
            if (assetmodelmaster != null && assetmodelmaster.First().Value != null && assetmodelmaster.FirstOrDefault().Key > 0)
            {
                var AssetModelList = (
                         from items in assetmodelmaster.First().Value
                         where items.Brand != null && items.Model != null
                         select new
                         {
                             Text = items.Model,
                             Value = items.Model
                         }).Distinct().ToList().OrderBy(x => x.Text);
                return Json(AssetModelList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region AssetInvoiceDetails
        public ActionResult AssetInvoiceDetails()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrEmpty(userId)) return RedirectToAction("LogOff", "Account");
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
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }
        public ActionResult AssetInvoiceDetailsJqGrid(AssetInvoiceDetails assetinvoicedetails, string InvoiceDate, string VendorId, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<string, object> likecriteria = new Dictionary<string, object>();
                sord = sord == "desc" ? "Desc" : "Asc";
                sidx = sidx == "VendorId" ? "VendorMaster.VendorId" : sidx;
                if (assetinvoicedetails != null)
                {
                    if (!string.IsNullOrEmpty(assetinvoicedetails.DocumentType))
                    { criteria.Add("DocumentType", assetinvoicedetails.DocumentType); }
                    if (!string.IsNullOrEmpty(assetinvoicedetails.InvoiceNo))
                    { likecriteria.Add("InvoiceNo", assetinvoicedetails.InvoiceNo); }
                    //if (!string.IsNullOrEmpty(assetinvoicedetails.Warranty))
                    //{ criteria.Add("Warranty", assetinvoicedetails.Warranty); }
                    if (!string.IsNullOrEmpty(VendorId))
                    { criteria.Add("VendorMaster.VendorId", Convert.ToInt64(VendorId)); }
                    if (!string.IsNullOrEmpty(InvoiceDate))
                    { criteria.Add("InvoiceDate", DateTime.Parse(InvoiceDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault)); }
                }
                Dictionary<long, IList<AssetInvoiceDetails>> AssetInvoiceDetailsList = ass.GetAssetInvoiceDetailsWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria, likecriteria);
                if (AssetInvoiceDetailsList != null && AssetInvoiceDetailsList.Count > 0)
                {
                    long totalrecords = AssetInvoiceDetailsList.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,
                        userdata = new
                        {
                            InovoiceDate = "Total:",
                            Amount = AssetInvoiceDetailsList.FirstOrDefault().Value.Sum(x => x.Amount).ToString()
                        },
                        rows = (from items in AssetInvoiceDetailsList.First().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[]{
                                       items.InvoiceDetailsId.ToString(),                                                                                                                     
                                       items.VendorMaster.VendorName,                         
                                       items.InvoiceNo,
                                       items.DocumentType,
                                       //items.DocumentName,
                                       String.Format(@"<a style='color:#034af3;text-decoration:underline' href= '#' onclick = 'uploaddat("+"\"" + items.InvoiceDetailsId + "\"" + ")' >{0}</a>",items.DocumentName),
                                       items.DocumentSize,
                                       items.DocumentData.ToString(),
                                       items.UploadedDate.ToString("dd/MM/yyyy"),
                                       items.InvoiceDate!=null?items.InvoiceDate.Value.ToString("dd/MM/yyyy"):"",
                                       items.Amount.ToString(),
                                       //items.Warranty!=null?CalculateWarrantyAsYear(Convert.ToInt64(items.Warranty)):"0 Year 0 Month",
                                       items.TotalAsset.ToString(),
                                       items.AssetCount.ToString(),                                      
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
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult AddAssetInvoiceDetails(AssetInvoiceDetails assetinvoicedetails, VendorMaster VendorMaster, string InvoiceDate, HttpPostedFileBase file1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrEmpty(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (file1 != null && assetinvoicedetails != null && VendorMaster != null)
                    {
                        AssetInvoiceDetails assetinvoicedtls = ass.GetAssetInvoiceDetailsByInvoiceNowithVendorId(assetinvoicedetails.InvoiceNo, VendorMaster.VendorId);
                        if (assetinvoicedtls != null)
                        {
                            StringBuilder retValue = new StringBuilder();
                            retValue.Append("Document is Already Exist for Invoice No:" + assetinvoicedtls.InvoiceNo + " for " + assetinvoicedtls.VendorMaster.VendorName);
                            return Json(new { success = false, result = retValue.ToString().Replace(Environment.NewLine, "<br />") }, "text/html", JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            string path = file1.InputStream.ToString();
                            byte[] imageSize = new byte[file1.ContentLength];
                            file1.InputStream.Read(imageSize, 0, (int)file1.ContentLength);
                            AssetInvoiceDetails fu1 = new AssetInvoiceDetails();
                            fu1.DocumentData = imageSize;
                            fu1.DocumentName = file1.FileName;
                            fu1.DocumentSize = file1.ContentLength.ToString();
                            fu1.UploadedDate = DateTime.Now;
                            //fu1.DocumentFor = "ITAsset";
                            fu1.DocumentType = assetinvoicedetails.DocumentType;
                            fu1.InvoiceDate = DateTime.Parse(InvoiceDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                            fu1.InvoiceNo = assetinvoicedetails.InvoiceNo;
                            fu1.TotalAsset = assetinvoicedetails.TotalAsset;
                            //fu1.Warranty = assetinvoicedetails.Warranty;
                            fu1.VendorMaster = VendorMaster;
                            fu1.IsActive = true;
                            fu1.CreatedBy = userId;
                            fu1.CreatedDate = DateTime.Now;
                            fu1.Amount = assetinvoicedetails.Amount;
                            ass.CreateOrUpdateAssetInvoiceDetails(fu1);
                            StringBuilder retValue = new StringBuilder();
                            retValue.Append("-------files uploaded successfully-----------");
                            return Json(new { success = true, result = retValue.ToString().Replace(Environment.NewLine, "<br />") }, "text/html", JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                return null;

            }
            catch (Exception)
            {

                throw;
            }
        }
        public JsonResult GetInvoiceNoByVendorIdandDocumentType(long VendorId, string DocumentType)
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            Dictionary<string, object> likecriteria = new Dictionary<string, object>();

            if (VendorId > 0)
            {
                criteria.Add("VendorMaster.VendorId", VendorId);
                criteria.Add("IsActive", true);
                if (!string.IsNullOrEmpty(DocumentType))
                {
                    criteria.Add("DocumentType", DocumentType);
                }
            }
            Dictionary<long, IList<AssetInvoiceDetails>> assetinvoicedetails = ass.GetAssetInvoiceDetailsWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria, likecriteria);
            if (assetinvoicedetails != null && assetinvoicedetails.First().Value != null && assetinvoicedetails.First().Value.Count > 0)
            {
                var VendorNameList = (
                         from items in assetinvoicedetails.First().Value
                         where items.TotalAsset > items.AssetCount
                         select new
                         {
                             Text = items.InvoiceNo,
                             Value = items.InvoiceDetailsId
                         }).Distinct().ToList();
                return Json(VendorNameList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        public string CalculateWarrantyAsYear(long MonthVal)
        {
            string Warranty = string.Empty;
            if (MonthVal > 0)
            {
                long Year = MonthVal / 12;
                long Month = MonthVal % 12;
                Warranty = Year > 1 ? Year + " Years " : Year + " Year ";
                Warranty += Month > 1 ? Month + " Months " : Month + " Month ";
            }
            else
            {
                Warranty = 0 + " Year " + 0 + " Month ";
            }
            return Warranty;
        }
        #endregion
        #region AssetDetailsReport
        public ActionResult AssetDetailsReport()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrEmpty(userId)) return RedirectToAction("LogOff", "Account");
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
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }
        public ActionResult AssetDetailsReportJqGrid(int rows, string sidx, string sord, int? page = 1, long? ExptXl = 0)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<string, object> likecriteria = new Dictionary<string, object>();
                sord = sord == "desc" ? "Desc" : "Asc";
                Dictionary<long, IList<AssetDetailsReport_vw>> AssetDetailsReportList = ass.GetAssetDetailsReport_vwListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (ExptXl == 1)
                {
                    base.ExptToXL(AssetDetailsReportList.FirstOrDefault().Value, "AssetDetailsCountReport-" + DateTime.Today.ToShortDateString(), (items => new
                    {
                        Asset_Type = items.AssetType,
                        items.Using,
                        items.Scrap,
                        items.Service,
                        items.Stock,
                        Total = items.TotalAsset,
                    }));
                    return new EmptyResult();
                }
                else if (AssetDetailsReportList != null && AssetDetailsReportList.Count > 0)
                {
                    long totalrecords = AssetDetailsReportList.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    //var TotalArr = (from p in AssetDetailsReportList.FirstOrDefault().Value
                    //               group p by p.AssetType into assdtls
                    //               select new 
                    //              {
                    //                  cell = new string[]
                    //                  {                                          
                    //                      assdtls.Sum(x=>x.Using).ToString(),
                    //                      assdtls.Sum(x=>x.Scrap).ToString(),
                    //                      assdtls.Sum(x=>x.Service).ToString(),
                    //                      assdtls.Sum(x=>x.Stock).ToString(),
                    //                      assdtls.Sum(x=>x.TotalAsset).ToString(),
                    //                  }

                    //              }).ToList();

                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,
                        userdata = new
                        {
                            AssetType = "Total:",
                            Using = AssetDetailsReportList.FirstOrDefault().Value.Sum(x => x.Using).ToString(),
                            Scrap = AssetDetailsReportList.FirstOrDefault().Value.Sum(x => x.Scrap).ToString(),
                            Service = AssetDetailsReportList.FirstOrDefault().Value.Sum(x => x.Service).ToString(),
                            Stock = AssetDetailsReportList.FirstOrDefault().Value.Sum(x => x.Stock).ToString(),
                            TotalAsset = AssetDetailsReportList.FirstOrDefault().Value.Sum(x => x.TotalAsset).ToString(),
                        },
                        rows = (from items in AssetDetailsReportList.First().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[]{
                                       items.Id.ToString(),                                                                                                                     
                                       items.AssetType,          
                                       items.Using.ToString(),
                                       items.Scrap.ToString(),
                                       items.Service.ToString(),
                                       items.Stock.ToString(),
                                       items.TotalAsset.ToString(),
                                       //"<a href='#' onclick=\"ShowAssetDetails('Using','"+items.AssetType+"');\" >"+items.Using+"</a>",
                                       //"<a href='#' onclick=\"ShowAssetDetails('Scrap','"+items.AssetType+"');\" >"+items.Scrap+"</a>",
                                       //"<a href='#' onclick=\"ShowAssetDetails('Service','"+items.AssetType+"');\" >"+items.Service+"</a>",
                                       //"<a href='#' onclick=\"ShowAssetDetails('Stock','"+items.AssetType+"');\" >"+items.Stock+"</a>",
                                       //"<a href='#' onclick=\"ShowAssetDetails('TotalAsset','"+items.AssetType+"');\" >"+items.TotalAsset+"</a>",                                       
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
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }
        public ActionResult ITAssetDetailsJqgrid(string TransactionType, string AssetType, int rows, string sidx, string sord, int? page = 1, long? ExptXl = 0)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    sord = sord == "desc" ? "Desc" : "Asc";
                    if (sidx == "FormId") sidx = "CampusMaster.FormId";
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<string, object> likecriteria = new Dictionary<string, object>();
                    criteria.Clear();
                    if (!string.IsNullOrEmpty(TransactionType))
                    {
                        if (TransactionType == "Using")
                        {
                            string[] TxnType = new string[4];
                            TxnType[0] = "InterCampus";
                            TxnType[1] = "IntraCampus";
                            TxnType[2] = "Inward";
                            TxnType[3] = "";
                            criteria.Add("TransactionType", TxnType);
                        }
                        else if (TransactionType == "TotalAsset")
                        {

                        }
                        else
                        {
                            criteria.Add("TransactionType", TransactionType);
                        }
                    }
                    if (!string.IsNullOrEmpty(AssetType))
                    {
                        criteria.Add("AssetType", AssetType);
                    }
                    Dictionary<long, IList<AssetDetails>> AssetDetailsList = ass.GetITAssetDetailsWithPagingAndExactAndLikeCriteria(page - 1, rows, sidx, sord, criteria, likecriteria);

                    if (ExptXl == 1)
                    {
                        base.ExptToXL(AssetDetailsList.FirstOrDefault().Value, "AssetDetailsReport-" + DateTime.Today.ToShortDateString(), (items => new
                        {
                            Asset_Code = items.AssetCode,
                            Asset_Type = items.AssetType,
                            Make = items.Make,
                            Serial_Number = items.SerialNo,
                            Transaction_Type = items.TransactionType,
                            Campus = items.CurrentCampus,
                            Block = items.CurrentBlock,
                            Location = items.CurrentLocation,
                            User_Type = items.UserType,
                            Name = items.UserType.Contains("Staff") ? items.StaffDetailsView.Name : items.UserType.Contains("Student") ? items.StudentTemplateView.Name : items.UserType.Contains("Common") ? "" : "",
                        }));
                        return new EmptyResult();
                    }
                    else if (AssetDetailsList != null && AssetDetailsList.Count > 0)
                    {
                        long totalrecords = AssetDetailsList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in AssetDetailsList.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[]{
                                       items.AssetDet_Id.ToString(),
                                       items.AssetCode,
                                       //items.AssetCode!=null?"<a href='/Asset/ITAssetTransaction?AssetDet_Id=" + items.AssetDet_Id +"'>" + items.AssetCode + "</a>":"",
                                       items.Asset_Id.ToString(),
                                       items.AssetType,
                                       items.Make,
                                       items.Model,
                                       items.SerialNo,
                                       items.TransactionType,
                                       //items.Location,
                                       //items.CampusMaster!=null?items.CampusMaster.Name:"",
                                       items.CurrentCampus,
                                       items.CurrentBlock,
                                       items.CurrentLocation,
                                       items.UserType,                                       
                                       //items.UserType.Contains("Staff")?sms.GetStaffNameByPreRegNum(items.PreRegNum):items.UserType.Contains("Student")?abc.GetStudentNameByPreRegNum(items.PreRegNum):items.UserType.Contains("Common")?"":"",                                       
                                       items.UserType.Contains("Staff")?items.StaffDetailsView.Name:items.UserType.Contains("Student")?items.StudentTemplateView.Name:items.UserType.Contains("Common")?"":"",                                       
                                       items.CreatedBy,
                                       items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy"):"",
                                       items.ModifiedBy,
                                       items.ModifiedDate!=null?items.ModifiedDate.Value.ToString("dd/MM/yyyy"):"",
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
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult ShowAssetDetails(string AssetType, string TransactionType)
        {
            try
            {
                ViewBag.AssetType = AssetType;
                ViewBag.TransactionType = TransactionType;
                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        public string CalculateWarrantyAge(DateTime InvoiceDate, string Warranty)
        {
            string WarrantyAge = string.Empty;
            DateTime ExpiryDate = InvoiceDate;
            ExpiryDate = ExpiryDate.AddMonths(Convert.ToInt32(Warranty));
            WarrantyAge = (ExpiryDate - DateTime.Now).Days.ToString();
            WarrantyAge = Convert.ToInt64(WarrantyAge) > 0 ? WarrantyAge : "0";
            return WarrantyAge + " Days";
        }
        #region IT Sub Asset Management
        public ActionResult ITSubAssetManagement()
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
        public ActionResult AddNewITSubAsset(long AssetDet_Id, long AssetId)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    AssetDetails assetDetails = ass.GetAssetDetailsByAssetId(AssetDet_Id);
                    AssetDetailsTemplate assetDetailsObj = ass.GetAssetDetailsTemplateByAssetId(AssetId);
                    List<string> specList = assetDetailsObj.Specifications.Split(',').ToList();
                    List<string> DescList = assetDetailsObj.SpecificationsDetails.Split(',').ToList();
                    assetDetailsObj.specList = specList;
                    ViewBag.DescList = DescList;
                    ViewBag.specList = specList;
                    ViewBag.AssetType = assetDetailsObj.AssetType;
                    return View(assetDetails);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public ActionResult AssetCodeAutoComplete(string Campus, string AssetType, string term)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<string, object> likecriteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(Campus))
                {
                    criteria.Add("FromCampus", Campus);
                }

                if (!string.IsNullOrEmpty(AssetType))
                {
                    criteria.Add("AssetType", AssetType);
                }
                if (!string.IsNullOrEmpty(term))
                {
                    likecriteria.Add("AssetCode", term);
                }
                criteria.Add("IsSubAsset", true);
                criteria.Add("IsActive", true);
                Dictionary<long, IList<AssetDetails>> AssetCodeList = ass.GetITAssetDetailsWithPagingAndExactAndLikeCriteria(0, 9999, "AssetType", "Asc", criteria, likecriteria);
                if (AssetCodeList != null && AssetCodeList.First().Value != null && AssetCodeList.FirstOrDefault().Key > 0)
                {
                    var AssetCode = (
                             from items in AssetCodeList.First().Value
                             where items.AssetCode != null && items.AssetDet_Id > 0 && items.AssetRefId == 0
                             select new
                             {
                                 Text = items.AssetCode,
                                 Value = items.AssetDet_Id
                             }).Distinct().ToList().OrderBy(x => x.Text);
                    return Json(AssetCode, JsonRequestBehavior.AllowGet);
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
        public ActionResult GetAssetDetails(long AssetDet_Id)
        {
            try
            {
                AssetDetails ad = ass.GetAssetDetailsByAssetId(AssetDet_Id);
                if (ad != null)
                {
                    var jsondata = new
                    {
                        Make = ad.Make,
                        Model = ad.Model,
                        Year = Convert.ToInt64(ad.Warranty) / 12,
                        Month = Convert.ToInt64(ad.Warranty) % 12,
                        SerialNo = ad.SerialNo,
                        Department = ad.StaffDetailsView != null ? ad.StaffDetailsView.Department : "",
                        StaffGroup = ad.StaffDetailsView != null ? ad.StaffDetailsView.StaffGroup : "",
                        Name = ad.UserType.Contains("Staff") ? ad.StaffDetailsView.Name : ad.UserType.Contains("Student") ? ad.StudentTemplateView.Name : ad.UserType.Contains("Common") ? "" : "",
                        IdNum = ad.IdNum,
                        Grade = ad.StudentTemplateView != null ? ad.StudentTemplateView.Grade : "",
                        Block = ad.CurrentBlock,
                        Location = ad.CurrentLocation
                    };
                    return Json(jsondata, JsonRequestBehavior.AllowGet);
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult GetSubAssetDetails(long AssetDet_Id)
        {
            try
            {
                SubAssetDetails_vw ad = ass.GetSubAssetDetailsByAssetId(AssetDet_Id);
                if (ad != null)
                {
                    var jsondata = new
                    {
                        Make = ad.Make,
                        Model = ad.Model,
                        Year = Convert.ToInt64(ad.Warranty) / 12,
                        Month = Convert.ToInt64(ad.Warranty) % 12,
                        SerialNo = ad.SerialNo,
                        Department = ad.StaffDetailsView != null ? ad.StaffDetailsView.Department : "",
                        StaffGroup = ad.StaffDetailsView != null ? ad.StaffDetailsView.StaffGroup : "",
                        Name = ad.UserType.Contains("Staff") ? ad.StaffDetailsView.Name : ad.UserType.Contains("Student") ? ad.StudentTemplateView.Name : ad.UserType.Contains("Common") ? "" : "",
                        IdNum = ad.IdNum,
                        Grade = ad.StudentTemplateView != null ? ad.StudentTemplateView.Grade : "",
                        Block = ad.CurrentBlock,
                        Location = ad.CurrentLocation
                    };
                    return Json(jsondata, JsonRequestBehavior.AllowGet);
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult AddSubAssetDetails(AssetDetails assetdetails, string SpecDetails, long FormId)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (assetdetails.SubAssetType == "External")
                    {
                        AssetDetails ad = ass.GetAssetDetailsByAssetId(assetdetails.AssetRefId);
                        AssetDetails subassetdtls = ass.GetAssetDetailsByAssetId(assetdetails.AssetDet_Id);
                        subassetdtls.AssetRefId = assetdetails.AssetRefId;
                        CampusMaster campus = ass.GetAssetDetailsTemplateByFormId(FormId);
                        //subassetdtls.CurrentCampus = campus.Name;
                        //subassetdtls.CurrentBlock = assetdetails.FromBlock;
                        //subassetdtls.CurrentLocation = assetdetails.Location;
                        //subassetdtls.UserType = assetdetails.UserType;
                        subassetdtls.IsStandBy = false;
                        subassetdtls.TransactionType = "Initital";
                        //subassetdtls.IdNum = ad.IdNum;
                        ass.CreateOrUpdateITAssetDetails(subassetdtls);
                        ITAssetDetailsTransactionHistory transactionhistory = new ITAssetDetailsTransactionHistory();
                        transactionhistory.FromCampus = subassetdtls.FromCampus;
                        transactionhistory.FromBlock = subassetdtls.FromBlock;
                        transactionhistory.FromLocation = subassetdtls.Location;
                        //transactionhistory.ToCampus = subassetdtls.CurrentCampus;
                        //transactionhistory.ToLocation = subassetdtls.CurrentLocation;
                        //transactionhistory.ToBlock = subassetdtls.CurrentBlock;
                        //transactionhistory.UserType = assetdetails.UserType;
                        //transactionhistory.IdNum = ad.IdNum;
                        transactionhistory.CreatedBy = userId;
                        transactionhistory.CreatedDate = DateTime.Now;
                        transactionhistory.TransactionType = "Initial";
                        transactionhistory.Amount = subassetdtls.Amount;
                        transactionhistory.InvoiceDetailsId = subassetdtls.InvoiceDetailsId;
                        transactionhistory.Warranty = assetdetails.Warranty;
                        transactionhistory.TransactionType_Id = assetdetails.AssetDet_Id;
                        transactionhistory.AssetDetails = subassetdtls;
                        ass.CreateOrUpdateITAssetDetailsHistory(transactionhistory);
                        return Json("success", JsonRequestBehavior.AllowGet);
                    }
                    else if (assetdetails.SubAssetType == "Internal")
                    {
                        if (assetdetails == null) return null;
                        Dictionary<string, object> criteria = new Dictionary<string, object>();
                        if (assetdetails.Asset_Id > 0)
                            criteria.Add("Asset_Id", assetdetails.Asset_Id);
                        if (!string.IsNullOrEmpty(assetdetails.Make))
                            criteria.Add("Make", assetdetails.Make);
                        if (!string.IsNullOrEmpty(assetdetails.Model))
                            criteria.Add("Model", assetdetails.Model);
                        if (!string.IsNullOrEmpty(assetdetails.Location))
                            criteria.Add("Location", assetdetails.Location);
                        if (!string.IsNullOrEmpty(assetdetails.SerialNo))
                            criteria.Add("SerialNo", assetdetails.SerialNo);
                        if (FormId > 0)
                            criteria.Add("CampusMaster.FormId", FormId);
                        Dictionary<long, IList<AssetDetails>> AssetList = ass.GetITAssetDetailsWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                        if (AssetList == null || AssetList.FirstOrDefault().Key == 0 || assetdetails.SerialNo == null || assetdetails.SerialNo == "")
                        {
                            //Get AssetCode
                            criteria.Clear();
                            if (assetdetails.Asset_Id > 0)
                                criteria.Add("Asset_Id", assetdetails.Asset_Id);
                            Dictionary<long, IList<AssetDetails>> AssetCont = ass.GetITAssetDetailsWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                            var Assetcount = (from u in AssetCont.FirstOrDefault().Value select u).Distinct().ToList();
                            long count = Assetcount.Count;
                            AssetDetailsTemplate assetDetailsObj = ass.GetAssetDetailsTemplateByAssetId(assetdetails.Asset_Id);
                            assetdetails.AssetCode = "TIPS-" + assetDetailsObj.AssetCode + "-" + (count + 1).ToString();
                            AssetDetails ad = ass.GetAssetDetailsByAssetId(assetdetails.AssetRefId);
                            CampusMaster campusmaster = new CampusMaster();
                            campusmaster.FormId = FormId;
                            assetdetails.CampusMaster = campusmaster;
                            assetdetails.Make = assetdetails.Make.ToUpper();
                            assetdetails.Model = assetdetails.Model.ToUpper();
                            assetdetails.Location = assetdetails.Location;
                            assetdetails.FromBlock = assetdetails.FromBlock;
                            assetdetails.TransactionType = "Initial";
                            assetdetails.IsSubAsset = true;
                            assetdetails.EngineerName = userId;
                            assetdetails.IsActive = true;
                            assetdetails.IsStandBy = false;
                            assetdetails.CreatedBy = userId;
                            assetdetails.CreatedDate = DateTime.Now;
                            assetdetails.InvoiceDetailsId = ad.InvoiceDetailsId;
                            //assetdetails.IdNum = ad.IdNum;
                            ass.CreateOrUpdateITAssetDetails(assetdetails);
                            #region commented for IT Asset
                            if (assetdetails.InvoiceDetailsId > 0)
                            {
                                AssetInvoiceDetails assetinvoicedetails = ass.GetAssetInvoiceDetailsByInvoiceDetailsId(assetdetails.InvoiceDetailsId);
                                if (assetinvoicedetails != null)
                                {
                                    assetinvoicedetails.AssetCount = assetinvoicedetails.AssetCount + 1;
                                    if (assetinvoicedetails.TotalAsset == assetinvoicedetails.AssetCount)
                                    {
                                        assetinvoicedetails.IsActive = false;
                                    }
                                    ass.CreateOrUpdateAssetInvoiceDetails(assetinvoicedetails);
                                }
                            }
                            #endregion
                            if (assetdetails.AssetDet_Id > 0)
                            {
                                string[] specList = assetDetailsObj.Specifications.Split(',').ToArray();
                                string[] specValues = SpecDetails.Split(',').ToArray();
                                List<string> specDetailsList = new List<string>();
                                Dictionary<string, object> spec = new Dictionary<string, object>();
                                for (int i = 0; i < specList.Length; i++)
                                {
                                    if (specList[i] == "Id")
                                        spec.Add(specList[i], assetdetails.AssetDet_Id);
                                    else
                                        spec.Add(specList[i], specValues[i]);
                                }
                                JavaScriptSerializer serializer = new JavaScriptSerializer();
                                assetdetails.SpecificationsDetails = serializer.Serialize(spec);
                                CampusMaster campus = ass.GetAssetDetailsTemplateByFormId(assetdetails.CampusMaster.FormId);
                                assetdetails.FromCampus = campus.Name;
                                //assetdetails.CurrentCampus = campus.Name;
                                //assetdetails.CurrentLocation = assetdetails.Location;
                                //assetdetails.CurrentBlock = assetdetails.FromBlock;
                                ass.CreateOrUpdateITAssetDetails(assetdetails);
                                ITAssetDetailsTransactionHistory transactionhistory = new ITAssetDetailsTransactionHistory();
                                transactionhistory.AssetDetails = assetdetails;
                                transactionhistory.FromCampus = assetdetails.FromCampus;
                                transactionhistory.FromBlock = assetdetails.FromBlock;
                                transactionhistory.FromLocation = assetdetails.Location;
                                //transactionhistory.ToCampus = assetdetails.CurrentCampus;
                                //transactionhistory.ToLocation = assetdetails.CurrentLocation;
                                //transactionhistory.ToBlock = assetdetails.CurrentBlock;
                                //transactionhistory.UserType = assetdetails.UserType;
                                //transactionhistory.IdNum = assetdetails.IdNum;
                                transactionhistory.CreatedBy = userId;
                                transactionhistory.CreatedDate = DateTime.Now;
                                transactionhistory.TransactionType = "Initial";
                                transactionhistory.Amount = assetdetails.Amount;
                                transactionhistory.InvoiceDetailsId = assetdetails.InvoiceDetailsId;
                                transactionhistory.Warranty = assetdetails.Warranty;
                                transactionhistory.TransactionType_Id = assetdetails.AssetDet_Id;
                                transactionhistory.AssetRefId = assetdetails.AssetRefId;
                                transactionhistory.IsSubAsset = assetdetails.IsSubAsset;
                                ass.CreateOrUpdateITAssetDetailsHistory(transactionhistory);
                            }
                            return Json("success", JsonRequestBehavior.AllowGet);
                        }
                    }
                    return Json("failed", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public JsonResult FillITSubAssetName(long? FormId)
        {
            string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
            Dictionary<string, object> criteria = new Dictionary<string, object>();            
            criteria.Add("IsSubAsset", true);
            Dictionary<long, IList<AssetDetailsTemplate>> AssetMaster = ass.GetAssetTemplateListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            if (AssetMaster != null && AssetMaster.First().Value != null && AssetMaster.First().Value.Count > 0)
            {
                var assetList = (
                         from items in AssetMaster.First().Value
                         select new
                         {
                             Text = items.AssetType,
                             Value = items.Asset_Id
                         }).Distinct().ToList();
                return Json(assetList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ITAssetCodeAutoComplete(string Campus, string UserType, string term)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<string, object> likecriteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(Campus))
                {
                    criteria.Add("CurrentCampus", Campus);
                }
                if (!string.IsNullOrEmpty(UserType))
                {
                    criteria.Add("UserType", UserType);
                }
                if (!string.IsNullOrEmpty(term))
                {
                    likecriteria.Add("AssetCode", term);
                }
                criteria.Add("IsSubAsset", false);
                criteria.Add("IsActive", true);
                Dictionary<long, IList<AssetDetails>> AssetCodeList = ass.GetITAssetDetailsWithPagingAndExactAndLikeCriteria(0, 9999, "AssetType", "Asc", criteria, likecriteria);
                if (AssetCodeList != null && AssetCodeList.First().Value != null && AssetCodeList.FirstOrDefault().Key > 0)
                {
                    var AssetCode = (
                             from items in AssetCodeList.First().Value
                             where items.AssetCode != null && items.TransactionType != "Stock" && items.TransactionType != "Service" && items.CurrentBlock != "Stock"
                             select new
                             {
                                 Text = items.AssetCode,
                                 Value = items.AssetDet_Id
                             }).Distinct().ToList().OrderBy(x => x.Text);
                    return Json(AssetCode, JsonRequestBehavior.AllowGet);
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
        public ActionResult ITSubAssetManagementjqGrid(SubAssetDetails_vw assetdetails, long? FormId, long? AssetRefId, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    sord = sord == "desc" ? "Desc" : "Asc";
                    if (sidx == "FormId") sidx = "CampusMaster.FormId";
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<string, object> likecriteria = new Dictionary<string, object>();
                    criteria.Clear();
                    if (assetdetails != null)
                    {

                        if (!string.IsNullOrEmpty(assetdetails.AssetCode))
                            likecriteria.Add("AssetCode", assetdetails.AssetCode);
                        //if (!string.IsNullOrEmpty(assetdetails.AssetType))
                        //    likecriteria.Add("AssetType", assetdetails.AssetType);
                        if (!string.IsNullOrEmpty(assetdetails.Make))
                            likecriteria.Add("Make", assetdetails.Make);
                        if (!string.IsNullOrEmpty(assetdetails.Model))
                            likecriteria.Add("Model", assetdetails.Model);
                        if (!string.IsNullOrEmpty(assetdetails.SerialNo))
                            likecriteria.Add("SerialNo", assetdetails.SerialNo);
                        if (!string.IsNullOrEmpty(assetdetails.CurrentBlock))
                            criteria.Add("CurrentBlock", assetdetails.CurrentBlock);
                        if (!string.IsNullOrEmpty(assetdetails.CurrentLocation))
                            criteria.Add("CurrentLocation", assetdetails.CurrentLocation);
                        if (!string.IsNullOrEmpty(assetdetails.TransactionType))
                            criteria.Add("TransactionType", assetdetails.TransactionType);
                        if (!string.IsNullOrEmpty(assetdetails.UserType))
                            criteria.Add("UserType", assetdetails.UserType);
                    }
                    if (FormId > 0)
                    {
                        likecriteria.Add("CampusMaster.FormId", FormId);
                    }
                    if (AssetRefId != null)
                    {
                        criteria.Add("AssetRefId", AssetRefId);
                    }
                    if (!string.IsNullOrEmpty(assetdetails.AssetType))
                        criteria.Add("Asset_Id", Convert.ToInt64(assetdetails.AssetType));
                    Dictionary<long, IList<SubAssetDetails_vw>> AssetDetailsList = ass.GetITSubAssetDetails_vwWithPagingAndExactAndLikeCriteria(page - 1, rows, sidx, sord, criteria, likecriteria);
                    if (AssetDetailsList != null && AssetDetailsList.Count > 0)
                    {
                        StaffManagementService sms = new StaffManagementService();
                        AdmissionManagementService abc = new AdmissionManagementService();
                        long totalrecords = AssetDetailsList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in AssetDetailsList.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[]{
                                       items.AssetDet_Id.ToString(),
                                       items.AssetCode!=null?"<a href='/Asset/ITSubAssetTransaction?AssetDet_Id=" + items.AssetDet_Id +"'>" + items.AssetCode + "</a>":"",
                                       items.Asset_Id.ToString(),
                                       items.AssetType,
                                       items.Make,
                                       items.Model,
                                       items.SerialNo,
                                       items.TransactionType,
                                       //items.CampusMaster!=null?items.CampusMaster.Name:"",
                                       items.CurrentCampus,
                                       items.CurrentBlock,
                                       items.CurrentLocation,
                                       items.UserType,                                       
                                       //items.UserType.Contains("Staff")?sms.GetStaffNameByPreRegNum(items.PreRegNum):items.UserType.Contains("Student")?abc.GetStudentNameByPreRegNum(items.PreRegNum):items.UserType.Contains("Common")?"":"",                                       
                                       items.UserType.Contains("Staff")?items.StaffDetailsView.Name:items.UserType.Contains("Student")?items.StudentTemplateView.Name:items.UserType.Contains("Common")?"":"",                                       
                                       items.CreatedBy,
                                       items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy"):"",
                                       items.ModifiedBy,
                                       items.ModifiedDate!=null?items.ModifiedDate.Value.ToString("dd/MM/yyyy"):"",
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
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult ITSubAssetTransaction(long AssetDet_Id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MastersService ms = new MastersService();
                    AssetService assetServ = new AssetService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Clear();
                    SubAssetDetails_vw assetDetails = new SubAssetDetails_vw();
                    AssetDetailsTemplate assetDetailsTemplate = new AssetDetailsTemplate();
                    var usrcmp = Session["UserCampus"] as IEnumerable<string>;
                    if (usrcmp.First() != null)            // to check if the usrcmp obj is null or with data
                    {
                        criteria.Add("Name", usrcmp);
                    }
                    Dictionary<long, IList<CampusMaster>> CampusMaster = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                    criteria.Clear();
                    criteria.Add("AssetDetails.AssetDet_Id", AssetDet_Id);
                    Dictionary<long, IList<ITAssetServiceDetails>> serviceDetails = ass.GetITAssetServiceDetailsWithPagingAndCriteria(0, 99999, "AssetService_Id", "desc", criteria);
                    if (AssetDet_Id > 0)
                    {
                        assetDetails = assetServ.GetSubAssetDetailsByAssetId(AssetDet_Id);
                        assetDetailsTemplate = assetServ.GetAssetDetailsTemplateByAssetId(assetDetails.Asset_Id);
                    }
                    if (serviceDetails != null && serviceDetails.FirstOrDefault().Key > 0)
                    {
                        var DCDate = (from u in serviceDetails.FirstOrDefault().Value where u.InwardDate == null select u.DCDate).ToArray();
                        if (DCDate.Length > 0)
                        {
                            ViewBag.DCDate = DCDate[0].Value.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            ViewBag.DCDate = "";
                        }
                    }
                    else
                    {
                        ViewBag.DCDate = "";
                    }
                    var jsonString = assetDetails.SpecificationsDetails;
                    var json = JValue.Parse(jsonString);
                    var specList = (from u in json select u).ToList();
                    ViewBag.specList = specList;
                    ViewBag.DescList = assetDetailsTemplate.SpecificationsDetails.Split(',');
                    ViewBag.campusddl = CampusMaster.First().Value;
                    AssetDetails ParentassetDetails = new AssetDetails();
                    if (assetDetails != null)
                    {
                        ParentassetDetails = assetDetails.AssetRefId > 0 ? assetServ.GetAssetDetailsByAssetId(assetDetails.AssetRefId) : null;
                    }
                    return View(assetDetails);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpPost]
        public ActionResult ITSubAssetTransaction(AssetDetails assetDetails)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MastersService ms = new MastersService();
                    AssetService assetServ = new AssetService();
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Clear();
                    var usrcmp = Session["UserCampus"] as IEnumerable<string>;
                    if (usrcmp.First() != null)            // to check if the usrcmp obj is null or with data
                    {
                        criteria.Add("Name", usrcmp);
                    }
                    Dictionary<long, IList<CampusMaster>> CampusMaster = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                    criteria.Clear();
                    ViewBag.campusddl = CampusMaster.First().Value;
                    AssetDetails assetDetailsObj = new AssetDetails();
                    if (assetDetails.Asset_Id > 0)
                        assetDetailsObj = assetServ.GetAssetDetailsByAssetId(assetDetails.AssetDet_Id);
                    var jsonString = assetDetailsObj.SpecificationsDetails;
                    var json = JValue.Parse(jsonString);
                    var specList = (from u in json select u).ToList();
                    ViewBag.specList = specList;
                    ITAssetDetailsTransactionHistory history = new ITAssetDetailsTransactionHistory();
                    if (Request.Form["btnSave"] == "SaveAsset" && assetDetails.IsSubAsset == true)
                    {
                        if (assetDetailsObj != null && assetDetails.TransactionType == "IntraCampus" || assetDetails.TransactionType == "InterCampus" || assetDetails.TransactionType == "Stock")
                        {
                            if (assetDetails.UserType == "Student" && assetDetails.TransactionType == "InterCampus" && assetDetailsObj.StudentTemplateView.Campus != assetDetails.AssetDetailsTransaction.ToCampus)
                            {
                                return RedirectToAction("ITAssetTransaction", new { assetDetails.AssetDet_Id });
                            }
                            else
                            {
                                assetDetails.AssetDetailsTransaction.AssetDet_Id = assetDetailsObj.AssetDet_Id;
                                assetDetails.AssetDetailsTransaction.AssetCode = assetDetailsObj.AssetCode;
                                assetDetails.AssetDetailsTransaction.FromCampus = assetDetailsObj.CurrentCampus;
                                assetDetails.AssetDetailsTransaction.FromBlock = assetDetailsObj.CurrentBlock;
                                assetDetails.AssetDetailsTransaction.FromLocation = assetDetailsObj.CurrentLocation;
                                assetDetails.AssetDetailsTransaction.AssetRefId = assetDetails.AssetRefId;
                                assetDetails.AssetDetailsTransaction.IsSubAsset = assetDetails.IsSubAsset;
                                history.FromCampus = assetDetailsObj.CurrentCampus;
                                history.FromBlock = assetDetailsObj.CurrentBlock;
                                history.FromLocation = assetDetailsObj.CurrentLocation;
                                if (assetDetails.TransactionType == "IntraCampus")
                                {
                                    assetDetails.AssetDetailsTransaction.ToCampus = assetDetailsObj.CurrentCampus;
                                }
                                if (assetDetails.TransactionType == "Stock")
                                {
                                    assetDetails.AssetDetailsTransaction.ToCampus = assetDetailsObj.CurrentCampus;
                                    assetDetails.AssetDetailsTransaction.FromCampus = assetDetailsObj.CurrentCampus;
                                    assetDetails.AssetDetailsTransaction.ToBlock = "Stock";
                                    assetDetails.AssetDetailsTransaction.ToLocation = "Stock";
                                }
                                assetDetails.AssetDetailsTransaction.InstalledOn = DateTime.ParseExact(Request.Form["InstalledOn"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                assetDetails.AssetDetailsTransaction.EngineerName = userId;
                                assetDetails.AssetDetailsTransaction.CreatedBy = userId;
                                assetDetails.AssetDetailsTransaction.CreatedDate = DateTime.Now;
                                assetServ.CreateOrUpdateAssetDetailsTransaction(assetDetails.AssetDetailsTransaction);
                                history.TransactionType_Id = assetDetails.AssetDetailsTransaction.AssetTrans_Id;
                                if (assetDetails.AssetDetailsTransaction.AssetTrans_Id > 0)
                                {
                                    assetDetailsObj.CurrentCampus = assetDetails.AssetDetailsTransaction.ToCampus;
                                    assetDetailsObj.CurrentBlock = assetDetails.AssetDetailsTransaction.ToBlock;
                                    assetDetailsObj.CurrentLocation = assetDetails.AssetDetailsTransaction.ToLocation;
                                    assetDetailsObj.TransactionType = assetDetails.TransactionType;
                                    assetDetailsObj.IsStandBy = false;
                                    if (assetDetails.TransactionType == "Stock")
                                    {
                                        assetDetailsObj.IsStandBy = true;
                                    }
                                    assetDetailsObj.IsActive = true;
                                    assetDetailsObj.EngineerName = userId;
                                    assetDetailsObj.ModifiedBy = userId;
                                    assetDetailsObj.ModifiedDate = DateTime.Now;
                                    assetDetailsObj.IdNum = assetDetails.IdNum;
                                    assetDetailsObj.UserType = assetDetails.UserType;
                                    assetDetailsObj.AssetRefId = assetDetails.AssetRefId;
                                    assetServ.CreateOrUpdateITAssetDetails(assetDetailsObj);
                                }
                            }
                        }
                        if (assetDetails.TransactionType == "Service")
                        {
                            assetDetails.ITAssetServiceDetails.AssetDetails = assetDetailsObj;
                            assetDetails.ITAssetServiceDetails.EngineerName = userId;
                            //assetDetails.ITAssetServiceDetails.FromCampus = assetDetailsObj.CurrentCampus;
                            //assetDetails.ITAssetServiceDetails.FromBlock = assetDetailsObj.CurrentBlock;
                            // assetDetails.ITAssetServiceDetails.FromLocation = assetDetailsObj.CurrentLocation;
                            assetDetails.ITAssetServiceDetails.AssetRefId = assetDetails.AssetRefId;
                            assetDetails.ITAssetServiceDetails.IsSubAsset = assetDetails.IsSubAsset;
                            //history.FromCampus = assetDetailsObj.CurrentCampus;
                            //history.FromBlock = assetDetailsObj.CurrentBlock;
                            //history.FromLocation = assetDetailsObj.CurrentLocation;
                            assetDetails.ITAssetServiceDetails.DCDate = assetDetails.AssetDetailsTransaction.InstalledOn = DateTime.ParseExact(Request.Form["ITAssetServiceDetails.DCDate"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            assetDetails.ITAssetServiceDetails.ExpectedDate = DateTime.ParseExact(Request.Form["ITAssetServiceDetails.ExpectedDate"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            assetDetails.ITAssetServiceDetails.CreatedBy = userId;
                            assetDetails.ITAssetServiceDetails.CreatedDate = DateTime.Now;
                            assetServ.CreateOrUpdateITAssetServiceDetails(assetDetails.ITAssetServiceDetails);
                            history.TransactionType_Id = assetDetails.ITAssetServiceDetails.AssetService_Id;
                            if (assetDetails.ITAssetServiceDetails.AssetService_Id > 0)
                            {
                                //assetDetailsObj.CurrentCampus = assetDetails.TransactionType;
                                //assetDetailsObj.CurrentLocation = assetDetails.ITAssetServiceDetails.Vendor;
                                //assetDetailsObj.CurrentBlock = assetDetails.ITAssetServiceDetails.Vendor;
                                assetDetailsObj.TransactionType = assetDetails.TransactionType;
                                assetDetailsObj.InstalledOn = assetDetails.ITAssetServiceDetails.DCDate;
                                assetDetailsObj.IsActive = true;
                                assetDetailsObj.IsStandBy = false;
                                assetDetailsObj.EngineerName = userId;
                                assetDetailsObj.ModifiedBy = userId;
                                assetDetailsObj.ModifiedDate = DateTime.Now;
                                assetServ.CreateOrUpdateITAssetDetails(assetDetailsObj);
                            }
                        }
                        if (assetDetails.TransactionType == "Scrap")
                        {
                            assetDetails.ITAssetScrapDetails.AssetDetails = assetDetailsObj;
                            assetDetails.ITAssetScrapDetails.InwardDate = DateTime.ParseExact(Request.Form["ITAssetScrapDetails.InwardDate"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            assetDetails.ITAssetScrapDetails.EngineerName = userId;
                            assetDetails.ITAssetScrapDetails.FromCampus = assetDetailsObj.CurrentCampus;
                            assetDetails.ITAssetScrapDetails.FromBlock = assetDetailsObj.CurrentBlock;
                            assetDetails.ITAssetScrapDetails.FromLocation = assetDetailsObj.CurrentLocation;
                            assetDetails.ITAssetScrapDetails.AssetRefId = assetDetails.AssetRefId;
                            assetDetails.ITAssetScrapDetails.IsSubAsset = assetDetails.IsSubAsset;
                            history.FromCampus = assetDetails.CurrentCampus;
                            history.FromBlock = assetDetails.CurrentBlock;
                            history.FromLocation = assetDetails.CurrentLocation;
                            assetDetails.ITAssetScrapDetails.CreatedBy = userId;
                            assetDetails.ITAssetScrapDetails.CreatedDate = DateTime.Now;
                            assetServ.CreateOrUpdateITAssetScrapDetails(assetDetails.ITAssetScrapDetails);
                            history.TransactionType_Id = assetDetails.ITAssetScrapDetails.AssetScrap_Id;
                            if (assetDetails.ITAssetScrapDetails.AssetScrap_Id > 0)
                            {
                                assetDetailsObj.CurrentCampus = assetDetailsObj.CurrentCampus;//changed by prabakaran
                                assetDetailsObj.CurrentLocation = "Scrap";
                                assetDetailsObj.CurrentBlock = "Scrap";
                                assetDetailsObj.InstalledOn = assetDetails.ITAssetScrapDetails.InwardDate;
                                assetDetailsObj.TransactionType = assetDetails.TransactionType;
                                assetDetailsObj.IsActive = false;
                                assetDetailsObj.IsStandBy = false;
                                assetDetailsObj.EngineerName = userId;
                                assetDetailsObj.ModifiedBy = userId;
                                assetDetailsObj.ModifiedDate = DateTime.Now;
                                assetServ.CreateOrUpdateITAssetDetails(assetDetailsObj);
                            }
                        }
                        history.AssetDetails = assetDetailsObj;
                        history.TransactionType = assetDetails.TransactionType;
                        history.ToBlock = assetDetailsObj.CurrentBlock;
                        history.ToLocation = assetDetailsObj.CurrentLocation;
                        history.ToCampus = assetDetailsObj.CurrentCampus;
                        history.CreatedBy = userId;
                        history.CreatedDate = DateTime.Now;
                        history.UserType = assetDetailsObj.UserType;
                        history.IdNum = assetDetailsObj.IdNum;
                        history.IsSubAsset = assetDetailsObj.IsSubAsset;
                        history.AssetRefId = assetDetailsObj.AssetRefId;
                        history.InvoiceDetailsId = assetDetailsObj.InvoiceDetailsId;
                        history.Warranty = assetDetailsObj.Warranty;
                        assetServ.CreateOrUpdateITAssetDetailsHistory(history);
                        return RedirectToAction("ITAssetTransaction", new { AssetDet_Id = assetDetails.AssetRefId });
                    }
                    return RedirectToAction("ITAssetManagement");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion
        #region ITSubAssetService
        public ActionResult ITSubAssetService()
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
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult ITSubAssetServiceReturn(long AssetId)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    var usrcmp = Session["UserCampus"] as IEnumerable<string>;
                    if (usrcmp.First() != null)            // to check if the usrcmp obj is null or with data
                    {
                        criteria.Add("Name", usrcmp);
                    }
                    Dictionary<long, IList<CampusMaster>> CampusMaster = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                    criteria.Clear();
                    criteria.Add("AssetDetails.AssetDet_Id", AssetId);
                    Dictionary<long, IList<ITAssetServiceDetails>> serviceDetails = ass.GetITAssetServiceDetailsWithPagingAndCriteria(0, 99999, "AssetService_Id", "desc", criteria);
                    ITAssetDetailsTransactionHistory transaction = ass.GetITAssetDetailsTransactionHistoryByTransactionType_IdwithAssetId(AssetId, serviceDetails.First().Value[0].AssetService_Id);
                    SubAssetDetails_vw assetDetailsObj = ass.GetSubAssetDetailsByAssetId(AssetId);
                    AssetDetailsTemplate assetdtls = ass.GetAssetDetailsTemplateByAssetId(assetDetailsObj.Asset_Id);
                    if (serviceDetails != null && serviceDetails.FirstOrDefault().Key > 0)
                    {
                        var DCDate = (from u in serviceDetails.FirstOrDefault().Value where u.InwardDate == null select u.DCDate).ToArray();
                        if (DCDate.Length > 0)
                        {
                            ViewBag.DCDate = DCDate[0].Value.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            ViewBag.DCDate = "";
                        }
                    }
                    else
                    {
                        ViewBag.DCDate = "";
                    }
                    List<string> specList = assetdtls.Specifications.Split(',').ToList();
                    List<string> DescList = assetdtls.SpecificationsDetails.Split(',').ToList();
                    //assetdtls.specList = specList;
                    ViewBag.DescList = DescList;
                    ViewBag.specList = specList;
                    ViewBag.campusddl = CampusMaster.First().Value;
                    assetDetailsObj.AssetHistory = transaction;
                    return View(assetDetailsObj);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion
        //public ActionResult BarCodeScan(string value)
        //{
        //    try
        //    {
        //        //string test = Server.UrlDecode(Request.QueryString["value"]);
        //        string strtextToPrint = "BarCode,";
        //        if (!string.IsNullOrEmpty(Request.QueryString["value"].ToString()))
        //        {
        //            //strtextToPrint = Request.QueryString["value"].ToString() + "','";
        //            strtextToPrint = Request.QueryString["value"].ToString() + ",";
        //        }
        //        string[] strArrFormCode = strtextToPrint.Split(',');
        //        string strCommand = "O" + (char)System.Web.UI.DataVisualization.Charting.Keys.LineFeed + "Q200,24" + (char)Keys.LineFeed + "q820" + (char)Keys.LineFeed + "D12" + (char)Keys.LineFeed + "ZT" + (char)Keys.LineFeed + "JF";
        //        bool flag = true;
        //        int j = strArrFormCode.Length;
        //        for (int i = 0; i < strArrFormCode.Length - 1; i++)
        //        {
        //            //strCommand += (char)Keys.LineFeed + "N" + (char)Keys.LineFeed + "B30,20,0,1,2,2,60,B,\"" + strArrFormCode[i] + "\""
        //            //    + (char)Keys.LineFeed + "A30,124,0,3,2,2,N,\"\"" + (char)Keys.LineFeed
        //            //    + "P1" + (char)Keys.LineFeed + "N" + (char)Keys.LineFeed;                

        //            //strCommand += (char)Keys.LineFeed + "N" + (char)Keys.LineFeed + "A30,20,0,1,2,2,N,\"" + strArrFormCode[i] + "\"" + (char)Keys.LineFeed + "B30,45,0,1,2,2,60,B,\"" + strArrFormCode[i] + "\""
        //            //    + (char)Keys.LineFeed + "A30,149,0,3,2,2,N,\"\"" + (char)Keys.LineFeed 
        //            //    + "P1" + (char)Keys.LineFeed + "N" + (char)Keys.LineFeed;                       
        //            AssetDetails assetdetails = ass.GetAssetDetailsByAssetId(Convert.ToInt64(strArrFormCode[i]));
        //            if (flag == true)
        //            {
        //                j = j - 1;
        //                strCommand += (char)Keys.LineFeed + "N" + (char)Keys.LineFeed + "A30,20,0,1,2,2,N,\"" + strArrFormCode[i] + "\"" + (char)Keys.LineFeed + "B30,45,0,1,2,2,60,B,\"" + strArrFormCode[i] + "\""
        //                    + (char)Keys.LineFeed + "A30,149,0,3,2,2,N,\"\"";
        //                if (j == 1)
        //                {
        //                    strCommand += (char)Keys.LineFeed + "P1" + (char)Keys.LineFeed + "N" + (char)Keys.LineFeed;
        //                }
        //                flag = false; 
        //            }
        //            else if (flag == false)
        //            {
        //                j = j - 1;
        //                strCommand += (char)Keys.LineFeed + "A430,20,0,1,2,2,N,\"" + strArrFormCode[i] + "\"" + (char)Keys.LineFeed + "B430,45,0,1,2,2,60,B,\"" + strArrFormCode[i] + "\""
        //                    + (char)Keys.LineFeed + "A430,149,0,3,2,2,N,\"\"" + (char)Keys.LineFeed;
        //                flag = true;
        //                if (j > 1 || j == 1)
        //                {
        //                    strCommand += "P1" + (char)Keys.LineFeed + "N" + (char)Keys.LineFeed;
        //                }
        //            }

        //        }
        //        Response.Write(strCommand);
        //    }

        //    catch (Exception ex)
        //    {
        //        //Response.Status = false;   
        //        Response.Write(ex.Message);
        //    }
        //    finally
        //    {
        //        //if (myConnection.State == ConnectionState.Open) myConnection.Close();
        //    }
        //}
        #region AssetBulkUpload
        public ActionResult BulkAssetUpload()
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
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult BulkAssetUpload(HttpPostedFileBase[] uploadedFile, long Asset_Id, long FormId, bool IsSubAsset, long InvoiceDetailsId)
        {
            StringBuilder retValue = new StringBuilder();
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    UserService us = new UserService();
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
                                string path = uploadedFile[l].InputStream.ToString();
                                byte[] imageSize = new byte[uploadedFile[l].ContentLength];
                                uploadedFile[l].InputStream.Read(imageSize, 0, (int)uploadedFile[l].ContentLength);
                                string UploadConnStr = "";
                                fileName = uploadedFile[l].FileName;
                                string fileExtn = Path.GetExtension(uploadedFile[l].FileName);
                                string fileLocation = ConfigurationManager.AppSettings["BulkUserCreationFilePath"].ToString() + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss").Replace(":", ".") + fileName;
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
                                System.Data.DataTable DtblXcelItemData = new System.Data.DataTable();
                                string QeryToGetXcelItemData = "select * from " + string.Format("{0}${1}", "[Sheet1", "A1:AZ]");
                                itemconn.ConnectionString = UploadConnStr;
                                itemconn.Open();
                                OleDbCommand cmd = new OleDbCommand(QeryToGetXcelItemData, itemconn);
                                cmd.CommandType = CommandType.Text;
                                OleDbDataAdapter DtAdptrr = new OleDbDataAdapter();
                                DtAdptrr.SelectCommand = cmd;
                                DtAdptrr.Fill(DtblXcelItemData);
                                //string[] strArray = { "Brand", "Model", "SerialNo" };
                                string[] strArray = { "Brand", "Model", "SerialNo", "Warranty(In Months)", "Amount" };
                                AssetDetailsTemplate adtemplate = ass.GetAssetDetailsTemplateByAssetId(Asset_Id);
                                if (adtemplate != null)
                                {
                                    strArray = strArray.Concat(adtemplate.SpecificationsDetails.Split(',')).ToArray();
                                }
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

                                        IList<AssetDetails> AssetDetailsList = new List<AssetDetails>();
                                        IList<ITAssetDetailsTransactionHistory> TransactionHistoryList = new List<ITAssetDetailsTransactionHistory>();
                                        criteria.Clear();
                                        if (Asset_Id > 0)
                                            criteria.Add("Asset_Id", Asset_Id);
                                        Dictionary<long, IList<AssetDetails>> AssetCont = ass.GetITAssetDetailsWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                                        var Assetcount = (from u in AssetCont.FirstOrDefault().Value select u).Distinct().ToList();
                                        long count = Assetcount.Count;
                                        AssetInvoiceDetails assetinvoicedetails = ass.GetAssetInvoiceDetailsByInvoiceDetailsId(InvoiceDetailsId);
                                        long invoiceremainigcount = assetinvoicedetails.TotalAsset - assetinvoicedetails.AssetCount;
                                        long invoicecount = assetinvoicedetails.AssetCount;
                                        if (invoiceremainigcount < DtblXcelItemData.Rows.Count)
                                        {
                                            return Json(new { success = true, result = "Excel has More Assets for this Invoice", status = "failed" }, "text/html", JsonRequestBehavior.AllowGet);
                                        }
                                        foreach (DataRow item in DtblXcelItemData.Rows)
                                        {
                                            Dictionary<string, object> likecriteria = new Dictionary<string, object>();
                                            criteria.Clear();
                                            if (!string.IsNullOrEmpty(item["Brand"] != null ? item["Brand"].ToString().Trim() : ""))
                                                criteria.Add("Make", item["Brand"].ToString().Trim().ToUpper());
                                            if (!string.IsNullOrEmpty(item["Model"] != null ? item["Model"].ToString().Trim() : ""))
                                                criteria.Add("Model", item["Model"].ToString().Trim().ToUpper());
                                            if (!string.IsNullOrEmpty(item["SerialNo"] != null ? item["SerialNo"].ToString().Trim() : ""))
                                                criteria.Add("SerialNo", item["SerialNo"].ToString().Trim());
                                            Dictionary<long, IList<AssetDetails>> assetdetails = ass.GetITAssetDetailsWithPagingAndExactAndLikeCriteria(0, 9999, "AssetDet_Id", "Asc", criteria, likecriteria);
                                            if (assetdetails == null || assetdetails.FirstOrDefault().Key == 0)
                                            {
                                                AssetDetails assetdtls = new AssetDetails();
                                                assetdtls.Asset_Id = adtemplate.Asset_Id;
                                                assetdtls.AssetType = adtemplate.AssetType;
                                                assetdtls.Model = item["Model"] != null ? item["Model"].ToString().Trim().ToUpper() : "";
                                                assetdtls.Make = item["Brand"] != null ? item["Brand"].ToString().Trim().ToUpper() : "";
                                                assetdtls.SerialNo = item["SerialNo"] != null ? item["SerialNo"].ToString().Trim() : "";
                                                assetdtls.AssetCode = "TIPS-" + adtemplate.AssetCode + "-" + (count + 1).ToString();
                                                MastersService ms = new MastersService();
                                                CampusMaster cm = ms.GetCampusById(FormId);
                                                assetdtls.CurrentCampus = cm.Name;
                                                assetdtls.FromCampus = cm.Name;
                                                assetdtls.CampusMaster = cm;
                                                assetdtls.CreatedDate = DateTime.Now;
                                                assetdtls.CreatedBy = userId;
                                                assetdtls.TransactionType = "Stock";
                                                assetdtls.Location = "Stock";
                                                assetdtls.CurrentBlock = "Stock";
                                                assetdtls.CurrentLocation = "Stock";
                                                assetdtls.FromBlock = "Stock";
                                                assetdtls.UserType = "Not Applicable";
                                                assetdtls.IsStandBy = true;
                                                assetdtls.IsActive = true;
                                                assetdtls.Amount = item["Amount"] != null ? Convert.ToDecimal(item["Amount"]) : 0;
                                                assetdtls.Warranty = item["Warranty(In Months)"] != null ? item["Warranty(In Months)"].ToString().Trim() : "";
                                                assetdtls.EngineerName = userId;
                                                if (IsSubAsset == true)
                                                {
                                                    assetdtls.SubAssetType = "External";
                                                    assetdtls.IsSubAsset = true;
                                                    assetdtls.UserType = null;
                                                }
                                                string[] specs = adtemplate.SpecificationsDetails.Split(',').ToArray();
                                                for (int k = 0; k < specs.Length; k++)
                                                {
                                                    item[specs[k]] = item[specs[k]] != null ? item[specs[k]].ToString().Trim() : "";
                                                    assetdtls.SpecificationsDetails += "," + item[specs[k]];
                                                }
                                                assetdtls.InvoiceDetailsId = assetinvoicedetails.InvoiceDetailsId;
                                                AssetDetailsList.Add(assetdtls);
                                                invoicecount = invoicecount + 1;
                                                if (assetinvoicedetails.TotalAsset == invoicecount)
                                                {
                                                    assetinvoicedetails.IsActive = false;
                                                }
                                                count = count + 1;
                                            }
                                        }
                                        if (AssetDetailsList.Count > 0)
                                        {
                                            ass.SaveOrUpdateAssetDetailsList(AssetDetailsList);
                                            assetinvoicedetails.AssetCount = invoicecount;
                                            ass.CreateOrUpdateAssetInvoiceDetails(assetinvoicedetails);
                                            foreach (var item in AssetDetailsList)
                                            {
                                                string[] specList = adtemplate.Specifications.Split(',').ToArray();
                                                string[] specValues = item.SpecificationsDetails.Split(',').ToArray();

                                                List<string> specDetailsList = new List<string>();

                                                Dictionary<string, object> spec = new Dictionary<string, object>();
                                                for (int i = 0; i < specList.Length; i++)
                                                {
                                                    if (specList[i] == "Id")
                                                        spec.Add(specList[i], item.AssetDet_Id);
                                                    else
                                                        spec.Add(specList[i], specValues[i]);
                                                }
                                                JavaScriptSerializer serializer = new JavaScriptSerializer();
                                                item.SpecificationsDetails = serializer.Serialize(spec);
                                                ITAssetDetailsTransactionHistory transactionhistory = new ITAssetDetailsTransactionHistory();
                                                transactionhistory.AssetDetails = item;
                                                transactionhistory.FromCampus = item.FromCampus;
                                                transactionhistory.FromBlock = item.FromBlock;
                                                transactionhistory.FromLocation = item.Location;
                                                transactionhistory.ToCampus = item.CurrentCampus;
                                                transactionhistory.ToLocation = item.CurrentLocation;
                                                transactionhistory.ToBlock = item.CurrentBlock;
                                                transactionhistory.UserType = item.UserType;
                                                transactionhistory.CreatedBy = userId;
                                                transactionhistory.CreatedDate = DateTime.Now;
                                                transactionhistory.TransactionType = "Stock";
                                                transactionhistory.TransactionType_Id = item.AssetDet_Id;
                                                TransactionHistoryList.Add(transactionhistory);
                                            }
                                            ass.SaveOrUpdateAssetDetailsList(AssetDetailsList);
                                            ass.SaveOrUpdateITAssetDetailsTransactionHistoryList(TransactionHistoryList);
                                            return Json(new { success = true, result = "Asset created successfully..", status = "success" }, "text/html", JsonRequestBehavior.AllowGet);
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
                                ErrorLogs el = new ErrorLogs();
                                el.ExceptionErrorLog = ex.ToString();
                                ass.CreateOrUpdateErrorLogs(el);
                                ErrorFilename.Append(fileName + ",");
                                ExceptionPolicy.HandleException(ex, "CustomAccountPolicy");
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
                ExceptionPolicy.HandleException(ex, "CustomAccountPolicy");
            }
            //MyLabel.Text = sb.ToString().Replace(Environment.NewLine, "<br />");
            return Json(new { success = true, result = retValue.ToString().Replace(Environment.NewLine, "<br />") }, "text/html", JsonRequestBehavior.AllowGet);
        }
        public ActionResult GenerateExcelFormat(long Asset_Id)
        {
            string[] Columns = { "Brand", "Model", "SerialNo", "Warranty(In Months)", "Amount" };

            AssetDetailsTemplate adtemplate = ass.GetAssetDetailsTemplateByAssetId(Asset_Id);
            if (adtemplate != null)
            {
                Columns = Columns.Concat(adtemplate.SpecificationsDetails.Split(',')).ToArray();
            }
            ExcelPackage objExcelPackage = new ExcelPackage();   //create new workbook

            //string[] filesindirectory = Directory.GetFiles(Server.MapPath("~/Images"));            
            int count = 1;
            ExcelWorksheet ews = objExcelPackage.Workbook.Worksheets.Add("Sheet" + count); //create new worksheet
            //            ews.View.ZoomScale = 100;
            ews.View.ShowGridLines = true;
            string[] alphabets = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            for (int i = 0; i < Columns.Length; i++)
            {
                ews.Cells[alphabets[i] + "1"].Value = Columns[i];
            }
            ews.Cells[ews.Dimension.Address].AutoFitColumns();
            string FileName = "ExcelFormat-" + adtemplate.AssetType;
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;  filename=" + FileName + ".xlsx");
            byte[] File = objExcelPackage.GetAsByteArray();
            Response.BinaryWrite(File);
            Response.End();
            return null;
            //StringBuilder sb = new StringBuilder();
            //sb.Append("<table border='" + "2px" + "'b>");
            ////write column headings
            //sb.Append("<tr>");
            //for (int k = 0; k < Columns.Length; k++)
            //{
            //    sb.Append("<td><b><font face=Arial size=2>" + Columns[k] + "</font></b></td>");
            //}
            //sb.Append("</tr>");
            //sb.Append("</table>");
            //this.Response.AddHeader("Content-Disposition", "attachment;filename=ExcelFormat-" + adtemplate.AssetType + ".xls");
            //this.Response.ContentType = "application/vnd.ms-excel";
            //byte[] buffer = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
            //return File(buffer, "application/vnd.ms-excel");
        }
        #endregion
        #region AssetProductMaster
        public ActionResult AssetProductMaster()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrEmpty(userId)) return RedirectToAction("LogOff", "Account");
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
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }
        public ActionResult AssetProductMasterJqGrid(string ProductName, string IsActive, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (!string.IsNullOrEmpty(ProductName))
                { criteria.Add("ProductName", ProductName); }
                if (!string.IsNullOrEmpty(IsActive))
                {
                    if (IsActive == "true" || IsActive == "True")
                        criteria.Add("IsActive", true);
                    if (IsActive == "false" || IsActive == "False")
                        criteria.Add("IsActive", false);
                }
                Dictionary<long, IList<AssetProductMaster>> AssetProductMasterList = ass.GetAssetProductMasterWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (AssetProductMasterList != null && AssetProductMasterList.Count > 0)
                {
                    long totalrecords = AssetProductMasterList.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,
                        rows = (from items in AssetProductMasterList.First().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[]{
                                       items.AssetProductMasterId.ToString(),                                                                                                                     
                                       items.ProductName,                                       
                                       items.IsActive==true?"Yes":"No",
                                       items.CreatedBy!=null?us.GetUserNameByUserId(items.CreatedBy):"",
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
        public ActionResult AddAssetProductMaster(AssetProductMaster apm)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    AssetProductMaster abmdetails = ass.GetAssetProductMasterByName(apm.ProductName);
                    if (abmdetails != null)
                    {
                        var script = @"ErrMsg(""Already Exist"");";
                        return JavaScript(script);
                    }
                    apm.ProductName = apm.ProductName;
                    apm.CreatedBy = userId;
                    apm.CreatedDate = DateTime.Now;
                    apm.IsActive = true;
                    ass.CreateOrUpdateAssetProductMaster(apm);
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult EditAssetProductMaster(AssetProductMaster apm)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (apm.AssetProductMasterId > 0)
                    {
                        AssetProductMaster apmdtls = ass.GetAssetProductMasterByName(apm.ProductName);
                        if (apmdtls != null)
                        {
                            var script = @"ErrMsg(""Already Exist"");";
                            return JavaScript(script);
                        }
                        AssetProductMaster apmdetails = ass.GetAssetProductMasterById(apm.AssetProductMasterId);
                        if (apmdetails != null)
                        {
                            apmdetails.ModifiedBy = userId;
                            apmdetails.ModifiedDate = DateTime.Now;
                            apmdetails.ProductName = apm.ProductName;
                            ass.CreateOrUpdateAssetProductMaster(apmdetails);
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
        public ActionResult DeleteAssetProductMaster(string Id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    string[] arrayId = Id.Split(',');
                    long[] AssetProductMasterId = Array.ConvertAll(arrayId, Int64.Parse);
                    ass.DeleteAssetProductMaster(AssetProductMasterId);
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
        public ActionResult GetAssetProductName()
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("IsActive", true);
            Dictionary<long, IList<AssetProductMaster>> assetProductMaster = ass.GetAssetProductMasterWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
            if (assetProductMaster != null && assetProductMaster.First().Value != null && assetProductMaster.FirstOrDefault().Key > 0)
            {
                var AssetProductList = (
                         from items in assetProductMaster.First().Value
                         where items.ProductName != null
                         select new
                         {
                             Text = items.ProductName,
                             Value = items.AssetProductMasterId
                         }).Distinct().ToList().OrderBy(x => x.Text);
                return Json(AssetProductList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetAssetProductNameddl()
        {
            try
            {

                Dictionary<string, string> ProductName = new Dictionary<string, string>();

                Dictionary<string, object> criteria = new Dictionary<string, object>();

                Dictionary<long, IList<AssetProductMaster>> AssetProductMstr = ass.GetAssetProductMasterWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);

                foreach (AssetProductMaster items in AssetProductMstr.First().Value)
                {
                    ProductName.Add(items.AssetProductMasterId.ToString(), items.ProductName);
                }
                return PartialView("Dropdown", ProductName);
            }
            catch (Exception ex)
            {
                // ExceptionPolicy.HandleException(ex, "Policy");
                throw ex;
            }
        }
        #endregion
        #region AssetProductTypeMaster
        public ActionResult AssetProductTypeMaster()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrEmpty(userId)) return RedirectToAction("LogOff", "Account");
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
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }
        public ActionResult AssetProductTypeMasterJqGrid(string ProductName, string ProductType, string IsActive, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (!string.IsNullOrEmpty(ProductName))
                { criteria.Add("ProductName", ProductName); }
                if (!string.IsNullOrEmpty(ProductType))
                { criteria.Add("ProductType", ProductType); }
                if (!string.IsNullOrEmpty(IsActive))
                {
                    if (IsActive == "true" || IsActive == "True")
                        criteria.Add("IsActive", true);
                    if (IsActive == "false" || IsActive == "False")
                        criteria.Add("IsActive", false);
                }
                Dictionary<long, IList<AssetProductTypeMaster>> AssetProductTypeMasterList = ass.GetAssetProductTypeMasterWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (AssetProductTypeMasterList != null && AssetProductTypeMasterList.Count > 0)
                {
                    long totalrecords = AssetProductTypeMasterList.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,
                        rows = (from items in AssetProductTypeMasterList.First().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[]{
                                       items.AssetProductTypeMasterId.ToString(),                                                                                                                     
                                       items.AssetProductMaster.ProductName,                         
                                       items.ProductType,
                                       items.IsActive==true?"Yes":"No",
                                       items.CreatedBy!=null?us.GetUserNameByUserId(items.CreatedBy):"",
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
        public ActionResult AddAssetProductTypeMaster(AssetProductTypeMaster aptm)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    AssetProductTypeMaster aptmdetails = ass.GetAssetProductTypeMasterByNameandtype(aptm.AssetProductMaster.AssetProductMasterId, aptm.ProductType);
                    if (aptmdetails != null)
                    {
                        var script = @"ErrMsg(""Already Exist"");";
                        return JavaScript(script);
                    }
                    aptm.CreatedBy = userId;
                    aptm.CreatedDate = DateTime.Now;
                    aptm.IsActive = true;
                    ass.CreateOrUpdateAssetProductTypeMaster(aptm);
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult EditAssetProductTypeMaster(AssetProductTypeMaster aptm)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (aptm.AssetProductTypeMasterId > 0)
                    {
                        AssetProductTypeMaster aptmdtls = ass.GetAssetProductTypeMasterByNameandtype(aptm.AssetProductMaster.AssetProductMasterId, aptm.ProductType);
                        if (aptmdtls != null)
                        {
                            var script = @"ErrMsg(""Already Exist"");";
                            return JavaScript(script);
                        }
                        AssetProductTypeMaster aptmdetails = ass.GetAssetProductTypeMasterById(aptm.AssetProductTypeMasterId);
                        if (aptmdetails != null)
                        {
                            aptmdetails.ModifiedBy = userId;
                            aptmdetails.ModifiedDate = DateTime.Now;
                            aptmdetails.AssetProductMaster.AssetProductMasterId = aptm.AssetProductMaster.AssetProductMasterId;
                            aptmdetails.ProductType = aptm.ProductType;
                            ass.CreateOrUpdateAssetProductTypeMaster(aptmdetails);
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
        public ActionResult DeleteAssetProductTypeMaster(string Id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    string[] arrayId = Id.Split(',');
                    long[] AssetProductTypeMasterId = Array.ConvertAll(arrayId, Int64.Parse);
                    ass.DeleteAssetProductTypeMaster(AssetProductTypeMasterId);
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
        public ActionResult GetAssetProductTypeByAssetProductName(long ProductNameId)
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            if (ProductNameId > 0)
            {
                criteria.Add("AssetProductMaster.AssetProductMasterId", ProductNameId);
            }
            criteria.Add("IsActive", true);
            Dictionary<long, IList<AssetProductTypeMaster>> assetproducttypemaster = ass.GetAssetProductTypeMasterWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
            if (assetproducttypemaster != null && assetproducttypemaster.First().Value != null && assetproducttypemaster.FirstOrDefault().Key > 0)
            {
                var AssetModelList = (
                         from items in assetproducttypemaster.First().Value
                         where items.AssetProductMaster.AssetProductMasterId > 0 && items.ProductType != null
                         select new
                         {
                             Text = items.ProductType,
                             Value = items.AssetProductTypeMasterId
                         }).Distinct().ToList().OrderBy(x => x.Text);
                return Json(AssetModelList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region IT Accesories
        public ActionResult ITAccessories()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrEmpty(userId)) return RedirectToAction("LogOff", "Account");
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
        public ActionResult ITAccessoriesJqgrid(string CampusId, string ProductNameId, string ProductTypeId, string BrandId, string ModelId, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (!string.IsNullOrEmpty(CampusId))
                { criteria.Add("CampusMaster.FormId", Convert.ToInt64(CampusId)); }
                if (!string.IsNullOrEmpty(ProductNameId))
                { criteria.Add("AssetProductMaster.AssetProductMasterId", Convert.ToInt64(ProductNameId)); }
                if (!string.IsNullOrEmpty(ProductTypeId))
                { criteria.Add("AssetProductTypeMaster.AssetProductTypeMasterId", Convert.ToInt64(ProductTypeId)); }
                if (!string.IsNullOrEmpty(BrandId))
                { criteria.Add("ITAccessoriesBrandMaster.Id", Convert.ToInt64(BrandId)); }
                if (!string.IsNullOrEmpty(ModelId))
                { criteria.Add("ITAccessoriesModelMaster.Id", Convert.ToInt64(ModelId)); }
                Dictionary<long, IList<ITAccessories>> ITAccessoriesList = ass.GetITAccessoriesDetailsWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (ITAccessoriesList != null && ITAccessoriesList.Count > 0)
                {
                    long totalrecords = ITAccessoriesList.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,
                        rows = (from items in ITAccessoriesList.First().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[]{
                                       items.Id.ToString(), 
                                       items.CampusMaster.Name,                                                                             
                                       items.AssetProductMaster.ProductName,                         
                                       items.AssetProductTypeMaster.ProductType,
                                       items.ITAccessoriesBrandMaster.Brand,
                                       items.ITAccessoriesModelMaster.Model,
                                       items.Quantity.ToString(),
                                       items.Amount.ToString(),
                                       items.Warranty!=null?CalculateWarrantyAsYear(Convert.ToInt64(items.Warranty)): "0 Year 0 Month",
                                       items.AssetInvoiceDetails.VendorMaster.VendorName,
                                       items.AssetInvoiceDetails!=null?String.Format(@"<a style='color:#034af3;text-decoration:underline' href= '#' onclick = 'uploaddat("+"\"" + items.AssetInvoiceDetails.InvoiceDetailsId + "\"" + ")' >{0}</a>",items.AssetInvoiceDetails.DocumentName):"",
                                       items.CreatedBy!=null?us.GetUserNameByUserId(items.CreatedBy):"",
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
                    var Empty = new { rows = (new { cell = new string[] { } }) };
                    return Json(Empty, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult AddNewITAccessories()
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
                throw ex;
            }
        }
        public ActionResult AddAccessoriesDetails(ITAccessories ITaccessories, long InvoiceDetailsId, long CampusId, long ProductNameId, long ProductTypeId, long BrandId, long ModelId)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    CampusMaster CampusMaster = new CampusMaster();
                    CampusMaster.FormId = CampusId;
                    AssetProductMaster AssetProductMaster = new AssetProductMaster();
                    AssetProductMaster.AssetProductMasterId = ProductNameId;
                    AssetProductTypeMaster AssetProductTypeMaster = new AssetProductTypeMaster();
                    AssetProductTypeMaster.AssetProductTypeMasterId = ProductTypeId;
                    ITAccessoriesBrandMaster ITAccessoriesBrandMaster = new ITAccessoriesBrandMaster();
                    ITAccessoriesBrandMaster.Id = BrandId;
                    ITAccessoriesModelMaster ITAccessoriesModelMaster = new ITAccessoriesModelMaster();
                    ITAccessoriesModelMaster.Id = ModelId;
                    AssetInvoiceDetails assetinvoice = new AssetInvoiceDetails();
                    assetinvoice.InvoiceDetailsId = InvoiceDetailsId;
                    ITaccessories.AssetInvoiceDetails = assetinvoice;
                    ITaccessories.CampusMaster = CampusMaster;
                    ITaccessories.ITAccessoriesModelMaster = ITAccessoriesModelMaster;
                    ITaccessories.ITAccessoriesBrandMaster = ITAccessoriesBrandMaster;
                    ITaccessories.AssetProductTypeMaster = AssetProductTypeMaster;
                    ITaccessories.AssetProductMaster = AssetProductMaster;
                    ITaccessories.CreatedBy = userId;
                    ITaccessories.CreatedDate = DateTime.Now;
                    ass.CreateOrUpdateITAccessories(ITaccessories);
                    if (InvoiceDetailsId > 0)
                    {
                        AssetInvoiceDetails assetinvoicedetails = ass.GetAssetInvoiceDetailsByInvoiceDetailsId(InvoiceDetailsId);
                        if (assetinvoicedetails != null)
                        {
                            assetinvoicedetails.AssetCount = assetinvoicedetails.AssetCount + 1;
                            if (assetinvoicedetails.TotalAsset == assetinvoicedetails.AssetCount)
                            {
                                assetinvoicedetails.IsActive = false;
                            }
                            ass.CreateOrUpdateAssetInvoiceDetails(assetinvoicedetails);
                        }
                    }
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        #region ITAccessoriesBrandMaster
        public ActionResult ITAccessoriesBrandMaster()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrEmpty(userId)) return RedirectToAction("LogOff", "Account");
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
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }
        public ActionResult ITAccessoriesBrandMasterJqGrid(string Brand, string IsActive, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (!string.IsNullOrEmpty(Brand))
                { criteria.Add("Brand", Brand); }
                if (!string.IsNullOrEmpty(IsActive))
                {
                    if (IsActive == "true" || IsActive == "True")
                        criteria.Add("IsActive", true);
                    if (IsActive == "false" || IsActive == "False")
                        criteria.Add("IsActive", false);
                }
                Dictionary<long, IList<ITAccessoriesBrandMaster>> ITAccessoriesBrandMasterList = ass.GetITAccessoriesBrandMasterWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (ITAccessoriesBrandMasterList != null && ITAccessoriesBrandMasterList.Count > 0)
                {
                    long totalrecords = ITAccessoriesBrandMasterList.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,
                        rows = (from items in ITAccessoriesBrandMasterList.First().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[]{
                                       items.Id.ToString(),                                                                                                                     
                                       items.Brand,                                       
                                       items.IsActive==true?"Yes":"No",
                                       items.CreatedBy!=null?us.GetUserNameByUserId(items.CreatedBy):"",
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
        public ActionResult AddITAccessoriesBrandMaster(ITAccessoriesBrandMaster abm)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    ITAccessoriesBrandMaster abmdetails = ass.GetITAccessoriesBrandMasterByBrandName(abm.Brand.ToUpper());
                    if (abmdetails != null)
                    {
                        var script = @"ErrMsg(""Already Exist"");";
                        return JavaScript(script);
                    }
                    abm.Brand = abm.Brand.ToUpper();
                    abm.CreatedBy = userId;
                    abm.CreatedDate = DateTime.Now;
                    abm.IsActive = true;
                    ass.CreateOrUpdateAssetBrandMaster(abm);
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult EditITAccessoriesBrandMaster(ITAccessoriesBrandMaster abm)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (abm.Id > 0)
                    {
                        ITAccessoriesBrandMaster abmdtls = ass.GetITAccessoriesBrandMasterByBrandName(abm.Brand.ToUpper());
                        if (abmdtls != null)
                        {
                            var script = @"ErrMsg(""Already Exist"");";
                            return JavaScript(script);
                        }
                        ITAccessoriesBrandMaster abmdetails = ass.GetITAccessoriesBrandMasterById(abm.Id);
                        if (abmdetails != null)
                        {
                            abmdetails.ModifiedBy = userId;
                            abmdetails.ModifiedDate = DateTime.Now;
                            abmdetails.Brand = abm.Brand.ToUpper();
                            ass.CreateOrUpdateAssetBrandMaster(abmdetails);
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
        public ActionResult DeleteITAccessoriesBrandMaster(string Id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    string[] arrayId = Id.Split(',');
                    long[] BrandMasterId = Array.ConvertAll(arrayId, Int64.Parse);
                    ass.DeleteITAccessoriesBrandMaster(BrandMasterId);
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
        public ActionResult GetITAccessoriesBrandName()
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("IsActive", true);
            Dictionary<long, IList<ITAccessoriesBrandMaster>> ITAccessoriesBrandMaster = ass.GetITAccessoriesBrandMasterWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
            if (ITAccessoriesBrandMaster != null && ITAccessoriesBrandMaster.First().Value != null && ITAccessoriesBrandMaster.FirstOrDefault().Key > 0)
            {
                var BrandList = (
                         from items in ITAccessoriesBrandMaster.First().Value
                         where items.Brand != null
                         select new
                         {
                             Text = items.Brand,
                             Value = items.Id
                         }).Distinct().ToList().OrderBy(x => x.Text);
                return Json(BrandList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetITAccessoriesBrandNameddl()
        {
            try
            {

                Dictionary<string, string> Brand = new Dictionary<string, string>();

                Dictionary<string, object> criteria = new Dictionary<string, object>();

                Dictionary<long, IList<ITAccessoriesBrandMaster>> ITAccessoriesBrandMaster = ass.GetITAccessoriesBrandMasterWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);

                foreach (ITAccessoriesBrandMaster brand in ITAccessoriesBrandMaster.First().Value)
                {
                    Brand.Add(brand.Id.ToString(), brand.Brand);
                }
                return PartialView("Dropdown", Brand);
            }
            catch (Exception ex)
            {
                // ExceptionPolicy.HandleException(ex, "Policy");
                throw ex;
            }
        }
        #endregion
        #region ITAccessoriesModelMaster
        public ActionResult ITAccessoriesModelMaster()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrEmpty(userId)) return RedirectToAction("LogOff", "Account");
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
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }
        public ActionResult ITAccessoriesModelMasterJqGrid(string Brand, string Model, string IsActive, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (!string.IsNullOrEmpty(Brand))
                { criteria.Add("Brand", Brand); }
                if (!string.IsNullOrEmpty(Model))
                { criteria.Add("Model", Model); }
                if (!string.IsNullOrEmpty(IsActive))
                {
                    if (IsActive == "true" || IsActive == "True")
                        criteria.Add("IsActive", true);
                    if (IsActive == "false" || IsActive == "False")
                        criteria.Add("IsActive", false);
                }
                Dictionary<long, IList<ITAccessoriesModelMaster>> ITAccessoriesModelMasterList = ass.GetITAccessoriesModelMasterWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (ITAccessoriesModelMasterList != null && ITAccessoriesModelMasterList.Count > 0)
                {
                    long totalrecords = ITAccessoriesModelMasterList.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,
                        rows = (from items in ITAccessoriesModelMasterList.First().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[]{
                                       items.Id.ToString(),                                                                                                                     
                                       items.ITAccessoriesBrandMaster.Brand,                         
                                       items.Model,
                                       items.IsActive==true?"Yes":"No",
                                       items.CreatedBy!=null?us.GetUserNameByUserId(items.CreatedBy):"",
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
        public ActionResult AddITAccessoriesModelMaster(ITAccessoriesModelMaster amm)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    ITAccessoriesModelMaster ammdetails = ass.GetITAccessoriesModelMasterByBrandandModel(amm.ITAccessoriesBrandMaster.Id, amm.Model.ToUpper());
                    if (ammdetails != null)
                    {
                        var script = @"ErrMsg(""Already Exist"");";
                        return JavaScript(script);
                    }
                    amm.ITAccessoriesBrandMaster.Id = amm.ITAccessoriesBrandMaster.Id;
                    amm.Model = amm.Model.ToUpper();
                    amm.CreatedBy = userId;
                    amm.CreatedDate = DateTime.Now;
                    amm.IsActive = true;
                    ass.CreateOrUpdateITAccessoriesModelMaster(amm);
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult EditITAccessoriesModelMaster(ITAccessoriesModelMaster amm)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (amm.Id > 0)
                    {
                        ITAccessoriesModelMaster ammdtls = ass.GetITAccessoriesModelMasterByBrandandModel(amm.ITAccessoriesBrandMaster.Id, amm.Model.ToUpper());
                        if (ammdtls != null)
                        {
                            var script = @"ErrMsg(""Already Exist"");";
                            return JavaScript(script);
                        }
                        ITAccessoriesModelMaster ammdetails = ass.GetITAccessoriesModelMasterById(amm.Id);
                        if (ammdetails != null)
                        {
                            ammdetails.ModifiedBy = userId;
                            ammdetails.ModifiedDate = DateTime.Now;
                            ammdetails.ITAccessoriesBrandMaster.Id = amm.ITAccessoriesBrandMaster.Id;
                            ammdetails.Model = amm.Model.ToUpper();
                            ass.CreateOrUpdateITAccessoriesModelMaster(ammdetails);
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
        public ActionResult DeleteITAccessoriesModelMaster(string Id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    string[] arrayId = Id.Split(',');
                    long[] ModelMasterId = Array.ConvertAll(arrayId, Int64.Parse);
                    ass.DeleteITAccessoriesModelMaster(ModelMasterId);
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
        public ActionResult GetITAccessoriesModelByBrand(long BrandId)
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            if (BrandId > 0)
            {
                criteria.Add("ITAccessoriesBrandMaster.Id", BrandId);
            }
            criteria.Add("IsActive", true);
            Dictionary<long, IList<ITAccessoriesModelMaster>> ITAccessoriesModelMaster = ass.GetITAccessoriesModelMasterWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
            if (ITAccessoriesModelMaster != null && ITAccessoriesModelMaster.First().Value != null && ITAccessoriesModelMaster.FirstOrDefault().Key > 0)
            {
                var AssetModelList = (
                         from items in ITAccessoriesModelMaster.First().Value
                         where items.ITAccessoriesBrandMaster.Id > 0 && items.Model != null
                         select new
                         {
                             Text = items.Model,
                             Value = items.Id
                         }).Distinct().ToList().OrderBy(x => x.Text);
                return Json(AssetModelList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        public ActionResult AddInvoiceDetails(long AssetDet_Id)
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
                    ViewBag.AssetDetailsId = AssetDet_Id;
                    return View();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult AddNewInvoiceDetails(AssetDetails assetdetails)
        {
            try
            {
                bool status=false;
                if(assetdetails != null)
                {
                    AssetDetails assetdtls = ass.GetAssetDetailsByAssetId(assetdetails.AssetDet_Id);
                    if (assetdtls.InvoiceDetailsId > 0)
                    {
                        status = false;
                    }
                    else
                    {
                        AssetInvoiceDetails assetinvoice = ass.GetAssetInvoiceDetailsByInvoiceDetailsId(assetdetails.InvoiceDetailsId);
                        assetinvoice.AssetCount = assetinvoice.AssetCount + 1;
                        if (assetinvoice.TotalAsset == assetinvoice.AssetCount)
                        {
                            assetinvoice.IsActive = false;
                        }                        
                        assetdtls.InvoiceDetailsId = assetdetails.InvoiceDetailsId;
                        assetdtls.Warranty = assetdetails.Warranty;
                        assetdtls.Amount = assetdetails.Amount;
                        ass.CreateOrUpdateITAssetDetails(assetdtls);
                        ass.CreateOrUpdateAssetInvoiceDetails(assetinvoice);
                        status = true;
                    }
                }
                return Json(status, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        //#region Students Laptop Distribution
        //public ActionResult StudentsLaptopDistribution()
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrEmpty(userId)) return RedirectToAction("LogOff", "Account");
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
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        ////public ActionResult AddSTAssetDetails(STAssetDetails assetDetails, String SpecDetails, long FormId)
        ////{
        ////    try
        ////    {
        ////        string userId = base.ValidateUser();
        ////        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        ////        else
        ////        {
        ////            if (assetDetails == null) return null;
        ////            Dictionary<string, object> criteria = new Dictionary<string, object>();
        ////            if (assetDetails.Asset_Id > 0)
        ////                criteria.Add("Asset_Id", assetDetails.Asset_Id);
        ////            if (!string.IsNullOrEmpty(assetDetails.Make))
        ////                criteria.Add("Make", assetDetails.Make);
        ////            if (!string.IsNullOrEmpty(assetDetails.Model))
        ////                criteria.Add("Model", assetDetails.Model);
        ////            if (!string.IsNullOrEmpty(assetDetails.SerialNo))
        ////                criteria.Add("SerialNo", assetDetails.SerialNo);
        ////            Dictionary<long, IList<STAssetDetails>> AssetList = ass.GetSTAssetDetailsWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
        ////            if (AssetList == null || AssetList.FirstOrDefault().Key == 0 || assetDetails.SerialNo == null || assetDetails.SerialNo == "")
        ////            {
        ////                //Get AssetCode
        ////                criteria.Clear();
        ////                if (assetDetails.Asset_Id > 0)
        ////                    criteria.Add("Asset_Id", assetDetails.Asset_Id);
        ////                Dictionary<long, IList<STAssetDetails>> AssetCont = ass.GetSTAssetDetailsWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
        ////                var Assetcount = (from u in AssetCont.FirstOrDefault().Value select u).Distinct().ToList();
        ////                long count = Assetcount.Count;
        ////                AssetDetailsTemplate assetDetailsObj = ass.GetAssetDetailsTemplateByAssetId(assetDetails.Asset_Id);
        ////                assetDetails.AssetCode = "STU-" + assetDetailsObj.AssetCode + "-" + (count + 1).ToString();
        ////                CampusMaster campusmaster = new CampusMaster();
        ////                campusmaster.FormId = FormId;
        ////                assetDetails.CampusMaster = campusmaster;
        ////                assetDetails.Make = assetDetails.Make.ToUpper();
        ////                assetDetails.Model = assetDetails.Model.ToUpper();
        ////                assetDetails.Location = assetDetails.Location;
        ////                assetDetails.FromBlock = assetDetails.FromBlock;
        ////                assetDetails.TransactionType = "";
        ////                assetDetails.TransactionType = "Stock";
        ////                if (assetDetailsObj.IsSubAsset == true)
        ////                {
        ////                    assetDetails.SubAssetType = "External";
        ////                    assetDetails.IsSubAsset = true;
        ////                }
        ////                assetDetails.EngineerName = userId;
        ////                assetDetails.IsActive = true;
        ////                assetDetails.IsStandBy = false;
        ////                assetDetails.IsStandBy = true;
        ////                assetDetails.CreatedBy = userId;
        ////                assetDetails.CreatedDate = DateTime.Now;
        ////                ass.CreateOrUpdateSTAssetDetails(assetDetails);

        ////                #region commented for IT Asset
        ////                if (assetDetails.InvoiceDetailsId > 0)
        ////                {
        ////                    AssetInvoiceDetails assetinvoicedetails = ass.GetAssetInvoiceDetailsByInvoiceDetailsId(assetDetails.InvoiceDetailsId);
        ////                    if (assetinvoicedetails != null)
        ////                    {
        ////                        assetinvoicedetails.AssetCount = assetinvoicedetails.AssetCount + 1;
        ////                        if (assetinvoicedetails.TotalAsset == assetinvoicedetails.AssetCount)
        ////                        {
        ////                            assetinvoicedetails.IsActive = false;
        ////                        }
        ////                        ass.CreateOrUpdateAssetInvoiceDetails(assetinvoicedetails);
        ////                    }
        ////                }
        ////                #endregion
        ////                if (assetDetails.AssetDet_Id > 0)
        ////                {
        ////                    string[] specList = assetDetailsObj.Specifications.Split(',').ToArray();
        ////                    string[] specValues = SpecDetails.Split(',').ToArray();

        ////                    List<string> specDetailsList = new List<string>();

        ////                    Dictionary<string, object> spec = new Dictionary<string, object>();


        ////                    for (int i = 0; i < specList.Length; i++)
        ////                    {
        ////                        if (specList[i] == "Id")
        ////                            spec.Add(specList[i], assetDetails.AssetDet_Id);
        ////                        else
        ////                            spec.Add(specList[i], specValues[i]);
        ////                    }
        ////                    JavaScriptSerializer serializer = new JavaScriptSerializer();
        ////                    assetDetails.SpecificationsDetails = serializer.Serialize(spec);

        ////                    CampusMaster campus = ass.GetAssetDetailsTemplateByFormId(assetDetails.CampusMaster.FormId);
        ////                    assetDetails.FromCampus = campus.Name;
        ////                    if (assetDetails.IsSubAsset == false)
        ////                    {
        ////                        assetDetails.CurrentCampus = campus.Name;
        ////                        assetDetails.CurrentLocation = assetDetails.Location;
        ////                        assetDetails.CurrentBlock = assetDetails.FromBlock;
        ////                    }
        ////                    ass.CreateOrUpdateSTAssetDetails(assetDetails);
        ////                    STAssetDetailsTransactionHistory transactionhistory = new STAssetDetailsTransactionHistory();
        ////                    transactionhistory.STAssetDetails = assetDetails;
        ////                    transactionhistory.FromCampus = assetDetails.FromCampus;
        ////                    transactionhistory.FromBlock = assetDetails.FromBlock;
        ////                    transactionhistory.FromLocation = assetDetails.Location;
        ////                    if (assetDetails.IsSubAsset == false)
        ////                    {
        ////                        transactionhistory.ToCampus = assetDetails.CurrentCampus;
        ////                        transactionhistory.ToLocation = assetDetails.CurrentLocation;
        ////                        transactionhistory.ToBlock = assetDetails.CurrentBlock;
        ////                    }
        ////                    transactionhistory.IdNum = assetDetails.IdNum;
        ////                    transactionhistory.CreatedBy = userId;
        ////                    transactionhistory.CreatedDate = DateTime.Now;
        ////                    transactionhistory.Amount = assetDetails.Amount;
        ////                    transactionhistory.InvoiceDetailsId = assetDetails.InvoiceDetailsId;
        ////                    transactionhistory.Warranty = assetDetails.Warranty;
        ////                    transactionhistory.IsSubAsset = assetDetails.IsSubAsset;
        ////                    transactionhistory.AssetRefId = assetDetails.AssetRefId;
        ////                    transactionhistory.TransactionType = "Stock";
        ////                    transactionhistory.TransactionType_Id = assetDetails.AssetDet_Id;
        ////                    ass.CreateOrUpdateSTAssetDetailsHistory(transactionhistory);
        ////                }
        ////                bool Flag = true;
        ////                return Json(Flag, JsonRequestBehavior.AllowGet);
        ////            }
        ////            else
        ////            {
        ////                bool Flag = false;
        ////                return Json(Flag, JsonRequestBehavior.AllowGet);
        ////            }

        ////        }
        ////    }
        ////    catch (Exception ex)
        ////    {

        ////        throw ex;
        ////    }
        ////}
        ////public ActionResult AddNewDistribution(long AssetId)
        ////{
        ////    try
        ////    {
        ////        string userId = base.ValidateUser();
        ////        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        ////        else
        ////        {
        ////            AssetDetailsTemplate assetDetailsObj = ass.GetAssetDetailsTemplateByAssetId(AssetId);
        ////            List<string> specList = assetDetailsObj.Specifications.Split(',').ToList();
        ////            List<string> DescList = assetDetailsObj.SpecificationsDetails.Split(',').ToList();
        ////            assetDetailsObj.specList = specList;
        ////            ViewBag.DescList = DescList;
        ////            ViewBag.specList = specList;
        ////            ViewBag.IsSubAsset = assetDetailsObj.IsSubAsset;
        ////            return View(assetDetailsObj);
        ////        }
        ////    }
        ////    catch (Exception ex)
        ////    {

        ////        throw ex;
        ////    }
        ////}
        //public ActionResult StudentsLaptopDistributionjqGrid(STAssetDetails assetdetails, long? FormId, long? AssetRefId, bool IsSubAsset, int rows, string sidx, string sord, int? page = 1)
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //        else
        //        {
        //            sord = sord == "desc" ? "Desc" : "Asc";
        //            if (sidx == "FormId") sidx = "CampusMaster.FormId";
        //            Dictionary<string, object> criteria = new Dictionary<string, object>();
        //            Dictionary<string, object> likecriteria = new Dictionary<string, object>();
        //            criteria.Clear();
        //            if (assetdetails != null)
        //            {

        //                if (!string.IsNullOrEmpty(assetdetails.AssetCode))
        //                    likecriteria.Add("AssetCode", assetdetails.AssetCode);
        //                if (!string.IsNullOrEmpty(assetdetails.Make))
        //                    likecriteria.Add("Make", assetdetails.Make);
        //                if (!string.IsNullOrEmpty(assetdetails.Model))
        //                    likecriteria.Add("Model", assetdetails.Model);
        //                if (!string.IsNullOrEmpty(assetdetails.SerialNo))
        //                    likecriteria.Add("SerialNo", assetdetails.SerialNo);
        //                if (!string.IsNullOrEmpty(assetdetails.CurrentBlock))
        //                    criteria.Add("CurrentBlock", assetdetails.CurrentBlock);
        //                if (!string.IsNullOrEmpty(assetdetails.CurrentLocation))
        //                    criteria.Add("CurrentLocation", assetdetails.CurrentLocation);
        //                if (!string.IsNullOrEmpty(assetdetails.TransactionType))
        //                    criteria.Add("TransactionType", assetdetails.TransactionType);
        //            }
        //            if (FormId > 0)
        //            {
        //                likecriteria.Add("CampusMaster.FormId", FormId);
        //            }
        //            criteria.Add("IsSubAsset", IsSubAsset);
        //            if (AssetRefId != null)
        //            {
        //                criteria.Add("AssetRefId", AssetRefId);
        //            }
        //            if (!string.IsNullOrEmpty(assetdetails.AssetType))
        //                criteria.Add("Asset_Id", Convert.ToInt64(assetdetails.AssetType));
        //            Dictionary<long, IList<STAssetDetails>> AssetDetailsList = ass.GetSTAssetDetailsWithPagingAndExactAndLikeCriteria(page - 1, rows, sidx, sord, criteria, likecriteria);
        //            if (AssetDetailsList != null && AssetDetailsList.Count > 0)
        //            {
        //                StaffManagementService sms = new StaffManagementService();
        //                AdmissionManagementService abc = new AdmissionManagementService();
        //                long totalrecords = AssetDetailsList.First().Key;
        //                int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
        //                var jsondat = new
        //                {
        //                    total = totalPages,
        //                    page = page,
        //                    records = totalrecords,
        //                    rows = (from items in AssetDetailsList.First().Value
        //                            select new
        //                            {
        //                                i = 2,
        //                                cell = new string[]{
        //                               items.AssetDet_Id.ToString(),                                       
        //                               items.AssetCode,
        //                               items.Asset_Id.ToString(),
        //                               items.AssetType,
        //                               items.Make,
        //                               items.Model,
        //                               items.SerialNo,
        //                               items.TransactionType,                                       
        //                               items.CurrentCampus,
        //                               items.CurrentBlock,
        //                               items.CurrentLocation,                                                                              
        //                               //items.UserType.Contains("Staff")?sms.GetStaffNameByPreRegNum(items.PreRegNum):items.UserType.Contains("Student")?abc.GetStudentNameByPreRegNum(items.PreRegNum):items.UserType.Contains("Common")?"":"",                                       
        //                               items.StudentTemplateView!=null?items.StudentTemplateView.Name:"",                                       
        //                               items.CreatedBy,
        //                               items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy"):"",
        //                               items.ModifiedBy,
        //                               items.ModifiedDate!=null?items.ModifiedDate.Value.ToString("dd/MM/yyyy"):"",
        //                               }
        //                            })

        //                };
        //                return Json(jsondat, JsonRequestBehavior.AllowGet);
        //            }
        //            else
        //            {
        //                var Empty = new { rows = (new { cell = new string[] { } }) };
        //                return Json(Empty, JsonRequestBehavior.AllowGet);
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
        //        throw ex;
        //    }
        //}
        //#endregion
   
        ////--Krishna,09062017
        //#region LaptopDistribution

        //public ActionResult LaptopDistribution()
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //        else
        //        {
        //            DateTime dttime = DateTime.Now;
        //            ViewBag.acadddl = GetAcademicYear();
        //            ViewBag.dattime = dttime.ToString("dd/MM/yyyy");

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
        //        ExceptionPolicy.HandleException(ex, "AttendancePolicy");
        //        throw ex;
        //    }

        //}

        //public ActionResult LaptopDistributionJqGrid(AssetDistributionStudent_vw adv, int rows, string sidx, string sord, int? page = 1)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(adv.AcademicYear) && string.IsNullOrEmpty(adv.Campus) && string.IsNullOrEmpty(adv.Grade) && (string.IsNullOrEmpty(adv.Section)))
        //        {
        //            var Empty = new { rows = (new { cell = new string[] { } }) };
        //            return Json(Empty, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            //Criteria passing taken in this Dictionary
        //            Dictionary<string, object> criteria = new Dictionary<string, object>();
        //            sord = sord == "desc" ? "Desc" : "Asc";

        //            //Search Process Add.DBColumnName,jqueryColumnName
        //            criteria.Add("AcademicYear", adv.AcademicYear);
        //            criteria.Add("Campus", adv.Campus);
        //            criteria.Add("Grade", adv.Grade);
        //            criteria.Add("Section", adv.Section);

        //            Dictionary<long, IList<AssetDistributionStudent_vw>> STAssetDetailsList = ass.GetStudentListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);

        //            if (STAssetDetailsList != null && STAssetDetailsList.FirstOrDefault().Key > 0)
        //            {
        //                long totalrecords = STAssetDetailsList.First().Key;
        //                int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
        //                var jsondat = new
        //                {
        //                    total = totalPages,
        //                    page = page,
        //                    records = totalrecords,

        //                    rows = (from items in STAssetDetailsList.First().Value
        //                            select new
        //                            {
        //                                i = 2,
        //                                cell = new string[]{
        //                               items.Id.ToString(),  
        //                               items.NewId,
        //                               items.Name, 
        //                               items.Campus, 
        //                               items.Grade,
        //                               items.Section
        //                               }
        //                            })

        //                };
        //                return Json(jsondat, JsonRequestBehavior.AllowGet);
        //            }
        //            else
        //            {
        //                var Empty = new { rows = (new { cell = new string[] { } }) };
        //                return Json(Empty, JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public ActionResult LaptopDistributionProcess(string sId, string AcademicYear, string Campus, string Grade, string Section)
        //{
        //    string[] str = null;
        //    long studentcount = 0;
        //    long stockcount = 0;
        //    string userId = base.ValidateUser();
        //    IList<STAssetDetails> stdetailslist = new List<STAssetDetails>();
        //    IList<STAssetDetailsTransactionHistory> stdetailslist1 = new List<STAssetDetailsTransactionHistory>();
        //    if (sId != null)
        //    {
        //        str = sId.Split(',');
        //        studentcount = str.Length;
        //        //-Checking Asset for Availability for distribution
        //        Dictionary<string, object> criteria = new Dictionary<string, object>();
        //        criteria.Add("TransactionType", "Stock");
        //        criteria.Add("AssetType", "LAPTOP");
        //        criteria.Add("IsActive", true);
        //        Dictionary<long, IList<STAssetDetails>> AssetAvailableList = ass.GetSTAssetDetailsWithPagingAndCriteria(0, 99999, "AssetCode", "Asc", criteria);                
        //        if (AssetAvailableList.FirstOrDefault().Key > 0)
        //            stockcount = AssetAvailableList.FirstOrDefault().Value.Count;
        //        else
        //            return Json("failed", JsonRequestBehavior.AllowGet);

        //        // --- Update in Asset Table
        //        if (studentcount <= stockcount)
        //        {
        //            for (int i = 0; i < str.Length; i++)
        //            {
        //                STAssetDetails stassetobj = ass.GetSTAssetDetailsByAssetId(AssetAvailableList.FirstOrDefault().Value[i].AssetDet_Id);
        //                stassetobj.IdNum = Convert.ToInt64(str[i]);
        //                stassetobj.TransactionType = "Distributed";
        //                stassetobj.ReceivedAcademicYr = AcademicYear;
        //                stdetailslist.Add(stassetobj);

        //                //--- Adding data in Asset Transaction History Table
        //                STAssetDetailsTransactionHistory transactionhistory = new STAssetDetailsTransactionHistory(); //-- Insert in Asset Table
        //                transactionhistory.STAssetDetails = stassetobj;
        //                transactionhistory.FromCampus = stassetobj.FromCampus;
        //                transactionhistory.FromBlock = stassetobj.FromBlock;
        //                transactionhistory.IdNum = stassetobj.IdNum;
        //                transactionhistory.CreatedBy = userId;
        //                transactionhistory.CreatedDate = DateTime.Now;
        //                transactionhistory.TransactionType = "Distributed";
        //                transactionhistory.Amount = stassetobj.Amount;
        //                transactionhistory.InvoiceDetailsId = stassetobj.InvoiceDetailsId;
        //                transactionhistory.Warranty = stassetobj.Warranty;
        //                transactionhistory.IsSubAsset = stassetobj.IsSubAsset;
        //                transactionhistory.AssetRefId = stassetobj.AssetRefId;
        //                transactionhistory.ReceivedAcademicYr = stassetobj.ReceivedAcademicYr;
        //                stdetailslist1.Add(transactionhistory);
        //            }
        //            if (stdetailslist.Count > 0)
        //            {
        //                ass.SaveOrUpdateSTAssetDetailsList(stdetailslist); //--Update in STAssetTransactiontbl
        //                ass.SaveOrUpdateSTAssetDetailsTransactionHistoryList(stdetailslist1); //--Insert STAssetTransactionHistorytbl
        //                return Json("success", JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //        else
        //            return Json("Available stock is lesser than Selection", JsonRequestBehavior.AllowGet);
        //    }

        //    return Json("failed", JsonRequestBehavior.AllowGet);
        //}

        //#endregion

        #region StudentLaptopDistributionNew_Krishna29062017
        public ActionResult StudentLaptopDistribution()
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
        public JsonResult GetDistributionCount(string AcademicYear, string Campus, string Grade, string Section, string RollNo)
        {
            var this_name = "0";
            try
            {
                if (string.IsNullOrEmpty(AcademicYear) && string.IsNullOrEmpty(Campus))
                {
                    var Empty = new { rows = (new { cell = new string[] { } }) };
                    return Json(Empty, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<string, object> likecriteria = new Dictionary<string, object>();
                    if (!string.IsNullOrEmpty(AcademicYear))
                        criteria.Add("AcademicYear", AcademicYear);
                    if (!string.IsNullOrEmpty(Campus))
                        criteria.Add("Campus", Campus);
                    if (!string.IsNullOrEmpty(Grade))
                        criteria.Add("Grade", Grade);
                    if (!string.IsNullOrEmpty(Section))
                        criteria.Add("Section", Section);
                    criteria.Add("AdmissionStatus", "Registered");
                    criteria.Add("TransactionType", "Distributed");
                    if (!string.IsNullOrEmpty(RollNo))
                        likecriteria.Add("NewId", RollNo);
                    Dictionary<long, IList<StudentLaptopDistribution_vw>> objStudentLaptopDistribution_vw = ass.GetStudentLaptopDistributionWithPagingAndCriteria(0, 9999, "", "", criteria, likecriteria);
                    int count = objStudentLaptopDistribution_vw.FirstOrDefault().Value.Count;
                    this_name = count.ToString();
                }
            }
            catch (Exception ex) { throw ex; }
            return Json(this_name);
        }
        public ActionResult StudentLaptopDistributionJqGrid(StudentLaptopDistribution_vw adv, string SS, string OS, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                if (string.IsNullOrEmpty(adv.AcademicYear) && string.IsNullOrEmpty(adv.Campus) && string.IsNullOrEmpty(adv.Grade) && (string.IsNullOrEmpty(adv.Section)))
                {
                    var Empty = new { rows = (new { cell = new string[] { } }) };
                    return Json(Empty, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<string, object> likecriteria = new Dictionary<string, object>();

                    sord = sord == "desc" ? "Desc" : "Asc";
                    if (!string.IsNullOrEmpty(adv.AcademicYear))
                        criteria.Add("AcademicYear", adv.AcademicYear); //--AcademicYear
                    if (!string.IsNullOrEmpty(adv.Campus))
                        criteria.Add("Campus", adv.Campus);//--Campus
                    if (!string.IsNullOrEmpty(adv.Grade))
                        criteria.Add("Grade", adv.Grade);//--Grade
                    if (!string.IsNullOrEmpty(adv.Section))
                        criteria.Add("Section", adv.Section);//--Section
                    criteria.Add("AdmissionStatus", "Registered");//--AdmissionStatus
                    if (!string.IsNullOrEmpty(adv.NewId))
                        likecriteria.Add("NewId", adv.NewId);//--RollNo

                    Dictionary<long, IList<StudentLaptopDistribution_vw>> STAssetDetailsList = ass.GetStudentLaptopDistributionWithPagingAndCriteria(page - 1, rows, sord, sidx, criteria, likecriteria);
                    if (STAssetDetailsList != null && STAssetDetailsList.FirstOrDefault().Key > 0)
                    {
                        long totalrecords = STAssetDetailsList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in STAssetDetailsList.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[]{
                                       items.Id.ToString(),
                                       items.studentId.ToString(),
                                       items.NewId,
                                       items.Name,
                                       items.Grade,
                                       items.TransactionType!=null?items.TransactionType:"",
                                       items.ReceivedDate!=null?items.ReceivedDate.Value.ToString("dd/MM/yyyy"):"",
                                       items.AssetCode!=null?items.AssetCode:"", 
                                       items.LTSize!=null?items.LTSize:"",
                                       items.OperatingSystemDtls!=null?items.OperatingSystemDtls:""
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult LaptopDistributionProcess(string sId, string AcademicYear, string Campus, string Grade, string Section, string ScreenSize, string OperatingSystem)
        {
            string[] str = null;
            long studentcount = 0;
            long stockcount = 0;
            string userId = base.ValidateUser();
            IList<STAssetDetails> stdetailslist = new List<STAssetDetails>();
            IList<STAssetDetailsTransactionHistory> stdetailslist1 = new List<STAssetDetailsTransactionHistory>();
            if (sId != null)
            {
                str = sId.Split(',');
                studentcount = str.Length;
                //-Checking Asset for Availability for distribution
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("TransactionType", "Stock");
                criteria.Add("AssetType", "LAPTOP");
                criteria.Add("IsActive", true);
                criteria.Add("OperatingSystemDtls", OperatingSystem);
                criteria.Add("LTSize", ScreenSize);

                Dictionary<long, IList<STAssetDetails>> AssetAvailableList = ass.GetSTAssetDetailsWithPagingAndCriteria(0, 99999, "AssetCode", "Asc", criteria);
                if (AssetAvailableList.FirstOrDefault().Key > 0)
                    stockcount = AssetAvailableList.FirstOrDefault().Value.Count;
                else
                    return Json("failed", JsonRequestBehavior.AllowGet);
                // --- Update in Asset Table
                if (studentcount <= stockcount)
                {
                    for (int i = 0; i < str.Length; i++)
                    {
                        STAssetDetails stassetobj = ass.GetSTAssetDetailsByAssetId(AssetAvailableList.FirstOrDefault().Value[i].AssetDet_Id);
                        stassetobj.IdNum = Convert.ToInt64(str[i]);
                        stassetobj.TransactionType = "Distributed";
                        stassetobj.CreatedDate = DateTime.Now;
                        stassetobj.ReceivedAcademicYr = AcademicYear;
                        stassetobj.ReceivedCampus = Campus;
                        stassetobj.ReceivedGrade = Grade;
                        stassetobj.ReceivedDate = DateTime.Now;
                        stdetailslist.Add(stassetobj);

                        //--- Adding data in Asset Transaction History Table
                        STAssetDetailsTransactionHistory transactionhistory = new STAssetDetailsTransactionHistory(); //-- Insert in Asset Table
                        transactionhistory.STAssetDetails = stassetobj;
                        transactionhistory.FromCampus = stassetobj.FromCampus;
                        transactionhistory.FromBlock = stassetobj.FromBlock;
                        transactionhistory.IdNum = stassetobj.IdNum;
                        transactionhistory.CreatedBy = userId;
                        transactionhistory.CreatedDate = DateTime.Now;
                        transactionhistory.TransactionType = "Distributed";
                        transactionhistory.Amount = stassetobj.Amount;
                        transactionhistory.InvoiceDetailsId = stassetobj.InvoiceDetailsId;
                        transactionhistory.Warranty = stassetobj.Warranty;
                        transactionhistory.IsSubAsset = stassetobj.IsSubAsset;
                        transactionhistory.AssetRefId = stassetobj.AssetRefId;
                        transactionhistory.ReceivedAcademicYr = stassetobj.ReceivedAcademicYr;
                        stdetailslist1.Add(transactionhistory);
                    }
                    if (stdetailslist.Count > 0)
                    {
                        ass.SaveOrUpdateSTAssetDetailsList(stdetailslist); //--Update in STAssetTransactiontbl
                        ass.SaveOrUpdateSTAssetDetailsTransactionHistoryList(stdetailslist1); //--Insert STAssetTransactionHistorytbl
                        return Json("success", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                    return Json("Available stock is lesser than Selection", JsonRequestBehavior.AllowGet);
            }

            return Json("failed", JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region LaptopEntry_Krishna and Prabhakar

        public ActionResult StudentsBulkAssetUpload()
        {
            try
            {
                DateTime dttime = DateTime.Now;
                ViewBag.acadddl = GetAcademicYear();
                //ViewBag.currentAcyear = DateTime.Now.Month > 5 ? DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Year + 1).ToString() : (DateTime.Now.Year - 1).ToString() + "-" + DateTime.Now.Year.ToString();
                ViewBag.dattime = dttime.ToString("dd/MM/yyyy");

                #region BreadCrumb
                string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                #endregion
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public long GeAssetID()
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("AssetType", "LAPTOP");
            Dictionary<long, IList<AssetDetailsTemplate>> assetinvoicedetails = ass.GetAssetDetailsTemplateWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
            long assetId = assetinvoicedetails.FirstOrDefault().Value[0].Asset_Id;
            return assetId;
        }

        public ActionResult LaptopGenerateExcelFormat()
        {
            string[] Columns = { "Brand", "Model", "SerialNo", "Warranty(In Months)", "Amount", "Windows OS", "Laptop Type" };

            AssetDetailsTemplate adtemplate = ass.GetAssetDetailsTemplateByAssetId(GeAssetID());

            if (adtemplate != null)
            {
                Columns = Columns.Concat(adtemplate.SpecificationsDetails.Split(',')).ToArray();
            }

            ExcelPackage objExcelPackage = new ExcelPackage();   //create new workbook
            int count = 1;
            ExcelWorksheet ews = objExcelPackage.Workbook.Worksheets.Add("Sheet" + count); //create new worksheet
            ews.View.ShowGridLines = true;
            string[] alphabets = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            for (int i = 0; i < Columns.Length; i++)
            {
                ews.Cells[alphabets[i] + "1"].Value = Columns[i];
            }
            ews.Cells[ews.Dimension.Address].AutoFitColumns();
            string FileName = "ExcelFormat-" + adtemplate.AssetType;
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;  filename=" + FileName + ".xlsx");
            byte[] File = objExcelPackage.GetAsByteArray();
            Response.BinaryWrite(File);
            Response.End();
            return null;
        }

        [HttpPost]
        public ActionResult StudentsBulkAssetUpload(HttpPostedFileBase[] uploadedFile, long FormId, bool IsSubAsset, long InvoiceDetailsId)
        {
            StringBuilder retValue = new StringBuilder();
            long lAssetID = GeAssetID();

            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    UserService us = new UserService();
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
                                string path = uploadedFile[l].InputStream.ToString();
                                byte[] imageSize = new byte[uploadedFile[l].ContentLength];
                                uploadedFile[l].InputStream.Read(imageSize, 0, (int)uploadedFile[l].ContentLength);
                                string UploadConnStr = "";
                                fileName = uploadedFile[l].FileName;
                                string fileExtn = Path.GetExtension(uploadedFile[l].FileName);
                                string fileLocation = ConfigurationManager.AppSettings["BulkUserCreationFilePath"].ToString() + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss").Replace(":", ".") + fileName;
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
                                System.Data.DataTable DtblXcelItemData = new System.Data.DataTable();
                                string QeryToGetXcelItemData = "select * from " + string.Format("{0}${1}", "[Sheet1", "A1:AZ]");
                                itemconn.ConnectionString = UploadConnStr;
                                itemconn.Open();
                                OleDbCommand cmd = new OleDbCommand(QeryToGetXcelItemData, itemconn);
                                cmd.CommandType = CommandType.Text;
                                OleDbDataAdapter DtAdptrr = new OleDbDataAdapter();
                                DtAdptrr.SelectCommand = cmd;
                                DtAdptrr.Fill(DtblXcelItemData);

                                string[] strArray = { "Brand", "Model", "SerialNo", "Warranty(In Months)", "Amount", "Windows OS", "Laptop Type" };

                                AssetDetailsTemplate adtemplate = ass.GetAssetDetailsTemplateByAssetId(lAssetID);
                                if (adtemplate != null)
                                {
                                    strArray = strArray.Concat(adtemplate.SpecificationsDetails.Split(',')).ToArray();
                                }

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
                                        IList<STAssetDetails> STAssetDetailsList = new List<STAssetDetails>();
                                        IList<STAssetDetailsTransactionHistory> STTransactionHistoryList = new List<STAssetDetailsTransactionHistory>();
                                        criteria.Clear();
                                        if (lAssetID > 0)
                                            criteria.Add("Asset_Id", lAssetID);
                                        Dictionary<long, IList<STAssetDetails>> AssetCont = ass.GetSTAssetDetailsWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                                        var Assetcount = (from u in AssetCont.FirstOrDefault().Value select u).Distinct().ToList();
                                        long count = Assetcount.Count;
                                        AssetInvoiceDetails assetinvoicedetails = ass.GetAssetInvoiceDetailsByInvoiceDetailsId(InvoiceDetailsId);
                                        long invoiceremainigcount = assetinvoicedetails.TotalAsset - assetinvoicedetails.AssetCount;
                                        long invoicecount = assetinvoicedetails.AssetCount;
                                        if (invoiceremainigcount < DtblXcelItemData.Rows.Count)
                                        {
                                            return Json(new { success = true, result = "Excel has More Assets for this Invoice", status = "failed" }, "text/html", JsonRequestBehavior.AllowGet);
                                        }
                                        foreach (DataRow item in DtblXcelItemData.Rows)
                                        {
                                            Dictionary<string, object> likecriteria = new Dictionary<string, object>();
                                            criteria.Clear();
                                            //if (!string.IsNullOrEmpty(item["Brand"] != null ? item["Brand"].ToString().Trim() : ""))
                                            //    criteria.Add("Make", item["Brand"].ToString().Trim().ToUpper());
                                            //if (!string.IsNullOrEmpty(item["Model"] != null ? item["Model"].ToString().Trim() : ""))
                                            //    criteria.Add("Model", item["Model"].ToString().Trim().ToUpper());
                                            if (lAssetID > 0)
                                                criteria.Add("Asset_Id", lAssetID);
                                            if (!string.IsNullOrEmpty(item["SerialNo"] != null ? item["SerialNo"].ToString().Trim() : ""))
                                                criteria.Add("SerialNo", item["SerialNo"].ToString().Trim());

                                            Dictionary<long, IList<STAssetDetails>> assetdetails = ass.GetSTAssetDetailsWithPagingAndExactAndLikeCriteria(0, 9999, "AssetDet_Id", "Asc", criteria, likecriteria);
                                            if (assetdetails == null || assetdetails.FirstOrDefault().Key == 0)
                                            {
                                                STAssetDetails assetdtls = new STAssetDetails();
                                                assetdtls.Asset_Id = adtemplate.Asset_Id;
                                                assetdtls.AssetType = adtemplate.AssetType;
                                                assetdtls.Model = item["Model"] != null ? item["Model"].ToString().Trim().ToUpper() : "";
                                                assetdtls.Make = item["Brand"] != null ? item["Brand"].ToString().Trim().ToUpper() : "";
                                                assetdtls.SerialNo = item["SerialNo"] != null ? item["SerialNo"].ToString().Trim() : "";
                                                assetdtls.AssetCode = "STU-" + adtemplate.AssetCode + "-" + (count + 1).ToString();
                                                MastersService ms = new MastersService();
                                                CampusMaster cm = ms.GetCampusById(FormId);
                                                assetdtls.CurrentCampus = cm.Name;
                                                assetdtls.FromCampus = cm.Name;
                                                assetdtls.CampusMaster = cm;
                                                assetdtls.CreatedDate = DateTime.Now;
                                                assetdtls.CreatedBy = userId;
                                                assetdtls.TransactionType = "Stock";
                                                assetdtls.Location = "Stock";
                                                assetdtls.CurrentBlock = "Stock";
                                                assetdtls.CurrentLocation = "Stock";
                                                assetdtls.FromBlock = "Stock";
                                                assetdtls.IsStandBy = true;
                                                assetdtls.IsActive = true;
                                                assetdtls.Amount = item["Amount"] != null ? Convert.ToDecimal(item["Amount"]) : 0;
                                                assetdtls.Warranty = item["Warranty(In Months)"] != null ? item["Warranty(In Months)"].ToString().Trim() : "";
                                                assetdtls.EngineerName = userId;
                                                assetdtls.OperatingSystemDtls = item["Windows OS"] != null ? item["Windows OS"].ToString().Trim() : "";
                                                assetdtls.LTSize = item["Laptop Type"] != null ? item["Laptop Type"].ToString().Trim() : "";

                                                if (IsSubAsset == true)
                                                {
                                                    assetdtls.SubAssetType = "External";
                                                    assetdtls.IsSubAsset = true;
                                                }
                                                string[] specs = adtemplate.SpecificationsDetails.Split(',').ToArray();

                                                for (int k = 0; k < specs.Length; k++)
                                                {
                                                    item[specs[k]] = item[specs[k]] != null ? item[specs[k]].ToString().Trim() : "";
                                                    assetdtls.SpecificationsDetails += "," + item[specs[k]];
                                                }
                                                assetdtls.InvoiceDetailsId = assetinvoicedetails.InvoiceDetailsId;
                                                STAssetDetailsList.Add(assetdtls);
                                                invoicecount = invoicecount + 1;

                                                if (assetinvoicedetails.TotalAsset == invoicecount)
                                                {
                                                    assetinvoicedetails.IsActive = false;
                                                }
                                                count = count + 1;
                                            }
                                        }
                                        if (STAssetDetailsList.Count > 0)
                                        {
                                            ass.SaveOrUpdateSTAssetDetailsList(STAssetDetailsList);
                                            assetinvoicedetails.AssetCount = invoicecount;
                                            ass.CreateOrUpdateAssetInvoiceDetails(assetinvoicedetails);
                                            foreach (var item in STAssetDetailsList)
                                            {
                                                string[] specList = adtemplate.Specifications.Split(',').ToArray();
                                                string[] specValues = item.SpecificationsDetails.Split(',').ToArray();

                                                List<string> specDetailsList = new List<string>();

                                                Dictionary<string, object> spec = new Dictionary<string, object>();
                                                for (int i = 0; i < specList.Length; i++)
                                                {
                                                    if (specList[i] == "Id")
                                                        spec.Add(specList[i], item.AssetDet_Id);
                                                    else
                                                        spec.Add(specList[i], specValues[i]);
                                                }
                                                JavaScriptSerializer serializer = new JavaScriptSerializer();
                                                item.SpecificationsDetails = serializer.Serialize(spec);
                                                STAssetDetailsTransactionHistory transactionhistory = new STAssetDetailsTransactionHistory();
                                                transactionhistory.STAssetDetails = item;
                                                transactionhistory.FromCampus = item.FromCampus;
                                                transactionhistory.FromBlock = item.FromBlock;
                                                transactionhistory.FromLocation = item.Location;
                                                transactionhistory.ToCampus = item.CurrentCampus;
                                                transactionhistory.ToLocation = item.CurrentLocation;
                                                transactionhistory.ToBlock = item.CurrentBlock;
                                                //transactionhistory.UserType = item.UserType;
                                                transactionhistory.CreatedBy = userId;
                                                transactionhistory.CreatedDate = DateTime.Now;
                                                transactionhistory.TransactionType = "Stock";
                                                transactionhistory.TransactionType_Id = item.AssetDet_Id;
                                                transactionhistory.InvoiceDetailsId = item.InvoiceDetailsId;
                                                transactionhistory.Warranty = item.Warranty;
                                                transactionhistory.Amount = item.Amount;
                                                transactionhistory.IsSubAsset = item.IsSubAsset;
                                                transactionhistory.AssetRefId = item.AssetRefId;
                                                STTransactionHistoryList.Add(transactionhistory);
                                            }
                                            ass.SaveOrUpdateSTAssetDetailsList(STAssetDetailsList);
                                            ass.SaveOrUpdateSTAssetDetailsTransactionHistoryList(STTransactionHistoryList);
                                            return Json(new { success = true, result = "Asset created successfully..", status = "success" }, "text/html", JsonRequestBehavior.AllowGet);
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
                                ErrorLogs el = new ErrorLogs();
                                el.ExceptionErrorLog = ex.ToString();
                                ass.CreateOrUpdateErrorLogs(el);
                                ErrorFilename.Append(fileName + ",");
                                ExceptionPolicy.HandleException(ex, "CustomAccountPolicy");
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
                //ExceptionPolicy.HandleException(ex, "CustomAccountPolicy");
            }
            //MyLabel.Text = sb.ToString().Replace(Environment.NewLine, "<br />");
            return Json(new { success = true, result = retValue.ToString().Replace(Environment.NewLine, "<br />") }, "text/html", JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddNewDistribution()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    AssetDetailsTemplate assetDetailsObj = ass.GetAssetDetailsTemplateByAssetId(GeAssetID());
                    List<string> specList = assetDetailsObj.Specifications.Split(',').ToList();
                    List<string> DescList = assetDetailsObj.SpecificationsDetails.Split(',').ToList();
                    assetDetailsObj.specList = specList;
                    ViewBag.DescList = DescList;
                    ViewBag.specList = specList;
                    ViewBag.IsSubAsset = assetDetailsObj.IsSubAsset;
                    return View(assetDetailsObj);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ActionResult AddSTAssetDetails(STAssetDetails assetDetails, String SpecDetails, long FormId)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (assetDetails == null) return null;
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    if (assetDetails.Asset_Id > 0)
                        criteria.Add("Asset_Id", GeAssetID()); //---Get Asset ID
                    if (!string.IsNullOrEmpty(assetDetails.SerialNo))
                        criteria.Add("SerialNo", assetDetails.SerialNo);
                    Dictionary<long, IList<STAssetDetails>> AssetList = ass.GetSTAssetDetailsWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                    if (AssetList == null || AssetList.FirstOrDefault().Key == 0 || assetDetails.SerialNo == null || assetDetails.SerialNo == "")
                    {
                        //Get AssetCode
                        criteria.Clear();
                        if (assetDetails.Asset_Id > 0)
                            criteria.Add("Asset_Id", assetDetails.Asset_Id);
                        Dictionary<long, IList<STAssetDetails>> AssetCont = ass.GetSTAssetDetailsWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                        var Assetcount = (from u in AssetCont.FirstOrDefault().Value select u).Distinct().ToList();
                        long count = Assetcount.Count;
                        AssetDetailsTemplate assetDetailsObj = ass.GetAssetDetailsTemplateByAssetId(assetDetails.Asset_Id);
                        assetDetails.AssetCode = "STU-" + assetDetailsObj.AssetCode + "-" + (count + 1).ToString();
                        CampusMaster campusmaster = new CampusMaster();
                        campusmaster.FormId = FormId;
                        assetDetails.CampusMaster = campusmaster;
                        assetDetails.Make = assetDetails.Make.ToUpper();
                        assetDetails.Model = assetDetails.Model.ToUpper();
                        assetDetails.Location = assetDetails.Location;
                        assetDetails.FromBlock = assetDetails.FromBlock;
                        assetDetails.TransactionType = "";
                        assetDetails.TransactionType = "Stock";
                        if (assetDetailsObj.IsSubAsset == true)
                        {
                            assetDetails.SubAssetType = "External";
                            assetDetails.IsSubAsset = true;
                        }
                        assetDetails.EngineerName = userId;
                        assetDetails.IsActive = true;
                        assetDetails.IsStandBy = false;
                        assetDetails.IsStandBy = true;
                        assetDetails.CreatedBy = userId;
                        assetDetails.CreatedDate = DateTime.Now;
                        assetDetails.OperatingSystemDtls = assetDetails.OperatingSystemDtls;
                        assetDetails.LTSize = assetDetails.LTSize;



                        ass.CreateOrUpdateSTAssetDetails(assetDetails);

                        #region commented for IT Asset
                        if (assetDetails.InvoiceDetailsId > 0)
                        {
                            AssetInvoiceDetails assetinvoicedetails = ass.GetAssetInvoiceDetailsByInvoiceDetailsId(assetDetails.InvoiceDetailsId);
                            if (assetinvoicedetails != null)
                            {
                                assetinvoicedetails.AssetCount = assetinvoicedetails.AssetCount + 1;
                                if (assetinvoicedetails.TotalAsset == assetinvoicedetails.AssetCount)
                                {
                                    assetinvoicedetails.IsActive = false;
                                }
                                ass.CreateOrUpdateAssetInvoiceDetails(assetinvoicedetails);
                            }
                        }
                        #endregion
                        if (assetDetails.AssetDet_Id > 0)
                        {
                            string[] specList = assetDetailsObj.Specifications.Split(',').ToArray();
                            string[] specValues = SpecDetails.Split(',').ToArray();

                            List<string> specDetailsList = new List<string>();

                            Dictionary<string, object> spec = new Dictionary<string, object>();


                            for (int i = 0; i < specList.Length; i++)
                            {
                                if (specList[i] == "Id")
                                    spec.Add(specList[i], assetDetails.AssetDet_Id);
                                else
                                    spec.Add(specList[i], specValues[i]);
                            }
                            JavaScriptSerializer serializer = new JavaScriptSerializer();
                            assetDetails.SpecificationsDetails = serializer.Serialize(spec);

                            CampusMaster campus = ass.GetAssetDetailsTemplateByFormId(assetDetails.CampusMaster.FormId);
                            assetDetails.FromCampus = campus.Name;
                            if (assetDetails.IsSubAsset == false)
                            {
                                assetDetails.CurrentCampus = campus.Name;
                                assetDetails.CurrentLocation = assetDetails.Location;
                                assetDetails.CurrentBlock = assetDetails.FromBlock;
                            }
                            ass.CreateOrUpdateSTAssetDetails(assetDetails);
                            STAssetDetailsTransactionHistory transactionhistory = new STAssetDetailsTransactionHistory();
                            transactionhistory.STAssetDetails = assetDetails;
                            transactionhistory.FromCampus = assetDetails.FromCampus;
                            transactionhistory.FromBlock = assetDetails.FromBlock;
                            transactionhistory.FromLocation = assetDetails.Location;
                            if (assetDetails.IsSubAsset == false)
                            {
                                transactionhistory.ToCampus = assetDetails.CurrentCampus;
                                transactionhistory.ToLocation = assetDetails.CurrentLocation;
                                transactionhistory.ToBlock = assetDetails.CurrentBlock;
                            }
                            transactionhistory.IdNum = assetDetails.IdNum;
                            transactionhistory.CreatedBy = userId;
                            transactionhistory.CreatedDate = DateTime.Now;
                            transactionhistory.Amount = assetDetails.Amount;
                            transactionhistory.InvoiceDetailsId = assetDetails.InvoiceDetailsId;
                            transactionhistory.Warranty = assetDetails.Warranty;
                            transactionhistory.IsSubAsset = assetDetails.IsSubAsset;
                            transactionhistory.AssetRefId = assetDetails.AssetRefId;
                            transactionhistory.TransactionType = "Stock";
                            transactionhistory.TransactionType_Id = assetDetails.AssetDet_Id;
                            ass.CreateOrUpdateSTAssetDetailsHistory(transactionhistory);
                        }
                        bool Flag = true;
                        return Json(Flag, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        bool Flag = false;
                        return Json(Flag, JsonRequestBehavior.AllowGet);
                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ActionResult LoadGridLaptopDtlsInvoiceNoWiseJqGrid(string lInvoiceNo, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                sord = sord == "desc" ? "Desc" : "Asc";

                if (!string.IsNullOrEmpty(lInvoiceNo))
                    criteria.Add("InvoiceDetailsId", Convert.ToInt64(lInvoiceNo));

                Dictionary<long, IList<STAssetDetails>> STAssetDetailsList = ass.GetAssetListbyInvoiceNo(page - 1, rows, sidx, sord, criteria);

                if (STAssetDetailsList != null && STAssetDetailsList.FirstOrDefault().Key > 0)
                {
                    long totalrecords = STAssetDetailsList.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,
                        rows = (from items in STAssetDetailsList.First().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[]{
                                               items.AssetDet_Id.ToString(),  
                                               items.AssetCode,
                                               //items.AssetType, 
                                               items.Make, 
                                               items.Model,
                                               items.SerialNo,
                                               items.LTSize,
                                               items.OperatingSystemDtls,
                                               items.TransactionType,
                                               items.ReceivedCampus!=null?items.ReceivedCampus:"",
                                               items.ReceivedGrade!=null?items.ReceivedGrade:"",
                                               items.StudentTemplateView!=null?items.StudentTemplateView.NewId:"",
                                               items.StudentTemplateView!=null?items.StudentTemplateView.Name:""
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
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult LoadGridLaptopDtlsInvoiceNoWiseJqGrid1(string FromDt, string ToDt, string Campus, string VendorId, string InvoiceNo, string LaptopType, string OS, string TransactionType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                sord = sord == "desc" ? "Desc" : "Asc";

                DateTime[] fromto = new DateTime[2];

                if (!string.IsNullOrEmpty(FromDt) && !string.IsNullOrEmpty(ToDt))
                {
                    fromto[0] = DateTime.Parse(FromDt, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    fromto[1] = DateTime.Parse(ToDt, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    criteria.Add("InvoiceDate", fromto);
                }

                if (!string.IsNullOrEmpty(Campus))
                    criteria.Add("CurrentCampus", Campus);

                if (!string.IsNullOrEmpty(VendorId))
                    criteria.Add("VendorId", Convert.ToInt64(VendorId));

                if (!string.IsNullOrEmpty(InvoiceNo))
                    criteria.Add("InvoiceDetailsId", Convert.ToInt64(InvoiceNo));

                if (!string.IsNullOrEmpty(LaptopType))
                    criteria.Add("LTSize", LaptopType);

                if (!string.IsNullOrEmpty(OS))
                    criteria.Add("OperatingSystemDtls", OS);

                if (!string.IsNullOrEmpty(TransactionType))
                    criteria.Add("TransactionType", TransactionType);

                Dictionary<long, IList<LaptopEntryDtls_vw>> objLaptopEntryDtls = ass.GetLaptopEntryDtls(page - 1, rows, sidx, sord, criteria);

                if (objLaptopEntryDtls != null && objLaptopEntryDtls.FirstOrDefault().Key > 0)
                {
                    long totalrecords = objLaptopEntryDtls.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,
                        rows = (from items in objLaptopEntryDtls.First().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[]{
                                               items.AssetDet_Id.ToString(),  
                                               items.AssetCode,
                                               //items.AssetType, 
                                               items.Make, 
                                               items.Model,
                                               items.SerialNo,
                                               items.LTSize,
                                               items.OperatingSystemDtls,
                                               items.TransactionType,
                                               items.ReceivedCampus!=null?items.ReceivedCampus:"",
                                               items.ReceivedGrade!=null?items.ReceivedGrade:"",
                                               items.NewId!=null?items.NewId:"",
                                               items.Name!=null?items.Name:""
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
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult GetInvoiceNoByVendorIdandDocumentTypeandAcademicYear(string DocumentType, string FromDt, string ToDt)
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            Dictionary<string, object> likecriteria = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(DocumentType))
                criteria.Add("DocumentType", DocumentType);

            DateTime[] fromto = new DateTime[2];

            if (!string.IsNullOrEmpty(FromDt) && !string.IsNullOrEmpty(ToDt))
            {
                fromto[0] = DateTime.Parse(FromDt, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                fromto[1] = DateTime.Parse(ToDt, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                criteria.Add("InvoiceDate", fromto);
            }

            Dictionary<long, IList<AssetInvoiceDetails>> assetinvoicedetails = ass.GetAssetInvoiceDetailsWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria, likecriteria);
            if (assetinvoicedetails != null && assetinvoicedetails.First().Value != null && assetinvoicedetails.First().Value.Count > 0)
            {
                var VendorNameList = (
                         from items in assetinvoicedetails.First().Value
                         where items.TotalAsset > items.AssetCount
                         select new
                         {
                             Text = items.InvoiceNo,
                             Value = items.InvoiceDetailsId
                         }).Distinct().ToList();
                return Json(VendorNameList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public virtual JsonResult GetLaptopEntryStatus(string FromDt, string ToDt, string Campus, string VendorId, string InvoiceNo, string LaptopType, string OS, string TransactionType)
        {
            string[] lcount = new string[3];

            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                DateTime[] fromto = new DateTime[2];

                if (!string.IsNullOrEmpty(FromDt) && !string.IsNullOrEmpty(ToDt))
                {
                    fromto[0] = DateTime.Parse(FromDt, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    fromto[1] = DateTime.Parse(ToDt, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    criteria.Add("InvoiceDate", fromto);
                }

                if (!string.IsNullOrEmpty(Campus))
                    criteria.Add("CurrentCampus", Campus);

                if (!string.IsNullOrEmpty(VendorId))
                    criteria.Add("VendorId", Convert.ToInt64(VendorId));

                if (!string.IsNullOrEmpty(InvoiceNo))
                    criteria.Add("InvoiceDetailsId", Convert.ToInt64(InvoiceNo));

                if (!string.IsNullOrEmpty(LaptopType))
                    criteria.Add("LTSize", LaptopType);

                if (!string.IsNullOrEmpty(OS))
                    criteria.Add("OperatingSystemDtls", OS);

                if (!string.IsNullOrEmpty(TransactionType))
                    criteria.Add("TransactionType", TransactionType);

                Dictionary<long, IList<LaptopEntryDtls_vw>> objLaptopEntryDtls = ass.GetLaptopEntryDtls(0, 99999, "AssetDet_Id", "Asc", criteria);

                if (objLaptopEntryDtls != null && objLaptopEntryDtls.FirstOrDefault().Key > 0)
                {
                    lcount[0] = objLaptopEntryDtls.FirstOrDefault().Value.Count.ToString();
                    lcount[1] = (from u in objLaptopEntryDtls.FirstOrDefault().Value where u.TransactionType == "Stock" select u).Count().ToString();
                    lcount[2] = (from u in objLaptopEntryDtls.FirstOrDefault().Value where u.TransactionType == "Distributed" select u).Count().ToString();
                }
                var jsonCount = lcount;
                return Json(jsonCount);
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        #endregion

        #region StudentsAssetBulkUpload
        //public ActionResult StudentsBulkAssetUpload()
        //{
        //    try
        //    {
        //        #region BreadCrumb
        //        string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //        string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //        ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
        //        #endregion
        //        return View();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //[HttpPost]
        //public ActionResult StudentsBulkAssetUpload(HttpPostedFileBase[] uploadedFile, long Asset_Id, long FormId, bool IsSubAsset, long InvoiceDetailsId)
        //{
        //    StringBuilder retValue = new StringBuilder();
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //        else
        //        {
        //            Dictionary<string, object> criteria = new Dictionary<string, object>();
        //            UserService us = new UserService();
        //            HttpPostedFileBase theFile = HttpContext.Request.Files["uploadedFile"];
        //            StringBuilder alreadyExists = new StringBuilder();
        //            StringBuilder ErrorFilename = new StringBuilder();
        //            StringBuilder UploadedFilename = new StringBuilder();
        //            if (theFile != null && theFile.ContentLength > 0)
        //            {
        //                string fileName = string.Empty;
        //                int length = uploadedFile.Length;
        //                for (int l = 0; l < length; l++)
        //                {
        //                    try
        //                    {
        //                        string path = uploadedFile[l].InputStream.ToString();
        //                        byte[] imageSize = new byte[uploadedFile[l].ContentLength];
        //                        uploadedFile[l].InputStream.Read(imageSize, 0, (int)uploadedFile[l].ContentLength);
        //                        string UploadConnStr = "";
        //                        fileName = uploadedFile[l].FileName;
        //                        string fileExtn = Path.GetExtension(uploadedFile[l].FileName);
        //                        string fileLocation = ConfigurationManager.AppSettings["BulkUserCreationFilePath"].ToString() + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss").Replace(":", ".") + fileName;
        //                        uploadedFile[l].SaveAs(fileLocation);
        //                        if (fileExtn == ".xls")
        //                        {
        //                            UploadConnStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
        //                        }
        //                        if (fileExtn == ".xlsx")
        //                        {
        //                            UploadConnStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";
        //                        }
        //                        OleDbConnection itemconn = new OleDbConnection();
        //                        System.Data.DataTable DtblXcelItemData = new System.Data.DataTable();
        //                        string QeryToGetXcelItemData = "select * from " + string.Format("{0}${1}", "[Sheet1", "A1:AZ]");
        //                        itemconn.ConnectionString = UploadConnStr;
        //                        itemconn.Open();
        //                        OleDbCommand cmd = new OleDbCommand(QeryToGetXcelItemData, itemconn);
        //                        cmd.CommandType = CommandType.Text;
        //                        OleDbDataAdapter DtAdptrr = new OleDbDataAdapter();
        //                        DtAdptrr.SelectCommand = cmd;
        //                        DtAdptrr.Fill(DtblXcelItemData);
        //                        //string[] strArray = { "Brand", "Model", "SerialNo" };
        //                        string[] strArray = { "Brand", "Model", "SerialNo", "Warranty(In Months)", "Amount" };
        //                        AssetDetailsTemplate adtemplate = ass.GetAssetDetailsTemplateByAssetId(Asset_Id);
        //                        if (adtemplate != null)
        //                        {
        //                            strArray = strArray.Concat(adtemplate.SpecificationsDetails.Split(',')).ToArray();
        //                        }
        //                        char chrFlag = 'Y';
        //                        if (DtblXcelItemData.Columns.Count == strArray.Length)
        //                        {
        //                            int j = 0;
        //                            string[] strColumnsAray = new string[DtblXcelItemData.Columns.Count];
        //                            foreach (DataColumn dtColumn in DtblXcelItemData.Columns)
        //                            {
        //                                strColumnsAray[j] = dtColumn.ColumnName;
        //                                j++;
        //                            }
        //                            for (int i = 0; i < strArray.Length - 1; i++)
        //                            {
        //                                if (strArray[i].Trim() != strColumnsAray[i].Trim())
        //                                {
        //                                    chrFlag = 'N';
        //                                    break;
        //                                }
        //                            }
        //                            if (chrFlag == 'Y')
        //                            {
        //                                IList<STAssetDetails> STAssetDetailsList = new List<STAssetDetails>();
        //                                IList<STAssetDetailsTransactionHistory> STTransactionHistoryList = new List<STAssetDetailsTransactionHistory>();
        //                                criteria.Clear();
        //                                if (Asset_Id > 0)
        //                                    criteria.Add("Asset_Id", Asset_Id);
        //                                Dictionary<long, IList<STAssetDetails>> AssetCont = ass.GetSTAssetDetailsWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
        //                                var Assetcount = (from u in AssetCont.FirstOrDefault().Value select u).Distinct().ToList();
        //                                long count = Assetcount.Count;
        //                                AssetInvoiceDetails assetinvoicedetails = ass.GetAssetInvoiceDetailsByInvoiceDetailsId(InvoiceDetailsId);
        //                                long invoiceremainigcount = assetinvoicedetails.TotalAsset - assetinvoicedetails.AssetCount;
        //                                long invoicecount = assetinvoicedetails.AssetCount;
        //                                if (invoiceremainigcount < DtblXcelItemData.Rows.Count)
        //                                {
        //                                    return Json(new { success = true, result = "Excel has More Assets for this Invoice", status = "failed" }, "text/html", JsonRequestBehavior.AllowGet);
        //                                }
        //                                foreach (DataRow item in DtblXcelItemData.Rows)
        //                                {
        //                                    Dictionary<string, object> likecriteria = new Dictionary<string, object>();
        //                                    criteria.Clear();
        //                                    if (!string.IsNullOrEmpty(item["Brand"] != null ? item["Brand"].ToString().Trim() : ""))
        //                                        criteria.Add("Make", item["Brand"].ToString().Trim().ToUpper());
        //                                    if (!string.IsNullOrEmpty(item["Model"] != null ? item["Model"].ToString().Trim() : ""))
        //                                        criteria.Add("Model", item["Model"].ToString().Trim().ToUpper());
        //                                    if (!string.IsNullOrEmpty(item["SerialNo"] != null ? item["SerialNo"].ToString().Trim() : ""))
        //                                        criteria.Add("SerialNo", item["SerialNo"].ToString().Trim());
        //                                    Dictionary<long, IList<STAssetDetails>> assetdetails = ass.GetSTAssetDetailsWithPagingAndExactAndLikeCriteria(0, 9999, "AssetDet_Id", "Asc", criteria, likecriteria);
        //                                    if (assetdetails == null || assetdetails.FirstOrDefault().Key == 0)
        //                                    {
        //                                        STAssetDetails assetdtls = new STAssetDetails();
        //                                        assetdtls.Asset_Id = adtemplate.Asset_Id;
        //                                        assetdtls.AssetType = adtemplate.AssetType;
        //                                        assetdtls.Model = item["Model"] != null ? item["Model"].ToString().Trim().ToUpper() : "";
        //                                        assetdtls.Make = item["Brand"] != null ? item["Brand"].ToString().Trim().ToUpper() : "";
        //                                        assetdtls.SerialNo = item["SerialNo"] != null ? item["SerialNo"].ToString().Trim() : "";
        //                                        assetdtls.AssetCode = "STU-" + adtemplate.AssetCode + "-" + (count + 1).ToString();
        //                                        MastersService ms = new MastersService();
        //                                        CampusMaster cm = ms.GetCampusById(FormId);
        //                                        assetdtls.CurrentCampus = cm.Name;
        //                                        assetdtls.FromCampus = cm.Name;
        //                                        assetdtls.CampusMaster = cm;
        //                                        assetdtls.CreatedDate = DateTime.Now;
        //                                        assetdtls.CreatedBy = userId;
        //                                        assetdtls.TransactionType = "Stock";
        //                                        assetdtls.Location = "Stock";
        //                                        assetdtls.CurrentBlock = "Stock";
        //                                        assetdtls.CurrentLocation = "Stock";
        //                                        assetdtls.FromBlock = "Stock";
        //                                        assetdtls.IsStandBy = true;
        //                                        assetdtls.IsActive = true;
        //                                        assetdtls.Amount = item["Amount"] != null ? Convert.ToDecimal(item["Amount"]) : 0;
        //                                        assetdtls.Warranty = item["Warranty(In Months)"] != null ? item["Warranty(In Months)"].ToString().Trim() : "";
        //                                        assetdtls.EngineerName = userId;
        //                                        if (IsSubAsset == true)
        //                                        {
        //                                            assetdtls.SubAssetType = "External";
        //                                            assetdtls.IsSubAsset = true;
        //                                        }
        //                                        string[] specs = adtemplate.SpecificationsDetails.Split(',').ToArray();
        //                                        for (int k = 0; k < specs.Length; k++)
        //                                        {
        //                                            item[specs[k]] = item[specs[k]] != null ? item[specs[k]].ToString().Trim() : "";
        //                                            assetdtls.SpecificationsDetails += "," + item[specs[k]];
        //                                        }
        //                                        assetdtls.InvoiceDetailsId = assetinvoicedetails.InvoiceDetailsId;
        //                                        STAssetDetailsList.Add(assetdtls);
        //                                        invoicecount = invoicecount + 1;
        //                                        if (assetinvoicedetails.TotalAsset == invoicecount)
        //                                        {
        //                                            assetinvoicedetails.IsActive = false;
        //                                        }
        //                                        count = count + 1;
        //                                    }
        //                                }
        //                                if (STAssetDetailsList.Count > 0)
        //                                {
        //                                    ass.SaveOrUpdateSTAssetDetailsList(STAssetDetailsList);
        //                                    assetinvoicedetails.AssetCount = invoicecount;
        //                                    ass.CreateOrUpdateAssetInvoiceDetails(assetinvoicedetails);
        //                                    foreach (var item in STAssetDetailsList)
        //                                    {
        //                                        string[] specList = adtemplate.Specifications.Split(',').ToArray();
        //                                        string[] specValues = item.SpecificationsDetails.Split(',').ToArray();

        //                                        List<string> specDetailsList = new List<string>();

        //                                        Dictionary<string, object> spec = new Dictionary<string, object>();
        //                                        for (int i = 0; i < specList.Length; i++)
        //                                        {
        //                                            if (specList[i] == "Id")
        //                                                spec.Add(specList[i], item.AssetDet_Id);
        //                                            else
        //                                                spec.Add(specList[i], specValues[i]);
        //                                        }
        //                                        JavaScriptSerializer serializer = new JavaScriptSerializer();
        //                                        item.SpecificationsDetails = serializer.Serialize(spec);
        //                                        STAssetDetailsTransactionHistory transactionhistory = new STAssetDetailsTransactionHistory();
        //                                        transactionhistory.STAssetDetails = item;
        //                                        transactionhistory.FromCampus = item.FromCampus;
        //                                        transactionhistory.FromBlock = item.FromBlock;
        //                                        transactionhistory.FromLocation = item.Location;
        //                                        transactionhistory.ToCampus = item.CurrentCampus;
        //                                        transactionhistory.ToLocation = item.CurrentLocation;
        //                                        transactionhistory.ToBlock = item.CurrentBlock;
        //                                        //transactionhistory.UserType = item.UserType;
        //                                        transactionhistory.CreatedBy = userId;
        //                                        transactionhistory.CreatedDate = DateTime.Now;
        //                                        transactionhistory.TransactionType = "Stock";
        //                                        transactionhistory.TransactionType_Id = item.AssetDet_Id;
        //                                        STTransactionHistoryList.Add(transactionhistory);
        //                                    }
        //                                    ass.SaveOrUpdateSTAssetDetailsList(STAssetDetailsList);
        //                                    ass.SaveOrUpdateSTAssetDetailsTransactionHistoryList(STTransactionHistoryList);
        //                                    return Json(new { success = true, result = "Asset created successfully..", status = "success" }, "text/html", JsonRequestBehavior.AllowGet);
        //                                }
        //                                else
        //                                {
        //                                    if (DtblXcelItemData.Rows.Count > 0)
        //                                    {
        //                                        return Json(new { success = true, result = "Already Exist!!", status = "exist" }, "text/html", JsonRequestBehavior.AllowGet);
        //                                    }
        //                                    else
        //                                    {
        //                                        ErrorFilename.Append(fileName + ",");
        //                                    }
        //                                }
        //                            }
        //                            else
        //                            {
        //                                ErrorFilename.Append(fileName + ",");
        //                            }
        //                        }
        //                        else
        //                        {
        //                            ErrorFilename.Append(fileName + ",");
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        ErrorLogs el = new ErrorLogs();
        //                        el.ExceptionErrorLog = ex.ToString();
        //                        ass.CreateOrUpdateErrorLogs(el);
        //                        ErrorFilename.Append(fileName + ",");
        //                        ExceptionPolicy.HandleException(ex, "CustomAccountPolicy");
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                return Json(new { success = false, result = "You have uploaded the empty file. Please upload the correct file." }, "text/html", JsonRequestBehavior.AllowGet);
        //            }

        //            if (UploadedFilename != null && !string.IsNullOrEmpty(UploadedFilename.ToString()))
        //            {
        //                retValue.Append("-------files uploaded successfully-----------");
        //                retValue.Append("<br />");
        //                string[] upfiles = UploadedFilename.ToString().Split(',');
        //                if (upfiles != null && upfiles.Count() > 0)
        //                {
        //                    foreach (string s in upfiles)
        //                    {
        //                        if (!string.IsNullOrEmpty(s))
        //                        {
        //                            retValue.Append(s + ";");
        //                            retValue.Append("<br />");
        //                        }
        //                    }

        //                    retValue.Append("<br />");
        //                    retValue.Append("Successfully uploaded files" + Convert.ToInt32(UploadedFilename.ToString().Split(',').Count() - 1));
        //                    retValue.Append("<br />");
        //                    //retValue.Append("-----------------------------------------------------");
        //                }
        //            }
        //            if (alreadyExists != null && !string.IsNullOrEmpty(alreadyExists.ToString()))
        //            {
        //                retValue.Append("-----------files already exists--------------");
        //                retValue.Append("<br />");
        //                string[] existsfiles = alreadyExists.ToString().Split(',');
        //                if (existsfiles != null && existsfiles.Count() > 0)
        //                {
        //                    foreach (string s in existsfiles)
        //                    { if (!string.IsNullOrEmpty(s)) retValue.Append(s + ";"); retValue.Append("<br />"); }
        //                    //retValue.Append("-------------------------------------------------");
        //                }
        //            }
        //            if (ErrorFilename != null && !string.IsNullOrEmpty(ErrorFilename.ToString()))
        //            {
        //                retValue.Append("-----------error occured Files--------------");
        //                string[] errfiles = ErrorFilename.ToString().Split(',');
        //                if (errfiles != null && errfiles.Count() > 0)
        //                {
        //                    foreach (string s in errfiles)
        //                    { if (!string.IsNullOrEmpty(s))retValue.Append(s + ";"); retValue.Append("<br />"); }
        //                    //retValue.Append("-------------------------------------------------");
        //                }
        //            }
        //            return Json(new { success = true, result = retValue.ToString().Replace(Environment.NewLine, "<br />") }, "text/html", JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "CustomAccountPolicy");
        //    }
        //    //MyLabel.Text = sb.ToString().Replace(Environment.NewLine, "<br />");
        //    return Json(new { success = true, result = retValue.ToString().Replace(Environment.NewLine, "<br />") }, "text/html", JsonRequestBehavior.AllowGet);
        //}
        //public ActionResult LaptopGenerateExcelFormat(long Asset_Id)
        //{
        //    string[] Columns = { "Brand", "Model", "SerialNo", "Warranty(In Months)", "Amount" };

        //    AssetDetailsTemplate adtemplate = ass.GetAssetDetailsTemplateByAssetId(Asset_Id);
        //    if (adtemplate != null)
        //    {
        //        Columns = Columns.Concat(adtemplate.SpecificationsDetails.Split(',')).ToArray();
        //    }
        //    ExcelPackage objExcelPackage = new ExcelPackage();   //create new workbook

        //    //string[] filesindirectory = Directory.GetFiles(Server.MapPath("~/Images"));            
        //    int count = 1;
        //    ExcelWorksheet ews = objExcelPackage.Workbook.Worksheets.Add("Sheet" + count); //create new worksheet
        //    //            ews.View.ZoomScale = 100;
        //    ews.View.ShowGridLines = true;
        //    string[] alphabets = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        //    for (int i = 0; i < Columns.Length; i++)
        //    {
        //        ews.Cells[alphabets[i] + "1"].Value = Columns[i];
        //    }
        //    ews.Cells[ews.Dimension.Address].AutoFitColumns();
        //    string FileName = "ExcelFormat-" + adtemplate.AssetType;
        //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //    Response.AddHeader("content-disposition", "attachment;  filename=" + FileName + ".xlsx");
        //    byte[] File = objExcelPackage.GetAsByteArray();
        //    Response.BinaryWrite(File);
        //    Response.End();
        //    return null;
        //}
        #endregion

    }
}