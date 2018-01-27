using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using TIPS.ServiceContract;
using TIPS.Entities.CanteenEntities;

namespace CMS.Controllers
{
    public class CanteenController : BaseController
    {
        public ActionResult CanteenMaster()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CanteenMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult CanteenUnitsMaster()
        {
            try
            {
                return PartialView();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CanteenMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult CanteenUnitsListJqGrid(string UnitCode, string Units, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                CanteenService ds = new CanteenService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrWhiteSpace(UnitCode)) { criteria.Add("UnitCode", UnitCode); }
                if (!string.IsNullOrWhiteSpace(Units)) { criteria.Add("Units", Units); }
                sord = sord == "desc" ? "Desc" : "Asc";
                Dictionary<long, IList<CanteenUnits>> CanteenUnits = ds.GetCanteenUnitslistWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (CanteenUnits != null && CanteenUnits.Count > 0)
                {
                    long totalrecords = CanteenUnits.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat1 = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,

                        rows = (from items in CanteenUnits.First().Value
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
                ExceptionPolicy.HandleException(ex, "CanteenMgmntPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddCanteenUnits(CanteenUnits su, string test)
        {
            try
            {
                su.UnitCode = su.UnitCode.Trim();
                CanteenService cs = new CanteenService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Units", su.Units);
                criteria.Add("UnitCode", su.UnitCode);
                Dictionary<long, IList<CanteenUnits>> CanteenUnits = cs.GetCanteenUnitslistWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                if (test == "edit")
                {
                    if (CanteenUnits != null && CanteenUnits.First().Value != null && CanteenUnits.First().Value.Count > 1)
                    {
                        //var script = @"ErrMsg(""Already Exists"");";
                        //return JavaScript(script);
                        return null;
                    }
                    else
                    {
                        ViewBag.flag = 1;
                        cs.CreateOrUpdateCanteenUnitsMaster(su);
                        return null;
                    }
                }
                else
                {
                    if (CanteenUnits != null && CanteenUnits.First().Value != null && CanteenUnits.First().Value.Count > 0)
                    {
                        var script = @"ErrMsg(""Already Exists"");";
                        return JavaScript(script);
                    }
                    else
                    {
                        su.Id = 0;
                        ViewBag.flag = 1;
                        long id = cs.CreateOrUpdateCanteenUnitsMaster(su);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CanteenMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult CanteenMaterialGroupMaster()
        {
            try
            {
                return PartialView();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CanteenMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult CanteenMaterialGroupMasterListJqGrid(string MaterialGroup, string MatGrpCode, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                CanteenService ds = new CanteenService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrWhiteSpace(MaterialGroup)) { criteria.Add("MaterialGroup", MaterialGroup); }
                if (!string.IsNullOrWhiteSpace(MatGrpCode)) { criteria.Add("MatGrpCode", MatGrpCode); }

                sord = sord == "desc" ? "Desc" : "Asc";
                Dictionary<long, IList<CanteenMaterialGroupMaster>> MaterialGroupMaster = ds.GetCanteenMaterialGroupMasterListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
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
                ExceptionPolicy.HandleException(ex, "CanteenMgmntPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddMaterialGroupMaster(CanteenMaterialGroupMaster mgm, string test)
        {
            try
            {
                mgm.MaterialGroup = mgm.MaterialGroup.Trim();
                CanteenService ss = new CanteenService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("MaterialGroup", mgm.MaterialGroup);
                criteria.Add("MatGrpCode", mgm.MatGrpCode);
                Dictionary<long, IList<CanteenMaterialGroupMaster>> Matgrp = ss.GetCanteenMaterialGroupMasterListWithPagingAndCriteria(0, 9999, null, null, criteria);
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
                ss.CreateOrUpdateCanteenMaterialGroupMaster(mgm);
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CanteenMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult CanteenMaterialsMaster()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CanteenMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult CanteenMaterialsMasterListJqGrid(string MaterialGroup,  string Material, string UnitCode, string ItemCode, string ItemLocation, string Notes, string IsActive, string ExptType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                CanteenService cs = new CanteenService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrWhiteSpace(MaterialGroup)) { criteria.Add("MaterialGroup", MaterialGroup); }
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
                Dictionary<long, IList<CanteenMaterialsMaster_vw>> SKUList = cs.GetCanteenMaterialsMasterlistWithPagingAndCriteriaUsingView(page - 1, rows, sord, sidx, criteria);
                if (SKUList != null && SKUList.Count > 0)
                {
                    if (ExptType == "Excel")
                    {
                        var List = SKUList.First().Value.ToList();
                        base.ExptToXL(List, "MaterialsList", (items => new
                        {
                            items.Id,
                            items.MaterialGroup,
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
                               items.Id.ToString(),items.MaterialGroupId.ToString(),items.MaterialGroup,items.Material,items.UnitCode,
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
                ExceptionPolicy.HandleException(ex, "CanteenMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult FillMaterialGroup()
        {
            try
            {
                CanteenService ss = new CanteenService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<long, IList<CanteenMaterialGroupMaster>> MaterialGroupList = ss.GetCanteenMaterialGroupMasterListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteria);
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
                ExceptionPolicy.HandleException(ex, "CanteenMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult Unitsddl()
        {
            try
            {
                CanteenService cs = new CanteenService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();

                Dictionary<long, IList<CanteenUnits>> CanteenUnitsList = cs.GetCanteenUnitslistWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteria);
                if (CanteenUnitsList != null && CanteenUnitsList.First().Value != null && CanteenUnitsList.First().Value.Count > 0)
                {
                    var CanteenUnits = (from u in CanteenUnitsList.First().Value
                                      where u.UnitCode != null
                                      select u.UnitCode).Distinct().ToList();
                    return Json(CanteenUnits, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CanteenMgmntPolicy");
                throw ex;
            }
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddMaterial(CanteenMaterialsMaster mm, string test)
        {
            try
            {
                mm.Material = mm.Material.Trim();
                CanteenService ss = new CanteenService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();

                CanteenMaterialGroupMaster mgm = new CanteenMaterialGroupMaster();
                mgm = ss.GetCanteenMaterialGroupById(mm.MaterialGroupId);
                string mg = mgm.MatGrpCode;
                string mgsplitted = mg.Substring(0, 3);
                criteria.Add("MaterialGroupId", mm.MaterialGroupId);
                criteria.Add("Material", mm.Material);
                Dictionary<long, IList<CanteenMaterialsMaster>> StoreUnits = ss.GetCanteenMaterialsMasterlistWithPagingAndCriteria(0, 9999, null, null, criteria);
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
                        ss.CreateOrUpdateCanteenMaterialsMaster(mm);
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
                        long id = ss.CreateOrUpdateCanteenMaterialsMaster(mm);
                        mm.ItemCode = mgsplitted.ToUpper() +  "000" + id;
                        ss.CreateOrUpdateCanteenMaterialsMaster(mm);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CanteenMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult CanteenFunctionaries()
        {
            try
            {
                return PartialView();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CanteenMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult CanteenSupplierListJqGrid1(string SupplierName, string CompanyName, string MobileNumber, string PhoneNumber, string Email, string TINNumber, string PANNumber, string IsPreferredSupplier, string Notes
          , string IsActive, string ExptType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                CanteenService cs = new CanteenService();
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
                Dictionary<long, IList<CanteenSupplierMaster>> storesuppliermasterlist = cs.GetCanteenSupplierMasterlistWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
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
                ExceptionPolicy.HandleException(ex, "CanteenMgmntPolicy");
                throw ex;
            }
        }


    }
}
