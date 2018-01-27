using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.Assess
{
    [DataContract]
    public class SummativeAssessmentVIVII_vw
    {
        [DataMember]
        public virtual long RptId { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual string IdNo { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Section { get; set; }
        [DataMember]
        public virtual string Grade { get; set; }
        [DataMember]
        public virtual string AcademicYear { get; set; }
        [DataMember]
        public virtual string Semester { get; set; }
        [DataMember]
        public virtual decimal ICT { get; set; }
        [DataMember]
        public virtual decimal English { get; set; }
        [DataMember]
        public virtual decimal HistoryGeography { get; set; }
        [DataMember]
        public virtual decimal Mathematics { get; set; }
        [DataMember]
        public virtual decimal Biology { get; set; }
        [DataMember]
        public virtual decimal Physics { get; set; }
        [DataMember]
        public virtual decimal Chemistry { get; set; }
        [DataMember]
        public virtual decimal Language { get; set; }
        [DataMember]
        public virtual string SeclangDesc { get; set; }
        [DataMember]
        public virtual decimal Total { get; set; }
        [DataMember]
        public virtual string Average { get; set; }
        [DataMember]
        public virtual string TestGrade { get; set; }
        //[DataMember]
        //public virtual string ntytohund {get;set;}
        ///<summary>
        /// For PDF
        ///</summary>
        [DataMember]
        public virtual string TipsLogo { get; set; }
        [DataMember]
        public virtual string TipsNaceLogo { get; set; }
        [DataMember]
        public virtual string TipsName { get; set; }
        [DataMember]
        public virtual string FileName { get; set; }
        [DataMember]
        public virtual string Exam { get; set; }
    }
}
