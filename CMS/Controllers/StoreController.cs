using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using TIPS.Entities;
using TIPS.Entities.StoreEntities;
using TIPS.Service;
using TIPS.ServiceContract;
using System.Net.Mail;
using TIPS.Entities.AdmissionEntities;
using TIPS.Entities.InboxEntities;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace CMS.Controllers
{
    public class StoreController : BaseController
    {
        UserService us = new UserService();
        public ActionResult StartMaterialRequest(MaterialRequest m)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    StoreService ss = new StoreService();
                    if (m.Id == 0)
                    {
                        long Id = 0;
                        Id = ss.StartStoreManagement(m, "StoreManagement", userId);
                        m.RequestNumber = "MRF-" + Id;
                        ss.CreateOrUpdateMaterialRequest(m);
                        long InstanceId = m.InstanceId;
                        string[] ret = new string[2];
                        ret[0] = m.RequestNumber;
                        ret[1] = Convert.ToString(m.InstanceId);
                        return Json(ret, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        ss.CreateOrUpdateMaterialRequest(m);
                        m = ss.GetMaterialRequestById(m.Id);
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult AddMaterialRequest(MaterialRequestList mrl)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    string aa = Request["txtReqDate"];
                    //IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    //mrl.RequiredDate = DateTime.Parse(Request["txtReqDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    StoreService ss = new StoreService();
                    ss.CreateOrUpdateMaterialRequestList(mrl);
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult AddMaterialRequestList(IList<MaterialRequestList> MatReqLst)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    StoreService ss = new StoreService();
                    if (MatReqLst != null)
                        ss.CreateOrUpdateMateraialRequestList(MatReqLst);
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult MaterialRequest(long? Id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    UserService us = new UserService();
                    MastersService ms = new MastersService();
                    StoreService ss = new StoreService();
                    Dictionary<string, object> Criteria = new Dictionary<string, object>();
                    var usrcmp = Session["UserCampus"] as IEnumerable<string>;
                    if (usrcmp != null && usrcmp.Count() != 0)
                    {
                        if (usrcmp.First() != null)            // to check if the usrcmp obj is null or with data
                        {
                            Criteria.Add("Name", usrcmp);
                        }
                    }
                    Dictionary<long, IList<CampusMaster>> CampusMaster = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, Criteria);
                    ViewBag.campusddl = CampusMaster.First().Value;
                    Criteria.Clear();
                    Criteria.Add("UserId", userId);
                    Criteria.Add("AppCode", "STR");
                    Dictionary<long, IList<UserAppRole>> UserAppRoleList = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, Criteria);
                    var UserDetails = (from u in UserAppRoleList.First().Value
                                       where u.UserId == userId
                                       select new { u.RoleName, u.BranchCode }).ToArray();
                    var rle = Session["userrolesarray"] as IEnumerable<string>;
                    if (rle != null && rle.Contains("INC"))
                    {
                        ViewBag.Flag = "Show";
                    }
                    else { ViewBag.Flag = "Hide"; }
                    Criteria.Clear();
                    Dictionary<long, IList<GradeMaster>> GradeMaster = ms.GetGradeMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, Criteria);
                    ViewBag.gradeddl = GradeMaster.First().Value;
                    if (Id > 0)
                    {
                        MaterialRequest m = ss.GetMaterialRequestById(Convert.ToInt64(Id));
                        return View(m);
                    }
                    else
                    {
                        MaterialRequest mr = new MaterialRequest();
                        User user = (User)Session["objUser"];
                        if (user != null)
                            mr.Campus = user.Campus;
                        mr.UserRole = UserDetails[0].RoleName;
                        mr.RequestStatus = "CreateMatRequest";
                        mr.ProcessedBy = userId;
                        mr.RequestedDate = DateTime.Now;
                        mr.Department = "";
                        mr.Id = 0;
                        return View(mr);
                    }

                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult MaterialRequest(MaterialRequest mr)
        {
            try
            {
                string userId = base.ValidateUser();
                string info = string.Empty;
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    StoreService ss = new StoreService();
                    bool isRejected = false;
                    if (Request.Form["btnSubmit"] == "Submit")
                    {
                        MaterialRequest m = ss.GetMaterialRequestById(mr.Id);
                        mr.InstanceId = m.InstanceId;
                        ss.CompleteActivityStoreManagement(mr, "StoreManagement", userId, "CreateMatRequest", isRejected);
                        info = " You have submitted an request to store with  " + m.RequestNumber;
                        UpdateInbox(m.Campus, info, userId, m.Id);
                        return RedirectToAction("MaterialRequestList");
                    }
                    else if (Request.Form["btnReject"] == "Reject")
                    {
                        isRejected = true;
                        string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                        CommentsService cmntsSrvc = new CommentsService();
                        Comments cmntsObj = new Comments();
                        cmntsObj.EntityRefId = mr.Id;

                        cmntsObj.CommentedBy = userid;
                        cmntsObj.CommentedOn = DateTime.Now;
                        cmntsObj.RejectionComments = Request["txtRejDescription"];
                        cmntsObj.AppName = "STR";
                        MaterialRequest m = ss.GetMaterialRequestById(mr.Id);
                        info = " You have submitted an request to store with  " + m.RequestNumber;
                        UpdateInbox(m.Campus, info, userId, m.Id);
                        cmntsSrvc.CreateOrUpdateComments(cmntsObj);
                        ss.CompleteActivityStoreManagement(mr, "StoreManagement", userId, mr.RequestStatus, isRejected);
                        return RedirectToAction("MaterialRequestList");
                    }
                    else if (Request.Form["btnReply"] == "Reply")
                    {
                        CommentsService cmntsSrvc1 = new CommentsService();
                        Dictionary<string, object> criteria1 = new Dictionary<string, object>();
                        criteria1.Add("EntityRefId", mr.Id);
                        Dictionary<long, IList<Comments>> list = cmntsSrvc1.GetCommentsListWithPaging(0, 1000, string.Empty, string.Empty, criteria1);
                        if (list != null && list.Count > 0)
                        {
                            foreach (Comments cm in list.First().Value)
                            {
                                if (string.IsNullOrWhiteSpace(cm.ResolutionComments))
                                {
                                    cm.ResolutionComments = Request["txtReplyDescription"];
                                    cmntsSrvc1.CreateOrUpdateComments(cm);
                                    break;
                                }
                            }
                        }
                        ss.CompleteActivityStoreManagement(mr, "StoreManagement", userId, mr.RequestStatus, isRejected);
                        return RedirectToAction("MaterialRequestList");
                    }
                    else if (Request.Form["btnApprove"] == "Approve")
                    {
                        ss.CompleteActivityStoreManagement(mr, "StoreManagement", userId, mr.RequestStatus, isRejected);
                        return RedirectToAction("MaterialRequestList");
                    }
                    //else if (Request.Form["btnIssue"] == "Issue")
                    //{
                    //    ss.CompleteActivityStoreManagement(mr, "StoreManagement", userId, mr.RequestStatus, isRejected);
                    //    return RedirectToAction("MaterialRequestList");
                    //}
                    else if (Request.Form["btnComplete"] == "Complete")
                    {
                        if (!string.IsNullOrWhiteSpace(mr.RequiredForStore))
                        {
                            Dictionary<string, object> criteria = new Dictionary<string, object>();
                            criteria.Add("MatReqRefId", mr.Id);
                            Dictionary<long, IList<MaterialRequestList>> MaterialRequestlist = ss.GetMaterialRequestListListWithPagingAndCriteria(0, 9999, "", "", criteria);
                            if (MaterialRequestlist != null && MaterialRequestlist.FirstOrDefault().Value != null && MaterialRequestlist.FirstOrDefault().Value.Count > 0)
                            {
                                MaterialInward mi = new MaterialInward();
                                mi.ProcessedBy = mr.ProcessedBy;
                                mi.UserRole = mr.UserRole;
                                mi.Status = "Completed";
                                mi.Campus = mr.Campus;
                                mi.Store = mr.RequiredForStore;
                                mi.CreatedDate = DateTime.Now;
                                mi.Supplier = mr.RequiredFromStore;
                                mi.ReceivedBy = mr.ProcessedBy;
                                mi.ReceivedDateTime = DateTime.Now;
                                mi.InvoiceDate = DateTime.Now;
                                mi.DCDate = DateTime.Now;
                                mi.SuppRefNo = mr.RequestNumber;
                                ss.CreateOrUpdateMaterialInward(mi);
                                mi.InwardNumber = "MIN-" + mi.Id;
                                info = " You have Completed an request on store with  " + mr.RequestNumber;
                                UpdateInbox(mr.Campus, info, userId, mr.Id);
                                ss.CreateOrUpdateMaterialInward(mi);
                                foreach (var item in MaterialRequestlist.FirstOrDefault().Value)
                                {
                                    SkuList sku = new SkuList();
                                    sku.MaterialRefId = mi.Id;
                                    sku.MaterialGroup = item.MaterialGroup;
                                    sku.MaterialSubGroup = item.MaterialSubGroup;
                                    sku.Material = item.Material;
                                    sku.OrderedUnits = item.Units;
                                    sku.OrderQty = item.Quantity;
                                    sku.ReceivedQty = item.IssuedQty;
                                    sku.DamagedQty = 0;
                                    sku.DamagelessQty = item.IssuedQty;
                                    sku.IssuedQty = 0;
                                    sku.StockAvailableQty = item.IssuedQty;
                                    sku.IssuedStatus = "Not Issued";
                                    sku.DamageDescription = string.Empty;
                                    sku.ReceivedUnits = item.Units;

                                    Dictionary<string, object> criteria1 = new Dictionary<string, object>();
                                    criteria1.Add("Material", sku.Material.Trim());
                                    criteria1.Add("MaterialInward." + "Store", mr.RequiredFromStore);
                                    string[] alias = new string[1];
                                    alias[0] = "MaterialInward";
                                    Dictionary<long, IList<SkuList_MaterialInward>> SkuList = ss.GetSkuList_MaterialInwardListWithPagingAndCriteria(0, 9999, "SkuId", "Desc", criteria1, "", null, alias);
                                    if (SkuList != null && SkuList.FirstOrDefault().Value != null && SkuList.Count() > 0 && SkuList.FirstOrDefault().Key > 0)
                                    {
                                        sku.UnitPrice = SkuList.FirstOrDefault().Value[0].UnitPrice;
                                        sku.Tax = SkuList.FirstOrDefault().Value[0].Tax;
                                        sku.Discount = SkuList.FirstOrDefault().Value[0].Discount;
                                        sku.TotalPrice = sku.ReceivedQty * (sku.UnitPrice - (sku.UnitPrice * sku.Discount / 100) + (sku.UnitPrice * sku.Tax / 100));
                                    }
                                    ss.CreateOrUpdateSku(sku);

                                    MaterialsMaster mm = ss.GetMaterialsMasterByMaterial(item.Material.Trim());
                                    if (mm != null)
                                    {
                                        StockTransaction st = new StockTransaction();
                                        st.TransactionCode = mi.InwardNumber;
                                        st.Store = mi.Store;
                                        st.ItemId = mm.Id;
                                        st.Units = sku.ReceivedUnits;
                                        st.TransactionDate = DateTime.Now;
                                        st.TransactionBy = mi.ProcessedBy;
                                        st.TransactionType = "Material Inward";
                                        st.Qty = sku.ReceivedQty;
                                        st.DamagedQty = Convert.ToInt16(sku.DamagedQty);
                                        // st.DamagedRemarks = MaterialSkulist.First().Value[i].DamageDescription;
                                        st.TransactionComments = sku.DamageDescription;

                                        ss.CreateOrUpdateStockTransaction(st);
                                    }
                                }
                            }
                        }
                        ss.CompleteActivityStoreManagement(mr, "StoreManagement", userId, mr.RequestStatus, isRejected);
                    }
                }
                return RedirectToAction("MaterialRequestList");
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        //public ActionResult MaterialRequest(MaterialRequest mr)
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //        else
        //        {
        //            StoreService ss = new StoreService();
        //            bool isRejected = false;

        //            if (Request.Form["btnSubmit"] == "Submit")
        //            {
        //                MaterialRequest m = ss.GetMaterialRequestById(mr.Id);
        //                mr.InstanceId = m.InstanceId;
        //                ss.CompleteActivityStoreManagement(mr, "StoreManagement", userId, "CreateMatRequest", isRejected);
        //                return RedirectToAction("MaterialRequestList");
        //            }
        //            else if (Request.Form["btnReject"] == "Reject")
        //            {
        //                isRejected = true;
        //                string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
        //                CommentsService cmntsSrvc = new CommentsService();
        //                Comments cmntsObj = new Comments();
        //                cmntsObj.EntityRefId = mr.Id;

        //                cmntsObj.CommentedBy = userid;
        //                cmntsObj.CommentedOn = DateTime.Now;
        //                cmntsObj.RejectionComments = Request["txtRejDescription"];
        //                cmntsObj.AppName = "STR";
        //                cmntsSrvc.CreateOrUpdateComments(cmntsObj);
        //                ss.CompleteActivityStoreManagement(mr, "StoreManagement", userId, mr.RequestStatus, isRejected);
        //                return RedirectToAction("MaterialRequestList");
        //            }
        //            else if (Request.Form["btnReply"] == "Reply")
        //            {
        //                CommentsService cmntsSrvc1 = new CommentsService();
        //                Dictionary<string, object> criteria1 = new Dictionary<string, object>();
        //                criteria1.Add("EntityRefId", mr.Id);
        //                Dictionary<long, IList<Comments>> list = cmntsSrvc1.GetCommentsListWithPaging(0, 1000, string.Empty, string.Empty, criteria1);
        //                if (list != null && list.Count > 0)
        //                {
        //                    foreach (Comments cm in list.First().Value)
        //                    {
        //                        if (string.IsNullOrWhiteSpace(cm.ResolutionComments))
        //                        {
        //                            cm.ResolutionComments = Request["txtReplyDescription"];
        //                            cmntsSrvc1.CreateOrUpdateComments(cm);
        //                            break;
        //                        }
        //                    }
        //                }
        //                ss.CompleteActivityStoreManagement(mr, "StoreManagement", userId, mr.RequestStatus, isRejected);
        //                return RedirectToAction("MaterialRequestList");
        //            }
        //            else if (Request.Form["btnApprove"] == "Approve")
        //            {
        //                ss.CompleteActivityStoreManagement(mr, "StoreManagement", userId, mr.RequestStatus, isRejected);
        //                return RedirectToAction("MaterialRequestList");
        //            }
        //            //else if (Request.Form["btnIssue"] == "Issue")
        //            //{
        //            //    ss.CompleteActivityStoreManagement(mr, "StoreManagement", userId, mr.RequestStatus, isRejected);
        //            //    return RedirectToAction("MaterialRequestList");
        //            //}
        //            else if (Request.Form["btnComplete"] == "Complete")
        //            {
        //                if (!string.IsNullOrWhiteSpace(mr.RequiredForStore))
        //                {
        //                    Dictionary<string, object> criteria = new Dictionary<string, object>();
        //                    criteria.Add("MatReqRefId", mr.Id);
        //                    Dictionary<long, IList<MaterialRequestList>> MaterialRequestlist = ss.GetMaterialRequestListListWithPagingAndCriteria(0, 9999, "", "", criteria);
        //                    if (MaterialRequestlist != null && MaterialRequestlist.FirstOrDefault().Value != null && MaterialRequestlist.FirstOrDefault().Value.Count > 0)
        //                    {
        //                        MaterialInward mi = new MaterialInward();
        //                        mi.ProcessedBy = mr.ProcessedBy;
        //                        mi.UserRole = mr.UserRole;
        //                        mi.Status = "Completed";
        //                        mi.Campus = mr.Campus;
        //                        mi.Store = mr.RequiredForStore;
        //                        mi.CreatedDate = DateTime.Now;
        //                        mi.Supplier = mr.RequiredFromStore;
        //                        mi.ReceivedBy = mr.ProcessedBy;
        //                        mi.ReceivedDateTime = DateTime.Now;
        //                        mi.InvoiceDate = DateTime.Now;
        //                        mi.DCDate = DateTime.Now;
        //                        mi.SuppRefNo = mr.RequestNumber;
        //                        ss.CreateOrUpdateMaterialInward(mi);
        //                        mi.InwardNumber = "MIN-" + mi.Id;
        //                        ss.CreateOrUpdateMaterialInward(mi);
        //                        foreach (var item in MaterialRequestlist.FirstOrDefault().Value)
        //                        {
        //                            SkuList sku = new SkuList();
        //                            sku.MaterialRefId = mi.Id;
        //                            sku.MaterialGroup = item.MaterialGroup;
        //                            sku.MaterialSubGroup = item.MaterialSubGroup;
        //                            sku.Material = item.Material;
        //                            sku.OrderedUnits = item.Units;
        //                            sku.OrderQty = item.Quantity;
        //                            sku.ReceivedQty = item.IssuedQty;
        //                            sku.DamagedQty = 0;
        //                            sku.DamagelessQty = item.IssuedQty;
        //                            sku.IssuedQty = 0;
        //                            sku.StockAvailableQty = item.IssuedQty;
        //                            sku.IssuedStatus = "Not Issued";
        //                            sku.DamageDescription = string.Empty;
        //                            sku.ReceivedUnits = item.Units;

        //                            Dictionary<string, object> criteria1 = new Dictionary<string, object>();
        //                            criteria1.Add("Material", sku.Material.Trim());
        //                            criteria1.Add("MaterialInward." + "Store", mr.RequiredFromStore);
        //                            string[] alias = new string[1];
        //                            alias[0] = "MaterialInward";
        //                            Dictionary<long, IList<SkuList_MaterialInward>> SkuList = ss.GetSkuList_MaterialInwardListWithPagingAndCriteria(0, 9999, "SkuId", "Desc", criteria1, "", null, alias);
        //                            if (SkuList != null && SkuList.FirstOrDefault().Value != null && SkuList.Count() > 0 && SkuList.FirstOrDefault().Key > 0)
        //                            {
        //                                sku.UnitPrice = SkuList.FirstOrDefault().Value[0].UnitPrice;
        //                                sku.TotalPrice = sku.ReceivedQty * sku.UnitPrice;
        //                            }
        //                            ss.CreateOrUpdateSku(sku);

        //                            MaterialsMaster mm = ss.GetMaterialsMasterByMaterial(item.Material.Trim());
        //                            if (mm != null)
        //                            {
        //                                StockTransaction st = new StockTransaction();
        //                                st.TransactionCode = mi.InwardNumber;
        //                                st.Store = mi.Store;
        //                                st.ItemId = mm.Id;
        //                                st.Units = sku.ReceivedUnits;
        //                                st.TransactionDate = DateTime.Now;
        //                                st.TransactionBy = mi.ProcessedBy;
        //                                st.TransactionType = "Material Inward";
        //                                st.Qty = sku.ReceivedQty;
        //                                st.DamagedQty = Convert.ToInt16(sku.DamagedQty);
        //                                // st.DamagedRemarks = MaterialSkulist.First().Value[i].DamageDescription;
        //                                st.TransactionComments = sku.DamageDescription;

        //                                ss.CreateOrUpdateStockTransaction(st);
        //                            }
        //                        }
        //                    }
        //                }
        //                ss.CompleteActivityStoreManagement(mr, "StoreManagement", userId, mr.RequestStatus, isRejected);
        //            }
        //        }
        //        return RedirectToAction("MaterialRequestList");
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
        //        throw ex;
        //    }
        //}

        public ActionResult MaterialRequestList()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TIPS.Service.UserService us = new UserService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("UserId", userId);
                    criteria.Add("AppCode", "STR");
                    Dictionary<long, IList<UserAppRole>> UserAppRoleList = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 100, string.Empty, string.Empty, criteria);
                    var ListCount = (from u in UserAppRoleList.First().Value
                                     where u.UserId == userId
                                     select u.RoleCode).ToList();
                    if (ListCount.Contains("MRC"))
                    {
                        ViewBag.Flag = "MRC";
                    }
                    else if (ListCount.Contains("INC"))
                    {
                        ViewBag.Flag = "INC";
                    }
                }
                string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                return View();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult MaterialRequestJqGrid(long Id, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    StoreService ss = new StoreService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    sord = sord == "desc" ? "Desc" : "Asc";
                    criteria.Add("MatReqRefId", Id);
                    Dictionary<long, IList<MaterialRequestList>> MaterialRequestlist = ss.GetMaterialRequestListListWithPagingAndCriteria(page - 1, rows, sord, sidx, criteria);
                    if (MaterialRequestlist != null && MaterialRequestlist.Count > 0)
                    {
                        long totalrecords = MaterialRequestlist.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat1 = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in MaterialRequestlist.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                               items.Id.ToString(),
                               items.RequestType,
                               items.RequiredForGrade,
                               items.Section,
                               items.RequiredFor,
                               items.Material,
                               items.MaterialGroup,
                               items.MaterialSubGroup,
                               items.Units,
                               items.RequiredDate != null ? items.RequiredDate.Value.ToString("dd/MM/yyyy") : null,
                               items.Status,
                               items.Quantity.ToString(),
                               items.ApprovedQty.ToString(),
                               items.IssuedQty.ToString()
                            }
                                    })
                        };
                        return Json(jsondat1, JsonRequestBehavior.AllowGet);
                    }
                    else
                    { return null; }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public JsonResult FillGrade()
        {
            try
            {
                MastersService ms = new MastersService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<long, IList<GradeMaster>> GradeMaster = ms.GetGradeMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                var Grade = (
                         from items in GradeMaster.First().Value
                         select new
                         {
                             Text = items.gradcod,
                             Value = items.gradcod
                         }).ToList();
                return Json(Grade, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult StudentDetails(string Campus, string Grade, string Section)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) { return RedirectToAction("LogOff", "Account"); }
                else
                {
                    ViewBag.Campus = Campus;
                    ViewBag.Grade = Grade;
                    ViewBag.Section = Section;
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult StudentDetailsListJqGrid(string idno, string name, string cname, string grade, string sect, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MasterDataService sds = new MasterDataService();
                    AdmissionManagementService ams = new AdmissionManagementService();
                    Dictionary<string, object> Eqcriteria = new Dictionary<string, object>();
                    Dictionary<string, object> Likecriteria = new Dictionary<string, object>();
                    if (!string.IsNullOrWhiteSpace(idno)) { Likecriteria.Add("NewId", idno); }
                    if (!string.IsNullOrWhiteSpace(name)) { Likecriteria.Add("Name", name); }
                    if (!string.IsNullOrWhiteSpace(cname)) { Eqcriteria.Add("Campus", cname); }
                    if (!string.IsNullOrWhiteSpace(grade)) { Eqcriteria.Add("Grade", grade); }
                    if (!string.IsNullOrWhiteSpace(sect)) { Eqcriteria.Add("Section", sect); }
                    Eqcriteria.Add("AdmissionStatus", "Registered");
                    sord = sord == "desc" ? sord = "Desc" : sord = "Asc";
                    Dictionary<long, IList<StudentTemplateView>> studentdetailslist;
                    studentdetailslist = ams.GetStudentTemplateViewListWithLikeandExactPagingAndCriteria(0, 9999, sord, sidx, Eqcriteria, Likecriteria);
                    if (studentdetailslist != null && studentdetailslist.Count > 0)
                    {
                        long totalRecords = studentdetailslist.First().Key;
                        int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                        var jsonData = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalRecords,
                            rows = (
                            from items in studentdetailslist.First().Value
                            select new
                            {
                                i = items.Id,
                                cell = new string[] 
                       { 
                           items.Id.ToString(),
                           items.NewId,
                           items.Name,
                           items.Section,
                           items.Campus,
                           items.Grade,
                           items.IsHosteller.ToString()
                    }
                            })
                        };
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                    }
                    else
                    { return null; }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult StoreSupplier()
        {
            return View();
        }

        public ActionResult StoreSupplierListJqGrid(string SupplierName, string CompanyName, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    StoreService ss = new StoreService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    if (!string.IsNullOrWhiteSpace(SupplierName)) { criteria.Add("SupplierName", SupplierName); }
                    if (!string.IsNullOrWhiteSpace(CompanyName)) { criteria.Add("CompanyName", CompanyName); }
                    sord = sord == "desc" ? sord = "Desc" : sord = "Asc";
                    Dictionary<long, IList<StoreSupplierMaster>> StoreSupplierlist = ss.GetStoreSupplierMasterlistWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    if (StoreSupplierlist != null && StoreSupplierlist.Count > 0)
                    {
                        long totalrecords = StoreSupplierlist.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat1 = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,

                            rows = (from items in StoreSupplierlist.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                               items.Id.ToString(),
                               items.SupplierName ,
                               items.CompanyName
                                    }
                                    })
                        };
                        return Json(jsondat1, JsonRequestBehavior.AllowGet);
                    }
                    else
                    { return null; }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public JsonResult FillStoreUnits()
        {
            try
            {
                string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                StoreService ss = new StoreService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<long, IList<StoreUnits>> StoreList = ss.GetStoreUnitsListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteria);
                if (StoreList != null && StoreList.First().Value != null && StoreList.First().Value.Count > 0)
                {
                    var StoreUnitsList = (
                             from items in StoreList.First().Value
                             select new
                             {
                                 Text = items.Units,
                                 Value = items.Units
                             }).Distinct().ToList();
                    return Json(StoreUnitsList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult FillMaterialGroup()
        {
            try
            {
                StoreService ss = new StoreService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<long, IList<MaterialGroupMaster>> MaterialGroupList = ss.GetMaterialGroupListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteria);
                if (MaterialGroupList != null && MaterialGroupList.First().Value != null && MaterialGroupList.First().Value.Count > 0)
                {
                    var MaterialGroup = (
                             from items in MaterialGroupList.First().Value
                             where items.MaterialGroup != null && items.MaterialGroup != ""
                             select new
                             {
                                 Text = items.MaterialGroup,
                                 Value = items.Id
                             }).Distinct().ToList().OrderBy(x => x.Text);

                    return Json(MaterialGroup, JsonRequestBehavior.AllowGet);
                }
                else
                { return Json(null, JsonRequestBehavior.AllowGet); }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult FillMaterialSubGroup(long MaterialGroupId)
        {
            try
            {
                StoreService ss = new StoreService();
                IList<MaterialSubGroupMaster> MaterialSubGroupList = ss.GetMaterialSubGroupByMaterialGroup(MaterialGroupId);
                var MatSubGroup = (
                         from items in MaterialSubGroupList
                         where items.MaterialSubGroup != null && items.MaterialSubGroup != ""
                         select new
                         {
                             Text = items.MaterialSubGroup,
                             Value = items.Id
                         }).Distinct().ToList().OrderBy(x => x.Text);

                return Json(MatSubGroup, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult FillMaterialSubGroupWithoutMaterialGroup()
        {
            try
            {
                StoreService ss = new StoreService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<long, IList<MaterialSubGroupMaster>> MaterialSubGroupList = ss.GetMaterialSubGroupListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteria);
                var MatSubGroup = (
                         from items in MaterialSubGroupList.First().Value
                         select new
                         {
                             Text = items.MaterialSubGroup,
                             Value = items.Id
                         }).Distinct().ToList();

                return Json(MatSubGroup, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult FillMaterial(long MaterialGroupId, long MaterialSubGroupId)
        {
            try
            {
                StoreService ss = new StoreService();
                IList<MaterialsMaster> MaterialsList = ss.GetMaterialByMaterialGroupAndMaterialSubGroup(MaterialGroupId, MaterialSubGroupId);
                var MaterialsListJson = (
                         from items in MaterialsList
                         where items.Material != null && items.Material != ""
                         select new
                         {
                             Value = items.Id,
                             Text = items.Material,
                         }).Distinct().ToList().OrderBy(x => x.Text);
                return Json(MaterialsListJson, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult AddSku(string reqnum, string matgrp, string matsubgrp, string mat)
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult MaterialRequestListJqGrid(string ReqNum, string ReqstDate, string cam, string ReqFor, string Mat, string ReqrdDate, string status, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    StoreService ss = new StoreService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    UserService us = new UserService();
                    //pass userid and get the list of roles from user service GetAppRoleForAnUserListWithPagingAndCriteria
                    Dictionary<long, IList<UserAppRole>> userAppRole = new Dictionary<long, IList<UserAppRole>>();
                    Dictionary<string, object> criteriaUserAppRole = new Dictionary<string, object>();
                    criteriaUserAppRole.Add("UserId", userId);
                    criteriaUserAppRole.Add("AppCode", "STR");
                    userAppRole = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteriaUserAppRole);
                    if (userAppRole != null && userAppRole.Count > 0 && userAppRole.First().Key > 0)
                    {
                        int count = userAppRole.First().Value.Count;
                        //if it has values then for each concatenate APP+ROLE 
                        string[] AppRole = new string[count];
                        string[] Roles = new string[count];
                        string[] deptCodeArr = new string[count];
                        string[] brnCodeArr = new string[count];
                        int i = 0;
                        foreach (UserAppRole uar in userAppRole.First().Value)
                        {
                            string deptCode = uar.DeptCode;
                            string branchCode = uar.BranchCode;
                            if (!string.IsNullOrWhiteSpace(uar.AppCode.Trim()) && !string.IsNullOrWhiteSpace(uar.RoleCode.Trim()))
                            {
                                AppRole[i] = uar.AppCode.Trim() + uar.RoleCode.Trim();
                            }
                            if (!string.IsNullOrWhiteSpace(uar.RoleCode.Trim()))
                            {
                                Roles[i] = uar.RoleCode.Trim();
                            }

                            if (!string.IsNullOrEmpty(deptCode))
                            {
                                deptCodeArr[i] = deptCode;
                            }
                            if (!string.IsNullOrEmpty(branchCode))
                            {
                                brnCodeArr[i] = branchCode;
                            }
                            i++;
                        }
                        brnCodeArr = brnCodeArr.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                        deptCodeArr = deptCodeArr.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                        criteria.Add("BranchCode", brnCodeArr);

                        if (status != "Assigned")
                            criteria.Add("DeptCode", deptCodeArr);
                        if (!string.IsNullOrEmpty(ReqNum))
                        {
                            criteria.Add("MaterialRequestView." + "RequestNumber", ReqNum);
                        }
                        if ((!string.IsNullOrEmpty(ReqstDate) && !(string.IsNullOrEmpty(ReqstDate.Trim()))))
                        {
                            IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                            ReqstDate = ReqstDate.Trim();
                            DateTime FromDate = DateTime.Parse(ReqstDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                            string To = string.Format("{0:MM/dd/yyyy}", FromDate);
                            DateTime ToDate = Convert.ToDateTime(To + " " + "23:59:59");
                            DateTime[] fromto = new DateTime[2];
                            fromto[0] = FromDate;
                            fromto[1] = ToDate;
                            criteria.Add("MaterialRequestView." + "RequestedDate", fromto);
                        }
                        if (!string.IsNullOrEmpty(cam))
                        {
                            criteria.Add("MaterialRequestView." + "Campus", cam);
                        }
                        if (Roles.Contains("MRC"))
                        {
                            criteria.Add("MaterialRequestView." + "ProcessedBy", userId);
                        }

                        if (!string.IsNullOrEmpty(status))
                        {
                            if (status == "Available")
                            {
                                criteria.Add("MaterialRequestView." + "RequiredForCampus", brnCodeArr);
                                criteria.Add("Available", true);
                            }
                            else if (status == "Assigned")
                            {
                                criteria.Add("Assigned", true);
                                criteria.Add("Performer", userId);
                            }
                            else if (status == "Sent")
                            {
                                criteria.Remove("DeptCode");
                                criteria.Add("Completed", true);
                            }
                            else if (status == "Completed")
                            {
                                criteria.Add("Completed", true);
                                criteria.Add("ActivityName", "Complete");
                            }
                        }
                        string[] alias = new string[1];
                        alias[0] = "MaterialRequestView";
                        sidx = "MaterialRequestView." + sidx;
                        sord = sord == "desc" ? "Desc" : "Asc";
                        //alias[1] = "MaterialRequestView.MaterialRequestList";
                        criteria.Add("TemplateId", (long)8);
                        Dictionary<long, IList<StoreMgmntActivity>> MaterialRequestlist = ss.GetMaterialRequestListWithPagingAndCriteria(page - 1, rows, sidx, sord, "AppRole", AppRole, criteria, alias);
                        if (MaterialRequestlist != null && MaterialRequestlist.Count > 0)
                        {
                            foreach (StoreMgmntActivity mt in MaterialRequestlist.First().Value)
                            {
                                mt.MaterialRequestView.DifferenceInHours = DateTime.Now - mt.MaterialRequestView.RequestedDate;
                            }
                            long totalrecords = MaterialRequestlist.First().Key;
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                            if (!string.IsNullOrWhiteSpace(Mat))
                            {
                                var jsondat1 = new
                                {

                                    total = totalPages,
                                    page = page,
                                    records = totalrecords,
                                    rows = (from items in MaterialRequestlist.First().Value
                                            where items.MaterialRequestView.MaterialRequestList.First().Material == Mat
                                            select new
                                            {
                                                i = 2,
                                                cell = new string[] {
                                                items.MaterialRequestView.Id.ToString(),
                                                items.MaterialRequestView.RequestNumber,
                                                items.MaterialRequestView.Campus,
                                                items.MaterialRequestView.ProcessedBy != null ? us.GetUserNameByUserId(items.MaterialRequestView.ProcessedBy) : "",
                                                items.MaterialRequestView.RequestedDate != null ? items.MaterialRequestView.RequestedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : null,
                                                items.MaterialRequestView.RequestStatus,
                                                items.MaterialRequestView.RequestStatus=="Completed"?"Completed":items.MaterialRequestView.DifferenceInHours.Value.TotalHours.ToString(),
                                                items.Id.ToString(),
                                                items.ActivityName,
                                                items.ActivityFullName,
                                                items.MaterialRequestView.ProcessedBy
                                                }
                                            })
                                };
                                return Json(jsondat1, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                var jsondat1 = new
                                {
                                    total = totalPages,
                                    page = page,
                                    records = totalrecords,
                                    rows = (from items in MaterialRequestlist.First().Value
                                            select new
                                            {
                                                i = 2,
                                                cell = new string[] {
                                   items.MaterialRequestView.Id.ToString(),
                                   items.MaterialRequestView.RequestNumber,
                                   items.MaterialRequestView.Campus,    
                                   items.MaterialRequestView.ProcessedBy != null ? us.GetUserNameByUserId(items.MaterialRequestView.ProcessedBy) : "",
                                   items.MaterialRequestView.RequestedDate != null ? items.MaterialRequestView.RequestedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : null,
                                   items.MaterialRequestView.RequestStatus,
                                   items.MaterialRequestView.RequestStatus=="Completed"?"Completed":items.MaterialRequestView.DifferenceInHours.Value.TotalHours.ToString(),
                                   items.Id.ToString(),
                                   items.ActivityName,
                                   items.ActivityFullName,
                                   items.MaterialRequestView.ProcessedBy,
                            }
                                            })
                                };
                                return Json(jsondat1, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        { return null; }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult ActOnMaterialRequest(long? id, long? activityId, string activityName, string ActivityFullName)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    ViewBag.ActivityId = activityId ?? 0;
                    if (id > 0)
                    {
                        StoreService ss = new StoreService(); // TODO: Initialize to an appropriate value
                        UserService us = new UserService();
                        ProcessFlowServices pfs = new ProcessFlowServices();
                        ViewBag.flag = 1;
                        //pass activity id and userid to assign activity to user
                        string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                        pfs.AssignActivity((Convert.ToInt64(activityId)), userid);
                        MaterialRequest mr = ss.GetMaterialRequestById(Convert.ToInt64(id));
                        mr.RequestStatus = !string.IsNullOrWhiteSpace(activityName) ? activityName : mr.RequestStatus;
                        mr.CreatedUserName = us.GetUserNameByUserId(mr.ProcessedBy);

                        Dictionary<string, object> criteriaUserAppRole = new Dictionary<string, object>();
                        criteriaUserAppRole.Add("UserId", userId);
                        criteriaUserAppRole.Add("AppCode", "STR");
                        Dictionary<long, IList<UserAppRole>> userAppRole = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteriaUserAppRole);
                        if (userAppRole != null && userAppRole.Count > 0 && userAppRole.First().Key > 0)
                        {
                            int count = userAppRole.First().Value.Count;
                            string[] Roles = new string[count];
                            int i = 0;
                            foreach (UserAppRole uar in userAppRole.First().Value)
                            {
                                if (!string.IsNullOrWhiteSpace(uar.RoleCode.Trim()))
                                {
                                    Roles[i] = uar.RoleCode.Trim();
                                }
                                i++;
                            }
                            if (Roles.Contains("MRC"))
                            { ViewBag.Role = "MRC"; }
                        }

                        return View(mr);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
            return View();
        }

        public ActionResult ShowMaterialRequest(long? id, long? activityId, string activityName, string ActivityFullName)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    ViewBag.ActivityId = activityId ?? 0;
                    if (id > 0)
                    {
                        StoreService ss = new StoreService(); // TODO: Initialize to an appropriate value
                        ProcessFlowServices pfs = new ProcessFlowServices();
                        ViewBag.flag = 1;
                        //pass activity id and userid to assign activity to user
                        string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                        pfs.AssignActivity((Convert.ToInt64(activityId)), userid);
                        //pass callmgmnt id to get call mgmnt object
                        MaterialRequest mr = ss.GetMaterialRequestById(Convert.ToInt64(id));
                        mr.RequestStatus = !string.IsNullOrWhiteSpace(activityName) ? activityName : mr.RequestStatus;
                        //cm.ActivityFullName = !string.IsNullOrWhiteSpace(ActivityFullName) ? ActivityFullName : cm.ActivityFullName;
                        UserService us = new UserService();
                        mr.CreatedUserName = us.GetUserNameByUserId(mr.ProcessedBy);
                        return View(mr);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }

            return View();
        }

        public ActionResult Test()
        {
            return View();
        }

        #region "Material Inward"
        public ActionResult MaterialInward(long? Id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    UserService us = new UserService();
                    StoreService ss = new StoreService();
                    Dictionary<string, object> Criteria = new Dictionary<string, object>();
                    MaterialInward mi = new MaterialInward();
                    if (Id > 0)
                    {
                        mi = ss.GetMaterialInwardById(Convert.ToInt64(Id));
                        mi.CreatedUserName = us.GetUserNameByUserId(mi.ProcessedBy);
                    }
                    else
                    {
                        Criteria.Add("UserId", userId);
                        Criteria.Add("AppCode", "STR");
                        Dictionary<long, IList<UserAppRole>> UserAppRoleList = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, Criteria);
                        var UserDetails = (from u in UserAppRoleList.First().Value
                                           where u.UserId == userId
                                           select new { u.RoleName, u.BranchCode }).ToArray();

                        User user = (User)Session["objUser"];
                        if (user != null)
                            mi.Campus = user.Campus;

                        mi.UserRole = UserDetails[0].RoleName;
                        mi.Status = "Open";
                        mi.ProcessedBy = userId;
                        mi.InvoiceDate = DateTime.Now;
                        mi.DCDate = DateTime.Now;
                        mi.ReceivedDateTime = DateTime.Now;
                        mi.CreatedUserName = user.UserName;
                    }
                    return View(mi);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        //public ActionResult StartMaterialInward(MaterialInward m)
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //        else
        //        {
        //            long Id = 0;
        //            StoreService ss = new StoreService();
        //            m.CreatedDate = DateTime.Now;
        //            Id = ss.CreateOrUpdateMaterialInward(m);
        //            m.InwardNumber = "MIN-" + Id;

        //            ss.CreateOrUpdateMaterialInward(m);
        //            return Json(m.InwardNumber, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception ex)
        //    { return ThrowJSONErrorNew(ex, "StoreMgmntPolicy"); }
        //}
        public ActionResult StartMaterialInward(string Id, string ProcessedBy, string UserRole, string Status, string Campus, string PoNumber, string Supplier,
            string SuppRefNo, string ReceivedBy, string ReceivedDateTime, string Store, string InvoiceDate, string InvoiceAmount, string DCNumber, string DCDate, string VehicleType,
            string VehicleNo, string DriverName, string DriverContactNo)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    long Id1 = 0;
                    StoreService ss = new StoreService();
                    MaterialInward m = new MaterialInward();
                    m.Id = Convert.ToInt64(Id);
                    m.ProcessedBy = ProcessedBy;
                    m.UserRole = UserRole;
                    m.Status = Status;
                    m.Campus = Campus;
                    m.PONumber = PoNumber;
                    m.Supplier = Supplier;
                    m.SuppRefNo = SuppRefNo;
                    m.ReceivedBy = ReceivedBy;
                    m.ReceivedDateTime = DateTime.Parse(ReceivedDateTime, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    m.Store = Store;
                    m.InvoiceDate = DateTime.Parse(InvoiceDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    m.InvoiceAmount = InvoiceAmount;
                    m.DCNumber = DCNumber;
                    m.DCDate = DateTime.Parse(DCDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    m.CreatedDate = DateTime.Now;
                    m.VehicleType = VehicleType;
                    m.VehicleNo = VehicleNo;
                    m.DriverName = DriverName;
                    m.DriverContactNo = DriverContactNo;
                    string info = string.Empty;
                    info = " You have Updated an stock with inward number " + m.InwardNumber + " with DC number " + m.DCNumber;
                    UpdateInbox(m.Campus, info, userId, m.Id);
                    Id1 = ss.CreateOrUpdateMaterialInward(m);
                    m.InwardNumber = "MIN-" + Id1;
                    ss.CreateOrUpdateMaterialInward(m);
                    return Json(m.InwardNumber, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            { return ThrowJSONErrorNew(ex, "StoreMgmntPolicy"); }
        }

        public ActionResult CompleteMaterialInward(MaterialInward m)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    StoreService ss = new StoreService();
                    m = ss.GetMaterialInwardById(m.Id);
                    m.Status = "Completed";
                    ss.CreateOrUpdateMaterialInward(m);
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("MaterialRefId", m.Id);
                    Dictionary<long, IList<SkuList>> MaterialSkulist = ss.GetSkulistWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                    if (MaterialSkulist != null && MaterialSkulist.Count > 0 && MaterialSkulist.First().Key > 0)
                    {
                        for (int i = 0; i < MaterialSkulist.First().Key; i++)
                        {
                            MaterialsMaster mm = ss.GetMaterialsMasterByMaterial(MaterialSkulist.First().Value[i].Material);
                            if (mm != null)
                            {
                                StockTransaction st = new StockTransaction();
                                st.TransactionCode = m.InwardNumber;
                                st.Store = m.Store;
                                st.ItemId = mm.Id;
                                st.Units = MaterialSkulist.First().Value[i].ReceivedUnits;
                                st.TransactionDate = DateTime.Now;
                                st.TransactionBy = m.ProcessedBy;
                                st.TransactionType = "Material Inward";
                                st.Qty = Convert.ToInt32(MaterialSkulist.First().Value[i].ReceivedQty - MaterialSkulist.First().Value[i].DamagedQty);
                                st.DamagedQty = Convert.ToInt32(MaterialSkulist.First().Value[i].DamagedQty);
                                // st.DamagedRemarks = MaterialSkulist.First().Value[i].DamageDescription;
                                st.TransactionComments = MaterialSkulist.First().Value[i].DamageDescription;
                                string info = string.Empty;
                                info = " You have Updated an stock with inward number " + m.InwardNumber + " with DC number " + m.DCNumber;
                                UpdateInbox(m.Campus, info, userId, m.Id);
                                ss.CreateOrUpdateStockTransaction(st);
                            }
                        }
                        return Json(m.InwardNumber, JsonRequestBehavior.AllowGet);
                    }
                    else
                    { return Json(null); }
                }
            }
            catch (Exception ex)
            { return ThrowJSONErrorNew(ex, "StoreMgmntPolicy"); }
        }
        public ActionResult AddSKUList(IList<SkuList> skuLst)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    StoreService ss = new StoreService();
                    if (skuLst != null)
                        ss.CreateOrUpdateSKUList(skuLst);
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            { return ThrowJSONErrorNew(ex, "StoreMgmntPolicy"); }
        }

        //public ActionResult MaterialSkuListJqGrid(string Store, long? Id, int rows, string sidx, string sord, int? page = 1)
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) { return RedirectToAction("LogOff", "Account"); }
        //        else
        //        {
        //            StoreService ss = new StoreService();
        //            Dictionary<string, object> criteria = new Dictionary<string, object>();
        //            sord = sord == "desc" ? sord = "Desc" : sord = "Asc";
        //            if (Id > 0)
        //            {
        //                criteria.Add("MaterialRefId", Id);
        //                Dictionary<long, IList<SkuList>> MaterialSkulist = ss.GetSkulistWithPagingAndCriteria(page - 1, rows, sord, sidx, criteria);

        //                if (MaterialSkulist != null && MaterialSkulist.Count > 0 && MaterialSkulist.FirstOrDefault().Value != null && MaterialSkulist.FirstOrDefault().Value.Count > 0)
        //                {
        //                    foreach (var item in MaterialSkulist.FirstOrDefault().Value)
        //                    {
        //                        Dictionary<string, object> criteria1 = new Dictionary<string, object>();
        //                        criteria1.Clear();
        //                        criteria1.Add("Material", item.Material.Trim());
        //                        criteria1.Add("MaterialInward." + "Store", Store);
        //                        // string[] IssuedStatus = new[] { "Not Issued", "Partially Issued" };
        //                        // criteria1.Add("IssuedStatus", IssuedStatus);
        //                        string[] alias = new string[1];
        //                        alias[0] = "MaterialInward";
        //                        Dictionary<long, IList<SkuList_MaterialInward>> SkuList = ss.GetSkuList_MaterialInwardListWithPagingAndCriteria(0, 9999, "SkuId", "Desc", criteria1, "", null, alias);
        //                        if (SkuList != null && SkuList.FirstOrDefault().Value != null && SkuList.Count() > 0 && SkuList.FirstOrDefault().Key > 0)
        //                        {
        //                            for (int j = 0; j < SkuList.FirstOrDefault().Value.Count; j++)
        //                            {
        //                                item.OldPrices = item.OldPrices + Convert.ToString(SkuList.First().Value[j].UnitPrice) + ", ";
        //                                if (j == 3)
        //                                    break;
        //                            }
        //                        }
        //                    }
        //                    long totalrecords = MaterialSkulist.First().Key;
        //                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
        //                    var jsondat1 = new
        //                    {
        //                        total = totalPages,
        //                        page = page,
        //                        records = totalrecords,

        //                        rows = (from items in MaterialSkulist.First().Value
        //                                select new
        //                                {
        //                                    i = 2,
        //                                    cell = new string[] {
        //                       items.SkuId.ToString(),
        //                       items.MaterialRefId.ToString(),
        //                       items.Material,
        //                       items.MaterialGroup,
        //                       items.MaterialSubGroup,
        //                       items.OrderedUnits,
        //                       items.ReceivedUnits,
        //                       items.OldPrices,
        //                       items.OrderQty.ToString(),
        //                       items.ReceivedQty.ToString(),
        //                       items.DamagedQty!=null?items.DamagedQty.ToString():null,
        //                       items.UnitPrice.ToString(),
        //                       items.TotalPrice.ToString(),
        //                       items.DamageDescription,
        //                    }
        //                                })
        //                    };
        //                    return Json(jsondat1, JsonRequestBehavior.AllowGet);
        //                }
        //            }
        //            else
        //            { return Json(null, JsonRequestBehavior.AllowGet); }
        //        }
        //        return Json(null, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
        //        throw ex;
        //    }
        //}

        public ActionResult MaterialSkuListJqGrid(string Store, long? Id, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) { return RedirectToAction("LogOff", "Account"); }
                else
                {
                    StoreService ss = new StoreService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    sord = sord == "desc" ? sord = "Desc" : sord = "Asc";
                    if (Id > 0)
                    {
                        criteria.Add("MaterialRefId", Id);
                        Dictionary<long, IList<SkuList>> MaterialSkulist = ss.GetSkulistWithPagingAndCriteria(page - 1, rows, sord, sidx, criteria);

                        if (MaterialSkulist != null && MaterialSkulist.Count > 0 && MaterialSkulist.FirstOrDefault().Value != null && MaterialSkulist.FirstOrDefault().Value.Count > 0)
                        {
                            foreach (var item in MaterialSkulist.FirstOrDefault().Value)
                            {
                                Dictionary<string, object> criteria1 = new Dictionary<string, object>();
                                criteria1.Clear();
                                criteria1.Add("Material", item.Material.Trim());
                                criteria1.Add("MaterialInward." + "Store", Store);
                                // string[] IssuedStatus = new[] { "Not Issued", "Partially Issued" };
                                // criteria1.Add("IssuedStatus", IssuedStatus);
                                string[] alias = new string[1];
                                alias[0] = "MaterialInward";
                                Dictionary<long, IList<SkuList_MaterialInward>> SkuList = ss.GetSkuList_MaterialInwardListWithPagingAndCriteria(0, 9999, "SkuId", "Asc", criteria1, "", null, alias);
                                if (SkuList != null && SkuList.FirstOrDefault().Value != null && SkuList.Count() > 0 && SkuList.FirstOrDefault().Key > 0)
                                {
                                    int incre = 0;
                                    for (int j = SkuList.FirstOrDefault().Value.Count; j >= 0; j--)
                                    {
                                        item.OldPrices = item.OldPrices + Convert.ToString(SkuList.First().Value[j].UnitPrice);
                                        incre++;
                                        if (incre == 3)
                                            break;
                                        else
                                            item.OldPrices = item.OldPrices + ", ";
                                    }
                                }
                            }
                            long totalrecords = MaterialSkulist.First().Key;
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var jsondat1 = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalrecords,

                                rows = (from items in MaterialSkulist.First().Value
                                        select new
                                        {
                                            i = 2,
                                            cell = new string[] {
                               items.SkuId.ToString(),
                               items.MaterialRefId.ToString(),
                               items.Material,
                               items.MaterialGroup,
                               items.MaterialSubGroup,
                               items.OrderedUnits,
                               items.ReceivedUnits,
                               items.OldPrices,
                               items.OrderQty.ToString(),
                               items.ReceivedQty.ToString(),
                               items.DamagedQty!=null?items.DamagedQty.ToString():null,
                               items.UnitPrice.ToString(),
                               items.Tax.ToString(),
                               items.Discount.ToString(),
                               items.TotalPrice.ToString(),
                               items.DamageDescription,
                            }
                                        })
                            };
                            return Json(jsondat1, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    { return Json(null, JsonRequestBehavior.AllowGet); }
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult ShowMaterialInward(long? Id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    StoreService ss = new StoreService();
                    Dictionary<string, object> Criteria = new Dictionary<string, object>();
                    MaterialInward mi = new MaterialInward();
                    if (Id > 0)
                    {
                        mi = ss.GetMaterialInwardById(Convert.ToInt64(Id));
                        mi.CreatedUserName = us.GetUserNameByUserId(mi.ProcessedBy);
                        mi.ReceivedByName = mi.ReceivedBy != null ? us.GetUserNameByUserId(mi.ReceivedBy) : "";
                    }
                    return View(mi);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }


        #endregion "Material Inward"

        #region "Master Data Entry"

        //public ActionResult StoreUnitsMaster()
        //{
        //    try
        //    {
        //        return PartialView();
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
        //        throw ex;
        //    }
        //}

        public ActionResult StoreUnitsListJqGrid(string UnitCode, string Units, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                StoreService ds = new StoreService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrWhiteSpace(UnitCode)) { criteria.Add("UnitCode", UnitCode); }
                if (!string.IsNullOrWhiteSpace(Units)) { criteria.Add("Units", Units); }
                sord = sord == "desc" ? "Desc" : "Asc";
                Dictionary<long, IList<StoreUnits>> StoreUnits = ds.GetStoreUnitslistWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (StoreUnits != null && StoreUnits.Count > 0)
                {
                    long totalrecords = StoreUnits.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat1 = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,

                        rows = (from items in StoreUnits.First().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[] {
                               items.Id.ToString(),items.UnitCode,items.Units
                            }
                                })
                    };
                    return Json(jsondat1, JsonRequestBehavior.AllowGet);
                }
                else { return Json(null); }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddStoreUnits(StoreUnits su, string test)
        {
            try
            {
                su.UnitCode = su.UnitCode.Trim();
                StoreService ss = new StoreService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Units", su.Units);
                criteria.Add("UnitCode", su.UnitCode);
                Dictionary<long, IList<StoreUnits>> StoreUnits = ss.GetStoreUnitslistWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                if (test == "edit")
                {
                    if (StoreUnits != null && StoreUnits.First().Value != null && StoreUnits.First().Value.Count > 1)
                    {
                        //var script = @"ErrMsg(""Already Exists"");";
                        //return JavaScript(script);
                        return null;
                    }
                    else
                    {
                        ViewBag.flag = 1;
                        ss.CreateOrUpdateStoreUnitsMaster(su);
                        return null;
                    }
                }
                else
                {
                    if (StoreUnits != null && StoreUnits.First().Value != null && StoreUnits.First().Value.Count > 0)
                    {
                        var script = @"ErrMsg(""Already Exists"");";
                        return JavaScript(script);
                    }
                    else
                    {
                        su.Id = 0;
                        ViewBag.flag = 1;
                        long id = ss.CreateOrUpdateStoreUnitsMaster(su);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        //public ActionResult StoreSKUMaster()
        //{
        //    try
        //    {
        //        return View();
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
        //        throw ex;
        //    }
        //}

        public ActionResult StoreSKUMasterListJqGrid(string MaterialGroup, string MaterialSubGroup, string Material, string UnitCode, string ItemCode, string ItemLocation, string Notes, string IsActive, string ExptType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                StoreService ds = new StoreService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrWhiteSpace(MaterialGroup)) { criteria.Add("MaterialGroup", MaterialGroup); }
                if (!string.IsNullOrWhiteSpace(MaterialSubGroup)) { criteria.Add("MaterialSubGroup", MaterialSubGroup); }
                if (!string.IsNullOrWhiteSpace(Material)) { criteria.Add("Material", Material); }
                if (!string.IsNullOrWhiteSpace(UnitCode)) { criteria.Add("UnitCode", UnitCode); }
                if (!string.IsNullOrWhiteSpace(ItemCode)) { criteria.Add("ItemCode", ItemCode); }
                if (!string.IsNullOrWhiteSpace(ItemLocation)) { criteria.Add("ItemLocation", ItemLocation); }
                if (!string.IsNullOrWhiteSpace(Notes)) { criteria.Add("Notes", Notes); }
                if (!string.IsNullOrWhiteSpace(IsActive))
                {
                    if (IsActive != "All")
                    {
                        bool isact = Convert.ToBoolean(IsActive);
                        criteria.Add("IsActive", isact);
                    }
                }
                sord = sord == "desc" ? "Desc" : "Asc";
                Dictionary<long, IList<MaterialsMaster_vw>> SKUList = ds.GetMaterialsMasterlistWithPagingAndCriteriaUsingView(page - 1, rows, sord, sidx, criteria);
                if (SKUList != null && SKUList.Count > 0)
                {
                    if (ExptType == "Excel")
                    {
                        var List = SKUList.First().Value.ToList();
                        base.ExptToXL(List, "MaterialsList", (items => new
                        {
                            items.Id,
                            items.MaterialGroup,
                            items.MaterialSubGroup,
                            items.Material,
                            items.UnitCode,
                            ItemCode = items.ItemCode != null ? items.ItemCode.ToUpper() : null,
                            items.ItemLocation,
                            items.Notes,
                            items.IsActive
                        }));
                        return new EmptyResult();
                    }
                    else
                    {
                        long totalrecords = SKUList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat1 = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,

                            rows = (from items in SKUList.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                               items.Id.ToString(),items.MaterialGroupId.ToString(),items.MaterialSubGroupId.ToString(),items.MaterialGroup,items.MaterialSubGroup,items.Material,items.UnitCode,
                               items.ItemCode!=null?items.ItemCode.ToUpper():null,
                               items.ItemLocation,items.Notes,items.IsActive.ToString()
                            }
                                    })
                        };
                        return Json(jsondat1, JsonRequestBehavior.AllowGet);
                    }
                }
                else return Json(null);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }
        public ActionResult StoreSKUListJqGrid(string MaterialGroup, string MaterialSubGroup, string Material, string Units, string Store, long? MaterialGroupId, long? MaterialSubGroupId, long? MaterialId, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                StoreService ds = new StoreService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrWhiteSpace(MaterialGroup)) { criteria.Add("MaterialGroup", MaterialGroup); }
                if (!string.IsNullOrWhiteSpace(MaterialSubGroup)) { criteria.Add("MaterialSubGroup", MaterialSubGroup); }
                if (!string.IsNullOrWhiteSpace(Material)) { criteria.Add("Material", Material); }
                if (!string.IsNullOrWhiteSpace(Units)) { criteria.Add("UnitCode", Units); }
                if (!string.IsNullOrWhiteSpace(Store)) { criteria.Add("Store", Store); }
                if (MaterialGroupId > 0)
                    criteria.Add("MaterialGroupId", MaterialGroupId);
                if (MaterialSubGroupId > 0)
                    criteria.Add("MaterialSubGroupId", MaterialSubGroupId);
                if (MaterialId > 0)
                    criteria.Add("MaterialId", MaterialId);
                criteria.Add("IsActive", true);
                sord = sord == "desc" ? "Desc" : "Asc";
                Dictionary<long, IList<MaterialsMaster_vw_Stock_vw>> MaterialsMasterList = ds.GetMaterialsMasterAndStockBalancelistWithPagingAndCriteriaUsingView(page - 1, rows, sidx, sord, criteria);
                if (MaterialsMasterList != null && MaterialsMasterList.Count > 0)
                {
                    long totalrecords = MaterialsMasterList.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat1 = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,

                        rows = (from items in MaterialsMasterList.First().Value
                                where items.MaterialGroup != null && items.MaterialSubGroup != null && items.Material != null && items.UnitCode != null
                                select new
                                {
                                    cell = new string[] {
                               items.Id.ToString(),items.MaterialGroup,items.MaterialSubGroup,items.Material,items.UnitCode,items.Store,items.ClosingBalance.ToString()
                            }
                                })
                    };
                    return Json(jsondat1, JsonRequestBehavior.AllowGet);

                }
                else return Json(null);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult StoreSKUListJqGridForMaterialInward(string MaterialGroup, string MaterialSubGroup, string Material, string Units, long? MaterialGroupId, long? MaterialSubGroupId, long? MaterialId, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                StoreService ds = new StoreService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrWhiteSpace(MaterialGroup)) { criteria.Add("MaterialGroup", MaterialGroup); }
                if (!string.IsNullOrWhiteSpace(MaterialSubGroup)) { criteria.Add("MaterialSubGroup", MaterialSubGroup); }
                if (!string.IsNullOrWhiteSpace(Material)) { criteria.Add("Material", Material); }
                if (!string.IsNullOrWhiteSpace(Units)) { criteria.Add("UnitCode", Units); }
                if (MaterialGroupId > 0)
                    criteria.Add("MaterialGroupId", MaterialGroupId);
                if (MaterialSubGroupId > 0)
                    criteria.Add("MaterialSubGroupId", MaterialSubGroupId);
                if (MaterialId > 0)
                    criteria.Add("Id", MaterialId);
                criteria.Add("IsActive", true);
                sord = sord == "desc" ? "Desc" : "Asc";
                Dictionary<long, IList<MaterialsMaster_vw>> MaterialsMasterList = ds.GetMaterialsMasterlistWithPagingAndCriteriaUsingView(page - 1, rows, sord, sidx, criteria);
                if (MaterialsMasterList != null && MaterialsMasterList.Count > 0)
                {
                    long totalrecords = MaterialsMasterList.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat1 = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,

                        rows = (from items in MaterialsMasterList.First().Value
                                where items.MaterialGroup != null && items.MaterialSubGroup != null && items.Material != null && items.UnitCode != null
                                select new
                                {
                                    i = 2,
                                    cell = new string[] {
                               items.Id.ToString(),items.MaterialGroup,items.MaterialSubGroup,items.Material,items.UnitCode
                            }
                                })
                    };
                    return Json(jsondat1, JsonRequestBehavior.AllowGet);

                }
                else return Json(null);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddMaterial(MaterialsMaster mm, string test)
        {
            try
            {
                mm.Material = mm.Material.Trim();
                StoreService ss = new StoreService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();

                MaterialGroupMaster mgm = new MaterialGroupMaster();
                mgm = ss.GetMaterialGroupById(mm.MaterialGroupId);
                string mg = mgm.MatGrpCode;
                Dictionary<string, object> criteria1 = new Dictionary<string, object>();
                criteria1.Add("Id", mm.MaterialSubGroupId);
                Dictionary<long, IList<MaterialSubGroupMaster>> materialsubgrouplist = ss.GetMaterialSubGroupListWithPagingAndCriteria(0, 9999, null, null, criteria1);
                string msg = materialsubgrouplist.First().Value[0].MatSubGrpCode;

                string mgsplitted = mg.Substring(0, 3);
                string msgsplitted = msg.Substring(0, 3);

                criteria.Add("MaterialGroupId", mm.MaterialGroupId);
                criteria.Add("MaterialSubGroupId", mm.MaterialSubGroupId);
                criteria.Add("Material", mm.Material);
                Dictionary<long, IList<MaterialsMaster>> StoreUnits = ss.GetMaterialsMasterlistWithPagingAndCriteria(0, 9999, null, null, criteria);
                if (test == "edit")
                {
                    if (StoreUnits != null && StoreUnits.First().Value != null && StoreUnits.First().Value.Count > 1)
                    {
                        //var script = @"ErrMsg(""Already Exists"");";
                        //return JavaScript(script);
                        return null;
                    }
                    else
                    {
                        ViewBag.flag = 1;
                        ss.CreateOrUpdateMaterialsMaster(mm);
                        return null;
                    }
                }
                else
                {
                    if (StoreUnits != null && StoreUnits.First().Value != null && StoreUnits.First().Value.Count > 0)
                    {
                        var script = @"ErrMsg(""Already Exists"");";
                        return JavaScript(script);
                    }
                    else
                    {
                        mm.Id = 0;
                        ViewBag.flag = 1;
                        long id = ss.CreateOrUpdateMaterialsMaster(mm);
                        mm.ItemCode = mgsplitted.ToUpper() + msgsplitted.ToUpper() + "000" + id;
                        ss.CreateOrUpdateMaterialsMaster(mm);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }
        //public ActionResult StoreMaterialGroupMaster()
        //{
        //    try
        //    {
        //        return PartialView();
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
        //        throw ex;
        //    }
        //}

        public ActionResult StoreMaterialGroupMasterListJqGrid(string MaterialGroup, string MatGrpCode, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                StoreService ds = new StoreService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrWhiteSpace(MaterialGroup)) { criteria.Add("MaterialGroup", MaterialGroup); }
                if (!string.IsNullOrWhiteSpace(MatGrpCode)) { criteria.Add("MatGrpCode", MatGrpCode); }

                sord = sord == "desc" ? "Desc" : "Asc";
                Dictionary<long, IList<MaterialGroupMaster>> MaterialGroupMaster = ds.GetMaterialGroupListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (MaterialGroupMaster != null && MaterialGroupMaster.Count > 0)
                {
                    long totalrecords = MaterialGroupMaster.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat1 = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,

                        rows = (from items in MaterialGroupMaster.First().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[] {
                               items.Id.ToString(),items.MaterialGroup,items.MatGrpCode
                            }
                                })
                    };
                    return Json(jsondat1, JsonRequestBehavior.AllowGet);

                }
                else return Json(null);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddMaterialGroupMaster(MaterialGroupMaster mgm, string test)
        {
            try
            {
                mgm.MaterialGroup = mgm.MaterialGroup.Trim();
                StoreService ss = new StoreService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("MaterialGroup", mgm.MaterialGroup);
                criteria.Add("MatGrpCode", mgm.MatGrpCode);
                Dictionary<long, IList<MaterialGroupMaster>> Matgrp = ss.GetMaterialGroupListWithPagingAndCriteria(0, 9999, null, null, criteria);
                if (Matgrp != null && Matgrp.First().Value != null && (Matgrp.First().Value.Count > 1 || Matgrp.First().Value.Count > 0))
                {
                    var script = @"ErrMsg(""Already Exists"");";
                    return JavaScript(script);
                }

                if (test != "edit")
                { mgm.Id = 0; }
                ViewBag.flag = 1;
                mgm.MatGrpCode = mgm.MatGrpCode.ToUpper();
                mgm.MaterialGroup = mgm.MaterialGroup.Trim();
                ss.CreateOrUpdateStoreMaterialGroupMaster(mgm);
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult MaterialGroupddl()
        {
            try
            {
                StoreService rs = new StoreService();
                Dictionary<long, string> regi = new Dictionary<long, string>();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<long, IList<MaterialGroupMaster>> storematerialslist = rs.GetMaterialGroupListWithPagingAndCriteria(0, 9999, null, null, criteria);
                var MaterialGroup = (from u in storematerialslist.First().Value
                                     select u.MaterialGroup).Distinct().ToList();
                return Json(MaterialGroup, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult MaterialSubGroupddl(long? MaterialGroupId)
        {
            try
            {
                StoreService ss = new StoreService();
                Dictionary<long, string> regi = new Dictionary<long, string>();
                IList<MaterialSubGroupMaster> MaterialSubGroupList = ss.GetMaterialSubGroupByMaterialGroup(Convert.ToInt64(MaterialGroupId));
                var MaterialSubGroup =
                                       (from items in MaterialSubGroupList
                                        select new
                                        {
                                            Text = items.MaterialSubGroup,
                                            Value = items.Id
                                        }).Distinct().ToList();
                return Json(MaterialSubGroup, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }
        public ActionResult StoreMaster()
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
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        //public ActionResult StorerFunctionaries()
        //{
        //    try
        //    {
        //        return PartialView();
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
        //        throw ex;
        //    }
        //}

        public ActionResult StoreSupplierListJqGrid1(string SupplierName, string CompanyName, string MobileNumber, string PhoneNumber, string Email, string TINNumber, string PANNumber, string IsPreferredSupplier, string Notes
            , string IsActive, string ExptType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                StoreService ds = new StoreService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrWhiteSpace(SupplierName)) { criteria.Add("SupplierName", SupplierName); }
                if (!string.IsNullOrWhiteSpace(CompanyName)) { criteria.Add("CompanyName", CompanyName); }
                if (!string.IsNullOrWhiteSpace(MobileNumber)) { criteria.Add("MobileNumber", MobileNumber); }
                if (!string.IsNullOrWhiteSpace(PhoneNumber)) { criteria.Add("PhoneNumber", PhoneNumber); }
                if (!string.IsNullOrWhiteSpace(Email)) { criteria.Add("Email", Email); }
                if (!string.IsNullOrWhiteSpace(TINNumber)) { criteria.Add("TINNumber", TINNumber); }
                if (!string.IsNullOrWhiteSpace(PANNumber)) { criteria.Add("PANNumber", PANNumber); }
                if (!string.IsNullOrWhiteSpace(IsPreferredSupplier))
                {
                    if (IsPreferredSupplier != "All")
                    {
                        bool ispre = Convert.ToBoolean(IsPreferredSupplier); criteria.Add("IsPreferredSupplier", ispre);
                    }
                }
                if (!string.IsNullOrWhiteSpace(Notes)) { criteria.Add("Notes", Notes); }
                if (!string.IsNullOrWhiteSpace(IsActive))
                {
                    if (IsActive != "All")
                    {
                        bool isact = Convert.ToBoolean(IsActive); criteria.Add("IsActive", isact);
                    }
                }
                sord = sord == "desc" ? "Desc" : "Asc";
                Dictionary<long, IList<StoreSupplierMaster>> storesuppliermasterlist = ds.GetStoreSupplierMasterlistWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (storesuppliermasterlist != null && storesuppliermasterlist.Count > 0)
                {
                    if (ExptType == "Excel")
                    {
                        var List = storesuppliermasterlist.First().Value.ToList();
                        base.ExptToXL(List, "StoreSupplierList", (items => new
                        {
                            items.SupplierName,
                            items.CompanyName,
                            items.PANNumber,
                            items.MobileNumber,
                            items.PhoneNumber,
                            items.Email,
                            items.City,
                            items.State,
                            items.ZipCode,
                            items.Country,
                            items.Type,
                            items.TINNumber,
                            items.CreditTerms,
                            items.IsPreferredSupplier,
                            items.IsActive,
                            items.Address,
                            items.Notes,
                        }));
                        return new EmptyResult();
                    }
                    else
                    {
                        long totalrecords = storesuppliermasterlist.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat1 = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,

                            rows = (from items in storesuppliermasterlist.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                               items.SupplierName,
                               items.CompanyName,
                               items.PANNumber,
                               items.MobileNumber,
                               items.PhoneNumber,
                               items.Email,
                               items.City,
                               items.State,
                               items.ZipCode.ToString(),
                               items.Country,
                               items.Type,
                               items.TINNumber,
                               items.CreditTerms,
                               items.IsPreferredSupplier.ToString(),
                               items.IsActive.ToString(),
                               items.Address,
                               items.Notes,
                               items.Id.ToString(),
                               items.FormCode,
                               items.FormCode,
                            }
                                    })
                        };
                        return Json(jsondat1, JsonRequestBehavior.AllowGet);
                    }
                }
                else return Json(null);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddStoreSupplier(StoreSupplierMaster ssm, string test)
        {
            try
            {
                ssm.SupplierName = ssm.SupplierName.Trim();
                StoreService ss = new StoreService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("SupplierName", ssm.SupplierName);
                Dictionary<long, IList<StoreSupplierMaster>> storesupplier = ss.GetStoreSupplierMasterlistWithPagingAndCriteria(0, 9999, null, null, criteria);
                if (test == "edit")
                {
                    if (storesupplier != null && storesupplier.First().Value != null && storesupplier.First().Value.Count > 1)
                    {
                        //var script = @"ErrMsg(""Already Exists"");";
                        //return JavaScript(script);
                        return null;
                    }
                    else
                    {
                        ViewBag.flag = 1;
                        ss.CreateOrUpdateStoreSupplierMaster(ssm);
                        return null;
                    }
                }
                else
                {
                    if (storesupplier != null && storesupplier.First().Value != null && storesupplier.First().Value.Count > 0)
                    {
                        var script = @"ErrMsg(""Already Exists"");";
                        return JavaScript(script);
                    }
                    else
                    {
                        ssm.Id = 0;
                        ssm.FormCode = "nil";
                        ViewBag.flag = 1;
                        long id = ss.CreateOrUpdateStoreSupplierMaster(ssm);
                        ssm.Id = id;
                        ssm.FormCode = "SS-" + ssm.Id;
                        ss.CreateOrUpdateStoreSupplierMaster(ssm);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddMaterialGroupSupplier(MaterialGroupSupplier mgs, string test)
        {
            try
            {
                StoreService ss = new StoreService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("SupplierId", mgs.SupplierId);
                criteria.Add("MaterialGroup", mgs.MaterialGroup);
                Dictionary<long, IList<MaterialGroupSupplier>> storesupplier = ss.GetMaterialGroupSupplierlistWithPagingAndCriteria(0, 9999, null, null, criteria);
                if (test == "edit")
                {
                    if (storesupplier != null && storesupplier.First().Value != null && storesupplier.First().Value.Count > 1)
                    {
                        //var script = @"ErrMsg(""Already Exists"");";
                        //return JavaScript(script);
                        return null;
                    }
                    else
                    {
                        ViewBag.flag = 1;
                        ss.CreateOrUpdateMaterialGroupSupplier(mgs);
                        return null;
                    }
                }
                else
                {
                    if (storesupplier != null && storesupplier.First().Value != null && storesupplier.First().Value.Count > 0)
                    {
                        var script = @"ErrMsg(""Already Exists"");";
                        return JavaScript(script);
                    }
                    else
                    {
                        mgs.Id = 0;
                        ViewBag.flag = 1;
                        long id = ss.CreateOrUpdateMaterialGroupSupplier(mgs);
                        mgs.Id = id;
                        ss.CreateOrUpdateMaterialGroupSupplier(mgs);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        #endregion "Master Data Entry"

        public ActionResult MaterialInwardList()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {

                    TIPS.Service.UserService us = new UserService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("UserId", userId);
                    criteria.Add("AppCode", "STR");
                    //criteria.Add("RoleCode", "MRC");
                    Dictionary<long, IList<UserAppRole>> UserAppRoleList = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 100, string.Empty, string.Empty, criteria);
                    var ListCount = (from u in UserAppRoleList.First().Value
                                     where u.UserId == userId
                                     select u.RoleCode).ToList();
                    if (ListCount.Contains("MRC"))
                    {
                        ViewBag.Flag = "MRC";
                    }
                    else if (ListCount.Contains("INC"))
                    {
                        ViewBag.Flag = "INC";
                    }
                    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                    return View();
                }

            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult MaterialInwardListJqGrid(string Supplier, string SuppRefNo, string InvoiceDate, string DCDate, string PONumber, string status, int rows, string sidx, string sord, int? page = 1, int? ExptXl = 0)
        {
            try
            {
                string userId = base.ValidateUser();
                StoreService ss = new StoreService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                sord = sord == "desc" ? "Desc" : "Asc";
                if (!string.IsNullOrEmpty(Supplier))
                    criteria.Add("Supplier", Supplier);
                if (!string.IsNullOrEmpty(SuppRefNo))
                    criteria.Add("SuppRefNo", SuppRefNo);
                if (!string.IsNullOrEmpty(InvoiceDate))
                {
                    // DateTime invdate = Convert.ToDateTime(InvoiceDate);
                    DateTime invdate = DateTime.Parse(InvoiceDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    criteria.Add("InvoiceDate", invdate);
                }
                if (!string.IsNullOrEmpty(DCDate))
                {
                    //DateTime DCDat = Convert.ToDateTime(DCDate);
                    DateTime DCDat = DateTime.Parse(DCDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    criteria.Add("DCDate", DCDat);
                }
                if (!string.IsNullOrEmpty(PONumber))
                    criteria.Add("PONumber", PONumber);
                if (!string.IsNullOrEmpty(status))
                    criteria.Add("Status", status);
                criteria.Add("ProcessedBy", userId);
                Dictionary<long, IList<MaterialInward_vw>> MaterialInwardlist = ss.GetMaterialInwardlistWithPagingAndCriteria_vw(page - 1, rows, sidx, sord, criteria);
                if (MaterialInwardlist != null && MaterialInwardlist.Count > 0)
                {
                    if (ExptXl == 1)
                    {
                        var List = MaterialInwardlist.First().Value.ToList();
                        base.ExptToXL(List, "MaterialInwardlist", (items => new
                        {
                            items.Id,
                            items.InwardNumber,
                            items.TotalCount,
                            items.Supplier,
                            items.PONumber,
                            items.SuppRefNo,
                            InvoiceDate = items.InvoiceDate != null ? items.InvoiceDate.ToString("dd/MM/yyyy") : null,
                            ReceivedBy = items.ReceivedBy != null ? us.GetUserNameByUserId(items.ReceivedBy) : "",
                            //items.ReceivedBy,
                            ReceivedDateTime = items.ReceivedDateTime != null ? items.ReceivedDateTime.ToString("dd/MM/yyyy hh:mm tt") : null,
                            CreatedDate = items.CreatedDate != null ? items.CreatedDate.ToString("dd/MM/yyyy") : null,
                            //items.ProcessedBy,
                            ProcessedBy = items.ProcessedBy != null ? us.GetUserNameByUserId(items.ProcessedBy) : "",
                            items.Status,
                        }));
                        return new EmptyResult();
                    }
                    else
                    {
                        long totalrecords = MaterialInwardlist.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat1 = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,

                            rows = (from items in MaterialInwardlist.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                               items.Id.ToString(),
                               items.InwardNumber,
                               items.TotalCount.ToString(),
                               items.Supplier,
                               items.PONumber,
                               items.SuppRefNo,
                               items.InvoiceDate!=null?items.InvoiceDate.ToString("dd/MM/yyyy"):null,
                               //items.ReceivedBy,
                               items.ReceivedBy != null ? us.GetUserNameByUserId( items.ReceivedBy) : "",
                               items.ReceivedDateTime!=null?items.ReceivedDateTime.ToString("dd/MM/yyyy hh:mm tt"):null,
                               items.CreatedDate!=null?items.CreatedDate.ToString("dd/MM/yyyy"):null,
                               //items.ProcessedBy,
                               items.ProcessedBy != null ? us.GetUserNameByUserId(items.ProcessedBy) : "",
                               items.Status,
                            }
                                    })
                        };
                        return Json(jsondat1, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                { return null; }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult DescriptionForSelectedIdJqGrid(long? Id, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    CommentsService cs = new CommentsService(); // TODO: Initialize to an appropriate value
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    { criteria.Add("EntityRefId", Id); criteria.Add("AppName", "STR"); }
                    sord = sord == "desc" ? "Desc" : "Asc";
                    Dictionary<long, IList<Comments>> CommentsList = cs.GetCommentsListWithPaging(page - 1, rows, sidx, sord, criteria);
                    if (CommentsList != null && CommentsList.Count > 0)
                    {
                        long totalRecords = CommentsList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                        var jsonData = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalRecords,
                            rows = (
                                 from items in CommentsList.First().Value
                                 select new
                                 {
                                     i = items.CommentId,
                                     cell = new string[] { items.CommentedBy, items.CommentedOn.ToString(), items.RejectionComments, items.ResolutionComments }
                                 })
                        };
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        TempData["NoComments"] = 0;
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

        public ActionResult Stock()
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
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult StockListJqGrid(string ItemId, string ItemCode, string Store, string Units, string ClosingBalance, string MatGrp, string MatSubGrp, string Mat, string MaterialGroup, string MaterialSubGroup, string Material, string ExptType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                StoreService ss = new StoreService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrWhiteSpace(ItemId)) { criteria.Add("ItemId", Convert.ToInt64(ItemId)); }
                if (!string.IsNullOrWhiteSpace(ItemCode)) { criteria.Add("ItemCode", ItemCode); }
                if (!string.IsNullOrWhiteSpace(MaterialGroup)) { criteria.Add("MaterialGroup", MaterialGroup); }
                if (!string.IsNullOrWhiteSpace(MaterialSubGroup)) { criteria.Add("MaterialSubGroup", MaterialSubGroup); }
                if (!string.IsNullOrWhiteSpace(Material)) { criteria.Add("Material", Material); }
                User usrObj = (User)Session["objUser"];
                if (usrObj != null)
                {
                    if (usrObj.Campus == "IB MAIN")
                    {
                        var usrcmp = Session["UserCampus"] as IEnumerable<string>;
                        if (!string.IsNullOrWhiteSpace(Store) && usrcmp.Contains(Store)) { criteria.Add("Store", Store); }
                        else if (!string.IsNullOrWhiteSpace(Store) && !usrcmp.Contains(Store)) { criteria.Add("Store", "none"); }
                        else
                        {
                            if (usrcmp != null && usrcmp.Count() != 0)
                            {
                                if (usrcmp.First() != null)   // to check if the usrcmp obj is null or with data
                                {
                                    criteria.Add("Store", usrcmp);
                                }
                            }
                        }
                    }
                    else
                    {
                        IList<StoreMaster> storelist = ss.GetStoreByCampus(usrObj.Campus);
                        if (storelist != null && storelist.Count > 0)
                            criteria.Add("Store", storelist[0].Store);
                    }
                }
                if (!string.IsNullOrWhiteSpace(Units)) { criteria.Add("Units", Units); }
                if (!string.IsNullOrWhiteSpace(ClosingBalance)) { criteria.Add("ClosingBalance", Convert.ToInt32(ClosingBalance)); }
                if (!string.IsNullOrEmpty(MatGrp)) criteria.Add("MaterialGroupId", Convert.ToInt64(MatGrp));
                if (!string.IsNullOrEmpty(MatSubGrp)) criteria.Add("MaterialSubGroupId", Convert.ToInt64(MatSubGrp));
                if (!string.IsNullOrEmpty(Mat)) criteria.Add("Id", Convert.ToInt64(Mat));

                sord = sord == "desc" ? "Desc" : "Asc";
                Dictionary<long, IList<Stock_vw>> StockList = ss.GetStockListWithPagingAndCriteria_vw(page - 1, rows, sidx, sord, criteria);

                if (StockList != null && StockList.Count > 0 && StockList.FirstOrDefault().Key > 0 && StockList.FirstOrDefault().Value.Count > 0)
                {
                    if (ExptType == "Excel")
                    {
                        var List = StockList.First().Value.ToList();
                        base.ExptToXL(List, "StockList", (items => new
                        {
                            items.Id,
                            items.ItemCode,
                            items.Store,
                            items.MaterialGroup,
                            items.MaterialSubGroup,
                            items.Material,
                            items.Units,
                            items.ClosingBalance
                        }));
                        return new EmptyResult();
                    }
                    else
                    {
                        long totalRecords = StockList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                        var jsonData = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalRecords,
                            rows = (
                                 from items in StockList.First().Value
                                 select new
                                 {
                                     i = items.Id,
                                     cell = new string[] { items.Id.ToString(), items.ItemId.ToString(), items.ItemCode, items.Store, items.MaterialGroup, items.MaterialSubGroup, items.Material, items.Units, items.ClosingBalance.ToString() }
                                 })
                        };
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                    }
                }

                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult MaterialGroupSupplier()
        {
            try
            {
                return PartialView();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult MaterialGroupSupplierList(string SupplierName, string MaterialGroup, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                StoreService ds = new StoreService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrWhiteSpace(SupplierName)) { criteria.Add("SupplierName", SupplierName); }
                if (!string.IsNullOrWhiteSpace(MaterialGroup)) { criteria.Add("MaterialGroup", MaterialGroup); }
                sord = sord == "desc" ? "Desc" : "Asc";
                Dictionary<long, IList<MaterialGroupSupplier_vw>> MatGrpSuppList = ds.GetMaterialGroupSupplierlistWithPagingAndCriteriaUsingView(page - 1, rows, sidx, sord, criteria);
                if (MatGrpSuppList != null && MatGrpSuppList.Count > 0)
                {
                    long totalrecords = MatGrpSuppList.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat1 = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,

                        rows = (from items in MatGrpSuppList.First().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[] {
                               items.Id.ToString(),
                               items.SupplierId.ToString(),
                               items.SupplierName,
                               items.MaterialGroup,
                              
                            }
                                })
                    };
                    return Json(jsondat1, JsonRequestBehavior.AllowGet);

                }
                else return Json(null);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult Unitsddl()
        {
            try
            {
                StoreService ss = new StoreService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();

                Dictionary<long, IList<StoreUnits>> StoreUnitsList = ss.GetStoreUnitsListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteria);
                if (StoreUnitsList != null && StoreUnitsList.First().Value != null && StoreUnitsList.First().Value.Count > 0)
                {
                    var StoreUnits = (from u in StoreUnitsList.First().Value
                                      where u.UnitCode != null
                                      select u.UnitCode).Distinct().ToList();
                    return Json(StoreUnits, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult MaterialSearch()
        {
            try
            {
                return PartialView();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult MaterialRequestMaterialSearch(string Campus)
        {
            try
            {
                ViewBag.Campus = Campus;
                return View();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult UpdateSKU(SkuList s)
        {
            try
            {
                StoreService ss = new StoreService();
                if (s.DamagedQty == null)
                    s.DamagedQty = 0;
                if (s.OrderQty == null)
                    s.OrderQty = 0;

                s.DamagelessQty = s.ReceivedQty - s.DamagedQty;
                s.StockAvailableQty = s.DamagelessQty;
                s.IssuedStatus = "Not Issued";
                s.IssuedQty = 0;
                //MaterialsMaster mm = ss.GetMaterialsMasterByMaterial(s.Material.Trim());
                //MaterialInward mi = ss.GetMaterialInwardById(s.MaterialRefId);
                //if (mm != null)
                //{
                //    MaterialPriceMaster mpm = ss.GetMaterialPriceByMaterialId(Convert.ToInt32(mm.Id));
                //    if (mpm != null)
                //        s.UnitPrice = Convert.ToDecimal(s.ReceivedQty * mpm.UnitPrice);
                //}
                s.Material = s.Material.Trim();
                ss.CreateOrUpdateSku(s);

                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public JsonResult FillAllStore()
        {
            string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
            StoreService ss = new StoreService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            Dictionary<long, IList<StoreMaster>> StoreMaster = ss.GetStoreMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            if (StoreMaster != null && StoreMaster.First().Value != null && StoreMaster.First().Value.Count > 0)
            {
                var StoreMasterList = (
                         from items in StoreMaster.First().Value
                         select new
                         {
                             Text = items.Store,
                             Value = items.Store
                         }).Distinct().ToList();
                return Json(StoreMasterList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult FillStore(string Campus)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    StoreService ss = new StoreService();
                    IList<StoreMaster> StoreList = ss.GetStoreByCampus(Campus);
                    var Store = new
                    {
                        rows = (
                             from items in StoreList
                             select new
                             {
                                 Text = items.Store,
                                 Value = items.Store
                             }).ToArray(),
                    };

                    return Json(Store, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        //public ActionResult StoreMaterialSubGroupMaster()
        //{
        //    try
        //    {
        //        return PartialView();
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
        //        throw ex;
        //    }
        //}

        public ActionResult StoreMaterialSubGroupMasterListJqGrid(string MaterialGroup, string MaterialSubGroup, string MatSubGrpCode, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                StoreService ds = new StoreService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrWhiteSpace(MaterialGroup)) { criteria.Add("MaterialGroup", MaterialGroup); }
                if (!string.IsNullOrWhiteSpace(MaterialSubGroup)) { criteria.Add("MaterialSubGroup", MaterialSubGroup); }
                if (!string.IsNullOrWhiteSpace(MatSubGrpCode)) { criteria.Add("MatSubGrpCode", MatSubGrpCode); }
                sord = sord == "desc" ? "Desc" : "Asc";
                Dictionary<long, IList<MaterialSubGroupMaster_vw>> MaterialSubGroupMasterList = ds.GetMaterialSubGroupListWithPagingAndCriteriaUsingView(page - 1, rows, sidx, sord, criteria);
                if (MaterialSubGroupMasterList != null && MaterialSubGroupMasterList.Count > 0)
                {
                    long totalrecords = MaterialSubGroupMasterList.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat1 = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,

                        rows = (from items in MaterialSubGroupMasterList.First().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[] {
                               items.Id.ToString(),items.MaterialGroupId.ToString(),items.MaterialGroup,items.MaterialSubGroup,items.MatSubGrpCode
                            }
                                })
                    };
                    return Json(jsondat1, JsonRequestBehavior.AllowGet);

                }
                else return Json(null);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddMaterialSubGroupMaster(MaterialSubGroupMaster msgm, string test)
        {
            try
            {
                msgm.MaterialSubGroup = msgm.MaterialSubGroup.Trim();
                StoreService ss = new StoreService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("MaterialGroupId", msgm.MaterialGroupId);
                criteria.Add("MaterialSubGroup", msgm.MaterialSubGroup);
                criteria.Add("MatSubGrpCode", msgm.MatSubGrpCode);
                Dictionary<long, IList<MaterialSubGroupMaster>> materialsubgroup = ss.GetMaterialSubGroupListWithPagingAndCriteria(0, 9999, null, null, criteria);
                if (materialsubgroup != null && materialsubgroup.First().Value != null && (materialsubgroup.First().Value.Count > 1 || materialsubgroup.First().Value.Count > 0))
                {
                    var script = @"ErrMsg(""Already Exists"");";
                    return JavaScript(script);
                }

                if (test != "edit")
                { msgm.Id = 0; }
                ViewBag.flag = 1;
                msgm.MatSubGrpCode = msgm.MatSubGrpCode.ToUpper();
                msgm.MaterialSubGroup = msgm.MaterialSubGroup.Trim();
                ss.CreateOrUpdateStoreMaterialSubGroupMaster(msgm);
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult GetSupplierName()
        {
            try
            {
                StoreService ss = new StoreService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<long, IList<StoreSupplierMaster>> SupplierList = ss.GetStoreSupplierMasterlistWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteria);
                if (SupplierList != null && SupplierList.First().Value != null && SupplierList.First().Value.Count > 0)
                {
                    var MaterialGroup = (
                             from items in SupplierList.First().Value

                             select new
                             {
                                 Value = items.Id,
                                 Text = items.SupplierName,
                             }).Distinct().ToList();

                    return Json(MaterialGroup, JsonRequestBehavior.AllowGet);
                }
                else
                { return Json(null, JsonRequestBehavior.AllowGet); }
            }

            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult DeleteSKU(long id)
        {
            try
            {
                string userId = ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    StoreService ss = new StoreService();
                    SkuList sku = new SkuList();
                    if (id > 0)
                        sku = ss.GetSkuListById(id);
                    if (sku != null)
                        ss.DeleteSKUbyId(sku);
                    return Json(id, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult UOMCovertion()
        {
            try
            {
                return PartialView();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult UOMCovertionListJqGrid(string BaseQuantity, string BaseUnit, string ConversionQuantity, string ConversionUnit, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                StoreService ss = new StoreService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrWhiteSpace(BaseQuantity)) { criteria.Add("BaseQuantity", Convert.ToInt32(BaseQuantity)); }
                if (!string.IsNullOrWhiteSpace(BaseUnit)) { criteria.Add("BaseUnit", BaseUnit); }
                if (!string.IsNullOrWhiteSpace(ConversionQuantity)) { criteria.Add("ConversionQuantity", Convert.ToInt32(ConversionQuantity)); }
                if (!string.IsNullOrWhiteSpace(ConversionUnit)) { criteria.Add("ConversionUnit", ConversionUnit); }
                sord = sord == "desc" ? "Desc" : "Asc";
                Dictionary<long, IList<UOM_ConversionMatrix>> UOMConversionList = ss.GetUOMConversionlistWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (UOMConversionList != null && UOMConversionList.Count > 0)
                {
                    long totalrecords = UOMConversionList.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat1 = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,

                        rows = (from items in UOMConversionList.First().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[] {
                               items.Id.ToString(),items.BaseQuantity.ToString(),items.BaseUnit,items.ConversionQuantity.ToString(),items.ConversionUnit
                            }
                                })
                    };
                    return Json(jsondat1, JsonRequestBehavior.AllowGet);
                }
                else { return Json(null); }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddUOMConversionUnits(UOM_ConversionMatrix ucm, string test)
        {
            try
            {
                StoreService ss = new StoreService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("BaseQuantity", ucm.BaseQuantity);
                criteria.Add("BaseUnit", ucm.BaseUnit);
                criteria.Add("ConversionQuantity", ucm.ConversionQuantity);
                criteria.Add("ConversionUnit", ucm.ConversionUnit);
                Dictionary<long, IList<UOM_ConversionMatrix>> UOMConversionList = ss.GetUOMConversionlistWithPagingAndCriteria(0, 9999, null, null, criteria);
                if (test == "edit")
                {
                    if (UOMConversionList != null && UOMConversionList.First().Value != null && UOMConversionList.First().Value.Count > 1)
                    {
                        //var script = @"ErrMsg(""Already Exists"");";
                        //return JavaScript(script);
                        return null;
                    }
                    else
                    {
                        ViewBag.flag = 1;
                        ss.CreateOrUpdateUOM_ConversionMatrix(ucm);
                        return null;
                    }
                }
                else
                {
                    if (UOMConversionList != null && UOMConversionList.First().Value != null && UOMConversionList.First().Value.Count > 0)
                    {
                        var script = @"ErrMsg(""Already Exists"");";
                        return JavaScript(script);
                    }
                    else
                    {
                        ucm.Id = 0;
                        ViewBag.flag = 1;
                        long id = ss.CreateOrUpdateUOM_ConversionMatrix(ucm);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult UpdateMaterialRequestList(MaterialRequestList mrl)
        {
            try
            {
                StoreService ss = new StoreService();
                ss.CreateOrUpdateMaterialRequestList(mrl);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult DeleteMaterialRequestList(long id)
        {
            try
            {
                string userId = ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    StoreService ss = new StoreService();
                    MaterialRequestList mrl = new MaterialRequestList();
                    if (id > 0)
                        mrl = ss.GetMaterialRequestListById(id);
                    if (mrl != null)
                        ss.DeleteMaterialRequestListById(mrl);
                    return Json(id, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult EmptyJsonUrl(int rows, string sidx, string sord, int? page = 1)
        {
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MaterialIssueListJqGrid(long Id, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                StoreService ss = new StoreService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("MRLId", Id);
                sord = sord == "desc" ? "Desc" : "Asc";
                Dictionary<long, IList<MaterialIssueList_vw>> MaterialIssueList_vw = ss.GetMaterialIssuelist_vwWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (MaterialIssueList_vw != null && MaterialIssueList_vw.First().Value != null && MaterialIssueList_vw.First().Value.Count > 0)
                {
                    long totalrecords = MaterialIssueList_vw.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat1 = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,

                        rows = (from items in MaterialIssueList_vw.First().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[] {
                               items.Id.ToString(),items.MRLId.ToString(),items.IssNoteNumber,items.IssueDate.ToString(),items.IssueQty.ToString(),items.IssuedBy,items.Status, 
                               items.IssNoteId.ToString()
                            }
                                })
                    };
                    return Json(jsondat1, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult UpdateQty(MaterialRequestList mrl)
        {
            try
            {
                StoreService ss = new StoreService();
                MaterialRequestList mr = new MaterialRequestList();
                mr = ss.GetMaterialRequestListById(mrl.Id);
                if (mrl.ApprovedQty != null)
                {
                    mr.ApprovedQty = mrl.ApprovedQty;
                    mr.Status = mrl.ApprovedQty == 0 ? "Completely Issued" : "Approved";
                    mr.IssuedQty = 0;
                }
                if (mrl.Quantity != null)
                    mr.Quantity = mrl.Quantity;
                ss.CreateOrUpdateMaterialRequestList(mr);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult ApproveRequest(MaterialRequest mr)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    StoreService ss = new StoreService();
                    string appcomm = mr.ApproverComments;
                    mr = ss.GetMaterialRequestById(mr.Id);
                    mr.ApproverComments = appcomm;
                    ss.CompleteActivityStoreManagement(mr, "StoreManagement", userId, mr.RequestStatus, false);
                    //  SendMailToMaterialRequestor(mr);
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult RejectRequest(long Id, string RejComments)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    StoreService ss = new StoreService();
                    bool isRejected = true;
                    string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                    CommentsService cmntsSrvc = new CommentsService();
                    Comments cmntsObj = new Comments();
                    cmntsObj.EntityRefId = Id;

                    cmntsObj.CommentedBy = userid;
                    cmntsObj.CommentedOn = DateTime.Now;
                    cmntsObj.RejectionComments = RejComments;
                    cmntsObj.AppName = "STR";
                    cmntsSrvc.CreateOrUpdateComments(cmntsObj);
                    MaterialRequest mr = ss.GetMaterialRequestById(Id);
                    ss.CompleteActivityStoreManagement(mr, "StoreManagement", userId, mr.RequestStatus, isRejected);

                    // SendMailToMaterialRequestor(mr);
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult ReplyRequest(long Id, string ReplyComments)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    CommentsService cmntsSrvc1 = new CommentsService();
                    StoreService ss = new StoreService();
                    Dictionary<string, object> criteria1 = new Dictionary<string, object>();
                    criteria1.Add("EntityRefId", Id);
                    Dictionary<long, IList<Comments>> list = cmntsSrvc1.GetCommentsListWithPaging(0, 1000, string.Empty, string.Empty, criteria1);
                    if (list != null && list.Count > 0)
                    {
                        foreach (Comments cm in list.First().Value)
                        {
                            if (string.IsNullOrWhiteSpace(cm.ResolutionComments))
                            {
                                cm.ResolutionComments = ReplyComments;
                                cmntsSrvc1.CreateOrUpdateComments(cm);
                                break;
                            }
                        }
                    }
                    MaterialRequest mr = ss.GetMaterialRequestById(Id);
                    ss.CompleteActivityStoreManagement(mr, "StoreManagement", userId, mr.RequestStatus, false);
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult MaterialIssueNote(long? Id, long? activityId)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    UserService us = new UserService();
                    StoreService ss = new StoreService();
                    Dictionary<string, object> Criteria = new Dictionary<string, object>();
                    ViewBag.ActivityId = activityId ?? 0;
                    if (Id > 0)
                    {
                        ProcessFlowServices pfs = new ProcessFlowServices();
                        pfs.AssignActivity((Convert.ToInt64(activityId)), userId);
                        MaterialRequest mr = ss.GetMaterialRequestById(Convert.ToInt64(Id));
                        MaterialIssueNote min = new MaterialIssueNote();
                        min.RequestId = mr.Id;
                        min.RequestNumber = mr.RequestNumber;
                        min.ProcessedBy = mr.ProcessedBy;
                        min.UserRole = mr.UserRole;
                        min.RequestStatus = mr.RequestStatus;
                        min.Campus = mr.Campus;
                        min.RequiredForCampus = mr.RequiredForCampus;
                        min.RequestedDate = mr.RequestedDate;
                        min.IssueDate = DateTime.Now;
                        min.IssuedBy = userId;
                        min.CreatedDate = DateTime.Now;
                        min.RequestorDescription = mr.RequestorDescription;
                        min.Department = mr.Department;
                        min.RequiredForStore = mr.RequiredForStore;
                        min.RequiredFromStore = mr.RequiredFromStore;
                        // ss.CreateOrUpdateMaterialIssueNote(min);
                        min.CreatedUserName = us.GetUserNameByUserId(min.ProcessedBy);
                        return View(min);
                    }
                    else
                        return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult SaveIssueNote(MaterialIssueNote min)
        {
            try
            {
                StoreService ss = new StoreService();
                IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                min.DeliveryDate = DateTime.Parse(Request["DeliveryDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                min.RequestedDate = DateTime.Parse(Request["RequestedDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                min.IssueDate = DateTime.Now;
                min.CreatedDate = DateTime.Now;
                long IssNoteId = ss.CreateOrUpdateMaterialIssueNote(min);
                min.IssNoteNumber = "INN-" + IssNoteId;
                ss.CreateOrUpdateMaterialIssueNote(min);
                var iss = IssNoteId;
                return Json(iss, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult UpdateIssueList(MaterialIssueList mil)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (!string.IsNullOrWhiteSpace(mil.InwardIds))
                    {
                        StoreService ss = new StoreService();
                        MaterialRequestList mrl = new MaterialRequestList();
                        Dictionary<string, object> criteria = new Dictionary<string, object>();
                        criteria.Add("MRLId", mil.MRLId);
                        Dictionary<long, IList<MaterialIssueList>> MaterialIssueList = ss.GetMaterialIssuelistWithPagingAndCriteria(0, 9999, "Id", "Desc", criteria);
                        if (MaterialIssueList != null && MaterialIssueList.First().Value != null && MaterialIssueList.First().Value.Count > 0)
                        {
                            mil.RequestQty = MaterialIssueList.First().Value[0].RequestQty;
                            mil.ApprovedQty = MaterialIssueList.First().Value[0].ApprovedQty;
                            mil.PrevIssdQty = MaterialIssueList.First().Value[0].TotalIssued;
                            mil.IssueQty = Convert.ToInt32(mil.IssueQty);
                            mil.TotalIssued = mil.PrevIssdQty + mil.IssueQty;

                            mrl = ss.GetMaterialRequestListById(mil.MRLId);
                            mrl.IssuedQty = mil.TotalIssued;
                        }
                        else
                        {
                            mrl = ss.GetMaterialRequestListById(mil.MRLId);
                            mil.MRLId = mrl.Id;
                            mil.RequestQty = Convert.ToInt32(mrl.Quantity);
                            mil.ApprovedQty = Convert.ToInt32(mrl.ApprovedQty);
                            mil.PrevIssdQty = 0;
                            mil.IssueQty = Convert.ToInt32(mil.IssueQty);
                            mil.TotalIssued = mil.PrevIssdQty + mil.IssueQty;

                            mrl.IssuedQty = mil.TotalIssued;
                        }
                        if (mil.ApprovedQty != mil.TotalIssued)
                        {
                            mrl.Status = "Partially Issued";
                        }
                        else
                        {
                            mrl.Status = "Completely Issued";
                        }

                        mil.Status = "Material Issued";
                        ss.CreateOrUpdateMaterialRequestList(mrl);
                        MaterialsMaster mm = ss.GetMaterialsMasterByMaterial(mrl.Material.Trim());

                        ss.CreateOrUpdateMaterialIssueList(mil);

                        var InwardIdarr = mil.InwardIds.Split(',', ' ');
                        InwardIdarr = InwardIdarr.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                        long[] InwardIdlongarr = Array.ConvertAll(InwardIdarr, s => long.Parse(s));
                        Dictionary<string, object> criteria1 = new Dictionary<string, object>();
                        string[] IssuedStatus = new[] { "Not Issued", "Partially Issued" };

                        if (InwardIdlongarr != null)
                        {
                            criteria1.Add("MaterialRefId", InwardIdlongarr);
                            criteria1.Add("Material", mil.Material.Trim());
                            criteria1.Add("IssuedStatus", IssuedStatus);
                            Dictionary<long, IList<SkuList>> SkuList = ss.GetSkulistWithPagingAndCriteria(0, 9999, "Asc", "SkuId", criteria1);
                            if (SkuList != null && SkuList.FirstOrDefault().Value != null && SkuList.FirstOrDefault().Value.Count() > 0 && SkuList.FirstOrDefault().Key > 0)
                            {
                                SkuList sku = SkuList.FirstOrDefault().Value[0];
                                if (mil.IssueQty <= sku.StockAvailableQty)
                                {
                                    sku.IssuedQty = sku.IssuedQty + mil.IssueQty;
                                    sku.StockAvailableQty = sku.DamagelessQty - sku.IssuedQty;
                                    if (sku.StockAvailableQty == 0)
                                        sku.IssuedStatus = "Completely Issued";
                                    else
                                        sku.IssuedStatus = "Partially Issued";
                                    ss.CreateOrUpdateSku(sku);
                                }
                                else if (mil.IssueQty > sku.StockAvailableQty)
                                {
                                    sku.IssuedQty = sku.IssuedQty + sku.StockAvailableQty;
                                    int TobeIssuedQty = Convert.ToInt32(mil.IssueQty - sku.StockAvailableQty);
                                    sku.StockAvailableQty = 0;
                                    sku.IssuedStatus = "Completely Issued";
                                    ss.CreateOrUpdateSku(sku);
                                    sku = SkuList.FirstOrDefault().Value[1];
                                    if (TobeIssuedQty <= sku.StockAvailableQty)
                                    {
                                        sku.IssuedQty = sku.IssuedQty + TobeIssuedQty;
                                        sku.StockAvailableQty = sku.DamagelessQty - sku.IssuedQty;
                                        if (sku.StockAvailableQty == 0)
                                            sku.IssuedStatus = "Completely Issued";
                                        else
                                            sku.IssuedStatus = "Partially Issued";
                                        ss.CreateOrUpdateSku(sku);
                                    }

                                    else if (TobeIssuedQty > sku.StockAvailableQty)
                                    {

                                    }
                                }
                            }

                        }

                        //
                        criteria.Clear();
                        criteria.Add("MatReqRefId", mrl.MatReqRefId);
                        Dictionary<long, IList<MaterialRequestList>> MaterialRequestList = ss.GetMaterialRequestListListWithPagingAndCriteria(0, 9999, "Desc", "Id", criteria);
                        if (MaterialRequestList != null && MaterialRequestList.First().Value != null && MaterialRequestList.First().Value.Count > 0)
                        {
                            var MRLId = (from u in MaterialRequestList.First().Value
                                         select u.Id).ToArray();
                            criteria.Clear();
                            criteria.Add("Id", MRLId);
                            Dictionary<long, IList<MaterialRequestList>> MaterialRequestList1 = ss.GetMaterialRequestListListWithPagingAndCriteria(0, 9999, "Desc", "Id", criteria);
                            if (MaterialRequestList1 != null && MaterialRequestList1.First().Value != null && MaterialRequestList1.First().Value.Count > 0)
                            {
                                var status = (from u in MaterialRequestList1.First().Value
                                              select u.Status).ToArray();
                                if (status.Contains("Partially Issued") || status.Contains("Approved") || status.Contains("Requested"))
                                {

                                }
                                else
                                {
                                    MaterialRequest mr = ss.GetMaterialRequestById(MaterialRequestList1.First().Value[0].MatReqRefId);
                                    ss.CompleteActivityStoreManagement(mr, "StoreManagement", userId, mr.RequestStatus, false);
                                }
                            }
                        }

                        MaterialRequest mr1 = ss.GetMaterialRequestById(mrl.MatReqRefId);
                        MaterialIssueNote min = ss.GetMaterialIssueNoteById(mil.IssNoteId);
                        mm = ss.GetMaterialsMasterByMaterial(mrl.Material);
                        StockTransaction st = new StockTransaction();
                        st.TransactionCode = mr1.RequestNumber;
                        st.Store = min.Store;
                        st.ItemId = mm.Id;
                        st.Units = mrl.Units;
                        st.TransactionDate = DateTime.Now;
                        st.TransactionBy = userId;
                        st.TransactionType = "Material Issue";
                        st.Qty = mil.IssueQty;
                        //st.DamagedQty = Convert.ToInt32(MaterialSkulist.First().Value[i].DamagedQty);
                        st.TransactionComments = mr1.RequestorDescription;
                        //st.RequiredForStore = min.RequiredForStore;
                        //st.RequiredFromStore = min.RequiredFromStore;
                        ss.CreateOrUpdateStockTransaction(st);
                        return Json(mil.IssNoteId, JsonRequestBehavior.AllowGet);
                    }
                    else
                        return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        //public ActionResult ItemsTobeIssued(long Id, string Store, int rows, string sidx, string sord, int? page = 1)
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //        else
        //        {
        //            StoreService ss = new StoreService();
        //            Dictionary<string, object> criteria = new Dictionary<string, object>();
        //            sord = sord == "desc" ? "Desc" : "Asc";
        //            criteria.Add("MatReqRefId", Id);
        //            Dictionary<long, IList<MaterialRequestList>> MaterialRequestlist = ss.GetMaterialRequestListListWithPagingAndCriteria(page - 1, rows, sord, sidx, criteria);
        //            if (MaterialRequestlist != null && MaterialRequestlist.Count > 0 && MaterialRequestlist.First().Key > 0)
        //            {
        //                for (int i = 0; i < MaterialRequestlist.First().Key; i++)
        //                {
        //                    MaterialsMaster mm = ss.GetMaterialsMasterByMaterial(MaterialRequestlist.First().Value[i].Material.Trim());
        //                    criteria.Clear();
        //                    criteria.Add("ItemId", mm.Id);
        //                    if (!string.IsNullOrWhiteSpace(Store)) { criteria.Add("Store", Store); }
        //                    criteria.Add("AMonth", DateTime.Now.Month);
        //                    criteria.Add("AYear", DateTime.Now.Year);
        //                    Dictionary<long, IList<StoreStockBalance>> StoreStockBalanceList = ss.GetStoreStockBalanceListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
        //                    Dictionary<string, object> criteria1 = new Dictionary<string, object>();
        //                    criteria1.Add("Material", mm.Material.Trim());
        //                    criteria1.Add("MaterialInward." + "Store", Store);
        //                    string[] IssuedStatus = new[] { "Not Issued", "Partially Issued" };
        //                    criteria1.Add("IssuedStatus", IssuedStatus);
        //                    string[] alias = new string[1];
        //                    alias[0] = "MaterialInward";
        //                    Dictionary<long, IList<SkuList_MaterialInward>> SkuList = ss.GetSkuList_MaterialInwardListWithPagingAndCriteria(0, 9999, "SkuId", "Asc", criteria1, "", null, alias);
        //                    if (SkuList != null && SkuList.FirstOrDefault().Value != null && SkuList.Count() > 0 && SkuList.FirstOrDefault().Key > 0)
        //                    {
        //                        for (int j = 0; j < SkuList.FirstOrDefault().Value.Count; j++)
        //                        {
        //                            MaterialRequestlist.First().Value[i].InwardIds = MaterialRequestlist.First().Value[i].InwardIds + Convert.ToString(SkuList.First().Value[j].MaterialRefId) + ", ";
        //                            MaterialRequestlist.First().Value[i].AvailableQtys = MaterialRequestlist.First().Value[i].AvailableQtys + Convert.ToString(SkuList.First().Value[j].StockAvailableQty) + ", ";
        //                            MaterialRequestlist.First().Value[i].UnitPrices = MaterialRequestlist.First().Value[i].UnitPrices + Convert.ToString(SkuList.First().Value[j].UnitPrice) + ", ";
        //                        }
        //                    }
        //                }
        //                long totalrecords = MaterialRequestlist.FirstOrDefault().Key;
        //                int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
        //                var jsondat1 = new
        //                {
        //                    total = totalPages,
        //                    page = page,
        //                    records = totalrecords,
        //                    rows = (from items in MaterialRequestlist.FirstOrDefault().Value
        //                            select new
        //                            {
        //                                i = 2,
        //                                cell = new string[] {
        //                       items.Id.ToString(),
        //                       items.RequestType,
        //                       items.RequiredForGrade,
        //                       items.Section,
        //                       items.RequiredFor,
        //                       items.Material,
        //                       items.MaterialGroup,
        //                       items.MaterialSubGroup,
        //                       items.Units,
        //                       items.RequiredDate != null ? items.RequiredDate.Value.ToString("dd/MM/yyyy") : null,
        //                       items.Status,
        //                       items.Quantity.ToString(),
        //                       items.ApprovedQty.ToString(),
        //                       items.IssuedQty.ToString(),
        //                       items.InwardIds,
        //                       items.AvailableQtys,
        //                       items.UnitPrices
        //                    }
        //                            })
        //                };
        //                return Json(jsondat1, JsonRequestBehavior.AllowGet);
        //            }
        //            else
        //            { return null; }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
        //        throw ex;
        //    }
        //}

        public ActionResult ItemsTobeIssued(long Id, string Store, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    StoreService ss = new StoreService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    sord = sord == "desc" ? "Desc" : "Asc";
                    criteria.Add("MatReqRefId", Id);
                    Dictionary<long, IList<MaterialRequestList>> MaterialRequestlist = ss.GetMaterialRequestListListWithPagingAndCriteria(page - 1, rows, sord, sidx, criteria);
                    if (MaterialRequestlist != null && MaterialRequestlist.Count > 0 && MaterialRequestlist.First().Key > 0)
                    {
                        for (int i = 0; i < MaterialRequestlist.First().Key; i++)
                        {
                            MaterialsMaster mm = ss.GetMaterialsMasterByMaterial(MaterialRequestlist.First().Value[i].Material.Trim());
                            criteria.Clear();
                            criteria.Add("ItemId", mm.Id);
                            if (!string.IsNullOrWhiteSpace(Store)) { criteria.Add("Store", Store); }
                            criteria.Add("AMonth", DateTime.Now.Month);
                            criteria.Add("AYear", DateTime.Now.Year);
                            Dictionary<long, IList<StoreStockBalance>> StoreStockBalanceList = ss.GetStoreStockBalanceListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                            Dictionary<string, object> criteria1 = new Dictionary<string, object>();
                            criteria1.Add("Material", mm.Material.Trim());
                            criteria1.Add("MaterialInward." + "Store", Store);
                            string[] IssuedStatus = new[] { "Not Issued", "Partially Issued" };
                            criteria1.Add("IssuedStatus", IssuedStatus);
                            string[] alias = new string[1];
                            alias[0] = "MaterialInward";
                            Dictionary<long, IList<SkuList_MaterialInward>> SkuList = ss.GetSkuList_MaterialInwardListWithPagingAndCriteria(0, 9999, "SkuId", "Desc", criteria1, "", null, alias);
                            if (SkuList != null && SkuList.FirstOrDefault().Value != null && SkuList.Count() > 0 && SkuList.FirstOrDefault().Key > 0)
                            {
                                for (int j = 0; j < SkuList.FirstOrDefault().Value.Count; j++)
                                {
                                    MaterialRequestlist.First().Value[i].InwardIds = MaterialRequestlist.First().Value[i].InwardIds + Convert.ToString(SkuList.First().Value[j].MaterialRefId) + ", ";
                                    MaterialRequestlist.First().Value[i].AvailableQtys = MaterialRequestlist.First().Value[i].AvailableQtys + Convert.ToString(SkuList.First().Value[j].StockAvailableQty) + ", ";
                                    MaterialRequestlist.First().Value[i].UnitPrices = MaterialRequestlist.First().Value[i].UnitPrices + Convert.ToString(SkuList.First().Value[j].UnitPrice) + ", ";
                                    MaterialRequestlist.First().Value[i].Taxes = MaterialRequestlist.First().Value[i].Taxes + Convert.ToString(SkuList.First().Value[j].Tax) + ", ";
                                    MaterialRequestlist.First().Value[i].Discounts = MaterialRequestlist.First().Value[i].Discounts + Convert.ToString(SkuList.First().Value[j].Discount) + ", ";
                                }
                            }
                        }
                        long totalrecords = MaterialRequestlist.FirstOrDefault().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat1 = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in MaterialRequestlist.FirstOrDefault().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                               items.Id.ToString(),
                               items.RequestType,
                               items.RequiredForGrade,
                               items.Section,
                               items.RequiredFor,
                               items.Material,
                               items.MaterialGroup,
                               items.MaterialSubGroup,
                               items.Units,
                               items.RequiredDate != null ? items.RequiredDate.Value.ToString("dd/MM/yyyy") : null,
                               items.Status,
                               items.Quantity.ToString(),
                               items.ApprovedQty.ToString(),
                               items.IssuedQty.ToString(),
                               items.InwardIds,
                               items.AvailableQtys,
                               items.UnitPrices,
                               items.Taxes,
                               items.Discounts
                            }
                                    })
                        };
                        return Json(jsondat1, JsonRequestBehavior.AllowGet);
                    }
                    else
                    { return null; }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult IssueNoteList()
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
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        //public ActionResult IssueNoteListJqGrid(MaterialIssueNote min, int rows, string sidx, string sord, int? page = 1)
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //        else
        //        {
        //            StoreService ss = new StoreService();
        //            Dictionary<string, object> criteria = new Dictionary<string, object>();
        //            if (!string.IsNullOrWhiteSpace(min.IssNoteNumber)) criteria.Add("IssNoteNumber", min.IssNoteNumber);
        //            if (!string.IsNullOrWhiteSpace(min.ProcessedBy)) criteria.Add("ProcessedBy", min.ProcessedBy);
        //            if (!string.IsNullOrWhiteSpace(min.RequiredForCampus)) criteria.Add("RequiredForCampus", min.RequiredForCampus);
        //            if (!string.IsNullOrWhiteSpace(min.RequiredForStore)) criteria.Add("RequiredForStore", min.RequiredForStore);
        //            if (!string.IsNullOrWhiteSpace(min.RequiredFromStore)) criteria.Add("RequiredFromStore", min.RequiredFromStore);
        //            if (!string.IsNullOrWhiteSpace(min.IssuedBy)) criteria.Add("IssuedBy", min.IssuedBy);
        //            if (!string.IsNullOrWhiteSpace(min.RequestStatus)) criteria.Add("RequestStatus", min.RequestStatus);
        //            if (!string.IsNullOrWhiteSpace(min.DeliveredThrough)) criteria.Add("DeliveredThrough", min.DeliveredThrough);
        //            if (!string.IsNullOrWhiteSpace(min.DeliveryDetails)) criteria.Add("DeliveryDetails", min.DeliveryDetails);
        //            //if (!string.IsNullOrWhiteSpace(min.IssNoteNumber)) criteria.Add("IssNoteNumber", min.IssNoteNumber);
        //            sord = sord == "desc" ? "Desc" : "Asc";
        //            Dictionary<long, IList<MaterialIssueNote>> MaterialIssueNotelist = ss.GetMaterialMaterialIssueNoteListWithPagingAndCriteria(page - 1, rows, sord, sidx, criteria);

        //            if (MaterialIssueNotelist != null && MaterialIssueNotelist.Count > 0)
        //            {
        //                long totalrecords = MaterialIssueNotelist.First().Key;
        //                int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
        //                var jsondat1 = new
        //                {
        //                    total = totalPages,
        //                    page = page,
        //                    records = totalrecords,

        //                    rows = (from items in MaterialIssueNotelist.First().Value
        //                            select new
        //                            {
        //                                i = 2,
        //                                cell = new string[] {
        //                       items.IssNoteId.ToString(),
        //                       items.IssNoteNumber,
        //                       items.ProcessedBy,
        //                       items.RequiredForCampus,
        //                       items.RequiredForStore,
        //                       items.RequiredFromStore,
        //                       items.RequestedDate!= null ? items.RequestedDate.Value.ToString("dd/MM/yyyy") : null,
        //                       items.IssueDate!= null ? items.IssueDate.Value.ToString("dd/MM/yyyy") : null,
        //                       items.IssuedBy,
        //                       items.RequestStatus,
        //                       items.DeliveredThrough,
        //                       items.DeliveryDetails,
        //                       items.DeliveryDate!= null ? items.DeliveryDate.Value.ToString("dd/MM/yyyy") : null
        //                            }
        //                            })
        //                };
        //                return Json(jsondat1, JsonRequestBehavior.AllowGet);
        //            }
        //            else
        //            {
        //                return Json(null);

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
        //        throw ex;
        //    }
        //}

        public ActionResult IssueNoteListJqGrid(MaterialIssueNote min, string RequiredCampus, string IssNoteNo, string DeliverThrough, string DeliverDate, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    StoreService ss = new StoreService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    if (!string.IsNullOrWhiteSpace(min.IssNoteNumber))
                        criteria.Add("IssNoteNumber", min.IssNoteNumber);
                    else if (!string.IsNullOrWhiteSpace(IssNoteNo))
                        criteria.Add("IssNoteNumber", IssNoteNo);
                    if (!string.IsNullOrWhiteSpace(min.ProcessedBy)) criteria.Add("ProcessedBy", min.ProcessedBy);

                    if (!string.IsNullOrWhiteSpace(min.RequiredForCampus))
                        criteria.Add("RequiredForCampus", min.RequiredForCampus);
                    else if (!string.IsNullOrWhiteSpace(RequiredCampus))
                        criteria.Add("RequiredForCampus", RequiredCampus);
                    if (!string.IsNullOrWhiteSpace(min.RequiredForStore)) criteria.Add("RequiredForStore", min.RequiredForStore);
                    if (!string.IsNullOrWhiteSpace(min.RequiredFromStore)) criteria.Add("RequiredFromStore", min.RequiredFromStore);
                    if (!string.IsNullOrWhiteSpace(min.IssuedBy)) criteria.Add("IssuedBy", min.IssuedBy);
                    if (!string.IsNullOrWhiteSpace(min.RequestStatus)) criteria.Add("RequestStatus", min.RequestStatus);
                    if (!string.IsNullOrWhiteSpace(min.DeliveryDetails)) criteria.Add("DeliveryDetails", min.DeliveryDetails);
                    if (!string.IsNullOrWhiteSpace(min.DeliveredThrough))
                        criteria.Add("DeliveredThrough", min.DeliveredThrough);
                    else if (!string.IsNullOrWhiteSpace(DeliverThrough))
                        criteria.Add("DeliveredThrough", DeliverThrough);
                    if (min.DeliveryDate != null) criteria.Add("DeliveryDate", min.DeliveryDate);
                    else if (!string.IsNullOrWhiteSpace(DeliverDate))
                    {
                        DateTime dtime = DateTime.Parse(DeliverDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        criteria.Add("DeliveryDate", dtime);
                    }

                    //if (!string.IsNullOrWhiteSpace(min.IssNoteNumber)) criteria.Add("IssNoteNumber", min.IssNoteNumber);
                    sord = sord == "desc" ? "Desc" : "Asc";
                    Dictionary<long, IList<MaterialIssueNote>> MaterialIssueNotelist = ss.GetMaterialMaterialIssueNoteListWithPagingAndCriteria(page - 1, rows, sord, sidx, criteria);

                    if (MaterialIssueNotelist != null && MaterialIssueNotelist.Count > 0)
                    {
                        long totalrecords = MaterialIssueNotelist.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat1 = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,

                            rows = (from items in MaterialIssueNotelist.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                               items.IssNoteId.ToString(),
                               items.IssNoteNumber,
                               items.ProcessedBy,
                               items.RequiredForCampus,
                               items.RequiredForStore,
                               items.RequiredFromStore,
                               items.RequestedDate!= null ? items.RequestedDate.Value.ToString("dd/MM/yyyy") : null,
                               items.IssueDate!= null ? items.IssueDate.Value.ToString("dd/MM/yyyy") : null,
                               items.IssuedBy,
                               items.RequestStatus,
                               items.DeliveredThrough,
                               items.DeliveryDetails,
                               items.DeliveryDate!= null ? items.DeliveryDate.Value.ToString("dd/MM/yyyy") : null
                                    }
                                    })
                        };
                        return Json(jsondat1, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(null);

                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult ShowMaterialIssueNote(long? Id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    UserService us = new UserService();
                    StoreService ss = new StoreService();
                    if (Id > 0)
                    {
                        MaterialIssueNote min = ss.GetMaterialIssueNoteById(Convert.ToInt64(Id));
                        min.RequestNumber = "MRF-" + min.RequestId;
                        return View(min);
                    }
                    else
                        return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult IssuedItemsList(long? Id, string ExportType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    StoreService ss = new StoreService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();

                    sord = sord == "desc" ? "Desc" : "Asc";

                    criteria.Add("IssNoteId", Id);
                    Dictionary<long, IList<MaterialIssueNote_vw>> MaterialIssueNote_vw = ss.GetMaterialMaterialIssueNote_vwListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);

                    if (MaterialIssueNote_vw != null && MaterialIssueNote_vw.Count > 0)
                    {
                        MaterialIssueNote min = ss.GetMaterialIssueNoteById(Convert.ToInt64(Id));
                        if (ExportType == "PDF")
                        {
                            var IssList = (from items in MaterialIssueNote_vw.First().Value
                                           where items.IssueQty != 0
                                           select new
                                           {
                                               items.RequestType,
                                               items.RequiredForGrade,
                                               items.Section,
                                               items.RequiredFor,
                                               items.Material,
                                               items.Units,
                                               items.IssueQty
                                           }).ToList();
                            DataTable dt = ListToDataTable(IssList);
                            // string[] TblHeaders = new string[] { "Request Type", "Required For Grade", "Section", "Required For", "Material", "Units", "Issued Qty" };
                            ExportToPDF_IssueNoteDetails(min, dt);
                            //return new EmptyResult();
                        }
                        else
                        {
                            long totalrecords = MaterialIssueNote_vw.First().Key;
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var jsondat1 = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalrecords,

                                rows = (from items in MaterialIssueNote_vw.First().Value
                                        where items.IssueQty != 0
                                        select new
                                        {
                                            cell = new string[] {
                               items.Id.ToString(),
                               items.MRLId.ToString(),
                               items.RequestType,
                               items.RequiredForGrade,
                               items.Section,
                               items.RequiredFor,
                               items.Material,
                               items.MaterialGroup,
                               items.MaterialSubGroup,
                               items.Units,
                               items.RequiredDate != null ? items.RequiredDate.Value.ToString("dd/MM/yyyy") : null,
                               items.Status,
                               items.RequestQty.ToString(),
                               items.ApprovedQty.ToString(),
                               items.IssueQty.ToString()
                            }
                                        })
                            };
                            return Json(jsondat1, JsonRequestBehavior.AllowGet);
                        }
                        return Json(null);
                    }
                    else
                    {
                        return Json(null);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        #region "Common Methods for PDF Generation"
        public PdfPCell formatCell(string CellData, string FontName, float FontSize, bool isBold, bool isUnderline, int HAlign, int vAlign, float pTop, float pBottom, int Colspan, int Rowspan, int brdr)
        {
            try
            {
                Colspan = Colspan == 0 ? 1 : Colspan;
                Rowspan = Rowspan == 0 ? 1 : Rowspan;
                PdfPCell frmtCell = new PdfPCell(new Phrase(CellData, new iTextSharp.text.Font(FontFactory.GetFont(FontName, FontSize))));

                if (isBold && isUnderline)
                {
                    frmtCell = new PdfPCell(new Phrase(CellData, new iTextSharp.text.Font(FontFactory.GetFont(FontName, FontSize, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE))));
                }
                else if (isBold)
                {
                    frmtCell = new PdfPCell(new Phrase(CellData, new iTextSharp.text.Font(FontFactory.GetFont(FontName, FontSize, iTextSharp.text.Font.BOLD))));
                }
                else if (isUnderline)
                {
                    frmtCell = new PdfPCell(new Phrase(CellData, new iTextSharp.text.Font(FontFactory.GetFont(FontName, FontSize, iTextSharp.text.Font.UNDERLINE))));
                }

                frmtCell.HorizontalAlignment = HAlign;
                frmtCell.VerticalAlignment = vAlign;
                frmtCell.PaddingBottom = pBottom == 0 ? 2 : pBottom;
                frmtCell.PaddingTop = pTop == 0 ? 2 : pTop;
                frmtCell.Colspan = Colspan;
                frmtCell.Rowspan = Rowspan;
                if (brdr > 0)
                { frmtCell.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.TOP_BORDER; }
                else
                { frmtCell.Border = PdfPCell.NO_BORDER; }

                return frmtCell;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public PdfPTable CreateTable(int NoOfColumns, int[] relativeWidth)
        {
            try
            {
                PdfPTable CrTbl = new PdfPTable(NoOfColumns);
                CrTbl.SetWidths(relativeWidth);
                CrTbl.WidthPercentage = 100;
                //CrTbl.DefaultCell.Border = 1;
                CrTbl.DefaultCell.PaddingBottom = 10;
                CrTbl.HorizontalAlignment = Element.ALIGN_CENTER;
                CrTbl.DefaultCell.NoWrap = false;
                return CrTbl;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }
        #endregion

        public class PDFFooter : PdfPageEventHelper
        {
            public override void OnEndPage(PdfWriter writer, Document document)
            {
                try
                {
                    base.OnEndPage(writer, document);

                    var content = writer.DirectContent;
                    var pageBorderRect = new Rectangle(document.PageSize);

                    pageBorderRect.Left += document.LeftMargin;
                    pageBorderRect.Right -= document.RightMargin;
                    pageBorderRect.Top -= document.TopMargin;
                    pageBorderRect.Bottom += document.BottomMargin;

                    pageBorderRect.BorderWidth = 0;
                    content.SetColorStroke(BaseColor.BLACK);
                    content.Rectangle(pageBorderRect.Left, pageBorderRect.Bottom, pageBorderRect.Width, pageBorderRect.Height);
                    content.Stroke();
                }

                catch (Exception ex)
                {
                    ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                    throw ex;
                }
            }
        }

        public void ExportToPDF_IssueNoteDetails(MaterialIssueNote min, DataTable dt)
        {
            try
            {
                string ReqDate = min.RequestedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt");
                string IssDate = min.IssueDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt");

                #region "PDF File Properties - meta information"
                var strIssueNote = new iTextSharp.text.Document();
                MemoryStream pdfStream = new MemoryStream();
                PdfWriter pdfWriter = PdfWriter.GetInstance(strIssueNote, pdfStream);

                strIssueNote.AddCreator(Resources.Global.tips);

                // strIssueNote.AddTitle("Issue Note Details");
                strIssueNote.AddAuthor(Resources.Global.tips);
                #endregion "PDF File Properties - meta information"
                // calling PDFFooter class to Include in document
                pdfWriter.PageEvent = new PDFFooter();
                strIssueNote.Open();

                #region "Empty Cell"
                PdfPCell emptyCell = formatCell(" ", "ARIAL", 7.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0, 0);
                #endregion "Empty Cell"

                #region "PDF Header"
                iTextSharp.text.Image LogoImage;
                string ImagePath = ConfigurationManager.AppSettings["RptCard"] + "logo.jpg";

                LogoImage = iTextSharp.text.Image.GetInstance(ImagePath);
                LogoImage.ScaleAbsolute(50, 50);

                iTextSharp.text.Image LogonaceImage;
                string LogonaceImagePath = ConfigurationManager.AppSettings["AppLogos"] + "logonace.jpg";

                LogonaceImage = iTextSharp.text.Image.GetInstance(LogonaceImagePath);
                LogonaceImage.ScaleAbsolute(50, 50);

                PdfPTable test = CreateTable(3, new int[] { 35, 40, 10 });

                PdfPCell PdfPCell1 = new PdfPCell();
                PdfPCell1.Padding = 5;
                PdfPCell1.Border = 0;
                PdfPCell1.AddElement(LogonaceImage);
                PdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT;
                test.AddCell(PdfPCell1);

                PdfPCell PdfPCell2 = new PdfPCell(new Phrase("Material Issue Note", new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 9.0f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE))));
                PdfPCell2.Padding = 10;
                PdfPCell2.Border = 0;
                PdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT;
                test.AddCell(PdfPCell2);

                PdfPCell PdfPCell3 = new PdfPCell();
                PdfPCell3.Padding = 5;
                PdfPCell3.Border = 0;
                PdfPCell3.AddElement(LogoImage);
                PdfPCell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                test.AddCell(PdfPCell3);

                strIssueNote.Add(test);
                #endregion "PDF Header"

                #region "Issue note details"
                //Paragraph pCmstrHdr = new Paragraph("Material Issue Note", new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 8.0f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE)));
                //pCmstrHdr.Alignment = Element.ALIGN_CENTER;
                //pCmstrHdr.SpacingAfter = 3f;
                //pCmstrHdr.SpacingBefore = 3f;
                //strIssueNote.Add(pCmstrHdr);

                strIssueNote.Add(new Paragraph("\n"));
                PdfPTable tblissDtls = CreateTable(6, new int[] { 2, 24, 24, 24, 24, 2 });
                tblissDtls.AddCell(emptyCell);
                tblissDtls.AddCell(formatCell("Issue Note Number", "ARIAL", 6.0f, true, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 1, 1, 0, 0, 0));
                tblissDtls.AddCell(formatCell("Requestor", "ARIAL", 6.0f, true, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 1, 1, 0, 0, 0));
                tblissDtls.AddCell(formatCell("Required For Campus", "ARIAL", 6.0f, true, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 1, 1, 0, 0, 0));
                tblissDtls.AddCell(formatCell("Issued By", "ARIAL", 6.0f, true, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 1, 1, 0, 0, 0));
                tblissDtls.AddCell(emptyCell);

                tblissDtls.AddCell(emptyCell);
                tblissDtls.AddCell(formatCell(min.IssNoteNumber, "ARIAL", 6.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 1, 1, 0, 0, 0));
                tblissDtls.AddCell(formatCell(min.ProcessedBy, "ARIAL", 6.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 1, 1, 0, 0, 0));
                tblissDtls.AddCell(formatCell(min.RequiredForCampus, "ARIAL", 6.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 1, 1, 0, 0, 0));
                tblissDtls.AddCell(formatCell(min.IssuedBy, "ARIAL", 6.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 1, 1, 0, 0, 0));
                tblissDtls.AddCell(emptyCell);


                tblissDtls.AddCell(formatCell("", "ARIAL", 6.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 3, 3, 6, 0, 0));

                tblissDtls.AddCell(emptyCell);
                tblissDtls.AddCell(formatCell("Request No", "ARIAL", 6.0f, true, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 1, 1, 0, 0, 0));
                tblissDtls.AddCell(formatCell("Requested Date", "ARIAL", 6.0f, true, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 1, 1, 0, 0, 0));
                tblissDtls.AddCell(formatCell("Issue Date", "ARIAL", 6.0f, true, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 1, 1, 0, 0, 0));
                tblissDtls.AddCell(formatCell("Delivered Through", "ARIAL", 6.0f, true, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 1, 1, 0, 0, 0));
                tblissDtls.AddCell(emptyCell);

                tblissDtls.AddCell(emptyCell);
                tblissDtls.AddCell(formatCell("MRF-" + min.RequestId, "ARIAL", 6.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 1, 1, 0, 0, 0));
                tblissDtls.AddCell(formatCell(ReqDate, "ARIAL", 6.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 1, 1, 0, 0, 0));
                tblissDtls.AddCell(formatCell(IssDate, "ARIAL", 6.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 1, 1, 0, 0, 0));
                tblissDtls.AddCell(formatCell(min.DeliveredThrough, "ARIAL", 6.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 1, 1, 0, 0, 0));
                tblissDtls.AddCell(emptyCell);

                tblissDtls.AddCell(formatCell("", "ARIAL", 6.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 3, 3, 6, 0, 0));

                tblissDtls.AddCell(emptyCell);
                tblissDtls.AddCell(formatCell("Delivery Details", "ARIAL", 6.0f, true, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 1, 1, 0, 0, 0));
                tblissDtls.AddCell(formatCell("DC Number", "ARIAL", 6.0f, true, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 1, 1, 0, 0, 0));
                tblissDtls.AddCell(formatCell("Required For Store", "ARIAL", 6.0f, true, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 1, 1, 0, 0, 0));
                tblissDtls.AddCell(formatCell("Required From Store", "ARIAL", 6.0f, true, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 1, 1, 0, 0, 0));
                tblissDtls.AddCell(emptyCell);
                // tblissDtls.AddCell(formatCell("", "ARIAL", 6.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 1, 1, 4, 0, 0));

                tblissDtls.AddCell(emptyCell);
                tblissDtls.AddCell(formatCell(min.DeliveryDetails, "ARIAL", 6.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 1, 1, 0, 0, 0));
                tblissDtls.AddCell(formatCell(min.DCNumber, "ARIAL", 6.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 1, 1, 0, 0, 0));
                tblissDtls.AddCell(formatCell(min.RequiredForStore, "ARIAL", 6.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 1, 1, 0, 0, 0));
                tblissDtls.AddCell(formatCell(min.RequiredFromStore, "ARIAL", 6.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 1, 1, 0, 0, 0));
                tblissDtls.AddCell(emptyCell);
                //tblissDtls.AddCell(formatCell("", "ARIAL", 6.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 1, 1, 4, 0, 0));

                tblissDtls.AddCell(formatCell("", "ARIAL", 6.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 3, 3, 6, 0, 0));

                tblissDtls.AddCell(emptyCell);
                tblissDtls.AddCell(formatCell("Requestor Remarks", "ARIAL", 6.0f, true, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 1, 1, 4, 0, 0));
                tblissDtls.AddCell(emptyCell);

                tblissDtls.AddCell(emptyCell);
                tblissDtls.AddCell(formatCell(min.RequestorDescription, "ARIAL", 6.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 1, 1, 4, 0, 0));
                tblissDtls.AddCell(emptyCell);

                strIssueNote.Add(tblissDtls);
                strIssueNote.Add(new Paragraph("\n"));
                #endregion "Issue note details"

                #region "Issue note Items"
                PdfPTable tblIssItemDtls = CreateTable(9, new int[] { 2, 14, 14, 14, 20, 14, 14, 14, 2 });
                tblIssItemDtls.AddCell(emptyCell);
                tblIssItemDtls.AddCell(formatCell("Request Type", "ARIAL", 6.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 1, 1, 0, 0, 1));
                tblIssItemDtls.AddCell(formatCell("Required For Grade", "ARIAL", 6.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 1, 1, 0, 0, 1));
                tblIssItemDtls.AddCell(formatCell("Section", "ARIAL", 6.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 1, 1, 0, 0, 1));
                tblIssItemDtls.AddCell(formatCell("Required For", "ARIAL", 6.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 1, 1, 0, 0, 1));
                tblIssItemDtls.AddCell(formatCell("Material", "ARIAL", 6.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 1, 1, 0, 0, 1));
                tblIssItemDtls.AddCell(formatCell("Units", "ARIAL", 6.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 1, 1, 0, 0, 1));
                tblIssItemDtls.AddCell(formatCell("Issued Qty", "ARIAL", 6.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 1, 1, 0, 0, 1));
                tblIssItemDtls.AddCell(emptyCell);

                for (int rows = 0; rows < dt.Rows.Count; rows++)
                {
                    tblIssItemDtls.AddCell(emptyCell);
                    for (int column = 0; column < dt.Columns.Count; column++)
                    {
                        tblIssItemDtls.AddCell(formatCell(dt.Rows[rows][column].ToString(), "ARIAL", 6.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 1, 1, 0, 0, 1));
                    }
                    tblIssItemDtls.AddCell(emptyCell);
                }
                strIssueNote.Add(tblIssItemDtls);
                strIssueNote.Add(new Paragraph("\n"));
                strIssueNote.Add(new Paragraph("\n"));
                #endregion "Issue note Items"

                #region "signature"
                PdfPTable tblsgn1 = CreateTable(7, new int[] { 2, 16, 16, 20, 16, 16, 2 });
                tblsgn1.AddCell(formatCell("Acknowledged By(Store Incharge)", "ARIAL", 6.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 1, 1, 3, 0, 0));
                tblsgn1.AddCell(emptyCell);
                tblsgn1.AddCell(formatCell("Received Date", "ARIAL", 6.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 1, 1, 3, 0, 0));
                strIssueNote.Add(tblsgn1);

                strIssueNote.Add(new Paragraph("\n"));
                strIssueNote.Add(new Paragraph("\n"));

                PdfPTable tblsgn2 = CreateTable(7, new int[] { 2, 16, 16, 20, 16, 16, 2 });
                tblsgn2.AddCell(formatCell("(Seal & Signature)", "ARIAL", 6.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 1, 1, 3, 0, 0));
                tblsgn2.AddCell(emptyCell);
                tblsgn2.AddCell(formatCell("Received By", "ARIAL", 6.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 1, 1, 3, 0, 0));
                strIssueNote.Add(tblsgn2);
                strIssueNote.Add(new Paragraph("\n"));
                #endregion "signature"

                #region "Pdf Document Close"
                strIssueNote.Close();
                pdfWriter.Close();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.ContentType = "application/pdf";
                Response.AppendHeader("Content-Disposition", "attachment; filename=IssueNote_Details_" + min.IssNoteNumber + ".pdf");
                Response.BinaryWrite(pdfStream.ToArray());
                Response.End();
                #endregion "Pdf Document Close"
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult StoreReports()
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
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public PartialViewResult MaterialInwardReport()
        {
            try
            {
                return PartialView();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult MaterialInwardReportListJqGrid(MatInward_SkuList_vw ms, string fromDate, string toDate, string Expt, int? rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                StoreService ss = new StoreService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if ((!string.IsNullOrEmpty(fromDate) && !(string.IsNullOrEmpty(toDate))))
                {
                    fromDate = fromDate.Trim();
                    toDate = toDate.Trim();
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    DateTime FromDate = DateTime.Parse(fromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    DateTime ToDate = DateTime.Parse(toDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    string To = string.Format("{0:dd/MM/yyyy}", ToDate);
                    DateTime TDate = Convert.ToDateTime(To + " " + "23:59:59.000");
                    DateTime[] fromto = new DateTime[2];
                    fromto[0] = FromDate;
                    fromto[1] = TDate;
                    criteria.Add("CreatedDate", fromto);

                    if (!string.IsNullOrWhiteSpace(ms.InwardNumber)) criteria.Add("InwardNumber", ms.InwardNumber);
                    if (!string.IsNullOrWhiteSpace(ms.Campus)) criteria.Add("Campus", ms.Campus);
                    if (!string.IsNullOrWhiteSpace(ms.Store)) criteria.Add("Store", ms.Store);
                    if (!string.IsNullOrWhiteSpace(ms.Supplier)) criteria.Add("Supplier", ms.Supplier);
                    if (!string.IsNullOrWhiteSpace(ms.ReceivedBy)) criteria.Add("ReceivedBy", ms.ReceivedBy);
                    if (!string.IsNullOrWhiteSpace(ms.Material)) criteria.Add("Material", ms.Material);
                    if (!string.IsNullOrWhiteSpace(ms.MaterialGroup))
                        criteria.Add("MaterialGroup", ms.MaterialGroup);
                    if (!string.IsNullOrWhiteSpace(ms.MaterialSubGroup))
                        criteria.Add("MaterialSubGroup", ms.MaterialSubGroup);
                    if (!string.IsNullOrWhiteSpace(ms.SuppRefNo)) criteria.Add("SuppRefNo", ms.SuppRefNo);
                    if (!string.IsNullOrWhiteSpace(ms.DamageDescription)) criteria.Add("DamageDescription", ms.DamageDescription);

                    if (ms.OrderQty >= 0)
                        criteria.Add("OrderQty", ms.OrderQty);
                    if (!string.IsNullOrWhiteSpace(ms.OrderedUnits))
                        criteria.Add("OrderedUnits", ms.OrderedUnits);

                    if (ms.ReceivedQty >= 0)
                        criteria.Add("ReceivedQty", ms.ReceivedQty);
                    if (!string.IsNullOrWhiteSpace(ms.ReceivedUnits))
                        criteria.Add("ReceivedUnits", ms.ReceivedUnits);

                    if (ms.UnitPrice >= 0)
                        criteria.Add("UnitPrice", ms.UnitPrice);
                    if (ms.TotalPrice >= 0)
                        criteria.Add("TotalPrice", ms.TotalPrice);
                    sord = sord == "desc" ? "Desc" : "Asc";
                    Dictionary<long, IList<MatInward_SkuList_vw>> MaterialInward_SkuList = ss.GetMatInward_SkuList_vwListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    if (MaterialInward_SkuList != null && MaterialInward_SkuList.Count > 0)
                    {
                        if (Expt == "Excel")
                        {
                            var List = MaterialInward_SkuList.First().Value.ToList();
                            base.ExptToXL(List, "MaterialInwardList", (items => new
                            {
                                items.InwardNumber,
                                items.Campus,
                                items.Store,
                                items.Supplier,
                                items.SuppRefNo,
                                InvoiceDate = items.InvoiceDate != null ? items.InvoiceDate.ToString("dd/MM/yyyy") : null,
                                ReceivedDateTime = items.ReceivedDateTime != null ? items.ReceivedDateTime.ToString("dd/MM/yyyy hh:mm:ss tt") : null,
                                items.ReceivedBy,
                                CreatedDate = items.CreatedDate != null ? items.CreatedDate.ToString("dd/MM/yyyy hh:mm:ss tt") : null,
                                items.Material,
                                OrderQty = items.OrderQty.ToString(),
                                items.OrderedUnits,
                                ReceivedQty = items.ReceivedQty.ToString(),
                                items.ReceivedUnits,
                                UnitPrice = items.UnitPrice.ToString(),
                                TotalPrice = items.TotalPrice.ToString(),
                                items.DamageDescription
                            }));
                            return new EmptyResult();
                        }

                        else
                        {
                            long totalrecords = MaterialInward_SkuList.First().Key;
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var jsondat1 = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalrecords,

                                rows = (from items in MaterialInward_SkuList.First().Value
                                        select new
                                        {
                                            i = 2,
                                            cell = new string[] {
                               items.Id.ToString(),
                               items.InwardNumber,
                               items.Campus,
                               items.Store,
                               items.Supplier,
                               items.SuppRefNo,
                               items.InvoiceDate!= null ? items.InvoiceDate.ToString("dd/MM/yyyy") : null,
                               items.ReceivedDateTime!= null ? items.ReceivedDateTime.ToString("dd/MM/yyyy hh:mm:ss tt") : null,
                               items.ReceivedBy,
                               items.CreatedDate!= null ? items.CreatedDate.ToString("dd/MM/yyyy hh:mm:ss tt") : null,                               
                               items.MaterialGroup,
                               items.MaterialSubGroup,
                               items.Material,
                               items.OrderQty.ToString(),
                               items.OrderedUnits,
                               items.ReceivedQty.ToString(),
                               items.ReceivedUnits,
                               items.UnitPrice.ToString(),
                               items.TotalPrice.ToString(),
                               items.DamageDescription
                                    }
                                        })
                            };
                            return Json(jsondat1, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    { return Json(null, JsonRequestBehavior.AllowGet); }
                }
                else
                { return Json(null, JsonRequestBehavior.AllowGet); }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public PartialViewResult MaterialRequestReport()
        {
            try
            {
                return PartialView();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult MaterialRequestReportListJqGrid(MatReq_ReqList_vw MatReq, string fromDate, string toDate, string Expt, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                StoreService ss = new StoreService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if ((!string.IsNullOrEmpty(fromDate) && !(string.IsNullOrEmpty(toDate))))
                {
                    fromDate = fromDate.Trim();
                    toDate = toDate.Trim();
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    DateTime FromDate = DateTime.Parse(fromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    DateTime ToDate = DateTime.Parse(toDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    string To = string.Format("{0:dd/MM/yyyy}", ToDate);
                    DateTime TDate = Convert.ToDateTime(To + " " + "23:59:59.000");
                    DateTime[] fromto = new DateTime[2];
                    fromto[0] = FromDate;
                    fromto[1] = TDate;
                    criteria.Add("RequestedDate", fromto);
                    if (!string.IsNullOrEmpty(MatReq.RequestNumber)) criteria.Add("RequestNumber", MatReq.RequestNumber);
                    if (!string.IsNullOrEmpty(MatReq.RequiredForCampus)) criteria.Add("RequiredForCampus", MatReq.RequiredForCampus);
                    if (!string.IsNullOrEmpty(MatReq.ProcessedBy)) criteria.Add("ProcessedBy", MatReq.ProcessedBy);
                    if (!string.IsNullOrEmpty(MatReq.RequestType)) criteria.Add("RequestType", MatReq.RequestType);
                    if (!string.IsNullOrEmpty(MatReq.RequiredForGrade)) criteria.Add("RequiredForGrade", MatReq.RequiredForGrade);
                    if (!string.IsNullOrEmpty(MatReq.RequiredFor)) criteria.Add("RequiredFor", MatReq.RequiredFor);
                    if (!string.IsNullOrEmpty(MatReq.Material)) criteria.Add("Material", MatReq.Material);

                    if (!string.IsNullOrEmpty(MatReq.MaterialGroup)) criteria.Add("MaterialGroup", MatReq.MaterialGroup);
                    if (!string.IsNullOrEmpty(MatReq.MaterialSubGroup)) criteria.Add("MaterialSubGroup", MatReq.MaterialSubGroup);

                    if (!string.IsNullOrEmpty(MatReq.Units)) criteria.Add("Units", MatReq.Units);
                    if (MatReq.Quantity >= 0) criteria.Add("Quantity", MatReq.Quantity);
                    if (MatReq.ApprovedQty >= 0) criteria.Add("ApprovedQty", MatReq.ApprovedQty);
                    if (MatReq.IssuedQty >= 0) criteria.Add("IssuedQty", MatReq.IssuedQty);
                    if (!string.IsNullOrEmpty(MatReq.Status)) criteria.Add("Status", MatReq.Status);
                    sord = sord == "desc" ? "Desc" : "Asc";
                    Dictionary<long, IList<MatReq_ReqList_vw>> MatReq_ReqList = ss.GetMatReq_ReqList_vwListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    if (MatReq_ReqList != null && MatReq_ReqList.Count > 0)
                    {
                        if (Expt == "Excel")
                        {
                            var List = MatReq_ReqList.First().Value.ToList();
                            base.ExptToXL(List, "MaterialRequestList", (items => new
                            {
                                items.RequestNumber,
                                items.RequiredForCampus,
                                items.ProcessedBy,
                                RequestedDate = items.RequestedDate != null ? items.RequestedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : null,
                                items.RequestType,
                                items.RequiredForGrade,
                                items.RequiredFor,
                                items.MaterialGroup,
                                items.MaterialSubGroup,
                                items.Material,
                                items.Units,
                                RequiredDate = items.RequiredDate != null ? items.RequiredDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : null,
                                ReqQty = items.Quantity.ToString(),
                                ApprovedQty = items.ApprovedQty.ToString(),
                                IssuedQty = items.IssuedQty.ToString(),
                                items.Status
                            }));
                            return new EmptyResult();
                        }

                        else
                        {

                            long totalrecords = MatReq_ReqList.First().Key;
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var jsondat1 = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalrecords,

                                rows = (from items in MatReq_ReqList.First().Value
                                        select new
                                        {
                                            i = 2,
                                            cell = new string[] {
                               items.Id.ToString(),
                               items.RequestNumber,
                               items.RequiredForCampus,
                               items.ProcessedBy,
                               items.RequestedDate!= null ? items.RequestedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : null,
                               items.RequestType,
                               items.RequiredForGrade,
                               items.RequiredFor,                               
                               items.MaterialGroup,
                               items.MaterialSubGroup,
                               items.Material,
                               items.Units,
                               items.RequiredDate!= null ? items.RequiredDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : null,
                               items.Quantity.ToString(),
                               items.ApprovedQty.ToString(),
                               items.IssuedQty.ToString(),
                               items.Status
                                    }
                                        })
                            };
                            return Json(jsondat1, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    { return Json(null, JsonRequestBehavior.AllowGet); }
                }
                else
                { return Json(null, JsonRequestBehavior.AllowGet); }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public PartialViewResult MaterialIssueNoteReport()
        {
            try
            {
                return PartialView();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult MaterialIssueNoteReportListJqGrid(MatIssNote_RequestList_vw min, string RequestNumber, decimal? TotalPrice, int? IssueQty, string fromDate, string toDate, string Expt, int? rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                StoreService ss = new StoreService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if ((!string.IsNullOrEmpty(fromDate) && !(string.IsNullOrEmpty(toDate))))
                {
                    fromDate = fromDate.Trim();
                    toDate = toDate.Trim();
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    DateTime FromDate = DateTime.Parse(fromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    DateTime ToDate = DateTime.Parse(toDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    string To = string.Format("{0:dd/MM/yyyy}", ToDate);
                    DateTime TDate = Convert.ToDateTime(To + " " + "23:59:59.000");
                    DateTime[] fromto = new DateTime[2];
                    fromto[0] = FromDate;
                    fromto[1] = TDate;
                    criteria.Add("IssueDate", fromto);
                    if (!string.IsNullOrWhiteSpace(min.IssNoteNumber)) criteria.Add("IssNoteNumber", min.IssNoteNumber);
                    if (!string.IsNullOrWhiteSpace(min.ProcessedBy)) criteria.Add("ProcessedBy", min.ProcessedBy);
                    if (!string.IsNullOrWhiteSpace(min.IssuedBy)) criteria.Add("IssuedBy", min.IssuedBy);
                    if (!string.IsNullOrWhiteSpace(min.RequiredForCampus)) criteria.Add("RequiredForCampus", min.RequiredForCampus);
                    if (!string.IsNullOrWhiteSpace(min.RequiredForStore)) criteria.Add("RequiredForStore", min.RequiredForStore);
                    if (!string.IsNullOrWhiteSpace(min.DeliveredThrough)) criteria.Add("DeliveredThrough", min.DeliveredThrough);
                    if (!string.IsNullOrWhiteSpace(min.DeliveryDetails)) criteria.Add("DeliveryDetails", min.DeliveryDetails);
                    if (!string.IsNullOrWhiteSpace(min.RequestType)) criteria.Add("RequestType", min.RequestType);
                    if (!string.IsNullOrWhiteSpace(min.RequiredForGrade)) criteria.Add("RequiredForGrade", min.RequiredForGrade);
                    if (!string.IsNullOrWhiteSpace(min.RequiredFor)) criteria.Add("RequiredFor", min.RequiredFor);
                    if (!string.IsNullOrWhiteSpace(min.Material)) criteria.Add("Material", min.Material);
                    if (!string.IsNullOrWhiteSpace(min.MaterialGroup))
                        criteria.Add("MaterialGroup", min.MaterialGroup);
                    if (!string.IsNullOrWhiteSpace(min.MaterialSubGroup))
                        criteria.Add("MaterialSubGroup", min.MaterialSubGroup);
                    if (!string.IsNullOrWhiteSpace(RequestNumber))
                    {
                        string[] ReqNumber = RequestNumber.Split('-');
                        if (ReqNumber.Length == 2)
                        {
                            criteria.Add("RequestId", Convert.ToInt64(ReqNumber[1]));
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(min.Units)) criteria.Add("Units", min.Units);
                    if (IssueQty >= 0) criteria.Add("IssueQty", IssueQty);
                    if (TotalPrice >= 0) criteria.Add("TotalPrice", TotalPrice);

                    sord = sord == "desc" ? "Desc" : "Asc";
                    Dictionary<long, IList<MatIssNote_RequestList_vw>> MatIssNote_RequestList = ss.GetMatIssNote_RequestList_vwListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    if (MatIssNote_RequestList != null && MatIssNote_RequestList.Count > 0)
                    {
                        if (Expt == "Excel")
                        {
                            var List = MatIssNote_RequestList.First().Value.ToList();
                            base.ExptToXL(List, "IssueNoteList", (items => new
                            {
                                items.IssNoteNumber,
                                RequestNumber = "MRF-" + items.RequestId,
                                items.ProcessedBy,
                                items.IssuedBy,
                                IssueDate = items.IssueDate != null ? items.IssueDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : null,
                                items.RequiredForCampus,
                                items.RequiredForStore,
                                items.DeliveredThrough,
                                items.DeliveryDetails,
                                items.RequestType,
                                items.RequiredForGrade,
                                items.RequiredFor,
                                items.MaterialGroup,
                                items.MaterialSubGroup,
                                items.Material,
                                items.Units,
                                IssueQty = items.IssueQty.ToString(),
                                TotalPrice = items.TotalPrice.ToString()
                            }));
                            return new EmptyResult();
                        }
                        else
                        {
                            long totalrecords = MatIssNote_RequestList.First().Key;
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var jsondat1 = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalrecords,
                                rows = (from items in MatIssNote_RequestList.First().Value
                                        select new
                                        {
                                            i = 2,
                                            cell = new string[] {
                               items.Id.ToString(),
                               items.IssNoteNumber,
                               "MRF-"+items.RequestId.ToString(),
                               items.ProcessedBy,
                               items.IssuedBy,
                               items.IssueDate!= null ? items.IssueDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : null,
                               items.RequiredForCampus,
                               items.RequiredForStore,
                               items.DeliveredThrough,
                               items.DeliveryDetails,
                               items.RequestType,
                               items.RequiredForGrade,
                               items.RequiredFor,                               
                               items.MaterialGroup,
                               items.MaterialSubGroup,
                               items.Material,
                               items.Units,
                               items.IssueQty.ToString(),
                               items.TotalPrice.ToString()
                                    }
                                        })
                            };
                            return Json(jsondat1, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    { return Json(null, JsonRequestBehavior.AllowGet); }
                }
                else
                { return Json(null, JsonRequestBehavior.AllowGet); }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public JsonResult FillDepartment()
        {
            try
            {
                MasterDataService mds = new MasterDataService();
                Dictionary<long, IList<IssueGroupMaster>> IssueGroupList = mds.GetIssueGroupListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, null);
                var IssueGroup1 = (
                         from items in IssueGroupList.First().Value
                         where items.IssueGroup != "Parent Portal" & items.IssueGroup != "Store"
                         //&& items.IssueGroup != "Store"
                         select new
                         {
                             Text = items.IssueGroup,
                             Value = items.IssueGroup
                         }).Distinct().ToList();
                return Json(IssueGroup1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult GetMaterials(string term)
        {
            try
            {
                StoreService ss = new StoreService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Material", term);
                criteria.Add("IsActive", true);
                Dictionary<long, IList<MaterialsMaster>> MaterialsList = ss.GetAutoCompleteMaterialsMasterlistWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                var Materials = (from u in MaterialsList.First().Value
                                 where u.Material != null
                                 select u.Material).Distinct().ToList();
                return Json(Materials, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CustomAccountPolicy");
                throw ex;
            }
        }

        public ActionResult DirectStockUpdate()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    StockTransaction st = new StockTransaction();
                    st.Store = "";
                    return View(st);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult DirectStockUpdate(StockTransaction st)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    string Mat = Request["txtMaterial"];
                    StoreService ss = new StoreService();
                    MaterialsMaster mm = ss.GetMaterialsMasterByMaterial(Mat.Trim());
                    if (mm != null)
                    {
                        st.TransactionCode = "Direct Update";
                        st.ItemId = mm.Id;
                        st.Units = mm.UnitCode;
                        st.TransactionDate = DateTime.Now;
                        st.TransactionBy = userId;

                        ss.CreateOrUpdateStockTransaction(st);
                        TempData["SubmitSuccessMsg"] = "Stock Updated Successfully";
                        return View();
                    }
                    else
                    {
                        var script = @"ErrMsg(""Please type exact Material Name"");";
                        return JavaScript(script);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public void SendMailToMaterialRequestor(MaterialRequest mr)
        {
            try
            {
                string SendEmail = ConfigurationManager.AppSettings["SendEmail1"];
                string From = ConfigurationManager.AppSettings["From"];
                if (SendEmail == "false")
                    return;
                else
                {
                    try
                    {
                        UserService us = new UserService();
                        StoreService ss = new StoreService();
                        Dictionary<long, IList<UserAppRole>> userAppRole = new Dictionary<long, IList<UserAppRole>>();
                        Dictionary<string, object> criteriaUserAppRole = new Dictionary<string, object>();
                        criteriaUserAppRole.Add("UserId", mr.ProcessedBy);
                        criteriaUserAppRole.Add("AppCode", "STR");
                        string ToAddress = "";
                        userAppRole = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteriaUserAppRole);
                        if (userAppRole != null && userAppRole.Count > 0 && userAppRole.First().Key > 0)
                        {
                            ToAddress = userAppRole.First().Value[0].Email;
                        }
                        Dictionary<string, object> criteria = new Dictionary<string, object>();
                        criteria.Add("MatReqRefId", mr.Id);
                        Dictionary<long, IList<MaterialRequestList>> MaterialRequestList = ss.GetMaterialRequestListListWithPagingAndCriteria(0, 9999, "Id", "Asc", criteria);
                        string body = "Dear Sir/Madam, <br/><br/>";
                        body = body + "Please find below the Approved Qty list for Material Request No " + mr.RequestNumber + " <br/><br/>";
                        body = body + "<table style='border-collapse:collapse; border:1px solid black; padding: 5px;'> ";

                        body = body + "<tr> <td style='font-weight:bold; border:1px solid black; padding: 5px;'><b> Material Name </b></td> <td style='font-weight:bold; border:1px solid black; padding: 5px;'> <b>Requested Qty</b> </td> <td style='font-weight:bold; border:1px solid black; padding: 5px;'> <b>Approved Qty</b> </td>  <tr> ";
                        if (MaterialRequestList != null && MaterialRequestList.FirstOrDefault().Value != null && MaterialRequestList.FirstOrDefault().Value.Count > 0)
                        {
                            for (int i = 0; i < MaterialRequestList.First().Key; i++)
                            {
                                body = body + "<tr> <td style='font-weight:bold; border:1px solid black; padding: 5px;'> " + MaterialRequestList.FirstOrDefault().Value[i].Material + " </td> <td style='font-weight:bold; border:1px solid black; padding: 5px;'> " + MaterialRequestList.FirstOrDefault().Value[i].Quantity + " </td> <td style='font-weight:bold; border:1px solid black; padding: 5px;'>" + MaterialRequestList.FirstOrDefault().Value[i].ApprovedQty + " </td>  <tr> ";
                            }
                        }
                        body = body + "</table><br/><br/>";

                        body = body + "<b>Approver Comments:</b><br/><br/>";

                        body = body + mr.ApproverComments;

                        IList<CampusEmailId> campusemaildet = GetEmailIdByCampus(mr.Campus, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
                        System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                        mail.To.Add(ToAddress);
                        mail.IsBodyHtml = true;
                        SmtpClient smtp = new SmtpClient("localhost", 25);
                        smtp.Host = "smtp.gmail.com";
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.EnableSsl = true;
                        mail.Subject = "Approved Qty list for Material Request No " + mr.RequestNumber + "";

                        mail.Body = body;
                        if (From == "live")
                        {
                            try
                            {
                                mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
                                smtp.Credentials = new System.Net.NetworkCredential
                              (campusemaildet.First().EmailId.ToString(), campusemaildet.First().Password.ToString());
                                if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                                {
                                    smtp.Send(mail);
                                }
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
                                        smtp.Send(mail);
                                    }
                                }
                            }
                        }
                        else if (From == "test")
                        {
                            mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
                            smtp.Credentials = new System.Net.NetworkCredential
                           (campusemaildet.First().EmailId.ToString(), campusemaildet.First().Password.ToString());
                            if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                            {
                                smtp.Send(mail);
                            }
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }
        #region Store Report Added By Micheal

        public ActionResult MaterialInwardOutwardReport()
        {
            string userId = base.ValidateUser();
            if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
            else
            {
                MastersService ms = new MastersService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<long, IList<CampusMaster>> Campus = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                ViewBag.ddlcampus = Campus.First().Value;
                #region BreadCrumb
                string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                #endregion
                DateTime daytime = DateTime.Now;
                ViewBag.curmonth = daytime.Month;
                return View();
            }
        }
        #endregion

        public ActionResult MaterialInwardMonthlyReport()
        {
            string userId = base.ValidateUser();
            if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
            else
            {
                MastersService ms = new MastersService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<long, IList<CampusMaster>> Campus = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                ViewBag.ddlcampus = Campus.First().Value;
                #region BreadCrumb
                string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                #endregion
                DateTime daytime = DateTime.Now;
                ViewBag.curmonth = daytime.Month;
                return View();
            }

        }

        public ActionResult ActOnMaterialRequestJqGrid(string Store, long Id, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    StoreService ss = new StoreService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    sord = sord == "desc" ? "Desc" : "Asc";
                    criteria.Add("MatReqRefId", Id);
                    Dictionary<long, IList<MaterialRequestList>> MaterialRequestlist = ss.GetMaterialRequestListListWithPagingAndCriteria(page - 1, rows, sord, sidx, criteria);
                    if (MaterialRequestlist != null && MaterialRequestlist.Count > 0 && MaterialRequestlist.First().Key > 0)
                    {
                        for (int i = 0; i < MaterialRequestlist.First().Key; i++)
                        {
                            MaterialsMaster mm = ss.GetMaterialsMasterByMaterial(MaterialRequestlist.First().Value[i].Material.Trim());
                            criteria.Clear();
                            //criteria.Add("ItemId", mm.Id);
                            //if (!string.IsNullOrWhiteSpace(Store)) { criteria.Add("Store", Store); }
                            //criteria.Add("AMonth", DateTime.Now.Month);
                            //criteria.Add("AYear", DateTime.Now.Year);
                            //Dictionary<long, IList<StoreStockBalance>> StoreStockBalanceList = ss.GetStoreStockBalanceListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                            Dictionary<string, object> criteria1 = new Dictionary<string, object>();
                            criteria1.Add("Material", mm.Material.Trim());
                            criteria1.Add("MaterialInward." + "Store", Store);
                            string[] IssuedStatus = new[] { "Not Issued", "Partially Issued" };
                            criteria1.Add("IssuedStatus", IssuedStatus);
                            string[] alias = new string[1];
                            alias[0] = "MaterialInward";
                            Dictionary<long, IList<SkuList_MaterialInward>> SkuList = ss.GetSkuList_MaterialInwardListWithPagingAndCriteria(0, 9999, "SkuId", "Asc", criteria1, "", null, alias);
                            if (SkuList != null && SkuList.FirstOrDefault().Value != null && SkuList.Count() > 0 && SkuList.FirstOrDefault().Key > 0)
                            {
                                for (int j = 0; j < SkuList.FirstOrDefault().Value.Count; j++)
                                {
                                    MaterialRequestlist.First().Value[i].InwardIds = MaterialRequestlist.First().Value[i].InwardIds + Convert.ToString(SkuList.First().Value[j].MaterialRefId) + ", ";
                                    MaterialRequestlist.First().Value[i].AvailableQtys = MaterialRequestlist.First().Value[i].AvailableQtys + Convert.ToString(SkuList.First().Value[j].StockAvailableQty) + ", ";
                                    MaterialRequestlist.First().Value[i].UnitPrices = MaterialRequestlist.First().Value[i].UnitPrices + Convert.ToString(SkuList.First().Value[j].UnitPrice) + ", ";
                                    MaterialRequestlist.First().Value[i].Taxes = MaterialRequestlist.First().Value[i].Taxes + Convert.ToString(SkuList.First().Value[j].Tax) + ", ";
                                    MaterialRequestlist.First().Value[i].Discounts = MaterialRequestlist.First().Value[i].Discounts + Convert.ToString(SkuList.First().Value[j].Discount) + ", ";
                                }
                            }
                        }

                        long totalrecords = MaterialRequestlist.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat1 = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in MaterialRequestlist.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                               items.Id.ToString(),
                               items.RequestType,
                               items.RequiredForGrade,
                               items.Section,
                               items.RequiredFor,
                               items.Material,
                               items.MaterialGroup,
                               items.MaterialSubGroup,
                               items.Units,
                               items.RequiredDate != null ? items.RequiredDate.Value.ToString("dd/MM/yyyy") : null,
                               items.Status,
                               items.InwardIds,
                               items.AvailableQtys,
                               items.UnitPrices,
                               items.Taxes,
                               items.Discounts,
                               items.Quantity.ToString(),
                               items.ApprovedQty.ToString(),
                               items.IssuedQty.ToString()
                            }
                                    })
                        };
                        return Json(jsondat1, JsonRequestBehavior.AllowGet);
                    }
                    else
                    { return null; }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult MaterialSearchOnRejection(string Campus)
        {
            try
            {
                ViewBag.Campus = Campus;
                return View();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public JsonResult FillAutoCompleteStudentName(string Campus, string Grade, string Section, string term)
        {
            try
            {
                AdmissionManagementService ams = new AdmissionManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrWhiteSpace(Campus)) criteria.Add("Campus", Campus);
                if (!string.IsNullOrWhiteSpace(Grade)) criteria.Add("Grade", Grade);
                if (!string.IsNullOrWhiteSpace(Section)) criteria.Add("Section", Section);
                criteria.Add("Name", term);
                criteria.Add("AdmissionStatus", "Registered");
                Dictionary<long, IList<StudentTemplate>> StudentList = ams.GetStudentDetailsListWithLikesearchCriteria(0, 9999, "Name", "Asc", criteria);
                var StudentNames = (from u in StudentList.First().Value
                                    where u.Name != null && u.Name != ""
                                    select u.Name).Distinct().ToList();
                return Json(StudentNames, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult MutipleStudentSearch(string Campus)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) { return RedirectToAction("LogOff", "Account"); }
                else
                {
                    ViewBag.Campus = Campus;
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult MultipleStudentDetailsListJqGrid(string NewId, string Name, string Campus, string Grade, string Section, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MasterDataService sds = new MasterDataService();
                    AdmissionManagementService ams = new AdmissionManagementService();
                    Dictionary<string, object> Eqcriteria = new Dictionary<string, object>();
                    Dictionary<string, object> Likecriteria = new Dictionary<string, object>();
                    if (!string.IsNullOrWhiteSpace(NewId)) { Likecriteria.Add("NewId", NewId); }
                    if (!string.IsNullOrWhiteSpace(Name)) { Likecriteria.Add("Name", Name); }
                    if (!string.IsNullOrWhiteSpace(Campus)) { Eqcriteria.Add("Campus", Campus); }
                    if (!string.IsNullOrWhiteSpace(Grade)) { Eqcriteria.Add("Grade", Grade); }
                    if (!string.IsNullOrWhiteSpace(Section)) { Eqcriteria.Add("Section", Section); }
                    Eqcriteria.Add("AdmissionStatus", "Registered");
                    sord = sord == "desc" ? sord = "Desc" : sord = "Asc";
                    Dictionary<long, IList<StudentTemplateView>> studentdetailslist;
                    if (!string.IsNullOrWhiteSpace(Grade))
                    {
                        studentdetailslist = ams.GetStudentTemplateViewListWithLikeandExactPagingAndCriteria(0, 9999, sord, sidx, Eqcriteria, Likecriteria);
                        //studentdetailslist = ams.GetStudentTemplate1ListWithEQsearchCriteria(page - 1, rows, sord, sidx, criteria);
                    }
                    else
                    {
                        studentdetailslist = ams.GetStudentTemplateViewListWithLikeandExactPagingAndCriteria(0, 9999, sord, sidx, Eqcriteria, Likecriteria);
                        //studentdetailslist = ams.GetStudentTemplate1ListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    }
                    if (studentdetailslist != null && studentdetailslist.Count > 0)
                    {
                        long totalRecords = studentdetailslist.First().Key;
                        int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                        var jsonData = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalRecords,
                            rows = (
                            from items in studentdetailslist.First().Value
                            select new
                            {
                                i = items.Id,
                                cell = new string[] 
                       { 
                           items.Id.ToString(),
                           items.NewId,
                           items.Name,
                           items.Campus,
                           items.Grade,
                           items.Section,
                           items.IsHosteller.ToString()
                    }
                            })
                        };
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                    }
                    else
                    { return null; }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult FillMaterialByMaterialGroupIdAndSubGroupIdString(string MaterialGroup, string MaterialSubGroup)
        {
            try
            {
                StoreService ss = new StoreService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrWhiteSpace(MaterialGroup)) { criteria.Add("MaterialGroup", MaterialGroup); }
                if (!string.IsNullOrWhiteSpace(MaterialSubGroup)) { criteria.Add("MaterialSubGroup", MaterialSubGroup); }
                Dictionary<long, IList<MaterialsMaster>> MaterialsList = ss.GetMaterialsMasterlistWithPagingAndCriteria(0, 9999, null, null, criteria);
                var Materials = (from u in MaterialsList.FirstOrDefault().Value
                                 where u.Material != null
                                 select u.Material).Distinct().ToList();
                return Json(Materials, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult FillMaterialByMaterialGroupIdAndSubGroupId(long? MaterialGroupId, long? MaterialSubGroupId)
        {
            try
            {
                StoreService ss = new StoreService();
                IList<MaterialsMaster> MaterialsList = ss.GetMaterialByMaterialGroupAndMaterialSubGroup(Convert.ToInt64(MaterialGroupId), Convert.ToInt64(MaterialSubGroupId));
                var Materials = (from u in MaterialsList
                                 where u.Material != null
                                 select u.Material).Distinct().ToList();
                return Json(Materials, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        #region Material Issued Report added by Micheal
        public ActionResult MaterialIssueMonthlyReport()
        {
            #region BreadCrumb
            string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
            #endregion
            DateTime daytime = DateTime.Now;
            ViewBag.curmonth = daytime.Month;
            return View();
        }
        public ActionResult MaterialsIssueReportJqGrid(MaterialIssueReportView mir, string RequiredForCampus, string Material, string MaterialGroup, string MaterialSubGroup, int? IssuedMonth, int? IssuedYear, int? IssuedQty, int? rows, string sidx, string sord, int? page = 1, int? ExptXl = 0)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                StoreService ss = new StoreService();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (!string.IsNullOrEmpty(mir.RequiredFromStore))
                    criteria.Add("RequiredFromStore", mir.RequiredFromStore.Trim());
                if (!string.IsNullOrEmpty(mir.RequiredForStore))
                    criteria.Add("RequiredForStore", mir.RequiredForStore.Trim());
                if (!string.IsNullOrEmpty(RequiredForCampus))
                    criteria.Add("RequiredForCampus", RequiredForCampus.Trim());
                if (!string.IsNullOrEmpty(Material))
                    criteria.Add("Material", Material.Trim());
                if (!string.IsNullOrEmpty(MaterialGroup))
                    criteria.Add("MaterialGroup", MaterialGroup.Trim());
                if (!string.IsNullOrEmpty(MaterialSubGroup))
                    criteria.Add("MaterialSubGroup", MaterialSubGroup.Trim());
                if (IssuedMonth >= 0)
                    criteria.Add("IssuedMonth", IssuedMonth);
                if (IssuedYear >= 0)
                    criteria.Add("IssuedYear", IssuedYear);
                if (IssuedQty >= 0)
                    criteria.Add("IssuedQty", IssuedQty);
                Dictionary<long, IList<MaterialIssueReportView>> MaterialIssueList = ss.GetMaterialIssueListWithPagingAndCriteria_vw(page - 1, rows, sidx, sord, criteria);
                if (MaterialIssueList != null && MaterialIssueList.Count > 0 && MaterialIssueList.FirstOrDefault().Key > 0 && MaterialIssueList.FirstOrDefault().Value.Count > 0)
                {
                    if (ExptXl == 1)
                    {
                        var List = MaterialIssueList.First().Value.ToList();
                        base.ExptToXL(List, "MaterialIssueReportList", (items => new
                        {
                            items.Id,
                            items.RequiredFromStore,
                            items.RequiredForStore,
                            items.RequiredForCampus,
                            items.MaterialGroup,
                            items.MaterialSubGroup,
                            items.Material,
                            items.IssuedMonth,
                            items.IssuedYear,
                            items.IssuedQty,
                            items.TotalPrice
                        }));
                        return new EmptyResult();
                    }
                    else
                    {
                        long totalRecords = MaterialIssueList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                        var jsonData = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalRecords,
                            rows = (
                                 from items in MaterialIssueList.First().Value
                                 select new
                                 {
                                     i = items.Id,
                                     cell = new string[] { 
                                     items.Id.ToString(), 
                                     items.RequiredFromStore,
                                     items.RequiredForStore,
                                     items.RequiredForCampus,
                                     items.MaterialGroup,
                                     items.MaterialSubGroup,
                                     items.Material, 
                                     items.IssuedMonth.ToString(), 
                                     items.IssuedYear.ToString(), 
                                     items.IssuedQty.ToString(), 
                                     items.TotalPrice.ToString()
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
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }
        #endregion

        #region Store to Store Material Transfer
        public ActionResult StoreToStore(int? Id)
        {
            StoreToStore mi = new StoreToStore();
            if (Id > 0)
            {
                StoreService ss = new StoreService();
                mi = ss.GetMaterialIssueById(Convert.ToInt32(Id));
            }
            return View(mi);
        }

        [HttpPost]
        public ActionResult StoreToStore(StoreToStore mi)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    StoreService ss = new StoreService();
                    mi.CreatedDate = DateTime.Now;
                    mi.CreatedBy = userId;
                    mi.DeliveryDate = DateTime.Parse(Request["DeliveryDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    ss.CreateOrUpdateMaterialIssue(mi);
                    mi.IssueNumber = "ISS-" + mi.Id;
                    ss.CreateOrUpdateMaterialIssue(mi);
                    return RedirectToAction("StoreToStore", new { Id = mi.Id });
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }


        public ActionResult SaveMaterialIssue(StoreToStore mi)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    StoreService ss = new StoreService();
                    mi.CreatedDate = DateTime.Now;
                    mi.CreatedBy = userId;
                    mi.DeliveryDate = DateTime.Now.Date;
                    ss.CreateOrUpdateMaterialIssue(mi);
                    mi.IssueNumber = "ISS-" + mi.Id;
                    ss.CreateOrUpdateMaterialIssue(mi);
                    return Json(mi.Id, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult StoreToStoreMaterialIssueListJqGrid(int Id, int rows, string sidx, string sord, int? page = 1)
        {
            StoreService ss = new StoreService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            Dictionary<string, object> criteria1 = new Dictionary<string, object>();
            if (Id > 0)
            {
                criteria.Add("IssueId", Id);
                Dictionary<long, IList<StoreToStoreIssuedMaterials>> MaterialsList = ss.GetStoreToStoreIssuedMaterialsListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (MaterialsList != null && MaterialsList.Count > 0 && MaterialsList.FirstOrDefault().Key > 0 && MaterialsList.FirstOrDefault().Value.Count > 0)
                {
                    for (int i = 0; i < MaterialsList.First().Key; i++)
                    {
                        criteria1.Clear();
                        criteria1.Add("Material", MaterialsList.FirstOrDefault().Value[i].Material.Trim());
                        // criteria1.Add("Store", Store);
                        string[] IssuedStatus = new[] { "Not Issued", "Partially Issued" };
                        criteria1.Add("IssuedStatus", IssuedStatus);
                        Dictionary<long, IList<SkuList>> SkuList = ss.GetSkulistWithPagingAndCriteria(0, 9999, "Asc", "SkuId", criteria1);
                        if (SkuList != null && SkuList.FirstOrDefault().Value != null && SkuList.Count() > 0 && SkuList.FirstOrDefault().Key > 0)
                        {
                            for (int j = 0; j < SkuList.FirstOrDefault().Value.Count; j++)
                            {
                                MaterialsList.First().Value[i].InwardIds = MaterialsList.First().Value[i].InwardIds + Convert.ToString(SkuList.First().Value[j].MaterialRefId) + ", ";

                                MaterialsList.First().Value[i].AvailableQtys = MaterialsList.First().Value[i].AvailableQtys + Convert.ToString(SkuList.First().Value[j].StockAvailableQty) + ", ";

                                MaterialsList.First().Value[i].UnitPrices = MaterialsList.First().Value[i].UnitPrices + Convert.ToString(SkuList.First().Value[j].UnitPrice) + ", ";
                            }
                        }
                    }
                    long totalRecords = MaterialsList.First().Key;
                    int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                    var jsonData = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalRecords,
                        rows = (
                             from items in MaterialsList.First().Value
                             select new
                             {
                                 i = items.Id,
                                 cell = new string[] { 
                                     items.Id.ToString(), 
                                     items.IssueId.ToString(),
                                     items.Material,
                                     items.MaterialGroup, 
                                     items.MaterialSubGroup, 
                                     items.Units, 
                                     items.InwardIds,
                                     items.AvailableQtys,
                                     items.UnitPrices,
                                     items.IssuedQty.ToString(), 
                                     }
                             })
                    };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult StoreToStoreMaterialSearch()
        {
            try
            {
                return PartialView();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult AddMaterialIssueList(IList<StoreToStoreIssuedMaterials> IssueLst)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    StoreService ss = new StoreService();
                    if (IssueLst != null)
                        ss.CreateOrUpdateStoreToStoreIssuedMaterialsList(IssueLst);
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            { return ThrowJSONErrorNew(ex, "StoreMgmntPolicy"); }
        }

        public ActionResult StoreSKUListJqGridForMaterialIssue(string FromStore, string MaterialGroup, string MaterialSubGroup, string Material, string Units, long? MaterialGroupId, long? MaterialSubGroupId, long? MaterialId, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                StoreService ds = new StoreService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrWhiteSpace(FromStore)) { criteria.Add("Store", FromStore); }
                if (!string.IsNullOrWhiteSpace(MaterialGroup)) { criteria.Add("MaterialGroup", MaterialGroup); }
                if (!string.IsNullOrWhiteSpace(MaterialSubGroup)) { criteria.Add("MaterialSubGroup", MaterialSubGroup); }
                if (!string.IsNullOrWhiteSpace(Material)) { criteria.Add("Material", Material); }
                if (!string.IsNullOrWhiteSpace(Units)) { criteria.Add("UnitCode", Units); }
                if (MaterialGroupId > 0)
                    criteria.Add("MaterialGroupId", MaterialGroupId);
                if (MaterialSubGroupId > 0)
                    criteria.Add("MaterialSubGroupId", MaterialSubGroupId);
                if (MaterialId > 0)
                    criteria.Add("Id", MaterialId);
                criteria.Add("IsActive", true);
                sord = sord == "desc" ? "Desc" : "Asc";
                Dictionary<long, IList<MaterialsMaster_vw_Stock_vw>> MaterialsMasterList = ds.GetMaterialsMasterAndStockBalancelistWithPagingAndCriteriaUsingView(page - 1, rows, sidx, sord, criteria);
                if (MaterialsMasterList != null && MaterialsMasterList.Count > 0)
                {
                    long totalrecords = MaterialsMasterList.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat1 = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,

                        rows = (from items in MaterialsMasterList.First().Value
                                where items.MaterialGroup != null && items.MaterialSubGroup != null && items.Material != null && items.UnitCode != null
                                select new
                                {
                                    i = 2,
                                    cell = new string[] {
                               items.Id.ToString(),items.MaterialGroup,items.MaterialSubGroup,items.Material,items.UnitCode, items.Store, items.ClosingBalance.ToString()
                            }
                                })
                    };
                    return Json(jsondat1, JsonRequestBehavior.AllowGet);

                }
                else return Json(null);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult IssueMaterialsAndUpdateStock(IList<StoreToStoreIssuedMaterials> IssueLst)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    StoreService ss = new StoreService();
                    if (IssueLst != null)
                    {
                        ss.CreateOrUpdateStoreToStoreIssuedMaterialsList(IssueLst);

                        //Update SKUList 
                        foreach (var item in IssueLst)
                        {
                            var InwardIdarr = item.InwardIds.Split(',', ' ');
                            InwardIdarr = InwardIdarr.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                            long[] InwardIdlongarr = Array.ConvertAll(InwardIdarr, s => long.Parse(s));
                            Dictionary<string, object> criteria1 = new Dictionary<string, object>();
                            criteria1.Clear();
                            string[] IssuedStatus = new[] { "Not Issued", "Partially Issued" };

                            if (InwardIdlongarr != null)
                            {
                                criteria1.Add("MaterialRefId", InwardIdlongarr);
                                criteria1.Add("Material", item.Material.Trim());
                                criteria1.Add("IssuedStatus", IssuedStatus);
                                Dictionary<long, IList<SkuList>> SkuList = ss.GetSkulistWithPagingAndCriteria(0, 9999, "Asc", "SkuId", criteria1);
                                if (SkuList != null && SkuList.FirstOrDefault().Value != null && SkuList.FirstOrDefault().Value.Count() > 0 && SkuList.FirstOrDefault().Key > 0)
                                {
                                    SkuList sku = SkuList.FirstOrDefault().Value[0];
                                    if (item.IssuedQty <= sku.StockAvailableQty)
                                    {
                                        sku.IssuedQty = sku.IssuedQty + item.IssuedQty;
                                        sku.StockAvailableQty = sku.DamagelessQty - sku.IssuedQty;
                                        if (sku.StockAvailableQty == 0)
                                            sku.IssuedStatus = "Completely Issued";
                                        else
                                            sku.IssuedStatus = "Partially Issued";
                                        ss.CreateOrUpdateSku(sku);
                                    }
                                    else if (item.IssuedQty > sku.StockAvailableQty)
                                    {
                                        sku.IssuedQty = sku.IssuedQty + sku.StockAvailableQty;
                                        int TobeIssuedQty = Convert.ToInt32(item.IssuedQty - sku.StockAvailableQty);
                                        sku.StockAvailableQty = 0;
                                        sku.IssuedStatus = "Completely Issued";
                                        ss.CreateOrUpdateSku(sku);
                                        sku = SkuList.FirstOrDefault().Value[1];
                                        if (TobeIssuedQty <= sku.StockAvailableQty)
                                        {
                                            sku.IssuedQty = sku.IssuedQty + TobeIssuedQty;
                                            sku.StockAvailableQty = sku.DamagelessQty - sku.IssuedQty;
                                            if (sku.StockAvailableQty == 0)
                                                sku.IssuedStatus = "Completely Issued";
                                            else
                                                sku.IssuedStatus = "Partially Issued";
                                            ss.CreateOrUpdateSku(sku);
                                        }

                                        else if (TobeIssuedQty > sku.StockAvailableQty)
                                        {

                                        }
                                    }
                                }

                            }
                        }


                        //

                        /////
                        for (int i = 0; i < IssueLst.Count; i++)
                        {
                            MaterialsMaster mm = ss.GetMaterialsMasterByMaterial(IssueLst[i].Material);
                            StoreToStore mi = ss.GetMaterialIssueById(Convert.ToInt32(IssueLst[i].IssueId));
                            StockTransaction st = new StockTransaction();
                            st.TransactionCode = "Store To Store";
                            st.Store = mi.FromStore;
                            st.ItemId = mm.Id;
                            st.Units = IssueLst[i].Units;
                            st.TransactionDate = DateTime.Now;
                            st.TransactionBy = userId;
                            st.TransactionType = "Material Issue";
                            st.Qty = IssueLst[i].IssuedQty;
                            //st.DamagedQty = Convert.ToInt32(MaterialSkulist.First().Value[i].DamagedQty);
                            st.TransactionComments = "";
                            st.RequiredForStore = mi.ToStore;
                            st.RequiredFromStore = mi.FromStore;
                            ss.CreateOrUpdateStockTransaction(st);
                        }
                        /////

                    }
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            { return ThrowJSONErrorNew(ex, "StoreMgmntPolicy"); }
        }

        public ActionResult ShowStoreToStore(int? Id)
        {
            StoreService ss = new StoreService();
            StoreToStore mi = ss.GetMaterialIssueById(Convert.ToInt32(Id));
            return View(mi);
        }

        public ActionResult StoreToStoreList()
        {
            return View();
        }


        public ActionResult StoreToStoreListJqGrid(string Status, int rows, string sidx, string sord, int? page = 1, int? ExptXl = 0)
        {
            try
            {
                StoreService ss = new StoreService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                sord = sord == "desc" ? "Desc" : "Asc";
                if (!string.IsNullOrEmpty(Status))
                    criteria.Add("Status", Status);
                Dictionary<long, IList<StoreToStore>> StoreToStoreList = ss.GetStoreToStoreListWithPagingAndCriteria(page - 1, rows, sord, sidx, criteria);
                if (StoreToStoreList != null && StoreToStoreList.Count > 0)
                {
                    if (ExptXl == 1)
                    {
                        var List = StoreToStoreList.First().Value.ToList();
                        base.ExptToXL(List, "StoreToStoreList", (items => new
                        {

                        }));
                        return new EmptyResult();
                    }
                    else
                    {
                        long totalrecords = StoreToStoreList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat1 = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,

                            rows = (from items in StoreToStoreList.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                               items.Id.ToString(),
                               items.IssueNumber,
                               items.FromStore,
                               items.ToStore,
                               items.DeliveredThrough,
                               items.DeliveryDetails,
                               items.DeliveryDate!=null?items.DeliveryDate.Value.ToString("dd/MM/yyyy"):null,
                               items.CreatedBy,
                               items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm tt"):null,
                               items.Status,
                            }
                                    })
                        };
                        return Json(jsondat1, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                { return null; }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }


        #endregion

        public ActionResult MaterialPrice()
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
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult MaterialPriceListJqGrid(string ExptType, string MatGrp, string MatSubGrp, string Mat, MaterialsMaster_vw mm, int? rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) { return RedirectToAction("LogOff", "Account"); }
                else
                {
                    StoreService ss = new StoreService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    sord = sord == "desc" ? sord = "Desc" : sord = "Asc";
                    if (!string.IsNullOrWhiteSpace(mm.MaterialGroup)) criteria.Add("MaterialGroup", mm.MaterialGroup);
                    if (!string.IsNullOrWhiteSpace(mm.Material)) criteria.Add("Material", mm.Material);
                    else
                        if (!string.IsNullOrWhiteSpace(Mat)) criteria.Add("Material", Mat);
                    if (!string.IsNullOrWhiteSpace(mm.UnitCode)) criteria.Add("UnitCode", mm.UnitCode);
                    if (!string.IsNullOrWhiteSpace(mm.MaterialSubGroup)) criteria.Add("MaterialSubGroup", mm.MaterialSubGroup);
                    if (!string.IsNullOrWhiteSpace(MatGrp)) criteria.Add("MaterialGroupId", Convert.ToInt64(MatGrp));
                    if (!string.IsNullOrWhiteSpace(MatSubGrp)) criteria.Add("MaterialSubGroupId", Convert.ToInt64(MatSubGrp));
                    criteria.Add("IsActive", true);
                    Dictionary<long, IList<MaterialsMaster_vw>> MaterialSkulist = ss.GetMaterialsMasterlistWithPagingAndCriteriaUsingView(page - 1, rows, sord, sidx, criteria);
                    if (MaterialSkulist != null && MaterialSkulist.Count > 0 && MaterialSkulist.FirstOrDefault().Value != null && MaterialSkulist.FirstOrDefault().Value.Count > 0)
                    {
                        foreach (var item in MaterialSkulist.FirstOrDefault().Value)
                        {
                            Dictionary<string, object> criteria1 = new Dictionary<string, object>();
                            criteria1.Clear();
                            if (!string.IsNullOrWhiteSpace(item.Material))
                            {
                                criteria1.Add("Material", item.Material.Trim());
                                criteria1.Add("MaterialInward." + "Store", "IB MAIN");
                                criteria1.Add("MaterialInward." + "Status", "Completed");
                                // string[] IssuedStatus = new[] { "Not Issued", "Partially Issued" };
                                // criteria1.Add("IssuedStatus", IssuedStatus);
                                string[] alias = new string[1];
                                alias[0] = "MaterialInward";
                                Dictionary<long, IList<SkuList_MaterialInward>> SkuList = ss.GetSkuList_MaterialInwardListWithPagingAndCriteria(0, 9999, "SkuId", "Asc", criteria1, "", null, alias);
                                if (SkuList != null && SkuList.FirstOrDefault().Value != null && SkuList.Count() > 0 && SkuList.FirstOrDefault().Key > 0)
                                {
                                    int incre = 0;
                                    for (int j = (SkuList.FirstOrDefault().Value.Count - 1); j >= 0; j--)
                                    {
                                        item.OldPrices = item.OldPrices + Convert.ToString(SkuList.First().Value[j].UnitPrice);
                                        item.Discounts = item.Discounts + Convert.ToString(SkuList.First().Value[j].Discount);
                                        item.Taxes = item.Taxes + Convert.ToString(SkuList.First().Value[j].Tax);
                                        item.CalculatedPrices = item.CalculatedPrices + ((decimal)(SkuList.First().Value[j].UnitPrice + (SkuList.First().Value[j].Tax / 100 * SkuList.First().Value[j].UnitPrice) - (SkuList.First().Value[j].Discount / 100 * SkuList.First().Value[j].UnitPrice))).ToString("0.00");
                                        incre++;
                                        if (incre == 4)
                                            break;
                                        else
                                        {
                                            item.OldPrices = item.OldPrices + ", ";
                                            item.Discounts = item.Discounts + ", ";
                                            item.Taxes = item.Taxes + ", ";
                                            item.CalculatedPrices = item.CalculatedPrices + " ,";
                                        }
                                    }
                                }
                            }
                        }

                        if (ExptType == "Excel")
                        {
                            var List = MaterialSkulist.First().Value.ToList();
                            base.ExptToXL(List, "MaterialPriceList", (items => new
                            {
                                items.Id,
                                items.MaterialGroup,
                                items.MaterialSubGroup,
                                items.Material,
                                items.UnitCode,
                                MRP = items.OldPrices,
                                items.Discounts,
                                items.Taxes,
                                items.CalculatedPrices
                            }));
                            return new EmptyResult();
                        }
                        else
                        {
                            long totalrecords = MaterialSkulist.First().Key;
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var jsondat1 = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalrecords,

                                rows = (from items in MaterialSkulist.First().Value
                                        select new
                                        {
                                            i = 2,
                                            cell = new string[] {
                               items.Id.ToString(),
                               items.MaterialGroup,
                               items.MaterialSubGroup,
                               items.Material,
                               items.UnitCode,
                               items.OldPrices,
                               items.Discounts,
                               items.Taxes,
                               items.CalculatedPrices
                            }
                                        })
                            };
                            return Json(jsondat1, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    { return Json(null, JsonRequestBehavior.AllowGet); }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        #region "Material Return"

        public ActionResult MaterialReturnList()
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
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }
        public ActionResult MaterialReturn(int? MatRetId)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) { return RedirectToAction("LogOff", "Account"); }
                else
                {
                    StoreService ss = new StoreService();
                    MaterialReturn mr = new MaterialReturn();
                    if (MatRetId > 0)
                    {
                        mr = ss.GetMaterialReturnById(Convert.ToInt32(MatRetId));
                    }
                    else
                    {
                        User user = (User)Session["objUser"];
                        //  if (user != null && user.Campus != null)
                        IList<StoreMaster> StoreMaster = ss.GetStoreByCampus(user.Campus);
                        mr.CreatedBy = userId;
                        mr.FromStore = StoreMaster.FirstOrDefault().Store;
                        mr.ToStore = StoreMaster.FirstOrDefault().MainStore;
                        mr.ReturnStatus = "Open";
                        mr.Campus = user.Campus;
                    }
                    return View(mr);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult MaterialReturnJqGrid(string ExptType, MaterialReturn mr, int? rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) { return RedirectToAction("LogOff", "Account"); }
                else
                {
                    StoreService ss = new StoreService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    sord = sord == "desc" ? sord = "Desc" : sord = "Asc";

                    //criteria.Add("MatRetId", mrl.MatRetId);
                    //if (!string.IsNullOrWhiteSpace(mrl.MaterialGroup)) criteria.Add("MaterialGroup", mrl.MaterialGroup);
                    //if (!string.IsNullOrWhiteSpace(mrl.MaterialSubGroup)) criteria.Add("MaterialSubGroup", mrl.MaterialSubGroup);
                    //if (!string.IsNullOrWhiteSpace(mrl.Material)) criteria.Add("Material", mrl.Material);
                    //if (!string.IsNullOrWhiteSpace(mrl.Units)) criteria.Add("UnitCode", mrl.Units);
                    //if (!string.IsNullOrWhiteSpace(mr.AcceptedStatus)) criteria.Add("AcceptedStatus", mr.AcceptedStatus);
                    criteria.Add("ReturnStatus", mr.ReturnStatus);
                    criteria.Add("CreatedBy", userId);
                    Dictionary<long, IList<MaterialReturn>> MaterialReturnList = ss.GetMaterialReturnWithPagingAndCriteria(page - 1, rows, sord, sidx, criteria);

                    if (ExptType == "Excel")
                    {
                        var List = MaterialReturnList.First().Value.ToList();
                        base.ExptToXL(List, "MaterialReturn", (items => new
                        {
                            items.MatRetId,
                            items.ReturnRefNum,
                            items.FromStore,
                            items.ToStore,
                            items.DCNumber,
                            items.CreatedBy,
                            items.CreatedDate,
                            items.ReturnStatus,
                            items.AcceptedStatus
                        }));
                        return new EmptyResult();
                    }
                    else
                    {
                        long totalrecords = MaterialReturnList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat1 = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in MaterialReturnList.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                                items.MatRetId.ToString(),
                                items.ReturnRefNum,
                                items.FromStore,
                                items.ToStore,
                                items.DCNumber,
                                items.CreatedBy,
                                items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss TT"):null,
                                items.ReturnStatus,
                                items.AcceptedStatus
                            }
                                    })
                        };
                        return Json(jsondat1, JsonRequestBehavior.AllowGet);
                    }

                }

            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult MaterialReturn(MaterialReturn mr)
        {
            StoreService ss = new StoreService();
            ss.CreateOrUpdateMaterialReturn(mr);
            return View();
        }

        public ActionResult SaveMaterialReturn(MaterialReturn mr)
        {
            StoreService ss = new StoreService();

            mr.CreatedDate = DateTime.Now;
            ss.CreateOrUpdateMaterialReturn(mr);
            mr.ReturnRefNum = "RET-" + mr.MatRetId;
            ss.CreateOrUpdateMaterialReturn(mr);
            return Json(mr);
        }

        public ActionResult MaterialReturnMaterialSearch(string Campus)
        {
            try
            {
                ViewBag.Campus = Campus;
                return View();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult SaveMaterialReturnList(IList<MaterialReturnList> mrl)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    StoreService ss = new StoreService();
                    if (mrl != null)
                        ss.CreateOrUpdateMaterialReturnList(mrl);
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            { return ThrowJSONErrorNew(ex, "StoreMgmntPolicy"); }
        }

        public ActionResult MaterialReturnListJqGrid(string ExptType, MaterialReturnList mrl, int? rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) { return RedirectToAction("LogOff", "Account"); }
                else
                {
                    StoreService ss = new StoreService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    sord = sord == "desc" ? sord = "Desc" : sord = "Asc";

                    if (mrl.MatRetId != 0)
                    {
                        criteria.Add("MatRetId", mrl.MatRetId);
                        if (!string.IsNullOrWhiteSpace(mrl.MaterialGroup)) criteria.Add("MaterialGroup", mrl.MaterialGroup);
                        if (!string.IsNullOrWhiteSpace(mrl.MaterialSubGroup)) criteria.Add("MaterialSubGroup", mrl.MaterialSubGroup);
                        if (!string.IsNullOrWhiteSpace(mrl.Material)) criteria.Add("Material", mrl.Material);
                        if (!string.IsNullOrWhiteSpace(mrl.Units)) criteria.Add("UnitCode", mrl.Units);
                        Dictionary<long, IList<MaterialReturnList>> MaterialReturnList = ss.GetMaterialReturnListWithPagingAndCriteria(page - 1, rows, sord, sidx, criteria);

                        if (ExptType == "Excel")
                        {
                            var List = MaterialReturnList.First().Value.ToList();
                            base.ExptToXL(List, "MaterialReturnList", (items => new
                            {
                                items.Id,
                                items.MaterialGroup,
                                items.MaterialSubGroup,
                                items.Material,
                                items.Units,
                                items.ReturnQty
                            }));
                            return new EmptyResult();
                        }
                        else
                        {
                            long totalrecords = MaterialReturnList.First().Key;
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var jsondat1 = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalrecords,

                                rows = (from items in MaterialReturnList.First().Value
                                        select new
                                        {
                                            i = 2,
                                            cell = new string[] {
                               items.Id.ToString(),
                               items.MaterialGroup,
                               items.MaterialSubGroup,
                               items.Material,
                               items.Units,
                               items.ReturnQty.ToString()
                            }
                                        })
                            };
                            return Json(jsondat1, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                        return Json(null, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult CompleteMaterialReturn(MaterialReturn mr)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    StoreService ss = new StoreService();
                    mr = ss.GetMaterialReturnById(mr.MatRetId);
                    mr.ReturnStatus = "Completed";
                    mr.AcceptedStatus = "Open";
                    ss.CreateOrUpdateMaterialReturn(mr);

                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("MatRetId", mr.MatRetId);
                    Dictionary<long, IList<MaterialReturnList>> MaterialReturnList = ss.GetMaterialReturnListWithPagingAndCriteria(0, 9999, "Desc", "Id", criteria);

                    if (MaterialReturnList != null && MaterialReturnList.Count > 0 && MaterialReturnList.First().Key > 0)
                    {
                        for (int i = 0; i < MaterialReturnList.First().Key; i++)
                        {
                            MaterialReturnList mrl = MaterialReturnList.First().Value[i];
                            MaterialsMaster mm = ss.GetMaterialsMasterByMaterial(mrl.Material);

                            ////

                            Dictionary<string, object> criteria1 = new Dictionary<string, object>();
                            criteria1.Add("Material", mm.Material.Trim());
                            criteria1.Add("MaterialInward." + "Store", mr.FromStore);
                            string[] IssuedStatus = new[] { "Not Issued", "Partially Issued" };
                            criteria1.Add("IssuedStatus", IssuedStatus);
                            string[] alias = new string[1];
                            alias[0] = "MaterialInward";
                            Dictionary<long, IList<SkuList_MaterialInward>> SkuMIList = ss.GetSkuList_MaterialInwardListWithPagingAndCriteria(0, 9999, "SkuId", "Asc", criteria1, "", null, alias);
                            if (SkuMIList != null && SkuMIList.FirstOrDefault().Value != null && SkuMIList.FirstOrDefault().Value.Count() > 0 && SkuMIList.FirstOrDefault().Key > 0)
                            {
                                long[] inwardarr = (from items in SkuMIList.First().Value select items.MaterialRefId).Distinct().ToArray();
                                Dictionary<string, object> skuCriteria = new Dictionary<string, object>();
                                skuCriteria.Add("MaterialRefId", inwardarr);
                                Dictionary<long, IList<SkuList>> SkuList = ss.GetSkulistWithPagingAndCriteria(0, 9999, "Asc", "SkuId", skuCriteria);
                                SkuList sku = SkuList.FirstOrDefault().Value[0];

                                if (mrl.ReturnQty <= sku.StockAvailableQty)
                                {
                                    sku.IssuedQty = sku.IssuedQty + mrl.ReturnQty;
                                    sku.StockAvailableQty = sku.DamagelessQty - sku.IssuedQty;
                                    if (sku.StockAvailableQty == 0)
                                        sku.IssuedStatus = "Completely Issued";
                                    else
                                        sku.IssuedStatus = "Partially Issued";
                                    ss.CreateOrUpdateSku(sku);
                                }
                                else if (mrl.ReturnQty > sku.StockAvailableQty)
                                {
                                    sku.IssuedQty = sku.IssuedQty + sku.StockAvailableQty;
                                    int TobeIssuedQty = Convert.ToInt32(mrl.ReturnQty - sku.StockAvailableQty);
                                    sku.StockAvailableQty = 0;
                                    sku.IssuedStatus = "Completely Issued";
                                    ss.CreateOrUpdateSku(sku);
                                    sku = SkuList.FirstOrDefault().Value[1];
                                    if (TobeIssuedQty <= sku.StockAvailableQty)
                                    {
                                        sku.IssuedQty = sku.IssuedQty + TobeIssuedQty;
                                        sku.StockAvailableQty = sku.DamagelessQty - sku.IssuedQty;
                                        if (sku.StockAvailableQty == 0)
                                            sku.IssuedStatus = "Completely Issued";
                                        else
                                            sku.IssuedStatus = "Partially Issued";
                                        ss.CreateOrUpdateSku(sku);
                                    }

                                    else if (TobeIssuedQty > sku.StockAvailableQty)
                                    {

                                    }
                                }
                            }
                            ////

                            if (mm != null)
                            {
                                StockTransaction st = new StockTransaction();
                                st.TransactionCode = mr.ReturnRefNum;
                                st.Store = mr.FromStore;
                                st.ItemId = mm.Id;
                                st.Units = MaterialReturnList.First().Value[i].Units;
                                st.TransactionDate = DateTime.Now;
                                st.TransactionBy = mr.CreatedBy;
                                st.TransactionType = "Material Issue";
                                st.Qty = MaterialReturnList.First().Value[i].ReturnQty;
                                st.DamagedQty = 0;
                                st.TransactionComments = mr.Description;

                                ss.CreateOrUpdateStockTransaction(st);
                            }
                        }
                        return Json(mr.ReturnRefNum, JsonRequestBehavior.AllowGet);
                    }
                    else
                    { return Json(null); }
                }
            }
            catch (Exception ex)
            { return ThrowJSONErrorNew(ex, "StoreMgmntPolicy"); }
        }

        public ActionResult ReturnedMaterialsList()
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
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }


        public ActionResult ShowMaterialReturnList(int MatRetId)
        {
            ViewBag.MatRetId = MatRetId;
            return View();
        }

        public ActionResult ReturnedMaterialJqGrid(string ExptType, MaterialReturn mr, int? rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) { return RedirectToAction("LogOff", "Account"); }
                else
                {
                    StoreService ss = new StoreService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    sord = sord == "desc" ? sord = "Desc" : sord = "Asc";

                    //criteria.Add("MatRetId", mrl.MatRetId);
                    //if (!string.IsNullOrWhiteSpace(mrl.MaterialGroup)) criteria.Add("MaterialGroup", mrl.MaterialGroup);
                    //if (!string.IsNullOrWhiteSpace(mrl.MaterialSubGroup)) criteria.Add("MaterialSubGroup", mrl.MaterialSubGroup);
                    //if (!string.IsNullOrWhiteSpace(mrl.Material)) criteria.Add("Material", mrl.Material);
                    //if (!string.IsNullOrWhiteSpace(mrl.Units)) criteria.Add("UnitCode", mrl.Units);
                    if (!string.IsNullOrWhiteSpace(mr.AcceptedStatus)) criteria.Add("AcceptedStatus", mr.AcceptedStatus);
                    Dictionary<long, IList<MaterialReturn>> MaterialReturnList = ss.GetMaterialReturnWithPagingAndCriteria(page - 1, rows, sord, sidx, criteria);

                    if (ExptType == "Excel")
                    {
                        var List = MaterialReturnList.First().Value.ToList();
                        base.ExptToXL(List, "MaterialReturn", (items => new
                        {
                            items.MatRetId,
                            items.ReturnRefNum,
                            items.FromStore,
                            items.ToStore,
                            items.DCNumber,
                            items.CreatedBy,
                            items.CreatedDate,
                            items.ReturnStatus,
                            items.AcceptedStatus
                        }));
                        return new EmptyResult();
                    }
                    else
                    {
                        long totalrecords = MaterialReturnList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat1 = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in MaterialReturnList.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                                items.MatRetId.ToString(),
                                items.ReturnRefNum,
                                items.FromStore,
                                items.ToStore,
                                items.DCNumber,
                                items.CreatedBy,
                                items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/YYYY hh:mm:ss TT"):null,
                                items.ReturnStatus,
                                items.AcceptedStatus
                            }
                                    })
                        };
                        return Json(jsondat1, JsonRequestBehavior.AllowGet);
                    }

                }

            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult AcceptReturnedMaterials(int? MatRetId)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) { return RedirectToAction("LogOff", "Account"); }
                else
                {
                    StoreService ss = new StoreService();
                    MaterialReturn mr = new MaterialReturn();
                    if (MatRetId > 0)
                    {
                        mr = ss.GetMaterialReturnById(Convert.ToInt32(MatRetId));
                        return View(mr);
                    }
                    else
                        return View();

                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult AcceptReturnedMaterialsAndUpdateStock(MaterialReturn mr)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    User user = (User)Session["objUser"];
                    StoreService ss = new StoreService();
                    mr = ss.GetMaterialReturnById(mr.MatRetId);
                    mr.AcceptedStatus = "Completed";
                    ss.CreateOrUpdateMaterialReturn(mr);
                    MaterialInward mi = new MaterialInward();
                    mi.ProcessedBy = userId;
                    //mi.UserRole = mr.UserRole;
                    mi.Status = "Completed";
                    mi.Campus = user.Campus;
                    mi.Store = mr.ToStore;
                    mi.CreatedDate = DateTime.Now;
                    mi.Supplier = mr.FromStore;
                    mi.ReceivedBy = userId;
                    mi.ReceivedDateTime = DateTime.Now;
                    mi.InvoiceDate = DateTime.Now;
                    mi.DCDate = DateTime.Now;
                    mi.SuppRefNo = mr.DCNumber;
                    mi.DCNumber = mr.DCNumber;
                    ss.CreateOrUpdateMaterialInward(mi);
                    mi.InwardNumber = "MIN-" + mi.Id;
                    ss.CreateOrUpdateMaterialInward(mi);
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("MatRetId", mr.MatRetId);
                    Dictionary<long, IList<MaterialReturnList>> MaterialReturnList = ss.GetMaterialReturnListWithPagingAndCriteria(0, 9999, "Desc", "Id", criteria);

                    if (MaterialReturnList != null && MaterialReturnList.Count > 0 && MaterialReturnList.First().Key > 0)
                    {
                        foreach (var item in MaterialReturnList.FirstOrDefault().Value)
                        {
                            MaterialsMaster mm = ss.GetMaterialsMasterByMaterial(item.Material);

                            SkuList sku = new SkuList();
                            sku.MaterialRefId = mi.Id;
                            sku.MaterialGroup = item.MaterialGroup;
                            sku.MaterialSubGroup = item.MaterialSubGroup;
                            sku.Material = item.Material;
                            sku.OrderedUnits = item.Units;
                            sku.OrderQty = item.ReturnQty;
                            sku.ReceivedQty = item.ReturnQty;
                            sku.DamagedQty = 0;
                            sku.DamagelessQty = item.ReturnQty;
                            sku.IssuedQty = 0;
                            sku.StockAvailableQty = item.ReturnQty;
                            sku.IssuedStatus = "Not Issued";
                            sku.DamageDescription = string.Empty;
                            sku.ReceivedUnits = item.Units;

                            Dictionary<string, object> criteria1 = new Dictionary<string, object>();
                            criteria1.Add("Material", sku.Material.Trim());
                            criteria1.Add("MaterialInward." + "Store", mr.FromStore);
                            string[] alias = new string[1];
                            alias[0] = "MaterialInward";
                            Dictionary<long, IList<SkuList_MaterialInward>> SkuList = ss.GetSkuList_MaterialInwardListWithPagingAndCriteria(0, 9999, "SkuId", "Desc", criteria1, "", null, alias);
                            if (SkuList != null && SkuList.FirstOrDefault().Value != null && SkuList.Count() > 0 && SkuList.FirstOrDefault().Key > 0)
                            {
                                sku.UnitPrice = SkuList.FirstOrDefault().Value[0].UnitPrice;
                                sku.TotalPrice = sku.ReceivedQty * sku.UnitPrice;
                            }
                            ss.CreateOrUpdateSku(sku);

                            if (mm != null)
                            {
                                StockTransaction st = new StockTransaction();
                                st.TransactionCode = mr.ReturnRefNum;
                                st.Store = mr.ToStore;
                                st.ItemId = mm.Id;
                                st.Units = item.Units;
                                st.TransactionDate = DateTime.Now;
                                st.TransactionBy = userId;
                                st.TransactionType = "Material Inward";
                                st.Qty = item.ReturnQty;
                                st.DamagedQty = 0;
                                st.TransactionComments = mr.Description;

                                ss.CreateOrUpdateStockTransaction(st);
                            }
                        }
                        return Json(mr.ReturnRefNum, JsonRequestBehavior.AllowGet);
                    }
                    else
                    { return Json(null); }
                }
            }
            catch (Exception ex)
            { return ThrowJSONErrorNew(ex, "StoreMgmntPolicy"); }
        }

        public ActionResult StoreSKUListJqGridForMaterialReturn(string MaterialGroup, string MaterialSubGroup, string Material, string Units, string Store, long? MaterialGroupId, long? MaterialSubGroupId, long? MaterialId, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                StoreService ss = new StoreService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrWhiteSpace(MaterialGroup)) { criteria.Add("MaterialGroup", MaterialGroup); }
                if (!string.IsNullOrWhiteSpace(MaterialSubGroup)) { criteria.Add("MaterialSubGroup", MaterialSubGroup); }
                if (!string.IsNullOrWhiteSpace(Material)) { criteria.Add("Material", Material); }
                if (!string.IsNullOrWhiteSpace(Units)) { criteria.Add("UnitCode", Units); }
                if (!string.IsNullOrWhiteSpace(Store)) { criteria.Add("Store", Store); }
                if (MaterialGroupId > 0)
                    criteria.Add("MaterialGroupId", MaterialGroupId);
                if (MaterialSubGroupId > 0)
                    criteria.Add("MaterialSubGroupId", MaterialSubGroupId);
                if (MaterialId > 0)
                    criteria.Add("Id", MaterialId);
                criteria.Add("IsActive", true);
                sord = sord == "desc" ? "Desc" : "Asc";
                Dictionary<long, IList<MaterialsMaster_vw_Stock_vw>> MaterialsMasterList = ss.GetMaterialsMasterAndStockBalancelistWithPagingAndCriteriaUsingView(page - 1, rows, sidx, sord, criteria);
                if (MaterialsMasterList != null && MaterialsMasterList.Count > 0)
                {
                    for (int i = 0; i < MaterialsMasterList.FirstOrDefault().Value.Count; i++)
                    {
                        MaterialsMaster mm = ss.GetMaterialsMasterByMaterial(MaterialsMasterList.First().Value[i].Material.Trim());
                        Dictionary<string, object> criteria1 = new Dictionary<string, object>();
                        criteria1.Add("Material", mm.Material.Trim());
                        criteria1.Add("MaterialInward." + "Store", Store);
                        criteria1.Add("MaterialInward." + "Status", "Completed");
                        string[] IssuedStatus = new[] { "Not Issued", "Partially Issued" };
                        criteria1.Add("IssuedStatus", IssuedStatus);
                        string[] alias = new string[1];
                        alias[0] = "MaterialInward";
                        Dictionary<long, IList<SkuList_MaterialInward>> SkuList = ss.GetSkuList_MaterialInwardListWithPagingAndCriteria(0, 9999, "SkuId", "Asc", criteria1, "", null, alias);
                        if (SkuList != null && SkuList.FirstOrDefault().Value != null && SkuList.Count() > 0 && SkuList.FirstOrDefault().Key > 0)
                        {
                            for (int j = 0; j < SkuList.FirstOrDefault().Value.Count; j++)
                            {
                                MaterialsMasterList.First().Value[i].InwardIds = MaterialsMasterList.First().Value[i].InwardIds + Convert.ToString(SkuList.First().Value[j].MaterialRefId) + ", ";
                                MaterialsMasterList.First().Value[i].AvailableQtys = MaterialsMasterList.First().Value[i].AvailableQtys + Convert.ToString(SkuList.First().Value[j].StockAvailableQty) + ", ";
                            }
                        }
                    }
                    long totalrecords = MaterialsMasterList.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat1 = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,

                        rows = (from items in MaterialsMasterList.First().Value
                                where items.MaterialGroup != null && items.MaterialSubGroup != null && items.Material != null && items.UnitCode != null
                                select new
                                {
                                    cell = new string[] {
                               items.Id.ToString(),items.MaterialGroup,items.MaterialSubGroup,items.Material,items.UnitCode,items.Store,items.ClosingBalance.ToString(),items.InwardIds,items.AvailableQtys
                            }
                                })
                    };
                    return Json(jsondat1, JsonRequestBehavior.AllowGet);

                }
                else return Json(null);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }


        #endregion "Material Return"

        public JsonResult MoveBackToAvailable(long ActivityId)
        {
            ProcessFlowServices pfs = new ProcessFlowServices();
            try
            {
                bool backToAvailable = false;
                if (ActivityId > 0)
                {
                    backToAvailable = pfs.MoveBackToAvailable(ActivityId);
                    return Json(backToAvailable, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }

        }

        public JsonResult FillMainStoreBySubStore(string SubStore)
        {
            try
            {
                StoreService ss = new StoreService();
                IList<StoreMaster> storelist = ss.GetStoreByCampus(SubStore);
                var mainStre = (from items in storelist
                                select new
                                {
                                    Text = items.MainStore,
                                    Value = items.MainStore
                                }).Distinct().ToList();
                return Json(mainStre, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult MaterialsInOutJqGrid(string Store, string Material, string MaterialGroup, string MaterialSubGroup, string StoreCampus, int? AMonth, int? AYear, int? AStore, int? OpeningBalance, int? Inward, int? Outward, int? ClosingBalance, int rows, string sidx, string sord, int? page = 1, int? ExptXl = 0)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                StoreService ss = new StoreService();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (!string.IsNullOrEmpty(StoreCampus))
                    criteria.Add("Store", StoreCampus);
                if (!string.IsNullOrEmpty(Store))
                    criteria.Add("Store", Store.Trim());
                if (!string.IsNullOrEmpty(Material))
                    criteria.Add("Material", Material.Trim());
                if (!string.IsNullOrEmpty(MaterialGroup))
                    criteria.Add("MaterialGroup", MaterialGroup.Trim());
                if (!string.IsNullOrEmpty(MaterialSubGroup))
                    criteria.Add("MaterialSubGroup", MaterialSubGroup.Trim());
                if (AMonth >= 0)
                    criteria.Add("AMonth", AMonth);
                if (AStore >= 0)
                    criteria.Add("AYear", AStore);
                if (AYear >= 0)
                    criteria.Add("AYear", AYear);
                if (OpeningBalance >= 0)
                    criteria.Add("OpeningBalance", OpeningBalance);
                if (Inward >= 0)
                    criteria.Add("Inward", Inward);
                if (Outward >= 0)
                    criteria.Add("Outward", Outward);
                if (ClosingBalance >= 0)
                    criteria.Add("ClosingBalance", ClosingBalance);
                Dictionary<long, IList<MaterialInwardOutwardView>> MaterialIOList = ss.GetMaterialIOListWithPagingAndCriteria_vw(page - 1, rows, sidx, sord, criteria);
                if (MaterialIOList != null && MaterialIOList.Count > 0 && MaterialIOList.FirstOrDefault().Key > 0 && MaterialIOList.FirstOrDefault().Value.Count > 0)
                {
                    if (ExptXl == 1)
                    {
                        var List = MaterialIOList.First().Value.ToList();
                        base.ExptToXL(List, "MaterialInwardOutwardList", (items => new
                        {
                            items.Id,
                            items.Store,
                            items.MaterialGroup,
                            items.MaterialSubGroup,
                            items.Material,
                            items.AMonth,
                            items.AYear,
                            items.OpeningBalance,
                            items.Inward,
                            items.Outward,
                            items.ClosingBalance
                        }));
                        return new EmptyResult();
                    }
                    else
                    {
                        long totalRecords = MaterialIOList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                        var jsonData = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalRecords,
                            rows = (
                                 from items in MaterialIOList.First().Value
                                 select new
                                 {
                                     i = items.Id,
                                     cell = new string[] { 
                                     items.Id.ToString(), 
                                     items.Store,
                                     items.MaterialGroup,
                                     items.MaterialSubGroup,
                                     items.Material, 
                                     items.AMonth.ToString(), 
                                     items.AYear.ToString(), 
                                     items.OpeningBalance.ToString(), 
                                     items.Inward.ToString(), 
                                     items.Outward.ToString(), 
                                     items.ClosingBalance.ToString() }
                                 })
                        };
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var AssLst = new { rows = (new { cell = new string[] { } }) };
                    return Json(AssLst, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult MaterialInwardMonthlyReportJqGrid(string Store, string Campus, string Material, string MaterialGroup, string MaterialSubGroup, string StoreCampus, int? Month, int? Year, int? ReceivedQty, int? DamagedQty, decimal? TotalPrice, int? rows, string sidx, string sord, int? page = 1, int? ExptXl = 0)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                StoreService ss = new StoreService();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (!string.IsNullOrEmpty(Material))
                    criteria.Add("Material", Material.Trim());
                if (!string.IsNullOrEmpty(MaterialGroup))
                    criteria.Add("MaterialGroup", MaterialGroup.Trim());
                if (!string.IsNullOrEmpty(MaterialSubGroup))
                    criteria.Add("MaterialSubGroup", MaterialSubGroup.Trim());
                if (Month >= 0)
                    criteria.Add("Month", Month);
                if (Year >= 0)
                    criteria.Add("Year", Year);
                if (ReceivedQty >= 0)
                    criteria.Add("ReceivedQty", ReceivedQty);
                if (DamagedQty >= 0)
                    criteria.Add("DamagedQty", DamagedQty);
                if (TotalPrice >= 0)
                    criteria.Add("TotalPrice", TotalPrice);
                if (!string.IsNullOrEmpty(Store))
                    criteria.Add("Store", Store);
                if (!string.IsNullOrEmpty(StoreCampus))
                    criteria.Add("Campus", StoreCampus);
                if (!string.IsNullOrEmpty(Campus))
                    criteria.Add("Campus", Campus);


                Dictionary<long, IList<MaterialInwardReport_Vw>> MaterialIOList = ss.GetMaterialInwardReport_VwListWithPagingAndCriteria_vw(page - 1, rows, sidx, sord, criteria);
                if (MaterialIOList != null && MaterialIOList.Count > 0 && MaterialIOList.FirstOrDefault().Key > 0 && MaterialIOList.FirstOrDefault().Value.Count > 0)
                {
                    if (ExptXl == 1)
                    {
                        var List = MaterialIOList.First().Value.ToList();
                        base.ExptToXL(List, "MaterialInwardMonthlyReport", (items => new
                        {
                            items.Id,
                            items.Campus,
                            items.MaterialGroup,
                            items.MaterialSubGroup,
                            items.Material,
                            items.Store,
                            items.ReceivedQty,
                            items.DamagedQty,
                            items.TotalPrice,
                            items.Month,
                            items.Year,

                        }));
                        return new EmptyResult();
                    }
                    else
                    {
                        long totalRecords = MaterialIOList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                        var jsonData = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalRecords,
                            rows = (
                                 from items in MaterialIOList.First().Value
                                 select new
                                 {
                                     i = items.Id,
                                     cell = new string[] { 
                                     items.Id.ToString(), 
                                    items.Campus,
                                    items.MaterialGroup,
                                    items.MaterialSubGroup,
                                    items.Material,
                                    items.Store,
                                    items.ReceivedQty.ToString(),
                                    items.DamagedQty.ToString(),
                                    items.TotalPrice.ToString(),
                                    items.Month.ToString(),
                                    items.Year.ToString()
                                     }
                                 })
                        };
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var AssLst = new { rows = (new { cell = new string[] { } }) };
                    return Json(AssLst, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }

        public void UpdateInbox(string Campus, string issue, string userId, long ReqNum)
        {
            InboxService IBS = new InboxService();
            Inbox In = new Inbox();
            In.Campus = Campus;
            In.UserId = userId;
            In.InformationFor = issue;
            In.CreatedDate = DateTime.Now;
            In.Module = "Store";
            In.Status = "Inbox";
            In.Campus = Campus;
            In.RefNumber = ReqNum;
            In.PreRegNum = ReqNum;
            IBS.CreateOrUpdateInbox(In);
        }
        #region  Material Distribution By vinoth

        public ActionResult MaterialsDistribution_vwListJqGrid(MaterialDistribution_Vw md, string IsHosteller, int? rows, string sidx, string sord, int? page = 1, int? ExptXl = 0)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                StoreService ss = new StoreService();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (!string.IsNullOrEmpty(md.AcademicYear))
                    criteria.Add("AcademicYear", md.AcademicYear);
                if (!string.IsNullOrEmpty(md.Campus))
                    criteria.Add("Campus", md.Campus);
                if (!string.IsNullOrEmpty(md.Grade))
                    criteria.Add("Grade", md.Grade);
                if (!string.IsNullOrEmpty(md.Section))
                    criteria.Add("Section", md.Section);

                if (!string.IsNullOrEmpty(md.MaterialSubGroup))
                    criteria.Add("MaterialSubGroup", md.MaterialSubGroup);
                //if (!string.IsNullOrEmpty(md.Gender))
                //    criteria.Add("Gender", md.Gender);
                //if (!string.IsNullOrEmpty(md.IsHosteller))
                //    criteria.Add("IsHosteller", md.IsHosteller);
                string[] arraygender = { "Male", "Female", "All" };
                if (!string.IsNullOrEmpty(md.Gender))
                {
                    if (md.Gender == "All")
                    {
                        criteria.Add("Gender", arraygender);
                    }
                    else
                    {
                        criteria.Add("Gender", md.Gender);
                    }
                }
                string[] arrayHosteller = { "True", "False", "All" };
                if (!string.IsNullOrEmpty(IsHosteller))
                {
                    if (IsHosteller == "All")
                    {
                        criteria.Add("IsHosteller", arrayHosteller);
                    }
                    else
                    {
                        if (IsHosteller == "True")
                        {
                            criteria.Add("IsHosteller", IsHosteller);
                        }
                        else
                        {
                            criteria.Add("IsHosteller", IsHosteller);
                        }
                    }
                }

                if (md.Quantity > 0)
                    criteria.Add("Quantity", md.Quantity);

                if (md.MaterialSubGroupId > 0)
                    criteria.Add("MaterialSubGroupId", md.MaterialSubGroupId);

                Dictionary<long, IList<MaterialDistribution_Vw>> MaterialDistributionList = ss.GetMaterialDistributionListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (MaterialDistributionList != null && MaterialDistributionList.Count > 0 && MaterialDistributionList.FirstOrDefault().Key > 0 && MaterialDistributionList.FirstOrDefault().Value.Count > 0)
                {
                    if (ExptXl == 1)
                    {
                        var List = MaterialDistributionList.First().Value.ToList();
                        base.ExptToXL(List, "MaterialDistributionList", (items => new
                        {
                            items.Id,
                            items.AcademicYear,
                            items.Campus,
                            items.Grade,
                            items.Section,
                            items.Gender,
                            items.IsHosteller,
                            items.MaterialSubGroupId,
                            items.MaterialSubGroup,

                            items.Quantity,
                            items.CreatedBy,
                            items.CreatedDate,
                        }));
                        return new EmptyResult();
                    }
                    else
                    {
                        long totalRecords = MaterialDistributionList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                        var jsonData = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalRecords,
                            rows = (
                                 from items in MaterialDistributionList.First().Value
                                 select new
                                 {
                                     cell = new string[] { 
                                      items.Id.ToString(),
                            items.AcademicYear,
                            items.Campus,
                            items.Grade,
                            items.Section,
                            items.Gender,                          
                            items.IsHosteller,
                            items.MaterialSubGroupId.ToString(),
                            items.MaterialSubGroup,
                            
                            items.Quantity.ToString(),
                            items.CreatedBy,
                            items.CreatedDate.ToString("dd'/'MM'/'yyyy")
                                     }
                                 })
                        };
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var AssLst = new { rows = (new { cell = new string[] { } }) };
                    return Json(AssLst, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }




        public ActionResult MaterialDistributionConfig()
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
                return View();
            }
        }

        public ActionResult SaveMaterialDistributionConfig(MaterialDistribution md)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    StoreService ss = new StoreService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    if (!string.IsNullOrEmpty(md.AcademicYear))
                        criteria.Add("AcademicYear", md.AcademicYear);
                    if (!string.IsNullOrEmpty(md.Campus))
                        criteria.Add("Campus", md.Campus);
                    if (!string.IsNullOrEmpty(md.Grade))
                        criteria.Add("Grade", md.Grade);

                    if (!string.IsNullOrEmpty(md.Gender))
                        criteria.Add("Gender", md.Gender);
                    if (!string.IsNullOrEmpty(md.IsHosteller))
                        criteria.Add("IsHosteller", md.IsHosteller);
                    if (!string.IsNullOrEmpty(md.MaterialSubGroup))
                        criteria.Add("MaterialSubGroup", md.MaterialSubGroup);
                    MaterialDistribution MaterialDistributionObj = ss.GetMaterialDistributionOnMaterial(md.AcademicYear, md.Campus, md.Grade, md.Gender, md.IsHosteller, md.MaterialSubGroup);
                    if (MaterialDistributionObj != null)
                    {
                        return Json("Failed", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        if (md.MaterialDistributionId == 0)
                        {

                            md.CreatedDate = DateTime.Now;
                            md.CreatedBy = userId;
                            md.ModifiedDate = DateTime.Now;
                            md.ModifiedBy = userId;
                            ss.CreateOrUpdateMaterialDistribution(md);
                            return Json("insert", JsonRequestBehavior.AllowGet);

                        }
                        else { }
                    }
                } return Json(md.MaterialDistributionId, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult StudentMaterialDistribution()
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
                    return View();
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public ActionResult GetJqGridStudentMaterialDistribution_vwList(StudentMaterialDistribution_vw studmaterial, string IsHosteller, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                if (string.IsNullOrEmpty(studmaterial.Campus) || string.IsNullOrEmpty(studmaterial.Grade) || string.IsNullOrEmpty(studmaterial.Gender))
                {
                    var AssLst = new { rows = (new { cell = new string[] { } }) };
                    return Json(AssLst, JsonRequestBehavior.AllowGet);
                }
                StoreService ss = new StoreService();
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("Logout", "Account");
                else
                {
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    if (!string.IsNullOrEmpty(studmaterial.Name))
                        criteria.Add("Name", studmaterial.Name);
                    if (!string.IsNullOrEmpty(studmaterial.Campus))
                        criteria.Add("Campus", studmaterial.Campus);
                    if (!string.IsNullOrEmpty(studmaterial.Grade))
                        criteria.Add("Grade", studmaterial.Grade);
                    if (!string.IsNullOrEmpty(studmaterial.AcademicYear))
                        criteria.Add("AcademicYear", studmaterial.AcademicYear);
                    if (!string.IsNullOrEmpty(studmaterial.Section))
                        criteria.Add("Section", studmaterial.Section);
                    string[] arraygender = { "Male", "Female" };
                    if (!string.IsNullOrEmpty(studmaterial.Gender))
                    {
                        if (studmaterial.Gender == "All")
                        {
                            criteria.Add("Gender", arraygender);
                        }
                        else
                        {
                            criteria.Add("Gender", studmaterial.Gender);
                        }
                    }
                    bool[] arrayHosteller = new bool[2];
                    arrayHosteller[0] = true;
                    if (!string.IsNullOrEmpty(IsHosteller))
                    {
                        if (IsHosteller == "All")
                        {
                            criteria.Add("IsHosteller", arrayHosteller);
                        }
                        else
                        {
                            if (IsHosteller == "True")
                            {
                                criteria.Add("IsHosteller", true);
                            }
                            else
                            {
                                criteria.Add("IsHosteller", false);
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(studmaterial.MaterialSubGroup))
                        criteria.Add("MaterialSubGroup", studmaterial.MaterialSubGroup);
                    Dictionary<long, IList<StudentMaterialDistribution_vw>> studentMeterialDetails = null;
                    studentMeterialDetails = ss.GetStudentMaterialDistribution_vwListWithExcactAndLikeSearchCriteria(page - 1, rows, sidx, sord, criteria);
                    if (studentMeterialDetails != null && studentMeterialDetails.FirstOrDefault().Key > 0)
                    {
                        long totalRecords = studentMeterialDetails.First().Key;
                        int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                        var jsonData = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalRecords,
                            rows = (from items in studentMeterialDetails.First().Value
                                    select new
                                    {
                                        i = items.MaterialviewId,
                                        cell = new string[]{
                                      items.MaterialviewId.ToString(), 
                                      items.AcademicYear,                        
                                      items.Campus,                                     
                                      items.Grade,  
                                      items.Section,
                                      items.StudId.ToString(),
                                      items.NewId,                                        
                                      "<a href='#' onclick=\"StudentMaterialListPopup('" + items.StudId + "');\">" + items.Name+"</a>", 
                                      items.Gender,
                                      items.IsHosteller==true?"Yes":"No",                            
                                      items.MaterialSubGroup,
                                      items.Quantity.ToString(),
                                      items.MaterialSubGroupId.ToString(),                                       
                                      items.Material,                                                                                                            
                                      items.IssueId.ToString(),
                                      items.StudentId.ToString(),
                                      items.MaterialId.ToString(),
                                      items.IssuedQty==0?"":items.IssuedQty.ToString(),
                                      items.ReceivedQty==0?"":items.ReceivedQty.ToString(),
                                      items.PendingItems,
                                      items.ExtraQty==0?"":items.ExtraQty.ToString(),
                                      items.TotalQty==0?"":items.TotalQty.ToString(),
                                      items.MaterialDistributionId.ToString()
                               }
                                    })
                        };
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
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
                ExceptionPolicy.HandleException(ex, "MasterPolicy");
                throw ex;
            }
        }
        public ActionResult MaterialIssueDetails(long? StudId)
        {
            try
            {
                StoreService ss = new StoreService();
                StudentMaterialDistribution_vw StudentMaterialObj = ss.GetStudentMaterialDistribution_vwId(StudId ?? 0);
                if (StudentMaterialObj != null && StudentMaterialObj.StudId > 0)
                {
                    ViewBag.AcademicYear = StudentMaterialObj.AcademicYear;
                    ViewBag.Campus = StudentMaterialObj.Campus;
                    ViewBag.Grade = StudentMaterialObj.Grade;
                    ViewBag.NewId = StudentMaterialObj.NewId;
                    ViewBag.Name = StudentMaterialObj.Name;
                    ViewBag.Gender = StudentMaterialObj.Gender;
                    ViewBag.IsHosteller = StudentMaterialObj.IsHosteller;
                    ViewBag.StudId = StudId;
                } return View();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult GetJqGridMaterialIssueDetailsList(StudentMaterialDistribution_vw materialissue, long? StudId, string ExptType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                StoreService ss = new StoreService();
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("Logout", "Account");
                else
                {
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    if (StudId > 0)
                        criteria.Add("StudId", StudId);
                    Dictionary<long, IList<StudentMaterialDistribution_vw>> MeterialIssueDetails = null;
                    MeterialIssueDetails = ss.GetStudentMaterialDistribution_vwListWithExcactAndLikeSearchCriteria(page - 1, rows, sidx, sord, criteria);
                    if (MeterialIssueDetails != null && MeterialIssueDetails.FirstOrDefault().Key > 0)
                    {
                        long totalRecords = MeterialIssueDetails.First().Key;
                        int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                        var jsonData = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalRecords,
                            rows = (from items in MeterialIssueDetails.First().Value
                                    select new
                                    {
                                        i = items.MaterialviewId,
                                        cell = new string[]{
                                      items.MaterialviewId.ToString(), 
                                      items.AcademicYear,                           
                                      items.Campus,                                     
                                      items.Grade,  
                                      items.Section,
                                      items.StudId.ToString(),
                                      items.NewId,
                                      //String.Format(@"<a style='color:#034af3;text-decoration:underline' href= '/Store/MaterialIssueDetails?StudId="+items.StudId+"' >"+items.Name+"</a>"), 
                                      items.Name,
                                      items.Gender,
                                      items.IsHosteller==true?"Yes":"No",                            
                                      items.MaterialSubGroup,
                                      items.Quantity.ToString(),
                                      items.MaterialSubGroupId.ToString(),                                       
                                      items.Material,                                                                                                            
                                      items.IssueId.ToString(),
                                      items.StudentId.ToString(),
                                      items.MaterialId.ToString(),
                                      items.IssuedQty==0?"":items.IssuedQty.ToString(),
                                      items.ReceivedQty==0?"":items.ReceivedQty.ToString(),
                                      items.PendingItems,
                                      items.ExtraQty==0?"":items.ExtraQty.ToString(),
                                      items.TotalQty==0?"":items.TotalQty.ToString()                            
                                      
                                                                   
                     
                                                                      
                                  }
                                    })
                        };
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
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
                ExceptionPolicy.HandleException(ex, "MasterPolicy");
                throw ex;
            }
        }
        public ActionResult AddOrEditMaterialIssueDetails(MaterialIssueDetails materialisssue)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("logOut", "Account");
                else
                {
                    if (materialisssue.IssueId == 0)
                    {
                        StoreService ss = new StoreService();
                        Dictionary<string, object> criteria = new Dictionary<string, object>();
                        Dictionary<string, object> likeCriteria = new Dictionary<string, object>();


                        if (materialisssue == null) return null;
                        materialisssue.MaterialId = Convert.ToInt64(materialisssue.Material);
                        if (!string.IsNullOrEmpty(materialisssue.Material))
                            materialisssue.StudentId = materialisssue.StudId;

                        MaterialIssueDetails materialDetailsobj = new MaterialIssueDetails();
                        materialDetailsobj = ss.GetMaterialIssueDetailsById(materialisssue.IssueId);
                        var script = "";
                        Dictionary<string, object> criteria1 = new Dictionary<string, object>();
                        string[] IssuedStatus = new[] { "Not Issued", "Partially Issued" };
                        MaterialsMaster ms = ss.GetMaterialsMasterById(materialisssue.MaterialId);
                        MaterialDistribution smdv = ss.GetMaterialDistributionById(materialisssue.MaterialDistributionId);
                        criteria1.Add("Material", ms.Material.Trim());
                        criteria1.Add("Campus", smdv.Campus);
                        StoreStockBalance ssb = new StoreStockBalance();
                        ssb.ItemId = ms.Id;
                        ssb = ss.GetStoreStockBalanceById(ssb.ItemId);
                        Dictionary<long, IList<SkuList_vw>> SkuListvw = ss.GetSkuList_vwListWithPagingAndCriteria(0, 9999, null, null, criteria1);
                        if (SkuListvw != null && SkuListvw.FirstOrDefault().Value != null && SkuListvw.FirstOrDefault().Value.Count() > 0 && SkuListvw.FirstOrDefault().Key > 0)
                        {
                            int TobeIssuedQty = Convert.ToInt32(materialisssue.IssuedQty);
                            foreach (var item in SkuListvw.FirstOrDefault().Value)
                            {
                                if (ssb.ClosingBalance >= TobeIssuedQty)
                                {

                                    if (TobeIssuedQty <= item.StockAvailableQty)
                                    {
                                        SkuList skudetails = ss.GetSkuListById(item.SkuId);
                                        skudetails.IssuedQty = skudetails.IssuedQty + TobeIssuedQty;
                                        skudetails.StockAvailableQty = skudetails.DamagelessQty - skudetails.IssuedQty;
                                        if (skudetails.StockAvailableQty == 0)
                                            skudetails.IssuedStatus = "Completely Issued";
                                        else
                                            skudetails.IssuedStatus = "Partially Issued";

                                        ss.CreateOrUpdateSku(skudetails);
                                        break;
                                    }
                                    else
                                    {
                                        SkuList skudetails = ss.GetSkuListById(item.SkuId);
                                        skudetails.IssuedQty = skudetails.IssuedQty + skudetails.StockAvailableQty;
                                        TobeIssuedQty = Convert.ToInt32(TobeIssuedQty - skudetails.StockAvailableQty);
                                        skudetails.StockAvailableQty = 0;
                                        skudetails.IssuedStatus = "Completely Issued";
                                        ss.CreateOrUpdateSku(skudetails);
                                    }
                                }
                                else
                                {
                                    script = @"ErrMsg(""The Issued Quantity is Greater than Stock  Available Quantity!:"");";
                                    return JavaScript(script);
                                }
                            }
                            materialisssue.CreatedBy = userId;
                            materialisssue.CreatedDate = DateTime.Now;
                            ss.CreateOrUpdateMaterialIssueDetails(materialisssue);
                        }
                        else
                        {
                            script = @"ErrMsg(""The Issued Quantity is Greater than Stock  Available Quantity!:"");";
                            return JavaScript(script);
                        }
                        MaterialsMaster mm = ss.GetMaterialsMasterById(materialisssue.MaterialId);
                        MaterialInward mi = ss.GetMaterialInwardById(SkuListvw.FirstOrDefault().Value[0].MaterialRefId);
                        StockTransaction st = new StockTransaction();
                        mi.InwardNumber = "SMD-" + materialisssue.IssueId;
                        st.TransactionCode = mi.InwardNumber;
                        st.Store = mi.Store;
                        st.Units = mm.UnitCode;
                        st.ItemId = mm.Id;
                        st.TransactionDate = DateTime.Now;
                        st.TransactionBy = userId;
                        st.TransactionType = "Material Issue";
                        st.Qty = Convert.ToInt32(materialisssue.IssuedQty);
                        ss.CreateOrUpdateStockTransaction(st);

                    }
                    else
                        return Json(null, JsonRequestBehavior.AllowGet);
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
                throw ex;
            }
        }
        //public ActionResult AddOrEditMaterialIssueDetails(MaterialIssueDetails materialisssue)
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("logOut", "Account");
        //        else
        //        {

        //            StoreService ss = new StoreService();
        //            Dictionary<string, object> criteria = new Dictionary<string, object>();
        //            Dictionary<string, object> likeCriteria = new Dictionary<string, object>();


        //            if (materialisssue == null) return null;
        //            materialisssue.MaterialId = Convert.ToInt64(materialisssue.Material);
        //            if (!string.IsNullOrEmpty(materialisssue.Material))
        //                materialisssue.StudentId = materialisssue.StudId;

        //            MaterialIssueDetails materialDetailsobj = new MaterialIssueDetails();
        //            materialDetailsobj = ss.GetMaterialIssueDetailsById(materialisssue.IssueId);
        //            var script = "";
        //            if (materialisssue.IssueId == 0)
        //            {

        //                if (materialDetailsobj == null)
        //                {
        //                    materialisssue.CreatedBy = userId;
        //                    materialisssue.CreatedDate = DateTime.Now;
        //                    script = @"SucessMsg(""Added Sucessfully"");";
        //                    ss.CreateOrUpdateMaterialIssueDetails(materialisssue);
        //                    return JavaScript(script);
        //                }
        //                else
        //                {
        //                    script = @"ErrMsg(""The Name is already exist!"");";
        //                    return JavaScript(script);

        //                }
        //            }
        //            else
        //            {
        //                materialisssue.CreatedBy = userId;
        //                materialisssue.CreatedDate = DateTime.Now;
        //                materialisssue.ModifiedBy = userId;
        //                materialisssue.ModifiedDate = DateTime.Now;

        //                ss.CreateOrUpdateMaterialIssueDetails(materialisssue);
        //                script = @"SucessMsg(""Updated Sucessfully"");";
        //                return JavaScript(script);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "MasterPolicy");
        //        throw ex;
        //    }
        //}

        public ActionResult GetMaterialSubGroupddl(long? MaterialSubGroupId)
        {
            try
            {
                StoreService ss = new StoreService();

                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (MaterialSubGroupId > 0)
                    criteria.Add("MaterialSubGroupId", MaterialSubGroupId);
                Dictionary<long, IList<MaterialsMaster>> MaterilsList = ss.GetMaterialsMasterlistWithPagingAndCriteria(0, 999999, "", "", criteria);
                var camp = (from items in MaterilsList.FirstOrDefault().Value
                            select new
                            {
                                Text = items.Material,
                                Value = items.Id
                            }).Distinct().ToList();
                return Json(camp, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "MasterPolicy");
                throw ex;
            }
        }
        public ActionResult GetStudentMaterialSearch(string AcademicYear, string Campus, string Grade, string Gender, string IsHosteller)
        {
            try
            {
                StoreService ss = new StoreService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(AcademicYear))
                    criteria.Add("AcademicYear", AcademicYear);
                if (!string.IsNullOrEmpty(Campus))
                    criteria.Add("Campus", Campus);
                if (!string.IsNullOrEmpty(Grade))
                    criteria.Add("Grade", Grade);

                if (!string.IsNullOrEmpty(Gender))
                    criteria.Add("Gender", Gender);
                if (!string.IsNullOrEmpty(IsHosteller))
                    criteria.Add("IsHosteller", IsHosteller);

                Dictionary<long, IList<MaterialDistribution_Vw>> MaterilsDistributionList = ss.GetMaterialDistributionListWithPagingAndCriteria(0, 999999, "", "", criteria);
                var materialdistribution = (from items in MaterilsDistributionList.FirstOrDefault().Value
                                            select new
                                            {
                                                Text = items.MaterialSubGroup,
                                                Value = items.Id
                                            }).Distinct().ToList();
                return Json(materialdistribution, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IList<StudentMaterialDistribution_vw> StudentMaterialDistributionView(int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                StoreService ss = new StoreService();
                List<StudentMaterialDistribution_vw> StudentMaterialDistribution_vwReportList = new List<StudentMaterialDistribution_vw>();
                Dictionary<string, object> Criteria = new Dictionary<string, object>();
                IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                var StudentMaterialDistribution_vwList = new List<StudentMaterialDistribution_vw>();
                Dictionary<long, IList<StudentMaterialDistribution_vw>> GetStudentMaterialDistribution = ss.GetStudentMaterialDistribution_vwListWithExcactAndLikeSearchCriteria(page - 1, 9999999, sidx, sord, Criteria);
                if (GetStudentMaterialDistribution != null && GetStudentMaterialDistribution.Count > 0)
                {
                    StudentMaterialDistribution_vwList = GetStudentMaterialDistribution.FirstOrDefault().Value.ToList();
                    return StudentMaterialDistribution_vwList;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExportToExcelStudentMaterialDistribution(int rows, string sidx, string sord, int? page = 1)
        {
            //for export
            ExcelPackage objExcelPackage = new ExcelPackage();   //create new workbook
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            //string[] filesindirectory = Directory.GetFiles(Server.MapPath("~/Images"));
            string[] files = new string[5];
            int count = 0;
            ExcelWorksheet ews = objExcelPackage.Workbook.Worksheets.Add("Worksheet" + count); //create new worksheet
            ews.View.ZoomScale = 100;
            ews.View.ShowGridLines = true;
            ews.Cells["A3"].Value = "Sl.No";
            ews.Cells["B3"].Value = "Id.No";
            ews.Cells["C3"].Value = "Student Name";
            ews.Cells["D3:F3"].Value = "Bag";
            ews.Cells["D4"].Value = "Qty";
            ews.Cells["E4"].Value = "IssuedQty";
            ews.Cells["F3"].Value = "ReceivedQty";
            ews.Cells["G3"].Value = "Pending Items";
            ews.Cells["H3"].Value = "Parent's Signature";
            ews.Cells["A1:H1"].Merge = true;
            ews.Cells["A2:H2"].Merge = true;
            ews.Cells["A3:A4"].Merge = true;
            ews.Cells["B3:B4"].Merge = true;
            ews.Cells["C3:C4"].Merge = true;
            ews.Cells["G3:G4"].Merge = true;
            ews.Cells["H3:H4"].Merge = true;
            ews.Cells["D3:E3"].Merge = true;
            ews.Cells["A2:H2"].Value = "GRADE X- BOYS – BAG";
            ews.Cells["A1:H1"].Value = "PARENT ORIENDATION - CAMPUS";
            ews.Cells["A3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ews.Cells["B3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ews.Cells["C3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ews.Cells["D3:E3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ews.Cells["D4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ews.Cells["E4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ews.Cells["G3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ews.Cells["H3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ews.Cells["A1:H1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ews.Cells["A2:H2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ews.Cells["A1:H1"].Style.Font.Bold = true;
            ews.Cells["A2:H2"].Style.Font.Bold = true;
            int i = 5;
            int serialNo = 1;
            IList<StudentMaterialDistribution_vw> StudentMaterialDistribution_vwReportDetails = new List<StudentMaterialDistribution_vw>();
            StudentMaterialDistribution_vwReportDetails = StudentMaterialDistributionView(rows, sidx, sord, page - 1);
            if (StudentMaterialDistribution_vwReportDetails.Count > 0)
            {
                for (int k = 0; k < StudentMaterialDistribution_vwReportDetails.Count; k++)
                {
                    ews.Cells["A" + i].Value = serialNo;
                    ews.Cells["B" + i].Value = StudentMaterialDistribution_vwReportDetails[k].NewId;
                    ews.Cells["C" + i].Value = StudentMaterialDistribution_vwReportDetails[k].Name;
                    ews.Cells["D" + i].Value = StudentMaterialDistribution_vwReportDetails[k].Quantity;
                    ews.Cells["E" + i].Value = StudentMaterialDistribution_vwReportDetails[k].IssuedQty;
                    ews.Cells["F" + i].Value = StudentMaterialDistribution_vwReportDetails[k].ReceivedQty;
                    ews.Cells["G" + i].Value = StudentMaterialDistribution_vwReportDetails[k].PendingItems;
                    ews.Cells["A" + i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    ews.Cells["D" + i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ews.Cells["E" + i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ews.Cells["F" + i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ews.Cells["G" + i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    count++;
                    i = i + 1;
                    serialNo = serialNo + 1;
                }
            }
            ews.Cells[ews.Dimension.Address].AutoFitColumns();
            string Todaydate = DateTime.Now.ToString("dd/MM/yyyy");
            string FileName = "StudentMaterialDistribution" + "-On-" + Todaydate; ;
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;  filename=" + FileName + ".xlsx");
            byte[] File = objExcelPackage.GetAsByteArray();
            Response.BinaryWrite(File);
            Response.End();
        }

        public ActionResult StudentWiseReport(long? StudId)
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
                    ViewBag.StudId = StudId;
                    return View();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult GetJqGridStudentWiseReportList(StudentWiseMaterialReport StudReport, long? StudId, string ExptType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                StoreService ss = new StoreService();
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("Logout", "Account");
                else
                {
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    if (!string.IsNullOrEmpty(StudReport.Name))
                        criteria.Add("Name", StudReport.Name);
                    if (!string.IsNullOrEmpty(StudReport.Campus))
                        criteria.Add("Campus", StudReport.Campus);
                    if (!string.IsNullOrEmpty(StudReport.Grade))
                        criteria.Add("Grade", StudReport.Grade);
                    if (!string.IsNullOrEmpty(StudReport.AcademicYear))
                        criteria.Add("AcademicYear", StudReport.AcademicYear);
                    if (!string.IsNullOrEmpty(StudReport.Gender))
                        criteria.Add("Gender", StudReport.Gender);
                    if (StudId > 0)
                        criteria.Add("StudId", StudId);
                    sord = sord == "desc" ? "Desc" : "Asc";
                    Dictionary<long, IList<StudentWiseMaterialReport>> StudentWiseMaterialReportDetails = null;
                    StudentWiseMaterialReportDetails = ss.GetStudentWiseMaterialReportListWithExcactAndLikeSearchCriteria(page - 1, rows, sord, sidx, criteria);
                    if (StudentWiseMaterialReportDetails != null && StudentWiseMaterialReportDetails.FirstOrDefault().Key > 0)
                    {
                        if (ExptType == "Excel")
                        {
                            var List = StudentWiseMaterialReportDetails.First().Value.ToList();
                            base.ExptToXL(List, "MaterialsList", (items => new
                            {
                                items.StudReportId,
                                items.AcademicYear,
                                items.Campus,
                                items.Grade,
                                items.StudId,
                                items.NewId,
                                items.Name,
                                items.Gender,
                                items.MaterialSubGroupId,
                                items.MaterialId,
                                items.Tshirts,
                                items.Shirts,
                                items.Pant,
                                items.TotalQty
                            }));
                            return new EmptyResult();
                        }
                        else
                        {

                            long totalRecords = StudentWiseMaterialReportDetails.First().Key;
                            int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                            var jsonData = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalRecords,
                                rows = (from items in StudentWiseMaterialReportDetails.First().Value
                                        select new
                                        {
                                            i = items.StudReportId,
                                            cell = new string[]{
                                      items.StudReportId.ToString(), 
                                      items.AcademicYear,                          
                                      items.Campus,                                     
                                      items.Grade,                                      
                                      items.StudId.ToString(),
                                      items.NewId,
                                      //String.Format(@"<a style='color:#034af3;text-decoration:underline' href= '/Store/PurchaseDetailsForm"+items.StudReportId+"' >"+items.Name+"</a>") ,
                                      items.Name, 
                                      items.Gender,
                                      items.MaterialSubGroupId.ToString(),                                 
                                      items.MaterialId.ToString(),
                                      items.Tshirts==0?"":items.Tshirts.ToString(),
                                      items.Shirts==0?"":items.Shirts.ToString(),
                                      items.Pant==0?"":items.Pant.ToString(),
                                      items.TotalQty==0?"":items.TotalQty.ToString()                                      
                               }
                                        })
                            };
                            return Json(jsonData, JsonRequestBehavior.AllowGet);
                        }
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
                ExceptionPolicy.HandleException(ex, "MasterPolicy");
                throw ex;
            }
        }
        #endregion
        #region For MaterialDistributionReport by Dhanabalan

        public ActionResult MaterialDistributionReport()
        {
            try
            {
                StoreService ss = new StoreService();
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("Logout", "Account");
                else
                {
                    //ViewBag.campus = Campus;
                    //ViewBag.Grade = Grade;
                    //ViewBag.MaterialSubGroup = MaterialSubGroup;
                    return View();
                }

            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "MasterPolicy");
                throw ex;

            }
        }

        public ActionResult GetMaterialDistributionListReport_vw(string Campus, string Grade, string Material, string MaterialSubGroup, string sortby, string sorttype)
        {
            try
            {
                StoreService ss = new StoreService();
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("Logout", "Account");

                else
                {
                    Dictionary<string, object> Likecriteria = new Dictionary<string, object>();
                    Dictionary<string, object> ExactCriteria = new Dictionary<string, object>();
                    MastersService ms = new MastersService();
                    if (!string.IsNullOrEmpty(Campus))
                    {
                        ExactCriteria.Add("Campus", Campus);
                    }
                    if (!string.IsNullOrEmpty(Grade)) { ExactCriteria.Add("Grade", Grade); }
                    if (!string.IsNullOrEmpty(Material)) { ExactCriteria.Add("Material", Material); }
                    if (!string.IsNullOrEmpty(MaterialSubGroup)) { ExactCriteria.Add("MaterialSubGroup", MaterialSubGroup); }
                    Dictionary<long, IList<MaterialDistributionReport>> ReportList = ss.GetMaterialdistributionReportListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, ExactCriteria, Likecriteria);
                    //ReportList = ss.GetMaterialdistributionReportListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                    if (ReportList != null && ReportList.FirstOrDefault().Key > 0)
                    {
                        long totalRecords = ReportList.First().Key;
                        var jsonData = new
                        {
                            rows = (from items in ReportList.First().Value
                                    select new
                                    {
                                        items.Id,
                                        items.Campus,
                                        items.Grade,
                                        items.MaterialSubGroup,
                                        items.Material,
                                        items.IssuedTotal,

                                    })
                        };
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
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
                ExceptionPolicy.HandleException(ex, "MasterPolicy");
                throw ex;

            }
        }

        public void ExportToExcelMaterialDistributionReport(string campus, string Grade, string MaterialSubGroup, string Material, int rows, string sidx, string sord, int? page = 1)
        {
            //for export
            ExcelPackage objExcelPackage = new ExcelPackage();   //create new workbook
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            //string[] filesindirectory = Directory.GetFiles(Server.MapPath("~/Images"));
            string[] files = new string[5];
            int count = 0;
            ExcelWorksheet ews = objExcelPackage.Workbook.Worksheets.Add("Worksheet" + count); //create new worksheet
            ews.View.ZoomScale = 100;
            ews.View.ShowGridLines = true;
            ews.Cells["A3"].Value = "Sl.No";
            ews.Cells["B3"].Value = "Campus";
            ews.Cells["C3"].Value = "Grade";
            //ews.Cells["D3:F3"].Value = "Bag";
            //ews.Cells["D4"].Value = "Qty";
            ews.Cells["D4"].Value = "Material";
            ews.Cells["E4"].Value = "MaterialGroup";
            ews.Cells["F4"].Value = "Total Issued";
            //ews.Cells["G3"].Value = "Pending Items";
            //ews.Cells["H3"].Value = "Parent's Signature";
            ews.Cells["A1:H1"].Merge = true;
            ews.Cells["A2:H2"].Merge = true;
            ews.Cells["A3:A4"].Merge = true;
            ews.Cells["B3:B4"].Merge = true;
            ews.Cells["C3:C4"].Merge = true;
            ews.Cells["G3:G4"].Merge = true;
            ews.Cells["H3:H4"].Merge = true;
            ews.Cells["D3:E3"].Merge = true;
            ews.Cells["A2:H2"].Value = "Material Distribution Report";
            //ews.Cells["A1:H1"].Value = "PARENT ORIENDATION - CAMPUS";
            ews.Cells["A3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ews.Cells["B3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ews.Cells["C3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ews.Cells["D3:E3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ews.Cells["D4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ews.Cells["E4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ews.Cells["G3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ews.Cells["H3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ews.Cells["A1:H1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ews.Cells["A2:H2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ews.Cells["A1:H1"].Style.Font.Bold = true;
            ews.Cells["A2:H2"].Style.Font.Bold = true;
            int i = 5;
            long IssuedTotal = 0;
            int serialNo = 1;
            IList<MaterialDistributionReport> StudentMaterialDistribution_vwReportDetails = new List<MaterialDistributionReport>();
            StudentMaterialDistribution_vwReportDetails = MaterialDistributionReportView(campus, Grade, MaterialSubGroup, Material, rows, sidx, sord, page - 1);
            if (StudentMaterialDistribution_vwReportDetails.Count > 0)
            {
                for (int k = 0; k < StudentMaterialDistribution_vwReportDetails.Count; k++)
                {
                    ews.Cells["A" + i].Value = serialNo;
                    ews.Cells["B" + i].Value = StudentMaterialDistribution_vwReportDetails[k].Campus;
                    ews.Cells["C" + i].Value = StudentMaterialDistribution_vwReportDetails[k].Grade;
                    ews.Cells["D" + i].Value = StudentMaterialDistribution_vwReportDetails[k].MaterialSubGroup;
                    ews.Cells["E" + i].Value = StudentMaterialDistribution_vwReportDetails[k].Material;
                    ews.Cells["F" + i].Value = StudentMaterialDistribution_vwReportDetails[k].IssuedTotal;
                    ews.Cells["A" + i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    ews.Cells["D" + i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ews.Cells["E" + i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ews.Cells["F" + i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ews.Cells["G" + i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    count++;
                    i = i + 1;
                    serialNo = serialNo + 1;
                    IssuedTotal = IssuedTotal + StudentMaterialDistribution_vwReportDetails[k].IssuedTotal;
                }
            }
            ews.Cells["E" + i].Value = "Total";
            ews.Cells["E" + i].Style.Font.Bold = true;
            ews.Cells["F" + i].Value = IssuedTotal;
            ews.Cells["F" + i].Style.Font.Bold = true;
            ews.Cells[ews.Dimension.Address].AutoFitColumns();
            string Todaydate = DateTime.Now.ToString("dd/MM/yyyy");
            string FileName = "MaterialDistributionReport" + "-On-" + Todaydate; ;
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;  filename=" + FileName + ".xlsx");
            byte[] File = objExcelPackage.GetAsByteArray();
            Response.BinaryWrite(File);
            Response.End();
        }
        public IList<MaterialDistributionReport> MaterialDistributionReportView(string Campus, string Grade, string MaterialSubGroup, string Material, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                StoreService ss = new StoreService();
                Dictionary<string, object> LikeCriteria = new Dictionary<string, object>();
                Dictionary<string, object> Criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(Campus))
                {
                    Criteria.Add("Campus", Campus);
                }
                if (!string.IsNullOrEmpty(Grade)) { Criteria.Add("Grade", Grade); }
                if (!string.IsNullOrEmpty(Material)) { Criteria.Add("Material", Material); }
                if (!string.IsNullOrEmpty(MaterialSubGroup)) { Criteria.Add("MaterialSubGroup", MaterialSubGroup); }
                IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                var MaterialDistributionReport_vwList = new List<MaterialDistributionReport>();
                Dictionary<long, IList<MaterialDistributionReport>> StudentMaterialDistributionReport = ss.GetMaterialdistributionReportListWithPagingAndCriteria(page - 1, 9999999, sidx, sord, Criteria, LikeCriteria);
                if (StudentMaterialDistributionReport != null && StudentMaterialDistributionReport.Count > 0)
                {
                    MaterialDistributionReport_vwList = StudentMaterialDistributionReport.FirstOrDefault().Value.ToList();
                    return MaterialDistributionReport_vwList;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
#region  Report in StudentMaterialDistribution
        public ActionResult OverViewByMaterial()
        {
            try
            {
                StoreService ss = new StoreService();
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("Logout", "Account");
                else
                {
                    //ViewBag.campus = Campus;
                    //ViewBag.Grade = Grade;
                    //ViewBag.MaterialSubGroup = MaterialSubGroup;
                    return View();
                }

            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "MasterPolicy");
                throw ex;

            }
        }
        public ActionResult OverViewByCampus()
        {
            try
            {
                StoreService ss = new StoreService();
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("Logout", "Account");
                else
                {
                    //ViewBag.campus = Campus;
                    //ViewBag.Grade = Grade;
                    //ViewBag.MaterialSubGroup = MaterialSubGroup;
                    return View();
                }

            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "MasterPolicy");
                throw ex;

            }
        }
        public ActionResult GetMaterialDistributionListReportByCampus(string Campus, string Material, string sortby, string sorttype)
        {
            try
            {
                StoreService ss = new StoreService();
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("Logout", "Account");

                else
                {
                    Dictionary<string, object> Likecriteria = new Dictionary<string, object>();
                    Dictionary<string, object> ExactCriteria = new Dictionary<string, object>();
                    MastersService ms = new MastersService();
                    if (!string.IsNullOrEmpty(Campus))
                    {
                        ExactCriteria.Add("Campus", Campus);
                    }
                    if (!string.IsNullOrEmpty(Material)) { ExactCriteria.Add("Material", Material); }
                    Dictionary<long, IList<MaterialDistributionReportByCampus>> ReportList = ss.GetMaterialdistributionReportByCampusListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, ExactCriteria, Likecriteria);
                    //ReportList = ss.GetMaterialdistributionReportListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                    if (ReportList != null && ReportList.FirstOrDefault().Key > 0)
                    {
                        long totalRecords = ReportList.First().Key;
                        var jsonData = new
                        {
                            rows = (from items in ReportList.First().Value
                                    select new
                                    {
                                        items.Id,
                                        items.AcademicYear,
                                        items.Campus,
                                        items.Material,
                                        items.IssuedTotal,

                                    })
                        };
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
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
                ExceptionPolicy.HandleException(ex, "MasterPolicy");
                throw ex;

            }
        }

        public void ExportToExcelMaterialDistributionOverviewReport(string AcademicYear, string campus, string Material, int rows, string sidx, string sord, int? page = 1)
        {
            //for export
            ExcelPackage objExcelPackage = new ExcelPackage();   //create new workbook
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            //string[] filesindirectory = Directory.GetFiles(Server.MapPath("~/Images"));
            string[] files = new string[5];
            int count = 0;
            ExcelWorksheet ews = objExcelPackage.Workbook.Worksheets.Add("Worksheet" + count); //create new worksheet
            ews.View.ZoomScale = 100;
            ews.View.ShowGridLines = true;
            ews.Cells["A3"].Value = "Sl.No";
            ews.Cells["B3"].Value = "Academic Year";
            ews.Cells["C3"].Value = "Campus";
            //ews.Cells["D3:F3"].Value = "Bag";
            //ews.Cells["D4"].Value = "Qty";
            ews.Cells["D4"].Value = "Material";
            //ews.Cells["E4"].Value = "MaterialGroup";
            ews.Cells["E4"].Value = "Total";
            //ews.Cells["G3"].Value = "Pending Items";
            //ews.Cells["H3"].Value = "Parent's Signature";
            ews.Cells["A1:H1"].Merge = true;
            ews.Cells["A2:H2"].Merge = true;
            ews.Cells["A3:A4"].Merge = true;
            ews.Cells["B3:B4"].Merge = true;
            ews.Cells["C3:C4"].Merge = true;
            ews.Cells["G3:G4"].Merge = true;
            ews.Cells["H3:H4"].Merge = true;
            ews.Cells["D3:E3"].Merge = true;
            ews.Cells["A2:H2"].Value = "Material Distribution Report";
            //ews.Cells["A1:H1"].Value = "PARENT ORIENDATION - CAMPUS";
            ews.Cells["A3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ews.Cells["B3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ews.Cells["C3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ews.Cells["D3:E3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ews.Cells["D4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ews.Cells["E4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ews.Cells["G3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ews.Cells["H3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ews.Cells["A1:H1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ews.Cells["A2:H2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ews.Cells["A1:H1"].Style.Font.Bold = true;
            ews.Cells["A2:H2"].Style.Font.Bold = true;
            int i = 5;
            long IssuedTotal = 0;
            int serialNo = 1;
            IList<MaterialDistributionReportByCampus> StudentMaterialDistribution_vwReportDetails = new List<MaterialDistributionReportByCampus>();
            StudentMaterialDistribution_vwReportDetails = MaterialDistributionReportByCampus(AcademicYear, campus, Material, rows, sidx, sord, page - 1);
            if (StudentMaterialDistribution_vwReportDetails.Count > 0)
            {
                for (int k = 0; k < StudentMaterialDistribution_vwReportDetails.Count; k++)
                {
                    ews.Cells["A" + i].Value = serialNo;
                    ews.Cells["B" + i].Value = StudentMaterialDistribution_vwReportDetails[k].AcademicYear;
                    ews.Cells["C" + i].Value = StudentMaterialDistribution_vwReportDetails[k].Campus;
                    ews.Cells["D" + i].Value = StudentMaterialDistribution_vwReportDetails[k].Material;
                    ews.Cells["E" + i].Value = StudentMaterialDistribution_vwReportDetails[k].IssuedTotal;
                    ews.Cells["A" + i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    ews.Cells["D" + i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ews.Cells["E" + i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ews.Cells["F" + i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ews.Cells["G" + i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    count++;
                    i = i + 1;
                    serialNo = serialNo + 1;
                    IssuedTotal = IssuedTotal + StudentMaterialDistribution_vwReportDetails[k].IssuedTotal;
                }
            }
            //ews.Cells["E" + i].Value = "Total";
            //ews.Cells["E" + i].Style.Font.Bold = true;
            //ews.Cells["F" + i].Value = IssuedTotal;
            //ews.Cells["F" + i].Style.Font.Bold = true;
            ews.Cells[ews.Dimension.Address].AutoFitColumns();
            string Todaydate = DateTime.Now.ToString("dd/MM/yyyy");
            string FileName = "MaterialDistributionReport" + "-On-" + Todaydate; ;
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;  filename=" + FileName + ".xlsx");
            byte[] File = objExcelPackage.GetAsByteArray();
            Response.BinaryWrite(File);
            Response.End();
        }
        public IList<MaterialDistributionReportByCampus> MaterialDistributionReportByCampus(string AcademicYear, string Campus, string Material, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                StoreService ss = new StoreService();
                Dictionary<string, object> LikeCriteria = new Dictionary<string, object>();
                Dictionary<string, object> Criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(Campus))
                {
                    Criteria.Add("Campus", Campus);
                }
                if (!string.IsNullOrEmpty(AcademicYear)) { Criteria.Add("Grade", AcademicYear); }
                if (!string.IsNullOrEmpty(Material)) { Criteria.Add("Material", Material); }
                //if (!string.IsNullOrEmpty(MaterialSubGroup)) { Criteria.Add("MaterialSubGroup", MaterialSubGroup); }
                IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                var MaterialDistributionReport_vwList = new List<MaterialDistributionReportByCampus>();
                Dictionary<long, IList<MaterialDistributionReportByCampus>> StudentMaterialDistributionReport = ss.GetMaterialdistributionReportByCampusListWithPagingAndCriteria(page - 1, 9999999, sidx, sord, Criteria, LikeCriteria);
                if (StudentMaterialDistributionReport != null && StudentMaterialDistributionReport.Count > 0)
                {
                    MaterialDistributionReport_vwList = StudentMaterialDistributionReport.FirstOrDefault().Value.ToList();
                    return MaterialDistributionReport_vwList;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public ActionResult OverviewMaterialDistributionReportByDate()
        {
            try
            {

                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("Logout", "Account");
                else
                {
                    return View();
                }

            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "MasterPolicy");
                throw ex;

            }
        }
        public ActionResult GetMaterialDistributionListReportByDate(string Campus, string Date, string AcademicYear, string Material, string sortby, string sorttype)
        {
            try
            {
                StoreService ss = new StoreService();
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("Logout", "Account");

                else
                {
                    Dictionary<string, object> Likecriteria = new Dictionary<string, object>();
                    Dictionary<string, object> ExactCriteria = new Dictionary<string, object>();
                    MastersService ms = new MastersService();
                    if (!string.IsNullOrEmpty(Campus))
                    {
                        ExactCriteria.Add("Campus", Campus);
                    }
                    if (!string.IsNullOrEmpty(Date))
                    {
                        IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                        //DateTime SelDate = Convert.ToDateTime(Date, System.Globalization.CultureInfo.GetCultureInfo("en-CA").DateTimeFormat);
                        //DateTime SelDate = DateTime.Parse(Request["Date"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        DateTime SelDate = DateTime.Parse(Date, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        string To = string.Format("{0:dd/MM/yyyy}", SelDate);
                        DateTime TDate = Convert.ToDateTime(To + " " + "");
                        DateTime[] fromto = new DateTime[1];
                        fromto[0] = TDate;
                        ExactCriteria.Add("DatePart", TDate.Date);
                    }
                    if (!string.IsNullOrEmpty(AcademicYear))
                    {
                        ExactCriteria.Add("AcademicYear", AcademicYear);
                    }
                    if (!string.IsNullOrEmpty(Material))
                    {
                        ExactCriteria.Add("Material", Material);
                    }
                    Dictionary<long, IList<MaterialDistributionReportByDate>> ReportList = ss.GetMaterialDistributionReportByDateListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, ExactCriteria, Likecriteria);
                    //ReportList = ss.GetMaterialdistributionReportListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                    if (ReportList != null && ReportList.FirstOrDefault().Key > 0)
                    {
                        long totalRecords = ReportList.First().Key;
                        var jsonData = new
                        {
                            rows = (from items in ReportList.First().Value
                                    select new
                                    {
                                        items.Id,
                                        items.AcademicYear,
                                        items.Campus,
                                        items.Material,
                                        items.IssuedTotal,
                                        items.DatePart,


                                    })
                        };
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
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
                ExceptionPolicy.HandleException(ex, "MasterPolicy");
                throw ex;

            }
        }
#endregion
        #endregion
        #region Student Material Over View
        public ActionResult StudentMaterialOverView()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    DateTime dttime = DateTime.Now;
                    ViewBag.acadddl = GetAcademicYear();
                    ViewBag.currentAcyear = DateTime.Now.Month > 5 ? DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Year + 1).ToString() : (DateTime.Now.Year - 1).ToString() + "-" + DateTime.Now.Year.ToString();
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
                ExceptionPolicy.HandleException(ex, "MasterPolicy");
                throw ex;
            }
        }

        public ActionResult GetJqGridStudentMaterialOverView_vwList(StudentMaterialOverView_vw smo, string ExportType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    sord = sord == "desc" ? "Desc" : "Asc";
                    if (!string.IsNullOrEmpty(smo.AcademicYear))
                        criteria.Add("AcademicYear", smo.AcademicYear); //--AcademicYear
                    else
                    {
                        string currentAcademicYr = DateTime.Now.Month > 5 ? DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Year + 1).ToString() : (DateTime.Now.Year - 1).ToString() + "-" + DateTime.Now.Year.ToString();
                        criteria.Add("AcademicYear", currentAcademicYr);
                        //criteria.Add("TransactionType", "Distributed");
                    }
                    StoreService ss = new StoreService();

                    Dictionary<long, IList<StudentMaterialOverView_vw>> StudentMaterialOverList = null;
                    StudentMaterialOverList = ss.GetStudentMaterialOverView_vwListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    if (StudentMaterialOverList != null && StudentMaterialOverList.Count > 0)
                    {
                        var StudentList = StudentMaterialOverList.First().Value.ToList();
                        if (ExportType == "Excel")
                        {
                            ExptToXL(StudentList, "StudentMaterialOverviewReport", (items => new
                            {
                                items.AcademicYear,
                                MaterialSubGroup = items.MaterialSubGroup.Trim(),
                                items.IB_MAIN,
                                items.IB_KG,
                                items.Karur,
                                items.Karur_KG,
                                items.Tips_Saran,
                                items.Tirupur,
                                items.Tirupur_KG,
                                items.Ernakulam,
                                items.Chennai_Main,
                                items.Chennai_City,
                                items.CBSE_Main
                            }));
                            return new EmptyResult();
                        }
                        else
                        {
                            long totalrecords = StudentMaterialOverList.First().Key;
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var StudentLocalityList = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalrecords,
                                rows = (from items in StudentMaterialOverList.First().Value
                                        select new
                                        {
                                            i = items.Id,
                                            cell = new string[] { 
                                      items.Id.ToString(), 
                                      items.AcademicYear,
                                      items.MaterialSubGroup,
                                      "Overall",
                                      items.IB_MAIN.ToString(),                                     
                                      items.IB_KG.ToString(),  
                                      items.Karur.ToString(),
                                      items.Karur_KG.ToString(),
                                      items.Tips_Saran.ToString(),
                                      items.Tirupur.ToString(),
                                      items.Tirupur_KG.ToString(),
                                      items.Ernakulam.ToString(),                            
                                      items.Chennai_Main.ToString(),
                                      items.Chennai_City.ToString(),
                                      items.CBSE_Main.ToString(),

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
        #region Student Material Sub Group View By john naveen
        public ActionResult StudentMaterialSubGroupView()
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
                throw ex;
            }
        }
        public ActionResult GetJqGridStudentMaterialSubGroup_vwList(long Id, string ExportType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    sord = sord == "desc" ? "Desc" : "Asc";

                    StoreService ss = new StoreService();
                    StudentMaterialOverView_vw stm = new StudentMaterialOverView_vw();
                    stm = ss.GetStudentMaterialOverView_vwId(Id);
                    if (stm != null)
                    {
                        criteria.Add("AcademicYear", stm.AcademicYear);
                        criteria.Add("MaterialSubGroup", stm.MaterialSubGroup);
                    }
                    Dictionary<long, IList<StudentMaterialSubGroupView_vw>> StudentMaterialSubGroupViewList = null;
                    StudentMaterialSubGroupViewList = ss.GetStudentMaterialSubGroupView_vwListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    if (StudentMaterialSubGroupViewList != null && StudentMaterialSubGroupViewList.Count > 0)
                    {
                        var StudentList = StudentMaterialSubGroupViewList.First().Value.ToList();
                        if (ExportType == "Excel")
                        {
                            ExptToXL(StudentList, "StudentMaterialSubGroupReport", (items => new
                            {
                                items.Campus,
                                items.AcademicYear,
                                MaterialSubGroup = items.MaterialSubGroup.Trim(),
                                //items.MaterialSubGroup,
                                items.Material,
                                items.Total
                            }));
                            return new EmptyResult();
                        }
                        else
                        {
                            long totalrecords = StudentMaterialSubGroupViewList.First().Key;
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var StudentLocalityList = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalrecords,
                                rows = (from items in StudentMaterialSubGroupViewList.First().Value
                                        select new
                                        {
                                            i = items.Id,
                                            cell = new string[] { 
                                      items.Id.ToString(), 
                                      items.Campus,
                                      items.AcademicYear,
                                      items.MaterialSubGroupId.ToString(),
                                      items.MaterialId.ToString(),
                                      items.MaterialSubGroup,
                                      items.Material,
                                      items.Total.ToString()
                                     
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
        #region Student Material Distribution Sub Over View
        public ActionResult StudentMaterialSubOverView()
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
                throw ex;
            }
        }

        public ActionResult GetJqGridStudentMaterialSubOverView_vwList(long Id, string ExportType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    sord = sord == "desc" ? "Desc" : "Asc";

                    StoreService ss = new StoreService();
                    StudentMaterialOverView_vw stm = new StudentMaterialOverView_vw();
                    stm = ss.GetStudentMaterialOverView_vwId(Id);
                    if (stm != null)
                    {
                        criteria.Add("AcademicYear", stm.AcademicYear);
                        criteria.Add("MaterialSubGroup", stm.MaterialSubGroup);
                    }
                    Dictionary<long, IList<StudentMaterialSubOverView_vw>> StudentMaterialSubOverViewList = null;
                    StudentMaterialSubOverViewList = ss.GetStudentMaterialSubOverView_vwListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    if (StudentMaterialSubOverViewList != null && StudentMaterialSubOverViewList.Count > 0)
                    {
                        var StudentList = StudentMaterialSubOverViewList.First().Value.ToList();
                        if (ExportType == "Excel")
                        {
                            ExptToXL(StudentList, "StudentMaterialSubOverviewReport", (items => new
                            {
                                items.AcademicYear,
                                MaterialSubGroup = items.MaterialSubGroup.Trim(),
                                //items.MaterialSubGroup,
                                items.Material,
                                items.Stock,
                                items.IB_MAIN,
                                items.IB_KG,
                                items.Karur,
                                items.Karur_KG,
                                items.Tips_Saran,
                                items.Tirupur,
                                items.Tirupur_KG,
                                items.Ernakulam,
                                items.Chennai_Main,
                                items.Chennai_City,
                                items.CBSE_Main
                            }));
                            return new EmptyResult();
                        }
                        else
                        {
                            long totalrecords = StudentMaterialSubOverViewList.First().Key;
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var StudentLocalityList = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalrecords,
                                rows = (from items in StudentMaterialSubOverViewList.First().Value
                                        select new
                                        {
                                            i = items.Id,
                                            cell = new string[] { 
                                      items.Id.ToString(), 
                                      items.AcademicYear,
                                      items.MaterialSubGroup,
                                      items.MaterialSubGroupId.ToString(),
                                      items.MaterialId.ToString(),
                                      items.Material,
                                      items.Stock.ToString(),
                                      items.IB_MAIN.ToString(),                                     
                                      items.IB_KG.ToString(),  
                                      items.Karur.ToString(),
                                      items.Karur_KG.ToString(),
                                      items.Tips_Saran.ToString(),
                                      items.Tirupur.ToString(),
                                      items.Tirupur_KG.ToString(),
                                      items.Ernakulam.ToString(),                            
                                      items.Chennai_Main.ToString(),
                                      items.Chennai_City.ToString(),
                                      items.CBSE_Main.ToString()
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

    }
}