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
    public class ParentLeaveType
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string FormCode { get; set; }
        [DataMember]
        public virtual string LeaveType { get; set; }
    }
}
