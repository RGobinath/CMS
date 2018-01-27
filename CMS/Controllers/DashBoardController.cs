using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using TIPS.Entities;
using TIPS.Entities.AdmissionEntities;
using TIPS.Service;
using TIPS.ServiceContract;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace CMS.Controllers
{
    public class DashBoardController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public DataTable CampusStudentCount(string year)
        {
            AdmissionManagementService us = new AdmissionManagementService();

            var strsql = "";

            strsql = strsql + "select isnull(Campus,'Grand Total') as Campus,SUM(studentcount) as studentcount,SUM(occupancy) as occupancy ,round(((SUM(studentcount)/SUM(occupancy))*100),2) as percentage from( ";
            strsql = strsql + "select a.campus,a.grade,a.section,a.studentcount,cast(b.occupancy as float) as occupancy from( ";
            strsql = strsql + "select campus,grade,section,cast(COUNT(*) as float) as studentcount from StudentTemplate  ";
            if (year == "All")
            {
                strsql = strsql + "where AdmissionStatus='Registered' and Section is not null ";
            }
            else
            {
                strsql = strsql + "where AdmissionStatus='Registered' and Section is not null and JoiningAcademicYear='" + year + "' ";
            }
            strsql = strsql + "group by campus,grade,section ) a ";
            // strsql = strsql + "left join (select Campus,Grade,occupancy,acadamicyear from OccupancyMaster where acadamicyear='2013-2014' ";
            strsql = strsql + "left join (select Campus,Grade,occupancy,acadamicyear from OccupancyMaster ";
            strsql = strsql + ") b ";
            strsql = strsql + "on a.campus= b.campus and a.grade = b.grade )c ";
            // strsql = strsql + "group by GROUPING SETS ( (Campus,AcademicYear) , ()) ";
            strsql = strsql + "group by GROUPING SETS ( (Campus), ()) ";


            DataTable StudentList = us.GetDashBoardStudentList(strsql);

            return StudentList;
        }

        public DataTable CampusStudentCountByGrade(string year)
        {
            AdmissionManagementService us = new AdmissionManagementService();

            var strsql = "";

            strsql = strsql + "select isnull(grade,'Grand Total') as grade,Spaingrade,SUM(studentcount) as studentcount,SUM(occupancy) as occupancy ,round(((SUM(studentcount)/SUM(occupancy))*100),2) as percentage from( ";
            strsql = strsql + "select a.campus,a.grade,d.spaingrade,a.section,a.studentcount,cast(b.occupancy as float) as occupancy from( ";
            strsql = strsql + "select campus,grade,section,cast(COUNT(*) as float) as studentcount from StudentTemplate  ";
            if (year == "All")
            {
                strsql = strsql + "where AdmissionStatus='Registered' and Section is not null ";
            }
            else
            {
                strsql = strsql + "where AdmissionStatus='Registered' and Section is not null and JoiningAcademicYear = '" + year + "' ";
            }
            strsql = strsql + "group by campus,grade,section ) a ";
            //strsql = strsql + "left join (select Campus,Grade,occupancy,acadamicyear from OccupancyMaster where acadamicyear='2013-2014' ";
            strsql = strsql + "left join (select Campus,Grade,occupancy,acadamicyear from OccupancyMaster ";
            strsql = strsql + ") b ";
            strsql = strsql + "on a.campus= b.campus and a.grade = b.grade ";
            strsql = strsql + "left join(select grade,spaingrade from Occupancygrade) d ";
            strsql = strsql + "on a.Grade = d.grade )c ";
            //strsql = strsql + "group by GROUPING SETS ( (Grade,AcademicYear) , ()) ";
            strsql = strsql + "group by GROUPING SETS ( (Grade,SpainGrade) , ()) ";

            DataTable StudentList = us.GetDashBoardStudentList(strsql);

            return StudentList;
        }

        public DataTable GradeStudentCount(string campus, string year)
        {
            AdmissionManagementService us = new AdmissionManagementService();

            var strsql = "";

            strsql = strsql + "select isnull(grade,'Grand Total') as grade,Spaingrade,SUM(studentcount) as studentcount,SUM(occupancy) as occupancy ,round(((SUM(studentcount)/SUM(occupancy))*100),2) as percentage from( ";
            strsql = strsql + "select a.campus,a.grade,d.spaingrade,a.section,a.studentcount,cast(b.occupancy as float) as occupancy from( ";
            strsql = strsql + "select campus,grade,section,cast(COUNT(*) as float) as studentcount from StudentTemplate  ";
            if (year == "All")
            {
                strsql = strsql + "where AdmissionStatus='Registered' and Section is not null and campus = '" + campus + "' ";
            }
            else
            {
                strsql = strsql + "where AdmissionStatus='Registered' and Section is not null and campus = '" + campus + "' and JoiningAcademicYear='" + year + "' ";
            }
            strsql = strsql + "group by campus,grade,section ) a ";
            // strsql = strsql + "left join (select Campus,Grade,occupancy,acadamicyear from OccupancyMaster where acadamicyear='2013-2014' and campus = '" + campus + "' ";
            strsql = strsql + "left join (select Campus,Grade,occupancy,acadamicyear from OccupancyMaster where campus = '" + campus + "' ";
            strsql = strsql + ") b ";
            strsql = strsql + "on a.campus= b.campus and a.grade = b.grade  ";
            strsql = strsql + "left join(select grade,spaingrade from Occupancygrade) d ";
            strsql = strsql + "on a.Grade = d.grade )c ";
            // strsql = strsql + "group by GROUPING SETS ( (Grade,AcademicYear) , ()) ";
            strsql = strsql + "group by GROUPING SETS ( (Grade,SpainGrade) , ()) ";

            DataTable StudentList = us.GetDashBoardStudentList(strsql);

            return StudentList;
        }

        public DataTable SectionStudentCount(string campus, string grade, string year)
        {
            AdmissionManagementService us = new AdmissionManagementService();

            var strsql = "";

            strsql = strsql + "select isnull(section,'Grand Total') as section,SUM(studentcount) as studentcount,SUM(occupancy) as occupancy ,round(((SUM(studentcount)/SUM(occupancy))*100),2) as percentage from( ";
            strsql = strsql + "select a.campus,a.grade,a.section,a.studentcount,cast(b.occupancy as float) as occupancy from( ";
            strsql = strsql + "select campus,grade,section,cast(COUNT(*) as float) as studentcount from StudentTemplate  ";
            if (year == "All")
            {
                strsql = strsql + "where AdmissionStatus='Registered' and Section is not null and campus = '" + campus + "'  and grade='" + grade + "' ";
            }
            else
            {
                strsql = strsql + "where AdmissionStatus='Registered' and Section is not null and campus = '" + campus + "'  and grade='" + grade + "' and JoiningAcademicYear='" + year + "' ";
            }
            strsql = strsql + "group by campus,grade,section ) a ";
            // strsql = strsql + "left join (select Campus,Grade,occupancy,acadamicyear from OccupancyMaster where acadamicyear='2013-2014' and campus = '" + campus + "' and grade='" + grade + "' ";
            strsql = strsql + "left join (select Campus,Grade,occupancy,acadamicyear from OccupancyMaster where campus = '" + campus + "' and grade='" + grade + "' ";
            strsql = strsql + ") b ";
            strsql = strsql + "on a.campus= b.campus and a.grade = b.grade )c ";
            //strsql = strsql + "group by GROUPING SETS ( (Section,AcademicYear) , ())  ";
            strsql = strsql + "group by GROUPING SETS ( (Section) , ())  ";

            DataTable StudentList = us.GetDashBoardStudentList(strsql);

            return StudentList;
        }

        public DataTable StudentCountGrid(string year, string campus, string grade)
        {
            AdmissionManagementService us = new AdmissionManagementService();

            var strsql = "";
            if (year == "All")
            {
                strsql = strsql + "select isnull(cast(Year as varchar),'Grand Total') as period,SUM(studentcount) as studentcount from ( ";
                strsql = strsql + "select DATEPART(YEAR,CreatedDateNew)as year, ";
                strsql = strsql + "campus,grade,section,COUNT(*)as studentcount  ";
            }
            else
            {
                strsql = strsql + "select isnull(month,'Grand Total') as period,SUM(studentcount) as studentcount,cast(isnull(monthnumber,'13') as int ) as monthnumber from ( ";
                strsql = strsql + "select datename(MONTH,CreatedDateNew)as month, ";
                strsql = strsql + "campus,grade,section,COUNT(*)as studentcount,DATEPART(month,CreatedDateNew)as monthnumber  ";
            }


            strsql = strsql + "from studenttemplate where AdmissionStatus='Registered' and CreatedDateNew is not null ";

            if (year != "All")
            {
                strsql = strsql + "and DATEPART(YEAR,CreatedDateNew) = '" + year + "' ";
            }

            if (campus != "")
            {
                strsql = strsql + "and Campus='" + campus + "' ";
            }
            if (grade != "")
            {
                strsql = strsql + "and Grade='" + grade + "' ";
            }

            if (year == "All")
            {
                strsql = strsql + "group by DATEPART(YEAR,CreatedDateNew),campus,grade,section ) a ";
                strsql = strsql + "group by GROUPING SETS( (year),()) ";
                strsql = strsql + "order by period ";
            }

            if (year != "All")
            {
                strsql = strsql + "group by datename(MONTH,CreatedDateNew),campus,grade,section,DATEPART(month,CreatedDateNew) ) a ";
                strsql = strsql + "group by GROUPING SETS ( (month,monthnumber) , ())  ";
                strsql = strsql + "order by monthnumber ";
            }

            DataTable StudentList = us.GetDashBoardStudentList(strsql);
            return StudentList;
        }

        public DataTable StudentCountReportGrid(string year, string campus)
        {
            AdmissionManagementService us = new AdmissionManagementService();

            var strsql = "";

            if (campus == "All")
            {
                strsql = strsql + "select isnull(campus,'All') as campus,isnull(Grade,'Grand Total') as grade,SUM(sectioncount) as sectioncount,SUM(studentcount) as studentcount,SUM(occupancy) as occupancy, (SUM(occupancy) - SUM(studentcount)) as availablecapacity ";
                strsql = strsql + "from ( ";
                strsql = strsql + "select 'All' as campus,grade,COUNT(section) as sectioncount,SUM(studentcount) as studentcount,SUM(occupancy) as occupancy from ( ";
                strsql = strsql + "select a.campus,a.grade,a.section,a.studentcount,cast(b.occupancy as float) as occupancy from( ";
                strsql = strsql + "select campus,grade,section,cast(COUNT(*) as float) as studentcount from StudentTemplate  ";
                strsql = strsql + "where AdmissionStatus='Registered' and Section is not null ";
                if (year != "All")
                {
                    strsql = strsql + "and JoiningAcademicYear = '" + year + "' ";
                }
                //strsql = strsql + "--and campus = 'CHENNAI CITY' and Grade='I' ";
                strsql = strsql + "group by campus,grade,section ) a ";
                strsql = strsql + "left join (select Campus,Grade,occupancy,acadamicyear from OccupancyMaster ";
                //strsql = strsql + "--where campus = 'CHENNAI CITY' and Grade='I' ";
                strsql = strsql + ") b ";
                strsql = strsql + "on a.campus= b.campus and a.grade = b.grade ) c ";
                strsql = strsql + "group by campus,Grade ) d ";
                strsql = strsql + "group by campus,Grade ";
                //--group by GROUPING SETS ( (Campus,Grade),(Campus),()) 
                strsql = strsql + "union all ";
                strsql = strsql + "select isnull(campus,'All') as campus,isnull(grade,'Grand Total') as grade,COUNT(section) as sectioncount,SUM(studentcount) as studentcount,SUM(occupancy) as occupancy, (SUM(occupancy) - SUM(studentcount)) as availablecapacity from ( ";
                strsql = strsql + "select a.campus,a.grade,a.section,a.studentcount,cast(b.occupancy as float) as occupancy from(  ";
                strsql = strsql + "select campus,grade,section,cast(COUNT(*) as float) as studentcount from StudentTemplate  ";
                strsql = strsql + "where AdmissionStatus='Registered' and Section is not null ";
                if (year != "All")
                {
                    strsql = strsql + "and JoiningAcademicYear = '" + year + "' ";
                }
                //strsql = strsql + "--and campus = 'CHENNAI CITY' and Grade='I' ";
                strsql = strsql + "group by campus,grade,section ) a  ";
                strsql = strsql + "left join (select Campus,Grade,occupancy,acadamicyear from OccupancyMaster ";
                //strsql = strsql + "--where campus = 'CHENNAI CITY' and Grade='I' ";
                strsql = strsql + ") b ";
                strsql = strsql + "on a.campus= b.campus and a.grade = b.grade ) c ";
                //--group by Campus,Grade
                strsql = strsql + "group by GROUPING SETS ( (Campus,Grade),(Campus),()) ";
                strsql = strsql + "order by Campus ";
            }
            else
            {

                strsql = strsql + "select campus,grade,sectioncount,studentcount,occupancy,occupancy-studentcount as availablecapacity from ( ";
                strsql = strsql + "select campus, isnull(grade,'Grand Total') as grade ,COUNT(section) as sectioncount,SUM(studentcount) as studentcount,SUM(occupancy) as occupancy from ( ";
                strsql = strsql + "select a.campus,a.grade,a.section,a.studentcount,cast(b.occupancy as float) as occupancy from( ";
                strsql = strsql + "select campus,grade,section,cast(COUNT(*) as float) as studentcount from StudentTemplate  ";
                strsql = strsql + "where AdmissionStatus='Registered' and Section is not null and campus = '" + campus + "' ";
                if (year != "All")
                {
                    strsql = strsql + "and JoiningAcademicYear = '" + year + "' ";
                }
                strsql = strsql + "group by campus,grade,section ) a ";
                strsql = strsql + "left join (select Campus,Grade,occupancy,acadamicyear from OccupancyMaster where campus = '" + campus + "' ";
                strsql = strsql + ") b ";
                strsql = strsql + "on a.campus= b.campus and a.grade = b.grade ) c ";
                //--group by Campus,Grade
                strsql = strsql + "group by GROUPING SETS ( (Campus,Grade),(Campus),()) )d ";
                strsql = strsql + "where Campus is not null ";
                strsql = strsql + "order by Campus ";
            }

            DataTable StudentList = us.GetDashBoardStudentList(strsql);

            return StudentList;
        }
        // For Loading Initial Data Start
        public DataTable GetYear()
        {
            AdmissionManagementService us = new AdmissionManagementService();

            var strsql = "";
            strsql = strsql + "select distinct DATEPART(year,CreatedDateNew)as year from ";
            strsql = strsql + "StudentTemplate where AdmissionStatus='Registered' and CreatedDateNew is not null ";
            strsql = strsql + "order by DATEPART(year,CreatedDateNew) ";

            DataTable StudentList = us.GetDashBoardStudentList(strsql);

            return StudentList;
        }

        public DataTable GetAcademicYear()
        {
            AdmissionManagementService us = new AdmissionManagementService();

            var strsql = "";
            strsql = strsql + "select distinct joiningAcademicYear from StudentTemplate where joiningAcademicYear is not null ";
            strsql = strsql + "order by joiningAcademicYear ";

            DataTable StudentList = us.GetDashBoardStudentList(strsql);

            return StudentList;
        }

        public DataTable GetGrade(string campus)
        {
            AdmissionManagementService us = new AdmissionManagementService();

            var strsql = "";
            strsql = strsql + "select distinct Grade from StudentTemplate where Grade is not null and Campus='" + campus + "' ";
            strsql = strsql + "order by Grade ";

            DataTable StudentList = us.GetDashBoardStudentList(strsql);

            return StudentList;
        }
        // For Loading Initial Data End

        // For Grid Start

        public ActionResult GetCampusStudentCount(string ExportType, string year)
        {
            DataTable dt = CampusStudentCount(year);
            var list = dt.AsEnumerable()
             .Select(row => new
              {
                  studentcount = row["studentcount"],
                  Campus = row["Campus"],
                  occupancy = row["occupancy"],
                  percentage = row["percentage"]
              }).ToList();

            if (ExportType == "Excel")
            {
                var grid = new GridView
                {
                    DataSource = from document in list
                                 select new
                                 {
                                     Campus = document.Campus.ToString(),
                                     StudentCount = document.studentcount.ToString(),
                                     MaxStudentCount = document.occupancy.ToString(),
                                     OccupancyPercentage = document.percentage.ToString() + '%',

                                 }
                };

                grid.DataBind();

                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment; filename=CampusStudentCount.xls");
                Response.ContentType = "application/vnd.ms-excel";

                StringWriter sw = new StringWriter();

                HtmlTextWriter htw = new HtmlTextWriter(sw);
                grid.HeaderStyle.BackColor = System.Drawing.Color.FromName("#B6B6B6");
                grid.HeaderStyle.Font.Bold = true;
                grid.HeaderStyle.ForeColor = System.Drawing.Color.White;
                grid.RenderControl(htw);

                Response.Write(sw.ToString());

                Response.End();
                return new EmptyResult();

            }
            else
            {
                var jsondat = new
                {
                    total = 10,
                    page = 1,
                    records = 50,

                    rows = (from items in list
                            select new
                            {
                                i = 2,
                                cell = new string[] {
                               items.Campus.ToString(),
                               items.studentcount.ToString(),
                               items.occupancy.ToString(),
                               items.percentage.ToString() +'%'
                            }
                            })
                };

                return Json(jsondat, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetCampusStudentCountByGrade(string ExportType, string year)
        {
            DataTable dt = CampusStudentCountByGrade(year);
            var list = dt.AsEnumerable()
             .Select(row => new
             {
                 studentcount = row["studentcount"],
                 grade = row["grade"],
                 spaingrade = row["SpainGrade"],
                 occupancy = row["occupancy"],
                 percentage = row["percentage"]
             }).ToList();

            if (ExportType == "Excel")
            {
                var grid = new GridView
                {
                    DataSource = from document in list
                                 select new
                                 {
                                     Grade = document.grade.ToString(),
                                     SpainGrade = document.spaingrade.ToString(),
                                     StudentCount = document.studentcount.ToString(),
                                     MaxStudentCount = document.occupancy.ToString(),
                                     OccupancyPercentage = document.percentage.ToString() + '%',

                                 }
                };

                grid.DataBind();

                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment; filename=CampusStudentCountByGrade.xls");
                Response.ContentType = "application/vnd.ms-excel";

                StringWriter sw = new StringWriter();

                HtmlTextWriter htw = new HtmlTextWriter(sw);
                grid.HeaderStyle.BackColor = System.Drawing.Color.FromName("#B6B6B6");
                grid.HeaderStyle.Font.Bold = true;
                grid.HeaderStyle.ForeColor = System.Drawing.Color.White;
                grid.RenderControl(htw);

                Response.Write(sw.ToString());

                Response.End();
                return new EmptyResult();

            }
            else
            {
                var jsondat = new
                {
                    total = 10,
                    page = 1,
                    records = 50,

                    rows = (from items in list
                            select new
                            {
                                i = 2,
                                cell = new string[] {
                               items.grade.ToString(),
                               items.spaingrade.ToString(),
                               items.studentcount.ToString(),
                               items.occupancy.ToString(),
                               items.percentage.ToString() +'%'
                            }
                            })
                };

                return Json(jsondat, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetGradeStudentCount(string campus, string ExportType, string year)
        {
            DataTable dt = GradeStudentCount(campus, year);
            var list = dt.AsEnumerable()
             .Select(row => new
             {
                 studentcount = row["studentcount"],
                 grade = row["grade"],
                 spaingrade = row["SpainGrade"],
                 occupancy = row["occupancy"],
                 percentage = row["percentage"]
             }).ToList();

            if (ExportType == "Excel")
            {
                var grid = new GridView
                {
                    DataSource = from document in list
                                 select new
                                 {
                                     CampusName = document.grade.ToString() != "Grand Total" ? campus.ToString() : "",
                                     Grade = document.grade.ToString(),
                                     SpainGrade = document.spaingrade.ToString(),
                                     StudentCount = document.studentcount.ToString(),
                                     MaxStudentCount = document.occupancy.ToString(),
                                     OccupancyPercentage = document.percentage.ToString() + '%',

                                 }
                };

                grid.DataBind();

                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment; filename=GradeStudentCount.xls");
                Response.ContentType = "application/vnd.ms-excel";

                StringWriter sw = new StringWriter();

                HtmlTextWriter htw = new HtmlTextWriter(sw);
                grid.HeaderStyle.BackColor = System.Drawing.Color.FromName("#B6B6B6");
                grid.HeaderStyle.Font.Bold = true;
                grid.HeaderStyle.ForeColor = System.Drawing.Color.White;
                grid.RenderControl(htw);

                Response.Write(sw.ToString());

                Response.End();
                return new EmptyResult();

            }
            else
            {
                var jsondat = new
                {
                    total = 10,
                    page = 1,
                    records = 50,

                    rows = (from items in list
                            select new
                            {
                                i = 2,
                                cell = new string[] {
                               items.grade.ToString(),
                               items.spaingrade.ToString(),
                               items.studentcount.ToString(),
                               items.occupancy.ToString(),
                               items.percentage.ToString() +'%'
                            }
                            })
                };

                return Json(jsondat, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetSectionStudentCount(string campus, string grade, string ExportType, string year)
        {
            DataTable dt = SectionStudentCount(campus, grade, year);
            var list = dt.AsEnumerable()
             .Select(row => new
             {
                 studentcount = row["studentcount"],
                 section = row["section"],
                 occupancy = row["occupancy"],
                 percentage = row["percentage"]
             }).ToList();

            if (ExportType == "Excel")
            {
                var grid = new GridView
                {
                    DataSource = from document in list
                                 select new
                                 {
                                     CampusName = document.section.ToString() != "Grand Total" ? campus.ToString() : " ",
                                     GradeLevel = document.section.ToString() != "Grand Total" ? grade.ToString() : " ",
                                     Section = document.section.ToString(),
                                     StudentCount = document.studentcount.ToString(),
                                     MaxStudentCount = document.occupancy.ToString(),
                                     OccupancyPercentage = document.percentage.ToString() + '%',

                                 }
                };

                grid.DataBind();

                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment; filename=SectionStudentCount.xls");
                Response.ContentType = "application/vnd.ms-excel";

                StringWriter sw = new StringWriter();

                HtmlTextWriter htw = new HtmlTextWriter(sw);
                grid.HeaderStyle.BackColor = System.Drawing.Color.FromName("#B6B6B6");
                grid.HeaderStyle.Font.Bold = true;
                grid.HeaderStyle.ForeColor = System.Drawing.Color.White;
                grid.RenderControl(htw);

                Response.Write(sw.ToString());

                Response.End();
                return new EmptyResult();

            }
            else
            {
                var jsondat = new
                {
                    total = 10,
                    page = 1,
                    records = 50,

                    rows = (from items in list
                            select new
                            {
                                i = 2,
                                cell = new string[] {
                               items.section.ToString(),
                               items.studentcount.ToString(),
                               items.occupancy.ToString(),
                               items.percentage.ToString() +'%'
                            }
                            })
                };

                return Json(jsondat, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetStudentCount(string year, string campus, string grade, string ExportType)
        {
            DataTable dt = StudentCountGrid(year, campus, grade);

            var list = dt.AsEnumerable()
             .Select(row => new
             {
                 period = row["period"],
                 studentcount = row["studentcount"]
             }).ToList();
            if (ExportType == "Excel")
            {
                var grid = new GridView();
                if (year == "All")
                {
                    if ((campus == "" && grade == "")||(campus == "All" && grade == "All"))
                    {
                        grid = new GridView
                       {
                           DataSource = from document in list
                                        select new{
                                            Period = document.period.ToString(),
                                            StudentCount = document.studentcount.ToString(),
                                        }};
                        grid.DataBind();
                    }
                    else if ((campus != "" && grade == "") || (campus != "All" && grade == "All"))
                    {
                        grid = new GridView
                       {
                           DataSource = from document in list
                                        select new{
                                            Period = document.period.ToString(),
                                            Campus = campus.ToString(),
                                            StudentCount = document.studentcount.ToString(),
                                        }};
                        grid.DataBind();
                    }
                    else if ((campus == "" && grade != "")||(campus == "All" && grade != "All"))
                    {
                        grid = new GridView
                       {
                           DataSource = from document in list
                                        select new
                                        {
                                            Period = document.period.ToString(),
                                            Grade = grade.ToString(),
                                            StudentCount = document.studentcount.ToString(),
                                        }
                       };
                        grid.DataBind();
                    }
                    else if ((campus != "" && grade != "")||(campus != "All" && grade != "All"))
                    {
                        grid = new GridView
                        {
                            DataSource = from document in list
                                         select new
                                         {
                                             Period = document.period.ToString(),
                                             Campus = campus.ToString(),
                                             Grade = grade.ToString(),
                                             StudentCount = document.studentcount.ToString(),
                                         }
                        };
                        grid.DataBind();
                    }
                }
                else if (year != "All" && (grade == "" && campus == "") || (grade == "All" && campus == "All"))
                {
                    grid = new GridView
                   {
                       DataSource = from document in list
                                    select new
                                    {
                                        Year = year.ToString(),
                                        Period = document.period.ToString(),
                                        StudentCount = document.studentcount.ToString(),
                                    }
                   };
                    grid.DataBind();
                }
                else if (year != "All" && (campus != "" && grade != "") || (campus != "All" && grade != "All"))
                {
                    grid = new GridView
                   {
                       DataSource = from document in list
                                    select new
                                    {
                                        Year = year.ToString(),
                                        Period = document.period.ToString(),
                                        Campus = campus,
                                        Grade = grade,
                                        StudentCount = document.studentcount.ToString(),
                                    }
                   };
                    grid.DataBind();
                }
                else if (year != "All" && (campus != "" && grade == "") || (campus != "All" && grade == "All"))
                {
                    grid = new GridView
                   {
                       DataSource = from document in list
                                    select new
                                    {
                                        Year = year,
                                        Period = document.period.ToString(),
                                        Campus = campus,
                                        StudentCount = document.studentcount.ToString(),
                                    }
                   };
                    grid.DataBind();
                }
                else if (year != "All" && (grade != "" && campus == "") || (grade != "All" && campus == "All"))
                {
                    grid = new GridView
                   {
                       DataSource = from document in list
                                    select new
                                    {
                                        Year = year,
                                        Period = document.period.ToString(),
                                        Grade = grade,
                                        StudentCount = document.studentcount.ToString(),
                                    }
                   };
                    grid.DataBind();
                }

                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment; filename=StudentCount.xls");
                Response.ContentType = "application/vnd.ms-excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                grid.HeaderStyle.BackColor = System.Drawing.Color.FromName("#B6B6B6");
                grid.HeaderStyle.Font.Bold = true;
                grid.HeaderStyle.ForeColor = System.Drawing.Color.White;
                grid.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.End();
                return new EmptyResult();
            }
            else
            {
                if (year == "All")
                {
                    if ((campus == "" && grade == "") || (campus == "All" && grade == "All"))
                    {
                        var jsondat = new
                        {
                            total = 10,
                            page = 1,
                            records = 50,

                            rows = (from items in list
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                               items.period.ToString(),
                               items.studentcount.ToString(),
                            }
                                    })
                        };
                        return Json(jsondat, JsonRequestBehavior.AllowGet);
                    }
                    else if ((campus != "" && grade == "") || (campus != "All" && grade == "All"))
                    {
                        var jsondat = new
                        {
                            total = 10,
                            page = 1,
                            records = 50,

                            rows = (from items in list
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                               items.period.ToString(),
                               campus.ToString(),
                               items.studentcount.ToString(),
                            }
                                    })
                        };
                        return Json(jsondat, JsonRequestBehavior.AllowGet);
                    }
                    else if ((campus == "" && grade != "") || (campus == "All" && grade != "All"))
                    {
                        var jsondat = new
                        {
                            total = 10,
                            page = 1,
                            records = 50,

                            rows = (from items in list
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                               items.period.ToString(),
                               grade.ToString(),
                               items.studentcount.ToString(),
                            }
                                    })
                        };
                        return Json(jsondat, JsonRequestBehavior.AllowGet);
                    }
                    else if ((campus != "" && grade != "") || (campus != "All" && grade != "All"))
                    {
                        var jsondat = new
                        {
                            total = 10,
                            page = 1,
                            records = 50,

                            rows = (from items in list
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                               items.period.ToString(),
                               campus.ToString(),
                               grade.ToString(),
                               items.studentcount.ToString(),
                            }
                                    })
                        };
                        return Json(jsondat, JsonRequestBehavior.AllowGet);
                    }
                }
                else if (year != "All" && (grade == "" && campus == "")||(grade == "All" && campus == "All"))
                {
                    var jsondat = new
                    {
                        total = 10,
                        page = 1,
                        records = 50,

                        rows = (from items in list
                                select new
                                {
                                    i = 2,
                                    cell = new string[] {
                                        year.ToString(),
                                        items.period.ToString(),
                                        items.studentcount.ToString(),
                            }
                                })
                    };
                    return Json(jsondat, JsonRequestBehavior.AllowGet);
                }
                else if (year != "All" && (campus != "" && grade != "")||(campus != "All" && grade != "All"))
                {
                    var jsondat = new
                    {
                        total = 10,
                        page = 1,
                        records = 50,

                        rows = (from items in list
                                select new
                                {
                                    i = 2,
                                    cell = new string[] {
                                        year.ToString(),
                                        items.period.ToString(),
                                        campus.ToString(),
                                        grade.ToString(),
                                        items.studentcount.ToString(),
                            }
                                })
                    };
                    return Json(jsondat, JsonRequestBehavior.AllowGet);
                }
                else if (year != "All" && (campus != "" && grade == "")||(campus != "All" && grade == "All"))
                {
                    var jsondat = new
                    {
                        total = 10,
                        page = 1,
                        records = 50,

                        rows = (from items in list
                                select new
                                {
                                    i = 2,
                                    cell = new string[] {
                                        year.ToString(),
                                        items.period.ToString(),
                                        campus.ToString(),
                                        items.studentcount.ToString(),
                            }
                                })
                    };
                    return Json(jsondat, JsonRequestBehavior.AllowGet);
                }
                else if (year != "All" && (grade != "" && campus == "") || grade != "All" && campus == "All")
                {
                    var jsondat = new
                    {
                        total = 10,
                        page = 1,
                        records = 50,

                        rows = (from items in list
                                select new
                                {
                                    i = 2,
                                    cell = new string[] {
                                        year.ToString(),
                                        items.period.ToString(),
                                        grade.ToString(),
                                        items.studentcount.ToString(),
                              
                            }
                                })
                    };
                    return Json(jsondat, JsonRequestBehavior.AllowGet);
                }

            }
            return null;
        }

        public ActionResult GetTotalStudentCount(string year, string campus, string ExportType)
        {
            DataTable dt = StudentCountReportGrid(year, campus);

            var list = dt.AsEnumerable()
             .Select(row => new
             {
                 campus = row["campus"],
                 grade = row["grade"],
                 sectioncount = row["sectioncount"],
                 studentcount = row["studentcount"],
                 occupancy = row["occupancy"],
                 availablecapacity = row["availablecapacity"]
             }).ToList();

            if (ExportType == "Excel")
            {
                var grid = new GridView();

                grid = new GridView
                {
                    DataSource = from document in list
                                 select new
                                 {
                                     Campus = document.campus.ToString(),
                                     Grade = document.grade.ToString(),
                                     SectionCount = document.sectioncount.ToString(),
                                     StudentCount = document.studentcount.ToString(),
                                     Occupancy = document.occupancy.ToString(),
                                     AvailableCapacity = document.availablecapacity.ToString(),
                                 }
                };
                grid.DataBind();

                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment; filename=StudentCountReport.xls");
                Response.ContentType = "application/vnd.ms-excel";

                StringWriter sw = new StringWriter();

                HtmlTextWriter htw = new HtmlTextWriter(sw);
                grid.HeaderStyle.BackColor = System.Drawing.Color.FromName("#B6B6B6");
                grid.HeaderStyle.Font.Bold = true;
                grid.HeaderStyle.ForeColor = System.Drawing.Color.White;
                grid.RenderControl(htw);

                Response.Write(sw.ToString());

                Response.End();
                return new EmptyResult();
            }
            else
            {
                var jsondat = new
                {
                    total = 10,
                    page = 1,
                    records = 50,

                    rows = (from items in list
                            select new
                            {
                                i = 2,
                                cell = new string[] {
                               items.campus.ToString(),
                               items.grade.ToString(),
                               items.sectioncount.ToString(),
                               items.studentcount.ToString(),
                               items.occupancy.ToString(),
                               items.availablecapacity.ToString(),
                            }
                            })
                };
                return Json(jsondat, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        // For Grid End

        public ActionResult StudentByCampusReport()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    List<SelectListItem> items = new List<SelectListItem>();
                    items.Add(new SelectListItem { Text = "By Campus", Value = "Campus", Selected = true });
                    items.Add(new SelectListItem { Text = "By Grade", Value = "Grade" });
                    items.Add(new SelectListItem { Text = "By Section", Value = "Section" });

                    ViewBag.ReportType = items;
                    IList<CampusMaster> campusMsObj = CampusMasterFunc();
                    IList<GradeMaster> gradeMsObj = GradeMasterFunc();
                    ViewBag.CampusType = campusMsObj;
                    ViewBag.GradeType = gradeMsObj;
                    List<DataRow> DashBoardList = null;
                    if (GetAcademicYear() != null)
                    {
                        DashBoardList = GetAcademicYear().AsEnumerable().ToList();

                    }
                    List<SelectListItem> yearlist = new List<SelectListItem>();
                    yearlist.Add(new SelectListItem { Text = "All", Value = "All", Selected = true });
                    foreach (var item in DashBoardList)
                    {
                        yearlist.Add(new SelectListItem { Text = item[0].ToString(), Value = item[0].ToString() });
                    }
                    ViewBag.YearList = yearlist;
                    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "Assess360Policy");
                throw ex;
            }
        }

        public ActionResult StudentReport()
        {
            try
            {
                 string userId = base.ValidateUser();
                 if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                 else
                 {
                     List<DataRow> DashBoardList = null;
                     if (GetYear() != null)
                     {
                         DashBoardList = GetYear().AsEnumerable().ToList();
                     }
                     List<SelectListItem> yearlist = new List<SelectListItem>();
                     yearlist.Add(new SelectListItem { Text = "All Year", Value = "All", Selected = true });
                     foreach (var items in DashBoardList)
                     {
                         yearlist.Add(new SelectListItem { Text = items[0].ToString(), Value = items[0].ToString() });
                     }
                     ViewBag.YearType = yearlist;
                     List<SelectListItem> monthlist = new List<SelectListItem>();
                     monthlist.Add(new SelectListItem { Text = "All Month", Value = "All", Selected = true });
                     monthlist.Add(new SelectListItem { Text = "January", Value = "January" });
                     monthlist.Add(new SelectListItem { Text = "February", Value = "February" });
                     monthlist.Add(new SelectListItem { Text = "March", Value = "March" });
                     monthlist.Add(new SelectListItem { Text = "April", Value = "April" });
                     monthlist.Add(new SelectListItem { Text = "May", Value = "May" });
                     monthlist.Add(new SelectListItem { Text = "June", Value = "June" });
                     monthlist.Add(new SelectListItem { Text = "July", Value = "July" });
                     monthlist.Add(new SelectListItem { Text = "August", Value = "August" });
                     monthlist.Add(new SelectListItem { Text = "September", Value = "September" });
                     monthlist.Add(new SelectListItem { Text = "October", Value = "October" });
                     monthlist.Add(new SelectListItem { Text = "November", Value = "November" });
                     monthlist.Add(new SelectListItem { Text = "December", Value = "December" });

                     ViewBag.MonthType = monthlist;

                     IList<CampusMaster> campusMsObj = CampusMasterFunc();
                     IList<GradeMaster> gradeMsObj = GradeMasterFunc();
                     ViewBag.CampusType = campusMsObj;
                     ViewBag.GradeType = gradeMsObj;
                     string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                     string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                     ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                     return View();
                 }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "Assess360Policy");
                throw ex;
            }
        }

        public ActionResult StudentCountReport()
        {
            List<DataRow> DashBoardList = null;

            if (GetAcademicYear() != null)
            {
                DashBoardList = GetAcademicYear().AsEnumerable().ToList();

            }
            List<SelectListItem> yearlist = new List<SelectListItem>();
            yearlist.Add(new SelectListItem { Text = "All AcademicYear", Value = "All", Selected = true });
            foreach (var items in DashBoardList)
            {
                yearlist.Add(new SelectListItem { Text = items[0].ToString(), Value = items[0].ToString() });
            }
            ViewBag.YearType = yearlist;

            List<SelectListItem> monthlist = new List<SelectListItem>();

            monthlist.Add(new SelectListItem { Text = "All Month", Value = "All", Selected = true });
            monthlist.Add(new SelectListItem { Text = "January", Value = "January" });
            monthlist.Add(new SelectListItem { Text = "February", Value = "February" });
            monthlist.Add(new SelectListItem { Text = "March", Value = "March" });
            monthlist.Add(new SelectListItem { Text = "April", Value = "April" });
            monthlist.Add(new SelectListItem { Text = "May", Value = "May" });
            monthlist.Add(new SelectListItem { Text = "June", Value = "June" });
            monthlist.Add(new SelectListItem { Text = "July", Value = "July" });
            monthlist.Add(new SelectListItem { Text = "August", Value = "August" });
            monthlist.Add(new SelectListItem { Text = "September", Value = "September" });
            monthlist.Add(new SelectListItem { Text = "October", Value = "October" });
            monthlist.Add(new SelectListItem { Text = "November", Value = "November" });
            monthlist.Add(new SelectListItem { Text = "December", Value = "December" });

            ViewBag.MonthType = monthlist;
            IList<CampusMaster> campusMsObj = CampusMasterFunc();
            IList<GradeMaster> gradeMsObj = GradeMasterFunc();
            ViewBag.CampusType = campusMsObj;
            ViewBag.GradeType = gradeMsObj;

            return View();
        }

        // For Chart Start

        public ActionResult StudentByCampus(string year)
        {
            AdmissionManagementService us = new AdmissionManagementService();

            var strsql = "";
            strsql = strsql + " select Campus,SUM(studentcount) as studentcount,SUM(occupancy) as occupancy from(";
            strsql = strsql + "select a.campus,a.grade,a.section,a.studentcount,b.occupancy from( ";
            strsql = strsql + "select campus,grade,section,COUNT(*) as studentcount from StudentTemplate  ";
            if (year == "All")
            {
                strsql = strsql + "where AdmissionStatus='Registered' and Section is not null ";
            }
            else
            {
                strsql = strsql + "where AdmissionStatus='Registered' and Section is not null and JoiningAcademicYear = '" + year + "' ";
            }
            strsql = strsql + "group by campus,grade,section ) a ";
            //strsql = strsql + "left join (select Campus,Grade,occupancy,acadamicyear from OccupancyMaster where acadamicyear='2013-2014') b ";
            strsql = strsql + "left join (select Campus,Grade,occupancy,acadamicyear from OccupancyMaster) b ";
            strsql = strsql + "on a.campus= b.campus and a.grade = b.grade )c ";
            //strsql = strsql + "group by Campus,academicyear ";
            strsql = strsql + "group by Campus";

            List<DataRow> DashBoardList = null;

            DataTable StudentList = us.GetDashBoardStudentList(strsql);

            if (StudentList != null)
            {
                DashBoardList = StudentList.AsEnumerable().ToList();

            }

            var strxml = "";
            if (StudentList != null)
            {
                strxml = strxml = "<graph rotateNames='1' caption='All Campus Students Count' xAxisName='Campus' yAxisName='Student Count' divLineColor='91AF46' formatNumber='0' formatNumberScale='0' decimalPrecision='0'>";
                // strxml = strxml + "<graph caption='Yearly Students Count' showValues='0' plotFillAlpha='95' formatNumberScale='0'>";

                strxml = strxml + "<categories>";
                foreach (var items in DashBoardList)
                {
                    strxml = strxml + "        <category name='" + items[0] + "'/>";
                }
                strxml = strxml + "</categories>";

                strxml = strxml + "<dataset seriesName='Student Count' color='AFD8F8'>";
                foreach (var items in DashBoardList)
                {
                    strxml = strxml + "        <set value='" + items[1] + "' />";
                }
                strxml = strxml + "</dataset>";

                strxml = strxml + "<dataset seriesName='Total Occupancy' color='F6BD0F'>";
                foreach (var items in DashBoardList)
                {
                    strxml = strxml + "        <set value='" + items[2] + "' />";
                }
                strxml = strxml + "</dataset>";


                strxml = strxml + "</graph>";
            }
            ViewBag.List = DashBoardList;
            Response.Write(strxml);
            return null;
        }

        public ActionResult StudentByCampusAllgrade(string year)
        {
            AdmissionManagementService us = new AdmissionManagementService();

            var strsql = "";

            strsql = strsql + "select Grade,SUM(studentcount) as studentcount,SUM(occupancy) as occupancy ,round(((SUM(studentcount)/SUM(occupancy))*100),2) as percentage from( ";
            strsql = strsql + "select a.campus,a.grade,a.section,a.studentcount,cast(b.occupancy as float) as occupancy from( ";
            strsql = strsql + "select campus,grade,section,cast(COUNT(*) as float) as studentcount from StudentTemplate  ";
            if (year == "All")
            {
                strsql = strsql + "where AdmissionStatus='Registered' and Section is not null ";
            }
            else
            {
                strsql = strsql + "where AdmissionStatus='Registered' and Section is not null and JoiningAcademicYear = '" + year + "' ";
            }
            strsql = strsql + "group by campus,grade,section ) a ";
            // strsql = strsql + "left join (select Campus,Grade,occupancy,acadamicyear from OccupancyMaster where acadamicyear='2013-2014' ";
            strsql = strsql + "left join (select Campus,Grade,occupancy,acadamicyear from OccupancyMaster ";
            strsql = strsql + ") b ";
            strsql = strsql + "on a.campus= b.campus and a.grade = b.grade )c ";
            //  strsql = strsql + "group by Grade,academicyear ";
            strsql = strsql + "group by Grade ";

            List<DataRow> DashBoardList = null;

            DataTable StudentList = us.GetDashBoardStudentList(strsql);

            if (StudentList != null)
            {
                DashBoardList = StudentList.AsEnumerable().ToList();

            }

            var strxml = "";
            if (StudentList != null)
            {
                strxml = strxml = "<graph rotateNames='1' caption='All Campus Grade Wise Students Count' xAxisName='Grade' yAxisName='Student Count' divLineColor='91AF46' formatNumber='0' formatNumberScale='0' decimalPrecision='0'>";
                // strxml = strxml + "<graph caption='Yearly Students Count' showValues='0' plotFillAlpha='95' formatNumberScale='0'>";

                strxml = strxml + "<categories>";
                foreach (var items in DashBoardList)
                {
                    strxml = strxml + "        <category name='" + items[0] + "'/>";
                }
                strxml = strxml + "</categories>";

                strxml = strxml + "<dataset seriesName='Student Count' color='AFD8F8'>";
                foreach (var items in DashBoardList)
                {
                    strxml = strxml + "        <set value='" + items[1] + "' />";
                }
                strxml = strxml + "</dataset>";

                strxml = strxml + "<dataset seriesName='Total Occupancy' color='F6BD0F'>";
                foreach (var items in DashBoardList)
                {
                    strxml = strxml + "        <set value='" + items[2] + "' />";
                }
                strxml = strxml + "</dataset>";

                strxml = strxml + "</graph>";
            }
            ViewBag.List = DashBoardList;
            Response.Write(strxml);
            return null;
        }

        public ActionResult StudentByCampusAllSection(string campus)
        {
            AdmissionManagementService us = new AdmissionManagementService();

            var strsql = "";

            strsql = strsql + "select section,COUNT(*) as studentcount from studenttemplate ";
            strsql = strsql + "where AdmissionStatus='Registered' and campus='" + campus + "' and section is not null ";
            strsql = strsql + " group by section ";
            strsql = strsql + " order by section ";

            List<DataRow> DashBoardList = null;

            DataTable StudentList = us.GetDashBoardStudentList(strsql);

            if (StudentList != null)
            {
                DashBoardList = StudentList.AsEnumerable().ToList();

            }

            var strxml = "";
            if (StudentList != null)
            {
                strxml = strxml = "<graph rotateNames='1' caption='" + campus + "- Campus All Section Wise Students Count' xAxisName='Student Count' divLineColor='91AF46' formatNumber='0' formatNumberScale='0' decimalPrecision='0'>";
                // strxml = strxml + "<graph caption='Yearly Students Count' showValues='0' plotFillAlpha='95' formatNumberScale='0'>";

                foreach (var items in DashBoardList)
                {
                    strxml = strxml + "        <set name = '" + items[0] + "' value='" + items[1] + "' link='/DashBoard/ManagementList?Report=Enquiry&Status=Total&Date=" + items[0] + "'/>";
                }
                strxml = strxml + "</graph>";
            }
            ViewBag.List = DashBoardList;
            Response.Write(strxml);
            return null;
        }

        public ActionResult StudentByGrade(string campus, string year)
        {
            AdmissionManagementService us = new AdmissionManagementService();

            var strsql = "";

            strsql = strsql + " select Campus,grade,SUM(studentcount) as studentcount,SUM(occupancy) as occupancy from( ";
            strsql = strsql + "select a.campus,a.grade,a.section,a.studentcount,b.occupancy from( ";
            strsql = strsql + "select campus,grade,section,COUNT(*) as studentcount from StudentTemplate ";
            if (year == "All")
            {
                strsql = strsql + "where AdmissionStatus='Registered' and Section is not null and Campus='" + campus + "'  ";
            }
            else
            {
                strsql = strsql + "where AdmissionStatus='Registered' and Section is not null and Campus='" + campus + "' and JoiningAcademicYear='" + year + "' ";
            }
            strsql = strsql + "group by campus,grade,section ) a ";
            strsql = strsql + "left join (select Campus,Grade,occupancy,acadamicyear from OccupancyMaster where Campus='" + campus + "') b ";
            strsql = strsql + "on a.campus= b.campus and a.grade = b.grade )c ";
            strsql = strsql + "group by Campus,grade";

            List<DataRow> DashBoardList = null;

            DataTable StudentList = us.GetDashBoardStudentList(strsql);

            if (StudentList != null)
            {
                DashBoardList = StudentList.AsEnumerable().ToList();

            }

            var strxml = "";
            if (StudentList != null)
            {
                strxml = strxml = "<graph caption='" + campus + "- Campus GradeWise Students Count' xAxisName='Grade' yAxisName='Student Count' divLineColor='91AF46' formatNumber='0' formatNumberScale='0' decimalPrecision='0'>";

                strxml = strxml + "<categories>";
                foreach (var items in DashBoardList)
                {
                    strxml = strxml + "        <category name='" + items[1] + "'/>";
                }
                strxml = strxml + "</categories>";

                strxml = strxml + "<dataset seriesName='Student Count' color='AFD8F8'>";
                foreach (var items in DashBoardList)
                {
                    strxml = strxml + "        <set value='" + items[2] + "' />";
                }
                strxml = strxml + "</dataset>";

                strxml = strxml + "<dataset seriesName='Total Occupancy' color='F6BD0F'>";
                foreach (var items in DashBoardList)
                {
                    strxml = strxml + "        <set value='" + items[3] + "' />";
                }
                strxml = strxml + "</dataset>";

                strxml = strxml + "</graph>";
            }
            ViewBag.List = DashBoardList;
            Response.Write(strxml);
            return null;
        }

        public ActionResult StudentBySection(string campus, string grade, string year)
        {
            AdmissionManagementService us = new AdmissionManagementService();

            var strsql = "";
            strsql = strsql + "select Section,SUM(studentcount) as studentcount,SUM(occupancy) as occupancy ,round(((SUM(studentcount)/SUM(occupancy))*100),2) as percentage from( ";
            strsql = strsql + "select a.campus,a.grade,a.section,a.studentcount,cast(b.occupancy as float) as occupancy from( ";
            strsql = strsql + "select campus,grade,section,cast(COUNT(*) as float) as studentcount from StudentTemplate  ";
            if (year == "All")
            {
                strsql = strsql + "where AdmissionStatus='Registered' and Section is not null and Campus='" + campus + "' and Grade='" + grade + "' ";
            }
            else
            {
                strsql = strsql + "where AdmissionStatus='Registered' and Section is not null and Campus='" + campus + "' and Grade='" + grade + "' and JoiningAcademicYear='" + year + "' ";
            }
            strsql = strsql + "group by campus,grade,section ) a ";
            // strsql = strsql + "left join (select Campus,Grade,occupancy,acadamicyear from OccupancyMaster where acadamicyear='2013-2014' and Campus='" + campus + "' and grade='" + grade + "'  ";
            strsql = strsql + "left join (select Campus,Grade,occupancy,acadamicyear from OccupancyMaster where Campus='" + campus + "' and grade='" + grade + "'  ";
            strsql = strsql + ") b ";
            strsql = strsql + "on a.campus= b.campus and a.grade = b.grade )c ";
            // strsql = strsql + "group by Section,academicyear ";
            strsql = strsql + "group by Section ";

            List<DataRow> DashBoardList = null;

            DataTable StudentList = us.GetDashBoardStudentList(strsql);

            if (StudentList != null)
            {
                DashBoardList = StudentList.AsEnumerable().ToList();

            }

            var strxml = "";
            if (StudentList != null)
            {
                strxml = strxml = "<graph caption='" + campus + "- Campus " + grade + "- Grade Section Wise Students Count' xAxisName='Section' yAxisName='Student Count' divLineColor='91AF46' formatNumber='0' formatNumberScale='0' decimalPrecision='0'>";

                strxml = strxml + "<categories>";
                foreach (var items in DashBoardList)
                {
                    strxml = strxml + "        <category name='" + items[0] + "'/>";
                }
                strxml = strxml + "</categories>";

                strxml = strxml + "<dataset seriesName='Student Count' color='AFD8F8'>";
                foreach (var items in DashBoardList)
                {
                    strxml = strxml + "        <set value='" + items[1] + "' />";
                }
                strxml = strxml + "</dataset>";

                strxml = strxml + "<dataset seriesName='Total Occupancy' color='F6BD0F'>";
                foreach (var items in DashBoardList)
                {
                    strxml = strxml + "        <set value='" + items[2] + "' />";
                }
                strxml = strxml + "</dataset>";
                strxml = strxml + "</graph>";

            }
            ViewBag.List = DashBoardList;
            Response.Write(strxml);
            return null;
        }

        public ActionResult StudentCount(string year, string campus, string grade)
        {
            AdmissionManagementService us = new AdmissionManagementService();

            var strsql = "";
            if (year == "All")
            {
                strsql = strsql + "select Year,SUM(studentcount) as studentcount from ( ";
                strsql = strsql + "select DATEPART(YEAR,CreatedDateNew)as year, ";
                strsql = strsql + "campus,grade,section,COUNT(*)as studentcount  ";
            }
            else
            {
                strsql = strsql + "select month,SUM(studentcount) as studentcount,monthnumber from ( ";
                strsql = strsql + "select datename(MONTH,CreatedDateNew)as month, ";
                strsql = strsql + "campus,grade,section,COUNT(*)as studentcount,DATEPART(month,CreatedDateNew)as monthnumber  ";
            }
            strsql = strsql + "from studenttemplate where AdmissionStatus='Registered' and CreatedDateNew is not null ";
            if (year != "All")
            {
                strsql = strsql + "and DATEPART(YEAR,CreatedDateNew) = '" + year + "' ";
            }
            if (campus != "")
            {
                strsql = strsql + "and Campus='" + campus + "' ";
            }
            if (grade != "")
            {
                strsql = strsql + "and Grade='" + grade + "' ";
            }

            if (year == "All")
            {
                strsql = strsql + "group by DATEPART(YEAR,CreatedDateNew),campus,grade,section ) a ";
                strsql = strsql + "group by year ";
                strsql = strsql + "order by year ";
            }

            if (year != "All")
            {
                strsql = strsql + "group by datename(MONTH,CreatedDateNew),campus,grade,section,DATEPART(month,CreatedDateNew) ) a ";
                strsql = strsql + "group by month,monthnumber ";
                strsql = strsql + "order by monthnumber ";
            }
            List<DataRow> DashBoardList = null;

            DataTable StudentList = us.GetDashBoardStudentList(strsql);

            if (StudentList != null)
            {
                DashBoardList = StudentList.AsEnumerable().ToList();
            }

            var strxml = "";
            if (StudentList != null)
            {
                if (year == "All")
                {
                    strxml = strxml = "<graph caption='" + year + "- Year " + campus + " - Campus " + grade + " - Grade Students Count' xAxisName='Year' yAxisName='Student Count' divLineColor='91AF46' formatNumber='0' formatNumberScale='0' decimalPrecision='0' rotateNames='1'>";
                }
                else
                {
                    strxml = strxml = "<graph caption='" + year + "- Year " + campus + " - Campus " + grade + " - Grade Students Count' xAxisName='Month' yAxisName='Student Count' divLineColor='91AF46' formatNumber='0' formatNumberScale='0' decimalPrecision='0' rotateNames='1'>";
                }

                foreach (var items in DashBoardList)
                {
                    strxml = strxml + "        <set name='" + items[0] + "' value='" + items[1] + "' />";
                }

                strxml = strxml + "</graph>";

            }
            ViewBag.List = DashBoardList;
            Response.Write(strxml);
            return null;
        }

        public ActionResult TotalStudentCount(string year, string campus)
        {
            AdmissionManagementService us = new AdmissionManagementService();

            var strsql = "";
            if (campus == "All")
            {
                strsql = strsql + "select campus,grade,sectioncount,studentcount,occupancy from ( ";
                strsql = strsql + "select isnull(campus,'All') as campus,isnull(Grade,'Grand Total') as grade,SUM(sectioncount) as sectioncount,SUM(studentcount) as studentcount,SUM(occupancy) as occupancy ";
                strsql = strsql + "from ( ";
                strsql = strsql + "select 'All' as campus,grade,COUNT(section) as sectioncount,SUM(studentcount) as studentcount,SUM(occupancy) as occupancy from ( ";
                strsql = strsql + "select a.campus,a.grade,a.section,a.studentcount,cast(b.occupancy as float) as occupancy from( ";
                strsql = strsql + "select campus,grade,section,cast(COUNT(*) as float) as studentcount from StudentTemplate  ";
                strsql = strsql + "where AdmissionStatus='Registered' and Section is not null ";
                if (year != "All")
                {
                    strsql = strsql + "and JoiningAcademicYear = '" + year + "' ";
                }
                //strsql = strsql + "--and campus = 'CHENNAI CITY' and Grade='I' ";
                strsql = strsql + "group by campus,grade,section ) a ";
                strsql = strsql + "left join (select Campus,Grade,occupancy,acadamicyear from OccupancyMaster ";
                //strsql = strsql + "--where campus = 'CHENNAI CITY' and Grade='I' ";
                strsql = strsql + ") b ";
                strsql = strsql + "on a.campus= b.campus and a.grade = b.grade ) c ";
                strsql = strsql + "group by campus,Grade ) d ";
                strsql = strsql + "group by campus,Grade ";
                //strsql = strsql + "--group by GROUPING SETS ( (Campus,Grade),(Campus),()) ";
                strsql = strsql + "union all ";
                strsql = strsql + "select isnull(campus,'All') as campus,isnull(grade,'Grand Total') as grade,COUNT(section) as sectioncount,SUM(studentcount) as studentcount,SUM(occupancy) as occupancy from ( ";
                strsql = strsql + "select a.campus,a.grade,a.section,a.studentcount,cast(b.occupancy as float) as occupancy from(  ";
                strsql = strsql + "select campus,grade,section,cast(COUNT(*) as float) as studentcount from StudentTemplate  ";
                strsql = strsql + "where AdmissionStatus='Registered' and Section is not null ";
                if (year != "All")
                {
                    strsql = strsql + "and JoiningAcademicYear = '" + year + "' ";
                }
                //strsql = strsql + "--and campus = 'CHENNAI CITY' and Grade='I' ";
                strsql = strsql + "group by campus,grade,section ) a ";
                strsql = strsql + "left join (select Campus,Grade,occupancy,acadamicyear from OccupancyMaster ";
                //strsql = strsql + "--where campus = 'CHENNAI CITY' and Grade='I' ";
                strsql = strsql + ") b ";
                strsql = strsql + "on a.campus= b.campus and a.grade = b.grade ) c ";
                //strsql = strsql + "--group by Campus,Grade ";
                strsql = strsql + "group by GROUPING SETS ( (Campus,Grade),(Campus),())) d ";
                strsql = strsql + "where grade = 'Grand Total' ";
                strsql = strsql + "order by Campus ";
            }
            else
            {
                strsql = strsql + "select campus, isnull(grade,'Grand Total') as grade ,COUNT(section) as sectioncount,SUM(studentcount) as studentcount,SUM(occupancy) as occupancy from ( ";
                strsql = strsql + "select a.campus,a.grade,a.section,a.studentcount,cast(b.occupancy as float) as occupancy from(  ";
                strsql = strsql + "select campus,grade,section,cast(COUNT(*) as float) as studentcount from StudentTemplate   ";
                strsql = strsql + "where AdmissionStatus='Registered' and Section is not null and campus = '" + campus + "'  ";
                if (year != "All")
                {
                    strsql = strsql + "and JoiningAcademicYear = '" + year + "' ";
                }
                strsql = strsql + "group by campus,grade,section ) a  ";
                strsql = strsql + "left join (select Campus,Grade,occupancy,acadamicyear from OccupancyMaster where campus = '" + campus + "' ";
                strsql = strsql + ") b  ";
                strsql = strsql + "on a.campus= b.campus and a.grade = b.grade ) c ";
                strsql = strsql + "group by Campus,Grade ";
                //--group by GROUPING SETS ( (Campus,Grade),(Campus),()) 
                strsql = strsql + "order by Campus ";
            }


            List<DataRow> DashBoardList = null;

            DataTable StudentList = us.GetDashBoardStudentList(strsql);

            if (StudentList != null)
            {
                DashBoardList = StudentList.AsEnumerable().ToList();

            }

            var strxml = "";

            if (StudentList != null)
            {
                //strxml = strxml = "<graph caption='" + campus + "- Campus " + grade + "- Grade Section Wise Students Count' xAxisName='Section' yAxisName='Student Count' divLineColor='91AF46' formatNumber='0' formatNumberScale='0' decimalPrecision='0'>";
                strxml = strxml + "<graph rotateNames='1' caption='Yearly Students Count' showValues='0' plotFillAlpha='95' formatNumberScale='0' formatNumberScale='0' decimalPrecision='0' divLineColor='91AF46'>";

                if (campus == "All")
                {
                    strxml = strxml + "<categories>";
                    foreach (var items in DashBoardList)
                    {
                        strxml = strxml + "        <category name='" + items[0] + "'/>";
                    }
                    strxml = strxml + "</categories>";
                }
                else
                {
                    strxml = strxml + "<categories>";
                    foreach (var items in DashBoardList)
                    {
                        strxml = strxml + "        <category name='" + items[1] + "'/>";
                    }
                    strxml = strxml + "</categories>";
                }

                strxml = strxml + "<dataset seriesName='Section Count' color='FC0909'>";
                foreach (var items in DashBoardList)
                {
                    strxml = strxml + "        <set value='" + items[2] + "' />";
                }
                strxml = strxml + "</dataset>";

                strxml = strxml + "<dataset seriesName='Student Count' color='AFD8F8'>";
                foreach (var items in DashBoardList)
                {
                    strxml = strxml + "        <set value='" + items[3] + "' />";
                }
                strxml = strxml + "</dataset>";

                strxml = strxml + "<dataset seriesName='Total Occupancy' color='F6BD0F'>";
                foreach (var items in DashBoardList)
                {
                    // strxml = strxml + "        <set value='" + items[3] + "' link='/DashBoard/ManagementList?Report=Enquiry&Status=Total&Date=" + items[0] + "'/>";
                    strxml = strxml + "        <set value='" + items[4] + "' />";
                }
                strxml = strxml + "</dataset>";
                strxml = strxml + "</graph>";

            }
            ViewBag.List = DashBoardList;
            Response.Write(strxml);
            return null;
        }
        // For Chart End

        public ActionResult StudentReportChart(string fromdate, string todate)
        {
            AdmissionManagementService us = new AdmissionManagementService();

            var strsql = "";

            strsql = strsql + "select YEAR,ISNULL(CHENNAICITY,0) AS CHENNAICITY,ISNULL(CHENNAIMAIN,0) AS CHENNAIMAIN,ISNULL(ERNAKULAM,0) AS ERNAKULAM, ";
            strsql = strsql + "ISNULL(IBKG,0) AS IBKG,ISNULL(IBMAIN,0) AS IBMAIN,ISNULL(KARUR,0) AS KARUR, ";
            strsql = strsql + "ISNULL(TIPSERODE,0) AS TIPSERODE,ISNULL(TIPSSALEM,0) AS TIPSSALEM,ISNULL(TIPSSARAN,0) AS TIPSSARAN,ISNULL(TIRUPUR,0) AS TIRUPUR ";
            strsql = strsql + "from ( ";
            strsql = strsql + "select YEAR, ";
            strsql = strsql + "case when CAMPUS='CHENNAI CITY' THEN 'CHENNAICITY'  ";
            strsql = strsql + "when CAMPUS='CHENNAI MAIN' THEN 'CHENNAIMAIN' ";
            strsql = strsql + "when CAMPUS='ERNAKULAM' THEN 'ERNAKULAM' ";
            strsql = strsql + "when CAMPUS='IB KG' THEN 'IBKG' ";
            strsql = strsql + "when CAMPUS='IB MAIN' THEN 'IBMAIN' ";
            strsql = strsql + "when CAMPUS='KARUR' THEN 'KARUR' ";
            strsql = strsql + "when CAMPUS='TIPS ERODE' THEN 'TIPSERODE' ";
            strsql = strsql + "when CAMPUS='TIPS SALEM' THEN 'TIPSSALEM' ";
            strsql = strsql + "when CAMPUS='TIPS SARAN' THEN 'TIPSSARAN' ";
            strsql = strsql + "when CAMPUS='TIRUPUR' THEN 'TIRUPUR' ";
            strsql = strsql + "end AS CAMPUS, SUM(STUDENTCOUNT) AS COUNT ";
            strsql = strsql + "FROM ( ";
            strsql = strsql + "select DATEPART(YEAR,convert(datetime,createddate,103))as year,campus, COUNT(*)as studentcount ";
            strsql = strsql + "from StudentTemplate ";
            strsql = strsql + "where JoiningAcademicYear is not null and CONVERT(Datetime,createddate,103)>= CONVERT(datetime,'01/01/2007',103)  ";
            strsql = strsql + "and CONVERT(datetime,createddate,103)<= CONVERT(datetime,'31/12/2013',103) AND AdmissionStatus='Registered' ";
            strsql = strsql + "group by JoiningAcademicYear,CreatedDate,Campus) S ";
            strsql = strsql + "GROUP BY YEAR,CAMPUS ";
            strsql = strsql + ") as P ";
            strsql = strsql + "pivot ";
            strsql = strsql + "( ";
            strsql = strsql + "sum(Count) ";
            strsql = strsql + "for campus in (CHENNAICITY,CHENNAIMAIN,ERNAKULAM,IBKG,IBMAIN,KARUR,TIPSERODE,TIPSSALEM,TIPSSARAN,TIRUPUR) ";
            strsql = strsql + ") AS PVT ";
            strsql = strsql + "order by year ";

            List<DataRow> DashBoardList = null;

            DataTable StudentList = us.GetDashBoardStudentList(strsql);

            if (StudentList != null)
            {
                DashBoardList = StudentList.AsEnumerable().ToList();

            }

            var strxml = "";

            if (StudentList != null)
            {
                strxml = strxml + "<graph caption='Yearly Students Count' showValues='0' plotFillAlpha='95' formatNumberScale='0'>";

                strxml = strxml + "<categories>";
                foreach (var items in DashBoardList)
                {
                    strxml = strxml + "        <category name='" + items[0] + "'/>";
                }
                strxml = strxml + "</categories>";



                strxml = strxml + "<dataset seriesName='Chennai City' color='AFD8F8'>";
                foreach (var items in DashBoardList)
                {
                    strxml = strxml + "        <set value='" + items[1] + "' link='/DashBoard/ManagementList?Report=Enquiry&Status=Total&Date=" + items[0] + "'/>";
                }
                strxml = strxml + "</dataset>";

                strxml = strxml + "<dataset seriesName='Chennai Main' color='F6BD0F'>";
                foreach (var items in DashBoardList)
                {
                    strxml = strxml + "        <set value='" + items[2] + "' link='/DashBoard/ManagementList?Report=Enquiry&Status=Initiated&Date=" + items[0] + "'/>";
                }
                strxml = strxml + "</dataset>";

                strxml = strxml + "<dataset seriesName='Ernakulam' color='8BBA00'>";
                foreach (var items in DashBoardList)
                {
                    strxml = strxml + "        <set value='" + items[3] + "' link='/DashBoard/ManagementList?Report=Enquiry&Status=NotInitiated&Date=" + items[0] + "'/>";
                }
                strxml = strxml + "</dataset>";

                strxml = strxml + "<dataset seriesName='IB KG' color='ff4040'>";
                foreach (var items in DashBoardList)
                {
                    strxml = strxml + "        <set value='" + items[4] + "' link='/DashBoard/ManagementList?Report=Enquiry&Status=NotInitiated&Date=" + items[0] + "'/>";
                }
                strxml = strxml + "</dataset>";

                strxml = strxml + "<dataset seriesName='IB Main' color='ff7f24'>";
                foreach (var items in DashBoardList)
                {
                    strxml = strxml + "        <set value='" + items[5] + "' link='/DashBoard/ManagementList?Report=Enquiry&Status=NotInitiated&Date=" + items[0] + "'/>";
                }
                strxml = strxml + "</dataset>";

                strxml = strxml + "<dataset seriesName='Karur' color='bf3eff'>";
                foreach (var items in DashBoardList)
                {
                    strxml = strxml + "        <set value='" + items[6] + "' link='/DashBoard/ManagementList?Report=Enquiry&Status=NotInitiated&Date=" + items[0] + "'/>";
                }
                strxml = strxml + "</dataset>";

                strxml = strxml + "<dataset seriesName='Tips Erode' color='696969'>";
                foreach (var items in DashBoardList)
                {
                    strxml = strxml + "        <set value='" + items[7] + "' link='/DashBoard/ManagementList?Report=Enquiry&Status=NotInitiated&Date=" + items[0] + "'/>";
                }
                strxml = strxml + "</dataset>";

                strxml = strxml + "<dataset seriesName='Tips Salem' color='adff2f'>";
                foreach (var items in DashBoardList)
                {
                    strxml = strxml + "        <set value='" + items[8] + "' link='/DashBoard/ManagementList?Report=Enquiry&Status=NotInitiated&Date=" + items[0] + "'/>";
                }
                strxml = strxml + "</dataset>";

                strxml = strxml + "<dataset seriesName='Tips Saran' color='cd5c5c'>";
                foreach (var items in DashBoardList)
                {
                    strxml = strxml + "        <set value='" + items[9] + "' link='/DashBoard/ManagementList?Report=Enquiry&Status=NotInitiated&Date=" + items[0] + "'/>";
                }
                strxml = strxml + "</dataset>";

                strxml = strxml + "<dataset seriesName='Tirupur' color='ff3030'>";
                foreach (var items in DashBoardList)
                {
                    strxml = strxml + "        <set value='" + items[10] + "' link='/DashBoard/ManagementList?Report=Enquiry&Status=NotInitiated&Date=" + items[0] + "'/>";
                }
                strxml = strxml + "</dataset>";

                strxml = strxml + "</graph>";
            }

            Response.Write(strxml);
            return null;
        }


        //New DashBoard Design

        public ActionResult NewDashboard()
        {
            return View();
        }
        
    }
}
