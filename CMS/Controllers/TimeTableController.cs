using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TIPS.ServiceContract;
using TIPS.Service;
using TIPS.Entities;
using TIPS.Entities.TimeTableEntities;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace CMS.Controllers
{
    public class TimeTableController : BaseController
    {
        //
        // GET: /TimeTable/
        TimeTablesService timeTblSrvc = new TimeTablesService();
        IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
        MastersService ms = new MastersService();
        Dictionary<string, object> criteria = new Dictionary<string, object>();
        public ActionResult TimeTable()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogIn", "Account");
                else
                {
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<long, IList<CampusMaster>> CampusMaster = ms.GetCampusMasterListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                    criteria.Clear();
                    Dictionary<long, IList<GradeMaster>> GradeMaster = ms.GetGradeMasterListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                    Dictionary<long, IList<SectionMaster>> SectionMaster = ms.GetSectionMasterListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                    ViewBag.campusddl = CampusMaster.First().Value;
                    ViewBag.gradeddl1 = GradeMaster.First().Value;
                    ViewBag.sectionddl = SectionMaster.First().Value;
                    //ViewBag.campusddl = "IB MAIN";
                    //ViewBag.gradeddl1 = "VI";
                    //ViewBag.sectionddl = "A";
                    return View();
                }
            }
            catch (Exception ex)
            {
                //ExceptionPolicy.HandleException(ex, );
                throw ex;
            }
        }

        public ActionResult TimeTableCampusGradeSection(string Cam, string Grd, string Sec)
        {
            ViewBag.campusddl = Cam;
            ViewBag.gradeddl1 = Grd;
            ViewBag.sectionddl = Sec;
            return View();
        }

        //public ActionResult ShowTimeTable(string cam, string grd, string sec)
        //{
        //    try
        //    {
        //        Dictionary<string, object> criteria = new Dictionary<string, object>();
        //        criteria.Add("Campus", cam);
        //        criteria.Add("Grade", grd);
        //        criteria.Add("Section", sec);
        //        Dictionary<long, IList<TimeTable>> TimeTableList = timeTblSrvc.GetTimeTableListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);

        //        //ttm = timeTblSrvc.GetTimeTableByCamGraSec("IB MAIN", "VI", "A");
        //        string table = string.Empty;

        //        if (TimeTableList != null && TimeTableList.Count > 0 && TimeTableList.FirstOrDefault().Key > 0 && TimeTableList.FirstOrDefault().Value != null)
        //        {
        //            List<TimeTable> ttm = new List<TimeTable>();
        //            table = table + "<table class='table table-bordered'>";
        //            ttm = (from p in TimeTableList.FirstOrDefault().Value
        //                   where p.Day == "Monday"
        //                   select p).ToList();
        //            if (ttm.Count != 0)
        //            {
        //                table = table + "<tr>";
        //                if (!string.IsNullOrEmpty(ttm[0].Period1))
        //                    table = table + " <td class=" + ttm[0].Period1 + "><br/><br/>" + ttm[0].Period1 + "</td>";
        //                if (!string.IsNullOrEmpty(ttm[0].Period2))
        //                    table = table + " <td class=" + ttm[0].Period2 + "><br/><br/>" + ttm[0].Period2 + "</td>";
        //                if (!string.IsNullOrEmpty(ttm[0].Period3))
        //                    table = table + " <td class=" + ttm[0].Period3 + "><br/><br/>" + ttm[0].Period3 + "</td>";
        //                if (!string.IsNullOrEmpty(ttm[0].Period4))
        //                    table = table + " <td class=" + ttm[0].Period4 + "><br/><br/>" + ttm[0].Period4 + "</td>";
        //                if (!string.IsNullOrEmpty(ttm[0].Period5))
        //                    table = table + " <td class=" + ttm[0].Period5 + "><br/><br/>" + ttm[0].Period5 + "</td>";
        //                if (!string.IsNullOrEmpty(ttm[0].Period6))
        //                    table = table + " <td class=" + ttm[0].Period6 + "><br/><br/>" + ttm[0].Period6 + "</td>";
        //                if (!string.IsNullOrEmpty(ttm[0].Period7))
        //                    table = table + " <td class=" + ttm[0].Period7 + "><br/><br/>" + ttm[0].Period7 + "</td>";
        //                if (!string.IsNullOrEmpty(ttm[0].Period8))
        //                    table = table + " <td class=" + ttm[0].Period8 + "><br/><br/>" + ttm[0].Period8 + "</td>";
        //                table = table + "</tr>";
        //            }

        //            List<TimeTable> ttt = new List<TimeTable>();
        //            ttt = (from p in TimeTableList.FirstOrDefault().Value
        //                   where p.Day == "Tuesday"
        //                   select p).ToList();
        //            if (ttt.Count != 0)
        //            {
        //                table = table + "<tr>";
        //                if (!string.IsNullOrEmpty(ttt[0].Period1))
        //                    table = table + " <td class=" + ttt[0].Period1 + "><br/><br/>" + ttt[0].Period1 + "</td>";
        //                if (!string.IsNullOrEmpty(ttt[0].Period2))
        //                    table = table + " <td class=" + ttt[0].Period2 + "><br/><br/>" + ttt[0].Period2 + "</td>";
        //                if (!string.IsNullOrEmpty(ttt[0].Period3))
        //                    table = table + " <td class=" + ttt[0].Period3 + "><br/><br/>" + ttt[0].Period3 + "</td>";
        //                if (!string.IsNullOrEmpty(ttt[0].Period4))
        //                    table = table + " <td class=" + ttt[0].Period4 + "><br/><br/>" + ttt[0].Period4 + "</td>";
        //                if (!string.IsNullOrEmpty(ttt[0].Period5))
        //                    table = table + " <td class=" + ttt[0].Period5 + "><br/><br/>" + ttt[0].Period5 + "</td>";
        //                if (!string.IsNullOrEmpty(ttt[0].Period6))
        //                    table = table + " <td class=" + ttt[0].Period6 + "><br/><br/>" + ttt[0].Period6 + "</td>";
        //                if (!string.IsNullOrEmpty(ttt[0].Period7))
        //                    table = table + " <td class=" + ttt[0].Period7 + "><br/><br/>" + ttt[0].Period7 + "</td>";
        //                if (!string.IsNullOrEmpty(ttt[0].Period8))
        //                    table = table + " <td class=" + ttt[0].Period8 + "><br/><br/>" + ttt[0].Period8 + "</td>";
        //                table = table + "</tr>";
        //            }
        //            List<TimeTable> ttw = new List<TimeTable>();
        //            ttw = (from p in TimeTableList.FirstOrDefault().Value
        //                   where p.Day == "Wednesday"
        //                   select p).ToList();
        //            if (ttw.Count != 0)
        //            {
        //                table = table + "<tr>";
        //                if (!string.IsNullOrEmpty(ttw[0].Period1))
        //                    table = table + " <td class=" + ttw[0].Period1 + "><br/><br/>" + ttw[0].Period1 + "</td>";
        //                if (!string.IsNullOrEmpty(ttw[0].Period2))
        //                    table = table + " <td class=" + ttw[0].Period2 + "><br/><br/>" + ttw[0].Period2 + "</td>";
        //                if (!string.IsNullOrEmpty(ttw[0].Period3))
        //                    table = table + " <td class=" + ttw[0].Period3 + "><br/><br/>" + ttw[0].Period3 + "</td>";
        //                if (!string.IsNullOrEmpty(ttw[0].Period4))
        //                    table = table + " <td class=" + ttw[0].Period4 + "><br/><br/>" + ttw[0].Period4 + "</td>";
        //                if (!string.IsNullOrEmpty(ttw[0].Period5))
        //                    table = table + " <td class=" + ttw[0].Period5 + "><br/><br/>" + ttw[0].Period5 + "</td>";
        //                if (!string.IsNullOrEmpty(ttw[0].Period6))
        //                    table = table + " <td class=" + ttw[0].Period6 + "><br/><br/>" + ttw[0].Period6 + "</td>";
        //                if (!string.IsNullOrEmpty(ttw[0].Period7))
        //                    table = table + " <td class=" + ttw[0].Period7 + "><br/><br/>" + ttw[0].Period7 + "</td>";
        //                if (!string.IsNullOrEmpty(ttw[0].Period8))
        //                    table = table + " <td class=" + ttw[0].Period8 + "><br/><br/>" + ttw[0].Period8 + "</td>";
        //                table = table + "</tr>";
        //            }
        //            List<TimeTable> ttth = new List<TimeTable>();
        //            ttth = (from p in TimeTableList.FirstOrDefault().Value
        //                    where p.Day == "Thursday"
        //                   select p).ToList();
        //            if (ttth.Count != 0)
        //            {
        //                table = table + "<tr>";
        //                if (!string.IsNullOrEmpty(ttth[0].Period1))
        //                    table = table + " <td class=" + ttth[0].Period1 + "><br/><br/>" + ttth[0].Period1 + "</td>";
        //                if (!string.IsNullOrEmpty(ttth[0].Period2))
        //                    table = table + " <td class=" + ttth[0].Period2 + "><br/><br/>" + ttth[0].Period2 + "</td>";
        //                if (!string.IsNullOrEmpty(ttth[0].Period3))
        //                    table = table + " <td class=" + ttth[0].Period3 + "><br/><br/>" + ttth[0].Period3 + "</td>";
        //                if (!string.IsNullOrEmpty(ttth[0].Period4))
        //                    table = table + " <td class=" + ttth[0].Period4 + "><br/><br/>" + ttth[0].Period4 + "</td>";
        //                if (!string.IsNullOrEmpty(ttth[0].Period5))
        //                    table = table + " <td class=" + ttth[0].Period5 + "><br/><br/>" + ttth[0].Period5 + "</td>";
        //                if (!string.IsNullOrEmpty(ttth[0].Period6))
        //                    table = table + " <td class=" + ttth[0].Period6 + "><br/><br/>" + ttth[0].Period6 + "</td>";
        //                if (!string.IsNullOrEmpty(ttth[0].Period7))
        //                    table = table + " <td class=" + ttth[0].Period7 + "><br/><br/>" + ttth[0].Period7 + "</td>";
        //                if (!string.IsNullOrEmpty(ttth[0].Period8))
        //                    table = table + " <td class=" + ttth[0].Period8 + "><br/><br/>" + ttth[0].Period8 + "</td>";
        //                table = table + "</tr>";
        //            }
        //            List<TimeTable> ttf = new List<TimeTable>();
        //            ttf = (from p in TimeTableList.FirstOrDefault().Value
        //                   where p.Day == "Friday"
        //                    select p).ToList();
        //            if (ttf.Count != 0)
        //            {
        //                table = table + "<tr>";
        //                if (!string.IsNullOrEmpty(ttf[0].Period1))
        //                    table = table + " <td class=" + ttf[0].Period1 + "><br/><br/>" + ttf[0].Period1 + "</td>";
        //                if (!string.IsNullOrEmpty(ttf[0].Period2))
        //                    table = table + " <td class=" + ttf[0].Period2 + "><br/><br/>" + ttf[0].Period2 + "</td>";
        //                if (!string.IsNullOrEmpty(ttf[0].Period3))
        //                    table = table + " <td class=" + ttf[0].Period3 + "><br/><br/>" + ttf[0].Period3 + "</td>";
        //                if (!string.IsNullOrEmpty(ttf[0].Period4))
        //                    table = table + " <td class=" + ttf[0].Period4 + "><br/><br/>" + ttf[0].Period4 + "</td>";
        //                if (!string.IsNullOrEmpty(ttf[0].Period5))
        //                    table = table + " <td class=" + ttf[0].Period5 + "><br/><br/>" + ttf[0].Period5 + "</td>";
        //                if (!string.IsNullOrEmpty(ttf[0].Period6))
        //                    table = table + " <td class=" + ttf[0].Period6 + "><br/><br/>" + ttf[0].Period6 + "</td>";
        //                if (!string.IsNullOrEmpty(ttf[0].Period7))
        //                    table = table + " <td class=" + ttf[0].Period7 + "><br/><br/>" + ttf[0].Period7 + "</td>";
        //                if (!string.IsNullOrEmpty(ttf[0].Period8))
        //                    table = table + " <td class=" + ttf[0].Period8 + "><br/><br/>" + ttf[0].Period8 + "</td>";
        //                table = table + "</tr>";
        //            }
        //            table = table + "</table>";
        //            Response.Write(table);
        //        }
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "StaffIssuesPolicy");
        //        throw ex;

        //    }
        //}

        //public JsonResult GetTimeTableData()
        //{
        //    Dictionary<string, object> criteria = new Dictionary<string, object>();
        //    Dictionary<long, IList<tt_timetable>> TimeTblDetails = null;
        //    TimeTblDetails = timeTblSrvc.Get_tt_timetableListWithPagingAndCriteria(null, 2, string.Empty, string.Empty, criteria);
        //    var eventList = from e in TimeTblDetails.FirstOrDefault().Value
        //                    select new
        //                    {
        //                        id = e.timetable_id,
        //                        title = e.period_no.ToString(),
        //                        start = e.start_date.ToString("s"),
        //                        end = e.end_date.ToString("s"),
        //                        //color = e.StatusColor,
        //                        //someKey = e.SomeImportantKeyID,
        //                        allDay = false
        //                    };
        //    var rows = eventList.ToArray();
        //    return Json(rows, JsonRequestBehavior.AllowGet); 
        //}

        public ActionResult ShowTimeTable(string cam, string grd, string sec)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Campus", cam);
                criteria.Add("Grade", grd);
                criteria.Add("Section", sec);
                Dictionary<long, IList<TimeTable>> TimeTableList = timeTblSrvc.GetTimeTableListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);

                //ttm = timeTblSrvc.GetTimeTableByCamGraSec("IB MAIN", "VI", "A");
                string table = string.Empty;

                if (TimeTableList != null && TimeTableList.Count > 0 && TimeTableList.FirstOrDefault().Key > 0 && TimeTableList.FirstOrDefault().Value != null)
                {
                    List<TimeTable> ttm = new List<TimeTable>();
                    table = table + "<table class='table table-bordered'>";
                    ttm = (from p in TimeTableList.FirstOrDefault().Value
                           where p.Day == "Monday"
                           select p).ToList();
                    if (ttm.Count != 0)
                    {
                        table = table + "<tr><td>";
                        if (!string.IsNullOrEmpty(ttm[0].Period1))
                            table = table + " <table class='" + ttm[0].Period1 + "'><tr class='tdborder'><td><i class='" + ttm[0].Period1Icon + "'></i></td><td><span class='infobox-data-number'>" + ttm[0].Period1 + "</span><div class='infobox-content'>" + ttm[0].Period1Caption + "</div></td></tr></table></td>";
                        if (!string.IsNullOrEmpty(ttm[0].Period2))
                            table = table + "<td> <table class='" + ttm[0].Period2 + "'><tr class='tdborder'><td><i class='" + ttm[0].Period2Icon + "'></i></td><td><span class='infobox-data-number'>" + ttm[0].Period2 + "</span><div class='infobox-content'>" + ttm[0].Period2Caption + "</div></td></tr></table></td>";
                        if (!string.IsNullOrEmpty(ttm[0].Period3))
                            table = table + "<td> <table class='" + ttm[0].Period3 + "'><tr class='tdborder'><td><i class='" + ttm[0].Period3Icon + "'></i></td><td><span class='infobox-data-number'>" + ttm[0].Period3 + "</span><div class='infobox-content'>" + ttm[0].Period3Caption + "</div></td></tr></table></td>";
                        if (!string.IsNullOrEmpty(ttm[0].Period4))
                            table = table + " <td><table class='" + ttm[0].Period4 + "'><tr class='tdborder'><td><i class='" + ttm[0].Period4Icon + "'></i></td><td><span class='infobox-data-number'>" + ttm[0].Period4 + "</span><div class='infobox-content'>" + ttm[0].Period4Caption + "</div></td></tr></table></td>";
                        if (!string.IsNullOrEmpty(ttm[0].Period5))
                            table = table + " <td><table class='" + ttm[0].Period5 + "'><tr class='tdborder'><td><i class='" + ttm[0].Period5Icon + "'></i></td><td><span class='infobox-data-number'>" + ttm[0].Period5 + "</span><div class='infobox-content'>" + ttm[0].Period5Caption + "</div></td></tr></table></td>";
                        if (!string.IsNullOrEmpty(ttm[0].Period6))
                            table = table + " <td><table class='" + ttm[0].Period6 + "'><tr class='tdborder'><td><i class='" + ttm[0].Period6Icon + "'></i></td><td><span class='infobox-data-number'>" + ttm[0].Period6 + "</span><div class='infobox-content'>" + ttm[0].Period6Caption + "</div></td></tr></table></td>";
                        if (!string.IsNullOrEmpty(ttm[0].Period7))
                            table = table + " <td><table class='" + ttm[0].Period7 + "'><tr class='tdborder'><td><i class='" + ttm[0].Period7Icon + "'></i></td><td><span class='infobox-data-number'>" + ttm[0].Period7 + "</span><div class='infobox-content'>" + ttm[0].Period7Caption + "</div></td></tr></table></td>";
                        if (!string.IsNullOrEmpty(ttm[0].Period8))
                            table = table + " <td><table class='" + ttm[0].Period8 + "'><tr class='tdborder'><td><i class='" + ttm[0].Period8Icon + "'></i></td><td><span class='infobox-data-number'>" + ttm[0].Period8 + "</span><div class='infobox-content'>" + ttm[0].Period8Caption + "</div></td></tr></table></td>";
                        table = table + "</tr>";
                    }

                    List<TimeTable> ttt = new List<TimeTable>();
                    ttt = (from p in TimeTableList.FirstOrDefault().Value
                           where p.Day == "Tuesday"
                           select p).ToList();
                    if (ttt.Count != 0)
                    {
                        table = table + "<tr><td>";
                        if (!string.IsNullOrEmpty(ttt[0].Period1))
                            table = table + " <table class='" + ttt[0].Period1 + "'><tr class='tdborder'><td><i class='" + ttt[0].Period1Icon + "'></i></td><td><span class='infobox-data-number'>" + ttt[0].Period1 + "</span><div class='infobox-content'>" + ttt[0].Period1Caption + "</div></td></tr></table></td>";
                        if (!string.IsNullOrEmpty(ttt[0].Period2))
                            table = table + "<td> <table class='" + ttt[0].Period2 + "'><tr class='tdborder'><td><i class='" + ttt[0].Period2Icon + "'></i></td><td><span class='infobox-data-number'>" + ttt[0].Period2 + "</span><div class='infobox-content'>" + ttt[0].Period2Caption + "</div></td></tr></table></td>";
                        if (!string.IsNullOrEmpty(ttt[0].Period3))
                            table = table + "<td> <table class='" + ttt[0].Period3 + "'><tr class='tdborder'><td><i class='" + ttt[0].Period3Icon + "'></i></td><td><span class='infobox-data-number'>" + ttt[0].Period3 + "</span><div class='infobox-content'>" + ttt[0].Period3Caption + "</div></td></tr></table></td>";
                        if (!string.IsNullOrEmpty(ttt[0].Period4))
                            table = table + " <td><table class='" + ttt[0].Period4 + "'><tr class='tdborder'><td><i class='" + ttt[0].Period4Icon + "'></i></td><td><span class='infobox-data-number'>" + ttt[0].Period4 + "</span><div class='infobox-content'>" + ttt[0].Period4Caption + "</div></td></tr></table></td>";
                        if (!string.IsNullOrEmpty(ttt[0].Period5))
                            table = table + " <td><table class='" + ttt[0].Period5 + "'><tr class='tdborder'><td><i class='" + ttt[0].Period5Icon + "'></i></td><td><span class='infobox-data-number'>" + ttt[0].Period5 + "</span><div class='infobox-content'>" + ttt[0].Period5Caption + "</div></td></tr></table></td>";
                        if (!string.IsNullOrEmpty(ttt[0].Period6))
                            table = table + " <td><table class='" + ttt[0].Period6 + "'><tr class='tdborder'><td><i class='" + ttt[0].Period6Icon + "'></i></td><td><span class='infobox-data-number'>" + ttt[0].Period6 + "</span><div class='infobox-content'>" + ttt[0].Period6Caption + "</div></td></tr></table></td>";
                        if (!string.IsNullOrEmpty(ttt[0].Period7))
                            table = table + " <td><table class='" + ttt[0].Period7 + "'><tr class='tdborder'><td><i class='" + ttt[0].Period7Icon + "'></i></td><td><span class='infobox-data-number'>" + ttt[0].Period7 + "</span><div class='infobox-content'>" + ttt[0].Period7Caption + "</div></td></tr></table></td>";
                        if (!string.IsNullOrEmpty(ttt[0].Period8))
                            table = table + " <td><table class='" + ttt[0].Period8 + "'><tr class='tdborder'><td><i class='" + ttt[0].Period8Icon + "'></i></td><td><span class='infobox-data-number'>" + ttt[0].Period8 + "</span><div class='infobox-content'>" + ttt[0].Period8Caption + "</div></td></tr></table></td>";
                        table = table + "</tr>";
                    }
                    List<TimeTable> ttw = new List<TimeTable>();
                    ttw = (from p in TimeTableList.FirstOrDefault().Value
                           where p.Day == "Wednesday"
                           select p).ToList();
                    if (ttw.Count != 0)
                    {
                        table = table + "<tr><td>";
                        if (!string.IsNullOrEmpty(ttw[0].Period1))
                            table = table + " <table class='" + ttw[0].Period1 + "'><tr class='tdborder'><td><i class='" + ttw[0].Period1Icon + "'></i></td><td><span class='infobox-data-number'>" + ttw[0].Period1 + "</span><div class='infobox-content'>" + ttw[0].Period1Caption + "</div></td></tr></table></td>";
                        if (!string.IsNullOrEmpty(ttw[0].Period2))
                            table = table + "<td> <table class='" + ttw[0].Period2 + "'><tr class='tdborder'><td><i class='" + ttw[0].Period2Icon + "'></i></td><td><span class='infobox-data-number'>" + ttw[0].Period2 + "</span><div class='infobox-content'>" + ttw[0].Period2Caption + "</div></td></tr></table></td>";
                        if (!string.IsNullOrEmpty(ttw[0].Period3))
                            table = table + "<td> <table class='" + ttw[0].Period3 + "'><tr class='tdborder'><td><i class='" + ttw[0].Period3Icon + "'></i></td><td><span class='infobox-data-number'>" + ttw[0].Period3 + "</span><div class='infobox-content'>" + ttw[0].Period3Caption + "</div></td></tr></table></td>";
                        if (!string.IsNullOrEmpty(ttw[0].Period4))
                            table = table + " <td><table class='" + ttw[0].Period4 + "'><tr class='tdborder'><td><i class='" + ttw[0].Period4Icon + "'></i></td><td><span class='infobox-data-number'>" + ttw[0].Period4 + "</span><div class='infobox-content'>" + ttw[0].Period4Caption + "</div></td></tr></table></td>";
                        if (!string.IsNullOrEmpty(ttw[0].Period5))
                            table = table + " <td><table class='" + ttw[0].Period5 + "'><tr class='tdborder'><td><i class='" + ttw[0].Period5Icon + "'></i></td><td><span class='infobox-data-number'>" + ttw[0].Period5 + "</span><div class='infobox-content'>" + ttw[0].Period5Caption + "</div></td></tr></table></td>";
                        if (!string.IsNullOrEmpty(ttw[0].Period6))
                            table = table + " <td><table class='" + ttw[0].Period6 + "'><tr class='tdborder'><td><i class='" + ttw[0].Period6Icon + "'></i></td><td><span class='infobox-data-number'>" + ttw[0].Period6 + "</span><div class='infobox-content'>" + ttw[0].Period6Caption + "</div></td></tr></table></td>";
                        if (!string.IsNullOrEmpty(ttw[0].Period7))
                            table = table + " <td><table class='" + ttw[0].Period7 + "'><tr class='tdborder'><td><i class='" + ttw[0].Period7Icon + "'></i></td><td><span class='infobox-data-number'>" + ttw[0].Period7 + "</span><div class='infobox-content'>" + ttw[0].Period7Caption + "</div></td></tr></table></td>";
                        if (!string.IsNullOrEmpty(ttw[0].Period8))
                            table = table + " <td><table class='" + ttw[0].Period8 + "'><tr class='tdborder'><td><i class='" + ttw[0].Period8Icon + "'></i></td><td><span class='infobox-data-number'>" + ttw[0].Period8 + "</span><div class='infobox-content'>" + ttw[0].Period8Caption + "</div></td></tr></table></td>";
                        table = table + "</tr>";
                    }
                    List<TimeTable> ttth = new List<TimeTable>();
                    ttth = (from p in TimeTableList.FirstOrDefault().Value
                            where p.Day == "Thursday"
                            select p).ToList();
                    if (ttth.Count != 0)
                    {
                        table = table + "<tr><td>";
                        if (!string.IsNullOrEmpty(ttth[0].Period1))
                            table = table + " <table class='" + ttth[0].Period1 + "'><tr class='tdborder'><td><i class='" + ttth[0].Period1Icon + "'></i></td><td><span class='infobox-data-number'>" + ttth[0].Period1 + "</span><div class='infobox-content'>" + ttth[0].Period1Caption + "</div></td></tr></table></td>";
                        if (!string.IsNullOrEmpty(ttth[0].Period2))
                            table = table + "<td> <table class='" + ttth[0].Period2 + "'><tr class='tdborder'><td><i class='" + ttth[0].Period2Icon + "'></i></td><td><span class='infobox-data-number'>" + ttth[0].Period2 + "</span><div class='infobox-content'>" + ttth[0].Period2Caption + "</div></td></tr></table></td>";
                        if (!string.IsNullOrEmpty(ttth[0].Period3))
                            table = table + "<td> <table class='" + ttth[0].Period3 + "'><tr class='tdborder'><td><i class='" + ttth[0].Period3Icon + "'></i></td><td><span class='infobox-data-number'>" + ttth[0].Period3 + "</span><div class='infobox-content'>" + ttth[0].Period3Caption + "</div></td></tr></table></td>";
                        if (!string.IsNullOrEmpty(ttth[0].Period4))
                            table = table + " <td><table class='" + ttth[0].Period4 + "'><tr class='tdborder'><td><i class='" + ttth[0].Period4Icon + "'></i></td><td><span class='infobox-data-number'>" + ttth[0].Period4 + "</span><div class='infobox-content'>" + ttth[0].Period4Caption + "</div></td></tr></table></td>";
                        if (!string.IsNullOrEmpty(ttth[0].Period5))
                            table = table + " <td><table class='" + ttth[0].Period5 + "'><tr class='tdborder'><td><i class='" + ttth[0].Period5Icon + "'></i></td><td><span class='infobox-data-number'>" + ttth[0].Period5 + "</span><div class='infobox-content'>" + ttth[0].Period5Caption + "</div></td></tr></table></td>";
                        if (!string.IsNullOrEmpty(ttth[0].Period6))
                            table = table + " <td><table class='" + ttth[0].Period6 + "'><tr class='tdborder'><td><i class='" + ttth[0].Period6Icon + "'></i></td><td><span class='infobox-data-number'>" + ttth[0].Period6 + "</span><div class='infobox-content'>" + ttth[0].Period6Caption + "</div></td></tr></table></td>";
                        if (!string.IsNullOrEmpty(ttth[0].Period7))
                            table = table + " <td><table class='" + ttth[0].Period7 + "'><tr class='tdborder'><td><i class='" + ttth[0].Period7Icon + "'></i></td><td><span class='infobox-data-number'>" + ttth[0].Period7 + "</span><div class='infobox-content'>" + ttth[0].Period7Caption + "</div></td></tr></table></td>";
                        if (!string.IsNullOrEmpty(ttth[0].Period8))
                            table = table + " <td><table class='" + ttth[0].Period8 + "'><tr class='tdborder'><td><i class='" + ttth[0].Period8Icon + "'></i></td><td><span class='infobox-data-number'>" + ttth[0].Period8 + "</span><div class='infobox-content'>" + ttth[0].Period8Caption + "</div></td></tr></table></td>";
                        table = table + "</tr>";
                    }
                    List<TimeTable> ttf = new List<TimeTable>();
                    ttf = (from p in TimeTableList.FirstOrDefault().Value
                           where p.Day == "Friday"
                           select p).ToList();
                    if (ttf.Count != 0)
                    {
                        table = table + "<tr><td>";
                        if (!string.IsNullOrEmpty(ttf[0].Period1))
                            table = table + " <table class='" + ttf[0].Period1 + "'><tr class='tdborder'><td><i class='" + ttf[0].Period1Icon + "'></i></td><td><span class='infobox-data-number'>" + ttf[0].Period1 + "</span><div class='infobox-content'>" + ttf[0].Period1Caption + "</div></td></tr></table></td>";
                        if (!string.IsNullOrEmpty(ttf[0].Period2))
                            table = table + "<td> <table class='" + ttf[0].Period2 + "'><tr class='tdborder'><td><i class='" + ttf[0].Period2Icon + "'></i></td><td><span class='infobox-data-number'>" + ttf[0].Period2 + "</span><div class='infobox-content'>" + ttf[0].Period2Caption + "</div></td></tr></table></td>";
                        if (!string.IsNullOrEmpty(ttf[0].Period3))
                            table = table + "<td> <table class='" + ttf[0].Period3 + "'><tr class='tdborder'><td><i class='" + ttf[0].Period3Icon + "'></i></td><td><span class='infobox-data-number'>" + ttf[0].Period3 + "</span><div class='infobox-content'>" + ttf[0].Period3Caption + "</div></td></tr></table></td>";
                        if (!string.IsNullOrEmpty(ttf[0].Period4))
                            table = table + " <td><table class='" + ttf[0].Period4 + "'><tr class='tdborder'><td><i class='" + ttf[0].Period4Icon + "'></i></td><td><span class='infobox-data-number'>" + ttf[0].Period4 + "</span><div class='infobox-content'>" + ttf[0].Period4Caption + "</div></td></tr></table></td>";
                        if (!string.IsNullOrEmpty(ttf[0].Period5))
                            table = table + " <td><table class='" + ttf[0].Period5 + "'><tr class='tdborder'><td><i class='" + ttf[0].Period5Icon + "'></i></td><td><span class='infobox-data-number'>" + ttf[0].Period5 + "</span><div class='infobox-content'>" + ttf[0].Period5Caption + "</div></td></tr></table></td>";
                        if (!string.IsNullOrEmpty(ttf[0].Period6))
                            table = table + " <td><table class='" + ttf[0].Period6 + "'><tr class='tdborder'><td><i class='" + ttf[0].Period6Icon + "'></i></td><td><span class='infobox-data-number'>" + ttf[0].Period6 + "</span><div class='infobox-content'>" + ttf[0].Period6Caption + "</div></td></tr></table></td>";
                        if (!string.IsNullOrEmpty(ttf[0].Period7))
                            table = table + " <td><table class='" + ttf[0].Period7 + "'><tr class='tdborder'><td><i class='" + ttf[0].Period7Icon + "'></i></td><td><span class='infobox-data-number'>" + ttf[0].Period7 + "</span><div class='infobox-content'>" + ttf[0].Period7Caption + "</div></td></tr></table></td>";
                        if (!string.IsNullOrEmpty(ttf[0].Period8))
                            table = table + " <td><table class='" + ttf[0].Period8 + "'><tr class='tdborder'><td><i class='" + ttf[0].Period8Icon + "'></i></td><td><span class='infobox-data-number'>" + ttf[0].Period8 + "</span><div class='infobox-content'>" + ttf[0].Period8Caption + "</div></td></tr></table></td>";
                        table = table + "</tr>";
                    }
                    List<TimeTable> tts = new List<TimeTable>();
                    tts = (from p in TimeTableList.FirstOrDefault().Value
                           where p.Day == "Saturday"
                           select p).ToList();
                    if (tts.Count != 0)
                    {
                        table = table + "<tr><td>";
                        if (!string.IsNullOrEmpty(tts[0].Period1))
                            table = table + " <table class='" + tts[0].Period1 + "'><tr class='tdborder'><td><i class='" + tts[0].Period1Icon + "'></i></td><td><span class='infobox-data-number'>" + tts[0].Period1 + "</span><div class='infobox-content'>" + tts[0].Period1Caption + "</div></td></tr></table></td>";
                        if (!string.IsNullOrEmpty(tts[0].Period2))
                            table = table + "<td> <table class='" + tts[0].Period2 + "'><tr class='tdborder'><td><i class='" + tts[0].Period2Icon + "'></i></td><td><span class='infobox-data-number'>" + tts[0].Period2 + "</span><div class='infobox-content'>" + tts[0].Period2Caption + "</div></td></tr></table></td>";
                        if (!string.IsNullOrEmpty(tts[0].Period3))
                            table = table + "<td> <table class='" + tts[0].Period3 + "'><tr class='tdborder'><td><i class='" + tts[0].Period3Icon + "'></i></td><td><span class='infobox-data-number'>" + tts[0].Period3 + "</span><div class='infobox-content'>" + tts[0].Period3Caption + "</div></td></tr></table></td>";
                        if (!string.IsNullOrEmpty(tts[0].Period4))
                            table = table + " <td><table class='" + tts[0].Period4 + "'><tr class='tdborder'><td><i class='" + tts[0].Period4Icon + "'></i></td><td><span class='infobox-data-number'>" + tts[0].Period4 + "</span><div class='infobox-content'>" + tts[0].Period4Caption + "</div></td></tr></table></td>";
                        if (!string.IsNullOrEmpty(tts[0].Period5))
                            table = table + " <td><table class='" + tts[0].Period5 + "'><tr class='tdborder'><td><i class='" + tts[0].Period5Icon + "'></i></td><td><span class='infobox-data-number'>" + tts[0].Period5 + "</span><div class='infobox-content'>" + tts[0].Period5Caption + "</div></td></tr></table></td>";
                        if (!string.IsNullOrEmpty(tts[0].Period6))
                            table = table + " <td><table class='" + tts[0].Period6 + "'><tr class='tdborder'><td><i class='" + tts[0].Period6Icon + "'></i></td><td><span class='infobox-data-number'>" + tts[0].Period6 + "</span><div class='infobox-content'>" + tts[0].Period6Caption + "</div></td></tr></table></td>";
                        if (!string.IsNullOrEmpty(tts[0].Period7))
                            table = table + " <td><table class='" + tts[0].Period7 + "'><tr class='tdborder'><td><i class='" + tts[0].Period7Icon + "'></i></td><td><span class='infobox-data-number'>" + tts[0].Period7 + "</span><div class='infobox-content'>" + tts[0].Period7Caption + "</div></td></tr></table></td>";
                        if (!string.IsNullOrEmpty(tts[0].Period8))
                            table = table + " <td><table class='" + tts[0].Period8 + "'><tr class='tdborder'><td><i class='" + tts[0].Period8Icon + "'></i></td><td><span class='infobox-data-number'>" + tts[0].Period8 + "</span><div class='infobox-content'>" + tts[0].Period8Caption + "</div></td></tr></table></td>";
                        table = table + "</tr>";
                    }

                    table = table + "</table>";
                    Response.Write(table);
                }
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StaffIssuesPolicy");
                throw ex;

            }
        }

        public JsonResult GetTimeTableData(string SearchCampus, string SearchGrade, string SearchSection)
        {
            Dictionary<long, IList<tt_timetable>> TimeTblDetails = null;
            //Dictionary<long, IList<PeriodTimeMaster>> periodTimeMast = null;
            // long a = 1370;
            // criteria.Add("timetable_id", a);
            IList<tt_timetable> timeTableIlist = new List<tt_timetable>();
            IList<PeriodTimeMaster> PeriodTimeMasterList = new List<PeriodTimeMaster>();
            PeriodTimeMaster periodTimeMaster = new PeriodTimeMaster();
            tt_SubjectMaster subMaster = new tt_SubjectMaster();

            //tt_Search search = new tt_Search();
            //search = timeTblSrvc.get_tt_SearchById(1);

            //if (search == null)
            //{
            //    search.SearchCampus = "IB MAIN";
            //    search.SearchGrade = "VI";
            //    search.SearchSection = "A"; 
            //    //if (!string.IsNullOrEmpty(search.SearchCampus)) { criteria.Add("SearchCampus", search.SearchCampus); }
            //    //if (!string.IsNullOrEmpty(search.SearchGrade)) { criteria.Add("SearchGrade", search.SearchGrade); }
            //    //if (!string.IsNullOrEmpty(search.SearchSection)) { criteria.Add("SearchSection", search.SearchSection); }
            //}
            //else
            //criteria.Add("division_id", "1");
            if (SearchCampus == null || SearchCampus == "") { SearchCampus = "IB MAIN"; }
            if (SearchGrade == null || SearchGrade == "") { SearchGrade = "VI"; }
            if (SearchSection == null || SearchSection == "") { SearchSection = "A"; }

            tt_division division = new tt_division();
            division = timeTblSrvc.get_tt_divisionByCampusGrdSec(SearchCampus, SearchGrade, SearchSection);
            long divisionId = 1;
            if (division != null)
            {
                divisionId = division.division_id;
            }
            else
            { return null; }
            criteria.Add("division_id", divisionId);
            TimeTblDetails = timeTblSrvc.Get_tt_timetableListWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, criteria);
            criteria.Clear();
            string Grade = division.Grade;
            foreach (var item in TimeTblDetails.FirstOrDefault().Value)
            {
                tt_timetable tt_timetable = new tt_timetable();
                periodTimeMaster = timeTblSrvc.getPeriodTimeMasterByGradeandPeriodNumber(item.period_no, Grade);
                subMaster = timeTblSrvc.getSubjectMasterById(item.subject_id);
                tt_days Days = timeTblSrvc.get_tt_days_By_day_id(Convert.ToInt64(item.day_id));

                DateTime finalStart_Time = new DateTime();
                DateTime finalEnd_Time = new DateTime();
                finalStart_Time = Days.day_date.Add(TimeSpan.Parse(periodTimeMaster.Start_Time));
                finalEnd_Time = Days.day_date.Add(TimeSpan.Parse(periodTimeMaster.End_Time));
                tt_timetable.start_date = finalStart_Time;
                tt_timetable.end_date = finalEnd_Time;
                //tt_timetable.start_date = periodTimeMaster.Start_Time;
                //tt_timetable.end_date = periodTimeMaster.End_Time;
                tt_timetable.SubjectColor = subMaster.subject_colour;
                tt_timetable.background_color = subMaster.background_color;


                tt_timetable.timetable_id = item.timetable_id;
                //tt_timetable.allotment_id = item.allotment_id;
                //tt_timetable.teacher_id = item.teacher_id;
                tt_timetable.Subject = subMaster.subject_name;
                timeTableIlist.Add(tt_timetable);
            }
            var eventList = from e in timeTableIlist
                            select new
                            {
                                // id = e.timetable_id,
                                id = e.timetable_id,
                                //id1 = e.allotment_id,
                                //id2 = e.teacher_id,
                                title = e.Subject,
                                start = e.start_date.ToString("s"),
                                end = e.end_date.ToString("s"),                            //color = e.StatusColor,
                                textColor = e.SubjectColor,
                                //someKey = e.SomeImportantKeyID,
                                backgroundColor = e.background_color,
                                //borderColor="#ffffff",
                                //color=e.SubjectColor,
                                allDay = false
                            };
            var rows = eventList.ToArray();
            return Json(rows, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult Test(string title, string start, string end)
        //{
        //    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
        //    DateTime start1 = DateTime.Parse(start, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
        //    DateTime end1 = DateTime.Parse(end, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
        //    tt_timetable ttable = new tt_timetable();
        //    ttable.Subject = title;
        //    // ttable.timetable_id = 3;
        //    ttable.start_date = start1;
        //    ttable.end_date = end1;
        //    timeTblSrvc.SaveOrUpdateTimeTable(ttable);
        //    return null;
        //}
        public JsonResult UpdateSearchTimeTable(string SearchCampus, string SearchGrade, string SearchSection)
        {
            bool retData = false;
            tt_Search search = new tt_Search();
            search.Id = 1;
            search.SearchId = 1;
            search.SearchCampus = SearchCampus;
            search.SearchGrade = SearchGrade;
            search.SearchSection = SearchSection;
            timeTblSrvc.SaveOrUpdate_tt_Search(search);
            tt_division division = new tt_division();
            division = timeTblSrvc.get_tt_divisionByCampusGrdSec(search.SearchCampus, search.SearchGrade, search.SearchSection);
            if (division != null && division.division_id > 0)
            {
                retData = true;
            }
            return Json(retData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getTeacherTimeTableHtml(string StaffId)
        {
            string html = string.Empty;
            if (!string.IsNullOrEmpty(StaffId) && StaffId != "0")
            {
                //criteria.Add("subject_id", Convert.ToInt64(StaffId));
                IList<TeachersTimeTable> techTimeList = new List<TeachersTimeTable>();
                criteria.Add("teacher_id", Convert.ToInt64(StaffId));
                Dictionary<long, IList<tt_timetable>> TimeTblDetails = null;
                TimeTblDetails = timeTblSrvc.Get_tt_timetableListWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, criteria);
                for (int i = 1; i < 6; i++)
                {
                    var timeTblList = (from u in TimeTblDetails.FirstOrDefault().Value
                                       where u.day_id == i
                                       select u).ToList();

                    TeachersTimeTable teachTimTblObj = new TeachersTimeTable();
                    teachTimTblObj.DayId = i;
                    teachTimTblObj.DayName = getDayName(i);
                    string Prd_1 = string.Empty;
                    string Prd_2 = string.Empty;
                    string Prd_3 = string.Empty;
                    string Prd_4 = string.Empty;
                    string Prd_5 = string.Empty;
                    string Prd_6 = string.Empty;
                    string Prd_7 = string.Empty;

                    string Prd_1_color = string.Empty;
                    string Prd_2_color = string.Empty;
                    string Prd_3_color = string.Empty;
                    string Prd_4_color = string.Empty;
                    string Prd_5_color = string.Empty;
                    string Prd_6_color = string.Empty;
                    string Prd_7_color = string.Empty;

                    foreach (var item in timeTblList)
                    {
                        string divisionName = string.Empty;
                        string subjectName = string.Empty;
                        string divsnAndSub = string.Empty;
                        string Color = string.Empty;
                        if (item.division_id > 0)
                        {
                            tt_division division = new tt_division();
                            division = timeTblSrvc.get_tt_divisionById(item.division_id);
                            if (division != null)
                            {
                                divisionName = division.Grade + "-" + division.Section;
                                Color = division.division_color;
                            }
                        }
                        if (item.subject_id > 0)
                        {
                            tt_SubjectMaster subject = new tt_SubjectMaster();
                            subject = timeTblSrvc.get_tt_SubjectMasterBysubject_id(item.subject_id);
                            subjectName = subject.subject_code;
                        }
                        divsnAndSub = divsnAndSub + divisionName + " (" + subjectName + ")";
                        if (item.period_no == 1)
                        {
                            Prd_1 = divsnAndSub;
                            Prd_1_color = Color;
                        }
                        if (item.period_no == 2)
                        {
                            Prd_2 = divsnAndSub;
                            Prd_2_color = Color;
                        }
                        if (item.period_no == 3)
                        {
                            Prd_3 = divsnAndSub;
                            Prd_3_color = Color;
                        }
                        if (item.period_no == 4)
                        {
                            Prd_4 = divsnAndSub;
                            Prd_4_color = Color;
                        }
                        if (item.period_no == 5)
                        {
                            Prd_5 = divsnAndSub;
                            Prd_5_color = Color;
                        }
                        if (item.period_no == 6)
                        {
                            Prd_6 = divsnAndSub;
                            Prd_6_color = Color;
                        }
                        if (item.period_no == 7)
                        {
                            Prd_7 = divsnAndSub;
                            Prd_7_color = Color;
                        }
                    }

                    teachTimTblObj.First_Period = Prd_1;
                    teachTimTblObj.Secnd_Period = Prd_2;
                    teachTimTblObj.Third_Period = Prd_3;
                    teachTimTblObj.Fourth_Period = Prd_4;
                    teachTimTblObj.Fifth_Period = Prd_5;
                    teachTimTblObj.Sixth_Period = Prd_6;
                    teachTimTblObj.Sevent_Period = Prd_7;

                    teachTimTblObj.First_Prd_color = Prd_1_color;
                    teachTimTblObj.Secnd_Prd_color = Prd_2_color;
                    teachTimTblObj.Third_Prd_color = Prd_3_color;
                    teachTimTblObj.Fourth_Prd_color = Prd_4_color;
                    teachTimTblObj.Fifth_Prd_color = Prd_5_color;
                    teachTimTblObj.Sixth_Prd_color = Prd_6_color;
                    teachTimTblObj.Sevent_Prd_color = Prd_7_color;





                    techTimeList.Add(teachTimTblObj);
                }
                html = "<html><head><title>Test</title></head><body>";
                html = html + "<table border='1' width='540px;'>";
                html = html + "<tr><th style='width:60px;'></th><th style='width:70px;text-align:center;'>1</th><th style='width:70px;text-align:center;'>2</th><th style='width:70px;text-align:center;'>3</th><th style='width:70px;text-align:center;'>4</th><th style='width:70px;text-align:center;'>5</th><th style='width:70px;text-align:center;'>6</th><th style='width:70px;text-align:center;'>7</th></tr>";
                foreach (var TeacheTblItem in techTimeList)
                {
                    html = html + "<tr style='height:35px;'>";
                    html = html + "<td style='text-align:center;font-size:10px;color:#1F4C24;font-weight:bold;'>" + TeacheTblItem.DayName + "</td>";
                    if (!string.IsNullOrEmpty(TeacheTblItem.First_Period)) { html = html + "<td style='background-color:white;font-size:10px;text-align:center;color:" + TeacheTblItem.First_Prd_color + ";'>" + TeacheTblItem.First_Period + "</td>"; }
                    else
                        html = html + "<td>" + TeacheTblItem.First_Period + "</td>";

                    if (!string.IsNullOrEmpty(TeacheTblItem.Secnd_Period)) { html = html + "<td style='background-color:white;font-size:10px;text-align:center;color:" + TeacheTblItem.Secnd_Prd_color + ";'>" + TeacheTblItem.Secnd_Period + "</td>"; }
                    else
                        html = html + "<td>" + TeacheTblItem.Secnd_Period + "</td>";

                    if (!string.IsNullOrEmpty(TeacheTblItem.Third_Period)) { html = html + "<td style='background-color:white;font-size:10px;text-align:center;color:" + TeacheTblItem.Third_Prd_color + ";'>" + TeacheTblItem.Third_Period + "</td>"; }
                    else
                        html = html + "<td>" + TeacheTblItem.Third_Period + "</td>";

                    if (!string.IsNullOrEmpty(TeacheTblItem.Fourth_Period)) { html = html + "<td style='background-color:white;font-size:10px;text-align:center;color:" + TeacheTblItem.Fourth_Prd_color + ";'>" + TeacheTblItem.Fourth_Period + "</td>"; }
                    else
                        html = html + "<td>" + TeacheTblItem.Fourth_Period + "</td>";

                    if (!string.IsNullOrEmpty(TeacheTblItem.Fifth_Period)) { html = html + "<td style='background-color:white;font-size:10px;text-align:center;color:" + TeacheTblItem.Fifth_Prd_color + ";'>" + TeacheTblItem.Fifth_Period + "</td>"; }
                    else
                        html = html + "<td>" + TeacheTblItem.Fifth_Period + "</td>";

                    if (!string.IsNullOrEmpty(TeacheTblItem.Sixth_Period)) { html = html + "<td style='background-color:white;font-size:10px;text-align:center;color:" + TeacheTblItem.Sixth_Prd_color + ";'>" + TeacheTblItem.Sixth_Period + "</td>"; }
                    else
                        html = html + "<td>" + TeacheTblItem.Sixth_Period + "</td>";

                    if (!string.IsNullOrEmpty(TeacheTblItem.Sevent_Period)) { html = html + "<td style='background-color:white;font-size:10px;text-align:center;color:" + TeacheTblItem.Sevent_Prd_color + ";'>" + TeacheTblItem.Sevent_Period + "</td>"; }
                    else
                        html = html + "<td>" + TeacheTblItem.Sevent_Period + "</td>";

                    html = html + "</tr>";
                }
                html = html + "</table>";
                html = html + "</body></html>";
            }
            else
            {
                html = "<html><head><title>Test</title></head><body>";
                html = html + "<table border='1' width='540px;'>";
                html = html + "<tr><th style='width:60px;'></th><th style='width:70px;text-align:center;'>1</th><th style='width:70px;text-align:center;'>2</th><th style='width:70px;text-align:center;'>3</th><th style='width:70px;text-align:center;'>4</th><th style='width:70px;text-align:center;'>5</th><th style='width:70px;text-align:center;'>6</th><th style='width:70px;text-align:center;'>7</th></tr>";
                for (int i = 1; i < 6; i++)
                {
                    html = html + "<tr style='height:35px;font-size:10px;'>";
                    html = html + "<td style='text-align:center;font-size:10px;color:#1F4C24'>" + getDayName(i) + "</td>";
                    html = html + "<td>" + " " + "</td>";
                    html = html + "<td>" + " " + "</td>";
                    html = html + "<td>" + " " + "</td>";
                    html = html + "<td>" + " " + "</td>";
                    html = html + "<td>" + " " + "</td>";
                    html = html + "<td>" + " " + "</td>";
                    html = html + "<td>" + " " + "</td>";
                    html = html + "</tr>";
                }
                html = html + "</table>";
                html = html + "</body></html>";
            }
            return Json(html, JsonRequestBehavior.AllowGet);
        }
        public string getDayName(long day)
        {
            string dayName = string.Empty;
            if (day > 0)
            {
                switch (day)
                {
                    case 1: dayName = "Mon";
                        break;
                    case 2: dayName = "Tue";
                        break;
                    case 3: dayName = "Wed";
                        break;
                    case 4: dayName = "Thu";
                        break;
                    case 5: dayName = "Fri";
                        break;
                    default:
                        dayName = "";
                        break;
                }

            }
            return dayName;
        }

        public JsonResult getTeachersBydivision(string Campus, string Grade, string Section)
        {
            tt_division division = new tt_division();
            if (!string.IsNullOrEmpty(Campus) && !string.IsNullOrEmpty(Grade) && !string.IsNullOrEmpty(Section))
            {
                division = timeTblSrvc.get_tt_divisionByCampusGrdSec(Campus, Grade, Section);
                if (division != null)
                {
                    criteria.Add("division_id", division.division_id);
                    Dictionary<long, IList<tt_timetable>> TimeTblDetails = null;
                    TimeTblDetails = timeTblSrvc.Get_tt_timetableListWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, criteria);
                    if (TimeTblDetails != null && TimeTblDetails.First().Key > 0)
                    {
                        var TeachersIdArry = (from u in TimeTblDetails.FirstOrDefault().Value
                                              where u.division_id > 0
                                              select u.teacher_id).Distinct().ToArray();
                        criteria.Clear();
                        criteria.Add("teacher_id", TeachersIdArry);
                        Dictionary<long, IList<tt_teacher>> TeachersDetails = null;
                        TeachersDetails = timeTblSrvc.Get_tt_teacherListWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, criteria);
                        if (TeachersDetails != null && TeachersDetails.First().Key > 0)
                        {
                            var TeachersList = (
                                 from items in TeachersDetails.First().Value
                                 where items.teacher_id != null
                                 select new
                                 {
                                     Text = items.teacher_code,
                                     Value = items.teacher_id
                                 }).Distinct().ToList();
                            return Json(TeachersList, JsonRequestBehavior.AllowGet);
                        }

                    }
                }
            }
            var jsondat = new { rows = (new { cell = new string[] { } }) };
            return Json(jsondat, JsonRequestBehavior.AllowGet);
        }

        public string getGrade(string Grade)
        {
            string retGrade = string.Empty;
            if (Grade == "Grade-I")
            {
                retGrade = "I";
            }
            if (Grade == "Grade-II")
            {
                retGrade = "II";
            }
            if (Grade == "Grade-III")
            {
                retGrade = "III";
            }
            if (Grade == "Grade-IV")
            {
                retGrade = "IV";
            }
            if (Grade == "Grade-V")
            {
                retGrade = "V";
            }
            if (Grade == "Grade-VI")
            {
                retGrade = "VI";
            }
            if (Grade == "Grade-VII")
            {
                retGrade = "VII";
            }
            if (Grade == "Grade-VIII")
            {
                retGrade = "VIII";
            }
            if (Grade == "Grade-VII")
            {
                retGrade = "VII";
            }
            return retGrade;
        }

        public JsonResult AddPeriod(string Subject, string start, string end, string staff, string Campus, string Grade, string Section)
        {
            IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
            DateTime start1 = DateTime.Parse(start, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
            DateTime end1 = DateTime.Parse(end, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
            tt_division division = new tt_division();
            tt_timetable ttable = new tt_timetable();
            division = timeTblSrvc.GetDivisionMasterByCampusGradeandSection(Campus, Grade, Section);
            ttable.division_id = division.division_id;
            tt_SubjectMaster subject = new tt_SubjectMaster();
            subject = timeTblSrvc.GetSubjectMasterBySubject(Subject);
            ttable.subject_id = subject.subject_id;
            tt_teacher Staff = new tt_teacher();
            Staff = timeTblSrvc.GetTeacherMasterByStaffName(staff);
            ttable.teacher_id = Staff.teacher_id;
            tt_days days = new tt_days();

            var dtPart = start1.ToShortDateString();

            DateTime dt = Convert.ToDateTime(dtPart);

            criteria.Clear();
            criteria.Add("day_date", dt);
            Dictionary<long, IList<tt_days>> daysList = timeTblSrvc.GetDaysMasterListWithPagingAndCriteriaSearch(0, 9999, string.Empty, string.Empty, criteria);
            if (daysList != null && daysList.FirstOrDefault().Key > 0)
            {
                ttable.day_id = daysList.FirstOrDefault().Value[0].day_id;
            }
            var end2 = end1.ToShortTimeString();
            var start2 = start1.ToShortTimeString();
            // TimeSpan startTime = TimeSpan.Parse(start2);
            //TimeSpan endTime = TimeSpan.Parse(end2);
            // DateTime startTime = Convert.ToDateTime(start2);
            //DateTime endTime = Convert.ToDateTime(end2);
            if (start2 == "12:50" && end2 == "13:40")
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            criteria.Clear();
            criteria.Add("Start_Time", start2);
            criteria.Add("End_Time", end2);
            criteria.Add("Campus", Campus);
            criteria.Add("Grade", Grade);
            criteria.Add("Section", Section);
            Dictionary<long, IList<PeriodTimeMaster>> PeriodTimeList = timeTblSrvc.GetPeriodTimeMasterListWithPagingAndCriteriaSearch(0, 9999, string.Empty, string.Empty, criteria);
            if (PeriodTimeList != null && PeriodTimeList.FirstOrDefault().Key > 0)
            {

                long periodtime = PeriodTimeList.FirstOrDefault().Value[0].Period_Number;
                ttable.period_no = Convert.ToInt32(periodtime);
            }
            tt_allotment AllotmentList = new tt_allotment();
            Int32 noOfPeriods;
            Int32 remainingPeriods;
            // string AllocatErrMsg = "Sorry can't create subject here!!!";
            AllotmentList = timeTblSrvc.GetAllotmentByDivSubjandTecherIds(ttable.division_id, ttable.subject_id);
            if (AllotmentList != null)
            {
                if (ttable.teacher_id == AllotmentList.teacher_id)
                {
                    ttable.allotment_id = AllotmentList.allotment_id;
                    noOfPeriods = AllotmentList.no_of_periods;
                    remainingPeriods = AllotmentList.remaining_periods;
                    if (0 < remainingPeriods && remainingPeriods <= noOfPeriods)
                    {
                        //criteria.Clear();
                        //criteria.Add("allotment_id", ttable.allotment_id);
                        //Dictionary<long, IList<tt_timetable>> AllotmentCheckList = timeTblSrvc.GetAllotmentMasterListWithPagingAndCriteriaSearch(0, 9999, string.Empty, string.Empty, criteria);
                        //if (AllotmentCheckList != null && AllotmentCheckList.FirstOrDefault().Key > 0)
                        //{
                        //    long allotmentCount = AllotmentCheckList.FirstOrDefault().Value.Count;
                        //if (allotmentCount <= noOfPeriods)
                        //{
                        criteria.Clear();
                        criteria.Add("day_id", ttable.day_id);
                        criteria.Add("division_id", ttable.division_id);
                        criteria.Add("period_no", ttable.period_no);
                        Dictionary<long, IList<tt_timetable>> AllotmentPeriodList = timeTblSrvc.GetAllotmentMasterListWithPagingAndCriteriaSearch(0, 9999, string.Empty, string.Empty, criteria);
                        if (AllotmentPeriodList != null && AllotmentPeriodList.FirstOrDefault().Key > 0)
                        {
                            return Json("already alloc", JsonRequestBehavior.AllowGet);//The period is already allocated to this class
                        }
                        else
                        {
                            tt_allotment remainingPeriod = new tt_allotment();
                            remainingPeriods -= 1;
                            remainingPeriod.teacher_id = ttable.teacher_id;
                            remainingPeriod.subject_id = ttable.subject_id;
                            remainingPeriod.division_id = ttable.division_id;
                            remainingPeriod.no_of_periods = noOfPeriods;
                            remainingPeriod.allotment_id = ttable.allotment_id;
                            remainingPeriod.remaining_periods = remainingPeriods;

                            timeTblSrvc.SaveOrUpdateAllotment(remainingPeriod);

                            long timetable_id = timeTblSrvc.SaveOrUpdateTimeTable(ttable);
                            return Json(timetable_id, JsonRequestBehavior.AllowGet);
                        }
                    }
                    //else
                    //{
                    //    return Json(null, JsonRequestBehavior.AllowGet);
                    //}
                    // }
                    //else
                    //{
                    //    return Json(null, JsonRequestBehavior.AllowGet);
                    //}
                    // }
                    else
                    {
                        return Json("remaining", JsonRequestBehavior.AllowGet);//check remaining period of this subject
                    }

                }
                else
                {
                    return Json("tehrnot", JsonRequestBehavior.AllowGet);//The teacher is not allocated to this subject
                }
            }
            else
            {
                return Json("nosub", JsonRequestBehavior.AllowGet);//The subject is not allocated to this class
            }

        }
        public JsonResult RetrieveSubjectLst(string term)
        {
            try
            {
                criteria.Clear();
                criteria.Add("subject_name", term);
                //change the method to not to use the count since it is not being used here "REVISIT"
                Dictionary<long, IList<tt_SubjectMaster>> UserList = timeTblSrvc.GetSubjectMasterListWithPagingAndCriteriaLikeSearch(0, 9999, string.Empty, string.Empty, criteria);
                var UserIds = (from u in UserList.First().Value
                               where u.subject_name != null
                               select u.subject_name).Distinct().ToList();
                return Json(UserIds, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult SubjectDetails(string start, string end, string Campus, string Grade, string Section)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogIn", "Account");
                else
                {
                    ViewBag.start = start;
                    ViewBag.end = end;
                    ViewBag.Campus = Campus;
                    ViewBag.Garde = Grade;
                    ViewBag.Section = Section;
                    return View();
                }
            }
            catch (Exception ex)
            {
                //  ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }

        }
        public ActionResult ValidSubject(string subject)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogIn", "Account");
                else
                {
                    criteria.Clear();
                    bool returnVal = false;
                    criteria.Add("subject_name", subject);
                    Dictionary<long, IList<tt_SubjectMaster>> subList = timeTblSrvc.GetValidSubjectWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                    if (subList != null && subList.FirstOrDefault().Key > 0)
                    {
                        returnVal = true;
                        //return Json(returnVal, JsonRequestBehavior.AllowGet);
                    }
                    return Json(returnVal, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                //  ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }

        }
        public JsonResult RetrieveStaffLst(string staff)
        {
            try
            {
                criteria.Clear();
                criteria.Add("teacher_name", staff);
                //change the method to not to use the count since it is not being used here "REVISIT"
                Dictionary<long, IList<tt_teacher>> UserList = timeTblSrvc.GetTeacherMasterListWithPagingAndCriteriaLikeSearch(0, 9999, string.Empty, string.Empty, criteria);
                var UserIds = (from u in UserList.First().Value
                               where u.teacher_name != null
                               select u.teacher_name).Distinct().ToList();
                return Json(UserIds, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult ValidStaff(string staff)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogIn", "Account");
                else
                {
                    criteria.Clear();
                    bool returnVal = false;
                    criteria.Add("teacher_name", staff);
                    Dictionary<long, IList<tt_teacher>> subList = timeTblSrvc.GetValidTeacherWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                    if (subList != null && subList.FirstOrDefault().Key > 0)
                    {
                        returnVal = true;
                        //return Json(returnVal, JsonRequestBehavior.AllowGet);
                    }
                    return Json(returnVal, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                //  ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }

        }

        public ActionResult DropSubject(string Subject, string start, string end, string id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogIn", "Account");
                else
                {
                    tt_timetable timetable = new tt_timetable();
                    timetable = timeTblSrvc.Get_tt_timetableById(Convert.ToInt64(id));
                    long AllotmentId = timetable.allotment_id;
                    tt_allotment Allotment = new tt_allotment();
                    Allotment = timeTblSrvc.GetAllotmentMasterById(Convert.ToInt64(AllotmentId));


                    Allotment.remaining_periods += 1;
                    timeTblSrvc.SaveOrUpdateAllotment(Allotment);
                    timeTblSrvc.DeleteTimeTableDetails(Convert.ToInt64(id));
                    return null;
                }
            }
            catch (Exception ex)
            {
                //  ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult ChangePeriod(string Subject, string start, string end, string ttableid, string Campus, string Grade, string Section)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogIn", "Account");
                else
                {
                    tt_timetable ttable = new tt_timetable();
                    tt_division division = new tt_division();
                    tt_SubjectMaster subject = new tt_SubjectMaster();
                    string AllocatErrMsg = "Sorry subject can't be change here!!!";
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    DateTime start1 = DateTime.Parse(start, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    DateTime end1 = DateTime.Parse(end, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    var dtPartS = start1.ToShortDateString();
                    DateTime dt = Convert.ToDateTime(dtPartS);
                    criteria.Clear();
                    criteria.Add("day_date", dt);
                    Dictionary<long, IList<tt_days>> daysList = timeTblSrvc.GetDaysMasterListWithPagingAndCriteriaSearch(0, 9999, string.Empty, string.Empty, criteria);
                    if (daysList != null && daysList.FirstOrDefault().Key > 0)
                    {
                        ttable.day_id = daysList.FirstOrDefault().Value[0].day_id;
                    }

                    var end2 = end1.ToShortTimeString();
                    var start2 = start1.ToShortTimeString();
                    if (start2 == "12:50" && end2 == "13:40")
                    {
                        return Json("You are moving subject into break hour", JsonRequestBehavior.AllowGet);
                    }
                    criteria.Clear();
                    criteria.Add("Start_Time", start2);
                    criteria.Add("End_Time", end2);
                    criteria.Add("Campus", Campus);
                    criteria.Add("Grade", Grade);
                    criteria.Add("Section", Section);
                    Dictionary<long, IList<PeriodTimeMaster>> PeriodTimeList = timeTblSrvc.GetPeriodTimeMasterListWithPagingAndCriteriaSearch(0, 9999, string.Empty, string.Empty, criteria);
                    if (PeriodTimeList != null && PeriodTimeList.FirstOrDefault().Key > 0)
                    {
                        long periodtime = PeriodTimeList.FirstOrDefault().Value[0].Period_Number;
                        ttable.period_no = Convert.ToInt32(periodtime);
                    }
                    else { return Json(AllocatErrMsg, JsonRequestBehavior.AllowGet); }
                    division = timeTblSrvc.GetDivisionMasterByCampusGradeandSection(Campus, Grade, Section);
                    ttable.division_id = division.division_id;
                    subject = timeTblSrvc.GetSubjectMasterBySubject(Subject);
                    ttable.subject_id = subject.subject_id;
                    criteria.Clear();
                    criteria.Add("day_id", ttable.day_id);
                    criteria.Add("division_id", ttable.division_id);
                    criteria.Add("period_no", ttable.period_no);
                    Dictionary<long, IList<tt_timetable>> AllotmentPeriodList = timeTblSrvc.GetAllotmentMasterListWithPagingAndCriteriaSearch(0, 9999, string.Empty, string.Empty, criteria);
                    if (AllotmentPeriodList != null && AllotmentPeriodList.FirstOrDefault().Key > 0)
                    {
                        return Json("The period is already allocated to this class", JsonRequestBehavior.AllowGet);
                    }
                    tt_timetable timetable = new tt_timetable();
                    timetable = timeTblSrvc.Get_tt_timetableById(Convert.ToInt64(ttableid));
                    ttable.allotment_id = timetable.allotment_id;
                    ttable.teacher_id = timetable.teacher_id;
                    ttable.timetable_id = timetable.timetable_id;
                    criteria.Clear();
                    criteria.Add("day_id", ttable.day_id);
                    criteria.Add("teacher_id", ttable.teacher_id);
                    criteria.Add("period_no", ttable.period_no);
                    Dictionary<long, IList<tt_timetable>> AllotmentTeacherList = timeTblSrvc.GetAllotmentMasterListWithPagingAndCriteriaSearch(0, 9999, string.Empty, string.Empty, criteria);
                    if (AllotmentTeacherList != null && AllotmentTeacherList.FirstOrDefault().Key > 0)
                    {
                        return Json("The staff is already allocated to some other class", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        timeTblSrvc.SaveOrUpdateTimeTable(ttable);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                //  ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult GetTeachersDetailsById(long teacherId)
        {
            SubjectTeacherDetails subTechObj = new SubjectTeacherDetails();
            tt_teacher teacher = new tt_teacher();
            if (teacherId > 0)
            {
                teacher = timeTblSrvc.get_tt_teacherByteacher_id(Convert.ToInt64(teacherId));
                subTechObj.TeacherName = teacher.teacher_name;
            }
            return Json(subTechObj, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSubjectTeacherTableHtml(string SearchCampus, string SearchGrade, string SearchSection)
        {
            string html = string.Empty;
            long t = 10;
            html = "<html><head><title>Test</title></head><body>";
            html = html + "<table border='1' width='540px;' id='SubjectTecherTbl'>";
            html = html + "<tr><th>Test1</th><th>Test2</th></tr>";
            html = html + "<tr id='1' style='height:35px;font-size:10px;'>";
            html = html + "<td style='background-color:#1F4C24;color:white;' id='testTD'><a style='color:#034af3;text-decoration:underline;cursor:pointer;' onclick=\"OpenOldRouteStudList(" + t + ");\" '>10</a></td>";
            html = html + "<td style='background-color:#1F4C24;color:white;'>20</td>";
            html = html + "</tr>";
            html = html + "<tr id='2' style='height:35px;font-size:10px;'>";
            html = html + "<td style='background-color:#1F4C24;color:white;'>30</td>";
            html = html + "<td style='background-color:#1F4C24;color:white;'>40</td>";
            html = html + "</tr>";
            html = html + "</table>";
            html = html + "</body></html>";
            return Json(html, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Test()
        {
            return null;
        }

        public JsonResult getSubTeachrTymTbl(string SearchCampus, string SearchGrade, string SearchSection)
        {
            string htmlTbl = string.Empty;
            tt_Search search = new tt_Search();
            search = timeTblSrvc.get_tt_SearchById(1);
            tt_timetable tt = new tt_timetable();
            tt.Campus = search.SearchCampus;
            tt.Grade = search.SearchGrade;
            tt.Section = search.SearchSection;

            tt_division division = new tt_division();
            tt_allotment allotment = new tt_allotment();
            division = timeTblSrvc.get_tt_divisionByCampusGrdSec(SearchCampus, SearchGrade, SearchSection);
            if (division != null && division.division_id > 0)
                allotment = timeTblSrvc.get_tt_allotmentByDivision_IdAndCharge(division.division_id);
            else
            {
                return Json(htmlTbl, JsonRequestBehavior.AllowGet);
            }
            tt_teacher teacher = new tt_teacher();
            if (allotment != null && allotment.allotment_id > 0)
                teacher = timeTblSrvc.get_tt_teacherByteacher_id(allotment.teacher_id);
            if (teacher != null && (!string.IsNullOrEmpty(teacher.teacher_code)))
                tt.ClassCharge = teacher.teacher_code;
            else
                tt.ClassCharge = "Not Mentioned";

            criteria.Clear();
            criteria.Add("division_id", division.division_id);
            Dictionary<long, IList<tt_allotment>> AllotmentDetails = null;
            AllotmentDetails = timeTblSrvc.Get_tt_allotment_ListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
            IList<tt_SubjectTeacherPeriodCount> SubTechPrdCont = new List<tt_SubjectTeacherPeriodCount>();
            foreach (var item in AllotmentDetails.FirstOrDefault().Value)
            {
                tt_SubjectTeacherPeriodCount obj = new tt_SubjectTeacherPeriodCount();
                tt_SubjectMaster subject = new tt_SubjectMaster();
                if (item.subject_id > 0)
                {
                    subject = timeTblSrvc.get_tt_SubjectMasterBysubject_id(item.subject_id);
                    obj.Subject = subject.subject_name;
                }
                else
                    obj.Subject = "";
                if (item.teacher_id > 0)
                {
                    tt_teacher techerDet = new tt_teacher();
                    techerDet = timeTblSrvc.get_tt_teacherByteacher_id(item.teacher_id);
                    obj.TeacherCode = techerDet.teacher_code;
                }
                else
                    obj.TeacherCode = "";
                if (item.teacher_id > 0)
                    obj.TeacherId = item.teacher_id;
                else
                    obj.TeacherId = 0;
                obj.PeriodCount = item.no_of_periods;
                obj.PeriodRemainingCount = item.remaining_periods;
                SubTechPrdCont.Add(obj);
            }
            tt.SubjTecherList = SubTechPrdCont;

            if (tt.SubjTecherList != null)
            {
                htmlTbl = htmlTbl + "<table class='SubjectTecherTbl' border='1' width='355px;' id='SubjectTecherTbl'>";
                htmlTbl = htmlTbl + "<tr style='background-color: #dedede'>";
                htmlTbl = htmlTbl + "<th class='tblHeader' style='width: 60px;'>Subject</th>";
                htmlTbl = htmlTbl + "<th class='tblHeader' style='width: 70px;'>Teacher</th>";
                htmlTbl = htmlTbl + "<th class='tblHeader' style='width: 30px;'>Periods</th>";
                htmlTbl = htmlTbl + "<th class='tblHeader' style='width: 40px;'>Periods Remaining</th>";
                htmlTbl = htmlTbl + "</tr>";
                for (int i = 0; i < tt.SubjTecherList.Count; i++)
                {
                    htmlTbl = htmlTbl + "<tr id=" + tt.SubjTecherList[i].TeacherId + " class='subTr'>";
                    //htmlTbl = htmlTbl + "<tr id='<a style='cursor:pointer;' onclick=\"OpenOldRouteStudList(" + tt.SubjTecherList[i].TeacherId + ");\" '></a>";
                    htmlTbl = htmlTbl + "<td class='tblData' id=" + tt.SubjTecherList[i].Subject + ">" + tt.SubjTecherList[i].Subject + "</td>";
                    //htmlTbl=htmlTbl+"<td class='tblData'>"+tt.SubjTecherList[i].TeacherCode+"</td>";
                    htmlTbl = htmlTbl + "<td class='tblData'><a style='color:#034af3;text-decoration:underline;cursor:pointer;' onclick=\"OpenOldRouteStudList(" + tt.SubjTecherList[i].TeacherId + ");\" '>" + tt.SubjTecherList[i].TeacherCode + "</a></td>";
                    htmlTbl = htmlTbl + "<td class='tblData' style='text-align: center;'>" + tt.SubjTecherList[i].PeriodCount + "</td>";
                    htmlTbl = htmlTbl + "<td class='tblData' style='text-align: center;'>" + tt.SubjTecherList[i].PeriodRemainingCount + "</td>";
                    htmlTbl = htmlTbl + "</tr>";
                }
                htmlTbl = htmlTbl + "</table>";
            }
            return Json(htmlTbl, JsonRequestBehavior.AllowGet);
        }
        //by benadict 06-07
        public JsonResult getTeacherDetailsHtml(string StaffId)
        {
            tt_teacher teacherDetails = new tt_teacher();
            tt_teacherDeatils teacherDetailsRes = new tt_teacherDeatils();

            teacherDetails = timeTblSrvc.getTeacherDetailsHtmlById(Convert.ToInt64(StaffId));
            if (teacherDetails != null && teacherDetails.teacher_id > 0)
            {
                teacherDetailsRes.Gender = teacherDetails.gender;
                teacherDetailsRes.TeacherName = teacherDetails.teacher_name;
                teacherDetailsRes.max_periods = teacherDetails.max_periods;
                teacherDetailsRes.rem_periods = teacherDetails.rem_periods;
            }
            return Json(teacherDetailsRes, JsonRequestBehavior.AllowGet);

        }


        public JsonResult GridRecords1()
        {
            Array rowscells = new[] {
                    new{institutionid="1",institutionname="test" },
                    new{institutionid="12",institutionname="test2"},
                    new{institutionid="11",institutionname="test3"},
                    new{institutionid="10",institutionname="test4"},
                    new{institutionid="9",institutionname="test5"},
                    new{institutionid="5",institutionname="test6"},
                    new{institutionid="3",institutionname="test7"}, 
                    };
            //List<string> list = rowscells.OfType<string,string>().ToList();
            //cellstr();
            var result = new
            {
                page = "1",
                total = "1",
                records = "1",
                //rows = (new { cell = rowscells.Cast<string>()  })
                //rows = (new { cell = list})
                rows = rowscells


                //rows = new[] {
                //    new{institutionid=1,institutionname="test" },
                //    new{institutionid=12,institutionname="test2"},
                //    new{institutionid=11,institutionname="test3"},
                //    new{institutionid=10,institutionname="test4"},
                //    new{institutionid=9,institutionname="test5"},
                //    new{institutionid=5,institutionname="test6"},
                //    new{institutionid=3,institutionname="test7"}, 
                //    }
            };
            //var SAemptyLst = new { rows = (new { cell = new string[] { } }) };
            //                return Json(SAemptyLst, JsonRequestBehavior.AllowGet);
            //return Json(result);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GridViewDynamicBinding()
        {
            return View();
        }

        //Added by anto
        //To add and view Subjects
        public ActionResult GenerateTT()
        {
            return View();
        }
        public ActionResult TTAddSubjectDetails()
        {
            return View();
        }
        public ActionResult SubjectDetailsJqGrid(int? Id, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    //sord = sord == "desc" ? "Desc" : "Asc";
                    Dictionary<string, object> criteria = new Dictionary<string, object>();

                    Dictionary<long, IList<TTSubjectMaster>> SubjectMaster = timeTblSrvc.GetTTSubjectWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    if (SubjectMaster != null && SubjectMaster.Count > 0)
                    {
                        long totalrecords = SubjectMaster.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var SubjectDetailsList = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in SubjectMaster.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {

                               items.Id.ToString(),
                               items.Campus,
                               items.Subject,
                               items.AcademicYear,
                                //String.Format( @"<b><a style='color:#034af3;text-decoration:underline' href= '#' onclick = 'SubjectDetails('"+ items.Subject +"')' >{0}</a></b>","View Details"  ),
                              //  String.Format("<img src='/Images/TTView.png ' id='ImgHistory' onclick=\"SubjectDetails('" + items.Subject+"');\" />")
                                        }
                                    })
                        };
                        return Json(SubjectDetailsList, JsonRequestBehavior.AllowGet);
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
        public ActionResult AddSubjects(TTSubjectMaster TTS)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {

                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("Subject", TTS.Subject);
                    criteria.Add("Campus", TTS.Campus);
                    criteria.Add("AcademicYear", TTS.AcademicYear);
                    Dictionary<long, IList<TTSubjectMaster>> SubjectMaster = timeTblSrvc.GetTTSubjectWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteria);
                    if (SubjectMaster != null && SubjectMaster.First().Value != null && SubjectMaster.First().Value.Count > 0)
                    {
                        var script = @"ErrMsg(""The given Subject Name already exists!"");";
                        return JavaScript(script);
                    }
                    else
                    {
                        timeTblSrvc.CreateOrUpdateTTSubjectMaster(TTS);
                        var script = @"InfoMsg(""Subject Name Added successfully."");";
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
        // To add and view Staff
        public ActionResult TTAddStaffDetails()
        {
            MastersService ms = new MastersService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            Dictionary<long, IList<CampusMaster>> CampusMaster = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            ViewBag.campusddl = CampusMaster.First().Value;
            return View();
        }
        public ActionResult StaffDetailsJqGrid(int? Id, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    //sord = sord == "desc" ? "Desc" : "Asc";
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<long, IList<TTStaffMaster>> SubjectMaster = timeTblSrvc.GetTTStaffWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    if (SubjectMaster != null && SubjectMaster.Count > 0)
                    {
                        long totalrecords = SubjectMaster.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var SubjectDetailsList = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in SubjectMaster.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {

                               items.Id.ToString(),
                               items.Campus,
                               items.StaffName,
                            }
                                    })
                        };
                        return Json(SubjectDetailsList, JsonRequestBehavior.AllowGet);
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
        public ActionResult AddStaffs(TTStaffMaster TTS)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    timeTblSrvc.CreateOrUpdateTTStaffMaster(TTS);
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        //To Configure staffs
        public ActionResult TTAddConfigurationDetails()
        {
            return View();
        }
        public ActionResult AddStaffsandSubjects(TTStaffConfig TTConfig)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("Subject", TTConfig.Subject);
                    criteria.Add("Campus", TTConfig.Campus);
                    criteria.Add("StaffName", TTConfig.StaffName);
                    criteria.Add("Class", TTConfig.Class);
                    criteria.Add("Section", TTConfig.Section);
                    Dictionary<long, IList<TTStaffConfig>> SubLesson = timeTblSrvc.GetTTStaffConfigWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteria);
                    if (SubLesson != null && SubLesson.First().Value != null && SubLesson.First().Value.Count > 0)
                    {
                        var script = @"ErrMsg(""The given Subject Name already exists!"");";
                        return JavaScript(script);
                    }
                    else
                    {
                        timeTblSrvc.CreateOrUpdateTTStaffConfig(TTConfig);
                        var script = @"InfoMsg(""Subject Name Added successfully."");";
                        return JavaScript(script);
                    }

                    //return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult StaffLessonsJqGrid(string Subject, int? Id, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    //sord = sord == "desc" ? "Desc" : "Asc";

                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    if (!string.IsNullOrWhiteSpace(Subject)) { criteria.Add("Subject", Subject); }
                    Dictionary<long, IList<TTStaffConfig>> SubjectMaster = timeTblSrvc.GetTTStaffConfigWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    if (SubjectMaster != null && SubjectMaster.Count > 0)
                    {
                        long totalrecords = SubjectMaster.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var SubjectDetailsList = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in SubjectMaster.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {

                               items.Id.ToString(),
                               items.Campus,
                               items.StaffName,
                               items.Class,
                               items.Section,
                               items.Subject,
                              items.LessonsPerWeek,
                              items.ClassContinueity
                            }
                                    })
                        };
                        return Json(SubjectDetailsList, JsonRequestBehavior.AllowGet);
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

        public JsonResult Staffddl(string Campus)
        {
            try
            {
                MastersService ms = new MastersService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();

                criteria.Add("Campus", Campus);
                Dictionary<long, IList<TTStaffMaster>> StaffMaster = timeTblSrvc.GetTTStaffWithPagingAndCriteria(0, 0, "", "", criteria);
                ViewBag.Staff = StaffMaster.First().Value;

                return Json(ViewBag.gradeddl1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult FillStaffByCampus(string Campus)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Campus", Campus);
                Dictionary<long, IList<TTStaffMaster>> StaffMaster = timeTblSrvc.GetTTStaffWithPagingAndCriteria(0, 0, "", "", criteria);
                var stafflist = (from u in StaffMaster select u).ToList();
                if (stafflist != null && stafflist.First().Value != null && stafflist.First().Value.Count > 0)
                {
                    var StfMasterList = (
                                             from items in stafflist.First().Value
                                             select new
                                             {
                                                 Text = items.StaffName,
                                                 Value = items.StaffName
                                             }).Distinct().ToList().OrderBy(x => x.Text);
                    return Json(StfMasterList, JsonRequestBehavior.AllowGet);

                }
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StaffIssuesPolicy");
                throw ex;
            }
        }

        public ActionResult FillSubjectByCampus(string Campus)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Campus", Campus);
                Dictionary<long, IList<TTSubjectMaster>> StaffMaster = timeTblSrvc.GetTTSubjectWithPagingAndCriteria(0, 0, "", "", criteria);
                var sublist = (from u in StaffMaster select u).ToList();
                if (sublist != null && sublist.First().Value != null && sublist.First().Value.Count > 0)
                {
                    var SubMasterList = (
                                             from items in sublist.First().Value
                                             select new
                                             {
                                                 Text = items.Subject,
                                                 Value = items.Subject
                                             }).Distinct().ToList().OrderBy(x => x.Text);
                    return Json(SubMasterList, JsonRequestBehavior.AllowGet);

                }
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StaffIssuesPolicy");
                throw ex;
            }
        }

        public ActionResult TimeTableMapping()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogIn", "Account");
                else
                {
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<long, IList<CampusMaster>> CampusMaster = ms.GetCampusMasterListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                    criteria.Clear();
                    Dictionary<long, IList<GradeMaster>> GradeMaster = ms.GetGradeMasterListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                    Dictionary<long, IList<SectionMaster>> SectionMaster = ms.GetSectionMasterListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                    ViewBag.campusddl = CampusMaster.First().Value;
                    ViewBag.gradeddl1 = GradeMaster.First().Value;
                    ViewBag.sectionddl = SectionMaster.First().Value;
                    return View();
                }
            }
            catch (Exception ex)
            {
                //ExceptionPolicy.HandleException(ex, );
                throw ex;
            }
        }
    }
}
