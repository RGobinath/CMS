using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TIPS.Entities.StaffManagementEntities;

namespace TIPS.ServiceContract
{
    public interface IStaffManagementSC
    {
       long CreateOrUpdateStaffDetails(StaffDetails StaffDetails);
    }
}
