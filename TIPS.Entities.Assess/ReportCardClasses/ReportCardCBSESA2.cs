﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.Assess.ReportCardClasses
{
    [DataContract]
    public class ReportCardCBSESA2
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long RptId { get; set; }
        [DataMember]
        public virtual long RptRequestId { get; set; }
        [DataMember]
        public virtual long PreRegNum { get; set; }
        [DataMember]
        public virtual string NewId { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Grade { get; set; }
        [DataMember]
        public virtual string Section { get; set; }
        [DataMember]
        public virtual string Subject { get; set; }
        [DataMember]
        public virtual string AcademicYear { get; set; }
        [DataMember]
        public virtual decimal SA2 { get; set; }
        [DataMember]
        public virtual string SA2Total { get; set; }
        [DataMember]
        public virtual decimal Term2Total { get; set; }
        [DataMember]
        public virtual string Absent { get; set; }
        /// <summary>
        /// Created details and Modified details
        /// </summary>
        [DataMember]
        public virtual DateTime CreatedDate { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual string ModifiedBy { get; set; }
        [DataMember]
        public virtual DateTime ModifiedDate { get; set; }
    }
}
