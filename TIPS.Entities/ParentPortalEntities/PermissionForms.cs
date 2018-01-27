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
    public class PermissionForms
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string PermissionNumber { get; set; }
        [DataMember]
        public virtual DateTime? PermissionAppliedOn { get; set; }
        [DataMember]
        public virtual DateTime? PermissionDate { get; set; }
        [DataMember]
        public virtual long PermissionHours { get; set; }
        [DataMember]
        public virtual string Reason { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual string StudentName { get; set; }
        [DataMember]
        public virtual long StPreRegNum { get; set; }
        [DataMember]
        public virtual string StudentNumber { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Grade { get; set; }
        [DataMember]
        public virtual string Section { get; set; }
        [DataMember]
        public virtual string EmailID { get; set; }
        [DataMember]
        public virtual string ContactNo { get; set; }
        [DataMember]
        public virtual string UserType { get; set; }
        [DataMember]
        public virtual string Status { get; set; }
        [DataMember]
        public virtual long InstanceId { get; set; }
        [DataMember]
        public virtual bool IsPermissionCompleted { get; set; }
        [DataMember]
        public virtual string ActivityFullName { get; set; }
    }
}
