using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.Assess.ReportCardClasses
{
    [DataContract]
    public class ReportCardCBSECo_ScholasticVAndPA
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
        public virtual string Term { get; set; }
        [DataMember]
        public virtual string VA_1 { get; set; }
        [DataMember]
        public virtual string VA_2 { get; set; }
        [DataMember]
        public virtual string VA_3 { get; set; }
        [DataMember]
        public virtual string VA_4 { get; set; }
        [DataMember]
        public virtual string VA_5 { get; set; }
        [DataMember]
        public virtual string VA_6 { get; set; }
        [DataMember]
        public virtual string VA_7 { get; set; }
        [DataMember]
        public virtual string VA_8 { get; set; }
        [DataMember]
        public virtual string VA_9 { get; set; }
        [DataMember]
        public virtual string VA_10 { get; set; }
        [DataMember]
        public virtual decimal? VA_Total { get; set; }
        [DataMember]
        public virtual decimal? VA_Average { get; set; }
        [DataMember]
        public virtual string VA_Grade { get; set; }
        [DataMember]
        public virtual string PA_1 { get; set; }
        [DataMember]
        public virtual string PA_2 { get; set; }
        [DataMember]
        public virtual string PA_3 { get; set; }
        [DataMember]
        public virtual string PA_4 { get; set; }
        [DataMember]
        public virtual string PA_5 { get; set; }
        [DataMember]
        public virtual string PA_6 { get; set; }
        [DataMember]
        public virtual string PA_7 { get; set; }
        [DataMember]
        public virtual string PA_8 { get; set; }
        [DataMember]
        public virtual string PA_9 { get; set; }
        [DataMember]
        public virtual string PA_10 { get; set; }
        [DataMember]
        public virtual decimal? PA_Total { get; set; }
        [DataMember]
        public virtual decimal? PA_Average { get; set; }
        [DataMember]
        public virtual string PA_Grade { get; set; }
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
