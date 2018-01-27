using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CMS
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IMISWindowsService" in both code and config file together.
    [ServiceContract]
    public interface IMISWindowsService
    {
        [OperationContract]
        bool SendEmailFromWindowsService(string campus);
        [OperationContract]
        bool SendBDayWishesMail();
        [OperationContract]
        bool SendMISConsolidateReport();
        [OperationContract]
        bool SendAdmissionReport();
        [OperationContract]
        bool SendRemainderMailtoGRL();
        [OperationContract]
        bool SendWeeklyIssueStatusMailService();
    }
}
