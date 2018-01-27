using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.HRManagementEntities
{
    [DataContract]
   public class StaffDetailsHRM
    {

       [DataMember]
       public virtual long Id { get; set; }
       [DataMember]
       public virtual string IdNumber { get; set; }
       [DataMember]
       public virtual string StaffUserName { get; set; }
       [DataMember]
       public virtual string Campus { get; set; }
       [DataMember]
       public virtual string Designation { get; set; }
       [DataMember]
       public virtual string Department { get; set; }
       [DataMember]
       public virtual string DateOfJoin { get; set; }
       [DataMember]
       public virtual string ReportingManager { get; set; }
       [DataMember]
       public virtual string Name { get; set; }
    }
}
