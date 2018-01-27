using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using CMS.Utilities.ChartUtilities;
using TIPS.Service;
using TIPS.Entities.Assess;
using TIPS.Entities.AdmissionEntities;


namespace CMS.Models
{
    public class RptCardShareModel
    {
        public class Biology
        {
            public string Name { get; set; }
            public double Mark { get; set; }
        }
        public class Chemistry
        {
            public string Name { get; set; }
            public double Mark { get; set; }
        }
        public class CombinedScience
        {
            public string Name { get; set; }
            public double Mark { get; set; }
        }
        public class ICT
        {
            public string Name { get; set; }
            public double Mark { get; set; }
        }
        public class Economics
        {
            public string Name { get; set; }
            public double Mark { get; set; }
        }
        public class English
        {
            public string Name { get; set; }
            public double Mark { get; set; }
        }
        public class EnglishasFirstLanguage
        {
            public string Name { get; set; }
            public double Mark { get; set; }
        }
        public class EnglishasSecondLanguage
        {
            public string Name { get; set; }
            public double Mark { get; set; }
        }
        public class FineArts
        {
            public string Name { get; set; }
            public double Mark { get; set; }
        }
        public class French
        {
            public string Name { get; set; }
            public double Mark { get; set; }
        }
        public class Hindi
        {
            public string Name { get; set; }
            public double Mark { get; set; }
        }
        public class HistoryAndGeography
        {
            public string Name { get; set; }
            public double Mark { get; set; }
        }
        public class Mathematics
        {
            public string Name { get; set; }
            public double Mark { get; set; }
        }
        public class PhysicalEducation
        {
            public string Name { get; set; }
            public double Mark { get; set; }
        }
        public class Physics
        {
            public string Name { get; set; }
            public double Mark { get; set; }
        }
        public class SparkLab
        {
            public string Name { get; set; }
            public double Mark { get; set; }
        }
        public class SportsAndGames
        {
            public string Name { get; set; }
            public double Mark { get; set; }
        }
        public class StemLab
        {
            public string Name { get; set; }
            public double Mark { get; set; }
        }

        public class RptCardShareChartData
        {
            public string Title { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public List<Biology> Biology { get; set; }
            public List<Chemistry> Chemistry { get; set; }
            public List<CombinedScience> CombinedScience { get; set; }
            public List<ICT> ICT { get; set; }
            public List<Economics> Economics { get; set; }
            public List<English> English { get; set; }
            public List<EnglishasFirstLanguage> EnglishasFirstLanguage { get; set; }
            public List<EnglishasSecondLanguage> EnglishasSecondLanguage { get; set; }
            public List<FineArts> FineArts { get; set; }
            public List<French> French { get; set; }
            public List<Hindi> Hindi { get; set; }
            public List<History> History { get; set; }
            public List<HistoryAndGeography> HistoryAndGeography { get; set; }
            public List<Mathematics> Mathematics { get; set; }
            public List<PhysicalEducation> PhysicalEducation { get; set; }
            public List<Physics> Physics { get; set; }
            public List<SparkLab> SparkLab { get; set; }
            public List<SportsAndGames> SportsAndGames { get; set; }
            public List<StemLab> StemLab { get; set; }

            public MemoryStream ChartImageStream(List<string> subLst)
            {
                var chart = new RptCardShareChart(this);
                return chart.GetChartImage(Width, Height, subLst);
            }

            public string ChartImageMap(string name, List<string> subLst)
            {
                var chart = new RptCardShareChart(this);
                return chart.GetChartImageMap(Width, Height, name, subLst);
            }
        }

        public class RptCardShareRepository
        {
            public static RptCardShareChartData GetStudentShares(long? Id, string term)
            {
                /// Variable and object  Declaration
                decimal hWorkAndAttendance = 0;
                Assess360Service a360Obj = new Assess360Service();
                Assess360 objAssess360 = a360Obj.GetAssess360ById(Id ?? 0);

                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Clear();
                criteria.Add("Campus", objAssess360.Campus);
                criteria.Add("Grade", objAssess360.Grade);
                Dictionary<long, IList<SubjectMaster>> subMasterLst = a360Obj.GetSubjectMasterListWithPagingAndCriteria(0, 9999, "SubjectName", "Asc", criteria);
                criteria.Clear();
                criteria.Add("Campus", objAssess360.Campus);
                criteria.Add("Grade", objAssess360.Grade);
                criteria.Add("Sem", term);
                criteria.Add("AcademicYear", objAssess360.AcademicYear);
                Dictionary<long, IList<SemesterMaster>> semesterList = a360Obj.GetSemesterListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
                if (semesterList != null && semesterList.FirstOrDefault().Value.Count > 0 && semesterList.FirstOrDefault().Key > 0)
                {
                    DateTime startDate, endDate;
                    DateTime[] fromtosem = new DateTime[2];
                    startDate = semesterList.FirstOrDefault().Value[0].FromDate;
                    endDate = semesterList.FirstOrDefault().Value[0].ToDate;
                    startDate = new DateTime(startDate.Year, startDate.Month, 1);
                    endDate = new DateTime(endDate.Year, endDate.Month, DateTime.DaysInMonth(endDate.Year, endDate.Month));
                    fromtosem[0] = startDate;
                    fromtosem[1] = endDate;
                    //hWorkAndAttendance = a360Obj.GetAttendenceandpunctualityandHomeworkcompletionMarks(Id ?? 0);
                    //hWorkAndAttendance = hWorkAndAttendance > 15 ? 15 : hWorkAndAttendance;
                    criteria.Clear();
                    criteria.Add("Assess360Id", Id);
                    criteria.Add("IncidentDate", fromtosem);
                    IList<SubjectForCharts> chartObj = new List<SubjectForCharts>();
                    Dictionary<long, IList<Assess360MarkcalculationForChart_Vw>> ChartListVw = a360Obj.GetAssess360MarkcalculationForChart_VwListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
                    if (ChartListVw != null && ChartListVw.FirstOrDefault().Value.Count > 0 && ChartListVw.FirstOrDefault().Key > 0)
                    {
                        string flag = string.Empty;
                        //int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
                        //monthsApart = Math.Abs(monthsApart);
                        int monthsApart = (endDate - startDate).Days/30;
                        for (int i = 0; i < monthsApart; i++)
                        {
                            //added by micheal
                            int montval = 0;
                            if ((startDate.Month + i) > 12)
                                montval = (startDate.Month + i) - 12;
                            else
                                montval = startDate.Month + i;
                            var monthEnteredValue = (from a in ChartListVw.FirstOrDefault().Value
                                                     where (a.MonthRslt == montval)
                                                     select a).ToList();
                            flag = GetMonthInWord(montval);

                            //var monthEnteredValue = (from a in ChartListVw.FirstOrDefault().Value
                            //                         where (a.MonthRslt == startDate.Month + i)
                            //                         select a).ToList();
                            //flag = GetMonthInWord(startDate.Month + i);

                            if (monthEnteredValue.Count > 0)
                            {
                                foreach (var item in subMasterLst.FirstOrDefault().Value)
                                {
                                    decimal CountMarks = 0;
                                    int countVal = 0;
                                    var HwOrEff = monthEnteredValue
                                            //.Where(x => x.Assess360Id == objAssess360.Id && x.Subject == item.SubjectName && x.Semester == term && (x.GroupName == "Homework score" || x.GroupName == "Effort"))
                                            .Where(x => x.Assess360Id == objAssess360.Id && x.Subject == item.SubjectName && (x.GroupName == "Homework score" || x.GroupName == "Effort"))
                                                    .GroupBy(x => new { x.Assess360Id, x.GroupName, x.Subject })
                                                    .Select(g => new SubjectForCharts
                                                    {
                                                        TotalMarks = g.Sum(x => x.CalculatedMarks) / g.Count(),
                                                        SubjectName = g.First().Subject,
                                                        CountVal = g.Count()
                                                    });

                                    var WkOrSkill = monthEnteredValue
                                           //.Where(x => x.Assess360Id == objAssess360.Id && x.Subject == item.SubjectName && x.Semester == term && (x.GroupName == "Weekly Tests" || x.GroupName == "Skill/FA"))
                                            .Where(x => x.Assess360Id == objAssess360.Id && x.Subject == item.SubjectName  && (x.GroupName == "Weekly Tests" || x.GroupName == "Skill/FA"))       
                                           .GroupBy(x => new { x.Assess360Id, x.GroupName, x.Subject })
                                                   .Select(g => new SubjectForCharts
                                                   {
                                                       TotalMarks = g.Sum(x => x.CalculatedMarks) / g.Count(),
                                                       SubjectName = g.First().Subject,
                                                       CountVal = g.Count()
                                                   });
                                    if (HwOrEff.Count() > 0)
                                    {
                                        if (monthEnteredValue.FirstOrDefault().GroupName != "Effort")
                                        {
                                            hWorkAndAttendance = a360Obj.GetAttendenceandpunctualityandHomeworkcompletionMarks(Id ?? 0, "Both");
                                            hWorkAndAttendance = hWorkAndAttendance > 15 ? 15 : hWorkAndAttendance;
                                        }
                                        else
                                        {
                                            hWorkAndAttendance = a360Obj.GetAttendenceandpunctualityandHomeworkcompletionMarks(Id ?? 0, "jnhd");
                                            hWorkAndAttendance = hWorkAndAttendance > 10 ? 10 : hWorkAndAttendance;
                                        }
                                        CountMarks += HwOrEff.FirstOrDefault().TotalMarks + hWorkAndAttendance;
                                        countVal += 1;

                                    }
                                    if (WkOrSkill.Count() > 0)
                                    {
                                        CountMarks += WkOrSkill.FirstOrDefault().TotalMarks;
                                        countVal += 1;
                                    }
                                    else
                                    {
                                        CountMarks += 0;
                                        countVal += 1;
                                    }
                                    SubjectForCharts ChartObj = new SubjectForCharts();
                                    ChartObj.SubjectName = item.SubjectName;
                                    ChartObj.TotalMarks = decimal.Round((CountMarks / countVal), 2);
                                    ChartObj.MonthFlag = flag;
                                    chartObj.Add(ChartObj);
                                }// item for each list
                            }
                            else
                            {
                                foreach (var item in subMasterLst.FirstOrDefault().Value)
                                {
                                    SubjectForCharts subChartObj = new SubjectForCharts();
                                    subChartObj.SubjectName = item.SubjectName;
                                    subChartObj.TotalMarks = 0;
                                    subChartObj.MonthFlag = flag;

                                    if (item.SubjectName == "French" || item.SubjectName == "Hindi")
                                    {
                                        StudentTemplate stud = a360Obj.GetStudentTemplateById(objAssess360.StudentId);
                                        if (stud.SecondLanguage == item.SubjectName)
                                        {
                                            chartObj.Add(subChartObj);
                                        }
                                    }
                                    else
                                    {
                                        chartObj.Add(subChartObj);
                                    }
                                }
                            }
                        }
                    }

                    if (chartObj != null)
                    {
                        var MnthLstCollection = (from u in chartObj
                                                 select new { u.SubjectName, u.MonthFlag }).ToList().Distinct();
                        var chartData = new RptCardShareChartData()
                        {
                            //Title = "Report Card Of Student",
                            Width = 600,
                            Height = 500,
                            Biology = new List<Biology>(),
                            Chemistry = new List<Chemistry>(),
                            CombinedScience = new List<CombinedScience>(),
                            ICT = new List<ICT>(),
                            Economics = new List<Economics>(),
                            English = new List<English>(),
                            EnglishasFirstLanguage = new List<EnglishasFirstLanguage>(),
                            EnglishasSecondLanguage = new List<EnglishasSecondLanguage>(),
                            FineArts = new List<FineArts>(),
                            French = new List<French>(),
                            Hindi = new List<Hindi>(),
                            History = new List<History>(),
                            HistoryAndGeography = new List<HistoryAndGeography>(),
                            Mathematics = new List<Mathematics>(),
                            PhysicalEducation = new List<PhysicalEducation>(),
                            Physics = new List<Physics>(),
                            SparkLab = new List<SparkLab>(),
                            SportsAndGames = new List<SportsAndGames>(),
                            StemLab = new List<StemLab>(),
                        };

                        foreach (var item in chartObj)
                        {
                            switch (item.SubjectName)
                            {
                                case "Biology":
                                    {
                                        chartData.Biology.Add(new Biology()
                                    {
                                        Name = item.MonthFlag,
                                        Mark = (double)item.TotalMarks
                                    });
                                        break;
                                    }
                                case "Chemistry":
                                    {
                                        chartData.Chemistry.Add(new Chemistry()
                                        {
                                            Name = item.MonthFlag,
                                            Mark = (double)item.TotalMarks
                                        });
                                        break;
                                    }
                                case "Combined Science":
                                    {
                                        chartData.CombinedScience.Add(new CombinedScience()
                                        {
                                            Name = item.MonthFlag,
                                            Mark = (double)item.TotalMarks
                                        });
                                        break;
                                    }
                                case "English":
                                    {
                                        chartData.English.Add(new English()
                                        {
                                            Name = item.MonthFlag,
                                            Mark = (double)item.TotalMarks
                                        });
                                        break;
                                    }
                                case "English as First Language":
                                    {
                                        chartData.EnglishasFirstLanguage.Add(new EnglishasFirstLanguage()
                                        {
                                            Name = item.MonthFlag,
                                            Mark = (double)item.TotalMarks
                                        });
                                        break;
                                    }
                                case "English as Second Language":
                                    {
                                        chartData.EnglishasSecondLanguage.Add(new EnglishasSecondLanguage()
                                        {
                                            Name = item.MonthFlag,
                                            Mark = (double)item.TotalMarks
                                        });
                                        break;
                                    }
                                case "ICT":
                                    {
                                        chartData.ICT.Add(new ICT()
                                        {
                                            Name = item.MonthFlag,
                                            Mark = (double)item.TotalMarks
                                        });
                                        break;
                                    }
                                case "Economics":
                                    {
                                        chartData.Economics.Add(new Economics()
                                        {
                                            Name = item.MonthFlag,
                                            Mark = (double)item.TotalMarks
                                        });
                                        break;
                                    }
                                
                                case "Fine Arts":
                                    {
                                        chartData.FineArts.Add(new FineArts()
                                        {
                                            Name = item.MonthFlag,
                                            Mark = (double)item.TotalMarks
                                        });
                                        break;
                                    }
                                case "French":
                                    {
                                        chartData.French.Add(new French()
                                        {
                                            Name = item.MonthFlag,
                                            Mark = (double)item.TotalMarks
                                        });
                                        break;
                                    }
                                case "Hindi":
                                    {
                                        chartData.Hindi.Add(new Hindi()
                                        {
                                            Name = item.MonthFlag,
                                            Mark = (double)item.TotalMarks
                                        });
                                        break;
                                    }
                                case "History & Geography":
                                    {
                                        chartData.HistoryAndGeography.Add(new HistoryAndGeography()
                                          {
                                              Name = item.MonthFlag,
                                              Mark = (double)item.TotalMarks
                                          });
                                        break;
                                    }
                                case "Math":
                                    {
                                        chartData.Mathematics.Add(new Mathematics()
                                        {
                                            Name = item.MonthFlag,
                                            Mark = (double)item.TotalMarks
                                        });
                                        break;
                                    }
                                case "Physical Education":
                                    {
                                        chartData.PhysicalEducation.Add(new PhysicalEducation()
                                        {
                                            Name = item.MonthFlag,
                                            Mark = (double)item.TotalMarks
                                        });
                                        break;
                                    }
                                case "Physics":
                                    {
                                        chartData.Physics.Add(new Physics()
                                        {
                                            Name = item.MonthFlag,
                                            Mark = (double)item.TotalMarks
                                        });
                                        break;
                                    }
                                case "Spark Lab":
                                    {
                                        chartData.SparkLab.Add(new SparkLab()
                                        {
                                            Name = item.MonthFlag,
                                            Mark = (double)item.TotalMarks
                                        });
                                        break;
                                    }
                                case "Sports & Games":
                                    {
                                        chartData.SportsAndGames.Add(new SportsAndGames()
                                        {
                                            Name = item.MonthFlag,
                                            Mark = (double)item.TotalMarks
                                        });
                                        break;
                                    }
                                case "Stem Lab":
                                    {
                                        chartData.StemLab.Add(new StemLab()
                                        {
                                            Name = item.MonthFlag,
                                            Mark = (double)item.TotalMarks
                                        });
                                        break;
                                    }
                            }

                        }
                        return chartData;
                    }
                }
                return null;
            }
        }


        /// <summary>
        /// Get months difference between two dates
        /// </summary>
        /// <param name="from">from date</param>
        /// <param name="to">to date</param>
        /// <returns>months (double) between </returns>
        internal static double GetMonths(DateTime from, DateTime to)
        {
            //change the dates if to is before from
            if (to.Ticks < from.Ticks)
            {
                DateTime temp = from;
                from = to;
                to = temp;
            }
            double percFrom = (double)from.Day / DateTime.DaysInMonth(from.Year, from.Month);
            double percTo = (double)to.Day / DateTime.DaysInMonth(to.Year, to.Month);
            double months = (to.Year * 12 + to.Month) - (from.Year * 12 + from.Month);
            return months - percFrom + percTo;
        }

        internal static string GetMonthInWord(int MonthVal)
        {
            string retVal = string.Empty;

            switch (MonthVal)
            {
                case 1:
                    retVal = "January";
                    break;
                case 2:
                    retVal = "February";
                    break;
                case 3:
                    retVal = "March";
                    break;
                case 4:
                    retVal = "April";
                    break;
                case 5:
                    retVal = "May";
                    break;
                case 6:
                    retVal = "June";
                    break;
                case 7:
                    retVal = "July";
                    break;
                case 8:
                    retVal = "August";
                    break;
                case 9:
                    retVal = "September";
                    break;
                case 10:
                    retVal = "October";
                    break;
                case 11:
                    retVal = "November";
                    break;
                case 12:
                    retVal = "December";
                    break;
            }
            return retVal;
        }

    }

    public class SubjectForCharts
    {
        public virtual string SubjectName { get; set; }
        public virtual int CountVal { get; set; }
        public virtual decimal TotalMarks { get; set; }
        public virtual string MonthFlag { get; set; }
    }
}