﻿using System;
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

namespace TIPS.Entities
{
    [DataContract]
    public class CallManagementView
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string IssueNumber { get; set; }
        [DataMember]
        public virtual string Description { get; set; }
        [DataMember]
        public virtual string UserInbox { get; set; }
        [DataMember]
        public virtual string IssueType { get; set; }
        [DataMember]
        public virtual string Status { get; set; }
        [DataMember]
        public virtual string StudentName { get; set; }
        [DataMember]
        public virtual DateTime? IssueDate { get; set; }
        [DataMember]
        public virtual string CallPhoneNumber { get; set; }
        [DataMember]
        public virtual string CallFrom { get; set; }
        [DataMember]
        public virtual string CallerName { get; set; }
        [DataMember]
        public virtual string StudentNumber { get; set; }

        [DataMember]
        public virtual string StudentType { get; set; }

        [DataMember]
        public virtual string School { get; set; }

        [DataMember]
        public virtual string Section { get; set; }

        [DataMember]
        public virtual string IssueGroup { get; set; }

        [DataMember]
        public virtual string Receiver { get; set; }

        [DataMember]
        public virtual string ReceiverGroup { get; set; }

        [DataMember]
        public virtual string Approver { get; set; }

        [DataMember]
        public virtual long InstanceId { get; set; }

        [DataMember]
        public virtual string Email { get; set; }

        [DataMember]
        public virtual string UserRoleName { get; set; }
       
        [DataMember]
        public virtual string Resolution { get; set; }
       
        [DataMember]
        public virtual bool IsInformation { get; set; }

        [DataMember]
        public virtual string Grade { get; set; }

        [DataMember]
        public virtual string InformationFor { get; set; }

        [DataMember]
        public virtual bool IsHosteller { get; set; }

        [DataMember]
        public virtual string BranchCode { get; set; }
        [DataMember]
        public virtual string DeptCode { get; set; }
        [DataMember]
        public virtual bool WaitingForParentCnfm { get; set; }
        [DataMember]
        public virtual bool IsIssueCompleted { get; set; }
        [DataMember]
        public virtual string ActivityFullName { get; set; }
        [DataMember]
        public virtual string LeaveType { get; set; }
        [DataMember]
        public virtual DateTime? ActionDate { get; set; }
        [DataMember]
        public virtual string UserType { get; set; }
        [DataMember]
        public virtual string Performer { get; set; }
    }
}



