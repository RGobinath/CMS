using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TIPS.ServiceContract;
using TIPS.Entities.TransportEntities;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using TIPS.Service;
using TIPS.Entities;
using TIPS.Component;
using TIPS.Entities.AdmissionEntities;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.Controllers.PDFGeneration;
using System.Net;
using CMS.Helpers;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Globalization;

namespace CMS.Controllers
{
    public class TransportController : PdfViewController 
    {
        MastersService ms = new MastersService();
        TransportService ts = new TransportService();
        public ActionResult VehicleTypeMaster()
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
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult VehicleTypeMasterJqGrid(VehicleTypeMaster vtm, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    if (!string.IsNullOrWhiteSpace(vtm.VehicleType)) { criteria.Add("VehicleType", vtm.VehicleType); }
                    sord = sord == "desc" ? "Desc" : "Asc";
                    Dictionary<long, IList<VehicleTypeMaster>> VehicleTypeMaster = ts.GetVehicleTypeMasterListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    if (VehicleTypeMaster != null && VehicleTypeMaster.Count > 0)
                    {
                        long totalrecords = VehicleTypeMaster.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var VehicleType = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in VehicleTypeMaster.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                               items.Id.ToString(),items.VehicleType
                            }
                                    })
                        };
                        return Json(VehicleType, JsonRequestBehavior.AllowGet);

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

        public ActionResult VehicleSubTypeMaster(string SaveUrl, string reloadGridUrl, string GridId, string BulkEntryType)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    ViewBag.SaveUrl = SaveUrl;
                    ViewBag.reloadGridUrl = reloadGridUrl;
                    ViewBag.GridId = GridId;
                    ViewBag.BulkEntryType = BulkEntryType;
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult VehicleSubTypeMasterJqGrid(VehicleTypeAndSubType vtst, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    //  if (!string.IsNullOrWhiteSpace(vtst.VehicleTypeMaster.VehicleType)) { criteria.Add("VehicleTypeMaster." + "VehicleType", vtst.VehicleTypeMaster.VehicleType); }
                    if (!string.IsNullOrWhiteSpace(vtst.Type)) { criteria.Add("Type", vtst.Type); }
                    if (!string.IsNullOrWhiteSpace(vtst.VehicleNo)) { criteria.Add("VehicleNo", vtst.VehicleNo); }
                    if (!string.IsNullOrWhiteSpace(vtst.FuelType)) { criteria.Add("FuelType", vtst.FuelType); }
                    if (!string.IsNullOrWhiteSpace(vtst.Campus)) { criteria.Add("Campus", vtst.Campus); }
                    if (!string.IsNullOrWhiteSpace(vtst.Purpose)) { criteria.Add("Purpose", vtst.Purpose); }
                    string[] alias = new string[1];
                    alias[0] = "VehicleTypeMaster";
                    sord = sord == "desc" ? "Desc" : "Asc";
                    Dictionary<long, IList<VehicleTypeAndSubType>> VehicleSubTypeMaster = ts.GetVehicleTypeAndSubTypeListWithsearchCriteriaLikeSearch(page - 1, rows, sord, sidx, criteria, alias);
                    if (VehicleSubTypeMaster != null && VehicleSubTypeMaster.Count > 0)
                    {
                        long totalrecords = VehicleSubTypeMaster.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var VehicleSubType = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in VehicleSubTypeMaster.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                               items.VehicleTypeMaster.VehicleType,items.VehicleNo,  items.FuelType, items.Campus,
                               items.EngineType, items.EngineNumber, 
                               items.FirstRegisteredDate!=null?items.FirstRegisteredDate.Value.ToString("dd/MM/yyyy"):"",
                               items.Make, items.Type, items.ChassisNo, items.BHP, items.CC, items.WheelBase, items.UnladenWeight, items.Color,
                               items.GVW, 
                         //      items.RCAttachment,
                               "<a style='color:#034af3;text-decoration:underline' onclick = \"uploaddat('"+ items.Id + "','"+items.RCAttachment+ "');\"' >"+items.RCAttachment+"</a>",
                               items.Model, items.Address, items.Purpose, items.Id.ToString()
                            }
                                    })
                        };
                        return Json(VehicleSubType, JsonRequestBehavior.AllowGet);

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

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult VehicleTypeCRUD(VehicleTypeMaster vtm, string Type)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (Type == "delete")
                    {
                        TransportBC tbc = new TransportBC();
                        if (vtm.Id > 0)
                        {
                            tbc.DeleteVehicleType(vtm);
                        }
                    }
                    else
                    {
                        TransportService ts = new TransportService();
                        Dictionary<string, object> criteria = new Dictionary<string, object>();
                        if (!string.IsNullOrWhiteSpace(vtm.VehicleType)) criteria.Add("VehicleType", vtm.VehicleType.Trim());
                        Dictionary<long, IList<VehicleTypeMaster>> VehicleTypeMaster = ts.GetVehicleTypeMasterListWithPagingAndCriteria(null, null, null, null, criteria);
                        if (VehicleTypeMaster != null && VehicleTypeMaster.First().Value != null && (VehicleTypeMaster.First().Value.Count > 1 || VehicleTypeMaster.First().Value.Count > 0))
                        {
                            var script = @"ErrMsg(""Already Exists"");";
                            return JavaScript(script);
                        }

                        if (Type == "add")
                        {
                            { vtm.Id = 0; }
                            ViewBag.flag = 1;
                            vtm.VehicleType = vtm.VehicleType.Trim();
                            ts.CreateOrUpdateVehicleTypeMaster(vtm);
                        }
                        if (Type == "edit")
                        {
                            vtm.VehicleType = vtm.VehicleType.Trim();
                            ts.CreateOrUpdateVehicleTypeMaster(vtm);
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

    
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddVehicleSubType(VehicleSubTypeMaster vstm, string test, string Status)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    vstm.Type = !string.IsNullOrWhiteSpace(vstm.Type) ? vstm.Type.Trim() : string.Empty;
                    TransportService ts = new TransportService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    if (vstm.VehicleTypeId > 0)
                    {
                        criteria.Add("VehicleTypeId", vstm.VehicleTypeId);
                    }
                    if (!string.IsNullOrEmpty(vstm.Type))
                    {
                        criteria.Add("Type", vstm.Type);
                    }
                    if (!string.IsNullOrEmpty(vstm.VehicleNo))
                    {
                        criteria.Add("VehicleNo", vstm.VehicleNo);
                    }
                    // criteria.Add("IsActive", true);
                    Dictionary<long, IList<VehicleSubTypeMaster>> VehicleSubTypeMaster = ts.GetVehicleSubTypeMasterListWithPagingAndCriteria(null, null, null, null, criteria);
                    if (test != "edit")
                    {
                        if (VehicleSubTypeMaster != null && VehicleSubTypeMaster.First().Value != null && (VehicleSubTypeMaster.First().Value.Count > 1 || VehicleSubTypeMaster.First().Value.Count > 0))
                        {
                            var script = @"ErrMsg(""Already Exists"");";
                            return JavaScript(script);
                        }
                        vstm.Id = 0;
                    }
                    ViewBag.flag = 1;
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    if (vstm.FirstRegisteredDate == null)
                    {
                        if (!string.IsNullOrWhiteSpace(Request["FirstRegisteredDate"]))
                            vstm.FirstRegisteredDate = DateTime.Parse(Request["FirstRegisteredDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    }
                    if (Status == "Registered")
                        vstm.IsActive = true;
                    else if (Status == "Inactive")
                        vstm.IsActive = false;
                    ts.CreateOrUpdateVehicleSubTypeMaster(vstm);
                    return Json(vstm.Id, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult AddVehicleSubType(VehicleSubTypeMaster vstm, string test,string Status)
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //        else
        //        {
        //            vstm.Type = !string.IsNullOrWhiteSpace(vstm.Type) ? vstm.Type.Trim() : string.Empty;
        //            TransportService ts = new TransportService();
        //            Dictionary<string, object> criteria = new Dictionary<string, object>();
        //            criteria.Add("VehicleTypeId", vstm.VehicleTypeId);
        //            criteria.Add("Type", vstm.Type);
        //            criteria.Add("VehicleNo", vstm.VehicleNo);
        //           // criteria.Add("IsActive", true);
        //            Dictionary<long, IList<VehicleSubTypeMaster>> VehicleSubTypeMaster = ts.GetVehicleSubTypeMasterListWithPagingAndCriteria(null, null, null, null, criteria);
        //            if (VehicleSubTypeMaster != null && VehicleSubTypeMaster.First().Value != null && (VehicleSubTypeMaster.First().Value.Count > 1 || VehicleSubTypeMaster.First().Value.Count > 0))
        //            {
        //                var script = @"ErrMsg(""Already Exists"");";
        //                return JavaScript(script);
        //            }

        //            if (test != "edit")
        //            { vstm.Id = 0; }
        //            ViewBag.flag = 1;
        //            IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
        //            if (vstm.FirstRegisteredDate == null)
        //            {
        //                if (!string.IsNullOrWhiteSpace(Request["FirstRegisteredDate"]))
        //                    vstm.FirstRegisteredDate = DateTime.Parse(Request["FirstRegisteredDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
        //            }
        //            if (Status == "Registered")
        //                vstm.IsActive = true;
        //            else if (Status == "Inactive")
        //                vstm.IsActive = false;
        //            ts.CreateOrUpdateVehicleSubTypeMaster(vstm);
        //            return Json(vstm.Id, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
        //        throw ex;
        //    }
        //}
        public ActionResult VehicleSubTypeMasterJqSubGrid(VehicleSubTypeMaster vstm, int? Id, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    //criteria.Add("IsActive", true);
                    if (Id > 0)
                        criteria.Add("VehicleTypeId", Id);
                    if (!string.IsNullOrEmpty(vstm.VehicleNo)) { criteria.Add("VehicleNo", vstm.VehicleNo); }
                    if (!string.IsNullOrEmpty(vstm.FuelType)) { criteria.Add("FuelType", vstm.FuelType); }
                    if (!string.IsNullOrWhiteSpace(vstm.Campus)) { criteria.Add("Campus", vstm.Campus); }
                    if (!string.IsNullOrWhiteSpace(vstm.EngineType)) { criteria.Add("EngineType", vstm.EngineType); }
                    if (!string.IsNullOrWhiteSpace(vstm.EngineNumber)) { criteria.Add("EngineNumber", vstm.EngineNumber); }
                    if (!string.IsNullOrWhiteSpace(vstm.Make)) { criteria.Add("Make", vstm.Make); }
                    if (!string.IsNullOrWhiteSpace(vstm.Type)) { criteria.Add("Type", vstm.Type); }
                    if (!string.IsNullOrWhiteSpace(vstm.ChassisNo)) { criteria.Add("ChassisNo", vstm.ChassisNo); }
                    if (!string.IsNullOrWhiteSpace(vstm.BHP)) { criteria.Add("BHP", vstm.BHP); }
                    if (!string.IsNullOrWhiteSpace(vstm.CC)) { criteria.Add("CC", vstm.CC); }
                    if (!string.IsNullOrWhiteSpace(vstm.WheelBase)) { criteria.Add("WheelBase", vstm.WheelBase); }
                    if (!string.IsNullOrWhiteSpace(vstm.UnladenWeight)) { criteria.Add("UnladenWeight", vstm.UnladenWeight); }
                    if (!string.IsNullOrWhiteSpace(vstm.Color)) { criteria.Add("Color", vstm.Color); }
                    if (!string.IsNullOrWhiteSpace(vstm.GVW)) { criteria.Add("GVW", vstm.GVW); }
                    if (!string.IsNullOrWhiteSpace(vstm.Model)) { criteria.Add("Model", vstm.Model); }
                    if (!string.IsNullOrWhiteSpace(vstm.Address)) { criteria.Add("Address", vstm.Address); }
                    if (!string.IsNullOrWhiteSpace(vstm.Purpose)) { criteria.Add("Purpose", vstm.Purpose); }

                    if (!string.IsNullOrWhiteSpace(vstm.Purpose)) { criteria.Add("Purpose", vstm.Purpose); }
                    criteria.Add("IsActive", true);
                    sord = sord == "desc" ? "Desc" : "Asc";
                    Dictionary<long, IList<VehicleSubTypeMaster>> VehicleSubTypeMaster = ts.GetVehicleSubTypeMasterListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    if (VehicleSubTypeMaster != null && VehicleSubTypeMaster.Count > 0)
                    {
                        long totalrecords = VehicleSubTypeMaster.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var VehicleSubType = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in VehicleSubTypeMaster.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                               items.VehicleNo,items.FuelType,items.Campus,
                               items.EngineType, items.EngineNumber,  
                               items.FirstRegisteredDate!=null?items.FirstRegisteredDate.Value.ToString("dd/MM/yyyy"):"",
                               items.Make, items.Type, items.ChassisNo, items.BHP, items.CC, items.WheelBase, items.UnladenWeight, items.Color,
                               items.GVW, 
                         //      items.RCAttachment,
                               "<a style='color:#034af3;text-decoration:underline' onclick = \"uploaddat('"+ items.Id + "','"+items.RCAttachment+ "');\"' >"+items.RCAttachment+"</a>",
                              items.Model, items.Address,items.Purpose, items.Id.ToString()
                            }
                                    })
                        };
                        return Json(VehicleSubType, JsonRequestBehavior.AllowGet);
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

        public ActionResult VehicleDistanceCovered()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {

                    return PartialView();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult VehicleDistanceCovered(VehicleDistanceCovered vdc)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    ts.CreateOrUpdateVehicleDistanceCovered(vdc);
                    vdc.RefNo = "Ref No-" + vdc.Id;
                    ts.CreateOrUpdateVehicleDistanceCovered(vdc);
                    return RedirectToAction("TransportManagement", "Transport");
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult ShowVehicleDistanceCovered(int Id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (Id > 0)
                    {
                        TransportService ts = new TransportService();
                        DistanceCoveredDetails vdc = ts.GetDistanceCoveredDetailsById(Id);
                        return View(vdc);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult TransportManagement()
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
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult DistanceCoveredListJqGrid(string ExportType, VehicleDistanceCovered vdc, int? Id, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    //if (!string.IsNullOrWhiteSpace(vdc.Status))
                    //    Session["VehicleStatus"] = vdc.Status;                    
                    TransportService ts = new TransportService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    sord = sord == "desc" ? "Desc" : "Asc";
                    if (vdc.VehicleId > 0)
                        criteria.Add("VehicleId", vdc.VehicleId);
                    //if (vdc.RefId > 0)
                    //    criteria.Add("RefId", vdc.RefId);
                    if (!string.IsNullOrWhiteSpace(vdc.VehicleNo)) { criteria.Add("VehicleNo", vdc.VehicleNo); }
                    if (!string.IsNullOrWhiteSpace(vdc.VehicleType)) { criteria.Add("VehicleType", vdc.VehicleType); }
                    if (!string.IsNullOrWhiteSpace(vdc.Campus)) { criteria.Add("Campus", vdc.Campus); }
                    if (!string.IsNullOrWhiteSpace(vdc.DriverName)) { criteria.Add("DriverName", vdc.DriverName); }
                    //if (vdc.KMOut > 0) { criteria.Add("KMOut", vdc.KMOut); }
                    if (!string.IsNullOrWhiteSpace(vdc.PurposeType)) { criteria.Add("PurposeType", vdc.PurposeType); }
                    if (!string.IsNullOrWhiteSpace(vdc.Purpose)) { criteria.Add("Purpose", vdc.Purpose); }
                    if (!string.IsNullOrWhiteSpace(vdc.Source)) { criteria.Add("Source", vdc.Source); }
                    if (!string.IsNullOrWhiteSpace(vdc.Destination)) { criteria.Add("Destination", vdc.Destination); }
                    if (!string.IsNullOrWhiteSpace(vdc.ServiceCenterName)) { criteria.Add("ServiceCenterName", vdc.ServiceCenterName); }
                    //if (vdc.KMIn > 0) { criteria.Add("KMIn", vdc.KMIn); }
                    //if (vdc.DistanceCovered > 0) { criteria.Add("DistanceCovered", vdc.DistanceCovered); }
                    //if (!string.IsNullOrWhiteSpace(vdc.CreatedBy)) { criteria.Add("CreatedBy", vdc.CreatedBy); }
                    if (!string.IsNullOrWhiteSpace(vdc.Status)) { criteria.Add("Status", vdc.Status); }
                    if (string.IsNullOrWhiteSpace(vdc.Status)) { criteria.Add("Status", "Open"); }
                    Dictionary<long, IList<VehicleDistanceCovered>> VehicleDistanceCovered = ts.GetVehicleDistanceCoveredListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    if (VehicleDistanceCovered != null && VehicleDistanceCovered.Count > 0)
                    {
                        foreach (var items in VehicleDistanceCovered.FirstOrDefault().Value)
                        {
                            if (items.Status == "Open" && items.OutDateTime != null)
                            {
                                items.DifferenceInHours = DateTime.Now - items.OutDateTime;
                            }
                            else
                            {
                                items.DifferenceInHours = TimeSpan.Zero;
                            }
                        }
                        if (ExportType == "Excel")
                        {
                            var List = VehicleDistanceCovered.First().Value.ToList();
                            ExptToXL(List, "VehicleDistanceCovered", (items => new
                            {
                                items.Id,
                                items.VehicleId,
                                items.VehicleNo,
                                items.VehicleType,
                                items.Campus,
                                items.DriverName,
                                OutDateTime = items.OutDateTime != null ? items.OutDateTime.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : "",
                                KMOut = items.KMOut.ToString(),
                                items.PurposeType,
                                items.Purpose,
                                items.Source,
                                items.Destination,
                                InDateTime = items.InDateTime != null ? items.InDateTime.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : "",
                                KMIn = items.KMIn.ToString(),
                                DistanceCovered = items.DistanceCovered.ToString(),
                                IsAnyService = items.IsAnyService != true ? "No" : "Yes",
                                items.ServiceCenterName,
                                IsKMReseted = items.IsKMReseted != true ? "No" : "Yes",
                                KMResetValue = items.KMResetValue.ToString(),
                                CreatedDate = items.CreatedDate != null ? items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : "",
                                items.CreatedBy,
                                items.Status
                            }));
                            return new EmptyResult();
                        }
                        else
                        {

                            long totalrecords = VehicleDistanceCovered.First().Key;
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var VehicleDistanceList = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalrecords,
                                rows = (from items in VehicleDistanceCovered.First().Value
                                        select new
                                        {
                                            i = 2,
                                            cell = new string[] {
                               items.Id.ToString(),items.VehicleId.ToString(),items.VehicleNo,
                               items.VehicleType,
                               items.Campus,
                               items.DriverName,
                               items.OutDateTime!=null?items.OutDateTime.Value.ToString("dd/MM/yyyy hh:mm:ss tt"):"",
                               items.KMOut.ToString(),items.PurposeType,items.Purpose, items.Source,items.Destination,
                               items.InDateTime!=null?items.InDateTime.Value.ToString("dd/MM/yyyy hh:mm:ss tt"):"",
                               items.KMIn.ToString(),items.DistanceCovered.ToString(),
                               //items.IsAnyService!=true?"No":"Yes",
                               items.ServiceCenterName,
                               items.IsKMReseted!=true?"No":"Yes",
                               items.KMResetValue.ToString(),
                               items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy"):"",
                               items.CreatedBy,
                               items.Status,
                               items.Status=="Completed"?"Completed":items.DifferenceInHours.Value.TotalHours.ToString()
                            }
                                        })
                            };
                            return Json(VehicleDistanceList, JsonRequestBehavior.AllowGet);
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
        //    public ActionResult DistanceCoveredListJqGrid(string ExportType, VehicleDistanceCovered vdc,
        //string VehicleNo, string Route, string Source, string Destination, string DistanceCovered, string CreatedDate, string CreatedBy,
        //string Status, int? Id, int rows, string sidx, string sord, int? page = 1)
        //    {
        //        try
        //        {
        //            string userId = base.ValidateUser();
        //            if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //            else
        //            {
        //                TransportService ts = new TransportService();
        //                Dictionary<string, object> criteria = new Dictionary<string, object>();
        //                sord = sord == "desc" ? "Desc" : "Asc";
        //                if (vdc.VehicleId > 0)
        //                    criteria.Add("VehicleId", vdc.VehicleId);
        //                //if (vdc.RefId > 0)
        //                //    criteria.Add("RefId", vdc.RefId);

        //                Dictionary<string, object> ExactCriteria = new Dictionary<string, object>();
        //                Dictionary<string, object> LikeCriteria = new Dictionary<string, object>();


        //                if (!string.IsNullOrEmpty(DistanceCovered))
        //                {
        //                    decimal DisCovered = Convert.ToDecimal(DistanceCovered);
        //                    ExactCriteria.Add("DistanceCovered", DisCovered);
        //                }
        //                if (!string.IsNullOrEmpty(VehicleNo)) { LikeCriteria.Add("VehicleNo", VehicleNo); }
        //                if (!string.IsNullOrEmpty(Route)) { LikeCriteria.Add("Route", Route); }
        //                if (!string.IsNullOrEmpty(Source)) { LikeCriteria.Add("Source", Source); }
        //                if (!string.IsNullOrEmpty(Destination)) { LikeCriteria.Add("Destination", Destination); }
        //                //if (!string.IsNullOrEmpty(CreatedDate)) { LikeCriteria.Add("CreatedDate", CreatedDate); }
        //                if (!string.IsNullOrEmpty(CreatedBy)) { LikeCriteria.Add("CreatedBy", CreatedBy); }
        //                if (!string.IsNullOrEmpty(Status)) { LikeCriteria.Add("Status", Status); }
        //                //Dictionary<long, IList<VehicleDistanceCovered>> VehicleDistanceCovered = ts.GetVehicleDistanceCoveredListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
        //                Dictionary<long, IList<VehicleDistanceCovered>> VehicleDistanceCovered = ts.VehicleDistanceCoveredListWithLikeAndExcactSerachCriteria(page - 1, rows, sidx, sord, ExactCriteria, LikeCriteria);
        //                if (VehicleDistanceCovered != null && VehicleDistanceCovered.Count > 0)
        //                {

        //                    if (ExportType == "Excel")
        //                    {
        //                        var List = VehicleDistanceCovered.First().Value.ToList();
        //                        ExptToXL(List, "VehicleDistanceCoverdReportList", (items => new
        //                        {
        //                            Id = items.Id.ToString(),
        //                            Vehicle_Id = items.VehicleId.ToString(),
        //                            Vehicle_No = items.VehicleNo,
        //                            Route = items.Route,
        //                            Source = items.Source,
        //                            Destination = items.Destination,
        //                            Distance_Covered = items.DistanceCovered.ToString(),
        //                            Created_Date = items.CreatedDate != null ? items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : "",
        //                            Created_By = items.CreatedBy,
        //                            Status = items.Status
        //                        }));
        //                        return new EmptyResult();
        //                    }
        //                    else
        //                    {
        //                        long totalrecords = VehicleDistanceCovered.First().Key;
        //                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
        //                        var VehicleDistanceList = new
        //                        {
        //                            total = totalPages,
        //                            page = page,
        //                            records = totalrecords,
        //                            rows = (from items in VehicleDistanceCovered.First().Value
        //                                    select new
        //                                    {
        //                                        i = 2,
        //                                        cell = new string[] 
        //                                    {
        //                           items.Id.ToString(),
        //                           items.VehicleId.ToString(),
        //                           items.VehicleNo,
        //                           items.Route, 
        //                           items.Source,
        //                           items.Destination,
        //                           items.DistanceCovered.ToString(),
        //                           //items.CreatedDate.ToString(),
        //                           items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt"):"",
        //                           items.CreatedBy,
        //                           items.Status
        //                        }
        //                                    })
        //                        };
        //                        return Json(VehicleDistanceList, JsonRequestBehavior.AllowGet);
        //                    }
        //                }
        //                else return Json(null);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
        //            throw ex;
        //        }
        //    }

        public ActionResult FuelManagement()
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
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        //public ActionResult FuelRefillListJqGrid(VehicleFuelManagement vfm, int? Id, int rows, string sidx, string sord, int? page = 1)
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //        else
        //        {
        //            TransportService ts = new TransportService();
        //            Dictionary<string, object> criteria = new Dictionary<string, object>();
        //            if (vfm.VehicleId > 0)
        //                criteria.Add("VehicleId", vfm.VehicleId);
        //            sord = sord == "desc" ? "Desc" : "Asc";
        //            Dictionary<long, IList<VehicleFuelManagement>> VehicleFuelManagement = ts.GetVehicleFuelManagementListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
        //            if (VehicleFuelManagement != null && VehicleFuelManagement.Count > 0)
        //            {
        //                long totalrecords = VehicleFuelManagement.First().Key;
        //                int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
        //                var VehicleDistanceList = new
        //                {
        //                    total = totalPages,
        //                    page = page,
        //                    records = totalrecords,
        //                    rows = (from items in VehicleFuelManagement.First().Value
        //                            select new
        //                            {
        //                                i = 2,
        //                                cell = new string[] {

        //                       items.Id.ToString(),items.VehicleId.ToString(),items.VehicleNo,items.FuelType,
        //                       items.FuelQuantity.ToString(),
        //                       items.LitrePrice.ToString(),
        //                       items.TotalPrice.ToString(),
        //                       items.FilledBy,
        //                       items.FilledDate.ToString(),
        //                       items.BunkName,
        //                       items.FuelFillType,
        //                       items.LastMilometerReading.ToString(),
        //                       items.CurrentMilometerReading.ToString(),
        //                       items.Mileage.ToString(),
        //                      // items.CreatedDate.ToString(),
        //                       items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt"):"",
        //                       items.CreatedBy,items.Status
        //                    }
        //                            })
        //                };
        //                return Json(VehicleDistanceList, JsonRequestBehavior.AllowGet);

        //            }
        //            else return Json(null);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
        //        throw ex;
        //    }
        //}

        public ActionResult FuelRefillListJqGrid(string ExportType, VehicleFuelManagement vfm, string VehicleId, string FuelQuantity, string FilledDate, string LastMilometerReading, string CurrentMilometerReading, string Mileage, string CreatedDate,
            int? Id, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    Dictionary<string, object> ExactCriteria = new Dictionary<string, object>();
                    Dictionary<string, object> LikeCriteria = new Dictionary<string, object>();

                    if (ExportType != "Excel" && vfm.VehicleId > 0)
                        ExactCriteria.Add("VehicleId", vfm.VehicleId);
                    sord = sord == "desc" ? "Desc" : "Asc";


                    if (!string.IsNullOrEmpty(FuelQuantity))
                    {
                        ExactCriteria.Add("FuelQuantity", Convert.ToDecimal(FuelQuantity));
                    }
                    if (ExportType == "Excel")
                    {
                        int VehcleId = Convert.ToInt32(VehicleId);
                        if (VehcleId > 0) { ExactCriteria.Add("VehicleId", VehcleId); }
                    }
                    if (!string.IsNullOrEmpty(vfm.VehicleNo)) { LikeCriteria.Add("VehicleNo", vfm.VehicleNo); }
                    if (!string.IsNullOrEmpty(vfm.FuelType)) { LikeCriteria.Add("FuelType", vfm.FuelType); }
                    if (!string.IsNullOrEmpty(vfm.FilledBy)) { LikeCriteria.Add("FilledBy", vfm.FilledBy); }
                    if (!string.IsNullOrEmpty(vfm.BunkName)) { LikeCriteria.Add("BunkName", vfm.BunkName); }
                    if (!string.IsNullOrEmpty(vfm.FuelFillType)) { LikeCriteria.Add("FuelFillType", vfm.FuelFillType); }
                    if (!string.IsNullOrEmpty(LastMilometerReading)) { LikeCriteria.Add("LastMilometerReading", LastMilometerReading); }
                    if (!string.IsNullOrEmpty(CurrentMilometerReading)) { LikeCriteria.Add("CurrentMilometerReading", CurrentMilometerReading); }
                    if (!string.IsNullOrEmpty(Mileage)) { LikeCriteria.Add("Mileage", Mileage); }
                    if (!string.IsNullOrEmpty(CreatedDate)) { LikeCriteria.Add("CreatedDate", CreatedDate); }
                    if (!string.IsNullOrEmpty(vfm.CreatedBy)) { LikeCriteria.Add("CreatedBy", vfm.CreatedBy); }

                    Dictionary<long, IList<VehicleFuelManagement>> VehicleFuelManagement = ts.VehicleFuelManagementListWithLikeAndExcactSerachCriteria(page - 1, rows, sord, sidx, ExactCriteria, LikeCriteria);
                    if (VehicleFuelManagement != null && VehicleFuelManagement.Count > 0)
                    {
                        if (ExportType == "Excel")
                        {
                            var List = VehicleFuelManagement.First().Value.ToList();
                            ExptToXL(List, "VehicleFuelManagementReportList", (items => new
                            {
                                Id = items.Id.ToString(),
                                Vehicle_Id = items.VehicleId.ToString(),
                                Vehicle_No = items.VehicleNo,
                                Fuel_Type = items.FuelType,
                                Fuel_Quantity = items.FuelQuantity.ToString(),
                                LitrePrice = items.LitrePrice.ToString(),
                                TotalPrice = items.TotalPrice.ToString(),
                                Filled_By = items.FilledBy,
                                Filled_Date = items.FilledDate.ToString(),
                                Bunk_Name = items.BunkName,
                                Fuel_Fill_Type = items.FuelFillType,
                                Last_Milometer_Reading = items.LastMilometerReading.ToString(),
                                Current_Milometer_Reading = items.CurrentMilometerReading.ToString(),
                                Mileage = items.Mileage.ToString(),
                                Created_Date = items.CreatedDate != null ? items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : "",
                                Created_By = items.CreatedBy,
                                Status = items.Status
                            }));
                            return new EmptyResult();
                        }
                        else
                        {
                            long totalrecords = VehicleFuelManagement.First().Key;
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var VehicleDistanceList = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalrecords,
                                rows = (from items in VehicleFuelManagement.First().Value
                                        select new
                                        {
                                            i = 2,
                                            cell = new string[] {

                               items.Id.ToString(),items.VehicleId.ToString(),items.VehicleNo,items.FuelType, items.FuelQuantity.ToString(),
                               items.LitrePrice.ToString(),
                               items.TotalPrice.ToString(),
                               items.FilledBy,
                               items.FilledDate.ToString(),
                               items.BunkName,
                               items.FuelFillType,
                               items.LastMilometerReading.ToString(),
                               items.CurrentMilometerReading.ToString(),
                               items.Mileage.ToString(),
                               items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt"):"",
                               items.CreatedBy,items.Status
                            }
                                        })
                            };
                            return Json(VehicleDistanceList, JsonRequestBehavior.AllowGet);
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

        public ActionResult FuelRefill()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    return PartialView();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult FuelRefill(VehicleFuelManagement vfm)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    ts.CreateOrUpdateVehicleFuelManagement(vfm);
                    vfm.RefNo = "Ref-" + vfm.Id;
                    ts.CreateOrUpdateVehicleFuelManagement(vfm);
                    return RedirectToAction("FuelManagement", "Transport");
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult ShowFuelRefill(int Id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (Id > 0)
                    {
                        TransportService ts = new TransportService();
                        FuelRefillDetails vfm = ts.GetFuelRefillDetailsById(Id);
                        return View(vfm);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult TransportDetails(int? VehicleId)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    VehicleDetails vd = new VehicleDetails();
                    VehicleSubTypeMaster vstm = new VehicleSubTypeMaster();
                    if (VehicleId > 0)
                    {
                        vstm = ts.GetVehicleSubTypeMasterById(Convert.ToInt32(VehicleId));
                    }
                    vd.Id = vstm.Id;
                    vd.Campus = vstm.Campus;
                    vd.Type = vstm.Type;
                    vd.VehicleNo = vstm.VehicleNo;
                    vd.Purpose = vstm.Purpose;
                    vd.FuelType = vstm.FuelType;
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<long, IList<LocationMaster>> LocationMaster = ts.GetLocationMasterDetails(0, 9999, "LocationName", "Asc", criteria);
                    Dictionary<long, IList<DriverMaster>> DriverMaster = ts.GetDriverMasterDetails(0, 9999, "Name", "Asc", criteria);
                    ViewBag.LocationMaster = LocationMaster.First().Value;
                    // ViewBag.DriverMaster =  DriverMaster.First().Value.TakeWhile( from items in DriverMaster.FirstOrDefault().Value where items.Name!=null select items.Name);
                    ViewBag.DriverMaster = (from items in DriverMaster.FirstOrDefault().Value
                                            where items.Name != null && items.Name != ""
                                            select items).Distinct().ToList();
                    return View(vd);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        #region ac and penalities

        [HttpPost]
        public ActionResult AddFinesAndPenalities(FinesAndPenalities fap)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportBC faprepo = new TransportBC();
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    fap.PenalityDate = DateTime.Parse(Request["PenalityDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    fap.PenalityDueDate = DateTime.Parse(Request["PenalityDueDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    fap.CreatedDate = DateTime.Now;
                    fap.CreatedBy = userId;
                    faprepo.SaveOrUpdateFinesAndPenalities(fap);
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        //public ActionResult FinesAndPenalitiesjqgrid(FinesAndPenalities fp, string Id, int rows, string sidx, string sord, int? page = 1)
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //        else
        //        {
        //            TransportBC fines = new TransportBC();
        //            Dictionary<string, object> criteria = new Dictionary<string, object>();
        //            if (fp.VehicleId > 0)
        //                criteria.Add("VehicleId", fp.VehicleId);
        //            Dictionary<long, IList<FinesAndPenalities>> FinesList = fines.GetFinesAndPenalitiesIdViewListWithCriteria(page - 1, rows, sidx, sord, criteria);

        //            long totalrecords = FinesList.First().Key;
        //            int totalPages = (int)Math.Ceiling(totalrecords / (float)5);
        //            var AssLst = new
        //            {
        //                total = totalPages,
        //                page = page,
        //                records = totalrecords,

        //                rows = (
        //                     from items in FinesList.First().Value

        //                     select new
        //                     {
        //                         cell = new string[] 
        //                                 {
        //                                    items.Id.ToString(),
        //                                    items.VehicleId.ToString(),
        //                                    items.VehicleNo,
        //                                    items.PenalityDate!=null?items.PenalityDate.Value.ToString("dd/MM/yyyy"):"",
        //                                    items.PenalityArea,
        //                                    items.PenalityReason,
        //                                    items.PenalityRupees.ToString(),
        //                                    items.PenalityDueDate!=null?items.PenalityDueDate.Value.ToString("dd/MM/yyyy"):"",
        //                                    items.PenalityPaidBy,
        //                                    items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt"):"",
        //                                    items.CreatedBy
        //                                 }
        //                     }).ToList()
        //            };
        //            return Json(AssLst, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
        //        throw ex;
        //    }
        //}


        public ActionResult FinesAndPenalitiesjqgrid(FinesAndPenalities fp, string Id, string ExportType, string VehicleId, string PenalityDate, string PenalityRupees,
         string PenalityDueDate, string CreatedDate, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportBC fines = new TransportBC();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<string, object> ExactCriteria = new Dictionary<string, object>();
                    Dictionary<string, object> LikeCriteria = new Dictionary<string, object>();
                    if (fp.VehicleId > 0)
                        ExactCriteria.Add("VehicleId", fp.VehicleId);
                    if (!string.IsNullOrEmpty(PenalityRupees))
                    {
                        long Rupees = Convert.ToInt64(PenalityRupees);
                        ExactCriteria.Add("PenalityRupees", Rupees);
                    }
                    if (!string.IsNullOrEmpty(fp.VehicleNo)) { LikeCriteria.Add("VehicleNo", fp.VehicleNo); }
                    if (!string.IsNullOrEmpty(fp.PenalityArea)) { LikeCriteria.Add("PenalityArea", fp.PenalityArea); }
                    if (!string.IsNullOrEmpty(fp.PenalityReason)) { LikeCriteria.Add("PenalityReason", fp.PenalityReason); }
                    if (!string.IsNullOrEmpty(fp.PenalityPaidBy)) { LikeCriteria.Add("PenalityPaidBy", fp.PenalityPaidBy); }
                    if (!string.IsNullOrEmpty(fp.CreatedBy)) { LikeCriteria.Add("CreatedBy", fp.CreatedBy); }
                    Dictionary<long, IList<FinesAndPenalities>> FinesList = fines.FinesAndPenalitiesListWithLikeAndExcactSerachCriteria(page - 1, rows, sord, sidx, ExactCriteria, LikeCriteria);
                    if (FinesList != null && FinesList.Count > 0)
                    {
                        if (ExportType == "Excel")
                        {
                            var List = FinesList.First().Value.ToList();
                            ExptToXL(List, "FinesAndPenalities", (items => new
                            {
                                Id = items.Id.ToString(),
                                VehicleId = items.VehicleId.ToString(),
                                VehicleNo = items.VehicleNo,
                                PenalityDate = items.PenalityDate != null ? items.PenalityDate.Value.ToString("dd/MM/yyyy") : "",
                                PenalityArea = items.PenalityArea,
                                PenalityReason = items.PenalityReason,
                                PenalityRupees = items.PenalityRupees.ToString(),
                                PenalityDueDate = items.PenalityDueDate != null ? items.PenalityDueDate.Value.ToString("dd/MM/yyyy") : "",
                                PenalityPaidBy = items.PenalityPaidBy,
                                CreatedDate = items.CreatedDate != null ? items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : "",
                                CreatedBy = items.CreatedBy
                            }));
                            return new EmptyResult();
                        }
                        else
                        {
                            long totalrecords = FinesList.First().Key;
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)5);
                            var AssLst = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalrecords,

                                rows = (
                                     from items in FinesList.First().Value

                                     select new
                                     {
                                         cell = new string[] 
                                         {
                                            items.Id.ToString(),
                                            items.VehicleId.ToString(),
                                          
                                            items.VehicleNo,
                                            items.PenalityDate!=null?items.PenalityDate.Value.ToString("dd/MM/yyyy"):"",
                                            items.PenalityArea,
                                            items.PenalityReason,
                                            items.PenalityRupees.ToString(),
                                            items.PenalityDueDate!=null?items.PenalityDueDate.Value.ToString("dd/MM/yyyy"):"",
                                            items.PenalityPaidBy,
                                            items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt"):"",
                                            items.CreatedBy
                                         }
                                     }).ToList()
                            };
                            return Json(AssLst, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else { return Json(null); }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        #endregion

        public ActionResult Maintenance()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    return PartialView();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult FinesandPenalities()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    return PartialView();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult TransportDetailsManagement()
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

        public ActionResult AddVehicleDistanceCovered(VehicleDistanceCovered vdc)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    vdc.CreatedDate = DateTime.Now;
                    vdc.CreatedBy = userId;
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    if (!string.IsNullOrWhiteSpace(Request["OutDateTime"]))
                    {
                        vdc.OutDateTime = DateTime.Parse(Request["OutDateTime"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    }
                    //if (!string.IsNullOrWhiteSpace(Request["InDateTime"]))
                    //{
                    //    vdc.InDateTime = DateTime.Parse(Request["InDateTime"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    //}
                    // vdc.TripDate = DateTime.Now;
                    if (vdc.DistanceCovered == 0)
                        vdc.Status = "Open";
                    else
                        vdc.Status = "Completed";

                    ts.CreateOrUpdateVehicleDistanceCovered(vdc);
                    if (vdc.Purpose != null)
                    {
                        TripPurposeMaster tpm = ts.GetPurposeByPurpose(vdc.Purpose);
                        if (tpm == null)
                        {
                            tpm = new TripPurposeMaster();
                            tpm.Purpose = vdc.Purpose;
                            ts.CreateOrUpdateTripPurposeMaster(tpm);
                        }
                    }
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        //public ActionResult AddFuelRefill(VehicleFuelManagement vfm)
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //        else
        //        {
        //            TransportService ts = new TransportService();
        //            IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
        //            vfm.FilledDate = DateTime.Parse(Request["FilledDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
        //            vfm.CreatedDate = DateTime.Now;
        //            vfm.CreatedBy = userId;
        //            if (vfm.IsKMReseted == false && vfm.KMResetValue == 0)
        //            {
        //                Dictionary<string, object> criteria = new Dictionary<string, object>();
        //                criteria.Add("Campus", vfm.Campus);
        //                criteria.Add("VehicleNo", vfm.VehicleNo);
        //                Dictionary<long, IList<FuelRefilDetails_Vw>> FuelList = ts.GetFuelTypeDetails(0, 9999, "Id", "Desc", criteria);
        //                if (FuelList.FirstOrDefault().Value[0] != null)
        //                {
        //                    decimal Distance = vfm.CurrentMilometerReading - vfm.LastMilometerReading;
        //                    vfm.CurrentMiloMeterReading1 = FuelList.FirstOrDefault().Value[0].CurrentMiloMeterReading1 + Distance;
        //                    vfm.LastMiloMeterReading1 = FuelList.FirstOrDefault().Value[0].CurrentMiloMeterReading1;
        //                }
        //            }
        //            if (vfm.IsKMReseted == true && vfm.KMResetValue > 0)
        //            {
        //                Dictionary<string, object> criteria = new Dictionary<string, object>();
        //                criteria.Add("Campus", vfm.Campus);
        //                criteria.Add("VehicleNo", vfm.VehicleNo);
        //                Dictionary<long, IList<FuelRefilDetails_Vw>> FuelList = ts.GetFuelTypeDetails(0, 9999, "Id", "Desc", criteria);
        //                decimal Distance = vfm.KMResetValue - vfm.LastMilometerReading + vfm.CurrentMilometerReading;
        //                vfm.CurrentMiloMeterReading1 = FuelList.FirstOrDefault().Value[0].CurrentMiloMeterReading1 + Distance;
        //                vfm.LastMiloMeterReading1 = FuelList.FirstOrDefault().Value[0].CurrentMiloMeterReading1;
        //            }
        //            ts.CreateOrUpdateVehicleFuelManagement(vfm);
        //            return Json("success", JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
        //        throw ex;
        //    }
        //}
        

        public ActionResult AddFuelRefill(VehicleFuelManagement vfm)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    vfm.FilledDate = DateTime.Parse(Request["FilledDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    vfm.CreatedDate = DateTime.Now;
                    vfm.CreatedBy = userId;
                    //if (vfm.IsKMReseted == false && vfm.KMResetValue == 0)
                    //{

                    //    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    //    criteria.Add("Campus", vfm.Campus);
                    //    criteria.Add("VehicleNo", vfm.VehicleNo);
                    //    Dictionary<long, IList<FuelRefilDetails_Vw>> FuelList = ts.GetFuelTypeDetails(0, 9999, "Id", "Desc", criteria);
                    //    if (FuelList.FirstOrDefault().Value[0] != null)
                    //    {
                    //        decimal Distance = vfm.CurrentMilometerReading - vfm.LastMilometerReading;
                    //        vfm.CurrentMiloMeterReading1 = FuelList.FirstOrDefault().Value[0].CurrentMiloMeterReading1 + Distance;
                    //        vfm.LastMiloMeterReading1 = FuelList.FirstOrDefault().Value[0].CurrentMiloMeterReading1;
                    //    }
                    //}
                    if (vfm.IsKMReseted == true && vfm.KMResetValue > 0)
                    {
                        VehicleFuelManagement vfmanagement = new VehicleFuelManagement();

                        decimal Distance1 = vfm.KMResetValue - vfm.LastMilometerReading;
                        decimal Distance2 = vfm.Distance - Distance1;
                        vfmanagement.Campus = vfm.Campus;
                        vfmanagement.VehicleNo = vfm.VehicleNo;
                        vfmanagement.Distance = Distance2;
                        vfmanagement.FuelQuantity = (Distance2 / vfm.Distance) * vfm.FuelQuantity;
                        vfmanagement.LitrePrice = vfm.LitrePrice;
                        decimal FuelQuantity = vfmanagement.FuelQuantity ?? 0;
                        vfmanagement.TotalPrice = FuelQuantity * vfm.LitrePrice;
                        vfmanagement.CurrentMilometerReading = vfm.CurrentMilometerReading;
                        vfmanagement.FilledDate = vfm.FilledDate;
                        vfmanagement.FilledBy = vfm.FilledBy;
                        vfmanagement.BunkName = vfm.BunkName;
                        vfmanagement.FuelFillType = vfm.FuelFillType;
                        vfmanagement.LastMilometerReading = 0;
                        vfmanagement.IsKMReseted = false;
                        vfmanagement.Mileage = Distance2 / FuelQuantity;
                        vfmanagement.CreatedBy = userId;
                        vfm.CurrentMilometerReading = vfm.KMResetValue;
                        vfm.FuelQuantity = (Distance1 / vfm.Distance) * vfm.FuelQuantity;
                        decimal FuelQuantity1 = vfm.FuelQuantity ?? 0;
                        vfm.TotalPrice = FuelQuantity1 * vfm.LitrePrice;
                        vfm.Mileage = Distance1 / FuelQuantity1;
                        vfm.Distance = Distance1;
                        ts.CreateOrUpdateVehicleFuelManagement(vfm);
                        vfmanagement.CreatedDate = vfm.CreatedDate;
                        vfmanagement.VehicleId = vfm.VehicleId;
                        vfmanagement.FuelType = vfm.FuelType;
                        ts.CreateOrUpdateVehicleFuelManagement(vfmanagement);
                        return Json("success", JsonRequestBehavior.AllowGet);

                        //Dictionary<string, object> criteria = new Dictionary<string, object>();
                        //criteria.Add("Campus", vfm.Campus);
                        //criteria.Add("VehicleNo", vfm.VehicleNo);
                        //Dictionary<long, IList<FuelRefilDetails_Vw>> FuelList = ts.GetFuelTypeDetails(0, 9999, "Id", "Desc", criteria);
                        //decimal Distance=vfm.KMResetValue-vfm.LastMilometerReading+vfm.CurrentMilometerReading;
                        //vfm.CurrentMiloMeterReading1 = FuelList.FirstOrDefault().Value[0].CurrentMiloMeterReading1 + Distance;
                        //vfm.LastMiloMeterReading1 = FuelList.FirstOrDefault().Value[0].CurrentMiloMeterReading1;

                    }
                    ts.CreateOrUpdateVehicleFuelManagement(vfm);
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        #region FitnessCertificate
        public PartialViewResult FitnessCertificate()
        {
            return PartialView();
        }

        public ActionResult AddFitnessCertificateDetails(FitnessCertificate fc)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportBC fcrepo = new TransportBC();
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    fc.FCDate = DateTime.Parse(Request["FCDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    fc.NextFCDate = DateTime.Parse(Request["NextFCDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    fc.FCTaxValidUpto = DateTime.Parse(Request["FCTaxValidUpto"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    fc.CreatedBy = userId;
                    fc.CreatedDate = DateTime.Now;
                    fcrepo.SaveOrUpdateFitnessCertificateDetails(fc);
                    return Json(fc.Id, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        //public ActionResult FitnessCertificateJqGrid(FitnessCertificate fc, int rows, string sidx, string sord, int? page = 1)
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //        else
        //        {
        //            TransportBC samp = new TransportBC();
        //            Dictionary<string, object> criteria = new Dictionary<string, object>();
        //            if (fc.VehicleId > 0)
        //                criteria.Add("VehicleId", fc.VehicleId);
        //            Dictionary<long, IList<FitnessCertificate>> FitnessCertificateList = samp.GetFitnessCertificateDetailsWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
        //            if (FitnessCertificateList != null && FitnessCertificateList.First().Key > 0)
        //            {
        //                long totalrecords = FitnessCertificateList.FirstOrDefault().Key;
        //                int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
        //                var AssLst = new
        //                {
        //                    total = totalPages,
        //                    page = page,
        //                    records = totalrecords,

        //                    rows = (
        //                         from items in FitnessCertificateList.First().Value


        //                         select new
        //                         {
        //                             cell = new string[] 
        //                                 {
        //                                    items.Id.ToString(),
        //                                    items.VehicleId.ToString(),
        //                                    items.VehicleNo,
        //                                    items.FCDate!=null?items.FCDate.Value.ToString("dd/MM/yyyy"):"",
        //                                    items.NextFCDate!=null?items.NextFCDate.Value.ToString("dd/MM/yyyy"):"",
        //                                    items.FCCost.ToString(),
        //                                    items.Description,
        //                                    items.FCWorkCarriedAt,
        //                                    items.RTO,
        //                                    items.Driver, 
        //                                     "<a style='color:#034af3;text-decoration:underline' onclick = \"uploaddat1('"+ items.Id + "','"+items.FCertificate+ "');\"' >"+items.FCertificate+"</a>",
        //                                 }
        //                         }).ToList()
        //                };
        //                return Json(AssLst, JsonRequestBehavior.AllowGet);

        //            }
        //            else
        //            {
        //                return null;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
        //        throw ex;
        //    }
        //}

        public ActionResult FitnessCertificateJqGrid(FitnessCertificate fc, string ExportType, string FCDate, string NextFCDate, string FCCost, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportBC samp = new TransportBC();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<string, object> ExactCriteria = new Dictionary<string, object>();
                    Dictionary<string, object> LikeCriteria = new Dictionary<string, object>();
                    if (fc.VehicleId > 0)
                        ExactCriteria.Add("VehicleId", fc.VehicleId);
                    if (!string.IsNullOrEmpty(fc.VehicleNo)) { LikeCriteria.Add("VehicleNo", fc.VehicleNo); }
                    if (!string.IsNullOrEmpty(FCCost))
                    {
                        ExactCriteria.Add("FCCost", Convert.ToInt64(FCCost));
                    }
                    if (!string.IsNullOrEmpty(fc.Description)) { LikeCriteria.Add("Description", fc.Description); }
                    if (!string.IsNullOrEmpty(fc.FCWorkCarriedAt)) { LikeCriteria.Add("FCWorkCarriedAt", fc.FCWorkCarriedAt); }
                    if (!string.IsNullOrEmpty(fc.RTO)) { LikeCriteria.Add("RTO", fc.RTO); }
                    if (!string.IsNullOrEmpty(fc.Driver)) { LikeCriteria.Add("Driver", fc.Driver); }
                    if (!string.IsNullOrEmpty(fc.FCertificate)) { LikeCriteria.Add("FCertificate", fc.FCertificate); }

                    Dictionary<long, IList<FitnessCertificate>> FitnessCertificateList = samp.FitnessCertificateListWithLikeAndExcactSerachCriteria(page - 1, rows, sord, sidx, ExactCriteria, LikeCriteria);
                    if (FitnessCertificateList != null && FitnessCertificateList.First().Key > 0)
                    {

                        if (ExportType == "Excel")
                        {
                            var List = FitnessCertificateList.First().Value.ToList();
                            ExptToXL(List, "FitnessCertificateList", (items => new
                            {
                                Id = items.Id.ToString(),
                                VehicleId = items.VehicleId.ToString(),
                                VehicleNo = items.VehicleNo,
                                FCDate = items.FCDate != null ? items.FCDate.Value.ToString("dd/MM/yyyy") : "",
                                NextFCDate = items.NextFCDate != null ? items.NextFCDate.Value.ToString("dd/MM/yyyy") : "",
                                FCCost = items.FCCost.ToString(),
                                Description = items.Description,
                                FCWorkCarriedAt = items.FCWorkCarriedAt,
                                RTO = items.RTO,
                                Driver = items.Driver,
                                FCertificate = "<a style='color:#034af3;text-decoration:underline' onclick = \"uploaddat1('" + items.Id + "','" + items.FCertificate + "');\"' >" + items.FCertificate + "</a>",
                            }));
                            return new EmptyResult();
                        }
                        else
                        {

                            long totalrecords = FitnessCertificateList.FirstOrDefault().Key;
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var AssLst = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalrecords,

                                rows = (
                                     from items in FitnessCertificateList.First().Value


                                     select new
                                     {
                                         cell = new string[] 
                                         {
                                            items.Id.ToString(),
                                            items.VehicleId.ToString(),
                                            items.VehicleNo,
                                            items.FCDate!=null?items.FCDate.Value.ToString("dd/MM/yyyy"):"",
                                            items.NextFCDate!=null?items.NextFCDate.Value.ToString("dd/MM/yyyy"):"",
                                            items.FCCost.ToString(),
                                            items.Description,
                                            items.FCWorkCarriedAt,
                                            items.RTO,
                                            items.Driver, 
                                             "<a style='color:#034af3;text-decoration:underline' onclick = \"uploaddat1('"+ items.Id + "','"+items.FCertificate+ "');\"' >"+items.FCertificate+"</a>",
                                         }
                                     }).ToList()
                            };
                            return Json(AssLst, JsonRequestBehavior.AllowGet);
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
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        #endregion

        #region Insurance
        public PartialViewResult Insurance()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult AddInsuranceDetails(Insurance ins)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportBC insrepo = new TransportBC();
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    ins.InsuranceDate = DateTime.Parse(Request["InsuranceDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    ins.NextInsuranceDate = DateTime.Parse(Request["NextInsuranceDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    //ins.ValidityFromDate = DateTime.Parse(Request["ValidityFromDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    //ins.ValidityToDate = DateTime.Parse(Request["ValidityToDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    //ins.InsTaxValidUpto = DateTime.Parse(Request["InsTaxValidUpto"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    insrepo.SaveOrUpdateInsuranceDetails(ins);
                    return Json(ins.Id, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        //public ActionResult InsuranceJqGrid(Insurance ins, int rows, string sidx, string sord, int? page = 1)
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //        else
        //        {
        //            TransportBC samp = new TransportBC();
        //            Dictionary<string, object> criteria = new Dictionary<string, object>();
        //            if (ins.VehicleId > 0)
        //                criteria.Add("VehicleId", ins.VehicleId);
        //            Dictionary<long, IList<Insurance>> InsuranceList = samp.GetInsuranceDetails(page - 1, rows, sidx, sord, criteria);
        //            if (InsuranceList != null && InsuranceList.First().Key > 0)
        //            {
        //                long totalrecords = InsuranceList.FirstOrDefault().Key;
        //                int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
        //                var AssLst = new
        //                {
        //                    total = totalPages,
        //                    page = page,
        //                    records = totalrecords,

        //                    rows = (
        //                         from items in InsuranceList.First().Value
        //                         select new
        //                         {
        //                             cell = new string[] 
        //                                 {
        //                                    items.Id.ToString(),
        //                                    items.VehicleId.ToString(),
        //                                    items.VehicleNo,
        //                                    items.InsuranceDate!=null?items.InsuranceDate.Value.ToString("dd/MM/yyyy"):"",
        //                                    items.NextInsuranceDate!=null?items.NextInsuranceDate.Value.ToString("dd/MM/yyyy"):"",
        //                                    items.InsuranceProvider,
        //                                    items.InsuranceConsultantName,
        //                                    items.InsuranceDeclaredValue.ToString(),
        //                                    //items.ValidityFromDate!=null?items.ValidityFromDate.Value.ToString("dd/MM/yyyy"):"",
        //                                    //items.ValidityToDate!=null?items.ValidityToDate.Value.ToString("dd/MM/yyyy"):"",
        //                                    "<a style='color:#034af3;text-decoration:underline' onclick = \"uploaddat('"+ items.Id + "','"+items.ICertificate+ "');\"' >"+items.ICertificate+"</a>",
        //                                 }
        //                         }).ToList()
        //                };
        //                return Json(AssLst, JsonRequestBehavior.AllowGet);
        //            }
        //            else
        //            {
        //                return null;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
        //        throw ex;
        //    }
        //}

        public ActionResult InsuranceJqGrid(Insurance ins, string ExportType, string InsuranceDate, string NextInsuranceDate, string InsuranceDeclaredValue, string ValidityFromDate, string ValidityToDate,
        int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportBC samp = new TransportBC();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<string, object> ExactCriteria = new Dictionary<string, object>();
                    Dictionary<string, object> LikeCriteria = new Dictionary<string, object>();
                    if (ins.VehicleId > 0)
                        criteria.Add("VehicleId", ins.VehicleId);
                    if (!string.IsNullOrEmpty(ins.VehicleNo)) { LikeCriteria.Add("VehicleNo", ins.VehicleNo); }
                    if (!string.IsNullOrEmpty(ins.InsuranceProvider)) { LikeCriteria.Add("InsuranceProvider", ins.InsuranceProvider); }
                    if (!string.IsNullOrEmpty(ins.InsuranceConsultantName)) { LikeCriteria.Add("InsuranceConsultantName", ins.InsuranceConsultantName); }
                    if (!string.IsNullOrEmpty(InsuranceDeclaredValue))
                    {
                        ExactCriteria.Add("InsuranceDeclaredValue", Convert.ToInt64(InsuranceDeclaredValue));
                    }
                    if (!string.IsNullOrEmpty(ins.ICertificate)) { LikeCriteria.Add("ICertificate", ins.ICertificate); }
                    Dictionary<long, IList<Insurance>> InsuranceList = samp.InsuranceListWithLikeAndExcactSerachCriteria(page - 1, rows, sord, sidx, ExactCriteria, LikeCriteria);
                    if (InsuranceList != null && InsuranceList.First().Key > 0)
                    {

                        if (ExportType == "Excel")
                        {
                            var List = InsuranceList.First().Value.ToList();
                            ExptToXL(List, "VehicleBodyMaintenanceReportList", (items => new
                            {
                                Id = items.Id.ToString(),
                                VehicleId = items.VehicleId.ToString(),
                                VehicleNo = items.VehicleNo,
                                InsuranceDate = items.InsuranceDate != null ? items.InsuranceDate.Value.ToString("dd/MM/yyyy") : "",
                                NextInsuranceDate = items.NextInsuranceDate != null ? items.NextInsuranceDate.Value.ToString("dd/MM/yyyy") : "",
                                InsuranceProvider = items.InsuranceProvider,
                                InsuranceConsultantName = items.InsuranceConsultantName,
                                InsuranceDeclaredValue = items.InsuranceDeclaredValue.ToString(),
                                // ValidityFromDate = items.ValidityFromDate != null ? items.ValidityFromDate.Value.ToString("dd/MM/yyyy") : "",
                                // ValidityToDate = items.ValidityToDate != null ? items.ValidityToDate.Value.ToString("dd/MM/yyyy") : "",
                                ICertificate = "<a style='color:#034af3;text-decoration:underline' onclick = \"uploaddat('" + items.Id + "','" + items.ICertificate + "');\"' >" + items.ICertificate + "</a>",
                            }));
                            return new EmptyResult();
                        }
                        else
                        {
                            long totalrecords = InsuranceList.FirstOrDefault().Key;
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var AssLst = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalrecords,

                                rows = (
                                     from items in InsuranceList.First().Value
                                     select new
                                     {
                                         cell = new string[] 
                                         {
                                            items.Id.ToString(),
                                            items.VehicleId.ToString(),
                                            items.VehicleNo,
                                            items.InsuranceDate!=null?items.InsuranceDate.Value.ToString("dd/MM/yyyy"):"",
                                            items.NextInsuranceDate!=null?items.NextInsuranceDate.Value.ToString("dd/MM/yyyy"):"",
                                            items.InsuranceProvider,
                                            items.InsuranceConsultantName,
                                            items.InsuranceDeclaredValue.ToString(),
                                            //items.ValidityFromDate!=null?items.ValidityFromDate.Value.ToString("dd/MM/yyyy"):"",
                                            //items.ValidityToDate!=null?items.ValidityToDate.Value.ToString("dd/MM/yyyy"):"",
                                            //"<a style='color:#034af3;text-decoration:underline' onclick=\"uploaddat('" + items.Id + "','" + items.Type + "','" + items.PreRegNum+  "');\" '>"+items.DocumentName+"</a>",
                                            "<a style='color:#034af3;text-decoration:underline' onclick = \"uploaddat('"+ items.Id + "','"+items.ICertificate+ "');\"' >"+items.ICertificate+"</a>",
                                         }
                                     }).ToList()
                            };
                            return Json(AssLst, JsonRequestBehavior.AllowGet);
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
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        #endregion
        public ActionResult DriverMaster()
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
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult LocationMaster()
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
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult RouteMaster()
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
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        #region LocationMaster

        public ActionResult AddLocationMasterDetails(LocationMaster lm)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportBC lmrepo = new TransportBC();
                    lm.CreatedDate = DateTime.Now;
                    lm.CreatedBy = userId;
                    lm.ModifiedDate = DateTime.Now;
                    lm.ModifiedBy = userId;
                    lmrepo.SaveOrUpdateLocationMasterDetails(lm);
                    lm.LocationId = "Loc-" + lm.Id;
                    lmrepo.SaveOrUpdateLocationMasterDetails(lm);
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult EditLocationMasterDetails(LocationMaster lm)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService TS = new TransportService();
                    if (lm.Id > 0)
                    {
                        LocationMaster Loc = new LocationMaster();
                        Loc = TS.GetLocationMasterDetailsById(lm.Id);
                        Loc.Campus = lm.Campus;
                        Loc.LocationName = lm.LocationName;
                        Loc.ModifiedBy = userId;
                        Loc.ModifiedDate = DateTime.Now;
                        TS.SaveOrUpdateLocationMasterDetails(Loc);
                    }
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult DeleteLocationMasterDetails(LocationMaster lm)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportBC lmrepo = new TransportBC();
                    if (lm.Id > 0)
                    {
                        lmrepo.DeleteLocationMasterDetails(lm);
                    }
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult LocationMasterJqGrid(LocationMaster lm, string ExportType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportBC samp = new TransportBC();
                    UserService us = new UserService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    string[] Criteria = new string[8];
                    if (!string.IsNullOrWhiteSpace(lm.LocationId)) { criteria.Add("LocationId", lm.LocationId); }
                    if (!string.IsNullOrWhiteSpace(lm.Campus)) { criteria.Add("Campus", lm.Campus); }
                    if (!string.IsNullOrWhiteSpace(lm.LocationName)) { criteria.Add("LocationName", lm.LocationName); }
                    if (!string.IsNullOrWhiteSpace(lm.CreatedBy)) { criteria.Add("CreatedBy", lm.CreatedBy); }
                    if (!string.IsNullOrWhiteSpace(lm.ModifiedBy)) { criteria.Add("ModifiedBy", lm.ModifiedBy); }
                    Dictionary<long, IList<LocationMaster>> LocationMasterList = samp.GetLocationMasterDetails(page - 1, rows, sidx, sord, criteria);
                    if (LocationMasterList != null && LocationMasterList.First().Key > 0)
                    {
                        if (ExportType == "Excel")
                        {
                            var List = LocationMasterList.First().Value.ToList();
                            ExptToXL(List, "LocationMasterList", (items => new
                            {
                                items.LocationId,
                                items.Campus,
                                items.LocationName,
                                CreatedDate = items.CreatedDate != null ? items.CreatedDate.ToString() : null,
                                CreatedBy = items.CreatedBy != null ? us.GetUserNameByUserId(items.CreatedBy) : "",
                                ModifiedDate = items.ModifiedDate != null ? items.ModifiedDate.ToString() : null,
                                items.ModifiedBy
                            }));
                            return new EmptyResult();
                        }
                        else
                        {
                            if (LocationMasterList != null && LocationMasterList.First().Key > 0)
                            {
                                long totalrecords = LocationMasterList.FirstOrDefault().Key;
                                int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                                var AssLst = new
                                {
                                    total = totalPages,
                                    page = page,
                                    records = totalrecords,

                                    rows = (
                                         from items in LocationMasterList.First().Value
                                         select new
                                         {
                                             cell = new string[] 
                                         {
                                            items.Id.ToString(),
                                            items.LocationId,
                                            items.Campus,
                                            items.LocationName,
                                             items.CreatedDate!=null? items.CreatedDate.ToString("dd/MM/yyyy"):null,
                                            items.CreatedBy!=null?us.GetUserNameByUserId(items.CreatedBy):"",
                                            items.ModifiedDate!=null? items.ModifiedDate.ToString("dd/MM/yyyy"):null,
                                            items.ModifiedBy,
                                            items.CreatedBy,
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
                    }

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

        #endregion

        #region DriverMaster
        [ValidateInput(false)]
        public ActionResult AddDriverMasterDetails(DriverMaster dm, HttpPostedFileBase file)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    //if (!string.IsNullOrWhiteSpace(Request["Dob"]))
                    //{
                    //    dm.Dob = DateTime.Parse(Request["Dob"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    //}
                    if (!string.IsNullOrWhiteSpace(Request["LicenseValDate"]))
                    {
                        dm.LicenseValDate = DateTime.Parse(Request["LicenseValDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    }
                    if (!string.IsNullOrWhiteSpace(Request["NonTraLicenseValDate"]))
                    {
                        dm.NonTraLicenseValDate = DateTime.Parse(Request["NonTraLicenseValDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    }
                    dm.CreatedDate = DateTime.Now;
                    dm.CreatedBy = userId;
                    dm.ModifiedDate = DateTime.Now;
                    dm.ModifiedBy = userId;
                    TransportService ts = new TransportService();
                    ts.CreateOrUpdateDriverMaster(dm);
                    return Json(dm.Id, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult EditDriverMasterDetails(DriverMaster lm, HttpPostedFileBase file)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    if (lm.Id > 0)
                    {
                        DriverMaster Dr = new DriverMaster();
                        Dr = ts.GetDriverMasterDetailsById(lm.Id);
                        Dr.Campus = lm.Campus;
                        Dr.Name = lm.Name;
                        IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                        //if (lm.Dob == null)
                        //{
                        //    if (!string.IsNullOrWhiteSpace(Request["Dob"]))
                        //    {
                        //        Dr.Dob = DateTime.Parse(Request["Dob"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        //    }
                        //}
                        Dr.Dob = lm.Dob;
                        Dr.Age = lm.Age;
                        Dr.Gender = lm.Gender;
                        Dr.ContactNo = lm.ContactNo;
                        Dr.LicenseNo = lm.LicenseNo;
                        Dr.DriverIdNo = lm.DriverIdNo;
                        Dr.BatchNo = lm.BatchNo;
                        if (lm.LicenseValDate == null)
                        {
                            if (!string.IsNullOrWhiteSpace(Request["LicenseValDate"]))
                            {
                                Dr.LicenseValDate = DateTime.Parse(Request["LicenseValDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                            }
                        }
                        if (lm.NonTraLicenseValDate == null)
                        {
                            if (!string.IsNullOrWhiteSpace(Request["NonTraLicenseValDate"]))
                            {
                                Dr.NonTraLicenseValDate = DateTime.Parse(Request["NonTraLicenseValDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                            }
                        }
                        Dr.ModifiedBy = userId;
                        Dr.ModifiedDate = DateTime.Now;
                        ts.CreateOrUpdateDriverMaster(Dr);
                    }
                    return Json(lm.Id, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult DeleteDriverMasterDetails(DriverMaster lm)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportBC lmrepo = new TransportBC();
                    if (lm.Id > 0)
                    {
                        lmrepo.DeleteDriverMasterDetails(lm);
                    }
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult DriverMasterJqGrid(DriverMaster dm, string Dob, string LicenseValDate, string NonTraLicenseValDate, string Status, string ExportType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportBC samp = new TransportBC();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();

                    if (!string.IsNullOrEmpty(Dob))
                    {
                        DateTime[] DateOfBirth = ConvertStringToDateTime(Dob);
                        criteria.Add("Dob", DateOfBirth);
                    }
                    if (!string.IsNullOrEmpty(LicenseValDate))
                    {
                        DateTime[] LicenseValidDate = ConvertStringToDateTime(LicenseValDate);
                        criteria.Add("LicenseValDate", LicenseValidDate);
                    }
                    if (!string.IsNullOrEmpty(NonTraLicenseValDate))
                    {
                        DateTime[] NonTraLicenseValidDate = ConvertStringToDateTime(NonTraLicenseValDate);
                        criteria.Add("NonTraLicenseValDate", NonTraLicenseValidDate);
                    }
                    if (!string.IsNullOrEmpty(dm.Name)) { criteria.Add("Name", dm.Name); }
                    if (!string.IsNullOrEmpty(dm.Campus)) { criteria.Add("Campus", dm.Campus); }
                    if (dm.Age > 0) criteria.Add("Age", dm.Age);
                    if (!string.IsNullOrEmpty(dm.Gender)) { criteria.Add("Gender", dm.Gender); }
                    if (!string.IsNullOrEmpty(dm.BatchNo)) { criteria.Add("BatchNo", dm.BatchNo); }
                    if (!string.IsNullOrEmpty(dm.ContactNo)) { criteria.Add("ContactNo", dm.ContactNo); }
                    if (!string.IsNullOrEmpty(dm.PresentAddress)) { criteria.Add("PresentAddress", dm.PresentAddress); }
                    if (!string.IsNullOrEmpty(dm.PermanentAddress)) { criteria.Add("PermanentAddress", dm.PermanentAddress); }
                    if (!string.IsNullOrEmpty(dm.DriverIdNo)) { criteria.Add("DriverIdNo", dm.DriverIdNo); }
                    if (!string.IsNullOrEmpty(dm.LicenseNo)) { criteria.Add("LicenseNo", dm.LicenseNo); }
                    if (!string.IsNullOrEmpty(Status))
                    {
                        if (Status == "Active")
                            criteria.Add("Status", true);
                        if (Status == "InActive")
                            criteria.Add("Status", false);
                    }
                    if (!string.IsNullOrEmpty(dm.CreatedBy)) { criteria.Add("CreatedBy", dm.CreatedBy); }
                    if (!string.IsNullOrEmpty(dm.ModifiedBy)) { criteria.Add("ModifiedBy", dm.ModifiedBy); }
                    Dictionary<long, IList<DriverMaster>> DriverMasterList = samp.GetDriverMasterDetails(page - 1, rows, sidx, sord, criteria);
                    if (DriverMasterList != null && DriverMasterList.First().Key > 0)
                    {
                        UserService us = new UserService();
                        if (ExportType == "Excel")
                        {
                            var List = DriverMasterList.First().Value.ToList();
                            ExptToXL(List, "DriverMasterList", (items => new
                            {
                                items.Campus,
                                items.Name,
                                items.Dob,
                                items.Age,
                                items.Gender,
                                items.ContactNo,
                                items.LicenseNo,
                                items.DriverIdNo,
                                items.BatchNo,
                                items.LicenseValDate,
                                items.NonTraLicenseValDate,
                                items.PresentAddress,
                                items.PermanentAddress,
                                items.CreatedDate,
                                CreatedBy = items.CreatedBy != null ? us.GetUserNameByUserId(items.CreatedBy) : "",
                                items.ModifiedDate,
                                items.ModifiedBy,
                                items.Status

                            }));
                            return new EmptyResult();
                        }
                        else
                        {
                            if (DriverMasterList != null && DriverMasterList.First().Key > 0)
                            {

                                long totalrecords = DriverMasterList.FirstOrDefault().Key;
                                int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                                var AssLst = new
                                {
                                    total = totalPages,
                                    page = page,
                                    records = totalrecords,

                                    rows = (
                                         from items in DriverMasterList.First().Value
                                         select new
                                         {
                                             cell = new string[] 
                                         {
                                            items.Campus,
                                            items.Name,
                                            items.Dob,
                                            items.Age.ToString(),
                                            items.Gender,
                                            items.ContactNo,
                                            items.LicenseNo,
                                            items.DriverIdNo,
                                            items.BatchNo,
                                            items.LicenseValDate!=null?items.LicenseValDate.Value.ToString("dd/MM/yyyy"):"",
                                            items.NonTraLicenseValDate!=null?items.NonTraLicenseValDate.Value.ToString("dd/MM/yyyy"):"",
                                           // items.DriverPhoto,
                                            "<a style='color:#034af3;text-decoration:underline' onclick = \"uploaddat('"+ items.Id + "','"+items.DriverPhoto+ "');\"' >"+items.DriverPhoto+"</a>",
                                            items.Status.ToString(),
                                            items.PresentAddress,
                                            items.PermanentAddress,
                                            items.CreatedDate!=null?items.CreatedDate.ToString("dd/MM/yyyy hh:mm:ss tt"):"",
                                            items.CreatedBy!=null?us.GetUserNameByUserId(items.CreatedBy):"",
                                            items.ModifiedDate!=null?items.ModifiedDate.ToString("dd/MM/yyyy hh:mm:ss tt"):"",
                                            items.ModifiedBy,
                                            items.Id.ToString(),
                                            items.CreatedBy
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
                    }
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
        #endregion

        #region RouteMaster
        public ActionResult AddRouteMasterDetails(RouteMaster rm)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    rm.CreatedDate = DateTime.Now;
                    rm.CreatedBy = userId;
                    rm.ModifiedDate = DateTime.Now;
                    rm.ModifiedBy = userId;
                    TransportBC rmrepo = new TransportBC();
                    rmrepo.SaveOrUpdateRouteMasterDetails(rm);
                    rm.RouteId = "Route-" + rm.Id;
                    rmrepo.SaveOrUpdateRouteMasterDetails(rm);
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        //Method replaced by Krishna done by dhana
        //public ActionResult EditRouteMasterDetails(RouteMaster Rt)
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //        else
        //        {
        //            TransportService TS = new TransportService();
        //            if (Rt.Id > 0)
        //            {
        //                RouteMaster Route = new RouteMaster();
        //                Route = TS.GetRouteMasterDetailsById(Rt.Id);
        //                Route.RouteId = Rt.RouteId;
        //                Route.RouteNo = Rt.RouteNo;
        //                Route.Campus = Rt.Campus;
        //                Route.Source = Rt.Source;
        //                Route.Destination = Rt.Destination;
        //                Route.Via = Rt.Via;
        //                Route.Distance = Rt.Distance;
        //                Route.District = Rt.District;
        //                Route.State = Rt.State;
        //                Route.Country = Rt.Country;
        //                Route.ModifiedBy = userId;
        //                Route.ModifiedDate = DateTime.Now;
        //                TS.SaveOrUpdateRouteMasterDetails(Route);
        //            }
        //            return Json(null, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
        //        throw ex;
        //    }
        //}

        public ActionResult DeleteRouteMasterDetails(RouteMaster lm)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportBC lmrepo = new TransportBC();
                    if (lm.Id > 0)
                    {
                        lmrepo.DeleteRouteMasterDetails(lm);
                    }
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        #region RouteMasterEdit
        public ActionResult EditRouteMasterDetails(RouteMaster Rt)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService TS = new TransportService();

                    if (Rt.Id > 0)
                    {
                        RouteMaster RouteCheck = new RouteMaster();
                        if (!string.IsNullOrWhiteSpace(Rt.Campus) && !string.IsNullOrWhiteSpace(Rt.RouteNo))
                        {
                            RouteCheck = TS.GetRouteDetails(Rt.Campus, Rt.RouteNo);
                            if (RouteCheck != null)
                            {
                                if (RouteCheck.Id != Rt.Id)
                                {
                                    var script = @"ErrMsg(""Already Exist"");";
                                    return JavaScript(script);
                                }
                            }
                        }
                        RouteMaster Route = new RouteMaster();
                        Route = TS.GetRouteMasterDetailsById(Rt.Id);
                        //--Adding data for RouteMaster Log
                        RouteMasterLog objRouteMasterLog = new RouteMasterLog();
                        objRouteMasterLog.Id = Route.Id;
                        objRouteMasterLog.RouteId = Route.RouteId;
                        objRouteMasterLog.RouteNo = Route.RouteNo;
                        objRouteMasterLog.VehicleNo = Route.VehicleNo;
                        objRouteMasterLog.Campus = Route.Campus;
                        objRouteMasterLog.Source = Route.Source;
                        objRouteMasterLog.Destination = Route.Destination;
                        objRouteMasterLog.Via = Route.Via;
                        objRouteMasterLog.ToRouteNo = Rt.RouteNo;
                        objRouteMasterLog.ToVehicleNo = Rt.VehicleNo;
                        objRouteMasterLog.ToCampus = Rt.Campus;
                        objRouteMasterLog.ToSource = Rt.Source;
                        objRouteMasterLog.ToDestination = Rt.Destination;
                        objRouteMasterLog.ToVia = Rt.Via;
                        objRouteMasterLog.CreatedBy = userId;
                        objRouteMasterLog.CreatedDate = DateTime.Now;
                        //--Adding data for Update
                        Route.RouteNo = Rt.RouteNo;
                        Route.Campus = Rt.Campus;
                        Route.Source = Rt.Source;
                        Route.Destination = Rt.Destination;
                        Route.Via = Rt.Via;
                        Route.ModifiedBy = userId;
                        Route.ModifiedDate = DateTime.Now;
                        long retID = TS.SaveOrUpdateRouteMasterDetails(Route);
                        if (retID > 0)
                            ts.SaveOrUpdateRouteMasterLog(objRouteMasterLog);
                    }
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult RouteMasterJqGrid(RouteMaster rm, string ExportType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportBC samp = new TransportBC();
                    TransportService ts = new TransportService();
                    Dictionary<string, object> exctcriteria = new Dictionary<string, object>();
                    Dictionary<string, object> likeCriteria = new Dictionary<string, object>();
                    if (!string.IsNullOrWhiteSpace(rm.RouteId)) { likeCriteria.Add("RouteId", rm.RouteId); }
                    if (!string.IsNullOrWhiteSpace(rm.RouteNo)) { likeCriteria.Add("RouteNo", rm.RouteNo); }
                    if (!string.IsNullOrWhiteSpace(rm.Campus)) { exctcriteria.Add("Campus", rm.Campus); }
                    if (!string.IsNullOrWhiteSpace(rm.Source)) { likeCriteria.Add("Source", rm.Source); }
                    if (!string.IsNullOrWhiteSpace(rm.Destination)) { likeCriteria.Add("Destination", rm.Destination); }
                    if (!string.IsNullOrWhiteSpace(rm.Via)) { likeCriteria.Add("Via", rm.Via); }
                    if (!string.IsNullOrWhiteSpace(rm.VehicleNo)) { likeCriteria.Add("VehicleNo", rm.VehicleNo); }
                    //if (rm.Distance > 0) { criteria.Add("Distance", Convert.ToDecimal(rm.Distance)); }
                    //if (!string.IsNullOrWhiteSpace(rm.District)) { criteria.Add("District", rm.District); }
                    //if (!string.IsNullOrWhiteSpace(rm.State)) { criteria.Add("State", rm.State); }
                    //if (!string.IsNullOrWhiteSpace(rm.Country)) { criteria.Add("Country", rm.Country); }
                    sord = sord == "desc" ? "Desc" : "Asc";
                    Dictionary<long, IList<RouteMasterWithIMEINumber_vw>> RouteMasterList = ts.GetRouteMasterWithIMEINumber_vwList(page - 1, rows, sord, sidx, exctcriteria, likeCriteria);
                    if (RouteMasterList != null && RouteMasterList.First().Key > 0)
                    {
                        if (ExportType == "Excel")
                        {
                            var List = RouteMasterList.First().Value.ToList();
                            ExptToXL(List, "RouteMasterList", (items => new
                            {
                                items.Campus,
                                items.VehicleNo,
                                items.RouteNo,
                                items.Source,
                                items.Destination,
                                items.Via,
                                items.IMEINmber,
                                //items.Distance,
                                //items.District,
                                //items.State,
                                //items.Country,
                                items.CreatedDate,
                                items.CreatedBy,
                                items.ModifiedDate,
                                items.ModifiedBy
                            }));
                            return new EmptyResult();
                        }
                        else
                        {
                            if (RouteMasterList != null && RouteMasterList.First().Key > 0)
                            {
                                long totalrecords = RouteMasterList.FirstOrDefault().Key;
                                int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                                var AssLst = new
                                {
                                    total = totalPages,
                                    page = page,
                                    records = totalrecords,

                                    rows = (
                                         from items in RouteMasterList.First().Value
                                         select new
                                         {
                                             cell = new string[] 
                                         {
                                            items.Id.ToString(),
                                            items.Campus,
                                            items.VehicleNo,
                                            String.Format(@"<a style='color:#034af3;text-decoration:underline' href= '/Transport/RouteConfiguration?Id="+items.Id+"'  >{0}</a>",items.RouteId),
                                            items.RouteNo,
                                            items.Source,
                                            items.Destination,
                                            items.Via,
                                            //items.Distance.ToString(),
                                            //items.District,
                                            //items.State,
                                            //items.Country,
                                            items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt"):"",
                                            items.CreatedBy,
                                            items.ModifiedDate!=null?items.ModifiedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt"):"",
                                            items.ModifiedBy,
                                            items.IMEINmber
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
                    }
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
        #endregion

        //Method replaced by Krishna done by dhana

        //public ActionResult RouteMasterJqGrid(RouteMaster rm, string ExportType, int rows, string sidx, string sord, int? page = 1)
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //        else
        //        {
        //            TransportBC samp = new TransportBC();
        //            Dictionary<string, object> criteria = new Dictionary<string, object>();
        //            if (!string.IsNullOrWhiteSpace(rm.RouteId)) { criteria.Add("RouteId", rm.RouteId); }
        //            if (!string.IsNullOrWhiteSpace(rm.RouteNo)) { criteria.Add("RouteNo", rm.RouteNo); }
        //            if (!string.IsNullOrWhiteSpace(rm.Campus)) { criteria.Add("Campus", rm.Campus); }
        //            if (!string.IsNullOrWhiteSpace(rm.Source)) { criteria.Add("Source", rm.Source); }
        //            if (!string.IsNullOrWhiteSpace(rm.Destination)) { criteria.Add("Destination", rm.Destination); }
        //            if (!string.IsNullOrWhiteSpace(rm.Via)) { criteria.Add("Via", rm.Via); }
        //            //if (rm.Distance > 0) { criteria.Add("Distance", Convert.ToDecimal(rm.Distance)); }
        //            //if (!string.IsNullOrWhiteSpace(rm.District)) { criteria.Add("District", rm.District); }
        //            //if (!string.IsNullOrWhiteSpace(rm.State)) { criteria.Add("State", rm.State); }
        //            //if (!string.IsNullOrWhiteSpace(rm.Country)) { criteria.Add("Country", rm.Country); }
        //            //if (!string.IsNullOrWhiteSpace(rm.CreatedBy)) { criteria.Add("CreatedBy", rm.CreatedBy); }
        //            //if (!string.IsNullOrWhiteSpace(rm.ModifiedBy)) { criteria.Add("ModifiedBy", rm.ModifiedBy); }
        //            Dictionary<long, IList<RouteMaster>> RouteMasterList = samp.GetRouteMasterDetails(page - 1, rows, sidx, sord, criteria);
        //            if (RouteMasterList != null && RouteMasterList.First().Key > 0)
        //            {
        //                if (ExportType == "Excel")
        //                {
        //                    var List = RouteMasterList.First().Value.ToList();
        //                    ExptToXL(List, "RouteMasterList", (items => new
        //                    {
        //                        items.RouteId,
        //                        items.RouteNo,
        //                        items.Campus,
        //                        items.Source,
        //                        items.Destination,
        //                        items.Via,
        //                        //items.Distance,
        //                        //items.District,
        //                        //items.State,
        //                        //items.Country,
        //                        items.CreatedDate,
        //                        items.CreatedBy,
        //                        items.ModifiedDate,
        //                        items.ModifiedBy
        //                    }));
        //                    return new EmptyResult();
        //                }
        //                else
        //                {
        //                    if (RouteMasterList != null && RouteMasterList.First().Key > 0)
        //                    {
        //                        long totalrecords = RouteMasterList.FirstOrDefault().Key;
        //                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
        //                        var AssLst = new
        //                        {
        //                            total = totalPages,
        //                            page = page,
        //                            records = totalrecords,

        //                            rows = (
        //                                 from items in RouteMasterList.First().Value
        //                                 select new
        //                                 {
        //                                     cell = new string[] 
        //                                 {
        //                                    items.Id.ToString(),
        //                                    //items.RouteId,
        //                                    String.Format(@"<a style='color:#034af3;text-decoration:underline' href= '/Transport/RouteConfiguration?Id="+items.Id+"'  >{0}</a>",items.RouteId),
        //                                    items.RouteNo,
        //                                    items.Campus,
        //                                    items.Source,
        //                                    items.Destination,
        //                                    items.Via,
        //                                    //items.Distance.ToString(),
        //                                    //items.District,
        //                                    //items.State,
        //                                    //items.Country,
        //                                    items.CreatedDate!=null?items.CreatedDate.ToString("dd/MM/yyyy hh:mm:ss tt"):"",
        //                                    items.CreatedBy,
        //                                    items.ModifiedDate!=null?items.ModifiedDate.ToString("dd/MM/yyyy hh:mm:ss tt"):"",
        //                                    items.ModifiedBy
        //                                 }
        //                                 }).ToList()
        //                        };
        //                        return Json(AssLst, JsonRequestBehavior.AllowGet);
        //                    }
        //                    else
        //                    {
        //                        var AssLst = new { rows = (new { cell = new string[] { } }) };
        //                        return Json(AssLst, JsonRequestBehavior.AllowGet);
        //                    }
        //                }
        //            }
        //            var Empty = new { rows = (new { cell = new string[] { } }) };
        //            return Json(Empty, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
        //        throw ex;
        //    }
        //}
        #endregion

        public ActionResult Locationddl()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    Dictionary<string, string> Location = new Dictionary<string, string>();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();

                    Dictionary<long, IList<LocationMaster>> branch = ts.GetLocationMasterDetails(0, 9999, null, null, criteria);
                    criteria.Clear();
                    foreach (LocationMaster loc in branch.First().Value)
                    {
                        if (!string.IsNullOrWhiteSpace(loc.LocationName) && !Location.ContainsKey(loc.LocationName))
                            Location.Add(loc.LocationName, loc.LocationName);
                    }
                    return PartialView("Dropdown", Location);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult Driverddl()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    Dictionary<string, string> DriverNames = new Dictionary<string, string>();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("Status", true);
                    Dictionary<long, IList<DriverMaster>> DriverList = ts.GetDriverMasterDetails(0, 9999, null, null, criteria);
                    foreach (DriverMaster dri in DriverList.First().Value)
                    {
                        if (!string.IsNullOrWhiteSpace(dri.Name) && !DriverNames.ContainsKey(dri.Name))
                            DriverNames.Add(dri.Name, dri.Name);
                    }
                    return PartialView("Dropdown", DriverNames.Distinct());
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult DistanceCoveredBulkEntry(int? Id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    DistanceCoveredDetails dcd = new DistanceCoveredDetails();
                    if (Id > 0)
                    {
                        dcd = ts.GetDistanceCoveredDetailsById(Convert.ToInt32(Id));
                        return View(dcd);
                    }
                    else
                    {
                        UserService us = new UserService();
                        Dictionary<string, object> Criteria = new Dictionary<string, object>();
                        Criteria.Add("UserId", userId);
                        Criteria.Add("AppCode", "TRA");
                        Dictionary<long, IList<UserAppRole>> UserAppRoleList = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, Criteria);
                        var UserDetails = (from u in UserAppRoleList.First().Value
                                           where u.UserId == userId
                                           select new { u.RoleName, u.BranchCode }).ToArray();
                        dcd.Campus = UserDetails[0].BranchCode;
                        dcd.UserRole = UserDetails[0].RoleName;
                        dcd.Status = "Open";
                        dcd.ProcessedBy = userId;
                        return View(dcd);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult DistanceCoveredBulkEntry(DistanceCoveredDetails dcd)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    dcd.CreatedDate = DateTime.Now;
                    ts.CreateOrUpdateDistanceCoveredDetails(dcd);
                    dcd.RefNo = "Ref-" + dcd.Id;
                    ts.CreateOrUpdateDistanceCoveredDetails(dcd);
                    return RedirectToAction("DistanceCoveredBulkEntry", new { Id = dcd.Id });
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult DistanceCoveredBulkEntryList()
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
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult DistanceCoveredBulkEntryJqGrid(int? Id, string ExportType, string status, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    if (!string.IsNullOrWhiteSpace(status)) { criteria.Add("Status", status); }
                    if (Id > 0) { criteria.Add("Id", Id); }
                    Dictionary<long, IList<DistanceCoveredDetails>> RouteMasterList = ts.GetDistanceCoveredDetailsListWithsearchCriteria(page - 1, rows, sidx, sord, criteria);
                    if (RouteMasterList != null && RouteMasterList.First().Key > 0)
                    {
                        long totalrecords = RouteMasterList.FirstOrDefault().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var AssLst = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (
                                 from items in RouteMasterList.First().Value
                                 select new
                                 {
                                     cell = new string[] 
                                         {
                                            items.Id.ToString(),
                                            items.RefNo,
                                            items.Campus,
                                            items.ProcessedBy,
                                            items.CreatedDate!=null? items.CreatedDate.ToString("dd/MM/yyyy hh:mm:ss tt"):null,
                                            //items.Description,
                                            items.UserRole,
                                            items.Status,
                                         }
                                 }).ToList()
                        };
                        return Json(AssLst, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult AddVehicleDistanceCoveredList(IList<VehicleDistanceCovered> DLst)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    if (DLst != null)
                    {
                        foreach (var d in DLst)
                        {
                            DLst.First().CreatedBy = userId;
                            DLst.First().CreatedDate = DateTime.Now;
                        }
                        ts.CreateOrUpdateVehicleDistanceCoveredList(DLst);
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            { return ThrowJSONErrorNew(ex, "TransportMgmtPolicy"); }
        }

        public ActionResult DistanceCoveredListBulkEntryJqGrid(VehicleDistanceCovered vdc, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    sord = sord == "desc" ? "Desc" : "Asc";
                    if (vdc.RefId == 0)
                    {
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                    if (vdc.RefId > 0)
                        criteria.Add("RefId", vdc.RefId);
                    Dictionary<long, IList<VehicleDistanceCovered>> VehicleDistanceCovered = ts.GetVehicleDistanceCoveredListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    if (VehicleDistanceCovered != null && VehicleDistanceCovered.Count > 0)
                    {
                        long totalrecords = VehicleDistanceCovered.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var VehicleDistanceList = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in VehicleDistanceCovered.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                               items.Id.ToString(),items.RefId.ToString(), items.VehicleId.ToString(),items.Type, items.VehicleNo,items.Route, 
                               items.Source,items.Destination, items.DistanceCovered.ToString(),
                              // items.TripDate!=null? items.TripDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt"):"",
                               items.DriverName
                            }
                                    })
                        };
                        return Json(VehicleDistanceList, JsonRequestBehavior.AllowGet);

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

        public ActionResult DeleteDistanceCoveredId(int Id)
        {
            try
            {
                string userId = ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    VehicleDistanceCovered vdc = new VehicleDistanceCovered();
                    if (Id > 0)
                        vdc = ts.GetVehicleDistanceCoveredById(Id);
                    if (vdc != null)
                        ts.DeleteVehicleDistanceCoveredbyId(vdc);
                    return Json(Id, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult DeleteDistanceCoveredList(string Id)
        {
            try
            {
                string userId = ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (!string.IsNullOrWhiteSpace(Id))
                    {
                        string[] stringArray = Id.Split(',');
                        int[] ints = stringArray.Select(x => int.Parse(x)).ToArray();
                        TransportService ts = new TransportService();
                        Dictionary<string, object> criteria = new Dictionary<string, object>();
                        criteria.Add("Id", ints);
                        Dictionary<long, IList<VehicleDistanceCovered>> DistanceCoveredList = ts.GetVehicleDistanceCoveredListWithPagingAndCriteria(0, 100, string.Empty, string.Empty, criteria);
                        if (DistanceCoveredList != null && DistanceCoveredList.FirstOrDefault().Value != null && DistanceCoveredList.FirstOrDefault().Value.Count > 0)
                        {
                            IList<VehicleDistanceCovered> DLst = DistanceCoveredList.FirstOrDefault().Value;
                            ts.DeleteVehicleDistanceCoveredList(DLst);
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
        public ActionResult CompleteVehicleDistanceCovered(DistanceCoveredDetails vdc)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    vdc = ts.GetDistanceCoveredDetailsById(Convert.ToInt32(vdc.Id));
                    vdc.Status = "Completed";
                    ts.CreateOrUpdateDistanceCoveredDetails(vdc);
                    return Json(vdc.RefNo, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult Routeddl()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    Dictionary<string, string> Route = new Dictionary<string, string>();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();

                    Dictionary<long, IList<RouteMaster>> RouteList = ts.GetRouteMasterDetails(0, 9999, null, null, criteria);
                    criteria.Clear();
                    foreach (RouteMaster rou in RouteList.First().Value)
                    {
                        if (rou.RouteNo != null)
                            Route.Add(rou.RouteNo, rou.RouteNo);
                    }
                    return PartialView("Dropdown", Route);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult Vehicleddl()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    Dictionary<long, string> VehicleList = new Dictionary<long, string>();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("IsActive", true);
                    Dictionary<long, IList<VehicleSubTypeMaster>> RouteList = ts.GetVehicleSubTypeMasterListWithPagingAndCriteria(0, 9999, null, null, criteria);
                    criteria.Clear();
                    foreach (VehicleSubTypeMaster veh in RouteList.First().Value)
                    {
                        VehicleList.Add(veh.Id, veh.Type + "(" + veh.VehicleNo + ")");
                    }
                    return PartialView("Select", VehicleList);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult GoogleMap()
        {
            return View();
        }

        public ActionResult Maintenance1()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    return PartialView();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult VehicleMaintenance()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    return PartialView();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult VehicleACMaintenance()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    return PartialView();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult AddVehicleMaintenance(VehicleMaintenance vm)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService tsk = new TransportService();
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    if (vm.VehicleMaintenanceType == "Breakdown")
                    {
                        if (Request["VehicleDateOfBreakdown"] != null)
                        {
                            vm.VehicleDateOfBreakdown = DateTime.Parse(Request["VehicleDateOfBreakdown"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        }
                    }
                    vm.VehiclePlannedDateOfService = DateTime.Parse(Request["VehiclePlannedDateOfService"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    vm.VehicleActualDateOfService = DateTime.Parse(Request["VehicleActualDateOfService"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    vm.CreatedDate = DateTime.Now;
                    vm.CreatedBy = userId;
                    tsk.CreateOrUpdateVehicleMaintenance(vm);
                    return Json(vm.Id, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult AddVehicleACMaintenance(VehicleACMaintenance Vam)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService tsk = new TransportService();
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    if (Vam.ACMaintenanceType == "Breakdown")
                    {
                        if (Request["ACDateOfBreakdown"] != null)
                        {
                            Vam.ACDateOfBreakdown = DateTime.Parse(Request["ACDateOfBreakdown"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        }
                    }
                    Vam.ACPlannedDateOfService = DateTime.Parse(Request["ACPlannedDateOfService"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    Vam.ACActualDateOfService = DateTime.Parse(Request["ACActualDateOfService"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    Vam.CreatedDate = DateTime.Now;
                    Vam.CreatedBy = userId;
                    tsk.CreateOrUpdateVehicleACMaintenance(Vam);
                    return Json(Vam.Id, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        //public ActionResult VehicleMaintenanceJqGrid(VehicleMaintenance vm, int rows, string sidx, string sord, int? page = 1)
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //        else
        //        {
        //            TransportService ts = new TransportService();
        //            Dictionary<string, object> criteria = new Dictionary<string, object>();
        //            sord = sord == "desc" ? "Desc" : "Asc";
        //            if (vm.VehicleId == 0)
        //            {
        //                return Json(null, JsonRequestBehavior.AllowGet);
        //            }
        //            else
        //            {
        //                criteria.Add("VehicleId", vm.VehicleId);
        //                Dictionary<long, IList<VehicleMaintenance>> VehicleMaintenance = ts.GetVehicleMaintenanceListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
        //                if (VehicleMaintenance != null && VehicleMaintenance.Count > 0)
        //                {
        //                    long totalrecords = VehicleMaintenance.First().Key;
        //                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
        //                    var VehicleMaintenanceList = new
        //                    {
        //                        total = totalPages,
        //                        page = page,
        //                        records = totalrecords,
        //                        rows = (from items in VehicleMaintenance.First().Value
        //                                select new
        //                                {
        //                                    i = 2,
        //                                    cell = new string[] {
        //                       items.Id.ToString(), items.VehicleId.ToString(), items.VehicleNo,items.VehicleMaintenanceType,
        //                       items.VehicleDateOfBreakdown!=null?items.VehicleDateOfBreakdown.Value.ToString("dd/MM/yyyy"):"",
        //                       items.VehicleBreakdownLocation, 
        //                       items.VehiclePlannedDateOfService!=null?items.VehiclePlannedDateOfService.Value.ToString("dd/MM/yyyy"):"",
        //                       items.VehicleActualDateOfService!=null?items.VehicleActualDateOfService.Value.ToString("dd/MM/yyyy"):"",
        //                       items.VehicleServiceProvider,
        //                       items.VehicleSeviceCost.ToString(),items.VehicleServiceBillNo,items.VehicleSparePartsUsed,
        //                        "<a style='color:#034af3;text-decoration:underline' onclick = \"ShowVMsparePartsUsed('"+ items.Id + "','"+items.VM_SparePartsUsedfile+ "');\"' >"+items.VM_SparePartsUsedfile+"</a>",
        //                       items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt"):"",
        //                       items.CreatedBy
        //                    }
        //                                })
        //                    };
        //                    return Json(VehicleMaintenanceList, JsonRequestBehavior.AllowGet);
        //                }
        //                return null;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
        //        throw ex;
        //    }
        //}

        public ActionResult VehicleMaintenanceJqGrid(VehicleMaintenance vm, string ExportType, string VehicleId, string VehicleDateOfBreakdown, string VehiclePlannedDateOfService, string VehicleActualDateOfService,
         string VehicleSeviceCost, string CreatedDate, string VM_SparePartsUsedfile, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();

                    sord = sord == "desc" ? "Desc" : "Asc";
                    if (ExportType != "Excel" && vm.VehicleId == 0)
                    {
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        Dictionary<string, object> ExactCriteria = new Dictionary<string, object>();
                        Dictionary<string, object> LikeCriteria = new Dictionary<string, object>();

                        if (!string.IsNullOrEmpty(VehicleSeviceCost))
                        {
                            decimal ServiceCost = Convert.ToDecimal(VehicleSeviceCost);
                            ExactCriteria.Add("VehicleSeviceCost", ServiceCost);
                        }
                        if (!string.IsNullOrEmpty(VehicleId))
                        {
                            int VehcleId = Convert.ToInt32(VehicleId);
                            ExactCriteria.Add("VehicleId", VehcleId);
                        }
                        if (!string.IsNullOrEmpty(vm.VehicleNo)) { LikeCriteria.Add("VehicleNo", vm.VehicleNo); }
                        if (!string.IsNullOrEmpty(vm.VehicleMaintenanceType)) { LikeCriteria.Add("VehicleMaintenanceType", vm.VehicleMaintenanceType); }
                        if (!string.IsNullOrEmpty(vm.VehicleBreakdownLocation)) { LikeCriteria.Add("VehicleBreakdownLocation", vm.VehicleBreakdownLocation); }
                        if (!string.IsNullOrEmpty(vm.VehicleMaintenanceDescription)) { LikeCriteria.Add("VehicleMaintenanceDescription", vm.VehicleMaintenanceDescription); }
                        if (!string.IsNullOrEmpty(vm.VehicleServiceProvider)) { LikeCriteria.Add("VehicleServiceProvider", vm.VehicleServiceProvider); }
                        if (!string.IsNullOrEmpty(vm.VehicleServiceBillNo)) { LikeCriteria.Add("VehicleServiceBillNo", vm.VehicleServiceBillNo); }
                        if (!string.IsNullOrEmpty(vm.CreatedBy)) { LikeCriteria.Add("CreatedBy", vm.CreatedBy); }
                        if (!string.IsNullOrEmpty(vm.VehicleSparePartsUsed)) { LikeCriteria.Add("VehicleSparePartsUsed", vm.VehicleSparePartsUsed); }
                        if (!string.IsNullOrEmpty(VM_SparePartsUsedfile)) { LikeCriteria.Add("VM_SparePartsUsedfile", VM_SparePartsUsedfile); }
                        Dictionary<long, IList<VehicleMaintenance>> VehicleMaintenance = ts.VehicleMaintenanceListWithLikeAndExcactSerachCriteria(page - 1, rows, sord, sidx, ExactCriteria, LikeCriteria);
                        if (VehicleMaintenance != null && VehicleMaintenance.Count > 0)
                        {
                            if (ExportType == "Excel")
                            {
                                var List = VehicleMaintenance.First().Value.ToList();
                                ExptToXL(List, "VehicleMaintenanceReportList", (items => new
                                {
                                    Id = items.Id.ToString(),
                                    Vehicle_Id = items.VehicleId.ToString(),
                                    Vehicle_No = items.VehicleNo,
                                    Vehicle_Maintenance_Type = items.VehicleMaintenanceType,
                                    Vehicle_DateOf_Breakdown = items.VehicleDateOfBreakdown != null ? items.VehicleDateOfBreakdown.Value.ToString("dd/MM/yyyy") : "",
                                    Vehicle_Breakdown_Location = items.VehicleBreakdownLocation,
                                    Vehicle_PlannedDateOf_Service = items.VehiclePlannedDateOfService != null ? items.VehiclePlannedDateOfService.Value.ToString("dd/MM/yyyy") : "",
                                    Vehicle_ActualDateOf_Service = items.VehicleActualDateOfService != null ? items.VehicleActualDateOfService.Value.ToString("dd/MM/yyyy") : "",
                                    Vehicle_Service_Provider = items.VehicleServiceProvider,
                                    Vehicle_Sevice_Cost = items.VehicleSeviceCost.ToString(),
                                    Vehicle_Service_BillNo = items.VehicleServiceBillNo,
                                    Vehicle_Spare_Parts_Used = items.VehicleSparePartsUsed,
                                    VM_Spare_Parts_Usedfile = "<a style='color:#034af3;text-decoration:underline' onclick = \"ShowVMsparePartsUsed('" + items.Id + "','" + items.VM_SparePartsUsedfile + "');\"' >" + items.VM_SparePartsUsedfile + "</a>",
                                    CreatedDate = items.CreatedDate != null ? items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : "",
                                    items.CreatedBy
                                }));
                                return new EmptyResult();
                            }
                            else
                            {
                                long totalrecords = VehicleMaintenance.First().Key;
                                int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                                var VehicleMaintenanceList = new
                                {
                                    total = totalPages,
                                    page = page,
                                    records = totalrecords,
                                    rows = (from items in VehicleMaintenance.First().Value
                                            select new
                                            {
                                                i = 2,
                                                cell = new string[] {
                               items.Id.ToString(), items.VehicleId.ToString(), items.VehicleNo,items.VehicleMaintenanceType,
                               items.VehicleDateOfBreakdown!=null?items.VehicleDateOfBreakdown.Value.ToString("dd/MM/yyyy"):"",
                               items.VehicleBreakdownLocation, 
                               items.VehiclePlannedDateOfService!=null?items.VehiclePlannedDateOfService.Value.ToString("dd/MM/yyyy"):"",
                               items.VehicleActualDateOfService!=null?items.VehicleActualDateOfService.Value.ToString("dd/MM/yyyy"):"",
                               items.VehicleServiceProvider,
                               items.VehicleSeviceCost.ToString(),items.VehicleServiceBillNo,items.VehicleSparePartsUsed,
                                "<a style='color:#034af3;text-decoration:underline' onclick = \"ShowVMsparePartsUsed('"+ items.Id + "','"+items.VM_SparePartsUsedfile+ "');\"' >"+items.VM_SparePartsUsedfile+"</a>",
                               items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt"):"",
                               items.CreatedBy
                            }
                                            })
                                };
                                return Json(VehicleMaintenanceList, JsonRequestBehavior.AllowGet);
                            }
                        }
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        //public ActionResult VehicleACMaintenanceJqGrid(VehicleACMaintenance vm, int rows, string sidx, string sord, int? page = 1)
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //        else
        //        {
        //            TransportService ts = new TransportService();
        //            Dictionary<string, object> criteria = new Dictionary<string, object>();
        //            sord = sord == "desc" ? "Desc" : "Asc";
        //            if (vm.VehicleId == 0)
        //            {
        //                return Json(null, JsonRequestBehavior.AllowGet);
        //            }
        //            if (vm.VehicleId > 0)
        //                criteria.Add("VehicleId", vm.VehicleId);
        //            Dictionary<long, IList<VehicleACMaintenance>> VehicleMaintenance = ts.GetVehicleACMaintenanceListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
        //            if (VehicleMaintenance != null && VehicleMaintenance.Count > 0)
        //            {
        //                long totalrecords = VehicleMaintenance.First().Key;
        //                int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
        //                var ACMaintenanceList = new
        //                {
        //                    total = totalPages,
        //                    page = page,
        //                    records = totalrecords,
        //                    rows = (from items in VehicleMaintenance.First().Value
        //                            select new
        //                            {
        //                                i = 2,
        //                                cell = new string[] {
        //                       items.Id.ToString(), items.VehicleId.ToString(), items.VehicleNo,items.ACModel, items.ACMaintenanceType,
        //                       items.ACDateOfBreakdown!=null?items.ACDateOfBreakdown.Value.ToString("dd/MM/yyyy"):"",
        //                       items.ACBreakdownLocation,
        //                       items.ACPlannedDateOfService!=null?items.ACPlannedDateOfService.Value.ToString("dd/MM/yyyy"):"",
        //                       items.ACActualDateOfService!=null?items.ACActualDateOfService.Value.ToString("dd/MM/yyyy"):"",
        //                       items.ACServiceProvider,
        //                       items.ACServiceCost.ToString(),items.ACServiceBillNo,items.ACSparePartsUsed ,
        //                       "<a style='color:#034af3;text-decoration:underline' onclick = \"ShowAMsparePartsUsed('"+ items.Id + "','"+items.AM_SparePartsUsedfile+ "');\"' >"+items.AM_SparePartsUsedfile+"</a>",
        //                       items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt"):"",
        //                       items.CreatedBy
        //                    }
        //                            })
        //                };
        //                return Json(ACMaintenanceList, JsonRequestBehavior.AllowGet);

        //            }
        //            else return Json(null);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
        //        throw ex;
        //    }
        //}


        //    public ActionResult VehicleACMaintenanceReportJqGrid(VehicleACMaintenance vm, string ExportType, string ACDateOfBreakdown, string ACPlannedDateOfService, string ACActualDateOfService,
        //string ACServiceCost, string CreatedDate, string AM_SparePartsUsedfile, int rows, string sidx, string sord, int? page = 1)
        //    {
        //        try
        //        {
        //            string userId = base.ValidateUser();
        //            if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //            else
        //            {
        //                TransportService ts = new TransportService();
        //                Dictionary<string, object> criteria = new Dictionary<string, object>();
        //                sord = sord == "desc" ? "Desc" : "Asc";
        //                if (vm.VehicleId > 0) criteria.Add("VehicleId", vm.VehicleId);

        //                Dictionary<string, object> ExactCriteria = new Dictionary<string, object>();
        //                Dictionary<string, object> LikeCriteria = new Dictionary<string, object>();

        //                if (!string.IsNullOrEmpty(ACServiceCost))
        //                {
        //                    decimal ServiceCost = Convert.ToDecimal(ACServiceCost);
        //                    ExactCriteria.Add("ACServiceCost", ServiceCost);
        //                }

        //                if (!string.IsNullOrEmpty(vm.VehicleNo)) { LikeCriteria.Add("VehicleNo", vm.VehicleNo); }
        //                if (!string.IsNullOrEmpty(vm.ACMaintenanceType)) { LikeCriteria.Add("ACMaintenanceType", vm.ACMaintenanceType); }
        //                if (!string.IsNullOrEmpty(vm.ACBreakdownLocation)) { LikeCriteria.Add("ACBreakdownLocation", vm.ACBreakdownLocation); }
        //                if (!string.IsNullOrEmpty(vm.ACModel)) { LikeCriteria.Add("ACModel", vm.ACModel); }
        //                if (!string.IsNullOrEmpty(vm.ACMaintenanceDescription)) { LikeCriteria.Add("ACMaintenanceDescription", vm.ACMaintenanceDescription); }
        //                if (!string.IsNullOrEmpty(vm.ACServiceProvider)) { LikeCriteria.Add("ACServiceProvider", vm.ACServiceProvider); }
        //                if (!string.IsNullOrEmpty(vm.ACServiceBillNo)) { LikeCriteria.Add("ACServiceBillNo", vm.ACServiceBillNo); }
        //                if (!string.IsNullOrEmpty(vm.CreatedBy)) { LikeCriteria.Add("CreatedBy", vm.CreatedBy); }
        //                if (!string.IsNullOrEmpty(vm.ACSparePartsUsed)) { LikeCriteria.Add("ACSparePartsUsed", vm.ACSparePartsUsed); }
        //                if (!string.IsNullOrEmpty(AM_SparePartsUsedfile)) { LikeCriteria.Add("AM_SparePartsUsedfile", AM_SparePartsUsedfile); }

        //                Dictionary<long, IList<VehicleACMaintenance>> VehicleMaintenance = ts.VehicleACMaintenanceListWithLikeAndExcactSerachCriteria(page - 1, rows, sord, sidx, ExactCriteria, LikeCriteria);
        //                if (VehicleMaintenance != null && VehicleMaintenance.Count > 0)
        //                {
        //                    if (ExportType == "Excel")
        //                    {
        //                        var List = VehicleMaintenance.First().Value.ToList();
        //                        ExptToXL(List, "VehicleACMaintenanceReportList", (items => new
        //                        {
        //                            items.VehicleId,
        //                            items.VehicleNo,
        //                            items.ACModel,
        //                            items.ACMaintenanceType,
        //                            items.ACDateOfBreakdown,
        //                            items.ACBreakdownLocation,
        //                            ACPlannedDateOfService = items.ACPlannedDateOfService != null ? items.ACPlannedDateOfService.Value.ToString("dd/MM/yyyy") : "",
        //                            ACActualDateOfService = items.ACActualDateOfService != null ? items.ACActualDateOfService.Value.ToString("dd/MM/yyyy") : "",
        //                            items.ACServiceProvider,
        //                            ACServiceCost = items.ACServiceCost.ToString(),
        //                            items.ACServiceBillNo,
        //                            items.ACSparePartsUsed,
        //                            items.CreatedDate,
        //                            items.CreatedBy
        //                        }));
        //                        return new EmptyResult();
        //                    }
        //                    else
        //                    {
        //                        long totalrecords = VehicleMaintenance.First().Key;
        //                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
        //                        var ACMaintenanceList = new
        //                        {
        //                            total = totalPages,
        //                            page = page,
        //                            records = totalrecords,
        //                            rows = (from items in VehicleMaintenance.First().Value
        //                                    select new
        //                                    {
        //                                        i = 2,
        //                                        cell = new string[] {
        //                           items.Id.ToString(), items.VehicleId.ToString(), items.VehicleNo,items.ACModel, items.ACMaintenanceType,
        //                           items.ACDateOfBreakdown!=null?items.ACDateOfBreakdown.Value.ToString("dd/MM/yyyy"):"",
        //                           items.ACBreakdownLocation,
        //                           items.ACPlannedDateOfService!=null?items.ACPlannedDateOfService.Value.ToString("dd/MM/yyyy"):"",
        //                           items.ACActualDateOfService!=null?items.ACActualDateOfService.Value.ToString("dd/MM/yyyy"):"",
        //                           items.ACServiceProvider,
        //                           items.ACServiceCost.ToString(),items.ACServiceBillNo,items.ACSparePartsUsed ,
        //                           items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy"):"",
        //                           items.CreatedBy
        //                        }
        //                                    })
        //                        };
        //                        return Json(ACMaintenanceList, JsonRequestBehavior.AllowGet);
        //                    }
        //                }
        //                else
        //                {
        //                    var Empty = new { rows = (new { cell = new string[] { } }) };
        //                    return Json(Empty, JsonRequestBehavior.AllowGet);
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
        //            throw ex;
        //        }
        //    }

        public ActionResult FuelRefillBulkEntry(int? Id)
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

                    TransportService ts = new TransportService();
                    FuelRefillDetails dcd = new FuelRefillDetails();
                    if (Id > 0)
                    {
                        dcd = ts.GetFuelRefillDetailsById(Convert.ToInt32(Id));

                        return View(dcd);
                    }
                    else
                    {
                        UserService us = new UserService();
                        Dictionary<string, object> Criteria = new Dictionary<string, object>();
                        Criteria.Add("UserId", userId);
                        Criteria.Add("AppCode", "TRA");
                        Dictionary<long, IList<UserAppRole>> UserAppRoleList = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, Criteria);
                        var UserDetails = (from u in UserAppRoleList.First().Value
                                           where u.UserId == userId
                                           select new { u.RoleName, u.BranchCode }).ToArray();
                        dcd.Campus = UserDetails[0].BranchCode;
                        dcd.UserRole = UserDetails[0].RoleName;
                        dcd.Status = "Open";
                        dcd.ProcessedBy = userId;
                        return View(dcd);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }


        [HttpPost]
        public ActionResult FuelRefillBulkEntry(FuelRefillDetails dcd)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    dcd.CreatedDate = DateTime.Now;
                    ts.CreateOrUpdateFuelRefillDetails(dcd);
                    dcd.RefNo = "Ref-" + dcd.Id;
                    ts.CreateOrUpdateFuelRefillDetails(dcd);
                    return RedirectToAction("FuelRefillBulkEntry", new { Id = dcd.Id });
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult FuelRefillListBulkEntryJqGrid(VehicleFuelManagement vfm, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    sord = sord == "desc" ? "Desc" : "Asc";
                    if (vfm.RefId == 0)
                    {
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                    if (vfm.RefId > 0)
                        criteria.Add("RefId", vfm.RefId);
                    Dictionary<long, IList<VehicleFuelManagement>> VehicleFuelManagement = ts.GetVehicleFuelManagementListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    if (VehicleFuelManagement != null && VehicleFuelManagement.Count > 0)
                    {
                        long totalrecords = VehicleFuelManagement.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var VehicleDistanceList = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in VehicleFuelManagement.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                               items.Id.ToString(),items.RefId.ToString(), items.VehicleId.ToString(),items.Type, items.VehicleNo,items.FuelType, items.FuelFillType,
                               items.FuelQuantity.ToString(), items.LitrePrice.ToString(), items.TotalPrice.ToString(), items.LastMilometerReading.ToString(), items.CurrentMilometerReading.ToString(), items.Mileage.ToString(),
                               items.FilledBy,
                               items.FilledDate!=null?  items.FilledDate.Value.ToString("dd/MM/yyyy"):"",
                               items.BunkName,
                               items.CreatedBy,
                                items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt"):"",
                            }
                                    })
                        };
                        return Json(VehicleDistanceList, JsonRequestBehavior.AllowGet);

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

        public ActionResult AddFuelRefillList(IList<VehicleFuelManagement> DLst)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    if (DLst != null)
                    {
                        foreach (var d in DLst)
                        {
                            Dictionary<string, object> criteria = new Dictionary<string, object>();
                            criteria.Add("VehicleId", d.VehicleId);
                            Dictionary<long, IList<VehicleFuelManagement>> FuelList = ts.GetVehicleFuelManagementListWithPagingAndCriteria(0, 50, "Id", "Desc", criteria);
                            if (FuelList != null && FuelList.FirstOrDefault().Value != null && FuelList.FirstOrDefault().Key > 0)
                                d.LastMilometerReading = FuelList.FirstOrDefault().Value[0].CurrentMilometerReading;
                            criteria.Clear();
                            d.CreatedBy = userId;
                            d.CreatedDate = DateTime.Now;
                        }
                        ts.CreateOrUpdateVehicleFuelManagementList(DLst);
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            { return ThrowJSONErrorNew(ex, "TransportMgmtPolicy"); }
        }

        public ActionResult CompleteFuelRefillList(IList<VehicleFuelManagement> DLst)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    if (DLst != null)
                    {
                        foreach (var d in DLst)
                        {
                            d.CreatedBy = userId;
                            d.CreatedDate = DateTime.Now;
                        }
                        ts.CreateOrUpdateVehicleFuelManagementList(DLst);
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            { return ThrowJSONErrorNew(ex, "TransportMgmtPolicy"); }
        }


        public ActionResult DeleteFuelRefillId(int Id)
        {
            try
            {
                string userId = ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    VehicleFuelManagement vdc = new VehicleFuelManagement();
                    if (Id > 0)
                        vdc = ts.GetVehicleFuelManagementById(Id);
                    if (vdc != null)
                        ts.DeleteVehicleFuelManagementbyId(vdc);
                    return Json(Id, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult CompleteFuelRefill(FuelRefillDetails vdc)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    vdc = ts.GetFuelRefillDetailsById(Convert.ToInt32(vdc.Id));
                    vdc.Status = "Completed";
                    ts.CreateOrUpdateFuelRefillDetails(vdc);
                    return Json(vdc.RefNo, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult FitnessCertificateBulkEntry(int? Id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    FitnessCertificateDetails dcd = new FitnessCertificateDetails();
                    if (Id > 0)
                    {
                        dcd = ts.GetFitnessCertificateDetailsById(Convert.ToInt32(Id));
                        return View(dcd);
                    }
                    else
                    {
                        UserService us = new UserService();
                        Dictionary<string, object> Criteria = new Dictionary<string, object>();
                        Criteria.Add("UserId", userId);
                        Criteria.Add("AppCode", "TRA");
                        Dictionary<long, IList<UserAppRole>> UserAppRoleList = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, Criteria);
                        var UserDetails = (from u in UserAppRoleList.First().Value
                                           where u.UserId == userId
                                           select new { u.RoleName, u.BranchCode }).ToArray();
                        dcd.Campus = UserDetails[0].BranchCode;
                        dcd.UserRole = UserDetails[0].RoleName;
                        dcd.Status = "Open";
                        dcd.ProcessedBy = userId;
                        return View(dcd);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }


        [HttpPost]
        public ActionResult FitnessCertificateBulkEntry(FitnessCertificateDetails dcd)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    dcd.CreatedDate = DateTime.Now;
                    ts.CreateOrUpdateFitnessCertificateDetails(dcd);
                    dcd.RefNo = "Ref-" + dcd.Id;
                    ts.CreateOrUpdateFitnessCertificateDetails(dcd);
                    return RedirectToAction("FitnessCertificateBulkEntry", new { Id = dcd.Id });
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult FitnessCertificateListBulkEntryJqGrid(FitnessCertificate vfm, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    sord = sord == "desc" ? "Desc" : "Asc";
                    if (vfm.RefId == 0)
                    {
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                    if (vfm.RefId > 0)
                        criteria.Add("RefId", vfm.RefId);
                    Dictionary<long, IList<FitnessCertificate>> FitnessCertificate = ts.GetFitnessCertificateDetailsWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    if (FitnessCertificate != null && FitnessCertificate.Count > 0)
                    {
                        long totalrecords = FitnessCertificate.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var FitnessCertificateList = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in FitnessCertificate.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                                        items.Id.ToString(),items.RefId.ToString(), items.VehicleId.ToString(),
                                        items.Type,
                                        items.VehicleNo,
                                        items.FCDate.ToString(),
                                        items.NextFCDate.ToString(),
                                        items.FCCost.ToString(),
                                        items.Description,
                                        items.FCWorkCarriedAt,
                                        items.RTO,
                                        items.Driver,
                                        items.CreatedBy,
                                        items.CreatedDate.ToString()
                            }
                                    })
                        };
                        return Json(FitnessCertificateList, JsonRequestBehavior.AllowGet);

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

        public ActionResult AddFitnessCertificateList(IList<FitnessCertificate> DLst)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    if (DLst != null)
                    {
                        foreach (var d in DLst)
                        {
                            d.CreatedBy = userId;
                            d.CreatedDate = DateTime.Now;
                        }
                        ts.CreateOrUpdateFitnessCertificateList(DLst);
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            { return ThrowJSONErrorNew(ex, "TransportMgmtPolicy"); }
        }

        public ActionResult CompleteFitnessCertificateDetails(FitnessCertificateDetails vdc)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    vdc = ts.GetFitnessCertificateDetailsById(Convert.ToInt32(vdc.Id));
                    vdc.Status = "Completed";
                    ts.CreateOrUpdateFitnessCertificateDetails(vdc);
                    return Json(vdc.RefNo, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult InsuranceBulkEntry(int? Id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    InsuranceBulkDetails dcd = new InsuranceBulkDetails();
                    if (Id > 0)
                    {
                        dcd = ts.GetInsuranceBulkDetailsById(Convert.ToInt32(Id));
                        return View(dcd);
                    }
                    else
                    {
                        UserService us = new UserService();
                        Dictionary<string, object> Criteria = new Dictionary<string, object>();
                        Criteria.Add("UserId", userId);
                        Criteria.Add("AppCode", "TRA");
                        Dictionary<long, IList<UserAppRole>> UserAppRoleList = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, Criteria);
                        var UserDetails = (from u in UserAppRoleList.First().Value
                                           where u.UserId == userId
                                           select new { u.RoleName, u.BranchCode }).ToArray();
                        dcd.Campus = UserDetails[0].BranchCode;
                        dcd.UserRole = UserDetails[0].RoleName;
                        dcd.Status = "Open";
                        dcd.ProcessedBy = userId;
                        return View(dcd);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult InsuranceBulkEntry(InsuranceBulkDetails dcd)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    dcd.CreatedDate = DateTime.Now;
                    ts.CreateOrUpdateInsuranceBulkDetails(dcd);
                    dcd.RefNo = "Ref-" + dcd.Id;
                    ts.CreateOrUpdateInsuranceBulkDetails(dcd);
                    return RedirectToAction("InsuranceBulkEntry", new { Id = dcd.Id });
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult InsuranceListBulkEntryJqGrid(Insurance vfm, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    sord = sord == "desc" ? "Desc" : "Asc";
                    if (vfm.RefId == 0)
                    {
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                    if (vfm.RefId > 0)
                        criteria.Add("RefId", vfm.RefId);
                    Dictionary<long, IList<Insurance>> Insurance = ts.GetInsuranceDetailsWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    if (Insurance != null && Insurance.Count > 0)
                    {
                        long totalrecords = Insurance.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var FitnessCertificateList = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in Insurance.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                                        items.Id.ToString(),items.RefId.ToString(), items.VehicleId.ToString(),
                                        items.Type,
                                        items.VehicleNo,
                                        items.InsuranceDate.ToString(),
                                        items.NextInsuranceDate.ToString(),
                                        items.InsuranceProvider,
                                        items.InsuranceDeclaredValue.ToString(),
                                        //items.ValidityFromDate.ToString(),
                                        //items.ValidityToDate.ToString(),
                                        items.CreatedBy,
                                        items.CreatedDate.ToString()
                            }
                                    })
                        };
                        return Json(FitnessCertificateList, JsonRequestBehavior.AllowGet);
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

        public ActionResult AddInsuranceList(IList<Insurance> DLst)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    if (DLst != null)
                    {
                        foreach (var d in DLst)
                        {
                            d.CreatedBy = userId;
                            d.CreatedDate = DateTime.Now;
                        }
                        ts.CreateOrUpdateInsuranceList(DLst);
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            { return ThrowJSONErrorNew(ex, "TransportMgmtPolicy"); }
        }

        public ActionResult DeleteInsuranceById(int? Id)
        {
            try
            {
                string userId = ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    Insurance vdc = new Insurance();
                    if (Id > 0)
                        vdc = ts.GetInsuranceDetailsById(Convert.ToInt64(Id));
                    if (vdc != null)
                        ts.DeleteInsurancebyId(vdc);
                    return Json(Id, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }


        public ActionResult FinesAndPenalitiesBulkEntry(int? Id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    FinesAndPenalitiesBulkDetails dcd = new FinesAndPenalitiesBulkDetails();
                    if (Id > 0)
                    {
                        dcd = ts.GetFinesAndPenalitiesBulkDetailsById(Convert.ToInt32(Id));
                        return View(dcd);
                    }
                    else
                    {
                        UserService us = new UserService();
                        Dictionary<string, object> Criteria = new Dictionary<string, object>();
                        Criteria.Add("UserId", userId);
                        Criteria.Add("AppCode", "TRA");
                        Dictionary<long, IList<UserAppRole>> UserAppRoleList = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, Criteria);
                        var UserDetails = (from u in UserAppRoleList.First().Value
                                           where u.UserId == userId
                                           select new { u.RoleName, u.BranchCode }).ToArray();
                        dcd.Campus = UserDetails[0].BranchCode;
                        dcd.UserRole = UserDetails[0].RoleName;
                        dcd.Status = "Open";
                        dcd.ProcessedBy = userId;
                        return View(dcd);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult FinesAndPenalitiesBulkEntry(FinesAndPenalitiesBulkDetails dcd)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    dcd.CreatedDate = DateTime.Now;
                    ts.CreateOrUpdateFinesAndPenalitiesBulkDetails(dcd);
                    dcd.RefNo = "Ref-" + dcd.Id;
                    ts.CreateOrUpdateFinesAndPenalitiesBulkDetails(dcd);
                    return RedirectToAction("FinesAndPenalitiesBulkEntry", new { Id = dcd.Id });
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult AddFinesAndPenalitiesList(IList<FinesAndPenalities> DLst)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    if (DLst != null)
                    {
                        foreach (var d in DLst)
                        {
                            d.CreatedBy = userId;
                            d.CreatedDate = DateTime.Now;
                        }
                        ts.CreateOrUpdateFinesAndPenalitiesList(DLst);
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            { return ThrowJSONErrorNew(ex, "TransportMgmtPolicy"); }
        }

        public ActionResult FinesAndPenalitiesBulkEntryJqGrid(FinesAndPenalities vfm, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    sord = sord == "desc" ? "Desc" : "Asc";
                    if (vfm.RefId == 0)
                    {
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                    if (vfm.RefId > 0)
                        criteria.Add("RefId", vfm.RefId);
                    Dictionary<long, IList<FinesAndPenalities>> FinesAndPenalities = ts.GetFinesAndPenalitiesIdViewListWithCriteria(page - 1, rows, sidx, sord, criteria);
                    if (FinesAndPenalities != null && FinesAndPenalities.Count > 0)
                    {
                        long totalrecords = FinesAndPenalities.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var FitnessCertificateList = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in FinesAndPenalities.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                                        items.Id.ToString(),items.RefId.ToString(), items.VehicleId.ToString(),
                                        items.Type,
                                        items.VehicleNo,
                                        items.PenalityDate.ToString(),
                                        items.PenalityArea,
                                        items.PenalityReason,
                                        items.PenalityRupees.ToString(),
                                        items.PenalityDueDate.ToString(),
                                        items.PenalityPaidBy,
                                        items.DriverName,
                                        items.CreatedBy,
                                        items.CreatedDate.ToString()
                            }
                                    })
                        };
                        return Json(FitnessCertificateList, JsonRequestBehavior.AllowGet);

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

        public ActionResult DeleteFinesAndPenalitiesById(int? Id)
        {
            try
            {
                string userId = ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    FinesAndPenalities vdc = new FinesAndPenalities();
                    if (Id > 0)
                        vdc = ts.GetFinesAndPenalitiesById(Convert.ToInt64(Id));
                    if (vdc != null)
                        ts.DeleteFinesAndPenalitiesbyId(vdc);
                    return Json(Id, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult FuelRefillBulkEntryList()
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
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult FuelRefillBulkEntryJqGrid(int? Id, string ExportType, string status, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    if (!string.IsNullOrWhiteSpace(status)) { criteria.Add("Status", status); }
                    if (Id > 0) { criteria.Add("Id", Id); }
                    Dictionary<long, IList<FuelRefillDetails>> RouteMasterList = ts.GetFuelRefillDetailsListWithsearchCriteria(page - 1, rows, sidx, sord, criteria);
                    if (RouteMasterList != null && RouteMasterList.First().Key > 0)
                    {
                        long totalrecords = RouteMasterList.FirstOrDefault().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var AssLst = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (
                                 from items in RouteMasterList.First().Value
                                 select new
                                 {
                                     cell = new string[] 
                                         {
                                            items.Id.ToString(),
                                            items.RefNo,
                                            items.Campus,
                                            items.ProcessedBy,
                                            items.CreatedDate!=null? items.CreatedDate.ToString("dd/MM/yyyy hh:mm:ss tt"):null,
                                            items.Description,
                                            items.UserRole,
                                            items.Status,
                                         }
                                 }).ToList()
                        };
                        return Json(AssLst, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }


        #region Vehicle Fuel and Distance covered Report added by micheal

        //public ActionResult VehicleFuelReport()
        //{
        //    int[] years = new int[15];
        //    DateTime daytime = DateTime.Now;
        //    int CurYear = daytime.Year;
        //    int CurMonth = daytime.Month;
        //    ViewBag.CurYear = CurYear;
        //    ViewBag.CurMonth = CurMonth;
        //    CurYear = CurYear - 5;
        //    for (int i = 0; i < 15; i++)
        //    {
        //        years[i] = CurYear + i;
        //    }
        //    ViewBag.years = years;
        //    #region BreadCrumb
        //    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
        //    #endregion
        //    return View();

        //}
        //public ActionResult VehicleDistanceCoveredReport()
        //{
        //    int[] years = new int[15];
        //    DateTime daytime = DateTime.Now;
        //    int CurYear = daytime.Year;
        //    int CurMonth = daytime.Month;
        //    ViewBag.CurYear = CurYear;
        //    ViewBag.CurMonth = CurMonth;
        //    CurYear = CurYear - 5;
        //    for (int i = 0; i < 15; i++)
        //    {
        //        years[i] = CurYear + i;
        //    }
        //    ViewBag.years = years;
        //    #region BreadCrumb
        //    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
        //    #endregion
        //    return View();
        //}
        //public ActionResult VehicleDistanceReportJqGrid(string Type, string VehicleNo, string Month, int? CurrMonth, int? CurrYear, string Year, string DistanceCovered, int rows, string sidx, string sord, int? page = 1, int? ExptXl = 0)
        //{
        //    try
        //    {
        //        Dictionary<string, object> criteria = new Dictionary<string, object>();
        //        TransportService tns = new TransportService();
        //        sord = sord == "desc" ? "Desc" : "Asc";
        //        if (!string.IsNullOrEmpty(Type))
        //            criteria.Add("Type", Type.Trim());
        //        if (!string.IsNullOrEmpty(VehicleNo))
        //            criteria.Add("VehicleNo", VehicleNo.Trim());
        //        if (CurrMonth >= 0)
        //            criteria.Add("Month", CurrMonth);
        //        if (!string.IsNullOrEmpty(Month))
        //        {
        //            if (Month == "select")
        //            {
        //                if (CurrMonth >= 0)
        //                    criteria.Remove("Month");
        //                criteria.Add("Month", CurrMonth);
        //            }
        //            else
        //            {
        //                criteria.Remove("Month");
        //                criteria.Add("Month", Convert.ToInt32(Month));
        //            }
        //        }
        //        if (CurrYear >= 0)
        //            criteria.Add("Year", CurrYear);
        //        if (!string.IsNullOrEmpty(Year))
        //        {
        //            criteria.Add("Year", Convert.ToInt32(Year));
        //        }
        //        if (!string.IsNullOrEmpty(DistanceCovered))
        //        {
        //            criteria.Add("DistanceCovered", Convert.ToDecimal(DistanceCovered));
        //        }
        //        Dictionary<long, IList<VehicleDistanceCoveredReport_vw>> VehicleDistanceList = tns.GetVehicleDistanceReportListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
        //        if (VehicleDistanceList != null && VehicleDistanceList.Count > 0 && VehicleDistanceList.FirstOrDefault().Key > 0 && VehicleDistanceList.FirstOrDefault().Value.Count > 0)
        //        {
        //            if (ExptXl == 1)
        //            {
        //                var List = VehicleDistanceList.First().Value.ToList();
        //                base.ExptToXL(List, "VehicleDistanceCoveredReport", (items => new
        //                {
        //                    items.Id,
        //                    items.Type,
        //                    items.VehicleNo,
        //                    items.Month,
        //                    items.Year,
        //                    items.DistanceCovered,
        //                    LastTripDate = items.LastTripDate != null ? items.LastTripDate.Value.ToString("dd/MM/yyyy") : null
        //                }));
        //                return new EmptyResult();
        //            }
        //            else
        //            {
        //                long totalRecords = VehicleDistanceList.First().Key;
        //                int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
        //                var jsonData = new
        //                {
        //                    total = totalPages,
        //                    page = page,
        //                    records = totalRecords,
        //                    rows = (
        //                         from items in VehicleDistanceList.First().Value
        //                         select new
        //                         {
        //                             i = items.Id,
        //                             cell = new string[] { 
        //                             items.Id.ToString(), 
        //                             items.Type,
        //                             items.VehicleNo,
        //                             items.Month.ToString(),
        //                             items.Year.ToString(), 
        //                             items.DistanceCovered.ToString(),
        //                             items.LastTripDate!=null? items.LastTripDate.Value.ToString("dd/MM/yyyy"):null
        //                             }
        //                         })
        //                };
        //                return Json(jsonData, JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //        var Empty = new { rows = (new { cell = new string[] { } }) };
        //        return Json(Empty, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
        //        throw ex;
        //    }
        //}
        public ActionResult VehicleDistanceReportChart(int? CurrMonth, int? CurrYear)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                TransportService tns = new TransportService();
                if (CurrMonth >= 0)
                    criteria.Add("Month", CurrMonth);
                if (CurrYear >= 0)
                    criteria.Add("Year", CurrYear);
                Dictionary<long, IList<VehicleDistanceCoveredReport_vw>> VehicleDistanceList = tns.GetVehicleDistanceReportListWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, criteria);
                if (VehicleDistanceList != null && VehicleDistanceList.Count > 0 && VehicleDistanceList.FirstOrDefault().Key > 0 && VehicleDistanceList.FirstOrDefault().Value.Count > 0)
                {
                    return null;
                }
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }

        }

        //        public ActionResult VehicleBodyMaintenanceReportJqGrid(string ExportType, VehicleBodyMaintenance vm,
        //string VehicleNo, string BTypeOfBody, string BDateOfRepair, string BTypeOfRepair, string BPartsRequired, string BServiceProvider,
        //string BServiceCost, string BBillNo, string BDescription, string CreatedDate, string CreatedBy, string BM_SparePartsUsedfile,
        //    int rows, string sidx, string sord, int? page = 1)
        //        {
        //            try
        //            {
        //                string userId = base.ValidateUser();
        //                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //                else
        //                {
        //                    TransportService ts = new TransportService();
        //                    Dictionary<string, object> criteria = new Dictionary<string, object>();
        //                    sord = sord == "desc" ? "Desc" : "Asc";

        //                    if (vm.VehicleId > 0)
        //                        criteria.Add("VehicleId", vm.VehicleId);
        //                    Dictionary<string, object> ExactCriteria = new Dictionary<string, object>();
        //                    Dictionary<string, object> LikeCriteria = new Dictionary<string, object>();
        //                    if (!string.IsNullOrEmpty(BServiceCost))
        //                    {
        //                        decimal ServiceCost = Convert.ToDecimal(BServiceCost);
        //                        ExactCriteria.Add("BServiceCost", ServiceCost);
        //                    }
        //                    if (!string.IsNullOrEmpty(VehicleNo)) { LikeCriteria.Add("VehicleNo", VehicleNo); }
        //                    if (!string.IsNullOrEmpty(BTypeOfBody)) { LikeCriteria.Add("BTypeOfBody", BTypeOfBody); }
        //                    if (!string.IsNullOrEmpty(BTypeOfRepair)) { LikeCriteria.Add("BTypeOfRepair", BTypeOfRepair); }
        //                    if (!string.IsNullOrEmpty(BPartsRequired)) { LikeCriteria.Add("BPartsRequired", BPartsRequired); }
        //                    if (!string.IsNullOrEmpty(BServiceProvider)) { LikeCriteria.Add("BServiceProvider", BServiceProvider); }
        //                    if (!string.IsNullOrEmpty(BBillNo)) { LikeCriteria.Add("BBillNo", BBillNo); }
        //                    if (!string.IsNullOrEmpty(BDescription)) { LikeCriteria.Add("BDescription", BDescription); }
        //                    if (!string.IsNullOrEmpty(CreatedBy)) { LikeCriteria.Add("CreatedBy", CreatedBy); }
        //                    if (!string.IsNullOrEmpty(BM_SparePartsUsedfile)) { LikeCriteria.Add("BM_SparePartsUsedfile", BM_SparePartsUsedfile); }
        //                    Dictionary<long, IList<VehicleBodyMaintenance>> VehicleBodyMaintenance = ts.VehicleBodyMaintenanceListWithLikeAndExcactSerachCriteria(page - 1, rows, sord, sidx, ExactCriteria, LikeCriteria);
        //                    if (VehicleBodyMaintenance != null && VehicleBodyMaintenance.Count > 0)
        //                    {
        //                        if (ExportType == "Excel")
        //                        {
        //                            var List = VehicleBodyMaintenance.First().Value.ToList();
        //                            ExptToXL(List, "VehicleBodyMaintenanceReportList", (items => new
        //                            {
        //                                items.VehicleId,
        //                                items.VehicleNo,
        //                                items.BTypeOfBody,
        //                                BDateOfRepair = items.BDateOfRepair != null ? items.BDateOfRepair.Value.ToString("dd/MM/yyyy") : "",
        //                                items.BTypeOfRepair,
        //                                items.BPartsRequired,
        //                                items.BServiceProvider,
        //                                BServiceCost = items.BServiceCost.ToString(),
        //                                items.BBillNo,
        //                                items.BDescription,
        //                                CreatedDate = items.CreatedDate != null ? items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : "",
        //                                items.CreatedBy
        //                            }));
        //                            return new EmptyResult();
        //                        }
        //                        else
        //                        {
        //                            long totalrecords = VehicleBodyMaintenance.First().Key;
        //                            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
        //                            var BodyMaintenanceList = new
        //                            {
        //                                total = totalPages,
        //                                page = page,
        //                                records = totalrecords,
        //                                rows = (from items in VehicleBodyMaintenance.First().Value
        //                                        select new
        //                                        {
        //                                            i = 2,
        //                                            cell = new string[] {
        //                               items.Id.ToString(), items.VehicleId.ToString(), items.VehicleNo,items.BTypeOfBody,
        //                               items.BDateOfRepair!=null?items.BDateOfRepair.Value.ToString("dd/MM/yyyy"):"",
        //                               items.BTypeOfRepair, items.BPartsRequired, items.BServiceProvider, items.BServiceCost.ToString(),items.BBillNo,items.BDescription,
        //                               items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy"):"",
        //                               items.CreatedBy
        //                            }
        //                                        })
        //                            };
        //                            return Json(BodyMaintenanceList, JsonRequestBehavior.AllowGet);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        var Empty = new { rows = (new { cell = new string[] { } }) };
        //                        return Json(Empty, JsonRequestBehavior.AllowGet);
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
        //                throw ex;
        //            }
        //        }
        //        public ActionResult VehicleFuelReportJqGrid(string Type, string VehicleNo, int? CurrMonth, int? CurrYear, string Month, string Year, string FuelQty, int rows, string sidx, string sord, int? page = 1, int? ExptXl = 0)
        //        {
        //            try
        //            {
        //                Dictionary<string, object> criteria = new Dictionary<string, object>();
        //                TransportService tns = new TransportService();
        //                sord = sord == "desc" ? "Desc" : "Asc";
        //                if (!string.IsNullOrEmpty(Type))
        //                    criteria.Add("Type", Type.Trim());
        //                if (!string.IsNullOrEmpty(VehicleNo))
        //                    criteria.Add("VehicleNo", VehicleNo.Trim());
        //                if (CurrMonth >= 0)
        //                    criteria.Add("Month", CurrMonth);
        //                if (CurrYear >= 0)
        //                    criteria.Add("Year", CurrYear);
        //                if (!string.IsNullOrEmpty(Month))
        //                {
        //                    if (Month == "select")
        //                    {
        //                        if (CurrMonth >= 0)
        //                            criteria.Remove("Month");
        //                        criteria.Add("Month", CurrMonth);
        //                    }
        //                    else
        //                    {
        //                        criteria.Remove("Month");
        //                        criteria.Add("Month", Convert.ToInt32(Month));
        //                    }
        //                }
        //                if (!string.IsNullOrEmpty(Year))
        //                {
        //                    criteria.Add("Year", Convert.ToInt32(Year));
        //                }
        //                if (!string.IsNullOrEmpty(FuelQty))
        //                {
        //                    criteria.Add("FuelQty", Convert.ToDecimal(FuelQty));
        //                }
        //                Dictionary<long, IList<VehicleFuelReport_vw>> VehicleFuelList = tns.GetVehicleFuelListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
        //                if (VehicleFuelList != null && VehicleFuelList.Count > 0 && VehicleFuelList.FirstOrDefault().Key > 0 && VehicleFuelList.FirstOrDefault().Value.Count > 0)
        //                {
        //                    if (ExptXl == 1)
        //                    {
        //                        var List = VehicleFuelList.First().Value.ToList();
        //                        base.ExptToXL(List, "VehicleFuelQuantityReport", (items => new
        //                        {
        //                            items.Id,
        //                            items.Type,
        //                            items.VehicleNo,
        //                            items.Month,
        //                            items.Year,
        //                            items.FuelQty,
        //                            items.TotalPrice,
        //                            LastFilledDate = items.LastFilledDate != null ? items.LastFilledDate.Value.ToString("dd/MM/yyyy") : null
        //                        }));
        //                        return new EmptyResult();
        //                    }
        //                    else
        //                    {
        //                        long totalRecords = VehicleFuelList.First().Key;
        //                        int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
        //                        var jsonData = new
        //                        {
        //                            total = totalPages,
        //                            page = page,
        //                            records = totalRecords,
        //                            rows = (
        //                                 from items in VehicleFuelList.First().Value
        //                                 select new
        //                                 {
        //                                     i = items.Id,
        //                                     cell = new string[] { 
        //                                     items.Id.ToString(), 
        //                                     items.Type,
        //                                     items.VehicleNo,
        //                                     items.Month.ToString(),
        //                                     items.Year.ToString(), 
        //                                     items.FuelQty.ToString(),
        //                                     items.TotalPrice.ToString(),
        //                                     items.LastFilledDate!=null? items.LastFilledDate.Value.ToString("dd/MM/yyyy"):null
        //                                     }
        //                                 })
        //                        };
        //                        return Json(jsonData, JsonRequestBehavior.AllowGet);
        //                    }
        //                }
        //                var Empty = new { rows = (new { cell = new string[] { } }) };
        //                return Json(Empty, JsonRequestBehavior.AllowGet);
        //            }
        //            catch (Exception ex)
        //            {
        //                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
        //                throw ex;
        //            }
        //        }

        public ActionResult VehicleDistanceCoveredReportChart(string VehicleId)
        {
            try
            {
                TransportService tns = new TransportService();
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    Dictionary<string, object> Criteria = new Dictionary<string, object>();
                    if (!string.IsNullOrEmpty(VehicleId))
                        Criteria.Add("VehicleId", VehicleId.Trim());
                    Criteria.Add("Year", DateTime.Now.Year);
                    var DistanceChart = "";
                    Dictionary<long, IList<VehicleDistanceCoveredChart_vw>> DistanceList = tns.GetVehicleDistancechartListWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, Criteria);
                    if (DistanceList != null && DistanceList.FirstOrDefault().Value != null && DistanceList.FirstOrDefault().Key > 0 && DistanceList.FirstOrDefault().Value.Count > 0)
                    {
                        var VehicleList = (from u in DistanceList.First().Value
                                           select u).ToList();
                        decimal Jan = 0; decimal Feb = 0; decimal Mar = 0; decimal Apr = 0; decimal May = 0; decimal Jun = 0; decimal Jul = 0; decimal Aug = 0; decimal Sep = 0; decimal Oct = 0; decimal Nov = 0; decimal Dec = 0;
                        foreach (var itemdata in VehicleList)
                        {
                            Jan = Jan + itemdata.Jan;
                            Feb = Feb + itemdata.Feb;
                            Mar = Mar + itemdata.Mar;
                            Apr = Apr + itemdata.Apr;
                            May = May + itemdata.May;
                            Jun = Jun + itemdata.Jun;
                            Jul = Jul + itemdata.Jul;
                            Aug = Aug + itemdata.Aug;
                            Sep = Sep + itemdata.Sep;
                            Oct = Oct + itemdata.Oct;
                            Nov = Nov + itemdata.Nov;
                            Dec = Dec + itemdata.Dec;
                        }
                        DistanceChart = "<graph caption='' decimalSeparator=',' thousandSeparator='.' xAxisName='Month' yAxisName='Distance Covered(Km)' decimalPrecision='0' formatNumberScale='0' showNames='1' rotateNames='1'>";
                        DistanceChart = DistanceChart + " <set name='January' value='" + Jan + "' color='AFD8F8' />";
                        DistanceChart = DistanceChart + " <set name='February' value='" + Feb + "' color='F6BD0F' />";
                        DistanceChart = DistanceChart + " <set name='March' value='" + Mar + "' color='8BBA00' />";
                        DistanceChart = DistanceChart + " <set name='April' value='" + Apr + "' color='FF8E46' />";
                        DistanceChart = DistanceChart + " <set name='May' value='" + May + "' color='08E8E' />";
                        DistanceChart = DistanceChart + " <set name='June' value='" + Jun + "' color='D64646' />";
                        DistanceChart = DistanceChart + " <set name='July' value='" + Jul + "' color='8BBA00' />";
                        DistanceChart = DistanceChart + " <set name='August' value='" + Aug + "' color='FF8E46' />";
                        DistanceChart = DistanceChart + " <set name='September' value='" + Sep + "' color='08E8EA' />";
                        DistanceChart = DistanceChart + " <set name='October' value='" + Oct + "' color='D64646' />";
                        DistanceChart = DistanceChart + " <set name='November' value='" + Nov + "' color='08A8EA' />";
                        DistanceChart = DistanceChart + " <set name='December' value='" + Dec + "' color='08E0E5' /></graph>";
                    }
                    else
                    {
                        DistanceChart = "<graph caption='' decimalSeparator=',' thousandSeparator='.' xAxisName='Month' yAxisName='Distance Covered(Km)' decimalPrecision='0' formatNumberScale='0' showNames='1' rotateNames='1'></graph>";
                    }
                    Response.Write(DistanceChart);
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult VehicleFuelReportChart(string VehicleId)
        {
            try
            {
                TransportService tns = new TransportService();
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    Dictionary<string, object> Criteria = new Dictionary<string, object>();
                    if (!string.IsNullOrEmpty(VehicleId))
                        Criteria.Add("VehicleId", VehicleId.Trim());
                    Criteria.Add("Year", DateTime.Now.Year);
                    Dictionary<long, IList<VehicleFuelQuantityChart_vw>> FuelList = tns.GetVehicleFuelchartListWithCriteria(null, 9999, string.Empty, string.Empty, Criteria);
                    var FuelChart = string.Empty;
                    if (FuelList != null && FuelList.FirstOrDefault().Value != null && FuelList.FirstOrDefault().Key > 0 && FuelList.FirstOrDefault().Value.Count > 0)
                    {
                        var VehicleList = (from u in FuelList.First().Value
                                           select u).ToList();
                        decimal Jan = 0; decimal Feb = 0; decimal Mar = 0; decimal Apr = 0; decimal May = 0; decimal Jun = 0; decimal Jul = 0; decimal Aug = 0; decimal Sep = 0; decimal Oct = 0; decimal Nov = 0; decimal Dec = 0;
                        foreach (var itemdata in VehicleList)
                        {
                            Jan = Jan + itemdata.Jan;
                            Feb = Feb + itemdata.Feb;
                            Mar = Mar + itemdata.Mar;
                            Apr = Apr + itemdata.Apr;
                            May = May + itemdata.May;
                            Jun = Jun + itemdata.Jun;
                            Jul = Jul + itemdata.Jul;
                            Aug = Aug + itemdata.Aug;
                            Sep = Sep + itemdata.Sep;
                            Oct = Oct + itemdata.Oct;
                            Nov = Nov + itemdata.Nov;
                            Dec = Dec + itemdata.Dec;
                        }
                        FuelChart = "<graph caption=''  xAxisName='Month' yAxisName='Fuel Quantity(ltr)' decimalPrecision='0' formatNumberScale='0' showNames='1' rotateNames='1'>";
                        FuelChart = FuelChart + " <set name='January' value='" + Jan + "' color='AFD8F8' />";
                        FuelChart = FuelChart + " <set name='February' value='" + Feb + "' color='F6BD0F' />";
                        FuelChart = FuelChart + " <set name='March' value='" + Mar + "' color='8BBA00' />";
                        FuelChart = FuelChart + " <set name='April' value='" + Apr + "' color='FF8E46' />";
                        FuelChart = FuelChart + " <set name='May' value='" + May + "' color='08E8E' />";
                        FuelChart = FuelChart + " <set name='June' value='" + Jun + "' color='D64646' />";
                        FuelChart = FuelChart + " <set name='July' value='" + Jul + "' color='8BBA00' />";
                        FuelChart = FuelChart + " <set name='August' value='" + Aug + "' color='FF8E46' />";
                        FuelChart = FuelChart + " <set name='September' value='" + Sep + "' color='08E8EA' />";
                        FuelChart = FuelChart + " <set name='October' value='" + Oct + "' color='D64646' />";
                        FuelChart = FuelChart + " <set name='November' value='" + Nov + "' color='08A8EA' />";
                        FuelChart = FuelChart + " <set name='December' value='" + Dec + "' color='08E0E5' /></graph>";
                    }
                    else
                        FuelChart = "<graph caption='' xAxisName='Month' yAxisName='Fuel Quantity(ltr)' decimalPrecision='0' formatNumberScale='0' showNames='1' rotateNames='1'></graph>";
                    Response.Write(FuelChart);
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        #endregion

        //public ActionResult VehicleDistanceFuelReport()
        //{
        //    int[] years = new int[15];
        //    DateTime daytime = DateTime.Now;
        //    int CurYear = daytime.Year;
        //    int CurMonth = daytime.Month;
        //    ViewBag.CurYear = CurYear;
        //    ViewBag.CurMonth = CurMonth;
        //    CurYear = CurYear - 5;
        //    for (int i = 0; i < 15; i++)
        //    {
        //        years[i] = CurYear + i;
        //    }
        //    ViewBag.years = years;
        //    #region BreadCrumb
        //    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
        //    #endregion
        //    return View();
        //}

        //public ActionResult VehicleDistanceFuelReportJqGrid(VehicleDistanceFuelReport_vw veh, string Type, string VehicleNo, int? CurrMonth, int? CurrYear, string Month, string Year, int rows, string sidx, string sord, int? page = 1, int? ExptXl = 0)
        //{
        //    try
        //    {
        //        Dictionary<string, object> criteria = new Dictionary<string, object>();
        //        TransportService tns = new TransportService();
        //        sord = sord == "desc" ? "Desc" : "Asc";
        //        if (veh.VehicleId > 0)
        //            criteria.Add("VehicleId", veh.VehicleId);
        //        if (!string.IsNullOrEmpty(Type))
        //            criteria.Add("Type", Type.Trim());
        //        if (!string.IsNullOrEmpty(VehicleNo))
        //            criteria.Add("VehicleNo", VehicleNo.Trim());
        //        if (!string.IsNullOrEmpty(veh.Campus))
        //            criteria.Add("Campus", veh.Campus);

        //        if (veh.DistanceCovered > 0)
        //            criteria.Add("DistanceCovered", Convert.ToDecimal(veh.DistanceCovered));
        //        if (veh.FuelConsumed > 0)
        //            criteria.Add("FuelConsumed", Convert.ToDecimal(veh.FuelConsumed));
        //        if (veh.Mileage > 0)
        //            criteria.Add("Mileage", Convert.ToDecimal(veh.Mileage));

        //        if (CurrMonth >= 0)
        //            criteria.Add("Month", CurrMonth);
        //        if (CurrYear >= 0)
        //            criteria.Add("Year", CurrYear);
        //        if (!string.IsNullOrEmpty(Month))
        //        {
        //            if (Month == "select")
        //            {
        //                if (CurrMonth >= 0)
        //                    criteria.Remove("Month");
        //                criteria.Add("Month", CurrMonth);
        //            }
        //            else
        //            {
        //                criteria.Remove("Month");
        //                criteria.Add("Month", Convert.ToInt32(Month));
        //            }
        //        }
        //        if (!string.IsNullOrEmpty(Year))
        //        {
        //            criteria.Add("Year", Convert.ToInt32(Year));
        //        }
        //        Dictionary<long, IList<VehicleDistanceFuelReport_vw>> VehicleFuelList = tns.GetVehicleDistanceFuelReport_vwListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
        //        if (VehicleFuelList != null && VehicleFuelList.Count > 0 && VehicleFuelList.FirstOrDefault().Key > 0 && VehicleFuelList.FirstOrDefault().Value.Count > 0)
        //        {
        //            if (ExptXl == 1)
        //            {
        //                var List = VehicleFuelList.First().Value.ToList();
        //                base.ExptToXL(List, "VehicleDistanceFuelReport", (items => new
        //                {
        //                    items.Id,
        //                    items.VehicleId,
        //                    items.Type,
        //                    items.VehicleNo,
        //                    items.Campus,
        //                    DistanceCovered = items.DistanceCovered.ToString(),
        //                    FuelConsumed = items.FuelConsumed.ToString(),
        //                    Mileage = items.DistanceCovered != 0 && items.FuelConsumed != 0 ? (items.DistanceCovered / items.FuelConsumed).Value.ToString("#.#") : "",
        //                    Month = items.Month.ToString(),
        //                    Year = items.Year.ToString(),
        //                }));
        //                return new EmptyResult();
        //            }
        //            else
        //            {
        //                long totalRecords = VehicleFuelList.First().Key;
        //                int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
        //                var jsonData = new
        //                {
        //                    total = totalPages,
        //                    page = page,
        //                    records = totalRecords,
        //                    rows = (
        //                         from items in VehicleFuelList.First().Value
        //                         select new
        //                         {
        //                             i = items.Id,
        //                             cell = new string[] { 
        //                            items.Id.ToString(),
        //                            items.VehicleId.ToString(),
        //                            items.Type,
        //                            items.VehicleNo,
        //                            items.Campus,
        //                             items.DistanceCovered!=null? items.DistanceCovered.ToString():null,
        //                            items.FuelConsumed.ToString(),
        //                            items.FuelConsumed!=null && items.DistanceCovered!=null && items.FuelConsumed!=0 &&  items.DistanceCovered!=0 ? (items.DistanceCovered/items.FuelConsumed).Value.ToString("#.#"):"",
        //                            items.Month.ToString(),
        //                            items.Year.ToString(),
        //                             }
        //                         })
        //                };
        //                return Json(jsonData, JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //        var Empty = new { rows = (new { cell = new string[] { } }) };
        //        return Json(Empty, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
        //        throw ex;
        //    }
        //}

        public JsonResult GetLocationByCampus(string Campus)
        {
            try
            {
                TransportService ts = new TransportService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Campus", Campus);
                Dictionary<long, IList<LocationMaster>> LocationList = ts.GetLocationMasterDetails(0, 9999, "", "", criteria);
                var Location =
                                       (from items in LocationList.FirstOrDefault().Value
                                        select new
                                        {
                                            Text = items.LocationName,
                                            Value = items.LocationName
                                        }).Distinct().ToList();
                return Json(Location, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult GetDriverByCampus(string Campus)
        {
            try
            {
                TransportService ts = new TransportService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Campus", Campus);
                criteria.Add("IsActive", true);
                Dictionary<long, IList<DriverMaster>> DriverList = ts.GetDriverMasterDetails(0, 9999, "", "", criteria);
                var DriverNames =
                                       (from items in DriverList.FirstOrDefault().Value
                                        where items.Name != null && items.Name != ""
                                        select new
                                        {
                                            Text = items.Name,
                                            Value = items.Name
                                        }).Distinct().ToList();
                return Json(DriverNames, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult UploadDocuments(long? Id, string AppName)
        {
            HttpPostedFileBase file = null;
            switch (AppName)
            {
                case "FIT":
                    {
                        file = HttpContext.Request.Files["FCertificate"];
                        break;
                    }
                case "INS":
                    {
                        file = HttpContext.Request.Files["ICertificate"];
                        break;
                    }
                case "DRI":
                    {
                        file = HttpContext.Request.Files["DriverPhoto"];
                        break;
                    }
                case "RC":
                    {
                        file = HttpContext.Request.Files["RCAttachment"];
                        break;
                    }
                case "VMSP":
                    {
                        file = HttpContext.Request.Files["VM_SparePartsUsedfile"];
                        break;
                    }
                case "AMSP":
                    {
                        file = HttpContext.Request.Files["AM_SparePartsUsedfile"];
                        break;
                    }
                case "EMSP":
                    {
                        file = HttpContext.Request.Files["EM_SparePartsUsedfile"];
                        break;
                    }
                case "BMSP":
                    {
                        file = HttpContext.Request.Files["BM_SparePartsUsedfile"];
                        break;
                    }
                default:
                    break;
            }

            if (file.ContentLength != 0 && Id > 0)
            {
                byte[] imageSize = new byte[file.ContentLength];
                file.InputStream.Read(imageSize, 0, (int)file.ContentLength);
                Documents d = new Documents();
                d.EntityRefId = Convert.ToInt64(Id);
                d.FileName = file.FileName;
                d.UploadedOn = DateTime.Now;
                d.UploadedBy = Session["UserId"].ToString();
                d.DocumentData = imageSize;
                d.DocumentSize = file.ContentLength.ToString();
                d.AppName = AppName;
                DocumentsService ds = new DocumentsService();
                ds.CreateOrUpdateDocuments(d);
                return Json("File Uploaded Successfully", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("You have uploded the empty file. Please upload the correct file.", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult uploaddisplay(long Id, string FileName, string AppName)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    DocumentsService ds = new DocumentsService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("EntityRefId", Id);
                    criteria.Add("FileName", FileName);
                    criteria.Add("AppName", AppName);
                    Dictionary<long, IList<Documents>> UploadedFiles = ds.GetDocumentsListWithPaging(0, 1000, string.Empty, string.Empty, criteria);

                    if (UploadedFiles != null && UploadedFiles.FirstOrDefault().Value != null)
                    {
                        IList<Documents> list = UploadedFiles.FirstOrDefault().Value;
                        Documents doc = list.FirstOrDefault();
                        if (doc != null && doc.DocumentData != null)
                        {
                            int startIndx = Convert.ToInt32(doc.FileName.LastIndexOf(".").ToString());
                            int FileLength = Convert.ToInt32(doc.FileName.Length);
                            string fileExtn = doc.FileName.Substring(startIndx, (FileLength - startIndx));
                            return File(doc.DocumentData, GetContentTypeByFileExtension(fileExtn), doc.FileName);
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
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public PartialViewResult Permit()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult AddPermitDetails(Permit per)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportBC insrepo = new TransportBC();
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    per.ValidFrom = DateTime.Parse(Request["ValidFrom"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    per.ValidTo = DateTime.Parse(Request["ValidTo"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    per.CreatedDate = DateTime.Now;
                    per.CreatedBy = userId;
                    insrepo.CreateOrUpdatePermitDetails(per);
                    return Json(per.Id, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }


        //public ActionResult PermitJqGrid(Permit per, int? Id, int rows, string sidx, string sord, int? page = 1)
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //        else
        //        {
        //            TransportService ts = new TransportService();
        //            Dictionary<string, object> criteria = new Dictionary<string, object>();
        //            sord = sord == "desc" ? "Desc" : "Asc";
        //            if (per.VehicleId > 0)
        //                criteria.Add("VehicleId", per.VehicleId);
        //            Dictionary<long, IList<Permit>> PermitDetails = ts.GetPermitDetailsListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
        //            if (PermitDetails != null && PermitDetails.Count > 0)
        //            {
        //                long totalrecords = PermitDetails.First().Key;
        //                int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
        //                var Permit = new
        //                {
        //                    total = totalPages,
        //                    page = page,
        //                    records = totalrecords,
        //                    rows = (from items in PermitDetails.First().Value
        //                            select new
        //                            {
        //                                i = 2,
        //                                cell = new string[] {
        //                       items.Id.ToString(),items.VehicleId.ToString(),items.VehicleNo,items.PermitNo,items.ValidIn,
        //                       items.ValidFrom!=null?items.ValidFrom.Value.ToString("dd/MM/yyyy"):"",
        //                       items.ValidTo!=null?items.ValidTo.Value.ToString("dd/MM/yyyy"):"",
        //                       items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt"):"",
        //                       items.CreatedBy
        //                    }
        //                            })
        //                };
        //                return Json(Permit, JsonRequestBehavior.AllowGet);

        //            }
        //            else return Json(null);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
        //        throw ex;
        //    }
        //}

        public ActionResult PermitJqGrid(Permit per, string ExportType, string ValidFrom, string ValidTo, string CreatedDate, int? Id, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<string, object> ExactCriteria = new Dictionary<string, object>();
                    Dictionary<string, object> LikeCriteria = new Dictionary<string, object>();
                    sord = sord == "desc" ? "Desc" : "Asc";
                    if (per.VehicleId > 0)
                        criteria.Add("VehicleId", per.VehicleId);
                    if (!string.IsNullOrEmpty(per.VehicleNo)) { LikeCriteria.Add("VehicleNo", per.VehicleNo); }
                    if (!string.IsNullOrEmpty(per.PermitNo)) { LikeCriteria.Add("PermitNo", per.PermitNo); }
                    if (!string.IsNullOrEmpty(per.ValidIn)) { LikeCriteria.Add("ValidIn", per.ValidIn); }
                    if (!string.IsNullOrEmpty(per.CreatedBy)) { LikeCriteria.Add("CreatedBy", per.CreatedBy); }
                    Dictionary<long, IList<Permit>> PermitDetails = ts.PermitListWithLikeAndExcactSerachCriteria(page - 1, rows, sord, sidx, ExactCriteria, LikeCriteria);
                    if (PermitDetails != null && PermitDetails.Count > 0)
                    {
                        if (ExportType == "Excel")
                        {
                            var List = PermitDetails.First().Value.ToList();
                            ExptToXL(List, "PermitDetailsList", (items => new
                            {
                                Id = items.Id.ToString(),
                                VehicleId = items.VehicleId.ToString(),
                                VehicleNo = items.VehicleNo,
                                PermitNo = items.PermitNo,
                                ValidIn = items.ValidIn,
                                ValidFrom = items.ValidFrom != null ? items.ValidFrom.Value.ToString("dd/MM/yyyy") : "",
                                ValidTo = items.ValidTo != null ? items.ValidTo.Value.ToString("dd/MM/yyyy") : "",
                                CreatedDate = items.CreatedDate != null ? items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : "",
                                CreatedBy = items.CreatedBy
                            }));
                            return new EmptyResult();
                        }
                        else
                        {
                            long totalrecords = PermitDetails.First().Key;
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var Permit = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalrecords,
                                rows = (from items in PermitDetails.First().Value
                                        select new
                                        {
                                            i = 2,
                                            cell = new string[] {
                               items.Id.ToString(),items.VehicleId.ToString(),items.VehicleNo,items.PermitNo,items.ValidIn,
                               items.ValidFrom!=null?items.ValidFrom.Value.ToString("dd/MM/yyyy"):"",
                               items.ValidTo!=null?items.ValidTo.Value.ToString("dd/MM/yyyy"):"",
                               items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt"):"",
                               items.CreatedBy
                            }
                                        })
                            };
                            return Json(Permit, JsonRequestBehavior.AllowGet);
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

        public ActionResult DriverOTDetails()
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

        [HttpPost]
        public ActionResult AddDriverOTDetails(DriverOTDetails dod)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    dod.OTDate = DateTime.Parse(Request["OTDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    dod.CreatedDate = DateTime.Now;
                    dod.CreatedBy = userId;
                    dod.ModifiedDate = DateTime.Now;
                    dod.ModifiedBy = userId;
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("Campus", dod.Campus);
                    criteria.Add("DriverName", dod.DriverName);
                    criteria.Add("DriverIdNo", dod.DriverIdNo);
                    criteria.Add("OTType", dod.OTType);

                    string To = string.Format("{0:MM/dd/yyyy}", dod.OTDate);
                    DateTime[] fromto = new DateTime[2];
                    fromto[0] = dod.OTDate;
                    fromto[1] = Convert.ToDateTime(To + " " + "23:59:59");
                    criteria.Add("OTDate", fromto);

                    Dictionary<long, IList<DriverOTDetails>> DriverOTDetails = ts.GetDriverOTDetailsListWithPagingAndCriteria(0, 9999, "", "", criteria);
                    if (DriverOTDetails != null && DriverOTDetails.FirstOrDefault().Value != null && DriverOTDetails.FirstOrDefault().Value.Count > 0)
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        ts.CreateOrUpdateDriverOTDetails(dod);
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult DriverOTDetailsListJqGrid(string OTDate, string CreatedDate, DriverOTDetails dod, int? Id, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    sord = sord == "desc" ? "Desc" : "Asc";
                    if (!string.IsNullOrWhiteSpace(dod.Campus)) { criteria.Add("Campus", dod.Campus); }
                    if (!string.IsNullOrWhiteSpace(dod.DriverName)) { criteria.Add("DriverName", dod.DriverName); }
                    if (!string.IsNullOrWhiteSpace(dod.DriverIdNo)) { criteria.Add("DriverIdNo", dod.DriverIdNo); }
                    if (!string.IsNullOrWhiteSpace(dod.OTType)) { criteria.Add("OTType", dod.OTType); }
                    if (dod.Allowance > 0) { criteria.Add("Allowance", dod.Allowance); }
                    if (!string.IsNullOrWhiteSpace(dod.CreatedBy)) { criteria.Add("CreatedBy", dod.CreatedBy); }

                    if (!string.IsNullOrWhiteSpace(OTDate))
                    {
                        OTDate = OTDate.Trim();
                        IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                        DateTime[] OTDatefromto = new DateTime[2];
                        OTDatefromto[0] = DateTime.Parse(OTDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        string To = string.Format("{0:MM/dd/yyyy}", OTDatefromto[0]);
                        OTDatefromto[1] = Convert.ToDateTime(To + " " + "23:59:59");
                        criteria.Add("OTDate", OTDatefromto);
                    }
                    if (!string.IsNullOrWhiteSpace(CreatedDate))
                    {
                        CreatedDate = CreatedDate.Trim();
                        IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                        DateTime[] CreatedDatefromto = new DateTime[2];
                        CreatedDatefromto[0] = DateTime.Parse(CreatedDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        string ToCreatedDate = string.Format("{0:MM/dd/yyyy}", CreatedDatefromto[0]);
                        CreatedDatefromto[1] = Convert.ToDateTime(ToCreatedDate + " " + "23:59:59");
                        criteria.Add("CreatedDate", CreatedDatefromto);
                    }
                    Dictionary<long, IList<DriverOTDetails>> DriverOTDetails = ts.GetDriverOTDetailsListWithPagingAndCriteria(page - 1, rows, sord, sidx, criteria);
                    if (DriverOTDetails != null && DriverOTDetails.Count > 0)
                    {
                        UserService us = new UserService();
                        long totalrecords = DriverOTDetails.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var DriverOT = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in DriverOTDetails.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                               items.Id.ToString(),items.Campus,items.DriverName, items.DriverIdNo,
                               items.OTDate!=null? items.OTDate.ToString("dd/MM/yyyy"):"",
                               items.OTType, items.Allowance.ToString(),
                               items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy"):"",
                               //items.CreatedBy!=null?us.GetUserNameByUserId(items.CreatedBy):"",
                               items.CreatedBy
                            }
                                    })
                        };
                        return Json(DriverOT, JsonRequestBehavior.AllowGet);

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

        public JsonResult GetAutoCompleteDriverNamesByCampus(string Campus, string term)
        {
            try
            {
                TransportService ts = new TransportService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Campus", Campus);
                criteria.Add("Name", term);
                criteria.Add("Status", "Registered");
                Dictionary<long, IList<DriverMaster>> DriverList = ts.GetDriverMasterDetails(0, 9999, string.Empty, string.Empty, criteria);
                var DriverNames = (from u in DriverList.First().Value
                                   where u.Name != null && u.Name != ""
                                   select u.Name).Distinct().ToList();
                return Json(DriverNames, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult GetDriverDetailsByNameAndCampus(string Campus, string DriverName)
        {
            try
            {
                TransportService ts = new TransportService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Campus", Campus);
                criteria.Add("Name", DriverName);
                criteria.Add("IsActive", true);
                Dictionary<long, IList<DriverMaster>> DriverList = ts.GetDriverMasterDetails(0, 9999, "", "", criteria);
                if (DriverList != null && DriverList.FirstOrDefault().Value != null && DriverList.FirstOrDefault().Key > 0)
                {
                    return Json(DriverList.FirstOrDefault().Value[0], JsonRequestBehavior.AllowGet);
                }
                return Json(null, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult DriverOTDetailsReport()
        {
            MastersService ms = new MastersService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            Dictionary<long, IList<CampusMaster>> CampusMaster = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            ViewBag.campusddl = CampusMaster.First().Value;
            int[] years = new int[15];
            DateTime daytime = DateTime.Now;
            int CurYear = daytime.Year;
            int CurMonth = daytime.Month;
            ViewBag.CurYear = CurYear;
            ViewBag.CurMonth = CurMonth;
            CurYear = CurYear - 5;
            for (int i = 0; i < 15; i++)
            {
                years[i] = CurYear + i;
            }
            ViewBag.years = years;

            return View();
        }

        public ActionResult DriverOTDetailsReportListJqGrid(string ExportType, DriverOTDetails dod, int? CurrMonth, int? CurrYear, int? Id, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    sord = sord == "desc" ? "Desc" : "Asc";
                    if (!string.IsNullOrWhiteSpace(dod.Campus)) { criteria.Add("Campus", dod.Campus); }
                    if (!string.IsNullOrWhiteSpace(dod.DriverName)) { criteria.Add("DriverName", dod.DriverName); }
                    if (!string.IsNullOrWhiteSpace(dod.DriverIdNo)) { criteria.Add("DriverIdNo", dod.DriverIdNo); }
                    if (!string.IsNullOrWhiteSpace(dod.OTType)) { criteria.Add("OTType", dod.OTType); }
                    //if (CurrMonth > 0)
                    //{
                    //    DateTime[] month = new DateTime[2];
                    //    month[0] = new DateTime(DateTime.Now.Year, (int)CurrMonth, 1);
                    //    month[1] = month[0].AddMonths(1).AddDays(-1);
                    //    criteria.Add("OTDate", month);
                    //}

                    //if (CurrYear > 0)
                    //{
                    //    DateTime[] year = new DateTime[2];
                    //    year[0] = new DateTime((int)CurrYear, 1, 1);
                    //    year[1] = new DateTime((int)CurrYear, 12, 31);
                    //    criteria.Add("OTDate", year);
                    //}
                    Dictionary<long, IList<DriverOTDetails>> DriverOTDetails = ts.GetDriverOTDetailsListWithPagingAndCriteria(page - 1, rows, sord, sidx, criteria);
                    if (DriverOTDetails != null && DriverOTDetails.Count > 0)
                    {
                        var Rows = (from items in DriverOTDetails.First().Value
                                    // where items.OTDate.Month == CurrMonth && items.OTDate.Year == CurrYear
                                    group items by new { items.Campus, items.DriverName, items.DriverIdNo, items.OTType, Month = items.OTDate.Month, Year = items.OTDate.Year } into gcs
                                    select new
                                    {
                                        cell = new string[] {
                                        gcs.Key.Campus,
                                        gcs.Key.DriverName,
                                        gcs.Key.DriverIdNo,
                                        gcs.Key.OTType,
                                        gcs.Count().ToString(),
                                        gcs.Sum(p => p.Allowance).ToString(),
                                        gcs.Key.Month.ToString(),
                                        gcs.Key.Year.ToString()
                                        }
                                    });
                        if (CurrMonth > 0)
                            Rows = Rows.Where(p => p.cell[6] == CurrMonth.ToString());
                        if (CurrYear > 0)
                            Rows = Rows.Where(p => p.cell[7] == CurrYear.ToString());
                        if (ExportType == "Excel")
                        {
                            var List = Rows.ToList();
                            base.ExptToXL(List, "DriverOTDetailsReport", (items => new
                            {
                                Campus = items.cell[0],
                                DriverName = items.cell[1],
                                DriverIdNo = items.cell[2],
                                OTType = items.cell[3],
                                OTCount = items.cell[4],
                                Allowance = items.cell[5],
                                Month = items.cell[6],
                                Year = items.cell[7],
                            }));
                            return new EmptyResult();
                        }
                        else
                        {
                            long totalrecords = Rows.Count();
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var DriverOT = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalrecords,
                                rows = Rows
                            };
                            return Json(DriverOT, JsonRequestBehavior.AllowGet);
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

        public ActionResult VehicleElectricalMaintenance()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    return PartialView();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult AddVehicleElectricalMaintenance(VehicleElectricalMaintenance vem)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService tsk = new TransportService();
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    vem.EDateOfService = DateTime.Parse(Request["EDateOfService"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    vem.CreatedDate = DateTime.Now;
                    vem.CreatedBy = userId;
                    tsk.CreateOrUpdateVehicleElectricalMaintenance(vem);
                    return Json(vem.Id, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        //public ActionResult VehicleElectricalMaintenanceJqGrid(VehicleElectricalMaintenance vm, int rows, string sidx, string sord, int? page = 1)
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //        else
        //        {
        //            TransportService ts = new TransportService();
        //            Dictionary<string, object> criteria = new Dictionary<string, object>();
        //            sord = sord == "desc" ? "Desc" : "Asc";
        //            if (vm.VehicleId == 0)
        //            {
        //                return Json(null, JsonRequestBehavior.AllowGet);
        //            }
        //            if (vm.VehicleId > 0)
        //                criteria.Add("VehicleId", vm.VehicleId);
        //            Dictionary<long, IList<VehicleElectricalMaintenance>> VehicleElectricalMaintenance = ts.GetVehicleElectricalMaintenanceListWithsearchCriteria(page - 1, rows, sidx, sord, criteria);
        //            if (VehicleElectricalMaintenance != null && VehicleElectricalMaintenance.Count > 0)
        //            {
        //                long totalrecords = VehicleElectricalMaintenance.First().Key;
        //                int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
        //                var ElectricalMaintenanceList = new
        //                {
        //                    total = totalPages,
        //                    page = page,
        //                    records = totalrecords,
        //                    rows = (from items in VehicleElectricalMaintenance.First().Value
        //                            select new
        //                            {
        //                                i = 2,
        //                                cell = new string[] {
        //                       items.Id.ToString(), items.VehicleId.ToString(), items.VehicleNo,
        //                       items.EDateOfService!=null?items.EDateOfService.Value.ToString("dd/MM/yyyy"):"",
        //                       items.EServiceProvider, 
        //                       items.EServiceCost.ToString(), items.EBillNo, items.ESparePartsUsed, items.EDescription,
        //                        "<a style='color:#034af3;text-decoration:underline' onclick = \"ShowEMsparePartsUsed('"+ items.Id + "','"+items.EM_SparePartsUsedfile+ "');\"' >"+items.EM_SparePartsUsedfile+"</a>",
        //                       items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt"):"",
        //                       items.CreatedBy
        //                    }
        //                            })
        //                };
        //                return Json(ElectricalMaintenanceList, JsonRequestBehavior.AllowGet);

        //            }
        //            else return Json(null);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
        //        throw ex;
        //    }
        //}

        public ActionResult VehicleElectricalMaintenanceJqGrid(string ExportType, VehicleElectricalMaintenance vm, string EDateOfService, string EServiceCost, string CreatedDate, string EM_SparePartsUsedfile,
         int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    sord = sord == "desc" ? "Desc" : "Asc";
                    if (vm.VehicleId == 0)
                    {
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                    Dictionary<string, object> ExactCriteria = new Dictionary<string, object>();
                    Dictionary<string, object> LikeCriteria = new Dictionary<string, object>();

                    if (vm.VehicleId > 0)
                        ExactCriteria.Add("VehicleId", vm.VehicleId);
                    if (!string.IsNullOrEmpty(EServiceCost))
                    {
                        decimal ServiceCost = Convert.ToDecimal(EServiceCost);
                        ExactCriteria.Add("EServiceCost", ServiceCost);
                    }
                    if (!string.IsNullOrEmpty(vm.VehicleNo)) { LikeCriteria.Add("VehicleNo", vm.VehicleNo); }
                    if (!string.IsNullOrEmpty(vm.EServiceProvider)) { LikeCriteria.Add("EServiceProvider", vm.EServiceProvider); }
                    if (!string.IsNullOrEmpty(vm.EBillNo)) { LikeCriteria.Add("EBillNo", vm.EBillNo); }
                    if (!string.IsNullOrEmpty(vm.ESparePartsUsed)) { LikeCriteria.Add("ESparePartsUsed", vm.ESparePartsUsed); }
                    if (!string.IsNullOrEmpty(vm.CreatedBy)) { LikeCriteria.Add("CreatedBy", vm.CreatedBy); }
                    if (!string.IsNullOrEmpty(vm.EDescription)) { LikeCriteria.Add("EDescription", vm.EDescription); }
                    if (!string.IsNullOrEmpty(EM_SparePartsUsedfile)) { LikeCriteria.Add("EM_SparePartsUsedfile", EM_SparePartsUsedfile); }
                    Dictionary<long, IList<VehicleElectricalMaintenance>> VehicleElectricalMaintenance = ts.VehicleElectricalMaintenanceListWithLikeAndExcactSerachCriteria(page - 1, rows, sord, sidx, ExactCriteria, LikeCriteria);
                    if (VehicleElectricalMaintenance != null && VehicleElectricalMaintenance.Count > 0)
                    {
                        if (ExportType == "Excel")
                        {
                            var List = VehicleElectricalMaintenance.First().Value.ToList();
                            ExptToXL(List, "VehicleElectricalMaintenanceReportList", (items => new
                            {
                                items.VehicleId,
                                items.VehicleNo,
                                EDateOfService = items.EDateOfService != null ? items.EDateOfService.Value.ToString("dd/MM/yyyy") : "",
                                items.EServiceProvider,
                                ServiceCost = items.EServiceCost,
                                items.EBillNo,
                                items.ESparePartsUsed,
                                items.EDescription,
                                EM_SparePartsUsedfile = "<a style='color:#034af3;text-decoration:underline' onclick = \"ShowEMsparePartsUsed('" + items.Id + "','" + items.EM_SparePartsUsedfile + "');\"' >" + items.EM_SparePartsUsedfile + "</a>",
                                CreatedDate = items.CreatedDate != null ? items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : "",
                                items.CreatedBy
                            }));
                            return new EmptyResult();
                        }
                        else
                        {
                            long totalrecords = VehicleElectricalMaintenance.First().Key;
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var ElectricalMaintenanceList = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalrecords,
                                rows = (from items in VehicleElectricalMaintenance.First().Value
                                        select new
                                        {
                                            i = 2,
                                            cell = new string[] {
                               items.Id.ToString(), items.VehicleId.ToString(), items.VehicleNo,
                               items.EDateOfService!=null?items.EDateOfService.Value.ToString("dd/MM/yyyy"):"",
                               items.EServiceProvider, 
                               items.EServiceCost.ToString(), items.EBillNo, items.ESparePartsUsed, items.EDescription,
                                "<a style='color:#034af3;text-decoration:underline' onclick = \"ShowEMsparePartsUsed('"+ items.Id + "','"+items.EM_SparePartsUsedfile+ "');\"' >"+items.EM_SparePartsUsedfile+"</a>",
                               items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt"):"",
                               items.CreatedBy
                            }
                                        })
                            };
                            return Json(ElectricalMaintenanceList, JsonRequestBehavior.AllowGet);
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

        public ActionResult VehicleBodyMaintenance()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    return PartialView();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult AddVehicleBodyMaintenance(VehicleBodyMaintenance vbm)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService tsk = new TransportService();
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    vbm.BDateOfRepair = DateTime.Parse(Request["BDateOfRepair"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    vbm.CreatedDate = DateTime.Now;
                    vbm.CreatedBy = userId;
                    tsk.CreateOrUpdateVehicleBodyMaintenance(vbm);
                    return Json(vbm.Id, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        //public ActionResult VehicleBodyMaintenanceJqGrid(VehicleBodyMaintenance vm, int rows, string sidx, string sord, int? page = 1)
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //        else
        //        {
        //            TransportService ts = new TransportService();
        //            Dictionary<string, object> criteria = new Dictionary<string, object>();
        //            sord = sord == "desc" ? "Desc" : "Asc";
        //            if (vm.VehicleId == 0)
        //            {
        //                return Json(null, JsonRequestBehavior.AllowGet);
        //            }
        //            if (vm.VehicleId > 0)
        //                criteria.Add("VehicleId", vm.VehicleId);
        //            Dictionary<long, IList<VehicleBodyMaintenance>> VehicleBodyMaintenance = ts.GetVehicleBodyMaintenanceListWithsearchCriteria(page - 1, rows, sidx, sord, criteria);
        //            if (VehicleBodyMaintenance != null && VehicleBodyMaintenance.Count > 0)
        //            {
        //                long totalrecords = VehicleBodyMaintenance.First().Key;
        //                int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
        //                var BodyMaintenanceList = new
        //                {
        //                    total = totalPages,
        //                    page = page,
        //                    records = totalrecords,
        //                    rows = (from items in VehicleBodyMaintenance.First().Value
        //                            select new
        //                            {
        //                                i = 2,
        //                                cell = new string[] {
        //                       items.Id.ToString(), items.VehicleId.ToString(), items.VehicleNo,items.BTypeOfBody,
        //                       items.BDateOfRepair!=null?items.BDateOfRepair.Value.ToString("dd/MM/yyyy"):"",
        //                       items.BTypeOfRepair, items.BPartsRequired, items.BServiceProvider, items.BServiceCost.ToString(),items.BBillNo,items.BDescription,
        //                       "<a style='color:#034af3;text-decoration:underline' onclick = \"ShowBMsparePartsUsed('"+ items.Id + "','"+items.BM_SparePartsUsedfile+ "');\"' >"+items.BM_SparePartsUsedfile+"</a>",
        //                       items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt"):"",
        //                       items.CreatedBy
        //                    }
        //                            })
        //                };
        //                return Json(BodyMaintenanceList, JsonRequestBehavior.AllowGet);

        //            }
        //            else return Json(null);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
        //        throw ex;
        //    }
        //}

        public ActionResult VehicleBodyMaintenanceJqGrid(string ExportType, VehicleBodyMaintenance vm, string BDateOfRepair, string BServiceCost, string CreatedDate, string BM_SparePartsUsedfile,
            int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    sord = sord == "desc" ? "Desc" : "Asc";
                    if (ExportType != "Excel" && vm.VehicleId == 0) { return Json(null, JsonRequestBehavior.AllowGet); }
                    Dictionary<string, object> ExactCriteria = new Dictionary<string, object>();
                    Dictionary<string, object> LikeCriteria = new Dictionary<string, object>();
                    if (vm.VehicleId > 0)
                        ExactCriteria.Add("VehicleId", vm.VehicleId);
                    if (!string.IsNullOrEmpty(vm.VehicleNo)) { LikeCriteria.Add("VehicleNo", vm.VehicleNo); }
                    if (!string.IsNullOrEmpty(vm.BTypeOfBody)) { LikeCriteria.Add("BTypeOfBody", vm.BTypeOfBody); }
                    if (!string.IsNullOrEmpty(vm.BTypeOfRepair)) { LikeCriteria.Add("BTypeOfRepair", vm.BTypeOfRepair); }
                    if (!string.IsNullOrEmpty(vm.BPartsRequired)) { LikeCriteria.Add("BPartsRequired", vm.BPartsRequired); }
                    if (!string.IsNullOrEmpty(vm.BServiceProvider)) { LikeCriteria.Add("BServiceProvider", vm.BServiceProvider); }
                    if (!string.IsNullOrEmpty(vm.BBillNo)) { LikeCriteria.Add("BBillNo", vm.BBillNo); }
                    if (!string.IsNullOrEmpty(vm.BDescription)) { LikeCriteria.Add("BDescription", vm.BDescription); }
                    if (!string.IsNullOrEmpty(vm.CreatedBy)) { LikeCriteria.Add("CreatedBy", vm.CreatedBy); }
                    if (!string.IsNullOrEmpty(BM_SparePartsUsedfile)) { LikeCriteria.Add("BM_SparePartsUsedfile", BM_SparePartsUsedfile); }
                    Dictionary<long, IList<VehicleBodyMaintenance>> VehicleBodyMaintenance = ts.VehicleBodyMaintenanceListWithLikeAndExcactSerachCriteria(page - 1, rows, sord, sidx, ExactCriteria, LikeCriteria);
                    if (VehicleBodyMaintenance != null && VehicleBodyMaintenance.Count > 0)
                    {
                        if (ExportType == "Excel")
                        {
                            var List = VehicleBodyMaintenance.First().Value.ToList();
                            ExptToXL(List, "VehicleBodyMaintenanceReportList", (items => new
                            {
                                items.VehicleId,
                                items.VehicleNo,
                                items.BTypeOfBody,
                                BDateOfRepair = items.BDateOfRepair != null ? items.BDateOfRepair.Value.ToString("dd/MM/yyyy") : "",
                                items.BTypeOfRepair,
                                items.BPartsRequired,
                                items.BServiceProvider,
                                items.BServiceCost,
                                items.BBillNo,
                                items.BDescription,
                                BM_SparePartsUsedfile = "<a style='color:#034af3;text-decoration:underline' onclick = \"ShowBMsparePartsUsed('" + items.Id + "','" + items.BM_SparePartsUsedfile + "');\"' >" + items.BM_SparePartsUsedfile + "</a>",
                                CreatedDate = items.CreatedDate != null ? items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : "",
                                items.CreatedBy
                            }));
                            return new EmptyResult();
                        }
                        else
                        {
                            long totalrecords = VehicleBodyMaintenance.First().Key;
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var BodyMaintenanceList = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalrecords,
                                rows = (from items in VehicleBodyMaintenance.First().Value
                                        select new
                                        {
                                            i = 2,
                                            cell = new string[] {
                               items.Id.ToString(), items.VehicleId.ToString(), items.VehicleNo,items.BTypeOfBody,
                               items.BDateOfRepair!=null?items.BDateOfRepair.Value.ToString("dd/MM/yyyy"):"",
                               items.BTypeOfRepair, items.BPartsRequired, items.BServiceProvider, items.BServiceCost.ToString(),items.BBillNo,items.BDescription,
                               "<a style='color:#034af3;text-decoration:underline' onclick = \"ShowBMsparePartsUsed('"+ items.Id + "','"+items.BM_SparePartsUsedfile+ "');\"' >"+items.BM_SparePartsUsedfile+"</a>",
                               items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt"):"",
                               items.CreatedBy
                            }
                                        })
                            };
                            return Json(BodyMaintenanceList, JsonRequestBehavior.AllowGet);
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

        public ActionResult VehicleTyreMaintenance()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    return PartialView();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult AddVehicleTyreMaintenance(VehicleTyreMaintenance vtm)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService tsk = new TransportService();
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    if (vtm.TyreMaintenanceType == "Maintenance")
                    {
                        vtm.TyreDateOfAlignment = DateTime.Parse(Request["TyreDateOfAlignment"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        vtm.TyreDateOfRotation = DateTime.Parse(Request["TyreDateOfRotation"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        vtm.TyreDateOfWheelService = DateTime.Parse(Request["TyreDateOfWheelService"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    }
                    else if (vtm.TyreMaintenanceType == "New")
                    {
                        vtm.TyreAssignedDate = DateTime.Parse(Request["TyreAssignedDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);

                    }
                    else if (vtm.TyreMaintenanceType == "Service")
                    {
                        vtm.TyreDateOfService = DateTime.Parse(Request["TyreDateOfService"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    }
                    vtm.CreatedDate = DateTime.Now;
                    vtm.CreatedBy = userId;
                    tsk.CreateOrUpdateVehicleTyreMaintenance(vtm);
                    if (vtm.TyreMaintenanceType == "New")
                    {
                        if (!string.IsNullOrWhiteSpace(vtm.TyreNo))
                        {
                            TyreDetails td = tsk.GetTyreDetailsByTyreNo(vtm.TyreNo);
                            td.IsAssigned = true;
                            td.AssignedTo = vtm.VehicleNo;
                            tsk.CreateOrUpdateTyreDetails(td);
                        }
                    }
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        //public ActionResult VehicleTyreMaintenanceJqGrid(VehicleTyreMaintenance vm, int rows, string sidx, string sord, int? page = 1)
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //        else
        //        {
        //            TransportService ts = new TransportService();
        //            Dictionary<string, object> criteria = new Dictionary<string, object>();
        //            sord = sord == "desc" ? "Desc" : "Asc";
        //            if (vm.VehicleId == 0)
        //            {
        //                return Json(null, JsonRequestBehavior.AllowGet);
        //            }
        //            if (vm.VehicleId > 0)
        //                criteria.Add("VehicleId", vm.VehicleId);
        //            Dictionary<long, IList<VehicleTyreMaintenance>> VehicleTyreMaintenance = ts.GetVehicleTyreMaintenanceListWithsearchCriteria(page - 1, rows, sidx, sord, criteria);
        //            if (VehicleTyreMaintenance != null && VehicleTyreMaintenance.Count > 0)
        //            {
        //                long totalrecords = VehicleTyreMaintenance.First().Key;
        //                int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
        //                var TyreMaintenanceList = new
        //                {
        //                    total = totalPages,
        //                    page = page,
        //                    records = totalrecords,
        //                    rows = (from items in VehicleTyreMaintenance.First().Value
        //                            select new
        //                            {
        //                                i = 2,
        //                                cell = new string[] {
        //                       items.Id.ToString(), items.VehicleId.ToString(), items.VehicleNo,items.TyreMaintenanceType, items.TyreLocation, 
        //                       items.TypeOfTyre, items.TyreMake, items.TyreModel, items.TyreSize,
        //                       items.TyreDateOfEntry!=null?items.TyreDateOfEntry.Value.ToString("dd/MM/yyyy"):"",
        //                       items.TyreMilometerReading.ToString(),
        //                       items.TyreCost.ToString(), items.TyreBillNo, 
        //                       items.TyreDateOfAlignment!=null?items.TyreDateOfAlignment.Value.ToString("dd/MM/yyyy"):"",
        //                       items.TyreDateOfRotation!=null?items.TyreDateOfRotation.Value.ToString("dd/MM/yyyy"):"",
        //                       items.TyreDateOfWheelService.ToString(),
        //                       items.TyreServiceCost.ToString() ,items.TyreMaintenanceBillNo, 
        //                       items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt"):"",
        //                       items.CreatedBy
        //                    }
        //                            })
        //                };
        //                return Json(TyreMaintenanceList, JsonRequestBehavior.AllowGet);
        //            }
        //            else return Json(null);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
        //        throw ex;
        //    }
        //}

        public ActionResult VehicleTyreMaintenanceJqGrid(VehicleTyreMaintenance vm, string ExportType, string VehicleId, string TyreDateOfEntry, string TyreMilometerReading, string TyreCost, string TyreDateOfAlignment,
    string TyreDateOfRotation, string TyreDateOfWheelService, string TyreServiceCost, string CreatedDate, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<string, object> ExactCriteria = new Dictionary<string, object>();
                    Dictionary<string, object> LikeCriteria = new Dictionary<string, object>();
                    sord = sord == "desc" ? "Desc" : "Asc";
                    if (ExportType != "Excel" && vm.VehicleId == 0)
                    {
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                    if (ExportType != "Excel" && vm.VehicleId > 0)
                        ExactCriteria.Add("VehicleId", vm.VehicleId);
                    if (!string.IsNullOrEmpty(VehicleId) && ExportType == "Excel")
                    {
                        ExactCriteria.Add("VehicleId", Convert.ToInt32(VehicleId));
                    }
                    if (!string.IsNullOrEmpty(vm.VehicleNo)) { LikeCriteria.Add("VehicleNo", vm.VehicleNo); }
                    if (!string.IsNullOrEmpty(vm.TyreMaintenanceType)) { LikeCriteria.Add("TyreMaintenanceType", vm.TyreMaintenanceType); }
                    if (!string.IsNullOrEmpty(vm.TyreLocation)) { LikeCriteria.Add("TyreLocation", vm.TyreLocation); }
                    if (!string.IsNullOrEmpty(vm.TypeOfTyre)) { LikeCriteria.Add("TypeOfTyre", vm.TypeOfTyre); }
                    if (!string.IsNullOrEmpty(vm.TyreMake)) { LikeCriteria.Add("TyreMake", vm.TyreMake); }
                    if (!string.IsNullOrEmpty(vm.TyreModel)) { LikeCriteria.Add("TyreModel", vm.TyreModel); }
                    if (!string.IsNullOrEmpty(vm.TyreSize)) { LikeCriteria.Add("TyreSize", vm.TyreSize); }
                    if (!string.IsNullOrEmpty(TyreMilometerReading))
                    {
                        ExactCriteria.Add("TyreMilometerReading", Convert.ToInt64(TyreMilometerReading));
                    }
                    if (!string.IsNullOrEmpty(TyreCost))
                    {
                        ExactCriteria.Add("TyreCost", Convert.ToInt64(TyreCost));
                    }
                    if (!string.IsNullOrEmpty(vm.TyreBillNo)) { LikeCriteria.Add("TyreBillNo", vm.TyreBillNo); }
                    if (!string.IsNullOrEmpty(TyreServiceCost))
                    {
                        ExactCriteria.Add("TyreServiceCost", Convert.ToInt64(TyreServiceCost));
                    }
                    if (!string.IsNullOrEmpty(vm.TyreMaintenanceBillNo)) { LikeCriteria.Add("TyreMaintenanceBillNo", vm.TyreMaintenanceBillNo); }
                    if (!string.IsNullOrEmpty(CreatedDate)) { LikeCriteria.Add("CreatedDate", CreatedDate); }
                    if (!string.IsNullOrEmpty(vm.CreatedBy)) { LikeCriteria.Add("CreatedBy", vm.CreatedBy); }

                    Dictionary<long, IList<VehicleTyreMaintenance>> VehicleTyreMaintenance = ts.VehicleTyreMaintenanceListWithLikeAndExcactSerachCriteria(page - 1, rows, sord, sidx, ExactCriteria, LikeCriteria);
                    if (VehicleTyreMaintenance != null && VehicleTyreMaintenance.Count > 0)
                    {

                        if (ExportType == "Excel")
                        {
                            var List = VehicleTyreMaintenance.First().Value.ToList();
                            ExptToXL(List, "VehicleTyreMaintenanceList", (items => new
                            {
                                Id = items.Id.ToString(),
                                VehicleId = items.VehicleId.ToString(),
                                VehicleNo = items.VehicleNo,
                                TyreMaintenanceType = items.TyreMaintenanceType,
                                TyreLocation = items.TyreLocation,
                                TypeOfTyre = items.TypeOfTyre,
                                TyreMake = items.TyreMake,
                                TyreModel = items.TyreModel,
                                TyreSize = items.TyreSize,
                                TyreAssignedDate = items.TyreAssignedDate != null ? items.TyreAssignedDate.Value.ToString("dd/MM/yyyy") : "",
                                TyreMilometerReading = items.TyreMilometerReading.ToString(),
                                TyreReasonForRemoving = items.TyreReasonForRemoving,
                                TyreCost = items.TyreCost.ToString(),
                                TyreBillNo = items.TyreBillNo,
                                TyreDateOfAlignment = items.TyreDateOfAlignment != null ? items.TyreDateOfAlignment.Value.ToString("dd/MM/yyyy") : "",
                                TyreDateOfRotation = items.TyreDateOfRotation != null ? items.TyreDateOfRotation.Value.ToString("dd/MM/yyyy") : "",
                                TyreDateOfWheelService = items.TyreDateOfWheelService.ToString(),
                                TyreServiceCost = items.TyreServiceCost.ToString(),
                                TyreMaintenanceBillNo = items.TyreMaintenanceBillNo,
                                TyreDateOfService = items.TyreDateOfService != null ? items.TyreDateOfService.Value.ToString("dd/MM/yyyy") : "",
                                TyreServiceProvider = items.TyreServiceProvider,
                                CostOfService = items.CostOfService.ToString(),
                                TyreServicedBy = items.TyreServicedBy,
                                TyreServiceBillNo = items.TyreServiceBillNo,
                                CreatedDate = items.CreatedDate != null ? items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : "",
                                CreatedBy = items.CreatedBy

                            }));
                            return new EmptyResult();
                        }
                        else
                        {
                            long totalrecords = VehicleTyreMaintenance.First().Key;
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var TyreMaintenanceList = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalrecords,
                                rows = (from items in VehicleTyreMaintenance.First().Value
                                        select new
                                        {
                                            i = 2,
                                            cell = new string[] {
                               items.Id.ToString(), items.VehicleId.ToString(), items.VehicleNo,items.TyreMaintenanceType, items.TyreLocation, 
                               items.TypeOfTyre, items.TyreMake, items.TyreModel, items.TyreSize,
                               items.TyreAssignedDate!=null?items.TyreAssignedDate.Value.ToString("dd/MM/yyyy"):"",
                               items.TyreMilometerReading.ToString(),
                               items.TyreReasonForRemoving,
                               items.TyreCost.ToString(), items.TyreBillNo, 
                               items.TyreDateOfAlignment!=null?items.TyreDateOfAlignment.Value.ToString("dd/MM/yyyy"):"",
                               items.TyreDateOfRotation!=null?items.TyreDateOfRotation.Value.ToString("dd/MM/yyyy"):"",
                               items.TyreDateOfWheelService.ToString(),
                               items.TyreServiceCost.ToString() ,items.TyreMaintenanceBillNo, 
                               items.TyreDateOfService != null ? items.TyreDateOfService.Value.ToString("dd/MM/yyyy") : "",
                               items.TyreServiceProvider,
                               items.CostOfService.ToString(),
                               items.TyreServicedBy,
                               items.TyreServiceBillNo,
                               items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt"):"",
                               items.CreatedBy
                            }
                                        })
                            };
                            return Json(TyreMaintenanceList, JsonRequestBehavior.AllowGet);
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

        public ActionResult VehicleSubTypeNewMaster()
        {
            return View();
        }


        public ActionResult VehicleSubTypeMasterListJqGrid(VehicleTypeAndSubType vtst, string VehicleType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();

                    criteria.Add("IsActive", true);
                    if (!string.IsNullOrWhiteSpace(VehicleType)) { criteria.Add("VehicleTypeMaster." + "VehicleType", VehicleType); }
                    if (!string.IsNullOrWhiteSpace(vtst.Type)) { criteria.Add("Type", vtst.Type); }
                    if (!string.IsNullOrWhiteSpace(vtst.VehicleNo)) { criteria.Add("VehicleNo", vtst.VehicleNo); }
                    if (!string.IsNullOrWhiteSpace(vtst.FuelType)) { criteria.Add("FuelType", vtst.FuelType); }
                    if (!string.IsNullOrWhiteSpace(vtst.Campus)) { criteria.Add("Campus", vtst.Campus); }
                    if (!string.IsNullOrWhiteSpace(vtst.Purpose)) { criteria.Add("Purpose", vtst.Purpose); }
                    string[] alias = new string[1];
                    alias[0] = "VehicleTypeMaster";
                    sord = sord == "desc" ? "Desc" : "Asc";
                    Dictionary<long, IList<VehicleTypeAndSubType>> VehicleSubTypeMaster = ts.GetVehicleTypeAndSubTypeListWithsearchCriteriaLikeSearch(page - 1, rows, sord, sidx, criteria, alias);
                    if (VehicleSubTypeMaster != null && VehicleSubTypeMaster.Count > 0)
                    {
                        long totalrecords = VehicleSubTypeMaster.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var VehicleSubType = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in VehicleSubTypeMaster.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                               items.Id.ToString(),items.VehicleTypeMaster.VehicleType,items.Type,items.VehicleNo,  items.FuelType, items.Campus,items.Purpose
                            }
                                    })
                        };
                        return Json(VehicleSubType, JsonRequestBehavior.AllowGet);

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
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult VehicleTypeddl()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    Dictionary<long, string> VehicleTypeList = new Dictionary<long, string>();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();

                    Dictionary<long, IList<VehicleTypeMaster>> VTypeList = ts.GetVehicleTypeMasterListWithPagingAndCriteria(0, 9999, null, null, criteria);
                    criteria.Clear();
                    foreach (VehicleTypeMaster veh in VTypeList.First().Value)
                    {
                        VehicleTypeList.Add(veh.Id, veh.VehicleType);
                    }
                    return PartialView("Select", VehicleTypeList);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult GetSourceAndDestinationByRoute(string Campus, string RouteNo)
        {
            TransportService ts = new TransportService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("Campus", Campus);
            criteria.Add("RouteNo", RouteNo);
            Dictionary<long, IList<RouteMaster>> RouteList = ts.GetRouteMasterDetails(0, 9999, null, null, criteria);
            if (RouteList != null && RouteList.Count > 0)
            {
                return Json(RouteList.FirstOrDefault().Value[0], JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        public ActionResult MaintenanceReports()
        {
            return View();
        }

        //public ActionResult MechanicalMaintenanceReport()
        //{
        //    #region BreadCrumb
        //    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
        //    #endregion
        //    return View();
        //}

        //public ActionResult MechanicalMaintenanceReportJqGrid(string ExportType, VehicleMaintenance vm, int rows, string sidx, string sord, int? page = 1)
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //        else
        //        {
        //            TransportService ts = new TransportService();
        //            Dictionary<string, object> criteria = new Dictionary<string, object>();
        //            sord = sord == "desc" ? "Desc" : "Asc";
        //            if (vm.VehicleId > 0)
        //                criteria.Add("VehicleId", vm.VehicleId);
        //            if (!string.IsNullOrEmpty(vm.VehicleNo)) { criteria.Add("VehicleNo", vm.VehicleNo); }
        //            if (!string.IsNullOrEmpty(vm.VehicleMaintenanceType)) { criteria.Add("VehicleMaintenanceType", vm.VehicleMaintenanceType); }
        //            if (!string.IsNullOrEmpty(vm.VehicleBreakdownLocation)) { criteria.Add("VehicleBreakdownLocation", vm.VehicleBreakdownLocation); }
        //            if (!string.IsNullOrEmpty(vm.VehicleMaintenanceDescription)) { criteria.Add("VehicleMaintenanceDescription", vm.VehicleMaintenanceDescription); }
        //            if (!string.IsNullOrEmpty(vm.VehicleServiceProvider)) { criteria.Add("VehicleServiceProvider", vm.VehicleServiceProvider); }
        //            if (vm.VehicleSeviceCost > 0) { criteria.Add("VehicleSeviceCost", Convert.ToDecimal(vm.VehicleSeviceCost)); }
        //            if (!string.IsNullOrEmpty(vm.VehicleServiceBillNo)) { criteria.Add("VehicleServiceBillNo", vm.VehicleServiceBillNo); }
        //            if (!string.IsNullOrEmpty(vm.CreatedBy)) { criteria.Add("CreatedBy", vm.CreatedBy); }
        //            if (!string.IsNullOrEmpty(vm.VehicleSparePartsUsed)) { criteria.Add("VehicleSparePartsUsed", vm.VehicleSparePartsUsed); }
        //            Dictionary<long, IList<VehicleMaintenance>> VehicleMaintenance = ts.GetVehicleMaintenanceListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
        //            if (VehicleMaintenance != null && VehicleMaintenance.Count > 0 && VehicleMaintenance.FirstOrDefault().Value != null)
        //            {
        //                if (ExportType == "Excel")
        //                {
        //                    var List = VehicleMaintenance.First().Value.ToList();
        //                    ExptToXL(List, "MaintenanceReportList", (items => new
        //                    {
        //                        items.VehicleId,
        //                        items.VehicleNo,
        //                        items.VehicleMaintenanceType,
        //                        VehicleDateOfBreakdown = items.VehicleDateOfBreakdown,
        //                        items.VehicleBreakdownLocation,
        //                        VehiclePlannedDateOfService = items.VehiclePlannedDateOfService != null ? items.VehiclePlannedDateOfService.Value.ToString("dd/MM/yyyy") : "",
        //                        VehicleActualDateOfService = items.VehicleActualDateOfService != null ? items.VehicleActualDateOfService.Value.ToString("dd/MM/yyyy") : "",
        //                        items.VehicleServiceProvider,
        //                        VehicleSeviceCost = items.VehicleSeviceCost.ToString(),
        //                        items.VehicleServiceBillNo,
        //                        items.VehicleSparePartsUsed,
        //                        items.CreatedDate,
        //                        items.CreatedBy
        //                    }));
        //                    return new EmptyResult();
        //                }
        //                else
        //                {
        //                    long totalrecords = VehicleMaintenance.First().Key;
        //                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
        //                    var VehicleMaintenanceList = new
        //                    {
        //                        total = totalPages,
        //                        page = page,
        //                        records = totalrecords,
        //                        rows = (from items in VehicleMaintenance.First().Value
        //                                select new
        //                                {
        //                                    i = 2,
        //                                    cell = new string[] {
        //                       items.Id.ToString(), items.VehicleId.ToString(), items.VehicleNo,items.VehicleMaintenanceType,
        //                       items.VehicleDateOfBreakdown!=null?items.VehicleDateOfBreakdown.Value.ToString("dd/MM/yyyy"):"",
        //                       items.VehicleBreakdownLocation, 
        //                       items.VehiclePlannedDateOfService!=null?items.VehiclePlannedDateOfService.Value.ToString("dd/MM/yyyy"):"",
        //                       items.VehicleActualDateOfService!=null?items.VehicleActualDateOfService.Value.ToString("dd/MM/yyyy"):"",
        //                       items.VehicleServiceProvider,
        //                       items.VehicleSeviceCost.ToString(),items.VehicleServiceBillNo,items.VehicleSparePartsUsed,
        //                       items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy"):"",
        //                       items.CreatedBy
        //                    }
        //                                })
        //                    };
        //                    return Json(VehicleMaintenanceList, JsonRequestBehavior.AllowGet);
        //                }
        //            }
        //        }
        //        var Empty = new { rows = (new { cell = new string[] { } }) };
        //        return Json(Empty, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
        //        throw ex;
        //    }
        //}

        //public ActionResult VehicleACMaintenanceReport()
        //{
        //    #region BreadCrumb
        //    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
        //    #endregion
        //    return View();
        //}

        //public ActionResult VehicleACMaintenanceReportJqGrid(string ExportType, VehicleACMaintenance vm, int rows, string sidx, string sord, int? page = 1)
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //        else
        //        {
        //            TransportService ts = new TransportService();
        //            Dictionary<string, object> criteria = new Dictionary<string, object>();
        //            sord = sord == "desc" ? "Desc" : "Asc";
        //            if (vm.VehicleId > 0)
        //                criteria.Add("VehicleId", vm.VehicleId);
        //            Dictionary<long, IList<VehicleACMaintenance>> VehicleMaintenance = ts.GetVehicleACMaintenanceListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
        //            if (VehicleMaintenance != null && VehicleMaintenance.Count > 0)
        //            {
        //                if (ExportType == "Excel")
        //                {
        //                    var List = VehicleMaintenance.First().Value.ToList();
        //                    ExptToXL(List, "VehicleACMaintenanceReportList", (items => new
        //                    {
        //                        items.VehicleId,
        //                        items.VehicleNo,
        //                        items.ACModel,
        //                        items.ACMaintenanceType,
        //                        items.ACDateOfBreakdown,
        //                        items.ACBreakdownLocation,
        //                        ACPlannedDateOfService = items.ACPlannedDateOfService != null ? items.ACPlannedDateOfService.Value.ToString("dd/MM/yyyy") : "",
        //                        ACActualDateOfService = items.ACActualDateOfService != null ? items.ACActualDateOfService.Value.ToString("dd/MM/yyyy") : "",
        //                        items.ACServiceProvider,
        //                        ACServiceCost = items.ACServiceCost.ToString(),
        //                        items.ACServiceBillNo,
        //                        items.ACSparePartsUsed,
        //                        items.CreatedDate,
        //                        items.CreatedBy
        //                    }));
        //                    return new EmptyResult();
        //                }
        //                else
        //                {
        //                    long totalrecords = VehicleMaintenance.First().Key;
        //                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
        //                    var ACMaintenanceList = new
        //                    {
        //                        total = totalPages,
        //                        page = page,
        //                        records = totalrecords,
        //                        rows = (from items in VehicleMaintenance.First().Value
        //                                select new
        //                                {
        //                                    i = 2,
        //                                    cell = new string[] {
        //                       items.Id.ToString(), items.VehicleId.ToString(), items.VehicleNo,items.ACModel, items.ACMaintenanceType,
        //                       items.ACDateOfBreakdown!=null?items.ACDateOfBreakdown.Value.ToString("dd/MM/yyyy"):"",
        //                       items.ACBreakdownLocation,
        //                       items.ACPlannedDateOfService!=null?items.ACPlannedDateOfService.Value.ToString("dd/MM/yyyy"):"",
        //                       items.ACActualDateOfService!=null?items.ACActualDateOfService.Value.ToString("dd/MM/yyyy"):"",
        //                       items.ACServiceProvider,
        //                       items.ACServiceCost.ToString(),items.ACServiceBillNo,items.ACSparePartsUsed ,
        //                       items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt"):"",
        //                       items.CreatedBy
        //                    }
        //                                })
        //                    };
        //                    return Json(ACMaintenanceList, JsonRequestBehavior.AllowGet);
        //                }
        //            }
        //            else return Json(null);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
        //        throw ex;
        //    }
        //}

        //        public ActionResult VehicleElectricalMaintenanceReportJqGrid(string ExportType, VehicleElectricalMaintenance vm,
        //string VehicleNo, string EDateOfService, string EServiceProvider, string EServiceCost, string EBillNo, string ESparePartsUsed,
        //string EDescription, string CreatedDate, string CreatedBy, string EM_SparePartsUsedfile,
        //   int rows, string sidx, string sord, int? page = 1)
        //        {
        //            try
        //            {
        //                string userId = base.ValidateUser();
        //                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //                else
        //                {
        //                    TransportService ts = new TransportService();
        //                    Dictionary<string, object> criteria = new Dictionary<string, object>();
        //                    sord = sord == "desc" ? "Desc" : "Asc";
        //                    if (vm.VehicleId > 0)
        //                        criteria.Add("VehicleId", vm.VehicleId);

        //                    Dictionary<string, object> ExactCriteria = new Dictionary<string, object>();
        //                    Dictionary<string, object> LikeCriteria = new Dictionary<string, object>();
        //                    if (!string.IsNullOrEmpty(EServiceCost))
        //                    {
        //                        decimal ServiceCost = Convert.ToDecimal(EServiceCost);
        //                        ExactCriteria.Add("EServiceCost", ServiceCost);
        //                    }
        //                    if (!string.IsNullOrEmpty(VehicleNo)) { LikeCriteria.Add("VehicleNo", VehicleNo); }
        //                    if (!string.IsNullOrEmpty(EServiceProvider)) { LikeCriteria.Add("EServiceProvider", EServiceProvider); }
        //                    if (!string.IsNullOrEmpty(EBillNo)) { LikeCriteria.Add("EBillNo", EBillNo); }
        //                    if (!string.IsNullOrEmpty(ESparePartsUsed)) { LikeCriteria.Add("ESparePartsUsed", ESparePartsUsed); }
        //                    if (!string.IsNullOrEmpty(CreatedBy)) { LikeCriteria.Add("CreatedBy", CreatedBy); }
        //                    if (!string.IsNullOrEmpty(EDescription)) { LikeCriteria.Add("EDescription", EDescription); }
        //                    if (!string.IsNullOrEmpty(EM_SparePartsUsedfile)) { LikeCriteria.Add("EM_SparePartsUsedfile", EM_SparePartsUsedfile); }
        //                    Dictionary<long, IList<VehicleElectricalMaintenance>> VehicleElectricalMaintenance = ts.VehicleElectricalMaintenanceListWithLikeAndExcactSerachCriteria(page - 1, rows, sord, sidx, ExactCriteria, LikeCriteria);
        //                    if (VehicleElectricalMaintenance != null && VehicleElectricalMaintenance.Count > 0)
        //                    {
        //                        if (ExportType == "Excel")
        //                        {
        //                            var List = VehicleElectricalMaintenance.First().Value.ToList();
        //                            ExptToXL(List, "VehicleElectricalMaintenanceReportList", (items => new
        //                            {
        //                                items.VehicleId,
        //                                items.VehicleNo,
        //                                EDateOfService = items.EDateOfService != null ? items.EDateOfService.Value.ToString("dd/MM/yyyy") : "",
        //                                items.EServiceProvider,
        //                                ServiceCost = items.EServiceCost.ToString(),
        //                                items.EBillNo,
        //                                items.ESparePartsUsed,
        //                                items.EDescription,
        //                                CreatedDate = items.CreatedDate != null ? items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : "",
        //                                items.CreatedBy
        //                            }));
        //                            return new EmptyResult();
        //                        }
        //                        else
        //                        {
        //                            long totalrecords = VehicleElectricalMaintenance.First().Key;
        //                            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
        //                            var ElectricalMaintenanceList = new
        //                            {
        //                                total = totalPages,
        //                                page = page,
        //                                records = totalrecords,
        //                                rows = (from items in VehicleElectricalMaintenance.First().Value
        //                                        select new
        //                                        {
        //                                            i = 2,
        //                                            cell = new string[] {
        //                               items.Id.ToString(), items.VehicleId.ToString(), items.VehicleNo,
        //                               items.EDateOfService!=null?items.EDateOfService.Value.ToString("dd/MM/yyyy"):"",
        //                               items.EServiceProvider, 
        //                               items.EServiceCost.ToString(), items.EBillNo, items.ESparePartsUsed, items.EDescription,
        //                               items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy"):"",
        //                               items.CreatedBy
        //                            }
        //                                        })
        //                            };
        //                            return Json(ElectricalMaintenanceList, JsonRequestBehavior.AllowGet);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        var Empty = new { rows = (new { cell = new string[] { } }) };
        //                        return Json(Empty, JsonRequestBehavior.AllowGet);
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
        //                throw ex;
        //            }
        //        }

        //public ActionResult ElectricalMaintenanceReport()
        //{
        //    #region BreadCrumb
        //    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
        //    #endregion
        //    return View();
        //}

        //public ActionResult VehicleElectricalMaintenanceReportJqGrid(string ExportType, VehicleElectricalMaintenance vm, int rows, string sidx, string sord, int? page = 1)
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //        else
        //        {
        //            TransportService ts = new TransportService();
        //            Dictionary<string, object> criteria = new Dictionary<string, object>();
        //            sord = sord == "desc" ? "Desc" : "Asc";
        //            if (vm.VehicleId > 0)
        //                criteria.Add("VehicleId", vm.VehicleId);
        //            Dictionary<long, IList<VehicleElectricalMaintenance>> VehicleElectricalMaintenance = ts.GetVehicleElectricalMaintenanceListWithsearchCriteria(page - 1, rows, sidx, sord, criteria);
        //            if (VehicleElectricalMaintenance != null && VehicleElectricalMaintenance.Count > 0)
        //            {
        //                if (ExportType == "Excel")
        //                {
        //                    var List = VehicleElectricalMaintenance.First().Value.ToList();
        //                    ExptToXL(List, "VehicleElectricalMaintenanceReportList", (items => new
        //                    {
        //                        items.VehicleId,
        //                        items.VehicleNo,
        //                        EDateOfService = items.EDateOfService != null ? items.EDateOfService.Value.ToString("dd/MM/yyyy") : "",
        //                        items.EServiceProvider,
        //                        EServiceCost = items.EServiceCost.ToString(),
        //                        items.EBillNo,
        //                        items.ESparePartsUsed,
        //                        items.EDescription,
        //                        CreatedDate = items.CreatedDate != null ? items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : "",
        //                        items.CreatedBy
        //                    }));
        //                    return new EmptyResult();
        //                }
        //                else
        //                {
        //                    long totalrecords = VehicleElectricalMaintenance.First().Key;
        //                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
        //                    var ElectricalMaintenanceList = new
        //                    {
        //                        total = totalPages,
        //                        page = page,
        //                        records = totalrecords,
        //                        rows = (from items in VehicleElectricalMaintenance.First().Value
        //                                select new
        //                                {
        //                                    i = 2,
        //                                    cell = new string[] {
        //                       items.Id.ToString(), items.VehicleId.ToString(), items.VehicleNo,
        //                       items.EDateOfService!=null?items.EDateOfService.Value.ToString("dd/MM/yyyy"):"",
        //                       items.EServiceProvider, 
        //                       items.EServiceCost.ToString(), items.EBillNo, items.ESparePartsUsed, items.EDescription,
        //                       items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt"):"",
        //                       items.CreatedBy
        //                    }
        //                                })
        //                    };
        //                    return Json(ElectricalMaintenanceList, JsonRequestBehavior.AllowGet);
        //                }
        //            }
        //            else return Json(null);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
        //        throw ex;
        //    }
        //}

        public ActionResult VehicleACMaintenanceJqGrid(VehicleACMaintenance vm, string ExportType, string ACDateOfBreakdown, string ACPlannedDateOfService, string ACActualDateOfService,
         string ACServiceCost, string CreatedDate, string AM_SparePartsUsedfile, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    sord = sord == "desc" ? "Desc" : "Asc";
                    if (vm.VehicleId == 0)
                    {
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }

                    Dictionary<string, object> ExactCriteria = new Dictionary<string, object>();
                    Dictionary<string, object> LikeCriteria = new Dictionary<string, object>();

                    if (vm.VehicleId > 0)
                        ExactCriteria.Add("VehicleId", vm.VehicleId);

                    if (!string.IsNullOrEmpty(ACServiceCost))
                    {
                        decimal ServiceCost = Convert.ToDecimal(ACServiceCost);
                        ExactCriteria.Add("ACServiceCost", ServiceCost);
                    }
                    if (!string.IsNullOrEmpty(vm.VehicleNo)) { LikeCriteria.Add("VehicleNo", vm.VehicleNo); }
                    if (!string.IsNullOrEmpty(vm.ACMaintenanceType)) { LikeCriteria.Add("ACMaintenanceType", vm.ACMaintenanceType); }
                    if (!string.IsNullOrEmpty(vm.ACBreakdownLocation)) { LikeCriteria.Add("ACBreakdownLocation", vm.ACBreakdownLocation); }
                    if (!string.IsNullOrEmpty(vm.ACModel)) { LikeCriteria.Add("ACModel", vm.ACModel); }
                    if (!string.IsNullOrEmpty(vm.ACMaintenanceDescription)) { LikeCriteria.Add("ACMaintenanceDescription", vm.ACMaintenanceDescription); }
                    if (!string.IsNullOrEmpty(vm.ACServiceProvider)) { LikeCriteria.Add("ACServiceProvider", vm.ACServiceProvider); }
                    if (!string.IsNullOrEmpty(vm.ACServiceBillNo)) { LikeCriteria.Add("ACServiceBillNo", vm.ACServiceBillNo); }
                    if (!string.IsNullOrEmpty(vm.CreatedBy)) { LikeCriteria.Add("CreatedBy", vm.CreatedBy); }
                    if (!string.IsNullOrEmpty(vm.ACSparePartsUsed)) { LikeCriteria.Add("ACSparePartsUsed", vm.ACSparePartsUsed); }
                    if (!string.IsNullOrEmpty(AM_SparePartsUsedfile)) { LikeCriteria.Add("AM_SparePartsUsedfile", AM_SparePartsUsedfile); }

                    Dictionary<long, IList<VehicleACMaintenance>> VehicleMaintenance = ts.VehicleACMaintenanceListWithLikeAndExcactSerachCriteria(page - 1, rows, sord, sidx, ExactCriteria, LikeCriteria);
                    if (VehicleMaintenance != null && VehicleMaintenance.Count > 0)
                    {
                        if (ExportType == "Excel")
                        {
                            var List = VehicleMaintenance.First().Value.ToList();
                            ExptToXL(List, "VehicleACMaintenanceReportList", (items => new
                            {
                                VehicleId = items.VehicleId,
                                Vehicle_No = items.VehicleNo,
                                AC_Model = items.ACModel,
                                AC_Maintenance_Type = items.ACMaintenanceType,
                                AC_Date_Of_Breakdown = items.ACDateOfBreakdown,
                                AC_Breakdown_Location = items.ACBreakdownLocation,
                                AC_Planned_Date_Of_Service = items.ACPlannedDateOfService != null ? items.ACPlannedDateOfService.Value.ToString("dd/MM/yyyy") : "",
                                AC_Actual_Date_Of_Service = items.ACActualDateOfService != null ? items.ACActualDateOfService.Value.ToString("dd/MM/yyyy") : "",
                                AC_Service_Provider = items.ACServiceProvider,
                                AC_Service_Cost = items.ACServiceCost.ToString(),
                                AC_Service_Bill_No = items.ACServiceBillNo,
                                AC_Spare_Parts_Used = items.ACSparePartsUsed,
                                AM_Spare_Parts_Used_file = "<a style='color:#034af3;text-decoration:underline' onclick = \"ShowAMsparePartsUsed('" + items.Id + "','" + items.AM_SparePartsUsedfile + "');\"' >" + items.AM_SparePartsUsedfile + "</a>",
                                Created_Date = items.CreatedDate,
                                Created_By = items.CreatedBy
                            }));
                            return new EmptyResult();
                        }
                        else
                        {

                            long totalrecords = VehicleMaintenance.First().Key;
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var ACMaintenanceList = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalrecords,
                                rows = (from items in VehicleMaintenance.First().Value
                                        select new
                                        {
                                            i = 2,
                                            cell = new string[] {
                               items.Id.ToString(), items.VehicleId.ToString(), items.VehicleNo,items.ACModel, items.ACMaintenanceType,
                               items.ACDateOfBreakdown!=null?items.ACDateOfBreakdown.Value.ToString("dd/MM/yyyy"):"",
                               items.ACBreakdownLocation,
                               items.ACPlannedDateOfService!=null?items.ACPlannedDateOfService.Value.ToString("dd/MM/yyyy"):"",
                               items.ACActualDateOfService!=null?items.ACActualDateOfService.Value.ToString("dd/MM/yyyy"):"",
                               items.ACServiceProvider,
                               items.ACServiceCost.ToString(),items.ACServiceBillNo,items.ACSparePartsUsed ,
                               "<a style='color:#034af3;text-decoration:underline' onclick = \"ShowAMsparePartsUsed('"+ items.Id + "','"+items.AM_SparePartsUsedfile+ "');\"' >"+items.AM_SparePartsUsedfile+"</a>",
                               items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt"):"",
                               items.CreatedBy
                            }
                                        })
                            };
                            return Json(ACMaintenanceList, JsonRequestBehavior.AllowGet);
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

        //public ActionResult VehicleBodyMaintenanceReport()
        //{
        //    #region BreadCrumb
        //    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
        //    #endregion
        //    return View();
        //}

        //public ActionResult VehicleBodyMaintenanceReportJqGrid(string ExportType, VehicleBodyMaintenance vm, int rows, string sidx, string sord, int? page = 1)
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //        else
        //        {
        //            TransportService ts = new TransportService();
        //            Dictionary<string, object> criteria = new Dictionary<string, object>();
        //            sord = sord == "desc" ? "Desc" : "Asc";

        //            if (vm.VehicleId > 0)
        //                criteria.Add("VehicleId", vm.VehicleId);
        //            Dictionary<long, IList<VehicleBodyMaintenance>> VehicleBodyMaintenance = ts.GetVehicleBodyMaintenanceListWithsearchCriteria(page - 1, rows, sidx, sord, criteria);
        //            if (VehicleBodyMaintenance != null && VehicleBodyMaintenance.Count > 0)
        //            {
        //                if (ExportType == "Excel")
        //                {
        //                    var List = VehicleBodyMaintenance.First().Value.ToList();
        //                    ExptToXL(List, "VehicleBodyMaintenanceReportList", (items => new
        //                    {
        //                        items.VehicleId,
        //                        items.VehicleNo,
        //                        items.BTypeOfBody,
        //                        BDateOfRepair = items.BDateOfRepair != null ? items.BDateOfRepair.Value.ToString("dd/MM/yyyy") : "",
        //                        items.BTypeOfRepair,
        //                        items.BPartsRequired,
        //                        items.BServiceProvider,
        //                        BServiceCost = items.BServiceCost.ToString(),
        //                        items.BBillNo,
        //                        items.BDescription,
        //                        CreatedDate = items.CreatedDate != null ? items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : "",
        //                        items.CreatedBy
        //                    }));
        //                    return new EmptyResult();
        //                }
        //                else
        //                {
        //                    long totalrecords = VehicleBodyMaintenance.First().Key;
        //                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
        //                    var BodyMaintenanceList = new
        //                    {
        //                        total = totalPages,
        //                        page = page,
        //                        records = totalrecords,
        //                        rows = (from items in VehicleBodyMaintenance.First().Value
        //                                select new
        //                                {
        //                                    i = 2,
        //                                    cell = new string[] {
        //                       items.Id.ToString(), items.VehicleId.ToString(), items.VehicleNo,items.BTypeOfBody,
        //                       items.BDateOfRepair!=null?items.BDateOfRepair.Value.ToString("dd/MM/yyyy"):"",
        //                       items.BTypeOfRepair, items.BPartsRequired, items.BServiceProvider, items.BServiceCost.ToString(),items.BBillNo,items.BDescription,
        //                       items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt"):"",
        //                       items.CreatedBy
        //                    }
        //                                })
        //                    };
        //                    return Json(BodyMaintenanceList, JsonRequestBehavior.AllowGet);
        //                }
        //            }
        //            else return Json(null);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
        //        throw ex;
        //    }
        //}


        //public ActionResult VehicleTyreMaintenanceReport()
        //{
        //    #region BreadCrumb
        //    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
        //    #endregion
        //    return View();
        //}

        //public ActionResult VehicleTyreMaintenanceReportJqGrid(string ExportType, VehicleTyreMaintenance vm, int rows, string sidx, string sord, int? page = 1)
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //        else
        //        {
        //            TransportService ts = new TransportService();
        //            Dictionary<string, object> criteria = new Dictionary<string, object>();
        //            sord = sord == "desc" ? "Desc" : "Asc";
        //            if (vm.VehicleId > 0)
        //                criteria.Add("VehicleId", vm.VehicleId);
        //            Dictionary<long, IList<VehicleTyreMaintenance>> VehicleTyreMaintenance = ts.GetVehicleTyreMaintenanceListWithsearchCriteria(page - 1, rows, sidx, sord, criteria);
        //            if (VehicleTyreMaintenance != null && VehicleTyreMaintenance.Count > 0)
        //            {
        //                if (ExportType == "Excel")
        //                {
        //                    var List = VehicleTyreMaintenance.First().Value.ToList();
        //                    ExptToXL(List, "VehicleTyreMaintenanceReportList", (items => new
        //                    {
        //                        items.VehicleId,
        //                        items.VehicleNo,
        //                        items.TyreMaintenanceType,
        //                        items.TyreLocation,
        //                        items.TypeOfTyre,
        //                        items.TyreMake,
        //                        items.TyreModel,
        //                        items.TyreSize,
        //                        TyreDateOfEntry = items.TyreDateOfEntry != null ? items.TyreDateOfEntry.Value.ToString("dd/MM/yyyy") : "",
        //                        items.TyreMilometerReading,
        //                        TyreCost = items.TyreCost.ToString(),
        //                        items.TyreBillNo,
        //                        TyreDateOfAlignment = items.TyreDateOfAlignment != null ? items.TyreDateOfAlignment.Value.ToString("dd/MM/yyyy") : "",
        //                        TyreDateOfRotation = items.TyreDateOfRotation != null ? items.TyreDateOfRotation.Value.ToString("dd/MM/yyyy") : "",
        //                        items.TyreDateOfWheelService,
        //                        items.TyreServiceCost,
        //                        items.TyreMaintenanceBillNo,
        //                        CreatedDate = items.CreatedDate != null ? items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : "",
        //                        items.CreatedBy
        //                    }));
        //                    return new EmptyResult();
        //                }
        //                else
        //                {
        //                    long totalrecords = VehicleTyreMaintenance.First().Key;
        //                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
        //                    var TyreMaintenanceList = new
        //                    {
        //                        total = totalPages,
        //                        page = page,
        //                        records = totalrecords,
        //                        rows = (from items in VehicleTyreMaintenance.First().Value
        //                                select new
        //                                {
        //                                    i = 2,
        //                                    cell = new string[] {
        //                       items.Id.ToString(), items.VehicleId.ToString(), items.VehicleNo,items.TyreMaintenanceType, items.TyreLocation, 
        //                       items.TypeOfTyre, items.TyreMake, items.TyreModel, items.TyreSize,
        //                       items.TyreDateOfEntry!=null?items.TyreDateOfEntry.Value.ToString("dd/MM/yyyy"):"",
        //                       items.TyreMilometerReading.ToString(),
        //                       items.TyreCost.ToString(), items.TyreBillNo, 
        //                       items.TyreDateOfAlignment!=null?items.TyreDateOfAlignment.Value.ToString("dd/MM/yyyy"):"",
        //                       items.TyreDateOfRotation!=null?items.TyreDateOfRotation.Value.ToString("dd/MM/yyyy"):"",
        //                       items.TyreDateOfWheelService.ToString(),
        //                       items.TyreServiceCost.ToString() ,items.TyreMaintenanceBillNo, 
        //                       items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt"):"",
        //                       items.CreatedBy
        //                    }
        //                                })
        //                    };
        //                    return Json(TyreMaintenanceList, JsonRequestBehavior.AllowGet);
        //                }
        //            }
        //            else return Json(null);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
        //        throw ex;
        //    }
        //}

        //public ActionResult VehicleTyreMaintenanceReportJqGrid(string ExportType, VehicleTyreMaintenance vm, string TyreModel, string TyreSize, string TyreDateOfEntry, string TyreMilometerReading, string TyreCost, string TyreDateOfAlignment,
        //      string TyreDateOfRotation, string TyreDateOfWheelService, string TyreServiceCost, string CreatedDate, int rows, string sidx, string sord, int? page = 1)
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //        else
        //        {
        //            TransportService ts = new TransportService();
        //            Dictionary<string, object> criteria = new Dictionary<string, object>();
        //            sord = sord == "desc" ? "Desc" : "Asc";
        //            if (vm.VehicleId > 0)
        //                criteria.Add("VehicleId", vm.VehicleId);

        //            Dictionary<string, object> ExactCriteria = new Dictionary<string, object>();
        //            Dictionary<string, object> LikeCriteria = new Dictionary<string, object>();
        //            if (!string.IsNullOrEmpty(TyreCost))
        //            {
        //                decimal ServiceCost = Convert.ToDecimal(TyreCost);
        //                ExactCriteria.Add("TyreCost", ServiceCost);
        //            }

        //            if (!string.IsNullOrEmpty(TyreServiceCost))
        //            {
        //                decimal ServiceCost = Convert.ToDecimal(TyreServiceCost);
        //                ExactCriteria.Add("TyreServiceCost", ServiceCost);
        //            }

        //            if (!string.IsNullOrEmpty(TyreMilometerReading))
        //            {
        //                decimal Radeing = Convert.ToDecimal(TyreMilometerReading);
        //                ExactCriteria.Add("TyreMilometerReading", Radeing);
        //            }
        //            if (!string.IsNullOrEmpty(vm.VehicleNo)) { LikeCriteria.Add("VehicleNo", vm.VehicleNo); }
        //            if (!string.IsNullOrEmpty(vm.TyreMaintenanceType)) { LikeCriteria.Add("TyreMaintenanceType", vm.TyreMaintenanceType); }
        //            if (!string.IsNullOrEmpty(vm.TyreLocation)) { LikeCriteria.Add("TyreLocation", vm.TyreLocation); }
        //            if (!string.IsNullOrEmpty(vm.TypeOfTyre)) { LikeCriteria.Add("TypeOfTyre", vm.TypeOfTyre); }
        //            if (!string.IsNullOrEmpty(vm.TyreMake)) { LikeCriteria.Add("TyreMake", vm.TyreMake); }
        //            if (!string.IsNullOrEmpty(TyreModel)) { LikeCriteria.Add("TyreModel", TyreModel); }
        //            if (!string.IsNullOrEmpty(TyreSize)) { LikeCriteria.Add("TyreSize", TyreSize); }
        //            if (!string.IsNullOrEmpty(vm.TyreBillNo)) { LikeCriteria.Add("TyreBillNo", vm.TyreBillNo); }
        //            if (!string.IsNullOrEmpty(vm.TyreMaintenanceBillNo)) { LikeCriteria.Add("TyreMaintenanceBillNo", vm.TyreMaintenanceBillNo); }
        //            if (!string.IsNullOrEmpty(vm.CreatedBy)) { LikeCriteria.Add("CreatedBy", vm.CreatedBy); }

        //            Dictionary<long, IList<VehicleTyreMaintenance>> VehicleTyreMaintenance = ts.VehicleTyreMaintenanceListWithLikeAndExcactSerachCriteria(page - 1, rows, sord, sidx, ExactCriteria, LikeCriteria);
        //            if (VehicleTyreMaintenance != null && VehicleTyreMaintenance.Count > 0)
        //            {
        //                if (ExportType == "Excel")
        //                {
        //                    var List = VehicleTyreMaintenance.First().Value.ToList();
        //                    ExptToXL(List, "VehicleTyreMaintenanceReportList", (items => new
        //                    {
        //                        items.VehicleId,
        //                        items.VehicleNo,
        //                        items.TyreMaintenanceType,
        //                        items.TyreLocation,
        //                        items.TypeOfTyre,
        //                        items.TyreMake,
        //                        items.TyreModel,
        //                        items.TyreSize,
        //                        TyreAssignedDate = items.TyreAssignedDate != null ? items.TyreAssignedDate.Value.ToString("dd/MM/yyyy") : "",
        //                        items.TyreMilometerReading,
        //                        items.TyreReasonForRemoving,
        //                        TyreCost = items.TyreCost.ToString(),
        //                        items.TyreBillNo,
        //                        TyreDateOfAlignment = items.TyreDateOfAlignment != null ? items.TyreDateOfAlignment.Value.ToString("dd/MM/yyyy") : "",
        //                        TyreDateOfRotation = items.TyreDateOfRotation != null ? items.TyreDateOfRotation.Value.ToString("dd/MM/yyyy") : "",
        //                        items.TyreDateOfWheelService,
        //                        TyreServiceCost = items.TyreServiceCost.ToString(),
        //                        items.TyreMaintenanceBillNo,
        //                        TyreDateOfService = items.TyreDateOfService != null ? items.TyreDateOfService.Value.ToString("dd/MM/yyyy") : "",
        //                        TyreServiceProvider = items.TyreServiceProvider,
        //                        CostOfService = items.CostOfService.ToString(),
        //                        TyreServicedBy = items.TyreServicedBy,
        //                        TyreServiceBillNo = items.TyreServiceBillNo,
        //                        CreatedDate = items.CreatedDate != null ? items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : "",
        //                        items.CreatedBy
        //                    }));
        //                    return new EmptyResult();
        //                }
        //                else
        //                {
        //                    long totalrecords = VehicleTyreMaintenance.First().Key;
        //                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
        //                    var TyreMaintenanceList = new
        //                    {
        //                        total = totalPages,
        //                        page = page,
        //                        records = totalrecords,
        //                        rows = (from items in VehicleTyreMaintenance.First().Value
        //                                select new
        //                                {
        //                                    i = 2,
        //                                    cell = new string[] {
        //                       items.Id.ToString(), items.VehicleId.ToString(), items.VehicleNo,items.TyreMaintenanceType, items.TyreLocation, 
        //                       items.TypeOfTyre, items.TyreMake, items.TyreModel, items.TyreSize,
        //                       items.TyreAssignedDate!=null?items.TyreAssignedDate.Value.ToString("dd/MM/yyyy"):"",
        //                       items.TyreMilometerReading.ToString(),
        //                       items.TyreReasonForRemoving,
        //                       items.TyreCost.ToString(), items.TyreBillNo, 
        //                       items.TyreDateOfAlignment!=null?items.TyreDateOfAlignment.Value.ToString("dd/MM/yyyy"):"",
        //                       items.TyreDateOfRotation!=null?items.TyreDateOfRotation.Value.ToString("dd/MM/yyyy"):"",
        //                       items.TyreDateOfWheelService.ToString(),
        //                       items.TyreServiceCost.ToString(),
        //                       items.TyreMaintenanceBillNo, 
        //                       items.TyreDateOfService != null ? items.TyreDateOfService.Value.ToString("dd/MM/yyyy") : "",
        //                       items.TyreServiceProvider,
        //                       items.CostOfService.ToString(),
        //                       items.TyreServicedBy,
        //                       items.TyreServiceBillNo,
        //                       items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt"):"",
        //                       items.CreatedBy
        //                    }
        //                                })
        //                    };
        //                    return Json(TyreMaintenanceList, JsonRequestBehavior.AllowGet);
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

        public JsonResult FillRouteByCampus(string Campus)
        {
            TransportService ts = new TransportService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            if (!string.IsNullOrWhiteSpace(Campus)) criteria.Add("Campus", Campus);
            Dictionary<long, IList<RouteMaster>> RouteMaster = ts.GetRouteMasterDetails(0, 9999, string.Empty, string.Empty, criteria);
            if (RouteMaster != null && RouteMaster.First().Value != null && RouteMaster.First().Value.Count > 0)
            {
                var RouteList = (
                         from items in RouteMaster.First().Value
                         select new
                         {
                             Text = items.RouteNo,
                             Value = items.RouteNo
                         }).Distinct().ToList();
                return Json(RouteList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DeleteDriverOTDetails(DriverOTDetails DOD)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportBC tbc = new TransportBC();
                    if (DOD.Id > 0)
                    {
                        tbc.DeleteDriverOTDetails(DOD);
                    }
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult DeletePermitDetailsById(Permit per)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportBC tbc = new TransportBC();
                    if (per.Id > 0)
                    {
                        tbc.DeletePermitDetailsById(per);
                    }
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult DeleteFitnessCertificateById(FitnessCertificate fc)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportBC tbc = new TransportBC();
                    if (fc.Id > 0)
                    {
                        tbc.DeleteFitnessCertificateById(fc);
                    }
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult DeleteVehicleMaintenanceById(VehicleMaintenance vm)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportBC tbc = new TransportBC();
                    if (vm.Id > 0)
                    {
                        tbc.DeleteVehicleMaintenanceById(vm);
                    }
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult DeleteVehicleACMaintenanceById(VehicleACMaintenance vacm)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportBC tbc = new TransportBC();
                    if (vacm.Id > 0)
                    {
                        tbc.DeleteVehicleACMaintenanceById(vacm);
                    }
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult DeleteVehicleElectricalMaintenanceById(VehicleElectricalMaintenance vem)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportBC tbc = new TransportBC();
                    if (vem.Id > 0)
                    {
                        tbc.DeleteVehicleElectricalMaintenanceById(vem);
                    }
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult DeleteVehicleBodyMaintenanceById(VehicleBodyMaintenance vbm)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportBC tbc = new TransportBC();
                    if (vbm.Id > 0)
                    {
                        tbc.DeleteVehicleBodyMaintenanceById(vbm);
                    }
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult DeleteVehicleTyreMaintenanceById(VehicleTyreMaintenance vtm)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportBC tbc = new TransportBC();
                    if (vtm.Id > 0)
                    {
                        tbc.DeleteVehicleTyreMaintenanceById(vtm);
                    }
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult DeleteVehicleSubTypeMasterById(VehicleSubTypeMaster vstm)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportBC tbc = new TransportBC();
                    if (vstm.Id > 0)
                    {
                        tbc.DeleteVehicleSubTypeMasterById(vstm);
                    }
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        #region Driver OT Allowance
        public ActionResult DriverOTAllowanceReport()
        {
            MastersService ms = new MastersService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            Dictionary<long, IList<CampusMaster>> CampusMaster = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            ViewBag.campusddl = CampusMaster.First().Value;
            int[] years = new int[15];
            DateTime daytime = DateTime.Now;
            int CurYear = daytime.Year;
            int CurMonth = daytime.Month;
            ViewBag.CurYear = CurYear;
            ViewBag.CurMonth = CurMonth;
            CurYear = CurYear - 5;
            for (int i = 0; i < 15; i++)
            {
                years[i] = CurYear + i;
            }
            ViewBag.years = years;
            return View();
        }
        public ActionResult DriverOTAllowanceJqGrid(string DriverName, string DriverIdNo, string Campus, int? CurrMonth, int? CurrYear, int rows, string sidx, string sord, int? page = 1, int? ExptXl = 0)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                TransportService tns = new TransportService();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (!string.IsNullOrEmpty(DriverName))
                    criteria.Add("DriverName", DriverName.Trim());
                if (!string.IsNullOrEmpty(DriverIdNo))
                    criteria.Add("DriverIdNo", DriverIdNo.Trim());
                if (!string.IsNullOrEmpty(Campus))
                    criteria.Add("Campus", Campus);
                if (CurrMonth >= 0)
                    criteria.Add("Month", CurrMonth);
                if (CurrYear >= 0)
                    criteria.Add("Year", CurrYear);
                Dictionary<long, IList<DriverOTAllowance_vw>> DriverOTAllowanceList = tns.GetDriverOTAllowanceListWithPagingAndCriteria(page - 1, rows, sord, sidx, criteria);
                if (DriverOTAllowanceList != null && DriverOTAllowanceList.Count > 0 && DriverOTAllowanceList.FirstOrDefault().Key > 0 && DriverOTAllowanceList.FirstOrDefault().Value.Count > 0)
                {
                    if (ExptXl == 1)
                    {
                        var List = DriverOTAllowanceList.First().Value.ToList();
                        base.ExptToXL(List, "DriverOTAllowanceReport", (items => new
                        {
                            items.Id,
                            items.Campus,
                            items.DriverName,
                            items.DriverIdNo,
                            items.Month,
                            items.Year,
                            items.Evening,
                            EveningAllowance = items.EveningAllowance.ToString(),
                            items.Night,
                            NightAllowance = items.NightAllowance.ToString(),
                            items.OutStation,
                            OutStationAllowance = items.OutStationAllowance.ToString(),
                            items.Holiday,
                            HolidayAllowance = items.HolidayAllowance.ToString(),
                            items.Remedial,
                            RemedialAllowance = items.RemedialAllowance.ToString(),
                            items.TotalOTCount,
                            TotalAllowance = items.TotalAllowance.ToString()
                        }));
                        return new EmptyResult();
                    }
                    else
                    {
                        long totalRecords = DriverOTAllowanceList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                        var jsonData = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalRecords,
                            rows = (
                                 from items in DriverOTAllowanceList.First().Value
                                 select new
                                 {
                                     i = items.Id,
                                     cell = new string[] { 
                                     items.Id.ToString(),
                                     items.Campus,
                                     items.DriverName,
                                     items.DriverIdNo,
                                     items.Month.ToString(),
                                     items.Year.ToString(),
                                     items.Evening.ToString(),
                                     items.EveningAllowance.ToString(),
                                     items.Night.ToString(),
                                     items.NightAllowance.ToString(),
                                     items.OutStation.ToString(),
                                     items.OutStationAllowance.ToString(),
                                     items.Holiday.ToString(),
                                     items.HolidayAllowance.ToString(),
                                     items.Remedial.ToString(),
                                     items.RemedialAllowance.ToString(),
                                     items.TotalOTCount.ToString(),
                                     items.TotalAllowance.ToString()
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
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        #endregion

        public ActionResult DriverOTAllowanceReportNew()
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

        public ActionResult DriverOTAllowanceJqGridNew(string DriverName, string DriverIdNo, string Campus, string FromDate, string ToDate, int rows, string sidx, string sord, int? page = 1, int? ExptXl = 0)
        {
            try
            {
                IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                DateTime? fdate = null;
                DateTime? tdate = null;
                if (!string.IsNullOrWhiteSpace(FromDate))
                {
                    fdate = DateTime.Parse(FromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                }
                if (!string.IsNullOrWhiteSpace(ToDate))
                {
                    tdate = DateTime.Parse(ToDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    string To = string.Format("{0:dd/MM/yyyy}", tdate);
                    tdate = Convert.ToDateTime(To + " " + "23:59:59.000");
                }
                string query = "";
                query = query + "SELECT     ROW_NUMBER() OVER (ORDER BY a.Campus) AS Id, a.Campus, a.DriverName, a.DriverIdNo,  SUM(Evening) Evening, SUM(a.EveningAllowance) ";
                query = query + "EveningAllowance, SUM(a.Night) Night, SUM(a.NightAllowance) NightAllowance, SUM(a.OutStation) OutStation, SUM(a.OutStationAllowance) OutStationAllowance, SUM(a.Holiday) ";
                query = query + "Holiday, SUM(a.HolidayAllowance) HolidayAllowance, SUM(a.RemedialTrip) RemedialTrip, SUM(a.RemedialAllowance) RemedialAllowance, SUM(a.TotalOTCount) TotalOTCount, ";
                query = query + "(SUM(a.EveningAllowance) + SUM(a.NightAllowance) + SUM(a.OutStationAllowance) + SUM(a.HolidayAllowance) + SUM(a.RemedialAllowance)) AS TotalAllowance ";
                query = query + "FROM         (SELECT     Campus, DriverName, DriverIdNo, CASE WHEN OTType = 'Evening' THEN count(Id) ELSE 0 END AS Evening, ";
                query = query + "CASE WHEN OTType = 'Evening' THEN SUM(Allowance) ELSE 0 END AS EveningAllowance, CASE WHEN OTType = 'Night' THEN count(Id) ";
                query = query + "ELSE 0 END AS Night, CASE WHEN OTType = 'Night' THEN SUM(Allowance) ELSE 0 END AS NightAllowance, ";
                query = query + "CASE WHEN OTType = 'Out Station' THEN count(Id) ELSE 0 END AS OutStation, CASE WHEN OTType = 'Out Station' THEN SUM(Allowance) ";
                query = query + "ELSE 0 END AS OutStationAllowance, CASE WHEN OTType = 'Holiday' THEN count(Id) ELSE 0 END AS Holiday, ";
                query = query + "CASE WHEN OTType = 'Holiday' THEN SUM(Allowance) ELSE 0 END AS HolidayAllowance, CASE WHEN OTType = 'Remedial Trip' THEN count(Id) ";
                query = query + "ELSE 0 END AS RemedialTrip, CASE WHEN OTType = 'Remedial Trip' THEN SUM(Allowance) ELSE 0 END AS RemedialAllowance, count(Id) TotalOTCount ";
                query = query + "FROM          DriverOTDetails ";
                if (!string.IsNullOrWhiteSpace(DriverName) || !string.IsNullOrWhiteSpace(DriverIdNo) || !string.IsNullOrWhiteSpace(Campus) || fdate != null || tdate != null)
                {
                    query = query + "where ";
                    if (!string.IsNullOrWhiteSpace(Campus))
                        query = query + " Campus = '" + Campus + "' ";
                    if (!string.IsNullOrWhiteSpace(DriverName))
                        query = query + " and DriverName = '" + DriverName + "' ";
                    if (!string.IsNullOrWhiteSpace(DriverIdNo))
                        query = query + " and DriverIdNo = '" + DriverIdNo + "' ";
                    if (fdate != null && tdate != null)
                        query = query + " and OTDate between '" + fdate + "' and '" + tdate + "' ";
                }
                query = query + "GROUP BY Campus, DriverName, DriverIdNo, OTType) a ";
                query = query + "GROUP BY a.Campus, a.DriverName, a.DriverIdNo";
                TransportService ts = new TransportService();
                DataTable DriverAllowance = ts.GetDriverAllowance(query);

                List<DataRow> DriverAllowanceList = null;
                if (DriverAllowance != null)
                {
                    DriverAllowanceList = DriverAllowance.AsEnumerable().ToList();

                    if (DriverAllowanceList.Count > 0)
                    {
                        if (ExptXl == 1)
                        {
                            ExptToXL(DriverAllowanceList, "DriverOTAllowanceList", (items => new
                            {
                                Id = items.ItemArray[0].ToString(),
                                Campus = items.ItemArray[1].ToString(),
                                DriverName = items.ItemArray[2].ToString(),
                                DriverIdNo = items.ItemArray[3].ToString(),
                                FromDate = FromDate.ToString(),
                                Todate = ToDate.ToString(),
                                Evening = items.ItemArray[4].ToString(),
                                EveningAllowance = items.ItemArray[5].ToString(),
                                Night = items.ItemArray[6].ToString(),
                                NightAllowance = items.ItemArray[7].ToString(),
                                OutStation = items.ItemArray[8].ToString(),
                                OutStationAllowance = items.ItemArray[9].ToString(),
                                Holiday = items.ItemArray[10].ToString(),
                                HolidayAllowance = items.ItemArray[11].ToString(),
                                Remedial = items.ItemArray[12].ToString(),
                                RemedialAllowance = items.ItemArray[13].ToString(),
                                TotalOTCount = items.ItemArray[14].ToString(),
                                TotalAllowance = items.ItemArray[15].ToString(),
                            }));
                            return new EmptyResult();
                        }
                        else
                        {
                            long totalRecords = DriverAllowanceList.Count;
                            int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                            var jsonData = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalRecords,
                                rows = (
                                     from items in DriverAllowanceList
                                     select new
                                     {
                                         cell = new string[] { 
                                     items.ItemArray[0].ToString(),
                                     items.ItemArray[1].ToString(),
                                     items.ItemArray[2].ToString(),
                                     items.ItemArray[3].ToString(),
                                     !string.IsNullOrWhiteSpace(FromDate)? FromDate.ToString():"",
                                     !string.IsNullOrWhiteSpace(ToDate)? ToDate.ToString():"",
                                     items.ItemArray[4].ToString(),
                                     items.ItemArray[5].ToString(),
                                     items.ItemArray[6].ToString(),
                                     items.ItemArray[7].ToString(),
                                     items.ItemArray[8].ToString(),
                                     items.ItemArray[9].ToString(),
                                     items.ItemArray[10].ToString(),
                                     items.ItemArray[11].ToString(),
                                     items.ItemArray[12].ToString(),
                                     items.ItemArray[13].ToString(),
                                     items.ItemArray[14].ToString(),
                                     items.ItemArray[15].ToString(),
                                     }
                                     })
                            };
                            return Json(jsonData, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                var AssLst = new { rows = (new { cell = new string[] { } }) };
                return Json(AssLst, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        //public JsonResult GetVehicleNo(string term)
        //{
        //    try
        //    {
        //        TransportService ts = new TransportService();
        //        Dictionary<string, object> criteria = new Dictionary<string, object>();
        //        criteria.Add("VehicleNo", term);
        //        criteria.Add("IsActive", true);
        //        Dictionary<long, IList<VehicleSubTypeMaster>> VehicleList = ts.GetVehicleSubTypeMasterListWithPagingAndCriteriaLikeSearch(0, 9999, string.Empty, string.Empty, criteria);
        //        var DriverNames = (from u in VehicleList.First().Value
        //                           where u.VehicleNo != null && u.VehicleNo != ""
        //                           select u.VehicleNo).Distinct().ToArray();
        //        return Json(DriverNames, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
        //        throw ex;
        //    }
        //}
        public JsonResult GetVehicleNo(string term, string Campus)
        {
            try
            {
                TransportService ts = new TransportService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("VehicleNo", term);
                if (!string.IsNullOrEmpty(Campus))
                    criteria.Add("Campus", Campus);
                Dictionary<long, IList<VehicleSubTypeMaster>> VehicleList = ts.GetVehicleSubTypeMasterListWithPagingAndCriteriaLikeSearch(0, 9999, string.Empty, string.Empty, criteria);
                var DriverNames = (from u in VehicleList.First().Value
                                   where u.VehicleNo != null && u.VehicleNo != ""
                                   select u.VehicleNo).Distinct().ToArray();
                return Json(DriverNames, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult VehicleDistanceCoveredNew()
        {
            #region BreadCrumb
            string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
            #endregion
            return View();
        }


        public JsonResult GetAutoCompleteLocationByCampus(string Campus, string term)
        {
            try
            {
                TransportService ts = new TransportService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                // criteria.Add("Campus", Campus);
                criteria.Add("LocationName", term);
                Dictionary<long, IList<LocationMaster>> LocationList = ts.GetLocationMasterDetailsWithPagingLikeSearch(0, 9999, "", "", criteria);
                var Location = (from u in LocationList.First().Value
                                where u.LocationName != null && u.LocationName != ""
                                select u.LocationName).Distinct().ToList();
                return Json(Location, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public JsonResult GetVehicleDetailsByVehicleNo(string VehicleNo)
        {
            try
            {
                TransportService ts = new TransportService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("VehicleNo", VehicleNo);
                criteria.Add("IsActive", true);
                Dictionary<long, IList<VehicleSubTypeMaster>> VehicleList = ts.GetVehicleSubTypeMasterListWithPagingAndCriteriaLikeSearch(0, 9999, string.Empty, string.Empty, criteria);
                var VehicleDetails = (from u in VehicleList.First().Value
                                      where u.VehicleNo != null && u.VehicleNo != ""
                                      select new { u.VehicleNo, u.Campus, u.Id, u.Type }).Distinct().ToList();
                return Json(VehicleDetails, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public JsonResult GetTripPurposeMaster(string term)
        {
            try
            {
                TransportService ts = new TransportService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Purpose", term);
                Dictionary<long, IList<TripPurposeMaster>> PurposeList = ts.GetTripPurposeMasterDetailsWithPagingLikeSearch(0, 9999, string.Empty, string.Empty, criteria);
                var Purpose = (from u in PurposeList.First().Value
                               where u.Purpose != null && u.Purpose != ""
                               select u.Purpose).Distinct().ToArray();
                return Json(Purpose, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult TransportMasters()
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

        //public ActionResult VehicleReport()
        //{
        //    #region BreadCrumb
        //    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
        //    #endregion
        //    return View();
        //}

        //public ActionResult VehicleReportJqGrid(string FromDate, string ToDate, VehicleReport vr, string ExportType, int rows, string sidx, string sord, int? page = 1)
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //        else
        //        {
        //            IList<VehicleReport> VehicleReport = new List<VehicleReport>();
        //            TransportService ts = new TransportService();
        //            Dictionary<string, object> VehicleCriteria = new Dictionary<string, object>();
        //            if (!string.IsNullOrWhiteSpace(vr.Type)) VehicleCriteria.Add("Type", vr.Type);
        //            if (!string.IsNullOrWhiteSpace(vr.VehicleNo)) VehicleCriteria.Add("VehicleNo", vr.VehicleNo);
        //            if (!string.IsNullOrWhiteSpace(vr.Campus)) VehicleCriteria.Add("Campus", vr.Campus);

        //            string[] alias = new string[1];
        //            alias[0] = "VehicleTypeMaster";
        //            Dictionary<long, IList<VehicleReport>> VehicleList = ts.GetVehicleReportListWithsearchCriteriaLikeSearch(page - 1, rows, string.Empty, string.Empty, VehicleCriteria, alias);
        //            if (VehicleList != null && VehicleList.FirstOrDefault().Value != null && VehicleList.FirstOrDefault().Value.Count() > 0)
        //            {
        //                Dictionary<string, object> criteria = new Dictionary<string, object>();
        //                IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);

        //                DateTime?[] FromTo = new DateTime?[2];
        //                DateTime tdate = DateTime.Now;
        //                if (!string.IsNullOrWhiteSpace(FromDate))
        //                {
        //                    FromTo[0] = DateTime.Parse(FromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
        //                }
        //                if (!string.IsNullOrWhiteSpace(ToDate))
        //                {
        //                    tdate = DateTime.Parse(ToDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
        //                    string To = string.Format("{0:MM/dd/yyyy}", tdate);
        //                    tdate = Convert.ToDateTime(To + " " + "23:59:59");
        //                }

        //                FromTo[1] = tdate;

        //                foreach (var item in VehicleList.FirstOrDefault().Value)
        //                {
        //                    VehicleReport vehrep = new VehicleReport();
        //                    vehrep.Id = item.Id;
        //                    vehrep.VehicleId = Convert.ToInt32(item.Id);
        //                    vehrep.Type = item.Type;
        //                    vehrep.VehicleNo = item.VehicleNo;
        //                    vehrep.Campus = item.Campus;

        //                    criteria.Clear();
        //                    criteria.Add("VehicleId", item.Id);
        //                    if (FromTo[0] != null && FromTo[1] != null)
        //                        criteria.Add("InDateTime", FromTo);
        //                    Dictionary<long, IList<VehicleDistanceCovered>> DistanceList = ts.GetVehicleDistanceCoveredListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
        //                    if (DistanceList != null && DistanceList.FirstOrDefault().Value != null && DistanceList.FirstOrDefault().Value.Count() > 0)
        //                    {
        //                        var DistanceArray = (from items in DistanceList.FirstOrDefault().Value
        //                                             group items by items.VehicleId into g
        //                                             select g.Sum(p => p.DistanceCovered)).ToArray();
        //                        if (DistanceArray != null)
        //                            vehrep.DistanceCovered = DistanceArray[0];
        //                    }

        //                    criteria.Clear();
        //                    criteria.Add("VehicleId", item.Id);
        //                    if (FromTo[0] != null && FromTo[1] != null)
        //                        criteria.Add("CreatedDate", FromTo);
        //                    Dictionary<long, IList<VehicleFuelManagement>> VehicleFuelManagement = ts.GetVehicleFuelManagementListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);

        //                    if (VehicleFuelManagement != null && VehicleFuelManagement.FirstOrDefault().Value != null && VehicleFuelManagement.FirstOrDefault().Value.Count() > 0)
        //                    {
        //                        var FuelArray = (from items in VehicleFuelManagement.FirstOrDefault().Value
        //                                         group items by items.VehicleId into g
        //                                         select new
        //                                         {
        //                                             cell = new decimal?[]{
        //                                                 g.Sum(p => p.FuelQuantity),
        //                                                 g.Sum(p => p.TotalPrice),
        //                                         }
        //                                         }).ToArray();
        //                        if (FuelArray != null)
        //                            vehrep.FuelConsumed = FuelArray[0].cell[0];
        //                        vehrep.FuelCost = FuelArray[0].cell[1];
        //                    }

        //                    vehrep.Mileage = vehrep.DistanceCovered != 0 && vehrep.FuelConsumed != 0 ? Convert.ToDecimal((vehrep.DistanceCovered / vehrep.FuelConsumed)) : 0;

        //                    criteria.Clear();
        //                    criteria.Add("VehicleId", (Int64)item.Id);
        //                    if (FromTo[0] != null && FromTo[1] != null)
        //                        criteria.Add("CreatedDate", FromTo);
        //                    Dictionary<long, IList<FitnessCertificate>> FitnessCertificate = ts.GetFitnessCertificateDetailsWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);

        //                    if (FitnessCertificate != null && FitnessCertificate.FirstOrDefault().Value != null && FitnessCertificate.FirstOrDefault().Value.Count() > 0)
        //                    {
        //                        var FCArray = (from items in FitnessCertificate.FirstOrDefault().Value
        //                                       group items by items.VehicleId into g
        //                                       select g.Sum(p => p.FCCost)).ToArray();
        //                        if (FCArray != null)
        //                            vehrep.FC = FCArray[0];
        //                    }
        //                    criteria.Clear();
        //                    criteria.Add("VehicleId", (Int64)item.Id);
        //                    if (FromTo[0] != null && FromTo[1] != null)
        //                        criteria.Add("CreatedDate", FromTo);
        //                    Dictionary<long, IList<Insurance>> Insurance = ts.GetInsuranceDetailsWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);

        //                    if (Insurance != null && Insurance.FirstOrDefault().Value != null && Insurance.FirstOrDefault().Value.Count() > 0)
        //                    {
        //                        var InsArray = (from items in Insurance.FirstOrDefault().Value
        //                                        group items by items.VehicleId into g
        //                                        select g.Sum(p => p.InsuranceDeclaredValue)).ToArray();
        //                        if (InsArray != null)
        //                            vehrep.Insurance = InsArray[0];
        //                    }
        //                    criteria.Clear();
        //                    criteria.Add("VehicleId", item.Id);
        //                    if (FromTo[0] != null && FromTo[1] != null)
        //                        criteria.Add("CreatedDate", FromTo);
        //                    Dictionary<long, IList<VehicleMaintenance>> VehicleMaintenance = ts.GetVehicleMaintenanceListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);

        //                    if (VehicleMaintenance != null && VehicleMaintenance.FirstOrDefault().Value != null && VehicleMaintenance.FirstOrDefault().Value.Count() > 0)
        //                    {
        //                        var VehMaintenanceArray = (from items in VehicleMaintenance.FirstOrDefault().Value
        //                                                   group items by items.VehicleId into g
        //                                                   select g.Sum(p => p.VehicleSeviceCost)).ToArray();
        //                        if (VehMaintenanceArray != null)
        //                            vehrep.MechanicalMaintenance = VehMaintenanceArray[0];
        //                    }

        //                    criteria.Clear();
        //                    criteria.Add("VehicleId", item.Id);
        //                    if (FromTo[0] != null && FromTo[1] != null)
        //                        criteria.Add("CreatedDate", FromTo);
        //                    Dictionary<long, IList<VehicleACMaintenance>> VehicleACMaintenance = ts.GetVehicleACMaintenanceListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
        //                    if (VehicleACMaintenance != null && VehicleACMaintenance.FirstOrDefault().Value != null && VehicleACMaintenance.FirstOrDefault().Value.Count() > 0)
        //                    {
        //                        var VehACMaintenanceArray = (from items in VehicleACMaintenance.FirstOrDefault().Value
        //                                                     group items by items.VehicleId into g
        //                                                     select g.Sum(p => p.ACServiceCost)).ToArray();
        //                        if (VehACMaintenanceArray != null)
        //                            vehrep.ACMaintenance = VehACMaintenanceArray[0];
        //                    }

        //                    criteria.Clear();
        //                    criteria.Add("VehicleId", item.Id);
        //                    if (FromTo[0] != null && FromTo[1] != null)
        //                        criteria.Add("CreatedDate", FromTo);
        //                    Dictionary<long, IList<VehicleElectricalMaintenance>> VehicleElectricalMaintenance = ts.GetVehicleElectricalMaintenanceListWithsearchCriteria(0, 9999, string.Empty, string.Empty, criteria);
        //                    if (VehicleElectricalMaintenance != null && VehicleElectricalMaintenance.FirstOrDefault().Value != null && VehicleElectricalMaintenance.FirstOrDefault().Value.Count() > 0)
        //                    {
        //                        var VehElecMaintenanceArray = (from items in VehicleElectricalMaintenance.FirstOrDefault().Value
        //                                                       group items by items.VehicleId into g
        //                                                       select g.Sum(p => p.EServiceCost)).ToArray();
        //                        if (VehElecMaintenanceArray != null)
        //                            vehrep.ElectricalMaintenance = VehElecMaintenanceArray[0];
        //                    }

        //                    criteria.Clear();
        //                    criteria.Add("VehicleId", item.Id);
        //                    if (FromTo[0] != null && FromTo[1] != null)
        //                        criteria.Add("CreatedDate", FromTo);
        //                    Dictionary<long, IList<VehicleBodyMaintenance>> VehicleBodyMaintenance = ts.GetVehicleBodyMaintenanceListWithsearchCriteria(0, 9999, string.Empty, string.Empty, criteria);
        //                    if (VehicleBodyMaintenance != null && VehicleBodyMaintenance.FirstOrDefault().Value != null && VehicleBodyMaintenance.FirstOrDefault().Value.Count() > 0)
        //                    {
        //                        var VehBodyMaintenanceArray = (from items in VehicleBodyMaintenance.FirstOrDefault().Value
        //                                                       group items by items.VehicleId into g
        //                                                       select g.Sum(p => p.BServiceCost)).ToArray();
        //                        if (VehBodyMaintenanceArray != null)
        //                            vehrep.BodyMaintenance = VehBodyMaintenanceArray[0];
        //                    }
        //                    criteria.Clear();
        //                    criteria.Add("VehicleId", item.Id);
        //                    if (FromTo[0] != null && FromTo[1] != null)
        //                        criteria.Add("CreatedDate", FromTo);
        //                    Dictionary<long, IList<VehicleTyreMaintenance>> VehicleTyreMaintenance = ts.GetVehicleTyreMaintenanceListWithsearchCriteria(0, 9999, string.Empty, string.Empty, criteria);
        //                    if (VehicleTyreMaintenance != null && VehicleTyreMaintenance.FirstOrDefault().Value != null && VehicleTyreMaintenance.FirstOrDefault().Value.Count() > 0)
        //                    {
        //                        var VehTyreMaintenanceArray = (from items in VehicleTyreMaintenance.FirstOrDefault().Value
        //                                                       // where items.CostOfService != null && items.TyreCost != null && items.TyreServiceCost != null
        //                                                       group items by items.VehicleId into g
        //                                                       select
        //                                                       new
        //                                                       {
        //                                                           cell = new decimal?[] {
        //                                                           g.Sum(p => p.CostOfService),
        //                                                           g.Sum(p=>p.TyreCost),
        //                                                           g.Sum(p=>p.TyreServiceCost)
        //                                                      }
        //                                                       }
        //                                                       ).ToArray();
        //                        if (VehTyreMaintenanceArray != null && VehTyreMaintenanceArray[0].cell != null)
        //                            vehrep.TyreMaintenance = Convert.ToDecimal(VehTyreMaintenanceArray[0].cell[0]) + Convert.ToDecimal(VehTyreMaintenanceArray[0].cell[1] + Convert.ToDecimal(VehTyreMaintenanceArray[0].cell[2]));
        //                    }

        //                    VehicleReport.Add(vehrep);

        //                }
        //            }

        //            if (VehicleReport != null && VehicleReport.Count() > 0)
        //            {
        //                if (ExportType == "Excel")
        //                {
        //                    var List = VehicleReport.ToList();
        //                    ExptToXL(List, "VehicleReport", (items => new
        //                    {
        //                        items.Id,
        //                        VehicleId = items.VehicleId.ToString(),
        //                        items.Type,
        //                        items.VehicleNo,
        //                        items.Campus,
        //                        DistanceCovered = items.DistanceCovered.ToString(),
        //                        FuelConsumed = items.FuelConsumed.ToString(),
        //                        FuelCost = items.FuelCost.ToString(),
        //                        Mileage = items.Mileage != null ? items.Mileage.Value.ToString("#.#") : "",
        //                        FC = items.FC.ToString(),
        //                        Insurance = items.Insurance.ToString(),
        //                        MechanicalMaintenance = items.MechanicalMaintenance.ToString(),
        //                        ACMaintenance = items.ACMaintenance.ToString(),
        //                        ElectricalMaintenance = items.ElectricalMaintenance.ToString(),
        //                        BodyMaintenance = items.BodyMaintenance.ToString(),
        //                        TyreMaintenance = items.TyreMaintenance.ToString()
        //                    }));
        //                    return new EmptyResult();
        //                }
        //                else
        //                {
        //                    long totalrecords = VehicleList.FirstOrDefault().Key;
        //                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
        //                    var VehicleReportList = new
        //                    {
        //                        total = totalPages,
        //                        page = page,
        //                        records = totalrecords,
        //                        rows = (from items in VehicleReport
        //                                select new
        //                                {
        //                                    cell = new string[] {
        //                        items.Id.ToString(),
        //                        items.VehicleId.ToString(),
        //                        items.Type,
        //                        items.VehicleNo,
        //                        items.Campus,
        //                        items.DistanceCovered.ToString(),
        //                        items.FuelConsumed.ToString(),
        //                        items.FuelCost.ToString(),
        //                        items.Mileage!=null? items.Mileage.Value.ToString("#.#"):"",
        //                        items.FC.ToString(),
        //                        items.Insurance.ToString(),
        //                        items.MechanicalMaintenance.ToString(),
        //                        items.ACMaintenance.ToString(),
        //                        items.ElectricalMaintenance.ToString(),
        //                        items.BodyMaintenance.ToString(),
        //                        items.TyreMaintenance.ToString()
        //                    }
        //                                })
        //                    };
        //                    return Json(VehicleReportList, JsonRequestBehavior.AllowGet);
        //                }
        //            }
        //            else
        //            {
        //                var AssLst = new { rows = (new { cell = new string[] { } }) };
        //                return Json(AssLst, JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
        //        throw ex;
        //    }

        //}

        public ActionResult TyreManagement()
        {
            #region BreadCrumb
            string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
            #endregion
            return View();
        }

        public ActionResult TyreInvoiceListJqGrid(TyreInvoiceDetails tid, string ExportType, int rows, string sidx, string sord, int? page = 1)
        {
            TransportService ts = new TransportService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            if (!string.IsNullOrWhiteSpace(tid.Status)) criteria.Add("Status", tid.Status);
            Dictionary<long, IList<TyreInvoiceDetails>> TyreInvoiceList = ts.GetTyreInvoiceDetailsListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
            if (TyreInvoiceList != null && TyreInvoiceList.FirstOrDefault().Value != null && TyreInvoiceList.FirstOrDefault().Key > 0 && TyreInvoiceList.FirstOrDefault().Value.Count > 0)
            {
                long totalrecords = TyreInvoiceList.First().Key;
                int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                var InvoiceList = new
                {
                    total = totalPages,
                    page = page,
                    records = totalrecords,
                    rows = (from items in TyreInvoiceList.First().Value
                            select new
                            {
                                i = 2,
                                cell = new string[] {
                               items.Id.ToString(),
                               items.RefNo,
                               items.Campus,
                               items.PurchasedDate!=null? items.PurchasedDate.Value.ToString("dd/MM/yyyy"):null,
                               items.PurchasedFrom,
                               items.InvoiceNo,
                               items.PaymentType,
                               items.TotalCost.ToString(),
                               items.TaxPercentage.ToString(),
                               items.TaxAmount.ToString(),
                               items.OtherExpenses.ToString(),
                               items.RoundedOffCost.ToString(),
                               items.CreatedDate!=null? items.CreatedDate.Value.ToString("dd/MM/yyyy"):null,
                               items.CreatedBy,
                               items.Status
                            }
                            })
                };
                return Json(InvoiceList, JsonRequestBehavior.AllowGet);
            }
            var AssLst = new { rows = (new { cell = new string[] { } }) };
            return Json(AssLst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TyreInvoiceDetails(int? Id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TyreInvoiceDetails tid = new TyreInvoiceDetails();
                    TransportService ts = new TransportService();
                    if (Id > 0)
                    {
                        tid = ts.GetTyreInvoiceDetailsById((int)Id);
                    }
                    else
                    {
                        tid.CreatedBy = userId;
                        User user = (User)Session["objUser"];
                        tid.Campus = user.Campus;
                        tid.Status = "Open";
                    }
                    return View(tid);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult TyreInvoiceDetails(TyreInvoiceDetails tid)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    TransportService ts = new TransportService();
                    //if (Request["PurchasedDate"]!=typeof(DateTime))
                    tid.PurchasedDate = DateTime.Parse(Request["PurchasedDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    tid.CreatedDate = DateTime.Now;
                    tid.CreatedBy = userId;
                    ts.CreateOrUpdateTyreInvoiceDetails(tid);
                    tid.RefNo = "Ref-" + tid.Id;
                    ts.CreateOrUpdateTyreInvoiceDetails(tid);
                    if (Request.Form["btnComplete"] == "Complete")
                    {
                        tid.Status = "Completed";
                        ts.CreateOrUpdateTyreInvoiceDetails(tid);
                        return RedirectToAction("ShowTyreInvoiceDetails", new { Id = tid.Id });
                    }
                    return RedirectToAction("TyreInvoiceDetails", new { Id = tid.Id });
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult ShowTyreInvoiceDetails(int? Id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TyreInvoiceDetails tid = new TyreInvoiceDetails();
                    TransportService ts = new TransportService();
                    if (Id > 0)
                    {
                        tid = ts.GetTyreInvoiceDetailsById((int)Id);
                    }
                    return View(tid);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        //public ActionResult SaveTyreInvoiceDetails(TyreInvoiceDetails tid, string PurchasedDate)
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //        else
        //        {
        //            IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
        //            TransportService ts = new TransportService();
        //            tid.PurchasedDate = DateTime.Parse(PurchasedDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
        //            tid.CreatedDate = DateTime.Now;
        //            tid.CreatedBy = userId;
        //            ts.CreateOrUpdateTyreInvoiceDetails(tid);
        //            return Json(tid.Id);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
        //        throw ex;
        //    }
        //}

        public ActionResult TyreDetailsListJqGrid(TyreDetails td, string ExportType, int rows, string sidx, string sord, int? page = 1)
        {
            TransportService ts = new TransportService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            if (td.InvoiceId > 0)
            {
                criteria.Add("InvoiceId", td.InvoiceId);
                Dictionary<long, IList<TyreDetails>> TyreDetails = ts.GetTyreDetailsListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (TyreDetails != null && TyreDetails.FirstOrDefault().Value != null && TyreDetails.FirstOrDefault().Key > 0 && TyreDetails.FirstOrDefault().Value.Count > 0)
                {
                    long totalrecords = TyreDetails.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var TyreList = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,
                        rows = (from items in TyreDetails.First().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[] {
                               items.Id.ToString(),items.InvoiceId.ToString(),items.TyreNo,items.Make,items.Model,items.Size,items.Type, items.TubeCost>0? items.TubeCost.ToString():Convert.ToString(0),items.TyreCost.ToString(),items.TotalCost.ToString()
                            }
                                })
                    };
                    return Json(TyreList, JsonRequestBehavior.AllowGet);
                }
                return null;
            }
            else
                return null;
        }

        [HttpPost]
        public ActionResult AddTyreDetails(TyreDetails td)
        {
            TransportService ts = new TransportService();
            ts.CreateOrUpdateTyreDetails(td);
            return Json("Tyre details added successfully", JsonRequestBehavior.AllowGet);
        }

        public ActionResult TyreSearch()
        {
            return PartialView();
        }

        public ActionResult GetTyresFromStock(TyreDetails td, string ExportType, int rows, string sidx, string sord, int? page = 1)
        {
            TransportService ts = new TransportService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("IsAssigned", td.IsAssigned);
            Dictionary<long, IList<TyreDetailsAndInvoiceDetails>> TyreDetails = ts.GetTyreDetailsAndInvoiceDetailsListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
            if (TyreDetails != null && TyreDetails.FirstOrDefault().Value != null && TyreDetails.FirstOrDefault().Key > 0 && TyreDetails.FirstOrDefault().Value.Count > 0)
            {
                long totalrecords = TyreDetails.First().Key;
                int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                var TyreList = new
                {
                    total = totalPages,
                    page = page,
                    records = totalrecords,
                    rows = (from items in TyreDetails.First().Value
                            select new
                            {
                                i = 2,
                                cell = new string[] {
                               items.Id.ToString(),items.InvoiceId.ToString(),items.TyreInvoiceDetails.InvoiceNo,items.TyreNo,items.Make,items.Model,items.Size,items.Type,items.TubeCost.ToString(),items.TyreCost.ToString(),items.TotalCost.ToString(),items.IsAssigned==true?"Yes":"No",items.AssignedTo
                            }
                            })
                };
                return Json(TyreList, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        public ActionResult TyreDetails()
        {
            #region BreadCrumb
            string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
            #endregion
            return View();
        }

        public ActionResult ShowTyreDetailsFromStock(TyreDetailsAndInvoiceDetails td, string TubeCost, string TyreCost, string TotalCost, string IsAssigned, string ExportType, int rows, string sidx, string sord, int? page = 1)
        {
            TransportService ts = new TransportService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            string[] alias = new string[1];
            alias[0] = "TyreInvoiceDetails";
            //criteria.Add("IsAssigned", td.IsAssigned);
            //if (!string.IsNullOrWhiteSpace(td.TyreInvoiceDetails.InvoiceNo)) criteria.Add("TyreInvoiceDetails.InvoiceNo", td.TyreInvoiceDetails.InvoiceNo);
            if (!string.IsNullOrWhiteSpace(td.TyreNo)) criteria.Add("TyreNo", td.TyreNo);
            if (!string.IsNullOrWhiteSpace(td.Make)) criteria.Add("Make", td.Make);
            if (!string.IsNullOrWhiteSpace(td.Model)) criteria.Add("Model", td.Model);
            if (!string.IsNullOrWhiteSpace(td.Size)) criteria.Add("Size", td.Size);
            if (!string.IsNullOrWhiteSpace(td.Type)) criteria.Add("Type", td.Type);
            if (!string.IsNullOrWhiteSpace(TubeCost)) criteria.Add("TubeCost", Convert.ToDecimal(TubeCost));
            if (!string.IsNullOrWhiteSpace(TyreCost)) criteria.Add("TyreCost", Convert.ToDecimal(TyreCost));
            if (!string.IsNullOrWhiteSpace(TotalCost)) criteria.Add("TotalCost", Convert.ToDecimal(TotalCost));
            if (!string.IsNullOrWhiteSpace(IsAssigned)) criteria.Add("IsAssigned", IsAssigned == "True" ? true : false);
            if (!string.IsNullOrWhiteSpace(td.AssignedTo)) criteria.Add("AssignedTo", td.AssignedTo);
            Dictionary<long, IList<TyreDetailsAndInvoiceDetails>> TyreDetails = ts.GetTyreDetailsAndInvoiceDetailsListWithAliasPagingAndCriteria(page - 1, rows, sidx, sord, criteria, alias);
            if (TyreDetails != null && TyreDetails.FirstOrDefault().Value != null && TyreDetails.FirstOrDefault().Key > 0 && TyreDetails.FirstOrDefault().Value.Count > 0)
            {
                long totalrecords = TyreDetails.First().Key;
                int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                var TyreList = new
                {
                    total = totalPages,
                    page = page,
                    records = totalrecords,
                    rows = (from items in TyreDetails.First().Value
                            select new
                            {
                                i = 2,
                                cell = new string[] {
                               items.Id.ToString(),items.InvoiceId.ToString(),items.TyreInvoiceDetails.InvoiceNo,items.TyreNo,items.Make,items.Model,items.Size,items.Type,items.TubeCost.ToString(),items.TyreCost.ToString(),items.TotalCost.ToString(),items.IsAssigned==true?"Yes":"No",items.AssignedTo
                            }
                            })
                };
                return Json(TyreList, JsonRequestBehavior.AllowGet);
            }
            var AssLst = new { rows = (new { cell = new string[] { } }) };
            return Json(AssLst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TransportVendorMaster()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }


        public ActionResult TransportVendorMasterListJqGrid(TransportVendorMaster tvm, string ExportType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                TransportService ts = new TransportService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(tvm.Name)) { criteria.Add("Name", tvm.Name); }
                if (!string.IsNullOrEmpty(tvm.DealerType)) { criteria.Add("DealerType", tvm.DealerType); }
                if (!string.IsNullOrEmpty(tvm.VendorType)) { criteria.Add("VendorType", tvm.VendorType); }
                if (!string.IsNullOrEmpty(tvm.VendorFor)) { criteria.Add("VendorFor", tvm.VendorFor); }
                if (!string.IsNullOrEmpty(tvm.PAN)) { criteria.Add("PAN", tvm.PAN); }
                if (!string.IsNullOrEmpty(tvm.TIN)) { criteria.Add("TIN", tvm.TIN); }
                if (!string.IsNullOrEmpty(tvm.FAX)) { criteria.Add("FAX", tvm.FAX); }
                if (!string.IsNullOrEmpty(tvm.ContactName)) { criteria.Add("ContactName", tvm.ContactName); }
                if (!string.IsNullOrEmpty(tvm.ContactNo)) { criteria.Add("ContactNo", tvm.ContactNo); }
                if (!string.IsNullOrEmpty(tvm.BankName)) { criteria.Add("BankName", tvm.BankName); }
                if (!string.IsNullOrEmpty(tvm.AccountName)) { criteria.Add("AccountName", tvm.AccountName); }
                Dictionary<long, IList<TransportVendorMaster>> TransportVendorMaster = ts.GetTransportVendorMasterListWithPagingAndCriteria(page - 1, rows, sord, sidx, criteria);
                if (TransportVendorMaster != null && TransportVendorMaster.FirstOrDefault().Value != null && TransportVendorMaster.FirstOrDefault().Key > 0 && TransportVendorMaster.FirstOrDefault().Value.Count > 0)
                {
                    long totalrecords = TransportVendorMaster.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var VendorList = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,
                        rows = (from items in TransportVendorMaster.First().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[] {
                               items.Name,
                               items.DealerType,
                               items.VendorType,
                               items.VendorFor,
                               items.PAN,
                               items.TIN,
                               items.FAX,
                               items.ContactName,
                               items.ContactNo,
                               items.Email,
                               items.Website,
                               items.ReasonForSelecting,
                               items.CreditDays,
                               items.ApplicableForTDS.ToString(),
                               items.BankName,
                               items.BankBranch,
                               items.AccountName,
                               items.AccountType,
                               items.AccountNo,
                               items.PIN,
                               items.Add1,
                               items.Add2,
                               items.Id.ToString(),

                            }
                                })
                    };
                    return Json(VendorList, JsonRequestBehavior.AllowGet);
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

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddTransportVendor(TransportVendorMaster ssm, string test)
        {
            try
            {
                ssm.Name = ssm.Name.Trim();
                TransportService ts = new TransportService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Name", ssm.Name);
                Dictionary<long, IList<TransportVendorMaster>> TransportVendorMaster = ts.GetTransportVendorMasterListWithPagingAndCriteria(0, 9999, "", "", criteria);
                if (test == "edit")
                {
                    if (TransportVendorMaster != null && TransportVendorMaster.First().Value != null && TransportVendorMaster.First().Value.Count > 1)
                    {
                        //var script = @"ErrMsg(""Already Exists"");";
                        //return JavaScript(script);
                        return null;
                    }
                    else
                    {
                        ViewBag.flag = 1;
                        ts.CreateOrUpdateTransportVendorMaster(ssm);
                        return null;
                    }
                }
                else
                {
                    if (TransportVendorMaster != null && TransportVendorMaster.First().Value != null && TransportVendorMaster.First().Value.Count > 0)
                    {
                        var script = @"ErrMsg(""Already Exists"");";
                        return JavaScript(script);
                    }
                    else
                    {
                        ssm.Id = 0;
                        ViewBag.flag = 1;
                        int id = ts.CreateOrUpdateTransportVendorMaster(ssm);
                        ssm.Id = id;
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult ChooseTransportVendor()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        #region Added By Gobi
        public DateTime[] ConvertStringToDateTime(string StringDate)
        {
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            DateTime[] fromto = new DateTime[2];
            if (!string.IsNullOrEmpty(StringDate))
            {
                string[] strDobArray = StringDate.Split('/');
                string Month = strDobArray[1];
                string Date = strDobArray[0];
                string Year = strDobArray[2];

                DateTime First = new DateTime(Convert.ToInt32(Year), Convert.ToInt32(Month), Convert.ToInt32(Date));
                DateTime Last = new DateTime(Convert.ToInt32(Year), Convert.ToInt32(Month), Convert.ToInt32(Date));

                string from = string.Format("{0:MM/dd/yyyy}", First);
                string to = string.Format("{0:MM/dd/yyyy}", Last);
                fromDate = Convert.ToDateTime(from + " " + "12:00:00 AM");
                toDate = Convert.ToDateTime(to + " " + "23:59:59");
                fromto[0] = fromDate;
                fromto[1] = toDate;
            }
            return fromto;
        }
        #endregion


        #region "Transport Driver Management By Micheal"

        public ActionResult DriverManagement()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    IList<CampusMaster> campusMsObj = CampusMasterFunc();
                    ViewBag.campusddl = campusMsObj;
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

        public ActionResult NewDriverManagement()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    IList<CampusMaster> campusMsObj = CampusMasterFunc();
                    ViewBag.campusddl = campusMsObj;
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

        public ActionResult DriverManagementListGrid(DriverMaster dm, string Campus, string DriverIdNo, string Name, string Status, string PageName, string ExportType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(Name)) { criteria.Add("Name", Name); }
                else if (!string.IsNullOrEmpty(dm.Name)) { criteria.Add("Name", dm.Name); }
                else { }
                if (!string.IsNullOrEmpty(Campus)) { criteria.Add("Campus", Campus); }
                else if (!string.IsNullOrEmpty(dm.Campus)) { criteria.Add("Campus", dm.Campus); }
                else { }
                if (dm.Age > 0) criteria.Add("Age", dm.Age);
                if (!string.IsNullOrEmpty(dm.Gender)) { criteria.Add("Gender", dm.Gender); }
                if (!string.IsNullOrEmpty(dm.ContactNo)) { criteria.Add("ContactNo", dm.ContactNo); }
                if (PageName == "Saved")
                {
                    if (!string.IsNullOrEmpty(Status))
                    {
                        criteria.Add("Status", Status);
                    }
                    else
                    {
                        criteria.Add("Status", "Registered");
                        criteria.Add("IsActive", true);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(Status))
                    {
                        criteria.Add("Status", Status);
                    }
                    else
                    {
                        //criteria.Add("Status", "Registered");
                        criteria.Add("IsRegistered", false);
                    }
                }
                Dictionary<long, IList<DriverMaster>> DriverMasterList = ts.GetDriverMasterDetails(page - 1, rows, sidx, sord, criteria);
                if (DriverMasterList != null && DriverMasterList.First().Key > 0 && DriverMasterList.FirstOrDefault().Value != null)
                {
                    UserService us = new UserService();
                    if (ExportType == "Excel")
                    {
                        var List = DriverMasterList.First().Value.ToList();
                        ExptToXL(List, "DriverMasterList", (items => new
                        {
                            items.Campus,
                            items.Name,
                            items.Dob,
                            items.Age,
                            items.Gender,
                            items.ContactNo,
                            items.LicenseNo,
                            items.DriverIdNo,
                            items.BatchNo,
                            items.LicenseValDate,
                            items.NonTraLicenseValDate,
                            items.PresentAddress,
                            items.PermanentAddress,
                            items.CreatedDate,
                            items.ModifiedDate,
                            items.ModifiedBy,
                            items.Status
                        }));
                        return new EmptyResult();
                    }
                    else
                    {
                        long totalrecords = DriverMasterList.FirstOrDefault().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var AssLst = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (
                                 from items in DriverMasterList.First().Value
                                 select new
                                 {
                                     cell = new string[] 
                                         {
                                            items.Id.ToString(),
                                            items.Campus,
                                            //items.Name,
                                            String.Format(@"<a style='color:#034af3;text-decoration:underline' href= '/Transport/DriverRegistrationForm?PreRegNo="+items.DriverRegNo+"'  >{0}</a>",items.Name),
                                            items.DriverIdNo,
                                            items.Dob,
                                            items.Age.ToString(),
                                            items.Gender,
                                            items.ContactNo,
                                            items.Status.ToString(),
                                            items.CreatedDate!=null?items.CreatedDate.ToString("dd/MM/yyyy hh:mm:ss tt"):"",
                                            items.CreatedBy,
                                            items.ModifiedDate!=null?items.ModifiedDate.ToString("dd/MM/yyyy hh:mm:ss tt"):"",
                                            items.ModifiedBy,
                                            items.LicenseValDate!=null && items.NonTraLicenseValDate !=null && items.Name !=null ? String.Format("<img src='/Images/Pdf_Icon.png ' id='ImgHistory' onclick=\"DriverDetails('" +items.Id +"');\" />") :"Please Enter Required Details"
                                         }
                                 }).ToList()
                        };
                        return Json(AssLst, JsonRequestBehavior.AllowGet);
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
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult DriverRegistrationForm(long? PreRegNo)
        {
            string userId = base.ValidateUser();
            if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
            else
            {
                if (PreRegNo == null)
                {
                    FillViewBag();
                    Dictionary<long, IList<DriverRegNumDetails>> drn = ts.GetDriverRegNumDetailsListWithPaging(0, 10000, string.Empty, string.Empty, null);
                    var id1 = drn.First().Value[0].PreRegNum + 1;
                    DriverRegNumDetails srn = new DriverRegNumDetails();
                    srn.PreRegNum = id1;
                    Session["status"] = "New Registration";
                    ViewBag.Registerstatus = "New Registration";
                    srn.Id = drn.First().Value[0].Id;
                    srn.Date = DateTime.Now.ToShortDateString();
                    srn.Time = DateTime.Now.ToShortTimeString();
                    ViewBag.processby = userId;
                    ViewBag.initialpage = "yes";
                    ts.CreateOrUpdateDriverRegNumDetails(srn);
                    ViewBag.DriverRegNo = id1;
                    return View();
                }
                else
                {
                    FillViewBag();
                    ViewBag.processby = userId;
                    DriverMaster Dr = new DriverMaster();
                    Dr = ts.GetDriverDetailsByDriverRegNo(PreRegNo ?? 0);
                    if (Dr.Status == "SentForApproval")
                    {
                        Session["sentforapproval"] = "yes";
                    }
                    else
                    {
                        Session["sentforapproval"] = "";
                    }
                    ViewBag.Registerstatus = Dr.Status;
                    ViewBag.DriverRegNo = Dr.DriverRegNo;
                    //Dr.DateOfJoin=
                    return View(Dr);
                }
            }
        }

        [HttpPost]
        public ActionResult DriverRegistrationForm(DriverMaster dm)
        {
            try
            {
                string userId = base.ValidateUser();
                string desMas = string.Empty;
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);

                    if (!string.IsNullOrWhiteSpace(Request["dateofjoin"]))
                    {
                        dm.LicenseValDate = DateTime.Parse(Request["dateofjoin"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    }
                    if (!string.IsNullOrWhiteSpace(Request["LicenseValDate"]))
                    {
                        dm.LicenseValDate = DateTime.Parse(Request["LicenseValDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    }
                    if (!string.IsNullOrWhiteSpace(Request["NonTraLicenseValDate"]))
                    {
                        dm.NonTraLicenseValDate = DateTime.Parse(Request["NonTraLicenseValDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    }
                    if (dm.Status == "Sent For Approval")
                    {
                        Session["sent for approval"] = "yes";
                    }
                    else
                    {
                        Session["sentforapproval"] = "";
                    }
                    if (Request.Form["btnSave"] == "Save")
                    {
                        if (dm.Id > 0)
                        {
                            ViewBag.Registerstatus = dm.Status;
                        }
                        else
                        {
                            dm.Status = "New Registration";
                            dm.CreatedDate = DateTime.Now;
                            dm.CreatedBy = "";
                            dm.ModifiedDate = DateTime.Now;
                            dm.ModifiedBy = "";
                            ViewBag.Registerstatus = dm.Status;

                        }
                    }

                    else if (Request.Form["btnsentforapproval"] == "Send For Approval")
                    {
                        dm.Status = "Sent For Approval";
                        //Session["status"] = dm.Status;
                        ViewBag.Registerstatus = dm.Status;
                        ViewBag.sentforappr = "yes";
                    }
                    else if (Request.Form["btnapprove"] == "Approve")
                    {
                        // MastersService ms = new MastersService();
                        // Dictionary<long, IList<DesignationMaster>> DesignationMaster = ms.GetDesignationMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteriam);
                        // if (DesignationMaster != null && DesignationMaster.FirstOrDefault().Value.Count > 0 && DesignationMaster.FirstOrDefault().Key > 0)
                        // {
                        //     desMas = DesignationMaster.FirstOrDefault().Value[0].Code;
                        // }
                        // TIPS.Service.UserService us = new UserService();
                        if (dm.Status == "Registered")
                        {
                            Dictionary<string, object> criteria2 = new Dictionary<string, object>();
                            criteria2.Add("DriverRegNo", dm.DriverRegNo);   // to check if staff is already registered or not while changing from inactive to registered
                            Dictionary<long, IList<DriverMaster>> drivermaster = ts.GetDriverMasterDetails(0, 999999, string.Empty, string.Empty, criteria2);
                            dm.IsRegistered = true;
                            dm.IsActive = true;
                            ViewBag.Registerstatus = dm.Status;

                        }
                        if (dm.Status == "Inactive")
                        {
                            dm.IsRegistered = false;
                            dm.IsActive = false;

                        }
                        //Session["status"] = sd.Status;
                        ViewBag.Registerstatus = dm.Status;

                    }
                    FillViewBag();
                    ts.CreateOrUpdateDriverMaster(dm);
                    ViewBag.DriverRegNo = dm.DriverRegNo;
                    return View(dm);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "");
                throw ex;
            }
        }

        private string FillViewBag()
        {
            IList<CampusMaster> campusMsObj = CampusMasterFunc();
            ViewBag.campusddl = campusMsObj;
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Clear();
            var usrcmp = Session["UserCampus"] as IEnumerable<string>;
            if (usrcmp.Count() != 0)
            {
                if (usrcmp.First() != null)            // to check if the usrcmp obj is null or with data
                {
                    criteria.Add("Name", usrcmp);
                }
            }
            Dictionary<long, IList<CampusMaster>> CampusMaster = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            ViewBag.campusddl = CampusMaster.First().Value;
            criteria.Clear();
            criteria.Add("DocumentFor", "Driver");
            Dictionary<long, IList<DocumentTypeMaster>> DocumentTypeMaster = ms.GetDocumentTypeMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            ViewBag.documentddl = DocumentTypeMaster.First().Value;
            criteria.Clear();
            Dictionary<long, IList<DesignationMaster>> DesignationMaster = ms.GetDesignationMasterListWithPagingAndCriteria(0, 200, string.Empty, string.Empty, criteria);
            ViewBag.designationddl = DesignationMaster.First().Value;
            Dictionary<long, IList<BloodGroupMaster>> BloodGroupMaster = ms.GetBloodGroupMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            ViewBag.bloodgrpddl = BloodGroupMaster.First().Value;
            criteria.Clear();
            criteria.Add("Module", "Driver");
            Dictionary<long, IList<RelationshipMaster>> familyddl = ms.GetRelationshipMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            ViewBag.familyddl = familyddl.First().Value;
            criteria.Clear();
            ViewBag.Date = DateTime.Now.ToShortDateString();
            ViewBag.Time = DateTime.Now.ToShortTimeString();
            return "";
        }

        public ActionResult uploadedFileDisplay(long Id)
        {
            try
            {
                AdmissionManagementService ads = new AdmissionManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("PreRegNum", Id);
                criteria.Add("DocumentFor", "Driver");
                criteria.Add("DocumentType", "Driver Photo");
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

        public JsonResult DriverDocumentsjqgrid(string DriverRegNo, string id, string txtSearch, string idno, string name, string sect, string cname, string grad, string btype, int rows, string sidx, string sord, int? page)
        {
            try
            {
                AdmissionManagementService ads = new AdmissionManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("DocumentFor", "Driver");
                criteria.Add("PreRegNum", Convert.ToInt64(DriverRegNo));
                Dictionary<long, IList<UploadedFilesView>> UploadedFilesview = ads.GetUploadedFilesViewListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                IList<UploadedFilesView> uploadList = new List<UploadedFilesView>();
                if (UploadedFilesview != null && UploadedFilesview.Count > 0 && UploadedFilesview.FirstOrDefault().Key > 0)
                {
                    long totalrecords = UploadedFilesview.Count;
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
                else { return Json(null, JsonRequestBehavior.AllowGet); }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        #endregion

        #region Driver Attendance By RajKumar
        public ActionResult DriverAttendance()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    ViewBag.campusddl = CampusMasterFunc();
                    ViewBag.Success = "";
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

        [HttpPost]
        public ActionResult DriverAttendance(DriverAttendance dod)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {

                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    //string temp = Request["AbsentDate"];
                    ////  var datevalue = DateTime.Parse(temp, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    //
                    //DateTime FromDt = new DateTime();
                    //if (!string.IsNullOrEmpty(temp))
                    //{
                    //   // FromDt = DateTime.ParseExact(temp, "dd/MM/yyyy", null);
                    //    FromDt = DateTime.ParseExact(temp, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    //}
                    //dod.AbsentDate = FromDt;
                    //    dod.AbsentDate = DateTime.Parse(Request["AbsentDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    dod.CreatedDate = DateTime.Now;
                    dod.CreatedBy = userId;
                    dod.ModifiedDate = DateTime.Now;
                    dod.ModifiedBy = userId;
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("Campus", dod.Campus);
                    criteria.Add("DriverName", dod.DriverName);
                    criteria.Add("DriverIdNo", dod.DriverIdNo);
                    // criteria.Add("AbsentType", dod.AbsentType);
                    criteria.Add("AbsentDate", dod.AbsentDate);
                    ViewBag.campusddl = CampusMasterFunc();
                    Dictionary<long, IList<DriverAttendance>> DriverAtDetails = ts.GetDriverAttendanceDetailsListWithPagingAndCriteria(0, 9999, "", "", criteria);
                    if (DriverAtDetails != null && DriverAtDetails.FirstOrDefault().Value != null && DriverAtDetails.FirstOrDefault().Value.Count > 0)
                    {
                        ViewBag.Success = "No";
                        return View();
                    }
                    else
                    {
                        ts.CreateOrUpdateAttendanceList(dod);
                        ViewBag.Success = "Yes";
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult DriverAttendanceDetailsListJqGrid(string AbsentDate, string CreatedDate, DriverAttendance da, int? Id, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    sord = sord == "desc" ? "Desc" : "Asc";
                    if (!string.IsNullOrWhiteSpace(da.Campus)) { criteria.Add("Campus", da.Campus); }
                    if (!string.IsNullOrWhiteSpace(da.DriverName)) { criteria.Add("DriverName", da.DriverName); }
                    if (!string.IsNullOrWhiteSpace(da.DriverIdNo)) { criteria.Add("DriverIdNo", da.DriverIdNo); }
                    if (!string.IsNullOrWhiteSpace(da.AbsentType)) { criteria.Add("AbsentType", da.AbsentType); }
                    if (!string.IsNullOrWhiteSpace(da.CreatedBy)) { criteria.Add("CreatedBy", da.CreatedBy); }

                    if (!string.IsNullOrWhiteSpace(AbsentDate))
                    {
                        AbsentDate = AbsentDate.Trim();
                        IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                        DateTime[] AttDatefromto = new DateTime[2];
                        AttDatefromto[0] = DateTime.Parse(AbsentDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        string To = string.Format("{0:dd/MM/yyyy}", AttDatefromto[0]);
                        AttDatefromto[1] = Convert.ToDateTime(To + " " + "23:59:59");
                        criteria.Add("AbsentDate", AttDatefromto);
                    }
                    if (!string.IsNullOrWhiteSpace(CreatedDate))
                    {
                        CreatedDate = CreatedDate.Trim();
                        IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                        DateTime[] CreatedDatefromto = new DateTime[2];
                        CreatedDatefromto[0] = DateTime.Parse(CreatedDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        string ToCreatedDate = string.Format("{0:dd/MM/yyyy}", CreatedDatefromto[0]);
                        CreatedDatefromto[1] = Convert.ToDateTime(ToCreatedDate + " " + "23:59:59");
                        criteria.Add("CreatedDate", CreatedDatefromto);
                    }
                    Dictionary<long, IList<DriverAttendance>> DriverAttendanceDetails = ts.GetDriverAttendanceDetailsListWithPagingAndCriteria(page - 1, rows, sord, sidx, criteria);
                    if (DriverAttendanceDetails != null && DriverAttendanceDetails.Count > 0)
                    {
                        long totalrecords = DriverAttendanceDetails.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var DriverAT = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in DriverAttendanceDetails.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                               items.Id.ToString(),
                               items.Campus,
                               items.DriverName, 
                               items.DriverIdNo,
                               items.AbsentDate!=null? items.AbsentDate.ToString("dd/MM/yyyy"):"",
                               items.AbsentType.ToString(),
                               items.CreatedDate!=null?items.CreatedDate.ToString("dd/MM/yyyy"):"",
                               //items.CreatedBy!=null?us.GetUserNameByUserId(items.CreatedBy):"",
                               items.CreatedBy
                            }
                                    })
                        };
                        return Json(DriverAT, JsonRequestBehavior.AllowGet);
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
        public ActionResult DeleteDriverAttendanceDetails(DriverAttendance DAT)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportBC tbc = new TransportBC();
                    if (DAT.Id > 0)
                    {
                        tbc.DeleteDriverAttendancevalue(DAT);
                    }
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

        #region Driver Attendance Report By RajKumar
        public ActionResult DriverAttendanceReports()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    DateTime DateNow = DateTime.Now;
                    string[] Academicyear = new string[3];
                    Academicyear[0] = "Select";
                    Academicyear[1] = (DateNow.Year - 1).ToString() + "-" + DateNow.Year.ToString();
                    Academicyear[2] = (DateNow.Year).ToString() + "-" + (DateNow.Year + 1).ToString();
                    //  ViewBag.acadddl = Academicyear;
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
        public ActionResult GetDriverAttendanceReportsJqGrid(string campus, string driverid, string drivername, string searchmonth, int year, string ExportType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                DateTime DateNow = DateTime.Now;
                int reduceCount = 0;
                int acaYear = 0;
                if (string.IsNullOrEmpty(campus))
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }

                else
                {

                    acaYear = year;
                    DateTime startDate = new DateTime(acaYear, Convert.ToInt32(searchmonth), 1);
                    DateTime endDate = new DateTime(acaYear, Convert.ToInt32(searchmonth), DateNow.Day);
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    if (!string.IsNullOrWhiteSpace(campus)) { criteria.Add("Campus", campus); }
                    criteria.Add("Status", "Registered");
                    criteria.Add("Designation", "Driver");
                    if (!string.IsNullOrWhiteSpace(driverid))
                    {
                        criteria.Add("DriverIdNo", driverid);
                    }
                    sord = sord == "desc" ? "Desc" : "Asc";
                    TransportService trServObj = new TransportService();
                    Dictionary<long, IList<DriverAttendanceReport>> DriverList = trServObj.GetDriverListListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    criteria.Clear();

                    Dictionary<long, IList<DriverAttendance>> AttendanceList = trServObj.GetAbsentListForAnAttendanceListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria);
                    List<DriverAttendance> alreadyExists = AttendanceList.FirstOrDefault().Value.ToList();
                    IEnumerable<long> blkLong = from p in alreadyExists
                                                orderby p.PreRegNum ascending
                                                select p.PreRegNum;
                    long[] attids = blkLong.ToArray();

                    foreach (DriverAttendanceReport a in DriverList.FirstOrDefault().Value)
                    {
                        if (attids.Contains((a.DriverRegNo)))
                        {
                            Dictionary<string, object> criteria1 = new Dictionary<string, object>();
                            criteria1.Add("PreRegNum", a.DriverRegNo);
                            if (!string.IsNullOrEmpty(searchmonth))
                            {
                                DateTime[] fromto = new DateTime[2];
                                fromto = GetLastAndFirstDateTimeinMonth(searchmonth, acaYear);
                                criteria1.Add("AbsentDate", fromto);
                            }
                            Dictionary<long, IList<DriverAttendance>> AbsentList = trServObj.GetAbsentListForAnAttendanceListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria1);
                            //a.AbsentCountList = AbsentList.First().Value.Count;
                            List<DriverAttendance> Absentdate = AbsentList.FirstOrDefault().Value.ToList();
                            var AbsentLong = (from p in Absentdate
                                              where p.AbsentType == "Absent"
                                              orderby p.PreRegNum ascending
                                              select p.AbsentDate).ToArray();
                            a.AbsentCountList = AbsentLong.Count();

                            var Leave = (from p in Absentdate
                                         where p.AbsentType == "Leave"
                                         orderby p.PreRegNum ascending
                                         select p.AbsentDate).ToArray();
                            a.LeaveCountList = Leave.Count();

                            // Default Prest List
                            a.Date1 = "<b style='color:Green'>P</b>"; a.Date2 = "<b style='color:Green'>P</b>"; a.Date3 = "<b style='color:Green'>P</b>"; a.Date4 = "<b style='color:Green'>P</b>"; a.Date5 = "<b style='color:Green'>P</b>"; a.Date6 = "<b style='color:Green'>P</b>"; a.Date7 = "<b style='color:Green'>P</b>"; a.Date8 = "<b style='color:Green'>P</b>"; a.Date9 = "<b style='color:Green'>P</b>"; a.Date10 = "<b style='color:Green'>P</b>"; a.Date11 = "<b style='color:Green'>P</b>"; a.Date12 = "<b style='color:Green'>P</b>"; a.Date13 = "<b style='color:Green'>P</b>"; a.Date14 = "<b style='color:Green'>P</b>"; a.Date15 = "<b style='color:Green'>P</b>";
                            a.Date16 = "<b style='color:Green'>P</b>"; a.Date17 = "<b style='color:Green'>P</b>"; a.Date18 = "<b style='color:Green'>P</b>"; a.Date19 = "<b style='color:Green'>P</b>"; a.Date20 = "<b style='color:Green'>P</b>"; a.Date21 = "<b style='color:Green'>P</b>"; a.Date22 = "<b style='color:Green'>P</b>"; a.Date23 = "<b style='color:Green'>P</b>"; a.Date24 = "<b style='color:Green'>P</b>"; a.Date25 = "<b style='color:Green'>P</b>"; a.Date26 = "<b style='color:Green'>P</b>"; a.Date27 = "<b style='color:Green'>P</b>"; a.Date28 = "<b style='color:Green'>P</b>"; a.Date29 = "<b style='color:Green'>P</b>"; a.Date30 = "<b style='color:Green'>P</b>";
                            a.Date31 = "<b style='color:Green'>P</b>";
                            // hide default present using this condition only for current month



                            if (DateNow.Month == Convert.ToInt32(searchmonth))
                            {
                                if (1 <= DateNow.Day) { a.Date1 = "<b style='color:Green'>P</b>"; } else { a.Date1 = ""; } if (2 <= DateNow.Day) { a.Date2 = "<b style='color:Green'>P</b>"; } else { a.Date2 = ""; } if (3 <= DateNow.Day) { a.Date3 = "<b style='color:Green'>P</b>"; } else { a.Date3 = ""; }
                                if (4 <= DateNow.Day) { a.Date4 = "<b style='color:Green'>P</b>"; } else { a.Date4 = ""; } if (5 <= DateNow.Day) { a.Date5 = "<b style='color:Green'>P</b>"; } else { a.Date5 = ""; } if (6 <= DateNow.Day) { a.Date6 = "<b style='color:Green'>P</b>"; } else { a.Date6 = ""; }
                                if (7 <= DateNow.Day) { a.Date7 = "<b style='color:Green'>P</b>"; } else { a.Date7 = ""; } if (8 <= DateNow.Day) { a.Date8 = "<b style='color:Green'>P</b>"; } else { a.Date8 = ""; } if (9 <= DateNow.Day) { a.Date9 = "<b style='color:Green'>P</b>"; } else { a.Date9 = ""; }
                                if (10 <= DateNow.Day) { a.Date10 = "<b style='color:Green'>P</b>"; } else { a.Date10 = ""; } if (11 <= DateNow.Day) { a.Date11 = "<b style='color:Green'>P</b>"; } else { a.Date11 = ""; } if (12 <= DateNow.Day) { a.Date12 = "<b style='color:Green'>P</b>"; } else { a.Date12 = ""; }
                                if (13 <= DateNow.Day) { a.Date13 = "<b style='color:Green'>P</b>"; } else { a.Date13 = ""; } if (14 <= DateNow.Day) { a.Date14 = "<b style='color:Green'>P</b>"; } else { a.Date14 = ""; } if (15 <= DateNow.Day) { a.Date15 = "<b style='color:Green'>P</b>"; } else { a.Date15 = ""; }
                                if (16 <= DateNow.Day) { a.Date16 = "<b style='color:Green'>P</b>"; } else { a.Date16 = ""; } if (17 <= DateNow.Day) { a.Date17 = "<b style='color:Green'>P</b>"; } else { a.Date17 = ""; } if (18 <= DateNow.Day) { a.Date18 = "<b style='color:Green'>P</b>"; } else { a.Date18 = ""; }
                                if (19 <= DateNow.Day) { a.Date19 = "<b style='color:Green'>P</b>"; } else { a.Date19 = ""; } if (20 <= DateNow.Day) { a.Date20 = "<b style='color:Green'>P</b>"; } else { a.Date20 = ""; } if (21 <= DateNow.Day) { a.Date21 = "<b style='color:Green'>P</b>"; } else { a.Date21 = ""; }
                                if (22 <= DateNow.Day) { a.Date22 = "<b style='color:Green'>P</b>"; } else { a.Date22 = ""; } if (23 <= DateNow.Day) { a.Date23 = "<b style='color:Green'>P</b>"; } else { a.Date23 = ""; } if (24 <= DateNow.Day) { a.Date24 = "<b style='color:Green'>P</b>"; } else { a.Date24 = ""; }
                                if (25 <= DateNow.Day) { a.Date25 = "<b style='color:Green'>P</b>"; } else { a.Date25 = ""; } if (26 <= DateNow.Day) { a.Date26 = "<b style='color:Green'>P</b>"; } else { a.Date26 = ""; } if (27 <= DateNow.Day) { a.Date27 = "<b style='color:Green'>P</b>"; } else { a.Date27 = ""; }
                                if (28 <= DateNow.Day) { a.Date28 = "<b style='color:Green'>P</b>"; } else { a.Date28 = ""; } if (29 <= DateNow.Day) { a.Date29 = "<b style='color:Green'>P</b>"; } else { a.Date29 = ""; } if (30 <= DateNow.Day) { a.Date30 = "<b style='color:Green'>P</b>"; } else { a.Date30 = ""; }
                                if (31 <= DateNow.Day) { a.Date31 = "<b style='color:Green'>P</b>"; } else { a.Date31 = ""; }
                            }

                            // Only for Absent List 
                            for (var i = 0; i < AbsentLong.Length; i++)
                            {
                                string Abvalue = AbsentLong[i].ToString("dd");

                                switch (Abvalue)
                                {
                                    case "1":
                                    case "01": { if (ExportType == "Excel") { a.Date1 = "<b style='color:Red'>A</b>"; } else { a.Date1 = "<b style='color:Red'>A</b>"; } } break;
                                    case "2":
                                    case "02": { if (ExportType == "Excel") { a.Date2 = "<b style='color:Red'>A</b>"; } else { a.Date2 = "<b style='color:Red'>A</b>"; } } break;
                                    case "3":
                                    case "03": { if (ExportType == "Excel") { a.Date3 = "<b style='color:Red'>A</b>"; } else { a.Date3 = "<b style='color:Red'>A</b>"; } } break;
                                    case "4":
                                    case "04": { if (ExportType == "Excel") { a.Date4 = "<b style='color:Red'>A</b>"; } else { a.Date4 = "<b style='color:Red'>A</b>"; } } break;
                                    case "5":
                                    case "05": { if (ExportType == "Excel") { a.Date5 = "<b style='color:Red'>A</b>"; } else { a.Date5 = "<b style='color:Red'>A</b>"; } } break;
                                    case "6":
                                    case "06": { if (ExportType == "Excel") { a.Date6 = "<b style='color:Red'>A</b>"; } else { a.Date6 = "<b style='color:Red'>A</b>"; } } break;
                                    case "7":
                                    case "07": { if (ExportType == "Excel") { a.Date7 = "<b style='color:Red'>A</b>"; } else { a.Date7 = "<b style='color:Red'>A</b>"; } } break;
                                    case "8":
                                    case "08": { if (ExportType == "Excel") { a.Date8 = "<b style='color:Red'>A</b>"; } else { a.Date8 = "<b style='color:Red'>A</b>"; } } break;
                                    case "9":
                                    case "09": { if (ExportType == "Excel") { a.Date9 = "<b style='color:Red'>A</b>"; } else { a.Date9 = "<b style='color:Red'>A</b>"; } } break;
                                    case "10": { if (ExportType == "Excel") { a.Date10 = "<b style='color:Red'>A</b>"; } else { a.Date10 = "<b style='color:Red'>A</b>"; } } break;
                                    case "11": { if (ExportType == "Excel") { a.Date11 = "<b style='color:Red'>A</b>"; } else { a.Date11 = "<b style='color:Red'>A</b>"; } } break;
                                    case "12": { if (ExportType == "Excel") { a.Date12 = "<b style='color:Red'>A</b>"; } else { a.Date12 = "<b style='color:Red'>A</b>"; } } break;
                                    case "13": { if (ExportType == "Excel") { a.Date13 = "<b style='color:Red'>A</b>"; } else { a.Date13 = "<b style='color:Red'>A</b>"; } } break;
                                    case "14": { if (ExportType == "Excel") { a.Date14 = "<b style='color:Red'>A</b>"; } else { a.Date14 = "<b style='color:Red'>A</b>"; } } break;
                                    case "15": { if (ExportType == "Excel") { a.Date15 = "<b style='color:Red'>A</b>"; } else { a.Date15 = "<b style='color:Red'>A</b>"; } } break;
                                    case "16": { if (ExportType == "Excel") { a.Date16 = "<b style='color:Red'>A</b>"; } else { a.Date16 = "<b style='color:Red'>A</b>"; } } break;
                                    case "17": { if (ExportType == "Excel") { a.Date17 = "<b style='color:Red'>A</b>"; } else { a.Date17 = "<b style='color:Red'>A</b>"; } } break;
                                    case "18": { if (ExportType == "Excel") { a.Date18 = "<b style='color:Red'>A</b>"; } else { a.Date18 = "<b style='color:Red'>A</b>"; } } break;
                                    case "19": { if (ExportType == "Excel") { a.Date19 = "<b style='color:Red'>A</b>"; } else { a.Date19 = "<b style='color:Red'>A</b>"; } } break;
                                    case "20": { if (ExportType == "Excel") { a.Date20 = "<b style='color:Red'>A</b>"; } else { a.Date20 = "<b style='color:Red'>A</b>"; } } break;
                                    case "21": { if (ExportType == "Excel") { a.Date21 = "<b style='color:Red'>A</b>"; } else { a.Date21 = "<b style='color:Red'>A</b>"; } } break;
                                    case "22": { if (ExportType == "Excel") { a.Date22 = "<b style='color:Red'>A</b>"; } else { a.Date22 = "<b style='color:Red'>A</b>"; } } break;
                                    case "23": { if (ExportType == "Excel") { a.Date23 = "<b style='color:Red'>A</b>"; } else { a.Date23 = "<b style='color:Red'>A</b>"; } } break;
                                    case "24": { if (ExportType == "Excel") { a.Date24 = "<b style='color:Red'>A</b>"; } else { a.Date24 = "<b style='color:Red'>A</b>"; } } break;
                                    case "25": { if (ExportType == "Excel") { a.Date25 = "<b style='color:Red'>A</b>"; } else { a.Date25 = "<b style='color:Red'>A</b>"; } } break;
                                    case "26": { if (ExportType == "Excel") { a.Date26 = "<b style='color:Red'>A</b>"; } else { a.Date26 = "<b style='color:Red'>A</b>"; } } break;
                                    case "27": { if (ExportType == "Excel") { a.Date27 = "<b style='color:Red'>A</b>"; } else { a.Date27 = "<b style='color:Red'>A</b>"; } } break;
                                    case "28": { if (ExportType == "Excel") { a.Date28 = "<b style='color:Red'>A</b>"; } else { a.Date28 = "<b style='color:Red'>A</b>"; } } break;
                                    case "29": { if (ExportType == "Excel") { a.Date29 = "<b style='color:Red'>A</b>"; } else { a.Date29 = "<b style='color:Red'>A</b>"; } } break;
                                    case "30": { if (ExportType == "Excel") { a.Date30 = "<b style='color:Red'>A</b>"; } else { a.Date30 = "<b style='color:Red'>A</b>"; } } break;
                                    case "31": { if (ExportType == "Excel") { a.Date31 = "<b style='color:Red'>A</b>"; } else { a.Date31 = "<b style='color:Red'>A</b>"; } } break;
                                    default: break;
                                }
                            }
                            for (var i = 0; i < Leave.Length; i++)
                            {
                                string Lvalue = Leave[i].ToString("dd");

                                switch (Lvalue)
                                {
                                    case "1":
                                    case "01": { if (ExportType == "Excel") { a.Date1 = "<b style='color:orange'>L</b>"; } else { a.Date1 = "<b style='color:orange'>L</b>"; } } break;
                                    case "2":
                                    case "02": { if (ExportType == "Excel") { a.Date2 = "<b style='color:orange'>L</b>"; } else { a.Date2 = "<b style='color:orange'>L</b>"; } } break;
                                    case "3":
                                    case "03": { if (ExportType == "Excel") { a.Date3 = "<b style='color:orange'>L</b>"; } else { a.Date3 = "<b style='color:orange'>L</b>"; } } break;
                                    case "4":
                                    case "04": { if (ExportType == "Excel") { a.Date4 = "<b style='color:orange'>L</b>"; } else { a.Date4 = "<b style='color:orange'>L</b>"; } } break;
                                    case "5":
                                    case "05": { if (ExportType == "Excel") { a.Date5 = "<b style='color:orange'>L</b>"; } else { a.Date5 = "<b style='color:orange'>L</b>"; } } break;
                                    case "6":
                                    case "06": { if (ExportType == "Excel") { a.Date6 = "<b style='color:orange'>L</b>"; } else { a.Date6 = "<b style='color:orange'>L</b>"; } } break;
                                    case "7":
                                    case "07": { if (ExportType == "Excel") { a.Date7 = "<b style='color:orange'>L</b>"; } else { a.Date7 = "<b style='color:orange'>L</b>"; } } break;
                                    case "8":
                                    case "08": { if (ExportType == "Excel") { a.Date8 = "<b style='color:orange'>L</b>"; } else { a.Date8 = "<b style='color:orange'>L</b>"; } } break;
                                    case "9":
                                    case "09": { if (ExportType == "Excel") { a.Date9 = "<b style='color: orange'>L</b>"; } else { a.Date9 = "<b style='color:orange'>L</b>"; } } break;
                                    case "10": { if (ExportType == "Excel") { a.Date10 = "<b style='color:orange'>L</b>"; } else { a.Date10 = "<b style='color:orange'>L</b>"; } } break;
                                    case "11": { if (ExportType == "Excel") { a.Date11 = "<b style='color:orange'>L</b>"; } else { a.Date11 = "<b style='color:orange'>L</b>"; } } break;
                                    case "12": { if (ExportType == "Excel") { a.Date12 = "<b style='color:orange'>L</b>"; } else { a.Date12 = "<b style='color:orange'>L</b>"; } } break;
                                    case "13": { if (ExportType == "Excel") { a.Date13 = "<b style='color:orange'>L</b>"; } else { a.Date13 = "<b style='color:orange'>L</b>"; } } break;
                                    case "14": { if (ExportType == "Excel") { a.Date14 = "<b style='color:orange'>L</b>"; } else { a.Date14 = "<b style='color:orange'>L</b>"; } } break;
                                    case "15": { if (ExportType == "Excel") { a.Date15 = "<b style='color:orange'>L</b>"; } else { a.Date15 = "<b style='color:orange'>L</b>"; } } break;
                                    case "16": { if (ExportType == "Excel") { a.Date16 = "<b style='color:orange'>L</b>"; } else { a.Date16 = "<b style='color:orange'>L</b>"; } } break;
                                    case "17": { if (ExportType == "Excel") { a.Date17 = "<b style='color:orange'>L</b>"; } else { a.Date17 = "<b style='color:orange'>L</b>"; } } break;
                                    case "18": { if (ExportType == "Excel") { a.Date18 = "<b style='color:orange'>L</b>"; } else { a.Date18 = "<b style='color:orange'>L</b>"; } } break;
                                    case "19": { if (ExportType == "Excel") { a.Date19 = "<b style='color:orange'>L</b>"; } else { a.Date19 = "<b style='color:orange'>L</b>"; } } break;
                                    case "20": { if (ExportType == "Excel") { a.Date20 = "<b style='color:orange'>L</b>"; } else { a.Date20 = "<b style='color:orange'>L</b>"; } } break;
                                    case "21": { if (ExportType == "Excel") { a.Date21 = "<b style='color:orange'>L</b>"; } else { a.Date21 = "<b style='color:orange'>L</b>"; } } break;
                                    case "22": { if (ExportType == "Excel") { a.Date22 = "<b style='color:orange'>L</b>"; } else { a.Date22 = "<b style='color:orange'>L</b>"; } } break;
                                    case "23": { if (ExportType == "Excel") { a.Date23 = "<b style='color:orange'>L</b>"; } else { a.Date23 = "<b style='color:orange'>L</b>"; } } break;
                                    case "24": { if (ExportType == "Excel") { a.Date24 = "<b style='color:orange'>L</b>"; } else { a.Date24 = "<b style='color:orange'>L</b>"; } } break;
                                    case "25": { if (ExportType == "Excel") { a.Date25 = "<b style='color:orange'>L</b>"; } else { a.Date25 = "<b style='color:orange'>L</b>"; } } break;
                                    case "26": { if (ExportType == "Excel") { a.Date26 = "<b style='color:orange'>L</b>"; } else { a.Date26 = "<b style='color:orange'>L</b>"; } } break;
                                    case "27": { if (ExportType == "Excel") { a.Date27 = "<b style='color:orange'>L</b>"; } else { a.Date27 = "<b style='color:orange'>L</b>"; } } break;
                                    case "28": { if (ExportType == "Excel") { a.Date28 = "<b style='color:orange'>L</b>"; } else { a.Date28 = "<b style='color:orange'>L</b>"; } } break;
                                    case "29": { if (ExportType == "Excel") { a.Date29 = "<b style='color:orange'>L</b>"; } else { a.Date29 = "<b style='color:orange'>L</b>"; } } break;
                                    case "30": { if (ExportType == "Excel") { a.Date30 = "<b style='color:orange'>L</b>"; } else { a.Date30 = "<b style='color:orange'>L</b>"; } } break;
                                    case "31": { if (ExportType == "Excel") { a.Date31 = "<b style='color:orange'>L</b>"; } else { a.Date31 = "<b style='color:orange'>L</b>"; } } break;
                                    default: break;
                                }
                            }
                        }
                        else
                        {
                            a.Date1 = "<b style='color:Green'>P</b>"; a.Date2 = "<b style='color:Green'>P</b>"; a.Date3 = "<b style='color:Green'>P</b>"; a.Date4 = "<b style='color:Green'>P</b>"; a.Date5 = "<b style='color:Green'>P</b>"; a.Date6 = "<b style='color:Green'>P</b>"; a.Date7 = "<b style='color:Green'>P</b>"; a.Date8 = "<b style='color:Green'>P</b>"; a.Date9 = "<b style='color:Green'>P</b>"; a.Date10 = "<b style='color:Green'>P</b>"; a.Date11 = "<b style='color:Green'>P</b>"; a.Date12 = "<b style='color:Green'>P</b>"; a.Date13 = "<b style='color:Green'>P</b>"; a.Date14 = "<b style='color:Green'>P</b>"; a.Date15 = "<b style='color:Green'>P</b>";
                            a.Date16 = "<b style='color:Green'>P</b>"; a.Date17 = "<b style='color:Green'>P</b>"; a.Date18 = "<b style='color:Green'>P</b>"; a.Date19 = "<b style='color:Green'>P</b>"; a.Date20 = "<b style='color:Green'>P</b>"; a.Date21 = "<b style='color:Green'>P</b>"; a.Date22 = "<b style='color:Green'>P</b>"; a.Date23 = "<b style='color:Green'>P</b>"; a.Date24 = "<b style='color:Green'>P</b>"; a.Date25 = "<b style='color:Green'>P</b>"; a.Date26 = "<b style='color:Green'>P</b>"; a.Date27 = "<b style='color:Green'>P</b>"; a.Date28 = "<b style='color:Green'>P</b>"; a.Date29 = "<b style='color:Green'>P</b>"; a.Date30 = "<b style='color:Green'>P</b>"; a.Date31 = "<b style='color:Green'>P</b>";

                            if (DateNow.Month == Convert.ToInt32(searchmonth))
                            {
                                // This month only
                                if (1 <= DateNow.Day) { a.Date1 = "<b style='color:Green'>P</b>"; } else { a.Date1 = ""; } if (2 <= DateNow.Day) { a.Date2 = "<b style='color:Green'>P</b>"; } else { a.Date2 = ""; } if (3 <= DateNow.Day) { a.Date3 = "<b style='color:Green'>P</b>"; } else { a.Date3 = ""; }
                                if (4 <= DateNow.Day) { a.Date4 = "<b style='color:Green'>P</b>"; } else { a.Date4 = ""; } if (5 <= DateNow.Day) { a.Date5 = "<b style='color:Green'>P</b>"; } else { a.Date5 = ""; } if (6 <= DateNow.Day) { a.Date6 = "<b style='color:Green'>P</b>"; } else { a.Date6 = ""; }
                                if (7 <= DateNow.Day) { a.Date7 = "<b style='color:Green'>P</b>"; } else { a.Date7 = ""; } if (8 <= DateNow.Day) { a.Date8 = "<b style='color:Green'>P</b>"; } else { a.Date8 = ""; } if (9 <= DateNow.Day) { a.Date9 = "<b style='color:Green'>P</b>"; } else { a.Date9 = ""; }
                                if (10 <= DateNow.Day) { a.Date10 = "<b style='color:Green'>P</b>"; } else { a.Date10 = ""; } if (11 <= DateNow.Day) { a.Date11 = "<b style='color:Green'>P</b>"; } else { a.Date11 = ""; } if (12 <= DateNow.Day) { a.Date12 = "<b style='color:Green'>P</b>"; } else { a.Date12 = ""; }
                                if (13 <= DateNow.Day) { a.Date13 = "<b style='color:Green'>P</b>"; } else { a.Date13 = ""; } if (14 <= DateNow.Day) { a.Date14 = "<b style='color:Green'>P</b>"; } else { a.Date14 = ""; } if (15 <= DateNow.Day) { a.Date15 = "<b style='color:Green'>P</b>"; } else { a.Date15 = ""; }
                                if (16 <= DateNow.Day) { a.Date16 = "<b style='color:Green'>P</b>"; } else { a.Date16 = ""; } if (17 <= DateNow.Day) { a.Date17 = "<b style='color:Green'>P</b>"; } else { a.Date17 = ""; } if (18 <= DateNow.Day) { a.Date18 = "<b style='color:Green'>P</b>"; } else { a.Date18 = ""; }
                                if (19 <= DateNow.Day) { a.Date19 = "<b style='color:Green'>P</b>"; } else { a.Date19 = ""; } if (20 <= DateNow.Day) { a.Date20 = "<b style='color:Green'>P</b>"; } else { a.Date20 = ""; } if (21 <= DateNow.Day) { a.Date21 = "<b style='color:Green'>P</b>"; } else { a.Date21 = ""; }
                                if (22 <= DateNow.Day) { a.Date22 = "<b style='color:Green'>P</b>"; } else { a.Date22 = ""; } if (23 <= DateNow.Day) { a.Date23 = "<b style='color:Green'>P</b>"; } else { a.Date23 = ""; } if (24 <= DateNow.Day) { a.Date24 = "<b style='color:Green'>P</b>"; } else { a.Date24 = ""; }
                                if (25 <= DateNow.Day) { a.Date25 = "<b style='color:Green'>P</b>"; } else { a.Date25 = ""; } if (26 <= DateNow.Day) { a.Date26 = "<b style='color:Green'>P</b>"; } else { a.Date26 = ""; } if (27 <= DateNow.Day) { a.Date27 = "<b style='color:Green'>P</b>"; } else { a.Date27 = ""; }
                                if (28 <= DateNow.Day) { a.Date28 = "<b style='color:Green'>P</b>"; } else { a.Date28 = ""; } if (29 <= DateNow.Day) { a.Date29 = "<b style='color:Green'>P</b>"; } else { a.Date29 = ""; } if (30 <= DateNow.Day) { a.Date30 = "<b style='color:Green'>P</b>"; } else { a.Date30 = ""; }
                                if (31 <= DateNow.Day) { a.Date31 = "<b style='color:Green'>P</b>"; } else { a.Date31 = ""; }
                            }
                            else
                            {
                                // not this month
                                a.Date1 = "<b style='color:Green'>P</b>"; a.Date2 = "<b style='color:Green'>P</b>"; a.Date3 = "<b style='color:Green'>P</b>"; a.Date4 = "<b style='color:Green'>P</b>"; a.Date5 = "<b style='color:Green'>P</b>"; a.Date6 = "<b style='color:Green'>P</b>"; a.Date7 = "<b style='color:Green'>P</b>"; a.Date8 = "<b style='color:Green'>P</b>"; a.Date9 = "<b style='color:Green'>P</b>"; a.Date10 = "<b style='color:Green'>P</b>"; a.Date11 = "<b style='color:Green'>P</b>"; a.Date12 = "<b style='color:Green'>P</b>"; a.Date13 = "<b style='color:Green'>P</b>"; a.Date14 = "<b style='color:Green'>P</b>"; a.Date15 = "<b style='color:Green'>P</b>";
                                a.Date16 = "<b style='color:Green'>P</b>"; a.Date17 = "<b style='color:Green'>P</b>"; a.Date18 = "<b style='color:Green'>P</b>"; a.Date19 = "<b style='color:Green'>P</b>"; a.Date20 = "<b style='color:Green'>P</b>"; a.Date21 = "<b style='color:Green'>P</b>"; a.Date22 = "<b style='color:Green'>P</b>"; a.Date23 = "<b style='color:Green'>P</b>"; a.Date24 = "<b style='color:Green'>P</b>"; a.Date25 = "<b style='color:Green'>P</b>"; a.Date26 = "<b style='color:Green'>P</b>"; a.Date27 = "<b style='color:Green'>P</b>"; a.Date28 = "<b style='color:Green'>P</b>"; a.Date29 = "<b style='color:Green'>P</b>"; a.Date30 = "<b style='color:Green'>P</b>"; a.Date31 = "<b style='color:Green'>P</b>";
                            }
                        }


                        // Find the Holy Day Leave List
                        List<string> value = getAllSundays(Convert.ToInt32(DateTime.Now.Year.ToString()));
                        var currentMnthHolidays = (from v in value where v.Substring(3, 2) == searchmonth select v);
                        int Hcount = 0;
                        foreach (string s in currentMnthHolidays)
                        {
                            //10/10/2013
                            switch (s.Substring(0, 2))
                            {
                                case "01": { if (ExportType == "Excel")a.Date1 = "H"; else { a.Date1 = "<b style='color:blue'>H</b>"; } } break;
                                case "02": { if (ExportType == "Excel")a.Date2 = "H"; else { a.Date2 = "<b style='color:blue'>H</b>"; } } break;
                                case "03": { if (ExportType == "Excel")a.Date3 = "H"; else { a.Date3 = "<b style='color:blue'>H</b>"; } } break;
                                case "04": { if (ExportType == "Excel")a.Date4 = "H"; else { a.Date4 = "<b style='color:blue'>H</b>"; } } break;
                                case "05": { if (ExportType == "Excel")a.Date5 = "H"; else { a.Date5 = "<b style='color:blue'>H</b>"; } } break;
                                case "06": { if (ExportType == "Excel")a.Date6 = "H"; else { a.Date6 = "<b style='color:blue'>H</b>"; } } break;
                                case "07": { if (ExportType == "Excel")a.Date7 = "H"; else { a.Date7 = "<b style='color:blue'>H</b>"; } } break;
                                case "08": { if (ExportType == "Excel")a.Date8 = "H"; else { a.Date8 = "<b style='color:blue'>H</b>"; } } break;
                                case "09": { if (ExportType == "Excel")a.Date9 = "H"; else { a.Date9 = "<b style='color:blue'>H</b>"; } } break;
                                case "10": { if (ExportType == "Excel")a.Date10 = "H"; else { a.Date10 = "<b style='color:blue'>H</b>"; } } break;
                                case "11": { if (ExportType == "Excel")a.Date11 = "H"; else { a.Date11 = "<b style='color:blue'>H</b>"; } } break;
                                case "12": { if (ExportType == "Excel")a.Date12 = "H"; else { a.Date12 = "<b style='color:blue'>H</b>"; } } break;
                                case "13": { if (ExportType == "Excel")a.Date13 = "H"; else { a.Date13 = "<b style='color:blue'>H</b>"; } } break;
                                case "14": { if (ExportType == "Excel")a.Date14 = "H"; else { a.Date14 = "<b style='color:blue'>H</b>"; } } break;
                                case "15": { if (ExportType == "Excel")a.Date15 = "H"; else { a.Date15 = "<b style='color:blue'>H</b>"; } } break;
                                case "16": { if (ExportType == "Excel")a.Date16 = "H"; else { a.Date16 = "<b style='color:blue'>H</b>"; } } break;
                                case "17": { if (ExportType == "Excel")a.Date17 = "H"; else { a.Date17 = "<b style='color:blue'>H</b>"; } } break;
                                case "18": { if (ExportType == "Excel")a.Date18 = "H"; else { a.Date18 = "<b style='color:blue'>H</b>"; } } break;
                                case "19": { if (ExportType == "Excel")a.Date19 = "H"; else { a.Date19 = "<b style='color:blue'>H</b>"; } } break;
                                case "20": { if (ExportType == "Excel")a.Date20 = "H"; else { a.Date20 = "<b style='color:blue'>H</b>"; } } break;
                                case "21": { if (ExportType == "Excel")a.Date21 = "H"; else { a.Date21 = "<b style='color:blue'>H</b>"; } } break;
                                case "22": { if (ExportType == "Excel")a.Date22 = "H"; else { a.Date22 = "<b style='color:blue'>H</b>"; } } break;
                                case "23": { if (ExportType == "Excel")a.Date23 = "H"; else { a.Date23 = "<b style='color:blue'>H</b>"; } } break;
                                case "24": { if (ExportType == "Excel")a.Date24 = "H"; else { a.Date24 = "<b style='color:blue'>H</b>"; } } break;
                                case "25": { if (ExportType == "Excel")a.Date25 = "H"; else { a.Date25 = "<b style='color:blue'>H</b>"; } } break;
                                case "26": { if (ExportType == "Excel")a.Date26 = "H"; else { a.Date26 = "<b style='color:blue'>H</b>"; } } break;
                                case "27": { if (ExportType == "Excel")a.Date27 = "H"; else { a.Date27 = "<b style='color:blue'>H</b>"; } } break;
                                case "28": { if (ExportType == "Excel")a.Date28 = "H"; else { a.Date28 = "<b style='color:blue'>H</b>"; } } break;
                                case "29": { if (ExportType == "Excel")a.Date29 = "H"; else { a.Date29 = "<b style='color:blue'>H</b>"; } } break;
                                case "30": { if (ExportType == "Excel")a.Date30 = "H"; else { a.Date30 = "<b style='color:blue'>H</b>"; } } break;
                                case "31": { if (ExportType == "Excel")a.Date31 = "H"; else { a.Date31 = "<b style='color:blue'>H</b>"; } } break;
                                default: break;
                            }
                            Hcount = Hcount + 1;
                        }
                        a.HolidayCountList = Hcount;
                        if (searchmonth == "01" || searchmonth == "03" || searchmonth == "05" || searchmonth == "07" || searchmonth == "08" || searchmonth == "10" || searchmonth == "12")
                        {
                            if (DateNow.Month == Convert.ToInt32(searchmonth) && DateNow.Year == Convert.ToInt32(year))
                            {
                                a.noofpre = ((DateNow.Day) - (a.AbsentCountList + a.LeaveCountList));
                                a.numofworkdays = 31 - a.HolidayCountList;

                                a.percround = (Convert.ToDecimal(Convert.ToDecimal(a.noofpre) / DateNow.Day) * 100);
                                a.Percentage = (Math.Round(a.percround, 1));
                                // Total attendance
                                a.totalWorkingday = Convert.ToInt32(a.numofworkdays);
                                a.totalAttendance = Convert.ToInt32(a.noofpre);
                                a.totalPercentage = 100;
                                a.totalPercentage = (Math.Round(a.totalPercentage, 1));
                                if (DateNow.Month == Convert.ToInt32(searchmonth)) { if (reduceCount == 1) { a.noofpre = a.noofpre - 1; } else { } }
                            }
                            else
                            {
                                a.noofpre = (31 - a.HolidayCountList) - (a.AbsentCountList + a.LeaveCountList);
                                a.numofworkdays = 31 - a.HolidayCountList;


                                a.percround = (Convert.ToDecimal(Convert.ToDecimal(a.noofpre) / a.numofworkdays) * 100);
                                a.Percentage = (Math.Round(a.percround, 1));
                                // Total attendance
                                a.totalWorkingday = Convert.ToInt32(a.numofworkdays);
                                a.totalAttendance = Convert.ToInt32(a.noofpre);
                                a.totalPercentage = 100;
                                a.totalPercentage = (Math.Round(a.totalPercentage, 1));
                                if (DateNow.Month == Convert.ToInt32(searchmonth)) { if (reduceCount == 1) { a.noofpre = a.noofpre - 1; } else { } }
                            }
                        }
                        else if (searchmonth == "02")
                        {
                            if (DateNow.Month == Convert.ToInt32(searchmonth) && DateNow.Year == Convert.ToInt32(year))
                            {
                                a.noofpre = ((DateNow.Day) - (a.AbsentCountList + a.LeaveCountList));
                                a.numofworkdays = 28 - a.HolidayCountList;

                                a.percround = (Convert.ToDecimal(Convert.ToDecimal(a.noofpre) / DateNow.Day) * 100);
                                a.Percentage = (Math.Round(a.percround, 1));
                                // Total attendance
                                a.totalWorkingday = Convert.ToInt32(a.numofworkdays);
                                a.totalAttendance = Convert.ToInt32(a.noofpre);
                                a.totalPercentage = 100;
                                a.totalPercentage = (Math.Round(a.totalPercentage, 1));
                                if (DateNow.Month == Convert.ToInt32(searchmonth)) { if (reduceCount == 1) { a.noofpre = a.noofpre - 1; } else { } }
                            }
                            else
                            {
                                a.noofpre = (28 - a.HolidayCountList) - (a.AbsentCountList + a.LeaveCountList);
                                a.numofworkdays = 28 - a.HolidayCountList;


                                a.percround = (Convert.ToDecimal(Convert.ToDecimal(a.noofpre) / a.numofworkdays) * 100);
                                a.Percentage = (Math.Round(a.percround, 1));
                                // Total attendance
                                a.totalWorkingday = Convert.ToInt32(a.numofworkdays);
                                a.totalAttendance = Convert.ToInt32(a.noofpre);
                                a.totalPercentage = 100;
                                a.totalPercentage = (Math.Round(a.totalPercentage, 1));
                                if (DateNow.Month == Convert.ToInt32(searchmonth)) { if (reduceCount == 1) { a.noofpre = a.noofpre - 1; } else { } }
                            }
                        }
                        else
                        {

                            if (DateNow.Month == Convert.ToInt32(searchmonth) && DateNow.Year == Convert.ToInt32(year))
                            {
                                a.noofpre = ((DateNow.Day) - (a.AbsentCountList + a.LeaveCountList));
                                a.numofworkdays = 30 - a.HolidayCountList;

                                a.percround = (Convert.ToDecimal(Convert.ToDecimal(a.noofpre) / DateNow.Day) * 100);
                                a.Percentage = (Math.Round(a.percround, 1));
                                // Total attendance
                                a.totalWorkingday = Convert.ToInt32(a.numofworkdays);
                                a.totalAttendance = Convert.ToInt32(a.noofpre);
                                a.totalPercentage = 100;
                                a.totalPercentage = (Math.Round(a.totalPercentage, 1));
                                if (DateNow.Month == Convert.ToInt32(searchmonth)) { if (reduceCount == 1) { a.noofpre = a.noofpre - 1; } else { } }
                            }
                            else
                            {
                                a.noofpre = (30 - a.HolidayCountList) - (a.AbsentCountList + a.LeaveCountList);
                                a.numofworkdays = 30 - a.HolidayCountList;


                                a.percround = (Convert.ToDecimal(Convert.ToDecimal(a.noofpre) / a.numofworkdays) * 100);
                                a.Percentage = (Math.Round(a.percround, 1));
                                // Total attendance
                                a.totalWorkingday = Convert.ToInt32(a.numofworkdays);
                                a.totalAttendance = Convert.ToInt32(a.noofpre);
                                a.totalPercentage = 100;
                                a.totalPercentage = (Math.Round(a.totalPercentage, 1));
                                if (DateNow.Month == Convert.ToInt32(searchmonth)) { if (reduceCount == 1) { a.noofpre = a.noofpre - 1; } else { } }
                            }
                        }
                    }


                    if (DriverList != null && DriverList.Count > 0)
                    {
                        if (ExportType == "Excel")
                        {
                            if (searchmonth == "01" || searchmonth == "03" || searchmonth == "05" || searchmonth == "07" || searchmonth == "08" || searchmonth == "10" || searchmonth == "12")
                            {
                                string headerTable = @"<Table border='1px' cellpadding='9' cellspacing='0'><tr><td colspan='2'>Campus - " + campus + "</td><td colspan='38' align='center' style='font-size: large;'>The Indian Public School</td></tr></b></Table>";
                                var studLst = DriverList.First().Value.ToList();

                                var List = (from s in studLst
                                            orderby s.Id
                                            select s).ToList();
                                ExptToXL_FinalResult(List, "AttendanceReports", (items => new
                                {
                                    items.Campus,
                                    items.Name,
                                    items.DriverIdNo,
                                    items.Date1,
                                    items.Date2,
                                    items.Date3,
                                    items.Date4,
                                    items.Date5,
                                    items.Date6,
                                    items.Date7,
                                    items.Date8,
                                    items.Date9,
                                    items.Date10,
                                    items.Date11,
                                    items.Date12,
                                    items.Date13,
                                    items.Date14,
                                    items.Date15,
                                    items.Date16,
                                    items.Date17,
                                    items.Date18,
                                    items.Date19,
                                    items.Date20,
                                    items.Date21,
                                    items.Date22,
                                    items.Date23,
                                    items.Date24,
                                    items.Date25,
                                    items.Date26,
                                    items.Date27,
                                    items.Date28,
                                    items.Date29,
                                    items.Date30,
                                    items.Date31,
                                    items.AbsentCountList,
                                    items.LeaveCountList,
                                    items.HolidayCountList,
                                    items.noofpre,

                                    items.Percentage,

                                    items.totalWorkingday,
                                    items.totalPercentage
                                }), headerTable);
                                return new EmptyResult();
                            }
                            else if (searchmonth == "02")
                            {
                                string headerTable = @"<Table border='1px' cellpadding='9' cellspacing='0'><tr><td colspan='2'>Campus - " + campus + "</td><td colspan='35' align='center' style='font-size: large;'>The Indian Public School</td></tr></b></Table>";
                                var studLst = DriverList.First().Value.ToList();

                                var List = (from s in studLst
                                            orderby s.Id
                                            select s).ToList();
                                ExptToXL_FinalResult(List, "AttendanceReports", (items => new
                                {
                                    items.Campus,
                                    items.Name,
                                    items.DriverIdNo,
                                    items.Date1,
                                    items.Date2,
                                    items.Date3,
                                    items.Date4,
                                    items.Date5,
                                    items.Date6,
                                    items.Date7,
                                    items.Date8,
                                    items.Date9,
                                    items.Date10,
                                    items.Date11,
                                    items.Date12,
                                    items.Date13,
                                    items.Date14,
                                    items.Date15,
                                    items.Date16,
                                    items.Date17,
                                    items.Date18,
                                    items.Date19,
                                    items.Date20,
                                    items.Date21,
                                    items.Date22,
                                    items.Date23,
                                    items.Date24,
                                    items.Date25,
                                    items.Date26,
                                    items.Date27,
                                    items.Date28,
                                    items.AbsentCountList,
                                    items.LeaveCountList,
                                    items.HolidayCountList,
                                    items.noofpre,
                                    items.Percentage,
                                    items.totalWorkingday,
                                    items.totalPercentage
                                }), headerTable);
                                return new EmptyResult();
                            }
                            else
                            {
                                string headerTable = @"<Table border='1px' cellpadding='9' cellspacing='0'><tr><td colspan='2'>Campus - " + campus + "</td><td colspan='38' align='center' style='font-size: large;'>The Indian Public School</td></tr></b></Table>";
                                var studLst = DriverList.First().Value.ToList();

                                var List = (from s in studLst
                                            orderby s.Id
                                            select s).ToList();
                                ExptToXL_FinalResult(List, "AttendanceReports", (items => new
                                {
                                    items.Campus,
                                    items.Name,
                                    items.DriverIdNo,
                                    items.Date1,
                                    items.Date2,
                                    items.Date3,
                                    items.Date4,
                                    items.Date5,
                                    items.Date6,
                                    items.Date7,
                                    items.Date8,
                                    items.Date9,
                                    items.Date10,
                                    items.Date11,
                                    items.Date12,
                                    items.Date13,
                                    items.Date14,
                                    items.Date15,
                                    items.Date16,
                                    items.Date17,
                                    items.Date18,
                                    items.Date19,
                                    items.Date20,
                                    items.Date21,
                                    items.Date22,
                                    items.Date23,
                                    items.Date24,
                                    items.Date25,
                                    items.Date26,
                                    items.Date27,
                                    items.Date28,
                                    items.Date29,
                                    items.Date30,
                                    items.AbsentCountList,
                                    items.LeaveCountList,
                                    items.HolidayCountList,
                                    items.noofpre,
                                    items.Percentage,

                                    items.totalWorkingday,
                                    items.totalPercentage
                                }), headerTable);
                                return new EmptyResult();
                            }


                        }
                        else
                        {
                            long totalrecords = DriverList.First().Key;
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var jsondat = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalrecords,

                                rows = (from items in DriverList.First().Value
                                        select new
                                        {
                                            i = items.Id,
                                            cell = new string[] {
                               
                               items.Campus,items.Name,items.DriverIdNo,
                               items.Date1,items.Date2,items.Date3,items.Date4,items.Date5,items.Date6,items.Date7,items.Date8,items.Date9,items.Date10,items.Date11,items.Date12,items.Date13,items.Date14,items.Date15,items.Date16,items.Date17,items.Date18,items.Date19,
                               items.Date20,items.Date21,items.Date22,items.Date23,items.Date24,items.Date25,items.Date26,items.Date27,items.Date28,items.Date29,items.Date30,items.Date31,items.AbsentCountList.ToString(),items.LeaveCountList.ToString(),items.HolidayCountList.ToString(),items.noofpre.ToString(),items.Percentage.ToString(),items.totalWorkingday.ToString(),items.totalPercentage.ToString(),
                            }
                                        })
                            };
                            return Json(jsondat, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AttendancePolicy");
                throw ex;
            }
        }

        public List<string> getAllSundays(int Year)
        {
            List<string> strDates = new List<string>();
            for (int month = 1; month <= 12; month++)
            {
                DateTime dt = new DateTime(Year, month, 1);
                int firstSundayOfMonth = (int)dt.DayOfWeek;
                if (firstSundayOfMonth != 0)
                {
                    dt = dt.AddDays((6 - firstSundayOfMonth) + 1);
                }
                while (dt.Month == month)
                {
                    strDates.Add(dt.ToString("dd/MM/yyyy"));
                    dt = dt.AddDays(7);
                }
            }
            return strDates;
        }
        public void ExptToXL_FinalResult<T, TResult>(IList<T> stuList, string filename, Func<T, TResult> selector, string headerTable)
        {
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=" + filename + ".xls");
            Response.ContentType = "application/vnd.ms-excel";
            System.IO.StringWriter stw = new System.IO.StringWriter();
            HtmlTextWriter htextw = new HtmlTextWriter(stw);
            DataGrid dg = new DataGrid();

            dg.HeaderStyle.BackColor = System.Drawing.Color.FromName("#B0B0B0");
            dg.HeaderStyle.Font.Bold = true;
            dg.HeaderStyle.ForeColor = System.Drawing.Color.White;

            dg.DataSource = stuList.Select(selector);
            dg.DataBind();
            dg.RenderControl(htextw);
            Response.Write(headerTable);
            // Response.Write(stw.ToString().Remove(stw.ToString().IndexOf("<tr "), (stw.ToString().IndexOf("</tr>") - stw.ToString().IndexOf("<tr ")) + 5));
            Response.Write(stw.ToString());
            Response.End();
        }
        #region Driver Attendance Monthly Report

        public ActionResult DriverAttendanceMonthlyReport()
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
                    ViewBag.campusddl = CampusMaster.First().Value;
                    int[] years = new int[15];
                    DateTime daytime = DateTime.Now;
                    int CurYear = daytime.Year;
                    int CurMonth = daytime.Month;
                    ViewBag.CurYear = CurYear;
                    ViewBag.CurMonth = CurMonth;
                    CurYear = CurYear - 5;
                    for (int i = 0; i < 15; i++)
                    {
                        years[i] = CurYear + i;
                    }
                    ViewBag.years = years;

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


        public ActionResult DriverAttendanceMonthlyReportJqGrid(string DriverName, string DriverIdNo, string Campus, string FromDate, string ToDate, int? CurrMonth, int? CurrYear, int rows, string sidx, string sord, int? page = 1, int? ExptXl = 0)
        {
            try
            {
                DateTime frdate = Convert.ToDateTime(FromDate);

                DateTime todate = Convert.ToDateTime(ToDate);
                int workdays = Convert.ToInt32((todate - frdate).TotalDays) + 1;
                int holydays = CountSundays(frdate, todate);
                int workingdays = workdays - holydays;
                CurrMonth = frdate.Month;
                CurrYear = frdate.Year;
                IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                DateTime DateNow = DateTime.Now;
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                TransportService tns = new TransportService();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (!string.IsNullOrEmpty(DriverName))
                    criteria.Add("Name", DriverName.Trim());
                if (!string.IsNullOrEmpty(DriverIdNo))
                    criteria.Add("DriverIdNo", DriverIdNo.Trim());
                if (!string.IsNullOrEmpty(Campus))
                {
                    criteria.Add("Campus", Campus);
                }
                else
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
                criteria.Add("Status", "Registered");
                criteria.Add("Designation", "Driver");
                Dictionary<long, IList<DriverAttendanceReport>> DriverList = tns.GetDriverListListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                criteria.Clear();
                //if (CurrMonth >= 0)
                //    criteria.Add("AbMonth", CurrMonth);
                //if (CurrYear >= 0)
                //    criteria.Add("AbYear", CurrYear);

                if ((!string.IsNullOrEmpty(FromDate)) && (!string.IsNullOrEmpty(ToDate)))
                {
                    if (!string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate))
                    {
                        DateTime[] fromto = new DateTime[2];
                        FromDate = FromDate.Trim();
                        ToDate = ToDate.Trim();
                        //IFormatProvider culture = System.Threading.Thread.CurrentThread.CurrentCulture;
                        //fromto[0] = DateTime.Parse(FromDate, culture, System.Globalization.DateTimeStyles.AssumeLocal);
                        //fromto[1] = DateTime.Parse(ToDate, culture, System.Globalization.DateTimeStyles.AssumeLocal);


                        fromto[0] = DateTime.Parse(FromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        fromto[1] = DateTime.Parse(ToDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);// Convert.ToDateTime(To + " " + "23:59:59");
                        criteria.Add("AbsentDate", fromto);

                    }


                }

                Dictionary<long, IList<DriverAttendanceMonthReport>> AttendanceList = tns.GetMonthlyAbsentListForAnAttendanceListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria);
                List<DriverAttendanceMonthReport> alreadyExists = AttendanceList.FirstOrDefault().Value.ToList();
                IEnumerable<long> blkLong = from p in alreadyExists
                                            orderby p.PreRegNum ascending
                                            select p.PreRegNum;
                long[] attids = blkLong.ToArray();

                foreach (DriverAttendanceReport a in DriverList.FirstOrDefault().Value)
                {
                    if (attids.Contains((a.DriverRegNo)))
                    {
                        var GetList = (from u in AttendanceList.First().Value
                                       where u.PreRegNum == Convert.ToInt64(a.DriverRegNo)
                                       select u).ToList();
                        a.Leave = GetList[0].Leave;
                        a.Absent = GetList[0].Absent;
                        a.TotalLandA = GetList[0].TotalLandA;
                        //   a.AbMonth = Convert.ToInt32(CurrMonth);
                        //  a.AbYear = Convert.ToInt32(CurrYear);
                        a.FromDate = FromDate;
                        a.ToDate = ToDate;
                        a.NoOfPre = Convert.ToInt32(workingdays) - a.TotalLandA;
                        a.TotalWorkingDay = Convert.ToInt32(workingdays);

                    }
                    else
                    {
                        a.Leave = 0; a.Absent = 0; a.TotalLandA = 0; a.AbMonth = Convert.ToInt32(CurrMonth); a.AbYear = Convert.ToInt32(CurrYear); a.FromDate = FromDate; a.ToDate = ToDate;
                        if (DateNow.Month == Convert.ToInt32(CurrMonth))
                        {

                            a.NoOfPre = Convert.ToInt32(workingdays) - a.TotalLandA;
                            a.TotalWorkingDay = Convert.ToInt32(workingdays);
                        }
                        else
                        {
                            //int days = DateTime.DaysInMonth(Convert.ToInt32(CurrYear), Convert.ToInt32(CurrMonth));
                            //a.NoOfPre = days;
                            //a.TotalWorkingDay = days;


                            a.NoOfPre = Convert.ToInt32(workingdays) - a.TotalLandA;
                            a.TotalWorkingDay = Convert.ToInt32(workingdays);
                        }
                    }
                }





                if (DriverList != null && DriverList.Count > 0 && DriverList.FirstOrDefault().Key > 0 && DriverList.FirstOrDefault().Value.Count > 0)
                {
                    if (ExptXl == 1)
                    {
                        string headerTable = @"<Table border='1px' cellpadding='9' cellspacing='0'><tr><td colspan='2'>Campus - " + Campus + "</td><td colspan='9' align='center' style='font-size: large;'>The Indian Public School_Driver Attendance Report From  " + FromDate + " To " + ToDate + "</td></tr></b></Table>";
                        var List = DriverList.First().Value.ToList();
                        ExptToXL_FinalResult(List, "Driver_Attendance_Monthly_Report", (items => new
                        {
                            items.Id,
                            items.Campus,
                            items.Name,
                            items.DriverIdNo,
                            items.FromDate,
                            items.ToDate,
                            items.Leave,
                            items.Absent,
                            items.TotalLandA,
                            items.NoOfPre,

                            items.TotalWorkingDay

                        }), headerTable);
                        return new EmptyResult();
                    }
                    else
                    {
                        long totalRecords = DriverList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                        var jsonData = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalRecords,
                            rows = (
                                 from items in DriverList.First().Value
                                 select new
                                 {
                                     i = items.Id,
                                     cell = new string[] { 
                                     items.Id.ToString(),
                                     items.Campus,
                                     items.Name,
                                     items.DriverIdNo,
                                     items.FromDate,
                                     items.ToDate,
                                     items.Leave.ToString(),
                                     items.Absent.ToString(),
                                     items.TotalLandA.ToString(),
                                     items.NoOfPre.ToString(),
                                     items.TotalWorkingDay.ToString(),
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
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }


        #endregion


        #endregion

        #region Count Sundays b/w two date
        // by Rajkumar
        public static int CountSundays(DateTime fromDate, DateTime toDate)
        {
            int sunday = 0;
            TimeSpan testSpan = new TimeSpan(6, 0, 0, 0);
            TimeSpan actualSpan = toDate - fromDate;
            if (actualSpan >= testSpan)
            {

                DateTime date = toDate;
                while (date > fromDate)
                {
                    if (date.DayOfWeek == DayOfWeek.Sunday)
                    {
                        sunday = sunday + 1;
                    }
                    date = date.AddDays(-1);


                }
                return sunday;
            }
            return sunday;
        }
        #endregion

        #region Driver ID Card
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
                // AdmissionManagementService ts = new AdmissionManagementService();

                var prereg = Session["IdCardData"].ToString().Split(',');
                // long[] id = new long[prereg.Length];
                string[] id = new string[prereg.Length];

                int j = 0;
                foreach (string val in prereg)
                {
                    // id[j] = Convert.ToInt64(val);
                    id[j] = val;
                    j++;
                }

                System.Text.StringBuilder html = new System.Text.StringBuilder();
                int rowcnt = 0;
                //var mobile1; var mobile;
                for (int i = 0; i < Convert.ToInt32(prereg.Count()); i = i + 2)
                {
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("DriverRegNo", Convert.ToInt64(id[i]));
                    Dictionary<long, IList<DriverMaster>> DriverCount1 = ts.GetDriverListWithEQsearchCriteria(0, 10000, string.Empty, string.Empty, criteria);
                    if (DriverCount1.First().Value[0].BGRP == "Not Given")
                    {
                        DriverCount1.First().Value[0].BGRP = "";
                    }
                    var mobile = DriverCount1.First().Value[0].ContactNo;
                    if ((mobile != "") && (mobile != null))
                    {
                        mobile = mobile.Split(',')[0].ToString();
                    }
                    else
                    {
                        mobile = "";
                    }
                    Dictionary<long, IList<DriverMaster>> DriverCount2 = null;
                    CampusEmailId campusemailiddtls2 = null;
                    UserService us = new UserService();
                    CampusEmailId campusemailiddtls = us.GetCampusEmailIdByCampusWithServer(DriverCount1.First().Value[0].Campus, "Test");
                    if (i + 1 < Convert.ToInt32(prereg.Count()))
                    {
                        Dictionary<string, object> criteria2 = new Dictionary<string, object>();
                        criteria2.Add("DriverRegNo", Convert.ToInt64(id[i + 1]));
                        DriverCount2 = ts.GetDriverListWithEQsearchCriteria(0, 10000, string.Empty, string.Empty, criteria2);
                        campusemailiddtls2 = us.GetCampusEmailIdByCampusWithServer(DriverCount2.First().Value[0].Campus, "Test");
                        if (DriverCount2.First().Value[0].BGRP == "Not Given")
                        {
                            DriverCount2.First().Value[0].BGRP = "";
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
                    html.Append("<td width='41%'><span><strong>:&nbsp;" + DriverCount1.First().Value[0].Name + "</strong><br/>");
                    html.Append("<strong>:&nbsp;" + DriverCount1.First().Value[0].DriverIdNo + "</strong><br/>");
                    html.Append("<strong>:&nbsp;" + DriverCount1.First().Value[0].BGRP + "</strong><br/>");
                    html.Append("<strong>:&nbsp;" + DriverCount1.First().Value[0].ContactNo + "</strong></span></td><td></td>");
                    html.Append("</table><br/><br/>");
                    html.Append("</div>");
                    html.Append("<div id='onbottom' style='width: 350px;'>" + campusemailiddtls.SchoolName + ", " + campusemailiddtls.Address + " -" + campusemailiddtls.PinCode + " " +campusemailiddtls.PhoneNumber + " | " + campusemailiddtls.WebSiteName + "");
                    html.Append("</div>");
                    html.Append("<td/>");                    
                    if (i + 1 < Convert.ToInt32(prereg.Count()))
                    {
                        var mobile1 = DriverCount2.First().Value[0].ContactNo;
                        if ((mobile1 != "") && (mobile1 != null))
                        {
                            mobile1 = mobile1.Split(',')[0].ToString();
                        }
                        else
                        {
                            mobile1 = "";
                        }
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
                        html.Append("<td width='41%'><span><strong>:&nbsp;" + DriverCount2.First().Value[0].Name + "</strong><br/>");
                        html.Append("<strong>:&nbsp;" + DriverCount2.First().Value[0].DriverIdNo + "</strong><br/>");
                        html.Append("<strong>:&nbsp;" + DriverCount2.First().Value[0].BGRP + "</strong><br/>");
                        html.Append("<strong>:&nbsp;" + DriverCount2.First().Value[0].ContactNo + "</strong></span></td><td></td>");
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

        #endregion

        #region Driver family details
        public ActionResult familyjqgrid(string DriverRegNo, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("DriverRegNo", Convert.ToInt64(DriverRegNo));

                Dictionary<long, IList<DriverFamilyDetails>> FamilyDetails = ts.GetDriverFamilyDetailsListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);

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
                          
                            items.Id.ToString(),
                            items.FRelationship,
                            items.FName,
                            items.FAge.ToString(),                           
                            items.FOccupation,
                            items.ContactNo,
                            items.CreatedDate.ToString(),
                            items.CreatedBy
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

        public ActionResult EditFamily(DriverFamilyDetails fd)
        {
            try
            {

                ts.CreateOrUpdateDriverFamilyDetails(fd);
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult AddDriverFamilydetails(string relationtype, string FMname, string FMDOB, string FMoccupation, string FMmobile, string age, string DriverRegNo)
        {
            try
            {
                if (age == "")
                {
                    age = "0";
                }
                DateTime dob = Convert.ToDateTime(FMDOB);

                DriverFamilyDetails fdet = new DriverFamilyDetails();
                fdet.DriverRegNo = Convert.ToInt64(DriverRegNo);
                fdet.FRelationship = relationtype;
                fdet.FName = FMname;
                fdet.FDob = dob;
                fdet.FAge = Convert.ToInt32(age);
                fdet.ContactNo = FMmobile;
                fdet.FOccupation = FMoccupation;
                fdet.ModifiedDate = DateTime.Now;
                fdet.CreatedDate = DateTime.Now;
                ts.CreateOrUpdateDriverFamilyDetails(fdet);
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        #endregion

        #region Route Configuration
        public ActionResult RouteConfiguration(long Id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MastersService ms = new MastersService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    AdmissionManagementService ams = new AdmissionManagementService();
                    TransportService ts = new TransportService();
                    Dictionary<long, IList<StudentLocationMaster>> StudentLocationMaster = ams.GetStudentLocationMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                    RouteMaster routeMaster = new TIPS.Entities.TransportEntities.RouteMaster();
                    routeMaster = ts.GetRouteMasterDetailsById(Id);
                    ViewBag.RouteLocations = StudentLocationMaster.First().Value;
                    return View(routeMaster);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public JsonResult GetLocationNames(string term, string Campus)
        {
            try
            {
                AdmissionManagementService admsnSrvc = new AdmissionManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("LocationName", term);
                if (!string.IsNullOrEmpty(Campus)) { criteria.Add("Campus", Campus); }
                //change the method to not to use the count since it is not being used here "REVISIT"
                Dictionary<long, IList<StudentLocationMaster>> LocationList = admsnSrvc.GetStudentLocationMasterListWithPagingAndCriteriaLikeSearch(0, 9999, string.Empty, string.Empty, criteria);
                var LocationNames = (from u in LocationList.First().Value
                                     where u.LocationName != null
                                     select u.LocationName).Distinct().ToList();
                return Json(LocationNames, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult DeleteRouteConfiguration(string[] Id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOn", "Account");
                else
                {
                    TransportService transSrvc = new TransportService();
                    int i;
                    string[] arrayId = Id[0].Split(',');
                    for (i = 0; i < arrayId.Length; i++)
                    {
                        var singleId = arrayId[i];
                        transSrvc.DeleteRouteConfiguration(Convert.ToInt64(singleId));
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "InsightMasterPolicy");
                throw ex;
            }
        }
        public ActionResult AddRouteConfigurationDetails(string RouteMasterId, string LocationName, string StopOrderNumber)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService transSrvc = new TransportService();
                    RouteConfiguration alrdyConfigured = new RouteConfiguration();
                    string rtnMsg = string.Empty;
                    string[] splitLocation = LocationName.Split(',');
                    string LocationOtherDetails = string.Empty;
                    LocationName = splitLocation[0];
                    for (int i = 1; i < splitLocation.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(splitLocation[i]))
                        {
                            string temp = splitLocation[i].Trim();
                            LocationOtherDetails = LocationOtherDetails + temp;
                            if ((i + 1) <= (splitLocation.Length - 1)) { LocationOtherDetails = LocationOtherDetails + ","; }
                        }
                    }
                    alrdyConfigured = transSrvc.GetRouteConfigurationByStopOrderNumber(Convert.ToInt64(RouteMasterId), Convert.ToInt64(StopOrderNumber));
                    if (alrdyConfigured != null)
                    {
                        rtnMsg = StopOrderNumber + " Stop Order Number is already allocated! Kindly choose another Stop Order Number!!";
                        return Json(rtnMsg, JsonRequestBehavior.AllowGet);
                    }
                    alrdyConfigured = transSrvc.GetRouteConfigurationByLocationName(Convert.ToInt64(RouteMasterId), LocationName, LocationOtherDetails);
                    if (alrdyConfigured != null)
                    {
                        rtnMsg = LocationName + " is already allocated! Kindly choose another Location!!";
                        return Json(rtnMsg, JsonRequestBehavior.AllowGet);
                    }
                    if (alrdyConfigured == null)
                    {
                        RouteConfiguration route = new RouteConfiguration();
                        if (!string.IsNullOrEmpty(RouteMasterId))
                            route.RouteMasterId = Convert.ToInt64(RouteMasterId);
                        if (!string.IsNullOrEmpty(StopOrderNumber))
                            route.StopOrderNumber = Convert.ToInt64(StopOrderNumber);
                        route.LocationName = LocationName;
                        route.LocationDetails = LocationOtherDetails;
                        route.CreatedDate = DateTime.Now;
                        route.CreatedBy = userId;
                        route.ModifiedDate = DateTime.Now;
                        route.ModifiedBy = userId;
                        transSrvc.CreateOrUpdateRouteConfiguration(route);
                        rtnMsg = LocationName + " is added successfully!";
                    }
                    return Json(rtnMsg, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult RouteConfiguraionJqGrid(RouteConfiguration routeConfig, string RouteMasterId, string LocationName, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportBC transSrvc = new TransportBC();
                    sord = sord == "desc" ? "Desc" : "Asc";
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    if (!string.IsNullOrEmpty(RouteMasterId)) { criteria.Add("RouteMasterId", Convert.ToInt64(RouteMasterId)); }
                    if (!string.IsNullOrEmpty(LocationName)) { criteria.Add("LocationName", LocationName); }
                    Dictionary<long, IList<RouteConfiguration>> RouteConfigList = transSrvc.GetRouteConfigurationListWithsearchCriteria(page - 1, rows, sidx, sord, criteria);
                    if (RouteConfigList != null && RouteConfigList.First().Key > 0)
                    {
                        long totalrecords = RouteConfigList.FirstOrDefault().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var AssLst = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,

                            rows = (
                                 from items in RouteConfigList.First().Value
                                 select new
                                 {
                                     cell = new string[] 
                                         {
                                            items.Id.ToString(),
                                            items.RouteMasterId.ToString(),
                                            items.LocationName,
                                            items.LocationDetails,
                                            items.StopOrderNumber.ToString(),
                                            items.CreatedBy,
                                            items.CreatedDate.Value!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy"):"",
                                            items.CreatedBy,
                                            items.ModifiedDate.Value!=null?items.ModifiedDate.Value.ToString("dd/MM/yyyy"):"",
                                            items.ModifiedBy
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
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }


        public ActionResult RouteStudentConfigurationPDF(string RouteStudCode)
        {
            //RouteStudCode = "R1SL1";
            TransportService transSrvc = new TransportService();
            RouteStudentPDF routeStudPdf = new RouteStudentPDF();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("RouteStudCode", RouteStudCode);
            string sidx = "StopOrderNumber";
            string sord = "Asc";

            //IList<RouteStudentConfigurationPDF_vw> RouteStudConfigpdflist_vw = new List<RouteStudentConfigurationPDF_vw>();

            Dictionary<long, IList<RouteStudentConfigurationPDF_vw>> RouteMstrList = transSrvc.GetRouteStudConfigPDFListWithsearchCriteria(0, 9999, string.Empty, string.Empty, criteria);
            //Dictionary<long, IList<RouteStudentConfigurationPDF_vw>> RouteMstrList = transSrvc.GetRouteStudConfigPDFListWithsearchCriteria(0, 9999, sidx, sord, criteria);

            if (RouteMstrList != null && RouteMstrList.First().Key > 0)
            {
                var LocationNameList = (from u in RouteMstrList.First().Value
                                        select u.LocationName).Distinct().ToArray();
                criteria.Clear();
                criteria.Add("LocationName", LocationNameList);
                criteria.Add("RouteMasterId", Convert.ToInt64(RouteMstrList.FirstOrDefault().Value[0].RouteId));

                Dictionary<long, IList<RouteConfiguration>> stopOrder = transSrvc.GetRouteConfigurationListWithsearchCriteria(0, 9999, sidx, sord, criteria);

                var LocationNameList1 = (from u in stopOrder.First().Value
                                         select u.LocationName).Distinct().ToList();

                IList<RouteConfiguration> routeConfigList = new List<RouteConfiguration>();


                for (int i = 0; i < LocationNameList1.Count; i++)
                {
                    var StudentList = (from u in RouteMstrList.First().Value
                                       where u.LocationName == LocationNameList1[i]
                                       select u).ToList();
                    RouteConfiguration routeConfig = new RouteConfiguration();
                    IList<RouteStudentConfigurationPDF_vw> routeStudConfigPdfList = new List<RouteStudentConfigurationPDF_vw>();
                    for (int j = 0; j < StudentList.Count; j++)
                    {
                        RouteStudentConfigurationPDF_vw obj = new RouteStudentConfigurationPDF_vw();
                        if (StudentList[j].PreRegNum > 0)
                        {
                            obj.LocationName = StudentList[j].LocationName;
                            obj.Name = StudentList[j].Name;
                            obj.Section = StudentList[j].Section;
                            obj.Grade = StudentList[j].Grade;
                            obj.TamilDescription = StudentList[j].TamilDescription;
                            obj.Campus = StudentList[j].Campus;
                            obj.RouteId = StudentList[j].RouteId;
                            obj.NoOfStudents = StudentList[j].NoOfStudents;
                            routeStudConfigPdfList.Add(obj);
                        }
                    }
                    routeConfig.routeStudConfigPdfList = routeStudConfigPdfList;
                    routeConfigList.Add(routeConfig);
                }

                routeStudPdf.RouteConfigList = routeConfigList;
            }

            routeStudPdf.Today = DateTime.Now.ToString("dd/MM/yyyy");
            TipsLogo(routeStudPdf, "TipsLogo.jpg");
            NaceLogo(routeStudPdf, "logonace.jpg");
            return new Rotativa.ViewAsPdf("RouteStudentConfigurationPDF", routeStudPdf)
            {
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageMargins = new Rotativa.Options.Margins(0, 0, 0, 0)
            };
        }

        //public ActionResult RouteStudentConfigurationPDF(string RouteStudCode)
        //{
        //    //RouteStudCode = "R1SL1";
        //    TransportService transSrvc = new TransportService();
        //    RouteStudentPDF routeStudPdf = new RouteStudentPDF();
        //    Dictionary<string, object> criteria = new Dictionary<string, object>();
        //    criteria.Add("RouteStudCode", RouteStudCode);
        //    string sidx = "StopOrderNumber";
        //    string sord = "Asc";

        //    //IList<RouteStudentConfigurationPDF_vw> RouteStudConfigpdflist_vw = new List<RouteStudentConfigurationPDF_vw>();

        //    Dictionary<long, IList<RouteStudentConfigurationPDF_vw>> RouteMstrList = transSrvc.GetRouteStudConfigPDFListWithsearchCriteria(0, 9999, string.Empty, string.Empty, criteria);
        //    //Dictionary<long, IList<RouteStudentConfigurationPDF_vw>> RouteMstrList = transSrvc.GetRouteStudConfigPDFListWithsearchCriteria(0, 9999, sidx, sord, criteria);

        //    if (RouteMstrList != null && RouteMstrList.First().Key > 0)
        //    {
        //        var LocationNameList = (from u in RouteMstrList.First().Value
        //                                select u.LocationName).Distinct().ToList();
        //        criteria.Clear();
        //        criteria.Add("LocationName", LocationNameList);
        //        criteria.Add("RouteMasterId", Convert.ToInt64(RouteMstrList.FirstOrDefault().Value[0].RouteId));

        //        Dictionary<long, IList<RouteConfiguration>> stopOrder = transSrvc.GetRouteConfigurationListWithsearchCriteria(0, 9999, sidx, sord, criteria);

        //        var LocationNameList1 = (from u in stopOrder.First().Value
        //                                 select u.LocationName).Distinct().ToList();

        //        IList<RouteConfiguration> routeConfigList = new List<RouteConfiguration>();


        //        for (int i = 0; i < LocationNameList1.Count; i++)
        //        {
        //            var StudentList = (from u in RouteMstrList.First().Value
        //                               where u.LocationName == LocationNameList1[i]
        //                               select u).ToList();
        //            RouteConfiguration routeConfig = new RouteConfiguration();
        //            IList<RouteStudentConfigurationPDF_vw> routeStudConfigPdfList = new List<RouteStudentConfigurationPDF_vw>();
        //            for (int j = 0; j < StudentList.Count; j++)
        //            {
        //                RouteStudentConfigurationPDF_vw obj = new RouteStudentConfigurationPDF_vw();
        //                if (StudentList[j].PreRegNum > 0)
        //                {
        //                    obj.LocationName = StudentList[j].LocationName;
        //                    obj.Name = StudentList[j].Name;
        //                    obj.Section = StudentList[j].Section;
        //                    obj.Grade = StudentList[j].Grade;
        //                    obj.TamilDescription = StudentList[j].TamilDescription;
        //                    obj.Campus = StudentList[j].Campus;
        //                    obj.RouteId = StudentList[j].RouteId;
        //                    obj.NoOfStudents = StudentList[j].NoOfStudents;
        //                    routeStudConfigPdfList.Add(obj);
        //                }
        //            }
        //            routeConfig.routeStudConfigPdfList = routeStudConfigPdfList;
        //            routeConfigList.Add(routeConfig);
        //        }

        //        routeStudPdf.RouteConfigList = routeConfigList;
        //    }

        //    routeStudPdf.Today = DateTime.Now.ToString("dd/MM/yyyy");
        //    TipsLogo(routeStudPdf, "TipsLogo.jpg");
        //    NaceLogo(routeStudPdf, "logonace.jpg");
        //    return new Rotativa.ViewAsPdf("RouteStudentConfigurationPDF", routeStudPdf)
        //    {
        //        PageOrientation = Rotativa.Options.Orientation.Portrait,
        //        PageMargins = new Rotativa.Options.Margins(0, 0, 0, 0)
        //    };
        //}

        //public ActionResult RouteStudentConfigurationPDF(string RouteStudCode)
        //{
        //    //RouteStudCode = "R1SL1";
        //    TransportService transSrvc = new TransportService();
        //    RouteStudentPDF routeStudPdf = new RouteStudentPDF();
        //    Dictionary<string, object> criteria = new Dictionary<string, object>();
        //    criteria.Add("RouteStudCode", RouteStudCode);


        //    //IList<RouteStudentConfigurationPDF_vw> RouteStudConfigpdflist_vw = new List<RouteStudentConfigurationPDF_vw>();

        //    Dictionary<long, IList<RouteStudentConfigurationPDF_vw>> RouteMstrList = transSrvc.GetRouteStudConfigPDFListWithsearchCriteria(0, 9999, string.Empty, string.Empty, criteria);
        //    if (RouteMstrList != null && RouteMstrList.First().Key > 0)
        //    {
        //        var LocationNameList = (from u in RouteMstrList.First().Value
        //                                select u.LocationName).Distinct().ToList();

        //        IList<RouteConfiguration> routeConfigList = new List<RouteConfiguration>();


        //        for (int i = 0; i < LocationNameList.Count; i++)
        //        {
        //            var StudentList = (from u in RouteMstrList.First().Value
        //                               where u.LocationName == LocationNameList[i]
        //                               select u).ToList();
        //            RouteConfiguration routeConfig = new RouteConfiguration();
        //            IList<RouteStudentConfigurationPDF_vw> routeStudConfigPdfList = new List<RouteStudentConfigurationPDF_vw>();
        //            for (int j = 0; j < StudentList.Count; j++)
        //            {
        //                RouteStudentConfigurationPDF_vw obj = new RouteStudentConfigurationPDF_vw();
        //                if (StudentList[j].PreRegNum > 0)
        //                {
        //                    obj.LocationName = StudentList[j].LocationName;
        //                    obj.Name = StudentList[j].Name;
        //                    obj.Section = StudentList[j].Section;
        //                    obj.Grade = StudentList[j].Grade;
        //                    obj.TamilDescription = StudentList[j].TamilDescription;
        //                    obj.Campus = StudentList[j].Campus;
        //                    obj.RouteId = StudentList[j].RouteId;
        //                    obj.NoOfStudents = StudentList[j].NoOfStudents;
        //                    routeStudConfigPdfList.Add(obj);
        //                }
        //            }
        //            routeConfig.routeStudConfigPdfList = routeStudConfigPdfList;
        //            routeConfigList.Add(routeConfig);
        //        }

        //        routeStudPdf.RouteConfigList = routeConfigList;
        //    }

        //    routeStudPdf.Today = DateTime.Now.ToString("dd/MM/yyyy");
        //    TipsLogo(routeStudPdf, "TipsLogo.jpg");
        //    NaceLogo(routeStudPdf, "logonace.jpg");
        //    return new Rotativa.ViewAsPdf("RouteStudentConfigurationPDF", routeStudPdf)
        //    {
        //        PageOrientation = Rotativa.Options.Orientation.Portrait,
        //        PageMargins = new Rotativa.Options.Margins(0, 0, 0, 0)
        //    };
        //}


        private void TipsLogo(RouteStudentPDF routeStudPdf, string imageName)
        {
            string url = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            routeStudPdf.TipsLogo = url + "Images/" + imageName;
        }
        private void NaceLogo(RouteStudentPDF routeStudPdf, string imageName)
        {
            string url = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            routeStudPdf.NaceLogo = url + "Images/" + imageName;
        }

        #region Route Configuration
        public ActionResult RouteMasterConfiguration(string RouteId, string Flag, string OldRouteStudCode, long longRouteId)
        {
            try
            {
                string userId = base.ValidateUser();
                TransportService transSrvc = new TransportService();
                RouteStudConfig RouteStudConfig = new RouteStudConfig();
                RouteMaster routeMaster = new RouteMaster();
                Student_Route_Configuration StudRoutConfig = new Student_Route_Configuration();
                if (Flag == "New")
                {
                    string RouteStudCode = string.Empty;
                    routeMaster = transSrvc.GetRouteMasterDetailsById(Convert.ToInt64(RouteId));
                    RouteStudCode = "R" + routeMaster.Id + "-" + "SL" + (routeMaster.NoOfStudList + 1);
                    StudRoutConfig.RouteMasterId = routeMaster.Id;
                    StudRoutConfig.RouteStudCode = RouteStudCode;
                    StudRoutConfig.Campus = routeMaster.Campus;
                    StudRoutConfig.RouteNo = routeMaster.RouteNo;
                    StudRoutConfig.Source = routeMaster.Source;
                    StudRoutConfig.Destination = routeMaster.Destination;
                    routeMaster.NoOfStudList = routeMaster.NoOfStudList + 1;

                    RouteStudConfig.RouteId = StudRoutConfig.RouteMasterId;
                    RouteStudConfig.RouteStudCode = StudRoutConfig.RouteStudCode;
                    RouteStudConfig.CreatedBy = userId;
                    RouteStudConfig.DateCreated = DateTime.Now;


                    transSrvc.SaveOrUpdateRouteStudConfigDetails(RouteStudConfig);

                    transSrvc.SaveOrUpdateRouteMasterDetails(routeMaster);
                    ViewBag.RouteId = Convert.ToInt64(RouteId);
                    return View(StudRoutConfig);
                }
                if (Flag == "Old")
                {
                    if (longRouteId > 0)
                    {
                        routeMaster = transSrvc.GetRouteMasterDetailsById(longRouteId);
                        StudRoutConfig.RouteMasterId = routeMaster.Id;
                        StudRoutConfig.RouteStudCode = OldRouteStudCode;
                        StudRoutConfig.Campus = routeMaster.Campus;
                        StudRoutConfig.RouteNo = routeMaster.RouteNo;
                        StudRoutConfig.Source = routeMaster.Source;
                        StudRoutConfig.Destination = routeMaster.Destination;
                        ViewBag.RouteId = longRouteId;
                        return View(StudRoutConfig);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }

        }
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult Routeddl(string Campus)
        {
            try
            {
                MastersService ms = new MastersService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();

                criteria.Add("Campus", Campus);
                Dictionary<long, IList<RouteMaster>> RouteMaster = ms.GetRouteMasterListWithPagingAndCriteria(null, null, null, null, criteria);
                // ViewBag.routedl1 = RouteMaster.First().Value;
                if (RouteMaster != null && RouteMaster.First().Value != null && RouteMaster.First().Value.Count > 0)
                {
                    var routeddl = (
                             from items in RouteMaster.First().Value

                             select new
                             {
                                 Text = items.RouteId,
                                 Value = items.Id
                             }).Distinct().ToList();

                    return Json(routeddl, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(null, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult locationddl(string Route)
        {
            try
            {
                TransportService ts = new TransportService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();

                criteria.Add("RouteMasterId", Convert.ToInt64(Route));
                Dictionary<long, IList<RouteConfiguration>> locationlist = ts.GetLocationListWithPagingAndCriteria(null, null, null, null, criteria);

                if (locationlist != null && locationlist.First().Value != null && locationlist.First().Value.Count > 0)
                {
                    var locationnddl = (
                             from items in locationlist.First().Value

                             select new
                             {
                                 Text = items.LocationName,
                                 Value = items.Id
                             }).Distinct().ToList();

                    return Json(locationnddl, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(null, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public JsonResult GetStudentList(string LocationId, string Campus)
        {
            try
            {
                AdmissionManagementService AMS = new AdmissionManagementService();
                TransportService TS = new TransportService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                RouteConfiguration RouteConfig = TS.GetLocationNameById(Convert.ToInt64(LocationId));

                IList<StudentTemplateView> StudentRouteMasterList = null;
                StudentRouteMasterList = TS.GetStudentNameListByQuery(RouteConfig.LocationName, Campus);
                criteria.Clear();
                criteria.Add("LocationId", RouteConfig.Id);
                Dictionary<long, IList<RouteStudentListConfigView>> RouteStudentList = TS.GetRouteStudentListConfigWithPagingAndCriteria(null, null, null, null, criteria);// null, null, null);
                if (StudentRouteMasterList != null)
                {
                    var Studentlistddl = (
                             from items in StudentRouteMasterList

                             select new
                             {
                                 Key = items.PreRegNum,
                                 Value = items.Name,
                                 Selected = "0"
                             }).Distinct().ToList();

                    var RouteStudentListSelected = (
                             from items in RouteStudentList.First().Value

                             select new
                             {
                                 Key = items.PreRegNum,
                                 Value = items.Name,
                                 Selected = "1"
                             }).Distinct().ToList();

                    return Json(new { Studentlistddl = Studentlistddl, RouteStudentListSelected = RouteStudentListSelected }, JsonRequestBehavior.AllowGet);
                    //return Json(Studentlistddl, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult UpdateRouteStudentTemplate(string StudId, string RouteId, string NotSelId, string RouteStudCode, string LocationId)
        {
            try
            {
                //string[] SelStudIds = StudId.Split(',');
                TransportBC Tbc = new TransportBC();
                string userId = base.ValidateUser();
                Tbc.UpdateRouteInStudentTemplate(StudId, RouteId, NotSelId, RouteStudCode, LocationId, userId);

                TransportService TS = new TransportService();
                RouteMasterConfig_vw RouteMasterConfig_vw = TS.GetRouteMasterConfig_vwByRouteStudCode(RouteStudCode);
                long StudCount = 0;
                StudCount = RouteMasterConfig_vw.NoOfStudents;
                return Json(StudCount, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Student Route Configuration
        public ActionResult RouteStudConfig()
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            var usrcmp = Session["UserCampus"] as IEnumerable<string>;

            if (usrcmp != null && usrcmp.Count() != 0)            // to check if the usrcmp obj is null or with dat)
            {
                criteria.Add("Name", usrcmp);
            }
            MastersService ms = new MastersService();
            Dictionary<long, IList<CampusMaster>> CampusMaster = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            ViewBag.campusddl = CampusMaster.First().Value;
            criteria.Clear();
            Dictionary<long, IList<RouteMaster>> RouteMaster = ms.GetRouteMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            ViewBag.routeddl = RouteMaster.First().Value;
            return View();
        }
        public ActionResult RouteStudJqGrid(RouteMasterConfig_vw RMCObj, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService transSrvc = new TransportService();
                    sord = sord == "desc" ? "Desc" : "Asc";
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    if (RMCObj.RouteId > 0) { criteria.Add("RouteId", RMCObj.RouteId); }
                    if (!string.IsNullOrEmpty(RMCObj.RouteStudCode)) { criteria.Add("RouteStudCode", RMCObj.RouteStudCode); }
                    if (!string.IsNullOrEmpty(RMCObj.RouteNo)) { criteria.Add("RouteNo", RMCObj.RouteNo); }
                    if (!string.IsNullOrEmpty(RMCObj.Campus)) { criteria.Add("Campus", RMCObj.Campus); }
                    if (!string.IsNullOrEmpty(RMCObj.Source)) { criteria.Add("Source", RMCObj.Source); }
                    if (!string.IsNullOrEmpty(RMCObj.Destination)) { criteria.Add("Destination", RMCObj.Destination); }
                    if (!string.IsNullOrEmpty(RMCObj.Via)) { criteria.Add("Via", RMCObj.Via); }
                    if (RMCObj.NoOfStudents > 0) { criteria.Add("NoOfStudents", RMCObj.NoOfStudents); }
                    Dictionary<long, IList<RouteMasterConfig_vw>> RouteStudConfigList = transSrvc.GetRouteMasterConfig_vwListWithsearchCriteria(page - 1, rows, sidx, sord, criteria);
                    if (RouteStudConfigList != null && RouteStudConfigList.First().Key > 0)
                    {
                        long totalrecords = RouteStudConfigList.FirstOrDefault().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var AssLst = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,

                            rows = (
                                 from items in RouteStudConfigList.First().Value
                                 select new
                                 {
                                     cell = new string[] 
                                         {
                                            items.Id.ToString(),
                                            items.RouteId.ToString(),
                                            
                                            //items.RouteStudCode,
                                            "<a style='color:#034af3;text-decoration:underline;cursor:pointer;' onclick=\"OpenOldRouteStudList('" + items.RouteId + "','" + items.RouteStudCode +  "');\" '>"+items.RouteStudCode+"</a>",
                                            items.RouteNo,
                                            items.Campus,
                                            items.Source,
                                            items.Destination,items.Via,
                                            items.NoOfStudents.ToString(),
                                            String.Format(@"<a style='color:#034af3;text-decoration:underline' href= '/Transport/RouteStudentConfigurationPDF?RouteStudCode="+items.RouteStudCode+"' target='_Blank'>{0}</a>","<i class='ace-icon fa fa-file-pdf-o red'></i>"),
                                            "<a style='color:#034af3;text-decoration:underline;cursor:pointer;' onclick=\"OpenStudentList('" + items.RouteId + "','" + items.RouteStudCode +  "');\" '>Click</a>",
                                            //String.Format(@"<a style='color:#034af3;text-decoration:underline' href= '/Transport/RouteStudentConfigurationForm?RouteStudCode=" + items.RouteStudCode +"&RouteId=" + items.RouteId+"target='StudentConfig'>0}</a>","<i class='ace-icon fa fa-file-pdf-o red'></i>"),
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
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        #endregion

        public JsonResult GetDestination(string term)
        {
            try
            {
                AdmissionManagementService admsnSrvc = new AdmissionManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                MastersService ms = new MastersService();
                //if (!string.IsNullOrEmpty(Campus)) { criteria.Add("Campus", Campus); }
                //check this count method "REVISIT" - referred from many places.
                Dictionary<long, IList<LocationMaster>> LocationMaster = ms.GetLocationMasterListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, null);
                var LocationNames = (from u in LocationMaster.First().Value
                                     where u.LocationName != null
                                     select u.LocationName).Distinct().ToList();
                return Json(LocationNames, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public JsonResult CheckStudentsCount(string RouteStudCode)
        {
            try
            {
                TransportBC transSrvc = new TransportBC();
                RouteMasterConfig_vw RouteMasterConfig_vw = new RouteMasterConfig_vw();
                bool rtnMsg = false;
                RouteMasterConfig_vw = transSrvc.GetRouteMasterConfig_vwByRouteStudCode(RouteStudCode);
                if (RouteMasterConfig_vw != null && RouteMasterConfig_vw.NoOfStudents != null && RouteMasterConfig_vw.NoOfStudents > 0)
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

        #region Route Configuration Report
        public ActionResult StudentRouteConfigReport()
        {
            try
            {
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
                return View();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }

        }
        public ActionResult StudentRouteConfigReportJqGrid(int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    AdmissionManagementService admsnSrvc = new AdmissionManagementService();
                    sord = sord == "desc" ? "Desc" : "Asc";
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("Transport", true);
                    Dictionary<long, IList<StudentTemplateView>> StudConfigList = admsnSrvc.GetStudentTemplateViewListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    if (StudConfigList != null && StudConfigList.First().Key > 0)
                    {
                        long totalrecords = StudConfigList.FirstOrDefault().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var AssLst = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,

                            rows = (
                                 from items in StudConfigList.First().Value
                                 select new
                                 {
                                     cell = new string[] 
                                         {
                                            items.Id.ToString(),
                                            items.PreRegNum.ToString(),
                                            items.Name,
                                            items.Grade,
                                            items.Section,
                                            items.LocationName
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
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult StudentsRouteConfigurationReport()
        {
            try
            {
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
                return View();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult StudentTransportRequiredCommonCountPieChart(string Campus, string Grade, string Section)
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            TransportService transSrvc = new TransportService();
            Dictionary<long, IList<StudentsRouteConfigReport_vw>> ReportDetails = null;
            if (!string.IsNullOrEmpty(Campus)) { criteria.Add("Campus", Campus); }
            if (!string.IsNullOrEmpty(Grade)) { criteria.Add("Grade", Grade); }
            if (!string.IsNullOrEmpty(Section)) { criteria.Add("Section", Section); }
            long TransportRequired = 0;
            long TransportNotRequired = 0;
            ReportDetails = transSrvc.GetStudentsRouteConfigReport_vwListWithsearchCriteria(null, 9999, string.Empty, string.Empty, criteria);
            var reqDetails = (from u in ReportDetails.FirstOrDefault().Value
                              select u).ToList();
            foreach (var ReportItem in reqDetails)
            {
                TransportRequired = TransportRequired + ReportItem.TransportRequired;
                TransportNotRequired = TransportNotRequired + ReportItem.TransportNotRequired;
            }
            var MasterChart = "<graph caption='Transport Required Chart' xAxisName='' decimalPrecision='0' forceDecimals='0' formatNumberScale='0' yAxisName='' animation='1' showNames='1' showValues='1' divlinecolor='d3d3d3' distance='5' angle='45' rotateNames='1'>";
            MasterChart = MasterChart + " <set name='Required' value='" + TransportRequired + "' color='008ee4' issliced='1'/>";
            MasterChart = MasterChart + " <set name='Not Required' value='" + TransportNotRequired + "' color='6baa01' issliced='1'/>";
            MasterChart = MasterChart + "</graph>";
            Response.Write(MasterChart);
            return null;
        }
        public ActionResult StudentTransportRequiredGenderCount(string Campus, string Grade, string Section)
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            TransportService transSrvc = new TransportService();
            Dictionary<long, IList<StudentsRouteConfigReport_vw>> ReportDetails = null;
            if (!string.IsNullOrEmpty(Campus)) { criteria.Add("Campus", Campus); }
            if (!string.IsNullOrEmpty(Grade)) { criteria.Add("Grade", Grade); }
            if (!string.IsNullOrEmpty(Section)) { criteria.Add("Section", Section); }
            long TransportRequiredMale = 0;
            long TransportRequiredFemale = 0;
            long TransportNotRequiredMale = 0;
            long TransportNotRequiredFeMale = 0;
            string[] ColorCodes = { "8B008B", "0e6aad", "8A2BE2", "FF4500", "f8bd19", "e44a00", "008ee4", "6baa01", "f8bd19", "BDB76B" };
            ReportDetails = transSrvc.GetStudentsRouteConfigReport_vwListWithsearchCriteria(null, 9999, string.Empty, string.Empty, criteria);
            var reqDetails = (from u in ReportDetails.FirstOrDefault().Value
                              select u).ToList();
            foreach (var ReportItem in reqDetails)
            {
                TransportRequiredMale = TransportRequiredMale + ReportItem.TransportRequiredMale;
                TransportRequiredFemale = TransportRequiredFemale + ReportItem.TransportRequiredFemale;
                TransportNotRequiredMale = TransportNotRequiredMale + ReportItem.TransportNotRequiredMale;
                TransportNotRequiredFeMale = TransportNotRequiredFeMale + ReportItem.TransportRequiredFemale;
            }
            var GenderChart = "<graph caption='Gender wise chart' xAxisName='' decimalPrecision='0' forceDecimals='0' formatNumberScale='0' yAxisName='' animation='1' showNames='1' showValues='0' divlinecolor='d3d3d3' distance='5' angle='45' rotateNames='1'>";
            GenderChart = GenderChart + "<categories>";
            GenderChart = GenderChart + "<category name='Required' />";
            GenderChart = GenderChart + "<category name='Not Required' />";
            GenderChart = GenderChart + "</categories>";

            GenderChart = GenderChart + " <dataset seriesname='Male' color='8B008B'>";
            GenderChart = GenderChart + "<set value='" + TransportRequiredMale + "' />";
            GenderChart = GenderChart + "<set value='" + TransportNotRequiredMale + "' />";
            GenderChart = GenderChart + "</dataset>";

            GenderChart = GenderChart + " <dataset seriesname='Female' color='0e6aad'>";
            GenderChart = GenderChart + "<set value='" + TransportRequiredFemale + "' />";
            GenderChart = GenderChart + "<set value='" + TransportNotRequiredFeMale + "' />";
            GenderChart = GenderChart + "</dataset>";
            GenderChart = GenderChart + "</graph>";

            Response.Write(GenderChart);
            return null;
        }
        public ActionResult StudentTransportAllocationCommonCountChart(string Campus, string Grade, string Section)
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            TransportService transSrvc = new TransportService();
            Dictionary<long, IList<StudentsRouteConfigReport_vw>> ReportDetails = null;
            if (!string.IsNullOrEmpty(Campus)) { criteria.Add("Campus", Campus); }
            if (!string.IsNullOrEmpty(Grade)) { criteria.Add("Grade", Grade); }
            if (!string.IsNullOrEmpty(Section)) { criteria.Add("Section", Section); }
            long RouteAllocated = 0;
            long RouteNotAllocated = 0;
            ReportDetails = transSrvc.GetStudentsRouteConfigReport_vwListWithsearchCriteria(null, 9999, string.Empty, string.Empty, criteria);
            var reqDetails = (from u in ReportDetails.FirstOrDefault().Value
                              select u).ToList();
            foreach (var ReportItem in reqDetails)
            {
                RouteAllocated = RouteAllocated + ReportItem.RouteAllocated;
                RouteNotAllocated = RouteNotAllocated + ReportItem.RouteNotAllocated;
            }
            var AllotmentChart = "<graph caption='Allotment Chart' xAxisName='' decimalPrecision='0' forceDecimals='0' formatNumberScale='0' yAxisName='' animation='1' showNames='1' showValues='1' divlinecolor='d3d3d3' distance='5' angle='45' rotateNames='1'>";
            AllotmentChart = AllotmentChart + " <set name='Alloted' value='" + RouteAllocated + "' color='008ee4' />";
            AllotmentChart = AllotmentChart + " <set name='Not Alloted' value='" + RouteNotAllocated + "' color='6baa01' />";
            AllotmentChart = AllotmentChart + "</graph>";
            Response.Write(AllotmentChart);
            return null;
        }
        public ActionResult StudentTransportAllocationGenderCountChart(string Campus, string Grade, string Section)
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            TransportService transSrvc = new TransportService();
            Dictionary<long, IList<StudentsRouteConfigReport_vw>> ReportDetails = null;
            if (!string.IsNullOrEmpty(Campus)) { criteria.Add("Campus", Campus); }
            if (!string.IsNullOrEmpty(Grade)) { criteria.Add("Grade", Grade); }
            if (!string.IsNullOrEmpty(Section)) { criteria.Add("Section", Section); }
            long RouteAllocatedMale = 0;
            long RouteAllocatedFeMale = 0;
            long RouteNotAllocatedMale = 0;
            long RouteNotAllocatedFeMale = 0;

            long TotalAlloted = 0;
            long TotalNotAlloted = 0;

            string[] ColorCodes = { "8B008B", "0e6aad", "8A2BE2", "FF4500", "f8bd19", "e44a00", "008ee4", "6baa01", "f8bd19", "BDB76B" };
            ReportDetails = transSrvc.GetStudentsRouteConfigReport_vwListWithsearchCriteria(null, 9999, string.Empty, string.Empty, criteria);
            var reqDetails = (from u in ReportDetails.FirstOrDefault().Value
                              select u).ToList();
            foreach (var ReportItem in reqDetails)
            {
                RouteAllocatedMale = RouteAllocatedMale + ReportItem.RouteAllocatedMale;
                RouteAllocatedFeMale = RouteAllocatedFeMale + ReportItem.RouteAllocatedFeMale;
                RouteNotAllocatedMale = RouteNotAllocatedMale + ReportItem.RouteNotAllocatedMale;
                RouteNotAllocatedFeMale = RouteNotAllocatedFeMale + ReportItem.RouteNotAllocatedFeMale;
            }
            TotalAlloted = RouteAllocatedMale + RouteAllocatedFeMale;
            TotalNotAlloted = RouteNotAllocatedMale + RouteNotAllocatedFeMale;

            var GenderChart = "<graph caption='Gender wise chart' xAxisName='' decimalPrecision='0' forceDecimals='0' formatNumberScale='0' yAxisName='' animation='1' showNames='1' showValues='0' divlinecolor='d3d3d3' distance='5' angle='45' rotateNames='1'>";
            GenderChart = GenderChart + "<categories>";
            GenderChart = GenderChart + "<category name='Alloted' />";
            GenderChart = GenderChart + "<category name='Not Alloted' />";
            GenderChart = GenderChart + "</categories>";

            GenderChart = GenderChart + " <dataset seriesname='Male' color='F6BD0F'>";
            GenderChart = GenderChart + "<set value='" + RouteAllocatedMale + "' />";
            GenderChart = GenderChart + "<set value='" + RouteNotAllocatedMale + "' />";
            GenderChart = GenderChart + "</dataset>";

            GenderChart = GenderChart + " <dataset seriesname='Female' color='8BBA00'>";
            GenderChart = GenderChart + "<set value='" + RouteAllocatedFeMale + "' />";
            GenderChart = GenderChart + "<set value='" + RouteNotAllocatedFeMale + "' />";
            GenderChart = GenderChart + "</dataset>";

            GenderChart = GenderChart + " <dataset seriesname='Total' color='08E8E' parentyaxis='S' renderas='Line'>";
            GenderChart = GenderChart + "<set value='" + TotalAlloted + "' />";
            GenderChart = GenderChart + "<set value='" + TotalNotAlloted + "' />";
            GenderChart = GenderChart + "</dataset>";


            GenderChart = GenderChart + "</graph>";

            Response.Write(GenderChart);
            return null;
        }
        //public ActionResult StudentRouteAllocationCountPieChart(string Campus, string Grade, string Section)
        //{
        //    Dictionary<string, object> criteria = new Dictionary<string, object>();
        //    TransportService transSrvc = new TransportService();
        //    string[] ColorCodes = { "8B008B", "0e6aad", "8A2BE2", "FF4500", "f8bd19", "e44a00", "008ee4", "6baa01", "f8bd19", "BDB76B" };
        //    Dictionary<long, IList<StudentsRouteConfigReport_vw>> ReportDetails = null;
        //    if (!string.IsNullOrEmpty(Campus)) { criteria.Add("Campus", Campus); }
        //    if (!string.IsNullOrEmpty(Grade)) { criteria.Add("Grade", Grade); }
        //    if (!string.IsNullOrEmpty(Section)) { criteria.Add("Section", Section); }
        //    ReportDetails = transSrvc.GetStudentsRouteConfigReport_vwListWithsearchCriteria(null, 9999, string.Empty, string.Empty, criteria);

        //    var MasterChart = "<graph caption='Route Allocation Chart' xAxisName='' decimalPrecision='0' forceDecimals='0' formatNumberScale='0' yAxisName='' animation='1' showNames='1' showValues='1' divlinecolor='d3d3d3' distance='5' angle='45' rotateNames='1'>";
        //    MasterChart = MasterChart + " <set name='Allocated' value='" + Convert.ToInt64(ReportDetails.FirstOrDefault().Value[0].RouteAllocated) + "' color='008ee4' issliced='1'/>";
        //    MasterChart = MasterChart + " <set name='Not Allocated' value='" + Convert.ToInt64(ReportDetails.FirstOrDefault().Value[0].RouteNotAllocated) + "' color='6baa01' issliced='1'/>";
        //    MasterChart = MasterChart + "</graph>";
        //    Response.Write(MasterChart);
        //    return null;
        //}

        public ActionResult RouteStudentReportCounts(string Campus, string Grade, string Section)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                TransportService transSrvc = new TransportService();
                if (!string.IsNullOrEmpty(Campus)) { criteria.Add("Campus", Campus); }
                if (!string.IsNullOrEmpty(Grade)) { criteria.Add("Grade", Grade); }
                if (!string.IsNullOrEmpty(Section)) { criteria.Add("Section", Section); }
                Dictionary<long, IList<StudentsRouteConfigReport_vw>> ReportDetails = null;
                ReportDetails = transSrvc.GetStudentsRouteConfigReport_vwListWithsearchCriteria(null, 9999, string.Empty, string.Empty, criteria);
                if (ReportDetails != null && ReportDetails.Count > 0 && ReportDetails.First().Key > 0)
                {
                    var ReportCountDetails = (from u in ReportDetails.First().Value
                                              select u).ToList();
                    return Json(ReportCountDetails, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult GetStudentLocationMasterByLocation(string Location)
        {
            AdmissionManagementService admsnSrvc = new AdmissionManagementService();
            bool retMsg = false;
            StudentLocationMaster StudLocation = new StudentLocationMaster();
            StudLocation = admsnSrvc.GetStudentLocationMasterByLocationName(Location);
            if (StudLocation == null)
                retMsg = true;
            return Json(retMsg, JsonRequestBehavior.AllowGet);
        }

        #endregion
        //17-06 by benadict

        public ActionResult AddStudentLocationDetails()
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

        public ActionResult AddStudentLocationDetailsList(StudentLocationMaster slm)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    slm.CreatedBy = userId;
                    slm.CreatedDate = DateTime.Now;
                    slm.ModifiedBy = userId;
                    slm.ModifiedDate = DateTime.Now;
                    TransportService ts = new TransportService();
                    ts.CreateOrUpdateStudentLocationNameMaster(slm);
                    return Json(null, JsonRequestBehavior.AllowGet);
                    //return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult AddStudentLocationDetailsListJqGrid(StudentLocationMaster slm, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    if (!string.IsNullOrWhiteSpace(slm.Campus)) criteria.Add("Campus", slm.Campus);
                    if (!string.IsNullOrWhiteSpace(slm.LocationName)) criteria.Add("LocationName", slm.LocationName);
                    Dictionary<long, IList<StudentLocationMaster>> StudentLocationMaster = ts.GetStudentLocationMasterListWithPagingAndCriteria(page - 1, rows, sord, sidx, criteria);
                    if (StudentLocationMaster != null && StudentLocationMaster.FirstOrDefault().Value != null && StudentLocationMaster.FirstOrDefault().Key > 0 && StudentLocationMaster.FirstOrDefault().Value.Count > 0)
                    {
                        long totalrecords = StudentLocationMaster.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var LocList = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in StudentLocationMaster.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                                            items.Id.ToString(),
                                            items.Campus,
                                            items.LocationName,
                                            items.TamilDescription,                              

                            }
                                    })
                        };
                        return Json(LocList, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var Empty = new { rows = (new { cell = new string[] { } }) };
                        return Json(Empty, JsonRequestBehavior.AllowGet);
                    }
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public JsonResult CheckCampusLocationMaster(string Campus, string LocationName)
        {
            try
            {
                TransportService transSrvc = new TransportService();
                StudentLocationMaster locationMaster = new StudentLocationMaster();
                AdmissionManagementService ams = new AdmissionManagementService();
                bool rtnMsg = false;
                locationMaster = transSrvc.GetStudentLocationMasterByLocationName(Campus, LocationName);
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
        #region ConfiguredStudentList Form
        public ActionResult RouteStudentConfigurationForm(string RouteStudCode, long RouteId)
        {
            try
            {
                TransportService TS = new TransportService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                //criteria.Add("RouteStudCode", RouteStudCode);
                //criteria.Add("Id", LocationId);
                if (RouteId > 0) { criteria.Add("RouteMasterId", RouteId); }
                RouteStudentListConfigView tempObj = new RouteStudentListConfigView();
                Dictionary<long, IList<RouteConfiguration>> RouteConfigList = TS.GetRouteConfigurationListWithsearchCriteria(null, 9999, null, null, criteria);
                if (RouteConfigList != null && RouteConfigList.Count > 0)
                {
                    var LocationList = (from u in RouteConfigList.FirstOrDefault().Value
                                        select u).Distinct().ToList();

                    IList<RouteStudentListConfigView> RouteStudConfigList = new List<RouteStudentListConfigView>();
                    foreach (var LocationItem in LocationList)
                    {
                        RouteStudentListConfigView routeObj = new RouteStudentListConfigView();
                        criteria.Clear();
                        criteria.Add("LocationId", LocationItem.Id);
                        criteria.Add("RouteStudCode", RouteStudCode);
                        Dictionary<long, IList<RouteStudentListConfigView>> RouteStudentList = TS.GetRouteStudentListConfigWithPagingAndCriteria(null, null, null, null, criteria);
                        var StudentsList = (from u in RouteStudentList.FirstOrDefault().Value
                                            select u).Distinct().ToList();
                        IList<StudentTemplateView> StudIlist = new List<StudentTemplateView>();
                        foreach (var StudItem in StudentsList)
                        {
                            StudentTemplateView obj = new StudentTemplateView();
                            obj = TS.GetStudentDetailsByPreRegNo(StudItem.PreRegNum);
                            //obj.Name = StudItem.Name;
                            //obj.PreRegNum = StudItem.PreRegNum;
                            StudIlist.Add(obj);
                        }
                        routeObj.LocationName = LocationItem.LocationName;
                        routeObj.StudTempList = StudIlist;
                        RouteStudConfigList.Add(routeObj);
                    }
                    tempObj.StudentDetailsList = RouteStudConfigList;
                }
                return View(tempObj);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult GetConfigStudentsCount(string RouteStudCode)
        {
            try
            {
                TransportService TS = new TransportService();
                RouteMasterConfig_vw RouteMasterConfig_vw = TS.GetRouteMasterConfig_vwByRouteStudCode(RouteStudCode);
                long StudCount = 0;
                StudCount = RouteMasterConfig_vw.NoOfStudents;
                return Json(StudCount, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Added By Anto


        public ActionResult ElectricalMaintenanceReport()
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            Dictionary<long, IList<CampusMaster>> Campus = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            ViewBag.campusddl = Campus.First().Value;
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
        public ActionResult VehicleElectricalMaintenanceReportJqGrid(string ExportType, VehicleElectricalMaintenance_Vw vm,
        string Campus, string VehicleNo, string EDateOfService, string EServiceProvider, string EServiceCost, string EBillNo, string ESparePartsUsed,
        string EDescription, string CreatedDate, string CreatedBy, string EM_SparePartsUsedfile,
           int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    sord = sord == "desc" ? "Desc" : "Asc";
                    if (vm.VehicleId > 0)
                        criteria.Add("VehicleId", vm.VehicleId);
                    Dictionary<string, object> Criteria = new Dictionary<string, object>();
                    if (!string.IsNullOrEmpty(Campus)) { Criteria.Add("Campus", Campus); }

                    Dictionary<string, object> LikeCriteria = new Dictionary<string, object>();
                    if (!string.IsNullOrEmpty(EServiceCost))
                    {
                        decimal ServiceCost = Convert.ToDecimal(EServiceCost);
                        Criteria.Add("EServiceCost", ServiceCost);
                    }
                    if (!string.IsNullOrEmpty(Campus)) { LikeCriteria.Add("Campus", Campus); }
                    if (!string.IsNullOrEmpty(VehicleNo)) { LikeCriteria.Add("VehicleNo", VehicleNo); }
                    if (!string.IsNullOrEmpty(EServiceProvider)) { LikeCriteria.Add("EServiceProvider", EServiceProvider); }
                    if (!string.IsNullOrEmpty(EBillNo)) { LikeCriteria.Add("EBillNo", EBillNo); }
                    if (!string.IsNullOrEmpty(ESparePartsUsed)) { LikeCriteria.Add("ESparePartsUsed", ESparePartsUsed); }
                    if (!string.IsNullOrEmpty(CreatedBy)) { LikeCriteria.Add("CreatedBy", CreatedBy); }
                    if (!string.IsNullOrEmpty(EDescription)) { LikeCriteria.Add("EDescription", EDescription); }
                    if (!string.IsNullOrEmpty(EM_SparePartsUsedfile)) { LikeCriteria.Add("EM_SparePartsUsedfile", EM_SparePartsUsedfile); }
                    Dictionary<long, IList<VehicleElectricalMaintenance_Vw>> VehicleElectricalMaintenance = ts.GetVehicleElectricalMaintenance_VwWithPagingLikeSearch(page - 1, rows, sord, sidx, Criteria);
                    if (VehicleElectricalMaintenance != null && VehicleElectricalMaintenance.Count > 0)
                    {
                        if (ExportType == "Excel")
                        {
                            var List = VehicleElectricalMaintenance.First().Value.ToList();
                            ExptToXL(List, "VehicleElectricalMaintenanceReportList", (items => new
                            {
                                items.VehicleId,
                                items.Campus,
                                items.VehicleNo,
                                EDateOfService = items.EDateOfService != null ? items.EDateOfService.Value.ToString("dd/MM/yyyy") : "",
                                items.EServiceProvider,
                                ServiceCost = items.EServiceCost.ToString(),
                                items.EBillNo,
                                items.ESparePartsUsed,
                                items.EDescription,
                                CreatedDate = items.CreatedDate != null ? items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : "",
                                items.CreatedBy
                            }));
                            return new EmptyResult();
                        }
                        else
                        {
                            long totalrecords = VehicleElectricalMaintenance.First().Key;
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var ElectricalMaintenanceList = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalrecords,
                                rows = (from items in VehicleElectricalMaintenance.First().Value
                                        select new
                                        {
                                            i = 2,
                                            cell = new string[] {
                               items.Id.ToString(), items.VehicleId.ToString(),items.Campus, items.VehicleNo,
                               items.EDateOfService!=null?items.EDateOfService.Value.ToString("dd/MM/yyyy"):"",
                               items.EServiceProvider, 
                               items.EServiceCost.ToString(), items.EBillNo, items.ESparePartsUsed, items.EDescription,
                               items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt"):"",
                               items.CreatedBy
                            }
                                        })
                            };
                            return Json(ElectricalMaintenanceList, JsonRequestBehavior.AllowGet);
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

        //MechanicalMaintenanceReport

        public ActionResult MechanicalMaintenanceReport()
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            Dictionary<long, IList<CampusMaster>> Campus = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            ViewBag.campusddl = Campus.First().Value;
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

        public ActionResult MechanicalMaintenanceReportJqGrid(string ExportType, VehicleMaintance_Vw vm, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    sord = sord == "desc" ? "Desc" : "Asc";
                    if (vm.VehicleId > 0)
                        criteria.Add("VehicleId", vm.VehicleId);
                    if (!string.IsNullOrEmpty(vm.Campus)) { criteria.Add("Campus", vm.Campus); }
                    if (!string.IsNullOrEmpty(vm.VehicleNo)) { criteria.Add("VehicleNo", vm.VehicleNo); }
                    if (!string.IsNullOrEmpty(vm.VehicleMaintenanceType)) { criteria.Add("VehicleMaintenanceType", vm.VehicleMaintenanceType); }
                    if (!string.IsNullOrEmpty(vm.VehicleBreakdownLocation)) { criteria.Add("VehicleBreakdownLocation", vm.VehicleBreakdownLocation); }

                    if (!string.IsNullOrEmpty(vm.VehicleServiceProvider)) { criteria.Add("VehicleServiceProvider", vm.VehicleServiceProvider); }
                    if (vm.VehicleSeviceCost > 0) { criteria.Add("VehicleSeviceCost", Convert.ToDecimal(vm.VehicleSeviceCost)); }
                    if (!string.IsNullOrEmpty(vm.VehicleServiceBillNo)) { criteria.Add("VehicleServiceBillNo", vm.VehicleServiceBillNo); }
                    if (!string.IsNullOrEmpty(vm.CreatedBy)) { criteria.Add("CreatedBy", vm.CreatedBy); }
                    if (!string.IsNullOrEmpty(vm.VehicleSparePartsUsed)) { criteria.Add("VehicleSparePartsUsed", vm.VehicleSparePartsUsed); }
                    Dictionary<long, IList<VehicleMaintance_Vw>> VehicleMaintenance = ts.GetVehicleMaintenance_VwListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    if (VehicleMaintenance != null && VehicleMaintenance.Count > 0 && VehicleMaintenance.FirstOrDefault().Value != null)
                    {
                        if (ExportType == "Excel")
                        {
                            var List = VehicleMaintenance.First().Value.ToList();
                            ExptToXL(List, "MaintenanceReportList", (items => new
                            {
                                items.VehicleId,
                                items.Campus,
                                items.VehicleNo,
                                items.VehicleMaintenanceType,
                                VehicleDateOfBreakdown = items.VehicleDateOfBreakdown,
                                items.VehicleBreakdownLocation,
                                VehiclePlannedDateOfService = items.VehiclePlannedDateOfService != null ? items.VehiclePlannedDateOfService.Value.ToString("dd/MM/yyyy") : "",
                                VehicleActualDateOfService = items.VehicleActualDateOfService != null ? items.VehicleActualDateOfService.Value.ToString("dd/MM/yyyy") : "",
                                items.VehicleServiceProvider,
                                VehicleSeviceCost = items.VehicleSeviceCost.ToString(),
                                items.VehicleServiceBillNo,
                                items.VehicleSparePartsUsed,
                                items.CreatedDate,
                                items.CreatedBy
                            }));
                            return new EmptyResult();
                        }
                        else
                        {
                            long totalrecords = VehicleMaintenance.First().Key;
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var VehicleMaintenanceList = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalrecords,
                                rows = (from items in VehicleMaintenance.First().Value
                                        select new
                                        {
                                            i = 2,
                                            cell = new string[] {
                               items.Id.ToString(), items.VehicleId.ToString(),items.Campus, items.VehicleNo,items.VehicleMaintenanceType,
                               items.VehicleDateOfBreakdown!=null?items.VehicleDateOfBreakdown.Value.ToString("dd/MM/yyyy"):"",
                               items.VehicleBreakdownLocation, 
                               items.VehiclePlannedDateOfService!=null?items.VehiclePlannedDateOfService.Value.ToString("dd/MM/yyyy"):"",
                               items.VehicleActualDateOfService!=null?items.VehicleActualDateOfService.Value.ToString("dd/MM/yyyy"):"",
                               items.VehicleServiceProvider,
                               items.VehicleSeviceCost.ToString(),items.VehicleServiceBillNo,items.VehicleSparePartsUsed,
                               items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy"):"",
                               items.CreatedBy
                            }
                                        })
                            };
                            return Json(VehicleMaintenanceList, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        //VehicleTyreMaintenanceReport


        public ActionResult VehicleTyreMaintenanceReport()
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<long, IList<CampusMaster>> Campus = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                ViewBag.campusddl = Campus.First().Value;
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

        public ActionResult VehicleTyreMaintenanceReportJqGrid(string Campus, string ExportType, VehicleTyreMaintenance_Vw vm, string TyreModel, string TyreSize, string TyreDateOfEntry, string TyreMilometerReading, string TyreCost, string TyreDateOfAlignment,
                     string TyreDateOfRotation, string TyreDateOfWheelService, string TyreServiceCost, string CreatedDate, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();

                    sord = sord == "desc" ? "Desc" : "Asc";
                    if (vm.VehicleId > 0)
                        criteria.Add("VehicleId", vm.VehicleId);

                    Dictionary<string, object> ExactCriteria = new Dictionary<string, object>();
                    Dictionary<string, object> LikeCriteria = new Dictionary<string, object>();
                    if (!string.IsNullOrEmpty(vm.Campus)) { ExactCriteria.Add("Campus", vm.Campus); }
                    if (!string.IsNullOrEmpty(TyreCost))
                    {
                        decimal ServiceCost = Convert.ToDecimal(TyreCost);
                        ExactCriteria.Add("TyreCost", ServiceCost);
                    }

                    if (!string.IsNullOrEmpty(TyreServiceCost))
                    {
                        decimal ServiceCost = Convert.ToDecimal(TyreServiceCost);
                        ExactCriteria.Add("TyreServiceCost", ServiceCost);
                    }

                    if (!string.IsNullOrEmpty(TyreMilometerReading))
                    {
                        decimal Radeing = Convert.ToDecimal(TyreMilometerReading);
                        ExactCriteria.Add("TyreMilometerReading", Radeing);
                    }

                    if (!string.IsNullOrEmpty(vm.VehicleNo)) { LikeCriteria.Add("VehicleNo", vm.VehicleNo); }
                    if (!string.IsNullOrEmpty(vm.TyreMaintenanceType)) { LikeCriteria.Add("TyreMaintenanceType", vm.TyreMaintenanceType); }
                    if (!string.IsNullOrEmpty(vm.TyreLocation)) { LikeCriteria.Add("TyreLocation", vm.TyreLocation); }
                    if (!string.IsNullOrEmpty(vm.TypeOfTyre)) { LikeCriteria.Add("TypeOfTyre", vm.TypeOfTyre); }
                    if (!string.IsNullOrEmpty(vm.TyreMake)) { LikeCriteria.Add("TyreMake", vm.TyreMake); }
                    if (!string.IsNullOrEmpty(TyreModel)) { LikeCriteria.Add("TyreModel", TyreModel); }
                    if (!string.IsNullOrEmpty(TyreSize)) { LikeCriteria.Add("TyreSize", TyreSize); }
                    if (!string.IsNullOrEmpty(vm.TyreBillNo)) { LikeCriteria.Add("TyreBillNo", vm.TyreBillNo); }

                    if (!string.IsNullOrEmpty(vm.CreatedBy)) { LikeCriteria.Add("CreatedBy", vm.CreatedBy); }

                    Dictionary<long, IList<VehicleTyreMaintenance_Vw>> VehicleTyreMaintenance = ts.GetVehicleTyreMaintenance_VwWithPagingLikeSearch(page - 1, rows, sord, sidx, ExactCriteria, LikeCriteria);
                    if (VehicleTyreMaintenance != null && VehicleTyreMaintenance.Count > 0)
                    {
                        if (ExportType == "Excel")
                        {
                            var List = VehicleTyreMaintenance.First().Value.ToList();
                            ExptToXL(List, "VehicleTyreMaintenanceReportList", (items => new
                            {
                                items.VehicleId,
                                items.VehicleNo,
                                items.Campus,
                                items.TyreMaintenanceType,
                                items.TyreLocation,
                                items.TypeOfTyre,
                                items.TyreMake,
                                items.TyreModel,
                                items.TyreSize,
                                TyreAssignedDate = items.TyreAssignedDate != null ? items.TyreAssignedDate.Value.ToString("dd/MM/yyyy") : "",
                                items.TyreMilometerReading,
                                TyreCost = items.TyreCost.ToString(),
                                items.TyreBillNo,
                                TyreDateOfAlignment = items.TyreDateOfAlignment != null ? items.TyreDateOfAlignment.Value.ToString("dd/MM/yyyy") : "",
                                TyreDateOfRotation = items.TyreDateOfRotation != null ? items.TyreDateOfRotation.Value.ToString("dd/MM/yyyy") : "",
                                items.TyreDateOfWheelService,
                                TyreServiceProvider = items.TyreServiceProvider,
                                TyreServicedBy = items.TyreServicedBy,
                                TyreServiceBillNo = items.TyreServiceBillNo,
                                CreatedDate = items.CreatedDate != null ? items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : "",
                                items.CreatedBy
                            }));
                            return new EmptyResult();
                        }
                        else
                        {
                            long totalrecords = VehicleTyreMaintenance.First().Key;
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var TyreMaintenanceList = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalrecords,
                                rows = (from items in VehicleTyreMaintenance.First().Value
                                        select new
                                        {
                                            i = 2,
                                            cell = new string[] {
                               items.Id.ToString(), items.VehicleId.ToString(),items.Campus, items.VehicleNo,items.TyreMaintenanceType, items.TyreLocation, 
                               items.TypeOfTyre, items.TyreMake, items.TyreModel, items.TyreSize,
                               items.TyreAssignedDate!=null?items.TyreAssignedDate.Value.ToString("dd/MM/yyyy"):"",
                               items.TyreMilometerReading.ToString(),
                               items.TyreCost.ToString(), items.TyreBillNo, 
                               items.TyreDateOfAlignment!=null?items.TyreDateOfAlignment.Value.ToString("dd/MM/yyyy"):"",
                               items.TyreDateOfRotation!=null?items.TyreDateOfRotation.Value.ToString("dd/MM/yyyy"):"",
                               items.TyreDateOfWheelService.ToString(),
                               items.TyreServiceProvider,
                               items.TyreServicedBy,
                               items.TyreServiceBillNo,
                               items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy"):"",
                               items.CreatedBy
                            }
                                        })
                            };
                            return Json(TyreMaintenanceList, JsonRequestBehavior.AllowGet);
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

        //VehicleACMaintenanceReport
        public ActionResult VehicleACMaintenanceReport()
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<long, IList<CampusMaster>> Campus = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                ViewBag.campusddl = Campus.First().Value;
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
        public ActionResult VehicleACMaintenanceReportJqGrid(VehicleAcMaintanceReport_Vw vm, string Campus, string ExportType, string ACDateOfBreakdown, string ACPlannedDateOfService, string ACActualDateOfService,
                string ACServiceCost, string CreatedDate, string AM_SparePartsUsedfile, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    sord = sord == "desc" ? "Desc" : "Asc";
                    if (vm.VehicleId > 0) criteria.Add("VehicleId", vm.VehicleId);

                    Dictionary<string, object> ExactCriteria = new Dictionary<string, object>();
                    Dictionary<string, object> LikeCriteria = new Dictionary<string, object>();

                    if (!string.IsNullOrEmpty(ACServiceCost))
                    {
                        decimal ServiceCost = Convert.ToDecimal(ACServiceCost);
                        ExactCriteria.Add("ACServiceCost", ServiceCost);
                    }
                    if (!string.IsNullOrEmpty(vm.Campus)) { LikeCriteria.Add("Campus", vm.Campus); }
                    if (!string.IsNullOrEmpty(vm.VehicleNo)) { LikeCriteria.Add("VehicleNo", vm.VehicleNo); }
                    if (!string.IsNullOrEmpty(vm.ACMaintenanceType)) { LikeCriteria.Add("ACMaintenanceType", vm.ACMaintenanceType); }
                    if (!string.IsNullOrEmpty(vm.ACBreakdownLocation)) { LikeCriteria.Add("ACBreakdownLocation", vm.ACBreakdownLocation); }
                    if (!string.IsNullOrEmpty(vm.ACModel)) { LikeCriteria.Add("ACModel", vm.ACModel); }
                    if (!string.IsNullOrEmpty(vm.ACServiceProvider)) { LikeCriteria.Add("ACServiceProvider", vm.ACServiceProvider); }
                    if (!string.IsNullOrEmpty(vm.ACServiceBillNo)) { LikeCriteria.Add("ACServiceBillNo", vm.ACServiceBillNo); }
                    if (!string.IsNullOrEmpty(vm.CreatedBy)) { LikeCriteria.Add("CreatedBy", vm.CreatedBy); }
                    if (!string.IsNullOrEmpty(vm.ACSparePartsUsed)) { LikeCriteria.Add("ACSparePartsUsed", vm.ACSparePartsUsed); }
                    if (!string.IsNullOrEmpty(AM_SparePartsUsedfile)) { LikeCriteria.Add("AM_SparePartsUsedfile", AM_SparePartsUsedfile); }

                    Dictionary<long, IList<VehicleAcMaintanceReport_Vw>> VehicleMaintenance = ts.GetVehicleACMaintenanceReport_VwListWithPagingAndCriteria(page - 1, rows, sord, sidx, LikeCriteria);
                    if (VehicleMaintenance != null && VehicleMaintenance.Count > 0)
                    {
                        if (ExportType == "Excel")
                        {
                            var List = VehicleMaintenance.First().Value.ToList();
                            ExptToXL(List, "VehicleACMaintenanceReportList", (items => new
                            {
                                items.VehicleId,
                                items.Campus,
                                items.VehicleNo,
                                items.ACModel,
                                items.ACMaintenanceType,
                                items.ACDateOfBreakdown,
                                items.ACBreakdownLocation,
                                ACPlannedDateOfService = items.ACPlannedDateOfService != null ? items.ACPlannedDateOfService.Value.ToString("dd/MM/yyyy") : "",
                                ACActualDateOfService = items.ACActualDateOfService != null ? items.ACActualDateOfService.Value.ToString("dd/MM/yyyy") : "",
                                items.ACServiceProvider,
                                ACServiceCost = items.ACServiceCost.ToString(),
                                items.ACServiceBillNo,
                                items.ACSparePartsUsed,
                                items.CreatedDate,
                                items.CreatedBy
                            }));
                            return new EmptyResult();
                        }
                        else
                        {
                            long totalrecords = VehicleMaintenance.First().Key;
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var ACMaintenanceList = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalrecords,
                                rows = (from items in VehicleMaintenance.First().Value
                                        select new
                                        {
                                            i = 2,
                                            cell = new string[] {
                               items.Id.ToString(), items.VehicleId.ToString(),items.Campus, items.VehicleNo,items.ACModel, items.ACMaintenanceType,
                               
                               items.ACDateOfBreakdown!=null?items.ACDateOfBreakdown.Value.ToString("dd/MM/yyyy"):"",
                               items.ACBreakdownLocation,
                               items.ACPlannedDateOfService!=null?items.ACPlannedDateOfService.Value.ToString("dd/MM/yyyy"):"",
                               items.ACActualDateOfService!=null?items.ACActualDateOfService.Value.ToString("dd/MM/yyyy"):"",
                               items.ACServiceProvider,
                               items.ACServiceCost.ToString(),items.ACServiceBillNo,items.ACSparePartsUsed ,
                               items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy"):"",
                               items.CreatedBy
                            }
                                        })
                            };
                            return Json(ACMaintenanceList, JsonRequestBehavior.AllowGet);
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
        //VehicleBodyMaintenanceReport

        public ActionResult VehicleBodyMaintenanceReport()
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            Dictionary<long, IList<CampusMaster>> Campus = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            ViewBag.campusddl = Campus.First().Value;
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

        public ActionResult VehicleBodyMaintenanceReportJqGrid(string ExportType, VehicleBodyMaintenance vm,
     string VehicleNo, string Campus, string BTypeOfBody, string BDateOfRepair, string BTypeOfRepair, string BPartsRequired, string BServiceProvider,
     string BServiceCost, string BBillNo, string BDescription, string CreatedDate, string CreatedBy, string BM_SparePartsUsedfile,
         int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    if (!string.IsNullOrEmpty(Campus)) { criteria.Add("Campus", Campus); }
                    sord = sord == "desc" ? "Desc" : "Asc";

                    if (vm.VehicleId > 0)
                        criteria.Add("VehicleId", vm.VehicleId);
                    Dictionary<string, object> ExactCriteria = new Dictionary<string, object>();
                    Dictionary<string, object> LikeCriteria = new Dictionary<string, object>();
                    if (!string.IsNullOrEmpty(BServiceCost))
                    {
                        decimal ServiceCost = Convert.ToDecimal(BServiceCost);
                        ExactCriteria.Add("BServiceCost", ServiceCost);
                    }
                    if (!string.IsNullOrEmpty(Campus)) { LikeCriteria.Add("Campus", Campus); }
                    if (!string.IsNullOrEmpty(VehicleNo)) { LikeCriteria.Add("VehicleNo", VehicleNo); }
                    if (!string.IsNullOrEmpty(BTypeOfBody)) { LikeCriteria.Add("BTypeOfBody", BTypeOfBody); }
                    if (!string.IsNullOrEmpty(BTypeOfRepair)) { LikeCriteria.Add("BTypeOfRepair", BTypeOfRepair); }
                    if (!string.IsNullOrEmpty(BPartsRequired)) { LikeCriteria.Add("BPartsRequired", BPartsRequired); }
                    if (!string.IsNullOrEmpty(BServiceProvider)) { LikeCriteria.Add("BServiceProvider", BServiceProvider); }
                    if (!string.IsNullOrEmpty(BBillNo)) { LikeCriteria.Add("BBillNo", BBillNo); }
                    if (!string.IsNullOrEmpty(BDescription)) { LikeCriteria.Add("BDescription", BDescription); }
                    if (!string.IsNullOrEmpty(CreatedBy)) { LikeCriteria.Add("CreatedBy", CreatedBy); }
                    if (!string.IsNullOrEmpty(BM_SparePartsUsedfile)) { LikeCriteria.Add("BM_SparePartsUsedfile", BM_SparePartsUsedfile); }
                    Dictionary<long, IList<VehicleBodyMaintainance_Vw>> VehicleBodyMaintenance = ts.GetVehicleBodyMaintainance_VwWithPagingLikeSearch(page - 1, rows, sord, sidx, ExactCriteria, LikeCriteria);
                    if (VehicleBodyMaintenance != null && VehicleBodyMaintenance.Count > 0)
                    {
                        if (ExportType == "Excel")
                        {
                            var List = VehicleBodyMaintenance.First().Value.ToList();
                            ExptToXL(List, "VehicleBodyMaintenanceReportList", (items => new
                            {
                                items.Campus,
                                items.VehicleId,
                                items.VehicleNo,
                                items.BTypeOfBody,
                                BDateOfRepair = items.BDateOfRepair != null ? items.BDateOfRepair.Value.ToString("dd/MM/yyyy") : "",
                                items.BTypeOfRepair,
                                items.BPartsRequired,
                                items.BServiceProvider,
                                BServiceCost = items.BServiceCost.ToString(),
                                items.BBillNo,
                                items.BDescription,
                                CreatedDate = items.CreatedDate != null ? items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : "",
                                items.CreatedBy
                            }));
                            return new EmptyResult();
                        }
                        else
                        {
                            long totalrecords = VehicleBodyMaintenance.First().Key;
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var BodyMaintenanceList = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalrecords,
                                rows = (from items in VehicleBodyMaintenance.First().Value
                                        select new
                                        {
                                            i = 2,
                                            cell = new string[] {
                               items.Id.ToString(), items.VehicleId.ToString(),items.Campus, items.VehicleNo,items.BTypeOfBody,
                               items.BDateOfRepair!=null?items.BDateOfRepair.Value.ToString("dd/MM/yyyy"):"",
                               items.BTypeOfRepair, items.BPartsRequired, items.BServiceProvider, items.BServiceCost.ToString(),items.BBillNo,items.BDescription,
                               items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt"):"",
                               items.CreatedBy
                            }
                                        })
                            };
                            return Json(BodyMaintenanceList, JsonRequestBehavior.AllowGet);
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

        //VehicleDistanceCoveredReport

        public ActionResult VehicleDistanceCoveredReport()
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<long, IList<CampusMaster>> Campus = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                ViewBag.campusddl = Campus.First().Value;
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    int[] years = new int[15];
                    DateTime daytime = DateTime.Now;
                    int CurYear = daytime.Year;
                    int CurMonth = daytime.Month;
                    ViewBag.CurYear = CurYear;
                    ViewBag.CurMonth = CurMonth;
                    CurYear = CurYear - 5;
                    for (int i = 0; i < 15; i++)
                    {
                        years[i] = CurYear + i;
                    }
                    ViewBag.years = years;
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
        public ActionResult VehicleDistanceReportJqGrid(string Campus, string Type, string VehicleNo, string Month, int? CurrMonth, int? CurrYear, string Year, string DistanceCovered, int rows, string sidx, string sord, int? page = 1, int? ExptXl = 0)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                TransportService tns = new TransportService();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (!string.IsNullOrEmpty(Campus))
                    criteria.Add("Campus", Campus.Trim());
                if (!string.IsNullOrEmpty(Type))
                    criteria.Add("Type", Type.Trim());
                if (!string.IsNullOrEmpty(VehicleNo))
                    criteria.Add("VehicleNo", VehicleNo.Trim());
                if (CurrMonth >= 0)
                    criteria.Add("Month", CurrMonth);
                if (!string.IsNullOrEmpty(Month))
                {
                    if (Month == "select")
                    {
                        if (CurrMonth >= 0)
                            criteria.Remove("Month");
                        criteria.Add("Month", CurrMonth);
                    }
                    else
                    {
                        criteria.Remove("Month");
                        criteria.Add("Month", Convert.ToInt32(Month));
                    }
                }
                if (CurrYear >= 0)
                    criteria.Add("Year", CurrYear);
                if (!string.IsNullOrEmpty(Year))
                {
                    criteria.Add("Year", Convert.ToInt32(Year));
                }
                if (!string.IsNullOrEmpty(DistanceCovered))
                {
                    criteria.Add("DistanceCovered", Convert.ToDecimal(DistanceCovered));
                }
                Dictionary<long, IList<DistanceCovered_Vw>> VehicleDistanceList = tns.GetDistanceCovered_VwWithPagingLikeSearch(page - 1, rows, sidx, sord, criteria);
                if (VehicleDistanceList != null && VehicleDistanceList.Count > 0 && VehicleDistanceList.FirstOrDefault().Key > 0 && VehicleDistanceList.FirstOrDefault().Value.Count > 0)
                {
                    if (ExptXl == 1)
                    {
                        var List = VehicleDistanceList.First().Value.ToList();
                        base.ExptToXL(List, "VehicleDistanceCoveredReport", (items => new
                        {
                            items.Id,
                            items.Campus,
                            items.Type,
                            items.VehicleNo,
                            items.Month,
                            items.Year,
                            items.DistanceCovered,
                            LastTripDate = items.LastTripDate != null ? items.LastTripDate.Value.ToString("dd/MM/yyyy") : null
                        }));
                        return new EmptyResult();
                    }
                    else
                    {
                        long totalRecords = VehicleDistanceList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                        var jsonData = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalRecords,
                            rows = (
                                 from items in VehicleDistanceList.First().Value
                                 select new
                                 {
                                     i = items.Id,
                                     cell = new string[] { 
                                     items.Id.ToString(), 
                                     items.Campus,
                                     items.Type,
                                     items.VehicleNo,
                                     items.Month.ToString(),
                                     items.Year.ToString(), 
                                     items.DistanceCovered.ToString(),
                                     items.LastTripDate!=null? items.LastTripDate.Value.ToString("dd/MM/yyyy"):null
                                     }
                                 })
                        };
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                    }
                }
                var Empty = new { rows = (new { cell = new string[] { } }) };
                return Json(Empty, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        //VehicleDistanceFuelReport

        public ActionResult VehicleDistanceFuelReport()
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            Dictionary<long, IList<CampusMaster>> Campus = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            ViewBag.campusddl = Campus.First().Value;
            int[] years = new int[15];
            DateTime daytime = DateTime.Now;
            int CurYear = daytime.Year;
            int CurMonth = daytime.Month;
            ViewBag.CurYear = CurYear;
            ViewBag.CurMonth = CurMonth;
            CurYear = CurYear - 5;
            for (int i = 0; i < 15; i++)
            {
                years[i] = CurYear + i;
            }
            ViewBag.years = years;

            #region BreadCrumb
            string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
            #endregion
            return View();
        }

        public ActionResult VehicleDistanceFuelReportJqGrid(VehicleDistanceFuelReport_vw veh, string Campus, string Type, string VehicleNo, int? CurrMonth, int? CurrYear, string Month, string Year, int rows, string sidx, string sord, int? page = 1, int? ExptXl = 0)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                TransportService tns = new TransportService();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (veh.VehicleId > 0)
                    criteria.Add("VehicleId", veh.VehicleId);
                if (!string.IsNullOrEmpty(Type))
                    criteria.Add("Type", Type.Trim());
                if (!string.IsNullOrEmpty(Campus))
                    criteria.Add("Campus", Campus.Trim());
                if (!string.IsNullOrEmpty(VehicleNo))
                    criteria.Add("VehicleNo", VehicleNo.Trim());


                if (veh.DistanceCovered > 0)
                    criteria.Add("DistanceCovered", Convert.ToDecimal(veh.DistanceCovered));
                if (veh.FuelConsumed > 0)
                    criteria.Add("FuelConsumed", Convert.ToDecimal(veh.FuelConsumed));
                if (CurrMonth >= 0)
                    criteria.Add("Month", CurrMonth);
                if (CurrYear >= 0)
                    criteria.Add("Year", CurrYear);
                if (!string.IsNullOrEmpty(Month))
                {
                    if (Month == "select")
                    {
                        if (CurrMonth >= 0)
                            criteria.Remove("Month");
                        criteria.Add("Month", CurrMonth);
                    }
                    else
                    {
                        criteria.Remove("Month");
                        criteria.Add("Month", Convert.ToInt32(Month));
                    }
                }
                if (!string.IsNullOrEmpty(Year))
                {
                    criteria.Add("Year", Convert.ToInt32(Year));
                }
                Dictionary<long, IList<VehicleDistanceFuelReport_vw>> VehicleFuelList = tns.GetVehicleDistanceFuelReport_vwListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (VehicleFuelList != null && VehicleFuelList.Count > 0 && VehicleFuelList.FirstOrDefault().Key > 0 && VehicleFuelList.FirstOrDefault().Value.Count > 0)
                {
                    if (ExptXl == 1)
                    {
                        var List = VehicleFuelList.First().Value.ToList();
                        base.ExptToXL(List, "VehicleDistanceFuelReport", (items => new
                        {
                            items.Id,
                            items.VehicleId,
                            items.Campus,
                            items.Type,
                            items.VehicleNo,

                            DistanceCovered = items.DistanceCovered.ToString(),
                            FuelConsumed = items.FuelConsumed.ToString(),
                            Mileage = items.DistanceCovered != 0 && items.FuelConsumed != 0 ? (items.DistanceCovered / items.FuelConsumed).Value.ToString("#.#") : "",
                            Month = items.Month.ToString(),
                            Year = items.Year.ToString(),
                        }));
                        return new EmptyResult();
                    }
                    else
                    {
                        long totalRecords = VehicleFuelList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                        var jsonData = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalRecords,
                            rows = (
                                 from items in VehicleFuelList.First().Value
                                 select new
                                 {
                                     i = items.Id,
                                     cell = new string[] { 
                                    items.Id.ToString(),
                                    items.Campus,
                                    items.VehicleId.ToString(),
                                    items.Type,
                                    items.VehicleNo,
                                    items.DistanceCovered!=null? items.DistanceCovered.ToString():null,
                                    items.FuelConsumed.ToString(),
                                    items.FuelConsumed!=null && items.DistanceCovered!=null && items.FuelConsumed!=0 && items.DistanceCovered!=0 ? (items.DistanceCovered/items.FuelConsumed).Value.ToString("#.#"):"",
                                    items.Month.ToString(),
                                    items.Year.ToString(),
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
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        //VehicleFuelReport

        public ActionResult VehicleFuelReport()
        {
            string userId = base.ValidateUser();
            if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
            else
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<long, IList<CampusMaster>> Campus = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                ViewBag.ddlcampus = Campus.First().Value;
                int[] years = new int[15];
                DateTime daytime = DateTime.Now;
                int CurYear = daytime.Year;
                int CurMonth = daytime.Month;
                ViewBag.CurYear = CurYear;
                ViewBag.CurMonth = CurMonth;
                CurYear = CurYear - 5;

                for (int i = 0; i < 15; i++)
                {
                    years[i] = CurYear + i;
                }
                ViewBag.years = years;
                #region BreadCrumb
                string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                #endregion
                return View();
            }

        }

        public ActionResult VehicleFuelReportJqGrid(string Campus, string Type, string VehicleNo, string FuelQty, string FromDate, string ToDate, int rows, string sidx, string sord, int? page = 1, int? ExptXl = 0)
        {
            try
            {
                string query = "";
                query = query + " SELECT     ROW_NUMBER() OVER (ORDER BY a.Campus) AS Id, a.Campus, a.VehicleNo, a.VehicleId,a.VehicleType,a.FuelType,a.Type, ";
                query = query + " SUM(FuelQty) FuelQty, SUM(a.TotalPrice)  TotalPrice,LastFilledDate =(SELECT     TOP 1 FilledDate FROM VehicleFuelManagement   WHERE  VehicleNo = a.VehicleNo  ORDER BY FilledDate DESC) ";
                query = query + " FROM    ( SELECT  Campus ,VehicleId, VehicleNo,VehicleType,Type,FuelType,FilledDate,LitrePrice,SUM(FuelQuantity) FuelQty, ";
                query = query + " SUM(TotalPrice) TotalPrice From VehicleFuelManagement  ";
                if (!string.IsNullOrWhiteSpace(Campus) || !string.IsNullOrWhiteSpace(FromDate) || !string.IsNullOrWhiteSpace(ToDate))
                {
                    query = query + "where";
                    if (!string.IsNullOrWhiteSpace(Campus))
                    {
                        query = query + " Campus = '" + Campus + "' and ";
                    }
                    if (!string.IsNullOrWhiteSpace(FromDate) && !string.IsNullOrWhiteSpace(ToDate))
                    {
                        query = query + "  FilledDate between CONVERT(datetime, '" + FromDate + "',103) and CONVERT(datetime,'" + ToDate + "',103) ";
                    }
                }
                query = query + "  GROUP BY Campus,VehicleId,VehicleNo,VehicleType,FuelType,FilledDate,LitrePrice,Type) a ";
                query = query + "  GROUP BY a.Campus, a.VehicleId, a.VehicleNo,a.VehicleType,a.FuelType,a.Type ";
                DataTable FuelRefilList = ts.FuelReportDetailsNew(query);
                List<DataRow> FuelConsumption = null;
                ////Dictionary<long, IList<VehicleFuelReport_vw>> VehicleFuelList = tns.GetVehicleFuelListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                //if (FuelRefilList != null && FuelRefilList.Count > 0 && FuelRefilList.FirstOrDefault().Key > 0 && FuelRefilList.FirstOrDefault().Value.Count > 0)
                //{
                if (FuelRefilList != null)
                {
                    FuelConsumption = FuelRefilList.AsEnumerable().ToList();

                    if (FuelConsumption.Count > 0)
                    {

                        if (ExptXl == 1)
                        {
                            ExptToXL(FuelConsumption, "FuelConsumptionReport", (items => new
                            {
                                Id = items.ItemArray[0].ToString(),
                                Campus = items.ItemArray[1].ToString(),
                                VehicleNo = items.ItemArray[2].ToString(),
                                VehicleId = items.ItemArray[3].ToString(),
                                VehicleType = items.ItemArray[4].ToString(),
                                FuelType = items.ItemArray[5].ToString(),
                                Type = items.ItemArray[6].ToString(),
                                FromDate = !string.IsNullOrWhiteSpace(FromDate) ? FromDate.ToString() : "",
                                ToDate = !string.IsNullOrWhiteSpace(ToDate) ? ToDate.ToString() : "",
                                FuelQty = items.ItemArray[7].ToString(),
                                TotalPrice = items.ItemArray[8].ToString(),
                                LastFilledDate = items.ItemArray[9].ToString(),

                            }));
                            return new EmptyResult();
                        }
                        else
                        {
                            long totalRecords = FuelConsumption.Count;
                            int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                            var jsonData = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalRecords,
                                rows = (
                                     from items in FuelConsumption
                                     select new
                                     {
                                         cell = new string[] { 
                                     items.ItemArray[0].ToString(),
                                     items.ItemArray[1].ToString(),
                                     items.ItemArray[2].ToString(),
                                     items.ItemArray[3].ToString(),
                                     items.ItemArray[4].ToString(),
                                     items.ItemArray[5].ToString(),
                                     items.ItemArray[6].ToString(),
                                     !string.IsNullOrWhiteSpace(FromDate)? FromDate.ToString():"",
                                     !string.IsNullOrWhiteSpace(ToDate)? ToDate.ToString():"",
                                     items.ItemArray[7].ToString(),
                                     items.ItemArray[8].ToString(),
                                     items.ItemArray[9].ToString(),
                                     }
                                     })
                            };
                            return Json(jsonData, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                var AssLst = new { rows = (new { cell = new string[] { } }) };
                return Json(AssLst, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        //VehicleReport

        public ActionResult VehicleReport()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<long, IList<CampusMaster>> Campus = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                    ViewBag.campusddl = Campus.First().Value;
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

        public ActionResult VehicleReportJqGrid(string FromDate, string ToDate, VehicleReport vr, string ExportType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    IList<VehicleReport> VehicleReport = new List<VehicleReport>();
                    TransportService ts = new TransportService();
                    Dictionary<string, object> VehicleCriteria = new Dictionary<string, object>();
                    if (!string.IsNullOrWhiteSpace(vr.Campus)) VehicleCriteria.Add("Campus", vr.Campus);
                    if (!string.IsNullOrWhiteSpace(vr.Type)) VehicleCriteria.Add("Type", vr.Type);
                    if (!string.IsNullOrWhiteSpace(vr.VehicleNo)) VehicleCriteria.Add("VehicleNo", vr.VehicleNo);


                    string[] alias = new string[1];
                    alias[0] = "VehicleTypeMaster";
                    Dictionary<long, IList<VehicleReport>> VehicleList = ts.GetVehicleReportListWithsearchCriteriaLikeSearch(page - 1, rows, string.Empty, string.Empty, VehicleCriteria, alias);
                    if (VehicleList != null && VehicleList.FirstOrDefault().Value != null && VehicleList.FirstOrDefault().Value.Count() > 0)
                    {
                        Dictionary<string, object> criteria = new Dictionary<string, object>();
                        IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);

                        DateTime?[] FromTo = new DateTime?[2];
                        DateTime tdate = DateTime.Now;
                        if (!string.IsNullOrWhiteSpace(FromDate))
                        {
                            FromTo[0] = DateTime.Parse(FromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        }
                        if (!string.IsNullOrWhiteSpace(ToDate))
                        {
                            tdate = DateTime.Parse(ToDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                            string To = string.Format("{0:dd/MM/yyyy}", tdate);
                            tdate = Convert.ToDateTime(To + " " + "23:59:59");
                        }

                        FromTo[1] = tdate;

                        foreach (var item in VehicleList.FirstOrDefault().Value)
                        {
                            VehicleReport vehrep = new VehicleReport();
                            vehrep.Id = item.Id;
                            vehrep.VehicleId = Convert.ToInt32(item.Id);
                            vehrep.Type = item.Type;
                            vehrep.VehicleNo = item.VehicleNo;
                            vehrep.Campus = item.Campus;

                            criteria.Clear();
                            criteria.Add("VehicleId", item.Id);
                            if (FromTo[0] != null && FromTo[1] != null)
                                criteria.Add("InDateTime", FromTo);
                            Dictionary<long, IList<VehicleDistanceCovered>> DistanceList = ts.GetVehicleDistanceCoveredListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                            if (DistanceList != null && DistanceList.FirstOrDefault().Value != null && DistanceList.FirstOrDefault().Value.Count() > 0)
                            {
                                var DistanceArray = (from items in DistanceList.FirstOrDefault().Value
                                                     group items by items.VehicleId into g
                                                     select g.Sum(p => p.DistanceCovered)).ToArray();
                                if (DistanceArray != null)
                                    vehrep.DistanceCovered = DistanceArray[0];
                            }

                            criteria.Clear();
                            criteria.Add("VehicleId", item.Id);
                            if (FromTo[0] != null && FromTo[1] != null)
                                criteria.Add("CreatedDate", FromTo);
                            Dictionary<long, IList<VehicleFuelManagement>> VehicleFuelManagement = ts.GetVehicleFuelManagementListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);

                            if (VehicleFuelManagement != null && VehicleFuelManagement.FirstOrDefault().Value != null && VehicleFuelManagement.FirstOrDefault().Value.Count() > 0)
                            {
                                var FuelArray = (from items in VehicleFuelManagement.FirstOrDefault().Value
                                                 group items by items.VehicleId into g
                                                 select new
                                                 {
                                                     cell = new decimal?[]{
                                                         g.Sum(p => p.FuelQuantity),
                                                         g.Sum(p => p.TotalPrice),
                                                 }
                                                 }).ToArray();
                                if (FuelArray != null)
                                    vehrep.FuelConsumed = FuelArray[0].cell[0];
                                vehrep.FuelCost = FuelArray[0].cell[1];
                            }

                            vehrep.Mileage = vehrep.DistanceCovered != 0 && vehrep.FuelConsumed != 0 ? Convert.ToDecimal((vehrep.DistanceCovered / vehrep.FuelConsumed)) : 0;

                            criteria.Clear();
                            criteria.Add("VehicleId", (Int64)item.Id);
                            if (FromTo[0] != null && FromTo[1] != null)
                                criteria.Add("CreatedDate", FromTo);
                            Dictionary<long, IList<FitnessCertificate>> FitnessCertificate = ts.GetFitnessCertificateDetailsWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);

                            if (FitnessCertificate != null && FitnessCertificate.FirstOrDefault().Value != null && FitnessCertificate.FirstOrDefault().Value.Count() > 0)
                            {
                                var FCArray = (from items in FitnessCertificate.FirstOrDefault().Value
                                               group items by items.VehicleId into g
                                               select g.Sum(p => p.FCCost)).ToArray();
                                if (FCArray != null)
                                    vehrep.FC = FCArray[0];
                            }
                            criteria.Clear();
                            criteria.Add("VehicleId", (Int64)item.Id);
                            if (FromTo[0] != null && FromTo[1] != null)
                                criteria.Add("CreatedDate", FromTo);
                            Dictionary<long, IList<Insurance>> Insurance = ts.GetInsuranceDetailsWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);

                            if (Insurance != null && Insurance.FirstOrDefault().Value != null && Insurance.FirstOrDefault().Value.Count() > 0)
                            {
                                var InsArray = (from items in Insurance.FirstOrDefault().Value
                                                group items by items.VehicleId into g
                                                select g.Sum(p => p.InsuranceDeclaredValue)).ToArray();
                                if (InsArray != null)
                                    vehrep.Insurance = InsArray[0];
                            }
                            criteria.Clear();
                            criteria.Add("VehicleId", item.Id);
                            if (FromTo[0] != null && FromTo[1] != null)
                                criteria.Add("CreatedDate", FromTo);
                            Dictionary<long, IList<VehicleMaintenance>> VehicleMaintenance = ts.GetVehicleMaintenanceListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);

                            if (VehicleMaintenance != null && VehicleMaintenance.FirstOrDefault().Value != null && VehicleMaintenance.FirstOrDefault().Value.Count() > 0)
                            {
                                var VehMaintenanceArray = (from items in VehicleMaintenance.FirstOrDefault().Value
                                                           group items by items.VehicleId into g
                                                           select g.Sum(p => p.VehicleSeviceCost)).ToArray();
                                if (VehMaintenanceArray != null)
                                    vehrep.MechanicalMaintenance = VehMaintenanceArray[0];
                            }

                            criteria.Clear();
                            criteria.Add("VehicleId", item.Id);
                            if (FromTo[0] != null && FromTo[1] != null)
                                criteria.Add("CreatedDate", FromTo);
                            Dictionary<long, IList<VehicleACMaintenance>> VehicleACMaintenance = ts.GetVehicleACMaintenanceListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                            if (VehicleACMaintenance != null && VehicleACMaintenance.FirstOrDefault().Value != null && VehicleACMaintenance.FirstOrDefault().Value.Count() > 0)
                            {
                                var VehACMaintenanceArray = (from items in VehicleACMaintenance.FirstOrDefault().Value
                                                             group items by items.VehicleId into g
                                                             select g.Sum(p => p.ACServiceCost)).ToArray();
                                if (VehACMaintenanceArray != null)
                                    vehrep.ACMaintenance = VehACMaintenanceArray[0];
                            }

                            criteria.Clear();
                            criteria.Add("VehicleId", item.Id);
                            if (FromTo[0] != null && FromTo[1] != null)
                                criteria.Add("CreatedDate", FromTo);
                            Dictionary<long, IList<VehicleElectricalMaintenance>> VehicleElectricalMaintenance = ts.GetVehicleElectricalMaintenanceListWithsearchCriteria(0, 9999, string.Empty, string.Empty, criteria);
                            if (VehicleElectricalMaintenance != null && VehicleElectricalMaintenance.FirstOrDefault().Value != null && VehicleElectricalMaintenance.FirstOrDefault().Value.Count() > 0)
                            {
                                var VehElecMaintenanceArray = (from items in VehicleElectricalMaintenance.FirstOrDefault().Value
                                                               group items by items.VehicleId into g
                                                               select g.Sum(p => p.EServiceCost)).ToArray();
                                if (VehElecMaintenanceArray != null)
                                    vehrep.ElectricalMaintenance = VehElecMaintenanceArray[0];
                            }

                            criteria.Clear();
                            criteria.Add("VehicleId", item.Id);
                            if (FromTo[0] != null && FromTo[1] != null)
                                criteria.Add("CreatedDate", FromTo);
                            Dictionary<long, IList<VehicleBodyMaintenance>> VehicleBodyMaintenance = ts.GetVehicleBodyMaintenanceListWithsearchCriteria(0, 9999, string.Empty, string.Empty, criteria);
                            if (VehicleBodyMaintenance != null && VehicleBodyMaintenance.FirstOrDefault().Value != null && VehicleBodyMaintenance.FirstOrDefault().Value.Count() > 0)
                            {
                                var VehBodyMaintenanceArray = (from items in VehicleBodyMaintenance.FirstOrDefault().Value
                                                               group items by items.VehicleId into g
                                                               select g.Sum(p => p.BServiceCost)).ToArray();
                                if (VehBodyMaintenanceArray != null)
                                    vehrep.BodyMaintenance = VehBodyMaintenanceArray[0];
                            }
                            criteria.Clear();
                            criteria.Add("VehicleId", item.Id);
                            if (FromTo[0] != null && FromTo[1] != null)
                                criteria.Add("CreatedDate", FromTo);
                            Dictionary<long, IList<VehicleTyreMaintenance>> VehicleTyreMaintenance = ts.GetVehicleTyreMaintenanceListWithsearchCriteria(0, 9999, string.Empty, string.Empty, criteria);
                            if (VehicleTyreMaintenance != null && VehicleTyreMaintenance.FirstOrDefault().Value != null && VehicleTyreMaintenance.FirstOrDefault().Value.Count() > 0)
                            {
                                var VehTyreMaintenanceArray = (from items in VehicleTyreMaintenance.FirstOrDefault().Value
                                                               // where items.CostOfService != null && items.TyreCost != null && items.TyreServiceCost != null
                                                               group items by items.VehicleId into g
                                                               select
                                                               new
                                                               {
                                                                   cell = new decimal?[] {
                                                                   g.Sum(p => p.CostOfService),
                                                                   g.Sum(p=>p.TyreCost),
                                                                   g.Sum(p=>p.TyreServiceCost)
                                                              }
                                                               }
                                                               ).ToArray();
                                if (VehTyreMaintenanceArray != null && VehTyreMaintenanceArray[0].cell != null)
                                    vehrep.TyreMaintenance = Convert.ToDecimal(VehTyreMaintenanceArray[0].cell[0]) + Convert.ToDecimal(VehTyreMaintenanceArray[0].cell[1] + Convert.ToDecimal(VehTyreMaintenanceArray[0].cell[2]));
                            }

                            VehicleReport.Add(vehrep);

                        }
                    }

                    if (VehicleReport != null && VehicleReport.Count() > 0)
                    {
                        if (ExportType == "Excel")
                        {
                            var List = VehicleReport.ToList();
                            ExptToXL(List, "VehicleReport", (items => new
                            {
                                items.Id,
                                VehicleId = items.VehicleId.ToString(),

                                items.Type,
                                items.Campus,
                                items.VehicleNo,
                                DistanceCovered = items.DistanceCovered.ToString(),
                                FuelConsumed = items.FuelConsumed.ToString(),
                                FuelCost = items.FuelCost.ToString(),
                                Mileage = items.Mileage != null ? items.Mileage.Value.ToString("#.#") : "",
                                FC = items.FC.ToString(),
                                Insurance = items.Insurance.ToString(),
                                MechanicalMaintenance = items.MechanicalMaintenance.ToString(),
                                ACMaintenance = items.ACMaintenance.ToString(),
                                ElectricalMaintenance = items.ElectricalMaintenance.ToString(),
                                BodyMaintenance = items.BodyMaintenance.ToString(),
                                TyreMaintenance = items.TyreMaintenance.ToString()
                            }));
                            return new EmptyResult();
                        }
                        else
                        {
                            long totalrecords = VehicleList.FirstOrDefault().Key;
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var VehicleReportList = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalrecords,
                                rows = (from items in VehicleReport
                                        select new
                                        {
                                            cell = new string[] {
                                items.Id.ToString(),
                                items.VehicleId.ToString(),
                                
                                items.Type,
                                items.Campus,
                                items.VehicleNo,
                                items.DistanceCovered.ToString(),
                                items.FuelConsumed.ToString(),
                                items.FuelCost.ToString(),
                                items.Mileage!=null? items.Mileage.Value.ToString("#.#"):"",
                                items.FC.ToString(),
                                items.Insurance.ToString(),
                                items.MechanicalMaintenance.ToString(),
                                items.ACMaintenance.ToString(),
                                items.ElectricalMaintenance.ToString(),
                                items.BodyMaintenance.ToString(),
                                items.TyreMaintenance.ToString()
                            }
                                        })
                            };
                            return Json(VehicleReportList, JsonRequestBehavior.AllowGet);
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
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }

        }

        #endregion

        #region FuelRefilDetails
        public ActionResult FuelRefilDetails()
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<long, IList<CampusMaster>> CampusList = ms.GetCampusMasterListWithPagingAndCriteria(0, 40, "", "Desc", criteria);
                ViewBag.ddlcampus = CampusList.First().Value;
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {

                    ViewBag.Success = "";
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
        [HttpPost]
        public ActionResult FuelRefilDetails(VehicleFuelManagement vfm)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    ts.CreateOrUpdateVehicleFuelManagement(vfm);
                    //ts.CreateOrUpdateVehicleFuelManagement(vfm);
                    return RedirectToAction("FuelRefilDetails", "Transport");
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        
        //public ActionResult FuelRefilListDetailsJqGrid(string ExportType, VehicleFuelManagement vfm, string Campus, string VehicleId, string FuelQuantity, string FilledDate, string Mileage,
        //     int? Id, int rows, string sidx, string sord, int? page = 1)
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        Dictionary<string, object> ExactCriteria = new Dictionary<string, object>();
        //        if (ExportType != "Excel" && vfm.VehicleId > 0)
        //            ExactCriteria.Add("VehicleId", vfm.VehicleId);
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //        else
        //        {
        //            TransportService ts = new TransportService();

        //            Dictionary<string, object> LikeCriteria = new Dictionary<string, object>();
        //            sord = sord == "desc" ? "Desc" : "Asc";
        //            if (!string.IsNullOrEmpty(FuelQuantity))
        //                if (!string.IsNullOrEmpty(vfm.Campus)) { LikeCriteria.Add("Campus", vfm.Campus); }
        //            if (!string.IsNullOrEmpty(vfm.VehicleNo)) { LikeCriteria.Add("VehicleNo", vfm.VehicleNo); }
        //            if (!string.IsNullOrEmpty(vfm.FuelType)) { LikeCriteria.Add("FuelType", vfm.FuelType); }
        //            if (!string.IsNullOrEmpty(vfm.FilledBy)) { LikeCriteria.Add("FilledBy", vfm.FilledBy); }
        //            if (!string.IsNullOrEmpty(vfm.BunkName)) { LikeCriteria.Add("BunkName", vfm.BunkName); }
        //            if (!string.IsNullOrEmpty(vfm.FuelFillType)) { LikeCriteria.Add("FuelFillType", vfm.FuelFillType); }
        //            if (!string.IsNullOrEmpty(Mileage)) { LikeCriteria.Add("Mileage", Mileage); }
        //            if (!string.IsNullOrEmpty(vfm.CreatedBy)) { LikeCriteria.Add("CreatedBy", vfm.CreatedBy); }

        //            Dictionary<long, IList<VehicleFuelManagement>> VehicleFuelManagement = ts.VehicleFuelManagementListWithLikeAndExcactSerachCriteria(page - 1, rows, sord, sidx, ExactCriteria, LikeCriteria);
        //            if (VehicleFuelManagement != null && VehicleFuelManagement.Count > 0)
        //            {
        //                if (ExportType == "Excel")
        //                {
        //                    var List = VehicleFuelManagement.First().Value.ToList();
        //                    ExptToXL(List, "FuelReportDetails", (items => new
        //                    {
        //                        Id = items.Id.ToString(),
        //                        Vehicle_Id = items.VehicleId.ToString(),
        //                        Campus = items.Campus,
        //                        Vehicle_No = items.VehicleNo,
        //                        Fuel_Type = items.FuelType,
        //                        Fuel_Quantity = items.FuelQuantity.ToString(),
        //                        LitrePrice = items.LitrePrice.ToString(),
        //                        TotalPrice = items.TotalPrice.ToString(),
        //                        Filled_By = items.FilledBy,
        //                        FilledDate = items.FilledDate.ToString(),
        //                        Bunk_Name = items.BunkName,
        //                        Fuel_Fill_Type = items.FuelFillType,
        //                        Last_Milometer_Reading = items.LastMilometerReading.ToString(),
        //                        Current_Milometer_Reading = items.CurrentMilometerReading.ToString(),
        //                        Mileage = items.Mileage.ToString(),
        //                    }));
        //                    return new EmptyResult();
        //                }
        //                else
        //                {
        //                    long totalrecords = VehicleFuelManagement.First().Key;
        //                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
        //                    var VehicleDistanceList = new
        //                    {
        //                        total = totalPages,
        //                        page = page,
        //                        records = totalrecords,
        //                        rows = (from items in VehicleFuelManagement.First().Value
        //                                select new
        //                                {
        //                                    i = 2,
        //                                    cell = new string[] {

        //                       items.Id.ToString(),items.VehicleId.ToString(),items.Campus,items.VehicleNo,items.FuelType, items.FuelQuantity.ToString(),
        //                       items.LitrePrice.ToString(),
        //                       items.TotalPrice.ToString(),
        //                       items.FilledBy,
        //                       items.FilledDate.ToString(),
        //                       items.BunkName,
        //                       items.FuelFillType,
        //                       items.LastMilometerReading.ToString(),
        //                       items.CurrentMilometerReading.ToString(),
        //                       items.Mileage.ToString(),
        //                       items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt"):"",
        //                       items.CreatedBy,
        //                    }
        //                                })
        //                    };
        //                    return Json(VehicleDistanceList, JsonRequestBehavior.AllowGet);
        //                }
        //            }
        //            else return Json(null);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
        //        throw ex;
        //    }
        //}


        public ActionResult FuelRefilListDetailsJqGrid(string ExportType, VehicleFuelManagement vfm, string Campus, string VehicleId, string FuelQuantity, string FilledDate, string Mileage,
             int? Id, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    Dictionary<string, object> ExactCriteria = new Dictionary<string, object>();
                    Dictionary<string, object> LikeCriteria = new Dictionary<string, object>();
                    sord = sord == "desc" ? "Desc" : "Asc";
                    if (!string.IsNullOrEmpty(FuelQuantity))
                        if (!string.IsNullOrEmpty(vfm.Campus)) { LikeCriteria.Add("Campus", vfm.Campus); }
                    if (!string.IsNullOrEmpty(vfm.VehicleNo)) { LikeCriteria.Add("VehicleNo", vfm.VehicleNo); }
                    if (!string.IsNullOrEmpty(vfm.FuelType)) { LikeCriteria.Add("FuelType", vfm.FuelType); }
                    if (!string.IsNullOrEmpty(vfm.FilledBy)) { LikeCriteria.Add("FilledBy", vfm.FilledBy); }
                    if (!string.IsNullOrEmpty(vfm.BunkName)) { LikeCriteria.Add("BunkName", vfm.BunkName); }
                    if (!string.IsNullOrEmpty(vfm.FuelFillType)) { LikeCriteria.Add("FuelFillType", vfm.FuelFillType); }
                    if (!string.IsNullOrEmpty(Mileage)) { LikeCriteria.Add("Mileage", Mileage); }
                    if (!string.IsNullOrEmpty(vfm.CreatedBy)) { LikeCriteria.Add("CreatedBy", vfm.CreatedBy); }

                    Dictionary<long, IList<VehicleFuelManagement>> VehicleFuelManagement = ts.VehicleFuelManagementListWithLikeAndExcactSerachCriteria(page - 1, rows, sord, sidx, ExactCriteria, LikeCriteria);
                    if (VehicleFuelManagement != null && VehicleFuelManagement.Count > 0)
                    {
                        if (ExportType == "Excel")
                        {
                            var List = VehicleFuelManagement.First().Value.ToList();
                            ExptToXL(List, "FuelReportDetails", (items => new
                            {
                                Id = items.Id.ToString(),
                                Vehicle_Id = items.VehicleId.ToString(),
                                Campus = items.Campus,
                                Vehicle_No = items.VehicleNo,
                                Fuel_Type = items.FuelType,
                                Fuel_Quantity = items.FuelQuantity.ToString(),
                                LitrePrice = items.LitrePrice.ToString(),
                                TotalPrice = items.TotalPrice.ToString(),
                                Filled_By = items.FilledBy,
                                Filled_Date = items.FilledDate.Value.ToString("dd/MM/yyyy"),
                                Bunk_Name = items.BunkName,
                                Fuel_Fill_Type = items.FuelFillType,
                                Is_KM_Reseted = items.IsKMReseted == true ? "Yes" : "No",
                                Last_Milometer_Reading = items.LastMilometerReading.ToString(),
                                KM_Reset_Value = items.KMResetValue.ToString(),
                                Current_Milometer_Reading = items.CurrentMilometerReading.ToString(),
                                Distance = items.Distance.ToString(),
                                Mileage = items.Mileage.ToString(),

                            }));
                            return new EmptyResult();
                        }
                        else
                        {
                            long totalrecords = VehicleFuelManagement.First().Key;
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var VehicleDistanceList = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalrecords,
                                rows = (from items in VehicleFuelManagement.First().Value
                                        select new
                                        {
                                            i = 2,
                                            cell = new string[] {

                               items.Id.ToString(),items.VehicleId.ToString(),items.Campus,items.VehicleNo,items.FuelType, items.FuelQuantity.ToString(),
                               items.LitrePrice.ToString(),
                               items.TotalPrice.ToString(),
                               items.FilledBy,
                               items.FilledDate!=null?items.FilledDate.Value.ToString("dd/MM/yyyy"):"",
                               items.BunkName,
                               items.FuelFillType,
                               items.IsKMReseted==true?"Yes":"No",
                               items.KMResetValue.ToString(),
                               items.LastMilometerReading.ToString(),                               
                               items.CurrentMilometerReading.ToString(),
                               items.Distance.ToString(),
                               items.Mileage.ToString(),
                               items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy"):"",
                               items.CreatedBy,                                                                                             
                            }
                                        })
                            };
                            return Json(VehicleDistanceList, JsonRequestBehavior.AllowGet);
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

        public ActionResult GetVehicleDetails(string VehicleNo, string CurrentMilometerReading, string VehicleId)
        {
            try
            {
                TransportService ts = new TransportService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("VehicleNo", VehicleNo);
                Dictionary<long, IList<FuelRefilDetails_Vw>> FuelList = ts.GetFuelTypeDetails(0, 9999, "Id", "Desc", criteria);
                if (FuelList != null && FuelList.FirstOrDefault().Value != null && FuelList.FirstOrDefault().Key > 0)
                {
                    return Json(FuelList.FirstOrDefault().Value[0], JsonRequestBehavior.AllowGet);
                }
                return Json(null, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        #endregion

        #region Driver OT Details
        public ActionResult DriverOtReportDetails()
        {
            try
            {
                MastersService ms = new MastersService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<long, IList<CampusMaster>> CampusMaster = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                ViewBag.campusddl = CampusMaster.First().Value;
                DateTime daytime = DateTime.Now;
                int CurMonth = daytime.Month;
                ViewBag.CurMonth = CurMonth;

                return View();
            }
            catch (Exception ex)
            {

                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult DriverOtReportDetailsJqgrid(string Campus, string Name, int Month, string OTType, string ExportType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                DateTime DateNow = DateTime.Now;
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {


                    if (string.IsNullOrEmpty(Campus) && string.IsNullOrEmpty(OTType))
                    {
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        DateTime today = DateTime.Today;
                        DateTime first = new DateTime(today.Year, Month, 1);
                        DateTime temp = first.AddMonths(1);
                        DateTime last = temp.AddDays(-1);
                        TransportService ts = new TransportService();

                        Dictionary<string, object> criteria = new Dictionary<string, object>();

                        criteria.Add("Campus", Campus);
                        if (!string.IsNullOrWhiteSpace(Name)) { criteria.Add("Name", Name); }
                        //if (!string.IsNullOrWhiteSpace(OTType)) { criteria.Add("OTType", OTType); }
                        Dictionary<long, IList<DriverOTReportDetails>> DriverOTDetails = ts.GetDriverOTReportDetailsWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                        criteria.Clear();
                        criteria.Add("Campus", Campus);
                        criteria.Add("OTType", OTType);
                        DateTime[] fromto = new DateTime[2];
                        fromto[0] = first;
                        fromto[1] = last;
                        criteria.Add("OTDate", fromto);
                        Dictionary<long, IList<DriverOTDetails>> DriverOTReport = ts.GetDriverOTDetailsListWithPagingAndCriteria(page - 1, rows, string.Empty, sord, criteria);
                        IList<DriverOTDetails> DriverOTRe = DriverOTReport.FirstOrDefault().Value.ToList();
                        IEnumerable<string> blkLong = from p in DriverOTRe
                                                      orderby p.DriverIdNo ascending
                                                      select p.DriverIdNo;
                        string[] Driverid = blkLong.ToArray();
                        foreach (DriverOTReportDetails a in DriverOTDetails.FirstOrDefault().Value)
                        {
                            a.Date1 = "<b style='color:Green'>-</b>"; a.Date2 = "<b style='color:Green'>-</b>"; a.Date3 = "<b style='color:Green'>-</b>"; a.Date4 = "<b style='color:Green'>-</b>"; a.Date5 = "<b style='color:Green'>-</b>"; a.Date6 = "<b style='color:Green'>-</b>"; a.Date7 = "<b style='color:Green'>-</b>"; a.Date8 = "<b style='color:Green'>-</b>"; a.Date9 = "<b style='color:Green'>-</b>"; a.Date10 = "<b style='color:Green'>-</b>"; a.Date11 = "<b style='color:Green'>-</b>"; a.Date12 = "<b style='color:Green'>-</b>"; a.Date13 = "<b style='color:Green'>-</b>"; a.Date14 = "<b style='color:Green'>-</b>"; a.Date15 = "<b style='color:Green'>-</b>";
                            a.Date16 = "<b style='color:Green'>-</b>"; a.Date17 = "<b style='color:Green'>-</b>"; a.Date18 = "<b style='color:Green'>-</b>"; a.Date19 = "<b style='color:Green'>-</b>"; a.Date20 = "<b style='color:Green'>-</b>"; a.Date21 = "<b style='color:Green'>-</b>"; a.Date22 = "<b style='color:Green'>-</b>"; a.Date23 = "<b style='color:Green'>-</b>"; a.Date24 = "<b style='color:Green'>-</b>"; a.Date25 = "<b style='color:Green'>-</b>"; a.Date26 = "<b style='color:Green'>-</b>"; a.Date27 = "<b style='color:Green'>-</b>"; a.Date28 = "<b style='color:Green'>-</b>"; a.Date29 = "<b style='color:Green'>-</b>"; a.Date30 = "<b style='color:Green'>-</b>";
                            a.Date31 = "<b style='color:Green'>-</b>"; a.TotalAllowance = "<b style='color:Green'>0</b>";

                            if (Driverid.Contains((a.DriverIdNo)))
                            {
                                //Dictionary<string, object> criteria1 = new Dictionary<string, object>();
                                //criteria1.Add("OTDate", fromto);
                                Dictionary<long, IList<DriverOTDetails>> DriverOTDeatailsObj = ts.GetDriverOTDetailsListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria);
                                var OTDate = (from p in DriverOTDeatailsObj.FirstOrDefault().Value
                                              where p.OTType == OTType && p.DriverIdNo == a.DriverIdNo
                                              orderby p.OTDate ascending
                                              select p.OTDate).ToArray();


                                a.OTCount = OTDate.Length;
                                var allwonce = DriverOTDeatailsObj.FirstOrDefault().Value
                                    .Where(x => x.OTType == OTType && x.DriverIdNo == a.DriverIdNo)
                                    .GroupBy(x => new { x.OTType })
                                    .Select(g => new
                                    {
                                        sum = g.Sum(x => x.Allowance) //To Do get total allwonce of ot
                                    });

                                a.TotalAllowance = allwonce.FirstOrDefault().sum.ToString();

                                for (var i = 0; i < OTDate.Length; i++)
                                {
                                    string Abvalue = OTDate[i].ToString("dd");

                                    switch (Abvalue)
                                    {
                                        case "1":
                                        case "01": { if (ExportType == "Excel") { a.Date1 = "<b style='color:Red'>X</b>"; } else { a.Date1 = "<b style='color:Red'>X</b>"; } } break;
                                        case "2":
                                        case "02": { if (ExportType == "Excel") { a.Date2 = "<b style='color:Red'>X</b>"; } else { a.Date2 = "<b style='color:Red'>X</b>"; } } break;
                                        case "3":
                                        case "03": { if (ExportType == "Excel") { a.Date3 = "<b style='color:Red'>X</b>"; } else { a.Date3 = "<b style='color:Red'>X</b>"; } } break;
                                        case "4":
                                        case "04": { if (ExportType == "Excel") { a.Date4 = "<b style='color:Red'>X</b>"; } else { a.Date4 = "<b style='color:Red'>X</b>"; } } break;
                                        case "5":
                                        case "05": { if (ExportType == "Excel") { a.Date5 = "<b style='color:Red'>X</b>"; } else { a.Date5 = "<b style='color:Red'>X</b>"; } } break;
                                        case "6":
                                        case "06": { if (ExportType == "Excel") { a.Date6 = "<b style='color:Red'>X</b>"; } else { a.Date6 = "<b style='color:Red'>X</b>"; } } break;
                                        case "7":
                                        case "07": { if (ExportType == "Excel") { a.Date7 = "<b style='color:Red'>X</b>"; } else { a.Date7 = "<b style='color:Red'>X</b>"; } } break;
                                        case "8":
                                        case "08": { if (ExportType == "Excel") { a.Date8 = "<b style='color:Red'>X</b>"; } else { a.Date8 = "<b style='color:Red'>X</b>"; } } break;
                                        case "9":
                                        case "09": { if (ExportType == "Excel") { a.Date9 = "<b style='color:Red'>X</b>"; } else { a.Date9 = "<b style='color:Red'>X</b>"; } } break;
                                        case "10": { if (ExportType == "Excel") { a.Date10 = "<b style='color:Red'>X</b>"; } else { a.Date10 = "<b style='color:Red'>X</b>"; } } break;
                                        case "11": { if (ExportType == "Excel") { a.Date11 = "<b style='color:Red'>X</b>"; } else { a.Date11 = "<b style='color:Red'>X</b>"; } } break;
                                        case "12": { if (ExportType == "Excel") { a.Date12 = "<b style='color:Red'>X</b>"; } else { a.Date12 = "<b style='color:Red'>X</b>"; } } break;
                                        case "13": { if (ExportType == "Excel") { a.Date13 = "<b style='color:Red'>X</b>"; } else { a.Date13 = "<b style='color:Red'>X</b>"; } } break;
                                        case "14": { if (ExportType == "Excel") { a.Date14 = "<b style='color:Red'>X</b>"; } else { a.Date14 = "<b style='color:Red'>X</b>"; } } break;
                                        case "15": { if (ExportType == "Excel") { a.Date15 = "<b style='color:Red'>X</b>"; } else { a.Date15 = "<b style='color:Red'>X</b>"; } } break;
                                        case "16": { if (ExportType == "Excel") { a.Date16 = "<b style='color:Red'>X</b>"; } else { a.Date16 = "<b style='color:Red'>X</b>"; } } break;
                                        case "17": { if (ExportType == "Excel") { a.Date17 = "<b style='color:Red'>X</b>"; } else { a.Date17 = "<b style='color:Red'>X</b>"; } } break;
                                        case "18": { if (ExportType == "Excel") { a.Date18 = "<b style='color:Red'>X</b>"; } else { a.Date18 = "<b style='color:Red'>X</b>"; } } break;
                                        case "19": { if (ExportType == "Excel") { a.Date19 = "<b style='color:Red'>X</b>"; } else { a.Date19 = "<b style='color:Red'>X</b>"; } } break;
                                        case "20": { if (ExportType == "Excel") { a.Date20 = "<b style='color:Red'>X</b>"; } else { a.Date20 = "<b style='color:Red'>X</b>"; } } break;
                                        case "21": { if (ExportType == "Excel") { a.Date21 = "<b style='color:Red'>X</b>"; } else { a.Date21 = "<b style='color:Red'>X</b>"; } } break;
                                        case "22": { if (ExportType == "Excel") { a.Date22 = "<b style='color:Red'>X</b>"; } else { a.Date22 = "<b style='color:Red'>X</b>"; } } break;
                                        case "23": { if (ExportType == "Excel") { a.Date23 = "<b style='color:Red'>X</b>"; } else { a.Date23 = "<b style='color:Red'>X</b>"; } } break;
                                        case "24": { if (ExportType == "Excel") { a.Date24 = "<b style='color:Red'>X</b>"; } else { a.Date24 = "<b style='color:Red'>X</b>"; } } break;
                                        case "25": { if (ExportType == "Excel") { a.Date25 = "<b style='color:Red'>X</b>"; } else { a.Date25 = "<b style='color:Red'>X</b>"; } } break;
                                        case "26": { if (ExportType == "Excel") { a.Date26 = "<b style='color:Red'>X</b>"; } else { a.Date26 = "<b style='color:Red'>X</b>"; } } break;
                                        case "27": { if (ExportType == "Excel") { a.Date27 = "<b style='color:Red'>X</b>"; } else { a.Date27 = "<b style='color:Red'>X</b>"; } } break;
                                        case "28": { if (ExportType == "Excel") { a.Date28 = "<b style='color:Red'>X</b>"; } else { a.Date28 = "<b style='color:Red'>X</b>"; } } break;
                                        case "29": { if (ExportType == "Excel") { a.Date29 = "<b style='color:Red'>X</b>"; } else { a.Date29 = "<b style='color:Red'>X</b>"; } } break;
                                        case "30": { if (ExportType == "Excel") { a.Date30 = "<b style='color:Red'>X</b>"; } else { a.Date30 = "<b style='color:Red'>X</b>"; } } break;
                                        case "31": { if (ExportType == "Excel") { a.Date31 = "<b style='color:Red'>X</b>"; } else { a.Date31 = "<b style='color:Red'>A</b>"; } } break;
                                        default: break;
                                    }


                                }
                            }
                        }
                        if (DriverOTDetails != null && DriverOTDetails.Count > 0)
                        {
                            if (ExportType == "Excel")
                            {
                                var List = DriverOTDetails.First().Value.ToList();
                                ExptToXL(List, "DriverOTDetails", (items => new
                                {
                                    items.Name,
                                    items.Date1,
                                    items.Date2,
                                    items.Date3,
                                    items.Date4,
                                    items.Date5,
                                    items.Date6,
                                    items.Date7,
                                    items.Date8,
                                    items.Date9,
                                    items.Date10,
                                    items.Date11,
                                    items.Date12,
                                    items.Date13,
                                    items.Date14,
                                    items.Date15,
                                    items.Date16,
                                    items.Date17,
                                    items.Date18,
                                    items.Date19,
                                    items.Date20,
                                    items.Date21,
                                    items.Date22,
                                    items.Date23,
                                    items.Date24,
                                    items.Date25,
                                    items.Date26,
                                    items.Date27,
                                    items.Date28,
                                    items.Date29,
                                    items.Date30,
                                    items.Date31,
                                    items.OTCount,
                                    items.TotalAllowance


                                }));
                                return new EmptyResult();
                            }
                            else
                            {

                                long totalrecords = DriverOTDetails.First().Key;
                                int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                                var DriverOT = new
                                {
                                    total = totalPages,
                                    page = page,
                                    records = totalrecords,
                                    rows = (from items in DriverOTDetails.First().Value
                                            select new
                                            {
                                                i = 2,
                                                cell = new string[] {
                                            items.Name,
                                              items.Date1,
                                    items.Date2,
                                    items.Date3,
                                    items.Date4,
                                    items.Date5,
                                    items.Date6,
                                    items.Date7,
                                    items.Date8,
                                    items.Date9,
                                    items.Date10,
                                    items.Date11,
                                    items.Date12,
                                    items.Date13,
                                    items.Date14,
                                    items.Date15,
                                    items.Date16,
                                    items.Date17,
                                    items.Date18,
                                    items.Date19,
                                    items.Date20,
                                    items.Date21,
                                    items.Date22,
                                    items.Date23,
                                    items.Date24,
                                    items.Date25,
                                    items.Date26,
                                    items.Date27,
                                    items.Date28,
                                    items.Date29,
                                    items.Date30,
                                    items.Date31,
                                    items.OTCount.ToString(),
                                    items.TotalAllowance

                        }
                                            })
                                };
                                return Json(DriverOT, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else return Json(null);
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

        private void TipsLogo(PrintDriverProfile DriverProf, string TipsLogo, string NaceLogo)
        {

            DriverProf.TipsLogo = ConfigurationManager.AppSettings["AddHeader"] + TipsLogo;
            DriverProf.TipsNaceLogo = ConfigurationManager.AppSettings["AddHeader"] + NaceLogo;
        }
        private void TipsName(PrintDriverProfile Rptcrd, string imageName)
        {
            string url = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            Rptcrd.TipsName = "THE INDIAN PUBLIC SCHOOL";
        }

        public ActionResult DriverProfilePDF(long id)
        {
            try
            {
                PrintDriverProfile DriverList = new PrintDriverProfile();
                TransportService TransSer = new TransportService();
                AdmissionManagementService ads = new AdmissionManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                DriverMaster DM = TransSer.GetDriverDetailsUsingID(id);
                DriverList.Name = DM.Name != null ? DM.Name : "";
                DriverList.DriverIdNo = DM.DriverIdNo != null ? DM.DriverIdNo : "";
                DriverList.Campus = DM.Campus != null ? DM.Campus : "";
                DriverList.Dob = DM.Dob != null ? DM.Dob : "";
                DriverList.ContactNo = DM.ContactNo != null ? DM.ContactNo : "";
                DriverList.BGRP = DM.BGRP != null ? DM.BGRP : "";
                DriverList.LicenseNo = DM.LicenseNo != null ? DM.LicenseNo : "";
                DriverList.Id = DM.Id;
                DriverList.DriverRegNo = DM.DriverRegNo;
                DriverList.Dob = DM.Dob;
                DriverList.DateOfJoin = DM.DateOfJoin.ToString("dd'/'MM'/'yyyy");
                DriverList.Age = DM.Age;
                DriverList.ESINo = DM.ESINo;
                DriverList.PFNo = DM.PFNo;
                DriverList.Gender = DM.Gender;
                DriverList.NativeState = DM.NativeState;
                DriverList.MaritalStatus = DM.MaritalStatus;
                DriverList.LicenseValDate = DM.LicenseValDate.Value.ToString("dd'/'MM'/'yyyy");
                DriverList.NonTraLicenseValDate = DM.NonTraLicenseValDate.Value.ToString("dd'/'MM'/'yyyy");
                DriverList.PermanentAddress = DM.PermanentAddress;
                DriverList.PresentAddress = DM.PresentAddress;
                DriverList.EmailId = DM.EmailId;
                DriverList.BankName = DM.BankName;
                DriverList.PhoneNo = DM.PhoneNo;
                DriverList.AltPhoneNo = DM.AltPhoneNo;
                DriverList.BankAccountNumber = DM.BankAccountNumber;
                TipsLogo(DriverList, "TipsLogo.jpg", "logonace.jpg");
                TipsName(DriverList, "TipsName.jpg");
                DriverList.EmergencyContactPerson = DM.EmergencyContactPerson != null ? DM.EmergencyContactPerson : "";
                criteria.Add("DriverRegNo", DM.DriverRegNo);
                IList<DriverFamilyDetails> FamilyList = new List<DriverFamilyDetails>();
                Dictionary<long, IList<DriverFamilyDetails>> FamilyDetailsList = TransSer.GetDriverFamilyDetailsListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                if (FamilyDetailsList != null && FamilyDetailsList.Count > 0 && FamilyDetailsList.FirstOrDefault().Key > 0)
                {
                    foreach (var item in FamilyDetailsList.FirstOrDefault().Value)
                    {
                        DriverFamilyDetails DFD = new DriverFamilyDetails();
                        DFD.FName = item.FName;
                        DFD.FRelationship = item.FRelationship;
                        DFD.FOccupation = item.FOccupation;
                        DFD.FAge = item.FAge;
                        FamilyList.Add(DFD);
                    }
                }
                DriverList.DriverFamilyDetailsList = FamilyList;
                // DriverList.FileName = DM.Name + "_DriverProfile";
                //return DriverList;
                return new Rotativa.ViewAsPdf("DriverProfilePDF", DriverList)
                {
                    FileName = DriverList.Name + ".pdf",
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageSize = Rotativa.Options.Size.A4,
                    //PageWidth = 110,
                    //PageHeight = 197,
                    PageMargins = new Rotativa.Options.Margins(0, 0, 0, 0)
                };
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
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
                criteria.Add("DocumentFor", "Driver");
                criteria.Add("DocumentType", "Driver Photo");
                PrintDriverProfile DriverProf = new PrintDriverProfile();
                Dictionary<long, IList<UploadedFiles>> UploadedFiles = ads.GetUploadedFilesListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteria);
                if (UploadedFiles != null && UploadedFiles.FirstOrDefault().Value != null && UploadedFiles.FirstOrDefault().Value.Count != 0)
                {
                    if (UploadedFiles.First().Value[0].OldFiles == 1)
                    {
                        string ImagePath = UploadedFiles.First().Value[0].FileDirectory + "\\" + UploadedFiles.First().Value[0].DocumentName;// "green.jpg"; ConfigurationManager.AppSettings["ImageFilePath1"] 

                        if (!System.IO.File.Exists(ImagePath))
                        {
                            ImagePath = ConfigurationManager.AppSettings["ImageFilePath1"] + UploadedFiles.First().Value[0].DocumentName;
                            DriverProf.DriverPhoto = ImagePath;
                        }
                        DriverProf.DriverPhoto = ImagePath + ".jpg";
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
                            DriverProf.DriverPhoto = File(doc.DocumentData, "image/jpg").ToString();
                            return File(doc.DocumentData, "image/jpg");
                        }
                        else
                        {
                            string ImagePath = ConfigurationManager.AppSettings["ImageFilePath"];
                            DriverProf.DriverPhoto = ImagePath + ".jpg";
                            return File(ImagePath, "image/jpg");
                        }
                    }
                }
                else
                {
                    var dir = Server.MapPath("/Images");
                    string ImagePath = ConfigurationManager.AppSettings["ImageFilePath"];
                    DriverProf.DriverPhoto = ImagePath + ".jpg";
                    return File(ImagePath, "image/jpg");
                }
            }
            catch (Exception ex)
            {

                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult AddVehicleDetails()
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
                    TransportService ts = new TransportService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<long, IList<VehicleTypeMaster>> Vehicleddl = ts.GetVehicleTypeMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                    ViewBag.VehicleTypeddl = Vehicleddl.First().Value;
                    Dictionary<long, IList<CampusMaster>> Campus = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                    ViewBag.campusddl = Campus.First().Value;
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult AddVehicleTypeDetails(string Status, int Id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService tsk = new TransportService();
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    VehicleSubTypeMaster DM = tsk.GetVehicleSubTypeMasterById(Id);
                    if (Status == "Registered")
                        DM.IsActive = true;
                    else if (Status == "Inactive")
                        DM.IsActive = false;
                    ViewBag.Status = Session["IsActive"];
                    tsk.CreateOrUpdateVehicleSubTypeMaster(DM);
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        #region CaptureImage Processing

        public ActionResult PhotoCapture(string PreRegNo)
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

                Session["photopath"] = path;

                Session["photoval"] = ConfigurationManager.AppSettings["ImageCropShow"] + date + "test.jpg";
            }
            return Json("", JsonRequestBehavior.AllowGet);
            //return View("CaptureImage");
        }

        public JsonResult Rebind()
        {
            string path = Session["photoval"].ToString();
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
            if (string.IsNullOrEmpty(Session["photopath"].ToString()) || !cropPointX.HasValue || !cropPointY.HasValue || !imageCropWidth.HasValue || !imageCropHeight.HasValue)
            {
                return new HttpStatusCodeResult((int)HttpStatusCode.BadRequest);
            }


            byte[] imageBytes = System.IO.File.ReadAllBytes(Session["photopath"].ToString());
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
            Session["photoCropedPath"] = Path.Combine(tempFolderName, fileName);
            string photoPath = ConfigurationManager.AppSettings["ImageCropShow"] + fileName;
            return Json(photoPath, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UploadCropedPhotos(string docType, string documentFor, long RegNo)
        {
            AdmissionManagementService ads = new AdmissionManagementService();
            //HttpPostedFileBase theFile = HttpContext.Request.Files["uploadedFile"];
            //string path = uploadedFile.InputStream.ToString();
            byte[] imageSize = System.IO.File.ReadAllBytes(Session["photoCropedPath"].ToString());
            //uploadedFile.InputStream.Read(imageSize, 0, (int)uploadedFile.ContentLength);
            UploadedFiles fu = new UploadedFiles();
            fu.DocumentFor = documentFor;
            fu.DocumentType = docType;
            fu.PreRegNum = RegNo;
            fu.DocumentData = imageSize;
            fu.DocumentName = Path.GetFileName(Session["photoCropedPath"].ToString());
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
        #region added by prabakaran for distance covered
        public ActionResult EditVehicleDistanceCovered(VehicleDistanceCovered vdc)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (vdc.Id > 0)
                    {
                        TransportService ts = new TransportService();
                        IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                        //if (!string.IsNullOrWhiteSpace(Request["OutDateTime"]))
                        //{
                        //    vdc.OutDateTime = DateTime.Parse(Request["OutDateTime"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        //}

                        VehicleDistanceCovered vdcovered = ts.GetVehicleDistanceCoveredById(vdc.Id);
                        // vdc.TripDate = DateTime.Now;
                        if (!string.IsNullOrWhiteSpace(Request["InDateTime"]))
                        {
                            vdcovered.InDateTime = DateTime.Parse(Request["InDateTime"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        }
                        if (vdc.DistanceCovered == 0)
                            vdcovered.Status = "Open";
                        else
                            vdcovered.Status = "Completed";
                        vdcovered.KMIn = vdc.KMIn;
                        vdcovered.KMOut = vdc.KMOut;
                        vdcovered.KMResetValue = vdc.KMResetValue;
                        vdcovered.IsKMReseted = vdc.IsKMReseted;
                        vdcovered.DistanceCovered = vdc.DistanceCovered;
                        ts.CreateOrUpdateVehicleDistanceCovered(vdcovered);
                    }
                    //TripPurposeMaster tpm = ts.GetPurposeByPurpose(vdc.Purpose);
                    //if (tpm == null)
                    //{
                    //    tpm = new TripPurposeMaster();
                    //    tpm.Purpose = vdc.Purpose;
                    //    ts.CreateOrUpdateTripPurposeMaster(tpm);
                    //}
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult GetVehicleReadingByVehcileNo(string Campus, string VehicleNo)
        {
            try
            {
                TransportService ts = new TransportService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Campus", Campus);
                criteria.Add("VehicleNo", VehicleNo);
                Dictionary<long, IList<VehicleDistanceCovered_vw>> VehicleDistanceCoveredList = ts.GetVehicleDistanceCovered_vw(0, 9999, "Id", "Desc", criteria);
                if (VehicleDistanceCoveredList != null && VehicleDistanceCoveredList.FirstOrDefault().Value != null && VehicleDistanceCoveredList.FirstOrDefault().Key > 0)
                {
                    return Json(VehicleDistanceCoveredList.FirstOrDefault().Value[0], JsonRequestBehavior.AllowGet);
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public JsonResult GetVehicleNoByStatus(string term, string Campus)
        {
            try
            {
                TransportService ts = new TransportService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("VehicleNo", term);
                if (!string.IsNullOrEmpty(Campus))
                    criteria.Add("Campus", Campus);
                Dictionary<long, IList<VehicleSubTypeMaster>> VehicleList = ts.GetVehicleSubTypeMasterListWithPagingAndCriteriaLikeSearch(0, 9999, string.Empty, string.Empty, criteria);
                criteria.Add("Status", "Open");
                Dictionary<long, IList<VehicleDistanceCovered>> vdc = ts.GetVehicleDistanceCoveredListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                var VehilceNos = (from u in VehicleList.First().Value
                                  where u.VehicleNo != null && u.VehicleNo != ""
                                  select u.VehicleNo).Distinct().ToList();
                var VehilceNo = (from u in vdc.First().Value
                                 where u.VehicleNo != null && u.VehicleNo != ""
                                 select u.VehicleNo).Distinct().ToList();
                var vehiclenolist = VehilceNos.Except(VehilceNo).ToArray();
                return Json(vehiclenolist, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        #endregion        
        #region Added by john naveen in TripMaster
        public ActionResult TripDetails()
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
        public ActionResult AddTripMasterDetails(TripMaster tm)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    var script = "";
                    TripMaster tripdetails = ts.GetTripMasterDetailsByTripName(tm.TripName);
                    if (tripdetails == null)
                    {

                        TransportBC lmrepo = new TransportBC();
                        tm.CreatedDate = DateTime.Now;
                        tm.CreatedBy = userId;
                        tm.ModifiedDate = DateTime.Now;
                        tm.ModifiedBy = userId;
                        lmrepo.SaveOrUpdateTripMasterDetails(tm);
                        return JavaScript(script);
                    }
                    else
                    {
                        script = @"ErrMsg(""The Name is already exist!"");";
                        return JavaScript(script);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult EditTripMasterDetails(TripMaster tm)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    if (tm.TripId > 0)
                    {
                        var script = "";
                        TripMaster tripdetails = ts.GetTripMasterDetailsByTripName(tm.TripName);
                        if (tripdetails != null)
                        {
                            script = @"ErrMsg(""The Name is already exist!"");";
                            return JavaScript(script);
                        }
                        else
                        {
                            TripMaster trpm = new TripMaster();
                            trpm = ts.GetTripMasterDetailsById(tm.TripId);
                            trpm.TripName = tm.TripName;
                            trpm.ModifiedBy = userId;
                            trpm.ModifiedDate = DateTime.Now;
                            ts.SaveOrUpdateTripMasterDetails(trpm);
                        }

                    }
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult DeleteTripMasterDetails(string[] Id)
        {
            try
            {
                TransportBC lmrepo = new TransportBC();
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
                    lmrepo.DeleteTripMasterDetails(longCityIdArray);
                    var script = @"SucessMsg(""Deleted Successfully"");";
                    return JavaScript(script);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public ActionResult TripMasterJqGrid(TripMaster tm, string TripName, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportBC samp = new TransportBC();
                    UserService us = new UserService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    string[] Criteria = new string[8];
                    if (tm.TripId > 0) { criteria.Add("TripId", tm.TripId); }
                    if (!string.IsNullOrWhiteSpace(tm.TripName)) { criteria.Add("TripName", tm.TripName); }
                    if (!string.IsNullOrWhiteSpace(tm.CreatedBy)) { criteria.Add("CreatedBy", tm.CreatedBy); }
                    if (!string.IsNullOrWhiteSpace(tm.ModifiedBy)) { criteria.Add("ModifiedBy", tm.ModifiedBy); }
                    Dictionary<long, IList<TripMaster>> TripMasterList = samp.GetTripMasterDetails(page - 1, rows, sidx, sord, criteria);
                    if (TripMasterList != null && TripMasterList.First().Key > 0)
                    {
                        long totalrecords = TripMasterList.FirstOrDefault().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var AssLst = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,

                            rows = (
                                 from items in TripMasterList.First().Value
                                 select new
                                 {
                                     cell = new string[] 
                                         {
                                            items.TripId.ToString(),
                                             items.TripName,
                                             items.CreatedDate!=null? items.CreatedDate.ToString("dd/MM/yyyy"):null,
                                            items.CreatedBy!=null?us.GetUserNameByUserId(items.CreatedBy):"",
                                            items.ModifiedDate!=null? items.ModifiedDate.ToString("dd/MM/yyyy"):null,
                                            items.ModifiedBy,
                                            items.CreatedBy,
                                            items.IsActive.ToString(),
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
                var Empty = new { rows = (new { cell = new string[] { } }) };
                return Json(Empty, JsonRequestBehavior.AllowGet);

            }



            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        #endregion
        #region vehicle details list by john naveen
        public ActionResult VehicleDetailList()
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

        //public ActionResult VehicleDetailsAdd(int? VehicleId)
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //        else
        //        {
        //            TransportService ts = new TransportService();
        //            VehicleDetails vd = new VehicleDetails();
        //            VehicleSubTypeMaster vstm = new VehicleSubTypeMaster();



        //            if (VehicleId > 0)
        //            {
        //                vstm = ts.GetVehicleSubTypeMasterById(Convert.ToInt32(VehicleId));
        //            }
        //            vd.Id = vstm.Id;
        //            vd.Campus = vstm.Campus;
        //            vd.Type = vstm.Type;
        //            vd.VehicleNo = vstm.VehicleNo;
        //            vd.Purpose = vstm.Purpose;
        //            vd.FuelType = vstm.FuelType;

        //            Dictionary<string, object> criteria = new Dictionary<string, object>();
        //            criteria.Add("Rank", Convert.ToInt64(1));
        //            //long VId = Convert.ToInt64(VehicleId);                    
        //            criteria.Add("VehicleId", Convert.ToInt64(VehicleId));
        //            Dictionary<long, IList<VehicleCostDetails_VW>> VehicleCostDetails_VW = ts.GetVehicleCostDetails_VWListWithPagingAndCriteriaLikeSearch(0, 9999, null, null, criteria);
        //            if (VehicleCostDetails_VW != null && VehicleCostDetails_VW.FirstOrDefault().Key > 0)
        //            {
        //                ViewBag.StartKmrs = VehicleCostDetails_VW.First().Value[0].EndKmrs.ToString();
        //            }
        //            else
        //            {
        //                ViewBag.StartKmrs = 0;
        //            }

        //            return View(vd);


        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
        //        throw ex;
        //    }
        //}

        public ActionResult VehicleDetailsAdd(int? VehicleId)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    VehicleDetails vd = new VehicleDetails();
                    VehicleSubTypeMaster vstm = new VehicleSubTypeMaster();



                    if (VehicleId > 0)
                    {
                        vstm = ts.GetVehicleSubTypeMasterById(Convert.ToInt32(VehicleId));
                    }
                    vd.Id = vstm.Id;
                    vd.Campus = vstm.Campus;
                    vd.Type = vstm.Type;
                    vd.VehicleNo = vstm.VehicleNo;
                    vd.Purpose = vstm.Purpose;
                    vd.FuelType = vstm.FuelType;

                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("Rank", Convert.ToInt64(1));
                    //long VId = Convert.ToInt64(VehicleId);                    
                    criteria.Add("VehicleId", Convert.ToInt64(VehicleId));
                    Dictionary<long, IList<VehicleCostDetails_Updated_VW>> VehicleCostDetails_VW = ts.GetVehicleCostDetails_Updated_VWListWithPagingAndCriteriaLikeSearch(0, 9999, null, null, criteria);
                    if (VehicleCostDetails_VW != null && VehicleCostDetails_VW.FirstOrDefault().Key > 0)
                    {
                        ViewBag.StartKmrs = VehicleCostDetails_VW.First().Value[0].EndKmrs.ToString();
                    }
                    else
                    {
                        ViewBag.StartKmrs = 0;
                    }
                    return View(vd);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult VehicleCostDetails(int? VehicleId)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    VehicleDetails vd = new VehicleDetails();
                    VehicleSubTypeMaster vstm = new VehicleSubTypeMaster();

                    if (VehicleId > 0)
                    {
                        vstm = ts.GetVehicleSubTypeMasterById(Convert.ToInt32(VehicleId));
                    }
                    vd.Id = vstm.Id;
                    vd.Campus = vstm.Campus;
                    vd.Type = vstm.Type;
                    vd.VehicleNo = vstm.VehicleNo;
                    vd.Purpose = vstm.Purpose;
                    vd.FuelType = vstm.FuelType;
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<long, IList<LocationMaster>> LocationMaster = ts.GetLocationMasterDetails(0, 9999, "LocationName", "Asc", criteria);
                    Dictionary<long, IList<DriverMaster>> DriverMaster = ts.GetDriverMasterDetails(0, 9999, "Name", "Asc", criteria);
                    ViewBag.LocationMaster = LocationMaster.First().Value;
                    // ViewBag.DriverMaster =  DriverMaster.First().Value.TakeWhile( from items in DriverMaster.FirstOrDefault().Value where items.Name!=null select items.Name);
                    ViewBag.DriverMaster = (from items in DriverMaster.FirstOrDefault().Value
                                            where items.Name != null && items.Name != ""
                                            select items).Distinct().ToList();
                    return View(vd);
                }
            }

            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult VehicleCostDetailsJqGrid(VehicleCostDetails vsd, int VehicleId, string ExprtToExcel, int rows, string sidx, string sord, int? page = 1, int? ExptXl = 0)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportBC samp = new TransportBC();
                    TransportService ts = new TransportService();
                    UserService us = new UserService();
                    VehicleDetails vd = new VehicleDetails();
                    VehicleSubTypeMaster vstm = new VehicleSubTypeMaster();
                    TIPS.Entities.TransportEntities.DriverMaster dm = new TIPS.Entities.TransportEntities.DriverMaster();
                    TIPS.Entities.StaffManagementEntities.StaffDetails sd = new TIPS.Entities.StaffManagementEntities.StaffDetails();
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    Dictionary<string, object> exctcriteria = new Dictionary<string, object>();
                    Dictionary<string, object> likeCriteria = new Dictionary<string, object>();
                    string[] Criteria = new string[8];
                    sord = sord == "asc" ? "Desc" : "Asc";
                    vstm = ts.GetVehicleSubTypeMasterById(Convert.ToInt32(VehicleId));
                    if (!string.IsNullOrEmpty(vsd.VehicleNo)) { likeCriteria.Add("VehicleNo", vsd.VehicleNo); }
                    if (!string.IsNullOrEmpty(vsd.Campus)) { likeCriteria.Add("Campus", vsd.Campus); }
                    if (!string.IsNullOrEmpty(vsd.TypeOfTrip)) { likeCriteria.Add("TypeOfTrip", vsd.TypeOfTrip); }
                    if (!string.IsNullOrEmpty(vsd.VehicleRoute)) { likeCriteria.Add("VehicleRoute", vsd.VehicleRoute); }
                    if (vsd.StartKmrs > 0) { likeCriteria.Add("StartKmrs", vsd.StartKmrs); }
                    if (vsd.EndKmrs > 0) { likeCriteria.Add("EndKmrs", vsd.EndKmrs); }
                    if (vsd.DriverOt > 0) { likeCriteria.Add("DriverOt", vsd.DriverOt); }
                    if (vsd.HelperOt > 0) { likeCriteria.Add("HelperOt", vsd.HelperOt); }
                    if (vsd.Diesel > 0) { likeCriteria.Add("Diesel", vsd.Diesel); }
                    if (vsd.Maintenance > 0) { exctcriteria.Add("Maintenance", vsd.Maintenance); }
                    if (vsd.VehicleId > 0) { exctcriteria.Add("VehicleId", vsd.VehicleId); }
                    Dictionary<long, IList<VehicleCostDetails>> VehicleCostList = ts.VehicleCostDetailsListWithLikeAndExcactSerachCriteria(page - 1, rows, sord, sidx, exctcriteria, likeCriteria);

                    if (VehicleCostList == null || VehicleCostList.FirstOrDefault().Key == 0)
                    {
                        var Empty = new { rows = (new { cell = new string[] { } }) };
                        return Json(Empty, JsonRequestBehavior.AllowGet);
                    }


                    long totalRecords = VehicleCostList.FirstOrDefault().Value.Count;
                    int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);

                    var jsonData = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalRecords,
                        userdata = new
                        {
                            Distance = VehicleCostList.FirstOrDefault().Value.Sum(x => x.Distance).ToString(),
                            DriverOt = VehicleCostList.FirstOrDefault().Value.Sum(x => x.DriverOt).ToString(),
                            HelperOt = VehicleCostList.FirstOrDefault().Value.Sum(x => x.HelperOt).ToString(),
                            Diesel = VehicleCostList.FirstOrDefault().Value.Sum(x => x.Diesel).ToString(),
                            Maintenance = VehicleCostList.FirstOrDefault().Value.Sum(x => x.Maintenance).ToString(),
                            Service = VehicleCostList.FirstOrDefault().Value.Sum(x => x.Service).ToString(),
                            FC = VehicleCostList.FirstOrDefault().Value.Sum(x => x.FC).ToString(),
                            Others = VehicleCostList.FirstOrDefault().Value.Sum(x => x.Others).ToString()
                        },
                        rows =
                        (from items in VehicleCostList.FirstOrDefault().Value
                         select new
                         {
                             i = items.VehicleId.ToString(),
                             cell = new string[]
                           {

                      
                                  items.VehicleCostId.ToString(),
                                  vstm.Id.ToString(),
                                  items.VehicleTravelDate!=null?items.VehicleTravelDate.ToString("dd/MM/yyyy"):null,
                                  vstm.Campus,
                                  items.TypeOfTrip,
                                  vstm.VehicleNo,
                                  items.DriverMaster!=null?items.DriverMaster.Name:"",
                                  items.StaffDetails!=null?items.StaffDetails.Name:"",
                                  items.VehicleRoute,
                                  items.StartKmrs.ToString(),
                                  items.EndKmrs.ToString(),
                                  items.Distance.ToString(),
                                  items.DriverOt.ToString(),
                                  items.HelperOt.ToString(),
                                  items.Diesel.ToString(),
                                  items.Maintenance.ToString(), 
                                  items.Service.ToString(),
                                  items.FC.ToString(),
                                  items.Others.ToString(),
                                  items.CreatedBy!=null?us.GetUserNameByUserId(items.CreatedBy):"",
                                  items.CreatedDate!=null? items.CreatedDate.ToString("dd/MM/yyyy"):null,
                                   items.ModifiedBy,
                                  items.ModifiedDate!=null? items.ModifiedDate.ToString("dd/MM/yyyy"):null,
                          }
                         })
                    };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult SaveOrUpdateVehicleCostDetails(VehicleCostDetails vcd, long DriverMasterId)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    var data = string.Empty;
                    TransportBC lmrepo = new TransportBC();
                    TransportService ts = new TransportService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<string, object> likeCriteria = new Dictionary<string, object>();
                    VehicleCostDetails vcdl = new VehicleCostDetails();
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    vcd.VehicleTravelDate = DateTime.Parse(Request["VehicleTravelDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    var VehicleDate = vcd.VehicleTravelDate.AddDays(-1);
                    criteria.Add("VehicleTravelDate", VehicleDate);
                    criteria.Add("TypeOfTrip", vcd.TypeOfTrip);
                    criteria.Add("VehicleId", vcd.VehicleId);
                    criteria.Add("HelperId", vcd.HelperId);
                    VehicleCostDetails vehicleCostDetails = new VehicleCostDetails();
                    vehicleCostDetails = ts.GetVehicleCostDetailsByTravelDate(vcd.VehicleId, VehicleDate);
                    vcdl = ts.GetVehicleCostDetailsByTripName(vcd.VehicleId, vcd.TypeOfTrip, vcd.VehicleTravelDate);
                    var script = "";
                    if (vehicleCostDetails != null)
                    {
                        data = "Success";
                        if (vcd.VehicleCostId == 0)
                        {
                            if (vcdl == null)
                            {
                                DriverMaster dm = new DriverMaster();
                                dm.Id = DriverMasterId;
                                vcd.Distance = vcd.EndKmrs - vcd.StartKmrs;
                                vcd.CreatedBy = userId;
                                vcd.CreatedDate = DateTime.Now;
                                vcd.ModifiedBy = userId;
                                vcd.ModifiedDate = DateTime.Now;
                                vcd.DriverMaster = dm;
                                ts.SaveOrUpdateVehicleCostDetails(vcd);
                                //script = @"SucessMsg(""Added Sucessfully"");";
                                //return JavaScript(script);
                                var jsondata = new
                                {
                                    EndKmrs = vcd.EndKmrs,
                                    statusval = "success"
                                };
                                return Json(jsondata, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                var jsondata = new
                                {

                                    statusval = "exist"
                                };
                                return Json(jsondata, JsonRequestBehavior.AllowGet);

                            }
                        }
                        else
                        {
                            vcd.CreatedBy = userId;
                            vcd.CreatedDate = DateTime.Now;
                            vcd.ModifiedBy = userId;
                            vcd.ModifiedDate = DateTime.Now;

                            ts.SaveOrUpdateVehicleCostDetails(vcd);

                            return JavaScript(script);
                        }

                    }
                    else
                    {
                        var jsondata = new
                        {

                            statusval = "failed"
                        };
                        return Json(jsondata, JsonRequestBehavior.AllowGet);

                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "MasterPolicy");
                throw ex;
            }
        }
        public ActionResult SaveVehicleCostDetails(VehicleCostDetails vcd, long DriverMasterId)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportBC lmrepo = new TransportBC();
                    TransportService ts = new TransportService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<string, object> likeCriteria = new Dictionary<string, object>();
                    VehicleCostDetails vcdl = new VehicleCostDetails();
                    criteria.Add("HelperId", vcd.HelperId);
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    vcd.VehicleTravelDate = DateTime.Parse(Request["VehicleTravelDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    vcdl = ts.GetVehicleCostDetailsByTripName(vcd.VehicleId, vcd.TypeOfTrip, vcd.VehicleTravelDate);
                    var script = "";

                    if (vcdl == null)
                    {
                        DriverMaster dm = new DriverMaster();
                        dm.Id = DriverMasterId;
                        vcd.Distance = vcd.EndKmrs - vcd.StartKmrs;
                        vcd.CreatedBy = userId;
                        vcd.CreatedDate = DateTime.Now;
                        vcd.ModifiedBy = userId;
                        vcd.ModifiedDate = DateTime.Now;
                        vcd.DriverMaster = dm;
                        ts.SaveOrUpdateVehicleCostDetails(vcd);
                        //script = @"SucessMsg(""Added Sucessfully"");";
                        //return JavaScript(script);
                        var jsondata = new
                        {
                            EndKmrs = vcd.EndKmrs,
                            statusval = "success"
                        };
                        return Json(jsondata, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var jsondata = new
                        {

                            statusval = "exist"
                        };
                        return Json(jsondata, JsonRequestBehavior.AllowGet);

                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult TripTypeddl()
        {
            try
            {
                MastersService ms = new MastersService();
                Dictionary<string, string> Cmp = new Dictionary<string, string>();

                Dictionary<string, object> criteria = new Dictionary<string, object>();
                TransportBC lmrepo = new TransportBC();
                Dictionary<long, IList<TripMaster>> trpMstr = lmrepo.GetTripMasterDetails(null, null, null, null, criteria);
                if (trpMstr != null && trpMstr.First().Value != null && trpMstr.First().Value.Count > 0)
                {
                    var tripList = (
                             from items in trpMstr.First().Value
                             select new
                             {
                                 Text = items.TripName,
                                 Value = items.TripName
                             }).Distinct().ToList().OrderBy(x => x.Text);
                    return Json(tripList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                // ExceptionPolicy.HandleException(ex, "Policy");
                throw ex;
            }
        }
        public ActionResult GetDriverName(string term)
        {
            try
            {
                TransportService ts = new TransportService();
                Dictionary<string, object> Criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(term)) { Criteria.Add("Name", term); }
                Dictionary<long, IList<DriverMaster>> DriverList = ts.GetDriverMasterDetails(0, 9999, null, null, Criteria);

                //if (VendorsList != null && VendorsList.FirstOrDefault().Key > 0)
                //{
                var DriverNameList = (from u in DriverList.FirstOrDefault().Value
                                      select new { Name = u.Name, Id = u.Id }).Distinct().ToList();
                return Json(DriverNameList, JsonRequestBehavior.AllowGet);



            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public ActionResult GetVehicleRoute(string RouteNo)
        {
            try
            {
                TransportService ts = new TransportService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(RouteNo)) { criteria.Add("RouteNo", RouteNo); }
                Dictionary<long, IList<RouteMaster>> RouteList = ts.GetRouteMasterDetailsByListWithLikeSearchCriteriaCount(0, 9999, null, null, criteria);
                if (RouteList != null && RouteList.FirstOrDefault().Key > 0)
                {
                    var VehicleRouteList = (from u in RouteList.FirstOrDefault().Value
                                            select u.RouteNo).Distinct().ToList();
                    return Json(VehicleRouteList, JsonRequestBehavior.AllowGet);
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

        public IList<VehicleCostDetails_Updated> VehicleCostDetailsReportWithCriteria(long VehicleId, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                TransportService ts = new TransportService();
                Dictionary<string, object> exctcriteria = new Dictionary<string, object>();
                Dictionary<string, object> likeCriteria = new Dictionary<string, object>();
                exctcriteria.Add("VehicleId", VehicleId);
                Dictionary<long, IList<VehicleCostDetails_Updated>> VehicleCostList = ts.VehicleCostDetails_UpdatedListWithLikeAndExcactSerachCriteria(page - 1, rows, sidx, sord, exctcriteria, likeCriteria);
                if (VehicleCostList != null && VehicleCostList.FirstOrDefault().Key > 0)
                {
                    return VehicleCostList.FirstOrDefault().Value.ToList();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw;
            }
        }
        //public void ExportToExcelVCD(long VehicleId, int rows, string sidx, string sord, int? page = 1)
        //{
        //    try
        //    {
        //        ExcelPackage objExcelPackage = new ExcelPackage();   //create new workbook
        //        int count = 0;
        //        ExcelWorksheet ews = objExcelPackage.Workbook.Worksheets.Add("Worksheet" + count); //create new worksheet
        //        ews.View.ZoomScale = 100;
        //        ews.View.ShowGridLines = true;
        //        ews.Cells["A3:C3"].Merge = true;
        //        ews.Cells["A3:C3"].Value = "VehicleNo";
        //        ews.Cells["A3:C3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        ews.Cells["D3:F3"].Merge = true;
        //        ews.Cells["D3:F3"].Value = "Campus";
        //        ews.Cells["D3:F3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        ews.Cells["G3:I3"].Merge = true;
        //        ews.Cells["G3:I3"].Value = "Type";
        //        ews.Cells["G3:I3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        ews.Cells["A4:B4"].Merge = true;
        //        ews.Cells["C4:D4"].Merge = true;
        //        ews.Cells["E4:F4"].Merge = true;
        //        ews.Cells["A5:A6"].Merge = true;
        //        ews.Cells["A5:A6"].Value = "Date";
        //        ews.Cells["B5:K5"].Merge = true;
        //        ews.Cells["B5:K5"].Value = "In Rupuees";
        //        ews.Cells["B5:K5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        ews.Cells["B6"].Value = "Start Kms";
        //        ews.Cells["B6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        ews.Cells["C6"].Value = "End Kms";
        //        ews.Cells["C6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        ews.Cells["D6"].Value = "Trip Kms";
        //        ews.Cells["D6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        ews.Cells["E6"].Value = "Driver OT";
        //        ews.Cells["E6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        ews.Cells["F6"].Value = "Helper OT";
        //        ews.Cells["F6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        ews.Cells["G6"].Value = "Diesel";
        //        ews.Cells["G6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        ews.Cells["H6"].Value = "Maintenance";
        //        ews.Cells["H6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        ews.Cells["I6"].Value = "Service";
        //        ews.Cells["I6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        ews.Cells["J6"].Value = "Fc";
        //        ews.Cells["J6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        ews.Cells["K6"].Value = "Others";
        //        ews.Cells["K6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        string SearchCriteria = "";
        //        ews.Cells["A1:K1"].Merge = true;
        //        ews.Cells["A2:K2"].Merge = true;
        //        ews.Cells["A2:K2"].Value = SearchCriteria;
        //        ews.Cells["A1:K1"].Value = "COST SHEET";
        //        ews.Cells["A1:K1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        ews.Cells["A2:K2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        ews.Cells["A1:K1"].Style.Font.Bold = true;
        //        ews.Cells["A2:K2"].Style.Font.Bold = true;
        //        ews.Cells["A3:K3"].Style.Font.Bold = true;
        //        ews.Cells["A5:A6"].Style.Font.Bold = true;
        //        ews.Cells["A6"].Style.Font.Bold = true;
        //        ews.Cells["B6"].Style.Font.Bold = true;
        //        ews.Cells["C6"].Style.Font.Bold = true;
        //        ews.Cells["D6"].Style.Font.Bold = true;
        //        ews.Cells["E6"].Style.Font.Bold = true;
        //        ews.Cells["F6"].Style.Font.Bold = true;
        //        ews.Cells["G6"].Style.Font.Bold = true;
        //        ews.Cells["H6"].Style.Font.Bold = true;
        //        ews.Cells["I6"].Style.Font.Bold = true;
        //        ews.Cells["J6"].Style.Font.Bold = true;
        //        ews.Cells["K6"].Style.Font.Bold = true;
        //        int i = 7;
        //        int rowIndex = 3;
        //        int columnIndex = 13;
        //        long Distance = 0;
        //        decimal DriverOt = 0;
        //        decimal HelperOt = 0;
        //        decimal Diesel = 0;
        //        decimal Maintenance = 0;
        //        decimal Service = 0;
        //        decimal FC = 0;
        //        decimal Others = 0;
        //        DailyUsageVehicleMaster vstm = new DailyUsageVehicleMaster();
        //        Dictionary<string, object> Criteria = new Dictionary<string, object>();
        //        Criteria.Add("VehicleId", VehicleId);
        //        IList<VehicleCostDetails_Updated> VehicleCostDetailsList = new List<VehicleCostDetails_Updated>();
        //        VehicleCostDetailsList = VehicleCostDetailsReportWithCriteria(VehicleId, rows, sidx, sord, page);
        //        if (VehicleCostDetailsList.Count > 0)
        //        {
        //            for (int k = 0; k < VehicleCostDetailsList.Count; k++)
        //            {
        //                ews.Cells["A" + i].Value = VehicleCostDetailsList[k].VehicleTravelDate.ToString("dd/MM/yyyy");
        //                ews.Cells["B" + i].Value = VehicleCostDetailsList[k].StartKmrs;
        //                ews.Cells["C" + i].Value = VehicleCostDetailsList[k].EndKmrs;
        //                ews.Cells["D" + i].Value = VehicleCostDetailsList[k].Distance;
        //                ews.Cells["E" + i].Value = VehicleCostDetailsList[k].DriverOt;
        //                ews.Cells["F" + i].Value = VehicleCostDetailsList[k].HelperOt;
        //                ews.Cells["G" + i].Value = VehicleCostDetailsList[k].Diesel;
        //                ews.Cells["H" + i].Value = VehicleCostDetailsList[k].Maintenance;
        //                ews.Cells["I" + i].Value = VehicleCostDetailsList[k].Service;
        //                ews.Cells["J" + i].Value = VehicleCostDetailsList[k].FC;
        //                ews.Cells["K" + i].Value = VehicleCostDetailsList[k].Others;
        //                Distance = Distance + VehicleCostDetailsList[k].Distance;
        //                DriverOt = DriverOt + VehicleCostDetailsList[k].DriverOt;
        //                HelperOt = HelperOt + VehicleCostDetailsList[k].HelperOt;
        //                Diesel = Diesel + VehicleCostDetailsList[k].Diesel;
        //                Maintenance = Maintenance + VehicleCostDetailsList[k].Maintenance;
        //                Service = Service + VehicleCostDetailsList[k].Service;
        //                FC = FC + VehicleCostDetailsList[k].FC;
        //                Others = Others + VehicleCostDetailsList[k].Others;
        //                count++;
        //                rowIndex++;
        //                i = i + 1;
        //            }
        //        }
        //        ews.Cells["A" + i].Value = "Total";
        //        ews.Cells["A" + i].Style.Font.Bold = true;
        //        ews.Cells["D" + i].Value = Distance;
        //        ews.Cells["D" + i].Style.Font.Bold = true;
        //        ews.Cells["E" + i].Value = DriverOt;
        //        ews.Cells["E" + i].Style.Font.Bold = true;
        //        ews.Cells["F" + i].Value = HelperOt;
        //        ews.Cells["F" + i].Style.Font.Bold = true;
        //        ews.Cells["G" + i].Value = Diesel;
        //        ews.Cells["G" + i].Style.Font.Bold = true;
        //        ews.Cells["H" + i].Value = Maintenance;
        //        ews.Cells["H" + i].Style.Font.Bold = true;
        //        ews.Cells["I" + i].Value = Service;
        //        ews.Cells["I" + i].Style.Font.Bold = true;
        //        ews.Cells["J" + i].Value = FC;
        //        ews.Cells["J" + i].Style.Font.Bold = true;
        //        ews.Cells["K" + i].Value = Others;
        //        ews.Cells["K" + i].Style.Font.Bold = true;
        //        ews.Cells[ews.Dimension.Address].AutoFitColumns();
        //        string Todaydate = DateTime.Now.ToString("dd/MM/yyyy");
        //        string FileName = "VehicleCostDetailsReport" + "-On-" + Todaydate; ;
        //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        Response.AddHeader("content-disposition", "attachment;  filename=" + FileName + ".xlsx");
        //        byte[] File = objExcelPackage.GetAsByteArray();
        //        Response.BinaryWrite(File);
        //        Response.End();
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
        //        throw;
        //    }
        //}

        public void ExportToExcelVCD(long VehicleId, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                ExcelPackage objExcelPackage = new ExcelPackage();   //create new workbook
                int count = 0;
                ExcelWorksheet ews = objExcelPackage.Workbook.Worksheets.Add("Worksheet" + count); //create new worksheet
                ews.View.ZoomScale = 100;
                ews.View.ShowGridLines = true;
                ews.Cells["A3:C3"].Merge = true;
                ews.Cells["A3:C3"].Value = "VehicleNo";
                ews.Cells["A3:C3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ews.Cells["D3:F3"].Merge = true;
                ews.Cells["D3:F3"].Value = "Campus";
                ews.Cells["D3:F3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ews.Cells["G3:I3"].Merge = true;
                ews.Cells["G3:I3"].Value = "Type";
                ews.Cells["G3:I3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ews.Cells["A4:B4"].Merge = true;
                ews.Cells["C4:D4"].Merge = true;
                ews.Cells["E4:F4"].Merge = true;
                ews.Cells["A5:A6"].Merge = true;
                ews.Cells["A5:A6"].Value = "Date";
                ews.Cells["B5:K5"].Merge = true;
                ews.Cells["B5:K5"].Value = "In Rupuees";
                ews.Cells["B5:K5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ews.Cells["B6"].Value = "Start Kms";
                ews.Cells["B6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ews.Cells["C6"].Value = "End Kms";
                ews.Cells["C6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ews.Cells["D6"].Value = "Trip Kms";
                ews.Cells["D6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ews.Cells["E6"].Value = "Driver OT";
                ews.Cells["E6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ews.Cells["F6"].Value = "Helper OT";
                ews.Cells["F6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ews.Cells["G6"].Value = "Diesel";
                ews.Cells["G6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ews.Cells["H6"].Value = "Maintenance";
                ews.Cells["H6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ews.Cells["I6"].Value = "Service";
                ews.Cells["I6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ews.Cells["J6"].Value = "Fc";
                ews.Cells["J6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ews.Cells["K6"].Value = "Others";
                ews.Cells["K6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                string SearchCriteria = "";
                ews.Cells["A1:K1"].Merge = true;
                ews.Cells["A2:K2"].Merge = true;
                ews.Cells["A2:K2"].Value = SearchCriteria;
                ews.Cells["A1:K1"].Value = "COST SHEET";
                ews.Cells["A1:K1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ews.Cells["A2:K2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ews.Cells["A1:K1"].Style.Font.Bold = true;
                ews.Cells["A2:K2"].Style.Font.Bold = true;
                ews.Cells["A3:K3"].Style.Font.Bold = true;
                ews.Cells["A5:A6"].Style.Font.Bold = true;
                ews.Cells["A6"].Style.Font.Bold = true;
                ews.Cells["B6"].Style.Font.Bold = true;
                ews.Cells["C6"].Style.Font.Bold = true;
                ews.Cells["D6"].Style.Font.Bold = true;
                ews.Cells["E6"].Style.Font.Bold = true;
                ews.Cells["F6"].Style.Font.Bold = true;
                ews.Cells["G6"].Style.Font.Bold = true;
                ews.Cells["H6"].Style.Font.Bold = true;
                ews.Cells["I6"].Style.Font.Bold = true;
                ews.Cells["J6"].Style.Font.Bold = true;
                ews.Cells["K6"].Style.Font.Bold = true;
                int i = 7;
                int rowIndex = 3;
                int columnIndex = 13;
                long Distance = 0;
                decimal DriverOt = 0;
                decimal HelperOt = 0;
                decimal Diesel = 0;
                decimal Maintenance = 0;
                decimal Service = 0;
                decimal FC = 0;
                decimal Others = 0;
                DailyUsageVehicleMaster vstm = new DailyUsageVehicleMaster();
                Dictionary<string, object> Criteria = new Dictionary<string, object>();
                Criteria.Add("VehicleId", VehicleId);
                IList<VehicleCostDetails_Updated> VehicleCostDetailsList = new List<VehicleCostDetails_Updated>();
                VehicleCostDetailsList = VehicleCostDetailsReportWithCriteria(VehicleId, rows, sidx, sord, page);
                if (VehicleCostDetailsList != null && VehicleCostDetailsList.Count > 0)
                {
                    for (int k = 0; k < VehicleCostDetailsList.Count; k++)
                    {
                        ews.Cells["A" + i].Value = VehicleCostDetailsList[k].VehicleTravelDate.ToString("dd/MM/yyyy");
                        ews.Cells["B" + i].Value = VehicleCostDetailsList[k].StartKmrs;
                        ews.Cells["C" + i].Value = VehicleCostDetailsList[k].EndKmrs;
                        ews.Cells["D" + i].Value = VehicleCostDetailsList[k].Distance;
                        ews.Cells["E" + i].Value = VehicleCostDetailsList[k].DriverOt;
                        ews.Cells["F" + i].Value = VehicleCostDetailsList[k].HelperOt;
                        ews.Cells["G" + i].Value = VehicleCostDetailsList[k].Diesel;
                        ews.Cells["H" + i].Value = VehicleCostDetailsList[k].Maintenance;
                        ews.Cells["I" + i].Value = VehicleCostDetailsList[k].Service;
                        ews.Cells["J" + i].Value = VehicleCostDetailsList[k].FC;
                        ews.Cells["K" + i].Value = VehicleCostDetailsList[k].Others;
                        Distance = Distance + VehicleCostDetailsList[k].Distance;
                        DriverOt = DriverOt + VehicleCostDetailsList[k].DriverOt;
                        HelperOt = HelperOt + VehicleCostDetailsList[k].HelperOt;
                        Diesel = Diesel + VehicleCostDetailsList[k].Diesel;
                        Maintenance = Maintenance + VehicleCostDetailsList[k].Maintenance;
                        Service = Service + VehicleCostDetailsList[k].Service;
                        FC = FC + VehicleCostDetailsList[k].FC;
                        Others = Others + VehicleCostDetailsList[k].Others;
                        count++;
                        rowIndex++;
                        i = i + 1;
                    }
                }
                ews.Cells["A" + i].Value = "Total";
                ews.Cells["A" + i].Style.Font.Bold = true;
                ews.Cells["D" + i].Value = Distance;
                ews.Cells["D" + i].Style.Font.Bold = true;
                ews.Cells["E" + i].Value = DriverOt;
                ews.Cells["E" + i].Style.Font.Bold = true;
                ews.Cells["F" + i].Value = HelperOt;
                ews.Cells["F" + i].Style.Font.Bold = true;
                ews.Cells["G" + i].Value = Diesel;
                ews.Cells["G" + i].Style.Font.Bold = true;
                ews.Cells["H" + i].Value = Maintenance;
                ews.Cells["H" + i].Style.Font.Bold = true;
                ews.Cells["I" + i].Value = Service;
                ews.Cells["I" + i].Style.Font.Bold = true;
                ews.Cells["J" + i].Value = FC;
                ews.Cells["J" + i].Style.Font.Bold = true;
                ews.Cells["K" + i].Value = Others;
                ews.Cells["K" + i].Style.Font.Bold = true;
                ews.Cells[ews.Dimension.Address].AutoFitColumns();
                string Todaydate = DateTime.Now.ToString("dd/MM/yyyy");
                string FileName = "VehicleCostDetailsReport" + "-On-" + Todaydate; ;
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=" + FileName + ".xlsx");
                byte[] File = objExcelPackage.GetAsByteArray();
                Response.BinaryWrite(File);
                Response.End();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw;
            }
        }

        //public ActionResult VehicleCostDetailsReportPDF(long VehicleId, int rows, string sidx, string sord, int? page = 1)
        //{
        //    try
        //    {
        //        VehicleDetails vd = new VehicleDetails();
        //        DailyUsageVehicleMaster vstm = new DailyUsageVehicleMaster();
        //        Dictionary<string, object> Criteria = new Dictionary<string, object>();
        //        Criteria.Add("VehicleId", VehicleId);
        //        IList<VehicleCostDetails_Updated> VehicleCostDetailsList = new List<VehicleCostDetails_Updated>();
        //        VehicleCostDetailsList = VehicleCostDetailsReportWithCriteria(VehicleId, rows, sidx, sord, page);
        //        if (VehicleCostDetailsList.Count > 0)
        //        {
        //            NewVehicleCostDetailsPDF VehicleCostDetailsPDF = new NewVehicleCostDetailsPDF();
        //            VehicleCostDetailsPDF.VehicleCostDetailsList = VehicleCostDetailsList;
        //            if (VehicleId > 0)
        //            {
        //                vstm = ts.GetDailyUsageVehicleMasterById(Convert.ToInt32(VehicleId));
        //            }
        //            ViewBag.Campus = vstm.Campus;
        //            ViewBag.VehicleNo = vstm.VehicleNo;
        //            ViewBag.Type = vstm.Type;

        //            return new Rotativa.ViewAsPdf("VehicleCostDetailsReportPDF", VehicleCostDetailsPDF)
        //            {
        //                FileName = "Vehicle Cost Details Report On-" + DateTime.Today.ToString("dd/MM/yyyy") + ".pdf",
        //                PageOrientation = Rotativa.Options.Orientation.Landscape,
        //                PageMargins = new Rotativa.Options.Margins(0, 0, 0, 0)
        //            };
        //        }
        //        else
        //        {
        //            NewVehicleCostDetailsPDF VehicleCostDetailsPDF = new NewVehicleCostDetailsPDF();
        //            return new Rotativa.ViewAsPdf("VehicleCostDetailsReportPDF", VehicleCostDetailsPDF)
        //            {
        //                FileName = "Vehicle Cost Details Report On-" + DateTime.Today.ToString("dd/MM/yyyy") + ".pdf",
        //                PageOrientation = Rotativa.Options.Orientation.Landscape,
        //                PageMargins = new Rotativa.Options.Margins(0, 0, 0, 0)
        //            };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
        //        throw;
        //    }
        //}

        public ActionResult VehicleCostDetailsReportPDF(long VehicleId, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                VehicleDetails vd = new VehicleDetails();
                //DailyUsageVehicleMaster vstm = new DailyUsageVehicleMaster();
                VehicleSubTypeMaster vstm = new VehicleSubTypeMaster();
                Dictionary<string, object> Criteria = new Dictionary<string, object>();
                Criteria.Add("VehicleId", VehicleId);
                IList<VehicleCostDetails_Updated> VehicleCostDetailsList = new List<VehicleCostDetails_Updated>();
                VehicleCostDetailsList = VehicleCostDetailsReportWithCriteria(VehicleId, rows, sidx, sord, page);
                if (VehicleCostDetailsList != null && VehicleCostDetailsList.Count > 0)
                {
                    NewVehicleCostDetailsPDF VehicleCostDetailsPDF = new NewVehicleCostDetailsPDF();
                    VehicleCostDetailsPDF.VehicleCostDetailsList = VehicleCostDetailsList;
                    if (VehicleId > 0)
                    {
                        vstm = ts.GetVehicleSubTypeMasterById(Convert.ToInt32(VehicleId));
                    }
                    ViewBag.Campus = vstm.Campus;
                    ViewBag.VehicleNo = vstm.VehicleNo;
                    ViewBag.Type = vstm.Type;

                    return new Rotativa.ViewAsPdf("VehicleCostDetailsReportPDF", VehicleCostDetailsPDF)
                    {
                        FileName = "Vehicle Cost Details Report On-" + DateTime.Today.ToString("dd/MM/yyyy") + ".pdf",
                        PageOrientation = Rotativa.Options.Orientation.Landscape,
                        PageMargins = new Rotativa.Options.Margins(0, 0, 0, 0)
                    };
                }
                else
                {
                    NewVehicleCostDetailsPDF VehicleCostDetailsPDF = new NewVehicleCostDetailsPDF();
                    return new Rotativa.ViewAsPdf("VehicleCostDetailsReportPDF", VehicleCostDetailsPDF)
                    {
                        FileName = "Vehicle Cost Details Report On-" + DateTime.Today.ToString("dd/MM/yyyy") + ".pdf",
                        PageOrientation = Rotativa.Options.Orientation.Landscape,
                        PageMargins = new Rotativa.Options.Margins(0, 0, 0, 0)
                    };
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw;
            }
        }
        public ActionResult NewVehicleTypeddl()
        {
            try
            {
                MastersService ms = new MastersService();
                Dictionary<string, string> Cmp = new Dictionary<string, string>();

                Dictionary<string, object> criteria = new Dictionary<string, object>();
                TransportService ts = new TransportService();
                Dictionary<long, IList<NewVehicleTypeMaster>> trpMstr = ts.GetNewVehicleTypeMasterListWithPagingAndCriteria(null, null, null, null, criteria);
                if (trpMstr != null && trpMstr.First().Value != null && trpMstr.First().Value.Count > 0)
                {
                    var tripList = (
                             from items in trpMstr.First().Value
                             select new
                             {
                                 Text = items.VehicleType,
                                 Value = items.Id
                             }).Distinct().ToList().OrderBy(x => x.Text);
                    return Json(tripList, JsonRequestBehavior.AllowGet);
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

        //public ActionResult NewVehicleTypeddl()
        //{
        //    try
        //    {
        //        MastersService ms = new MastersService();
        //        Dictionary<string, string> Cmp = new Dictionary<string, string>();

        //        Dictionary<string, object> criteria = new Dictionary<string, object>();
        //        TransportService ts = new TransportService();
        //        Dictionary<long, IList<NewVehicleTypeMaster>> trpMstr = ts.GetNewVehicleTypeMasterListWithPagingAndCriteria(null, null, null, null, criteria);
        //        if (trpMstr != null && trpMstr.First().Value != null && trpMstr.First().Value.Count > 0)
        //        {
        //            var tripList = (
        //                     from items in trpMstr.First().Value
        //                     select new
        //                     {
        //                         Text = items.VehicleType,
        //                         Value = items.VehicleTypeId
        //                     }).Distinct().ToList().OrderBy(x => x.Text);
        //            return Json(tripList, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return Json(null, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
        //        throw ex;
        //    }
        //}
        #endregion
        #region monthwise report added by john naveen
        public ActionResult VehicleCostDetailsReport()
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
        public ActionResult VehicleCostDetailsReportJqGrid(string Campus, string VehicleNo, string FromDate, string ToDate, string ExprtToExcel, int rows, string sidx, string sord, int? page = 1)
        {
            string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
            try
            {
                sord = sord == "desc" ? "Desc" : "Asc";
                TransportService ts = new TransportService();
                IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                DateTime? FromDateNew = new DateTime();
                DateTime? ToDatenew = new DateTime();
                if (!string.IsNullOrEmpty(FromDate))
                {
                    FromDate = FromDate + " " + "23:59:59";
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
                criteria.Add("VehicleNo", VehicleNo);
                criteria.Add("Campus", Campus);
                Dictionary<long, IList<VehicleCostDetailsReport_sp>> VehicleCostDetailsReportList = ts.GetVehicleCostDetailsReportSP(Campus, VehicleNo, FromDateNew, ToDatenew);
                if (VehicleCostDetailsReportList == null || VehicleCostDetailsReportList.FirstOrDefault().Key == 0)
                {
                    var Empty = new { rows = (new { cell = new string[] { } }) };
                    return Json(Empty, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    IList<VehicleCostDetailsReport_sp> VehicleReportList = new List<VehicleCostDetailsReport_sp>();
                    if (sord == "Desc")
                    {
                        VehicleReportList = (from u in VehicleCostDetailsReportList.FirstOrDefault().Value select u).OrderByDescending(x => x.GetType().GetProperty(sidx).GetValue(x, null)).ToList();
                    }
                    else
                    {
                        VehicleReportList = (from u in VehicleCostDetailsReportList.FirstOrDefault().Value select u).OrderBy(x => x.GetType().GetProperty(sidx).GetValue(x, null)).ToList();
                    }

                    if (ExprtToExcel == "Excel")
                    {
                        base.ExptToXL(VehicleReportList, "VehicleCostDetailsReport", (item => new
                        {

                            item.Campus,
                            item.VehicleNo,
                            item.TripCount,
                            item.DriverOt,
                            item.HelperOt,
                            item.Maintenance,
                            item.Diesel,
                            item.Service,
                            item.FC,
                            item.Others

                        }));
                        return new EmptyResult();
                    }
                    else
                    {

                        long totalRecords = VehicleReportList.Count;
                        int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                        var jsonData = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalRecords,
                            rows =
                            (from items in VehicleReportList
                             select new
                             {
                                 i = items.Id,
                                 cell = new string[]
                           {
                                 items.Id.ToString(),
                                 items.Campus,
                                 items.VehicleNo,
                                 items.TripCount,
                                 items.DriverOt.ToString(),
                                 items.HelperOt.ToString(),
                                 items.Diesel.ToString(),
                                 items.Maintenance.ToString(),
                                 items.Service.ToString(),
                                 items.FC.ToString(),
                                 items.Others.ToString()

                          
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
        #region GPS Tracking Device
        public ActionResult GPSTrackingDeviceMaster()
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
        public ActionResult GPSTrackingDeviceMasterJqGrid(GPS_TrackingDeviceMaster gpst, string PurchaseFromDate, string PurchaseTodate, int rows, string sidx, string sord, int? page = 1, int? ExptXl = 0)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportBC samp = new TransportBC();
                    TransportService ts = new TransportService();
                    UserService us = new UserService();
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    Dictionary<string, object> exctcriteria = new Dictionary<string, object>();
                    Dictionary<string, object> likeCriteria = new Dictionary<string, object>();
                    string[] Criteria = new string[8];
                    sord = sord == "asc" ? "Desc" : "Asc";
                    if (!string.IsNullOrEmpty(gpst.Campus)) { exctcriteria.Add("Campus", gpst.Campus); }
                    if (!string.IsNullOrEmpty(gpst.BrandName)) { likeCriteria.Add("BrandName", gpst.BrandName); }
                    if (!string.IsNullOrEmpty(gpst.ModelName)) { likeCriteria.Add("ModelName", gpst.ModelName); }
                    if (!string.IsNullOrEmpty(gpst.IMEINmber)) { likeCriteria.Add("IMEINmber", gpst.IMEINmber); }
                    if (!string.IsNullOrEmpty(PurchaseFromDate))
                    {
                        if (!string.IsNullOrEmpty(PurchaseTodate))
                        {
                            DateTime[] FromToDate = new DateTime[2];
                            FromToDate[0] = DateTime.Parse(PurchaseFromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                            FromToDate[1] = DateTime.Parse(PurchaseTodate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                            string ToDate1 = string.Format("{0:dd/MM/yyyy}", FromToDate[1]);
                            ToDate1 = ToDate1 + " 23:59:59";
                            FromToDate[1] = DateTime.Parse(ToDate1, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                            exctcriteria.Add("PurchaseDate", FromToDate);
                        }
                        else
                        {
                            DateTime[] fromto = new DateTime[2];
                            fromto[0] = DateTime.Parse(PurchaseFromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                            fromto[1] = DateTime.Now;
                            exctcriteria.Add("PurchaseDate", fromto);
                        }
                    }
                    Dictionary<long, IList<GPS_TrackingDeviceMaster>> GPSTrackingDeviceMasterList = ts.GPSTrackingDeviceMasterListWithLikeAndExcactSerachCriteria(page - 1, rows, sord, sidx, exctcriteria, likeCriteria);
                    if (GPSTrackingDeviceMasterList == null || GPSTrackingDeviceMasterList.FirstOrDefault().Key == 0)
                    {
                        var Empty = new { rows = (new { cell = new string[] { } }) };
                        return Json(Empty, JsonRequestBehavior.AllowGet);
                    }
                    long totalRecords = GPSTrackingDeviceMasterList.FirstOrDefault().Value.Count;
                    int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);

                    var jsonData = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalRecords,
                        rows =
                        (from items in GPSTrackingDeviceMasterList.FirstOrDefault().Value
                         select new
                         {
                             i = items.GPS_TrackingDeviceMaster_Id.ToString(),
                             cell = new string[]
                           {

                                 items.GPS_TrackingDeviceMaster_Id.ToString(),
                                 items.Campus,
                                 items.BrandName,
                                 items.ModelName,
                                 items.IMEINmber,
                                 items.PurchaseDate!=null? items.PurchaseDate.ToString("dd/MM/yyyy"):null,
                                 items.CreatedBy!=null?us.GetUserNameByUserId(items.CreatedBy):"",
                                 items.CreatedDate!=null? items.CreatedDate.ToString("dd/MM/yyyy"):null,
                                 items.ModifiedBy,
                                 items.ModifiedDate!=null? items.ModifiedDate.ToString("dd/MM/yyyy"):null,
                          }
                         })
                    };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult SaveOrUpdateGPSTrackingDeviceMaster(GPS_TrackingDeviceMaster gps)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("logOut", "Account");
                else
                {

                    TransportService ts = new TransportService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    if (!string.IsNullOrEmpty(gps.Campus))
                        criteria.Add("Campus", gps.Campus);
                    if (!string.IsNullOrEmpty(gps.ModelName))
                        criteria.Add("ModelName", gps.ModelName);
                    if (!string.IsNullOrEmpty(gps.BrandName))
                        criteria.Add("BrandName", gps.BrandName);
                    if (!string.IsNullOrEmpty(gps.IMEINmber))
                        criteria.Add("IMEINmber", gps.IMEINmber);
                    if (gps == null) return null;
                    GPS_TrackingDeviceMaster gpsObj = new GPS_TrackingDeviceMaster();
                    gpsObj = ts.GetGPS_TrackingDeviceMasterByName(gps.Campus, gps.BrandName, gps.ModelName, gps.IMEINmber);
                    var script = "";
                    if (gps.GPS_TrackingDeviceMaster_Id == 0)
                    {

                        if (gpsObj == null)
                        {

                            gps.CreatedBy = userId;
                            gps.CreatedDate = DateTime.Now;
                            gps.ModifiedBy = userId;
                            gps.ModifiedDate = DateTime.Now;
                            ts.SaveOrUpdateGPSTrackingDeviceMaster(gps);
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
                        if (gpsObj == null)
                        {
                            gps.CreatedBy = userId;
                            gps.CreatedDate = DateTime.Now;
                            gps.ModifiedBy = userId;
                            gps.ModifiedDate = DateTime.Now;
                            ts.SaveOrUpdateGPSTrackingDeviceMaster(gps);
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
        public ActionResult DeleteGPSTrackingDeviceMaster(string[] Id)
        {
            try
            {
                TransportService transportService = new TransportService();
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
                    transportService.DeleteGPSTrackingDeviceMaster(longCityIdArray);
                    var script = @"SucessMsg(""Deleted Successfully"");";
                    return JavaScript(script);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult FillGPSTrackingDeviceIMEINumberByCampus(string Campus)
        {
            Dictionary<string, object> ExactCriteria = new Dictionary<string, object>();
            Dictionary<string, object> LikeCriteria = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(Campus))
                ExactCriteria.Add("Campus", Campus);
            Dictionary<long, IList<GPS_TrackingDeviceMaster>> GPSTrackingDeviceMasterList = ts.GPSTrackingDeviceMasterListWithLikeAndExcactSerachCriteria(null, 99999, string.Empty, string.Empty, ExactCriteria, LikeCriteria);
            if (GPSTrackingDeviceMasterList != null && GPSTrackingDeviceMasterList.FirstOrDefault().Key > 0)
            {
                var DeviceList = (
                         from items in GPSTrackingDeviceMasterList.FirstOrDefault().Value
                         where !string.IsNullOrEmpty(items.IMEINmber)
                         select new
                         {
                             Text = items.IMEINmber,
                             Value = items.GPS_TrackingDeviceMaster_Id
                         }).Distinct().ToList();
                return Json(DeviceList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion        
        #region DailyUsageVehicleMasterList
        //public ActionResult DailyUsageVehicleMasterListJqGrid(VehicleTypeAndSubType vtst, string VehicleType, int rows, string sidx, string sord, int? page = 1)
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //        else
        //        {
        //            TransportService ts = new TransportService();
        //            Dictionary<string, object> criteria = new Dictionary<string, object>();
        //            if (!string.IsNullOrWhiteSpace(VehicleType)) { criteria.Add("VehicleTypeMaster." + "VehicleType", VehicleType); }
        //            if (!string.IsNullOrWhiteSpace(vtst.Type)) { criteria.Add("Type", vtst.Type); }
        //            if (!string.IsNullOrWhiteSpace(vtst.VehicleNo)) { criteria.Add("VehicleNo", vtst.VehicleNo); }
        //            if (!string.IsNullOrWhiteSpace(vtst.FuelType)) { criteria.Add("FuelType", vtst.FuelType); }
        //            if (!string.IsNullOrWhiteSpace(vtst.Campus)) { criteria.Add("Campus", vtst.Campus); }
        //            if (!string.IsNullOrWhiteSpace(vtst.Purpose)) { criteria.Add("Purpose", vtst.Purpose); }
        //            criteria.Add("IsActive", true);
        //            string[] alias = new string[1];
        //            alias[0] = "VehicleTypeMaster";
        //            sord = sord == "desc" ? "Desc" : "Asc";
        //            Dictionary<long, IList<DailyUsageVehicleMaster>> DailyUsageVehicleMaster = ts.GetDailyUsageVehicleMasterListWithsearchCriteriaLikeSearch(page - 1, rows, sord, sidx, criteria, alias);
        //            if (DailyUsageVehicleMaster != null && DailyUsageVehicleMaster.Count > 0)
        //            {
        //                long totalrecords = DailyUsageVehicleMaster.First().Key;
        //                int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
        //                var VehicleSubType = new
        //                {
        //                    total = totalPages,
        //                    page = page,
        //                    records = totalrecords,
        //                    rows = (from items in DailyUsageVehicleMaster.First().Value
        //                            select new
        //                            {
        //                                i = 2,
        //                                cell = new string[] {
        //                       items.Id.ToString(),items.VehicleTypeMaster.VehicleType,items.Type,items.VehicleNo,  items.FuelType, items.Campus,items.Purpose
        //                    }
        //                            })
        //                };
        //                return Json(VehicleSubType, JsonRequestBehavior.AllowGet);

        //            }
        //            else
        //            {
        //                var AssLst = new { rows = (new { cell = new string[] { } }) };
        //                return Json(AssLst, JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
        //        throw ex;
        //    }
        //}
        public ActionResult DailyUsageVehicleMasterListJqGrid(VehicleTypeAndSubType vtst, string VehicleType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    //if (!string.IsNullOrWhiteSpace(VehicleType)) { criteria.Add("NewVehicleTypeMaster.Id", Convert.ToInt64(VehicleType)); }
                    if (!string.IsNullOrWhiteSpace(VehicleType)) { criteria.Add("VehicleTypeMaster.Id", Convert.ToInt32(VehicleType)); }
                    if (!string.IsNullOrWhiteSpace(vtst.Type)) { criteria.Add("Type", vtst.Type); }
                    if (!string.IsNullOrWhiteSpace(vtst.VehicleNo)) { criteria.Add("VehicleNo", vtst.VehicleNo); }
                    if (!string.IsNullOrWhiteSpace(vtst.FuelType)) { criteria.Add("FuelType", vtst.FuelType); }
                    if (!string.IsNullOrWhiteSpace(vtst.Campus)) { criteria.Add("Campus", vtst.Campus); }
                    if (!string.IsNullOrWhiteSpace(vtst.Purpose)) { criteria.Add("Purpose", vtst.Purpose); }
                    criteria.Add("IsActive", true);
                    //string[] alias = new string[1];
                    //alias[0] = "VehicleTypeMaster";
                    sord = sord == "desc" ? "Desc" : "Asc";
                    Dictionary<long, IList<DailyUsageVehicleMaster_vw>> DailyUsageVehicleMaster = ts.GetDailyUsageVehicleMaster_vwListWithsearchCriteriaLikeSearch(page - 1, rows, sidx, sord, criteria);
                    if (DailyUsageVehicleMaster != null && DailyUsageVehicleMaster.Count > 0)
                    {
                        long totalrecords = DailyUsageVehicleMaster.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var VehicleSubType = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in DailyUsageVehicleMaster.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                               items.ViewId.ToString(),items.Id.ToString(),items.Campus,items.VehicleTypeMaster.VehicleType,items.Type,items.VehicleNo,  items.FuelType, items.VehicleTravelDate!=null?items.VehicleTravelDate.Value.ToString("dd/MM/yyyy"):null,
                            }
                                    })
                        };
                        return Json(VehicleSubType, JsonRequestBehavior.AllowGet);

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
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        #endregion

        #region Purpose Master
        public ActionResult PurposeMaster()
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
        public ActionResult GetPurposeMasterJqGrid(PurposeMaster pm, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportBC samp = new TransportBC();
                    TransportService ts = new TransportService();
                    UserService us = new UserService();
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    Dictionary<string, object> criteria = new Dictionary<string, object>();

                    string[] Criteria = new string[8];
                    sord = sord == "asc" ? "Asc" : "Desc";
                    if (!string.IsNullOrEmpty(pm.PurposeName)) { criteria.Add("PurposeName", pm.PurposeName); }
                    Dictionary<long, IList<PurposeMaster>> PurposeMasterList = ts.GetPurposeMasterDetailsByListWithLikeSearchCriteriaCount(page - 1, rows, sord, sidx, criteria);
                    if (PurposeMasterList == null || PurposeMasterList.FirstOrDefault().Key == 0)
                    {
                        var Empty = new { rows = (new { cell = new string[] { } }) };
                        return Json(Empty, JsonRequestBehavior.AllowGet);
                    }
                    long totalRecords = PurposeMasterList.FirstOrDefault().Value.Count;
                    int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);

                    var jsonData = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalRecords,
                        rows =
                        (from items in PurposeMasterList.FirstOrDefault().Value
                         select new
                         {
                             i = items.Purpose_Id.ToString(),
                             cell = new string[]
                           {

                                 items.Purpose_Id.ToString(),
                                 items.PurposeName,
                                 items.IsActive==true?"Yes":"No" ,
                                 items.CreatedBy!=null?us.GetUserNameByUserId(items.CreatedBy):"",
                                 items.CreatedDate!=null? items.CreatedDate.Value.ToString("dd/MM/yyyy"):null,
                                 items.ModifiedBy,
                                 items.ModifiedDate!=null? items.ModifiedDate.Value.ToString("dd/MM/yyyy"):null,
                          }
                         })
                    };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult SaveOrUpdatePurposeMaster(PurposeMaster pm)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("logOut", "Account");
                else
                {

                    TransportService ts = new TransportService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();

                    if (pm == null) return null;
                    PurposeMaster pmObj = new PurposeMaster();
                    if (!string.IsNullOrEmpty(pm.PurposeName))
                        criteria.Add("PurposeName", pm.PurposeName);
                    pmObj = ts.GetPurposeMasterDetailsByPurposeName(pm.PurposeName);
                    var script = "";
                    if (pm.Purpose_Id == 0)
                    {

                        if (pmObj == null)
                        {
                            pm.IsActive = true;
                            pm.CreatedBy = userId;
                            pm.CreatedDate = DateTime.Now;
                            pm.ModifiedBy = userId;
                            pm.ModifiedDate = DateTime.Now;
                            ts.SaveOrUpdatePurposeMaster(pm);
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
                        if (pmObj == null)
                        {
                            pm.IsActive = true;
                            pm.CreatedBy = userId;
                            pm.CreatedDate = DateTime.Now;
                            pm.ModifiedBy = userId;
                            pm.ModifiedDate = DateTime.Now;
                            ts.SaveOrUpdatePurposeMaster(pm);
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
        public ActionResult DeletePurposeMaster(string[] Id)
        {
            try
            {
                TransportService transportService = new TransportService();
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
                    transportService.DeletePurposeMaster(longCityIdArray);
                    var script = @"SucessMsg(""Deleted Successfully"");";
                    return JavaScript(script);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public ActionResult Purposeddl()
        {
            try
            {
                MastersService ms = new MastersService();
                Dictionary<string, string> Cmp = new Dictionary<string, string>();

                Dictionary<string, object> criteria = new Dictionary<string, object>();
                TransportService ts = new TransportService();
                Dictionary<long, IList<PurposeMaster>> trpMstr = ts.GetPurposeMasterDetailsByListWithLikeSearchCriteriaCount(null, 99999, null, null, criteria);
                if (trpMstr != null && trpMstr.First().Value != null && trpMstr.First().Value.Count > 0)
                {
                    var tripList = (
                             from items in trpMstr.First().Value
                             select new
                             {
                                 Text = items.PurposeName,
                                 Value = items.PurposeName
                             }).Distinct().ToList().OrderBy(x => x.Text);
                    return Json(tripList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "Policy");
                throw ex;
            }
        }
        #endregion

        #region VehicleCostDetails_UpdatedJqGridList


        public ActionResult VehicleCostDetails_UpdatedJqGrid(VehicleCostDetails_Updated vsd, int VehicleId, string ExprtToExcel, string EntryType, string VehicleTravelDate, int rows, string sidx, string sord, int? page = 1, int? ExptXl = 0)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportBC samp = new TransportBC();
                    TransportService ts = new TransportService();
                    UserService us = new UserService();
                    VehicleDetails vd = new VehicleDetails();
                    //DailyUsageVehicleMaster vstm = new DailyUsageVehicleMaster();
                    VehicleSubTypeMaster vstm = new VehicleSubTypeMaster();
                    TIPS.Entities.TransportEntities.DriverMaster dm = new TIPS.Entities.TransportEntities.DriverMaster();
                    TIPS.Entities.StaffManagementEntities.StaffDetails sd = new TIPS.Entities.StaffManagementEntities.StaffDetails();
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    Dictionary<string, object> exctcriteria = new Dictionary<string, object>();
                    Dictionary<string, object> likeCriteria = new Dictionary<string, object>();
                    string[] Criteria = new string[8];
                    sord = sord == "asc" ? "Desc" : "Asc";
                    vstm = ts.GetVehicleSubTypeMasterById(VehicleId);
                    if (!string.IsNullOrEmpty(vsd.VehicleNo)) { likeCriteria.Add("VehicleNo", vsd.VehicleNo); }
                    if (!string.IsNullOrEmpty(vsd.Campus)) { likeCriteria.Add("Campus", vsd.Campus); }
                    if (!string.IsNullOrEmpty(vsd.TypeOfTrip)) { likeCriteria.Add("TypeOfTrip", vsd.TypeOfTrip); }
                    if (!string.IsNullOrEmpty(vsd.VehicleRoute)) { likeCriteria.Add("VehicleRoute", vsd.VehicleRoute); }
                    if (vsd.StartKmrs > 0) { likeCriteria.Add("StartKmrs", vsd.StartKmrs); }
                    if (vsd.EndKmrs > 0) { likeCriteria.Add("EndKmrs", vsd.EndKmrs); }
                    if (vsd.DriverOt > 0) { likeCriteria.Add("DriverOt", vsd.DriverOt); }
                    if (vsd.HelperOt > 0) { likeCriteria.Add("HelperOt", vsd.HelperOt); }
                    if (vsd.Diesel > 0) { likeCriteria.Add("Diesel", vsd.Diesel); }
                    if (vsd.Maintenance > 0) { exctcriteria.Add("Maintenance", vsd.Maintenance); }
                    if (vsd.VehicleId > 0) { exctcriteria.Add("VehicleId", vsd.VehicleId); }
                    if (!string.IsNullOrEmpty(vsd.EntryType)) { exctcriteria.Add("EntryType", vsd.EntryType); }
                    if (!string.IsNullOrEmpty(VehicleTravelDate))
                    {
                        if (!string.IsNullOrEmpty(VehicleTravelDate))
                        {
                            string[] traveldate = VehicleTravelDate.Split('-');
                            string LastDay = DateTime.DaysInMonth(Convert.ToInt32(traveldate[1]), Convert.ToInt32(traveldate[0])).ToString();
                            DateTime[] FromToDate = new DateTime[2];
                            string LastDate = LastDay + "-" + VehicleTravelDate;
                            FromToDate[0] = DateTime.Parse(VehicleTravelDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                            FromToDate[1] = DateTime.Parse(LastDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);

                            string ToDate1 = string.Format("{0:dd/MM/yyyy}", FromToDate[1]);
                            ToDate1 = ToDate1 + " 23:59:59";
                            FromToDate[1] = DateTime.Parse(ToDate1, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                            exctcriteria.Add("VehicleTravelDate", FromToDate);
                        }

                    }
                    Dictionary<long, IList<VehicleCostDetails_Updated>> VehicleCostList = ts.VehicleCostDetails_UpdatedListWithLikeAndExcactSerachCriteria(page - 1, rows, sord, sidx, exctcriteria, likeCriteria);

                    if (VehicleCostList == null || VehicleCostList.FirstOrDefault().Key == 0)
                    {
                        var Empty = new { rows = (new { cell = new string[] { } }) };
                        return Json(Empty, JsonRequestBehavior.AllowGet);
                    }


                    long totalRecords = VehicleCostList.FirstOrDefault().Value.Count;
                    int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);

                    var jsonData = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalRecords,
                        userdata = new
                        {
                            Distance = VehicleCostList.FirstOrDefault().Value.Sum(x => x.Distance).ToString("0,0"),
                            DriverOt = VehicleCostList.FirstOrDefault().Value.Sum(x => x.DriverOt).ToString("C"),
                            HelperOt = VehicleCostList.FirstOrDefault().Value.Sum(x => x.HelperOt).ToString("C"),
                            Diesel = VehicleCostList.FirstOrDefault().Value.Sum(x => x.Diesel).ToString("C"),
                            Maintenance = VehicleCostList.FirstOrDefault().Value.Sum(x => x.Maintenance).ToString("C"),
                            Service = VehicleCostList.FirstOrDefault().Value.Sum(x => x.Service).ToString("C"),
                            FC = VehicleCostList.FirstOrDefault().Value.Sum(x => x.FC).ToString("C"),
                            Others = VehicleCostList.FirstOrDefault().Value.Sum(x => x.Others).ToString("C"),

                        },
                        rows =
                        (from items in VehicleCostList.FirstOrDefault().Value
                         select new
                         {
                             i = items.VehicleId.ToString(),
                             cell = new string[]
                           {                      
                                  items.VehicleCostId.ToString(),
                                  vstm.Id.ToString(),
                                  items.VehicleTravelDate!=null?items.VehicleTravelDate.ToString("dd/MM/yyyy"):null,
                                  items.EntryType,
                                  vstm.Campus,
                                  items.TypeOfTrip,
                                  vstm.VehicleNo,
                                  items.DriverMaster!=null?items.DriverMaster.Name:"",
                                  items.StaffDetails!=null?items.StaffDetails.Name:"",
                                  items.VehicleRoute,
                                  items.StartKmrs.ToString(),
                                  items.EndKmrs.ToString(),
                                  items.IsKMReseted == true ? "Yes":"No",
                                  items.KMResetValue.ToString(),
                                  items.Distance == 0 ? "0":items.Distance.ToString("0,0"),
                                  items.DriverOt.ToString("C"),
                                  items.HelperOt.ToString("C"),
                                  items.Diesel.ToString("C"),                                 
                                  items.Maintenance.ToString("C"),                                 
                                  items.Service.ToString("C"),                                 
                                  items.FC.ToString("C"),                                 
                                  items.Others.ToString("C"),                                 
                                  items.CreatedBy!=null?us.GetUserNameByUserId(items.CreatedBy):"",
                                  items.CreatedDate!=null? items.CreatedDate.ToString("dd/MM/yyyy"):null,
                                  items.ModifiedBy,
                                  items.ModifiedDate!=null? items.ModifiedDate.ToString("dd/MM/yyyy"):null,
                          }
                         })
                    };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        #endregion

        #region SaveOrUpdateVehicleCostDetails_Updated
        public ActionResult SaveOrUpdateVehicleCostDetails_Updated(VehicleCostDetails_Updated vcd, long DriverMasterId)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    var data = string.Empty;
                    TransportBC lmrepo = new TransportBC();
                    TransportService ts = new TransportService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<string, object> likeCriteria = new Dictionary<string, object>();
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    vcd.VehicleTravelDate = DateTime.Parse(Request["VehicleTravelDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    var VehicleDate = vcd.VehicleTravelDate.AddDays(-1);
                    criteria.Add("VehicleTravelDate", VehicleDate);
                    criteria.Add("TypeOfTrip", vcd.TypeOfTrip);
                    criteria.Add("VehicleId", vcd.VehicleId);
                    criteria.Add("HelperId", vcd.HelperId);
                    VehicleCostDetails_Updated vcdl = new VehicleCostDetails_Updated();
                    VehicleCostDetails_Updated vehicleCostDetails = new VehicleCostDetails_Updated();
                    vehicleCostDetails = ts.GetVehicleCostDetails_UpdatedByTravelDate(vcd.VehicleId, VehicleDate);
                    criteria.Clear();
                    criteria.Add("VehicleId", vcd.VehicleId);
                    criteria.Add("Rank", Convert.ToInt64(1));
                    criteria.Add("EntryType", "Trip");
                    Dictionary<long, IList<VehicleCostDetails_Updated_VW>> VehicleCostDetails_VW = ts.GetVehicleCostDetails_Updated_VWListWithPagingAndCriteriaLikeSearch(0, 9999, null, null, criteria);
                    if (vcd.EntryType == "Trip")
                    {
                        if (VehicleCostDetails_VW != null && VehicleCostDetails_VW.FirstOrDefault().Key > 0)
                        {
                            //VehicleTravelDate = VehicleCostDetails_VW.First().Value.Vehicle.ToString();
                            DateTime OldVehicleTravelDate = VehicleCostDetails_VW.First().Value[0].VehicleTravelDate;
                            if (OldVehicleTravelDate > vcd.VehicleTravelDate)
                            {
                                var jsondata = new
                                {

                                    statusval = "DateFaild"
                                };
                                return Json(jsondata, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    var script = "";
                    if (vehicleCostDetails != null)
                    {
                        data = "Success";
                        if (vcd.VehicleCostId == 0)
                        {
                            DriverMaster dm = new DriverMaster();
                            dm.Id = DriverMasterId;
                            if (vcd.IsKMReseted == true)
                            {
                                vcd.Distance = (vcd.KMResetValue - vcd.StartKmrs) + vcd.EndKmrs;
                            }
                            else
                            {
                                vcd.Distance = vcd.EndKmrs - vcd.StartKmrs;
                            }
                            vcd.CreatedBy = userId;
                            vcd.CreatedDate = DateTime.Now;
                            vcd.ModifiedBy = userId;
                            vcd.ModifiedDate = DateTime.Now;
                            vcd.DriverMaster = dm;
                            ts.SaveOrUpdateVehicleCostDetails_Updated(vcd);
                            var jsondata = new
                            {
                                EndKmrs = vcd.EndKmrs,
                                statusval = "success"
                            };
                            return Json(jsondata, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            vcd.CreatedBy = userId;
                            vcd.CreatedDate = DateTime.Now;
                            vcd.ModifiedBy = userId;
                            vcd.ModifiedDate = DateTime.Now;

                            ts.SaveOrUpdateVehicleCostDetails_Updated(vcd);

                            return JavaScript(script);
                        }

                    }
                    else
                    {
                        var jsondata = new
                        {

                            statusval = "failed"
                        };
                        return Json(jsondata, JsonRequestBehavior.AllowGet);

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



        #region SaveVehicleCostDetails_Updated
        public ActionResult SaveVehicleCostDetails_Updated(VehicleCostDetails_Updated vcd, long DriverMasterId)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportBC lmrepo = new TransportBC();
                    TransportService ts = new TransportService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<string, object> likeCriteria = new Dictionary<string, object>();
                    VehicleCostDetails_Updated vcdl = new VehicleCostDetails_Updated();
                    criteria.Add("HelperId", vcd.HelperId);
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    vcd.VehicleTravelDate = DateTime.Parse(Request["VehicleTravelDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    var script = "";

                    DriverMaster dm = new DriverMaster();
                    dm.Id = DriverMasterId;
                    if (vcd.IsKMReseted == true)
                    {
                        vcd.Distance = (vcd.KMResetValue - vcd.StartKmrs) + vcd.EndKmrs;
                    }
                    else
                    {
                        vcd.Distance = vcd.EndKmrs - vcd.StartKmrs;
                    }
                    vcd.CreatedBy = userId;
                    vcd.CreatedDate = DateTime.Now;
                    vcd.ModifiedBy = userId;
                    vcd.ModifiedDate = DateTime.Now;
                    vcd.DriverMaster = dm;
                    ts.SaveOrUpdateVehicleCostDetails_Updated(vcd);
                    var jsondata = new
                    {
                        EndKmrs = vcd.EndKmrs,
                        statusval = "success"
                    };
                    return Json(jsondata, JsonRequestBehavior.AllowGet);
                }
            }

            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw;
            }
        }
        #endregion

        public JsonResult FillVehicleNoOnDailyUsageVehicleMasterByCampus(string Campus)
        {
            string userId = base.ValidateUser();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(Campus)) { criteria.Add("Campus", Campus); }
            Dictionary<long, IList<DailyUsageVehicleMaster>> DailyUsageVehicleMaster = ts.GetDailyUsageVehicleMasterListWithsearchCriteriaLikeSearch(null, 99999, string.Empty, string.Empty, criteria);
            if (DailyUsageVehicleMaster != null && DailyUsageVehicleMaster.Count > 0)
            {
                var VehicleList = (
                         from items in DailyUsageVehicleMaster.First().Value
                         select new
                         {
                             Text = items.VehicleNo,
                             Value = items.VehicleNo
                         }).Distinct().ToList();
                return Json(VehicleList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        #region Supplier master for transport
        public ActionResult TransportSupplierMaster()
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
        public ActionResult TransportSupplierMasterListJqGrid(TransportSupplierMaster tsm, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    UserService us = new UserService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    sord = sord == "desc" ? "Desc" : "Asc";
                    if (!string.IsNullOrEmpty(tsm.SupplierName)) { criteria.Add("SupplierName", tsm.SupplierName); }
                    if (!string.IsNullOrEmpty(tsm.SupplierType)) { criteria.Add("SupplierType", tsm.SupplierType); }
                    Dictionary<long, IList<TransportSupplierMaster>> TransportSupplierMasterList = ts.GetTransportSupplierMasterWithPagingLikeSearch(page - 1, rows, sidx, sord, criteria);
                    if (TransportSupplierMasterList != null && TransportSupplierMasterList.Count > 0)
                    {
                        long totalrecords = TransportSupplierMasterList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var SupplierMaster = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in TransportSupplierMasterList.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                                        
                                        items.SupplierId.ToString(),
                                        items.SupplierName,
                                        items.SupplierType,
                                        !string.IsNullOrEmpty(items.CreatedBy)?us.GetUserNameByUserId(items.CreatedBy):string.Empty,
                                        items.CreatedDate!=null? items.CreatedDate.Value.ToString("dd/MM/yyyy"):null,
                                        !string.IsNullOrEmpty(items.ModifiedBy)?us.GetUserNameByUserId(items.ModifiedBy):string.Empty,
                                        items.ModifiedDate!=null? items.ModifiedDate.Value.ToString("dd/MM/yyyy"):null

                            }
                                    })
                        };
                        return Json(SupplierMaster, JsonRequestBehavior.AllowGet);

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
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult SaveOrUpdateTransportSupplierMaster(TransportSupplierMaster tsm)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("logOut", "Account");
                else
                {

                    TransportService ts = new TransportService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();

                    if (tsm == null) return null;
                    TransportSupplierMaster tsmObj = new TransportSupplierMaster();
                    tsmObj = ts.GetTransportSupplierMasterBySupplierName(tsm.SupplierName, tsm.SupplierType);
                    var script = "";
                    if (tsm.SupplierId == 0)
                    {

                        if (tsmObj == null)
                        {
                            tsm.CreatedBy = userId;
                            tsm.CreatedDate = DateTime.Now;
                            tsm.ModifiedBy = userId;
                            tsm.ModifiedDate = DateTime.Now;
                            ts.SaveOrUpdateTransportSupplierMaster(tsm);
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
                        if (tsmObj == null)
                        {
                            tsm.CreatedBy = userId;
                            tsm.CreatedDate = DateTime.Now;
                            tsm.ModifiedBy = userId;
                            tsm.ModifiedDate = DateTime.Now;
                            ts.SaveOrUpdateTransportSupplierMaster(tsm);
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
        public ActionResult DeleteTransportSupplierMaster(string[] Id)
        {
            try
            {
                TransportService transportService = new TransportService();
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
                    transportService.DeleteTransportSupplierMaster(longCityIdArray);
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
        #region Vehicle Fuel Entry Form
        public ActionResult VehicleFuelEntryForm(long VehicleCostId)
        {
            try
            {
                TransportService ts = new TransportService();
                //VehicleCostDetails_Updated vcd = new VehicleCostDetails_Updated();
                VehicleFuelRefillEntry vf = new VehicleFuelRefillEntry();
                vf = ts.GetVehicleFuelRefillEntryByVehicleCostId(VehicleCostId);
                ViewBag.VehicleCostId = VehicleCostId;
                return PartialView(vf);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult VehicleFuelEntryListJqGrid(VehicleFuelRefillEntry vfr, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    UserService us = new UserService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    sord = sord == "desc" ? "Desc" : "Asc";
                    Dictionary<long, IList<VehicleFuelRefillEntry>> VehicleFuelRefillList = ts.GetVehicleFuelRefillEntryDetailsWithPagingLikeSearch(page - 1, rows, sidx, sord, criteria);
                    if (VehicleFuelRefillList != null && VehicleFuelRefillList.Count > 0)
                    {
                        long totalrecords = VehicleFuelRefillList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var FuelRefill = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in VehicleFuelRefillList.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                                        
                                        items.FuelRefillId.ToString(),
                                        items.VehicleId.ToString(),
                                        items.VehicleCostId.ToString(),
                                        items.VehicleNo,
                                        items.Campus,
                                        items.Vendor,
                                        items.IndentNumber.ToString(),
                                        items.InvoiceNumber.ToString(),
                                        items.Description,
                                        items.EntryDate!=null? items.EntryDate.Value.ToString("dd/MM/yyyy"):null,
                                        !string.IsNullOrEmpty(items.CreatedBy)?us.GetUserNameByUserId(items.CreatedBy):string.Empty,
                                        items.CreatedDate!=null? items.CreatedDate.Value.ToString("dd/MM/yyyy"):null,
                                        !string.IsNullOrEmpty(items.ModifiedBy)?us.GetUserNameByUserId(items.ModifiedBy):string.Empty,
                                        items.ModifiedDate!=null? items.ModifiedDate.Value.ToString("dd/MM/yyyy"):null

                            }
                                    })
                        };
                        return Json(FuelRefill, JsonRequestBehavior.AllowGet);

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
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult SaveOrUpdateVehicleFuelEntry(VehicleFuelRefillEntry vfr)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("logOut", "Account");
                else
                {

                    TransportService ts = new TransportService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    if (vfr == null) return null;
                    // string iDate = EntryDate;
                    //this.Text = "22/11/2009";
                    //DateTime date = DateTime.ParseExact(this.Text, "dd/MM/yyyy", null);
                    //DateTime Date = Convert.ToDateTime(EntryDate);
                    //DateTime Date = DateTime.ParseExact(EntryDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);                    
                    if (vfr.FuelRefillId == 0)
                    {
                        //vfr.EntryDate = Date;
                        vfr.CreatedBy = userId;
                        vfr.CreatedDate = DateTime.Now;
                        vfr.ModifiedBy = userId;
                        vfr.ModifiedDate = DateTime.Now;
                        ts.SaveOrUpdateVehicleFuelRefillEntry(vfr);
                        var jsondata = new { statusval = "added", FuelRefillId = vfr.FuelRefillId };
                        return Json(jsondata, JsonRequestBehavior.AllowGet);
                    }
                    else if (vfr.FuelRefillId > 0)
                    {
                        VehicleFuelRefillEntry VehicleFuelEntryDetails = ts.GetVehicleFuelRefillEntryById(vfr.FuelRefillId);
                        //VehicleFuelEntryDetails.EntryDate = Date;
                        VehicleFuelEntryDetails.ModifiedBy = userId;
                        VehicleFuelEntryDetails.ModifiedDate = DateTime.Now;
                        VehicleFuelEntryDetails.Description = vfr.Description;
                        VehicleFuelEntryDetails.IndentNumber = vfr.IndentNumber;
                        VehicleFuelEntryDetails.InvoiceNumber = vfr.InvoiceNumber;
                        VehicleFuelEntryDetails.Vendor = vfr.Vendor;
                        ts.SaveOrUpdateVehicleFuelRefillEntry(VehicleFuelEntryDetails);
                        var jsondata = new { statusval = "updated", FuelRefillId = vfr.FuelRefillId };
                        return Json(jsondata, JsonRequestBehavior.AllowGet);
                    }
                    return Json(null, JsonRequestBehavior.AllowGet);
                }

            }

            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "MasterPolicy");
                throw ex;
            }
        }
        #endregion
        #region Vehicle Maintenance Entry form
        public ActionResult VehicleMaintenanceEntryForm(long VehicleCostId)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    VehicleCostDetails_Updated vcd = new VehicleCostDetails_Updated();
                    VehicleMaintenanceEntry vme = new VehicleMaintenanceEntry();
                    vcd = ts.GetVehicleCostDetails_UpdatedById(VehicleCostId);
                    ViewBag.EntryDate = vcd.VehicleTravelDate;
                    vme = ts.GetVehicleMaintenanceEntryByVehicleCostId(VehicleCostId);
                    ViewBag.VehicleCostId = VehicleCostId;
                    return PartialView(vme);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult VehicleMaintenanceEntryListJqGrid(VehicleMaintenanceEntry vme, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    UserService us = new UserService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    sord = sord == "desc" ? "Desc" : "Asc";
                    Dictionary<long, IList<VehicleMaintenanceEntry>> VehicleMaintenanceEntryList = ts.GetVehicleMaintenanceEntryDetailsWithPagingLikeSearch(page - 1, rows, sidx, sord, criteria);
                    if (VehicleMaintenanceEntryList != null && VehicleMaintenanceEntryList.Count > 0)
                    {
                        long totalrecords = VehicleMaintenanceEntryList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var VehicleMaintenanceEntry = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in VehicleMaintenanceEntryList.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                                        
                                        items.MaintenanceId.ToString(),
                                        items.VehicleId.ToString(),
                                        items.VehicleNo,
                                        items.Campus,
                                        items.AC.ToString(),
                                        items.Battery.ToString(),
                                        items.Tyre.ToString(),
                                        items.SupplierName,
                                        items.InvoiceNo.ToString(),
                                        items.Amount.ToString(),
                                        items.Description,
                                        items.VehicleTravelDate!=null? items.VehicleTravelDate.Value.ToString("dd/MM/yyyy"):null,
                                        !string.IsNullOrEmpty(items.CreatedBy)?us.GetUserNameByUserId(items.CreatedBy):string.Empty,
                                        items.CreatedDate!=null? items.CreatedDate.Value.ToString("dd/MM/yyyy"):null,
                                        !string.IsNullOrEmpty(items.ModifiedBy)?us.GetUserNameByUserId(items.ModifiedBy):string.Empty,
                                        items.ModifiedDate!=null? items.ModifiedDate.Value.ToString("dd/MM/yyyy"):null,
                                        items.MechanicalMaintenance.ToString(),
                                        items.ElectricalMaintenance.ToString(),
                                        items.BodyMaintenance.ToString()

                            }
                                    })
                        };
                        return Json(VehicleMaintenanceEntry, JsonRequestBehavior.AllowGet);

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
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult SaveOrUpdateVehicleMaintenanceEntry(VehicleMaintenanceEntry vme, string VehicleTravelDate)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("logOut", "Account");
                else
                {

                    TransportService ts = new TransportService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    if (vme == null) return null;
                    //var script = "";
                    DateTime Date = DateTime.ParseExact(VehicleTravelDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    if (vme.MaintenanceId == 0)
                    {
                        vme.VehicleTravelDate = Date;
                        vme.CreatedBy = userId;
                        vme.CreatedDate = DateTime.Now;
                        vme.ModifiedBy = userId;
                        vme.ModifiedDate = DateTime.Now;
                        ts.SaveOrUpdateVehicleMaintenanceEntry(vme);
                        var jsondata = new { statusval = "added", MaintenanceId = vme.MaintenanceId };
                        return Json(jsondata, JsonRequestBehavior.AllowGet);
                    }
                    else if (vme.MaintenanceId > 0)
                    {
                        VehicleMaintenanceEntry VehicleMaintenanceEntryDetails = ts.GetVehicleMaintenanceEntryById(vme.MaintenanceId);
                        VehicleMaintenanceEntryDetails.VehicleTravelDate = Date;
                        VehicleMaintenanceEntryDetails.Campus = vme.Campus;
                        VehicleMaintenanceEntryDetails.VehicleId = vme.VehicleId;
                        VehicleMaintenanceEntryDetails.Description = vme.Description;
                        VehicleMaintenanceEntryDetails.Amount = vme.Amount;
                        VehicleMaintenanceEntryDetails.AC = vme.AC;
                        VehicleMaintenanceEntryDetails.Tyre = vme.Tyre;
                        VehicleMaintenanceEntryDetails.Battery = vme.Battery;
                        VehicleMaintenanceEntryDetails.InvoiceNo = vme.InvoiceNo;
                        VehicleMaintenanceEntryDetails.SupplierName = vme.SupplierName;
                        VehicleMaintenanceEntryDetails.MechanicalMaintenance = vme.MechanicalMaintenance;
                        VehicleMaintenanceEntryDetails.ElectricalMaintenance = vme.ElectricalMaintenance;
                        VehicleMaintenanceEntryDetails.BodyMaintenance = vme.BodyMaintenance;
                        VehicleMaintenanceEntryDetails.ModifiedBy = userId;
                        VehicleMaintenanceEntryDetails.ModifiedDate = DateTime.Now;
                        ts.SaveOrUpdateVehicleMaintenanceEntry(VehicleMaintenanceEntryDetails);
                        var jsondata = new { statusval = "updated", MaintenanceId = vme.MaintenanceId };
                        return Json(jsondata, JsonRequestBehavior.AllowGet);
                    }
                    return Json(null, JsonRequestBehavior.AllowGet);
                }

            }

            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "MasterPolicy");
                throw ex;
            }
        }
        #endregion
        #region Service Entry form
        public ActionResult VehicleServiceEntryForm(long VehicleCostId)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    VehicleCostDetails_Updated vcd = new VehicleCostDetails_Updated();
                    VehicleService vs = new VehicleService();
                    vcd = ts.GetVehicleCostDetails_UpdatedById(VehicleCostId);
                    vs = ts.GetVehicleServiceByVehicleCostId(VehicleCostId);
                    ViewBag.EntryDate = vcd.VehicleTravelDate;
                    ViewBag.VehicleCostId = VehicleCostId;
                    return PartialView(vs);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult VehicleVehicleServiceListJqGrid(VehicleService vs, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    UserService us = new UserService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    sord = sord == "desc" ? "Desc" : "Asc";
                    Dictionary<long, IList<VehicleService>> VehicleServiceList = ts.GetVehicleServiceDetailsWithPagingLikeSearch(page - 1, rows, sidx, sord, criteria);
                    if (VehicleServiceList != null && VehicleServiceList.Count > 0)
                    {
                        long totalrecords = VehicleServiceList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var VehicleService = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in VehicleServiceList.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                                        
                                        items.ServiceId.ToString(),
                                        items.VehicleId.ToString(),
                                        items.VehicleNo,
                                        items.Campus,
                                        items.StartKms.ToString(),
                                        items.EndKms.ToString(),
                                        items.Vendor,
                                        items.InvoiceNo.ToString(),
                                        items.Description,
                                        items.EntryDate!=null? items.EntryDate.Value.ToString("dd/MM/yyyy"):null,
                                        !string.IsNullOrEmpty(items.CreatedBy)?us.GetUserNameByUserId(items.CreatedBy):string.Empty,
                                        items.CreatedDate!=null? items.CreatedDate.Value.ToString("dd/MM/yyyy"):null,
                                        !string.IsNullOrEmpty(items.ModifiedBy)?us.GetUserNameByUserId(items.ModifiedBy):string.Empty,
                                        items.ModifiedDate!=null? items.ModifiedDate.Value.ToString("dd/MM/yyyy"):null

                            }
                                    })
                        };
                        return Json(VehicleService, JsonRequestBehavior.AllowGet);

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
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult SaveOrUpdateVehicleService(VehicleService vm, string EntryDate)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("logOut", "Account");
                else
                {

                    TransportService ts = new TransportService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    if (vm == null) return null;
                    //var script = "";
                    DateTime Date = DateTime.ParseExact(EntryDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    if (vm.ServiceId == 0)
                    {
                        vm.EntryDate = Date;
                        vm.CreatedBy = userId;
                        vm.CreatedDate = DateTime.Now;
                        vm.ModifiedBy = userId;
                        vm.ModifiedDate = DateTime.Now;
                        ts.SaveOrUpdateVehicleService(vm);
                        var jsondata = new { statusval = "added", ServiceId = vm.ServiceId };
                        return Json(jsondata, JsonRequestBehavior.AllowGet);
                    }
                    else if (vm.ServiceId > 0)
                    {
                        VehicleService VehicleServiceDetails = ts.GetVehicleServiceById(vm.ServiceId);
                        VehicleServiceDetails.EntryDate = Date;
                        VehicleServiceDetails.Description = vm.Description;
                        VehicleServiceDetails.InvoiceNo = vm.InvoiceNo;
                        VehicleServiceDetails.Vendor = vm.Vendor;
                        VehicleServiceDetails.StartKms = vm.StartKms;
                        VehicleServiceDetails.EndKms = vm.EndKms;
                        VehicleServiceDetails.ModifiedBy = userId;
                        VehicleServiceDetails.ModifiedDate = DateTime.Now;
                        ts.SaveOrUpdateVehicleService(VehicleServiceDetails);
                        var jsondata = new { statusval = "updated", ServiceId = VehicleServiceDetails.ServiceId };
                        return Json(jsondata, JsonRequestBehavior.AllowGet);
                    }
                    return Json(null, JsonRequestBehavior.AllowGet);
                }

            }

            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "MasterPolicy");
                throw ex;
            }
        }
        #endregion
        #region FC Entry form
        public ActionResult VehicleFCEntryForm(long VehicleCostId)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    VehicleCostDetails_Updated vcd = new VehicleCostDetails_Updated();
                    //VehicleFuelRefillEntry vf = new VehicleFuelRefillEntry();
                    VehicleFCEntry vfce = new VehicleFCEntry();
                    vcd = ts.GetVehicleCostDetails_UpdatedById(VehicleCostId);
                    vfce = ts.GetVehicleFCEntryByVehicleCostId(VehicleCostId);
                    ViewBag.EntryDate = vcd.VehicleTravelDate;
                    ViewBag.VehicleCostId = VehicleCostId;
                    return PartialView(vfce);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult VehicleVehicleFCEntryListJqGrid(VehicleFCEntry vfe, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    UserService us = new UserService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    sord = sord == "desc" ? "Desc" : "Asc";
                    Dictionary<long, IList<VehicleFCEntry>> VehicleFCEntryList = ts.GetVehicleFCEntryDetailsWithPagingLikeSearch(page - 1, rows, sidx, sord, criteria);
                    if (VehicleFCEntryList != null && VehicleFCEntryList.Count > 0)
                    {
                        long totalrecords = VehicleFCEntryList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var VehicleFCEntry = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in VehicleFCEntryList.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                                        
                                        items.FCId.ToString(),
                                        items.VehicleId.ToString(),
                                        items.VehicleNo,
                                        items.Campus,
                                        items.Vendor,
                                        items.InvoiceNo.ToString(),
                                        items.Description,
                                        items.EntryDate!=null? items.EntryDate.Value.ToString("dd/MM/yyyy"):null,
                                        !string.IsNullOrEmpty(items.CreatedBy)?us.GetUserNameByUserId(items.CreatedBy):string.Empty,
                                        items.CreatedDate!=null? items.CreatedDate.Value.ToString("dd/MM/yyyy"):null,
                                        !string.IsNullOrEmpty(items.ModifiedBy)?us.GetUserNameByUserId(items.ModifiedBy):string.Empty,
                                        items.ModifiedDate!=null? items.ModifiedDate.Value.ToString("dd/MM/yyyy"):null

                            }
                                    })
                        };
                        return Json(VehicleFCEntry, JsonRequestBehavior.AllowGet);

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
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult SaveOrUpdateVehicleFCEntry(VehicleFCEntry vfc, string EntryDate)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("logOut", "Account");
                else
                {

                    TransportService ts = new TransportService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    if (vfc == null) return null;
                    //var script = "";
                    DateTime Date = DateTime.ParseExact(EntryDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    if (vfc.FCId == 0)
                    {
                        vfc.EntryDate = Date;
                        vfc.CreatedBy = userId;
                        vfc.CreatedDate = DateTime.Now;
                        vfc.ModifiedBy = userId;
                        vfc.ModifiedDate = DateTime.Now;
                        ts.SaveOrUpdateVehicleFCEntry(vfc);
                        var jsondata = new { statusval = "added", FCId = vfc.FCId };
                        return Json(jsondata, JsonRequestBehavior.AllowGet);
                    }
                    else if (vfc.FCId > 0)
                    {
                        VehicleFCEntry VehicleFCEntryDetails = ts.GetVehicleFCEntryById(vfc.FCId);
                        VehicleFCEntryDetails.EntryDate = Date;
                        VehicleFCEntryDetails.Description = vfc.Description;
                        VehicleFCEntryDetails.InvoiceNo = vfc.InvoiceNo;
                        VehicleFCEntryDetails.Vendor = vfc.Vendor;
                        VehicleFCEntryDetails.ModifiedBy = userId;
                        VehicleFCEntryDetails.ModifiedDate = DateTime.Now;
                        ts.SaveOrUpdateVehicleFCEntry(VehicleFCEntryDetails);
                        var jsondata = new { statusval = "updated", FCId = VehicleFCEntryDetails.FCId };
                        return Json(jsondata, JsonRequestBehavior.AllowGet);
                    }
                    return Json(null, JsonRequestBehavior.AllowGet);
                }

            }

            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "MasterPolicy");
                throw ex;
            }
        }
        #endregion
        #region Others Entry Form
        public ActionResult VehicleOthersEntryForm(long VehicleCostId)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    VehicleCostDetails_Updated vcd = new VehicleCostDetails_Updated();
                    VehicleOthersEntry voe = new VehicleOthersEntry();
                    vcd = ts.GetVehicleCostDetails_UpdatedById(VehicleCostId);
                    voe = ts.GetVehicleOthersEntryByVehicleCostId(VehicleCostId);
                    ViewBag.EntryDate = vcd.VehicleTravelDate;
                    ViewBag.VehicleCostId = VehicleCostId;
                    return PartialView(voe);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult VehicleOthersEntryListJqGrid(VehicleOthersEntry voe, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TransportService ts = new TransportService();
                    UserService us = new UserService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    sord = sord == "desc" ? "Desc" : "Asc";
                    Dictionary<long, IList<VehicleOthersEntry>> VehicleOthersEntryList = ts.GetVehicleOthersEntryDetailsWithPagingLikeSearch(page - 1, rows, sidx, sord, criteria);
                    if (VehicleOthersEntryList != null && VehicleOthersEntryList.Count > 0)
                    {
                        long totalrecords = VehicleOthersEntryList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var VehicleOthersEntry = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in VehicleOthersEntryList.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                                        
                                        items.OthersId.ToString(),
                                        items.VehicleId.ToString(),
                                        items.VehicleNo,
                                        items.Campus,
                                        items.Description,
                                        items.EntryDate!=null? items.EntryDate.Value.ToString("dd/MM/yyyy"):null,
                                        !string.IsNullOrEmpty(items.CreatedBy)?us.GetUserNameByUserId(items.CreatedBy):string.Empty,
                                        items.CreatedDate!=null? items.CreatedDate.Value.ToString("dd/MM/yyyy"):null,
                                        !string.IsNullOrEmpty(items.ModifiedBy)?us.GetUserNameByUserId(items.ModifiedBy):string.Empty,
                                        items.ModifiedDate!=null? items.ModifiedDate.Value.ToString("dd/MM/yyyy"):null

                            }
                                    })
                        };
                        return Json(VehicleOthersEntry, JsonRequestBehavior.AllowGet);

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
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult SaveOrUpdateVehicleOthersEntry(VehicleOthersEntry vom, string EntryDate)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("logOut", "Account");
                else
                {

                    TransportService ts = new TransportService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    if (vom == null) return null;
                    DateTime Date = DateTime.ParseExact(EntryDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    if (vom.OthersId == 0)
                    {
                        vom.EntryDate = Date;
                        vom.CreatedBy = userId;
                        vom.CreatedDate = DateTime.Now;
                        vom.ModifiedBy = userId;
                        vom.ModifiedDate = DateTime.Now;
                        ts.SaveOrUpdateVehicleOthersEntry(vom);
                        var jsondata = new { statusval = "added", OthersId = vom.OthersId };
                        return Json(jsondata, JsonRequestBehavior.AllowGet);
                    }
                    else if (vom.OthersId > 0)
                    {
                        VehicleOthersEntry VehicleOthersEntryDetails = ts.GetVehicleOthersEntryById(vom.OthersId);
                        VehicleOthersEntryDetails.EntryDate = Date;
                        VehicleOthersEntryDetails.Description = vom.Description;
                        VehicleOthersEntryDetails.ModifiedBy = userId;
                        VehicleOthersEntryDetails.ModifiedDate = DateTime.Now;
                        ts.SaveOrUpdateVehicleOthersEntry(VehicleOthersEntryDetails);
                        var jsondata = new { statusval = "updated", OthersId = VehicleOthersEntryDetails.OthersId };
                        return Json(jsondata, JsonRequestBehavior.AllowGet);
                    }
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "MasterPolicy");
                throw ex;
            }
        }
        #endregion
        public ActionResult VehicleOverviewReport()
        {
            try
            {
                return PartialView();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult VehicleOverviewReportListJqGrid(string Campus, string VehicleNo, string VehicleType, string MonthYear, string ExportType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    sord = sord == "desc" ? "Desc" : "Asc";
                    DateTime? FromDate = null;
                    DateTime? ToDate = null;
                    if (!string.IsNullOrEmpty(MonthYear))
                    {
                        string[] traveldate = MonthYear.Split('-');
                        string LastDay = DateTime.DaysInMonth(Convert.ToInt32(traveldate[1]), Convert.ToInt32(traveldate[0])).ToString();
                        string LastDate = LastDay + "/" + String.Join("/", traveldate).ToString();
                        FromDate = DateTime.ParseExact(MonthYear, "MM-yyyy", CultureInfo.InvariantCulture);
                        ToDate = DateTime.ParseExact(LastDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        DateTime DateNow = DateTime.Now;
                        if (DateNow.Month > 3)
                        {
                            string StartDate = "01/04/" + DateNow.Year;
                            FromDate = DateTime.ParseExact(StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            ToDate = DateTime.Now;
                        }
                        else
                        {
                            string StartDate = "01/04/" + (DateNow.Year - 1);
                            string EndDate = "31/03/" + DateNow.Year;
                            FromDate = DateTime.ParseExact(StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            ToDate = DateTime.ParseExact(EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                    }
                    Dictionary<long, IList<VehicleOverviewReport_SP>> VehicleOverviewReport_SPList = ts.GetVehicleOverviewReportListbySP(Campus, VehicleNo, VehicleType, FromDate, ToDate, page, rows, sidx, sord);
                    if (ExportType == "Excel")
                    {
                        base.ExptToXL(VehicleOverviewReport_SPList.FirstOrDefault().Value, "VehicleOverviewReport", item => new
                        {
                            item.Campus,
                            Vehicle_Type = item.VehicleType,
                            Vehicle_No = item.VehicleNo,
                            Total_No_Of_Trips = item.TotalNoOfTrip,
                            Total_No_Of_Trip_Kms = item.TotalDistance,
                            Total_Expenses = item.Expenses,
                        });
                        return new EmptyResult();
                    }
                    else if (VehicleOverviewReport_SPList != null && VehicleOverviewReport_SPList.Count > 0)
                    {
                        Dictionary<long, IList<VehicleOverviewReport_SP>> VehicleOverviewReport_SPList1 = ts.GetVehicleOverviewReportListbySP(Campus, VehicleNo, VehicleType, FromDate, ToDate, 0, rows, sidx, sord);
                        long totalrecords = VehicleOverviewReport_SPList1.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondata = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in VehicleOverviewReport_SPList.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                                        items.Id.ToString(),
                                        items.Campus,                                        
                                        items.VehicleNo,
                                        items.VehicleType,
                                        items.DriverOt.ToString(),
                                        items.HelperOt.ToString(),
                                        items.Fuel.ToString(),
                                        items.Maintenance.ToString(),
                                        items.Service.ToString(),
                                        items.FC.ToString(),
                                        items.Others.ToString(),                                                                                                                        
                                        items.TotalNoOfTrip.ToString(),
                                        items.TotalDistance.ToString(),
                                        items.Expenses.ToString(),                                       
                            }
                                    })
                        };
                        return Json(jsondata, JsonRequestBehavior.AllowGet);

                    }
                    else
                    {
                        var jsondata = new { rows = (new { cell = new string[] { } }) };
                        return Json(jsondata, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
