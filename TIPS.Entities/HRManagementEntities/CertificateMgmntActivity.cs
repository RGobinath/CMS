﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.HRManagementEntities
{
    [DataContract]
   public partial class CertificateMgmntActivity
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string ActivityName { get; set; }
        [DataMember]
        public virtual string ActivityFullName { get; set; }
        [DataMember]
        public virtual long TemplateId { get; set; }
        [DataMember]
        public virtual string Performer { get; set; }
        [DataMember]
        public virtual bool Assigned { get; set; }
        [DataMember]
        public virtual bool Available { get; set; }
        [DataMember]
        public virtual bool Completed { get; set; }
        [DataMember]
        public virtual string AppRole { get; set; }
        [DataMember]
        public virtual Int32 PreviousActOrder { get; set; }
        [DataMember]
        public virtual Int32 NextActOrder { get; set; }
        [DataMember]
        public virtual bool Waiting { get; set; }
        [DataMember]
        public virtual long WaitingFor { get; set; }
        [DataMember]
        public virtual bool Suspended { get; set; }
        [DataMember]
        public virtual long InstanceId { get; set; }
        [DataMember]
        public virtual Int32 ActivityOrder { get; set; }
        [DataMember]
        public virtual bool IsRejApplicable { get; set; }
        [DataMember]
        public virtual long ProcessRefId { get; set; }

        [DataMember]
        public virtual CertificateRequest CertificateRequest { get; set; }

        [DataMember]
        public virtual DateTime? CreatedDate { get; set; }
        [DataMember]
        public virtual string BranchCode { get; set; }
        [DataMember]
        public virtual string DeptCode { get; set; }


        [DataMember]
        public virtual ProcessInstance ProcessInstance { get; set; }
    }
}

