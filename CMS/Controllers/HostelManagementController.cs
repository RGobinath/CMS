using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TIPS.Entities.HostelMgntEntities;
using TIPS.Entities;
using TIPS.Service;
using TIPS.ServiceContract;
using TIPS.Entities.Assess.ReportCardClasses;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using TIPS.Entities.AdmissionEntities;

namespace CMS.Controllers
{
    public class HostelManagementController : BaseController
    {

        #region "Create Service Object Details"

        IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
        HostelMngtService hstMgntSerObj = new HostelMngtService();
        MastersService ms = new MastersService();
        AdmissionManagementService ads = new AdmissionManagementService();

        #endregion "End Service Object Details"

        #region "Landing Page"

        public ActionResult LandingPage()
        {
            return View();
        }
        public ActionResult JqgridLandingPage(HostelMasters_Vw obj, string type, int rows, string sord, string sidx, int page)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(obj.Campus)) { criteria.Add("Campus", obj.Campus); }
                if (!string.IsNullOrEmpty(obj.HostelName)) { criteria.Add("HostelName", obj.HostelName); }
                if (!string.IsNullOrEmpty(obj.HostelType)) { criteria.Add("HostelType", obj.HostelType); }
                Dictionary<long, IList<HostelMasters_Vw>> dcnStdntDtls = hstMgntSerObj.GetHostelMasters_VwLIKEListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
                //Dictionary<long, IList<HostelMasters_Vw>> dcnStdntDtls = hstMgntSerObj.GetHostelMasters_VwListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
                if (dcnStdntDtls != null && dcnStdntDtls.Count > 0)
                {
                    long totalRecords = dcnStdntDtls.FirstOrDefault().Key;
                    int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                    var jsonData = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalRecords,
                        rows = (
                        from items in dcnStdntDtls.FirstOrDefault().Value
                        select new
                        {
                            i = items.HstlMst_Id,
                            cell = new string[] 
                       { 
                           items.HstlMst_Id.ToString(), items.HostelName, items.HostelType, items.Campus, items.Rooms.ToString(), 
                           items.Beds.ToString(),items.InCharge
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
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "");
                throw ex;
            }

        }
        public ActionResult HostelDetlsPage(Int64 HstlMst_Id)
        {
            HostelMasters_Vw hmObj = hstMgntSerObj.GetHostelMasters_VwById(HstlMst_Id);
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("HstlMst_Id", HstlMst_Id);
            Dictionary<long, IList<StudHostelDtls_Vw>> dcnStdntDtls = hstMgntSerObj.GetStudHostelDtls_VwListWithPagingAndCriteria(0, 999999, string.Empty, string.Empty, criteria);
            if (dcnStdntDtls != null && dcnStdntDtls.FirstOrDefault().Value.Count > 0 && dcnStdntDtls.FirstOrDefault().Key > 0)
            {
                hmObj.CaptyUtilised = dcnStdntDtls.FirstOrDefault().Value.Count;
                hmObj.AvibleCapty = hmObj.Beds - dcnStdntDtls.FirstOrDefault().Value.Count;
            }
            else
            {
                hmObj.AvibleCapty = hmObj.Beds;
            }
            return View(hmObj);
        }
        public ActionResult NewRoomAllocationPage(string location)
        {
            ViewBag.lction = location;
            return View();
        }
        public JsonResult GetHostelName(string campus)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(campus)) { criteria.Add("Campus", campus); }
                Dictionary<long, IList<HostelMasters_Vw>> dcnStdntDtls = hstMgntSerObj.GetHostelMasters_VwListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
                if (dcnStdntDtls != null && dcnStdntDtls.First().Key > 0 && dcnStdntDtls.First().Value.Count > 0)
                {
                    var dcnStdntDtlsLst = (
                             from items in dcnStdntDtls.First().Value
                             where items.Campus != null
                             select new
                             {
                                 Text = items.HostelName,
                                 Value = items.HostelName
                             }).Distinct().ToList();
                    return Json(dcnStdntDtlsLst, JsonRequestBehavior.AllowGet);
                }
                var jsondat = new { rows = (new { cell = new string[] { } }) };
                return Json(jsondat, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public JsonResult GetFloorDetails(string campus, string hstName, string hstType)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(campus)) { criteria.Add("Campus", campus); }
                if (!string.IsNullOrEmpty(hstName)) { criteria.Add("HostelName", hstName); }
                if (!string.IsNullOrEmpty(hstType)) { criteria.Add("HostelType", hstType); }
                Dictionary<long, IList<HostelMasters_Vw>> dcnStdntDtls = hstMgntSerObj.GetHostelMasters_VwListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
                if (dcnStdntDtls != null && dcnStdntDtls.First().Key > 0 && dcnStdntDtls.First().Value.Count > 0)
                {
                    var dcnStdntDtlsLst = (
                             from items in dcnStdntDtls.First().Value
                             where items.Campus != null
                             select new
                             {
                                 Text = items.Floor,
                                 Value = items.Floor
                             }).Distinct().ToList();
                    return Json(dcnStdntDtlsLst, JsonRequestBehavior.AllowGet);
                }
                var jsondat = new { rows = (new { cell = new string[] { } }) };
                return Json(jsondat, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public JsonResult GetRoomListDetails(string campus, string hstName, string hstType, string floor)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(campus)) { criteria.Add("Campus", campus); }
                if (!string.IsNullOrEmpty(hstName)) { criteria.Add("HostelName", hstName); }
                if (!string.IsNullOrEmpty(hstType)) { criteria.Add("HostelType", hstType); }
                if (!string.IsNullOrEmpty(floor)) { criteria.Add("Floor", floor); }
                Dictionary<long, IList<HstlMgmt_HostelMaster>> AdminObj = hstMgntSerObj.GetHstMgntAdmissionFormListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
                if (AdminObj != null && AdminObj.First().Key > 0 && AdminObj.First().Value.Count > 0)
                {
                    //IList<HstlMgmt_HostelMaster> campusMsObj = AdminObj.FirstOrDefault().Value[0].HstlMst_Id;
                    criteria.Clear();
                    criteria.Add("HstlMst_Id", AdminObj.FirstOrDefault().Value[0].HstlMst_Id);
                    Dictionary<long, IList<HstlMgmt_RoomMaster>> dcnStdntDtls = hstMgntSerObj.GetHstlMgmt_RoomMasterListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
                    if (dcnStdntDtls != null && dcnStdntDtls.First().Key > 0 && dcnStdntDtls.First().Value.Count > 0)
                    {
                        var dcnStdntDtlsLst = (
                                 from items in dcnStdntDtls.First().Value
                                 select new
                                 {
                                     Text = items.RoomNumber,
                                     Value = items.RoomMst_Id
                                 }).Distinct().ToList();
                        return Json(dcnStdntDtlsLst, JsonRequestBehavior.AllowGet);
                    }
                }
                var jsondat = new { rows = (new { cell = new string[] { } }) };
                return Json(jsondat, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public JsonResult GetType(string hostelNm, string campus)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(hostelNm)) { criteria.Add("HostelName", hostelNm); }
                if (!string.IsNullOrEmpty(campus)) { criteria.Add("Campus", campus); }
                Dictionary<long, IList<HostelMasters_Vw>> dcnStdntDtls = hstMgntSerObj.GetHostelMasters_VwListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
                if (dcnStdntDtls != null && dcnStdntDtls.First().Key > 0 && dcnStdntDtls.First().Value.Count > 0)
                {
                    var dcnStdntDtlsLst = (
                             from items in dcnStdntDtls.First().Value
                             where items.Campus != null
                             select new
                             {
                                 Text = items.HostelType,
                                 Value = items.HostelType
                             }).Distinct().ToList();
                    return Json(dcnStdntDtlsLst, JsonRequestBehavior.AllowGet);
                }
                var jsondat = new { rows = (new { cell = new string[] { } }) };
                return Json(jsondat, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public JsonResult GetHosteIdbyHostelNameandType(string hstName, string hstType)
        {
            return Json(hstMgntSerObj.GetHosteIdValuebyHostelNameandType(hstName, hstType), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetRoomLst(Int64 hstId)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("HstlMst_Id", hstId);
                Dictionary<long, IList<HstlMgmt_RoomMaster>> dcnStdntDtls = hstMgntSerObj.GetHstlMgmt_RoomMasterListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
                if (dcnStdntDtls != null && dcnStdntDtls.First().Key > 0 && dcnStdntDtls.First().Value.Count > 0)
                {
                    var dcnStdntDtlsLst = (
                             from items in dcnStdntDtls.First().Value
                             select new
                             {
                                 Text = items.RoomNumber,
                                 Value = items.RoomMst_Id
                             }).Distinct().ToList();
                    return Json(dcnStdntDtlsLst, JsonRequestBehavior.AllowGet);
                }
                var jsondat = new { rows = (new { cell = new string[] { } }) };
                return Json(jsondat, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "");
                throw ex;
            }
        }
        public ActionResult GetRoomsDtls(Int64 rmNum)
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            if (rmNum > 0) { criteria.Add("RoomMst_Id", rmNum); }
            Dictionary<long, IList<HstlMgmt_BedMaster>> bedObj = hstMgntSerObj.GetHstlMgmt_BedMasterListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
            string cmpsLst = "<div class='col-xs-12'>";
            foreach (var item in bedObj.FirstOrDefault().Value)
            {
                criteria.Clear();
                criteria.Add("RoomMst_Id", item.RoomMst_Id);
                criteria.Add("BedNumber", item.BedNumber);
                Dictionary<long, IList<HstlMgmt_HostelDetails>> hstObj = hstMgntSerObj.GetHstlMgmt_HostelDetailsListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
                if (hstObj != null && hstObj.FirstOrDefault().Value.Count > 0 && hstObj.FirstOrDefault().Key > 0)
                {
                    cmpsLst += "<div class='col-sm-1' onclick=\"GetSelectedBedLst('" + item.BedMst_Id + "','" + item.BedNumber + "','" + item.HstlMst_Id + "');\"><span>(" + item.BedNumber + ")</span><img src='../../Images/Selected.png' /></div>";
                }
                else
                {
                    cmpsLst += "<div class='col-sm-1' onclick=\"GetBedLst('" + item.BedMst_Id + "','" + item.BedNumber + "','" + item.HstlMst_Id + "');\"><span>(" + item.BedNumber + ")</span><img src='../../Images/Notselected.png' /></div>";
                }

            }
            cmpsLst += "</div>";
            return Json(cmpsLst, JsonRequestBehavior.AllowGet);
        }

        #endregion "End"

        #region "MASTER DETAILS"

        public ActionResult HostelMasterPage()
        {
            return View();

        }
        public ActionResult HostelMasterPageJqgrid(HstlMgmt_HostelMaster hs, int rows, string sord, string sidx, int page)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(hs.HostelName)) { criteria.Add("HostelName", hs.HostelName); }
                if (!string.IsNullOrEmpty(hs.HostelType)) { criteria.Add("HostelType", hs.HostelType); }
                if (!string.IsNullOrEmpty(hs.Campus)) { criteria.Add("Campus", hs.Campus); }
                if (!string.IsNullOrEmpty(hs.Floor)) { criteria.Add("Floor", hs.Floor); }
                Dictionary<long, IList<HstlMgmt_HostelMaster>> AdminObj = hstMgntSerObj.GetHstMgntAdmissionFormListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
                IList<HstlMgmt_HostelMaster> campusMsObj = AdminObj.FirstOrDefault().Value;
                if (AdminObj != null && AdminObj.FirstOrDefault().Value.Count > 0 && AdminObj.FirstOrDefault().Key > 0)
                {
                    long totalRecords = AdminObj.FirstOrDefault().Key;
                    int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                    var jsonData = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalRecords,
                        rows = (
                        from items in AdminObj.FirstOrDefault().Value
                        select new
                        {
                            i = items.HstlMst_Id,
                            cell = new string[] 
                       {
                        items.HstlMst_Id.ToString(),
                        items.HostelName,
                        items.HostelType,
                        items.Floor,
                        items.Campus,
                        items.InCharge,
                        items.CreatedBy,
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
        public ActionResult RoomMasterPageJqgrid(string hostelId, int rows, string sord, string sidx, int page)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("HstlMst_Id", Convert.ToInt64(hostelId));
                Dictionary<long, IList<HstlMgmt_RoomMaster>> AdminObj = hstMgntSerObj.GetHstlMgmt_RoomMasterListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);

                if (AdminObj != null && AdminObj.FirstOrDefault().Value.Count > 0 && AdminObj.FirstOrDefault().Key > 0)
                {
                    long totalRecords = AdminObj.FirstOrDefault().Key;
                    int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                    var jsonData = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalRecords,
                        rows = (
                        from items in AdminObj.FirstOrDefault().Value
                        select new
                        {
                            i = items.RoomMst_Id,
                            cell = new string[] 
                       {
                           items.RoomMst_Id.ToString(),
                        items.HstlMst_Id.ToString(),
                        items.RoomNumber.ToString(),
                        items.CreatedBy,
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
        public ActionResult BedMasterPageJqgrid(string RoomMst_Id, int rows, string sord, string sidx, int page)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("RoomMst_Id", Convert.ToInt64(RoomMst_Id));
                Dictionary<long, IList<HstlMgmt_BedMaster>> bedObj = hstMgntSerObj.GetHstlMgmt_BedMasterListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
                if (bedObj != null && bedObj.FirstOrDefault().Value.Count > 0 && bedObj.FirstOrDefault().Key > 0)
                {
                    long totalRecords = bedObj.FirstOrDefault().Key;
                    int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                    var jsonData = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalRecords,
                        rows = (
                        from items in bedObj.FirstOrDefault().Value
                        select new
                        {
                            i = items.BedMst_Id,
                            cell = new string[] 
                       {
                           items.BedMst_Id.ToString(),
                           items.RoomMst_Id.ToString(),
                        items.BedNumber.ToString(),
                        items.IsAllocate.ToString(),
                        items.CreatedBy,
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
        public ActionResult AddMenu(HstlMgmt_HostelMaster hsObj, string edit)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ValidateUser())) return RedirectToAction("LogOff", "Account");
                else
                {
                    //if (hsObj.HstlMst_Id == 0 && string.IsNullOrEmpty(edit))
                    //{
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    if (!string.IsNullOrEmpty(hsObj.HostelName)) { criteria.Add("HostelName", hsObj.HostelName); }
                    if (!string.IsNullOrEmpty(hsObj.HostelType)) { criteria.Add("HostelType", hsObj.HostelType); }
                    if (!string.IsNullOrEmpty(hsObj.Campus)) { criteria.Add("Campus", hsObj.Campus); }
                    if (!string.IsNullOrEmpty(hsObj.Floor)) { criteria.Add("Floor", hsObj.Floor); }
                    Dictionary<long, IList<HstlMgmt_HostelMaster>> AdminObj = hstMgntSerObj.GetHstMgntAdmissionFormListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
                    if ((AdminObj != null && AdminObj.FirstOrDefault().Value.Count > 0 && AdminObj.FirstOrDefault().Key > 0))
                    {
                        var script = @"SucessMsg(""Already Exists"");";
                        return JavaScript(script);
                    }

                    if (hsObj.HstlMst_Id == 0)
                    {
                        hsObj.CreatedBy = ValidateUser();
                        hsObj.DateCreated = DateTime.Now;
                    }
                    else
                    {
                        HstlMgmt_HostelMaster bed = hstMgntSerObj.GetHstlMgmt_HostelMasterbyId(hsObj.HstlMst_Id);
                        hsObj.ModifiedBy = ValidateUser();
                        hsObj.DateCreated = bed.DateCreated;
                        hsObj.DateModified = DateTime.Now;
                    }
                    hstMgntSerObj.CreateOrUpdateHstlMgmt_HostelMaster(hsObj);
                }
                var scriptrtn = @"SucessMsg(""Added  Successfully"");";
                return JavaScript(scriptrtn);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "SystemMgntPolicy");
                throw ex;
            }
        }
        public ActionResult DeleteMenu(long Id)
        {
            try
            {
                //string userId = ValidateUser();
                if (string.IsNullOrWhiteSpace(ValidateUser())) return RedirectToAction("LogOff", "Account");
                else
                {
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("HstlMst_Id", Id);
                    Dictionary<long, IList<HstlMgmt_HostelMaster>> AdminObj = hstMgntSerObj.GetHstMgntAdmissionFormListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
                    Dictionary<long, IList<HstlMgmt_RoomMaster>> roomObj = hstMgntSerObj.GetHstlMgmt_RoomMasterListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);

                    if ((AdminObj != null && AdminObj.FirstOrDefault().Value.Count > 0 && AdminObj.FirstOrDefault().Key > 0))
                    {
                        if ((roomObj != null && roomObj.FirstOrDefault().Value.Count > 0 && roomObj.FirstOrDefault().Key > 0))
                        {
                            foreach (var rObj in roomObj.FirstOrDefault().Value.ToList())
                            {
                                criteria.Clear();
                                criteria.Add("RoomMst_Id", rObj.RoomMst_Id);
                                Dictionary<long, IList<HstlMgmt_BedMaster>> bedObj = hstMgntSerObj.GetHstlMgmt_BedMasterListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
                                if ((bedObj != null && bedObj.FirstOrDefault().Value.Count > 0 && bedObj.FirstOrDefault().Key > 0))
                                {
                                    hstMgntSerObj.DeleteBedMasterAll(bedObj.FirstOrDefault().Value.ToList());
                                }
                            }
                            hstMgntSerObj.DeleteRoomMasterAll(roomObj.FirstOrDefault().Value.ToList());
                        }
                        hstMgntSerObj.DeleteHostalMasterAll(AdminObj.FirstOrDefault().Value.ToList());
                    }
                    var script = @"SucessMsg(""Deleted  Successfully"");";
                    return JavaScript(script);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "SystemMgntPolicy");
                throw ex;
            }
        }
        public ActionResult AddSubMenus(HstlMgmt_RoomMaster roomObj, int ids)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ValidateUser())) return RedirectToAction("LogOff", "Account");
                else
                {
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    if (ids > 0) { criteria.Add("HstlMst_Id", Convert.ToInt64(ids)); }
                    if (!string.IsNullOrEmpty(roomObj.RoomNumber)) { criteria.Add("RoomNumber", roomObj.RoomNumber); }
                    //if (roomObj.RoomNumber > 0) { criteria.Add("RoomNumber", roomObj.RoomNumber); }
                    Dictionary<long, IList<HstlMgmt_RoomMaster>> roomObjLst = hstMgntSerObj.GetHstlMgmt_RoomMasterListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
                    if ((roomObjLst != null && roomObjLst.FirstOrDefault().Value.Count > 0 && roomObjLst.FirstOrDefault().Key > 0))
                    {
                        var script = @"SucessMsg(""Already Exists"");";
                        return JavaScript(script);
                    }
                    roomObj.HstlMst_Id = ids;
                    if (roomObj.RoomMst_Id == 0)
                    {
                        roomObj.CreatedBy = ValidateUser();
                        roomObj.DateCreated = DateTime.Now;
                    }
                    else
                    {
                        HstlMgmt_RoomMaster bed = hstMgntSerObj.GetHstlMgmt_RoomMasterbyId(roomObj.RoomMst_Id);
                        roomObj.ModifiedBy = ValidateUser();
                        roomObj.DateCreated = bed.DateCreated;
                        roomObj.DateModified = DateTime.Now;
                    }

                    hstMgntSerObj.CreateOrUpdateHstlMgmt_RoomMaster(roomObj);
                }
                var scriptrtn = @"SucessMsg(""Added  Successfully"");";
                return JavaScript(scriptrtn);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "SystemMgntPolicy");
                throw ex;
            }
        }
        public ActionResult DeleteSubMenus(long Id)
        {
            try
            {
                //string userId = ValidateUser();
                if (string.IsNullOrWhiteSpace(ValidateUser())) return RedirectToAction("LogOff", "Account");
                else
                {
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("RoomMst_Id", Id);
                    Dictionary<long, IList<HstlMgmt_RoomMaster>> roomObjLst = hstMgntSerObj.GetHstlMgmt_RoomMasterListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
                    if ((roomObjLst != null && roomObjLst.FirstOrDefault().Value.Count > 0 && roomObjLst.FirstOrDefault().Key > 0))
                    {
                        foreach (var rObj in roomObjLst.FirstOrDefault().Value.ToList())
                        {
                            criteria.Clear();
                            criteria.Add("RoomMst_Id", rObj.RoomMst_Id);
                            Dictionary<long, IList<HstlMgmt_BedMaster>> bedObj = hstMgntSerObj.GetHstlMgmt_BedMasterListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
                            if ((bedObj != null && bedObj.FirstOrDefault().Value.Count > 0 && bedObj.FirstOrDefault().Key > 0))
                            {
                                hstMgntSerObj.DeleteBedMasterAll(bedObj.FirstOrDefault().Value.ToList());
                            }
                        }
                        hstMgntSerObj.DeleteRoomMasterAll(roomObjLst.FirstOrDefault().Value.ToList());
                    }
                    var script = @"SucessMsg(""Deleted  Successfully"");";
                    return JavaScript(script);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "SystemMgntPolicy");
                throw ex;
            }
        }
        public ActionResult AddThirdLevelMenus(HstlMgmt_BedMaster bedObj, int ids)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ValidateUser())) return RedirectToAction("LogOff", "Account");
                else
                {
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    if (ids > 0) { criteria.Add("RoomMst_Id", Convert.ToInt64(ids)); }
                    if (!string.IsNullOrEmpty(bedObj.BedNumber)) { criteria.Add("BedNumber", bedObj.BedNumber); }
                    //if (bedObj.BedNumber > 0) { criteria.Add("BedNumber", bedObj.BedNumber); }
                    Dictionary<long, IList<HstlMgmt_BedMaster>> bedObjLst = hstMgntSerObj.GetHstlMgmt_BedMasterListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);

                    if ((bedObjLst != null && bedObjLst.FirstOrDefault().Value.Count > 0 && bedObjLst.FirstOrDefault().Key > 0))
                    {
                        var script = @"SucessMsg(""Already Exists"");";
                        return JavaScript(script);
                    }

                    HstlMgmt_RoomMaster room = hstMgntSerObj.GetHstlMgmt_RoomMasterbyId(Convert.ToInt64(ids));
                    bedObj.HstlMst_Id = room.HstlMst_Id;
                    bedObj.RoomMst_Id = ids;
                    if (bedObj.BedMst_Id == 0)
                    {
                        bedObj.CreatedBy = ValidateUser();
                        bedObj.DateCreated = DateTime.Now;
                    }
                    else
                    {
                        HstlMgmt_BedMaster bed = hstMgntSerObj.GetHstlMgmt_BedMasterbyId(bedObj.BedMst_Id);
                        bedObj.ModifiedBy = ValidateUser();
                        bedObj.DateCreated = bed.DateCreated;
                        bedObj.DateModified = DateTime.Now;
                    }
                    hstMgntSerObj.CreateOrUpdateHstlMgmt_BedMaster(bedObj);
                }
                var scriptrtn = @"SucessMsg(""Added  Successfully"");";
                return JavaScript(scriptrtn);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "SystemMgntPolicy");
                throw ex;
            }
        }
        public ActionResult DeleteThirdLevelSubMenus(long Id)
        {
            try
            {
                //string userId = ValidateUser();
                if (string.IsNullOrWhiteSpace(ValidateUser())) return RedirectToAction("LogOff", "Account");
                else
                {
                    Dictionary<string, object> criteria = new Dictionary<string, object>();

                    criteria.Add("BedMst_Id", Id);
                    Dictionary<long, IList<HstlMgmt_BedMaster>> bedObj = hstMgntSerObj.GetHstlMgmt_BedMasterListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
                    if ((bedObj != null && bedObj.FirstOrDefault().Value.Count > 0 && bedObj.FirstOrDefault().Key > 0))
                    {
                        hstMgntSerObj.DeleteBedMasterAll(bedObj.FirstOrDefault().Value.ToList());
                    }
                    var script = @"SucessMsg(""Deleted  Successfully"");";
                    return JavaScript(script);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "SystemMgntPolicy");
                throw ex;
            }
        }

        #endregion "END"

        #region " ROOM ALLOTMENT "

        public ActionResult RoomAllotment()
        {
            return View();
        }
        public ActionResult JqgridRoomAllotment(RoomAllotment rmObj, int rows, string sord, string sidx, int page)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(rmObj.Name)) { criteria.Add("Name", rmObj.Name); }
                if (!string.IsNullOrEmpty(rmObj.NewId)) { criteria.Add("NewId", rmObj.NewId); }
                if (!string.IsNullOrEmpty(rmObj.Grade)) { criteria.Add("Grade", rmObj.Grade); }
                if (!string.IsNullOrEmpty(rmObj.Section)) { criteria.Add("Section", rmObj.Section); }
                if (!string.IsNullOrEmpty(rmObj.AcademicYear)) { criteria.Add("AcademicYear", rmObj.AcademicYear); }
                if (!string.IsNullOrEmpty(rmObj.BoardingType)) { criteria.Add("BoardingType", rmObj.BoardingType); }
                if (!string.IsNullOrEmpty(rmObj.SIPNo)) { criteria.Add("SIPNo", rmObj.SIPNo); }

                Dictionary<long, IList<RoomAllotment>> dcnStdntDtls = hstMgntSerObj.GetRoomAllotmentLIKEListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
                if (dcnStdntDtls != null && dcnStdntDtls.Count > 0)
                {
                    long totalRecords = dcnStdntDtls.FirstOrDefault().Key;
                    int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                    var jsonData = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalRecords,
                        rows = (
                        from items in dcnStdntDtls.FirstOrDefault().Value
                        select new
                        {
                            i = items.Id.ToString(),
                            cell = new string[] 
                       { 
                           items.Id.ToString(),
                           items.StudID.ToString(),
                           items.Name, 
                           items.NewId,
                           items.Campus, 
                           items.Grade, 
                           items.Section,
                           items.AcademicYear,
                           items.HstlMst_Id.ToString(),
                           items.HostelName,
                           items.HostelType,
                           items.Floor,
                           items.BoardingType,
                           items.RoomMst_Id.ToString()=="0"?"":items.RoomMst_Id.ToString(),
                           items.BedMst_Id.ToString()=="0"?"":items.BedMst_Id.ToString(),
                           items.BedNumber.ToString()=="0"?"":items.BedNumber.ToString(),
                           items.SIPNo,
                           items.Room_Allotment,
                           items.ChangeRoomAllotment,
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
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "");
                throw ex;
            }

        }
        public ActionResult RoomBookingForm(string studId, string Flag)
        {
            ViewBag.StudID = studId;
            ViewBag.flag = Flag;
            if (Flag == "Allotted" || Flag == "ChangeRoomAllotment")
            {
                HstlMgmt_HostelDetails hsRec = hstMgntSerObj.GetHstlMgmt_HostelDetailsbyStudentId(Convert.ToInt64(studId));
                ViewBag.Hname = hsRec.HostelName;
                ViewBag.Htype = hsRec.HostelType;
                ViewBag.RNum = hsRec.RoomNumber;
                ViewBag.Floor = hsRec.Floor;
                ViewBag.BNum = hsRec.BedNumber;
            }

            return View();
        }
        public ActionResult RoomAllocation(string hstName, string hstType, string hstCampus, string hstFloor, Int64 hstId, Int64 AvbRoomsId, string roomNum, Int64 bedId, string bedNum, Int64 studID, string dateofjoining, string sipno, string Flag)
        {
            StudentTemplate stud = ads.GetStudentTemplateDetailsById(studID);
            HstlMgmt_HostelDetails hsRec = hstMgntSerObj.GetHstlMgmt_HostelDetailsbyStudentId(studID);
            if (Flag == "ChangeRoomAllocation")
            {
                SaveChangeRoomAllocation(studID, stud, hsRec);
                hstMgntSerObj.RemoveExistingRoomInfo(hsRec); // It's Delete the Existing Records from the Hostel Details by hostel ID " HostDts_Id "
                hsRec = null;
            }

            if (hsRec == null)
            {
                HstlMgmt_HostelDetails createNew = new HstlMgmt_HostelDetails();
                createNew.HostDts_Id = 0;
                createNew.HostelName = hstName;
                createNew.HostelType = hstType;
                createNew.Campus = hstCampus;
                createNew.HstlMst_Id = hstId;
                createNew.Floor = hstFloor;
                createNew.RoomMst_Id = AvbRoomsId;
                createNew.RoomNumber = roomNum;

                createNew.BedMst_Id = bedId;
                createNew.IsRoomAllocate = true;
                createNew.BedNumber = bedNum;
                createNew.Stud_Id = studID; // get Id from the student template table...
                createNew.Name = stud.Name;
                createNew.NewId = stud.NewId;
                if (Flag == "ChangeRoomAllocation")
                {
                    createNew.DateOfJoining = DateTime.Now;
                }
                else
                {
                    createNew.DateOfJoining = ConvertDateMonthToMonthDate(dateofjoining);
                    createNew.SIPNo = sipno;
                }

                createNew.Grade = stud.Grade;  // Get Records from the Student template table...
                createNew.Section = stud.Section;
                createNew.AcademicYear = stud.AcademicYear;
                createNew.BoardingType = stud.BoardingType;

                createNew.CreatedBy = ValidateUser();
                createNew.DateCreated = DateTime.Now;
                hstMgntSerObj.CreateOrUpdateHstlMgmt_HostelDetails(createNew);
                return Json(null, JsonRequestBehavior.AllowGet);
            }


            return Json("Already exists", JsonRequestBehavior.AllowGet);
        }
        private void SaveChangeRoomAllocation(long studID, StudentTemplate stud, HstlMgmt_HostelDetails hsRec)
        {
            try
            {
                ChangeRoomAllotment chang = new ChangeRoomAllotment();
                chang.Id = 0;
                chang.Before_HostDts_Id = hsRec.HostDts_Id;
                chang.Before_HostelName = hsRec.HostelName;
                chang.Before_HostelType = hsRec.HostelType;
                chang.Before_Campus = hsRec.Campus;
                chang.Before_HstlMst_Id = hsRec.HstlMst_Id;
                chang.Before_Floor = hsRec.Floor;
                chang.Before_RoomMst_Id = hsRec.RoomMst_Id;
                chang.Before_RoomNumber = hsRec.RoomNumber;
                chang.Before_BedMst_Id = hsRec.BedMst_Id;
                chang.Before_BedNumber = hsRec.BedNumber;
                chang.Stud_Id = studID; // get Id from the student template table...
                chang.DateCreated = DateTime.Now;
                chang.CreatedBy = ValidateUser();
                chang.DateCreated = DateTime.Now;
                hstMgntSerObj.CreateOrUpdateChangeRoomAllotment(chang);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private DateTime ConvertDateMonthToMonthDate(string DateParam)
        {
            try
            {
                IFormatProvider culture = new System.Globalization.CultureInfo("fr-FR", true);

                // Alternate choice: If the string has been input by an end user, you might  
                // want to format it according to the current culture: 
                // IFormatProvider culture = System.Threading.Thread.CurrentThread.CurrentCulture;
                DateTime dt2 = DateTime.Parse(DateParam, culture, System.Globalization.DateTimeStyles.AssumeLocal);
                return dt2;
                //DateTime.Parse(DateParam, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion " END "

        #region "Material Damage"


        public ActionResult MaterialDamage()
        {
            return View();
        }
        public ActionResult JqgridMaterialDamage(HstMgnt_MaterialDamage_Vw hsObj, int rows, string sord, string sidx, int page)
        {
            try
            {

                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(hsObj.Name)) { criteria.Add("Name", hsObj.Name); }
                if (!string.IsNullOrEmpty(hsObj.NewId)) { criteria.Add("NewId", hsObj.NewId); }
                if (!string.IsNullOrEmpty(hsObj.DetailsOfIncident)) { criteria.Add("DetailsOfIncident", hsObj.DetailsOfIncident); }
                if (hsObj.Amount > 0) { criteria.Add("Amount", hsObj.Amount); }
                if (!string.IsNullOrEmpty(hsObj.Remarks)) { criteria.Add("Remarks", hsObj.Remarks); }
                // if (hsObj.DateOfIncident.ToString() != "1/1/0001 12:00:00 AM") { criteria.Add("DateOfIncident", hsObj.DateOfIncident); }
                if (!string.IsNullOrEmpty(hsObj.DummyRec)) { criteria.Add("DateOfIncident", ConvertDateMonthToMonthDate(hsObj.DummyRec)); }

                Dictionary<long, IList<HstMgnt_MaterialDamage_Vw>> dcnStdntDtls = hstMgntSerObj.GetHstMgnt_MaterialDamage_VwLIKEListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);

                if (dcnStdntDtls != null && dcnStdntDtls.Count > 0)
                {
                    List<HstMgnt_MaterialDamage_Vw> dcnLst = dcnStdntDtls.FirstOrDefault().Value.Where(e => e.MDId > 0).ToList();

                    long totalRecords = dcnLst.Count;
                    int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                    var jsonData = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalRecords,
                        rows = (
                        from items in dcnLst // display the list values
                        select new
                        {
                            i = items.Id.ToString(),
                            cell = new string[] 
                       { 
                           items.Id.ToString(),
                           items.MDId.ToString(),
                           items.Stud_Id.ToString(),
                           items.Name,
                           items.NewId,
                            items.DateOfIncident.ToString("dd/MM/yyy"), 
                        
                           items.DetailsOfIncident,
                           items.Amount.ToString(), 
                           items.Remarks, 
                              items.DateOfIncident.ToString("dd/MM/yyy"), 
                          
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
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "");
                throw ex;
            }

        }
        public ActionResult SaveAndEditMaterialDamage(HstMgnt_MaterialDamage_Vw damObj, string edit)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ValidateUser())) return RedirectToAction("LogOff", "Account");
                else
                {
                    StudentTemplate stud = ads.GetStudentDetailsByNewId(damObj.NewId);
                    HstMgnt_MaterialDamage mdObj = new HstMgnt_MaterialDamage();
                    mdObj.Stud_Id = stud.Id;
                    mdObj.DateOfIncident = ConvertDateMonthToMonthDate(damObj.DummyRec);
                    mdObj.DetailsOfIncident = damObj.DetailsOfIncident;
                    mdObj.Remarks = damObj.Remarks;
                    mdObj.Amount = damObj.Amount;
                    mdObj.DateCreated = DateTime.Now;
                    mdObj.CreatedBy = ValidateUser();
                    hstMgntSerObj.CreateOrUpdateHstMgnt_MaterialDamage(mdObj);
                }
                var scriptrtn = @"SucessMsg(""Added  Successfully"");";
                return JavaScript(scriptrtn);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "SystemMgntPolicy");
                throw ex;
            }
        }
        public ActionResult DeleteMaterialDamage(long Id)
        {
            try
            {
                HstMgnt_MaterialDamage hsObj = hstMgntSerObj.GetHstMgnt_MaterialDamageById(Id);
                hstMgntSerObj.DeleteHstMgnt_MaterialDamage(hsObj);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult GetStudentNameWithNewIds(string term)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Name", term);
                //Dictionary<long, IList<RoomAllotment>> dcnStdntDtls = hstMgntSerObj.GetRoomAllotmentLIKEListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
                Dictionary<long, IList<HstMgnt_MaterialDamage_Vw>> dcnStdntDtls = hstMgntSerObj.GetHstMgnt_MaterialDamage_VwLIKEListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);

                if (dcnStdntDtls != null && dcnStdntDtls.Count > 0)
                {
                    var UserIds = (from u in dcnStdntDtls.First().Value
                                   where u.Name != null && u.NewId != null
                                   select u.Name + "/" + u.NewId).Distinct().ToList();
                    return Json(UserIds, JsonRequestBehavior.AllowGet);
                }
                return Json(string.Empty, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }

        }

        #endregion " END "

        #region "Medical Expenses"

        public ActionResult MedicalExpenses()
        {
            return View();
        }
        public ActionResult JqgridMedicalExpenses(HstlMgmt_MedicalExpenses hmObj, int rows, string sord, string sidx, int page)
        {
            try
            {

                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(hmObj.Name)) { criteria.Add("Name", hmObj.Name); }
                if (!string.IsNullOrEmpty(hmObj.NewId)) { criteria.Add("NewId", hmObj.NewId); }
                if (hmObj.Amount > 0) { criteria.Add("Amount", hmObj.Amount); }
                if (!string.IsNullOrEmpty(hmObj.Remarks)) { criteria.Add("Remarks", hmObj.Remarks); }
                if (!string.IsNullOrEmpty(hmObj.DummyRec)) { criteria.Add("DateOfIllness", ConvertDateMonthToMonthDate(hmObj.DummyRec)); }
                Dictionary<long, IList<HstlMgmt_MedicalExpenses_Vw>> dcnStdntDtls = hstMgntSerObj.GetHstlMgmt_MedicalExpenses_VwLIKEListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);

                if (dcnStdntDtls != null && dcnStdntDtls.Count > 0)
                {
                    List<HstlMgmt_MedicalExpenses_Vw> dcnLst = dcnStdntDtls.FirstOrDefault().Value.Where(e => e.MainID > 0).ToList();

                    long totalRecords = dcnLst.Count;
                    int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                    var jsonData = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalRecords,
                        rows = (
                        from items in dcnLst // display the list values
                        select new
                        {
                            i = items.Id.ToString(),
                            cell = new string[] 
                       { 
                           items.Id.ToString(),
                           items.MainID.ToString(),
                           items.Stud_Id.ToString(),
                           items.Name,
                           items.NewId,
                           items.DateOfIllness.ToString("dd/MM/yyy"), 
                           items.Amount.ToString(), 
                           items.Remarks, 
                           items.DateOfIllness.ToString("dd/MM/yyy"), 
                          
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
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "");
                throw ex;
            }

        }
        public ActionResult SaveAndEditMedicalExpenses(HstlMgmt_MedicalExpenses_Vw damObj, string edit)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ValidateUser())) return RedirectToAction("LogOff", "Account");
                else
                {
                    StudentTemplate stud = ads.GetStudentDetailsByNewId(damObj.NewId);
                    HstlMgmt_MedicalExpenses mdObj = new HstlMgmt_MedicalExpenses();
                    mdObj.Stud_Id = stud.Id;
                    mdObj.DateOfIllness = ConvertDateMonthToMonthDate(damObj.DummyRec);
                    mdObj.Remarks = damObj.Remarks;
                    mdObj.Amount = damObj.Amount;
                    mdObj.DateCreated = DateTime.Now;
                    mdObj.CreatedBy = ValidateUser();
                    hstMgntSerObj.CreateOrUpdateHstlMgmt_MedicalExpenses(mdObj);
                }
                var scriptrtn = @"SucessMsg(""Added  Successfully"");";
                return JavaScript(scriptrtn);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "SystemMgntPolicy");
                throw ex;
            }
        }
        public ActionResult DeleteMedicalExpenses(long Id)
        {
            try
            {
                HstlMgmt_MedicalExpenses hsObj = hstMgntSerObj.GetHstlMgmt_MedicalExpensesById(Id);
                hstMgntSerObj.DeleteHstlMgmt_MedicalExpenses(hsObj);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion " END "

        #region " Food Expenses"

        public ActionResult FoodExpenses()
        {
            return View();
        }
        public ActionResult JqgridFoodExpenses(HstlMgmt_FoodExpenses hmObj, int rows, string sord, string sidx, int page)
        {
            try
            {

                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(hmObj.Name)) { criteria.Add("Name", hmObj.Name); }
                if (!string.IsNullOrEmpty(hmObj.NewId)) { criteria.Add("NewId", hmObj.NewId); }
                if (hmObj.Amount > 0) { criteria.Add("Amount", hmObj.Amount); }
                if (!string.IsNullOrEmpty(hmObj.Venue)) { criteria.Add("Venue", hmObj.Venue); }
                if (!string.IsNullOrEmpty(hmObj.DummyRec)) { criteria.Add("Date", ConvertDateMonthToMonthDate(hmObj.DummyRec)); }
                Dictionary<long, IList<HstlMgmt_FoodExpenses_Vw>> dcnStdntDtls = hstMgntSerObj.GetHstlMgmt_FoodExpenses_VwLIKEListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);

                if (dcnStdntDtls != null && dcnStdntDtls.Count > 0)
                {
                    List<HstlMgmt_FoodExpenses_Vw> dcnLst = dcnStdntDtls.FirstOrDefault().Value.Where(e => e.MainID > 0).ToList();
                    long totalRecords = dcnLst.Count;
                    int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                    var jsonData = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalRecords,
                        rows = (
                        from items in dcnLst // display the list values
                        select new
                        {
                            i = items.Id.ToString(),
                            cell = new string[] 
                       { 
                           items.Id.ToString(),
                           items.MainID.ToString(),
                           items.Stud_Id.ToString(),
                           items.Name,
                           items.NewId,
                           items.Date.ToString("dd/MM/yyy"), 
                           items.Amount.ToString(), 
                           items.Venue, 
                           items.Date.ToString("dd/MM/yyy"), 
                          
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
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "");
                throw ex;
            }

        }
        public ActionResult SaveAndEditFoodExpenses(HstlMgmt_FoodExpenses_Vw damObj, string edit)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ValidateUser())) return RedirectToAction("LogOff", "Account");
                else
                {
                    StudentTemplate stud = ads.GetStudentDetailsByNewId(damObj.NewId);
                    HstlMgmt_FoodExpenses mdObj = new HstlMgmt_FoodExpenses();
                    mdObj.Stud_Id = stud.Id;
                    mdObj.Date = ConvertDateMonthToMonthDate(damObj.DummyRec);
                    mdObj.Venue = damObj.Venue;
                    mdObj.Amount = damObj.Amount;
                    mdObj.AcademicYear = damObj.AcademicYear;
                    mdObj.DateCreated = DateTime.Now;
                    mdObj.CreatedBy = ValidateUser();
                    hstMgntSerObj.CreateOrUpdateHstlMgmt_FoodExpenses(mdObj);
                }
                var scriptrtn = @"SucessMsg(""Added  Successfully"");";
                return JavaScript(scriptrtn);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "SystemMgntPolicy");
                throw ex;
            }
        }
        public ActionResult DeleteFoodExpenses(long Id)
        {
            try
            {
                HstlMgmt_FoodExpenses hsObj = hstMgntSerObj.GetHstlMgmt_FoodExpensesById(Id);
                hstMgntSerObj.DeleteHstlMgmt_FoodExpenses(hsObj);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion " END "


    }
}
