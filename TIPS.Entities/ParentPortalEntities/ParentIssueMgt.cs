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
    public class ParentIssueMgt
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string IssueNo { get; set; }
        [DataMember]
        public virtual string Description { get; set; }
        [DataMember]
        public virtual string IssueType { get; set; }
        [DataMember]
        public virtual DateTime? IssueDate { get; set; }
        [DataMember]
        public virtual string Status { get; set; }
        [DataMember]
        public virtual string StudentName { get; set; }
        [DataMember]
        public virtual long StPreRegNum { get; set; }
        [DataMember]
        public virtual string UserInbox { get; set; }
        [DataMember]
        public virtual long InstanceId { get; set; }
        [DataMember]
        public virtual string UserRoleName { get; set; }
        [DataMember]
        public virtual string Approver { get; set; }
        [DataMember]
        public virtual string Resolution { get; set; }
        [DataMember]
        public virtual string Performer { get; set; }
    }
}
