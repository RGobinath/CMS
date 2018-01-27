using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Runtime.Serialization;
using System.IO;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Hosting;
using System.ComponentModel.DataAnnotations;

namespace TIPS.Entities.ParentPortalEntities
{
    [DataContract]
    public class ParentLeaveRequest
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long LeaveReqNo { get; set; }
        [DataMember]
        public virtual DateTime? LeaveAppliedOn { get; set; }
        [DataMember]
        public virtual string LeaveType { get; set; }
        [DataMember]
        public virtual DateTime? LeaveFrom { get; set; }
        [DataMember]
        public virtual DateTime? LeaveTo { get; set; }
        [DataMember]
        public virtual string Reason { get; set; }
        [DataMember]
        public virtual string Status { get; set; }
        [DataMember]
        public virtual decimal StPreRegNum { get; set; }
        [DataMember]
        public virtual string Performer { get; set; }
        [DataMember]
        public virtual long InstanceId { get; set; }
    }
}
