//using OfficeOpenXml;
//using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Web;
using System.Web.Mvc;
//using System.Web.Mail;
using TIPS.Entities;
using TIPS.Entities.StudentsReportEntities;
using TIPS.ServiceContract;
using CMS.Controllers;
using CMS.Helpers;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Net.NetworkInformation;
using TIPS.Service;
using TIPS.Component;




namespace CMS
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "MISWindowsService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select MISWindowsService.svc or MISWindowsService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class MISWindowsService : IMISWindowsService
    {
        StudentsReportBC SRBC = new StudentsReportBC();
        StaffManagementBC SMBC = new StaffManagementBC();
        AdmissionManagementBC ADMNBC = new AdmissionManagementBC();
        #region MISReport
        // StudentsReportBC SRBC = new StudentsReportBC();
        public bool SendEmailFromWindowsService(string Campus)
        {
            bool ret = false;
            ret = SRBC.SendEmailFromWindowsService(Campus);
            return ret;
        }
        #endregion

        #region Birthday Wishes For Staffs
        //StaffManagementBC SMBC = new StaffManagementBC();
        public bool SendBDayWishesMail()
        {
            bool ret;
            ret = SMBC.SendBDayWishes();
            return ret;
        }
        #endregion

        #region Consolidate MIS Report
        public bool SendMISConsolidateReport()
        {
            bool ret = false;
            ret = SRBC.SendMISConsolidateReport();
            return ret;
        }
        #endregion

        #region Admission Report
        public bool SendAdmissionReport()
        {
            bool ret = false;
            ret = SRBC.SendAdmissionReport();
            return ret;
        }
        #endregion

        #region Enquiry Remainder
        public bool SendRemainderMailtoGRL()
        {
            bool ret = false;
            ret = ADMNBC.SendEnquiryRemainderMailtoAdmin();
            return ret;
        }
        #endregion

        public bool SendWeeklyIssueStatusMailService()
        {
            CallManagementBC cbc = new CallManagementBC();
            cbc.SendWeeklyIssueStatusMailService();
            return true;
        }

    }
}

