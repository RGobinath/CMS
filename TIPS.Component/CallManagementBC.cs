using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PersistenceFactory;
using TIPS.Entities;
using System.Data;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Web;
using TIPS.Entities.StudentsReportEntities;
using System.Data.SqlClient;

namespace TIPS.Component
{
    public class CallManagementBC
    {
        PersistenceServiceFactory PSF = null;
        public CallManagementBC()
        {
            List<String> Assembly = new List<String>();
            Assembly.Add("TIPS.Entities");
            Assembly.Add("TIPS.Entities.Assess");
            Assembly.Add("TIPS.Entities.TicketingSystem");
            Assembly.Add("TIPS.Entities.TaskManagement");
            PSF = new PersistenceServiceFactory(Assembly);
        }

        public long CreateOrUpdateCallManagement(CallManagement cm)
        {
            try
            {
                if (cm != null)
                    PSF.SaveOrUpdate<CallManagement>(cm);
                else { throw new Exception("CallManagement is required and it cannot be null.."); }
                return cm.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public CallManagement GetCallManagementById(long Id)
        {
            try
            {
                CallManagement CallManagement = null;
                if (Id > 0)
                    CallManagement = PSF.Get<CallManagement>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return CallManagement;
            }
            catch (Exception)
            {

                throw;
            }
        }
        //public Dictionary<long, IList<CallManagement>> GetCallManagementListWithPaging(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria, string[] alias)
        public Dictionary<long, IList<CallManagement>> GetCallManagementListWithPaging(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<CallManagement>> retValue = new Dictionary<long, IList<CallManagement>>();
                //return PSF.GetListWithSearchCriteriaCount<CallManagement>(page, pageSize, sortType, sortBy, criteria, alias);
                return PSF.GetListWithSearchCriteriaCount<CallManagement>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<long, IList<BulkCompleteInfo>> GetInformationListWithsearchCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<BulkCompleteInfo>> retValue = new Dictionary<long, IList<BulkCompleteInfo>>();
                return PSF.GetListWithSearchCriteriaCount<BulkCompleteInfo>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<CallManagementChart>> GetCallManagementListWithPagingChart(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<CallManagementChart>> retValue = new Dictionary<long, IList<CallManagementChart>>();
                return PSF.GetListWithEQSearchCriteriaCount<CallManagementChart>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<CallManagementDashboard>> GetCallManagementListWithPagingDashboard(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<CallManagementDashboard>> retValue = new Dictionary<long, IList<CallManagementDashboard>>();
                return PSF.GetListWithSearchCriteriaCount<CallManagementDashboard>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }
        #region Monthly wise Issue count report

        public Dictionary<long, IList<IssueCountReportView>> GetIssueCountListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<IssueCountReportView>> retValue = new Dictionary<long, IList<IssueCountReportView>>();
                return PSF.GetListWithSearchCriteriaCount<IssueCountReportView>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        public DataTable GetStatusWiseIssueCount(string strQry1) //Administrative
        {

            try
            {
                return PSF.ExecuteSqlUsingSQLCommand(strQry1);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public bool SendWeeklyIssueStatusMailService()
        {
            DateTime d = DateTime.Today;
            int offset = d.DayOfWeek - DayOfWeek.Monday;

            DateTime FromDate = d.AddDays(-offset);
            DateTime ToDate = DateTime.Now;

            List<DataRow> IssueCountList = GetIssueCountList(string.Empty, FromDate.ToString(), ToDate.ToString());
            List<DataRow> IssueCountListIssueGroupWise = GetIssueCountListIssueGroupWise(string.Empty, FromDate.ToString(), ToDate.ToString());
            List<DataRow> AllIssuesWithDuration = GetAllIssueWithDuration(string.Empty, FromDate.ToString(), ToDate.ToString());
            // Export to Excel with Multiple Sheets

            DataSet Workbookset = new DataSet("WorkBook");
            DataTable table1 = new DataTable();
            table1.TableName = "Status wise";
            Workbookset.Tables.Add(table1);

            DataTable table2 = new DataTable();
            table2.TableName = "Issue Group wise";
            Workbookset.Tables.Add(table2);

            DataTable table3 = new DataTable();
            table3.TableName = "Show Issues";
            Workbookset.Tables.Add(table3);
            //
            byte[] file = ExportToExcelIssueStatusCount(Workbookset, IssueCountList, IssueCountListIssueGroupWise, AllIssuesWithDuration);
            if (file != null)
                SendMail(file);

            return true;
        }

        public bool SendMail(byte[] file)
        {
            StudentsReportBC srbc = new StudentsReportBC();
            bool retValue = false;
            DateTime Currnt_Date = DateTime.Now;
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Clear();
            criteria.Add("EmailType", "IssueMgmtWeeklyReport");
            Dictionary<long, IList<MISMailMaster>> MISMailMasterDetails = srbc.GetMISMailMasterListWithPaging(null, 9999, string.Empty, string.Empty, criteria);
            if (MISMailMasterDetails != null && MISMailMasterDetails.Count > 0 && MISMailMasterDetails.FirstOrDefault().Value.Count > 0)
            {
                foreach (MISMailMaster item in MISMailMasterDetails.First().Value)
                {
                    criteria.Clear();
                    criteria.Add("EmailRefId", item.Id);
                    Dictionary<long, IList<MISMailStatus>> MailStatusById = srbc.GetMISMailStatusListWithPaging(null, 9999, string.Empty, string.Empty, criteria);
                    MISMailStatus MISMailStatus = srbc.MailSendStatus(item.Id, item.EmailId, item.SentCategory, item.EmailType);
                    if ( MISMailStatus == null || MailStatusById.FirstOrDefault().Value.Count == 0 || (MISMailStatus != null && MISMailStatus.IsSent == false))
                    {
                        if (MISMailStatus == null || MISMailStatus.IsSent == false)
                        {
                            if (MISMailStatus == null)
                            {
                                MISMailStatus = srbc.MISMailStatusFirstInsert(item.Id, item.EmailId, item.Campus, item.SentCategory, item.EmailType);
                            }
                            string CampusVal = item.Campus;
                            bool ret = false;
                            int Currnt_Year = Currnt_Date.Year;

                            string MailBody;
                            MailBody = srbc.GetBodyofMail();
                            if (CampusVal == "All")
                            {
                                ret = srbc.email_send(file, "Issue Mgmt Weekly Report on - " + DateTime.Now.ToString("dd/MM/yyyy") + "", CampusVal, item.EmailId, MailBody, item.EmailType);
                            }
                            if (ret == true)
                            {
                                string TodayDate = DateTime.Now.ToShortDateString();
                                MISMailStatus.IsSent = true;
                                MISMailStatus.CheckDate = TodayDate;
                                MISMailStatus.Campus = item.Campus;
                                MISMailStatus.EmailId = item.EmailId;
                                MISMailStatus.EmailRefId = item.Id;
                                MISMailStatus.SentCategory = item.SentCategory;
                                MISMailStatus.EmailType = item.EmailType;
                                MISMailStatus.SentDate = Currnt_Date;
                                srbc.SaveOrUpdateMISMailStatusDetails(MISMailStatus);
                            }
                        }
                    }
                }
            }
            return retValue;
        }


        public List<DataRow> GetIssueCountList(string Campus, string fdate, string tdate)
        {
            string strQry1 = "";
            strQry1 = strQry1 + " select ROW_NUMBER() OVER (ORDER BY b.BranchCode) AS Id,  b.BranchCode, (b.Completed+b.NonCompleted) Logged, b.Completed, b.NonCompleted  from ";
            strQry1 = strQry1 + " ( ";
            strQry1 = strQry1 + " select a.BranchCode ,SUM(completed) as Completed, SUM(ResolveIssue+ResolveIssueRejection+approveissue+ApproveIssueRejection+Complete) as NonCompleted ";
            strQry1 = strQry1 + " from ( ";
            strQry1 = strQry1 + " select BranchCode, ";
            strQry1 = strQry1 + " case when Status='ResolveIssue'then count(Status) else 0 end as ResolveIssue, ";
            strQry1 = strQry1 + " case when Status='ResolveIssueRejection'then count(Status) else 0 end as ResolveIssueRejection, ";
            strQry1 = strQry1 + " case when Status='ApproveIssue'then count(Status) else 0 end as approveissue, ";
            strQry1 = strQry1 + " case when Status='ApproveIssueRejection'then count(Status) else 0 end as ApproveIssueRejection, ";
            strQry1 = strQry1 + " case when Status='Complete'then count(Status) else 0 end as Complete, ";
            strQry1 = strQry1 + " case when Status='Completed'then count(Status) else 0 end as Completed ";
            strQry1 = strQry1 + " from CallManagement where  BranchCode is not null and IsInformation=0 and Status in ('Approveissue','Completed','Logissue','Resolveissue','ResolveIssueRejection','ApproveIssueRejection') ";

            if (!string.IsNullOrWhiteSpace(Campus) && fdate != null && tdate != null)
            {
                strQry1 = strQry1 + " and BranchCode = '" + Campus + "' ";
                if (fdate != null && tdate != null)
                    strQry1 = strQry1 + " and IssueDate between '" + fdate + "' and '" + tdate + "' ";
            }
            if (!string.IsNullOrWhiteSpace(Campus) && fdate == null && tdate == null)
            {
                strQry1 = strQry1 + " and BranchCode = '" + Campus + "' ";
            }
            if (!string.IsNullOrWhiteSpace(Campus) && fdate == null && tdate != null)
            {
                strQry1 = strQry1 + " and BranchCode = '" + Campus + "' ";
                strQry1 = strQry1 + " and IssueDate <='" + tdate + "' ";
            }
            if (fdate != null && tdate != null)
                strQry1 = strQry1 + " and IssueDate between '" + fdate + "' and '" + tdate + "' ";

            strQry1 = strQry1 + " group by Status,BranchCode) a ";
            strQry1 = strQry1 + " group by a.BranchCode ";
            strQry1 = strQry1 + " )b ";

            DataTable IssueCount = GetStatusWiseIssueCount(strQry1);
            List<DataRow> IssueCountList = null;
            if (IssueCount != null)
            {
                IssueCountList = IssueCount.AsEnumerable().ToList();
            }
            return IssueCountList;
        }

        public List<DataRow> GetIssueCountListIssueGroupWise(string Campus, string fdate, string tdate)
        {
            string strQry1 = "";
            strQry1 = strQry1 + " select ROW_NUMBER() OVER (ORDER BY b.BranchCode) AS Id,  b.BranchCode, b.IssueGroup, (b.Completed+b.NonCompleted) Logged, b.Completed, b.NonCompleted  from ";
            strQry1 = strQry1 + " ( ";
            strQry1 = strQry1 + " select a.BranchCode,a.IssueGroup ,SUM(completed) as Completed, SUM(ResolveIssue+ResolveIssueRejection+approveissue+ApproveIssueRejection+Complete) as NonCompleted ";
            strQry1 = strQry1 + " from ( ";
            strQry1 = strQry1 + " select BranchCode, IssueGroup, ";
            strQry1 = strQry1 + " case when Status='ResolveIssue'then count(Status) else 0 end as ResolveIssue, ";
            strQry1 = strQry1 + " case when Status='ResolveIssueRejection'then count(Status) else 0 end as ResolveIssueRejection, ";
            strQry1 = strQry1 + " case when Status='ApproveIssue'then count(Status) else 0 end as approveissue, ";
            strQry1 = strQry1 + " case when Status='ApproveIssueRejection'then count(Status) else 0 end as ApproveIssueRejection, ";
            strQry1 = strQry1 + " case when Status='Complete'then count(Status) else 0 end as Complete, ";
            strQry1 = strQry1 + " case when Status='Completed'then count(Status) else 0 end as Completed ";
            strQry1 = strQry1 + " from CallManagement where BranchCode is not null and IsInformation=0 and IssueGroup is not null and  Status in ('Approveissue','Completed','Logissue','Resolveissue','ResolveIssueRejection','ApproveIssueRejection') ";

            if (!string.IsNullOrWhiteSpace(Campus) && fdate != null && tdate != null)
            {
                strQry1 = strQry1 + " and BranchCode = '" + Campus + "' ";
                if (fdate != null && tdate != null)
                    strQry1 = strQry1 + " and IssueDate between '" + fdate + "' and '" + tdate + "' ";
            }
            if (!string.IsNullOrWhiteSpace(Campus) && fdate == null && tdate == null)
            {
                strQry1 = strQry1 + " and BranchCode = '" + Campus + "' ";
            }
            if (!string.IsNullOrWhiteSpace(Campus) && fdate == null && tdate != null)
            {
                strQry1 = strQry1 + " and BranchCode = '" + Campus + "' ";
                strQry1 = strQry1 + " and IssueDate <='" + tdate + "' ";
            }
            if (fdate != null && tdate != null)
                strQry1 = strQry1 + " and IssueDate between '" + fdate + "' and '" + tdate + "' ";

            strQry1 = strQry1 + " group by Status,BranchCode,IssueGroup) a ";
            strQry1 = strQry1 + " group by a.BranchCode,a.IssueGroup  ";
            strQry1 = strQry1 + " )b ";
            DataTable IssueCount = GetStatusWiseIssueCount(strQry1);
            List<DataRow> IssueCountList = null;
            if (IssueCount != null)
            {
                IssueCountList = IssueCount.AsEnumerable().ToList();
            }
            return IssueCountList;
        }

        public List<DataRow> GetAllIssueWithDuration(string Campus, string fdate, string tdate)
        {
            string strQry1 = "";

            strQry1 = strQry1 + " select ROW_NUMBER() OVER (ORDER BY BranchCode) AS Id, BranchCode,Grade, Section, IssueGroup, IssueType, Description, IssueDate,UserInbox,Status ";
            strQry1 = strQry1 + " from CallManagement ";
            strQry1 = strQry1 + " where BranchCode is not null and IsInformation=0 and IssueGroup is not null ";

            if (!string.IsNullOrWhiteSpace(Campus) && fdate != null && tdate != null)
            {
                strQry1 = strQry1 + " and BranchCode = '" + Campus + "' ";
                if (fdate != null && tdate != null)
                    strQry1 = strQry1 + " and IssueDate between '" + fdate + "' and '" + tdate + "' ";
            }
            if (!string.IsNullOrWhiteSpace(Campus) && fdate == null && tdate == null)
            {
                strQry1 = strQry1 + " and BranchCode = '" + Campus + "' ";
            }
            if (!string.IsNullOrWhiteSpace(Campus) && fdate == null && tdate != null)
            {
                strQry1 = strQry1 + " and BranchCode = '" + Campus + "' ";
                strQry1 = strQry1 + " and IssueDate <='" + tdate + "' ";
            }
            if (fdate != null && tdate != null)
                strQry1 = strQry1 + " and IssueDate between '" + fdate + "' and '" + tdate + "' ";

            DataTable IssueCount = GetStatusWiseIssueCount(strQry1);
            List<DataRow> IssueCountList = null;
            if (IssueCount != null)
            {
                IssueCountList = IssueCount.AsEnumerable().ToList();
            }
            return IssueCountList;
        }

        public byte[] ExportToExcelIssueStatusCount(DataSet Workbookset, List<DataRow> IssueCountList, List<DataRow> IssueCountListIssueGroupWise, List<DataRow> AllIssuesWithDuration)
        {
            try
            {
                if (IssueCountList.Count > 0 && IssueCountListIssueGroupWise.Count > 0 && AllIssuesWithDuration.Count > 0)
                {
                    using (ExcelPackage pck = new ExcelPackage())
                    {
                        int TableCount = Workbookset.Tables.Count;
                        for (int i = 0; i < TableCount; i++)
                        {
                            if (Workbookset.Tables[i].TableName.ToString() == "Status wise")
                            {
                                ExcelWorksheet ws = pck.Workbook.Worksheets.Add(Workbookset.Tables[i].TableName.ToString());
                                //ws.View.ShowGridLines = false;
                                //ws.Column(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                ws.Row(1).Height = 28.50;
                                //Color Selection
                                System.Drawing.Color WhiteHex = System.Drawing.ColorTranslator.FromHtml("#F2F2F2");
                                System.Drawing.Color GreenHex = System.Drawing.ColorTranslator.FromHtml("#70AD47");
                                List<string> lstHeader = new List<string> { "Id", "Campus", "Logged", "Completed", "NonCompleted" };
                                //Header Section
                                for (int k = 0; k < lstHeader.Count; k++)
                                {
                                    ws.Cells[1, k + 1].Value = lstHeader[k];
                                    ws.Cells[1, k + 1].Style.Font.Bold = true;
                                    ws.Cells[1, k + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    ws.Cells[1, k + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    ws.Cells[1, k + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    ws.Cells[1, k + 1].Style.Fill.BackgroundColor.SetColor(GreenHex);
                                    ws.Cells[1, k + 1].Style.Font.Color.SetColor(System.Drawing.Color.White);
                                    ws.Cells[1, k + 1].AutoFitColumns(15);
                                }
                                int j = 2;
                                for (int P = 0; P < IssueCountList.Count(); P++)
                                {
                                    ws.Cells["A" + j + ""].Value = Convert.ToInt32(IssueCountList[P].ItemArray[0]);
                                    ws.Cells["B" + j + ""].Value = IssueCountList[P].ItemArray[1];
                                    ws.Cells["C" + j + ""].Value = Convert.ToInt32(IssueCountList[P].ItemArray[2]);
                                    ws.Cells["D" + j + ""].Value = Convert.ToInt32(IssueCountList[P].ItemArray[3]);
                                    ws.Cells["E" + j + ""].Value = Convert.ToInt32(IssueCountList[P].ItemArray[4]);
                                    j = j + 1;
                                }
                                int Rowcount = IssueCountList.Count() + 2;
                                int columnCount = lstHeader.Count() + 1;

                                //Borders Matrix Logic
                                for (int l = 1; l < Rowcount; l++)
                                {
                                    for (int m = 1; m < columnCount; m++)
                                    {
                                        ws.Cells[l, m].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        ws.Cells[l, m].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                        ws.Cells[l, m].Style.Border.Bottom.Color.SetColor(GreenHex);
                                        ws.Cells[l, m].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                        ws.Cells[l, m].Style.Border.Left.Color.SetColor(GreenHex);
                                        ws.Cells[l, m].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                        ws.Cells[l, m].Style.Border.Right.Color.SetColor(GreenHex);
                                    }
                                }
                                ws.Cells["A" + 2 + ":E" + IssueCountList.Count() + 1 + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            }
                            if (Workbookset.Tables[i].TableName.ToString() == "Issue Group wise")
                            {
                                ExcelWorksheet ws = pck.Workbook.Worksheets.Add(Workbookset.Tables[i].TableName.ToString());
                                // ws.View.ShowGridLines = false;
                                ws.Row(1).Height = 28.50;
                                //Color Selection
                                System.Drawing.Color WhiteHex = System.Drawing.ColorTranslator.FromHtml("#F2F2F2");
                                System.Drawing.Color GreenHex = System.Drawing.ColorTranslator.FromHtml("#70AD47");
                                List<string> lstHeader = new List<string> { "Id", "Campus", "Issue Group", "Logged", "Completed", "NonCompleted" };
                                //Header Section
                                for (int k = 0; k < lstHeader.Count; k++)
                                {
                                    ws.Cells[1, k + 1].Value = lstHeader[k];
                                    ws.Cells[1, k + 1].Style.Font.Bold = true;
                                    ws.Cells[1, k + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    ws.Cells[1, k + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    ws.Cells[1, k + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    ws.Cells[1, k + 1].Style.Fill.BackgroundColor.SetColor(GreenHex);
                                    ws.Cells[1, k + 1].Style.Font.Color.SetColor(System.Drawing.Color.White);
                                    ws.Cells[1, k + 1].AutoFitColumns(15);
                                }
                                int j = 2;
                                for (int P = 0; P < IssueCountListIssueGroupWise.Count(); P++)
                                {
                                    ws.Cells["A" + j + ""].Value = Convert.ToInt32(IssueCountListIssueGroupWise[P].ItemArray[0]);
                                    ws.Cells["B" + j + ""].Value = IssueCountListIssueGroupWise[P].ItemArray[1];
                                    ws.Cells["C" + j + ""].Value = IssueCountListIssueGroupWise[P].ItemArray[2];
                                    ws.Cells["D" + j + ""].Value = Convert.ToInt32(IssueCountListIssueGroupWise[P].ItemArray[3]);
                                    ws.Cells["E" + j + ""].Value = Convert.ToInt32(IssueCountListIssueGroupWise[P].ItemArray[4]);
                                    ws.Cells["F" + j + ""].Value = Convert.ToInt32(IssueCountListIssueGroupWise[P].ItemArray[5]);
                                    j = j + 1;
                                }
                                int Rowcount = IssueCountListIssueGroupWise.Count() + 2;
                                int columnCount = lstHeader.Count() + 1;

                                //Borders Matrix Logic
                                for (int l = 1; l < Rowcount; l++)
                                {
                                    for (int m = 1; m < columnCount; m++)
                                    {
                                        ws.Cells[l, m].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        ws.Cells[l, m].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                        ws.Cells[l, m].Style.Border.Bottom.Color.SetColor(GreenHex);
                                        ws.Cells[l, m].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                        ws.Cells[l, m].Style.Border.Left.Color.SetColor(GreenHex);
                                        ws.Cells[l, m].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                        ws.Cells[l, m].Style.Border.Right.Color.SetColor(GreenHex);
                                    }
                                }
                                ws.Cells["A" + 2 + ":F" + IssueCountListIssueGroupWise.Count() + 1 + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            }

                            if (Workbookset.Tables[i].TableName.ToString() == "Show Issues")
                            {
                                ExcelWorksheet ws = pck.Workbook.Worksheets.Add(Workbookset.Tables[i].TableName.ToString());
                                // ws.View.ShowGridLines = false;
                                ws.Row(1).Height = 28.50;
                                //Color Selection
                                System.Drawing.Color WhiteHex = System.Drawing.ColorTranslator.FromHtml("#F2F2F2");
                                System.Drawing.Color GreenHex = System.Drawing.ColorTranslator.FromHtml("#70AD47");
                                List<string> lstHeader = new List<string> { "Id", "Campus", "Grade", "Section", "Issue Group", "Issue Type", "Description", "Created Date", "Created By", "Status" };
                                //Header Section
                                for (int k = 0; k < lstHeader.Count; k++)
                                {
                                    ws.Cells[1, k + 1].Value = lstHeader[k];
                                    ws.Cells[1, k + 1].Style.Font.Bold = true;
                                    ws.Cells[1, k + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    ws.Cells[1, k + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    ws.Cells[1, k + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    ws.Cells[1, k + 1].Style.Fill.BackgroundColor.SetColor(GreenHex);
                                    ws.Cells[1, k + 1].Style.Font.Color.SetColor(System.Drawing.Color.White);
                                    ws.Cells[1, k + 1].AutoFitColumns(15);
                                }
                                int j = 2;
                                for (int P = 0; P < AllIssuesWithDuration.Count(); P++)
                                {
                                    ws.Cells["A" + j + ""].Value = Convert.ToInt32(AllIssuesWithDuration[P].ItemArray[0]);
                                    ws.Cells["B" + j + ""].Value = AllIssuesWithDuration[P].ItemArray[1];
                                    ws.Cells["C" + j + ""].Value = AllIssuesWithDuration[P].ItemArray[2];
                                    ws.Cells["D" + j + ""].Value = AllIssuesWithDuration[P].ItemArray[3];
                                    ws.Cells["E" + j + ""].Value = AllIssuesWithDuration[P].ItemArray[4];
                                    ws.Cells["F" + j + ""].Value = AllIssuesWithDuration[P].ItemArray[5];
                                    ws.Cells["G" + j + ""].Value = AllIssuesWithDuration[P].ItemArray[6];
                                    ws.Cells["H" + j + ""].Value = AllIssuesWithDuration[P].ItemArray[7];
                                    ws.Cells["I" + j + ""].Value = AllIssuesWithDuration[P].ItemArray[8];
                                    ws.Cells["J" + j + ""].Value = AllIssuesWithDuration[P].ItemArray[9];
                                    j = j + 1;
                                }
                                int Rowcount = AllIssuesWithDuration.Count() + 2;
                                int columnCount = lstHeader.Count() + 1;

                                //Borders Matrix Logic
                                for (int l = 1; l < Rowcount; l++)
                                {
                                    for (int m = 1; m < columnCount; m++)
                                    {
                                        ws.Cells[l, m].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        ws.Cells[l, m].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                        ws.Cells[l, m].Style.Border.Bottom.Color.SetColor(GreenHex);
                                        ws.Cells[l, m].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                        ws.Cells[l, m].Style.Border.Left.Color.SetColor(GreenHex);
                                        ws.Cells[l, m].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                        ws.Cells[l, m].Style.Border.Right.Color.SetColor(GreenHex);
                                    }
                                }
                                ws.Cells["A" + 2 + ":J" + AllIssuesWithDuration.Count() + 1 + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            }
                        }
                        HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + "Reports" + ".xlsx");
                        byte[] File = pck.GetAsByteArray();
                        HttpContext.Current.Response.BinaryWrite(File);
                        return File;
                    }
                }
                else
                    return null;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Dictionary<long, IList<Activity>> GetCallManagementActivityList(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<Activity>> retValue = new Dictionary<long, IList<Activity>>();
                return PSF.GetListWithSearchCriteriaCount<Activity>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }


        public bool SendMailForSLABreched(byte[] file)
        {
            StudentsReportBC srbc = new StudentsReportBC();
            bool retValue = false;
            DateTime Currnt_Date = DateTime.Now;
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Clear();
            criteria.Add("EmailType", "IssueMgmtWeeklyReport");
            criteria.Add("IsMailNeeded", true);
            Dictionary<long, IList<MISMailMaster>> MISMailMasterDetails = srbc.GetMISMailMasterListWithPaging(null, 9999, string.Empty, string.Empty, criteria);
            if (MISMailMasterDetails != null && MISMailMasterDetails.Count > 0 && MISMailMasterDetails.FirstOrDefault().Value.Count > 0)
            {
                foreach (MISMailMaster item in MISMailMasterDetails.First().Value)
                {
                    criteria.Clear();
                    criteria.Add("EmailRefId", item.Id);
                    Dictionary<long, IList<MISMailStatus>> MailStatusById = srbc.GetMISMailStatusListWithPaging(null, 9999, string.Empty, string.Empty, criteria);
                    MISMailStatus MISMailStatus = srbc.MailSendStatus(item.Id, item.EmailId, item.SentCategory, item.EmailType);
                    if (MISMailStatus == null || MailStatusById.FirstOrDefault().Value.Count == 0 || (MISMailStatus != null && MISMailStatus.IsSent == false))
                    {
                        if (MISMailStatus == null || MISMailStatus.IsSent == false)
                        {
                            if (MISMailStatus == null)
                            {
                                MISMailStatus = srbc.MISMailStatusFirstInsert(item.Id, item.EmailId, item.Campus, item.SentCategory, item.EmailType);
                            }
                            string CampusVal = item.Campus;
                            bool ret = false;
                            int Currnt_Year = Currnt_Date.Year;

                            string MailBody;
                            MailBody = srbc.GetBodyofMail();
                            if (CampusVal == "All")
                            {
                                ret = srbc.email_send(file, "SLA Breached Report on - " + DateTime.Now.ToString("dd/MM/yyyy") + "", CampusVal, item.EmailId, MailBody, item.EmailType);
                            }
                            if (ret == true)
                            {
                                string TodayDate = DateTime.Now.ToShortDateString();
                                MISMailStatus.IsSent = true;
                                MISMailStatus.CheckDate = TodayDate;
                                MISMailStatus.Campus = item.Campus;
                                MISMailStatus.EmailId = item.EmailId;
                                MISMailStatus.EmailRefId = item.Id;
                                MISMailStatus.SentCategory = item.SentCategory;
                                MISMailStatus.EmailType = item.EmailType;
                                MISMailStatus.SentDate = Currnt_Date;
                                srbc.SaveOrUpdateMISMailStatusDetails(MISMailStatus);
                            }
                        }
                    }
                }
            }
            return retValue;
        }

        public bool SendSLABreachedIssueGroupMailService()
        {
            DateTime d = DateTime.Today;
            DateTime FromDate = d.Add(new TimeSpan(-45, 0, 0));
            DateTime ToDate = DateTime.Now;
            //List<DataRow> IssueCountList = GetIssueCountList(string.Empty, FromDate.ToString(), ToDate.ToString());
            List<DataRow> IssueCountListIssueGroupWise = GetIssueGroupWiseCountForSLABreached(string.Empty, FromDate.ToString(), ToDate.ToString());
            List<DataRow> AllIssuesWithDuration = GetIssuesForSLABreached(string.Empty, FromDate.ToString(), ToDate.ToString());
            // Export to Excel with Multiple Sheets

            DataSet Workbookset = new DataSet("WorkBook");
            //DataTable table1 = new DataTable();
            //table1.TableName = "Status wise";
            //Workbookset.Tables.Add(table1);

            DataTable table2 = new DataTable();
            table2.TableName = "Issue Group wise";
            Workbookset.Tables.Add(table2);

            DataTable table3 = new DataTable();
            table3.TableName = "Show Issues";
            Workbookset.Tables.Add(table3);

            //
            byte[] file = ExportToExcelForSLABreached(Workbookset, IssueCountListIssueGroupWise, AllIssuesWithDuration);
            if (file != null)
                SendMailForSLABreched(file);

            return true;
        }

        public List<DataRow> GetIssueGroupWiseCountForSLABreached(string Campus, string fdate, string tdate)
        {
            string strQry1 = "";
            strQry1 = strQry1 + " select Row_number() OVER (ORDER BY a.BranchCode)as id ,a.BranchCode,a.IssueGroup, sum(a.Assigned) as Assigned,sum(a.Available) as Available, sum(ResolveIssue+ApproveIssueRejection) as IssueTobeResolved";
            strQry1 = strQry1 + " from ( select am.BranchCode,ac.Assigned,ac.Available,am.IssueGroup, ";
            strQry1 = strQry1 + " case when Status ='Resolveissue' then 1 else 0 end as ResolveIssue,";
            strQry1 = strQry1 + " case when Status ='ApproveIssueRejection' then 1 else 0 end as ApproveIssueRejection";
            strQry1 = strQry1 + " from CallManagement as am INNER JOIN Activities ac on am.Id=ac.ProcessRefId where am.BranchCode is not null and IsInformation=0 and ";
            strQry1 = strQry1 + " Status in ('Resolveissue','ApproveIssueRejection')";

            if (!string.IsNullOrWhiteSpace(Campus) && fdate != null && tdate != null)
            {
                strQry1 = strQry1 + " and am.BranchCode = '" + Campus + "' ";
            }
            if (!string.IsNullOrWhiteSpace(Campus) && fdate == null && tdate == null)
            {
                strQry1 = strQry1 + " and am.BranchCode = '" + Campus + "' ";
            }
            if (!string.IsNullOrWhiteSpace(Campus) && fdate == null && tdate != null)
            {
                strQry1 = strQry1 + " and am.BranchCode = '" + Campus + "' ";
                strQry1 = strQry1 + " and IssueDate <='" + tdate + "' ";
            }
            if (fdate != null && tdate != null)
                strQry1 = strQry1 + " and IssueDate between '" + fdate + "' and '" + tdate + "' ";

            strQry1 = strQry1 + "and ac.Completed=0 and ac.TemplateId=1) a";
            strQry1 = strQry1 + " group by a.IssueGroup,a.BranchCode";
            DataTable IssueCount = GetStatusWiseIssueCount(strQry1);
            List<DataRow> IssueCountList = null;
            if (IssueCount != null)
            {
                IssueCountList = IssueCount.AsEnumerable().ToList();
            }
            return IssueCountList;
        }

        public List<DataRow> GetIssuesForSLABreached(string Campus, string fdate, string tdate)
        {
            string strQry1 = "";
            strQry1 = strQry1 + "select  Row_number() OVER (ORDER BY am.BranchCode)as id,am.BranchCode,am.Grade,am.Section,am.IssueNumber,ac.Performer, ";
            strQry1 = strQry1 + "am.IssueGroup,am.IssueType,am.Status,am.Description from CallManagement as am INNER JOIN Activities ac on am.Id=ac.ProcessRefId where am.BranchCode is not null and IsInformation=0 and";
            strQry1 = strQry1 + " Status in ('Resolveissue','ApproveIssueRejection') and ac.Completed=0 and ac.TemplateId=1";
            if (!string.IsNullOrWhiteSpace(Campus) && fdate != null && tdate != null)
            {
                strQry1 = strQry1 + " and am.BranchCode = '" + Campus + "' ";
                if (fdate != null && tdate != null)
                    strQry1 = strQry1 + " and am.IssueDate between '" + fdate + "' and '" + tdate + "' ";
            }
            if (!string.IsNullOrWhiteSpace(Campus) && fdate == null && tdate == null)
            {
                strQry1 = strQry1 + " and am.BranchCode = '" + Campus + "' ";
            }
            if (!string.IsNullOrWhiteSpace(Campus) && fdate == null && tdate != null)
            {
                strQry1 = strQry1 + " and am.BranchCode = '" + Campus + "' ";
                strQry1 = strQry1 + " and am.IssueDate <='" + tdate + "' ";
            }
            if (fdate != null && tdate != null)
                strQry1 = strQry1 + " and am.IssueDate between '" + fdate + "' and '" + tdate + "' ";

            DataTable IssueCount = GetStatusWiseIssueCount(strQry1);
            List<DataRow> IssueCountList = null;
            if (IssueCount != null)
            {
                IssueCountList = IssueCount.AsEnumerable().ToList();
            }
            return IssueCountList;
        }

        public byte[] ExportToExcelForSLABreached(DataSet Workbookset, List<DataRow> GetIssueGroupWiseCountForSLABreached, List<DataRow> GetIssuesForSLABreached)
        {
            try
            {
                if (GetIssueGroupWiseCountForSLABreached.Count > 0 && GetIssuesForSLABreached.Count > 0)
                {
                    using (ExcelPackage pck = new ExcelPackage())
                    {
                        int TableCount = Workbookset.Tables.Count;
                        for (int i = 0; i < TableCount; i++)
                        {
                            //if (Workbookset.Tables[i].TableName.ToString() == "Status wise")
                            //{
                            //    ExcelWorksheet ws = pck.Workbook.Worksheets.Add(Workbookset.Tables[i].TableName.ToString());
                            //    //ws.View.ShowGridLines = false;
                            //    //ws.Column(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            //    ws.Row(1).Height = 28.50;
                            //    //Color Selection
                            //    System.Drawing.Color WhiteHex = System.Drawing.ColorTranslator.FromHtml("#F2F2F2");
                            //    System.Drawing.Color GreenHex = System.Drawing.ColorTranslator.FromHtml("#70AD47");
                            //    List<string> lstHeader = new List<string> { "Id", "Campus", "Logged", "Completed", "NonCompleted" };
                            //    //Header Section
                            //    for (int k = 0; k < lstHeader.Count; k++)
                            //    {
                            //        ws.Cells[1, k + 1].Value = lstHeader[k];
                            //        ws.Cells[1, k + 1].Style.Font.Bold = true;
                            //        ws.Cells[1, k + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            //        ws.Cells[1, k + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            //        ws.Cells[1, k + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            //        ws.Cells[1, k + 1].Style.Fill.BackgroundColor.SetColor(GreenHex);
                            //        ws.Cells[1, k + 1].Style.Font.Color.SetColor(System.Drawing.Color.White);
                            //        ws.Cells[1, k + 1].AutoFitColumns(15);
                            //    }
                            //    int j = 2;
                            //    //for (int P = 0; P < IssueCountList.Count(); P++)
                            //    //{
                            //    //    ws.Cells["A" + j + ""].Value = Convert.ToInt32(IssueCountList[P].ItemArray[0]);
                            //    //    ws.Cells["B" + j + ""].Value = IssueCountList[P].ItemArray[1];
                            //    //    ws.Cells["C" + j + ""].Value = Convert.ToInt32(IssueCountList[P].ItemArray[2]);
                            //    //    ws.Cells["D" + j + ""].Value = Convert.ToInt32(IssueCountList[P].ItemArray[3]);
                            //    //    ws.Cells["E" + j + ""].Value = Convert.ToInt32(IssueCountList[P].ItemArray[4]);
                            //    //    j = j + 1;
                            //    //}
                            //    //int Rowcount = IssueCountList.Count() + 2;
                            //    //int columnCount = lstHeader.Count() + 1;

                            //    //Borders Matrix Logic
                            //    for (int l = 1; l < Rowcount; l++)
                            //    {
                            //        for (int m = 1; m < columnCount; m++)
                            //        {
                            //            ws.Cells[l, m].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            //            ws.Cells[l, m].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            //            ws.Cells[l, m].Style.Border.Bottom.Color.SetColor(GreenHex);
                            //            ws.Cells[l, m].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            //            ws.Cells[l, m].Style.Border.Left.Color.SetColor(GreenHex);
                            //            ws.Cells[l, m].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            //            ws.Cells[l, m].Style.Border.Right.Color.SetColor(GreenHex);
                            //        }
                            //    }
                            //   // ws.Cells["A" + 2 + ":E" + IssueCountList.Count() + 1 + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            //}
                            if (Workbookset.Tables[i].TableName.ToString() == "Issue Group wise")
                            {
                                ExcelWorksheet ws = pck.Workbook.Worksheets.Add(Workbookset.Tables[i].TableName.ToString());
                                // ws.View.ShowGridLines = false;
                                ws.Row(1).Height = 28.50;
                                //Color Selection
                                System.Drawing.Color WhiteHex = System.Drawing.ColorTranslator.FromHtml("#F2F2F2");
                                System.Drawing.Color GreenHex = System.Drawing.ColorTranslator.FromHtml("#70AD47");
                                List<string> lstHeader = new List<string> { "Id", "Campus", "Issue Group", "Assigned", "Avilable", "ToBeResolved" };
                                //Header Section
                                for (int k = 0; k < lstHeader.Count; k++)
                                {
                                    ws.Cells[1, k + 1].Value = lstHeader[k];
                                    ws.Cells[1, k + 1].Style.Font.Bold = true;
                                    ws.Cells[1, k + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    ws.Cells[1, k + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    ws.Cells[1, k + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    ws.Cells[1, k + 1].Style.Fill.BackgroundColor.SetColor(GreenHex);
                                    ws.Cells[1, k + 1].Style.Font.Color.SetColor(System.Drawing.Color.White);
                                    ws.Cells[1, k + 1].AutoFitColumns(15);
                                }
                                int j = 2;
                                for (int P = 0; P < GetIssueGroupWiseCountForSLABreached.Count(); P++)
                                {
                                    ws.Cells["A" + j + ""].Value = Convert.ToInt32(GetIssueGroupWiseCountForSLABreached[P].ItemArray[0]);
                                    ws.Cells["B" + j + ""].Value = GetIssueGroupWiseCountForSLABreached[P].ItemArray[1];
                                    ws.Cells["C" + j + ""].Value = GetIssueGroupWiseCountForSLABreached[P].ItemArray[2];
                                    ws.Cells["D" + j + ""].Value = Convert.ToInt32(GetIssueGroupWiseCountForSLABreached[P].ItemArray[3]);
                                    ws.Cells["E" + j + ""].Value = Convert.ToInt32(GetIssueGroupWiseCountForSLABreached[P].ItemArray[4]);
                                    ws.Cells["F" + j + ""].Value = Convert.ToInt32(GetIssueGroupWiseCountForSLABreached[P].ItemArray[5]);
                                    j = j + 1;
                                }
                                int Rowcount = GetIssueGroupWiseCountForSLABreached.Count() + 2;
                                int columnCount = lstHeader.Count() + 1;

                                //Borders Matrix Logic
                                for (int l = 1; l < Rowcount; l++)
                                {
                                    for (int m = 1; m < columnCount; m++)
                                    {
                                        ws.Cells[l, m].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        ws.Cells[l, m].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                        ws.Cells[l, m].Style.Border.Bottom.Color.SetColor(GreenHex);
                                        ws.Cells[l, m].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                        ws.Cells[l, m].Style.Border.Left.Color.SetColor(GreenHex);
                                        ws.Cells[l, m].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                        ws.Cells[l, m].Style.Border.Right.Color.SetColor(GreenHex);
                                    }
                                }
                                ws.Cells["A" + 2 + ":F" + GetIssueGroupWiseCountForSLABreached.Count() + 1 + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            }

                            if (Workbookset.Tables[i].TableName.ToString() == "Show Issues")
                            {
                                ExcelWorksheet ws = pck.Workbook.Worksheets.Add(Workbookset.Tables[i].TableName.ToString());
                                // ws.View.ShowGridLines = false;
                                ws.Row(1).Height = 28.50;
                                //Color Selection
                                System.Drawing.Color WhiteHex = System.Drawing.ColorTranslator.FromHtml("#F2F2F2");
                                System.Drawing.Color GreenHex = System.Drawing.ColorTranslator.FromHtml("#70AD47");
                                List<string> lstHeader = new List<string> { "Id", "Campus", "Grade", "Section", "Issue Number", "Performer", "Issue Group", "Issue Type", "Status", "Description", "Created Date" };
                                //Header Section
                                for (int k = 0; k < lstHeader.Count; k++)
                                {
                                    ws.Cells[1, k + 1].Value = lstHeader[k];
                                    ws.Cells[1, k + 1].Style.Font.Bold = true;
                                    ws.Cells[1, k + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    ws.Cells[1, k + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    ws.Cells[1, k + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    ws.Cells[1, k + 1].Style.Fill.BackgroundColor.SetColor(GreenHex);
                                    ws.Cells[1, k + 1].Style.Font.Color.SetColor(System.Drawing.Color.White);
                                    ws.Cells[1, k + 1].AutoFitColumns(15, 450);
                                }
                                int j = 2;
                                for (int P = 0; P < GetIssuesForSLABreached.Count(); P++)
                                {
                                    ws.Cells["A" + j + ""].Value = Convert.ToInt32(GetIssuesForSLABreached[P].ItemArray[0]);
                                    ws.Cells["B" + j + ""].Value = GetIssuesForSLABreached[P].ItemArray[1];
                                    ws.Cells["C" + j + ""].Value = GetIssuesForSLABreached[P].ItemArray[2];
                                    ws.Cells["D" + j + ""].Value = GetIssuesForSLABreached[P].ItemArray[3];
                                    ws.Cells["E" + j + ""].Value = GetIssuesForSLABreached[P].ItemArray[4];
                                    ws.Cells["F" + j + ""].Value = GetIssuesForSLABreached[P].ItemArray[5];
                                    ws.Cells["G" + j + ""].Value = GetIssuesForSLABreached[P].ItemArray[6];
                                    ws.Cells["H" + j + ""].Value = GetIssuesForSLABreached[P].ItemArray[7];
                                    ws.Cells["I" + j + ""].Value = GetIssuesForSLABreached[P].ItemArray[8];
                                    ws.Cells["J" + j + ""].Value = GetIssuesForSLABreached[P].ItemArray[9];
                                    j = j + 1;
                                }
                                int Rowcount = GetIssuesForSLABreached.Count() + 2;
                                int columnCount = lstHeader.Count() + 1;

                                //Borders Matrix Logic
                                for (int l = 1; l < Rowcount; l++)
                                {
                                    for (int m = 1; m < columnCount; m++)
                                    {
                                        ws.Cells[l, m].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        ws.Cells[l, m].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                        ws.Cells[l, m].Style.Border.Bottom.Color.SetColor(GreenHex);
                                        ws.Cells[l, m].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                        ws.Cells[l, m].Style.Border.Left.Color.SetColor(GreenHex);
                                        ws.Cells[l, m].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                        ws.Cells[l, m].Style.Border.Right.Color.SetColor(GreenHex);
                                    }
                                }
                                ws.Cells["A" + 2 + ":J" + GetIssuesForSLABreached.Count() + 1 + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            }
                        }
                        HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + "Reports" + ".xlsx");
                        byte[] File = pck.GetAsByteArray();
                        HttpContext.Current.Response.BinaryWrite(File);
                        return File;
                    }
                }
                else
                    return null;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #region Added By Prabakaran Call Management
        public Dictionary<long, IList<CallManagementHistory>> GetCallManagementHistoryListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria,Dictionary<string, object> likecriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<CallManagementHistory>(page, pageSize, sortType, sortby, criteria,likecriteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public CallManagement GetCallManagementByIssueNumber(string IssueNumber)
        {
            try
            {
                CallManagement CallManagement = null;
                if (!string.IsNullOrEmpty(IssueNumber))
                    CallManagement = PSF.Get<CallManagement>("IssueNumber", IssueNumber);
                else { throw new Exception("IssueNumber is required and it cannot be 0"); }
                return CallManagement;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool DeleteCallManagementById(long Id)
        {
            try
            {
                CallManagement callmanagement = PSF.Get<CallManagement>(Id);
                PSF.Delete<CallManagement>(callmanagement);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public CallManagementHistory GetCallManagementHistoryByCallManagementId(long Id)
        {
            try
            {
                CallManagementHistory callmanagementhistory = null;
                if (Id > 0)
                    callmanagementhistory = PSF.Get<CallManagementHistory>("Id", Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return callmanagementhistory;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public CallManagementHistory GetCallManagementHistoryById(long Id)
        {
            try
            {
                CallManagementHistory callmanagementhistory = null;
                if (Id > 0)
                    callmanagementhistory = PSF.Get<CallManagementHistory>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return callmanagementhistory;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public long CreateOrUpdateCallManagementHistory(CallManagementHistory cmh)
        {
            try
            {
                if (cmh != null)
                    PSF.SaveOrUpdate<CallManagementHistory>(cmh);
                else { throw new Exception("CallManagement is required and it cannot be null.."); }
                return cmh.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IList<Activity> GetActivityByProcessRefId(long Id)
        {
            try
            {
                IList<Activity> activity = PSF.GetListById<Activity>("InstanceId", Id);
                return activity;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public long CreateOrUpdateActivity(Activity activity)
        {
            try
            {
                if (activity != null)
                    PSF.SaveOrUpdate<Activity>(activity);
                else { throw new Exception("CallManagement is required and it cannot be null.."); }
                return activity.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        #region IssueCountReportByCampus By Prabakaran
        public Dictionary<long, IList<IssueCountReportByCampus_SP>> GetIssueCountReportByCampus_SPList(string Campus, DateTime? FromDate, DateTime? ToDate)
        {
            try
            {
                return PSF.ExecuteStoredProcedurewithOptionalParametersByDictonary<IssueCountReportByCampus_SP>("GetIssueCountReportByCampusList",
                         new[] { new SqlParameter("BranchCode", Campus),                             
                             new SqlParameter("FromDate",FromDate),
                             new SqlParameter("ToDate",ToDate),                             
                    });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region IssueCountReportByIssueGroup By Prabakaran
        public Dictionary<long, IList<IssueCountReportByIssueGroup_SP>> GetIssueCountReportByIssueGroup_SPList(string Campus, string IssueGroup, DateTime? FromDate, DateTime? ToDate)
        {
            try
            {
                return PSF.ExecuteStoredProcedurewithOptionalParametersByDictonary<IssueCountReportByIssueGroup_SP>("GetIssueCountReportByIssueGroupList",
                         new[] { new SqlParameter("BranchCode", Campus),                             
                             new SqlParameter("IssueGroup",IssueGroup),
                             new SqlParameter("FromDate",FromDate),
                             new SqlParameter("ToDate",ToDate),                             
                    });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region PerformerWiseIssueCountReport By Prabakaran
        public Dictionary<long, IList<PerformerWiseIssueCountReport_SP>> GetPerformerWiseIssueCountReport_SPList(string BranchCode,string Performer, DateTime? FromDate, DateTime? ToDate)
        {
            try
            {
                return PSF.ExecuteStoredProcedurewithOptionalParametersByDictonary<PerformerWiseIssueCountReport_SP>("GetPerformerWiseIssueCountReportList",
                         new[] {new SqlParameter("BranchCode", BranchCode),                             
                             new SqlParameter("Performer", Performer),                             
                             new SqlParameter("FromDate",FromDate),
                             new SqlParameter("ToDate",ToDate),                             
                    });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
