using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.Assess.ReportCardClasses
{
    [DataContract]
    public class ReportCardCBSECommon
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
        public virtual string AcademicYear { get; set; }
        [DataMember]
        public virtual long TofWorkingDayT1 { get; set; }
        [DataMember]
        public virtual long TofWorkingDayT2 { get; set; }
        [DataMember]
        public virtual decimal TotalAttendenceT1 { get; set; }
        [DataMember]
        public virtual decimal TotalAttendenceT2 { get; set; }
        [DataMember]
        public virtual decimal HeightT1 { get; set; }
        [DataMember]
        public virtual decimal HeightT2 { get; set; }
        [DataMember]
        public virtual decimal WeightT1 { get; set; }
        [DataMember]
        public virtual decimal WeightT2 { get; set; }

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
