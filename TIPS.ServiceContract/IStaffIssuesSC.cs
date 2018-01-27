using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TIPS.Entities.StaffEntities;

namespace TIPS.ServiceContract
{
    public interface IStaffIssuesSC
    {
        long CreateOrUpdateStaffIssues(StaffIssues StaffIssues);
        StaffIssues GetStaffIssuesById(long Id);
    }
}
